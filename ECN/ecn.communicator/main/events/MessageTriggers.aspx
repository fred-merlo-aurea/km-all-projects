<%@ Page Language="c#" Inherits="ecn.communicator.main.events.MessageTriggers" CodeBehind="MessageTriggers.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Src="~/main/ECNWizard/Content/layoutExplorer.ascx" TagName="layoutExplorer" TagPrefix="ecn" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<%@ Register Src="~/main/ECNWizard/OtherControls/EmailSubject.ascx" TagName="EmailSubject"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            bindControls();  //initialBind
            if (args.get_isPartialLoad()) {  //ajaxBind
                bindControls();
            }
        }
        function bindControls() {
            $("[id*='txtCampaingItemNameTA']").bind('keydown', function (event) {
                validateKeypress(event);
            });
            $("[id*='txtCampaingItemNameNO']").bind('keydown', function (event) {
                validateKeypress(event);
            });
        }
        function validateKeypress(event) {
            if (event.which == 8) { // Backspace
                return true;
            }
            if (event.which == 46) { // Delete
                return true;
            }
            var regex = new RegExp("^[a-zA-Z0-9]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        }
    </script>

    <script language="javascript">
        function replyTo_focus() {
            document.MessageTriggerForm.ReplyTo.value = document.MessageTriggerForm.EmailFrom.value;
        }

        function NOOP_replyTo_focus() {
            document.MessageTriggerForm.NOOP_ReplyTo.value = document.MessageTriggerForm.NOOP_EmailFrom.value;
        }

        function IsNumber(source, arguments) {
            var ValidChars = "0123456789";
            var Char;
            var sText = document.BlastForm.weekFreqcreateuency.value;
            for (i = 0; i < sText.length; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    arguments.IsValid = false;
                    alert('Only Numbers [0 - 9] are allowed in the Week Frequecny field');
                }
            }
            return;
        }
    </script>
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
            z-index: 10001 !important;
        }

        .modalPopupLayoutExplorer {
            background-color: white;
            border-width: 3px;
            border-style: solid;
            border-color: white;
            padding: 3px;
            overflow: auto;
        }

        .twemoji-picker{
            top:-200px !important;
        }
        .twemoji-error{top:5px !important;}
        .twemoji-icon-picker img {top: 0px !important; }
        @media screen and (-moz-images-in-menus:0) {
            .twemoji-picker-category .close {
                position: relative;
                top: 0px !important;
            }
    }
    .inner-container {
        width: 264px !important;
    }
    </style>
    <script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
    <script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
    <link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />

    <script type="text/javascript">
        if (window.attachEvent) { window.attachEvent('onload', pageloaded); }
        else if (window.addEventListener) { window.addEventListener('load', pageloaded, false); }
        else { document.addEventListener('load', pageloaded, false); }

        function pageloaded() {
            var setScrollBackFieldID = '<%=_setScrollBackField.ClientID%>';
            var setScrollBack = document.getElementById(setScrollBackFieldID).value;
            if (setScrollBack != null) {
                if (setScrollBack.toLowerCase() == "true") {
                    var _scrollHeight = document.body.scrollHeight;
                    setTimeout(function () { window.scrollTo(0, _scrollHeight); }, 200);
                    document.getElementById(setScrollBackFieldID).value = "";
                }
            }

            var initialStringA = $('#<%= _emailSubject.ClientID %>').val();
            initialStringA = initialStringA.replace(/'/g, "\\'");
            initialStringA = initialStringA.replace(/\r?\n|\r/g, ' ');
            initialStringA = twemoji.parse(eval("'" + initialStringA + "'"));
            $('#<%= _emailSubject.ClientID %>').twemojiPicker({ init: initialStringA, height: '30px', size: "16px" });

            var initialStringB = $('#<%= NOOP_EmailSubject.ClientID %>').val();
            initialStringB = initialStringB.replace(/'/g, "\\'");
            initialStringB = initialStringB.replace(/\r?\n|\r/g, ' ');
            initialStringB = twemoji.parse(eval("'" + initialStringB + "'"));
            

            var rblNoop = $("[id*=<%= NOOP_RadioList.ClientID %>] input:checked");
            var selectedNOOP = rblNoop.val();
            if (selectedNOOP == "Y")
            {
                $('#<%= NOOP_EmailSubject.ClientID %>').twemojiPicker({ init: initialStringB, height: '30px', size: "16px", disabled: false });
            }
            else
            {
                $('#<%= NOOP_EmailSubject.ClientID %>').twemojiPicker({ init: initialStringB, height: '30px', size: "16px", disabled: true });
            }
        }



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="_setScrollBackField" />
            <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop"></td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table height="67" width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                    </td>
                                    <td valign="middle" align="left" width="80%" height="100%">
                                        <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="errorBottom"></td>
                    </tr>
                </table>
            </asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>


    <br />
    <asp:UpdatePanel ID="upMain" UpdateMode="Conditional" runat="server">
        <ContentTemplate>


            <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
                <tr>
                    <td class="tableHeader" align="left">
                        <asp:DataList ID="lstLayoutSummary" CellSpacing="0" CellPadding="0" runat="Server"
                            Width="800px">
                            <HeaderStyle CssClass="tableHeader1"></HeaderStyle>
                            <ItemStyle CssClass="tableContentSmall" BorderStyle="Solid" BorderWidth="1"></ItemStyle>
                            <AlternatingItemStyle CssClass="tableContentAlt1"></AlternatingItemStyle>
                            <HeaderTemplate>
                                <table class="tableHeader" cellpadding="0">
                                    <tr>
                                        <td width="300" style="color: #ffffff;">Message</td>
                                        <td width="400" style="color: #ffffff;" align="center">Triggers</td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="300">
                                            <asp:LinkButton ID="btnMessageName" runat="Server" CausesValidation="False" CommandName="DrillDown"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ID") %>'>
												<%# DataBinder.Eval(Container.DataItem,"Name") %>
                                            </asp:LinkButton>
                                        </td>
                                        <td width="400" align="center">
                                            <asp:LinkButton ID="btnCount" runat="Server" CausesValidation="False" CommandName="DrillDown"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ID") %>'>
												<%# DataBinder.Eval(Container.DataItem,"TriggerCount") %>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <SelectedItemTemplate>
                                <table class="tableHeader" cellpadding="0" width="100%">
                                    <tr>
                                        <td width="300" colspan="2">&nbsp;
                                        <asp:Label ID="lblName" runat="Server">
												<%# DataBinder.Eval(Container.DataItem,"Name") %>
                                        </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                        <td width="100%">
                                            <asp:DataGrid ID="LayoutActions" runat="Server" AutoGenerateColumns="False" DataSource='<%# GetLayoutPlansForCampaign(Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ID"))) %>'
                                                Width="100%" OnItemCommand="LayoutActions_ItemCommand" CssClass="grid" BackColor="#ffffff">
                                                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                                                <ItemStyle Height="22px"></ItemStyle>
                                                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundColumn HeaderStyle-HorizontalAlign="Center" DataField="ActionName" HeaderText="Trigger Name"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderStyle-HorizontalAlign="Center" DataField="EventType" HeaderText="Trigger Event"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        DataField="Status" HeaderText="Status"></asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="90" HeaderText="Actions"
                                                        ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnEdit" Text="<img src=/ecn.images/images/icon-edits1.gif alt='Edit Trigger' border='0'>"
                                                                CssClass="tableContentSmall" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LayoutPlanID") %>'
                                                                CausesValidation="False" runat="Server" />&nbsp;&nbsp;
                                                        <asp:LinkButton ID="btnDelete" Text="<img src='/ecn.images/images/icon-delete1.gif' alt='Delete Trigger' border='0'>"
                                                            CssClass="tableContentSmall" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LayoutPlanID") %>'
                                                            CausesValidation="False" runat="Server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:HyperLinkColumn Text="<img src='/ecn.images/images/icon-reports.gif' alt='View Trigger Reporting' border='0'>"
                                                        DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="../blasts/reports.aspx?BlastID={0}"
                                                        ItemStyle-HorizontalAlign="Center"></asp:HyperLinkColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                </table>
                            </SelectedItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border='0' width="900">
                            <tr>
                                <td colspan="4"></td>
                            </tr>
                            <tr>
                                <td class="tableHeader" colspan="4" align="left">1. General Information:</td>
                            </tr>
                            <tr>
                                <td class="label" align='right'>Trigger Name&nbsp;</td>
                                <td class="tableHeader" colspan='3' align="left">
                                    <asp:TextBox ID="_layoutName" runat="Server" Width="264px" CssClass="formfield" TabIndex="1"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="Requiredfieldvalidator2" runat="Server" ControlToValidate="_layoutName" ValidationGroup="Save" ErrorMessage="Trigger Name is a required field."
                                        Display="Static" CssClass="errormsg">&nbsp;*</asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td class="label" align='right' height="25">Target Message&nbsp;</td>
                                <td class="tableHeader" height="25" colspan='3' align="left">
                                    <asp:ImageButton ID="imgSelectLayoutA" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" CausesValidation="false" OnClick="imgSelectLayoutTrigger_Click" Visible="true" />
                                    <asp:Label ID="lblSelectedLayoutTrigger" runat="server" />
                                    <asp:HiddenField ID="hfSelectedLayoutTrigger" runat="server" Value="" />
                                    <asp:HiddenField ID="hfWhichLayout" runat="server" Value="" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" height="7"></td>
                            </tr>
                            <tr>
                                <td class="tableHeader" colspan="4" align="left">2. Define Message Event:</td>
                            </tr>
                            <tr>
                                <td class="label" align='right'>Trigger Type&nbsp;</td>
                                <td class="tableHeader" colspan='3' align="left">
                                    <!--<asp:ListItem value="subscribe">Subscribe</asp:ListItem>-->
                                    <!--<asp:ListItem value="refer">Forwards</asp:ListItem>-->
                                    <asp:DropDownList ID="EventType" runat="Server" Width="264px" AutoPostBack="true"
                                        CssClass="formfield" TabIndex="3" OnSelectedIndexChanged="EventType_SelectedIndexChanged">
                                        <asp:ListItem Value="click" Selected="True">Clicks</asp:ListItem>
                                        <asp:ListItem Value="open">Opens</asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="label" align='right'>Trigger Condition&nbsp;</td>
                                <td class="tableHeader" colspan='3' align="left">
                                    <asp:DropDownList ID="_criteria" runat="Server" Width="264px" CssClass="formfield"
                                        TabIndex="4">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td colspan="4" height="7"></td>
                            </tr>
                            <tr>
                                <td class="tableHeader" colspan="2" valign="middle" align="left">3a. Define Trigger Action:</td>
                                <td class="tableHeader" valign="middle" align="left">3b. Define No-Open Action: <span class='label'>[optional]</span>
                                </td>
                                <td class="label" valign="bottom">
                                    <asp:RadioButtonList ID="NOOP_RadioList" runat="Server" CssClass="label" RepeatDirection="Horizontal"
                                        AutoPostBack="True" TabIndex="13" OnSelectedIndexChanged="NOOP_RadioList_SelectedIndexChanged">
                                        <asp:ListItem Value="Y">YES</asp:ListItem>
                                        <asp:ListItem Value="N" Selected="True">NO</asp:ListItem>
                                    </asp:RadioButtonList></td>
                            </tr>

                            <tr>
                                <td class="label" valign="top" align='right'>Campaign Item Name&nbsp;</td>
                                <td align="left">
                                    <asp:TextBox ID="txtCampaingItemNameTA" runat="Server" Width="264px" CssClass="formfield" TabIndex="6"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtCampaingItemNameTA" ValidationGroup="Save" ErrorMessage="Campaign Item Name is a required field."
                                        Display="Static" CssClass="errormsg">&nbsp;*</asp:RequiredFieldValidator></td>
                                <td colspan="2" align="left">
                                    <asp:TextBox ID="txtCampaingItemNameNO" runat="Server" Width="264px" CssClass="formfield" Enabled="False"
                                        TabIndex="15"></asp:TextBox><asp:RequiredFieldValidator ID="rvfCampaingItemNameNO" runat="Server"
                                            ControlToValidate="txtCampaingItemNameNO" ValidationGroup="Save" ErrorMessage="Campaign Item Name is a required field."
                                            Display="Static" CssClass="errormsg" Enabled="False">&nbsp;*</asp:RequiredFieldValidator></td>
                            </tr>

                            <tr>
                                <td class="label" valign="top" align='right'>Follow Up Message&nbsp;</td>
                                <td align="left" class="tableHeader">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" CausesValidation="false" OnClick="imgSelectLayoutReply_Click" Visible="true" />
                                    <asp:Label ID="lblSelectedLayoutReply" runat="server" />
                                    <asp:HiddenField ID="hfSelectedLayoutReply" runat="server" Value="" />
                                </td>
                                <td colspan="2" align="left" class="tableHeader">
                                    <asp:ImageButton ID="imgbtnNOOP" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" CausesValidation="false" OnClick="imgSelectLayoutNOOPReply_Click" Visible="true" />
                                    <asp:Label ID="lblSelectedLayoutNOOPReply" runat="server" />
                                    <asp:HiddenField ID="hfSelectedLayoutNOOPReply" runat="server" Value="" />
                                </td>
                            </tr>
                            <tr>
                                <td class="label" valign="top" align='right'>From Email&nbsp;</td>
                                <td align="left">
                                    <asp:TextBox ID="_emailFrom" runat="Server" Width="264px" CssClass="formfield" TabIndex="6"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="val_EmailFrom" runat="Server" ControlToValidate="_emailFrom" ValidationGroup="Save" ErrorMessage="From Email address is a required field."
                                        Display="Static" CssClass="errormsg">&nbsp;*</asp:RequiredFieldValidator></td>
                                <td colspan="2" align="left">
                                    <asp:TextBox ID="NOOP_EmailFrom" runat="Server" Width="264px" CssClass="formfield"
                                        TabIndex="15"></asp:TextBox><asp:RequiredFieldValidator ID="val_NOOP_EmailFrom" runat="Server"
                                            ControlToValidate="NOOP_EmailFrom" ValidationGroup="Save" ErrorMessage="From Email address is a required field."
                                            Display="Static" CssClass="errormsg" Enabled="False">&nbsp;*</asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td class="label" valign="top" align='right'>Reply To&nbsp;</td>
                                <td align="left">
                                    <asp:TextBox ID="_replyTo" runat="Server" Width="264px"
                                        CssClass="formfield" TabIndex="7"></asp:TextBox><%--onfocus="replyTo_focus()"--%><asp:RequiredFieldValidator ID="val_ReplyTo"
                                            runat="Server" ControlToValidate="_replyTo" ValidationGroup="Save" ErrorMessage="Reply Email address is a required field."
                                            Display="Static" CssClass="errormsg">&nbsp;*</asp:RequiredFieldValidator></td>
                                <td colspan="2" align="left">
                                    <asp:TextBox ID="NOOP_ReplyTo" runat="Server" Width="264px"
                                        CssClass="formfield" TabIndex="15"></asp:TextBox><%--onfocus="NOOP_replyTo_focus()"--%><asp:RequiredFieldValidator ID="val_NOOP_ReplyTo"
                                            runat="Server" ControlToValidate="NOOP_ReplyTo" ErrorMessage="Reply Email address is a required field."
                                            Display="Static" CssClass="errormsg" ValidationGroup="Save" Enabled="False">&nbsp;*</asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td class="label" valign="top" align='right'>From Name&nbsp;</td>
                                <td align="left">
                                    <asp:TextBox ID="_emailFromName" runat="Server" Width="264px" CssClass="formfield"
                                        TabIndex="8"></asp:TextBox><asp:RequiredFieldValidator ID="val_EmailFromName" runat="Server"
                                            ControlToValidate="_emailFromName" ValidationGroup="Save" ErrorMessage="From Name is a required field."
                                            Display="Static" CssClass="errormsg">&nbsp;*</asp:RequiredFieldValidator></td>
                                <td colspan="2" align="left">
                                    <asp:TextBox ID="NOOP_EmailFromName" runat="Server" Width="264px" CssClass="formfield"
                                        TabIndex="16"></asp:TextBox><asp:RequiredFieldValidator ID="val_NOOP_EmailFromName"
                                            runat="Server" ControlToValidate="NOOP_EmailFromName" ValidationGroup="Save" ErrorMessage="From Name is a required field."
                                            Display="Static" CssClass="errormsg" Enabled="False">&nbsp;*</asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td class="label" valign="middle" align='right' height="60">Subject&nbsp;</td>
                                <td align="left" height="60">
                                    <asp:HiddenField ID="_emailSubject" runat="server" Value="" />

                                    <%--<asp:TextBox ID="EmailSubject" runat="Server" Width="264px" CssClass="formfield"
                                        TabIndex="9"></asp:TextBox><asp:RequiredFieldValidator ID="val_EmailSubject" runat="Server"
                                            ControlToValidate="EmailSubject" ValidationGroup="Save" ErrorMessage="Email Subject is a required field."
                                            Display="Static" CssClass="errormsg">&nbsp;*</asp:RequiredFieldValidator>--%>

                                </td>
                                <td height="60" colspan="2" align="left" style="padding-right:40px;">
                                    <asp:HiddenField ID="NOOP_EmailSubject" runat="server" Value="" />


                                    <%--<asp:TextBox ID="NOOP_EmailSubject" runat="Server" Width="264px" CssClass="formfield"
                                        TabIndex="17"></asp:TextBox><asp:RequiredFieldValidator ID="val_NOOP_EmailSubject"
                                            runat="Server" ControlToValidate="NOOP_EmailSubject" ErrorMessage="Email Subject is a required field."
                                            Display="Static" ValidationGroup="Save" CssClass="errormsg" Enabled="False">&nbsp;*</asp:RequiredFieldValidator>--%>

                                </td>
                            </tr>
                            <tr>
                                <td class="label" align='right' valign="top">Send Follow Up&nbsp;
                                </td>
                                <td class="label" valign="top" align="left">Message after&nbsp;
                                <div style="padding-top: 4px">
                                    <asp:TextBox ID="_period" runat="Server" Width="24px" CssClass="formfield" TabIndex="10">0</asp:TextBox>&nbsp;days&nbsp;&nbsp;
                                    <asp:TextBox ID="_txtHours" runat="Server" Width="24px" CssClass="formfield" TabIndex="11">0</asp:TextBox>&nbsp;hours&nbsp;&nbsp;
                                    <asp:TextBox ID="_txtMinutes" runat="Server" Width="24px" CssClass="formfield" TabIndex="12">0</asp:TextBox>&nbsp;mins&nbsp;&nbsp;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="Server" ValidationGroup="Save" ErrorMessage="Days is a required Filed"
                                        ControlToValidate="_period">&nbsp;*</asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="RangeValidator1" runat="Server" ValidationGroup="Save" ErrorMessage="Days need to between 0~15 days."
                                        ControlToValidate="_period" MaximumValue="15" MinimumValue="0" Type="Integer">&nbsp;*</asp:RangeValidator>
                                    <asp:RequiredFieldValidator ID="valForHours" runat="Server" ValidationGroup="Save" ErrorMessage="Hours is a requried field."
                                        ControlToValidate="_txtHours">&nbsp;*</asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="rangeValForHour" runat="Server" ValidationGroup="Save" ErrorMessage="Hours need to be between 0~24"
                                        ControlToValidate="_txtHours" MaximumValue="24" MinimumValue="0" Type="Integer">&nbsp;*</asp:RangeValidator>
                                    <asp:RequiredFieldValidator ID="validationForMin" runat="Server" ValidationGroup="Save" ErrorMessage="Minutes is a requried field."
                                        ControlToValidate="_txtMinutes">&nbsp;*</asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="rangeValForMin" runat="Server" ValidationGroup="Save" ErrorMessage="Minutes need to be between 0~60"
                                        ControlToValidate="_txtMinutes" MaximumValue="60" MinimumValue="0" Type="Integer">&nbsp;*</asp:RangeValidator>
                                </div>
                                </td>
                                <td class="label" valign="middle" colspan="2" align="left">No-Open action will trigger&nbsp;
                                <div style="padding-top: 4px">
                                    <asp:TextBox ID="NOOP_Period" runat="Server" Width="24px" CssClass="formfield" TabIndex="18">0</asp:TextBox>&nbsp;days&nbsp;&nbsp;
                                    <asp:TextBox ID="NOOP_txtHours" runat="Server" Width="24px" CssClass="formfield"
                                        TabIndex="19">0</asp:TextBox>&nbsp;hours&nbsp;&nbsp;
                                    <asp:TextBox ID="NOOP_txtMinutes" runat="Server" Width="24px" CssClass="formfield"
                                        TabIndex="20">0</asp:TextBox>&nbsp;mins&nbsp;
                                </div>
                                    <div style="padding-top: 4px">
                                        after Follow Up Message is sent out in 3a.
                                    <asp:RequiredFieldValidator ID="val_NOOP_Period" runat="Server" ValidationGroup="Save" ErrorMessage="Days is a required Filed"
                                        ControlToValidate="NOOP_Period" Enabled="False">&nbsp;*</asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rangeval_NOOP_Period" runat="Server" ErrorMessage="Days need to between 0~15 days."
                                            ControlToValidate="NOOP_Period" MaximumValue="15" ValidationGroup="Save" MinimumValue="0" Type="Integer"
                                            Enabled="False">&nbsp;*</asp:RangeValidator>
                                        <asp:RequiredFieldValidator ID="val_NOOP_txtHours" runat="Server" ValidationGroup="Save" ErrorMessage="Hours is a requried field."
                                            ControlToValidate="NOOP_txtHours" Enabled="False">&nbsp;*</asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rangeval_NOOP_txtHours" runat="Server" ValidationGroup="Save" ErrorMessage="Hours need to be between 0~24"
                                            ControlToValidate="NOOP_txtHours" MaximumValue="24" MinimumValue="0" Type="Integer"
                                            Enabled="False">&nbsp;*</asp:RangeValidator>
                                        <asp:RequiredFieldValidator ID="val_NOOP_txtMinutes" ValidationGroup="Save" runat="Server" ErrorMessage="Minutes is a requried field."
                                            ControlToValidate="NOOP_txtMinutes" Enabled="False">&nbsp;*</asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rangeval_NOOP_txtMinutes" runat="Server" ErrorMessage="Minutes need to be between 0~60"
                                            ControlToValidate="NOOP_txtMinutes" MaximumValue="60" ValidationGroup="Save" MinimumValue="0" Type="Integer"
                                            Enabled="False">&nbsp;*</asp:RangeValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="Server"></asp:ValidationSummary>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:Button ID="_createButton" runat="Server" Text="Create Trigger" ValidationGroup="Save" CssClass="formbuttonsmall"
                                        TabIndex="21" OnClick="CreateButton_Click"></asp:Button></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="hfLayoutExplorer" style="display:none;" runat="server" />
    <ajaxToolkit:ModalPopupExtender ID="mpeLayoutExplorer" PopupControlID="pnlLayoutExplorer" BackgroundCssClass="modalBackground" TargetControlID="hfLayoutExplorer" CancelControlID="btnCloseLayoutExplorer" runat="server" />
    <asp:UpdatePanel ID="pnlLayoutExplorer" Style="display:none;" CssClass="modalPopupLayoutExplorer" UpdateMode="Always" ChildrenAsTriggers="true" runat="server">
        <ContentTemplate>
            <table style="background-color: white;">
                <tr>
                    <td>
                        <ecn:layoutExplorer ID="layoutExplorer" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnCloseLayoutExplorer" runat="server" OnClick="btnCloseLayoutExplorer_Click" Text="Close" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
