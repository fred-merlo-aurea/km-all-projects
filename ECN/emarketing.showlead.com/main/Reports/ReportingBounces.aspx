<%@ Page Language="c#" ContentType="text/html" ResponseEncoding="iso-8859-1" CodeBehind="ReportingBounces.aspx.cs" AutoEventWireup="false" Inherits="ecn.showcare.wizard.main.Reports.ReportingBounces" %>
<%@ register tagprefix="sc" TagName="button" src="btnShowcare.ascx"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
  <HEAD>
		<title>Showcare</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<link rel="stylesheet" href="../style.css" type="text/css">
  </HEAD>
	<body>
		<form runat="server">
			<div align="center">
				<div style="BACKGROUND:url(../../images/bg.gif) repeat-y; WIDTH:752px">
					<table width="752" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td colspan="2"><img src="../../images/header.gif" alt="Show Care" width="752" height="65"></td>
						</tr>
						<tr>
							<td width="265" align="right" valign="top"><img src="../../images/img_reporting_bounces.gif" alt="" width="255" height="181"></td>
							<td width="487" align="left" valign="top"><BR>
								<BR>
								<!--<p class="title">Email Reporting :
									<asp:Label ID="email" Runat="server" ForeColor="#FF0000"></asp:Label></p>-->
								<b>Bounces</b><br>
								Bounces are emails that were not delivered to the recipient. This can occur for 
								a variety of reasons, including an invalid email address, and address box that 
								is full, etc.<br>
								<br>
								<b>Displays:</b>
								<P></P>
								<UL>
									<LI>
										<DIV class="rightpaddingfortext">Date and time that the email bounced
										</DIV>
									<LI>
										<DIV class="rightpaddingfortext">The email address of the recipient
										</DIV>
									<LI>
										<DIV class="rightpaddingfortext">First name of the recipient
										</DIV>
									<LI>
										<DIV class="rightpaddingfortext">Last name of the recipient
										</DIV>
									<LI>
										<DIV class="rightpaddingfortext">Reason for the bounce
										</DIV>
									</LI>
								</UL>
								<BR>
								<BR>
							</td>
						</tr>
						<tr>
							<td colspan="2" align="right" valign="top">
								<table align="center" width="650" border="0" cellpadding="0" cellspacing="0">
									<tr>
										<td align="left"><b>Bounces</b></td>
										<td align="right">
											<b>Download Current View</b>&nbsp;&nbsp;
											<asp:DropDownList ID="ddlType" EnableViewState="True" Runat="server">
												<asp:ListItem Selected="True" Value=".csv">CSV File</asp:ListItem>
												<asp:ListItem Value=".txt">Text File</asp:ListItem>
											</asp:DropDownList>&nbsp;&nbsp;
											<asp:ImageButton ID="btnDownload" Runat="server" ImageUrl="../../images/btn_download.gif" />
										</td>
									</tr>
									<tr>
									
										<td colspan="2" class="leftpaddingfortables">
											<asp:DataGrid ID="dgBounces" Runat="server" AutoGenerateColumns="False" PageSize="20" AllowPaging="True"
												Width="645px">
												<AlternatingItemStyle BackColor="#EDFFC7"></AlternatingItemStyle>
												<HeaderStyle HorizontalAlign="Center" ForeColor="White" BackColor="#299EE4"></HeaderStyle>
												<Columns>
													<asp:BoundColumn DataField="BounceTime" HeaderText="Bounce Time">
														<HeaderStyle ForeColor="White"></HeaderStyle>
														<ItemStyle HorizontalAlign="Center"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address">
														<HeaderStyle ForeColor="White"></HeaderStyle>
														<ItemStyle HorizontalAlign="Left"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FirstName" HeaderText="First Name">
														<HeaderStyle ForeColor="White"></HeaderStyle>
														<ItemStyle HorizontalAlign="Left"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="LastName" HeaderText="Last Name">
														<HeaderStyle ForeColor="White"></HeaderStyle>
														<ItemStyle HorizontalAlign="Left"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="BounceReason" HeaderText="Bounce Reason">
														<HeaderStyle ForeColor="White"></HeaderStyle>
														<ItemStyle HorizontalAlign="Left"></ItemStyle>
													</asp:BoundColumn>
												</Columns>
												<PagerStyle NextPageText="&lt;span style=&quot;color:white;&quot;&gt;&amp;nbsp;Next&lt;/span&gt;"
													PrevPageText="&lt;span style=&quot;color:white&quot;&gt;Prev&amp;nbsp;&lt;/span&gt;" HorizontalAlign="Right"
													ForeColor="White" BackColor="#299EE4"></PagerStyle>
											</asp:DataGrid>
										</td>
									</tr>
								</table>
								<!-- end table -->
								<div align="center" style="MARGIN:20px 0px">
									<table width="340" border="0" cellspacing="0" cellpadding="0">
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
