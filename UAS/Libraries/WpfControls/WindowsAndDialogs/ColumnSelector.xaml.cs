using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.DragDrop.Behaviors;

namespace WpfControls.WindowsAndDialogs
{
    /// <summary>
    /// Interaction logic for ColumnSelector.xaml
    /// </summary>
    public partial class ColumnSelector : Window
    {
        ObservableCollection<string> addedCols = new ObservableCollection<string>();
        ObservableCollection<string> currCols = new ObservableCollection<string>();
        public event Action<List<string>> Answer;

        public ColumnSelector()
        {
            InitializeComponent();

            currCols.Add("ACSCode");
            currCols.Add("IMBSeq");
            currCols.Add("MailerID");
            currCols.Add("Keyline");
            currCols.Add("Split");
            currCols.Add("KeyCode");
            currCols.Add("Copies");
            currCols.Add("ProductCode");
            //currCols.Add("SequenceID");
            currCols.Add("PubCategoryID");
            currCols.Add("PubTransactionID");
            currCols.Add("Qualificationdate");
            currCols.Add("ExpireIssueDate");
            currCols.Add("FirstName");
            currCols.Add("LastName");
            currCols.Add("Title");
            currCols.Add("Company");
            currCols.Add("Address1");
            currCols.Add("Address2");
            currCols.Add("Address3");
            currCols.Add("City");
            currCols.Add("RegionCode");
            currCols.Add("ZipCode");
            currCols.Add("Plus4");
            currCols.Add("Country");

            rlbCurrentColumns.ItemsSource = currCols;
            rlbAddedColumn.ItemsSource = addedCols;
        }

        private void rlbCurrentColumns_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RadListBox rcb = sender as RadListBox;
            if (rcb.SelectedItem != null)
            {
                addedCols.Add(rcb.SelectedItem.ToString());
                currCols.Remove(rcb.SelectedItem.ToString());
            }
        }

        private void rlbAddedColumn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RadListBox rcb = sender as RadListBox;
            if (rcb.SelectedItem != null)
            {
                currCols.Add(rcb.SelectedItem.ToString());
                addedCols.Remove(rcb.SelectedItem.ToString());
            }
        }

        private void rlbAddedColumn_PreviewDrop(object sender, System.Windows.DragEventArgs e)
        {
            var data = DataObjectHelper.GetData(e.Data, typeof(string), true) as IEnumerable;
            string t = data.OfType<string>().First();
            addedCols.Add(t);
            e.Handled = true;
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            List<string> cols = new List<string>(addedCols);
            if (Answer != null)
                Answer(cols);

            this.Close();
        }
    }
}
