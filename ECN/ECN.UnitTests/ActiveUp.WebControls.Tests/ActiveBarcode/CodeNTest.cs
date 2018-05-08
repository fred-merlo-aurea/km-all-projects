using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveBarcode
{
	[TestFixture]
	public class CodeNTest
	{
		private const string DummyStringValue = "1234";

		[Test]
		public void Code93EncoderText_SetAndGetValue_ReturnsDefaultValue()
		{
			// Arrange, Act
			var code93Encoder = new Code93Encoder();

			// Assert
			code93Encoder.Text.ShouldBeNull();
		}

		[Test]
		public void Code93EncoderText_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange, Act
			var code93Encoder = new Code93Encoder
			{
				Text = DummyStringValue
			};

			// Assert
			code93Encoder.Text.ShouldBe(DummyStringValue);
		}

		[Test]
		public void Code39EncoderText_SetAndGetValue_ReturnsDefaultValue()
		{
			// Arrange, Act
			var code39Encoder = new Code39Encoder();
			
			// Assert
			code39Encoder.Text.ShouldBeNull();
		}

		[Test]
		public void Code39EncoderText_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange, Act
			var code39Encoder = new Code39Encoder
			{
				Text = DummyStringValue
			};

			// Assert
			code39Encoder.Text.ShouldBe(DummyStringValue);
		}
	}
}