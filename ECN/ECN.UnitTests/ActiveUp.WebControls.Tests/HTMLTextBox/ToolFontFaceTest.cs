using System.Collections;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class ToolFontFaceTest
	{
		private readonly ArrayList TestFontArrayList = new ArrayList(new[] { "Font1", "Font2" });

		[Test]
		public void Fonts_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			using (var testObject = new ToolFontFace())
			{
				// Act, Assert
				testObject.ShouldSatisfyAllConditions(
				() => testObject.Fonts.ShouldNotBeNull(),
				() => testObject.Fonts.Count.ShouldBe(0));
			}
		}

		[Test]
		public void Fonts_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new ToolFontFace())
			{
				// Act
				testObject.Fonts = TestFontArrayList;

				// Assert
				testObject.ShouldSatisfyAllConditions(
				() => testObject.Fonts.Count.ShouldBe(2),
				() => testObject.Fonts[0].ShouldBeSameAs(TestFontArrayList[0]),
				() => testObject.Fonts[1].ShouldBeSameAs(TestFontArrayList[1]));
			}
		}
	}
}
