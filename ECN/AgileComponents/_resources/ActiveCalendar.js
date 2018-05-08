// Active Calendar v2 Server Control for ASP.NET
// Copyright (c) 2004 Active Up - http://www.activeup.com/?r=acl2

ACL_ver = navigator.appName;
ACL_num = ACL_ParseStringToInt(navigator.appVersion);
ACL_os = navigator.platform;
ACL_ng = (document.getElementById) ? true : false;
ACL_ns = (document.layers) ? true : false;
ACL_ie = (document.all) ? true : false;
ACL_ieVer = getInternetExplorerVersion();
ACL_topZIndex = 0;
ACL_backupStyle = new ACL_Style();
ACL_selectedDateOver = null;


function getInternetExplorerVersion() {
    var rv = -1; // Return value assumes failure.
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}

// Display the calendar
function ACL_ShowCalendar(id) {
    var pickup = document.getElementById(id + '_pickupText');
    var layerTarget = ACL_LayersTarget(id);
    var completeCal = document.getElementById(id + '_completeCalendar');
    if (ACL_ng) {
        //layerTarget.style.left = parseInt(completeCal.offsetLeft) + 'px';
        //layerTarget.style.top = parseInt(completeCal.offsetTop) + parseInt(pickup.offsetHeight) + parseInt(pickup.offsetTop) + 'px';
        layerTarget.style.left = ACL_FindPosX(pickup) + 'px';
        layerTarget.style.top = ACL_FindPosY(pickup) + parseInt(pickup.offsetHeight) + 2 + 'px';
        layerTarget.style.visibility = 'visible';

    }
    else if (ACL_ns) {
        layerTarget.visibility = 'show';
    }
    else if (ACL_ie) {
        //layerTarget.style.left = completeCal.offsetLeft;
        //layerTarget.style.top = completeCal.offsetTop + pickup.offsetHeight + pickup.offsetTop;
        layerTarget.style.left = ACL_FindPosX(pickup) + 'px';
        layerTarget.style.top = ACL_FindPosY(pickup) + parseInt(pickup.offsetHeight) + 2 + 'px';
        layerTarget.style.visibility = 'visible';
    }

    layerTarget.style.zIndex = parseFloat(layerTarget.style.zIndex) + 10000;
    layerTarget.style.zIndex = parseFloat(layerTarget.style.zIndex) + parseFloat(ACL_topZIndex);
    document.getElementById(id + '_topZIndex').value = ACL_topZIndex;
    ACL_topZIndex++;

    if (ACL_ie) {
        var mask = document.getElementById(id + '_mask');
        mask.style.width = layerTarget.clientWidth;
        mask.style.height = layerTarget.clientHeight;
        //mask.style.top = completeCal.offsetTop + pickup.offsetHeight + pickup.offsetTop;
        //mask.style.left = completeCal.offsetLeft;
        mask.style.left = ACL_FindPosX(pickup) + 'px';
        mask.style.top = ACL_FindPosY(pickup) + parseInt(pickup.offsetHeight) + 2 + 'px';
        mask.style.zIndex = layerTarget.style.zIndex - 1;
        mask.style.display = 'block';
    }

    layerTarget.style.display = 'block';
}

function ACL_SetZIndex(id, value) {
    var layerTarget = ACL_LayersTarget(id);
    layerTarget.style.zIndex = value;
}

// hide the calendar
function ACL_HideCalendar(id, init) {
    var layerTarget = ACL_LayersTarget(id);
    if (ACL_ng) {
        //layerTarget.style.display = 'none';
        layerTarget.style.visibility = 'hidden';
    }
    else if (ACL_ns) layerTarget.visibility = 'hide';
    else if (ACL_ie) layerTarget.visibility = 'hiden';

    if (!init) {
        layerTarget.style.zIndex = parseFloat(layerTarget.style.zIndex) - 10000;
        layerTarget.style.zIndex = parseFloat(layerTarget.style.zIndex) - parseFloat(document.getElementById(id + '_topZIndex').value);
        if (ACL_ie) {
            var mask = document.getElementById(id + '_mask');
            mask.style.display = 'none';
        }
    }
}

// indicate if the calendar is visible or not
function ACL_IsVisible(id) {
    var layerTarget = ACL_LayersTarget(id);
    if (ACL_ng)
        if (layerTarget.style.visibility == 'visible')
        return 'True';
    else return 'False';

    else if (ACL_ns)
        if (layerTarget.visibility == 'show')
        return 'True';
    else return 'False';

    else if (ACL_ie)
        if (layerTarget.visibility == 'visible')
        return 'True';
    else return 'False';

    else return 'False';
}

// show or hide the calendar
function ACL_ShowHideCalendar(id) {
    if (ACL_IsVisible(id) == 'True')
        ACL_HideCalendar(id, false);
    else
        ACL_ShowCalendar(id);
}

function ACL_LayersTarget(id) {
    if (ACL_ng) iLayer = eval(document.getElementById(id + '_div'));
    else if (ACL_ns) iLayer = eval('document.' + divName);
    else if (ACL_ie) iLayer = eval(divName + '.style');
    return iLayer;
}

// Get the max days in the specified month with leap year check
function ACL_MaxDays(year, month) {
    if (month == 2)
        return (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) ? 29 : 28;
    else
        return (month == 4 || month == 6 || month == 9 || month == 11) ? 30 : 31;
}

// Check if the specified date is valid.
function ACL_IsDateValid(year, month, day) {
    var today = new Date();
    year = ((!year) ? ACL_Y2k(today.getYear()) : year);
    month = ((!month) ? today.getMonth() : month - 1);
    if (!day) return false
    var test = new Date(year, month, day);
    if ((ACL_Y2k(test.getYear()) == year) &&
		(month == test.getMonth()) &&
		(day == test.getDate()))
        return true;
    else
        return false
}

function ACL_ChangeSelectedDay(id, year, month, day, deselectIfSelected) {
    selectedMonth = ACL_GetSelectedMonth(id)

    document.getElementById(id + '_day').value = day;
    document.getElementById(id + '_month').value = month;
    document.getElementById(id + '_year').value = year;

    var multiSelection = eval(id + '_multiSelection');
    if (multiSelection == 'True') {
        var dayToSelected = 1, monthToRender = 1;
        if (day < 10) dayToSelected = '0' + day;
        else dayToSelected = day;
        if (month < 10) monthToSelected = '0' + month;
        else monthToSelected = month;
        var dateToSelected = year + '/' + monthToSelected + '/' + dayToSelected;

        var selectedDates = document.getElementById(id + '_selectedDates').value;
        if (selectedDates.indexOf(dateToSelected) == -1) {
            document.getElementById(id + '_selectedDates').value = selectedDates.concat(dateToSelected + ';');
        }
        else if (deselectIfSelected == true) {
            document.getElementById(id + '_selectedDates').value = ACL_RemoveDateInSelecteDate(selectedDates, dateToSelected);
        }
    }

    navigationChangeDate = eval(id + '_navigationChangeDate');
    if (navigationChangeDate == 'True') {
        if ((month < selectedMonth && !(selectedMonth == 12 && month == 1)) || (selectedMonth == 1 && month == 12))
            ACL_ChangeMonth(id, -1);
        else if (month > selectedMonth || (selectedMonth == 12 && month == 1))
            ACL_ChangeMonth(id, +1);
        else
            ACL_RenderCalendar(id);
    }
    else {
        ACL_RenderCalendar(id);
    }

    if (ACL_UseDayOver(id) == true)
        ACL_selectedDateOver = new Date(year, month, day);
}

// Selected the day
function ACL_SelectDay(id, year, month, day) {
    ACL_ChangeSelectedDay(id, year, month, day, true);
    //$DAYCLICKED$
    ACL_OnClickClientSide(id);

    var doPostBack = document.getElementById(id + '_doPostBackWhenClick').value;
    if (doPostBack == 'True')
        __doPostBack(id, '');

    var useDatePicker = eval(id + '_useDatePicker');
    if (useDatePicker == 'True') {
        var selDate = ACL_GetSelectedDate(id);
        var dateFormated = '';
        if (eval(id + '_useCustomDateFormat') == 'True') {
            var customFormat = eval(id + '_customDateFormat');

            if (day < 10) day = '0' + day;
            if (month < 10) month = '0' + month;
            dateFormatedNorm = day + '/' + month + '/' + year;

            dateFormated = ACL_FormatDateCustom(id, new Date(ACL_GetDateFromFormat(id, dateFormatedNorm, 'dd/mm/yyyy')), customFormat);
        }
        else
            dateFormated = ACL_FormatDate(selDate, eval(id + '_dateFormatLocale'));

        var useTime = eval(id + '_useTime');
        if (useTime == 'True') {
            var hour = ACL_GetSelectedOptionText(id + '_hour');
            var min = ACL_GetSelectedOptionText(id + '_minute');

            if ((hour != null && hour != '') && (min != null && min != '')) {
                dateFormated += ' ' + hour + ':' + min;
            }
        }

        document.getElementById(id + '_pickupText').value = dateFormated;
        ACL_HideCalendar(id, false);
    }
}

function ACL_OnClickClientSide(id) {
    var onClickCode = eval(id + '_onClickClientSide');
    if (onClickCode != null && onClickCode != '')
        window.setTimeout(eval(id + '_onClickClientSide'), 1);
    return false;
}

function ACL_OnBlurPickerClientSide(id) {
    var onBlurPicker = eval(id + '_onBlurPickerClientSide');
    if (onBlurPicker != null && onBlurPicker != '')
        window.setTimeout(eval(id + '_onBlurPickerClientSide'), 1);
    return false;
}

function ACL_RemoveDateInSelecteDate(s, stringToRemove) {
    var reg = new RegExp('[;]+', 'g');
    var tab = s.split(reg);
    var res = '';
    for (var i = 0; i < tab.length; i++) {
        if (tab[i] != stringToRemove) {
            res = res.concat(tab[i]);
            res = res.concat(';');
        }
    }

    return res;
}

function ACL_FormatDate(date, format) {
    var day = date.getDate();
    var month = date.getMonth(); month++;
    var year = date.getYear(); if (year < 1900) year += 1900;
    var formatedDate = '';

    switch (format) {
        case 'en':
            {
                formatedDate = month + '/' + day + '/' + year;
            } break;

        case 'de':
            {
                if (day < 10) day = '0' + day;
                if (month < 10) month = '0' + month;
                formatedDate = day + '.' + month + '.' + year;
            } break;

        case 'en_GB':
        case 'fr':
        case 'it':
        case 'es':
            {
                if (day < 10) day = '0' + day;
                if (month < 10) month = '0' + month;
                formatedDate = day + '/' + month + '/' + year;
            } break;

        case 'pt':
            {
                if (day < 10) day = '0' + day;
                if (month < 10) month = '0' + month;
                formatedDate = day + '-' + month + '-' + year;
            } break;

        default: break;
    }

    return formatedDate;
}

function ACL_GetSelectedDate(id) {
    var y = ACL_GetSelectedYear(id);
    var m = ACL_GetSelectedMonth(id);
    var d = document.getElementById(id + '_day').value;
    return new Date(y, m - 1, d);
}

function ACL_ResetSelection(id) {
    var now = new Date();

    document.getElementById(id + '_day').value = now.getDate();
    document.getElementById(id + '_month').value = now.getMonth() - 1;
    document.getElementById(id + '_year').value = now.getYear();
}

// Change the month and adjust selected year if needed
function ACL_ChangeMonth(id, factor) {
    // Calculate the new date
    selected = new Date(ACL_GetSelectedYear(id), ACL_GetSelectedMonth(id) - 1, 1);

    newdate = selected.setMonth(selected.getMonth() + factor);

    // Calculate the new index
    yearelement = document.getElementById(id + '_year_selector');
    newindex = yearelement.selectedIndex + (selected.getFullYear() - ACL_GetSelectedYear(id));

    // If the new index is valid, proceed to the change
    if (newindex >= 0 && newindex < yearelement.length) {
        monthelement = document.getElementById(id + '_month_selector');
        monthelement[selected.getMonth()].selected = true;
        yearelement[newindex].selected = true;

    }

    // Render the calendar
    ACL_RenderCalendar(id);
}

// Get the selected month
function ACL_GetSelectedMonth(id) {
    monthelement = document.getElementById(id + '_month_selector');
    return monthelement[monthelement.selectedIndex].value;
}

// Get the selected year
function ACL_GetSelectedYear(id) {
    yearelement = document.getElementById(id + '_year_selector');
    return yearelement[yearelement.selectedIndex].value;
}

function ACL_ChangeToTodayDate(id) {
    var monthelement = document.getElementById(id + '_month_selector');
    var yearelement = document.getElementById(id + '_year_selector');
    var now = new Date();
    var currentMonth = now.getMonth() + 1, currentYear = now.getYear(); if (currentYear < 1900) currentYear += 1900;
    var year = now.getYear(); if (year < 1900) year += 1900;

    var factor = 0;

    var result = yearelement.value - currentYear;
    if (result != 0) {
        factor -= result * 12;
    }

    result = monthelement.value - currentMonth;
    if (result != 0) {
        factor -= result;
    }

    if (factor != 0)
        ACL_ChangeMonth(id, factor);

    ACL_SelectDay(id, year, now.getMonth() + 1, now.getDate());

}

function ACL_SetCalendarAbsolute(id) {
    var calendar = document.getElementById(id + '_div');
    if (calendar != null)
        calendar.style.position = 'absolute';
}

// Render the calendar
function ACL_RenderCalendar(id) {
    // Initialize variables
    var day = 1;
    var month = ACL_GetSelectedMonth(id);
    var year = ACL_GetSelectedYear(id);

    var selectedDay = document.getElementById(id + '_day').value;
    var selectedMonth = document.getElementById(id + '_month').value;
    var selectedYear = document.getElementById(id + '_year').value;
    var dayMax = ACL_MaxDays(year, month);
    var dayWeek = ACL_GetDayPos(id, ACL_DayOfWeek(year, month, 1));
    var weekNumberObject = null;
    var weekNumberYearFrom = null;
    var weekNumberMonthFrom = null;
    var weekNumberDayFrom = null;
    var multiSelect = eval(id + '_multiSelection');

    if (selectedDay > dayMax)
        selectedDay = dayMax;

    dayNext = 1;
    todayDate = new Date();

    showDay = 1;
    showMonth = 1;

    // Render the days
    for (rowIndex = 0; rowIndex < 6; rowIndex++) {
        var showWeekNumber = eval(id + '_showWeekNumber');
        if (showWeekNumber == 'True') {
            element = document.getElementById(id + '_wn' + (rowIndex));

            var dayForWeekNumber = 1;
            var monthForWeekNumber = 1;
            var yearForWeekNumber = year;

            if (rowIndex == 0) {
                dayForWeekNumber = 7 - dayWeek;
                monthForWeekNumber = month - 1;
            }

            else if (showDay + 7 <= dayMax && showMonth == month) {
                dayForWeekNumber = showDay + 7;
                monthForWeekNumber = showMonth - 1;
            }

            else if (showDay + 7 > dayMax && showMonth == month) {
                dayForWeekNumber = (showDay + 7) - dayMax;
                monthForWeekNumber = showMonth;
            }

            else {
                dayForWeekNumber = showDay + 7;
                monthForWeekNumber = showMonth - 1;
            }

            var weekNumber = ACL_GetWeekNumber(yearForWeekNumber, monthForWeekNumber, dayForWeekNumber);

            element.innerHTML = weekNumber;

            weekNumberObject = element;
        }

        for (colIndex = 0; colIndex < 7; colIndex++) {
            element = document.getElementById(id + '_d' + (rowIndex) + (colIndex));
            var isOtherMonthDay = 'True';

            // The day is in the Selected month
            if (day <= dayMax && !(rowIndex == 0 && colIndex < dayWeek)) {
                isOtherMonthDay = 'False';

                var dayStyle = eval(id + '_dayStyle');

                showBackColor = ACL_GetBackgroundColor(dayStyle);
                showForeColor = ACL_GetForeColor(dayStyle);
                showBackImg = ACL_GetBackgroundImage(dayStyle);

                showDay = day;
                showMonth = month;
                showYear = year;
                day++
            }
            // The day is in the previous month
            else if (rowIndex == 0) {
                var otherMonthDayStyle = eval(id + '_otherMonthDayStyle');

                showBackColor = ACL_GetBackgroundColor(otherMonthDayStyle);
                showForeColor = ACL_GetForeColor(otherMonthDayStyle);
                showBackImg = ACL_GetBackgroundImage(otherMonthDayStyle);

                showDay = (ACL_MaxDays(month == 1 ? year - 1 : year, month == 1 ? 12 : month - 1) - (dayWeek - colIndex - 1));
                showMonth = month == 1 ? 12 : month - 1;
                showYear = month == 1 ? year - 1 : year;
            }
            // The day is in the next month
            else if (rowIndex != 0) {
                var otherMonthDayStyle = eval(id + '_otherMonthDayStyle');

                showBackColor = ACL_GetBackgroundColor(otherMonthDayStyle);
                showForeColor = ACL_GetForeColor(otherMonthDayStyle);
                showBackImg = ACL_GetBackgroundImage(otherMonthDayStyle);

                showDay = dayNext;
                showMonth = month == 12 ? 1 : (month - 1 + 2);
                showYear = month == 12 ? year - 1 + 2 : year;
                dayNext++;
            }

            if (multiSelect == 'True') {
                if (colIndex == 0) {
                    weekNumberYearFrom = showYear;
                    weekNumberMonthFrom = showMonth;
                    weekNumberDayFrom = showDay;

                }

                else if (colIndex == 6) {
                    var weekStyle = eval(id + '_dayStyle');
                    weekForeColor = ACL_GetForeColor(weekStyle);

                    if (weekNumberObject != null) {
                        weekNumber = weekNumberObject.innerHTML;
                        //weekNumberObject.innerHTML = '<a style="text-decoration:none" href="javascript:ACL_SelectWeek(\'' + id + '\',' + weekNumberYearFrom + ',' + weekNumberMonthFrom + ',' + weekNumberDayFrom + ')"><font color=' + weekForeColor + '>' + weekNumber + '</font></a>';	
                        weekNumberObject.innerHTML = '<font color=' + weekForeColor + '>' + weekNumber + '</font>';
                        if (ACL_ie)
                            weekNumberObject.style.cursor = 'hand';
                        else
                            weekNumberObject.style.cursor = 'pointer';
                        weekNumberObject.weekNumberYearFrom = weekNumberYearFrom;
                        weekNumberObject.weekNumberMonthFrom = weekNumberMonthFrom;
                        weekNumberObject.weekNumberDayFrom = weekNumberDayFrom;
                        weekNumberObject.calendarID = id;
                        if (ACL_ie && ACL_ieVer < 8)
                            weekNumberObject.setAttribute('onclick', function() { ACL_SelectWeek('' + this.calendarID + '', this.weekNumberYearFrom, this.weekNumberMonthFrom, this.weekNumberDayFrom); });
                        else
                            weekNumberObject.onclick = function() { ACL_SelectWeek('' + this.calendarID + '', this.weekNumberYearFrom, this.weekNumberMonthFrom, this.weekNumberDayFrom); };
                    }
                    weekNumberYearFrom = null;
                    weekNumberMonthFrom = null;
                    weekNumberDayFrom = null;
                }
            }

            // Set the color of the weekend
            if (colIndex == ACL_GetDayPos(id, 6) || colIndex == ACL_GetDayPos(id, 0)) {
                var weekEndDayStyle = eval(id + '_weekEndDayStyle');

                showBackColor = ACL_GetBackgroundColor(weekEndDayStyle);
                if (isOtherMonthDay == 'False')
                    showForeColor = ACL_GetForeColor(weekEndDayStyle);
                showBackImg = ACL_GetBackgroundImage(weekEndDayStyle);
            }


            if (showBackImg != null)
                element.style.backgroundImage = showBackImg;
            else
                element.style.background = showBackColor;

            // Check if this is the current date

            if (showMonth == month && showDay == todayDate.getDate() && month == (todayDate.getMonth() - 1 + 2) && year == todayDate.getFullYear()) {
                var todayDayStyle = eval(id + '_todayDayStyle');

                showBackColor = ACL_GetBackgroundColor(todayDayStyle);
                showForeColor = ACL_GetForeColor(todayDayStyle);
                showBackImg = ACL_GetBackgroundImage(todayDayStyle);

                if (showBackImg != null)
                    element.style.backgroundImage = showBackImg;
                else
                    element.style.background = showBackColor;
            }

            // Check if this is the selected day or not

            navigationChangeDate = eval(id + '_navigationChangeDate');

            var dayToRender = 1, monthToRender = 1;
            if (showDay < 10) dayToRender = '0' + showDay;
            else dayToRender = showDay;
            if (showMonth < 10) monthToRender = '0' + showMonth;
            else monthToRender = showMonth;
            var dateToRender = showYear + '/' + monthToRender + '/' + dayToRender;

            var isSelected = 'False';
            if (multiSelect == 'False') {
                if (navigationChangeDate == 'False') {
                    if (selectedDay == showDay && document.getElementById(id + '_month').value == showMonth && document.getElementById(id + '_year').value == showYear) {

                        var selectedDayStyle = eval(id + '_selectedDayStyle');

                        showBackColor = ACL_GetBackgroundColor(selectedDayStyle);
                        showForeColor = ACL_GetForeColor(selectedDayStyle);
                        showBackImg = ACL_GetBackgroundImage(selectedDayStyle);

                        if (showBackImg != null)
                            element.style.backgroundImage = showBackImg
                        else
                            element.style.background = showBackColor;

                        isSelected = 'True';
                    }
                }
                else {
                    if (selectedDay == showDay && showMonth == month) {
                        var selectedDayStyle = eval(id + '_selectedDayStyle');

                        showBackColor = ACL_GetBackgroundColor(selectedDayStyle);
                        showForeColor = ACL_GetForeColor(selectedDayStyle);
                        showBackImg = ACL_GetBackgroundImage(selectedDayStyle);

                        if (showBackImg != null)
                            element.style.backgroundImage = showBackImg
                        else
                            element.style.background = showBackColor;

                        // Store the day value in the hidden field
                        document.getElementById(id + '_day').value = showDay;
                        document.getElementById(id + '_month').value = showMonth;
                        document.getElementById(id + '_year').value = showYear;

                        isSelected = 'True';
                    }
                }
            }

            else {
                var selectedDates = document.getElementById(id + '_selectedDates').value;
                if (selectedDates.indexOf(dateToRender, 0) != -1) {
                    var selectedDayStyle = eval(id + '_selectedDayStyle');
                    showBackColor = ACL_GetBackgroundColor(selectedDayStyle);
                    showForeColor = ACL_GetForeColor(selectedDayStyle);
                    showBackImg = ACL_GetBackgroundImage(selectedDayStyle);

                    if (showBackImg != null)
                        element.style.backgroundImage = showBackImg
                    else
                        element.style.background = showBackColor;

                    isSelected = 'True';
                }
            }

            // Render the day
            var styleDates = eval(id + '_styleDates');
            var blockedDates = eval(id + '_blockedDates');


            if (ACL_UseDayOver(id) == true) {

                if (ACL_ng && (ACL_ie == false)) {
                    element.addEventListener("mouseover", function(e) {
                        ACL_OverDay(id, '' + this.id + '', this.showYear, this.showMonth, this.showDay);
                        ACL_OverDayToolTip(id, '' + this.id + '', this.showYear, this.showMonth, this.showDay);
                    }, true);

                    element.addEventListener("mouseout", function(e) {

                        ACL_OutDay('' + this.id + '', this.showYear, this.showMonth, this.showDay);
                        ACL_OutDayToolTip(id);

                    }, true);

                }
                else if (ACL_ieVer < 8) {
                    element.setAttribute('onmouseover', function() { ACL_OverDay(id, '' + this.id + '', this.showYear, this.showMonth, this.showDay); ACL_OverDayToolTip(id, '' + this.id + '', this.showYear, this.showMonth, this.showDay); });
                    element.setAttribute('onmouseout', function() { ACL_OutDay('' + this.id + '', this.showYear, this.showMonth, this.showDay); ACL_OutDayToolTip(id); });
                }
            }

            else {
                if (ACL_ng && ACL_ie == false) {
                    element.addEventListener("mouseover", function(e) {
                        ACL_OverDayToolTip(id, '' + this.id + '', this.showYear, this.showMonth, this.showDay);

                    }, true);

                    element.addEventListener("mouseout", function(e) {
                        ACL_OutDayToolTip(id);

                    }, true);
                }
                else if (ACL_ieVer < 8) {
                    element.setAttribute('onmouseover', function() { ACL_OverDayToolTip(id, '' + this.id + '', this.showYear, this.showMonth, this.showDay); });
                    element.setAttribute('onmouseout', function() { ACL_OutDayToolTip(id); });
                }
            }

            if (Date.parse(dateToRender) < Date.parse(document.getElementById(id + '_minDate').value) || Date.parse(dateToRender) > Date.parse(document.getElementById(id + '_maxDate').value)) {
                var blockedDayStyle = eval(id + '_blockedDayStyle');

                showBackColor = ACL_GetBackgroundColor(blockedDayStyle);
                showForeColor = ACL_GetForeColor(blockedDayStyle);
                showBackImg = ACL_GetBackgroundImage(blockedDayStyle);

                if (showBackImg != null)
                    element.style.backgroundImage = showBackImg
                else
                    element.style.background = showBackColor;

                //element.innerHTML = '<font color=' + showForeColor + '>' + showDay + '</font>'
                element.style.foreColore = showForeColor;
                element.childNodes[0].childNodes[0].nodeValue = showDay;
            }

            else if (isSelected == 'True') {
                //element.innerHTML = '<font color=' + showForeColor + '>' + showDay + '</font>';
                element.style.foreColore = showForeColor;
                element.childNodes[0].childNodes[0].nodeValue = showDay;
                if (ACL_ie)
                    element.style.cursor = 'hand';
                else
                    element.style.cursor = 'pointer';
                element.showYear = showYear;
                element.showMonth = showMonth;
                element.showDay = showDay;
                element.calendarID = id;
                if (ACL_ie && ACL_ieVer < 8)
                    element.setAttribute('onclick', function() { ACL_SelectDay('' + this.calendarID + '', this.showYear, this.showMonth, this.showDay); });
                else
                    element.onclick = function() { ACL_SelectDay('' + this.calendarID + '', this.showYear, this.showMonth, this.showDay); };

                //element.innerHTML = '<a style="text-decoration:none" href="javascript:ACL_SelectDay(\'' + id + '\',' + showYear + ',' + showMonth + ', ' + showDay + ')"><font color=' + showForeColor + '>' + showDay + '</font></a>';	
            }

            else {

                if (styleDates.indexOf(dateToRender, 0) != -1) {
                    var ndxstart = styleDates.indexOf(dateToRender);
                    var ndxend = styleDates.indexOf(',', ndxstart);
                    var styledate = styleDates.substring(ndxstart, ndxend);
                    var style = styledate.substring(styledate.indexOf(';') + 1);

                    showBackColor = ACL_GetBackgroundColor(style);
                    showForeColor = ACL_GetForeColor(style);
                    showBackImg = ACL_GetBackgroundImage(style);

                    if (showBackImg != null)
                        element.style.backgroundImage = showBackImg
                    else
                        element.style.background = showBackColor;

                    if (blockedDates.indexOf(dateToRender, 0) == -1) {
                        //element.innerHTML = '<a style="text-decoration:none" href="javascript:ACL_SelectDay(\'' + id + '\',' + showYear + ',' + showMonth + ', ' + showDay + ')"><font color=' + showForeColor + '>' + showDay + '</font></a>';
                        //element.innerHTML = '<font color=' + showForeColor + '>' + showDay + '</font>';
                        element.style.foreColore = showForeColor;
                        element.childNodes[0].childNodes[0].nodeValue = showDay;
                        if (ACL_ie)
                            element.style.cursor = 'hand';
                        else
                            element.style.cursor = 'pointer';
                        element.showYear = showYear;
                        element.showMonth = showMonth;
                        element.showDay = showDay;
                        element.calendarID = id;
                        if (ACL_ie && ACL_ieVer < 8)
                            element.setAttribute('onclick', function() { ACL_SelectDay('' + this.calendarID + '', this.showYear, this.showMonth, this.showDay); });
                        else
                            element.onclick = function() { ACL_SelectDay('' + this.calendarID + '', this.showYear, this.showMonth, this.showDay); };
                    }
                    else {
                        element.style.cursor = 'default';
                        //element.innerHTML = '<font color=' + showForeColor + '>' + showDay + '</font>'
                        element.style.foreColore = showForeColor;
                        element.childNodes[0].childNodes[0].nodeValue = showDay;
                    }
                }

                else if (blockedDates.indexOf(dateToRender, 0) != -1) {
                    var blockedDayStyle = eval(id + '_blockedDayStyle');

                    showBackColor = ACL_GetBackgroundColor(blockedDayStyle);
                    showForeColor = ACL_GetForeColor(blockedDayStyle);
                    showBackImg = ACL_GetBackgroundImage(blockedDayStyle);

                    if (showBackImg != null)
                        element.style.backgroundImage = showBackImg
                    else
                        element.style.background = showBackColor;

                    element.style.cursor = 'default';

                    //element.innerHTML = '<font color=' + showForeColor + '>' + showDay + '</font>'
                    element.style.foreColore = showForeColor;
                    element.childNodes[0].childNodes[0].nodeValue = showDay;
                }

                else {
                    //element.innerHTML = '<a style="text-decoration:none" href="javascript:ACL_SelectDay(\'' + id + '\',' + showYear + ',' + showMonth + ', ' + showDay + ')"><font color=' + showForeColor + '>' + showDay + '</font></a>';
                    //element.innerHTML = '<font color=' + showForeColor + '>' + showDay + '</font>';
                    element.style.foreColore = showForeColor;
                    element.childNodes[0].childNodes[0].nodeValue = showDay;
                    if (ACL_ie)
                        element.style.cursor = 'hand';
                    else
                        element.style.cursor = 'pointer';
                    element.showYear = showYear;
                    element.showMonth = showMonth;
                    element.showDay = showDay;
                    element.calendarID = id;
                    if (ACL_ie && ACL_ieVer < 8)
                        element.setAttribute('onclick', function() { ACL_SelectDay('' + this.calendarID + '', this.showYear, this.showMonth, this.showDay); });
                    else
                        element.onclick = function() { ACL_SelectDay('' + this.calendarID + '', this.showYear, this.showMonth, this.showDay); };
                }


            }

        }
    }
}

function ACL_GetBackgroundColor(style) {
    var backgroundcolorID = 'background-color:';

    var firstpos = style.indexOf(backgroundcolorID, 0);
    if (firstpos == -1) return null;

    var lastpos = style.indexOf(';', firstpos);
    if (lastpos == -1) return null;

    return style.substring(firstpos + backgroundcolorID.length, lastpos);
}

function ACL_GetBackgroundImage(style) {
    var backgroundimageID = 'background-image:';

    var firstpos = style.indexOf(backgroundimageID, 0);
    if (firstpos == -1) return null;

    var lastpos = style.indexOf(';', firstpos);
    if (lastpos == -1) return null;

    return style.substring(firstpos + backgroundimageID.length, lastpos);

}

function ACL_GetForeColor(style) {
    var forecolorID = 'fore-color:';

    var firstpos = style.indexOf(forecolorID, 0);
    if (firstpos == -1) return null;

    var lastpos = style.indexOf(';', firstpos);
    if (lastpos == -1) return null;

    return style.substring(firstpos + forecolorID.length, lastpos);

}

function ACL_GetDayPos(id, weekDay) {
    firstDayOfWeek = eval(id + '_firstDayOfWeek');

    if (weekDay - firstDayOfWeek >= 0)
        return weekDay - firstDayOfWeek;
    else
        return 7 - firstDayOfWeek + weekDay;
}

// Get the day number
function ACL_DayOfWeek(year, month, day) {
    today = new Date(year, month - 1, day);
    return today.getDay();
}

function ACL_Y2k(number) { return (number < 1000) ? number + 1900 : number; }

function ACL_GetWeekNumber(year, month, day) {
    var when = new Date(year, month, day);
    var newYear = new Date(year, 0, 1);
    var modDay = newYear.getDay();
    if (modDay == 0) modDay = 6; else modDay--;

    var daynum = ((Date.UTC(ACL_Y2k(year), when.getMonth(), when.getDate(), 0, 0, 0) -
				Date.UTC(ACL_Y2k(year), 0, 1, 0, 0, 0)) / 1000 / 60 / 60 / 24) + 1;

    if (modDay < 4) {
        var weeknum = Math.floor((daynum + modDay - 1) / 7) + 1;
    }
    else {
        var weeknum = Math.floor((daynum + modDay - 1) / 7);
        if (weeknum == 0) {
            year--;
            var prevNewYear = new Date(year, 0, 1);
            var prevmodDay = prevNewYear.getDay();
            if (prevmodDay == 0) prevmodDay = 6; else prevmodDay--;
            if (prevmodDay < 4) weeknum = 53; else weeknum = 52;
        }
    }

    return +weeknum;
}

function ACL_PickupTextValidation(id) {

    var doPostBack = document.getElementById(id + '_autoPostBack').value;

    var pickup = document.getElementById(id + '_pickupText');

    if (eval(id + '_useCustomDateFormat') == 'True') {
        var customDateFormat = eval(id + '_customDateFormat');
        var date = ACL_GetDateFromFormat(id, pickup.value, customDateFormat);
        if (date > 0) {
            var selectedDate = new Date(date);
            //var newDate = ACL_FormatDateCustom(id,new Date(date),customDateFormat);
            document.getElementById(id + '_month_selector').value = selectedDate.getMonth() + 1;
            document.getElementById(id + '_year_selector').value = selectedDate.getYear();
            ACL_ChangeSelectedDay(id, selectedDate.getYear(), selectedDate.getMonth() + 1, selectedDate.getDate(), true);
            if (doPostBack == 'True')
                __doPostBack(id, '');

        }

        else {

            var customFormat = eval(id + '_customDateFormat');

            var day = document.getElementById(id + '_day').value;
            var month = document.getElementById(id + '_month').value;
            var year = document.getElementById(id + '_year').value;

            if (day < 10) day = '0' + day;
            if (month < 10) month = '0' + month;
            dateFormatedNorm = day + '/' + month + '/' + year;

            pickup.value = ACL_FormatDateCustom(id, new Date(ACL_GetDateFromFormat(id, dateFormatedNorm, 'dd/mm/yyyy')), customFormat);
        }
    }

    else {
        var dateFormatLocale = eval(id + '_dateFormatLocale');
        var day = -1, month = -1, year = -1;
        var curr, ndx = 0;


        switch (dateFormatLocale) {
            case 'en':
                {
                    curr = pickup.value.indexOf('/', ndx);
                    if (curr > 0 && curr <= 2) {
                        month = ACL_ParseStringToInt(pickup.value.substring(0, curr));

                        ndx = curr;
                        curr = pickup.value.indexOf('/', ndx + 1);
                        if ((curr - ndx == 2 || curr - ndx == 3) && curr >= 3 && curr <= 5) {
                            day = ACL_ParseStringToInt(pickup.value.substring(ndx + 1, curr));

                            if (pickup.value.length - 1 - curr == 4) {
                                year = ACL_ParseStringToInt(pickup.value.substring(curr + 1));
                            }
                        }
                    }
                } break;

            case 'de':
                {
                    curr = pickup.value.indexOf('.', ndx);
                    if (curr > 0 && curr <= 2) {
                        day = ACL_ParseStringToInt(pickup.value.substring(0, curr));

                        ndx = curr;
                        curr = pickup.value.indexOf('.', ndx + 1);
                        if ((curr - ndx == 2 || curr - ndx == 3) && curr >= 3 && curr <= 5) {
                            month = ACL_ParseStringToInt(pickup.value.substring(ndx + 1, curr));

                            if (pickup.value.length - 1 - curr == 4) {
                                year = ACL_ParseStringToInt(pickup.value.substring(curr + 1));
                            }
                        }
                    }

                } break;

            case 'en_GB':
            case 'fr':
            case 'it':
            case 'es':
                {
                    curr = pickup.value.indexOf('/', ndx);
                    if (curr > 0 && curr <= 2) {
                        day = ACL_ParseStringToInt(pickup.value.substring(0, curr));

                        ndx = curr;
                        curr = pickup.value.indexOf('/', ndx + 1);
                        if ((curr - ndx == 2 || curr - ndx == 3) && curr >= 3 && curr <= 5) {
                            month = ACL_ParseStringToInt(pickup.value.substring(ndx + 1, curr));

                            if (pickup.value.length - 1 - curr == 4) {
                                year = ACL_ParseStringToInt(pickup.value.substring(curr + 1));
                            }
                        }
                    }

                } break;

            case 'pt':
                {
                    curr = pickup.value.indexOf('-', ndx);
                    if (curr > 0 && curr <= 2) {
                        day = ACL_ParseStringToInt(pickup.value.substring(0, curr));

                        ndx = curr;
                        curr = pickup.value.indexOf('-', ndx + 1);
                        if ((curr - ndx == 2 || curr - ndx == 3) && curr >= 3 && curr <= 5) {
                            month = ACL_ParseStringToInt(pickup.value.substring(ndx + 1, curr));

                            if (pickup.value.length - 1 - curr == 4) {
                                year = ACL_ParseStringToInt(pickup.value.substring(curr + 1));
                            }
                        }
                    }
                } break;
        }

        if (day != -1 && month != -1 && year != -1 && ACL_IsDateValid(year, month, day)) {
            document.getElementById(id + '_month_selector').value = month;
            document.getElementById(id + '_year_selector').value = year;
            ACL_ChangeSelectedDay(id, year, month, day, true);

        }

        else if (day == -1 && month == -1 && year == -1)
            pickup.value = "";

        else
            pickup.value = ACL_FormatDate(new Date(document.getElementById(id + '_year').value, document.getElementById(id + '_month').value - 1, document.getElementById(id + '_day').value), dateFormatLocale);
    }

    ACL_OnBlurPickerClientSide(id);
}

function ACL_ParseStringToInt(stringToParse) {
    var length = stringToParse.length;
    var sign = '';
    var indexStart = 0;
    var i = 0;
    var returnValue = 0;

    if (length > 0) {
        if (stringToParse.charAt(0) == '-' || stringToParse.charAt(0) == '+') {
            sign = stringToParse.charAt(0);
            i = 1;
        }


        while (stringToParse.charAt(i) == '0' && i < length) {
            i++;
            indexStart = i;
        }
        returnValue = parseInt(stringToParse.substring(indexStart));
    }

    if (returnValue != 0 && sign == '-') {
        returnValue *= -1;
    }

    return returnValue;
}

function ACL_SelectWeek(id, yearFrom, monthFrom, dayFrom) {
    var continueToSelectDay = true

    var yearToSelect = yearFrom;
    var monthToSelect = monthFrom;
    var dayToSelect = dayFrom;
    var maxDay = ACL_MaxDays(yearFrom, monthFrom);
    var allWeekIsSelected = true;
    var visibleMonth = document.getElementById(id + '_month_selector').value;
    var blockedDates = document.getElementById(id + '_blockedDates').value;

    var selectedDates = document.getElementById(id + '_selectedDates').value;
    for (i = 0; i < 7; i++) {
        if (dayToSelect < 10) day = '0' + dayToSelect;
        else day = dayToSelect;
        if (monthToSelect < 10) month = '0' + monthToSelect;
        else month = monthToSelect;
        var dateToSelected = yearToSelect + '/' + month + '/' + day;
        if (selectedDates.indexOf(dateToSelected) == -1 && blockedDates.indexOf(dateToSelected) == -1 && Date.parse(dateToSelected) >= Date.parse(document.getElementById(id + '_minDate').value) && Date.parse(dateToSelected) <= Date.parse(document.getElementById(id + '_maxDate').value)) {
            allWeekIsSelected = false;
            break;
        }

        if (dayToSelect + 1 > maxDay && monthToSelect == 12) {
            dayToSelect = 1;
            monthToSelect = 1;
            yearToSelect++;
        }
        else if (dayToSelect + 1 > maxDay && monthToSelect != 12) {
            dayToSelect = 1;
            monthToSelect++;
        }
        else
            dayToSelect++;
    }

    yearToSelect = yearFrom;
    monthToSelect = monthFrom;
    dayToSelect = dayFrom;
    maxDay = ACL_MaxDays(yearFrom, monthFrom);

    for (i = 0; i < 7; i++) {
        if (dayToSelect < 10) day = '0' + dayToSelect;
        else day = dayToSelect;
        if (monthToSelect < 10) month = '0' + monthToSelect;
        else month = monthToSelect;
        var dateToSelected = yearToSelect + '/' + month + '/' + day;

        if (allWeekIsSelected == false) {
            if (blockedDates.indexOf(dateToSelected) == -1 && Date.parse(dateToSelected) >= Date.parse(document.getElementById(id + '_minDate').value) && Date.parse(dateToSelected) <= Date.parse(document.getElementById(id + '_maxDate').value))
                ACL_AddSelectedDay(id, yearToSelect, monthToSelect, dayToSelect);
        }
        else {
            if (blockedDates.indexOf(dateToSelected) == -1 && Date.parse(dateToSelected) >= Date.parse(document.getElementById(id + '_minDate').value) && Date.parse(dateToSelected) <= Date.parse(document.getElementById(id + '_maxDate').value))
                ACL_RemoveSelectedDay(id, yearToSelect, monthToSelect, dayToSelect);
        }

        if (dayToSelect + 1 > maxDay && monthToSelect == 12) {
            dayToSelect = 1;
            monthToSelect = 1;
            yearToSelect++;
        }
        else if (dayToSelect + 1 > maxDay && monthToSelect != 12) {
            dayToSelect = 1;
            monthToSelect++;
        }
        else
            dayToSelect++;
    }

    if ((monthToSelect < visibleMonth && !(visibleMonth == 12 && monthToSelect == 1)) || (visibleMonth == 1 && monthToSelect == 12))
        ACL_ChangeMonth(id, -1);
    else if (monthToSelect > visibleMonth || (visibleMonth == 12 && monthToSelect == 1))
        ACL_ChangeMonth(id, +1);
    else
        ACL_RenderCalendar(id);
}

function ACL_AddSelectedDay(id, year, month, day) {
    selectedMonth = ACL_GetSelectedMonth(id)

    document.getElementById(id + '_day').value = day;
    document.getElementById(id + '_month').value = month;
    document.getElementById(id + '_year').value = year;

    var multiSelection = eval(id + '_multiSelection');
    if (multiSelection == 'True') {
        var dayToSelected = 1, monthToRender = 1;
        if (day < 10) dayToSelected = '0' + day;
        else dayToSelected = day;
        if (month < 10) monthToSelected = '0' + month;
        else monthToSelected = month;
        var dateToSelected = year + '/' + monthToSelected + '/' + dayToSelected;

        var selectedDates = document.getElementById(id + '_selectedDates').value;
        if (selectedDates.indexOf(dateToSelected) == -1) {
            document.getElementById(id + '_selectedDates').value = selectedDates.concat(dateToSelected + ';');
        }
    }
}

function ACL_RemoveSelectedDay(id, year, month, day) {
    if (day < 10) dayToSelect = '0' + day;
    else dayToSelect = day;
    if (month < 10) monthToSelect = '0' + month;
    else monthToSelect = month;
    var dateToSelect = year + '/' + monthToSelect + '/' + dayToSelect;

    var selectedDates = document.getElementById(id + '_selectedDates').value;
    if (selectedDates.indexOf(dateToSelect) != -1) {
        document.getElementById(id + '_selectedDates').value = ACL_RemoveDateInSelecteDate(selectedDates, dateToSelect);
    }
}

function ACL_IsSelectorValid(syear, smonth, sday) {
    var maxDay = ACL_MaxDays(syear[syear.selectedIndex].value, smonth[smonth.selectedIndex].value, sday[sday.selectedIndex].value);
    if (sday[sday.selectedIndex].value != '' && maxDay < sday[sday.selectedIndex].value)
        sday[maxDay - 1].selected = true;
}

function ACL_LZ(x) {
    return (x < 0 || x > 9 ? "" : "0") + x
}

function ACL_IsDate(val, format) {
    var date = ACL_GetDateFromFormat(val, format);
    if (date == 0) { return false; }
    return true;
}

function ACL_FormatDateCustom(id, date, format) {
    format = format + "";
    var month_names = eval(id + '_months');
    var day_names = eval(id + '_days');
    var result = "";
    var i_format = 0;
    var c = "";
    var token = "";
    var y = date.getYear() + "";
    var m = date.getMonth() + 1;
    var d = date.getDate();
    var dn = date.getDay();
    var hour = date.getHours();
    var min = date.getMinutes();
    // Convert real date parts into formatted versions
    var value = new Object();
    if (y.length < 4) { y = "" + (y - 0 + 1900); }
    value["y"] = "" + y;
    value["yyyy"] = y;
    value["yy"] = y.substring(2, 4);
    value["m"] = m;
    value["mm"] = ACL_LZ(m);
    value["mmmm"] = month_names[m - 1];
    value["mmm"] = month_names[m + 11];
    value["d"] = d;
    value["dd"] = ACL_LZ(d);
    value["ddd"] = day_names[dn + 7];
    value["dddd"] = day_names[dn];
    value["h"] = hour;
    value["hh"] = ACL_LZ(hour);
    value["mi"] = min;

    while (i_format < format.length) {
        c = format.charAt(i_format);
        token = "";
        while ((format.charAt(i_format) == c) && (i_format < format.length)) {
            token += format.charAt(i_format++);
        }
        if (value[token] != null) {
            result = result + value[token];
        }
        else {
            result = result + token;
        }
    }

    return result;
}

function ACL_IsInteger(val) {
    var digits = "1234567890";
    for (var i = 0; i < val.length; i++) {
        if (digits.indexOf(val.charAt(i)) == -1) {
            return false;
        }
    }
    return true;
}

function ACL_GetInt(str, i, minlength, maxlength) {
    for (var x = maxlength; x >= minlength; x--) {
        var token = str.substring(i, i + x);
        if (token.length < minlength) { return null; }
        if (ACL_IsInteger(token)) { return token; }
    }
    return null;
}

function ACL_GetDateFromFormat(id, val, format) {
    var month_names = eval(id + '_months');
    var day_names = eval(id + '_days');
    val = val + "";
    format = format + "";
    var i_val = 0;
    var i_format = 0;
    var c = "";
    var token = "";
    var token2 = "";
    var x, y;
    var now = new Date();
    var year = now.getYear();
    var month = now.getMonth() + 1;
    var date = 1;

    while (i_format < format.length) {
        // Get next token from format string
        c = format.charAt(i_format);
        token = "";
        while ((format.charAt(i_format) == c) && (i_format < format.length)) {
            token += format.charAt(i_format++);
        }
        // Extract contents of value based on format token
        if (token == "yyyy" || token == "yy" || token == "y") {
            if (token == "yyyy") { x = 4; y = 4; }
            if (token == "yy") { x = 2; y = 2; }
            if (token == "y") { x = 2; y = 4; }
            year = ACL_GetInt(val, i_val, x, y);
            if (year == null) { return 0; }
            i_val += year.length;
            if (year.length == 2) {
                if (year > 70) { year = 1900 + (year - 0); }
                else { year = 2000 + (year - 0); }
            }
        }
        else if (token == "mmmm" || token == "mmm") {
            month = 0;
            for (var i = 0; i < month_names.length; i++) {
                var month_name = month_names[i];
                if (val.substring(i_val, i_val + month_name.length).toLowerCase() == month_name.toLowerCase()) {
                    if (token == "mmmm" || (token == "mmm" && i > 11)) {
                        month = i + 1;
                        if (month > 12) { month -= 12; }
                        i_val += month_name.length;
                        break;
                    }
                }
            }
            if ((month < 1) || (month > 12)) { return 0; }
        }
        else if (token == "ddd" || token == "dddd") {
            for (var i = 0; i < day_names.length; i++) {
                var day_name = day_names[i];
                if (val.substring(i_val, i_val + day_name.length).toLowerCase() == day_name.toLowerCase()) {
                    i_val += day_name.length;
                    break;
                }
            }
        }
        else if (token == "mm" || token == "m") {
            month = ACL_GetInt(val, i_val, token.length, 2);
            if (month == null || (month < 1) || (month > 12)) { return 0; }
            i_val += month.length;
        }
        else if (token == "dd" || token == "d") {
            date = ACL_GetInt(val, i_val, token.length, 2);
            if (date == null || (date < 1) || (date > 31)) { return 0; }
            i_val += date.length;
        }
        else {
            if (val.substring(i_val, i_val + token.length) != token) { return 0; }
            else { i_val += token.length; }
        }
    }
    // If there are any trailing characters left in the value, it doesn't match
    if (i_val != val.length) { return 0; }
    // Is date valid for month?
    if (month == 2) {
        // Check for leap year
        if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) { // leap year
            if (date > 29) { return 0; }
        }
        else { if (date > 28) { return 0; } }
    }
    if ((month == 4) || (month == 6) || (month == 9) || (month == 11)) {
        if (date > 30) { return 0; }
    }
    var newdate = new Date(year, month - 1, date);
    return newdate.getTime();
}

function ACL_SetAsFirstPlan(id) {
    ACL_ShowCalendar(id);
    ACL_HideCalendar(id);
}

function ACL_EnterValidation(id, pEvent) {
    if (pEvent.keyCode == 13) {
        ACL_PickupTextValidation(id);
        return false;
    }
}

function ACL_GetSelectedOptionText(id) {
    var ddl = document.getElementById(id);
    return ddl.options[ddl.selectedIndex].text;
}

function ACL_OverDay(id, dayId, year, month, day) {
    /*if (ACL_selectedDateOver != null)
    {
		
		if (ACL_CorrectYear(ACL_selectedDateOver) == year && ACL_selectedDateOver.getMonth() == (month) && ACL_selectedDateOver.getDate() == day)
    {
    }
    else
    {
    if (ACL_ie)
    {
    ACL_RestoreStyle(id,dayId);
    }
    else
    {
    ACL_RestoreStyleMozilla(id,dayId,year,month,day);
    }
    }
	
	}
    else
    {
    if (ACL_ie)
    {
    ACL_RestoreStyle(id,dayId);
    }
    else
    {
    ACL_RestoreStyleMozilla(id,dayId,year,month,day);
    }
    }*/
}

function ACL_OutDay(dayId, year, month, day) {
    var dayObj = document.getElementById(dayId);
    if (ACL_selectedDateOver != null && ACL_CorrectYear(ACL_selectedDateOver) == year && ACL_selectedDateOver.getMonth() == (month) && ACL_selectedDateOver.getDate() == day) {
    }
    else {
        dayObj.style.backgroundColor = ACL_backupStyle.backgroundColor;
        dayObj.style.foreColor = ACL_backupStyle.foreColor;
        if (ACL_backupStyle.backgroundImage != null)
            dayObj.style.backgroundImage = ACL_backupStyle.backgroundImage;

        ACL_selectedDateOver = null;
    }
}

function ACL_Style(backgroundColor, foreColor, backgroundImage) {
    this.backgroundColor = backgroundColor;
    this.foreColor = foreColor;
    this.backgroundImage = backgroundImage;
}

function ACL_UseDayOver(id) {
    var dayOverStyle = eval(id + '_dayOverStyle');
    if (ACL_GetBackgroundColor(dayOverStyle) == '#FFFFFF' &&
		ACL_GetForeColor(dayOverStyle) == '#FFFFFF' &&
		ACL_GetBackgroundImage(dayOverStyle) == null)
        return false;

    return true;
}

function ACL_RestoreStyleMozilla(id, dayId, year, month, day) {
    var dayOverStyle = eval(id + '_dayOverStyle');
    var dayObj = document.getElementById(dayId);

    var style;
    if (ACL_GetSelectedMonth(id) == month && ACL_GetSelectedYear(id) == year)
        otherDay = false;

    year = ((!year) ? ACL_Y2k(ACL_GetSelectedYear(today)) : year);
    month = ((!month) ? today.getMonth() : month - 1);
    var date = new Date(year, month, day);
    if (ACL_IsBlockedDate(id, day) == true)
        style = eval(id + '_blockedDayStyle');
    else if (ACL_IsSelectedDate(id, year, month, day) == true)
        style = eval(id + '_selectedDayStyle');
    else if (ACL_IsTodayDate(id, year, month, day) == true)
        style = eval(id + '_todayDayStyle');
    else if (otherDay)
        style = eval(id + '_otherMonthDayStyle');
    else if (date.getDay() == 0 || date.getDay() == 6)
        style = eval(id + '_weekEndDayStyle');
    else
        style = eval(id + '_dayStyle');


    ACL_backupStyle.backgroundColor = ACL_GetBackgroundColor(style); ;
    ACL_backupStyle.foreColor = ACL_GetForeColor(style);
    ACL_backupStyle.backgroundImage = ACL_GetBackgroundImage(style);

    var styleToApply = new ACL_Style();
    styleToApply.backgroundColor = ACL_GetBackgroundColor(dayOverStyle);
    styleToApply.foreColor = ACL_GetForeColor(dayOverStyle);
    styleToApply.backgroundImage = ACL_GetBackgroundImage(dayOverStyle);

    dayObj.style.backgroundColor = styleToApply.backgroundColor;
    dayObj.style.color = styleToApply.foreColor;
    if (styleToApply.backgroundImage != null)
        dayObj.style.backgroundImage = styleToApply.backgroundImage;
}

function ACL_RestoreStyle(id, dayId) {
    var dayOverStyle = eval(id + '_dayOverStyle');
    var dayObj = document.getElementById(dayId);

    ACL_backupStyle.backgroundColor = dayObj.style.backgroundColor;
    ACL_backupStyle.foreColor = dayObj.style.foreColor;
    ACL_backupStyle.backgroundImage = dayObj.style.backgroundImage;

    var styleToApply = new ACL_Style();
    styleToApply.backgroundColor = ACL_GetBackgroundColor(dayOverStyle);
    styleToApply.foreColor = ACL_GetForeColor(dayOverStyle);
    styleToApply.backgroundImage = ACL_GetBackgroundImage(dayOverStyle);

    dayObj.style.backgroundColor = styleToApply.backgroundColor;
    dayObj.style.color = styleToApply.foreColor;
    if (styleToApply.backgroundImage != null)
        dayObj.style.backgroundImage = styleToApply.backgroundImage;

}

function ACL_OverDayToolTip(id, dayId, year, month, day) {
    /*var text = '';
    var otherDay = true;
 
if (ACL_GetSelectedMonth(id) == month && ACL_GetSelectedYear(id) == year)
    otherDay = false;

 year = ((!year) ? ACL_Y2k(today.getYear()):year);
    month = ((!month) ? today.getMonth():month-1);
    var date = new Date(year,month,day); 
    if (ACL_IsBlockedDate(id,day) == true)
    text = eval(id + '_toolTipTextBlocked');
    else if (ACL_IsSelectedDate(id,year,month,day) == true)
    text = eval(id + '_toolTipTextSelected');
    else if (ACL_IsTodayDate(id,year,month,day) == true)
    text = eval(id + '_toolTipTextToday');
    else if (otherDay)
    text = eval(id + '_toolTipTextOther'); 	
    else if (date.getDay() == 0 || date.getDay() == 6)
    text = eval(id + '_toolTipTextWeekend');
    else
    text = eval(id + '_toolTipTextNormal');

 var toolTipBox = document.getElementById(id + '_toolTipBox');
 
 if (toolTipBox != null && text != '')
    {
    toolTipBox.style.visibility = 'visible';
 	
  var dayObj = document.getElementById(dayId);
  
 	toolTipBox.innerHTML = text;
    toolTipBox.style.left = parseInt(ACL_FindPosX(dayObj)) + 'px';
    toolTipBox.style.top = parseInt(ACL_FindPosY(dayObj)) + dayObj.offsetHeight + 'px';
    }
    */
}

function ACL_IsBlockedDate(id, day) {
    if (day == null)
        return true;

    return false;
}

function ACL_CorrectYear(theDate) {
    var correctYear = theDate.getYear();

    if (ACL_ng == true && ACL_ie == false) {
        correctYear = correctYear + 1900;
    }

    return correctYear;
}

function ACL_IsSelectedDate(id, year, month, day) {
    var oneSelected = ACL_GetSelectedDate(id);

    if (year == ACL_CorrectYear(oneSelected) && month == oneSelected.getMonth() && day == oneSelected.getDate()) {
        return true;
    }

    var selectedDates = document.getElementById(id + '_selectedDates');
    if (selectedDates != null) {
        var dayToSelected = 1, monthToRender = 1;
        if (day < 10) dayToSelected = '0' + day;
        else dayToSelected = day;
        if (month < 10) monthToSelected = '0' + month;
        else monthToSelected = month;
        var dateToSelected = year + '/' + monthToSelected + '/' + dayToSelected;

        if (selectedDates.value.indexOf(dateToSelected) >= 0) {
            return true;
        }
    }

    return false;
}

function ACL_IsTodayDate(id, year, month, day) {
    var today = new Date();
    if (ACL_CorrectYear(today) == year && today.getMonth() == month && today.getDate() == day)
        return true;

    return false;
}

function ACL_OutDayToolTip(id) {
    var toolTipBox = document.getElementById(id + '_toolTipBox');

    if (toolTipBox != null) {
        toolTipBox.innerHTML = '';
        toolTipBox.style.visibility = 'hidden';
    }
}

function ACL_FindPosX(obj) {
    var curleft = 0;
    if (obj.offsetParent) {
        while (obj.offsetParent) {
            if (obj.tagName != 'BODY')
                curleft += obj.offsetLeft;
            obj = obj.offsetParent;
        }
    }
    else if (obj.x) {
        curleft += obj.x;
    }
    return curleft;
}

function ACL_FindPosY(obj) {
    var curtop = 0;
    if (obj.offsetParent) {
        while (obj.offsetParent) {
            if (obj.tagName != 'BODY') {
                curtop += obj.offsetTop;
            }
            obj = obj.offsetParent;
        }
    }
    else if (obj.y) curtop += obj.y;
    return curtop;
}

function ACL_BlockRange(id, startYear, startMonth, startDay, endYear, endMonth, endDay) {
    var startDate = new Date(startYear, ACL_GetCorrectMonth(startMonth, -1), startDay);
    var endDate = new Date(endYear, ACL_GetCorrectMonth(endMonth, -1), endDay);
    var numOfDays = ACL_DaysDiff(endDate, startDate) + 1;

    var currentDate = startDate;
    for (var i = 0; i < numOfDays; i++) {
        ACL_BlockDate(id, currentDate.getYear(), ACL_GetCorrectMonth(currentDate.getMonth(), 1), currentDate.getDate());
        currentDate = new Date(currentDate.getTime() + 86400000);
    }

    ACL_RenderCalendar(id);

}

function ACL_GetCorrectMonth(month, factor) {
    if (factor == -1) {
        if (month == 0)
            return 12;

        return month - 1;
    }

    else if (factor == 1) {
        if (month == 12)
            return 1;

        return month + 1;
    }

    return month;
}

function ACL_BlockDate(id, year, month, day) {
    ACL_BlockDate(id, year, month, day, true);
}

function ACL_BlockDate(id, year, month, day, mustBeRedendered) {
    var dateToBlock = ACL_FormatNorm(year, month, day);
    ACL_AddBlodkedToInput(dateToBlock, id);

    if (mustBeRedendered)
        ACL_RenderCalendar(id);
}

function ACL_AddBlodkedToInput(dateToAdd, id) {
    var blockedDates = document.getElementById(id + '_blockedDates').value;

    if (blockedDates.length > 0 && blockedDates.charAt(blockedDates.length - 1) != ",")
        blockedDates += ",";

    if (blockedDates.indexOf(dateToAdd) == -1) {
        blockedDates += dateToAdd;
    }

    document.getElementById(id + '_blockedDates').value = blockedDates;
}

function ACL_FormatNorm(year, month, day) {
    if (day < 10) day = '0' + day;

    if (month < 10) month = '0' + month;

    return year + '/' + month + '/' + day;
}

function ACL_TrimEnd(text, separator) {
    if (text.charAt(text.length - 1) == separator) {
        text = text.substring(0, text.length - 1);
    }

    return text;
}

function ACL_DaysDiff(start, end) {
    return Math.round((start - end) / 86400000);
}

