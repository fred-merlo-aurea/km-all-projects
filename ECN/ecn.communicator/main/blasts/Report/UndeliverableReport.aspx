<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UndeliverableReport.aspx.cs"
    Inherits="ecn.communicator.main.blasts.reports.UndeliverableReport" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    	 <style type='text/css'>
        .ui-datepicker-trigger { position: relative; vertical-align:middle; padding-left:5px; }
    </style>
	
	<script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtstartDate.ClientID%>").datepicker({
                showOn: "button",
                buttonImage: "/ecn.images/images/icon-calendar.gif",
                buttonImageOnly: true,
                buttonText: "Select date",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true
            });
            $("#<%=txtendDate.ClientID%>").datepicker({
                showOn: "button",
                buttonImage: "/ecn.images/images/icon-calendar.gif",
                buttonImageOnly: true,
                buttonText: "Select date",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true
            });
        });
    </script>

    <br />
    <table id="idMain" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr class="gradient">
                        <td width="50%" valign="middle" style="border-right: medium none; padding-right: 5px;
                            border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif;
                            border-left: #a4a2a3 1px solid; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid">
                            &nbsp;Undeliverable Report
                        </td>
                        <td width="50%" align='right' valign="middle" style="border-right: #a4a2a3 1px solid;
                            padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px;
                            font: bold 13px Arial, Helvetica, sans-serif; border-left: medium none; color: #333;
                            padding-top: 0px; border-bottom: #a4a2a3 1px solid">
                            Download as:&nbsp;<asp:DropDownList ID="drpExport" CssClass="formlabel" runat="Server">
                                <asp:ListItem Value="xls">XLS</asp:ListItem>
                                <asp:ListItem Value="xlsdata">XLSDATA</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="offWhite borderSides" valign="top" colspan="2">
                            <table cellspacing="2" cellpadding="5" border='0' width="100%" class="formLabel"
                                style="margin: 10px 0px">
                                <tr>
                                    <td width="30%" align='right'>
                                        <b>Start Date&nbsp;:&nbsp;</b>
                                    </td>
                                    <td width="70%" align='left'>
                                        <asp:TextBox ID="txtstartDate" runat="Server" Width="80" CssClass="formfield"></asp:TextBox>
                                        &nbsp;
                                        <asp:RequiredFieldValidator
                                                ID="rfv1" runat="Server" Font-Size="xx-small" ControlToValidate="txtstartDate"
                                                ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right'>
                                        <b>End Date&nbsp;:&nbsp;</b>
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox ID="txtendDate" runat="Server" Width="80" CssClass="formfield"></asp:TextBox>
                                        &nbsp;
                                        <asp:RequiredFieldValidator
                                                ID="rfv2" runat="Server" Font-Size="xx-small" ControlToValidate="txtendDate"
                                                ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                                        <asp:CustomValidator ID="EndDateValidator" runat="server" ControlToValidate="txtendDate" Display="Dynamic" 
                                            Font-Size="xx-small" Font-Italic="True" Font-Bold="True" OnServerValidate="EndDateValidator_ServerValidate"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right'>
                                         <b>Undeliverable Type&nbsp;:&nbsp;</b>
                                    </td>
                                    <td  align='left'>
                                        <asp:DropDownList ID="drpUndeliverableType" CssClass="formfield" runat="Server">
                                            <asp:ListItem Value="All" Selected="true">All</asp:ListItem>
                                            <asp:ListItem Value="HardBounces">Hard Bounces</asp:ListItem>
                                            <asp:ListItem Value="SoftBounces">Soft Bounces</asp:ListItem>
                                            <asp:ListItem Value="MailBoxFull">Mail Box Full</asp:ListItem>
                                            <asp:ListItem Value="Unsubscribes">Unsubscribes</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="offWhite borderSides" colspan="2" align="center">
                            <asp:Button ID="btnReport" runat="Server" Text="Show Report" CssClass="formfield"
                                OnClick="btnReport_Click"></asp:Button>
                            <br>
                            <br>
                        </td>
                    </tr>
                    <tr>
                        <td class="offWhite borderSides" colspan="2" align="center">
                            <rsweb:ReportViewer ID="ReportViewer1" Width="100%" runat="server" Visible="False"  ShowRefreshButton="false">
                            </rsweb:ReportViewer>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="gradient" colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
