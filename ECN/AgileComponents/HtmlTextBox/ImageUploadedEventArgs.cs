using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// This class holds the informations about the uploaded image.
	/// </summary>
	public class ImageUploadedEventArgs : EventArgs 
	{
		string _filename, _directory;
		int _size;
		bool _success;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public ImageUploadedEventArgs()
		{
			_filename = string.Empty;
			_directory = string.Empty;
		}

		/// <summary>
		/// Create the object with the specified data.
		/// </summary>
		/// <param name="filename">The file name of the file.</param>
		/// <param name="directory">The directory.</param>
		/// <param name="size">The size of the image.</param>
		/// <param name="success">The status of the upload.</param>
		public ImageUploadedEventArgs(string filename, string directory, int size, bool success)
		{
			_filename = filename;
			_directory = directory;
			_size = size;
			_success = success;
		}

		/// <summary>
		/// Gets or sets the filename.
		/// </summary>
		public string Filename
		{
			get
			{
				return _filename;
			}
			set
			{
				_filename = value;
			}
		}

		/// <summary>
		/// Gets or sets the Directory.
		/// </summary>
		public string Directory
		{
			get
			{
				return _directory;
			}
			set
			{
				_directory = value;
			}
		}

		/// <summary>
		/// Gets or sets the size in bytes.
		/// </summary>
		public int Size
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
		/// Gets or sets the value indicating if the uploaded image was a success.
		/// </summary>
		public bool Success
		{
			get
			{
				return _success;
			}
			set
			{
				_success = value;
			}
		}
	}
}
