using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ActiveUp.WebControls.Design;
using ActiveUp.WebControls.Tests.Helper;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox.Design
{
    /// <summary>
    /// Unit Tests for <see cref="EditorPropertyBuilderForm._Init"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class EditorPropertyBuilderFormTest
    {
        private EditorPropertyBuilderForm _editorForm;
        private Editor _editor;

        [SetUp]
        public void Setup()
        {
            _editor = new Editor
            {
                TabIndex = TabIndex,
                MaxLength = MaxLength,
                EditorModeDesignIcon = DesignIcon,
                EditorModeDesignLabel = DesignLabel,
                EditorModeHtmlIcon = HtmlIcon,
                EditorModeHtmlLabel = HtmlLabel,
                EditorModePreviewIcon = PreviewIcon,
                EditorModePreviewLabel = PreviewLabel,
                Text = "Text",
                BackColor = BackColor,
                BorderColor = BorderColor,
                BorderStyle = BorderStyle,
                BorderWidth = BorderWidth,
                CssClass = CssClass,
                ForeColor = ForeColor,
                Height = Height,
                Width = Width,
                TextareaRows = TextAreaRows,
                TextareaColumns = TextAreaColumns,
                UseBR = true,
                AllowRollOver = true,
                AutoDetectSsl = true,
                AutoHideToolBars = true,
                CleanOnPaste = true,
                StartupFocus = true,
                HackProtectionDisabled = true,
                Visible = true
            };
        }

        [Test]
        public void Init_WhenCalledWithDefaultStartupMode_SelectDesignRadioButton()
        {
            // Arrange
            _editorForm = new EditorPropertyBuilderForm(_editor);

            // Act
            _editorForm.GetType().CallMethod(Init, new object[0], _editorForm);
            ReteriveControls();

            // Assert
            VerifyCommonLogic();
            _rbDesign.ShouldSatisfyAllConditions(
                () => _rbDesign.Checked.ShouldBeTrue(),
                () => _rbHtml.Checked.ShouldBeFalse(),
                () => _rbPreview.Checked.ShouldBeFalse());
        }

        [Test]
        public void Init_WhenCalledWithDesignStartupMode_SelectDesignRadioButton()
        {
            // Arrange
            _editor.StartupMode = EditorMode.Design;
            _editorForm = new EditorPropertyBuilderForm(_editor);

            // Act
            _editorForm.GetType().CallMethod(Init, new object[0], _editorForm);
            ReteriveControls();

            // Assert
            VerifyCommonLogic();
            _rbDesign.ShouldSatisfyAllConditions(
                () => _rbDesign.Checked.ShouldBeTrue(),
                () => _rbHtml.Checked.ShouldBeFalse(),
                () => _rbPreview.Checked.ShouldBeFalse());
        }

        [Test]
        public void Init_WhenCalledWithHtmlStartupMode_SelectHtmlRadioButton()
        {
            // Arrange
            _editor.StartupMode = EditorMode.Html;
            _editorForm = new EditorPropertyBuilderForm(_editor);

            // Act
            _editorForm.GetType().CallMethod(Init, new object[0], _editorForm);
            ReteriveControls();

            // Assert
            VerifyCommonLogic();
            _rbHtml.ShouldSatisfyAllConditions(
                () => _rbHtml.Checked.ShouldBeTrue(),
                () => _rbDesign.Checked.ShouldBeFalse(),
                () => _rbPreview.Checked.ShouldBeFalse());
        }

        [Test]
        public void Init_WhenCalledWithPreviewMode_SelectPreviewRadioButton()
        {
            // Arrange
            _editor.StartupMode = EditorMode.Preview;
            _editorForm = new EditorPropertyBuilderForm(_editor);

            // Act
            _editorForm.GetType().CallMethod(Init, new object[0], _editorForm);
            ReteriveControls();

            // Assert
            VerifyCommonLogic();
            _rbPreview.ShouldSatisfyAllConditions(
                () => _rbPreview.Checked.ShouldBeTrue(),
                () => _rbDesign.Checked.ShouldBeFalse(),
                () => _rbHtml.Checked.ShouldBeFalse());
        }

        [Test]
        public void Init_WhenCalledWithDefaultEditorMode_SelectTabsRadioButton()
        {
            // Arrange
            _editorForm = new EditorPropertyBuilderForm(_editor);

            // Act
            _editorForm.GetType().CallMethod(Init, new object[0], _editorForm);
            ReteriveControls();

            // Assert
            VerifyCommonLogic();
            _rbTabs.ShouldSatisfyAllConditions(
                () => _rbTabs.Checked.ShouldBeTrue(),
                () => _rbCheckbox.Checked.ShouldBeFalse(),
                () => _rbNone.Checked.ShouldBeFalse());
        }

        [Test]
        public void Init_WhenCalledWithNoneEditorMode_SelectNoneRadioButton()
        {
            // Arrange
            _editor.EditorModeSelector = EditorModeSelectorType.None;
            _editorForm = new EditorPropertyBuilderForm(_editor);

            // Act
            _editorForm.GetType().CallMethod(Init, new object[0], _editorForm);
            ReteriveControls();

            // Assert
            VerifyCommonLogic();
            _rbNone.ShouldSatisfyAllConditions(
                () => _rbNone.Checked.ShouldBeTrue(),
                () => _rbCheckbox.Checked.ShouldBeFalse(),
                () => _rbTabs.Checked.ShouldBeFalse());
        }

        [Test]
        public void Init_WhenCalledWithCheckBoxEditorMode_SelectCheckBoxRadioButton()
        {
            // Arrange
            _editor.EditorModeSelector = EditorModeSelectorType.CheckBox;
            _editorForm = new EditorPropertyBuilderForm(_editor);

            // Act
            _editorForm.GetType().CallMethod(Init, new object[0], _editorForm);
            ReteriveControls();

            // Assert
            VerifyCommonLogic();
            _rbCheckbox.ShouldSatisfyAllConditions(
                () => _rbCheckbox.Checked.ShouldBeTrue(),
                () => _rbNone.Checked.ShouldBeFalse(),
                () => _rbTabs.Checked.ShouldBeFalse());
        }

        [Test]
        public void Init_WhenCalledWithTabEditorMode_SelectTabsRadioButton()
        {
            // Arrange
            _editor.EditorModeSelector = EditorModeSelectorType.Tabs;
            _editorForm = new EditorPropertyBuilderForm(_editor);

            // Act
            _editorForm.GetType().CallMethod(Init, new object[0], _editorForm);
            ReteriveControls();

            // Assert
            VerifyCommonLogic();
            _rbTabs.ShouldSatisfyAllConditions(
                () => _rbTabs.Checked.ShouldBeTrue(),
                () => _rbNone.Checked.ShouldBeFalse(),
                () => _rbCheckbox.Checked.ShouldBeFalse());
        }

        private void VerifyCommonLogic()
        {
            _editorForm.ShouldSatisfyAllConditions(
                () => _tbTabIndex.Text.ShouldBe(TabIndex.ToString()),
                () => _tbMaxLength.Text.ShouldBe(MaxLength.ToString()),
                () => _tbDesignIcon.Text.ShouldBe(DesignIcon),
                () => _tbDesignLabel.Text.ShouldBe(DesignLabel),
                () => _tbHtmlIcon.Text.ShouldBe(HtmlIcon),
                () => _tbHtmlLabel.Text.ShouldBe(HtmlLabel),
                () => _tbPreviewIcon.Text.ShouldBe(PreviewIcon),
                () => _tbPreviewLabel.Text.ShouldBe(PreviewLabel),
                () => _tbRows.Text.ShouldBe(TextAreaRows),
                () => _tbCols.Text.ShouldBe(TextAreaColumns),
                () => _tbCssClass.Text.ShouldBe(CssClass));

            VerifyCheckBoxList();
            VerifyStyles();
        }

        private void VerifyCheckBoxList()
        {
            Enumerable.Range(0, 8).ToList().ForEach(x =>
                _clbBehavior.CheckedItems[x].ShouldBe(_clbBehavior.Items[x])
            );
        }

        private void VerifyStyles()
        {
            _style.ShouldSatisfyAllConditions(
                () => _style.BackColor.ShouldBe(BackColor),
                () => _style.BorderColor.ShouldBe(BorderColor),
                () => _style.BorderStyle.ShouldBe(BorderStyle),
                () => _style.BorderWidth.ShouldBe(BorderWidth),
                () => _style.CssClass.ShouldBe(CssClass),
                () => _style.ForeColor.ShouldBe(ForeColor),
                () => _style.Height.ShouldBe(Height),
                () => _style.Width.ShouldBe(Width),
                () => _pgStyle.SelectedObject.ShouldBe(_style));
        }
    }
}