var prepopulateJson = '';
var fieldRulesJson = '';
var pageRulesJson = '';
var formRulesJson = '';
var buttonNamesJson = '';
var url = '';
var message = '';
var url_def = '';
var message_def = '';
var inactive_url = '';
var inactive_message = '';
var SubmitInProgress = false;
var submitHandle = null;
var timersHandle = null;
var gsitekey = '';
var g_lst = new Object();
var pagesHistory = [];
var emailRex = '';
var inner_validating = false;
var currentDivId = '';
var divArray = new Object();
var endForm = "00000000-0000-0000-0000-000000000000";
var http = 'http://';
var https = 'https://';
var emailControlID = '';
var allowChanges = '';
var countryControlID = '';
var stateControlID = '';
var passwordControlID = '';
var prepopulate_url = '';
var prepopulate_timeout = 3000;
var prepopulate_handle = null;
var messageDelay = '';
var inIframe = '';
var subLoginJson = '';
var isLoggedIn = false;
var pathname = '';
var ajaxformurl = '';
var formKeyValue = [];
var currentformStatisticID = 0;
var loginEmail = '';
var loginEmailID = 0;

String.prototype.myStartsWith = function (str) {
    if (this.indexOf(str) === 0) {
        return true;
    } else {
        return false;
    }
};

if (!Array.prototype.remove) {
    Array.prototype.remove = function (val, all) {
        var i, removedItems = [];
        if (all) {
            for (i = this.length; i--;) {
                if (this[i].indexOf(val) >= 0) removedItems.push(this.splice(i, 1));
            }
        }
        else {  //same as before...
            i = this.indexOf(val);
            if (i > -1) removedItems = this.splice(i, 1);
        }
        return removedItems;
    };
}

$(document).ready(function () {
    $('form').each(function () {
        $(this).find('input').keypress(function (e) {
            if (e.which == 13) // Enter key = keycode 13
                return false;
        });
    });

    // ChangeEmail check boxes
    var changeEmailSuppress = $(".ChangeEmailSuppress");
    changeEmailSuppress.change(function () {
        var thisID = this.id;
        changeEmailSuppress.each(function (i, elem) {
            var val = $(elem).val().toString();
            $(elem).attr("disabled", false);
            if (!thisID.toLowerCase().endsWith(val.toLowerCase())) {
                $(elem).prop("checked", false);
            }
        });
        $(this).attr("disabled", true);
    });

    // setting the url for Form Controller methods
    pathname = window.location.pathname;
    if (pathname.toLowerCase().myStartsWith("/kmweb")) {
        ajaxformurl = '/KMWeb/Forms';
    }
    else {
        ajaxformurl = '/Forms';
    }
    //for iframe we have a key
    var isIFrame = getParameterByName('isIFrame');
    if (isIFrame == '1' || isIFrame == 'true') {
        $('div.formprewcontetnbox').css('width', 'auto');
    }
    if (inactive_url == '' && inactive_message == '') {
        //get statistic part
        //var currentformStatisticID = InitStatistic();
        var isSubmitDone = false;
        $(window).unload(function () {
            if (!isSubmitDone) {
                if (isLoggedIn) {
                    var div = $('#' + currentDivId);
                    var currentPage = getNumberPage(currentDivId);
                    unloadForm(currentPage);
                }
                else {
                    unloadForm(0);
                }
            }
        });
        url = url_def;
        message = message_def;
        AddValidationMethods();
        PrepareValidationRules();
        var objFormString = getFormDataSerialized();
        var objForm = unserializeFormData(objFormString);
        for (var i = 0; i < objForm.length; i++) {
            formKeyValue.push(objForm[i]);
            formKeyValue[objForm[i]] = 1;
        }

        $('div.loaderbox').hide();
        $('div.kmcontainer').hide();
        $('button.nav').hide();
        var firstDiv = $('div.kmcontainer').first();
        currentDivId = firstDiv[0].id;
        firstDiv.show();
        PrepopulateData();
        PrepopulateFromDB();
        ApplyFieldRules();
        ChangeEmailHide();
        ApplyNavigation(firstDiv);
        RunTimers(firstDiv);
        function getNumberPage(idDiv) {
            var currentPage = null;
            var i = 1;
            var count = $('div.kmcontainer').length;
            for (; i <= count; i++) {
                if (($('div.kmcontainer')[i - 1]).id == idDiv) {
                    currentPage = i;
                }
            }
            if (currentPage == null) {
                return 1;
            }
            else {
                return currentPage;
            }
        }

        //add handlers to buttons
        $('button.btn_prev_form').click(function () {
            $('div.kmcontainer').hide();
            var id = currentDivId;
            var currentPage = getNumberPage(id);
            finishLog(currentPage);
            currentDivId = pagesHistory.pop();
            currentPage = getNumberPage(currentDivId);
            saveNew(currentPage);
            var currentDiv = $('#' + currentDivId);
            ApplyNavigation(currentDiv);
            currentDiv.show();
            return false;
        });
        $('button.btn_next_form').click(function () {
            var div = $('#' + currentDivId);
            var currentPage = getNumberPage(currentDivId);            
            if (Validate(div, false)) {
                finishLog(currentPage);
                updateTotalPages(currentPage + 1);
                div.hide();
                pagesHistory.push(div[0].id);
                var currentDiv = $(this).data('next');
                currentDivId = currentDiv[0].id;
                ApplyNavigation(currentDiv);
                currentDiv.show();
                currentPage = getNumberPage(currentDivId);
                saveNew(currentPage);
            }
            return false;
        });
        $('#' + emailControlID).blur(function () {
            var email = $('#' + emailControlID).prop("value");
            var email_rex = new RegExp(emailRex);
            res = email_rex.test($('#' + emailControlID).val());
            if (res) {
                updateEmail(email);
            }
        });
        $('#' + emailControlID).keypress(function () {
            if (isLoggedIn && loginEmail != '' && endsWith(loginEmail.toLowerCase(), "kmpsgroup.com"))
                setTimeout(checkIfEmailExist, 10);
            else
                RequestPrepopulateFromDB();
        });
        //$('#' + emailControlID).change(function ()
        //{
        //    RequestPrepopulateFromDB();
        //});
        $('#' + emailControlID).on('paste', function () {
            if (isLoggedIn && loginEmail != '' && endsWith(loginEmail.toLowerCase(), "kmpsgroup.com"))
                setTimeout(checkIfEmailExist, 10);
            else
                RequestPrepopulateFromDB();
        });
        // Clear states options if exist.
        clearStates();
        // Check if countries exists.        
        if ($('#' + countryControlID).length != 0) {
            $('#' + countryControlID).change(function () {
                countryControlChangeEvent(); // On Country selection, adjust states
            }).trigger('change');
        } else {
            if ($('#' + stateControlID).length != 0) {
                $.ajax({
                    async: false,
                    type: 'POST',
                    url: ajaxformurl + '/GetStatesAll',
                    success: function (response) {
                        $('#' + stateControlID).empty();
                        var states = "<option></option>";
                        $.each(response, function (i, inputItem) {
                            var s = inputItem.split('|');
                            states += '<option value="' + s[1] + '">' + s[2] + '</option>';
                        });
                        $('#' + stateControlID).append(states);
                        ApplyFieldRules();
                    }
                });
            }
        }
        $('button.btn_submit_form').click(function (e) {
            e.preventDefault();
            //ApplyFormRules();

            // Validate ConfirmPassword_Control
            if ($("#ConfirmPassword_Control").length != 0) {
                var pass = $('#' + passwordControlID).val();
                var cpass = $("#ConfirmPassword_Control").val();
                if (pass != cpass) {
                    fancyAlert("Passwords must match");
                    return false;
                }
            }

            //var div = $(this).parents('div.kmcontainer');
            var div = $('#' + currentDivId);
            var currentPage = getNumberPage(currentDivId);

            if (!SubmitInProgress && Validate(div, true)) {
                OnBeginSubmit();
                var toCheck = div.find('div.g-recaptcha');
                var g_responses = div[0].id + ';';
                for (var i = 0; i < toCheck.length; i++) {
                    g_responses += grecaptcha.getResponse(g_lst[toCheck[i].id]) + ';'
                }
                $('#g_resp').val(g_responses.substr(0, g_responses.length - 1));
                var submit_url = $(div).find('form').attr('action');
                var data = getFormDataSerialized();
                var dataArr = data.split('&');
                var pIndex = 0;
                for (var i = 0; i < formKeyValue.length; i++) {
                     if (formKeyValue[formKeyValue[i]] == 0) {
                        dataArr.remove(formKeyValue[i], true);
                     }
                     if (passwordControlID == formKeyValue[i])
                     {
                         var pass = $('#' + passwordControlID).val();
                         if(pass == "KMPS_Form_Pwd")
                             dataArr.remove(formKeyValue[i], true);
                     }
                 }
               
                data = '';
                for (var i = 0; i < dataArr.length; i++) {
                    data += dataArr[i] + '&';
                }
                data = data.slice(0, -1);

                var submitReady = false;
                if (loginEmail != '' && endsWith(loginEmail.toLowerCase(), "kmpsgroup.com")) {
                    var repData = { kmpsemail: loginEmail, newemail: $('#' + emailControlID).val(), groupID: subLoginJson.GroupID, update: true, emailID: loginEmailID };
                    $.ajax({
                        url: ajaxformurl + '/UpdateKMPSEmail',
                        method: "post",
                        data: JSON.stringify(repData),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (response) {
                            if (response[0] == '200') {
                                submitReady = true;
                            }
                            else {
                                fancyAlert(response[1]);
                                OnEndSumbit(false, true);
                                submitReady = false;
                            }
                        }
                    });
                }
                else
                    submitReady = true;

                if (submitReady) {
                    $.ajax({
                        url: submit_url,
                        type: 'post',
                        data: data,
                        dataType: "json",
                        success: function (res) {
                            $('.btn_submit_form').prop('disabled', true);
                            var email = $('#' + emailControlID).prop("value");
                            submitLog(email, currentPage);
                            isSubmitDone = true;
                            url = res[1];
                            message = res[2];
                            //if (res[1].startsWith("http"))
                            //{ }
                            //whatever you wanna do after the form is successfully submitted
                            formRulesJson = JSON.parse(res[3]);
                            ApplyFormRules();
                            OnEndSumbit(true, true);
                        },
                        error: function (error) {
                            //on error
                            if (error.responseText != '') {
                                fancyAlert('Error during submit form process: ' + error.responseText);
                            }
                            OnEndSumbit(false, true);
                        }
                        //statusCode: {
                        //    //here you can trigger code for concrete status
                        //    200: function () {
                        //    }
                        //},
                        //complete: function () {
                        //    //complete after all other events
                        //}
                    });
                    submitHandle = setTimeout(OnEndSumbit, 180000, false, false);
                }
            }
        });

        // Subscriber Login & Password
        if (subLoginJson.LoginRequired) {
            currentformStatisticID = InitStatistic(0);
            var emailAddress = getParameterByName(subLoginJson.EmailAddressQuerystringName);
            if (emailAddress == null) { emailAddress = ""; }            

            var other = "";
            if (subLoginJson.OtherQuerystringName != "") {
                other = getParameterByName(subLoginJson.OtherQuerystringName);
            }
            if (other == null) { other = ""; }

            var password = "";
            if (subLoginJson.PasswordQuerystringName != "") {
                password = getParameterByName(subLoginJson.PasswordQuerystringName);
            }
            if (password == null) { password = ""; }

            if (emailAddress != "" && other != "") {
                emailAddress = "";
            }                

            if (subLoginJson.AutoLoginAllowed) {                
                if (subLoginJson.PasswordRequired) {
                    if ((emailAddress != '' || other != '') && password != '') {
                        logInProcess(emailAddress, other, password);
                    }
                    else {
                        showLogin(emailAddress, other);
                    }
                }
                else {
                    if (emailAddress != '' || other != '') {
                        logInProcess(emailAddress, other, password);
                    }
                    else {
                        showLogin(emailAddress, other);
                    }
                }
            }
            else {
                showLogin(emailAddress, other);
            }
        }
        else {
            currentformStatisticID = InitStatistic(1);
            isLoggedIn = true;
            hideLogin();
        }

        $("input[name='subscription']").click(function () {
            if ($('input:radio[name=subscription]')[0].checked) {
                $("#newSubscription").show();
                $("#existingSubscription").hide();
            }
            else {
                $("#newSubscription").hide();
                $("#existingSubscription").show();
            }
        });

        $("#EmailAddress2").focusin(function () {
            $("#EmailAddress3").val("");
            $("#Password3").val("");
        });

        $("#Password2").focusin(function () {
            $("#EmailAddress3").val("");
            $("#Password3").val("");
        });

        $("#EmailAddress3").focusin(function () {
            $("#EmailAddress2").val("");
            $("#Password2").val("");
        });

        $("#Password3").focusin(function () {
            $("#EmailAddress2").val("");
            $("#Password2").val("");
        });

        // Tabbing to Login
        $('#EmailAddress2').keydown(function (e) {
            if ((e.keyCode || e.which) == '9') {
                if ($('#Password2').is(':visible'))
                    $('#Password2').focus();
                else
                    $('#loginBtn').focus();

                return false;
            }
        });

        $('#Password2').keydown(function (e) {
            if ((e.keyCode || e.which) == '9') {
                $('#loginBtn').focus();

                return false;
            }
        });

        $('#EmailAddress3').keydown(function (e) {
            if ((e.keyCode || e.which) == '9') {
                if ($('#Password3').is(':visible'))
                    $('#Password3').focus();
                else
                    $('#loginBtn').focus();

                return false;
            }
        });

        $('#Password3').keydown(function (e) {
            if ((e.keyCode || e.which) == '9') {
                $('#loginBtn').focus();

                return false;
            }
        });
    }
    else {
        RedirectOrAlert(inactive_message, inactive_url, false);
    }

    $(window).resize(function () {
        updateContainerSize();
    });
});

function showLogin(emailAddress, other) {
    $("#loginModalDisplay").show();
    $("#loginModalPopupWindow").show();
    $("#LoginModalHTML").html(subLoginJson.LoginModalHTML);
    $('label[for=new]').html(subLoginJson.NewSubscriberButtonText);
    $('label[for=existing]').html(subLoginJson.ExistingSubscriberButtonText);
    $("#signupBtn").html(subLoginJson.SignUpButtonText);
    $("#forgotpasswordBtn").html(subLoginJson.ForgotPasswordButtonText);
    $("#loginBtn").html(subLoginJson.LoginButtonText);
    if (subLoginJson.SubIdLabel != "")
        $("#SubIdLabel").html(subLoginJson.SubIdLabel);

    if (emailAddress != '' || other != '') {
        $("#existing").prop("checked", true);
        $("#newSubscription").hide();
        $("#existingSubscription").show();
        $("#EmailAddress2").val(emailAddress);
        $("#EmailAddress3").val(other);
    }
    else {
        $("#new").prop("checked", true);
        $("#newSubscription").show();
        $("#existingSubscription").hide();        
    }

    if (subLoginJson.PasswordRequired) {
        $("#existingPasswordRow1").show();
        $("#existingPasswordRow2").show();
        $("#forgotpasswordBtn").show();
    }
    else {
        $("#existingPasswordRow1").hide();
        $("#existingPasswordRow2").hide();
        $("#forgotpasswordBtn").hide();
    }
    
    if (subLoginJson.OtherIdentificationSelection) {
        $("#cont2").show();
        $("#cont3").show();
    }
    else {
        $("#cont2").hide();
        $("#cont3").hide();
        //$('#existingSubscription').css("margin", "0px 75px");
        $('#EmailAddress2').css("width", "474px");
        $('#Password2').css("width", "474px");
    }
}

function hideLogin() {
    $("#loginModalDisplay").hide();
    $("#loginModalPopupWindow").hide();
}

function logInProcess(emailAddress, other, password) {
    $('div.loaderbox').show();
    $.ajax({
        type: 'POST',
        url: ajaxformurl + '/PublicFormLogin',
        data: { emailAddress: emailAddress, other: other, password: password.trim(), passReq: subLoginJson.PasswordRequired, groupID: subLoginJson.GroupID, OtherIdentification: subLoginJson.OtherIdentification },
        dataType: "json",
        success: function (response) {
            if (response[0] == '200') {
                isLoggedIn = true;
                hideLogin();
                finishLog(0);
                updateTotalPages(1);
                saveNew(1);
                $('#' + emailControlID).val(response[1]);
                updateEmail(response[1]);
                loginEmail = response[1];
                loginEmailID = response[2];
                RequestPrepopulateFromDB();                
                MarkEmailFieldReadonly(true);
            }
            else if (response[0] == '404') {
                showLogin('', '');
                fancyAlert(response[1]);
                $("#EmailAddress").val(emailAddress);
            }
            else {
                showLogin(emailAddress, other);
                fancyAlert(response[1]);
            }
            $('div.loaderbox').hide();
        }
    });
}

function signup() {
    var emailAddress = $("#EmailAddress").val();
    //var password = $("#Password").val();
    //var confirmPassword = $("#ConfirmPassword").val();
    if (emailAddress == null || emailAddress == "") {
        fancyAlert("Email Address cannot be empty.");
        return false;
    }
    if (!validateEmail(emailAddress)) {
        fancyAlert("Email Address not valid.");
        return false;
    }
    //if (subLoginJson.PasswordRequired) {
    //    if (password == null || password == "") {
    //        fancyAlert("Password cannot be empty.");
    //        return false;
    //    }
    //    if (password != confirmPassword) {
    //        fancyAlert("Confirm Password do not match.");
    //        return false;
    //    }
    //}
    //if (password == null)
    //    password = '';
    $('div.loaderbox').show();
    $.ajax({
        type: 'POST',
        url: ajaxformurl + '/PublicFormSignUp',
        data: { emailAddress: emailAddress, groupID: subLoginJson.GroupID },
        dataType: "json",
        success: function (response) {
            if (response[0] == '404') {
                isLoggedIn = true;
                hideLogin();
                finishLog(0);
                updateTotalPages(1);
                saveNew(1);
                $('#' + emailControlID).val(emailAddress);
                updateEmail(emailAddress);
                MarkEmailFieldReadonly(true);
            }
            else if (response[0] == '200') {
                showLogin(emailAddress, '');
                fancyAlert(response[1]);
            }
            else {
                showLogin(emailAddress, '');
                fancyAlert(response[1]);
            }
            $('div.loaderbox').hide();
        }
    });
}

function login() {
    var emailAddress = $("#EmailAddress2").val();
    var other = $("#EmailAddress3").val();
    var password = "";
    if (emailAddress == "") {
        if (subLoginJson.OtherIdentificationSelection) {
            if (other == "") {
                fancyAlert(subLoginJson.SubIdLabel + " cannot be empty.");
                return false;
            }
        } else {
            fancyAlert("Email Address cannot be empty.");
            return false;
        }
    }

    if (emailAddress != "")
        password = $("#Password2").val();
    if (other != "")
        password = $("#Password3").val();


    logInProcess(emailAddress, other, password);
}

function MarkEmailFieldReadonly(isReadonly) {
    var id = "#" + emailControlID;
    if (isReadonly) {
        $(id).prop("readonly", "readonly");
    }
    else {
        $(id).removeProp("readonly");
    }
}

function ChangeEmailHide() {
    $("#changeEmailModalDisplay").hide();
    $("#changeEmailPopupWindow").hide();
}

function ChangeEmailShow(suppressOn) {
    if (suppressOn) {
        $(".ChangeEmailSuppress").each(function (i, elem) {
            $(elem).prop("checked", false);
            $(elem).prop("disabled", false);
        });
        $("#changeEmailSupressOptions").show();
    }
    else {
        $("#changeEmailSupressOptions").hide();
    }
    var id1 = "#changeEmailPopupWindow";
    var id2 = "#changeEmailModalDisplay";
    var id3 = "#" + emailControlID;
    var id4 = "#EmailAddressOld";
    var show = suppressOn || !$(id1).is(":visible");
    if (show) {
        $(id1).show();
        $(id2).show();
        var v = $(id3).val();
        $(id4).val(v);
    }
    else {
        $(id1).hide();
        $(id2).hide();
        $(id4).val("");
    }
}

function ChangeEmailValidate() {
    var oldEmailAddress = $("#EmailAddressOld").val();
    var newEmailAddress = $("#EmailAddressNew").val();
    var confirmEmailAddress = $("#EmailAddressNewConfirm").val();
    if (oldEmailAddress == null || oldEmailAddress == "") {
        var msg = "Old Email Address cannot be empty.";
        fancyPrompt(false, false, msg);
        return false;
    }
    if (newEmailAddress == null || newEmailAddress == "") {
        var msg = "New Email Address cannot be empty.";
        fancyPrompt(false, false, msg);
        return false;
    }
    if (newEmailAddress == oldEmailAddress) {
        var msg = "New Email Address cannot be the Old Email Address.";
        fancyPrompt(false, false, msg);
        return false;
    }
    if (newEmailAddress != confirmEmailAddress) {
        var msg = "Emails must match.";
        fancyPrompt(false, true, msg);
        return false;
    }
    if (!validateEmail(newEmailAddress)) {
        var msg = "Email Address not valid.";
        fancyPrompt(false, true, msg);
        return false;
    }
    return true;
}

function ChangeEmailSubmit() {
    var isValid = ChangeEmailValidate();
    if (!isValid) {
        return;
    }
    var oldEmailAddress = $("#" + emailControlID).val();
    var newEmailAddress = $("#EmailAddressNew").val();
    var suppressOn = $("#changeEmailSupressOptions").is(':visible');
    ChangeEmailProcess(newEmailAddress, oldEmailAddress, suppressOn);
}

function ChangeEmailProcess(newAddress, oldAddress, suppressOn) {
    $('div.loaderbox').show();
    var suppressValue = 0;
    if (suppressOn) {
        $(".ChangeEmailSuppress").each(function (i, elem) {
            if ($(elem).is(':checked')) {
                suppressValue = parseInt($(elem).val().toString());
            }
        });
    }
    if (suppressValue == 3) {
        ChangeEmailPromptCancel(true);
        $('div.loaderbox').hide();
        ChangeEmailHide();
        return;
    }
    $.ajax({
        type: 'POST',
        url: ajaxformurl + '/PublicFormChangeEmail',
        data: { newEmailAddress: newAddress, oldEmailAddress: oldAddress, groupID: subLoginJson.GroupID, formID: subLoginJson.FormID, suppressValue: suppressValue },
        async: true,
        dataType: "json",
        success: function (response) {
            if (response[0] === '200') {
                var newEmailAddress = $("#EmailAddressNew").val();
                $("#EmailAddress2").val(newEmailAddress);
                $('#' + emailControlID).val(newEmailAddress);
                updateEmail(newEmailAddress);
                loginEmail = newEmailAddress;
                isLoggedIn = true;
                ChangeEmailHide();
            }
            else if (response[1].indexOf('Suppressed') !== -1) {
                fancyPrompt(false, true, "Your email address has been suppressed in our system.To receive our emails again, please check the appropriate box.");
                ChangeEmailShow(true);
            }
            else if (response[1].indexOf('Email invalid') !== -1) {
                $('div.loaderbox').hide();
                fancyPrompt(false, true, "We couldn't reach your email address's mail server. Please re-enter your email address or provide a different one.");
                return;
            }
            else if (response[1].indexOf('Email exists') !== -1) {
                $('div.loaderbox').hide();
                fancyPrompt(true, true, "The email address you entered is already signed up for this product. Please select Cancel to enter a different email address or select Login to manage your preferences for this email address.");
                return;
            }
            else {
                $('div.loaderbox').hide();
                fancyPrompt(false, false, response[1]);
                return;
            }
            $('div.loaderbox').hide();
        }
    });
}

function forgotpassword() {
    var emailAddress = $("#EmailAddress2").val();
    var other = $("#EmailAddress3").val();
    if (emailAddress == "") {
        if (subLoginJson.OtherIdentificationSelection) {
            if (other == "") {
                fancyAlert(subLoginJson.SubIdLabel + " cannot be empty.");
                return false;
            }
        } else {
            fancyAlert("Email Address cannot be empty.");
            return false;
        }
    }
    $('div.loaderbox').show();
    $.ajax({
        type: 'POST',
        url: ajaxformurl + '/PublicFormSendPassword',
        data: { emailAddress: emailAddress, other: other, groupID: subLoginJson.GroupID, formID: subLoginJson.FormID, OtherIdentification: subLoginJson.OtherIdentification },
        dataType: "json",
        success: function (response) {
            if (response[0] == '200') {
                showLogin(emailAddress, other);
                fancyAlert(response[1]);
            }
            else if (response[0] == '404') {
                showLogin(emailAddress, other);
                fancyAlertWithTextbox(response[1]);
            }
            else
            {
                showLogin(emailAddress, other);
                fancyAlert(response[1]);
            }
            $('div.loaderbox').hide();
        }
    });
}

function UpdateProfileEmail() {
    var emailAddress = $("#EmailAddress2").val();
    var other = $("#EmailAddress3").val();
    var newEmailAddress = $("#EmailAddress4").val();
    if (newEmailAddress == "") {
        $("#validation_msg").html("Email Address cannot be empty.");
        return false;
    }
    if (!validateEmail(newEmailAddress)) {
        $("#validation_msg").html("Email Address not valid.");
        return false;
    }

    $('div.loaderbox').show();
    $.ajax({
        type: 'POST',
        url: ajaxformurl + '/PublicFormUpdateProfileAndSendPassword',
        data: { newEmailAddress: newEmailAddress, emailAddress: emailAddress, other: other, groupID: subLoginJson.GroupID, formID: subLoginJson.FormID, OtherIdentification: subLoginJson.OtherIdentification },
        dataType: "json",
        success: function (response) {
            if (response[0] == '200') {
                jQuery.fancybox.close();
                showLogin(emailAddress, other);
                $("#EmailAddress2").val("");
                $("#EmailAddress3").val("");
                fancyAlert(response[1]);
            }
            else {
                showLogin(emailAddress, other);
                $("#validation_msg").html(response[1]);
            }
            $('div.loaderbox').hide();
        }
    });
}

function validateEmail(emailAddress) {
    var email_rex = /^[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@([a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[a-z][a-z])|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$/i; // new RegExp('^[A-z\-_0-9\.]+@[A-z\-_0-9\.]+\.[A-z_0-9]{2,}$');
    if (email_rex.test(emailAddress))
        return true;
    else
        return false;
}

function saveNew(currentPage) {
    var data = { FormStatistic_ID: currentformStatisticID, numberPage: currentPage };
    var url = "UploadStatistic/UploadNewer";
    $.ajax({
        url: url,
        method: "post",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (paramData, textStatus, jqXHR) {
            //currentformStatisticID = paramData["FormStatistic_ID"];
        },
        error: function () {
            //alert('error');
        },
        statusCode: {
            404: function () {
                alert("page not found");
            }
        }
    });
}
function finishLog(currentPage) {
    var data = { FormStatistic_ID: currentformStatisticID, numberPage: currentPage };
    var url = "UploadStatistic/UploadFinish";
    $.ajax({
        url: url,
        method: "post",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (paramData, textStatus, jqXHR) {
            //currentformStatisticID = paramData["FormStatistic_ID"];
        },
        error: function () {
            //alert('error');
        },
        statusCode: {
            404: function () {
                alert("page not found");
            }
        }
    });
}
function updateTotalPages(currentPage) {
    var data = { FormStatistic_ID: currentformStatisticID, totalPages: currentPage };
    var url = "UploadStatistic/UpdateTotalPages";
    $.ajax({
        url: url,
        method: "post",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (paramData, textStatus, jqXHR) {
            //currentformStatisticID = paramData["FormStatistic_ID"];
        },
        error: function () {
            //alert('error');
        },
        statusCode: {
            404: function () {
                alert("page not found");
            }
        }
    });
}
function updateEmail(email) {
    var data = { FormStatistic_ID: currentformStatisticID, email: email };
    var url = "UploadStatistic/UpdateEmail";
    $.ajax({
        url: url,
        method: "post",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (paramData, textStatus, jqXHR) {
            //currentformStatisticID = paramData["FormStatistic_ID"];
        },
        error: function () {
            //alert('error');
        },
        statusCode: {
            404: function () {
                alert("page not found");
            }
        }
    });
}
function unloadForm(currentPage) {
    var data = { FormStatistic_ID: currentformStatisticID, numberPage: currentPage };
    var url = "UploadStatistic/UnloadForm";
    $.ajax({
        url: url,
        method: "post",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (paramData, textStatus, jqXHR) {
            //currentformStatisticID = paramData["FormStatistic_ID"];
        },
        error: function () {
            //alert('error');
        },
        statusCode: {
            404: function () {
                alert("page not found");
            }
        }
    });
}
function submitLog(email, currentPage) {
    var data = { FormStatistic_ID: currentformStatisticID, numberPage: currentPage, email: email };
    var url = "UploadStatistic/UploadSubmit";
    $.ajax({
        url: url,
        method: "post",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (paramData, textStatus, jqXHR) {
            //currentformStatisticID = paramData["FormStatistic_ID"];
        },
        error: function () {
            //alert('error');
        },
        statusCode: {
            404: function () {
                alert("page not found");
            }
        }
    });
}

function InitStatistic(page) {
    var num = 0;
    var currentdate = new Date();
    var datetime = (currentdate.getMonth() + 1) + "/"
                    + currentdate.getDate() + "/"
                    + currentdate.getFullYear() + " "
                    + currentdate.getHours() + ":"
                    + currentdate.getMinutes() + ":"
                    + currentdate.getSeconds();


    var email = $('#' + emailControlID).prop("value");
    var email_rex = new RegExp(emailRex);
    res = email_rex.test($('#' + emailControlID).val());
    email = res ? email : null;

    var totalPages = $('div.kmcontainer').length;
    var data = { formUID: $("#km_form_token").attr("value"), totalPages: page, email: email };
    var url = "UploadStatistic/CreateStatistic";
    $.ajax({
        url: url,
        method: "post",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (paramData, textStatus, jqXHR) {
            currentformStatisticID = paramData["FormStatistic_ID"];
        },
        error: function () {
            alert('Create Statistic Error');
        },
        statusCode: {
            404: function () {
                alert("page not found");
            }
        }
    });
}

var onloadCallback = function () {
    $('div.g-recaptcha').each(function (i, el) {
        this.style.height = "81px";
        var widId = grecaptcha.render(el.id, {
            'sitekey': gsitekey,
            'callback': 'captchaSuccess'
        });
        g_lst[el.id] = widId;
        // Removing Catpcha Width
        var gcapt = $('.g-recaptcha div');
        gcapt[0].removeAttribute("style");
    });
}

function OnBeginSubmit() {
    SubmitInProgress = true;
    $('div.loaderbox').show();
}

function OnEndSumbit(success, resetFlag) {
    clearTimeout(submitHandle);
    if (SubmitInProgress) {
        clearInterval(timersHandle);
        $('div.loaderbox').hide();
        if (resetFlag) {
            SubmitInProgress = false;
        }
        if (SubmitInProgress) {
            SubmitInProgress = false;
            fancyAlert('Submit failed by timeout!');
            $.ajax({
                async: false,
                type: 'POST',
                url: ajaxformurl + '/PublicFormTimeout',
                success: function (response) {
                    // Logging sev 2 FD client timeout
                }
            });
        }
        else {
            if (success) {
                RedirectOrAlert(message, url, true);
            }
        }
    }
}

function RedirectOrAlert(message, url, activeForm) {
    $('html,body').scrollTop(0);
    if (message != '' && url != '') {
        // Set Delay Redirect
        var redirectCounter = '<div style="text-align:center;margin-top:10px;"><h5 style="color:#bfbfbf">You will be redirected after <span id="timeLeft" style="color:#999999">' + messageDelay + '</span> seconds. Thanks!</h5></div>';
        var btn = ''
        if (activeForm && !inIframe) { btn = '<div style="text-align:center;margin-top:10px;"><input style="margin:3px; padding:5px 15px;" type="button" onclick="redirectToURL(\'' + url + '\');" value="Ok"></div>'; }
        $('<div style="position:fixed; top:0; left:0; width:100%; height:100%; background-color:#fff; z-index:1000;"></div>').appendTo(document.body);
        $('<div id="confirmationMessageDiv" style="position:absolute; top:10%;font-family:Verdana; z-index:10000;">' + message + btn + redirectCounter + '</div>').appendTo(document.body);
        updateContainerSize();

        window.setInterval(function () {
            var timeLeft = $("#timeLeft").html();
            if (parseInt(timeLeft) == 0) {
                redirectToURL(url);
            } else {
                $("#timeLeft").html(parseInt(timeLeft) - parseInt(1));
            }
        }, 1000);
    }
    else if (url != '') {
        redirectToURL(url);
    }
    else if (message != '') {
        var btn = ''
        if (activeForm && !inIframe) { btn = '<div style="text-align:center;margin-top:10px;"><input style="margin:3px; padding:5px 15px;" type="button" onclick="window.close();" value="Ok"></div>'; }
        $('<div style="position:fixed; top:0; left:0; width:100%; height:100%; background-color:#fff; z-index:1000;"></div>').appendTo(document.body);
        $('<div id="confirmationMessageDiv" style="position:absolute; top:10%;font-family:Verdana; z-index:10000;">' + message + btn + '</div>').appendTo(document.body);
        updateContainerSize();
    }
}

function updateContainerSize() {
    var maxWidth = Math.max.apply(null, $('div').map(function () {
        return $(this).outerWidth(true);
    }).get());
    var msgDiv = $('#confirmationMessageDiv').outerWidth(true);
    var prtgWidth = (maxWidth - msgDiv) / 2;

    $('#confirmationMessageDiv').css('left', prtgWidth + 'px');
}

function redirectToURL(url) {
    if (url.indexOf('%%') == -1)
        url = decodeURIComponent(url);
    if (url.indexOf(http) != 0 && url.indexOf(https) != 0) {
        url = http + url;
    }
    document.location.href = url;
}

function Translate(lang) {
    var curr_lang = getParameterByName('lang');
    if (curr_lang == null) {
        location += '&lang=' + lang;
    }
    else {
        var new_loc = location.href;
        new_loc = new_loc.replace('&lang=' + curr_lang, '&lang=' + lang).replace('?lang=' + curr_lang, '?lang=' + lang);
        location = new_loc;
    }
}

function Original() {
    var curr_lang = getParameterByName('lang');
    if (curr_lang != null) {
        var new_loc = location.href;
        new_loc = new_loc.replace('&lang=' + curr_lang, '').replace('?lang=' + curr_lang, '');
        location = new_loc;
    }
}

function PrepopulateData() {
    for (var key in prepopulateJson) {
        SetValueById(key, getParameterByName(prepopulateJson[key]));
    }
}

function RequestPrepopulateFromDB() {
    prepopulate_timeout = 10;
    if (prepopulate_timeout > 0) {
        if (prepopulate_handle != null) {
            clearTimeout(prepopulate_handle);
        }
        prepopulate_handle = setTimeout(PrepopulateFromDB, prepopulate_timeout);
    }
    else {
        PrepopulateFromDB();
    }
}

var prePopDataObj = null; // Prepopulate from Database data as a global variable
function PrepopulateFromDB() {
    var email = $('#' + emailControlID).val();
    var email_rex = new RegExp(emailRex);
    if (email != '' && email_rex.test(email)) {
        $('div.loaderbox').show();
        $.ajax({
            url: prepopulate_url,
            type: 'post',
            data: $('div.kmcontainer form').serialize(),
            success: function (data) {
                if (email == $('#' + emailControlID).val()) {
                    if (data != null && data != "") {
                        if (data[0] == '{' && data[data.length - 1] == '}') {
                            prePopDataObj = JSON.parse(data);
                            for (var id in prePopDataObj) {
                                SetValueById(id, prePopDataObj[id]);
                            }
                            ApplyFieldRules(); // Trigger field rules
                            ApplyFieldRules(); // Trigging again for correct field rules after hidding controls
                            if (endsWith(email.toLowerCase(), "kmpsgroup.com")) {
                                $('#' + emailControlID).val("");
                            }
                        }
                        else {
                            var e = email.split("@");
                            var f = e[1].split(".");
                            if (f[1].length > 2 || f[2] != null) {
                                fancyAlert(data);
                            }
                        }
                    }
                }
                $('div.loaderbox').hide();
            },
            error: function (error) {
                $('div.loaderbox').hide();
                if (error.responseText != '') {
                    fancyAlert(error.responseText);
                }
            },
        });
    }
}

function checkIfEmailExist() {
    var email = $('#' + emailControlID).val();
    var email_rex = new RegExp(emailRex);
    var skipdomains = "gmail.co;hotmail.co;msn.co;yahoo.co;juno.co;hushmail.co;zoho.co";
    var sda = skipdomains.split(";");
    var skip = false;
    for (var i = 0; i < sda.length; i++) {
        if (endsWith(email, sda[i])) {
            skip = true;
        }
    }
    if (email != '' && email_rex.test(email) && !skip) {
        $('div.loaderbox').show();
        var repData = { kmpsemail: loginEmail, newemail: $('#' + emailControlID).val(), groupID: subLoginJson.GroupID, update: false, emailID: loginEmailID };
        $.ajax({
            url: ajaxformurl + '/UpdateKMPSEmail',
            method: "post",
            data: JSON.stringify(repData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (response) {
                if (response[0] == '500') {
                    fancyAlert(response[1]);
                }
                $('div.loaderbox').hide();
            }
        });
    }
}

function SetValueById(id, value) {
    if (value != null) {
        if (id.indexOf('#') != 0) {
            id = '#' + id;
        }
        var controlsToSkip = new Array();

        var customControls = $('.km_item [id^="' + id.replace('#', '') + '"]');//this will find all controls that start with the clean ControlID(ex:list of all checkboxes in a list)
        var listBoxControls = $('.kmListbox [id^="' + id.replace('#', '') + '"]');//this will find all list box controls because they have a diff parent div
        if (customControls.length > 0) {
            for (var i = 0; i < customControls.length; i++) {
                var current = customControls[i];
                var values = value.split(',');
                for (var j = 0; j < values.length; j++) {
                    if (current.hasAttribute('value') && current.value.toLowerCase() == values[j].toLowerCase()) {
                        try {
                            current.checked = true;

                        } catch (err) { }

                        try {
                            current.selected = true;

                        } catch (err) { }

                        try {
                            var selected = customControls.filter(function () {
                                return $(this).val().toLowerCase() == values[j].toLowerCase() || $(this).text().toLowerCase() == values[j].toLowerCase();
                            });
                            $(id).val(selected.val());
                            break;
                        } catch (err) { }

                        break;
                    }
                    else if (values[j].trim() == "") {
                        try {
                            current.checked = false;

                        } catch (err) { }

                        try {
                            current.selected = false;

                        } catch (err) { }
                    }
                }
            }
        }
        else if (listBoxControls.length > 0) {
            var controlsToSkip = new Array();

            if (value.indexOf(',') >= 0) {
                controlsToSkip.push(id.replace('#', ''));
                var valueSplit = value.split(',');
                for (var i = 0; i < valueSplit.length; i++) {
                    for (var j = 0; j < listBoxControls.length; j++) {
                        var current = listBoxControls[j];
                        if (current.hasAttribute('value') && current.value.toLowerCase() == valueSplit[i].toLowerCase()) {
                            try {
                                current.selected = true;

                            } catch (err) { }
                        }
                    }
                }
            }

        }
        try {
            var options = $(id + ' option');//this will find check boxes for newsletters
            if (options.length == 0) {
                if (endsWith(id, '_subscribe')) {
                    //for newsletter
                    if (value != '') {
                        if (value == 'S' || value == 'U') {
                            $(id).show();
                            $(id).next().show();
                            $(id)[0].checked = value == 'S';
                        }
                        else {
                            $(id).hide();
                            $(id).next().hide();
                        }
                        if (value == 'hide_control') {
                            $(id).parents('li.liElement').hide();
                        }
                    }
                }
                else {
                    if (controlsToSkip.indexOf(id) < 0) {
                        $(id).val(value);
                        if (id == '#' + passwordControlID && $('#' + passwordControlID).length != 0 && $("#ConfirmPassword_Control").length != 0) {
                            $('#ConfirmPassword_Control').val(value);
                        }
                        if (id == '#' + emailControlID && allowChanges == "no") {
                            $(id).prop('readonly', true);
                        }                            
                    }
                }
            }
            else {
                var selected = options.filter(function () {
                    return $(this).val().toLowerCase() == value.toLowerCase() || $(this).text().toLowerCase() == value.toLowerCase();
                });
                if (controlsToSkip.indexOf(id.replace('#', '')) < 0) {
                    $(id).val(selected.val());
                }
                if (id == '#' + countryControlID && $('#' + countryControlID).length != 0) {
                    if (selected.val() != "") {
                        countryControlChangeEvent();
                    }
                    else {
                        clearStates();
                    }
                }
            }
        }
        catch (err) { }
    }
}

function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)");
    results = regex.exec(location.search);

    return results === null ? null : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function captchaSuccess(response) {
    $("#captchaValidate").val('valid');

    var div = $('#' + currentDivId);
    
    div.find('form').each(function (i, el) {        
        if ($(el).validate().numberOfInvalids() > 0) {
            //they have submitted the form once, run validate on everything            
            $(el).validate().settings.ignore = "";
            $(el).valid();
            $(el).validate().settings.ignore = ":hidden";

        }
        else {//they haven't submitted the form
            $(el).data("validator").settings.ignore = ":not(.dontIgnore)";
            $(el).valid();
            $(el).data("validator").settings.ignore = ":hidden";
        }
    });
}

function Validate(div, isSubmit) {
    var res = true;
    SetGridValues(div);
    if (isSubmit) {
        var email_rex = new RegExp(emailRex);
        res = email_rex.test($('#' + emailControlID).val());
    }
    if (res) {
        div.find('form').each(function (i, el) {
            if (res) {
                Revalidate(el);
                //res = $(el).validate({ ignore: ":not(:visible)" }).numberOfInvalids() == 0;
                res = $(el).validate({ ignore: ":not(:visible) :not(.dontIgnore)" }).numberOfInvalids() == 0;
            }
        });
    }
    else {
        fancyAlert(TranslateNotif('Email is not specified or incorrect!'));
    }

    return res;
}

function TranslateNotif(text) {
    var lang = getParameterByName('lang');
    if (lang) {

        var data = { text: text, target: lang };
        var results;
        $.ajax({
            url: ajaxformurl + '/TranslateNotification',
            type: 'post',
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (retData) {
                results = retData;
            },
            error: function (error) {
                results = text;

            }

        });

        return results;
    }
    else {
        return text;
    }
}

function SetGridValues(div) {
    var grids = $('div.kmGrid');
    if (div != null) {
        grids = div.find('div.kmGrid');
    }
    grids.each(function (i, el) {
        var table = $(el).find('table');
        var checked = table.find(':checked');
        var value = '';
        for (var i = 0; i < checked.length; i++) {
            value += checked[i].id + ',';
        }
        $(el).find('#' + table[0].id + '_value').val(value.substr(0, value.length - 1));
    });
}

function AddValidationMethods() {
    ////handler for newsletter radio
    //$('div.kmNewsletter input[type=\'radio\']').click(function () {
    //    var txt = $(this).closest('div').find('input[type=\'text\']');
    //    txt.rules("remove");
    //    txt.rules("add", { pattern: emailRex });
    //    txt.closest('form').valid();

    //    return true;
    //});

    //fix regex with empty input
    jQuery.validator.addMethod("customrex", function (value, element, regexp) {
        var rex = new RegExp(regexp);

        return this.optional(element) || rex.test(value);
    }, TranslateNotif("Invalid format."))

    //add grid
    jQuery.validator.addMethod("gridtype", function (value, element, type) {
        var error = TranslateNotif("Invalid or not full selection.");
        var table = $(element).closest('table');
        var tableId = table[0].id;
        var selected = table.find(':checked');
        var res = false;
        switch (type) {
            case 1:
                res = selected.length == 1;
                break;
            case 2:
                res = selected.length > 0;
                break;
            case 3:
                res = selected.length > 0;
                if (res) {
                    var rows = table.find('tr').length - 1;
                    res = selected.length >= rows;
                    if (res) {
                        var values = ';';
                        for (var i = 0; i < selected.length; i++) {
                            try {
                                var id = selected[i].id;
                                id = id.substr(tableId.length + 1);
                                id = id.substr(0, id.indexOf('_'));
                                values += id + ';';
                            }
                            catch (e)
                            { }
                        }
                        for (var i = 0; i < rows; i++) {
                            res = values.indexOf(';' + i + ';') > -1;
                            if (!res) {
                                break;
                            }
                        }
                    }
                }
                break;
        }
        table.closest('div').find('div.grid_error').html(res ? '' : error);
        Revalidate(table.find('input'));

        return res;
    }, "");

    $('div.kmCheckbox div.km_item').click(function () {
        Revalidate($(this).closest('div.kmCheckbox').find('div.km_item input'));

        return true;
    });
    $('div.kmListbox select').click(function () {
        Revalidate($(this).closest('div.kmListbox').find('select'));

        return true;
    });
}

function Revalidate(el) {
    if (!inner_validating) {
        inner_validating = true;
        $(el).valid();
        inner_validating = false;
    }
}

function RunTimers(first) {
    var index = 0;
    while (first.length > 0) {
        divArray[first[0].id] = index++;
        first = first.next();
    }
    timersHandle = setInterval(RunEverySecond, 1000, null);
}

function RunEverySecond(e) {
    var input = $('#km_form_timers');
    var value = input.val();
    var data = value.split(';');
    data[divArray[currentDivId]]++;
    var newValue = '';
    for (var i = 0; i < data.length; i++) {
        newValue += data[i] + ';';
    }
    input.val(newValue.substr(0, newValue.length - 1));
}

function ApplyFieldRules() {
    var targets = getTargets(fieldRulesJson);
    var sources = getSourcesTargets(fieldRulesJson);
    for (var target in targets) {
        applyAction(targets, target);
    }
    for (var source in sources) {
        getByName(source).off();
        if (countryControlID == source) {
            $('#' + countryControlID).change(function () {
                countryControlChangeEvent();
            });
        }            
        getByName(source).on('keyup change', function () {
            var sourceTargets = sources[$(this).attr('name')];
            $.each(sourceTargets, function (i, target) {
                applyAction(targets, target);                
            });
            ApplyFieldRules();
        });
    }
}

function ApplyNavigation(page) {
    var pageRules = [];
    $.each(pageRulesJson, function (i, rule) {
        var groupsInfo = rule[0];
        var sources = getSources(groupsInfo);
        $.each(sources, function (i, source) {
            if (page.has("[name='" + source + "']").length) {
                pageRules.push(rule);
            }
        });
    });
    var pages = $("div.kmcontainer");
    var pageIndex = pages.index(page);
    var processNavigation = function () {
        var ruleIsTrue = false;
        $.each(pageRules, function (i, rule) {
            var groupsInfo = rule[0];
            var target = rule[1];
            var groupsConditions = getGroupsConditions(groupsInfo);
            var result = checkGroups(groupsConditions); // the conditions validation per group
            if (result) {
                if (target == endForm) {
                    showSubmit();
                } else {
                    var targetPage = $("#" + target);
                    if (pages.index(targetPage) <= pageIndex) {
                        return true;
                    }
                    var resultPage = getPage(targetPage);
                    if (resultPage) {
                        showNext(resultPage);
                    } else {
                        showSubmit();
                    }
                }
                ruleIsTrue = true;
                return false;
            }
        });
        if (!ruleIsTrue) { // this only enters if conditions checkGroups return false
            var resultPage = getPage(page.next());
            if (resultPage) {
                showNext(resultPage);
            } else {
                showSubmit();
            }
        }
    }
    var form = page.find("form");
    form.unbind('.navigation');
    form.bind('keyup.navigation change.navigation', processNavigation);
    processNavigation();
    setButtonsText(page.attr('id'));
    if (pagesHistory.length) {
        showPrevious();
    } else {
        hidePrevious();
    }
}

function showNext(next) {
    $('button.btn_submit_form').hide();
    $('button.btn_next_form').data('next', next);
    $('button.btn_next_form').show();
}

function showSubmit() {
    $('button.btn_next_form').hide();
    $('button.btn_submit_form').show();
}

function setButtonsText(pageId) {
    var previousText = buttonNamesJson[pageId][0];
    var nextText = buttonNamesJson[pageId][1];
    $('button.btn_prev_form').text(previousText);
    $('button.btn_next_form').text(nextText);
}

function showPrevious() {
    $('button.btn_prev_form').show();
}

function hidePrevious() {
    $('button.btn_prev_form').hide();
}

function getPage(page) {
    if (!page.length) {
        return null;
    }
    var elements = page.find("li.liElement");
    var hasVisible = false;
    $.each(elements, function (i, element) {
        if ($(element).css("display") != "none") {
            hasVisible = true;
            return false;
        }
    });
    if (!hasVisible) {
        return getPage(page.next());
    }
    else {
        return page;
    }
}

function ApplyFormRules() {
    //url = url_def;
    //message = message_def;
    $.each(formRulesJson, function (i, rule) {
        var groupInfo = rule[0];
        var resultType = rule[1];
        var value = rule[2];
        var groupsConditions = getGroupsConditions(groupInfo);
        var result = checkGroups(groupsConditions);
        if (result) {
            if (resultType == "message") {
                message = value;
                url = '';
            }
            if (resultType == "url") {
                url = value;
                message = '';
            }
            return false;
        }
    });
}

function getTargets(rules) {
    var targets = {};
    $.each(rules, function (i, rule) {
        var groupsInfo = rule[0];
        var target = rule[1];
        var parameter = rule[2];
        targets[target] = {
            groupsConditions: getGroupsConditions(groupsInfo),
            parameter: parameter
        };
    });
    return targets;
}

function getSources(groupsInfo) {
    var sources = [];
    for (var groupsConnector in groupsInfo) {
        var groups = groupsInfo[groupsConnector];
        $.each(groups, function (i, group) {
            for (var conditionsConnector in group) {
                var conditions = group[conditionsConnector];
                $.each(conditions, function (i, condition) {
                    var source = condition[0];
                    if (sources.indexOf(source) == -1) {
                        sources.push(source);
                    }
                });
            }
        });
    }
    return sources;
}

function getSourcesTargets(rules) {
    var sources = {};
    $.each(rules, function (i, rule) {
        var groupsInfo = rule[0];
        var target = rule[1];
        for (var groupsConnector in groupsInfo) {
            var groups = groupsInfo[groupsConnector];
            $.each(groups, function (i, group) {
                for (var conditionsConnector in group) {
                    var conditions = group[conditionsConnector];
                    $.each(conditions, function (i, condition) {
                        var source = condition[0];
                        if (!sources[source]) {
                            sources[source] = [];
                        }
                        if (sources[source].indexOf(target) == -1) {
                            sources[source].push(target);
                        }
                    });
                }
            });
        }
    });
    return sources;
}

function getGroupsConditions(groupsInfo) {
    var groupsConditions = {};
    for (var groupsConnector in groupsInfo) {
        groupsConditions.connector = groupsConnector;
        groupsConditions.groups = [];
        var groups = groupsInfo[groupsConnector];
        $.each(groups, function (i, group) {
            var targetGroup = {};
            for (var conditionsConnector in group) {
                targetGroup.connector = conditionsConnector;
                targetGroup.conditions = [];
                var conditions = group[conditionsConnector];
                $.each(conditions, function (i, condition) {
                    var comparisonType = condition[1];
                    targetGroup.conditions.push({
                        name: condition[0],
                        comparison: comparisonTypes[comparisonType],
                        value: condition[2]
                    });
                });
            }
            groupsConditions.groups.push(targetGroup);
        });
    }
    return groupsConditions;
}

function applyAction(targets, target) {
    var targetInfo = targets[target];
    var action = targetInfo.parameter;
    var result = checkGroups(targetInfo.groupsConditions);
    var resultAction = result ? action : (action == "show" ? "hide" : "show");    
    if (resultAction == "hide") {
        SetValueById(target, "");
        formKeyValue[target] = 0;
    }
    else if (resultAction == "show" && formKeyValue[target] == 0) {
        if (prePopDataObj != null) {
            var ctrls = [];
            for (var key in prePopDataObj) {
                if (key.myStartsWith(target)) {
                    ctrls.push(key);
                }
            }
            for (var i = 0; i < ctrls.length; i++) {
                SetValueById(ctrls[i], prePopDataObj[ctrls[i]]);
            }
        }
        formKeyValue[target] = 1;
    }
    getByName(target).closest('li')[resultAction]();
    if (resultAction == "hide")
        SetValueById(target, "");
    if ($("#ConfirmPassword_Control").length != 0 && target == passwordControlID) {
        getByName('ConfirmPassword_Control').closest('li')[resultAction]();
        if (resultAction == "hide")
            SetValueById('ConfirmPassword_Control', "");
    }
}

function checkGroups(conditions) {
    var groupConnector = conditions.connector;
    var groups = conditions.groups;
    var groupResults = [];
    $.each(groups, function (i, group) {
        var groupResult;
        var conditionConnector = group.connector;
        var conditions = group.conditions;
        var conditionResults = [];
        $.each(conditions, function (i, condition) {
            conditionResults.push(checkCondition(condition));
        });
        groupResult = conditionConnector == "and" ?
            all(conditionResults) :
            any(conditionResults);
        groupResults.push(groupResult);
    });
    var result = groupConnector == "and" ?
        all(groupResults) :
        any(groupResults);
    return result;
}

function checkCondition(condition) {
    var control = getByName(condition.name);
    var controlValue;
    var conditionValue;
    if (control.attr('type') == 'checkbox' || control.attr('type') == 'radio') {
        control = control.filter("[value='" + condition.value + "']");
        controlValue = control.is(":checked");
        conditionValue = true;
    } else if (control.prop("tagName").toLowerCase() == 'select') {
        control = control.find("option[value='" + condition.value + "']");
        controlValue = control.is(":selected");
        conditionValue = true;
    } else {
        controlValue = control.val();
        conditionValue = condition.value;
    }
    var comparisonType = condition.comparison;
    return compare(controlValue, conditionValue, comparisonType);
}

function compare(value1, value2, comparisonType) {
    var string1 = value1.toString().toLowerCase();
    var string2 = value2.toString().toLowerCase();
    switch (comparisonType) {
        case 'Is': return string1 == string2;
        case 'IsNot': return string1 != string2;
        case 'IsNull': return string1 == '';
        case 'IsNotNull': return string1 != '';
        case 'Contains': return string1.indexOf(string2) != -1;
        case 'DoesNotContain': return string1.indexOf(string2) == -1;
        case 'StartsWith': return string1.indexOf(string2) == 0;
        case 'EndsWith': return string1.indexOf(string2, string1.length - string2.length) != -1;
        case 'Before': return getDate(value1) < getDate(value2);
        case 'After': return getDate(value1) > getDate(value2);
        case 'Equals': return getNumber(value1) == getNumber(value2);
        case 'LessThan': return getNumber(value1) < getNumber(value2);
        case 'GreaterThan': return getNumber(value1) > getNumber(value2);
    }
}

function getNumber(value) {
    return value != '' ? Number(value) : NaN;
}

function getDate(value) {
    return new Date(value);
}

function getByName(name) {
    return $("[name='" + name + "']");
}

function all(arr) {
    var result = true;
    if (arr.length == 0) {
        return false;
    }
    $.each(arr, function (i, item) {
        if (!item)
            result = false;
        return;
    });
    return result;
}

function any(arr) {
    var result = false;
    $.each(arr, function (i, item) {
        if (item)
            result = true;
        return;
    });
    return result;
}

function ClickById(id) {
    $('#' + id).click();
}

function fancyAlert(msg) {
    $.fancybox({
        'modal': true,
        'content': '<div class="cleanLabel" style="margin:1px; width:auto; font-family:Verdana;">' + msg + '<div style="text-align:center;margin-top:10px;"><input style="margin:3px; padding:0px;" type="button" onclick="jQuery.fancybox.close();" value="Ok"></div></div>'
    });
}

function fancyAlertWithTextbox(msg) {
    $.fancybox({
        'modal': true,
        'content': '<div class="cleanLabel" style="margin:1px; width:auto; font-family:Verdana;">' + msg + '<div style="text-align:center;margin-top:10px;"><input style="margin:3px; padding:0px;" type="text" id="EmailAddress4"><br/><span style="font-size: 12px; color: red;" id="validation_msg"></span><br/><input style="margin:3px; padding:0px;" type="button" onclick="UpdateProfileEmail()" value="Ok"><input style="margin:3px 8px; padding:0px;" type="button" onclick="jQuery.fancybox.close();" value="Cancel"></div></div>'
    });
}

function fancyPrompt(showLogin, clear, msg) {
    var promptHTML = '<div class="cleanLabel" style="margin:1px; width:auto; font-family:Verdana;">' + msg + '<div style="text-align:center;margin-top:10px;">';
    if (showLogin) {
        promptHTML += '<input style="margin:3px; padding:2px 16px 2px 16px;" type="button" onclick="ChangeEmailPromptLogin();" value="Login">';
        promptHTML += '<input style="margin:3px; padding:2px 16px 2px 16px;" type="button" onclick="ChangeEmailPromptCancel(' + clear + ');" value="Cancel">';
    }
    else {
        promptHTML += '<input style="margin:3px; padding:2px 16px 2px 16px;" type="button" onclick="ChangeEmailPromptCancel(' + clear + ');" value="Ok">';
    }
    promptHTML += '</div></div>';
    $.fancybox({
        'modal': true,
        'content': promptHTML
    });
}

function ChangeEmailPromptCancel(clear)
{
    if (!clear) {
        jQuery.fancybox.close();
        return;
    }
    $("#EmailAddressNew").val("");
    $("#EmailAddressNewConfirm").val("");
    jQuery.fancybox.close();
}

function ChangeEmailPromptLogin() {
    ChangeEmailHide();
    var redirectAccount = $("#EmailAddressNew").val();
    showLogin(redirectAccount, "");
    jQuery.fancybox.close();
}

function unserializeFormData(data) {
    var objs = [], temp;
    var temps = data.split('&');

    for (var i = 0; i < temps.length; i++) {
        temp = temps[i].split('=');
        objs.push(temp[0]);
        objs[temp[0]] = temp[1];
    }
    return objs;
}

function getFormDataSerialized() {
    var data = $('div.kmcontainer form').serialize();
    $.each($('div.kmcontainer form input[type=checkbox]').filter(function (idx) { return $(this).prop('checked') === false }),
        function (idx, el) {
            if (data.indexOf($(el).attr('name')) < 0)
                data += '&' + $(el).attr('name') + '=';
        }
    );
    $.each($('div.kmcontainer form input[type=radio]').filter(function (idx) { return $(this).prop('checked') === false }),
        function (idx, el) {
            if (data.indexOf($(el).attr('name')) < 0)
                data += '&' + $(el).attr('name') + '=';
        }
    );
    $.each($('div.kmcontainer form').find('option').not(':selected'),
        function (idx, el) {
            if (data.indexOf($(el.parentElement).attr('name')) < 0)
                data += '&' + $(el.parentElement).attr('name') + '=';
        }
    );
    return data;
}

function countryControlChangeEvent() {           
    if ($('#' + stateControlID).length != 0) {
        //$('#' + stateControlID).prop('selectedIndex', 0);
        $('#' + stateControlID + ' option').each(function () {
            $(this).remove();
        });
        var states = "<option></option>";
        var key = $('#' + countryControlID).val();
        if (key != "") {
            $.ajax({
                async: false,
                type: 'POST',
                url: ajaxformurl + '/GetStatesByCountryId',
                data: "key=" + key,
                success: function (response) {
                    if (response.length != 0) {
                        for (var i = 0; i < response.length; i++) {
                            var s = response[i].split("|");
                            states += '<option value="' + s[0] + '">' + s[1] + '</option>';
                        }
                    } else {
                        $.ajax({
                            async: false,
                            type: 'POST',
                            url: ajaxformurl + '/GetStatesByCountryId',
                            data: "key=" + 0,
                            success: function (response) {
                                for (var i = 0; i < response.length; i++) {
                                    var s = response[i].split("|");
                                    states += '<option value="' + s[0] + '">' + s[1] + '</option>';
                                }
                            }
                        });
                    }
                }
            });
        }
        $('#' + stateControlID).append(states);
        //ApplyFieldRules();
    }
}

function clearStates() {
    if ($('#' + stateControlID).length != 0) {
        $('#' + stateControlID + ' option').each(function () {
            $(this).remove();
        });
        var states = "<option></option>";
        $('#' + stateControlID).append(states);
    }
}