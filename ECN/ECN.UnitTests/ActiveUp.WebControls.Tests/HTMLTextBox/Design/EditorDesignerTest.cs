using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using ActiveUp.WebControls.Tests.Utils;
using NUnit.Framework;
using Shouldly;
using DesignEditorDesigner = ActiveUp.WebControls.Design.EditorDesigner;

namespace ActiveUp.WebControls.Tests.HTMLTextBox.Design
{
    /// <summary>
    /// Unit tests for <see cref="DesignEditorDesigner.GetDesignTimeHtml()"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class EditorDesignerTest : HtmlDesignerTestBase
    {
        private const string FiftyPx = "50px";
        private const string TwentyPx = "20px";
        private const string True = "true";

        private DesignEditorDesigner _designer;
        private bool _isMaxLengthLessThanEqualToZero;

        [SetUp]
        public override void SetUp()
        {
            _designer = new DesignEditorDesigner();
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
        [TestCase(5)]
        [TestCase(-1)]
        [TestCase(0)]
        public void GetDesignTimeHtml_CheckBoxEditorSelectorMode_VerifyHtml(int maxLength)
        {
            // Arrange
            Editor.EditorModeSelector = EditorModeSelectorType.CheckBox;
            Editor.StartupMode = EditorMode.Html;
            _designer.Initialize(Editor);
            Editor.MaxLength = maxLength;
            _isMaxLengthLessThanEqualToZero = maxLength <= 0;

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
        [TestCase(5)]
        [TestCase(-1)]
        [TestCase(0)]
        public void GetDesignTimeHtml_TabsEditorSelectorModeWithDesignStartupMode_VerifyHtml(int maxLength)
        {
            // Arrange
            Editor.EditorModeSelector = EditorModeSelectorType.Tabs;
            Editor.StartupMode = EditorMode.Design;
            Editor.TextMode = false;
            Editor.MaxLength = maxLength;
            _isMaxLengthLessThanEqualToZero = maxLength <= 0;
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
        [TestCase(5)]
        [TestCase(-1)]
        [TestCase(0)]
        public void GetDesignTimeHtml_TabsEditorSelectorModeWithHtmlStartupMode_VerifyHtml(int maxLength)
        {
            // Arrange
            Editor.EditorModeSelector = EditorModeSelectorType.Tabs;
            Editor.StartupMode = EditorMode.Html;
            Editor.TextMode = false;
            Editor.MaxLength = maxLength;
            _isMaxLengthLessThanEqualToZero = maxLength <= 0;
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
        [TestCase(5)]
        [TestCase(-1)]
        [TestCase(0)]
        public void GetDesignTimeHtml_TabsEditorSelectorModeWithPreviewStartupMode_VerifyHtml(int maxLength)
        {
            // Arrange
            Editor.EditorModeSelector = EditorModeSelectorType.Tabs;
            Editor.StartupMode = EditorMode.Preview;
            Editor.TextMode = false;
            Editor.MaxLength = maxLength;
            _isMaxLengthLessThanEqualToZero = maxLength <= 0;
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
            Console.WriteLine(result);
            Console.WriteLine(StringWriter.ToString());

            result.ShouldBe(StringWriter.ToString());
        }

        private void RenderEnd()
        {
            Render(2, RenderEndTag);
        }

        private void RenderStart()
        {
            RenderBeginTag(HtmlTextWriterTag.Tr);
            if (_isMaxLengthLessThanEqualToZero)
            {
                AddAttribute(HtmlTextWriterAttribute.Align, Left);
            }
            RenderBeginTag(HtmlTextWriterTag.Td);
        }

        private void RenderTabsEnd()
        {
            Render(Editor.EditorModePreviewIcon, "tab_preview.gif", Editor.EditorModePreviewLabel);
            Render(7, RenderEndTag);

            if (!_isMaxLengthLessThanEqualToZero)
            {
                AddAttribute(HtmlTextWriterAttribute.Align, Right);
                RenderBeginTag(HtmlTextWriterTag.Table, HtmlTextWriterTag.Tr, HtmlTextWriterTag.Td);
                AddAttributes(Attributes(HtmlTextWriterAttribute.Cellpadding, HtmlTextWriterAttribute.Cellspacing), Zero, Zero);
                RenderBeginTag(HtmlTextWriterTag.Table, HtmlTextWriterTag.Tr, HtmlTextWriterTag.Td);
                Writer.AddStyleAttribute("font-family", Font);
                Writer.AddStyleAttribute("font-size", FontSize);
                Writer.AddAttribute(HtmlTextWriterAttribute.Valign, Middle);
                RenderBeginTag(HtmlTextWriterTag.Span);
                Write($"Counter{Space}{Space}");
                RenderEndTag();

                var valCounter = Editor.Text?.Length - Editor.MaxLength ?? Editor.MaxLength;
                AddAttribute(HtmlTextWriterAttribute.Value, valCounter.ToString());
                Writer.AddStyleAttribute(HtmlTextWriterStyle.Width, FiftyPx);
                Writer.AddStyleAttribute(HtmlTextWriterStyle.Height, TwentyPx);
                Writer.AddAttribute(HtmlTextWriterAttribute.Disabled, True);
                Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                Render(10, RenderEndTag);
            }
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
            if (!_isMaxLengthLessThanEqualToZero)
            {
                AddAttribute(HtmlTextWriterAttribute.Align, Left);
                RenderBeginTag(HtmlTextWriterTag.Table, HtmlTextWriterTag.Tr, HtmlTextWriterTag.Td);
            }
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
            Editor.Toolbars.RenderControl(Writer);

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

        protected override string Image(string icon, string fileName)
        {
            return Editor.IconsDirectory + icon;
        }
    }
}