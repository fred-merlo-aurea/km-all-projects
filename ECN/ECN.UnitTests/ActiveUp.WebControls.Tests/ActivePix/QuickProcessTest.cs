using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActivePix
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class QuickProcessTest
    {
        private Image _image;
        private const int number1 = 100;
        private const int number2 = 200;
        private const int number3 = 300;
        private static Color color = Color.Olive;

        [SetUp]
        public void SetUp()
        {
            _image = CreateImageObject();
        }

        [TestCase(10F)]
        [TestCase(-10F)]
        public void Rotate_Angle10AndNegative_ReturnsImage(float angle)
        {
            // Act
            var result = QuickProcess.Rotate(_image, angle);

            // Assert
            result.ShouldSatisfyAllConditions
                (
                    () => result.Width.ShouldBe(215),
                    () => result.Height.ShouldBe(134)
                );
        }

        [TestCase(90F)]
        [TestCase(270F)]
        public void Rotate_Angle90And270_ShouldReturnRotatedImage(float angle)
        {
            // Act
            var result = QuickProcess.Rotate(_image, angle);

            // Assert
            result.ShouldSatisfyAllConditions
                (
                    () => result.Width.ShouldBe(number1),
                    () => result.Height.ShouldBe(number2)
                );
        }

        [TestCase(0F)]
        public void Rotate_Angle0_ShouldReturnRotatedImage(float angle)
        {
            // Act
            var result = QuickProcess.Rotate(_image, angle);

            // Assert
            result.ShouldSatisfyAllConditions
                (
                    () => result.Width.ShouldBe(number2),
                    () => result.Height.ShouldBe(number1)
                );
        }

        [TestCase(number1, number1, AnchorType.TopLeft)]
        [TestCase(number1, number1, AnchorType.TopCenter)]
        [TestCase(number3, number1, AnchorType.TopCenter)]
        [TestCase(number1, number1, AnchorType.TopRight)]
        [TestCase(number3, number1, AnchorType.TopRight)]
        [TestCase(number1, number1, AnchorType.MiddleLeft)]
        [TestCase(number1, number3, AnchorType.MiddleLeft)]
        [TestCase(number3, number1, AnchorType.MiddleLeft)]
        [TestCase(number3, number3, AnchorType.MiddleLeft)]
        [TestCase(number1, number1, AnchorType.MiddleCenter)]
        [TestCase(number1, number3, AnchorType.MiddleCenter)]
        [TestCase(number3, number1, AnchorType.MiddleCenter)]
        [TestCase(number3, number3, AnchorType.MiddleCenter)]
        [TestCase(number1, number1, AnchorType.MiddleRight)]
        [TestCase(number1, number3, AnchorType.MiddleRight)]
        [TestCase(number3, number1, AnchorType.MiddleRight)]
        [TestCase(number3, number3, AnchorType.MiddleRight)]
        [TestCase(number1, number1, AnchorType.BottomLeft)]
        [TestCase(number1, number3, AnchorType.BottomLeft)]
        [TestCase(number3, number1, AnchorType.BottomLeft)]
        [TestCase(number3, number3, AnchorType.BottomLeft)]
        [TestCase(number1, number1, AnchorType.BottomCenter)]
        [TestCase(number1, number3, AnchorType.BottomCenter)]
        [TestCase(number3, number1, AnchorType.BottomCenter)]
        [TestCase(number3, number3, AnchorType.BottomCenter)]
        [TestCase(number1, number1, AnchorType.BottomRight)]
        [TestCase(number1, number3, AnchorType.BottomRight)]
        [TestCase(number3, number1, AnchorType.BottomRight)]
        [TestCase(number3, number3, AnchorType.BottomRight)]
        public void ResizeCanvas_WithAnchorType_ShouldReturnImage(int width,
            int heigth, AnchorType anchorType)
        {
            // Act
            var result = QuickProcess.ResizeCanvas(_image, width, heigth, 
                anchorType, color);

            // Assert
            result.ShouldSatisfyAllConditions
                (
                    () => result.Size.ShouldNotBeNull(),
                    () => result.Height.ShouldBe(heigth),
                    () => result.Width.ShouldBe(width)
                );
        }

        private static Image CreateImageObject()
        {
            var bitMapImage = new Bitmap(number2, number1);
            var flagGraphics = Graphics.FromImage(bitMapImage);
            int red = 0;
            int white = 11;
            while (white <= number1)
            {
                flagGraphics.FillRectangle(Brushes.Red, 0, red, number2, 10);
                flagGraphics.FillRectangle(Brushes.White, 0, white, number2, 10);
                red += 20;
                white += 20;
            }
            Image image = bitMapImage;
            return image;
        }
    }
}
