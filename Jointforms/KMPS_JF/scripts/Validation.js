
///////////////////////////////////////////////////////////
// File:			Validation.js
// File Start Date:	July 30, 2002
// Description:		Routes validation to appropriate validation handler
//
// This file must be placed in to /inetpub/wwwroot/_vti_script

String.prototype.trim = function () {
    return this.replace(/^\s*(\b.*\b|)\s*$/, "$1");
}

String.prototype.isDate = function () {
    var re = /^((0?\d)|(1[0-2]))(\\|\/|-)((0?\d)|([1-2]\d)|(3[0-1]))(\\|\/|-)(\d{2}|\d{4})$/;
    return re.test(this);
}

String.prototype.isTime = function () {
    var re = /^(((0?[1-9]|1[0-2])(:|\.)[0-5]\d((:|\.)[0-5]\d)?( )?(([aA]|[pP])[mM]))|((0?\d|1\d|2[0-3])(:|\.)[0-5]\d((:|\.)[0-5]\d)?))$/;
    return re.test(this);
}

String.prototype.isDateTime = function () {
    var re = /^((0?\d)|(1[0-2]))(\\|\/|-)((0?\d)|([1-2]\d)|(3[0-1]))(\\|\/|-)(\d{2}|\d{4})( )(((0?[1-9]|1[0-2])(:|\.)[0-5]\d((:|\.)[0-5]\d)?( )?(([aA]|[pP])[mM]))|((0?\d|1\d|2[0-3])(:|\.)[0-5]\d((:|\.)[0-5]\d)?))$/;

    if (this.isDate()) return true;
    if (this.isTime()) return true;
    if (re.test(this)) return true;
    return false;
}

/*Replaced Array.indexOf function with exists(array, val)*/

function exists(array, val) {
    for (var i = 0; i < array.length; i++)
        if (array[i] == val) return i;
    return -1;
}

function IsInteger(val) {
    if (val == '') return true;
    if (isNaN(parseFloat(val))) return false;
    if (parseInt(val) < parseFloat(val)) return false;
    if (val.lastIndexOf(".") == val.length - 1) return false;
    return true;
}

function IsDouble(val) {
    if (val == '') return true;
    if (isNaN(parseFloat(val))) return false;
    if (val.lastIndexOf(".") == val.length - 1) return false;
    return true;
}

/* validation - KeyPress - converted to support all browsers*/


//*******************Number Validation Starts Here**************************

function checkKeyPressForInteger(control, oEvent, allownegative) {

    var keyCode;

    if (window.event) {
        // for IE, e.keyCode or window.event.keyCode can be used
        keyCode = oEvent.keyCode;
    }
    else if (oEvent.which) {	// for Netscape and Mozilla
        keyCode = oEvent.which;
    }

    //Minus is allowed only as first char
    if (allownegative == 0 && keyCode == 45) {
        oEvent.returnValue = false;
        return false;
    }

    //only 0..9,-,backspace and non-keycode chars(del, arrows, ...)
    if ((keyCode < 48 || keyCode > 57) && (keyCode != 45) && (keyCode != 8) && (keyCode != 0)) {
        oEvent.returnValue = false;
        return false;

    }

    if (allownegative == 1 && keyCode == 45) {
        if (control.value.substring(0, 1) == "-") {
            oEvent.returnValue = false;
            return false;
        }
    }


}

function validateInteger(control) {
    if (control.value == "") {
        return true;
    }

    //alert(control.value.length)
    //check first letter for 0 
    if (control.value.length > 1) {
        if (control.value.match(/^0/) != null) {
            control.value = parseInt(control.value);
            return true;
        }
    }

    if (!IsInteger(control.value)) {
        alert("Only Integer values (0 to 9) are allowed");
        control.focus();
    }
}

//*******************Number Validation Ends Here**************************



//*******************Decimal Validation Starts Here**************************
var err_Decimal = 'Decimal input error';
var err_Decimal_General = 'Entered value is not a valid decimal number';

function checkKeyPressForDecimal(control, oEvent) {

    var value = control.value;
    var keyCode;

    if (window.event) {
        // for IE, e.keyCode or window.event.keyCode can be used
        keyCode = oEvent.keyCode;
    }
    else if (oEvent.which) {	// for Netscape and Mozilla 
        keyCode = oEvent.which;
    }

    var decimalSeparator = control.getAttribute("decimalSeparator");
    var sepCode = decimalSeparator.charCodeAt(0);

    //Decimal point is not allowed as first char
    if ((value.length == 0) && (keyCode == sepCode)) {
        oEvent.returnValue = false;
        return false;
    }
    //If a decimal point exists
    if ((value.indexOf(decimalSeparator) != -1) && (keyCode == sepCode)) {
        oEvent.returnValue = false;
        return false;
    }

    //Minus is allowed only as first char
    //if ((value.length > 0) && (keyCode == 45)) {
    //	oEvent.returnValue = false;
    //	return false;
    //}
    //alert(keyCode)


    if (value.length > 7) {
        if ((keyCode < 48 || keyCode > 57)) {
            oEvent.returnValue = false;
            return false;
        }
    }

    //only 0..9,-,backspace and non-keycode chars(del, arrows, ...)
    if ((keyCode < 48 || keyCode > 57) && (keyCode != 8) && (keyCode != 0) && keyCode != sepCode) {
        oEvent.returnValue = false;
        return false;
    }
}

function validateDecimal(control) {

    if (control.value == "") {
        return true;
    }

    var value = formatDecimal(control);
    if (value != "") {
        control.value = value;
        return true;
    }

    alert("Invalid Data. ");
    control.focus();
    return false;
}

function formatDecimal(control) {
    var num = control.value;
    if (num == "") return "";

    /*var IsDecimalExist;
    for(i=0;i<num.length;i++)
    if(num.substr(i,1)=='.')
    return false;*/

    var dg = control.getAttribute("decimalDigits");
    var ds = control.getAttribute("decimalSeparator");
    var gs = control.getAttribute("groupSeparator");

    var str = "";
    var sign = "";
    var nullDecimals = "";

    for (i = 0; i < dg; i++) {
        nullDecimals += "0";
    }
    if (num.charAt(0) == "-") {
        num = num.substr(1);
        sign = "-";
    }

    var DecPlace = num.indexOf(ds);
    if (DecPlace > 0) {
        str = "." + num.substr(DecPlace + 1);
        num = num.substr(0, DecPlace);
    }
    num = formatDecimal_replace(num, gs, "");
    num = num + str;

    var zeros = "";
    if (num.substr(0, 1) == "0") {
        var temp = Math.round(num * Math.pow(10, dg));
        temp = temp.toString();
        for (var i = temp.length; i < dg; i++) {
            zeros += "0";
        }
    }

    num = Math.round(num * Math.pow(10, dg));

    if (isNaN(num) == true) return handleDecimalError(control, err_Decimal_General);
    num = num.toString();

    if (num == "0") return ("0" + ds + nullDecimals);
    str = ds + zeros + num.substr(num.length - dg);

    num = num.substr(0, num.length - dg);

    //if (num == "") num = "0";
    //commented by vinayak on Aug 6th 5:00 PM  
    // it display the two decimal dot in some cases

    /*while (num.length > 2) {
    str = gs + num.substr(num.length - 2) + str;
    num = num.substr(0,num.length - 2);
    }*/
    str = num + str;
    return (sign + str);
}

function handleDecimalError(control, error) {
    return "";
}

function formatDecimal_replace(string, text, by) {
    //Replaces text with by in string
    var i = string.indexOf(text), newstr = '';
    if ((!i) || (i == -1))
        return string;
    newstr += string.substring(0, i) + by;
    if (i + text.length < string.length)
        newstr += formatDecimal_replace(string.substring(i + text.length, string.length), text, by);
    return newstr;
}
//*************************Decimal Validation ends here************************


///DATE VALIDATION

// Declaring valid date character, minimum year and maximum year
var dtCh = "/";
var minYear = 1900;
var maxYear = 2100;

function isInteger(s) {
    var i;
    for (i = 0; i < s.length; i++) {
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

function stripCharsInBag(s, bag) {
    var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++) {
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}

function daysInFebruary(year) {
    // February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
}
function DaysArray(n) {
    for (var i = 1; i <= n; i++) {
        this[i] = 31
        if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
        if (i == 2) { this[i] = 29 }
    }
    return this
}

function isDate(dtStr) {
    var daysInMonth = DaysArray(12)
    var pos1 = dtStr.indexOf(dtCh)
    var pos2 = dtStr.indexOf(dtCh, pos1 + 1)
    var strMonth = dtStr.substring(0, pos1)
    var strDay = dtStr.substring(pos1 + 1, pos2)
    var strYear = dtStr.substring(pos2 + 1)
    strYr = strYear
    if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)
    if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)
    for (var i = 1; i <= 3; i++) {
        if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
    }

    month = parseInt(strMonth)
    day = parseInt(strDay)
    year = parseInt(strYr)
    if (pos1 == -1 || pos2 == -1) {
        //alert("The date format should be : mm/dd/yyyy")
        return false
    }
    if (strMonth.length < 1 || month < 1 || month > 12) {
        alert("Please enter a valid month")
        return false
    }
    if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
        alert("Please enter a valid day")
        return false
    }
    if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
        alert("Please enter a valid 4 digit year between " + minYear + " and " + maxYear)
        return false
    }
    if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
        alert("Please enter a valid date")
        return false
    }
    //return true
}

function ValidateForm(txtDateCtl) {
    var dtval = document.getElementById(txtDateCtl).value;
    if (isDate(dtval) == false) {
        document.getElementById(txtDateCtl).value = '';
        //txtDateCtl.focus();
        return false;
    }
    return true;
}

///DATE VALIDATION ENDS HERE


//VALIDATE STATE AND ZIPCODE

function ValidateStateZip(source, args, country, state, zip) {

    var isValid = false;

    if (country != 205) {
        isValid = true;
    }
    else {
        var allstates = {
            AK: '9950099929',
            AL: '3500036999',
            AR: '7160072999,7550275505',
            AZ: '8500086599',
            CA: '9000096199',
            CO: '8000081699',
            CT: '0600006999',
            DC: '2000020099,2020020599',
            DE: '1970019999',
            FL: '3200033999,3410034999',
            GA: '3000031999,3981539837',
            GU: '9691096919,9692196923,9692596932',
            HI: '9670096798,9680096899',
            IA: '5000052999',
            ID: '8320083899',
            IL: '6000062999',
            IN: '4600047999',
            KS: '6600067999',
            KY: '4000042799,4527545275',
            LA: '7000071499,7174971749',
            MA: '0100002799',
            MD: '2033120331,2060021999',
            ME: '0380103801,0380403804,0390004999',
            MI: '4800049999',
            MN: '5500056799',
            MO: '6300065899',
            MS: '3860039799',
            MT: '5900059999',
            NC: '2700028999',
            ND: '5800058899',
            NE: '6800069399',
            NH: '0300003803,0380903899',
            NJ: '0700008999',
            NM: '8700088499',
            NV: '8900089899',
            NY: '0040000599,0639006390,0900014999',
            OH: '4300045999',
            OK: '7300073199,7340074999',
            OR: '9700097999',
            PA: '1500019699',
            PR: '0060000999',
            RI: '0280002999,0637906379',
            SC: '2900029999',
            SD: '5700057799',
            TN: '3700038599,7239572395',
            TX: '7330073399,7394973949,7500079999,8850188599',
            UT: '8400084799',
            VA: '2010520199,2030120301,2037020370,2200024699',
            VI: '0080100804,0082000824,0083000831,0084000841,0085000851,0080500805',
            VT: '0500005999',
            WA: '9800099499',
            WI: '4993649936,5300054999',
            WV: '2470026899',
            WY: '8200083199',
            AA: '3400034099',
            AE: '0900009899',
            AP: '9620096699',
            AS: '9679996799'
        };

        $.each(allstates, function (key, value) {
            if (key == state) {
                var zipRange = value.split(',');
                for (var i = 0; i < zipRange.length; i++) {
                    if (zip >= zipRange[i].substring(0, 5) && zip <= zipRange[i].substring(5)) {
                        isValid = true;
                    }
                }
            }
        });
    }

    args.IsValid = isValid;
}


function ValidateEmailAddress(email) {
    var emailRegxp = /\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
    return emailRegxp.test(email);
}