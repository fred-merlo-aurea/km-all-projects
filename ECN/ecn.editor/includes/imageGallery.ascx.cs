using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Helpers;
using ECN_Framework_Common.Objects;

namespace ecn.editor.includes
{
    /// <summary>
    /// Summary description for imageGallery.
    /// </summary>
    public partial class imageGallery : BaseImageGallery
	{
        protected System.Web.UI.WebControls.Label imageList;
        protected System.Web.UI.HtmlControls.HtmlTable imageTable;

        protected System.Web.UI.WebControls.Panel PanelFolders;


        string customerID = "", channelID = "";


        ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

        public string getCustomerID()
        {
            try
            {
                if (Convert.ToInt32(sc.CustomerID()) > 0)
                    return sc.CustomerID().ToString();
                else
                    return Request.QueryString["cuID"].ToString();
            }
            catch
            {
                return Request.QueryString["cuID"].ToString();
            }
        }

        public string getChannelID()
        {
            try
            {
                if (Convert.ToInt32(sc.BasechannelID()) > 0)
                    return sc.BasechannelID().ToString();
                else
                    return Request.QueryString["chID"].ToString();
            }
            catch
            {
                return Request.QueryString["chID"].ToString();
            }
        }

        #region properties
        public string imageDirectory {
			set {
				imagepath.Text = value;
			}
			get {
				return imagepath.Text;
			}
		}

		public string thumbnailSize {
			set {
				imagesize.Text = value;
			}
			get {
				return imagesize.Text;
			}
		}

        protected override ActiveUp.WebControls.PagerBuilder ImageListRepeaterPagerControl { get { return ImageListRepeaterPager; } }

        protected override DataGrid ImageListGridControl { get { return ImageListGrid; } }

        protected override RadioButtonList ImgListViewRBControl { get { return ImgListViewRB; } }

        protected override DropDownList ImagesToShowDRControl { get { return ImagesToShowDR; } }

        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {

			customerID	= getCustomerID();
			channelID		= getChannelID();

			System.Web.UI.Control currentParentControl = this.Parent;
			CurrentParent =  currentParentControl.ID;

			if(CurrentParent.Equals("ImageManagerForm")){
				PanelTabs.Visible = true;
				PanelTabsBottom.Visible = true;
			}else {
				PanelTabs.Visible = false;
				PanelTabsBottom.Visible = false;
			}

			if(!(Page.IsPostBack)){
				ViewState["SortField"] = "ImageName";
				ViewState["SortDirection"] = "ASC";

				loadFoldersTable();
				loadImagesTable();
			}
		}

	    public override void loadImagesTable()
	    {
	        var currentImageDirectory = Server.MapPath(
	            $"{ConfigurationManager.AppSettings[ImagesVirtualPathConfig]}{imageDirectory}{CurrentFolder}");

	        var currentFolderFiles = System.IO.Directory.GetFiles(currentImageDirectory, "*.*")
	            .Where(fileName => fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
	                               || fileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)
	                               || fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
	            .ToArray();

	        var totalImgFiles = currentFolderFiles.Length;

	        if (ImgListViewRB.SelectedValue.Equals("DETAILS", StringComparison.OrdinalIgnoreCase))
            {
                DisplayImagesGrid(currentFolderFiles);
            }
            else
            {
                DisplayImagesList(currentFolderFiles, totalImgFiles);
            }
        }

        private void DisplayImagesGrid(string[] currentFolderFiles)
        {
            ImageListGridPanel.Visible = true;
            ImageListRepeaterPanel.Visible = false;

            var imagesTable = ImageGalleryHelper.GetImagesTable(getCustomerID(), CurrentFolder, currentFolderFiles, imageDirectory);
            var imagesDataView = new DataView(imagesTable);
            imagesDataView.Sort = $"{ViewState[ViewStateSortField]} {ViewState[ViewStateSortDirection]}";
            ImageListGrid.DataSource = imagesDataView;
            try
            {
                ImageListGrid.DataBind();
            }
            catch
            {
                ImageListGrid.CurrentPageIndex = 0;
                ImageListGrid.DataBind();
            }
        }

        private void DisplayImagesList(string[] currentFolderFiles, int totalImgFiles)
        {
            ImageListGridPanel.Visible = false;
            ImageListRepeaterPanel.Visible = true;

            PagerPageSize = Convert.ToInt32(ImagesToShowDR.SelectedValue.ToString());
            ImageListRepeaterPager.PageSize = PagerPageSize;
            if (PagerCurrentPage > 1)
            {
                PagerCurrentPage = ImageListRepeaterPager.CurrentPage;
            }
            else
            {
                PagerCurrentPage = 1;
            }

            var picturesTable = ImageGalleryHelper.GetPicturesTable(getCustomerID(), CurrentFolder, currentFolderFiles, thumbnailSize, imageDirectory);
            var picturesDataView = new DataView(picturesTable);
            ImageListRepeater.DataSource = picturesDataView;
            ImageListRepeater.DataBind();
            ImageListRepeaterPager.RecordCount = totalImgFiles;
        }
        
		public void loadFoldersTable(){
			string currentImageDirectory = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + imageDirectory);

			System.IO.DirectoryInfo dir = null;
			string[] dirs			= null;
			string foldersCode		= "";
			string dirname		= "";	
			string selectedStyle = "class='gridaltrowWizard'";
			string normalStyle = "class='tableContent'";

            if (!System.IO.Directory.Exists(currentImageDirectory))
            {
               System.IO.Directory.CreateDirectory(currentImageDirectory);
            }

			dirs = System.IO.Directory.GetDirectories(currentImageDirectory);
			dir = new System.IO.DirectoryInfo(currentImageDirectory);	
 
			foldersCode += "<tr "+((CurrentFolder.Length > 0)?normalStyle:selectedStyle)+"><td colspan=3 align='left'><table border=0><tr><td><img src="+((CurrentFolder.Length > 0)?"'/ecn.images/icons/folder_img_25_closed.gif'":"'/ecn.images/icons/folder_img_25_open.gif'")+"></td><td align='left' valign=middle>&nbsp;&nbsp;<a href='filemanager.aspx?chID=" + channelID + "&cuID=" + customerID + "' >ROOT</a></td></tr></table></td></tr>";	

			for (int i=0; i<=dirs.Length-1; i++) {	
				dir = new System.IO.DirectoryInfo(dirs[i]);
				dirname = dir.Name.ToString();
				foldersCode += "<tr><td width=20></td><td width=20 style='Padding-bottom:3px' align=left class='tableContent' "+((CurrentFolder.Equals("/"+dirname))?"bgcolor='#EBEBEC'":"")+"><img src="+((CurrentFolder.Equals("/"+dirname))?"'/ecn.images/icons/folder_img_25_open.gif'":"'/ecn.images/icons/folder_img_25_closed.gif'")+"></td><td  align='left' style='Padding-left:4px;Padding-top:4px' valign=middle "+((CurrentFolder.Equals("/"+dirname))?"bgcolor='#EBEBEC'":"")+">";
				if(CurrentParent.Equals("ImageManagerForm")){
					foldersCode += "<a href='filemanager.aspx?chID=" + channelID + "&cuID=" + customerID + "&folder=/"+dirname+" '>"+ dirname+"</a></td></tr>";
				}else{
					foldersCode += "<a href='browse.aspx?chID="+channelID+"&cuID="+customerID+"&folder=/"+dirname+" '>"+ dirname+"</a></td></tr>";
				}
			}

			FolderSrc.Text = foldersCode;
		}

		public void loadFolderImages(object sender, CommandEventArgs e){
			loadFoldersTable();
		}

		public void showBrowse(object sender, System.EventArgs e){
			tabUpload.Visible		= true;
			tabUploadBottom.Visible		= true;
			tabBrowse.Visible	= true;
			tabBrowseBottom.Visible	= true;
			PanelBrowse.Visible		= true;
			PanelUpload.Visible		= false;
		}

		public void showUpload(object sender, System.EventArgs e){
            pageuploader.uploadDirectory = imageDirectory; // Server.MapPath(imageDirectory);

			tabUpload.Visible		= true;
			tabUploadBottom.Visible		= true;
			tabBrowse.Visible	= true;
			tabBrowseBottom.Visible	= true;
			PanelBrowse.Visible		= false;
			PanelUpload.Visible		= true;
		}

		public void deleteSelectedImages(object sender, System.EventArgs e){
			string currentImageDirectory = imageDirectory + CurrentFolder;
			System.IO.FileInfo file = null;
			ArrayList filesToDelete = new ArrayList();
			CheckBox chkBox = null;

			if(ImageListGrid.Visible){
				DataGridItem dgItem = null;
				string fileNameToDelete = "";

				for (int i=0; i < ImageListGrid.Items.Count; i++) {
					dgItem = ImageListGrid.Items[i];
					chkBox = (CheckBox)dgItem.FindControl("DGChkBxImages");
					if ((chkBox != null) && chkBox.Checked) {
						fileNameToDelete = dgItem.Cells[0].Text;
						filesToDelete.Add(fileNameToDelete);
					}
				}
			}else if(ImageListRepeater.Visible){
				string fileNameToDelete = "";

				foreach (DataListItem dlItem in ImageListRepeater.Items) {
					if (((CheckBox) dlItem.FindControl("DLChkBxImages")).Checked) {
						fileNameToDelete = ((TextBox) dlItem.FindControl("ImageKeyLbl")).Text.ToString();
						filesToDelete.Add(fileNameToDelete);
					}
				}	
				PagerPageSize = ImageListRepeaterPager.PageSize;
				PagerCurrentPage = ImageListRepeaterPager.CurrentPage;
			}

			for(int i=0; i<filesToDelete.Count; i++){
				string filewithPath = currentImageDirectory+"/"+filesToDelete[i];
				file = new System.IO.FileInfo(Server.MapPath(filewithPath));
				file.Delete();
			}
			loadImagesTable();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent() {
			this.ImageListGrid.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.ImageListGrid_PageIndexChanged);
			this.ImageListGrid.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.ImageListGrid_ItemDataBound);

		}
		#endregion
	}
}