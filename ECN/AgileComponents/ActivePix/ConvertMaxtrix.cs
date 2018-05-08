using System.Drawing;

namespace ActiveUp.WebControls
{
	internal enum ControlState
	{
		NormalPixels,
		NormalBitmap,
		BusyPixels,
		BusyBitmap
	}

	internal class ConvertMatrix
	{
		public int TopLeft = 0;
		public int TopMid = 0; 
		public int TopRight = 0;
		public int MidLeft = 0;
		public int Pixel = 1;
		public int MidRight = 0;
		public int BottomLeft = 0;
		public int BottomMid = 0;
		public int BottomRight = 0;
		public int Factor = 1;
		public int Offset = 0;

		public void SetAll(int nVal)
		{
			TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight = BottomLeft = BottomMid = BottomRight = nVal;
		}
		public static bool Convert3x3(Bitmap src, ConvertMatrix matrix)
		{
			if (0 == matrix.Factor) return false;
			Bitmap bitmap = (Bitmap)src.Clone(); 

			int nWidth = bitmap.Width - 2;
			int nHeight = bitmap.Height - 2;

			int	nPixel;
			Color preColor;
			Color color;

			for(int y = 0; y < nHeight; ++y)
			{
				for(int x = 0; x < nWidth; ++x)
				{
					nPixel = ( ( ( (bitmap.GetPixel(x,y).B * matrix.TopLeft) + (bitmap.GetPixel(x + 1,y).B * matrix.TopMid) + (bitmap.GetPixel(x + 2,y).B * matrix.TopRight) +
						(bitmap.GetPixel(x,y + 1).B * matrix.MidLeft) + (bitmap.GetPixel(x + 1,y + 1).B * matrix.Pixel) + (bitmap.GetPixel(x + 1,y + 1).B * matrix.MidRight) +
						(bitmap.GetPixel(x,y + 2).B * matrix.BottomLeft) + (bitmap.GetPixel(x + 1,y + 2).B * matrix.BottomMid) + (bitmap.GetPixel(x + 2,y + 2).B * matrix.BottomRight)) / matrix.Factor) + matrix.Offset); 

					if (nPixel < 0) nPixel = 0;
					if (nPixel > 255) nPixel = 255;

					preColor = src.GetPixel(x + 1, y + 1);
					color = Color.FromArgb(preColor.R, preColor.G, (byte)nPixel);
					src.SetPixel(x + 1, y + 1, color);

					nPixel = ( ( ( (bitmap.GetPixel(x,y).G * matrix.TopLeft) + (bitmap.GetPixel(x + 1,y).G * matrix.TopMid) + (bitmap.GetPixel(x + 2,y).G * matrix.TopRight) +
						(bitmap.GetPixel(x,y + 1).G * matrix.MidLeft) + (bitmap.GetPixel(x + 1,y + 1).G * matrix.Pixel) + (bitmap.GetPixel(x + 1,y + 1).G * matrix.MidRight) +
						(bitmap.GetPixel(x,y + 2).G * matrix.BottomLeft) + (bitmap.GetPixel(x + 1,y + 2).G * matrix.BottomMid) + (bitmap.GetPixel(x + 2,y + 2).G * matrix.BottomRight)) / matrix.Factor) + matrix.Offset); 

					if (nPixel < 0) nPixel = 0;
					if (nPixel > 255) nPixel = 255;

					preColor = src.GetPixel(x + 1, y + 1);
					color = Color.FromArgb(preColor.R, (byte)nPixel, preColor.B);
					src.SetPixel(x + 1, y + 1, color);

					nPixel = ( ( ( (bitmap.GetPixel(x,y).R * matrix.TopLeft) + (bitmap.GetPixel(x + 1,y).R * matrix.TopMid) + (bitmap.GetPixel(x + 2,y).R * matrix.TopRight) +
						(bitmap.GetPixel(x,y + 1).R * matrix.MidLeft) + (bitmap.GetPixel(x + 1,y + 1).R * matrix.Pixel) + (bitmap.GetPixel(x + 1,y + 1).R * matrix.MidRight) +
						(bitmap.GetPixel(x,y + 2).R * matrix.BottomLeft) + (bitmap.GetPixel(x + 1,y + 2).R * matrix.BottomMid) + (bitmap.GetPixel(x + 2,y + 2).R * matrix.BottomRight)) / matrix.Factor) + matrix.Offset); 

					if (nPixel < 0) nPixel = 0;
					if (nPixel > 255) nPixel = 255;

					preColor = src.GetPixel(x + 1, y + 1);
					color = Color.FromArgb((byte)nPixel, preColor.G, preColor.B);
					src.SetPixel(x + 1, y + 1, color);
				}
			}
			return true;
		}
	}

}
