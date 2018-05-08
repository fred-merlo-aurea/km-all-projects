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
    public class ToolFlipTest
    {
        private const string ToolIdPrefix = "_toolFlip";
        private const int InitialImageWidth = 200;
        private const int InitialImageHeight = 100;

        private const string ToolImageName = "flip_off.gif";
        private const string ToolOverImageName = "flip_over.gif";
        private const string ToolImageResourceName = "ActiveUp.WebControls._resources.Images.flip_off.gif";
        private const string ToolOverImageResourceName = "ActiveUp.WebControls._resources.Images.flip_over.gif";

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
            var image = ActivePixToolsTestHelper.CreateDummyImage(InitialImageWidth, InitialImageHeight, ActivePixToolsTestHelper.ColorsDirection.Vertical, new Color[] { Color.Black, Color.Gray });
            ActivePixToolsTestHelper.SaveImage(image, imageFileServerPath);

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
            var toolFlip = new ToolFlip();
            var expectedToolFlipId = string.Format("{0}{1}", ToolIdPrefix, ImageEditor.indexTools);

            // Assert
            toolFlip.ID.ShouldBe(expectedToolFlipId);
        }

        [Test]
        public void Constructor_WithId_CreatesObjectWithGivenId()
        {
            // Arrange
            var expectedToolFlipId = "myToolFlip";

            // Act
            var toolFlip = new ToolFlip(expectedToolFlipId);

            // Assert
            toolFlip.ID.ShouldBe(expectedToolFlipId);
        }

        [Test]
        public void OnPreRender_SetsButtonImages()
        {
            // Arrange
            var toolFlip = new ToolFlip();
            toolFlip.Page = _shimPage;
            toolFlip.ImageURL = string.Empty;
            toolFlip.OverImageURL = string.Empty;

            _imageEditor.Toolbar.Tools.Add(toolFlip);

            var toolFlipPrivateObject = new MsUnitTesting.PrivateObject(toolFlip);

            // Act
            toolFlipPrivateObject.Invoke("OnPreRender", EventArgs.Empty);

            // Assert
            TestsHelper.AssertNotFX1(ToolImageResourceName, toolFlip.ImageURL);
            TestsHelper.AssertNotFX1(ToolOverImageResourceName, toolFlip.OverImageURL);

            TestsHelper.AssertFX1(ToolImageName, toolFlip.ImageURL);
            TestsHelper.AssertFX1(ToolOverImageName, toolFlip.OverImageURL);
        }

        [Test]
        public void ParentImageEditorGetter_IfToolIsNotAddedIntoImageEditor_ThrowsException()
        {
            // Arrange
            var toolFlip = new ToolFlip();

            // Act
            var getParentImageEditor = new Action(() => { var returnedImageEditor = toolFlip.ParentImageEditor; });

            // Assert
            getParentImageEditor.ShouldThrow<NullReferenceException>();
        }

        [Test]
        public void ParentImageEditorGetter_IfToolIsAddedIntoAnImageEditor_ReturnsImageEditor()
        {
            // Arrange
            var toolFlip = new ToolFlip();
            _imageEditor.Toolbar.Tools.Add(toolFlip);

            // Act
            var returnedImageEditor = toolFlip.ParentImageEditor;

            // Assert
            returnedImageEditor.ShouldBeSameAs(_imageEditor);
        }

        [Test]
        public void ParentToolbarGetter_IfToolIsAddedIntoAnImageEditor_ReturnsToolbarOfImageEditor()
        {
            // Arrange
            var toolFlip = new ToolFlip();
            _imageEditor.Toolbar.Tools.Add(toolFlip);

            // Act
            var returnedToolbar = toolFlip.ParentToolbar;

            // Assert
            returnedToolbar.ShouldBeSameAs(_imageEditor.Toolbar);
        }

        [Test]
        public void ParentToolbarGetter_IfToolIsNotAddedIntoAnImageEditor_ReturnsNull()
        {
            // Arrange
            var toolFlip = new ToolFlip();

            // Act
            var returnedToolbar = toolFlip.ParentToolbar;

            // Assert
            returnedToolbar.ShouldBeNull();
        }

        [Test]
        public void FlipClicked_IfDirectWriteSetAsTrue_FlipsAndSavesImageDirectly()
        {
            // Arrange
            var toolFlip = new ToolFlip();
            var toolFlipPrivateWrapper = new MsUnitTesting.PrivateObject(toolFlip);

            _imageEditor.Toolbar.Tools.Add(toolFlip);
            _imageEditor.DirectWrite = true;

            // Act
            toolFlipPrivateWrapper.Invoke("FlipClicked", null, EventArgs.Empty);

            // Assert
            var imagePath = _shimHttpServerUtility.Instance.MapPath(_imageEditor.ImageURL);

            using (var image = ActivePixToolsTestHelper.LoadImage(imagePath))
            {
                var firstPixelOfTheImage = image.GetPixel(0, 0);

                firstPixelOfTheImage.A.ShouldBe(Color.Gray.A);
                firstPixelOfTheImage.R.ShouldBe(Color.Gray.R);
                firstPixelOfTheImage.G.ShouldBe(Color.Gray.G);
                firstPixelOfTheImage.B.ShouldBe(Color.Gray.B);
            }
        }

        [Test]
        public void FlipClicked_IfDirectWriteSetAsFalse_FlipsAndSavesImageToAlternativePath()
        {
            // Arrange
            var toolFlip = new ToolFlip();
            var toolFlipPrivateWrapper = new MsUnitTesting.PrivateObject(toolFlip);

            _imageEditor.Toolbar.Tools.Add(toolFlip);
            _imageEditor.DirectWrite = false;

            // Act
            toolFlipPrivateWrapper.Invoke("FlipClicked", null, EventArgs.Empty);

            // Assert
            var alternativeDirectoryFileNames = Directory.GetFiles(ToolImagesAlternativeDirectory);

            alternativeDirectoryFileNames.Length.ShouldBe(1);

            var imagePath = alternativeDirectoryFileNames[0];

            using (var image = ActivePixToolsTestHelper.LoadImage(imagePath))
            {
                var firstPixelOfTheImage = image.GetPixel(0, 0);

                firstPixelOfTheImage.A.ShouldBe(Color.Gray.A);
                firstPixelOfTheImage.R.ShouldBe(Color.Gray.R);
                firstPixelOfTheImage.G.ShouldBe(Color.Gray.G);
                firstPixelOfTheImage.B.ShouldBe(Color.Gray.B);
            }
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
                throw new NullReferenceException("_shimsContext local member must be initialized to setup the Page shim object");
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
