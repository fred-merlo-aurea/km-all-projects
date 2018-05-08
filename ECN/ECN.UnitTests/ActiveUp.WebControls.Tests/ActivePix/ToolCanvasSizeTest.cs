using System;
using System.Collections.Generic;
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
    public class ToolCanvasSizeTest
    {
        private const string ToolIdPrefix = "_toolCanvasSize";
        private const int InitialImageWidth = 200;
        private const int InitialImageHeight = 100;

        private const string ToolImageName = "canvassize_off.gif";
        private const string ToolOverImageName = "canvassize_over.gif";
        private const string ToolImageResourceName = "ActiveUp.WebControls._resources.Images.canvassize_off.gif";
        private const string ToolOverImageResourceName = "ActiveUp.WebControls._resources.Images.canvassize_over.gif";

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
            var toolCanvasSize = new ToolCanvasSize();
            var expectedToolCanvasSizeId = string.Format("{0}{1}", ToolIdPrefix, ImageEditor.indexTools - 1);

            // Assert
            toolCanvasSize.ID.ShouldBe(expectedToolCanvasSizeId);
        }

        [Test]
        public void Constructor_WithId_CreatesObjectWithGivenId()
        {
            // Arrange
            var expectedToolCanvasSizeId = "myToolCanvasSize";

            // Act
            var toolCanvasSize = new ToolCanvasSize(expectedToolCanvasSizeId);

            // Assert
            toolCanvasSize.ID.ShouldBe(expectedToolCanvasSizeId);
        }

        [Test]
        public void OnPreRender_SetsButtonImages()
        {
            // Arrange
            var toolCanvasSize = new ToolCanvasSize();
            toolCanvasSize.Page = _shimPage;
            toolCanvasSize.ImageURL = string.Empty;
            toolCanvasSize.OverImageURL = string.Empty;

            _imageEditor.Toolbar.Tools.Add(toolCanvasSize);

            var toolCanvasSizePrivateObject = new MsUnitTesting.PrivateObject(toolCanvasSize);

            // Act
            toolCanvasSizePrivateObject.Invoke("OnPreRender", EventArgs.Empty);

            // Assert
            TestsHelper.AssertNotFX1(ToolImageResourceName, toolCanvasSize.ImageURL);
            TestsHelper.AssertNotFX1(ToolOverImageResourceName, toolCanvasSize.OverImageURL);

            TestsHelper.AssertFX1(ToolImageName, toolCanvasSize.ImageURL);
            TestsHelper.AssertFX1(ToolOverImageName, toolCanvasSize.OverImageURL);
        }

        [Test]
        public void ParentImageEditorGetter_IfToolIsNotAddedIntoImageEditor_ThrowsException()
        {
            // Arrange
            var toolCanvasSize = new ToolCanvasSize();

            // Act
            var getParentImageEditor = new Action(() => { var returnedImageEditor = toolCanvasSize.ParentImageEditor; });

            // Assert
            getParentImageEditor.ShouldThrow<NullReferenceException>();
        }

        [Test]
        public void ParentImageEditorGetter_IfToolIsAddedIntoAnImageEditor_ReturnsImageEditor()
        {
            // Arrange
            var toolCanvasSize = new ToolCanvasSize();
            _imageEditor.Toolbar.Tools.Add(toolCanvasSize);

            // Act
            var returnedImageEditor = toolCanvasSize.ParentImageEditor;

            // Assert
            returnedImageEditor.ShouldBeSameAs(_imageEditor);
        }

        [Test]
        public void ParentToolbarGetter_IfToolIsAddedIntoAnImageEditor_ReturnsToolbarOfImageEditor()
        {
            // Arrange
            var toolCanvasSize = new ToolCanvasSize();
            _imageEditor.Toolbar.Tools.Add(toolCanvasSize);

            // Act
            var returnedImageEditor = toolCanvasSize.ParentToolbar;

            // Assert
            returnedImageEditor.ShouldBeSameAs(_imageEditor.Toolbar);
        }

        [Test]
        public void ParentToolbarGetter_IfToolCanvasSizeIsNotAddedIntoAnImageEditor_ReturnsNull()
        {
            // Arrange
            var toolCanvasSize = new ToolCanvasSize();

            // Act
            var returnedToolbar = toolCanvasSize.ParentToolbar;

            // Assert
            returnedToolbar.ShouldBeNull();
        }

        [Test]
        public void RaisePostBackEvent_IfDirectWriteSetAsTrue_SavesImageDirectly()
        {
            // Arrange
            var toolCanvasSize = new ToolCanvasSize();

            _imageEditor.Toolbar.Tools.Add(toolCanvasSize);
            _imageEditor.DirectWrite = true;

            // RaisePostBackEvent method eventArgument parameter scheme
            // int width; int height; AnchorType anchor
            // AnchorType values: TopLeft, TopCenter, TopRight, MiddleLeft, MiddleCenter, MiddleRight, BottomLeft, BottomCenter, BottomRight

            // make no changes
            var raisePostBackEventArgs = string.Format("{0};{1};TopLeft", InitialImageWidth, InitialImageHeight);

            // Act
            toolCanvasSize.RaisePostBackEvent(raisePostBackEventArgs);

            // Assert
            var imagePath = _shimHttpServerUtility.Instance.MapPath(_imageEditor.ImageURL);

            using (var image = ActivePixToolsTestHelper.LoadImage(imagePath))
            {
                image.ShouldNotBeNull();
            }
        }

        [Test]
        public void RaisePostBackEvent_IfDirectWriteSetAsFalse_SavesImageToAlternativePath()
        {
            // Arrange
            var toolCanvasSize = new ToolCanvasSize();

            _imageEditor.Toolbar.Tools.Add(toolCanvasSize);
            _imageEditor.DirectWrite = false;

            // RaisePostBackEvent method eventArgument parameter scheme
            // int width; int height; AnchorType anchor
            // AnchorType values: TopLeft, TopCenter, TopRight, MiddleLeft, MiddleCenter, MiddleRight, BottomLeft, BottomCenter, BottomRight

            // make no changes
            var raisePostBackEventArgs = string.Format("{0};{1};TopLeft", InitialImageWidth, InitialImageHeight);

            // Act
            toolCanvasSize.RaisePostBackEvent(raisePostBackEventArgs);

            // Assert
            var alternativeDirectoryFilesCount = Directory.GetFiles(ToolImagesAlternativeDirectory).Length;
            alternativeDirectoryFilesCount.ShouldBeGreaterThan(0);
        }

        [Test]
        public void RaisePostBackEvent_WhenCalledWithValidArguments_ResizesAndSavesImage()
        {
            // Arrange
            var toolCanvasSize = new ToolCanvasSize();

            _imageEditor.Toolbar.Tools.Add(toolCanvasSize);
            _imageEditor.DirectWrite = true;

            // RaisePostBackEvent method eventArgument parameter scheme
            // int width; int height; AnchorType anchor
            // AnchorType values: TopLeft, TopCenter, TopRight, MiddleLeft, MiddleCenter, MiddleRight, BottomLeft, BottomCenter, BottomRight

            // resize
            // width: 100, height: 50
            var raisePostBackEventArgs = "100;50;TopLeft;";

            // Act
            toolCanvasSize.RaisePostBackEvent(raisePostBackEventArgs);

            // Assert
            var imagePath = _shimHttpServerUtility.Instance.MapPath(_imageEditor.ImageURL);

            using (var image = ActivePixToolsTestHelper.LoadImage(imagePath))
            {
                image.Width.ShouldBe(100);
                image.Height.ShouldBe(50);
            }
        }

        [Test]
        public void RaisePostBackEvent_WhenCalledWithInvalidArguments_RegistersStartupScript()
        {
            // Arrange
            var toolCanvasSize = new ToolCanvasSize();

            _imageEditor.Toolbar.Tools.Add(toolCanvasSize);
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
            // int width; int height; bool constrainProportions; bool resizeSmaller
            // NOTE: job.ResizeImage() method that called inside of RaisePostBackEvent only resized image if
            // new values are smaller than original. resizeSmaller parameter doesn't work!

            // throw exception
            var raisePostBackEventArgs = "error prone argument to throw exception";

            // Act
            toolCanvasSize.RaisePostBackEvent(raisePostBackEventArgs);

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
