using System;
using System.Web.UI.WebControls;
using KM.Common;

namespace ecn.controls
{
    public static class TabBarHelpers
    {
        private const string HandCursorStyle = "cursor:hand";
        private const string OnMouseOverEventName = "onmouseover";
        private const string OnMouseOutEventName = "onmouseout";
        private const string OnClickEventName = "onclick";
        private const string StyleTagName = "style";
        private const string PreventDefaultScript = "javascript:return false;";
        private const string DefaultCursorStyle = "cursor:default";
        private const string SetImageSourceScriptTemplate = "this.src='{0}';";

        public static void EnableTabBar(
            int numberOfWizardSteps, 
            int currentWizardStepIndex, 
            int completedWizardStepIndex, 
            Func<int, ImageButton> getTab,
            Func<int, string> getCurrentWizardStepImageUrl,
            Func<int, string> getUncompletedWizardStepImageUrl,
            Func<int, string> getCompletedWizardStepImageUrl,
            Func<int, string> getMouseOverImageUrl,
            Func<int, string> getMouseOutImageUrl)
        {
            Guard.For(() => numberOfWizardSteps <= 0, () => new ArgumentOutOfRangeException(nameof(numberOfWizardSteps)));
            Guard.For(() => currentWizardStepIndex < 0, () => new ArgumentOutOfRangeException(nameof(currentWizardStepIndex)));
            Guard.For(() => completedWizardStepIndex < 0, () => new ArgumentOutOfRangeException(nameof(completedWizardStepIndex)));

            for (var index = 1; index <= numberOfWizardSteps; index++)
            {
                var tab = getTab?.Invoke(index);
                if (tab == null)
                {
                    continue;
                }

                tab.Attributes.Add(StyleTagName, DefaultCursorStyle);
                tab.Attributes.Add(OnMouseOverEventName, string.Empty);
                tab.Attributes.Add(OnMouseOutEventName, string.Empty);
                tab.Attributes.Add(OnClickEventName, string.Empty);
                tab.Enabled = false;

                if (index == currentWizardStepIndex)
                {
                    tab.ImageUrl = getCurrentWizardStepImageUrl?.Invoke(index);
                }
                else if (index <= completedWizardStepIndex)
                {
                    var mouseOverImageUrl = getMouseOverImageUrl?.Invoke(index);
                    var mouseOutImageUrl = getMouseOutImageUrl?.Invoke(index);

                    tab.ImageUrl = getCompletedWizardStepImageUrl?.Invoke(index);
                    tab.Attributes.Add(OnMouseOverEventName, string.Format(SetImageSourceScriptTemplate, mouseOverImageUrl));
                    tab.Attributes.Add(OnMouseOutEventName, string.Format(SetImageSourceScriptTemplate, mouseOutImageUrl));
                    tab.Attributes.Add(StyleTagName, HandCursorStyle);
                    tab.Enabled = true;
                }
                else
                {
                    tab.ImageUrl = getUncompletedWizardStepImageUrl?.Invoke(index);
                    tab.Attributes.Add(OnClickEventName, PreventDefaultScript);
                }
            }
        }
    }
}
