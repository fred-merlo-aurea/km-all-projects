<%@ Control Language="c#" Inherits="ecn.accounts.includes.EmailNotes" Codebehind="EmailNotes.ascx.cs" %>
<table width="200px" border="0" cellpadding="2" class="tableContent" bgcolor="#f4f4f4" style="BORDER-RIGHT:#cccccc 1px solid; BORDER-TOP:#cccccc 1px solid; BORDER-LEFT:#cccccc 1px solid; BORDER-BOTTOM:#cccccc 1px solid">
	<tr>
		<td bgcolor="#281163"><span style="COLOR:white">Email 
				Notes:</span></td>
		<!--<td width="10"></td>-->
	</tr>
	<tr>
		<td align=center><div align=left>Public Notes:</div>
			<asp:TextBox ID="txtNotes" TextMode="MultiLine" Runat="server" Rows="5" Columns="34" CssClass=formtextfield></asp:TextBox>
		</td>
	</tr>
	<tr><td height=2></td></tr>
	<tr>
		<td align=center><div align=left>Internal Notes:</div>
			<asp:TextBox ID="txtInternalNotes" TextMode="MultiLine" Runat="server" Rows="5" Columns="34" CssClass=formtextfield></asp:TextBox>
		</td>
	</tr>
</table>
