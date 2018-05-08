//FX1_1 is defined to test the FX1_1 conditional attribute method
#if !FX1_1
#define FX1_1
#endif

using System;
using System.Drawing;
using System.IO;
using System.Web.Configuration.Fakes;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ActiveUp.WebControls.Common.Extension;
using ActiveUp.WebControls.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;


namespace ActiveUp.WebControls.Tests.ActiveToolbar
{
	[TestFixture]
	public class ToolColorPickerTest
	{
		private const string ActiveToolbarScriptKey = "ActiveToolbar";
		private const string DummyClientSideScript = "DummyClientSideScript";
		private const string DummyUrl = "DummyUrl";
		private const string DummyDropDownImage = @"c\\temp\\test.png";
		private const string DummyIndentText = "DummyIndentText";
		private const string DummyId = "DummyId";
		private const string DummyText = "DummyText";
        private const string DummyString = "DummyString";
        private const string _MethodRender = "Render";
        private const string _InputHidden = "<input type=\"Hidden\"";
        private PrivateObject _privateObject;
        private ToolColorPicker _toolColorPicker;
        private IDisposable _shimObject;
        private StubToolbar stubToolbar;

        private void SetUp()
        {
            _shimObject = ShimsContext.Create();
            _toolColorPicker = new ToolColorPicker();
            _privateObject = new PrivateObject(_toolColorPicker);
            stubToolbar = new StubToolbar();
            stubToolbar.ParentGet = () => null;
            stubToolbar.ImagesDirectory = DummyString;
        }

        private void TearDown()
        {
            _shimObject.Dispose();
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

		[Test]
		public void ViewState_LoadSaveTrack_ReturnsTheSetValue()
		{
			// Arrange:
			//Create the control, start tracking viewstate, then set a new Text value.
			var toolColorPicker = new ToolColorPicker();
			toolColorPicker.EnableSsl = true;
			var privateObject = new PrivateObject(toolColorPicker);
			privateObject.Invoke("TrackViewState");
			toolColorPicker.EnableSsl = false;

			//Save the control's state
			var viewState = privateObject.Invoke("SaveViewState");

			//Create a new control instance and load the state
			//back into it, overriding any existing values
			var colorPicker = new ToolColorPicker();
			toolColorPicker.EnableSsl = true;

			// Act:
			var pickerPrivateObject = new PrivateObject(colorPicker);
			pickerPrivateObject.Invoke("LoadViewState", viewState);

			// Assert:
			colorPicker.EnableSsl.
				ShouldBeFalse("Value restored from viewstate does not match the original value we set");
		}

		[Test]
		public void DropDownImage_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();

			// Act:
			toolColorPicker.DropDownImage = DummyDropDownImage;

			// Assert:
			toolColorPicker.DropDownImage.ShouldBe(DummyDropDownImage);
		}

		[Test]
		public void IndentText_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();
			var privateObject = new PrivateObject(toolColorPicker);

			// Act:
			privateObject.SetFieldOrProperty("IndentText", DummyIndentText);

			// Assert:
			var propertyValue = privateObject.GetFieldOrProperty("IndentText") as string;
			propertyValue.ShouldBe(DummyIndentText);
		}

		[Test]
		public void AutoPostBack_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();

			// Act:
			toolColorPicker.AutoPostBack = true;

			// Assert:
			toolColorPicker.AutoPostBack.ShouldBeTrue();
		}

		[Test]
		public void ChangeToSelectedText_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();
			var privateObject = new PrivateObject(toolColorPicker);

			// Act:
			privateObject.SetFieldOrProperty("ChangeToSelectedText", SelectedText.Text);

			// Assert:
			var propertyValue = (SelectedText)privateObject.GetFieldOrProperty("ChangeToSelectedText");
			propertyValue.ShouldBe(SelectedText.Text);
		}

		[Test]
		public void WindowBorderStyle_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();

			// Act:
			toolColorPicker.WindowBorderStyle = BorderStyle.Solid;

			// Assert:
			toolColorPicker.WindowBorderStyle.ShouldBe(BorderStyle.Solid);
		}

		[Test]
		public void WindowBorderWidth_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();

			// Act:
			toolColorPicker.WindowBorderWidth = Unit.Pixel(1);

			// Assert:
			toolColorPicker.WindowBorderWidth.ShouldBe(Unit.Pixel(1));
		}

		[Test]
		public void ItemBorderColor_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();
			var privateObject = new PrivateObject(toolColorPicker);

			// Act:
			privateObject.SetFieldOrProperty("ItemBorderColor", Color.Black);

			// Assert:
			var propertyValue = (Color)privateObject.GetFieldOrProperty("ItemBorderColor");
			propertyValue.ShouldBe(Color.Black);
		}

		[Test]
		public void ItemBorderColorRollOver_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();
			var privateObject = new PrivateObject(toolColorPicker);

			// Act:
			privateObject.SetFieldOrProperty("ItemBorderColorRollOver", Color.Black);

			// Assert:
			var propertyValue = (Color)privateObject.GetFieldOrProperty("ItemBorderColorRollOver");
			propertyValue.ShouldBe(Color.Black);
		}

		[Test]
		public void ItemAlign_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();
			var privateObject = new PrivateObject(toolColorPicker);

			// Act:
			privateObject.SetFieldOrProperty("ItemAlign", Align.Center);

			// Assert:
			var propertyValue = (Align)privateObject.GetFieldOrProperty("ItemAlign");
			propertyValue.ShouldBe(Align.Center);
		}

		[Test]
		public void BorderColorRollOver_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();

			// Act:
			toolColorPicker.BorderColorRollOver = Color.Black;

			// Assert:
			toolColorPicker.BorderColorRollOver.ShouldBe(Color.Black);
		}

		[Test]
		public void BorderWidth_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();

			// Act:
			toolColorPicker.BorderWidth = Unit.Pixel(1);

			// Assert:
			toolColorPicker.BorderWidth.ShouldBe(Unit.Pixel(1));
		}

		[Test]
		public void BorderStyle_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();

			// Act:
			toolColorPicker.BorderStyle = BorderStyle.Solid;

			// Assert:
			toolColorPicker.BorderStyle.ShouldBe(BorderStyle.Solid);
		}

		[Test]
		public void CellPadding_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();

			// Act:
			toolColorPicker.Cellpadding = Unit.Pixel(1);

			// Assert:
			toolColorPicker.Cellpadding.ShouldBe(Unit.Pixel(1));
		}

		[Test]
		public void CellSpacing_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();

			// Act:
			toolColorPicker.Cellspacing = Unit.Pixel(1);

			// Assert:
			toolColorPicker.Cellspacing.ShouldBe(Unit.Pixel(1));
		}

		[Test]
		public void BackColorItems_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();

			// Act:
			toolColorPicker.BackColorItems = Color.Black;

			// Assert:
			toolColorPicker.BackColorItems.ShouldBe(Color.Black);
		}

		[Test]
		public void Text_SetAndGetValue_ReturnsDefaultValue()
		{
			// Arrange:
			var toolColorPicker = new ToolColorPicker();
			var privateObject = new PrivateObject(toolColorPicker);
			privateObject.SetFieldOrProperty("ID", DummyId);

			// Act: // Assert:
			var propertyValue = privateObject.GetFieldOrProperty("Text") as string;
			propertyValue.ShouldNotBeNull();
			propertyValue.ShouldBe(DummyId);
		}

        [Test]
        public void Text_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            var toolColorPicker = new ToolColorPicker();
            var privateObject = new PrivateObject(toolColorPicker);

            // Act: 
            privateObject.SetFieldOrProperty("Text", DummyText);

            // Assert:
            var propertyValue = privateObject.GetFieldOrProperty("Text") as string;
            propertyValue.ShouldNotBeNull();
            propertyValue.ShouldBe(DummyText);
        }

        [Test]
        [TestCase(true, true, true)]
        [TestCase(false, false, false)]
        public void Render_AllowDHTMLIsTrue_RenderHTMLTextWrite(bool autoPostBack, bool parentIsNull, bool useSquareColor)
        {
            // Arrange
            SetUp();
            var stringWriter = new StringWriter();
            var outputTextWriter = new HtmlTextWriter(stringWriter);
            _toolColorPicker.Page = GetMockOfPage(string.Empty).Instance;
            _toolColorPicker.AutoPostBack = autoPostBack;
            _toolColorPicker.Style.Add("left", "100px");
            _toolColorPicker.BackImage = DummyString;
            _toolColorPicker.Height = new Unit(100);
            _toolColorPicker.ForeColor = Color.Red;
            _toolColorPicker.UseSquareColor = useSquareColor;
            _toolColorPicker.EnableSsl = true;
            ShimControl.AllInstances.ParentGet = (obj) =>
            {
                return parentIsNull ? null : stubToolbar;
            };

            // Act	
            _privateObject.Invoke(_MethodRender, new object[] { outputTextWriter });
            var actualResult = stringWriter.ToString();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrWhiteSpace(),
                () => actualResult.ShouldContain(_InputHidden)
            );
            TearDown();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Render_AllowDHTMLIsTrueAndPropsNotDefault_RenderHTMLTextWrite(bool parentIsNull)
        {
            // Arrange
            SetUp();
            var stringWriter = new StringWriter();
            var outputTextWriter = new HtmlTextWriter(stringWriter);
            _toolColorPicker.Page = GetMockOfPage(string.Empty).Instance;
            _toolColorPicker.Cellpadding = new Unit(10);
            _toolColorPicker.Cellspacing = new Unit(100);
            _toolColorPicker.ForeColor = Color.Empty;
            _toolColorPicker.UseSquareColor = true;
            _toolColorPicker.Width = Unit.Empty;
            _toolColorPicker.DropDownImage = DummyString;
            ShimControl.AllInstances.ParentGet = (obj) =>
            {
                return parentIsNull ? null : stubToolbar;
            };

            // Act	
            _privateObject.Invoke(_MethodRender, new object[] { outputTextWriter });
            var actualResult = stringWriter.ToString();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrWhiteSpace(),
                () => actualResult.ShouldContain(_InputHidden)
            );
            TearDown();
        }

        [Test]
        public void Render_AllowDHTMLIsFalse_RenderHTMLTextWrite()
        {
            // Arrange
            SetUp();
            var stringWriter = new StringWriter();
            var outputTextWriter = new HtmlTextWriter(stringWriter);
            _toolColorPicker.Page = GetMockOfPage(string.Empty).Instance;
            _toolColorPicker.AutoPostBack = true;
            _privateObject.SetField("_allowDHTML", false);

            // Act	
            _privateObject.Invoke(_MethodRender, new object[] { outputTextWriter });
            var actualResult = stringWriter.ToString();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrWhiteSpace(),
                () => actualResult.ShouldContain(_InputHidden)
            );
            TearDown();
        }

        [Test]
        [TestCase("IE")]
        [TestCase("")]
        public void Render_HttpContextNotNull_RenderHTMLTextWrite(string browser)
        {
            // Arrange
            SetUp();
            var shimHttpContext = new ShimHttpContext();
            ShimHttpContext.CurrentGet = () => shimHttpContext.Instance;
            var stringWriter = new StringWriter();
            var outputTextWriter = new HtmlTextWriter(stringWriter);
            _toolColorPicker.Page = GetMockOfPage(browser).Instance;
            _toolColorPicker.AutoPostBack = true;
            _privateObject.SetField("_allowDHTML", false);
            ShimControl.AllInstances.ParentGet = (obj) =>
            {
                return stubToolbar;
            };

            // Act	
            _privateObject.Invoke(_MethodRender, new object[] { outputTextWriter });
            var actualResult = stringWriter.ToString();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrWhiteSpace(),
                () => actualResult.ShouldContain(_InputHidden)
            );
            TearDown();
        }

		[Test]
		public void ClearSelection_SetItemSelectedTrue_ItemSelectedFalse()
		{
			// Arrange
			var toolColorPicker = new ToolColorPicker();
			var toolItem = new ToolItem
			{
				Selected = true
			};
			toolColorPicker.Items.Add(toolItem);

			// Act
			toolColorPicker.ClearSelection();

			// Assert
			toolItem.Selected.ShouldBeFalse();
		}

		[Test]
		public void RegisterAPIScriptBlock_ClientScriptInclude_ScriptIncluded()
		{
			// Arrange
			var toolColorPicker = new ToolColorPicker
			{
				Page = new Page()
			};

			using (ShimsContext.Create())
			{
				ShimClientScriptManager.AllInstances.GetWebResourceUrlTypeString = (type, obj1, obj2) => DummyUrl;

				// Act
				toolColorPicker.RegisterAPIScriptBlock();

				// Assert
				toolColorPicker.Page.ClientScript.IsClientScriptIncludeRegistered(ActiveToolbarScriptKey).ShouldBeTrue();
			}
		}

		[Test]
		public void RegisterActiveToolBarScript_ClientScriptInclude_ScriptIncluded()
		{
			// Arrange
			var toolColorPicker = new ToolColorPicker
			{
				Page = new Page()
			};

			using (ShimsContext.Create())
			{
				ShimClientScriptManager.AllInstances.GetWebResourceUrlTypeString = (type, obj1, obj2) => DummyUrl;

				// Act
				toolColorPicker.Page.RegisterActiveToolBarScript(DummyClientSideScript, ActiveToolbarScriptKey);

				// Assert
				toolColorPicker.Page.IsClientScriptBlockRegistered(ActiveToolbarScriptKey).ShouldBeTrue();
			}
		}
	}
}