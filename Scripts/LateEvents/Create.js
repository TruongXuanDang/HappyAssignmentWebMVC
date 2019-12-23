$(document).ready(function () {
    $("#lateType").change(ajaxCalculatePunishment);
    $("#studentId").change(ajaxCalculatePunishment);

    function ajaxCalculatePunishment() {
        $.ajax({
            url: '/LateEvents/CalculatePunishment',
            type: 'POST',
            data: $("form").serialize(),
            success: function (response) {
                if ($("#lateType").val() == 1) {
                    $("#lateMoney").val(response);
                    $("#pushCount").val(0);
                }
                else if ($("#lateType").val() == 2) {
                    $("#pushCount").val(response);
                    $("#lateMoney").val(0);
                }
                else {
                    $("#pushCount").val(0);
                    $("#lateMoney").val(0);
                }
            }
        });
    }

});