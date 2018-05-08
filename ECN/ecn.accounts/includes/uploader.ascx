<%@ Control language="c#" Inherits="ecn.accounts.includes.uploader" Codebehind="uploader.ascx.cs" %>
<table border=0>
	<tr>
		<td>
			<INPUT class="formfield" id="FindFile" style="WIDTH: 274px; HEIGHT: 22px" type="file" size="26" runat="server">
		</td>
	</tr>
	<tr>
		<td>
			<asp:listbox id="FilesListBox" runat="server" CssClass="formfield" Height="100px" Width="274px" Font-Size="XX-Small"></asp:listbox>
		</td>
	</tr>
	<tr>
		<td>
			<asp:button id="AddFile" runat="server" CssClass="formbutton" Height="23px" Width="72px" Text="Add" onclick="AddFile_Click"></asp:button>
			<asp:button id="RemvFile" runat="server" CssClass="formbutton" Height="23px" Width="72px" Text="Remove" onclick="RemvFile_Click"></asp:button>
			<INPUT class="formbutton" id="Upload" style="WIDTH: 71px; HEIGHT: 24px" type="submit" value="Upload" runat="server" onserverclick="Upload_ServerClick">
		</td>
	</tr>
	<tr>
		<td>
			<asp:label id="MessageLabel" runat="server" Height="25px" Width="249px" class=tableContent></asp:label>
			<asp:TextBox ID="uploadpath" Runat="server" Visible="False"></asp:TextBox>
		</td>
	</tr>
</table>
