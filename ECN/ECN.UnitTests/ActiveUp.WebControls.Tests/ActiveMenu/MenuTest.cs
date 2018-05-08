using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;

namespace ActiveUp.WebControls.Tests.ActiveMenu
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class MenuTest
	{
		private const string TestValue = "Test1";
		private const string MenuItemId1 = "Id1";
		private const string MenuItemId2 = "Id2";
		private const string MenuItemProperyName = "_menuItems";
		private const string MethodTrackViewState = "TrackViewState";
		private const string MethodSaveViewState = "SaveViewState";
		private const string MethodLoadViewState = "LoadViewState";

		[Test]
		public void ImagesDirectory_DefaultValue()
		{
			// Arrange
			var testObject = new Menu();

			// Act, Assert
			AssertNotFX1(string.Empty, testObject.ImagesDirectory);
			AssertFX1(Define.IMAGES_DIRECTORY, testObject.ImagesDirectory);
		}

		[Test]
		public void ImagesDirectory_SetAndGetValue()
		{
			// Arrange
			var testObject = new Menu();

			// Act
			testObject.ImagesDirectory = TestValue;

			// Assert
			testObject.ImagesDirectory.ShouldBe(TestValue);
		}

		[Test]
		public void IconsDirectory_DefaultValue_ReturnsEmptyStringOrImagesDirectory()
		{
			// Arrange
			using (var testObject = new Menu())
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
			using (var testObject = new Menu())
			{
				// Act
				testObject.IconsDirectory = TestValue;

				// Assert
				testObject.IconsDirectory.ShouldBe(TestValue);
			}
		}

		[Test]
		public void ScriptDirectory_DefaultValue_ReturnsEmptyStringOrScriptDirectory()
		{
			// Arrange
			using (var testObject = new Menu())
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
			using (var testObject = new Menu())
			{
				// Act
				testObject.ScriptDirectory = TestValue;

				// Assert
				testObject.ScriptDirectory.ShouldBe(TestValue);
			}
		}

		[Test]
		public void ViewState_LoadSaveTrack_ReturnsTheSetValue()
		{
			// Arrange:
			//Create the control, start tracking viewstate, then set a new Text value.
			var menu = new Menu();
			var item1 = new MenuItem(MenuItemId1);
			var item2 = new MenuItem(MenuItemId2);
			var controlCollection = new ControlCollection(menu);
			var itemCollection = new MenuItemCollection(controlCollection)
			{
				item1,
				item2
			};

			var privateObject = new PrivateObject(menu);
			privateObject.SetFieldOrProperty(MenuItemProperyName, itemCollection);

			privateObject.Invoke(MethodTrackViewState);
			itemCollection.RemoveAt(0);
			privateObject.SetFieldOrProperty(MenuItemProperyName, itemCollection);

			//Save the control's state
			var viewState = privateObject.Invoke(MethodSaveViewState);

			//Create a new control instance and load the state
			//back into it, overriding any existing values
			var menuNew = new Menu();
			var controlCollectionNew = new ControlCollection(menuNew);
			itemCollection = new MenuItemCollection(controlCollectionNew)
			{
				item1,
				item2
			};

			var menuNewPrivateObject = new PrivateObject(menuNew);
			menuNewPrivateObject.SetFieldOrProperty(MenuItemProperyName, itemCollection);

			// Act:
			menuNewPrivateObject.Invoke(MethodLoadViewState, viewState);

			// Assert:
			var propertyValue = menuNewPrivateObject.GetFieldOrProperty(MenuItemProperyName) as MenuItemCollection;
			propertyValue.ShouldNotBeNull();
			propertyValue.Count.ShouldBe(1);
		}

		[Test]
		public void DataParent_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new Menu())
			{
				// Act
				testObject.DataParent = TestValue;

				// Assert
				testObject.DataParent.ShouldBe(TestValue);
			}
		}

		[Test]
		public void DataTarget_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new Menu())
			{
				// Act
				testObject.DataTarget = TestValue;
				
				// Assert
				testObject.DataTarget.ShouldBe(TestValue);
			}
		}

		[Test]
		public void BackImage_DefaultValue_ReturnsEmptyString()
		{
			// Arrange
			using (var testObject = new Menu())
			{
				// Act, Assert
				testObject.BackImage.ShouldBeEmpty();
			}
		}

		[Test]
		public void BackImage_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new Menu())
			{
				// Act
				testObject.BackImage = TestValue;

				// Assert
				testObject.BackImage.ShouldBe(TestValue);
			}
		}
	}
}
