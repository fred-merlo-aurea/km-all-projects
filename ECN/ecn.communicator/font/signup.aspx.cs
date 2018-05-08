using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ecn.communicator.classes;

namespace ecn.communicator.front
{
    public partial class signup : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.LOGIN;
            Master.SubMenu = "";
            Master.Heading = "Request Information on ECN.communicator";
            Master.HelpContent = "Once you fill out this form, someone from Teckman will contact you to discuss ECN and how you can get started immediately.";
            Master.HelpTitle = "Next Step";

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
                    "\n<tr><td align=right bgcolor=lightGrey><b>Email Address</b></td><td>" + EmailAddress.Text + "</td></tr>" +
                    "\n<tr><td align=right bgcolor=lightGrey><b>Service Level</b></td><td>" + ServiceLevel.SelectedItem.Value + "</td></tr>" +
                    "\n<tr><td align=right bgcolor=lightGrey><b>How did you Hear</b></td><td>" + HowHear.SelectedItem.Value + "</td></tr>" +
                    "\n<tr><td align=right bgcolor=lightGrey><b>Questions/Comments</b></td><td>" + QuestionComment.Text + "</td></tr>" +
                    "\n<tr><td align=right bgcolor=lightGrey><b>IP Address</b></td><td>" + Request.UserHostAddress + "</td></tr>" +
                    "\n</table>";
                EmailFunctions ef = new EmailFunctions();
                ef.SimpleSend("domain_admin@teckman.com", "blast@teckman.com", "ECN.communicator Request", body);
                questionPanel.Visible = false;
                responseLabel.Visible = true;
            }
            else
            {
                ValidationSummary.ShowSummary = true;
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {

        }
        #endregion
    }
}