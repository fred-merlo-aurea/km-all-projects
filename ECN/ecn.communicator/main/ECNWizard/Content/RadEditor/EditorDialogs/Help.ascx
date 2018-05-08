<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Widgets" TagPrefix="widgets" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Dialogs" TagPrefix="dialogs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<style type="text/css">
	td {
		text-align: center;
	}
	
	.reDescriptionCell {
		text-align: left;
	}
	
	.reLightweightDialog td {
		width: 50px;
	}
	.reLightweightDialog td span.reTool {
		display: none;
	}

	.reClassicDialog td a.reTool {
		display: none;
	}
</style>
<table cellpadding="0" border="0" cellspacing="0" class="reDialog HelpDialog" style="width: 700px; display: block; margin: 4px;">
	<tr>
		<td style="text-align: left; padding: 8px 0">
			<h1>RadEditor for ASP.NET AJAX</h1>
			<h2>Help Topics</h2>
		</td>
	</tr>
	<tr>
		<td style="text-align: left;">
			<div class="helpTopics">
				<!-- help topics go here -->
				<table cellpadding="0" cellspacing="0" class="helpTable bottomBorder">
					<tr>
						<th style="width: 70px;">Icon </th>
						<th style="width: 450px;">Description </th>
						<th>Shortcut</th>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="StyleBuilder">&nbsp;</span></span>
							<a class="reTool reStyleBuilder reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Style Builder - Allows the user to apply styles to the currently selected element.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="XhtmlValidator">&nbsp;</span></span>
							<a class="reTool reXhtmlValidator reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">XhtmlValidator - Uses the W3C XHTML Validator Page to perform validation of the
							current editor content.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="ConvertToUpper">&nbsp;</span></span>
							<a class="reTool reConvertToUpper reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">ConvertToUpper - Convert the text of the current selection to upper case, preserving
							the non-text elements such as images and tables.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="ConvertToLower">&nbsp;</span></span>
							<a class="reTool reConvertToLower reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">ConvertToLower - Convert the text of the current selection to lower case, preserving
							the non-text elements such as images and tables. </td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="ImageMapDialog">&nbsp;</span></span>
							<a class="reTool reImageMapDialog reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">ImageMapDialog - Allow users to create image maps through draging over the images
							and creating hyperlink areas of different shapes. </td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="FormatCodeBlock">&nbsp;</span></span>
							<a class="reTool reFormatCodeBlock reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">FormatCodeBlock - Allow users to insert and format code blocks into the content. </td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<select disabled="disabled">
								<option>&nbsp;</option>
							</select>
						</td>
						<td class="reDescriptionCell">RealFontSize - Allows the user to apply to the current selection font size measured
							in pixels, rather than a fixed-size 1 to 7 (as does the FontSize tool). </td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="ToggleScreenMode">&nbsp;</span></span>
							<a class="reTool reToggleScreenMode reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">ToggleScreenMode - Switches RadEditor into Full Screen Mode. </td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="ToggleTableBorder">&nbsp;</span></span>
							<a class="reTool reToggleTableBorder reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Show/Hide Border - Shows or hides borders around tables in the content area. </td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<select disabled="disabled">
								<option>&nbsp;</option>
							</select>
						</td>
						<td class="reDescriptionCell">Zoom - Changes the level of text magnification.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="ModuleManager">&nbsp;</span></span>
							<a class="reTool reModuleManager reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Module Manager - Activates /Deactivates modules from a drop-down list of available
							modules.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="FindAndReplace">&nbsp;</span></span>
							<a class="reTool reFindAndReplace reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Find and Replace - Find (and replaces) text in the editor's content area.</td>
						<td>Ctrl+F</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="Print">&nbsp;</span></span>
							<a class="reTool rePrint reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Print button - Prints the contents of the RadEditor or the whole web page. </td>
						<td>Ctrl+P</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="AjaxSpellCheck">&nbsp;</span></span>
							<a class="reTool reAjaxSpellCheck reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">AjaxSpellCheck - Launches the spellchecker. </td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="Cut">&nbsp;</span></span>
							<a class="reTool reCut reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Cut button - Cuts the selected content and copies it to the clipboard. </td>
						<td>Ctrl+X</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="Copy">&nbsp;</span></span>
							<a class="reTool reCopy reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Copy button - Copies the selected content to the clipboard. </td>
						<td>Ctrl+C</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="Paste">&nbsp;</span></span>
							<a class="reTool rePaste reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Paste button - Pastes the copied content from the clipboard into the editor. </td>
						<td>Ctrl+V</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="PasteFromWord">&nbsp;</span></span>
							<a class="reTool rePasteFromWord reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Paste from Word button - Pastes content copied from Word and removes the web-unfriendly
							tags. </td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="PasteFromWordNoFontsNoSizes">&nbsp;</span></span>
							<a class="reTool rePasteFromWordNoFontsNoSizes reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Paste from Word cleaning fonts and sizes button - cleans all Word-specific tags
							and removes font names and text sizes. </td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="PastePlainText">&nbsp;</span></span>
							<a class="reTool rePastePlainText reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Paste Plain Text button - Pastes plain text (no formatting) into the editor.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="PasteAsHtml">&nbsp;</span></span>
							<a class="reTool rePasteAsHtml reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Paste as HTML button - Pastes HTML code in the content area and keeps all the HTML
							tags.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="PasteHtml">&nbsp;</span></span>
							<a class="reTool rePasteHtml reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Paste HTML button - Pastes HTML content in to the editor.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="Undo">&nbsp;</span></span>
							<a class="reTool reUndo reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Undo button - Undoes the last action. </td>
						<td>Ctrl+Z</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="Redo">&nbsp;</span></span>
							<a class="reTool reRedo reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Redo button - Redoes/Repeats the last action, which has been undone. </td>
						<td>Ctrl+Y</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="FormatStripper">&nbsp;</span></span>
							<a class="reTool reFormatStripper reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Format Stripper button - Removes custom or all formatting from selected text. </td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="Help">&nbsp;</span></span>
							<a class="reTool reHelp reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Help - Launches the Quick Help you are currently viewing.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="AboutDialog">&nbsp;</span></span>
							<a class="reTool reAboutDialog reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">About Dialog - Shows the current version and credentials of RadEditor.</td>
						<td>-</td>
					</tr>
					<tr>
						<td colspan="3" align="center" valign="middle" style="padding-top: 10px">
							<strong>INSERT AND MANAGE LINKS, TABLES, SPECIAL CHARACTERS, IMAGES and MEDIA</strong>
						</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="ImageManager">&nbsp;</span></span>
							<a class="reTool reImageManager reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Image Manager button - Inserts an image from a predefined image folder(s).</td>
						<td>Ctrl+G</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="ImageMapDialog">&nbsp;</span></span>
							<a class="reTool reImageMapDialog reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Image map - Allows users to define clickable areas within image.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="AbsolutePosition">&nbsp;</span></span>
							<a class="reTool reAbsolutePosition reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Absolute Object Position button - Sets an absolute position of an object (free move).</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="InsertTable">&nbsp;</span></span>
							<a class="reTool reInsertTable reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Insert Table button - Inserts a table in the RadEditor.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="ToggleTableBorder">&nbsp;</span></span>
							<a class="reTool reToggleTableBorder reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Toggle Table Borders - Toggles borders of all tables within the editor.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="InsertSnippet">&nbsp;</span></span>
							<a class="reTool reInsertSnippet reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Insert Snippet - Inserts pre-defined code snippets.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="InsertFormElement">&nbsp;</span></span>
							<a class="reTool reInsertFormElement reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Insert Form Element - Inserts a form element from a drop-down list with available
							elements.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="InsertDate">&nbsp;</span></span>
							<a class="reTool reInsertDate reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Insert Date button - Inserts current date.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="InsertTime">&nbsp;</span></span>
							<a class="reTool reInsertTime reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Insert Time button - Inserts current time.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="FlashManager">&nbsp;</span></span>
							<a class="reTool reFlashManager reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Flash Manager button - Inserts a Flash animation and lets you set its properties.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="MediaManager">&nbsp;</span></span>
							<a class="reTool reMediaManager reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Windows Media Manager button - Inserts a Windows media object (AVI, MPEG, WAV, etc.)
							and lets you set its properties.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="DocumentManager">&nbsp;</span></span>
							<a class="reTool reDocumentManager reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Document Manager - Inserts a link to a document on the server (PDF, DOC, etc.)</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="LinkManager">&nbsp;</span></span>
							<a class="reTool reLinkManager reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Hyperlink Manager button - Makes the selected text or image a hyperlink.</td>
						<td>Ctrl+K</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="Unlink">&nbsp;</span></span>
							<a class="reTool reUnlink reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Remove Hyperlink button - Removes the hyperlink from the selected text or image.</td>
						<td>Ctrl+Shift+K</td>
					</tr>
					<tr>
						<td>
							<select disabled="disabled">
								<option>&nbsp;</option>
							</select>
						</td>
						<td class="reDescriptionCell">
							Insert Special Character dropdown - Inserts a special character (&euro; &reg;,
							<span style="font-family: Arial;">©, ±</span>, etc.)
						</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<select disabled="disabled">
								<option>&nbsp;</option>
							</select>
						</td>
						<td class="reDescriptionCell">Insert Custom Link dropdown - Inserts an internal or external link from a predefined
							list.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="TemplateManager">&nbsp;</span></span>
							<a class="reTool reTemplateManager reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Choose HTML Template - Applies and HTML template from a predefined list of templates.</td>
						<td>-</td>
					</tr>
					<tr>
						<td colspan="3" align="center" valign="middle" style="padding-top: 10px">
							<strong>CREATE, FORMAT AND EDIT PARAGRAPHS and LINES</strong>
						</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="InsertParagraph">&nbsp;</span></span>
							<a class="reTool reInsertParagraph reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Insert New Paragraph button - Inserts new paragraph.</td>
						<td>Ctrl+M</td>
					</tr>
					<tr>
						<td>
							<select disabled="disabled">
								<option>&nbsp;</option>
							</select>
						</td>
						<td class="reDescriptionCell">Paragraph Style Dropdown button - Applies standard text styles to selected text.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="Outdent">&nbsp;</span></span>
							<a class="reTool reOutdent reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Outdent button - Indents paragraphs to the left.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="Indent">&nbsp;</span></span>
							<a class="reTool reIndent reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Indent button - Indents paragraphs to the right.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="JustifyLeft">&nbsp;</span></span>
							<a class="reTool reJustifyLeft reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Align Left button - Aligns the selected paragraph to the left.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="JustifyCenter">&nbsp;</span></span>
							<a class="reTool reJustifyCenter reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Center button - Aligns the selected paragraph to the center.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="JustifyRight">&nbsp;</span></span>
							<a class="reTool reJustifyRight reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Align Right button - Aligns the selected paragraph to the right.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="JustifyFull">&nbsp;</span></span>
							<a class="reTool reJustifyFull reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Justify button - Justifies the selected paragraph.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="InsertUnorderedList">&nbsp;</span></span>
							<a class="reTool reInsertUnorderedList reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Bulleted List button - Creates a bulleted list from the selection.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="InsertOrderedList">&nbsp;</span></span>
							<a class="reTool reInsertOrderedList reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Numbered List button - Creates a numbered list from the selection.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="InsertHorizontalRule">&nbsp;</span></span>
							<a class="reTool reInsertHorizontalRule reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Insert horizontal line (e.g. horizontal rule) button - Inserts a horizontal line
							at the cursor position.</td>
						<td>-</td>
					</tr>
					<tr>
						<td colspan="3" align="center" valign="middle" style="padding-top: 10px">
							<strong>CREATE, FORMAT AND EDIT TEXT, FONT and LISTS</strong>
						</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="Bold">&nbsp;</span></span>
							<a class="reTool reBold reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Bold button - Applies bold formatting to selected text.</td>
						<td>Ctrl+B</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="Italic">&nbsp;</span></span>
							<a class="reTool reItalic reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Italic button - Applies italic formatting to selected text.</td>
						<td>Ctrl+I</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="Underline">&nbsp;</span></span>
							<a class="reTool reUnderline reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Underline button - Applies underline formatting to selected text.</td>
						<td>Ctrl+U</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="StrikeThrough">&nbsp;</span></span>
							<a class="reTool reStrikeThrough reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Strikethrough button - Applies strikethrough formatting to selected text.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="Superscript">&nbsp;</span></span>
							<a class="reTool reSuperscript reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Superscript button - Makes a text superscript.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="Subscript">&nbsp;</span></span>
							<a class="reTool reSubscript reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Subscript button - Makes a text subscript.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<select disabled="disabled">
								<option>&nbsp;</option>
							</select>
						</td>
						<td class="reDescriptionCell">Font Select button - Sets the font typeface.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<select disabled="disabled">
								<option>&nbsp;</option>
							</select>
						</td>
						<td class="reDescriptionCell">Font Size button - Sets the font size.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="ForeColor">&nbsp;</span></span>
							<a class="reTool reForeColor reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Text Color (foreground) button - Changes the foreground color of the selected text.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="BackColor">&nbsp;</span></span>
							<a class="reTool reBackColor reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Text Color (background) button - Changes the background color of the selected text.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<select disabled="disabled">
								<option>&nbsp;</option>
							</select>
						</td>
						<td class="reDescriptionCell">Apply class - applies custom, predefined styles to the selected text.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<span class="reTool"><span class="FormatCodeBlock">&nbsp;</span></span>
							<a class="reTool reFormatCodeBlock reToolIcon">&nbsp;</a>
						</td>
						<td class="reDescriptionCell">Format Code Block - Allow users to insert and format code blocks into the content.</td>
						<td>-</td>
					</tr>
					<tr>
						<td>
							<select disabled="disabled">
								<option>&nbsp;</option>
							</select>
						</td>
						<td class="reDescriptionCell">Custom Links dropdown - Inserts custom, predefined link.</td>
						<td>-</td>
					</tr>
				</table>
			</div>
		</td>
	</tr>
	<tr>
		<td class="reBottomcell">
			<table border="0" cellpadding="0" cellspacing="0" class="reConfirmCancelButtonsTbl">
				<tr>
					<td>
						<button type="button" onclick="javascript: Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close();" style="width: 75px;" id="OkButton">
							<script type="text/javascript">
								setInnerHtml("OkButton", localization["OK"]);
							</script>
						</button>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<div style="display: none;">
	<!-- hidden color picker will load the necessary css images -->
	<tools:ColorPicker ID="ColorPicker" runat="server"></tools:ColorPicker>
</div>