<%@ Page Language="c#" ContentType="text/html" ResponseEncoding="iso-8859-1" CodeBehind="PreviewEmail.aspx.cs" AutoEventWireup="false" Inherits="ecn.showcare.wizard.PreviewEmail" validateRequest="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
  <HEAD>
		<title>Showcare</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<LINK href="./style.css" type="text/css" rel="stylesheet">
  </HEAD>
	<body>
		<div align="center">
			<div style="BACKGROUND: url(../images/bg.gif) repeat-y; WIDTH: 752px">
				<table cellSpacing="0" cellPadding="0" width="752" border="0">
					<TBODY>
						<form runat="server">
							<tr>
								<td colSpan="2"><IMG height="65" alt="Show Care" src="../images/header.gif" width="752"></td>
							</tr>
							<tr>
								<td vAlign="top" align="right" width="265"><IMG height="186" src="../images/img_step3.gif" width="255"></td>
								<td vAlign="top" align="left" width="487">
									<p class="title">Preview:</p>
									<p class="rightpaddingfortext">Please review the email carefully. If you are 
										satisfied, continue on to step 4. If you would like to make any&nbsp;changes 
										or&nbsp;corrections please use the back button at the bottom of the page.</p>
									<p align="center">
										<B>Email Address :</B> &nbsp;<asp:textbox width="200" id="txtTestEmail" Runat="server"></asp:textbox>
										<asp:button ID="btnTestEmail" Text="Send Test  Email" Runat="server" Width="130"></asp:button>
										<BR>
										<asp:requiredfieldvalidator id="rfvEmail" runat="server" ErrorMessage="«Required" ControlToValidate="txtTestEmail"></asp:requiredfieldvalidator>
										<asp:regularexpressionvalidator id="valEmailAddress" runat="server" ErrorMessage="«Not Valid" ControlToValidate="txtTestEmail"
											ValidationExpression=".*@.*\..*" Display="Static"></asp:regularexpressionvalidator>&nbsp;<br>
										<asp:Label ID="lblSentEmail" Visible=False ForeColor="red" Runat=server>A test email has been sent to the above mentioned email address.</asp:Label>	
									</p>
								</td>
							</tr>
							<tr>
								<td align="center" colSpan="2">
									<!--<div style="BACKGROUND:url(../images/preview_header.gif) no-repeat; WIDTH:620px; HEIGHT:147px"></div>-->
									<!--<div align="center" style="BACKGROUND:url(../images/preview_bg.gif) repeat-y; WIDTH:620px; HEIGHT:100px">-->
									<div>
										<!--preview content goes here-->
										<asp:Label id="previewLbl" runat="server" Font-Names="Arial"></asp:Label>
									</div>
									<!--<div style="BACKGROUND:url(../images/preview_footer.gif) no-repeat; WIDTH:620px; HEIGHT:59px"></div> -->
									<div style="MARGIN-TOP: 20px; MARGIN-BOTTOM: 25px" align="center">
										<table cellSpacing="0" cellPadding="0" width="340" border="0">
											<tr>
												<td><asp:imagebutton id="btnBack" style="CURSOR: hand" runat="server" CausesValidation="False" ImageUrl="../images/btn_goback.gif"></asp:imagebutton></td>
												<td><IMG height="41" src="../images/line_separator.gif" width="8"></td>
												<td><asp:imagebutton id="btnNext" runat="server" CausesValidation="False" ImageUrl="../images/btn_gostep4.gif"></asp:imagebutton></td>
											</tr>
										</table>
									</div>
								</td>
							</tr>
							<tr>
								<td colSpan="2" align="center" height="22" class="footer">
									Need Help? Click here for <a href="http://www.showlead.com/help.aspx" style="COLOR:#ffff00" target="_blank">
										assistance.</a></td>
							</tr>
						</form>
					</TBODY></table>
			</div>
		</div>
	</body>
</HTML>
