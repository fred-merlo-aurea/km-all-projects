<%@ Page language="c#" Codebehind="ReportingUnSubscribes.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard.Reports.ReportingUnSubscribes" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<center>
	<form name="frm" method="post" id="Form1" class="body_bg" runat="server">
		<table width="718" cellpadding="0" cellspacing="0" border="0" ID="Table4">
			<tr>
				<td valign="top" width="154"><img src="../images/Unsubscribes.gif" runat="server" id="imgSelection"></td>
				<td width="564" valign="top" align="left">
					<!--content-->
					<div style="PADDING-RIGHT:20px; PADDING-LEFT:10px; FONT-SIZE:12px; PADDING-TOP:10px">
						<div style="PADDING-RIGHT:0px; FONT-SIZE:12px; PADDING-BOTTOM:0px; PADDING-TOP:0px" class="dashed_lines1">
							<strong>Reporting-Unsubscribers:</strong></div>
						<div style="PADDING-RIGHT:70px; PADDING-LEFT:20px; PADDING-BOTTOM:0px; PADDING-TOP:10px">
							Identifies the person who has clicked on a unsubscribe link in your email.<BR>
							<br>
							<b>Displays:</b>
							<ul>
								<li>Date and time that the email was unsubscribed 
        
								<li>The email address of the sender</li>
							</ul>
						</div>
					</div>
					<!--eof content-->
				</td>
			</tr>
			<!--reports tables row-->
			<tr>
				<td colspan="2">
					<div align="center" style="PADDING-RIGHT:0px; PADDING-LEFT:0px; FONT-SIZE:12px; PADDING-BOTTOM:10px; PADDING-TOP:10px; BORDER-BOTTOM:#999999 1px dashed">
						<!--reports table-->
						<div align="center" style="WIDTH:700px">
							<asp:DataGrid Width="700px" ID="dgUnsubscribes" AutoGenerateColumns="False" Runat="server"  CellPadding="2" BackColor="#eeeeee">
											<AlternatingItemStyle BackColor="White" />
											<ItemStyle CssClass="tableContentSmall"></ItemStyle>
											<HeaderStyle CssClass="tableHeader1"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="unsubscribeTime" HeaderText="Time"  headerstyle-width="17%" itemstyle-width="17%"></asp:BoundColumn>
									<asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address" ItemStyle-Width="83%"></asp:BoundColumn>
								</Columns>
							</asp:DataGrid>
							<AU:PAGERBUILDER id="UnsubscribesPager" Runat="server" PageSize="50" Width="100%" ControlToPage="dgUnsubscribes">
								<PAGERSTYLE CssClass="tableContent"></PAGERSTYLE>
							</AU:PAGERBUILDER>
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
	</form>
</center>
