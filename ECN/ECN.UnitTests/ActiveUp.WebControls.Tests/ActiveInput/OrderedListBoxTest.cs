using System.Diagnostics.CodeAnalysis;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveInput
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class OrderedListBoxTest
	{
		private const string TestValue = "Test1";
		[Test]
		public void ButtonDownDisabled_DefaultValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act, Assert - Verify initial value of ButtonDownDisabled is false
			orderedListBox.ButtonDownDisabled.ShouldBeFalse();
		}

		[Test]
		public void ButtonDownDisabled_SetAndGetValueTrue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonDownDisabled = true;

			// Assert
			orderedListBox.ButtonDownDisabled.ShouldBeTrue();
		}

		[Test]
		public void ButtonDownDisabled_SetAndGetValueFalse()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonDownDisabled = false;

			// Assert
			orderedListBox.ButtonDownDisabled.ShouldBeFalse();
		}

		[Test]
		public void ButtonLeftDisabled_DefaultValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act, Assert - Verify initial value of ButtonLeftDisabled is false
			orderedListBox.ButtonLeftDisabled.ShouldBeFalse();
		}

		[Test]
		public void ButtonLeftDisabled_SetAndGetValueTrue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonLeftDisabled = true;

			// Assert
			orderedListBox.ButtonLeftDisabled.ShouldBeTrue();
		}

		[Test]
		public void ButtonLeftDisabled_SetAndGetValueFalse()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonLeftDisabled = false;

			// Assert
			orderedListBox.ButtonLeftDisabled.ShouldBeFalse();
		}

		[Test]
		public void ButtonRightDisabled_DefaultValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act, Assert - Verify initial value of ButtonRightDisabled is false
			orderedListBox.ButtonRightDisabled.ShouldBeFalse();
		}

		[Test]
		public void ButtonRightDisabled_SetAndGetValueTrue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonRightDisabled = true;

			// Assert
			orderedListBox.ButtonRightDisabled.ShouldBeTrue();
		}

		[Test]
		public void ButtonRightDisabled_SetAndGetValueFalse()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonRightDisabled = false;

			// Assert
			orderedListBox.ButtonRightDisabled.ShouldBeFalse();
		}

		[Test]
		public void ButtonUpDisabled_DefaultValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act, Assert - Verify initial value of ButtonUpDisabled is false
			orderedListBox.ButtonUpDisabled.ShouldBeFalse();
		}

		[Test]
		public void ButtonUpDisabled_SetAndGetValueTrue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonUpDisabled = true;

			// Assert
			orderedListBox.ButtonUpDisabled.ShouldBeTrue();
		}

		[Test]
		public void ButtonUpDisabled_SetAndGetValueFalse()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonUpDisabled = false;

			// Assert
			orderedListBox.ButtonUpDisabled.ShouldBeFalse();
		}

		[Test]
		public void ButtonUpText_DefaultValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act, Assert - Verify initial value of ButtonUpText is "Up"
			orderedListBox.ButtonUpText.ShouldBe("Up");
		}

		[Test]
		public void ButtonUpText_SetAndGetValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonUpText = "Test1";

			// Assert
			orderedListBox.ButtonUpText.ShouldBe("Test1");
		}

		[Test]
		public void ButtonDownText_DefaultValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act, Assert - Verify initial value of ButtonDownText is "Down"
			orderedListBox.ButtonDownText.ShouldBe("Down");
		}

		[Test]
		public void ButtonDownText_SetAndGetValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonDownText = "Test1";

			// Assert
			orderedListBox.ButtonDownText.ShouldBe("Test1");
		}

		[Test]
		public void ButtonLeftText_DefaultValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act, Assert - Verify initial value of ButtonLeftText is "Left"
			orderedListBox.ButtonLeftText.ShouldBe("Left");
		}

		[Test]
		public void ButtonLeftText_SetAndGetValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonLeftText = "Test1";

			// Assert
			orderedListBox.ButtonLeftText.ShouldBe("Test1");
		}

		[Test]
		public void ButtonRightText_DefaultValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act, Assert - Verify initial value of ButtonRightText is "Right"
			orderedListBox.ButtonRightText.ShouldBe("Right");
		}

		[Test]
		public void ButtonRightText_SetAndGetValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonRightText = "Test1";

			// Assert
			orderedListBox.ButtonRightText.ShouldBe("Test1");
		}

		[Test]
		public void IconsDirectory_DefaultValue_ReturnsEmptyStringOrImagesDirectory()
		{
			// Arrange
			using (var testObject = new OrderedListBox())
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
			using (var testObject = new OrderedListBox())
			{
				// Act
				testObject.IconsDirectory = TestValue;

				// Assert
				testObject.IconsDirectory.ShouldBe(TestValue);
			}
		}

		[Test]
		public void ButtonUpImage_DefaultValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();
			
			// Act, Assert
			AssertNotFX1(string.Empty, orderedListBox.ButtonUpImage);
			AssertFX1("arrow_up_blue.gif", orderedListBox.ButtonUpImage);
		}

		[Test]
		public void ButtonUpImage_SetAndGetValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonUpImage = "Test1";

			// Assert
			orderedListBox.ButtonUpImage.ShouldBe("Test1");
		}

		[Test]
		public void ButtonDownImage_DefaultValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act, Assert
			AssertNotFX1(string.Empty, orderedListBox.ButtonDownImage);
			AssertFX1("arrow_down_blue.gif", orderedListBox.ButtonDownImage);
		}

		[Test]
		public void ButtonDownImage_SetAndGetValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonDownImage = "Test1";

			// Assert
			orderedListBox.ButtonDownImage.ShouldBe("Test1");
		}

		[Test]
		public void ButtonLeftImage_DefaultValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act, Assert
			AssertNotFX1(string.Empty, orderedListBox.ButtonLeftImage);
			AssertFX1("arrow_left_blue.gif", orderedListBox.ButtonLeftImage);
		}

		[Test]
		public void ButtonLeftImage_SetAndGetValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonLeftImage = "Test1";

			// Assert
			orderedListBox.ButtonLeftImage.ShouldBe("Test1");
		}

		[Test]
		public void ButtonRightImage_DefaultValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act, Assert
			AssertNotFX1(string.Empty, orderedListBox.ButtonRightImage);
			AssertFX1("arrow_right_blue.gif", orderedListBox.ButtonRightImage);
		}

		[Test]
		public void ButtonRightImage_SetAndGetValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ButtonRightImage = "Test1";

			// Assert
			orderedListBox.ButtonRightImage.ShouldBe("Test1");
		}

		[Test]
		public void RenderType_DefaultValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act, Assert
			orderedListBox.RenderType.ShouldBe(RenderType.NotSet);
		}

		[Test]
		public void RenderType_SetAndGetValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.RenderType = RenderType.UpLevel;

			// Assert
			orderedListBox.RenderType.ShouldBe(RenderType.UpLevel);
		}

		[Test]
		public void ScriptDirectory_DefaultValue_ReturnsEmptyStringOrScriptDirectory()
		{
			// Arrange
			using (var testObject = new OrderedListBox())
			{
				// Act, Assert
				AssertNotFX1(string.Empty, testObject.ScriptDirectory);
				AssertFX1(Define.SCRIPT_DIRECTORY, testObject.ScriptDirectory);
			}
		}

		[Test]
		public void ScriptDirectory_SetAndGetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new OrderedListBox())
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
			var orderedListBox = new OrderedListBox();

			// Act, Assert
			orderedListBox.ExternalScript.ShouldBeEmpty();
		}

		[Test]
		public void ExternalScript_SetAndGetValue()
		{
			// Arrange
			var orderedListBox = new OrderedListBox();

			// Act
			orderedListBox.ExternalScript = "Test1";

			// Assert
			orderedListBox.ExternalScript.ShouldBe("Test1");
		}

		[Test]
		public void DataValueField_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			using (var testObject = new OrderedListBox())
			{
				// Act, Assert
				testObject.DataValueField.ShouldBeEmpty();
			}
		}

		[Test]
		public void DataValueField_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new OrderedListBox())
			{
				// Act
				testObject.DataValueField = TestValue;
				
				// Assert
				testObject.DataValueField.ShouldBe(TestValue);
			}
		}

		[Test]
		public void DataSelectedField_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			using (var testObject = new OrderedListBox())
			{
				// Act, Assert
				testObject.DataSelectedField.ShouldBeEmpty();
			}
		}

		[Test]
		public void DataSelectedField_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new OrderedListBox())
			{
				// Act
				testObject.DataSelectedField = TestValue;

				// Assert
				testObject.DataSelectedField.ShouldBe(TestValue);
			}
		}
	}
}
