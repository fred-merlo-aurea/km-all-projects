using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web.Fakes;
using System.Web.SessionState.Fakes;
using System.Web.UI.Fakes;
using MsUnitTesting = Microsoft.VisualStudio.TestTools.UnitTesting;
using ActiveUp.WebControls.Tests.Helper;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActivePix
{
    [TestFixture()]
    public class ToolZoomTest
    {
        private const string ToolIdPrefix = "_toolZoom";
        private const int InitialImageWidth = 200;
        private const int InitialImageHeight = 100;

        private const string ToolImageName = "zoom_off.gif";
        private const string ToolOverImageName = "zoom_over.gif";
        private const string ToolImageResourceName = "ActiveUp.WebControls._resources.Images.zoom_off.gif";
        private const string ToolOverImageResourceName = "ActiveUp.WebControls._resources.Images.zoom_over.gif";

        private const string ToolImagesFolderName = "toolImages";
        private const string ToolImagesAlternativeFolderName = "toolAlternativeFolder";

        private readonly string ToolImagesDirectory = string.Format("{0}\\{1}", TestContext.CurrentContext.TestDirectory, ToolImagesFolderName);
        private readonly string ToolImagesAlternativeDirectory = string.Format("{0}\\{1}", TestContext.CurrentContext.TestDirectory, ToolImagesAlternativeFolderName);

        private IDisposable _shimsContext;

        private ShimPage _shimPage;
        private ShimHttpSessionState _shimHttpSessionState;
        private ShimHttpServerUtility _shimHttpServerUtility;
        private ShimClientScriptManager _shimClientScriptManager;

        private Dictionary<string, string> _registeredScripts;

        private FakeSessionStateCollection _sessionCollection;

        private ImageEditor _imageEditor;
        private MsUnitTesting.PrivateObject _imageEditorPrivateWrapper;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _shimsContext = ShimsContext.Create();
            SetupHttpSessionState();
            SetupHttpServerUtility();
            SetupClientScriptManager();
            SetupPage();

            if (!Directory.Exists(ToolImagesDirectory))
            {
                Directory.CreateDirectory(ToolImagesDirectory);
            }

            if (!Directory.Exists(ToolImagesAlternativeDirectory))
            {
                Directory.CreateDirectory(ToolImagesAlternativeDirectory);
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (Directory.Exists(ToolImagesDirectory))
            {
                Directory.Delete(ToolImagesDirectory, true);
            }

            if (Directory.Exists(ToolImagesAlternativeDirectory))
            {
                Directory.Delete(ToolImagesAlternativeDirectory, true);
            }

            _shimsContext.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            var imageFileName = string.Format("{0}.jpg", Guid.NewGuid());
            var imageFileServerPath = string.Format("{0}\\{1}", ToolImagesDirectory, imageFileName);
            var imageFileVirtualPath = string.Format("/{0}/{1}", ToolImagesFolderName, imageFileName);
            var image = ActivePixToolsTestHelper.CreateAndSaveDummyImage(InitialImageWidth, InitialImageHeight, imageFileServerPath);

            var alternativeFiles = Directory.GetFiles(ToolImagesAlternativeDirectory);
            foreach (var file in alternativeFiles)
            {
                System.IO.File.Delete(file);
            }

            _imageEditor = new ImageEditor();
            _imageEditor.Page = _shimPage;
            _imageEditor.ImageURL = imageFileVirtualPath;
            _imageEditor.TempURL = imageFileVirtualPath;
            _imageEditor.TempDirectory = string.Format("/{0}/", ToolImagesAlternativeFolderName);

            _imageEditorPrivateWrapper = new MsUnitTesting.PrivateObject(_imageEditor);
            _imageEditorPrivateWrapper.Invoke("OnInit", EventArgs.Empty);
        }

        [TearDown]
        public void TearDown()
        {
            _imageEditor.Dispose();
        }

        [Test]
        public void Constructor_WithoutId_CreatesObjectWithGeneratedId()
        {
            // Arrange & Act
            var toolZoom = new ToolZoom();
            var expectedToolZoomId = string.Format("{0}{1}", ToolIdPrefix, ImageEditor.indexTools - 1);

            // Assert
            toolZoom.ID.ShouldBe(expectedToolZoomId);
        }

        [Test]
        public void Constructor_WithId_CreatesObjectWithGivenId()
        {
            // Arrange
            var expectedToolZoomId = "myToolZoom";

            // Act
            var toolZoom = new ToolZoom(expectedToolZoomId);

            // Assert
            toolZoom.ID.ShouldBe(expectedToolZoomId);
        }

        [Test]
        public void OnPreRender_SetsButtonImages()
        {
            // Arrange
            var toolZoom = new ToolZoom();
            toolZoom.Page = _shimPage;
            toolZoom.ImageURL = string.Empty;
            toolZoom.OverImageURL = string.Empty;

            _imageEditor.Toolbar.Tools.Add(toolZoom);

            var toolZoomPrivateObject = new MsUnitTesting.PrivateObject(toolZoom);

            // Act
            toolZoomPrivateObject.Invoke("OnPreRender", EventArgs.Empty);

            // Assert
            TestsHelper.AssertNotFX1(ToolImageResourceName, toolZoom.ImageURL);
            TestsHelper.AssertNotFX1(ToolOverImageResourceName, toolZoom.OverImageURL);

            TestsHelper.AssertFX1(ToolImageName, toolZoom.ImageURL);
            TestsHelper.AssertFX1(ToolOverImageName, toolZoom.OverImageURL);
        }

        [Test]
        public void ParentImageEditorGetter_IfToolIsNotAddedIntoAnImageEditor_ThrowsException()
        {
            // Arrange
            var toolZoom = new ToolZoom();

            // Act
            var getParentImageEditor = new Action(() => { var returnedImageEditor = toolZoom.ParentImageEditor; });

            // Assert
            getParentImageEditor.ShouldThrow<NullReferenceException>();
        }

        [Test]
        public void ParentImageEditorGetter_IfToolIsAddedIntoAnImageEditor_ReturnsImageEditor()
        {
            // Arrange
            var toolZoom = new ToolZoom();
            _imageEditor.Toolbar.Tools.Add(toolZoom);

            // Act
            var returnedImageEditor = toolZoom.ParentImageEditor;

            // Assert
            returnedImageEditor.ShouldBeSameAs(_imageEditor);
        }

        [Test]
        public void ParentToolbarGetter_IfToolIsAddedIntoAnImageEditor_ReturnsToolbarOfImageEditor()
        {
            // Arrange
            var toolZoom = new ToolZoom();
            _imageEditor.Toolbar.Tools.Add(toolZoom);

            // Act
            var returnedToolbar = toolZoom.ParentToolbar;

            // Assert
            returnedToolbar.ShouldBeSameAs(_imageEditor.Toolbar);
        }

        [Test]
        public void ParentToolbarGetter_IfToolIsNotAddedIntoAnImageEditor_ReturnsNull()
        {
            // Arrange
            var toolZoom = new ToolZoom();

            // Act
            var returnedToolbar = toolZoom.ParentToolbar;

            // Assert
            returnedToolbar.ShouldBeNull();
        }

        [Test]
        public void RaisePostBackEvent_IfDirectWriteSetAsTrue_ZoomsAndSavesImageDirectly()
        {
            // Arrange
            var toolZoom = new ToolZoom();

            _imageEditor.Toolbar.Tools.Add(toolZoom);
            _imageEditor.DirectWrite = true;

            // RaisePostBackEvent method eventArgument parameter scheme
            // float factor

            // zoom 4 times to test if all image is gray colored
            var raisePostBackEventArgs = "200";

            // select center point to be zoomed
            _imageEditor.Selection.X1 = 151;
            _imageEditor.Selection.Y1 = 76;

            // Act
            toolZoom.RaisePostBackEvent(raisePostBackEventArgs);

            // Assert
            var imagePath = _shimHttpServerUtility.Instance.MapPath(_imageEditor.ImageURL);

            using (var image = ActivePixToolsTestHelper.LoadImage(imagePath))
            {
                var firstPixelOfImage = image.GetPixel(0, 0);

                firstPixelOfImage.A.ShouldBe(Color.Gray.A);
                firstPixelOfImage.R.ShouldBe(Color.Gray.R);
                firstPixelOfImage.G.ShouldBe(Color.Gray.G);
                firstPixelOfImage.B.ShouldBe(Color.Gray.B);
            }
        }

        [Test]
        public void RaisePostBackEvent_IfDirectWriteSetAsFalse_ZoomsAndSavesImageToAlternativePath()
        {
            // Arrange
            var toolZoom = new ToolZoom();

            _imageEditor.Toolbar.Tools.Add(toolZoom);
            _imageEditor.DirectWrite = false;

            // RaisePostBackEvent method eventArgument parameter scheme
            // float factor

            // zoom 4 times to test if all image is gray colored
            var raisePostBackEventArgs = "200";

            // select center point to be zoomed
            _imageEditor.Selection.X1 = 151;
            _imageEditor.Selection.Y1 = 76;

            // Act
            toolZoom.RaisePostBackEvent(raisePostBackEventArgs);

            // Assert
            var alternativeDirectoryFileNames = Directory.GetFiles(ToolImagesAlternativeDirectory);

            alternativeDirectoryFileNames.Length.ShouldBe(1);

            var imagePath = alternativeDirectoryFileNames[0];

            using (var image = ActivePixToolsTestHelper.LoadImage(imagePath))
            {
                var firstPixelOfImage = image.GetPixel(0, 0);

                firstPixelOfImage.A.ShouldBe(Color.Gray.A);
                firstPixelOfImage.R.ShouldBe(Color.Gray.R);
                firstPixelOfImage.G.ShouldBe(Color.Gray.G);
                firstPixelOfImage.B.ShouldBe(Color.Gray.B);
            }
        }

        [Test]
        public void RaisePostBackEvent_IfSelectionValuesAreInvalid_RegistersStartupScript()
        {
            // Arrange
            var toolZoom = new ToolZoom();

            _imageEditor.Toolbar.Tools.Add(toolZoom);
            _imageEditor.DirectWrite = true;

            var registeredStartupScriptKey = string.Empty;
            var registeredStartupScriptValue = string.Empty;

            _shimPage.RegisterStartupScriptStringString =
                (key, value) =>
                {
                    registeredStartupScriptKey = key;
                    registeredStartupScriptValue = value;
                    _registeredScripts[key] = value;
                };

            // RaisePostBackEvent method eventArgument parameter scheme
            // float factor

            // no selection to make sure exception is going to be thrown inside method
            var raisePostBackEventArgs = "100";
            _imageEditor.Selection.X1 = 0;
            _imageEditor.Selection.Y1 = 0;
            _imageEditor.Selection.X2 = 0;
            _imageEditor.Selection.Y2 = 0;

            // Act
            toolZoom.RaisePostBackEvent(raisePostBackEventArgs);

            // Assert
            _registeredScripts[registeredStartupScriptKey].ShouldBe(registeredStartupScriptValue);
        }

        private void SetupHttpSessionState()
        {
            if (_shimsContext == null)
            {
                throw new InvalidOperationException("_shimsContext local member must be initialized to setup the HttpSessionState shim object");
            }

            _sessionCollection = new FakeSessionStateCollection();

            _shimHttpSessionState = new ShimHttpSessionState();
            _shimHttpSessionState.SessionIDGet = () => Guid.NewGuid().ToString();
            _shimHttpSessionState.AddStringObject = (key, value) => _sessionCollection.Add(key, value);
            _shimHttpSessionState.RemoveAll = () => _sessionCollection.Clear();
            _shimHttpSessionState.RemoveAtInt32 = (index) => _sessionCollection.RemoveAt(index);
            _shimHttpSessionState.RemoveString = (key) => _sessionCollection.Remove(key);
            _shimHttpSessionState.ItemGetInt32 = (index) => _sessionCollection[index];
            _shimHttpSessionState.ItemSetInt32Object = (index, value) => _sessionCollection[index] = value;
            _shimHttpSessionState.ItemGetString = (key) => _sessionCollection[key];
            _shimHttpSessionState.ItemSetStringObject = (key, value) => _sessionCollection[key] = value;
        }

        private void SetupHttpServerUtility()
        {
            if (_shimsContext == null)
            {
                throw new InvalidOperationException("_shimsContext local member must be initialized to setup the HttpServerUtility shim object");
            }

            ShimHttpServerUtility.AllInstances.MapPathString = (utility, path) => $"{TestContext.CurrentContext.TestDirectory}{path?.Replace("/", "\\") ?? string.Empty}";

            _shimHttpServerUtility = new ShimHttpServerUtility();
        }

        private void SetupClientScriptManager()
        {
            if (_shimsContext == null)
            {
                throw new InvalidOperationException("_shimsContext local member must be initialized to setup the ClientScriptManager shim object");
            }

            ShimClientScriptManager.AllInstances.GetWebResourceUrlTypeString = (manager, type, url) => url;

            _shimClientScriptManager = new ShimClientScriptManager();
            _registeredScripts = new Dictionary<string, string>();
        }

        private void SetupPage()
        {
            if (_shimsContext == null)
            {
                throw new InvalidOperationException("_shimsContext local member must be initialized to setup the Page shim object");
            }

            ShimPage.AllInstances.GetPostBackClientEventControlString = (page, control, value) => string.Empty;
            ShimPage.AllInstances.RegisterStartupScriptStringString = (page, key, value) => _registeredScripts[key] = value;

            _shimPage = new ShimPage();
            _shimPage.SessionGet = () => _shimHttpSessionState;
            _shimPage.ServerGet = () => _shimHttpServerUtility;
            _shimPage.ClientScriptGet = () => _shimClientScriptManager;
        }
    }
}
