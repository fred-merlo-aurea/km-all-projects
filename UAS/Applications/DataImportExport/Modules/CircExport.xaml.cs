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
    /// Interaction logic for CircExport.xaml
    /// </summary>
    public partial class CircExport : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> publisherWorker = FrameworkServices.ServiceClient.UAS_ClientClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> publicationWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IBatch> batchWorker = FrameworkServices.ServiceClient.UAD_BatchClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userWorker = FrameworkServices.ServiceClient.UAS_UserClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<KMPlatform.Entity.Client> svPublisher = new FrameworkUAS.Service.Response<KMPlatform.Entity.Client>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svPublication = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Batch>> svBatch = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Batch>>();

        List<KMPlatform.Entity.Client> allPublishers = new List<KMPlatform.Entity.Client>();
        List<KMPlatform.Object.Product> allPublications = new List<KMPlatform.Object.Product>();
        List<FrameworkUAD.Entity.Batch> allBatches = new List<FrameworkUAD.Entity.Batch>();

        KMPlatform.Entity.Client currentPublisher = new KMPlatform.Entity.Client();
        KMPlatform.Object.Product currentPublication = new KMPlatform.Object.Product();
        #endregion

        public CircExport()
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();            
            LoadData();
        }

        private void LoadData()
        {
            radBusy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                //svPublisher = publisherWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
                //svPublication = publicationWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient);
                svBatch = batchWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);

            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Load Publishers
                allPublishers = new List<KMPlatform.Entity.Client>();
                allPublishers.AddRange(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients);

                allPublishers.Add(svPublisher.Result);
                cbPublisher.ItemsSource = allPublishers;
                cbPublisher.SelectedValuePath = "ClientID";
                cbPublisher.DisplayMemberPath = "DisplayName";
   
                //if (svPublisher.Result != null && svPublisher.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //{
                //    allPublishers.Add(svPublisher.Result);
                //    cbPublisher.ItemsSource = allPublishers;
                //    cbPublisher.SelectedValuePath = "PublisherID";
                //    cbPublisher.DisplayMemberPath = "PublisherName";
                //}
                //else
                //{
                //    Core_AMS.Utilities.WPF.MessageServiceError();
                //}
                #endregion
                #region Load Publications
                allPublications = new List<KMPlatform.Object.Product>();
                foreach (var cp in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients)
                    allPublications.AddRange(cp.Products.Where(x=> x.IsCirc == true));

                //if (svPublication.Result != null && svPublication.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //{
                //    allPublications = svPublication.Result;
                //}
                //else
                //{
                //    Core_AMS.Utilities.WPF.MessageServiceError();
                //}
                #endregion
                #region Load Batches
                if (svBatch.Result != null && svBatch.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allBatches = svBatch.Result;
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

        private void cbPublisher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPublisher.SelectedValue != null)
            {
                int pubID = 0;
                int.TryParse(cbPublisher.SelectedValue.ToString(), out pubID);
                currentPublisher = allPublishers.FirstOrDefault(x => x.ClientID == pubID);

                // For now we will have to do this until all publications are loaded at the beginning of log in. DONE - Q.K. 04062015
                //svPublication = publicationWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, currentPublisher);
                //if (svPublication.Result != null && svPublication.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //    allPublications = svPublication.Result;
                //else
                //    Core_AMS.Utilities.WPF.MessageServiceError();

                cbPublication.ItemsSource = null;
                cbPublication.ItemsSource = allPublications.Where(x => x.ProductID == currentPublisher.ClientID && x.AllowDataEntry == true);
                cbPublication.SelectedValuePath = "ProductID";
                cbPublication.DisplayMemberPath = "ProductName";
            }
        }

        private void cbPublication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPublication.SelectedValue != null)
            {
                int pubID = 0;
                int.TryParse(cbPublication.SelectedValue.ToString(), out pubID);
                currentPublication = allPublications.FirstOrDefault(x => x.ProductID == pubID);
            }
        }

        private void btnLock_Click(object sender, RoutedEventArgs e)
        {            
            //Check if Batch opened before locking and Exporting
            if (currentPublisher != null && currentPublication != null)
            {
                List<FrameworkUAD.Entity.Batch> batches = allBatches.Where(x => x.PublicationID == currentPublication.ProductID && x.IsActive == true).ToList();
                if (batches.Count > 0)
                {
                    //Display User that have open batch.
                    List<int> batchUsers = (from x in batches where x.PublicationID == currentPublication.ProductID && x.IsActive == true select x.UserID).ToList();                    
                    FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>> svUser = userWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.ClientGroupID);
                    if (svUser != null && svUser.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        List<KMPlatform.Entity.User> users = svUser.Result.Where(x => batchUsers.Contains(x.UserID)).ToList();
                        string userNames = String.Join(", ", users.Select(x => x.FirstName + " " + x.LastName));
                        Core_AMS.Utilities.WPF.Message("User(s): " + userNames + " currently have an open batch on this Publication that needs to be closed before export.", MessageBoxButton.OK, MessageBoxImage.Information, "Action Not Available");
                    }
                    else
                    {
                        Core_AMS.Utilities.WPF.Message("Currently one or more users have an open batch on this Publication that needs to be closed before export. Users could not be specified.", MessageBoxButton.OK, MessageBoxImage.Information, "Action Not Available");
                    }
                }
                else
                {
                    //FrameworkUAD.Entity.Product pub = allPublications.FirstOrDefault(x => x.ProductID == currentPublication.ProductID);
                    FrameworkUAD.Entity.Product pub = new FrameworkUAD.Entity.Product();
                    FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
                    FrameworkUAS.Service.Response<FrameworkUAD.Entity.Product> productResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.Product>();
                    productResponse = productWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, currentPublication.ProductID, currentPublisher.ClientConnections);
                    if (productResponse.Result != null && productResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                        pub = productResponse.Result;
                    
                    if (pub.AllowDataEntry == false)
                    {
                        Core_AMS.Utilities.WPF.Message("Publication is already being exported. If this is incorrect please contact customer service.", MessageBoxButton.OK, MessageBoxImage.Information, "Action Not Available");
                        return;
                    }
                    currentPublication.AllowDataEntry = false;
                    //currentPublication.DateUpdated = DateTime.Now;
                    //currentPublication.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    pub.AllowDataEntry = false;
                    pub.DateUpdated = DateTime.Now;
                    pub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    FrameworkUAS.Service.Response<int> svSave = publicationWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, pub, currentPublisher.ClientConnections);
                    if (svSave != null && svSave.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        Core_AMS.Utilities.WPF.Message("Publication has been locked.", MessageBoxButton.OK, MessageBoxImage.Information, "Action Successful");
                        btnExport.IsEnabled = true;
                    }
                    else
                    {
                        Core_AMS.Utilities.WPF.Message("Publication failed to lock.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Action Unsuccessful");
                        btnExport.IsEnabled = true;
                    }
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Publisher and/or Publication not selected. Please do so before you continue.", MessageBoxButton.OK, MessageBoxImage.Information, "Missing Data");
                return;
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();
            string fileName = "";
            FileFunctions ff = new FileFunctions();

            if (currentPublisher != null && currentPublication != null)
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (o, ea) =>
                {
                    //Grab data into datatable based on Publisher and Publication                 
                    dt = FrameworkUAD.DataAccess.CircImportExport.SelectDataTable(currentPublisher.ClientID, currentPublication.ProductID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    fileName = Core.ADMS.BaseDirs.getExportDataCircDir() + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HHmmssffff") + "_CIRC.csv";
                    ff.CreateCSVFromDataTable(dt, fileName);
                    };
                bw.RunWorkerCompleted += (o, ea) =>
                {
                    lbOutput.Content = "File Save Location: " + fileName;
                };
                bw.RunWorkerAsync();
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("No publisher and/or publication were selected. Please select.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
        }
        
    }
}
