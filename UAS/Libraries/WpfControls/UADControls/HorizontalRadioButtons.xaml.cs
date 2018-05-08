using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfControls.UADControls
{
    /// <summary>
    /// Interaction logic for HorizontalRadioButtons.xaml
    /// </summary>
    public partial class HorizontalRadioButtons : UserControl
    {
        public HorizontalRadioButtons(string tag)
        {
            InitializeComponent();

            TagType = tag;

            rbtnSearchAll.IsChecked = true;            
        }

        private string _tagType = "";
        private string _selectedText = "";
        private string _controlName = "";

        public string TagType
        {
            get { return _tagType; }
            set
            {
                _tagType = value;
                rbtnSearchAll.Tag = value;
                rbtnSearchSelected.Tag = value;
            }
        }

        public string SelectedText
        {
            get { return _selectedText; }
            set
            {
                _selectedText = value;
                if (_selectedText == "Search All")
                    rbtnSearchAll.IsChecked = true;
                else if (_selectedText == "Search Selected")
                    rbtnSearchSelected.IsChecked = true;
                else
                {
                    rbtnSearchAll.IsChecked = false;
                    rbtnSearchSelected.IsChecked = false;
                }
            }
        }

        public string ControlName
        {
            get { return _controlName; }
        }

        private void rbtnSearchAll_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            _selectedText = "Search All";
            _controlName = rbtn.Name;
        }

        private void rbtnSearchSelected_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            _selectedText = "Search Selected";
            _controlName = rbtn.Name;
        }

        private void rbtnSearchAll_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            _selectedText = string.Empty;
            _controlName = string.Empty;
        }

        private void rbtnSearchSelected_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            _selectedText = string.Empty;
            _controlName = string.Empty;
        }
    }
}
