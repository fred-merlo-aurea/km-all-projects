<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="emailProfile_ListSubscriptions.ascx.cs" Inherits="ecn.activityengines.includes.emailProfile_ListSubscriptions" %>
<br />
<table cellspacing="0" cellpadding="0" width="100%" border="0">
    <tr>
        <td align="left" style="font-weight: bold; color: red"><asp:Label ID="messageLabel" runat="server" CssClass="errormsg" Font-Bold="True" Visible="False"></asp:Label></td>
    </tr>
    <tr>
        <td width="100%" align=left>
            <asp:DataGrid ID="listSubscriptionsGrid" runat="server" Width="100%" AutoGenerateColumns="False" Visible="True" CssClass="grid">
                <ItemStyle></ItemStyle>
                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                <Columns>
                    <asp:BoundColumn DataField="ListName" HeaderText="List Name" ReadOnly="true" ItemStyle-Width="40%" HeaderStyle-HorizontalAlign="left"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Status" HeaderText="Status" ReadOnly="true" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="10%"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Type" HeaderText="Email Type" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ></asp:BoundColumn>
                    <asp:BoundColumn DataField="DateSubscribed" HeaderText="Subscribed On" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ></asp:BoundColumn>    
                    <asp:BoundColumn DataField="DateModified" HeaderText="Subscribe Changes On" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ></asp:BoundColumn>                                               
                </Columns>
                <AlternatingItemStyle CssClass="gridaltrow" />
            </asp:DataGrid>
        </td>
    </tr>
   </table> 