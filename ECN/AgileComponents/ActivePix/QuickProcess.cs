using System;
using System.Drawing;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="QuickProcess"/> object.
	/// </summary>
	public class QuickProcess
	{
		private const float Degree0 = 0f;
		private const float Degree90 = 90f;
		private const float Degree180 = 180f;
		private const float Degree270 = 270f;
		private const float Degree360 = 360f;

		/// <summary>
		/// Initializes a new instance of the <see cref="QuickProcess"/> class.
		/// </summary>
		public QuickProcess()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private delegate void DlgResizeCanvas(
			AnchorType anchor,
			ref Rectangle cropRect,
			ref Point point,
			Size newSize,
			Size origSize,
			bool isNewWidthSmallerOrEqual,
			bool isNewHeightSmallerOrEqual,
			int widthDifference,
			int heightDifference,
			int widthDifferenceHalf,
			int heightDifferenceHalf);

		private static bool IsRegistered()
		{
#if (LICENSE)
			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.AIE, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, QuickProcess.License);

			/*if (!licenseStatus.IsRegistered)
				return false;

			return true;*/

			return false;
#else
			return true;
#endif
		}

		private static Image ApplyTrial(Image image)
		{
			if (!IsRegistered())
				return AddText(image, "www.activeup.com", "Arial", 16, FontStyle.Bold, Color.Red, false, 0, 0, StringAlignment.Near);
			else
				return image;
		}

		/// <summary>
		/// Creates the thumbnail.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destImage">The dest image.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="resizeSmaller">if set to <c>true</c> resizes smaller.</param>
		public static void CreateThumbnail(string sourceImage, string destImage, int width, int height, bool resizeSmaller)
		{
			ResizeImage(sourceImage, destImage, width, height, true, resizeSmaller);
		}

		/// <summary>
		/// Creates the thumbnail.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="prefix">The prefix.</param>
		/// <param name="suffix">The suffix.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="resizeSmaller">if set to <c>true</c> resize smaller.</param>
		public static void CreateThumbnail(string sourceImage, string prefix, string suffix, int width, int height, bool resizeSmaller)
		{
			string destImage = System.IO.Path.GetDirectoryName(sourceImage) + "\\" + prefix + System.IO.Path.GetFileNameWithoutExtension(sourceImage) + suffix + System.IO.Path.GetExtension(sourceImage);
			ResizeImage(sourceImage, destImage, width, height, true, resizeSmaller);
		}

		/// <summary>
		/// Resizes the image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		public static void ResizeImage(string sourceImage, int width, int height)
		{
			ResizeImage(sourceImage, sourceImage, width, height, true, false);
		}

		/// <summary>
		/// Resizes the image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destinationImage">The destination image.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="constrainProportions">if set to <c>true</c> constrain proportions.</param>
		/// <param name="resizeSmaller">if set to <c>true</c> resize smaller.</param>
		public static void ResizeImage(string sourceImage, string destinationImage, int width, int height, bool constrainProportions, bool resizeSmaller)
		{
			ResizeImage(sourceImage, destinationImage, width, height, constrainProportions, resizeSmaller, false);
		}

		/// <summary>
		/// Resizes the image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destinationImage">The destination image.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="constrainProportions">if set to <c>true</c> constrains proportions.</param>
		/// <param name="resizeSmaller">if set to <c>true</c> resizes smaller.</param>
		/// <param name="bilinear">if set to <c>true</c> using bilinear.</param>
		public static void ResizeImage(string sourceImage, string destinationImage, int width, int height, bool constrainProportions, bool resizeSmaller, bool bilinear)
		{
			/*Image src = Image.FromFile(sourceImage);
			Image _image = new Bitmap(src.Width,src.Height);
			src.Dispose();

			//ResizeImage(Image.FromFile(sourceImage), width, height, constrainProportions, resizeSmaller, bilinear).Save(destinationImage, QuickProcess.GetImageFormat(destinationImage));
			ResizeImage(_image, width, height, constrainProportions, resizeSmaller, bilinear).Save(destinationImage, QuickProcess.GetImageFormat(destinationImage));*/

			ImageJob job = new ImageJob(sourceImage);
			job.ResizeImage(width,height,constrainProportions,resizeSmaller);
			job.Save(destinationImage);
		}

		/// <summary>
		/// Resizes the image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="constrainProportions">if set to <c>true</c> constrains proportions.</param>
		/// <param name="resizeSmaller">if set to <c>true</c> resizes smaller.</param>
		/// <returns></returns>
		public static Image ResizeImage(Image image, int width, int height, bool constrainProportions, bool resizeSmaller)
		{
			return ResizeImage(image, width, height, constrainProportions, resizeSmaller, false);
		}

		/// <summary>
		/// Resizes the image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="constrainProportions">if set to <c>true</c> constrains proportions.</param>
		/// <param name="resizeSmaller">if set to <c>true</c> resize smaller.</param>
		/// <param name="bilinear">if set to <c>true</c> using bilinear.</param>
		/// <returns></returns>
		public static Image ResizeImage(Image image, int width, int height, bool constrainProportions, bool resizeSmaller, bool bilinear)
		{
			
			Size newSize;

			if  (image.Width > width || image.Height > height || (image.Width < width && image.Height < height) && resizeSmaller)
			{
				if (constrainProportions)
					newSize = ImageHelper.Sizer(image.Width, image.Height, width, height);
				else
					newSize = new Size(width, height);
			}
			else
				return image;

			Image scaledBitmap = new Bitmap((int)newSize.Width, (int)newSize.Height);
			Graphics g = Graphics.FromImage(scaledBitmap);
			Rectangle destRect = new Rectangle(0, 0, (int)newSize.Width, (int)newSize.Height);

			if (bilinear)
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
			else
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			g.DrawImage(image, destRect); 

			return ApplyTrial(scaledBitmap);

			/*Image imageCopy = new Bitmap(image.Width,image.Height);
			Graphics g = Graphics.FromImage(imageCopy);
			g.DrawImage(image,0,0,image.Width,image.Height);
			image.Dispose();

			Size newSize;*/

			/*if  (image.Width > width || image.Height > height || (image.Width < width && image.Height < height) && resizeSmaller)
			{
				if (constrainProportions)
					newSize = ImageHelper.Sizer(image.Width, image.Height, width, height);
				else
					newSize = new Size(width, height);

				Bitmap newImage = new Bitmap(image, (int)newSize.Width, (int)newSize.Height);

				image.Dispose();
				
				return ApplyTrial(newImage);
			}
			else
			{
				return ApplyTrial(image);
			}*/

			
			//if  (dest.Width > width || dest.Height > height || (dest.Width < width && dest.Height < height) && resizeSmaller)
			/*if  (imageCopy.Width > width || imageCopy.Height > height || (imageCopy.Width < width && imageCopy.Height < height) && resizeSmaller)
			{
				if (constrainProportions)
					newSize = ImageHelper.Sizer(imageCopy.Width, imageCopy.Height, width, height);
				else
					newSize = new Size(width, height);

				//Bitmap newImage = new Bitmap(dest, (int)newSize.Width, (int)newSize.Height);

				//dest = new Bitmap(dest, (int)newSize.Width, (int)newSize.Height);
				//dest = Resize((Bitmap)dest, (int)newSize.Width, (int)newSize.Height, bilinear);

				Bitmap dest = new Bitmap((int)newSize.Width, (int)newSize.Height, PixelFormat.Format24bppRgb);

				Graphics g2 = Graphics.FromImage(dest);
				Rectangle destRect = new Rectangle(0, 0, (int)newSize.Width, (int)newSize.Height);
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				g.DrawImage(imageCopy, destRect);

				g.Dispose();
				g2.Dispose();
				
				return ApplyTrial(dest);
			}
			else
			{
				return image;//return ApplyTrial(dest);
			}*/
		}

		/// <summary>
		/// Resizes the specified bitmap.
		/// </summary>
		/// <param name="b">The b.</param>
		/// <param name="nWidth">Image width.</param>
		/// <param name="nHeight">Image width.</param>
		/// <param name="bBilinear">if set to <c>true</c> using bilinear.</param>
		/// <returns></returns>
		public static Bitmap Resize(Bitmap b, int nWidth, int nHeight, bool bBilinear)
		{
			Bitmap bTemp = (Bitmap)b.Clone();
			b = new Bitmap(nWidth, nHeight, bTemp.PixelFormat);

			double nXFactor = (double)bTemp.Width/(double)nWidth;
			double nYFactor = (double)bTemp.Height/(double)nHeight;

			if (bBilinear)
			{
				double fraction_x, fraction_y, one_minus_x, one_minus_y;
				int ceil_x, ceil_y, floor_x, floor_y;
				Color c1 = new Color();
				Color c2 = new Color();
				Color c3 = new Color();
				Color c4 = new Color();
				byte red, green, blue;

				byte b1, b2;

				for (int x = 0; x < b.Width; ++x)
					for (int y = 0; y < b.Height; ++y)
					{
						// Setup

						floor_x = (int)Math.Floor(x * nXFactor);
						floor_y = (int)Math.Floor(y * nYFactor);
						ceil_x = floor_x + 1;
						if (ceil_x >= bTemp.Width) ceil_x = floor_x;
						ceil_y = floor_y + 1;
						if (ceil_y >= bTemp.Height) ceil_y = floor_y;
						fraction_x = x * nXFactor - floor_x;
						fraction_y = y * nYFactor - floor_y;
						one_minus_x = 1.0 - fraction_x;
						one_minus_y = 1.0 - fraction_y;

						c1 = bTemp.GetPixel(floor_x, floor_y);
						c2 = bTemp.GetPixel(ceil_x, floor_y);
						c3 = bTemp.GetPixel(floor_x, ceil_y);
						c4 = bTemp.GetPixel(ceil_x, ceil_y);

						// Blue
						b1 = (byte)(one_minus_x * c1.B + fraction_x * c2.B);

						b2 = (byte)(one_minus_x * c3.B + fraction_x * c4.B);
						
						blue = (byte)(one_minus_y * (double)(b1) + fraction_y * (double)(b2));

						// Green
						b1 = (byte)(one_minus_x * c1.G + fraction_x * c2.G);

						b2 = (byte)(one_minus_x * c3.G + fraction_x * c4.G);
						
						green = (byte)(one_minus_y * (double)(b1) + fraction_y * (double)(b2));

						// Red
						b1 = (byte)(one_minus_x * c1.R + fraction_x * c2.R);

						b2 = (byte)(one_minus_x * c3.R + fraction_x * c4.R);
						
						red = (byte)(one_minus_y * (double)(b1) + fraction_y * (double)(b2));

						b.SetPixel(x,y, System.Drawing.Color.FromArgb(255, red, green, blue));
					}
			}
			else
			{
				for (int x = 0; x < b.Width; ++x)
					for (int y = 0; y < b.Height; ++y)
						b.SetPixel(x, y, bTemp.GetPixel((int)(Math.Floor(x * nXFactor)),(int)(Math.Floor(y * nYFactor))));
			}

			return b;
		}

		/// <summary>
		/// Resizes the canvas.
		/// </summary>
		/// <param name="filename">The file name.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="anchor">The anchor.</param>
		/// <param name="background">The background.</param>
		public static void ResizeCanvas(string filename, int width, int height, AnchorType anchor, Color background)
		{
			Image image = Image.FromFile(filename);

			Image resized = ResizeCanvas(image, width, height, anchor, background);

			image.Dispose();
			resized.Save(filename, QuickProcess.GetImageFormat(filename));
			resized.Dispose();
		}

		/// <summary>
		/// Resizes the canvas.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="anchor">The anchor.</param>
		/// <param name="background">The background.</param>
		/// <returns></returns>
		public static Image ResizeCanvas(Image image, int width, int height, AnchorType anchor, Color background)
		{
			var newImage = new Bitmap(width, height, image.PixelFormat);
			using (var graph = Graphics.FromImage(newImage))
			{
				var oWidth = image.Width;
				var oHeight = image.Height;
				var cropRect = new Rectangle(0, 0, oWidth, oHeight);
				var widthDifference = Math.Abs(oWidth - width);
				var heightDifference = Math.Abs(oHeight - height);
				var widthDifferenceHalf = widthDifference / 2;
				var heightDifferenceHalf = heightDifference / 2;
				var newSize = new Size(width, height);
				var isNewWidthSmallerOrEqual = width <= oWidth;
				var isNewHeightSmallerOrEqual = height <= oHeight;
				var point = new Point(0, 0);

				DlgResizeCanvas dlgResizeCanvas = null;

				switch (anchor)
				{
					case AnchorType.TopLeft:
					case AnchorType.TopCenter:
					case AnchorType.TopRight:
						dlgResizeCanvas = ResizeCanvasHorizontalTop;
						break;
					case AnchorType.MiddleLeft:
					case AnchorType.MiddleCenter:
					case AnchorType.MiddleRight:
						dlgResizeCanvas = ResizeCanvasHorizontalMiddle;
						break;
					case AnchorType.BottomLeft:
					case AnchorType.BottomCenter:
					case AnchorType.BottomRight:
						dlgResizeCanvas = ResizeCanvasHorizontalBottom;
						break;
				}

				dlgResizeCanvas?.Invoke(
					anchor,
					ref cropRect,
					ref point,
					newSize,
					image.Size,
					isNewWidthSmallerOrEqual,
					isNewHeightSmallerOrEqual,
					widthDifference,
					heightDifference,
					widthDifferenceHalf,
					heightDifferenceHalf);

				graph.DrawImage(image, point.X, point.Y, cropRect, GraphicsUnit.Pixel);
			}

			image.Dispose();
			return ApplyTrial(newImage);
		}

		/*public static Image Copy(Image image, Rectangle toCopy, int xPos, int yPos)
		{
			Graphics graph = Graphics.FromImage(image);
			
			graph.DrawImage(image, xPos, yPos, toCopy, GraphicsUnit.Pixel);
			
			Bitmap copied = (Bitmap)image.Clone();
			graph.Dispose();
			image.Dispose();

			return copied;
		}

		public static void Copy(string filename, Rectangle toCopy, int xPos, int yPos)
		{
			Image image = Image.FromFile(filename);

			Bitmap coppied = (Bitmap)Copy(image, toCopy, xPos, yPos);

			image.Dispose();

			coppied.Save(filename + ".sd.jpg");
			coppied.Dispose();
		}*/

		/*public static Image Trim(Image image, System.Drawing.Color color)
		{
			//(y x Width) + x
		}*/

		/// <summary>
		/// Gets the image format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public static System.Drawing.Imaging.ImageFormat GetImageFormat(string format)
		{
			string extension = System.IO.Path.GetExtension(format);

			switch(extension.ToUpper())
			{
				case ".JPG": return System.Drawing.Imaging.ImageFormat.Jpeg;
				case ".GIF": return System.Drawing.Imaging.ImageFormat.Gif;
				case ".BMP": return System.Drawing.Imaging.ImageFormat.Bmp;
				case ".EMF": return System.Drawing.Imaging.ImageFormat.Emf;
				case ".PNG": return System.Drawing.Imaging.ImageFormat.Png;
				case ".TIF":
				case ".TIFF": return System.Drawing.Imaging.ImageFormat.Tiff;
				case ".EXIF": return System.Drawing.Imaging.ImageFormat.Exif;
				case ".WMF": return System.Drawing.Imaging.ImageFormat.Wmf;
			}

			return System.Drawing.Imaging.ImageFormat.Jpeg;
		}

		/// <summary>
		/// Zooms the specified file name.
		/// </summary>
		/// <param name="filename">The file name.</param>
		/// <param name="xPos">The x position.</param>
		/// <param name="yPos">The y position.</param>
		/// <param name="factor">The factor.</param>
		public static void Zoom(string filename, int xPos, int yPos, float factor)
		{
			/*Image image = Image.FromFile(filename);

			image = Zoom(image, xPos, yPos, factor);
			image.Save(filename, QuickProcess.GetImageFormat(filename));
			image.Dispose();*/

			Image src = Image.FromFile(filename);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			dest = Zoom(dest,xPos,yPos,factor);
			g.Dispose(); 
			dest.Save(filename,QuickProcess.GetImageFormat(filename)); 
		}

		/// <summary>
		/// Zooms the specified image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="xPos">The x position.</param>
		/// <param name="yPos">The y position.</param>
		/// <param name="factor">The factor.</param>
		/// <returns></returns>
		public static Image Zoom(Image image, int xPos, int yPos, float factor)
		{
			Bitmap img;
			System.Drawing.Image temp;
			Graphics graph;

			temp=(Image)image.Clone();
			img=new Bitmap(image.Width,image.Height,image.PixelFormat);
			graph = Graphics.FromImage(img);
			graph.Clear(System.Drawing.Color.Black);
			//factor = 100 / (factor / 100);

			factor = 1 / (factor / 100);

			float portionWidth=(image.Width*factor);
			float portionHeight=(image.Height*factor);

			float startXpos = (float)xPos-(portionWidth/2);
			float startYPos = (float)yPos-(portionHeight/2);

			RectangleF desRect = new RectangleF(0, 0, image.Width, image.Height);
			RectangleF sourceRect = new RectangleF(startXpos, startYPos, portionWidth, portionHeight);

			graph.DrawImage(temp,desRect,sourceRect,GraphicsUnit.Pixel);

			temp.Dispose();
			
			return ApplyTrial((Image)img);

		}

		/// <summary>
		/// Adds image text.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="text">The text.</param>
		/// <param name="font">The font.</param>
		/// <param name="size">The size.</param>
		/// <param name="style">The style.</param>
		/// <param name="foreColor">Color of the fore.</param>
		/// <param name="antialias">if set to <c>true</c> using antialias.</param>
		/// <param name="xpos">The x position.</param>
		/// <param name="ypos">The y position.</param>
		/// <param name="aligment">The aligment.</param>
		public static void AddText(string filename, string text, string font, int size, FontStyle style, System.Drawing.Color foreColor, bool antialias, int xpos, int ypos, StringAlignment aligment)
		{
			//Image oImage2 = Image.FromFile(filename);
			//Image image = Image.FromFile(filename);

			/*image = AddText(image, text, font, size, style, antialias, xpos, ypos);

			image.Save(filename);
			image.Dispose();*/
			/*FileStream oFileStream = new FileStream(filename, FileMode.Open);
			Image oImage = Image.FromStream(oFileStream);
			oFileStream.Close();
			oFileStream = null;*/

			//Bitmap cropped = (Bitmap)Crop((Image)oImage, 0, 0, 1, 2);
			//Bitmap cropped = (Bitmap)AddText((Image)oImage, text, font, size, style, antialias, xpos, ypos);

			/*Image oImage = (Image)oImage2.Clone();
			oImage2.Dispose();*/

			//Image cropped = AddText(image, text, font, size, style, foreColor, antialias, xpos, ypos, aligment);
			//Image cropped = (Image)image.Clone();
			//oImage.Dispose();
			
			//oImage.Dispose();

			/*FileInfo fileInfo = new FileInfo(filename);
			if (fileInfo.Exists)
				System.IO.File.Delete(fileInfo.FullName);*/
			
			//cropped.Save(filename + "3.jpg", GetImageFormat(filename));
			//Bitmap imgOut = new Bitmap((System.Drawing.Image)cropped, new Size(cropped.Width,cropped.Height));

			//System.Drawing.Imaging.ImageFormat test = ((Image)cropped).RawFormat;
			//imgOut.Save(filename + "2.jpg", GetImageFormat(filename));
			//imgOut.Dispose();
			/*cropped.Save(filename, GetImageFormat(filename));
			cropped.Dispose();*/

			Image src = Image.FromFile(filename);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			dest = AddText(dest, text, font, size, style, foreColor, antialias, xpos, ypos, aligment);
			g.Dispose(); 
			dest.Save(filename,QuickProcess.GetImageFormat(filename)); 			
		}

		/// <summary>
		/// Adds image text.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="text">The text.</param>
		/// <param name="font">The font.</param>
		/// <param name="size">The size.</param>
		/// <param name="style">The style.</param>
		/// <param name="foreColor">Color of the fore.</param>
		/// <param name="antialias">if set to <c>true</c> using antialias.</param>
		/// <param name="xpos">The x position.</param>
		/// <param name="ypos">The y position.</param>
		/// <param name="aligment">The aligment.</param>
		/// <returns></returns>
		public static Image AddText(Image image, string text, string font, int size, FontStyle style, System.Drawing.Color foreColor, bool antialias, int xpos, int ypos, StringAlignment aligment)
		{
			if (!QuickProcess.IsRegistered())
				text = text + "(Trial)";
			/*Graphics graph = Graphics.FromImage(image);

			//graph.DrawString(text, new Font(font, size, style), SystemBrushes.WindowText, new Point(xpos, ypos));
			
			Bitmap cropped = ((Bitmap)image).Clone(new Rectangle(0, 0, image.Width, image.Height), image.PixelFormat);
			graph.Dispose();
			image.Dispose();

			return cropped;*/

			//Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

			Graphics graph = Graphics.FromImage(image);
			
			if (antialias)
				graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			else
				graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

			//graph.DrawImage(image, rect, rect, GraphicsUnit.Pixel);
			SolidBrush drawBrush = new SolidBrush(foreColor);
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = aligment;

			graph.DrawString(text, new Font(font, size, style), drawBrush, new Point(xpos, ypos), stringFormat);
			//Bitmap cropped = (Bitmap)image.Clone();
			//Image cropped = (Image)image.Clone();
			graph.Dispose();
			//image.Dispose();

			return image;
		}

		/// <summary>
		/// Crops the specified image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="selection">The selection.</param>
		/// <returns></returns>
		public static Image Crop(Image image, Selection selection)
		{
			return Crop(image, selection.X1, selection.Y1, selection.X2, selection.Y2);
		}

		/// <summary>
		/// Crops the specified image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="from">From position.</param>
		/// <param name="to">To position.</param>
		/// <returns></returns>
		public static Image Crop(Image image, Point from, Point to)
		{
			return Crop(image, from.X, from.Y, to.X, to.Y);
		}

		/// <summary>
		/// Crops the images.
		/// </summary>
		/// <param name="filename">The file name.</param>
		/// <param name="selection">The selection.</param>
		public static void Crop(string filename, Selection selection)
		{
			Crop(filename, selection.X1, selection.Y1, selection.X2, selection.Y2);
		}

		/// <summary>
		/// Crops the image.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="from">From position.</param>
		/// <param name="to">To position.</param>
		public static void Crop(string filename, Point from, Point to)
		{
			Crop(filename, from.X, from.Y, to.X, to.Y);
		}

		/// <summary>
		/// Crops the image.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="xFrom">The x from position.</param>
		/// <param name="yFrom">The y from position.</param>
		/// <param name="xTo">The x to position.</param>
		/// <param name="yTo">The y to position.</param>
		public static void Crop(string filename, int xFrom, int yFrom, int xTo, int yTo)
		{
			Image oImage = Image.FromFile(filename);

			Bitmap cropped = (Bitmap)Crop((Image)oImage, xFrom, yFrom, xTo, yTo);

			oImage.Dispose();
			
			cropped.Save(filename, QuickProcess.GetImageFormat(filename));
			cropped.Dispose();
		}

		/// <summary>
		/// Crops the image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="xFrom">The x from position.</param>
		/// <param name="yFrom">The y from position.</param>
		/// <param name="xTo">The x to position.</param>
		/// <param name="yTo">The y to position.</param>
		/// <returns></returns>
		public static Image Crop(Image image, int xFrom, int yFrom, int xTo, int yTo)
		{
			Bitmap cropped = ((Bitmap)image).Clone(new Rectangle(xFrom, yFrom, xTo - xFrom, yTo - yFrom), image.PixelFormat);
			image.Dispose();
			return ApplyTrial(cropped);
		}

		/// <summary>
		/// Flips the image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="flipType">Flip type..</param>
		/// <returns></returns>
		public static Image Flip(Image image, FlipType flipType)
		{
			switch (flipType)
			{
				case FlipType.Horizontal:
					image.RotateFlip(RotateFlipType.RotateNoneFlipX); break;
				case FlipType.Vertical:
					image.RotateFlip(RotateFlipType.RotateNoneFlipY); break;
				case FlipType.Both:
					image.RotateFlip(RotateFlipType.RotateNoneFlipX);
					image.RotateFlip(RotateFlipType.RotateNoneFlipY); break;
			}

			return ApplyTrial(image);
		}

		/// <summary>
		/// Flips the image.
		/// </summary>
		/// <param name="filename">The file name.</param>
		/// <param name="flipType">The flip type..</param>
		public static void Flip(string filename, FlipType flipType)
		{
			Image src = Image.FromFile(filename);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			dest = Flip(dest,flipType);
			g.Dispose(); 
			dest.Save(filename,QuickProcess.GetImageFormat(filename)); 
			
		}

		/// <summary>
		/// Rotates the image.
		/// </summary>
		/// <param name="filename">The file name.</param>
		/// <param name="angle">The angle.</param>
		public static void Rotate(string filename, float angle)
		{
			Rotate(Image.FromFile(filename), angle).Save(filename,QuickProcess.GetImageFormat(filename));
		}

		/// <summary>
		/// Rotates the image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="angle">The angle.</param>
		/// <returns></returns>
		public static Image Rotate(Image image, float angle)
		{
			var rotated = false;

			if (angle < -Degree360 || angle > Degree360)
			{
				throw new InvalidOperationException("rotation value must be between -360 and 360.");
			}

			if (angle < Degree0)
			{
				angle -= Degree360;
			}

			RotateFlipType rotateFlipType;
			GetRotateFlipType(angle, out rotateFlipType, out rotated);

			if (rotated)
			{
				if (rotateFlipType != RotateFlipType.RotateNoneFlipNone)
				{
					image.RotateFlip(rotateFlipType);
				}

				return ApplyTrial(image);
			}

			const double pi2 = Math.PI / 2.0;

			var theta = angle * Math.PI / Degree180;
			var lockedTheta = theta;

			while (lockedTheta < 0.0)
				lockedTheta += 2 * Math.PI;

			var isLockedThetaInRange = (lockedTheta >= 0.0 && lockedTheta < pi2) ||
			                           (lockedTheta >= Math.PI && lockedTheta < (Math.PI + pi2));

			var rotatedImage = GetBitmapAdjacentOppositeValues(
				lockedTheta,
				isLockedThetaInRange,
				pi2,
				image);

			image.Dispose();
			return ApplyTrial(rotatedImage);
		}

		/// <summary>
		/// Rotates at left.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public static void RotateLeft(string filename)
		{
			Rotate(filename, 270);
		}

		/// <summary>
		/// Rotates at right.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public static void RotateRight(string filename)
		{
			Rotate(filename, 90);
		}

		/// <summary>
		/// Rotates at left.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <returns></returns>
		public static Image RotateLeft(Image image)
		{
			return Rotate(image, 270);
		}

		/// <summary>
		/// Rotates at right.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <returns></returns>
		public static Image RotateRight(Image image)
		{
			return Rotate(image, 90);
		}

		/// <summary>
		/// Brightnesses the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="brightness">The brightness.</param>
		public static void Brightness(string sourceImage, int brightness)
		{
			Brightness(sourceImage,sourceImage,brightness);
		}

		/// <summary>
		/// Brightnesses the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destImage">The destination image.</param>
		/// <param name="brightness">The brightness.</param>
		public static void Brightness(string sourceImage, string destImage, int brightness)
		{
			Image src = Image.FromFile(sourceImage);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			Brightness(dest,brightness);
			g.Dispose(); 
			dest.Save(destImage,QuickProcess.GetImageFormat(destImage)); 			
		}

		/// <summary>
		/// Brightnesses the specified image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="brightness">The brightness.</param>
		/// <returns></returns>
		public static Image Brightness(Image image, int brightness)
		{
			Bitmap workImage = (Bitmap)image;
			if( brightness < -255 ) brightness = -255;
			if( brightness > 255 ) brightness = 255;

			for( int x = 0; x < workImage.Width; x++)
				for( int y = 0; y < workImage.Height; y++)
				{
					Color normal = workImage.GetPixel( x, y );
					Color bright = Color.FromArgb( SmartByte( normal.R + brightness ) ,
						SmartByte( normal.G + brightness ),
						SmartByte( normal.B + brightness ));
					workImage.SetPixel( x, y, bright);
				}
			return ApplyTrial((Image)workImage);
		}
	
		/// <summary>
		/// Smart byte functions transform integer to byte
		/// with our way.
		/// </summary>
		/// <param name="_byte">The _byte.</param>
		/// <returns></returns>
		private static byte SmartByte(int _byte)
		{
			if( _byte < 0 ) _byte = 0;
			if( _byte > 255) _byte = 255;
			return (byte) _byte;
		}

		
		/// <summary>
		/// Colorizes the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="red">The red color.</param>
		/// <param name="green">The green color.</param>
		/// <param name="blue">The blue color.</param>
		public static void Colorize(string sourceImage, int red, int green, int blue)
		{
			Colorize(sourceImage,sourceImage,red, green, blue);
		}

		/// <summary>
		/// Colorizes the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destImage">The destination image.</param>
		/// <param name="red">The red color.</param>
		/// <param name="green">The green color.</param>
		/// <param name="blue">The blue color.</param>
		public static void Colorize(string sourceImage, string destImage, int red, int green, int blue)
		{
			Image src = Image.FromFile(sourceImage);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			Colorize(dest,red, green, blue);
			g.Dispose(); 
			dest.Save(destImage,QuickProcess.GetImageFormat(destImage)); 			
		}

		/// <summary>
		/// Colorizes the specified image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="red">The red color.</param>
		/// <param name="green">The green color.</param>
		/// <param name="blue">The blue color.</param>
		/// <returns></returns>
		public static Image Colorize(Image image, int red, int green, int blue)
		{
			Bitmap workImage = (Bitmap)image;

			if( red < -255 ) red = -255;
			if( red > 255 ) red = 255;

			if( green < -255 ) green = -255;
			if( green > 255 ) green = 255;

			if( blue < -255 ) blue = -255;
			if( blue > 255 ) blue = 255;

			for( int x = 0; x < workImage.Width; x++)
				for( int y = 0; y < workImage.Height; y++)
				{
					Color normal = workImage.GetPixel( x, y );
					int _red = Math.Max( normal.R + red, 0);
					int _green = Math.Max( normal.G + green, 0);
					int _blue = Math.Max( normal.B + blue, 0);

					if(_red > 255) _red = 255;
					if(_green > 255) _green = 255;
					if(_blue > 255) _blue = 255;

					Color contrasted = Color.FromArgb( 
						_red,
						_green,
						_blue 
						);
					workImage.SetPixel( x, y, contrasted );
				}

			return ApplyTrial((Image)workImage);
		}

		/// <summary>
		/// Contrasts the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="contrastValue">The contrast value.</param>
		public static void Contrast(string sourceImage, int contrastValue)
		{
			Contrast(sourceImage,sourceImage,contrastValue);
		}

		/// <summary>
		/// Contrasts the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destImage">The destination image.</param>
		/// <param name="contrastValue">The contrast value.</param>
		public static void Contrast(string sourceImage, string destImage, int contrastValue)
		{
			Image src = Image.FromFile(sourceImage);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			Contrast(dest,contrastValue);
			g.Dispose(); 
			dest.Save(destImage,QuickProcess.GetImageFormat(destImage)); 			
		}

		/// <summary>
		/// Change contrast of the image
		/// </summary>
		public static Image Contrast(Image image, int constrastValue)
		{
			Bitmap workImage = (Bitmap)image;

			if( constrastValue < -100 )constrastValue = -100;
			if( constrastValue > 100 )constrastValue = 100;

			double dContrast = (100.0 + constrastValue) /100.0;

			for( int x = 0; x < workImage.Width; x++)
				for( int y = 0; y < workImage.Height; y++)
				{
					Color normal = workImage.GetPixel( x, y );
					Color contrasted = Color.FromArgb( 
						ContrastColor(normal.R, dContrast),
						ContrastColor(normal.G, dContrast),
						ContrastColor(normal.B, dContrast)
						);
					workImage.SetPixel( x, y, contrasted );
				}

			return ApplyTrial((Image)workImage);
		}

		/// <summary>
		/// Contrasts the color with specified value.
		/// </summary>
		/// <param name="color">color value</param>
		/// <param name="contrast">contrast value </param>
		/// <returns>contrasted color</returns>
		private static byte ContrastColor(byte color, double contrast )
		{
			double pixel = 0;
			pixel = (color/255.0 - 0.5) * contrast + 0.5;
			pixel *= 255;
			if ( pixel < 0 ) pixel = 0;
			if ( pixel > 255 ) pixel = 255;

			return (byte) pixel;
		}

		/// <summary>
		/// Gammas the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="red">The red color.</param>
		/// <param name="green">The green color.</param>
		/// <param name="blue">The blue color.</param>
		public static void Gamma(string sourceImage, int red, int green, int blue)
		{
			Gamma(sourceImage,sourceImage,red, green, blue);
		}

		/// <summary>
		/// Gammas the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destImage">The destination image.</param>
		/// <param name="red">The red color.</param>
		/// <param name="green">The green color.</param>
		/// <param name="blue">The blue color.</param>
		public static void Gamma(string sourceImage, string destImage, int red, int green, int blue)
		{
			Image src = Image.FromFile(sourceImage);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			Gamma(dest,red, green, blue);
			g.Dispose(); 
			dest.Save(destImage,QuickProcess.GetImageFormat(destImage)); 			
		}

		/// <summary>
		/// Change gamma of the picture using gamma pallete
		/// </summary>
		public static Image Gamma(Image image, int red, int green, int blue)
		{
			Bitmap workImage = (Bitmap)image;

			double redLoc = red;
			if( redLoc < 0 ) red = 0;
			if( redLoc > 5 ) redLoc = 0.2;

			double greenLoc = green;
			if( greenLoc < 0 ) greenLoc = 0;	
			if( greenLoc > 5 ) greenLoc = 0.2;

			double blueLoc = blue;
			if( blueLoc < 0 ) blueLoc = 0;
			if( blueLoc > 5 ) blueLoc = .2;

			byte [] redGamma = new byte [256];
			byte [] greenGamma = new byte [256];
			byte [] blueGamma = new byte [256];
			
			for (int i = 0; i< 256; ++i)
			{
				redGamma[i] = (byte)Math.Min(255, (int)(( 255.0 * Math.Pow(i/255.0, 1.0/redLoc)) + 0.5));
				greenGamma[i] = (byte)Math.Min(255, (int)(( 255.0 * Math.Pow(i/255.0, 1.0/greenLoc)) + 0.5));
				blueGamma[i] = (byte)Math.Min(255, (int)(( 255.0 * Math.Pow(i/255.0, 1.0/blueLoc)) + 0.5));
			}

			for( int x = 0; x < workImage.Width; x++)
				for( int y = 0; y < workImage.Height; y++)
				{
					Color normal = workImage.GetPixel( x, y );
					Color contrasted = Color.FromArgb( 
						redLoc != .0 ? redGamma[ normal.R ] : normal.R,
						greenLoc != .0 ? greenGamma[ normal.G ] : normal.G,
						blueLoc != .0 ? blueGamma[ normal.B ] : normal.B
						);
					workImage.SetPixel( x, y, contrasted );
				}

			return ApplyTrial((Image)workImage);
		}

		/// <summary>
		/// Grayscale the image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		public static void GrayScale(string sourceImage)
		{
			GrayScale(sourceImage,sourceImage);
		}

		/// <summary>
		/// Grayscale the image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destImage">The destination image.</param>
		public static void GrayScale(string sourceImage, string destImage)
		{
			Image src = Image.FromFile(sourceImage);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			GrayScale(dest);
			g.Dispose(); 
			dest.Save(destImage,QuickProcess.GetImageFormat(destImage)); 			
		}

		/// <summary>
		/// Grayscale filter for image
		/// </summary>
		public static Image GrayScale(Image image)
		{
			Bitmap workImage = (Bitmap)image;

			for( int x = 0; x < workImage.Width; x++)
				for( int y = 0; y < workImage.Height; y++)
				{
					Color normal = workImage.GetPixel( x, y );
					byte grayByte = (byte)(.299 * normal.R + .587 * normal.G + .114 * normal.B);
					Color gray = Color.FromArgb( grayByte , grayByte, grayByte);
					workImage.SetPixel( x, y, gray);
				}

			return ApplyTrial((Image)workImage);
		}

		/// <summary>
		/// Inverts the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		public static void Invert(string sourceImage)
		{
			Invert(sourceImage,sourceImage);
		}

		/// <summary>
		/// Inverts the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destImage">The destination image.</param>
		public static void Invert(string sourceImage, string destImage)
		{
			Image src = Image.FromFile(sourceImage);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			Invert(dest);
			g.Dispose(); 
			dest.Save(destImage,QuickProcess.GetImageFormat(destImage)); 			
		}

		/// <summary>
		/// Inverts colors on image.
		/// </summary>
		public static Image Invert(Image image)
		{
			Bitmap workImage = (Bitmap)image;

			for( int y = 0; y < workImage.Height; y++)
				for( int x = 0; x < workImage.Width; x++)
				{
					Color normal = workImage.GetPixel( x, y );
					Color invert = Color.FromArgb(255 - normal.R, 255 - normal.G, 255 - normal.B);
					workImage.SetPixel( x, y, invert);
				}

			return ApplyTrial((Image)workImage);
		}

		/// <summary>
		/// Image edge detection.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="threshold">The threshold.</param>
		public static void EdgeDetection(string sourceImage, byte threshold)
		{
			EdgeDetection(sourceImage,sourceImage,threshold);
		}

		/// <summary>
		/// Image edge detection.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destImage">The destination image.</param>
		/// <param name="threshold">The threshold.</param>
		public static void EdgeDetection(string sourceImage, string destImage, byte threshold)
		{
			Image src = Image.FromFile(sourceImage);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			EdgeDetection(dest,threshold);
			g.Dispose(); 
			dest.Save(destImage,QuickProcess.GetImageFormat(destImage)); 	
		}

		private static byte EdgePixel(byte pixel1, byte pixel2, byte threshold)
		{
			int pixel = (int) Math.Sqrt( pixel1 * pixel1 + pixel2 * pixel2 );
			if( pixel < threshold )pixel = threshold;
			if( pixel > 255 )pixel = 255;
			return (byte) pixel;
		}

		
		/// <summary>
		/// Image edges detection.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="threshold">The threshold.</param>
		/// <returns></returns>
		public static Image EdgeDetection(Image image, byte threshold)
		{
			Bitmap workImage = (Bitmap)image;

			ConvertMatrix convMatrix = new ConvertMatrix();

			Bitmap edgeBitmap = (Bitmap)workImage.Clone();
			
			convMatrix.SetAll(-3);
			convMatrix.Pixel = 0;
			convMatrix.TopLeft = convMatrix.MidLeft = convMatrix.BottomLeft = 5;
			convMatrix.Offset = 0;

			ConvertMatrix.Convert3x3(workImage, convMatrix);

			convMatrix.SetAll(-3);
			convMatrix.Pixel = 0;
			convMatrix.BottomLeft = convMatrix.BottomMid = convMatrix.BottomRight = 5;
			convMatrix.Offset = 0;

			ConvertMatrix.Convert3x3(edgeBitmap, convMatrix);
			for( int x = 0; x < workImage.Width; x++)
				for( int y = 0; y < workImage.Height; y++)
				{
					Color color1 = workImage.GetPixel( x, y );
					Color color2 = edgeBitmap.GetPixel( x, y );
					
					Color edged = Color.FromArgb( 
						EdgePixel(color1.R, color2.R, threshold),
						EdgePixel(color1.G, color2.G, threshold),
						EdgePixel(color1.B, color2.B, threshold));
				}

			return ApplyTrial((Image)workImage);
		}

		/// <summary>
		/// Jiffers the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="degree">The degree.</param>
		public static void Jiffer(string sourceImage, int degree)
		{
			Jiffer(sourceImage,sourceImage,degree);
		}

		/// <summary>
		/// Jiffers the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destImage">The destination image.</param>
		/// <param name="degree">The degree.</param>
		public static void Jiffer(string sourceImage, string destImage, int degree)
		{
			Image src = Image.FromFile(sourceImage);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			Jiffer(dest,degree);
			g.Dispose(); 
			dest.Save(destImage,QuickProcess.GetImageFormat(destImage)); 			
		}

		/// <summary>
		/// Making random jiffer for image. 
		/// </summary>
		public static Image Jiffer(Image image, int degree)
		{
			Bitmap workImage = (Bitmap)image;

			Bitmap jifferBitmap = (Bitmap) workImage.Clone();
			short halfDegree = (short)Math.Floor((double)degree/2);
			Random rnd = new Random();

			Point vector = new Point(0,0);

			for( int x = 0; x < workImage.Width; x++)
			{
				for( int y = 0; y < workImage.Height; y++)
				{
					Color normal = workImage.GetPixel( x, y );
					vector.X = rnd.Next(degree) - halfDegree;
					vector.Y = rnd.Next(degree) - halfDegree;

					if (x + vector.X < 0 || x + vector.X >= workImage.Width)  vector.X = 0;
					if (y + vector.Y < 0 || y + vector.Y >= workImage.Height) vector.Y = 0;

					jifferBitmap.SetPixel( x + vector.X, y + vector.Y, normal );
				}
			}
			workImage = jifferBitmap;

			return ApplyTrial((Image)workImage);
		}

		/// <summary>
		/// Luminances the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="factor">The factor.</param>
		public static void Luminance(string sourceImage, int factor)
		{
			Luminance(sourceImage,sourceImage,factor);
		}

		/// <summary>
		/// Luminances the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destImage">The destination image.</param>
		/// <param name="factor">The factor.</param>
		public static void Luminance(string sourceImage, string destImage, int factor)
		{
			Image src = Image.FromFile(sourceImage);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			Luminance(dest,factor);
			g.Dispose(); 
			dest.Save(destImage,QuickProcess.GetImageFormat(destImage)); 			
		}

		/// <summary>
		/// Water effect for specified wave size
		/// </summary>
		public static Image Luminance(Image image, int factor)
		{
			Bitmap workImage = (Bitmap)image;

			for( int x = 0; x < workImage.Width; x++)
				for( int y = 0; y < workImage.Height; y++)
				{
					Color normal = workImage.GetPixel( x, y );
					HSL hsl = HSL.FromRGB( normal.B, normal.G, normal.R); 
					hsl.Luminance *= factor;

					Color saturated = hsl.RGB;
					workImage.SetPixel( x, y, saturated );
				}

			return ApplyTrial((Image)workImage);
		}

		/// <summary>
		/// Pixelizes the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="pixelSize">Size of the pixel.</param>
		/// <param name="grid">if set to <c>true</c> using grid.</param>
		/// <param name="gridColor">The grid color.</param>
		public static void Pixelize(string sourceImage, short pixelSize, bool grid, Color gridColor)
		{
			Pixelize(sourceImage,sourceImage,pixelSize, grid, gridColor);
		}

		/// <summary>
		/// Pixelizes the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destImage">The dest image.</param>
		/// <param name="pixelSize">Size of the pixel.</param>
		/// <param name="grid">if set to <c>true</c> using grid.</param>
		/// <param name="gridColor">The grid color.</param>
		public static void Pixelize(string sourceImage, string destImage, short pixelSize, bool grid, Color gridColor)
		{
			Image src = Image.FromFile(sourceImage);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			Pixelize(dest,pixelSize, grid, gridColor);
			g.Dispose(); 
			dest.Save(destImage,QuickProcess.GetImageFormat(destImage)); 			
		}

		/// <summary>
		/// Pixelize effect for specified wave size
		/// </summary>
		public static Image Pixelize(Image image, short pixelSize, bool grid, Color gridColor)
		{
			Bitmap workImage = (Bitmap)image;
			
			Bitmap pixelizeBitmap = new Bitmap( workImage.Width, workImage.Height);

			for( int x = 0; x < workImage.Width; x++)
				for( int y = 0; y < workImage.Height; y++)
				{
					int dX = x - x%pixelSize;
					int dY = y - y%pixelSize;
					Color normal = workImage.GetPixel( dX, dY );
					int newX = x;
					int newY = y;
					//check range
					if ( grid && x%pixelSize == 0 || grid && y%pixelSize == 0)
						normal = gridColor;
					pixelizeBitmap.SetPixel( newX, newY , normal );
				}
			workImage = (Bitmap)pixelizeBitmap.Clone();

			return ApplyTrial((Image)workImage);
		}

		/// <summary>
		/// Saturations the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="factor">The factor.</param>
		public static void Saturation(string sourceImage, int factor)
		{
			Saturation(sourceImage,sourceImage,factor);
		}

		/// <summary>
		/// Saturations the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destImage">The destination image.</param>
		/// <param name="factor">The factor.</param>
		public static void Saturation(string sourceImage, string destImage, int factor)
		{
			Image src = Image.FromFile(sourceImage);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			Saturation(dest,factor);
			g.Dispose(); 
			dest.Save(destImage,QuickProcess.GetImageFormat(destImage)); 			
		}

		/// <summary>
		/// Change saturation of the picture using gamma pallete
		/// </summary>
		public static Image Saturation(Image image, int factor)
		{
			Bitmap workImage = (Bitmap)image;

			for( int x = 0; x < workImage.Width; x++)
				for( int y = 0; y < workImage.Height; y++)
				{
					Color normal = workImage.GetPixel( x, y );
					HSL hsl = HSL.FromRGB( normal.B, normal.G, normal.R); 
					hsl.Saturation *= factor;

					Color saturated = hsl.RGB;
					workImage.SetPixel( x, y, saturated );
				} 

			return ApplyTrial((Image)workImage);
		}

		/// <summary>
		/// Spheres the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		public static void Sphere(string sourceImage)
		{
			Sphere(sourceImage,sourceImage);
		}

		/// <summary>
		/// Spheres the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destImage">The destination image.</param>
		public static void Sphere(string sourceImage, string destImage)
		{
			Image src = Image.FromFile(sourceImage);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			Sphere(dest);
			g.Dispose(); 
			dest.Save(destImage,QuickProcess.GetImageFormat(destImage)); 			
		}

		/// <summary>
		/// Sphere effect apply to the image.
		/// </summary>
		public static Image Sphere(Image image)
		{
			Bitmap workImage = (Bitmap)image;

			int centerX = (int)Math.Floor( (double)workImage.Width / 2 );
			int centerY = (int)Math.Floor((double)workImage.Height / 2);

			for( int x = 0; x < workImage.Width; x++)
				for( int y = 0; y < workImage.Height; y++)
				{
					Color normal = workImage.GetPixel( x, y );
					double relX = x - centerX;
					double relY = y - centerY;
					double theta = Math.Atan2( relY, relX );
					double radius = Math.Sqrt( relX*relX + relY*relY);
					double newRadius = radius * radius/(Math.Max(centerX, centerY));
					double newX = centerX + newRadius * Math.Cos( theta );
					double newY = centerY + newRadius * Math.Sin( theta );
					if ( newX < 0 || newX >= workImage.Width)newX = 0;
					if ( newY < 0 || newY >= workImage.Height)newY = 0;
					workImage.SetPixel( (int)newX, (int)newY, normal );
				}

			return ApplyTrial((Image)workImage);
		}

		/// <summary>
		/// Waters the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="wave">The wave.</param>
		public static void Water(string sourceImage, int wave)
		{
			Water(sourceImage,sourceImage,wave);
		}

		/// <summary>
		/// Waters the specified source image.
		/// </summary>
		/// <param name="sourceImage">The source image.</param>
		/// <param name="destImage">The destination image.</param>
		/// <param name="wave">The wave.</param>
		public static void Water(string sourceImage, string destImage, int wave)
		{
			Image src = Image.FromFile(sourceImage);
			Image dest = new Bitmap(src.Width,src.Height);
			Graphics g = Graphics.FromImage(dest);
			g.DrawImage(src,0,0,src.Width,src.Height);
			src.Dispose(); 
			Water(dest,wave);
			g.Dispose(); 
			dest.Save(destImage,QuickProcess.GetImageFormat(destImage)); 			
		}

		/// <summary>
		/// Waters the specified image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="wave">The wave.</param>
		/// <returns></returns>
		public static Image Water(Image image, int wave)
		{
			Bitmap workImage = (Bitmap)image;
			
			for( int x = 0; x < workImage.Width; x++)
			{
				for( int y = 0; y < workImage.Height; y++)
				{
					Color normal = workImage.GetPixel( x, y );

					double dX = ((double)wave * Math.Sin(2.0 * 3.1415 * (float)y / 128.0));
					double dY = ((double)wave * Math.Cos(2.0 * 3.1415 * (float)x / 128.0));

					if ( x + dX < 0 || x + dX >= workImage.Width) dX = 0;
					if ( y + dY < 0 || y + dY >= workImage.Height)dY = 0;

					workImage.SetPixel( (int)(x + dX), (int)(y + dY), normal );
				}
			}

			return ApplyTrial((Image)workImage);
		}

		private static void GetRotateFlipType(float angle, out RotateFlipType rotateFlipType, out bool rotated)
		{
			rotateFlipType = RotateFlipType.RotateNoneFlipNone;

			if (angle == Degree90)
			{
				rotateFlipType = RotateFlipType.Rotate90FlipNone;
				rotated = true;
			}
			else if (angle == Degree180)
			{
				rotateFlipType = RotateFlipType.Rotate180FlipNone;
				rotated = true;
			}
			else if (angle == Degree270)
			{
				rotateFlipType = RotateFlipType.Rotate270FlipNone;
				rotated = true;
			}
			else if (angle == Degree0 || angle == Degree360)
			{
				rotated = true;
			}
			else
			{
				rotated = false;
			}
		}

		private static Point[] GetPoints(
			double lockedtheta,
			double pi2,
			int nWidth,
			int nHeight,
			double adjacentTop,
			double adjacentBottom,
			double oppositeTop,
			double oppositeBottom)
		{
			if (lockedtheta >= 0.0 && lockedtheta < pi2)
			{
				return new[]
				{
					new Point((int) oppositeBottom, 0),
					new Point(nWidth, (int) oppositeTop),
					new Point(0, (int) adjacentBottom)
				};
			}

			if (lockedtheta >= pi2 && lockedtheta < Math.PI)
			{
				return new[]
				{
					new Point(nWidth, (int) oppositeTop),
					new Point((int) adjacentTop, nHeight),
					new Point((int) oppositeBottom, 0)
				};
			}

			if (lockedtheta >= Math.PI && lockedtheta < (Math.PI + pi2))
			{
				return new[]
				{
					new Point((int) adjacentTop, nHeight),
					new Point(0, (int) adjacentBottom),
					new Point(nWidth, (int) oppositeTop)
				};
			}

			return new[]
			{
				new Point(0, (int) adjacentBottom),
				new Point((int) oppositeBottom, 0),
				new Point((int) adjacentTop, nHeight)
			};
		}

		private static Bitmap GetBitmapAdjacentOppositeValues(
			double lockedtheta,
			bool isLockedThetaInRange,
			double pi2,
			Image image)
		{
			var cosLockedTheta = Math.Abs(Math.Cos(lockedtheta));
			var sinLockedTheta = Math.Abs(Math.Sin(lockedtheta));
			var oldWidth = image.Width;
			var oldHeight = image.Height;
			var adjacentTop = isLockedThetaInRange ? cosLockedTheta * oldWidth : sinLockedTheta * oldHeight;
			var oppositeTop = isLockedThetaInRange ? sinLockedTheta * oldWidth : cosLockedTheta * oldHeight;
			var adjacentBottom = isLockedThetaInRange ? cosLockedTheta * oldHeight : sinLockedTheta * oldWidth;
			var oppositeBottom = isLockedThetaInRange ? sinLockedTheta * oldHeight : cosLockedTheta * oldWidth;
			var nWidth = (int)Math.Ceiling(adjacentTop + oppositeBottom);
			var nHeight = (int)Math.Ceiling(adjacentBottom + oppositeTop);

			var tempImage = new Bitmap(nWidth, nHeight);
			using (var graphics = Graphics.FromImage(tempImage))
			{
				graphics.Clear(Color.Black);
				var points = GetPoints(
					lockedtheta,
					pi2,
					nWidth,
					nHeight,
					adjacentTop,
					adjacentBottom,
					oppositeTop,
					oppositeBottom);

				graphics.DrawImage(image, points);
			}

			return new Bitmap(nWidth, nHeight);
		}

		private static void ResizeCanvasHorizontalTop(
			AnchorType anchor,
			ref Rectangle cropRect,
			ref Point point,
			Size newSize,
			Size origSize,
			bool isNewWidthSmallerOrEqual,
			bool isNewHeightSmallerOrEqual,
			int widthDifference,
			int heightDifference,
			int widthDifferenceHalf,
			int heightDifferenceHalf)
		{
			switch (anchor)
			{
				case AnchorType.TopLeft:
					break;
				case AnchorType.TopCenter:
					if (isNewWidthSmallerOrEqual)
					{
						cropRect = new Rectangle(widthDifferenceHalf, 0, newSize.Width, origSize.Height);
					}
					else
					{
						point = new Point(widthDifferenceHalf, 0);
					}

					break;
				case AnchorType.TopRight:
					if (isNewWidthSmallerOrEqual)
					{
						cropRect = new Rectangle(widthDifference, 0, newSize.Width, newSize.Height);
					}
					else
					{
						point = new Point(widthDifference, 0);
					}

					break;
			}
		}

		private static void ResizeCanvasHorizontalMiddle(
			AnchorType anchor,
			ref Rectangle cropRect,
			ref Point point,
			Size newSize,
			Size origSize,
			bool isNewWidthSmallerOrEqual,
			bool isNewHeightSmallerOrEqual,
			int widthDifference,
			int heightDifference,
			int widthDifferenceHalf,
			int heightDifferenceHalf)
		{
			if ((isNewWidthSmallerOrEqual && !isNewHeightSmallerOrEqual) ||
				(anchor == AnchorType.MiddleLeft && !isNewWidthSmallerOrEqual && !isNewHeightSmallerOrEqual))
			{
				point = new Point(0, heightDifferenceHalf);
			}

			var rectX = isNewHeightSmallerOrEqual ? widthDifferenceHalf : 0;
			var rectY = isNewHeightSmallerOrEqual ? heightDifferenceHalf : 0;
			var rectWidth = isNewHeightSmallerOrEqual ? newSize.Width : origSize.Width;
			var rectHeight = isNewHeightSmallerOrEqual ? newSize.Height : origSize.Height;

			switch (anchor)
			{
				case AnchorType.MiddleLeft:
					if (isNewWidthSmallerOrEqual)
					{
						cropRect = new Rectangle(0, rectY, newSize.Width, origSize.Height);
					}
					else if (isNewHeightSmallerOrEqual)
					{
						cropRect = new Rectangle(0, heightDifferenceHalf, origSize.Width, newSize.Height);
					}

					break;
				case AnchorType.MiddleCenter:
					if (isNewWidthSmallerOrEqual)
					{
						cropRect = new Rectangle(rectX, rectY, rectWidth, newSize.Height);
					}
					else if (isNewHeightSmallerOrEqual)
					{
						point = new Point(widthDifferenceHalf, 0);
						cropRect = new Rectangle(0, heightDifferenceHalf, origSize.Width, newSize.Height);
					}
					else
					{
						point = new Point(widthDifferenceHalf, heightDifferenceHalf);
					}

					break;
				case AnchorType.MiddleRight:
					if (isNewWidthSmallerOrEqual)
					{
						cropRect = new Rectangle(widthDifferenceHalf, rectY, newSize.Width, rectHeight);
					}
					else if (isNewHeightSmallerOrEqual)
					{
						point = new Point(widthDifferenceHalf, 0);
						cropRect = new Rectangle(0, heightDifferenceHalf, origSize.Width, newSize.Height);
					}
					else
					{
						point = new Point(widthDifference, heightDifferenceHalf);
					}

					break;
			}
		}

		private static void ResizeCanvasHorizontalBottom(
			AnchorType anchor,
			ref Rectangle cropRect,
			ref Point point,
			Size newSize,
			Size origSize,
			bool isNewWidthSmallerOrEqual,
			bool isNewHeightSmallerOrEqual,
			int widthDifference,
			int heightDifference,
			int widthDifferenceHalf,
			int heightDifferenceHalf)
		{
			if ((isNewWidthSmallerOrEqual && !isNewHeightSmallerOrEqual) ||
				(anchor == AnchorType.BottomLeft && !isNewWidthSmallerOrEqual && !isNewHeightSmallerOrEqual))
			{
				point = new Point(0, heightDifference);
			}

			var rectY = isNewHeightSmallerOrEqual ? heightDifference : 0;
			var rectHeight = isNewHeightSmallerOrEqual ? newSize.Height : origSize.Height;

			switch (anchor)
			{
				case AnchorType.BottomLeft:
					if (isNewWidthSmallerOrEqual)
					{
						cropRect = new Rectangle(0, rectY, newSize.Width, rectHeight);
					}
					else if (isNewHeightSmallerOrEqual)
					{
						cropRect = new Rectangle(0, heightDifference, origSize.Width, newSize.Height);
					}

					break;
				case AnchorType.BottomCenter:
					if (isNewWidthSmallerOrEqual)
					{
						cropRect = new Rectangle(widthDifferenceHalf, rectY, newSize.Width, rectHeight);
					}
					else if (isNewHeightSmallerOrEqual)
					{
						point = new Point(widthDifferenceHalf, 0);
						cropRect = new Rectangle(0, heightDifference, origSize.Width, newSize.Height);
					}
					else
					{
						point = new Point(widthDifferenceHalf, heightDifference);
					}

					break;
				case AnchorType.BottomRight:
					if (isNewWidthSmallerOrEqual)
					{
						cropRect = new Rectangle(widthDifference, rectY, newSize.Width, rectHeight);
					}
					else if (isNewHeightSmallerOrEqual)
					{
						point = new Point(widthDifference, 0);
						cropRect = new Rectangle(0, heightDifference, origSize.Width, newSize.Height);
					}
					else
					{
						point = new Point(widthDifference, heightDifference);
					}

					break;
			}
		}
	}
}
