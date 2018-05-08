using System.Web.UI;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;

namespace ActiveUp.WebControls.Tests.ActiveToolbar
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class ToolbarTest
	{
		private const string TestValue = "Test1";
		private const string Scriptkey = "ActiveToolbar";
		private const string ScriptKeyPostFix = "_startup";
		private const string DummyScriptDirectory = @"C://temp";
		private const string DummyScript = "<script></script>";
		private const string ImageDock = "Dock.gif";
		private const string _MethodRenderToolbar = "RenderToolbar";
		private Toolbar _toolbar;
		private MSTest::PrivateObject _privateObject;

		[SetUp]
		public void SetUp()
		{
			_toolbar = new Toolbar();
			_privateObject = new MSTest::PrivateObject(_toolbar);
		}

		[Test]
		public void RegisterAPIScriptBlock_Toolbar_ScriptRegistered()
		{
			//Arrange:
			var toolbar = new Mock<Toolbar>();
			var dummyPage = new Page();

			dummyPage.RegisterClientScriptBlock(Scriptkey, DummyScript);

			toolbar.Setup(x => x.Page).Returns(dummyPage);
			toolbar.SetupGet(x => x.ScriptDirectory).Returns(DummyScriptDirectory);

			//Act:
			toolbar.Object.RegisterAPIScriptBlock();

			// Assert:
			MSTest::Assert.IsTrue(dummyPage.IsClientScriptBlockRegistered(string.Concat(Scriptkey, ScriptKeyPostFix)));
		}

		[Test]
		public void RenderToolbar_OnValidCall_WriteToTextWriter()
		{
			// Arrange
			var stringWriter = new StringWriter();
			var outputTextWriter = new HtmlTextWriter(stringWriter);
			_privateObject.SetProperty("Dragable", true);
			_toolbar.Style.Add("left", "100px");
			_toolbar.Style.Add("top", "100px");
			_toolbar.Style.Add("overflow", "auto");
			const string iframe = "<iframe";
			const string hiddenInput = "<input type=\"hidden\"";

			// Act	
			_privateObject.Invoke(_MethodRenderToolbar, new object[] { outputTextWriter });
			var actualResult = stringWriter.ToString();

			// Assert
			actualResult.ShouldNotBeNullOrWhiteSpace();
			actualResult.ShouldContain(iframe);
			actualResult.ShouldContain(hiddenInput);
		}

		[Test]
		public void RenderToolbar_DirectionVerticalAndToolsNotEmpty_WriteToTextWriter()
		{
			// Arrange
			var stringWriter = new StringWriter();
			var outputTextWriter = new HtmlTextWriter(stringWriter);
			_privateObject.SetProperty("Dragable", true);
			_toolbar.Style.Add("left", "100px");
			_toolbar.Style.Add("top", "100px");
			_toolbar.Style.Add("overflow", "auto");
			_toolbar.Direction = ToolbarDirection.Vertical;
			_toolbar.BackImage = "Image";
			_toolbar.Tools.Add(new ToolBold
			{
				EnableSsl = true
			});
			const string iframe = "<iframe";
			const string hiddenInput = "<input type=\"hidden\"";

			// Act	
			_privateObject.Invoke(_MethodRenderToolbar, new object[] { outputTextWriter });
			var actualResult = stringWriter.ToString();

			// Assert
			actualResult.ShouldNotBeNullOrWhiteSpace();
			actualResult.ShouldContain(iframe);
			actualResult.ShouldContain(hiddenInput);
		}

		[Test]
		public void ImagesDirectory_DefaultValue_ReturnsEmptyStringOrImagesDirectory()
		{
			// Arrange
			using (var testObject = new Toolbar())
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
			using (var testObject = new Toolbar())
			{
				// Act
				testObject.ImagesDirectory = TestValue;

				// Assert
				testObject.ImagesDirectory.ShouldBe(TestValue);
			}
		}

		[Test]
		public void DragAndDropImage_DefaultValue_ReturnsEmptyStringOrImageDock()
		{
			// Arrange
			using (var testObject = new Toolbar())
			{
				// Act, Assert
				AssertNotFX1(string.Empty, testObject.DragAndDropImage);
				AssertFX1(ImageDock, testObject.DragAndDropImage);
			}
		}

		[Test]
		public void DragAndDropImage_SetAndGetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new Toolbar())
			{
				// Act
				testObject.DragAndDropImage = TestValue;

				// Assert
				testObject.DragAndDropImage.ShouldBe(TestValue);
			}
		}
		
		[Test]
		public void EnableSsl_SetAndGetValue_ReturnsDefaultValue()
		{
			// Arrange
			var toolbar = new Toolbar();

			// Act, Assert
			toolbar.EnableSsl.ShouldBeFalse();
		}

		[Test]
		public void EnableSsl_SetAndGetValue_ReturnsSetValue()
		{
			// Arrange
			var toolbar = new Toolbar();

			// Act
			toolbar.EnableSsl = true;

			// Assert
			toolbar.EnableSsl.ShouldBeTrue();
		}
	}
}