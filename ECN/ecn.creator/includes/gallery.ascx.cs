using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using ecn.common.classes;
using WebControlsImage = System.Web.UI.WebControls.Image;

namespace ecn.creator.includes
{
	///		Summary description for gallery.
	
    public partial class gallery : UserControlEx
    {

		ECN_Framework.Common.SecurityCheck sc = null;
		string DataFilePath = "";
		string DataWebPath = "";

		#region properties

        protected override DataList ImageRepeaterControl => imagerepeater;
        protected override WebControlsImage ImagePreviewControl => ImagePreview;
        protected override LinkButton TabPreviewControl => tabPreview;
        protected override LinkButton TabUploadControl => tabUpload;
        protected override LinkButton TabBrowseControl => tabBrowse;
        protected override Panel PanelBrowseControl => PanelBrowse;
        protected override Panel PanelPreviewControl => PanelPreview;
        protected override Panel PanelUploadControl => PanelUpload;

        public string imageDirectory
        {
            set
            {
				imagepath.Text = value;
			}
            get
            {
				return imagepath.Text;
			}
		}

        public string thumbnailSize
        {
            set
            {
				imagesize.Text = value;
			}
            get
            {
				return imagesize.Text;
			}
		}

        public string imagesPerColumn
        {
            set
            {
				imagerepeater.RepeatColumns = Convert.ToInt32(value);
			}
            get
            {
				return imagerepeater.RepeatColumns.ToString();
			}
		}

		#endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
			sc = new ECN_Framework.Common.SecurityCheck();
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            string channelID = es.CurrentBaseChannel.BaseChannelID.ToString();

            browse_img.Text = "<img src='" + System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/icons/browse_img.gif'>";
            upload_img.Text = "<img src='" + System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/icons/upload_off.gif'>";
            browse_other.Text = "<img src='" + System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/icons/browse_img.gif'>";

            DataWebPath = "/customers/" + sc.CustomerID() + "/data";
            DataFilePath = ECN_Framework_BusinessLayer.Communicator.NewFile.CreateDirectoryIfDNE(DataWebPath, ECN_Framework_Common.Objects.Enums.Entity.ImageFolder);

            imageDirectory = ConfigurationManager.AppSettings["Images_VirtualPath"].ToString() + "/customers/" + sc.CustomerID() + "/images";
           

            string deletefile = getDeleteFile();
            if (deletefile != "")
            {
                ECN_Framework_BusinessLayer.Communicator.NewFile.DeleteImage(DataFilePath + "\\" + deletefile);
                Response.Redirect(Request.Url.LocalPath + "?bIndex=Other");
            }
            else
            {
                if (!Page.IsPostBack)
                {
				loadBrowseTable();			
				loadFilesTable(DataFilePath);
			}
            }
            try
            {
                if (Request.QueryString["bIndex"] == "Other" && !Page.IsPostBack)
                {
					object oSender = " ";
					EventArgs oE = EventArgs.Empty;
//					Request.QueryString.Remove("bIndex");
					showBrowseOther(oSender, oE);
				}
			} 
            catch { }
		}

        public void loadBrowseTable()
        {
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            DataTable dt = ECN_Framework_BusinessLayer.Communicator.NewFile.GetDataTable_Image(es.CustomerID.ToString(), imageDirectory, Page ,"150");
            DataView dvPictures = new DataView(dt);
			imagerepeater.DataSource = dvPictures;
			imagerepeater.DataBind();
			imagerepeater.RepeatColumns = 4;
		}

        public void previewImage(object sender, CommandEventArgs e)
        {
            string previewfile = e.CommandArgument.ToString();
            ImagePreview.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"] + "/includes/thumbnail.aspx?size=300&image=" + Server.UrlPathEncode(previewfile);
			System.IO.FileInfo file = null;
			file = new System.IO.FileInfo(Server.MapPath(previewfile));
            previewName.Text = file.Name.ToString();
            previewSize.Text = Convert.ToInt32(file.Length / 1000).ToString() + "kb";
            previewDate.Text = file.LastWriteTime.ToShortDateString();

			System.Drawing.Image g = System.Drawing.Image.FromFile(Server.MapPath(previewfile));
            previewHeight.Text = g.Height.ToString() + " pixels ";
            previewWidth.Text = g.Width.ToString() + " pixels ";
            previewResolution.Text = g.HorizontalResolution.ToString() + " dpi";
			g.Dispose();

            deleteLink.CommandArgument = previewfile;
            fullsizeLink.NavigateUrl = previewfile;
            showPreview(sender, e);
		}

        public void deleteImage(object sender, CommandEventArgs e)
        {
            ResetImagePreview();
            ECN_Framework_BusinessLayer.Communicator.NewFile.DeleteImage(Server.MapPath(e.CommandArgument.ToString()));
            //loadBrowseTable();
            Response.Redirect("default.aspx");// showBrowse(sender, e);
		}

        public void showBrowse(object sender, System.EventArgs e)
        {
            SetControlsVisibility(false, false, true);
            PanelBrowseOther.Visible = false;
            loadBrowseTable();
        }

        public void showPreview(object sender, System.EventArgs e)
        {
            SetControlsVisibility(true, false, false);
            PanelBrowseOther.Visible = false;
		}

        public void showUpload(object sender, System.EventArgs e)
        {
            SetControlsVisibility(false, true, false);
            PanelBrowseOther.Visible = false;
            pageuploader.uploadDirectory = imageDirectory;
        }

        public void showBrowseOther(object sender, System.EventArgs e)
        {
			tabPreview.Visible = false;
			tabUpload.Visible = true;
			tabBrowse.Visible = true;

			PanelBrowse.Visible = false;
			PanelPreview.Visible = false;
			PanelUpload.Visible = false;
			PanelBrowseOther.Visible = true;
            loadFilesTable(DataFilePath);
		}

		#region DataGrid Methods

        public void Grid_Change(Object sender, DataGridPageChangedEventArgs e)
        {
			// Set CurrentPageIndex to the page the user clicked.
			FileGrid.CurrentPageIndex = e.NewPageIndex;
			// Rebind the data. 
            FileGrid.DataSource = ECN_Framework_BusinessLayer.Communicator.NewFile.GetDataTable_Files(DataFilePath, System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"].ToString() + DataWebPath);
			FileGrid.DataBind();
		}
		


        public void loadFilesTable(string datapath)
        {
	
            DataTable files = ECN_Framework_BusinessLayer.Communicator.NewFile.GetDataTable_Files(datapath, System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"].ToString() + DataWebPath);
			FileGrid.DataSource = files;
			FileGrid.DataBind();
		}

        public string getDeleteFile()
        {
			string theFile = "";
            try
            {
				theFile = Request.QueryString["deletefile"].ToString();
			}
            catch (Exception E)
            {
                string devnull = E.ToString();
			}
			return theFile;
		}

		# endregion

		#region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
        private void InitializeComponent()
        {
		}
		#endregion

        protected void BrowseImage_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            string previewfile = imgbtn.CommandArgument.ToString();
            ImagePreview.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"] + "/includes/thumbnail.aspx?size=300&image=" + Server.UrlPathEncode(previewfile);
            System.IO.FileInfo file = null;
            file = new System.IO.FileInfo(Server.MapPath(previewfile));
            previewName.Text = file.Name.ToString();
            previewSize.Text = Convert.ToInt32(file.Length / 1000).ToString() + "kb";
            previewDate.Text = file.LastWriteTime.ToShortDateString();

            System.Drawing.Image g = System.Drawing.Image.FromFile(Server.MapPath(previewfile));
            previewHeight.Text = g.Height.ToString() + " pixels ";
            previewWidth.Text = g.Width.ToString() + " pixels ";
            previewResolution.Text = g.HorizontalResolution.ToString() + " dpi";
            g.Dispose();

            deleteLink.CommandArgument = previewfile;
            fullsizeLink.NavigateUrl = previewfile;
            showPreview(sender, e);
        }

        protected void imagerepeater_ItemCommand(object source, DataListCommandEventArgs e)
        {
            string previewfile = e.CommandArgument.ToString();
            
            
            ImagePreview.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"] + "/includes/thumbnail.aspx?size=300&image=" + Server.UrlPathEncode(previewfile);
            System.IO.FileInfo file = null;
            file = new System.IO.FileInfo(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"].ToString() + previewfile));
            previewName.Text = file.Name.ToString();
            previewSize.Text = Convert.ToInt32(file.Length / 1000).ToString() + "kb";
            previewDate.Text = file.LastWriteTime.ToShortDateString();

            System.Drawing.Image g = System.Drawing.Image.FromFile(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"].ToString() + previewfile));
            previewHeight.Text = g.Height.ToString() + " pixels ";
            previewWidth.Text = g.Width.ToString() + " pixels ";
            previewResolution.Text = g.HorizontalResolution.ToString() + " dpi";
            g.Dispose();

            deleteLink.CommandArgument = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"].ToString() + previewfile;
            fullsizeLink.NavigateUrl = System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"].ToString() + previewfile;
            showPreview(source, e);
        }
	}
}
