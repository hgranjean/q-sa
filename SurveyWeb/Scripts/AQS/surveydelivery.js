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

    function requiresNoteClickHandler(event) {

        // $(this).hide();

        // .gn-notes-icon

        var surveyId = $('#SurveyId').attr('value');
        var parentId = $(this).closest("label").attr("data-ng-href");
        var questionGroupId = $(this).closest("label").attr("data-ng-groupid");
        
        $.ajax({
            type: 'GET',
            url: $.editNotePath,
            data: {
                surveyId: surveyId,
                questionGroupId: questionGroupId, // TODO: Add questiongroupId
                questionId: parentId,
            },
            success: function (data) {

                // alert(data);

                var editNote = $('#divid' + parentId).html(data);
                //$('.questionEdit').hide();
                //$('.questionDelete').hide();

                editNote.on("click", ".editNote", editNoteClickHandler);
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        });

    }
    $('.requiresNote').click(requiresNoteClickHandler);

    function editNoteClickHandler() {
        alert("Edited: " + $('#editNote').outerHTML);

        $.ajax({
            type: 'POST',
            url: $.editNotePath,
            data: $('#editNote').serialize(),
            success: function (data) {
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        });
    }
    
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

    $('.questionAddzzz').click(questionAddClickHandler);

});