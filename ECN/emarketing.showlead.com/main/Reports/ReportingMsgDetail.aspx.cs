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
using System.Configuration;

namespace ecn.showcare.wizard.main.Reports
{
	/// <summary>
	/// Summary description for ReportingMsgDetail.
	/// </summary>
	public class ReportingMsgDetail : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label mail_subject;
		protected System.Web.UI.WebControls.Label mail_from;
		//protected System.Web.UI.WebControls.HyperLink message;
		protected System.Web.UI.WebControls.Label message;
		protected System.Web.UI.WebControls.Label send_time;
		protected System.Web.UI.WebControls.Label finish_time;
		protected System.Web.UI.WebControls.Label success_rate;
		protected System.Web.UI.WebControls.Label opens_unique;
		protected System.Web.UI.WebControls.Label opens_total;
		protected System.Web.UI.WebControls.Label opens_percent;
		protected System.Web.UI.WebControls.Label clicks_unique;
		protected System.Web.UI.WebControls.Label clicks_total;
		protected System.Web.UI.WebControls.Label clicks_percent;
		protected System.Web.UI.WebControls.Label bounces_unique;
		protected System.Web.UI.WebControls.Label bounces_total;
		protected System.Web.UI.WebControls.Label bounces_percent;
		protected System.Web.UI.WebControls.Label forward_unique;
		protected System.Web.UI.WebControls.Label forward_total;
		protected System.Web.UI.WebControls.Label forward_percent;
		protected System.Web.UI.WebControls.Label lblMessage;
		protected System.Web.UI.WebControls.ImageButton backtoMain;
		protected Reporting r = null;
	
		protected string getBlastID () {
			string bid = "";
			try {
				bid = Request.QueryString["ID"];
			} catch {}
			return bid;
		}

		private void setToReporting() {
			try {
				r.BlastID = Request.QueryString["ID"];
			} catch (Exception err) {
				lblMessage.Text = "<B>Could not find ID.</B><BR>"+err.Message;
			}
		}

		private void PopulateData() {
			string bid = r.BlastID;
			
			if (!bid.Equals(null)) {
				string[] report_domains = new string[] {
                "msn.com", "aol.com","excite.com","yahoo.com"};

				Decimal SendTotal=0;
				Decimal Success=0;

				//set groups, campaign, Blasts info Labels
/*				string sqlgroupname = 
					" SELECT "+
					" 'GroupName' = CASE "+
					" WHEN LEN(ltrim(rtrim(g.GroupName))) <> 0 THEN g.GroupName "+
					" ELSE '< GROUP DELETED >' "+
					" END, "+
					" 'GrpNavigateURL' = CASE "+
					" WHEN LEN(ltrim(rtrim(g.GroupName))) <> 0 THEN '../lists/groupeditor.aspx?GroupID='+CONVERT(VARCHAR,g.GroupID) "+
					" ELSE '' "+
					" END, "+
					" 'FilterName' = CASE "+
					" WHEN f.FilterID <> 0 THEN f.FilterName "+
					" ELSE '< NO FILTER / FILTER DELETED >' "+
					" END, "+
					" 'FltrNavigateURL' = CASE "+
					" WHEN f.FilterID <> 0 THEN '../lists/filters.aspx?FilterID='+CONVERT(VARCHAR,f.FilterID)+'&action=editFilter' "+
					" ELSE '' "+
					" END, "+
					" 'LayoutName' = CASE "+
					" WHEN LEN(ltrim(rtrim(l.LayoutName))) <> 0 THEN l.LayoutName "+
					" ELSE '< CAMPAIGN DELETED >' "+
					" END, "+
					" 'LytNavigateURL' = CASE "+
					" WHEN LEN(ltrim(rtrim(l.LayoutName))) <> 0 THEN '../content/layouteditor.aspx?LayoutID='+CONVERT(VARCHAR,l.LayoutID)  "+
					" ELSE '' "+
					" END, "+
					" b.EmailSubject, b.EmailFromName, b.EmailFrom, b.SendTime, b.FinishTime, b.SuccessTotal, b.SendTotal "+
					" FROM Groups g JOIN Blasts b ON b.groupID = g.groupID LEFT OUTER JOIN Filters f ON b.filterID = f.filterID "+
					" JOIN Layouts l on b.LayoutID = l.LayoutID "+
					" WHERE b.BlastID = "+bid+
					" AND b.CustomerID = "+customerID;
*/
				string sql = 
					"SELECT  "+
						"'LayoutName' = CASE  "+
							"WHEN LEN(ltrim(rtrim(l.LayoutName))) <> 0 THEN l.LayoutName  ELSE '< CAMPAIGN DELETED >'  END, "+ 
//						"'LytNavigateURL' = CASE "+
//							"WHEN LEN(ltrim(rtrim(l.LayoutName))) <> 0 THEN '../content/layouteditor.aspx?LayoutID='+CONVERT(VARCHAR,l.LayoutID)   ELSE ''  END, "+ 
						"b.EmailSubject, b.EmailFromName, b.EmailFrom, b.SendTime, b.FinishTime, b.SuccessTotal, b.SendTotal "+ 
					"FROM Blasts b JOIN Layouts l on b.LayoutID = l.LayoutID "+
					"WHERE b.BlastID = "+bid+" AND b.CustomerID = "+r.CustomerID;

				DataTable grpNmDT = DataFunctions.GetDataTable(sql);
				foreach ( DataRow dr in grpNmDT.Rows ) {
					//GroupTo.Text			= dr["GroupName"].ToString();
					//GroupTo.NavigateUrl = dr["GrpNavigateURL"].ToString();
					//Filter.Text				= dr["FilterName"].ToString();
					//Filter.NavigateUrl		= dr["FltrNavigateURL"].ToString();
					message.Text			= dr["LayoutName"].ToString();
//					message.NavigateUrl = dr["LytNavigateURL"].ToString();
					mail_subject.Text		= dr["EmailSubject"].ToString();
					mail_from.Text		= dr["EmailFromName"].ToString()+" &lt;"+dr["EmailFrom"].ToString()+"&gt;";
					send_time.Text			= dr["SendTime"].ToString();
					finish_time.Text		= dr["FinishTime"].ToString();
				}

				string sendTotalSql = string.Format("select count(emailID) from BlastActivitySends with (nolock) where BlastID = {0}", bid);
				SendTotal = Convert.ToInt32(DataFunctions.ExecuteScalar("activity", sendTotalSql));

                string bounceTotalSql = string.Format("select count(distinct emailID) from BlastActivityBounces with (nolock) where BlastID = {0}", bid);
                int bounce = Convert.ToInt32(DataFunctions.ExecuteScalar("activity", bounceTotalSql));
				Success = SendTotal - bounce;

				Decimal clicksuniquecount = 0.0M;

				Decimal opensuniquecount = 0.0M;
			
				Decimal bouncesuniquecount = 0.0M;

//				Decimal subscribesuniquecount = 0.0M;

//				Decimal resendsuniquecount = 0.0M;

				Decimal forwardsuniquecount = 0.0M;

				//CLICKS
				string sqlCLICKS =
                    " select COUNT(DISTINCT bac.EmailID) as 'DistinctEmailCount', count(bac.EmailID) as 'EmailCount' " +
                    " FROM BlastActivityClicks bac with (nolock) JOIN ECN5_COMMUNICATOR..Emails e with (nolock) on e.EmailID = bac.EmailID  " +
                    " WHERE bac.BlastID = " + bid;

				DataTable clicksDT = DataFunctions.GetDataTable(sqlCLICKS, DataFunctions.GetConnectionString("activity"));
				foreach ( DataRow dr in clicksDT.Rows ) {
					clicksuniquecount	= Convert.ToDecimal(dr["DistinctEmailCount"].ToString());
					clicks_unique.Text	= dr["DistinctEmailCount"].ToString();
					clicks_total.Text		= dr["EmailCount"].ToString();
				}

				//OPENS 
				string sqlOPENS =
                    " select COUNT(DISTINCT bao.EmailID) as 'DistinctEmailCount', count(bao.EmailID) as 'EmailCount' " +
                    " BlastActivityOpens bao with (nolock) JOIN ECN5_COMMUNICATOR..Emails e with (nolock) on e.EmailID = bao.EmailID  " +
                    " WHERE bao.BlastID = " + bid;

                DataTable opensDT = DataFunctions.GetDataTable(sqlOPENS, DataFunctions.GetConnectionString("activity"));
				foreach ( DataRow dr in opensDT.Rows ) {
					opensuniquecount	= Convert.ToDecimal(dr["DistinctEmailCount"].ToString());
					opens_unique.Text	= dr["DistinctEmailCount"].ToString();
					opens_total.Text		= dr["EmailCount"].ToString();
				}

				//BOUNCES
				string sqlBOUNCES =
                    " select COUNT(DISTINCT bab.EmailID) as 'DistinctEmailCount', count(bab.EmailID) as 'EmailCount' " +
                    " FROM BlastActivityBounces bab with (nolock) JOIN ECN5_COMMUNICATOR..Emails e with (nolock) on e.EmailID = bab.EmailID  " +
                    " WHERE bab.BlastID = " + bid;

                DataTable bouncesDT = DataFunctions.GetDataTable(sqlBOUNCES, DataFunctions.GetConnectionString("activity"));
				foreach ( DataRow dr in bouncesDT.Rows ) {
					bouncesuniquecount	= Convert.ToDecimal(dr["DistinctEmailCount"].ToString());
					bounces_unique.Text	= dr["DistinctEmailCount"].ToString();
					bounces_total.Text	= dr["EmailCount"].ToString();
				}

/*
				//UNSUBSCRIBES
				string sqlUNSUBSCRIBES = 
					" select COUNT(DISTINCT eal.EmailID) as 'DistinctEmailCount', count(eal.EmailID) as 'EmailCount' "+
					" FROM EmailActivityLog eal JOIN Emails e on e.EmailID = eal.EmailID  "+
					" WHERE eal.BlastID = "+bid+"  AND eal.ActionTypeCode='subscribe' ";

				DataTable unSubsDT = DataFunctions.GetDataTable(sqlUNSUBSCRIBES);
				foreach ( DataRow dr in unSubsDT.Rows ) {
					subscribesuniquecount	= Convert.ToDecimal(dr["DistinctEmailCount"].ToString());
					SubscribesUnique.Text	= dr["DistinctEmailCount"].ToString();
					SubscribesTotal.Text	= dr["EmailCount"].ToString();
				}

				//RESENDS
				string sqlRESENDS = 
					" select COUNT(DISTINCT eal.EmailID) as 'DistinctEmailCount', count(eal.EmailID) as 'EmailCount' "+
					" FROM EmailActivityLog eal JOIN Emails e on e.EmailID = eal.EmailID  "+
					" WHERE eal.BlastID = "+bid+"  AND eal.ActionTypeCode='resend' ";

				DataTable resendDT = DataFunctions.GetDataTable(sqlRESENDS);
				foreach ( DataRow dr in resendDT.Rows ) {
					resendsuniquecount	= Convert.ToDecimal(dr["DistinctEmailCount"].ToString());
					ResendsUnique.Text	= dr["DistinctEmailCount"].ToString();
					ResendsTotal.Text	= dr["EmailCount"].ToString();
				}
*/
				//FORWARDS
				string sqlFORWARDS =
                    " select COUNT(DISTINCT bar.EmailID) as 'DistinctEmailCount', count(bar.EmailID) as 'EmailCount' " +
                    " FROM BlastActivityRefer bar with (nolock) JOIN ECN5_COMMUNICATOR..Emails e with (nolock) on e.EmailID = bar.EmailID  " +
                    " WHERE bar.BlastID = " + bid;

                DataTable forwardDT = DataFunctions.GetDataTable(sqlFORWARDS, DataFunctions.GetConnectionString("activity"));
				foreach ( DataRow dr in forwardDT.Rows ) {
					forwardsuniquecount		= Convert.ToDecimal(dr["DistinctEmailCount"].ToString());
					forward_unique.Text		= dr["DistinctEmailCount"].ToString();
					forward_total.Text		= dr["EmailCount"].ToString();
				}

				//Success = Success - bouncesuniquecount;
				//Successful.Text	= Success.ToString()+"/"+SendTotal.ToString();

				//set percentages
				success_rate.Text="("+Decimal.Round(((SendTotal==0?0:Success/SendTotal)*100),0).ToString()+"%)";
				clicks_percent.Text=Decimal.Round(((SendTotal==0?0:clicksuniquecount/SendTotal)*100),0).ToString()+" %";
				bounces_percent.Text=Decimal.Round(((SendTotal==0?0:bouncesuniquecount/SendTotal)*100),0).ToString()+" %";
				opens_percent.Text=Decimal.Round(((SendTotal==0?0:opensuniquecount/SendTotal)*100),0).ToString()+" %";
//				SubscribesPercentage.Text=Decimal.Round(((SendTotal==0?0:subscribesuniquecount/SendTotal)*100),0).ToString()+" %";
//				ResendsPercentage.Text=Decimal.Round(((SendTotal==0?0:resendsuniquecount/SendTotal)*100),0).ToString()+" %";
				forward_percent.Text=Decimal.Round(((SendTotal==0?0:forwardsuniquecount/SendTotal)*100),0).ToString()+" %";
			}
		}

		private void getReporting() {
			try {
				r = (Reporting) Session["reports"];
			} catch (Exception) {		// Here! means either the session has expired or user is trying to access this page wrong way. Redirect to Showcare
				Response.Redirect("http://www.showcare.com", true);
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Get reporting object from session
			getReporting();

			// Set Email
			//email.Text = r.Email;

			// Get BlastID once
			if (!IsPostBack)
				setToReporting();

			// Show Details
			PopulateData();
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
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.backtoMain.Click += new System.Web.UI.ImageClickEventHandler(this.backtoMain_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void backtoMain_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			Response.Redirect("ReportingMain.aspx");
		}

	}
}
