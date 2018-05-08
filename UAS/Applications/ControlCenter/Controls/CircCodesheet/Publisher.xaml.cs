using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls.CircCodesheet
{
    /// <summary>
    /// Interaction logic for Publisher.xaml
    /// </summary>
    public partial class Publisher : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> pWorker = FrameworkServices.ServiceClient.UAS_ClientClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> svPUB = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();

        List<KMPlatform.Entity.Client> allPublishers = new List<KMPlatform.Entity.Client>();

        KMPlatform.Entity.Client currentPublisher = new KMPlatform.Entity.Client();        
        #endregion
        public Publisher()
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
                svPUB = pWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Load Publishers
                if (svPUB.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allPublishers = svPUB.Result;
                    gridPublishers.ItemsSource = allPublishers.Where(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID).ToList();
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
                if (b.DataContext.GetType() == typeof(KMPlatform.Entity.Client))
                {
                    KMPlatform.Entity.Client pItem = (KMPlatform.Entity.Client)b.DataContext;
                    if (pItem != null)
                    {
                        currentPublisher = pItem;

                        tbxPublisherID.Text = pItem.ClientID.ToString();
                        tbxPublisherName.Text = pItem.ClientName;
                        tbxPublisherCode.Text = pItem.ClientCode;

                        btnSave.Tag = pItem.ClientID.ToString();                        
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
            currentPublisher = null;

            //Clear control
            tbxPublisherID.Text = "";
            tbxPublisherName.Text = "";
            tbxPublisherCode.Text = "";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int ID = 0;
            int ClientID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
            string Name = tbxPublisherName.Text;
            string Code = tbxPublisherCode.Text;
            
            if (btnSave.Tag != null)
                int.TryParse(btnSave.Tag.ToString(), out ID);

            if (!(ClientID > 0))
            {
                Core_AMS.Utilities.WPF.Message("Client is not clear. Please select a client before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client");
                return;
            }

            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Code))
            {
                #region Check Values
                if (ID > 0)
                {
                    //Check not value existence name and code based by client
                    if (allPublishers.FirstOrDefault(x => x.ClientName.Equals(Name, StringComparison.CurrentCultureIgnoreCase) && x.ClientID == ClientID && x.ClientID != ID) != null)
                    {
                        Core_AMS.Utilities.WPF.Message("Name currently exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                        return;
                    }
                    else if (allPublishers.FirstOrDefault(x => x.ClientName.Equals(Code, StringComparison.CurrentCultureIgnoreCase) && x.ClientID == ClientID && x.ClientID != ID) != null)
                    {
                        Core_AMS.Utilities.WPF.Message("Code currently exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                        return;
                    }
                }
                else
                {
                    //Check not value existence name and code
                    if (allPublishers.FirstOrDefault(x => x.ClientName.Equals(Name, StringComparison.CurrentCultureIgnoreCase) && x.ClientID == ClientID) != null)
                    {
                        Core_AMS.Utilities.WPF.Message("Name currently exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                        return;
                    }
                    else if (allPublishers.FirstOrDefault(x => x.ClientName.Equals(Code, StringComparison.CurrentCultureIgnoreCase) && x.ClientID == ClientID) != null)
                    {
                        Core_AMS.Utilities.WPF.Message("Code currently exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                        return;
                    }
                }
                #endregion

                #region Prepare Item Save|Update
                if (ID > 0)
                {
                    currentPublisher.ClientID = ID;
                    currentPublisher.ClientName = Name;
                    currentPublisher.ClientCode = Code;
                    currentPublisher.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    currentPublisher.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    currentPublisher.DateCreated = DateTime.Now;
                    currentPublisher.DateUpdated = DateTime.Now;
                }
                else
                {
                    currentPublisher = new KMPlatform.Entity.Client();
                    currentPublisher.ClientID = ID;
                    currentPublisher.ClientName = Name;
                    currentPublisher.ClientCode = Code;
                    currentPublisher.IsActive = true;
                    currentPublisher.HasPaid = false;
                    currentPublisher.ClientID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                    currentPublisher.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    currentPublisher.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    currentPublisher.DateCreated = DateTime.Now;
                    currentPublisher.DateUpdated = DateTime.Now;
                }

                FrameworkUAS.Service.Response<int> svSave = new FrameworkUAS.Service.Response<int>();
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (o, ea) =>
                {
                    svSave = pWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, currentPublisher);
                };
                bw.RunWorkerCompleted += (o, ea) =>
                {
                    #region Refresh|Clear
                    if (svSave != null && svSave.Result > 0 && svSave.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        //Set currentItem to null
                        currentPublisher = null;

                        //Change Button tag to zero content back to save
                        btnSave.Tag = "0";
                        btnSave.Content = "Save";

                        //Clear control
                        tbxPublisherID.Text = "";
                        tbxPublisherName.Text = "";
                        tbxPublisherCode.Text = "";

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
            else
            {
                Core_AMS.Utilities.WPF.Message("Please enter a valid name and code before saving.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
        }
    }
}
