using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using FrameworkUAD_Lookup;
using KM.Common.Import;
using CommonEnums = KM.Common.Enums;

namespace UAS.Web.Models.FileMapperWizard
{
    ///<summary>
    /// 1 _step1 Setup 
    /// 2 _step2 ColumnMapping
    /// 3 _step3 Rules - ADMS | Custom
    /// 4 _step4 Review
    ///</summary>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class FileMapperWizardViewModel
    {
        #region Properties
        public int currentStep { get; set; }

        FrameworkUAD_Lookup.BusinessLogic.Code workerCode;
        KMPlatform.BusinessLogic.ServiceFeature servFeatureWrk;

        public SetupViewModel setupViewModel { get; set; }
        public ColumnMappingViewModel columnMappingViewModel { get; set; }
        //public TransformationsViewModel transformationsViewModel { get; set; }
        //public RulesViewModel rulesViewModel { get; set; }
        public FrameworkUAS.Model.RuleSet ruleSet { get; set; }//new object for RulesViewModel

        public ReviewViewModel reviewViewModel { get; set; }
        public StandardTransformationDataModel standardTransformationDataModel { get; set; }
        public UserCreatedTransformations userCreatedTransformations { get; set; }

        public KMPlatform.Entity.Client client { get; set; }
        public KMPlatform.Entity.User user { get; set; }
        public bool isCirculation { get; set; }
        public bool isNewFile { get; set; }//isEdit
        public string filePath { get; set; }
        public int sourceFileId { get; set; }
        public FrameworkUAD.Entity.Product product { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> DatabaseFileTypeList { get; set; }
        public int ignoreTypeID { get; set; }// = 0;
        public int demoTypeID { get; set; }
        public int standardTypeID { get; set; }
        public List<KMPlatform.Entity.Service> AllServices { get; set; }// = new List<KMPlatform.Entity.Service>();
        public KMPlatform.Entity.Service myService { get; set; }// = new KMPlatform.Entity.Service();
        public List<KMPlatform.Entity.ServiceFeature> AllFeatures { get; set; }// = new List<KMPlatform.Entity.ServiceFeature>();

        public string FieldMappingErrorMessage { get; set; }
        #endregion

        public FileMapperWizardViewModel()
        {

        }

        public FileMapperWizardViewModel(bool isNewFile, bool isCirc = false, int currentClientID = 0, KMPlatform.Entity.User currentuser = null, KMPlatform.Entity.Client client = null, FrameworkUAD.Entity.Product prod = null, string filePath = "")
        {
            workerCode = new FrameworkUAD_Lookup.BusinessLogic.Code();
            servFeatureWrk = new KMPlatform.BusinessLogic.ServiceFeature();
            if (client != null)
                this.client = client;
            else
                this.client = new KMPlatform.Entity.Client();
            this.user = currentuser;
            if (!string.IsNullOrEmpty(filePath))
                this.filePath = filePath;
            else
                this.filePath = "";
            this.isCirculation = isCirc;
            this.isNewFile = isNewFile;
            if (prod != null)
                this.product = prod;
            else
                this.product = null;
            DatabaseFileTypeList = new List<FrameworkUAD_Lookup.Entity.Code>();

            #region Load Services
            KMPlatform.BusinessLogic.Service workerService = new KMPlatform.BusinessLogic.Service();
            this.AllServices = workerService.Select(true);
            #endregion
            #region Populate CODE
            List<FrameworkUAD_Lookup.Entity.Code> codes = workerCode.Select(FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping);
            this.standardTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString())).CodeId;
            this.demoTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString())).CodeId;
            this.ignoreTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString())).CodeId;
            #endregion
            #region Service Selection
            //LOAD FEATURES THAT ARE ENABLED FOR CLIENT
            List<KMPlatform.Entity.ServiceFeature> featuresList = new List<KMPlatform.Entity.ServiceFeature>();
            if (isCirc)
            {
                #region Circ Features
                myService = AllServices.SingleOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
                if (myService != null)
                {
                    KMPlatform.Entity.Service selectedService = AllServices.Single(x => x.ServiceID == myService.ServiceID);
                    AllFeatures = servFeatureWrk.Select();
                    List<KMPlatform.Entity.ServiceFeature> currentClientFeatures = new List<KMPlatform.Entity.ServiceFeature>();
                    foreach (KMPlatform.Entity.ServiceFeature ss in selectedService.ServiceFeatures)
                    {
                        if (selectedService.ServiceFeatures.Count > 0 && ss.IsEnabled)
                        {
                            //Outside User
                            if (client.ClientID > 1 && ss.KMAdminOnly == false)
                                featuresList.Add(AllFeatures.SingleOrDefault(x => x.ServiceFeatureID == ss.ServiceFeatureID));

                            //KM Employee
                            if (client.ClientID == 1)
                                featuresList.Add(AllFeatures.SingleOrDefault(x => x.ServiceFeatureID == ss.ServiceFeatureID));
                        }
                    }
                }
                else
                {
                    return;
                }
                #endregion
            }
            else
            {
                #region UAD Features
                myService = AllServices.SingleOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.UADFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
                if (myService != null)
                {
                    KMPlatform.Entity.Service selectedService = AllServices.Single(x => x.ServiceID == myService.ServiceID);
                    AllFeatures = servFeatureWrk.Select();
                    List<KMPlatform.Entity.ServiceFeature> currentClientFeatures = new List<KMPlatform.Entity.ServiceFeature>();
                    foreach (KMPlatform.Entity.ServiceFeature ss in selectedService.ServiceFeatures)
                    {
                        if (selectedService.ServiceFeatures.Count > 0 && ss.IsEnabled)
                        {
                            //Outside User
                            if (client.ClientID > 1 && ss.KMAdminOnly == false)
                                featuresList.Add(AllFeatures.SingleOrDefault(x => x.ServiceFeatureID == ss.ServiceFeatureID));

                            //KM Employee
                            if (client.ClientID == 1)
                                featuresList.Add(AllFeatures.SingleOrDefault(x => x.ServiceFeatureID == ss.ServiceFeatureID));
                        }
                    }
                }
                else
                {
                    return;
                }
                #endregion
            }
            #endregion

            this.setupViewModel = new SetupViewModel(client.ClientID, featuresList);
            if (isCirc)
            {
                KMPlatform.Entity.Service circService = AllServices.SingleOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
                this.setupViewModel.ServiceID = circService.ServiceID;
            }
            else
            {
                KMPlatform.Entity.Service uadService = AllServices.SingleOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.UADFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
                this.setupViewModel.ServiceID = uadService.ServiceID;
            }
            this.columnMappingViewModel = new ColumnMappingViewModel();
            //this.rulesViewModel = new RulesViewModel();
            this.ruleSet = new FrameworkUAS.Model.RuleSet();
            this.reviewViewModel = new ReviewViewModel();
            this.standardTransformationDataModel = new StandardTransformationDataModel();
            this.userCreatedTransformations = new UserCreatedTransformations();

            #region Service Selection
            //LOAD FEATURES THAT ARE ENABLED FOR CLIENT
            //int serviceID = 0;
            this.myService = this.AllServices.SingleOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.UADFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
            if (this.myService != null)
            {
                KMPlatform.Entity.Service selectedService = this.AllServices.Single(x => x.ServiceID == myService.ServiceID);
                this.AllFeatures = servFeatureWrk.Select();
                List<KMPlatform.Entity.ServiceFeature> currentClientFeatures = new List<KMPlatform.Entity.ServiceFeature>();
            }
            else
            {
                return;
            }
            #endregion
        }
    }

    /// <summary>
    /// Validation tags here are ONLY used on SERVER side
    /// </summary>
    public class SetupViewModel
    {
        #region Properties
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Save FileName As:")]
        public string FileSaveAsName { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        [Display(Name = "Select a file to map:")]
        public HttpPostedFileBase DataFile { get; set; }

        [Display(Name = "New or Existing file:")]
        public bool IsNewFile { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Select a Process:")]
        public int ServiceFeatureID { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Is this a full file:")]
        public bool IsFullFile { get; set; }

        [Display(Name = "Select Matching:")]
        public string Matching { get; set; }//Default, IgrpNumber, SequenceId, ExternalKeyId
        [Display(Name = "File Delimiter:")]
        public string Delimeter { get; set; }
        [Display(Name = "Has Text Qualifier:")]
        public bool HasQuotation { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Select QDate Format:")]
        public string QDateFormat { get; set; } //MMDDYYYY

        public int ClientId { get; set; }
        public int SourceFileId { get; set; }
        public FileInfo IncomingFile { get; set; }

        public List<KMPlatform.Entity.Service> ServicesList { get; set; } //= new List<KMPlatform.Entity.Service>();
        public List<KMPlatform.Entity.ServiceFeature> FeaturesList { get; set; } //= new List<KMPlatform.Entity.ServiceFeature>();
        public Dictionary<string, string> Delimiters { get; set; } //= new Dictionary<string, string>
        public Dictionary<string, string> DateFormats { get; set; } //= new Dictionary<string, string>();
        public Dictionary<string, string> MatchOptions { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> DatabaseFileTypeList { get; set; }
        public List<FrameworkUAD.Entity.Product> Products { get; set; }
        public bool isCirc { get; set; }
        [Display(Name = "Select a Product:")]
        public int PublicationID { get; set; }
        [Display(Name = "Select a Feature:")]
        public int DatabaseFileTypeID { get; set; }
        public int ServiceID { get; set; }
        public string Extension { get; set; }

        public List<Common.UASError> ErrorList { get; set; }
        #endregion

        #region Entities
        FrameworkUAD_Lookup.BusinessLogic.Code workerCode;
        KMPlatform.BusinessLogic.ServiceFeature sf;
        #endregion

        public SetupViewModel()
        {
            ClientId = 0;
            SourceFileId = 0;
            ServiceFeatureID = 0;
            FileSaveAsName = "";
            IsFullFile = false;
            Delimeter = "";
            HasQuotation = false;
            QDateFormat = "MMDDYYYY";
            DataFile = null;
            Matching = "Default";
            ServicesList = new List<KMPlatform.Entity.Service>();
            FeaturesList = new List<KMPlatform.Entity.ServiceFeature>();
            Delimiters = new Dictionary<string, string>();
            DateFormats = new Dictionary<string, string>();
            MatchOptions = new Dictionary<string, string>();
            IsNewFile = true;
            isCirc = false;
            PublicationID = 0;
            DatabaseFileTypeID = 0;
            ServiceID = 0;
            Extension = "";
            ErrorList = new List<Common.UASError>();
        }
        public SetupViewModel(int clientId, List<KMPlatform.Entity.ServiceFeature> featuresList)
        {
            ErrorList = new List<Common.UASError>();
            LoadData(clientId);
        }

        private void LoadData(int clientID)
        {
            ClientId = clientID;
            workerCode = new FrameworkUAD_Lookup.BusinessLogic.Code();
            sf = new KMPlatform.BusinessLogic.ServiceFeature();
        }
    }
    public class ReviewViewModel
    {
        public ReviewViewModel()
        {
            SourceFileId = 0;
            fieldMappings = new List<FrameworkUAS.Entity.FieldMapping>();
            transformationFieldMappings = new List<FrameworkUAS.Entity.TransformationFieldMap>();
            transformations = new List<FrameworkUAS.Entity.Transformation>();
            transformationTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
            demoUpdateTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
            fieldMappingTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
        }

        public ReviewViewModel(int sourceFileId, List<FrameworkUAS.Entity.FieldMapping> fieldMappingList, List<FrameworkUAS.Entity.TransformationFieldMap> transformationFieldMappingList,
                                    List<FrameworkUAS.Entity.Transformation> transformationList, List<FrameworkUAD_Lookup.Entity.Code> transformationTypeList,
                                    List<FrameworkUAD_Lookup.Entity.Code> demoUpdateList, List<FrameworkUAD_Lookup.Entity.Code> fieldMappingTypeList)
        {
            SourceFileId = sourceFileId;
            fieldMappings = fieldMappingList;
            transformationFieldMappings = transformationFieldMappingList;
            transformations = transformationList;
            transformationTypes = transformationTypeList;
            demoUpdateTypes = demoUpdateList;
            fieldMappingTypes = fieldMappingTypeList;
        }

        #region Properties
        public int SourceFileId { get; set; }
        public List<FrameworkUAD.Object.FileMappingColumn> MappingColumns { get; set; }

        public List<FrameworkUAS.Entity.FieldMapping> fieldMappings { get; set; }
        public List<FrameworkUAS.Entity.TransformationFieldMap> transformationFieldMappings { get; set; }
        public List<FrameworkUAS.Entity.Transformation> transformations { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> transformationTypes { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> demoUpdateTypes { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> fieldMappingTypes { get; set; }

        #endregion
        private void LoadData(KMPlatform.Entity.Client myClient)
        {

        }
    }

    #region column mapping / transformations / add columns
    public class ColumnMappingViewModel
    {
        #region Properties
        public int SourceFileID { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FilePath { get; set; }
        public bool IsNewFile { get; set; }
        private List<FrameworkUAD.Object.FileMappingColumn> MappingColumns { get; set; }//yes
        
        public List<ColumnMap> IncomingColumns { get; set; }
        //tried using this but the object is really large, changing to just fileName, fileExtension, filePath - Wagner 3/21/2017
        //public FileInfo IncomingFile { get; set; }
        public List<AddNewColumn> AdditionalColumns { get; set; }
        #endregion

        public ColumnMappingViewModel()
        {
            SourceFileID = 0;
            IsNewFile = true;
            MappingColumns = new List<FrameworkUAD.Object.FileMappingColumn>();
            IncomingColumns = new List<ColumnMap>();
            AdditionalColumns = new List<AddNewColumn>();
        }
        private List<FrameworkUAD.Object.FileMappingColumn> GetDataBaseColumns(KMPlatform.Entity.Client client)
        {
            FrameworkUAD.BusinessLogic.FileMappingColumn fmcWrk = new FrameworkUAD.BusinessLogic.FileMappingColumn();
            List<FrameworkUAD.Object.FileMappingColumn> uadColumns = fmcWrk.Select(client.ClientConnections);
            return uadColumns.Where(x => !x.IsDemographic && !x.IsDemographicOther).ToList();
        }
        public ColumnMappingViewModel(bool isNewFile, string filePath, int sourceFileId, KMPlatform.Entity.Client client, string delimeter, bool hasQuotation, int userID)
        {
            FrameworkUAS.BusinessLogic.FieldMapping fieldMappingWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
            FrameworkUAD.BusinessLogic.FileMappingColumn fmcWrk = new FrameworkUAD.BusinessLogic.FileMappingColumn();
            List<FrameworkUAD.Object.FileMappingColumn> uadColumns = fmcWrk.Select(client.ClientConnections);
            MappingColumns = uadColumns;//.Where(x => !x.IsDemographic && !x.IsDemographicOther).ToList();
            this.IncomingColumns = new List<FileMapperWizard.ColumnMap>();
            this.AdditionalColumns = new List<FileMapperWizard.AddNewColumn>();
            this.IsNewFile = isNewFile;
            this.SourceFileID = sourceFileId;
            FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> DemoUpdateTypes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);

            if (this.IsNewFile)
            {
                #region Get File Data
                FileInfo myFile = new System.IO.FileInfo(filePath);
                FileName = myFile.Name;
                FileExtension = myFile.Extension;
                FilePath = filePath;

                //use FilePath to parse file to get headers.
                Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();
                var fc = new FileConfiguration();
                fc.FileExtension = myFile.Extension;
                fc.FileColumnDelimiter = delimeter;
                fc.FileFolder = myFile.DirectoryName;
                fc.IsQuoteEncapsulated = hasQuotation;

                Core_AMS.Utilities.FileWorker fw = new Core_AMS.Utilities.FileWorker();
                StringDictionary headers = fw.GetFileHeaders(myFile, fc);
                DataTable dt = fw.GetDataTopRows(myFile, fc, 10);//excel files return all rows - change to using Telerik.Spreadsheet
                #endregion

                #region Reorder columns to match file column order
                Dictionary<int, string> columns = new Dictionary<int, string>();
                foreach (DictionaryEntry col in headers)
                {
                    int colOrder = 0;
                    int.TryParse(col.Value.ToString(), out colOrder);
                    columns.Add(colOrder, col.Key.ToString());
                }

                int order = 1;
                foreach (KeyValuePair<int, string> h in columns.OrderBy(x => x.Key))
                {
                    List<string> data = new List<string>();
                    int rowCount = 1;
                    while (rowCount <= 10)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            data.Add(dr[h.Value.ToString()].ToString());
                            rowCount++;
                            if (rowCount >= 10)
                                break;
                        }
                    }
                    #region Pre-Save FieldMapping
                    FrameworkUAS.Entity.FieldMapping fm = new FrameworkUAS.Entity.FieldMapping();
                    //fill out fm object
                    fm.ColumnOrder = order;
                    fm.CreatedByUserID = userID;
                    fm.DataType = "";                                   //parse for string, int, date
                    fm.DateCreated = DateTime.Now;
                    fm.DemographicUpdateCodeId = 0;                     // "need to set this if a demo column";
                    fm.FieldMappingID = 0;                              //will be 0 if new > 0 if an edit
                    fm.FieldMappingTypeID = 0;
                    //fm.FieldMultiMappings;                            //need someing on CM model
                    fm.HasMultiMapping = false;
                    fm.IncomingField = h.Value.ToString();
                    fm.IsNonFileColumn = false;                         //need to handle on CM
                    fm.MAFField = "Ignore";
                    fm.PreviewData = "";
                    //fm.PubNumber;                                     //This should be depricated from the FieldMapping table
                    fm.SourceFileID = sourceFileId;

                    if (IsNewFile == false)
                    {
                        fm.UpdatedByUserID = userID;
                        fm.DateUpdated = DateTime.Now;
                    }

                    int fieldMappingID = fieldMappingWorker.Save(fm);
                    order++;
                    #endregion
                    ColumnMap cm = new FileMapperWizard.ColumnMap(sourceFileId, MappingColumns, IsNewFile, h.Value.ToString(), string.Join(",", data), DemoUpdateTypes, fieldMappingID, fm.FieldMappingTypeID, fm.DemographicUpdateCodeId);
                    this.IncomingColumns.Add(cm);
                }
                #endregion                
            }
            else
            {
                List<FrameworkUAS.Entity.FieldMapping> loopList = new List<FrameworkUAS.Entity.FieldMapping>();
                List<FrameworkUAS.Entity.FieldMapping> additionalColumnsList = new List<FrameworkUAS.Entity.FieldMapping>();
                List<FrameworkUAS.Entity.FieldMapping> fieldMappings = new List<FrameworkUAS.Entity.FieldMapping>();
                fieldMappings = fieldMappingWorker.Select(sourceFileId, true);

                FrameworkUAS.BusinessLogic.TransformationFieldMap tfmWorker = new FrameworkUAS.BusinessLogic.TransformationFieldMap();
                List<FrameworkUAS.Entity.TransformationFieldMap> transformationFieldMaps = new List<FrameworkUAS.Entity.TransformationFieldMap>();
                transformationFieldMaps = tfmWorker.Select(sourceFileId);

                loopList = fieldMappings.Where(x => x.IsNonFileColumn == false).OrderBy(x => x.ColumnOrder).ToList();
                foreach (FrameworkUAS.Entity.FieldMapping fm in loopList)
                {
                    bool hasTransformations = false;
                    if (transformationFieldMaps != null && transformationFieldMaps.Count(x => x.FieldMappingID == fm.FieldMappingID) > 0)
                        hasTransformations = true;

                    ColumnMap cm = new FileMapperWizard.ColumnMap(sourceFileId, MappingColumns, IsNewFile, fm.IncomingField, fm.PreviewData, DemoUpdateTypes, fm.FieldMappingID, fm.FieldMappingTypeID, fm.DemographicUpdateCodeId, fm.MAFField, hasTransformations, fm.HasMultiMapping);
                    this.IncomingColumns.Add(cm);
                }

                additionalColumnsList = fieldMappings.Where(x => x.IsNonFileColumn == true).OrderBy(x => x.ColumnOrder).ToList();
                foreach (FrameworkUAS.Entity.FieldMapping fm in additionalColumnsList)
                {
                    bool hasTransformations = false;
                    if (transformationFieldMaps != null && transformationFieldMaps.Count(x => x.FieldMappingID == fm.FieldMappingID) > 0)
                        hasTransformations = true;

                    AddNewColumn cm = new FileMapperWizard.AddNewColumn(sourceFileId, MappingColumns, IsNewFile, fm.IncomingField, fm.PreviewData, DemoUpdateTypes, fm.FieldMappingID, fm.FieldMappingTypeID, fm.DemographicUpdateCodeId, fm.ColumnOrder, fm.MAFField, hasTransformations, fm.HasMultiMapping);
                    this.AdditionalColumns.Add(cm);
                }
            }
        }
    }
    public class ColumnMap
    {
        #region Properties
        public int FieldMapID { get; set; }//yes
        public int FieldMapTypeID { get; set; }//yes
        public int SourceFileID { get; set; }//yes
        public string SourceColumn { get; set; }//yes
        public string PreviewData { get; set; }//yes
        public string MappedColumn { get; set; }//yes
        private string MapperControlType;//added
        public List<CustomDropDownList> ProfileColumnList { get; set; }//yes
        //public List<SelectListItem> IncomingFileColumnList { get; set; }
        public bool IsEdit { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> DemoUpdateTypes { get; set; }
        public int DemoUpdateID { get; set; }
        public List<FrameworkUAD.Object.FileMappingColumn> MappingColumns { get; set; }
        public bool HasTransformations { get; set; }
        public bool HasMultiMap { get; set; }
        #endregion

        public ColumnMap()
        {
            FieldMapID = 0;
            FieldMapTypeID = 0;//es
            SourceFileID = 0;//yes
            SourceColumn = string.Empty;//yes
            PreviewData = string.Empty;//yes
            MappedColumn = string.Empty;//yes
            MapperControlType = string.Empty;
            ProfileColumnList = new List<CustomDropDownList>();
            //public List<SelectListItem> IncomingFileColumnList { get; set; }
            IsEdit = false;
            DemoUpdateID = 0;
            DemoUpdateTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
            MappingColumns = new List<FrameworkUAD.Object.FileMappingColumn>();
            HasTransformations = false;
            HasMultiMap = false;
        }
        public ColumnMap(int sourceFileId, List<FrameworkUAD.Object.FileMappingColumn> mappingColumns, bool isNewFile, string sourcecolumn, string dataPreview, List<FrameworkUAD_Lookup.Entity.Code> demoUpdateTypes, int fieldMappingID, int fieldMapTypeID, int demoUpdateID, string mappedcolumn = "", bool hasTransformations = false, bool hasMultiMap = false)
        {
            this.FieldMapID = fieldMappingID;
            this.FieldMapTypeID = fieldMapTypeID;
            this.DemoUpdateTypes = demoUpdateTypes;
            this.DemoUpdateID = demoUpdateID;
            this.MappingColumns = mappingColumns;
            this.HasTransformations = hasTransformations;
            this.HasMultiMap = hasMultiMap;

            this.SourceColumn = sourcecolumn;
            this.SourceFileID = sourceFileId;
            this.PreviewData = dataPreview;
            if (isNewFile)
                this.MapperControlType = "New";
            else
                this.MapperControlType = "Edit";

            ProfileColumnList = new List<CustomDropDownList>();
            ProfileColumnList.Add(new CustomDropDownList() { Text = "Ignore", Value = "Ignore" });
            ProfileColumnList.Add(new CustomDropDownList() { Text = "KMTransform", Value = "kmTransform" });

            mappingColumns = mappingColumns.OrderBy(x => x.ColumnName).ToList();

            foreach (FrameworkUAD.Object.FileMappingColumn md in mappingColumns)
            {
                ProfileColumnList.Add(new CustomDropDownList() { Text = md.ColumnName, Value = md.DataTable + "." + md.ColumnName, Group = md.DataTable });
            }

            FrameworkUAD.Object.FileMappingColumn foundColumn = new FrameworkUAD.Object.FileMappingColumn();

            if (MapperControlType == "New")
            {
                foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals(sourcecolumn, StringComparison.CurrentCultureIgnoreCase));
                if (foundColumn == null)
                {
                    #region Extra Matching
                    switch (sourcecolumn.ToLower())
                    {
                        case "account number":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("AccountNumber", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "acctnum":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("AccountNumber", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "acct num":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("AccountNumber", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "acctnbr":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("AccountNumber", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "acct nbr":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("AccountNumber", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "firstname":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("FirstName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "first name":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("FirstName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "1st name":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("FirstName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "first":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("FirstName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "fname":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("FirstName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "lastname":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("LastName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "last name":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("LastName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "last":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("LastName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "lname":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("LastName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "zipcode":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "zip code":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "zip":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "postal":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "postal code":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "postalcode":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "emailaddress":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Email", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "email address":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Email", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "address":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "address1":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "address 1":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "mailing address":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "street":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "streetaddress":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "street address":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "mailstop":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address2", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "mail stop":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address2", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "address2":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address2", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "address 2":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address2", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "phone number":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Phone", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "phonenumber":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Phone", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "voice":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Phone", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "cell":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Mobile", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "fax number":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Fax", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "state":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("RegionCode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "country":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Country", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "foreign country name":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Country", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "foreigncountryname":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Country", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "foreigncountry":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Country", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "foreign country":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Country", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "verified":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Verify", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "categoryid":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubCategoryID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "category id":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubCategoryID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "cat":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubCategoryID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "category code":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubCategoryID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "categorycode":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubCategoryID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "transactionid":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubTransactionID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "transaction id":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubTransactionID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "xact":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubTransactionID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "transaction code":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubTransactionID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "transactioncode":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubTransactionID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "sequence":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("SequenceID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "qsource":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubQSourceID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "q source":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubQSourceID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "qsourceid":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubQSourceID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "par3c":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Par3cID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "qdate":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("QualificationDate", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "q date":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("QualificationDate", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "verification date":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("QualificationDate", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "verificationdate":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("QualificationDate", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo 7":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Demo7", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "media":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Demo7", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "subscriptiontype":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Demo7", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "subscription type":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Demo7", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "original subsrc":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("OrigsSrc", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "originalsubsrc":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("OrigsSrc", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "subsrc":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("SUBSCRIBERSOURCECODE", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo31":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("MailPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "mail permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("MailPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo32":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("FaxPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "fax permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("FaxPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo33":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PhonePermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "phone permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PhonePermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo34":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("OtherProductsPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "other products permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("OtherProductsPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo35":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("ThirdPartyPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "third party permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("ThirdPartyPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "3rd party permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("ThirdPartyPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo36":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("EmailRenewPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "email renew permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("EmailRenewPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "text permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("TextPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        default:
                            foundColumn = null;
                            break;
                    }
                    #endregion
                }

                if (foundColumn != null && !String.IsNullOrEmpty(foundColumn.ColumnName))
                {
                    CustomDropDownList item = ProfileColumnList.FirstOrDefault(x => x.Value.Equals(foundColumn.DataTable + "." + foundColumn.ColumnName, StringComparison.CurrentCultureIgnoreCase));
                    if (item != null)
                        item.Selected = true;

                    MappedColumn = foundColumn.DataTable + "." + foundColumn.ColumnName;
                }
                else
                {
                    CustomDropDownList item = ProfileColumnList.FirstOrDefault(x => x.Value.Equals("Ignore", StringComparison.CurrentCultureIgnoreCase));
                    if (item != null)
                        item.Selected = true;

                    MappedColumn = "Ignore";
                }
            }
            else
            {
                foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals(mappedcolumn, StringComparison.CurrentCultureIgnoreCase));

                if (foundColumn != null && !String.IsNullOrEmpty(foundColumn.ColumnName))
                {
                    CustomDropDownList item = ProfileColumnList.FirstOrDefault(x => x.Value.Equals(foundColumn.DataTable + "." + foundColumn.ColumnName, StringComparison.CurrentCultureIgnoreCase));
                    if (item != null)
                        item.Selected = true;

                    MappedColumn = foundColumn.DataTable + "." + foundColumn.ColumnName;
                }
                else
                {
                    CustomDropDownList item = ProfileColumnList.FirstOrDefault(x => x.Value.Equals("Ignore", StringComparison.CurrentCultureIgnoreCase));
                    if (item != null)
                        item.Selected = true;

                    MappedColumn = "Ignore";
                }
            }
        }
    }

    public class AddNewColumn
    {
        #region Properties
        public int FieldMapID { get; set; }//yes
        public int FieldMapTypeID { get; set; }//yes
        public int SourceFileID { get; set; }//yes
        public string SourceColumn { get; set; }//yes
        public string PreviewData { get; set; }//yes
        public string MappedColumn { get; set; }//yes
        private string MapperControlType;//added
        public List<CustomDropDownList> ProfileColumnList { get; set; }//yes
        //public List<SelectListItem> IncomingFileColumnList { get; set; }
        public bool IsEdit { get; set; }
        public List<FrameworkUAD_Lookup.Entity.Code> DemoUpdateTypes { get; set; }
        public int DemoUpdateID { get; set; }
        public List<FrameworkUAD.Object.FileMappingColumn> MappingColumns { get; set; }
        public bool HasTransformations { get; set; }
        public bool HasMultiMap { get; set; }
        public int ColumnOrder { get; set; }
        #endregion

        public AddNewColumn()
        {
            FieldMapID = 0;
            FieldMapTypeID = 0;//es
            SourceFileID = 0;//yes
            SourceColumn = string.Empty;//yes
            PreviewData = string.Empty;//yes
            MappedColumn = string.Empty;//yes
            MapperControlType = string.Empty;
            ProfileColumnList = new List<CustomDropDownList>();
            //public List<SelectListItem> IncomingFileColumnList { get; set; }
            IsEdit = false;
            DemoUpdateID = 0;
            DemoUpdateTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
            MappingColumns = new List<FrameworkUAD.Object.FileMappingColumn>();
            HasTransformations = false;
            HasMultiMap = false;
            ColumnOrder = 0;
        }
        public AddNewColumn(int sourceFileId, List<FrameworkUAD.Object.FileMappingColumn> mappingColumns, bool isNewFile, string sourcecolumn, string dataPreview, List<FrameworkUAD_Lookup.Entity.Code> demoUpdateTypes, int fieldMappingID, int fieldMapTypeID, int demoUpdateID, int order, string mappedcolumn = "", bool hasTransformations = false, bool hasMultiMap = false)
        {
            this.FieldMapID = fieldMappingID;
            this.FieldMapTypeID = fieldMapTypeID;
            this.DemoUpdateTypes = demoUpdateTypes;
            this.DemoUpdateID = demoUpdateID;
            this.MappingColumns = mappingColumns;
            this.HasTransformations = hasTransformations;
            this.HasMultiMap = hasMultiMap;
            this.ColumnOrder = order;

            this.SourceColumn = sourcecolumn;
            this.SourceFileID = sourceFileId;
            this.PreviewData = dataPreview;
            if (isNewFile)
                this.MapperControlType = "New";
            else
                this.MapperControlType = "Edit";

            ProfileColumnList = new List<CustomDropDownList>();
            ProfileColumnList.Add(new CustomDropDownList() { Text = "Ignore", Value = "Ignore" });
            ProfileColumnList.Add(new CustomDropDownList() { Text = "KMTransform", Value = "kmTransform" });

            mappingColumns = mappingColumns.OrderBy(x => x.ColumnName).ToList();

            foreach (FrameworkUAD.Object.FileMappingColumn md in mappingColumns)
            {
                ProfileColumnList.Add(new CustomDropDownList() { Text = md.ColumnName, Value = md.DataTable + "." + md.ColumnName, Group = md.DataTable });
            }

            FrameworkUAD.Object.FileMappingColumn foundColumn = new FrameworkUAD.Object.FileMappingColumn();

            if (MapperControlType == "New")
            {
                foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals(sourcecolumn, StringComparison.CurrentCultureIgnoreCase));
                if (foundColumn == null)
                {
                    #region Extra Matching
                    switch (sourcecolumn.ToLower())
                    {
                        case "account number":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("AccountNumber", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "acctnum":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("AccountNumber", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "acct num":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("AccountNumber", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "acctnbr":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("AccountNumber", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "acct nbr":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("AccountNumber", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "firstname":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("FirstName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "first name":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("FirstName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "1st name":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("FirstName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "first":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("FirstName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "fname":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("FirstName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "lastname":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("LastName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "last name":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("LastName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "last":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("LastName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "lname":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("LastName", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "zipcode":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "zip code":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "zip":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "postal":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "postal code":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "postalcode":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Zipcode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "emailaddress":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Email", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "email address":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Email", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "address":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "address1":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "address 1":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "mailing address":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "street":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "streetaddress":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "street address":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address1", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "mailstop":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address2", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "mail stop":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address2", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "address2":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address2", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "address 2":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Address2", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "phone number":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Phone", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "phonenumber":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Phone", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "voice":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Phone", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "cell":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Mobile", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "fax number":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Fax", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "state":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("RegionCode", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "country":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Country", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "foreign country name":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Country", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "foreigncountryname":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Country", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "foreigncountry":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Country", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "foreign country":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Country", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "verified":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Verify", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "categoryid":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubCategoryID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "category id":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubCategoryID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "cat":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubCategoryID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "category code":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubCategoryID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "categorycode":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubCategoryID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "transactionid":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubTransactionID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "transaction id":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubTransactionID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "xact":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubTransactionID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "transaction code":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubTransactionID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "transactioncode":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubTransactionID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "sequence":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("SequenceID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "qsource":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubQSourceID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "q source":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubQSourceID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "qsourceid":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PubQSourceID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "par3c":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Par3cID", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "qdate":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("QualificationDate", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "q date":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("QualificationDate", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "verification date":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("QualificationDate", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "verificationdate":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("QualificationDate", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo 7":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Demo7", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "media":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Demo7", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "subscriptiontype":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Demo7", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "subscription type":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("Demo7", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "original subsrc":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("OrigsSrc", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "originalsubsrc":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("OrigsSrc", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "subsrc":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("SUBSCRIBERSOURCECODE", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo31":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("MailPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "mail permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("MailPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo32":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("FaxPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "fax permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("FaxPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo33":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PhonePermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "phone permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("PhonePermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo34":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("OtherProductsPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "other products permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("OtherProductsPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo35":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("ThirdPartyPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "third party permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("ThirdPartyPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "3rd party permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("ThirdPartyPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "demo36":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("EmailRenewPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "email renew permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("EmailRenewPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        case "text permission":
                            foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals("TextPermission", StringComparison.CurrentCultureIgnoreCase));
                            break;
                        default:
                            foundColumn = null;
                            break;
                    }
                    #endregion
                }

                if (foundColumn != null && !String.IsNullOrEmpty(foundColumn.ColumnName))
                {
                    CustomDropDownList item = ProfileColumnList.FirstOrDefault(x => x.Value.Equals(foundColumn.DataTable + "." + foundColumn.ColumnName, StringComparison.CurrentCultureIgnoreCase));
                    if (item != null)
                        item.Selected = true;

                    MappedColumn = foundColumn.DataTable + "." + foundColumn.ColumnName;
                }
                else
                {
                    CustomDropDownList item = ProfileColumnList.FirstOrDefault(x => x.Value.Equals("Ignore", StringComparison.CurrentCultureIgnoreCase));
                    if (item != null)
                        item.Selected = true;

                    MappedColumn = "Ignore";
                }
            }
            else
            {
                foundColumn = mappingColumns.FirstOrDefault(x => x.ColumnName.Equals(mappedcolumn, StringComparison.CurrentCultureIgnoreCase));

                if (foundColumn != null && !String.IsNullOrEmpty(foundColumn.ColumnName))
                {
                    CustomDropDownList item = ProfileColumnList.FirstOrDefault(x => x.Value.Equals(foundColumn.DataTable + "." + foundColumn.ColumnName, StringComparison.CurrentCultureIgnoreCase));
                    if (item != null)
                        item.Selected = true;

                    MappedColumn = foundColumn.DataTable + "." + foundColumn.ColumnName;
                }
                else
                {
                    CustomDropDownList item = ProfileColumnList.FirstOrDefault(x => x.Value.Equals("Ignore", StringComparison.CurrentCultureIgnoreCase));
                    if (item != null)
                        item.Selected = true;

                    MappedColumn = "Ignore";
                }
            }
        }
    }

    public class CustomDropDownList
    {
        #region Properties
        public string Text { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
        public bool Selected { get; set; }
        #endregion

        public CustomDropDownList()
        {
            Text = "";
            Value = "";
            Group = "";
            Selected = false;
        }

        public CustomDropDownList(string text, string value, string group)
        {
            Text = text;
            Value = value;
            Group = group;
            Selected = false;
        }
    }

    public class AdditionalColumnMapModel
    {
        #region Properties
        public List<AdditionalColumnMap> AdditionalColumns { get; set; }
        private List<FrameworkUAD.Object.FileMappingColumn> MappingColumns { get; set; }
        #endregion

        public AdditionalColumnMapModel()
        {
            AdditionalColumns = new List<AdditionalColumnMap>();
        }

        public AdditionalColumnMapModel(bool isNewFile, int sourceFileId, KMPlatform.Entity.Client client)
        {
            if (isNewFile)
            {
                AdditionalColumns = new List<AdditionalColumnMap>();
            }
            else
            {
                AdditionalColumns = new List<AdditionalColumnMap>();

                FrameworkUAS.BusinessLogic.FieldMapping fieldMappingWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
                List<FrameworkUAS.Entity.FieldMapping> loopList = new List<FrameworkUAS.Entity.FieldMapping>();
                List<FrameworkUAS.Entity.FieldMapping> fieldMappings = new List<FrameworkUAS.Entity.FieldMapping>();
                fieldMappings = fieldMappingWorker.Select(sourceFileId, true);

                FrameworkUAD.BusinessLogic.FileMappingColumn fmcWrk = new FrameworkUAD.BusinessLogic.FileMappingColumn();
                List<FrameworkUAD.Object.FileMappingColumn> uadColumns = fmcWrk.Select(client.ClientConnections);
                MappingColumns = uadColumns;

                FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
                List<FrameworkUAD_Lookup.Entity.Code> DemoUpdateTypes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);

                loopList = fieldMappings.Where(x => x.IsNonFileColumn == true).OrderBy(x => x.ColumnOrder).ToList();
                int index = 0;
                foreach (FrameworkUAS.Entity.FieldMapping fm in loopList)
                {
                    AdditionalColumnMap cm = new FileMapperWizard.AdditionalColumnMap(fm.SourceFileID, fm.FieldMappingID, fm.FieldMappingTypeID, fm.IncomingField, fm.MAFField, MappingColumns, fm.DemographicUpdateCodeId, DemoUpdateTypes, fm.ColumnOrder, index);
                    this.AdditionalColumns.Add(cm);
                    index++;
                }
            }
        }
    }
    public class AdditionalColumnMap
    {
        #region Properties
        public int ACMSourceFileID { get; set; }
        public int ACMFieldMapID { get; set; }
        public int ACMFieldMapTypeID { get; set; }
        public bool ACMIsNonFileColumn { get; set; }
        public string ACMSourceColumn { get; set; }
        public SelectListItem selectedMAFField { get; set; }
        public SelectListItem selectedDemoUpdate { get; set; }
        public List<SelectListItem> ACMDemoUpdates { get; set; }
        public List<SelectListItem> ACMMappingColumns { get; set; }
        public int ACMColumnOrder { get; set; }
        public int ACMIndex { get; set; }
        #endregion

        public AdditionalColumnMap()
        {
            ACMSourceFileID = 0;
            ACMFieldMapID = 0;
            ACMFieldMapTypeID = 0;
            ACMIsNonFileColumn = true;
            ACMSourceColumn = string.Empty;
            selectedMAFField = new SelectListItem() { Text = "", Value = "" };
            selectedDemoUpdate = new SelectListItem() { Text = "", Value = "" };
            ACMColumnOrder = 0;
            ACMIndex = 0;

            ACMMappingColumns = new List<SelectListItem>();
            ACMDemoUpdates = new List<SelectListItem>();
        }

        public AdditionalColumnMap(int sourceFileID, int fieldMapID, int fieldMapTypeID, string sourceColumn, string mappedColumn, List<FrameworkUAD.Object.FileMappingColumn> mappingColumns, int demoUpdateID, List<FrameworkUAD_Lookup.Entity.Code> demoUpdateTypes, int columnOrder, int index)
        {
            ACMSourceFileID = sourceFileID;
            ACMFieldMapID = fieldMapID;
            ACMFieldMapTypeID = fieldMapTypeID;
            ACMIsNonFileColumn = true;
            ACMSourceColumn = sourceColumn;
            selectedMAFField = new SelectListItem() { Text = mappedColumn, Value = mappedColumn };
            FrameworkUAD_Lookup.Entity.Code demo = demoUpdateTypes.FirstOrDefault(x => x.CodeId == demoUpdateID);
            selectedDemoUpdate = new SelectListItem() { Text = demo.DisplayName, Value = demoUpdateID.ToString() };
            ACMColumnOrder = columnOrder;
            ACMIndex = index;

            ACMMappingColumns = new List<SelectListItem>();
            ACMMappingColumns.Add(new SelectListItem() { Text = "Ignore", Value = "Ignore" });
            ACMMappingColumns.Add(new SelectListItem() { Text = "Delete", Value = "Delete" });

            foreach (FrameworkUAD.Object.FileMappingColumn md in mappingColumns)
            {
                if (md.ColumnName.Equals(mappedColumn, StringComparison.CurrentCultureIgnoreCase))
                    ACMMappingColumns.Add(new SelectListItem() { Text = md.ColumnName, Value = md.DataTable + "." + md.ColumnName, Selected = true });
                else
                    ACMMappingColumns.Add(new SelectListItem() { Text = md.ColumnName, Value = md.DataTable + "." + md.ColumnName });

            }

            ACMDemoUpdates = new List<SelectListItem>();
            foreach (FrameworkUAD_Lookup.Entity.Code c in demoUpdateTypes)
            {
                if (c.CodeId.Equals(demoUpdateID))
                    ACMDemoUpdates.Add(new SelectListItem() { Text = c.DisplayName, Value = c.CodeId.ToString(), Selected = true });
                else
                    ACMDemoUpdates.Add(new SelectListItem() { Text = c.DisplayName, Value = c.CodeId.ToString() });
            }
        }
    }

    public class UserAdditionalColumn
    {
        public UserAdditionalColumn() { }
        public UserAdditionalColumn(int fieldMappingID, int sourceFileID, string incomingField, string mafField, int columnOrder, int demographicUpdateCodeId, int fieldMapTypeId)
        {
            FieldMappingID = fieldMappingID;
            SourceFileID = sourceFileID;
            IncomingField = incomingField;
            MAFField = mafField;
            ColumnOrder = columnOrder;
            DemographicUpdateCodeId = demographicUpdateCodeId;
            FieldMapTypeId = fieldMapTypeId;
        }
        #region Properties
        [DataMember]
        public int FieldMappingID { get; set; }//yes
        [DataMember]
        public int SourceFileID { get; set; }//yes
        [DataMember]
        public string IncomingField { get; set; }//yes
        [DataMember]
        public string MAFField { get; set; }//yes
        [DataMember]
        public int ColumnOrder { get; set; }//yes
        [DataMember]
        public int DemographicUpdateCodeId { get; set; }//yes
        [DataMember]
        public int FieldMapTypeId { get; set; }
        #endregion        
    }
    public class UserMultiMapColumn
    {
        public UserMultiMapColumn() { }
        public UserMultiMapColumn(int fieldMultiMapID, int fieldMappingID, int fieldMappingTypeID, int columnOrder, string mafField)
        {
            FieldMultiMapID = fieldMultiMapID;
            FieldMappingID = fieldMappingID;
            FieldMappingTypeID = fieldMappingTypeID;
            ColumnOrder = columnOrder;
            MAFField = mafField;
        }
        #region Properties
        [DataMember]
        public int FieldMultiMapID { get; set; }//yes
        [DataMember]
        public int FieldMappingID { get; set; }//yes
        [DataMember]
        public int FieldMappingTypeID { get; set; }//yes
        [DataMember]
        public int ColumnOrder { get; set; }//yes
        [DataMember]
        public string MAFField { get; set; }//yes
        #endregion        
    }

    public class MultiMapModel
    {
        #region Properties
        public int FieldMappingID { get; set; }
        public List<MultiColumnMap> MultiMapColumns { get; set; }
        #endregion

        public MultiMapModel()
        {
            FieldMappingID = 0;
            MultiMapColumns = new List<MultiColumnMap>();
        }

        public MultiMapModel(int fieldMappingId)
        {
            FieldMappingID = fieldMappingId;
            MultiMapColumns = new List<MultiColumnMap>();

            FrameworkUAS.BusinessLogic.FieldMultiMap fmmWorker = new FrameworkUAS.BusinessLogic.FieldMultiMap();
            List<FrameworkUAS.Entity.FieldMultiMap> fieldMultiMaps = new List<FrameworkUAS.Entity.FieldMultiMap>();
            fieldMultiMaps = fmmWorker.SelectFieldMappingID(fieldMappingId);

            int index = 0;
            foreach (FrameworkUAS.Entity.FieldMultiMap fmm in fieldMultiMaps)
            {
                MultiColumnMap mcm = new FileMapperWizard.MultiColumnMap(fmm.FieldMultiMapID, fmm.FieldMappingID, fmm.FieldMappingTypeID, fmm.MAFField, fmm.ColumnOrder, index);
                this.MultiMapColumns.Add(mcm);
                index++;
            }
        }
    }
    public class MultiColumnMap
    {
        #region Properties
        public int FieldMultiMapID { get; set; }
        public int FieldMappingID { get; set; }
        public int FieldMappingTypeID { get; set; }
        public string MAFField { get; set; }
        public int ColumnOrder { get; set; }
        public int MCMIndex { get; set; }
        public SelectListItem selectedMAFField { get; set; }
        public List<SelectListItem> MappingColumns { get; set; }
        #endregion

        public MultiColumnMap()
        {
            FieldMultiMapID = 0;
            FieldMappingID = 0;
            FieldMappingTypeID = 0;
            MAFField = string.Empty;
            selectedMAFField = new SelectListItem() { Text = "", Value = "" };
            ColumnOrder = 0;
            MCMIndex = 0;
        }

        public MultiColumnMap(int fieldMultiMapID, int fieldMappingID, int fieldMappingTypeID, string mappedColumn, int columnOrder, int index)
        {
            FieldMultiMapID = fieldMultiMapID;
            FieldMappingID = fieldMappingID;
            FieldMappingTypeID = fieldMappingTypeID;
            MAFField = mappedColumn;
            selectedMAFField = new SelectListItem() { Text = mappedColumn, Value = mappedColumn };
            ColumnOrder = columnOrder;
            MCMIndex = index;
        }
    }
    [Serializable]
    public class UserColumnValidation
    {
        public UserColumnValidation() { }
        public UserColumnValidation(string type, string sourceColumn, string mappedColumn, int demoUpdateID)
        {
            Type = type;
            SourceColumn = sourceColumn;
            MappedColumn = mappedColumn;
            DemoUpdateID = demoUpdateID;
        }
        #region Properties
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string SourceColumn { get; set; }
        [DataMember]
        public string MappedColumn { get; set; }
        [DataMember]
        public int DemoUpdateID { get; set; }
        #endregion
    }
    [Serializable]
    public class UserMappedColumn
    {
        public UserMappedColumn() { }
        public UserMappedColumn(int fieldMapId, int fieldMapTypeId, int sourceFileId, string sourceColumn, string mappedColumn, int demoUpdateID, string previewData)
        {
            FieldMapId = fieldMapId;
            FieldMapTypeId = FieldMapTypeId;
            SourceFileId = sourceFileId;
            SourceColumn = sourceColumn;
            MappedColumn = mappedColumn;
            DemoUpdateID = demoUpdateID;
            PreviewData = previewData;
        }
        #region Properties
        [DataMember]
        public int FieldMapId { get; set; }//yes
        [DataMember]
        public int FieldMapTypeId { get; set; }//yes
        [DataMember]
        public int SourceFileId { get; set; }//yes
        [DataMember]
        public string SourceColumn { get; set; }//yes
        [DataMember]
        public string MappedColumn { get; set; }//yes
        [DataMember]
        public int DemoUpdateID { get; set; }//yes
        [DataMember]
        public string PreviewData { get; set; }
        #endregion
    }
    //public class AddColumnsViewModel
    //{
    //    public AddColumnsViewModel()
    //    {
    //        AddedColumnsUsed = new List<string>();
    //        FirstLoad = true;
    //        DemoUpdateOptions = new List<FrameworkUAD_Lookup.Entity.Code>();
    //        AdhocBitFields = new List<string>();
    //        IsRequiredFields = new List<string>();
    //    }
    //    #region Properties
    //    public int SourceFileId { get; set; }

    //    List<string> AddedColumnsUsed { get; set; }
    //    bool FirstLoad { get; set; }
    //    public List<FrameworkUAD_Lookup.Entity.Code> DemoUpdateOptions { get; set; }
    //    public List<string> AdhocBitFields { get; set; }
    //    public List<string> IsRequiredFields { get; set; }
    //    #endregion
    //    #region PAGE ENUMS
    //    public enum StepThreeActions
    //    {
    //        Add_Column_Not_Defined_In_File,
    //        Add_Additional_Column_Mapping
    //    }
    //    #endregion
    //    private void LoadData(KMPlatform.Entity.Client myClient)
    //    {
    //        AddedColumnsUsed = new List<string>();
    //        FirstLoad = true;
    //        DemoUpdateOptions = new List<FrameworkUAD_Lookup.Entity.Code>();
    //        AdhocBitFields = new List<string>();
    //        IsRequiredFields = new List<string>();

    //        //POPULATE STEP 3 ACTION LIST
    //        //rcbStep3Actions.Items.Clear();
    //        //    foreach (StepThreeActions action in (StepThreeActions[]) Enum.GetValues(typeof(StepThreeActions)))
    //        //    {
    //        //        rcbStep3Actions.Items.Add(action.ToString().Replace('_', ' '));
    //        //    }

    //        FrameworkUAD_Lookup.BusinessLogic.Code duData = new FrameworkUAD_Lookup.BusinessLogic.Code();
    //        List<FrameworkUAD_Lookup.Entity.Code> demoUpdateTypes = duData.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);
    //        if (demoUpdateTypes != null)
    //            DemoUpdateOptions = demoUpdateTypes;
    //        else
    //            DemoUpdateOptions = new List<FrameworkUAD_Lookup.Entity.Code>();

    //        FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper semWorker = new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper();

    //        List<FrameworkUAD.Entity.SubscriptionsExtensionMapper> semList = new List<FrameworkUAD.Entity.SubscriptionsExtensionMapper>();
    //        semList = semWorker.SelectAll(myClient.ClientConnections);

    //        FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper pseWorker = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper();
    //        List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> psemList = new List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper>();
    //        psemList = pseWorker.SelectAll(myClient.ClientConnections);

    //        AdhocBitFields.AddRange(semList.Where(x => x.CustomFieldDataType == "bit").Select(x => x.CustomField).ToList());
    //        AdhocBitFields.AddRange(psemList.Where(x => x.CustomFieldDataType == "bit").Select(x => x.CustomField).ToList());

    //        FrameworkUAD.BusinessLogic.ResponseGroup rgWorker = new FrameworkUAD.BusinessLogic.ResponseGroup();
    //        List<FrameworkUAD.Entity.ResponseGroup> rgList = new List<FrameworkUAD.Entity.ResponseGroup>();
    //        rgList = rgWorker.Select(myClient.ClientConnections);
    //        //int pubID = 0;
    //        //if (thisContainer.selectedProduct != null)
    //        //    int.TryParse(thisContainer.selectedProduct.PubID.ToString(), out pubID);

    //        //IsRequiredFields = rgList.Where(x => x.IsRequired == true && x.PubID == pubID).Select(x => x.ResponseGroupName).ToList();
    //        IsRequiredFields = rgList.Where(x => x.IsRequired == true).Select(x => x.ResponseGroupName).ToList();
    //    }
    //}
    public class StandardTransformationDataModel
    {
        #region Properties
        public int SourceFileId { get; set; }
        public int FieldMappingId { get; set; }
        public string FieldMappingName { get; set; }
        #endregion

        public StandardTransformationDataModel()
        {
            SourceFileId = 0;
            FieldMappingId = 0;
            FieldMappingName = "";
        }

        public StandardTransformationDataModel(int sourceFileId, int fieldMappingId, string fieldMappingName)
        {
            SourceFileId = sourceFileId;
            FieldMappingId = fieldMappingId;
            FieldMappingName = fieldMappingName;
        }
    }
    public class UserCreatedTransformations
    {
        #region Properties
        public List<int> transformationIds { get; set; }
        #endregion        

        public UserCreatedTransformations()
        {
            transformationIds = new List<int>();
        }

        public UserCreatedTransformations(int transformationID)
        {
            transformationIds.Add(transformationID);
        }
    }
    public class TransformationsViewModel
    {
        #region Properties
        public int SourceFileId { get; set; }
        public int FieldMappingId { get; set; }
        public List<TransformationMap> transformations { get; set; }
        #endregion

        public TransformationsViewModel() { }

        public TransformationsViewModel(int transformFieldMapID, int transformID, int sourceFileID, int fieldMapID, string transformName)
        {
            TransformationMap tm = new TransformationMap();
            tm.TransformationFieldMapId = transformFieldMapID;
            tm.TransformationId = transformID;
            tm.SourceFileId = sourceFileID;
            tm.FieldMappingId = fieldMapID;
            tm.TransformationName = transformName;
            transformations.Add(tm);
        }
    }
    public class TransformationMap
    {
        public TransformationMap() { }

        #region Properties
        public int TransformationFieldMapId { get; set; }
        public int TransformationId { get; set; }
        public int SourceFileId { get; set; }
        public int FieldMappingId { get; set; }
        public string TransformationName { get; set; }
        #endregion
    }
    public class TransformationSearchFilteredViewModel
    {
        public string TransformationTypeId { get; set; }
        public TransformationSearchFilteredViewModel() { }
    }
    public class TransformationSearchViewModel
    {
        public List<TransformationModel> transformations { get; set; }
        public TransformationSearchViewModel() { }
    }
    public class TransformationModel
    {
        #region Properties
        public int TotalRecordCounts { get; set; }
        public int TransformationId { get; set; }
        [Display(Name = "Transformation Name:")]
        public string TransformationName { get; set; }
        [Display(Name = "Transformation Description:")]
        public string TransformationDescription { get; set; }
        [Display(Name = "Select Transformation:")]
        public int TransformationTypeId { get; set; }
        public string TransformationType { get; set; }
        public bool MapsPubCode { get; set; }
        public bool LastStepDataMap { get; set; }
        [Display(Name = "Save as Transformation Template")]
        public bool IsTemplate { get; set; }
        public bool IsEdit { get; set; }
        public string ColumnName { get; set; }
        #endregion

        public TransformationModel(string columnName)
        {
            TransformationId = 0;
            TransformationName = "";
            TransformationDescription = "";
            TransformationTypeId = 0;
            TransformationType = "";
            MapsPubCode = false;
            LastStepDataMap = false;
            IsTemplate = true;
            IsEdit = false;
            ColumnName = columnName;
        }

        public TransformationModel(int totalRecords, int id, string name, string desc, string type, string columnName)
        {
            TotalRecordCounts = totalRecords;
            TransformationId = id;
            TransformationName = name;
            TransformationDescription = desc;
            TransformationType = type;
            ColumnName = columnName;
        }
    }

    public class TransformationAssignModel
    {
        #region Properties  
        public List<SelectListItem> products { get; set; }
        public MultiSelectList selectedProducts { get; set; }
        public List<TransformAssignModel> transformAssignMaps { get; set; }
        public bool isEnabled { get; set; }
        #endregion

        public TransformationAssignModel(List<TransformAssignModel> dataMaps, List<FrameworkUAD.Entity.Product> productList, bool enabled = false)
        {
            transformAssignMaps = dataMaps;
            isEnabled = enabled;

            products = new List<SelectListItem>();
            products.Add(new SelectListItem() { Text = "ALL PRODUCTS", Value = "0", Selected = true });
            products.Add(new SelectListItem() { Text = "", Value = "-1" });
            foreach (FrameworkUAD.Entity.Product p in productList)
            {
                products.Add(new SelectListItem() { Text = p.PubCode, Value = p.PubID.ToString() });
            }

            selectedProducts = new MultiSelectList(products, "Value", "Text", new List<int>() { 0 });
        }
    }

    public class TransformAssignModel
    {
        #region Properties  
        public int TransformAssignID { get; set; }
        public string GroupedTransformAssignIDs { get; set; }
        public int TransIndex { get; set; }
        public MultiSelectList selectedPubID { get; set; }
        public string selectedValue { get; set; }        
        #endregion
        public TransformAssignModel()
        {
            List<SelectListItem> products = new List<SelectListItem>();

            TransformAssignID = 0;
            GroupedTransformAssignIDs = "0";
            TransIndex = 0;
            selectedPubID = new MultiSelectList(products, "Value", "Text", new List<int>() { 0 });
            selectedValue = "";            
        }
        public TransformAssignModel(int id, string groupedIDs, int index, List<int> pubId, string pubCode, string value, List<FrameworkUAD.Entity.Product> productList)
        {
            List<SelectListItem> products = new List<SelectListItem>();
            List<int> selectProducts = pubId;

            if (selectProducts.Count() == 0 || (selectProducts.Count() == 1 && (selectProducts.Contains(0) || selectProducts.Contains(0))))
            {
                products.Add(new SelectListItem() { Text = "ALL PRODUCTS", Value = "0", Selected = true });
                selectProducts.Add(0);
            }
            else
            {
                foreach (FrameworkUAD.Entity.Product p in productList)
                {
                    if (pubId.Count(x => x == p.PubID) > 0)
                    {
                        products.Add(new SelectListItem() { Text = p.PubCode, Value = p.PubID.ToString(), Selected = true });
                    }
                }
            }

            TransformAssignID = id;
            GroupedTransformAssignIDs = groupedIDs;
            TransIndex = index;
            selectedPubID = new MultiSelectList(products, "Value", "Text", selectProducts);
            selectedValue = value;            
        }
    }

    public class UserTransformAssign
    {
        public UserTransformAssign() { }
        public UserTransformAssign(int transformAssignID, int rowID, string pubID, string value)
        {
            TransformAssignID = transformAssignID;
            RowID = rowID;
            PubID = pubID;
            Value = value;
        }
        #region Properties
        [DataMember]
        public int TransformAssignID { get; set; }
        [DataMember]
        public int RowID { get; set; }
        [DataMember]
        public string PubID { get; set; }
        [DataMember]
        public string Value { get; set; }
        #endregion
    }

    public class TransformationChangeValueModel
    {
        #region Properties  
        public bool MapsPubCode { get; set; }
        public bool IsLastStep { get; set; }
        public List<SelectListItem> products { get; set; }
        public MultiSelectList selectedProducts { get; set; }
        public List<TransformDataMapModel> transformDataMaps { get; set; }
        public bool isEnabled { get; set; }
        #endregion

        public TransformationChangeValueModel(List<TransformDataMapModel> dataMaps, List<FrameworkUAD.Entity.Product> productList, bool enabled = false)
        {
            MapsPubCode = false;
            IsLastStep = false;
            transformDataMaps = dataMaps;
            isEnabled = enabled;

            products = new List<SelectListItem>();
            products.Add(new SelectListItem() { Text = "ALL PRODUCTS", Value = "0", Selected = true });
            products.Add(new SelectListItem() { Text = "", Value = "-1" });
            foreach (FrameworkUAD.Entity.Product p in productList)
            {
                products.Add(new SelectListItem() { Text = p.PubCode, Value = p.PubID.ToString() });
            }

            selectedProducts = new MultiSelectList(products, "Value", "Text", new List<int>() { 0 });
        }
    }

    public class TransformDataMapModel
    {
        #region Properties  
        public int TransformDataMapID { get; set; }
        public string GroupedTransformDataMapIDs { get; set; }
        public int TransIndex { get; set; }
        public MultiSelectList selectedPubID { get; set; }
        public SelectListItem selectedMatchType { get; set; }
        public string selectedSourceData { get; set; }
        public string selectedDesiredData { get; set; }
        #endregion
        public TransformDataMapModel()
        {
            List<SelectListItem> products = new List<SelectListItem>();

            TransformDataMapID = 0;
            GroupedTransformDataMapIDs = "0";
            TransIndex = 0;
            selectedPubID = new MultiSelectList(products, "Value", "Text", new List<int>() { 0 });
            selectedMatchType = new SelectListItem() { Text = "", Value = "" };
            selectedSourceData = "";
            selectedDesiredData = "";
        }
        public TransformDataMapModel(int id, string groupedIDs, int index, List<int> pubId, string pubCode, string match, string source, string desire, List<FrameworkUAD.Entity.Product> productList)
        {
            List<SelectListItem> products = new List<SelectListItem>();
            List<int> selectProducts = pubId;

            if (selectProducts.Count() == 0 || (selectProducts.Count() == 1 && (selectProducts.Contains(0) || selectProducts.Contains(0))))
            {
                products.Add(new SelectListItem() { Text = "ALL PRODUCTS", Value = "0", Selected = true });
                selectProducts.Add(0);
            }
            else
            {                
                foreach (FrameworkUAD.Entity.Product p in productList)
                {
                    if (pubId.Count(x => x == p.PubID) > 0)
                    {
                        products.Add(new SelectListItem() { Text = p.PubCode, Value = p.PubID.ToString(), Selected = true });
                    }
                }
            }

            TransformDataMapID = id;
            GroupedTransformDataMapIDs = groupedIDs;
            TransIndex = index;
            selectedPubID = new MultiSelectList(products, "Value", "Text", selectProducts);
            selectedMatchType = new SelectListItem() { Text = match, Value = match };
            selectedSourceData = source;
            selectedDesiredData = desire;
        }
    }

    public class UserTransformDataMap
    {
        public UserTransformDataMap() { }
        public UserTransformDataMap(int transformDataMapID, int rowID, string pubID, string matchType, string sourceData, string desiredData)
        {
            TransformDataMapID = transformDataMapID;
            RowID = rowID;
            PubID = pubID;
            MatchType = matchType;
            SourceData = sourceData;
            DesiredData = desiredData;
        }
        #region Properties
        [DataMember]
        public int TransformDataMapID { get; set; }
        [DataMember]
        public int RowID { get; set; }
        [DataMember]
        public string PubID { get; set; }
        [DataMember]
        public string MatchType { get; set; }
        [DataMember]
        public string SourceData { get; set; }
        [DataMember]
        public string DesiredData { get; set; }
        #endregion
    }

    public class TransformationJoinColumnModel
    {
        #region Properties 
        public int TransformJoinId { get; set; }
        public string ColumnsToJoin { get; set; }
        public string Delimiter { get; set; }
        public List<SelectListItem> availableColumns { get; set; }
        public List<SelectListItem> joinedColumns { get; set; }
        public List<SelectListItem> products { get; set; }
        public MultiSelectList selectedProducts { get; set; }
        public bool isEnabled { get; set; }
        #endregion

        public TransformationJoinColumnModel(List<string> fileColumns, List<FrameworkUAD.Entity.Product> productList, bool enabled = false)
        {
            TransformJoinId = 0;
            ColumnsToJoin = "";
            Delimiter = "";
            availableColumns = new List<SelectListItem>();
            joinedColumns = new List<SelectListItem>();
            isEnabled = enabled;

            foreach (string s in fileColumns)
            {
                availableColumns.Add(new SelectListItem() { Text = s, Value = s });
            }

            products = new List<SelectListItem>();
            products.Add(new SelectListItem() { Text = "ALL PRODUCTS", Value = "0", Selected = true });
            products.Add(new SelectListItem() { Text = "", Value = "-1" });
            productList.ForEach(c => products.Add(new SelectListItem() { Text = c.PubCode, Value = c.PubID.ToString() }));

            selectedProducts = new MultiSelectList(products, "Value", "Text", new List<int>() { 0 });            
        }

        public TransformationJoinColumnModel(int transformJoinId, string columnsToJoin, string delimiter, List<string> fileColumns, List<FrameworkUAD.Entity.Product> productList, List<int> selectProducts, bool enabled = false)
        {
            TransformJoinId = transformJoinId;
            ColumnsToJoin = columnsToJoin;

            string del = CommonEnums.ColumnDelimiter.comma.ToString();
            if (delimiter.Equals(":", StringComparison.CurrentCultureIgnoreCase))
                del = CommonEnums.ColumnDelimiter.colon.ToString();
            else if (delimiter.Equals(",", StringComparison.CurrentCultureIgnoreCase))
                del = CommonEnums.ColumnDelimiter.comma.ToString();
            else if (delimiter.Equals(";", StringComparison.CurrentCultureIgnoreCase))
                del = CommonEnums.ColumnDelimiter.semicolon.ToString();
            else if (delimiter.Equals("\t", StringComparison.CurrentCultureIgnoreCase))
                del = CommonEnums.ColumnDelimiter.tab.ToString();
            else if (delimiter.Equals("~", StringComparison.CurrentCultureIgnoreCase))
                del = CommonEnums.ColumnDelimiter.tild.ToString();
            else if (delimiter.Equals("|", StringComparison.CurrentCultureIgnoreCase))
                del = CommonEnums.ColumnDelimiter.pipe.ToString();

            Delimiter = del;
            availableColumns = new List<SelectListItem>();
            isEnabled = enabled;
            bool disabled = false;
            if (isEnabled == false)
                disabled = true;

            joinedColumns = new List<SelectListItem>();
            List<string> columns = new List<string>();
            if (delimiter.Equals("\t", StringComparison.CurrentCultureIgnoreCase))
            {
                columns = ColumnsToJoin.Replace(" ", "\t").Split('\t').ToList();
                columns.RemoveAll(x => x.Equals(""));
            }
            else
                columns = ColumnsToJoin.Split(new[] { delimiter }, StringSplitOptions.None).ToList();

            foreach (string s in columns)
            {
                joinedColumns.Add(new SelectListItem() { Text = s, Value = s, Disabled = disabled });
            }

            foreach (string s in fileColumns)
            {
                if (columns.Count(x => x.Equals(s, StringComparison.CurrentCultureIgnoreCase)) > 0)
                    continue;

                availableColumns.Add(new SelectListItem() { Text = s, Value = s, Disabled = disabled });
            }

            products = new List<SelectListItem>();
            if (selectProducts.Count() == 0 || (selectProducts.Count() == 1 && selectProducts.Contains(0)))
            {
                products.Add(new SelectListItem() { Text = "ALL PRODUCTS", Value = "0", Selected = true });
                selectProducts.Add(0);
            }
            else
                products.Add(new SelectListItem() { Text = "ALL PRODUCTS", Value = "0" });

            products.Add(new SelectListItem() { Text = "", Value = "-1" });
            foreach (FrameworkUAD.Entity.Product p in productList)
            {
                bool isSelected = false;
                if (selectProducts.Count(x => x == p.PubID) > 0)
                    isSelected = true;

                products.Add(new SelectListItem() { Text = p.PubCode, Value = p.PubID.ToString(), Selected = isSelected });
            }

            selectedProducts = new MultiSelectList(products, "Value", "Text", selectProducts);
        }
    }

    public class TransformationSplitIntoRowModel
    {
        #region Properties  
        public int TransformSplitId { get; set; }
        public string Delimiter { get; set; }
        public List<SelectListItem> products { get; set; }
        public MultiSelectList selectedProducts { get; set; }
        public bool isEnabled { get; set; }
        #endregion        

        public TransformationSplitIntoRowModel(List<FrameworkUAD.Entity.Product> productList, bool enabled = false)
        {
            TransformSplitId = 0;
            Delimiter = "";
            isEnabled = enabled;

            products = new List<SelectListItem>();
            products.Add(new SelectListItem() { Text = "ALL PRODUCTS", Value = "0", Selected = true });
            products.Add(new SelectListItem() { Text = "", Value = "-1" });
            productList.ForEach(c => products.Add(new SelectListItem() { Text = c.PubCode, Value = c.PubID.ToString() }));

            selectedProducts = new MultiSelectList(products, "Value", "Text", new List<int>() { 0 });
        }

        public TransformationSplitIntoRowModel(int transformSplitId, string delimiter, List<FrameworkUAD.Entity.Product> productList, List<int> selectProducts, bool enabled = false)
        {
            TransformSplitId = transformSplitId;
            Delimiter = delimiter;
            isEnabled = enabled;

            products = new List<SelectListItem>();
            if (selectProducts.Count() == 0 || (selectProducts.Count() == 1 && selectProducts.Contains(0)))
            {
                products.Add(new SelectListItem() { Text = "ALL PRODUCTS", Value = "0", Selected = true });
                selectProducts.Add(0);
            }
            else
                products.Add(new SelectListItem() { Text = "ALL PRODUCTS", Value = "0" });

            products.Add(new SelectListItem() { Text = "", Value = "-1" });
            foreach (FrameworkUAD.Entity.Product p in productList)
            {
                bool isSelected = false;
                if (selectProducts.Count(x => x == p.PubID) > 0)
                    isSelected = true;

                products.Add(new SelectListItem() { Text = p.PubCode, Value = p.PubID.ToString(), Selected = isSelected });
            }

            selectedProducts = new MultiSelectList(products, "Value", "Text", selectProducts);
        }
    }
    #endregion

    #region Custom Rules
    //[Serializable]
    //public class CustomRules
    //{
    //    public CustomRules() { }
    //    #region properties
    //    public int SourceFileId { get; set; }
    //    public bool IsFullFile { get; set; }
    //    public List<FrameworkUAS.Entity.RuleSet> TabFilteredRuleSets { get; set; }
    //    public List<FrameworkUAS.Entity.Rule> TabFilteredRules { get; set; }
    //    public List<FieldMapping> AvailableColumnsToMap

    //    public List<FieldMapping> MappedColumns

    //    #endregion
    //}




    //[Serializable]
    //public class RulesViewModel
    //{
    //    public RulesViewModel()
    //    {
    //        //RuleFields = new List<FrameworkUAS.Entity.RuleField>();
    //        //RuleFieldValues = new List<FrameworkUAS.Entity.RuleFieldPredefinedValue>();

    //        //ExistingRuleSets = new List<FrameworkUAS.Entity.RuleSet>();
    //        //ExistingRules = new List<FrameworkUAS.Entity.Rule>();

    //        sourceFile = new FrameworkUAS.Entity.SourceFile();
    //        SourceFileId = 0;
    //        IsFullFile = false;

    //        postDQMViewModel = new RulesPostDQMViewModel(null, new List<ColumnMap>());
    //        //admsProcessingViewModel = new RulesAdmsProcessingViewModel();

    //        NewRuleSetName = string.Empty;
    //        IsGlobalRuleSet = false;
    //        NewRuleType = string.Empty;
    //        NewRuleName = string.Empty;
    //        NewRuleAction = string.Empty;

    //        SelectedRuleSetId = 0;
    //        SelectedRuleId = 0;

    //        TabFilteredRuleSets = new List<FrameworkUAS.Entity.RuleSet>();
    //        TabFilteredRules = new List<FrameworkUAS.Entity.Rule>();

    //        //holders for new Rules
    //        CustomRuleGridRules = new List<FrameworkUAS.Object.CustomRuleGrid>();
    //    }
    //    #region Properties
    //    //need these system values so condition and update UI can be set based on selected Database Field
    //    //public List<FrameworkUAS.Entity.RuleField> RuleFields { get; set; }
    //    //public List<FrameworkUAS.Entity.RuleFieldPredefinedValue> RuleFieldValues { get; set; }

    //    //use this to copy from existing rule - as RuleSets / Rules added also add to these lists
    //    //public List<FrameworkUAS.Entity.RuleSet> ExistingRuleSets { get; set; }
    //    //public List<FrameworkUAS.Entity.Rule> ExistingRules { get; set; }

    //    //public FrameworkUAS.Entity.SourceFile sourceFile { get; set; }
    //    public int SourceFileId { get; set; }
    //    public bool IsFullFile { get; set; }

    //    public RulesPostDQMViewModel postDQMViewModel { get; set; }
    //    //not implementing at this time - public RulesAdmsProcessingViewModel admsProcessingViewModel { get; set; }

    //    [Required(ErrorMessage = "Rule Set name is required.")]
    //    [Display(Name = "Rule Set Name:")]
    //    public string NewRuleSetName { get; set; }

    //    public bool IsGlobalRuleSet { get; set; }
    //    public string NewRuleType { get; set; }//Insert Update Delete
    //    public string NewRuleName { get; set; }
    //    public string NewRuleAction { get; set; }

    //    public int SelectedRuleSetId { get; set; }
    //    public int SelectedRuleId { get; set; }

    //    //when changing tabs need to grab Rules that have been added from Rules object by correct CustomRuleImportType and display in grid for ordering
    //    //will bind the grid on _orderRules to this object
    //    public List<FrameworkUAS.Entity.RuleSet> TabFilteredRuleSets { get; set; }
    //    public List<FrameworkUAS.Entity.Rule> TabFilteredRules { get; set; }

    //    //this will be bound to grid
    //    //as new rules are created or copied (saved as new) - add to this object
    //    public List<FrameworkUAS.Object.CustomRuleGrid> CustomRuleGridRules { get; set; }

    //    //public List<Common.UASError> ErrorList { get; set; }
    //    #endregion
    //}


    //[Serializable]
    //public class RulesPostDQMViewModel
    //{
    //    public RulesPostDQMViewModel(KMPlatform.Entity.Client client, List<ColumnMap> incomingColumns)
    //    {
    //        Conditions = new List<RuleConditionViewModel>(); //List<FrameworkUAS.Entity.RuleCondition>();
    //        NewRuleType = string.Empty;
    //        SourceFileId = 0;
    //        NewRuleSetId = 0;
    //        NewRuleId = 0;
    //        Conditions = new List<RuleConditionViewModel>();
    //        IncomingColumns = incomingColumns;
    //        RuleActions = new List<SelectListItem>();
    //        if (client != null)
    //            DataBaseColumns = GetDataBaseColumns(client);
    //        else
    //            DataBaseColumns = new List<FrameworkUAD.Object.FileMappingColumn>();
    //        ErrorList = new List<Common.UASError>();
    //        insertUpdateNewModel = new FileMapperWizard.InsertUpdateNewModel();


    //        //Connectors = new List<SelectListItem>();
    //        //Connectors.Add(new SelectListItem() { Text = "And", Value = "And" });
    //        //Connectors.Add(new SelectListItem() { Text = "Or", Value = "Or" });
    //        //Operators = new List<SelectListItem>();
    //        //FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
    //        //DataTable dt = cWrk.dtGetCode(FrameworkUAD_Lookup.Enums.CodeType.Operators);
    //        //foreach (DataRow dr in dt.Rows)
    //        //    Operators.Add(new SelectListItem() { Text = dr["DisplayName"].ToString(), Value = dr["CodeId"].ToString() });

    //        //IncomingFields = new List<SelectListItem>();
    //        //Functions = new List<SelectListItem>();
    //    }
    //    public RulesPostDQMViewModel(KMPlatform.Entity.Client client, List<ColumnMap> incomingColumns, string newRuleType, int sourceFileId, string ruleSetName, string ruleName, int newRuleSetId, int newRuleId)
    //    {
    //        //Conditions = new List<FrameworkUAS.Entity.RuleCondition>();
    //        NewRuleType = newRuleType;
    //        SourceFileId = sourceFileId;
    //        NewRuleId = newRuleId;
    //        NewRuleName = ruleName;
    //        NewRuleSetName = ruleSetName;
    //        NewRuleSetId = newRuleSetId;
    //        RuleActions = new List<SelectListItem>();
    //        ErrorList = new List<Common.UASError>();
    //        insertUpdateNewModel = new FileMapperWizard.InsertUpdateNewModel();

    //        Conditions = new List<RuleConditionViewModel>();
    //        if (newRuleId == 0)
    //        {
    //            //RuleConditionViewModel rcvm = new RuleConditionViewModel();
    //            //foreach (var cm in incomingColumns)
    //            //    rcvm.IncomingFields.Add(new SelectListItem() { Text = cm.SourceColumn, Value = cm.SourceColumn });
    //            //if (!Conditions.Contains(rcvm))
    //            //    Conditions.Add(rcvm);

    //            //foreach (var cm in incomingColumns)
    //            //{
    //            //    RuleConditionViewModel rcvm = new RuleConditionViewModel();
    //            //    rcvm.IncomingFields.Add(new SelectListItem() { Text = cm.SourceColumn, Value = cm.SourceColumn });
    //            //    if (!Conditions.Contains(rcvm))
    //            //        Conditions.Add(rcvm);
    //            //}

    //            RuleConditionViewModel rcvm = new RuleConditionViewModel(incomingColumns);
    //            Conditions.Add(rcvm);
    //        }
    //        else
    //        {
    //            //need to get RuleConditions
    //            FrameworkUAS.BusinessLogic.RuleCondition rcWrk = new FrameworkUAS.BusinessLogic.RuleCondition();
    //            var myRCs = rcWrk.Select(newRuleId);
    //            myRCs.ForEach(x =>
    //            {
    //                RuleConditionViewModel rcvm = new RuleConditionViewModel(incomingColumns, x);
    //                Conditions.Add(rcvm);
    //            });

    //        }
    //        IncomingColumns = incomingColumns;
    //        DataBaseColumns = GetDataBaseColumns(client);


    //        //Connectors = new List<SelectListItem>();
    //        //Connectors.Add(new SelectListItem() { Text = "And", Value = "And" });
    //        //Connectors.Add(new SelectListItem() { Text = "Or", Value = "Or" });
    //        //Operators = new List<SelectListItem>();
    //        //FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
    //        //DataTable dt = cWrk.dtGetCode(FrameworkUAD_Lookup.Enums.CodeType.Operators);
    //        //foreach (DataRow dr in dt.Rows)
    //        //    Operators.Add(new SelectListItem() { Text = dr["DisplayName"].ToString(), Value = dr["CodeId"].ToString() });
    //        //IncomingFields = new List<SelectListItem>();
    //        //Functions = new List<SelectListItem>();
    //    }

    //    #region Properties
    //    public string NewRuleType { get; set; }//from controller method
    //    public string NewRuleAction { get; set; }//from controller method - Insert Update Delete
    //    public int SourceFileId { get; set; }//from controller method

    //    public string NewRuleSetName { get; set; }//from controller method
    //    public int NewRuleSetId { get; set; }//from controller method - 0 is default
    //    public string IsGlobalRuleSet { get; set; }
    //    public int NewRuleId { get; set; }//from controller method - 0 is default
    //    public string NewRuleName { get; set; }//from controller method
    //    public string IsGlobalRule { get; set; }

    //    public List<SelectListItem> RuleActions { get; set; }
    //    public string selectedRuleAction { get; set; }

    //    public InsertUpdateNewModel insertUpdateNewModel { get; set; }

    //    public List<Common.UASError> ErrorList { get; set; }

    //    #region properties for drop down in grid templates - bound via ajax json methods on controller
    //    //these properties are for the condition grid drop down controls
    //    //public List<SelectListItem> Connectors { get; set; }//and or
    //    //public List<SelectListItem> IncomingFields { get; set; }//set from ColumnMappingViewModel.IncomingColumns
    //    //public List<SelectListItem> Operators { get; set; }//predefined CodetypeId - GetOperators
    //    //public List<SelectListItem> Functions { get; set; }//undefined for now

    //    //holder for the new conditions we create
    //    //public List<FrameworkUAS.Entity.RuleCondition> Conditions { get; set; }
    //    public List<RuleConditionViewModel> Conditions { get; set; }
    //    public List<ColumnMap> IncomingColumns { get; set; }
    //    public List<FrameworkUAD.Object.FileMappingColumn> DataBaseColumns { get; set; }
    //    #endregion

    //    #endregion
    //    private List<FrameworkUAD.Object.FileMappingColumn> GetDataBaseColumns(KMPlatform.Entity.Client client)
    //    {
    //        FrameworkUAD.BusinessLogic.FileMappingColumn fmcWrk = new FrameworkUAD.BusinessLogic.FileMappingColumn();
    //        List<FrameworkUAD.Object.FileMappingColumn> uadColumns = fmcWrk.Select(client.ClientConnections);
    //        return uadColumns.ToList();// Where(x => !x.IsDemographic && !x.IsDemographicOther).ToList();//likely need to allow demographics
    //    }
    //    private void LoadData(KMPlatform.Entity.Client myClient)
    //    {

    //    }
    //}
    [Serializable]
    public class RuleConditionViewModel
    {
        public RuleConditionViewModel()
        {
            ruleCondition = new FrameworkUAS.Entity.RuleCondition();
            ruleId = 0;
            lineNumber = 0;
            compareValue = string.Empty;
            isGrouped = false;
            groupNumber = 0;
            ruleFieldId = 0;
            uiControl = "textbox";

            //Connectors = new List<SelectListItem>();
            //Connectors.Add(new SelectListItem() { Text = "", Value = "" });
            //Connectors.Add(new SelectListItem() { Text = "And", Value = "And" });
            //Connectors.Add(new SelectListItem() { Text = "Or", Value = "Or" });
            selectedConnector = new SelectListItem() { Text = "", Value = "" };

            //Operators = new List<SelectListItem>();
            //Operators.Add(new SelectListItem() { Text = "- Select -", Value = "0" });
            selectedOperator = new SelectListItem() { Text = "- Select -", Value = "0" };

            //FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
            //DataTable dt = cWrk.dtGetCode(FrameworkUAD_Lookup.Enums.CodeType.Operators);
            //foreach (DataRow dr in dt.Rows)
            //    Operators.Add(new SelectListItem() { Text = dr["DisplayName"].ToString(), Value = dr["CodeId"].ToString() });

            //IncomingFields = new List<SelectListItem>();
            selectedIncomingField = new SelectListItem() { Text = "", Value = "" };

            //DataBaseFields = new List<SelectListItem>();
            selectedDataBaseField = new SelectListItem() { Text = "", Value = "" };

            //Functions = new List<SelectListItem>();
        }
        public RuleConditionViewModel(List<ColumnMap> incomingColumns)
        {
            ruleCondition = new FrameworkUAS.Entity.RuleCondition();
            ruleId = 0;
            lineNumber = 0;
            compareValue = string.Empty;
            isGrouped = false;
            groupNumber = 0;
            ruleFieldId = 0;
            uiControl = "textbox";

            //Connectors = new List<SelectListItem>();
            //Connectors.Add(new SelectListItem() { Text = "", Value = "" });
            //Connectors.Add(new SelectListItem() { Text = "And", Value = "And" });
            //Connectors.Add(new SelectListItem() { Text = "Or", Value = "Or" });
            selectedConnector = new SelectListItem() { Text = "", Value = "" };

            //Operators = new List<SelectListItem>();
            //Operators.Add(new SelectListItem() { Text = "- Select -", Value = "0" });
            selectedOperator = new SelectListItem() { Text = "- Select -", Value = "0" };

            //FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
            //DataTable dt = cWrk.dtGetCode(FrameworkUAD_Lookup.Enums.CodeType.Operators);
            //foreach (DataRow dr in dt.Rows)
            //    Operators.Add(new SelectListItem() { Text = dr["DisplayName"].ToString(), Value = dr["CodeId"].ToString() });

            //IncomingFields = new List<SelectListItem>();
            //foreach (var cm in incomingColumns)
            //    IncomingFields.Add(new SelectListItem() { Text = cm.SourceColumn, Value = cm.SourceColumn });
            selectedIncomingField = new SelectListItem() { Text = "", Value = "" };

            //DataBaseFields = new List<SelectListItem>();
            //var ic = incomingColumns.First();
            //if (ic != null)
            //{
            //    foreach (var i in ic.ProfileColumnList)
            //        DataBaseFields.Add(new SelectListItem() { Text = i.Text, Value = i.Value });
            //}
            selectedDataBaseField = new SelectListItem() { Text = "", Value = "" };

            //Functions = new List<SelectListItem>();
        }
        public RuleConditionViewModel(List<ColumnMap> incomingColumns, FrameworkUAS.Entity.RuleCondition rc)
        {
            ruleCondition = rc != null ? rc : new FrameworkUAS.Entity.RuleCondition();
            ruleId = rc != null ? rc.RuleId : 0;
            lineNumber = rc != null ? rc.Line : 0;
            compareValue = rc != null ? rc.CompareValue : string.Empty;
            isGrouped = rc != null ? rc.IsGrouped : false;
            groupNumber = rc != null ? rc.GroupNumber : 0;
            ruleFieldId = 0;
            uiControl = "textbox";

            //Connectors = new List<SelectListItem>();
            //Connectors.Add(new SelectListItem() { Text = "", Value = "" });
            //Connectors.Add(new SelectListItem() { Text = "And", Value = "And" });
            //Connectors.Add(new SelectListItem() { Text = "Or", Value = "Or" });
            selectedConnector = new SelectListItem() { Text = "", Value = "" };

            //Operators = new List<SelectListItem>();
            //Operators.Add(new SelectListItem() { Text = "- Select -", Value = "0" });
            selectedOperator = new SelectListItem() { Text = "- Select -", Value = "0" };

            //FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
            //DataTable dt = cWrk.dtGetCode(FrameworkUAD_Lookup.Enums.CodeType.Operators);
            //foreach (DataRow dr in dt.Rows)
            //    Operators.Add(new SelectListItem() { Text = dr["DisplayName"].ToString(), Value = dr["CodeId"].ToString() });

            //IncomingFields = new List<SelectListItem>();
            //foreach (var cm in incomingColumns)
            //    IncomingFields.Add(new SelectListItem() { Text = cm.SourceColumn, Value = cm.SourceColumn });
            selectedIncomingField = new SelectListItem() { Text = "", Value = "" };

            //DataBaseFields = new List<SelectListItem>();
            //var ic = incomingColumns.First();
            //if (ic != null)
            //{
            //    foreach (var i in ic.ProfileColumnList)
            //        DataBaseFields.Add(new SelectListItem() { Text = i.Text, Value = i.Value });
            //}
            selectedDataBaseField = new SelectListItem() { Text = "", Value = "" };

            //Functions = new List<SelectListItem>();
        }
        public RuleConditionViewModel(List<ColumnMap> incomingColumns, int _ruleId, int _lineNumber)
        {
            FrameworkUAS.BusinessLogic.RuleCondition rcWrk = new FrameworkUAS.BusinessLogic.RuleCondition();
            FrameworkUAS.Entity.RuleCondition rc = rcWrk.Select(_ruleId).SingleOrDefault(x => x.Line == _lineNumber);
            ruleCondition = rc != null ? rc : new FrameworkUAS.Entity.RuleCondition();
            ruleId = rc != null ? rc.RuleId : 0;
            lineNumber = rc != null ? rc.Line : 0;
            compareValue = rc != null ? rc.CompareValue : string.Empty;
            isGrouped = rc != null ? rc.IsGrouped : false;
            groupNumber = rc != null ? rc.GroupNumber : 0;
            ruleFieldId = 0;
            uiControl = "textbox";

            //Connectors = new List<SelectListItem>();
            //Connectors.Add(new SelectListItem() { Text = "", Value = "" });
            //Connectors.Add(new SelectListItem() { Text = "And", Value = "And" });
            //Connectors.Add(new SelectListItem() { Text = "Or", Value = "Or" });
            selectedConnector = new SelectListItem() { Text = "", Value = "" };

            //Operators = new List<SelectListItem>();
            //Operators.Add(new SelectListItem() { Text = "- Select -", Value = "0" });
            selectedOperator = new SelectListItem() { Text = "- Select -", Value = "0" };

            //FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
            //DataTable dt = cWrk.dtGetCode(FrameworkUAD_Lookup.Enums.CodeType.Operators);
            //foreach (DataRow dr in dt.Rows)
            //    Operators.Add(new SelectListItem() { Text = dr["DisplayName"].ToString(), Value = dr["CodeId"].ToString() });

            //IncomingFields = new List<SelectListItem>();
            //foreach (var cm in incomingColumns)
            //    IncomingFields.Add(new SelectListItem() { Text = cm.SourceColumn, Value = cm.SourceColumn });
            selectedIncomingField = new SelectListItem() { Text = "", Value = "" };

            //DataBaseFields = new List<SelectListItem>();
            //var ic = incomingColumns.First();
            //if (ic != null)
            //{
            //    foreach (var i in ic.ProfileColumnList)
            //        DataBaseFields.Add(new SelectListItem() { Text = i.Text, Value = i.Value });
            //}
            selectedDataBaseField = new SelectListItem() { Text = "", Value = "" };

            //Functions = new List<SelectListItem>();
        }
        #region Properties
        public FrameworkUAS.Entity.RuleCondition ruleCondition { get; set; }

        public int ruleId { get; set; }
        public int lineNumber { get; set; }
        public string compareValue { get; set; }
        public bool isGrouped { get; set; }
        public int groupNumber { get; set; }
        public int ruleFieldId { get; set; }
        public string uiControl { get; set; }

        //these were strings - changing to SelectedListItem to see if binding works better
        public SelectListItem selectedConnector { get; set; }
        public SelectListItem selectedIncomingField { get; set; }
        public SelectListItem selectedDataBaseField { get; set; }
        public SelectListItem selectedOperator { get; set; }

        //these properties are for the condition grid drop down controls
        //public List<SelectListItem> Connectors { get; set; }//and or
        //public List<SelectListItem> IncomingFields { get; set; }//set from ColumnMappingViewModel.IncomingColumns
        //public List<SelectListItem> DataBaseFields { get; set; }//set from ColumnMappingViewModel.IncomingColumns
        //public List<SelectListItem> Operators { get; set; }//predefined CodetypeId - GetOperators
        //public List<SelectListItem> Functions { get; set; }//undefined for now
        #endregion
    }
    [Serializable]
    public class RuleSelectListItem : SelectListItem
    {
        public RuleSelectListItem()
        {
            RuleFieldId = 0;
            UIControl = "textbox";
            IsMultiSelect = false;
            Item = new SelectListItem();
        }

        #region Properties
        [DataMember]
        public int RuleFieldId { get; set; }
        [DataMember]
        public string UIControl { get; set; }
        [DataMember]
        public bool IsMultiSelect { get; set; }
        [DataMember]
        SelectListItem Item { get; set; }
        #endregion
    }
    public class RulesAdmsProcessingViewModel
    {
        public RulesAdmsProcessingViewModel()
        {
            NewRuleType = string.Empty;
            SourceFileId = 0;
            ExistingRules = new List<FrameworkUAS.Entity.Rule>();
        }
        public RulesAdmsProcessingViewModel(string newRuleType, int sourceFileId)
        {
            NewRuleType = newRuleType;
            SourceFileId = sourceFileId;
        }
        #region Properties
        public string NewRuleType { get; set; }
        public string NewRuleName { get; set; }
        public int SourceFileId { get; set; }
        public int ExistingRuleId { get; set; }
        public string NewRuleAction { get; set; }//Insert Update Delete
        public List<FrameworkUAS.Entity.Rule> ExistingRules { get; set; }
        #endregion
        private void LoadData(KMPlatform.Entity.Client myClient)
        {

        }
    }
    //[Serializable]
    //public class RuleOrderGridViewModel
    //{
    //    public RuleOrderGridViewModel()
    //    {
    //        rules = new List<FrameworkUAS.Object.CustomRuleGrid>();
    //    }
    //    public RuleOrderGridViewModel(List<FrameworkUAS.Object.CustomRuleGrid> crGrid)
    //    {
    //        rules = crGrid;
    //    }
    //    public RuleOrderGridViewModel(int _ruleSetId, int _ruleId, string _ruleName, string _ruleTypeAction, string _ruleScript, int _executionOrder)
    //    {
    //        rules = new List<FrameworkUAS.Object.CustomRuleGrid>();
    //        FrameworkUAS.Object.CustomRuleGrid crg = new FrameworkUAS.Object.CustomRuleGrid()
    //            {
    //                RuleSetId = _ruleSetId,
    //                RuleId = _ruleId,
    //                RuleName = _ruleName,
    //                RuleTypeAction = _ruleTypeAction, 
    //                RuleScript = _ruleScript,
    //                ExecutionOrder = _executionOrder
    //            };
    //        rules.Add(crg);
    //    }

    //    public List<FrameworkUAS.Object.CustomRuleGrid> rules { get; set; }
    //}
    [Serializable]
    public class InsertUpdateNewModel
    {
        public InsertUpdateNewModel() { updateList = new List<FileMapperWizard.InsertUpdateNew>(); }
        #region properties
        [DataMember]
        public List<InsertUpdateNew> updateList { get; set; }
        #endregion
    }
    [Serializable]
    public class InsertUpdateNew
    {
        public InsertUpdateNew() { }
        #region properties
        //data.Add(new SelectListItem() { Text = i.ColumnName, Value = i.DataTable + "." + i.ColumnName });
        [DataMember]
        public string dataTableColumnName { get; set; }
        [DataMember]
        public string columnName {
            get
            {
                if (!string.IsNullOrEmpty(dataTableColumnName) && dataTableColumnName.Contains("."))
                    return dataTableColumnName.Split('.')[1].ToString();
                else
                    return string.Empty;
            }
        }
        [DataMember]
        public string dataTable
        {
            get
            {
                if (!string.IsNullOrEmpty(dataTableColumnName) && dataTableColumnName.Contains("."))
                    return dataTableColumnName.Split('.')[0].ToString();
                else
                    return string.Empty;
            }
        }
        [DataMember]
        public bool isClientField { get; set; }
        [DataMember]
        public string uiControl { get; set; }
        [DataMember]
        public string dataType { get; set; }
        [DataMember]
        public int ruleFieldId { get; set; }
        [DataMember]
        public bool isMultiSelect { get; set; }
        [DataMember]
        public string updateText { get; set; }
        [DataMember]
        public string updateValue { get; set; }
        #endregion
    }



    //new model structure
    /// <summary>
    /// main model for _rules view - List<Rule>
    /// </summary>
    //public class RuleSet
    //{
    //    public RuleSet() { }
    //    public RuleSet(int _sourceFileId, bool _isFullFile, int _ruleSetId = 0, string _ruleSetName = "", string _description = "")
    //    {
    //        sourceFileId = _sourceFileId;
    //        isFullFile = _isFullFile;
    //        ruleSetId = _ruleSetId;
    //        ruleSetName = !string.IsNullOrEmpty(_ruleSetName) ? _ruleSetName : "Rule Set - sourceFileId " + _sourceFileId.ToString() + " " + DateTime.Now.ToString("MMddyyyy");
    //        description = !string.IsNullOrEmpty(_description) ? _description : "new rule set created on " + DateTime.Now.ToString();
    //        isGlobalRuleSet = false;
    //        rules = new List<Rule>();
    //    }

    //    public int sourceFileId { get; set; }
    //    public int ruleSetId { get; set; }
    //    [Required(ErrorMessage = "Rule Set name is required.")]
    //    [Display(Name = "Rule Set Name:")]
    //    public string ruleSetName { get; set; }
    //    public string description { get; set; }
    //    public bool isFullFile { get; set; }
    //    public List<Rule> rules { get; set; }
    //    public bool isGlobalRuleSet { get; set; }
    //}
    /// <summary>
    /// model for Rules/_postDQM view - List<Condition>
    /// </summary>
    //public class Rule
    //{
    //    public Rule()
    //    {
    //        conditions = new List<FileMapperWizard.Condition>();
    //        updates = new List<FileMapperWizard.Update>();
    //    }
    //    public Rule(int _sourceFileId, int _ruleSetId)
    //    {
    //        ruleSetId = _ruleSetId;
    //        sourceFileId = _sourceFileId;

    //        conditions = new List<FileMapperWizard.Condition>();
    //        updates = new List<FileMapperWizard.Update>();
    //    }
    //    public string ruleId { get; set; }// UAS.Rule
    //    public string ruleName { get; set; }// UAS.Rule
    //    public string ruleType { get; set; }//UAS.Rule.CustomImportRuleId (UAD_Lookup.Code / CodeType 'Custom Import Rule')
    //    public string ruleAction { get; set; }//UAS.Rule.RuleActionId 
    //    public string sortOrder { get; set; }//UAS.RuleSetRuleOrder.ExecutionOrder 

    //    //items from RuleSet object
    //    public int ruleSetId { get; set; }
    //    public int sourceFileId { get; set; }

    //    public List<Condition> conditions { get; set; }//RuleCondtion
    //    public List<Update> updates { get; set; }//RuleResult
    //} 
    
    //public class Condition
    //{
    //    public string comparator { get; set; }//compareValue
    //    public List<Field> databaseFields { get; set; }//list of all available fields - if a circ file this should be limited by pubcode
    //    public List<Field> mappedFields { get; set; }//list of all fields that were mapped
    //    public IEnumerable<SelectListItem> lookupData { get; set; }
    //    public string mafField { get; set; }
    //    public Enums.FieldType fieldType { get; set; }
    //    public Enums.FieldDataType fieldDataType { get; set; }
    //    public List<FrameworkUAD_Lookup.Model.Operator> operators { get; set; }
    //    public string Operator { get; set; }
    //    public string OperatorFunction { get; set; }
    //    public string values { get; set; }
    //    public string lineNumber { get; set; }//SortOrder

    //    public FrameworkUAS.Entity.RuleCondition ruleCondition { get; set; }
    //    public int ruleId { get; set; }
    //    public bool isGrouped { get; set; }

    //    //public int ruleId { get; set; }
    //    //public int lineNumber { get; set; }
    //    //public string compareValue { get; set; }
    //    //public bool isGrouped { get; set; }
    //    //public int groupNumber { get; set; }
    //    //public int ruleFieldId { get; set; }
    //    //public string uiControl { get; set; }

    //    ////these were strings - changing to SelectedListItem to see if binding works better
    //    //public SelectListItem selectedConnector { get; set; }
    //    //public SelectListItem selectedIncomingField { get; set; }
    //    //public SelectListItem selectedDataBaseField { get; set; }
    //    //public SelectListItem selectedOperator { get; set; }

    //}
    ///// <summary>
    ///// this will now come from a sproc - nothing stored in database
    ///// </summary>
    //public class Field
    //{
    //    public string MAFField { get; set; }
    //    public string DisplayName { get; set; }

    //    public Enums.FieldDataType DataType { get; set; }

    //    public Enums.FieldType FieldType { get; set; }

    //    public static List<Field> GetFields()
    //    {
    //        //this has to come from a database sproc - use sproc that created data for RuleField table - delete RuleField table and objects
    //        List<Field> Fields = new List<Field>();

    //        Fields.Add(new Field() { DisplayName = "FirstName", MAFField = "FirstName", DataType = Enums.FieldDataType.String, FieldType = Enums.FieldType.Profile });
    //        Fields.Add(new Field() { DisplayName = "LastName", MAFField = "LastName", DataType = Enums.FieldDataType.String, FieldType = Enums.FieldType.Profile });
    //        Fields.Add(new Field() { DisplayName = "State", MAFField = "State", DataType = Enums.FieldDataType.Lookup, FieldType = Enums.FieldType.Lookup_State });
    //        Fields.Add(new Field() { DisplayName = "Country", MAFField = "Country", DataType = Enums.FieldDataType.Lookup, FieldType = Enums.FieldType.Lookup_Country });
    //        Fields.Add(new Field() { DisplayName = "QualificationDate", MAFField = "QualificationDate", DataType = Enums.FieldDataType.Date, FieldType = Enums.FieldType.Profile });
    //        Fields.Add(new Field() { DisplayName = "EmailExists", MAFField = "EmailExists", DataType = Enums.FieldDataType.Bit, FieldType = Enums.FieldType.Profile });
    //        Fields.Add(new Field() { DisplayName = "PhoneExists", MAFField = "PhoneExists", DataType = Enums.FieldDataType.Bit, FieldType = Enums.FieldType.Profile });
    //        Fields.Add(new Field() { DisplayName = "CategoryCode", MAFField = "CategoryCode", DataType = Enums.FieldDataType.Lookup, FieldType = Enums.FieldType.Lookup_Category });
    //        Fields.Add(new Field() { DisplayName = "TransactionCode", MAFField = "TransactionCode", DataType = Enums.FieldDataType.Lookup, FieldType = Enums.FieldType.Lookup_Transaction });
    //        Fields.Add(new Field() { DisplayName = "Demo1", MAFField = "Demo1", DataType = Enums.FieldDataType.Demo, FieldType = Enums.FieldType.Demo });
    //        Fields.Add(new Field() { DisplayName = "Demo2", MAFField = "Demo2", DataType = Enums.FieldDataType.Demo, FieldType = Enums.FieldType.Demo });
    //        Fields.Add(new Field() { DisplayName = "CustomFieldString", MAFField = "CustomFieldString", DataType = Enums.FieldDataType.String, FieldType = Enums.FieldType.Custom });
    //        Fields.Add(new Field() { DisplayName = "CustomFieldDate", MAFField = "CustomFieldDate", DataType = Enums.FieldDataType.Date, FieldType = Enums.FieldType.Custom });
    //        Fields.Add(new Field() { DisplayName = "CustomFieldInt", MAFField = "CustomFieldInt", DataType = Enums.FieldDataType.Int, FieldType = Enums.FieldType.Custom });
    //        Fields.Add(new Field() { DisplayName = "CustomFieldFloat", MAFField = "CustomFieldFloat", DataType = Enums.FieldDataType.Float, FieldType = Enums.FieldType.Custom });
    //        Fields.Add(new Field() { DisplayName = "CustomFieldBit", MAFField = "CustomFieldBit", DataType = Enums.FieldDataType.Bit, FieldType = Enums.FieldType.Custom });

    //        return Fields;
    //    }
    //}

    //public class Update
    //{
    //    public string MAFField { get; set; }
    //    public string Values { get; set; }

    //    [DataMember]
    //    public string dataTableColumnName { get; set; }
    //    [DataMember]
    //    public string columnName
    //    {
    //        get
    //        {
    //            if (!string.IsNullOrEmpty(dataTableColumnName) && dataTableColumnName.Contains("."))
    //                return dataTableColumnName.Split('.')[1].ToString();
    //            else
    //                return string.Empty;
    //        }
    //    }
    //    [DataMember]
    //    public string dataTable
    //    {
    //        get
    //        {
    //            if (!string.IsNullOrEmpty(dataTableColumnName) && dataTableColumnName.Contains("."))
    //                return dataTableColumnName.Split('.')[0].ToString();
    //            else
    //                return string.Empty;
    //        }
    //    }
    //    [DataMember]
    //    public bool isClientField { get; set; }
    //    [DataMember]
    //    public string uiControl { get; set; }
    //    [DataMember]
    //    public string dataType { get; set; }
    //    [DataMember]
    //    public int ruleFieldId { get; set; }
    //    [DataMember]
    //    public bool isMultiSelect { get; set; }
    //    [DataMember]
    //    public string updateText { get; set; }
    //    [DataMember]
    //    public string updateValue { get; set; }
    //}

    #endregion

    #region Search page
    public class FileManagementSearchModel
    {
        public FileManagementSearchModel()
        {

        }

        #region Properties
        public string type { get; set; }
        public string fileName { get; set; }
        public int fileType { get; set; }
        public int pubID { get; set; }
        #endregion
    }

    public class CurrentMappingModel
    {
        public CurrentMappingModel()
        {

        }

        public CurrentMappingModel(int _totalCounts, bool _isCirc, int _SourceFileID, int _DatabaseFileTypeId, string _FileType, string _FileName, bool _IsDeleted, string _Extension, string _Delimiter, bool _IsTextQualifier, int _ServiceID,
                                     int _ServiceFeatureID, DateTime _DateCreated, int _CreatedByUserID, string _CreatedByUserName, int? _PublicationID, string _PubCode, DateTime? _DateUpdated, int? _UpdatedByUserID, string _UpdatedByUserName)
        {
            TotalRecordCounts = _totalCounts;
            isCirc = _isCirc;
            SourceFileID = _SourceFileID;
            DatabaseFileTypeId = _DatabaseFileTypeId;
            FileType = _FileType;
            FileName = _FileName;
            IsDeleted = _IsDeleted;
            Extension = _Extension;
            Delimiter = _Delimiter;
            IsTextQualifier = _IsTextQualifier;
            ServiceID = _ServiceID;
            ServiceFeatureID = _ServiceFeatureID;
            DateCreated = _DateCreated;
            CreatedByUserID = _CreatedByUserID;
            CreatedByUserName = _CreatedByUserName;
            PublicationID = _PublicationID;
            PubCode = _PubCode;
            DateUpdated = _DateUpdated;
            UpdatedByUserID = _UpdatedByUserID;
            UpdatedByUserName = _UpdatedByUserName;
        }

        #region Properties      
        public int TotalRecordCounts { get; set; }
        public bool isCirc { get; set; }
        public int SourceFileID { get; set; }
        public int DatabaseFileTypeId { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public int? PublicationID { get; set; }
        public string PubCode { get; set; }
        public bool IsDeleted { get; set; }
        public string Extension { get; set; }
        public string Delimiter { get; set; }
        public bool IsTextQualifier { get; set; }
        public int ServiceID { get; set; }
        public int ServiceFeatureID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedByUserID { get; set; }
        public string CreatedByUserName { get; set; }
        public int? UpdatedByUserID { get; set; }
        public string UpdatedByUserName { get; set; }
        #endregion
    }
    #endregion
}