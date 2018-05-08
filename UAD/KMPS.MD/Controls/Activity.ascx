<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Activity.ascx.cs" Inherits="KMPS.MD.Controls.Activity" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<style type="text/css">
    .modalBackground {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }

    .ModalWindow {
        border: solid 1px #c0c0c0;
        background: #ffffff;
        padding: 0px 10px 10px 10px;
        position: absolute;
        top: -1000px;
    }

    .modalPopup {
        background-color: transparent;
        padding: 1em 6px;
        z-index: 10001 !important;
    }

    .modalPopupCal {
        background-color: transparent;
        padding: 1em 6px;
        z-index: 10002 !important;
    }

    .modalPopup2 {
        background-color: #ffffff;
        width: 270px;
        vertical-align: top;
    }

    .modalPopup2Cal {
        background-color: #ffffff;
        width: 270px;
        vertical-align: top;
        z-index: 10002 !important;
    }

    .watermarked {
        padding: 2px 0 0 2px;
        color: Gray;
    }

div.RadComboBox_Default table .rcbInputCell, 

div.RadComboBox_Default table .rcbArrowCell  
{ 
    border: Solid 1px grey !important;
    height: 14px;   
    line-height: 13px; 
    padding: 0; 
}  

div.RadComboBox_Default table .rcbInputCell input 
{ 
    background-color:white !important;  
    height: 14px; 
    line-height: 13px; 
    padding: 0; 
   
} 

.RadComboBox *
{
    height: 14px !important;
}

</style>
<asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="UpdatePanelActivity" DynamicLayout="true">
    <ProgressTemplate>
        <div class="TransparentGrayBackground">
        </div>
        <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 1000000; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
            <br />
            <b>Processing...</b><br />
            <br />
            <img src="../images/loading.gif" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="UpdatePanelActivity" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
        <ajaxToolkit:ModalPopupExtender ID="mpeCalendar" runat="server" TargetControlID="btnShowPopup"
            PopupControlID="pnlPopupDimensions2" CancelControlID="btnCancelDate" BackgroundCssClass="modalBackground" />
        <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender8" runat="server" BehaviorID="RoundedCornersBehavior8"
            TargetControlID="pnlCalendar" Radius="6" Corners="All" />
        <asp:Panel ID="pnlPopupDimensions2" runat="server" Width="400px" CssClass="modalPopupCal">
            <asp:Panel ID="pnlCalendar" runat="server" Width="400px" CssClass="modalPopup2Cal">
                <div align="center" style="text-align: center; height: 100%; padding: 0px 10px 0px 10px;">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD; height: auto">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">Choose Date
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" width="100%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td class="label" valign="middle" align="left" width="100%">
                                            <asp:Label ID="lblID" Text="" runat="server"
                                                Visible="false"></asp:Label>
                                            <input type="radio" id="rbToday" runat="server" name="DatePicker"
                                                onchange="rbToday_onchange(this);" />&nbsp;&nbsp;Today
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label" valign="middle" align="left">
                                            <input type="radio" id="rbTodayPlusMinus" runat="server" name="DatePicker"
                                                onchange="rbTodayPlusMinus_onchange(this);" />&nbsp;&nbsp;Plus/Minus Today
                                            <div id="divTodayPlusMinus" runat="Server" style="display: none;">
                                                <asp:DropDownList ID="ddlPlusMinus" runat="server">
                                                    <asp:ListItem Value="Plus" />
                                                    <asp:ListItem Value="Minus" />
                                                </asp:DropDownList>
                                                &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtDays" runat="server" class="formtextfield" Width="100"></asp:TextBox>&nbsp;<asp:RangeValidator
                                                    ID="rvDays" runat="Server" CssClass="errormsg" ControlToValidate="txtDays" ErrorMessage="Enter a number between 1 and 364."  ForeColor="red"
                                                    Display="Dynamic" ValidationGroup="Calendar" MaximumValue="364" MinimumValue="1"
                                                    Type="Integer" Enabled="False"></asp:RangeValidator>
                                                <asp:RequiredFieldValidator ID="rfvDays" ValidationGroup="Calendar" Display="Dynamic"
                                                    ControlToValidate="txtDays" runat="server" ErrorMessage="Enter a number between 1 and 364."  ForeColor="red"
                                                    Enabled="False"></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label" valign="middle" align="left">
                                            <input type="radio" id="rbOther" runat="server" name="DatePicker"
                                                onchange="rbOther_onchange(this);" />
                                            &nbsp;Other
                                            <div id="divOther" runat="Server" style="display: none;">
                                                <asp:TextBox ID="txtDatePicker" Width="70" MaxLength="10" runat="server"></asp:TextBox>&nbsp;<asp:ImageButton
                                                    ID="btnDatePicker" runat="server" ImageUrl="~/Images/icon-calendar.gif" ImageAlign="Bottom" /><ajaxToolkit:CalendarExtender
                                                        ID="CalendarExtender11" runat="server" CssClass="MyCalendar" TargetControlID="txtDatePicker"
                                                        Format="MM/dd/yyyy" PopupButtonID="btnDatePicker">
                                                    </ajaxToolkit:CalendarExtender>
                                                <asp:CompareValidator ID="cmpStartDate" runat="server"
                                                    ControlToValidate="txtDatePicker"
                                                    ErrorMessage="Please enter a valid date"
                                                    Operator="DataTypeCheck" Type="Date" ValidationGroup="Calendar"></asp:CompareValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100%" height="40" align="center">
                                            <asp:Button runat="server" Text="Choose Date" ID="btnSelectDate" class="formbuttonsmall"
                                                OnClick="btnSelectDate_Click" ValidationGroup="Calendar" Width="90px"></asp:Button>
                                            <asp:Button runat="server" Text="Cancel" ID="btnCancelDate" class="formbuttonsmall"
                                                Width="90px"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Button ID="btnShowActivitys" runat="server" Style="display: none" />
        <ajaxToolkit:ModalPopupExtender ID="mdlActivitys" runat="server" TargetControlID="btnShowActivitys"
            PopupControlID="pnlActivitys" BackgroundCssClass="modalBackground" BehaviorID="BPopup" />
        <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender" runat="server" BehaviorID="RoundedCornersBehavior6"
            TargetControlID="pnlPopupActivityRound" Radius="6" Corners="All" />
        <asp:Panel ID="pnlActivitys" runat="server" Width="800px" CssClass="modalPopup">
            <asp:Panel ID="pnlPopupActivityRound" runat="server" Width="800px" CssClass="modalPopup2">
                <div id="dvDownloads" align="center" style="text-align: center; padding: 10px 10px 10px 10px;"
                    runat="server">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD; height: auto">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">Activity Filter
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divError" runat="Server" visible="false" style="width: 400px">
                                    <table cellspacing="0" cellpadding="0" width="400px" align="center">
                                        <tr>
                                            <td id="errorMiddle" width="100%">
                                                <table width="100%">
                                                    <tr>
                                                        <td valign="top" align="center" width="20%">
                                                            <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                                                alt="" />
                                                        </td>
                                                        <td valign="middle" align="left" width="80%" height="100%">
                                                            <asp:Label ID="lblErrorMessage" runat="Server" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="always">
                                    <ContentTemplate>
                                        <table cellspacing="0" cellpadding="6" width="100%" align="center">
                                            <tr style="background-color: #EBEBEB">
                                                <td align="left">
                                                    <b>Open Criteria</b>&nbsp;&nbsp;<asp:DropDownList ID="drpOpenActivity" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                        Style="text-transform: uppercase" Width="100px">
                                                        <asp:ListItem Value=""></asp:ListItem>
                                                        <asp:ListItem Value="0">No Opens</asp:ListItem>
                                                        <asp:ListItem Value="1">Opened 1+</asp:ListItem>
                                                        <asp:ListItem Value="2">Opened 2+</asp:ListItem>
                                                        <asp:ListItem Value="3">Opened 3+</asp:ListItem>
                                                        <asp:ListItem Value="4">Opened 4+</asp:ListItem>
                                                        <asp:ListItem Value="5">Opened 5+</asp:ListItem>
                                                        <asp:ListItem Value="10">Opened 10+</asp:ListItem>
                                                        <asp:ListItem Value="15">Opened 15+</asp:ListItem>
                                                        <asp:ListItem Value="20">Opened 20+</asp:ListItem>
                                                        <asp:ListItem Value="30">Opened 30+</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <div id="divOpenDate" runat="Server" style="display: none;">
                                                        <table border="0" width="100%">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Panel ID="pnlOpenSearchType" runat="server" Visible="false">
                                                                        <asp:RadioButtonList ID="rblOpenSearchType" runat="server" Visible="false" RepeatDirection="Horizontal">
                                                                            <asp:ListItem Value="Search All" Selected="True">Search All</asp:ListItem>
                                                                            <asp:ListItem Value="Search Selected Products">Search Selected Products</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="20%" align="right">Open Date</td>
                                                                <td width="80%">
                                                                    <asp:DropDownList ID="drpOpenActivityDateRange" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                        Width="100" onchange="drpOpenActivityDateRange_onchange(this);">
                                                                        <asp:ListItem Value="DateRange" Selected="True">DATE RANGE</asp:ListItem>
                                                                        <asp:ListItem Value="XDays">LAST X DAYS</asp:ListItem>
                                                                        <asp:ListItem Value="Year">YEAR</asp:ListItem>
                                                                        <asp:ListItem Value="Month">MONTH</asp:ListItem>
                                                                    </asp:DropDownList>&nbsp;
                                                                    <div id="divOpenActivityDateRange" runat="Server" style="display: none">
                                                                        <asp:TextBox ID="txtOpenActivityFrom" Width="95" CssClass="formfield" MaxLength="10"
                                                                            runat="server" ReadOnly="true"></asp:TextBox>
                                                                        <asp:ImageButton ID="ibOpenActivtyFromDate" runat="server" ImageUrl="~/Images/icon-calendar.gif" OnCommand="ibChooseDate_Command" ImageAlign="Bottom" CommandArgument="OpenActivityFromDate" />&nbsp                                                    
                                                                        To &nbsp;
                                                                        <asp:TextBox ID="txtOpenActivityTo" Width="95" CssClass="formfield" MaxLength="10" runat="server" ReadOnly="true"></asp:TextBox>
                                                                        <asp:ImageButton ID="ibOpenActivtyToDate" runat="server" ImageUrl="~/Images/icon-calendar.gif" OnCommand="ibChooseDate_Command" ImageAlign="Bottom" CommandArgument="OpenActivityToDate" />&nbsp                                                    
                                                                    </div>
                                                                    <asp:DropDownList ID="drpOpenActivityDays" runat="server" Width="100" Style="display: none" onchange="drpOpenActivityDays_onchange(this);">
                                                                        <asp:ListItem Selected="true" Value="7">7 days</asp:ListItem>
                                                                        <asp:ListItem Value="14">14 days</asp:ListItem>
                                                                        <asp:ListItem Value="21">21 days</asp:ListItem>
                                                                        <asp:ListItem Value="30">30 days</asp:ListItem>
                                                                        <asp:ListItem Value="60">60 days</asp:ListItem>
                                                                        <asp:ListItem Value="90">90 days</asp:ListItem>
                                                                        <asp:ListItem Value="120">120 days</asp:ListItem>
                                                                        <asp:ListItem Value="150">150 days</asp:ListItem>
                                                                        <asp:ListItem Value="6mon">6 months</asp:ListItem>
                                                                        <asp:ListItem Value="1yr">1 year</asp:ListItem>
                                                                        <asp:ListItem Value="Custom">Custom</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <div id="divCustomOpenActivityDays" runat="Server" style="display: none">
                                                                        <asp:TextBox ID="txtCustomOpenActivityDays" Width="95" CssClass="formfield" MaxLength="10" runat="server"></asp:TextBox>&nbsp;Days
                                                                        <asp:RequiredFieldValidator ID="rfvCustomOpenActivityDays" ValidationGroup="Select" Display="Dynamic"
                                                                        ControlToValidate="txtCustomOpenActivityDays" runat="server" ErrorMessage="Enter a number between 1 and 730." ForeColor="red"
                                                                        Enabled="False">
                                                                        </asp:RequiredFieldValidator>
                                                                        <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Enter a number between 1 and 730." MaximumValue="730"
                                                                        MinimumValue="0" ControlToValidate="txtCustomOpenActivityDays" Type="Integer" ForeColor="red" Display="Dynamic" ValidationGroup="Select"></asp:RangeValidator>
                                                                    </div>
                                                                    <div id="divOpenActivityYear" runat="Server" style="display: none">
                                                                        <asp:TextBox ID="txtOpenActivityFromYear" CssClass="formfield" Width="100px"
                                                                            runat="server" MaxLength="4"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtOpenActivityFromYear" FilterType="Numbers" />
                                                                        &nbsp;To&nbsp;
                                                                        <asp:TextBox ID="txtOpenActivityToYear" CssClass="formfield" Width="100px" MaxLength="4"
                                                                            runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtOpenActivityToYear" FilterType="Numbers" />
                                                                    </div>
                                                                    <div id="divOpenActivityMonth" runat="Server" style="display: none">
                                                                        <asp:TextBox ID="txtOpenActivityFromMonth" CssClass="formfield" Width="100px" MaxLength="2"
                                                                            runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtOpenActivityFromMonth" FilterType="Numbers" />
                                                                        &nbsp;To&nbsp;
                                                                        <asp:TextBox ID="txtOpenActivityToMonth" CssClass="formfield" Width="100px" MaxLength="2"
                                                                            runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtOpenActivityToMonth" FilterType="Numbers" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div id="divOpenEmail" runat="Server" style="display: none;">
                                                        <table border="0" width="100%">
                                                            <tr>
                                                                <td width="20%" align="right">BlastID </td>
                                                                <td width="80%">
                                                                    <asp:TextBox ID="txtOpenBlastID" CssClass="formfield" Width="140px"
                                                                        runat="server"></asp:TextBox>
                                                                    <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                                                                        Enabled="true" TargetControlID="txtOpenBlastID" WatermarkCssClass="watermarked"
                                                                        WatermarkText="single, comma delimited" />
                                                                    &nbsp; &nbsp;
                                                                    Campaigns
                                                                    <telerik:RadComboBox ID="RadCBOpenCampaigns" runat="server" CheckBoxes="true" Font-Names="Arial" Font-Size="x-small" Width="150px" DataValueField="ECNCampaignID" DataTextField="ECNCampaignName" ZIndex="999999" DropDownAutoWidth="Enabled"  EnableScreenBoundaryDetection ="false">
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">Email Subject </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtOpenEmailSubject" Width="205" CssClass="formfield" MaxLength="255"
                                                                        runat="server"></asp:TextBox>
                                                                    <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server"
                                                                        Enabled="true" TargetControlID="txtOpenEmailSubject" WatermarkCssClass="watermarked"
                                                                        WatermarkText="exact match, partial match, keyword" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">Email Sent Date</td>
                                                                <td>
                                                                    <asp:DropDownList ID="drpOpenEmailDateRange" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                        Width="100" onchange="drpOpenEmailDateRange_onchange(this);">
                                                                        <asp:ListItem Value="DateRange">DATE RANGE</asp:ListItem>
                                                                        <asp:ListItem Value="XDays">LAST X DAYS</asp:ListItem>
                                                                        <asp:ListItem Value="Year">YEAR</asp:ListItem>
                                                                        <asp:ListItem Value="Month">MONTH</asp:ListItem>
                                                                    </asp:DropDownList>&nbsp;
                                                                    <div id="divOpenEmailDateRange" runat="Server" style="display: none">
                                                                        <asp:TextBox ID="txtOpenEmailFromDate" Width="95" CssClass="formfield" MaxLength="10" runat="server" ReadOnly="true" />
                                                                        <asp:ImageButton ID="ibOpenEmailFromDate" runat="server" ImageUrl="~/Images/icon-calendar.gif" OnCommand="ibChooseDate_Command" ImageAlign="Bottom" CommandArgument="OpenEmailFromDate" />&nbsp                                                    
                                                                         To &nbsp;
                                                                        <asp:TextBox ID="txtOpenEmailToDate" Width="95" CssClass="formfield" MaxLength="10" runat="server" ReadOnly="true" />
                                                                        <asp:ImageButton ID="ibOpenEmailToDate" runat="server" ImageUrl="~/Images/icon-calendar.gif" OnCommand="ibChooseDate_Command" ImageAlign="Bottom" CommandArgument="OpenEmailToDate" />&nbsp                                                    
                                                                    </div>
                                                                    <asp:DropDownList ID="drpOpenEmailDays" runat="server" Width="100" Style="display: none"  onchange="drpOpenEmailDays_onchange(this);">
                                                                        <asp:ListItem Selected="true" Value="7">7 days</asp:ListItem>
                                                                        <asp:ListItem Value="14">14 days</asp:ListItem>
                                                                        <asp:ListItem Value="21">21 days</asp:ListItem>
                                                                        <asp:ListItem Value="30">30 days</asp:ListItem>
                                                                        <asp:ListItem Value="60">60 days</asp:ListItem>
                                                                        <asp:ListItem Value="90">90 days</asp:ListItem>
                                                                        <asp:ListItem Value="120">120 days</asp:ListItem>
                                                                        <asp:ListItem Value="150">150 days</asp:ListItem>
                                                                        <asp:ListItem Value="6mon">6 months</asp:ListItem>
                                                                        <asp:ListItem Value="1yr">1 year</asp:ListItem>
                                                                        <asp:ListItem Value="Custom">Custom</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <div id="divCustomOpenEmailDays" runat="Server" style="display: none" >
                                                                        <asp:TextBox ID="txtCustomOpenEmailDays" Width="95" CssClass="formfield" MaxLength="10" runat="server"></asp:TextBox>&nbsp;Days
                                                                        <asp:RequiredFieldValidator ID="rfvCustomOpenEmailDays" ValidationGroup="Select" Display="Dynamic"
                                                                        ControlToValidate="txtCustomOpenEmailDays" runat="server" ErrorMessage="Enter a number between 1 and 730." ForeColor="red"
                                                                        Enabled="False">
                                                                        </asp:RequiredFieldValidator>
                                                                        <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="Enter a number between 1 and 730." MaximumValue="730"
                                                                        MinimumValue="0" ControlToValidate="txtCustomOpenEmailDays" Type="Integer" ForeColor="red" Display="Dynamic" ValidationGroup="Select"></asp:RangeValidator>
                                                                    </div>
                                                                    <div id="divOpenEmailYear" runat="Server" style="display: none">
                                                                        <asp:TextBox ID="txtOpenEmailFromYear" CssClass="formfield" Width="100px" MaxLength="4"
                                                                            runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtOpenEmailFromYear" FilterType="Numbers" />
                                                                        &nbsp;To&nbsp;
                                                                        <asp:TextBox ID="txtOpenEmailToYear" CssClass="formfield" Width="100px" MaxLength="4"
                                                                            runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtOpenEmailToYear" FilterType="Numbers" />
                                                                    </div>
                                                                    <div id="divOpenEmailMonth" runat="Server" style="display: none">
                                                                        <asp:TextBox ID="txtOpenEmailFromMonth" CssClass="formfield" Width="100px" MaxLength="2"
                                                                            runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtOpenEmailFromMonth" FilterType="Numbers" />
                                                                        &nbsp;To&nbsp;
                                                                        <asp:TextBox ID="txtOpenEmailToMonth" CssClass="formfield" Width="100px" MaxLength="2"
                                                                            runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtOpenEmailToMonth" FilterType="Numbers" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #EBEBEB">
                                                <td align="left">
                                                    <b>Click Criteria</b>&nbsp;&nbsp;<asp:DropDownList ID="drpClickActivity" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                        Style="text-transform: uppercase" Width="100px">
                                                        <asp:ListItem Value=""></asp:ListItem>
                                                        <asp:ListItem Value="0">No Clicks</asp:ListItem>
                                                        <asp:ListItem Value="1">Clicked 1+</asp:ListItem>
                                                        <asp:ListItem Value="2">Clicked 2+</asp:ListItem>
                                                        <asp:ListItem Value="3">Clicked 3+</asp:ListItem>
                                                        <asp:ListItem Value="4">Clicked 4+</asp:ListItem>
                                                        <asp:ListItem Value="5">Clicked 5+</asp:ListItem>
                                                        <asp:ListItem Value="10">Clicked 10+</asp:ListItem>
                                                        <asp:ListItem Value="15">Clicked 15+</asp:ListItem>
                                                        <asp:ListItem Value="20">Clicked 20+</asp:ListItem>
                                                        <asp:ListItem Value="30">Clicked 30+</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <div id="divClickDate" runat="Server" style="display: none;">
                                                        <table border="0" width="100%">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Panel ID="pnlClickSearchType" runat="server" Visible="false">
                                                                        <asp:RadioButtonList ID="rblClickSearchType" runat="server" Visible="false" RepeatDirection="Horizontal">
                                                                            <asp:ListItem Value="Search All" Selected="True">Search All</asp:ListItem>
                                                                            <asp:ListItem Value="Search Selected Products">Search Selected Products</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" width="20%">Click Date</td>
                                                                <td width="80%">
                                                                    <asp:DropDownList ID="drpClickActivityDateRange" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                        Width="100" onchange="drpClickActivityDateRange_onchange(this);">
                                                                        <asp:ListItem Value="DateRange">DATE RANGE</asp:ListItem>
                                                                        <asp:ListItem Value="XDays">LAST X DAYS</asp:ListItem>
                                                                        <asp:ListItem Value="Year">YEAR</asp:ListItem>
                                                                        <asp:ListItem Value="Month">MONTH</asp:ListItem>
                                                                    </asp:DropDownList>&nbsp;
                                                                    <div id="divClickActivityDateRange" runat="Server" style="display: none; line-height: 30px;">
                                                                        <asp:TextBox ID="txtClickActivityFrom" Width="95" CssClass="formfield" MaxLength="10"
                                                                            runat="server" ReadOnly="true"></asp:TextBox>
                                                                        <asp:ImageButton ID="ibClickActivtyFromDate" runat="server" ImageUrl="~/Images/icon-calendar.gif" OnCommand="ibChooseDate_Command" ImageAlign="Bottom" CommandArgument="ClickActivityFromDate" />&nbsp                                                                                                        
                                                                        To &nbsp;
                                                                    <asp:TextBox ID="txtClickActivityTo" Width="95" CssClass="formfield" MaxLength="10" runat="server" ReadOnly="true"></asp:TextBox>
                                                                        <asp:ImageButton ID="ibClickActivtyToDate" runat="server" ImageUrl="~/Images/icon-calendar.gif" OnCommand="ibChooseDate_Command" ImageAlign="Bottom" CommandArgument="ClickActivityToDate" />&nbsp                                                                                                        
                                                                    </div>
                                                                    <asp:DropDownList ID="drpClickActivityDays" runat="server" Width="100" Style="display: none" onchange="drpClickActivityDays_onchange(this);">
                                                                        <asp:ListItem Selected="true" Value="7">7 days</asp:ListItem>
                                                                        <asp:ListItem Value="14">14 days</asp:ListItem>
                                                                        <asp:ListItem Value="21">21 days</asp:ListItem>
                                                                        <asp:ListItem Value="30">30 days</asp:ListItem>
                                                                        <asp:ListItem Value="60">60 days</asp:ListItem>
                                                                        <asp:ListItem Value="90">90 days</asp:ListItem>
                                                                        <asp:ListItem Value="120">120 days</asp:ListItem>
                                                                        <asp:ListItem Value="150">150 days</asp:ListItem>
                                                                        <asp:ListItem Value="6mon">6 months</asp:ListItem>
                                                                        <asp:ListItem Value="1yr">1 year</asp:ListItem>
                                                                        <asp:ListItem Value="Custom">Custom</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <div id="divCustomClickActivityDays" runat="Server" style="display: none" >
                                                                        <asp:TextBox ID="txtCustomClickActivityDays" Width="95" CssClass="formfield" MaxLength="10" runat="server"></asp:TextBox>&nbsp;Days
                                                                        <asp:RequiredFieldValidator ID="rfvCustomClickActivityDays" ValidationGroup="Select" Display="Dynamic"
                                                                        ControlToValidate="txtCustomClickActivityDays" runat="server" ErrorMessage="Enter a number between 1 and 730." ForeColor="red"
                                                                        Enabled="False">
                                                                        </asp:RequiredFieldValidator>
                                                                        <asp:RangeValidator ID="RangeValidator3" runat="server" ErrorMessage="Enter a number between 1 and 730." MaximumValue="730"
                                                                        MinimumValue="0" ControlToValidate="txtCustomClickActivityDays" Type="Integer" ForeColor="red" Display="Dynamic" ValidationGroup="Select"></asp:RangeValidator>
                                                                    </div>
                                                                    <div id="divClickActivityYear" runat="Server" style="display: none">
                                                                        <asp:TextBox ID="txtClickActivityFromYear" CssClass="formfield" Width="100px" MaxLength="4"
                                                                            runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtClickActivityFromYear" FilterType="Numbers" />
                                                                        &nbsp;To&nbsp;
                                                                        <asp:TextBox ID="txtClickActivityToYear" CssClass="formfield" Width="100px" MaxLength="4"
                                                                            runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtClickActivityToYear" FilterType="Numbers" />
                                                                    </div>
                                                                    <div id="divClickActivityMonth" runat="Server" style="display: none">
                                                                        <asp:TextBox ID="txtClickActivityFromMonth" CssClass="formfield" Width="100px" MaxLength="2"
                                                                            runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtClickActivityFromMonth" FilterType="Numbers" />
                                                                        &nbsp;To&nbsp;
                                                                        <asp:TextBox ID="txtClickActivityToMonth" CssClass="formfield" Width="100px" MaxLength="2"
                                                                            runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtClickActivityToMonth" FilterType="Numbers" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div id="divClickEmail" runat="Server" style="display: none;">
                                                        <table border="0" width="100%">
                                                            <tr>
                                                                <td align="right" width="20%">URL</td>
                                                                <td width="80%">
                                                                    <asp:TextBox ID="txtLink" Width="346" CssClass="formfield" MaxLength="2048" runat="server"></asp:TextBox>
                                                                    <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server"
                                                                        Enabled="true" TargetControlID="txtLink" WatermarkCssClass="watermarked"
                                                                        WatermarkText="exact match, partial match, keyword, single, comma delimited" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">BlastID</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtClickBlastID" CssClass="formfield" Width="140px"
                                                                        runat="server"></asp:TextBox>
                                                                    <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server"
                                                                        Enabled="true" TargetControlID="txtClickBlastID" WatermarkCssClass="watermarked"
                                                                        WatermarkText="single, comma delimited" />
                                                                    &nbsp; &nbsp;
                                                                    Campaigns
                                                                    <telerik:RadComboBox ID="RadCBClickCampaigns" runat="server" CheckBoxes="true" Font-Names="Arial" Font-Size="x-small" Width="150px" DataValueField="ECNCampaignID" DataTextField="ECNCampaignName"  ZIndex="999999" DropDownAutoWidth="Enabled" EnableScreenBoundaryDetection ="false">
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">Email Subject</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtClickEmailSubject" Width="205" CssClass="formfield" MaxLength="255"
                                                                        runat="server"></asp:TextBox>
                                                                    <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server"
                                                                        Enabled="true" TargetControlID="txtClickEmailSubject" WatermarkCssClass="watermarked"
                                                                        WatermarkText="exact match, partial match, keyword" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">Email Sent Date</td>
                                                                <td>
                                                                    <asp:DropDownList ID="drpClickEmailDateRange" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                        Width="100" onchange="drpClickEmailDateRange_onchange(this);">
                                                                        <asp:ListItem Value="DateRange">DATE RANGE</asp:ListItem>
                                                                        <asp:ListItem Value="XDays">LAST X DAYS</asp:ListItem>
                                                                        <asp:ListItem Value="Year">YEAR</asp:ListItem>
                                                                        <asp:ListItem Value="Month">MONTH</asp:ListItem>
                                                                    </asp:DropDownList>&nbsp;
                                                                    <div id="divClickEmailDateRange" runat="Server" style="display: none">
                                                                        <asp:TextBox ID="txtClickEmailFromDate" Width="95" CssClass="formfield" MaxLength="10" runat="server" ReadOnly="true" />
                                                                        <asp:ImageButton ID="ibClickEmailFromDate" runat="server" ImageUrl="~/Images/icon-calendar.gif" OnCommand="ibChooseDate_Command" ImageAlign="Bottom" CommandArgument="ClickEmailFromDate" />&nbsp                                                    
                                                                    To &nbsp;
                                                                    <asp:TextBox ID="txtClickEmailToDate" Width="95" CssClass="formfield" MaxLength="10" runat="server" ReadOnly="true" />
                                                                        <asp:ImageButton ID="ibClickEmailToDate" runat="server" ImageUrl="~/Images/icon-calendar.gif" OnCommand="ibChooseDate_Command" ImageAlign="Bottom" CommandArgument="ClickEmailToDate" />&nbsp                                                                                                        
                                                                    </div>
                                                                    <asp:DropDownList ID="drpClickEmailDays" runat="server" Width="100" Style="display: none" onchange="drpClickEmailDays_onchange(this);">
                                                                        <asp:ListItem Selected="true" Value="7">7 days</asp:ListItem>
                                                                        <asp:ListItem Value="14">14 days</asp:ListItem>
                                                                        <asp:ListItem Value="21">21 days</asp:ListItem>
                                                                        <asp:ListItem Value="30">30 days</asp:ListItem>
                                                                        <asp:ListItem Value="60">60 days</asp:ListItem>
                                                                        <asp:ListItem Value="90">90 days</asp:ListItem>
                                                                        <asp:ListItem Value="120">120 days</asp:ListItem>
                                                                        <asp:ListItem Value="150">150 days</asp:ListItem>
                                                                        <asp:ListItem Value="6mon">6 months</asp:ListItem>
                                                                        <asp:ListItem Value="1yr">1 year</asp:ListItem>
                                                                        <asp:ListItem Value="Custom">Custom</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <div id="divCustomClickEmailDays" runat="Server" style="display: none" >
                                                                        <asp:TextBox ID="txtCustomClickEmailDays" Width="95" CssClass="formfield" MaxLength="10" runat="server"></asp:TextBox>&nbsp;Days
                                                                        <asp:RequiredFieldValidator ID="rfvCustomClickEmailDays" ValidationGroup="Select" Display="Dynamic"
                                                                        ControlToValidate="txtCustomClickEmailDays" runat="server" ErrorMessage="Enter a number between 1 and 730." ForeColor="red"
                                                                        Enabled="False">
                                                                        </asp:RequiredFieldValidator>
                                                                        <asp:RangeValidator ID="RangeValidator4" runat="server" ErrorMessage="Enter a number between 1 and 730." MaximumValue="730"
                                                                        MinimumValue="0" ControlToValidate="txtCustomClickEmailDays" Type="Integer" ForeColor="red" Display="Dynamic" ValidationGroup="Select"></asp:RangeValidator>
                                                                    </div>
                                                                    <div id="divClickEmailYear" runat="Server" style="display: none">
                                                                        <asp:TextBox ID="txtClickEmailFromYear" CssClass="formfield" Wi1dth="100px" MaxLength="4"
                                                                            runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txtClickEmailFromYear" FilterType="Numbers" />
                                                                        &nbsp;To&nbsp;
                                                                        <asp:TextBox ID="txtClickEmailToYear" CssClass="formfield" Width="100px" MaxLength="4"
                                                                            runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtClickEmailToYear" FilterType="Numbers" />
                                                                    </div>
                                                                    <div id="divClickEmailMonth" runat="Server" style="display: none">
                                                                        <asp:TextBox ID="txtClickEmailFromMonth" CssClass="formfield" Width="100px" MaxLength="2"
                                                                            runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txtClickEmailFromMonth" FilterType="Numbers" />
                                                                        &nbsp;To&nbsp;
                                                                        <asp:TextBox ID="txtClickEmailToMonth" CssClass="formfield" Width="100px" MaxLength="2"
                                                                            runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="txtClickEmailToMonth" FilterType="Numbers" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #EBEBEB">
                                                <td align="left">
                                                    <b>Website Visits</b>&nbsp;&nbsp;<asp:DropDownList ID="drpVisitActivity" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                        Style="text-transform: uppercase" Width="100px">
                                                        <asp:ListItem Value=""></asp:ListItem>
                                                        <asp:ListItem Value="0">No Visits</asp:ListItem>
                                                        <asp:ListItem Value="1">Visited 1+</asp:ListItem>
                                                        <asp:ListItem Value="2">Visited 2+</asp:ListItem>
                                                        <asp:ListItem Value="3">Visited 3+</asp:ListItem>
                                                        <asp:ListItem Value="4">Visited 4+</asp:ListItem>
                                                        <asp:ListItem Value="5">Visited 5+</asp:ListItem>
                                                        <asp:ListItem Value="10">Visited 10+</asp:ListItem>
                                                        <asp:ListItem Value="15">Visited 15+</asp:ListItem>
                                                        <asp:ListItem Value="20">Visited 20+</asp:ListItem>
                                                        <asp:ListItem Value="30">Visited 30+</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="divVisitDomain" runat="Server" style="display: none;">
                                                        <table border="0" width="100%">
                                                            <tr>
                                                                <td align="left">Domain&nbsp;<asp:DropDownList ID="drpDomain" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                    Style="text-transform: uppercase" Width="200px" DataTextField="DomainName" DataValueField="DomainTrackingID">
                                                                </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;URL&nbsp;<asp:TextBox ID="txtURL" Width="400" CssClass="formfield" MaxLength="500" runat="server"></asp:TextBox>
                                                                    <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender6" runat="server"
                                                                        Enabled="true" TargetControlID="txtURL" WatermarkCssClass="watermarked"
                                                                        WatermarkText="exact match, partial match, keyword, single, comma delimited" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <div id="divVisitDate" runat="Server" style="display: none;">
                                                        <asp:DropDownList ID="drpVisitActivityDateRange" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                            Width="100px" onchange="drpVisitActivityDateRange_onchange(this);">
                                                            <asp:ListItem Value="DateRange">DATE RANGE</asp:ListItem>
                                                            <asp:ListItem Value="XDays">LAST X DAYS</asp:ListItem>
                                                            <asp:ListItem Value="Year">YEAR</asp:ListItem>
                                                            <asp:ListItem Value="Month">MONTH</asp:ListItem>
                                                        </asp:DropDownList>&nbsp;    
                                                    <div id="divVisitActivityDateRange" runat="Server" style="display: inline">
                                                        <asp:TextBox ID="txtVisitActivityFrom" Width="100px" CssClass="formfield" MaxLength="10"
                                                            runat="server" ReadOnly="true"></asp:TextBox>
                                                        <asp:ImageButton ID="ibVisitActivtyFromDate" runat="server" ImageUrl="~/Images/icon-calendar.gif" OnCommand="ibChooseDate_Command" ImageAlign="Bottom" CommandArgument="VisitActivityFromDate" />&nbsp                                                    
                                                        
                                                    To &nbsp;
                                                    <asp:TextBox ID="txtVisitActivityTo" Width="100px" CssClass="formfield" MaxLength="10" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <asp:ImageButton ID="ibVisitActivtyToDate" runat="server" ImageUrl="~/Images/icon-calendar.gif" OnCommand="ibChooseDate_Command" ImageAlign="Bottom" CommandArgument="VisitActivityToDate" />&nbsp                                                                                                        
                                                    </div>
                                                        <asp:DropDownList ID="drpVisitActivityDays" runat="server" Width="100px" Style="display: none" onchange="drpVisitActivityDays_onchange(this);">
                                                            <asp:ListItem Selected="true" Value="7">7 days</asp:ListItem>
                                                            <asp:ListItem Value="14">14 days</asp:ListItem>
                                                            <asp:ListItem Value="21">21 days</asp:ListItem>
                                                            <asp:ListItem Value="30">30 days</asp:ListItem>
                                                            <asp:ListItem Value="60">60 days</asp:ListItem>
                                                            <asp:ListItem Value="90">90 days</asp:ListItem>
                                                            <asp:ListItem Value="120">120 days</asp:ListItem>
                                                            <asp:ListItem Value="150">150 days</asp:ListItem>
                                                            <asp:ListItem Value="6mon">6 months</asp:ListItem>
                                                            <asp:ListItem Value="1yr">1 year</asp:ListItem>
                                                            <asp:ListItem Value="Custom">Custom</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <div id="divCustomVisitActivityDays" runat="Server" style="display: none" >
                                                            <asp:TextBox ID="txtCustomVisitActivityDays" Width="95" CssClass="formfield" MaxLength="10" runat="server"></asp:TextBox>&nbsp;Days
                                                            <asp:RequiredFieldValidator ID="rfvCustomVisitActivityDays" ValidationGroup="Select" Display="Dynamic"
                                                            ControlToValidate="txtCustomVisitActivityDays" runat="server" ErrorMessage="Enter a number between 1 and 730." ForeColor="red"
                                                            Enabled="False">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:RangeValidator ID="RangeValidator5" runat="server" ErrorMessage="Enter a number between 1 and 730." MaximumValue="730"
                                                            MinimumValue="0" ControlToValidate="txtCustomVisitActivityDays" Type="Integer" ForeColor="red" Display="Dynamic" ValidationGroup="Select"></asp:RangeValidator>
                                                        </div>
                                                        <div id="divVisitActivityYear" runat="Server" style="display: none">
                                                            <asp:TextBox ID="txtVisitActivityFromYear" CssClass="formfield" Width="100px" MaxLength="4"
                                                                runat="server"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="txtVisitActivityFromYear" FilterType="Numbers" />
                                                            &nbsp;To&nbsp;
                                                        <asp:TextBox ID="txtVisitActivityToYear" CssClass="formfield" Width="100px" MaxLength="4"
                                                            runat="server"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="txtVisitActivityToYear" FilterType="Numbers" />
                                                        </div>
                                                        <div id="divVisitActivityMonth" runat="Server" style="display: none">
                                                            <asp:TextBox ID="txtVisitActivityFromMonth" CssClass="formfield" Width="100px" MaxLength="2"
                                                                runat="server"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txtVisitActivityFromMonth" FilterType="Numbers" />
                                                            &nbsp;To&nbsp;
                                                        <asp:TextBox ID="txtVisitActivityToMonth" CssClass="formfield" Width="100px" MaxLength="2"
                                                            runat="server"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="txtVisitActivityToMonth" FilterType="Numbers" />
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSelect" Text="Select/Close" CssClass="button" runat="server" ValidationGroup="Select"
                                    OnClick="btnSelect_Click" />
                                <asp:Button ID="btnReset" Text="Reset" CssClass="button" runat="server" OnClick="btnReset_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
