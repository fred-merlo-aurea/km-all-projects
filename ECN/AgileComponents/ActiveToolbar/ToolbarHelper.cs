// Html TextBox 2.x
// Copyright (c) 2003 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;

namespace ActiveUp.WebControls
{
	#region class ToolbarHelper

	/// <summary>
	/// Debug trace for the toolbar.
	/// </summary>
	public class ToolbarHelper
	{
		#region Constructor

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ToolbarHelper()
		{
		}

		#endregion

		#region Methods

		/// <summary>
		/// Adds a string in the trace.
		/// </summary>
		/// <param name="traceText">The string to add.</param>
		public static void DebugTrace(string traceText)
		{
			if (System.Web.HttpContext.Current != null)
				System.Web.HttpContext.Current.Trace.Write("ActiveToolbar", traceText);
		}

		#endregion
	}

	#endregion
}
