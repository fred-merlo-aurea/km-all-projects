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
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// This is an object to represent a file.
	/// </summary>
	[
		Serializable,
		ToolboxItem(false)
	]
	public class File
	{
		private string _label, _location;
		private int _width, _height;
		private long _size;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public File()
		{
			_label = string.Empty;
			_location = string.Empty;
			_size = 0;
			_width = 0;
			_height = 0;
		}

		/// <summary>
		/// Create the File object specifying the Label, Location and Size.
		/// </summary>
		/// <param name="label">The label (description) of the file.</param>
		/// <param name="location">The location of the file.</param>
		/// <param name="size">The size of the file in bytes.</param>
		public File(string label, string location, long size)
		{
			_label = label;
			_location = location;
			_size = size;
			_width = 0;
			_height = 0;
		}

		/// <summary>
		/// Create the File object specifying the Label, Location, Size, Width and Height (usually an image).
		/// </summary>
		/// <param name="label">The label (description) of the file.</param>
		/// <param name="location">The location of the file.</param>
		/// <param name="size">The size of the file in bytes.</param>
		/// <param name="width">The width of the file (image) in pixels.</param>
		/// <param name="height">The height of the file (image) in pixels.</param>
		public File(string label, string location, long size, int width, int height)
		{
			_label = label;
			_location = location;
			_size = size;
			_width = width;
			_height = height;
		}

		/// <summary>
		/// Gets or sets the file label (description).
		/// </summary>
		public string Label
		{
			get
			{
				return _label;
			}
			set
			{
				_label = value;
			}
		}

		/// <summary>
		/// Gets or sets the location of the file.
		/// </summary>
		public string Location
		{
			get
			{
				return _location;
			}
			set
			{
				_location = value;
			}
		}

		/// <summary>
		/// Gets or sets the file size in bytes.
		/// </summary>
		public long Size
		{
			get
			{
				return _size;
			}
			set
			{
				_size = value;
			}
		}

		/// <summary>
		/// Gets or sets the width of the file (image).
		/// </summary>
		public int Width
		{
			get
			{
				return _width;
			}
			set
			{
				_width = value;
			}
		}

		/// <summary>
		/// Gets or sets the height of the file (image).
		/// </summary>
		public int Height
		{
			get
			{
				return _height;
			}
			set
			{
				_height = value;
			}
		}
	}
}
