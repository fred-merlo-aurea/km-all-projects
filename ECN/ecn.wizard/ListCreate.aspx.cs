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
using System.Configuration;
using System.Data.SqlClient;

using ecn.common.classes;

namespace ecn.wizard
{
	/// <summary>
	/// Summary description for ListCreate.
	/// </summary>
	public partial class ListCreate : ecn.wizard.MasterPage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblError.Text = "";
			Page.ClientScript.RegisterHiddenField( "__EVENTTARGET", btnSave.ClientID);
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

		}
		#endregion

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			string sql = "";
		
			if (Page.IsValid)
			{
			// Check if Group by the same name exists...
			sql = "SELECT count(*) FROM Groups WHERE GroupName='"+txtNewListName.Text.Replace("'","''") +"' AND "+
				"CustomerID="+ CustomerID;

			int exists = Convert.ToInt32(DataFunctions.ExecuteScalar(sql));

				if (exists > 0) 
				{
					lblError.Text = "Group by the same name already exists. Please choose a different name.";
				} 
				else 
				{
					sql = "INSERT INTO Groups (GroupName, CustomerID, FolderID, OwnerTypeCode, PublicFolder) "+
						"VALUES (@spgn, @spcid, @spfid, @spotc, @sppf); SELECT @@IDENTITY; ";
					SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["com"]);
					SqlCommand com = new SqlCommand(sql, con);

					com.Parameters.Add(new SqlParameter("@spgn", txtNewListName.Text));
					com.Parameters.Add(new SqlParameter("@spcid", CustomerID));
					com.Parameters.Add(new SqlParameter("@spfid", "0"));
					com.Parameters.Add(new SqlParameter("@spotc", "customer"));
					com.Parameters.Add(new SqlParameter("@sppf", "0"));

					try 
					{
						con.Open();
					
						Response.Redirect("ListManage.aspx?gid=" + Convert.ToString(com.ExecuteScalar()) + "&Message=" + txtNewListName.Text.Replace("'","''") + " has been successfully created.");
					} 
					catch (Exception err) 
					{
						lblError.Text = "Following error occured...<BR>"+err.Message;
					} 
					finally 
					{
						con.Close();
					}
				}
			}
		}
	}
}
