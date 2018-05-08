<%@ Control Language="c#" Inherits="ecn.accounts.includes.TestingAccount" Codebehind="TestingAccount.ascx.cs" %>
<table class="tableContent" style="COLOR: #6699cc" width=150px>
	<tr>
		<td align="center"><span style="FONT-SIZE: 14pt">Trial Account</span></td>
	</tr>
	<tr>
		<td align="center">
			<asp:Button id="btnLogin" Text="Login" runat="server" CausesValidation="False" onclick="btnLogin_Click"></asp:Button></td>
	</tr>
	<tr>
		<td align="center"><asp:Label ID="lblTermsAndConditions" Runat="server"></asp:Label></td>
	</tr>
	<tr>
		<td align="center"><asp:CheckBox ID="chkAgreeToTerms" Runat="server"></asp:CheckBox>
		</td>
	</tr>
	<tr>
		<td><asp:Label id="lblErrorMessage" runat="server" CssClass="errormsg" Visible="False">You need to agree to terms and conditions.</asp:Label></td>
	</tr>
</table>
