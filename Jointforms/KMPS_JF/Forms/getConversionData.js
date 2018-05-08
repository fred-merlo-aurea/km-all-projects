var ECNstepname = '';

function getConversionURL()
{
    var sURL = window.document.URL.toString();
    var conversionURL = "";
    var bidValue = "";
    var eidValue = "";
    if (sURL.indexOf("?") > 0)
    {
        var arrParams = sURL.split("?");
        var arrURLParams = arrParams[1].split("&");

        var arrParamNames = new Array(arrURLParams.length);
        var arrParamValues = new Array(arrURLParams.length);

        var i = 0;
        for (i = 0; i < arrURLParams.length; i++)
        {
            var sParam = arrURLParams[i].split("=");
            arrParamNames[i] = sParam[0];
            if (sParam[1] != "")
                arrParamValues[i] = unescape(sParam[1]);
            else
                arrParamValues[i] = "No Value";
        }

        for (i = 0; i < arrURLParams.length; i++)
        {
            if (arrParamNames[i] == "ctrk_bid")
            {
                bidValue = arrParamValues[i];
            } else if (arrParamNames[i] == "ctrk_eid")
            {
                eidValue = arrParamValues[i];
            }
        }

        var origURL = sURL.split("ctrk_bid");
        var url = origURL[0].split("?")
        conversionURL = "http://www.ecn5.com/ecn.communicator/engines/conversion.aspx?b=" + bidValue + "&e=" + eidValue + "&oLink=" + url[0] + "?step=" + ECNstepname + "&" + url[1];
    }
    else
    {
        conversionURL = "http://www.ecn5.com/1x1.jpg";
    }
    return conversionURL;
}
