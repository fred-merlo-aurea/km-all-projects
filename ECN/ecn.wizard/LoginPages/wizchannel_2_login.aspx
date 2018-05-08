<%@ Page language="c#" Codebehind="wizchannel_2_login.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard.LoginPages.wizchannel_2_login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
  <HEAD>
		<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>
		<meta name='GENERATOR' content='ecn.wizard.ListCreate Class'>
		<link rel='stylesheet' type='text/css' href='../style.css'>
  </HEAD>
	<body>
		<center>
			<form action="../LoginHandlers/DirectLogin.aspx" method="post"  class="body_bg">
				<INPUT id="channelID" type="hidden" name="channelID" value="2">
					<img border="0" src="../images/KM_header.jpg"></td></TD>
				<br>
				<br>
				<table  cellpadding="0" cellspacing="0" border="0" ID="Table4">
					<tr>
						<td valign="top" width="154"><img src="../images/img_Login.gif"></td>
						<td width="564" valign="top" align="left" >
							<div style="PADDING-RIGHT: 20px; PADDING-LEFT: 10px; FONT-SIZE: 12px; PADDING-BOTTOM: 10px"><BR>
								<div class="dashed_lines1" style=" FONT-SIZE: 12px"><strong>Login</strong></div>
								<br>
								<span id="lblError" style="FONT-WEIGHT:bold;COLOR:red"></span>
								<br>
								<div style="PADDING-LEFT: 20px">
									<table border="0" cellpadding="0" cellspacing="0" width="100%">
										<tr>
											<td align="right" valign="middle" width="20%">Email&nbsp;:&nbsp;</td>
											<td align="left" valign="middle" width="80%"><INPUT id="userName" type="text" name="userName" size="30"></td>
										</tr>
										<tr>
											<td align="right" valign="middle">Password&nbsp;:&nbsp;</td>
											<td align="left" valign="middle"><INPUT id="password" type="password" name="password" size="30"></td>
										</tr>
										<tr>
											<td colspan="2" align="left" style="PADDING-LEFT: 102px;PADDING-top: 10px;PADDING-bottom: 10px;"><asp:Label id="MsgLabel" runat="server" Visible="False" ForeColor="red"></asp:Label></td>
										</tr>
										<tr>
											<td colspan="2" align="left" style="PADDING-LEFT: 102px;PADDING-top: 10px;PADDING-bottom: 10px;"><INPUT id="Button1" type="submit" value="Login" name="Button1"></td>
										</tr>
									</table>
									<br>	
								</div>	
								<div class="dashed_lines1" style="FONT-SIZE: 12px"><strong>New Customer</strong></div>
								<br>		
								<div style="PADDING-LEFT: 20px">							
								 If you are a new customer, please click <a href="registration.aspx?cID=2">HERE</a> to register.
										</div>
									<br>
									<br>
									<br>
									<br>
									<br>
									<br>
								
							</div>
						</td>
					</tr>
				</table>
				<table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td width="100%"><img src="../images/KM_footer.jpg" align="top" usemap="#Map" border="0"></td></tr></table>
				<map name="Map" id="Map">
					<area shape="rect" coords="448,20,751,36" href="http://www.knowledgemarketing.com/services_email.php" target="_blank" />
					<area shape="rect" coords="160,59,220,71" href="http://www.ecn5.com/privacy.htm" target="_blank" />
					<area shape="rect" coords="230,59,299,71" href="http://www.ecn5.com/antispam.htm" target="_blank" />
				</map>				
			</form>
		</center>
	</body>
</HTML>
