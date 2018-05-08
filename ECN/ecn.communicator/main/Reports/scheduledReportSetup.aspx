<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="scheduledReportSetup.aspx.cs" EnableEventValidation="false" Inherits="ecn.communicator.main.Reports.scheduledReportSetup" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Src="~/main/Reports/ReportSettingsControls/AudienceEngagementSettings.ascx" TagName="AudienceEngagement" TagPrefix="ReportSettings" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel='stylesheet' href="../../MasterPages/ECN_MainMenu.css" type="text/css" />
    <link rel='stylesheet' href="../../MasterPages/ECN_Controls.css" type="text/css" />
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .modalPopupContentExplorer {
            background-color: white;
            border-width: 3px;
            border-style: solid;
            border-color: white;
            padding: 3px;
            overflow: auto;
        }

        .overlay {
            position: fixed;
            z-index: 99;
            top: 0px;
            left: 0px;
            background-color: gray;
            width: 100%;
            height: 100%;
            filter: Alpha(Opacity=70);
            opacity: 0.70;
            -moz-opacity: 0.70;
        }

        .loader {
            z-index: 100;
            position: fixed;
            width: 120px;
            margin-left: -60px;
            background-color: #F4F3E1;
            font-size: x-small;
            color: black;
            border: solid 2px Black;
            top: 40%;
            left: 50%;
        }

        .styled-select {
            width: 240px;
            background: transparent;
            height: 28px;
            overflow: hidden;
            border: 1px solid #ccc;
        }

        .styled-text {
            width: 240px;
            height: 28px;
            overflow: hidden;
            border: 1px solid #ccc;
        }

        .reorderStyle {
            list-style-type: disc;
            font: Verdana;
            font-size: 12px;
        }

            .reorderStyle li {
                list-style-type: none;
                padding-bottom: 1em;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div>
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
        <table cellpadding="5" cellspacing="5" border="0">
            <tr>
                <td align="right" class="label">Report :
                </td>
                <td style="text-align: left;">
                    <asp:DropDownList ID="drpReports" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpReports_SelectedIndexChanged" CssClass="styled-text" />
                    
                </td>
                <td align="right" class="label">
                    <asp:Label ID="lblFormat" runat="server" Text="Format:"></asp:Label>
                </td>
                <td style="text-align: left;">
                    <asp:DropDownList ID="dllFormats" runat="server" CssClass="styled-text">
                        <asp:ListItem Value="pdf" Selected="true">PDF [.pdf]</asp:ListItem>
                        <asp:ListItem Value="csv">CSV [.csv]</asp:ListItem>
                        <asp:ListItem Value="xml">XML [.xml]</asp:ListItem>
                        <asp:ListItem Value="xls">EXCEL [.xls]</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td></td>
                <td colspan="3">
                    <asp:UpdatePanel ID="upReportControl" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:PlaceHolder ID="phReportSettings" runat="server"></asp:PlaceHolder>
                        </ContentTemplate>

                    </asp:UpdatePanel>

                </td>
            </tr>
            <tr>
                <td colspan="4" align="left">
                    <asp:Label ID="Label2" runat="server" Text="Envelope" Font-Size="Large"></asp:Label>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td align="right" class="label">From Email :
                </td>
                <td style="text-align: left;">
                    <asp:TextBox ID="txtFromEmail" runat="server" CssClass="styled-text" Text="automatedreports@ecn5.com" Enabled="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFromEmail" runat="server" ControlToValidate="txtFromEmail" ErrorMessage="*" ValidationGroup="schedule" />
                </td>
                <td align="right" class="label">From Name :
                </td>
                <td style="text-align: left;">
                    <asp:TextBox ID="txtFromName" runat="server" CssClass="styled-text"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFromName" runat="server" ControlToValidate="txtFromName" ErrorMessage="*" ValidationGroup="schedule" />
                </td>
            </tr>
            <tr>
                <td align="left">                    
                </td>
                <td colspan="3" align="left" style="font-style: italic;">
                    To ensure delivery of emails to your inbox, please add automatedreports@ecn5.com to your address book or safe sender list
                </td>
            </tr>
            <tr>
                <td align="right" class="label">Subject :
                </td>
                <td style="text-align: left;">
                    <asp:TextBox ID="txtSubject" runat="server" CssClass="styled-text"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" ErrorMessage="*" ValidationGroup="schedule" />
                </td>
                <td align="right" class="label">To Email :
                </td>
                <td style="text-align: left;">
                    <asp:TextBox ID="txtToEmail" runat="server" CssClass="styled-text"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvToEmail" runat="server" ControlToValidate="txtToEmail" ErrorMessage="*" ValidationGroup="schedule" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td style="font-style: italic;">Separate emails with commas when entering a list 
                </td>
            </tr>
            <asp:Panel runat="server" ID="pnlScheduleType" Visible="false">
                <tr>
                    <td align="right" class="label">Schedule Type :
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="ddlScheduleType" runat="server" AutoPostBack="True" CssClass="styled-text" OnSelectedIndexChanged="ddlScheduleType_SelectedIndexChanged" Visible="true">
                            <asp:ListItem Value="One-Time">Schedule One-Time</asp:ListItem>
                            <asp:ListItem Value="Recurring" Selected="True">Schedule Recurring</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </asp:Panel>
            <asp:Panel ID="pnlOneTime" runat="server">
                <tr>
                    <td align="right" class="label">Start Date :</td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="styled-text" MaxLength="10"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvtxtStartDate" runat="server" ControlToValidate="txtStartDate" ValidationGroup="schedule"
                            ErrorMessage="*"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cmpStartDate" runat="server"
                            ControlToValidate="txtStartDate"
                            ErrorMessage="Please enter a valid date"
                            Operator="DataTypeCheck" Type="Date" ValidationGroup="schedule"></asp:CompareValidator>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                            Format="MM/dd/yyyy">
                        </ajaxToolkit:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="label">Start Time :</td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="ddlStartTime" runat="server" CssClass="styled-text">
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
                                <asp:RequiredFieldValidator ID="rfvdrpStartTime" runat="server" ControlToValidate="ddlStartTime" ValidationGroup="schedule"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </asp:Panel>
            <tr>
                <td colspan="4" align="left">
                    <br />
                    <asp:Label ID="Label1" runat="server" Text="Schedule" Font-Size="Large"></asp:Label>
                    <br />
                    <br />
                </td>
            </tr>
            <asp:Panel runat="server" ID="pnlRecurrence" Visible="false">
                <tr>
                    <td align="right" class="label">Recurrence :
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlRecurrence" runat="server" AutoPostBack="True" CssClass="styled-text"
                            OnSelectedIndexChanged="ddlRecurrence_SelectedIndexChanged">
                            <asp:ListItem>Daily</asp:ListItem>
                            <asp:ListItem>Weekly</asp:ListItem>
                            <asp:ListItem>Monthly</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right" class="label">Send Time :</td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="ddlRecurringStartTime" runat="server" CssClass="styled-text">
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
                                <asp:RequiredFieldValidator ID="rfvdrpRecurringStartTime" runat="server" ControlToValidate="ddlRecurringStartTime" ValidationGroup="schedule"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </asp:Panel>
            <asp:Panel ID="pnlRecurring" runat="server" Visible="false">
                <tr>
                    <td align="right" class="label">Start Date :</td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtRecurringStartDate" runat="server" CssClass="styled-text" MaxLength="10"></asp:TextBox>
                        <asp:HiddenField ID="hdnDate" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvtxtRecurringStartDate" runat="server" ControlToValidate="txtRecurringStartDate" ValidationGroup="schedule"
                            ErrorMessage="*"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cmpRecStartDate" runat="server"
                            ControlToValidate="txtRecurringStartDate"
                            ErrorMessage="Please enter a valid date"
                            Operator="DataTypeCheck" Type="Date" ValidationGroup="schedule"></asp:CompareValidator>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtRecurringStartDate"
                            Format="MM/dd/yyyy">
                        </ajaxToolkit:CalendarExtender>

                    </td>
                    <td align="right" class="label">End Date :</td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtRecurringEndDate" runat="server" CssClass="styled-text" MaxLength="10"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtRecurringEndDate"
                            Format="MM/dd/yyyy">
                        </ajaxToolkit:CalendarExtender>
                        <asp:RequiredFieldValidator ID="rfvtxtRecurringEndDate" runat="server" ControlToValidate="txtRecurringEndDate" ValidationGroup="schedule"
                            ErrorMessage="*"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cmpRecEndDate" runat="server"
                            ErrorMessage="Please enter a valid date"
                            ControlToValidate="txtRecurringEndDate"
                            Operator="DataTypeCheck" Type="Date" ValidationGroup="schedule"></asp:CompareValidator>

                    </td>
                </tr>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlDays" Visible="false">
                <tr>
                    <td align="right" class="label">Day :</td>
                    <td colspan="3">
                        <asp:RadioButtonList ID="cbDays" runat="server" RepeatDirection="Horizontal" CssClass="label" Width="100%">
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
                    <td colspan="3" style="text-align: left;">
                        <asp:DropDownList ID="drpDayofMonth" runat="server" CssClass="styled-select">
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                            <asp:ListItem>17</asp:ListItem>
                            <asp:ListItem>18</asp:ListItem>
                            <asp:ListItem>19</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>21</asp:ListItem>
                            <asp:ListItem>22</asp:ListItem>
                            <asp:ListItem>23</asp:ListItem>
                            <asp:ListItem>24</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                            <asp:ListItem>26</asp:ListItem>
                            <asp:ListItem>27</asp:ListItem>
                            <asp:ListItem>28</asp:ListItem>
                            <asp:ListItem>Last Day</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </asp:Panel>
            <tr>
                <td colspan="4" align="center">
                    <br />
                    <br />
                    <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="schedule" CausesValidation="true" OnClick="btnSave_Click" class="ECN-Button-Medium" />

                </td>
            </tr>
        </table>
    </div>
</asp:Content>
