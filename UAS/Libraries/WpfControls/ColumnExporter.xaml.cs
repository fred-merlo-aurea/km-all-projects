using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.DragDrop.Behaviors;

namespace WpfControls
{
    /// <summary>
    /// Interaction logic for ColumnExporter.xaml
    /// </summary>
    public partial class ColumnExporter : Window
    {
        public event Action<ObservableCollection<NewColumn>> Answer;
        ObservableCollection<string> addedCols = new ObservableCollection<string>();
        ObservableCollection<string> currCols = new ObservableCollection<string>();
        ObservableCollection<NewColumn> newCols = new ObservableCollection<NewColumn>();
        public class NewColumn
        {
            public string Name { get; set; }
            public List<string> Columns { get; set; }
            public string Delimiter { get; set; }

            public NewColumn(string name, List<string> cols, string delimiter)
            {
                this.Name = name;
                this.Columns = cols;
                this.Delimiter = delimiter;
            }
        }
        public ColumnExporter(List<string> columns)
        {
            InitializeComponent();

            currCols = new ObservableCollection<string>(columns);

            rlbCurrentColumns.ItemsSource = currCols;
            rlbAddedColumn.ItemsSource = addedCols;
            rlbNewCols.ItemsSource = newCols;

            rcbDelimiters.Items.Add("Comma");
            rcbDelimiters.Items.Add("Space");
            rcbDelimiters.Items.Add("None");
        }
        private void OnDragOver(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
        {
            //var options = DragDropPayloadManager.GetDataFromObject() as TreeViewDragDropOptions;
            //RadTreeViewItem target = options.DropTargetItem as RadTreeViewItem;

            //if (options != null)
            //{
            //    options.DropPosition = DropPosition.Before;
            //    var dragVisual = options.DragVisual as TreeViewDragVisual;
            //    if (dragVisual != null)
            //    {
            //        dragVisual.IsDropPossible = true;
            //        dragVisual.DropActionText = "Map to ";
            //    }
            //}

            //if (DataObjectHelper.GetDataPresent(e.Data, typeof(string), false))
            //{
            //    var customers = DataObjectHelper.GetData(e.Data, typeof(string), true) as IEnumerable;
            //    if (customers != null)
            //    {
            //        var newApp = customers.OfType<Customer>().Select(c => new Appointment { Subject = c.Name });
            //        return newApp;
            //    }
            //}
        }

        #region UI Events

        private void btnNewColumn_Click(object sender, RoutedEventArgs e)
        {
            string error = "";
            if(tBoxName.Text == "")
            {
                error += "New Column Name not entered, ";
            }
            if(rcbDelimiters.SelectedItem == null)
            {
                error += "Delimiter not selected, ";
            }
            if(rlbAddedColumn.Items.Count == 0)
            {
                error += "No columns selected";
            }
            if(error != "")
            {
                error = error.TrimEnd(' ');
                error = error.TrimEnd(',');
                MessageBox.Show("Please fix the following errors: " + error + ".", "Error", MessageBoxButton.OK);
            }
            else
            {
                List<string> cols = new List<string>();
                foreach(string s in rlbAddedColumn.Items)
                    cols.Add(s);
                newCols.Add(new NewColumn(tBoxName.Text, cols, rcbDelimiters.SelectedItem.ToString()));
                tBoxName.Text = "";
                rcbDelimiters.SelectedItem = null;
                addedCols.Clear();
            }
        }

        private void rlbNewCols_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadListBox rlb = sender as RadListBox;
            if(rlb.SelectedItem != null)
            {
                dpDelimiters.Visibility = System.Windows.Visibility.Visible;
                rlbPreviewColumns.Items.Clear();
                NewColumn me = rlb.SelectedItem as NewColumn;
                foreach(string s in me.Columns)
                {
                    rlbPreviewColumns.Items.Add(s);
                }
                txtDelimiter.Text = me.Delimiter;
            }
        }

        private void rlbAddedColumn_PreviewDrop(object sender, System.Windows.DragEventArgs e)
        {
            var data = DataObjectHelper.GetData(e.Data, typeof(string), true) as IEnumerable;
            string t = data.OfType<string>().First();
            addedCols.Add(t);
            e.Handled = true;
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (Answer != null)
                Answer(newCols);

            this.Close();
        }

        private void btnRemoveCol_Click(object sender, RoutedEventArgs e)
        {
            RadButton btn = sender as RadButton;
            NewColumn n = btn.DataContext as NewColumn;
            if (rlbNewCols.SelectedItem == n)
            {
                dpDelimiters.Visibility = System.Windows.Visibility.Hidden;
                rlbPreviewColumns.Items.Clear();
            }
            foreach (string s in n.Columns)
            {
                currCols.Add(s);
            }
            newCols.Remove(n);
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

        #endregion
    }
}
