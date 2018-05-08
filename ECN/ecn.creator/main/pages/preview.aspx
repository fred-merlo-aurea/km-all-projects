<%@ Page Language="c#" Inherits="ecn.creator.pages.preview" Codebehind="preview.aspx.cs" %>
<%@ MasterType VirtualPath="~/Creator.Master" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>
        <%= pageTitle %>
    </title>
    <meta http-equiv='Content-Type' content='text/html; charset=windows-1252'>
    <meta name='Keywords' content='<%= Keywords %>'>
    <%= StyleSheet %>
    <%= JavaScriptCode %>
</head>
<body <%= pageProps %>>
    <%= body %>
    <asp:Label ID="lblPreview" runat="server" />
</body>
</html>
