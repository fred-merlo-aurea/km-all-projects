<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="ecn.activityengines.Error" MasterPageFile="/MasterPages/Activity.Master" %>
<%@ MasterType VirtualPath="/MasterPages/Activity.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="ECN-Label"></asp:Label>
</asp:Content>
