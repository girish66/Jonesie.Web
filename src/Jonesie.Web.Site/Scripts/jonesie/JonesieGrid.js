Jonesie.ns('Jonesie.basicgrid.Model');
Jonesie.ns('Jonesie.basicgrid.PagedCollection');
Jonesie.ns('Jonesie.basicgrid.Collection');
Jonesie.ns('Jonesie.basicgrid.Grid');
Jonesie.ns('Jonesie.basicgrid.QuickGrid');

(function ($, backbone, b) {
 
  var model = Jonesie.basicgrid.Model = backbone.Model.extend({

  });

  var pagedCollection = Jonesie.basicgrid.PagedCollection = backbone.Paginator.requestPager.extend({
    model: model,

    initialize: function (config) {
      if (config) {
        if (config.pageSize) this.pageSize = config.pageSize;
        if (config.url) this.url = config.url;
      }
    },

    paginator_core: {
      type: 'POST',
      dataType: 'json',
      url: function () { return this.url; }
    },

    paginator_ui: {
      firstPage: 1,
      currentPage: 1,
      perPage: 10,
      totalPages: 10,
      sortColumn: '',
      totalMatching: 0,
      orderDescending: false,
      filter: ""
    },

    server_api: {
      'page': function () { return this.currentPage; },
      'pageSize': function () { return this.pageSize; },
      'sortColumn': function () { return this.sortColumn; },
      'orderDescending': function () { return this.orderDescending; },
      'filter': function () { return this.filter; }
    },

    goToNextPage: function () {
      this.currentPage++;
      if (this.currentPage > this.totalPages) {
        this.currentPage = this.totalPages;
      }
      this.pager();
    },

    goToPreviousPage: function () {
      this.currentPage--;
      if (this.currentPage < 1) {
        this.currentPage = 1;
      }
      this.pager();
    },

    goToPage: function (pageNo) {
      this.currentPage = pageNo;
      this.pager();
    },

    goToStart: function () {
      this.currentPage = 1;
      this.pager();
    },

    goToEnd: function () {
      this.currentPage = this.totalPages;
      this.pager();
    },

    getPages: function () {
      var startPage = parseInt(this.currentPage) - 5;
      if (startPage < 1) startPage = 1;

      var endPage = startPage + 5;
      if (endPage > this.totalPages) endPage = this.totalPages;

      var pages = [];

      if (this.totalPages > 1) {
        for (var i = startPage; i <= endPage; i++) {
          pages.push({
            'label': i,
            'id': 'page_goto_' + i,
            'active' : ((i == this.currentPage) ? 'active' :'')
          });
        }
      }

      return { Pages: pages };
    },

    sort: function (columnName) {
      if (this.sortColumn != columnName) {
        this.sortColumn = columnName;
        this.orderDescending = true;
      } else {
        this.orderDescending = !this.orderDescending;
      }
      this.currentPage = 1;
      this.pager();
    },

    parse: function (response) {

      this.totalPages = response.TotalPages;
      this.pageSize = response.PageSize;
      //this.currentPage = response.Page;
      this.totalMatching = response.TotalMatching;
      //this.orderDescending = response.OrderDescending;

      return response;
    }

  });

  var gridView = Jonesie.basicgrid.Grid = backbone.View.extend({
    row_template: '#row_template',
    grid_template: '#grid_container_template',
    page_template: '#paging_controls_template',
    row_templateC: '',
    page_templateC: '',
    grid_templateC: '',
    userRowSelected: null,
    userGridRender: null,
    sort_column: {},

    initialize: function () {

      if (this.options.extraconfig.row_template)
        this.row_template = this.options.extraconfig.row_template;

      if (this.options.extraconfig.grid_template)
        this.grid_template = this.options.extraconfig.grid_template;

      if (this.options.extraconfig.page_template)
        this.page_template = this.options.extraconfig.page_template;

      if (this.options.extraconfig.gridRender)
        this.userGridRender = this.options.extraconfig.gridRender;

      if (this.options.extraconfig.rowSelected)
        this.userRowSelected = this.options.extraconfig.rowSelected;

      this.row_templateC = Handlebars.compile($(this.row_template).html());
      this.page_templateC = Handlebars.compile($(this.page_template).html());
      this.grid_templateC = Handlebars.compile($(this.grid_template).html());

      this.model.bind("all", this.render, this);

      Handlebars.registerHelper('utcDate', function (jsonDate) {
        if (jsonDate) {
          return moment(jsonDate);
        }

        return '';
      });

      Handlebars.registerHelper('utcDateShort', function (jsonDate) {
        if (jsonDate) {
          return moment(jsonDate).format('YYYY-MM-DD HH:mm Z');
        }

        return '';
      });

    },

    events: {
      "click #page_next": "onPageNextClicked",
      "click #page_prev": "onPagePreviousClicked",
      "click .page-goto": "onPageGoToClicked",
      "click #page_start": "onPageGoToStart",
      "click #page_end": "onPageGoToEnd",
      "click .sortable-column": "onSortingColumnClicked",
      "click tr": "onRowSelected"
    },

    render: function () {
      var me = this;

      var columns = _.map(this.options.columns, function (obj) {
        return {
          Label: obj.Label,
          ColumnName: obj.ColumnName || "",
          SortableCss: me.getSortCss(obj.Sortable, obj.ColumnName)
        };
      });

      var rows = this.row_templateC(this.model.toJSON()[0]);
      var pages = this.page_templateC(this.model.getPages());
      var grid = this.grid_templateC({
        Rows: rows,
        Columns: columns,
        Paging: pages
      });

      this.$el.html(grid);

      if (this.userGridRender)
        this.userGridRender();

      return this;
    },

    getSortCss: function (sortable, columnName) {
      if (!sortable) return "";

      if (columnName == this.model.sortColumn) {
        return this.model.orderDescending ? 'sortable-column sorted sorted-desc' : 'sortable-column sorted sorted-asc';
      }
      return 'sortable-column';
    },

    onPageNextClicked: function (event) {
      event.preventDefault();
      this.model.goToNextPage();
    },

    onPagePreviousClicked: function (event) {
      event.preventDefault();
      this.model.goToPreviousPage();
    },

    onPageGoToClicked: function (event) {
      event.preventDefault();
      var clickedEl = event.target;
      var pageNo = clickedEl.id.replace('page_goto_', '');
      this.model.goToPage(pageNo);
    },

    onSortingColumnClicked: function (event) {
      event.preventDefault();
      var columnName = event.target.getAttribute('data-column-name');
      if (columnName) {
        this.model.sort(columnName);
      }
    },

    onPageGoToStart: function (event) {
      event.preventDefault();
      this.model.goToStart();
    },

    onPageGoToEnd: function (event) {
      event.preventDefault();
      this.model.goToEnd();
    },

    onRowSelected: function (event) {
      event.preventDefault();
      $(event.currentTarget).addClass('info').siblings().removeClass('info');

      if (this.userRowSelected) {
        this.userRowSelected($(event.currentTarget).attr('data-id'));
      }
    },

    load: function () {
      this.model.fetch();
    }

  });

  var quickGrid = Jonesie.basicgrid.QuickGrid = function (config) {
    var collection = new Jonesie.basicgrid.PagedCollection({
      url: config.url,
      pageSize: config.pageSize
    });

    var g = new Jonesie.basicgrid.Grid({
      el: config.el,
      model: collection,
      columns: config.columns,
      extraconfig: config
    });

    //collection.fetch();

    return g;
  };

}(jQuery, Backbone, Jonesie));

