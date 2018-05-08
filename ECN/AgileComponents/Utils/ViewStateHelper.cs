using System;
using System.Web.UI;

namespace ActiveUp.WebControls
{
	public static class ViewStateHelper
	{
		public static T GetFromViewState<T>(StateBag viewState, string viewStateKey, T defaultValue)
		{
			if (viewState == null)
			{
				throw new ArgumentNullException(nameof(viewState));
			}
			else
			{
				var local = viewState[viewStateKey];

				if (local != null && local is T)
				{
					return (T)local;
				}
				else
				{
					return defaultValue;
				}
			}
		}

		public static void SetViewState<T>(StateBag viewState, string viewStateKey, T value)
		{
			if (viewState != null)
			{
				viewState[viewStateKey] = value;
			}
		}
	}
}
