<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="Adhoc.ascx.cs" Inherits="KMPS.MD.Controls.Adhoc" %>
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
        z-index: 10001 !important;
    }

    .modalPopup2Cal {
        background-color: #ffffff;
        width: 270px;
        vertical-align: top;
        z-index: 10002 !important;
    }
</style>
<asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="UpdatePanelDownload" DynamicLayout="true">
    <ProgressTemplate>
        <div class="TransparentGrayBackground">
        </div>
        <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 10; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
            <br />
            <b>Processing...</b><br />
            <br />
            <img src="../images/loading.gif" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="UpdatePanelDownload" runat="server" UpdateMode="Always">
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
                                                    ID="rvDays" runat="Server" CssClass="errormsg" ControlToValidate="txtDays" ErrorMessage="Enter a number between 1 and 364."
                                                    Display="Dynamic" ValidationGroup="Calendar" MaximumValue="364" MinimumValue="1"
                                                    Type="Integer" Enabled="False"></asp:RangeValidator>
                                                <asp:RequiredFieldValidator ID="rfvDays" ValidationGroup="Calendar" Display="Dynamic"
                                                    ControlToValidate="txtDays" runat="server" ErrorMessage="Enter a number between 1 and 364."
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

        <asp:Button ID="btnShowDownloads" runat="server" Style="display: none" />
        <ajaxToolkit:ModalPopupExtender ID="mpeDownloads" runat="server" TargetControlID="btnShowDownloads"
            PopupControlID="pnlDownloads" BackgroundCssClass="modalBackground" BehaviorID="BPopup" />
        <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender" runat="server" BehaviorID="RoundedCornersBehavior6"
            TargetControlID="pnlPopupDownloadRound" Radius="6" Corners="All" />
        <asp:Panel ID="pnlDownloads" runat="server" Width="600px" CssClass="modalPopup">
            <asp:Panel ID="pnlPopupDownloadRound" runat="server" Width="600px" CssClass="modalPopup2">
                <div id="dvDownloads" align="center" style="text-align: center; padding: 10px 10px 10px 10px; height: 100%"
                    runat="server">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD; height: auto">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">Adhoc Filter
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
                                <%--                                <asp:Label ID="lbAdhocText" Text='<%#Eval("ColumnValue") %>' runat="server" Visible="false"></asp:Label>--%>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="always">
                                    <ContentTemplate>
                                        <div id="divScroll" style="height: 350px; overflow: auto;" runat="server">
                                            <asp:GridView ID="gvCategory" runat="server" AutoGenerateColumns="False" DataKeyNames="CategoryID"
                                                EnableModelValidation="True" AllowSorting="false" OnRowDataBound="gvCategory_RowDataBound"
                                                AllowPaging="True" RowStyle-BackColor="#EBEBEB">
                                                <Columns>
                                                    <asp:BoundField DataField="CategoryName" HeaderText="Category"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="50%" ItemStyle-Width="88%"
                                                        ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td colspan="80%" align="left">
                                                                    <asp:DataList ID="dlAdhocFilter" runat="server" AlternatingItemStyle-Width="50%"
                                                                        ItemStyle-Width="50%" RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Table"
                                                                        ItemStyle-BorderWidth="0" Width="100%" OnItemDataBound="dlAdhocFilter_ItemDataBound">
                                                                        <ItemTemplate>
                                                                            <table cellspacing="0" cellpadding="0" width="100%" align="center" border='0'>
                                                                                <tr>
                                                                                    <td width="30%" align="left">
                                                                                        <asp:Label ID="lbAdhocColumnValue" Text='<%#Eval("ColumnValue") %>' runat="server"
                                                                                            Visible="false"></asp:Label>
                                                                                        <asp:Label ID="lbAdhocColumnType" Text='<%#Eval("ColumnType") %>' runat="server"
                                                                                            Visible="false"></asp:Label>
                                                                                        <asp:Label ID="lblAdhocDisplayName" Font-Bold="True" Font-Names="Arial" Font-Size="x-small"
                                                                                            Text='<%#Eval("DisplayName") %>' runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <asp:Panel ID="pnlText" runat="server" Visible='<%#Eval("ColumnType").ToString().Contains("varchar") || Eval("ColumnType").ToString().Contains("uniqueidentifier")%>'>
                                                                                        <td align="left" width="25%">
                                                                                            <asp:DropDownList ID="drpAdhocSearch" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                                                Width="100" onchange="drpAdhocSearch_onchange(this);">
                                                                                                <asp:ListItem Selected="true" Value="Contains">CONTAINS</asp:ListItem>
                                                                                                <asp:ListItem Value="Equal">EQUAL</asp:ListItem>
                                                                                                <asp:ListItem Value="Start With">START WITH</asp:ListItem>
                                                                                                <asp:ListItem Value="End With">END WITH</asp:ListItem>
                                                                                                <asp:ListItem Value="Does Not Contain">DOES NOT CONTAIN</asp:ListItem>
                                                                                                <asp:ListItem Value="Is Empty">IS EMPTY</asp:ListItem>
                                                                                                <asp:ListItem Value="Is Not Empty">IS NOT EMPTY</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtAdhocSearchValue" Width="160" runat="server"></asp:TextBox>
                                                                                            <div id="divAdhocRange" runat="server" style="display: none;">
                                                                                                <asp:TextBox ID="txtAdhocRangeFrom" Width="60" runat="server"></asp:TextBox>&nbsp;
                                                                                                <asp:Label ID="lblTo" Text="To" runat="server"></asp:Label>
                                                                                                &nbsp;
                                                                                                <asp:TextBox ID="txtAdhocRangeTo" Width="60" runat="server"></asp:TextBox>
                                                                                            </div>
                                                                                        </td>
                                                                                    </asp:Panel>
                                                                                    <asp:Panel ID="pnlDate" runat="server" Visible='<%#Eval("ColumnType").ToString().Contains("date")%>'>
                                                                                        <td align="left" width="25%">
                                                                                            <asp:DropDownList ID="drpDateRange" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                                                Width="100" onchange="drpDateRange_onchange(this);">
                                                                                                <asp:ListItem Selected="true" Value="DateRange">DATERANGE</asp:ListItem>
                                                                                                <asp:ListItem Value="XDays">LAST X DAYS</asp:ListItem>
                                                                                                <asp:ListItem Value="Year">YEAR</asp:ListItem>
                                                                                                <asp:ListItem Value="Month">MONTH</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <div id="divAdhocDateRange" runat="server" style="display: inline;">
                                                                                                <asp:TextBox ID="txtAdhocDateRangeFrom" Width="65" MaxLength="10" runat="server" ReadOnly="true"></asp:TextBox>
                                                                                                <asp:ImageButton ID="ibChooseFromDate" runat="server" ImageUrl="~/Images/icon-calendar.gif"
                                                                                                    Visible="true" OnCommand="ibChooseDate_Command" ImageAlign="Bottom" />&nbsp;
                                                                                                To&nbsp;
                                                                                                <asp:TextBox ID="txtAdhocDateRangeTo" Width="65" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                                                                <asp:ImageButton ID="ibChooseToDate" runat="server" ImageUrl="~/Images/icon-calendar.gif"
                                                                                                    Visible="true" OnCommand="ibChooseDate_Command" ImageAlign="Bottom" />&nbsp;
                                                                                            </div>
                                                                                            <asp:DropDownList ID="drpAdhocDays" runat="server" Width="100px" Style="display: none"  onchange="drpAdhocDays_onchange(this);"> 
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
                                                                                            <div id="divCustomAdhocDays" runat="Server" style="display: none" >
                                                                                                <asp:TextBox ID="txtCustomAdhocDays" Width="25" CssClass="formfield" MaxLength="3" runat="server"></asp:TextBox>&nbsp;Days
                                                                                                <asp:RequiredFieldValidator ID="rfvCustomAdhocDays" ValidationGroup="Select" Display="Dynamic" ForeColor="red"
                                                                                                    ControlToValidate="txtCustomAdhocDays" runat="server" ErrorMessage="Enter a number between 1 and 730." Enabled ="false" >
                                                                                                </asp:RequiredFieldValidator>
                                                                                                <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Enter a number between 1 and 730." MaximumValue="730"
                                                                                                MinimumValue="0" ControlToValidate="txtCustomAdhocDays" Type="Integer" ForeColor="red" Display="Dynamic" ValidationGroup="Select"></asp:RangeValidator>
                                                                                            </div>
                                                                                            <div id="divAdhocDateYear" runat="server" style="display: none;">
                                                                                                <asp:TextBox ID="txtAdhocDateYearFrom" Width="65" MaxLength="4"
                                                                                                    runat="server"></asp:TextBox>&nbsp;
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtAdhocDateYearFrom"
                                                                                                    FilterType='Numbers' />
                                                                                                &nbsp;To&nbsp;
                                                                                                <asp:TextBox ID="txtAdhocDateYearTo" Width="65" MaxLength="4" runat="server"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtAdhocDateYearTo"
                                                                                                    FilterType="Numbers" />
                                                                                            </div>
                                                                                            <div id="divAdhocDateMonth" runat="server" style="display: none;">
                                                                                                <asp:TextBox ID="txtAdhocDateMonthFrom" Width="65" MaxLength="2"
                                                                                                    runat="server"></asp:TextBox>&nbsp;
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtAdhocDateMonthFrom"
                                                                                                    FilterType='Numbers' />
                                                                                                &nbsp;To&nbsp;
                                                                                                <asp:TextBox ID="txtAdhocDateMonthTo" Width="65" MaxLength="2" runat="server"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtAdhocDateMonthTo"
                                                                                                    FilterType="Numbers" />
                                                                                            </div>
                                                                                        </td>
                                                                                    </asp:Panel>
                                                                                    <asp:Panel ID="PnlBit" runat="server" Visible='<%#Eval("ColumnType").ToString().Contains("bit")%>'>
                                                                                        <td colspan="2" align="left">
                                                                                            <asp:RadioButtonList ID="rblAdhocBit" runat="server" EnableTheming="False" RepeatDirection="Horizontal" CellPadding="3">
                                                                                                <asp:ListItem Value="1">YES</asp:ListItem>
                                                                                                <asp:ListItem Value="0">NO</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                    </asp:Panel>
                                                                                    <asp:Panel ID="pnlInt" runat="server" Visible='<%#((Eval("ColumnType").ToString().Contains("int") || Eval("ColumnType").ToString().Contains("float")) && !Eval("DisplayName").ToString().Contains("PRODUCT COUNT") )%>'>
                                                                                        <td align="left" width="25%">
                                                                                            <asp:DropDownList ID="drpAdhocInt" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                                                Width="100" onchange="drpAdhocInt_onchange(this);">
                                                                                                <asp:ListItem Selected="true" Value="Range">RANGE</asp:ListItem>
                                                                                                <asp:ListItem Value="Equal">EQUAL</asp:ListItem>
                                                                                                <asp:ListItem Value="Greater">GREATER THAN</asp:ListItem>
                                                                                                <asp:ListItem Value="Lesser">LESSER THAN</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtAdhocIntFrom" Width="65" MaxLength="500"
                                                                                                runat="server"></asp:TextBox>&nbsp;
                                                                                            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe1" runat="server" TargetControlID="txtAdhocIntFrom"
                                                                                                FilterType='<%#Eval("ColumnType").ToString().Contains("int") ? FilterTypes.Numbers : FilterTypes.Custom|FilterTypes.Numbers %>'
                                                                                                ValidChars="." />
                                                                                            <asp:Label ID="lblAdhocTo" Text="To" runat="server"></asp:Label>&nbsp;
                                                                                            <asp:TextBox ID="txtAdhocIntTo" Width="65" MaxLength="500" runat="server"></asp:TextBox>
                                                                                            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe2" runat="server" TargetControlID="txtAdhocIntTo"
                                                                                                FilterType='<%#Eval("ColumnType").ToString().Contains("int") ? FilterTypes.Numbers : FilterTypes.Custom|FilterTypes.Numbers %>'
                                                                                                ValidChars="." />
                                                                                        </td>
                                                                                    </asp:Panel>
                                                                                    <asp:Panel ID="pnlProductCount" runat="server" Visible='<%#(Eval("ColumnType").ToString().Contains("int") && Eval("DisplayName").ToString().Contains("PRODUCT COUNT") )%>'>
                                                                                        <td align="left" width="25%">
                                                                                            <asp:DropDownList ID="drpSubscribed" runat="server" Font-Names="Arial" Font-Size="x-small" Width="100">
                                                                                                <asp:ListItem Selected="true" Value="Equal">EQUAL</asp:ListItem>
                                                                                                <asp:ListItem Value="Greater">GREATER THAN</asp:ListItem>
                                                                                                <asp:ListItem Value="Lesser">LESSER THAN</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtPubCount" Width="65" MaxLength="500"
                                                                                                runat="server"></asp:TextBox>
                                                                                        </td>
                                                                                    </asp:Panel>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:DataList>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
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
