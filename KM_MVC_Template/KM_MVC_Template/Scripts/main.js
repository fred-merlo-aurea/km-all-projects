$.ajaxSetup({ cache: true });

$(document).ajaxStart(function () {
    $.blockUI({
        message: 'Please wait...',
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        }
    });
});

$(document).ajaxStop(function () {
    $.unblockUI();
});

$(document).ajaxComplete(function (event, request, settings) {
    var redirect = request.getResponseHeader('REDIRECT');
    if (redirect) {
        location = redirect;
        return;
    }
});