function validate()
{
	try
	{
		var bSuccess = true;
		
		var strRequired = ""; 
		var strDatatype = ""; 
		
		for(i=0;i<fv.length;i++)
		{
			var arr = fv[i].split("|");

			var ctl = getobj(arr[0]);
			var IsRequired = arr[1];
			var dtype = arr[2];
			var dn = arr[3];
			
			if (IsRequired == 1)
			{ 
				if (!checkRequired(ctl))
				{
					bSuccess = false;
					strRequired += dn + "\n";
				}
			}
			
			if (dtype == 'NUMBER')
			{
				if (!IsN(ctl.value) && !IsD(ctl.value))
				{
					bSuccess = false;
					strDatatype += dn + " should be a number.\n";
				}
			}
			
			if (dtype == 'DATE' && ctl.type=='text')
			{
				if (!isDate(ctl.value))
				{
					bSuccess = false;
					strDatatype += dn + " should be a date.\n";
				}
			}
		}
		
		if (!bSuccess)
		{
			if (strRequired != "") 
				strRequired = "These fields cannot be empty.\n\n" + strRequired + "\n\n";

			if (strDatatype != "") 
				strDatatype = "Invalid data.\n\n" + strDatatype;
				
			alert(strRequired + strDatatype)
		}
	}
	catch(err)
	{
		txt="There was an error on this page.\n\n"
		txt+="Error description: " + err.description + "\n\n"
		alert(txt);
		bSuccess = false;
	}
	
	return bSuccess;
}

function getobj(id) 
{
  if (document.all && !document.getElementById) 
    obj = eval('document.all.' + id);
  else if (document.layers) 
    obj = eval('document.' + id);
  else if (document.getElementById) 
    obj = document.getElementById(id);

  return obj;
}

function checkRequired(ctrl)
{
	if (ctrl.type != 'radio' && ctrl.type != 'checkbox')
	{
		if (trim(ctrl.value) == '')
			return false;
		else
			return true;	
	}
	else
	{
		var col = document.getElementsByName(ctrl.name)

		for(index=0;index<col.length;index++)
		{
			if (col[index].checked)
				return true;
		}
		return false;
	}
}

function IsN(val) 
{ 
	if (trim(val) != "")  
		return !/\D/.test(val)
	else
		return true;
}

function IsD(val) 
{ 
	if (trim(val) != "")  
		return /^\d+\.\d+$/.test(val)
	else
		return true;
}

// Removes leading whitespaces
function LTrim( value ) {
	
	var re = /\s*((\S+\s*)*)/;
	return value.replace(re, "$1");
	
}

// Removes ending whitespaces
function RTrim( value ) {
	
	var re = /((\s*\S+)*)\s*/;
	return value.replace(re, "$1");
	
}

// Removes leading and ending whitespaces
function trim( value ) {
	return LTrim(RTrim(value));
}


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
	
	var decimalSeparator = ".";
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
/* date validation */

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
	
	if (trim(dtStr) != "") 
	{
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
			alert("Please enter a valid month");
			return false;
		}
		if (strDay.length<1 || day<1 || day>31 || (month==2 && day>daysInFebruary(year)) || day > daysInMonth[month]){
			alert("Please enter a valid day");
			return false;
		}
		if (strYear.length != 4 || year==0 || year<minYear || year>maxYear){
			alert("Please enter a valid 4 digit year between "+minYear+" and "+maxYear);
			return false;
		}
		if (dtStr.indexOf(dtCh,pos2+1)!=-1 || isInteger(stripCharsInBag(dtStr, dtCh))==false){
			alert("Please enter a valid date");
			return false;
		}
	}
	return true;
}

document.write("\<script language=\"Javascript\" src=\"http://email.ecn5.com/engines/PrePopForms.aspx"+window.location.search+"\"\>\</script\>");


