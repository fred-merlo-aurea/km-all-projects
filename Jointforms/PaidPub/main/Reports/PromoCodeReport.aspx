<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="PromoCodeReport.aspx.cs"
    Inherits="PaidPub.main.Reports.PromoCodeReport" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div class="contentheader">
        Report
    </div>
    <br />
    <div class="padding20">
        <div class="box">
            <div class="boxheader">
                PromoCode Report</div>
            <div class="boxcontent">
                <table cellpadding="5" cellspacing="0" border="0">
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left" valign="top">
                            NewsLetters :
                        </td>
                        <td width="50%" align="left">
                            <asp:DropDownList ID="drpNewsletters" runat="server" Width="400">
                            </asp:DropDownList>
                        </td>
                        <td width="20%" align="left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="Server" Font-Size="xx-small"
                                ControlToValidate="drpNewsletters" ErrorMessage="&laquo; required" Font-Italic="True"
                                Font-Bold="True" InitialValue=""></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left" valign="top">
                            Promotion Codes :
                        </td>
                        <td width="50%" align="left">
                            <asp:ListBox ID="lstPromoCodes" runat="server" Width="400" SelectionMode="Multiple"
                                Rows="6"></asp:ListBox>
                        </td>
                        <td width="20%" align="left">
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left" valign="top">
                            Start Date&nbsp;:&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtstartDate" runat="Server" Width="80"></asp:TextBox>
                            &nbsp;<img alt="" onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('ctl00_Content_txtstartDate'),document.getElementById('ctl00_Content_txtstartDate')); return false"
                                src="../../images/icon-calendar.gif" align="middle" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="Server" Font-Size="xx-small" ControlToValidate="txtstartDate"
                                ErrorMessage="&laquo; required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left" valign="top">
                            End Date&nbsp;:&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtendDate" runat="Server" Width="80"></asp:TextBox>
                            &nbsp;<img alt="" onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('ctl00_Content_txtendDate'),document.getElementById('ctl00_Content_txtendDate')); return false"
                                src="../../images/icon-calendar.gif" align="middle" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfv2" runat="Server" Font-Size="xx-small" ControlToValidate="txtendDate"
                                ErrorMessage="&laquo; required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left" valign="top">
                            Export Type&nbsp;:&nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="drpExport" Width="100" runat="server">
                                <asp:ListItem Selected="true" Value="html">HTML</asp:ListItem>
                                <asp:ListItem Value="pdf">PDF</asp:ListItem>
                                <asp:ListItem Value="xls">Excel</asp:ListItem>
                                <asp:ListItem Value="doc">Word</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left">
                            <br />
                            <asp:Label ID="lblErrorMessage" runat="server" Visible="false" CssClass="error"></asp:Label>
                            <br />
                            <asp:Button CssClass="blackButton" ID="btnReport" runat="server" Text="Generate Report"
                                OnClick="btnReport_Click"></asp:Button>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left">
                            <cr:CrystalReportViewer ID="crv" runat="server" Width="100%" SeparatePages="False"
                                DisplayGroupTree="False" EnableViewState="false" EnableDrillDown="False" DisplayToolbar="False">
                            </cr:CrystalReportViewer>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; visibility: visible;
        position: absolute; top: -500px" name="gToday:normal:agenda.js" src="../../scripts/ipopeng.htm"
        frameborder='0' width="174" scrolling="no" height="189"></iframe>
</asp:Content>
