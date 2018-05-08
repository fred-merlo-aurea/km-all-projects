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