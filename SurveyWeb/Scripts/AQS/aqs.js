
    $(function() {

        var ajaxformSubmit = function () {
            var $form = $(this);
            //alert('was here!');
            var options = {
                url: $form.attr("action"),
                type: $form.attr("method"),
                data: $form.serialize()
            };

            $.ajax(options).done(function (data) {
                var $target = $($form.attr("data-aqs-target"));
                //alert('ajax done!')
                //alert($target.val);
                $target.replaceWith(data);
            });
            //alert('ajax done!')
            return false;
        };


        //jquery selector
        //$("form[data-aqs-ajax='true']").submit(ajaxformSubmit);

        $('textarea[data-aqs-ajax="true"]').focusout(ajaxformSubmit);


        //Dropdown List on chang  e
        var ajaxGetForChangedValue = function () {
            var selectedValue = $(this).val();
            //alert(selectedValue);

            //TODO: Get/Set from dom element
            var url = '/Learning/NBTrainingDocument/?trainingDocumetId=' + selectedValue;


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


        //$('select[data-aqs-ajax="true"]').change(alert('selectlistchanged'));

        //$('#observationText').blur(ajaxformSubmit);

        
        //$('#observationText').focusout(function () {
        //    alert('lostFocus');
        //    $(this).closest('form').submit();
        //});


        //$('#ClassList').change(function () {
        //    var classKey = $(this).val();
        //    alert(classKey);
        //    //$('#classText').val('Herve');

        //    $.get('@Url.Action("NBTrainingDocument","Learning")', { trainingDocumetId: classKey}, function (data) {
        //        $('#classText').val(data);               
        //    });
        //});


    });


