using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Configuration;
using ecn.communicator.classes;
using ecn.common.classes;
using System.Linq;
using System.Collections.Generic;

namespace ecn.activityengines
{

    public partial class emailtoafriend : System.Web.UI.Page
    {
        ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();
        int BlastID = 0;
        int EmailID = 0;
        private int Preview = 0;
        int RefBlastID = 0;
        private static int GroupID = 0;
        private static int CustomerID = 0;
        string EmailSubject = string.Empty;

        private string EmailAddress = string.Empty;
        private string CustomerName = string.Empty;
        private string GroupName = string.Empty;
        private string GroupDescription = string.Empty;

        private KMPlatform.Entity.User User;
        private ECN_Framework_Entities.Accounts.LandingPageAssign LPA;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
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

                    GetValuesFromQuerystring(Request.Url.Query.Substring(1, Request.Url.Query.Length - 1));

                    if (EmailID > 0 && BlastID > 0 && GetBlastInfo() && ECN_Framework_BusinessLayer.Communicator.EmailGroup.ValidForTracking(RefBlastID, EmailID))
                    {
                        ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(CustomerID, false);
                        if (customer != null)
                        {
                            CustomerName = customer.CustomerName;
                            cc = new ECN_Framework.Common.ChannelCheck(customer.CustomerID);
                        }
                        if (!IsPostBack)
                        {
                            lblFrom.Text = EmailAddress;
                            lblSubject.Text = EmailSubject;
                            pnlMain.Visible = true;
                            SetupPageValues();
                        }
                    }
                    else
                    {
                        //WGH: 10/31/2014 - Removing old logging
                        //KM.Common.Entity.ApplicationLog.LogNonCriticalError("One or more of the following:BlastID = 0,EmailID = 0,couldn't get blast info,not valid for tracking", "EmailToFriend.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                        SetError(Enums.ErrorMessage.InvalidLink);
                    }
                }
                else
                {
                    //WGH: 10/31/2014 - Removing old logging
                    //KM.Common.Entity.ApplicationLog.LogNonCriticalError("Querystring appears invalid", "EmailToFriend.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                    SetError(Enums.ErrorMessage.InvalidLink);
                }
            }
            catch (TimeoutException te)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(te, "EmailToFriend.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                SetError(Enums.ErrorMessage.HardError);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "EmailToFriend.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                SetError(Enums.ErrorMessage.HardError);
            }
        }

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

        private void GetValuesFromQuerystring(string queryString)
        {
            try
            {
                ECN_Framework_Common.Objects.QueryString qs = ECN_Framework_Common.Objects.QueryString.GetECNParameters(Server.UrlDecode(QSCleanUP(queryString)));
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.EmailID).ParameterValue, out EmailID);
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.BlastID).ParameterValue, out BlastID);
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.Preview).ParameterValue, out Preview);
            }
            catch { }
        }

        private string QSCleanUP(string querystring)
        {
            try
            {
                querystring = querystring.Replace("&amp;", "&");
                querystring = querystring.Replace("&lt;", "<");
                querystring = querystring.Replace("&gt;", ">");
            }
            catch (Exception)
            {
            }

            return querystring.Trim();
        }

        private bool GetBlastInfo()
        {
            bool success = false;
            RefBlastID = BlastID;
            try
            {
                ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.BlastAbstract.GetByBlastID_NoAccessCheck(BlastID, false);
                ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID_NoAccessCheck(EmailID);

                if (email != null && blast != null)
                {
                    CustomerID = blast.CustomerID.Value;
                    EmailAddress = email.EmailAddress;
                    EmailSubject = blast.EmailSubject;
                    if (blast.GroupID != null)
                    {
                        GroupID = blast.GroupID.Value;
                        ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(GroupID);
                        GroupName = group.GroupName;
                        GroupDescription = group.GroupDescription;
                    }
                    if (blast.BlastType.ToUpper() == "LAYOUT" || blast.BlastType.ToUpper() == "NOOPEN")
                    {
                        RefBlastID = ECN_Framework_BusinessLayer.Communicator.BlastSingle.GetRefBlastID(BlastID, email.EmailID, blast.CustomerID.Value, blast.BlastType);
                        blast = ECN_Framework_BusinessLayer.Communicator.BlastAbstract.GetByBlastID_NoAccessCheck(RefBlastID, false);
                        GroupID = blast.GroupID.Value;
                        ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(GroupID);
                        GroupName = group.GroupName;
                        GroupDescription = group.GroupDescription;

                    }
                    success = true;
                }
            }
            catch (Exception){  }
            return success;
        }

        private void SetupPageValues()
        {
            pnlMain.Visible = true;
            pnlThankYou.Visible = false;
            phError.Visible = false;

            SetHeaderFooter();

        }

        private void SetHeaderFooter()
        {
            //if it's a preview just show them the one they are requesting
            if (Preview > 0)
                LPA = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByLPAID(Preview, true);
            else
                LPA = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetOneToUse(3, CustomerID, true);

            Page.Title = "Email to a Friend";

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

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            //string admimEmailBody = string.Empty;

            try
            {
                adminEmailVariables.AppendLine("<BR><BR>BlastID: " + BlastID.ToString());
                adminEmailVariables.AppendLine("<BR>EmailID: " + EmailID.ToString());
                adminEmailVariables.AppendLine("<BR>GroupID: " + GroupID.ToString());
                adminEmailVariables.AppendLine("<BR>CustomerID: " + CustomerID.ToString());
                try
                {
                    adminEmailVariables.AppendLine("<BR>Page URL: " + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + Request.RawUrl.ToString());
                    adminEmailVariables.AppendLine("<BR>SPY Info:&nbsp;[" + Request.UserHostAddress + "] / [" + Request.UserAgent + "]");
                    if (Request.UrlReferrer != null)
                    {
                        adminEmailVariables.AppendLine("<BR>Referring URL: " + Request.UrlReferrer.ToString());
                    }
                    adminEmailVariables.AppendLine("<BR>HEADERS");
                    var headers = String.Empty;
                    foreach (var key in Request.Headers.AllKeys)
                        headers += "<BR>" + key + ":" + Request.Headers[key];
                    adminEmailVariables.AppendLine(headers);
                }
                catch (Exception)   {}
                
            }
            catch (Exception) { }

            return adminEmailVariables.ToString();
        }

        private void SendBlast(int theBlastID, int theEmailID, string theEmail, int fromEmailID)
        {

            // The Event code should handle all of this....

            try
            {
                EmailFunctions emailFunctions = new EmailFunctions();
                emailFunctions.SendSingle(theBlastID, theEmailID, theEmail, System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"], cc.getHostName(), cc.getBounceDomain(), User, fromEmailID);
            }
            catch (Exception)
            {
                List<string> sb = new List<string>();
                if (theBlastID == null || theBlastID <= 0)
                    sb.Add("theBlastID is null or less than zero");
                if (theEmailID == null || theEmailID <= 0)
                    sb.Add("theEmailID is null or less than zero");
                if (theEmail == null || theEmail.Trim().Length == 0)
                    sb.Add("theEmail is null or length is zero");
                if (System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"] == null || System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"].Trim().Length == 0)
                    sb.Add("Communicator_VirtualPath is null or length is zero");
                if (cc == null)
                    sb.Add("cc is null");
                else
                {
                    try
                    {
                        if (cc.getHostName() == null || cc.getHostName().Trim().Length == 0)
                            sb.Add("cc.getHostName() is null or length is zero");
                    }
                    catch (Exception)
                    {
                        sb.Add("cc.getHostName() is null or length is zero");
                    }
                    try
                    {
                        if (cc.getBounceDomain() == null || cc.getBounceDomain().Trim().Length == 0)
                            sb.Add("cc.getBounceDomain() is null or length is zero");
                    }
                    catch (Exception)
                    {
                        sb.Add("cc.getBounceDomain() is null or length is zero");
                    }
                }
                if (User == null)
                    sb.Add("User is null");
                if (fromEmailID == null || fromEmailID <= 0)
                    sb.Add("fromEmailID is null or less than zero");

                if (sb.Count > 0)
                {
                    List<ECN_Framework_Common.Objects.ECNError> errorList = new List<ECN_Framework_Common.Objects.ECNError>();                    

                    foreach (string err in sb)
                    {
                        errorList.Add(new ECN_Framework_Common.Objects.ECNError(ECN_Framework_Common.Objects.Enums.Entity.Page, ECN_Framework_Common.Objects.Enums.Method.None, err.ToString().Trim()));
                    }

                    throw new ECN_Framework_Common.Objects.ECNException(errorList);
                }
            }
        }




        /// Track a "Friend" referral for this blast.

        /// <param name="theBlastID"> The Blast ID with the Forward link in it.</param>
        /// <param name="RefererID">The Email ID of who refered us.</param>
        /// <param name="theEmail"> The email address of who we want to add. </param>
        /// <param name="theName"> The "FullName" column of the Emails table for the new add.</param>
        /// <returns>EmailID</returns>
        private int TrackData(string theEmail, string theName)
        {
            string my_note = string.Empty;
            ECN_Framework_Entities.Communicator.Email email = null;
            try
            {
                my_note = txtNote.Text;
                my_note = my_note.Replace("\r\n", "<br>");
                my_note = my_note.Replace("'", "");
                Session.Add("F2FNotes", my_note);

                // Log it for reporting
                string spyinfo = string.Empty;
                if (Request.UserHostAddress != null)
                {
                    spyinfo = Request.UserHostAddress;
                }
                if (Request.UserAgent != null)
                {
                    if (spyinfo.Length > 0)
                    {
                        spyinfo += " | ";
                    }
                    spyinfo += Request.UserAgent;
                }
                ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.InsertForward(EmailID, BlastID, theEmail, spyinfo, User);

                string xmlInsert = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML><Emails><emailaddress>" + theEmail + "</emailaddress></Emails></XML>";
                DataTable emailRecordsDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(User, CustomerID, GroupID, xmlInsert, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>", "html", "P", true, "", "ActivityEngine.EmailToFriend");
                email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(theEmail, CustomerID);                
            }
            catch (Exception)
            {
                List<string> sb = new List<string>();
                if (my_note == null || my_note.Trim().Length == 0)
                    sb.Add("my_note is null or length is zero");
                if (theEmail == null || theEmail.Trim().Length == 0)
                    sb.Add("theEmail is null or length is zero");
                if (theName == null || theName.Trim().Length == 0)
                    sb.Add("theName is null or length is zero");
                if (System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"] == null || System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"].Trim().Length == 0)
                    sb.Add("Communicator_VirtualPath is null or length is zero");
                if (User == null)
                    sb.Add("User is null");
                if (EmailID == null || EmailID <= 0)
                    sb.Add("EmailID is null or less than zero");
                if (BlastID == null || BlastID <= 0)
                    sb.Add("BlastID is null or less than zero");
                if (CustomerID == null || CustomerID <= 0)
                    sb.Add("CustomerID is null or less than zero");
                if (GroupID == null || GroupID <= 0)
                    sb.Add("GroupID is null or less than zero");

                if (sb.Count > 0)
                {
                    List<ECN_Framework_Common.Objects.ECNError> errorList = new List<ECN_Framework_Common.Objects.ECNError>();

                    foreach (string err in sb)
                    {
                        errorList.Add(new ECN_Framework_Common.Objects.ECNError(ECN_Framework_Common.Objects.Enums.Entity.Page, ECN_Framework_Common.Objects.Enums.Method.None, err.ToString().Trim()));
                    }

                    throw new ECN_Framework_Common.Objects.ECNException(errorList);
                }
            }

            return email != null && email.EmailID > 0 ? email.EmailID : -1;
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int theNewID = 0;
            string sentemails = "";
            string alreadySentEmails = "";
            string invalidEmails = "";
            string existingEmails = "";
            try
            {
                if (Preview <= 0)
                {
                    if (txtEmail1.Text != string.Empty)
                    {
                        if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(txtEmail1.Text))
                        {
                            if (!ECN_Framework_BusinessLayer.Communicator.EmailGroup.Exists(txtEmail1.Text, GroupID, CustomerID))
                            {
                                theNewID = TrackData(txtEmail1.Text, txtName1.Text);
                                if (ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.GetSendCount(theNewID, BlastID) <= 0)
                                {
                                    if (theNewID > 0)
                                    {
                                        SendBlast(BlastID, theNewID, lblFrom.Text, EmailID);
                                        sentemails += txtEmail1.Text + "<br>";
                                    }
                                    else
                                    {
                                        invalidEmails += txtEmail1.Text + "<br>";
                                    }
                                }
                                else
                                {
                                    alreadySentEmails += txtEmail1.Text + "<br>";
                                }
                            }
                            else
                            {
                                existingEmails += txtEmail1.Text + "<br>";
                            }
                        }
                        else
                        {
                            invalidEmails += txtEmail1.Text + "<br>";
                        }
                    }
                    if (txtEmail2.Text != string.Empty)
                    {
                        if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(txtEmail2.Text))
                        {
                            if (!ECN_Framework_BusinessLayer.Communicator.EmailGroup.Exists(txtEmail2.Text, GroupID, CustomerID))
                            {
                                theNewID = TrackData(txtEmail2.Text, txtName2.Text);
                                if (ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.GetSendCount(theNewID, BlastID) <= 0)
                                {
                                    if (theNewID > 0)
                                    {
                                        SendBlast(BlastID, theNewID, lblFrom.Text, EmailID);
                                        sentemails += txtEmail2.Text + "<br>";
                                    }
                                    else
                                    {
                                        invalidEmails += txtEmail2.Text + "<br>";
                                    }
                                }
                                else
                                {
                                    alreadySentEmails += txtEmail2.Text + "<br>";
                                }
                            }
                            else
                            {
                                existingEmails += txtEmail2.Text + "<br>";
                            }
                        }
                        else
                        {
                            invalidEmails += txtEmail2.Text + "<br>";
                        }
                    }
                    if (txtEmail3.Text != string.Empty)
                    {
                        if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(txtEmail3.Text))
                        {
                            if (!ECN_Framework_BusinessLayer.Communicator.EmailGroup.Exists(txtEmail3.Text, GroupID, CustomerID))
                            {
                                theNewID = TrackData(txtEmail3.Text, txtName3.Text);
                                if (ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.GetSendCount(theNewID, BlastID) <= 0)
                                {
                                    if (theNewID > 0)
                                    {
                                        SendBlast(BlastID, theNewID, lblFrom.Text, EmailID);
                                        sentemails += txtEmail3.Text + "<br>";
                                    }
                                    else
                                    {
                                        invalidEmails += txtEmail3.Text + "<br>";
                                    }

                                }
                                else
                                {
                                    alreadySentEmails += txtEmail3.Text + "<br>";
                                }
                            }
                            else
                            {
                                existingEmails += txtEmail3.Text + "<br>";
                            }
                        }
                        else
                        {
                            invalidEmails += txtEmail3.Text + "<br>";
                        }
                    }
                    if (txtEmail4.Text != string.Empty)
                    {
                        if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(txtEmail4.Text))
                        {
                            if (!ECN_Framework_BusinessLayer.Communicator.EmailGroup.Exists(txtEmail4.Text, GroupID, CustomerID))
                            {
                                theNewID = TrackData(txtEmail4.Text, txtName4.Text);
                                if (ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.GetSendCount(theNewID, BlastID) <= 0)
                                {
                                    if (theNewID > 0)
                                    {
                                        SendBlast(BlastID, theNewID, lblFrom.Text, EmailID);
                                        sentemails += txtEmail4.Text + "<br>";
                                    }
                                    else
                                    {
                                        invalidEmails += txtEmail4.Text + "<br>";
                                    }
                                }
                                else
                                {
                                    alreadySentEmails += txtEmail4.Text + "<br>";
                                }
                            }
                            else
                            {
                                existingEmails += txtEmail4.Text + "<br>";
                            }
                        }
                        else
                        {
                            invalidEmails += txtEmail4.Text + "<br>";
                        }
                    }
                    if (txtEmail5.Text != string.Empty)
                    {
                        if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(txtEmail5.Text))
                        {
                            if (!ECN_Framework_BusinessLayer.Communicator.EmailGroup.Exists(txtEmail5.Text, GroupID, CustomerID))
                            {
                                theNewID = TrackData(txtEmail5.Text, txtName5.Text);
                                if (ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.GetSendCount(theNewID, BlastID) <= 0)
                                {
                                    if (theNewID > 0)
                                    {
                                        SendBlast(BlastID, theNewID, lblFrom.Text, EmailID);
                                        sentemails += txtEmail5.Text + "<br>";
                                    }
                                    else
                                    {
                                        invalidEmails += txtEmail5.Text + "<br>";
                                    }
                                }
                                else
                                {
                                    alreadySentEmails += txtEmail5.Text + "<br>";
                                }
                            }
                            else
                            {
                                existingEmails += txtEmail5.Text + "<br>";
                            }
                        }
                        else
                        {
                            invalidEmails += txtEmail5.Text + "<br>";
                        }
                    }
                }
                else
                {
                    if(txtEmail1.Text != string.Empty)
                        sentemails += txtEmail1.Text + "<br>";
                    if (txtEmail2.Text != string.Empty)
                        alreadySentEmails += txtEmail2.Text + "<br>";
                    if (txtEmail3.Text != string.Empty)
                        invalidEmails += txtEmail3.Text + "<br>";
                    if (txtEmail4.Text != string.Empty)
                        sentemails += txtEmail4.Text + "<br>";
                    if (txtEmail5.Text != string.Empty)
                        sentemails += txtEmail5.Text + "<br>";
                }
                string resultMessage = "";
                if (sentemails != "")
                {
                    resultMessage = "<b>Message has been successfully forwarded to:</b><BR><i>" + sentemails + "</i><br><br>";
                }
                if (alreadySentEmails != "")
                {
                    resultMessage += "<b>Message has already been sent to:</b><BR><i>" + alreadySentEmails + "</i><br><br>";
                }
                if (invalidEmails != "")
                {
                    resultMessage += "<b>Message cannot be sent to invalid email address:</b><BR><i>" + invalidEmails + "</i><br><br>";
                }
                if (existingEmails != "")
                {
                    resultMessage += "<b>Message cannot be sent to existing email address:</b><BR><i>" + existingEmails + "</i><br><br>";
                }                
                pnlThankYou.Visible = true;
                lblConfirmation.Text = resultMessage + "Thank you!";
                pnlMain.Visible = false;
                phError.Visible = false;
            }
            catch (ECN_Framework_Common.Objects.ECNException ECNex)
            {
                string errorMessage = string.Empty;
                foreach (ECN_Framework_Common.Objects.ECNError ecnError in ECNex.ErrorList)
                {
                    errorMessage += "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                }
                KM.Common.Entity.ApplicationLog.LogNonCriticalError("Invalid Form Post", "EmailToFriend.btnSubmit_Click", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), errorMessage);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "EmailToFriend.btnSubmit_Click", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                SetError(Enums.ErrorMessage.HardError);
            }
            Session.Remove("F2FNotes");
        }
    }
}
