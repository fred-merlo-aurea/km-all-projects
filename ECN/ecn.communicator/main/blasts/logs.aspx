<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.logs_main" CodeBehind="logs.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
<!--
@import url("/ecn.accounts/assets/channelID_1/images/ecnstyle.css");
@import url("/ecn.accounts/assets/channelID_1/stylesheet.css");
-->
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DataGrid ID="LogGrid" runat="Server" Width="100%" AutoGenerateColumns="False"
        CssClass="grid">
        <ItemStyle></ItemStyle>
        <HeaderStyle CssClass="gridheader"></HeaderStyle>
        <FooterStyle CssClass="tableHeader1"></FooterStyle>
        <Columns>
            <asp:BoundColumn ItemStyle-Width="10%" DataField="EmailID" HeaderText="EmailID" ItemStyle-HorizontalAlign="center"
                HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
            <asp:BoundColumn ItemStyle-Width="50%" DataField="EmailAddress" HeaderText="EmailAddress">
            </asp:BoundColumn>
            <asp:BoundColumn ItemStyle-Width="10%" DataField="Success" HeaderText="Success" ItemStyle-HorizontalAlign="center"
                HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
            <asp:BoundColumn ItemStyle-Width="30%" DataField="SendTime" HeaderText="SendTime"
                ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
        </Columns>
        <AlternatingItemStyle CssClass="gridaltrow" />
    </asp:DataGrid>
    <AU:PagerBuilder ID="EmailsPager" runat="Server" ControlToPage="LogGrid" PageSize="100"
        Width="100%">
        <PagerStyle CssClass="gridpager"></PagerStyle>
    </AU:PagerBuilder>
</asp:Content>
