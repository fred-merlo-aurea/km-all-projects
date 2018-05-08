// ActiveInput
// Copyright (c) 2005 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Enumeration that define the navigation type for the listboxes.
	/// </summary>
	public enum OrderedListBoxNavigation
	{
		/// <summary>
		/// Nothing is explicitely set.
		/// </summary>
		NotSet,
		/// <summary>
		/// Navigation uses hyperlinks.
		/// </summary>
		Link,
		/// <summary>
		/// Navigation uses standard HTML buttons.
		/// </summary>
		Button,
		/// <summary>
		/// Navigation uses images as buttons.
		/// </summary>
		Image
	}
}
