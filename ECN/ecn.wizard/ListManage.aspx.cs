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
using System.Configuration;
using ecn.common.classes;
using System.IO;
using ecn.communicator.classes;

namespace ecn.wizard
{
	/// <summary>
	/// Summary description for ListManage.
	/// </summary>
	public partial class ListManage : ecn.wizard.MasterPage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			errlabel.Visible = false;
			btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
			rblFileType.Attributes.Add("onclick", "SetWorkSheet();");

			if (!IsPostBack)
			{
				if (Convert.ToInt32(DataFunctions.ExecuteScalar("accounts", "select count(*)  from wizard_basefee where basechannelID = " + ChannelID + " and ChargeCrCard= 'Y'")) > 0)
				{					
					btnPricing.Attributes.Add("onclick","openwindow();");
				}
				else
				{
					btnPricing.Visible=false;
				}
				LoadGroupList();
			}
		}


		private void LoadGroupList()
		{
			string sql = "SELECT GroupID, GroupName FROM Groups WHERE MasterSupression = 0 and CustomerID="+CustomerID + " order by GroupName ";

			DataTable dt = DataFunctions.GetDataTable(sql, ConfigurationManager.AppSettings["com"]);

			ddlList.DataSource = dt;
			ddlList.DataTextField = "GroupName";
			ddlList.DataValueField = "GroupID";
			ddlList.DataBind();	
		
			if (Request.QueryString["gid"] != null && Request.QueryString["gid"].ToString() != string.Empty )
			{
				ListItem gSelected = ddlList.Items.FindByValue(Request.QueryString["gid"].ToString());
				if (gSelected != null)
				{
					gSelected.Selected = true;				
				}
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
			this.btnViewEdit.Click += new System.Web.UI.ImageClickEventHandler(this.btnViewEdit_Click);
			this.btnDelete.Click += new System.Web.UI.ImageClickEventHandler(this.btnDelete_Click);
			this.btnImportData.Click += new System.Web.UI.ImageClickEventHandler(this.btnImportData_Click);

		}
		#endregion

		private void btnViewEdit_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			if (ddlList.SelectedIndex >= 0)
			{
				Response.Redirect("ListEditor.aspx?gid=" + ddlList.SelectedValue);
			}
			else
			{
				errlabel.Text = "Please select the list.";
				errlabel.Visible = true;
			}
		}

		private void btnDelete_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			string sql = "DELETE FROM EmailGroups WHERE GroupID="+ddlList.SelectedValue;
			SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["com"]);
			SqlCommand com = new SqlCommand(sql, con);

			// Delete from EmailGroups
			try 
			{
				con.Open();
				com.ExecuteNonQuery();
			} 
			catch 
			{
				// Write Error
			}

			// Delete from Groups
			sql = "DELETE FROM Groups WHERE GroupId="+ddlList.SelectedValue;
			com.CommandText = sql;

			try 
			{
				com.ExecuteNonQuery();
			} 
			catch 
			{
				// Write Error
			} 
			finally 
			{
				con.Close();
			}
			LoadGroupList();
		}

		private void ShowError(string ErrMessage)
		{
			errlabel.Text = ErrMessage;
			errlabel.Visible=true;
		}

		private void btnImportData_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			if (ddlList.SelectedIndex < 0)
			{
				ShowError("Please select the list.");

			}
			else if ((rblFileType.SelectedIndex == 1) && (txtSheetName.Text.Length <= 0)) 
			{
				ShowError("Please enter the sheet name for the excel file.");
			} 
			else 
			{
				string fileName = "";
				string fileType="";

				fileName = System.IO.Path.GetFileName(fBrowse.PostedFile.FileName).Replace(" ","_");;
				
				if (fileName == string.Empty)
				{
					ShowError("File must be specified to import.");
				}
				else
				{
					if (!(fileName.ToLower().EndsWith(".xls") || fileName.ToLower().EndsWith(".csv"))) 
					{
						ShowError("Only Excel or CSV files can be uploaded.");
					} 
					else 
					{
						ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();

						string targetPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Customers/" + CustomerID + "/data/");                            

						try 
						{
							fileType = (fileName.ToLower().EndsWith(".xls")) ? "X" : "C";

							if(!Directory.Exists(targetPath))
							{
								Directory.CreateDirectory(targetPath);
							}

							fBrowse.PostedFile.SaveAs(targetPath + fileName);

							Response.Redirect("Importer.aspx?fn="+fileName+"&ft="+fileType+"&gid="+ddlList.SelectedValue+"&sheet=" + txtSheetName.Text);
						} 
						catch (Exception err) 
						{						
							ShowError("Could not save file. <BR>[Error: " + err.Message + "]");
						}
					}
				}
			}
		}
	}
}
