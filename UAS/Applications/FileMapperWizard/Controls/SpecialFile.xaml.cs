using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for SpecialFile.xaml
    /// </summary>
    public partial class SpecialFile : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> blfmt = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();        
        FrameworkServices.ServiceClient<UAS_WS.Interface.ISourceFile> blsf = FrameworkServices.ServiceClient.UAS_SourceFileClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClientCustomProcedure> blccp = FrameworkServices.ServiceClient.UAS_ClientCustomProcedureClient();        
        #endregion

        #region VARIABLES
        FileMapperWizard.Modules.FMUniversal thisContainer;
        List<FrameworkUAS.Entity.ClientCustomProcedure> allProcs = new List<FrameworkUAS.Entity.ClientCustomProcedure>();
        List<FrameworkUAD_Lookup.Entity.Code> allResults = new List<FrameworkUAD_Lookup.Entity.Code>();
        List<FrameworkUAD_Lookup.Entity.Code> allEP = new List<FrameworkUAD_Lookup.Entity.Code>();
        List<FrameworkUAD_Lookup.Entity.Code> allPT = new List<FrameworkUAD_Lookup.Entity.Code>();

        FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.ClientCustomProcedure>> svProcs = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.ClientCustomProcedure>>();
        #endregion

        public SpecialFile(FileMapperWizard.Modules.FMUniversal container)
        {
            InitializeComponent();
            thisContainer = container;            
            svProcs = blccp.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.myClient.ClientID);
            if (svProcs.Result != null && svProcs.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                allProcs = svProcs.Result;
            else
                Core_AMS.Utilities.WPF.Message("An error occurred while loading client custom procedures. Please contact Customer Support.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Data");
                        

            rcbProcedure.ItemsSource = allProcs;
            rcbProcedure.DisplayMemberPath = "ProcedureName";
            rcbProcedure.SelectedValuePath = "ClientCustomProcedureID";

            allResults = blfmt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Special_File_Result).Result;
            rcbResult.ItemsSource = allResults;
            rcbResult.DisplayMemberPath = "DisplayName";
            rcbResult.SelectedValuePath = "CodeId";

            allEP = blfmt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Execution_Points).Result;
            rcbExecutionPointID.ItemsSource = allEP;
            rcbExecutionPointID.DisplayMemberPath = "DisplayName";
            rcbExecutionPointID.SelectedValuePath = "CodeId";

            allPT = blfmt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Procedure).Result;
            rcbProcedureType.ItemsSource = allPT;
            rcbProcedureType.DisplayMemberPath = "DisplayName";
            rcbProcedureType.SelectedValuePath = "CodeId";

            if (thisContainer.isEdit)
            {
                if (thisContainer.ClientCustomProcedureID != null && thisContainer.ClientCustomProcedureID > 0)
                    rcbProcedure.SelectedValue = thisContainer.ClientCustomProcedureID;

                if (thisContainer.SpecialFileResultID != null && thisContainer.SpecialFileResultID > 0)
                    rcbResult.SelectedValue = thisContainer.SpecialFileResultID;

                if (thisContainer.IsSpecialFile != null && thisContainer.IsSpecialFile)
                    cbIsSourceFileASpecialFile.IsChecked = true;
                else
                    cbIsSourceFileASpecialFile.IsChecked = false;

            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            tProcName.Visibility = System.Windows.Visibility.Visible;
            tbProcName.Visibility = System.Windows.Visibility.Visible;
            tExecutionOrder.Visibility = System.Windows.Visibility.Visible;
            rcbExecutionOrder.Visibility = System.Windows.Visibility.Visible;
            tProcedureType.Visibility = System.Windows.Visibility.Visible;
            rcbProcedureType.Visibility = System.Windows.Visibility.Visible;
            tExecutionPointID.Visibility = System.Windows.Visibility.Visible;
            rcbExecutionPointID.Visibility = System.Windows.Visibility.Visible;
            tIsForSpecialFile.Visibility = System.Windows.Visibility.Visible;
            cbIsForSpecialFile.Visibility = System.Windows.Visibility.Visible;            
            btnSave.Visibility = System.Windows.Visibility.Visible;
            btnCancel.Visibility = System.Windows.Visibility.Visible;

            btnNew.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region Proc Name
            string myProcName = "";
            if (!string.IsNullOrEmpty(tbProcName.Text.Trim()))
            {
                myProcName = tbProcName.Text.Trim();
                if (allProcs.FirstOrDefault(x => x.ProcedureName.Equals(myProcName, StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Core_AMS.Utilities.WPF.Message("Name already used. Please provide a valid name.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Error");
                    return;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Please provide a valid name.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Error");
                return;
            }
            #endregion
            int myExecOrder = 1;
            if (rcbExecutionOrder.Value != null)
                int.TryParse(rcbExecutionOrder.Value.ToString(), out myExecOrder);

            #region Execution Point
            int myExecID = 0;
            if (rcbExecutionPointID.SelectedValue != null)
                int.TryParse(rcbExecutionPointID.SelectedValue.ToString(), out myExecID);
            else
            {
                Core_AMS.Utilities.WPF.Message("Please select a Execution Point.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Error");
                return;
            }
            #endregion
            #region ProcType
            string myProcType = "";
            if (rcbProcedureType.SelectedValue != null)
                myProcType = rcbProcedureType.SelectedValue.ToString();
            else
            {
                Core_AMS.Utilities.WPF.Message("Please provide a valid Procedure Type.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Error");
                return;
            }
            #endregion

            FrameworkUAS.Entity.ClientCustomProcedure ccp = new FrameworkUAS.Entity.ClientCustomProcedure();
            ccp.ClientID = thisContainer.myClient.ClientID;
            ccp.IsActive = true;
            ccp.ProcedureName = myProcName;
            ccp.ExecutionOrder = myExecOrder;
            ccp.DateCreated = DateTime.Now;
            ccp.DateUpdated = DateTime.Now;
            ccp.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            ccp.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            ccp.ProcedureType = myProcType;
            ccp.ServiceID = thisContainer.myService.ServiceID;
            ccp.ServiceFeatureID = thisContainer.myFeatureID;
            ccp.ExecutionPointID = myExecID;
            ccp.IsForSpecialFile = true;

            //int ccpID = blccp.Proxy.SaveReturnID(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, ccp).Result;

            allProcs = blccp.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result.Where(x => x.ClientID == thisContainer.myClient.ClientID).ToList();
            rcbProcedure.ItemsSource = allProcs;
            rcbProcedure.DisplayMemberPath = "ProcedureName";
            rcbProcedure.SelectedValuePath = "ClientCustomProcedureID";

            ClearControls();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
        }

        private void ClearControls()
        {
            tbProcName.Text = "";
            rcbExecutionOrder.Value = 1;
            rcbProcedureType.SelectedIndex = -1;
            rcbExecutionPointID.SelectedIndex = -1;
            cbIsForSpecialFile.IsChecked = false;

            tProcName.Visibility = System.Windows.Visibility.Collapsed;
            tbProcName.Visibility = System.Windows.Visibility.Collapsed;
            tExecutionOrder.Visibility = System.Windows.Visibility.Collapsed;
            rcbExecutionOrder.Visibility = System.Windows.Visibility.Collapsed;
            tProcedureType.Visibility = System.Windows.Visibility.Collapsed;
            rcbProcedureType.Visibility = System.Windows.Visibility.Collapsed;
            tExecutionPointID.Visibility = System.Windows.Visibility.Collapsed;
            rcbExecutionPointID.Visibility = System.Windows.Visibility.Collapsed;
            tIsForSpecialFile.Visibility = System.Windows.Visibility.Collapsed;
            cbIsForSpecialFile.Visibility = System.Windows.Visibility.Collapsed;            
            btnSave.Visibility = System.Windows.Visibility.Collapsed;
            btnCancel.Visibility = System.Windows.Visibility.Collapsed;

            btnNew.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            #region FileSnippet
            int FileSnippetID = 0;
            FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.Code> svFileSnipCode = new FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.Code>();
            svFileSnipCode = blfmt.Proxy.SelectCodeValue(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.File_Snippet, FrameworkUAD_Lookup.Enums.FileSnippetTypes.Prefix.ToString());
            if (svFileSnipCode.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                FileSnippetID = svFileSnipCode.Result.CodeId;
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Issue retrieving data. Please try again or contact customer service if issue persists." + thisContainer.issues, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Error");
                return;
            }
            #endregion
            #region SOURCEFILE SAVE

            int myCCPID = 0;
            if (rcbProcedure.SelectedValue != null)
                int.TryParse(rcbProcedure.SelectedValue.ToString(), out myCCPID);

            int mySFRID = 0;
            if (rcbResult.SelectedValue != null)
                int.TryParse(rcbResult.SelectedValue.ToString(), out mySFRID);

            bool sourceSpecial = false;
            if (cbIsSourceFileASpecialFile.IsChecked == true)
                sourceSpecial = true;

            FrameworkUAS.Entity.SourceFile sourceFile = new FrameworkUAS.Entity.SourceFile();
            sourceFile.SourceFileID = thisContainer.sourceFileID;
            sourceFile.FileRecurrenceTypeId = thisContainer.srcFileTypeID;                    
            sourceFile.DatabaseFileTypeId = thisContainer.DatabaseFileType;                    
            sourceFile.FileName = System.IO.Path.GetFileNameWithoutExtension(thisContainer.saveFileName);
            sourceFile.ClientID = thisContainer.myClient.ClientID;
            if (thisContainer.isCirculation)
            {                        
                sourceFile.PublicationID = thisContainer.currentPublication.PubID;
            }
            else
            {                        
                sourceFile.PublicationID = 0;
            }
            sourceFile.IsDeleted = thisContainer.IsDeleted;
            sourceFile.IsIgnored = thisContainer.IsIgnored;
            sourceFile.FileSnippetID = FileSnippetID;                    
            sourceFile.Extension = thisContainer.extension;
            sourceFile.IsDQMReady = thisContainer.IsDQMReady;
            sourceFile.Delimiter = thisContainer.myDelimiter;
            sourceFile.IsTextQualifier = thisContainer.myTextQualifier;
            sourceFile.ServiceID = thisContainer.myService.ServiceID;
            sourceFile.ServiceFeatureID = thisContainer.myFeatureID;
            //IF UAD File and Db File Type = 0 try and set to CodeId for Audience Data
            if (thisContainer.DatabaseFileType == 0)
            {
                if (thisContainer.isCirculation == false)
                {
                    var dbTypeCodes = blfmt.Proxy.SelectCodeName(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Database_File, FrameworkUAD_Lookup.Enums.FileTypes.Audience_Data.ToString());
                    if (dbTypeCodes.Result != null)
                        thisContainer.DatabaseFileType = dbTypeCodes.Result.CodeId;
                    else
                        thisContainer.DatabaseFileType = 0;

                    sourceFile.DatabaseFileTypeId = thisContainer.DatabaseFileType;
                }                        
            }

            sourceFile.MasterGroupID = thisContainer.MasterGroupID;
            sourceFile.UseRealTimeGeocoding = thisContainer.UseRealTimeGeocoding;//enableGeoCoding;
            sourceFile.IsSpecialFile = sourceSpecial;
            sourceFile.ClientCustomProcedureID = myCCPID;
            sourceFile.SpecialFileResultID = mySFRID;
            sourceFile.DateCreated = DateTime.Now;
            sourceFile.DateUpdated = DateTime.Now;
            sourceFile.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;                    
            sourceFile.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;                    
            sourceFile.QDateFormat = thisContainer.qDateFormat;
            sourceFile.BatchSize = thisContainer.batchSize;

            thisContainer.sourceFileID = blsf.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, sourceFile).Result;
            #endregion

            if (thisContainer.sourceFileID > 0)
            {
                Core_AMS.Utilities.WPF.MessageSaveComplete();
                CloseWindow();
            }
        }

        private void CloseWindow()
        {
            Windows.FMWindow win = this.ParentOfType<Windows.FMWindow>();
            if (win != null)
                win.Close();
            else
            {
                object pwin = Core_AMS.Utilities.WPF.GetWindow(this);
                if (pwin != null)
                {
                    if (pwin.GetType() == typeof(WpfControls.WindowsAndDialogs.PopOut))
                    {
                        WpfControls.WindowsAndDialogs.PopOut p = (WpfControls.WindowsAndDialogs.PopOut)pwin;
                        p.Close();
                    }                    
                }
            }
        } 
    }
}
