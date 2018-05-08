<%@ Page language="c#" Codebehind="ListCreate.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard.ListCreate" %>
<center>
	<form name="frm" method="post" id="frm" class="body_bg" runat="server">
		<table width="718" cellpadding="0" cellspacing="0" border="0" ID="Table4">
			<tr>
				<td valign="top" width="154"><img src="images/img_create_new_list.gif" /></td>
				<td width="564" valign="top" align="left" style="PADDING-LEFT:20px">
					<div style="PADDING-RIGHT: 20px; PADDING-LEFT: 10px; FONT-SIZE: 12px; PADDING-BOTTOM: 10px"><BR>
						<div class="dashed_lines1" style=" FONT-SIZE: 12px"><strong>Create a New Email List</strong></div>
						<br>
						<asp:label id="lblError" Runat="server" ForeColor="Red" Font-Bold="True"></asp:label>
						<br>
						<div style="PADDING-LEFT: 20px; ">
							<table>
								<tr>
									<td align="left" valign="bottom">
										Name your New list:<br>
										<asp:textbox id="txtNewListName" runat="server" CssClass="blue_border_box"></asp:textbox>
									</td>
									<td align="left" valign="bottom">
										<asp:Button id="btnSave" runat="server" Text="Save" CssClass="blue_border_box" Height="20px" onclick="btnSave_Click"></asp:Button>
									</td>
									<td align="left" valign="bottom">
										<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="«Required" ControlToValidate="txtNewListName"></asp:requiredfieldvalidator>
										<asp:Label ID="lblMsg" Runat="server" Font-Size="11px"></asp:Label>
									</td>
								</tr>
							</table>
							<br>
							<br>
							<br>
							<br>
							<br>
							<br>
							<br>
							<br>
						</div>
					</div>
				</td>
			</tr>
		</table>
		<!--eof content-->
	</form>
</center>
