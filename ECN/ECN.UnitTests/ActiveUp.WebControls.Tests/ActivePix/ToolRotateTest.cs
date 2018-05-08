using System;
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
    public class ToolRotateTest
    {
        private const string ToolIdPrefix = "_toolRotate";
        private const int InitialImageWidth = 200;
        private const int InitialImageHeight = 100;

        private const string ToolImageName = "rotate_off.gif";
        private const string ToolOverImageName = "rotate_over.gif";
        private const string ToolImageResourceName = "ActiveUp.WebControls._resources.Images.rotate_off.gif";
        private const string ToolOverImageResourceName = "ActiveUp.WebControls._resources.Images.rotate_over.gif";

        private const string ToolImagesFolderName = "toolImages";
        private const string ToolImagesAlternativeFolderName = "toolAlternativeFolder";

        private readonly string ToolImagesDirectory = string.Format("{0}\\{1}", TestContext.CurrentContext.TestDirectory, ToolImagesFolderName);
        private readonly string ToolImagesAlternativeDirectory = string.Format("{0}\\{1}", TestContext.CurrentContext.TestDirectory, ToolImagesAlternativeFolderName);
        
        private IDisposable _shimsContext;

        private ShimPage _shimPage;
        private ShimHttpSessionState _shimHttpSessionState;
        private ShimHttpServerUtility _shimHttpServerUtility;
        private ShimClientScriptManager _shimClientScriptManager;

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
            var toolRotate = new ToolRotate();
            var expectedToolRotateId = string.Format("{0}{1}", ToolIdPrefix, ImageEditor.indexTools - 1);

            // Assert
            toolRotate.ID.ShouldBe(expectedToolRotateId);
        }

        [Test]
        public void Constructor_WithId_CreatesObjectWithGivenId()
        {
            // Arrange
            var expectedToolRotateId = "myToolRotate";

            // Act
            var toolRotate = new ToolRotate(expectedToolRotateId);

            // Assert
            toolRotate.ID.ShouldBe(expectedToolRotateId);
        }

        [Test]
        public void OnPreRender_SetsButtonImages()
        {
            // Arrange
            var toolRotate = new ToolRotate();
            toolRotate.Page = _shimPage;
            toolRotate.ImageURL = string.Empty;
            toolRotate.OverImageURL = string.Empty;

            _imageEditor.Toolbar.Tools.Add(toolRotate);

            var toolRotatePrivateObject = new MsUnitTesting.PrivateObject(toolRotate);

            // Act
            toolRotatePrivateObject.Invoke("OnPreRender", EventArgs.Empty);

            // Assert
            TestsHelper.AssertNotFX1(ToolImageResourceName, toolRotate.ImageURL);
            TestsHelper.AssertNotFX1(ToolOverImageResourceName, toolRotate.OverImageURL);

            TestsHelper.AssertFX1(ToolImageName, toolRotate.ImageURL);
            TestsHelper.AssertFX1(ToolOverImageName, toolRotate.OverImageURL);
        }

        [Test]
        public void ParentImageEditorGetter_IfToolIsNotAddedIntoAnImageEditor_ThrowsException()
        {
            // Arrange
            var toolRotate = new ToolRotate();

            // Act
            var getParentImageEditor = new Action(() => { var returnedImageEditor = toolRotate.ParentImageEditor; });

            // Assert
            getParentImageEditor.ShouldThrow<NullReferenceException>();
        }

        [Test]
        public void ParentImageEditorGetter_IfToolIsAddedIntoAnImageEditor_ReturnsImageEditor()
        {
            // Arrange
            var toolRotate = new ToolRotate();
            _imageEditor.Toolbar.Tools.Add(toolRotate);

            // Act
            var returnedImageEditor = toolRotate.ParentImageEditor;

            // Assert
            returnedImageEditor.ShouldBeSameAs(_imageEditor);
        }

        [Test]
        public void ParentToolbarGetter_IfToolIsAddedIntoAnImageEditor_ReturnsToolbarOfImageEditor()
        {
            // Arrange
            var toolRotate = new ToolRotate();
            _imageEditor.Toolbar.Tools.Add(toolRotate);

            // Act
            var returnedToolbar = toolRotate.ParentToolbar;

            // Assert
            returnedToolbar.ShouldBeSameAs(_imageEditor.Toolbar);
        }

        [Test]
        public void ParentToolbarGetter_IfToolIsNotAddedIntoAnImageEditor_ReturnsNull()
        {
            // Arrange
            var toolRotate = new ToolRotate();

            // Act
            var returnedToolbar = toolRotate.ParentToolbar;

            // Assert
            returnedToolbar.ShouldBeNull();
        }

        [Test]
        public void RaisePostBackEvent_IfDirectWriteSetAsTrue_SavesImageDirectly()
        {
            // Arrange
            var toolRotate = new ToolRotate();

            _imageEditor.Toolbar.Tools.Add(toolRotate);
            _imageEditor.DirectWrite = true;

            // RaisePostBackEvent method eventArgument parameter scheme
            // bool rotateLeft; bool rotateRight; bool rotateDegrees; float degrees

            // just save in current state
            var raisePostBackEventArgs = "false;false;false;0";

            // Act
            toolRotate.RaisePostBackEvent(raisePostBackEventArgs);

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
            var toolRotate = new ToolRotate();

            _imageEditor.Toolbar.Tools.Add(toolRotate);
            _imageEditor.DirectWrite = false;

            // RaisePostBackEvent method eventArgument parameter scheme
            // bool rotateLeft; bool rotateRight; bool rotateDegrees; float degrees

            // just save in current state
            var raisePostBackEventArgs = "false;false;false;0";

            // Act
            toolRotate.RaisePostBackEvent(raisePostBackEventArgs);

            // Assert
            var alternativeDirectoryFilesCount = Directory.GetFiles(ToolImagesAlternativeDirectory).Length;
            alternativeDirectoryFilesCount.ShouldBe(1);
        }

        [Test]
        public void RaisePostBackEvent_IfEventArgumentIsSetForRotateLeft_RotatesLeftAndSavesImage()
        {
            // Arrange
            var toolRotate = new ToolRotate();

            _imageEditor.Toolbar.Tools.Add(toolRotate);
            _imageEditor.DirectWrite = true;

            // RaisePostBackEvent method eventArgument parameter scheme
            // bool rotateLeft; bool rotateRight; bool rotateDegrees; float degrees

            // rotateLeft: true
            var raisePostBackEventArgs = "true;false;false;0";

            // Act
            toolRotate.RaisePostBackEvent(raisePostBackEventArgs);

            // Assert
            var imagePath = _shimHttpServerUtility.Instance.MapPath(_imageEditor.ImageURL);

            using (var image = ActivePixToolsTestHelper.LoadImage(imagePath))
            {
                image.Width.ShouldBe(InitialImageHeight);
                image.Height.ShouldBe(InitialImageWidth);

                var firstPixelOfImage = image.GetPixel(0, 0);

                firstPixelOfImage.A.ShouldBe(Color.Gray.A);
                firstPixelOfImage.R.ShouldBe(Color.Gray.R);
                firstPixelOfImage.G.ShouldBe(Color.Gray.G);
                firstPixelOfImage.B.ShouldBe(Color.Gray.B);
            }
        }

        [Test]
        public void RaisePostBackEvent_IfEventArgumentIsSetForRotateRight_RotatesRightAndSavesImage()
        {
            // Arrange
            var toolRotate = new ToolRotate();

            _imageEditor.Toolbar.Tools.Add(toolRotate);
            _imageEditor.DirectWrite = true;

            // RaisePostBackEvent method eventArgument parameter scheme
            // bool rotateLeft; bool rotateRight; bool rotateDegrees; float degrees

            // rotateRight: true
            var raisePostBackEventArgs = "false;true;false;0";

            // Act
            toolRotate.RaisePostBackEvent(raisePostBackEventArgs);

            // Assert
            var imagePath = _shimHttpServerUtility.Instance.MapPath(_imageEditor.ImageURL);

            using (var image = ActivePixToolsTestHelper.LoadImage(imagePath))
            {
                image.Width.ShouldBe(InitialImageHeight);
                image.Height.ShouldBe(InitialImageWidth);

                var firstPixelOfImage = image.GetPixel(0, 0);

                firstPixelOfImage.A.ShouldBe(Color.Black.A);
                firstPixelOfImage.R.ShouldBe(Color.Black.R);
                firstPixelOfImage.G.ShouldBe(Color.Black.G);
                firstPixelOfImage.B.ShouldBe(Color.Black.B);
            }
        }

        [Test]
        public void RaisePostBackEvent_IfEventArgumentIsSetForRotateDegreesAnd180Degrees_Rotates180DegreesAndSavesImage()
        {
            // Arrange
            var toolRotate = new ToolRotate();

            _imageEditor.Toolbar.Tools.Add(toolRotate);
            _imageEditor.DirectWrite = true;

            // RaisePostBackEvent method eventArgument parameter scheme
            // bool rotateLeft; bool rotateRight; bool rotateDegrees; float degrees

            // rotateDegrees: true; degrees: 180
            var raisePostBackEventArgs = "false;false;true;180";

            // Act
            toolRotate.RaisePostBackEvent(raisePostBackEventArgs);

            // Assert
            var imagePath = _shimHttpServerUtility.Instance.MapPath(_imageEditor.ImageURL);

            using (var image = ActivePixToolsTestHelper.LoadImage(imagePath))
            {
                image.Width.ShouldBe(InitialImageWidth);
                image.Height.ShouldBe(InitialImageHeight);

                var firstPixelOfImage = image.GetPixel(0, 0);

                firstPixelOfImage.A.ShouldBe(Color.Gray.A);
                firstPixelOfImage.R.ShouldBe(Color.Gray.R);
                firstPixelOfImage.G.ShouldBe(Color.Gray.G);
                firstPixelOfImage.B.ShouldBe(Color.Gray.B);
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
        }

        private void SetupPage()
        {
            if (_shimsContext == null)
            {
                throw new InvalidOperationException("_shimsContext local member must be initialized to setup the Page shim object");
            }

            ShimPage.AllInstances.GetPostBackClientEventControlString = (page, control, value) => string.Empty;

            _shimPage = new ShimPage();
            _shimPage.SessionGet = () => _shimHttpSessionState;
            _shimPage.ServerGet = () => _shimHttpServerUtility;
            _shimPage.ClientScriptGet = () => _shimClientScriptManager;
        }
    }
}
