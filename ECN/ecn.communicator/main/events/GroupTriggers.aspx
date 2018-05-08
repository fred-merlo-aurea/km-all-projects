<%@ Page Language="c#" Inherits="ecn.communicator.main.events.GroupTriggers" CodeBehind="GroupTriggers.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript">
        function replyTo_focus() {
            document.GroupTriggerForm.ReplyTo.value = document.GroupTriggerForm.EmailFrom.value;
        }

        function NOOP_replyTo_focus() {
            document.GroupTriggerForm.NOOP_ReplyTo.value = document.GroupTriggerForm.NOOP_EmailFrom.value;
        }

        function IsNumber(source, arguments) {
            var ValidChars = "0123456789";
            var Char;
            var sText = document.BlastForm.weekFrequency.value;
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
    <script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
    <script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
    <link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />

    <script type="text/javascript">
        if (window.attachEvent) { window.attachEvent('onload', pageloaded); }
        else if (window.addEventListener) { window.addEventListener('load', pageloaded, false); }
        else { document.addEventListener('load', pageloaded, false); }

        function pageloaded() {

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
            if (selectedNOOP == "Y") {
                $('#<%= NOOP_EmailSubject.ClientID %>').twemojiPicker({ init: initialStringB, height: '30px', size: "16px", disabled: false });
            }
            else {
                $('#<%= NOOP_EmailSubject.ClientID %>').twemojiPicker({ init: initialStringB, height: '30px', size: "16px", disabled: true });
            }
        }



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:PlaceHolder ID="phError" runat="Server" Visible="false">
    <table cellspacing="0" cellpadding="0" width="674" align="center">
        <tr>
            <td id="errorTop">
            </td>
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
            <td id="errorBottom">
            </td>
        </tr>
    </table>
</asp:PlaceHolder>
<br />
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <td class="tableHeader" align="left">
                <asp:DataList ID="lstLayoutSummary" runat="Server" CellPadding="0" Width="800px">
                    <HeaderStyle CssClass="tableHeader1"></HeaderStyle>
                    <ItemStyle CssClass="tableContentSmall" BorderStyle="Solid" BorderWidth="1"></ItemStyle>
                    <AlternatingItemStyle CssClass="tableContentAlt1"></AlternatingItemStyle>
                    <HeaderTemplate>
                        <table class="tableHeader" cellpadding="0">
                            <tr>
                                <td width="300" style="color: #ffffff;">
                                    Group Name
                                </td>
                                <td width="400" style="color: #ffffff;" align="center">
                                    # of applied rules
                                </td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="300">
                                    <asp:LinkButton ID="btnCampaignName" runat="Server" CausesValidation="False" CommandName="DrillDown"
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
                                <td width="300" colspan="2">
                                    &nbsp;
                                    <asp:Label ID="lblName" runat="Server">
												<%# DataBinder.Eval(Container.DataItem,"Name") %>
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td width="100%">
                                    <asp:DataGrid ID="LayoutActions" runat="Server" DataSource='<%# GetLayoutPlansForGroup(Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ID"))) %>'
                                        OnItemCommand="LayoutActions_ItemCommand" AutoGenerateColumns="False" Width="100%"
                                        CssClass="grid">
                                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                        <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                                        <ItemStyle Height="22px"></ItemStyle>
                                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn HeaderStyle-HorizontalAlign="Center" DataField="EventType" HeaderText="EventType">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn HeaderStyle-HorizontalAlign="Center" DataField="ActionName" HeaderText="ActionName">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                DataField="Status" HeaderText="Status"></asp:BoundColumn>
                                            <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="90" HeaderText="Actions"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnCopy" Text="<img src=/ecn.images/images/icon-copy.gif alt='Copy Trigger' border='0'>"
                                                        CssClass="tableContentSmall" CommandName="Copy" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LayoutPlanID") %>'
                                                        CausesValidation="False" runat="Server" Visible="false" />&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnEdit" Text="<img src=/ecn.images/images/icon-edits1.gif alt='Edit Trigger' border='0'>"
                                                        CssClass="tableContentSmall" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LayoutPlanID") %>'
                                                        CausesValidation="False" runat="Server" />&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnActivate" Text="<img src=/ecn.images/images/icon-cancel.gif alt='Activate/DeActivate Trigger' border='0'>"
                                                        CssClass="tableContentSmall" CommandName="Activate" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LayoutPlanID") %>'
                                                        CausesValidation="False" runat="Server" Visible="false" />&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnDelete" Text="<img src='/ecn.images/images/icon-delete1.gif' alt='Delete Trigger' border='0'>"
                                                        CssClass="tableContentSmall" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LayoutPlanID") %>'
                                                        CausesValidation="False" runat="Server" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:HyperLinkColumn Text="<img src='/ecn.images/images/icon-reports.gif' alt='View Trigger Reporting' border='0'>"
                                                DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="../blasts/reports.aspx?BlastID={0}"
                                                ItemStyle-HorizontalAlign="Center"></asp:HyperLinkColumn>
                                            <%--<asp:BoundColumn HeaderStyle-HorizontalAlign="Center" DataField="NOOPRptLnk" HeaderText="">
                                            </asp:BoundColumn>--%>
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
                <table border='0' width="800">
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td class="tableHeader" colspan="4" align="left">
                            1. General Information:
                        </td>
                    </tr>
                    <tr>
                        <td class="label" align='right'>
                            Trigger Name&nbsp;
                        </td>
                        <td class="label" colspan='3'  align="left">
                            <asp:TextBox ID="_layoutName" runat="Server" Width="276px" CssClass="formfield" TabIndex="1"></asp:TextBox><asp:RequiredFieldValidator
                                ID="Requiredfieldvalidator2" runat="Server" CssClass="errormsg" Display="Static"
                                ErrorMessage="Email Subject is a required field." ControlToValidate="_layoutName">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="label" align='right' height="25">
                            Target Group&nbsp;
                        </td>
                        <td class="label" height="25" colspan='3'  align="left">
                            <asp:DropDownList ID="_triggerCampaign" runat="Server" Width="276px" DataTextField="LayoutName"
                                DataValueField="LayoutID" AutoPostBack="true" CssClass="formfield" TabIndex="2"
                                OnSelectedIndexChanged="TriggerCampaign_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" height="7">
                        </td>
                    </tr>
                    <tr>
                        <td class="label" align='right'>
                            Trigger Condition&nbsp;
                        </td>
                        <td class="label" colspan='3'  align="left">
                            <asp:DropDownList ID="_criteria" runat="Server" Width="276px" TabIndex="4">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td class="tableHeader" colspan="2" valign="middle"  align="left">
                            2a. Define Trigger Action:
                        </td>
                        <td class="tableHeader" valign="middle" width="240"  align="left">
                            2b. Define No-Open Action: <span class='label'>[optional]</span>
                        </td>
                        <td class="label" valign="bottom"  align="left">
                            <asp:RadioButtonList ID="NOOP_RadioList" runat="Server" CssClass="label" RepeatDirection="Horizontal"
                                AutoPostBack="True" TabIndex="13" OnSelectedIndexChanged="NOOP_RadioList_SelectedIndexChanged">
                                <asp:ListItem Value="Y">YES</asp:ListItem>
                                <asp:ListItem Value="N" Selected="True">NO</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label" valign="top" align='right'>
                            Follow Up Campaign&nbsp;
                        </td>
                        <td  align="left">
                            <asp:DropDownList ID="_replyCampaign" runat="Server" Width="276px" DataValueField="LayoutID"
                                DataTextField="LayoutName" CssClass="formfield" TabIndex="5">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2"  align="left">
                            <asp:DropDownList ID="NOOP_ReplyCampaign" runat="Server" Width="276px" DataValueField="LayoutID"
                                DataTextField="LayoutName" CssClass="formfield" TabIndex="14">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label" valign="top" align='right'>
                            From Email&nbsp;
                        </td>
                        <td  align="left">
                            <asp:TextBox ID="_emailFrom" runat="Server" Width="276px" CssClass="formfield" TabIndex="6"></asp:TextBox><asp:RequiredFieldValidator
                                ID="val_EmailFrom" runat="Server" ControlToValidate="_emailFrom" ErrorMessage="From Email address is a required field."
                                Display="Static" CssClass="errormsg">&nbsp;*</asp:RequiredFieldValidator>
                        </td>
                        <td colspan="2" align="left">
                            <asp:TextBox ID="NOOP_EmailFrom" runat="Server" Width="276px" CssClass="formfield"
                                TabIndex="15"></asp:TextBox><asp:RequiredFieldValidator ID="val_NOOP_EmailFrom" runat="Server"
                                    ControlToValidate="NOOP_EmailFrom" ErrorMessage="From Email address is a required field."
                                    Display="Static" CssClass="errormsg" Enabled="False">&nbsp;*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="label" valign="top" align='right'>
                            Reply To&nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="_replyTo" onfocus="replyTo_focus()" runat="Server" Width="276px"
                                CssClass="formfield" TabIndex="7"></asp:TextBox><asp:RequiredFieldValidator ID="val_ReplyTo"
                                    runat="Server" ControlToValidate="_replyTo" ErrorMessage="Reply Email address is a required field."
                                    Display="Static" CssClass="errormsg">&nbsp;*</asp:RequiredFieldValidator>
                        </td>
                        <td colspan="2" align="left">
                            <asp:TextBox ID="NOOP_ReplyTo" onfocus="NOOP_replyTo_focus()" runat="Server" Width="276px"
                                CssClass="formfield" TabIndex="15"></asp:TextBox><asp:RequiredFieldValidator ID="val_NOOP_ReplyTo"
                                    runat="Server" ControlToValidate="NOOP_ReplyTo" ErrorMessage="Reply Email address is a required field."
                                    Display="Static" CssClass="errormsg" Enabled="False">&nbsp;*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="label" valign="top" align='right'>
                            From Name&nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="_emailFromName" runat="Server" Width="276px" CssClass="formfield"
                                TabIndex="8"></asp:TextBox><asp:RequiredFieldValidator ID="val_EmailFromName" runat="Server"
                                    ControlToValidate="_emailFromName" ErrorMessage="From Name is a required field."
                                    Display="Static" CssClass="errormsg">&nbsp;*</asp:RequiredFieldValidator>
                        </td>
                        <td colspan="2" align="left">
                            <asp:TextBox ID="NOOP_EmailFromName" runat="Server" Width="276px" CssClass="formfield"
                                TabIndex="16"></asp:TextBox><asp:RequiredFieldValidator ID="val_NOOP_EmailFromName"
                                    runat="Server" ControlToValidate="NOOP_EmailFromName" ErrorMessage="From Name is a required field."
                                    Display="Static" CssClass="errormsg" Enabled="False">&nbsp;*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="label" valign="top" align='right' height="25">
                            Subject&nbsp;
                        </td>
                        <td align="left">
                            <asp:HiddenField ID="_emailSubject" runat="server"  Value="" />
                            <%--<asp:TextBox ID="EmailSubject" runat="Server" Width="276px" CssClass="formfield"
                                TabIndex="9"></asp:TextBox><asp:RequiredFieldValidator ID="val_EmailSubject" runat="Server"
                                    ControlToValidate="EmailSubject" ErrorMessage="Email Subject is a required field."
                                    Display="Static" CssClass="errormsg">&nbsp;*</asp:RequiredFieldValidator>--%>
                        </td>
                        <td height="25" colspan="2" align="left" style="padding-right:40px;">
                            <asp:HiddenField ID="NOOP_EmailSubject" runat="server"  Value="" />
                           <%-- <asp:TextBox ID="NOOP_EmailSubject" runat="Server" Width="276px" CssClass="formfield"
                                TabIndex="17"></asp:TextBox><asp:RequiredFieldValidator ID="val_NOOP_EmailSubject"
                                    runat="Server" ControlToValidate="NOOP_EmailSubject" ErrorMessage="Email Subject is a required field."
                                    Display="Static" CssClass="errormsg" Enabled="False">&nbsp;*</asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="label" align='right' valign="top">
                            Send Follow Up&nbsp;
                        </td>
                        <td class="label" valign="top"  align="left">
                            campaign after&nbsp;
                            <div style="padding-top: 4px">
                                <asp:TextBox ID="_period" runat="Server" Width="24px" CssClass="formfield" TabIndex="10">0</asp:TextBox>&nbsp;days&nbsp;&nbsp;
                                <asp:TextBox ID="_txtHours" runat="Server" Width="24px" CssClass="formfield" TabIndex="11">0</asp:TextBox>&nbsp;hours&nbsp;&nbsp;
                                <asp:TextBox ID="_txtMinutes" runat="Server" Width="24px" CssClass="formfield" TabIndex="12">0</asp:TextBox>&nbsp;mins&nbsp;&nbsp;
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="Server" ErrorMessage="Days is a required Filed"
                                    ControlToValidate="_period">&nbsp;*</asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="RangeValidator1" runat="Server" ErrorMessage="Days need to between 0~15 days."
                                    ControlToValidate="_period" MaximumValue="15" MinimumValue="0" Type="Integer">&nbsp;*</asp:RangeValidator>
                                <asp:RequiredFieldValidator ID="valForHours" runat="Server" ErrorMessage="Hours is a requried field."
                                    ControlToValidate="_txtHours">&nbsp;*</asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rangeValForHour" runat="Server" ErrorMessage="Hours need to be between 0~24"
                                    ControlToValidate="_txtHours" MaximumValue="24" MinimumValue="0" Type="Integer">&nbsp;*</asp:RangeValidator>
                                <asp:RequiredFieldValidator ID="validationForMin" runat="Server" ErrorMessage="Minutes is a requried field."
                                    ControlToValidate="_txtMinutes">&nbsp;*</asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rangeValForMin" runat="Server" ErrorMessage="Minutes need to be between 0~60"
                                    ControlToValidate="_txtMinutes" MaximumValue="60" MinimumValue="0" Type="Integer">&nbsp;*</asp:RangeValidator>
                            </div>
                        </td>
                        <td class="label" valign="middle" colspan="2"  align="left">
                            No-Open action will trigger&nbsp;
                            <div style="padding-top: 4px">
                                <asp:TextBox ID="NOOP_Period" runat="Server" Width="24px" CssClass="formfield" TabIndex="18">0</asp:TextBox>&nbsp;days&nbsp;&nbsp;
                                <asp:TextBox ID="NOOP_txtHours" runat="Server" Width="24px" CssClass="formfield"
                                    TabIndex="19">0</asp:TextBox>&nbsp;hours&nbsp;&nbsp;
                                <asp:TextBox ID="NOOP_txtMinutes" runat="Server" Width="24px" CssClass="formfield"
                                    TabIndex="20">0</asp:TextBox>&nbsp;mins&nbsp;
                            </div>
                            <div style="padding-top: 4px">
                                after Follow Up Campaign is sent out in 3a.
                                <asp:RequiredFieldValidator ID="val_NOOP_Period" runat="Server" ErrorMessage="Days is a required Filed"
                                    ControlToValidate="NOOP_Period" Enabled="False">&nbsp;*</asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rangeval_NOOP_Period" runat="Server" ErrorMessage="Days need to between 0~15 days."
                                    ControlToValidate="NOOP_Period" MaximumValue="15" MinimumValue="0" Type="Integer"
                                    Enabled="False">&nbsp;*</asp:RangeValidator>
                                <asp:RequiredFieldValidator ID="val_NOOP_txtHours" runat="Server" ErrorMessage="Hours is a requried field."
                                    ControlToValidate="NOOP_txtHours" Enabled="False">&nbsp;*</asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rangeval_NOOP_txtHours" runat="Server" ErrorMessage="Hours need to be between 0~24"
                                    ControlToValidate="NOOP_txtHours" MaximumValue="24" MinimumValue="0" Type="Integer"
                                    Enabled="False">&nbsp;*</asp:RangeValidator>
                                <asp:RequiredFieldValidator ID="val_NOOP_txtMinutes" runat="Server" ErrorMessage="Minutes is a requried field."
                                    ControlToValidate="NOOP_txtMinutes" Enabled="False">&nbsp;*</asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rangeval_NOOP_txtMinutes" runat="Server" ErrorMessage="Minutes need to be between 0~60"
                                    ControlToValidate="NOOP_txtMinutes" MaximumValue="60" MinimumValue="0" Type="Integer"
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
                            <asp:Button ID="_createButton" runat="Server" Text="Create Trigger" CssClass="formbuttonsmall"
                                TabIndex="21" OnClick="CreateButton_Click"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
