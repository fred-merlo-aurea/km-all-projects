<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MedtechComp.aspx.cs" Inherits="KMPS_JF.CustomForms.MedtechComp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Editorial Comps Subscription Form </title>
    <link href="../CSS/styles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .errorsummary
        {
            border: 1px solid black;
            color: red;
            margin: 5px 0px;
            padding: 15px;
            background: white url(../images/ic-alert.jpg) no-repeat 5px 50%;
            background-color: white;
        }
        .errorsummary ul
        {
            margin: 5px;
            padding: 10px;
            margin-left: 80px;
            list-style: square;
            color: red;
        }
        .ccblcategory
        {
            font-size: 11pt;
            font-weight: 900;
            padding: 5px 0px 5px 0px;
            text-align: left;
        }
        .Grouppadding
        {
            padding: 5px 3px 5px 0px;
        }
        .addpadding
        {
            padding: 3px 3px 3px 3px;
        }
    </style>

    <script src="../scripts/highslide/highslide-full.js" type="text/javascript"></script>

    <!-- START Conversion Tracking -->

    <script language="javascript" src="getConversionData.js"></script>

    <!-- END Conversion Tracking -->

    <script language="javascript">
        function getobj(id) {
            //alert(id);
            try {
                if (document.all && !document.getElementById)
                    obj = eval('document.all.' + id);
                else if (document.layers)
                    obj = eval('document.' + id);
                else if (document.getElementById)
                    obj = document.getElementById(id);

                return obj;
            }
            catch (err) {
                return null;
            }
        }

        function EnableTextControl(ctrltype, optionctrlname, textctrlname, optionCount) {
            try {

                //alert(optionctrlname + " / " + textctrlname + " / " + optionctrlname + "_" + (optionCount - 1));                                               
                var bEnable = false;

                if (ctrltype == 'r' || ctrltype == 'c') {
                    if (getobj(optionctrlname + "_" + (optionCount - 1)).checked)
                        bEnable = true;
                }
                else if (ctrltype == 'cc') {
                    if (getobj(optionctrlname + "_" + (optionCount - 1)).checked)
                        bEnable = true;
                }
                else {
                    //alert(optionCount + "Control:"  + optionctrlname);
                    if (getobj(optionctrlname).selectedIndex == optionCount) {
                        //alert("In the button disable part!!!"); 
                        bEnable = true;
                    }
                }

                if (bEnable) {
                    //alert("In the Validator enable!!!");
                    ValidatorEnable(getobj("rfv_" + textctrlname), true);
                    getobj("pnl_" + textctrlname).style.display = "block";
                    getobj(textctrlname).disabled = false;
                    //getobj(textctrlname).focus();
                }
                else {
                    //alert("In the Validator disable!!!"); 
                    ValidatorEnable(getobj("rfv_" + textctrlname), false);
                    getobj("pnl_" + textctrlname).style.display = "none";
                    getobj(textctrlname).value = "";
                    getobj(textctrlname).disabled = true;
                }

            }
            catch (err) {
                //alert("Throwing exception!!!");                 
                return null;
            }
        }

        String.prototype.startsWith = function(str)
        { return (this.match("^" + str) == str) }


        function CheckBox_RFV(ctrlname) {

            // Loop from zero to the one minus the number of checkbox button selections
            for (counter = 0; counter < document.forms[0].elements.length; counter++) {

                elm = document.forms[0].elements[counter];
                if (elm.type == "checkbox") {
                    var checkboxname = document.forms[0].elements[counter].name;
                    if (checkboxname.startsWith(ctrlname)) // + "$"
                    {
                        if (document.forms[0].elements[counter].checked)
                            return true;
                    }
                }
            }
            return false;
        }

        function cv_question_demo7_print_or_digital(source, args) {
            var ischecked = false;

            if (getobj("chk_user_Demo7_Print") && getobj("chk_user_Demo7_Digital") && getobj("chk_user_Demo7_Both")) {
                if (getobj("chk_user_Demo7_Print").checked)
                    ischecked = true;

                if (getobj("chk_user_Demo7_Digital").checked)
                    ischecked = true;

                if (getobj("chk_user_Demo7_Both").checked)
                    ischecked = true;
            }
            else if (getobj("chk_user_Demo7_Print") && getobj("chk_user_Demo7_Digital")) {
                if (getobj("chk_user_Demo7_Print").checked)
                    ischecked = true;

                if (getobj("chk_user_Demo7_Digital").checked)
                    ischecked = true;
            }
            else if (getobj("chk_user_Demo7_Print")) {
                if (getobj("chk_user_Demo7_Print").checked)
                    ischecked = true;
            }
            else if (getobj("chk_user_Demo7_Digital")) {
                if (getobj("chk_user_Demo7_Digital").checked)
                    ischecked = true;
            }
            else if (getobj("chk_user_Demo7_Both")) {
                if (getobj("chk_user_Demo7_Both").checked)
                    ischecked = true;
            }
            else {
                ischecked = true;
            }

            args.IsValid = ischecked;
        } 

    </script>

    <div id="divcss">
        <style type='text/css'>
            body
            {
                background-color: #c0c0c0;
                text-align: center;
                padding: 20px 0;
                font-size: 12px;
                font-family: Arial;
            }
            #container
            {
                font-family: Arial;
                text-align: left;
                width: 760px;
                background-color: #;
                margin: 0 auto;
                border: 2px #000 solid;
                min-height: 100%;
                height: auto !important;
                height: 100%;
                width: 800px;
            }
            .Category
            {
                font-family: Arial;
                background-color: #c0c0c0;
                font-size: 12px;
                color: #;
            }
            .label
            {
                font-family: Arial;
                font-size: 12px;
                color: #000000;
                font-weight: normal;
            }
            .labelAnswer
            {
                font-family: Arial;
                font-size: 12px;
                color: #000000;
                font-weight: normal;
            }
        </style>
        <style>
            #question_COMP td
            {
                vertical-align: top;
                border: 0px solid black;
                padding: 1px 1px 1px 1px;
            }
            #question_COMP label
            {
                margin-left: 2px;
            }
            .style1
            {
                width: 3%;
                height: 13px;
            }
            .style2
            {
                width: 98%;
                height: 13px;
            }
            .style3
            {
                height: 27px;
            }
            .style4
            {
                width: 3%;
                height: 35px;
            }
            .style5
            {
                width: 98%;
                height: 35px;
            }
            .style6
            {
                width: 3%;
            }
            .style7
            {
                width: 98%;
            }
        </style>
    </div>
</head>
<body>
    <form runat="server">
    <div id="container" runat="server">
        <div id="innerContainer">
            <div>
                <div style="text-align: center">
                    <span style="font-size: 32px">Medtech Newsletter Comps</span></div>
                <span id="lblPageDesc"></span>
                <br />
                <div id="pnlFormStep2">
                    <input type="hidden" name="hidTRANSACTIONTYPE" id="hidTRANSACTIONTYPE" value="NEW" />
                    <input type="hidden" name="hidSUBSCRIBERID" id="hidSUBSCRIBERID" />
                    <div id="valSummary" class="errorsummary" style="color: Red; display: none;">
                    </div>
                    <asp:Panel ID="pnlErrorMessage" runat="Server" Visible="false" BorderColor="#A30100"
                        BorderStyle="Solid" BorderWidth="1px">
                        <div style="padding-left: 65px; padding-right: 5px; padding-top: 15px; padding-bottom: 5px;
                            height: 55px; background-image: url('../images/errorEx.jpg'); background-repeat: no-repeat">
                            <asp:Label ID="lblErrorMessage" runat="Server" ForeColor="Red"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlForm" runat="server">
                        <p align="left" style="padding-left: 5px;">
                            <span id="lblRequiredField">
                                <p>
                                    Required fields</p>
                            </span>
                            <p>
                            </p>
                            <div ID="pnlPersonalInfo">
                                <table width="100%">
                                    <tr>
                                        <td class="Category" 
                                            style="padding-top: 5px; padding-bottom: 5px; padding-left: 5px;">
                                            <b>Personal Account Information</b>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                            <div style="padding-left: 5px; padding-bottom: 10px">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <table border="0" width="100%">
                                                <tr>
                                                    <td class="label" valign="middle" width="20%">
                                                        <b>* <span>Email Address</span>: </b>
                                                    </td>
                                                    <td valign="middle" width="80%">
                                                        <input ID="txtEmailAddress" runat="server" maxlength="50" 
                                                            name="txtemailaddress" style="width: 250px;" type="text" />
                                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmailAddress">
                                                      <img src='../images/required_field.jpg'></span> <span 
                                                        id="rexEmail4" style="color: Red;
                                                        display: none;">Invalid Email Address</span> 
                                                    </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="padding-left: 0px;">
                                            <div ID="upQuestions">
                                                <table border="0" width="100%">
                                                    <tr ID="tr_q_COMP">
                                                        <td align="left" class="label" 
                                                            style="padding-top: 3px; padding-bottom: 3px; padding-left: 0px;" valign="top" 
                                                            width="20%">
                                                            Is this a comp record?
                                                        </td>
                                                        <td align="left" class="label" 
                                                            style="padding-top: 3px; padding-bottom: 3px; padding-left: 0px;" 
                                                            valign="middle" width="80%">
                                                            <table ID="question_COMP" border="0" class="labelAnswer">
                                                                <tr>
                                                                    <td>
                                                                        <input ID="question_COMP_0" runat="server" name="question_COMP$0" 
                                                                            type="checkbox" /><label for="question_COMP_0">Yes</label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                            <div ID="pnlNewslettersB">
                                <p>
                                    <br />
                                    <table align="center" border="0" cellpadding="2" cellspacing="0" width="100%">
                                        <tr class="Category" width="100%">
                                            <td align="left" style="padding: 6px 5px 6px 5px; font-weight: bold" 
                                                width="100%">
                                                <span ID="rptCategoryB_ctl00_lblcategoryName" class="Category">Newsletters</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top: 5px; padding-bottom: 5px;">
                                                <span class="label"><span ID="rptCategoryB_ctl00_lblNewsletterHeaderB">
                                                <p>
                                                    Check all that apply.</p>
                                                </span></span>
                                            </td>
                                        </tr>
                                        <tr bgcolor="#ffffff">
                                            <td align="left" width="100%">
                                                <div>
                                                    <table ID="rptCategoryB_ctl00_gvNewsletters" border="0" cellpadding="10" 
                                                        cellspacing="5" style="color: Black; width: 100%;">
                                                        <tr>
                                                            <td align="center" style="width: 3%;" valign="top">
                                                                <input ID="ChkNewsLetter_0" runat="server" name="ChkNewsLetter_0" 
                                                                    type="checkbox" />
                                                            </td>
                                                            <td align="left" style="width: 98%;">
                                                                <span ID="rptCategoryB_ctl00_gvNewsletters_ctl02_lblDisplayNameB" class="label" style="font-weight: bold;
                                                                font-style: italic;">Healthcare IT NewsDay</span>
                                                                <span ID="rptCategoryB_ctl00_gvNewsletters_ctl02_lblSpaceB" class="label" 
                                                                    style="font-weight: bold; font-style: italic;"></span>
                                                                <font class="labelAnswer">(Daily e-newsletter)</font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" class="style1" valign="top">
                                                                <input ID="ChkNewsLetter_1" runat="server" name="ChkNewsLetter_1" 
                                                                    type="checkbox" />
                                                            </td>
                                                            <td align="left" class="style2">
                                                                <span ID="rptCategoryB_ctl00_gvNewsletters_ctl03_lblDisplayNameB" class="label" style="font-weight: bold;
                                                                font-style: italic;">Healthcare IT NewsWeek</span>
                                                                <span ID="rptCategoryB_ctl00_gvNewsletters_ctl03_lblSpaceB" class="label" 
                                                                    style="font-weight: bold; font-style: italic;"></span>
                                                                <font class="labelAnswer">(Monday e-newsletter)</font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" class="style6" valign="top">
                                                                <input ID="ChkNewsLetter_2" runat="server" name="ChkNewsLetter_2" 
                                                                    type="checkbox" />
                                                            </td>
                                                            <td align="left" class="style7">
                                                                <span ID="rptCategoryB_ctl00_gvNewsletters_ctl04_lblDisplayNameB" class="label" style="font-weight: bold;
                                                                font-style: italic;">Healthcare Finance NewsDay</span>
                                                                <span ID="rptCategoryB_ctl00_gvNewsletters_ctl04_lblSpaceB" class="label" 
                                                                    style="font-weight: bold; font-style: italic;"></span>
                                                                <font class="labelAnswer">(Daily e-newsletter)</font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="width: 3%;" valign="top">
                                                                <input ID="ChkNewsLetter_3" runat="server" name="ChkNewsLetter_3" 
                                                                    type="checkbox" />
                                                            </td>
                                                            <td align="left" style="width: 98%;">
                                                                <span ID="rptCategoryB_ctl00_gvNewsletters_ctl05_lblDisplayNameB" class="label" style="font-weight: bold;
                                                                font-style: italic;">Healthcare Finance NewsWeek</span>
                                                                <span ID="rptCategoryB_ctl00_gvNewsletters_ctl05_lblSpaceB" class="label" 
                                                                    style="font-weight: bold; font-style: italic;"></span>
                                                                <font class="labelAnswer">(Tuesday e-newsletter)</font>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table align="center" border="0" cellpadding="2" cellspacing="0" width="100%">
                                        <tr class="Category" width="100%">
                                            <td align="left" class="style3" 
                                                style="padding: 6px 5px 6px 5px; font-weight: bold" width="100%">
                                                <span ID="rptCategoryB_ctl01_lblcategoryName" class="Category">Newsletter</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top: 5px; padding-bottom: 5px;">
                                                <span class="label"><span ID="rptCategoryB_ctl01_lblNewsletterHeaderB">
                                                <p>
                                                    Check all that apply.
                                                </p>
                                                </span></span>
                                            </td>
                                        </tr>
                                        <tr bgcolor="#ffffff">
                                            <td align="left" width="100%">
                                                <div>
                                                    <table ID="rptCategoryB_ctl01_gvNewsletters" border="0" cellpadding="10" 
                                                        cellspacing="5" style="color: Black; width: 100%;">
                                                        <tr>
                                                            <td align="center" style="width: 3%;" valign="top">
                                                                <input ID="ChkNewsLetter_4" runat="server" name="ChkNewsLetter_4" 
                                                                    type="checkbox" />
                                                            </td>
                                                            <td align="left" style="width: 98%;">
                                                                <font class="labelAnswer">&nbsp;<img align="absMiddle" 
                                                                    alt="Delivered on the fourth Tuesday of the month, Healthcare IT News Get Smart provides updates on live and on-demand Web seminars, informative white papers, industry research, content briefs and more." 
                                                                    border="0" 
                                                                    src="http://images.ecn5.com/Customers/3051/images/HITN_Get_Smart_News_Logo.jpg" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <a href="http://www.cfmediaview.com/ViewMessage.aspx?org=Medtech&amp;msg=4647" 
                                                                    target="_blank">Click here</a> to preview.</font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="width: 3%;" valign="top">
                                                                <input ID="ChkNewsLetter_5" runat="server" name="ChkNewsLetter_5" 
                                                                    type="checkbox" />
                                                            </td>
                                                            <td align="left" style="width: 98%;">
                                                                <font class="labelAnswer">&nbsp;<img align="absMiddle" 
                                                                    alt="Delivered on the first and third Wednesday of the month, Healthcare IT JobSpot provides the latest healthcare IT employment news and job postings." 
                                                                    border="0" 
                                                                    src="http://images.ecn5.com/Customers/3051/images/HITN_JobSpot_Newsletter_Logo.jpg" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <a href="http://www.cfmediaview.com/ViewMessage.aspx?org=Medtech&amp;msg=4673" 
                                                                    target="_blank">Click here </a>to preview.</font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="width: 3%;" valign="top">
                                                                <input ID="ChkNewsLetter_6" runat="server" name="ChkNewsLetter_6" 
                                                                    type="checkbox" />
                                                            </td>
                                                            <td align="left" style="width: 98%;">
                                                                <font class="labelAnswer">&nbsp;<img align="absMiddle" 
                                                                    alt="Delivered on the third Monday of the month, Healthcare IT News International provides a monthly roundup of global IT news from the international section of HealthcareITNews.com." 
                                                                    border="0" 
                                                                    src="http://images.ecn5.com/Customers/3051/images/HITN_INTL_Newsletter_logo.jpg" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <a href="http://www.cfmediaview.com/ViewMessage.aspx?org=Medtech&amp;msg=4847" 
                                                                    target="_blank">Click here</a> to preview.</font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" class="style4" valign="top">
                                                                <input ID="ChkNewsLetter_7" runat="server" name="ChkNewsLetter_7" 
                                                                    type="checkbox" />
                                                            </td>
                                                            <td align="left" class="style5">
                                                                <font class="labelAnswer">&nbsp;<img align="absMiddle" 
                                                                    alt="Delivered on the fourth Thursday of the month, Healthcare Finance News Get Smart provides updates on live and on-demand Web seminars, informative white papers, industry research, content briefs and more." 
                                                                    border="0" 
                                                                    src="http://images.ecn5.com/Customers/3051/images/HFN_GetSmart_Newsletter.gif" 
                                                                    title="Delivered on the fourth Thursday of the month, Healthcare Finance News Get Smart provides updates on live and on-demand Web seminars, informative white papers, industry research, content briefs and more." />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <a href="http://www.cfmediaview.com/ViewMessage.aspx?org=Medtech&amp;msg=4750" 
                                                                    target="_blank">Click here</a> to preview.</font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="width: 3%;" valign="top">
                                                                <input ID="ChkNewsLetter_8" runat="server" name="ChkNewsLetter_8" 
                                                                    type="checkbox" />
                                                            </td>
                                                            <td align="left" style="width: 98%;">
                                                                <font class="labelAnswer">
                                                                <div style="text-align: left">
                                                                    <img alt="" 
                                                                        src="http://images.ecn5.com/Customers/3051/images/HFJS_Newsletter_logo.jpg" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    <a href="http://staging.computerfulfillment.com/mv2/ViewMessage.aspx?org=Medtech&amp;msg=4612" 
                                                                        target="_blank">Click here</a> to preview.</div>
                                                                </font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="width: 3%;" valign="top">
                                                                <input ID="ChkNewsLetter_9" runat="server" name="ChkNewsLetter_9" 
                                                                    type="checkbox" />
                                                            </td>
                                                            <td align="left" style="width: 98%;">
                                                                <font class="labelAnswer">&nbsp;<img align="absMiddle" 
                                                                    alt="Delivered every Friday, HITECHWatch provides a roundup of coverage about the ARRA stimulus and its effect on healthcare, with contributions from industry thought leaders." 
                                                                    border="0" 
                                                                    src="http://images.ecn5.com/Customers/3051/images/HITECHWatch_Newsletter.gif" 
                                                                    title="Delivered every Friday, HITECHWatch provides a roundup of coverage about the ARRA stimulus and its effect on healthcare, with contributions from industry thought leaders." />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <a href="http://www.cfmediaview.com/ViewMessage.aspx?org=Medtech&amp;msg=4680" 
                                                                    target="_blank">Click here</a> to preview.</font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="width: 3%;" valign="top">
                                                                <input ID="ChkNewsLetter_10" runat="server" name="ChkNewsLetter_10" 
                                                                    type="checkbox" />
                                                            </td>
                                                            <td align="left" style="width: 98%;">
                                                                <font class="labelAnswer">
                                                                <table align="left" border="0" cellpadding="0" cellspacing="0" style="width: 460px;
                                                                    height: 29px" width="460">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>
                                                                                <img alt="Delivered every Monday, Healthcare Payer Newsweek provides original news to health plan executives and other professionals, covering the areas of IT, finance and policy surrounding the current landscape of regulated reimbursements." 
                                                                                    src="http://images.ecn5.com/Customers/3051/images/HCPN_sub_logo_form.gif" 
                                                                                    title="Delivered every Monday, Healthcare Payer Newsweek provides original news to health plan executives and other professionals, covering the areas of IT, finance and policy surrounding the current landscape of regulated reimbursements." />
                                                                            </td>
                                                                            <td>
                                                                                <a href="http://emailactivity.ecn5.com/engines/publicPreview.aspx?blastID=355600" 
                                                                                    target="_blank">Click here </a>to preview.
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="width: 3%;" valign="top">
                                                                <input ID="ChkNewsLetter_11" runat="server" name="ChkNewsLetter_11" 
                                                                    type="checkbox" />
                                                            </td>
                                                            <td align="left" style="width: 98%;">
                                                                <font class="labelAnswer">
                                                                <img align="absMiddle" 
                                                                    alt="Destination HIMSS, published 14 times in the weeks leading up to the HIMSS Annual Conference and Exhibition, offers news and tools that help attendess make the most of their time at the show." 
                                                                    border="0" 
                                                                    src="http://images.ecn5.com/Customers/3051/images/Destination_HIMSS_header_small.gif" 
                                                                    title="Destination HIMSS, published 14 times in the weeks leading up to the HIMSS Annual Conference and Exhibition, offers news and tools that help attendess make the most of their time at the show." 
                                                                    width="255" /></a></font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="width: 3%;" valign="top">
                                                                <input ID="ChkNewsLetter_12" runat="server" name="ChkNewsLetter_12" 
                                                                    type="checkbox" />
                                                            </td>
                                                            <td align="left" style="width: 98%;">
                                                                <font class="labelAnswer"><span style="font-size: 22px">
                                                                <span style="font-family: Arial">Healthcare IT&nbsp;News Telehealth Weekly</span></span></font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="width: 3%;" valign="top">
                                                                <input ID="ChkNewsLetter_13" runat="server" name="ChkNewsLetter_13" 
                                                                    type="checkbox" />
                                                            </td>
                                                            <td align="left" style="width: 98%;">
                                                                <font class="labelAnswer"><font color="#003366" size="4">
                                                                <table align="left" border="0" cellpadding="0" cellspacing="0" style="width: 460px;
                                                                    height: 29px" width="460">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>
                                                                                <img alt="" 
                                                                                    src="http://images.ecn5.com/Customers/3051/images/ghit_email_banner.gif" 
                                                                                    style="width: 299px; height: 35px" />
                                                                            </td>
                                                                            <td>
                                                                                <a href="http://emailactivity.ecn5.com/engines/publicPreview.aspx?blastID=355512" 
                                                                                    target="_blank">Click here </a>to preview.
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                                <span style="color: #000000"><span style="font-size: 10px">
                                                                <span style="font-family: Arial">
                                                                <br />
                                                                </span></span></span></font></font>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <br></br>
                                </p>
                            </div>
                            <br />
                            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" 
                                Text="Submit" />
                            &nbsp;<input ID="btnReset" name="btnReset" 
                                onclick="return confirm('Are you sure you want to clear the values?');" 
                                type="submit" value="Reset" /></p>
                    </asp:Panel>
                </div>
                <br />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
