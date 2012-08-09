$(document).ready(function () {
    $("#dialog").dialog({
        modal: true,
        bgiframe: true,
        width: 500,
        height: 200,
        autoOpen: false
    });

    $(".confirmDelete").click(function (e) {
        e.preventDefault();
        var deleteUrl = this.href;
        $("#dialog").dialog('option', 'buttons', {
            "Confirm": function () {
                $.post(deleteUrl, function (data) {
                    window.location.reload();
                });
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        });

        $("#dialog").dialog("open");

    });
});