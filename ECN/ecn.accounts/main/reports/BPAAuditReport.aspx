<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.Master" AutoEventWireup="true"
    CodeBehind="BPAAuditReport.aspx.cs" Inherits="ecn.accounts.main.reports.BPAAuditReport" %>

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
                <table width="100%" cellspacing="0" cellpadding="0" border='0'>
                    <tr class="gradient">
                        <td width="50%" valign="middle" style="border: 1px #A4A2A3 solid; border-right: none;
                            font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding: 0 5px;" align="left">
                            &nbsp;BPA Report&nbsp;
                        </td>
                        <td width="50%" align='right' valign="middle" style="border: 1px #A4A2A3 solid; border-left: none;
                            font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding: 0 5px;">
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
                                            DataTextField="BaseChannelName" DataValueField="BaseChannelID" AutoPostBack="True"
                                            OnSelectedIndexChanged="drpChannel_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right' valign="top">
                                        <b>Customer&nbsp;:&nbsp;</b>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="drpCustomer" runat="Server" DataTextField="customerName" DataValueField="customerID"
                                            Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right' valign="top">
                                        <b>Group ID&nbsp;:&nbsp;</b>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtGroupID" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right' valign="top">
                                        <b>PubCode&nbsp;:&nbsp;</b>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtPubCode" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="offWhite borderSides">
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
                                <tr>
                                    <td align='right' valign="top">
                                        <b>How Many Clicks&nbsp;:&nbsp;</b>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtClicks" runat="server" Width="50px"></asp:TextBox>
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
                    <tr>
                        <td class="offWhite borderSides" colspan="2" align="center">
                                <a href="" runat="server" id="hAuditReport" target="_blank" >
                                <asp:Label ID="lblAuditReport" runat="server" Text=""></asp:Label></a><br /><br />
                                <a href="" runat="server" id="hDataReport" target="_blank" >
                                <asp:Label ID="lblDataReport" runat="server" Text=""></asp:Label></a><br /><br />
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
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; visibility: visible;
        position: absolute; top: -500px" name="gToday:normal:agenda.js" src="/ecn.collector/scripts/ipopeng.htm"
        frameborder='0' width="174" scrolling="no" height="189"></iframe>
</asp:Content>
