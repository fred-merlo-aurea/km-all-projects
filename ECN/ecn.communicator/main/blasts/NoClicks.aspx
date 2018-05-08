<%@ Page Language="c#" Inherits="ecn.communicator.main.blasts.NoClicks" CodeBehind="NoClicks.aspx.cs"
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
        <tr class="gradient">
            <td class="tableHeader" style="padding-right: 0px; border-top: #a4a2a3 1px solid;
                padding-left: 5px; padding-bottom: 0px; border-left: #a4a2a3 1px solid; padding-top: 0px;
                border-bottom: #a4a2a3 1px solid">
                <img src="/ecn.images/images/no_click.gif" />&nbsp;&nbsp;No Clicks
            </td>
            <td class="tableHeader" align='right' style="border-right: #a4a2a3 1px solid; padding-right: 5px;
                border-top: #a4a2a3 1px solid; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;
                border-bottom: #a4a2a3 1px solid">
                <asp:Panel ID="DownloadPanel" runat="Server" Visible="true">
                    Download No Clicks as&nbsp;&nbsp;
                    <asp:DropDownList class="formfield" ID="DownloadType" runat="Server" Visible="true"
                        EnableViewState="true">
                        <asp:ListItem Selected="true" Value=".xls">XLS file</asp:ListItem>
                        <asp:ListItem Value=".csv">CSV file</asp:ListItem>
                        <asp:ListItem Value=".txt">TXT File</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button class="formbuttonsmall" ID="DownloadEmailsButton" OnClick="downloadUnsubscribedEmails"
                        runat="Server" Visible="true" Text="Download"></asp:Button>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan='2' class="greySides offWhite" style="padding-right: 10px; padding-left: 10px;
                padding-bottom: 15px; padding-top: 15px; border-bottom: #a4a2a3 1px solid">
                <asp:DataGrid ID="NoClicksGrid" runat="Server" Width="100%" AutoGenerateColumns="False"
                    CssClass="gridWizard">
                    <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                    <AlternatingItemStyle CssClass="gridaltrowWizard" />
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn ItemStyle-Width="20%" DataField="Actiondate" HeaderText="Send Time"
                            ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                        <asp:BoundColumn ItemStyle-Width="80%" DataField="EmailAddress" HeaderText="Email Address"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
                <AU:PagerBuilder ID="NoClicksPager" runat="Server" Width="100%" PageSize="50">
                    <PagerStyle CssClass="gridpagerWizard"></PagerStyle>
                </AU:PagerBuilder>
            </td>
        </tr>
    </table>
    <br>
</asp:Content>
