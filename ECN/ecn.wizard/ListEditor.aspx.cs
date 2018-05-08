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
using System.Data.SqlClient;
using System.IO;
using ecn.communicator.classes;

namespace ecn.wizard
{
	/// <summary>
	/// Summary description for ListEditor.
	/// </summary>
	public partial class ListEditor : ecn.wizard.MasterPage
	{

		string gid = null;
		DataTable dt = null;

		ArrayList columnHeadings = new ArrayList();
		string txtoutFilePath	= "";
		string fileDownloadPath = "";
		IEnumerator aListEnum = null;
		string downloadType=".xls";
		ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();
		protected System.Web.UI.WebControls.LinkButton lnkLogout;
		ECN_Framework.Common.SecurityCheck securityCheck = new ECN_Framework.Common.SecurityCheck();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Delete Email from the current list
			gid = Request.QueryString["gid"];
			btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
			pnlError.Visible=false;
			if (!IsPostBack)
			{
				LoadGrid();
				LoadListName();
				GetTotals();
			}
		}


		public int getGroupID() 
		{
			if (gid == null) 
			{
				try 
				{
					gid = Request.QueryString["gid"];
					return Convert.ToInt32(gid);
				} 
				catch
				{
					// Write error
				}
			}
			return Convert.ToInt32(gid);
		}

		private void LoadGrid() 
		{
			string sql = "SELECT e.EmailID, e.EmailAddress, e.FirstName, e.LastName, e.Voice as PhoneNumber, e.Company "+
				"FROM Emails e join EmailGroups eg on e.EmailID=eg.EmailID WHERE eg.GroupID=" + gid + " and SubscribeTypeCode='" + ddlEmailStatus.SelectedValue + "'";

			dt = DataFunctions.GetDataTable(sql, ConfigurationManager.AppSettings["com"]);

			dgList.DataSource = dt;
			dgList.DataBind();

			lblActiveTotal.Text = dt.Rows.Count.ToString();

			if (ddlEmailStatus.SelectedValue.ToUpper() == "S")
			{
				lblStatus.Text="Active ";
				dgList.Columns[6].Visible=true;
			}
			else
			{
				lblStatus.Text="Unsubscribe ";
				dgList.Columns[6].Visible=false;
			}
		}

		private void LoadListName() 
		{
			string sql = "SELECT GroupName FROM Groups WHERE GroupID="+gid;
			SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["com"]);
			SqlCommand com = new SqlCommand(sql, con);

			try 
			{
				con.Open();
				lblListName.Text = Convert.ToString(com.ExecuteScalar());
				txtListName.Text = lblListName.Text;
			} 
			catch
			{
				// Write Error
			} 
			finally 
			{
				con.Close();
			}
		}

		private void GetTotals() 
		{
			string sql = "SELECT count(*) FROM EmailGroups WHERE GroupID="+gid;
			SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["com"]);
			SqlCommand com = new SqlCommand(sql, con);
			try 
			{
				con.Open();	
				lblListTotal.Text = Convert.ToString(com.ExecuteScalar());
			} 
			catch  
			{
				// Write Error
			} 
			finally 
			{
				con.Close();
			}
		}

		public void DeleteEmail(int theEmailID) 
		{
			//delete EmailID from EmailGroups for that Group.	
			string sqlQuery	=	" DELETE FROM EmailGroups "+
				" WHERE EmailID="+theEmailID +
				" AND GroupID = "+ gid;
			try
			{
				DataFunctions.Execute(sqlQuery);
				//Response.Redirect("groupeditor.aspx?GroupID="+theGroupID);
			}
			catch(Exception ex)
			{
				PopUp.PopupMsg = "<font color='#FF0000'>Error Occured when Deleting EmailID : "+theEmailID+"</font> <br> "+ex.ToString();
				Response.Write("<script>javascript:window.open('../../UserControls/popup.aspx','Error', 'left=100,top=100,height=300,width=615,resizable=yes,scrollbar=yes,status=no' );</script>");					
			}			

			//check EmailGroups Table to see if the history of this email exists.
			sqlQuery =	" SELECT COUNT(*) FROM EmailGroups WHERE EmailID = "+theEmailID;
			int count = 0;
			try
			{
				count = Convert.ToInt32(DataFunctions.ExecuteScalar(sqlQuery).ToString());
			}
			catch(Exception ex)
			{
				PopUp.PopupMsg = "<font color='#FF0000'>Error Occured when Deleting EmailID : "+theEmailID+"</font> <br> "+ex.ToString();
				Response.Write("<script>javascript:window.open('../../UserControls/popup.aspx','Error', 'left=100,top=100,height=300,width=615,resizable=yes,scrollbar=yes,status=no' );</script>");				
			}

			//check to see if there's any other occurence.. 
			//if so it might be for another group.. so leave it if not delete it from the Emails Table.
			if((count == 0))
			{
				string sqlQuery2 =	" DELETE FROM Emails "+
					" WHERE EmailID = "+ theEmailID;
				try
				{
					DataFunctions.Execute(sqlQuery2);
				}
				catch(Exception ex)
				{
					PopUp.PopupMsg = "<font color='#FF0000'>Error Occured when Deleting EmailID : "+theEmailID+"</font> <br> "+ex.ToString();
					Response.Write("<script>javascript:window.open('../../UserControls/popup.aspx','Error', 'left=100,top=100,height=300,width=615,resizable=yes,scrollbar=yes,status=no' );</script>");					
				}
			}	

			//check to see if this email has any EmailDataValues in the EmailDataValues Table & delete by doing a join on the 
			//GroupDataFields Table using the groupID.
			string deleteFromEDV =	" DELETE FROM EmailDataValues FROM "+
				" EmailDataValues edv JOIN GroupDataFields gdf ON edv.GroupDataFieldsID = gdf.GroupDataFieldsID "+
				" WHERE gdf.groupID = "+gid+" AND edv.emailID = "+theEmailID;
			try
			{
				DataFunctions.Execute(deleteFromEDV);
			}
			catch(Exception ex)
			{
				PopUp.PopupMsg = "<font color='#FF0000'>Error Occured when Deleting EmailID : "+theEmailID+"</font> <br> "+ex.ToString();
				Response.Write("<script>javascript:window.open('../../UserControls/popup.aspx','Error', 'left=100,top=100,height=300,width=615,resizable=yes,scrollbar=yes,status=no' );</script>");					
			}
		}


		public void UnsubscribeEmail(int theEmailID) 
		{
			//delete EmailID from EmailGroups for that Group.	
			string sqlQuery	=	" Update EmailGroups Set SubscribeTypeCode ='U' WHERE EmailID=" + theEmailID + " AND GroupID = "+ gid;
			try
			{
				DataFunctions.Execute(sqlQuery);
			}
			catch(Exception ex)
			{
				PopUp.PopupMsg = "<font color='#FF0000'>Error Occured when Unsubscribing EmailID : "+theEmailID+"</font> <br> "+ex.ToString();
				Response.Write("<script>javascript:window.open('../../UserControls/popup.aspx','Error', 'left=100,top=100,height=300,width=615,resizable=yes,scrollbar=yes,status=no' );</script>");					
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
			this.btnSave.Click += new System.Web.UI.ImageClickEventHandler(this.btnSave_Click);
			this.btnExport.Click += new System.Web.UI.ImageClickEventHandler(this.btnExport_Click);
			this.btnRename.Click += new System.Web.UI.ImageClickEventHandler(this.btnRename_Click);
			this.btnDelete.Click += new System.Web.UI.ImageClickEventHandler(this.btnDelete_Click);
			this.dgList.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgList_ItemCommand);
			this.dgList.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgList_PageIndexChanged);
			this.dgList.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgList_ItemDataBound);
			this.btnPrevious.Click += new System.Web.UI.ImageClickEventHandler(this.btnPrevious_Click);
			this.btnSendMail.Click += new System.Web.UI.ImageClickEventHandler(this.btnSendMail_Click);

		}
		#endregion

		public void getDownloadProperties()
		{
			fileDownloadPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Channels/" + ChannelID + "/downloads/");
            txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Channels/" + ChannelID + "/downloads/");
		}

		private void btnExport_Click(object sender, System.Web.UI.ImageClickEventArgs e) 
		{
			string newline	= "";
			getDownloadProperties();
			string delimiter			= "";
			string contentType = "";
			string responseFile = "";

			delimiter			= "\t";
			contentType	= "application/vnd.ms-excel";
			responseFile	= "emails.xls";
			
			string filterSQL	 =	" SELECT e.EmailAddress, e.FirstName, e.LastName, e.Company, e.Voice as 'Phone'"+
				" FROM Emails e JOIN EmailGroups eg ON e.EmailID=eg.EmailID WHERE eg.GroupID="+gid+" ORDER BY e.EmailAddress";

			//output txt file format <customerID>-<groupID>-emails.txt
			DateTime date = DateTime.Now;
			String tfile = CustomerID+"_"+gid+"_emails"+downloadType;
			string outfileName		= txtoutFilePath+tfile;

			if(!Directory.Exists(txtoutFilePath))
			{
				Directory.CreateDirectory(txtoutFilePath);
			}
		

			if(File.Exists(outfileName))
			{
				File.Delete(outfileName);
			}

			TextWriter txtfile= File.AppendText(outfileName);
			//TextWriter schfile= File.AppendText(txtoutFilePath+schemaFile);			
			
			if(downloadType.Equals(".xls") || downloadType.Equals(".csv"))
			{

				// get the data from the database 
				// reset the IEnumerator Object of the ArrayList so tha the pointer is set.
				dt = DataFunctions.GetDataTable(filterSQL);
			
				columnHeadings = DataFunctions.GetDataTableColumns(dt);
				aListEnum = columnHeadings.GetEnumerator();
				while(aListEnum.MoveNext())
				{
					newline += aListEnum.Current.ToString()+delimiter;
				}
				txtfile.WriteLine(newline);

				foreach ( DataRow dr in dt.Rows ) 
				{
					newline = "";
					aListEnum.Reset();
					while(aListEnum.MoveNext())
					{
						newline += dr[aListEnum.Current.ToString()].ToString()+delimiter;
					}
					txtfile.WriteLine(newline);
				}
			}
			txtfile.Close();
			//schfile.Close();
			
			//create the zip file
			//download.createZipFile(outfileName, zipfileName, 9, 4096);
			Response.ContentType = contentType;
			Response.AddHeader( "content-disposition","attachment; filename="+responseFile);
			//Response.WriteFile(zipfileName);
			Response.WriteFile(outfileName);
			//Response.WriteFile(txtoutFilePath+schemaFile);
			Response.Flush();
			Response.End();

			if(File.Exists(outfileName))
			{
				File.Delete(outfileName);
			}
		}

		private void btnDelete_Click(object sender, System.Web.UI.ImageClickEventArgs e) 
		{
			string sql = "DELETE FROM EmailGroups WHERE GroupID="+gid;
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
			sql = "DELETE FROM Groups WHERE GroupId="+gid;
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

			Response.Redirect("default.aspx");
		}

		private void btnSendMail_Click(object sender, System.Web.UI.ImageClickEventArgs e) 
		{
			Response.Redirect("wizard.aspx?gid=" + gid);
		}

		private void dgList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			ImageButton img;
       
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				img = (ImageButton) e.Item.Cells[6].FindControl("cmddelete");            
				img.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");        

				img = (ImageButton) e.Item.Cells[5].FindControl("cmdUnsubscribe");            
				img.Attributes.Add("onclick", "return confirm('Are you sure you want to Unsubscribe?');");
			}
		}

		private void dgList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if (e.CommandName.ToLower() == "delete")
				DeleteEmail(Convert.ToInt32(dgList.DataKeys[e.Item.ItemIndex]));
			else if (e.CommandName.ToLower() == "unsubscribe")
				UnsubscribeEmail(Convert.ToInt32(dgList.DataKeys[e.Item.ItemIndex]));

			LoadGrid();
			GetTotals();

		}

		private void btnPrevious_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Response.Redirect("ListManage.aspx?gid=" + gid);	
		}

		private void btnRename_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			btnRename.Visible=false;
			btnSave.Visible=true;
			txtListName.Visible=true;
			lblListName.Visible=false;
		}

		private void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			string sql;
			// Check if Group by the same name exists...
			sql = "SELECT count(*) FROM Groups WHERE GroupName='"+txtListName.Text+"' AND "+
				"CustomerID="+ CustomerID+" AND GroupID<>"+ gid;

			int exists = Convert.ToInt32(DataFunctions.ExecuteScalar(sql));

			if (exists > 0) 
			{
				lblError.Text = "Group by the same name already exists. Please choose a different name.";
				pnlError.Visible=true;
				lblError.Visible=true;
			} 
			else 
		{
				sql = "update Groups Set GroupName=@spgn where groupID=@spgid";
				SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["com"]);
				SqlCommand com = new SqlCommand(sql, con);

				com.Parameters.Add(new SqlParameter("@spgn", txtListName.Text));
				com.Parameters.Add(new SqlParameter("@spgid", gid));

				try 
				{
					con.Open();
					com.ExecuteScalar();

					lblListName.Text = txtListName.Text;
					btnRename.Visible=true;
					lblListName.Visible=true;
					btnSave.Visible=false;
					txtListName.Visible=false;
				} 
				catch (Exception err) 
				{
					pnlError.Visible=true;
					lblError.Visible=true;
					lblError.Text = "Error:...<BR>"+err.Message;
				} 
				finally 
				{
					LoadGrid();
					GetTotals();
					con.Close();
				}
			}		
		}

		protected void ddlEmailStatus_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			dgList.CurrentPageIndex = 0;
			LoadGrid();
		}

		private void dgList_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			dgList.CurrentPageIndex = e.NewPageIndex;
			LoadGrid();
		}

	}																																																																												 
}
	