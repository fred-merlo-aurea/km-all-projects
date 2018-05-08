<%@ Page language="c#" Codebehind="ReportingBounces.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard.Reports.ReportingBounces" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<center>
	<form class="body_bg" id="Form1" name="frm" method="post" runat="server">
		<table id="Table4" cellSpacing="0" cellPadding="0" width="718" border="0">
			<tr>
				<td vAlign="top" width="154"><IMG id="imgSelection" src="../images/bounces.gif" runat="server"></td>
				<td vAlign="top" align="left" width="564">
					<!--content-->
					<div style="PADDING-RIGHT: 20px; PADDING-LEFT: 10px; FONT-SIZE: 12px; PADDING-TOP: 10px">
						<div class="dashed_lines1" style="FONT-SIZE: 14px">
							<table cellSpacing="0" cellPadding="0" width="100%">
								<tr>
									<td align="left"><strong>Reporting-Bounces:</strong></td>
									<td align="right"><asp:ImageButton id=ImageButton1 src="../images/btn_unsubscribe.gif" runat="server" onclick="UnsubscribeBounces"></asp:ImageButton>&nbsp;</td>
								</tr>
							</table>
						</div>
						<div style="PADDING-RIGHT: 70px; PADDING-LEFT: 20px; PADDING-BOTTOM: 0px; PADDING-TOP: 10px">Bounces 
							are emails that were not delivered to the recipient. This can occur for a 
							variety of reasons, including an invalid email address, and address box that is 
							full, etc.<br>
							<br>
							<b>Displays:</b>
							<P></P>
							<UL>
								<LI>
								Date and time that the email bounced
								<LI>
								The email address of the recipient
								<LI>
								Type of bounce
								<LI>
									Reason for the bounce
								</LI>
							</UL>
						</div>
					</div>
					<!--eof content--></td>
			</tr>
			<!--reports tables row-->
			<tr>
				<td colSpan="2">
					<div style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; FONT-SIZE: 12px; PADDING-BOTTOM: 10px; PADDING-TOP: 10px"
						align="center">
						<!--reports table-->
						<div style="WIDTH: 700px" align="center">
							<table cellSpacing="0" cellPadding="0" width="700" align="center" border="0">
								<TR>
									<td align="left">&nbsp; &nbsp;<b>Filter Bounces types</b>&nbsp;&nbsp;
										<asp:dropdownlist class="formfield" id="BounceType" runat="server" EnableViewState="true" AutoPostBack="true"
											visible="true" onselectedindexchanged="BounceType_SelectedIndexChanged">
											<asp:ListItem selected="true" Value="*">All types</asp:ListItem>
											<asp:ListItem Value="hard">Hard</asp:ListItem>
											<asp:ListItem Value="soft">Soft</asp:ListItem>
											<asp:ListItem Value="blocked">Blocked</asp:ListItem>
											<asp:ListItem Value="notify">Notify</asp:ListItem>
											<asp:ListItem Value="resend">Resends</asp:ListItem>
											<asp:ListItem Value="U">Unsubscribed</asp:ListItem>
											<asp:ListItem Value="unknown">Unknown</asp:ListItem>
										</asp:dropdownlist></td>
									<td align="right"><b>Download Current View</b>&nbsp;&nbsp;
										<asp:dropdownlist id="ddlType" EnableViewState="True" Runat="server">
											<asp:ListItem Selected="True" Value=".csv">CSV File</asp:ListItem>
											<asp:ListItem Value=".txt">Text File</asp:ListItem>
										</asp:dropdownlist>&nbsp;&nbsp;
										<asp:imagebutton id="btnDownload" Runat="server" ImageUrl="../images/btn_download.gif"></asp:imagebutton></td>
								</TR>
								<tr>
									<td colSpan="2"><asp:datagrid id="dgBounces" Runat="server" AutoGenerateColumns="False"   CellPadding="2" BackColor="#eeeeee" width="100%">
											<AlternatingItemStyle BackColor="White" />
											<ItemStyle CssClass="tableContentSmall"></ItemStyle>
											<HeaderStyle CssClass="tableHeader1"></HeaderStyle>
											<Columns>
												<asp:BoundColumn ItemStyle-Width="17%" headerstyle-width="17%" DataField="ActionDate" HeaderText="Bounce Time"></asp:BoundColumn>
												<asp:BoundColumn ItemStyle-Width="33%" DataField="EmailAddress" HeaderText="Email Address"></asp:BoundColumn>
												<asp:BoundColumn ItemStyle-Width="10%" DataField="ActionValue" HeaderText="Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
												<asp:BoundColumn ItemStyle-Width="40%" DataField="ActionNotes" HeaderText="Bounce Reason"></asp:BoundColumn>
											</Columns>
										</asp:datagrid>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<AU:PAGERBUILDER id="BouncesPager" Runat="server" PageSize="50" Width="100%" ControlToPage="dgBounces" onindexchanged="BouncesPager_IndexChanged">
											<PagerStyle CssClass="tableContent"></PagerStyle>
										</AU:PAGERBUILDER>
									</td>
								</tr>								
							</table>
						</div>
						<!--eof reports tables-->
						<div style="PADDING-TOP: 10px" align="center">
							<table cellSpacing="0" cellPadding="0">
								<tr>
									<td vAlign="top"><asp:imagebutton id="btnPrevious" runat="server" ImageUrl="../images/btn_back_to_previous_page.gif"></asp:imagebutton></td>
									<td vAlign="top"><asp:imagebutton id="btnReportMenu" runat="server" ImageUrl="../images/btn_back_to_report_menu.gif"></asp:imagebutton></td>
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
