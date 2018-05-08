<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.blastoverview" CodeBehind="blastoverview.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DataGrid ID="LogGrid" runat="Server" Width="100%" AutoGenerateColumns="False"
        CssClass="grid">
        <ItemStyle></ItemStyle>
        <HeaderStyle CssClass="gridaltrow"></HeaderStyle>
        <FooterStyle CssClass="tableHeader1"></FooterStyle>
        <Columns>
            <asp:BoundColumn ItemStyle-Width="10%" DataField="EmailID" HeaderText="EmailID">
            </asp:BoundColumn>
            <asp:BoundColumn ItemStyle-Width="50%" DataField="EmailAddress" HeaderText="EmailAddress" ItemStyle-HorizontalAlign="Left">
            </asp:BoundColumn>
            <asp:BoundColumn ItemStyle-Width="10%" DataField="Success" HeaderText="Success">
            </asp:BoundColumn>
            <asp:BoundColumn ItemStyle-Width="30%" DataField="SendTime" HeaderText="SendTime">
            </asp:BoundColumn>
        </Columns>
    </asp:DataGrid>
    <br />
</asp:Content>
