<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.forwards" CodeBehind="forwards.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="msglabel" runat="Server" CssClass="errormsg" Visible="false" />
    <asp:Panel ID="BasePanel" runat="Server" Visible="true">
        <table id="layoutWrapper" cellspacing="0" cellpadding="0" width="100%" border='0'>
            <tr>
                <td colspan="2" align='right' class="homeButton" style="height: 40px" valign="top">
                    <asp:LinkButton ID="btnHome" runat="Server" Text="<span>Report Summary Page</span>"
                        OnClick="btnHome_Click"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td class="gradient">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="20" style="padding: 0 0 0 5px;" align="center" valign="top">
                                <img src="/ecn.images/images/forwards.gif" />
                            </td>
                            <td valign="top" align="left" class="tableHeader" style="padding: 2px 0 0 5px;">
                                Forwards
                            </td>
                        </tr>
                    </table>
                </td>
                </tr>
                <tr>
                    <td class="offWhite greySides" style="padding: 10px; border-bottom: 1px #A4A2A3 solid;">
                        <asp:DataGrid ID="ForwardsGrid" runat="Server" Width="100%" AutoGenerateColumns="False"
                            CssClass="gridWizard">
                            <ItemStyle CssClass="tableContentSmall"></ItemStyle>
                            <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                            <FooterStyle CssClass="tableHeader1"></FooterStyle>
                            <Columns>
                                <asp:BoundColumn ItemStyle-Width="20%" DataField="ActionDate" HeaderText="Forward Time"
                                    ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Email Address" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="left" >
                                    <ItemTemplate>
                                        <a href="../lists/emaileditor.aspx?<%# DataBinder.Eval(Container.DataItem, "URL")%>">
                                            <%# DataBinder.Eval(Container.DataItem, "EmailAddress")%></a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Forwarded to" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="center"
                                    HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        >>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn ItemStyle-Width="35%" DataField="ActionValue" HeaderText="Referral">
                                </asp:BoundColumn>
                            </Columns>
                            <AlternatingItemStyle CssClass="gridaltrowWizard" />
                        </asp:DataGrid>
                        <AU:PagerBuilder ID="ForwardsPager" runat="Server" Width="100%" PageSize="50" OnIndexChanged="ForwardsPager_IndexChanged">
                            <PagerStyle CssClass="gridpagerWizard"></PagerStyle>
                        </AU:PagerBuilder>
                    </td>
                </tr>
        </table>
        <br />
    </asp:Panel>
</asp:Content>
