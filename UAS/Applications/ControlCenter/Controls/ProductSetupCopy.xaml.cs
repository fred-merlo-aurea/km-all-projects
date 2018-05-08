using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for ProductSetupCopy.xaml
    /// </summary>
    public partial class ProductSetupCopy : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svProducts = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();

        FrameworkUAS.Service.Response<bool> svBoolP = new FrameworkUAS.Service.Response<bool>();

        List<FrameworkUAD.Entity.Product> products = new List<FrameworkUAD.Entity.Product>();
        #endregion

        public ProductSetupCopy()
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {             
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();            
            LoadProducts();            
        }

        #region Load Methods
        public void LoadProducts()
        {
	        busy.IsBusy = true;
	        BackgroundWorker bw = new BackgroundWorker();
	        bw.DoWork += (o, ea) =>
	        {
                svProducts = pWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
	        };
	        bw.RunWorkerCompleted += (o, ea) =>
	        {
                if (svProducts.Result != null && svProducts.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    products = svProducts.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                LoadFrom();
                LoadTo(); 
   
		        busy.IsBusy = false;
	        };
	        bw.RunWorkerAsync();                                    
        }
        public void LoadFrom()
        {
            if (products.Count > 0)
            {
                cbFrom.ItemsSource = products;
                cbFrom.SelectedValuePath = "PubID";
                cbFrom.DisplayMemberPath = "PubName";
            }
        }
        public void LoadTo()
        {
            if (products.Count > 0)
            {
                cbTo.ItemsSource = products;
                cbTo.SelectedValuePath = "PubID";
                cbTo.DisplayMemberPath = "PubName";
            }
        }
        #endregion

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            if (cbFrom.SelectedValue != null && cbTo.SelectedValue != null)
            {
                int fromID = 0;
                int toID = 0;

                int.TryParse(cbFrom.SelectedValue.ToString(), out fromID);
                int.TryParse(cbTo.SelectedValue.ToString(), out toID);

                if (fromID > 0 && toID > 0)
                {
                    if (fromID == toID)
                    {
                        Core_AMS.Utilities.WPF.MessageError("Cannot copy the same Product to itself.");
                        return; 
                    }

                    MessageBoxResult result = MessageBox.Show("Are you sure you want to copy this Product?", "Confirmation", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {

                        pWorker.Proxy.Copy(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, fromID, toID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        
                        svProducts = pWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        if (svProducts.Result != null && svProducts.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                        {
                            LoadProducts();
                            Core_AMS.Utilities.WPF.MessageError("Copy Completed.");
                        }
                        else
                        {
                            Core_AMS.Utilities.WPF.MessageError("An unexpected error occured during a service request, please reload the page. If the problem persists please contact Customer Support.");
                            return;
                        }
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageError("Cannot continue. Issue with getting data for From value or To value. If the problem persists please contact Customer Support.");
                    return;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.MessageError("Please select both From value and To value before continuing.");
                return;
            }
        }
    }
}
