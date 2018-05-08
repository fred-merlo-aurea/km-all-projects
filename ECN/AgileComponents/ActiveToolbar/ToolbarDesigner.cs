using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Collections;
using System.Drawing;

namespace ActiveUp.WebControls
{
	#region class ToolbarDesigner

	/// <summary>
	/// Designer of the <see cref="Toolbar"/> control.
	/// </summary>
	[Serializable]
	internal class ToolbarDesigner : ControlDesigner
	{
		#region Constrcutor

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ToolbarDesigner()
		{
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
				Toolbar toolbar = (Toolbar)base.Component;
				
				if (toolbar.Tools.Count == 0)
					return CreatePlaceHolderDesignTimeHtml("Please add items through the Tools property in the property pane.");

				StringWriter stringWriter = new StringWriter();
				HtmlTextWriter output = new HtmlTextWriter(stringWriter);

				DesignToolbar(ref output, toolbar);

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
			string text = string.Format("There was an error and the Toolbar control can't be displayed<br>Exception : {0}",e.Message);
			return this.CreatePlaceHolderDesignTimeHtml(text);
		}

		public override bool AllowResize
		{
			get {return false;}
		}

		/// <summary>
		/// Create a Toolbar object at design time.
		/// </summary>
		/// <param name="output">The output.</param>
		/// <param name="toolbar">The toolbar.</param>
		public static void DesignToolbar(ref HtmlTextWriter output, Toolbar toolbar)
		{
            /*StringWriter stringWriter = new StringWriter();
            output = new HtmlTextWriter(stringWriter);*/

			IEnumerator enumerator = toolbar.Style.Keys.GetEnumerator();
				while(enumerator.MoveNext())
				{
					string val = (string)enumerator.Current;
					if (val.ToUpper() == "LEFT" && toolbar.Left == Unit.Empty)
						toolbar.Left = (new Unit(toolbar.Style[val]));
					else if (val.ToUpper() == "TOP" && toolbar.Top == Unit.Empty)
						toolbar.Top = (new Unit(toolbar.Style[val]));
					else
						output.AddStyleAttribute((string)enumerator.Current, toolbar.Style[(string)enumerator.Current]);
				}
				if (toolbar.Left != Unit.Empty)
					output.AddStyleAttribute("left",toolbar.Left.ToString());
				if (toolbar.Top != Unit.Empty)
					output.AddStyleAttribute("top",toolbar.Top.ToString());

				toolbar.ControlStyle.AddAttributesToRender(output);

				output.AddStyleAttribute("position",toolbar.Position.ToString());
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth,"0px");
				output.AddAttribute(HtmlTextWriterAttribute.Id, toolbar.UniqueID);
				output.RenderBeginTag(HtmlTextWriterTag.Div);
				/*output.AddAttribute("ondragstart", "return false");
				output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, toolbar.CellPadding.ToString());
				output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, toolbar.CellSpacing.ToString());
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(toolbar.BorderColor));
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, toolbar.BorderStyle.ToString());
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, toolbar.BorderWidth.ToString());
				output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(toolbar.BackColor));
				if (toolbar.BackImage != string.Empty)
					output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + Utils.ConvertToImageDir(toolbar.ImagesDirectory,toolbar.BackImage) + ")");
				output.RenderBeginTag(HtmlTextWriterTag.Table); */

                string style = string.Empty;
                style += "style=\"";
                if (toolbar.BorderColor != Color.Empty)
                    style += string.Format("border-color:{0};", Utils.Color2Hex(toolbar.BorderColor));
                style += string.Format("border-style:{0};", toolbar.BorderStyle.ToString());
                if (toolbar.BorderWidth != Unit.Empty)
                    style += string.Format("border-width:{0};", toolbar.BorderWidth.ToString());
                if (toolbar.BackColor != Color.Empty)
                    style += string.Format("background-color:{0};", Utils.Color2Hex(toolbar.BackColor));
                if (toolbar.BackImage != string.Empty)
                    style += string.Format("background-image:url({0});", Utils.ConvertToImageDir(toolbar.ImagesDirectory, toolbar.BackImage));
                style += "\"";

                string table = string.Empty;
                table += "<table";
                table += string.Format(" cellpadding={0} cellspacing={1}", toolbar.CellPadding, toolbar.CellSpacing);
                table += " ondragstart=return false;";
                table += string.Format(" {0}", style);
                table += ">";
                output.Write(table);

				if (toolbar.Direction == ToolbarDirection.Horizontal)
					output.RenderBeginTag(HtmlTextWriterTag.Tr); 
		
				if (toolbar.Dragable)
				{
					if (toolbar.Direction == ToolbarDirection.Vertical)	
						output.RenderBeginTag(HtmlTextWriterTag.Tr);

					output.AddAttribute(HtmlTextWriterAttribute.Align,"center");
					output.RenderBeginTag(HtmlTextWriterTag.Td);
					output.AddStyleAttribute("cursor","move");
					output.AddAttribute("onmousedown",string.Format("return ATB_dockmousedown('{0}')",toolbar.UniqueID));
					output.AddAttribute("onmouseup","return ATB_dockmouseup()");
                    output.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertToImageDir(toolbar.ImagesDirectory, toolbar.DragAndDropImage, "dock.gif", toolbar.Page, toolbar.GetType()));
					output.RenderBeginTag(HtmlTextWriterTag.Img);
					output.RenderEndTag();
					output.RenderEndTag();

					if (toolbar.Direction == ToolbarDirection.Vertical)	
						output.RenderEndTag();
				}

				foreach(ToolBase tool in toolbar.Tools)
				{
                
					if (toolbar.Direction == ToolbarDirection.Vertical)	
						output.RenderBeginTag(HtmlTextWriterTag.Tr);

					output.RenderBeginTag(HtmlTextWriterTag.Td);

					tool.RenderDesign(output);

					output.RenderEndTag();
					if (toolbar.Direction == ToolbarDirection.Vertical)	
						output.RenderEndTag();

				}

				if (toolbar.Direction == ToolbarDirection.Horizontal)
				{
					output.RenderBeginTag(HtmlTextWriterTag.Td);
					output.Write("&nbsp;");
					output.RenderEndTag();

					output.RenderEndTag(); 
				}
				//output.RenderEndTag(); 
                output.Write("</table>");

				output.RenderEndTag();

		}

		#endregion

	}

	#endregion
}
