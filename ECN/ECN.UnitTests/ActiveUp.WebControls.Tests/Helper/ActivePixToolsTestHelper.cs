using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveUp.WebControls.Tests.Helper
{
    public static class ActivePixToolsTestHelper
    {
        /// <summary>
        /// Creates a 2 color (black and gray) dummy image. Left half of the image is Black colored. Right half of the image is Gray colored.
        /// </summary>
        /// <param name="width">Width of Dummy image</param>
        /// <param name="height">Height of Dummy image</param>
        /// <returns></returns>
        public static Bitmap CreateDummyImage(int width, int height)
        {
            return CreateDummyImage(200, 100, ColorsDirection.Horizontal);
        }

        public static Bitmap CreateDummyImage(int width, int height, ColorsDirection direction, params Color[] colors)
        {
            if (colors == null || colors.Length == 0)
            {
                colors = new Color[]
                {
                    Color.Black,
                    Color.Gray
                };
            }

            var bitmap = new Bitmap(width, height);
            var graphics = Graphics.FromImage(bitmap);

            try
            {
                Color color;
                int colorStartX, colorStartY, colorEndX, colorEndY;

                for (int i = 0; i < colors.Length; i++)
                {
                    color = colors[i];

                    colorStartX = direction == ColorsDirection.Horizontal ? (width / colors.Length) * i : 0;
                    colorEndX = direction == ColorsDirection.Horizontal ? (width / colors.Length) * (i + 1) : width;

                    colorStartY = direction == ColorsDirection.Vertical ? (height / colors.Length) * i : 0;
                    colorEndY = direction == ColorsDirection.Vertical ? (height / colors.Length) * (i + 1) : height;

                    graphics.FillRectangle(new SolidBrush(color), colorStartX, colorStartY, colorEndX, colorEndY);
                }
            }
            finally
            {
                graphics.Dispose();
            }

            return bitmap;
        }

        public static void SaveImage(Image dummyImage, string fullFileName)
        {
            dummyImage.Save(fullFileName);
        }

        public static Bitmap LoadImage(string fullFileName)
        {
            using (var image = Image.FromFile(fullFileName))
            {
                return new Bitmap(image);
            }
        }

        /// <summary>
        /// Creates a 2 color (black and gray) dummy image and saves to a file that is specified via fullFileName. Left half of the image is Black colored. Right half of the image is Gray colored.
        /// </summary>
        /// <param name="width">Width of Dummy image</param>
        /// <param name="height">Height of Dummy image</param>
        /// <param name="fullFileName">File location to save image</param>
        /// <returns></returns>
        public static Bitmap CreateAndSaveDummyImage(int width, int height, string fullFileName)
        {
            var dummyImage = CreateDummyImage(width, height);

            dummyImage.Save(fullFileName);

            return dummyImage;
        }

        public enum ColorsDirection
        {
            Horizontal = 0,
            Vertical = 1
        }
    }
}
