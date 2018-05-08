<%@ Page Language="C#" %>

<%@ Import Namespace="ecn.communicator.classes" %>

<script runat="Server">
    public void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //do nothing
        }
        else
        {
            CheckSend(sender, e);
        }
    }

    private void CheckSend(Object sender, EventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            //send email;
            string body =
                "\n<table border=1 bordercolor=silver cellspacing=0 cellpadding=5>" +
                "\n<tr><td align=right bgcolor=lightGrey><b>Name</b></td><td>" + Name.Text + "</td></tr>" +
                "\n<tr><td align=right bgcolor=lightGrey><b>Company</b></td><td>" + Company.Text + "</td></tr>" +
                "\n<tr><td align=right bgcolor=lightGrey><b>Phone Number</b></td><td>" + PhoneNumber.Text + "</td></tr>" +
                "\n<tr><td align=right bgcolor=lightGrey><b>Call Time</b></td><td>" + CallTime.SelectedItem.Value + "</td></tr>" +
                "\n<tr><td align=right bgcolor=lightGrey><b>IP Address</b></td><td>" + Request.UserHostAddress + "</td></tr>" +
                "\n</table>";
            EmailFunctions ef = new EmailFunctions();
            ef.SimpleSend("domain_admin@teckman.com", "blast@teckman.com", "ECN.communicator Call Request", body);
            questionPanel.Visible = false;
            responseLabel.Visible = true;
        }
        else
        {
            ValidationSummary.ShowSummary = true;
        }
    }
</script>

<style type='text/css'>
.tableContent { font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-weight: bold; color: #000000; border-color:Silver }
.formfield { border: solid; border-width: 2px 1px 1px 2px; border-color: #AD8000 #FF9900 #FF9900 #AD8000; font-family:Verdana, Arial, Helvetica, sans-serif; font-size: 11px;}
.formtextfield { font-family:Verdana, Arial, Helvetica, sans-serif; font-size: 11px;}
.formbutton { border: 1px solid; border-color: #FF9900 #AD8000 #AD8000 #FF9900; background-color: #D4D0C8}
</style>
<form id="signupForm" runat="Server">
    <asp:panel id="questionPanel" runat="Server">
	<table class=tableContent border='0' cellspacing="0" cellpadding="3">
		<tr>
			<td class="tableContent" colspan="2" align="center">
				Please fill out the information below and a representative will contact you as specified.
			</td>
		</tr>
		<tr>
			<td class="tableContent" colspan="2" align="center">
				<asp:ValidationSummary id="ValidationSummary" runat="Server" EnableClientValidation="True" ShowMessageBox="True" ShowSummary="False"></asp:ValidationSummary> 
			</td>
		</tr>
		<tr>
			<td class="tableContent" align='right'>Name:</td>
			<td class="tableContent">
				<asp:TextBox id="Name" runat="Server" size="30" CssClass="formfield" />
				<asp:RequiredFieldValidator runat="Server" id="val_name" ControlToValidate="Name" ErrorMessage = "Name is a required field." display="Static"><--</asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td class="tableContent" align='right'>Company:</td>
			<td class="tableContent">
				<asp:TextBox id="Company" runat="Server" size="30" CssClass="formfield" />
			</td>
		</tr>
		<tr>
			<td class="tableContent" align='right'>Phone Number:</td>
			<td class="tableContent">
				<asp:TextBox id="PhoneNumber" runat="Server" size="12" CssClass="formfield" />
				<asp:RequiredFieldValidator runat="Server" id="val_phoneNo" ControlToValidate="PhoneNumber" ErrorMessage = "Phone Number is a required field" display="Static"><--</asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td class="tableContent" valign="top" align='right'>Call Time?</td>
			<td class="tableContent">
			<asp:DropDownList EnableViewState="true" id="CallTime" runat="Server" CssClass="formfield" visible="true">
				<asp:ListItem value="ASAP">ASAP</asp:ListItem>
				<asp:ListItem value="Today">Today</asp:ListItem>
				<asp:ListItem value="Tomorrow">Tomorrow</asp:ListItem>
				<asp:ListItem value="This Week">This Week</asp:ListItem>
				<asp:ListItem value="Next Week">Next Week</asp:ListItem>			
			</asp:DropDownList>&nbsp;&nbsp;		
			</td>
		</tr>
	
		<tr>
			<td class="tableContent" colspan="2" align="middle">
				<asp:Button id="Submit" runat="Server" Text="Submit" CssClass="formbutton" OnServerClick="CheckSend"></asp:Button>
			</td>
		</tr>
	</table>
	</asp:panel>
    <asp:label id="responseLabel" runat="Server" visible="false">
	<center><br />Thank you for your response..<br /> You will be contacted by one our representatives soon.
	</asp:label>
</form>
