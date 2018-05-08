<%@ Page language="c#" Codebehind="ReportingMain.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard.Reports.ReportingMain" %>
<center>
	<form class="body_bg" id="Form1" name="frm" method="post" runat="server">
		<table id="Table4" cellSpacing="0" cellPadding="0" width="718" border="0">
			<tr>
				<td vAlign="top" width="154"><IMG id="imgSelection" src="../images/main.gif" runat="server"></td>
				<td vAlign="top" align="left" width="564">
					<div style="PADDING-RIGHT: 20px; PADDING-LEFT: 10px; FONT-SIZE: 12px; PADDING-BOTTOM: 10px">
						<div class="dashed_lines1" style="PADDING-RIGHT: 0px; FONT-SIZE: 12px; PADDING-BOTTOM: 0px; PADDING-TOP: 0px"><strong>Email 
								Reporting:</strong></div>
						<div style="PADDING-RIGHT: 20px; PADDING-LEFT: 20px; PADDING-BOTTOM: 0px"><BR>
							Welcome to the Email Wizard reporting page. Here, you can review a summary of 
							reports relating to your previously sent emails. Click on the Message name to 
							view your previously sent email.<br>
							<br>
							<strong>Click on the icon in the 'Reports' column to view Report details. <IMG src="../images/img_file_icon.gif"></strong><br>
							Here, you have the ability to view; opens, clicks, bounces, resends and 
							forwards.<br>
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
						<div style="WIDTH: 700px" align="center"><asp:datagrid id="dgReports" Runat="server" AutoGenerateColumns="False">
								<AlternatingItemStyle Font-Size="12px" BackColor="#EDF0F5"></AlternatingItemStyle>
								<ItemStyle Font-Size="12px"></ItemStyle>
								<HeaderStyle Font-Size="12px" HorizontalAlign="Center" ForeColor="White" BackColor="#A6A6A6"></HeaderStyle>
								<Columns>
									<asp:HyperLinkColumn DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="javascript:var w =window.open('PreviewEmail.aspx?ID={0}',null,'width=600,height=600,location=no')" DataTextField="MessageName" HeaderText="Message Name">
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
									<asp:HyperLinkColumn Text="&lt;img border='0' src='../images/img_file_icon.gif' alt='View Email Report'&gt;" DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="ReportingMsgDetail.aspx?ID={0}" HeaderText="Reports">
										<HeaderStyle ForeColor="White"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
									</asp:HyperLinkColumn>
								</Columns>
							</asp:datagrid></div>
						<!--eof reports tables--></div>
				</td>
			</tr>
		</table>
		<br>
	</form>
</center>
