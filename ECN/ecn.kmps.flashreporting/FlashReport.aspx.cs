using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.common.classes;

namespace ecn.kmps.FlashReporting {
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public partial class FlashReport : System.Web.UI.Page 	{
		
		ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

		public static string comm_connString{
			get { return ConfigurationManager.AppSettings["com"].ToString(); }	
		}
	
		protected void Page_Load(object sender, System.EventArgs e) {
			// Put user code to initialize the page here
			if(!(Page.IsPostBack))
				loadPubDR();
		}

		public void loadPubDR(){

            string selectGroupsSQL =
                        " SELECT GroupID, GroupName  FROM Groups g join Folders f on g.FolderID = f.folderID " +
                        " WHERE g.CustomerID = " + sc.CustomerID() + " AND f.FolderName like '%group%'  AND GroupName not like '%Supression%'" +
                        " ORDER BY GroupName ";

			DataTable dt = DataFunctions.GetDataTable(selectGroupsSQL, comm_connString);
			PubGroupID.DataSource = dt.DefaultView;
			PubGroupID.DataBind();
		}


		protected void SubmitBtn_Click(object sender, System.EventArgs e) {
			int groupID	= 0;
			try{
				groupID	= Convert.ToInt32(PubGroupID.SelectedValue.ToString());
			}catch(Exception){}
			int  custID	= Convert.ToInt32(sc.CustomerID().ToString());

			SqlConnection dbConn		= new SqlConnection(comm_connString);
			SqlCommand resultsCmd	= new SqlCommand("sp_KMPS_FlashReporting",dbConn);
			resultsCmd.CommandTimeout	= 0;
			resultsCmd.CommandType= CommandType.StoredProcedure;

			resultsCmd.Parameters.Add(new SqlParameter("@pubGroupID", SqlDbType.Int));
			resultsCmd.Parameters["@pubGroupID"].Value = groupID;

			resultsCmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
			resultsCmd.Parameters["@CustomerID"].Value = custID;

			resultsCmd.Parameters.Add(new SqlParameter("@PromoCode", SqlDbType.VarChar,25));
			resultsCmd.Parameters["@PromoCode"].Value = PromoCode.Text.ToString().Trim();

			resultsCmd.Parameters.Add(new SqlParameter("@fromdt", SqlDbType.VarChar,10));
			resultsCmd.Parameters["@fromdt"].Value = FromDate.Text.ToString().Trim();

			resultsCmd.Parameters.Add(new SqlParameter("@todt", SqlDbType.VarChar,10));
			resultsCmd.Parameters["@todt"].Value = ToDate.Text.ToString().Trim();

			SqlDataAdapter da = new SqlDataAdapter(resultsCmd);
			DataSet ds = new DataSet();
			dbConn.Open();
			da.Fill(ds, "sp_KMPS_FlashReporting");
			dbConn.Close();

			ResultsGrid.DataSource = ds.Tables[0].DefaultView;
			ResultsGrid.DataBind();
		}
	}
}
