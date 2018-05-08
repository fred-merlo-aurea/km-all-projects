<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BlastDeliveryReport.aspx.cs"
    Inherits="ecn.communicator.main.blasts.reports.BlastDeliveryReport" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type='text/css'>
        .ui-datepicker-trigger {
            position: relative;
            vertical-align: middle;
            padding-left: 5px;
        }
        .auto-style1 {
            width: 45%;
        }
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
        <br />
    </asp:PlaceHolder>
    <br />
    <table id="idMain" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr class="gradient">
                        <td width="50%" valign="middle" style="border-right: medium none; padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; border-left: #a4a2a3 1px solid; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid">&nbsp;Blast Delivery Report
                        </td>
                        <td width="50%" align='right' valign="middle" style="border-right: #a4a2a3 1px solid; padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; border-left: medium none; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid">Download as:&nbsp;<asp:DropDownList ID="drpExport" CssClass="formlabel" runat="Server">
                            <asp:ListItem Value="pdf">PDF</asp:ListItem>
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
                                    <td colspan ="3" align ="right">
                                        <font class="formLabel" style="color: black; ">Downloading this report as a PDF file can be slow or even cause a timeout.<br />
                                        For quicker downloading, please choose XLS or XLSDATA file type.<br />
                                        If possible, for PDF files, please use the Blast Delivery Report under Scheduled Reports.<br /><br /><br /></font>
                                    </td>
                                    </tr>
                               <tr>
                                    <td colspan="2" style="width: 100%; text-align: center;">
                                        <asp:ListBox ID="lstboxCustomer" runat="server" Visible="false" SelectionMode="Multiple"></asp:ListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right' class="auto-style1">
                                        <b>Start Date&nbsp;:&nbsp;</b>
                                    </td>
                                    <td width="70%" align='left'>
                                        <asp:TextBox ID="txtstartDate" runat="Server" Width="80" CssClass="formfield"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator
                                            ID="rfv1" runat="Server" Font-Size="xx-small" ControlToValidate="txtstartDate"
                                            ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                                        <ajaxToolkit:FilteredTextBoxExtender ID="ftbeStartDate" runat="server" TargetControlID="txtstartDate" ValidChars="1234567890/" FilterMode="ValidChars" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right' class="auto-style1">
                                        <b>End Date&nbsp;:&nbsp;</b>
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox ID="txtendDate" runat="Server" Width="80" CssClass="formfield"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator
                                            ID="rfv2" runat="Server" Font-Size="xx-small" ControlToValidate="txtendDate"
                                            ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                                        <ajaxToolkit:FilteredTextBoxExtender ID="ftbetxtendDate" runat="server" TargetControlID="txtendDate" ValidChars="1234567890/" FilterMode="ValidChars" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right' class="auto-style1">
                                        <%-- <b>Report by &nbsp;:&nbsp;</b>--%>
                                    </td>
                                    <td align='left'>
                                        <asp:DropDownList ID="drpIsUnique" CssClass="formlabel" runat="Server" Visible="false">
                                            <asp:ListItem Value="true" Selected="true">Unique</asp:ListItem>
                                            <asp:ListItem Value="false">Total</asp:ListItem>
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
                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False" ShowRefreshButton="false">
                            </rsweb:ReportViewer>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="gradient" colspan="2">&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
