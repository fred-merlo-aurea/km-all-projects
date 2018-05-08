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
    public class ToolTextTest
    {
        private const string ToolIdPrefix = "_toolText";
        private const int InitialImageWidth = 200;
        private const int InitialImageHeight = 100;

        private const string ToolImageName = "text_off.gif";
        private const string ToolOverImageName = "text_over.gif";
        private const string ToolImageResourceName = "ActiveUp.WebControls._resources.Images.text_off.gif";
        private const string ToolOverImageResourceName = "ActiveUp.WebControls._resources.Images.text_over.gif";

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
            var toolText = new ToolText();
            var expectedToolTextId = string.Format("{0}{1}", ToolIdPrefix, ImageEditor.indexTools - 1);

            // Assert
            toolText.ID.ShouldBe(expectedToolTextId);
        }

        [Test]
        public void Constructor_WithId_CreatesObjectWithGivenId()
        {
            // Arrange
            var expectedToolTextId  = "myToolText";

            // Act
            var toolText = new ToolText(expectedToolTextId);

            // Assert
            toolText.ID.ShouldBe(expectedToolTextId);
        }

        [Test]
        public void OnPreRender_SetsButtonImages()
        {
            // Arrange
            var toolText = new ToolText();
            toolText.Page = _shimPage;
            toolText.ImageURL = string.Empty;
            toolText.OverImageURL = string.Empty;

            _imageEditor.Toolbar.Tools.Add(toolText);

            var toolTextPrivateObject = new MsUnitTesting.PrivateObject(toolText);

            // Act
            toolTextPrivateObject.Invoke("OnPreRender", EventArgs.Empty);

            // Assert
            TestsHelper.AssertNotFX1(ToolImageResourceName, toolText.ImageURL);
            TestsHelper.AssertNotFX1(ToolOverImageResourceName, toolText.OverImageURL);

            TestsHelper.AssertFX1(ToolImageName, toolText.ImageURL);
            TestsHelper.AssertFX1(ToolOverImageName, toolText.OverImageURL);
        }

        [Test]
        public void ParentImageEditorGetter_IfToolIsNotAddedIntoAnImageEditor_ThrowsException()
        {
            // Arrange
            var toolText = new ToolText();

            // Act
            var getParentImageEditor = new Action(() => { var returnedImageEditor = toolText.ParentImageEditor; });

            // Assert
            getParentImageEditor.ShouldThrow<NullReferenceException>();
        }

        [Test]
        public void ParentImageEditorGetter_IfToolIsAddedIntoAnImageEditor_ReturnsImageEditor()
        {
            // Arrange
            var toolText = new ToolText();
            _imageEditor.Toolbar.Tools.Add(toolText);

            // Act
            var returnedImageEditor = toolText.ParentImageEditor;

            // Assert
            returnedImageEditor.ShouldBeSameAs(_imageEditor);
        }

        [Test]
        public void ParentToolbarGetter_IfToolIsAddedIntoAnImageEditor_ReturnsToolbarOfImageEditor()
        {
            // Arrange
            var toolText = new ToolText();
            _imageEditor.Toolbar.Tools.Add(toolText);

            // Act
            var returnedToolbar = toolText.ParentToolbar;

            // Assert
            returnedToolbar.ShouldBeSameAs(_imageEditor.Toolbar);
        }

        [Test]
        public void ParentToolbarGetter_IfToolIsNotAddedIntoAnImageEditor_ReturnsNull()
        {
            // Arrange
            var toolText = new ToolText();

            // Act
            var returnedToolbar = toolText.ParentToolbar;

            // Assert
            returnedToolbar.ShouldBeNull();
        }

        [Test]
        public void RaisePostBackEvent_IfDirectWriteSetAsTrue_SavesImageDirectly()
        {
            // Arrange
            var toolText = new ToolText();

            _imageEditor.Toolbar.Tools.Add(toolText);
            _imageEditor.DirectWrite = true;

            // RaisePostBackEvent method eventArgument parameter scheme
            // string text; Color forecolor; FontStyle style; int size; string font; boolean antialias; StringAlignment alignment

            // sample args to toolText
            var raisePostBackEventArgs = "lorem ipsum; White; Regular; 20; Arial; false; Near";

            // text starting point
            _imageEditor.Selection.X1 = 10;
            _imageEditor.Selection.Y1 = 10;

            // Act
            toolText.RaisePostBackEvent(raisePostBackEventArgs);

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
            var toolText = new ToolText();

            _imageEditor.Toolbar.Tools.Add(toolText);
            _imageEditor.DirectWrite = false;

            // RaisePostBackEvent method eventArgument parameter scheme
            // float factor

            // sample args to toolText
            var raisePostBackEventArgs = "lorem ipsum; White; Regular; 20; Arial; false; Near";

            // text starting point
            _imageEditor.Selection.X1 = 10;
            _imageEditor.Selection.Y1 = 10;

            // Act
            toolText.RaisePostBackEvent(raisePostBackEventArgs);

            // Assert
            var alternativeDirectoryFilesCount = Directory.GetFiles(ToolImagesAlternativeDirectory).Length;
            alternativeDirectoryFilesCount.ShouldBe(1);
        }

        [Test]
        public void RaisePostBackEvent_IfEventArgumentIsInvalid_RegistersStartupScript()
        {
            // Arrange
            var toolText = new ToolText();

            _imageEditor.Toolbar.Tools.Add(toolText);
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
            // string text; Color forecolor; FontStyle style; int size; string font; boolean antialias; StringAlignment alignment

            // sample args to toolText
            var raisePostBackEventArgs = "error prone argument to get exception inside";

            // Act
            toolText.RaisePostBackEvent(raisePostBackEventArgs);

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
