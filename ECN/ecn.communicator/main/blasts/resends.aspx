<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.resends" CodeBehind="resends.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper1" cellspacing="0" cellpadding="0" width="100%" border='0'>
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
                    <table id="layoutWrapper2" cellspacing="1" cellpadding="1" width="100%" border='0'>
                        <tr>
                            <td class="tableHeader">
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td width="20">
                                            <img src="/ecn.images/images/resends.gif" />
                                        </td>
                                        <td valign="top" align="left" class="tableHeader" style="padding: 2px 0 0 0;">
                                            Resends
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tableHeader" align='right'>
                                <asp:Panel ID="DownloadPanel" runat="Server" Visible="true">
                                    Download Resent email address as&nbsp;&nbsp;
                                    <asp:DropDownList EnableViewState="true" ID="DownloadType" runat="Server" class="formfield"
                                        Visible="true">
                                        <asp:ListItem Selected="true" Value=".xls">XLS file</asp:ListItem>
                                        <asp:ListItem Value=".csv">CSV file</asp:ListItem>
                                        <asp:ListItem Value=".txt">TXT File</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Button class="formbuttonsmall" ID="DownloadEmailsButton" runat="Server" Visible="true"
                                        Text="Download" OnClick="downloadResentEmails"></asp:Button>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan='2'>
                                <br />
                                <asp:DataGrid ID="ResendsGrid" runat="Server" Width="100%" AutoGenerateColumns="False"
                                    CssClass="gridWizard">
                                    <ItemStyle></ItemStyle>
                                    <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                    <Columns>
                                        <asp:BoundColumn ItemStyle-Width="25%" DataField="ActionDate" HeaderText="Date" ItemStyle-HorizontalAlign="center"
                                            HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Email Address" ItemStyle-Width="75%"  ItemStyle-HorizontalAlign="left">
                                            <ItemTemplate>
                                                <a href="../lists/emaileditor.aspx?<%# DataBinder.Eval(Container.DataItem, "URL")%>">
                                                    <%# DataBinder.Eval(Container.DataItem, "EmailAddress")%></a>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <AlternatingItemStyle CssClass="gridaltrowWizard" />
                                </asp:DataGrid>
                                <AU:PagerBuilder ID="ResendsPager" runat="Server" Width="100%" PageSize="20" OnIndexChanged="ResendsPager_IndexChanged">
                                    <PagerStyle CssClass="gridpagerWizard"></PagerStyle>
                                </AU:PagerBuilder>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
