using Core_AMS.Utilities;
using FrameworkUAS.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using FrameworkServices;
using FrameworkUAD.Object;
using FrameworkUAS.Object;
using KM.Common.Import;
using UAD_WS.Interface;

namespace DQM.Modules
{
    /// <summary>
    /// Interaction logic for DataImport.xaml
    /// </summary>
    public partial class DataImport : UserControl
    {
        private const int FileRowBatch = 2500;
        private const string CommaDelimiter = "comma";

        private string FileName { get; set; }
        private Guid accessKey { get; set; }

        //
        // READ ME!!!!!
        // As of 3-24-2015 Jason said this one will be going away and will not be used any more - Q.K.
        //


        public DataImport(bool isCirc = false)
        {
            InitializeComponent();
            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;

            ModifySetup(isCirc);
            LoadPublishers();
        }

        #region ComboLoaders
        private void LoadPublishers()
        {

        }
        private void LoadPublication(int publisherID)
        {

        }
        private void LoadDB()
        {
            if (rbUAD.IsChecked == true)
            {
                FrameworkServices.ServiceClient<UAD_WS.Interface.IDatabases> db = FrameworkServices.ServiceClient.UAD_DatabasesClient();
                List<FrameworkUAD.Entity.Databases> allowed = new List<FrameworkUAD.Entity.Databases>();

                foreach (FrameworkUAD.Entity.Databases database in db.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result.OrderBy(x => x.DatabaseName))
                {
                    foreach (var c in AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients)
                    {
                        if (c.ClientLiveDBConnectionString.ToLower().Contains(database.DatabaseName.ToLower()))
                        {
                            allowed.Add(database);
                            break;
                        }
                    }
                }

                cbDB.ItemsSource = allowed.OrderBy(x => x.DatabaseName).ToList();
                cbDB.DisplayMemberPath = "DatabaseName";
                cbDB.SelectedValuePath = "DatabaseName";
            }
        }
        private void LoadTables(string dbName)
        {
            if (rbUAD.IsChecked == true)
            {
                FrameworkServices.ServiceClient<UAD_WS.Interface.ITable> t = FrameworkServices.ServiceClient.UAD_TableClient();
                cbTable.ItemsSource = t.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, dbName).Result.OrderBy(x => x.TableName).ToList();
                cbTable.DisplayMemberPath = "TableName";
                cbTable.SelectedValuePath = "TableName";
            }
        }
        #endregion

        #region ModuleChanges
        public void ModifySetup(bool isCirc, bool onlyCirc = true)
        {
            ClearControls();
            if (isCirc)
            {
                rbCirc.IsChecked = true;                
                spDB.Visibility = Visibility.Hidden;
                spTable.Visibility = Visibility.Hidden;                
                spPublisher.Visibility = Visibility.Visible;
                spPublication.Visibility = Visibility.Visible;
                cbPublication.IsEnabled = false;
                if (onlyCirc)
                {
                    rbUAD.Visibility = Visibility.Hidden;
                    rbUAD.IsEnabled = false;
                }
            }
            else
            {
                spDB.Visibility = Visibility.Visible;
                spTable.Visibility = Visibility.Visible;
                spPublisher.Visibility = Visibility.Hidden;
                spPublication.Visibility = Visibility.Hidden;
                cbDB.IsEnabled = false;
                cbTable.IsEnabled = false;
            }
        }
        public void ClearControls()
        {
            cbDB.ItemsSource = null;
            cbDB.SelectedValue = -1;
            cbTable.ItemsSource = null;
            cbTable.SelectedValue = -1;
            cbPublisher.SelectedValue = -1;
            cbPublication.SelectedValue = -1;
            btnImport.Visibility = Visibility.Hidden;
            FileName = "";
        }
        public void EnableComboBox(bool enable, Telerik.Windows.Controls.RadRibbonComboBox cb)
        {
            cb.IsEnabled = enable;
        }
        public void ShowStackPanel(bool enable, StackPanel sp)
        {
            sp.Visibility = Visibility.Visible;
        }
        public void EnableButton(bool enable, Button btn)
        {
            btn.IsEnabled = enable;
            if (enable)
                btn.Visibility = Visibility.Visible;
            else
                btn.Visibility = Visibility.Hidden;
        }
        #endregion

        #region ControlEvents
        private void RadRadioButtonSystem_Checked(object sender, RoutedEventArgs e)
        {
            Telerik.Windows.Controls.RadRibbonRadioButton rb = (Telerik.Windows.Controls.RadRibbonRadioButton)sender;
            switch (rb.Name)
            {
                case "rbCirc":
                    ModifySetup(true, false);                    
                    break;
                case "rbUAD":
                    ModifySetup(false);
                    EnableComboBox(true, cbDB);
                    LoadDB();
                    break;
            }
        }
        private void cbDB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableComboBox(true, cbTable);
            if (cbDB.SelectedValue != null)
                LoadTables(cbDB.SelectedValue.ToString());

        }
        private void cbTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton(true, btnImport);
        }
        private void cbPublisher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int publisherID = 0;
            int.TryParse(cbPublisher.SelectedValue.ToString(), out publisherID);
            if (publisherID > 0)
                LoadPublication(publisherID);
        }
        private void cbPublication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            EnableButton(true, btnImport);
        }

        public void DoWork()
        {

        }

        public void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lbOutput.Content = "";
            radBusy.IsBusy = true;
            Thread t = new Thread(new ThreadStart(DoWork));
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.IsBackground = true;
            t.Start();

            FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> worker = FrameworkServices.ServiceClient.UAD_ProductClient();
            //Allow Data Entry on Pubcode after Import
            
            lbOutput.Content = "Import has finished.";
            radBusy.IsBusy = false;

            t.Abort();
        }

        public void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var fileTotalRowCount = 0;
            var fileRowProcessedCount = 0;

            FileConfiguration fileConfig;
            FileInfo fileInfo;
            if (!OpenFileAndEnsureDataExist(out fileConfig, out fileInfo, out fileTotalRowCount))
            {
                return;
            }

            var ivWorker = ServiceClient.UAD_ImportVesselClient();
            var dataIv = new ImportVessel();
            while (fileRowProcessedCount < fileTotalRowCount)
            {
                //Reading the whole file
                dataIv = ReadFileBatch(fileRowProcessedCount, ivWorker, fileInfo, fileConfig);
                fileRowProcessedCount += dataIv.TotalRowCount;
            }

            var cieList = new List<CircImportExport>();
            foreach (DataRow dr in dataIv.DataOriginal.Rows)
            {
                cieList.Add(ParseDataRow(dr));
            }

            var blcie = ServiceClient.UAD_CircImportExportClient();
            blcie.Proxy.SaveBulkSqlUpdate(
                accessKey,
                AppData.myAppData.AuthorizedUser.User.UserID,
                cieList,
                AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
        }

        private static CircImportExport ParseDataRow(DataRow dr)
        {
            var cieSolo = new CircImportExport();

            foreach (var prop in typeof(CircImportExport).GetProperties())
            {
                var columnName = prop.Name;
                if (!dr.Table.Columns.Contains(columnName))
                {
                    continue;
                }

                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(cieSolo, dr[columnName].ToString());
                }
                else if (prop.PropertyType == typeof(int))
                {
                    prop.SetValue(cieSolo, GetInt(dr, columnName));
                }
                else if (prop.PropertyType == typeof(float))
                {
                    prop.SetValue(cieSolo, GetFloat(dr, columnName));
                }
                else if (prop.PropertyType == typeof(bool))
                {
                    prop.SetValue(cieSolo, GetBool(dr, columnName));
                }
                else if (prop.PropertyType == typeof(DateTime))
                {
                    prop.SetValue(cieSolo, GetDate(dr, columnName));
                }
            }

            return cieSolo;
        }

        private static float GetFloat(DataRow dr, string columnName)
        {
            var floatValue = 0f;
            float.TryParse(dr[columnName].ToString(), out floatValue);
            return floatValue;
        }

        private static bool GetBool(DataRow dr, string columnName)
        {
            bool stat = false;
            Boolean.TryParse(dr[columnName].ToString(), out stat);
            return stat;
        }

        private static DateTime GetDate(DataRow dr, string columnName)
        {
            DateTime dateValue;
            DateTime.TryParse(dr[columnName].ToString(), out dateValue);
            return dateValue < new DateTime(1980, 1, 1) ? DateTime.Now : dateValue;
        }

        private static int GetInt(DataRow dr, string columnName)
        {
            int intValue = 0;
            int.TryParse(dr[columnName].ToString(), out intValue);
            return intValue;
        }

        private ImportVessel ReadFileBatch(
            int fileRowProcessedCount,
            ServiceClient<IImportVessel> ivWorker,
            FileInfo fileInfo,
            FileConfiguration fileConfig)
        {
            var startRow = fileRowProcessedCount + 1;

            //loads the data from the file
            return ivWorker.Proxy.GetImportVessel(accessKey, fileInfo, startRow, FileRowBatch, fileConfig).Result;
        }

        private bool OpenFileAndEnsureDataExist(
            out FileConfiguration fileConfig,
            out FileInfo fileInfo,
            out int fileTotalRowCount)
        {
            var fileWorker = new FileWorker();
            fileConfig = new FileConfiguration()
            {
                FileColumnDelimiter = CommaDelimiter,
                IsQuoteEncapsulated = true
            };
            fileInfo = new FileInfo(FileName);
            fileTotalRowCount = fileWorker.GetRowCount(fileInfo);
            return fileTotalRowCount != 0;
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            lbOutput.Content = "";
            radBusy.IsBusy = true;
            radBusy.BusyContent = "Uploading Data Into System";
            if (FileName != null && FileName != "")
            {
                //Run this in Background Worker
                BackgroundWorker w = new BackgroundWorker();
                w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                w.DoWork += new DoWorkEventHandler(worker_DoWork);
                w.RunWorkerAsync();                 
            }
            else
                Core_AMS.Utilities.WPF.Message("Please select a file to import.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Missing File.");
        }
        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Recognized Files(*.txt;*.csv;*.xls;*.xlsx)|*.txt;*.csv;*.xls;*.xlsx"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                FileName = dlg.FileName;
                tbFile.Text = FileName;
            }
        }
        #endregion
    }
}
