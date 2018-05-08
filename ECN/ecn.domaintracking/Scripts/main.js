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

function setValidationServer(messageArray, divMsg) {
    var blockDiv = $("<div/>").css({ "display": "block", "width": "600px" });
    if (divMsg == undefined) divMsg = '.breadcrumb';
    blockDiv.insertAfter($(divMsg));
    var divToInsert = $("<div/>").addClass('validation-summary').attr('id', 'validationSummary').css({ "border-style": "none" });
    divToInsert.appendTo(blockDiv);
    var ulToInsert = $("<ul/>").attr('id', 'validationUL');
    ulToInsert.appendTo(divToInsert);

    $.each(messageArray, function (i) {
        var li = $("<li/>").appendTo(ulToInsert);
        var span = $("<span/>").html(messageArray[i]).appendTo(li);
        li.find('br').remove();
    });
}

function setValidationClient(controlsToHighlight) {
    for (var i = 0; i < controlsToHighlight.length; i++) {
        var control = $("#" + controlsToHighlight[i].name);
        var span = $("<span/>").addClass('validationMessage').html(controlsToHighlight[i].message)
        if (controlsToHighlight[i].isKendo) {
            control.closest('.k-widget').addClass('validationError');
            span.insertAfter(control.closest('.k-widget'));
        }
        else {
            control.addClass("validationError");
            span.insertAfter(control);
        }
        $("#" + controlsToHighlight[i].label).addClass('validationLabel');
    }
}

function clearValidationErrorMessages() {
    try {
        var div = $("#validationSummary");
        div.remove()
    }
    catch (e) { }

    $('.validationError').each(function (i) { $(this).removeClass("validationError"); });
    $('.validationMessage').each(function (i) { $(this).remove(); });
    $('.validationLabel').each(function (i) { $(this).removeClass("validationLabel"); });
}