$(document).ready(function () {
    // Context menu/autocomplete code
    function cMenu() {
        var input = $(this).autocomplete({
            delay: 0,
            minLength: 0,
            source: ['Delete this conveyance', 'Print all'
                      , 'Attach a cargo to this conveyance'],
            select: function (event, ui) {
                var val = ui.item.value;
                var self = $(this);
                switch (val) {
                    case 'Delete this conveyance':
                        alert(val);
                        break;
                    case 'Print all':
                        alert(val);
                        break;
                    case 'Attach a cargo to this conveyance':
                        alert(val);
                        break;
                    default:
                        alert(val + ' is not implemented yet');
                }
                return false;
            }
        });

        if (input.autocomplete("widget").is(":visible")) {
            input.autocomplete("close");
            return;
        }
        // Pass empty string as value to search for, displaying all results
        input.autocomplete("search", "");
        input.focus();
    }

    return;
    // Events and ui-styling
    /*$('.c-menu').button({ icons: { primary: 'ui-icon-arrowreturnthick-1-e' } })
                .next()
                .button({ icons: { primary: 'ui-icon-triangle-1-s' }, text: false })
                .click(cMenu)
                .parent()
                .buttonset();*/
});// doc.ready();