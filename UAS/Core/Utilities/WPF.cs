using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Configuration;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Telerik.Windows.Documents.FormatProviders;

namespace Core_AMS.Utilities
{
    public static class WPF
    {
        //private static bool dialogResult { get; set; }

        #region Message
        public static void Message(string msg, string caption = "")
        {
            //Xceed.Wpf.Toolkit.MessageBox.Show(msg, caption);
            MessageBox.Show(msg, caption);
        }
        public static void Message(string msg, System.Windows.MessageBoxButton btn, string caption = "")
        {
            //Xceed.Wpf.Toolkit.MessageBox.Show(msg, caption, btn);
            MessageBox.Show(msg, caption, btn);
        }
        public static void Message(string msg, System.Windows.MessageBoxButton btn, System.Windows.MessageBoxImage icon, string caption = "")
        {
            //Xceed.Wpf.Toolkit.MessageBox.Show(msg, caption, btn, icon);
            MessageBox.Show(msg, caption, btn, icon);
        }

        public static void Message(string msg, System.Windows.MessageBoxButton btn, System.Windows.MessageBoxImage icon, System.Windows.MessageBoxResult result, string caption = "")
        {
            //Xceed.Wpf.Toolkit.MessageBox.Show(msg, caption, btn, icon, result);
            //RadMessage(msg, btn, icon, result, caption);
            MessageBox.Show(msg, caption, btn, icon, result);
        }
        public static MessageBoxResult MessageResult(string msg, System.Windows.MessageBoxButton btn, System.Windows.MessageBoxImage icon, string caption = "")
        {
            //MessageBoxResult messageResult = Xceed.Wpf.Toolkit.MessageBox.Show(msg, caption, btn, icon);
            //return messageResult;
            MessageBoxResult messageResult = MessageBox.Show(msg, caption, btn, icon);
            return messageResult;
        }

        #region TelerikAttempt
        //public static void radClosing(object sender, WindowClosedEventArgs e)
        //{
        //    if (e.DialogResult == true)
        //        dialogResult = true;
        //    else
        //        dialogResult = false;
        //}

        //public static DialogParameters ComputeRadContent(System.Windows.MessageBoxButton btn, System.Windows.MessageBoxImage icon)
        //{
        //    DialogParameters dialog = new DialogParameters();
        //    //dialog.Closed = radClosing;
        //    //dialog.
        //    switch(btn)
        //    {
        //        case MessageBoxButton.OK:
        //            dialog.OkButtonContent = "Ok";
        //            break;
        //        case MessageBoxButton.OKCancel:
        //            dialog.OkButtonContent = "Ok";
        //            dialog.CancelButtonContent = "Cancel";
        //            break;
        //        case MessageBoxButton.YesNo:
        //            dialog.OkButtonContent = "Yes";
        //            dialog.CancelButtonContent = "No";
        //            break;
        //    }
        //    return dialog;
        //}

        //public static void RadMessage(string msg, System.Windows.MessageBoxButton btn, System.Windows.MessageBoxImage icon, System.Windows.MessageBoxResult result, string caption = "")
        //{
        //    DialogParameters dialogResult = ComputeRadContent(btn, icon);
        //    Style style = Application.Current.FindResource("RadPopUp") as Style;
        //    //Style image = Application.Current.FindResource("")
        //    Style style2 = Application.Current.FindResource("Test") as Style;
        //    System.Drawing.Image i = (System.Drawing.Image)style.Resources.FindName("IconRadWindow");
        //    Telerik.Windows.Controls.RadWindow.Confirm(new DialogParameters()
        //    {
        //        OkButtonContent = dialogResult.OkButtonContent,
        //        CancelButtonContent = dialogResult.CancelButtonContent,
        //        ContentStyle = style2,
        //        Content = msg,
        //        IconContent = caption,
        //        Header = "",
        //        WindowStyle = style,
        //        Closed = radClosing
        //    });
        //}

        //public static bool RadMessageResult(string msg, System.Windows.MessageBoxButton btn, System.Windows.MessageBoxImage icon, System.Windows.MessageBoxResult result, string caption = "")
        //{
        //    Style style = Application.Current.FindResource("RadPopUp") as Style;
        //    Telerik.Windows.Controls.RadWindow.Confirm(new DialogParameters()
        //    {
        //        //OkButtonContent = "Hello",
        //        //CancelButtonContent = "Goodbye",
        //        //ContentStyle = style,
        //        Content = msg,
        //        IconContent = caption,
        //        WindowStyle = style,
        //        Closed = radClosing
        //    });
        //    return dialogResult;
        //}
        #endregion

        public static void MessageAccessDenied()
        {
            Message("You do not have access to this module.", MessageBoxButton.OK, MessageBoxImage.Stop, "Access Denied");
        }
        public static void MessageApplicationErrorMessage()
        {
            Message("An ERROR occurred during the operation, please contact Support.", MessageBoxButton.OK, MessageBoxImage.Error, "Contact Support");
        }
        public static void MessageObjectNull(string nullObject)
        {
            Message(nullObject.ToUpper() + " cannot be empty, the current process will be stopped.", MessageBoxButton.OK, MessageBoxImage.Error, "Contact Support");
        }
        public static void MessageSaveComplete()
        {
            Message("Save Complete", MessageBoxButton.OK, MessageBoxImage.Information, "Save Complete");
        }
        public static void LogOutComplete()
        {
            Message("Log Out Complete - you may now close your browser", MessageBoxButton.OK, MessageBoxImage.Information, "Log Out Complete");
        }
        public static void MessageDataExportComplete()
        {
            Message("Data Export Complete", MessageBoxButton.OK, MessageBoxImage.Information, "Data Export Complete");
        }
        public static void MessageUpdateComplete()
        {
            Message("Update Complete", MessageBoxButton.OK, MessageBoxImage.Information, "Update Complete");
        }
        public static void MessageInsertComplete()
        {
            Message("Insert Complete", MessageBoxButton.OK, MessageBoxImage.Information, "Insert Complete");
        }
        public static void MessageDeleteComplete()
        {
            Message("Delete Complete", MessageBoxButton.OK, MessageBoxImage.Information, "Delete Complete");
        }
        public static void MessageTaskComplete()
        {
            Message("Task Complete", MessageBoxButton.OK, MessageBoxImage.Information, "Task Complete");
        }
        public static void MessageTaskComplete(string msg)
        {
            Message("Task Complete - " + msg, MessageBoxButton.OK, MessageBoxImage.Information, "Task Complete");
        }
        public static void MessageTaskValidationFailed(string msg)
        {
            Message("Task Validation Failed - " + msg, MessageBoxButton.OK, MessageBoxImage.Information, "Task Validation Failed");
        }
        public static void MessageEmptyGrid()
        {
            Message("No results to display", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, "No Data");
        }
        public static void MessageFinalizeBatch()
        {
            Message("You must finalize your current batch before you are allowed to process any more transactions", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.OK, "Finalize Batch");
        }
        public static void MessageSelectPublication()
        {
            Message("Please select a Publication to proceed.", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.OK, "Publication Required");
        }
        public static void MessageFileUploadComplete()
        {
            Message("File upload complete.", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, "Success");
        }
        public static void MessageFileDownloadComplete(string filePath)
        {
            Message("File download complete - " + filePath, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, "Success");
        }
        public static void MessageFileUploadError()
        {
            Message("File upload error.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Error");
        }
        public static void MessageFileOpenWarning()
        {
            Message("The file you are attempting to open or upload is currently in use, please close the file and try again.", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, "File open or in use");
        }
        public static void MessageError(string error)
        {
            Message(error, MessageBoxButton.OK, MessageBoxImage.Error, "Error");
        }
        public static void MessageServiceError()
        {
            Message("An unexpected error occurred during a service request.  Please try again.  If the problem persists, please contact Customer Support.", MessageBoxButton.OK, MessageBoxImage.Error, "Error");
        }
        public static void SearchMessages(int messageId)
        {
            if (messageId == 1)
            {
                Message("Can't search on Address alone, need to have City,State or Zip populated", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.OK, "Address Search");
            }
            else if (messageId == 2)
            {
                Message("Address field must contain 4 or more characters", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.OK, "Address Search");
            }
            else if (messageId == 3)
            {
                Message("Name field must be longer than 2 characters", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.OK, "Name Search");
            }
            else if (messageId == 4)
            {
                Message("Can't search on Title alone, must include Company", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.OK, "Title Search");
            }
            else if (messageId == 5)
            {
                Message("Can't search on State alone, need to have Address,City or Zip populated", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.OK, "Address Search");
            }
            else if (messageId == 6)
            {
                Message("Can't search on Zip alone, need to have Address,City or State populated", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.OK, "Address Search");
            }
            else if (messageId == 7)
            {
                Message("Can't search on Company alone, must include Title", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.OK, "Title Search");
            }
            else if (messageId == 8)
            {
                Message("Can't search on City alone, need to have Address, State or Zip", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.OK, "Title Search");
            }
        }
        #endregion
        #region Telerik Controls
        #region GridView
        public static IEnumerable<Telerik.Windows.Controls.GridView.GridViewRow> GetDataGridRows(Telerik.Windows.Controls.RadGridView grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable<Telerik.Windows.Controls.GridView.GridViewRow>;
            if (null == itemsSource) yield return null;
            if (itemsSource != null)
            {
                foreach (var item in itemsSource)
                {
                    var row = grid.ItemContainerGenerator.ContainerFromItem(item) as Telerik.Windows.Controls.GridView.GridViewRow;
                    if (null != row) yield return row;
                }
            }
        }
        public static Telerik.Windows.Controls.GridView.GridViewRow GetSelectedRow(this Telerik.Windows.Controls.RadGridView grid)
        {
            return (Telerik.Windows.Controls.GridView.GridViewRow)grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
        }
        public static Telerik.Windows.Controls.GridView.GridViewRow GetRow(this Telerik.Windows.Controls.RadGridView grid, int index)
        {
            Telerik.Windows.Controls.GridView.GridViewRow row = (Telerik.Windows.Controls.GridView.GridViewRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // May be virtualized, bring into view and try again.
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.Items[index]);
                row = (Telerik.Windows.Controls.GridView.GridViewRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }
        public static Telerik.Windows.Controls.GridView.GridViewCell GetCell(this Telerik.Windows.Controls.RadGridView grid, Telerik.Windows.Controls.GridView.GridViewRowItem row, int column)
        {
            if (row != null)
            {
                Telerik.Windows.Controls.GridView.DataCellsPresenter presenter = GetVisualChild<Telerik.Windows.Controls.GridView.DataCellsPresenter>(row);
                
                if (presenter == null)
                {
                    grid.ScrollIntoView(row, grid.Columns[column]);
                    presenter = GetVisualChild<Telerik.Windows.Controls.GridView.DataCellsPresenter>(row);
                }

                Telerik.Windows.Controls.GridView.GridViewCell cell = null;
                if (presenter != null)
                    cell = (Telerik.Windows.Controls.GridView.GridViewCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                else
                {
                    cell = grid.GetCell(row, column);
                }
                return cell;
            }
            return null;
        }
        public static Telerik.Windows.Controls.GridView.GridViewCell GetCell(this Telerik.Windows.Controls.RadGridView grid, int row, int column)
        {
            Telerik.Windows.Controls.GridView.GridViewRow rowContainer = grid.GetRow(row);
            return grid.GetCell(rowContainer, column);
        }
        public static Telerik.Windows.Controls.GridView.GridViewCell GetCell(Telerik.Windows.Controls.RadGridView grid, Telerik.Windows.Controls.GridViewCellInfo cellInfo)
        {
            Telerik.Windows.Controls.GridView.GridViewCell result = null;
            Telerik.Windows.Controls.GridView.GridViewRow row = (Telerik.Windows.Controls.GridView.GridViewRow)grid.ItemContainerGenerator.ContainerFromItem(cellInfo.Item);
            if (row != null)
            {
                int columnIndex = grid.Columns.IndexOf(cellInfo.Column);
                if (columnIndex > -1)
                {
                    Telerik.Windows.Controls.GridView.DataCellsPresenter presenter = GetVisualChild<Telerik.Windows.Controls.GridView.DataCellsPresenter>(row);
                    result = presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex) as Telerik.Windows.Controls.GridView.GridViewCell;
                }
            }
            return result;
        }
        public static int GetRowIndex(Telerik.Windows.Controls.GridView.GridViewCell dataGridCell)
        {
            PropertyInfo rowDataItemProperty = dataGridCell.GetType().GetProperty("DataColumn", BindingFlags.Instance | BindingFlags.NonPublic);

            Telerik.Windows.Controls.RadGridView dataGrid = GetRadGridViewFromChild(dataGridCell);

            return dataGrid.Items.IndexOf(rowDataItemProperty.GetValue(dataGridCell, null));
        }
        public static Telerik.Windows.Controls.RadGridView GetRadGridViewFromChild(DependencyObject dataGridPart)
        {
            if (VisualTreeHelper.GetParent(dataGridPart) == null)
            {
                throw new NullReferenceException("Control is null.");
            }
            if (VisualTreeHelper.GetParent(dataGridPart) is DataGrid)
            {
                return (Telerik.Windows.Controls.RadGridView)VisualTreeHelper.GetParent(dataGridPart);
            }
            else
            {
                return GetRadGridViewFromChild(VisualTreeHelper.GetParent(dataGridPart));
            }
        }
        #endregion
        #endregion
        #region DataGrid Helpers
        public static IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable<DataGridRow>;
            if (null == itemsSource) yield return null;
            if (itemsSource != null)
            {
                foreach (var item in itemsSource)
                {
                    var row = grid.ItemContainerGenerator.ContainerFromItem(item) as System.Windows.Controls.DataGridRow;
                    if (null != row) yield return row;
                }
            }
        }

        /// <summary>
        /// helper function for selecting a visual child
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        private static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
        /// <summary>
        ///  method for getting the current (selected) row of the DataGrid
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static DataGridRow GetSelectedRow(this DataGrid grid)
        {
            return (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
        }
        /// <summary>
        ///  get a row by its indices
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static DataGridRow GetRow(this DataGrid grid, int index)
        {
            DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // May be virtualized, bring into view and try again.
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.Items[index]);
                row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }
        /// <summary>
        ///  get a cell of a DataGrid by an existing row
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static DataGridCell GetCell(this DataGrid grid, DataGridRow row, int column)
        {
            if (row != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

                if (presenter == null)
                {
                    grid.ScrollIntoView(row, grid.Columns[column]);
                    presenter = GetVisualChild<DataGridCellsPresenter>(row);
                }

                DataGridCell cell = null;
                if (presenter != null)
                    cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                else
                {
                    cell = grid.GetCell(row, column);
                }
                return cell;
            }
            return null;
        }
        /// <summary>
        ///  select a row by its indices
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static DataGridCell GetCell(this DataGrid grid, int row, int column)
        {
            DataGridRow rowContainer = grid.GetRow(row);
            return grid.GetCell(rowContainer, column);
        }
        public static DataGridCell GetCell(DataGrid grid, DataGridCellInfo cellInfo)
        {
            DataGridCell result = null;
            DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(cellInfo.Item);
            if (row != null)
            {
                int columnIndex = grid.Columns.IndexOf(cellInfo.Column);
                if (columnIndex > -1)
                {
                    DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);
                    result = presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex) as DataGridCell;
                }
            }
            return result;
        }
        public static int GetRowIndex(DataGridCell dataGridCell)
        {
            // Use reflection to get DataGridCell.RowDataItem property value.
            PropertyInfo rowDataItemProperty = dataGridCell.GetType().GetProperty("RowDataItem", BindingFlags.Instance | BindingFlags.NonPublic);

            DataGrid dataGrid = GetDataGridFromChild(dataGridCell);

            return dataGrid.Items.IndexOf(rowDataItemProperty.GetValue(dataGridCell, null));
        }
        public static DataGrid GetDataGridFromChild(DependencyObject dataGridPart)
        {
            if (VisualTreeHelper.GetParent(dataGridPart) == null)
            {
                throw new NullReferenceException("Control is null.");
            }
            if (VisualTreeHelper.GetParent(dataGridPart) is DataGrid)
            {
                return (DataGrid)VisualTreeHelper.GetParent(dataGridPart);
            }
            else
            {
                return GetDataGridFromChild(VisualTreeHelper.GetParent(dataGridPart));
            }
        }
        #endregion
        #region Main Application Window
        public static Window GetWindow(DependencyObject depObj)
        {
            Window w = Window.GetWindow(depObj);//MainWindow
            return w;
        }
        public static Window GetMainWindow()
        {
            Window mw = (Window)Application.Current.MainWindow;
            return mw;
        }
        #endregion
        #region Validations - Control Helpers
        public static string EmailVerify(string email)
        {
            if (email == null)
                email = string.Empty;

            Regex regexObj = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            if (!regexObj.IsMatch(email))
                email = "";
            return email;
        }
        public static bool IsValidEmail(string email)
        {
            if (email == null)
                email = string.Empty;

            Regex regexObj = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            if (!regexObj.IsMatch(email))
                return false;
            else
                return true;
        }
        public static bool TrueFalse(ListBox listBox)
        {
            bool isTrue = false;
            if (listBox.SelectedValue != null)
                bool.TryParse(listBox.SelectedValue.ToString(), out isTrue);
            return isTrue;
        }
        public static ListBox SetTrue(ListBox lstBx)
        {
            ListBoxItem li = (ListBoxItem)lstBx.Items.GetItemAt(0);
            li.IsSelected = true;
            lstBx.SelectedItem = li;
            return lstBx;
        }
        public static ListBox SetFalse(ListBox lstBx)
        {
            ListBoxItem li = (ListBoxItem)lstBx.Items.GetItemAt(1);
            li.IsSelected = true;
            lstBx.SelectedItem = li;
            return lstBx;
        }
        #endregion
        #region Window save methods
        public static void SaveCanvas(Window window, Canvas canvas, int dpi, string filename)
        {
            System.Windows.Size size = new System.Windows.Size(window.ActualWidth, window.ActualHeight);
            canvas.Measure(size);

            var rtb = new RenderTargetBitmap(
                (int)window.ActualWidth, //width 
                (int)window.ActualHeight, //height 
                dpi, //dpi x 
                dpi, //dpi y 
                PixelFormats.Pbgra32 // pixelformat 
                );
            rtb.Render(canvas);

            SaveRTBAsPNG(rtb, filename);
        }
        public static void SaveWindow(Window window, int dpi, string filename)
        {
            var rtb = new RenderTargetBitmap(
                (int)window.ActualWidth, //width 
                (int)window.ActualHeight, //height 
                dpi, //dpi x 
                dpi, //dpi y 
                PixelFormats.Pbgra32 // pixelformat 
                );
            rtb.Render(window);

            SaveRTBAsPNG(rtb, filename);
        }
        private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
        {
            //filename = Variables.FileDirectory + filename.Replace("/", "_").Replace(@"/", "_");
            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }
        #endregion
        #region App.config Encryption
        public static void EncryptSection(ConfigurationSection configSection)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (configSection.SectionInformation.IsProtected == false)
            {
                configSection.SectionInformation.ProtectSection("RSAProtectedConfigurationProvider");
                configSection.SectionInformation.ForceSave = true;
                config.Save();
            }
        }
        public static void DeCryptSection(ConfigurationSection configSection)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (configSection.SectionInformation.IsProtected == true)
            {
                configSection.SectionInformation.UnprotectSection();
                configSection.SectionInformation.ForceSave = true;
                config.Save();
            }
        }
        #endregion
        #region Create New Windows
        public static Window GetNewWindow(bool isTopMost = true, string title = "", string name = "", object content = null, double minWidth = 400, double minHeight = 600)
        {
            Window w = new Window();
            w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            w.Content = content;
            w.MinHeight = minHeight;
            w.MinWidth = minWidth;
            w.Height = minHeight;
            w.Width = minWidth;
            w.Name = name;
            w.Title = title;
            w.Topmost = isTopMost;
            //w.Icon = Variables.myMainWindow.Icon;

            return w;
        }
        public static Window GetNewWindow_NoIcon(bool isTopMost = true, string title = "", string name = "", object content = null, double minWidth = 400, double minHeight = 600)
        {
            Window w = new Window();
            w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            w.Content = content;
            w.MinHeight = minHeight;
            w.MinWidth = minWidth;
            w.Height = minHeight;
            w.Width = minWidth;
            w.Name = name;
            w.Title = title;
            w.Topmost = isTopMost;

            return w;
        }
        #endregion
        #region Find Object / Windows
        public static T FindChild<T>(DependencyObject depObj, string childName) where T : DependencyObject
        {
            // Confirm obj is valid. 
            if (depObj == null) return null;

            // success case
            if (depObj is T && ((FrameworkElement)depObj).Name == childName)
                return depObj as T;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                //DFS
                T obj = FindChild<T>(child, childName);

                if (obj != null)
                    return obj;
            }

            return null;
        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        public static Window GetParentWindow(DependencyObject child)
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;
            Window parent = parentObject as Window;
            if (parent != null)
                return parent;
            else
                return GetParentWindow(parentObject);
        }
        public static T FindControl<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindControl<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
        #endregion
        #region ComboBox Controls
        public static ComboBox Months(ComboBox myCB)
        {
            myCB.Items.Add(new KeyValuePair<int, string>(1, "January"));
            myCB.Items.Add(new KeyValuePair<int, string>(2, "February"));
            myCB.Items.Add(new KeyValuePair<int, string>(3, "March"));
            myCB.Items.Add(new KeyValuePair<int, string>(4, "April"));
            myCB.Items.Add(new KeyValuePair<int, string>(5, "May"));
            myCB.Items.Add(new KeyValuePair<int, string>(6, "June"));
            myCB.Items.Add(new KeyValuePair<int, string>(7, "July"));
            myCB.Items.Add(new KeyValuePair<int, string>(8, "August"));
            myCB.Items.Add(new KeyValuePair<int, string>(9, "September"));
            myCB.Items.Add(new KeyValuePair<int, string>(10, "October"));
            myCB.Items.Add(new KeyValuePair<int, string>(11, "November"));
            myCB.Items.Add(new KeyValuePair<int, string>(12, "December"));
            return myCB;
        }

        public static ComboBox Years(ComboBox myCB, int futureYears = 20, int previousYears = 0)
        {
            for (int i = 0; i < previousYears; i++)
            {
                int year = DateTime.Now.Year - i;
                myCB.Items.Add(new KeyValuePair<int, string>(year, year.ToString()));
            }

            for (int i = 0; i <= futureYears; i++)
            {
                int year = DateTime.Now.Year + i;
                myCB.Items.Add(new KeyValuePair<int, string>(year,year.ToString()));
            }
            return myCB;
        }
        #endregion
        #region Extension Methods
        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            MemberExpression body = (MemberExpression)expression.Body;
            return body.Member.Name;
        }
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
        #endregion
    }
}
