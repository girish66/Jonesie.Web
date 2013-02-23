// change some default to get bootsrap working nicely with MVC validation
$(function () {
  jQuery.validator.setDefaults({
    highlight: function (element, errorClass, validClass) {
      if (element.type === 'radio') {
        this.findByName(element.name).addClass(errorClass).removeClass(validClass);
      } else {
        $(element).addClass(errorClass).removeClass(validClass);
        $(element).closest('.control-group').removeClass('success').addClass('error');
      }
    },
    unhighlight: function (element, errorClass, validClass) {
      if (element.type === 'radio') {
        this.findByName(element.name).removeClass(errorClass).addClass(validClass);
      } else {
        $(element).removeClass(errorClass).addClass(validClass);
        $(element).closest('.control-group').removeClass('error').addClass('success');
      }
    }
  });


  $('span.field-validation-valid, span.field-validation-error').each(function () {
    $(this).addClass('help-inline');
  });

  $('form').each(function () {
    $(this).find('div.control-group').each(function () {
      if ($(this).find('span.field-validation-error').length > 0) {
        $(this).addClass('error');
      }
    });
  });

  $('form').submit(function () {
    if ($(this).valid()) {
      $(this).find('div.control-group').each(function () {
        if ($(this).find('span.field-validation-error').length == 0) {
          $(this).removeClass('error');
        }
      });
    }
    else {
      $(this).find('div.control-group').each(function () {
        if ($(this).find('span.field-validation-error').length > 0) {
          $(this).addClass('error');
        }
      });
    }
  });

});
