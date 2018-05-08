<%@ Control Language="c#" Inherits="ecn.accounts.includes.ContactEditor" Codebehind="ContactEditor.ascx.cs" %>
<TABLE>
	<TR>
		<TD class="tableHeader" align="right" width="119" style="WIDTH: 119px; HEIGHT: 24px">Salutation</TD>
		<TD style="HEIGHT: 24px"><asp:DropDownList ID="ddlSalutation" Runat="server" CssClass="formfield">
				<asp:ListItem Value="Mr" Selected="True">Mr.</asp:ListItem>
				<asp:ListItem Value="Ms">Ms.</asp:ListItem>
			</asp:DropDownList>
		</TD>
	</TR>
	<TR>
		<TD class="tableHeader" vAlign="top" align="right" style="WIDTH: 119px">
			Contact</TD>
		<TD>
			<asp:textbox CssClass="formfield" id="txtFirstName" Size="50" runat="server" Width="128px"></asp:textbox>
			<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirstName" ErrorMessage="Contact is required field.">*</asp:RequiredFieldValidator>
			<asp:textbox CssClass="formfield" id="txtLastName" runat="server" Size="50" Width="128px"></asp:textbox>
			<asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" ControlToValidate="txtLastName" ErrorMessage="*"> </asp:RequiredFieldValidator></TD>
		<td></td>
	</TR>
	<TR>
		<TD class="tableHeader" vAlign="top" align="right" style="WIDTH: 119px">Title</TD>
		<TD><asp:textbox CssClass="formfield" id="ContactTitle" runat="server" Size="50"></asp:textbox></TD>
		<td>
			<asp:RequiredFieldValidator id="Requiredfieldvalidator2" runat="server" ErrorMessage="Title is required field."
				ControlToValidate="ContactTitle">*</asp:RequiredFieldValidator></td>
	</TR>
	<TR>
		<TD class="tableHeader" vAlign="top" align="right" style="WIDTH: 119px">Phone</TD>
		<TD><asp:textbox CssClass="formfield" id="Phone" runat="server" Size="50"></asp:textbox></TD>
		<td>
			<asp:RequiredFieldValidator id="Requiredfieldvalidator3" runat="server" ErrorMessage="Phone is required field."
				ControlToValidate="Phone">*</asp:RequiredFieldValidator></td>
	</TR>
	<TR>
		<TD class="tableHeader" vAlign="top" align="right" style="WIDTH: 119px">Fax</TD>
		<TD><asp:textbox CssClass="formfield" id="Fax" runat="server" Size="50"></asp:textbox></TD>
		<td></td>
	</TR>
	<TR>
		<TD class="tableHeader" vAlign="top" align="right" style="WIDTH: 119px">Email</TD>
		<TD><asp:textbox CssClass="formfield" id="Email" runat="server" Size="50"></asp:textbox></TD>
		<td>
			<asp:RequiredFieldValidator id="Requiredfieldvalidator5" runat="server" ErrorMessage="Email is required field."
				ControlToValidate="Email">*</asp:RequiredFieldValidator></td>
	</TR>
	<TR>
		<TD class="tableHeader" vAlign="top" align="right" style="WIDTH: 119px">Address</TD>
		<TD><asp:textbox CssClass="formfield" id="Address" runat="server" Size="50"></asp:textbox></TD>
		<td>
			<asp:RequiredFieldValidator id="Requiredfieldvalidator6" runat="server" ErrorMessage="Addres is required field."
				ControlToValidate="Address">*</asp:RequiredFieldValidator></td>
	</TR>
	<TR>
		<TD class="tableHeader" vAlign="top" align="right" style="WIDTH: 119px">City</TD>
		<TD><asp:textbox CssClass="formfield" id="City" runat="server" Size="50"></asp:textbox></TD>
		<td>
			<asp:RequiredFieldValidator id="Requiredfieldvalidator7" runat="server" ErrorMessage="City is required field."
				ControlToValidate="City">*</asp:RequiredFieldValidator></td>
	</TR>
	<TR>
		<TD class="tableHeader" vAlign="top" align="right" style="WIDTH: 119px">State</TD>
		<TD>
			<asp:DropDownList id="ddlState" Runat="server" CssClass="formfield">
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
			</asp:DropDownList></TD>
		<td>
			<asp:RequiredFieldValidator id="Requiredfieldvalidator8" runat="server" ErrorMessage="State is required field."
				ControlToValidate="ddlState">*</asp:RequiredFieldValidator></td>
	</TR>
	<TR>
		<TD class="tableHeader" vAlign="top" align="right" style="WIDTH: 119px">Zip</TD>
		<TD><asp:textbox CssClass="formfield" id="Zip" runat="server" Size="50"></asp:textbox></TD>
		<td>
			<asp:RequiredFieldValidator id="Requiredfieldvalidator9" runat="server" ErrorMessage="Zip Code is required field."
				ControlToValidate="Zip">*</asp:RequiredFieldValidator></td>
	</TR>
	<TR>
		<TD class="tableHeader" vAlign="top" align="right" style="WIDTH: 119px">Country</TD>
		<TD><asp:textbox CssClass="formfield" id="Country" runat="server" Size="50"></asp:textbox></TD>
		<td>
			<asp:RequiredFieldValidator id="Requiredfieldvalidator10" runat="server" ErrorMessage="Country is required field."
				ControlToValidate="Country">*</asp:RequiredFieldValidator></td>
	</TR>
</TABLE>
