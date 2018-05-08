using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.Utils
{
	[ExcludeFromCodeCoverage]
	public class StateManagerTestBase<T> where T : IStateManager, new()
	{
		protected const string TestValue1 = "Test1";
		private const string TestValue2 = "Test2";
		private const string ViewStateName = "ViewState";
		private const string SetDirtyMethod = "SetDirty";

		[Test]
		public void IsTrackingViewState_DefaultValue_ReturnsFalse()
		{
			// Arrange
			var testObject = new T() as IStateManager;

			// Act, Assert
			testObject.IsTrackingViewState.ShouldBeFalse();
		}

		[Test]
		public void IsTrackingViewState_InvokeTrackViewState_ExpectIsTrackingViewStateSetToTrue()
		{
			// Arrange
			var testObject = new T() as IStateManager;

			// Act
			testObject.TrackViewState();

			// Assert
			testObject.IsTrackingViewState.ShouldBeTrue();
		}

		[Test]
		public void SetDirty_InvokeSetDirty_ExpectDirtyFlagSetForAllItemsInViewState()
		{
			// Arrange 
			var testObject = new T() as IStateManager;
			testObject.TrackViewState();
			var privateObject = new PrivateObject(testObject);
			var viewState = privateObject.GetProperty(ViewStateName) as StateBag;

			// Act
			viewState[TestValue1] = TestValue1;
			viewState[TestValue2] = TestValue2;
			privateObject.Invoke(SetDirtyMethod);

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => viewState.IsItemDirty(TestValue1).ShouldBeTrue(),
				() => viewState.IsItemDirty(TestValue2).ShouldBeTrue()
				);
		}

		[Test]
		public void SaveViewState_SaveViewState_ExpectSetValue()
		{
			// Arrange 
			var testObject = new T() as IStateManager;
			testObject.TrackViewState();
			var privateObject = new PrivateObject(testObject);
			var viewState = privateObject.GetProperty(ViewStateName) as StateBag;
			viewState[TestValue1] = TestValue1;

			// Act
			var returnValue = testObject.SaveViewState();

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => returnValue.ShouldNotBeNull(),
				() => (returnValue as System.Collections.ArrayList)[1].ShouldBeSameAs(TestValue1));
		}

		[Test]
		public void SaveViewState_ViewStateNull_ExpectReturnsNull()
		{
			// Arrange 
			var testObject = new T() as IStateManager;

			// Act
			var returnValue = testObject.SaveViewState();

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => returnValue.ShouldBeNull());
		}

		[Test]
		public void LoadViewState_LoadViewState_ExpectSetValue()
		{
			// Arrange 
			var testObject = new T() as IStateManager;
			testObject.TrackViewState();
			var privateObject = new PrivateObject(testObject);
			var viewState = privateObject.GetProperty(ViewStateName) as StateBag;
			viewState[TestValue1] = TestValue1;

			var savedState = testObject.SaveViewState();
			var testObject1 = new T() as IStateManager;

			// Act
			testObject1.LoadViewState(savedState);

			// Assert
			viewState[TestValue1].ShouldBeSameAs(TestValue1);
		}
	}
}
