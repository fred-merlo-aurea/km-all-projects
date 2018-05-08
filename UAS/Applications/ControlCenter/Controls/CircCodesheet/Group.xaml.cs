using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls.CircCodesheet
{
    /// <summary>
    /// Interaction logic for Group.xaml
    /// </summary>
    public partial class Group : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> pWorker = FrameworkServices.ServiceClient.UAS_ClientClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> rWorker = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();        
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> svPub = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svPublication = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> svResponseGroup = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();        

        List<KMPlatform.Entity.Client> allPublishers = new List<KMPlatform.Entity.Client>();
        List<KMPlatform.Object.Product> allPublications = new List<KMPlatform.Object.Product>();
        List<FrameworkUAD.Entity.ResponseGroup> allResponseGroup = new List<FrameworkUAD.Entity.ResponseGroup>();        

        KMPlatform.Entity.Client currentPublisher = new KMPlatform.Entity.Client();
        KMPlatform.Object.Product currentPublication = new KMPlatform.Object.Product();
        FrameworkUAD.Entity.ResponseGroup currentResponseGroup = new FrameworkUAD.Entity.ResponseGroup();
        #endregion
        public Group()
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
                //svPub = pWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                //svPublication = pubWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                svResponseGroup = rWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Load Publishers
                //if (svPub.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //{
                    //allPublishers = svPub.Result;
                    allPublishers = new List<KMPlatform.Entity.Client>();
                    allPublishers.AddRange(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients);

                    cbPublisher.ItemsSource = null;
                    cbPublisher.ItemsSource = allPublishers.Where(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID).ToList();
                    cbPublisher.SelectedValuePath = "ClientID";
                    cbPublisher.DisplayMemberPath = "DisplayName";

                    if (cbPublisher.SelectedValue != null)
                    {
                        int PublisherID = 0;
                        int.TryParse(cbPublisher.SelectedValue.ToString(), out PublisherID);
                        currentPublisher = allPublishers.FirstOrDefault(x => x.ClientID == PublisherID);                        
                    } 
                    else if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0)
                    {
                        currentPublisher = allPublishers.FirstOrDefault(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
                    }
                //}
                //else
                //{
                //    Core_AMS.Utilities.WPF.MessageServiceError();
                //}
                #endregion
                #region Load Publications
                //if (svPublication.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //{
                    allPublications = new List<KMPlatform.Object.Product>();
                foreach (var cp in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients)
                    allPublications.AddRange(cp.Products);

                if (currentPublisher != null)
                {
                    cbPublisher.SelectedItem = currentPublisher;

                    cbMagazine.ItemsSource = null;
                    cbMagazine.ItemsSource = allPublications.Where(x => x.ClientID == currentPublisher.ClientID).ToList(); ;
                    cbMagazine.SelectedValuePath = "ProductID";
                    cbMagazine.DisplayMemberPath = "ProductCode";

                    if (cbMagazine.SelectedValue != null)
                    {
                        int PublicationID = 0;
                        int.TryParse(cbMagazine.SelectedValue.ToString(), out PublicationID);
                        currentPublication = allPublications.FirstOrDefault(x => x.ProductID == PublicationID);                            
                    }
                    else if (currentPublication != null)
                    {
                        cbMagazine.SelectedItem = currentPublication;
                        cbMagazine.SelectedValue = currentPublication.ProductID;
                    }
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

                    if (currentPublication != null)
                        gridGroup.ItemsSource = allResponseGroup.Where(x => x.PubID == currentPublication.ProductID).OrderBy(x => x.DisplayOrder).ToList();                                            

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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.ResponseGroup))
                {
                    FrameworkUAD.Entity.ResponseGroup rtItem = (FrameworkUAD.Entity.ResponseGroup)b.DataContext;
                    if (rtItem != null)
                    {
                        currentResponseGroup = rtItem;

                        tbxGroup.Text = rtItem.ResponseGroupName;
                        tbxDisplayName.Text = rtItem.DisplayName;
                        if (rtItem.IsRequired == true)
                            cbxIsRequired.IsChecked = true;
                        else
                            cbxIsRequired.IsChecked = false;

                        if (rtItem.IsMultipleValue == true)
                            cbxIsMultipleValue.IsChecked = true;
                        else
                            cbxIsMultipleValue.IsChecked = false;

                        if (rtItem.IsActive == true)
                            cbxIsActive.IsChecked = true;
                        else
                            cbxIsActive.IsChecked = false;

                        btnSave.Tag = rtItem.ResponseGroupID.ToString();
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //Change Button tag to zero content back to save
            btnSave.Tag = "0";
            btnSave.Content = "Save";

            //Set currentItem to null
            currentResponseGroup = null;

            //Clear control
            tbxGroup.Text = "";
            tbxDisplayName.Text = "";
            cbxIsRequired.IsChecked = false;
            cbxIsMultipleValue.IsChecked = false;
            cbxIsActive.IsChecked = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region Check Values
            int ResponseGroupID = 0;
            if (btnSave.Tag != null)
                int.TryParse(btnSave.Tag.ToString(), out ResponseGroupID);

            string Group = tbxGroup.Text;
            string Name = tbxDisplayName.Text;
            bool IsReq = false;
            if (cbxIsRequired.IsChecked == true)
                IsReq = true;

            bool IsMV = false;
            if (cbxIsMultipleValue.IsChecked == true)
                IsMV = true;

            bool IsActive = false;
            if (cbxIsActive.IsChecked == true)
                IsActive = true;

            int PubID = 0;
            if (cbMagazine.SelectedValue != null)
                int.TryParse(cbMagazine.SelectedValue.ToString(), out PubID);
            else
            {
                Core_AMS.Utilities.WPF.Message("No magazine was selected. Please select a magazine.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }

            if (string.IsNullOrEmpty(Group))
            {
                Core_AMS.Utilities.WPF.Message("No group was provided. Please provide a group.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
            if (string.IsNullOrEmpty(Name))
            {
                Core_AMS.Utilities.WPF.Message("No name was provided. Please provide a name.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }

            if (ResponseGroupID > 0)
            {
                //Check not value existence name and code based by client
                if (allResponseGroup.FirstOrDefault(x => x.ResponseGroupName.Equals(Group, StringComparison.CurrentCultureIgnoreCase) && x.PubID == PubID && x.ResponseGroupID != ResponseGroupID) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Group currently exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                    return;
                }
            }
            else
            {
                //Check not value existence name and code
                if (allResponseGroup.FirstOrDefault(x => x.ResponseGroupName.Equals(Group, StringComparison.CurrentCultureIgnoreCase) && x.PubID == PubID) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Group currently exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                    return;
                }
            }
            #endregion

            #region Prepare
            if (ResponseGroupID > 0)
            {
                currentResponseGroup.ResponseGroupID = ResponseGroupID;                
                currentResponseGroup.ResponseGroupName = Group;
                currentResponseGroup.DisplayName = Name;               
                currentResponseGroup.IsMultipleValue = IsMV;
                currentResponseGroup.IsRequired = IsReq;
                currentResponseGroup.IsActive = IsActive;
                currentResponseGroup.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                currentResponseGroup.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                currentResponseGroup.DateCreated = DateTime.Now;
                currentResponseGroup.DateUpdated = DateTime.Now;
            }
            else
            {
                currentResponseGroup = new FrameworkUAD.Entity.ResponseGroup();
                currentResponseGroup.ResponseGroupID = ResponseGroupID;
                currentResponseGroup.PubID = PubID;
                currentResponseGroup.ResponseGroupName = Group;
                currentResponseGroup.DisplayName = Name;
                currentResponseGroup.DisplayOrder = allResponseGroup.Where(x => x.PubID == PubID).ToList().Count;
                currentResponseGroup.IsMultipleValue = IsMV;
                currentResponseGroup.IsRequired = IsReq;
                currentResponseGroup.IsActive = IsActive;
                currentResponseGroup.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                currentResponseGroup.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                currentResponseGroup.DateCreated = DateTime.Now;
                currentResponseGroup.DateUpdated = DateTime.Now;
            }
            #endregion

            #region Save|Update
            FrameworkUAS.Service.Response<int> svSave = new FrameworkUAS.Service.Response<int>();
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svSave = rWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, currentResponseGroup);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Refresh|Clear
                if (svSave != null && svSave.Result > 0 && svSave.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    //Set currentItem to null
                    currentResponseGroup = null;

                    //Change Button tag to zero content back to save
                    btnSave.Tag = "0";
                    btnSave.Content = "Save";

                    //Clear control
                    tbxGroup.Text = "";
                    tbxDisplayName.Text = "";
                    cbxIsRequired.IsChecked = false;
                    cbxIsMultipleValue.IsChecked = false;
                    cbxIsActive.IsChecked = false;

                    //Refresh Grid
                    LoadData();

                    Core_AMS.Utilities.WPF.MessageSaveComplete();
                }
                else
                    Core_AMS.Utilities.WPF.Message("Save failed.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Incomplete");

                #endregion

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
            #endregion
        }

        private void cbPublisher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPublisher.SelectedValue != null)
            {
                int PublisherID = 0;
                int.TryParse(cbPublisher.SelectedValue.ToString(), out PublisherID);
                currentPublisher = allPublishers.FirstOrDefault(x => x.ClientID == PublisherID);

                cbMagazine.ItemsSource = null;
                cbMagazine.ItemsSource = allPublications.Where(x => x.ProductID == PublisherID).ToList(); ;
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
                currentPublication = allPublications.FirstOrDefault(x => x.ProductID == PublicationID);

                gridGroup.ItemsSource = allResponseGroup.Where(x => x.PubID == PublicationID).OrderBy(x => x.DisplayOrder).ToList();
            }
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            //Show a window to change order
            Telerik.Windows.Controls.RadWindow rw = new Telerik.Windows.Controls.RadWindow();
            rw.Height = 350;
            rw.Width = 450;
            rw.Header = "Display Order";
            rw.ResizeMode = ResizeMode.NoResize;
            rw.Content = new ControlCenter.Controls.CircCodesheet.GroupOrder(currentPublisher.ClientID, currentPublication.ProductID);
            rw.ShowDialog();

            LoadData();
        }
    }
}
