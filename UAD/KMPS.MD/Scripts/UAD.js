function drpopenactivity_onchange(clientID) {
    $('#' + clientID + 'drpOpenActivityDateRange').prop('selectedIndex', 0);
    $('#' + clientID + 'drpOpenActivityDays').prop('selectedIndex', 0);
    $('#' + clientID + 'divOpenActivityYear').prop('selectedIndex', 0);
    $('#' + clientID + 'divOpenActivityMonth').prop('selectedIndex', 0);
    $('#' + clientID + 'drpOpenEmailDateRange').prop('selectedIndex', 0);
    $('#' + clientID + 'drpOpenEmailDays').prop('selectedIndex', 0);
    $('#' + clientID + 'divOpenEmailYear').prop('selectedIndex', 0);
    $('#' + clientID + 'divOpenEmailMonth').prop('selectedIndex', 0);

    if ($('#' + clientID + 'drpOpenActivity').val() == '' || $('#' + clientID + 'drpOpenActivity').val() == 0) {
        $('#' + clientID + 'txtOpenActivityFrom').val("");
        $('#' + clientID + 'txtOpenActivityTo').val("");
        $('#' + clientID + 'txtOpenActivityFromYear').val("");
        $('#' + clientID + 'txtOpenActivityToYear').val("");
        $('#' + clientID + 'txtOpenActivityFromMonth').val("");
        $('#' + clientID + 'txtOpenActivityToMonth').val("");
        $('#' + clientID + 'txtOpenBlastID').val("single, comma delimited");

        var Opencombo = $find(clientID + 'RadCBOpenCampaigns');

        Opencombo.trackChanges();

        for (var i = 0; i < Opencombo.get_items().get_count() ; i++) {
            Opencombo.get_items().getItem(i).set_checked(false);
        }

        Opencombo.commitChanges();

        $('#' + clientID + 'txtOpenEmailSubject').val("exact match, partial match, keyword");
        $('#' + clientID + 'txtOpenEmailFromDate').val("");
        $('#' + clientID + 'txtOpenEmailToDate').val("");
        $('#' + clientID + 'txtOpenEmailFromYear').val("");
        $('#' + clientID + 'txtOpenEmailToYear').val("");
        $('#' + clientID + 'txtOpenEmailFromMonth').val("");
        $('#' + clientID + 'txtOpenEmailToMonth').val("");
    }

    if ($('#' + clientID + 'drpOpenActivity').val() == '') {
        $('#' + clientID + 'divOpenDate').css('display', 'none');
        $('#' + clientID + 'divOpenActivityDateRange').css('display', 'none');
        $('#' + clientID + 'drpOpenActivityDays').css('display', 'none');
        $('#' + clientID + 'divOpenEmail').css('display', 'none');
        $('#' + clientID + 'divOpenEmailDateRange').css('display', 'none');
        $('#' + clientID + 'drpOpenEmailDays').css('display', 'none');
    }
    else if ($('#' + clientID + 'drpOpenActivity').val() == '0') {
        $('#' + clientID + 'divOpenDate').css('display', 'inline');
        $('#' + clientID + 'divOpenActivityDateRange').css('display', 'inline');
        $('#' + clientID + 'drpOpenActivityDays').css('display', 'none');
        $('#' + clientID + 'divOpenEmail').css('display', 'none');
        $('#' + clientID + 'divOpenEmailDateRange').css('display', 'none');
        $('#' + clientID + 'drpOpenEmailDays').css('display', 'none');
    }
    else {
        $('#' + clientID + 'divOpenDate').css('display', 'inline');
        $('#' + clientID + 'divOpenActivityDateRange').css('display', 'inline');
        $('#' + clientID + 'drpOpenActivityDays').css('display', 'none');
        $('#' + clientID + 'divOpenEmail').css('display', 'inline');
        $('#' + clientID + 'divOpenEmailDateRange').css('display', 'inline');
        $('#' + clientID + 'drpOpenEmailDays').css('display', 'none');
    }
}

function drpOpenActivityDateRange_onchange(ctrl) {

    var thisctrlID = ctrl.id;
    var clientID = thisctrlID.replace("drpOpenActivityDateRange", "");

    $('#' + clientID + 'txtOpenActivityFrom').val("");
    $('#' + clientID + 'txtOpenActivityTo').val("");
    $('#' + clientID + 'txtOpenActivityFromYear').val("");
    $('#' + clientID + 'txtOpenActivityToYear').val("");
    $('#' + clientID + 'txtOpenActivityFromMonth').val("");
    $('#' + clientID + 'txtOpenActivityToMonth').val("");
    $('#' + clientID + 'divCustomOpenActivityDays').css("display", "none");
    $('#' + clientID + 'rfvCustomOpenActivityDays').prop('enabled', false);
    $('#' + clientID + 'drpOpenActivityDays').prop('selectedIndex', 0);
    $('#' + clientID + 'txtCustomOpenActivityDays').val("");

    if ($('#' + clientID + 'drpOpenActivityDateRange').val() == 'DateRange') {
        $('#' + clientID + 'divOpenActivityDateRange').css("display", "inline");
        $('#' + clientID + 'drpOpenActivityDays').css("display", "none");
        $('#' + clientID + 'divOpenActivityYear').css("display", "none");
        $('#' + clientID + 'divOpenActivityMonth').css("display", "none");
    }
    else if ($('#' + clientID + 'drpOpenActivityDateRange').val() == 'XDays') {
        $('#' + clientID + 'divOpenActivityDateRange').css("display", "none");
        $('#' + clientID + 'drpOpenActivityDays').css("display", "inline");
        $('#' + clientID + 'divOpenActivityYear').css("display", "none");
        $('#' + clientID + 'divOpenActivityMonth').css("display", "none");
    }
    else if($('#' + clientID + 'drpOpenActivityDateRange').val() == 'Year') {
        $('#' + clientID + 'divOpenActivityDateRange').css("display", "none");
        $('#' +clientID + 'drpOpenActivityDays').css("display", "none");
        $('#' + clientID + 'divOpenActivityYear').css("display", "inline");
        $('#' +clientID + 'divOpenActivityMonth').css("display", "none");
}
    else if ($('#' +clientID + 'drpOpenActivityDateRange').val() == 'Month') {
        $('#' +clientID + 'divOpenActivityDateRange').css("display", "none");
        $('#' +clientID + 'drpOpenActivityDays').css("display", "none");
        $('#' +clientID + 'divOpenActivityYear').css("display", "none");
        $('#' +clientID + 'divOpenActivityMonth').css("display", "inline");
    }
}

function drpOpenEmailDateRange_onchange(ctrl) {

    var thisctrlID = ctrl.id;
    var clientID = thisctrlID.replace("drpOpenEmailDateRange", "");

    $('#' + clientID + 'txtOpenEmailFromDate').val("");
    $('#' + clientID + 'txtOpenEmailToDate').val("");
    $('#' + clientID + 'txtOpenEmailFromYear').val("");
    $('#' + clientID + 'txtOpenEmailToYear').val("");
    $('#' + clientID + 'txtOpenEmailFromMonth').val("");
    $('#' + clientID + 'txtOpenEmailToMonth').val("");
    $('#' + clientID + 'divCustomOpenEmailDays').css("display", "none");
    $('#' + clientID + 'rfvCustomOpenEmailDays').prop('enabled', false);
    $('#' + clientID + 'drpOpenEmailDays').prop('selectedIndex', 0);
    $('#' + clientID + 'txtCustomOpenEmailDays').val("");

    if ($('#' + clientID + 'drpOpenEmailDateRange').val() == 'DateRange') {
        $('#' + clientID + 'divOpenEmailDateRange').css("display", "inline");
        $('#' + clientID + 'drpOpenEmailDays').css("display", "none");
        $('#' + clientID + 'divOpenEmailYear').css("display", "none");
        $('#' +clientID + 'divOpenEmailMonth').css("display", "none");
    }
    else if ($('#' + clientID + 'drpOpenEmailDateRange').val() == 'XDays') {
        $('#' +clientID + 'divOpenEmailDateRange').css("display", "none");
        $('#' +clientID + 'drpOpenEmailDays').css("display", "inline");
        $('#' +clientID + 'divOpenEmailYear').css("display", "none");
        $('#' +clientID + 'divOpenEmailMonth').css("display", "none");
    }
    else if($('#' +clientID + 'drpOpenEmailDateRange').val() == 'Year') {
        $('#' +clientID + 'divOpenEmailDateRange').css("display", "none");
        $('#' + clientID + 'drpOpenEmailDays').css("display", "none");
        $('#' +clientID + 'divOpenEmailYear').css("display", "inline");
        $('#' +clientID + 'divOpenEmailMonth').css("display", "none");
    }
    else if ($('#' + clientID + 'drpOpenEmailDateRange').val() == 'Month') {
        $('#' + clientID + 'divOpenEmailDateRange').css("display", "none");
        $('#' +clientID + 'drpOpenEmailDays').css("display", "none");
        $('#' +clientID + 'divOpenEmailYear').css("display", "none");
        $('#' + clientID + 'divOpenEmailMonth').css("display", "inline");
    }
}

function drpclickactivity_onchange(clientID) {

    $('#' +clientID + 'drpClickActivityDateRange').prop('selectedIndex', 0);
    $('#' +clientID + 'drpClickActivityDays').prop('selectedIndex', 0);
    $('#' +clientID + 'divClickActivityYear').prop('selectedIndex', 0);
    $('#' +clientID + 'divClickActivityMonth').prop('selectedIndex', 0);
    $('#' +clientID + 'drpClickEmailDateRange').prop('selectedIndex', 0);
    $('#' +clientID + 'drpClickEmailDays').prop('selectedIndex', 0);
    $('#' +clientID + 'divClickEmailYear').prop('selectedIndex', 0);
    $('#' +clientID + 'divClickEmailMonth').prop('selectedIndex', 0);

    if ($('#' + clientID + 'drpClickActivity').val() == '' || $('#' + clientID + 'drpClickActivity').val() == 0) {
        $('#' + clientID + 'txtClickActivityFrom').val("");
        $('#' + clientID + 'txtClickActivityTo').val("");
        $('#' + clientID + 'txtClickActivityFromYear').val("");
        $('#' + clientID + 'txtClickActivityToYear').val("");
        $('#' + clientID + 'txtClickActivityFromMonth').val("");
        $('#' + clientID + 'txtClickActivityToMonth').val("");
        $('#' + clientID + 'txtLink').val("exact match, partial match, keyword, single, comma delimited");
        $('#' + clientID + 'txtClickBlastID').val("single, comma delimited");

        var Clickcombo = $find(clientID + 'RadCBClickCampaigns');

        Clickcombo.trackChanges();

        for (var i = 0; i < Clickcombo.get_items().get_count() ; i++) {
            Clickcombo.get_items().getItem(i).set_checked(false);
        }

        Clickcombo.commitChanges();

        $('#' + clientID + 'txtClickEmailSubject').val("exact match, partial match, keyword");
        $('#' + clientID + 'txtClickEmailFromDate').val("");
        $('#' + clientID + 'txtClickEmailToDate').val("");
        $('#' + clientID + 'txtClickEmailFromYear').val("");
        $('#' + clientID + 'txtClickEmailToYear').val("");
        $('#' + clientID + 'txtClickEmailFromMonth').val("");
        $('#' + clientID + 'txtClickEmailToMonth').val("");
    }

    if ($('#' +clientID + 'drpClickActivity').val() == '') {
        $('#' +clientID + 'divClickDate').css('display', 'none');
        $('#' + clientID + 'divClickActivityDateRange').css('display', 'none');
        $('#' + clientID + 'drpClickActivityDays').css('display', 'none');
        $('#' + clientID + 'divClickEmail').css('display', 'none');
        $('#' +clientID + 'divClickEmailDateRange').css('display', 'none');
        $('#' +clientID + 'drpClickEmailDays').css('display', 'none');
    }
    else if ($('#' +clientID + 'drpClickActivity').val() == '0') {
        $('#' +clientID + 'divClickDate').css('display', 'inline');
        $('#' +clientID + 'divClickActivityDateRange').css('display', 'inline');
        $('#' +clientID + 'drpClickActivityDays').css('display', 'none');
        $('#' +clientID + 'divClickEmail').css('display', 'none');
        $('#' +clientID + 'divClickEmailDateRange').css('display', 'none');
        $('#' +clientID + 'drpClickEmailDays').css('display', 'none');
    }
    else {
        $('#' +clientID + 'divClickDate').css('display', 'inline');
        $('#' +clientID + 'divClickActivityDateRange').css('display', 'inline');
        $('#' +clientID + 'drpClickActivityDays').css('display', 'none');
        $('#' +clientID + 'divClickEmail').css('display', 'inline');
        $('#' + clientID + 'divClickEmailDateRange').css('display', 'inline');
        $('#' +clientID + 'drpClickEmailDays').css('display', 'none');
    }
}

function drpClickActivityDateRange_onchange(ctrl) {

    var thisctrlID = ctrl.id;
    var clientID = thisctrlID.replace("drpClickActivityDateRange", "");

    $('#' + clientID + 'txtClickActivityFrom').val("");
    $('#' + clientID + 'txtClickActivityTo').val("");
    $('#' + clientID + 'txtClickActivityFromYear').val("");
    $('#' + clientID + 'txtClickActivityToYear').val("");
    $('#' + clientID + 'txtClickActivityFromMonth').val("");
    $('#' + clientID + 'txtClickActivityToMonth').val("");
    $('#' + clientID + 'divCustomClickActivityDays').css("display", "none");
    $('#' + clientID + 'rfvCustomClickActivityDays').prop('enabled', false);
    $('#' + clientID + 'drpClickActivityDays').prop('selectedIndex', 0);
    $('#' + clientID + 'txtCustomClickActivityDays').val("");

    if ($('#' + clientID + 'drpClickActivityDateRange').val() == 'DateRange') {
        $('#' + clientID + 'divClickActivityDateRange').css("display", "inline");
        $('#' + clientID + 'drpClickActivityDays').css("display", "none");
        $('#' + clientID + 'divClickActivityYear').css("display", "none");
        $('#' + clientID + 'divClickActivityMonth').css("display", "none");
    }
    else if ($('#' + clientID + 'drpClickActivityDateRange').val() == 'XDays') {
        $('#' + clientID + 'divClickActivityDateRange').css("display", "none");
        $('#' + clientID + 'drpClickActivityDays').css("display", "inline");
        $('#' + clientID + 'divClickActivityYear').css("display", "none");
        $('#' + clientID + 'divClickActivityMonth').css("display", "none");
        }
    else if($('#' + clientID + 'drpClickActivityDateRange').val() == 'Year') {
        $('#' + clientID + 'divClickActivityDateRange').css("display", "none");
        $('#' + clientID + 'drpClickActivityDays').css("display", "none");
        $('#' + clientID + 'divClickActivityYear').css("display", "inline");
        $('#' + clientID + 'divClickActivityMonth').css("display", "none");
    }
    else if ($('#' +clientID + 'drpClickActivityDateRange').val() == 'Month') {
        $('#' + clientID + 'divClickActivityDateRange').css("display", "none");
        $('#' + clientID + 'drpClickActivityDays').css("display", "none");
        $('#' + clientID + 'divClickActivityYear').css("display", "none");
        $('#' +  clientID + 'divClickActivityMonth').css("display", "inline");
    }
}

function drpClickEmailDateRange_onchange(ctrl) {

    var thisctrlID = ctrl.id;
    var clientID = thisctrlID.replace("drpClickEmailDateRange", "");

    $('#' + clientID + 'txtClickEmailFromDate').val("");
    $('#' + clientID + 'txtClickEmailToDate').val("");
    $('#' + clientID + 'txtClickEmailFromYear').val("");
    $('#' + clientID + 'txtClickEmailToYear').val("");
    $('#' + clientID + 'txtClickEmailFromMonth').val("");
    $('#' + clientID + 'txtClickEmailToMonth').val("");
    $('#' + clientID + 'divCustomClickEmailDays').css("display", "none");
    $('#' + clientID + 'rfvCustomClickEmailDays').prop('enabled', false);
    $('#' + clientID + 'drpClickEmailDays').prop('selectedIndex', 0);
    $('#' + clientID + 'txtCustomClickEmailDays').val("");

    if ($('#' + clientID + 'drpClickEmailDateRange').val() == 'DateRange') {
        $('#' + clientID + 'divClickEmailDateRange').css("display", "inline");
        $('#' + clientID + 'drpClickEmailDays').css("display", "none");
        $('#' + clientID + 'divClickEmailYear').css("display", "none");
        $('#' + clientID + 'divClickEmailMonth').css("display", "none");
    }
    else if ($('#' + clientID + 'drpClickEmailDateRange').val() == 'XDays') {
        $('#' + clientID + 'divClickEmailDateRange').css("display", "none");
        $('#' + clientID + 'drpClickEmailDays').css("display", "inline");
        $('#' + clientID + 'divClickEmailYear').css("display", "none");
        $('#' + clientID + 'divClickEmailMonth').css("display", "none");
    }
    else if ($('#' + clientID + 'drpClickEmailDateRange').val() == 'Year') {
        $('#' + clientID + 'divClickEmailDateRange').css("display", "none");
        $('#' + clientID + 'drpClickEmailDays').css("display", "none");
        $('#' + clientID + 'divClickEmailYear').css("display", "inline");
        $('#' + clientID + 'divClickEmailMonth').css("display", "none");
    }
    else if ($('#' + clientID + 'drpClickEmailDateRange').val() == 'Month') {
        $('#' + clientID + 'divClickEmailDateRange').css("display", "none");
        $('#' + clientID + 'drpClickEmailDays').css("display", "none");
        $('#' + clientID + 'divClickEmailYear').css("display", "none");
        $('#' + clientID + 'divClickEmailMonth').css("display", "inline");
    }
}

function drpvisitactivity_onchange(clientID) {
    $('#' + clientID + 'drpVisitActivityDateRange').prop('selectedIndex', 0);
    $('#' + clientID + 'drpVisitActivityDays').prop('selectedIndex', 0);
    $('#' + clientID + 'divVisitActivityYear').prop('selectedIndex', 0);
    $('#' + clientID + 'divVisitActivityMonth').prop('selectedIndex', 0);

    if ($('#' + clientID + 'drpVisitActivity').val() == '' || $('#' + clientID + 'drpVisitActivity').val() == 0) {
        $('#' + clientID + 'txtVisitActivityFrom').val("");
        $('#' + clientID + 'txtVisitActivityTo').val("");
        $('#' + clientID + 'txtVisitActivityFromYear').val("");
        $('#' + clientID + 'txtVisitActivityToYear').val("");
        $('#' + clientID + 'txtVisitActivityFromMonth').val("");
        $('#' + clientID + 'txtVisitActivityToMonth').val("");
        $('#' + clientID + 'drpDomain').prop('selectedIndex', 0);
        $('#' + clientID + 'txtURL').val("exact match, partial match, keyword, single, comma delimited");
    }

    if ($('#' + clientID + 'drpVisitActivity').val() == '') {
        $('#' + clientID + 'divVisitDate').css('display', 'none');
        $('#' + clientID + 'divVisitActivityDateRange').css('display', 'none');
        $('#' + clientID + 'drpVisitActivityDays').css('display', 'none');
        $('#' + clientID + 'divVisitDomain').css('display', 'none');
    }
    else if ($('#' + clientID + 'drpVisitActivity').val() == '0') {
        $('#' + clientID + 'divVisitDate').css('display', 'inline');
        $('#' + clientID + 'divVisitActivityDateRange').css('display', 'inline');
        $('#' + clientID + 'drpVisitActivityDays').css('display', 'none');
        $('#' + clientID + 'divVisitDomain').css('display', 'none');
    }
    else {
        $('#' + clientID + 'divVisitDate').css('display', 'inline');
        $('#' + clientID + 'divVisitActivityDateRange').css('display', 'inline');
        $('#' + clientID + 'drpVisitActivityDays').css('display', 'none');
        $('#' + clientID + 'divVisitDomain').css('display', 'inline');
    }
}

function drpVisitActivityDateRange_onchange(ctrl) {

    var thisctrlID = ctrl.id;
    var clientID = thisctrlID.replace("drpVisitActivityDateRange", "");

    $('#' + clientID + 'txtVisitActivityFrom').val("");
    $('#' + clientID + 'txtVisitActivityTo').val("");
    $('#' + clientID + 'txtVisitActivityFromYear').val("");
    $('#' + clientID + 'txtVisitActivityToYear').val("");
    $('#' + clientID + 'txtVisitActivityFromMonth').val("");
    $('#' + clientID + 'txtVisitActivityToMonth').val("");
    $('#' + clientID + 'divCustomVisitActivityDays').css("display", "none");
    $('#' + clientID + 'rfvCustomVisitActivityDays').prop('enabled', false);
    $('#' + clientID + 'drpVisitActivityDays').prop('selectedIndex', 0);
    $('#' + clientID + 'txtCustomVisitActivityDays').val("");

    if ($('#' + clientID + 'drpVisitActivityDateRange').val() == 'DateRange') {
        $('#' + clientID + 'divVisitActivityDateRange').css("display", "inline");
        $('#' + clientID + 'drpVisitActivityDays').css("display", "none");
        $('#' + clientID + 'divVisitActivityYear').css("display", "none");
        $('#' + clientID + 'divVisitActivityMonth').css("display", "none");
    }
    else if ($('#' + clientID + 'drpVisitActivityDateRange').val() == 'XDays') {
        $('#' + clientID + 'divVisitActivityDateRange').css("display", "none");
        $('#' + clientID + 'drpVisitActivityDays').css("display", "inline");
        $('#' + clientID + 'divVisitActivityYear').css("display", "none");
        $('#' + clientID + 'divVisitActivityMonth').css("display", "none");
    }
    else if ($('#' + clientID + 'drpVisitActivityDateRange').val() == 'Year') {
        $('#' + clientID + 'divVisitActivityDateRange').css("display", "none");
        $('#' + clientID + 'drpVisitActivityDays').css("display", "none");
        $('#' + clientID + 'divVisitActivityYear').css("display", "inline");
        $('#' + clientID + 'divVisitActivityMonth').css("display", "none");
    }
    else if ($('#' + clientID + 'drpVisitActivityDateRange').val() == 'Month') {
        $('#' + clientID + 'divVisitActivityDateRange').css("display", "none");
        $('#' + clientID + 'drpVisitActivityDays').css("display", "none");
        $('#' + clientID + 'divVisitActivityYear').css("display", "none");
        $('#' + clientID + 'divVisitActivityMonth').css("display", "inline");
    }
}

function rbToday_onchange(ctrl)
{
    var thisctrlID = ctrl.id;
    var clientID = thisctrlID.replace("rbToday", "");
    if ($('#' + clientID + 'rbToday').attr("checked", "checked")) {
        $('#' + clientID + 'divTodayPlusMinus').css('display', 'none');
        $('#' + clientID + 'txtDays').val("");
        $('#' + clientID + 'divOther').css('display', 'none');
        $('#' + clientID + 'txtDatePicker').val("");
    }
}

function rbTodayPlusMinus_onchange(ctrl) {
    var thisctrlID = ctrl.id;
    var clientID = thisctrlID.replace("rbTodayPlusMinus", "");
    if ($('#' + clientID + 'rbTodayPlus').attr("checked", "checked")) {
        $('#' + clientID + 'divTodayPlusMinus').css('display', 'inline');
        $('#' + clientID + 'txtDays').val("");
        $('#' + clientID + 'divOther').css('display', 'none');
        $('#' + clientID + 'txtDatePicker').val("");
     }
    }

function rbOther_onchange(ctrl) {
    var thisctrlID = ctrl.id;
    var clientID = thisctrlID.replace("rbOther", "");
    if ($('#' + clientID + 'rbSelect').attr("checked", "checked")) {
        $('#' + clientID + 'divTodayPlusMinus').css('display', 'none');
        $('#' + clientID + 'txtDays').val("");
        $('#' + clientID + 'divOther').css('display', 'inline');
        $('#' + clientID + 'txtDatePicker').val("");
    }
}

function drpAdhocSearch_onchange(ctrl) {
    //    window.status = ctrl.id;
    var thisctrlID = ctrl.id;

    var clientID = thisctrlID.replace("drpAdhocSearch", "");

    if ($('#' + clientID + 'drpAdhocSearch').val() == 'Is Empty' || $('#' + clientID + 'drpAdhocSearch').val() == 'Is Not Empty') {
        $('#' + clientID + 'txtAdhocSearchValue').css('display', 'inline');
        $('#' + clientID + 'divAdhocRange').css('display', 'none');

        $('#' + clientID + 'txtAdhocSearchValue').val('');
        $('#' + clientID + 'txtAdhocSearchValue').prop('disabled', true);
    }
    else if ($('#' + clientID + 'drpAdhocSearch').val() == 'RANGE') {
        $('#' + clientID + 'txtAdhocSearchValue').css('display', 'none');
        $('#' + clientID + 'divAdhocRange').css('display', 'inline');
        $('#' +clientID + 'txtAdhocSearchValue').val("");
    }
    else {
        $('#' + clientID + 'txtAdhocSearchValue').prop('disabled', false);
        $('#' + clientID + 'txtAdhocSearchValue').css('display', 'inline');
        $('#' +clientID + 'divAdhocRange').css('display', 'none');
        $('#' +clientID + 'txtAdhocRangeFrom').val("");
        $('#' + clientID + 'txtAdhocRangeTo').val("");
    }
}

function drpAdhocInt_onchange(ctrl) {
    //    window.status = ctrl.id;
    var thisctrlID = ctrl.id;

    var clientID = thisctrlID.replace("drpAdhocInt", "");

    if ($('#' + clientID + 'drpAdhocInt').val() == 'Range') {
        
        $('#' + clientID + 'lblAdhocTo').css('display', 'inline');
        $('#' + clientID + 'txtAdhocIntTo').css('display', 'inline');
    }
    else  {
        $('#' + clientID + 'txtAdhocIntTo').val('');
        $('#' + clientID + 'txtAdhocIntTo').css('display', 'none');
        $('#' + clientID + 'lblAdhocTo').css('display', 'none');
    }
}

function drpDateRange_onchange(ctrl) {
    //    window.status = ctrl.id;
    var thisctrlID = ctrl.id;

    var clientID = thisctrlID.replace("drpDateRange", "");
    $('#' + clientID + 'divCustomAdhocDays').css("display", "none");
    $('#' + clientID + 'rfvCustomAdhocDays').prop('enabled', false);
    $('#' + clientID + 'drpAdhocDays').prop('selectedIndex', 0);
    $('#' + clientID + 'txtCustomAdhocDays').val("");

    if ($('#' + clientID + 'drpDateRange').val() == 'DateRange') {
        $('#' + clientID + 'divAdhocDateRange').css('display', 'inline');
        $('#' + clientID + 'divAdhocDateYear').css('display', 'none');
        $('#' + clientID + 'divAdhocDateMonth').css('display', 'none');
        $('#' + clientID + 'drpAdhocDays').css('display', 'none');
        $('#' +clientID + 'txtAdhocDateRangeFrom').val('');
        $('#' +clientID + 'txtAdhocDateRangeTo').val('');
    }
    else if ($('#' + clientID + 'drpDateRange').val() == 'XDays') {
        $('#' + clientID + 'divAdhocDateRange').css('display', 'none');
        $('#' + clientID + 'divAdhocDateYear').css('display', 'none');
        $('#' + clientID + 'divAdhocDateMonth').css('display', 'none');
        $('#' + clientID + 'drpAdhocDays').css('display', 'inline');
        $('#' + clientID + 'drpAdhocDays').prop('selectedIndex', 0);
    }
    else if ($('#' + clientID + 'drpDateRange').val() == 'Year') {
        $('#' + clientID + 'divAdhocDateRange').css('display', 'none');
        $('#' + clientID + 'divAdhocDateYear').css('display', 'inline');
        $('#' + clientID + 'divAdhocDateMonth').css('display', 'none');
        $('#' + clientID + 'drpAdhocDays').css('display', 'none');
        $('#' +clientID + 'txtAdhocDateYearFrom').val('');
        $('#' +clientID + 'txtAdhocDateYearTo').val('');
    }
    else if ($('#' + clientID + 'drpDateRange').val() == 'Month') {
        $('#' + clientID + 'divAdhocDateRange').css('display', 'none');
        $('#' + clientID + 'divAdhocDateYear').css('display', 'none');
        $('#' + clientID + 'divAdhocDateMonth').css('display', 'inline');
        $('#' + clientID + 'drpAdhocDays').css('display', 'none');
        $('#' +clientID + 'txtAdhocDateMonthFrom').val('');
        $('#' +clientID + 'txtAdhocDateMonthTo').val('');
    }
}

function drpAdhocDays_onchange(ctrl) {
    //    window.status = ctrl.id;
    var thisctrlID = ctrl.id;
    var clientID = thisctrlID.replace("drpAdhocDays", "");

    $('#' + clientID + 'txtCustomAdhocDays').val('');

    if ($('#' + clientID + 'drpAdhocDays').val() == 'Custom') {
        $('#' + clientID + 'divCustomAdhocDays').css('display', 'inline');
        $('#' + clientID + 'rfvCustomAdhocDays').prop('enabled', true);
    }
    else {
        $('#' + clientID + 'divCustomAdhocDays').css('display', 'none');
        $('#' + clientID + 'rfvCustomAdhocDays').prop('enabled', false);
    }
}

function drpOpenActivityDays_onchange(ctrl) {
    var thisctrlID = ctrl.id;
    var clientID = thisctrlID.replace("drpOpenActivityDays", "");

    $('#' + clientID + 'txtCustomOpenActivityDays').val("");

    if ($('#' + clientID + 'drpOpenActivityDays').val() == 'Custom') {
        $('#' + clientID + 'divCustomOpenActivityDays').css("display", "inline");
        $('#' + clientID + 'rfvCustomOpenActivityDays').prop('enabled', true);
    }
    else {
        $('#' + clientID + 'divCustomOpenActivityDays').css("display", "none");
        $('#' + clientID + 'rfvCustomOpenActivityDays').prop('enabled', false);
    }
}

function drpOpenEmailDays_onchange(ctrl) {

    var thisctrlID = ctrl.id;
    var clientID = thisctrlID.replace("drpOpenEmailDays", "");

    $('#' + clientID + 'txtCustomOpenEmailDays').val("");

    if ($('#' + clientID + 'drpOpenEmailDays').val() == 'Custom') {
        $('#' + clientID + 'divCustomOpenEmailDays').css("display", "inline");
        $('#' + clientID + 'rfvCustomOpenEmailDays').prop('enabled', true);
    }
    else {
        $('#' + clientID + 'divCustomOpenEmailDays').css("display", "none");
        $('#' + clientID + 'rfvCustomOpenEmailDays').prop('enabled', false);
    }
}

function drpClickActivityDays_onchange(ctrl) {

    var thisctrlID = ctrl.id;
    var clientID = thisctrlID.replace("drpClickActivityDays", "");

    $('#' + clientID + 'txtCustomClickActivityDays').val("");

    if ($('#' + clientID + 'drpClickActivityDays').val() == 'Custom') {
        $('#' + clientID + 'divCustomClickActivityDays').css("display", "inline");
        $('#' + clientID + 'rfvCustomClickActivityDays').prop('enabled', true);
    }
    else {
        $('#' + clientID + 'divCustomClickActivityDays').css("display", "none");
        $('#' + clientID + 'rfvCustomClickActivityDays').prop('enabled', false);
    }
}

function drpClickEmailDays_onchange(ctrl) {

    var thisctrlID = ctrl.id;
    var clientID = thisctrlID.replace("drpClickEmailDays", "");

    $('#' + clientID + 'txtCustomClickEmailDays').val("");

    if ($('#' + clientID + 'drpClickEmailDays').val() == 'Custom') {
        $('#' + clientID + 'divCustomClickEmailDays').css("display", "inline");
        $('#' + clientID + 'rfvCustomClickEmailDays').prop('enabled', true);
    }
    else {
        $('#' + clientID + 'divCustomClickEmailDays').css("display", "none");
        $('#' + clientID + 'rfvCustomClickEmailDays').prop('enabled', false);
    }
}

function drpVisitActivityDays_onchange(ctrl) {

    var thisctrlID = ctrl.id;
    var clientID = thisctrlID.replace("drpVisitActivityDays", "");

    $('#' + clientID + 'txtCustomVisitActivityDays').val("");

    if ($('#' + clientID + 'drpVisitActivityDays').val() == 'Custom') {
        $('#' + clientID + 'divCustomVisitActivityDays').css("display", "inline");
        $('#' + clientID + 'rfvCustomVisitActivityDays').prop('enabled', true);
    }
    else {
        $('#' + clientID + 'divCustomVisitActivityDays').css("display", "none");
        $('#' + clientID + 'rfvCustomVisitActivityDays').prop('enabled', false);
    }
}