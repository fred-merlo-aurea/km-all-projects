
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true"
    CodeBehind="ClearCache.aspx.cs" Inherits="KMPS.MD.Main.ClearCache" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
   
    <div>
     <asp:Button ID="btnClearCache"  runat="server" Text="Clear Cache"  
            onclick="btnClearCache_Click" />
    </div>
   
</asp:Content>