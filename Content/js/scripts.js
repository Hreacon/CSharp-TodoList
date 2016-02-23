$(document).ready(function() {
  $(".task input").each(function() {
    $(this).on('change', function() {
      var taskId = $(this).val();
      var checked = "uncheck";
      var selector = "#"+$(this).attr('id') + ":checked";
      if($(selector).length > 0)
        checked = "check";
      // alert(checked);
      window.location.href="/"+checked+"/" + taskId;
    });
  });
});