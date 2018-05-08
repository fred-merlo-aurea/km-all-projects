using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for Product_Sort_Order.xaml
    /// </summary>
    public partial class ProductSortOrder : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProductTypes> ptWorker = FrameworkServices.ServiceClient.UAD_ProductTypesClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svProducts = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductTypes>> svProductTypes = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductTypes>>();

        FrameworkUAS.Service.Response<int> svIntP = new FrameworkUAS.Service.Response<int>();
        

        List<FrameworkUAD.Entity.Product> products = new List<FrameworkUAD.Entity.Product>();
        List<FrameworkUAD.Entity.ProductTypes> productTypes = new List<FrameworkUAD.Entity.ProductTypes>();
        #endregion

        public ProductSortOrder()
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {             
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();            
            LoadData();                        
        }

        #region Load Methods
        public void LoadData()
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svProducts = pWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svProductTypes = ptWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (svProducts.Result != null && svProducts.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    products = svProducts.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                if (svProductTypes.Result != null && svProductTypes.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)                
                    productTypes = svProductTypes.Result;                
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                LoadProductList();
                LoadProductTypes();

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }
        public void LoadProductTypes()
        {                        
            cbType.ItemsSource = productTypes;
            cbType.SelectedValuePath = "PubTypeID";
            cbType.DisplayMemberPath = "PubTypeDisplayName";                                   
        }
        public void LoadProductList()
        {
            if (cbType.SelectedValue != null)
            {
                int pubTypeID = 0;
                int.TryParse(cbType.SelectedValue.ToString(), out pubTypeID);

                List<FrameworkUAD.Entity.Product> distinctProducts = products.Where(x => x.PubTypeID == pubTypeID).ToList();
                lbxProduct.Items.Clear();
                foreach (FrameworkUAD.Entity.Product p in distinctProducts)
                {
                    lbxProduct.Items.Add(p.PubName.ToString());
                }
            }
        }
        #endregion
        #region Button Methods
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            List<FrameworkUAD.Entity.Product> failedSaves = new List<FrameworkUAD.Entity.Product>();
            int count = 0;
            foreach (string item in lbxProduct.Items)
            {
                FrameworkUAD.Entity.Product thisP = products.FirstOrDefault(x => x.PubName.Equals(item, StringComparison.CurrentCultureIgnoreCase));
                count++;

                if (thisP != null)
                {
                    thisP.SortOrder = count;

                    svIntP = pWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisP, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    if (svIntP.Result != null && svIntP.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        continue;
                    }
                    else
                    {
                        failedSaves.Add(thisP);
                    }
                }
            }
            if (failedSaves.Count > 0)
            {
                Core_AMS.Utilities.WPF.Message("There was an error saving the data order. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Error");
                return;
            }
            else
            {
                LoadData();                                
                Core_AMS.Utilities.WPF.MessageSaveComplete();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            LoadProductList();
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (lbxProduct.SelectedItems.Count > 0)
            {
                for (int i = 0; i < lbxProduct.Items.Count; i++)
                {
                    if (lbxProduct.SelectedItems.Contains(lbxProduct.Items[i]))
                    {
                        if (i > 0 && !lbxProduct.SelectedItems.Contains(lbxProduct.Items[i - 1]))
                        {
                            var item = lbxProduct.Items[i];
                            lbxProduct.Items.Remove(item);
                            lbxProduct.Items.Insert(i - 1, item);
                            lbxProduct.SelectedItems.Add(item);
                            lbxProduct.ScrollIntoView(item);
                        }
                    }
                }
            }
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (lbxProduct.SelectedItems.Count > 0)
            {
                int startindex = lbxProduct.Items.Count - 1;

                for (int i = startindex; i > -1; i--)
                {
                    if (lbxProduct.SelectedItems.Contains(lbxProduct.Items[i]))
                    {
                        if (i < startindex && !lbxProduct.SelectedItems.Contains(lbxProduct.Items[i + 1]))
                        {
                            var item = lbxProduct.Items[i];
                            lbxProduct.Items.Remove(item);
                            lbxProduct.Items.Insert(i + 1, item);
                            lbxProduct.SelectedItems.Add(item);
                            lbxProduct.ScrollIntoView(item);
                        }
                    }
                }
            }
        }
        #endregion

        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadProductList();
        }
    }
}
