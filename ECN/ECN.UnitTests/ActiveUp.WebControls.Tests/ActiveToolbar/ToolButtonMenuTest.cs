using System.Drawing;
using System.Web.UI.WebControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveToolbar
{
	[TestFixture]
	public class ToolButtonMenuTest
	{
		private const string toolButtonMenuItemText1 = "item1";
		private const string toolButtonMenuItemText2 = "item2";
		private const string ItemsPropertyName = "_items";
		private const string DummyBackImageClickedText = "DummyBackImageClickedText";
		private const string MethodTrackViewState = "TrackViewState";
		private const string MethodSaveViewState = "SaveViewState";
		private const string MethodLoadViewState = "LoadViewState";

		static object[] PropertyNameWithDefaultValue =
		{
			new object[] {"BorderWidthRollOver", Unit.Empty},
			new object[] {"BackColorRollOver", Color.Empty},
			new object[] {"BackColorClicked", Color.Empty},
			new object[] {"BackImageClicked", string.Empty}
		};

		static object[] PropertyNameWithExpectedValue =
		{
			new object[] {"BorderWidthRollOver", Unit.Pixel(1)},
			new object[] {"BackColorRollOver", Color.Black},
			new object[] {"BackColorClicked", Color.Black},
			new object[] {"BackImageClicked", DummyBackImageClickedText}
		};

		[Test]
		public void WindowBorderStyle_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolButtonMenu = new ToolButtonMenu();

			// Act:
			toolButtonMenu.WindowBorderStyle = BorderStyle.Solid;

			// Assert:
			toolButtonMenu.WindowBorderStyle.ShouldBe(BorderStyle.Solid);
		}

		[Test]
		public void WindowBorderWidth_SetAndGetValue_ReturnsTheSetValue()
		{
			// Arrange:
			var toolButtonMenu = new ToolButtonMenu();

			// Act:
			toolButtonMenu.WindowBorderWidth = Unit.Pixel(1);

			// Assert:
			toolButtonMenu.WindowBorderWidth.ShouldBe(Unit.Pixel(1));
		}

		[Test]
		[TestCaseSource("PropertyNameWithDefaultValue")]
		public void Property_SetAndGetValue_ReturnsDefaultValue(string propertyName, object expected)
		{
			// Arrange:
			var toolButtonMenu = new ToolButtonMenu();
			var privateObject = new PrivateObject(toolButtonMenu);

			// Act: // Assert:
			privateObject.GetFieldOrProperty(propertyName).ShouldBe(expected);
		}

		[Test]
		[TestCaseSource("PropertyNameWithExpectedValue")]
		public void Property_SetAndGetValue_ReturnsTheSetValue(string propertyName, object expected)
		{
			// Arrange:
			var toolButtonMenu = new ToolButtonMenu();
			var privateObject = new PrivateObject(toolButtonMenu);

			// Act:
			privateObject.SetFieldOrProperty(propertyName, expected);

			// Assert:
			privateObject.GetFieldOrProperty(propertyName).ShouldBe(expected);
		}

		[Test]
		public void ViewState_LoadSaveTrack_ReturnsTheSetValue()
		{
			// Arrange:
			//Create the control, start tracking viewstate, then set a new Text value.
			var toolButtonMenu = new ToolButtonMenu();
			var item1 = new ToolButtonMenuItem(toolButtonMenuItemText1);
			var item2 = new ToolButtonMenuItem(toolButtonMenuItemText2);
			var itemCollection = new ToolButtonMenuItemCollection
			{
				item1,
				item2
			};

			var privateObject = new PrivateObject(toolButtonMenu);
			privateObject.SetFieldOrProperty(ItemsPropertyName, itemCollection);

			privateObject.Invoke(MethodTrackViewState);
			itemCollection.Remove(item1);
			privateObject.SetFieldOrProperty(ItemsPropertyName, itemCollection);

			//Save the control's state
			var viewState = privateObject.Invoke(MethodSaveViewState);

			//Create a new control instance and load the state
			//back into it, overriding any existing values
			var toolButton = new ToolButtonMenu();
			itemCollection = new ToolButtonMenuItemCollection
			{
				item1,
				item2
			};

			var toolButtonPrivateObject = new PrivateObject(toolButton);
			toolButtonPrivateObject.SetFieldOrProperty(ItemsPropertyName, itemCollection);

			// Act:
			toolButtonPrivateObject.Invoke(MethodLoadViewState, viewState);

			// Assert:
			var propertyValue = toolButtonPrivateObject.GetFieldOrProperty(ItemsPropertyName) as ToolButtonMenuItemCollection;
			propertyValue.ShouldNotBeNull();
			propertyValue.Count.ShouldBe(1);
		}
	}
}