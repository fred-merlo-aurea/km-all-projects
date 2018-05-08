<%@ Reference Control="~/includes/ContactEditor.ascx" %>
<%@ Reference Control="~/includes/ContactEditor2.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ContactEditor2" Src="ContactEditor2.ascx" %>
<%@ Control Language="c#" Inherits="ecn.accounts.includes.CCInfoCollector" Codebehind="CCInfoCollector.ascx.cs" %>
<%@ Register TagPrefix="uc1" TagName="ContactEditor" Src="ContactEditor.ascx" %>
<div align="left"><span style="FONT-SIZE: 14pt; COLOR: #6699cc; FONT-FAMILY: Arial">Credit 
		Card Information</span></div>
<table id="tblCreditCard" style="BORDER-RIGHT:#cccccc 1px solid; BORDER-TOP:#cccccc 1px solid; FONT-SIZE:11px; BORDER-LEFT:#cccccc 1px solid; BORDER-BOTTOM:#cccccc 1px solid; FONT-FAMILY:arial"
	bgcolor="#f4f4f4" width="100%">
	<tr>
		<td Class="tableHeader" width="123" style="WIDTH: 123px" align="right">Credit Card 
			Number:</td>
		<td>
			<asp:TextBox ID="txtCreditCardNumber" Runat="server"></asp:TextBox>
			<asp:RequiredFieldValidator id="CCNumberValidator" ErrorMessage="Credit card number is required field." runat="server"
				ControlToValidate="txtCreditCardNumber">*</asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td Class="tableHeader" style="WIDTH: 123px" align="right">Credit Card Type:</td>
		<td>
			<asp:DropDownList id="ddlCreditCardType" Runat="server">
				<asp:ListItem Value="visa">VISA</asp:ListItem>
				<asp:ListItem Value="master">MASTER</asp:ListItem>
				<asp:ListItem Value="discover">Discover</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td Class="tableHeader" style="WIDTH: 123px" align="right">Expiration Date:</td>
		<td>
			<asp:DropDownList id="ddlMonth" runat="server">
				<asp:ListItem Value="1" Selected="True">1</asp:ListItem>
				<asp:ListItem Value="2">2</asp:ListItem>
				<asp:ListItem Value="3">3</asp:ListItem>
				<asp:ListItem Value="4">4</asp:ListItem>
				<asp:ListItem Value="5">5</asp:ListItem>
				<asp:ListItem Value="6">6</asp:ListItem>
				<asp:ListItem Value="7">7</asp:ListItem>
				<asp:ListItem Value="8">8</asp:ListItem>
				<asp:ListItem Value="9">9</asp:ListItem>
				<asp:ListItem Value="10">10</asp:ListItem>
				<asp:ListItem Value="11">11</asp:ListItem>
				<asp:ListItem Value="12">12</asp:ListItem>
			</asp:DropDownList>/
			<asp:DropDownList id="ddlYear" runat="server">
				<asp:ListItem Value="2005" Selected="True">2005</asp:ListItem>
				<asp:ListItem Value="2006">2006</asp:ListItem>
				<asp:ListItem Value="2007">2007</asp:ListItem>
				<asp:ListItem Value="2008">2008</asp:ListItem>
				<asp:ListItem Value="2009">2009</asp:ListItem>
				<asp:ListItem Value="2010">2010</asp:ListItem>
				<asp:ListItem Value="2011">2011</asp:ListItem>
				<asp:ListItem Value="2012">2012</asp:ListItem>
				<asp:ListItem Value="2013">2013</asp:ListItem>
				<asp:ListItem Value="2014">2014</asp:ListItem>
				<asp:ListItem Value="2015">2015</asp:ListItem>
			</asp:DropDownList></ASP:ADROTATOR></td>
	</tr>
	<tr>
		<td Class="tableHeader" style="WIDTH: 123px" align="right">Security Number:</td>
		<td>
			<asp:TextBox ID="txtSecurityNumber" Width="40" Runat="server"></asp:TextBox>
			<asp:RequiredFieldValidator id="SecurityNumber" runat="server" ControlToValidate="txtSecurityNumber" ErrorMessage="Security number is required field.">*</asp:RequiredFieldValidator></ASP:ADROTATOR>
		</td>
	</tr>
</table>
<div align="left"><SPAN style="FONT-SIZE: 14pt; COLOR: #6699cc; FONT-FAMILY: Arial">
		<BR>
		Billing Address</SPAN></div>
<TABLE id="Table1" width="100%" bgColor="#f4f4f4" border="0" style="BORDER-RIGHT:#cccccc 1px solid; BORDER-TOP:#cccccc 1px solid; BORDER-LEFT:#cccccc 1px solid; BORDER-BOTTOM:#cccccc 1px solid">
	<TR>
		<TD>
			<P>
				<uc1:ContactEditor2 id="BillingContact" ShowSameAsTechContact="false" ShowSameAsBillingAddress="false"
					runat="server"></uc1:ContactEditor2></P>
		</TD>
	</TR>
</TABLE>
<BR>
<div align="center" style="FONT-SIZE:11px; FONT-FAMILY:arial"><asp:CheckBox id="chkAgree" Runat="server" Text="I agree to use recurring billing service."></asp:CheckBox></div>
