<%@ Control Language="c#" Inherits="ecn.accounts.includes.ContactEditor2" Codebehind="ContactEditor2.ascx.cs" %>
<table border=0 cellpadding=2 cellspacing=0>
	<tr>
		<td align="right" class="tableContent">First Name</td>
		<td vAlign="middle" colSpan="3"  class="tableContent"><asp:textbox id="ContactName" Width="104px" runat="server" Size="50"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="ContactName" ErrorMessage="Contact is required field.">*</asp:requiredfieldvalidator>&nbsp;&nbsp; 
			Last Name&nbsp;<asp:textbox id="txtLastName" Width="104px" runat="server" Size="50"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator8" runat="server" ControlToValidate="txtLastName" ErrorMessage="Contact is required field.">*</asp:requiredfieldvalidator></td>
	</tr>
	<TR>
		<TD style="WIDTH: 119px" align="right"  class="tableContent">Title</TD>
		<TD colSpan="3"><asp:textbox id="ContactTitle" runat="server" Size="50" CssClass="formtextfield"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" ControlToValidate="ContactTitle" ErrorMessage="Title is required field.">*</asp:requiredfieldvalidator></TD>
	</TR>
	<tr>
		<td align="right"  class="tableContent">Company</td>
		<td colSpan="3"><asp:textbox id="txtCompany" runat="server" Size="50" CssClass="formtextfield"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator4" runat="server" ControlToValidate="txtCompany" ErrorMessage="Title is required field.">*</asp:requiredfieldvalidator></td>
	</tr>
	<TR>
		<TD align="right" class="tableContent">Email</TD>
		<TD colSpan="3"><asp:textbox id="Email" runat="server" Size="50" CssClass="formtextfield"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator5" runat="server" ControlToValidate="Email" ErrorMessage="Email is required field.">*</asp:requiredfieldvalidator></TD>
	</TR>
	<tr>
		<td align="right" class="tableContent">Address</td>
		<td colSpan="3"><asp:textbox id="Address" runat="server" Size="50" CssClass="formtextfield"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator6" runat="server" ControlToValidate="Address" ErrorMessage="Addres is required field.">*</asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td align="right" class="tableContent">City</td>
		<td colSpan="3"><asp:textbox id="City" runat="server" Size="50" CssClass="formtextfield"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator7" runat="server" ControlToValidate="City" ErrorMessage="City is required field.">*</asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td align="right" class="tableContent" valign=top>State</td>
		<td colSpan="3" class="tableContent"><asp:dropdownlist id="ddlState" Runat="server" CssClass="formfield">
				<asp:ListItem value="AK">AK</asp:ListItem>
				<asp:ListItem value="AL">AL</asp:ListItem>
				<asp:ListItem value="AR">AR</asp:ListItem>
				<asp:ListItem value="AZ">AZ</asp:ListItem>
				<asp:ListItem value="CA">CA</asp:ListItem>
				<asp:ListItem value="CO">CO</asp:ListItem>
				<asp:ListItem value="CT">CT</asp:ListItem>
				<asp:ListItem value="DC">DC</asp:ListItem>
				<asp:ListItem value="DE">DE</asp:ListItem>
				<asp:ListItem value="FL">FL</asp:ListItem>
				<asp:ListItem value="GA">GA</asp:ListItem>
				<asp:ListItem value="HI">HI</asp:ListItem>
				<asp:ListItem value="IA">IA</asp:ListItem>
				<asp:ListItem value="ID">ID</asp:ListItem>
				<asp:ListItem value="IL">IL</asp:ListItem>
				<asp:ListItem value="IN">IN</asp:ListItem>
				<asp:ListItem value="KS">KS</asp:ListItem>
				<asp:ListItem value="KY">KY</asp:ListItem>
				<asp:ListItem value="LA">LA</asp:ListItem>
				<asp:ListItem value="MA">MA</asp:ListItem>
				<asp:ListItem value="MD">MD</asp:ListItem>
				<asp:ListItem value="ME">ME</asp:ListItem>
				<asp:ListItem value="MI">MI</asp:ListItem>
				<asp:ListItem value="MN">MN</asp:ListItem>
				<asp:ListItem value="MO">MO</asp:ListItem>
				<asp:ListItem value="MS">MS</asp:ListItem>
				<asp:ListItem value="MT">MT</asp:ListItem>
				<asp:ListItem value="NC">NC</asp:ListItem>
				<asp:ListItem value="ND">ND</asp:ListItem>
				<asp:ListItem value="NE">NE</asp:ListItem>
				<asp:ListItem value="NH">NH</asp:ListItem>
				<asp:ListItem value="NJ">NJ</asp:ListItem>
				<asp:ListItem value="NM">NM</asp:ListItem>
				<asp:ListItem value="NV">NV</asp:ListItem>
				<asp:ListItem value="NY">NY</asp:ListItem>
				<asp:ListItem value="OH">OH</asp:ListItem>
				<asp:ListItem value="OK">OK</asp:ListItem>
				<asp:ListItem value="OR">OR</asp:ListItem>
				<asp:ListItem value="PA">PA</asp:ListItem>
				<asp:ListItem value="RI">RI</asp:ListItem>
				<asp:ListItem value="SC">SC</asp:ListItem>
				<asp:ListItem value="SD">SD</asp:ListItem>
				<asp:ListItem value="TN">TN</asp:ListItem>
				<asp:ListItem value="TX">TX</asp:ListItem>
				<asp:ListItem value="UT">UT</asp:ListItem>
				<asp:ListItem value="VA">VA</asp:ListItem>
				<asp:ListItem value="WA">WA</asp:ListItem>
				<asp:ListItem value="WI">WI</asp:ListItem>
				<asp:ListItem value="WV">WV</asp:ListItem>
				<asp:ListItem value="WY">WY</asp:ListItem>
			</asp:dropdownlist>&nbsp;&nbsp;&nbsp;Zip &nbsp;
			<asp:textbox id="Zip" Width="96px" runat="server" Size="50" MaxLength="10" CssClass="formtextfield"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator9" runat="server" ControlToValidate="Zip" ErrorMessage="Zip Code is required field.">*</asp:requiredfieldvalidator>&nbsp;Country&nbsp;<asp:textbox id="Country" Width="66px" runat="server" Size="50"></asp:textbox>
			<asp:requiredfieldvalidator id="Requiredfieldvalidator10" runat="server" ControlToValidate="Country" ErrorMessage="Country is required field.">*</asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td align="right" class="tableContent">Phone</td>
		<td class="tableContent"><asp:textbox id="Phone" Width="160px" runat="server" Size="50" CssClass="formtextfield"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" ControlToValidate="Phone" ErrorMessage="Phone is required field.">*</asp:requiredfieldvalidator></td>
		<td colSpan="2" rowSpan="2" class="tableContent"><asp:checkbox id="chkIsTechInfo" Runat="server" TextAlign="Right" Text="Use this as my technical contact"></asp:checkbox><br>
			<asp:checkbox id="chkIsBillingAddress" Runat="server" TextAlign="Right" Text="Use this as my billing address"
				Checked="True"></asp:checkbox></td>
	</tr>
	<tr>
		<td align="right" class="tableContent">Fax</td>
		<td><asp:textbox id="Fax" Width="160px" runat="server" Size="50" CssClass="formtextfield"></asp:textbox></td>
	</tr>
	<tr>
		<td colSpan="5"><asp:validationsummary id="ValidationSummary1" Width="464px" runat="server"></asp:validationsummary></td>
	</tr>
</table>
