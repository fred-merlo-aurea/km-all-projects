<%@ Page Language="c#" Inherits="ecn.creator.headerfooter.HeaderFooterList" CodeBehind="default.aspx.cs" MasterPageFile="~/Creator.Master" %>

<%@ Register TagPrefix="ecn" TagName="header" Src="../../includes/header.ascx" %>
<%@ Register TagPrefix="ecn" TagName="footer" Src="../../includes/footer.ascx" %>
<%@ MasterType VirtualPath="~/Creator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        function deleteHeaderFooter(theID) {
            if (confirm('Are you Sure?\nSelected Header Footer will be permanently deleted.')) {
                window.location = "default.aspx?HeaderFooterID=" + theID;
            }
        }
        function openWindow(hfID) {
            window.open('../pages/viewHeaderFooter.aspx?hfID=' + hfID + '', '', 'width=800,height=600,resizable=yes,scrollbars=yes')
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <td class="tableHeader"></td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="HeaderFootersGrid" runat="Server" CssClass="grid" Width="100%"
                    AutoGenerateColumns="False">
                    <ItemStyle Height="22"></ItemStyle>
                    <HeaderStyle CssClass="gridHeader"></HeaderStyle>
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn ItemStyle-Width="25%" DataField="HeaderFooterName" HeaderText="Name"></asp:BoundColumn>
                        <asp:BoundColumn ItemStyle-Width="50%" DataField="Keywords" HeaderText="Search Engine Keywords"></asp:BoundColumn>
                        <asp:HyperLinkColumn HeaderText="Preview" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="6%" Text="<img src=/ecn.images/images/icon-preview-HTML.gif alt='Preview Header - Footer' border='0'>" DataNavigateUrlField="HeaderFooterID" DataNavigateUrlFormatString="javascript:openWindow({0});" ItemStyle-HorizontalAlign="center"></asp:HyperLinkColumn>
                        <asp:TemplateColumn HeaderText="Edit" HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <a href='HeaderFooterDetail.aspx?HeaderFooterID=<%# DataBinder.Eval(Container.DataItem, "HeaderFooterIDplus") %>&amp;action=Edit'>
                                    <center>
                                        <img src="/ecn.images/images/icon-edits1.gif" alt='Edit Header - Footer' border='0'></center>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:HyperLinkColumn HeaderText="Delete" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="5%" Text="<img src=/ecn.images/images/icon-delete1.gif alt='Delete Header - Footer' border='0'>" DataNavigateUrlField="HeaderFooterID" DataNavigateUrlFormatString="javascript:deleteHeaderFooter({0});" ItemStyle-HorizontalAlign="center"></asp:HyperLinkColumn>
                    </Columns>
                    <AlternatingItemStyle CssClass="gridaltrow" />
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>

