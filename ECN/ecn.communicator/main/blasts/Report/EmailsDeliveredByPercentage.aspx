<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="EmailsDeliveredByPercentage.aspx.cs" Inherits="ecn.communicator.blasts.reports.EmailsDeliveredByPercentage" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
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
            $("#<%=txtFrom.ClientID%>").datepicker({
                showOn: "button",
                buttonImage: "/ecn.images/images/icon-calendar.gif",
                buttonImageOnly: true,
                buttonText: "Select date",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true
            });
            $("#<%=txtTo.ClientID%>").datepicker({
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
                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tr class="gradient">
                        <td valign="middle" style="border-right: medium none; padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; border-left: #a4a2a3 1px solid; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid" colspan="2">&nbsp;Email Delivered by Percentage
                        </td>
                    </tr>
                    <tr>
                        <td class="offWhite borderSides" colspan="2">
                            <table width="100%" cellspacing="2" cellpadding="10" border="0" class="formLabel">
                                <tr>
                                    <td valign="top" align="left" width="15%" class="offWhite borderSides">
                                        <asp:RadioButtonList ID="rbRange" runat="Server" AutoPostBack="True" RepeatDirection="Horizontal" class="TableContent" OnSelectedIndexChanged="rbRange_SelectedIndexChanged">
                                            <asp:ListItem Value="Week" Selected="True">Week</asp:ListItem>
                                            <asp:ListItem Value="Month">Month</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <asp:Panel ID="pnlWeek" runat="server" Visible="false">
                                        <td valign="middle" align="left" class="offWhite borderSides">
                                            <asp:TextBox ID="txtFrom" Width="65" CssClass="formfield" MaxLength="10" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                            <asp:TextBox ID="txtTo" Width="65" runat="server" CssClass="formfield" MaxLength="10"></asp:TextBox> &nbsp;&nbsp;
                                            <br />
                                        </td>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlMonth" runat="server" Visible="false">
                                        <td align="left" valign="middle" class="offWhite borderSides">
                                            <b>Month&nbsp;:&nbsp;</b>
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
                                            <b>Year&nbsp;:&nbsp;</b>
                                            <asp:DropDownList ID="drpYear" CssClass="formlabel" runat="Server"></asp:DropDownList>
                                        </td>
                                    </asp:Panel>

                                </tr>
                                <tr>
                                    <td class="offWhite borderSides"></td>
                                    <td align="left" valign="middle" class="offWhite borderSides">
                                        <asp:Button ID="btnReport" Text="View Report" runat="server" CssClass="button" Font-Size="XX-Small"
                                            OnClick="btnReport_Click"></asp:Button>
                                        &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnReset" Text="Reset" runat="server" CssClass="button" Font-Size="XX-Small"
                    OnClick="btnReset_Click"></asp:Button><br />

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDateError" runat="server" Text="Please submit a valid date" Visible="false" Width="150px"></asp:Label>
                                    </td>
                                </tr>
                                <asp:Panel ID="pnlReport" runat="server" Visible="false">
                                    <tr class="gridheader">
                                        <td align="right" colspan="2">
                                            <asp:DropDownList ID="drpExport" Width="100" runat="server" Font-Size="XX-Small">
                                                <asp:ListItem Selected="true" Value="pdf">PDF</asp:ListItem>
                                                <asp:ListItem Value="xls">Excel</asp:ListItem>
                                                <asp:ListItem Value="doc">Word</asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;<asp:Button ID="btnDownload" runat="server" Text="Download" OnClick="btnDownload_Click"
                                                Font-Size="XX-Small"></asp:Button>&nbsp;&nbsp;
                        &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="offWhite borderSides" colspan="2">
                                            <ecnCustom:ecnGridView ID="grdEmailsDelivered" runat="Server" AutoGenerateColumns="False"
                                                Style="margin: 7px 0;" Width="100%" OnPageIndexChanging="grdEmailsDelivered_PageIndexChanging"
                                                CssClass="grid" PageSize="25"
                                                ShowEmptyTable="true" EmptyTableRowText="No Data"
                                                AllowPaging="true" AllowSorting="false">
                                                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                <Columns>
                                                    <asp:BoundField DataField="Range" HeaderText="Range" ItemStyle-Width="35%" SortExpression="Range" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="TotalCount" HeaderText="Total Count" ItemStyle-Width="35%" SortExpression="Range" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    <asp:BoundField DataField="Percentage" HeaderText="Percentage" ItemStyle-Width="30%" SortExpression="Percentage" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:F2} %"></asp:BoundField>
                                                </Columns>
                                                <AlternatingRowStyle CssClass="gridaltrow" />
                                            </ecnCustom:ecnGridView>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="gradient" colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False" ShowRefreshButton="false">
                            </rsweb:ReportViewer>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
