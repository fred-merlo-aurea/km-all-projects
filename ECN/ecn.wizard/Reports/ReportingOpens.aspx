<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Page language="c#" Codebehind="ReportingOpens.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard.Reports.ReportingOpens" %>
<center>
	<form name="frm" method="post" id="Form1" class="body_bg" runat="server">
		<table width="718" cellpadding="0" cellspacing="0" border="0" ID="Table4">
			<tr>
				<td valign="top" width="154"><img src="../images/opens.gif" runat="server" id="imgSelection"></td>
				<td width="564" valign="top" align="left">
					<!--content-->
					<div style="PADDING-RIGHT:20px; PADDING-LEFT:10px; FONT-SIZE:12px; PADDING-TOP:10px">
						<div style="PADDING-RIGHT:0px; FONT-SIZE:12px; PADDING-BOTTOM:0px; PADDING-TOP:0px" class="dashed_lines1">
							<strong>Reporting-Opens:</strong></div>
						<div style="PADDING-RIGHT:70px; PADDING-LEFT:20px; PADDING-BOTTOM:0px; PADDING-TOP:10px">
							Lists information about who has opened your email. You may also use the 
							Download feature below to create a file from the Open information.<br>
							<BR>
							<B>Displays:</B>
							<UL>
								<LI>
									Date and time that the email was opened</LI>
								<LI>
									The email address of the recipient</LI>
							</UL>
						</div>
					</div>
					<!--eof content-->
				</td>
			</tr>
			<!--reports tables row-->
			<tr>
				<td colspan="2">
					<div align="center" style="PADDING-RIGHT:0px; PADDING-LEFT:0px; FONT-SIZE:12px; PADDING-BOTTOM:10px; PADDING-TOP:10px">
						<!--reports table-->
						<div align="center" style="WIDTH:700px">
							<table width="700" border="0" cellspacing="0" cellpadding="0">
								<tr>
									<td align="left">
										<b>Most Active Opens</b><br>
									</td>
								</tr>
								<tr>
									<td class="leftpaddingfortables">
										<asp:DataGrid ID="dgActive" AutoGenerateColumns="False" Width="700px" Runat="server" CellPadding="2"  BackColor="#eeeeee">
											<AlternatingItemStyle BackColor="White" />
											<ItemStyle CssClass="tableContentSmall"></ItemStyle>
											<HeaderStyle CssClass="tableHeader1"></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="ActionCount" HeaderText="Count" headerstyle-HorizontalAlign="Center" headerstyle-width="17%" itemstyle-width="17%">
													<ItemStyle HorizontalAlign="Center"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address">
												</asp:BoundColumn>
											</Columns>
										</asp:DataGrid>
									</td>
								</tr>
							</table>
							<br>
							<br>
							<table width="700" cellpadding="0" border="0" cellspacing="0">
								<tr>
									<td align="left">
										<b>List of Opens</b><br>
									</td>
									<td align="right">
										<b>Download Email Addresses as &nbsp;&nbsp;</b>
										<asp:DropDownList EnableViewState="True" ID="ddlDLType" Runat="server">
											<asp:ListItem Selected="True" Value=".csv">CSV File</asp:ListItem>
											<asp:ListItem Value=".txt">Text File</asp:ListItem>
										</asp:DropDownList>
										<asp:ImageButton ID="btnDl" Runat="server" ImageUrl="../images/btn_download.gif" />
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:DataGrid ID="dgOpens" AutoGenerateColumns="False" Runat="server" Width="700px"  CellPadding="2" BackColor="#eeeeee">
											<AlternatingItemStyle BackColor="White" />
											<ItemStyle CssClass="tableContentSmall"></ItemStyle>
											<HeaderStyle CssClass="tableHeader1"></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="OpenTime" HeaderText="Open Time" headerstyle-width="17%" itemstyle-width="17%"></asp:BoundColumn>
												<asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address"></asp:BoundColumn>
												<asp:BoundColumn ItemStyle-Width="50%" DataField="ActionValue" HeaderText="Info"></asp:BoundColumn>
											</Columns>
										</asp:DataGrid>
										<AU:PAGERBUILDER id="OpensPager" Runat="server" PageSize="50" Width="100%" ControlToPage="dgOpens">
											<PagerStyle CssClass="tableContent"></PagerStyle>
										</AU:PAGERBUILDER>										
									</td>
								</tr>
								<tr>
									<td colspan="2">

									</td>
								</tr>
							</table>
						</div>
						<!--eof reports tables-->
						<div align="center" style="PADDING-TOP:10px">
							<table cellpadding="0" cellspacing="0" border="0">
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
