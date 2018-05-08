<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.subscribes" CodeBehind="subscribes.aspx.cs"
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
            <td class="tableHeader" style="padding: 0 0 0 5px; border-top: 1px #A4A2A3 solid;
                border-left: 1px #A4A2A3 solid; border-bottom: 1px #A4A2A3 solid;">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td width="20">
                            <img src="/ecn.images/images/unsubscribe.gif" />
                        </td>
                        <td valign="top" align="left" class="tableHeader" style="padding: 2px 0 0 0;">
                            Unsubscribes
                        </td>
                    </tr>
                </table>
            </td>
            <td class="tableHeader" align='right' style="padding: 0 5px 0 0; border-top: 1px #A4A2A3 solid;
                border-right: 1px #A4A2A3 solid; border-bottom: 1px #A4A2A3 solid;">
                <asp:Panel ID="DownloadPanel" runat="Server" Visible="true">
                    Download
                     <%--<asp:DropDownList EnableViewState="true" ID="DownloadFilter" runat="Server" class="formfield"
                        Visible="true">
                        <asp:ListItem Selected="true" Value="all">All</asp:ListItem>
                        <asp:ListItem Value="unique">Unique</asp:ListItem>
                    </asp:DropDownList>--%>
                     &nbsp;Unsubscribed email addresses as&nbsp;&nbsp;
                    <asp:DropDownList EnableViewState="true" ID="DownloadType" runat="Server" class="formfield"
                        Visible="true">
                        <asp:ListItem Selected="true" Value=".xls">XLS file</asp:ListItem>
                        <asp:ListItem Value=".csv">CSV file</asp:ListItem>
                        <asp:ListItem Value=".txt">TXT File</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button class="formbuttonsmall" ID="DownloadEmailsButton" runat="Server" Visible="true"
                        Text="Download" OnClick="downloadUnsubscribedEmails"></asp:Button>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan='2' class="greySides offWhite" style="padding: 15px 10px; border-bottom: 1px #A4A2A3 solid;">
                <!-- removed the column from the following grid -->
                <!-- <asp:BoundColumn ItemStyle-Width="5%" DataField="SubscriptionChange" HeaderText="Change" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:BoundColumn> -->
                <asp:DataGrid ID="SubscribesGrid" runat="Server" Width="100%" AutoGenerateColumns="False"
                    CssClass="gridWizard">
                    <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                    <AlternatingItemStyle CssClass="gridaltrowWizard" />
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn ItemStyle-Width="18%" DataField="UnsubscribeTime" HeaderText="Time"
                            ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Email Address" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="left">
                            <ItemTemplate>
                                <a href="../lists/emaileditor.aspx?<%# DataBinder.Eval(Container.DataItem, "URL")%>">
                                    <%# DataBinder.Eval(Container.DataItem, "EmailAddress")%></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn ItemStyle-Width="62%" DataField="Reason" HeaderText="Reason" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="left">
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
                <AU:PagerBuilder id="SubscribesPager" runat="Server" Width="100%" PageSize="50" OnIndexChanged="SubscribesPager_IndexChanged">
                    <pagerstyle cssclass="gridpagerWizard"></pagerstyle>
                </AU:PagerBuilder>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
