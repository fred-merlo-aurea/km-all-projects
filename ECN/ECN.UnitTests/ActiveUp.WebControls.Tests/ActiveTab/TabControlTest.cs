using System.Collections;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveTab
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class TabControlTest
	{
		private const string TestValue = "Test1";

		[Test]
		public void ExternalScript_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			using (var testObject = new TabControl())
			{
				// Act, Assert
				testObject.ExternalScript.ShouldBeEmpty();
			}
		}

		[Test]
		public void ExternalScript_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new TabControl())
			{
				// Act
				testObject.ExternalScript = TestValue;

				// Assert
				testObject.ExternalScript.ShouldBe(TestValue);
			}
		}
	}
}
