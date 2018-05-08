using System;
using System.Web.UI.Design;
using System.IO;
using System.Text;
using System.Web.UI;

namespace ActiveUp.WebControls
{
	#region AjaxPanelDesigner

	/// <summary>
	/// This class is used to render the ajax panel at design time.
	/// </summary>
	public class AjaxPanelDesigner : ControlDesigner
	{
		#region Constructors

		/// <summary>
		/// The default constructor.
		/// </summary>
		public AjaxPanelDesigner()
		{
	
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the design time HTML code.
		/// </summary>
		/// <returns>A string containing the HTML to render.</returns>
		public override string GetDesignTimeHtml()
		{
			AjaxPanel ajaxPanel = (AjaxPanel)base.Component;

			StringBuilder stringBuilder = new StringBuilder();

			StringWriter stringWriter = new StringWriter();
			HtmlTextWriter output = new HtmlTextWriter(stringWriter);

			ajaxPanel.RenderControl(output);

			return stringWriter.ToString();
		}

		#endregion
	}

	#endregion
}
