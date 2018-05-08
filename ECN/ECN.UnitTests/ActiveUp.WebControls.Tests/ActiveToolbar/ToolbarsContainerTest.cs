using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using Microsoft.QualityTools.Testing.Fakes;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Moq;
using Shouldly;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;

namespace ActiveUp.WebControls.Tests.ActiveToolbar
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ToolbarsContainerTest
    {
        private const string TestValue = "Test1";
        private const string Scriptkey = "ActiveToolbar";
        private const string ScriptKeyPostFix = "_startup";
        private const string DummyScriptDirectory = @"C://temp";
        private const string DummyScript = "<script></script>";
        private const string DummyToolbarId = "DummyToolbarId";
        private IDisposable _shimObject;
        private ToolbarsContainer _toolbarsContainer;
        private MSTest::PrivateObject _privateObj;
        private const string _MethodRenderToolbarsContainer = "RenderToolbarsContainer";

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
            _toolbarsContainer = new ToolbarsContainer();
            _privateObj = new MSTest::PrivateObject(_toolbarsContainer);
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void RenderToolbarsContainer_DefaultValues_RenderHTMLTextWriter()
        {
            // Arrange
            var stringWriter = new StringWriter();
            var outputTextWriter = new HtmlTextWriter(stringWriter);
            const string HiddenInput = "<input type=\"hidden\" value=\"\" id=\"_toolbars\" name=\"_toolbars\" />";
            const string Width = "width:100%";
            const string PositionValue = "position:Absolute";

            // Act	
            _privateObj.Invoke(_MethodRenderToolbarsContainer, new object[] { outputTextWriter });
            var actualResult = stringWriter.ToString();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrWhiteSpace(),
                () => actualResult.ShouldContain(PositionValue),
                () => actualResult.ShouldContain(HiddenInput),
                () => actualResult.ShouldContain(Width)
            );
        }

        [Test]
        public void RenderToolbarsContainer_ToolbarsPositionAbsolute_RenderHTMLTextWriter()
        {
            // Arrange
            var stringWriter = new StringWriter();
            var outputTextWriter = new HtmlTextWriter(stringWriter);
            _toolbarsContainer.BackImage = "TestDirectory";
            _toolbarsContainer.BorderWidth = new Unit(100);
            _toolbarsContainer.BorderStyle = BorderStyle.Dashed;
            _toolbarsContainer.Height = new Unit(100);
            _toolbarsContainer.Width = new Unit(100);
            _toolbarsContainer.Style.Add("left", "100px");
            _toolbarsContainer.Style.Add("top", "100px");
            const string HiddenInput = "<input type=\"hidden\" value=\"\" id=\"_toolbars\" name=\"_toolbars\" />";
            const string BorderWidth = "order-width:100px";
            const string Width = "width:100px";
            const string Height = "height:100px";
            const string BorderStyleValue = "border-style:Dashed";
            const string PositionValue = "position:Absolute";

            // Act	
            _privateObj.Invoke(_MethodRenderToolbarsContainer, new object[] { outputTextWriter });
            var actualResult = stringWriter.ToString();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrWhiteSpace(),
                () => actualResult.ShouldContain(PositionValue),
                () => actualResult.ShouldContain(HiddenInput),
                () => actualResult.ShouldContain(BorderWidth),
                () => actualResult.ShouldContain(Width),
                () => actualResult.ShouldContain(Height),
                () => actualResult.ShouldContain(BorderStyleValue)
            );
        }

        [Test]
        [TestCase(Position.Absolute)]
        [TestCase(Position.Relative)]
        public void RenderToolbarsContainer_InternalToolBarsIDNotEmpty_RenderHTMLTextWriter(Position toolbarsPosition)
        {
            // Arrange
            _privateObj.SetProperty("InternalToolBarsID", "1,2");
            var shimPage = new ShimPage();
            ShimPage.AllInstances.FindControlString = (obj, id) =>
            {
                return new Toolbar();
            };
            _privateObj.SetProperty("Page", shimPage.Instance);
            _toolbarsContainer.ToolbarsPosition = toolbarsPosition;
            var stringWriter = new StringWriter();
            var outputTextWriter = new HtmlTextWriter(stringWriter);
            const string HiddenInput = "<input type=\"hidden\" value=\"\" id=\"_toolbars\" name=\"_toolbars\" />";
            const string Width = "width:100%";
            const string PositionValue = "position:Absolute";

            // Act	
            _privateObj.Invoke(_MethodRenderToolbarsContainer, new object[] { outputTextWriter });
            var actualResult = stringWriter.ToString();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrWhiteSpace(),
                () => actualResult.ShouldContain(PositionValue),
                () => actualResult.ShouldContain(HiddenInput),
                () => actualResult.ShouldContain(Width)
            );
        }

        [Test]
        public void RenderToolbarsContainer_ToolbarsNotEmpty_RenderHTMLTextWriter()
        {
            // Arrange
            const string ToolbarId = "toolbar-1";
            _privateObj.SetProperty("InternalToolBarsID", "");
            _privateObj.SetField("_toolbars", new ToolbarCollection(_toolbarsContainer.Controls)
            {
                new Toolbar
                {
                    ID = ToolbarId
                }
            });
            _toolbarsContainer.ToolbarsPosition = Position.Relative;
            var stringWriter = new StringWriter();
            var outputTextWriter = new HtmlTextWriter(stringWriter);
            const string HiddenInput = "<input type=\"hidden\" value=\"\" id=\"_toolbars\" name=\"_toolbars\" />";
            const string Width = "width:100%";
            const string PositionValue = "position:Absolute";

            // Act	
            _privateObj.Invoke(_MethodRenderToolbarsContainer, new object[] { outputTextWriter });
            var actualResult = stringWriter.ToString();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrWhiteSpace(),
                () => actualResult.ShouldContain(PositionValue),
                () => actualResult.ShouldContain(HiddenInput),
                () => actualResult.ShouldContain(Width),
                () => actualResult.ShouldContain(ToolbarId)
            );
        }

        [Test]
        public void ImagesDirectory_DefaultValue()
        {
            // Arrange
            var toolBarsContainer = new ToolbarsContainer();

            // Act, Assert
            AssertNotFX1(string.Empty, toolBarsContainer.ImagesDirectory);
            AssertFX1(Define.IMAGES_DIRECTORY, toolBarsContainer.ImagesDirectory);
        }

        [Test]
        public void ImagesDirectory_SetAndGetValue()
        {
            // Arrange
            var toolBarsContainer = new ToolbarsContainer();

            // Act
            toolBarsContainer.ImagesDirectory = "Test1";

            // Assert
            Assert.AreEqual("Test1", toolBarsContainer.ImagesDirectory);
        }

        [Test]
        public void RegisterAPIScriptBlock_ScriptRegistered()
        {
            //Arrange:
            var toolbarsContainer = new Mock<ToolbarsContainer>();
            var dummyPage = new Page();

            dummyPage.RegisterClientScriptBlock(Scriptkey, DummyScript);
            dummyPage.RegisterClientScriptBlock(string.Concat(Scriptkey, "_", DummyToolbarId),
                DummyScript);

            toolbarsContainer.Setup(x => x.ClientID).Returns(DummyToolbarId);
            toolbarsContainer.Setup(x => x.Page).Returns(dummyPage);
            toolbarsContainer.SetupGet(x => x.ScriptDirectory).Returns(DummyScriptDirectory);

            //Act:
            toolbarsContainer.Object.RegisterAPIScriptBlock();

            // Assert:
            MSTest::Assert.IsTrue(dummyPage.IsClientScriptBlockRegistered(string.Concat(Scriptkey, ScriptKeyPostFix)));
        }

        [Test]
        public void ImagesDirectory_DefaultValue_ReturnsEmptyStringOrImagesDirectory()
        {
            // Arrange
            using (var testObject = new ToolbarsContainer())
            {
                // Act, Assert
                AssertNotFX1(string.Empty, testObject.ImagesDirectory);
                AssertFX1(Define.IMAGES_DIRECTORY, testObject.ImagesDirectory);
            }
        }

        [Test]
        public void ImagesDirectory_SetAndGetValue_ReturnsSetValue()
        {
            // Arrange
            using (var testObject = new ToolbarsContainer())
            {
                // Act
                testObject.ImagesDirectory = TestValue;

                // Assert
                testObject.ImagesDirectory.ShouldBe(TestValue);
            }
        }

        [Test]
        public void EnableSsl_DefaultValue_ReturnsFalse()
        {
            // Arrange
            using (var testObject = new ToolbarsContainer())
            {
                // Act, Assert
                testObject.EnableSsl.ShouldBeFalse();
            }
        }

        [Test]
        public void EnableSsl_SetAndGetValue_ReturnsTrue()
        {
            // Arrange
            using (var testObject = new ToolbarsContainer())
            {
                // Act
                testObject.EnableSsl = true;

                // Assert
                testObject.EnableSsl.ShouldBeTrue();
            }
        }

        [Test]
        public void EnableSsl_SetAndGetValue_ReturnsFalse()
        {
            // Arrange
            using (var testObject = new ToolbarsContainer())
            {
                // Act
                testObject.EnableSsl = false;

                // Assert
                testObject.EnableSsl.ShouldBeFalse();
            }
        }

		[Test]
		public void BackImage_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			using (var testObject = new ToolbarsContainer())
			{
				// Assert
				testObject.BackImage.ShouldBeEmpty();
			}
		}

		[Test]
		public void BackImage_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new ToolbarsContainer())
			{
				// Act
				testObject.BackImage = TestValue;

				// Assert
				testObject.BackImage.ShouldBe(TestValue);
			}
		}
	}
}
