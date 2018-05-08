<%@ PAGE LANGUAGE="C#" %>
<%@ Import Namespace="ecn.communicator.classes" %>
<%@ Import Namespace="ecn.common.classes" %>
<html>

<script runat="Server">
ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();

public void Page_Load(Object sender, EventArgs e){
	if(!IsPostBack){
		//do nothing
	}else{
		CheckSend(sender,e);	
	}
}

private void CheckSend(Object sender, EventArgs e){
	Page.Validate();
	if(Page.IsValid){
		//send email;
		string body=
			"\n<table border=1 bordercolor=silver cellspacing=0 cellpadding=5>"+
			"\n<tr><td align=right bgcolor=lightGrey><b>Name</b></td><td>"+Server.HtmlEncode(Name.Text)+"</td></tr>"+
			"\n<tr><td align=right bgcolor=lightGrey><b>Company</b></td><td>"+Server.HtmlEncode(Company.Text)+"</td></tr>"+
			"\n<tr><td align=right bgcolor=lightGrey><b>Phone Number</b></td><td>"+Server.HtmlEncode(PhoneNumber.Text)+"</td></tr>"+
			"\n<tr><td align=right bgcolor=lightGrey><b>Email Address</b></td><td>"+Server.HtmlEncode(EmailAddress.Text)+"</td></tr>"+
			"\n<tr><td align=right bgcolor=lightGrey><b>City</b></td><td>"+Server.HtmlEncode(City.Text)+"</td></tr>"+
			"\n<tr><td align=right bgcolor=lightGrey><b>State</b></td><td>"+Server.HtmlEncode(State.Text)+"</td></tr>"+
			"\n<tr><td align=right bgcolor=lightGrey><b>Business Type</b></td><td>"+BusinessType.SelectedItem.Value+"</td></tr>"+
			"\n<tr><td align=right bgcolor=lightGrey><b>Opt-In Size</b></td><td>"+OptInSize.SelectedItem.Value+"</td></tr>"+
			"\n<tr><td align=right bgcolor=lightGrey><b>Time Frame</b></td><td>"+TimeFrame.SelectedItem.Value+"</td></tr>"+
			"\n<tr><td align=right bgcolor=lightGrey><b>How did you Hear</b></td><td>"+HowHear.SelectedItem.Value+"</td></tr>"+
			"\n<tr><td align=right bgcolor=lightGrey><b>Questions/Comments</b></td><td>"+Server.HtmlEncode(QuestionComment.Text)+"</td></tr>"+
			"\n<tr><td align=right bgcolor=lightGrey><b>IP Address</b></td><td>"+Request.UserHostAddress+"</td></tr>"+
			"\n</table>";
		EmailFunctions ef = new EmailFunctions();
		ef.SimpleSend("darin.lynch@knowledgemarketing.com","demo@knowledgemarketing.com","ECN.communicator Demo Request",body );
		questionPanel.Visible=false;
		responseLabel.Visible=true;
	}else{
		ValidationSummary.ShowSummary=true;
	}
}
</script>
<head>
<style type='text/css'>
.tableContent { font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; font-weight: bold; color: #000000; border-color:Silver }
.formfield { border: solid; border-width: 2px 1px 1px 2px; border-color: #AD8000 #FF9900 #FF9900 #AD8000; font-family:Verdana, Arial, Helvetica, sans-serif; font-size: 11px;}
.formtextfield { font-family:Verdana, Arial, Helvetica, sans-serif; font-size: 11px;}
.formbutton { border: 1px solid; border-color: #FF9900 #AD8000 #AD8000 #FF9900; background-color: #D4D0C8}
</style>
</head>
<body>


<form id="signupForm" runat="Server">
	<asp:Panel id=questionPanel runat="Server">
	<table class=tableContent background="/ecn.images/images/bglogo.jpg" border='0' cellspacing="0" cellpadding="3">
		<tr>
			<td class="tableContent" colspan="2" align="middle">
				Please fill out the information below and a representative will contact you to set up a live demo.
			</td>
		</tr>
		<tr>
			<td class="tableContent" colspan="2" align="middle">
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
			<td class="tableContent" align='right'>Email Address:</td>
			<td class="tableContent">
				<asp:TextBox id="EmailAddress" runat="Server" size="30" CssClass="formfield" />
				<asp:RequiredFieldValidator runat="Server" id="val_emailAddress" ControlToValidate="EmailAddress" ErrorMessage = "Email Address is a required field" display="Static"><--</asp:RequiredFieldValidator>
				<asp:RegularExpressionValidator id="valValidEmail" runat="Server" ControlToValidate="EmailAddress" ValidationExpression=".*@.*\..*" ErrorMessage="Email address must be valid" Display="Static"><--</asp:RegularExpressionValidator>
			</td>
		</tr>
		<tr>
			<td class="tableContent" align='right'>City:</td>
			<td class="tableContent">
				<asp:TextBox id="City" runat="Server" size="30" CssClass="formfield" />
			</td>
		</tr>
		<tr>
			<td class="tableContent" align='right'>State:</td>
			<td class="tableContent">
				<asp:TextBox id="State" runat="Server" size="2" CssClass="formfield" />
			</td>
		</tr>				
		<tr>
			<td class="tableContent" valign="top" align='right'>Type of Business:</td>
			<td class="tableContent">
			<asp:DropDownList EnableViewState="true" id="BusinessType" runat="Server" CssClass="formfield" visible="true">
				<asp:ListItem value=" "> </asp:ListItem>
				<asp:ListItem value="Advertising/Marketing">Advertising/Marketing</asp:ListItem>
				<asp:ListItem value="Computer Software">Computer Software</asp:ListItem>
				<asp:ListItem value="Consulting">Consulting</asp:ListItem>
				<asp:ListItem value="Electronics/Tech">Electronics/Tech</asp:ListItem>
				<asp:ListItem value="Entertainment">Entertainment</asp:ListItem>
				<asp:ListItem value="Financial Services">Financial Services</asp:ListItem>
				<asp:ListItem value="Food/Beverage">Food/Beverage</asp:ListItem>
				<asp:ListItem value="Healthcare">Healthcare</asp:ListItem>
				<asp:ListItem value="Hospitality/Travel">Hospitality/Travel</asp:ListItem>
				<asp:ListItem value="Manufacturing">Manufacturing</asp:ListItem>
				<asp:ListItem value="Non-Profit/Edu/Govt">Non-Profit/Edu/Govt</asp:ListItem>
				<asp:ListItem value="Publishing">Publishing</asp:ListItem>
				<asp:ListItem value="Retailing">Retailing</asp:ListItem>
				<asp:ListItem value="Telecommunications">Telecommunications</asp:ListItem>
				<asp:ListItem value="Transportation">Transportation</asp:ListItem>
				<asp:ListItem value="Wholesale">Wholesale</asp:ListItem>
				<asp:ListItem value="Consulting">Consulting</asp:ListItem>
				<asp:ListItem value="Other">Other</asp:ListItem>
			</asp:DropDownList>&nbsp;&nbsp;		
			</td>
		</tr>
		<tr>
			<td class="tableContent" valign="top" align='right'>Opt-in List Size?</td>
			<td class="tableContent">
			<asp:DropDownList EnableViewState="true" id="OptInSize" runat="Server" CssClass="formfield" visible="true">
				<asp:ListItem value="Less than 10k">Less than 10k</asp:ListItem>
				<asp:ListItem value="10K to 50K">10K to 50K</asp:ListItem>
				<asp:ListItem value="50K to 100K">50K to 100K</asp:ListItem>
				<asp:ListItem value="100K to 500K">100K to 500K</asp:ListItem>
				<asp:ListItem value="500K - 1 Million">500K - 1 Million</asp:ListItem>
				<asp:ListItem value="1 million - 2.5 Million">1 million - 2.5 Million</asp:ListItem>
				<asp:ListItem value="Above 2.5 Million">Above 2.5 Million</asp:ListItem>					
			</asp:DropDownList>&nbsp;&nbsp;		
			</td>
		</tr>
		<tr>
			<td class="tableContent" valign="top" align='right'>Time Frame?</td>
			<td class="tableContent">
			<asp:DropDownList EnableViewState="true" id="TimeFrame" runat="Server" CssClass="formfield" visible="true">
				<asp:ListItem value="ASAP">ASAP</asp:ListItem>
				<asp:ListItem value="0 to 3 months">0 to 3 months</asp:ListItem>
				<asp:ListItem value="3 to 6 months">3 to 6 months</asp:ListItem>
				<asp:ListItem value="6 to 12 months">6 to 12 months</asp:ListItem>				
			</asp:DropDownList>&nbsp;&nbsp;		
			</td>
		</tr>		
		<tr>
			<td class="tableContent" valign="top" align='right'>How did you hear about us?</td>
			<td class="tableContent">
			<asp:DropDownList EnableViewState="true" id="HowHear" runat="Server" CssClass="formfield" visible="true">
				<asp:ListItem value="Magazine Ads">Magazine Ads</asp:ListItem>
				<asp:ListItem value="Newsletter">Newsletter</asp:ListItem>
				<asp:ListItem value="Received an EMail powered by ECN">Received an EMail powered by ECN</asp:ListItem>
				<asp:ListItem value="Newspaper Article">Newspaper Article</asp:ListItem>
				<asp:ListItem value="Conference or Convention">Conference or Convention</asp:ListItem>
				<asp:ListItem value="Surfing the Web">Surfing the Web</asp:ListItem>
				<asp:ListItem value="Press Release">Press Release</asp:ListItem>
				<asp:ListItem value="Other" Selected="true">Other</asp:ListItem>					
			</asp:DropDownList>&nbsp;&nbsp;		
			</td>
		</tr>		
		<tr>
			<td class="tableContent" valign="top" align='right'>Question or Comment:</td>
			<td class="tableContent">
				<asp:TextBox id="QuestionComment" runat="Server" cols="30" rows="5" CssClass="formfield" TextMode="MultiLine" />
			</td>
		</tr>
		
	
		<tr>
			<td class="tableContent" colspan="2" align="middle">
				<asp:Button id="Submit" runat="Server" Text="Submit" CssClass="formbutton" OnServerClick="CheckSend"></asp:Button>
			</td>
		</tr>
	</table>
	</asp:Panel>
	<asp:Label id=responseLabel runat="Server" visible=false>
	<center><br />Thank you for your response..<br /> You will be contacted by one our representatives soon.
	</asp:Label>
</form>

</body>
</html>
<style type='text/css'>