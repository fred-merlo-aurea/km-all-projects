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
using ecn.accounts.classes;
using ecn.communicator.classes;

namespace ecn.accounts.main.leads
{



    public partial class DemoSetup : ECN_Framework.WebPageHelper
	{
	
		#region Event Handlers
		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.LEADS;
            Master.SubMenu = "Demo Setup";
            Master.Heading = "Demo Setup";
            Master.HelpContent = "Demo Setup";
            Master.HelpTitle = "Demo Setup";
           
            if (!IsPostBack) {				
				calDemoDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
				ArrayList leads = GetLeadsBySelectedDate();
                LoadLeadsGrid(leads);
				UpdateSetupPanel(leads);
			}
		}
		
		protected void calDemoDate_SelectionChanged(object sender, System.EventArgs e) {
			ArrayList leads = GetLeadsBySelectedDate();
			LoadLeadsGrid(leads);
			UpdateSetupPanel(leads);
		}

		protected void btnSetup_Click(object sender, System.EventArgs e) {
			ArrayList leads = GetLeadsBySelectedDate();

			if (leads == null || leads.Count == 0) {			
				return;
			}
			
			foreach(Lead lead in leads) {
				Emails email = new Emails(lead.EmailID);
				email.SetValue("User2", SqlDbType.VarChar, 50, txtUrl.Text);
				email.SetValue("User3", SqlDbType.VarChar, 50, txtConferenceCall.Text);
				email.SetValue("User4", SqlDbType.VarChar, 50, txtMeetingID.Text);
			}			
			lblStatus.Text = "Demo info is saved to all users.";
		}
		#endregion

		private void LoadLeadsGrid(ArrayList leads) {
			dgdLeads.DataSource = leads;
			dgdLeads.DataBind();
			//btnSetup.Enabled = leads.Count > 0;
		}

		private ArrayList GetLeadsBySelectedDate() {
			DateTime today = calDemoDate.SelectedDate;
			return new Lead().GetLeadsWithDemo(today, today.AddDays(1));
		}

		private void UpdateSetupPanel(ArrayList leads) {
			lblStatus.Text = string.Empty;
			txtUrl.Text = txtConferenceCall.Text = txtMeetingID.Text = string.Empty;
			if (leads == null || leads.Count == 0) {
				lblMessage.Text = "No demo for this date";					
				return;
			}

			lblMessage.Text = string.Format("Setup Demo for users on {0}", calDemoDate.SelectedDate.ToShortDateString());

			DateTime today = calDemoDate.SelectedDate;
			ArrayList leadsWithDemoSetup = new Lead().GetLeadsWithDemoSetupInfo(today, today.AddDays(1));
			if (leadsWithDemoSetup == null || leadsWithDemoSetup.Count == 0) {				
				return;
			}

			Lead lead = leadsWithDemoSetup[0] as Lead;		
			Emails email = Emails.GetEmailByID(lead.EmailID);			
			if (email == null) {
				return;
			}

			txtUrl.Text = email.User2;
			txtConferenceCall.Text = email.User3;
			txtMeetingID.Text = email.User4;
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
