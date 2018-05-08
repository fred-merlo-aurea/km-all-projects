<script language="javascript">
    document.location.href = "http://www.knowledgemarketing.com/contact_us.php?demo=true";
</script>
<%--
        <%@ Import Namespace="ecn.common.classes" %>
<%@ Import Namespace="ecn.communicator.classes" %>
<%@ Register TagPrefix="ecn" TagName="footer" Src="../includes/footer.ascx" %>
<%@ Register TagPrefix="ecn" TagName="header" Src="../includes/header.ascx" %>            
<%@ Page Language="C#" %>
<script runat="Server">
    ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();
    ImportFunctions imp = new ImportFunctions();

    public void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Find the name we should display for this email if any
            NameValueCollection coll;
            coll = Request.QueryString;
            if (coll.Get("e") != null)
            {
                EmailAccessor user_info = new EmailAccessor(Convert.ToInt32(coll.Get("e")));
                MyName.Text = user_info.FirstName();
            }

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

            // Convert the checkboxes and import the email into the database
            string wants_test = "no";
            string wants_demo = "no";
            if (WantsDemo.Checked)
            {
                wants_demo = "yes";
            }
            if (WantsTest.Checked)
            {
                wants_test = "yes";
            }
            imp.LoadDemoEmail(EmailAddress.Text, FirstName.Text, LastName.Text, Company.Text, PhoneNumber.Text, wants_demo, wants_test, Request.UserHostAddress);

            questionPanel.Visible = false;
            responseLabel.Visible = true;
        }
        else
        {
            ValidationSummary.ShowSummary = true;
        }
    }
</script>

<ecn:header ID="pageheader" runat="Server" ecnMenu="login" divHelpBoxTitle="The Power of the Latest eMarketing Tools "
    divHelpBox="&#13;&#10;<b>List Builders</b>&#13;&#10;<br />Our web forms and viral marketing tools grow your list of permission-based email addresses quickly.&#13;&#10;<br /><br />&#13;&#10;<b>Libraries</b>&#13;&#10;<br />ECN.communicator provides simple management of your contacts, content, images, surveys and documents in our online libraries. &#13;&#10;<br /><br />&#13;&#10;<b>Precision Target Marketing Tools</b>&#13;&#10;<br />Our Direct Marketing tools help you target, personalize and optimize your emails.&#13;&#10;<br /><br />&#13;&#10;<a href=signup.aspx><img src='/ecn.images/images/actionbutton.gif' border='0'></a>&#13;&#10;"
    divContentTitle="Get a Demo!" />
<style type="text/css">.tableContent { FONT-SIZE: 11px; BORDER-LEFT-COLOR: silver; BORDER-BOTTOM-COLOR: silver; COLOR: #000000; BORDER-TOP-COLOR: silver; FONT-FAMILY: Verdana, Arial, Helvetica, sans-serif; BORDER-RIGHT-COLOR: silver }
	.formfield { BORDER-RIGHT: #ff9900 1px solid; BORDER-TOP: #ad8000 2px solid; FONT-SIZE: 11px; BORDER-LEFT: #ad8000 2px solid; BORDER-BOTTOM: #ff9900 1px solid; FONT-FAMILY: Verdana, Arial, Helvetica, sans-serif }
	.formtextfield { FONT-SIZE: 11px; FONT-FAMILY: Verdana, Arial, Helvetica, sans-serif }
	.formbutton { BORDER-RIGHT: #ad8000 1px solid; BORDER-TOP: #ff9900 1px solid; BORDER-LEFT: #ff9900 1px solid; BORDER-BOTTOM: #ad8000 1px solid; BACKGROUND-COLOR: #d4d0c8 }
a:link, a:visited, a:active
{
	text-decoration:none;
	color:#000;
	font-weight:bold;
}
a:hover
{
	color:#7B95A2;
	text-decoration:underline;
	font-weight:bold;
}
	</style>
<form id="signupForm" runat="Server">
    <asp:panel id="questionPanel" runat="Server">
        <TABLE class="tableContent" align="center" cellspacing="3" cellpadding="1" border='0'>
            <tr>
                <td class="tableContent" colspan='3' align="center">
                    <P>Thanks <I><FONT color="#ff0000">
                                <asp:Label id="MyName" runat="Server"></asp:Label></FONT></I>&nbsp;for 
                        your interest in our relationship enhancing&nbsp;software.&nbsp;
                    </P>
					
					<!--<p><A href="http://www.ecn5.com">Enterprise 
                        Communication Network (ECN)</A> is focused on helping clients engage and 
                    better understand their customer's.
                   ECN's true integration of <A href="http://www.ecn5.com/comm.htm">
                            email marketing</A>, <A href="http://www.ecn5.com/creator.htm">
                            content management</A> and <A href="http://www.ecn5.com/collect.htm">
                            online surveys</A>
                    results in individualized marketing intelligence. Customers now have the 
                    ability to conduct a survey, store the response to the individual's profile, 
                    and present a 'landing page' which is based entirely on how that person 
                    answered the questions to the survey. ECN users can leverage one or all of the 
                    applications. ECN clients include: Mall of America, Digi International, Rush 
                    Creek and The Wilds Golf Course.</p>
					
                    <P>To discover how your organization can benefit from permission-based email 
                        marketing please request a "Test Account" or "Online Demo" below;
                    </P>-->
                	
					
					
					<p>
                    <asp:ValidationSummary id="ValidationSummary" runat="Server" EnableClientValidation="True" ShowMessageBox="True"
                        ShowSummary="False"></asp:ValidationSummary>
					</p>
					<div align="center">
					<table align="center" class="tableContent">
					
						<tr>
							<td align='right'  style="font-weight:bold; padding:15px 0;">Email Address:</td>
							<td><asp:TextBox id="EmailAddress" runat="Server" size="30"></asp:TextBox>
							  <asp:RequiredFieldValidator ID="val_emailAddress" runat="Server" ControlToValidate="EmailAddress" ErrorMessage="Email Address is a required field"
                            Display="Static"><--</asp:RequiredFieldValidator>
							  <asp:RegularExpressionValidator ID="valValidEmail" runat="Server" ControlToValidate="EmailAddress" ErrorMessage="Email address must be valid"
                            ValidationExpression=".*@.*\..*" Display="Static"><--</asp:RegularExpressionValidator></td>
						    <td align='right'  style="font-weight:bold">Company:</td> 
							<td><asp:TextBox ID="Company" runat="Server" size="30"></asp:TextBox>
							<asp:RequiredFieldValidator id="val_Company" runat="Server" ControlToValidate="Company" ErrorMessage="Company is a required field."
                        display="Static"><--</asp:RequiredFieldValidator></td>
						</tr>
						<tr>
							<td align='right' width="100" style="font-weight:bold">First Name:</td>
							<td><asp:TextBox id="FirstName" runat="Server" size="30"></asp:TextBox>
							  <asp:RequiredFieldValidator ID="val_firstName" runat="Server" ControlToValidate="FirstName" ErrorMessage="First Name is a required field."
                        Display="Static"><--</asp:RequiredFieldValidator>
							  <br /></td>
							<td align='right'  style="font-weight:bold">Last Name:</td>
							<td><asp:TextBox id="LastName" runat="Server" size="30"></asp:TextBox>
                    <asp:RequiredFieldValidator id="val_lastName" runat="Server" ControlToValidate="LastName" ErrorMessage="Last Name is a required field."
                        display="Static"><--</asp:RequiredFieldValidator></td>
						</tr>
						
						<tr>
							<td align='right'  style="font-weight:bold; padding:15px 0;">Phone Number:</td>
							<td><asp:TextBox id="PhoneNumber" runat="Server" size="12"></asp:TextBox>
						    <asp:RequiredFieldValidator ID="val_phoneNo" runat="Server" ControlToValidate="PhoneNumber" ErrorMessage="Phone Number is a required field"
                        Display="Static"><--</asp:RequiredFieldValidator></td>
							<td colspan="2" align="left" style="padding:0 0 0 95px;">
                           <!--   Live Demo: 
                    <asp:CheckBox id="WantsDemo" runat="Server"></asp:CheckBox> Test Account 
						<asp:CheckBox ID="WantsTest" runat="Server"></asp:CheckBox>--></td>
						</tr>
						
						<tr>
							<td colspan="4"  style="font-weight:bold;padding:0 0 0 117px;"></td>
						</tr>
						<tr>
							<td colspan="4"  style="font-weight:bold;padding:0 0 0 105px;"></td>
						</tr>
						<tr>
							<td colspan="4" align="center"><asp:Button id="Submit" runat="Server" Text="Submit" OnServerClick="CheckSend"></asp:Button></td>
						</tr>
				  </table>
					</div>	
				</td>
            </tr>
        </TABLE>
        <p class="tableContent" style="text-align:center;">or <A href="http://www.ecncommunicator.com/">click here</A>
                to read more about our product and service</p>
				
		<p class="tableContent" style="text-align:center;">To learn more, visit us at<br />
		<A href="http://www.knowledgemarketing.com/">www.knowledgemarketing.com</A> (or) <A href="http://www.ecn5.com">www.ecn5.com</A></p>
    </asp:panel>
    <asp:label id="responseLabel" runat="Server" visible="false">
	<center><br />
            <font color="#000000" size=2 face=verdana>Thank you for your response..<br /> You will be contacted by one our representatives soon.</font>
	</asp:label>
</form>
<ecn:footer ID="pagefooter" runat="Server" />
--%>