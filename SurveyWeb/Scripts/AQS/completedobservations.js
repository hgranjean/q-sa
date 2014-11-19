$(document).ready(function () {
    $(".actionmenu").dropit();

    $.extend({
        confirm: function (title, message, yesText, yesCallback) {
            $("<div></div>").dialog({
                buttons: [{
                    text: yesText,
                    click: function () {
                        yesCallback();
                        $(this).remove();
                    }
                },
                {
                    text: "Cancel",
                    click: function () {
                        $(this).remove();
                    }
                }
                ],
                close: function (event, ui) { $(this).remove(); },
                resizable: false,
                title: title,
                modal: true
            }).text(message).parent().addClass("alert");
        }
    });

    function questionDeleteClickHandler() {

        var parentId = $(this).parents('li').map(function () {
            return $(this).find('a').first().attr("data-ng-href");
        }).get().join(", ");

        var questionText = $(this).parents('li').map(function () {
            return $(this).find('a').first().attr("data-aqs-text");
        }).get().join(", ");

        $.confirm(
            "Delete response", //title
            "Delete the response #" + questionText + "?", //message
            "Delete", //button text
            function deleteOk() { //"yes" callback

                $.ajax({
                    type: 'POST',
                    url: $.deleteObservationPath,
                    data: {
                        responseId: parentId
                    },
                    success: function (data) {
                        location.reload();
                    },
                    error: function (xhr, status, error) {
                        console.log(xhr.responseText);
                    }
                });
            }
        );
    }

    $('.responseDelete').click(questionDeleteClickHandler);
});