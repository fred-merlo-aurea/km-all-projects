using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.includes;
using ecn.communicator.includes.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Includes
{
    /// <summary>
    /// UT for <see cref="gallery"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class GalleryTest
    {
        private const string ImageRepeater = "imagerepeater";
        private const string DeleteImage = "DeleteImage.png";
        private const string TabPreview = "tabPreview";
        private const string TabUpload = "tabUpload";
        private const string TabBrowse = "tabBrowse";
        private const string PanelBrowse = "PanelBrowse";
        private const string PanelPreview = "PanelPreview";
        private const string PanelUpload = "PanelUpload";
        private const string ImagePreview = "ImagePreview";
        private const string ShowPreviewPanel = "showPreviewPanel";
        private const string PanelFolders = "PanelFolders";
        private PrivateObject _privateObject;
        private LinkButton _tabPreview;
        private LinkButton _tabUpload;
        private LinkButton _tabBrowse;
        private Panel _panelBrowse;
        private Panel _panelPreview;
        private Panel _panelUpload;
        private Panel _showPreviewPanel;
        private Panel _panelFolders;

        [Test]
        public void BorderWidth_SetValue_ReturnsSetValue()
        {
            // Arrange
            using (var testObject = new gallery())
            {
                const string TestValue = "10";
                var privateObject = new PrivateObject(testObject);
                privateObject.SetFieldOrProperty(ImageRepeater, new DataList());

                // Act
                testObject.borderWidth = TestValue;

                // Assert
                testObject.borderWidth.ShouldBe($"{TestValue}px");
            }
        }

        [Test]
        public void DeleteImage_Invoke_InvokesBusinesLayerDeleteImage()
        {
            // Arrange
            using (ShimsContext.Create())
            {
                var imageToDelete = string.Empty;
                var loadBrowseTableCalled = false;
                var showBrowseCalled = false;
                ShimFileInfo.AllInstances.Delete = input => { };
                Shimgallery.AllInstances.loadBrowseTable = gallery => loadBrowseTableCalled = true;
                Shimgallery.AllInstances.showBrowseObjectEventArgs = (gallery, obj, eventArgs) => showBrowseCalled = true;
                ShimUserControl.AllInstances.ServerGet = obj => new ShimHttpServerUtility();
                ShimHttpServerUtility.AllInstances.MapPathString = (obj, input) =>
                    {
                        imageToDelete = input;
                        return DeleteImage;
                    };

                using (var testObject = new gallery())
                {
                    var privateObject = new PrivateObject(testObject);
                    privateObject.SetFieldOrProperty(ImagePreview, new Image());

                    // Act
                    testObject.deleteImage(null, new CommandEventArgs(null, DeleteImage));

                    // Assert
                    testObject.ShouldSatisfyAllConditions(() => imageToDelete.ShouldBe(DeleteImage), () => loadBrowseTableCalled.ShouldBeTrue(), () => showBrowseCalled.ShouldBeTrue());
                }
            }
        }

        [Test]
        public void ShowBrowse_Invoke_SetsOrResetsVisibility()
        {
            // Arrange
            using (var testObject = new gallery())
            {
                InitControls(testObject);

                // Act
                testObject.showBrowse(null, new CommandEventArgs(null, DeleteImage));

                // Assert
                testObject.ShouldSatisfyAllConditions(
                    () => _tabPreview?.Visible.ShouldBeFalse(),
                    () => _tabUpload?.Visible.ShouldBeTrue(),
                    () => _tabBrowse?.Visible.ShouldBeTrue(),
                    () => _panelBrowse?.Visible.ShouldBeTrue(),
                    () => _panelPreview?.Visible.ShouldBeFalse(),
                    () => _panelUpload?.Visible.ShouldBeFalse(),
                    () => _showPreviewPanel?.Visible.ShouldBeFalse(),
                    () => _panelFolders?.Visible.ShouldBeFalse());
            }
        }

        [Test]
        public void ShowPreview_Invoke_SetsOrResetsVisibility()
        {
            // Arrange
            using (var testObject = new gallery())
            {
                InitControls(testObject);

                // Act
                testObject.showPreview(null, new CommandEventArgs(null, DeleteImage));

                // Assert
                testObject.ShouldSatisfyAllConditions(
                    () => _tabPreview?.Visible.ShouldBeTrue(),
                    () => _tabUpload?.Visible.ShouldBeTrue(),
                    () => _tabBrowse?.Visible.ShouldBeTrue(),
                    () => _panelBrowse?.Visible.ShouldBeFalse(),
                    () => _panelPreview?.Visible.ShouldBeTrue(),
                    () => _panelUpload?.Visible.ShouldBeFalse(),
                    () => _showPreviewPanel?.Visible.ShouldBeTrue(),
                    () => _panelFolders?.Visible.ShouldBeFalse());
            }
        }

        [Test]
        public void ShowUpload_Invoke_SetsOrResetsVisibility()
        {
            // Arrange
            using (var testObject = new gallery())
            {
                InitControls(testObject);

                // Act
                testObject.showUpload(null, new CommandEventArgs(null, DeleteImage));

                // Assert
                testObject.ShouldSatisfyAllConditions(
                    () => _tabPreview?.Visible.ShouldBeFalse(),
                    () => _tabUpload?.Visible.ShouldBeTrue(),
                    () => _tabBrowse?.Visible.ShouldBeTrue(),
                    () => _panelBrowse?.Visible.ShouldBeFalse(),
                    () => _panelPreview?.Visible.ShouldBeFalse(),
                    () => _panelUpload?.Visible.ShouldBeTrue(),
                    () => _showPreviewPanel?.Visible.ShouldBeFalse(),
                    () => _panelFolders?.Visible.ShouldBeFalse());
            }
        }

        private void InitControls(gallery testObject)
        {
            _privateObject = new PrivateObject(testObject);
            _privateObject.SetFieldOrProperty(TabPreview, new LinkButton());
            _privateObject.SetFieldOrProperty(TabUpload, new LinkButton());
            _privateObject.SetFieldOrProperty(TabBrowse, new LinkButton());
            _privateObject.SetFieldOrProperty(PanelBrowse, new Panel());
            _privateObject.SetFieldOrProperty(PanelPreview, new Panel());
            _privateObject.SetFieldOrProperty(PanelUpload, new Panel());
            _privateObject.SetFieldOrProperty(ShowPreviewPanel, new Panel());
            _privateObject.SetFieldOrProperty(PanelFolders, new Panel());

            _tabPreview = _privateObject.GetFieldOrProperty(TabPreview) as LinkButton;
            _tabUpload = _privateObject.GetFieldOrProperty(TabUpload) as LinkButton;
            _tabBrowse = _privateObject.GetFieldOrProperty(TabBrowse) as LinkButton;
            _panelBrowse = _privateObject.GetFieldOrProperty(PanelBrowse) as Panel;
            _panelPreview = _privateObject.GetFieldOrProperty(PanelPreview) as Panel;
            _panelUpload = _privateObject.GetFieldOrProperty(PanelUpload) as Panel;
            _showPreviewPanel = _privateObject.GetFieldOrProperty(ShowPreviewPanel) as Panel;
            _panelFolders = _privateObject.GetFieldOrProperty(PanelFolders) as Panel;
        }
    }
}