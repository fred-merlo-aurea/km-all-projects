namespace ecn.accounts.includes
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.accounts.classes;
	using ecn.common.classes;
	using ecn.common.classes.billing;

	
	///		Summary description for LeadsWeeklyReport.
	
	public partial class LeadsWeeklyReport : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			
		}

		public void Show(int staffID, DateTime start, DateTime end, ArrayList leads, bool hidePanelWithZeroLeads) {
			lblFrom.Text = start.ToShortDateString();
			lblTo.Text = end.ToShortDateString();

			Hashtable weeklyLeadsStats = new Hashtable();
			Hashtable weeklyDemoStats = new Hashtable();
			ArrayList weeklyCallRecords = CallRecord.GetCallRecords(staffID, start,end);
			int leadsCount = 0;
			int demoCount = 0;
			foreach(Lead lead in leads) {
				if (lead.SendDate < start || lead.SendDate > end) {
					continue;
				}
				IncreaseLeadsCount(weeklyLeadsStats, lead.SendDate.DayOfWeek);
				leadsCount ++;

				// Just calculate invites which generate demos. Use sendDate here, not demo date.
				if (lead.HasDemo) {
					IncreaseLeadsCount(weeklyDemoStats, lead.SendDate.DayOfWeek);
					demoCount ++;
				}
			}

			if (leadsCount == 0 && hidePanelWithZeroLeads) {
				HtmlTable tblLeadsReport = this.FindControl("tblLeadsReport") as HtmlTable;
				tblLeadsReport.Visible = false;
				return;
			}
		
			lblSunday.Text = string.Format("Invites:{0}<br/>Demos:{1}<br/>C#:{2}", GetLeadsCount(weeklyLeadsStats, DayOfWeek.Sunday), GetLeadsCount(weeklyDemoStats, DayOfWeek.Sunday),GetCallCount(weeklyCallRecords,DayOfWeek.Sunday));
			lblMonday.Text = string.Format("Invites:{0}<br/>Demos:{1}<br/>C#:{2}", GetLeadsCount(weeklyLeadsStats, DayOfWeek.Monday), GetLeadsCount(weeklyDemoStats, DayOfWeek.Monday),GetCallCount(weeklyCallRecords,DayOfWeek.Monday));
			lblTuesday.Text = string.Format("Invites:{0}<br/>Demos:{1}<br/>C#:{2}", GetLeadsCount(weeklyLeadsStats, DayOfWeek.Tuesday), GetLeadsCount(weeklyDemoStats, DayOfWeek.Tuesday),GetCallCount(weeklyCallRecords,DayOfWeek.Tuesday));
			lblWednesday.Text = string.Format("Invites:{0}<br/>Demos:{1}<br/>C#:{2}", GetLeadsCount(weeklyLeadsStats, DayOfWeek.Wednesday), GetLeadsCount(weeklyDemoStats, DayOfWeek.Wednesday),GetCallCount(weeklyCallRecords,DayOfWeek.Wednesday));
			lblThursday.Text = string.Format("Invites:{0}<br/>Demos:{1}<br/>C#:{2}", GetLeadsCount(weeklyLeadsStats, DayOfWeek.Thursday), GetLeadsCount(weeklyDemoStats, DayOfWeek.Thursday),GetCallCount(weeklyCallRecords,DayOfWeek.Thursday));
			lblFriday.Text = string.Format("Invites:{0}<br/>Demos:{1}<br/>C#:{2}", GetLeadsCount(weeklyLeadsStats, DayOfWeek.Friday), GetLeadsCount(weeklyDemoStats, DayOfWeek.Friday),GetCallCount(weeklyCallRecords,DayOfWeek.Friday));
			lblSaturday.Text = string.Format("Invites:{0}<br/>Demos:{1}<br/>C#:{2}", GetLeadsCount(weeklyLeadsStats, DayOfWeek.Saturday), GetLeadsCount(weeklyDemoStats, DayOfWeek.Saturday),GetCallCount(weeklyCallRecords,DayOfWeek.Saturday));


			int quoteCount = Quote.GetQuoteNumberByStaffIDAndDateRange(staffID, start, end);
			lblLeadsCount.Text = leadsCount.ToString();
			lblDemoCount.Text = demoCount.ToString();
			lblQuotes.Text = quoteCount.ToString();

			lblDemoRate.Text = string.Format("{0:0%}", leadsCount==0?0:Convert.ToDouble(demoCount)/leadsCount);
			lblQuoteRate.Text = string.Format("{0:0%}",leadsCount==0?0:Convert.ToDouble(quoteCount)/leadsCount);
			
		}

		private void IncreaseLeadsCount(Hashtable stats, DayOfWeek dayOfWeek) {
			if (stats[dayOfWeek] == null) {
				stats.Add(dayOfWeek, 1);
				return;
			}

			int count = Convert.ToInt32(stats[dayOfWeek]);
			stats[dayOfWeek] = count + 1;
		}

		private int GetLeadsCount(Hashtable stats, DayOfWeek dayOfWeek) {
			if (stats[dayOfWeek] == null) {				
				return 0 ;
			}

			return Convert.ToInt32(stats[dayOfWeek]);			
		}

		private int GetCallCount(ArrayList weeklyCallRecords, DayOfWeek dayOfWeek) {
			foreach(CallRecord record in weeklyCallRecords) {
				if (record.CallDate.DayOfWeek == dayOfWeek) {
					return record.CallCount;
				}
			}
			return 0;
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
		
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
