using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core_AMS.Utilities;
using FileMapperWizard.Helpers;
using KM.Common.Functions;
using UADFeatures = KMPlatform.BusinessLogic.Enums.UADFeatures;
using FileTypes = FrameworkUAD_Lookup.Enums.FileTypes;
using Enums = KM.Common.Enums;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for EditSetup.xaml
    /// </summary>
    public partial class EditSetup : UserControl
    {
        private const string TrueValue = "Yes";
        private const string FalseValue = "No";
        private const string DelimiterMissingMessage = "Please provide a file delimiter and mark if the file contains quotation marks before continuing.";
        private const string DataMissingMessage = "Data is missing. Please provide a 'Save Filename As'.";
        private const string FileNameTooLongMessage = "Data invalid. 'Save Filename As' cannot exceed 50 characters in length. Please fix and continue.";
        private const string ErrorCaption = "Missing Data";
        private const int MaxFileNameLength = 50;
        private const string InvalidDataCaption = "Invalid Data";
        private const string FileAlreadyMappedMessage = "Records show the current file has been previously mapped. Please locate the file in Edit Mapping to make changes to it.";
        private const string FileAlreadyMappedCaption = "File Previously Mapped";
        private const string QuotationsUpdateRequiredMessage = "File Delimiter and/or File contains double quotation marks.  Please update Quotations selection before advancing.";
        private const string QuotationsUpdateRequiredCaption = "Missing File Info";
        private const string MissingDataMessage = "Data is missing. Please make sure client was selected and/or file was selected.";
        private const string MissingDataCaption = "Missing Data";
        private readonly ServiceClientSet serviceClientSet = new ServiceClientSet();
        
        #region VARIABLES
        FileMapperWizard.Modules.FMUniversal thisContainer;
        FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> svPublishers = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svPublications = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.SourceFile>> svSources = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.SourceFile>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> svCodes = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();

        List<FrameworkUAD_Lookup.Entity.Code> AllDatabaseFileCodes = new List<FrameworkUAD_Lookup.Entity.Code>();
        List<FrameworkUAS.Entity.SourceFile> AllSourceFiles = new List<FrameworkUAS.Entity.SourceFile>();
        List<KMPlatform.Entity.Client> clientList = new List<KMPlatform.Entity.Client>();
        List<KMPlatform.Object.Product> productList = new List<KMPlatform.Object.Product>();
        List<KMPlatform.Entity.ServiceFeature> currentClientFeatures { get; set; }
        bool initialLoad { get; set; }
        FrameworkUAD_Lookup.Entity.Code dcCode;
        #endregion

        public EditSetup(FileMapperWizard.Modules.FMUniversal container)
        {
            initialLoad = true;
            thisContainer = container;
            InitializeComponent();
            LoadData();
            initialLoad = false;
        }

        public void LoadData()
        {
            #region Load Client
            //TEMP PATCH
            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserName.Equals("LRoberto@NTMLLC.com"))
            {
                List<KMPlatform.Entity.Client> lrClients = new List<KMPlatform.Entity.Client>();
                foreach (var cg in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups)
                {
                    foreach(var c in cg.Clients)
                    {
                        if (!lrClients.Contains(c))
                            lrClients.Add(c);
                    }
                }
                thisContainer.AllClients = lrClients;
                rcbClients.ItemsSource = lrClients.OrderBy(x => x.ClientName).ToList();
                rcbClients.SelectedValuePath = "ClientID";
                rcbClients.DisplayMemberPath = "DisplayName";
                thisContainer.AllPublishers.AddRange(thisContainer.AllClients);
            }
            else
            {
                thisContainer.AllClients = serviceClientSet.Clients.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, false).Result;
                rcbClients.ItemsSource = thisContainer.AllClients.OrderBy(x => x.ClientName).ToList();
                rcbClients.SelectedValuePath = "ClientID";
                rcbClients.DisplayMemberPath = "DisplayName";
                thisContainer.AllPublishers.AddRange(thisContainer.AllClients);
            }
            //if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient != null && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > -1)
            //{
            //    KMPlatform.Entity.Client sc = thisContainer.AllClients.SingleOrDefault(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
            //    if (sc != null)
            //        rcbClients.SelectedItem = sc;

            //}
            FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> pubResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubData = FrameworkServices.ServiceClient.UAD_ProductClient();
            pubResponse = pubData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            thisContainer.AllPublications = pubResponse.Result;
            #endregion

            #region Services
            thisContainer.AllServices = serviceClientSet.Services.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, true).Result.Where(x => x.IsEnabled == true).ToList();
            #endregion

            #region Load SourceFiles
            svSources = serviceClientSet.SourceFile.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, false, false);

            if (svSources.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                if (thisContainer.myUADFeature == KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                {
                    dcCode = serviceClientSet.LookUpCode.Proxy.SelectCodeName(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Database_File, "Data Compare").Result;

                    AllSourceFiles = svSources.Result.Where(x => x.DatabaseFileTypeId == dcCode.CodeId).ToList();
                }
                else
                {
                    AllSourceFiles = svSources.Result;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.MessageServiceError();
            }
            #endregion
            
            #region CODE
            svCodes = serviceClientSet.LookUpCode.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping);
            List<FrameworkUAD_Lookup.Entity.Code> codes = svCodes.Result;
            thisContainer.standardTypeID = codes.FirstOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString())).CodeId;
            thisContainer.demoTypeID = codes.FirstOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString())).CodeId;
            thisContainer.ignoreTypeID = codes.FirstOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString())).CodeId;
            #endregion

            #region POPULATE File Recurrence
            rcbFrequency.ItemsSource = serviceClientSet.LookUpCode.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.File_Recurrence).Result;
            rcbFrequency.DisplayMemberPath = "DisplayName";
            rcbFrequency.SelectedValuePath = "CodeId";
            #endregion

            #region DatabaseFileTypes
            AllDatabaseFileCodes = serviceClientSet.LookUpCode.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Database_File).Result.Where(x => x.IsActive == true).ToList();
            rcbDatabaseFileType.ItemsSource = AllDatabaseFileCodes;
            thisContainer.DatabaseFileTypeList = AllDatabaseFileCodes.ToList();

            rcbDatabaseFileType.DisplayMemberPath = "DisplayName";
            rcbDatabaseFileType.SelectedValuePath = "CodeId";
            #endregion

            #region POPULATE QDate
            Dictionary<string, string> dates = new Dictionary<string, string>();
            foreach (DateFormat df in (DateFormat[])Enum.GetValues(typeof(DateFormat)))
            {
                dates.Add(df.ToString(), df.ToString());
            }
            rcbDateFormat.ItemsSource = dates;
            rcbDateFormat.SelectedValuePath = "Key";
            rcbDateFormat.DisplayMemberPath = "Value";
            rcbDateFormat.SelectedValue = DateFormat.MMDDYYYY.ToString();
            #endregion

            #region POPULATE THE DELIMITERS
            foreach (Enums.ColumnDelimiter dl in (Enums.ColumnDelimiter[])Enum.GetValues(typeof(Enums.ColumnDelimiter)))
            {
                rcbDelimiters.Items.Add(dl.ToString().Replace("_", " "));
            }
            #endregion

            #region POPULATE THE YES NO FOR QUOTE ENCAPSULATION
            rcbQuotations.Items.Add(TrueValue);
            rcbQuotations.Items.Add(FalseValue);
            #endregion

            #region POPULATE Extensions
            //Extension
            foreach (Core_AMS.Utilities.Enums.FileExtensions dl in (Core_AMS.Utilities.Enums.FileExtensions[])Enum.GetValues(typeof(Core_AMS.Utilities.Enums.FileExtensions)))
            {
                rcbExtension.Items.Add("." + dl.ToString().Replace("_", " ").ToUpper());
            }
            #endregion


            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient != null)
            {
                thisContainer.myClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
                rcbClients.SelectedItem = thisContainer.AllClients.FirstOrDefault(x => x.ClientID == thisContainer.myClient.ClientID);
            }
            
        }

        private void rcbClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            #region Client Selection
            int clientID = -1;
            if (rcbClients.SelectedValue != null)
                int.TryParse(rcbClients.SelectedValue.ToString(), out clientID);

            if (clientID > -1)
            {
                thisContainer.myClient = thisContainer.AllClients.FirstOrDefault(x => x.ClientID == clientID);
                List<FrameworkUAS.Entity.SourceFile> files = new List<FrameworkUAS.Entity.SourceFile>();
                if (thisContainer.isCirculation)
                {
                    var serv = thisContainer.AllServices.FirstOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    if (serv != null)
                        files = AllSourceFiles.Where(x => x.ClientID == clientID && x.IsDeleted == false && x.ServiceID == serv.ServiceID).ToList();
                    else
                        files = AllSourceFiles.Where(x => x.ClientID == clientID && x.IsDeleted == false).ToList();

                }
                else if (thisContainer.isCirculation == false)
                {
                    var serv = thisContainer.AllServices.FirstOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.UADFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    if (serv != null)
                        files = AllSourceFiles.Where(x => x.ClientID == clientID && x.IsDeleted == false && x.ServiceID == serv.ServiceID).ToList();
                    else
                        files = AllSourceFiles.Where(x => x.ClientID == clientID && x.IsDeleted == false).ToList();

                }

                rlbFiles.ItemsSource = files.OrderBy(x => x.FileName);
                rlbFiles.DisplayMemberPath = "FileName";
                rlbFiles.SelectedValuePath = "SourceFileID";
                rlbFiles.Visibility = Visibility.Visible;
                btnDeleteFile.Visibility = Visibility.Visible;
                txtSelectFile.Visibility = Visibility.Visible;
                Hide_Options();
            }
            else
            {
                if (initialLoad == false)
                    Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            #endregion
            #region Service Selection
            //LOAD FEATURES THAT ARE ENABLED FOR CLIENT
            int serviceID = 0;
            if (thisContainer.isCirculation)
                thisContainer.myService = thisContainer.AllServices.FirstOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
            else
                thisContainer.myService = thisContainer.AllServices.FirstOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.UADFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));

            if (thisContainer.myService != null)
            {
                serviceID = thisContainer.myService.ServiceID;
                KMPlatform.Entity.Service selectedService = thisContainer.myService;// bls.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, serviceID, true).Result;
                thisContainer.AllFeatures = serviceClientSet.ServiceFeature.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result.Where(x => x.IsEnabled == true).ToList();
                currentClientFeatures = new List<KMPlatform.Entity.ServiceFeature>();
                foreach (KMPlatform.Entity.ServiceFeature ss in selectedService.ServiceFeatures)
                {
                    if (selectedService.ServiceFeatures.Count > 0 && ss.IsEnabled)
                    {
                        //Outside User
                        if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 1 && ss.KMAdminOnly == false)
                            currentClientFeatures.Add(thisContainer.AllFeatures.FirstOrDefault(x => x.ServiceFeatureID == ss.ServiceFeatureID));

                        //KM Employee
                        if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID == 1)
                            currentClientFeatures.Add(thisContainer.AllFeatures.FirstOrDefault(x => x.ServiceFeatureID == ss.ServiceFeatureID));
                    }
                }
                rcbFeatures.ItemsSource = currentClientFeatures.Where(x => x.IsEnabled).OrderBy(x => x.SFName);
                rcbFeatures.DisplayMemberPath = "SFName";
                rcbFeatures.SelectedValuePath = "ServiceFeatureID";

                if (thisContainer.myFeatureID != null)
                    rcbFeatures.SelectedValue = thisContainer.myFeatureID.ToString();

            }
            else
                Core_AMS.Utilities.WPF.Message("No service was selected. Please report to appropriate personnel", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Service");

            #endregion
        }

        private void rlbFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int sfID = 0;
            if (rlbFiles.SelectedValue != null)
            {
                int.TryParse(rlbFiles.SelectedValue.ToString(), out sfID);
                FrameworkUAS.Entity.SourceFile me = AllSourceFiles.FirstOrDefault(x => x.SourceFileID == sfID);
                if (me != null)
                {
                    thisContainer.sourceFileID = me.SourceFileID;
                    thisContainer.qDateFormat = me.QDateFormat;
                    thisContainer.batchSize = me.BatchSize;
                    thisContainer.DatabaseFileType = me.DatabaseFileTypeId;
                    thisContainer.IsDeleted = me.IsDeleted;
                    thisContainer.IsIgnored = me.IsIgnored;
                    thisContainer.IsDQMReady = me.IsDQMReady;
                    thisContainer.MasterGroupID = me.MasterGroupID;
                    thisContainer.UseRealTimeGeocoding = me.UseRealTimeGeocoding;
                    thisContainer.IsSpecialFile = me.IsSpecialFile;
                    thisContainer.ClientCustomProcedureID = me.ClientCustomProcedureID;
                    thisContainer.SpecialFileResultID = me.SpecialFileResultID;
                    thisContainer.myService = thisContainer.AllServices.FirstOrDefault(x => x.ServiceID == me.ServiceID);
                    thisContainer.myFeatureID = me.ServiceFeatureID;
                    if (rcbFeatures.SelectedItem != null)
                        thisContainer.myUADFeature = KMPlatform.BusinessLogic.Enums.GetUADFeature(rcbFeatures.SelectedItem.ToString());
                    else
                    {
                        KMPlatform.Entity.ServiceFeature servFeat = currentClientFeatures.FirstOrDefault(x => x.ServiceFeatureID == me.ServiceFeatureID);
                        if (servFeat != null)
                            thisContainer.myUADFeature = KMPlatform.BusinessLogic.Enums.GetUADFeature(servFeat.SFName);
                        
                    }
                    thisContainer.myFileConfig.FileColumnDelimiter = me.Delimiter;
                    thisContainer.myFileConfig.IsQuoteEncapsulated = me.IsTextQualifier;
                    thisContainer.FileName = me.FileName + me.Extension;
                    thisContainer.srcFileTypeID = me.FileRecurrenceTypeId;
                    thisContainer.extension = me.Extension;

                    if (me.PublicationID > 0)
                    {
                        thisContainer.isCirculation = true;
                        thisContainer.currentPublication = thisContainer.AllPublications.FirstOrDefault(x => x.PubID == me.PublicationID);
                        thisContainer.currentPublisher = thisContainer.AllPublishers.FirstOrDefault(x => x.ClientID == thisContainer.currentPublication.ClientID);
                        rcbDatabaseFileType.SelectedValue = me.DatabaseFileTypeId;
                        txtSelectFileType.Visibility = Visibility.Visible;
                        rcbDatabaseFileType.Visibility = Visibility.Visible;
                        rcbFeatures.Visibility = Visibility.Visible;
                        txtSelectProcess.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        rcbFeatures.Visibility = Visibility.Visible;
                        txtSelectProcess.Visibility = Visibility.Visible;
                    }
                    
                    rlbFiles.Visibility = Visibility.Collapsed;
                    btnDeleteFile.Visibility = Visibility.Collapsed;
                    txtSelectFile.Visibility = Visibility.Collapsed;
                    btnRefresh.Visibility = Visibility.Collapsed;
                    KMPlatform.Entity.ServiceFeature sfMe = serviceClientSet.ServiceFeature.Proxy.SelectServiceFeature(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, me.ServiceFeatureID).Result;

                    rcbExtension.SelectedValue = me.Extension.ToUpper();

                    rcbFeatures.SelectedValue = sfMe.ServiceFeatureID;
                    rcbFrequency.SelectedValue = me.FileRecurrenceTypeId;
                    txtSaveName.Text = me.FileName;
                    txtFileName.Text = me.FileName;
                    txtDateCreated.Text = me.DateCreated.ToString();
                    rcbDateFormat.SelectedValue = me.QDateFormat;
                    spFileInfo.Visibility = Visibility.Visible;
                    //rcbFeatures.Visibility = Visibility.Visible;
                    //txtSelectProcess.Visibility = Visibility.Visible;
                    txtSelectFF.Visibility = Visibility.Visible;
                    rcbFrequency.Visibility = Visibility.Visible;
                    txtSelectQD.Visibility = Visibility.Visible;
                    rcbDateFormat.Visibility = Visibility.Visible;
                    txtBatchSize.Visibility = Visibility.Visible;
                    rnudBatchSize.Visibility = Visibility.Visible;
                    lblSaveName.Visibility = Visibility.Visible;
                    //borderSaveName.Visibility = Visibility.Visible;
                    txtSaveName.Visibility = Visibility.Visible;
                    btnStep1Next.Visibility = Visibility.Visible;
                    btnDuplicate.Visibility = Visibility.Visible;
                    lblExtension.Visibility = Visibility.Visible;
                    rcbExtension.Visibility = Visibility.Visible;

                    btnSelectDifferentFile.Visibility = Visibility.Visible;

                    if (me.Extension.ToLower() == ".csv" || me.Extension.ToLower() == ".txt")
                    {
                        if (me.IsTextQualifier == true)
                            rcbQuotations.SelectedItem = TrueValue;
                        else
                            rcbQuotations.SelectedItem = FalseValue;
                        rcbDelimiters.SelectedValue = me.Delimiter;

                        lblDelimiters.Visibility = Visibility.Visible;
                        lblQuotations.Visibility = Visibility.Visible;
                        rcbDelimiters.Visibility = Visibility.Visible;
                        rcbQuotations.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lblDelimiters.Visibility = Visibility.Collapsed;
                        lblQuotations.Visibility = Visibility.Collapsed;
                        rcbDelimiters.Visibility = Visibility.Collapsed;
                        rcbQuotations.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
        
        private void Select_Different_File(object sender, RoutedEventArgs e)
        {
            Hide_Options();
        }
        
        private void Hide_Options()
        {
            rlbFiles.Visibility = Visibility.Visible;
            btnDeleteFile.Visibility = Visibility.Visible;
            txtSelectFile.Visibility = Visibility.Visible;
            btnRefresh.Visibility = Visibility.Visible;
            rcbFeatures.Visibility = Visibility.Collapsed;
            txtSelectFileType.Visibility = Visibility.Collapsed;
            rcbDatabaseFileType.Visibility = Visibility.Collapsed;
            txtSelectProcess.Visibility = Visibility.Collapsed;
            txtSelectFF.Visibility = Visibility.Collapsed;
            rcbFrequency.Visibility = Visibility.Collapsed;
            txtSelectQD.Visibility = Visibility.Collapsed;
            rcbDateFormat.Visibility = Visibility.Collapsed;
            txtBatchSize.Visibility = Visibility.Collapsed;
            rnudBatchSize.Visibility = Visibility.Collapsed;
            lblSaveName.Visibility = Visibility.Collapsed;
            //borderSaveName.Visibility = Visibility.Collapsed;
            txtSaveName.Visibility = Visibility.Collapsed;
            lblDelimiters.Visibility = Visibility.Collapsed;
            lblQuotations.Visibility = Visibility.Collapsed;
            rcbDelimiters.Visibility = Visibility.Collapsed;
            rcbQuotations.Visibility = Visibility.Collapsed;
            lblExtension.Visibility = Visibility.Collapsed;
            rcbExtension.Visibility = Visibility.Collapsed;
            btnSelectDifferentFile.Visibility = Visibility.Collapsed;
            spFileInfo.Visibility = Visibility.Collapsed;
            btnStep1Next.Visibility = Visibility.Collapsed;
            btnDuplicate.Visibility = Visibility.Collapsed;
        }

        private void btnStep1Next_Click(object sender, RoutedEventArgs e)
        {
            if (CheckClientAndFileFilledOut() &&
                SaveFileAsChecks() &&
                SaveDataForNextTile())
            {
                SetupNextTile();
            }
        }

        private bool CheckClientAndFileFilledOut()
        {
            if (thisContainer.myClient != null && thisContainer.FileName != "")
            {
                thisContainer.fileInfo = new FileInfo(thisContainer.FileName);
                if (thisContainer.fw.IsExcelFile(thisContainer.fileInfo) ||
                    thisContainer.fw.IsDbfFile(thisContainer.fileInfo) ||
                    thisContainer.fw.IsZipFile(thisContainer.fileInfo) ||
                    thisContainer.fw.IsJsonFile(thisContainer.fileInfo) ||
                    thisContainer.fw.IsXmlFile(thisContainer.fileInfo))
                {
                    thisContainer.myFileConfig = null;
                }
                else
                {
                    if (rcbDelimiters.SelectedValue == null ||
                        rcbQuotations.SelectedValue == null ||
                        string.IsNullOrWhiteSpace(rcbDelimiters.SelectedValue.ToString()) ||
                        string.IsNullOrWhiteSpace(rcbQuotations.SelectedValue.ToString()))
                    {
                        WPF.Message(
                            QuotationsUpdateRequiredMessage,
                            MessageBoxButton.OK,
                            MessageBoxImage.Error,
                            MessageBoxResult.OK,
                            QuotationsUpdateRequiredCaption);
                        return false;
                    }

                    var isQuoted = rcbQuotations.SelectedValue.ToString().Trim()
                        .Equals(TrueValue, StringComparison.CurrentCultureIgnoreCase);
                    thisContainer.myFileConfig.FileColumnDelimiter = rcbDelimiters.SelectedValue.ToString().Trim();
                    thisContainer.myFileConfig.IsQuoteEncapsulated = isQuoted;
                }
            }
            else
            {
                WPF.Message(
                    MissingDataMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error,
                    MessageBoxResult.OK,
                    MissingDataCaption);
                return false;
            }

            return true;
        }

        private bool SaveFileAsChecks()
        {
            if (string.IsNullOrWhiteSpace(txtSaveName.Text))
            {
                WPF.Message(
                    DataMissingMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error, 
                    MessageBoxResult.OK,
                    ErrorCaption);
                return false;
            }

            if (txtSaveName.Text.Length > MaxFileNameLength)
            {
                WPF.Message(
                    FileNameTooLongMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error,
                    MessageBoxResult.OK,
                    InvalidDataCaption);
                return false;
            }

            var clientSources = serviceClientSet.SourceFile.Proxy
                .Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, false)
                .Result
                .Where(x => x.ClientID == thisContainer.myClient.ClientID && !x.IsDeleted)
                .ToList();
            var match = clientSources.FirstOrDefault(x =>
                x.FileName.Equals(
                    Path.GetFileNameWithoutExtension(thisContainer.saveFileName),
                    StringComparison.CurrentCultureIgnoreCase));
            if (match == null)
            {
                return true;
            }

            if (match.SourceFileID == thisContainer.sourceFileID)
            {
                return true;
            }

            WPF.Message(
                FileAlreadyMappedMessage,
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.OK,
                FileAlreadyMappedCaption);
            return false;
        }

        private bool SaveDataForNextTile()
        {
            thisContainer.extension = rcbExtension.SelectedValue.ToString();

            thisContainer.myDelimiter = string.Empty;
            if (rcbDelimiters.SelectedValue != null)
            {
                thisContainer.myDelimiter = rcbDelimiters.SelectedValue.ToString();
            }

            thisContainer.myTextQualifier = false;
            if (rcbQuotations.SelectedValue != null)
            {
                if (rcbQuotations.SelectedValue.ToString() == TrueValue)
                {
                    thisContainer.myTextQualifier = true;
                }
                else if (rcbQuotations.SelectedValue.ToString() == FalseValue)
                {
                    thisContainer.myTextQualifier = false;
                }
            }

            if (new [] { ".csv", ".txt" }.Contains(thisContainer.extension.ToLower()) &&
                string.IsNullOrWhiteSpace(thisContainer.myDelimiter))
            {
                WPF.MessageError(DelimiterMissingMessage);
                return false;
            }

            int sFileTypeID = 0;
            if (rcbFrequency.SelectedValue != null)
            {
                int.TryParse(rcbFrequency.SelectedValue.ToString(), out sFileTypeID);
                thisContainer.srcFileTypeID = sFileTypeID;
            }

            thisContainer.batchSize = rnudBatchSize.Value.HasValue ? Convert.ToInt32(rnudBatchSize.Value.Value) : 2500;

            if (rcbFeatures.SelectedValue != null)
            {
                var featureID = 0;
                int.TryParse(rcbFeatures.SelectedValue.ToString(), out featureID);
                thisContainer.myFeatureID = featureID;
            }

            thisContainer.qDateFormat = rcbDateFormat.SelectedValue?.ToString() ?? "";

            if (rcbDatabaseFileType.SelectedValue != null)
            {
                var databaseFileTypeID = 0;
                int.TryParse(rcbDatabaseFileType.SelectedValue.ToString(), out databaseFileTypeID);
                thisContainer.DatabaseFileType = databaseFileTypeID;
            }

            thisContainer.saveFileName = Path.GetFileNameWithoutExtension(txtFileName.Text);
            return true;
        }

        private void SetupNextTile()
        {
            thisContainer.SetupToMapColumns();
            thisContainer.ShowRules();

            int servfeatID = -1;

            var servfeat = thisContainer.AllFeatures.FirstOrDefault(x => x.SFName.Equals(UADFeatures.Special_Files.ToString().Replace("_", " ")));
            if (servfeat != null)
            {
                servfeatID = servfeat.ServiceFeatureID;
            }

            var databaseFileType = thisContainer.DatabaseFileTypeList.FirstOrDefault(x => x.CodeId == thisContainer.DatabaseFileType);
            var borderList = WPF.FindVisualChildren<Border>(thisContainer);
            var thisBorder = borderList.FirstOrDefault(x =>
                x.Name.Equals("StepTwoContainer", StringComparison.CurrentCultureIgnoreCase));
            if (thisContainer.myFeatureID == servfeatID ||
                thisContainer.IsSpecialFile ||
                (databaseFileType != null && 
                 databaseFileType.CodeName.Equals(FileTypes.ACS.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                 thisContainer.fw.IsZipFile(thisContainer.fileInfo)))
            {
                var fieldMappings = serviceClientSet.FieldMapping.Proxy
                    .Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.sourceFileID)
                    .Result;
                if (fieldMappings == null || fieldMappings.Count == 0)
                {
                    thisContainer.SetupToSpecialFile();
                    if (thisBorder != null)
                    {
                        thisBorder.Child = new SpecialFile(thisContainer);
                    }

                    return;
                }
            }

            thisContainer.SetupToMapColumns();
            if (thisBorder != null)
            {
                thisBorder.Child = new MapColumns(thisContainer);
            }
        }

        private void btnDuplicate_Click(object sender, RoutedEventArgs e)
        {                        
            if (thisContainer.sourceFileID > 0)
            {
                #region Display input for new file name.
                string input = "";
                input = Microsoft.VisualBasic.Interaction.InputBox("Please provide a new filename for this mapping.", "Source File Name", "");
                #endregion

                #region Name Validation
                if (string.IsNullOrEmpty(input))
                { 
                    Core_AMS.Utilities.WPF.Message("New filename cannot be blank.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Data");
                    return;
                }     
                else if (input.Length > 50)
                {
                    Core_AMS.Utilities.WPF.Message("Data invalid. New filename cannot exceed 50 characters in length. Please fix and continue.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Data");
                    return;
                }

                FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.SourceFile>> svClientSources = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.SourceFile>>();
                svClientSources = serviceClientSet.SourceFile.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.myClient.ClientID, false);
                if (svClientSources.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    if (svClientSources.Result.FirstOrDefault(x => x.FileName.Equals(input, StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        Core_AMS.Utilities.WPF.Message("New filename already exists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Data");
                        return;
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageServiceError();
                    return;
                }
                #endregion

                List<FrameworkUAS.Entity.TransformationFieldMap> allTransFieldMaps = new List<FrameworkUAS.Entity.TransformationFieldMap>();
                allTransFieldMaps = serviceClientSet.TransformationFieldMapData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result.Where(x => x.SourceFileID == thisContainer.sourceFileID).ToList();

                #region SourceFile
                FrameworkUAS.Entity.SourceFile thisSource = new FrameworkUAS.Entity.SourceFile();
                thisSource = serviceClientSet.SourceFile.Proxy.SelectForSourceFile(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.sourceFileID, false).Result;
                thisSource.SourceFileID = 0;
                thisSource.FileName = input;
                thisSource.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                thisSource.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                thisSource.DateCreated = DateTime.Now;
                thisSource.DateUpdated = DateTime.Now;

                int sfID = 0;
                sfID = serviceClientSet.SourceFile.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisSource).Result;
                #endregion

                if (sfID > 0)
                {
                    #region FieldMapping
                    List<FrameworkUAS.Entity.FieldMapping> allFieldMaps = new List<FrameworkUAS.Entity.FieldMapping>();
                    allFieldMaps = serviceClientSet.FieldMapping.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.sourceFileID).Result;
                    foreach (FrameworkUAS.Entity.FieldMapping thisFieldMap in allFieldMaps)
                    {
                        int oldFMID = thisFieldMap.FieldMappingID;
                        thisFieldMap.FieldMappingID = 0;
                        thisFieldMap.SourceFileID = sfID;                        
                        thisFieldMap.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        thisFieldMap.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        thisFieldMap.DateCreated = DateTime.Now;
                        thisFieldMap.DateUpdated = DateTime.Now;
                        
                        int fmID = 0;
                        fmID = serviceClientSet.FieldMapping.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisFieldMap).Result;

                        #region FieldMultiMap
                        if (thisFieldMap.HasMultiMapping && fmID > 0)
                        {                            
                            List<FrameworkUAS.Entity.FieldMultiMap> allFieldMultiMaps = new List<FrameworkUAS.Entity.FieldMultiMap>();
                            allFieldMultiMaps = serviceClientSet.FieldMultiMap.Proxy.SelectFieldMappingID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, oldFMID).Result;
                            foreach (FrameworkUAS.Entity.FieldMultiMap thisFieldMultiMap in allFieldMultiMaps)
                            {
                                thisFieldMultiMap.FieldMultiMapID = 0;
                                thisFieldMultiMap.FieldMappingID = fmID;                                                      
                                thisFieldMultiMap.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                                thisFieldMultiMap.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                                thisFieldMultiMap.DateCreated = DateTime.Now;
                                thisFieldMultiMap.DateUpdated = DateTime.Now;

                                serviceClientSet.FieldMultiMap.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisFieldMultiMap);
                            }                            
                        }
                        #endregion
                        #region TransformationFieldMap
                        if (allTransFieldMaps.FirstOrDefault(x => x.FieldMappingID == oldFMID) != null)
                        {
                            List<FrameworkUAS.Entity.TransformationFieldMap> fieldMappingTransFieldMaps = allTransFieldMaps.Where(x => x.FieldMappingID == oldFMID).ToList();
                            foreach (FrameworkUAS.Entity.TransformationFieldMap tfm in fieldMappingTransFieldMaps)
                            {
                                tfm.FieldMappingID = fmID;
                                tfm.SourceFileID = sfID;                        
                                tfm.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                                tfm.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                                tfm.DateCreated = DateTime.Now;
                                tfm.DateUpdated = DateTime.Now;

                                serviceClientSet.TransformationFieldMapData.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, tfm);
                            }
                        }
                        #endregion
                    }
                    Core_AMS.Utilities.WPF.MessageSaveComplete();
                    #endregion                    
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("Saving new source file failed. Please contact customer support if the problem persists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Save Failed");
                    return;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Source file to duplicate was unclear. Please contact customer support if the problem persists.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Data");
                return;
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            int clientID = 0;
            if (rcbClients.SelectedValue != null)
                int.TryParse(rcbClients.SelectedValue.ToString(), out clientID);

            if (clientID > 0)
            {
                thisContainer.myClient = thisContainer.AllClients.FirstOrDefault(x => x.ClientID == clientID);
                List<FrameworkUAS.Entity.SourceFile> files = new List<FrameworkUAS.Entity.SourceFile>();
                KMPlatform.Entity.Service circService = thisContainer.AllServices.FirstOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
                if (circService != null)
                {
                    if (thisContainer.isCirculation && circService != null)
                        files = AllSourceFiles.Where(x => x.ClientID == clientID && x.IsDeleted == false && x.ServiceID == circService.ServiceID).ToList();
                    else                    
                        files = AllSourceFiles.Where(x => x.ClientID == clientID && x.IsDeleted == false && x.ServiceID != circService.ServiceID).ToList();
                }

                rlbFiles.ItemsSource = null;
                rlbFiles.ItemsSource = files.OrderBy(x => x.FileName);
                rlbFiles.DisplayMemberPath = "FileName";
                rlbFiles.SelectedValuePath = "SourceFileID";
                Core_AMS.Utilities.WPF.Message("Refresh Complete.", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, "Action Complete");
            }
        }

        private void btnDeleteFile_Click(object sender, RoutedEventArgs e)
        {
            if (rlbFiles.SelectedItem == null)            
            {
                MessageBox.Show("No File Mapping selected, please select one", "Notification", MessageBoxButton.OK);
            }
            else
            {
                int sID = 0;
                int.TryParse(rlbFiles.SelectedValue.ToString(), out sID);
                FrameworkUAS.Entity.SourceFile t = serviceClientSet.SourceFile.Proxy.SelectForSourceFile(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, sID).Result;

                MessageBoxResult areYouSureDelete = MessageBox.Show("Are you sure you want to delete \"" + t.FileName + "\"? ", "Warning", MessageBoxButton.YesNo);

                if (areYouSureDelete == MessageBoxResult.Yes)
                {
                    serviceClientSet.SourceFile.Proxy.Delete(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, t.SourceFileID, t.ClientID);                    
                    int clientID = 0;
                    if (rcbClients.SelectedValue != null)
                        int.TryParse(rcbClients.SelectedValue.ToString(), out clientID);

                    FrameworkUAS.Entity.SourceFile sourceFile = AllSourceFiles.FirstOrDefault(x => x.SourceFileID == t.SourceFileID);
                    AllSourceFiles.Remove(sourceFile);
                    sourceFile.IsDeleted = true;
                    AllSourceFiles.Add(sourceFile);                    

                    List<FrameworkUAS.Entity.SourceFile> files = serviceClientSet.SourceFile.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, clientID, false).Result.Where(x=> x.IsDeleted == false).ToList();
                    KMPlatform.Entity.Service circService = thisContainer.AllServices.FirstOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    if (circService != null)
                    {
                        if (thisContainer.isCirculation && circService != null)
                            files = files.Where(x => x.ServiceID == circService.ServiceID).ToList();
                        else                    
                            files = files.Where(x => x.ServiceID != circService.ServiceID).ToList();
                    }
                    if (thisContainer.myUADFeature == KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                    {
                        if(dcCode == null)
                            dcCode = serviceClientSet.LookUpCode.Proxy.SelectCodeName(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Database_File, "Data Compare").Result;

                        files = files.Where(x => x.DatabaseFileTypeId == dcCode.CodeId).ToList();
                    }

                    rlbFiles.ItemsSource = files.OrderBy(x => x.FileName);
                    rlbFiles.DisplayMemberPath = "FileName";
                    rlbFiles.SelectedValuePath = "SourceFileID";

                    MessageBox.Show("File Mapping deleted", "Notification", MessageBoxButton.OK);
                }
            }
        }

        private void rcbExtension_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rcbExtension.SelectedValue != null)
            {
                string ext = rcbExtension.SelectedValue.ToString().Trim();
                if (!string.IsNullOrEmpty(ext) && (ext.ToLower() == ".csv" || ext.ToLower() == ".txt"))
                {
                    lblDelimiters.Visibility = Visibility.Visible;
                    lblQuotations.Visibility = Visibility.Visible;
                    rcbDelimiters.Visibility = Visibility.Visible;
                    rcbQuotations.Visibility = Visibility.Visible;
                }
                else
                {
                    lblDelimiters.Visibility = Visibility.Collapsed;
                    lblQuotations.Visibility = Visibility.Collapsed;
                    rcbDelimiters.Visibility = Visibility.Collapsed;
                    rcbQuotations.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
