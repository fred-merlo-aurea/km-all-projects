<%@ Page Language="c#" ContentType="text/html" ResponseEncoding="iso-8859-1" CodeBehind="PreviewEmail.aspx.cs" AutoEventWireup="false" Inherits="ecn.showcare.wizard.main.Reports.PreviewEmail" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>Showcare</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<link rel="stylesheet" href="./style.css" type="text/css">
	</HEAD>
	<body>
		<div align="center">
			<!--			<div style="BACKGROUND:url(../../images/bg.gif) repeat-y; WIDTH:752px">
				<table width="752" border="0" cellspacing="0" cellpadding="0">
					<form action="#" method="get">
						<TBODY>
							<tr>
								<td colspan="2"><img src="../../images/header.gif" alt="Show Care" width="752" height="65"></td>
							</tr>
							<tr>
								<td width="265" align="right" valign="top"><img src="../../images/img_step3.gif" width="255" height="186"></td>
								<td width="487" align="left" valign="top">
									<p class="title">Preview:</p>
									<p>Please review the email carefully. If you are sativsfied, continue on to step 4. 
										If you would like to make changes/ corrections please use the back button at 
										the bottom of the page.</p>
								</td>
							</tr>
							<tr>
								<td align="center" colspan="2">
									<div align="center" style="MARGIN-TOP:20px">
										<table width="340" border="0" cellspacing="0" cellpadding="0">
											<tr>
												<td><img src="../../images/btn_goback.gif" width="166" height="41"></td>
												<td><img src="../../images/line_separator.gif" width="8" height="41"></td>
												<td><input name="submit" type="image" id="submit" src="../../images/btn_gostep4.gif"></td>
											</tr>
										</table>
									</div>
								</td>
							</tr>
							<tr>
								<td colspan="2"><img src="../../images/footer.gif" alt="footer" width="752" height="22"></td>
							</tr>
					</form>
					</TBODY>
				</table> -->
			<asp:Label ID="lblPreview" Runat="server"></asp:Label>
		</div>
		<DIV></DIV>
	</body>
</HTML>
