<%@ Page language="c#" Codebehind="ReportingMsgDetail.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard.Reports.ReportingMsgDetail" %>
<center>
	<form name="frm" method="post" id="Form1" class="body_bg" runat="server">
		<table width="718" cellpadding="0" cellspacing="0" border="0" ID="Table4">
			<tr>
				<td valign="top" width="154"><img src="../images/message_detail.gif" runat="server" id="imgSelection"></td>
				<td width="564" valign="top" align="left"  style="PADDING-LEFT:20px">
					<div style="PADDING-RIGHT: 20px; PADDING-LEFT: 10px; FONT-SIZE: 12px; PADDING-BOTTOM: 10px; PADDING-TOP: 10px">
						<div style=" FONT-SIZE: 12px; PADDING-TOP: 10px"><asp:label id="lblMessage" Runat="server"></asp:label></div>
						<div class="dashed_lines1" style="PADDING-RIGHT: 0px; FONT-SIZE: 12px; PADDING-BOTTOM: 0px; PADDING-TOP: 0px"><strong>Reporting-Message 
								Detail</strong></div>
						<div style="PADDING-RIGHT: 20px; PADDING-LEFT: 10px; PADDING-BOTTOM: 0px; PADDING-TOP: 10px">
							<table cellPadding="3" width="100%">
								<tr>
									<td align="right" width="22%">Email Subject :</td>
									<td width="78%"><asp:label id="mail_subject" runat="server"></asp:label></td>
								</tr>
								<tr>
									<td align="right">Mail From :</td>
									<td><asp:label id="mail_from" runat="server"></asp:label></td>
								</tr>
								<tr>
									<td align="right">Message :</td>
									<td><asp:label id="message" runat="server"></asp:label></td>
								</tr>
								<tr>
									<td align="right">Send Time :</td>
									<td><asp:label id="send_time" runat="server"></asp:label></td>
								</tr>
								<tr>
									<td align="right">Finish Time :</td>
									<td><asp:label id="finish_time" runat="server"></asp:label></td>
								</tr>
								<tr>
									<td align="right">Successful :</td>
									<td><asp:label id="success_rate" runat="server"></asp:label></td>
								</tr>
							</table>
						</div>
						<div class="dashed_lines1" style="PADDING-RIGHT: 75px; PADDING-LEFT: 0px; FONT-SIZE: 12px; PADDING-BOTTOM: 0px; PADDING-TOP: 0px"
							align="right"><strong><span style="COLOR: #003399">Click Totals to view details.</span></strong>
							<IMG height="12" src="../images/img_arrow6.gif" width="17">
						</div>
						<div class="dashed_lines1">
							<table cellSpacing="0" cellPadding="0" width="100%">
								<tr>
									<td vAlign="top" width="50%">
										<div style="PADDING-RIGHT: 4px; PADDING-LEFT: 4px; PADDING-BOTTOM: 4px; COLOR: #5b8fc3; PADDING-TOP: 4px">Click 
											on the numbers under TOTAL, in the table to the right, to access specific 
											reporting data. You can view reporting data from; Opens, Clicks, Bounces and 
											Forwards.
										</div>
									</td>
									<td vAlign="top" width="50%">
										<table cellSpacing="0" cellPadding="0" width="100%">
											<tr class="table_report_bg1">
												<td width="29%">&nbsp;</td>
												<td align="center" width="23%"><strong>Unique</strong></td>
												<td align="center" width="27%"><strong>Total</strong></td>
												<td align="center" width="21%"><strong>%</strong></td>
											</tr>
											<tr>
												<td align="right" width="29%"><strong>Opens: </strong>
												</td>
												<td align="center" width="23%"><asp:label id="opens_unique" runat="server"></asp:label></td>
												<td align="center" width="27%"><asp:HyperLink id="lnkOpen" runat="server"></asp:HyperLink>
												</td>
												<td align="center" width="21%"><asp:label id="opens_percent" runat="server"></asp:label></td>
											</tr>
											<tr class="table_report_bg1">
												<td align="right" width="29%"><strong>Clicks:</strong></td>
												<td align="center" width="23%"><asp:label id="clicks_unique" runat="server"></asp:label></td>
												<td align="center" width="27%"><asp:HyperLink id="lnkClick" runat="server"></asp:HyperLink>
												</td>
												<td align="center" width="21%"><asp:label id="clicks_percent" runat="server"></asp:label></td>
											</tr>
											<tr>
												<td align="right" width="29%"><strong>Bounces: </strong>
												</td>
												<td align="center" width="23%"><asp:label id="bounces_unique" runat="server"></asp:label></td>
												<td align="center" width="27%"><asp:HyperLink id="lnkBounce" runat="server"></asp:HyperLink>
												</td>
												<td align="center" width="21%"><asp:label id="bounces_percent" runat="server"></asp:label></td>
											</tr>
											<tr class="table_report_bg1">
												<td align="right" width="29%"><strong>Unsubscribe: </strong>
												</td>
												<td align="center" width="23%"><asp:label id="unsubscribe_unique" runat="server"></asp:label></td>
												<td align="center" width="27%"><asp:HyperLink id="lnkUnsubscribe" runat="server"></asp:HyperLink>
												</td>
												<td align="center" width="21%"><asp:label id="unsubscribe_percent" runat="server"></asp:label></td>
											</tr>											
											<tr>
												<td align="right" width="29%"><strong>Forward: </strong>
												</td>
												<td align="center" width="23%"><asp:label id="forward_unique" runat="server"></asp:label></td>
												<td align="center" width="27%"><asp:HyperLink id="lnkForward" runat="server"></asp:HyperLink>
												</td>
												<td align="center" width="21%"><asp:label id="forward_percent" runat="server"></asp:label></td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</div>
						<div style="PADDING-TOP: 10px" align="center">
							<table cellSpacing="0" cellPadding="0">
								<tr>
									<td valign="top"><asp:ImageButton ImageUrl="../images/btn_back_to_previous_page.gif" runat="server" ID="btnPrevious"></asp:ImageButton>&nbsp;</td>
									<td valign="top"></td>
								</tr>
							</table>
						</div>
					</div>
					<!--eof content-->
				</td>
			</tr>
		</table>
		<br>
	</form>
</center>
