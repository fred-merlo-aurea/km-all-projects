using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfControls.WindowsAndDialogs
{
    /// <summary>
    /// Interaction logic for SplitsOrganizer.xaml
    /// </summary>
    public partial class SplitsOrganizer : Window
    {
        public event Action<string> Answer;

        public SplitsOrganizer(List<string> splits, int records)
        {
            InitializeComponent();
            lbSplits.ItemsSource = splits;
            txtStandard.Text = "Choose a Split to apply " + records.ToString() + " records to: ";
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (lbSplits.SelectedItem != null)
            {
                if (Answer != null)
                    Answer(lbSplits.SelectedItem.ToString());

                this.Close();
            }
            else
                MessageBox.Show("Please select an Issue Split.");
        }
    }
}
