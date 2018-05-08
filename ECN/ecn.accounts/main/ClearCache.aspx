<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/accounts.Master"
    AutoEventWireup="true" CodeBehind="ClearCache.aspx.cs" Inherits="ecn.accounts.main.ClearCache" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView ID="gvCache" Width="100%" CssClass="grid" 
         AutoGenerateColumns="true" runat="server"  GridLines="None">
        <Columns>
        </Columns>
        <HeaderStyle BackColor="#c3c3c3" Font-Bold="True" Height="30px" HorizontalAlign="Left" />
    </asp:GridView>
    <asp:Button ID="btnClearCache"  runat="server" Text="Clear Cache"  
            onclick="btnClearCache_Click" />
</asp:Content>
