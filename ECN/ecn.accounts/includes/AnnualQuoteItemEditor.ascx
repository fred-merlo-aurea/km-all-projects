<%@ Control Language="c#" Inherits="ecn.accounts.includes.AnnualQuoteItemEditor" Codebehind="AnnualQuoteItemEditor.ascx.cs" %>
<table class="tableContent" style="BORDER-RIGHT: #cccccc 1px solid; BORDER-TOP: #cccccc 1px solid; BORDER-LEFT: #cccccc 1px solid; BORDER-BOTTOM: #cccccc 1px solid"
	cellPadding="0" width="200px" bgColor="#f4f4f4" border="0">
	<tr>
		<td colspan="2" style="PADDING-RIGHT:2px;PADDING-LEFT:2px;BACKGROUND:#281163;PADDING-BOTTOM:2px;PADDING-TOP:2px; color:white;width=100%">
			Annual 	Technology Access</td>
	</tr>
	<tr>
		<td colSpan="2">Type:</td>
	</tr>
	<tr>
		<td colSpan="2"><asp:dropdownlist id="ddlAnnualTechOptions" Width="100%" Runat="server" AutoPostBack="True" class="formfield" onselectedindexchanged="ddlAnnualTechOptions_SelectedIndexChanged"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td width="40%">Rate:</td>
		<td>$&nbsp;<asp:textbox id="txtRate" Width="72px" Runat="server" Enabled="False"  class="formfield"></asp:textbox></td>
	</tr>
	<tr>
		<td width="40%">Discount Rate:</td>
		<td>&nbsp;&nbsp;&nbsp;<asp:textbox id="txtDiscountRate" Width="26px" Runat="server" MaxLength="3"  class="formfield">0</asp:textbox>%
			<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txtDiscountRate">*</asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td><asp:checkbox id="chkIsActive" Text="Active" TextAlign="Left" runat="server" Checked="True"></asp:checkbox></td>
		<td>	<asp:LinkButton id="lnkUpdate" runat="server" CausesValidation="False" onclick="lnkUpdate_Click">Update Rate</asp:LinkButton></td>
	</tr>
</table>
