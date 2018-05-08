using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Shouldly;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;

namespace ActiveUp.WebControls.Tests.ActiveInputCp
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class ExtendedButtonTest
	{
		private const string ExternalScriptDefault = "ActiveInput.js";
		private const string TestValue = "Test1";

		[Test]
		public void ExternalScript_DefaultValue()
		{
			// Arrange
			var testObject = new ExtendedButton();

			// Act, Assert
			AssertNotFX1(string.Empty, testObject.ExternalScript);
			AssertFX1(ExternalScriptDefault, testObject.ExternalScript);
		}

		[Test]
		public void ExternalScript_SetAndGetValue()
		{
			// Arrange
			var testObject = new ExtendedButton();

			// Act
			testObject.ExternalScript = TestValue;

			// Assert
			testObject.ExternalScript.ShouldBe(TestValue);
		}
	}
}
