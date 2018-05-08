function isValidDate(dateString)
{
	// Validates that the input string is a valid date formatted as "mm/dd/yyyy"
	// First check for the pattern
	if(!/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(dateString))return false;
	// Parse the date parts to integers
	var parts = dateString.split("/");
	var day = parseInt(parts[1], 10);
	var month = parseInt(parts[0], 10);
	var year = parseInt(parts[2], 10);
	// Check the ranges of month and year
	if(year < 1000 || year > 3000 || month === 0 || month > 12)return false;
	var monthLength = [ 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 ];
	// Adjust for leap years
	if(year % 400 === 0 || (year % 100 !== 0 && year % 4 === 0))monthLength[1] = 29;
	// Check the range of the day
	return day > 0 && day <= monthLength[month - 1];
};

function fieldDateCheck(field2Check) {
	if (  isValidDate(field2Check.val())  ) {
		field2Check.removeClass("errorClass");
		return true;
	} else {
		field2Check.addClass("errorClass");
		return false;
	}
}

function isValidDateTwoYearMark(dateString) {
	var now = new Date();
	var twoYrAgo = new Date();
	twoYrAgo.setYear(now.getFullYear() - 2);
	twoYrAgo.setDate(twoYrAgo.getDate()-1);
	return (twoYrAgo <= new Date(dateString));
}

function fieldDateCheckTwoYearMark(field2Check) {
	if (  isValidDateTwoYearMark(field2Check.val())  ) {
		field2Check.removeClass("errorClass");
		return true;
	} else {
		field2Check.addClass("errorClass");
		return false;
	}
}

//TFS
function validateKmDate(field2Check) {
    var pass = "";
    if (!fieldDateCheck(field2Check)) { pass += "Improper date format\n\n"; }
    if (pass === "") {
        if (!fieldDateCheckTwoYearMark(field2Check)) { pass += "Date can not be older than two years ago\n\n"; }
    }
    return pass;
}