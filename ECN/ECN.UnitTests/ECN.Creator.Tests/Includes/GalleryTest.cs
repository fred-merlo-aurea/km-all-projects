using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.creator.includes;
using ecn.creator.includes.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Creator.Tests.Includes
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
        private const string PanelBrowseOther = "PanelBrowseOther";
        private const string PageUploader = "pageuploader";
        private PrivateObject _privateObject;
        private LinkButton _tabPreview;
        private LinkButton _tabUpload;
        private LinkButton _tabBrowse;
        private Panel _panelBrowse;
        private Panel _panelPreview;
        private Panel _panelUpload;
        private Panel _panelBrowseOther;

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
                ShimFileInfo.AllInstances.Delete = input => { };
                Shimgallery.AllInstances.loadBrowseTable = gallery => { };
                Shimgallery.AllInstances.showBrowseObjectEventArgs = (gallery, obj, eventArgs) => { };
                ShimUserControl.AllInstances.ServerGet = obj => new ShimHttpServerUtility();
                ShimHttpServerUtility.AllInstances.MapPathString = (obj, input) =>
                    {
                        imageToDelete = input;
                        return DeleteImage;
                    };
                ShimUserControl.AllInstances.ResponseGet = (obj) => new ShimHttpResponse().Instance;
                ShimHttpResponse.AllInstances.RedirectString = (obj, input) => { };

                using (var testObject = new gallery())
                {
                    var privateObject = new PrivateObject(testObject);
                    privateObject.SetFieldOrProperty(ImagePreview, new Image());

                    // Act
                    testObject.deleteImage(null, new CommandEventArgs(null, DeleteImage));

                    // Assert
                    imageToDelete.ShouldBe(DeleteImage);
                }
            }
        }

        [Test]
        public void ShowBrowse_Invoke_SetsOrResetsVisibility()
        {
            // Arrange
            using (ShimsContext.Create())
            {
                Shimgallery.AllInstances.loadBrowseTable = gallery => { };

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
                        () => _panelBrowseOther?.Visible.ShouldBeFalse());
                }
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
                    () => _panelBrowseOther?.Visible.ShouldBeFalse());
            }
        }

        [Test]
        public void ShowUpload_Invoke_SetsOrResetsVisibility()
        {
            // Arrange
            using (ShimsContext.Create())
            {
                Shimuploader.AllInstances.uploadDirectorySetString = (uploader, s) => { };
                Shimgallery.AllInstances.imageDirectoryGet = gallery => DeleteImage;

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
                        () => _panelBrowseOther?.Visible.ShouldBeFalse());
                }
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
            _privateObject.SetFieldOrProperty(PanelBrowseOther, new Panel());
            _privateObject.SetFieldOrProperty(PageUploader, new uploader());

            _tabPreview = _privateObject.GetFieldOrProperty(TabPreview) as LinkButton;
            _tabUpload = _privateObject.GetFieldOrProperty(TabUpload) as LinkButton;
            _tabBrowse = _privateObject.GetFieldOrProperty(TabBrowse) as LinkButton;
            _panelBrowse = _privateObject.GetFieldOrProperty(PanelBrowse) as Panel;
            _panelPreview = _privateObject.GetFieldOrProperty(PanelPreview) as Panel;
            _panelUpload = _privateObject.GetFieldOrProperty(PanelUpload) as Panel;
            _panelBrowseOther = _privateObject.GetFieldOrProperty(PanelBrowseOther) as Panel;
        }
    }
}
