<%@ Page language="c#" Codebehind="ReportingClicks.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard.Reports.ReportingClicks" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>

<center>
	<form name="frm" method="post" id="Form1" class="body_bg" runat="server">
		<table width="718" cellpadding="0" cellspacing="0" border="0" ID="Table4">
			<tr>
				<td valign="top" width="154"><img src="../images/clicks.gif" runat="server" id="imgSelection"></td>
				<td width="564" valign="top" align="left">
					<!--content-->
					<div style="PADDING-RIGHT:20px; PADDING-LEFT:10px; FONT-SIZE:12px; PADDING-TOP:10px">
						<div style="PADDING-RIGHT:0px; FONT-SIZE:12px; PADDING-BOTTOM:0px; PADDING-TOP:0px" class="dashed_lines1">
							<strong>Reporting-Clicks:</strong></div>
						<div style="PADDING-RIGHT:70px; PADDING-LEFT:20px; PADDING-BOTTOM:0px; PADDING-TOP:10px">
							Identifies the person who has clicked on a link in your email, and the link 
							that was clicked.<br>
							<br>
							<b>Displays:</b>
							<UL>
								<LI>
									The most frequently clicked links
									</LI>
								<LI>
									The people who have clicked the most
									</LI>
								<LI>
									Date and time that the link was clicked
									</LI>
								<LI>
									The email address of the recipient
									</LI>
								<LI>
									URL of the link that was clicked
									</LI>
							</UL>
						</div>
					</div>
					<!--eof content-->
				</td>
			</tr>
			<!--reports tables row-->
			<tr>
				<td colspan="2">
					<div align="center" style="PADDING-RIGHT:0px; MARGIN-TOP:10px; PADDING-LEFT:0px; FONT-SIZE:12px; PADDING-BOTTOM:10px; PADDING-TOP:10px">
						<!--reports table-->
						<div align="center" style="WIDTH:700px">
							<table cellSpacing="0" cellPadding="0" width="700" border="0">
								<tr>
									<td align="left"><b>Top Click Throughs</b></td>
								</tr>
								<tr>
									<td><asp:datagrid id="dgTopClicks" AutoGenerateColumns="False" Width="700px" Runat="server"  CellPadding="2" BackColor="#eeeeee">
											<AlternatingItemStyle BackColor="White" />
											<ItemStyle CssClass="tableContentSmall"></ItemStyle>
											<HeaderStyle CssClass="tableHeader1"></HeaderStyle>											<Columns>
												<asp:BoundColumn DataField="ClickCount" HeaderText="Click Count" headerstyle-width="17%" itemstyle-width="17%">
													<HeaderStyle HorizontalAlign="Center" ForeColor="White"></HeaderStyle>
													<ItemStyle HorizontalAlign="Center"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Url" HeaderText="URL"  itemstyle-width="83%"></asp:BoundColumn>
											</Columns>
										</asp:datagrid></td>
								</tr>
							</table>
							<br>
							<br>
							<table cellSpacing="0" cellPadding="0" width="700" border="0">
								<tr>
									<td align="left"><b>Top Visitors</b></td>
								</tr>
								<tr>
									<td><asp:datagrid id="dgTopVisitors" AutoGenerateColumns="False" Width="700px" Runat="server"  CellPadding="2" BackColor="#eeeeee">
											<AlternatingItemStyle BackColor="White" />
											<ItemStyle CssClass="tableContentSmall"></ItemStyle>
											<HeaderStyle CssClass="tableHeader1"></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="ClickCount" HeaderText="Click Count" headerstyle-width="17%" itemstyle-width="17%">
													<HeaderStyle HorizontalAlign="Center" ForeColor="White"></HeaderStyle>
													<ItemStyle HorizontalAlign="Center"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address" itemstyle-width="83%"></asp:BoundColumn>
											</Columns>
											<PagerStyle Font-Size="12px" BackColor="#A6A6A6"></PagerStyle>
										</asp:datagrid></td>
								</tr>
							</table>
							<br>
							<br>
							<table cellSpacing="0" cellPadding="0" width="700" border="0">
								<tr>
									<td align="left"><b>Click-Throughs by Person</b></td>
									<td align="right"><b>Download Email Addresses as&nbsp;&nbsp;</b>
										<asp:dropdownlist id="ddlDLType" Runat="server" EnableViewState="True">
											<asp:ListItem Selected="True" Value=".csv">CSV File</asp:ListItem>
											<asp:ListItem Value=".txt">Text File</asp:ListItem>
										</asp:dropdownlist>&nbsp;
										<asp:imagebutton id="btnDownload" Runat="server" ImageUrl="../images/btn_download.gif"></asp:imagebutton></td>
								</tr>
								<tr>
									<td colSpan="2"><asp:datagrid id="dgClicksPerPerson" AutoGenerateColumns="False" Width="700px" Runat="server" CellPadding="2" BackColor="#eeeeee">
											<AlternatingItemStyle BackColor="White" />
											<ItemStyle CssClass="tableContentSmall"></ItemStyle>
											<HeaderStyle CssClass="tableHeader1"></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="ClickTime" HeaderText="Click Time" headerstyle-width="17%" itemstyle-width="17%"></asp:BoundColumn>
												<asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address"></asp:BoundColumn>
												<asp:BoundColumn DataField="Url" HeaderText="URL"></asp:BoundColumn>
											</Columns>
										</asp:datagrid></td>
								</tr>

								<tr>
									<td colspan="2">
										<AU:PAGERBUILDER id="ClicksPager" Runat="server" PageSize="50" Width="100%" ControlToPage="dgClicksPerPerson">
											<PagerStyle CssClass="tableContent"></PagerStyle>
										</AU:PAGERBUILDER>
									</td>
								</tr>									
									
							</table>
						</div>
						<!--eof reports tables-->
						<div align="center" style="PADDING-TOP:10px">
							<table cellpadding="0" cellspacing="0">
								<tr>
									<td valign="top"><asp:ImageButton ImageUrl="../images/btn_back_to_previous_page.gif" runat="server" ID="btnPrevious"></asp:ImageButton></td>
									<td valign="top"><asp:ImageButton ImageUrl="../images/btn_back_to_report_menu.gif" runat="server" ID="btnReportMenu"></asp:ImageButton></td>
								</tr>
							</table>
						</div>
					</div>
				</td>
			</tr>
		</table>
		<br>
	</form>
</center>
