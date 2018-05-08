using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for ReportGroups.xaml
    /// </summary>
    public partial class ReportGroups : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> pWorker = FrameworkServices.ServiceClient.UAS_ClientClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> rWorker = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IReportGroups> rgWorker = FrameworkServices.ServiceClient.UAD_ReportGroupsClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> svPub = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svPublication = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> svResponseGroup = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ReportGroups>> svReportGroups = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ReportGroups>>();

        List<KMPlatform.Entity.Client> allPublishers = new List<KMPlatform.Entity.Client>();
        List<KMPlatform.Object.Product> allPublications = new List<KMPlatform.Object.Product>();
        List<FrameworkUAD.Entity.ResponseGroup> allResponseGroup = new List<FrameworkUAD.Entity.ResponseGroup>();
        List<FrameworkUAD.Entity.ReportGroups> allReportGroups = new List<FrameworkUAD.Entity.ReportGroups>();

        KMPlatform.Object.Product currentPublication = new KMPlatform.Object.Product();
        FrameworkUAD.Entity.ResponseGroup currentResponseGroup = new FrameworkUAD.Entity.ResponseGroup();
        FrameworkUAD.Entity.ReportGroups currentReportGroup = new FrameworkUAD.Entity.ReportGroups();
        #endregion
        public ReportGroups()
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
                svPub = pWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                //svPublication = pubWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                svResponseGroup = rWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svReportGroups = rgWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Load Publications
                allPublications = new List<KMPlatform.Object.Product>();
                foreach (var p in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups)
                {
                    foreach (var c in p.Clients)
                    {
                        foreach (var a in c.Products)
                        {
                            if (!allPublications.Select(x=> x.ProductCode).Contains(a.ProductCode))
                                allPublications.Add(a);
                        }
                    }
                }

                cbMagazine.ItemsSource = allPublications.Where(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID).OrderBy(x=> x.ProductCode).ToList();
                cbMagazine.SelectedValuePath = "ProductID";
                cbMagazine.DisplayMemberPath = "ProductCode";

                if (cbMagazine.SelectedValue != null)
                {
                    int PublicationID = 0;
                    int.TryParse(cbMagazine.SelectedValue.ToString(), out PublicationID);
                    currentPublication = allPublications.FirstOrDefault(x => x.ProductID == PublicationID);
                }
                #endregion
                #region Load Response Group
                if (svResponseGroup.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allResponseGroup = svResponseGroup.Result;
                    if (currentPublication != null)
                    {
                        cbMagazine.SelectedItem = currentPublication;
         
                        cbGroup.ItemsSource = allResponseGroup.Where(x => x.PubID == currentPublication.ProductID).ToList();
                        cbGroup.SelectedValuePath = "ResponseGroupID";
                        cbGroup.DisplayMemberPath = "ResponseGroupName";
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }
                #endregion
                #region Load Report Groups
                if (svReportGroups.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allReportGroups = svReportGroups.Result;
                    if (cbGroup.SelectedValue != null)
                    {
                        int RTID = 0;
                        int.TryParse(cbGroup.SelectedValue.ToString(), out RTID);
                        if (RTID > 0)
                        {
                            gridReport.ItemsSource = null;
                            gridReport.ItemsSource = allReportGroups.Where(x => x.ResponseTypeID == RTID);
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

        private void RefreshGrid(int rspID)
        {
            svReportGroups = rgWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (svReportGroups.Result != null && svReportGroups.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                allReportGroups = svReportGroups.Result;
                gridReport.ItemsSource = allReportGroups.Where(x => x.ResponseGroupID == rspID);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.ReportGroups))
                {
                    FrameworkUAD.Entity.ReportGroups rItem = (FrameworkUAD.Entity.ReportGroups)b.DataContext;
                    if (rItem != null)
                    {
                        currentReportGroup = rItem;

                        tbxDisplayName.Text = rItem.DisplayName;

                        btnSave.Tag = rItem.ReportGroupID.ToString();
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
            currentReportGroup = null;

            //Clear control
            tbxDisplayName.Text = "";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region Initialize and Check
            int ReportGroupID = 0;
            if (btnSave.Tag != null)
                int.TryParse(btnSave.Tag.ToString(), out ReportGroupID);

            string Name = tbxDisplayName.Text;

            if (string.IsNullOrEmpty(Name))
            {
                Core_AMS.Utilities.WPF.Message("Name not provided. Please provide name.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }

            if (ReportGroupID > 0)
            {
                //Check not value existence name and code based by client
                if (allReportGroups.FirstOrDefault(x => x.DisplayName.Equals(Name, StringComparison.CurrentCultureIgnoreCase) && x.ReportGroupID != ReportGroupID) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Name currently exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                    return;
                }
            }
            else
            {
                //Check not value existence name and code
                if (allReportGroups.FirstOrDefault(x => x.DisplayName.Equals(Name, StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Name currently exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                    return;
                }
            }

            int ResponseTypeID = 0;
            if (cbGroup.SelectedValue != null)
            {
                int.TryParse(cbGroup.SelectedValue.ToString(), out ResponseTypeID);
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Group must be selected before saving.", "Invalid Submission");
                return;
            }
            #endregion
            #region Prepare
            if (ReportGroupID == 0)
            {
                currentReportGroup = new FrameworkUAD.Entity.ReportGroups();
                currentReportGroup.ReportGroupID = ReportGroupID;
                currentReportGroup.DisplayName = Name;
                currentReportGroup.ResponseGroupID = ResponseTypeID;
                currentReportGroup.DisplayOrder = allReportGroups.Count + 1;
            }
            else
            {
                currentReportGroup.ReportGroupID = ReportGroupID;
                currentReportGroup.DisplayName = Name;
                currentReportGroup.ResponseGroupID = ResponseTypeID;
            }
            #endregion
            #region Save|Update
            FrameworkUAS.Service.Response<int> svSave = new FrameworkUAS.Service.Response<int>();
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svSave = rgWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, currentReportGroup);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Refresh|Clear
                if (svSave != null && svSave.Result > 0 && svSave.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    //Set currentItem to null

                    //Change Button tag to zero content back to save
                    btnSave.Tag = "0";
                    btnSave.Content = "Save";

                    //Clear control                    
                    tbxDisplayName.Text = "";                    

                    //Refresh Grid
                    //LoadData();
                    RefreshGrid(currentReportGroup.ResponseGroupID);
                    currentReportGroup = null;

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

        private void cbMagazine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMagazine.SelectedValue != null)
            {
                int PID = 0;
                int.TryParse(cbMagazine.SelectedValue.ToString(), out PID);
                currentPublication = allPublications.FirstOrDefault(x => x.ProductID == PID);
                cbGroup.ItemsSource = null;
                cbGroup.ItemsSource = allResponseGroup.Where(x => x.PubID == PID).ToList();
                cbGroup.SelectedValuePath = "ResponseGroupID";
                cbGroup.DisplayMemberPath = "ResponseGroupName";

                gridReport.ItemsSource = null;
            }
        }

        private void cbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbGroup.SelectedValue != null)
            {
                int PID = 0;
                int.TryParse(cbGroup.SelectedValue.ToString(), out PID);
                currentResponseGroup = allResponseGroup.FirstOrDefault(x => x.PubID == currentPublication.ProductID && x.ResponseGroupID == PID);

                gridReport.ItemsSource = null;
                gridReport.ItemsSource = allReportGroups.Where(x => x.ResponseGroupID == currentResponseGroup.ResponseGroupID);
            }
        }
    }
}
