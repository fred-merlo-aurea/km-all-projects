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
using System.IO;
using ecn.common.classes;
using System.Configuration;

namespace ecn.wizard.Reports
{
	/// <summary>
	/// Summary description for ReportingBounces.
	/// </summary>
	public partial class ReportingBounces : ecn.wizard.MasterPage
	{
		protected ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();
		public int BlastID = -1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (Request.QueryString["ID"] != null && Request.QueryString["id"].ToString() != string.Empty )
			{
				BlastID = Convert.ToInt32(Request.QueryString["id"]);
			}
			if (!IsPostBack)
				loadBouncesGrid();
		}

		private void loadBouncesGrid() 
		{
			DataTable bouncesDT = null;
			string connString = ConfigurationManager.AppSettings["activity"];
			try
			{
				SqlConnection dbConn		= new SqlConnection(connString);			
				SqlCommand bouncesCmd	= new SqlCommand("spGetBouncesBlastReportData",dbConn);
				bouncesCmd.CommandTimeout = 0;
				bouncesCmd.CommandType	= CommandType.StoredProcedure;		
	
				bouncesCmd.Parameters.Add(new SqlParameter("@blastID", SqlDbType.VarChar));
				bouncesCmd.Parameters["@blastID"].Value = BlastID.ToString();		
				bouncesCmd.Parameters.Add(new SqlParameter("@imagePath", SqlDbType.VarChar));
				bouncesCmd.Parameters["@imagePath"].Value = "";	
				bouncesCmd.Parameters.Add(new SqlParameter("@bounceType", SqlDbType.VarChar));
				bouncesCmd.Parameters["@bounceType"].Value = BounceType.SelectedItem.Value.ToString();	
				bouncesCmd.Parameters.Add(new SqlParameter("@ISP", SqlDbType.VarChar));
				bouncesCmd.Parameters["@ISP"].Value = "";	

				SqlDataAdapter bouncesDA	= new SqlDataAdapter(bouncesCmd);
				DataSet bouncesDS				= new DataSet();			
				dbConn.Open();
				bouncesDA.Fill(bouncesDS, "spGetBouncesBlastReportData");
				dbConn.Close();
				bouncesDT = bouncesDS.Tables[0];
			}
			catch
			{
			}
			//DataTable dt = DataFunctions.GetDataTable(sqlquery);
			dgBounces.DataSource=bouncesDT.DefaultView;
			dgBounces.CurrentPageIndex = 0;
			dgBounces.DataBind();
			//BouncesPager.CurrentPage = 1;
			//BouncesPager.CurrentIndex = 0;
			if (bouncesDT.Rows.Count > 0)
			{
				BouncesPager.Visible = true;
				BouncesPager.RecordCount = bouncesDT.Rows.Count;
			}
			else
				BouncesPager.Visible = false;

		}

		public void UnsubscribeBounces(object sender, System.Web.UI.ImageClickEventArgs e) 
		{

			string connString = ConfigurationManager.AppSettings["activity"];
			try
			{
				SqlConnection dbConn		= new SqlConnection(connString);			
				SqlCommand bouncesCmd	= new SqlCommand("spUnsubscribeBounces",dbConn);
				bouncesCmd.CommandTimeout = 0;
				bouncesCmd.CommandType	= CommandType.StoredProcedure;		
	
				bouncesCmd.Parameters.Add(new SqlParameter("@blastID", SqlDbType.VarChar));
				bouncesCmd.Parameters["@blastID"].Value = BlastID.ToString();		

				bouncesCmd.Parameters.Add(new SqlParameter("@bounceType", SqlDbType.VarChar));
				bouncesCmd.Parameters["@bounceType"].Value = "hard";	

				bouncesCmd.Parameters.Add(new SqlParameter("@ISP", SqlDbType.VarChar));
				bouncesCmd.Parameters["@ISP"].Value = "";	

				dbConn.Open();
				bouncesCmd.ExecuteNonQuery();
				dbConn.Close();
				loadBouncesGrid();

			}
			catch
			{
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
			this.btnDownload.Click += new System.Web.UI.ImageClickEventHandler(this.btnDownload_Click);
			this.btnPrevious.Click += new System.Web.UI.ImageClickEventHandler(this.btnPrevious_Click);
			this.btnReportMenu.Click += new System.Web.UI.ImageClickEventHandler(this.btnReportMenu_Click);

		}
		#endregion

		private void btnDownload_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			string newline	= "";
            ArrayList columnHeadings = new ArrayList();
            IEnumerator aListEnum = null;
            DataTable emailstable = new DataTable();
            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Channels/" + ChannelID + "/downloads/");

            if (!Directory.Exists(txtoutFilePath))
                Directory.CreateDirectory(txtoutFilePath);

			string downloadType	= ddlType.SelectedItem.Value.ToString();
			string bounceType	= BounceType.SelectedItem.Value.ToString();

			string connString = ConfigurationManager.AppSettings["activity"];
			try
			{
				SqlConnection dbConn		= new SqlConnection(connString);			
				SqlCommand bouncesCmd	= new SqlCommand("spDownloadBouncesData",dbConn);
				bouncesCmd.CommandTimeout = 0;
				bouncesCmd.CommandType	= CommandType.StoredProcedure;		
	
				bouncesCmd.Parameters.Add(new SqlParameter("@blastID", SqlDbType.VarChar));
				bouncesCmd.Parameters["@blastID"].Value = BlastID.ToString();		

				bouncesCmd.Parameters.Add(new SqlParameter("@bounceType", SqlDbType.VarChar));
				bouncesCmd.Parameters["@bounceType"].Value = bounceType.Equals("*")?"":bounceType;	

				bouncesCmd.Parameters.Add(new SqlParameter("@ISP", SqlDbType.VarChar));
				bouncesCmd.Parameters["@ISP"].Value = "";	


				SqlDataAdapter bouncesDA	= new SqlDataAdapter(bouncesCmd);
				DataSet bouncesDS				= new DataSet();			
				dbConn.Open();
				bouncesDA.Fill(bouncesDS, "spGetBouncesBlastReportData");
				dbConn.Close();
				emailstable = bouncesDS.Tables[0];

			}
			catch
			{
			}

			//output file format <customerID>-<blastID>-open-emails.<downloadType>
			DateTime date = DateTime.Now;
			string tfile = txtoutFilePath+BlastID.ToString() + "-bounced-emails"+downloadType;

			if (File.Exists(tfile))
			{
				File.Delete(tfile);
			}

			TextWriter txtfile= File.AppendText(tfile);
			
			columnHeadings.Insert(0,"BounceTime");
			columnHeadings.Insert(1,"EmailAddress");
			columnHeadings.Insert(2,"BounceType");
			columnHeadings.Insert(3,"BounceSignature");

			aListEnum = columnHeadings.GetEnumerator();
			while(aListEnum.MoveNext())
			{
				newline += aListEnum.Current.ToString()+", ";
			}
			txtfile.WriteLine(newline);

			// reset the IEnumerator Object of the ArrayList so tha the pointer is set.
			foreach ( DataRow dr in emailstable.Rows ) 
			{
				newline = "";
				aListEnum.Reset();
				while(aListEnum.MoveNext())
				{
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
			//create the zip file
			Response.ContentType = "text";
			Response.AddHeader( "content-disposition","attachment; filename=bounces"+downloadType);
			Response.WriteFile(tfile);
			Response.Flush();
			Response.End();
		}

		private void btnPrevious_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
		Response.Redirect("ReportingMsgDetail.aspx?ID=" + BlastID);		
		}

		private void btnReportMenu_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
		Response.Redirect("ReportingMain.aspx");
		}

		protected void BouncesPager_IndexChanged(object sender, System.EventArgs e)
		{
			loadBouncesGrid();
		}

		private void resetGrid()
		{
			dgBounces.CurrentPageIndex = 0; 
			BouncesPager.CurrentPage = 1;
			BouncesPager.CurrentIndex = 0;	
			loadBouncesGrid();
		}

		protected void BounceType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			resetGrid();
			loadBouncesGrid();
		}

	}
}
