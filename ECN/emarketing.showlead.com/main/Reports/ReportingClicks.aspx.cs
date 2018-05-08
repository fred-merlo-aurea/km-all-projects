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
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

namespace ecn.showcare.wizard.main.Reports
{
	/// <summary>
	/// Summary description for ReportingClicks.
	/// </summary>
	public class ReportingClicks : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.ImageButton backtoMain;
		protected System.Web.UI.WebControls.DataGrid dgTopClicks;
		protected System.Web.UI.WebControls.DataGrid dgTopVisitors;
		protected System.Web.UI.WebControls.DropDownList ddlDLType;
		protected System.Web.UI.WebControls.ImageButton btnDownload;
		protected System.Web.UI.WebControls.DataGrid dgClicksPerPerson;

		protected Reporting r = null;

		DownloadFunctions download = new DownloadFunctions();
		string txtoutFilePath	= "";
		string zipoutFilePath	= "";
		string fileDownloadPath = "";
		ArrayList columnHeadings = new ArrayList();
		IEnumerator aListEnum = null;
		DataTable emailstable;
		
		protected ChannelCheck cc = new ChannelCheck();
	
		private void getReporting() {
			try {
				r = (Reporting) Session["reports"];
			} catch (Exception) {		// Here! means either the session has expired or user is trying to access this page wrong way. Redirect to Showcare
				Response.Redirect("http://www.showcare.com", true);
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Get the Reporting object from Session
			getReporting();

			// Set header email address
			//email.Text = r.Email;

			// Top Click Thorughs
			GetTopClickThroughs();

			// Top Visitors
			GetTopVisitors();

			// Top Click Throughs by Person
			GetClickThroughsByPerson();
		}

		private string getLinkAlias(string BlastID, String Link) {
			string sqlquery=	" SELECT Alias FROM " +
				" Blasts b, Layouts l, Content c, linkAlias la "+
				" WHERE "+ 
				" b.blastID = "+BlastID+" AND b.layoutID = l.layoutID AND "+ 
				" (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR "+
				" l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR "+
				" l.ContentSlot9 = c.contentID) AND "+
				" la.ContentID = c.ContentID AND la.Link = '"+Link+"'";
			string alias = "";
			try{
				alias = DataFunctions.ExecuteScalar(sqlquery).ToString();
			}catch(Exception ){
				alias = "";
			}

			return alias;
		}

		private void GetTopClickThroughs() {
			SqlConnection dbConn		= new SqlConnection(ConfigurationManager.AppSettings["activity"]);
			SqlCommand topClicksCmd	= new SqlCommand("spClickActivity",dbConn);
			topClicksCmd.CommandTimeout	= 100;
			topClicksCmd.CommandType= CommandType.StoredProcedure;

			topClicksCmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
			topClicksCmd.Parameters["@BlastID"].Value = r.BlastID;
			topClicksCmd.Parameters.Add(new SqlParameter("@HowMuch", SqlDbType.VarChar));
			topClicksCmd.Parameters["@HowMuch"].Value = "TOP 15";

			SqlDataAdapter da = new SqlDataAdapter(topClicksCmd);
			DataSet ds = new DataSet();
			da.Fill(ds, "spClickActivity");
			dbConn.Close();

			DataTable dt = ds.Tables[0];

			DataTable newDT = new DataTable();
			newDT.Columns.Add(new DataColumn("ClickCount"));
			newDT.Columns.Add(new DataColumn("Url"));
			newDT.Columns.Add("LinkDetail", typeof(String));
			
			DataRow newDR;

			foreach ( DataRow dr in dt.Rows ) {
				string clickCount	= dr["ClickCount"].ToString();

				string linkDetail	= "BlastID=" + r.BlastID + "&link=" + HttpUtility.UrlEncode(dr["NewActionValue"].ToString());
				string fullLink		= dr["NewActionValue"].ToString();
				string smallLink		= dr["SmallLink"].ToString();
				string linkORalias	= "";
				
				newDR			= newDT.NewRow();
				newDR[0] = clickCount.ToString();
				//newDR[0]		= "<center><a href='clickslinks.aspx?"+linkDetail+"'>"+clickCount.ToString()+"</a></center>";
				string alias		= getLinkAlias(r.BlastID, fullLink);
				if(alias.Length > 0){
					linkORalias = alias;
				}else {
					linkORalias = smallLink;
				}
				newDR[1] = linkORalias.ToString();
				//newDR[1]		= "<a href='"+fullLink.ToString()+"' target='_blank'>"+linkORalias.ToString()+"</a>";

				newDT.Rows.Add(newDR);
			}
			dgTopClicks.DataSource = newDT.DefaultView;
			dgTopClicks.DataBind();
		}

		private void GetTopVisitors() {
			string sqlquery=
                " SELECT TOP 10 Count(bac.URL) AS ClickCount, e.EmailAddress, e.FirstName, e.LastName, e.Voice as Phone " +
                " FROM ECN5_COMMUNICATOR..Emails e with (nolock) JOIN BlastActivityClicks bac with (nolock) on e.EMailID=bac.EMailID JOIN ECN5_COMMUNICATOR..Blasts b with (nolock) on bac.BlastID = b.BlastID " +
                " WHERE bac.BlastID=" + r.BlastID +
				" GROUP BY e.EmailAddress, e.FirstName, e.LastName, e.Voice "+
				" ORDER BY ClickCount DESC, e.EmailAddress ";
            DataTable dt = DataFunctions.GetDataTable(sqlquery, DataFunctions.GetConnectionString("activity"));
			dgTopVisitors.DataSource = dt;
			dgTopVisitors.DataBind();
		}

		private void GetClickThroughsByPerson() {
			string sqlquery=
				" SELECT e.EmailAddress as EmailAddress, bac.ClickTime as ClickTime, e.FirstName, e.LastName, e.Voice as Phone,  "+
				" bac.URL AS FullLink, "+
				" 'EmailID='+CONVERT(VARCHAR,bac.EmailID)+'&GroupID='+CONVERT(VARCHAR,b.GroupID) AS 'URL',"+
				" CASE WHEN LEN(eal.ActionValue) > 6 THEN LEFT(RIGHT(eal.ActionValue,LEN(eal.ActionValue)-7),40) ELSE eal.ActionValue END AS SmallLink"+
                " FROM ECN5_COMMUNICATOR..Emails e with (nolock) JOIN BlastActivityClicks bac with (nolock) on e.EMailID=bac.EMailID JOIN ECN5_COMMUNICATOR..Blasts b with (nolock) on bac.BlastID = b.BlastID " +
                " WHERE bac.BlastID=" + r.BlastID +
                " ORDER BY ClickTime DESC";
            DataTable dt = DataFunctions.GetDataTable(sqlquery, DataFunctions.GetConnectionString("activity"));
			//ClicksGrid.DataSource=dt.DefaultView;
			//ClicksGrid.DataBind();

			DataTable newDT = new DataTable();
			newDT.Columns.Add(new DataColumn("ClickTime"));
			newDT.Columns.Add(new DataColumn("EmailAddress"));
			newDT.Columns.Add(new DataColumn("FirstName"));
			newDT.Columns.Add(new DataColumn("LastName"));
			newDT.Columns.Add(new DataColumn("Phone"));
			newDT.Columns.Add(new DataColumn("Url"));

			DataRow newDR;
			foreach ( DataRow dr in dt.Rows ) {
				string emailAdd	= dr["EmailAddress"].ToString();
				string clickTime	= dr["ClickTime"].ToString();
				string fullLink		= dr["FullLink"].ToString();
				string smallLink		= dr["SmallLink"].ToString();
				string url				= dr["URL"].ToString();
				string linkORalias	= "";
				
				newDR	= newDT.NewRow();
				newDR[0] = clickTime;
				newDR[1]	 = emailAdd;
				newDR[2] = dr["FirstName"].ToString();
				newDR[3] = dr["LastName"].ToString();
				newDR[4] = dr["Phone"].ToString();
				string alias		= getLinkAlias(r.BlastID, fullLink);
				if(alias.Length > 0){
					linkORalias = alias;
				}else {
					linkORalias = smallLink;
				}
				newDR[5] = linkORalias.ToString();
				//newDR[5]		= "<a href='"+fullLink.ToString()+"' target='_blank'>"+linkORalias.ToString()+"</a>";

				newDT.Rows.Add(newDR);
			}
			dgClicksPerPerson.DataSource = newDT.DefaultView;
			dgClicksPerPerson.DataBind();
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
			this.dgClicksPerPerson.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgClicksPerPerson_PageIndexChanged);
			this.backtoMain.Click += new System.Web.UI.ImageClickEventHandler(this.backtoMain_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void dgClicksPerPerson_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e) {
			dgClicksPerPerson.CurrentPageIndex = e.NewPageIndex;
			GetClickThroughsByPerson();
		}

		public void getDownloadProperties(){
			fileDownloadPath = "http://"+cc.getHostName()+cc.getAssetsPath("accounts")+"/channelID_"+r.ChannelID+"/";
			txtoutFilePath	= Server.MapPath(cc.getAssetsPath("accounts")+"/channelID_"+r.ChannelID+"/customers/"+r.CustomerID+"/");
			zipoutFilePath	= txtoutFilePath;
		}

		private void btnDownload_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			string newline	= "";
			getDownloadProperties();
			string downloadType	= ddlDLType.SelectedItem.Value.ToString();
			string downloadSQL			=	
				" SELECT bac.ClickTime as ClickTime, e.EmailAddress as EmailAddress, e.FirstName as FirstName, e.LastName as LastName, "+
				" e.Voice as Phone, bac.URL AS FullLink "+
                " FROM ECN5_COMMUNICATOR..Emails e with (nolock) JOIN BlastActivityClicks bac with (nolock) on e.EMailID=bac.EMailID " +
				" WHERE BlastID="+r.BlastID+
                " ORDER BY ClickTime DESC";

			//output txt file format <customerID>-<blastID>-open-emails.<downloadType>
			DateTime date = DateTime.Now;
			string tfile = txtoutFilePath+"click-emails"+downloadType;
			
			TextWriter txtfile= File.AppendText(tfile);
			
			columnHeadings.Insert(0,"ClickTime");
			columnHeadings.Insert(1,"EmailAddress");
			columnHeadings.Insert(2,"FirstName");
			columnHeadings.Insert(3,"LastName");
			columnHeadings.Insert(4,"Phone");
			columnHeadings.Insert(5,"FullLink");

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
					newline += dr[aListEnum.Current.ToString()].ToString()+", ";
				}
				txtfile.WriteLine(newline);
			}
			txtfile.Close();
			
			//create the zip file
			Response.ContentType = "text";
			Response.AddHeader( "content-disposition","attachment; filename=clicks"+downloadType);
			Response.WriteFile(tfile);
			Response.Flush();
			Response.End();
		}

		private void backtoMain_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			Response.Redirect("ReportingMain.aspx");
		}
	}
}
