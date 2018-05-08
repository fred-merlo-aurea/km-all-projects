using System;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using HtmlTextWriter = System.Web.UI.HtmlTextWriter;

namespace ActiveUp.WebControls
{

	/// <summary>
	/// Represents a <see cref="EditorDesigner"/> object.
	/// </summary>
	[Serializable]
	public class EditorDesigner : ControlDesigner
	{
		private const string Left = "left";
		private const string CheckBoxName = "checkbox";
		private const string NonBreakingSpace = "&nbsp;";
		private const string TabPreview = "tab_preview.gif";
		private const string TabHtml = "tab_html.gif";
		private const string TabDesign = "tab_design.gif";

		/// <summary>
		/// Initializes a new instance of the <see cref="EditorDesigner"/> class.
		/// </summary>
		public EditorDesigner()
		{
		}

		/// <summary>
		/// Gets the HTML that is used to represent the control at design time.
		/// </summary>
		/// <returns>The HTML that is used to represent the control at design time.</returns>
		public override string GetDesignTimeHtml()
		{
			try
			{
				var stringWriter = new StringWriter();
				var writer = new HtmlTextWriter(stringWriter);
				var editor = GetDesignTimeHtmlHeader(writer);

				if(editor.EditorModeSelector != EditorModeSelectorType.None)
				{
					writer.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
					writer.AddAttribute(HtmlTextWriterAttribute.Align, Left);
					writer.RenderBeginTag(HtmlTextWriterTag.Td); // TD

					if(editor.EditorModeSelector == EditorModeSelectorType.CheckBox)
					{
						if(editor.StartupMode == EditorMode.Html)
						{
							writer.AddAttribute(HtmlTextWriterAttribute.Checked, string.Empty);
						}

						writer.AddAttribute(HtmlTextWriterAttribute.Type, CheckBoxName);

						writer.RenderBeginTag(HtmlTextWriterTag.Input); // INPUT
						writer.Write(editor.EditorModeHtmlLabel);
						writer.RenderEndTag(); // INPUT
					}
					else
					{
						SelectorNotCheckBox(writer, editor);
					}

					writer.RenderEndTag(); // TD
					writer.RenderEndTag(); // TR
				}

				writer.RenderEndTag(); // TABLE

				return stringWriter.ToString();
			}

			catch(Exception e)
			{
				return GetErrorDesignTimeHtml(e);
			}
		}

		private static void SelectorNotCheckBox(HtmlTextWriter writer, Editor editor)
		{
			SelectorNotCheckBoxHeader(writer, editor);

			if(editor.StartupMode == EditorMode.Html)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1");
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, BorderStyle.Solid.ToString());
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(Color.Black));
				writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(Color.White));
			}
			else
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "0");
			}

			writer.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE
			writer.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
			writer.RenderBeginTag(HtmlTextWriterTag.Td); // TD
			writer.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "verdana");
			writer.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "11px");
			writer.RenderBeginTag(HtmlTextWriterTag.Span); // SPAN

			if(!editor.TextMode)
			{
				writer.Write(NonBreakingSpace);
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
				writer.AddAttribute(HtmlTextWriterAttribute.Src,
					Utils.ConvertToImageDir(
						editor.IconsDirectory == "/" 
							? editor.IconsDirectory = string.Empty 
							: editor.IconsDirectory,
						editor.EditorModeHtmlIcon, TabHtml, editor.Page, editor.GetType()));
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
				writer.Write(NonBreakingSpace);
			}

			SelectorNotCheckBoxFooter(writer, editor);
		}

		private static void SelectorNotCheckBoxFooter(HtmlTextWriter writer, Editor editor)
		{
			writer.Write(editor.EditorModeHtmlLabel);
			writer.Write(NonBreakingSpace);
			writer.RenderEndTag(); // SPAN
			writer.RenderEndTag(); // TD
			writer.RenderEndTag(); // TR
			writer.RenderEndTag(); // TABLE
			writer.RenderEndTag(); // TD

			writer.RenderBeginTag(HtmlTextWriterTag.Td); // TD

			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");

			if(editor.StartupMode == EditorMode.Preview)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1");
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, BorderStyle.Solid.ToString());
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(Color.Black));
				writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(Color.White));
			}
			else
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "0");
			}

			writer.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE
			writer.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
			writer.RenderBeginTag(HtmlTextWriterTag.Td); // TD
			writer.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "verdana");
			writer.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "11px");
			writer.RenderBeginTag(HtmlTextWriterTag.Span); // SPAN

			if(!editor.TextMode)
			{
				writer.Write(NonBreakingSpace);
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
				writer.AddAttribute(HtmlTextWriterAttribute.Src,
					Utils.ConvertToImageDir(
						editor.IconsDirectory == "/" 
							? editor.IconsDirectory = string.Empty 
							: editor.IconsDirectory,
						editor.EditorModePreviewIcon, TabPreview, editor.Page, editor.GetType()));
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
				writer.Write(NonBreakingSpace);
			}

			writer.Write(editor.EditorModePreviewLabel);
			writer.Write(NonBreakingSpace);
			writer.RenderEndTag(); // SPAN
			writer.RenderEndTag(); // TD
			writer.RenderEndTag(); // TR
			writer.RenderEndTag(); // TABLE
			writer.RenderEndTag(); // TD

			writer.RenderEndTag(); // TR
			writer.RenderEndTag(); // TABLE
		}

		private static void SelectorNotCheckBoxHeader(HtmlTextWriter writer, Editor editor)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "2");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE
			writer.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
			writer.AddAttribute(HtmlTextWriterAttribute.Align, Left);
			writer.RenderBeginTag(HtmlTextWriterTag.Td); // TD

			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");

			if(editor.StartupMode == EditorMode.Design)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1");
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, BorderStyle.Solid.ToString());
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(Color.Black));
				writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(Color.White));
			}
			else
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "0");
			}

			writer.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE
			writer.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
			writer.RenderBeginTag(HtmlTextWriterTag.Td); // TD
			writer.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "verdana");
			writer.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "11px");
			writer.RenderBeginTag(HtmlTextWriterTag.Span); // SPAN

			if(!editor.TextMode)
			{
				writer.Write(NonBreakingSpace);
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
				writer.AddAttribute(HtmlTextWriterAttribute.Src,
					Utils.ConvertToImageDir(
						editor.IconsDirectory == "/" 
							? editor.IconsDirectory = string.Empty 
							: editor.IconsDirectory,
						editor.EditorModeDesignIcon, TabDesign, editor.Page, editor.GetType()));
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
				writer.Write(NonBreakingSpace);
			}

			writer.Write(editor.EditorModeDesignLabel);
			writer.Write(NonBreakingSpace);
			writer.RenderEndTag(); // SPAN
			writer.RenderEndTag(); // TD
			writer.RenderEndTag(); // TR
			writer.RenderEndTag(); // TABLE

			writer.RenderEndTag(); // TD
			writer.RenderBeginTag(HtmlTextWriterTag.Td); // TD

			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
		}

		private Editor GetDesignTimeHtmlHeader(HtmlTextWriter writer)
		{
			var editor = (Editor) Component;

			editor.ControlStyle.AddAttributesToRender(writer);
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE
			writer.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
			writer.RenderBeginTag(HtmlTextWriterTag.Td); // TD

			writer.AddAttribute(HtmlTextWriterAttribute.Style, "position: relative; display: block; width:100%; border: 0px;");

			editor.CreateTools(editor.Template, false);
			editor.Toolbars.RenderDesign(writer);

			writer.RenderEndTag(); // TD
			writer.RenderEndTag(); // TR

			writer.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
			writer.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Td); // TD
			writer.AddAttribute(HtmlTextWriterAttribute.Style, "position: relative; display: block; width:100%; border: 0px;");
			writer.AddAttribute("onkeydown", "keydown('" + editor.ClientID + "');");

			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
			writer.AddAttribute(HtmlTextWriterAttribute.Style, "WIDTH: 100%; HEIGTH: 100%;");

			writer.RenderBeginTag(HtmlTextWriterTag.Iframe); // IFRAME
			writer.RenderEndTag(); // IFRAME

			writer.RenderEndTag(); // TD
			writer.RenderEndTag(); // TR
			return editor;
		}

		/// <summary>
		/// Gets the HTML that provides information about the specified exception. This method is typically called after an error has been encountered at design time.
		/// </summary>
		/// <param name="e">The exception that occurred.</param>
		/// <returns>The HTML for the specified exception.</returns>
		protected override string GetErrorDesignTimeHtml(System.Exception e)
		{
			string text = string.Format("There was an error and the Editor control can't be displayed<br>Exception : {0}",e);
			return this.CreatePlaceHolderDesignTimeHtml(text);
		}

	}
}
