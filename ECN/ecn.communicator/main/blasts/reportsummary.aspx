<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.reportsummary" CodeBehind="reportsummary.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="ecn" TagName="activitychart" Src="../../includes/activitychart.ascx" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tbody>
            <tr>
                <td class="tableHeader">
                    &nbsp;Latest Clicks
                </td>
            </tr>
            <tr>
                <td align="center">
                    <ecn:activitychart ID="ClickChart" runat="Server" />
                    <asp:DataGrid ID="ClickGrid" runat="Server" Width="700" AutoGenerateColumns="False"
                        CssClass="grid">
                        <ItemStyle></ItemStyle>
                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn ItemStyle-Width="20%" DataField="ActionDate" HeaderText="Time">
                            </asp:BoundColumn>
                            <asp:HyperLinkColumn ItemStyle-Width="30%"  ItemStyle-HorizontalAlign="Left" DataTextField="EmailSubject" DataTextFormatString="<font size=-2>{0}</font>"
                                HeaderText="Blast" DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="../blasts/reports.aspx?BlastID={0}">
                            </asp:HyperLinkColumn>
                            <asp:HyperLinkColumn ItemStyle-Width="30%"  ItemStyle-HorizontalAlign="Left" DataTextField="EmailAddress" DataTextFormatString="<font size=-2>{0}</font>"
                                HeaderText="EMail" DataNavigateUrlField="EmailID" DataNavigateUrlFormatString="../lists/emaileditor.aspx?EmailID={0}">
                            </asp:HyperLinkColumn>
                            <asp:HyperLinkColumn ItemStyle-Width="45%"  ItemStyle-HorizontalAlign="Left" DataTextField="ActionValue" DataTextFormatString="<font size=-2>{0}</font>"
                                HeaderText="Link" Target="_blank" DataNavigateUrlField="ActionValue" DataNavigateUrlFormatString="{0}">
                            </asp:HyperLinkColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td class="tableHeader">
                    <br />
                    &nbsp;Latest Bounces
                </td>
            </tr>
            <tr>
                <td  align="center">
                    <ecn:activitychart ID="BounceChart" runat="Server" />
                    <asp:DataGrid ID="BounceGrid" runat="Server" Width="700" AutoGenerateColumns="False"
                        CssClass="grid">
                        <ItemStyle></ItemStyle>
                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn ItemStyle-Width="20%" DataField="ActionDate" HeaderText="Time">
                            </asp:BoundColumn>
                            <asp:HyperLinkColumn ItemStyle-Width="30%"  ItemStyle-HorizontalAlign="Left" DataTextField="EmailSubject" DataTextFormatString="<font size=-2>{0}</font>"
                                HeaderText="Blast" DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="../blasts/reports.aspx?BlastID={0}">
                            </asp:HyperLinkColumn>
                            <asp:HyperLinkColumn ItemStyle-Width="30%"  ItemStyle-HorizontalAlign="Left" DataTextField="EmailAddress" DataTextFormatString="<font size=-2>{0}</font>"
                                HeaderText="EMail" DataNavigateUrlField="EmailID" DataNavigateUrlFormatString="../lists/emaileditor.aspx?EmailID={0}">
                            </asp:HyperLinkColumn>
                            <asp:BoundColumn ItemStyle-Width="20%"  DataField="ActionValue" HeaderText="BounceType">
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td class="tableHeader">
                    <br />
                    &nbsp;Latest Subscription Changes
                </td>
            </tr>
            <tr>
                <td  align="center">
                    <ecn:activitychart ID="SubscribeChart" runat="Server" />
                    <asp:DataGrid ID="SubscribeGrid" runat="Server" Width="700" AutoGenerateColumns="False"
                        CssClass="grid">
                        <ItemStyle></ItemStyle>
                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn ItemStyle-Width="20%" DataField="ActionDate" HeaderText="Time">
                            </asp:BoundColumn>
                            <asp:HyperLinkColumn  ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%" DataTextField="EmailSubject" DataTextFormatString="<font size=-2>{0}</font>"
                                HeaderText="Blast" DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="../blasts/reports.aspx?BlastID={0}">
                            </asp:HyperLinkColumn>
                            <asp:HyperLinkColumn  ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%" DataTextField="EmailAddress" DataTextFormatString="<font size=-2>{0}</font>"
                                HeaderText="EMail" DataNavigateUrlField="EmailID" DataNavigateUrlFormatString="../lists/emaileditor.aspx?EmailID={0}">
                            </asp:HyperLinkColumn>
                            <asp:BoundColumn ItemStyle-Width="20%" DataField="ActionValue" HeaderText="SubscribeType">
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
    </table>
    <br />
</asp:Content>
