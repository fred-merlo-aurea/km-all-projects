<%@ Page Language="c#" CodeBehind="DomainTrackerReport.aspx.cs" Inherits="ecn.communicator.main.lists.DomainTrackerReport" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel='stylesheet' href="../../MasterPages/ECN_MainMenu.css" type="text/css" />
    <link rel='stylesheet' href="../../MasterPages/ECN_Controls.css" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
        <table cellspacing="0" cellpadding="0" width="674" align="center">
            <tr>
                <td id="errorTop"></td>
            </tr>
            <tr>
                <td id="errorMiddle">
                    <table height="67" width="80%">
                        <tr>
                            <td valign="top" align="center" width="20%">
                                <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                            </td>
                            <td valign="middle" align="left" width="80%" height="100%">
                                <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td id="errorBottom"></td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <br />
    <br />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label ID="lblDomainTracker" runat="server" Text="Domain Tracking Report" Font-Bold="true" Font-Size="Medium"></asp:Label>
            </td>
            <td align="right"></td>
        </tr>
    </table>
    <br />
    <table width="100%">
        <tr valign="top">
            <td></td>
            <td>
                <asp:Label ID="lblTotalViews" runat="server" Text="" Font-Bold="true" Font-Size="Medium"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr valign="top">
            <td></td>
            <td>
                <asp:Chart ID="chtPageViews" runat="server" BackImageTransparentColor="White">
                </asp:Chart>
            </td>
            <td align="center">
                <table width="100%">
                    <tr align="center">
                        <td>
                            <asp:Label ID="lblBrowserStats" runat="server" Text="Browser Stats" Font-Bold="true" Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvBrowserStats" runat="server" ShowHeader="false" OnRowDataBound="gvBrowserStats_RowDataBound"
                                GridLines="None" AutoGenerateColumns="false" CssClass="ECN-GridView" Width="100%">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="true">
                                        <ItemStyle HorizontalAlign="Left" Height="32px" />
                                        <ItemTemplate>
                                            <asp:Image ID="imgBrowser" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="true">
                                        <ItemStyle HorizontalAlign="Left" Height="32px" />
                                        <ItemTemplate>
                                            &nbsp;  
                                            <asp:Label ID="lblBrowser" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Browser") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Domain">
                                        <ItemStyle HorizontalAlign="Left" Height="32px" />
                                        <ItemTemplate>
                                            &nbsp;  &nbsp;
                                            <asp:Label ID="lblCounts" runat="server" Font-Bold="true" Text='<%# DataBinder.Eval(Container,"DataItem.Counts") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr valign="top">
            <td></td>
            <td>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblMostVisitedPages" runat="server" Text="Most Visited Pages" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            <asp:Label ID="lblMostVisitedPagesRange" runat="server" Text=" - Tracked since " Visible="false"></asp:Label>
                            <asp:DropDownList ID="ddlMostVisitedPagesRange" runat="server" AutoPostback="true" Visible="false" OnSelectedIndexChanged="ddlMostVisitedPagesRange_IndexChanged">
                                <asp:ListItem Text="domain tracking implemented" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="last week" Value="7"></asp:ListItem>
                                <asp:ListItem Text="the last 30 days" Value="30"></asp:ListItem>
                                <asp:ListItem Text="the last 60 days" Value="60"></asp:ListItem>
                                <asp:ListItem Text="the last 90 days" Value="90"></asp:ListItem>
                                <asp:ListItem Text="the last year" Value="365"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvPageViews" runat="server" ShowHeader="false" Width="80%" GridLines="None" CssClass="ECN-GridView" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="true">
                                        <ItemStyle HorizontalAlign="Left" Height="32px" />
                                        <ItemTemplate>
                                            &nbsp;  
                                            <asp:Label ID="lblPageURL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.PageURL") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Domain">
                                        <ItemStyle HorizontalAlign="Left" Height="32px" />
                                        <ItemTemplate>
                                            &nbsp;  &nbsp;
                                            <asp:Label ID="lblCounts" runat="server" Font-Bold="true" Text='<%# DataBinder.Eval(Container,"DataItem.Counts") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>

            </td>
            <td align="center">
                <table width="100%">
                    <tr align="center">
                        <td>
                            <asp:Label ID="lblPlatformStats" runat="server" Text="Platform Stats" Font-Bold="true" Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvPlatformStats" runat="server" ShowHeader="false" OnRowDataBound="gvPlatformStats_RowDataBound"
                                GridLines="None" AutoGenerateColumns="false" CssClass="ECN-GridView" Width="100%">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="true">
                                        <ItemStyle HorizontalAlign="Left" Height="32px" />
                                        <ItemTemplate>
                                            <asp:Image ID="imgPlatform" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="true">
                                        <ItemStyle HorizontalAlign="Left" Height="32px" />
                                        <ItemTemplate>
                                            &nbsp;  
                                            <asp:Label ID="lblOS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.OS") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Domain">
                                        <ItemStyle HorizontalAlign="Left" Height="32px" />
                                        <ItemTemplate>
                                            &nbsp;  &nbsp;
                                            <asp:Label ID="lblCounts" runat="server" Font-Bold="true" Text='<%# DataBinder.Eval(Container,"DataItem.Counts") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
