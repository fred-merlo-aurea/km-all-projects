<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientProd.ascx.cs" Inherits="KMPS.MD.Main.Widgets.ClientProd" %>

<asp:UpdatePanel ID="UpdatePanelClientProd" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="PnlChart" runat="server">
            <table width="100%">
                <tr align="center">
                    <td>
                        <asp:Image ID="imgVenn" runat="server" />
                        <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="PnlSettings" runat="server" Visible="false">
            <table>
                <tr>
                    <td>Product Type :&nbsp;</td>
                    <td><asp:ListBox ID="lbProductType" runat="server" DataTextField="PubTypeDisplayName" DataValueField="PubTypeID" SelectionMode="Multiple" Rows="10"></asp:ListBox>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnApply" runat="server" Text="Apply" OnClick="btnApply_Click" /></td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Label ID="lblMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label></td>
                    <td></td>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
