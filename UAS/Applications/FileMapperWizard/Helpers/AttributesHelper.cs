using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core_AMS.Utilities;
using FileMapperWizard.Modules;
using Telerik.Windows.Controls;

namespace FileMapperWizard.Helpers
{
    public class AttributesHelper
    {
        private const string AskToLeave = "You have not saved.  Are you sure you want to leave?";

        public static string GetFirstChildText(DependencyObject dependencyObject)
        {
            var tbList = WPF.FindVisualChildren<TextBlock>(dependencyObject);
            return tbList.First().Text;
        }

        public static void SetCheckBoxForRadioList(CheckBox checkedBox, RadListBox radioListBox)
        {
            var checkBoxes = WPF.FindVisualChildren<CheckBox>(radioListBox);

            foreach (var item in checkBoxes)
            {
                if (checkedBox != item)
                {
                    item.IsChecked = false;
                }
                else
                {
                    item.IsChecked = checkedBox.IsChecked;
                }
            }
        }

        public static void SetVisibility(Visibility visibility, params UIElement[] elements)
        {
            foreach (var element in elements)
            {
                element.Visibility = visibility;
            }
        }

        public static bool GoNextAllowed(bool didSave)
        {
            if (didSave)
            {
                return true;
            }

            var messageBoxResult = WPF.MessageResult(AskToLeave, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return messageBoxResult == MessageBoxResult.Yes;
        }

        public static Border FindBorder(DataCompareSteps compareSteps, string borderName)
        {
            var borderList = WPF.FindVisualChildren<Border>(compareSteps);
            var border = borderList.FirstOrDefault(x => x.Name.Equals(borderName, StringComparison.CurrentCultureIgnoreCase));
            return border;
        }
    }
}
