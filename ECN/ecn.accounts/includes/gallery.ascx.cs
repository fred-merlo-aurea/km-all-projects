using System;
using System.Data;
using System.Web.UI.WebControls;
using ecn.common.classes;
using WebControlsImage = System.Web.UI.WebControls.Image;

namespace ecn.accounts.includes
{
	///		Summary description for gallery.
	
    public partial class gallery : UserControlEx
    {
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

        protected void Page_Load(object sender, System.EventArgs e)
        {
            pageuploader.uploadDirectory = Server.MapPath(imageDirectory);
			loadBrowseTable();
		}

        public void loadBrowseTable()
        {
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            DataTable dt = ECN_Framework_BusinessLayer.Communicator.NewFile.GetDataTable_Image(es.CurrentCustomer.CustomerID.ToString(), imageDirectory, Page, thumbnailSize);
            DataView dvPictures = new DataView(dt);
            imagerepeater.DataSource = dvPictures;
            imagerepeater.DataBind();
        }

        public void previewImage(object sender, CommandEventArgs e)
        {
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            string previewfile = e.CommandArgument.ToString();
            ImagePreview.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["Accounts_VirtualPath"] + "/includes/thumbnail.aspx?size=300&image=" + Server.UrlPathEncode(previewfile);
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
			loadBrowseTable();
            showBrowse(sender, e);
		}

        public void showBrowse(object sender, System.EventArgs e)
        {
            SetControlsVisibility(false, false, true);
        }

        public void showPreview(object sender, System.EventArgs e)
        {
            SetControlsVisibility(true, false, false);
        }

        public void showUpload(object sender, System.EventArgs e)
        {
            SetControlsVisibility(false, true, false);
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
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
        private void InitializeComponent()
        {

		}
		#endregion
	}
}
