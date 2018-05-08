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
	/// Summary description for ReportingForwards.
	/// </summary>
	public class ReportingForwards : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.ImageButton backtoMain;
		protected System.Web.UI.WebControls.DataGrid dgForwards;
	
		protected Reporting r  = null;

		private void getReporting() {
			try {
				r = (Reporting) Session["reports"];
			} catch (Exception) {		// Here! means either the session has expired or user is trying to access this page wrong way. Redirect to Showcare
				Response.Redirect("http://www.showcare.com", true);
			}
		}

		private void PopulateForwardsDataGrid () {
			string sqlquery=
                " SELECT bar.EMailID, e.EmailAddress, bar.ReferTime AS ForwardTime, bar.EmailAddress as Referral, '>>' as ForwardTo, " +
				" 'EmailID='+CONVERT(VARCHAR,bar.EmailID)+'&GroupID='+CONVERT(VARCHAR,b.GroupID) AS 'URL' "+
                " FROM ECN5_COMMUNICATOR..Emails e with (nolock) JOIN BlastActivityRefer bar with (nolock) ON e.EMailID=bar.EMailID JOIN ECN5_COMMUNICATOR..Blasts b with (nolock) ON bar.BlastID = b.BlastID  " +
                " WHERE bar.BlastID=" + r.BlastID +
                " ORDER BY ForwardTime DESC";
            DataTable dt = DataFunctions.GetDataTable(sqlquery, DataFunctions.GetConnectionString("activity"));
			dgForwards.DataSource=dt.DefaultView;
			dgForwards.DataBind();
			//ForwardsPager.RecordCount = dt.Rows.Count;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// get Reporting object from session
			getReporting();

			// Set the email address on the top
			//email.Text = r.Email;

			// Populate Forwards data grid
			PopulateForwardsDataGrid();
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
