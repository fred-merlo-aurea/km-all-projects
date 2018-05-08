using System;
using System.Web.UI;
using System.Web.UI.Design;
using System.IO;

namespace ActiveUp.WebControls
{
	#region PanelDesigner

	/// <summary>
	/// Designer of the <see cref="Panel"/> control.
	/// </summary>
	public class PanelDesigner : ControlDesigner
	{
		#region Constructor

		/// <summary>
		/// The default constructor.
		/// </summary>
		public PanelDesigner()
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
				Panel panel = (Panel)base.Component;

				StringWriter stringWriter = new StringWriter();
				HtmlTextWriter output = new HtmlTextWriter(stringWriter);
				panel.RenderControl(output);
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
			string text = string.Format("There was an error and the Panel control can't be displayed<br>Exception : {0}",e.ToString());
			return this.CreatePlaceHolderDesignTimeHtml(text);
		}

		#endregion
	}

	#endregion
}
