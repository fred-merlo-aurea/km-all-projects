using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for ProductType.xaml
    /// </summary>
    public partial class ProductType : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProductTypes> ptWorker = FrameworkServices.ServiceClient.UAD_ProductTypesClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductTypes>> svProductTypes = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductTypes>>();

        FrameworkUAS.Service.Response<int> svIntPT = new FrameworkUAS.Service.Response<int>();

        List<FrameworkUAD.Entity.ProductTypes> productTypes = new List<FrameworkUAD.Entity.ProductTypes>();

        FrameworkUAD.Entity.ProductTypes currentProductType = new FrameworkUAD.Entity.ProductTypes();
        #endregion

        public ProductType()
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {             
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();            
            LoadProductTypes();            
        }

        #region Loads
        public void LoadProductTypes()
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svProductTypes = ptWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (svProductTypes.Result != null && svProductTypes.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    productTypes = svProductTypes.Result;
                    gridTypes.ItemsSource = productTypes;
                }
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                LoadCombos();

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();                        
        }        
        public void LoadCombos()
        {
            int items = productTypes.Count + 1;
            for (int i = 1; i <= items; i++)
            {
                cbOrder.Items.Add(i.ToString());
            }
        
            foreach (Core_AMS.Utilities.Enums.YesNo tf in (Core_AMS.Utilities.Enums.YesNo[])Enum.GetValues(typeof(Core_AMS.Utilities.Enums.YesNo)))
            {
                cbActive.Items.Add(tf.ToString().Replace("_", " "));
            } 
        }
        #endregion
        #region Button Methods
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int PubTypeID = 0;
            string Name = "";            
            bool IsActive = false;            
            int SortOrder = 0;

            if (btnSave.Tag != null)
                int.TryParse(btnSave.Tag.ToString(), out PubTypeID);

            #region Check Empty Data
            if (string.IsNullOrEmpty(tbxName.Text))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a Name.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            else
                Name = tbxName.Text;            

            if (cbActive.SelectedItem != null)
            {
                if (cbActive.SelectedItem.ToString() == Core_AMS.Utilities.Enums.YesNo.Yes.ToString())
                    IsActive = true;
                else
                    IsActive = false;

            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Please select active value.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }

            if (cbOrder.SelectedItem != null)
            {
                int.TryParse(cbOrder.SelectedItem.ToString(), out SortOrder);
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Please select sub reporting value.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }            
            #endregion

            #region Check value doesn't exist
            if (currentProductType != null)
            {
                if (currentProductType.PubTypeDisplayName != Name)
                {
                    if (productTypes.FirstOrDefault(x => x.PubTypeID == PubTypeID && x.PubTypeDisplayName == Name) != null)
                    {
                        Core_AMS.Utilities.WPF.Message("Value already exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Duplicate Submission");
                        return;
                    }
                }
            }
            else
            {
                if (productTypes.FirstOrDefault(x => x.PubTypeDisplayName == Name) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Value already exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Duplicate Submission");
                    return;
                }
            }
            #endregion

            #region Save|Update
            FrameworkUAD.Entity.ProductTypes ptEntry = new FrameworkUAD.Entity.ProductTypes();
            ptEntry.PubTypeID = PubTypeID;
            ptEntry.PubTypeDisplayName = Name;
            ptEntry.ColumnReference = Name;
            ptEntry.IsActive = IsActive;
            ptEntry.SortOrder = SortOrder;
            svIntPT = ptWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, ptEntry, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (svIntPT.Result != null && svIntPT.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                Core_AMS.Utilities.WPF.MessageSaveComplete();
                currentProductType = null;
                tbxName.Text = "";
                cbOrder.SelectedIndex = -1;
                cbActive.SelectedIndex = -1;

                btnSave.Tag = "";
                btnSave.Content = "Save"; 
                LoadProductTypes();
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error saving the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Error");
                return;
            }
            #endregion
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            currentProductType = null;
            tbxName.Text = "";
            cbOrder.SelectedIndex = -1;
            cbActive.SelectedIndex = -1;

            btnSave.Tag = "";
            btnSave.Content = "Save";  
        }       

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.ProductTypes))
                {
                    FrameworkUAD.Entity.ProductTypes ptItem = (FrameworkUAD.Entity.ProductTypes)b.DataContext;
                    if (ptItem != null)
                    {
                        currentProductType = ptItem;
                        tbxName.Text = currentProductType.PubTypeDisplayName;

                        if (currentProductType.IsActive.ToString() == Core_AMS.Utilities.Enums.TrueFalse.True.ToString())
                            cbActive.SelectedItem = Core_AMS.Utilities.Enums.YesNo.Yes.ToString();
                        else
                            cbActive.SelectedItem = Core_AMS.Utilities.Enums.YesNo.No.ToString();
                        
                        cbOrder.SelectedItem = currentProductType.SortOrder.ToString();                        

                        btnSave.Tag = currentProductType.PubTypeID.ToString();
                        btnSave.Content = "Update";
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult areYouSureDelete = MessageBox.Show("Are you sure you want to delete this Product Type?", "Warning", MessageBoxButton.YesNo);

            if (areYouSureDelete == MessageBoxResult.Yes)
            {
                if (e.OriginalSource.GetType() == typeof(Button))
                {
                    Button b = (Button)e.OriginalSource;
                    if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.ProductTypes))
                    {
                        FrameworkUAD.Entity.ProductTypes ptItem = (FrameworkUAD.Entity.ProductTypes)b.DataContext;
                        if (ptItem != null)
                        {
                            int PubTypeID = ptItem.PubTypeID;
                            #region Delete SubscriptionDetails
                            FrameworkUAS.Service.Response<bool> svBoolPT = new FrameworkUAS.Service.Response<bool>();
                            svBoolPT = ptWorker.Proxy.Delete(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, PubTypeID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                            if ((svBoolPT.Result == true || svBoolPT.Result == false) && svBoolPT.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                            {
                                LoadProductTypes();
                                Core_AMS.Utilities.WPF.MessageDeleteComplete();
                            }
                            else
                            {
                                Core_AMS.Utilities.WPF.Message("Failed to delete. If this problem consists please contact Customer Service.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                                return;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                        return;
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                    return;
                }
            }
        }
        #endregion
    }
}
