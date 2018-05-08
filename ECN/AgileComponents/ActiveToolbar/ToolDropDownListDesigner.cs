using System;
using System.Drawing;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Collections;

namespace ActiveUp.WebControls
{
	#region class DropDownListDesigner

	/// <summary>
	/// Designer of the <see cref="DropDownList"/> control.
	/// </summary>
	[Serializable]
	public class ToolDropDownListDesigner : ControlDesigner
	{
		#region Constructor

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ToolDropDownListDesigner()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a value indicating whether the control can be resized.
		/// </summary>
		public override bool AllowResize
		{
			get { return false; }
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
				ToolDropDownList ddl = (ToolDropDownList)base.Component;

				StringWriter stringWriter = new StringWriter();
				HtmlTextWriter writer = new HtmlTextWriter(stringWriter);

				DesignDropDownList(ref writer, ddl);

				/*StreamWriter sw = new StreamWriter(@"c:\temp\render.txt", false);
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
		/// Gets the HTML that is used to represent an empty control at design time.
		/// </summary>
		/// <returns>The HTML that is used to represent an empty control at design time. By default, this HTML contains the name of the component.</returns>
		protected override string GetEmptyDesignTimeHtml()
		{
			string text;
			text = "this should be never displayed.";
			return CreatePlaceHolderDesignTimeHtml(text);
		}

		/// <summary>
		/// Gets the HTML that provides information about the specified exception. This method is typically called after an error has been encountered at design time.
		/// </summary>
		/// <param name="e">The exception that occurred.</param>
		/// <returns>The HTML for the specified exception.</returns>
		protected override string GetErrorDesignTimeHtml(System.Exception e)
		{
			string text = string.Format("There was an error and the DropDownList control can't be displayed<br>Exception : {0}", e.Message);
			return this.CreatePlaceHolderDesignTimeHtml(text);
		}

		/// <summary>
		/// Create a DropDownList object at design time.
		/// </summary>
		/// <param name="output">Output stream that contains the HTML used to represent the control.</param>
		/// <param name="ddl"><see cref="DropDownList"/> object to design.</param>
		/// 
		public static void DesignDropDownList(ref HtmlTextWriter output, ToolDropDownList ddl)
		{
			IEnumerator enumerator = ddl.Style.Keys.GetEnumerator();
			while(enumerator.MoveNext())
			{
				output.AddStyleAttribute((string)enumerator.Current, ddl.Style[(string)enumerator.Current]);
			}
			ddl.ControlStyle.AddAttributesToRender(output);

			string backImage = string.Empty;
			if (ddl.BackImage != string.Empty)
			{
				if (ddl.Parent != null && ddl.Parent is ActiveUp.WebControls.Toolbar)
					backImage = "url(" + Utils.ConvertToImageDir(((Toolbar)ddl.Parent).ImagesDirectory,ddl.BackImage) + ")";
				else
					backImage = "url(" + ddl.BackImage + ")";
			}

			string style = "style=\"";
			if (ddl.BackColor != Color.Empty)
				style+= string.Format("background-color:{0};",Utils.Color2Hex(ddl.BackColor));
			if (ddl.BorderColor != Color.Empty)
				style+= string.Format("border-color:{0};",Utils.Color2Hex(ddl.BorderColor));
			style += string.Format("border-style:{0};",ddl.BorderStyle.ToString());
			if (ddl.BorderWidth != Unit.Empty)
				style += string.Format("border-width:{0};",ddl.BorderWidth);
			if (backImage != string.Empty)
				style += string.Format("background-image:{0};",backImage);
			style += "\"";

			string table = string.Empty;
			table += "<table";
			if (ddl.Width != Unit.Empty)
				table += string.Format(" width={0}",ddl.Width.ToString());
			if (ddl.Height != Unit.Empty)
				table += string.Format(" height={0}",ddl.Height.ToString());
			table += " cellpadding=0 cellspacing=0";
			table += string.Format(" {0}",style);
			table += ">";
			output.Write(table);

			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.Write("\n");

			if (ddl.Width.IsEmpty == true)
				output.AddAttribute(HtmlTextWriterAttribute.Width,"100%");
			else
				output.AddAttribute(HtmlTextWriterAttribute.Width,ddl.Width.Value.ToString());
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.AddAttribute(HtmlTextWriterAttribute.Width,"100%");
			output.AddAttribute(HtmlTextWriterAttribute.Height,"100%"); 
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Border,"0");
			output.RenderBeginTag(HtmlTextWriterTag.Table);

			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.AddAttribute(HtmlTextWriterAttribute.Width,"100%"); 
			output.AddStyleAttribute("padding","0px 0px 0px 4px");
			output.AddAttribute(HtmlTextWriterAttribute.Nowrap,null);
			if (ddl.ForeColor != Color.Empty)
				output.AddStyleAttribute(HtmlTextWriterStyle.Color,Utils.Color2Hex(ddl.ForeColor));
			Utils.AddStyleFontAttribute(output,ddl.Font);
			output.RenderBeginTag(HtmlTextWriterTag.Td);

            string text = string.Empty;
            if (ddl.SelectedIndex == -1)
				text = ddl.Text.TrimEnd();
			else if (ddl.ChangeToSelectedText == SelectedText.Text)
				text = ddl.Items[ddl.SelectedIndex].Text;
			else if (ddl.ChangeToSelectedText == SelectedText.Value)
				text = ddl.Items[ddl.SelectedIndex].Value;

            if (text.IndexOf("$IMAGESDIRECTORY$") >= 0)
            {
                string imagesDirectory = string.Empty;
                if (ddl.Parent != null && ddl.Parent is ActiveUp.WebControls.Toolbar)
                {
                    imagesDirectory = Utils.GetCorrectImageDir(((Toolbar)ddl.Parent).ImagesDirectory);
                }

                text = text.Replace("$IMAGESDIRECTORY$", imagesDirectory);
            }
            //text = ddl.Text;
            output.Write(text);

			output.RenderEndTag();
			output.AddAttribute(HtmlTextWriterAttribute.Height,"100%");
			output.RenderBeginTag(HtmlTextWriterTag.Td);

			// drop down image
			output.AddAttribute(HtmlTextWriterAttribute.Width,"100%");
			output.AddAttribute(HtmlTextWriterAttribute.Height,"100%");
			
			// Added by PMENGAL
			output.AddAttribute(HtmlTextWriterAttribute.Width,"100%");
			output.AddAttribute(HtmlTextWriterAttribute.Height,"100%");
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0");
			output.RenderBeginTag(HtmlTextWriterTag.Table);
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.AddAttribute(HtmlTextWriterAttribute.Height,"100%");
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0");
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.AddAttribute(HtmlTextWriterAttribute.Border,"0");
			if (ddl.DropDownImage != string.Empty)
			{
				if (ddl.Parent != null && ddl.Parent is ActiveUp.WebControls.Toolbar)
                    output.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertToImageDir(((Toolbar)ddl.Parent).ImagesDirectory, ddl.DropDownImage, "down.gif", ddl.Page, ddl.GetType()));
				else
					output.AddAttribute(HtmlTextWriterAttribute.Src,ddl.DropDownImage);
			}
			else
			{
                if (ddl.Parent != null && ddl.Parent is ActiveUp.WebControls.Toolbar)
                {
#if (FX1_1)
                        ddl.DropDownImage = "down.gif";
#endif
                    output.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertToImageDir(((Toolbar)ddl.Parent).ImagesDirectory, ddl.DropDownImage, "down.gif", ddl.Page, ddl.GetType()));
                }
                else
                    output.AddAttribute(HtmlTextWriterAttribute.Src, "down.gif");
			}

			output.RenderBeginTag(HtmlTextWriterTag.Img);
			output.RenderEndTag();

			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
			
			output.Write("</table>");
						
		}


		#endregion

	}

	#endregion
}
