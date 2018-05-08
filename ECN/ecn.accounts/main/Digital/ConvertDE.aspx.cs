using System;
using System.Configuration;
using System.Data;
using System.IO;
using ecn.accounts.classes;
using ecn.publisher.classes;
using ecn.publisher.helpers;
using pdftron;
using pdftron.PDF;

namespace ecn.accounts.main.Digital
{

    public partial class ConvertDE : EditionBase
	{
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
                " SELECT  b.basechannelID, p.customerID, e.EditionName, b.basechannelName, c.CustomerName, p.PublicationID, p.PublicationName, e.FileName,convert(varchar(10), e.enableDate,101) as EnableDate, convert(varchar(10), e.DisableDate,101) as DisableDate, Isnull(e.editionType,'') as editionType, e.FileName, e.Pages, e.status, Isnull(e.IsSearchable,0) as IsSearchable, Isnull(e.IsLoginRequired,0) as IsLoginRequired " +
				" FROM Editions e join Publications p on p.PublicationID = e.PublicationID join ecn5_accounts..customer c on p.customerID = c.customerID join ecn5_accounts..basechannel b on c.basechannelID = b.basechannelID"+
				" WHERE EditionID=" + getEditionID() ;

			DataRow dr = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sqlQuery,ConfigurationManager.AppSettings["pub"].ToString()).Rows[0];

			txtEditionName.Text=dr["EditionName"].ToString();
			txtActivationDate.Text =dr["EnableDate"].ToString();
			txtDeActivationDate.Text =dr["DisableDate"].ToString();
			lblPublication.Text =dr["PublicationName"].ToString();
			lblChannel.Text =dr["basechannelName"].ToString();
			lblCustomer.Text =dr["CustomerName"].ToString();

			hlPDFlink.Target="blank";
            hlPDFlink.NavigateUrl = System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"] + "/customers/" + dr["customerID"].ToString() + "/Publisher/" + getEditionID() + "/" + dr["FileName"].ToString();
			hlPDFlink.Text = "Download";
			lblFileName.Text = dr["FileName"].ToString();

			drpStatus.ClearSelection();
			drpStatus.Items.FindByValue("InActive").Selected = true;
			drpEditionType.ClearSelection();
			drpEditionType.Items.FindByValue(dr["editionType"].ToString()).Selected = true;
		
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

		protected void SaveButton_Click(object sender, System.EventArgs e)
		{
			string pdffilename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName).Replace(" ","_");
			try
			{
                String sqlQuery =
                " SELECT  c.basechannelID, c.customerID " +
                " FROM Editions e join Publications p on p.PublicationID = e.PublicationID join ecn5_accounts..customer c on p.customerID = c.customerID" +
                " WHERE EditionID=" + getEditionID();

                DataRow dr = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sqlQuery, ConfigurationManager.AppSettings["pub"].ToString()).Rows[0];

                var virtualImagePath = string.Format(
                    "{0}/customers/{1}/Publisher/{2}/",
                    ConfigurationManager.AppSettings["Images_VirtualPath"],
                    dr["customerID"].ToString(),
                    getEditionID());
                _imagePath = Server.MapPath(virtualImagePath);

                objEdition = Edition.GetEditionbyID(getEditionID());

				if (pdffilename != string.Empty)
				{
                    if (!Directory.Exists(_imagePath))
                    {
                        Directory.CreateDirectory(_imagePath);
                    }

					FileUpload1.PostedFile.SaveAs(_imagePath + pdffilename);
				}
				else
				{
					pdffilename	= lblFileName.Text; 				
				}
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

				objEdition.EditionName = txtEditionName.Text;
				objEdition.Status = drpStatus.SelectedItem.Value;
				objEdition.ActivationDate = txtActivationDate.Text;
				objEdition.DeActivationDate = txtDeActivationDate.Text;
				objEdition.FileName = pdffilename;

				objEdition.Save();

				foreach (ecn.publisher.classes.Page pg in objEdition.PageCollection)
				{
					pg.EditionID = objEdition.ID;
					pg.Save();
				}

				NotifyBillingDept.EmailSubject = "Digital Edition";

				string EmailBody = "<table border='1' cellpadding='3' cellspacing='0'><tr><td>Channel :</td><td>" + lblChannel.Text + "</td></tr>";
				EmailBody += "<tr><td>Customer :</td><td>" + lblCustomer.Text + "</td></tr>";
				EmailBody += "<tr><td>Publication :</td><td>" + lblPublication.Text + "<BR></td></tr>";
				EmailBody += "<tr><td>Edition Name :</td><td>" + objEdition.EditionName + "</td></tr>";
				EmailBody += "<tr><td>Total Pages :</td><td>" + objEdition.PageCollection.Count + "</td></tr>";
				EmailBody += "<tr><td>Status :</td><td>" + objEdition.Status + "</td></tr>";
				EmailBody += "<tr><td></td><td></td></tr>";
				EmailBody += "</table>";

				NotifyBillingDept.EmailBody = EmailBody;
				NotifyBillingDept.notifyBillingDept();

				//Send Notification to Billing Deparment
				Response.Redirect("default.aspx");
				
			}
			catch(Exception ex)
			{
				lblErrorMessage.Text = ex.Message;
				lblErrorMessage.Visible=true;
			}
		}
	}
}
