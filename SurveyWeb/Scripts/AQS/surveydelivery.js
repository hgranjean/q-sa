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

        var parentId = $(this).parents('#editNote').map(function () {
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
            SurveyId: surveyId,
            QuestionId: parentId,
            QuestionGroupNumber: questionGroupNumber,
            ResponseId: responseId,
            NoteText: noteText
        }

        alert(JSON.stringify(noteJson));
        
        $.ajax({
            type: 'POST',
            url: $.editNotePath,
            data: noteJson,
            success: function (data) {
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        });
    }

    function requiresNoteClickHandler(event) {

        // $(this).hide();

        // .gn-notes-icon

        // var noteId = $('#NoteId').attr('value');
        var surveyId = $('#SurveyId').attr('value');
        var parentId = $(this).closest("label").attr("data-ng-href");
        var questionGroupId = $(this).closest("label").attr("data-ng-groupid");
        var responseId = $('#ResponseId').attr('value');

        // alert(noteId);
        
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

                // alert(data);

                var editNote = $('#divid' + parentId).html(data);
                //$('.questionEdit').hide();
                //$('.questionDelete').hide();

                $('body').on('click', '.editNote', editNoteClickHandler);
                //editNote.on("click", ".editNote", );
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        });

    }
    $('.requiresNote').click(requiresNoteClickHandler);

    $.fn.serializeObject = function serializeObject() {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };
    
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