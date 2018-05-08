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

using ecn.common.classes;
using ecn.communicator.classes;
using ecn.accounts.classes;

namespace ecn.accounts.Engines
{
	public partial class ScheduleDemo : System.Web.UI.Page
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack) {
				LoadDemoSchedule();						
			}	
			if (HasEmailAddress) {
				CheckDemoDate();
			}		

			if (Email == null) {
				lblErrorMessage.Text = "Email address doesn't exist.";
				lblErrorMessage.Visible = true;
			}
			btnSchduleDemo.Enabled = btnDecline.Enabled = Email != null;
		}

		private bool HasEmailAddress {
			get { 
				return EmailAddress!=null && EmailAddress.Trim() != string.Empty;
			}
		}

		private string EmailAddress {
			get { return Request["EmailAddress"];}
		}

		private Emails _email = null;
		public Emails Email {
			get {
				if (_email == null) {
					_email = new Emails();
					_email.GetEmail(EmailAddress, LeadConfig.CustomerID);
					_email = Emails.GetEmailByID(_email.ID());
				}
				return (this._email);
			}			
		}


		private void LoadDemoSchedule() {
			ArrayList dates =  new ArrayList();
			for(int i=0; i< 35; i++) {
				DateTime theDay =DateTime.Now.AddDays(i+1);
				if (IsTheDayForDemo(theDay)) {
					TimeSpan diff = Convert.ToDateTime(theDay.ToShortDateString()) - Convert.ToDateTime(DateTime.Now.ToShortDateString());
					if (diff.Days > 1) {
						dates.Add(GetDemoSchedule(theDay));
					}
				}
			}
			ddlDemoDates.DataSource = dates;
			ddlDemoDates.DataBind();
		}

		private void CheckDemoDate() {	
			if (Email == null) {
				lblMessage.Text = string.Empty;
				return;
			}

			if (Email.UserEvent2Date == DateTime.MinValue) {
				lblMessage.Text = string.Empty;
				return;
			}
			DisplayDemoDate(Email.UserEvent2Date);
		}

		private void DisplayDemoDate(DateTime date) {
			lblMessage.Text = string.Format("You have scheduled a demo on '{0}'.", date.ToString());
		}

		private bool IsTheDayForDemo(DateTime date) {
			if (date.DayOfWeek == DayOfWeek.Tuesday||date.DayOfWeek==DayOfWeek.Thursday) {
				return true;
			}
			return false;
		}

		private string GetDemoSchedule(DateTime date) {
			if (date.DayOfWeek == DayOfWeek.Tuesday) {
				return new DateTime(date.Year,date.Month, date.Day,10,0,0).ToString();
			}

			if (date.DayOfWeek == DayOfWeek.Thursday) {
				return new DateTime(date.Year,date.Month, date.Day,14,0,0).ToString();
			}

			throw new ApplicationException("Can not find demo schdule for " + date.DayOfWeek.ToString());
		}

		private bool HasTimeSlotAvailableForTheDate(DateTime date) {
			string sql = string.Format("select count(*) from Emails e join EmailGroups eg on e.EmailID = eg.EmailID WHERE eg.GroupID = {0} and UserEvent2Date = '{1}'", LeadConfig.DemoInviteGroupID, date.ToString());
			int count = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", sql));
			return count<LeadConfig.MaxAllowedDemoNumber;
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

		/// User1 -- Staff First Name
		/// User2 -- Url for demo
		/// User3 -- Conference Call number for demo
		/// User4 -- Meeting ID
		/// User5 -- Date a customer decides to sign up for a demo.
		/// User6 -- Staff ID
		/// UserEvent1Date -- Invite Send Date
		/// UserEvent2Date -- Demo Date
		protected void btnSchduleDemo_Click(object sender, System.EventArgs e) {
			lblErrorMessage.Visible = false;
			DateTime date = Convert.ToDateTime(ddlDemoDates.SelectedValue);
			if (HasTimeSlotAvailableForTheDate(date)) {				
				Email.SetValue("UserEvent2Date",SqlDbType.DateTime, 8, date);
				Email.SetValue("UserEvent2",SqlDbType.VarChar, 50, "");
				Email.SetValue("User5", SqlDbType.VarChar, 50, DateTime.Now.ToString());
				DisplayDemoDate(date);

				DateTime today = new DateTime(date.Year, date.Month, date.Day);
				
				ArrayList leads = new Lead().GetLeadsWithDemoSetupInfo(today, today.AddDays(1));

				if (leads == null || leads.Count == 0) {			
					return;
				}

				Lead lead = leads[0] as Lead;		
				Emails email = Emails.GetEmailByID(lead.EmailID);			
				if (email == null) {
					return;
				}			

				Email.SetValue("User2", SqlDbType.VarChar, 50, email.User2);
				Email.SetValue("User3", SqlDbType.VarChar, 50, email.User3);
				Email.SetValue("User4", SqlDbType.VarChar, 50, email.User4);
				return;
			}

			lblErrorMessage.Visible = true;
			lblErrorMessage.Text = string.Format("'{0}' is not available. Please choose another demo time.", ddlDemoDates.SelectedValue);
		}

		protected void btnDecline_Click(object sender, System.EventArgs e) {
				Email.SetValue("UserEvent2Date",SqlDbType.DateTime, 8, DBNull.Value);
				Email.SetValue("UserEvent2",SqlDbType.VarChar, 50, "Decline");
				lblMessage.Text = string.Empty;
		}
	}
}
