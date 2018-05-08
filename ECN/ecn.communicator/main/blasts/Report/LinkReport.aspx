<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LinkReport.aspx.cs" Inherits="ecn.communicator.blasts.reports.LinkReport"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
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
    <div style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 10px 0px; padding-top: 0px"
        align="center">
        <table class="grid" style="width: 100%" cellspacing="0" cellpadding="0" align="center"
            border="0">
            <tr class="gridheader">
                <td style="padding-right: 4px; padding-left: 8px; padding-bottom: 4px; width: 60%; padding-top: 4px"
                    align="left">
                    <span style="font-weight: bold; font-size: 8pt; color: #000000; font-style: normal; font-family: Arial">Link Report Filters</span>
                </td>
            </tr>
            <tr>
                <td align="left" height="150">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="labelsmall">
                                <b>Advertiser:</b>
                            </td>
                            <td class="labelsmall">
                                <b>Link Type:</b>
                            </td>
                            <td class="labelsmall">
                                <b>Date:(From - To)</b>
                            </td>
                            <td class="labelsmall">
                                <b>Campaign:</b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ListBox ID="lstlinkowner" runat="server" Rows="5" DataValueField="LinkOwnerIndexID"
                                    DataTextField="LinkOwnerName" SelectionMode="Multiple" Font-Size="XX-Small" Font-Names="Arial"
                                    Width="200px"></asp:ListBox>
                            </td>
                            <td>
                                <asp:ListBox ID="lstlinktype" runat="server" Rows="5" DataValueField="CodeID" DataTextField="CodeDisplay"
                                    SelectionMode="Multiple" Font-Size="XX-Small" Font-Names="Arial" Width="150px"></asp:ListBox>
                            </td>
                            <td class="labelsmall" valign="middle">
                                <asp:TextBox ID="txtFrom" Width="65" CssClass="formfield" MaxLength="10" runat="server"></asp:TextBox>&nbsp;<br />
                                <br />
                                <asp:TextBox ID="txtTo" Width="65" runat="server" CssClass="formfield" MaxLength="10"></asp:TextBox>&nbsp;&nbsp;<br />
                            </td>
                            <td>
                                <asp:ListBox ID="lstCampaign" runat="server" Rows="5" SelectionMode="Multiple"
                                    Font-Size="XX-Small" DataValueField="CampaignID" DataTextField="CampaignName" Font-Names="Arial"
                                    Width="200px"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 10px" valign="middle" align="center" colspan="6">
                                <asp:Button ID="btnReport" Text="View Report" runat="server" CssClass="button" Font-Size="XX-Small"
                                    OnClick="btnReport_Click"></asp:Button>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnReset" Text="Reset" runat="server" CssClass="button" Font-Size="XX-Small"
                                    OnClick="btnReset_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="pnlReport" runat="server" Visible="false">
            <table style="border-right: #dde4e8 2px solid; border-top: #dde4e8 2px solid; border-left: #dde4e8 2px solid; border-bottom: #dde4e8 2px solid"
                width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr class="gridheader">
                    <td style="padding-right: 4px; padding-left: 8px; padding-bottom: 4px; width: 60%; padding-top: 4px"
                        align="left">
                        <span style="font-weight: bold; font-size: 8pt; color: #000000; font-style: normal; font-family: Arial">Report</span>
                    </td>
                    <td align="right">
                        <asp:DropDownList ID="drpExport" Width="100" runat="server" Font-Size="XX-Small">
                            <asp:ListItem Selected="true" Value="pdf">PDF</asp:ListItem>
                            <asp:ListItem Value="xls">Excel</asp:ListItem>
                            <asp:ListItem Value="doc">Word</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;<asp:Button ID="btnDownload" runat="server" Text="Download" OnClick="btnDownload_Click"
                            Font-Size="XX-Small"></asp:Button>&nbsp;&nbsp;
                        <asp:Button ID="btndownloaddetails" runat="server" Text="Download Details" OnClick="btndownloaddetails_Click"
                            Font-Size="XX-Small"></asp:Button>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding: 10px 10px 10px 10px;">
                        <asp:DataGrid ID="ReportGrid" runat="Server" Width="100%" AutoGenerateColumns="False"
                            CssClass="gridWizard">
                            <FooterStyle CssClass="tableHeader1"></FooterStyle>
                            <AlternatingItemStyle CssClass="gridaltrowWizard"></AlternatingItemStyle>
                            <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                            <Columns>
                                <asp:BoundColumn DataField="LinkOwnerName" HeaderText="Link Owner">
                                    <ItemStyle Width="15%" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="LinkType" HeaderText="Link Type">
                                    <ItemStyle Width="10%" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:HyperLinkColumn ItemStyle-Width="20%" HeaderText="Link" Target="_blank" DataTextField="Alias"
                                    DataNavigateUrlField="Link"></asp:HyperLinkColumn>
                                <asp:BoundColumn DataField="blastcategory" HeaderText="CampaignName">
                                    <ItemStyle Width="8%" HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:HyperLinkColumn HeaderText="Effort Name" Target="_blank" DataTextField="EmailSubject"
                                    DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="../preview.aspx?BlastID={0}">
                                    <ItemStyle Width="30%" HorizontalAlign="Left"></ItemStyle>
                                </asp:HyperLinkColumn>
                                <asp:BoundColumn DataField="ActionDate" HeaderText="Date" DataFormatString="{0:d}"
                                    HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="8%" HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="ActionFrom" HeaderText="Effort Type" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="SendCount" HeaderText="Total Sends" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="ViewCount" HeaderText="Views" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="UClickCount" HeaderText="Unique Clicks" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="ClickCount" HeaderText="Total Clicks" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                        <AU:PagerBuilder ID="ReportGridPager" OnIndexChanged="ReportGridPager_IndexChanged"
                            runat="Server" Width="100%" PageSize="25" ControlToPage="ReportGrid">
                            <PagerStyle CssClass="gridpagerWizard"></PagerStyle>
                        </AU:PagerBuilder>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False" ShowRefreshButton="false">
                        </rsweb:ReportViewer>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
