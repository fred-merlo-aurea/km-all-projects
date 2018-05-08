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
	/// Enumeration to specify the predefined numeric textbox values.
	/// </summary>
	public enum NumericValues
	{
		/// <summary>
		/// Uses the values specified in the different properties.
		/// </summary>
		NotSet,
		/// <summary>
		/// Stricly accepts byte value.
		/// </summary>
		Byte,
		/// <summary>
		/// Stricly accepts 16bits integer value.
		/// </summary>
		Int16,
		/// <summary>
		/// Stricly accepts 32bits integer value.
		/// </summary>
		Int32,
		/// <summary>
		/// Stricly accepts 64bits integer value.
		/// </summary>
		Int64
	}
}
