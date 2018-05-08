using System;
using System.Web.UI.WebControls;

namespace ECN_Framework
{
    public class ToggleEnvelopePanelHelper
    {
        private const string DisplayNone = "display:none";
        private const string DisplayInline = "display:inline";
        private const string StyleAttribute = "style";

        public static void ToggleEnvelopePanel(bool IsPredefinedEnvelopeInfo, TextBox[] textBoxes, DropDownList[] dropDownLists)
        {
            if (IsPredefinedEnvelopeInfo)
            {
                SetTextBoxesStyleAttribute(textBoxes, DisplayNone);
                SetDropDownListsVisibility(dropDownLists, true);
            }
            else
            {
                SetTextBoxesStyleAttribute(textBoxes, DisplayInline);
                SetDropDownListsVisibility(dropDownLists, false);
            }
        }

        public static void SetControlsToValidate(RequiredFieldValidator[] requiredFieldValidators, string[] controlNames)
        {
            if (requiredFieldValidators == null)
            {
                throw new ArgumentNullException(nameof(requiredFieldValidators));
            }

            for (var i = 0; i < requiredFieldValidators.Length; i++)
            {
                if (i >= 0 && i < controlNames.Length)
                {
                    requiredFieldValidators[i].ControlToValidate = controlNames[i];
                }
            }
        }

        private static void SetDropDownListsVisibility(DropDownList[] dropDownLists, bool isVisible)
        {
            foreach (var dropDown in dropDownLists)
            {
                dropDown.Visible = isVisible;
            }
        }

        private static void SetTextBoxesStyleAttribute(TextBox[] textBoxes, string displayStyle)
        {
            foreach (var textBox in textBoxes)
            {
                textBox.Attributes.Add(StyleAttribute, displayStyle);
            }
        }
    }
}
