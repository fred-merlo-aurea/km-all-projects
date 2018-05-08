// Active Calendar v2.0
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
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
using System.Web.UI;
using AttributeCollection = System.Web.UI.AttributeCollection;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Supports the page parser in building a control and the child controls it contains for the <see cref="DateCollectionItem"/>.
	/// </summary>
	public class DateToolTipCollectionItemControlBuilder : ControlBuilder
	{

		/// <summary>
		/// Determines whether the white space literals in the control must be processed or ignored.
		/// </summary>
		/// <returns>true if the white space literals in the control must be processed; otherwise, false.</returns>
		public override bool AllowWhitespaceLiterals()
		{
			return false;
		}

		/// <summary>
		/// Determines whether the literal string of an HTML control must be HTML decoded.
		/// </summary>
		/// <returns>true if the HTML control literal string is to be decoded; otherwise, false.</returns>
		public override bool HtmlDecodeLiterals()
		{
			return true;
		}
	}
}