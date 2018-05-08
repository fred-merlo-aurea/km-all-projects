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
using System.Threading;
using ecn.communicator.classes;
using System.Configuration;

namespace ecn.showcare.wizard.main.Reports
{
	/// <summary>
	/// Summary description for ReportingBounces.
	/// </summary>
	public class ReportingBounces : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.ImageButton resend_soft_bounces;
		protected System.Web.UI.WebControls.DataGrid dgBounces;
		protected System.Web.UI.WebControls.DropDownList ddlType;
		protected System.Web.UI.WebControls.ImageButton btnDownload;
		protected System.Web.UI.WebControls.ImageButton backtoMain;
	
		protected Reporting r = null;
		protected ChannelCheck cc = new ChannelCheck();

		string txtoutFilePath	= "";
		ArrayList columnHeadings = new ArrayList();
		IEnumerator aListEnum = null;
		protected System.Web.UI.WebControls.Label email;
		DataTable emailstable;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Get the Reporting object from Session
			getReporting();

			// Show email address at the top
			email.Text = r.Email;

			// Load data into the Bounces data grid
			PopulateBounces();
		}

		private void getReporting() {
			try {
				r = (Reporting)Session["reports"];
			} catch {
				// this means either session is expired or trying invalid entry point. Send back to Showcare home
				Response.Redirect("http://www.showcare.com", true);
			}
		}

		private void PopulateBounces() {
			string sqlquery = 
					" SELECT e.EmailAddress as EmailAddress, bab.BounceTime as BounceTime, bc.BounceCode as BounceType, "+
					" bab.BounceMessage as BounceReason, e.FirstName as FirstName, e.LastName as LastName"+
					" FROM " +
                    "       ECN5_COMMUNICATOR..Emails e with (nolock) " +
                    "       JOIN BlastActivityBounces bab with (nolock) ON e.EMailID=bab.EMailID " +
                    "       JOIN BounceCodes bc with (nolock) ON bab.BounceCodeID = bc.BounceCodeID  " +
                    "       JOIN ECN5_COMMUNICATOR..Blasts b with (nolock) ON bab.BlastID = b.BlastID  " +
					" WHERE bab.BlastID="+r.BlastID+
                    " ORDER BY BounceTime DESC";

            DataTable dt = DataFunctions.GetDataTable(sqlquery, DataFunctions.GetConnectionString("activity"));
			dgBounces.DataSource=dt.DefaultView;
			dgBounces.CurrentPageIndex = 0;
			dgBounces.DataBind();
//			checkUnsubscribeBounces();
//			checkResendsoftBounces();
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
			this.btnDownload.Click += new System.Web.UI.ImageClickEventHandler(this.btnDownload_Click);
			this.backtoMain.Click += new System.Web.UI.ImageClickEventHandler(this.backtoMain_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void resend_soft_bounces_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			int blastid = Convert.ToInt32(r.BlastID);
			// resend the blast 
			EmailFunctions emailFunctions = new EmailFunctions();
			emailFunctions.MSPickupReSendBounceThread(blastid,cc.getVirtualPath("communicator"),cc.getHostName(),cc.getBounceDomain());
			//give it a second or two to start
			Thread.Sleep( 1000 );
		}

		public void getDownloadProperties(){
			txtoutFilePath	= Server.MapPath(cc.getAssetsPath("accounts")+"/channelID_"+r.ChannelID+"/customers/"+r.CustomerID+"/");
		}

		private void btnDownload_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			string newline	= "";
			getDownloadProperties();
			string downloadType	= ddlType.SelectedItem.Value.ToString();
			string downloadSQL	=
				" SELECT e.EmailAddress as EmailAddress, bab.BounceTime as BounceTime, bc.BounceCode as BounceType, "+
				" bab.BounceMessage as BounceReason, e.FirstName as FirstName, e.LastName as LastName"+
				" FROM "+
                "       ECN5_COMMUNICATOR..Emails e with (nolock) " +
                "       JOIN BlastActivityBounces bab with (nolock) ON e.EMailID=bab.EMailID " +
                "       JOIN BounceCodes bc with (nolock) ON bab.BounceCodeID = bc.BounceCodeID  " +
                "       JOIN ECN5_COMMUNICATOR..Blasts b with (nolock) ON bab.BlastID = b.BlastID  " +
				" WHERE bab.BlastID="+r.BlastID+
				" ORDER BY BounceTime DESC";

			//output file format <customerID>-<blastID>-open-emails.<downloadType>
			DateTime date = DateTime.Now;
			String tfile = txtoutFilePath + "bounced-emails"+downloadType;
			
			System.IO.TextWriter txtfile= System.IO.File.AppendText(tfile);
			
			columnHeadings.Insert(0,"BounceTime");
			columnHeadings.Insert(1,"EmailAddress");
			columnHeadings.Insert(2,"FirstName");
			columnHeadings.Insert(3,"LastName");
			columnHeadings.Insert(4,"BounceType");
			columnHeadings.Insert(5,"BounceReason");

			aListEnum = columnHeadings.GetEnumerator();
			while(aListEnum.MoveNext()){
				newline += aListEnum.Current.ToString()+", ";
			}
			txtfile.WriteLine(newline);

			// get the data from the database 
			// reset the IEnumerator Object of the ArrayList so tha the pointer is set.
            emailstable = DataFunctions.GetDataTable(downloadSQL, DataFunctions.GetConnectionString("activity"));
			foreach ( DataRow dr in emailstable.Rows ) {
				newline = "";
				aListEnum.Reset();
				while(aListEnum.MoveNext()){
					string val = dr[aListEnum.Current.ToString()].ToString();
					val = StringFunctions.Replace(val,"\r"," ");
					val = StringFunctions.Replace(val,"\n"," ");
					val = StringFunctions.Replace(val,"'","");
					val = StringFunctions.Replace(val,"\"","");
					newline += val.ToString()+", ";
				}
				txtfile.WriteLine(newline);
			}
			txtfile.Close();
			
			Response.ContentType = "text";
			Response.AddHeader( "content-disposition","attachment; filename=bounces"+downloadType);
			Response.WriteFile(tfile);
			Response.Flush();
			Response.End();
		}

		private void backtoMain_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			Response.Redirect("ReportingMain.aspx");
		}

	}
}
