using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for UADDataRemovalMgmt.xaml
    /// </summary>
    public partial class UADDataRemovalMgmt : UserControl
    {
        public static FrameworkUAS.Object.AppData myAppData { get; set; }

        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IOperations> operationsWorker = FrameworkServices.ServiceClient.UAD_OperationsClient();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svProducts = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<bool> svBoolPubCode = new FrameworkUAS.Service.Response<bool>();
        FrameworkUAS.Service.Response<bool> svBoolProcess = new FrameworkUAS.Service.Response<bool>();

        public UADDataRemovalMgmt()
        {
            //only want this available to users that belong to KM
            if (FrameworkUAS.Object.AppData.IsKmUser() == true)
            {
                InitializeComponent();
                LoadData();
            }
            else
            {
                Core_AMS.Utilities.WPF.MessageAccessDenied();
            }
        }

        private void LoadData()
        {
            List<FrameworkUAD.Entity.Product> allProducts = new List<FrameworkUAD.Entity.Product>();
            svProducts = productWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.AccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            allProducts = svProducts.Result;

            foreach (Core_AMS.Utilities.Enums.FileRemovalOption dl in (Core_AMS.Utilities.Enums.FileRemovalOption[])Enum.GetValues(typeof(Core_AMS.Utilities.Enums.FileRemovalOption)))
            {
                rcbOption.Items.Add(dl.ToString().Replace("_", " "));
            }

            rcbProduct.ItemsSource = allProducts;
            rcbProduct.DisplayMemberPath = "PubCode";
            rcbProduct.SelectedValuePath = "PubCode";
        }

        private void rcbOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            #region Product
            if (rcbOption.SelectedValue.ToString().Equals(Core_AMS.Utilities.Enums.FileRemovalOption.Product_Code.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase))
            {
                tbProcess.Visibility = System.Windows.Visibility.Collapsed;
                txtProcess.Visibility = System.Windows.Visibility.Collapsed;

                tbProduct.Visibility = System.Windows.Visibility.Visible;
                rcbProduct.Visibility = System.Windows.Visibility.Visible;

                btnExecute.Visibility = System.Windows.Visibility.Visible;
            }
            #endregion
            #region Process
            else if (rcbOption.SelectedValue.ToString().Equals(Core_AMS.Utilities.Enums.FileRemovalOption.Process_Code.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase))
            {
                tbProcess.Visibility = System.Windows.Visibility.Visible;
                txtProcess.Visibility = System.Windows.Visibility.Visible;

                tbProduct.Visibility = System.Windows.Visibility.Collapsed;
                rcbProduct.Visibility = System.Windows.Visibility.Collapsed;

                btnExecute.Visibility = System.Windows.Visibility.Visible;
            }
            #endregion
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            #region Product
            if (rcbOption.SelectedValue.ToString().Equals(Core_AMS.Utilities.Enums.FileRemovalOption.Product_Code.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase))
            {
                if (rcbProduct.SelectedValue != null)
                {
                    string pubCode = rcbProduct.SelectedValue.ToString();
                    MessageBoxResult areYouSureDelete = MessageBox.Show("Are you sure you want to remove data for the product: " + pubCode, "Warning", MessageBoxButton.YesNo);

                    if (areYouSureDelete == MessageBoxResult.Yes)
                    {
                        svBoolPubCode = operationsWorker.Proxy.RemovePubCode(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.AccessKey, pubCode, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        if (svBoolPubCode.Result != null && svBoolPubCode.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                            Core_AMS.Utilities.WPF.MessageDeleteComplete();
                        else
                            Core_AMS.Utilities.WPF.MessageServiceError();
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageError("You must select a Product before executing.");
                    return;
                }
            }
            #endregion
            #region Process
            else if (rcbOption.SelectedValue.ToString().Equals(Core_AMS.Utilities.Enums.FileRemovalOption.Process_Code.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase))
            {
                if (!string.IsNullOrEmpty(txtProcess.Text.Trim()))
                {
                    string processCode = txtProcess.Text.Trim();
                    MessageBoxResult areYouSureDelete = MessageBox.Show("Are you sure you want to remove data for the process code: " + processCode, "Warning", MessageBoxButton.YesNo);

                    if (areYouSureDelete == MessageBoxResult.Yes)
                    {
                        svBoolProcess = operationsWorker.Proxy.RemoveProcessCode(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.AccessKey, processCode, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        if (svBoolProcess.Result != null && svBoolProcess.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                            Core_AMS.Utilities.WPF.MessageDeleteComplete();
                        else
                            Core_AMS.Utilities.WPF.MessageServiceError();

                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageError("You must enter a Process Code before executing.");
                    return;
                }
            }
            #endregion
        }
    }
}
