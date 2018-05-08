// ActiveTimer
// Copyright (c) 2002 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Text;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// This class is used to render the pager at design time.
	/// </summary>
	/// <remarks>You should not use this class in your project.</remarks>
	internal class WebTimerControlDesigner : ControlDesigner
	{
		/// <summary>
		/// The default constructor.
		/// </summary>
		public WebTimerControlDesigner()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Gets the design time HTML code.
		/// </summary>
		/// <returns>A string containing the HTML to render.</returns>
		public override string GetDesignTimeHtml()
		{
			WebTimer webTimer = (WebTimer)base.Component;

			StringBuilder stringBuilder = new StringBuilder();

			StringWriter stringWriter = new StringWriter();
			HtmlTextWriter writer = new HtmlTextWriter(stringWriter);

			// Initialize the structure.
			//

			// Add the whole structure to the control collection
			//webTimer.RenderControl(writer)
			writer.Write("[Web Timer]");

			return stringWriter.ToString();
		}

		/// <summary>
		/// Gets the design time HTML code when empty (never used in ActivePager).
		/// </summary>
		/// <returns>A string containing the HTML to render.</returns>
		protected override string GetEmptyDesignTimeHtml() 
		{
			string text;
			text = "this should be never displayed.";
			return CreatePlaceHolderDesignTimeHtml(text);
		}
	}
}
