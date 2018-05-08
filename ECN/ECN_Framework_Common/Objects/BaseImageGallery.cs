using System;
using System.Configuration;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using PagerBuilder = ActiveUp.WebControls.PagerBuilder;

namespace ECN_Framework_Common.Objects
{
    public class BaseImageGallery : UserControl
    {
        public static readonly string ImagesVirtualPathConfig = "Images_VirtualPath";
        public static readonly string ViewStateSortField = "SortField";
        public static readonly string ViewStateSortDirection = "SortDirection";
        public static readonly string CommunicatorVirtualPathConfig = "Communicator_VirtualPath";
        public static readonly string ImageDomainPathConfig = "Image_DomainPath";

        private const string QueryStringFolder = "folder";
        private const string QueryStringAction = "action";
        private const string ASC = "ASC";
        private const string DESC = "DESC";
        private const string DetailsOption = "DETAILS";
        private const string StyleAttribute = "style";
        private const string StyleHand = "cursor:hand;";
        private const string ParseFailedExceptionMessage = "Failed to parse ImagesToShowDR value";

        public int ImagesPerColumn { get; set; }

        private string _currentFolder;
        public string CurrentFolder
        {
            set
            {
                _currentFolder = value;
            }
            get
            {
                if (_currentFolder == null)
                {
                    try
                    {
                        _currentFolder = Request.QueryString[QueryStringFolder];
                        if(_currentFolder == null)
                        {
                            _currentFolder = string.Empty;
                        }
                    }
                    catch
                    {
                        _currentFolder = string.Empty;
                    }
                }

                return _currentFolder;
            }
        }

        private string _action;
        public string Action
        {
            get
            {
                try
                {
                    _action = Request.QueryString[QueryStringAction];
                }
                catch
                {
                    _action = string.Empty;
                }

                return _action;
            }
        }

        public string CurrentParent { get; set; }

        public int PagerCurrentPage { get; set; }

        public int PagerPageSize { get; set; }

        protected virtual PagerBuilder ImageListRepeaterPagerControl { get; }

        protected virtual DataGrid ImageListGridControl { get; }

        protected virtual RadioButtonList ImgListViewRBControl { get; }

        protected virtual DropDownList ImagesToShowDRControl { get; }
        
        public virtual void ImageList_Sort(Object sender, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression == ViewState[ViewStateSortField].ToString())
            {
                if (ViewState[ViewStateSortDirection].ToString() == ASC)
                {
                    ViewState[ViewStateSortDirection] = DESC;
                }
                else if (ViewState[ViewStateSortDirection].ToString() == DESC)
                {
                    ViewState[ViewStateSortDirection] = ASC;
                }
            }
            else
            {
                ViewState[ViewStateSortField] = e.SortExpression;
                ViewState[ViewStateSortDirection] = ASC;
            }

            loadImagesTable();
        }

        public virtual void loadImagesTable()
        {
            throw new NotImplementedException("This method should be implemented in a derived class");
        }

        public string LoadFoldersTable(string imageDirectory)
        {
            var style = CurrentFolder.Length > 0
                ? "class='tableContent'"
                : "class='gridaltrowWizard'";

            var image = (CurrentFolder.Length > 0)
                ? "'/ecn.images/icons/folder_img_25_closed.gif'"
                : "'/ecn.images/icons/folder_img_25_open.gif'";

            var foldersCode = $"<tr {style}><td colspan=3><table border=0><tr><td><img src={image}></td><td valign=middle>&nbsp;&nbsp;<a href='filemanager.aspx' >ROOT</a></td></tr></table></td></tr>";

            var currentImageDirectory = Server.MapPath(ConfigurationManager.AppSettings[ImagesVirtualPathConfig] + imageDirectory);
            var dirs = Directory.GetDirectories(currentImageDirectory);

            for (var i = 0; i < dirs.Length; i++)
            {
                var dir = new DirectoryInfo(dirs[i]);
                foldersCode = GetRecursiveImageFolders(foldersCode, dir, dir.Name, string.Empty);
            }

            return foldersCode;
        }

        protected virtual string GetRecursiveImageFolders(string foldersCode, DirectoryInfo dir, string currentdirectory, string spacer)
        {
            throw new NotImplementedException("This method should be implemented in a derived class");
        }

        protected void ImageListGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add(StyleAttribute, StyleHand);
            }
        }

        protected void ImgListViewRB_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadImagesTable();
        }

        protected void ImageListRepeaterPager_IndexChanged(object sender, EventArgs e)
        {
            PagerPageSize = ImageListRepeaterPagerControl.PageSize;
            PagerCurrentPage = ImageListRepeaterPagerControl.CurrentPage;

            loadImagesTable();
        }

        protected void ImagesToShowDR_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ImgListViewRBControl.SelectedValue.Equals(DetailsOption, StringComparison.OrdinalIgnoreCase))
            {
                int pageSize;
                if (int.TryParse(ImagesToShowDRControl.SelectedValue, out pageSize))
                {
                    ImageListGridControl.PageSize = pageSize;
                    ImageListGridControl.CurrentPageIndex = 0;
                }
                else
                {
                    throw new InvalidOperationException(ParseFailedExceptionMessage);
                }
            }
            else
            {
                PagerPageSize = ImageListRepeaterPagerControl.PageSize;
                PagerCurrentPage = ImageListRepeaterPagerControl.CurrentPage;
            }

            loadImagesTable();
        }

        protected void ImageListGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            ImageListGridControl.CurrentPageIndex = e.NewPageIndex;
            loadImagesTable();
        }
    }
}
