<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="rssList.aspx.cs" Inherits="ecn.communicator.main.content.rssList" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
        <table cellspacing="0" cellpadding="0" width="674" align="center">
            <tr>
                <td id="errorTop">
                </td>
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
                <td id="errorBottom">
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <table style="width:100%;">
        <tr>
            <td style="text-align:left;">
               
            </td>
            <td style="text-align:right;">
                <asp:Button ID="btnAddNewFeed" Text="Add New Feed" runat="server" OnClick="btnAddNewFeed_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvRSSFeeds" AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvRSSFeeds_RowDataBound" OnRowCommand="gvRSSFeeds_RowCommand" runat="server">
                    <Columns>
                        <asp:BoundField ItemStyle-Width="20%" HeaderText="Name" DataField="Name" />
                        <asp:BoundField ItemStyle-Width="50%" HeaderText="URL" DataField="URL" />
                        <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderText="Display Stories">
                            <ItemTemplate>
                                <asp:Label ID="lblStories" runat="server" />
                            </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderText="Edit" >
                            <ItemTemplate>
                                <asp:HyperLink ID="hlEdit" runat="server" Target="_self" ImageUrl="/ecn.images/images/icon-edits1.gif" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnDeleteRSS" ImageUrl="/ecn.images/images/icon-delete1.gif" runat="server" CommandName="deleterss" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
