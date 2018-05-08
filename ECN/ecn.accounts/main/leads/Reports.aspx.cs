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
using ecn.common.classes.billing;
using ecn.accounts.classes;
using ecn.accounts.includes;
using ecn.accounts.main.channels;

namespace ecn.accounts.main.leads
{



    public partial class Reports : ECN_Framework.WebPageHelper
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.LEADS;
            Master.SubMenu = "Reports";
            Master.Heading = "Leads Reports";
            Master.HelpContent = "";
            Master.HelpTitle = "Leads Reports";
           
            if (!IsPostBack) {
				LoadStaff();
				txtStartDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToShortDateString();
				txtEndDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToShortDateString();
				btnSearch_Click(null, null);
			}
		}

		private void LoadStaff() {
			if (Staff.CurrentStaff.Role == StaffRoleEnum.AccountManager) {
				ddlStaff.DataSource = Staff.GetStaff();
				ddlStaff.DataTextField = "Email";
				ddlStaff.DataValueField = "ID";
				ddlStaff.DataBind();
				ddlStaff.SelectedIndex = ddlStaff.Items.IndexOf(ddlStaff.Items.FindByValue(Staff.CurrentStaff.ID.ToString()));
			} else {
				ddlStaff.Items.Add(new ListItem(Staff.CurrentStaff.Email, Staff.CurrentStaff.ID.ToString()));
				ddlStaff.SelectedIndex = 0;
			}
		}

		private DateTime StartDate {
			get {
				try {
					return Convert.ToDateTime(txtStartDate.Text);
				} catch (Exception) {									
					txtStartDate.Text = DateTime.Now.ToShortDateString();
					return DateTime.Now;		
				}
			}
		}

		private DateTime EndDate {
			get {
				try {
					return Convert.ToDateTime(txtEndDate.Text);
				} catch( Exception) {					
					txtEndDate.Text = DateTime.Now.ToShortDateString();
					return DateTime.Now;		
				}
			}
		}

		private ArrayList GetLeadsByDateRange() {			
			return new Lead().GetLeadsByStafffIDAndDateRange(Convert.ToInt32(ddlStaff.SelectedValue), StartDate, EndDate);
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

		protected void btnSearch_Click(object sender, System.EventArgs e) {
			lblMessage.Visible = false;
			if (EndDate < StartDate) {
				lblMessage.Visible = true;
				lblMessage.Text = "Start date should be before 'End Date'.";
				return;
			}

			ArrayList leads = GetLeadsByDateRange();			
			ArrayList weeks = GetWeeks(StartDate, EndDate);
			plhReports.Controls.Clear();
			foreach(DateRange week in weeks) {
				LeadsWeeklyReport weeklyReport = this.LoadControl(@"..\..\includes\LeadsWeeklyReport.ascx") as LeadsWeeklyReport;
				weeklyReport.Show(Convert.ToInt32(ddlStaff.SelectedValue), week.Start, week.End, leads, chkDoNotShow.Checked);
				plhReports.Controls.Add(weeklyReport);
			}

			int leadsCount = leads.Count;
			int quoteCount = Quote.GetQuoteNumberByStaffIDAndDateRange(Convert.ToInt32(ddlStaff.SelectedValue), StartDate, EndDate);
			int demoCount = 0;
			foreach(Lead lead in leads) {
				if (lead.HasDemo) {
					demoCount ++;
				}
			}
			
			lblLeadsCount.Text = leadsCount.ToString();
			lblDemoCount.Text = demoCount.ToString();
			lblQuotes.Text = quoteCount.ToString();

			lblDemoRate.Text = string.Format("{0:0%}", leadsCount==0?0:Convert.ToDouble(demoCount)/leadsCount);
			lblQuoteRate.Text = string.Format("{0:0%}",leadsCount==0?0:Convert.ToDouble(quoteCount)/leadsCount);

		}

		private ArrayList GetWeeks(DateTime start, DateTime end) {
			ArrayList weeks = new ArrayList();
			DateTime startDayOfTheWeek = start;
			DateTime endDayOfTheWeek = start;
			do {
				endDayOfTheWeek = endDayOfTheWeek.AddDays(1);
				if (endDayOfTheWeek > end || endDayOfTheWeek.DayOfWeek == DayOfWeek.Saturday) {
					weeks.Add(new DateRange(startDayOfTheWeek, endDayOfTheWeek));
					startDayOfTheWeek = endDayOfTheWeek.AddDays(1);
				}				
			} while (startDayOfTheWeek <= end);
			
			return weeks;
		}
	}
}
