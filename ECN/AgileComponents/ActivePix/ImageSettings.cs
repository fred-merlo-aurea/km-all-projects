using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ImageSettings"/> object.
	/// </summary>
	[Serializable]
	public class ImageSettings
	{
		private int _maxHeight, _maxWidth, _quality;
		private FileFormat _fileFormat;
		private FileCompression _fileCompression;
		private bool _constrainProportions, _resizeSmaller;
	
		/// <summary>
		/// Initializes a new instance of the <see cref="ImageSettings"/> class.
		/// </summary>
		public ImageSettings()
		{
			_maxHeight = _maxWidth = 0;
			_fileFormat = FileFormat.Jpeg;
			_fileCompression  = FileCompression.None;
			_constrainProportions = _resizeSmaller = true;
			_quality = 100;
		}

		/// <summary>
		/// Gets or sets the quality.
		/// </summary>
		/// <value>The quality.</value>
		public int Quality
		{
			get
			{
				return _quality;
			}
			set
			{
				_quality = value;
			}
		}

		/// <summary>
		/// Gets or sets the format.
		/// </summary>
		/// <value>The format.</value>
		public FileFormat Format
		{
			get
			{
				return _fileFormat;
			}
			set
			{
				_fileFormat = value;
			}
		}

		/// <summary>
		/// Gets or sets the compression.
		/// </summary>
		/// <value>The compression.</value>
		public FileCompression Compression
		{
			get
			{
				return _fileCompression;
			}
			set
			{
				_fileCompression = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether resize smaller.
		/// </summary>
		/// <value><c>true</c> if resize smaller; otherwise, <c>false</c>.</value>
		public bool ResizeSmaller
		{
			get
			{
				return _resizeSmaller;
			}
			set
			{
				_resizeSmaller = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether constrain proportions.
		/// </summary>
		/// <value><c>true</c> if constrain proportions; otherwise, <c>false</c>.</value>
		public bool ConstrainProportions
		{
			get
			{
				return _constrainProportions;
			}
			set
			{
				_constrainProportions = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum height.
		/// </summary>
		/// <value>The maximum height.</value>
		public int MaxHeight
		{
			get
			{
				return _maxHeight;
			}
			set
			{
				_maxHeight = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum width.
		/// </summary>
		/// <value>The maximum width.</value>
		public int MaxWidth
		{
			get
			{
				return _maxWidth;
			}
			set
			{
				_maxWidth = value;
			}
		}
	}
}
