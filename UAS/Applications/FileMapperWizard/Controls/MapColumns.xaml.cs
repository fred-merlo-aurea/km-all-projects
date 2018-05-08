using Core_AMS.Utilities;
using FileMapperWizard.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using FileMapperWizard.Helpers;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using FrameworkUAS.Object;
using KM.Common.Import;
using Telerik.Windows.Controls;
using Enums = FrameworkUAD_Lookup.Enums;
using CommonEnums = KM.Common.Enums;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for MapColumns.xaml
    /// </summary>
    public partial class MapColumns : UserControl
    {
        private const string MissingDataCaption = "Missing Data";
        private const string IncorrectContentMessage = "Data is missing. No columns were found. Please check the file contents/file information provided was correct.";
        private const string NoColumnsMessage = "Issue gathering file columns. Please check file is not open or file has no duplicate columns. Returning to the first step.";
        private const string MissingDataMessage = "Data is missing. No columns were found. Please check the file contents/file information provided was correct.";
        private const string NormalType = "Normal";
        private const string EditUserType = "EditUser";
        private const string PreviewErrorText = "Preview error";
        private const string BitDataTypeName = "bit";
        private const string SpaceEscaped = "_";
        private const string Whitespace = " ";
        private readonly ServiceClientSet serviceClientSet = new ServiceClientSet();
        
        #region VARIABLES
        FileMapperWizard.Modules.FMUniversal thisContainer;
        List<FrameworkUAD_Lookup.Entity.Code> fieldMappingTypes;
        public List<FrameworkUAD_Lookup.Entity.Code> DemoUpdateOptions = new List<FrameworkUAD_Lookup.Entity.Code>();
        public List<string> adhocBitFields = new List<string>();
        public List<string> isRequiredFields = new List<string>();

        string myRescanFile = "";
        string rescanDelimiter = "";
        string rescanQualifier = "";
        FileConfiguration rescanFileConfig = new FileConfiguration();
        #endregion

        public MapColumns(FileMapperWizard.Modules.FMUniversal container)
        {
            thisContainer = container;
            thisContainer.Expanded = true;
            InitializeComponent();
            LoadData();
            if (!thisContainer.isEdit)
                ButtonRescan.Visibility = Visibility.Collapsed;

            LabelDelimiter.Visibility = Visibility.Collapsed;
            LabelTextQualifier.Visibility = Visibility.Collapsed;
            ComboBoxDelimiter.Visibility = Visibility.Collapsed;
            ComboBoxQualifier.Visibility = Visibility.Collapsed;
            ButtonStart.Visibility = Visibility.Collapsed;

            if (thisContainer.isCirculation)
                txbMessage.Text = "Link file columns to this Product's Subscriber File Columns.";
        }

        private bool PassFieldMappingSaving()
        {
            bool pass = true;
            thisContainer.FieldMappingErrorMessage = "";
            List<string> mapToList = new List<string>();
            List<string> mappedToColumns = new List<string>();

            foreach (ColumnMapper c in flowLayout.Children)
            {
                string mapTo = c.ComboBoxMappingText;
                mappedToColumns.Add(mapTo.ToLower());

                if ((mapTo.ToLower() == "ignore") || (mapTo.ToLower() == "kmtransform"))
                {
                    continue;
                }
                else if ((mapTo.ToLower() == "demo"))
                {
                    thisContainer.FieldMappingErrorMessage = "\nField can no longer be mapped to Demo. Please create the correct column or create a new column and re-map file.";
                    pass = false;
                    break;
                }
                else
                {
                    if (mapToList.Contains(c.ComboBoxMappingText) && c.TypeOfRow != ColumnMapperRowType.Remove.ToString())
                    {
                        thisContainer.FieldMappingErrorMessage = "\nDuplicate Mapping To: " + c.ComboBoxMappingText + " exists. Fix then try again.";
                        pass = false;
                        break;
                    }
                    else
                        mapToList.Add(c.ComboBoxMappingText);

                }
            }

            return pass;
        }                

        private void btnStep2Next_Click(object sender, RoutedEventArgs e)
        {
            #region FileSnippet
            int FileSnippetID = 0;
            FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.Code> svFileSnipCode = new FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.Code>();
            svFileSnipCode = serviceClientSet.LookUpCode.Proxy.SelectCodeValue(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.File_Snippet, FrameworkUAD_Lookup.Enums.FileSnippetTypes.Prefix.ToString());
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

            #region Initial Save of File
            if (PassFieldMappingSaving())
            {
                #region SOURCEFILE SAVE
                #region OLD
                //FrameworkUAS.Entity.SourceFile sourceFile = new FrameworkUAS.Entity.SourceFile()
                //{
                //    SourceFileID = thisContainer.sourceFileID,
                //    FileRecurrenceTypeId = thisContainer.srcFileTypeID,
                //    FileName = System.IO.Path.GetFileNameWithoutExtension(thisContainer.saveFileName),
                //    ClientID = thisContainer.myClient.ClientID,
                //    IsDeleted = false,
                //    CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                //    DateCreated = DateTime.Now,
                //    UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                //    DateUpdated = DateTime.Now,
                //    IsIgnored = false,
                //    FileSnippetID = 1,
                //    Extension = System.IO.Path.GetExtension(thisContainer.FileName),
                //    Delimiter = thisContainer.myDelimiter,
                //    IsTextQualifier = thisContainer.myTextQualifier,
                //    IsSpecialFile = false,
                //    IsDQMReady = true,
                //    ServiceID = thisContainer.myService.ServiceID,
                //    ServiceFeatureID = thisContainer.myFeatureID,
                //    MasterGroupID = 0,
                //    UseRealTimeGeocoding = false,
                //    ClientCustomProcedureID = 0,
                //    SpecialFileResultID = 0
                //};
                #endregion
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
                        if (thisContainer.DatabaseFileTypeList == null || thisContainer.DatabaseFileTypeList.Count == 0)
                        {
                            var dbTypeCodes = serviceClientSet.LookUpCode.Proxy.SelectCodeName(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Database_File, FrameworkUAD_Lookup.Enums.FileTypes.Audience_Data.ToString());
                            if (dbTypeCodes.Result != null)
                                thisContainer.DatabaseFileType = dbTypeCodes.Result.CodeId;
                        }
                        else
                            thisContainer.DatabaseFileType = thisContainer.DatabaseFileTypeList.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FileTypes.Audience_Data.ToString().Replace("_"," "))).CodeId;

                        sourceFile.DatabaseFileTypeId = thisContainer.DatabaseFileType;
                    }                        
                }

                sourceFile.MasterGroupID = thisContainer.MasterGroupID;
                sourceFile.UseRealTimeGeocoding = thisContainer.UseRealTimeGeocoding;
                sourceFile.IsSpecialFile = thisContainer.IsSpecialFile;
                sourceFile.ClientCustomProcedureID = thisContainer.ClientCustomProcedureID;
                sourceFile.SpecialFileResultID = thisContainer.SpecialFileResultID;
                sourceFile.DateCreated = DateTime.Now;
                sourceFile.DateUpdated = DateTime.Now;
                sourceFile.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;                    
                sourceFile.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;                    
                sourceFile.QDateFormat = thisContainer.qDateFormat;
                sourceFile.BatchSize = thisContainer.batchSize;

                //FrameworkUAS.BusinessLogic.SourceFile sfWrk = new FrameworkUAS.BusinessLogic.SourceFile();
                //thisContainer.sourceFileID = sfWrk.Save(sourceFile);

                thisContainer.sourceFileID = serviceClientSet.SourceFile.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, sourceFile).Result;
                if (thisContainer.sourceFileID > 0)
                {
                    sourceFile.SourceFileID = thisContainer.sourceFileID;
                    List<FrameworkUAS.Entity.SourceFile> sourceFilesList = new List<FrameworkUAS.Entity.SourceFile>();
                    //If Client SourceFiles exist cool just remove potential old and add the new
                    if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties.ContainsKey(thisContainer.myClient.ClientID))
                    {
                        if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[thisContainer.myClient.ClientID].SourceFilesList.FirstOrDefault(x => x.SourceFileID == thisContainer.sourceFileID) != null)
                        {
                            FrameworkUAS.Entity.SourceFile sf = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[thisContainer.myClient.ClientID].SourceFilesList.FirstOrDefault(x => x.SourceFileID == thisContainer.sourceFileID);
                            FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[thisContainer.myClient.ClientID].SourceFilesList.Remove(sf);
                        }

                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[thisContainer.myClient.ClientID].SourceFilesList.Add(sourceFile);
                    }
                    else
                    {
                        sourceFilesList = serviceClientSet.SourceFile.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.myClient.ClientID, false).Result.Where(x => x.IsDeleted == false).ToList();
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties.Add(thisContainer.myClient.ClientID, new FrameworkUAS.Object.ClientAdditionalProperties());
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[thisContainer.myClient.ClientID].SourceFilesList = sourceFilesList;
                    }
                }

                #endregion
                #region SAVE FIELD MAPPINGS
                List<FrameworkUAS.Entity.FieldMapping> originalFieldMappings = new List<FrameworkUAS.Entity.FieldMapping>();
                originalFieldMappings = serviceClientSet.FieldMapping.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.AccessKey, thisContainer.sourceFileID).Result;
                int columnOrder = 1;
                List<ColumnMapper> removeMapping = new List<ColumnMapper>();
                foreach (ColumnMapper cm in flowLayout.Children)
                {                        
                    int fieldMap = 0;
                    if (cm.ButtonTag != null)
                        Int32.TryParse(cm.ButtonTag, out fieldMap);

                    if (cm.TypeOfRow == ColumnMapperRowType.Remove.ToString())
                    {
                        cm.TypeOfRow = ColumnMapperRowType.Normal.ToString();
                        removeMapping.Add(cm);
                        if (fieldMap > 0)
                        {
                            int tFM = serviceClientSet.TransformationFieldMapData.Proxy.DeleteFieldMapping(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, fieldMap).Result;
                            int iFM = serviceClientSet.FieldMapping.Proxy.DeleteMapping(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, fieldMap).Result;                       
                        }
                    } 
                    else
                        cm.TypeOfRow = ColumnMapperRowType.Normal.ToString();

                    string mappedTo = cm.ComboBoxMappingText;
                    string colName = cm.TextboxColumnText;
                    string previewData = cm.TextboxPreviewData;
                    bool ignore = false;

                    if (mappedTo == "Ignore" || mappedTo == "kmTransform")
                    {
                        ignore = true;
                    }                        

                    //FieldMapping Save Here
                    FrameworkUAS.Entity.FieldMapping mapping = new FrameworkUAS.Entity.FieldMapping();
                    mapping.FieldMappingID = fieldMap;
                    mapping.SourceFileID = thisContainer.sourceFileID;
                    mapping.IncomingField = colName;
                    mapping.MAFField = mappedTo;
                    mapping.PubNumber = 0;
                    mapping.DataType = "varchar";
                    if (!string.IsNullOrEmpty(previewData))
                    {
                        if (previewData.Length > 1000)
                            mapping.PreviewData = previewData.Substring(0, 1000).Replace("'", "");
                        else
                            mapping.PreviewData = previewData.Replace("'", "");

                    }
                    else
                        mapping.PreviewData = previewData;

                    mapping.IsNonFileColumn = cm.IsUserDefinedColumn;

                    if (fieldMappingTypes == null || fieldMappingTypes.Count == 0)
                    {
                        var fmtResp = serviceClientSet.LookUpCode.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping);
                        if (fmtResp.Result != null)
                            fieldMappingTypes = fmtResp.Result;
                    }
                    int fmlIgnoredId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString(), StringComparison.CurrentCultureIgnoreCase)).CodeId;
                    int fmlKmTransformId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.kmTransform.ToString(), StringComparison.CurrentCultureIgnoreCase)).CodeId;
                    int fmlStandardId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString(), StringComparison.CurrentCultureIgnoreCase)).CodeId;
                    int fmlDemoId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString(), StringComparison.CurrentCultureIgnoreCase)).CodeId;
                    int fmlDemoOtherId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Other.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase)).CodeId;
                    int fmlDemoDateId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Date.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase)).CodeId;

                    if (cm.FieldMapType == FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString())
                        mapping.FieldMappingTypeID = fmlIgnoredId;
                    else if (cm.FieldMapType == FrameworkUAD_Lookup.Enums.FieldMappingTypes.kmTransform.ToString())
                        mapping.FieldMappingTypeID = fmlKmTransformId;
                    else if (cm.FieldMapType == FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString())
                        mapping.FieldMappingTypeID = fmlDemoId;
                    else if (cm.FieldMapType == FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Other.ToString())
                        mapping.FieldMappingTypeID = fmlDemoOtherId;
                    else if (cm.FieldMapType == FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Date.ToString())
                        mapping.FieldMappingTypeID = fmlDemoDateId;
                    else
                        mapping.FieldMappingTypeID = fmlStandardId;

                    mapping.ColumnOrder = columnOrder;
                    if (originalFieldMappings.Count > 0)
                    {
                        if (originalFieldMappings.FirstOrDefault(x => x.FieldMappingID == fieldMap) != null && cm.TypeOfRow != ColumnMapperRowType.Remove.ToString())
                            mapping.HasMultiMapping = originalFieldMappings.FirstOrDefault(x => x.FieldMappingID == fieldMap).HasMultiMapping;
                        else
                            mapping.HasMultiMapping = false;

                    }
                    else
                        mapping.HasMultiMapping = false;
                    //if (cm.spUADAdded.Children.Count > 0)
                    //    mapping.HasMultiMapping = true;
                    //else
                    //    mapping.HasMultiMapping = false;

                    mapping.DemographicUpdateCodeId = cm.DemographicUpdateCodeId;

                    mapping.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    mapping.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    mapping.DateCreated = DateTime.Now;

                    if (cm.IsUserDefinedColumn == true)
                        mapping.IncomingField = mappedTo;

                    //ASSIGN FIELDMAPPINGID TO DELETE BUTTON TAG
                    int i = serviceClientSet.FieldMapping.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, mapping).Result;
                    fieldMap = i;
                    cm.ButtonTag = i.ToString();
                    columnOrder++;

                    cm.AddSourceFileID = thisContainer.sourceFileID;
                    cm.AddFieldMappingID = i;
                }
                foreach (ColumnMapper cm in removeMapping)
                {
                    flowLayout.Children.Remove(cm);
                }
                serviceClientSet.FieldMapping.Proxy.ColumnReorder(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey,thisContainer.sourceFileID);
                #endregion
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Invalid Field Mapping." + thisContainer.FieldMappingErrorMessage, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Field Mapping");
                return;
            }
            #endregion

            #region Show Next Tile
            thisContainer.MapColumnsToNewColumns();
            var borderList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.Border>(thisContainer);
            if (borderList.FirstOrDefault(x => x.Name.Equals("StepThreeContainer", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                Border thisBorder = borderList.FirstOrDefault(x => x.Name.Equals("StepThreeContainer", StringComparison.CurrentCultureIgnoreCase));
                thisBorder.Child = new FileMapperWizard.Controls.NewColumns(thisContainer);
            }
            #endregion
        }           
        
        private void ButtonOpenPreview_Click(object sender, RoutedEventArgs e)
        {
            Expansion();
        }
        public void Expansion(bool closeNow = false)
        {
            bool setExpanded = true;
            foreach (ColumnMapper mc in flowLayout.Children)
            {
                if (closeNow)
                {
                    mc.ClosePreviewData();
                    setExpanded = false;
                    continue;
                }

                if (thisContainer.Expanded)
                {
                    mc.ClosePreviewData();
                    setExpanded = false;
                }
                else
                {
                    mc.OpenPreviewData();
                    setExpanded = true;
                }
            }
            thisContainer.Expanded = setExpanded;
            if (thisContainer.Expanded == true)
                ButtonOpenPreview.Content = "Minimize Details";
            else
                ButtonOpenPreview.Content = "View Details";

        }

        private void ButtonRescan_Click(object sender, RoutedEventArgs e)
        {
            //Get source file columns
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name            
            dlg.Filter = "Recognized Files(*.txt;*.csv;*.xls;*.xlsx;*.dbf;*.xml;*.json)|*.txt;*.csv;*.xls;*.xlsx;*.dbf;*.xml;*.json";
            // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result != true) return;

            myRescanFile = dlg.FileName;
            if (System.IO.Path.GetExtension(myRescanFile).ToLower() == ".csv" || System.IO.Path.GetExtension(myRescanFile).ToLower() == ".txt")
            {
                LabelDelimiter.Visibility = Visibility.Visible;
                LabelTextQualifier.Visibility = Visibility.Visible;
                ComboBoxDelimiter.Visibility = Visibility.Visible;
                ComboBoxQualifier.Visibility = Visibility.Visible;
                ButtonStart.Visibility = Visibility.Visible;
            }
            else
            {
                if (Precheck())
                    Rescan(myRescanFile);

            }
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            bool result = Precheck();
            if (result)
            {
                busyIcon.IsBusy = true;
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (o, ea) =>
                {

                };
                bw.RunWorkerCompleted += (o, ea) =>
                {
                    Rescan(myRescanFile);
                };
                bw.RunWorkerAsync();                                
                busyIcon.IsBusy = false;
            }
        }

        private bool Precheck()
        {
            if (myRescanFile == "")
            {
                Core_AMS.Utilities.WPF.Message("Error file was lost during process.", MessageBoxButton.OK, MessageBoxImage.Information, "Invalid Operation");
                return false;
            }

            FileInfo fInfo = new FileInfo(myRescanFile);
            if (IsFileLocked(fInfo))
            {
                Core_AMS.Utilities.WPF.Message("Error trying to talk to file. Is the file currently opened?", MessageBoxButton.OK, MessageBoxImage.Warning, "File Exception");
                return false;
            }

            if (System.IO.Path.GetExtension(myRescanFile).ToLower() == ".csv" || System.IO.Path.GetExtension(myRescanFile).ToLower() == ".txt")
            {
                if (ComboBoxDelimiter.SelectedValue != null)
                    rescanDelimiter = ComboBoxDelimiter.SelectedValue.ToString();
                else
                {
                    Core_AMS.Utilities.WPF.Message("Delimiter and Text Qualifier must be selected.", MessageBoxButton.OK, MessageBoxImage.Information, "Invalid Operation");
                    return false;
                }

                if (ComboBoxQualifier.SelectedValue != null)
                    rescanQualifier = ComboBoxQualifier.SelectedValue.ToString();
                else
                {
                    Core_AMS.Utilities.WPF.Message("Delimiter and Text Qualifier must be selected.", MessageBoxButton.OK, MessageBoxImage.Information, "Invalid Operation");
                    return false;
                }
                rescanFileConfig = new FileConfiguration()
                {
                    FileColumnDelimiter = rescanDelimiter,
                    IsQuoteEncapsulated = Convert.ToBoolean(rescanQualifier)
                };
            }

            return true;
        }

        private static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }

        public void Rescan(string rescanFile)
        {
            FileWorker fw = new FileWorker();
            List<string> duplicates = fw.GetDuplicateColumns(new FileInfo(rescanFile), rescanFileConfig);
            if (duplicates.Any())
            {
                Core_AMS.Utilities.WPF.Message("File contains duplicate columns: " + String.Join(", ", duplicates), MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, "Duplicates Found");
                LabelDelimiter.Visibility = Visibility.Collapsed;
                LabelTextQualifier.Visibility = Visibility.Collapsed;
                ComboBoxDelimiter.Visibility = Visibility.Collapsed;
                ComboBoxQualifier.Visibility = Visibility.Collapsed;
                ButtonStart.Visibility = Visibility.Collapsed;
                return;
            }

            flowLayout.IsEnabled = false;
            //List<string> newList = new List<string>();
            StringDictionary newList = new StringDictionary();
            List<RescanMapping> oldList = new List<RescanMapping>();
            List<FrameworkUAS.Entity.FieldMapping> missingList = new List<FrameworkUAS.Entity.FieldMapping>();

            newList = fw.GetFileHeaders(new FileInfo(rescanFile), rescanFileConfig, false);

            foreach (ColumnMapper c in flowLayout.Children)
            {
                //int pub = 0;
                //if (c.TextboxPubnumText != "")
                //    pub = Convert.ToInt32(c.TextboxPubnumText);

                int fid = 0;
                int.TryParse(c.ButtonTag, out fid);

                oldList.Add(new RescanMapping()
                {
                    FieldMapID = fid,
                    IncomingField = c.TextboxColumnText,
                    DataType = "varchar",//c.ComboBoxDatatypeText,
                    MAFField = c.ComboBoxMappingText,
                    PubNumber = 0,
                    PreviewData = c.TextboxPreviewData,
                    controlType = c.MapperControlType,
                    //isDemo = Convert.ToBoolean(c.GetMapToDemoData),
                    fieldMapType = c.FieldMapType,
                    isUserDefined = c.IsUserDefinedColumn,
                    fieldMultiMappings = c.multiMappings,
                    DemographicUpdateCodeId = c.DemographicUpdateCodeId
                });
            }

            flowLayout.Children.Clear();
            bool firstControl = true;
            ColumnMapper mc;
            Dictionary<int, string> columns = new Dictionary<int, string>();
            foreach (DictionaryEntry col in newList)
            {
                int colOrder = 0;
                int.TryParse(col.Value.ToString(), out colOrder);
                columns.Add(colOrder, col.Key.ToString());
            }
            foreach (KeyValuePair<int, string> col in columns.OrderBy(x => x.Key))
            {
            //foreach (KeyValuePair<int, string> newItem in newList)
            //{
                if (oldList.Any(r => r.IncomingField.Equals(col.Value.ToString(), StringComparison.CurrentCultureIgnoreCase)))
                {
                    var item = oldList.Find(v => v.IncomingField.Equals(col.Value.ToString(), StringComparison.CurrentCultureIgnoreCase));

                    oldList.Remove(item);

                    FrameworkUAD_Lookup.Entity.Code demoUpdateOption = DemoUpdateOptions.FirstOrDefault(x => x.CodeId == item.DemographicUpdateCodeId);

                    mc = new ColumnMapper("", FrameworkUAS.Object.AppData.myAppData, thisContainer.myClient, thisContainer.uadColumns, ColumnMapperControlType.Edit.ToString(), col.Value.ToString(), item.PreviewData, DemoUpdateOptions, adhocBitFields, isRequiredFields, demoUpdateOption, item.MAFField, item.fieldMapType);
                    mc.AddSourceFileID = thisContainer.sourceFileID;
                    mc.AddFieldMappingID = item.FieldMapID;                       
                    mc.CloseLabelRow();

                    mc.ButtonTag = item.FieldMapID.ToString();

                    firstControl = false;
                    flowLayout.Children.Add(mc);
                }
                else
                {                    
                    mc = new ColumnMapper("", FrameworkUAS.Object.AppData.myAppData, thisContainer.myClient, thisContainer.uadColumns, ColumnMapperControlType.New.ToString(), col.Value.ToString(), "", DemoUpdateOptions, adhocBitFields, isRequiredFields, null, "", "", ColumnMapperRowType.New.ToString());
                    mc.AddSourceFileID = thisContainer.sourceFileID;
                    mc.AddFieldMappingID = 0;                          
                    mc.CloseLabelRow();

                    firstControl = false;
                    flowLayout.Children.Add(mc);
                }
            }
            
            foreach (RescanMapping oldItem in oldList)
            {
                if (!newList.ContainsKey(oldItem.IncomingField))
                {
                    string pub = "";
                    if (oldItem.PubNumber != 0)
                        pub = oldItem.PubNumber.ToString();

                    if (oldItem.IncomingField == "" || oldItem.isUserDefined == true)
                    {
                        FrameworkUAD_Lookup.Entity.Code demoUpdateOption = DemoUpdateOptions.FirstOrDefault(x => x.CodeId == oldItem.DemographicUpdateCodeId);

                        mc = new ColumnMapper("", FrameworkUAS.Object.AppData.myAppData, thisContainer.myClient, thisContainer.uadColumns, ColumnMapperControlType.EditUser.ToString(), oldItem.IncomingField, oldItem.PreviewData, DemoUpdateOptions, adhocBitFields, isRequiredFields, demoUpdateOption, oldItem.MAFField, oldItem.fieldMapType);
                        mc.AddSourceFileID = thisContainer.sourceFileID;
                        mc.AddFieldMappingID = oldItem.FieldMapID;                               
                        mc.CloseLabelRow();
                        mc.ButtonTag = oldItem.FieldMapID.ToString();
                    }
                    else
                    {
                        FrameworkUAD_Lookup.Entity.Code demoUpdateOption = DemoUpdateOptions.FirstOrDefault(x => x.CodeId == oldItem.DemographicUpdateCodeId);

                        mc = new ColumnMapper("", FrameworkUAS.Object.AppData.myAppData, thisContainer.myClient, thisContainer.uadColumns, ColumnMapperControlType.Edit.ToString(), oldItem.IncomingField, oldItem.PreviewData, DemoUpdateOptions, adhocBitFields, isRequiredFields,
                            demoUpdateOption, oldItem.MAFField, oldItem.fieldMapType,ColumnMapperRowType.Remove.ToString(),true);
                        mc.AddSourceFileID = thisContainer.sourceFileID;
                        mc.AddFieldMappingID = oldItem.FieldMapID;
                        mc.ButtonTag = oldItem.FieldMapID.ToString();
                        mc.CloseLabelRow();
                    }
                    flowLayout.Children.Add(mc);
                    firstControl = false;
                }
            }
            flowLayout.IsEnabled = true;
            LabelDelimiter.Visibility = Visibility.Collapsed;
            LabelTextQualifier.Visibility = Visibility.Collapsed;
            ComboBoxDelimiter.Visibility = Visibility.Collapsed;
            ComboBoxQualifier.Visibility = Visibility.Collapsed;
            ButtonStart.Visibility = Visibility.Collapsed;
        }

        public void LoadData()
        {
            var fmtResp = serviceClientSet.LookUpCode.Proxy.Select(AppData.myAppData.AuthorizedUser.AuthAccessKey, Enums.CodeType.Field_Mapping);
            if (fmtResp?.Result != null)
            {
                fieldMappingTypes = fmtResp.Result;
            }

            var busyIcon = WPF.FindControl<RadBusyIndicator>(thisContainer, "busyIcon");
            busyIcon.IsBusy = true;
            InitializeDelimitersAndQualifiers();
            InitializeDemoUpdateOptions();
            LoadSemLists();
            InitializeIsFieldRequired();

            StartBackgroundInitialization();
        }

        private void InitializeDelimitersAndQualifiers()
        {
            foreach (var delimiter in Enum.GetValues(typeof(CommonEnums.ColumnDelimiter)))
            {
                ComboBoxDelimiter.Items.Add(delimiter.ToString().Replace(SpaceEscaped, Whitespace));
            }

            foreach (var dl in Enum.GetValues(typeof(Core_AMS.Utilities.Enums.TrueFalse)))
            {
                ComboBoxQualifier.Items.Add(dl.ToString().Replace(SpaceEscaped, Whitespace));
            }
        }

        private void StartBackgroundInitialization()
        {
            var backgroundWorker = new BackgroundWorker();
            if (!thisContainer.isEdit)
            {
                backgroundWorker.DoWork += BackgroundLoadNewItem;
                backgroundWorker.RunWorkerCompleted += BackgroundLoadNewItemCompleted;
            }
            else
            {
                backgroundWorker.DoWork += BackgroundLoadEditItem;
                backgroundWorker.RunWorkerCompleted += BackgroundLoadEditItemComplete;
            }

            backgroundWorker.RunWorkerAsync();
        }

        private void InitializeIsFieldRequired()
        {
            var pubId = 0;
            if (thisContainer.selectedProduct != null)
            {
                int.TryParse(thisContainer.selectedProduct.PubID.ToString(), out pubId);
            }
            else if (thisContainer.currentPublication != null)
            {
                int.TryParse(thisContainer.currentPublication.PubID.ToString(), out pubId);
            }

            isRequiredFields = serviceClientSet.ResponseGroupWorker.Proxy.Select(
                    AppData.myAppData.AuthorizedUser.AuthAccessKey,
                    thisContainer.myClient.ClientConnections)
                .Result
                .Where(x => x.IsRequired == true && x.PubID == pubId)
                .Select(x => x.ResponseGroupName)
                .ToList();
        }

        private void InitializeDemoUpdateOptions()
        {
            var demoUpdateTypes = serviceClientSet.LookUpCode.Proxy.Select(
                            AppData.myAppData.AuthorizedUser.AuthAccessKey,
                            Enums.CodeType.Demographic_Update)
                            .Result;
            DemoUpdateOptions = demoUpdateTypes?.Where(x => x.IsActive).ToList() ?? new List<Code>();
        }

        private void LoadSemLists()
        {
            var semList = serviceClientSet.SubscriptionsExtensionMapperWorker.Proxy.SelectAll(
                    AppData.myAppData.AuthorizedUser.AuthAccessKey,
                    thisContainer.myClient.ClientConnections)
                .Result;
            var psemList = serviceClientSet.ProductSubscriptionsExtensionWorker.Proxy.SelectAll(
                    AppData.myAppData.AuthorizedUser.AuthAccessKey,
                    thisContainer.myClient.ClientConnections)
                .Result;

            adhocBitFields.AddRange(
                semList.Where(x => x.CustomFieldDataType == BitDataTypeName)
                    .Select(x => x.CustomField).ToList());

            adhocBitFields.AddRange(
                psemList.Where(x => x.CustomFieldDataType == BitDataTypeName)
                    .Select(x => x.CustomField).ToList());
        }

        private void BackgroundLoadEditItemComplete(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            var loadResult = (MapColumnsLoadResult)runWorkerCompletedEventArgs.Result;

            if (fieldMappingTypes == null || fieldMappingTypes.Count == 0)
            {
                var fmtResp = serviceClientSet.LookUpCode.Proxy.Select(AppData.myAppData.AuthorizedUser.AuthAccessKey, Enums.CodeType.Field_Mapping);
                if (fmtResp.Result != null)
                {
                    fieldMappingTypes = fmtResp.Result;
                }
            }

            thisContainer.fieldMappingsWithTransformations.AddRange(loadResult.Alltfm.Select(x => x.FieldMappingID).Distinct());
            if (thisContainer.uadColumns.Count > 0)
            {
                thisContainer.currentFieldMappings = loadResult.Mapping as List<FieldMapping>;
                foreach (var eml in loadResult.Mapping)
                {
                    if (eml.IsNonFileColumn)
                    {
                        continue;
                    }

                    var controlType = ColumnMapperControlType.Edit.ToString();
                    if (string.IsNullOrWhiteSpace(eml.IncomingField)|| eml.IsNonFileColumn)
                    {
                        controlType = ColumnMapperControlType.EditUser.ToString();
                    }

                    var demoUpdateOption =
                        DemoUpdateOptions.FirstOrDefault(x => x.CodeId == eml.DemographicUpdateCodeId);

                    var mc = new ColumnMapper(
                        EditUserType,
                        AppData.myAppData,
                        thisContainer.myClient,
                        thisContainer.uadColumns,
                        controlType,
                        eml.IncomingField,
                        eml.PreviewData,
                        DemoUpdateOptions,
                        adhocBitFields,
                        isRequiredFields,
                        demoUpdateOption,
                        eml.MAFField)
                    {
                        AddSourceFileID = eml.SourceFileID,
                        AddFieldMappingID = eml.FieldMappingID,
                        AddMultiMapColumn = eml.FieldMultiMappings.ToList(),
                        AddTranFieldMultiMap = loadResult.Alltfmm as List<TransformationFieldMultiMap>
                    };
                    mc.CloseLabelRow();

                    mc.HasTransformation = loadResult.Alltfm.FirstOrDefault(x => x.FieldMappingID.Equals(eml.FieldMappingID)) != null;

                    mc.ButtonTag = eml.FieldMappingID.ToString();

                    flowLayout.Children.Add(mc);
                }
            }
            else
            {
                WPF.Message(
                    IncorrectContentMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error,
                    MessageBoxResult.OK,
                    MissingDataCaption);
                busyIcon.IsBusy = false;
                return;
            }

            busyIcon.IsBusy = false;
        }

        private void BackgroundLoadEditItem(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            thisContainer.uadColumns = serviceClientSet.FileMappingColumn.Proxy.Select(AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.myClient.ClientConnections).Result;
            thisContainer.pubCode = serviceClientSet.DbWorker.Proxy.GetPubIDAndCodesByClient(AppData.myAppData.AuthorizedUser.AuthAccessKey, thisContainer.myClient).Result;
            doWorkEventArgs.Result = new MapColumnsLoadResult
            {
                Mapping = serviceClientSet.FieldMapping.Proxy.Select(
                    AppData.myAppData.AuthorizedUser.AuthAccessKey,
                    thisContainer.sourceFileID)
                    .Result
                    .OrderBy(x => x.ColumnOrder)
                    .ToList(),
                Alltfm = serviceClientSet.TransformationFieldMapData.Proxy.Select(AppData.myAppData.AuthorizedUser.AuthAccessKey)
                    .Result
                    .Where(x => x.SourceFileID.Equals(thisContainer.sourceFileID))
                    .ToList(),
                Alltfmm = serviceClientSet.TransformationFieldMultiMap.Proxy.Select(AppData.myAppData.AuthorizedUser.AuthAccessKey).Result
            };
        }

        private void BackgroundLoadNewItemCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            if (thisContainer.columns.Count > 0)
            {
                if (thisContainer.uadColumns.Count > 0)
                {
                    var order = 1;
                    var columns = new Dictionary<int, string>();
                    foreach (DictionaryEntry col in thisContainer.columns)
                    {
                        var colOrder = 0;
                        int.TryParse(col.Value.ToString(), out colOrder);
                        columns.Add(colOrder, col.Key.ToString());
                    }

                    foreach (var col in columns.OrderBy(x => x.Key))
                    {
                        if (string.IsNullOrEmpty(col.Value))
                        {
                            continue;
                        }

                        var dataPreview = string.Empty;
                        try
                        {
                            dataPreview = string.Join(",", thisContainer.fileData.AsEnumerable().Select(s => s.Field<object>(col.Value)).Distinct().ToArray());
                        }
                        catch
                        {
                            dataPreview = PreviewErrorText;
                        }

                        var mc = new ColumnMapper(
                            NormalType,
                            AppData.myAppData,
                            thisContainer.myClient,
                            thisContainer.uadColumns,
                            ColumnMapperControlType.New.ToString(),
                            col.Value,
                            dataPreview,
                            DemoUpdateOptions,
                            adhocBitFields,
                            isRequiredFields);
                        mc.CloseLabelRow();

                        flowLayout.Children.Add(mc);
                        thisContainer.columnMapperList.Add(order, mc);
                        order++;
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
                    busyIcon.IsBusy = false;
                    return;
                }
            }
            else
            {
                WPF.Message(
                    NoColumnsMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error,
                    MessageBoxResult.OK,
                    MissingDataCaption);
                thisContainer.MapToSetupColumns();
            }

            busyIcon.IsBusy = false;
        }

        private void BackgroundLoadNewItem(object sender, DoWorkEventArgs args)
        {
            thisContainer.fw.GetDuplicateColumns(thisContainer.fileInfo, thisContainer.myFileConfig);
            try
            {
                thisContainer.columns = thisContainer.fw.GetFileHeaders(
                    thisContainer.fileInfo,
                    thisContainer.myFileConfig,
                    false);
            }
            catch (Exception)
            {
            }

            var rowCounter = 0;
            rowCounter = thisContainer.fw.GetRowCount(thisContainer.fileInfo);
            if (rowCounter > 0)
            {
                thisContainer.fileData =
                    thisContainer.fw.GetData(thisContainer.fileInfo, thisContainer.myFileConfig);
            }

            thisContainer.uadColumns = serviceClientSet.FileMappingColumn.Proxy.Select(
                AppData.myAppData.AuthorizedUser.AuthAccessKey,
                thisContainer.myClient.ClientConnections)
                .Result;
            thisContainer.pubCode = serviceClientSet.DbWorker.Proxy.GetPubIDAndCodesByClient(
                AppData.myAppData.AuthorizedUser.AuthAccessKey,
                thisContainer.myClient)
                .Result;
        }
    }

    [Serializable]
    [DataContract]
    public class RescanMapping
    {
        public RescanMapping() { }
        [DataMember]
        public int FieldMapID { get; set; }
        [DataMember]
        public string IncomingField { get; set; }
        [DataMember]
        public string DataType { get; set; }
        [DataMember]
        public string MAFField { get; set; }
        [DataMember]
        public int PubNumber { get; set; }
        [DataMember]
        public string PreviewData { get; set; }
        [DataMember]
        public string controlType { get; set; }
        [DataMember]
        public string fieldMapType { get; set; }
        [DataMember]
        public bool isUserDefined { get; set; }
        [DataMember]
        public List<FrameworkUAS.Entity.FieldMultiMap> fieldMultiMappings { get; set; }
        [DataMember]
        public int DemographicUpdateCodeId { get; set; }
    }
}
