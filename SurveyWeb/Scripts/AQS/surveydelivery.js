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

    function editNoteClickHandler(event) {

        var noteId = $(this).parents('#editNote').map(function () {
            return $(this).find('#NoteId').first().val();
        }).get().join(", ");

        var questionId = $(this).parents('#editNote').map(function () {
            return $(this).first().attr("data-ng-href");
        }).get().join(", ");

        var questionGroupNumber = $(this).parents('#editNote').map(function () {
            return $(this).find('#QuestionGroupNumber').first().val();
        }).get().join(", ");

        var surveyId = $(this).parents('#editNote').map(function () {
            return $(this).find('#SurveyId').first().val();
        }).get().join(", ");

        var responseId = $(this).parents('#editNote').map(function () {
            return $(this).find('#ResponseId').first().val();
        }).get().join(", ");

        var noteText = $(this).parents('#editNote').map(function () {
            return $(this).find('#NoteText').first().val();
        }).get().join(", ");

        var noteJson = {
            NoteId: noteId,
            SurveyId: parseInt(surveyId),
            QuestionGroupNumber: parseInt(questionGroupNumber),
            QuestionId: parseInt(questionId),
            ResponseId: responseId,
            NoteText: noteText
        }
        
        $.ajax({
            type: 'POST',
            url: $.editNotePath,
            data: JSON.stringify({ viewModel: noteJson }),
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                $("#divid" + questionId).html("");
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        });
    }

    function cancelNoteClickHandler(event) {

        var questionId = $(this).parents('#editNote').map(function () {
            return $(this).first().attr("data-ng-href");
        }).get().join(", ");

        $("#divid" + questionId).html("");
    }

    function requiresNoteClickHandler(event) {
        
        var surveyId = $('#SurveyId').attr('value');
        var parentId = $(this).closest("label").attr("data-ng-href");
        var questionGroupId = $(this).closest("label").attr("data-ng-groupid");
        var responseId = $('#ResponseId').attr('value');
        
        $.ajax({
            type: 'GET',
            url: $.editNotePath,
            data: {
                surveyId: surveyId,
                questionGroupId: questionGroupId,
                questionId: parentId,
                responseId: responseId
            },
            success: function (data) {
                var editNote = $('#divid' + parentId).html(data);
                
                $('body').on('click', '.editNote', editNoteClickHandler);
                $('body').on('click', '.cancelNote', cancelNoteClickHandler);
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        });

    }
    $('.requiresNote').click(requiresNoteClickHandler);

});