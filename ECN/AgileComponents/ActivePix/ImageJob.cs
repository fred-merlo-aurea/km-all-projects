using System;
using System.IO;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace ActiveUp.WebControls 
{
	/// <summary>
	/// Represents a <see cref="ImageJob"/> object.
	/// </summary>
	public class ImageJob
	{
		private Image _image;
		private string _filename = string.Empty/*, _license = string.Empty*/;
		private Graphics _g;

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageJob"/> class.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="background">The background.</param>
		public ImageJob(int width, int height, System.Drawing.Color background)
		{
			Bitmap bitmap = new Bitmap(width, height);
			Graphics graph = Graphics.FromImage(bitmap);
			graph.Clear(background);
			_image = (Image)bitmap; 
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageJob"/> class.
		/// </summary>
		/// <param name="image">The image.</param>
		public ImageJob(Image image)
		{
			//_image = (Image)image.Clone();

			Image src = (Image)image.Clone();
			_image = new Bitmap(src.Width,src.Height);
			_g = Graphics.FromImage(_image);
			_g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageJob"/> class.
		/// </summary>
		/// <param name="stream">The stream.</param>
		public ImageJob(Stream stream)
		{
			Load(stream);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageJob"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public ImageJob(string filename)
		{
			Load(filename);
			_filename = filename;
		}

		/// <summary>
		/// Disposes this instance.
		/// </summary>
		public void Dispose()
		{
			this._image.Dispose();
		}

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		/// <value>The image.</value>
		public Image Image
		{
			get
			{
				return _image;
			}
			set
			{
				_image = value;
			}
		}

		/*private void SetLicense()
		{
			QuickProcess.License = this.License;
		}*/

		/// <summary>
		/// Loads the specified filename.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public void Load(string filename)
		{
			//_image = Image.FromFile(filename);

			Image src = Image.FromFile(filename);
			_image = new Bitmap(src.Width,src.Height);
			_g = Graphics.FromImage(_image);
			_g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
		}

		/// <summary>
		/// Loads the specified stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		public void Load(Stream stream)
		{
			//_image = Image.FromStream(stream);

			Image src = Image.FromStream(stream);
			_image = new Bitmap(src.Width,src.Height);
			_g = Graphics.FromImage(_image);
			_g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
		}

		/// <summary>
		/// Resizes the image.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="constrainProportions">if set to <c>true</c> constrain proportions.</param>
		/// <param name="resizeSmaller">if set to <c>true</c> resize smaller.</param>
		/// <param name="bilinear">if set to <c>true</c> using bilinear.</param>
		public void ResizeImage(int width, int height, bool constrainProportions, bool resizeSmaller,bool bilinear)
		{
			//SetLicense();
			_image = QuickProcess.ResizeImage(_image, width, height, constrainProportions, resizeSmaller,bilinear);
		}

		/// <summary>
		/// Resizes the image.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="constrainProportions">if set to <c>true</c> constrains proportions.</param>
		/// <param name="resizeSmaller">if set to <c>true</c> resize smaller.</param>
		public void ResizeImage(int width, int height, bool constrainProportions, bool resizeSmaller)
		{
			//SetLicense();
			_image = QuickProcess.ResizeImage(_image, width, height, constrainProportions, resizeSmaller);
		}

		/// <summary>
		/// Resizes the canvas.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="anchor">The anchor.</param>
		/// <param name="background">The background.</param>
		public void ResizeCanvas(int width, int height, AnchorType anchor, Color background)
		{
			//SetLicense();
			_image = QuickProcess.ResizeCanvas(_image, width, height, anchor, background);
		}
		
		/// <summary>
		/// Crops the specified point.
		/// </summary>
		/// <param name="from">From point.</param>
		/// <param name="to">To point.</param>
		public void Crop(Point from, Point to)
		{
			//SetLicense();
			_image = QuickProcess.Crop(_image, from, to);
		}

		/// <summary>
		/// Crops the image with the specified selection.
		/// </summary>
		/// <param name="selection">The selection.</param>
		public void Crop(Selection selection)
		{
			//SetLicense();
			_image = QuickProcess.Crop(_image, selection.X1, selection.Y1, selection.X2, selection.Y2);
		}

		/// <summary>
		/// Crops the image with specified selection.
		/// </summary>
		/// <param name="xFrom">The x from position.</param>
		/// <param name="yFrom">The y from position.</param>
		/// <param name="xTo">The x to position.</param>
		/// <param name="yTo">The y to position.</param>
		public void Crop(int xFrom, int yFrom, int xTo, int yTo)
		{
			//SetLicense();
			_image = QuickProcess.Crop(_image, xFrom, yFrom, xTo, yTo);
		}

		/// <summary>
		/// Flips the image with the specified flip type.
		/// </summary>
		/// <param name="flipType">Type of the flip.</param>
		public void Flip(FlipType flipType)
		{
			//SetLicense();
			_image = QuickProcess.Flip(_image, flipType);
		}

		/// <summary>
		/// Zooms the image with the specified x position.
		/// </summary>
		/// <param name="xPos">The x position.</param>
		/// <param name="yPos">The y position.</param>
		/// <param name="factor">The factor.</param>
		public void Zoom(int xPos, int yPos, float factor)
		{
			//SetLicense();
			_image = QuickProcess.Zoom(_image, xPos, yPos, factor);
		}

		/// <summary>
		/// Rotates the image with the specified angle.
		/// </summary>
		/// <param name="angle">The angle.</param>
		public void Rotate(float angle)
		{
			//SetLicense();
			_image = QuickProcess.Rotate(_image, angle);
		}

		/// <summary>
		/// Rotates the image at left.
		/// </summary>
		public void RotateLeft()
		{
			//SetLicense();
			_image = QuickProcess.RotateLeft(_image);
		}

		/// <summary>
		/// Rotates the image at right.
		/// </summary>
		public void RotateRight()
		{
			//SetLicense();
			_image = QuickProcess.RotateRight(_image);
		}

		/// <summary>
		/// Adds image text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="font">The font.</param>
		/// <param name="size">The size.</param>
		/// <param name="style">The style.</param>
		/// <param name="foreColor">Fore color.</param>
		/// <param name="antialias">if set to <c>true</c> using of antialias.</param>
		/// <param name="xpos">The x position.</param>
		/// <param name="ypos">The y position.</param>
		/// <param name="aligment">The aligment.</param>
		public void AddText(string text, string font, int size, FontStyle style, System.Drawing.Color foreColor, bool antialias, int xpos, int ypos, StringAlignment aligment)
		{
			//SetLicense();
			_image = QuickProcess.AddText(_image, text, font, size, style, foreColor, antialias, xpos, ypos, aligment);
		}

		/// <summary>
		/// Saves the image.
		/// </summary>
		public void Save()
		{
			if (_filename != null && _filename != string.Empty)
			{
				if (_g != null)
					_g.Dispose();
				_image.Save(_filename);
			}
			else
				throw new Exception("Can't save the Image. Filename is not set");
		}

		/// <summary>
		/// Saves the image with the specified filename.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public void Save(string filename)
		{
			if (_g != null)
				_g.Dispose(); 
			_image.Save(filename);
		}

		/// <summary>
		/// Saves the image with the specified stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="imageFormat">The image format.</param>
		public void Save(System.IO.Stream stream, FileFormat imageFormat)
		{
			Save(stream, FileCompression.None, 100, imageFormat);
		}

		/// <summary>
		/// Saves the image with the specified stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="imageCompression">The image compression.</param>
		/// <param name="quality">The quality.</param>
		/// <param name="imageFormat">The image format.</param>
		public void Save(System.IO.Stream stream, FileCompression imageCompression, int quality, FileFormat imageFormat)
		{
			ImageFormat format;
			string mimeType = string.Empty;
			EncoderValue compression;

			compression = GetCompression(imageCompression);

			switch (imageFormat)
			{
				case FileFormat.Bmp: format = ImageFormat.Bmp; mimeType = "image/bmp"; break;
				case FileFormat.Emf: format = ImageFormat.Emf; mimeType = "image/x-emf"; break;
				case FileFormat.Exif: format = ImageFormat.Exif; mimeType = "image/jpeg"; break;
				case FileFormat.Gif: format = ImageFormat.Gif; mimeType = "image/gif"; compression = EncoderValue.CompressionNone; break;
				case FileFormat.Jpeg: format = ImageFormat.Jpeg; mimeType = "image/jpeg"; break;
				case FileFormat.Png: format = ImageFormat.Png; mimeType = "image/png"; break;
				case FileFormat.Tiff: format = ImageFormat.Tiff; mimeType = "image/tiff"; break;
				case FileFormat.Wmf: format = ImageFormat.Wmf; mimeType = "image/x-wmf"; break;
			}

			if (mimeType == "image/tiff" && compression != EncoderValue.CompressionNone)
				throw new Exception("Only LZW compression is allowed with TIFF format.");

			ImageCodecInfo encoder = GetEncoderInfo(mimeType);

			if (encoder == null)
				throw new Exception("Encoder not found. Try another format.");

			System.Drawing.Imaging.EncoderParameters parameters = new EncoderParameters(2);
			EncoderParameter param1 = new EncoderParameter(Encoder.Compression, (long)compression);
			parameters.Param[0] = param1;
			EncoderParameter param2 = new EncoderParameter(Encoder.Quality, quality);
			parameters.Param[1] = param2;

			if (_g != null)
				_g.Dispose(); 
			_image.Save(stream, encoder, parameters);
		}

		/// <summary>
		/// Saves the image with the specified filename.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="imageFormat">The image format.</param>
		public void Save(string filename, FileFormat imageFormat)
		{
			Save(filename, FileCompression.None, 100, imageFormat);
		}

		/// <summary>
		/// Saves the image with the specified filename.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="imageCompression">The image compression.</param>
		/// <param name="quality">The quality.</param>
		/// <param name="imageFormat">The image format.</param>
		public void Save(string filename, FileCompression imageCompression, int quality, FileFormat imageFormat)
		{
			ImageFormat format;
			string mimeType = string.Empty;
			EncoderValue compression;

			compression = GetCompression(imageCompression);

			switch (imageFormat)
			{
				case FileFormat.Bmp: format = ImageFormat.Bmp; mimeType = "image/bmp"; break;
				case FileFormat.Emf: format = ImageFormat.Emf; mimeType = "image/x-emf"; break;
				case FileFormat.Exif: format = ImageFormat.Exif; mimeType = "image/jpeg"; break;
				case FileFormat.Gif: format = ImageFormat.Gif; mimeType = "image/gif"; compression = EncoderValue.CompressionNone; break;
				case FileFormat.Jpeg: format = ImageFormat.Jpeg; mimeType = "image/jpeg"; break;
				case FileFormat.Png: format = ImageFormat.Png; mimeType = "image/png"; break;
				case FileFormat.Tiff: format = ImageFormat.Tiff; mimeType = "image/tiff"; break;
				case FileFormat.Wmf: format = ImageFormat.Wmf; mimeType = "image/x-wmf"; break;
			}

			if (mimeType == "image/tiff" && compression != EncoderValue.CompressionNone)
				throw new Exception("Only LZW compression is allowed with TIFF format.");

			ImageCodecInfo encoder = GetEncoderInfo(mimeType);

			if (encoder == null)
				throw new Exception("Encoder not found. Try another format.");

			System.Drawing.Imaging.EncoderParameters parameters = new EncoderParameters(2);
			EncoderParameter param1 = new EncoderParameter(Encoder.Compression, (long)compression);
			parameters.Param[0] = param1;
			EncoderParameter param2 = new EncoderParameter(Encoder.Quality, quality);
			parameters.Param[1] = param2;

			if (_g != null)
				_g.Dispose(); 

			_image.Save(filename, encoder, parameters);
		}

		private static ImageCodecInfo GetEncoderInfo(String mimeType)
		{
			int j;
			ImageCodecInfo[] encoders;
			encoders = ImageCodecInfo.GetImageEncoders();
			for(j = 0; j < encoders.Length; ++j)
			{
				if(encoders[j].MimeType == mimeType)
					return encoders[j];
			} return null;
		}

		private static EncoderValue GetCompression(FileCompression compression)
		{
			switch (compression)
			{
				case FileCompression.CCITT3: return EncoderValue.CompressionCCITT3;
				case FileCompression.CCITT4: return EncoderValue.CompressionCCITT4;
				case FileCompression.LZW: return EncoderValue.CompressionLZW;
				case FileCompression.None: return EncoderValue.CompressionNone;
				case FileCompression.Rle: return EncoderValue.CompressionRle;
			}

			return EncoderValue.CompressionNone;
		}

		/*
		/// <summary>
		/// Gets or sets the license key.
		/// </summary>
		public string License
		{
			get
			{
				return _license;
			}
		
			set
			{
				_license = value;
			}
		}*/

		/// <summary>
		/// Brightnesses the image with the specified brightness.
		/// </summary>
		/// <param name="brightness">The brightness.</param>
		public void Brightness(int brightness)
		{
			_image = QuickProcess.Brightness(_image, brightness);
		}

		/// <summary>
		/// Colorizes the image with the specified red.
		/// </summary>
		/// <param name="red">The red color.</param>
		/// <param name="green">The green color.</param>
		/// <param name="blue">The blue color.</param>
		public void Colorize(int red, int green, int blue)
		{
			_image = QuickProcess.Colorize(_image, red, green, blue);
		}

		/// <summary>
		/// Contrasts the specified contrast value.
		/// </summary>
		/// <param name="contrastValue">The contrast value.</param>
		public void Contrast(int contrastValue)
		{
			_image = QuickProcess.Contrast(_image, contrastValue);
		}	

		/// <summary>
		/// Gammas the image with the specified color.
		/// </summary>
		/// <param name="red">The red color.</param>
		/// <param name="green">The green color.</param>
		/// <param name="blue">The blue color.</param>
		public void Gamma(int red, int green, int blue) 
		{
			_image = QuickProcess.Gamma(_image, red, green, blue);
		}

		/// <summary>
		/// Grayscale the image.
		/// </summary>
		public void GrayScale() 
		{
			_image = QuickProcess.GrayScale(_image);
		}

		/// <summary>
		/// Inverts the image.
		/// </summary>
		public void Invert()
		{
			_image = QuickProcess.Invert(_image);
		}

		/// <summary>
		/// Image edges detection.
		/// </summary>
		/// <param name="threshold">The threshold.</param>
		public void EdgeDetection(byte threshold) 
		{
			_image = QuickProcess.EdgeDetection(_image, threshold); 
		}

		/// <summary>
		/// Jiffers the image with the specified degree.
		/// </summary>
		/// <param name="degree">The degree.</param>
		public void Jiffer(int degree) 
		{
			_image = QuickProcess.Jiffer(_image,degree);
		}

		/// <summary>
		/// Luminances the image with the specified factor.
		/// </summary>
		/// <param name="factor">The factor.</param>
		public void Luminance(int factor) 
		{
			_image = QuickProcess.Luminance(_image,factor);
		}

		/// <summary>
		/// Pixelizes the image with the specified pixel size.
		/// </summary>
		/// <param name="pixelSize">Size of the pixel.</param>
		/// <param name="grid">if set to <c>true</c> [grid].</param>
		/// <param name="gridColor">Color of the grid.</param>
		public void Pixelize(short pixelSize, bool grid, Color gridColor)
		{
			_image = QuickProcess.Pixelize(_image, pixelSize, grid, gridColor);
		}

		/// <summary>
		/// Satures the images with the specified factor.
		/// </summary>
		/// <param name="factor">The factor.</param>
		public void Saturation(int factor)
		{
			_image = QuickProcess.Saturation(_image, factor);
		}

		/// <summary>
		/// Spheres the image.
		/// </summary>
		public void Sphere()
		{
			_image = QuickProcess.Sphere(_image);
		}

		/// <summary>
		/// Waters the image with the specified wave.
		/// </summary>
		/// <param name="wave">The wave.</param>
		public void Water(int wave)
		{
			_image = QuickProcess.Water(_image,wave);
		}
	}
}
