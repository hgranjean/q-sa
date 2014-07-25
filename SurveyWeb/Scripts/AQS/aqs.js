
    $(function() {

        //var updateList = function () {
        //    $.getjson('/Learning/ObservationClassifier', function (result) {
        //        var ddl = $('#EPIds');
        //        ddl.empty();
        //        $(result).each(function () {
        //            $(document.createelement('option'))
        //                .attr('value', this.id)
        //                .text(this.value)
        //                .appendto(ddl);
        //        });
        //    });
        //}

        $('#observationText').focusout(function () {
            $(this).closest('form').submit();
        });
    });


