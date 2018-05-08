using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using ActiveUp.WebControls.Tests.Utils;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
    /// <summary>
    /// Unit tests for <see cref="EditorDesigner.GetDesignTimeHtml()"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class EditorDesignerTest : HtmlDesignerTestBase
    {
        private EditorDesigner _designer;

        [SetUp]
        public override void SetUp()
        {
            _designer = new EditorDesigner();
            base.SetUp();
        }

        [Test]
        public void GetDesignTimeHtml_NoneEditorSelectorMode_VerifyHtml()
        {
            // Arrange
            Editor.EditorModeSelector = EditorModeSelectorType.None;
            _designer.Initialize(Editor);

            RenderHeader();
            RenderFooter();

            // Act
            var result = _designer.GetDesignTimeHtml();

            // Assert
            result.ShouldBe(StringWriter.ToString());
        }

        [Test]
        public void GetDesignTimeHtml_CheckBoxEditorSelectorMode_VerifyHtml()
        {
            // Arrange
            Editor.EditorModeSelector = EditorModeSelectorType.CheckBox;
            Editor.StartupMode = EditorMode.Html;
            _designer.Initialize(Editor);

            RenderHeader();
            RenderStart();
            RenderCheckBox();
            RenderEnd();
            RenderFooter();

            // Act
            var result = _designer.GetDesignTimeHtml();

            // Assert
            result.ShouldBe(StringWriter.ToString());
        }

        [Test]
        public void GetDesignTimeHtml_TabsEditorSelectorModeWithDesignStartupMode_VerifyHtml()
        {
            // Arrange
            Editor.EditorModeSelector = EditorModeSelectorType.Tabs;
            Editor.StartupMode = EditorMode.Design;
            Editor.TextMode = false;
            _designer.Initialize(Editor);

            RenderHeader();
            RenderStart();
            RenderTabsStart();
            RenderStartupModeStyles();
            RenderTabsBeforeHtmlStartupMode();
            AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, Zero);
            RenderTabsBeforePreviewStartupMode();
            AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, Zero);
            RenderTabsEnd();

            RenderEnd();
            RenderFooter();

            // Act
            var result = _designer.GetDesignTimeHtml();

            // Assert
            result.ShouldBe(StringWriter.ToString());
        }

        [Test]
        public void GetDesignTimeHtml_TabsEditorSelectorModeWithHtmlStartupMode_VerifyHtml()
        {
            // Arrange
            Editor.EditorModeSelector = EditorModeSelectorType.Tabs;
            Editor.StartupMode = EditorMode.Html;
            Editor.TextMode = false;
            _designer.Initialize(Editor);

            RenderHeader();
            RenderStart();
            RenderTabsStart();
            AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, Zero);
            RenderTabsBeforeHtmlStartupMode();
            RenderStartupModeStyles();
            RenderTabsBeforePreviewStartupMode();
            AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, Zero);
            RenderTabsEnd();

            RenderEnd();
            RenderFooter();

            // Act
            var result = _designer.GetDesignTimeHtml();

            // Assert
            result.ShouldBe(StringWriter.ToString());
        }

        [Test]
        public void GetDesignTimeHtml_TabsEditorSelectorModeWithPreviewStartupMode_VerifyHtml()
        {
            // Arrange
            Editor.EditorModeSelector = EditorModeSelectorType.Tabs;
            Editor.StartupMode = EditorMode.Preview;
            Editor.TextMode = false;
            _designer.Initialize(Editor);

            RenderHeader();
            RenderStart();
            RenderTabsStart();
            AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, Zero);
            RenderTabsBeforeHtmlStartupMode();
            AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, Zero);
            RenderTabsBeforePreviewStartupMode();
            RenderStartupModeStyles();
            RenderTabsEnd();

            RenderEnd();
            RenderFooter();

            // Act
            var result = _designer.GetDesignTimeHtml();

            // Assert
            result.ShouldBe(StringWriter.ToString());
        }

        private void RenderStart()
        {
            RenderBeginTag(HtmlTextWriterTag.Tr);
            AddAttribute(HtmlTextWriterAttribute.Align, Left);
            RenderBeginTag(HtmlTextWriterTag.Td);
        }

        private void RenderEnd()
        {
            Render(2, RenderEndTag);
        }

        private void RenderTabsEnd()
        {
            Render(Editor.EditorModePreviewIcon, "tab_preview.gif", Editor.EditorModePreviewLabel);
            Render(7, RenderEndTag);
        }

        private void RenderTabsBeforePreviewStartupMode()
        {
            Render(Editor.EditorModeHtmlIcon, "tab_html.gif", Editor.EditorModeHtmlLabel);
            Render(5, RenderEndTag);
            RenderBeginTag(HtmlTextWriterTag.Td);
            AddAttributes(Attributes(HtmlTextWriterAttribute.Cellpadding, HtmlTextWriterAttribute.Cellspacing), One, Zero);
        }

        private void RenderTabsBeforeHtmlStartupMode()
        {
            Render(Editor.EditorModeDesignIcon, "tab_design.gif", Editor.EditorModeDesignLabel);
            Render(5, RenderEndTag);
            RenderBeginTag(HtmlTextWriterTag.Td);
            AddAttributes(Attributes(HtmlTextWriterAttribute.Cellpadding, HtmlTextWriterAttribute.Cellspacing), One, Zero);
        }

        private void RenderTabsStart()
        {
            AddAttributes(Attributes(HtmlTextWriterAttribute.Cellpadding, HtmlTextWriterAttribute.Cellspacing), Two, Zero);
            RenderBeginTag(HtmlTextWriterTag.Table, HtmlTextWriterTag.Tr);
            AddAttributes(Attributes(HtmlTextWriterAttribute.Align), Left);
            RenderBeginTag(HtmlTextWriterTag.Td);
            AddAttribute(HtmlTextWriterAttribute.Cellpadding, One);
            AddAttribute(HtmlTextWriterAttribute.Cellspacing, Zero);
        }

        private void RenderCheckBox()
        {
            AddAttributes(Attributes(HtmlTextWriterAttribute.Checked, HtmlTextWriterAttribute.Type), string.Empty, "checkbox");
            RenderBeginTag(HtmlTextWriterTag.Input);
            Writer.Write(Editor.EditorModeHtmlLabel);
            RenderEndTag();
        }

        private void RenderHeader()
        {
            Editor.ControlStyle.AddAttributesToRender(Writer);
            AddAttributes(Attributes(HtmlTextWriterAttribute.Cellpadding, HtmlTextWriterAttribute.Cellspacing), Zero, Zero);
            RenderBeginTag(HtmlTextWriterTag.Table, HtmlTextWriterTag.Tr, HtmlTextWriterTag.Td);
            AddAttribute(HtmlTextWriterAttribute.Style, Style);

            Editor.CreateTools(Editor.Template, false);
            Editor.Toolbars.RenderDesign(Writer);

            Render(2, RenderEndTag);
            RenderBeginTag(HtmlTextWriterTag.Tr);
            AddAttribute(HtmlTextWriterAttribute.Height, Full);
            RenderBeginTag(HtmlTextWriterTag.Td);
            AddAttributes(Attributes(HtmlTextWriterAttribute.Style), Style);
            Writer.AddAttribute("onkeydown", $"keydown(\'{Editor.ClientID}\');");
            AddAttributes(Attributes(HtmlTextWriterAttribute.Width, HtmlTextWriterAttribute.Height, HtmlTextWriterAttribute.Style), Full, Full, $"WIDTH: {Full}; HEIGTH: {Full};");
            RenderBeginTag(HtmlTextWriterTag.Iframe);
            Render(3, RenderEndTag);
        }

        private void RenderFooter()
        {
            RenderEndTag();
        }
    }
}