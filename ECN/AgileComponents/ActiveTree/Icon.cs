using System;
using System.Drawing.Design;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Web.UI;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="Icon"/> object.
	/// </summary>
	[Editor(typeof(IconEditor),typeof(UITypeEditor))]
	[TypeConverterAttribute(typeof(IconConverter))]
	internal class Icon
	{
		private Image _image = new Bitmap(16,16);

		private SizeIcon _size = new SizeIcon();

		private string _file;

		public Icon(Image image)
		{
			_image = image;
		}

		public Icon(string fileName)
		{
			
			/*FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			byte[] bs = new byte[(uint)fileStream.Length];
			fileStream.Read(bs, 0, (int)fileStream.Length);
			_image = Image.FromStream(new MemoryStream(bs));
			_size = _image.Size;*/
			
			_file = fileName;
		}

		public Icon(Stream stream,string fileName)
		{
			byte[] bs = new byte[(uint)stream.Length];
			stream.Read(bs, 0, (int)stream.Length);
			_image = Image.FromStream(new MemoryStream(bs));
			_size = _image.Size;
			_file = fileName;
		}	

		public Icon()
		{
			_image = new Bitmap(16,16);
			_size = _image.Size;
		}


		[
			Bindable(true), 
			NotifyParentProperty(true),
			DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public Image Image
		{
			get
			{
				return _image;
			}

		}

		[
		Bindable(true),
		ReadOnlyAttribute(true)
		]
		public string FileName
		{
			get
			{
				return _file;
			}

			set
			{
				_file = value;
			}

		}
			
	}
}
