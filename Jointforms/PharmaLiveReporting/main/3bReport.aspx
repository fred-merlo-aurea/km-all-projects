<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="3bReport.aspx.cs" Inherits="PharmaLiveReporting.main._bReport" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<%@ Register TagPrefix="KM" TagName="header" Src="../Controls/Header.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>PharmaLive Reports</title>
    <link rel="stylesheet" href="http://www.ecn5.com/ecn.accounts/images/stylesheet.css"
        type="text/css" />
    <link rel="stylesheet" href="../stylesheet.css" type="text/css" />
    <link rel="stylesheet" href="http://www.ecn5.com/ecn.accounts/images/stylesheet_default.css"
        type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
</head>
<body topmargin="0" leftmargin="0" bottommargin="0" rightmargin="0">
    <form runat="server" id="Form1">
        <KM:header ID="pageheader" runat="server"></KM:header>
        <div style="padding: 10px 10px 10px 10px;">
            <br />
            <table style="border-right: #dde4e8 2px solid; border-top: #dde4e8 2px solid; border-left: #dde4e8 2px solid;
                border-bottom: #dde4e8 2px solid" cellspacing="0" cellpadding="0" width="98%"
                border="0">
                <tr class="gridheader" style="padding-right: 4px; padding-left: 8px; padding-bottom: 4px;
                    width: 100%; padding-top: 4px">
                    <td align="left">
                        <span style="font-weight: bold; font-size: 8pt; color: #000000; font-style: normal;
                            font-family: Arial">3b Report Parameters</span></td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%" cellspacing="0" cellpadding="5" border="0">
                            <tr>
                                <td align="left" width="15%" valign="top">
                                    Group:<br />
                                    <br />
                                    <asp:ListBox ID="lstGroups" runat="server" SelectionMode="Single" DataTextField="GroupName"
                                        DataValueField="GroupID" Width="250px" Rows="8" Font-Names="Arial" Font-Size="x-small">
                                    </asp:ListBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="lstGroups"
                                        ErrorMessage="<< required"></asp:RequiredFieldValidator></td>
                                <td align="left" width="15%" valign="top">
                                    Date:(From - To)<br />
                                    <br />
                                    <asp:TextBox ID="txtQFrom" Width="100" CssClass="formfield" MaxLength="10" runat="server"></asp:TextBox><img
                                        onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('txtQFrom'),document.getElementById('txtQFrom')); return false"
                                        src="http://images.ecn5.com/Images/icon-calendar.gif" align="absMiddle">&nbsp;<asp:RangeValidator
                                            ID="RangeValidator1" runat="server" ControlToValidate="txtQFrom" ErrorMessage="<< Invalid Date"
                                            MaximumValue="12/31/2050" MinimumValue="01/01/1950" Type="Date"></asp:RangeValidator><br /><br />
                                    <asp:TextBox ID="txtQTo" Width="100" runat="server" CssClass="formfield" MaxLength="10"></asp:TextBox><img
                                        onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('txtQTo'),document.getElementById('txtQTo')); return false"
                                        src="http://images.ecn5.com/Images/icon-calendar.gif" align="absMiddle">&nbsp;<asp:RangeValidator
                                            ID="RangeValidator2" runat="server" ControlToValidate="txtQTo" ErrorMessage="<< Invalid Date"
                                            MaximumValue="12/31/2050" MinimumValue="01/01/1950" Type="Date"></asp:RangeValidator>
                                    &nbsp;&nbsp;
                                    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; visibility: visible;
                                        position: absolute; top: -500px" name="gToday:normal:agenda.js" src="../scripts/ipopeng.htm"
                                        frameborder="0" width="174" scrolling="no" height="189"></iframe>
                                </td>
                                <td align="center" width="20%" valign="middle">
                                    <asp:Button ID="btnReload" Text="Apply Filters" runat="server" CssClass="button"
                                        OnClick="btnReload_Click"></asp:Button>&nbsp;<br />
                                    <br />
                                    &nbsp;<asp:Button ID="btnReset" Text="Reset Filters" runat="server" CssClass="button"
                                        OnClick="btnReset_Click"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" align="left" colspan="3">
                        <table style="border-right: #dde4e8 2px solid; border-top: #dde4e8 2px solid; border-left: #dde4e8 2px solid;
                            border-bottom: #dde4e8 2px solid" width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr class="gridheader">
                                <td align="left">
                                    <span style="font-weight: bold; font-size: 8pt; color: #000000; font-style: normal;
                                        font-family: Arial">Report</span></td>
                                <td align="right">
                                    <asp:DropDownList ID="drpExport" Width="100" runat="server">
                                        <asp:ListItem Selected="true" Value="pdf">PDF</asp:ListItem>
                                        <asp:ListItem Value="xls">Excel</asp:ListItem>
                                        <asp:ListItem Value="doc">Word</asp:ListItem>
                                    </asp:DropDownList>&nbsp;<asp:Button ID="btnDownload" runat="server" Text="Download"
                                        OnClick="btnDownload_Click"></asp:Button>&nbsp;&nbsp;
                                    <asp:Button ID="btndownloaddetails" runat="server" Text="Download Details" OnClick="btndownloaddetails_Click" Visible=false>
                                    </asp:Button>&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <br />
                                    <cr:CrystalReportViewer ID="crv" runat="server" Width="100%" SeparatePages="False"
                                        DisplayGroupTree="False" EnableViewState="false" EnableDrillDown="False" DisplayToolbar="False">
                                    </cr:CrystalReportViewer>
                                </td>
                            </tr>
                        </table>
                        <br>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
