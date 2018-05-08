using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls.Common
{
	public class StateManagerImpl : IStateManager
	{
		private StateBag viewState;
		private IStateManager GetViewStateManager(StateBag viewState)
		{
			if (viewState != null)
			{
				return viewState as IStateManager;
			}
			return null;
		}

		/// <summary>
		/// Gets a dictionary of state information that allows you to save and restore the view
		/// state of a server control across multiple requests for the same page.
		/// </summary>
		/// <value></value>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		internal StateBag ViewState
		{
			get
			{
				if (viewState == null)
				{
					viewState = new StateBag(false);
					if (IsTrackingViewState)
					{
						((IStateManager)viewState).TrackViewState();
					}
				}
				return viewState;
			}
		}

		public void SetDirty()
		{
			if (viewState != null)
			{
				var keys = viewState.Keys;
				foreach (string key in keys)
				{
					viewState.SetItemDirty(key, true);
				}
			}
		}

		public bool IsTrackingViewState { get; private set; }

		public void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				GetViewStateManager(ViewState)?.LoadViewState(savedState);
			}
		}

		public object SaveViewState()
		{
			return GetViewStateManager(viewState)?.SaveViewState();
		}

		public void TrackViewState()
		{
			IsTrackingViewState = true;
			GetViewStateManager(viewState)?.TrackViewState();
		}

		public void AddParsedSubObject(object obj, Action<string> assignText)
		{
			var literalControlObject = obj as LiteralControl;
			if(literalControlObject != null)
			{
				assignText(literalControlObject.Text);
			}
		}
	}
}
