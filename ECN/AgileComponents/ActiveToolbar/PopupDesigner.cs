using System;
using System.Drawing;
using System.IO;
using System.Web.UI;
using ActiveUp.WebControls.ActiveToolbar;

namespace ActiveUp.WebControls
{
    #region class PopupDesigner

    /// <summary>
    /// Designer of the <see cref="Popup"/> control.
    /// </summary>
    [Serializable]
	internal class PopupDesigner : CoreControlDesigner
    {
		#region Constrcutor

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PopupDesigner()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a value indicating whether the control can be resized.
		/// </summary>
		public override bool AllowResize
		{
			get {return true;}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the HTML that is used to represent the control at design time.
		/// </summary>
		/// <returns>The HTML that is used to represent the control at design time.</returns>
		public override string GetDesignTimeHtml()
		{
			try
			{
				Popup popup = (Popup)base.Component;

				StringWriter stringWriter = new StringWriter();
				HtmlTextWriter writer = new HtmlTextWriter(stringWriter);

				DesignPopup(ref writer, popup);

				/*StreamWriter sw = new StreamWriter(@"c:\temp\render1.txt",false);
				sw.Write(stringWriter.ToString());
				sw.Close();*/

				return stringWriter.ToString();

			}

			catch (Exception e)
			{
				return this.GetErrorDesignTimeHtml(e);
			}
		}

		/// <summary>
		/// Gets the HTML that provides information about the specified exception. This method is typically called after an error has been encountered at design time.
		/// </summary>
		/// <param name="e">The exception that occurred.</param>
		/// <returns>The HTML for the specified exception.</returns>
		protected override string GetErrorDesignTimeHtml(System.Exception e)
		{
			string text = string.Format("There was an error and the Popup control can't be displayed<br>Exception : {0}",e.Message);
			return this.CreatePlaceHolderDesignTimeHtml(text);
		}

		/// <summary>
		/// Create a Popup object at design time.
		/// </summary>
		/// <param name="writer">Output stream that contains the HTML used to represent the control.</param>
		/// <param name="popup"><see cref="Popup"/> object to design.</param>
		public static void DesignPopup(ref HtmlTextWriter writer, Popup popup)
		{
			if (popup.ShowWindow == true)
			{
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(popup.LayoutWindow.BackColor));
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(popup.LayoutWindow.BorderColor));
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, popup.LayoutWindow.BorderStyle.ToString());
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, popup.LayoutWindow.BorderWidth.ToString());
				writer.AddStyleAttribute(HtmlTextWriterStyle.Width, popup.Width.ToString());
				if (popup.ShowTitle == false)
					writer.AddStyleAttribute(HtmlTextWriterStyle.Height, (popup.Height.Value - 20).ToString());
				else
					writer.AddStyleAttribute(HtmlTextWriterStyle.Height, popup.Height.ToString());
				writer.AddStyleAttribute("padding","0px 0px 0px 0px");
				writer.AddStyleAttribute("left","0px");
				writer.AddStyleAttribute("top","0px");
				writer.RenderBeginTag(HtmlTextWriterTag.Div);

				if (popup.ShowTitle == true)
				{
					//writer.AddStyleAttribute(HtmlTextWriterStyle.Width, ((int)(popup.Width.Value - 7)).ToString() + "px");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
					if (popup.LayoutTitle.Height.IsEmpty)
							writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "20px");
						else
							writer.AddStyleAttribute(HtmlTextWriterStyle.Height, popup.LayoutTitle.Height.ToString());
					writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(popup.LayoutTitle.BackColor));
					writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(popup.LayoutTitle.BorderColor));
					writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, popup.LayoutTitle.BorderStyle.ToString());
					writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, popup.LayoutTitle.BorderWidth.ToString());
                    if (popup.LayoutTitle.BackGradientFirstColor != Color.Empty && popup.LayoutTitle.BackGradientLastColor != Color.Empty)
						writer.AddStyleAttribute("filter",string.Format("progid:DXImageTransform.Microsoft.Gradient(endColorstr='{0}', startColorstr='{1}', gradientType='1')",Utils.Color2Hex(popup.LayoutTitle.BackGradientLastColor),Utils.Color2Hex(popup.LayoutTitle.BackGradientFirstColor)));
					Utils.AddStyleFontAttribute(writer,popup.LayoutTitle.Font);
					writer.AddStyleAttribute("left", "3px");
					writer.AddStyleAttribute("top", "3px");
					writer.AddStyleAttribute("padding","0px 0px 0px 0px");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                    writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                    writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
					writer.RenderBeginTag(HtmlTextWriterTag.Table); // OPEN TABLE
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr); // OPEN TR

					Utils.AddStyleFontAttribute(writer, popup.LayoutTitle.Font);
					if (!popup.LayoutTitle.ForeColor.IsEmpty)
						writer.AddStyleAttribute(HtmlTextWriterStyle.Color,Utils.Color2Hex(popup.LayoutTitle.ForeColor));
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					if (popup.TitleText != string.Empty)
						writer.Write(popup.TitleText);
					else
						writer.Write("&nbsp;");
					writer.RenderEndTag();

                    writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
                    writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.AddAttribute(HtmlTextWriterAttribute.Width,"16px");
					writer.AddAttribute(HtmlTextWriterAttribute.Height,"16px");
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertToImageDir(popup.ImagesDirectory, popup.CloseImage, "close.gif", popup.Page, popup.GetType()));
					writer.RenderBeginTag(HtmlTextWriterTag.Img);
					writer.RenderEndTag();
					writer.RenderEndTag();

          writer.RenderEndTag(); // CLOSE TR
					writer.RenderEndTag(); // CLOSE TABLE
          writer.RenderEndTag();
				}
			}

			writer.AddStyleAttribute("overflow", "auto");
			writer.AddStyleAttribute("scrollbar-base-color",Utils.Color2Hex(popup.LayoutContent.ScrollBarColor));
			writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(popup.LayoutContent.BackColor));
			writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(popup.LayoutContent.BorderColor));
			writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, popup.LayoutContent.BorderStyle.ToString());
			writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, popup.LayoutContent.BorderWidth.Value.ToString() + "px");
			writer.AddStyleAttribute(HtmlTextWriterStyle.Width,((int)(popup.Width.Value - 7)).ToString() + "px");
			writer.AddStyleAttribute(HtmlTextWriterStyle.Height,((int)(popup.Height.Value - 30)).ToString() + "px");
			writer.AddStyleAttribute("left", "2px");
			if (popup.ShowTitle == false || popup.ShowWindow == false)
				writer.AddStyleAttribute("top", "4px");
			else
				writer.AddStyleAttribute("top", "24px");
			writer.AddStyleAttribute("padding","0px 2px 0px 4px");
			Utils.AddStyleFontAttribute(writer, popup.LayoutContent.Font);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.Write(popup.ContentText);
			writer.RenderEndTag(); 
			
			if (popup.ShowWindow == true)
				writer.RenderEndTag();
		}


		#endregion

	}

	#endregion
}
