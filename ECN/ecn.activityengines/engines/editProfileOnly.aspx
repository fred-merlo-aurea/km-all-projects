<%@ Page language="c#" Inherits="ecn.activityengines.editProfile" Codebehind="editProfileOnly.aspx.cs" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head>
		<title>Edit Profile</title>
		
		
		
		
		<style type="text/css"> @import url( /ecn.accounts/assets/channelID_1/images/ecnstyle.css ); @import url( /ecn.accounts/assets/channelID_1/stylesheet.css );  </style>
	</head>
	<body>
		<form id="Form1" method="post" runat="Server">
			<asp:Label id="MessageLabel" runat="Server" Visible="False" CssClass="errormsg" Font-Bold="True"></asp:Label></td>
			<asp:Panel ID="EmailProfile_Panel" Visible="False" runat="Server"></asp:Panel>
		</form>
	</body>
</html>
