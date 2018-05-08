using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Helpers;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.includes
{
    public partial class imageGallery : BaseImageGallery
    {
        protected System.Web.UI.WebControls.Label imageList;
        protected System.Web.UI.HtmlControls.HtmlTable imageTable;

        protected System.Web.UI.WebControls.Panel PanelFolders;

        string customerID = "", channelID = "";

        #region properties

        public string getCustomerID()
        {
            try
            {
                if (ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID > 0)
                    return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID.ToString();
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
                if (ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID > 0)
                    return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID.ToString();
                else
                    return Request.QueryString["chID"].ToString();
            }
            catch
            {
                return Request.QueryString["chID"].ToString();
            }
        }

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

        protected override ActiveUp.WebControls.PagerBuilder ImageListRepeaterPagerControl { get { return ImageListRepeaterPager; } }

        protected override DataGrid ImageListGridControl { get { return ImageListGrid; } }

        protected override RadioButtonList ImgListViewRBControl { get { return ImgListViewRB; } }

        protected override DropDownList ImagesToShowDRControl { get { return ImagesToShowDR; } }

        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            customerID = getCustomerID();
            channelID = getChannelID();
            tabDeleteImages.Attributes.Add("onclick", "return ConfirmDelete();");
            tabDeleteImagesBottom.Attributes.Add("onclick", "return ConfirmDelete();");

            System.Web.UI.Control currentParentControl = this.Parent;
            CurrentParent = currentParentControl.ID;

            PanelTabs.Visible = true;
            PanelTabsBottom.Visible = true;

            if (!(Page.IsPostBack))
            {
                ViewState["SortField"] = "ImageName";
                ViewState["SortDirection"] = "ASC";
                KMPlatform.Entity.User currentUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;

                loadFoldersTable();
                loadImagesTable();
                if(KM.Platform.User.HasAccess(currentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ManageImages, KMPlatform.Enums.Access.Edit))
                {
                    tabUploadImagesPanel.Visible = true;
                    tabDeleteImagesPanel.Visible = true;
                    pnlUploadImagesBottom.Visible = true;
                    tabDeleteImagesPanelBottom.Visible = true;
                }
                else
                {
                    tabUploadImagesPanel.Visible = false;
                    tabDeleteImagesPanel.Visible = false;
                    pnlUploadImagesBottom.Visible = false;
                    tabDeleteImagesPanelBottom.Visible = false;
                }
            }
        }

        public override void loadImagesTable()
        {
            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ManageImages, KMPlatform.Enums.Access.View))
            {
                var currentImageDirectory = Server.MapPath(
                    $"{ConfigurationManager.AppSettings[ImagesVirtualPathConfig]}{imageDirectory}{HttpUtility.UrlDecode(CurrentFolder)}");

                var currentFolderFiles = System.IO.Directory.GetFiles(currentImageDirectory, "*.*")
                    .Where(fileName => fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                        || fileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)
                        || fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                var totalImgFiles = currentFolderFiles.Length;
                
                if (ImgListViewRB.SelectedValue.Equals("DETAILS"))
                {
                    DisplayImagesGrid(currentFolderFiles);
                }
                else
                {
                    DisplayImagesList(currentFolderFiles, totalImgFiles);
                }
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

            PagerPageSize = Convert.ToInt32(ImagesToShowDR.SelectedValue);
            ImageListRepeaterPager.PageSize = PagerPageSize;
            PagerCurrentPage = PagerCurrentPage > 1 ? ImageListRepeaterPager.CurrentPage : 1;

            var picturesTable = ImageGalleryHelper.GetPicturesTable(getCustomerID(), CurrentFolder, currentFolderFiles, thumbnailSize, imageDirectory);
            
            ImageListRepeater.DataSource = new DataView(picturesTable);
            ImageListRepeater.DataBind();
            ImageListRepeaterPager.RecordCount = totalImgFiles;
        }
        
        public void loadFoldersTable()
        {
            FolderSrc.Text = LoadFoldersTable(imageDirectory);
        }

        protected override string GetRecursiveImageFolders(string foldersCode, System.IO.DirectoryInfo dir, string currentdirectory, string spacer)
        {
            string imageDirectory = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + customerID + "/images";

            string dirName = dir.Name.ToString();

            foldersCode += "<tr><td width=5%></td><td width=95%  align=left class='tableContent' " + ((CurrentFolder.Equals("/" + currentdirectory)) ? "bgcolor='#EBEBEC'" : "") + ">" + spacer + "<img src='/ecn.images/images/L.gif' style='height:auto;vertical-align: middle;'>&nbsp;<img  style='height:auto;vertical-align: middle;' src=" + ((CurrentFolder.Equals("/" + currentdirectory)) ? "'/ecn.images/icons/folder_img_25_open.gif'" : "'/ecn.images/icons/folder_img_25_closed.gif'") + ">";
            foldersCode += "<span style='Padding-left:10px;'><a href='filemanager.aspx?folder=/" + System.Web.HttpUtility.UrlEncode(currentdirectory) + " '>" + dirName + "</a></span></td></tr>";

            string[] dirs = null;


            dirs = System.IO.Directory.GetDirectories(Server.MapPath(imageDirectory + "/" + currentdirectory));

            for (int i = 0; i <= dirs.Length - 1; i++)
            {
                System.IO.DirectoryInfo subdir = new System.IO.DirectoryInfo(dirs[i]);
                foldersCode = GetRecursiveImageFolders(foldersCode, subdir, currentdirectory + "/" + subdir.Name, spacer + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            }

            return foldersCode;
        }
        
        public void loadFolderImages(object sender, CommandEventArgs e)
        {
            loadFoldersTable();
        }

        public void showBrowse(object sender, System.EventArgs e)
        {
            tabUpload.Visible = true;
            tabUploadBottom.Visible = true;
            tabBrowse.Visible = true;
            tabBrowseBottom.Visible = true;
            tabDeleteImagesPanel.Visible = true;
            tabDeleteImagesPanelBottom.Visible = true;

            PanelBrowse.Visible = true;
            PanelUpload.Visible = false;
        }

        public void showUpload(object sender, System.EventArgs e)
        {
            pageuploader.uploadDirectory = imageDirectory;

            tabUpload.Visible = true;
            tabUploadBottom.Visible = true;
            tabBrowse.Visible = true;
            tabBrowseBottom.Visible = true;
            tabDeleteImagesPanel.Visible = false;
            tabDeleteImagesPanelBottom.Visible = false;

            PanelBrowse.Visible = false;
            PanelUpload.Visible = true;
        }

        public void deleteSelectedImages(object sender, System.EventArgs e)
        {
            ArrayList filesToDelete = new ArrayList();
            System.IO.FileInfo file = null;
            System.Web.UI.WebControls.CheckBox chkBox = null;


            if (ImageListGrid.Visible)
            {
                DataGridItem dgItem = null;
                string fileNameToDelete = "";

                for (int i = 0; i < ImageListGrid.Items.Count; i++)
                {
                    dgItem = ImageListGrid.Items[i];
                    chkBox = (CheckBox)dgItem.FindControl("DGChkBxImages");
                    if ((chkBox != null) && chkBox.Checked)
                    {
                        fileNameToDelete = dgItem.Cells[0].Text;
                        filesToDelete.Add(fileNameToDelete);
                    }
                }
            }
            else if (ImageListRepeater.Visible)
            {
                string fileNameToDelete = "";

                foreach (DataListItem dlItem in ImageListRepeater.Items)
                {
                    if (((CheckBox)dlItem.FindControl("DLChkBxImages")).Checked)
                    {
                        fileNameToDelete = ((TextBox)dlItem.FindControl("ImageKeyLbl")).Text.ToString();
                        filesToDelete.Add(fileNameToDelete);
                    }
                }
            }

            ECN_Framework_BusinessLayer.Communicator.NewFile.DeleteSelectedImages(filesToDelete);
            if (ImageListRepeater.Visible)
            {
                PagerPageSize = ImageListRepeaterPager.PageSize;
                PagerCurrentPage = ImageListRepeaterPager.CurrentPage;
            }
            loadImagesTable();
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
            this.ImageListGrid.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.ImageListGrid_PageIndexChanged);
            this.ImageListGrid.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.ImageListGrid_ItemDataBound);

        }
        #endregion
    }
}