<%@ Control Language="c#" AutoEventWireup="True" Codebehind="ChooseTemplate.ascx.cs" Inherits="ecn.wizard.wizard.ChooseTemplate" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language=javascript>
 function rbselected(val)
 {
	if (val == 'G')
	{
		document.getElementById('ECNWizard_ddlEmailList').disabled = false;
		document.getElementById('ECNWizard_txtfirstName').disabled = true;
		document.getElementById('ECNWizard_txtLastName').disabled = true;
		document.getElementById('ECNWizard_txtEmailaddress').disabled = true;	
	
	}
	else
	{
		document.getElementById('ECNWizard_ddlEmailList').disabled = true;
		document.getElementById('ECNWizard_txtfirstName').disabled = false;
		document.getElementById('ECNWizard_txtLastName').disabled = false;
		document.getElementById('ECNWizard_txtEmailaddress').disabled = false;
	}
 }
</script>
<div style="PADDING-RIGHT:20px; FONT-SIZE:12px; PADDING-BOTTOM:10px; PADDING-TOP:10px">
	<asp:Label ID="lblError" Runat="server" Visible="False" Font-Bold="True" ForeColor="#ff0000">Select an Email List and a template before you can continue.</asp:Label><br>
	<div style="PADDING-RIGHT:20px; PADDING-BOTTOM:0px; PADDING-TOP:10px">
		You will now be guided through the process of creating an HTML email message to 
		send to your customers. The Email Wizard will walk you through each step of the 
		process. Before you send the final email message, you will be able to preview 
		the content of the email to ensure quality.<br>
		<br>
		<strong>Select an Email List and a template before you can continue.</strong><br>
		<ul style="COLOR:#0099cc">
			<li>
				Select the list you would like to use to send the email message. If you have 
				not created a list yet, please <a href="ListCreate.aspx">Click Here</a>.
			<li>
			Choose a template from the thumbnails below.
			<li>
			Click on the template thumbnail to preview the template and text.
			<li>
				In step 2 you can choose to use the text provided or create your own message.</li>
		</ul>
	</div>
</div>
</TD></TR>
<tr>
	<td colspan="2" align=left>
		<div class="dashed_lines1" align="left"><strong>*Select a list to send an Email message.</strong></div><br>
		<table cellpadding="3" cellspacing="0" border="0" width="100%">
			<tr>
				<td align="left"><asp:RadioButton id="rbGroup" Runat="Server" Text="Send emails to Group" GroupName="grpSelect" Checked="True" CssClass="txtsmall"  /></td>
				<td  class="txtsmall"><asp:DropDownList runat="server" ID="ddlEmailList" CssClass="blue_border_box" />
					If you need to create a new Email list <a href="ListCreate.aspx">Click Here</a></td>
			</tr>
			<tr>
				<td align="left"><asp:RadioButton id="rbSingle" Runat="Server" Text="Send single Email" GroupName="grpSelect"  CssClass="txtsmall"/></td>
				<td>
					<table cellpadding="0" cellspacing="0" border="0">
						<tr valign=top>
							<td class="txtsmall">First Name&nbsp;</td>
							<td class="txtsmall">Last Name&nbsp;</td>
							<td class="txtsmall">Email Address</td>							
							
						</tr>	
						<tr valign=top>	
							<td><asp:TextBox ID="txtfirstName" Runat="server" CssClass="blue_border_box"></asp:TextBox>&nbsp;</td>
							<td><asp:TextBox ID="txtLastName" Runat="server" CssClass="blue_border_box"></asp:TextBox>&nbsp;</td>
							<td><asp:TextBox ID="txtEmailaddress" Runat="server" CssClass="blue_border_box"></asp:TextBox>&nbsp;<asp:regularexpressionvalidator id=RegularExpressionValidator1 runat="server" ControlToValidate="txtEmailaddress" ErrorMessage="«Not Valid" ValidationExpression=".*@.*\..*"></asp:regularexpressionvalidator></td>
						</tr>	
					</table>
				</td>
			</tr>
		</table>
	</td>
</tr>
<tr>
	<td colspan="2"><br>
		<div class="dashed_lines1" align="left"><strong>*Please select a template</strong></div>
		<div align="left" style="PADDING-RIGHT:0px; PADDING-LEFT:10px; FONT-SIZE:11px; PADDING-BOTTOM:10px; PADDING-TOP:10px"><strong>Choose 
				a template from the thumbnails below by clicking on the button above your 
				chosen template.</strong></div>
		<div align="left" id="templatesDiv" runat="server" style="PADDING-RIGHT:0px; PADDING-LEFT:0px; PADDING-BOTTOM:10px; PADDING-TOP:10px">
			<table id="templatesTable" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
			</table>
		</div>
	</td>
</tr>
