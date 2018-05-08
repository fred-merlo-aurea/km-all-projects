using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using ecn.activityengines;

namespace ecn.communicator.engines {
	/// <summary>
	/// Summary description for referralProgram.
	/// </summary>
	public partial class referralProgram : System.Web.UI.Page
    {
        private KMPlatform.Entity.User User;
        private ECN_Framework_Entities.Accounts.LandingPageAssign LPA;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //redirect to unsubscribe as this page is old
            KM.Common.Entity.ApplicationLog.LogNonCriticalError("Should no longer be using this page", "ReferralProgram.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
            SetError(Enums.ErrorMessage.PageNotFound);
        }

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();

            try
            {
                adminEmailVariables.AppendLine("<br><b>Customer ID:</b>&nbsp;" + getCustomerID());
                adminEmailVariables.AppendLine("<br><b>Group ID:</b>&nbsp;" + getGroupID());
                adminEmailVariables.AppendLine("<br><b>Referral Email ID:</b>&nbsp;" + getRefererEmailID());
                adminEmailVariables.AppendLine("<br><b>Referral Program ID:</b>&nbsp;" + getReferralProgramID());
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }

        private void SetError(Enums.ErrorMessage errorMessage, string pageMessage = "")
        {
            phError.Visible = true;
            lblErrorMessage.Text = ActivityError.GetErrorMessage(errorMessage, pageMessage);
            LPA = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetOneToUse(2, -1, true);

            Page.Title = "Referral Program";

            Label lblHeader = Master.FindControl("lblHeader") as Label;
            lblHeader.Text = LPA.Header;

            Label lblFooter = Master.FindControl("lblFooter") as Label;
            lblFooter.Text = LPA.Footer;
        }

		#region Get Request Variables methods [ getReferralEmailID, getCustomerID, getBlastID, getEmailAddress, getReturnURL, getFullName, getGroupID, getSubscribe, getFormat, getFirstName getLastName, getFullName, getStreetAddress, getCompanyName, getCity, getState, getZipCode, getPhone, getBirthdate] 

		private int getRefererEmailID() {
			int theRefererEmailID = 0;
			try {
				theRefererEmailID = Convert.ToInt32(Request.QueryString["reID"].ToString());
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theRefererEmailID;
		}

		private int getCustomerID() {
			int theCustomerID = 0;
			try {
				theCustomerID = Convert.ToInt32(Request.QueryString["c"].ToString());
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theCustomerID;
		}

		private string getReferralProgramID() {
			string theReferralProgramID = "";
			try {
				theReferralProgramID	= Request.QueryString["rpID"].ToString();
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theReferralProgramID;
		}

		private string getGroupID() {
			string theGroupID= "";
			try {
				theGroupID	= Request.QueryString["g"].ToString();
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theGroupID;
		}
        #endregion
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {    
		}
		#endregion
	}
}
