<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Page Language="c#" Inherits="ecn.accounts.main.reports.DiskSpace" CodeBehind="DiskSpace.aspx.cs"
    MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="idMain" style="width: 100%" cellspacing="0" cellpadding="0" border='0'
        align="center">
        <tr>
            <td>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False">
                </rsweb:ReportViewer>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" cellspacing="0" cellpadding="0" border='0'>
                    <tr class="gradient">
                        <td width="60%" valign="middle" style="border: 1px #A4A2A3 solid; border-right: none;
                            font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding: 0 5px;">
                            &nbsp;Customer&nbsp;Diskspace&nbsp;Report
                        </td>
                        <td width="40%" align='right' valign="middle" style="border: 1px #A4A2A3 solid; border-left: none;
                            font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding: 0 5px;">
                            Download as:&nbsp;<asp:DropDownList ID="drpExport" CssClass="formlabel" runat="Server">
                                <asp:ListItem Value="1" Selected>PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLS</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="offWhite borderSides">
                            <table cellspacing="2" cellpadding="5" border='0' width="100%" class="formLabel"
                                style="margin: 10px 0;">
                                <tr>
                                    <td width="220" align='right'>
                                        Channel&nbsp;:&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="drpChannel" runat="Server" CssClass="formlabel" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right'>
                                        Month&nbsp;:&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList CssClass="formlabel" ID="drpMonth" runat="Server">
                                            <asp:ListItem Value="1">January</asp:ListItem>
                                            <asp:ListItem Value="2">February</asp:ListItem>
                                            <asp:ListItem Value="3">March</asp:ListItem>
                                            <asp:ListItem Value="4">April</asp:ListItem>
                                            <asp:ListItem Value="5">May</asp:ListItem>
                                            <asp:ListItem Value="6">June</asp:ListItem>
                                            <asp:ListItem Value="7">July</asp:ListItem>
                                            <asp:ListItem Value="8">August</asp:ListItem>
                                            <asp:ListItem Value="9">September</asp:ListItem>
                                            <asp:ListItem Value="10">October</asp:ListItem>
                                            <asp:ListItem Value="11">November</asp:ListItem>
                                            <asp:ListItem Value="12">December</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right'>
                                        Show Exceeded Customers Only&nbsp;:&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="chkOverLimit" runat="Server"></asp:CheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Button ID="btnSubmit" runat="Server" Text="Show Report" OnClick="btnSubmit_Click">
                                        </asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr colspan="2">
                        <td class="gradient" style="border: 1px #A4A2A3 solid;" colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                    <br />
            </td>
        </tr>
    </table>
</asp:Content>
