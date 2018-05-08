using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.includes;
using ECN.Communicator.Tests.Setup;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Includes
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class ImageGalleryTestloadImagesTable
    {
        private const int KiloByte = 1024;
        private const int AnyNumber = 123456;
        private const string AnyString = "{B4A286B6-B091-49A8-A9AC-47A8B3A20B9D}";
        private const string AnyUrl = "http://test.com";
        private const string CommunicatorVirtualPathKey = "Communicator_VirtualPath";
        private const string CommunicatorVirtualPath = "communicator virtual path";
        private const string ImageDomainPath = "Image_DomainPath";
        private const string DetailsOption = "DETAILS";
        private const string ThumbnailSize = "12345";
        private static readonly DateTime _lastWriteTime = new DateTime(2022, 6, 23);
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
        private int _customerId;
        private User _currentUser;
        private Customer _currentCustomer;
        private readonly Random _random = new Random();

        [SetUp]
        public void Setup()
        {
            _gallery = new imageGallery();
            _galleryPrivate = new PrivateObject(_gallery);
            _shimsContext = ShimsContext.Create();
            _mocks = new MocksContext();
            _mocks.AppSettings.Add(CommunicatorVirtualPathKey, CommunicatorVirtualPath);
            InitializeControlChildren();
            CommonShims();

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
        [TestCaseSource(nameof(GetTestCasesForDetails))]
        public void loadImagesTable_ForDetails_ShouldSetFields(
            int customerId,
            string imagePathText,
            string fileName,
            long fileLength,
            string expectedImageName,
            string expectedImageSizeRaw,
            string expectedImageSize,
            string expectedImageType,
            DateTime expectedImageDtModified,
            string expectedImageKey,
            string expectedImagePath)
        {
            //Arrange
            _customerId = customerId;
            _currentCustomer.CustomerID = customerId;
            var files = new[] { fileName };
            _imgListViewRB.SelectedValue = DetailsOption;
            _mocks.Directory.Setup(directory => directory.GetFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(files);
            _mocks.FileInfo.FileLengths[fileName] = fileLength;
            _mocks.FileInfo.LastWriteTimes[fileName] = _lastWriteTime;
            var sortField = "ImageName";
            var sortDirection = "ASC";
            _viewState["SortField"] = sortField;
            _viewState["SortDirection"] = sortDirection;
            _imagepath.Text = imagePathText;
            //Act
            _gallery.loadImagesTable();
            //Assert
            var dataView = _imageListGrid.DataSource as DataView;
            dataView.ShouldNotBeNull();
            var table = dataView.Table;
            table.ShouldNotBeNull();
            table.Rows.Count.ShouldBe(1);
            var row = table.Rows[0];
            row.ShouldSatisfyAllConditions(
                () => row["ImageName"].ToString().ShouldContain(expectedImageName),
                () => row["ImageSizeRaw"].ToString().ShouldContain(expectedImageSizeRaw),
                () => row["ImageSize"].ToString().ShouldContain(expectedImageSize),
                () => row["ImageType"].ToString().ShouldContain(expectedImageType),
                () => row["ImageDtModified"].ShouldBe(expectedImageDtModified),
                () => row["ImageKey"].ToString().ShouldContain(expectedImageKey));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCasesForNoDetails))]
        public void loadImagesTable_ForNoneDetails_ShouldSetFields(
            int pagerCurrentPage,
            int expcectedPagerCurrentPage,
            int customerId,
            string imagePathText,
            string fileName,
            long fileLength,
            string expectedImage,
            string expectedImageName,
            string expectedImageDIV,
            string expectedImageKey,
            string expectedImagePath)
        {
            //Arrange
            _customerId = customerId;
            _currentCustomer.CustomerID = customerId;
            _gallery.PagerCurrentPage = pagerCurrentPage;
            var files = new[] { fileName };
            _mocks.Directory.Setup(directory => directory.GetFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(files);
            _mocks.FileInfo.FileLengths[fileName] = fileLength;
            _mocks.FileInfo.LastWriteTimes[fileName] = _lastWriteTime;
            _imagepath.Text = imagePathText;
            _imagesize.Text = ThumbnailSize;
            //Act
            _gallery.loadImagesTable();
            //Assert
            var dataView = _imageListRepeater.DataSource as DataView;
            dataView.ShouldNotBeNull();
            var table = dataView.Table;
            table.ShouldNotBeNull();
            table.Rows.Count.ShouldBe(1);
            var row = table.Rows[0];
            row.ShouldSatisfyAllConditions(
                () => row["Image"].ToString().ShouldContain(expectedImage),
                () => row["ImageName"].ToString().ShouldContain(expectedImageName),
                () => row["ImageDIV"].ToString().ShouldContain(expectedImageDIV),
                () => row["ImageKey"].ToString().ShouldContain(expectedImageKey));
        }

        private static IEnumerable<IEnumerable<object>> GetTestCasesForDetails()
        {
            var jpgFileName = $"{AnyString}.jpg";
            var gifFileName = $"{AnyString}.gif";
            var pngFileName = $"{AnyString}.png";
            var customerId = 1000;
            const string FileTypeJpg = "JPG";
            const string FileTypeGif = "GIF";
            const string FileTypePng = "PNG";
            const int FiveKiloBytes = 5 * KiloByte;
            const int Bytes = KiloByte - 1;
            var imagePathText = AnyString;
            return new object[][]
            {
                new object[]
                {
                    customerId,
                    imagePathText,
                    jpgFileName,
                    FiveKiloBytes,
                    Path.GetFileName(jpgFileName),
                    FiveKiloBytes.ToString(),
                    $"5 KB",
                    FileTypeJpg,
                    _lastWriteTime,
                    jpgFileName,
                    $"{ImageDomainPath}/Customers/{customerId}/images//{jpgFileName}"
                },
               new object[]
               {
                    customerId,
                    imagePathText,
                    gifFileName,
                    KiloByte,
                    Path.GetFileName(gifFileName),
                    KiloByte.ToString(),
                    $"1 KB",
                    FileTypeGif,
                    _lastWriteTime,
                    gifFileName,
                    $"{ImageDomainPath}/Customers/{customerId}/images//{gifFileName}"
               },
               new object[]
               {
                    customerId,
                    imagePathText,
                    pngFileName,
                    Bytes,
                    Path.GetFileName(pngFileName),
                    Bytes.ToString(),
                    $"{Bytes} Bytes",
                    FileTypePng,
                    _lastWriteTime,
                    pngFileName,
                    $"{ImageDomainPath}/Customers/{customerId}/images//{pngFileName}"
               }
            };
        }

        private static string GetMouseAltForDetails(
            string fileName,
            string fileType,
            long fileLength,
            DateTime lastWriteTime,
            string imagePathText)
        {
            var result = "<div style=\\'background-color:#FFFFFF;BORDER-TOP: #B6BCC6 1px solid;BORDER-LEFT: " +
                "#B6BCC6 1px solid;BORDER-RIGHT: #B6BCC6 1px solid;	BORDER-BOTTOM: #B6BCC6 1px solid;" +
                "position:absolute;\\'><table border=0 width=350><tr><td width=200  align=center><img src=\\'" +
                $"{CommunicatorVirtualPath}/includes/thumbnail.aspx?size=200&image=" +
                $"{imagePathText}/{fileName}\\'></td><td  width=150 class=TableContent><b>Name: {fileName}" +
                $"</b><br><br><b>TYPE:</b> {fileType}<br><b>Size:</b> {(fileLength / 1000)}kb<br><b>Date:</b> " +
                $"{lastWriteTime}<br><br><b><u>NOTE:</u></b><br>Click on the image to view its original size in " +
                "a separate browser window.</td></tr></table></div>";
            return result;
        }

        private static IEnumerable<IEnumerable<object>> GetTestCasesForNoDetails()
        {
            var jpgFileName = $"{AnyString}.jpg";
            var gifFileName = $"{AnyString}.gif";
            var pngFileName = $"short.png";
            var customerId = AnyNumber;
            Func<string, string, string> getFileNameFun = (path, mouseAlt) =>
            {
                var fileName = Path.GetFileName(path);
                return $"<DIV onclick=\"javascript:window.open('{ImageDomainPath}/Customers/" +
                $"{customerId}/images/{fileName}')\" onmouseover=\"return overlib('" + mouseAlt +
                $"', FULLHTML, VAUTO, HAUTO, RIGHT, WIDTH, 350);\" onmouseout=\"return nd();\">";
            };
            Func<string, string> getImageShortNameFunc = fileName =>
            {
                return fileName.Length > 13
                    ? $"{fileName.Substring(0, 13)}..."
                    : fileName;
            };
            const string FileTypeJpg = "JPG";
            const string FileTypeGif = "GIF";
            const string FileTypePng = "PNG";
            const int FiveKiloBytes = 5 * KiloByte;
            const int Bytes = KiloByte - 1;
            var imagePathText = AnyString;
            Func<string, string> getThumbImageFunc = fileName =>
            {
                return $"{CommunicatorVirtualPath}/includes/thumbnail.aspx?size={ThumbnailSize}&image=" +
                    $"{imagePathText}/{fileName}";
            };
            return new object[3][]
            {
                new object[11]
                {
                    0,
                    1,
                    customerId,
                    imagePathText,
                    jpgFileName,
                    FiveKiloBytes,
                    getThumbImageFunc(jpgFileName),
                    getImageShortNameFunc(jpgFileName),
                    getFileNameFun(jpgFileName, GetMouseAltForNoDetails(jpgFileName, FileTypeJpg, FiveKiloBytes,
                        imagePathText)),
                    jpgFileName,
                    $"{ImageDomainPath}/Customers/{customerId}/images/{jpgFileName}"
                },
               new object[11]
               {
                    5,
                    5,
                    customerId,
                    imagePathText,
                    gifFileName,
                    KiloByte,
                    getThumbImageFunc(gifFileName),
                    getImageShortNameFunc(gifFileName),
                    getFileNameFun(gifFileName, GetMouseAltForNoDetails(gifFileName, FileTypeGif, KiloByte,
                        imagePathText)),
                    gifFileName,
                    $"{ImageDomainPath}/Customers/{customerId}/images/{gifFileName}"
               },
               new object[11]
               {
                   1,
                   1,
                   customerId,
                   imagePathText,
                   pngFileName,
                   Bytes,
                   getThumbImageFunc(pngFileName),
                   getImageShortNameFunc(pngFileName),
                   getFileNameFun(pngFileName, GetMouseAltForNoDetails(pngFileName, FileTypePng, Bytes,
                       imagePathText)),
                   pngFileName,
                   $"{ImageDomainPath}/Customers/{customerId}/images/{pngFileName}"
               }
            };
        }

        private static string GetMouseAltForNoDetails(
            string fileName,
            string fileType,
            long fileLength,
            string imagePathText)
        {
            return "<div style=\\'background-color:#FFFFFF;BORDER-TOP: #B6BCC6 1px solid;BORDER-LEFT: " +
                "#B6BCC6 1px solid;BORDER-RIGHT: #B6BCC6 1px solid;	BORDER-BOTTOM: #B6BCC6 1px solid;position:" +
                "absolute;\\'><table border=0 cellpadding=0 cellspacing=0 width=350><tr><td width=200 align=center>" +
                $"<img src=\\'{CommunicatorVirtualPath}/includes/thumbnail.aspx?size=200&image=" +
                $"{imagePathText}/{fileName}\\'></td><td  width=150 class=TableContent><b>Name: {fileName}" +
                $"</b><br><br><b>TYPE:</b> {fileType}<br><b>Size:</b> {(fileLength / 1000)}kb<br><b>Date:</b> " +
                $"{_lastWriteTime.ToShortDateString()}<br><br><b><u>NOTE:</u></b><br>Click on the image to view its " +
                $"original size in a separate browser window.</td></tr></table></div>";
        }

        private static string GetAnyString()
        {
            return Guid.NewGuid().ToString();
        }

        private void CommonShims()
        {
            ShimECNSession.Constructor = instance => { };
            var session = GetSession();
            ShimECNSession.CurrentSession = () => session;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess =
                (user, serviceCode, featureCode, AccessCode) => true;
            KM.Platform.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess =
                (user, serviceCode, featureCode, AccessCode) => true;
        }

        private ECNSession GetSession()
        {
            var result = CreateInstance(typeof(ECNSession)) as ECNSession;
            _currentUser = new User
            {
                CustomerID = AnyNumber
            };
            result.CurrentUser = _currentUser;
            _currentCustomer = new Customer();
            result.CurrentCustomer = _currentCustomer;
            return result;
        }

        private object CreateInstance(Type type)
        {
            var privateFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            return type
                .GetConstructor(privateFlags, null, new Type[0], null)
                ?.Invoke(new object[0]);
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
            _viewState = GetReferenceProperty<StateBag>("ViewState");
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
