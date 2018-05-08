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
    /// Interaction logic for UADExport.xaml
    /// </summary>
    public partial class UADExport : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAD_WS.Interface.IDatabases> dbWorker = FrameworkServices.ServiceClient.UAD_DatabasesClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.ITable> tWorker = FrameworkServices.ServiceClient.UAD_TableClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IPubCodes> pcWorker = FrameworkServices.ServiceClient.UAD_PubCodesClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Databases>> svDatabases = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Databases>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Object.Table>> svTables = new FrameworkUAS.Service.Response<List<FrameworkUAD.Object.Table>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Object.PubCode>> svPubCodes = new FrameworkUAS.Service.Response<List<FrameworkUAD.Object.PubCode>>();

        List<FrameworkUAD.Entity.Databases> allDatabases = new List<FrameworkUAD.Entity.Databases>();
        List<FrameworkUAD.Object.Table> allTables = new List<FrameworkUAD.Object.Table>();
        List<FrameworkUAD.Object.PubCode> allPubCodes = new List<FrameworkUAD.Object.PubCode>();

        FrameworkUAD.Entity.Databases currentDatabase = new FrameworkUAD.Entity.Databases();
        FrameworkUAD.Object.Table currentTable = new FrameworkUAD.Object.Table();
        FrameworkUAD.Object.PubCode currentPubCode = new FrameworkUAD.Object.PubCode();
        #endregion

        public UADExport()
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

            List<FrameworkUAD.Entity.Databases> allowed = new List<FrameworkUAD.Entity.Databases>();

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svDatabases = dbWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);                
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (svDatabases != null && svDatabases.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allDatabases = svDatabases.Result;
                    foreach (FrameworkUAD.Entity.Databases database in allDatabases.OrderBy(x => x.DatabaseName))
                    {
                        //foreach (var c in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient)
                        //{
                        if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientLiveDBConnectionString.ToLower().Contains(database.DatabaseName.ToLower()))
                        {
                            allowed.Add(database);
                            break;
                        }
                        //}
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }

                cbDB.ItemsSource = allowed.OrderBy(x => x.DatabaseName).ToList();
                cbDB.DisplayMemberPath = "DatabaseName";
                cbDB.SelectedValuePath = "DatabaseName";

                radBusy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }        

        private void cbDB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbDB.SelectedValue != null)
            {
                string dbName = cbDB.SelectedValue.ToString();

                radBusy.IsBusy = true;
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (o, ea) =>
                {
                    svTables = tWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, dbName);
                    svPubCodes = pcWorker.Proxy.SelectAllPubs(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, dbName);
                };
                bw.RunWorkerCompleted += (o, ea) =>
                {
                    #region Tables
                    if (svTables != null && svTables.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
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
                    #region PubCodes
                    if (svPubCodes != null && svPubCodes.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        allPubCodes = svPubCodes.Result;
                        cbPubCode.ItemsSource = allPubCodes.OrderBy(x => x.Pubcode);
                        cbPubCode.SelectedValuePath = "Pubcode";
                        cbPubCode.DisplayMemberPath = "Pubcode";
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
            else
            {
                Core_AMS.Utilities.WPF.Message("No database was selected. Please select a database.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();
            string fileName = "";
            FileFunctions ff = new FileFunctions();

            string dbName = "";
            string table = "";
            string pubCode = "";

            if (cbDB.SelectedValue != null)
                dbName = cbDB.SelectedValue.ToString();

            if (cbTable.SelectedValue != null)
                table = cbTable.SelectedValue.ToString();

            if (cbPubCode.SelectedValue != null)
                pubCode = cbPubCode.SelectedValue.ToString();

            if (!String.IsNullOrEmpty(dbName) && !String.IsNullOrEmpty(table))
            {
                FrameworkUAS.Service.Response<DataTable> svDataTable = new FrameworkUAS.Service.Response<DataTable>();
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (o, ea) =>
                {
                    //Grab data into datatable                      
                    svDataTable = tWorker.Proxy.SelectDataTable(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, dbName, table, pubCode);                    
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
                Core_AMS.Utilities.WPF.Message("No database, table, and/or pubcode were selected. Please select.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
        }
    }
}
