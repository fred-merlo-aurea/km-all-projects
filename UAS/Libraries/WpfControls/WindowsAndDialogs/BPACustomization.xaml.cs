using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace WpfControls.WindowsAndDialogs
{
    /// <summary>
    /// Interaction logic for BPACustomization.xaml
    /// </summary>
    public partial class BPACustomization : Window
    {
        ObservableCollection<FrameworkUAD.Entity.Report> addedCols = new ObservableCollection<FrameworkUAD.Entity.Report>();
        ObservableCollection<FrameworkUAD.Entity.Report> currCols = new ObservableCollection<FrameworkUAD.Entity.Report>();
        public event Action<ObservableCollection<FrameworkUAD.Entity.Report>> Answer;
        public BPACustomization(List<FrameworkUAD.Entity.Report> reports)
        {
            InitializeComponent();

            //reports.RemoveAll(x => x.URL == "~/main/reports/CrossTabNew.aspx" || x.URL =);
            currCols = new ObservableCollection<FrameworkUAD.Entity.Report>(reports);

            rlbCurrentColumns.ItemsSource = currCols;
            rlbAddedColumn.ItemsSource = addedCols;
        }
        private void rlbCurrentColumns_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RadListBox rcb = sender as RadListBox;
            if (rcb.SelectedItem != null)
            {
                addedCols.Add((FrameworkUAD.Entity.Report)rcb.SelectedItem);
                currCols.Remove((FrameworkUAD.Entity.Report)rcb.SelectedItem);
            }
        }

        private void rlbAddedColumn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RadListBox rcb = sender as RadListBox;
            if (rcb.SelectedItem != null)
            {
                currCols.Add((FrameworkUAD.Entity.Report)rcb.SelectedItem);
                addedCols.Remove((FrameworkUAD.Entity.Report)rcb.SelectedItem);
            }
        }

        private void rlbAddedColumn_PreviewDrop(object sender, System.Windows.DragEventArgs e)
        {

            RadListBox rcb = sender as RadListBox;
            if (rcb.SelectedItems != null)
            {
                foreach (var item in rcb.SelectedItems)
                {
                    addedCols.Add((FrameworkUAD.Entity.Report)item);
                    currCols.Remove((FrameworkUAD.Entity.Report)item);
                }
            }
            //var data = DataObjectHelper.GetData(e.Data, typeof(FrameworkUAD.Entity.Report), true) as IEnumerable;
            //if(data != null)
            //    addedCols.Add((FrameworkUAD.Entity.Report)data);
            //e.Handled = true;
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            if (Answer != null)
                Answer(addedCols);

            this.DialogResult = true;

            this.Close();
        }
    }
}
