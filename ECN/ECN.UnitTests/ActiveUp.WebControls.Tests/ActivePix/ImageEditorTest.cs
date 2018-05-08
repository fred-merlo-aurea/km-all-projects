using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web.UI;
using System.Web.UI.Fakes;
using ActiveUp.WebControls.Common.Fakes;
using ActiveUp.WebControls.Common.Interface;
using ActiveUp.WebControls.Fakes;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;


namespace ActiveUp.WebControls.Tests.ActivePix
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ImageEditorTest
    {
        private const string ImageDummyDirectory1 = @"C:\temp1";
        private const string ImageDummyDirectory2 = @"C:\temp2";
        private const string TestValue = "Test1";
        private ImageEditor _imageEditor;
        private PrivateObject _privateObject;
        private IDisposable _shim;

        [TearDown]
        public void TearDown()
        {
            ReleaseFakes();
        }

        [Test]
        public void IconsDirectory_SetAndGetValue_ReturnsTheSetValue()
        {
            //Arrange
            //Create the control, start tracking viewstate, then set a new Text value.
            var imageEditor = new ImageEditor();
            imageEditor.IconsDirectory = ImageDummyDirectory1;
            ((IControl)imageEditor).TrackViewState();
            imageEditor.IconsDirectory = ImageDummyDirectory2;

            //Save the control's state
            var viewState = ((IControl)imageEditor).SaveViewState();

            //Act
            //Create a new control instance and load the state back into it, overriding any existing values.
            var editor = new ImageEditor();
            imageEditor.IconsDirectory = ImageDummyDirectory1;
            ((IControl)editor).LoadViewState(viewState);

            //Assert
            NUnit.Framework.Assert.AreEqual(ImageDummyDirectory2, editor.IconsDirectory,
                "Value restored from viewstate does not match the original value we set");
        }

        [Test]
        public void IconsDirectory_DefaultValue_ReturnsEmptyStringOrIconsDirectory()
        {
            // Arrange
            using (var testObject = new Menu())
            {
                // Act, Assert
                AssertNotFX1(string.Empty, testObject.IconsDirectory);
                AssertFX1(Define.IMAGES_DIRECTORY, testObject.IconsDirectory);
            }
        }

        [Test]
        public void IconsDirectory_SetAndGetValue_ReturnsSetValue()
        {
            // Arrange
            using (var testObject = new Menu())
            {
                // Act
                testObject.IconsDirectory = TestValue;

                // Assert
                testObject.IconsDirectory.ShouldBe(TestValue);
            }
        }

        [Test]
        public void Render_IfImageUrlIsNotNull_WriteOutput()
        {
            // Arrange
            _imageEditor = new ImageEditor();
            _privateObject = new PrivateObject(_imageEditor);
            HtmlTextWriter htmlTextWriter = Setup("x");

            // Act
            _privateObject.Invoke("Render", htmlTextWriter);

            // Assert
            htmlTextWriter.ShouldSatisfyAllConditions
                (
                    () => htmlTextWriter.Indent.ShouldBe(1),
                    () => htmlTextWriter.NewLine.ShouldBe(null)
                );
        }

        [Test]
        public void Render_IfImageUrlIsNull_WriteOutput()
        {
            // Arrange
            _imageEditor = new ImageEditor();
            _privateObject = new PrivateObject(_imageEditor);
            HtmlTextWriter htmlTextWriter = Setup("z");

            // Act
            _privateObject.Invoke("Render", htmlTextWriter);

            // Assert
            htmlTextWriter.ShouldSatisfyAllConditions
                (
                    () => htmlTextWriter.Indent.ShouldBe(0),
                    () => htmlTextWriter.NewLine.ShouldBe(null)
                );
        }

        [Test]
        public void Render_IfEditorModeIsUpdate_WriteOutput()
        {
            // Arrange
            _imageEditor = new ImageEditor();
            _privateObject = new PrivateObject(_imageEditor);
            HtmlTextWriter htmlTextWriter = Setup("y");

            // Act
            _privateObject.Invoke("Render", htmlTextWriter);

            // Assert
            htmlTextWriter.ShouldSatisfyAllConditions
                (
                    () => htmlTextWriter.Indent.ShouldBe(0),
                    () => htmlTextWriter.NewLine.ShouldBe(null)
                );
        }

        [Test]
        public void Render_ForNulImageUrlAndModeIsUpload_WriteOutput()
        {
            // Arrange
            _imageEditor = new ImageEditor();
            _privateObject = new PrivateObject(_imageEditor);
            HtmlTextWriter htmlTextWriter = Setup("v");

            // Act
            _privateObject.Invoke("Render", htmlTextWriter);

            // Assert
            htmlTextWriter.ShouldSatisfyAllConditions
                (
                    () => htmlTextWriter.Indent.ShouldBe(0),
                    () => htmlTextWriter.NewLine.ShouldBe(null)
                );
        }

        protected HtmlTextWriter Setup(String param)
        {

            var textWritter = new Moq.Mock<TextWriter>();
            var htmlTextWritter = new HtmlTextWriter(textWritter.Object);
            _shim = ShimsContext.Create();
            ShimControl.AllInstances.ClientIDGet = (v) => "10:10";
            ShimControl.AllInstances.PageGet = (v) => new Page();
            if (param.Equals("x"))
            {
                ShimImageEditor.AllInstances.ImageURLGet = (x) => "image.com";
                ShimImageEditor.AllInstances.ImageWidthGet = (x) => 100;
                ShimImageEditor.AllInstances.ImageHeightGet = (x) => 200;
                ShimImageEditor.AllInstances.DirectWriteGet = (x) => true;
                ShimImageEditor.AllInstances.TdCssClassGet = (x) => "tdcss";
                ShimCoreWebControl.AllInstances.IconsDirectoryGet = (x) => "dir";
            }
            else if (param.Equals("y"))
            {
                ShimImageEditor.AllInstances.ImageURLGet = (y) => "image.com";
                ShimImageEditor.AllInstances.AllowUploadGet = (y) => false;
                ShimImageEditor.AllInstances.EditorModeGet = (y) => ImageEditorMode.Upload;
            }
            else if (param.Equals("z"))
            {
                ShimImageEditor.AllInstances.AllowUploadGet = (z) => true;
                ShimImageEditor.AllInstances.EditorModeGet = (z) => ImageEditorMode.Edit;
            }
            else
            {
                ShimImageEditor.AllInstances.AllowUploadGet = (u) => false;
                ShimImageEditor.AllInstances.EditorModeGet = (u) => ImageEditorMode.Upload;
            }

            return htmlTextWritter;
        }

        protected void ReleaseFakes()
        {
            _shim?.Dispose();
        }
    }
}
