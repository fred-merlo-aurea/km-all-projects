using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Collections;

namespace ecn.communicator.main.ECNWizard.OtherControls
{
    public partial class ImageSelector : System.Web.UI.UserControl
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

        int _imagesPerColumn;
        public int imagesPerColumn
        {
            set
            {
                _imagesPerColumn = value;
            }
            get
            {
                return _imagesPerColumn;
            }
        }

        string _currentFolder;
        public string currentFolder
        {
            set
            {
                _currentFolder = value;
            }
            get
            {
                try
                {
                    _currentFolder = Request.QueryString["folder"].ToString();
                }
                catch
                {
                    _currentFolder = "";
                }

                return _currentFolder;
            }
        }

        string _action;
        public string action
        {
            get
            {
                try
                {
                    _action = Request.QueryString["action"].ToString();
                }
                catch
                {
                    _action = "";
                }

                return _action;
            }
        }

        string _currentParent;
        public string currentParent
        {
            set
            {
                _currentParent = value;
            }
            get
            {
                return _currentParent;
            }
        }

        int _pagerCurrentPage = 1;
        public int pagerCurrentPage
        {
            set
            {
                _pagerCurrentPage = value;
            }
            get
            {
                return _pagerCurrentPage;
            }
        }

        int _pagerPageSize = 0;
        public int pagerPageSize
        {
            set
            {
                _pagerPageSize = value;
            }
            get
            {
                return _pagerPageSize;
            }
        }
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {

            customerID = getCustomerID();
            channelID = getChannelID();
            tabDeleteImages.Attributes.Add("onclick", "return ConfirmDelete();");
            tabDeleteImagesBottom.Attributes.Add("onclick", "return ConfirmDelete();");
            tabDeleteImagesPanel.Visible = false;
            tabDeleteImagesPanelBottom.Visible = false;
            System.Web.UI.Control currentParentControl = this.Parent;
            _currentParent = currentParentControl.ID;
            
            //if(currentParent.Equals("ImageManagerForm")){
            PanelTabs.Visible = true;
            PanelTabsBottom.Visible = true;
            //}else {
            //    PanelTabs.Visible = false;
            //    PanelTabsBottom.Visible = false;
            //}

            if (!(Page.IsPostBack))
            {
                ViewState["SortField"] = "ImageName";
                ViewState["SortDirection"] = "ASC";

                loadFoldersTable();
                loadImagesTable();
            }
        }

        public void loadImagesTable()
        {

            string currentImageDirectory = Server.MapPath( System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + imageDirectory + currentFolder);
            DataTable DT = ECN_Framework_BusinessLayer.Communicator.NewFile.GetDataTable_Image(customerID, currentImageDirectory,currentFolder, Page,ImgListViewRB);


            if (ImgListViewRB.SelectedValue.ToString().Equals("DETAILS"))
            {
                ImageListGridPanel.Visible = true;
                ImageListRepeaterPanel.Visible = false;
                DataView ImagesDV = new DataView(DT);
                ImagesDV.Sort = ViewState["SortField"].ToString() + ' ' + ViewState["SortDirection"].ToString();
                ImageListGrid.DataSource = ImagesDV;
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
            else
            {
                pagerPageSize = Convert.ToInt32(ImagesToShowDR.SelectedValue.ToString());
                ImageListRepeaterPager.PageSize = pagerPageSize;
                if (pagerCurrentPage > 1)
                {
                    pagerCurrentPage = ImageListRepeaterPager.CurrentPage;
                }
                else
                {
                    pagerCurrentPage = 1;
                }
                ImageListGridPanel.Visible = false;
                ImageListRepeaterPanel.Visible = true;
                DataView dvPictures = new DataView(DT);
                ImageListRepeater.DataSource = dvPictures;
                ImageListRepeater.DataBind();
                ImageListRepeaterPager.RecordCount = DT.Rows.Count;
            }
        }

        public void ImageList_Sort(Object sender, DataGridSortCommandEventArgs e)
        {

            if (e.SortExpression.ToString() == ViewState["SortField"].ToString())
            {
                switch (ViewState["SortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["SortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["SortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["SortField"] = e.SortExpression;
                ViewState["SortDirection"] = "ASC";
            }
            loadImagesTable();
        }

        public void loadFoldersTable()
        {
            //IMPLEMENT IN FUNCTION
            if (string.IsNullOrEmpty(imageDirectory))
                imageDirectory = "/customers/" + getCustomerID().ToString() + "/images";
            string currentImageDirectory = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + imageDirectory;
            //
            string fileName = "socialfilemanager.aspx";
            string foldersCode = ECN_Framework_BusinessLayer.Communicator.NewFile.GetSourceForTable_ImageFolder(getCustomerID(), imageDirectory, currentFolder, fileName, Request.QueryString.ToString());
            FolderSrc.Text = foldersCode;
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
            tabDeleteImagesPanel.Visible = false;
            tabDeleteImagesPanelBottom.Visible = false;
            PanelBrowse.Visible = true;
            SocialPanelUpload.Visible = false;
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
            SocialPanelUpload.Visible = true;
        }

        public void deleteSelectedImages(object sender, System.EventArgs e)
        {
            ArrayList filesToDelete = null;
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
                pagerPageSize = ImageListRepeaterPager.PageSize;
                pagerCurrentPage = ImageListRepeaterPager.CurrentPage;
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

        private void ImageListGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("style", "cursor:hand;");
            }
        }

        protected void ImgListViewRB_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            loadImagesTable();
        }

        protected void ImageListRepeaterPager_IndexChanged(object sender, EventArgs e)
        {
            pagerPageSize = ImageListRepeaterPager.PageSize;
            pagerCurrentPage = ImageListRepeaterPager.CurrentPage;

            loadImagesTable();
        }

        protected void ImagesToShowDR_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ImgListViewRB.SelectedValue.ToString().Equals("DETAILS"))
            {
                ImageListGrid.PageSize = Convert.ToInt32(this.ImagesToShowDR.SelectedValue);
                ImageListGrid.CurrentPageIndex = 0;
            }
            else
            {
                pagerPageSize = ImageListRepeaterPager.PageSize;
                pagerCurrentPage = ImageListRepeaterPager.CurrentPage;
            }

            loadImagesTable();
        }

        private void ImageListGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            ImageListGrid.CurrentPageIndex = e.NewPageIndex;
            loadImagesTable();
        }
    }
}