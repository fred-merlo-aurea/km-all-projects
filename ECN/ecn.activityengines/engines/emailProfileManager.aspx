<%@ Page language="c#" Inherits="ecn.activityengines.emailProfileManager" Codebehind="emailProfileManager.aspx.cs" %>
	
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head>
		<title>Email Profile Manager</title>
		<style type="text/css">
			@import url( "/ecn.images/images/ecnstyle.css" ); @import url( "/ecn.images/images/stylesheet.css" ); 
		</style>
	</head>
	<body>
		<form id="Form1" method="post" runat="Server">
			<asp:Panel ID="EmailProfile_ManagerPanel" Visible="true" runat="Server">
				<TABLE style="BORDER-RIGHT: #281163 1px solid; BORDER-TOP: #281163 1px solid; BORDER-LEFT: #281163 1px solid; BORDER-BOTTOM: #281163 1px solid"
					cellspacing="2" cellpadding="2" width="770" align="center" border='0'>
					<tr>
						<td bgColor="#281163" colspan='3'>
							<DIV align="center"><FONT face="Verdana" color="#ffffff" size="2"><STRONG>Customer account information for <I><asp:Label id="EmailProfileNameLabel" runat="Server"></asp:Label></I></STRONG></FONT></DIV>
						</td>
					</tr>
					<tr>
						<td>
							<asp:Label id="MessageLabel" runat="Server" Visible="False" CssClass="errormsg" Font-Bold="True"></asp:Label></td>
					</tr>
					<tr>
						<td>
							<asp:Label id="EmailProfileManager_Menu_Label" runat="Server" Visible="true" Font-Bold="True"></asp:Label></td>
					</tr>
				</TABLE>
			</asp:Panel>
			<asp:Panel ID="EmailProfile_Control_Panel" Visible="False" runat="Server"></asp:Panel>
		</form>
	</body>
</html>
