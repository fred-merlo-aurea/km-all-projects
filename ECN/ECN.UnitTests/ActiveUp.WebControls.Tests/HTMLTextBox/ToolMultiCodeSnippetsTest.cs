using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
	[TestFixture]
	public class ToolMultiCodeSnippetsTest
	{
		private const string ToolItemText1 = "item1";
		private const string ToolItemText2 = "item2";
		private const string ToolItemValue1 = "value1";
		private const string ToolItemValue2 = "value2";
		private const string ItemsPropertyName = "_items";
		private const string MethodTrackViewState = "TrackViewState";
		private const string MethodSaveViewState = "SaveViewState";
		private const string MethodLoadViewState = "LoadViewState";

		[Test]
		public void ViewState_LoadSaveTrack_ReturnsTheSetValue()
		{
			// Arrange:
			//Create the control, start tracking viewstate, then set a new Text value.
			var toolMultiCodeSnippets = new ToolMultiCodeSnippets();
			var item1 = new ToolItem(ToolItemText1, ToolItemValue1);
			var item2 = new ToolItem(ToolItemText2, ToolItemValue2);
			var itemCollection = new ToolItemCollection
			{
				item1,
				item2
			};

			var privateObject = new PrivateObject(toolMultiCodeSnippets);
			privateObject.SetFieldOrProperty(ItemsPropertyName, itemCollection);

			privateObject.Invoke(MethodTrackViewState);
			itemCollection.RemoveAt(0);
			privateObject.SetFieldOrProperty(ItemsPropertyName, itemCollection);

			//Save the control's state
			var viewState = privateObject.Invoke(MethodSaveViewState);

			//Create a new control instance and load the state
			//back into it, overriding any existing values
			var toolMultiCode = new ToolMultiCodeSnippets();
			itemCollection = new ToolItemCollection
			{
				item1,
				item2
			};

			var toolMultiCodePrivateObject = new PrivateObject(toolMultiCode);
			toolMultiCodePrivateObject.SetFieldOrProperty(ItemsPropertyName, itemCollection);

			// Act:
			toolMultiCodePrivateObject.Invoke(MethodLoadViewState, viewState);

			// Assert:
			var propertyValue = toolMultiCodePrivateObject.GetFieldOrProperty(ItemsPropertyName) as ToolItemCollection;
			propertyValue.ShouldNotBeNull();
			propertyValue.Count.ShouldBe(1);
		}
	}
}
