<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="rssEdit.aspx.cs" Inherits="ecn.communicator.main.content.rssEdit" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <table style="width: 100%;">
        <tr>
            <td style="text-align: left;">
                

            </td>
        </tr>
       
        <tr>
            <td style="text-align: right;">
                <asp:Label ID="lblName" runat="server" Text="RSS Feed Name:" />
            </td>
            <td>
                <asp:TextBox ID="txtName" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="text-align: right;">
                <asp:Label ID="lblURL" runat="server" Text="RSS Feed URL:" />
            </td>
            <td>
                <asp:TextBox ID="txtURL" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="text-align: right;">
                <asp:Label ID="lblStories" runat="server" Text="Stories to display:" />
            </td>
            <td>
                <asp:DropDownList ID="ddlStories" runat="server">
                    <asp:ListItem Text="--Select--" Value="0" Selected ="true" />
                    <asp:ListItem Text="1" Value="1" />
                    <asp:ListItem Text="2" Value="2" />
                    <asp:ListItem Text="3" Value="3" />
                    <asp:ListItem Text="4" Value="4" />
                    <asp:ListItem Text="5" Value="5" />
                    <asp:ListItem Text="6" Value="6" />
                    <asp:ListItem Text="7" Value="7" />
                    <asp:ListItem Text="8" Value="8" />
                    <asp:ListItem Text="9" Value="9" />
                    <asp:ListItem Text="10" Value="10" />
                    </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="btnSaveFeed" runat="server" Text="Save" OnClick="btnSaveFeed_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
