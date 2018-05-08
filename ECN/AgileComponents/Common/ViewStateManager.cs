using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActiveUp.WebControls.Common
{
	public class ViewStateManager : WebControl
	{
		/// Loads the view state.
		/// </summary>
		/// <param name="savedState">The saved view state.</param>
		protected void LoadViewState(object savedState, Action<object> stateManagerLoadViewState)
		{
			object baseState = null;
			var myState = savedState as object[];

			if (myState != null)
			{
				if (myState.Length < 1)
				{
					throw new ArgumentException("Invalid view state: Should not be empty.");
				}

				baseState = myState[0];
			}

			base.LoadViewState(baseState);

			if ((myState != null) && (myState[1] != null))
			{
				stateManagerLoadViewState(myState[1]);
			}
		}

		/// <summary>
		/// Saves the view state.
		/// </summary>
		/// <returns></returns>
		protected object SaveViewState<T>(T itemCollection)
		{
			var baseState = base.SaveViewState();
			object itemsState = null;
			var count = 0;

			var countProperty = itemCollection.GetType().GetProperty("Count");
			if (countProperty != null)
			{
				int.TryParse(countProperty.GetValue(itemCollection, null).ToString(), out count);
			}

			if ((itemCollection != null) && (count > 0))
			{
				itemsState = ((IStateManager)itemCollection).SaveViewState();
			}

			if (Should.AnyBeNonNull(baseState, itemsState))
			{
				var savedState = new object[]
				{
					baseState,
					itemsState
				};

				return savedState;
			}
			return null;
		}

		/// <summary>
		/// Tracks the view state.
		/// </summary>
		protected void TrackViewState<T>(T itemCollection)
		{
			base.TrackViewState();
			if (itemCollection != null)
			{
				((IStateManager)itemCollection).TrackViewState();
			}
		}
	}
}