using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Shouldly;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;

namespace ActiveUp.WebControls.Tests.ActiveToolbar
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class ToolTextBoxTest
	{
		[Test]
		public void BorderColor_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new ToolTextBox())
			{
				// Act
				testObject.BorderColor = System.Drawing.Color.Yellow;

				// Assert
				testObject.BorderColor.ShouldBe(System.Drawing.Color.Yellow);
			}
		}

		[Test]
		public void BorderWidth_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new ToolTextBox())
			{
				// Act
				testObject.BorderWidth = 5;

				// Assert
				testObject.BorderWidth.ShouldBe(5);
			}
		}
	}
}
