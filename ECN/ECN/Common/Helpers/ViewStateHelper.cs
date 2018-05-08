using System;
using System.Web.UI;

namespace ECN.Common.Helpers
{
    /// <summary>
    /// Helper class for storing ViewState data
    /// </summary>
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

                return local != null && local is T ? (T)local : defaultValue;
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
