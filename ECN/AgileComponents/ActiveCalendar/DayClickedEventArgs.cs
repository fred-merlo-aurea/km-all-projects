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

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Provides data for the DateClicked events of the <see cref="DayClickedEventArgs"/> control.
	/// </summary>
	public class DayClickedEventArgs : EventArgs
	{
		private DateTime _clickedDate;
            
		/// <summary>
		/// Initializes a new instance of DateChangedEventArgs class.
		/// </summary>
		/// <param name="clickedDate">Clicked date.</param>
		public DayClickedEventArgs (DateTime clickedDate)
		{
			_clickedDate = clickedDate;
		}

		/// <summary>
		/// The clicked date after change.
		/// </summary>
		public DateTime ClickedDate
		{
			get
			{
				return _clickedDate;
			}
		}
	}      
}
