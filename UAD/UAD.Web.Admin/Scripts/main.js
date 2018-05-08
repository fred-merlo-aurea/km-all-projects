$.ajaxSetup({ cache: true });

$(document).ajaxStart(function () {
    showLoadingWindow();
});

$(document).ajaxStop(function () {
    closeLoadingWindow();
});

$(document).ajaxComplete(function (event, request, settings) {
    var redirect = request.getResponseHeader('REDIRECT');
    if (redirect) {
        location = redirect;
        return;
    }
});

//Enents -Page number input changed.
$("#pagerInputPageNumber").on("change", function (e) {
    RebindOnPageOrSizeChange()
});

//Events - Page Size input changed
$("#ddPageSize").on("change", function (e) {
    RebindOnPageOrSizeChange()
});
