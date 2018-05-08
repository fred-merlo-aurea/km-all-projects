using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for ResponseGroups.xaml
    /// </summary>
    public partial class ResponseGroups : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> cWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> rgWorker = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();        
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svProducts = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> svResponseGroups = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codesResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();     

        FrameworkUAS.Service.Response<bool> svBoolRG = new FrameworkUAS.Service.Response<bool>();
        FrameworkUAS.Service.Response<int> svIntRG = new FrameworkUAS.Service.Response<int>();

        List<FrameworkUAD.Entity.Product> products = new List<FrameworkUAD.Entity.Product>();
        List<FrameworkUAD.Entity.ResponseGroup> responseGroups = new List<FrameworkUAD.Entity.ResponseGroup>();
        List<ResponseGroupContainer> responseGroupContainers = new List<ResponseGroupContainer>();

        List<FrameworkUAD_Lookup.Entity.Code> responseGroupTypes = new List<FrameworkUAD_Lookup.Entity.Code>();

        ResponseGroupContainer currentResponseGroup = new ResponseGroupContainer();
        #endregion

        private class ResponseGroupContainer
        {
            public int ResponseGroupID { get; set; }
            public int PubID { get; set; }
            public string ResponseGroupName { get; set; }
            public string DisplayName { get; set; }
            public int? DisplayOrder { get; set; }
            public bool? IsMultipleValue { get; set; }
            public bool? IsRequired { get; set; }
            public bool? IsActive { get; set; }
            public int? WQT_ResponseGroupID { get; set; }
            public int ResponseGroupTypeId { get; set; }
            public string KMProduct { get; set; }

            public ResponseGroupContainer(FrameworkUAD.Entity.ResponseGroup group)
            {
                this.ResponseGroupID = group.ResponseGroupID;
                this.PubID = group.PubID;
                this.ResponseGroupName = group.ResponseGroupName;
                this.DisplayName = group.DisplayName;
                this.DisplayOrder = group.DisplayOrder;
                this.IsMultipleValue = group.IsMultipleValue;
                this.IsRequired = group.IsRequired;
                this.IsActive = group.IsActive;
                this.WQT_ResponseGroupID = group.WQT_ResponseGroupID;
                this.ResponseGroupTypeId = group.ResponseGroupTypeId;
            }
            public ResponseGroupContainer() {}
        }

        public ResponseGroups(int PubID = 0)
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {             
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();
            LoadData(PubID);            
        }

        #region Load Methods
        public void LoadData(int PubID = 0)
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svProducts = pWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svResponseGroups = rgWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                codesResponse = cWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Response_Group);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (svProducts.Result != null && svProducts.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    products = svProducts.Result.OrderBy(x=> x.PubCode).ToList();
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                if (svResponseGroups.Result != null && svResponseGroups.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    responseGroups = svResponseGroups.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                if (codesResponse.Result != null && codesResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    responseGroupTypes = codesResponse.Result;
                    rcbKMProduct.ItemsSource = responseGroupTypes;
                    rcbKMProduct.SelectedValuePath = "CodeId";
                    rcbKMProduct.DisplayMemberPath = "DisplayName";
                }
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                foreach(FrameworkUAD.Entity.ResponseGroup rg in responseGroups)
                {
                    ResponseGroupContainer rgc = new ResponseGroupContainer(rg);
                    FrameworkUAD_Lookup.Entity.Code group = responseGroupTypes.Where(x => x.CodeId == rg.ResponseGroupTypeId).FirstOrDefault();

                    if (group != null)
                        rgc.KMProduct = group.DisplayName;

                    responseGroupContainers.Add(rgc);
                }

                LoadProducts(PubID);
                LoadGrid();

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }
        public void LoadProducts(int PubID = 0)
        {                        
            cbProduct.ItemsSource = null;
            cbProduct.ItemsSource = products.OrderBy(x=> x.PubCode);
            cbProduct.SelectedValuePath = "PubID";
            cbProduct.DisplayMemberPath = "PubName";

            if (PubID > 0)
                cbProduct.SelectedItem = products.FirstOrDefault(x => x.PubID == PubID);
            else
            {
                int i = 0;
                List<ResponseGroupContainer> distinctResponseGroups = responseGroupContainers.Where(x => x.PubID == PubID).ToList();
                while (distinctResponseGroups.Count == 0)
                {

                    if (i == products.Count)
                        return;
                    PubID = products[i].PubID;
                    distinctResponseGroups = responseGroupContainers.Where(x => x.PubID == PubID).ToList();
                    i++;
                }
                cbProduct.SelectedIndex = i;
            }
            
        }
        public void LoadGrid()
        {
            if (cbProduct.SelectedValue != null)
            {
                int pubID = 0;
                int.TryParse(cbProduct.SelectedValue.ToString(), out pubID);

                //List<FrameworkUAD.Entity.ResponseGroup> distinctResponseGroups = responseGroups.Where(x => x.PubID == pubID).ToList();
                //gridGroups.ItemsSource = distinctResponseGroups;
                List<ResponseGroupContainer> distinctResponseGroups = responseGroupContainers.Where(x => x.PubID == pubID).ToList();
                gridGroups.ItemsSource = distinctResponseGroups;
            }
        }
        public void RefreshData()
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svProducts = pWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svResponseGroups = rgWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (svProducts.Result != null && svProducts.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    products = svProducts.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                if (svResponseGroups.Result != null && svResponseGroups.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    responseGroups = svResponseGroups.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                responseGroupContainers.Clear();
                foreach (FrameworkUAD.Entity.ResponseGroup rg in responseGroups)
                {
                    ResponseGroupContainer rgc = new ResponseGroupContainer(rg);
                    FrameworkUAD_Lookup.Entity.Code group = responseGroupTypes.Where(x => x.CodeId == rg.ResponseGroupTypeId).FirstOrDefault();

                    if (group != null)
                        rgc.KMProduct = group.DisplayName;

                    responseGroupContainers.Add(rgc);
                }

                LoadGrid();

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }
        #endregion

        private void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentResponseGroup = null;
            tbxGroup.Text = "";
            tbxName.Text = "";
            tbxDisplay.Text = "";
            btnSave.Tag = "";
            btnSave.Content = "Save";
            cbxIsActive.IsChecked = false;
            cbxMulti.IsChecked = false;
            cbxReq.IsChecked = false;
            rcbKMProduct.SelectedItem = null;

            LoadGrid();
        }

        #region Button Methods
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string Group = "";
            string DisplayName = "";
            int PubID = 0;
            int KMProduct = 0;
            int ResponseGroupID = 0;
            if (btnSave.Tag != null)
                int.TryParse(btnSave.Tag.ToString(), out ResponseGroupID);

            int display = 0;
            bool multi = false;
            bool req = false;
            bool active = true;

            #region Check Empty Data
            if (string.IsNullOrEmpty(tbxGroup.Text))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a Group.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            else
            {
                Group = tbxGroup.Text;
                if (Group.Contains(" "))
                {
                    Core_AMS.Utilities.WPF.Message("Group cannot contain space characters.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Data");
                    return;
                }
            }

            if (string.IsNullOrEmpty(tbxName.Text))
            {
                Core_AMS.Utilities.WPF.Message("Please provide a Display Name.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            else
                DisplayName = tbxName.Text;
            
            if (cbProduct.SelectedValue != null)
            {
                int.TryParse(cbProduct.SelectedValue.ToString(), out PubID);
                if (!(PubID > 0))
                {
                    Core_AMS.Utilities.WPF.Message("Error loading product data. Please contact us if problem persists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                    return;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Please select a product.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            if(rcbKMProduct.SelectedItem != null)
            {
                int.TryParse(rcbKMProduct.SelectedValue.ToString(), out KMProduct);
                if (!(KMProduct > 0))
                {
                    Core_AMS.Utilities.WPF.Message("Error loading product data. Please contact us if problem persists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                    return;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Please select KM Product.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Empty Data");
                return;
            }
            #endregion

            #region Check Empty Data
            int.TryParse(tbxDisplay.Text, out display);

            if (cbxMulti.IsChecked == true)
                multi = true;
            else
                multi = false;

            if (cbxReq.IsChecked == true)
                req = true;
            else
                req = false;

            if (cbxIsActive.IsChecked == true)
                active = true;
            else
                active = false;
            #endregion

            #region Check value doesn't exist
            if (currentResponseGroup != null)
            {
                if (!(currentResponseGroup.ResponseGroupName.Equals(Group, StringComparison.CurrentCultureIgnoreCase)))
                {
                    if (responseGroups.Where(x=> x.PubID == PubID).FirstOrDefault(x => x.ResponseGroupName.Equals(Group, StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        Core_AMS.Utilities.WPF.Message("Group already exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Duplicate Submission");
                        return;
                    }
                }
            }
            else
            {
                if (responseGroups.Where(x=> x.PubID == PubID).FirstOrDefault(x => x.ResponseGroupName.Equals(Group, StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Group already exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Duplicate Submission");
                    return;
                }
            }
            #endregion

            #region Save|Update
            FrameworkUAD.Entity.ResponseGroup rgEntry = new FrameworkUAD.Entity.ResponseGroup();
            rgEntry.ResponseGroupID = ResponseGroupID;
            rgEntry.PubID = PubID;
            rgEntry.ResponseGroupName = Group;
            rgEntry.DisplayName = DisplayName;
            rgEntry.DisplayOrder = display;
            rgEntry.IsMultipleValue = multi;
            rgEntry.IsRequired = req;
            rgEntry.IsActive = active;
            rgEntry.ResponseGroupTypeId = KMProduct;
            svIntRG = rgWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, rgEntry);
            if (svIntRG.Result != null && svIntRG.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                Core_AMS.Utilities.WPF.MessageSaveComplete();
                currentResponseGroup = null;
                tbxGroup.Text = "";
                tbxName.Text = "";
                rcbKMProduct.SelectedItem = null;
                btnSave.Tag = "";
                btnSave.Content = "Save";
                tbxDisplay.Text = "";
                RefreshData();
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error saving the data. If this problem persists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Error");
                return;
            }
            #endregion
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            currentResponseGroup = null;
            tbxGroup.Text = "";
            tbxName.Text = "";
            tbxDisplay.Text = "";
            cbxMulti.IsChecked = false;
            cbxReq.IsChecked = false;
            cbxIsActive.IsChecked = false;
            rcbKMProduct.SelectedItem = null;

            btnSave.Tag = "";
            btnSave.Content = "Save";
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(ResponseGroupContainer))
                {
                    ResponseGroupContainer rgItem = (ResponseGroupContainer)b.DataContext;
                    if (rgItem != null)
                    {
                        currentResponseGroup = rgItem;
                        tbxGroup.Text = rgItem.ResponseGroupName;
                        tbxName.Text = rgItem.DisplayName;
                        rcbKMProduct.SelectedValue = rgItem.ResponseGroupTypeId;

                        if (rgItem.DisplayOrder != null)
                            tbxDisplay.Text = rgItem.DisplayOrder.ToString();

                        if (rgItem.IsMultipleValue != null && rgItem.IsMultipleValue == true)
                            cbxMulti.IsChecked = true;
                        else
                            cbxMulti.IsChecked = false;

                        if (rgItem.IsRequired != null && rgItem.IsRequired == true)
                            cbxReq.IsChecked = true;
                        else
                            cbxReq.IsChecked = false;

                        if (rgItem.IsActive != null && rgItem.IsActive == true)
                            cbxIsActive.IsChecked = true;
                        else
                            cbxIsActive.IsChecked = false;

                        btnSave.Tag = rgItem.ResponseGroupID.ToString();
                        btnSave.Content = "Update";
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us. Reference code AS10.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("There was an error loading the data. If this problem consists please contact us. Reference code AS9.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Data Error");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult areYouSureDelete = MessageBox.Show("Are you sure you want to delete this Response Group?  It will delete the Response Group, Codesheet Responses and all mappings for this Response Group.", "Warning", MessageBoxButton.YesNo);

            if (areYouSureDelete == MessageBoxResult.Yes)
            {
                if (e.OriginalSource.GetType() == typeof(Button))
                {
                    Button b = (Button)e.OriginalSource;
                    if (b.DataContext.GetType() == typeof(ResponseGroupContainer))
                    {
                        ResponseGroupContainer rgItem = (ResponseGroupContainer)b.DataContext;
                        if (rgItem != null)
                        {
                            svBoolRG = rgWorker.Proxy.Delete(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, rgItem.ResponseGroupID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                            if ((svBoolRG.Result == true || svBoolRG.Result == false) && svBoolRG.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                            {
                                currentResponseGroup = null;
                                tbxGroup.Text = "";
                                tbxName.Text = "";

                                btnSave.Tag = "";
                                btnSave.Content = "Save";
                                RefreshData();
                                Core_AMS.Utilities.WPF.MessageDeleteComplete();
                            }
                            else
                            {
                                Core_AMS.Utilities.WPF.Message("There was an error deleting the data. If this problem consists please contact us.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Deletion Error");
                                return;
                            }
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

        private void btnResponses_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(ResponseGroupContainer))
                {
                    ResponseGroupContainer rgItem = (ResponseGroupContainer)b.DataContext;
                    if (rgItem != null)
                    {
                        //OPEN MasterCodeSheet pass MasterGroupID
                        List<DockPanel> dps = Application.Current.MainWindow.ChildrenOfType<DockPanel>().ToList();
                        foreach (DockPanel dp in dps)
                        {
                            if (dp.Name == "spModule")
                            {
                                List<DockPanel> dpControls = dp.ChildrenOfType<DockPanel>().ToList();
                                foreach (DockPanel sdp in dpControls)
                                {
                                    if (sdp.Name == "spControls")
                                    {
                                        sdp.Children.Clear();
                                        ControlCenter.Controls.CodeSheet cs = new CodeSheet(rgItem.PubID, rgItem.ResponseGroupID);
                                        sdp.Children.Add(cs);
                                    }
                                }
                                    
                            }
                        }
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
        #endregion
    }
}
