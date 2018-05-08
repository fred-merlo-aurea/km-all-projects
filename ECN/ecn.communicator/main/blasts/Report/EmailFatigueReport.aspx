<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="EmailFatigueReport.aspx.cs" Inherits="ecn.communicator.main.blasts.Report.EmailFatigueReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type='text/css'>
        .ui-datepicker-trigger {
            position: relative;
            vertical-align: middle;
            padding-left: 5px;
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
        <br />
    </asp:PlaceHolder>
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr class="gradient">
                        <td width="50%" valign="middle" style="border-right: medium none; padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; border-left: #a4a2a3 1px solid; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid">&nbsp;Email Fatigue Report
                        </td>
                        <td width="50%" align='right' valign="middle" style="border-right: #a4a2a3 1px solid; padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; border-left: medium none; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid">Download as:&nbsp;
                            <asp:DropDownList ID="ddlExport" CssClass="formlabel" runat="Server">
                                <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                                <asp:ListItem Value="xls">XLS</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="offWhite borderSides" valign="top" colspan="2">
                <table cellspacing="2" cellpadding="5" border='0' width="100%" class="formLabel"
                    style="margin: 10px 0px">

                    <tr>
                        <td width="40%" align='right'>
                            <b>Start Date&nbsp;:&nbsp;</b>
                        </td>
                        <td width="60%" align='left'>
                            <asp:TextBox ID="txtstartDate" runat="Server" Width="80" CssClass="formfield"></asp:TextBox>
                            &nbsp;<asp:RequiredFieldValidator
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
                            &nbsp;<asp:RequiredFieldValidator
                                ID="rfv2" runat="Server" Font-Size="xx-small" ControlToValidate="txtendDate"
                                ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblFilterField" Text="Filter Field:" runat="server" />
                        </td>
                        <td style="text-align: left; max-width: 200px;">
                            <asp:DropDownList ID="ddlFilterField" runat="server" OnSelectedIndexChanged="ddlFilterField_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="-1" Text="-SELECT-" Selected="True" />
                                <asp:ListItem Value="MessageTypeID" Text="Message Type" />
                                <asp:ListItem Value="GroupID" Text="Group" />
                                <asp:ListItem Value="CampaignID" Text="Campaign" />

                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblFilterValue" Text="Filter Value:" runat="server" />
                        </td>
                        <td style="text-align: left; max-width: 200px;">
                            <asp:DropDownList ID="ddlFilterValue" runat="server" Width="200px" AutoPostBack="false" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="Label1" Text="Show Opens and Clicks as:" runat="server" />
                        </td>
                        <td style="text-align: left; max-width: 200px;">
                            <asp:DropDownList ID="ddlOutputType" runat="server">
                                <asp:ListItem Value="Counts" Text="Counts" Selected="True" />
                                <asp:ListItem Value="Percents" Text="Percentages" />
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
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td class="offWhite borderSides" colspan="2" align="center">
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False" Width="100%" ShowZoomControl="false" ShowRefreshButton="false" ShowPrintButton="False" ShowExportControls="False">
                </rsweb:ReportViewer>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="gradient" colspan="2">&nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
