<%@ Page Language="c#" ContentType="text/html" ResponseEncoding="iso-8859-1" CodeBehind="ReportingClicks.aspx.cs" AutoEventWireup="false" Inherits="ecn.showcare.wizard.main.Reports.ReportingClicks" %>
<%@ register tagprefix="sc" TagName="button" src="btnShowcare.ascx"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
  <HEAD>
		<title>Showcare</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<LINK href="../style.css" type="text/css" rel="stylesheet">
  </HEAD>
	<body>
		<form runat="server">
			<div align="center">
				<div style="BACKGROUND: url(../../images/bg.gif) repeat-y; WIDTH: 752px">
					<table cellSpacing="0" cellPadding="0" width="752" border="0">
						<tr>
							<td colSpan="2"><IMG height="65" alt="Show Care" src="../../images/header.gif" width="752"></td>
						</tr>
						<tr>
							<td vAlign="top" align="right" width="265"><IMG height="201" alt="" src="../../images/img_reporting_clicks.gif" width="255"></td>
							<td vAlign="top" align="left" width="487"><br>
								<br>
								<P class="rightpaddingfortext">
									<b>Clicks</b><br>
									Identifies the person who has clicked on a link in your email, and the link 
									that was clicked.<br>
									<br>
									<b>Displays:</b>
								</P>
								<UL>
									<LI>
										<DIV class="rightpaddingfortext">The most frequently clicked links
										</DIV>
									<LI>
										<DIV class="rightpaddingfortext">The people who have clicked the most
										</DIV>
									<LI>
										<DIV class="rightpaddingfortext">Date and time that the link was clicked
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
										<DIV class="rightpaddingfortext">Phone number of the recipient
										</DIV>
									<LI>
										<DIV class="rightpaddingfortext">URL of the link that was clicked
										</DIV>
									</LI>
								</UL>
							</td>
						</tr>
						<tr>
							<td align="center" colSpan="2">
								<!-- tables go here. make sure it's 650px wide-->
								<table cellSpacing="0" cellPadding="0" width="650" border="0">
									<tr>
										<td align="left"><b>Top Click Throughs</b></td>
									</tr>
									<tr>
										<td class="leftpaddingfortables"><asp:datagrid id="dgTopClicks" AutoGenerateColumns="False" Width="645px" Runat="server">
												<AlternatingItemStyle BackColor="#EDFFC7"></AlternatingItemStyle>
												<HeaderStyle HorizontalAlign="Center" ForeColor="White" BackColor="#299EE4"></HeaderStyle>
												<Columns>
													<asp:BoundColumn DataField="ClickCount" HeaderText="Click Count">
														<HeaderStyle ForeColor="White"></HeaderStyle>
														<ItemStyle HorizontalAlign="Center"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="Url" HeaderText="URL">
														<HeaderStyle ForeColor="White"></HeaderStyle>
														<ItemStyle HorizontalAlign="Left"></ItemStyle>
													</asp:BoundColumn>
												</Columns>
											</asp:datagrid></td>
									</tr>
								</table>
								<br>
								<br>
								<table cellSpacing="0" cellPadding="0" width="650" border="0">
									<tr>
										<td align="left"><b>Top Visitors</b></td>
									</tr>
									<tr>
										<td><asp:datagrid id="dgTopVisitors" AutoGenerateColumns="False" Width="645px" Runat="server">
												<AlternatingItemStyle BackColor="#EDFFC7"></AlternatingItemStyle>
												<HeaderStyle HorizontalAlign="Center" ForeColor="White" BackColor="#299EE4"></HeaderStyle>
												<Columns>
													<asp:BoundColumn DataField="ClickCount" HeaderText="Click Count">
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
													<asp:BoundColumn DataField="Phone" HeaderText="Phone">
														<HeaderStyle ForeColor="White"></HeaderStyle>
														<ItemStyle HorizontalAlign="Center"></ItemStyle>
													</asp:BoundColumn>
												</Columns>
											</asp:datagrid></td>
									</tr>
								</table>
								<br>
								<br>
								<table cellSpacing="0" cellPadding="0" width="650" border="0">
									<tr>
										<td align="left"><b>Click-Throughs by Person</b></td>
										<td align="right"><b>Download Email Addresses as&nbsp;&nbsp;</b>
											<asp:dropdownlist id="ddlDLType" Runat="server" EnableViewState="True">
												<asp:ListItem Selected="True" Value=".csv">CSV File</asp:ListItem>
												<asp:ListItem Value=".txt">Text File</asp:ListItem>
											</asp:dropdownlist>&nbsp;
											<asp:imagebutton id="btnDownload" Runat="server" ImageUrl="../../images/btn_download.gif"></asp:imagebutton></td>
									</tr>
									<tr>
										<td colSpan="2"><asp:datagrid id="dgClicksPerPerson" AutoGenerateColumns="False" Width="645px" Runat="server"
												AllowPaging="True">
												<AlternatingItemStyle BackColor="#EDFFC7"></AlternatingItemStyle>
												<HeaderStyle HorizontalAlign="Center" ForeColor="White" BackColor="#299EE4"></HeaderStyle>
												<Columns>
													<asp:BoundColumn DataField="ClickTime" HeaderText="Click Time">
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
													<asp:BoundColumn DataField="Phone" HeaderText="Phone">
														<HeaderStyle ForeColor="White"></HeaderStyle>
														<ItemStyle HorizontalAlign="Center"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="Url" HeaderText="URL">
														<HeaderStyle ForeColor="White"></HeaderStyle>
														<ItemStyle HorizontalAlign="Left"></ItemStyle>
													</asp:BoundColumn>
												</Columns>
												<PagerStyle NextPageText="&lt;span style=&quot;color:white;&quot;&gt;&amp;nbsp;Next&lt;/span&gt;"
													PrevPageText="&lt;span style=&quot;color:white&quot;&gt;Prev&amp;nbsp;&lt;/span&gt;" HorizontalAlign="Right"
													ForeColor="White" BackColor="#299EE4" CssClass="pagerLink"></PagerStyle>
											</asp:datagrid></td>
									</tr>
								</table>
								<!-- end table -->
								<div style="MARGIN: 20px 0px" align="center">
									<table cellSpacing="0" cellPadding="0" width="340" border="0">
										<tr>
											<td><sc:button id="sc1" runat=server></sc:button></td>
											<td><IMG height="41" src="../../images/line_separator.gif" width="8"></td>
											<td><asp:imagebutton id="backtoMain" runat="server" ImageUrl="../../images/btn_backto_main.gif"></asp:imagebutton></td>
										</tr>
									</table>
								</div>
							</td>
						</tr>
						<tr>
							<td align="center" colSpan="2" height="22" class="footer">Need Help? Click here for <A style="COLOR: #ffff00" href="http://www.showlead.com/help.aspx" target="_blank">
									assistance.</A>
							</td>
						</tr>
					</table>
				</div>
			</div>
		</form>
	</body>
</HTML>
