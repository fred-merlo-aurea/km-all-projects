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
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class UADSetup : UserControl
    {
        private readonly ServiceClientSet serviceClientSet = new ServiceClientSet();

        #region VARIABLES
        FileMapperWizard.Modules.FMUniversal thisContainer;
        FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> svCodes = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        bool IsDataCompare { get; set; } 
        #endregion

        public UADSetup(FileMapperWizard.Modules.FMUniversal container, bool isDataCompare = false)
        {
            InitializeComponent();
            thisContainer = container;
            IsDataCompare = isDataCompare;

            List<FrameworkUAD_Lookup.Entity.Code> fileTypes = serviceClientSet.LookUpCode.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Database_File).Result.Where(x => x.IsActive == true).ToList();
            thisContainer.DatabaseFileTypeList = fileTypes;

            if (IsDataCompare == true)
            {
                thisContainer.myUADFeature = KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare;
                FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> blfmt = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
                if (thisContainer.DatabaseFileTypeList != null)
                    thisContainer.DatabaseFileType = thisContainer.DatabaseFileTypeList.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FileTypes.Data_Compare.ToString().Replace("_", " "))).CodeId;//dbTypeCodes.Result.CodeId;
            }

            LoadData(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient);
        }

        public void LoadData(KMPlatform.Entity.Client SelectedClient)
        {
            //thisContainer.DataCompareCheck();

            #region Load Clients
            if (SelectedClient.ClientID == 1)
            {
                //var result = blc.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, false);
                thisContainer.AllClients = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.OrderBy(x => x.DisplayName).ToList(); 
                //thisContainer.AllServices = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup.Services; 
                thisContainer.AllServices = serviceClientSet.Services.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, true).Result;
                rcbClients.ItemsSource = thisContainer.AllClients.Where(x => x.IsActive);
                rcbClients.DisplayMemberPath = "DisplayName";
                rcbClients.SelectedValuePath = "ClientID";
                KMPlatform.Entity.Client sc = thisContainer.AllClients.SingleOrDefault(x => x.ClientID == SelectedClient.ClientID);
                if (sc != null)
                    rcbClients.SelectedItem = sc;

            }
            else
            {
                thisContainer.AllClients = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.OrderBy(x => x.DisplayName).ToList(); //blc.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, false).Result.OrderBy(x => x.ClientName).ToList();
                //thisContainer.AllServices = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup.Services; //bls.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                thisContainer.AllServices = serviceClientSet.Services.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, true).Result;
                rcbClients.ItemsSource = thisContainer.AllClients.Where(x => x.IsActive && x.ClientID == SelectedClient.ClientID);
                rcbClients.DisplayMemberPath = "DisplayName";
                rcbClients.SelectedValuePath = "ClientID";
                KMPlatform.Entity.Client sc = thisContainer.AllClients.SingleOrDefault(x => x.ClientID == SelectedClient.ClientID);
                if (sc != null)
                    rcbClients.SelectedItem = sc;

            }
            #endregion

            #region POPULATE File Recurrence
            rcbFrequency.ItemsSource = serviceClientSet.LookUpCode.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.File_Recurrence).Result;
            rcbFrequency.DisplayMemberPath = "DisplayName";
            rcbFrequency.SelectedValuePath = "CodeId";
            #endregion

            #region CODE
            svCodes = serviceClientSet.LookUpCode.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping);
            List<FrameworkUAD_Lookup.Entity.Code> codes = svCodes.Result;
            thisContainer.standardTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString())).CodeId;
            thisContainer.demoTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString())).CodeId;
            thisContainer.ignoreTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString())).CodeId;
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
        }

        //REFACTOR --OUTSIDE USER VS KM EMPLOYEE SECTION
        private void rcbClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            #region Client Selection
            int clientID = -1;
            if (rcbClients.SelectedValue != null)
                int.TryParse(rcbClients.SelectedValue.ToString(), out clientID);

            if (clientID > -1)
            {
                thisContainer.myClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.FirstOrDefault(x => x.ClientID == clientID);
                if (thisContainer.myClient == null)
                {
                    Core_AMS.Utilities.WPF.Message("Error during client selection. If problem persists, please contact your account representative.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                    return;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            #endregion
            #region Service Selection
            //LOAD FEATURES THAT ARE ENABLED FOR CLIENT
            int serviceID = 0;
            thisContainer.myService = thisContainer.AllServices.SingleOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.UADFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
            if (thisContainer.myService.ServiceID != null)
            {
                serviceID = thisContainer.myService.ServiceID;
                KMPlatform.Entity.Service selectedService = thisContainer.AllServices.Single(x => x.ServiceID == serviceID);// bls.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, serviceID).Result;
                thisContainer.AllFeatures = serviceClientSet.ServiceFeature.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                List<KMPlatform.Entity.ServiceFeature> currentClientFeatures = new List<KMPlatform.Entity.ServiceFeature>();
                foreach (KMPlatform.Entity.ServiceFeature ss in selectedService.ServiceFeatures)
                {
                    if (selectedService.ServiceFeatures.Count > 0 && ss.IsEnabled)
                    {
                        //Outside User
                        if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 1 && ss.KMAdminOnly == false)
                            currentClientFeatures.Add(thisContainer.AllFeatures.SingleOrDefault(x => x.ServiceFeatureID == ss.ServiceFeatureID));

                        //KM Employee
                        if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID == 1)
                            currentClientFeatures.Add(thisContainer.AllFeatures.SingleOrDefault(x => x.ServiceFeatureID == ss.ServiceFeatureID));
                    }
                }
                rcbFeatures.ItemsSource = currentClientFeatures.Where(x => x.IsEnabled).OrderBy(x => x.SFName).ToList();
                rcbFeatures.DisplayMemberPath = "SFName";
                rcbFeatures.SelectedValuePath = "ServiceFeatureID";

                if(IsDataCompare == true)
                {
                    if(currentClientFeatures.Exists(x => x.SFName.Equals("Data Compare")))
                    {
                        KMPlatform.Entity.ServiceFeature mySF = currentClientFeatures.Single(x => x.SFName.Equals("Data Compare"));
                        rcbFeatures.SelectedItem = mySF;
                    }
                }
            }
            else
                Core_AMS.Utilities.WPF.Message("No service was selected. Please report to appropriate personnel", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Service");
            #endregion
        }
        
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
                        Core_AMS.Utilities.WPF.Message("File Delimiter and/or File contains double quotation marks.  Please update Quotations selection before advancing.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing File Info");
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
            
            #region CHECK PROCESS AND FILE FREQUENCY SELECTED
            if (rcbFeatures.SelectedValue == null || rcbFrequency.SelectedValue == null)
            {
                Core_AMS.Utilities.WPF.Message("Data is missing. Please make sure process was selected and/or file frequency was selected.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
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

            List<FrameworkUAS.Entity.SourceFile> clientSources = serviceClientSet.SourceFile.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, false).Result.Where(x => x.ClientID == thisContainer.myClient.ClientID && x.IsDeleted == false).ToList();
            if (clientSources.FirstOrDefault(x => x.FileName.Equals(System.IO.Path.GetFileNameWithoutExtension(txtSaveName.Text), StringComparison.CurrentCultureIgnoreCase)) != null)
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

            thisContainer.batchSize = rnudBatchSize.Value.HasValue  == true ? Convert.ToInt32(rnudBatchSize.Value.Value) : 2500;

            thisContainer.myTextQualifier = false;
            if (rcbQuotations.SelectedValue != null)
            {
                if (rcbQuotations.SelectedValue.ToString() == "Yes")
                    thisContainer.myTextQualifier = true;
                else if (rcbQuotations.SelectedValue.ToString() == "No")
                    thisContainer.myTextQualifier = false;

            }

            int sFileTypeID = 0;
            if (rcbFrequency.SelectedValue != null)
                int.TryParse(rcbFrequency.SelectedValue.ToString(), out sFileTypeID);

            int featureID = 0;
            if (rcbFeatures.SelectedValue != null)
                int.TryParse(rcbFeatures.SelectedValue.ToString(), out featureID);

            thisContainer.extension = System.IO.Path.GetExtension(thisContainer.FileName);

            thisContainer.srcFileTypeID = sFileTypeID;
            thisContainer.myFeatureID = featureID;
            if (rcbFeatures.SelectedItem != null && thisContainer.isCirculation == false)
            {
                KMPlatform.Entity.ServiceFeature sf = (KMPlatform.Entity.ServiceFeature)rcbFeatures.SelectedItem;
                thisContainer.myUADFeature = KMPlatform.BusinessLogic.Enums.GetUADFeature(sf.SFName);
                if (sf.SFName.Equals(FrameworkUAD_Lookup.Enums.FileTypes.Data_Compare.ToString().Replace("_", " ")))
                {
                    FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> blfmt = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
                    var dbTypeCodes = blfmt.Proxy.SelectCodeName(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Database_File, FrameworkUAD_Lookup.Enums.FileTypes.Data_Compare.ToString());
                    if (dbTypeCodes.Result != null)
                        thisContainer.DatabaseFileType = dbTypeCodes.Result.CodeId;
                }
            }

            thisContainer.saveFileName = txtSaveName.Text;
            #endregion
            
            #region Setup Next Tile
            //thisContainer.DataCompareCheck();

            int servfeatID = -1;
            var servfeat = thisContainer.AllFeatures.FirstOrDefault(x => x.SFName.Equals(KMPlatform.BusinessLogic.Enums.UADFeatures.Special_Files.ToString().Replace("_", " ")));
            if (servfeat != null)
                servfeatID = servfeat.ServiceFeatureID;

            if (thisContainer.extension.Equals(".zip", StringComparison.CurrentCultureIgnoreCase))
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
            else if (featureID == servfeatID || thisContainer.IsSpecialFile == true)
            {
                MessageBoxResult additionalMapping = MessageBox.Show("Will this file's fields need to be mapped as well?", "Information", MessageBoxButton.YesNo);

                if (additionalMapping == MessageBoxResult.Yes)
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
                else
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
    }
}