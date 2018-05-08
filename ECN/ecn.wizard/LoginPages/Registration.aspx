<%@ Page language="c#" Codebehind="Registration.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard.LoginPages.Registration" %>
<script language="javascript"> 
 
function validateCheckbox(source, args) 
{ 
	if (!document.all["checkTerms"].checked)
	{
		alert('Please "Check" the Checkbox to accept the terms and conditions.');
	}
     args.IsValid = document.all["checkTerms"].checked; 
} 
 
</script>  

<center>
	<form id="Form1" method="post" runat="server" class="body_bg">
		<br>
		<table cellpadding="0" cellspacing="0" border="0" ID="Table4">
			<tr>
				<td valign="top" width="154"><img src="../images/img_cust_reg.gif"></td>
				<td width="564" valign="top" align="left" >
					<asp:label id="lblChannelID" value="0" runat="server" visible="false"></asp:label>
					<div style="PADDING-RIGHT: 20px; PADDING-LEFT: 10px; FONT-SIZE: 12px; PADDING-BOTTOM: 5px"><BR>
						<div class="dashed_lines1" style=" FONT-SIZE: 12px"><strong>New Customer Registration</strong>&nbsp; 
							* You must Fill out all required fields to register.</div>
						<br>
						<div style="PADDING-LEFT: 25px; PADDING-BOTTOM: 0px; PADDING-TOP: 5px">
							<table width="100%" cellpadding="3" cellspacing="0" border="0">
								<tr>
									<td colspan="2" valign="middle"><font color="#ff0000">*&nbsp;</font>Email Address: 
										(this will be your user name)<br>
										<asp:textbox id="txtEmailAddress" CssClass="blue_border_box" runat="server" size="35"></asp:textbox><asp:requiredfieldvalidator id="rfv3" runat="server" ErrorMessage="&nbsp;«&nbsp;Required" ControlToValidate="txtEmailAddress"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" ErrorMessage="&nbsp;«&nbsp;Not Valid" ValidationExpression=".*@.*\..*"
			ControlToValidate="txtEmailAddress"></asp:regularexpressionvalidator>
									</td>
								</tr>
								<tr>
									<td colspan="2" valign="middle">
										<font color="#ff0000">*&nbsp;</font>Password:<br>
										<asp:textbox id="txtPassword" CssClass="blue_border_box" runat="server" size="20" TextMode="Password"></asp:textbox><asp:requiredfieldvalidator id="rfv1" runat="server" ErrorMessage="&nbsp;«&nbsp;Required" ControlToValidate="txtPassword"></asp:requiredfieldvalidator>
									</td>
								</tr>
								<tr>
									<td colspan="2" valign="middle">
										<font color="#ff0000">*&nbsp;</font>Re-enter Password:<br>
										<asp:textbox id="txtcPassword" CssClass="blue_border_box" runat="server" size="20" TextMode="Password"></asp:textbox>
										<asp:requiredfieldvalidator id="rfv2" runat="server" ErrorMessage="&nbsp;«&nbsp;Required" ControlToValidate="txtcPassword"></asp:requiredfieldvalidator>
										<asp:CompareValidator id="CompareValidator1" runat="server" ControlToValidate="txtcPassword" ErrorMessage="CompareValidator"
											ControlToCompare="txtPassword">«&nbsp;Password did not match.</asp:CompareValidator>
									</td>
								</tr>
							</table>
						</div>
						<div class="dashed_lines1" style=" FONT-SIZE: 12px"><strong></strong>&nbsp;</div>
						<div style="PADDING-LEFT: 25px; PADDING-BOTTOM: 0px; PADDING-TOP: 5px">
							<table width="100%" cellpadding="3" cellspacing="0" border="0">
								<tr>
									<td colspan="2">
										<font color="#ff0000">*&nbsp;</font>Company Name:<br>
										<asp:textbox id="txtCustomerName" CssClass="blue_border_box" runat="server" size="35"></asp:textbox><asp:requiredfieldvalidator id="rfv4" runat="server" ErrorMessage="&nbsp;«&nbsp;Required" ControlToValidate="txtCustomerName"></asp:requiredfieldvalidator>
									</td>
								</tr>
								<tr>
									<td colspan="2"><font color="#ff0000">*&nbsp;</font>First Name:<br>
										<asp:textbox id="txtFirstName" CssClass="blue_border_box" runat="server" size="20"></asp:textbox><asp:requiredfieldvalidator id="rfv5" runat="server" ErrorMessage="&nbsp;«&nbsp;Required" ControlToValidate="txtFirstName"></asp:requiredfieldvalidator>
									</td>
								</tr>
								<tr>
									<td colspan="2"><font color="#ff0000">*&nbsp;</font>Last Name:<br>
										<asp:textbox id="txtLastName" CssClass="blue_border_box" runat="server" size="20"></asp:textbox><asp:requiredfieldvalidator id="rfv6" runat="server" ErrorMessage="&nbsp;«&nbsp;Required" ControlToValidate="txtLastName"></asp:requiredfieldvalidator></td>
								</tr>
								<tr>
									<td colspan="2"><font color="#ff0000">*&nbsp;</font>Phone:<br>
										<asp:textbox id="txtphone" CssClass="blue_border_box" runat="server" size="20"></asp:textbox><asp:requiredfieldvalidator id="rfv7" runat="server" ErrorMessage="&nbsp;«&nbsp;Required" ControlToValidate="txtphone"></asp:requiredfieldvalidator></td>
								</tr>
								<tr>
									<td colspan="2">Fax:<br>
										<asp:textbox id="txtfax" CssClass="blue_border_box" runat="server" size="20"></asp:textbox></td>
								</tr>
								<tr>
									<td colspan="2">
										<font color="#ff0000">*&nbsp;</font>Address:<br>
										<asp:textbox id="txtaddress" CssClass="blue_border_box" runat="server" size="35"></asp:textbox><asp:requiredfieldvalidator id="rfv8" runat="server" ErrorMessage="&nbsp;«&nbsp;Required" ControlToValidate="txtaddress"></asp:requiredfieldvalidator>
									</td>
								</tr>
								<tr>
									<td colspan="2"><font color="#ff0000">*&nbsp;</font>City:<br>
										<asp:textbox id="txtcity" CssClass="blue_border_box" runat="server" size="15"></asp:textbox><asp:requiredfieldvalidator id="rfv9" runat="server" ErrorMessage="&nbsp;«&nbsp;Required" ControlToValidate="txtcity"></asp:requiredfieldvalidator></td>
								</tr>
								<tr>
									<td colspan="2"><font color="#ff0000">*&nbsp;</font>State:
										<br>
										<asp:DropDownList runat="server" ID="ddlstate" CssClass="blue_border_box" width="50">
											<asp:Listitem value=""> </asp:Listitem>
											<asp:Listitem value="AK">AK</asp:Listitem>
											<asp:Listitem value="AL">AL</asp:Listitem>
											<asp:Listitem value="AR">AR</asp:Listitem>
											<asp:Listitem value="AZ">AZ</asp:Listitem>
											<asp:Listitem value="CA">CA</asp:Listitem>
											<asp:Listitem value="CO">CO</asp:Listitem>
											<asp:Listitem value="CT">CT</asp:Listitem>
											<asp:Listitem value="DC">DC</asp:Listitem>
											<asp:Listitem value="DE">DE</asp:Listitem>
											<asp:Listitem value="FL">FL</asp:Listitem>
											<asp:Listitem value="GA">GA</asp:Listitem>
											<asp:Listitem value="HI">HI</asp:Listitem>
											<asp:Listitem value="IA">IA</asp:Listitem>
											<asp:Listitem value="ID">ID</asp:Listitem>
											<asp:Listitem value="IL">IL</asp:Listitem>
											<asp:Listitem value="IN">IN</asp:Listitem>
											<asp:Listitem value="KS">KS</asp:Listitem>
											<asp:Listitem value="KY">KY</asp:Listitem>
											<asp:Listitem value="LA">LA</asp:Listitem>
											<asp:Listitem value="MA">MA</asp:Listitem>
											<asp:Listitem value="MD">MD</asp:Listitem>
											<asp:Listitem value="ME">ME</asp:Listitem>
											<asp:Listitem value="MI">MI</asp:Listitem>
											<asp:Listitem value="MN">MN</asp:Listitem>
											<asp:Listitem value="MO">MO</asp:Listitem>
											<asp:Listitem value="MS">MS</asp:Listitem>
											<asp:Listitem value="MT">MT</asp:Listitem>
											<asp:Listitem value="NC">NC</asp:Listitem>
											<asp:Listitem value="ND">ND</asp:Listitem>
											<asp:Listitem value="NE">NE</asp:Listitem>
											<asp:Listitem value="NH">NH</asp:Listitem>
											<asp:Listitem value="NJ">NJ</asp:Listitem>
											<asp:Listitem value="NM">NM</asp:Listitem>
											<asp:Listitem value="NV">NV</asp:Listitem>
											<asp:Listitem value="NY">NY</asp:Listitem>
											<asp:Listitem value="OH">OH</asp:Listitem>
											<asp:Listitem value="OK">OK</asp:Listitem>
											<asp:Listitem value="OR">OR</asp:Listitem>
											<asp:Listitem value="PA">PA</asp:Listitem>
											<asp:Listitem value="RI">RI</asp:Listitem>
											<asp:Listitem value="SC">SC</asp:Listitem>
											<asp:Listitem value="SD">SD</asp:Listitem>
											<asp:Listitem value="TN">TN</asp:Listitem>
											<asp:Listitem value="TX">TX</asp:Listitem>
											<asp:Listitem value="UT">UT</asp:Listitem>
											<asp:Listitem value="VA">VA</asp:Listitem>
											<asp:Listitem value="WA">WA</asp:Listitem>
											<asp:Listitem value="WI">WI</asp:Listitem>
											<asp:Listitem value="WV">WV</asp:Listitem>
											<asp:Listitem value="WY">WY</asp:Listitem>
										</asp:DropDownList><asp:requiredfieldvalidator id="rfv10" runat="server" ErrorMessage="&nbsp;«&nbsp;Required" ControlToValidate="ddlstate"></asp:requiredfieldvalidator></td>								
								</tr>
								<tr>
									<td colspan="2"><font color="#ff0000">*&nbsp;</font>Zip:<br>
										<asp:textbox id="txtzip" CssClass="blue_border_box" runat="server" size="15"></asp:textbox><asp:requiredfieldvalidator id="rfv11" runat="server" ErrorMessage="&nbsp;«&nbsp;Required" ControlToValidate="txtzip"></asp:requiredfieldvalidator>
									</td>
								</tr>
								<tr>
									<td colspan="2"><font color="#ff0000">*&nbsp;</font>Country:<br>
										<asp:textbox id="txtcountry" CssClass="blue_border_box" runat="server" size="15"></asp:textbox><asp:requiredfieldvalidator id="rfv12" runat="server" ErrorMessage="&nbsp;«&nbsp;Required" ControlToValidate="txtcountry"></asp:requiredfieldvalidator>
									</td>
								</tr>
								<tr>
									<td colspan="2"><asp:CheckBox id="checkTerms" runat="server"></asp:CheckBox>&nbsp;<font size="2" face="arial">I have read and accept the terms and conditions stated in the <a href="http://www.knowledgemarketing.com/terms.php" target=_blank>KMLLC Services Agreement</a></font><br>
									<asp:CustomValidator runat="server" ErrorMessage="" ClientValidationFunction="validateCheckbox" ID="Customvalidator1" />  

									</td>
								</tr>								
								<tr>
									<td colspan="2"><br>
										<asp:Label id="lblError" runat="server" Visible="False" ForeColor="red"></asp:Label>
									</td>
								</tr>
								<tr>
									<td colspan="2" align=center>
										<asp:Button id="SaveButton" Text="Create New Customer" runat="server" onclick="SaveButton_Click"></asp:Button>&nbsp;
										<asp:Button id="btnCancel" Text="Cancel" runat="server" causesvalidation="false" onclick="btnCancel_Click"></asp:Button>
									</td>
								</tr>								
							</table>
						</div>
					</div>
				</td>
			</tr>
		</table>
	</form>
</center>
