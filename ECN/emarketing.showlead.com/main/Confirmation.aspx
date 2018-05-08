<%@ Page language="c#" Codebehind="Confirmation.aspx.cs" AutoEventWireup="false" Inherits="ecn.showcare.wizard.main.Confirmation" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Confirmation</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="./style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" action="#" runat="server">
			<div align="center">
				<div style="BACKGROUND: url(../images/bg.gif) repeat-y; WIDTH: 752px">
					<table cellSpacing="0" cellPadding="0" width="752" border="0">
						<TBODY>
							<tr>
								<td colSpan="2"><IMG height="65" alt="Show Care" src="../images/header.gif" width="752"></td>
							</tr>
						
							<tr>
								<td style="WIDTH: 277px" vAlign="top" align="right" width="277"><IMG height="186" src="../images/img_confirm.gif" width="255"></td>
								<td vAlign="middle" align="left" width="487">
									<DIV style="PADDING-RIGHT: 10px; PADDING-LEFT: 10px; FONT-SIZE: 16px; PADDING-BOTTOM: 0px; PADDING-TOP: 0px; POSITION: relative"
										align="center" runat="server">
										<div style="WIDTH: 450px; HEIGHT: 80px">
											<div style="WIDTH: 450px" align="center"><IMG src="../images/step4_header.gif" width="450"></div>
											<div id="divMsg" style="FONT-SIZE: 11px; BACKGROUND: url(../images/step4_bg.gif) repeat-y; MARGIN-LEFT: -2px; WIDTH: 450px"
												runat="server"></div>
											<IMG style="HEIGHT: 16px" height="16" src="../images/step4_footer.gif" width="450">
										</div>
										<!--receipt content goes here--></DIV>
								</td>
							</tr>
							<tr>
								<td style="HEIGHT: 75px" align="center" colSpan="2" height="75">
									<asp:ImageButton id="btnShowlead" runat="server" ImageUrl="../images/btn_showcare.gif" />
								</td>
								<!--
												<td><IMG height="41" src="../images/line_separator.gif" width="8"></td>
												<td><asp:ImageButton id="btnNext" runat="server" ImageUrl="../images/btn_create_new_email.gif" />
												</td>
												-->
								</TD>
							</tr>
							<tr>
								<td colSpan="2" align="center" height="22" class="footer">
									Need Help? Click here for <a href="http://www.showlead.com/help.aspx" style="COLOR:#ffff00" target="_blank">
										assistance.</a></td>
							</tr>
						</TBODY>
					</table>
				</div>
			</div>
		</form>
	</body>
</HTML>
