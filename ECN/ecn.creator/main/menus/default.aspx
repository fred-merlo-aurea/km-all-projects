<%@ Register TagPrefix="ecn" TagName="footer" Src="../../includes/footer.ascx" %>
<%@ Register TagPrefix="ecn" TagName="header" Src="../../includes/header.ascx" %>
<%@ MasterType VirtualPath="~/Creator.Master" %>
<%@ Page Language="c#" Inherits="ecn.creator.menus.MenuList" CodeBehind="default.aspx.cs" MasterPageFile="~/Creator.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        function deleteMenu(theID) {
            if (confirm('Are you Sure?\n Selected Page will be permanently deleted.')) {
                window.location = "default.aspx?MenuID=" + theID;
            }
        }
    </script>
    <script>
        function openWindow() {
            window.open('previewMenu.aspx', '', 'width=800,height=600,resizable=yes,scrollbars=yes,status=yes')
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="95%" border='0'>
        <tr>
            <td class="tableHeader" align='right'>Menu Type:
                <asp:DropDownList EnableViewState="true" ID="MenuTypeCode" runat="Server" DataValueField="CodeValue"
                    DataTextField="CodeDisplay" OnSelectedIndexChanged="MenuType_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:openWindow()">preview Menu -website</a>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="MenuGrid" runat="Server" Width="100%" AutoGenerateColumns="False"
                    CssClass="grid">
                    <ItemStyle></ItemStyle>
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn ItemStyle-Width="5%" DataField="MenuID" HeaderText="ID"></asp:BoundColumn>
                        <asp:BoundColumn ItemStyle-Width="20%" DataField="MenuCode" HeaderText="Code"></asp:BoundColumn>
                        <asp:BoundColumn ItemStyle-Width="25%" DataField="MenuName" HeaderText="Name"></asp:BoundColumn>
                        <asp:BoundColumn ItemStyle-Width="5%" DataField="SortOrder" HeaderText="SortOrder"></asp:BoundColumn>
                        <asp:BoundColumn ItemStyle-Width="5%" DataField="ParentID" HeaderText="ParentID"></asp:BoundColumn>
                        <asp:HyperLinkColumn ItemStyle-Width="5%" Text="Edit" DataNavigateUrlField="MenuID" DataNavigateUrlFormatString="MenuDetail.aspx?MenuID={0}"></asp:HyperLinkColumn>
                        <asp:HyperLinkColumn ItemStyle-Width="10%" Text="Delete" DataNavigateUrlField="MenuID" DataNavigateUrlFormatString="javascript:deleteMenu({0});"></asp:HyperLinkColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>

