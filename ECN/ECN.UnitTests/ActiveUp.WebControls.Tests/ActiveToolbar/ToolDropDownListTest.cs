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
    public class ToolDropDownListTest
    {
        private const string ActiveToolbarScriptKey = "ActiveToolbar";
        private const string DummyClientSideScript = "DummyClientSideScript";
        private const string DummyUrl = "DummyUrl";
        private const string DummyDropDownImage = @"c\\temp\\test.png";
        private const string DummyIndentText = "DummyIndentText";
        private const string DummyId = "DummyId";
        private const string DummyText = "DummyText";
        private const string MethodRender = "Render";
        private const string FieldAllowDHTML = "_allowDHTML";
        private ToolDropDownList _toolDropDownList;
        private PrivateObject _privateObject;
        private IDisposable _shimObject;
        private StubToolbar stubToolbar;

        private ShimPage GetMockOfPage(string browser)
        {
            var shimPage = new ShimPage();
            var shimHttpRequest = new ShimHttpRequest();
            var shimHttpBrowserCapabilities = new ShimHttpBrowserCapabilities();
            ShimHttpCapabilitiesBase.AllInstances.BrowserGet = (obj) => browser;
            ShimHttpRequest.AllInstances.BrowserGet = (obj) => shimHttpBrowserCapabilities.Instance;
            ShimPage.AllInstances.RequestGet = (obj) => shimHttpRequest.Instance;
            ShimPage.AllInstances.ClientScriptGet = (obj) => new ShimClientScriptManager().Instance;
            ShimClientScriptManager.AllInstances.GetWebResourceUrlTypeString = (obj, type, url) => DummyText;
            return shimPage;
        }

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
            _toolDropDownList = new ToolDropDownList();
            _privateObject = new PrivateObject(_toolDropDownList);
            stubToolbar = new StubToolbar();
            stubToolbar.ParentGet = () => null;
            stubToolbar.ImagesDirectory = DummyText;
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void ViewState_LoadSaveTrack_ReturnsTheSetValue()
        {
            // Arrange:
            //Create the control, start tracking viewstate, then set a new Text value.
            var toolDropDownList = new ToolDropDownList();
            toolDropDownList.EnableSsl = true;
            var privateObject = new PrivateObject(toolDropDownList);
            privateObject.Invoke("TrackViewState");
            toolDropDownList.EnableSsl = false;

            //Save the control's state
            var viewState = privateObject.Invoke("SaveViewState");

            //Create a new control instance and load the state
            //back into it, overriding any existing values
            var dropDownList = new ToolDropDownList();
            toolDropDownList.EnableSsl = true;

            var dropdownPrivateObject = new PrivateObject(dropDownList);

            // Act:
            dropdownPrivateObject.Invoke("LoadViewState", viewState);

            // Assert:
            dropDownList.EnableSsl.
                ShouldBeFalse("Value restored from viewstate does not match the original value we set");
        }

        [Test]
        public void DropDownImage_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            var toolDropDownList = new ToolDropDownList();

            // Act:
            toolDropDownList.DropDownImage = DummyDropDownImage;

            // Assert:
            toolDropDownList.DropDownImage.ShouldBe(DummyDropDownImage);
        }

        [Test]
        public void IndentText_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            var toolDropDownList = new ToolDropDownList();
            var privateObject = new PrivateObject(toolDropDownList);

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
            var toolDropDownList = new ToolDropDownList();

            // Act:
            toolDropDownList.AutoPostBack = true;

            // Assert:
            toolDropDownList.AutoPostBack.ShouldBeTrue();
        }

        [Test]
        public void ChangeToSelectedText_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            var toolDropDownList = new ToolDropDownList();
            var privateObject = new PrivateObject(toolDropDownList);

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
            var toolDropDownList = new ToolDropDownList();

            // Act:
            toolDropDownList.WindowBorderStyle = BorderStyle.Solid;

            // Assert:
            toolDropDownList.WindowBorderStyle.ShouldBe(BorderStyle.Solid);
        }

        [Test]
        public void WindowBorderWidth_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            var toolDropDownList = new ToolDropDownList();

            // Act:
            toolDropDownList.WindowBorderWidth = Unit.Pixel(1);

            // Assert:
            toolDropDownList.WindowBorderWidth.ShouldBe(Unit.Pixel(1));
        }

        [Test]
        public void ItemBorderColor_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            var toolDropDownList = new ToolDropDownList();

            // Act:
            toolDropDownList.ItemBorderColor = Color.Black;

            // Assert:
            toolDropDownList.ItemBorderColor.ShouldBe(Color.Black);
        }

        [Test]
        public void ItemBorderColorRollOver_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            var toolDropDownList = new ToolDropDownList();

            // Act:
            toolDropDownList.ItemBorderColorRollOver = Color.Black;

            // Assert:
            toolDropDownList.ItemBorderColorRollOver.ShouldBe(Color.Black);
        }

        [Test]
        public void ItemAlign_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            var toolDropDownList = new ToolDropDownList();

            // Act:
            toolDropDownList.ItemAlign = Align.Center;

            // Assert:
            toolDropDownList.ItemAlign.ShouldBe(Align.Center);
        }

        [Test]
        public void BorderColorRollOver_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            var toolDropDownList = new ToolDropDownList();

            // Act:
            toolDropDownList.BorderColorRollOver = Color.Black;

            // Assert:
            toolDropDownList.BorderColorRollOver.ShouldBe(Color.Black);
        }

        [Test]
        public void BorderWidth_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            var toolDropDownList = new ToolDropDownList();

            // Act:
            toolDropDownList.BorderWidth = Unit.Pixel(1);

            // Assert:
            toolDropDownList.BorderWidth.ShouldBe(Unit.Pixel(1));
        }

        [Test]
        public void BorderStyle_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            var toolDropDownList = new ToolDropDownList();

            // Act:
            toolDropDownList.BorderStyle = BorderStyle.Solid;

            // Assert:
            toolDropDownList.BorderStyle.ShouldBe(BorderStyle.Solid);
        }

        [Test]
        public void CellPadding_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            var toolDropDownList = new ToolDropDownList();

            // Act:
            toolDropDownList.Cellpadding = Unit.Pixel(1);

            // Assert:
            toolDropDownList.Cellpadding.ShouldBe(Unit.Pixel(1));
        }

        [Test]
        public void CellSpacing_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            var toolDropDownList = new ToolDropDownList();

            // Act:
            toolDropDownList.Cellspacing = Unit.Pixel(1);

            // Assert:
            toolDropDownList.Cellspacing.ShouldBe(Unit.Pixel(1));
        }

        [Test]
        public void BackColorItems_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            var toolDropDownList = new ToolDropDownList();

            // Act:
            toolDropDownList.BackColorItems = Color.Black;

            // Assert:
            toolDropDownList.BackColorItems.ShouldBe(Color.Black);
        }

        [Test]
        public void Text_SetAndGetValue_ReturnsDefaultValue()
        {
            // Arrange:
            var toolDropDownList = new ToolDropDownList();
            toolDropDownList.ID = DummyId;

            // Act: // Assert:
            toolDropDownList.Text.ShouldBe(DummyId);
        }

        [Test]
        public void Text_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            var toolDropDownList = new ToolDropDownList();

            // Act:
            toolDropDownList.Text = DummyText;

            // Assert:
            toolDropDownList.Text.ShouldBe(DummyText);
        }

        [Test]
        public void Render_AllowDHTMLIsFalse_RenderHTMLTextWriter()
        {
            // Arrange
            var stringWriter = new StringWriter();
            var outputTextWriter = new HtmlTextWriter(stringWriter);
            const string HiddenInput = "<input type=\"hidden\"";
            _toolDropDownList.Page = GetMockOfPage(string.Empty).Instance;
            _toolDropDownList.AutoPostBack = true;
            _privateObject.SetField(FieldAllowDHTML, false);

            // Act	
            _privateObject.Invoke(MethodRender, new object[] { outputTextWriter });
            var actualResult = stringWriter.ToString();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrWhiteSpace(),
                () => actualResult.ShouldContain(HiddenInput)
            );
        }

        [Test]
        [TestCase("IE", true, true)]
        [TestCase("", false, false)]
        public void Render_AllowDHTMLIsTrue_RenderHTMLTextWriter(string browser, bool autoPostBack, bool parentIsNull)
        {
            // Arrange
            var stringWriter = new StringWriter();
            var outputTextWriter = new HtmlTextWriter(stringWriter);
            const string HiddenInput = "<input type=\"hidden\"";
            _toolDropDownList.Page = GetMockOfPage(browser).Instance;
            _toolDropDownList.AutoPostBack = autoPostBack;
            _toolDropDownList.Text = DummyText;
            _toolDropDownList.Style.Add(DummyText, DummyText);
            _toolDropDownList.BackImage = DummyText;
            _toolDropDownList.Width = new Unit(100);
            _toolDropDownList.Height = new Unit(100);
            _toolDropDownList.ForeColor = Color.Red;
            _toolDropDownList.EnableSsl = true;
            ShimControl.AllInstances.ParentGet = (obj) =>
            {
                return parentIsNull ? null : stubToolbar;
            };
            const string WidthValue = "width=\"100%\"";
            const string HeightValue = "height=\"100%\"";

            // Act	
            _privateObject.Invoke(MethodRender, new object[] { outputTextWriter });
            var actualResult = stringWriter.ToString();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrWhiteSpace(),
                () => actualResult.ShouldContain(HiddenInput),
                () => actualResult.ShouldContain(WidthValue),
                () => actualResult.ShouldContain(HeightValue),
                () => actualResult.ShouldContain(DummyText)
            );
        }

        [Test]
        [TestCase(SelectedText.Text, false)]
        [TestCase(SelectedText.Value, true)]
        public void Render_AllowDHTMLIsTrueAndDropDownImage_RenderHTMLTextWriter(SelectedText changeToSelectedText, bool parentIsNull)
        {
            // Arrange
            var stringWriter = new StringWriter();
            var outputTextWriter = new HtmlTextWriter(stringWriter);
            const string HiddenInput = "<input type=\"hidden\"";
            _toolDropDownList.Page = GetMockOfPage(string.Empty).Instance;
            _toolDropDownList.DropDownImage = DummyText;
            _toolDropDownList.Cellpadding = new Unit(10);
            _toolDropDownList.Cellspacing = new Unit(10);
            _toolDropDownList.BackImage = DummyText;
            _toolDropDownList.Items.Add(new ToolItem(DummyText, DummyText));
            _toolDropDownList.SelectedIndex = 0;
            const string HeightValue = "height=\"100%\"";
            ShimControl.AllInstances.ParentGet = (obj) =>
            {
                return parentIsNull ? null : stubToolbar;
            };
            _toolDropDownList.ChangeToSelectedText = changeToSelectedText;

            // Act	
            _privateObject.Invoke(MethodRender, new object[] { outputTextWriter });
            var actualResult = stringWriter.ToString();

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNullOrWhiteSpace(),
                () => actualResult.ShouldContain(HiddenInput),
                () => actualResult.ShouldContain(HeightValue),
                () => actualResult.ShouldContain(DummyText)
            );
        }

		[Test]
		public void DataValueField_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			using (var testObject = new ToolDropDownList())
			{
				// Act, Assert
				testObject.DataValueField.ShouldBeEmpty();
			}
		}

		[Test]
		public void DataValueField_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new ToolDropDownList())
			{
				// Act
				testObject.DataValueField = DummyDropDownImage;

				// Assert
				testObject.DataValueField.ShouldBe(DummyDropDownImage);
			}
		}

		[Test]
		public void DataTextOnlyField_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			using (var testObject = new ToolDropDownList())
			{
				// Act, Assert
				testObject.DataTextOnlyField.ShouldBeEmpty();
			}
		}

		[Test]
		public void DataTextOnlyField_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new ToolDropDownList())
			{
				// Act
				testObject.DataTextOnlyField = DummyDropDownImage;

				// Assert
				testObject.DataTextOnlyField.ShouldBe(DummyDropDownImage);
			}
		}
      
        [Test]
        public void ClearSelection_SetItemSelectedTrue_ItemSelectedFalse()
        {
            // Arrange
            var toolDropDownList = new ToolDropDownList();
            var toolItem = new ToolItem
            {
                Selected = true
            };
            toolDropDownList.Items.Add(toolItem);

            // Act
            toolDropDownList.ClearSelection();

            // Assert
            toolItem.Selected.ShouldBeFalse();
        }

        [Test]
        public void RegisterAPIScriptBlock_ClientScriptInclude_ScriptIncluded()
        {
            // Arrange
            var toolDropDownList = new ToolDropDownList
            {
                Page = new Page()
            };

            using (ShimsContext.Create())
            {
                ShimClientScriptManager.AllInstances.GetWebResourceUrlTypeString = (type, obj1, obj2) => DummyUrl;

                // Act
                toolDropDownList.RegisterAPIScriptBlock();

                // Assert
                toolDropDownList.Page.ClientScript.IsClientScriptIncludeRegistered(ActiveToolbarScriptKey).ShouldBeTrue();
            }
        }

        [Test]
        public void RegisterActiveToolBarScript_ClientScriptInclude_ScriptIncluded()
        {
            // Arrange
            var toolDropDownList = new ToolDropDownList
            {
                Page = new Page()
            };

            using (ShimsContext.Create())
            {
                ShimClientScriptManager.AllInstances.GetWebResourceUrlTypeString = (type, obj1, obj2) => DummyUrl;

                // Act
                toolDropDownList.Page.RegisterActiveToolBarScript(DummyClientSideScript, ActiveToolbarScriptKey);

                // Assert
                toolDropDownList.Page.IsClientScriptBlockRegistered(ActiveToolbarScriptKey).ShouldBeTrue();
            }
        }
    }
}