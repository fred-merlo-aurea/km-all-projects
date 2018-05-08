using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Summary description for ImageSet.
	/// </summary>
	public class ImageSet
	{
		private string _off, _over, _on;

		public ImageSet()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public string Off
		{
			get
			{
				return _off;
			}
			set
			{
				_off = value;
			}
		}

		public string On
		{
			get
			{
				return _on;
			}
			set
			{
				_on = value;
			}
		}

		public string Over
		{
			get
			{
				return _over;
			}
			set
			{
				_over = value;
			}
		}
	}
}
