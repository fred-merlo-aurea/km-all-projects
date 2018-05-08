namespace ecn.wizard.wizard
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Data.SqlClient;
	using ecn.common.classes;
	using System.Configuration;

	public partial class ChooseTemplate : ecn.wizard.MasterControl, IWizard
	{

		private int index = 0;

		public bool Save() 
		{
			// Get the selected TemplateID
			ResetSession();

			int i = 0;
			for (i=0; i<index; i++) 
			{
				HtmlInputRadioButton hirb = (HtmlInputRadioButton) templatesTable.FindControl("rblTemplates"+i);
				if (hirb.Checked) 
				{

					if (WizardSession.TemplateID != Convert.ToInt32(hirb.Value))
					{
						WizardSession.TemplateID = Convert.ToInt32(hirb.Value);
						WizardSession.ContentSource = "";
						WizardSession.ContentText = "";
					}

					string selectQuery = "SELECT  TemplateName FROM Templates WHERE TemplateID=" + hirb.Value;

					if (DataFunctions.ExecuteScalar(selectQuery).ToString().ToLower() == "custom_template")
						WizardSession.IsCustomHeader = true;
					else
						WizardSession.IsCustomHeader = false;

					break;
				}
			}

			if (i >= index)
				lblError.Text = "Select a Template";
			
			if (rbGroup.Checked && ddlEmailList.SelectedIndex <= 0)
				lblError.Text = "Select an Email List";
			else if (rbSingle.Checked && (txtfirstName.Text.Trim() == string.Empty || txtLastName.Text.Trim() == string.Empty || txtEmailaddress.Text.Trim() == string.Empty))
				lblError.Text = "Enter firstname, LastName and EmailAddress";

			if (lblError.Text != string.Empty) 
			{
				if (rbGroup.Checked)
				{
					txtfirstName.Enabled = false;
					txtLastName.Enabled=false;
					txtEmailaddress.Enabled=false;
					ddlEmailList.Enabled=true;				
				}
				else if(rbSingle.Checked)
				{
					ddlEmailList.Enabled=false;
					txtfirstName.Enabled = true;
					txtLastName.Enabled=true;
					txtEmailaddress.Enabled=true;
				} 

				lblError.Visible = true;
				return false;
			} 
			else 
			{
				if (rbGroup.Checked)
				{
					WizardSession.GroupID = Convert.ToInt32(ddlEmailList.SelectedValue);
				}
				else if (rbSingle.Checked)
				{

					string sqlquery = string.Format(@"SELECT GroupID FROM Groups WHERE GroupName = 'BlastSingleEmail' and CustomerID={0}", CustomerID.ToString());
					int GroupID  = Convert.ToInt32(DataFunctions.ExecuteScalar(ConfigurationManager.AppSettings["com"], sqlquery));

					if (GroupID == 0)
					{
						sqlquery = " insert into groups (CustomerID, FolderID, GroupName, GroupDescription, OwnerTypeCode, MasterSupression, PublicFolder, OptinHTML, OptinFields, AllowUDFHistory) "+
							" VALUES ( @custID, 0, 'BlastSingleEmail', '', 'customer', 0, 1, '', '', 'N');SELECT @@IDENTITY";

						// Create Connection
						SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["com"]);	
						SqlCommand cmd = new SqlCommand(sqlquery, con);

						// Declare Parameters
						cmd.Parameters.Add(new SqlParameter("@custID", SqlDbType.Int));
						cmd.Parameters["@custID"].Value = CustomerID;	

						try
						{
							con.Open();
							GroupID = Convert.ToInt32(cmd.ExecuteScalar());
							con.Close();
						}
						catch{}
					}

					WizardSession.GroupID = GroupID;
					WizardSession.SendSingle=true;
					WizardSession.FName = txtfirstName.Text;
					WizardSession.LName = txtLastName.Text;
					WizardSession.Email = txtEmailaddress.Text;
					
				}
			}
			return true;
		
		}

		private void ResetSession()
		{
			WizardSession.SendSingle=false;
			WizardSession.GroupID = -1;
			WizardSession.FName = "";
			WizardSession.LName = "";
			WizardSession.Email = "";
		}

		public void Initialize() 
		{
			lblError.Text = "";
			lblError.Visible = false;
			rbGroup.Attributes.Add("ONCLICK","javascript:rbselected('G')");
			rbSingle.Attributes.Add("ONCLICK","javascript:rbselected('S')");
			LoadEmailList();
			LoadTemplates();
			ShowValues();

		}

		private void LoadEmailList() 
		{
			string sql = "SELECT GroupID, GroupName FROM Groups WHERE MasterSupression = 0 and CustomerID="+CustomerID;
			DataTable dt  = DataFunctions.GetDataTable(sql, ConfigurationManager.AppSettings["com"]);
			ddlEmailList.DataSource = dt;
			ddlEmailList.DataTextField = "GroupName";
			ddlEmailList.DataValueField = "GroupID";
			ddlEmailList.DataBind();
			ddlEmailList.Items.Insert(0, "Select a List");
			ddlEmailList.Items[0].Value = "";
		}

		private void LoadTemplates() 
		{
			string selectQuery =	
				"SELECT TemplateID, TemplateImage "+
				"FROM Templates "+
				"WHERE ActiveFlag = 'Y' and ChannelID="+ ChannelID +" AND TemplateStyleCode='wizard'";//+sc.ChannelID();

			DataTable dt = DataFunctions.GetDataTable(selectQuery);

			// Find how many table rows are required if we were to show 5 columns
			int nRows = dt.Rows.Count;
			int nColumns = 4;

			// Check if number of rows are divisible by 5, coz if they are then we get exact number of rows required. 
			// Otherwise we get the number with one row less, so we gotta add one more row
			nRows = ((nRows%nColumns)==0) ? (nRows/nColumns) : ( Convert.ToInt32(nRows/nColumns) + 1);
			
			// Create n number of rows and add individual cell to it.
			for (int i=0; i<nRows; i++) 
			{
				HtmlTableRow htr = new HtmlTableRow();
				htr.Attributes.Add("runat","server");	
				// Create n number of columns / cells
				for (int j=0; j<nColumns; j++) 
				{
					try 
					{
						HtmlTableCell htc = new HtmlTableCell();
						htc.Attributes.Add("runat","server");
						htc.Attributes.Add("style", "padding-bottom: 10px;");

						// Add RadioButton Control to it.
						HtmlInputRadioButton hirb = new HtmlInputRadioButton();

						// Set Name property
						hirb.Name = "rblTemplates";
						hirb.Attributes.Add("runat", "server");

						// Set the value to TemplateID
						hirb.Value = dt.Rows[(i*nColumns)+j]["TemplateID"].ToString();

						// Set ID property
						hirb.ID = "rblTemplates"+(index++);																					

						// check if Template has already been selected when user visited last time...
						HtmlAnchor ha = new HtmlAnchor();
						HtmlImage hi = new HtmlImage();
						ha.Attributes.Add("target","_blank");
						
						hi.Attributes.Add("name",hirb.Value);

						// Set src property
						hi.Attributes.Add("src",dt.Rows[(i*nColumns)+j]["TemplateImage"].ToString());			
						
						// Set style cusrsor:hand
						hi.Style["cursor"] = "hand";
																								
						// Set Image border width to 0
						hi.Attributes.Add("border","0");																						
						// Set OnClick event of image to open a new window with a preview of template image
						hi.Attributes.Add("onClick","javascript:window.open('" + dt.Rows[(i*nColumns)+j]["TemplateImage"].ToString().Replace("thumb","preview") +"','Preview','heigth=800,width=800,location=no,menubar=no,scrollbars=no,status=no,toolbar=no')");
						ha.Controls.Add(hi);

						//Add the RadioButton and Image to Cell
						htc.Attributes.Add("align","center");
						htc.Attributes.Add("valign","top");
						htc.Controls.Add(hirb);
						htc.Controls.Add(new LiteralControl("<BR>"));
						htc.Controls.Add(ha);
						if (i < nRows-1) 
						{
							htc.Controls.Add(new LiteralControl("<BR>"));
							htc.Controls.Add(new LiteralControl("<BR>"));
						}

						// Add this cell to the row
						htr.Cells.Add(htc);
					} 
					catch
					{
//						lblError.Text = ex.Message;
//						lblError.Visible = true;
						break;
					}
				}

				// Add this row to the table
				templatesTable.Rows.Add(htr);
			}
		}

		private void ShowValues () 
		{
			try 
			{
				// Get the selected TemplateID
				for (int i=0; i<index; i++) 
				{
					HtmlInputRadioButton hirb = (HtmlInputRadioButton) templatesTable.FindControl("rblTemplates"+i);
					if (Convert.ToInt32(hirb.Value) == WizardSession.TemplateID) 
					{
						hirb.Checked = true;
						break;
					}
				}

				if (Convert.ToInt32(WizardSession.GroupID.ToString()) > 0 && !WizardSession.SendSingle)
				{
					rbGroup.Checked=true;
					txtfirstName.Enabled = false;
					txtLastName.Enabled=false;
					txtEmailaddress.Enabled=false;

					ddlEmailList.Enabled=true;
					ddlEmailList.Items.FindByValue(WizardSession.GroupID.ToString()).Selected = true;
				}
				else if(WizardSession.SendSingle)
				{
					rbSingle.Checked=true;
					ddlEmailList.Enabled=false;

					txtfirstName.Enabled = true;
					txtLastName.Enabled=true;
					txtEmailaddress.Enabled=true;

					txtfirstName.Text = WizardSession.FName;
					txtLastName.Text = WizardSession.LName;
					txtEmailaddress.Text = WizardSession.Email;
				} 
				else
				{
					rbGroup.Checked=true;
					txtfirstName.Enabled = false;
					txtLastName.Enabled=false;
					txtEmailaddress.Enabled=false;

					ddlEmailList.Enabled=true;				
				}

			}
			catch {}
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

	}
}
