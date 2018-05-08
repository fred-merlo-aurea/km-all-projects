'use strict';
window.XdUtils = window.XdUtils || (function () {

    function extend(object, defaultObject) {
        var result = defaultObject || {};
        var key;
        for (key in object) {
            if (object.hasOwnProperty(key)) {
                result[key] = object[key];
            }
        }
        return result;
    }

    //public interface
    return {
        extend: extend
    };
})();

window.xdLocalStorage = window.xdLocalStorage || (function () {
    var MESSAGE_NAMESPACE = 'cross-domain-local-message';
    var options = {
        iframeId: 'cross-domain-iframe',
        iframeUrl: undefined,
        initCallback: function () { }
    };
    var requestId = -1;
    var iframe;
    var requests = {};
    var wasInit = false;
    var iframeReady = true;

    function applyCallback(data) {
        if (requests[data.id]) {
            requests[data.id](data);
            delete requests[data.id];
        }
    }


    function receiveMessage(event) {
        var data;
        try {
            data = JSON.parse(event.data);
        } catch (err) {
            //not our message, can ignore
        }
        if (data && data.namespace === MESSAGE_NAMESPACE) {
            if (data.id === 'iframe-ready') {
                iframeReady = true;
                options.initCallback();
            } else {
                applyCallback(data);
            }
        }
    }

    function buildMessage(action, key, value, callback) {
        requestId++;
        requests[requestId] = callback;
        var data = {
            namespace: MESSAGE_NAMESPACE,
            id: requestId,
            action: action,
            key: key,
            value: value
        };
        iframe.contentWindow.postMessage(JSON.stringify(data), '*');
    }
    function init(customOptions) {
        options = XdUtils.extend(customOptions, options);
        var temp = document.createElement('div');

        if (window.addEventListener) {
            window.addEventListener('message', receiveMessage, false);
        } else {
            window.attachEvent('onmessage', receiveMessage);
        }

        temp.innerHTML = '<iframe id="' + options.iframeId + '" src=' + options.iframeUrl + ' style="display: none;"></iframe>';
        document.body.appendChild(temp);
        iframe = document.getElementById(options.iframeId);
    }

    function isApiReady() {
        if (!wasInit) {
            console.log('You must call xdLocalStorage.init() before using it.');
            return false;
        }
        if (!iframeReady) {
            console.log('You must wait for iframe ready message before using the api.');
            return false;
        }
        return true;
    }

    return {
        //callback is optional for cases you use the api before window load.
        init: function (customOptions) {
            if (!customOptions.iframeUrl) {
                throw 'You must specify iframeUrl';
            }
            if (wasInit) {
                console.log('xdLocalStorage was already initialized!');
                return;
            }
            wasInit = true;
            if (document.readyState === 'complete') {
                init(customOptions);
            } else {
                if (window.addEventListener) {
                    window.addEventListener('load', init(customOptions), false);
                } else {
                    window.attachEvent('onload', init(customOptions));
                }
            }
        },
        setItem: function (key, value, callback) {
            if (!isApiReady()) {
                return;
            }
            buildMessage('set', key, value, callback);
        },

        getItem: function (key, callback) {
            if (!isApiReady()) {
                return;
            }
            buildMessage('get', key, null, callback);
        },
        removeItem: function (key, callback) {
            if (!isApiReady()) {
                return;
            }
            buildMessage('remove', key, null, callback);
        },
        key: function (index, callback) {
            if (!isApiReady()) {
                return;
            }
            buildMessage('key', index, null, callback);
        },
        clear: function (callback) {
            if (!isApiReady()) {
                return;
            }
            buildMessage('clear', null, null, callback);
        },
        wasInit: function () {
            return wasInit;
        }
    };
})();

// ECN_tracker
var apiUrl = "http://apidt.ecn5.com";
var wsUrl = "http://specialprojects.ecn5.com/ecn.webservice";

jQuery(document).ready(function () {
    xdLocalStorage.init({
        iframeUrl: wsUrl + '/cross-domain-local-storage.html',
        initCallback: function () {
            console.log('XDLS iframe ready');
            xdLocalStorage.setItem('check', 'no callback');

            var emailID = ECNGetQueryStringByName('eid');
            var emailAddress = ECNGetQueryStringByName('emailAddress');
            emailAddress = ECNValidateEmail(emailAddress);
            var baseChannelID = -1;

            if (TrackerKey != null) {
                jQuery.ajax({
                    type: "POST",
                    url: apiUrl + "/api/internal/domaintracking/VerifyAccountAnon",
                    data: JSON.stringify(TrackerKey),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (retDataVerify) {
                        if (retDataVerify.BaseChannelID != -1) {
                            baseChannelID = retDataVerify.BaseChannelID;
                            if (emailAddress != '')
                                ECNSetLocalStorage(baseChannelID + "_ECNEmailAddress", emailAddress, retDataVerify.TrackAnon, baseChannelID);
                            else if (emailID != '')
                                ECNGetEmailAddress(emailID, baseChannelID, retDataVerify.TrackAnon);
                            else
                                ECNGetLocalStorage(baseChannelID + "_ECNEmailAddress", baseChannelID, retDataVerify.TrackAnon);
                        }
                    }
                });
            }
        }
    });
});

function ECNGetEmailAddress(emailID, baseChannelID, trackAnon) {
    var emailAddress = null;
    jQuery.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: apiUrl + "/api/internal/domaintracking/GetEmailAddress",
        data: JSON.stringify(emailID),
        dataType: "json",
        success: function (retDataGET) {
            emailAddress = retDataGET;
            if (emailAddress != null) {
                ECNSetLocalStorage(baseChannelID + "_ECNEmailAddress", emailAddress, trackAnon, baseChannelID);
            }
        }
    });
    return emailAddress;
}

function ECNGetDomainTrackerFields(emailAddress) {
    jQuery.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: apiUrl + "/api/internal/domaintracking/GetDomainTrackerFields",
        data: JSON.stringify(TrackerKey),
        dataType: "json",
        success: function (domainTrackerFields) {
            ECNSendData(domainTrackerFields, emailAddress);
        }
    });
}

function ECNSendData(domainTrackerFields, emailAddress) {
    console.log('Sending data...');
    var JSONobj = eval(domainTrackerFields);
    var domainTrackerFieldCollection = JSONobj;
    var sendUDFData = null;
    var resultValue = null;
    //Construct sendUDFData JSON here
    if (domainTrackerFieldCollection != null) {
        sendUDFData = '[';
        for (var j = 0; j < domainTrackerFieldCollection.length; j++) {
            var UDF = domainTrackerFieldCollection[j];
            sendUDFData = sendUDFData + '{"DomainTrackerFieldsID" : "' + UDF.DomainTrackerFieldsID + '" , "FieldValue" :';
            if (UDF.Source == "QueryString") {
                var result = ECNGetQueryStringByName(UDF.SourceID);
                if (result == "")
                    result = null;
                sendUDFData = sendUDFData + '"' + result + '"';
            }
            else if (UDF.Source == "Cookie") {
                var result = ECNReadCookie(UDF.SourceID);
                if (result == null || result.trim() == "")
                    resultValue = null;
                else
                    resultValue = result;
                sendUDFData = sendUDFData + '"' + resultValue + '"';
            }
            else if (UDF.Source == "HTMLElement") {
                var result = document.getElementById(UDF.SourceID);
                if (result == null || result.value.trim() == "")
                    resultValue = null;
                else
                    resultValue = result.value;
                sendUDFData = sendUDFData + '"' + resultValue + '"';
            }
            else {
                resultValue = null;
                sendUDFData = sendUDFData + '"' + resultValue + '"';
            }
            sendUDFData = sendUDFData + ', "Source": "' + UDF.Source + '", "SourceID": "' + UDF.SourceID + '"}';
            if (j < domainTrackerFieldCollection.length - 1) {
                sendUDFData = sendUDFData + ',';
            }

        }
        sendUDFData = sendUDFData + ']';
    }
    var JsonSend = eval("(" + sendUDFData + ")");
    var refURL = document.referrer;
    var curURL = document.location.href;
    var domainTrackActivity = { DomainTrackerFieldCollection: JsonSend, TrackerKey: TrackerKey, EmailAddress: emailAddress, SourceBlastID: 0, ReferralURL: refURL, CurrentURL: curURL };
    jQuery.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: apiUrl + "/api/internal/domaintracking/UpdateDomainTrackerActivity",
        data: JSON.stringify(domainTrackActivity),
        dataType: "json",
        success: function (data) {
            console.log('Domain tracker activity updated.');
        },
        error: function (errorObj) {
        }
    });
}


function ECNGetQueryStringByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS, 'i');
    var results = regex.exec(window.location.search);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}

function ECNCreateCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";

    var domain = document.location.hostname;
    var spl = domain.split('.');
    domain = spl[spl.length - 2] + '.' + spl[spl.length - 1];

    document.cookie = name + "=" + value + expires + "; domain=" + domain + "; path=/";

}

function ECNReadCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    var email = null;
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ')
            c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0)
            email = c.substring(nameEQ.length, c.length);
    }
    return email;
}

function ECNSetLocalStorage(key, value, trackAnon, baseChannelID) {
    xdLocalStorage.setItem(key, value, function (data) {
        if (data.success) {
            console.log('Data has been successfully stored.');
            ECNCreateCookie("ECNEmailAddress", value, 365);
            if (trackAnon == true) {
                ECNReconcileAnon(value, baseChannelID);
            }
            ECNGetDomainTrackerFields(value);
        } else {
            console.log('Ops, could not store local data.');
        }
    });
}

function ECNSetLocalAnonStorage(key, value) {
    xdLocalStorage.setItem(key, value, function (data) {
        if (data.success) {
            console.log('Data has been successfully stored.');
            ECNCreateCookie("Anon_ECNEmailAddress", value, 365);
            ECNGetDomainTrackerFields(value);
        } else {
            console.log('Ops, could not store local data.');
        }
    });
}

function ECNGetLocalStorage(key, baseChannelID, trackAnon) {
    xdLocalStorage.getItem(key, function (data) {
        if (data.value != null) {
            console.log('Data has been successfully retrieved.');
            ECNGetDomainTrackerFields(data.value);
        } else {
            var email = ECNReadCookie("ECNEmailAddress");
            if (email != null) {
                email = decodeURIComponent(email);
                email = ECNValidateEmail(email);

                if (trackAnon && email != '')
                    ECNReconcileAnon(email, baseChannelID);

                if (email != '')
                    ECNSetLocalStorage(data.key, email);
            }
            else if (trackAnon == true) {
                
                ECNGetLocalAnonStorage(baseChannelID + "_Anon_ECNEmailAddress", baseChannelID);
                
            }
            else {
                console.log('Could not get any local data.');
            }
        }
    });
}

function ECNGetLocalAnonStorage(key, baseChannelID) {
    var anonEmail = "";
    xdLocalStorage.getItem(key, function (data) {
        if (data.value != null) {
            anonEmail = data.value;
            anonEmail = decodeURIComponent(anonEmail);
            anonEmail = ECNValidateEmail(anonEmail);
            if (anonEmail != '' && anonEmail != '-@unknownkmpsgroup.com')
                ECNGetDomainTrackerFields(anonEmail);
            //return data.value;
        }
        else {
            anonEmail = ECNReadCookie("Anon_ECNEmailAddress");
            if (anonEmail != null) {
                anonEmail = decodeURIComponent(anonEmail);
                anonEmail = ECNValidateEmail(anonEmail);
                if (anonEmail != '' && anonEmail != '-@unknownkmpsgroup.com')
                    ECNGetDomainTrackerFields(anonEmail);
                
            }
            else {
                anonEmail = ECNCreateAnonEmail(baseChannelID);
                if (anonEmail != '' && anonEmail != '-@unknownkmpsgroup.com')
                    ECNGetDomainTrackerFields(anonEmail);

            }
        }
    });
}

function ECNRemoveLocalAnonStorage(key) {
    xdLocalStorage.removeItem(key, function (data) {
        console.log("Anon email removed from LS");
    });

    ECNCreateCookie("Anon_ECNEmailAddress", "", -1);

}

function ECNReconcileAnon(email, baseChannelID) {
    //var anonEmail = ECNCreateAnonEmail();
    var anonLSEmail = "";

    xdLocalStorage.getItem(baseChannelID + "_Anon_ECNEmailAddress", function (data) {
        if (data.value != null) {
            anonLSEmail = data.value;
            ECNMergeCall(anonLSEmail, email, baseChannelID);
        }
        else {
            var anonCookieEmail = ECNReadCookie("Anon_ECNEmailAddress");
            if (anonCookieEmail != null) {
                anonLSEmail = anonCookieEmail;
                ECNMergeCall(anonLSEmail, email, baseChannelID);
            }
            else {
                console.log('could not find anon email');
            }
        }
    });

    

}

function ECNMergeCall(anonLSEmail, email, baseChannelID)
{
    if (anonLSEmail != '' && ECNValidateEmail(email) != '') {
        //merge activity by email address
        var mergeObject = { BaseChannelID: baseChannelID, AnonEmail: anonLSEmail, ActualEmail: email };
        jQuery.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: apiUrl + "/api/internal/domaintracking/MergeAnonActivity",
            data: JSON.stringify(mergeObject),
            dataType: "json",
            success: function () {
                console.log("successful merge");
                ECNRemoveLocalAnonStorage(baseChannelID + "_Anon_ECNEmailAddress");
            },
            error: function (errorObj) {
            }
        });
    }
}

function ECNCreateAnonEmail(baseChannelID) {

    var ECN_localIP = "";
    var ECN_PublicIP = "";
    /*Usage example*/
    try {
        window.RTCPeerConnection = window.RTCPeerConnection || window.mozRTCPeerConnection || window.webkitRTCPeerConnection;   //compatibility for firefox and chrome
        var pc = new RTCPeerConnection({ iceServers: [] }), noop = function () { };
        pc.createDataChannel("");    //create a bogus data channel
        pc.createOffer(pc.setLocalDescription.bind(pc), noop);    // create offer and set local description
        pc.onicecandidate = function (ice) {  //listen for candidate events
            if (!ice || !ice.candidate || !ice.candidate.candidate) return;
            var myIP = /([0-9]{1,3}(\.[0-9]{1,3}){3}|[a-f0-9]{1,4}(:[a-f0-9]{1,4}){7})/.exec(ice.candidate.candidate)[1];
            console.log('my IP: ', myIP);
            ECN_localIP = myIP;
            pc.onicecandidate = noop;

        };
    }
    catch (e) {

    }
    jQuery.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: apiUrl + "/api/internal/domaintracking/GetPublicIP",
        dataType: "json",
        success: function (data) {
            ECN_PublicIP = data;
            var anonEmail = ECN_localIP + "-" + ECN_PublicIP + "@unknownkmpsgroup.com";
            anonEmail = decodeURIComponent(anonEmail);
            anonEmail = ECNValidateEmail(anonEmail);
            if (anonEmail != '' && anonEmail != '-@unknownkmpsgroup.com')
                ECNSetLocalAnonStorage(baseChannelID + "_Anon_ECNEmailAddress", anonEmail);
        },
        error: function (errorObj) {
        }
    });

    return ECN_localIP + "-" + ECN_PublicIP + "@unknownkmpsgroup.com";

}

function ECNMergeForClient(knownEmailAddress)
{
    knownEmailAddress = ECNValidateEmail(knownEmailAddress);
    
    if (TrackerKey != null) {
        jQuery.ajax({
            type: "POST",
            url: apiUrl + "/api/internal/domaintracking/VerifyAccountAnon",
            data: JSON.stringify(TrackerKey),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (retData) {
                if (retData.BaseChannelID != -1 && retData.TrackAnon && knownEmailAddress != '') {
                     var baseChannelID = retData.BaseChannelID;
                    ECNReconcileAnonAndSetLocal(knownEmailAddress, baseChannelID);
                        
                }
            }
        });
    }
}

function ECNReconcileAnonAndSetLocal(email, baseChannelID) {
    //var anonEmail = ECNCreateAnonEmail();
    var anonLSEmail = "";

    xdLocalStorage.getItem(baseChannelID + "_Anon_ECNEmailAddress", function (data) {
        if (data.value != null) {
            anonLSEmail = data.value;
            ECNMergeCallAndSetLocal(anonLSEmail, email, baseChannelID);
        }
        else {
            var anonCookieEmail = ECNReadCookie("Anon_ECNEmailAddress");
            if (anonCookieEmail != null) {
                anonLSEmail = anonCookieEmail;
                ECNMergeCallAndSetLocal(anonLSEmail, email, baseChannelID);
            }
            else {
                console.log('could not find anon email, adding known to cache and cookie');
                ECNSetLocalStorage(baseChannelID + "_ECNEmailAddress", email, false, baseChannelID);
            }
        }
    });
}

function ECNMergeCallAndSetLocal(anonLSEmail, email, baseChannelID) {
    if (anonLSEmail != '' && ECNValidateEmail(email) != '') {
        //merge activity by email address
        var mergeObject = { BaseChannelID: baseChannelID, AnonEmail: anonLSEmail, ActualEmail: email };
        var key = baseChannelID + "_ECNEmailAddress";
        jQuery.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: apiUrl + "/api/internal/domaintracking/MergeAnonActivity",
            data: JSON.stringify(mergeObject),
            dataType: "json",
            success: function () {
                console.log("successful merge");
                ECNRemoveLocalAnonStorage(baseChannelID + "_Anon_ECNEmailAddress");
                xdLocalStorage.setItem(key, email, function (data) {
                    if (data.success) {
                        console.log('Data has been successfully stored.');
                        ECNCreateCookie("ECNEmailAddress", email, 365);                        
                    } else {
                        console.log('Ops, could not store local data.');
                    }
                });
            },
            error: function (errorObj) {
            }
        });
    }
}

function ECNValidateEmail(emailAddress) {
    var email_rex = /^[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@([a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[a-z][a-z])|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$/i; // new RegExp('^[A-z\-_0-9\.]+@[A-z\-_0-9\.]+\.[A-z_0-9]{2,}$');
    if (!email_rex.test(emailAddress))
        emailAddress = '';
    return emailAddress;
}