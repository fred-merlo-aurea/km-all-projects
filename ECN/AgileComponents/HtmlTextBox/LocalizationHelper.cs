using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace ActiveUp.WebControls
{
    /// <summary>
    /// Represents a <see cref="LocalizationHelper"/> object.
    /// </summary>
    public class LocalizationHelper
	{
	    private const string NotInDictionaryPlaceholder = "$NOTINDIC$";
	    private const string ChangeToPlaceholder = "$CHANGETO$";
	    private const string SuggestionPlaceholder = "$SUGGESTION$";
	    private const string ButtonIgnorePlaceholder = "$BUTTON_IGNORE$";
	    private const string ButtonIgnoreAllPlaceholder = "$BUTTON_IGNORE_ALL$";
	    private const string ButtonReplacePlaceholder = "$BUTTON_REPLACE$";
	    private const string ButtonReplaceAllPlaceholder = "$BUTTON_REPLACE_ALL$";
	    private const string ButtonClosePlaceholder = "$BUTTON_CLOSE$";
	    private const string OldValuePlaceholder = "$FIND$";
	    private const string CaseSensitivePlaceholder = "$CASE_SENSITIVE$";
	    private const string WholeWordPlaceholder = "$WHOLE_WORD$";
	    private const string NextPlaceholder = "$NEXT$";
	    private const string ReplacePlaceholder = "$REPLACE$";
	    private const string ReplaceAllPlaceholder = "$REPLACE_ALL$";
	    private const string ReplaceButtonPlaceholder = "$REPLACEB$";
	    private const string CancelPlaceholder = "$CANCEL$";
	    private const string GeneralPlaceholder = "$GENERAL$";
	    private const string PicturePlaceholder = "$PICTURE$";
	    private const string TextPlaceholder = "$TEXT$";
	    private const string DispositionPlaceholder = "$DISPOSITION$";
	    private const string AlignmentPlaceholder = "$ALIGNMENT$";
	    private const string HorizontalSpacingPlaceholder = "$HORIZONTAL_SPACING$";
	    private const string VerticalSpacingPlaceholder = "$VERTICAL_SPACING$";
	    private const string BorderThicknessPlaceholder = "$BORDER_THICKNESS$";
	    private const string SizePlaceholder = "$SIZE$";
	    private const string SpecifySizePlaceholder = "$SPECIFY_SIZE$";
	    private const string WidthPlaceholder = "$WIDTH$";
	    private const string HeightPlaceholder = "$HEIGHT$";
	    private const string InPixelsPlaceholder = "$IN_PIXELS$";
	    private const string InPercentPlaceholder = "$IN_PERCENT$";
	    private const string KeepAspectRatioPlaceholder = "$KEEP_ASPECT_RATIO$";
	    private const string StylePlaceholder = "$STYLE$";
	    private const string CssClassPlaceholder = "$CSS_CLASS$";
	    private const string OkPlaceholder = "$OK$";
	    private const string DefaultPlaceholder = "$DEFAULT$";
	    private const string AbsBottomPlaceholder = "$ABSBOTTOM$";
	    private const string AbsMiddlePlaceholder = "$ABSMIDDLE$";
	    private const string BaselinePlaceholder = "$BASELINE$";
	    private const string BottomPlaceholder = "$BOTTOM$";
	    private const string LeftPlaceholder = "$LEFT$";
	    private const string MiddlePlaceholder = "$MIDDLE$";
	    private const string RightPlaceholder = "$RIGHT$";
	    private const string TextTopPlaceholder = "$TEXTTOP$";
	    private const string TopPlaceholder = "$TOP$";
	    private const string AddressPlaceholder = "$ADDRESS$";
	    private const string AnchorsPlaceholder = "$ANCHOR$";
	    private const string TargetPlaceholder = "$TARGET$";
	    private const string TooltipPlaceholder = "$TOOLTIP$";
	    private const string AltTextPlaceholder = "$ALT_TEXT$";
	    private const string RemoveLinkPlaceholder = "$REMOVE_LINK$";
	    private const string InsertLinkPlaceholder = "$INSERT_LINK$";
	    private const string FlashFilePlaceholder = "$FLASH_FILE$";
	    private const string SpecifyClassIdPlaceholder = "$SPECIFY_CLASS_ID$";
	    private const string ClassIdPlaceholder = "$CLASS_ID$";
	    private const string FlashVersionPlaceholder = "$FLASH_VERSION$";
	    private const string FlashAlignmentPlaceholder = "$FLASH_ALIGNMENT$";
	    private const string OptionsPlaceholder = "$OPTIONS$";
	    private const string QualityPlaceholder = "$QUALITY$";
	    private const string LoopPlaceholder = "$LOOP$";
	    private const string AutoPlayPlaceholder = "$AUTO_PLAY$";
	    private const string YesPlaceholder = "$YES$";
	    private const string NoPlaceholder = "$NO$";
	    private const string CenterTopPlaceholder = "$CENTER_TOP$";
	    private const string CenterCenterPlaceholder = "$CENTER_CENTER$";
	    private const string CenterBottomPlaceholder = "$CENTER_BOTTOM$";
	    private const string LeftTopPlaceholder = "$LEFT_TOP$";
	    private const string LeftCenterPlaceholder = "$LEFT_CENTER$";
	    private const string LeftBottomPlaceholder = "$LEFT_BOTTOM$";
	    private const string RightTopPlaceholder = "$RIGHT_TOP$";
	    private const string RightCenterPlaceholder = "$RIGHT_CENTER$";
	    private const string RightBottomPlaceholder = "$RIGHT_BOTTOM$";
	    private const string LowPlaceholder = "$LOW$";
	    private const string MediumPlaceholder = "$MEDIUM$";
	    private const string HighPlaceholder = "$HIGH$";
	    private static string ROOT_NAME = "NAME";
		private static string ROOT_CODE = "CODE";
		private static string ROOT_COMPATIBLEVERSION = "COMPATIBLEVERSION";
		private static string TEXT_ELEMENT = "TEXT";
		private static string TEXT_ATTRIBUTE_ID = "ID";
		private static string TEXT_ATTRIBUTE_VALUE = "VALUE";

        private static readonly IDictionary<Type, string> ToolsTooltipLocalizations = new Dictionary<Type, string>
	    {
            [typeof(ToolBold)] = LocalizedTextId.TOOL_BOLD,
            [typeof(ToolItalic)] = LocalizedTextId.TOOL_ITALIC,
            [typeof(ToolUnderline)] = LocalizedTextId.TOOL_UNDERLINE,
            [typeof(ToolCut)] = LocalizedTextId.TOOL_CUT,
            [typeof(ToolCopy)] = LocalizedTextId.TOOL_COPY,
            [typeof(ToolPaste)] = LocalizedTextId.TOOL_PASTE,
            [typeof(ToolAlignLeft)] = LocalizedTextId.TOOL_ALIGN_LEFT,
            [typeof(ToolAlignCenter)] = LocalizedTextId.TOOL_ALIGN_CENTER,
            [typeof(ToolAlignRight)] = LocalizedTextId.TOOL_ALIGN_RIGHT,
            [typeof(ToolAlignJustify)] = LocalizedTextId.TOOL_ALIGN_JUSTIFY,
            [typeof(ToolOrderedList)] = LocalizedTextId.TOOL_ORDERED_LIST,
            [typeof(ToolUnorderedList)] = LocalizedTextId.TOOL_UNORDERED_LIST,
            [typeof(ToolIndent)] = LocalizedTextId.TOOL_INDENT,
            [typeof(ToolOutdent)] = LocalizedTextId.TOOL_OUTDENT,
            [typeof(ToolPrint)] = LocalizedTextId.TOOL_PRINT,
            [typeof(ToolSubscript)] = LocalizedTextId.TOOL_SUBSCRIPT,
            [typeof(ToolSuperscript)] = LocalizedTextId.TOOL_SUPERSCRIPT,
            [typeof(ToolStrikeThrough)] = LocalizedTextId.TOOL_STRIKE_THROUGH,
            [typeof(ToolSpellChecker)] = LocalizedTextId.TOOL_SPELL_CHECKER,
            [typeof(ToolFind)] = LocalizedTextId.TOOL_FIND,
            [typeof(ToolReplace)] = LocalizedTextId.TOOL_REPLACE,
            [typeof(ToolImage)] = LocalizedTextId.TOOL_IMAGE,
            [typeof(ToolLink)] = LocalizedTextId.TOOL_HYPERLINK,
            [typeof(ToolCodeCleaner)] = LocalizedTextId.TOOL_CODE_CLEANER,
            [typeof(ToolFlash)] = LocalizedTextId.TOOL_FLASH
        };

	    private static readonly IDictionary<Type, string> ToolsTextLocalizations = new Dictionary<Type, string>
	    {
	        [typeof(ToolCustomLinks)] = LocalizedTextId.TOOL_CUSTOM_LINKS,
	        [typeof(ToolParagraph)] = LocalizedTextId.TOOL_PARAGRAPH,
	        [typeof(ToolFontFace)] = LocalizedTextId.TOOL_FONT_FACE,
	        [typeof(ToolFontSize)] = LocalizedTextId.TOOL_FONT_SIZE,
	        [typeof(ToolCustomTags)] = LocalizedTextId.TOOL_CUSTOM_TAGS,
	        [typeof(ToolCodeSnippets)] = LocalizedTextId.TOOL_CODE_SNIPPETS
        };
	    
	    private static readonly IDictionary<Type, Action<ToolBase, LocalizationSettings>> ToolsSpecializedLocalizations 
	        = new Dictionary<Type, Action<ToolBase, LocalizationSettings>>
	    {
	        [typeof(ToolSpellChecker)] = ApplySpellcheckerLocalization,
	        [typeof(ToolFind)] = ApplyToolFindLocalization,
	        [typeof(ToolReplace)] = ApplyToolReplaceLocalization,
	        [typeof(ToolImage)] = ApplyToolImageLocalization,
	        [typeof(ToolLink)] = ApplyToolLinkLocalization,
	        [typeof(ToolFlash)] = ApplyToolFlashLocalization
	    };
	    
        private delegate void Action<in T, in U>(T arg1, U arg2);

        /// <summary>
        /// Loads the specified file using the file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static LocalizationSettings Load(string filePath)
		{
			return Load(filePath,Encoding.UTF7);
		}

		/// <summary>
		/// Load the specified localization settings.
		/// </summary>
		/// <param name="filePath">The full path to the XML file.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns>The LocalizationSettings class.</returns>
		public static LocalizationSettings Load(string filePath,Encoding encoding)
		{
			StreamReader stream = new StreamReader(filePath,encoding); 
			XmlTextReader tr = new XmlTextReader(stream);
			return _Load(tr);
		}

		/// <summary>
		/// Load the specified localization settings from the specified string.
		/// </summary>
		/// <param name="content">The XML content.</param>
		/// <returns>The LocalizationSettings class.</returns>
		public static LocalizationSettings LoadFromString(string content)
		{
			MemoryStream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(content));
			XmlTextReader tr = new XmlTextReader(stream);
			return _Load(tr);
		}

		private static LocalizationSettings _Load(XmlTextReader textReader)
		{
			LocalizationSettings localizationSettings = new LocalizationSettings();
			LocalizedText localizedText = null;
			int indexCurrentText = -1;

			while(textReader.Read())
			{
				if (textReader.Name.ToUpper() == TEXT_ELEMENT)
				{
					localizedText = new LocalizedText();
					indexCurrentText = localizationSettings.Texts.Add(localizedText);
				}
				else
				{
					indexCurrentText = -1;
				}

				if (textReader.HasAttributes)
				{
					for (int i = 0; i < textReader.AttributeCount; i++)
					{
						textReader.MoveToAttribute(i);
						if (indexCurrentText != -1 && textReader.Name.ToUpper() == TEXT_ATTRIBUTE_ID)
							localizationSettings.Texts[indexCurrentText].Id = textReader.Value;
						else if (indexCurrentText != -1 && textReader.Name.ToUpper() == TEXT_ATTRIBUTE_VALUE)
							localizationSettings.Texts[indexCurrentText].Value = textReader.Value;
						else if (indexCurrentText == -1 && textReader.Name.ToUpper() == ROOT_NAME)
							localizationSettings.Name = textReader.Value;
						else if (indexCurrentText == -1 && textReader.Name.ToUpper() == ROOT_CODE)
							localizationSettings.Code = textReader.Value;
						else if (indexCurrentText == -1 && textReader.Name.ToUpper() == ROOT_COMPATIBLEVERSION)
							localizationSettings.CompatibleVersion = textReader.Value;
					}
				}
			}
			
			return localizationSettings;
		}

		/// <summary>
		/// Apply the specified localization settings to the specified Editor object
		/// </summary>
		/// <param name="localizationSettings">The localization settings to apply.</param>
		/// <param name="editor">The editor to update.</param>
		/// TODO: remove internal
		internal static void Apply(LocalizationSettings localizationSettings, Editor editor)
		{
		    GuardNotNull(localizationSettings, nameof(localizationSettings));
		    GuardNotNull(editor, nameof(editor));

            SetEditorMode(localizationSettings, editor);

		    foreach (var toolbarItem in editor.Toolbars.Toolbars)
		    {
		        var toolbar = toolbarItem as Toolbar;
		        if (toolbar == null)
		        {
		            continue;
		        }

		        foreach (var toolItem in toolbar.Tools)
		        {
		            var tool = toolItem as ToolBase;
		            if (tool == null)
		            {
		                continue;
		            }

		            var toolType = tool.GetType();
		            if (ToolsTooltipLocalizations.ContainsKey(toolType))
		            {
		                tool.ToolTip = localizationSettings.Texts[ToolsTooltipLocalizations[toolType]].Value;
		            }

		            if (ToolsTextLocalizations.ContainsKey(toolType))
		            {
		                tool.Text = localizationSettings.Texts[ToolsTextLocalizations[toolType]].Value;
		            }

		            if (ToolsSpecializedLocalizations.ContainsKey(toolType))
		            {
		                ToolsSpecializedLocalizations[toolType].Invoke(tool, localizationSettings);
		            }
                }
            }
		}

	    private static void SetEditorMode(LocalizationSettings localizationSettings, Editor editor)
	    {
	        GuardNotNull(localizationSettings, nameof(localizationSettings));
	        GuardNotNull(editor, nameof(editor));

            if (IsNullOrWhiteSpace(editor.EditorModeDesignLabel))
	        {
	            editor.EditorModeDesignLabel = localizationSettings.Texts[LocalizedTextId.DESIGN_LABEL].Value;
	        }

	        if (IsNullOrWhiteSpace(editor.EditorModeHtmlLabel))
	        {
	            editor.EditorModeHtmlLabel = localizationSettings.Texts[LocalizedTextId.HTML_LABEL].Value;
	        }

	        if (IsNullOrWhiteSpace(editor.EditorModePreviewLabel))
	        {
	            editor.EditorModePreviewLabel = localizationSettings.Texts[LocalizedTextId.PREVIEW_LABEL].Value;
	        }
	    }

	    private static void ApplySpellcheckerLocalization(ToolBase tool, LocalizationSettings localizationSettings)
	    {
            GuardNotNull(tool, nameof(tool));
            GuardNotNull(localizationSettings, nameof(localizationSettings));

	        var toolSpellChecker = tool as ToolSpellChecker;
	        if (toolSpellChecker == null)
	        {
	            return;
	        }

	        toolSpellChecker.PopupContents.TitleText = localizationSettings.Texts[LocalizedTextId.TOOL_SPELL_CHECKER_TITLE].Value;
	        toolSpellChecker.PopupContents.ContentText = toolSpellChecker
	            .PopupContents
	            .ContentText
	            .Replace(NotInDictionaryPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_NOT_IN_DICTIONARY].Value)
	            .Replace(ChangeToPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_CHANGE_TO].Value)
	            .Replace(SuggestionPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_SUGGESTION].Value)
	            .Replace(ButtonIgnorePlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_IGNORE].Value)
	            .Replace(ButtonIgnoreAllPlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_IGNORE_ALL].Value)
	            .Replace(ButtonReplacePlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_REPLACE].Value)
	            .Replace(ButtonReplaceAllPlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_REPLACE_ALL].Value)
	            .Replace(ButtonClosePlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_CLOSE].Value);

	    }

        private static void ApplyToolFindLocalization(ToolBase tool, LocalizationSettings localizationSettings)
	    {
	        GuardNotNull(tool, nameof(tool));
	        GuardNotNull(localizationSettings, nameof(localizationSettings));

	        var toolFind = tool as ToolFind;
	        if (toolFind == null)
	        {
	            return;
	        }

            toolFind.PopupContents.TitleText = localizationSettings.Texts[LocalizedTextId.TOOL_FIND_TITLE].Value;
            toolFind.PopupContents.ContentText = toolFind
	            .PopupContents
	            .ContentText
	            .Replace(OldValuePlaceholder, localizationSettings.Texts[LocalizedTextId.TOOL_FIND_LABEL].Value)
	            .Replace(CaseSensitivePlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_CASE_SENSITIVE].Value)
	            .Replace(WholeWordPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_WHOLE_WORD].Value)
	            .Replace(NextPlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_NEXT].Value);
	    }

	    private static void ApplyToolReplaceLocalization(ToolBase tool, LocalizationSettings localizationSettings)
	    {
	        GuardNotNull(tool, nameof(tool));
	        GuardNotNull(localizationSettings, nameof(localizationSettings));

	        var toolReplace = tool as ToolReplace;
	        if (toolReplace == null)
	        {
	            return;
	        }

            toolReplace.PopupContents.TitleText = localizationSettings.Texts[LocalizedTextId.TOOL_REPLACE_TITLE].Value;
	        toolReplace.PopupContents.ContentText = toolReplace
	            .PopupContents
	            .ContentText
	            .Replace(OldValuePlaceholder, localizationSettings.Texts[LocalizedTextId.TOOL_FIND_LABEL].Value)
	            .Replace(ReplacePlaceholder, localizationSettings.Texts[LocalizedTextId.TOOL_REPLACE_LABEL].Value)
	            .Replace(CaseSensitivePlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_CASE_SENSITIVE].Value)
	            .Replace(WholeWordPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_WHOLE_WORD].Value)
	            .Replace(ReplaceAllPlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_REPLACE_ALL].Value)
	            .Replace(ReplaceButtonPlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_REPLACE].Value)
	            .Replace(NextPlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_NEXT].Value)
	            .Replace(CancelPlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_CANCEL].Value);
	    }

	    private static void ApplyToolImageLocalization(ToolBase tool, LocalizationSettings localizationSettings)
	    {
	        GuardNotNull(tool, nameof(tool));
	        GuardNotNull(localizationSettings, nameof(localizationSettings));

	        var toolImage = tool as ToolImage;
	        if (toolImage == null)
	        {
	            return;
	        }

            toolImage.PopupContents.TitleText = localizationSettings.Texts[LocalizedTextId.TOOL_IMAGE_TITLE].Value;
	        toolImage.PopupContents.ContentText = toolImage
	            .PopupContents
	            .ContentText
	            .Replace(GeneralPlaceholder, localizationSettings.Texts[LocalizedTextId.CAT_GENERAL].Value)
	            .Replace(PicturePlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_PICTURE].Value)
	            .Replace(TextPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_TEXT].Value)
	            .Replace(DispositionPlaceholder, localizationSettings.Texts[LocalizedTextId.CAT_DISPOSITION].Value)
	            .Replace(AlignmentPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_ALIGNMENT].Value)
	            .Replace(HorizontalSpacingPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_HORIZONTAL_SPACING].Value)
	            .Replace(VerticalSpacingPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_VERTICAL_SPACING].Value)
	            .Replace(BorderThicknessPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_BORDER_THICKNESS].Value)
	            .Replace(SizePlaceholder, localizationSettings.Texts[LocalizedTextId.CAT_SIZE].Value)
	            .Replace(SpecifySizePlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_SPECIFY_SIZE].Value)
	            .Replace(WidthPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_WIDTH].Value)
	            .Replace(HeightPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_HEIGHT].Value)
	            .Replace(InPixelsPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_IN_PIXELS].Value)
	            .Replace(InPercentPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_IN_PERCENT].Value)
	            .Replace(KeepAspectRatioPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_KEEP_ASPECT_RATIO].Value)
	            .Replace(StylePlaceholder, localizationSettings.Texts[LocalizedTextId.CAT_STYLE].Value)
	            .Replace(CssClassPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_CSS_CLASS].Value)
	            .Replace(OkPlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_OK].Value)
	            .Replace(CancelPlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_CANCEL].Value)
	            .Replace(DefaultPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_DEFAULT].Value)
	            .Replace(AbsBottomPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_ABSBOTTOM].Value)
	            .Replace(AbsMiddlePlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_ABSMIDDLE].Value)
	            .Replace(BaselinePlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_BASELINE].Value)
	            .Replace(BottomPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_BOTTOM].Value)
	            .Replace(LeftPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_LEFT].Value)
	            .Replace(MiddlePlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_MIDDLE].Value)
	            .Replace(RightPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_RIGHT].Value)
	            .Replace(TextTopPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_TEXTTOP].Value)
	            .Replace(TopPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_TOP].Value);
	    }

	    private static void ApplyToolLinkLocalization(ToolBase tool, LocalizationSettings localizationSettings)
	    {
	        GuardNotNull(tool, nameof(tool));
	        GuardNotNull(localizationSettings, nameof(localizationSettings));

	        var toolLink = tool as ToolLink;
	        if (toolLink == null)
	        {
	            return;
	        }

            toolLink.PopupContents.TitleText = localizationSettings.Texts[LocalizedTextId.TOOL_LINK_TITLE].Value;
	        toolLink.PopupContents.ContentText = toolLink
	            .PopupContents
	            .ContentText
	            .Replace(AddressPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_ADDRESS].Value)
	            .Replace(TextPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_TEXT].Value)
	            .Replace(AnchorsPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_ANCHOR].Value)
	            .Replace(TargetPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_TARGET].Value)
	            .Replace(TooltipPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_TOOLTIP].Value)
	            .Replace(AltTextPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_ALT_TEXT].Value)
	            .Replace(RemoveLinkPlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_REMOVE_LINK].Value)
	            .Replace(InsertLinkPlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_INSERT_LINK].Value);
	    }

	    private static void ApplyToolFlashLocalization(ToolBase tool, LocalizationSettings localizationSettings)
	    {
	        GuardNotNull(tool, nameof(tool));
	        GuardNotNull(localizationSettings, nameof(localizationSettings));

	        var toolFlash = tool as ToolFlash;
	        if (toolFlash == null)
	        {
	            return;
	        }

            toolFlash.PopupContents.TitleText = localizationSettings.Texts[LocalizedTextId.TOOL_FLASH_TITLE].Value;
	        toolFlash.PopupContents.ContentText = toolFlash
	            .PopupContents
	            .ContentText
	            .Replace(GeneralPlaceholder, localizationSettings.Texts[LocalizedTextId.CAT_GENERAL].Value)
	            .Replace(FlashFilePlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_FLASH_FILE].Value)
	            .Replace(SpecifyClassIdPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_SPECIFY_CLASS_ID].Value)
	            .Replace(ClassIdPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_CLASS_ID].Value)
	            .Replace(FlashVersionPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_FLASH_VERSION].Value)
	            .Replace(DispositionPlaceholder, localizationSettings.Texts[LocalizedTextId.CAT_DISPOSITION].Value)
	            .Replace(AlignmentPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_ALIGNMENT].Value)
	            .Replace(FlashAlignmentPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_FLASH_ALIGNMENT].Value)
	            .Replace(SizePlaceholder, localizationSettings.Texts[LocalizedTextId.CAT_SIZE].Value)
	            .Replace(SpecifySizePlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_SPECIFY_SIZE].Value)
	            .Replace(WidthPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_WIDTH].Value)
	            .Replace(HeightPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_HEIGHT].Value)
	            .Replace(InPixelsPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_IN_PIXELS].Value)
	            .Replace(InPercentPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_IN_PERCENT].Value)
	            .Replace(OptionsPlaceholder, localizationSettings.Texts[LocalizedTextId.CAT_OPTIONS].Value)
	            .Replace(QualityPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_QUALITY].Value)
	            .Replace(LoopPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_LOOP].Value)
	            .Replace(AutoPlayPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_AUTO_PLAY].Value)
	            .Replace(YesPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_YES].Value)
	            .Replace(NoPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_NO].Value)
	            .Replace(OkPlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_OK].Value)
	            .Replace(CancelPlaceholder, localizationSettings.Texts[LocalizedTextId.BUTTON_CANCEL].Value)
	            .Replace(AbsBottomPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_ABSBOTTOM].Value)
	            .Replace(AbsMiddlePlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_ABSMIDDLE].Value)
	            .Replace(BaselinePlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_BASELINE].Value)
	            .Replace(BottomPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_BOTTOM].Value)
	            .Replace(LeftPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_LEFT].Value)
	            .Replace(MiddlePlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_MIDDLE].Value)
	            .Replace(RightPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_RIGHT].Value)
	            .Replace(TextTopPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_TEXTTOP].Value)
	            .Replace(TopPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_TOP].Value)
	            .Replace(CenterTopPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_CENTER_TOP].Value)
	            .Replace(CenterCenterPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_CENTER_CENTER].Value)
	            .Replace(CenterBottomPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_CENTER_BOTTOM].Value)
	            .Replace(LeftTopPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_LEFT_TOP].Value)
	            .Replace(LeftCenterPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_LEFT_CENTER].Value)
	            .Replace(LeftBottomPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_LEFT_BOTTOM].Value)
	            .Replace(RightTopPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_RIGHT_TOP].Value)
	            .Replace(RightCenterPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_RIGHT_CENTER].Value)
	            .Replace(RightBottomPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_RIGHT_BOTTOM].Value)
	            .Replace(LowPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_LOW].Value)
	            .Replace(MediumPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_MEDIUM].Value)
	            .Replace(HighPlaceholder, localizationSettings.Texts[LocalizedTextId.LABEL_HIGH].Value);
	    }

        private static void GuardNotNull<T>(T item, string itemName)
            where T : class
	    {
	        if (item == null)
	        {
	            throw new ArgumentNullException(itemName);
	        }
        }

	    // .NET 2.0 does not have such a method.
	    public static bool IsNullOrWhiteSpace(string value)
	    {
	        if (value == null)
	        {
	            return true;
	        }

	        for (var index = 0; index < value.Length; index++)
	        {
	            if (!Char.IsWhiteSpace(value[index]))
	            {
	                return false;
	            }
	        }

	        return true;
	    }
    }
}
