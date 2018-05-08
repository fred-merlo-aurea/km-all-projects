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

function setValidationError(message, divID, controlsToHighlight)
{
    var div = $("#" + divID);
    div.html(message );
    div.css({ "color": "#e80c4d" })
    div.show();


    for(var i = 0; i < controlsToHighlight.length; i++)
    {
        var control = $("#" + controlsToHighlight[i]);
        control.addClass("validationError");
    }
}

function setControlValidationError(controlsToHighlight)
{
    for (var i = 0; i < controlsToHighlight.length; i++) {
        var error = controlsToHighlight[i];
        
        var control = $("#" + error.name);
        var span = $("<span/>").addClass('validationErrorMessage').html(error.message);
        if (error.isKendo) {
            control.closest('.k-widget').addClass('validationError');
            span.insertAfter(control.closest('.k-widget'));
        }
        else {
            control.addClass("validationError");
            span.insertAfter(control);
        }
        
    }
}

function clearControlValidationError()
{
    $('.validationErrorMessage').each(function (i) { $(this).remove(); });
}

function clearValidationError(divID, controlsToClear)
{
    var div = $("#" + divID);
    div.html('');
    div.hide();
    for (var i = 0; i < controlsToClear.length; i++) {
        var control = $("#" + controlsToClear[i]);
        control.removeClass("validationError");
    }
}

function setValidationSummary(messageArray, controlsToHighlight) {
    //var div = $("#validationSummary");
    var blockDiv = $("<div/>").css({ "display": "block", "margin-left": "auto", "margin-right": "auto", "width": "600px" });
    blockDiv.insertAfter($('.breadcrumb'));
    var divToInsert = $("<div/>").addClass('validation-summary').attr('id', 'validationSummary');
    divToInsert.appendTo(blockDiv);
    var imageDiv = $("<div/>").addClass('picon validation-image');
    imageDiv.appendTo(divToInsert);
    var image = $("<img/>").attr('src', '/ecn.communicator.mvc/Images/modal-icon-error.png').css({ "border": "none" });
    image.appendTo(imageDiv);
    var ulToInsert = $("<ul/>").attr('id', 'validationUL');
    ulToInsert.appendTo(divToInsert);

    //var ul = $("validationUL");
    $.each(messageArray, function (i) {
        var li = $("<li/>").appendTo(ulToInsert);
        var span = $("<span/>").html(messageArray[i]).appendTo(li);
        li.find('br').remove();
    });
    //div.html(message);
    //div.css({ "color": "#e80c4d" })
    //div.show();


    for (var i = 0; i < controlsToHighlight.length; i++) {
        var control = $("#" + controlsToHighlight[i]);
        control.addClass("validationError");
    }
}

function clearValidationSummary() {
    try {
        var div = $("#validationSummary");
        div.remove()
    }
    catch (e) { }
    //var ul = $("validationUL");        
    //ul.empty();
    //div.hide();
    $('.validationError').each(function (i) { $(this).removeClass("validationError"); });
    //for (var i = 0; i < controlsToClear.length; i++) {
    //    var control = $("#" + controlsToClear[i]);
    //    control.removeClass("validationError");
    //}
}

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