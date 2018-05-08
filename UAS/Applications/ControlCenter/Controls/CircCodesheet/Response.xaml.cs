using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Controls.CircCodesheet
{
    /// <summary>
    /// Interaction logic for Response.xaml
    /// </summary>
    public partial class Response : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> pWorker = FrameworkServices.ServiceClient.UAS_ClientClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.ICodeSheet> rWorker = FrameworkServices.ServiceClient.UAD_CodeSheetClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> rtWorker = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IReportGroups> rgWorker = FrameworkServices.ServiceClient.UAD_ReportGroupsClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> svPub = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svPublication = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>> svResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> svResponseGroup = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ReportGroups>> svReportGroup = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ReportGroups>>();

        List<KMPlatform.Entity.Client> allPublishers = new List<KMPlatform.Entity.Client>();
        List<KMPlatform.Object.Product> allPublications = new List<KMPlatform.Object.Product>();
        List<FrameworkUAD.Entity.CodeSheet> allResponse = new List<FrameworkUAD.Entity.CodeSheet>();
        List<FrameworkUAD.Entity.ResponseGroup> allResponseGroup = new List<FrameworkUAD.Entity.ResponseGroup>();
        List<FrameworkUAD.Entity.ReportGroups> allReportGroups = new List<FrameworkUAD.Entity.ReportGroups>(); 

        KMPlatform.Entity.Client currentPublisher = new KMPlatform.Entity.Client();
        KMPlatform.Object.Product currentPublication = new KMPlatform.Object.Product();
        FrameworkUAD.Entity.CodeSheet currentResponse = new FrameworkUAD.Entity.CodeSheet();
        FrameworkUAD.Entity.ResponseGroup currentResponseGroup = new FrameworkUAD.Entity.ResponseGroup();
        #endregion
        public Response(int PublicationID = 0, int PublisherID = 0)
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();
            LoadData(PublicationID, PublisherID);
        }

        public void LoadData(int PublicationID = 0, int PublisherID = 0)
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svPub = pWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                //svPublication = pubWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                svResponse = rWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svResponseGroup = rtWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svReportGroup = rgWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Load Publishers
                if (svPub.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allPublishers = svPub.Result;
                    cbPublisher.ItemsSource = null;
                    cbPublisher.ItemsSource = allPublishers.Where(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID).ToList();
                    cbPublisher.SelectedValuePath = "ClientID";
                    cbPublisher.DisplayMemberPath = "DisplayName";

                    if (PublisherID > 0)
                    {
                        currentPublisher = allPublishers.FirstOrDefault(x => x.ClientID == PublisherID);
                        //cbPublisher.SelectedValue = currentPublisher;
                    }
                    else if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0)
                    {
                        currentPublisher = allPublishers.FirstOrDefault(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }
                #endregion
                #region Load Publications
                //if (svPublication.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //{
                allPublications = new List<KMPlatform.Object.Product>();
                foreach (var p in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups)
                {
                    foreach (var c in p.Clients)
                    {
                        foreach (var a in c.Products)
                        {
                            if (!allPublications.Contains(a))
                                allPublications.Add(a);
                        }
                    }
                }
                if (currentPublisher != null)
                {
                    cbPublisher.SelectedItem = currentPublisher;

                    cbMagazine.ItemsSource = null;
                    cbMagazine.ItemsSource = allPublications.Where(x => x.ClientID == currentPublisher.ClientID).ToList(); ;
                    cbMagazine.SelectedValuePath = "ProductID";
                    cbMagazine.DisplayMemberPath = "ProductName";

                    if (PublicationID > 0)
                    {
                        currentPublication = allPublications.FirstOrDefault(x => x.ProductID == PublicationID);
                        cbMagazine.SelectedValue = currentPublication;
                        cbPublisher.SelectedItem = allPublishers.FirstOrDefault(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
                    }
                }
                //}
                //else
                //{
                //    Core_AMS.Utilities.WPF.MessageServiceError();
                //}
                #endregion
                #region Load Responses
                if (svResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allResponse = svResponse.Result;
                    //if (currentPublication != null)
                    //{                    
                    //    cbGroup.ItemsSource = allResponse.Where(x => x.PublicationID == currentPublication.PublicationID).ToList();
                    //    cbGroup.SelectedValuePath = "PublicationID";
                    //    cbGroup.DisplayMemberPath = "PublicationName";
                    //}
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }
                #endregion
                #region Load ResponseGroups
                if (svResponseGroup.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allResponseGroup = svResponseGroup.Result;
                    if (currentPublication != null)
                    {
                        cbGroup.ItemsSource = null;
                        cbGroup.ItemsSource = allResponseGroup.Where(x => x.PubID == currentPublication.ProductID).ToList();
                        cbGroup.SelectedValuePath = "ResponseGroupID";
                        cbGroup.DisplayMemberPath = "DisplayName";

                        cbMagazine.SelectedItem = allPublications.FirstOrDefault(x => x.ProductID == currentPublication.ProductID);
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                }
                #endregion
                #region Load Report Groups
                if (svReportGroup.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    allReportGroups = svReportGroup.Result;
                    cbReportGroup.ItemsSource = allReportGroups;
                    cbReportGroup.SelectedValuePath = "ReportGroupID";
                    cbReportGroup.DisplayMemberPath = "DisplayName";
                    if (currentResponseGroup != null)
                    {
                        cbGroup.SelectedItem = currentResponseGroup;
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.CodeSheet))
                {
                    FrameworkUAD.Entity.CodeSheet rItem = (FrameworkUAD.Entity.CodeSheet)b.DataContext;
                    if (rItem != null)
                    {
                        currentResponse = rItem;

                        cbReportGroup.SelectedItem = allReportGroups.FirstOrDefault(x => x.ReportGroupID ==rItem.ReportGroupID);
                        tbxValue.Text = rItem.ResponseValue;
                        tbxDesc.Text = rItem.ResponseDesc;

                        btnSave.Tag = rItem.CodeSheetID.ToString();
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
            currentResponse = null;

            //Clear control
            cbReportGroup.SelectedIndex = -1;
            tbxValue.Text = "";
            tbxDesc.Text = "";

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region Set Variables and Check
            int ResponseID = 0;
            if (btnSave.Tag != null)
                int.TryParse(btnSave.Tag.ToString(), out ResponseID);

            int ReportID = 0;
            if (cbReportGroup.SelectedValue != null)
                int.TryParse(cbReportGroup.SelectedValue.ToString(), out ReportID);

            string Value = tbxValue.Text;
            string Desc = tbxDesc.Text;

            int ResponseGroupID = 0;
            string ResponseName = "";
            if (cbGroup.SelectedValue != null)
            {
                int.TryParse(cbGroup.SelectedValue.ToString(), out ResponseGroupID);
                ResponseName = cbGroup.Text;
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Please provide a group.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                return;
            }

            int PublicationID = 0;
            if (cbMagazine.SelectedValue != null)
                int.TryParse(cbMagazine.SelectedValue.ToString(), out PublicationID);
            else
            {
                Core_AMS.Utilities.WPF.Message("Please provide a magazine.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                return;
            }
            #endregion

            if (!string.IsNullOrEmpty(Value) && !string.IsNullOrEmpty(Desc))
            {
                #region Check Values
                if (ResponseID > 0)
                {
                    //Check not value existence name and code based by client
                    if (allResponse.FirstOrDefault(x => x.ResponseValue.Equals(Value, StringComparison.CurrentCultureIgnoreCase) && x.PubID == currentResponse.PubID && x.CodeSheetID != ResponseID) != null)
                    {
                        Core_AMS.Utilities.WPF.Message("Name currently exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                        return;
                    }
                }
                else
                {
                    //Check not value existence name and code
                    if (allResponse.FirstOrDefault(x => x.ResponseValue.Equals(Value, StringComparison.CurrentCultureIgnoreCase) && x.PubID == currentResponse.PubID) != null)
                    {
                        Core_AMS.Utilities.WPF.Message("Name currently exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                        return;
                    }
                }
                #endregion

                #region Prepare Item Save|Update
                if (ResponseID > 0)
                {
                    currentResponse.CodeSheetID = ResponseID;
                    currentResponse.ResponseValue = Value;
                    currentResponse.ResponseDesc = Desc;
                    currentResponse.ReportGroupID = ReportID;
                    currentResponse.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    currentResponse.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    currentResponse.DateCreated = DateTime.Now;
                    currentResponse.DateUpdated = DateTime.Now;
                }
                else
                {
                    currentResponse = new FrameworkUAD.Entity.CodeSheet();
                    currentResponse.CodeSheetID = ResponseID;
                    currentResponse.ResponseGroupID = ResponseGroupID;
                    currentResponse.PubID = PublicationID;
                    currentResponse.ResponseGroup = ResponseName;
                    currentResponse.ResponseValue = Value;
                    currentResponse.ResponseDesc = Desc;
                    currentResponse.DisplayOrder = allResponse.Where(x => x.PubID == PublicationID && x.ResponseGroupID == ResponseGroupID).ToList().Count() + 1;
                    currentResponse.ReportGroupID = ReportID;
                    currentResponse.IsActive = true;
                    currentResponse.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    currentResponse.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    currentResponse.DateCreated = DateTime.Now;
                    currentResponse.DateUpdated = DateTime.Now;                    
                }

                FrameworkUAS.Service.Response<int> svSave = new FrameworkUAS.Service.Response<int>();
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (o, ea) =>
                {
                    svSave = rWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, currentResponse, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                };
                bw.RunWorkerCompleted += (o, ea) =>
                {
                    #region Refresh|Clear
                    if (svSave != null && svSave.Result > 0 && svSave.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        //Set currentItem to null
                        currentResponse = null;

                        //Change Button tag to zero content back to save
                        btnSave.Tag = "0";
                        btnSave.Content = "Save";

                        //Clear control
                        cbReportGroup.SelectedIndex = -1;
                        tbxValue.Text = "";
                        tbxDesc.Text = "";

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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Button))
            {
                Button b = (Button)e.OriginalSource;
                if (b.DataContext.GetType() == typeof(FrameworkUAD.Entity.CodeSheet))
                {
                    FrameworkUAD.Entity.CodeSheet rItem = (FrameworkUAD.Entity.CodeSheet)b.DataContext;
                    if (rItem != null)
                    {
                        FrameworkUAS.Service.Response<bool> svDelete = new FrameworkUAS.Service.Response<bool>();
                        BackgroundWorker bw = new BackgroundWorker();
                        bw.DoWork += (o, ea) =>
                        {
                            svDelete = rWorker.Proxy.DeleteCodeSheetID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, rItem.CodeSheetID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        };
                        bw.RunWorkerCompleted += (o, ea) =>
                        {
                            #region Refresh|Clear
                            if (svDelete != null && svDelete.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                            {
                                //Set currentItem to null
                                currentResponse = null;

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

        private void cbPublisher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPublisher.SelectedValue != null)
            {
                int PID = 0;
                int.TryParse(cbPublisher.SelectedValue.ToString(), out PID);
                currentPublisher = allPublishers.FirstOrDefault(x => x.ClientID == PID);

                cbMagazine.ItemsSource = null;
                cbMagazine.ItemsSource = allPublications.Where(x => x.ClientID == PID).ToList(); ;
                cbMagazine.SelectedValuePath = "ProductID";
                cbMagazine.DisplayMemberPath = "ProductCode";
            }
        }

        private void cbMagazine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMagazine.SelectedValue != null)
            {
                int PID = 0;
                int.TryParse(cbMagazine.SelectedValue.ToString(), out PID);
                currentPublication = allPublications.FirstOrDefault(x => x.ProductID == PID);

                cbGroup.ItemsSource = null;
                cbGroup.ItemsSource = allResponseGroup.Where(x => x.PubID == currentPublication.ProductID).ToList();
                cbGroup.SelectedValuePath = "ResponseGroupID";
                cbGroup.DisplayMemberPath = "ResponseGroupName";

                gridResponses.ItemsSource = null;
            }
        }

        private void cbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbGroup.SelectedValue != null)
            {
                int GID = 0;
                int.TryParse(cbGroup.SelectedValue.ToString(), out GID);
                currentResponseGroup = allResponseGroup.FirstOrDefault(x => x.ResponseGroupID == GID);
                gridResponses.ItemsSource = allResponse.Where(x => x.ResponseGroupID == GID);
            }
        }
    }
}
