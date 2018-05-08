using System.Web.UI.WebControls;

namespace ecn.communicator.main.Salesforce.Extensions
{
    public static class CheckBoxExtensions
    {
        private const string ColorAttributeName = "Color";
        private const string GreyDark = "GreyDark";
        private const string GreyLight = "GreyLight";

        public static bool IsGreyDark(this CheckBox checkbox)
        {
            return checkbox.GetColor() == GreyDark;
        }

        public static bool IsGreyLight(this CheckBox checkbox)
        {
            return checkbox.GetColor() == GreyLight;
        }

        private static string GetColor(this CheckBox checkBox)
        {
            return checkBox.Attributes[ColorAttributeName];
        }
    }
}