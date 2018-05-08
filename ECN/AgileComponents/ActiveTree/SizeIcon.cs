using System;
using System.Globalization;
using System.Drawing;
using System.ComponentModel;
using System.Runtime;
using System.Runtime.InteropServices;


namespace ActiveUp.WebControls
{
	[TypeConverterAttribute(typeof(SizeIconConverter))]
	[ComVisibleAttribute(true)]
	[SerializableAttribute()]
	internal class SizeIcon
	{
		private int _width;

		private int _height;

		public SizeIcon()
		{
			_width = 0;
			_height = 0;
		}

		public SizeIcon(int width, int height)
		{
			_width = width;
			_height = height;
		}

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

		public int Heigth
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

		public static implicit operator SizeIcon(Size size)
		{
			SizeIcon sizeIcon = new SizeIcon();
			sizeIcon.Heigth = size.Height;
			sizeIcon.Width = size.Width;

			return sizeIcon;
			
		}
	}
}
