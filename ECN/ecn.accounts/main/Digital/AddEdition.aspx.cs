using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using ecn.accounts.classes;
using ecn.publisher.classes;
using ecn.publisher.helpers;
using pdftron;
using pdftron.PDF;

namespace ecn.accounts.main.Digital
{
    public partial class AddEdition : EditionBase
	{
		protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator2;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.DIGITALEDITION;
            
            lblErrorMessage.Visible=false;
			if (!IsPostBack)
			{
				if(KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
				{
					if (!IsPostBack) 
					{
						loadChannels();
					}
				}
				else 
				{
					Response.Redirect("../default.aspx");				
				}

			}
		}

		private void loadChannels()
		{
            drpChannels.DataSource = ECN_Framework_DataLayer.DataFunctions.GetDataTable("select BaseChannelID, BaseChannelName from [basechannel] where Accesspublisher = 1 and IsDeleted = 0", ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());
			drpChannels.DataBind();
			drpChannels.Items.Insert(0, new ListItem("Select Channel","0"));
		}

		private void loadCustomers()
		{
            drpCustomers.DataSource = ECN_Framework_DataLayer.DataFunctions.GetDataTable("select CustomerID, CustomerName from [customer] where basechannelID = " + drpChannels.SelectedValue + " and IsDeleted = 0 and isnull(PublisherChannelID,0) <> 0", ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());
			drpCustomers.DataBind();
			drpCustomers.Items.Insert(0, new ListItem("Select Customer","0"));
			drpCustomers.ClearSelection();

			drpCustomers.Items.FindByValue("0").Selected = true;
			loadPublications();
		}

		private void loadPublications()
		{
			DataTable dt = Publications.getPublications(Convert.ToInt32(drpCustomers.SelectedValue));
			drpPublicationList.DataSource = dt;
			drpPublicationList.DataBind();
			drpPublicationList.Items.Insert(0, new ListItem("----- Select Publication -----",""));
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

		protected void drpChannels_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			loadCustomers();
		}

		protected void drpCustomers_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			loadPublications();
		}

		protected void Save(object sender, System.EventArgs e)
		{
			string pdffilename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName).Replace(" ","_");

            var guid = Guid.NewGuid().ToString();
            var virtualImagePath = string.Format(
                "{0}/customers/{1}/Publisher/{2}/",
                ConfigurationManager.AppSettings["Images_VirtualPath"],
                drpCustomers.SelectedValue,
                guid);
            _imagePath = Server.MapPath(virtualImagePath);

            try
			{
				objEdition = new ecn.publisher.classes.Edition();

				objEdition.ID = 0;
				objEdition.EditionName = txtEditionName.Text;
				objEdition.PublicationID = Convert.ToInt32(drpPublicationList.SelectedValue);
				objEdition.Status = drpStatus.SelectedValue;
				objEdition.ActivationDate = txtActivationDate.Text;
				objEdition.DeActivationDate = txtDeActivationDate.Text;
				objEdition.IsSearchEnabled = false;
				objEdition.IsLoginRequired = false;

				if (!objEdition.Exists(txtEditionName.Text))
				{
					if (pdffilename != string.Empty)
					{

						// Upload file to Customer Folder
						if (!Directory.Exists(_imagePath))
						{
							Directory.CreateDirectory(_imagePath);
						}

						FileUpload1.PostedFile.SaveAs(_imagePath + pdffilename);

						// Iterate PDF file to get the attributes

                        PDFNet.Initialize(ConfigurationManager.AppSettings["PDFTron_LicenseKey"].ToString());

						PDFNet.SetResourcesPath(Server.MapPath("../../resources"));

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

						objEdition.Save();

						foreach (ecn.publisher.classes.Page pg in objEdition.PageCollection)
						{
							pg.EditionID = objEdition.ID;
							pg.Save();
						}

                        Directory.Move(_imagePath, _imagePath.Replace(guid, objEdition.ID.ToString()));

						NotifyBillingDept.EmailSubject = "Digital Edition";

						string EmailBody = "<table border='1' cellpadding='3' cellspacing='0'><tr><td>Channel :</td><td>" + drpChannels.SelectedItem.Text + "</td></tr>";
						EmailBody += "<tr><td>Customer :</td><td>" + drpCustomers.SelectedItem.Text + "</td></tr>";
						EmailBody += "<tr><td>Publication :</td><td>" + drpPublicationList.SelectedItem.Text + "<BR></td></tr>";
						EmailBody += "<tr><td>Edition Name :</td><td>" + objEdition.EditionName + "</td></tr>";
						EmailBody += "<tr><td>Total Pages :</td><td>" + objEdition.PageCollection.Count + "</td></tr>";
						EmailBody += "<tr><td>Status :</td><td>" + objEdition.Status + "</td></tr>";
						EmailBody += "<tr><td></td><td></td></tr>";
						EmailBody += "</table>";

                        //Send Notification to Billing Deparment

						NotifyBillingDept.EmailBody = EmailBody;
                        NotifyBillingDept.notifyBillingDept();
					}

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
	}
}