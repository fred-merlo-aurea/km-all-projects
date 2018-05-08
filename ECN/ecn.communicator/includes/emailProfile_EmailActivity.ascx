<%@ Control Language="c#" Inherits="ecn.communicator.includes.emailProfile_EmailActivity"
    CodeBehind="emailProfile_EmailActivity.ascx.cs" %>
<br>
<table style="border-right: #281163 1px solid; border-top: #281163 1px solid; border-left: #281163 1px solid;
    border-bottom: #281163 1px solid" cellspacing="2" cellpadding="2" width="770"
    align="center" border="0">
    <tr>
        <td bgcolor="#281163" colspan="3">
            <div align="center">
                <font face="Verdana" color="#ffffff" size="2"><strong>
                    <asp:Label ID="EmailActivityLabel" runat="server"></asp:Label></strong></font></div>
        </td>
    </tr>
    <tr>
        <td>
            <div align="left">
                <font style="font-weight: bold; color: red">
                    <asp:Label ID="MessageLabel" runat="server" CssClass="errormsg" Font-Bold="True"
                        Visible="False"></asp:Label></font></div>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="OpensPanel" runat="server">
                <asp:DataGrid ID="OpensEmailActivityGrid" runat="server" Visible="True" AutoGenerateColumns="False"
                    Width="100%" CssClass="grid">
                    <ItemStyle></ItemStyle>
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn DataField="EmailSubject" HeaderText="Email Title" ReadOnly="true"
                            ItemStyle-Width="65%"></asp:BoundColumn>
                        <asp:BoundColumn DataField="SendTime" HeaderText="Date Sent" ReadOnly="true" ItemStyle-Width="20%"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Opens" HeaderText="Open #'s" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                    </Columns>
                    <AlternatingItemStyle CssClass="gridaltrow" />
                </asp:DataGrid>
            </asp:Panel>
            <asp:Panel ID="ClicksPanel" runat="server">
                <asp:DataGrid ID="ClickssEmailActivityGrid" runat="server" Visible="True" AutoGenerateColumns="false"
                    Width="100%" CssClass="grid">
                    <ItemStyle></ItemStyle>
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn DataField="EmailSubject" HeaderText="Email Title" ReadOnly="true"
                            ItemStyle-Width="35%"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ActionValue" HeaderText="URL Clicked" ReadOnly="true"
                            ItemStyle-Width="43%" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ActionDate" HeaderText="Clicked On" ReadOnly="true" ItemStyle-Width="14%"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Clicks" HeaderText="Click #'s" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                    </Columns>
                    <AlternatingItemStyle CssClass="gridaltrow" />
                </asp:DataGrid>
            </asp:Panel>
        </td>
    </tr>
</table>
