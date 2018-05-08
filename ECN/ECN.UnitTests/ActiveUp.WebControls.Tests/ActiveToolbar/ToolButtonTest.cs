using System;
using System.Drawing;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ActiveUp.WebControls.Fakes;
using ActiveUp.WebControls.Common.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveToolbar
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ToolButtonTest
    {
        private ToolButton _toolButton;
        private PrivateObject _privateObject;
        private IDisposable _shims;
        private const string DummyBackImageClickedText = "DummyBackImageClickedText";
        private const string Image = "image";
        private const string Text = "text";
        private const string PopUpFalse = "x";
        private const string PopUpTrue = "y";
        private const string TextAlignRight = "z";
        private const string MethodRenderImage = "RenderImage";

        static object[] PropertyNameWithDefaultValue =
        {
            new object[] {"BorderWidthRollOver", Unit.Empty},
            new object[] {"BackColorRollOver", Color.Empty},
            new object[] {"BackColorClicked", Color.Empty},
            new object[] {"BackImageClicked", string.Empty}
        };

        static object[] PropertyNameWithExpectedValue =
        {
            new object[] {"BorderWidthRollOver", Unit.Pixel(1)},
            new object[] {"BackColorRollOver", Color.Black},
            new object[] {"BackColorClicked", Color.Black},
            new object[] {"BackImageClicked", DummyBackImageClickedText}
        };

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
        }

        [Test]
        [TestCaseSource("PropertyNameWithDefaultValue")]
        public void Property_SetAndGetValue_ReturnsDefaultValue(string propertyName, object expected)
        {
            // Arrange:
            var toolButton = new ToolButton();
            var privateObject = new PrivateObject(toolButton);

            // Act: // Assert:
            privateObject.GetFieldOrProperty(propertyName).ShouldBe(expected);
        }

        [Test]
        [TestCaseSource("PropertyNameWithExpectedValue")]
        public void Property_SetAndGetValue_ReturnsTheSetValue(string propertyName, object expected)
        {
            // Arrange:
            var toolButton = new ToolButton();
            var privateObject = new PrivateObject(toolButton);

            // Act:
            privateObject.SetFieldOrProperty(propertyName, expected);

            // Assert:
            privateObject.GetFieldOrProperty(propertyName).ShouldBe(expected);
        }

        [Test]
        public void OnPreRender_ForEqualBorderWidth_PreRender()
        {
            // Arrange
            _toolButton = new ToolButton();
            _privateObject = new PrivateObject(_toolButton);
            SetUp("equal");

            // Act
            _privateObject.Invoke("OnPreRender", new EventArgs());

            // Assert
            _toolButton.ShouldSatisfyAllConditions(
                () => _toolButton.BorderColorRollOver.ShouldBe(Color.Aqua),
                () => _toolButton.BackImageRollOver.ShouldBe("roll"));
        }

        [Test]
        public void OnPreRender_ForHigherBorderWidth_PreRender()
        {
            // Arrange
            _toolButton = new ToolButton();
            _privateObject = new PrivateObject(_toolButton);
            SetUp("high");

            // Act
            _privateObject.Invoke("OnPreRender", new EventArgs());

            // Assert
            _toolButton.ShouldSatisfyAllConditions(
                () => _toolButton.BorderColorRollOver.ShouldBe(Color.Aqua),
                () => _toolButton.BackImageRollOver.ShouldBe("roll"));
        }

        [Test]
        public void OnPreRender_ForLowBorderWidth_PreRender()
        {
            // Arrange
            _toolButton = new ToolButton();
            _privateObject = new PrivateObject(_toolButton);
            SetUp("low");

            // Act
            _privateObject.Invoke("OnPreRender", new EventArgs());

            // Assert
            _toolButton.ShouldSatisfyAllConditions(
                () => _toolButton.BorderColorRollOver.ShouldBe(Color.Aqua),
                () => _toolButton.BackImageRollOver.ShouldBe("roll"));
        }

        [Test]
        public void RenderImage_ForUsePopUpFalse_ShouldRender()
        {
            // Arrange
            var textWriter = new Moq.Mock<TextWriter>();
            using (var htmlTextWriter = new HtmlTextWriter(textWriter.Object))
            {
                _toolButton = new ToolButton();
                _privateObject = new PrivateObject(_toolButton);
                RenderImageSetUp(PopUpFalse);

                // Act
                _privateObject.Invoke(MethodRenderImage, htmlTextWriter);

                // Assert
                htmlTextWriter.ShouldSatisfyAllConditions(
                    () => htmlTextWriter.Indent.ShouldBe(0),
                    () => htmlTextWriter.NewLine.ShouldBeNull());
            } 
        }

        [Test]
        public void RenderImage_ForUsePopUpTrue_ShouldRender()
        {
            // Arrange
            var textWriter = new Moq.Mock<TextWriter>();
            using (var htmlTextWriter = new HtmlTextWriter(textWriter.Object))
            {
                _toolButton = new ToolButton();
                _privateObject = new PrivateObject(_toolButton);
                RenderImageSetUp(PopUpTrue);

                // Act
                _privateObject.Invoke(MethodRenderImage, htmlTextWriter);

                // Assert
                htmlTextWriter.ShouldSatisfyAllConditions(
                    () => htmlTextWriter.Indent.ShouldBe(0),
                    () => htmlTextWriter.NewLine.ShouldBeNull());
            }
        }

        [Test]
        public void RenderImage_ForUsePopUpTrueAndAlignRight_ShouldRender()
        {
            // Arrange
            var textWriter = new Moq.Mock<TextWriter>();
            using (var htmlTextWriter = new HtmlTextWriter(textWriter.Object))
            {
                _toolButton = new ToolButton();
                _privateObject = new PrivateObject(_toolButton);
                RenderImageSetUp(TextAlignRight);

                // Act
                _privateObject.Invoke(MethodRenderImage, htmlTextWriter);

                // Assert
                htmlTextWriter.ShouldSatisfyAllConditions(
                    () => htmlTextWriter.Indent.ShouldBe(0),
                    () => htmlTextWriter.NewLine.ShouldBeNull());
            }
        }

        protected void SetUp(String param)
        {
            _shims = ShimsContext.Create();
            var bag = new StateBag();
            bag.Add("_type", ToolButtonType.Checkbox);
            ShimToolButton.AllInstances.GroupNameGet = (x) => "name";
            ShimControl.AllInstances.PageGet = (x) => new Page();
            ShimControl.AllInstances.ViewStateGet = (x) => bag;
            if (param.Equals("low"))
            {
                ShimToolBase.AllInstances.BorderWidthGet = (x) => new Unit("4");
            }
            else if (param.Equals("high"))
            {
                ShimToolBase.AllInstances.BorderWidthGet = (x) => new Unit("6");
            }
            else if (param.Equals("equal"))
            {
                ShimToolBase.AllInstances.BorderWidthGet = (x) => new Unit("5");
            }
            ShimToolBase.AllInstances.BorderWidthRollOverGet = (x) => new Unit("5");
            ShimWebControl.AllInstances.BackColorGet = (x) => Color.AliceBlue;
            ShimWebControl.AllInstances.BorderColorGet = (x) => Color.AliceBlue;
            ShimToolBase.AllInstances.BackColorClickedGet = (x) => Color.AntiqueWhite;
            ShimToolButton.AllInstances.BorderColorRollOverGet = (x) => Color.Aqua;
            ShimToolBase.AllInstances.BackImageGet = (x) => Image;
            ShimCoreWebControl.AllInstances.ImagesDirectoryGet = (x) => "http://image";
            ShimToolBase.AllInstances.BackImageClickedGet = (x) => "backImage";
            ShimToolBase.AllInstances.AllowRollOverGet = (x) => true;
            ShimToolButton.AllInstances.BackImageRollOverGet = (x) => "roll";
            ShimToolBase.AllInstances.BackColorRollOverGet = (x) => Color.AliceBlue;

        }

        protected void RenderImageSetUp(string param)
        {
            _shims = ShimsContext.Create();
            ShimToolButton.AllInstances.TypeGet = (x) => ToolButtonType.Checkbox;
            ShimToolBase.AllInstances.AllowRollOverGet = (x) => true;
            if (param.Equals(PopUpFalse))
            {
                ShimToolButton.AllInstances.UsePopupOnClickGet = (x) => false;
                ShimToolBase.AllInstances.TextGet = (x) => Text;
                ShimToolButton.AllInstances.TextAlignGet = (x) => TextAlign.Left;
            }
            else if (param.Equals(PopUpTrue))
            {
                ShimToolButton.AllInstances.UsePopupOnClickGet = (x) => true;
                ShimToolBase.AllInstances.BackImageClickedGet = (x) => Image;
                ShimToolBase.AllInstances.BackImageGet = (x) => Image;
                ShimToolBase.AllInstances.TextGet = (x) => string.Empty;
                ShimToolButton.AllInstances.OverImageURLGet = (x) => Image;
                ShimToolButton.AllInstances.TextAlignGet = (x) => TextAlign.Right;
            }
            else if (param.Equals(TextAlignRight))
            {
                ShimToolButton.AllInstances.UsePopupOnClickGet = (x) => false;
                ShimToolBase.AllInstances.TextGet = (x) => Text;
                ShimToolButton.AllInstances.TextAlignGet = (x) => TextAlign.Right;
            }
            ShimToolBase.AllInstances.ClientSideClickGet = (x) => Text;
            ShimToolButton.AllInstances.CheckedGet = (x) => true;
            ShimToolBase.AllInstances.BackColorClickedGet = (x) => Color.SaddleBrown;
            ShimToolButton.AllInstances.ImageURLGet = (x) => Image;
            ShimToolButton.AllInstances.NavigateURLGet = (x) => Image;
            ShimToolButton.AllInstances.TargetGet = (x) => Image;
        }
    }
}
