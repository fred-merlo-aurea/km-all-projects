using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

namespace ActiveUp.WebControls
{
	#region class ToolBarsContainerDesigner

	/// <summary>
	/// Designer of the <see cref="ToolbarsContainer"/> control.
	/// </summary>
	[Serializable]
	internal class ToolbarsContainerDesigner : ControlDesigner
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ToolbarsContainerDesigner()
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
				ToolbarsContainer toolbarsContainer = (ToolbarsContainer)base.Component;
				
				if (toolbarsContainer.Toolbars.Count == 0)
					return CreatePlaceHolderDesignTimeHtml("Please add items through the Toolbars property in the property pane.");

				StringWriter stringWriter = new StringWriter();
				HtmlTextWriter output = new HtmlTextWriter(stringWriter);

				DesignToolbarsContainer(ref output, toolbarsContainer);

				string generatedCode = stringWriter.ToString();
				generatedCode = RemoveAbsolutePosition(generatedCode,0);

				return generatedCode;
			}

			catch (Exception e)
			{
				return this.GetErrorDesignTimeHtml(e);
			}
		}

		private string RemoveAbsolutePosition(string s, int ndx)
		{
			string sToRemove = "position:absolute;";
			int index = s.IndexOf(sToRemove,ndx);
			if (index != -1)
			{
				s = s.Remove(index,sToRemove.Length);
				return RemoveAbsolutePosition(s,index);
			}
			return s;
		}

		/// <summary>
		/// Gets the HTML that provides information about the specified exception. This method is typically called after an error has been encountered at design time.
		/// </summary>
		/// <param name="e">The exception that occurred.</param>
		/// <returns>The HTML for the specified exception.</returns>
		protected override string GetErrorDesignTimeHtml(System.Exception e)
		{
			string text = string.Format("There was an error and the ToolbarsContainer control can't be displayed<br>Exception : {0}",e.Message);
			return this.CreatePlaceHolderDesignTimeHtml(text);
		}

		/// <summary>
		/// Create a ToolbarContainers object at design time.
		/// </summary>
		/// <param name="output">Output stream that contains the HTML used to represent the control.</param>
		/// <param name="toolbarsContainer">The <see cref="ToolbarsContainer"/> object to design.</param>
		public static void DesignToolbarsContainer(ref HtmlTextWriter output, ToolbarsContainer toolbarsContainer)
		{
			/*output.AddStyleAttribute(HtmlTextWriterStyle.Width,"100%");
				output.AddStyleAttribute(HtmlTextWriterStyle.Height,"100%");
				output.AddStyleAttribute("position","absolute");
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(toolbarsContainer.BorderColor));
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle,toolbarsContainer.BorderStyle.ToString());
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, toolbarsContainer.BorderWidth.ToString());
				output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(toolbarsContainer.BackColor));
				output.RenderBeginTag(HtmlTextWriterTag.Table);
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.Write("&nbsp");
				toolbarsContainer.Toolbars[0].RenderControl(output);
				output.RenderEndTag();
				output.RenderEndTag();
				output.RenderEndTag();*/

			if (toolbarsContainer.Width != Unit.Empty)
				output.AddStyleAttribute(HtmlTextWriterStyle.Width, toolbarsContainer.Width.ToString());
			else
				output.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			output.AddStyleAttribute(HtmlTextWriterStyle.Height, toolbarsContainer.Height.ToString());
			output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(toolbarsContainer.BorderColor));
			output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, toolbarsContainer.BorderStyle.ToString());
			output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, toolbarsContainer.BorderWidth.ToString());
			output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(toolbarsContainer.BackColor));
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            /*if (toolbarsContainer.BackImage != string.Empty)
                output.AddStyleAttribute("background-image", string.Format("url({0}{1})", toolbarsContainer.ImagesDirectory, toolbarsContainer.BackImage));*/
			output.RenderBeginTag(HtmlTextWriterTag.Table);

			foreach (Toolbar t in toolbarsContainer.Toolbars)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
				output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
				output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				output.RenderBeginTag(HtmlTextWriterTag.Table);
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				t.Position = Position.Relative;
				t.RenderDesign(output);
				output.RenderEndTag();
				output.RenderEndTag();
				output.RenderEndTag();
				output.RenderEndTag();
				output.RenderEndTag();
			}

			output.RenderEndTag();
		}

		#endregion
	}

	#endregion
}
