<%@ Page Language="c#" AutoEventWireup="True" Inherits="ecn.accounts.error" CodeBehind="error.aspx.cs"
    MasterPageFile="~/MasterPages/Accounts.Master" EnableViewState="false" Title="error" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="errorTable" style="width: 800px;" cellspacing="1" cellpadding="1" width="800" 
        border='0'>
        <tr>
            <td style="width:15%;">
                <asp:Image ID="errorMsgImage" ImageUrl="http://images.ecn5.com/images/errorEx.jpg" runat="Server"></asp:Image>
            </td>
            <td style="width:85%;text-align:left;font-size:large;">
                <asp:Label ID="lblErrorMsg" runat="server" />
            </td>
        </tr>
       
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtErr" runat="Server" TextMode="MultiLine" Visible="false" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
