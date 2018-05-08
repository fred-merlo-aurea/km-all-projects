using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ECN.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.TestHelpers
{
	[ExcludeFromCodeCoverage]
	public class MasterPageExTestBase<T> where T : MasterPageEx, new()
	{
		private const string TestValue = "Test1";
		private const string lblHeadingName = "lblHeading";
		private const string HideHeadingName = "HideHeading";
		private const string lblHelpContentName = "lblHelpContent";
		private const string lblHelpHeadingName = "lblHelpHeading";

		[Test]
		public void HelpContent_SetValue_UpdateslblHelpContent()
		{
			// Arrange
			var testObject = new T();

			// Act, Assert
			SetAndExpect(testObject, (input) => testObject.HelpContent = TestValue, lblHelpContentName);
		}

		[Test]
		public void HelpTitle_SetValue_UpdateslblHelpHeading()
		{
			// Arrange
			var testObject = new T();

			// Act, Assert
			SetAndExpect(testObject, (input) => testObject.HelpTitle = TestValue, lblHelpHeadingName);
		}

		[Test]
		public void Heading_SetValue_UpdatesLblHeadingAndHideHeading()
		{
			// Arrange
			var testObject = new T();
			var privateObject = new PrivateObject(testObject);

			privateObject.SetField(lblHeadingName, new Label());
			var lblHeading = privateObject.GetField(lblHeadingName) as Label;

			privateObject.SetField(HideHeadingName, new HtmlTableRow());
			var hideHeading = privateObject.GetField(HideHeadingName) as HtmlTableRow;

			// Act
			testObject.Heading = TestValue;

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => lblHeading.Text.ShouldBe(TestValue),
				() => hideHeading.Visible.ShouldBeTrue());
		}

		[Test]
		public void SubMenu_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			var testObject = new T();

			// Act, Assert
			testObject.SubMenu.ShouldBeEmpty();
		}

		[Test]
		public void SubMenu_SetValue_ExpectSetValue()
		{
			// Arrange
			var testObject = new T
			{
				// Act
				SubMenu = TestValue
			};

			// Assert
			testObject.SubMenu.ShouldBe(TestValue);
		}

		private void SetAndExpect(T testObject, Action<string> action, string labelName)
		{
			// Arrange
			var privateObject = new PrivateObject(testObject);
			privateObject.SetField(labelName, new Label());
			var label = privateObject.GetField(labelName) as Label;

			// Act
			action(TestValue);

			// Assert
			label.Text.ShouldBe(TestValue);
		}
	}
}
