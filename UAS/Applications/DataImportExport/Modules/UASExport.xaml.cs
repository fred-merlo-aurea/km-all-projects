using Core_AMS.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DataImportExport.Modules
{
    /// <summary>
    /// Interaction logic for UASExport.xaml
    /// </summary>
    public partial class UASExport : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAS_WS.Interface.ISourceFile> sfWorker = FrameworkServices.ServiceClient.UAS_SourceFileClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITable> tWorker = FrameworkServices.ServiceClient.UAS_TableClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.SourceFile>> svFiles = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.SourceFile>>();
        FrameworkUAS.Service.Response<List<FrameworkUAS.Object.Table>> svTables = new FrameworkUAS.Service.Response<List<FrameworkUAS.Object.Table>>();

        List<FrameworkUAS.Entity.SourceFile> allSourceFiles = new List<FrameworkUAS.Entity.SourceFile>();
        List<FrameworkUAS.Object.Table> allTables = new List<FrameworkUAS.Object.Table>();

        FrameworkUAS.Entity.SourceFile currentSourceFile = new FrameworkUAS.Entity.SourceFile();
        FrameworkUAS.Object.Table currentTable = new FrameworkUAS.Object.Table();
        #endregion

        public UASExport()
        {
            InitializeComponent();
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            LoadData();
        }

        private void LoadData()
        {
            radBusy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svFiles = sfWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID, false, false);
                svTables = tWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Load Files
                if (svFiles.Result != null && svFiles.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allSourceFiles = svFiles.Result;
                    cbFile.ItemsSource = allSourceFiles.OrderBy(x => x.FileName);
                    cbFile.SelectedValuePath = "SourceFileID";
                    cbFile.DisplayMemberPath = "FileName";
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }
                #endregion

                #region Load Database
                Dictionary<string, string> db = new Dictionary<string, string>() { { "UAS", "UAS" } };
                cbDB.ItemsSource = db;
                cbDB.DisplayMemberPath = "Key";
                cbDB.SelectedValuePath = "Value";
                cbDB.SelectedItem = db["UAS"];
                #endregion

                #region Load Tables
                if (svTables.Result != null && svTables.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allTables = svTables.Result;
                    cbTable.ItemsSource = allTables.OrderBy(x => x.TableName);
                    cbTable.SelectedValuePath = "TableName";
                    cbTable.DisplayMemberPath = "TableName";
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }
                #endregion

                radBusy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }

        private void cbTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();
            string fileName = "";
            FileFunctions ff = new FileFunctions();

            string table = "";
            int sourceFileID = 0;

            if (cbTable.SelectedValue != null)
                table = cbTable.SelectedValue.ToString();

            if (cbFile.SelectedValue != null)
                int.TryParse(cbFile.SelectedValue.ToString(), out sourceFileID);

            if (!string.IsNullOrEmpty(table))
            {
                FrameworkUAS.Service.Response<DataTable> svDataTable = new FrameworkUAS.Service.Response<DataTable>();
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (o, ea) =>
                {
                    //Grab data into datatable    
                    svDataTable = tWorker.Proxy.SelectDataTable(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, table, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID, sourceFileID);                    
                };
                bw.RunWorkerCompleted += (o, ea) =>
                {
                    if (svDataTable != null && svDataTable.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        dt = svDataTable.Result;
                        fileName = Core.ADMS.BaseDirs.getExportDataUASDir() + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HHmmssffff") + "_UAS.csv";
                        ff.CreateCSVFromDataTable(dt, fileName);
                        lbOutput.Content = "File Save Location: " + fileName;
                    }
                    else
                    {
                        Core_AMS.Utilities.WPF.Message("Error occurred exporting data. Please contact customer service if the issue persists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Export Error");
                        return;
                    }                    
                };
                bw.RunWorkerAsync();
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("No table and/or source file were selected. Please select.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
        }
    }
}
