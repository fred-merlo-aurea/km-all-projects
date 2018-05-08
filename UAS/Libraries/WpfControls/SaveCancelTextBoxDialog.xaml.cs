using Core_AMS.Utilities;
using System;
using System.Linq;
using System.Windows;

namespace WpfControls
{
    /// <summary>
    /// Interaction logic for SaveAddRemoveFilterDialog.xaml
    /// </summary>
    public partial class SaveCancelTextBoxDialog : Window
    {
        public event Action<Enums.DialogResponses> Answer;
        public event Action<string> filterNameAnswer;

        public SaveCancelTextBoxDialog(string windowTitle, string saveText, bool cancelEnabled = true)
        {
            InitializeComponent();
            this.Title = windowTitle;
            this.txtDescription.Text = saveText;
            if (cancelEnabled == true)
                btnCancel.Visibility = Visibility.Visible;
            else
            {
                btnCancel.Visibility = Visibility.Collapsed;
                txtHeader.Text = "Save";
                this.Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxFilterName.Text != null && txtBoxFilterName.Text != String.Empty)
            {
                this.Closing -= Window_Closing;

                if (Answer != null)
                    Answer(Enums.DialogResponses.Save);

                if (filterNameAnswer != null)
                    filterNameAnswer(txtBoxFilterName.Text);

                this.Close();
            }
            else
                MessageBox.Show("Please enter a name.");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Answer != null)
                Answer(Enums.DialogResponses.Cancel);

            this.Close();
        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtBoxFilterName.Focus();
        }
    }
}
