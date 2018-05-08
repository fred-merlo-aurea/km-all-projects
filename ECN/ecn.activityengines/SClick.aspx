<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SClick.aspx.cs" Inherits="ecn.activityengines.SClick" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
	<head id="Head1" runat="server" />
    <title></title>
		<link rel="stylesheet" href="/ecn.images/images/stylesheet.css" type="text/css">
		<link rel="stylesheet" href="/ecn.images/images//stylesheet_default.css" type="text/css">
	</head>
	<body>
		<asp:panel id="errorMsgPanel" runat=server visible=false>
			<table width=50% align=left border=0 cellpadding=5>
				<tr>
					<td style="font-family:arial; font-size:12px">
					We're sorry - the page you have requested does not exist.<br><br>If you typed the link in your email browser, please either retype the link making note of case sensivitity (‘a’ and ‘A’ are different), or click on the link in the original email you received.<br><br>If you clicked on the link in a text version email, it may have been broken by your email program. Please reply to the email you received and let us know which link caused a problem. We’ll correct the problem and send you a replacement link.<br><br><br><br>Sincerely,<br>Customer Support
					</td>
				</tr>
			</table>	
		</asp:Panel>
	</body>
</html>

