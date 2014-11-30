$(document).ready(function () {

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

    function questionEditClickHandler() {

        $(this).hide();

        var surveyId = $('#Survey_Id').attr('value');
        var parentId = $(this).attr("data-ng-href");
            
        $.ajax({
            type: 'GET',
            url: $.editQuestionPath,
            data: {
                surveyId: surveyId,
                questionId: parentId,
            },
            success: function (data) {
                $('#divid' + parentId).html(data);
                $('.questionEdit').hide();
                $('.questionDelete').hide();
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        });

    }
    $('.questionEdit').click(questionEditClickHandler);

    function questionDeleteClickHandler() {

        var surveyId = $('#Survey_Id').attr('value');
        var parentId = $(this).parents('li').map(function () {
            return $(this).find('a').first().attr("data-ng-href");
        }).get().join(", ");
        var parentGroupId = $(this).closest('li').closest('ul').attr("data-ng-href");

        $.confirm(
            "Delete question", //title
            "Delete the question " + parentId + "?", //message
            "Delete", //button text
            function deleteOk() { //"yes" callback
                
                var liToDelete = $(this).closest('li');
                
                $.ajax({
                    type: 'POST',
                    url: $.deleteQuestionPath,
                    data: {
                        surveyId: surveyId,
                        questionGroupId: parentGroupId,
                        questionId: parentId
                    },
                    success: function (data) {
                        liToDelete.remove();
                        location.reload();
                    },
                    error: function (xhr, status, error) {
                        console.log(xhr.responseText);
                    }
                });
            }
        );
    }

    $('.questionDelete').click(questionDeleteClickHandler);

    function questionAddClickHandler() {
        
        var surveyId = $('#Survey_Id').attr('value');
        var parentGroupId = $(this).attr("data-ng-href");
        
        $.ajax({
            type: 'POST',
            url: $.addQuestionPath,
            dataType: 'html',
            data: {
                surveyId: surveyId,
                questionGroupId: parentGroupId,
            },
            success: function (questionData) {

                var newQuestion = $("#questionGroup" + parentGroupId).append(
                    '<li class="questionItem">' +
                    questionData +
                    '</li>');

                newQuestion.on("click", ".questionEdit", questionEditClickHandler);
                newQuestion.on("click", ".questionDelete", questionDeleteClickHandler);

                // $("#questionGroup" + parentGroupId).children("li").last().find(".questionEdit").trigger("click");
                window.location.href = $.surveyDesignPath + "/" + surveyId;
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        });
    }

    $('.questionAdd').click(questionAddClickHandler);

    function surveyEditHeaderClickHandler() {

        $.ajax({
            type: 'POST',
            url: $.surveyEditHeaderPath,
            data: $('#surveyHeader').serialize(),
            success: function (data) {
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        });
    }

    $('.surveyEditHeader').click(surveyEditHeaderClickHandler);
});