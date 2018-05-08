<%@ Page Language="c#" ContentType="text/html" ResponseEncoding="iso-8859-1" CodeBehind="ReportingMain.aspx.cs" AutoEventWireup="false" Inherits="ecn.showcare.wizard.main.Reports.ReportingMain" %>
<%@ register tagprefix="sc" TagName="button" src="btnShowcare.ascx"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
  <HEAD>
		<title>Showcare</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<link rel="stylesheet" href="../style.css" type="text/css">
  </HEAD>
	<body>
		<form action="#" runat="server">
			<div align="center">
				<div style="BACKGROUND:url(../../images/bg.gif) repeat-y; WIDTH:752px">
					<table width="752" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td colspan="2"><img src="../../images/header.gif" alt="Show Care" width="752" height="65"></td>
						</tr>
						<tr>
							<td width="265" align="right" valign="top"><img src="../../images/img_reporting_main.gif" alt="Choose a template"></td>
							<td width="487" valign="top"><br>
								<br>
								<!-- <p class="title">Email Reporting :
									<asp:Label ID="lblEmail" ForeColor="#FF0000" Runat="server"></asp:Label></p>-->
								<p align="left" class="rightpaddingfortext">Welcome to the E-Lead Manager reporting 
									page. Here, you can review a summary of reports relating to your previously 
									sent emails. Click on the Message name to view your previously sent email.
								</p>
								<p align="left" class="rightpaddingfortext">Click on the icon in the 'Reports' 
									column to view Report details. Here, you have the ability to view; opens, 
									clicks, bounces, resends and forwards.
								</p>
								<!--								<p>"Delivery %" indicates what percentage of your emails sent were opened by the 
									recipients.</p>
-->
							</td>
						</tr>
						<tr>
							<td colspan="2" align="center" class="leftpaddingfortables">
								<!-- insert your table here width=650 -->
								<asp:DataGrid ID="dgReports" AutoGenerateColumns="False" Runat="server">
									<AlternatingItemStyle BackColor="#EDFFC7"></AlternatingItemStyle>
									<HeaderStyle HorizontalAlign="Center" ForeColor="White" BackColor="#299EE4"></HeaderStyle>
									<Columns>
										<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="PreviewEmail.aspx?ID={0}" DataTextField="MessageName" HeaderText="Message Name">
											<HeaderStyle ForeColor="White"></HeaderStyle>
											<ItemStyle HorizontalAlign="Left" CssClass="gridPadding"></ItemStyle>
										</asp:HyperLinkColumn>
										<asp:BoundColumn DataField="Subject" HeaderText="Subject">
											<HeaderStyle ForeColor="White"></HeaderStyle>
											<ItemStyle HorizontalAlign="Left" CssClass="gridPadding"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="SentTime" HeaderText="Sent Time">
											<HeaderStyle ForeColor="White"></HeaderStyle>
											<ItemStyle HorizontalAlign="Center" Width="12%"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Sends" HeaderText="Sends">
											<HeaderStyle ForeColor="White"></HeaderStyle>
											<ItemStyle HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:HyperLinkColumn Text="&lt;img border='0' src='../../images/icon_report2.gif' alt='View Email Report'&gt;" DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="ReportingMsgDetail.aspx?ID={0}" HeaderText="Reports">
											<HeaderStyle ForeColor="White"></HeaderStyle>
											<ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
										</asp:HyperLinkColumn>
									</Columns>
								</asp:DataGrid>
								<!-- end table -->
								<div align="center" style="MARGIN:20px 0px">
									<sc:button id="sc1" runat=server></sc:button>
								</div>
							</td>
						</tr>
						<tr>
							<td colspan="2" align="center" height="22" class="footer">
								Need Help? Click here for <a href="http://www.showlead.com/help.aspx" style="COLOR:#ffff00" target="_blank">
									assistance.</a>
							</td>
						</tr>
					</table>
				</div>
			</div>
		</form>
	</body>
</HTML>
