<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="StatusReport.aspx.cs" Inherits="PaidPub.main.Reports.StatusReport" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div class="contentheader">
        Report
    </div>
    <br />
    <div class="padding20">
        <div class="box">
            <div class="boxheader">
                Subscriber Status Report</div>
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
                            <asp:DropDownList ID="drpNewsletters" runat="server" Width="300">
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
                            Month&nbsp;:&nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="drpMonth" runat="server">
                            <asp:ListItem value="1">January</asp:ListItem>
											<asp:ListItem value="2">February</asp:ListItem>
											<asp:ListItem value="3">March</asp:ListItem>
											<asp:ListItem value="4">April</asp:ListItem>
											<asp:ListItem value="5">May</asp:ListItem>
											<asp:ListItem value="6">June</asp:ListItem>
											<asp:ListItem value="7">July</asp:ListItem>
											<asp:ListItem value="8">August</asp:ListItem>
											<asp:ListItem value="9">September</asp:ListItem>
											<asp:ListItem value="10">October</asp:ListItem>
											<asp:ListItem value="11">November</asp:ListItem>
											<asp:ListItem value="12">December</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left" valign="top">
                            Year&nbsp;:&nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="drpYear" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                     <tr>
                        <td width="20%" align="left" valign="top">
                            Report Type&nbsp;:&nbsp;
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbReportType" runat="server" RepeatDirection=Horizontal>
                                <asp:ListItem Text="Summary" Value="S" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Detail" Value="D"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            &nbsp;
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
        position: absolute; top: -500px" name="gToday:normal:agenda.js" src="/Paidpub/scripts/ipopeng.htm"
        frameborder='0' width="174" scrolling="no" height="189"></iframe>
</asp:Content>

