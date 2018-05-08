<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.Master" AutoEventWireup="true" CodeBehind="TotalBlastsForDay.aspx.cs" Inherits="ecn.accounts.main.reports.TotalBlastsForDay" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .mainmenu
        {
            background: #5C5D5F url(http://images.ecn5.com/images/bar.jpg) top left repeat-x;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />

    <table id="idMain" width="100%" cellspacing="0" cellpadding="0" border='0' align="center">

        <tr>
            <td>
                <table width="100%" cellspacing="0" cellpadding="0" border='0'>
                    <tr class="gradient">
                        <td width="50%" align="left" valign="middle" style="border: 1px #A4A2A3 solid; border-right: none; font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding: 0 5px;">&nbsp;Total Blasts For Day&nbsp;
                        </td>
                        <td width="50%" align='right' valign="middle" style="border: 1px #A4A2A3 solid; border-left: none; font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding: 0 5px;">Download as&nbsp;:&nbsp;
                            <asp:DropDownList ID="drpExport" CssClass="formlabel" runat="Server">
                                <asp:ListItem Value="1" Selected>PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLS</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
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
            </td>
        </tr>
        <tr>
            <td class="offWhite borderSides">
                <br />
                <table width="100%">
                    <tr>


                        <td width="45%" align='right'>
                            <b>Date&nbsp;:&nbsp;</b>

                        </td>
                        <td width="55%" align='left'>
                            <asp:TextBox ID="txtstartDate" runat="Server" Width="80" CssClass="formfield"></asp:TextBox>
                            &nbsp;<img onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('ctl00_ContentPlaceHolder1_txtstartDate'),document.getElementById('ctl00_ContentPlaceHolder1_txtstartDate')); return false"
                                src="/ecn.images/images/icon-calendar.gif" align="absMiddle"><asp:RequiredFieldValidator
                                    ID="rfv1" runat="Server" Font-Size="xx-small" ControlToValidate="txtstartDate"
                                    ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                        </td>
                    </tr>


                </table>
                <br />
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
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False" ShowRefreshButton="false">
                </rsweb:ReportViewer>
                &nbsp;
                
            </td>
        </tr>
        <tr colspan="2">
            <td class="gradient" colspan="2">&nbsp;
            </td>
        </tr>
    </table>

    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; visibility: visible; position: absolute; top: -500px"
        name="gToday:normal:agenda.js" src="/ecn.collector/scripts/ipopeng.htm"
        frameborder='0' width="174" scrolling="no" height="189"></iframe>
</asp:Content>
