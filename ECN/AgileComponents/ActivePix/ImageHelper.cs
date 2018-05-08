using System;
using System.IO;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ImageHelper"/> object.
	/// </summary>
	public class ImageHelper
	{
		/// <summary>
		/// Sizers the specified current width.
		/// </summary>
		/// <param name="currentWidth">Current width.</param>
		/// <param name="currentHeight">Current height.</param>
		/// <param name="maxWidth">Maximum width.</param>
		/// <param name="maxHeight">Maximum heigth.</param>
		/// <returns></returns>
		public static Size Sizer(int currentWidth, int currentHeight, int maxWidth, int maxHeight)  
		{
			Size size = new Size(currentWidth, currentHeight);

			if ((double)currentWidth / (double)maxWidth > (double)currentHeight / (double)maxHeight)
			{
				size.Width = maxWidth;
				size.Height = currentHeight * maxWidth / currentWidth;
			}
			else
			{
				size.Height = maxHeight;
				size.Width = currentWidth * maxHeight / currentHeight;
			}
			return size;
		}

		/// <summary>
		/// Gets the image format.
		/// </summary>
		/// <param name="_outputFormat">The output format.</param>
		/// <returns></returns>
		public static System.Drawing.Imaging.ImageFormat GetImageFormat(string _outputFormat)
		{
			switch (_outputFormat.ToLower())
			{
				case "bmp":
				case ".bmp": return System.Drawing.Imaging.ImageFormat.Bmp;
				case "emf":
				case ".emf": return System.Drawing.Imaging.ImageFormat.Emf;
				case "exif":
				case ".exif": return System.Drawing.Imaging.ImageFormat.Exif;
				case "gif":
				case ".gif": return System.Drawing.Imaging.ImageFormat.Gif;
				case "icon":
				case ".ico": return System.Drawing.Imaging.ImageFormat.Icon;
				case "jpeg":
				case ".jpeg":
				case "jpg":
				case ".jpg": return System.Drawing.Imaging.ImageFormat.Jpeg;
				case "png":
				case ".png": return System.Drawing.Imaging.ImageFormat.Png;
				case "tiff":
				case ".tiff":
				case "tif":
				case ".tif": return System.Drawing.Imaging.ImageFormat.Tiff;
				case "wmf":
				case ".wmf": return System.Drawing.Imaging.ImageFormat.Wmf;
			}

			return System.Drawing.Imaging.ImageFormat.Jpeg;

		}

		/// <summary>
		/// Gets the image extetion.
		/// </summary>
		/// <param name="outputFormat">The output format.</param>
		/// <returns></returns>
		public static string GetImageExt(string outputFormat)
		{
			switch (outputFormat.ToString())
			{
				case "bmp": return ".bmp";
				case "emf": return ".emf";
				case "exif": return ".exif";
				case "gif": return ".gif";
				case "icon": return ".ico";
				case "jpeg": return ".jpg";
				case "png": return ".png";
				case "tiff": return ".tif";
				case "wmf": return ".wmf";
			}

			return "";

		}
	}
}
