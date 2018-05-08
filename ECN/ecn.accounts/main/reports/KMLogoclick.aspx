<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KMLogoclick.aspx.cs" Inherits="ecn.accounts.main.reports.KMLogoclick"
    MasterPageFile="~/MasterPages/Accounts.Master" %>

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
    <table id="idMain" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False">
                </rsweb:ReportViewer>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr class="gradient">
                        <td width="50%" valign="middle" style="border-right: medium none; padding-right: 5px;
                            border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif;
                            border-left: #a4a2a3 1px solid; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid">
                            &nbsp;Internet Retailer KM Click Count report
                        </td>
                        <td width="50%" align='right' valign="middle" style="border-right: #a4a2a3 1px solid;
                            padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px;
                            font: bold 13px Arial, Helvetica, sans-serif; border-left: medium none; color: #333;
                            padding-top: 0px; border-bottom: #a4a2a3 1px solid">
                            Download as:&nbsp;<asp:DropDownList ID="drpExport" CssClass="formlabel" runat="Server">
                                <asp:ListItem Value="pdf">PDF</asp:ListItem>
                                <asp:ListItem Value="xls">XLS</asp:ListItem>
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
                                    <td width="70%"  align="left">
                                        <asp:TextBox ID="txtstartDate" runat="Server" Width="80"></asp:TextBox>&nbsp;
                                        <asp:RequiredFieldValidator ID="rfv1" runat="Server" Font-Size="xx-small" ControlToValidate="txtstartDate"
                                            ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right'>
                                        <b>End Date&nbsp;:&nbsp;</b>
                                    </td>
                                    <td  align="left">
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
                            <asp:Button ID="btnReport" runat="Server" Text="Show Report" CssClass="formfield"
                                OnClick="btnReport_Click"></asp:Button>
                            <br>
                            <br>
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
    <br>
</asp:Content>
