using Core_AMS.Utilities;
using System;
using System.Linq;
using System.Windows;

namespace WpfControls.WindowsAndDialogs
{
    /// <summary>
    /// Interaction logic for TwoTextBoxDialog.xaml
    /// </summary>
    public partial class TwoTextBoxDialog : Window
    {
        public event Action<Enums.DialogResponses> Answer;
        public event Action<string> NameOneAnswer;
        public event Action<string> NameTwoAnswer;
        public TwoTextBoxDialog(string windowTitle, string saveText, string firstTxtBoxHeader = "", string secondTxtBoxHeader = "", bool cancelEnabled = true)
        {
            InitializeComponent();

            this.Title = windowTitle;
            this.txtDescription.Text = saveText;
            this.txtBoxOneHeader.Text = firstTxtBoxHeader;
            this.txtBoxTwoHeader.Text = secondTxtBoxHeader;
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
            if (txtBoxNameOne.Text != null && txtBoxNameOne.Text != String.Empty && txtBoxNameTwo.Text != null && txtBoxNameTwo.Text != String.Empty)
            {
                this.Closing -= Window_Closing;

                if (Answer != null)
                    Answer(Enums.DialogResponses.Save);

                if (NameOneAnswer != null)
                    NameOneAnswer(txtBoxNameOne.Text);

                if (NameTwoAnswer != null)
                    NameTwoAnswer(txtBoxNameTwo.Text);

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
            txtBoxNameOne.Focus();
        }
    }
}
