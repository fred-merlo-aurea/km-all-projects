<%@ Control Language="c#" Inherits="ecn.accounts.includes.UserInfoCollector" Codebehind="UserInfoCollector.ascx.cs" %>
<table border=0 cellpadding=2 cellspacing=0>
	<TR>
		<td align="left" class=tableContent><div>Email Address</div>
			<asp:textbox id="txtUserName" runat="server" columns="40" CssClass="formtextfield"></asp:textbox><asp:requiredfieldvalidator id="val_UserName" runat="server" CssClass="errormsg" display="Static" ErrorMessage="User Name is a required field."
				ControlToValidate="txtUserName">*</asp:requiredfieldvalidator></TD>
	</TR>
	<TR>
		<td align="left" class=tableContent><div>Password</div>
			<asp:textbox id="txtPassword1" runat="server" columns="40" TextMode="Password" CssClass="formtextfield"></asp:textbox><asp:requiredfieldvalidator id="val_Password" runat="server" CssClass="errormsg" display="Static" ErrorMessage="Password is a required field."
				ControlToValidate="txtPassword1">*</asp:requiredfieldvalidator></TD>
	</TR>
	<TR>
		<td align="left" class=tableContent valign=top><div>Confirm Password</div>
			<asp:textbox id="txtPassword2" runat="server" columns="40" TextMode="Password" CssClass="formtextfield"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" CssClass="errormsg" display="Static"
				ErrorMessage="Confirm Passowrd is a required field." ControlToValidate="txtPassword2">*</asp:requiredfieldvalidator>
			<div align=center>			
			<asp:CompareValidator id="CompareValidator1" runat="server" ErrorMessage="The passwords you typed do not match. Please re-type passwords in both text boxes."
				ControlToValidate="txtPassword2" ControlToCompare="txtPassword1"></asp:CompareValidator></div>
		</TD>
	</TR>
</table>
