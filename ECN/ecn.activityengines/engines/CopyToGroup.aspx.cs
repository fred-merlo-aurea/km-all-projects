using System;
using System.Collections;
using System.ComponentModel;

using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Configuration;

namespace ecn.activityengines.engines
{
    public partial class CopyToGroup : System.Web.UI.Page
    {
        private string EmailAddress = string.Empty;
        private string CustomerName = string.Empty;
        private string GroupName = string.Empty;
        private string GroupDescription = string.Empty;

        #region Get Request Variables
        private int getSourceGroupID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["sgID"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private int getDestinationGroupID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["dgID"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private int getEmailID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["e"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private string getRedirectURL()
        {
            try
            {
                return Request.QueryString["redirectURL"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            int sourceGroupID = getSourceGroupID();
            int destinationGroupID = getDestinationGroupID();
            int emailID = getEmailID();
            string url = getRedirectURL();
            if (sourceGroupID > 0 && destinationGroupID > 0 && emailID > 0)
            {
                if (!IsPostBack)
                {
                    KMPlatform.Entity.User User;

                    if (Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())] == null)
                    {
                        User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
                        Cache.Add(string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString()), User, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                    else
                    {
                        User = (KMPlatform.Entity.User)Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())];
                    }

                    ECN_Framework_Entities.Communicator.Group groupSource = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(sourceGroupID, User);
                    ECN_Framework_Entities.Communicator.Group groupDestination = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(destinationGroupID, User);

                    if (groupSource != null && groupDestination != null)
                    {
                        ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(groupSource.CustomerID, false);
                        if (customer != null)
                            CustomerName = customer.CustomerName;
                        ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(emailID, User);
                        EmailAddress = email.EmailAddress;
                        GroupName = groupSource.GroupName;
                        GroupDescription = groupSource.GroupDescription;

                        ECN_Framework_Entities.Accounts.LandingPageAssign LPA = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetOneToUse(1, groupSource.CustomerID, true);

                        Page.Title = "Copy To Group";

                        Label lblHeader = Master.FindControl("lblHeader") as Label;
                        lblHeader.Text = ReplaceStandardFields(LPA.Header);

                        Label lblFooter = Master.FindControl("lblFooter") as Label;
                        lblFooter.Text = ReplaceStandardFields(LPA.Footer);

                        try
                        {
                            ECN_Framework_BusinessLayer.Communicator.EmailGroup.CopyProfileFromGroup(sourceGroupID, destinationGroupID, emailID);

                            if (url != string.Empty)
                                Response.Redirect(url, false);
                            else
                                lblMessage.Text = "You have successfully copied subscriber information.";
                        }
                        catch (Exception ex)
                        {
                            //NotifyAdmin(ex);
                            KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "CopyToGroup.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                            lblMessage.Text = ActivityError.GetErrorMessage(Enums.ErrorMessage.HardError, "");
                        }
                    }
                    else
                    {
                        KM.Common.Entity.ApplicationLog.LogNonCriticalError("Querystring appears invalid", "CopyToGroup.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                        lblMessage.Text = ActivityError.GetErrorMessage(Enums.ErrorMessage.InvalidLink, "");
                    }
                }
            }
            else
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError("Querystring appears invalid", "CopyToGroup.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                lblMessage.Text = ActivityError.GetErrorMessage(Enums.ErrorMessage.InvalidLink, "");
            }
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
                adminEmailVariables.AppendLine("<br><b>Source Group ID:</b>&nbsp;" + getSourceGroupID());
                adminEmailVariables.AppendLine("<br><b>Destination Group ID:</b>&nbsp;" + getDestinationGroupID());
                adminEmailVariables.AppendLine("<br><b>Email ID:</b>&nbsp;" + getEmailID());
                adminEmailVariables.AppendLine("<br><b>Re-Direct URL:</b>&nbsp;" + getRedirectURL());
                adminEmailVariables.AppendLine("<BR>Page URL: " + Request.RawUrl.ToString());
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
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
