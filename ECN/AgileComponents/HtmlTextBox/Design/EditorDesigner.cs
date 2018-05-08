using System;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace ActiveUp.WebControls.Design
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
		private DesignerVerbCollection designerVerbs;

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

				if (editor.EditorModeSelector != EditorModeSelectorType.None)
				{
					writer.RenderBeginTag(HtmlTextWriterTag.Tr); // TR

					if (editor.MaxLength <= 0)
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Align, Left);
					}

					writer.RenderBeginTag(HtmlTextWriterTag.Td); // TD

					if (editor.EditorModeSelector == EditorModeSelectorType.CheckBox)
					{
						if (editor.StartupMode == EditorMode.Html)
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

			catch (Exception e)
			{
				return GetErrorDesignTimeHtml(e);
			}
		}

		private Editor GetDesignTimeHtmlHeader(HtmlTextWriter writer)
		{
			var editor = (Editor)Component;

			editor.ControlStyle.AddAttributesToRender(writer);
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE
			writer.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
			writer.RenderBeginTag(HtmlTextWriterTag.Td); // TD

			writer.AddAttribute(HtmlTextWriterAttribute.Style, "position: relative; display: block; width:100%; border: 0px;");

			editor.CreateTools(editor.Template, false);
			editor.Toolbars.RenderControl(writer);

			writer.RenderEndTag(); // TD
			writer.RenderEndTag(); // TR

			writer.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
			writer.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Td); // TD
			writer.AddAttribute(HtmlTextWriterAttribute.Style, "position: relative; display: block; width:100%; border: 0px;");
			writer.AddAttribute("onkeydown", $"keydown(\'{editor.ClientID}\');");

			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
			writer.AddAttribute(HtmlTextWriterAttribute.Style, "WIDTH: 100%; HEIGTH: 100%;");

			writer.RenderBeginTag(HtmlTextWriterTag.Iframe); // IFRAME
			writer.RenderEndTag(); // IFRAME

			writer.RenderEndTag(); // TD
			writer.RenderEndTag(); // TR
			return editor;
		}

		private static void SelectorNotCheckBox(HtmlTextWriter writer, Editor editor)
		{
			SelectorNotCheckBoxHeader(writer, editor);

			if (editor.StartupMode == EditorMode.Html)
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

			if (!editor.TextMode)
			{
				writer.Write(NonBreakingSpace);
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
				writer.AddAttribute(HtmlTextWriterAttribute.Src, editor.IconsDirectory + editor.EditorModeHtmlIcon);
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

			if (editor.StartupMode == EditorMode.Preview)
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

			if (!editor.TextMode)
			{
				writer.Write(NonBreakingSpace);
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
				writer.AddAttribute(HtmlTextWriterAttribute.Src, editor.IconsDirectory + editor.EditorModePreviewIcon);
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

			SelectorNotCheckBoxMaxLength(writer, editor);
		}

		private static void SelectorNotCheckBoxMaxLength(HtmlTextWriter writer, Editor editor)
		{
			if(editor.MaxLength > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
				writer.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE 1
				writer.RenderBeginTag(HtmlTextWriterTag.Tr); // TR 1
				writer.RenderBeginTag(HtmlTextWriterTag.Td); // TD 1
				writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
				writer.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE 2
				writer.RenderBeginTag(HtmlTextWriterTag.Tr); // TR 2
				writer.RenderBeginTag(HtmlTextWriterTag.Td); // TD 2
				writer.AddStyleAttribute("font-family", "verdana");
				writer.AddStyleAttribute("font-size", "11px");
				writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
				writer.RenderBeginTag(HtmlTextWriterTag.Span);
				writer.Write("Counter&nbsp;&nbsp;");
				writer.RenderEndTag();

				var valCounter = editor.MaxLength;
				if(editor.Text != null)
				{
					valCounter = editor.Text.Length - editor.MaxLength;
				}

				writer.AddAttribute(HtmlTextWriterAttribute.Value, valCounter.ToString());
				writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "50px");
				writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "20px");
				writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
				writer.RenderBeginTag(HtmlTextWriterTag.Input);
				writer.RenderEndTag();
				writer.RenderEndTag(); // TD 2
				writer.RenderEndTag(); // TR 2
				writer.RenderEndTag(); // TABLE 2
				writer.RenderEndTag(); // TD 1
				writer.RenderEndTag(); // TR 1
				writer.RenderEndTag(); // TABLE 1

				writer.RenderEndTag();
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
		}

		private static void SelectorNotCheckBoxHeader(HtmlTextWriter writer, Editor editor)
		{
			if (editor.MaxLength > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Align, Left);
				writer.RenderBeginTag(HtmlTextWriterTag.Table);
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
			}

			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "2");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE
			writer.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
			writer.AddAttribute(HtmlTextWriterAttribute.Align, Left);
			writer.RenderBeginTag(HtmlTextWriterTag.Td); // TD

			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");

			if (editor.StartupMode == EditorMode.Design)
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

			if (!editor.TextMode)
			{
				writer.Write(NonBreakingSpace);
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
				writer.AddAttribute(HtmlTextWriterAttribute.Src, editor.IconsDirectory + editor.EditorModeDesignIcon);
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

		/// <summary>
		/// Gets the design-time verbs supported by the component that is associated with the designer.
		/// </summary>
		/// <value></value>
		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				if (designerVerbs == null) 
				{
					designerVerbs = new DesignerVerbCollection();
					designerVerbs.Add(new DesignerVerb("Property Builder...", new EventHandler(this.OnPropertyBuilder)));
				}

				return designerVerbs;
			}
		}

		private void OnPropertyBuilder(object sender, EventArgs e) 
		{
			EditorComponentEditor compEditor = new EditorComponentEditor();
			compEditor.EditComponent(Component);
		}

		/// <summary>
		/// Initializes the designer and
		/// loads the specified component.
		/// </summary>
		/// <param name="component">The control element being designed.</param>
		public override void Initialize(IComponent component) 
		{
			if (!(component is ActiveUp.WebControls.Editor)) 
			{
				throw new ArgumentException("Component must be a ActiveUp.WebControls.Editor control.", "component");
			}
			base.Initialize(component);
		}
	}
}
