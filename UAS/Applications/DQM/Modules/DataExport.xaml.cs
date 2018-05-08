using Core_AMS.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using FrameworkUAS.Object;

namespace DQM.Modules
{
    /// <summary>
    /// Interaction logic for DataExport.xaml
    /// </summary>
    public partial class DataExport : UserControl
    {
        private KMPlatform.Entity.Client myPublisher { get; set; }
        private FrameworkUAD.Entity.Product myPublication { get; set; }
        private List<KMPlatform.Entity.Client> Client;

        public DataExport(bool isCirc = false)
        {
            InitializeComponent();

            ModifySetup(isCirc);
            LoadPublishers();
            LoadClients();
            //LoadDB();
            //LoadTables();
            //LoadPubCodes();
        }

        #region ComboLoaders
        private void LoadPublishers()
        {
            FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> worker = FrameworkServices.ServiceClient.UAS_ClientClient();
            Client = worker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result.ToList();
            var pubs = from p in Client
                       join c in AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients on p.ClientID equals c.ClientID
                       select p;

            cbPublisher.ItemsSource = pubs;
            cbPublisher.DisplayMemberPath = "DisplayName";
            cbPublisher.SelectedValuePath = "ClientID";
        }
        private void LoadPublication(int publisherID)
        {
            if (publisherID > 0)
            {
                KMPlatform.Entity.Client c = Client.SingleOrDefault(p => p.ClientID == publisherID);
                FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> worker = FrameworkServices.ServiceClient.UAD_ProductClient();
                cbPublication.IsEnabled = true;
                cbPublication.ItemsSource = worker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, c.ClientConnections).Result.Where(x => x.AllowDataEntry == true).ToList();
                cbPublication.DisplayMemberPath = "PubCode";
                cbPublication.SelectedValuePath = "PubID";
            }
        }
        private void LoadClients()
        {
            cbClient.ItemsSource = AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients;
            cbClient.DisplayMemberPath = "FtpFolder";
            cbClient.SelectedValuePath = "ClientID"; 
        }
        private void LoadClientFiles(int ClientID)
        {
            if (ClientID > 0)
            {

                List<FrameworkUAS.Entity.SourceFile> Files = AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[clientID].SourceFilesList;//s.Select(ClientID, false).ToList();

                cbFile.ItemsSource = Files;
                cbFile.DisplayMemberPath = "FileName";
                cbFile.SelectedValuePath = "SourceFileID";
            }
        }
        private void LoadPubCodes()
        {
            if (cbDB.SelectedValue.ToString() != null && cbDB.SelectedValue.ToString() != "-1")
            {
                string dbName = cbDB.SelectedValue.ToString();
                FrameworkServices.ServiceClient<UAD_WS.Interface.IPubCodes> pc = FrameworkServices.ServiceClient.UAD_PubCodesClient();
                cbPubCode.ItemsSource = pc.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, dbName).Result.OrderBy(x => x.Pubcode).ToList();
                cbPubCode.DisplayMemberPath = "PubCode";
                cbPubCode.SelectedValuePath = "PubCode";
            }
        }
        private void LoadDB()
        {
            if (rbUAD.IsChecked == true)
            {
                FrameworkServices.ServiceClient<UAD_WS.Interface.IDatabases> db = FrameworkServices.ServiceClient.UAD_DatabasesClient();

                List<FrameworkUAD.Entity.Databases> allowed = new List<FrameworkUAD.Entity.Databases>();

                foreach (FrameworkUAD.Entity.Databases database in db.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result.OrderBy(x => x.DatabaseName))
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
            else if (rbUAS.IsChecked == true)
            {
                Dictionary<string, string> db = new Dictionary<string, string>() {{"UAS","UAS"}};
                cbDB.ItemsSource = db;
                cbDB.DisplayMemberPath = "Key";
                cbDB.SelectedValuePath = "Value";
                cbDB.SelectedIndex = 0;// = "UAS";
            }
        }
        private void LoadTables(string dbName)
        {
            if (rbUAD.IsChecked == true)
            {
                if (rbTable.IsChecked == true)
                {
                    FrameworkServices.ServiceClient<UAD_WS.Interface.ITable> t = FrameworkServices.ServiceClient.UAD_TableClient();
                    cbTable.ItemsSource = t.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, dbName).Result.OrderBy(x => x.TableName).ToList();
                    cbTable.DisplayMemberPath = "TableName";
                    cbTable.SelectedValuePath = "TableName";
                }
                else if (rbReport.IsChecked == true)
                {
                    
                }
            }
            else if (rbUAS.IsChecked == true)
            {
                if (rbTable.IsChecked == true)
                {
                    FrameworkServices.ServiceClient<UAS_WS.Interface.ITable> t = FrameworkServices.ServiceClient.UAS_TableClient();
                    cbTable.ItemsSource = t.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result.OrderBy(x => x.TableName).ToList();
                    cbTable.DisplayMemberPath = "TableName";
                    cbTable.SelectedValuePath = "TableName";
                }
                else if (rbReport.IsChecked == true)
                {
                    
                }
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
                spOption.Visibility = Visibility.Collapsed;
                spDB.Visibility = Visibility.Hidden;
                spTable.Visibility = Visibility.Hidden;
                spClient.Visibility = Visibility.Hidden;
                cbClient.IsEnabled = false;
                spFile.Visibility = Visibility.Hidden;
                cbFile.IsEnabled = false;
                spPubCode.Visibility = Visibility.Hidden;
                cbPubCode.IsEnabled = false;
                spPublisher.Visibility = Visibility.Visible;
                spPublication.Visibility = Visibility.Visible;
                btnLock.Visibility = Visibility.Hidden;
                cbPublication.IsEnabled = false;
                btnLock.IsEnabled = false;
                btnExport.Visibility = Visibility.Hidden;
                btnExport.IsEnabled = false;
                if (onlyCirc)
                {
                    rbUAD.Visibility = Visibility.Hidden;
                    rbUAS.Visibility = Visibility.Hidden;
                    rbUAD.IsEnabled = false;
                    rbUAS.IsEnabled = false;
                }
            }
            else
            {                
                spOption.Visibility = Visibility.Visible;
                spDB.Visibility = Visibility.Visible;
                spTable.Visibility = Visibility.Visible;
                spPublisher.Visibility = Visibility.Hidden;
                spPublication.Visibility = Visibility.Hidden;
                btnLock.Visibility = Visibility.Hidden;
                spClient.Visibility = Visibility.Hidden;
                cbClient.IsEnabled = false;
                spFile.Visibility = Visibility.Hidden;
                cbFile.IsEnabled = false;
                spPubCode.Visibility = Visibility.Hidden;
                cbPubCode.IsEnabled = false;
                rbReport.IsEnabled = false;
                rbTable.IsEnabled = false;
                cbDB.IsEnabled = false;
                cbTable.IsEnabled = false;
                btnExport.Visibility = Visibility.Hidden;                
            }
        }
        public void ClearControls()
        {
            rbReport.IsChecked = false;
            rbTable.IsChecked = false;
            cbDB.ItemsSource = null;
            cbDB.SelectedValue = -1;
            cbTable.ItemsSource = null;
            cbTable.SelectedValue = -1;
            cbPublisher.SelectedValue = -1;
            cbPublication.SelectedValue = -1;
            cbClient.SelectedValue = -1;
            cbFile.ItemsSource = null;
            cbFile.SelectedValue = -1;
            cbPubCode.ItemsSource = null;
            cbPubCode.SelectedValue = -1;
            lbOutput.Content = "";
        }
        public void EnableOption(bool enable)
        {
            rbReport.IsEnabled = enable;
            rbTable.IsEnabled = enable;
        }
        public void EnableComboBox(bool enable, Telerik.Windows.Controls.RadRibbonComboBox cb)
        {
            cb.IsEnabled = enable;
            if (enable)
                cb.Visibility = Visibility.Visible;
            else
                cb.Visibility = Visibility.Hidden;
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
                    ShowStackPanel(true, spPubCode);                    
                    break;
                case "rbUAS":
                    ModifySetup(false);
                    ShowStackPanel(true, spClient);                    
                    break;
            }
            EnableOption(true);
        }
        private void RadRadioButtonOption_Checked(object sender, RoutedEventArgs e)
        {
            EnableOption(true);
            Telerik.Windows.Controls.RadRibbonRadioButton rb = (Telerik.Windows.Controls.RadRibbonRadioButton)sender;
            rb.IsChecked = true;
            switch (rb.Name)
            {
                case "rbReport":
                    EnableComboBox(true, cbDB);
                    LoadDB();
                    break;
                case "rbTable":
                    EnableComboBox(true, cbDB);
                    LoadDB();
                    break;               
            }
        }

        private void cbDB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableComboBox(true, cbTable);                        
            if (cbDB.SelectedValue != null && cbDB.SelectedValue.ToString() != "-1")
                LoadTables(cbDB.SelectedValue.ToString());
            
        }
        private void cbTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rbUAD.IsChecked == true)
            {
                EnableComboBox(true, cbPubCode);
                LoadPubCodes();
                EnableButton(true, btnExport);
            }
            else if (rbUAS.IsChecked == true)
            {
                EnableComboBox(true, cbClient);
            }
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
            myPublisher = (KMPlatform.Entity.Client)cbPublisher.SelectedItem;
            myPublication = (FrameworkUAD.Entity.Product)cbPublication.SelectedItem;

            EnableButton(true, btnLock);
        }
        private void cbClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rbUAS.IsChecked == true)
            {
                EnableComboBox(true, cbFile);
                ShowStackPanel(true, spFile);
            }
            else if (rbUAD.IsChecked == true)
            {
                EnableComboBox(true, cbPubCode);
                ShowStackPanel(true, spPubCode);
            }
            EnableButton(true, btnExport);
            int ClientID = 0;
            int.TryParse(cbClient.SelectedValue.ToString(), out ClientID);
            if (ClientID > 0)
                LoadClientFiles(ClientID);
        }
        private void cbFile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        private void cbPubCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void btnLock_Click(object sender, RoutedEventArgs e)
        {
            FrameworkServices.ServiceClient<UAD_WS.Interface.IBatch> batchWorker = FrameworkServices.ServiceClient.UAD_BatchClient();
            //Check if Batch opened before locking and Exporting
            List<FrameworkUAD.Entity.Batch> batches = batchWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result.Where(x => x.PublicationID == myPublication.PubID && x.IsActive == true).ToList();
            if (batches.Count > 0)
            {
                //Display User that have open batch.
                List<int> batchUsers = (from x in batches where x.PublicationID == myPublication.PubID && x.IsActive == true select x.UserID).ToList();
                FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> u = FrameworkServices.ServiceClient.UAS_UserClient();
                List<KMPlatform.Entity.User> users = u.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.ClientGroupID).Result.Where(x => batchUsers.Contains(x.UserID)).ToList();
                string userNames = String.Join(", ", users.Select(x => x.FirstName + " " + x.LastName));
                Core_AMS.Utilities.WPF.Message("User(s): " + userNames + " currently have an open batch on this Publication that needs to be closed before export.", MessageBoxButton.OK, MessageBoxImage.Information, "Action Not Available");
            }
            else
            {
                FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> worker = FrameworkServices.ServiceClient.UAD_ProductClient();
                FrameworkUAD.Entity.Product pub = worker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myPublication.PubID, myPublisher.ClientConnections).Result;
                if (pub.AllowDataEntry == false)
                {
                    Core_AMS.Utilities.WPF.Message("Publication is already being exported.", MessageBoxButton.OK, MessageBoxImage.Information, "Action Not Available");
                    return;
                }
                myPublication.AllowDataEntry = false;
                myPublication.DateUpdated = DateTime.Now;
                myPublication.UpdatedByUserID = AppData.myAppData.AuthorizedUser.User.UserID;
                worker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myPublication, myPublisher.ClientConnections);
                EnableButton(true, btnExport);
            }
        }
        public void DoWork()
        {
            
        }

        public void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            radBusy.IsBusy = true;
            Thread t = new Thread(new ThreadStart(DoWork));
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.IsBackground = true;
            t.Start();


            lbOutput.Content = "File Save Location: " + fileName;
            radBusy.IsBusy = false;

            t.Abort();
        }

        public void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            dt = new DataTable();
            FileFunctions ff = new FileFunctions();
            FrameworkServices.ServiceClient<UAD_WS.Interface.ICircImportExport> cieWrk = FrameworkServices.ServiceClient.UAD_CircImportExportClient();
            
            //if (rbCirc.IsChecked == true)
            if(rbSelected == "rbCirc")
            {
                //Grab data into datatable based on Publisher and Publication    
                FrameworkUAS.Service.Response<DataTable> dtResp = cieWrk.Proxy.SelectDataTable(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myPublisher.ClientID, myPublication.PubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                if(dtResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success && dtResp.Result != null)
                {
                    dt = dtResp.Result;
                    //dt = FrameworkUAD.DataAccess.CircImportExport.SelectDataTable(myPublisher.ClientID, myPublication.PubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    fileName = Core.ADMS.BaseDirs.getExportDataCircDir() + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HHmmssffff") + "_CIRC.csv";
                    ff.CreateCSVFromDataTable(dt, fileName);
                }
            }
            //else if (rbUAD.IsChecked == true)
            else if (rbSelected == "rbUAD")
            {
                //Grab data into datatable based on DB, Table, Opt PubCode  
                dt = FrameworkUAD.DataAccess.Table.Select(dbName, table, pubCode);
                fileName = Core.ADMS.BaseDirs.getExportDataUADDir() + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HHmmssffff") + "_UAD.csv";
                ff.CreateCSVFromDataTable(dt, fileName);
            }
            //else if (rbUAS.IsChecked == true)
            else if (rbSelected == "rbUAS")
            {
                //Grab data into datatable based on DB, Table, Client, Opt File                                
                dt = FrameworkUAS.DataAccess.Table.Select(table, clientID, sourceFileID);
                fileName = Core.ADMS.BaseDirs.getExportDataUASDir() + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HHmmssffff") + "_UAS.csv";
                ff.CreateCSVFromDataTable(dt, fileName);
            }            
        }
        public DataTable dt;
        public string fileName;
        public string rbSelected;
        int clientID;
        int sourceFileID;
        string dbName;
        string pubCode;
        string table;
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            radBusy.IsBusy = true;
            radBusy.BusyContent = "Loading Data Into File";
            dbName = "";
            table = "";
            pubCode = "";
            sourceFileID = 0;
            clientID = 0;
            if (rbCirc.IsChecked == true)
            {
                rbSelected = "rbCirc";
            }
            else if (rbUAD.IsChecked == true)
            {
                rbSelected = "rbUAD";
                dbName = cbDB.SelectedValue.ToString();
                table = cbTable.SelectedValue.ToString();
                if (cbPubCode.SelectedValue != null)
                    pubCode = cbPubCode.SelectedValue.ToString();
            }
            else if (rbUAS.IsChecked == true)
            {
                rbSelected = "rbUAS";
                int.TryParse(cbClient.SelectedValue.ToString(), out clientID);
                table = cbTable.SelectedValue.ToString();
                if (cbFile.SelectedValue != null)
                    int.TryParse(cbFile.SelectedValue.ToString(), out sourceFileID);
            }

            BackgroundWorker w = new BackgroundWorker();
            w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            w.DoWork += new DoWorkEventHandler(worker_DoWork);
            w.RunWorkerAsync();                        
        }
        
        #endregion 
    }
}
