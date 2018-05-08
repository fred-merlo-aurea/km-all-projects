using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Fakes;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.includes;
using ecn.communicator.includes.Fakes;
using ECN.Communicator.Tests.Setup;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Includes
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class ImageGalleryTest
    {
        private const int AnyNumber = 123456;
        private const string AnyUrl = "http://test.com";
        private const string AnyString = "{B4A286B6-B091-49A8-A9AC-47A8B3A20B9D}";
        private const string ImagesVirtualPathKey = "Images_VirtualPath";
        private const string CommunicatorVirtualPathKey = "Communicator_VirtualPath";
        private const string CommunicatorVirtualPath = "communicator virtual path";
        private const string ImageDomainPathKey = "Image_DomainPath";
        private const string ImageDomainPath = "image domain path";
        private const string DetailsOption = "DETAILS";
        private const string ActionQueryString = "action";
        private const string ViewStateSortField = "SortField";
        private const string ViewStateSortDirection = "SortDirection";
        private const string ImageName = "ImageName";
        private const string Asc = "ASC";
        private const string Desc = "DESC";
        private const string ViewStateProperty = "ViewState";
        private const string PanelTabs = "PanelTabs";
        private const string PanelTabsBottom = "PanelTabsBottom";
        private const string FieldName = "FolderSrc";
        private const int One = 1;
        private const string ImageListRepeaterPager = "ImageListRepeaterPager";
        private const string PagerPageSize = "PagerPageSize";
        private const string PagerCurrentPage = "PagerCurrentPage";
        private const string ImgListViewRB = "ImgListViewRB";
        private const string ImageListGrid = "ImageListGrid";
        private const string ImagesToShowDR = "ImagesToShowDR";
        private imageGallery _gallery;
        private PrivateObject _galleryPrivate;
        private IDisposable _shimsContext;
        private MocksContext _mocks;
        private RadioButtonList _imgListViewRB;
        private Panel _imageListGridPanel;
        private Panel _imageListRepeaterPanel;
        private DataGrid _imageListGrid;
        private DataList _imageListRepeater;
        private TextBox _imagepath;
        private TextBox _imagesize;
        private DropDownList _imagesToShowDR;
        private StateBag _viewState;
        private readonly Random _random = new Random();

        [SetUp]
        public void Setup()
        {
            _gallery = new imageGallery();
            _galleryPrivate = new PrivateObject(_gallery);
            _shimsContext = ShimsContext.Create();
            _mocks = new MocksContext();
            _mocks.AppSettings.Add(ImagesVirtualPathKey, GetAnyString());
            _mocks.AppSettings.Add(CommunicatorVirtualPathKey, CommunicatorVirtualPath);
            _mocks.AppSettings.Add(ImageDomainPathKey, ImageDomainPath);
            InitializeControlChildren();

            var session = new ShimECNSession();
            ReflectionHelper.SetField(session.Instance, "CurrentUser", new KMPlatform.Entity.User());
            ShimECNSession.CurrentSession = () => session;

            var request = new HttpRequest(AnyUrl, AnyUrl, "cuID=1000&chID=1000");
            ShimUserControl.AllInstances.RequestGet = (c) => request;
            ShimHttpContext.CurrentGet = () => new HttpContext(request, new HttpResponse(TextWriter.Null));
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
            _gallery.Dispose();
        }
        
        [Test]
        [TestCase("thumbnailSize", AnyString)]
        [TestCase("imageDirectory", AnyString)]        
        [TestCase("ImagesPerColumn", AnyNumber)]
        [TestCase("CurrentFolder", AnyString)]
        [TestCase("CurrentParent", AnyString)]
        [TestCase("PagerCurrentPage", AnyNumber)]
        [TestCase("PagerPageSize", AnyNumber)]
        public void PropertySet_SetValue_GetReturnsSameValue(string propertyName, object value)
        {
            // Arrange, Act
            ReflectionHelper.SetProperty(_gallery, propertyName, value);
            var result = ReflectionHelper.GetPropertyValue(_gallery, propertyName);

            // Assert
            result.ShouldBe(value);
        }

        [Test]
        public void Action_Get_ReturnsSameValue()
        {
            // Arrange
            ShimHttpRequest.AllInstances.QueryStringGet = (r) =>
            {
                return new NameValueCollection
                {
                    { ActionQueryString, AnyString}
                };
            };

            // Act
            var result = _gallery.Action;

            // Assert
            result.ShouldBe(AnyString);
        }

        [Test]
        public void Page_Load_Default_HidePanelsAndSetDefaultSorting()
        {
            // Arrange
            ShimControl.AllInstances.ParentGet = (c) => new Control { ID = AnyString };
            ShimimageGallery.AllInstances.loadFoldersTable = (c) => { };
            ShimimageGallery.AllInstances.loadImagesTable = (c) => { };

            // Act
            ReflectionHelper.CallMethod(typeof(imageGallery), "Page_Load", new object[] { this, EventArgs.Empty }, _gallery);

            // Assert
            var viewState = (StateBag)ReflectionHelper.GetPropertyValue((UserControl)_gallery, ViewStateProperty);

            _gallery.ShouldSatisfyAllConditions(
                () => ReflectionHelper.GetValue<Panel>(_gallery, PanelTabs).Visible.ShouldBeTrue(),
                () => ReflectionHelper.GetValue<Panel>(_gallery, PanelTabsBottom).Visible.ShouldBeTrue(),
                () => viewState[ViewStateSortField].ShouldBe(ImageName),
                () => viewState[ViewStateSortDirection].ShouldBe(Asc));
        }

        [Test]
        public void ImageList_Sort_DirectionWasASC_DirectionIsDESC()
        {
            // Arrange
            ShimimageGallery.AllInstances.loadImagesTable = (c) => { };
            var args = new DataGridSortCommandEventArgs(
                this,
                new DataGridCommandEventArgs(
                    new DataGridItem(0, 0, ListItemType.Item),
                    this,
                    new CommandEventArgs(ViewStateSortField, ImageName)));

            var viewState = (StateBag)ReflectionHelper.GetPropertyValue((UserControl)_gallery, ViewStateProperty);
            viewState[ViewStateSortField] = ImageName;
            viewState[ViewStateSortDirection] = Asc;
            
            // Act
            ReflectionHelper.CallMethod(typeof(imageGallery), "ImageList_Sort", new object[] { this, args }, _gallery);

            // Assert
            _gallery.ShouldSatisfyAllConditions(
                () => viewState[ViewStateSortField].ShouldBe(ImageName),
                () => viewState[ViewStateSortDirection].ShouldBe(Desc));
        }

        [Test]
        public void ImageList_Sort_DirectionWasDESC_DirectionIsASC()
        {
            // Arrange
            ShimimageGallery.AllInstances.loadImagesTable = (c) => { };
            var args = new DataGridSortCommandEventArgs(
                this,
                new DataGridCommandEventArgs(
                    new DataGridItem(0, 0, ListItemType.Item),
                    this,
                    new CommandEventArgs(ViewStateSortField, ImageName)));

            var viewState = (StateBag)ReflectionHelper.GetPropertyValue((UserControl)_gallery, ViewStateProperty);
            viewState[ViewStateSortField] = ImageName;
            viewState[ViewStateSortDirection] = Desc;

            // Act
            ReflectionHelper.CallMethod(typeof(imageGallery), "ImageList_Sort", new object[] { this, args }, _gallery);

            // Assert
            _gallery.ShouldSatisfyAllConditions(
                () => viewState[ViewStateSortField].ShouldBe(ImageName),
                () => viewState[ViewStateSortDirection].ShouldBe(Asc));
        }

        [Test]
        public void ImageList_Sort_FieldIsNotImageName_DirectionIsASC()
        {
            // Arrange
            ShimimageGallery.AllInstances.loadImagesTable = (c) => { };
            var args = new DataGridSortCommandEventArgs(
                this,
                new DataGridCommandEventArgs(
                    new DataGridItem(0, 0, ListItemType.Item),
                    this,
                    new CommandEventArgs(ViewStateSortField, AnyString)));

            var viewState = (StateBag)ReflectionHelper.GetPropertyValue((UserControl)_gallery, ViewStateProperty);
            viewState[ViewStateSortField] = ImageName;
            viewState[ViewStateSortDirection] = Desc;

            // Act
            ReflectionHelper.CallMethod(typeof(imageGallery), "ImageList_Sort", new object[] { this, args }, _gallery);

            // Assert
            _gallery.ShouldSatisfyAllConditions(
                () => viewState[ViewStateSortField].ShouldBe(AnyString),
                () => viewState[ViewStateSortDirection].ShouldBe(Asc));
        }

        [Test]
        public void loadFoldersTable_Default_SetsFolderSrcText()
        {
            // Arrange
            ShimDirectory.GetDirectoriesString = (path) => new string[] { AnyString };
            ShimDirectoryInfo.ConstructorString = (a, b) => new ShimDirectoryInfo();
            ShimDirectoryInfo.AllInstances.NameGet = (a) => AnyString;
            ShimimageGallery.AllInstances.GetRecursiveImageFoldersStringDirectoryInfoStringString = (a, b, c, d, e) => AnyString;

            // Act
            ReflectionHelper.CallMethod(typeof(imageGallery), "loadFoldersTable", new object[0], _gallery);

            // Assert
            var folderSrc = ReflectionHelper.GetFieldValue(_gallery, FieldName) as Label;
            folderSrc.Text.ShouldBe(AnyString);
        }

        [Test]
        [TestCase(ListItemType.Item, true)]
        [TestCase(ListItemType.AlternatingItem, true)]
        [TestCase(ListItemType.Header, false)]
        public void ImageListGrid_ItemDataBound_ItemBound_AddedProperStyleAttribute(ListItemType itemType, bool shouldAddStyle)
        {
            // Arrange
            var item = new DataGridItem(0, 0, itemType);
            var args = new DataGridItemEventArgs(item);

            // Act
            ReflectionHelper.CallMethod(typeof(imageGallery), "ImageListGrid_ItemDataBound", new object[] { this, args }, _gallery);

            // Assert
            if (shouldAddStyle)
            {
                item.Attributes.Count.ShouldBe(One);
            }
            else
            {
                item.Attributes.Count.ShouldBe(0);
            }
        }

        [Test]
        public void ImgListViewRB_SelectedIndexChanged_Default_CallsLoadImagesTable()
        {
            // Arrange
            var isCalled = false;
            ShimimageGallery.AllInstances.loadImagesTable = (g) => isCalled = true;

            // Act
            ReflectionHelper.CallMethod(typeof(imageGallery), "ImgListViewRB_SelectedIndexChanged", new object[] { this, EventArgs.Empty }, _gallery);

            // Assert
            isCalled.ShouldBeTrue();
        }

        [Test]
        public void ImageListRepeaterPager_IndexChanged_Default_CallsLoadImagesTableAndSetsPagerProperties()
        {
            // Arrange
            var isCalled = false;
            ShimimageGallery.AllInstances.loadImagesTable = (g) => isCalled = true;

            var pager = ReflectionHelper.GetFieldValue(_gallery, ImageListRepeaterPager) as ActiveUp.WebControls.PagerBuilder;
            pager.PageSize = AnyNumber;
            pager.CurrentPage = One;

            // Act
            ReflectionHelper.CallMethod(typeof(imageGallery), "ImageListRepeaterPager_IndexChanged", new object[] { this, EventArgs.Empty }, _gallery);

            // Assert
            isCalled.ShouldBeTrue();
            ReflectionHelper.GetPropertyValue<int>(_gallery, PagerPageSize).ShouldBe(AnyNumber);
            ReflectionHelper.GetPropertyValue<int>(_gallery, PagerCurrentPage).ShouldBe(One);
        }

        [Test]
        public void ImagesToShowDR_SelectedIndexChanged_ImgListViewRBNoSelectedValue_CallsLoadImagesTableAndSetsPagerProperties()
        {
            // Arrange
            var isCalled = false;
            ShimimageGallery.AllInstances.loadImagesTable = (g) => isCalled = true;

            var pager = ReflectionHelper.GetFieldValue(_gallery, ImageListRepeaterPager) as ActiveUp.WebControls.PagerBuilder;
            pager.PageSize = AnyNumber;
            pager.CurrentPage = One;

            // Act
            ReflectionHelper.CallMethod(typeof(imageGallery), "ImagesToShowDR_SelectedIndexChanged", new object[] { this, EventArgs.Empty }, _gallery);

            // Assert
            isCalled.ShouldBeTrue();
            ReflectionHelper.GetPropertyValue<int>(_gallery, PagerPageSize).ShouldBe(AnyNumber);
            ReflectionHelper.GetPropertyValue<int>(_gallery, PagerCurrentPage).ShouldBe(One);
        }

        [Test]
        public void ImagesToShowDR_SelectedIndexChanged_ImgListViewRBSelectedValueIsDetails_CallsLoadImagesTableAndSetsPagerProperties()
        {
            // Arrange
            var isCalled = false;
            ShimimageGallery.AllInstances.loadImagesTable = (g) => isCalled = true;

            var pager = ReflectionHelper.GetFieldValue(_gallery, ImageListRepeaterPager) as ActiveUp.WebControls.PagerBuilder;
            pager.PageSize = AnyNumber;
            pager.CurrentPage = One;

            var imageList = ReflectionHelper.GetFieldValue(_gallery, ImgListViewRB) as RadioButtonList;
            imageList.SelectedValue = DetailsOption;

            var dataGrid = ReflectionHelper.GetFieldValue(_gallery, ImageListGrid) as DataGrid;
            var imagesToShow = ReflectionHelper.GetFieldValue(_gallery, ImagesToShowDR) as DropDownList;
            imagesToShow.Items.Add(AnyNumber.ToString());
            imagesToShow.SelectedValue = AnyNumber.ToString();

            // Act
            ReflectionHelper.CallMethod(typeof(imageGallery), "ImagesToShowDR_SelectedIndexChanged", new object[] { this, EventArgs.Empty }, _gallery);

            // Assert
            isCalled.ShouldBeTrue();
            dataGrid.CurrentPageIndex.ShouldBe(0);
            dataGrid.PageSize.ShouldBe(AnyNumber);
        }

        [Test]
        public void ImageListGrid_PageIndexChanged_Default_CallsLoadImagesTableAndSetPageIndex()
        {
            // Arrange
            var isCalled = false;
            ShimimageGallery.AllInstances.loadImagesTable = (g) => isCalled = true;

            var dataGrid = ReflectionHelper.GetFieldValue(_gallery, ImageListGrid) as DataGrid;
            var args = new DataGridPageChangedEventArgs(this, AnyNumber);

            // Act
            ReflectionHelper.CallMethod(typeof(imageGallery), "ImageListGrid_PageIndexChanged", new object[] { this, args }, _gallery);

            // Assert
            isCalled.ShouldBeTrue();
            dataGrid.CurrentPageIndex.ShouldBe(AnyNumber);
        }
        
        private static string GetAnyString()
        {
            return Guid.NewGuid().ToString();
        }

        private int GetAnyNumber()
        {
            return _random.Next(10, 100);
        }

        private void InitializeControlChildren()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var fields = _gallery.GetType()
                .GetFields(flags)
                .Where(field => field.GetValue(_gallery) == null)
                .ToList();
            foreach (var field in fields)
            {
                var value = field.FieldType
                    .GetConstructor(flags, null, new Type[0], null)
                    ?.Invoke(new object[0]);
                field.SetValue(_gallery, value);
            }
            _viewState = GetReferenceProperty<StateBag>(ViewStateProperty);
            _imgListViewRB = GetReferenceField<RadioButtonList>("ImgListViewRB");
            _imgListViewRB.Items.Add(DetailsOption);
            _imgListViewRB.Items.Add(GetAnyString());
            _imgListViewRB.SelectedIndex = 1;
            _imageListGridPanel = GetReferenceField<Panel>("ImageListGridPanel");
            _imageListRepeaterPanel = GetReferenceField<Panel>("ImageListRepeaterPanel");
            _imageListGrid = GetReferenceField<DataGrid>("ImageListGrid");
            _imageListRepeater = GetReferenceField<DataList>("ImageListRepeater");
            _imagepath = GetReferenceField<TextBox>("imagepath");
            _imagesize = GetReferenceField<TextBox>("imagesize");
            _imagesToShowDR = GetReferenceField<DropDownList>("ImagesToShowDR");
            _imagesToShowDR.Items.Add(GetAnyNumber().ToString());
            _imagesToShowDR.SelectedIndex = 0;
        }

        private T GetReferenceField<T>(string controlName) where T : class
        {
            var result = _galleryPrivate.GetField(controlName) as T;
            result.ShouldNotBeNull();
            return result;
        }

        private T GetReferenceProperty<T>(string controlName) where T : class
        {
            var result = _galleryPrivate.GetProperty(controlName) as T;
            result.ShouldNotBeNull();
            return result;
        }
    }
}
