using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Shouldly;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;

namespace ActiveUp.WebControls.Tests.ActiveTimer
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class WebTimerTest
	{
		private const string TestValue = "Test1";

		[Test]
		public void ScriptDirectory_DefaultValue_ReturnsEmptyStringOrDefinesScriptDirectory()
		{
			// Arrange
			using (var testObject = new Editor())
			{
				// Act, Assert
				AssertNotFX1(string.Empty, testObject.ScriptDirectory);
				AssertFX1(Define.SCRIPT_DIRECTORY, testObject.ScriptDirectory);
			}
		}

		[Test]
		public void ScriptDirectory_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new Editor())
			{
				// Act
				testObject.ScriptDirectory = TestValue;

				// Assert
				testObject.ScriptDirectory.ShouldBe(TestValue);
			}
		}
	}
}
