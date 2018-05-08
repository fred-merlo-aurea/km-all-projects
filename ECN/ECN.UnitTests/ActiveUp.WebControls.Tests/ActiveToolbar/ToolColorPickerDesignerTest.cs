using System;
using System.Drawing;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ActiveUp.WebControls.Fakes;
using ActiveUp.WebControls.Common.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveToolbar
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ToolColorPickerDesignerTest
    {
        private IDisposable _shims;
        private const string image = "image";
        private const string value = "5";

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
        }

        [Test]
        public void DesignColorPicker_ForUseSquareColorTrue_WritesOutput()
        {
            // Arrange
            SetUp(true);
            var textWriter = new Moq.Mock<TextWriter>();
            using (var toolColorPicker = new ToolColorPicker())
            {
                var htmlTextWriter = new HtmlTextWriter(textWriter.Object);

                // Act
                ToolColorPickerDesigner.DesignColorPicker(ref htmlTextWriter, toolColorPicker);

                // Assert
                htmlTextWriter.ShouldSatisfyAllConditions(
                    () => htmlTextWriter.Indent.ShouldBe(0),
                    () => htmlTextWriter.NewLine.ShouldBeNull());
            }
        }

        [Test]
        public void DesignColorPicker_ForUseSquareColorFalse_WritesOutput()
        {
            // Arrange
            SetUp(false);
            var textWriter = new Moq.Mock<TextWriter>();
            using (var toolColorPicker = new ToolColorPicker())
            {
                var htmlTextWriter = new HtmlTextWriter(textWriter.Object);

                // Act
                ToolColorPickerDesigner.DesignColorPicker(ref htmlTextWriter, toolColorPicker);

                // Assert
                htmlTextWriter.ShouldSatisfyAllConditions(
                    () => htmlTextWriter.Indent.ShouldBe(0),
                    () => htmlTextWriter.NewLine.ShouldBeNull());
            }
        }

        protected void SetUp(Boolean param)
        {
            _shims = ShimsContext.Create();
            ShimToolBase.AllInstances.BackImageGet = (x) => image;
            ShimWebControl.AllInstances.HeightGet = (x) => new Unit(value);
            ShimToolColorPicker.AllInstances.UseSquareColorGet = (x) => true;
            if (param)
            {
                ShimToolBase.AllInstances.DropDownImageGet = (x) => image;
            }
            else
            {
                ShimWebControl.AllInstances.WidthGet = (x) => Unit.Empty;
                ShimToolBase.AllInstances.DropDownImageGet = (x) => string.Empty;
                ShimCoreWebControl.AllInstances.ImagesDirectoryGet = (x) => image;
            }
            ShimWebControl.AllInstances.ForeColorGet = (x) => Color.AliceBlue;
        }
    }
}
