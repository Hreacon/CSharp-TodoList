$(document).ready(function() {
  $(".task input").each(function() {
    $(this).on('change', function() {
      var taskId = $(this).val();
      var checked = "check";
      if($(this).checked)
        checked = "uncheck";
      window.location.href="/"+checked+"/" + taskId;
    });
  });
});