using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using ecn.common.classes;
using ecn.publisher.classes;
using ecn.publisher.helpers;
using pdftron;
using pdftron.PDF;

namespace ecn.accounts.main.Digital
{
    public partial class EditEdition : EditionBase
	{
		private int getChannelID() 
		{
			try 
			{
				return Convert.ToInt32(DataFunctions.ExecuteScalar("publisher", "select BasechannelID from Publications m join editions e on m.PublicationID = e.PublicationID join ecn5_accounts..customer c on c.customerID = m.customerID where e.editionID = " + getEditionID()));
				}
			catch
			{
				return 0;
			}
		}

		private int getCustomerID() 
		{
			try 
			{
				return Convert.ToInt32(DataFunctions.ExecuteScalar("publisher", "select customerID from Publications m join editions e on m.PublicationID = e.PublicationID where e.editionID = " + getEditionID()));
			}
			catch
			{
				return 0;
			}
		}


		private int getEditionID() 
		{
			try 
			{
				return Convert.ToInt32(Request.QueryString["EditionID"].ToString());
			}
			catch
			{
				return 0;
			}
		}


		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.DIGITALEDITION; 
            
            lblErrorMessage.Visible=false;
			
			if(KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
			{
				if (!IsPostBack)
				{
					if (getEditionID() > 0 ) //Update
					{
						DataTable dt = Publications.getPublications(getCustomerID());
						drpPublicationList.DataSource = dt;
						drpPublicationList.DataBind();
						drpPublicationList.Items.Insert(0, new ListItem("----- Select Publication -----",""));

						drpPublicationList.ClearSelection();

						LoadFormData();
					}
					else
					{
						Response.Redirect("default.aspx");	
					}
				}
			}
			else 
			{
				Response.Redirect("default.aspx");				
			}
		}


		private void LoadFormData() 
		{
			String sqlQuery=
				" SELECT e.EditionName, m.PublicationID, convert(varchar(10), e.enableDate,101) as EnableDate, convert(varchar(10), e.DisableDate,101) as DisableDate, Isnull(e.editionType,'') as editionType, e.FileName, e.Pages, e.status, Isnull(e.IsSearchable,0) as IsSearchable, Isnull(e.IsLoginRequired,0) as IsLoginRequired "+
				" FROM Editions e join Publications m on m.PublicationID = e.PublicationID "+
				" WHERE EditionID=" + getEditionID() ;

			DataRow dr = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sqlQuery,ConfigurationManager.AppSettings["pub"].ToString()).Rows[0];

			txtEditionName.Text=dr["EditionName"].ToString();
			txtActivationDate.Text =dr["EnableDate"].ToString();
			txtDeActivationDate.Text =dr["DisableDate"].ToString();
			drpPublicationList.ClearSelection();
			drpPublicationList.Items.FindByValue(dr["PublicationID"].ToString()).Selected = true;
			drpEditionType.ClearSelection();
			drpEditionType.Items.FindByValue(dr["EditionType"].ToString()).Selected = true;
			
			lblFileName.Text = dr["FileName"].ToString();
			lblTotalPages.Text = dr["Pages"].ToString();

			drpStatus.ClearSelection();
			drpStatus.Items.FindByValue(dr["Status"].ToString()).Selected = true;
			//chkSearch.Checked = Convert.ToBoolean(dr["IsSearchable"].ToString());
			//chklogin.Checked = Convert.ToBoolean(dr["IsLoginRequired"].ToString());

			DataTable dtVersions = ECN_Framework_DataLayer.DataFunctions.GetDataTable("select EditionID, filename, pages, version, CreateDate from editions where originaleditionID = (select originaleditionID from editions where editionID = " + getEditionID()  + ") and editionID <> " + getEditionID()  + " order by version",ConfigurationManager.AppSettings["pub"].ToString());

			if (dtVersions.Rows.Count > 0)
			{
				dgVersions.DataSource = dtVersions;
				dgVersions.DataBind();
			}
			else
			{
				dpnlHistory.Visible =false;
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
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void Save(object sender, System.EventArgs e)
		{
			try
			{
				objEdition = new ecn.publisher.classes.Edition();

				objEdition.ID = getEditionID();
				objEdition.EditionName = txtEditionName.Text;
				objEdition.PublicationID = Convert.ToInt32(drpPublicationList.SelectedValue);
				objEdition.Status = drpStatus.SelectedValue;
				objEdition.ActivationDate = txtActivationDate.Text;
				objEdition.DeActivationDate = txtDeActivationDate.Text;
				objEdition.IsSearchEnabled = false; //chkSearch.Checked;
				objEdition.IsLoginRequired = false; //chklogin.Checked;

				if (!objEdition.Exists(txtEditionName.Text))
				{
					objEdition.Save();
					Response.Redirect("default.aspx");

				}
				else
				{
					lblErrorMessage.Text = "Edition Already exists for this Publication";
					lblErrorMessage.Visible=true;
				}
			}
			catch(Exception ex)
			{
				lblErrorMessage.Text = ex.Message;
				lblErrorMessage.Visible=true;
			}
		}

		protected void btnReUpload_Click(object sender, System.EventArgs e)
		{
			string pdffilename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName).Replace(" ","_");

            var guid = Guid.NewGuid().ToString();
            var virtualImagePath = string.Format(
                "{0}/customers/{1}/Publisher/{2}/",
                ConfigurationManager.AppSettings["Images_VirtualPath"],
                getCustomerID(),
                guid);
            _imagePath = Server.MapPath(virtualImagePath);

            try
			{
				objEdition = new ecn.publisher.classes.Edition();

				objEdition.ID = getEditionID();

				if (pdffilename != string.Empty)
				{
                    // Upload file to Customer Folder
                    if (!Directory.Exists(_imagePath))
                    {
                        Directory.CreateDirectory(_imagePath);
                    }

					FileUpload1.PostedFile.SaveAs(Path.Combine(_imagePath, pdffilename));

					// Iterate PDF file to get the attributes

                    PDFNet.Initialize(ConfigurationManager.AppSettings["PDFTron_LicenseKey"].ToString());

					PDFNet.SetResourcesPath(Server.MapPath("../../resources/"));

					var pdfdoc = new PDFDoc(_imagePath + pdffilename);
                    try
                    {
                        pdfdoc.InitSecurityHandler();

                        objEdition.FileName = pdffilename;
                        objEdition.TableofContents = pdfdoc.GetFirstBookmark().ToTableOfContents();

						GetPageDetails(pdfdoc);
						GenerateImages(pdfdoc);
					}
					catch 
					{
						throw;
					}
					finally
					{
						pdfdoc.Close();
                        pdfdoc.Dispose();
					}

					objEdition.ReUpload();

					foreach (ecn.publisher.classes.Page pg in objEdition.PageCollection)
					{
						pg.EditionID = objEdition.ID;
						pg.Save();
					}
					Directory.Move(_imagePath, _imagePath.Replace(guid, objEdition.ID.ToString()));
				}
				Response.Redirect("default.aspx");

			}
			catch(Exception ex)
			{
				lblErrorMessage.Text = ex.Message;
				lblErrorMessage.Visible=true;
			}
		}

		public void dgVersions_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			ecn.publisher.classes.Edition objEdition = new ecn.publisher.classes.Edition();
			objEdition.Delete(Convert.ToInt32(dgVersions.DataKeys[e.Item.ItemIndex]));
			LoadFormData();
		}

		public void dgVersions_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			ImageButton btnDelete = new ImageButton();
			if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
			{
				btnDelete = (ImageButton) e.Item.FindControl("btnDelete");
				btnDelete.Attributes.Add("onclick", "return confirm('Are You Sure You want to delete this version?');");
			}
		}
	}
}