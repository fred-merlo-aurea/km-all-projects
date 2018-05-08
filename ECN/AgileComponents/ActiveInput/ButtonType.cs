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
	/// Enumeration to define the rendering type of the ExtendedButton control.
	/// </summary>
	public enum ExtendedButtonType
	{
		/// <summary>
		/// The default rendering type. Renders a standard HTML button using the INPUT tag with BUTTON attribute.
		/// </summary>
		Button,
		/// <summary>
		/// Renders the button as an hyper link.
		/// </summary>
		Link,
		/// <summary>
		/// Rendes the button as an image button.
		/// </summary>
		Image
	}
}
