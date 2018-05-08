<%@ Page language="c#" Codebehind="pageDetail.aspx.cs" AutoEventWireup="false" Inherits="ecn.creator.fe.pages.pageDetail" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
	<title><%= pageTitle %></title>
	<meta http-equiv='Content-Type' content='text/html; charset=windows-1252'>
	<meta name='Keywords' content='<%= keywords %>'>
    <%= StyleSheet %>	
	<%= javaScriptCode %>	
</head>
<body <%= pageProps %> >
<%= body %>
</body>
</html>
