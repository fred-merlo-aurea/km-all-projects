using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls.CircCodesheet
{
    /// <summary>
    /// Interaction logic for CustomerService.xaml
    /// </summary>
    public partial class CustomerService : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> pWorker = FrameworkServices.ServiceClient.UAS_ClientClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> rWorker = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> svClient = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svProduct = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> svResponseGroup = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();

        List<KMPlatform.Entity.Client> allClients = new List<KMPlatform.Entity.Client>();
        List<KMPlatform.Object.Product> allProducts = new List<KMPlatform.Object.Product>();
        List<FrameworkUAD.Entity.ResponseGroup> allResponseGroup = new List<FrameworkUAD.Entity.ResponseGroup>();

        KMPlatform.Entity.Client currentClient = new KMPlatform.Entity.Client();
        KMPlatform.Object.Product currentProduct = new KMPlatform.Object.Product();
        FrameworkUAD.Entity.ResponseGroup currentResponseGroup = new FrameworkUAD.Entity.ResponseGroup();
        #endregion
        public CustomerService()
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                //svClient = pWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                //svProduct = pubWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                svResponseGroup = rWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Load Publishers
                //if (svClient.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //{                    
                    //allClients = svClient.Result;

                    allClients = new List<KMPlatform.Entity.Client>();
                    allClients.AddRange(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients);

                    cbPublisher.ItemsSource = null;
                    cbPublisher.ItemsSource = allClients.Where(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID).ToList();
                    cbPublisher.SelectedValuePath = "ClientID";
                    cbPublisher.DisplayMemberPath = "DisplayName";

                    if (cbPublisher.SelectedValue != null)
                    {
                        int PublisherID = 0;
                        int.TryParse(cbPublisher.SelectedValue.ToString(), out PublisherID);
                        currentClient = allClients.FirstOrDefault(x => x.ClientID == PublisherID);
                    }
                    else if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0)
                        currentClient = allClients.FirstOrDefault(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
                //}
                //else
                //{
                //    Core_AMS.Utilities.WPF.MessageServiceError();
                //}
                #endregion
                #region Load Publications

                    allProducts = new List<KMPlatform.Object.Product>();
                foreach (var cp in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients)
                    allProducts.AddRange(cp.Products);

                if (currentClient != null)
                {
                    cbPublisher.SelectedItem = allClients.FirstOrDefault(x => x.ClientID == currentClient.ClientID); 

                    cbMagazine.ItemsSource = null;
                    cbMagazine.ItemsSource = allProducts.Where(x => x.ClientID == currentClient.ClientID);
                    cbMagazine.SelectedValuePath = "ProductID";
                    cbMagazine.DisplayMemberPath = "ProductCode";

                    if (cbMagazine.SelectedValue != null)
                    {
                        int PublicationID = 0;
                        int.TryParse(cbMagazine.SelectedValue.ToString(), out PublicationID);
                        currentProduct = allProducts.FirstOrDefault(x => x.ProductID == PublicationID);
                    }
                }
                else if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0)
                {
                    currentClient = allClients.FirstOrDefault(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
                    cbPublisher.SelectedItem = allClients.FirstOrDefault(x => x.ClientID == currentClient.ClientID);                        
                }
                //}
                //else
                //{
                //    Core_AMS.Utilities.WPF.MessageServiceError();
                //}
                #endregion
                #region Load ResponseGroups
                if (svResponseGroup.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allResponseGroup = svResponseGroup.Result;
                    if (currentProduct != null)
                    {
                        cbMagazine.SelectedItem = currentProduct;

                        lbxAvailable.Items.Clear();
                        foreach (string rt in allResponseGroup.Select(x => x.DisplayName).Distinct())
                        {
                            lbxAvailable.Items.Add(rt);
                        }
                        lbxSelected.Items.Clear();
                        foreach (FrameworkUAD.Entity.ResponseGroup rt in allResponseGroup.Where(x => x.PubID == currentProduct.ProductID).ToList().OrderBy(y => y.DisplayOrder))
                        {
                            lbxSelected.Items.Add(rt.DisplayName);
                            if (lbxAvailable.Items.Contains(rt.DisplayName))
                                lbxAvailable.Items.Remove(rt.DisplayName);

                        } 
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }
                #endregion
                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //Change Button tag to zero content back to save
            btnSave.Tag = "0";
            btnSave.Content = "Save";

            lbxSelected.Items.Clear();

            LoadData();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (currentProduct == null)
            {
                Core_AMS.Utilities.WPF.Message("Magazine unknown. Please select a valid magazine before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Incomplete");
                return;
            }
            FrameworkUAS.Service.Response<bool> svDelete = new FrameworkUAS.Service.Response<bool>();
            //svDelete = rWorker.Proxy.DeletePublicationID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, currentProduct.PubID);
            //if (svDelete != null && svDelete.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            //{
            List<string> badSaves = new List<string>();
            int i = 1;
            foreach (string s in lbxSelected.Items)
            {
                FrameworkUAD.Entity.ResponseGroup rt = allResponseGroup.FirstOrDefault(x => x.DisplayName.Equals(s, StringComparison.CurrentCultureIgnoreCase));
                rt.DisplayOrder = i;
                rt.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                rt.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                rt.DateCreated = DateTime.Now;
                rt.DateUpdated = DateTime.Now;

                FrameworkUAS.Service.Response<int> svSave = new FrameworkUAS.Service.Response<int>();
                svSave = rWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, rt);
                if (svSave != null && svSave.Result > 0 && svSave.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    i++;
                }
                else
                {
                    badSaves.Add(s);
                    i++;
                }
            }
            if (badSaves.Count > 0)
            {
                Core_AMS.Utilities.WPF.Message("Save failed.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Incomplete");
                return;
            }
            else
            {
                LoadData();
                Core_AMS.Utilities.WPF.MessageSaveComplete();
            }
            //}
            //else
            //{
            //    Core_AMS.Utilities.WPF.Message("Issue occurred before save could complete.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Incomplete");
            //    return;
            //}
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lbxAvailable.SelectedItems.Count > 0)
            {
                List<string> selectedItems = new List<string>();
                foreach (string s in lbxAvailable.SelectedItems)
                {
                    lbxSelected.Items.Add(s);
                    selectedItems.Add(s);
                }
                foreach (string s in selectedItems)
                {
                    lbxAvailable.Items.Remove(s);
                }
            }
        }
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lbxSelected.SelectedItems.Count > 0)
            {
                List<string> selectedItems = new List<string>();
                foreach (string s in lbxSelected.SelectedItems)
                {
                    lbxAvailable.Items.Add(s);
                    selectedItems.Add(s);
                }
                foreach (string s in selectedItems)
                {
                    lbxSelected.Items.Remove(s);
                }
                lbxAvailable.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            }
        }                        
        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (lbxSelected.SelectedItems.Count > 0)
            {
                for (int i = 0; i < lbxSelected.Items.Count; i++)
                {
                    if (lbxSelected.SelectedItems.Contains(lbxSelected.Items[i]))
                    {
                        if (i > 0 && !lbxSelected.SelectedItems.Contains(lbxSelected.Items[i - 1]))
                        {
                            var item = lbxSelected.Items[i];
                            lbxSelected.Items.Remove(item);
                            lbxSelected.Items.Insert(i - 1, item);
                        }
                    }
                }
            }
        }
        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (lbxSelected.SelectedItems.Count > 0)
            {
                int startindex = lbxSelected.Items.Count - 1;

                for (int i = startindex; i > -1; i--)
                {
                    if (lbxSelected.SelectedItems.Contains(lbxSelected.Items[i]))
                    {
                        if (i < startindex && !lbxSelected.SelectedItems.Contains(lbxSelected.Items[i + 1]))
                        {
                            var item = lbxSelected.Items[i];
                            lbxSelected.Items.Remove(item);
                            lbxSelected.Items.Insert(i + 1, item);
                        }
                    }
                }
            }
        }

        private void cbPublisher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPublisher.SelectedValue != null)
            {
                int PublisherID = 0;
                int.TryParse(cbPublisher.SelectedValue.ToString(), out PublisherID);
                currentClient = allClients.FirstOrDefault(x => x.ClientID == PublisherID);

                cbMagazine.ItemsSource = null;
                cbMagazine.ItemsSource = allProducts.Where(x => x.ClientID == PublisherID);
                cbMagazine.SelectedValuePath = "ProductID";
                cbMagazine.DisplayMemberPath = "ProductCode";
            }
        }

        private void cbMagazine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMagazine.SelectedValue != null)
            {
                int PublicationID = 0;
                int.TryParse(cbMagazine.SelectedValue.ToString(), out PublicationID);
                currentProduct = allProducts.FirstOrDefault(x => x.ProductID == PublicationID);

                lbxAvailable.Items.Clear();
                foreach (string rt in allResponseGroup.Select(x => x.DisplayName).Distinct())
                {
                    lbxAvailable.Items.Add(rt);
                }
                lbxSelected.Items.Clear();
                foreach (FrameworkUAD.Entity.ResponseGroup rt in allResponseGroup.Where(x => x.PubID == PublicationID).ToList().OrderBy(y => y.DisplayOrder))
                {
                    lbxSelected.Items.Add(rt.DisplayName);
                    if (lbxAvailable.Items.Contains(rt.DisplayName))
                        lbxAvailable.Items.Remove(rt.DisplayName);

                } 
            }
        }
    }
}
