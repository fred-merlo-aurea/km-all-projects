//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows;
//using System.Windows.Media;
//using System.Windows.Data;
//using System.Windows.Controls;
//using System.Windows.Controls.DataVisualization;
//using System.Windows.Controls.Primitives;
//using System.Web.UI.WebControls;
//using Xceed.Wpf.Toolkit;
//using System.Configuration;
//using System.Net.Mail;
//using System.Reflection;
//using GD_Framework.Entity;
//using GD_Framework.Object;

//namespace KM.Common.Functions
//{
//    public static class WPF_Helpers
//    {
        
//        #region DataGrid Helpers
//        public static IEnumerable<DataGrid> GetDataGridRows(DataGrid grid)
//        {
//            var itemsSource = grid.ItemsSource as IEnumerable<DataGridRow>;
//            if (null == itemsSource) yield return null;
//            foreach (var item in itemsSource)
//            {
//                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as System.Windows.Controls.DataGridRow;
//                if (null != row) yield return row;
//            }
//        }
//        /// <summary>
//        /// helper function for selecting a visual child
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="parent"></param>
//        /// <returns></returns>
//        private static T GetVisualChild<T>(Visual parent) where T : Visual
//        {
//            T child = default(T);
//            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
//            for (int i = 0; i < numVisuals; i++)
//            {
//                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
//                child = v as T;
//                if (child == null)
//                {
//                    child = GetVisualChild<T>(v);
//                }
//                if (child != null)
//                {
//                    break;
//                }
//            }
//            return child;
//        }
//        /// <summary>
//        ///  method for getting the current (selected) row of the DataGrid
//        /// </summary>
//        /// <param name="grid"></param>
//        /// <returns></returns>
//        public static GridViewRow GetSelectedRow(this GridView grid)
//        {
//            return (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
//        }
//        /// <summary>
//        ///  get a row by its indices
//        /// </summary>
//        /// <param name="grid"></param>
//        /// <param name="index"></param>
//        /// <returns></returns>
//        public static DataGridRow GetRow(this DataGrid grid, int index)
//        {
//            DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
//            if (row == null)
//            {
//                // May be virtualized, bring into view and try again.
//                grid.UpdateLayout();
//                grid.ScrollIntoView(grid.Items[index]);
//                row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
//            }
//            return row;
//        }
//        /// <summary>
//        ///  get a cell of a DataGrid by an existing row
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="depObj"></param>
//        /// <param name="childName"></param>
//        /// <returns></returns>
//        public static DataGridCell GetCell(this DataGrid grid, DataGridRow row, int column)
//        {
//            if (row != null)
//            {
//                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

//                if (presenter == null)
//                {
//                    grid.ScrollIntoView(row, grid.Columns[column]);
//                    presenter = GetVisualChild<DataGridCellsPresenter>(row);
//                }

//                DataGridCell cell = null;
//                if (presenter != null)
//                    cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
//                return cell;
//            }
//            return null;
//        }
//        /// <summary>
//        ///  select a row by its indices
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="depObj"></param>
//        /// <param name="childName"></param>
//        /// <returns></returns>
//        public static DataGridCell GetCell(this DataGrid grid, int row, int column)
//        {
//            DataGridRow rowContainer = grid.GetRow(row);
//            return grid.GetCell(rowContainer, column);
//        }
//        public static DataGridCell GetCell(DataGrid grid, DataGridCellInfo cellInfo)
//        {
//            DataGridCell result = null;
//            DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(cellInfo.Item);
//            if (row != null)
//            {
//                int columnIndex = grid.Columns.IndexOf(cellInfo.Column);
//                if (columnIndex > -1)
//                {
//                    DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);
//                    result = presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex) as DataGridCell;
//                }
//            }
//            return result;
//        }
//        public static int GetRowIndex(DataGridCell dataGridCell)
//        {
//            // Use reflection to get DataGridCell.RowDataItem property value.
//            PropertyInfo rowDataItemProperty = dataGridCell.GetType().GetProperty("RowDataItem", BindingFlags.Instance | BindingFlags.NonPublic);

//            DataGrid dataGrid = GetDataGridFromChild(dataGridCell);

//            return dataGrid.Items.IndexOf(rowDataItemProperty.GetValue(dataGridCell, null));
//        }
//        public static DataGrid GetDataGridFromChild(DependencyObject dataGridPart)
//        {
//            if (VisualTreeHelper.GetParent(dataGridPart) == null)
//            {
//                throw new NullReferenceException("Control is null.");
//            }
//            if (VisualTreeHelper.GetParent(dataGridPart) is DataGrid)
//            {
//                return (DataGrid)VisualTreeHelper.GetParent(dataGridPart);
//            }
//            else
//            {
//                return GetDataGridFromChild(VisualTreeHelper.GetParent(dataGridPart));
//            }
//        }
//        public static T FindChild<T>(DependencyObject depObj, string childName)
//           where T : DependencyObject
//        {
//            // Confirm obj is valid. 
//            if (depObj == null) return null;

//            // success case
//            if (depObj is T && ((FrameworkElement)depObj).Name == childName)
//                return depObj as T;

//            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
//            {
//                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

//                //DFS
//                T obj = FindChild<T>(child, childName);

//                if (obj != null)
//                    return obj;
//            }

//            return null;
//        }
//        public static MainWindow GetMainWindow(DependencyObject depObj)
//        {
//            Window w = Window.GetWindow(depObj);//MainWindow
//            MainWindow mw = (MainWindow)w;//Application.Current.MainWindow;
//            return mw;
//        }
//        #endregion

//    }
//}
