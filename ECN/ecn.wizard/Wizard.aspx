<%@ Page language="c#" Codebehind="Wizard.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard.Wizard" ValidateRequest="false" %>
<center>
	<form name="frm" method="post" id="frm" class="body_bg" runat="server">
		<table width="718" cellpadding="0" cellspacing="0" border="0" ID="Table4">
			<tr>
				<td valign="top" width="154"><asp:Image ID="wizImage" Runat="server"></asp:Image></td>
				<td width="564" valign="top" align="left">
					<div style="PADDING-LEFT:20px">
						<asp:PlaceHolder ID="phwizContent" Runat="server"></asp:PlaceHolder>&nbsp;
						<br>
						<div align="center">
							<table cellpadding="0" cellspacing="0">
								<tr>
									<td valign="top"><asp:ImageButton ImageUrl="images/btn_previous.gif" runat="server" ID="btnBack" CausesValidation="False"></asp:ImageButton>&nbsp;</td>
									<td valign="top"><asp:ImageButton ImageUrl="images/btn_continue.gif" runat="server" ID="btnNext"></asp:ImageButton></td>
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
