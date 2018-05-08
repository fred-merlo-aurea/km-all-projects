using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FileMapperWizard.Modules
{
    /// <summary>
    /// Interaction logic for FileAudit.xaml
    /// </summary>
    public partial class FileAudit : UserControl
    {
        #region Services
        FrameworkServices.ServiceClient<UAS_WS.Interface.ISourceFile> sourceFileWorker = FrameworkServices.ServiceClient.UAS_SourceFileClient();

        FrameworkServices.ServiceClient<UAD_WS.Interface.IFileAudit> fileAuditWorker = FrameworkServices.ServiceClient.UAD_FileAuditClient();

        FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriberOriginal> subOrigWorker = FrameworkServices.ServiceClient.UAD_SubscriberOriginalClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriberTransformed> subTranWorker = FrameworkServices.ServiceClient.UAD_SubscriberTransformedClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriberInvalid> subInvalidWorker = FrameworkServices.ServiceClient.UAD_SubscriberInvalidClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriberArchive> subArchWorker = FrameworkServices.ServiceClient.UAD_SubscriberArchiveClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriberFinal> subFinalWorker = FrameworkServices.ServiceClient.UAD_SubscriberFinalClient();

        FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriberDemographicOriginal> subOrigDemoWorker = FrameworkServices.ServiceClient.UAD_SubscriberDemographicOriginalClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriberDemographicTransformed> subTranDemoWorker = FrameworkServices.ServiceClient.UAD_SubscriberDemographicTransformedClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriberDemographicInvalid> subInvalidDemoWorker = FrameworkServices.ServiceClient.UAD_SubscriberDemographicInvalidClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriberDemographicArchive> subArchDemoWorker = FrameworkServices.ServiceClient.UAD_SubscriberDemographicArchiveClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriberDemographicFinal> subFinalDemoWorker = FrameworkServices.ServiceClient.UAD_SubscriberDemographicFinalClient();

        FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscription> subscriptionWorker = FrameworkServices.ServiceClient.UAD_SubscriptionClient();
        #endregion

        #region Variables
        private FrameworkUAS.Object.AppData myAppData = new FrameworkUAS.Object.AppData();
        private KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();        
        private List<FrameworkUAS.Entity.SourceFile> sourceList = new List<FrameworkUAS.Entity.SourceFile>();
        private List<FrameworkUAD.Object.FileAudit> processCodeToSourceFileList = new List<FrameworkUAD.Object.FileAudit>();

        private List<FrameworkUAD.Entity.SubscriberOriginal> soList = new List<FrameworkUAD.Entity.SubscriberOriginal>();
        private List<FrameworkUAD.Entity.SubscriberTransformed> stList = new List<FrameworkUAD.Entity.SubscriberTransformed>();
        private List<FrameworkUAD.Entity.SubscriberInvalid> siList = new List<FrameworkUAD.Entity.SubscriberInvalid>();
        private List<FrameworkUAD.Entity.SubscriberArchive> saList = new List<FrameworkUAD.Entity.SubscriberArchive>();
        private List<FrameworkUAD.Entity.SubscriberFinal> sfList = new List<FrameworkUAD.Entity.SubscriberFinal>();

        private List<FrameworkUAD.Entity.SubscriberDemographicOriginal> sdoList = new List<FrameworkUAD.Entity.SubscriberDemographicOriginal>();
        private List<FrameworkUAD.Entity.SubscriberDemographicTransformed> sdtList = new List<FrameworkUAD.Entity.SubscriberDemographicTransformed>();
        private List<FrameworkUAD.Entity.SubscriberDemographicInvalid> sdiList = new List<FrameworkUAD.Entity.SubscriberDemographicInvalid>();
        private List<FrameworkUAD.Entity.SubscriberDemographicArchive> sdaList = new List<FrameworkUAD.Entity.SubscriberDemographicArchive>();
        private List<FrameworkUAD.Entity.SubscriberDemographicFinal> sdfList = new List<FrameworkUAD.Entity.SubscriberDemographicFinal>();
                
        private List<FrameworkUAD.Entity.Subscription> subList = new List<FrameworkUAD.Entity.Subscription>();

        #region Service Results
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberOriginal>> svSO = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberOriginal>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberTransformed>> svST = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberTransformed>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberInvalid>> svSI = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberInvalid>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberArchive>> svSA = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberArchive>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberFinal>> svSF = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberFinal>>();

        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberDemographicOriginal>> svSDO = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberDemographicOriginal>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberDemographicTransformed>> svSDT = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberDemographicTransformed>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberDemographicInvalid>> svSDI = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberDemographicInvalid>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberDemographicArchive>> svSDA = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberDemographicArchive>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberDemographicFinal>> svSDF = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberDemographicFinal>>();        

        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Subscription>> svSUB = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Subscription>>();
        #endregion
        bool needsReDisplay;
        int oldSourceFileID = -1;
        string oldProcessCode = "";
        DateTime? oldStart = null;
        DateTime? oldEnd = null;
        #endregion

        public FileAudit(FrameworkUAS.Object.AppData appData)
        {            
            InitializeComponent();
            myAppData = appData;
            myClient = myAppData.AuthorizedUser.User.CurrentClient;
            needsReDisplay = true;
            HideGrid();
            LoadData();
        }

        #region Page Methods
        public void LoadData()
        {
            BackgroundWorker bw = new BackgroundWorker();
            busyIcon.IsBusy = true;
            bw.DoWork += (o, ea) =>
            {
                sourceList = sourceFileWorker.Proxy.Select(myAppData.AuthorizedUser.AuthAccessKey, myClient.ClientID, false).Result.ToList();
                processCodeToSourceFileList = fileAuditWorker.Proxy.SelectDistinctProcessCodePerSourceFile(myAppData.AuthorizedUser.AuthAccessKey, myClient.ClientConnections).Result.ToList();                
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Load File Audit Locations
                Dictionary<string, string> dates = new Dictionary<string, string>();
                foreach (Core_AMS.Utilities.Enums.FileAuditLocations fal in (Core_AMS.Utilities.Enums.FileAuditLocations[])Enum.GetValues(typeof(Core_AMS.Utilities.Enums.FileAuditLocations)))
                {
                    dates.Add(fal.ToString(), fal.ToString());
                }
                cbTable.ItemsSource = dates;
                cbTable.SelectedValuePath = "Key";
                cbTable.DisplayMemberPath = "Value";
                #endregion
                #region Set ItemsSource
                cbFiles.ItemsSource = sourceList.OrderBy(x => x.FileName);
                cbFiles.SelectedValuePath = "SourceFileID";
                cbFiles.DisplayMemberPath = "FileName";
                #endregion                
                busyIcon.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }
        
        private void DisplayGrid(string location)
        {
            HideGrid();
            if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Original.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                grdOriginal.Visibility = System.Windows.Visibility.Visible;
                pagerOriginal.Visibility = System.Windows.Visibility.Visible;
                grdDemoOriginal.Visibility = System.Windows.Visibility.Visible;
                pagerDemoOriginal.Visibility = System.Windows.Visibility.Visible;
            }
            else if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Transformed.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                grdTransformed.Visibility = System.Windows.Visibility.Visible;
                pagerTransformed.Visibility = System.Windows.Visibility.Visible;
                grdDemoTransformed.Visibility = System.Windows.Visibility.Visible;
                pagerDemoTransformed.Visibility = System.Windows.Visibility.Visible;
            }
            else if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Invalid.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                grdInvalid.Visibility = System.Windows.Visibility.Visible;
                pagerInvalid.Visibility = System.Windows.Visibility.Visible;
                grdDemoInvalid.Visibility = System.Windows.Visibility.Visible;
                pagerDemoInvalid.Visibility = System.Windows.Visibility.Visible;
            }
            else if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Archive.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                grdArchive.Visibility = System.Windows.Visibility.Visible;
                pagerArchive.Visibility = System.Windows.Visibility.Visible;
                grdDemoArchive.Visibility = System.Windows.Visibility.Visible;
                pagerDemoArchive.Visibility = System.Windows.Visibility.Visible;
            }
            else if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Final.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                grdFinal.Visibility = System.Windows.Visibility.Visible;
                pagerFinal.Visibility = System.Windows.Visibility.Visible;
                grdDemoFinal.Visibility = System.Windows.Visibility.Visible;
                pagerDemoFinal.Visibility = System.Windows.Visibility.Visible;
            }
            else if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Subscription.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                grdSubscriptions.Visibility = System.Windows.Visibility.Visible;
                pagerSubscriptions.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void HideGrid()
        {
            grdOriginal.Visibility = System.Windows.Visibility.Collapsed;
            pagerOriginal.Visibility = System.Windows.Visibility.Collapsed;
            grdTransformed.Visibility = System.Windows.Visibility.Collapsed;
            pagerTransformed.Visibility = System.Windows.Visibility.Collapsed;
            grdInvalid.Visibility = System.Windows.Visibility.Collapsed;
            pagerInvalid.Visibility = System.Windows.Visibility.Collapsed;
            grdArchive.Visibility = System.Windows.Visibility.Collapsed;
            pagerArchive.Visibility = System.Windows.Visibility.Collapsed;
            grdFinal.Visibility = System.Windows.Visibility.Collapsed;
            pagerFinal.Visibility = System.Windows.Visibility.Collapsed;

            grdDemoOriginal.Visibility = System.Windows.Visibility.Collapsed;
            pagerDemoOriginal.Visibility = System.Windows.Visibility.Collapsed;
            grdDemoTransformed.Visibility = System.Windows.Visibility.Collapsed;
            pagerDemoTransformed.Visibility = System.Windows.Visibility.Collapsed;
            grdDemoInvalid.Visibility = System.Windows.Visibility.Collapsed;
            pagerDemoInvalid.Visibility = System.Windows.Visibility.Collapsed;
            grdDemoArchive.Visibility = System.Windows.Visibility.Collapsed;
            pagerDemoArchive.Visibility = System.Windows.Visibility.Collapsed;
            grdDemoFinal.Visibility = System.Windows.Visibility.Collapsed;
            pagerDemoFinal.Visibility = System.Windows.Visibility.Collapsed;
            grdSubscriptions.Visibility = System.Windows.Visibility.Collapsed;
            pagerSubscriptions.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void GrabData()
        {
            #region Error Insufficent Data
            if (cbFiles.SelectedValue == null && cbProcessCode.SelectedValue == null && string.IsNullOrEmpty(rdpStart.SelectedValue.ToString()))
            {
                Core_AMS.Utilities.WPF.Message("Insufficent display criteria. Select a file, process code, and/or start date to continue.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Required Data");
                return;
            }
            else if (!string.IsNullOrEmpty(rdpEnd.SelectedValue.ToString()) && string.IsNullOrEmpty(rdpStart.SelectedValue.ToString()))
            {
                Core_AMS.Utilities.WPF.Message("Start Date was not provided. Please select a date before you continue.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Required Data");
                return;
            }
            else if (string.IsNullOrEmpty(rdpStart.SelectedValue.ToString()) && cbFiles.SelectedValue == null)
            {
                if (cbProcessCode.SelectedValue != null)
                {
                    Core_AMS.Utilities.WPF.Message("Start Date was not provided. Please select a date before you continue.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Required Data");
                    return;
                }
            }
            #endregion
            #region Set Data
            int sourceFileID = -1;
            if (cbFiles.SelectedValue != null)
                int.TryParse(cbFiles.SelectedValue.ToString(), out sourceFileID);

            string processCode = "";
            if (cbProcessCode.SelectedValue != null)
                processCode = cbProcessCode.SelectedValue.ToString();

            DateTime? start = null;
            if (!string.IsNullOrEmpty(rdpStart.SelectedValue.ToString()))
                start = DateTime.Parse(rdpStart.SelectedValue.ToString());                            

            DateTime? end = null;
            if (!string.IsNullOrEmpty(rdpEnd.SelectedValue.ToString()))
                end = DateTime.Parse(rdpEnd.SelectedValue.ToString());

            #region Data Location
            string location = "";
            if (cbTable.SelectedValue != null)
                location = cbTable.SelectedValue.ToString();

            if (location == "")
            {
                Core_AMS.Utilities.WPF.Message("You must select a data location before proceeding.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Required Data");
                return;
            }            
            #endregion
            if (oldSourceFileID != sourceFileID)
            {
                needsReDisplay = true;
                oldSourceFileID = sourceFileID;
            }
            else if (oldProcessCode != processCode)
            {
                needsReDisplay = true;
                oldProcessCode = processCode;
            }
            else if (oldStart != start)
            {
                needsReDisplay = true;
                oldStart = start;
            }
            else if (oldEnd != end)
            {
                needsReDisplay = true;
                oldEnd = end;
            }

            if (needsReDisplay)
            {
                ClearLists();
                needsReDisplay = false;
            }

            #endregion
            #region BackgroundWorker
            BackgroundWorker bw = new BackgroundWorker();
            busyIcon.IsBusy = true;
            bw.DoWork += (o, ea) =>
            {
                #region Load Lists
                if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Original.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!(soList.Count > 0))
                        svSO = subOrigWorker.Proxy.SelectForFileAudit(myAppData.AuthorizedUser.AuthAccessKey, processCode, sourceFileID, start, end, myClient.ClientConnections);

                    if (!(sdoList.Count > 0))
                        svSDO = subOrigDemoWorker.Proxy.SelectForFileAudit(myAppData.AuthorizedUser.AuthAccessKey, processCode, sourceFileID, start, end, myClient.ClientConnections);
                }
                else if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Transformed.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!(stList.Count > 0))
                        svST = subTranWorker.Proxy.SelectForFileAudit(myAppData.AuthorizedUser.AuthAccessKey, processCode, sourceFileID, start, end, myClient.ClientConnections);

                    if (!(sdtList.Count > 0))
                        svSDT = subTranDemoWorker.Proxy.SelectForFileAudit(myAppData.AuthorizedUser.AuthAccessKey, processCode, sourceFileID, start, end, myClient.ClientConnections);
                }
                else if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Invalid.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!(siList.Count > 0))
                        svSI = subInvalidWorker.Proxy.SelectForFileAudit(myAppData.AuthorizedUser.AuthAccessKey, processCode, sourceFileID, start, end, myClient.ClientConnections);

                    if (!(sdiList.Count > 0))
                        svSDI = subInvalidDemoWorker.Proxy.SelectForFileAudit(myAppData.AuthorizedUser.AuthAccessKey, processCode, sourceFileID, start, end, myClient.ClientConnections);
                }
                else if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Archive.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!(saList.Count > 0))
                        svSA = subArchWorker.Proxy.SelectForFileAudit(myAppData.AuthorizedUser.AuthAccessKey, processCode, sourceFileID, start, end, myClient.ClientConnections);

                    if (!(sdaList.Count > 0))
                        svSDA = subArchDemoWorker.Proxy.SelectForFileAudit(myAppData.AuthorizedUser.AuthAccessKey, processCode, sourceFileID, start, end, myClient.ClientConnections);
                }
                else if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Final.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!(sfList.Count > 0))
                        svSF = subFinalWorker.Proxy.SelectForFileAudit(myAppData.AuthorizedUser.AuthAccessKey, processCode, sourceFileID, start, end, myClient.ClientConnections);

                    if (!(sdfList.Count > 0))
                        svSDF = subFinalDemoWorker.Proxy.SelectForFileAudit(myAppData.AuthorizedUser.AuthAccessKey, processCode, sourceFileID, start, end, myClient.ClientConnections);
                }
                else if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Subscription.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!(subList.Count > 0))
                        svSUB = subscriptionWorker.Proxy.SelectForFileAudit(myAppData.AuthorizedUser.AuthAccessKey, processCode, sourceFileID, start, end, myClient.ClientConnections);
                }                     
                #endregion
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                #region Set Grids
                #region ORIGINAL
                if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Original.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if (svSO.Result != null && svSO.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        soList = svSO.Result;
                        grdOriginal.ItemsSource = soList;
                    }
                    if (svSDO.Result != null && svSDO.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        sdoList = svSDO.Result;
                        grdDemoOriginal.ItemsSource = sdoList;
                    }
                }
                #endregion
                #region TRANSFORMED
                if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Transformed.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if (svST.Result != null && svST.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        stList = svST.Result;
                        grdTransformed.ItemsSource = stList;
                    }
                    if (svSDT.Result != null && svSDT.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        sdtList = svSDT.Result;
                        grdDemoTransformed.ItemsSource = sdtList;
                    }
                }
                #endregion
                #region INVALID
                if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Invalid.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if (svSI.Result != null && svSI.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        siList = svSI.Result;
                        grdInvalid.ItemsSource = siList;
                    }
                    if (svSDI.Result != null && svSDI.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        sdiList = svSDI.Result;
                        grdDemoInvalid.ItemsSource = sdiList;
                    }
                }
                #endregion
                #region ARCHIVE
                if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Archive.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if (svSA.Result != null && svSA.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        saList = svSA.Result;
                        grdArchive.ItemsSource = saList;
                    }
                    if (svSDA.Result != null && svSDA.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        sdaList = svSDA.Result;
                        grdDemoArchive.ItemsSource = sdaList;
                    }
                }
                #endregion
                #region FINAL
                if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Final.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if (svSF.Result != null && svSF.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        sfList = svSF.Result;
                        grdFinal.ItemsSource = sfList;
                    }
                    if (svSDF.Result != null && svSDF.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        sdfList = svSDF.Result;
                        grdDemoFinal.ItemsSource = sdfList;
                    }
                }
                #endregion
                #region SUB
                if (location.Equals(Core_AMS.Utilities.Enums.FileAuditLocations.Subscription.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if (svSUB.Result != null && svSUB.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        subList = svSUB.Result;
                        grdSubscriptions.ItemsSource = subList;
                    }
                }
                #endregion                
                #endregion
                if (cbTable.SelectedValue != null)
                    DisplayGrid(cbTable.SelectedValue.ToString());

                needsReDisplay = false;
                busyIcon.IsBusy = false;
            };
            bw.RunWorkerAsync();
            #endregion
        }
     
        private void ClearLists()
        {
            #region Clear Lists
            soList.Clear();
            stList.Clear();
            siList.Clear();
            saList.Clear();
            sfList.Clear();
            sdoList.Clear();
            sdtList.Clear();
            sdiList.Clear();
            sdaList.Clear();
            sdfList.Clear();
            subList.Clear();
            #endregion
        }
        #endregion

        #region Combobox Controls
        private void cbFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFiles.SelectedValue != null)
            {
                int sfID = 0;
                int.TryParse(cbFiles.SelectedValue.ToString(), out sfID);
                #region
                cbProcessCode.ItemsSource = processCodeToSourceFileList.Where(x => x.SourceFileID == sfID).ToList();
                cbProcessCode.SelectedValuePath = "ProcessCode";
                cbProcessCode.DisplayMemberPath = "ProcessCode";
                #endregion
            }
        }
        
        private void cbTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTable.SelectedValue != null)
            {
                GrabData();
            }
        }
        #endregion

        #region Button Controls
        private void btnDisplay_Click(object sender, RoutedEventArgs e)
        {
            GrabData();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cbFiles.SelectedIndex = -1;
            cbProcessCode.SelectedIndex = -1;
            cbTable.SelectedIndex = -1;
            rdpStart.SelectedValue = null;
            rdpEnd.SelectedValue = null;
            ClearLists();
            #region Reset Grids
            grdOriginal.ItemsSource = null;
            grdTransformed.ItemsSource = null;
            grdInvalid.ItemsSource = null;
            grdArchive.ItemsSource = null;
            grdFinal.ItemsSource = null;
            grdDemoOriginal.ItemsSource = null;
            grdDemoTransformed.ItemsSource = null;
            grdDemoInvalid.ItemsSource = null;
            grdDemoArchive.ItemsSource = null;
            grdDemoFinal.ItemsSource = null;
            grdSubscriptions.ItemsSource = null;
            #endregion
            needsReDisplay = true;
            HideGrid();
            Core_AMS.Utilities.WPF.Message("Reset Complete.", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, "Reset Complete");
        }
        
        private void btnClearStart_Click(object sender, RoutedEventArgs e)
        {
            rdpStart.SelectedValue = null;            
        }

        private void btnClearEnd_Click(object sender, RoutedEventArgs e)
        {
            rdpEnd.SelectedValue = null;
        }
        #endregion                        
    }
}
