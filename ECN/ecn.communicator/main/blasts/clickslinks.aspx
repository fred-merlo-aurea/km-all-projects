<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.clicks_links" CodeBehind="clickslinks.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <td class="tableContent" style="padding-top: 10px; text-align:left">
                &nbsp;<asp:Label ID="LinkLabel" runat="Server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="ClicksGrid" runat="Server" Width="100%" AutoGenerateColumns="true"
                    CssClass="grid">
                    <ItemStyle  HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Left"></HeaderStyle>
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <AlternatingItemStyle CssClass="gridaltrow" />
                </asp:DataGrid>
                <AU:PagerBuilder ID="ClicksPager" runat="Server" Width="100%" PageSize="50" ControlToPage="ClicksGrid"
                    onindexchanged="ClicksPager_IndexChanged">
                    <pagerstyle cssclass="gridpager"></pagerstyle>
                </AU:PagerBuilder>
            </td>
        </tr>
    </table>
</asp:Content>
