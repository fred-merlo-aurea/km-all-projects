<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.active" CodeBehind="active.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="cpanel" Namespace="BWare.UI.Web.WebControls" Assembly="BWare.UI.Web.WebControls.DataPanel" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function deleteBlast(theID) {
            if (confirm('Are you Sure?\nSelected Blast will be permanently deleted.')) {
                window.location = "blastApprovalHandler.aspx?action=delete&blastID=" + theID;
            }
        }	
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <td class="tableHeader" colspan="2" align="left">
                <asp:Panel ID="ActivePanel" Visible="False" runat="Server">
                    &nbsp;Active Emails<br />
                    <asp:DataGrid ID="ActiveGrid" runat="Server" Width="100%" CssClass="grid" AutoGenerateColumns="False">
                        <ItemStyle></ItemStyle>
                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn ItemStyle-Width="15%" DataField="BaseChannelName" HeaderText="Channel">
                            </asp:BoundColumn>
                            <asp:BoundColumn ItemStyle-Width="15%" DataField="CustomerName" HeaderText="Customer">
                            </asp:BoundColumn>
                            <asp:BoundColumn ItemStyle-Width="35%" DataField="EmailSubject" HeaderText="Email Title">
                            </asp:BoundColumn>
                            <asp:BoundColumn ItemStyle-Width="20%" DataField="GroupName" HeaderText="Group">
                            </asp:BoundColumn>
                            <asp:BoundColumn ItemStyle-Width="10%" DataField="SendTime" HeaderText="Send Time">
                            </asp:BoundColumn>
                            <asp:HyperLinkColumn HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="center" Text="<img src=/ecn.images/images/icon-preview-HTML.gif alt='Preview Content' border='0'>"
                                DataNavigateUrlField="BlastID" Target="_blank" DataNavigateUrlFormatString="preview.aspx?BlastID={0}">
                            </asp:HyperLinkColumn>
                            <asp:HyperLinkColumn ItemStyle-Width="5%" Text="Status" DataNavigateUrlField="BlastID"
                                DataNavigateUrlFormatString="status.aspx?BlastID={0}"></asp:HyperLinkColumn>
                        </Columns>
                    </asp:DataGrid><br />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" bgcolor="#eeeeee">
                <cpanel:DataPanel ID="Datapanel1" Style="z-index: 101" runat="Server" ExpandImageUrl="/ecn.images/images/collapse2.gif"
                    CollapseImageUrl="/ecn.images/images/collapse2.gif" CollapseText="Click to hide Pending Scheduled Blasts"
                    ExpandText="Click to display Pending Scheduled Blasts" Collapsed="False" TitleText="List of Pending Scheduled Blasts"
                    AllowTitleExpandCollapse="True">
                    <asp:DataGrid ID="ScheduledGrid" runat="Server" Width="100%" CssClass="grid" AutoGenerateColumns="False">
                        <ItemStyle></ItemStyle>
                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn ItemStyle-Width="15%" DataField="BaseChannelName" HeaderText="Channel">
                            </asp:BoundColumn>
                            <asp:BoundColumn ItemStyle-Width="15%" DataField="CustomerName" HeaderText="Customer">
                            </asp:BoundColumn>
                            <asp:BoundColumn ItemStyle-Width="35%" DataField="EmailSubject" HeaderText="Email Title">
                            </asp:BoundColumn>
                            <asp:BoundColumn ItemStyle-Width="20%" DataField="GroupName" HeaderText="Group">
                            </asp:BoundColumn>
                            <asp:BoundColumn ItemStyle-Width="10%" DataField="SendTime" HeaderText="Scheduled"
                                ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                            <asp:HyperLinkColumn HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="center" Text="<img src=/ecn.images/images/icon-preview-HTML.gif alt='Preview Content' border='0'>"
                                DataNavigateUrlField="BlastID" Target="_blank" DataNavigateUrlFormatString="preview.aspx?BlastID={0}">
                            </asp:HyperLinkColumn>
                            <asp:HyperLinkColumn ItemStyle-Width="5%" Text="Status" DataNavigateUrlField="BlastID"
                                DataNavigateUrlFormatString="status.aspx?BlastID={0}"></asp:HyperLinkColumn>
                        </Columns>
                    </asp:DataGrid>
                    <AU:PagerBuilder ID="ScheduledPager" runat="Server" ControlToPage="ScheduledGrid"
                        PageSize="10" Width="100%">
                        <PagerStyle CssClass="gridpager"></PagerStyle>
                    </AU:PagerBuilder>
                </cpanel:DataPanel>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" bgcolor="#eeeeee">
                <cpanel:DataPanel ID="DataPanel2" Style="z-index: 101" runat="Server" ExpandImageUrl="/ecn.images/images/collapse2.gif"
                    CollapseImageUrl="/ecn.images/images/collapse2.gif" CollapseText="Click to hide Pending Approval Blasts"
                    ExpandText="Click to display Pending Approval Blasts" Collapsed="False" TitleText="List of Pending Approval Blasts"
                    AllowTitleExpandCollapse="True">
                    <asp:DataGrid ID="PendingApprovalGrid" runat="Server" Width="100%" CssClass="grid"
                        AutoGenerateColumns="false" BackColor="#eeeeee" CellPadding="2" CellSpacing="0">
                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                        <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                        <ItemStyle></ItemStyle>
                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="CustomerName" HeaderText="Customer">
                                <ItemStyle Width="20%"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="EmailFromCol" HeaderText="From">
                                <ItemStyle Width="15%"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="EmailSubject" HeaderText="Subject">
                                <ItemStyle Width="30%"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SendTime" HeaderText="Date">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:HyperLinkColumn HeaderText="Preview" Text="<img src=/ecn.images/images/icon-preview-HTML.gif alt='Review this Message' border='0'>"
                                DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="javascript:var w=window.open('blastApprovalHandler.aspx?action=review&blastID={0}',null,'width=800,location=no,status=yes,resizable=yes,scrollbars=yes')">
                                <ItemStyle Width="3%" HorizontalAlign="Center"></ItemStyle>
                            </asp:HyperLinkColumn>
                            <asp:HyperLinkColumn HeaderText="delete" Text="<img src=/ecn.images/images/icon-delete1.gif alt='Reject / Delete this Message' border='0'>"
                                DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="javascript:deleteBlast({0});">
                                <ItemStyle Width="3%" HorizontalAlign="Center"></ItemStyle>
                            </asp:HyperLinkColumn>
                            <asp:HyperLinkColumn Text="Approve" DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="blastApprovalHandler.aspx?action=approve&blastID={0}">
                                <ItemStyle Width="3%"></ItemStyle>
                            </asp:HyperLinkColumn>
                        </Columns>
                    </asp:DataGrid>
                    <AU:PagerBuilder ID="PendingApprovalPager" runat="Server" ControlToPage="PendingApprovalGrid"
                        PageSize="15" Width="100%">
                        <PagerStyle CssClass="tableContent"></PagerStyle>
                    </AU:PagerBuilder>
                </cpanel:DataPanel>
            </td>
        </tr>
    </table>
    </br>
</asp:Content>
