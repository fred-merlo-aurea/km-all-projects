<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.CH_28_CustomReports"
    CodeBehind="CH_28_CustomReports.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="cpanel" Namespace="BWare.UI.Web.WebControls" Assembly="BWare.UI.Web.WebControls.DataPanel" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="3" width="100%" border='0'>
        <tbody>
            <tr>
                <td class="tableHeader" colspan='3'  align='left' >
                    &nbsp;Reports&nbsp;
                </td>
                <td class="tableHeader" align='right'>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" valign="top" align='right'>
                    &nbsp;Email Subject:
                </td>
                <td class="tableContent" width="600"  align='left'>
                    <asp:Label ID="EmailSubject" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" align='right'>
                    &nbsp;Email From:
                </td>
                <td class="tableContent"  align='left'>
                    <asp:Label ID="EmailFrom" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" align='right'>
                    &nbsp;Group To:
                </td>
                <td class="tableContent"  align='left'>
                    <asp:Label ID="GroupTo" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" align='right'>
                    &nbsp;Filter used:
                </td>
                <td class="tableContent"  align='left'>
                    <asp:Label ID="Filter" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" align='right'>
                    &nbsp;Message:
                </td>
                <td class="tableContent"  align='left'>
                    <asp:Label ID="Campaign" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" align='right'>
                    &nbsp;Send Time:
                </td>
                <td class="tableContent"  align='left'>
                    <asp:Label ID="SendTime" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" align='right'>
                    &nbsp;Finish Time:
                </td>
                <td class="tableContent"  align='left'>
                    <asp:Label ID="FinishTime" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" align='right'>
                    &nbsp;Successful:
                </td>
                <td class="tableContent"  align='left'>
                    <asp:Label ID="Successful" runat="Server"></asp:Label><asp:Label ID="SuccessfulPercentage"
                        runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" valign="top" align="left" colspan="2">
                    <table style="border-collapse: collapse; height: 100%" border="1">
                        <tbody>
                            <tr>
                                <td class="tableHeader1" bgcolor="#ffe09f">
                                    &nbsp;
                                </td>
                                <td class="tableHeader1" align="center" bgcolor="#ffe09f" width="80">
                                    Unique
                                </td>
                                <td class="tableHeader1" align="center" bgcolor="#ffe09f" width="80">
                                    Total
                                </td>
                                <td class="tableHeader1" align="center" bgcolor="#ffe09f" width="70">
                                    %
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeader1" align='right'>
                                    Opens&nbsp;
                                </td>
                                <td class="tableContent" align="center">
                                    <asp:Label ID="OpensUnique" runat="Server"></asp:Label>
                                </td>
                                <td class="tableContent" align="center">
                                    <asp:Label ID="OpensTotal" runat="Server"></asp:Label>
                                </td>
                                <td class="tableContent" align="center" bgcolor="#eeeeee">
                                    <asp:Label ID="OpensPercentage" runat="Server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeader1" align='right'>
                                    Clicks&nbsp;
                                </td>
                                <td class="tableContent" align="center">
                                    <asp:Label ID="ClicksUnique" runat="Server"></asp:Label>
                                </td>
                                <td class="tableContent" align="center">
                                    <asp:Label ID="ClicksTotal" runat="Server"></asp:Label>
                                </td>
                                <td class="tableContent" align="center" bgcolor="#eeeeee">
                                    <asp:Label ID="ClicksPercentage" runat="Server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeader1" align='right'>
                                    Bounces&nbsp;
                                </td>
                                <td class="tableContent" align="center">
                                    <asp:Label ID="BouncesUnique" runat="Server"></asp:Label>
                                </td>
                                <td class="tableContent" align="center">
                                    <asp:Label ID="BouncesTotal" runat="Server"></asp:Label>
                                </td>
                                <td class="tableContent" align="center" bgcolor="#eeeeee">
                                    <asp:Label ID="BouncesPercentage" runat="Server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeader1" align='right'>
                                    Unsubscribes&nbsp;
                                </td>
                                <td class="tableContent" align="center">
                                    <asp:Label ID="SubscribesUnique" runat="Server"></asp:Label>
                                </td>
                                <td class="tableContent" align="center">
                                    <asp:Label ID="SubscribesTotal" runat="Server"></asp:Label>
                                </td>
                                <td class="tableContent" align="center" bgcolor="#eeeeee">
                                    <asp:Label ID="SubscribesPercentage" runat="Server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeader1" align='right' width="143">
                                    Resends&nbsp;
                                </td>
                                <td class="tableContent" align="center">
                                    <asp:Label ID="ResendsUnique" runat="Server"></asp:Label>
                                </td>
                                <td class="tableContent" align="center">
                                    <asp:Label ID="ResendsTotal" runat="Server"></asp:Label>
                                </td>
                                <td class="tableContent" align="center" bgcolor="#eeeeee">
                                    <asp:Label ID="ResendsPercentage" runat="Server"></asp:Label>
                                </td>
                            </tr>
                            <% if (!(checkCustomerLevel() == "1"))
                               { %>
                            <tr>
                                <td class="tableHeader1" align='right' width="143">
                                    Forwards&nbsp;
                                </td>
                                <td class="tableContent" align="center">
                                    <asp:Label ID="ForwardsUnique" runat="Server"></asp:Label>
                                </td>
                                <td class="tableContent" align="center">
                                    <asp:Label ID="ForwardsTotal" runat="Server"></asp:Label>
                                </td>
                                <td class="tableContent" align="center" bgcolor="#eeeeee">
                                    <asp:Label ID="ForwardsPercentage" runat="Server"></asp:Label>
                                </td>
                            </tr>
                            <% } %>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan='3' align='right'>
                    <br />
                </td>
            </tr>
    </table>
    <table cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <td colspan='2' bgcolor='#eeeeee'>
                <cpanel:DataPanel ID="DataPanel1" runat="Server" ExpandImageUrl="expand.gif" CollapseImageUrl="collapse.gif"
                    CollapseText="Click to hide Contents List" ExpandText="Click to display Contents List"
                    Collapsed="False" TitleText="Top Click-Throughs" AllowTitleExpandCollapse="True"
                    Width="100%">
                    <div align='right'>
                        <asp:DropDownList ID="ClickSelectionDD" runat="Server" AutoPostBack="true" OnSelectedIndexChanged="ClickSelectionDD_SelectedIndexChanged"
                            CssClass="formfield">
                            <asp:ListItem Value="TOP 10" Selected="True">Top 10 Clicks</asp:ListItem>
                            <asp:ListItem Value="TOP 20">Top 20 Clicks</asp:ListItem>
                            <asp:ListItem Value="TOP 30">Top 30 Clicks</asp:ListItem>
                            <asp:ListItem Value="TOP 40">Top 40 Clicks</asp:ListItem>
                            <asp:ListItem Value="TOP 50">Top 50 Clicks</asp:ListItem>
                            <asp:ListItem Value="">All Clicks</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:DataGrid ID="TopGrid" runat="Server" BackColor="#eeeeee" CellPadding="2" RepeatColumns="3"
                        Width="100%">
                        <ItemStyle CssClass="tableContentSmall"></ItemStyle>
                        <HeaderStyle CssClass="tableHeader1"></HeaderStyle>
                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                        <AlternatingItemStyle BackColor="White" />
                    </asp:DataGrid>
                </cpanel:DataPanel>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
