<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.Master" AutoEventWireup="true" CodeBehind="EmailDirectReport.aspx.cs" Inherits="ecn.accounts.main.reports.EmailDirectReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script language="javascript" type="text/javascript">
        $(document).ready(function () {

            $("#<%= txtstartDate.ClientID %>").datepicker({
                defaultDate: "+1w",
                changeMonth: true,
                numberOfMonths: 3,
                onSelect: function (selectedDate) {
                    $("#<%= txtendDate.ClientID %>").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#<%= txtendDate.ClientID %>").datepicker({
                defaultDate: "+1w",
                changeMonth: true,
                numberOfMonths: 3,
                onSelect: function (selectedDate) {
                    $("#<%= txtstartDate.ClientID %>").datepicker("option", "maxDate", selectedDate);
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="idMain" width="100%" cellspacing="0" cellpadding="0" border='0' align="center">
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
                        <td colspan="2" width="50%" valign="middle" style="border: 1px #A4A2A3 solid; border-right: none;
                            font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding: 0 5px;">
                            &nbsp;Email&nbsp;Direct&nbsp;Report
                        </td>

                    </tr>
                    <tr>
                        <td class="offWhite borderSides">
                            <table cellspacing="2" cellpadding="5" border='0' width="100%" class="formLabel"
                                style="margin: 10px 0px">
                                <tr>
                                    <td width="30%" align='right'>
                                        <b>Channel&nbsp;:&nbsp;</b>
                                    </td>
                                    <td width="70%" align="left">
                                        <asp:DropDownList ID="drpChannel" runat="Server" CssClass="formlabel" Width="200px"
                                            AutoPostBack="True" OnSelectedIndexChanged="drpChannel_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right' valign="top">
                                        <b>Customer&nbsp;:&nbsp;</b>
                                    </td>
                                    <td align="left">
                                        <asp:ListBox ID="lstCustomer" runat="Server" Width="200" Height="200px" Rows="10"
                                            SelectionMode="Multiple" CssClass="formfield"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="offWhite borderSides" valign="top">
                            <table cellspacing="2" cellpadding="5" border='0' width="100%" class="formLabel"
                                style="margin: 10px 0px">
                                <tr>
                                    <td width="30%" align='right'>
                                        <b>Start Date&nbsp;:&nbsp;</b>
                                    </td>
                                    <td width="70%" align="left">
                                        <asp:TextBox ID="txtstartDate" runat="Server" Width="80"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="rfv1" runat="Server" Font-Size="xx-small" ControlToValidate="txtstartDate"
                                            ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right'>
                                        <b>End Date&nbsp;:&nbsp;</b>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtendDate" runat="Server" Width="80"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="rfv2" runat="Server" Font-Size="xx-small" ControlToValidate="txtendDate"
                                            ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="offWhite borderSides" colspan="2" align="center">
                            <asp:Button ID="btnSubmit" runat="Server" Text="Show Report" CssClass="formfield"
                                OnClick="btnSubmit_Click"></asp:Button>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr colspan="2">
                        <td class="gradient" colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; visibility: visible;
        position: absolute; top: -500px" name="gToday:normal:agenda.js" src="/ecn.collector/scripts/ipopeng.htm"
        frameborder='0' width="174" scrolling="no" height="189"></iframe>
</asp:Content>
