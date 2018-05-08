<%@ Register TagPrefix="ecn" TagName="footer" Src="../../includes/footer.ascx" %>
<%@ Register TagPrefix="ecn" TagName="header" Src="../../includes/header.ascx" %>

<%@ Page Language="c#" Inherits="ecn.creator.templates.TemplateList" CodeBehind="default.aspx.cs" MasterPageFile="~/Creator.Master" %>
<%@ MasterType VirtualPath="~/Creator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        function deleteTemplate(theID) {
            if (confirm('Are you Sure?\n Selected Page will be permanently deleted.')) {
                window.location = "default.aspx?TemplateID=" + theID;
            }
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
                <asp:DataGrid ID="TemplatesGrid" runat="Server" Width="95%" AutoGenerateColumns="False"
                    CssClass="grid">
                    <ItemStyle></ItemStyle>
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn ItemStyle-Width="5%" DataField="TemplateID" HeaderText="ID"></asp:BoundColumn>
                        <asp:BoundColumn ItemStyle-Width="80%" DataField="TemplateName" HeaderText="Name"></asp:BoundColumn>
                        <asp:HyperLinkColumn ItemStyle-Width="5%" Text="Edit" DataNavigateUrlField="TemplateID" DataNavigateUrlFormatString="TemplateDetail.aspx?TemplateID={0}"></asp:HyperLinkColumn>
                        <asp:HyperLinkColumn ItemStyle-Width="10%" Text="Delete" DataNavigateUrlField="TemplateID" DataNavigateUrlFormatString="javascript:deleteTemplate({0});"></asp:HyperLinkColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>

