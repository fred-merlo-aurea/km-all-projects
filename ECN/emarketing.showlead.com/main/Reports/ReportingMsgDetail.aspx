<%@ Page Language="c#" ContentType="text/html" ResponseEncoding="iso-8859-1" CodeBehind="ReportingMsgDetail.aspx.cs" AutoEventWireup="false" Inherits="ecn.showcare.wizard.main.Reports.ReportingMsgDetail" %>
<%@ register tagprefix="sc" TagName="button" src="btnShowcare.ascx"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
  <HEAD>
		<title>Showcare</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<link rel="stylesheet" href="../style.css" type="text/css">
			<script language="javascript">
			function windownewopen(id) {
				window.open('ReportingLogs.aspx?ID='+id, 'Logs', 'width=700,height=500,resizable=yes,scrollbars=yes,status=yes');
			}
			</script>
</HEAD>
	<body>
		<form method="get" action="#" runat="server">
			<div align="center">
				<div style="BACKGROUND:url(../../images/bg.gif) repeat-y; WIDTH:752px">
					<table width="752" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td colspan="2"><img src="../../images/header.gif" alt="Show Care" width="752" height="65"></td>
						</tr>
						<tr>
							<td width="265" align="right" valign="top"><img src="../../images/img_reporting_msg_detail.gif" alt="" width="255" height="195"></td>
							<td width="487" valign="top">
								<!--<p class="title">Email Reporting :
									<asp:Label ID="email" ForeColor="#FF0000" Runat="server">email@email.com</asp:Label></p>-->
								<br><br><br>
								<p align="left"  class="rightpaddingfortext">Click on the numbers in the table below, under "Total" to access specific 
									reporting data.
								</p>
								<p align="left"  class="rightpaddingfortext"> You can view detail information about Opens, Clicks, Bounces and Forwards.</p>
							</td>
						</tr>
						<tr>
							<td colspan="2" align="center">
								<br >
								<!--this is the top box table-->
								<table cellspacing="0" border="0" cellpadding="0" style="FONT-SIZE:12px">
								<tr>
									<td style="font-size:9px; width:130px; height:16px; background:url(../../images/msgdetail_1.gif) no-repeat;">&nbsp;</td>
									<td style="font-size:9px; width:500px; border-top:#8ED300 solid 1px;">&nbsp;</td>
								</tr>
								<tr>
									<td style="background:url(../../images/msgdetail_2.gif) repeat-y; padding-left:10px;">
									
									<!--this are only the label names not actual labels-->
									<table border="0" cellspacing="0" cellpadding="2">
									<tr>
										<td align="right"><strong>Email Subject: </strong>	</td>
									</tr>
									<tr>
										<td align="right"><strong>Mail From: </strong></td>
									</tr>
									<tr>
										<td align="right"><strong>Message: </strong></td>
									</tr>
									<tr>
										<td align="right"><strong>Send Time: </strong>	</td>
									</tr>
									<tr>
										<td align="right"><strong>Finish Time: </strong></td>
									</tr>
									<tr>
										<td align="right"><strong>Successful: </strong>	</td>
									</tr>
									</table>
									<!--end of label names-->	
															
									</td>
									<td align="left">
									
									<!--this where the actual labels are-->
									<table border="0" cellspacing="0" cellpadding="2">
									<tr>
										<td align="left"><asp:Label ID="mail_subject" runat="server"></asp:Label></td>
									</tr>
									<tr>
										<td align="left"><asp:Label ID="mail_from" runat="server"></asp:Label></td>
									</tr>
									<tr>
										<td align="left"><asp:Label ID="message" Runat="server"></asp:Label></td>
									</tr>
									<tr>
										<td align="left"><asp:Label ID="send_time" runat="server"></asp:Label></td>
									</tr>
									<tr>
										<td align="left"><asp:Label ID="finish_time" runat="server"></asp:Label></td>
									</tr>
									<tr>
										<td align="left">
										<asp:Label ID="success_rate" runat="server"></asp:Label>
										[<a style="FONT-WEIGHT:bold; CURSOR:pointer; COLOR:blue; TEXT-DECORATION:underline" onclick="windownewopen(<%=getBlastID()%>)" id="view_log">View Log</a>]
										</td>
									</tr>
									</table>
									<!--end of actual labels-->
									
									</td>
								</tr>
								<tr>
									<td style="height:17px; background:url(../../images/msgdetail_3.gif) no-repeat;">&nbsp;</td>
									<td style="border-bottom:#8ED300 solid 1px;">&nbsp;</td>
								</tr>
								</table>
								<!--end of top box-->						
								<br ><br >
								<!--this is the bottom box-->
								<table border="0" cellspacing="0" cellpadding="0" style="FONT-SIZE:11px">
							    <tr>
    								<td><img src="../../images/msgdetail_4.gif" ></td>
  								</tr>
								<tr>
    								<td style="background:url(../../images/msgdetail_5.gif) no-repeat; height:20px;">
									<table width="100%" border="0" cellspacing="0" cellpadding="0">
									  <tr>
									    <td width="29%">&nbsp;</td>
									    <td width="23%" align="center"><strong>Unique</strong></td>
									    <td width="27%" align="center"><strong>Total</strong></td>
									    <td width="21%" align="center"><strong>%</strong></td>
									  </tr>
									</table>
									</td>
  								</tr>
								<tr>
    								<td style="background:url(../../images/msgdetail_6.gif) no-repeat; height:18px;">
									<table width="100%" border="0" cellspacing="0" cellpadding="0">
									  <tr>
									    <td width="29%" align="right"><strong>Opens: </strong></td>
									    <td width="23%" align="center">
										<asp:Label ID="opens_unique" runat="server"></asp:Label></td>
									    <td width="27%" align="center">
										<a href="ReportingOpens.aspx?ID=<%=getBlastID()%>"><asp:Label ID="opens_total" runat="server"></asp:Label></a></td>
									    <td width="21%" align="center">
										<asp:Label ID="opens_percent" runat="server"></asp:Label></td>
									  </tr>
									</table>
									</td>
  								</tr>
								<tr>
    								<td style="background:url(../../images/msgdetail_7.gif) no-repeat; height:18px;">
									<table width="100%" border="0" cellspacing="0" cellpadding="0">
									  <tr>
									    <td width="29%" align="right"><strong>Clicks:</strong></td>
									    <td width="23%" align="center">
										<asp:Label ID="clicks_unique" runat="server"></asp:Label></td>
									    <td width="27%" align="center">
										<a href="ReportingClicks.aspx?ID=<%=getBlastID()%>"><asp:Label ID="clicks_total" runat="server"></asp:Label></a></td>
									    <td width="21%" align="center">
										<asp:Label ID="clicks_percent" runat="server"></asp:Label></td>
									  </tr>
									</table>
									</td>
  								</tr>
								<tr>
    								<td style="background:url(../../images/msgdetail_8.gif) no-repeat; height:20px;">
									<table width="100%" border="0" cellspacing="0" cellpadding="0">
									  <tr>
									    <td width="29%" align="right"><strong>Bounces: </strong></td>
									    <td width="23%" align="center">
										<asp:Label ID="bounces_unique" runat="server"></asp:Label></td>
									    <td width="27%" align="center">
										<a href="ReportingBounces.aspx?ID=<%=getBlastID()%>"><asp:Label ID="bounces_total" runat="server"></asp:Label></a></td>
									    <td width="21%" align="center">
										<asp:Label ID="bounces_percent" runat="server"></asp:Label></td>
									  </tr>
									</table>
									</td>
  								</tr>
								<tr>
    								<td style="background:url(../../images/msgdetail_9.gif) no-repeat; height:22px;">
									<table width="100%" border="0" cellspacing="0" cellpadding="0">
									  <tr>
									    <td width="29%" align="right"><strong>Forward: </strong></td>
									    <td width="23%" align="center">
										<asp:Label ID="forward_unique" runat="server"></asp:Label></td>
									    <td width="27%" align="center">
										<a href="ReportingForwards.aspx?ID=<%=getBlastID()%>"><asp:Label ID="forward_total" runat="server"></asp:Label></a></td>
									    <td width="21%" align="center">
										<asp:Label ID="forward_percent" runat="server"></asp:Label></td>
									  </tr>
									</table>
									</td>
  								</tr>
								</table>
								<!--end of bottom box-->
								
								<!--buttons-->
								<div align="center" style="MARGIN:60px auto 0px; POSITION:relative">
									<table width="340" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td align="center" colspan="3"><asp:Label ID="lblMessage" Runat="server"></asp:Label></td>
										</tr>
										<tr>
											<td><sc:button id="sc1" runat=server></sc:button></td>
											<td><img src="../../images/line_separator.gif" width="8" height="41"></td>
											<td><asp:ImageButton ID="backtoMain" ImageUrl="../../images/btn_backto_main.gif" runat="server"></asp:ImageButton></td>
										</tr>
									</table>
								</div>
							</td>
						</tr>
						<tr>
							<td colspan="2" align="center" height="22" class="footer">
								Need Help? Click here for <a href="http://www.showlead.com/help.aspx" target="_blank" style="COLOR:#ffff00"> assistance.</a>
							</td>
						</tr>
					</table>
				</div>
			</div>
		</form>
	</body>
</HTML>
