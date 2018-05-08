using System.Collections.Generic;
using System.Web.UI.WebControls;
using KM.Common;

namespace KMPS.MD.Helpers
{
    public class ControlsValidators
    {
        private const string DefaultKey = "Default";
        private const string LowerCaseKey = "LOWERCASE";
        private const string ProperCaseKey = "PROPERCASE";
        private const string UpperCaseKey = "UPPERCASE";

        private const string LowerCaseDescription = "lower case";
        private const string ProperCaseDescription = "Proper Case";
        private const string UpperCaseDescription = "UPPER CASE";

        public static void LoadEditCase(IDictionary<string, string> downloadfields, ListBox selectedFields)
        {
            Guard.NotNull(downloadfields, nameof(downloadfields));
            Guard.NotNull(selectedFields, nameof(selectedFields));

            foreach (var field in downloadfields)
            {
                var item = selectedFields.Items.FindByValue(field.Key);
                if (item != null)
                {
                    var curIndex = selectedFields.Items.IndexOf(item);
                    selectedFields.Items.Remove(item);

                    string text;
                    switch (field.Value.ToUpper())
                    {
                        case ProperCaseKey:
                            text = ProperCaseDescription;
                            break;
                        case UpperCaseKey:
                            text = UpperCaseDescription;
                            break;
                        case LowerCaseKey:
                            text = LowerCaseDescription;
                            break;
                        default:
                            text = DefaultKey;
                            break;
                    }

                    var itemText = $"{item.Text.Split('(')[0].ToUpper()}({text})";
                    var itemValue = $"{item.Value.Split('|')[0]}|{item.Value.Split('|')[1]}|{field.Value}";

                    selectedFields.Items.Insert(curIndex, new ListItem(itemText, itemValue));
                }
            }
        }

        public static void ReorderSelectedList(ListBox lstDestFields)
        {
            Guard.NotNull(lstDestFields, nameof(lstDestFields));
            var startindex = lstDestFields.Items.Count - 1;

            for (var position = startindex; position > -1; position--)
            {
                if (lstDestFields.Items[position].Selected && (position < startindex && !lstDestFields.Items[position + 1].Selected))
                {
                    var bottom = lstDestFields.Items[position];
                    lstDestFields.Items.Remove(bottom);
                    lstDestFields.Items.Insert(position + 1, bottom);
                    lstDestFields.Items[position + 1].Selected = true;
                }
            }
        }
    }
}