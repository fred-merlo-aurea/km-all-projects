<%@ Page language="c#" Codebehind="wizchannel_26_login.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard.LoginPages.wizchannel_26_login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
  <HEAD>
		<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>
		<meta name='GENERATOR' content='ecn.wizard.ListCreate Class'>
		<link rel='stylesheet' type='text/css' href='../style.css'>
  </HEAD>
	<body>
		<center>
			<form action="../LoginHandlers/DirectLogin.aspx" method="post" class="body_bg">
				<INPUT id="channelID" type="hidden" name="channelID" value="26">
				<table border="0" cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<td width="100%">
							<img border="0" src="../images/nb_header.jpg"></td>
					</tr>
				</table>
				<br>
				<br>
				<table width="718" cellpadding="0" cellspacing="0" border="0" ID="Table4">
					<tr>
						<td valign="top" width="154"><img src="../images/img_Login.gif"></td>
						<td width="564" valign="top" align="left">
							<div style="PADDING-RIGHT: 20px; PADDING-LEFT: 10px; FONT-SIZE: 12px; PADDING-BOTTOM: 10px"><BR>
								<div class="dashed_lines1" style=" FONT-SIZE: 12px"><strong>Login</strong></div>
								<br>
								<span id="lblError" style="FONT-WEIGHT:bold;COLOR:red"></span>
								<br>
								<div style="PADDING-LEFT: 20px">
									<table border="0" cellpadding="0" cellspacing="0" width="100%">
										<tr>
											<td align="right" valign="middle" width="20%">Email&nbsp;:&nbsp;</td>
											<td align="left" valign="middle" width="80%"><INPUT id="userName" type="text" name="userName"> <BR></td></tr>
										<tr>
											<td align="right" valign="middle">Password&nbsp;:&nbsp;</td>
											<td align="left" valign="middle"><INPUT id="password" type="password" name="password"></td>
										</tr>
										<tr>
											<td colspan="2" align="left" style="PADDING-LEFT: 102px;PADDING-top: 10px;PADDING-bottom: 10px;"><asp:Label id="MsgLabel" runat="server" Visible="False" ForeColor="red"></asp:Label></td>
										</tr>
										<tr>
											<td colspan="2" align="left" style="PADDING-LEFT: 150px;"><INPUT id="Button1" type="submit" value="Login" name="Button1"></td>
										</tr></table>
									<br>
									<br>
									<br>
									<br>
									<br>
									<br>
									<br>
									<br></div></div>
											</td>
										</tr>
									</table>
				<table border="0" cellpadding="0" cellspacing="0" width="94%" align="center"><tr><td width="100%"><img border="0" src="../images/nb_footer.jpg"></td></tr></table>
			</form>
		</center>
	</body>
</HTML>
