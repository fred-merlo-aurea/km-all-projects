using System;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using System.Web.UI.WebControls;
using static System.Web.HttpUtility;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;

namespace ActiveUp.WebControls.Tests.ActiveInput
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SliderTest
    {
        private const string TestValue = "Test1";
        private const string _DummyString = "DummyString";
        private const string TdWithPlusArrowRenderedHtml =
            "<td><img id=\"_plusArrow\" name=\"_plusArrow\" src=\"\" border=\"0\" " +
            "onmousedown=\"ASD_PlusArrowDown('');\" onmouseout=\"ASD_StopStepPlus('');\" " +
            "onmouseup=\"ASD_StopStepPlus('');\" /></td>";
        private const string _MethodRenderSlider = "RenderSlider";
        private Slider _slider;
        private MSTest::PrivateObject _PrivateObject;
        private const string ArrowUpOff = "ArrowUpOff.gif";
        private const string ArrowDownOff = "ArrowDownOff.gif";
        private const string LiftCenterOff = "ButtonVertCenterOff.gif";
        private const string LiftOn = "BackgroundVertOn.gif";
        private const string LiftEndOff = "ButtonVertEndOff.gif";

        [SetUp]
        public void SetUp()
        {
            _slider = new Slider();
            _PrivateObject = new MSTest::PrivateObject(_slider);
        }

        [Test]
        public void RenderTdWithPlusArrowImage_GetRenderedHtml()
        {
            //Arrange:
            var stringWriter = new StringWriter();
            var writer = new HtmlTextWriter(stringWriter);

            var slider = new Slider();
            var privateObject = new MSTest::PrivateObject(slider);

            //Act:
            privateObject.Invoke("RenderTdWithPlusArrowImage", writer);
            var result = stringWriter.ToString();

            //Assert:
            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
            Assert.AreEqual(HtmlDecode(TdWithPlusArrowRenderedHtml), HtmlDecode(result));
        }

        [Test]
        public void RenderSlider_OnValidCall_RenderHtmlTextWriter()
        {
            // Arrange
            var stringWriter = new StringWriter();
            var outputTextWriter = new HtmlTextWriter(stringWriter);
            const string HiddenInput = "<input type=\"hidden\"";
            _slider.BorderStyle = BorderStyle.Dotted;

            // Act	
            _PrivateObject.Invoke(_MethodRenderSlider, new object[] { outputTextWriter });
            var actualResult = stringWriter.ToString();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrWhiteSpace(),
                () => actualResult.ShouldContain(HiddenInput)
            );
        }

        [Test]
        public void RenderSlider_NotDefaultValues_RenderHtmlTextWriter()
        {
            // Arrange
            var stringWriter = new StringWriter();
            var outputTextWriter = new HtmlTextWriter(stringWriter);
            const string HiddenInput = "<input type=\"hidden\"";
            _slider.CssClass = _DummyString;
            _PrivateObject.SetField("_direction", DirectionLift.Horizontal);

            // Act	
            _PrivateObject.Invoke(_MethodRenderSlider, new object[] { outputTextWriter });
            var actualResult = stringWriter.ToString();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrWhiteSpace(),
                () => actualResult.ShouldContain(HiddenInput)
            );
        }

        public void ScriptDirectory_DefaultValue_ReturnsEmptyStringOrDefinesValue()
        {
            // Arrange
            using (var testObject = new Slider())
            {
                // Act, Assert
                AssertNotFX1(string.Empty, testObject.ScriptDirectory);
                AssertFX1(Define.SCRIPT_DIRECTORY, testObject.ScriptDirectory);
            }
        }

        [Test]
        public void ScriptDirectory_SetValue_ReturnsSetValue()
        {
            // Arrange
            using (var testObject = new Slider())
            {
                // Act
                testObject.ScriptDirectory = TestValue;

                // Assert
                testObject.ScriptDirectory.ShouldBe(TestValue);
            }
        }

        [Test]
        public void PlusArrowOffImage_DefaultValue_ReturnsEmptyStringOrPlusArrowOffImage()
        {
            // Arrange
            using (var testObject = new Slider())
            {
                // Act, Assert
                AssertNotFX1(string.Empty, testObject.PlusArrowOffImage);
                AssertFX1(ArrowDownOff, testObject.PlusArrowOffImage);
            }
        }

        [Test]
        public void PlusArrowOffImage_SetValue_ReturnsSetValue()
        {
            // Arrange
            using (var testObject = new Slider())
            {
                // Act
                testObject.PlusArrowOffImage = TestValue;

                // Assert
                testObject.PlusArrowOffImage.ShouldBe(TestValue);
            }
        }

        [Test]
        public void LiftCenterOff_DefaultValue_ReturnsEmptyStringOrButtonVertCenterOff()
        {
            // Arrange
            using (var testObject = new Slider())
            {
                // Act, Assert
                AssertNotFX1(string.Empty, testObject.LiftCenterOff);
                AssertFX1(LiftCenterOff, testObject.LiftCenterOff);
            }
        }

        [Test]
        public void LiftCenterOff_SetValue_ReturnsSetValue()
        {
            // Arrange
            using (var testObject = new Slider())
            {
                // Act
                testObject.LiftCenterOff = TestValue;

                // Assert
                testObject.LiftCenterOff.ShouldBe(TestValue);
            }
        }

        [Test]
        public void MinusArrowOffImage_DefaultValue_ReturnsEmptyStringOrArrowUpOff()
        {
            // Arrange
            using (var testObject = new Slider())
            {
                // Act, Assert
                AssertNotFX1(string.Empty, testObject.MinusArrowOffImage);
                AssertFX1(ArrowUpOff, testObject.MinusArrowOffImage);
            }
        }

        [Test]
        public void MinusArrowOffImage_SetValue_ReturnsSetValue()
        {
            // Arrange
            using (var testObject = new Slider())
            {
                // Act
                testObject.MinusArrowOffImage = TestValue;

                // Assert
                testObject.MinusArrowOffImage.ShouldBe(TestValue);
            }
        }

        [Test]
        public void LiftOn_DefaultValue_ReturnsEmptyStringOrBackgroundVertOn()
        {
            // Arrange
            using (var testObject = new Slider())
            {
                // Act, Assert
                AssertNotFX1(string.Empty, testObject.LiftOn);
                AssertFX1(LiftOn, testObject.LiftOn);
            }
        }

        [Test]
        public void LiftOn_SetValue_ReturnsSetValue()
        {
            // Arrange
            using (var testObject = new Slider())
            {
                // Act
                testObject.LiftOn = TestValue;

                // Assert
                testObject.LiftOn.ShouldBe(TestValue);
            }
        }

        [Test]
        public void LiftEndOff_DefaultValue_ReturnsEmptyStringOrButtonVertEndOff()
        {
            // Arrange
            using (var testObject = new Slider())
            {
                // Act, Assert
                AssertNotFX1(string.Empty, testObject.LiftEndOff);
                AssertFX1(LiftEndOff, testObject.LiftEndOff);
            }
        }

        [Test]
        public void LiftEndOff_SetValue_ReturnsSetValue()
        {
            // Arrange
            using (var testObject = new Slider())
            {
                // Act
                testObject.LiftEndOff = TestValue;

                // Assert
                testObject.LiftEndOff.ShouldBe(TestValue);
            }
        }
    }
}