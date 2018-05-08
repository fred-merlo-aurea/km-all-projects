using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Shouldly;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;

namespace ActiveUp.WebControls.Tests.ActiveInput
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public partial class TreeViewTest
	{
		private const string TestValue = "Test1";

		[Test]
		public void UseSameClickEvent_DefaultValue_ReturnsTrue()
		{
			// Arrange
			using (var testObject = new TreeView())
			{
				// Act, Assert - Verify initial value of UseSameClickEvent is true
				testObject.UseSameClickEvent.ShouldBeTrue();
			}
		}

		[Test]
		public void UseSameClickEvent_SetAndGetValue_ReturnsSetValueTrue()
		{
			// Arrange
			using (var testObject = new TreeView())
			{
				// Act
				testObject.UseSameClickEvent = true;

				// Assert
				testObject.UseSameClickEvent.ShouldBeTrue();
			}
		}

		[Test]
		public void UseSameClickEvent_SetAndGetValue_ReturnsSetValueFalse()
		{
			// Arrange
			using (var testObject = new TreeView())
			{
				// Act
				testObject.UseSameClickEvent = false;

				// Assert
				testObject.UseSameClickEvent.ShouldBeFalse();
			}
		}

		[Test]
		public void ImagesDirectory_DefaultValue_ReturnsEmptyStringOrImagesDirectory()
		{
			// Arrange
			using (var testObject = new TreeView())
			{
				// Act, Assert
				AssertNotFX1(string.Empty, testObject.ImagesDirectory);
				AssertFX1(Define.IMAGES_DIRECTORY, testObject.ImagesDirectory);
			}
		}

		[Test]
		public void ImagesDirectory_SetAndGetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new TreeView())
			{
				// Act
				testObject.ImagesDirectory = TestValue;

				// Assert
				testObject.ImagesDirectory.ShouldBe(TestValue);
			}
		}

		[Test]
		public void DataTarget_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new TreeView())
			{
				// Act
				testObject.DataTarget = TestValue;

				// Assert
				testObject.DataTarget.ShouldBe(TestValue);
			}
		}
	}
}