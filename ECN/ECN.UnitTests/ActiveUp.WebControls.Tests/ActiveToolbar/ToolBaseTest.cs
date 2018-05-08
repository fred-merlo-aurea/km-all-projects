using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveToolbar
{
	[TestFixture]
	public class ToolBaseTest
	{
		[Test]
		public void EnableSsl_SetAndGetValue_ReturnsDefaultValue()
		{
			// Arrange
			var toolBaseForTesting = new ToolBaseForTesting();

			// Act, Assert
			toolBaseForTesting.EnableSsl.ShouldBeFalse();
		}

		[Test]
		public void EnableSsl_SetAndGetValue_ReturnsSetValue()
		{
			// Arrange
			var toolBaseForTesting = new ToolBaseForTesting();

			// Act
			toolBaseForTesting.EnableSsl = true;

			// Assert
			toolBaseForTesting.EnableSsl.ShouldBeTrue();
		}
	}
}
