using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Summary description for ImageToBase64.
	/// </summary>
	public class ImageToBase64
	{
		private ImageToBase64()
		{
		}

		/// <summary>
		/// Transforme une chaîne de caractères
		/// en base 64 en un objet <see cref="System.Drawing.Image"/>
		/// </summary>
		/// <param name="str">Chaîne de caractères</param>
		/// <returns><see cref="System.Drawing.Image"/> résultante</returns>
		public static Image GetImageFromBase64String(string str) 
		{
			return GetImageFromBase64String(str, false);
		}

		/// <summary>
		/// Transforme une chaîne de caractères
		/// en base 64 en un objet <see cref="System.Drawing.Image"/>
		/// </summary>
		/// <param name="str">Chaîne de caractères</param>
		/// <param name="makeTransparent">Si vrai, rend la couleur de fond transparent</param>
		/// <returns><see cref="System.Drawing.Image"/> résultante</returns>
		public static Image GetImageFromBase64String(string str, bool makeTransparent) 
		{
			return GetImageFromBytes(Convert.FromBase64String(str), makeTransparent);
		}

		/// <summary>
		/// Transforme un tableau de <see cref="byte"/>
		/// en base 64 en un objet <see cref="System.Drawing.Image"/>
		/// </summary>
		/// <param name="bytes">tableau de <see cref="byte"/></param>
		/// <returns><see cref="System.Drawing.Image"/> résultante</returns>
		public static Image GetImageFromBytes(byte[] bytes) 
		{
			return GetImageFromBytes(bytes, false);
		}

		/// <summary>
		/// Transforme un tableau de <see cref="byte"/>
		/// en base 64 en un objet <see cref="System.Drawing.Image"/>
		/// </summary>
		/// <param name="bytes">tableau de <see cref="byte"/></param>
		/// <param name="makeTransparent">Si vrai, rend la couleur de fond transparent</param>
		/// <returns><see cref="System.Drawing.Image"/> résultante</returns>
		public static Image GetImageFromBytes(byte[] bytes, bool makeTransparent) 
		{
			try 
			{
				MemoryStream ms = new MemoryStream(bytes);
				Bitmap img = (Bitmap)Bitmap.FromStream(ms);
				if (makeTransparent)
					img.MakeTransparent();

				ms.Close();
				return img;
			} 
			catch 
			{
				return null;
			}
		}

		/// <summary>
		/// Transforme un objet <see cref="System.Drawing.Image"/>
		/// en un tableau de <see cref="byte"/>.
		/// </summary>
		/// <param name="img">Objet <see cref="System.Drawing.Image"/></param>
		/// <returns>tableau de <see cref="byte"/></returns>
		public static byte[] GetBytesFromImage(Image img) 
		{
			MemoryStream ms = new MemoryStream();

            EncoderParameters eps = new EncoderParameters(1);
            eps.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
            ImageCodecInfo ici = GetEncoderInfo("image/jpeg");

			//img.Save( ms, img.RawFormat);
            img.Save(ms, ImageFormat.Jpeg);

            return ms.ToArray();
		}

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {

            int j;

            ImageCodecInfo[] encoders;

            encoders = ImageCodecInfo.GetImageEncoders();

            for (j = 0; j < encoders.Length; ++j)
            {

                if (encoders[j].MimeType == mimeType)

                    return encoders[j];

            }

            return null;

        }

		/// <summary>
		/// Transforme un objet <see cref="System.Drawing.Image"/>
		/// en une chaîne en base 64.
		/// </summary>
		/// <param name="img">Objet <see cref="System.Drawing.Image"/></param>
		/// <returns>Chaîne de caractères</returns>
		public static string GetBase64StringFromImage(Image img) 
		{
			return Convert.ToBase64String(GetBytesFromImage(img));
		
		}

	}
}
