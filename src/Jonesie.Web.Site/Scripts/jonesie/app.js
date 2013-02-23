(function () {
  'use strict';

  var namespace = function (name, value) {
    var ns = name.split('.'),
        n = ns.length,
        obj = window,
        i;

    for (i = 0; i < n; i += 1) {
      if (i === n - 1) {
        obj = obj[ns[i]] = obj[ns[i]] || value || {};
      } else {
        obj = obj[ns[i]] = obj[ns[i]] || {};
      }
    }

    return obj;
  };

  var b = namespace("Jonesie");
  b.ns = namespace;

  // global startup code we need before all other stuff
  b.globals = {
    baseUrl: '',      // these get set in the _layouts pages since ASP.Net does not have an easy way to inject into .js files
    hub: undefined,
    sessionId: '',
    userName: '',
    showAlert: function (alertType, message) { }
  };

  b.createForm = function (entity, heading, mode, controller, $grid, id, height, width) {

    var $modal;
    var title = mode + ' ' + heading;
    var dlgid = '#' + entity + '_DIALOG';
    //var dlgid = '#DIALOGCONTAINER';
    var $dlgContainer = $(dlgid);

    var cheight = '', cwidth = '';

    if (height)
      cheight = "data-height:'" + height + "px'";
    if (width)
      cwidth = "data-width:'" + width + "px'";

    $dlgContainer.empty();
    var outerdiv = $("<div id='" + entity + "Modal' class='modal hide fade' tabindex='-1' data-focus-on='input:first' " + cheight + " " + cwidth + " aria-labelledby=" + entity + "ModalHeading' data-backdrop='static' ></div>");
    var headerdiv = $('<div class="modal-header"></div>');
    headerdiv.append($('<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>'));
    headerdiv.append($('<h3 id="' + entity + 'ModalHeading"></h3>'));

    var bodydiv = $('<div class="modal-body" id="' + entity + '_DIALOG_BODY"></div>');

    var footerdiv = $('<div class="modal-footer"></div>');
    footerdiv.append($('<span class="pull-left" id="errorMsg"></span>'));
    footerdiv.append($('<a href="#" class="btn" data-dismiss="modal">Cancel</a>'));
    footerdiv.append($('<a href="#" class="btn btn-primary" id="save' + entity + '">Save</a>'));

    outerdiv.append(headerdiv);
    outerdiv.append(bodydiv);
    outerdiv.append(footerdiv);

    $dlgContainer.append(outerdiv);

    var url = Jonesie.globals.baseUrl + controller;
    if (mode == 'New') {
      url += '/New' + entity;
    } else if (mode == 'Edit') {
      url += ('/Edit' + entity + '/' + id);
    } else if (mode == 'Delete') {
      url += ('/Delete' + entity + '/' + id);
      $("#save" + entity).text("Delete");
    } else {
      url += ('/' + mode + entity);
      if (id)
        url += ('/' + id);
    }

    $("#save" + entity).on("click", function (event) {
      event.preventDefault();

      var $form = $('#' + entity + 'Modal form');
      $.validator.unobtrusive.parse($form);
      $form.ajaxForm(
        {
          dataType: 'json',
          beforeSubmit : function(formdata, form, options) {
            return $form.valid();
          },
          success: function (data) {
            // hide the dialog and refresh the grid
            //$("#" + entity + "Modal").modal('hide');
            $modal.modal('hide');
            if ($grid)
              $grid.load();
          },
          error: function (response, status, err) {
            var data = eval('[' + response.responseText + ']');
            var msg = 'Unknown Error';
            if (data && data.length > 0) {
              msg = data[0].Message;
            }
            $('#errorMsg').text(err + " :: " + msg);
          }
        });

      $form.submit();
    });
    $("#" + entity + "ModalHeading").html(title);
    $modal = $("#" + entity + "Modal").modal({ remote: url, show: true, attentionAnimation:'shake' });    
  };

  // start the app
  b.startUp = function () {
    // startup the signalr hub and get the proxies we need
    if ($.connection && $.connection.jonesie) {
      b.globals.hub = $.connection.jonesie;

      b.globals.hub.client.showalert = b.globals.showAlert;

      $.connection.hub.start().done(function () {
        b.globals.hub.server.registerUser(b.globals.userName);
      });

      //$.Jonesie.Globals.hub.dataChangeNotification = function (data) {
      //    if (data[0] != $.Jonesie.Globals.sessionId) {
      //        $.publish('dataChange' + data[1], [data]);
      //    }
      //};
    }
  };
  window.Jonesie = b;
}());