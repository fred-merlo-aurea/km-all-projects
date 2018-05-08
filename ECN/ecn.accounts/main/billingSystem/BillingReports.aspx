<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.Master" AutoEventWireup="true" CodeBehind="BillingReports.aspx.cs" Inherits="ecn.accounts.main.billingSystem.BillingReports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .modalPopupFull {
            background-color: #D0D0D0;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
            width: 100%;
            height: 100%;
            overflow: auto;
        }

        .modalPopup {
            background-color: #D0D0D0;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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
                                <img style="padding: 0 0 0 15px;"
                                    src="/ecn.images/images/errorEx.jpg"></td>
                            <td valign="middle" align="left" width="80%" height="100%">
                                <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td id="errorBottom"></td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <asp:Panel ID="pnlReportList" runat="server">
        <table class="tableContent" style="width: 100%;">
            <tr>
                <td style="text-align: right; width: 100%;">
                    <asp:Button ID="btnAddNewReport" runat="server" OnClick="btnAddNewReport_Click" Text="Create Report" />

                </td>
            </tr>
            <tr>
                <td style="width: 100%; text-align: center;">
                    <asp:Label ID="lblNoReports" runat="server" Text="There are no saved reports.  Please create one." />
                    <ecnCustom:ecnGridView ID="gvBillingReports" CssClass="ECN-GridView" Width="70%" runat="server" OnRowDataBound="gvBillingReports_RowDataBound" OnRowCommand="gvBillingReports_RowCommand" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="BillingReportName" ItemStyle-Width="80%" HeaderText="Name" />
                            <asp:TemplateField ItemStyle-Width="10%" HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnEditReport" runat="server" CommandName="EditReport" ImageUrl="/ecn.images/images/icon-edits1.gif" />
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="10%" HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnDeleteReport" runat="server" CommandName="DeleteReport" ImageUrl="/ecn.images/images/icon-delete1.gif" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </ecnCustom:ecnGridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlEditReport" Visible="false" runat="server">
        <table class="tableContent" width="100%">
            <tr>
                <td colspan="2">
                    <table style="width: 100%;">
                        <tr>
                            <td style="text-align: right; width: 40%;">
                                <asp:Label ID="lblReportName" runat="server" Text="Report Name" Font-Bold="true" Font-Size="Medium" />
                            </td>
                            <td style="text-align: left; width: 60%;">
                                <asp:TextBox ID="txtReportName" Width="300px" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvReportName" ControlToValidate="txtReportName" ValidationGroup="SaveReport" runat="server" ErrorMessage="*" ForeColor="Red" />
                            </td>
                        </tr>
                    </table>
                </td>

            </tr>
            <tr>
                <td style="width: 50%;">
                    <table style="width: 100%;">
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblBaseChannel" Text="Base Channel:" Font-Bold="true" runat="server" />
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlBaseChannel" AutoPostBack="true" OnSelectedIndexChanged="ddlBaseChannel_SelectedIndexChanged" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 50%;">
                    <asp:RadioButtonList ID="rblCustomer" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblCustomer_SelectedIndexChanged" RepeatDirection="Horizontal">
                        <asp:ListItem Text="All Customers" Value="all" Selected="True" />
                        <asp:ListItem Text="Select Customers" Value="select" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:ListBox ID="lstbxCustomers" runat="server" Visible="false" Height="150px" SelectionMode="Multiple" AutoPostBack="false" />
                </td>
            </tr>
            <tr>
                <td style="width: 50%; text-align: center;">
                    <asp:Label ID="lblBlastColumns" runat="server" Text="Blast Columns" Font-Bold="true" />
                </td>
                <td colspan="2" rowspan="2" style="width: 50%; text-align: center; vertical-align: top;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblAdditional" runat="server" Font-Bold="true" Text="Flat Rate Items" />
                            </td>
                            <td>
                                <asp:Button ID="btnAddFlatItem" runat="server" CausesValidation="false" OnClick="btnAddFlatItem_Click" Text="Add" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="gvFlatRateItems" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="ECN-GridView" OnRowCommand="gvFlatRateItems_RowCommand" OnRowDataBound="gvFlatRateItems_RowDataBound" runat="server">
                                    <Columns>
                                        <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                                        <asp:BoundField DataField="Amount" HeaderText="Amount" />
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnEdit" ImageUrl="http://images.ecn5.com/images/icon-edits1.gif" CommandName="editflatrate" runat="server" ToolTip="Edit" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnDelete" ImageUrl="http://images.ecn5.com/images/icon-delete1.gif" CommandName="deleteflatrate" runat="server" ToolTip="Delete" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:HiddenField ID="hfIsEdit" runat="server" Value="" />
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 50%; text-align: center;">
                    <asp:ListBox ID="lstbxBlastColumns" runat="server" SelectionMode="Multiple" AutoPostBack="false">
                        <asp:ListItem Text="BlastField1" Value="blastfield1" />
                        <asp:ListItem Text="BlastField2" Value="blastfield2" />
                        <asp:ListItem Text="BlastField3" Value="blastfield3" />
                        <asp:ListItem Text="BlastField4" Value="blastfield4" />
                        <asp:ListItem Text="BlastField5" Value="blastfield5" />
                        <asp:ListItem Text="From Email" Value="fromemail" />
                        <asp:ListItem Text="From Name" Value="fromname" />
                        <asp:ListItem Text="Email Subject" Value="emailsubject" />
                        <asp:ListItem Text="Group Name" Value="groupname" />
                    </asp:ListBox>
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <table style="width: 100%; margin-top: 20px;">
                        <tr>
                            <td style="text-align: center;">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblRunFromDate" Text="From:" runat="server" />
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtRunFromDate" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfvRunFromDate" ControlToValidate="txtRunFromDate" ValidationGroup="RunNow" runat="server" ErrorMessage="*" />
                                            <ajaxToolkit:CalendarExtender ID="ceRunFromDate" TargetControlID="txtRunFromDate" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblRunToDate" Text="To:" runat="server" />
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtRunToDate" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfvRunToDate" ControlToValidate="txtRunToDate" ValidationGroup="RunNow" runat="server" ErrorMessage="*" />
                                            <ajaxToolkit:CalendarExtender ID="ceRunToDate" TargetControlID="txtRunToDate" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:Button ID="btnRunReport" Text="Run Report" runat="server" ValidationGroup="RunNow" CausesValidation="true" OnClick="btnRunReport_Click" />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Button ID="btnSaveReport" Text="Save Report" OnClick="btnSaveReport_Click" ValidationGroup="SaveReport" CausesValidation="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>

                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
        </table>


        <asp:Button ID="hfFlatRate" Style="visibility: hidden;" runat="server" />
        <ajaxToolkit:ModalPopupExtender ID="modalPopupFlatRate" runat="server" BackgroundCssClass="modalBackground"
            PopupControlID="upFlatRateItem" CancelControlID="btnCancelFlatRate" TargetControlID="hfFlatRate">
        </ajaxToolkit:ModalPopupExtender>
        <asp:UpdatePanel ID="upFlatRateItem" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSaveFlatRateItem" />
                <asp:PostBackTrigger ControlID="btnCancelFlatRate" />
            </Triggers>
            <ContentTemplate>
                <table style="background-color: #CCCCCC;">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblFlatRatePopup" runat="server" Text="Flat Rate Item" Font-Bold="true" Font-Size="Medium" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblItemName" runat="server" Text="Item:" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemName" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvItemName" runat="server" ErrorMessage="*" ControlToValidate="txtItemName" ValidationGroup="FlatRate" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblItemRate" runat="server" Text="Amount:" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemRate" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvItemRate" runat="server" ErrorMessage="*" ControlToValidate="txtItemRate" ValidationGroup="FlatRate" />
                            <ajaxToolkit:FilteredTextBoxExtender ID="ftbeItemRate" ValidChars="0123456789." TargetControlID="txtItemRate" FilterMode="ValidChars" FilterType="Custom" Enabled="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Button ID="btnSaveFlatRateItem" runat="server" ValidationGroup="FlatRate" CausesValidation="true" Text="Save" OnClick="btnSaveFlatRateItem_Click" />
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Button ID="btnCancelFlatRate" runat="server" CausesValidation="false" OnClick="btnCancelFlatRate_Click" UseSubmitBehavior="true" Text="Cancel" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:Button ID="hfSchedule" Style="visibility: hidden;" runat="server" />
        <ajaxToolkit:ModalPopupExtender ID="modalPopupSchedule" runat="server" BackgroundCssClass="modalBackground" PopupControlID="pnlSchedule" CancelControlID="btnCancelSchedule" TargetControlID="hfSchedule" />
        <asp:Panel ID="pnlSchedule" runat="server" CssClass="modalPopup">
            <asp:UpdatePanel ID="upSchedule" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <table style="background-color: #CCCCCC;">
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblSchedule" runat="server" Text="Schedule Report" Font-Bold="true" Font-Size="Medium" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFromEmail" runat="server" Text="From Email:" Font-Bold="true" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFromEmail" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfvFromEmail" runat="server" ErrorMessage="*" ControlToValidate="txtFromEmail" ValidationGroup="Export" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFromName" runat="server" Text="From Name:" Font-Bold="true" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFromName" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfvFromName" ErrorMessage="*" runat="server" ControlToValidate="txtFromName" ValidationGroup="Export" />
                                        </td>
                                    </tr>

                                </table>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblToEmail" runat="server" Text="To Email:" Font-Bold="true" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToEmail" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfvToEmail" ErrorMessage="*" runat="server" ControlToValidate="txtToEmail" ValidationGroup="Export" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSubject" runat="server" Text="Email Subject:" Font-Bold="true" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSubject" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfbSubject" ErrorMessage="*" runat="server" ControlToValidate="txtSubject" ValidationGroup="Export" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td align="right" class="label">Schedule Type :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlScheduleType" runat="server" AutoPostBack="True" Width="200px" OnSelectedIndexChanged="ddlScheduleType_SelectedIndexChanged">
                                                <asp:ListItem Value="One-Time">Schedule One-Time</asp:ListItem>
                                                <asp:ListItem Value="Recurring">Schedule Recurring</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <asp:Panel ID="pnlOneTime" runat="server">
                                        <tr>
                                            <td align="right" class="label">Start Date :</td>
                                            <td>
                                                <asp:TextBox ID="txtStartDate" runat="server" Width="200px" MaxLength="10"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtStartDate" runat="server" ControlToValidate="txtStartDate" ValidationGroup="Export"
                                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="cmpStartDate" runat="server"
                                                    ControlToValidate="txtStartDate"
                                                    ErrorMessage="Please enter a valid date"
                                                    Operator="DataTypeCheck" Type="Date" ValidationGroup="Export"></asp:CompareValidator>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                                    Format="MM/dd/yyyy"></ajaxToolkit:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="label">Start Time :</td>
                                            <td>
                                                <asp:DropDownList ID="ddlStartTime" runat="server" Width="200px">
                                                    <asp:ListItem Value="0:00:00">0:00:00</asp:ListItem>
                                                    <asp:ListItem Value="1:00:00">1:00:00</asp:ListItem>
                                                    <asp:ListItem Value="2:00:00">2:00:00</asp:ListItem>
                                                    <asp:ListItem Value="3:00:00">3:00:00</asp:ListItem>
                                                    <asp:ListItem Value="4:00:00">4:00:00</asp:ListItem>
                                                    <asp:ListItem Value="5:00:00">5:00:00</asp:ListItem>
                                                    <asp:ListItem Value="6:00:00">6:00:00</asp:ListItem>
                                                    <asp:ListItem Value="7:00:00">7:00:00</asp:ListItem>
                                                    <asp:ListItem Value="8:00:00">8:00:00</asp:ListItem>
                                                    <asp:ListItem Value="9:00:00">9:00:00</asp:ListItem>
                                                    <asp:ListItem Value="10:00:00">10:00:00</asp:ListItem>
                                                    <asp:ListItem Value="11:00:00">11:00:00</asp:ListItem>
                                                    <asp:ListItem Value="12:00:00">12:00:00</asp:ListItem>
                                                    <asp:ListItem Value="13:00:00">13:00:00</asp:ListItem>
                                                    <asp:ListItem Value="14:00:00">14:00:00</asp:ListItem>
                                                    <asp:ListItem Value="15:00:00">15:00:00</asp:ListItem>
                                                    <asp:ListItem Value="16:00:00">16:00:00</asp:ListItem>
                                                    <asp:ListItem Value="17:00:00">17:00:00</asp:ListItem>
                                                    <asp:ListItem Value="18:00:00">18:00:00</asp:ListItem>
                                                    <asp:ListItem Value="19:00:00">19:00:00</asp:ListItem>
                                                    <asp:ListItem Value="20:00:00">20:00:00</asp:ListItem>
                                                    <asp:ListItem Value="21:00:00">21:00:00</asp:ListItem>
                                                    <asp:ListItem Value="22:00:00">22:00:00</asp:ListItem>
                                                    <asp:ListItem Value="23:00:00">23:00:00</asp:ListItem>
                                                </asp:DropDownList>&nbsp;&nbsp;CST
                                <asp:RequiredFieldValidator ID="rfvdrpStartTime" runat="server" ControlToValidate="ddlStartTime" ValidationGroup="Export"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="pnlRecurrence" Visible="false">
                                        <tr>
                                            <td align="right" class="label">Recurrence :
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlRecurrence" runat="server" AutoPostBack="True" Width="200px"
                                                    OnSelectedIndexChanged="ddlRecurrence_SelectedIndexChanged">
                                                    <asp:ListItem>Monthly</asp:ListItem>
                                                    <asp:ListItem>Quarterly</asp:ListItem>
                                                    <asp:ListItem>Yearly</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlRecurring" runat="server" Visible="false">
                                        <tr>
                                            <td align="right" class="label">Start Date :</td>
                                            <td>
                                                <asp:TextBox ID="txtRecurringStartDate" runat="server" Width="200px" MaxLength="10"></asp:TextBox>
                                                <asp:HiddenField ID="hdnDate" runat="server" />
                                                <asp:RequiredFieldValidator ID="rfvtxtRecurringStartDate" runat="server" ControlToValidate="txtRecurringStartDate" ValidationGroup="Export"
                                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="cmpRecStartDate" runat="server"
                                                    ControlToValidate="txtRecurringStartDate"
                                                    ErrorMessage="Please enter a valid date"
                                                    Operator="DataTypeCheck" Type="Date" ValidationGroup="Export"></asp:CompareValidator>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtRecurringStartDate"
                                                    Format="MM/dd/yyyy"></ajaxToolkit:CalendarExtender>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="label">Start Time :</td>
                                            <td>
                                                <asp:DropDownList ID="ddlRecurringStartTime" runat="server" Width="200px">
                                                    <asp:ListItem Value="0:00:00">0:00:00</asp:ListItem>
                                                    <asp:ListItem Value="1:00:00">1:00:00</asp:ListItem>
                                                    <asp:ListItem Value="2:00:00">2:00:00</asp:ListItem>
                                                    <asp:ListItem Value="3:00:00">3:00:00</asp:ListItem>
                                                    <asp:ListItem Value="4:00:00">4:00:00</asp:ListItem>
                                                    <asp:ListItem Value="5:00:00">5:00:00</asp:ListItem>
                                                    <asp:ListItem Value="6:00:00">6:00:00</asp:ListItem>
                                                    <asp:ListItem Value="7:00:00">7:00:00</asp:ListItem>
                                                    <asp:ListItem Value="8:00:00">8:00:00</asp:ListItem>
                                                    <asp:ListItem Value="9:00:00">9:00:00</asp:ListItem>
                                                    <asp:ListItem Value="10:00:00">10:00:00</asp:ListItem>
                                                    <asp:ListItem Value="11:00:00">11:00:00</asp:ListItem>
                                                    <asp:ListItem Value="12:00:00">12:00:00</asp:ListItem>
                                                    <asp:ListItem Value="13:00:00">13:00:00</asp:ListItem>
                                                    <asp:ListItem Value="14:00:00">14:00:00</asp:ListItem>
                                                    <asp:ListItem Value="15:00:00">15:00:00</asp:ListItem>
                                                    <asp:ListItem Value="16:00:00">16:00:00</asp:ListItem>
                                                    <asp:ListItem Value="17:00:00">17:00:00</asp:ListItem>
                                                    <asp:ListItem Value="18:00:00">18:00:00</asp:ListItem>
                                                    <asp:ListItem Value="19:00:00">19:00:00</asp:ListItem>
                                                    <asp:ListItem Value="20:00:00">20:00:00</asp:ListItem>
                                                    <asp:ListItem Value="21:00:00">21:00:00</asp:ListItem>
                                                    <asp:ListItem Value="22:00:00">22:00:00</asp:ListItem>
                                                    <asp:ListItem Value="23:00:00">23:00:00</asp:ListItem>
                                                </asp:DropDownList>&nbsp;&nbsp;CST
                                <asp:RequiredFieldValidator ID="rfvdrpRecurringStartTime" runat="server" ControlToValidate="ddlRecurringStartTime" ValidationGroup="Export"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="label">End Date :</td>
                                            <td>
                                                <asp:TextBox ID="txtRecurringEndDate" runat="server" Width="200px" MaxLength="10"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtRecurringEndDate"
                                                    Format="MM/dd/yyyy"></ajaxToolkit:CalendarExtender>
                                                <asp:CompareValidator ID="cmpRecEndDate" runat="server"
                                                    ErrorMessage="Please enter a valid date"
                                                    ControlToValidate="txtRecurringEndDate"
                                                    Operator="DataTypeCheck" Type="Date" ValidationGroup="Export"></asp:CompareValidator>

                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="pnlDays" Visible="false">
                                        <tr>
                                            <td align="right" class="label">Days :</td>
                                            <td>
                                                <asp:RadioButtonList ID="cbDays" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                                                    <asp:ListItem Value="Monday">Monday</asp:ListItem>
                                                    <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                                                    <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                                                    <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                                                    <asp:ListItem Value="Friday">Friday</asp:ListItem>
                                                    <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="pnlMonth" Visible="false">
                                        <tr>
                                            <td align="right" class="label">Day</td>
                                            <td class="label">
                                                <asp:TextBox ID="txtMonth" runat="server"></asp:TextBox>
                                                <asp:RangeValidator ID="rvMonth" runat="server"
                                                    ControlToValidate="txtMonth" MinimumValue="1" Type="Integer" MaximumValue="28" ValidationGroup="Export"
                                                    CssClass="errormsg" Display="Dynamic">&laquo;&laquo Invalid Value></asp:RangeValidator>
                                                <asp:CheckBox ID="cbLastDay" runat="server" Text=" Last Day" />
                                            </td>
                                        </tr>
                                    </asp:Panel>

                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSaveSchedule" runat="server" OnClick="btnSaveSchedule_Click" CausesValidation="true" ValidationGroup="Export" Text="Save" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCancelSchedule" runat="server" CausesValidation="false" UseSubmitBehavior="true" Text="Cancel" />
                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
