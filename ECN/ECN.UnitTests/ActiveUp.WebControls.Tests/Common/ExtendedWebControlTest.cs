using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Shouldly;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;

namespace ActiveUp.WebControls.Tests.Common
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class ExtendedWebControlTest
	{
		private const string ExternalScriptDefault = "base.js";
		private const string ImageTabDesign = "tab_design.gif";
		private const string TestValue = "Test1";

		[Test]
		public void IconsDirectory_DefaultValue_ReturnsEmptyStringOrImagesDirectory()
		{
			// Arrange
			using (var testObject = new ExtendedWebControl())
			{
				// Act, Assert
				AssertNotFX1(string.Empty, testObject.IconsDirectory);
				AssertFX1(Define.IMAGES_DIRECTORY, testObject.IconsDirectory);
			}
		}

		[Test]
		public void IconsDirectory_SetAndGetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new ExtendedWebControl())
			{
				// Act
				testObject.IconsDirectory = TestValue;

				// Assert
				testObject.IconsDirectory.ShouldBe(TestValue);
			}
		}

		[Test]
		public void ScriptDirectory_DefaultValue_ReturnsEmptyStringOrImageTabDesign()
		{
			// Arrange
			using (var testObject = new ExtendedWebControl())
			{
				// Act, Assert
				AssertNotFX1(string.Empty, testObject.ScriptDirectory);
				AssertFX1(ImageTabDesign, testObject.ScriptDirectory);
			}
		}

		[Test]
		public void ScriptDirectory_SetAndGetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new ExtendedWebControl())
			{
				// Act
				testObject.ScriptDirectory = TestValue;

				// Assert
				testObject.ScriptDirectory.ShouldBe(TestValue);
			}
		}

		[Test]
		public void ExternalScript_DefaultValue()
		{
			// Arrange
			var testObject = new ExtendedWebControl();

			// Act, Assert
			AssertNotFX1(string.Empty, testObject.ExternalScript);
			AssertFX1(ExternalScriptDefault, testObject.ExternalScript);
		}

		[Test]
		public void ExternalScript_SetAndGetValue()
		{
			// Arrange
			var testObject = new ExtendedWebControl();

			// Act
			testObject.ExternalScript = TestValue;

			// Assert
			testObject.ExternalScript.ShouldBe(TestValue);
		}
	}
}
