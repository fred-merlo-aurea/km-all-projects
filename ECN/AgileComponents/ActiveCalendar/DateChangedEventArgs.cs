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
	/// Provides data for the DateChanged events of the <see cref="DateChangedEventArgs"/> control.
	/// </summary>
	public class DateChangedEventArgs : EventArgs
	{
		private DateTime _oldDate, _newDate;
            
		/// <summary>
		/// Initializes a new instance of DateChangedEventArgs class.
		/// </summary>
		/// <param name="oldDate">The date value before the change.</param>
		/// <param name="newDate">The date value after the change.</param>
		public DateChangedEventArgs (DateTime oldDate, DateTime newDate)
		{
			_oldDate = oldDate;
			_newDate = newDate;
		}

		/// <summary>
		/// The date value before the change.
		/// </summary>
		public DateTime OldDate
		{
			get
			{
				return _oldDate;
			}
		}

		/// <summary>
		/// The date value after the change.
		/// </summary>
		public DateTime NewDate
		{
			get
			{
				return _newDate;
			}
		}
	}      
}
