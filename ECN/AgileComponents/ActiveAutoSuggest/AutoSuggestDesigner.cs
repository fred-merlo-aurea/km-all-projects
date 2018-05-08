using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;
using System.IO;

namespace ActiveUp.WebControls
{
	#region AutoSuggestControlDesigner

	internal class AutoSuggestDesigner : ControlDesigner
	{
		/// <summary>
		/// Gets the design time HTML code.
		/// </summary>
		/// <returns>A string containing the HTML to render.</returns>
		public override string GetDesignTimeHtml()
		{
			AutoSuggest autoSuggest = (AutoSuggest)base.Component;

			StringWriter stringWriter = new StringWriter();
			HtmlTextWriter output = new HtmlTextWriter(stringWriter);

			autoSuggest.RenderControl(output);

			return stringWriter.ToString();
		}

	}

	#endregion
}
