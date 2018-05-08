
///////////////////////////////////////////////////////////
// File:			Validation.js
// File Start Date:	July 30, 2002
// Description:		Routes validation to appropriate validation handler
//
// This file must be placed in to /inetpub/wwwroot/_vti_script

String.prototype.trim = function()
{
	return this.replace(/^\s*(\b.*\b|)\s*$/, "$1");
}

String.prototype.isDate = function()
{
	var re=/^((0?\d)|(1[0-2]))(\\|\/|-)((0?\d)|([1-2]\d)|(3[0-1]))(\\|\/|-)(\d{2}|\d{4})$/;
	return re.test(this);
}

String.prototype.isTime = function()
{
	var re=/^(((0?[1-9]|1[0-2])(:|\.)[0-5]\d((:|\.)[0-5]\d)?( )?(([aA]|[pP])[mM]))|((0?\d|1\d|2[0-3])(:|\.)[0-5]\d((:|\.)[0-5]\d)?))$/;
	return re.test(this);
}

String.prototype.isDateTime = function()
{
	var re=/^((0?\d)|(1[0-2]))(\\|\/|-)((0?\d)|([1-2]\d)|(3[0-1]))(\\|\/|-)(\d{2}|\d{4})( )(((0?[1-9]|1[0-2])(:|\.)[0-5]\d((:|\.)[0-5]\d)?( )?(([aA]|[pP])[mM]))|((0?\d|1\d|2[0-3])(:|\.)[0-5]\d((:|\.)[0-5]\d)?))$/;
	
	if (this.isDate()) return true;
	if (this.isTime()) return true;
	if (re.test(this)) return true;
	return false;
}

/*Replaced Array.indexOf function with exists(array, val)*/

function exists(array, val)
{
	for (var i=0; i<array.length; i++)
		if (array[i]==val) return i;
	return -1;
}

function IsInteger(val)
{
	if (val=='') return true;
	if (isNaN(parseFloat(val))) return false;
	if (parseInt(val) < parseFloat(val)) return false;
	if (val.lastIndexOf(".")==val.length-1) return false;
	return true;
}

function IsDouble(val)
{
	if (val=='') return true;
	if (isNaN(parseFloat(val))) return false;
	if (val.lastIndexOf(".")==val.length-1) return false;
	return true;
}

/* validation - KeyPress - converted to support all browsers*/


//*******************Number Validation Starts Here**************************

function checkKeyPressForInteger(control, oEvent, allownegative){

		var keyCode;
		
		if(window.event) {
			// for IE, e.keyCode or window.event.keyCode can be used
			keyCode = oEvent.keyCode; 
		}
		else if(oEvent.which)
		{	// for Netscape and Mozilla
			keyCode = oEvent.which;
		}

		//Minus is allowed only as first char
		if (allownegative==0 && keyCode == 45) {
			oEvent.returnValue = false;
			return false;
        }

		//only 0..9,-,backspace and non-keycode chars(del, arrows, ...)
		if((keyCode < 48 || keyCode > 57) && (keyCode != 45) && (keyCode != 8) && (keyCode != 0)) {
			oEvent.returnValue = false;
			return false;

        }   
        
        if (allownegative == 1 && keyCode == 45) 
        {
            if (control.value.substring(0, 1) == "-") {
                oEvent.returnValue = false;
                return false;
            }
        }
		
		
}

function validateInteger(control){
		if (control.value == "") {
		return true;
		}
		
		//alert(control.value.length)
		//check first letter for 0 
		if(control.value.length>1)
		{
			if(control.value.match(/^0/)!=null)
			{
			control.value=parseInt(control.value);
			return true;
			}
		}
		
		if (!IsInteger(control.value))
		{
		    alert("Only Integer values (0 to 9) are allowed");
		    control.focus();
		}
}

//*******************Number Validation Ends Here**************************



//*******************Decimal Validation Starts Here**************************
var err_Decimal='Decimal input error';
var err_Decimal_General='Entered value is not a valid decimal number';

function checkKeyPressForDecimal(control, oEvent) 
{
	
	var value = control.value;
	var keyCode;
	
	if(window.event) {
		// for IE, e.keyCode or window.event.keyCode can be used
		keyCode = oEvent.keyCode; 
	}
	else if(oEvent.which)
	{	// for Netscape and Mozilla
		keyCode = oEvent.which;
	}
	
	var decimalSeparator = control.getAttribute("decimalSeparator");
	var sepCode = decimalSeparator.charCodeAt(0);	

	//Decimal point is not allowed as first char
	if ((value.length == 0) && (keyCode == sepCode))	{
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
	
	
	if(value.length>7)
	{
		if((keyCode < 48 || keyCode > 57))
		{
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
	if (value!="") {
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
	var gs =  control.getAttribute("groupSeparator");

	var str = "";
	var sign = "";
	var nullDecimals = "";
	
	for (i=0;i<dg;i++){
		nullDecimals += "0";
	}
	if (num.charAt(0) == "-") {
		num = num.substr(1);
		sign = "-";
	}
	
	
	
	var DecPlace = num.indexOf(ds);
	if (DecPlace > 0) {
		str = "." + num.substr(DecPlace+1);
		num = num.substr(0, DecPlace);
	}
	num = formatDecimal_replace(num,gs,"");
	num = num + str;
	
	var zeros = "";
	if (num.substr(0,1)=="0") {
		var temp = Math.round(num * Math.pow(10,dg));
		temp = temp.toString();
		for (var i=temp.length;i<dg;i++) {
			zeros += "0";
		}
	}
	num = Math.round(num * Math.pow(10,dg));
	
	if (isNaN(num) == true) return handleDecimalError(control, err_Decimal_General);
	num = num.toString();

	if (num == "0") return ("0" + ds + nullDecimals);
	str = ds + zeros + num.substr(num.length - dg);
	
	num = num.substr(0, num.length-dg);
	
	
	
	//if (num == "") num = "0";
	//commented by vinayak on Aug 6th 5:00 PM  
	// it display the two decimal dot in some cases
	
	/*while (num.length > 2) {
		str = gs + num.substr(num.length - 2) + str;
		num = num.substr(0,num.length - 2);
	}*/
	str=num+str;
	return(sign + str);	
}

function handleDecimalError(control, error) {
	return "";
}

function formatDecimal_replace(string,text,by) {
    //Replaces text with by in string
    var i = string.indexOf(text), newstr = '';
    if ((!i) || (i == -1))
		return string;
    newstr += string.substring(0,i) + by;
    if (i+text.length < string.length)
			newstr += formatDecimal_replace(string.substring(i+text.length,string.length),text,by);
    return newstr;
}
//*************************Decimal Validation ends here************************


///DATE VALIDATION

// Declaring valid date character, minimum year and maximum year
var dtCh= "/";
var minYear=1900;
var maxYear=2100;

function isInteger(s){
	var i;
    for (i = 0; i < s.length; i++){   
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

function stripCharsInBag(s, bag){
	var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++){   
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}

function daysInFebruary (year){
	// February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
}
function DaysArray(n) {
	for (var i = 1; i <= n; i++) {
		this[i] = 31
		if (i==4 || i==6 || i==9 || i==11) {this[i] = 30}
		if (i==2) {this[i] = 29}
   } 
   return this
}

function isDate(dtStr){
	var daysInMonth = DaysArray(12)
	var pos1=dtStr.indexOf(dtCh)
	var pos2=dtStr.indexOf(dtCh,pos1+1)
	var strMonth=dtStr.substring(0,pos1)
	var strDay=dtStr.substring(pos1+1,pos2)
	var strYear=dtStr.substring(pos2+1)
	strYr=strYear
	if (strDay.charAt(0)=="0" && strDay.length>1) strDay=strDay.substring(1)
	if (strMonth.charAt(0)=="0" && strMonth.length>1) strMonth=strMonth.substring(1)
	for (var i = 1; i <= 3; i++) {
		if (strYr.charAt(0)=="0" && strYr.length>1) strYr=strYr.substring(1)
	}
	month=parseInt(strMonth)
	day=parseInt(strDay)
	year=parseInt(strYr)
	if (pos1==-1 || pos2==-1){
		//alert("The date format should be : mm/dd/yyyy")
		return false
	}
	if (strMonth.length<1 || month<1 || month>12){
		alert("Please enter a valid month")
		return false
	}
	if (strDay.length<1 || day<1 || day>31 || (month==2 && day>daysInFebruary(year)) || day > daysInMonth[month]){
		alert("Please enter a valid day")
		return false
	}
	if (strYear.length != 4 || year==0 || year<minYear || year>maxYear){
		alert("Please enter a valid 4 digit year between "+minYear+" and "+maxYear)
		return false
	}
	if (dtStr.indexOf(dtCh,pos2+1)!=-1 || isInteger(stripCharsInBag(dtStr, dtCh))==false){
		alert("Please enter a valid date")
		return false
	}
//return true
}

function ValidateForm(txtDateCtl)
{
	var dtval=document.getElementById(txtDateCtl).value;
	if (isDate(dtval)==false)
	{
		document.getElementById(txtDateCtl).value='';
		//txtDateCtl.focus();
		return false;
	}
    return true;
}
 
///DATE VALIDATION ENDS HERE