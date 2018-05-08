<%@ Page Language="c#" Inherits="ecn.creator.pages.viewHeaderFooter" Codebehind="viewHeaderFooter.aspx.cs" %>
<%@ MasterType VirtualPath="~/Creator.Master" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Preview of Header Footer -
        <%= headerFooterTitle %>
    </title>
    <meta http-equiv='Content-Type' content='text/html; charset=windows-1252'>
    <meta name='Keywords' content='<%= keywords %>'>
    <!--<style type="text/css">
		@import url("/ecn.accounts/assets/channelID_1/images/ecnstyle.css");
		@import url("/ecn.accounts/assets/channelID_1/stylesheet.css");
	</style>-->
    <%= StyleSheet %>
    <%= javaScriptCode %>
</head>
<body>
    <%= body %>
</body>
</html>
