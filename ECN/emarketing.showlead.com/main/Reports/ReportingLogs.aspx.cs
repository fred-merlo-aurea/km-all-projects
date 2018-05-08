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

namespace ecn.showcare.wizard.main.Reports
{
	/// <summary>
	/// Summary description for ReportingLogs.
	/// </summary>
	public class ReportingLogs : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lblMsg;
		protected System.Web.UI.WebControls.DataGrid dgLogs;
	
		protected string BlastID = "";

		private void setBlastID () {
			try {
				BlastID = Request.QueryString["ID"];
			} catch (Exception err) {
				lblMsg.Text = err.Message;
			}
		}

		private void PopulateLogsDataGrid() {
			string sqlquery=
				" SELECT bas.SendTime As SendTime, e.EmailAddress, 'Y' as Success"+
                " FROM BlastActivitySends bas with (nolock) JOIN ECN5_COMMUNICATOR..Emails e with (nolock) ON e.EmailID = bas.EmailID" +
				" WHERE bas.BlastID="+BlastID;

			DataTable dt = DataFunctions.GetDataTable(sqlquery, DataFunctions.GetConnectionString("activity"));

            //sqlquery=
            //    " SELECT l.ActionDate As SendTime, e.EmailAddress, 'N' as Success"+
            //    " FROM EmailActivityLog l JOIN Emails e ON e.EmailID = l.EmailID"+
            //    " WHERE l.BlastID="+BlastID +
            //    " AND l.ActionTypeCode = 'nosend' ";
			
            //DataTable no_send_dt = DataFunctions.GetDataTable(sqlquery);
			
            //foreach(DataRow dr in no_send_dt.Rows) {
            //    dt.ImportRow(dr);
            //}

            //sqlquery=
            //    " SELECT l.ActionDate As SendTime, e.EmailAddress, 'T' as Success"+
            //    " FROM EmailActivityLog l JOIN Emails e ON e.EmailID = l.EmailID"+
            //    " WHERE l.BlastID="+BlastID +
            //    " AND l.ActionTypeCode = 'testsend' ";
			
            //DataTable test_send_dt = DataFunctions.GetDataTable(sqlquery);

            //foreach(DataRow dr in test_send_dt.Rows) {
            //    dt.ImportRow(dr);
            //}

			dgLogs.DataSource=dt.DefaultView;
			dgLogs.DataBind();
//			EmailsPager.RecordCount = dt.Rows.Count;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Get the blast id from query string
			setBlastID();

			if (BlastID.Equals(null)) {
				lblMsg.Text = "No logs found!";
			} else {
				PopulateLogsDataGrid();
			}
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
