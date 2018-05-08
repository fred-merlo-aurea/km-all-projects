using System.Collections;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class ToolFontSizeTest
	{
		private readonly ArrayList TestFontArrayList = new ArrayList(new[] { "Size1", "Size2" });

		[Test]
		public void Sizes_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			using (var testObject = new ToolFontSize())
			{
				// Act, Assert
				testObject.ShouldSatisfyAllConditions(
				() => testObject.Sizes.ShouldNotBeNull(),
				() => testObject.Sizes.Count.ShouldBe(0));
			}
		}

		[Test]
		public void Sizes_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new ToolFontSize())
			{
				// Act
				testObject.Sizes = TestFontArrayList;

				// Assert
				testObject.ShouldSatisfyAllConditions(
				() => testObject.Sizes.Count.ShouldBe(2),
				() => testObject.Sizes[0].ShouldBeSameAs(TestFontArrayList[0]),
				() => testObject.Sizes[1].ShouldBeSameAs(TestFontArrayList[1]));
			}
		}
	}
}
