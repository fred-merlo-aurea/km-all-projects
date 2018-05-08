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
	/// Represents a <see cref="ExtendedButtonClickEventArgs"/> object.
	/// </summary>
	public class ExtendedButtonClickEventArgs : EventArgs
	{
		/// <summary>
		/// The default construtor.
		/// </summary>
		/// <param name="type">The type of button.</param>
		public ExtendedButtonClickEventArgs(ExtendedButtonType type)
		{
			this.X = -1;
			this.Y = -1;
			this.ButtonType = type;
		}

		/// <summary>
		/// The ExtendedButtonClickEventArgs.
		/// </summary>
		/// <param name="type">The button type.</param>
		/// <param name="x">The x position of the click.</param>
		/// <param name="y">The y position of the click.</param>
		public ExtendedButtonClickEventArgs(ExtendedButtonType type, int x, int y)
		{
			this.X = x; this.Y = y; this.ButtonType = type;
		}

		/// <summary>
		/// The x position of the click.
		/// </summary>
		public int X;

		/// <summary>
		/// The y position of the click.
		/// </summary>
		public int Y;

		/// <summary>
		/// The button type.
		/// </summary>
		public ExtendedButtonType ButtonType;
	}
}
