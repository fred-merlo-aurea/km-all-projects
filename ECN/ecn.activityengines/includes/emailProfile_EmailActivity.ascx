<%@ Control Language="c#" Inherits="ecn.activityengines.includes.emailProfile_EmailActivity"
    Codebehind="emailProfile_EmailActivity.ascx.cs" %>
<br>
<table cellspacing="0" cellpadding="0" width="100%" border="0">
    <tr>
        <td valign="bottom" align="left">
            <table cellspacing="0" cellpadding="0" border='0'>
                <tr>
                    <td class="wizTabsSub" valign="bottom">
                        <asp:LinkButton ID="btnEmailsSent" runat="Server" Text="<span>Sends</span>" OnClick="btnEmailsSent_Click"></asp:LinkButton>
                    </td>
                    <td class="wizTabsSub" valign="bottom">
                        <asp:LinkButton ID="btnEmailsOpened" runat="Server" Text="<span>Opens</span>" OnClick="btnEmailsOpened_Click"></asp:LinkButton>
                    </td>
                    <td class="wizTabsSub" valign="bottom">
                        <asp:LinkButton ID="btnEmailsClicked" runat="Server" Text="<span>Clicks</span>" OnClick="btnEmailsClicked_Click"></asp:LinkButton>
                    </td>
                    <td class="wizTabsSub" valign="bottom">
                        <asp:LinkButton ID="btnEmailsBunces" runat="Server" Text="<span>Bounces</span>" OnClick="btnEmailsBounced_Click"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="greySidesB" width="100%" align="left" style="PADDING: 5px 5px 5px 5px;
            BORDER-TOP: 1px #A4A2A3 solid; BACKGROUND-COLOR: #FFFFFF">
            <asp:DataGrid ID="campaignActivityGrid" runat="server" Visible="True" AutoGenerateColumns="False"
                Width="100%" CssClass="grid">
                <ItemStyle></ItemStyle>
                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                <Columns>
                    <asp:BoundColumn DataField="EmailSubject" HeaderText="Email Title" ReadOnly="true"
                        ItemStyle-Width="30%"></asp:BoundColumn>
                    <asp:BoundColumn DataField="BlastSendTime" HeaderText="Date Sent" ReadOnly="true"
                        ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="ActionValue" HeaderText="Click URL" ReadOnly="true" ItemStyle-Width="45%"
                        ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                    <asp:BoundColumn DataField="ActionCount" HeaderText="Count" ItemStyle-Width="5%"
                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                </Columns>
                <AlternatingItemStyle CssClass="gridaltrow" />
            </asp:DataGrid>
            <asp:Label ID="messageLabel" runat="server" Font-Size="Small" Visible="False"></asp:Label>
        </td>
    </tr>
</table>
