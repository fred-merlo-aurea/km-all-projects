using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FileMapperWizard.Helpers;
using KM.Common;
using KM.Common.Functions;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for CircSetup.xaml
    /// </summary>
    public partial class CircSetup : UserControl
    {
        private readonly ServiceClientSet serviceClientSet = new ServiceClientSet();

        #region VARIABLES
        FileMapperWizard.Modules.FMUniversal thisContainer;
        FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> svPublishers = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svPublications = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> svCodes = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        KMPlatform.Entity.Client clientList = new KMPlatform.Entity.Client();

        List<KMPlatform.Object.Product> productList = new List<KMPlatform.Object.Product>();
        #endregion

        public CircSetup(FileMapperWizard.Modules.FMUniversal container)
        {
            thisContainer = container;
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            #region Load Client
            thisContainer.AllClients = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients;            
            #endregion

            #region Hide Rules
            //FrameworkServices.ServiceClient<UAS_WS.Interface.IFileRule> rWorker = FrameworkServices.ServiceClient.UAS_FileRuleClient();
            //FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileRule>> svRules = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileRule>>();
            //svRules = rWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            //if (!(svRules.Result.Count > 0))
            //    thisContainer.HideRules();
            #endregion

            #region Services
            //thisContainer.AllServices = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup.Services;
            thisContainer.AllServices = serviceClientSet.Services.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, true).Result;
            //thisContainer.myService = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup.Services.FirstOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
            thisContainer.myService = thisContainer.AllServices.FirstOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
            #endregion

            #region Service Features
            thisContainer.AllFeatures = thisContainer.myService.ServiceFeatures;
            rcbServiceFeature.ItemsSource = thisContainer.AllFeatures.Where(x => x.IsEnabled == true);
            rcbServiceFeature.DisplayMemberPath = "SFName";
            rcbServiceFeature.SelectedValuePath = "ServiceFeatureID";
            #endregion

            #region Load Publisher and Publication
            foreach (var x in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients)
            {
                productList.AddRange(x.Products.Where(y=> y.IsCirc == true).ToList());
            }

            thisContainer.AllPublishers = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients;//svPublishers.Result;
            rcbPublisher.ItemsSource = thisContainer.AllPublishers.OrderBy(x => x.DisplayName);
            rcbPublisher.SelectedValuePath = "ClientID";
            rcbPublisher.DisplayMemberPath = "DisplayName";

            //thisContainer.AllPublications = productList.Where(x => x.IsCirc == true).ToList();
            FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> pubResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubData = FrameworkServices.ServiceClient.UAD_ProductClient();
            pubResponse = pubData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            thisContainer.AllPublications = pubResponse.Result;
            
            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient != null && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientCode != "KM")
            {
                KMPlatform.Entity.Client c = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
                rcbPublisher.ItemsSource = thisContainer.AllPublishers.Where(x => x.ClientID == c.ClientID).ToList();
                rcbPublisher.SelectedValuePath = "ClientID";
                rcbPublisher.DisplayMemberPath = "DisplayName";
                rcbPublisher.SelectedValue = thisContainer.myClient.ClientID;
                thisContainer.currentPublisher = thisContainer.AllPublishers.FirstOrDefault(x => x.ClientID == thisContainer.myClient.ClientID);
            }
            else
            {
                rcbPublisher.SelectedValue = thisContainer.myClient.ClientID;
                thisContainer.currentPublisher = thisContainer.AllPublishers.FirstOrDefault(x => x.ClientID == thisContainer.myClient.ClientID);
            }            

            if (thisContainer.currentPublisher != null)
            {                
                rcbPublication.ItemsSource = thisContainer.AllPublications.Where(x => x.IsCirc == true).OrderBy(y => y.PubCode);
                rcbPublication.SelectedValuePath = "PubID";
                rcbPublication.DisplayMemberPath = "PubCode";
            }

            if (thisContainer.selectedProduct != null)            
                rcbPublication.SelectedValue = thisContainer.selectedProduct.PubID;   
         
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
            string ad = FrameworkUAD_Lookup.Enums.FileTypes.Audience_Data.ToString().Replace("_"," ");
            var fileTypes = serviceClientSet.LookUpCode.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Database_File).Result;
            thisContainer.DatabaseFileTypeList = fileTypes;

            rcbDatabaseFileType.ItemsSource = fileTypes.Where(x => x.IsActive == true);
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
            rcbQuotations.Items.Add("Yes");
            rcbQuotations.Items.Add("No");
            #endregion

            #region File Pre-populated
            if (!string.IsNullOrEmpty(thisContainer.filePath))
            {
                string fileName = thisContainer.filePath;
                txtFileName.Text = System.IO.Path.GetFileName(fileName);
                txtFilePath_FileName.Text = fileName.ToString();

                txtSaveName.Text = System.IO.Path.GetFileName(fileName);

                thisContainer.FileName = fileName;
                OpenFileConfiguration(fileName);
            }
            #endregion
        }

        #region File
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name            
            dlg.Filter = "Recognized Files(*.txt;*.csv;*.xls;*.xlsx;*.dbf;*.zip;*.xml;*.json)|*.txt;*.csv;*.xls;*.xlsx;*.dbf;*.zip;*.xml;*.json"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                string fileName = dlg.FileName;
                txtFileName.Text = System.IO.Path.GetFileName(fileName);
                txtFilePath_FileName.Text = fileName.ToString();

                txtSaveName.Text = System.IO.Path.GetFileNameWithoutExtension(fileName);

                thisContainer.FileName = fileName;
                OpenFileConfiguration(fileName);
            }
            else
                return;
        }

        private void OpenFileConfiguration(string fileName)
        {
            //SHOW | HIDE THE DELIMITER AND QUOTE ENCAPSULATION IF NOT CSV OR TXT
            if (System.IO.Path.GetExtension(fileName).ToLower() == ".csv" || System.IO.Path.GetExtension(fileName).ToLower() == ".txt")
            {
                lblDelimiters.Visibility = Visibility.Visible;
                lblQuotations.Visibility = Visibility.Visible;
                rcbDelimiters.Visibility = Visibility.Visible;
                rcbQuotations.Visibility = Visibility.Visible;
            }
            else
            {
                lblDelimiters.Visibility = Visibility.Hidden;
                lblQuotations.Visibility = Visibility.Hidden;
                rcbDelimiters.Visibility = Visibility.Hidden;
                rcbQuotations.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        private void btnStep1Next_Click(object sender, RoutedEventArgs e)
        {
            #region CHECK CLIENT AND FILE FILLED OUT
            if (thisContainer.myClient != null && !string.IsNullOrEmpty(thisContainer.FileName))
            {
                thisContainer.fileInfo = new FileInfo(thisContainer.FileName);
                if (thisContainer.fw.IsExcelFile(thisContainer.fileInfo) || thisContainer.fw.IsDbfFile(thisContainer.fileInfo) || thisContainer.fw.IsZipFile(thisContainer.fileInfo) || thisContainer.fw.IsJsonFile(thisContainer.fileInfo) || thisContainer.fw.IsXmlFile(thisContainer.fileInfo))
                {
                    thisContainer.myFileConfig = null;
                }
                else
                {
                    if (rcbDelimiters.SelectedValue == null || rcbQuotations.SelectedValue == null || string.IsNullOrEmpty(rcbDelimiters.SelectedValue.ToString()) || string.IsNullOrEmpty(rcbQuotations.SelectedValue.ToString()))
                    {
                        Core_AMS.Utilities.WPF.Message("File Delimiter and/or File contains double quotation marks needs to be selected before advancing.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing File Info");
                        return;
                    }
                    else
                    {
                        bool isQuoted = false;
                        if (rcbQuotations.SelectedValue.ToString().Trim().Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                            isQuoted = true;

                        thisContainer.myFileConfig.FileColumnDelimiter = rcbDelimiters.SelectedValue.ToString().Trim();
                        thisContainer.myFileConfig.IsQuoteEncapsulated = isQuoted;
                    }
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Data is missing. Please make sure client was selected and/or file was selected.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
            #endregion

            #region PUBLISHER AND PUBLICATION SELECTED
            #region PUBLISHER
            if (rcbPublisher.SelectedValue != null)
            {
                int PID = 0;
                int.TryParse(rcbPublisher.SelectedValue.ToString(), out PID);
                thisContainer.currentPublisher = thisContainer.AllPublishers.FirstOrDefault(x => x.ClientID == PID);
                if (thisContainer.currentPublisher == null)
                {
                    Core_AMS.Utilities.WPF.Message("Issue with selection. Publisher not selected.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                    return;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Publisher not selected.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
            #endregion
            #region PUBLICATION
            if (rcbPublication.SelectedValue != null)
            {
                int PID = 0;
                int.TryParse(rcbPublication.SelectedValue.ToString(), out PID);
                thisContainer.currentPublication = thisContainer.AllPublications.FirstOrDefault(x => x.PubID == PID);
                if (thisContainer.currentPublication == null)
                {
                    Core_AMS.Utilities.WPF.Message("Issue with selection. Publication not selected.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                    return;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Publication not selected.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
            #endregion
            #endregion

            #region CHECK DATABASE FILE TYPE & SERVICE FEATURE SELECTED
            if (rcbDatabaseFileType.SelectedValue == null)
            {
                Core_AMS.Utilities.WPF.Message("Data is missing. Please make sure database file type was selected.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
            if (rcbServiceFeature.SelectedValue == null)
            {
                Core_AMS.Utilities.WPF.Message("Data is missing. Please make sure service feature was selected.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
            #endregion

            #region CHECK FILE FREQUENCY SELECTED
            if (rcbFrequency.SelectedValue == null)
            {
                Core_AMS.Utilities.WPF.Message("Data is missing. Please make sure file frequency was selected.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
            #endregion

            #region SAVE FILE AS CHECKS
            if (String.IsNullOrEmpty(txtSaveName.Text))
            {
                Core_AMS.Utilities.WPF.Message("Data is missing. Please provide a 'Save Filename As'.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                return;
            }
            else if (txtSaveName.Text.Length > 50)
            {
                Core_AMS.Utilities.WPF.Message("Data invalid. 'Save Filename As' cannot exceed 50 characters in length. Please fix and continue.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Data");
                return;
            }
            else if (CheckFileNameIsUnique() == false)
            {
                Core_AMS.Utilities.WPF.Message("Filename already exists, please enter a unique file name or delete the existing one first.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Data");
                return;
            }
            List<FrameworkUAS.Entity.SourceFile> clientSources = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[thisContainer.myClient.ClientID].SourceFilesList.Where(x => x.IsDeleted == false).ToList();//blsf.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, false).Result.Where(x => x.ClientID == thisContainer.myClient.ClientID && x.IsDeleted == false).ToList();
            if (clientSources.FirstOrDefault(x => x.FileName.Equals(System.IO.Path.GetFileNameWithoutExtension(thisContainer.saveFileName), StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                Core_AMS.Utilities.WPF.Message("Records show the current file has been previously mapped. Please locate the file in Edit Mapping to make changes to it.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "File Previously Mapped");
                return;
            }
            #endregion

            #region Save Data For Next Tile
            thisContainer.myDelimiter = "";
            if (rcbDelimiters.SelectedValue != null)
                thisContainer.myDelimiter = rcbDelimiters.SelectedValue.ToString();

            thisContainer.qDateFormat = "";
            if (rcbDateFormat.SelectedValue != null)
                thisContainer.qDateFormat = rcbDateFormat.SelectedValue.ToString();

            thisContainer.batchSize = rnudBatchSize.Value.HasValue == true ? Convert.ToInt32(rnudBatchSize.Value.Value) : 2500;

            thisContainer.myTextQualifier = false;
            if (rcbQuotations.SelectedValue != null)
            {
                if (rcbQuotations.SelectedValue.ToString() == "Yes")
                    thisContainer.myTextQualifier = true;
                else if (rcbQuotations.SelectedValue.ToString() == "No")
                    thisContainer.myTextQualifier = false;

            }

            thisContainer.extension = System.IO.Path.GetExtension(thisContainer.FileName);

            int sFileRecurrenceId = 0;
            if (rcbFrequency.SelectedValue != null)
                int.TryParse(rcbFrequency.SelectedValue.ToString(), out sFileRecurrenceId);
            
            thisContainer.fileRecurrenceTypeId = sFileRecurrenceId;

            int databaseFileTypeID = 0;
            if (rcbDatabaseFileType.SelectedValue != null)
                int.TryParse(rcbDatabaseFileType.SelectedValue.ToString(), out databaseFileTypeID);

            thisContainer.DatabaseFileType = databaseFileTypeID;

            int myFeatureID = 0;
            if (rcbServiceFeature.SelectedValue != null)
                int.TryParse(rcbServiceFeature.SelectedValue.ToString(), out myFeatureID);

            thisContainer.myFeatureID = myFeatureID;

            //thisContainer.srcFileTypeID = sFileTypeID;
            //thisContainer.myFeatureID = featureID;

            thisContainer.saveFileName = txtSaveName.Text;

            int sFileTypeID = 0;
            if (rcbFrequency.SelectedValue != null)
                int.TryParse(rcbFrequency.SelectedValue.ToString(), out sFileTypeID);

            thisContainer.srcFileTypeID = sFileTypeID;
            #endregion

            #region Setup Next Tile
            //FrameworkServices.ServiceClient<UAS_WS.Interface.IFileRule> rWorker = FrameworkServices.ServiceClient.UAS_FileRuleClient();
            //FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileRule>> svRules = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FileRule>>();
            //svRules = rWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
            //if (!(svRules.Result.Count > 0))
            //    thisContainer.HideRules();

            int dftID = -1;
            var dft = thisContainer.DatabaseFileTypeList.FirstOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.AddressUpdateSourceTypes.ACS.ToString().Replace("_", " ")));
            if (dft != null)
                dftID = dft.CodeId;

            var dbft = thisContainer.DatabaseFileTypeList.FirstOrDefault(x => x.CodeId == thisContainer.DatabaseFileType);
            
            //if (dftID == thisContainer.DatabaseFileType && thisContainer.fw.IsZipFile(thisContainer.fileInfo))
            if (thisContainer.fw.IsZipFile(thisContainer.fileInfo))
            {
                thisContainer.SetupToSpecialFile();
                //Find bordertwocontainer set the child.
                var borderList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.Border>(thisContainer);
                if (borderList.FirstOrDefault(x => x.Name.Equals("StepTwoContainer", StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Border thisBorder = borderList.FirstOrDefault(x => x.Name.Equals("StepTwoContainer", StringComparison.CurrentCultureIgnoreCase));
                    thisBorder.Child = new FileMapperWizard.Controls.SpecialFile(thisContainer);
                }
            }
            else if (dbft != null && dbft.CodeName.Equals(FrameworkUAD_Lookup.Enums.FileTypes.ACS.ToString(), StringComparison.CurrentCultureIgnoreCase) && thisContainer.fw.IsZipFile(thisContainer.fileInfo))
            {
                thisContainer.SetupToSpecialFile();
                //Find bordertwocontainer set the child.
                var borderList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.Border>(thisContainer);
                if (borderList.FirstOrDefault(x => x.Name.Equals("StepTwoContainer", StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Border thisBorder = borderList.FirstOrDefault(x => x.Name.Equals("StepTwoContainer", StringComparison.CurrentCultureIgnoreCase));
                    thisBorder.Child = new FileMapperWizard.Controls.SpecialFile(thisContainer);
                }
            }
            else
            {
                thisContainer.SetupToMapColumns();
                //Find bordertwocontainer set the child.
                var borderList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.Border>(thisContainer);
                if (borderList.FirstOrDefault(x => x.Name.Equals("StepTwoContainer", StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Border thisBorder = borderList.FirstOrDefault(x => x.Name.Equals("StepTwoContainer", StringComparison.CurrentCultureIgnoreCase));
                    thisBorder.Child = new FileMapperWizard.Controls.MapColumns(thisContainer);
                }
            }
            #endregion
        }
        private bool CheckFileNameIsUnique()
        {
            bool isUnique = true;
            var resp = serviceClientSet.SourceFile.Proxy.IsFileNameUnique(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.myClient.ClientID, txtSaveName.Text);
            if (resp != null)
                isUnique = resp.Result;
            return isUnique;
        }
        private void rcbPublisher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rcbPublisher.SelectedValue != null)
            {
                int PID = 0;
                int.TryParse(rcbPublisher.SelectedValue.ToString(), out PID);
                thisContainer.currentPublisher = thisContainer.AllPublishers.FirstOrDefault(x => x.ClientID == PID);
                thisContainer.myClient = thisContainer.AllClients.FirstOrDefault(x => x.ClientID == thisContainer.currentPublisher.ClientID);
                if (thisContainer.currentPublisher != null)
                {
                    FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> pubResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
                    FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubData = FrameworkServices.ServiceClient.UAD_ProductClient();
                    pubResponse = pubData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.currentPublisher.ClientConnections);
                    thisContainer.AllPublications = pubResponse.Result;

                    rcbPublication.ItemsSource = thisContainer.AllPublications.Where(x => x.IsCirc == true).OrderBy(y => y.PubCode);
                    rcbPublication.SelectedValuePath = "PubID";
                    rcbPublication.DisplayMemberPath = "PubCode";
                }
            }
        }

        private void rcbPublication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rcbPublication.SelectedValue != null)
            {
                int PID = 0;
                int.TryParse(rcbPublication.SelectedValue.ToString(), out PID);
                thisContainer.currentPublication = thisContainer.AllPublications.FirstOrDefault(x => x.PubID == PID);
            }
        }
    }
}
