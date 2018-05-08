<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Subscription.aspx.cs" Inherits="KMPS_JF.Forms.Subscription"
    Culture="auto:en-US" UICulture="auto" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<meta http-equiv="X-UA-Compatible" content="IE=edge" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title>Subscription Form</title>
    <link href="../CSS/styles.css" rel="stylesheet" type="text/css">
    <link rel="stylesheet" type="text/css" href="../css/ecnHighslide.css" />
    <link rel="stylesheet" type="text/css" href="../css/ecnHighslide-styles.css" />

    <script src="../scripts/highslide/highslide-full.js" type="text/javascript"></script>

    <script src="../scripts/jQuery/jquery-1.4.3.js" type="text/javascript"></script>

    <script src="../scripts/jQuery/jquery.maskedinput.js" type="text/javascript"></script>
    <script src='https://www.google.com/recaptcha/api.js' type="text/javascript"></script>
    <script>
        hs.graphicsDir = '../scripts/highslide/graphics/';
        hs.outlineType = 'rounded-white';
        hs.allowSizeReduction = 'false';
        hs.objectLoadTime = 'after';
    </script>

    <style>
        .labelAnswer input {
            vertical-align: middle;
        }

        .labelAnswer label {
            padding-left: 5px;
            padding-right: 10px;
            vertical-align: middle;
        }
    </style>

    <script type="text/javascript">
        var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www.");
        document.write(unescape("%3Cscript src='" + gaJsHost + "google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E"));
    </script>
 
    <script type="text/javascript">

        function changeFocus(e) {
            //        try{
            //            var src = e ? e.target : event.srcElement;
            //            if (src.value.length < src.maxLength) return true;
            //            if (src.id == "question_Voice1") {
            //                var target = document.getElementById('question_Voice2');
            //                return target.focus();
            //            }
            //            if (src.id == "question_Voice2") {
            //                var target = document.getElementById('question_Voice3');
            //                return target.focus();
            //            }
            //            if (src.id == "question_Fax1") {
            //                var target = document.getElementById('question_Fax2');
            //                return target.focus();
            //            }
            //            if (src.id == "question_Fax2") {
            //                var target = document.getElementById('question_Fax3');
            //                return target.focus();
            //            }
            //            }
            //            catch(Error e)
            //            {}
            //            


        }
        onload = function () {
            //       try{
            //            var A = document.getElementById('question_Voice1');
            //            A.onkeyup = changeFocus;
            //            var B = document.getElementById('question_Voice2');
            //            B.onkeyup = changeFocus;

            //            var C = document.getElementById('question_Fax1');
            //            C.onkeyup = changeFocus;
            //            var D = document.getElementById('question_Fax2');
            //            D.onkeyup = changeFocus;
            //            }
            //            catch(Error e)            
            //            {}
        }

        function googleTracking(step) {
            try {
                var pageTracker = _gat._getTracker("UA-10775168-1");
                pageTracker._trackPageview(step);
            } catch (err) {
            }

            try {
                var ECNpageTracker = _gat._getTracker("UA-2081962-10");
                ECNpageTracker._trackPageview();
            } catch (err) {
            }
        }

    </script>

    <!-- START Conversion Tracking -->

    <script language="javascript" src="getConversionData.js"></script>

    <script language="javascript" src="../scripts/Validation.js"></script>

    <script language="javascript" src="../scripts/jQuery/jquery-1.4.3.js"></script>

    <!-- END Conversion Tracking -->

    <script language="javascript">
        function getobj(id) {
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
                    if (getobj(optionctrlname).selectedIndex == optionCount) {
                        bEnable = true;
                    }
                }

                if (bEnable) {
                    ValidatorEnable(getobj("rfv_" + textctrlname), true);
                    getobj("pnl_" + textctrlname).style.display = "block";
                    getobj(textctrlname).disabled = false;
                }
                else {
                    ValidatorEnable(getobj("rfv_" + textctrlname), false);
                    getobj("pnl_" + textctrlname).style.display = "none";
                    getobj(textctrlname).value = "";
                    getobj(textctrlname).disabled = true;
                }

            }
            catch (err) {
                return null;
            }
        }

        

        function Page_ValidationSummariesReset() {
            if (typeof (Page_ValidationSummaries) == "undefined")
                return;
            for (var i = 0; i < Page_ValidationSummaries.length; i++)
                Page_ValidationSummaries[i].style.display = "none";

        }

        String.prototype.startsWith = function (str)
        { return (this.match("^" + str) == str) }

        function CatCheckBox_RFV(ctrlname) {
            for (counter = 0; counter < document.forms[0].elements.length; counter++) {
                elm = document.forms[0].elements[counter];
                if (elm.type == "checkbox") {
                    var checkboxname = document.forms[0].elements[counter].name;
                    if (checkboxname == ctrlname) {
                        if (document.forms[0].elements[counter].checked)
                            return true;
                    }
                }
            }
            return false;
        }

        function CheckBox_RFV(ctrlname, IsRequired, maxSelections) {

            //alert(ctrlname + ' / ' + maxSelections);

            if (maxSelections == 0 && IsRequired == 1) {
                for (counter = 0; counter < document.forms[0].elements.length; counter++) {
                     
                    elm = document.forms[0].elements[counter];
                    if (elm.type == "checkbox") {
                        var checkboxname = document.forms[0].elements[counter].name;
                        if (checkboxname.startsWith(ctrlname + "$")) {

                            if (document.forms[0].elements[counter].checked)
                                return true;
                        }
                    }
                }
            }
            else if (maxSelections > 0)
            {
                currentSelections = 0;
                for (counter = 0; counter < document.forms[0].elements.length; counter++) {

                    elm = document.forms[0].elements[counter];
                    if (elm.type == "checkbox") {
                        var checkboxname = document.forms[0].elements[counter].name;
                        if (checkboxname.startsWith(ctrlname + "$")) {

                            if (document.forms[0].elements[counter].checked) {
                                currentSelections++;
                            }
                        }
                    }
                }

                if (currentSelections > 0)
                {
                    if (currentSelections <= maxSelections)
                        return true;
                    else
                        return false;
                }
                else if (IsRequired == 0) {
                    return true;
                }
                else if (IsRequired == 1)
                {
                    return false;
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
            else if (getobj("chk_user_Demo7_Print") && getobj("chk_user_Demo7_Both")) {
                if (getobj("chk_user_Demo7_Print").checked)
                    ischecked = true;

                if (getobj("chk_user_Demo7_Both").checked)
                    ischecked = true;
            }
            else if (getobj("chk_user_Demo7_Digital") && getobj("chk_user_Demo7_Both")) {
                if (getobj("chk_user_Demo7_Digital").checked)
                    ischecked = true;

                if (getobj("chk_user_Demo7_Both").checked)
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

        function SetPanels(nlCat, nlHeader) {

            var imgID = nlCat.id;
            var Cat = imgID.substring(imgID.length - 1);
            var divID = imgID.substring(0, 18) + "_divNewsletterDetails" + Cat;

            var divNL = getobj(divID);

            if (divNL.style.display == 'none') {
                divNL.style.display = 'block';

                if (nlHeader != '')
                    getobj(nlHeader).style.display = 'block';

                getobj(imgID).src = 'http://eforms.kmpsgroup.com/jointforms/images/collapse_blue.png';
                getobj(imgID).title = 'Click to collapse';

            } else {
                divNL.style.display = 'none';

                if (nlHeader != '')
                    getobj(nlHeader).style.display = 'none';

                getobj(imgID).src = 'http://eforms.kmpsgroup.com/jointforms/images/expand_blue.png';
                getobj(imgID).title = 'Click to expand';
            }
        }

        function CheckUncheckCategory(ctrl) {

            var countUnchecked = 0;
            var countChecked = 0;
            var countControls = 0;

            for (var i = 0; i < 10; i++) {

                var chkid = ctrl.id.substring(0, 18) + "_gvNewsletters_ctl" + (i < 10 ? ("0" + i) : i) + "_chkselect";
                var obj = getobj(chkid);

                if (obj != null) {

                    countControls++;

                    if (!obj.checked) {
                        countUnchecked++;
                    }
                    else {
                        countChecked++;
                    }
                }
            }

            var catid = ctrl.id.substring(0, 18) + "_chkNewsletterCat";

            if (countUnchecked > 0) {
                if (getobj(catid) != null) {
                    getobj(catid).checked = false;
                }
            }
            else if (countChecked > 0 && countChecked == countControls) {
                if (getobj(catid) != null) {
                    getobj(catid).checked = true;
                }
            }
        }

        function CheckNewsletters(ctrl) {
            for (var i = 0; i < 100; i++) {

                var chkid = ctrl.id.substring(0, 18) + "_gvNewsletters_ctl" + (i < 10 ? ("0" + i) : i) + "_chkselect";
                var obj = getobj(chkid);

                if (obj != null) {
                    if (!obj.checked && ctrl.checked) {
                        obj.checked = true;
                    }
                    else if (!ctrl.checked && obj.checked) {
                        obj.checked = false;
                    }
                }
            }
        }

        function Trim(str) {
            if (str != null) {
                return str.replace(/^\s+|\s+$/g, "");
            }
        }

        function ValidateStateAndZip(source, args) {
            var country = document.getElementById('drpCountry');
            var state = document.getElementById('question_State');

            if (state == null) {
                state = document.getElementById('question_STATE');

            }

            var zip = document.getElementById('question_Zip');

            if (zip == null) {
                zip = document.getElementById('question_ZIP');

            }


            if (state != null && zip != null) {
                if (Trim(zip.value).length > 0 && Trim(state.value).length > 0) {
                    ValidateStateZip(source, args, country.value, state.value, zip.value);
                }
            }
        }

        function ValidatePhone(source, args) {
           
            var isValid = false;

            var Voice1 = document.getElementById('question_Voice1');
            var Voice2 = document.getElementById('question_Voice2');
            var Voice3 = document.getElementById('question_Voice3');

            if (Trim(Voice1.value).length > 0 && Trim(Voice2.value).length > 0 && Trim(Voice3.value).length > 0)
            {
                isValid = true;
            }

            args.IsValid = isValid;
        }

        function ValidateFax(source, args) {

            var isValid = false;

            var Fax1 = document.getElementById('question_Fax1');
            var Fax2 = document.getElementById('question_Fax2');
            var Fax3 = document.getElementById('question_Fax3');

            if (Trim(Fax1.value).length > 0 && Trim(Fax2.value).length > 0 && Trim(Fax3.value).length > 0) {
                isValid = true;
            }

            args.IsValid = isValid;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

    </script>

    <div id="divcss" runat="server">
    </div>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div id="container">
            <div id="innerContainer">
                <div>
                    <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
                    <asp:Label ID="lblPageDesc" runat="server"></asp:Label>
                    <br />
                    <asp:GridView ID="gvNewsletterResults" runat="server" AutoGenerateColumns="false"
                        CellPadding="2" CellSpacing="5" DataKeyNames="EcnGroupID" SkinID="skinewslettersearch">
                        <Columns>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"
                                ItemStyle-VerticalAlign="Top" ItemStyle-Width="3%" ControlStyle-BorderStyle="None"
                                ItemStyle-CssClass="addpadding">
                                <ItemTemplate>
                                    <input type="checkbox" id="chkselect" runat="server" checked='<%# Eval("subscribed").ToString().ToUpper()=="Y"?true:false %>' />
                                    <asp:Label ID="lblCustomerID" runat="server" Text='<%# Eval("CustomerID").ToString()%>'
                                        Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"
                                ItemStyle-Width="97%" ItemStyle-CssClass="addpadding">
                                <ItemTemplate>
                                    <asp:Label ID="lblDisplayName" runat="server" Text='<%# Eval("Displayname").ToString()%>'
                                        CssClass="label" Font-Bold="true" Font-Italic="true" Visible='<%# Convert.ToBoolean(Eval("ShowDisplayName").ToString()) %>' />
                                    <%--<asp:Label ID="lblSpace" runat="server" Text='&nbsp;&nbsp;' CssClass="label" Font-Bold="true"
                                        Font-Italic="true" Visible='<%# Convert.ToBoolean(Eval("ShowDisplayName").ToString()) %>' />--%>
                                    <font class="labelAnswer">
                                        <asp:Label ID="lblDescription" CssClass="labelAnswer" runat="server" Text='<%# Eval("Description") %>' />
                                    </font>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                        <br>
                        <table cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
                            <tr>
                                <td id="errorMiddle" align="left">
                                    <table height="67" align="center" width="100%" border="0" style="padding-top: 5px; padding-bottom: 5px;">
                                        <tr>
                                            <td valign="top" align="center" width="20%">
                                                <img src="../images/errorEx.jpg" />
                                            </td>
                                            <td valign="middle" align="left" width="80%" height="100%">
                                                <asp:Label ID="lblErrorMessage" runat="Server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </asp:PlaceHolder>
                    <ajaxToolkit:RoundedCornersExtender ID="rounderCorneremailValidation" runat="server"
                        Corners="All" Radius="10" BorderColor="Black" TargetControlID="pnlEmailValidationPopup" />
                    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderEmailValidation" runat="server"
                        BackgroundCssClass="modalBackground" PopupControlID="pnlEmailValidationPopup"
                        TargetControlID="btnAddEmailValidation" CancelControlID="btnCancelProductEmail" />
                    <asp:Button ID="btnAddEmailValidation" Text="Add" runat="server" Style="display: none;"
                        CausesValidation="false" UseSubmitBehavior="false" />
                    <asp:Panel ID="pnlEmailValidationPopup" runat="server" CssClass="popupbody" Width="700px"
                        Height="130px" Style="display: none;">
                        <table border="0" align="center" style="padding-top: 20px;">
                            <tr>
                                <td style="padding: 5px 5px 5px 5px">
                                    <asp:Label ID="lblEmailValidationText" runat="server" Font-Bold="true" ForeColor="Red" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="padding-top: 15px;">
                                    <asp:Button ID="btnCancelProductEmail" runat="server" Text="OK" Width="50" CausesValidation="false"
                                        UseSubmitBehavior="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="pnlSearch" runat="server" Style="display: none;">
                        <table width="100%">
                            <tr>
                                <td align="right">Search Newsletters:&nbsp;&nbsp;
                                <asp:TextBox ID="txtSearch" runat="server" CausesValidation="false" />&nbsp;
                                <asp:Button ID="btnSearch" CausesValidation="false" UseSubmitBehavior="false" runat="server"
                                    Text="  Go  " OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <ajaxToolkit:RoundedCornersExtender ID="rceNewsletter" runat="server" Corners="All"
                        Radius="10" BorderColor="Black" TargetControlID="pnlNewsletterSearchResults"
                        Enabled="false" />
                    <ajaxToolkit:ModalPopupExtender ID="extpnlNewsletter" runat="server" BackgroundCssClass="modalBackground"
                        PopupControlID="pnlNewsletterSearchResults" TargetControlID="btnAdd" />
                    <asp:Button ID="btnAdd" Text="Add" runat="server" Style="display: none;" CausesValidation="false"
                        UseSubmitBehavior="false" />
                    <asp:Panel ID="pnlNewsletterSearchResults" runat="server" CssClass="popupbody" Width="800px"
                        Style="display: none;">
                        <table>
                            <tr>
                                <td style="padding-left: 10px; padding-right: 10px;">
                                    <asp:Label ID="lblResultMsg" runat="server" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 10px; padding-right: 10px;">&nbsp;
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table border="0" width="100%">
                            <tr>
                                <td align="right" style="padding-right: 10px;">
                                    <asp:Button ID="btnNewsletterSearchResults" runat="server" Text="    OK      " CausesValidation="false"
                                        UseSubmitBehavior="false" OnClick="btnNewsletterSearchResults_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlRedirectMsg" runat="server" Visible="false">
                        <asp:Label ID="lblRedirectMsg" runat="server" />
                    </asp:Panel>
                    <asp:Panel ID="pnlHomePage" runat="server" Visible="true" Height="265px">
                        <asp:PlaceHolder ID="plNewSubscriptionHeader" runat="server"></asp:PlaceHolder>
                        <br />
                        <asp:LinkButton ID="lnkNewSubscription" runat="server" Text="" Font-Size="Medium"
                            Style="padding-bottom: 20px;" OnClick="lnkNewSubscription_Click"></asp:LinkButton><br />
                        <br />
                        <asp:PlaceHolder ID="plManageSubscriptionHeader" runat="server"></asp:PlaceHolder>
                        <br />
                        <asp:LinkButton ID="lnkManageSubscription" runat="server" Text="" Font-Size="Medium"
                            Style="padding-bottom: 20px;" OnClick="lnkManageSubscription_Click" meta:resourceKey="lnkManageSubscription"></asp:LinkButton><br />
                        <br />
                        <asp:LinkButton ID="lnkCustomerService" runat="server" Text="Customer Service" Font-Size="Medium"
                            Style="padding-bottom: 20px;" OnClick="lnkCustomerService_Click"></asp:LinkButton><br />
                        <br />
                        <asp:LinkButton ID="lnkTradeShow" runat="server" Text="Click here to visit our related Trade Shows"
                            Font-Size="Medium" Style="padding-bottom: 20px;" OnClick="lnkTradeShow_Click"></asp:LinkButton><br />
                        <br />

                        <script language="Javascript">

                            //                        ECNstepname = 'HomePage';
                            //                        document.write("<img src='" + getConversionURL() + "' height=1 width=1 border=0>");
                            //                        googleTracking('/home.html'); 	
                        </script>

                    </asp:Panel>
                    <asp:Panel ID="pnlLogin" runat="server" Visible="true" Height="265px">

                        <script language="Javascript">
                            //	
                            //                        ECNstepname = 'Login';
                            //                        document.write("<img src='" + getConversionURL() + "' height=1 width=1 border=0>");
                            //                        googleTracking('/login.html');

                        </script>

                        <asp:Label ID="lblLoginPageDesc" runat="server"></asp:Label><asp:HiddenField ID="hidVerificationType"
                            runat="server" />
                        <table width="100%" cellpadding="10" cellspacing="5" border="0">
                            <tr>
                                <td valign="top" align="left" colspan="3">
                                    <asp:Image runat="server" ID="imgPubLabel" Height="75px" Width="250px" alt="" ImageUrl="~/images/PUB-label.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left">
                                    <table width="100%">
                                        <tr>
                                            <td valign="top" align="left" class="label" width="50%">
                                                <asp:Label ID="lblSubscriberID" Text="<%$ Resources:Resource, subscriberID %>" runat="server"></asp:Label><br />
                                                <asp:TextBox ID="txtSubscriberID" runat="server" Width="175" TabIndex="4"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                                    ID="rfvS" ControlToValidate="txtSubscriberID" ValidationGroup="groupSubscriber"
                                                    ErrorMessage="<< required." runat="server" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator><br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:Label CssClass="label" ID="lblVerfication" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblVerficationTEXT" Text="<%$ Resources:Resource, forverfication %>"
                                                    runat="server"></asp:Label><br />
                                                <asp:TextBox ID="txtVerification" runat="server" Width="175" TabIndex="5"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                                    ID="rfvV" ControlToValidate="txtVerification" CssClass="label" ErrorMessage="<< required."
                                                    ValidationGroup="groupSubscriber" runat="server" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:Button ID="btnLoginWithSubscriberID" runat="server" Text="<%$ Resources:Resource, login %>"
                                                    OnClick="btnLoginWithSubscriberID_Click" ValidationGroup="groupSubscriber" TabIndex="6"
                                                    UseSubmitBehavior="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="left" valign="top" style="padding-left: 0px; padding-right: 40px; padding-top: 30px;">
                                    <img id="imgOR" src="../Images/or.png" runat="server" visible="true" />
                                </td>
                                <td valign="top">
                                    <table align="left">
                                        <asp:Panel ID="pnlEmailAddress" runat="server">
                                            <tr>
                                                <td valign="top" class="label">
                                                    <asp:Label Text="<%$ Resources:Resource, emailaddress %>" runat="server"></asp:Label><br />
                                                    <asp:TextBox ID="txtUserName" runat="server" Width="175" MaxLength="50" TabIndex="1"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator7" ControlToValidate="txtUserName" ValidationGroup="groupUserName"
                                                        ErrorMessage="<< required." runat="server" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator>
                                                    <asp:Label ID="lblUserNameInvalid" runat="server" Text="&lt;br&gt;Please enter a valid email address." Visible="false" />
                                                    <%--<asp:RegularExpressionValidator
                                                            ValidationGroup="groupUserName" ID="RegularExpressionValidator2" runat="server"
                                                            ControlToValidate="txtUserName" Display="Dynamic" ErrorMessage="&lt;br&gt;Please enter a valid email address."
                                                            ValidationExpression="^([a-zA-Z0-9_\-\.+]+)@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,10})$"></asp:RegularExpressionValidator>--%><br />
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlPasswordAndLogin" runat="server">
                                            <tr>
                                                <td valign="top" class="label">
                                                    <br />
                                                    <asp:Label ID="lblUserPassword" Text="<%$ Resources:Resource, password %>" runat="server"></asp:Label><br />
                                                    <asp:TextBox ID="txtUserPassword" runat="server" Width="175" MaxLength="25" TextMode="Password"
                                                        TabIndex="2"></asp:TextBox>&nbsp;
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <tr>
                                            <asp:Panel ID="pnlLoginButton" runat="server" Visible="true">
                                                <td valign="top">
                                                    <asp:Button ID="btnLoginWithUserName" runat="server" Text="<%$ Resources:Resource, login %>"
                                                        OnClick="btnLoginWithUserName_Click" TabIndex="3" ValidationGroup="groupUserName"
                                                        UseSubmitBehavior="true" />
                                                </td>
                                            </asp:Panel>
                                        </tr>
                                        <asp:Panel ID="pnlCantAccessAccount" runat="server">
                                            <tr>
                                                <td>
                                                    <a style="font-size: 11px" href='PasswordHelp.aspx?enableCS=<%= EnableCS %>&CustomerId=<%= CustomerID %>&GroupID=<%= GroupID %>&pubcode=<%= PubCode%>'
                                                        onclick="return hs.htmlExpand(this, {objectType: 'iframe', height:380, width:550, objectHeight:380, outlineType: 'rounded-white', dimmingOpacity: 0.5, preserveContent : false } )">
                                                        <asp:Label ID="lblCantaccessaccount" Text="<%$ Resources:Resource, cantaccessaccount %>"
                                                            runat="server"></asp:Label></a>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlFormStep1" runat="server" Visible="true">

                        <script language="Javascript">
                            //                        ECNstepname = 'New';
                            //                        document.write("<img src='" + getConversionURL() + "' height=1 width=1 border=0>");   
                            //                        googleTracking('/new1.html'); 
                        </script>

                        <table width="100%" cellpadding="10" cellspacing="5" border="0">
                            <tr>
                                <td width="10%" valign="middle" class="label">
                                    <asp:Label Text="<%$ Resources:Resource, emailaddress %>" runat="server"></asp:Label>:
                                </td>
                                <td align="left" valign="middle" width="50%" class="label">
                                    <asp:TextBox ID="txtLoginEmailAddress" runat="server" Width="200" OnTextChanged="txtLoginEmailAddress_TextChanged"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator2" ControlToValidate="txtLoginEmailAddress" ErrorMessage="Email Address"
                                        runat="server" Display="Dynamic" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator>
                                    <asp:Label ID="lblLoginEmailInvalid" runat="server" Visible="false" Text="Invalid Email Address" />
                                    <%--<asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtLoginEmailAddress"
                                            Display="Dynamic" ErrorMessage="<%$ Resources:Resource, emailaddressvalid %>"
                                            ValidationExpression="^([a-zA-Z0-9_\-\.+]+)@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,10})$"></asp:RegularExpressionValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" class="label">
                                    <asp:Label Text="<%$ Resources:Resource, country %>" runat="server"></asp:Label>:
                                </td>
                                <td valign="middle" align="left">
                                    <asp:DropDownList ID="drpNewCountry" runat="server" CssClass="labelAnswer" AppendDataBoundItems="true">
                                        <asp:ListItem Text="<%$ Resources:Resource, selectcountry %>" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="drpNewCountry"
                                        InitialValue="" runat="server" ErrorMessage="Country" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <asp:Label ID="lblNewErrorMessage" runat="server" ForeColor="Red" Font-Size="X-Small"
                                        Visible="false" Font-Bold="true"></asp:Label><br />
                                    <br />
                                    <asp:Button ID="btnStep1Submit" runat="server" Text="<%$ Resources:Resource, submit %>"
                                        OnClick="btnStep1Submit_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlFormStep2" runat="server" Visible="true">
                        <asp:HiddenField ID="hidTRANSACTIONTYPE" runat="server" />
                        <asp:HiddenField ID="hidSUBSCRIBERID" runat="server" />
                        <asp:ValidationSummary ID="valSummary" runat="server" CssClass="errorsummary" DisplayMode="BulletList"
                            HeaderText="<%$ Resources:Resource, warningmessage %>" ShowMessageBox="false" />
                        <p align="left" style="padding-left: 5px;">
                            <asp:Label ID="lblRequiredField" Text="" runat="server"></asp:Label>
                        </p>
                        <asp:Panel ID="pnlSubscription" runat="server" Visible="true" HorizontalAlign="left">
                            <p align="left" style="padding-left: 5px;">
                                <span class="label">*
                                <asp:Label ID="lblSubscriptionQuestion" runat="server"></asp:Label></span><asp:RequiredFieldValidator
                                    ControlToValidate="rbuser_SUBSCRIPTION" runat="server" ErrorMessage="Do you wish to receive/continue receiving your copy"
                                    Text="<%$ Resources:Resource, requiredfieldImage %>" Display="Dynamic"></asp:RequiredFieldValidator><br>
                                <asp:RadioButtonList ID="rbuser_SUBSCRIPTION" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="table" OnSelectedIndexChanged="rbuser_SUBSCRIPTION_SelectedIndexChanged"
                                    AutoPostBack="true" CellSpacing="4" CellPadding="5">
                                    <asp:ListItem Value="Y" Text="<%$ Resources:Resource, yes %>"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="<%$ Resources:Resource, no %>"></asp:ListItem>
                                </asp:RadioButtonList>
                            </p>
                        </asp:Panel>
                        <asp:Panel ID="pnlReceiveCopy" runat="server" Visible="false">
                            <p align="left" style="padding-left: 5px;">
                                <span class="label">
                                    <asp:Label ID="lblPRINTDIGITALQuestion" runat="server"></asp:Label></span><asp:CustomValidator
                                        ID="CustomValidator1" ErrorMessage="How would you like to receive your copy?"
                                        ClientValidationFunction="cv_question_demo7_print_or_digital" Display="Static"
                                        Text="<%$ Resources:Resource, requiredfieldImage %>" runat="server" /><br>
                                <asp:RadioButton ID="chk_user_Demo7_Print" runat="server" Text="<%$ Resources:Resource, print %>"
                                    AutoPostBack="true" OnCheckedChanged="chk_user_Demo7_Print_CheckedChanged" />&nbsp;&nbsp;
                            <asp:RadioButton ID="chk_user_Demo7_Digital" runat="server" Text="<%$ Resources:Resource, digital %>"
                                AutoPostBack="true" OnCheckedChanged="chk_user_Demo7_Digital_CheckedChanged" />
                                &nbsp;&nbsp;
                            <asp:RadioButton ID="chk_user_Demo7_Both" runat="server" Text=" Both Print and Digital" Visible="false"
                                AutoPostBack="true" OnCheckedChanged="chk_user_Demo7_Both_CheckedChanged" />
                            </p>
                        </asp:Panel>
                        <asp:Panel ID="pnlNewslettersA" runat="server" Visible="false">
                            <p>
                                <div id="divNewsletterHeaderA" style="display: block;">
                                    <span class="label">
                                        <asp:Label ID="lblNewsletterHeaderA" Text="" runat="server"></asp:Label></span>
                                </div>
                                <br />
                                <asp:Repeater ID="rptCategoryA" runat="Server" OnItemDataBound="rptCategory_ItemDataBound">
                                    <ItemTemplate>
                                        <br />
                                        <table cellpadding="2" cellspacing="0" border="0" width="100%" align="center">
                                            <tr class="Category" width="100%">
                                                <td align="left" width="100%" style="padding: 6px 5px 6px 5px; font-weight: bold">
                                                    <a href="javascript:void(0);">
                                                        <img id="imgCollapseA" runat="server" onclick="SetPanels(this,'')" alt="" src="http://eforms.kmpsgroup.com/jointforms/images/collapse_blue.png"
                                                            border="0" visible="true" title="Click to collapse" /></a>
                                                    <input type="checkbox" id="chkNewsletterCat" runat="server" onclick="CheckNewsletters(this)" />
                                                    <asp:Label ID="lblcategoryName" CssClass="Category" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryName") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr bgcolor="#ffffff">
                                                <td align="left" width="100%">
                                                    <div id="divNewsletterDetailsA" runat="server" style="display: block;">
                                                        <asp:GridView ID="gvNewsletters" runat="server" AllowPaging="false" AllowSorting="false"
                                                            AutoGenerateColumns="False" CellPadding="10" CellSpacing="5" DataKeyNames="ECNGroupID"
                                                            ForeColor="Black" GridLines="None" ShowFooter="false" ShowHeader="false" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"
                                                                    ItemStyle-VerticalAlign="Top" ItemStyle-Width="3%">
                                                                    <ItemTemplate>
                                                                        <input type="checkbox" id="chkselect" runat="server" onclick="CheckUncheckCategory(this)"
                                                                            checked='<%# Eval("subscribed").ToString().ToUpper()=="Y"?true:false %>' />
                                                                        <asp:Label ID="lblCustomerID" runat="server" Text='<%# Eval("CustomerID").ToString()%>'
                                                                            Visible="false" />
                                                                        <asp:Label ID="lblNewsLetterID" runat="server" Text='<%# Eval("NewsLetterID").ToString()%>'
                                                                            Visible="false" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"
                                                                    ItemStyle-Width="98%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDisplayName" runat="server" Text='<%# Eval("Displayname").ToString()%>'
                                                                            CssClass="label" Font-Bold="true" Font-Italic="true" Visible='<%# Convert.ToBoolean(Eval("ShowDisplayName").ToString()) %>' />
                                                                        <%--<asp:Label ID="lblSpace" runat="server" Text='&nbsp;&nbsp;' CssClass="label" Font-Bold="true"
                                                                            Font-Italic="true" Visible='<%# Convert.ToBoolean(Eval("ShowDisplayName").ToString()) %>' />--%>
                                                                        <asp:Label ID="lblDescription" CssClass="labelAnswer" runat="server" Text='<%# Eval("Description") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <br>
                                <p>
                                </p>
                            </p>
                        </asp:Panel>
                        <asp:Panel ID="pnlPersonalInfo" runat="server">
                            <table width="100%">
                                <tr>
                                    <td class="Category" style="padding-top: 5px; padding-bottom: 5px; padding-left: 5px;">
                                        <asp:Label ID="lblCopy" runat="server" Text="" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Category" style="padding-top: 5px; padding-bottom: 5px; padding-left: 5px;">
                                        <asp:Label ID="lblPersonalAccountInfo" runat="server" Font-Bold="true" Text="<%$ Resources:Resource, PersonalAccountInfo %>" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </asp:Panel>
                        <div style="padding-left: 5px; padding-bottom: 10px">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <table border="0" width="100%">
                                            <tr>
                                                <td runat="server" class="label" style="padding-top: 3px; padding-bottom: 3px; padding-left: 0px;" valign="top" width="<%$ Resources:Resource, profileleftColWidth %>">
                                                    <b>*
                                                    <asp:Label runat="server" Text="<%$ Resources:Resource, country %>"></asp:Label>:</b>
                                                </td>
                                                <td runat="server" style="padding-top: 3px; padding-bottom: 3px; padding-left: 0px;" width="<%$ Resources:Resource, profileRightColWidth %>">
                                                    <asp:DropDownList ID="drpCountry" runat="server" AppendDataBoundItems="true" AutoPostBack="True"
                                                        class="labelAnswer" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged"
                                                        Width="250">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpCountry"
                                                        Display="Dynamic" ErrorMessage="Country" InitialValue="" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table border="0" width="100%">
                                            <tr>
                                                <td runat="server" style="padding-top: 3px; padding-bottom: 3px; padding-left: 0px;" width="<%$ Resources:Resource, profileleftColWidth %>" class='label'
                                                    valign="middle">
                                                    <b>*
                                                    <asp:Label Text="<%$ Resources:Resource, emailaddress %>" runat="server"></asp:Label>:
                                                    </b>
                                                </td>
                                                <td runat="server" valign="middle" style="padding-top: 3px; padding-bottom: 3px; padding-left: 0px;" width="<%$ Resources:Resource, profileRightColWidth %>">
                                                    <asp:TextBox ID="txtemailaddress" OnTextChanged="txtemailaddress_TextChanged" runat="server"
                                                        MaxLength="50" Width="250" AutoPostBack="true"></asp:TextBox>
                                                    <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator4" ControlToValidate="txtemailaddress" Display="Dynamic"
                                                            InitialValue="" runat="server" ErrorMessage="Email Address" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator>
                                                    <asp:Label ID="lblInvalidEmail" runat="server" ForeColor="Red" Visible="false" Text="Invalid Email Address" />
                                                    <%--<asp:RegularExpressionValidator
                                                                ID="rexEmail4" runat="server" ControlToValidate="txtemailaddress" Display="Dynamic"
                                                                ErrorMessage="Invalid Email Address" ForeColor="Red" Text="&nbsp;Invalid Email Address" ValidationExpression="^.*?"></asp:RegularExpressionValidator>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <asp:Panel ID="pnlPassword" runat="server" Visible="false">
                                    <tr>
                                        <td>
                                            <table border="0" width="100%">
                                                <tr>
                                                    <td runat="server" align="left" style="padding-top: 3px; padding-bottom: 3px; padding-left: 0px;" width="<%$ Resources:Resource, profileleftColWidth %>"
                                                        class="label" valign="middle">
                                                        <b>*
                                                        <asp:Label ID="lblPassword" Text="<%$ Resources:Resource, password %>" runat="server"></asp:Label>:
                                                        </b>
                                                    </td>
                                                    <td runat="server" align="Left" style="padding-top: 3px; padding-bottom: 3px; padding-left: 0px;" width="<%$ Resources:Resource, profileRightColWidth %>"
                                                        valign="top">
                                                        <asp:TextBox ID="txtPassword" runat="server" MaxLength="25" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="rfvPassword" ControlToValidate="txtPassword" InitialValue="" runat="server"
                                                            ErrorMessage="<%$ Resources:Resource, password %>" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                                ID="regPassword" runat="server" ValidationExpression="[a-zA-Z0-9_]{6,}" ErrorMessage="Invalid Password Format"
                                                                ControlToValidate="txtPassword"></asp:RegularExpressionValidator><br />
                                                        <asp:Label ID="lblPasswordText" runat="server" Text="<%$ Resources:Resource, passwordText %>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table border="0" width="100%">
                                                <tr>
                                                    <td runat="server" align="left" style="padding-top: 3px; padding-bottom: 3px; padding-left: 0px;" width="<%$ Resources:Resource, profileleftColWidth %>"
                                                        class="label" valign="middle">
                                                        <b>*
                                                        <asp:Label ID="lblConfirmPassword" Text="<%$ Resources:Resource, confirmpassword %>"
                                                            runat="server"></asp:Label>: </b>
                                                    </td>
                                                    <td runat="server" align="Left" style="padding-top: 3px; padding-bottom: 3px; padding-left: 0px;" valign="middle" width="<%$ Resources:Resource, profileRightColWidth %>">
                                                        <asp:TextBox ID="txtConfirmPassword" runat="server" MaxLength="25" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="rfvcPassword" ControlToValidate="txtConfirmPassword" Display="Dynamic" InitialValue=""
                                                            runat="server" ErrorMessage="<%$ Resources:Resource, confirmpassword %>" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator><asp:CompareValidator
                                                                ID="PasswordCompare" runat="server" ControlToCompare="txtPassword" Type="String"
                                                                Operator="Equal" ControlToValidate="txtConfirmPassword" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match."></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <tr>
                                    <td align="left" style="padding-left: 0px;">
                                        <asp:UpdatePanel ID="upQuestions" UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <asp:PlaceHolder ID="plProfileQuestions" runat="server"></asp:PlaceHolder>
                                                <asp:PlaceHolder ID="plDemoQuestions" runat="server"></asp:PlaceHolder>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Panel ID="pnlPayPalQuestions" runat="server" Visible="false">
                                <div class="Category" style="padding: 5px; vertical-align: middle; float: left; width: 100%">
                                    <b>CREDIT CARD AUTHORIZATION:</b>
                                </div>
                                <br />
                                <br />
                                <table cellspacing="5" cellpadding="5" width="100%" border="0">
                                    <tr>
                                        <td width='25%' class='label' valign="middle" style="font-weight: bold">* Card Holder's First Name:
                                        </td>
                                        <td width='75%'>
                                            <asp:TextBox ID="PaypalFirstName" runat="server" MaxLength="50" Width="250"></asp:TextBox><asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator5" ControlToValidate="PaypalFirstName" InitialValue=""
                                                runat="server" ErrorMessage="Card Holder's First Name" Text="<%$ Resources:Resource, requiredfieldImage %>"> </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width='25%' class='label' valign="middle" style="font-weight: bold">* Card Holder's Last Name:
                                        </td>
                                        <td width='75%'>
                                            <asp:TextBox ID="PaypalLastName" runat="server" MaxLength="50" Width="250"></asp:TextBox><asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator6" ControlToValidate="PaypalLastName" InitialValue=""
                                                runat="server" ErrorMessage="Card Holder's Last Name" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width='25%' class='label' valign="middle" style="font-weight: bold">* Address:
                                        </td>
                                        <td width='75%'>
                                            <asp:TextBox ID="PaypalStreet" runat="server" MaxLength="50" Width="250"></asp:TextBox><asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator9" ControlToValidate="PaypalStreet" InitialValue=""
                                                runat="server" ErrorMessage="Address" Text="<%$ Resources:Resource, requiredfieldImage %>">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width='25%' class='label' valign="middle" style="font-weight: bold"></td>
                                        <td width='75%'>
                                            <asp:TextBox ID="PaypalStreet2" runat="server" MaxLength="50" Width="250"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width='25%' class='label' valign="middle" style="font-weight: bold">* City:
                                        </td>
                                        <td width='75%'>
                                            <asp:TextBox ID="PaypalCity" runat="server" MaxLength="50" Width="250"></asp:TextBox><asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator10" ControlToValidate="PaypalCity" InitialValue=""
                                                runat="server" ErrorMessage="City" Text="<%$ Resources:Resource, requiredfieldImage %>">&nbsp;&nbsp;                                         
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                        
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width='25%' class='label' valign="middle" style="font-weight: bold">* State/Province:
                                        </td>
                                        <td width='75%'>
                                            <asp:TextBox ID="PaypalState" runat="server" MaxLength="50" Width="250"></asp:TextBox><asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator11" ControlToValidate="PaypalState" InitialValue=""
                                                runat="server" ErrorMessage="State/Province" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width='25%' class='label' valign="middle" style="font-weight: bold">* Zip/Postal Code:
                                        </td>
                                        <td width='75%'>
                                            <asp:TextBox ID="PaypalZip" runat="server" MaxLength="50" Width="250"></asp:TextBox><asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator12" ControlToValidate="PaypalZip" InitialValue="" runat="server"
                                                ErrorMessage="Zip/Postal Code" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width='25%' class='label' valign="middle" style="font-weight: bold">* Country:
                                        </td>
                                        <td width='75%'>
                                            <asp:DropDownList ID="PaypalCountry" runat="server" CssClass="labelAnswer">
                                                <asp:ListItem Value="" Text="<%$ Resources:Resource, selectcountry %>"></asp:ListItem>
                                                <asp:ListItem Value="US" Text="United States"></asp:ListItem>
                                                <asp:ListItem Value="CA" Text="Canada"></asp:ListItem>
                                                <asp:ListItem Value="AF" Text="Afghanistan"></asp:ListItem>
                                                <asp:ListItem Value="AL" Text="Albania"></asp:ListItem>
                                                <asp:ListItem Value="DZ" Text="Algeria"></asp:ListItem>
                                                <asp:ListItem Value="AS" Text="American Samoa"></asp:ListItem>
                                                <asp:ListItem Value="AD" Text="Andorra"></asp:ListItem>
                                                <asp:ListItem Value="AO" Text="Angola"></asp:ListItem>
                                                <asp:ListItem Value="AI" Text="Anguilla"></asp:ListItem>
                                                <asp:ListItem Value="AQ" Text="Antarctica"></asp:ListItem>
                                                <asp:ListItem Value="AG" Text="Antigua and Barbuda"></asp:ListItem>
                                                <asp:ListItem Value="AR" Text="Argentina"></asp:ListItem>
                                                <asp:ListItem Value="AM" Text="Armenia"></asp:ListItem>
                                                <asp:ListItem Value="AW" Text="Aruba"></asp:ListItem>
                                                <asp:ListItem Value="AU" Text="Australia"></asp:ListItem>
                                                <asp:ListItem Value="AT" Text="Austria"></asp:ListItem>
                                                <asp:ListItem Value="AZ" Text="Azerbaidjan"></asp:ListItem>
                                                <asp:ListItem Value="BS" Text="Bahamas"></asp:ListItem>
                                                <asp:ListItem Value="BH" Text="Bahrain"></asp:ListItem>
                                                <asp:ListItem Value="BD" Text="Bangladesh"></asp:ListItem>
                                                <asp:ListItem Value="BB" Text="Barbados"></asp:ListItem>
                                                <asp:ListItem Value="BY" Text="Belarus"></asp:ListItem>
                                                <asp:ListItem Value="BE" Text="Belgium"></asp:ListItem>
                                                <asp:ListItem Value="BZ" Text="Belize"></asp:ListItem>
                                                <asp:ListItem Value="BJ" Text="Benin"></asp:ListItem>
                                                <asp:ListItem Value="BM" Text="Bermuda"></asp:ListItem>
                                                <asp:ListItem Value="BT" Text="Bhutan"></asp:ListItem>
                                                <asp:ListItem Value="BO" Text="Bolivia"></asp:ListItem>
                                                <asp:ListItem Value="BA" Text="Bosnia-Herzegovina"></asp:ListItem>
                                                <asp:ListItem Value="BW" Text="Botswana"></asp:ListItem>
                                                <asp:ListItem Value="BV" Text="Bouvet Island"></asp:ListItem>
                                                <asp:ListItem Value="BR" Text="Brazil"></asp:ListItem>
                                                <asp:ListItem Value="IO" Text="British Indian Ocean Territory"></asp:ListItem>
                                                <asp:ListItem Value="BN" Text="Brunei Darussalam"></asp:ListItem>
                                                <asp:ListItem Value="BG" Text="Bulgaria"></asp:ListItem>
                                                <asp:ListItem Value="BF" Text="Burkina Faso"></asp:ListItem>
                                                <asp:ListItem Value="BI" Text="Burundi"></asp:ListItem>
                                                <asp:ListItem Value="KH" Text="Cambodia"></asp:ListItem>
                                                <asp:ListItem Value="CM" Text="Cameroon"></asp:ListItem>
                                                <asp:ListItem Value="CA" Text="Canada"></asp:ListItem>
                                                <asp:ListItem Value="CV" Text="Cape Verde"></asp:ListItem>
                                                <asp:ListItem Value="KY" Text="Cayman Islands"></asp:ListItem>
                                                <asp:ListItem Value="CF" Text="Central African Republic"></asp:ListItem>
                                                <asp:ListItem Value="TD" Text="Chad"></asp:ListItem>
                                                <asp:ListItem Value="CL" Text="Chile"></asp:ListItem>
                                                <asp:ListItem Value="CN" Text="China"></asp:ListItem>
                                                <asp:ListItem Value="CX" Text="Christmas Island"></asp:ListItem>
                                                <asp:ListItem Value="CC" Text="Cocos (Keeling) Islands"></asp:ListItem>
                                                <asp:ListItem Value="CO" Text="Colombia"></asp:ListItem>
                                                <asp:ListItem Value="KM" Text="Comoros"></asp:ListItem>
                                                <asp:ListItem Value="CG" Text="Congo"></asp:ListItem>
                                                <asp:ListItem Value="CK" Text="Cook Islands"></asp:ListItem>
                                                <asp:ListItem Value="CR" Text="Costa Rica"></asp:ListItem>
                                                <asp:ListItem Value="HR" Text="Croatia"></asp:ListItem>
                                                <asp:ListItem Value="CU" Text="Cuba"></asp:ListItem>
                                                <asp:ListItem Value="CY" Text="Cyprus"></asp:ListItem>
                                                <asp:ListItem Value="CZ" Text="Czech Republic"></asp:ListItem>
                                                <asp:ListItem Value="DK" Text="Denmark"></asp:ListItem>
                                                <asp:ListItem Value="DJ" Text="Djibouti"></asp:ListItem>
                                                <asp:ListItem Value="DM" Text="Dominica"></asp:ListItem>
                                                <asp:ListItem Value="DO" Text="Dominican Republic"></asp:ListItem>
                                                <asp:ListItem Value="TP" Text="East Timor"></asp:ListItem>
                                                <asp:ListItem Value="EC" Text="Ecuador"></asp:ListItem>
                                                <asp:ListItem Value="EG" Text="Egypt"></asp:ListItem>
                                                <asp:ListItem Value="SV" Text="El Salvador"></asp:ListItem>
                                                <asp:ListItem Value="GQ" Text="Equatorial Guinea"></asp:ListItem>
                                                <asp:ListItem Value="ER" Text="Eritrea"></asp:ListItem>
                                                <asp:ListItem Value="EE" Text="Estonia"></asp:ListItem>
                                                <asp:ListItem Value="ET" Text="Ethiopia"></asp:ListItem>
                                                <asp:ListItem Value="FK" Text="Falkland Islands"></asp:ListItem>
                                                <asp:ListItem Value="FO" Text="Faroe Islands"></asp:ListItem>
                                                <asp:ListItem Value="FJ" Text="Fiji"></asp:ListItem>
                                                <asp:ListItem Value="FI" Text="Finland"></asp:ListItem>
                                                <asp:ListItem Value="CS" Text="Former Czechoslovakia"></asp:ListItem>
                                                <asp:ListItem Value="SU" Text="Former USSR"></asp:ListItem>
                                                <asp:ListItem Value="FR" Text="France"></asp:ListItem>
                                                <asp:ListItem Value="FX" Text="France (European Territory)"></asp:ListItem>
                                                <asp:ListItem Value="GF" Text="French Guyana"></asp:ListItem>
                                                <asp:ListItem Value="TF" Text="French Southern Territories"></asp:ListItem>
                                                <asp:ListItem Value="GA" Text="Gabon"></asp:ListItem>
                                                <asp:ListItem Value="GM" Text="Gambia"></asp:ListItem>
                                                <asp:ListItem Value="GE" Text="Georgia"></asp:ListItem>
                                                <asp:ListItem Value="DE" Text="Germany"></asp:ListItem>
                                                <asp:ListItem Value="GH" Text="Ghana"></asp:ListItem>
                                                <asp:ListItem Value="GI" Text="Gibraltar"></asp:ListItem>
                                                <asp:ListItem Value="GB" Text="Great Britain"></asp:ListItem>
                                                <asp:ListItem Value="GR" Text="Greece"></asp:ListItem>
                                                <asp:ListItem Value="GL" Text="Greenland"></asp:ListItem>
                                                <asp:ListItem Value="GD" Text="Grenada"></asp:ListItem>
                                                <asp:ListItem Value="GP" Text="Guadeloupe (French)"></asp:ListItem>
                                                <asp:ListItem Value="GU" Text="Guam (USA)"></asp:ListItem>
                                                <asp:ListItem Value="GT" Text="Guatemala"></asp:ListItem>
                                                <asp:ListItem Value="GN" Text="Guinea"></asp:ListItem>
                                                <asp:ListItem Value="GW" Text="Guinea Bissau"></asp:ListItem>
                                                <asp:ListItem Value="GY" Text="Guyana"></asp:ListItem>
                                                <asp:ListItem Value="HT" Text="Haiti"></asp:ListItem>
                                                <asp:ListItem Value="HM" Text="Heard and McDonald Islands"></asp:ListItem>
                                                <asp:ListItem Value="HN" Text="Honduras"></asp:ListItem>
                                                <asp:ListItem Value="HK" Text="Hong Kong"></asp:ListItem>
                                                <asp:ListItem Value="HU" Text="Hungary"></asp:ListItem>
                                                <asp:ListItem Value="IS" Text="Iceland"></asp:ListItem>
                                                <asp:ListItem Value="IN" Text="India"></asp:ListItem>
                                                <asp:ListItem Value="ID" Text="Indonesia"></asp:ListItem>
                                                <asp:ListItem Value="INT" Text="International"></asp:ListItem>
                                                <asp:ListItem Value="IR" Text="Iran"></asp:ListItem>
                                                <asp:ListItem Value="IQ" Text="Iraq"></asp:ListItem>
                                                <asp:ListItem Value="IE" Text="Ireland"></asp:ListItem>
                                                <asp:ListItem Value="IL" Text="Israel"></asp:ListItem>
                                                <asp:ListItem Value="IT" Text="Italy"></asp:ListItem>
                                                <asp:ListItem Value="CI" Text="Ivory Coast (Cote D&#39;Ivoire)"></asp:ListItem>
                                                <asp:ListItem Value="JM" Text="Jamaica"></asp:ListItem>
                                                <asp:ListItem Value="JP" Text="Japan"></asp:ListItem>
                                                <asp:ListItem Value="JO" Text="Jordan"></asp:ListItem>
                                                <asp:ListItem Value="KZ" Text="Kazakhstan"></asp:ListItem>
                                                <asp:ListItem Value="KE" Text="Kenya"></asp:ListItem>
                                                <asp:ListItem Value="KI" Text="Kiribati"></asp:ListItem>
                                                <asp:ListItem Value="KW" Text="Kuwait"></asp:ListItem>
                                                <asp:ListItem Value="KG" Text="Kyrgyzstan"></asp:ListItem>
                                                <asp:ListItem Value="LA" Text="Laos"></asp:ListItem>
                                                <asp:ListItem Value="LV" Text="Latvia"></asp:ListItem>
                                                <asp:ListItem Value="LB" Text="Lebanon"></asp:ListItem>
                                                <asp:ListItem Value="LS" Text="Lesotho"></asp:ListItem>
                                                <asp:ListItem Value="LR" Text="Liberia"></asp:ListItem>
                                                <asp:ListItem Value="LY" Text="Libya"></asp:ListItem>
                                                <asp:ListItem Value="LI" Text="Liechtenstein"></asp:ListItem>
                                                <asp:ListItem Value="LT" Text="Lithuania"></asp:ListItem>
                                                <asp:ListItem Value="LU" Text="Luxembourg"></asp:ListItem>
                                                <asp:ListItem Value="MO" Text="Macau"></asp:ListItem>
                                                <asp:ListItem Value="MK" Text="Macedonia"></asp:ListItem>
                                                <asp:ListItem Value="MG" Text="Madagascar"></asp:ListItem>
                                                <asp:ListItem Value="MW" Text="Malawi"></asp:ListItem>
                                                <asp:ListItem Value="MY" Text="Malaysia"></asp:ListItem>
                                                <asp:ListItem Value="MV" Text="Maldives"></asp:ListItem>
                                                <asp:ListItem Value="ML" Text="Mali"></asp:ListItem>
                                                <asp:ListItem Value="MT" Text="Malta"></asp:ListItem>
                                                <asp:ListItem Value="MH" Text="Marshall Islands"></asp:ListItem>
                                                <asp:ListItem Value="MQ" Text="Martinique (French)"></asp:ListItem>
                                                <asp:ListItem Value="MR" Text="Mauritania"></asp:ListItem>
                                                <asp:ListItem Value="MU" Text="Mauritius"></asp:ListItem>
                                                <asp:ListItem Value="YT" Text="Mayotte"></asp:ListItem>
                                                <asp:ListItem Value="MX" Text="Mexico"></asp:ListItem>
                                                <asp:ListItem Value="FM" Text="Micronesia"></asp:ListItem>
                                                <asp:ListItem Value="MD" Text="Moldavia"></asp:ListItem>
                                                <asp:ListItem Value="MC" Text="Monaco"></asp:ListItem>
                                                <asp:ListItem Value="MN" Text="Mongolia"></asp:ListItem>
                                                <asp:ListItem Value="MS" Text="Montserrat"></asp:ListItem>
                                                <asp:ListItem Value="MA" Text="Morocco"></asp:ListItem>
                                                <asp:ListItem Value="MZ" Text="Mozambique"></asp:ListItem>
                                                <asp:ListItem Value="MM" Text="Myanmar"></asp:ListItem>
                                                <asp:ListItem Value="NA" Text="Namibia"></asp:ListItem>
                                                <asp:ListItem Value="NR" Text="Nauru"></asp:ListItem>
                                                <asp:ListItem Value="NP" Text="Nepal"></asp:ListItem>
                                                <asp:ListItem Value="NL" Text="Netherlands"></asp:ListItem>
                                                <asp:ListItem Value="AN" Text="Netherlands Antilles"></asp:ListItem>
                                                <asp:ListItem Value="NT" Text="Neutral Zone"></asp:ListItem>
                                                <asp:ListItem Value="NC" Text="New Caledonia (French)"></asp:ListItem>
                                                <asp:ListItem Value="NZ" Text="New Zealand"></asp:ListItem>
                                                <asp:ListItem Value="NI" Text="Nicaragua"></asp:ListItem>
                                                <asp:ListItem Value="NE" Text="Niger"></asp:ListItem>
                                                <asp:ListItem Value="NG" Text="Nigeria"></asp:ListItem>
                                                <asp:ListItem Value="NU" Text="Niue"></asp:ListItem>
                                                <asp:ListItem Value="NF" Text="Norfolk Island"></asp:ListItem>
                                                <asp:ListItem Value="KP" Text="North Korea"></asp:ListItem>
                                                <asp:ListItem Value="MP" Text="Northern Mariana Islands"></asp:ListItem>
                                                <asp:ListItem Value="NO" Text="Norway"></asp:ListItem>
                                                <asp:ListItem Value="OM" Text="Oman"></asp:ListItem>
                                                <asp:ListItem Value="PK" Text="Pakistan"></asp:ListItem>
                                                <asp:ListItem Value="PW" Text="Palau"></asp:ListItem>
                                                <asp:ListItem Value="PA" Text="Panama"></asp:ListItem>
                                                <asp:ListItem Value="PG" Text="Papua New Guinea"></asp:ListItem>
                                                <asp:ListItem Value="PY" Text="Paraguay"></asp:ListItem>
                                                <asp:ListItem Value="PE" Text="Peru"></asp:ListItem>
                                                <asp:ListItem Value="PH" Text="Philippines"></asp:ListItem>
                                                <asp:ListItem Value="PN" Text="Pitcairn Island"></asp:ListItem>
                                                <asp:ListItem Value="PL" Text="Poland"></asp:ListItem>
                                                <asp:ListItem Value="PF" Text="Polynesia (French)"></asp:ListItem>
                                                <asp:ListItem Value="PT" Text="Portugal"></asp:ListItem>
                                                <asp:ListItem Value="PR" Text="Puerto Rico"></asp:ListItem>
                                                <asp:ListItem Value="QA" Text="Qatar"></asp:ListItem>
                                                <asp:ListItem Value="RE" Text="Reunion (French)"></asp:ListItem>
                                                <asp:ListItem Value="RO" Text="Romania"></asp:ListItem>
                                                <asp:ListItem Value="RU" Text="Russian Federation"></asp:ListItem>
                                                <asp:ListItem Value="RW" Text="Rwanda"></asp:ListItem>
                                                <asp:ListItem Value="GS" Text="S. Georgia & S. Sandwich Isls."></asp:ListItem>
                                                <asp:ListItem Value="SH" Text="Saint Helena"></asp:ListItem>
                                                <asp:ListItem Value="KN" Text="Saint Kitts & Nevis Anguilla"></asp:ListItem>
                                                <asp:ListItem Value="LC" Text="Saint Lucia"></asp:ListItem>
                                                <asp:ListItem Value="PM" Text="Saint Pierre and Miquelon"></asp:ListItem>
                                                <asp:ListItem Value="ST" Text="Saint Tome (Sao Tome) and Principe"></asp:ListItem>
                                                <asp:ListItem Value="VC" Text="Saint Vincent & Grenadines"></asp:ListItem>
                                                <asp:ListItem Value="WS" Text="Samoa"></asp:ListItem>
                                                <asp:ListItem Value="SM" Text="San Marino"></asp:ListItem>
                                                <asp:ListItem Value="SA" Text="Saudi Arabia"></asp:ListItem>
                                                <asp:ListItem Value="SN" Text="Senegal"></asp:ListItem>
                                                <asp:ListItem Value="SC" Text="Seychelles"></asp:ListItem>
                                                <asp:ListItem Value="SL" Text="Sierra Leone"></asp:ListItem>
                                                <asp:ListItem Value="SG" Text="Singapore"></asp:ListItem>
                                                <asp:ListItem Value="SK" Text="Slovak Republic"></asp:ListItem>
                                                <asp:ListItem Value="SI" Text="Slovenia"></asp:ListItem>
                                                <asp:ListItem Value="SB" Text="Solomon Islands"></asp:ListItem>
                                                <asp:ListItem Value="SO" Text="Somalia"></asp:ListItem>
                                                <asp:ListItem Value="ZA" Text="South Africa"></asp:ListItem>
                                                <asp:ListItem Value="KR" Text="South Korea"></asp:ListItem>
                                                <asp:ListItem Value="ES" Text="Spain"></asp:ListItem>
                                                <asp:ListItem Value="LK" Text="Sri Lanka"></asp:ListItem>
                                                <asp:ListItem Value="SD" Text="Sudan"></asp:ListItem>
                                                <asp:ListItem Value="SR" Text="Suriname"></asp:ListItem>
                                                <asp:ListItem Value="SJ" Text="Svalbard and Jan Mayen Islands"></asp:ListItem>
                                                <asp:ListItem Value="SZ" Text="Swaziland"></asp:ListItem>
                                                <asp:ListItem Value="SE" Text="Sweden"></asp:ListItem>
                                                <asp:ListItem Value="CH" Text="Switzerland"></asp:ListItem>
                                                <asp:ListItem Value="SY" Text="Syria"></asp:ListItem>
                                                <asp:ListItem Value="TJ" Text="Tadjikistan"></asp:ListItem>
                                                <asp:ListItem Value="TW" Text="Taiwan"></asp:ListItem>
                                                <asp:ListItem Value="TZ" Text="Tanzania"></asp:ListItem>
                                                <asp:ListItem Value="TH" Text="Thailand"></asp:ListItem>
                                                <asp:ListItem Value="TG" Text="Togo"></asp:ListItem>
                                                <asp:ListItem Value="TK" Text="Tokelau"></asp:ListItem>
                                                <asp:ListItem Value="TO" Text="Tonga"></asp:ListItem>
                                                <asp:ListItem Value="TT" Text="Trinidad and Tobago"></asp:ListItem>
                                                <asp:ListItem Value="TN" Text="Tunisia"></asp:ListItem>
                                                <asp:ListItem Value="TR" Text="Turkey"></asp:ListItem>
                                                <asp:ListItem Value="TM" Text="Turkmenistan"></asp:ListItem>
                                                <asp:ListItem Value="TC" Text="Turks and Caicos Islands"></asp:ListItem>
                                                <asp:ListItem Value="TV" Text="Tuvalu"></asp:ListItem>
                                                <asp:ListItem Value="UG" Text="Uganda"></asp:ListItem>
                                                <asp:ListItem Value="UA" Text="Ukraine"></asp:ListItem>
                                                <asp:ListItem Value="AE" Text="United Arab Emirates"></asp:ListItem>
                                                <asp:ListItem Value="GB" Text="United Kingdom"></asp:ListItem>
                                                <asp:ListItem Value="US" Text="United States"></asp:ListItem>
                                                <asp:ListItem Value="UY" Text="Uruguay"></asp:ListItem>
                                                <asp:ListItem Value="MIL" Text="USA Military"></asp:ListItem>
                                                <asp:ListItem Value="UM" Text="USA Minor Outlying Islands"></asp:ListItem>
                                                <asp:ListItem Value="UZ" Text="Uzbekistan"></asp:ListItem>
                                                <asp:ListItem Value="VU" Text="Vanuatu"></asp:ListItem>
                                                <asp:ListItem Value="VA" Text="Vatican City State"></asp:ListItem>
                                                <asp:ListItem Value="VE" Text="Venezuela"></asp:ListItem>
                                                <asp:ListItem Value="VN" Text="Vietnam"></asp:ListItem>
                                                <asp:ListItem Value="VG" Text="Virgin Islands (British)"></asp:ListItem>
                                                <asp:ListItem Value="VI" Text="Virgin Islands (USA)"></asp:ListItem>
                                                <asp:ListItem Value="WF" Text="Wallis and Futuna Islands"></asp:ListItem>
                                                <asp:ListItem Value="EH" Text="Western Sahara"></asp:ListItem>
                                                <asp:ListItem Value="YE" Text="Yemen"></asp:ListItem>
                                                <asp:ListItem Value="YU" Text="Yugoslavia"></asp:ListItem>
                                                <asp:ListItem Value="ZR" Text="Zaire"></asp:ListItem>
                                                <asp:ListItem Value="ZM" Text="Zambia"></asp:ListItem>
                                                <asp:ListItem Value="ZW" Text="Zimbabwe"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" ControlToValidate="PaypalCountry"
                                                InitialValue="" runat="server" ErrorMessage="Country" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width='25%' class='label' valign="middle" style="font-weight: bold">* Card Type:
                                        </td>
                                        <td width='75%'>
                                            <asp:DropDownList ID="PaypalCardType" runat="server" CssClass="labelAnswer">
                                                <asp:ListItem Text="--- Select Card Type ---" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Visa" Value="Visa"></asp:ListItem>
                                                <asp:ListItem Text="MasterCard" Value="MasterCard"></asp:ListItem>
                                                <asp:ListItem Text="American Express" Value="Amex"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ControlToValidate="PaypalCardType"
                                                InitialValue="" runat="server" ErrorMessage="Card Type" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width='25%' class='label' valign="middle" style="font-weight: bold">* Card Number:
                                        </td>
                                        <td width='75%'>
                                            <asp:TextBox ID="PaypalAcct" runat="server" MaxLength="50" Width="250"></asp:TextBox><asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator15" ControlToValidate="PaypalAcct" InitialValue=""
                                                runat="server" ErrorMessage="Card Number" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width='25%' class='label' valign="middle" style="font-weight: bold">* Expiration Date:
                                        </td>
                                        <td width='75%'>
                                            <asp:DropDownList ID="PaypalExpMonth" runat="server" CssClass="labelAnswer">
                                                <asp:ListItem Text="" Value="" />
                                                <asp:ListItem Text="1" Value="01" />
                                                <asp:ListItem Text="2" Value="02" />
                                                <asp:ListItem Text="3" Value="03" />
                                                <asp:ListItem Text="4" Value="04" />
                                                <asp:ListItem Text="5" Value="05" />
                                                <asp:ListItem Text="6" Value="06" />
                                                <asp:ListItem Text="7" Value="07" />
                                                <asp:ListItem Text="8" Value="08" />
                                                <asp:ListItem Text="9" Value="09" />
                                                <asp:ListItem Text="10" Value="10" />
                                                <asp:ListItem Text="11" Value="11" />
                                                <asp:ListItem Text="12" Value="12" />
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="PaypalExpYear" runat="server" CssClass="labelAnswer">
                                                <asp:ListItem Text="" Value="" />
                                                <asp:ListItem Text="2009" Value="2009" />
                                                <asp:ListItem Text="2010" Value="2010" />
                                                <asp:ListItem Text="2011" Value="2011" />
                                                <asp:ListItem Text="2012" Value="2012" />
                                                <asp:ListItem Text="2013" Value="2013" />
                                                <asp:ListItem Text="2014" Value="2014" />
                                                <asp:ListItem Text="2015" Value="2015" />
                                                <asp:ListItem Text="2016" Value="2016" />
                                                <asp:ListItem Text="2017" Value="2017" />
                                                <asp:ListItem Text="2018" Value="2018" />
                                                <asp:ListItem Text="2019" Value="2019" />
                                                <asp:ListItem Text="2020" Value="2020" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" ControlToValidate="PaypalExpMonth"
                                                InitialValue="" runat="server" ErrorMessage="Expiration Month" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator><asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator16" ControlToValidate="PaypalExpYear" InitialValue=""
                                                    runat="server" ErrorMessage="Expiration Year" Text="<%$ Resources:Resource, requiredfieldImage %>"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width='25%' class='label' valign="middle" style="font-weight: bold; font-color: red">Order Total:
                                        </td>
                                        <td width='75%'>
                                            <asp:Label ID="PaypalPrice" runat="server" Text="" />
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                        <asp:Panel ID="pnlNewslettersB" runat="server" Visible="false">
                            <p>
                                <asp:Repeater ID="rptCategoryB" runat="Server" OnItemDataBound="rptCategory_ItemDataBound">
                                    <ItemTemplate>
                                        <br />
                                        <table cellpadding="2" cellspacing="0" border="0" width="100%" align="center">
                                            <tr class="Category" width="100%">
                                                <td align="left" width="100%" style="padding: 6px 5px 6px 5px; font-weight: bold">
                                                    <a href="javascript:void(0);">
                                                        <img id="imgCollapseB" runat="server" alt="" onclick="SetPanels(this,'divNewsletterHeaderB')"
                                                            src="http://eforms.kmpsgroup.com/jointforms/images/expand_blue.png" title="Click to expand"
                                                            border="0" /></a>
                                                    <input type="checkbox" id="chkNewsletterCat" runat="server" onclick="CheckNewsletters(this)" />
                                                    <asp:Label ID="lblcategoryName" CssClass="Category" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryName") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-top: 5px; padding-bottom: 5px;">
                                                    <div id="divNewsletterHeaderB" style="display: block">
                                                        <span class="label">
                                                            <asp:Label ID="lblNewsletterHeaderB" Text="" runat="server"></asp:Label></span>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr bgcolor="#ffffff">
                                                <td align="left" width="100%" colspan="2">
                                                    <div id="divNewsletterDetailsB" runat="server" style="display: block">
                                                        <asp:GridView ID="gvNewsletters" runat="server" AllowPaging="false" AllowSorting="false"
                                                            AutoGenerateColumns="False" CellPadding="10" CellSpacing="5" DataKeyNames="ECNGroupID"
                                                            ForeColor="Black" GridLines="None" ShowFooter="false" ShowHeader="false" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"
                                                                    ItemStyle-VerticalAlign="Top" ItemStyle-Width="3%">
                                                                    <ItemTemplate>
                                                                        <input type="checkbox" id="chkselect" runat="server" onclick="CheckUncheckCategory(this)"
                                                                            checked='<%# Eval("subscribed").ToString().ToUpper()=="Y"?true:false %>' />
                                                                        <asp:Label ID="lblCustomerID" runat="server" Text='<%# Eval("CustomerID").ToString()%>'
                                                                            Visible="false" />
                                                                        <asp:Label ID="lblNewsLetterID" runat="server" Text='<%# Eval("NewsLetterID").ToString()%>'
                                                                            Visible="false" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"
                                                                    ItemStyle-Width="98%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDisplayName" runat="server" Text='<%# Eval("Displayname").ToString()%>'
                                                                            CssClass="label" Font-Bold="true" Font-Italic="true" Visible='<%# Convert.ToBoolean(Eval("ShowDisplayName").ToString()) %>' />
                                                                        <%--<asp:Label ID="lblSpace" runat="server" Text='&nbsp;&nbsp;' CssClass="label" Font-Bold="true"
                                                                            Font-Italic="true" Visible='<%# Convert.ToBoolean(Eval("ShowDisplayName").ToString()) %>' />--%>
                                                                        <font class="labelAnswer">
                                                                            <asp:Label ID="lblDescription" CssClass="labelAnswer" runat="server" Text='<%# Eval("Description") %>' /></font>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <br>
                            </p>
                        </asp:Panel>
                        <asp:Panel ID="pnlEvents" runat="server" Visible="false">
                            <div class="Category" style="padding: 5px; vertical-align: middle; float: left; width: 100%">
                                <b>View Current & Upcoming Events</b>
                            </div>
                            <br />
                            <br />
                            <asp:DataList ID="dlEvents" runat="server">
                                <ItemTemplate>
                                    <table cellpadding="10" cellspacing="5">
                                        <tr>
                                            <td width="20" align="center">
                                                <font style="color: #A6A6A6; font-size: 16px;">&#8226;</font>
                                            </td>
                                            <td valign="top">
                                                <a href='<%# DataBinder.Eval(Container.DataItem, "EventURL") %>' target="_blank"
                                                    style="color: black"><b>
                                                        <%# DataBinder.Eval(Container.DataItem, "DisplayName") %></b></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="20" />
                                            <td valign="top">
                                                <font style="font-size: 90%; font-weight: bold">
                                                    <%# DataBinder.Eval(Container.DataItem, "Location").ToString().Trim() %>;
                                                <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "StartDate")).ToString("MMM dd, yyyy")%></font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="20" />
                                            <td valign="top">
                                                <font style="font-size: 90%;">
                                                    <%# DataBinder.Eval(Container.DataItem, "EventDesc") %></font>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    <br />
                                </SeparatorTemplate>
                            </asp:DataList>
                            <br />
                        </asp:Panel>
                        <asp:Panel ID="pnlCustomPages" runat="server" Visible="false">
                            <div class="Category" style="padding: 5px; vertical-align: middle; float: left; width: 100%">
                                <b>Other Resources</b>
                            </div>
                            <br />
                            <br />
                            <asp:DataList ID="dlCustomPages" runat="server">
                                <ItemTemplate>
                                    <table cellpadding="10" cellspacing="5">
                                        <tr>
                                            <td width="20" align="center">
                                                <font style="color: #A6A6A6; font-size: 16px;">&#8226;</font>
                                            </td>
                                            <td valign="top">
                                                <a href='viewpage.aspx?PCPID=<%# DataBinder.Eval(Container.DataItem, "PCPID") %>'
                                                    target="_blank" style="color: black"><b>
                                                        <%# DataBinder.Eval(Container.DataItem, "PageName") %></b></a>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    <br />
                                </SeparatorTemplate>
                            </asp:DataList>
                        </asp:Panel>
                        <br />
                        <div style="width:100%">
                            <div class="g-recaptcha" data-sitekey="6LdH3AoUAAAAAL-QRSj-PeLIYrbckR_wTd48ub6l"></div>
                        </div>
                        <br />
                        <asp:Button ID="btnSubmit" runat="server" UseSubmitBehavior="false" name="btnSubmit_Click" OnClick="btnSave_Click" Text="<%$ Resources:Resource, submit %>" />&nbsp;
                        <asp:Button
                            CausesValidation="false" ID="btnReset" runat="server" OnClick="btnReset_Click"
                            Text="<%$ Resources:Resource, reset %>" OnClientClick="return confirm('Are you sure you want to clear the values?');" />
                    </asp:Panel>
                    <br />
                    <asp:PlaceHolder ID="phFooter" runat="server"></asp:PlaceHolder>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
