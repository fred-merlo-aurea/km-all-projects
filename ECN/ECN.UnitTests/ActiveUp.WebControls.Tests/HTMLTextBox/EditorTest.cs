using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web.Configuration.Fakes;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using ActiveUp.WebControls.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class EditorTest
	{
		private const string TestValue = "Test1";
	    private const string DummyString = "DummyString";
        private const string MethodRenderEditor = "RenderEditor";
        private Editor _editor;
	    private PrivateObject _privateObject;
	    private IDisposable _shimObject;

	    [SetUp]
	    public void SetUp()
	    {
	        _shimObject = ShimsContext.Create();
            _editor = new Editor();
	        _privateObject = new PrivateObject(_editor);
	    }

        [TearDown]
	    public void TearDown()
	    {
	        _shimObject.Dispose();
	    }

        [Test]
		public void IconsDirectory_DefaultValue()
		{
			// Arrange
			using (var testObject = new Editor())
			{
				// Act, Assert
				AssertNotFX1(string.Empty, testObject.IconsDirectory);
				AssertFX1(Define.IMAGES_DIRECTORY, testObject.IconsDirectory);
			}
		}

		[Test]
		public void IconsDirectory_SetAndGetValue()
		{
			// Arrange
			using (var testObject = new Editor())
			{
				// Act
				testObject.IconsDirectory = TestValue;

				// Assert
				testObject.IconsDirectory.ShouldBe(TestValue);
			}
		}

		[Test]
		public void ScriptDirectory_DefaultValue_ReturnsEmptyStringOrDefinesScriptDirectory()
		{
			// Arrange
			using (var testObject = new Editor())
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
			using (var testObject = new Editor())
			{
				// Act
				testObject.ScriptDirectory = TestValue;

				// Assert
				testObject.ScriptDirectory.ShouldBe(TestValue);
			}
		}

		[Test]
		public void EditorModeDesignIcon_DefaultValue()
		{
			// Arrange
			using (var testObject = new Editor())
			{
				// Act, Assert
				AssertNotFX1(string.Empty, testObject.EditorModeDesignIcon);
				AssertFX1(Define.IMAGES_DIRECTORY, testObject.EditorModeDesignIcon);
			}
		}

		[Test]
		public void EditorModeDesignIcon_SetAndGetValue()
		{
			// Arrange
			using (var testObject = new Editor())
			{
				// Act
				testObject.EditorModeDesignIcon = TestValue;

				// Assert
				testObject.EditorModeDesignIcon.ShouldBe(TestValue);
			}
		}

		[Test]
		public void BorderColor_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new Editor())
			{
				// Act
				testObject.BorderColor = System.Drawing.Color.Yellow;

				// Assert
				testObject.BorderColor.ShouldBe(System.Drawing.Color.Yellow);
			}
		}

		[Test]
		public void BorderWidth_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new Editor())
			{
				// Act
				testObject.BorderWidth = 5;

				// Assert
				testObject.BorderWidth.ShouldBe(5);
			}
		}

	    [Test]
        [TestCase("IE")]
	    [TestCase("")]
        public void RenderEditor_DefaultValues_RenderHTMLTextWriter(string browser)
	    {
	        // Arrange
	        var stringWriter = new StringWriter();
	        var outputTextWriter = new HtmlTextWriter(stringWriter);
	        _editor.Page = GetMockOfPage(browser);
	        _editor.Style.Add("Width", "100px");
	        _editor.ToolTip = DummyString;
	        _editor.EnableSsl = true;
	        _editor.AllowTransparency = true;
	        _editor.Text = DummyString;
	        _editor.Editable = false;
            const string HiddenInput = "<input";
	        const string WidthValue = "100px";

            // Act	
            _privateObject.Invoke(MethodRenderEditor, new object[] { outputTextWriter });
	        var actualResult = stringWriter.ToString();

	        // Assert
	        actualResult.ShouldSatisfyAllConditions(
	            () => actualResult.ShouldNotBeNullOrWhiteSpace(),
	            () => actualResult.ShouldContain(WidthValue),
	            () => actualResult.ShouldContain(DummyString),
                () => actualResult.ShouldContain(HiddenInput)
	        );
	    }

	    [Test]
        [TestCase(EditorModeSelectorType.CheckBox, -1)]
	    [TestCase(EditorModeSelectorType.Tabs, 10)]
        public void RenderEditor_MultipleValues_RenderHTMLTextWriter(EditorModeSelectorType editorModeSelector, int maxLength)
	    {
	        // Arrange
	        var stringWriter = new StringWriter();
	        var outputTextWriter = new HtmlTextWriter(stringWriter);
	        _editor.Page = GetMockOfPage(String.Empty);
            _privateObject.SetField("_toolbarsContainer", new ToolbarsContainer
	        {
                Toolbars =
                {
                    new Toolbar
                    {
                        Tools =
                        {
                            new ToolButton
                            {
                                ImageURL = "ImageURL",
                                OverImageURL = "OverImageURL"
                            }
                        }
                    }
                }
            });
	        _editor.Text = DummyString;
	        _editor.EditorModeSelector = editorModeSelector;
	        _editor.StartupMode = EditorMode.Html;
	        _editor.MaxLength = maxLength;
	        _editor.Editable = true;
            _privateObject.SetProperty("ShowNavigation", true);
            const string HiddenInput = "<input";

	        // Act	
	        _privateObject.Invoke(MethodRenderEditor, new object[] { outputTextWriter });
	        var actualResult = stringWriter.ToString();

	        // Assert
	        actualResult.ShouldSatisfyAllConditions(
	            () => actualResult.ShouldNotBeNullOrWhiteSpace(),
	            () => actualResult.ShouldContain(DummyString),
	            () => actualResult.ShouldContain(HiddenInput)
	        );
	    }

        [Test]
	    public void RenderEditor_EditorTypeTextArea_RenderHTMLTextWriter()
	    {
	        // Arrange
	        var stringWriter = new StringWriter();
	        var outputTextWriter = new HtmlTextWriter(stringWriter);
	        _editor.Page = GetMockOfPage(String.Empty);	      
	        const string HiddenInput = "<input";
            _editor.TextareaColumns = DummyString;
	        _editor.TextareaCssClass = DummyString;
            ShimEditor.AllInstances.DetermineEditorType = (obj) => EditorType.TextArea;

            // Act	
            _privateObject.Invoke(MethodRenderEditor, new object[] { outputTextWriter });
	        var actualResult = stringWriter.ToString();

	        // Assert
	        actualResult.ShouldSatisfyAllConditions(
	            () => actualResult.ShouldNotBeNullOrWhiteSpace(),
	            () => actualResult.ShouldContain(HiddenInput)
	        );
	    }

        [TestCase(true)]
        [TestCase(false)]
        public void AutoDetectSsl_SetValue_ReturnsSetValue(bool value)
        {
            // Arrange
            using (var testObject = new Editor())
            {
                // Act
                testObject.AutoDetectSsl = value;

                // Assert
                testObject.AutoDetectSsl.ShouldBe(value);
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void EnableSsl_SetValue_ReturnsSetValue(bool value)
        {
            // Arrange
            using (var testObject = new Editor())
            {
                // Act
                testObject.EnableSsl = value;

                // Assert
                testObject.EnableSsl.ShouldBe(value);
            }
        }

        private ShimPage GetMockOfPage(string browser)
	    {
	        var shimPage = new ShimPage();
	        var shimHttpRequest = new ShimHttpRequest();
	        var shimHttpBrowserCapabilities = new ShimHttpBrowserCapabilities();
	        ShimHttpCapabilitiesBase.AllInstances.BrowserGet = (obj) => browser;
	        ShimHttpRequest.AllInstances.BrowserGet = (obj) => shimHttpBrowserCapabilities.Instance;
	        ShimPage.AllInstances.RequestGet = (obj) => shimHttpRequest.Instance;
	        ShimPage.AllInstances.ClientScriptGet = (obj) => new ShimClientScriptManager().Instance;
	        ShimClientScriptManager.AllInstances.GetWebResourceUrlTypeString = (obj, type, url) => DummyString;
	        return shimPage;
	    }
    }
}
