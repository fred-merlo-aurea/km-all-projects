using System;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Text;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace ecn.activityengines {
	
	
	
	public partial class reportSpam : System.Web.UI.Page
    {

        #region Private Properties
        //private static string connString	= ConfigurationManager.AppSettings["connString"];
        //private static string accountsDB = ConfigurationManager.AppSettings["accountsdb"];
        //private string _ErrorMessage;
        private string EmailAddress = string.Empty;
        private int EmailID = 0;
        private int GroupID = 0;
        private int CustomerID = 0;
        private int BlastID = 0; 
        private int Preview = 0;

        private string CustomerName = string.Empty;
        private string GroupName = string.Empty;
        private string GroupDescription = string.Empty;
        
        private KMPlatform.Entity.User User;
        private ECN_Framework_Entities.Accounts.LandingPageAssign LPA;
        #endregion

        #region Protected Methods
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Request.Url.Query.Length > 0 && Request.Url.Query.Contains("="))
            {
                if (Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())] == null)
                {
                    User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
                    Cache.Add(string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString()), User, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
                }
                else
                {
                    User = (KMPlatform.Entity.User)Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())];
                }

                if (ValidateParams())
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(CustomerID, false);
                    if (customer != null)
                        CustomerName = customer.CustomerName;

                    //setup page
                    if (!IsPostBack)
                        SetupPageValues();
                }
                else
                {
                    try
                    {
                        //WGH: 10/31/2014 - Removing old logging
                        //KM.Common.Entity.ApplicationLog.LogNonCriticalError("EmailAddress, EmailID, GroupID, BlastID, CustomerID are empty or are invalid", "ReportSpam.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                        SetError(Enums.ErrorMessage.InvalidLink);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else
            {
                //WGH: 10/31/2014 - Removing old logging
                //KM.Common.Entity.ApplicationLog.LogNonCriticalError("Querystring appears invalid", "ReportSpam.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                SetError(Enums.ErrorMessage.InvalidLink);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //they clicked unsubscribe, if it's a preview don't record the unsubscribe
            if (txtReason.Text.Length <= 150)
            {
                if (Preview <= 0)
                {
                    ReportSpam();
                    SendEmailReportToAdmin("");
                }

                phError.Visible = false;
                pnlMain.Visible = false;
                pnlThankYou.Visible = true;
            }
            else
            {
                phError.Visible = true;
                lblErrorMessage.Text = "Please limit your feedback to a maximum of 150 characters";
            }
        }
        #endregion

        #region Private Methods
        private void SetError(Enums.ErrorMessage errorMessage, string pageMessage = "")
        {
            pnlMain.Visible = false;
            pnlThankYou.Visible = false;
            phError.Visible = true;
            lblErrorMessage.Text = ActivityError.GetErrorMessage(errorMessage, pageMessage);
            Label lblHeader = Master.FindControl("lblHeader") as Label;
            if (lblHeader.Text.Trim().Length == 0)
            {
                SetHeaderFooter();
            }
        }

        private void SetHeaderFooter()
        {
            //if it's a preview just show them the one they are requesting
            if (Preview > 0)
                LPA = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByLPAID(Preview, true);
            else
                LPA = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetOneToUse(4, CustomerID, true);

            Page.Title = "Report Abuse";

            Label lblHeader = Master.FindControl("lblHeader") as Label;
            lblHeader.Text = ReplaceStandardFields(LPA.Header);

            Label lblFooter = Master.FindControl("lblFooter") as Label;
            lblFooter.Text = ReplaceStandardFields(LPA.Footer);
        }

        private string ReplaceStandardFields(string input)
        {
            string output = input;
            output = output.Replace("%%groupname%%", GroupName);
            output = output.Replace("%%groupdescription%%", GroupDescription);
            output = output.Replace("%%customername%%", CustomerName);
            output = output.Replace("%%emailaddress%%", EmailAddress);

            return output;
        }

        private void SetupPageValues()
        {
            pnlMain.Visible = true;
            pnlThankYou.Visible = false;
            phError.Visible = false;

            SetHeaderFooter();

            //Get the email address
            lblEmailAddress.Text = "Your Email Address: " + EmailAddress;
        }

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();

            try
            {
                adminEmailVariables.AppendLine("<br><b>Blast ID:</b>&nbsp;" + BlastID);
                adminEmailVariables.AppendLine("<br><b>Group ID:</b>&nbsp;" + GroupID);
                adminEmailVariables.AppendLine("<br><b>Email ID:</b>&nbsp;" + EmailID);
                adminEmailVariables.AppendLine("<br><b>Email Address:</b>&nbsp;" + EmailAddress);
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }        

		private bool ValidateParams() 
        {
            //[FORMAT]: ?p=emailAddress,emailID,groupID,customerID,blastID
            //[EXAMPLE]: http://localhost/ecn.communicator/engines/reportSpam.aspx?p=ashanaa@teckman.com,14034271,5816,87,69635
            
            ArrayList parametersList = new ArrayList();
            string parameters = "";
            bool success = false;
            try 
            { 				
				parameters = Request.QueryString["p"].ToString();
                ECN_Framework_Common.Functions.StringTokenizer st = new ECN_Framework_Common.Functions.StringTokenizer(parameters, ',');
                for(int i=0; st.HasMoreTokens(); i++){
                    parametersList.Insert(i, st.NextToken().ToString());
				}
                try
                {
                    EmailAddress = parametersList[0].ToString().Trim();
                }
                catch (Exception) { }
                try
                {
                    EmailID = Convert.ToInt32(parametersList[1].ToString().Trim());
                }
                catch (Exception) { }
                try
                {
                    GroupID = Convert.ToInt32(parametersList[2].ToString().Trim());
                }
                catch (Exception) {}
                try
                {
                    CustomerID = Convert.ToInt32(parametersList[3].ToString().Trim());
                }
                catch (Exception) { }
                try
                {
                    BlastID = Convert.ToInt32(parametersList[4].ToString().Trim());
                }
                catch (Exception) { }
                int refBlastID = BlastID;
                int refGroupID = GroupID;
                if (BlastID > 0 && EmailID > 0)
                {
                    try
                    {
                        ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.BlastAbstract.GetByBlastID_NoAccessCheck(BlastID, false);
                        if (blast.BlastType.ToUpper() == "LAYOUT" || blast.BlastType.ToUpper() == "NOOPEN")
                        {
                            refBlastID = ECN_Framework_BusinessLayer.Communicator.BlastSingle.GetRefBlastID(BlastID, EmailID, blast.CustomerID.Value, blast.BlastType);
                            blast = ECN_Framework_BusinessLayer.Communicator.BlastAbstract.GetByBlastID_NoAccessCheck(refBlastID, false);
                            refGroupID = blast.GroupID.Value;
                        }
                    }
                    catch (Exception) { }
                }
                if(EmailAddress.Length > 0 && EmailID > 0 && CustomerID > 0 && refBlastID > 0 && refGroupID > 0)
                {
                    if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddressForBlast(EmailID, EmailAddress, CustomerID, refGroupID, refBlastID))
                    {
                        success = true;
                    }                    
                }
                try
                {
                    Preview = Convert.ToInt32(Request.QueryString["preview"].ToString());
                }
                catch (Exception)
                {
                }
			}
			catch(Exception) 
            {                 
            }
			return success;
        }

        private void ReportSpam()
        {
            try
            {
                string notes = txtReason.Text.Trim();
                string violation = ddlViolation.SelectedValue.ToString();

                ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.InsertSpamFeedback(BlastID, EmailID, "Reported ABUSE:\n[VIOLATION: " + violation + "]\n" + notes, "A", "ABUSERPT_UNSUB");
            }
            catch (Exception)
            {
                SendEmailReportToAdmin("Attention Admin: Spam was not logged due to error.");
            }
        }

        private void SendEmailReportToAdmin(string alertMessage)
        {
            string basechannelName = string.Empty;
            string customerName = string.Empty;
            string sendTime = string.Empty;

            try
            {
                string mail_ToEmail = ConfigurationManager.AppSettings["ABUSERPT_ToEmail"].ToString();
                string mail_FromEmail = ConfigurationManager.AppSettings["ABUSERPT_FromEmail"].ToString();
                string mail_Subject = ConfigurationManager.AppSettings["ABUSERPT_Subject"].ToString();

                DataTable dt = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastInfoForAbuseReporting(BlastID);

                if (dt.Rows.Count > 0)
                {
                    basechannelName = dt.Rows[0]["basechannelname"].ToString();
                    customerName = dt.Rows[0]["customerName"].ToString();
                    sendTime = dt.Rows[0]["sendtime"].ToString();

                    string body = "";
                    body += "Channel: " + basechannelName + "<br>";
                    body += "Customer: " + customerName + "<br>";
                    body += "EmailID: " + EmailID + "<br>";
                    body += "BlastID: " + BlastID + "<br>";
                    body += "Campaign Date: " + sendTime + "<br>";
                    body += "<p>Email Violation:&nbsp;" + this.ddlViolation.SelectedValue.ToString() + "</p>";
                    body += "<p>Email Subject:&nbsp;" + dt.Rows[0]["EmailSubject"].ToString() + "</p>";
                    body += "<p>Feedback:<br>" + this.txtReason.Text.ToString().Trim() + "</p>";
                    if (alertMessage.Length > 0)
                    {
                        body += "<p>Additional Info:<br>" + alertMessage + "</p>";
                    }

                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(mail_FromEmail);
                    message.To.Add(mail_ToEmail);
                    message.Subject = mail_Subject + " - BlastID: " + BlastID;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    message.Priority = MailPriority.High;

                    SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SmtpServer"]);
                    smtp.Send(message);
                }
                else
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError("Couldn't get blast info", "ReportSpam.SendEmailReportToAdmin", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                    SetError(Enums.ErrorMessage.InvalidLink);
                }                
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "ReportSpam.SendEmailReportToAdmin(EmailID: " + EmailID, Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                SetError(Enums.ErrorMessage.HardError);
            }
        }
        #endregion	

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
