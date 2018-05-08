using System;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ActiveUp.WebControls.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveToolbar
{
    [TestFixture]
    public class ToolDropDownListDesignerTest
    {
        private IDisposable _shims;
        private const string imageDir = "$IMAGESDIRECTORY$";
        private const string image = "image";
        private const string value = "5";
        private const int indentValue = 0;
        private const string negativeIndex = "x";
        private const string indexText = "y";
        private const string indexValue = "z";

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
        }

        [Test]
        public void DesignDropDownList_ForNegativeIndex_WriteOutput()
        {
            // Arrange
            SetUp(negativeIndex);
            var textWriter = new Moq.Mock<TextWriter>();
            using (var toolDropDownList = new ToolDropDownList())
            {
                var htmlTextWriter = new HtmlTextWriter(textWriter.Object);

                // Act
                ToolDropDownListDesigner.DesignDropDownList(ref htmlTextWriter, toolDropDownList);

                // Assert
                htmlTextWriter.ShouldSatisfyAllConditions(
                    () => htmlTextWriter.Indent.ShouldBe(indentValue),
                    () => htmlTextWriter.NewLine.ShouldBeNull());
            } 
        }

        [Test]
        public void DesignDropDownList_ForSelectedIndexText_WriteOutput()
        {
            // Arrange
            SetUp(indexText);
            var textWriter = new Moq.Mock<TextWriter>();
            using (var toolDropDownList = new ToolDropDownList())
            {
                var htmlTextWriter = new HtmlTextWriter(textWriter.Object);

                // Act
                ToolDropDownListDesigner.DesignDropDownList(ref htmlTextWriter, toolDropDownList);

                // Assert
                htmlTextWriter.ShouldSatisfyAllConditions(
                    () => htmlTextWriter.Indent.ShouldBe(indentValue),
                    () => htmlTextWriter.NewLine.ShouldBeNull());
            }
        }

        [Test]
        public void DesignDropDownList_ForSelectedIndexValue_WriteOutput()
        {
            // Arrange
            SetUp(indexValue);
            var textWriter = new Moq.Mock<TextWriter>();
            using (var toolDropDownList = new ToolDropDownList())
            {
                var htmlTextWriter = new HtmlTextWriter(textWriter.Object);

                // Act
                ToolDropDownListDesigner.DesignDropDownList(ref htmlTextWriter, toolDropDownList);

                // Assert
                htmlTextWriter.ShouldSatisfyAllConditions(
                    () => htmlTextWriter.Indent.ShouldBe(indentValue),
                    () => htmlTextWriter.NewLine.ShouldBeNull());
            }
        }

        protected void SetUp(string param)
        {
            _shims = ShimsContext.Create();
            var toolItem = new ToolItem(imageDir, imageDir);
            var toolItemCollection = new ToolItemCollection();
            toolItemCollection.Insert(0, toolItem);
            ShimToolBase.AllInstances.BackImageGet = (x) => image;
            if (param.Equals(negativeIndex))
            {
                ShimToolDropDownList.AllInstances.SelectedIndexGet = (x) => -1;
                ShimToolDropDownList.AllInstances.TextGet = (x) => imageDir;
                ShimWebControl.AllInstances.WidthGet = (x) => new Unit(value);
                ShimToolBase.AllInstances.DropDownImageGet = (x) => image;
            }
            else if (param.Equals(indexText))
            {
                ShimToolDropDownList.AllInstances.SelectedIndexGet = (x) => 0;
                ShimToolDropDownList.AllInstances.ChangeToSelectedTextGet = (x) => SelectedText.Text;
                ShimToolBase.AllInstances.ItemsGet = (x) => toolItemCollection;
                ShimWebControl.AllInstances.WidthGet = (x) => Unit.Empty;
                ShimToolBase.AllInstances.DropDownImageGet = (x) => image;
            }
            else if (param.Equals(indexValue))
            {
                ShimToolDropDownList.AllInstances.SelectedIndexGet = (x) => 0;
                ShimToolDropDownList.AllInstances.ChangeToSelectedTextGet = (x) => SelectedText.Value;
                ShimToolBase.AllInstances.ItemsGet = (x) => toolItemCollection;
                ShimWebControl.AllInstances.WidthGet = (x) => Unit.Empty;
                ShimToolBase.AllInstances.DropDownImageGet = (x) => string.Empty;
            }
            ShimWebControl.AllInstances.HeightGet = (x) => new Unit(value);
            ShimWebControl.AllInstances.ForeColorGet = (x) => Color.AliceBlue;
        }
    }
}
