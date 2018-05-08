<%@ Page Language="c#" Inherits="ecn.communicator.main.blasts.Sends" CodeBehind="Sends.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="0" cellpadding="0" width="100%" border='0'>
        <tr>
            <td colspan="2" align='right' class="homeButton" style="height: 40px" valign="top">
                <asp:LinkButton ID="btnHome" runat="Server" Text="<span>Report Summary Page</span>"
                    OnClick="btnHome_Click"></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="gradient">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan='2' class="offWhite greySides" style="padding: 0 5px; border-bottom: 1px #A4A2A3 solid;">
                <div class="moveUp">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="left" class="tableHeader" valign="top" style="padding: 3px 0 0 0;" width="20">
                                <img src="/ecn.images/images/email_reports.gif" />
                            </td>
                            <td valign="top" align="left" class="tableHeader" style="padding: 5px 0 0 0;">
                                &nbsp;Sends
                            </td>
                            <td align='right'>
                                <div style="padding: 2px 0 0 0;">
                                    <asp:Panel ID="DownloadPanel" runat="Server" Visible="true" CssClass="tableHeader"
                                        Height="35px">
                                        Download as&nbsp;
                                        <asp:DropDownList class="formfield" ID="DownloadType" runat="Server" Visible="true"
                                            EnableViewState="true">
                                            <asp:ListItem Selected="true" Value=".xls">XLS file</asp:ListItem>
                                            <asp:ListItem Value=".csv">CSV file</asp:ListItem>
                                            <asp:ListItem Value=".txt">TXT File</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;
                                        <asp:Button class="formbuttonsmall" ID="DownloadEmailsButton" OnClick="downloadSendEmails"
                                            runat="Server" Visible="true" Text="Download"></asp:Button>
                                    </asp:Panel>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <asp:DataGrid ID="SendsGrid" runat="Server" Width="100%" AutoGenerateColumns="False"
                        CssClass="gridWizard">
                        <ItemStyle></ItemStyle>
                        <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn ItemStyle-Width="20%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"
                                DataField="SendTime" HeaderText="Send Time"></asp:BoundColumn>
                            <asp:BoundColumn ItemStyle-Width="80%" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left"
                                DataField="EmailAddress" HeaderText="Email Address"></asp:BoundColumn>
                        </Columns>
                        <AlternatingItemStyle CssClass="gridaltrowWizard" />
                    </asp:DataGrid>
                    <AU:PagerBuilder ID="SendsPager" runat="Server" Width="100%" PageSize="50" OnIndexChanged="SendsPager_IndexChanged">
                        <PagerStyle CssClass="gridpagerWizard"></PagerStyle>
                    </AU:PagerBuilder>
                </div>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
