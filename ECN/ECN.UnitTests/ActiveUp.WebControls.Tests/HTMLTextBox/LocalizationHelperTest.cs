using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class LocalizationHelperTest
    {
        private LocalizationSettings _localizationSettings;
        private Editor _editor;

        [SetUp]
        public void SetUp()
        {
            _localizationSettings = GetLocalizationSettings();
            _editor = GetEditor();
        }

        [Test]
        public void Apply_EditorWithNoTools_SetsCorrectEditorMode()
        {
            // Arrange
            _editor.EditorModeDesignLabel = string.Empty;
            _editor.EditorModeHtmlLabel = string.Empty;
            _editor.EditorModePreviewLabel = string.Empty;

            // Act	
            LocalizationHelper.Apply(_localizationSettings, _editor);

            // Assert
            _editor.ShouldSatisfyAllConditions(
                () => _editor.ShouldNotBeNull(),
                () => _editor.EditorModeDesignLabel.ShouldBe(LocalizedTextId.DESIGN_LABEL),
                () => _editor.EditorModeHtmlLabel.ShouldBe(LocalizedTextId.HTML_LABEL),
                () => _editor.EditorModePreviewLabel.ShouldBe(LocalizedTextId.PREVIEW_LABEL));
        }

        [Test]
        public void Apply_EditorWithDifferentTools_SetsCorrectTooltips()
        {
            // Arrange
            var toolbar = _editor.Toolbars.Toolbars[0];
            var toolBold = toolbar.Tools.Add(new ToolBold());
            var toolItalic = toolbar.Tools.Add(new ToolItalic());
            var toolUnderline = toolbar.Tools.Add(new ToolUnderline());
            var toolCut = toolbar.Tools.Add(new ToolCut());
            var toolCopy = toolbar.Tools.Add(new ToolCopy());
            var toolPaste = toolbar.Tools.Add(new ToolPaste());
            var toolAlignLeft = toolbar.Tools.Add(new ToolAlignLeft());
            var toolAlignCenter = toolbar.Tools.Add(new ToolAlignCenter());
            var toolAlignRight = toolbar.Tools.Add(new ToolAlignRight());
            var toolAlignJustify = toolbar.Tools.Add(new ToolAlignJustify());
            var toolOrderedList = toolbar.Tools.Add(new ToolOrderedList());
            var toolUnorderedList = toolbar.Tools.Add(new ToolUnorderedList());
            var toolIndent = toolbar.Tools.Add(new ToolIndent());
            var toolOutdent = toolbar.Tools.Add(new ToolOutdent());
            var toolPrint = toolbar.Tools.Add(new ToolPrint());
            var toolSubscript = toolbar.Tools.Add(new ToolSubscript());
            var toolSuperscript = toolbar.Tools.Add(new ToolSuperscript());
            var toolStrikeThrough = toolbar.Tools.Add(new ToolStrikeThrough());
            var toolSpellChecker = toolbar.Tools.Add(new ToolSpellChecker());
            var toolFind = toolbar.Tools.Add(new ToolFind());
            var toolReplace = toolbar.Tools.Add(new ToolReplace());
            var toolImage = toolbar.Tools.Add(new ToolImage());
            var toolLink = toolbar.Tools.Add(new ToolLink());
            var toolCodeCleaner = toolbar.Tools.Add(new ToolCodeCleaner());
            var toolFlash = toolbar.Tools.Add(new ToolFlash());

            // Arrange & Act	
            LocalizationHelper.Apply(_localizationSettings, _editor);

            // Assert
            _editor.ShouldSatisfyAllConditions(
                () => _editor.ShouldNotBeNull(),
                () => toolbar.Tools[toolBold].ToolTip.ShouldBe(LocalizedTextId.TOOL_BOLD),
                () => toolbar.Tools[toolItalic].ToolTip.ShouldBe(LocalizedTextId.TOOL_ITALIC),
                () => toolbar.Tools[toolUnderline].ToolTip.ShouldBe(LocalizedTextId.TOOL_UNDERLINE),
                () => toolbar.Tools[toolCut].ToolTip.ShouldBe(LocalizedTextId.TOOL_CUT),
                () => toolbar.Tools[toolCopy].ToolTip.ShouldBe(LocalizedTextId.TOOL_COPY),
                () => toolbar.Tools[toolPaste].ToolTip.ShouldBe(LocalizedTextId.TOOL_PASTE),
                () => toolbar.Tools[toolAlignLeft].ToolTip.ShouldBe(LocalizedTextId.TOOL_ALIGN_LEFT),
                () => toolbar.Tools[toolAlignCenter].ToolTip.ShouldBe(LocalizedTextId.TOOL_ALIGN_CENTER),
                () => toolbar.Tools[toolAlignRight].ToolTip.ShouldBe(LocalizedTextId.TOOL_ALIGN_RIGHT),
                () => toolbar.Tools[toolAlignJustify].ToolTip.ShouldBe(LocalizedTextId.TOOL_ALIGN_JUSTIFY),
                () => toolbar.Tools[toolOrderedList].ToolTip.ShouldBe(LocalizedTextId.TOOL_ORDERED_LIST),
                () => toolbar.Tools[toolUnorderedList].ToolTip.ShouldBe(LocalizedTextId.TOOL_UNORDERED_LIST),
                () => toolbar.Tools[toolIndent].ToolTip.ShouldBe(LocalizedTextId.TOOL_INDENT),
                () => toolbar.Tools[toolOutdent].ToolTip.ShouldBe(LocalizedTextId.TOOL_OUTDENT),
                () => toolbar.Tools[toolPrint].ToolTip.ShouldBe(LocalizedTextId.TOOL_PRINT),
                () => toolbar.Tools[toolSubscript].ToolTip.ShouldBe(LocalizedTextId.TOOL_SUBSCRIPT),
                () => toolbar.Tools[toolSuperscript].ToolTip.ShouldBe(LocalizedTextId.TOOL_SUPERSCRIPT),
                () => toolbar.Tools[toolStrikeThrough].ToolTip.ShouldBe(LocalizedTextId.TOOL_STRIKE_THROUGH),
                () => toolbar.Tools[toolSpellChecker].ToolTip.ShouldBe(LocalizedTextId.TOOL_SPELL_CHECKER),
                () => toolbar.Tools[toolFind].ToolTip.ShouldBe(LocalizedTextId.TOOL_FIND),
                () => toolbar.Tools[toolReplace].ToolTip.ShouldBe(LocalizedTextId.TOOL_REPLACE),
                () => toolbar.Tools[toolImage].ToolTip.ShouldBe(LocalizedTextId.TOOL_IMAGE),
                () => toolbar.Tools[toolLink].ToolTip.ShouldBe(LocalizedTextId.TOOL_HYPERLINK),
                () => toolbar.Tools[toolCodeCleaner].ToolTip.ShouldBe(LocalizedTextId.TOOL_CODE_CLEANER),
                () => toolbar.Tools[toolFlash].ToolTip.ShouldBe(LocalizedTextId.TOOL_FLASH));
        }

        [Test]
        public void Apply_EditorWithDifferentTools_SetsCorrectText()
        {
            // Arrange
            var toolbar = _editor.Toolbars.Toolbars[0];
            var toolCustomLinks = toolbar.Tools.Add(new ToolCustomLinks());
            var toolParagraph = toolbar.Tools.Add(new ToolParagraph());
            var toolFontFace = toolbar.Tools.Add(new ToolFontFace());
            var toolCustomTags = toolbar.Tools.Add(new ToolCustomTags());
            var toolCodeSnippets = toolbar.Tools.Add(new ToolCodeSnippets());
            var toolFontSize = toolbar.Tools.Add(new ToolFontSize());
            
            // Arrange & Act	
            LocalizationHelper.Apply(_localizationSettings, _editor);

            // Assert
            _editor.ShouldSatisfyAllConditions(
                () => _editor.ShouldNotBeNull(),
                () => toolbar.Tools[toolCustomLinks].Text.ShouldBe(LocalizedTextId.TOOL_CUSTOM_LINKS),
                () => toolbar.Tools[toolParagraph].Text.ShouldBe(LocalizedTextId.TOOL_PARAGRAPH),
                () => toolbar.Tools[toolFontFace].Text.ShouldBe(LocalizedTextId.TOOL_FONT_FACE),
                () => toolbar.Tools[toolCustomTags].Text.ShouldBe(LocalizedTextId.TOOL_CUSTOM_TAGS),
                () => toolbar.Tools[toolCodeSnippets].Text.ShouldBe(LocalizedTextId.TOOL_CODE_SNIPPETS),
                () => toolbar.Tools[toolFontSize].Text.ShouldBe(LocalizedTextId.TOOL_FONT_SIZE));
        }

        [Test]
        public void Apply_EditorWithDifferentTools_SetsCorrectPopupContentTitleText()
        {
            // Arrange
            var toolbar = _editor.Toolbars.Toolbars[0];
            var toolSpellChecker = new ToolSpellChecker();
            toolbar.Tools.Add(toolSpellChecker);
            var toolFind = new ToolFind();
            toolbar.Tools.Add(toolFind);
            var toolReplace = new ToolReplace();
            toolbar.Tools.Add(toolReplace);
            var toolImage = new ToolImage();
            toolbar.Tools.Add(toolImage);
            var toolLink = new ToolLink();
            toolbar.Tools.Add(toolLink);
            var toolFlash = new ToolFlash();
            toolbar.Tools.Add(toolFlash);

            // Arrange & Act	
            LocalizationHelper.Apply(_localizationSettings, _editor);

            // Assert
            _editor.ShouldSatisfyAllConditions(
                () => _editor.ShouldNotBeNull(),
                () => toolSpellChecker.PopupContents.TitleText.ShouldBe(LocalizedTextId.TOOL_SPELL_CHECKER_TITLE),
                () => toolFind.PopupContents.TitleText.ShouldBe(LocalizedTextId.TOOL_FIND_TITLE),
                () => toolReplace.PopupContents.TitleText.ShouldBe(LocalizedTextId.TOOL_REPLACE_TITLE),
                () => toolImage.PopupContents.TitleText.ShouldBe(LocalizedTextId.TOOL_IMAGE_TITLE),
                () => toolLink.PopupContents.TitleText.ShouldBe(LocalizedTextId.TOOL_LINK_TITLE),
                () => toolFlash.PopupContents.TitleText.ShouldBe(LocalizedTextId.TOOL_FLASH_TITLE));
        }

        private static Editor GetEditor()
        {            
            var toolbar = new Toolbar();
            var editor = new Editor();
            editor.Toolbars = new ToolbarsContainer();
            editor.Toolbars.Toolbars.Add(toolbar);

            return editor;
        }

        private static LocalizationSettings GetLocalizationSettings()
        {
            var localizationSettings = new LocalizationSettings();
            localizationSettings.Texts.Add(LocalizedTextId.DESIGN_LABEL, LocalizedTextId.DESIGN_LABEL);
            localizationSettings.Texts.Add(LocalizedTextId.HTML_LABEL, LocalizedTextId.HTML_LABEL);
            localizationSettings.Texts.Add(LocalizedTextId.PREVIEW_LABEL, LocalizedTextId.PREVIEW_LABEL);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_BOLD, LocalizedTextId.TOOL_BOLD);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_ITALIC, LocalizedTextId.TOOL_ITALIC);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_UNDERLINE, LocalizedTextId.TOOL_UNDERLINE);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_CUT, LocalizedTextId.TOOL_CUT);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_COPY, LocalizedTextId.TOOL_COPY);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_PASTE, LocalizedTextId.TOOL_PASTE);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_CUSTOM_LINKS, LocalizedTextId.TOOL_CUSTOM_LINKS);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_PARAGRAPH, LocalizedTextId.TOOL_PARAGRAPH);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_FONT_FACE, LocalizedTextId.TOOL_FONT_FACE);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_FONT_SIZE, LocalizedTextId.TOOL_FONT_SIZE);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_ALIGN_LEFT, LocalizedTextId.TOOL_ALIGN_LEFT);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_ALIGN_CENTER, LocalizedTextId.TOOL_ALIGN_CENTER);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_ALIGN_RIGHT, LocalizedTextId.TOOL_ALIGN_RIGHT);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_ALIGN_JUSTIFY, LocalizedTextId.TOOL_ALIGN_JUSTIFY);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_ORDERED_LIST, LocalizedTextId.TOOL_ORDERED_LIST);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_UNORDERED_LIST, LocalizedTextId.TOOL_UNORDERED_LIST);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_INDENT, LocalizedTextId.TOOL_INDENT);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_OUTDENT, LocalizedTextId.TOOL_OUTDENT);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_PRINT, LocalizedTextId.TOOL_PRINT);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_SUBSCRIPT, LocalizedTextId.TOOL_SUBSCRIPT);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_SUPERSCRIPT, LocalizedTextId.TOOL_SUPERSCRIPT);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_STRIKE_THROUGH, LocalizedTextId.TOOL_STRIKE_THROUGH);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_SPELL_CHECKER, LocalizedTextId.TOOL_SPELL_CHECKER);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_FIND, LocalizedTextId.TOOL_FIND);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_REPLACE, LocalizedTextId.TOOL_REPLACE);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_IMAGE, LocalizedTextId.TOOL_IMAGE);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_HYPERLINK, LocalizedTextId.TOOL_HYPERLINK);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_CODE_CLEANER, LocalizedTextId.TOOL_CODE_CLEANER);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_FLASH, LocalizedTextId.TOOL_FLASH);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_CUSTOM_TAGS, LocalizedTextId.TOOL_CUSTOM_TAGS);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_CODE_SNIPPETS, LocalizedTextId.TOOL_CODE_SNIPPETS);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_HYPERLINK, LocalizedTextId.TOOL_HYPERLINK);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_FLASH_TITLE, LocalizedTextId.TOOL_FLASH_TITLE);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_LINK_TITLE, LocalizedTextId.TOOL_LINK_TITLE);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_IMAGE_TITLE, LocalizedTextId.TOOL_IMAGE_TITLE);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_REPLACE_TITLE, LocalizedTextId.TOOL_REPLACE_TITLE);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_FIND_TITLE, LocalizedTextId.TOOL_FIND_TITLE);
            localizationSettings.Texts.Add(LocalizedTextId.TOOL_SPELL_CHECKER_TITLE, LocalizedTextId.TOOL_SPELL_CHECKER_TITLE);

            return localizationSettings;
        }
    }
}
