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
	/// Enumeration to define the input mode of the <see cref="MaskedTextBox"/> control.
	/// </summary>
	public enum InputMode
	{
		/// <summary>
		/// Any characters are allowed including numerics and special characters.
		/// </summary>
		NotSet,
		/// <summary>
		/// Only alpha characters are allowed.
		/// </summary>
		Alpha,
		/// <summary>
		/// Only numerics characters are allowed.
		/// </summary>
		Numeric,
		/// <summary>
		/// Only numerics characters are allowed with percent formatting applied.
		/// </summary>
		Percent,
		/// <summary>
		/// Only numerics characters are allowed with currency formatting applied.
		/// </summary>
		Currency,
		/// <summary>
		/// Only numerics characters are allowed with the specified date formatting applied.
		/// </summary>
		Date,
		/// <summary>
		/// The specified mask is applied.
		/// </summary>
		Mask
	}
}
