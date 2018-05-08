using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace WpfControls.WindowsAndDialogs
{
    /// <summary>
    /// Interaction logic for ColumnReorder.xaml
    /// </summary>
    public partial class ColumnReorder : Window
    {
        DataTable MyDataTable = new DataTable();
        Dictionary<string, int> ordering = new Dictionary<string, int>();
        public event Action<Dictionary<string ,int>> Answer;
        public ColumnReorder(DataTable dt)
        {
            InitializeComponent();

            dt.Rows.Clear();
            if (dt.Columns.Contains("PubSubscriptionID"))
                dt.Columns.Remove("PubSubscriptionID");
            if (dt.Columns.Contains("IssueCompDetailId"))
                dt.Columns.Remove("IssueCompDetailId");

            MyDataTable = dt;            
            grdCols.ItemsSource = dt;
        }

        private void grdCols_ColumnReordered(object sender, Telerik.Windows.Controls.GridViewColumnEventArgs e)
        {
            if (!ordering.ContainsKey(e.Column.UniqueName))
                ordering.Add(e.Column.UniqueName, e.Column.DisplayIndex);
            else
                ordering[e.Column.UniqueName] = e.Column.DisplayIndex;
            ////MyDataTable.Columns[e.Column.UniqueName].SetOrdinal(e.Column.DisplayIndex);
            ////int index = MyDataTable.Columns.IndexOf(e.Column.UniqueName);
            //////int oldValue = ordering[index];
            ////ordering[index] = e.Column.DisplayIndex;
            ////ordering[e.Column.DisplayIndex] = oldValue;
            //int oldIndex = ordering[e.Column.UniqueName];
            //ordering[e.Column.UniqueName] = e.Column.DisplayIndex;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //foreach (GridViewColumn column in grdCols.Columns)
            //{
            //    ordering[column.UniqueName] = column.DisplayIndex;
            //    //if (column is GridViewDataColumn)
            //    //{
            //    //    GridViewDataColumn col = column as GridViewDataColumn;
            //    //    if (col != null)
            //    //    {
            //    //        col.Width = 90;
            //    //        col.HeaderText = "Column count : " + i.ToString();
            //    //        i++;
            //    //    }
            //    //}
            //}
            if (Answer != null)
                Answer(ordering);
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
