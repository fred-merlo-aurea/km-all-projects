<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard._default" %>
<center>
	<form name="frm" method="post" id="frm" class="body_bg" runat="server">
		<table width="718" cellpadding="0" cellspacing="0" border="0" ID="Table4">
			<tr>
				<td valign="top" width="154"><img src="images/img_selection.gif" /></td>
				<td width="564" valign="top" align="left" style="PADDING-LEFT:20px">
					<table border="0" cellpadding="0" cellspacing="0">
						<tr>
							<td colspan="2" align="left" valign="middle"><strong>Please select one of the following</strong>
							</td>
						</tr>
						<tr>
							<td colspan="2">&nbsp;</td>
						</tr>
						<tr>
							<td align="right" valign="middle">
								<a href="ListCreate.aspx" onMouseOver="document.image1.src='images/icon_1_on.gif'"
									onMouseOut="document.image1.src='images/icon_1_off.gif'"><img name="image1" border="0" src="images/icon_1_off.gif"></a>
								</A>
							</td>
							<td align="left" valign="middle">
								<div style="PADDING-LEFT:15px; FONT-FAMILY:Arial,Helvetica, Sans-Serif">
									<a href="ListCreate.aspx" style="FONT-WEIGHT:bold; FONT-SIZE:16px; COLOR:#000000; TEXT-DECORATION:none">
										Create a New Email List.<br>
									</a><font size="1">Add an Email list.</font>
								</div>
							</td>
						</tr>
						<tr>
							<td colspan="2">&nbsp;</td>
						</tr>
						<tr>
							<td align="right" valign="middle">
								<a href="ListManage.aspx" onMouseOver="document.image2.src='images/icon_2_on.gif'"
									onMouseOut="document.image2.src='images/icon_2_off.gif'"><img name="image2" border="0" src="images/icon_2_off.gif"></a>
							</td>
							<td align="left" valign="middle">
								<div style="PADDING-LEFT:15px; FONT-FAMILY:Arial,Helvetica, Sans-Serif">
									<a href="ListManage.aspx" style="FONT-WEIGHT:bold; FONT-SIZE:16px; COLOR:#000000; TEXT-DECORATION:none">
										Manage Your Email Lists.<br>
									</a><FONT size="1">Add to, Modify, or delete an existing Email list.</FONT>
								</div>
							</td>
						</tr>
						<tr>
							<td colspan="2">&nbsp;</td>
						</tr>
						<tr>
							<td align="right" valign="middle">
								<a href="wizard.aspx" onMouseOver="document.image3.src='images/icon_3_on.gif'" onMouseOut="document.image3.src='images/icon_3_off.gif'">
									<img name="image3" border="0" src="images/icon_3_off.gif"></a>
							</td>
							<td align="left" valign="middle">
								<div style="PADDING-LEFT:15px; FONT-FAMILY:Arial,Helvetica, Sans-Serif">
									<a href="wizard.aspx" style="FONT-WEIGHT:bold; FONT-SIZE:16px; COLOR:#000000; TEXT-DECORATION:none">
										Send Email.</a><br>
									<FONT size="1">Send an Email to one of your lists.</FONT>
								</div>
							</td>
						</tr>
						<tr>
							<td colspan="2">&nbsp;</td>
						</tr>
						<tr>
							<td align="right" valign="middle">
								<a href="reports/ReportingMain.aspx" onMouseOver="document.image4.src='images/icon_4_on.gif'"
									onMouseOut="document.image4.src='images/icon_4_off.gif'"><img name="image4" border="0" src="images/icon_4_off.gif"></a>
							</td>
							<td align="left" valign="middle">
								<div style="PADDING-LEFT:15px; FONT-FAMILY:Arial,Helvetica, Sans-Serif">
									<a href="reports/ReportingMain.aspx" style="FONT-WEIGHT:bold; FONT-SIZE:16px; COLOR:#000000; TEXT-DECORATION:none">
										View Reports.</a><br>
									<FONT size="1">View reports on Emails you have sent.<br>
										View information on opens, click throughs, bounces and forwards. </FONT>
								</div>
							</td>
						</tr>
					</table>
					<br>
					<br>
					<br>
					<br>
					<br>
					<br>
					<br>
					<br>
					<br>
				</td>
			</tr>
		</table>
		<!--eof content-->
	</form>
</center>
