using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveBarcode
{
    [TestFixture]
    public class EANTest
    {
        private const string DummyEan13StringValue = "1234567890128";
        private const string DummyEan8StringValue = "12345670";

        [Test]
        public void EAN13EncoderText_SetAndGetValue_ReturnsDefaultValue()
        {
            // Arrange, Act
            var ean13Encoder = new EAN13Encoder();

            // Assert
            ean13Encoder.Text.ShouldBeNull();
        }

        [Test]
        public void EAN13EncoderText_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange, Act
            var ean13Encoder = new EAN13Encoder
            {
                Text = DummyEan13StringValue
            };

            // Assert
            ean13Encoder.Text.ShouldBe(DummyEan13StringValue);
        }

        [Test]
        public void EAN8EncoderText_SetAndGetValue_ReturnsDefaultValue()
        {
            // Arrange, Act
            var ean8Encoder = new EAN8Encoder();
            
            // Assert
            ean8Encoder.Text.ShouldBeNull();
        }

        [Test]
        public void EAN8EncoderText_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange, Act
            var ean8Encoder = new EAN8Encoder
            {
                Text = DummyEan8StringValue
            };

            // Assert
            ean8Encoder.Text.ShouldBe(DummyEan8StringValue);
        }
    }
}