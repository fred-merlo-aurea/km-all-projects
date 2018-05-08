<%@ Page language="c#" Codebehind="Importer.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard.Importer1" %>
<center>
	<form name="frm" method="post" id="Form1" class="body_bg" runat="server">
		<table width="718" cellpadding="0" cellspacing="0" border="0" ID="Table4">
			<tr>
				<td valign="top" width="154"><img src="images/img_map_data.gif"></td>
				<td width="564" valign="top" align="left" >
					<div style="PADDING-RIGHT: 20px; PADDING-LEFT: 10px; FONT-SIZE: 12px; PADDING-BOTTOM: 10px"><BR>
						<div class="dashed_lines1" style=" FONT-SIZE: 14px"><strong>Map Data Fields</strong></div>
						<br>
						<div style="PADDING-RIGHT:10px; PADDING-LEFT:10px; FONT-SIZE:12px; PADDING-BOTTOM:0px; PADDING-TOP:10px">Choose 
							the data Fields from your document that match the names to the Left.<br>
							<span style="COLOR:#ff0000">Email address, first name and last name are required.</span>
						</div>
						<div style="PADDING-TOP:25px"><asp:Label ID="lblMessage" Runat="server"></asp:Label></div>
						<div style="PADDING-RIGHT:0px; PADDING-LEFT:10px; PADDING-BOTTOM:0px; PADDING-TOP:10px">
							<TABLE id="layoutWrapper" cellSpacing="0" cellPadding="0" width="504" border="0" style="WIDTH: 504px; HEIGHT: 231px">
								<TR>
									<asp:label id="errlabel" runat="server" CssClass="errormsg" visible="false"></asp:label>
									<asp:label id="msglabel" runat="server" CssClass="TableHeader" visible="false"></asp:label>
									<TD><BR>
										<TABLE id="dataCollectionTable" cellSpacing="1" cellPadding="3" align="center" border="0"
											Runat="server">
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD vAlign="middle" align="center" height="50">
									</TD>
								<TR>
									<TD>
										<asp:DataGrid id="dgdErrorReport" runat="server" CssClass="TableHeader" AutoGenerateColumns="False"
											Width="100%" Visible="False">
											<HeaderStyle CssClass="TableHeader" HorizontalAlign="Center" />
											<ItemStyle CssClass="TableContent" HorizontalAlign="Center" />
											<Columns>
												<asp:BoundColumn DataField="RowIndex" HeaderText="Row#"></asp:BoundColumn>
												<asp:BoundColumn DataField="EmailAddress" HeaderText="Email"></asp:BoundColumn>
												<asp:BoundColumn DataField="ErrorMessage" HeaderText="Error Message"></asp:BoundColumn>
											</Columns>
										</asp:DataGrid>
									</TD>
								</TR>
							</TABLE>
						</div>
						<div style="PADDING-RIGHT:0px; PADDING-LEFT:170px; PADDING-BOTTOM:10px; PADDING-TOP:10px">
							<asp:ImageButton ID="btnImport" Runat="server" ImageUrl="images/btn_submit.gif"></asp:ImageButton></div>
					</div>
				</td>
			</tr>
		</table>
	</form>
</center>
