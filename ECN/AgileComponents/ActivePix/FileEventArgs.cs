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
	/// Provides data for the FileSave events of the <see cref="FileEventArgs"/> control.
	/// </summary>
	public class FileEventArgs : EventArgs
	{

		/// <summary>
		/// Initializes a new instance of FileEventArgs class.
		/// </summary>
		public FileEventArgs (string fileName)
		{
			_FileName = fileName;
		}

		private string _FileName;
		/// <summary>
		/// Gets or sets the file name.
		/// </summary>
		/// <value>The file name.</value>
		public string FileName
		{
			get
			{
				return _FileName;
			}
			set
			{
				_FileName = value;
			}
		}
	}      
}
