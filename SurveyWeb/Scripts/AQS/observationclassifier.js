$(document).ready(function() {

    var ajaxformSubmit = function () {

        event.preventDefault()

        var form = $("#observationForm");
        var textarea = $(this);
        var options = {
            url: form.attr("action"),
            type: form.attr("method"),
            data: { observationText: textarea.val() }
        };

        $(document.body).off('change', '#ClassList', ajaxGetForChangedValue);
        
        $.ajax(options).done(function (data) {
            
            var target = $(textarea.attr("data-aqs-target"));

            target.replaceWith(data);

            $(document.body).on('change', '#ClassList', ajaxGetForChangedValue);
        });
        
        return false;
    };
        
    $('textarea[data-aqs-ajax="true"]').focusout(ajaxformSubmit);


    //Dropdown List on change
    var ajaxGetForChangedValue = function () {
        var selectedValue = $(this).val();

        var url = $.nbTrainingDocumentPath + '/?trainingDocumetId=' + selectedValue;
            
        $.ajax({
            url: url,
            data: { trainingDocumetId: selectedValue }, //parameters go here in object literal form
            type: 'GET',
            datatype: 'html',
            success: function (data) {
                //var $target = $($(this).attr("data-aqs-target"));
                //alert($target.attr("name").val());
                $('#documentText').replaceWith(data);
            },
            error: function () { alert('something bad happened'); }
        });


        return false;

    };
    //end dropdown list change

    $(document.body).on('change', '#ClassList', ajaxGetForChangedValue);
});


