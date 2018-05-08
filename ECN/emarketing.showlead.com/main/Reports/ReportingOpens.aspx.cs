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
using System.Data.SqlClient;

using ecn.common.classes;
using System.IO;
using System.Configuration;

namespace ecn.showcare.wizard.main.Reports
{
	/// <summary>
	/// Summary description for ReportingOpens.
	/// </summary>
	public class ReportingOpens : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DataGrid dgActive;
		protected System.Web.UI.WebControls.DropDownList ddlDLType;
		protected System.Web.UI.WebControls.ImageButton btnDl;
		protected System.Web.UI.WebControls.DataGrid dgOpens;
		protected System.Web.UI.WebControls.ImageButton backtoMain;
	
		protected Reporting r;
		protected string bid="";

		protected ChannelCheck cc = new ChannelCheck();

		DownloadFunctions download = new DownloadFunctions();
		string txtoutFilePath	= "";
		string zipoutFilePath	= "";
		string fileDownloadPath = "";
		ArrayList columnHeadings = new ArrayList();
		IEnumerator aListEnum = null;
		DataTable emailstable;

		private void getReporting() {
			try {
				r = (Reporting)Session["reports"];
			} catch {
				// this means either session is expired or trying invalid entry point. Send back to Showcare home
				Response.Redirect("http://www.showcare.com", true);
			}
		}

		protected string getBlastID () {
			try {
				bid = Request.QueryString["ID"];
			} catch {
				// Ideally display Error Message
			}
			return bid;
		}

		private void PopulateMostActiveOpens() {
			string sql=
                " SELECT " +
                " 	TOP 15 COUNT(bao.emailID) AS ActionCount, e.emailaddress as EmailAddress, e.FirstName, e.LastName, e.Voice AS Phone " +
                " FROM " +
                " 	ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) " +
                " 	JOIN BlastActivityOpens bao WITH (NOLOCK) ON bao.emailid=e.emailid " +
                " 	JOIN ECN5_COMMUNICATOR..Blasts b WITH (NOLOCK) ON bao.BlastID = b.BlastID " +
                " WHERE " +
                " 	bao.blastid=" + r.BlastID +
                " GROUP BY " +
                " 	bao.emailid, e.emailaddress, e.FirstName, e.LastName, e.Voice " +
                " ORDER BY ActionCount DESC ";
            DataTable dt = DataFunctions.GetDataTable(sql, DataFunctions.GetConnectionString("activity"));
			dgActive.DataSource = dt;
			dgActive.DataBind();
		}

		private void PopulateListOfOpens() {
			string sql=
				" SELECT " + 
                " 	e.EmailAddress, bao.OpenTime AS OpenTime, e.FirstName, e.LastName, e.Voice AS Phone " + 
                " FROM " +
                " 	ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) " +
                " 	JOIN BlastActivityOpens bao WITH (NOLOCK) ON e.EMailID=bao.EMailID " +
                " 	JOIN ECN5_COMMUNICATOR..Blasts b WITH (NOLOCK) ON bao.BlastID = b.BlastID " + 
                " WHERE " +
                " 	b.BlastID="+r.BlastID+
                " ORDER BY ActionDate DESC ";
            
			DataTable dt = DataFunctions.GetDataTable(sql, DataFunctions.GetConnectionString("activity"));
			dgOpens.DataSource = dt.DefaultView;
			dgOpens.DataBind();
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Get the Reporting object from Session
			getReporting();

			// Set header email address
			//email.Text = r.Email;

			// Populate First DataGrid
			PopulateMostActiveOpens();

			// Populate Second DataFrid
			PopulateListOfOpens();
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
			this.btnDl.Click += new System.Web.UI.ImageClickEventHandler(this.btnDl_Click);
			this.dgOpens.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgOpens_PageIndexChanged);
			this.backtoMain.Click += new System.Web.UI.ImageClickEventHandler(this.backtoMain_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void dgOpens_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e) {
			dgOpens.CurrentPageIndex = e.NewPageIndex;
			PopulateListOfOpens();
		}

		public void getDownloadProperties(){
			fileDownloadPath = "http://"+cc.getHostName()+cc.getAssetsPath("accounts")+"/channelID_"+r.ChannelID+"/";
			txtoutFilePath	= Server.MapPath(cc.getAssetsPath("accounts")+"/channelID_"+r.ChannelID+"/customers/"+r.CustomerID+"/");
			zipoutFilePath	= txtoutFilePath;
		}

		private void btnDl_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			string newline = "";
			getDownloadProperties();
			string downloadType = ddlDLType.SelectedItem.Value.ToString();
			string downloadSQL	 =
                " SELECT bao.OpenTime as OpenTime, e.EmailAddress as EmailAddress, e.FirstName as FirstName, e.LastName as LastName, e.Voice as Phone " +
                " FROM " +
                " 	BlastActivityOpens bao WITH (NOLOCK) " +
                " 	JOIN ECN5_COMMUNICATOR..Emails e WITH (NOLOCK) ON bao.EmailID = e.EmailID " +
                " WHERE " +
                " 	bao.BlastID=" + r.BlastID +
                " ORDER BY OpenTime DESC ";

			//output txt file format <customerID>-<blastID>-open-emails.<downloadType>
			DateTime date = DateTime.Now;
			string tfile = txtoutFilePath+"open-emails"+downloadType;
			
			TextWriter txtfile= File.AppendText(tfile);
			
			columnHeadings.Insert(0,"OpenTime");
			columnHeadings.Insert(1,"EmailAddress");
			columnHeadings.Insert(2,"FirstName");
			columnHeadings.Insert(3,"LastName");
			columnHeadings.Insert(4,"Phone");

			aListEnum = columnHeadings.GetEnumerator();
			while(aListEnum.MoveNext()) {
				newline += aListEnum.Current.ToString()+", ";
			}
			txtfile.WriteLine(newline);

			// get the data from the database 
			// reset the IEnumerator Object of the ArrayList so tha the pointer is set.
			emailstable = DataFunctions.GetDataTable(downloadSQL, DataFunctions.GetConnectionString("activity"));
			foreach ( DataRow dr in emailstable.Rows ) {
				newline = "";
				aListEnum.Reset();
				while(aListEnum.MoveNext()) {
					newline += dr[aListEnum.Current.ToString()].ToString()+", ";
				}
				txtfile.WriteLine(newline);
			}
			txtfile.Close();
			
			Response.ContentType = "text";
			Response.AddHeader( "content-disposition","attachment; filename=opens"+downloadType);
			Response.WriteFile(tfile);
			Response.Flush();
			Response.End();
			
			//	Response.Redirect(fileDownloadPath+zfile);
		}

		private void backtoMain_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			Response.Redirect("ReportingMain.aspx");
		}

	}
}
