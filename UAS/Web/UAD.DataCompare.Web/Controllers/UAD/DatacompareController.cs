using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using Core_AMS.Utilities;
using FrameworkUAD.BusinessLogic;
using KM.Common;
using KM.Common.Extensions;
using KM.Common.Import;
using KMPlatform.Entity;
using KMPlatform.Object;
using UAD.DataCompare.Web.Controllers.Common;
using UAD.DataCompare.Web.Models;
using EntityDcDownloadView = FrameworkUAS.Entity.DataCompareDownloadView;
using EntityDcFilterDetail = FrameworkUAS.Entity.DataCompareDownloadFilterDetail;
using EntityFieldMapping = FrameworkUAS.Entity.FieldMapping;
using EntitySourceFile = FrameworkUAS.Entity.SourceFile;
using EnumAccess = KMPlatform.Enums.Access;
using EnumFeatures = KMPlatform.Enums.ServiceFeatures;
using EnumServices = KMPlatform.Enums.Services;
using FilterTypes = FrameworkUAD.BusinessLogic.Enums.FiltersType;
using LookupEnums = FrameworkUAD_Lookup.Enums;
using LookupWorkers = FrameworkUAD_Lookup.BusinessLogic;
using PlatformUser = KM.Platform.User;
using Product = FrameworkUAD.BusinessLogic.Product;
using UasWorkers = FrameworkUAS.BusinessLogic;

namespace UAD.DataCompare.Web.Controllers.UAD
{
    public class DatacompareController : BaseController
    {
        private const string FilterEmailStatus = "EMAIL STATUS";
        private const string FilterCountry = "COUNTRY";
        private const string FilterCategoryCode = "CATEGORY CODE";
        private const string FilterXact = "XACT";
        private const string FilterQSource = "QSOURCE";
        private const string ValuesSeparator = ",";
        private const char CharSeparator = ',';
        private const char VBarChar = '|';
        private const int SourceFileBatchSize = 2500;
        private const string DateFormat = "MMDDYYYY";
        private const string YesString = "yes";
        private const string NoString = "no";
        private const string CodeDc = "DC";
        private const string NameIgnore = "Ignore";
        private const string NameDelete = "Delete";
        private const string NameKmTransform = "kmTransform";
        private const string TypeVarChar = "varchar";
        private const string SpaceString = " ";
        private const int PreviewDataMaxLength = 1000;
        private const string NameBrand = "Brand";
        private const string NameProduct = "Product";
        private const string UrlEcnAccountsMain = "/ecn.accounts/main/";
        private const string TextPrint = "Print";
        private const string TextDigital = "Digital";
        private const string TextOptOut = "Opt Out";
        private const string TextBoth = "Both";
        private const string NameSteps = "steps";
        private const string NameSaveMapping = "Savemapping";
        private const string NameSaveAndImport = "SaveAndImport";
        private const string NameMapColumns = "Mapcolumns";
        private const string NamePath = "path";
        private const string NameExtention = "Extention";
        private const string NameProfileColumnList = "ProfileColumnList";
        private const string NameNotes = "Notes";

        #region Entity Declarations
        private SourceFileManager _sfMgr;
        private FrameworkUAD_Lookup.BusinessLogic.Code _code = new FrameworkUAD_Lookup.BusinessLogic.Code();
        private FrameworkUAD_Lookup.BusinessLogic.CodeType _codeType = new FrameworkUAD_Lookup.BusinessLogic.CodeType();
        private KMPlatform.Entity.Client _client = new KMPlatform.Entity.Client();
        private KMPlatform.BusinessLogic.User _kmUserBO = new KMPlatform.BusinessLogic.User();
        private KMPlatform.BusinessLogic.Client _clientWorker = new KMPlatform.BusinessLogic.Client() ;
        private KMPlatform.BusinessLogic.Service _serviceMgr = new KMPlatform.BusinessLogic.Service();
        private KMPlatform.BusinessLogic.ServiceFeature _sBFeatures = new KMPlatform.BusinessLogic.ServiceFeature();
        private FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper _prdSubExtMapper = new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper();
        private FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper _prdSubExt = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper();
        private FrameworkUAD.BusinessLogic.FileMappingColumn _fileMappingColumn = new FrameworkUAD.BusinessLogic.FileMappingColumn();
        private FrameworkUAD.BusinessLogic.ResponseGroup rgWorker = new FrameworkUAD.BusinessLogic.ResponseGroup();
        private FrameworkUAS.BusinessLogic.SourceFile _sourcefile = new FrameworkUAS.BusinessLogic.SourceFile();
        private FrameworkUAS.BusinessLogic.FieldMapping _sfFieldMapping =  new FrameworkUAS.BusinessLogic.FieldMapping();
        private FrameworkUAS.BusinessLogic.DataCompareRun _dcCompareRunBO = new FrameworkUAS.BusinessLogic.DataCompareRun();
        private FrameworkUAS.BusinessLogic.DataCompareView _dcCompareViewBO = new FrameworkUAS.BusinessLogic.DataCompareView();
        private FrameworkUAS.BusinessLogic.DataCompareDownload _dcCompareDownLoadBO = new FrameworkUAS.BusinessLogic.DataCompareDownload();
        private FrameworkUAS.BusinessLogic.DataCompareDownloadCostDetail _dcCompareDownloadCostBO = new FrameworkUAS.BusinessLogic.DataCompareDownloadCostDetail();
        #endregion

        public DatacompareController()
        {     

        }
        public enum ColumnMapperControlType
        {
            User,
            EditUser,
            Edit,
            New
        }
        public enum ColumnMapperRowType
        {
            New,
            Remove,
            Normal
        }

        #region Current Session Properties
        private int CurrentClientID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID; }

        }
        private KMPlatform.Entity.User CurrentUser
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser; }
        }

        private int CurrentClientGroupID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientGroupID; }
        }
        #endregion

        #region Index Page
        [OutputCache(Duration = 20,Location = OutputCacheLocation.Any, VaryByParam = "none")]
        public ActionResult Index()
        {
            List<SourceFile> _sfCurrentList = new List<SourceFile>();
            if (CurrentClientID >0)
            {
                if(KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.DataCompare, KMPlatform.Enums.Access.Yes))
                {
                    _sfCurrentList = new List<SourceFile>();
                    _sfMgr = new SourceFileManager();
                    _sfCurrentList = _sfMgr.getFileListByClient(CurrentClientID);
                    return View(_sfCurrentList);
                }
                else
                {
                    return Redirect("/ecn.accounts/main/");
                }

            }
            else
            {
                return Redirect("/ecn.accounts/main/");
            }

        }
        #endregion

        #region Import file
        public ActionResult ImportFileMapping()
        {
            ViewBag.MappingSuccess = false;
            ViewBag.FieldSaveSuccess = false;
            ViewBag.ImportSuccess = false;
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.DataCompare, KMPlatform.Enums.Access.Yes))
            {
                FileDetails importfile = new FileDetails();
              
                //if (TempData["model"] != null)
                //{
                //    importfile = (FileDetails)TempData["model"];
                //    //and use you viewbag data in the view
                //}
                importfile.isKMStaff = CurrentUser.IsKMStaff;
                return View(importfile);
            }
            else
            {
                  return Redirect("/ecn.accounts/main/");
            }
        }

        [HttpPost]
        public ActionResult ImportFileMapping(FileDetails file, FormCollection fc)
        {
            ViewBag.MappingSuccess = false;
            ViewBag.FieldSaveSuccess = false;
            ViewBag.ImportSuccess = false;

            var profileColumnList = Session[NameProfileColumnList];

            if (profileColumnList != null)
            {
                var tempColumnMaps = new List<ColumnMap>();
                foreach (var columnMap in file.ColumnMapping)
                {
                    columnMap.ProfileColumnList = (List<SelectListItem>)profileColumnList;
                    tempColumnMaps.Add(columnMap);
                }

                file.ColumnMapping = tempColumnMaps;
            }

            if (file.IsImportBillable.EqualsIgnoreCase(YesString) || !file.isKMStaff)
            {
                ModelState.Remove(NameNotes);
            }

            var currentStep = fc[NameSteps];
            if (currentStep.EqualsIgnoreCase(NameMapColumns))
            {
                if (ModelState.IsValid)
                {
                    return MapColumns(file);
                }
            }
            else if (currentStep.EqualsAnyIgnoreCase(NameSaveMapping, NameSaveAndImport))
            {
                var uploadToFtp = currentStep.EqualsIgnoreCase(NameSaveAndImport);
                return SaveFileAndMapping(file, uploadToFtp);
            }
            else
            {
                ModelState.AddModelError("ErrorValidation", @"Please check all the details are entered correctly?");
            }

            return View(file);
        }

        private ActionResult MapColumns(FileDetails file)
        {
            Guard.NotNull(file, nameof(file));
            _clientWorker = new KMPlatform.BusinessLogic.Client();
            _client = _clientWorker.Select(CurrentClientID);
            var fileWorker = new FileWorker();
            var fileConfig = new FileConfiguration();
            var columnMaps = new List<ColumnMap>();
            FileInfo fileInfo = null;

            if (!TryCreateFileDirectory(file, _client.ClientID))
            {
                return View(file);
            }

            if (!ValidateFileDetails(file, ref fileInfo, ref fileConfig, fileWorker))
            {
                return View(file);
            }

            if (ModelState.IsValid)
            {
                //For New mapping
                var columns = fileWorker.GetFileHeaders(fileInfo, fileConfig);
                var fileData = fileWorker.GetData(fileInfo, fileConfig);

                _fileMappingColumn = new FileMappingColumn();
                var uadColumns = _fileMappingColumn.Select(_client.ClientConnections);

                if (columns.Count > 0 && uadColumns.Count > 0)
                {
                    var columnDictionary = new Dictionary<int, string>();
                    foreach (DictionaryEntry col in columns)
                    {
                        int colOrder;
                        int.TryParse(col.Value.ToString(), out colOrder);
                        columnDictionary.Add(colOrder, col.Key.ToString());
                    }

                    foreach (var col in columnDictionary.OrderBy(x => x.Key))
                    {
                        if (!string.IsNullOrWhiteSpace(col.Value))
                        {
                            string dataPreview;
                            try
                            {
                                dataPreview = string.Join(",", fileData.AsEnumerable()
                                    .Select(s => s.Field<object>(col.Value))
                                    .Distinct()
                                    .ToArray());
                            }
                            catch (Exception ex)
                            {
                                Trace.TraceError("Unable to get data preview: {0}", ex);
                                dataPreview = "Preview error";
                            }

                            var map = new ColumnMap(_client, uadColumns, ColumnMapperControlType.New.ToString(), col.Value, dataPreview);
                            columnMaps.Add(map);
                        }
                    }
                }

                file.ColumnMapping = columnMaps;
                Session[NameProfileColumnList] = columnMaps.FirstOrDefault()?.ProfileColumnList;
                ViewBag.MappingSuccess = true;
            }

            return View(file);
        }

        private bool TryCreateFileDirectory(FileDetails file, int clientId)
        {
            Guard.NotNull(file, nameof(file));

            if (file.DataFile == null)
            {
                ModelState.AddModelError("FileIsMissing", @"Please select file to map.");
                return false;
            }

            var path = Path.Combine(Server.MapPath("~/App_Data"), clientId.ToString());
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ErrorDirectoryCreationFailed", @"Error while creating a directory.");
                return false;
            }

            file.Extention = Path.GetExtension(file.DataFile.FileName);
            file.FilePath = $"{path}\\{file.FileName}{file.Extention}";
            Session[NameExtention] = Path.GetExtension(file.DataFile.FileName);
            Session[NamePath] = $"{path}\\{file.FileName}{file.Extention}";

            using (var output = new FileStream($"{path}\\{file.FileName}{file.Extention}", FileMode.Create))
            {
                file.DataFile.InputStream.CopyTo(output);
            }

            return true;
        }

        private bool ValidateFileDetails(FileDetails file, ref FileInfo fileInfo, ref FileConfiguration fileConfig, FileWorker fileWorker)
        {
            Guard.NotNull(file, nameof(file));
            Guard.NotNull(fileWorker, nameof(fileWorker));

            if (_client != null && !string.IsNullOrWhiteSpace(file.FileName))
            {
                fileInfo = new FileInfo(file.FilePath);
                if (fileWorker.IsExcelFile(fileInfo) || fileWorker.IsDbfFile(fileInfo) || fileWorker.IsZipFile(fileInfo) ||
                    fileWorker.IsJsonFile(fileInfo) || fileWorker.IsXmlFile(fileInfo))
                {
                    fileConfig = null;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(file.Delimiter) || string.IsNullOrWhiteSpace(file.HasQuotation))
                    {
                        ModelState.AddModelError(
                            "NullDelOrQuote",
                            @"Missing File Info: File Delimiter and/or File contains double quotation marks.  Please update Quotations selection before advancing.<");
                        return false;
                    }

                    var isQuoted = file.HasQuotation.EqualsIgnoreCase(YesString);
                    fileConfig.FileColumnDelimiter = file.Delimiter;
                    fileConfig.IsQuoteEncapsulated = isQuoted;
                }
            }
            else
            {
                ModelState.AddModelError("NullData", @"Data is missing:Please make sure client was selected and/or file was selected.");
                return false;
            }

            _sourcefile = new UasWorkers.SourceFile();
            var clientSources = _sourcefile.Select(false)
                .Where(x => x.ClientID == CurrentClientID && !x.IsDeleted)
                .ToList();
            if (clientSources.Any(x => x.FileName.Equals(file.FileName, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(
                    "SaveAsNameAlreadyExist",
                    @"File Previously Mapped: Records show the current file has been previously mapped. Please locate the file in Edit Mapping to make changes to it.");
                return false;
            }

            return true;
        }

        private ActionResult SaveFileAndMapping(FileDetails file, bool uploadToFtp)
        {
            Guard.NotNull(file, nameof(file));
            if (string.IsNullOrWhiteSpace(file.Extention))
            {
                if (Session[NameExtention] != null)
                {
                    file.Extention = (string)Session[NameExtention];
                }
                else if (Session[NamePath] != null)
                {
                    file.Extention = Path.GetExtension((string)Session[NamePath]);
                }
                else
                {
                    ModelState.AddModelError(
                        "SFE_NoFileExtention",
                        "File extetion can not be detected. Please try again. If issue persist, please contact system administrator.");
                }
            }

            var sourceFileId = !string.IsNullOrWhiteSpace(file.Extention)
                ? SaveSourceFile(file)
                : 0;

            if (sourceFileId > 0)
            {
                var saveFieldMapStatus = SaveFieldMappingNew(file.ColumnMapping, sourceFileId);

                if (saveFieldMapStatus)
                {
                    if (uploadToFtp)
                    {
                        bool uploadStatus;
                        if (Session[NamePath] != null)
                        {
                            var fileinfo = new FileInfo(Session[NamePath].ToString());
                            uploadStatus = UploadFileToFtp(fileinfo);
                        }
                        else
                        {
                            ModelState.AddModelError(
                                "FileIsNotPresent",
                                "Please select file for updload. File is not available at temporory location path.");

                            return View(file);
                        }

                        if (!uploadStatus)
                        {
                            ModelState.AddModelError(
                                "MoreThanOneFTPSetting",
                                "More Than One FTP Setting for Client: Client may have more than one active FTP settings. Please contact customer service to have this fixed before proceeding.");

                            return View(file);
                        }

                        ViewBag.ImportSuccess = true;
                        return RedirectToAction("Index", "Datacompare");
                    }

                    ViewBag.FieldSaveSuccess = true;
                    return RedirectToAction("Index", "Datacompare");
                }

                ModelState.AddModelError("Duplicatecolumn", "Duplicate columne map found. Please correct and save the changes.");
            }
            else
            {
                ModelState.AddModelError(
                    "SFE_SaveFailed",
                    "File save failed. Please try later. If issue persist, please contact the system administrator.");
            }

            return View(file);
        }

        #endregion

        #region Edit Mapping
        public ActionResult EditFileMapping(int SourceFileID =0)
        {
            ViewBag.MappingSuccess = false;
            ViewBag.FieldSaveSuccess = false;
            ViewBag.ImportSuccess = false;
            _sourcefile = new FrameworkUAS.BusinessLogic.SourceFile();
            FrameworkUAS.Entity.SourceFile sourceFileEnt = _sourcefile.SelectSourceFileID(SourceFileID, true);

            //Check if file is belong to same client
            bool isValid = sourceFileEnt.ClientID == CurrentClientID;

            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.DataCompare, KMPlatform.Enums.Access.Yes))
            {
                FileDetails importfile = LoadFileDetails(SourceFileID);
                return View(importfile);
            }
            else
            {
                return Redirect("/ecn.accounts/main/");
            }

        }

        [HttpPost]
        public ActionResult EditFileMapping(FileDetails file, FormCollection fc)
        {
            ViewBag.MappingSuccess = false;
            ViewBag.FieldSaveSuccess = false;
            ViewBag.ImportSuccess = false;
            ColumnMap newColumnMap;
           
            #region ENTITY INTITALIZATION
            _clientWorker = new KMPlatform.BusinessLogic.Client();
            _client = new KMPlatform.Entity.Client();
            _client = _clientWorker.Select(CurrentClientID, false);
            FileInfo _fInfo;
            FileWorker fw = new FileWorker();
            var _fConfig = new FileConfiguration();
            List<ColumnMap> _lstColumnMapper = new List<ColumnMap>();
            StringDictionary columns;
            bool saveFieldMapStatus = false;
            int sourceFileID = 0;
            bool uploadStatus = false;
            #endregion

            List<ColumnMap> _lstTempColumnMapper = new List<ColumnMap>();
            if (file.IsImportBillable.Equals("Yes", StringComparison.OrdinalIgnoreCase) || !file.isKMStaff)
            {
                file.Notes = string.Empty;
                this.ModelState.Remove("Notes");
            }
            foreach (ColumnMap cm in file.ColumnMapping)
            {
                if (cm.MappedColumn != "Delete")
                {
                    newColumnMap = cm;
                    newColumnMap.ProfileColumnList = (List<SelectListItem>)Session["ProfileColumnList"];
                    _lstTempColumnMapper.Add(newColumnMap);
                }
            }
            file.ColumnMapping = _lstTempColumnMapper;


            
            #region Validation and Column Import
            if (fc["steps"].ToString() == "Rescan")
            {
                if (ModelState.IsValid)
                {
                    #region Create File Directory To Store Files

                    if (file.DataFile == null)
                    {
                        ModelState.AddModelError("FileIsMissing", @"Please select file to map.");
                        return View(file);
                    }

                    string path = Server.MapPath("~/App_Data") + "\\" + _client.ClientID;
                    try
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ErrorDirectoryCreationFailed", @"Error while creating a directory.");
                        return View(file);
                    }

                    file.Extention = Path.GetExtension(file.DataFile.FileName.ToString());
                    file.FilePath = path + "\\" + file.FileName + file.Extention;
                    Session["Extention"] = Path.GetExtension(file.DataFile.FileName.ToString());
                    Session["path"] = path + "\\" + file.FileName + file.Extention;
                    //Assign Path for importing file to FTP

                    using (System.IO.FileStream output = new System.IO.FileStream(path + "\\" + file.FileName + file.Extention, FileMode.Create))
                    {
                        file.DataFile.InputStream.CopyTo(output);
                    }



                    #endregion

                    #region CHECK CLIENT AND FILE DETAILS ARE FILLED OUT
                    if (_client != null && !string.IsNullOrEmpty(file.FileName))
                    {
                        _fInfo = new FileInfo(file.FilePath);
                        if (fw.IsExcelFile(_fInfo) || fw.IsDbfFile(_fInfo) || fw.IsZipFile(_fInfo) || fw.IsJsonFile(_fInfo) || fw.IsXmlFile(_fInfo))
                        {
                            _fConfig = null;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(file.Delimiter) || string.IsNullOrEmpty(file.HasQuotation))
                            {

                                ModelState.AddModelError("NullDelOrQuote", @"Missing File Info: File Delimiter and/or File contains double quotation marks.  Please update Quotations selection before advancing.<");
                                return View(file);
                            }
                            else
                            {
                                bool isQuoted = false;
                                if (file.HasQuotation.Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                                    isQuoted = true;

                                _fConfig.FileColumnDelimiter = file.Delimiter;
                                _fConfig.IsQuoteEncapsulated = isQuoted;
                            }
                        }
                    }
                    else
                    {

                        ModelState.AddModelError("NullData", @"Data is missing:Please make sure client was selected and/or file was selected.");
                        return View(file);
                    }
                    #endregion

                   
                    if (ModelState.IsValid)
                    {

                        //For New mapping
                        List<string> dupColumns = new List<string>();
                        dupColumns = fw.GetDuplicateColumns(_fInfo, _fConfig);
                        columns = fw.GetFileHeaders(_fInfo, _fConfig, true);

                        DataTable fileData = fw.GetData(_fInfo, _fConfig);

                        _fileMappingColumn = new FrameworkUAD.BusinessLogic.FileMappingColumn();
                        List<FrameworkUAD.Object.FileMappingColumn> uadColumns = _fileMappingColumn.Select(_client.ClientConnections);


                        Dictionary<int, string> pubCode = FrameworkUAS.Object.DBWorker.GetPubIDAndCodesByClient(_client);
                        if (columns.Count > 0)
                        {
                            if (uadColumns.Count > 0)
                            {

                                Dictionary<int, string> _columns = new Dictionary<int, string>();
                                foreach (DictionaryEntry col in columns)
                                {
                                    int colOrder = 0;
                                    int.TryParse(col.Value.ToString(), out colOrder);
                                    _columns.Add(colOrder, col.Key.ToString());
                                }
                                foreach (KeyValuePair<int, string> col in _columns.OrderBy(x => x.Key))
                                {
                                    string dataPreview = "";
                                    if (col.Value != null && !string.IsNullOrEmpty(col.Value))
                                    {
                                        try
                                        {
                                            dataPreview = String.Join(",",
                                                fileData.AsEnumerable().Select(s => s.Field<object>(col.Value)).Distinct().ToArray<object>());
                                        }
                                        catch
                                        {
                                            dataPreview = "Preview error";
                                        }
                                        ColumnMap mc;
                                        mc = new ColumnMap(_client, uadColumns, ColumnMapperControlType.New.ToString(), col.Value, dataPreview);

                                        _lstColumnMapper.Add(mc);


                                    }
                                    else
                                        continue;
                                }
                            }

                        }

                        file.ColumnMapping = _lstColumnMapper;
                        Session["ProfileColumnList"] = _lstColumnMapper[0].ProfileColumnList;
                        ViewBag.MappingSuccess = true;
                    }
                }
            }

            #endregion

            #region Save File and Mapping Without FTP Upload
            if (fc["steps"].ToString() == "Savemapping")
            {

                if (string.IsNullOrEmpty(file.Extention))
                {
                    if (Session["Extention"] != null)
                        file.Extention = (string)Session["Extention"];
                    else if (Session["path"] != null)
                        file.Extention = Path.GetExtension((string)Session["path"]);
                    else
                    {
                        ModelState.AddModelError("SFE_NoFileExtention", "File extetion can not be detected. Please try again. If issue persist, please contact system administrator.");
                    }
                }
                if (!string.IsNullOrEmpty(file.Extention))
                    sourceFileID = SaveSourceFile(file);
                else
                    sourceFileID = 0;
                if (sourceFileID > 0)
                {
                    saveFieldMapStatus = SaveFieldMappingNew(file.ColumnMapping, sourceFileID);
                    if (saveFieldMapStatus)
                    {
                        ViewBag.FieldSaveSuccess = true;
                    }
                    else
                    {
                        ModelState.AddModelError("Duplicatecolumn", "Duplicate columne map found. Please correct and save the changes.");
                    }
                }
                file.ColumnMapping = RebuildColumnMap(file, _lstColumnMapper) as List<ColumnMap>;

            }
            #endregion

            #region Save Field mapping and Import File to FTP
            else if (fc["steps"].ToString() == "SaveAndImport")
            {
                if (string.IsNullOrEmpty(file.Extention))
                {
                    if (Session["Extention"] != null)
                        file.Extention = (string)Session["Extention"];
                    else if (Session["path"] != null)
                        file.Extention = Path.GetExtension((string)Session["path"]);
                    else
                    {
                        ModelState.AddModelError("SFE_NoFileExtention", "File extetion can not be detected. Please try again. If issue persist, please contact system administrator.");
                    }
                }
                if (!string.IsNullOrEmpty(file.Extention))
                    sourceFileID = SaveSourceFile(file);
                else
                    sourceFileID = 0;
                if (sourceFileID > 0)
                {
                    saveFieldMapStatus = SaveFieldMappingNew(file.ColumnMapping, sourceFileID);

                    if (saveFieldMapStatus)
                    {
                        file.ColumnMapping = RebuildColumnMap(file, _lstColumnMapper) as List<ColumnMap>;

                        FileInfo fileinfo;
                        if (Session["path"] != null)
                        {
                            fileinfo = new System.IO.FileInfo(Session["path"].ToString());
                            uploadStatus = UploadFileToFtp(fileinfo);

                        }
                        else
                        {
                            ModelState.AddModelError("FileIsNotPresent", "Please select file for updload. File is not available at temporory location path.");
                        }
                        if (!uploadStatus)
                        {
                            ModelState.AddModelError("MoreThanOneFTPSetting", "More Than One FTP Setting for Client: Client may have more than one active FTP settings. Please contact customer service to have this fixed before proceeding.");

                        }
                        else
                        {
                            ViewBag.ImportSuccess = true;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Duplicatecolumn", "Duplicate columne map found. Please correct and save the changes.");
                    }
                }
                }
            #endregion

            return View(file);
        }

        public IList<ColumnMap> RebuildColumnMap(FileDetails file, IList<ColumnMap> lstColumnMapper)
        {
            if (file == null) 
            {  
                throw new ArgumentNullException(nameof(file));
            }

            if (lstColumnMapper == null) 
            {
                throw new ArgumentNullException(nameof(lstColumnMapper));
            }

            foreach (var cm in file.ColumnMapping)
            {
                if (cm.MappedColumn == "Delete")
                {
                    continue;
                }

                var newColumnMap = cm;
                if (Session != null)
                {
                    newColumnMap.ProfileColumnList = Session["ProfileColumnList"] as List<SelectListItem>;
                }

                lstColumnMapper.Add(newColumnMap);
            }

            return lstColumnMapper;
        }

        private FileDetails LoadFileDetails(int SourceFileID)
        {
            _sourcefile = new FrameworkUAS.BusinessLogic.SourceFile();
            FrameworkUAS.BusinessLogic.FieldMapping fm = new FrameworkUAS.BusinessLogic.FieldMapping();

            _clientWorker = new KMPlatform.BusinessLogic.Client();
            _client = new KMPlatform.Entity.Client();
            _client = _clientWorker.Select(CurrentClientID, false);

            FrameworkUAS.Entity.SourceFile sourceFileEnt = _sourcefile.SelectSourceFileID(SourceFileID, true);
            List<FrameworkUAS.Entity.FieldMapping> fmEntityList = new List<FrameworkUAS.Entity.FieldMapping>();
            fmEntityList = fm.Select(SourceFileID, false);
            _fileMappingColumn = new FrameworkUAD.BusinessLogic.FileMappingColumn();
            List<FrameworkUAD.Object.FileMappingColumn> uadColumns = _fileMappingColumn.Select(_client.ClientConnections);


            FileDetails importfile = new FileDetails();
            ColumnMap cm;
            importfile.SourceFileID = SourceFileID;
            importfile.ColumnMapping = new List<ColumnMap>();
            foreach(var fmap in fmEntityList)
            {
                cm = new ColumnMap(_client, uadColumns, ColumnMapperControlType.Edit.ToString(), fmap.IncomingField, fmap.PreviewData, fmap.MAFField);
                cm.SourceFileID = SourceFileID;
                cm.FieldMapID = fmap.FieldMappingID;
                cm.SourceColumn = fmap.IncomingField;
                cm.MappedColumn = fmap.MAFField;
                cm.PreviewDataColumn = fmap.PreviewData;
                importfile.ColumnMapping.Add(cm);
            }
            importfile.FileName = sourceFileEnt.FileName;
            importfile.DateCreated = sourceFileEnt.DateCreated;
            importfile.Delimiter = sourceFileEnt.Delimiter;
            importfile.Extention = sourceFileEnt.Extension;
            importfile.isKMStaff = CurrentUser.IsKMStaff;
            importfile.DateUpdated = sourceFileEnt.DateUpdated;
            importfile.Notes = sourceFileEnt.Notes;
            importfile.NotificationEmail = sourceFileEnt.NotifyEmailList;
            if (sourceFileEnt.IsBillable)
            {
                importfile.IsImportBillable = "Yes";
            }
            else
            {
                importfile.IsImportBillable = "No";
            }
            if (sourceFileEnt.IsTextQualifier)
            {
                importfile.HasQuotation = "Yes";
            }
            else
            {
                importfile.HasQuotation = "No";
            }

            if(importfile.ColumnMapping!=null && importfile.ColumnMapping.Count>0)
            {
                Session["ProfileColumnList"] = importfile.ColumnMapping[0].ProfileColumnList;
            }
            return importfile;
        }
        #endregion

        #region Save Source file and Field Mapping

        private int SaveSourceFile(FileDetails file)
        {
            Guard.NotNull(file, nameof(file));
            var sourceFileId = file.SourceFileID;
            var sourceFileWorker = new UasWorkers.SourceFile();
            var sourceFile = new EntitySourceFile();

            try
            {
                if (file.SourceFileID > 0)
                {
                    sourceFile = sourceFileWorker.SelectSourceFileID(file.SourceFileID);
                    sourceFile.FileName = file.FileName;
                    sourceFile.UpdatedByUserID = CurrentUser.UserID;
                    sourceFile.DateUpdated = DateTime.Now;
                    sourceFile.Delimiter = file.Delimiter;
                    sourceFile.IsTextQualifier = file.HasQuotation.Equals(YesString, StringComparison.OrdinalIgnoreCase);
                    //Added for Version 2.0
                    sourceFile.Notes = file.Notes;
                    sourceFile.IsBillable = !file.IsImportBillable.Equals(NoString, StringComparison.OrdinalIgnoreCase);
                    sourceFileId = sourceFileWorker.Save(sourceFile);
                }
                else if (sourceFileId == 0)
                {
                    sourceFileId = SaveNewSourceFile(sourceFileWorker, file, sourceFile);
                }
            }
            catch (Exception ex)
            {
                // Possible Bug: error not handled.
                Trace.TraceError($"An error occurred saving FileDetails with name: {file.FileName}");
                return 0;
            }

            return sourceFileId;
        }

        private int SaveNewSourceFile(UasWorkers.SourceFile sourceFileWorker, FileDetails file, EntitySourceFile sourceFile)
        {
            Guard.NotNull(sourceFileWorker, nameof(sourceFileWorker));
            Guard.NotNull(file, nameof(file));
            Guard.NotNull(sourceFile, nameof(sourceFile));

            var fileSnippetId = 0;
            var svFileSnipCode = _code.SelectCodeValue(LookupEnums.CodeType.File_Snippet,
                LookupEnums.FileSnippetTypes.Prefix.ToString());
            sourceFile.SourceFileID = 0;
            _code = new LookupWorkers.Code();

            UpdateDbFileType(sourceFile, _code);

            if (svFileSnipCode != null)
            {
                fileSnippetId = svFileSnipCode.CodeId;
            }

            var oneTimeString = LookupEnums.FileRecurrenceTypes.One_Time.ToString().Replace("_", " ");
            var fileRecList = _code.Select(LookupEnums.CodeType.File_Recurrence)
                .Where(x => x.IsActive)
                .ToList();
            var recurrenceType = fileRecList
                .Single(x => x.CodeName.Equals(oneTimeString))
                .CodeId;
            sourceFile.FileRecurrenceTypeId = recurrenceType;
            sourceFile.PublicationID = 0;
            sourceFile.FileName = file.FileName;
            sourceFile.ClientID = CurrentClientID;
            sourceFile.IsDeleted = false;
            sourceFile.IsIgnored = false;
            sourceFile.FileSnippetID = fileSnippetId;
            sourceFile.IsDQMReady = true;
            sourceFile.Delimiter = file.Delimiter;
            sourceFile.IsTextQualifier = file.HasQuotation.Equals(YesString, StringComparison.OrdinalIgnoreCase);

            //Added for Version 2.0
            sourceFile.Notes = file.Notes;
            sourceFile.IsBillable = !file.IsImportBillable.Equals(NoString, StringComparison.OrdinalIgnoreCase);
            sourceFile.NotifyEmailList = file.NotificationEmail;
            sourceFile.Extension = file.Extention;

            _serviceMgr = new KMPlatform.BusinessLogic.Service();
            var entityService = _serviceMgr.SelectForClientID(CurrentClientID);

            var currentService = entityService.Single(x =>
                x.ServiceCode.Equals(KMPlatform.Enums.Services.UADFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));

            if (currentService.ServiceID > 0)
            {
                var serviceId = currentService.ServiceID;

                _sBFeatures = new KMPlatform.BusinessLogic.ServiceFeature();
                var features = _sBFeatures.SelectOnlyEnabledClientID(serviceId, CurrentClientID);
                var feature = features.First(x => x.SFCode == CodeDc);

                sourceFile.ServiceID = currentService.ServiceID;
                sourceFile.ServiceFeatureID = feature.ServiceFeatureID;
            }

            sourceFile.MasterGroupID = 0;
            sourceFile.UseRealTimeGeocoding = false;
            sourceFile.IsSpecialFile = false;
            sourceFile.ClientCustomProcedureID = 0;
            sourceFile.SpecialFileResultID = 0;
            sourceFile.DateCreated = DateTime.Now;
            sourceFile.DateUpdated = DateTime.Now;
            sourceFile.CreatedByUserID = UserID;
            sourceFile.UpdatedByUserID = UserID;
            sourceFile.QDateFormat = DateFormat;
            sourceFile.BatchSize = SourceFileBatchSize;

            return sourceFileWorker.Save(sourceFile);
        }

        private void UpdateDbFileType(EntitySourceFile sourceFile, LookupWorkers.Code codeWorker)
        {
            Guard.NotNull(sourceFile, nameof(sourceFile));
            Guard.NotNull(codeWorker, nameof(codeWorker));
            var databaseFileTypeList = codeWorker.Select(LookupEnums.CodeType.Database_File)
                .Where(x => x.IsActive)
                .ToList();
            var dataCompareString = LookupEnums.FileTypes.Data_Compare.ToString().Replace("_", " ");
            var databaseFileType = databaseFileTypeList
                .Single(x => x.CodeName.Equals(dataCompareString))
                .CodeId;
            sourceFile.DatabaseFileTypeId = databaseFileType;

            //IF UAD File and Db File Type = 0 try and set to CodeId for Audience Data
            if (databaseFileType == 0)
            {
                if (databaseFileTypeList.Count == 0)
                {
                    var dbTypeCodes = codeWorker.SelectCodeName(
                        LookupEnums.CodeType.Database_File,
                        LookupEnums.FileTypes.Audience_Data.ToString());
                    if (dbTypeCodes != null)
                    {
                        databaseFileType = dbTypeCodes.CodeId;
                    }
                }
                else
                {
                    var audienceDataString = LookupEnums.FileTypes.Audience_Data.ToString().Replace("_", " ");
                    databaseFileType = databaseFileTypeList
                        .Single(x => x.CodeName.Equals(audienceDataString))
                        .CodeId;
                }

                sourceFile.DatabaseFileTypeId = databaseFileType;
            }
        }

        private bool SaveFieldMappingNew(IReadOnlyList<ColumnMap> columnMappings, int sourceFileId)
        {
            Guard.NotNull(columnMappings, nameof(columnMappings));
            var fieldMappingUpdateStatus = false;
            var passFieldSaving = true;
            var checkDuplicateMapping = new Dictionary<string, int>();
            _sfFieldMapping = new UasWorkers.FieldMapping();

            foreach(var mapping in columnMappings)
            {
                if(mapping.MappedColumn == NameIgnore || mapping.MappedColumn == NameDelete)
                {
                    continue;
                }

                if (checkDuplicateMapping.ContainsKey(mapping.MappedColumn))
                {
                    passFieldSaving = false;
                    break;
                }

                checkDuplicateMapping.Add(mapping.MappedColumn, 1);
            }

            if (passFieldSaving && sourceFileId > 0)
            {
                try
                {
                    SaveColumnMappingsForExistingSourceFile(columnMappings, sourceFileId);
                    fieldMappingUpdateStatus = true;
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                    fieldMappingUpdateStatus = false;
                }
            }

            return fieldMappingUpdateStatus;
        }

        private void SaveColumnMappingsForExistingSourceFile(IEnumerable<ColumnMap> columnMappings, int sourceFileId)
        {
            Guard.NotNull(columnMappings, nameof(columnMappings));

            var columnOrder = 1;
            foreach (var columnMap in columnMappings)
            {
                var fieldMapId = columnMap.FieldMapID;
                var mappedTo = columnMap.MappedColumn;
                var colName = columnMap.SourceColumn;
                var previewData = columnMap.PreviewDataColumn;

                var mapping = new EntityFieldMapping
                {
                    FieldMappingID = fieldMapId,
                    SourceFileID = sourceFileId,
                    IncomingField = colName,
                    MAFField = mappedTo,
                    PubNumber = 0,
                    DataType = TypeVarChar,
                    IsNonFileColumn = false,
                    HasMultiMapping = false,
                    ColumnOrder = columnOrder,
                    DemographicUpdateCodeId = 0,
                    CreatedByUserID = UserID,
                    UpdatedByUserID = UserID,
                    DateCreated = DateTime.Now
                };

                UpdateFieldMappingEntity(mapping, previewData, mappedTo);

                if (fieldMapId > 0)
                {
                    mapping.DateUpdated = DateTime.Now;
                }

                // If incoming field and Outputfields are null or blank do nothing
                if (string.IsNullOrWhiteSpace(mapping.IncomingField) || string.IsNullOrWhiteSpace(mapping.MAFField))
                {
                    continue;
                }

                if (mappedTo == NameDelete)
                {
                    if (mapping.FieldMappingID > 0)
                    {
                        _sfFieldMapping.DeleteMapping(mapping.FieldMappingID);
                    }
                }
                else
                {
                    _sfFieldMapping.Save(mapping);
                    columnOrder++;
                }
            }
        }

        private void UpdateFieldMappingEntity(EntityFieldMapping mapping, string previewData, string mappedTo)
        {
            Guard.NotNull(mapping, nameof(mapping));

            // White space is taken into consideration in "else" branch.
            if (!string.IsNullOrEmpty(previewData))
            {
                mapping.PreviewData = previewData.Length > PreviewDataMaxLength
                    ? previewData.Substring(0, PreviewDataMaxLength).Replace("'", string.Empty)
                    : previewData.Replace("'", string.Empty);
            }
            else
            {
                mapping.PreviewData = SpaceString;
            }

            var fieldMappingTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
            _code = new LookupWorkers.Code();

            if (fieldMappingTypes.Count == 0)
            {
                fieldMappingTypes = _code.Select(LookupEnums.CodeType.Field_Mapping);
            }

            var fmlIgnoredId = fieldMappingTypes
                .Single(x => x.CodeName.EqualsIgnoreCase(LookupEnums.FieldMappingTypes.Ignored.ToString()))
                .CodeId;
            var fmlKmTransformId = fieldMappingTypes
                .Single(x => x.CodeName.EqualsIgnoreCase(LookupEnums.FieldMappingTypes.kmTransform.ToString()))
                .CodeId;
            var fmlStandardId = fieldMappingTypes
                .Single(x => x.CodeName.EqualsIgnoreCase(LookupEnums.FieldMappingTypes.Standard.ToString()))
                .CodeId;

            if (mappedTo == NameIgnore)
            {
                mapping.FieldMappingTypeID = fmlIgnoredId;
            }
            else if (mappedTo == NameKmTransform)
            {
                mapping.FieldMappingTypeID = fmlKmTransformId;
            }
            else
            {
                mapping.FieldMappingTypeID = fmlStandardId;
            }
        }

        #endregion

        #region Get Owner
        private string getFileOwner(int createdByUserID)
        {
            string userEmail = "";
            KMPlatform.Entity.User _user = new KMPlatform.Entity.User();
            try
            {
                _kmUserBO = new KMPlatform.BusinessLogic.User();
                _user = _kmUserBO.SelectUser(createdByUserID);
                userEmail = _user.EmailAddress;
            }
            catch (Exception ex)
            {
                userEmail = "N/A";
            }

            return userEmail;
        }
        #endregion

        #region Upload to FTP Function
        private bool UploadFileToFtp(System.IO.FileInfo file)
        {

            //if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[CurrentClientID].ClientFtpDirectoriesList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().Count > 1)
            //{

            //    return false;

            //}

            FrameworkUAS.BusinessLogic.ClientFTP cFTPBl = new FrameworkUAS.BusinessLogic.ClientFTP();
            FrameworkUAS.Entity.ClientFTP cFTP = cFTPBl.SelectClient(CurrentClientID).FirstOrDefault();
            if (cFTP != null)
            {
                string host = "";
                //host = cFTP.Server + "/ADMS/DataCompare/";
                //Modified to new FTP location
                int count = 0;  
                host = cFTP.Server + ConfigurationManager.AppSettings["ftpfolder"].ToString();

                Core_AMS.Utilities.FtpFunctions ftp = new Core_AMS.Utilities.FtpFunctions(host, cFTP.UserName, cFTP.Password);

                bool uploadSuccess = false;
                while (!uploadSuccess && count<5)
                {
                    uploadSuccess = ftp.Upload(file.Name, file.FullName);
                    count++;
                }
                
                return uploadSuccess;
                

            }
            else
            {
                return false;

            }



        }
        #endregion

        #region View Downloads

        public ActionResult Viewcomparision(int SourceFileID = 0, int targetFilter = 0, int typeFilter = 0, int? scopeFilter = null)
        {
            ViewBag.fileNameFilter = string.Empty;
            ViewBag.targetFilter = string.Empty;
            ViewBag.typeFilter = string.Empty;
            ViewBag.scopeFilter = string.Empty;

            var dataCompareDownloadViewWorker = new UasWorkers.DataCompareDownloadView();
            var brandWorker = new Brand();
            var productWorker = new Product();
            var dcDownloadViewList = dataCompareDownloadViewWorker.SelectForClient(CurrentClientID);
            var vmViewComparisionList = new List<ViewComparisionViewModel>();
            var codeList = _code.Select();

            _clientWorker = new KMPlatform.BusinessLogic.Client();
            _client = _clientWorker.Select(CurrentClientID);

            if (dcDownloadViewList == null || !dcDownloadViewList.Any())
            {
                return View(vmViewComparisionList);
            }

            var isValid = true;
            if (PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.DataCompare, EnumAccess.Yes))
            {
                if (SourceFileID > 0)
                {
                    var sourceFile = _sourcefile.SelectSourceFileID(SourceFileID);
                    ViewBag.fileNameFilter = sourceFile.FileName;
                    isValid = sourceFile.ClientID == CurrentClientID;
                }

                if (isValid && dcDownloadViewList.Any())
                {
                    var brands = brandWorker.Select(_client.ClientConnections);
                    var products = productWorker.Select(_client.ClientConnections);
                    if (targetFilter > 0)
                    {
                        var targetFilterValue = codeList.First(x => x.CodeId == targetFilter).CodeName;
                        ViewBag.targetFilter = targetFilterValue;
                        var scopeFilterValue = GetBrandOrProductScopeFilter(targetFilterValue, scopeFilter, brands, products);
                        ViewBag.scopeFilter = scopeFilterValue;
                    }

                    if (typeFilter > 0)
                    {
                        ViewBag.typeFilter = codeList.First(x => x.CodeId == typeFilter).CodeName;
                    }

                    foreach (var item in dcDownloadViewList)
                    {
                        var model = GetViewComparisionViewModel(item, codeList, brands, products);
                        vmViewComparisionList.Add(model);
                    }
                }
                else
                {
                    return Redirect(UrlEcnAccountsMain);
                }
            }
            else
            {
                return Redirect(UrlEcnAccountsMain);
            }

            return View(vmViewComparisionList);
        }

        private ViewComparisionViewModel GetViewComparisionViewModel(
            EntityDcDownloadView item,
            IReadOnlyList<FrameworkUAD_Lookup.Entity.Code> codeList,
            IEnumerable<FrameworkUAD.Entity.Brand> brands,
            IEnumerable<FrameworkUAD.Entity.Product> products)
        {
            var model = new ViewComparisionViewModel();
            var sfEntity = _sourcefile.SelectSourceFileID(item.SourceFileID);
            var target = codeList.First(x => x.CodeId == item.DcTargetCodeId).CodeName;
            var criteria = codeList.First(x => x.CodeId == item.DcTypeCodeId).CodeName;
            var scope = GetBrandOrProductScopeFilter(target, item.DcTargetIdUad, brands, products);
            var comparisonType = codeList.FirstOrDefault(x => x.CodeId == item.DcTypeCodeId)?.CodeName;

            model.Price = $"{item.TotalDownLoadCost:C2}";
            model.FileName = sfEntity.FileName;
            model.DcDownloadId = item.DcDownloadId;
            model.User = getFileOwner(item.CreatedByUserID);
            model.Target = target;
            model.TotalRecords = item.TotalRecordCount;
            model.Scope = scope;
            model.Type = comparisonType;
            model.Query = $"{criteria}:{item.WhereClause}";
            model.DownloadedFileName = item.DownloadFileName;
            model.DownLoadDate = item.DateCreated;

            return model;
        }

        private string GetBrandOrProductScopeFilter(
            string target,
            int? targetId,
            IEnumerable<FrameworkUAD.Entity.Brand> brands,
            IEnumerable<FrameworkUAD.Entity.Product> products)
        {
            Guard.NotNull(target, nameof(target));
            Guard.NotNull(brands, nameof(brands));
            Guard.NotNull(products, nameof(products));

            string scope;
            if (target.EqualsIgnoreCase(NameBrand) && targetId > 0)
            {
                scope = $"Brand: {brands.FirstOrDefault(x => x.BrandID == targetId)?.BrandName}";
            }
            else if (target.EqualsIgnoreCase(NameProduct) && targetId > 0)
            {
                scope = $"Product: {products.FirstOrDefault(x => x.PubID == targetId)?.PubName}";
            }
            else
            {
                scope = target;
            }

            return scope;
        }

        public ActionResult LoadFilterDetails(int dcDownloadId)
        {
            var filterDetails = new List<FilterDetailsModel>();
            var dcFilterGroups = new UasWorkers.DataCompareDownloadFilterGroup().SelectForDownload(dcDownloadId);
            var filterNumber = 0;

            foreach (var filterGroup in dcFilterGroups)
            {
                filterNumber++;
                foreach (var filterDetail in filterGroup.DcFilterDetails)
                {
                    string selectedText;

                    if (TryGetFilterValue(dcDownloadId, filterDetail, out selectedText))
                    {
                        var model = new FilterDetailsModel
                        {
                            FilterNo = filterNumber,
                            Field = filterDetail.Name,
                            Values = selectedText
                        };
                        filterDetails.Add(model);
                    }
                }
            }

            return PartialView("~/Views/DataCompare/Partials/ViewFilters/_FilterDetailView.cshtml", filterDetails);
        }

        private bool TryGetFilterValue(int downloadId, EntityDcFilterDetail filterDetail, out string filterValue)
        {
            Guard.NotNull(filterDetail, nameof(filterDetail));

            var clientConnections = new KMPlatform.BusinessLogic.Client()
                .Select(CurrentClientID)
                .ClientConnections;
            var dcCompareDownloadViewList = new UasWorkers.DataCompareDownloadView().SelectForClient(CurrentClientID)
                .Find(x => x.DcDownloadId == downloadId);
            var target = _code.Select()
                .Where(x => x.CodeId == dcCompareDownloadViewList.DcTargetCodeId)
                .Select(x => x.CodeName)
                .First();
            var dcTargetIdUad = dcCompareDownloadViewList.DcTargetIdUad;

            filterValue = string.Empty;
            var success = true;

            switch ((FilterTypes)filterDetail.FilterType)
            {
                case FilterTypes.Brand:
                    filterValue = GetBrandFilterValue(clientConnections, filterDetail);
                    break;
                case FilterTypes.Product:
                    filterValue = GetProductFilterValue(clientConnections, filterDetail);
                    break;
                case FilterTypes.Dimension:
                    filterValue = GetDimensionFilterValue(clientConnections, filterDetail, target);
                    break;
                case FilterTypes.Standard:
                    success = TryGetStandardFilterValue(clientConnections, filterDetail, out filterValue);
                    break;
                case FilterTypes.Geo:
                    var selectedValue = filterDetail.SearchCondition.Split(VBarChar);
                    filterValue = $"{selectedValue[0]} & {selectedValue[1]} miles - {selectedValue[2]} miles";
                    break;
                case FilterTypes.Activity:
                    filterValue = GetActivityFilterValue(clientConnections, filterDetail);
                    break;
                case FilterTypes.Adhoc:
                    filterValue = GetAdhocFilterValue(clientConnections, filterDetail, target, dcTargetIdUad);
                    break;
                case FilterTypes.Circulation:
                    filterValue = GetCirculationFilterValue(filterDetail);
                    break;
                default:
                    success = false;
                    break;
            }

            return success;
        }

        private bool TryGetStandardFilterValue(ClientConnections clientConnections, EntityDcFilterDetail filterDetail, out string filterValue)
        {
            Guard.NotNull(filterDetail, nameof(filterDetail));
            filterValue = string.Empty;

            switch (filterDetail.Name.ToUpper())
            {
                case "STATE":
                    filterValue = GetStandardStateFilterValue(filterDetail);
                    break;
                case "COUNTRY":
                    filterValue = GetStandardCountryFilterValue(filterDetail);
                    break;
                case "MEDIA":
                    filterValue = GetStandardMediaFilterValue(filterDetail);
                    break;
                case "EMAIL":
                case "PHONE":
                case "FAX":
                case "MAILPERMISSION":
                case "FAXPERMISSION":
                case "PHONEPERMISSION":
                case "OTHERPRODUCTSPERMISSION":
                case "THIRDPARTYPERMISSION":
                case "EMAILRENEWPERMISSION":
                case "TEXTPERMISSION":
                case "GEOLOCATED":
                    filterValue = GetStandardGeoLocatedFilterValue(filterDetail);
                    break;
                case "EMAIL STATUS":
                    filterValue = GetEmailStatusFilterValue(clientConnections, filterDetail);
                    break;
                default:
                    return false;
            }

            return true;
        }

        private string GetStandardGeoLocatedFilterValue(EntityDcFilterDetail filterDetail)
        {
            Guard.NotNull(filterDetail, nameof(filterDetail));
            var selectedValues = filterDetail.Values.Split(CharSeparator);
            var stringBuilder = new StringBuilder();

            foreach (var selected in selectedValues)
            {
                var text = selected == "1"
                    ? "Yes"
                    : selected == "0"
                        ? "No"
                        : "Blank";
                stringBuilder.Append(stringBuilder.Length == 0 ? text : $", {text}");
            }

            return stringBuilder.ToString();
        }

        private string GetStandardMediaFilterValue(EntityDcFilterDetail filterDetail)
        {
            Guard.NotNull(filterDetail, nameof(filterDetail));
            var selectedValues = filterDetail.Values.Split(CharSeparator);
            var stringBuilder = new StringBuilder();

            foreach (var selected in selectedValues)
            {
                switch (selected)
                {
                    case "A":
                        stringBuilder.Append(stringBuilder.Length == 0 ? TextPrint : $",{TextPrint}");
                        break;
                    case "B":
                        stringBuilder.Append(stringBuilder.Length == 0 ? TextDigital : $",{TextDigital}");
                        break;
                    case "O":
                        stringBuilder.Append(stringBuilder.Length == 0 ? TextOptOut : $",{TextOptOut}");
                        break;
                    case "C":
                        stringBuilder.Append(stringBuilder.Length == 0 ? TextBoth : $",{TextBoth}");
                        break;
                }
            }

            return stringBuilder.ToString();
        }

        private string GetStandardCountryFilterValue(EntityDcFilterDetail filterDetail)
        {
            Guard.NotNull(filterDetail, nameof(filterDetail));
            var selectedValues = filterDetail.Values.Split(CharSeparator);
            var stringBuilder = new StringBuilder();

            foreach (var selected in selectedValues)
            {
                int countryId;
                if (!int.TryParse(selected, out countryId))
                {
                    throw new InvalidOperationException($"Unable to parse country ID from '{selected}'");
                }

                var text = new LookupWorkers.Country().Select()
                    .Where(x => x.CountryID == countryId)
                    .Select(x => x.ShortName)
                    .FirstOrDefault();
                stringBuilder.Append(stringBuilder.Length == 0 ? text : $",{text}");
            }

            return stringBuilder.ToString();
        }

        private string GetStandardStateFilterValue(EntityDcFilterDetail filterDetail)
        {
            Guard.NotNull(filterDetail, nameof(filterDetail));
            var selectedValues = filterDetail.Values.Split(CharSeparator);
            var stringBuilder = new StringBuilder();

            foreach (var selected in selectedValues)
            {
                var text = new LookupWorkers.Region().Select()
                    .Where(x => x.RegionCode == selected)
                    .Select(x => x.RegionCode)
                    .FirstOrDefault();
                stringBuilder.Append(stringBuilder.Length == 0 ? text : $",{text}");
            }

            return stringBuilder.ToString();
        }

        private string GetBrandFilterValue(ClientConnections clientConnections, EntityDcFilterDetail filterDetail)
        {
            Guard.NotNull(clientConnections, nameof(clientConnections));
            Guard.NotNull(filterDetail, nameof(filterDetail));

            int brandId;
            if (!int.TryParse(filterDetail.Values, out brandId))
            {
                throw new InvalidOperationException($"Unable to parse brand ID from '{filterDetail.Values}'");
            }

            var selectedText = new Brand().Select(clientConnections)
                .Where(x => x.BrandID == brandId)
                .Select(x => x.BrandName)
                .FirstOrDefault();

            return selectedText ?? string.Empty;
        }


        private string GetProductFilterValue(ClientConnections clientConnections, EntityDcFilterDetail filterDetail)
        {
            Guard.NotNull(clientConnections, nameof(clientConnections));
            Guard.NotNull(filterDetail, nameof(filterDetail));
            var selectedValue = filterDetail.Values.Split(CharSeparator);
            var stringBuilder = new StringBuilder();

            foreach (var selected in selectedValue)
            {
                int pubId;
                if (!int.TryParse(selected, out pubId))
                {
                    throw new InvalidOperationException($"Unable to parse product ID from '{selected}'");
                }

                var text = new Product().Select(clientConnections)
                    .Where(x => x.PubID == pubId)
                    .Select(x => x.PubName)
                    .FirstOrDefault();
                stringBuilder.Append(stringBuilder.Length == 0 ? text : $",{text}");
            }

            return stringBuilder.ToString();
        }

        private string GetDimensionFilterValue(ClientConnections clientConnections, EntityDcFilterDetail filterDetail, string target)
        {
            Guard.NotNull(clientConnections, nameof(clientConnections));
            Guard.NotNull(filterDetail, nameof(filterDetail));

            string selectedText;
            if (NameProduct.EqualsAnyIgnoreCase(target))
            {
                selectedText = GetEmailStatusFilterValue(clientConnections, filterDetail);
            }
            else
            {
                var selectedValues = filterDetail.Values.Split(CharSeparator);
                var stringBuilder = new StringBuilder();

                foreach (var selected in selectedValues)
                {
                    int masterId;
                    if (!int.TryParse(selected, out masterId))
                    {
                        throw new InvalidOperationException($"Unable to parse master ID from '{selected}'");
                    }
                    var masterCodeSheet = new FrameworkUAD.BusinessLogic.MasterCodeSheet()
                        .Select(clientConnections)
                        .FirstOrDefault(x => x.MasterID == masterId);
                    var text = $"{masterCodeSheet?.MasterDesc} ({masterCodeSheet?.MasterValue})";
                    stringBuilder.Append(stringBuilder.Length == 0 ? text : $",{text}");
                }

                selectedText = stringBuilder.ToString();
            }

            return selectedText;
        }

        private string GetCirculationFilterValue(EntityDcFilterDetail filterDetail)
        {
            Guard.NotNull(filterDetail, nameof(filterDetail));
            var stringBuilder = new StringBuilder();

            foreach (var item in filterDetail.Values.Split(CharSeparator))
            {
                var text = SelectItemDisplayName(filterDetail.Name, item);

                var separator = stringBuilder.Length == 0 ? string.Empty : ValuesSeparator;
                stringBuilder.Append($"{separator}{text}");
            }

            return stringBuilder.ToString();
        }

        private string GetAdhocFilterValue(
            ClientConnections clientConnections,
            EntityDcFilterDetail filterDetail,
            string target,
            int dcTargetIdUad)
        {
            Guard.NotNull(clientConnections, nameof(clientConnections));
            Guard.NotNull(filterDetail, nameof(filterDetail));
            string selectedText;
            string[] selectedValues;
            var strIds = filterDetail.Group.Split(VBarChar);

            switch (strIds[0].ToLower())
            {
                case "m":
                    int masterGroupId;
                    selectedValues = filterDetail.Group.Split(VBarChar);

                    if (!int.TryParse(selectedValues[1], out masterGroupId))
                    {
                        throw new InvalidOperationException($"Unable parse MasterGroupId from '{selectedValues[1]}'");
                    }
                    selectedText = new MasterGroup().Select(clientConnections)
                        .Where(x => x.MasterGroupID == masterGroupId)
                        .Select(x => x.DisplayName)
                        .FirstOrDefault();

                    break;
                case "e":
                    if (NameProduct.EqualsIgnoreCase(target))
                    {
                        selectedValues = filterDetail.Group.Split(VBarChar);
                        selectedText = new ProductSubscriptionsExtensionMapper().SelectAll(clientConnections)
                            .Where(x => x.StandardField.EqualsIgnoreCase(selectedValues[1]) && x.PubID == dcTargetIdUad)
                            .Select(x => x.CustomField)
                            .FirstOrDefault();
                    }
                    else
                    {
                        selectedValues = filterDetail.Group.Split(VBarChar);
                        selectedText = new SubscriptionsExtensionMapper().SelectAll(clientConnections)
                            .Where(x => x.StandardField.EqualsIgnoreCase(selectedValues[1]))
                            .Select(x => x.CustomField)
                            .FirstOrDefault();
                    }

                    break;
                default:
                    var groupValues = filterDetail.Group.Split(VBarChar);

                    if (groupValues.Length > 1)
                    {
                        groupValues[1] = groupValues[1].Replace("[", string.Empty);
                        groupValues[1] = groupValues[1].Replace("]", string.Empty);
                        selectedText = groupValues[1];
                    }
                    else
                    {
                        groupValues[0] = groupValues[0].Replace("[", string.Empty);
                        groupValues[0] = groupValues[0].Replace("]", string.Empty);
                        selectedText = groupValues[0];
                    }

                    break;
            }

            return selectedText;
        }

        private string GetActivityFilterValue(ClientConnections clientConnections, EntityDcFilterDetail filterDetail)
        {
            Guard.NotNull(filterDetail, nameof(filterDetail));
            var selectedText = string.Empty;

            switch (filterDetail.Name.ToUpper())
            {
                case "OPEN CRITERIA":
                    selectedText = GetNumberRangeText(filterDetail.Values, "Opened", "No Opens");
                    break;
                case "CLICK CRITERIA":
                    selectedText = GetNumberRangeText(filterDetail.Values, "Clicked", "No Clicks");
                    break;
                case "VISIT CRITERIA":
                    selectedText = GetNumberRangeText(filterDetail.Values, "Visited", "No Visits");
                    break;
                case "OPEN ACTIVITY":
                case "OPEN BLASTID":
                    selectedText = filterDetail.Values;
                    break;
                case "OPEN CAMPAIGNS":
                    selectedText = GetActivityCampaignFilterValue(clientConnections, filterDetail);
                    break;
                case "OPEN EMAIL SUBJECT":
                case "OPEN EMAIL SENT DATE":
                case "LINK":
                case "CLICK ACTIVITY":
                case "CLICK BLASTID":
                    selectedText = filterDetail.Values;
                    break;
                case "CLICK CAMPAIGNS":
                    selectedText = GetActivityCampaignFilterValue(clientConnections, filterDetail);
                    break;
                case "CLICK EMAIL SUBJECT":
                case "CLICK EMAIL SENT DATE":
                    selectedText = filterDetail.Values;
                    break;
                case "DOMAIN TRACKING":
                    int trackingId;
                    if (!int.TryParse(filterDetail.Values, out trackingId))
                    {
                        throw new InvalidOperationException($"Unable parse DomainTrackingId from '{filterDetail.Values}'");
                    }
                    selectedText = new DomainTracking().Select(clientConnections)
                        .Where(x => x.DomainTrackingID == trackingId)
                        .Select(x => x.DomainName)
                        .FirstOrDefault();
                    break;
                case "URL":
                case "VISIT ACTIVITY":
                    selectedText = filterDetail.Values;
                    break;
            }

            return selectedText;
        }

        private static string GetNumberRangeText(string filterValue, string prefix, string zeroText)
        {
            var selectedText = string.Empty;
            switch (filterValue)
            {
                case "0":
                    selectedText = zeroText;
                    break;
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "10":
                case "15":
                case "20":
                case "30":
                    selectedText = $"{prefix} {filterValue}+";
                    break;
            }

            return selectedText;
        }

        private static string GetActivityCampaignFilterValue(ClientConnections clientConnections, EntityDcFilterDetail filterDetail)
        {
            Guard.NotNull(clientConnections, nameof(clientConnections));
            Guard.NotNull(filterDetail, nameof(filterDetail));
            var selectedValues = filterDetail.Values.Split(CharSeparator);
            var stringBuilder = new StringBuilder();

            foreach (var selected in selectedValues)
            {
                int campaignId;
                if (!int.TryParse(selected, out campaignId))
                {
                    throw new InvalidOperationException($"Unable parse campaign ID from '{selected}'");
                }
                var text = new ECNCampaign().Select(clientConnections)
                    .Where(x => x.ECNCampaignID == campaignId)
                    .Select(x => x.ECNCampaignName)
                    .FirstOrDefault();
                stringBuilder.Append(stringBuilder.Length == 0 ? text : $",{text}");
            }

            return stringBuilder.ToString();
        }

        public string GetEmailStatusFilterValue(ClientConnections clientConnections, EntityDcFilterDetail filterDetail)
        {
            Guard.NotNull(clientConnections, nameof(clientConnections));
            Guard.NotNull(filterDetail, nameof(filterDetail));

            int intItem;
            var stringBuilder = new StringBuilder();

            foreach (var item in filterDetail.Values.Split(CharSeparator))
            {
                var text = string.Empty;

                if (filterDetail.Name.Equals(FilterEmailStatus, StringComparison.OrdinalIgnoreCase))
                {
                    int.TryParse(item, out intItem);
                    text = new EmailStatus().Select(clientConnections)
                        .Where(x => x.EmailStatusID == intItem)
                        .Select(x => x.Status)
                        .FirstOrDefault();
                }
                else
                {
                    int.TryParse(item, out intItem);
                    var codeSheet = new CodeSheet().Select(clientConnections)
                        .FirstOrDefault(x => x.CodeSheetID == intItem);

                    if (codeSheet != null)
                    {
                        text = $"{codeSheet.ResponseDesc} ({codeSheet.ResponseValue})";
                    }
                }
                var separator = stringBuilder.Length == 0 ? string.Empty : ValuesSeparator;
                stringBuilder.Append($"{separator}{text}");
            }

            return stringBuilder.ToString();
        }

        public string SelectItemDisplayName(string filterDetailName, string item)
        {
            int intItem;
            var text = string.Empty;

            if (string.IsNullOrWhiteSpace(filterDetailName))
            {
                return text;
            }

            switch (filterDetailName.ToUpper())
            {
                case FilterCountry:
                    int.TryParse(item, out intItem);
                    text = new FrameworkUAD_Lookup.BusinessLogic.Country().Select()
                        .Where(x => x.CountryID == intItem)
                        .Select(x => x.ShortName)
                        .FirstOrDefault();
                    break;

                case FilterCategoryCode:
                    int.TryParse(item, out intItem);
                    text = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode().Select()
                        .Where(x => x.CategoryCodeID == intItem)
                        .Select(x => x.CategoryCodeName)
                        .FirstOrDefault();
                    break;

                case FilterXact:
                    int.TryParse(item, out intItem);
                    text = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType().Select()
                        .Where(x => x.TransactionCodeTypeID == intItem)
                        .Select(x => x.TransactionCodeTypeName)
                        .FirstOrDefault();
                    break;

                case FilterQSource:
                    int.TryParse(item, out intItem);
                    text = new FrameworkUAD_Lookup.BusinessLogic.Code().Select()
                        .Where(x => x.CodeId == intItem)
                        .Select(x => x.DisplayName)
                        .FirstOrDefault();
                    break;
                default:
                    break;
            }

            return text ?? string.Empty;
        }

        public FileContentResult Download(string file)
        {
            string url = ConfigurationManager.AppSettings["DataCompare_Downloadfilepath"].ToString();
            var webClient = new WebClient();
            url = url + CurrentClientID + "/" + file;
            byte[] fileBytes;
            string fileName = "filter_report.tsv";
            try
            {
                fileBytes = webClient.DownloadData(url);
               
            }
            catch (Exception ex)
            {
                fileBytes = Encoding.ASCII.GetBytes("File with name "+ file+" is not available.");
                fileName = "FileNotFound.txt";
            }
           
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        #endregion

        #region View Pricing
        public ActionResult ViewPricing(int SourceFileID = 0, string FileName = "")
        {
            List<ViewPricingListModel> viewPricingList = new List<ViewPricingListModel>();

            _clientWorker = new KMPlatform.BusinessLogic.Client();
            _client = new KMPlatform.Entity.Client();
            _client = _clientWorker.Select(CurrentClientID, false);


            ViewPricingListModel viewPricing = new ViewPricingListModel();

            FrameworkUAS.Entity.SourceFile _sfEntity;
            List<FrameworkUAD_Lookup.Entity.Code> _codeList = _code.Select();

            FrameworkUAS.BusinessLogic.DataComparePricingView dataComparePricingViewBO = new FrameworkUAS.BusinessLogic.DataComparePricingView();
            List<FrameworkUAS.Entity.DataComparePricingView> dataComparePricingView = dataComparePricingViewBO.SelectForClient(CurrentClientID);

            List<FrameworkUAD.Entity.Brand> brands = new List<FrameworkUAD.Entity.Brand>();
            List<FrameworkUAD.Entity.BrandDetail> brandDetails = new List<FrameworkUAD.Entity.BrandDetail>();
            List<FrameworkUAD.Entity.Product> products = new List<FrameworkUAD.Entity.Product>();
            FrameworkUAD.BusinessLogic.Brand _brandWorker = new FrameworkUAD.BusinessLogic.Brand();
            FrameworkUAD.BusinessLogic.Product _productWorker = new FrameworkUAD.BusinessLogic.Product();
            List<PaymentStatus> statusList = new List<PaymentStatus>();
            bool isAdmin = CurrentUser.IsPlatformAdministrator;
            ViewBag.IsAdmin = isAdmin;
            //Create Payment Status List
            FrameworkUAD_Lookup.Entity.CodeType paymentstatusType = _codeType.Select(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status);
            List<FrameworkUAD_Lookup.Entity.Code> paymentStatusList = _codeList.Where(x => x.CodeTypeId == paymentstatusType.CodeTypeId).ToList();

            if (dataComparePricingView == null || dataComparePricingView.Count() == 0)
            {
                return View(viewPricingList);
            }
            PaymentStatus ps;
            foreach (var paymentsstaus in paymentStatusList)
            {
                ps = new PaymentStatus();
                ps.PaymentStatusID = paymentsstaus.CodeId;
                ps.PaymentStatusName = paymentsstaus.CodeName;
                statusList.Add(ps);
            }

            if(KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.DataCompare, KMPlatform.Enums.Access.Yes))
            { 
                ViewData["Status"] = statusList;
                brands = _brandWorker.Select(_client.ClientConnections);
                products = _productWorker.Select(_client.ClientConnections);
                ViewBag.fileNameFilter = FileName;

                foreach (var item in dataComparePricingView)
                {
                    viewPricing = new ViewPricingListModel();
                    _sfEntity = new FrameworkUAS.Entity.SourceFile();
                    _sfEntity = _sourcefile.SelectSourceFileID(item.SourceFileID);
                    viewPricing.payStatusList = statusList;
                    viewPricing.TargetId = item.DcTargetCodeId;
                    viewPricing.ScopeId = item.DcTargetIdUad;
                    viewPricing.TypeId = item.DcTypeCodeId;
                    if (item.PaymentStatusId == 0)
                        viewPricing.PStatus = viewPricing.payStatusList.Where(x => x.PaymentStatusName == "Pending").First();
                    else
                    {
                        viewPricing.PStatus = viewPricing.payStatusList.Where(x => x.PaymentStatusID == item.PaymentStatusId).First();
                    }
                    string target = _codeList.Where(x => x.CodeId == item.DcTargetCodeId).Select(x => x.CodeName).First();
                    string scope = "";
                    if (target == "Brand" && item.DcTargetIdUad > 0)
                    {
                        scope = "Brand: " + brands.Where(x => x.BrandID == item.DcTargetIdUad).Select(x => x.BrandName).FirstOrDefault();
                    }
                    else if (target.Equals("Product", StringComparison.OrdinalIgnoreCase) && item.DcTargetIdUad > 0)
                    {
                        scope = "Product: " + products.Where(x => x.PubID == item.DcTargetIdUad).Select(x => x.PubName).FirstOrDefault();
                    }
                    else
                    {
                        scope = target;
                    }
                    string comparisionType = _codeList.Where(x => x.CodeId == item.DcTypeCodeId).Select(x => x.CodeName).FirstOrDefault();
                    viewPricing.DcViewID = item.DcViewID;
                    viewPricing.SourceFileID = item.SourceFileID;
                    viewPricing.DateCompared = item.DateCreated;
                    viewPricing.Billable = item.IsBillable ? "Yes" : "No";
                    viewPricing.User = getFileOwner(item.CreatedByUserID);
                    viewPricing.FileName = _sfEntity.FileName;
                    viewPricing.Price =  item.Cost+item.TotalDownLoadCost;
                    viewPricing.FileComaprsionCost = String.Format("{0:C2}", item.Cost);
                    viewPricing.TotalDownLoadCost = String.Format("{0:C2}", item.TotalDownLoadCost);
                    viewPricing.UnpaidDate = item.DateCreated.AddDays(14);
                    viewPricing.TotalDownloaded = item.TotalRecordCount;
                    viewPricing.TypeOfComparision = comparisionType;
                    viewPricing.Target = target;
                    viewPricing.Scope = scope;
                    viewPricing.Notes = item.Notes;
                    viewPricing.TotalRecordCount = item.UadNetCount;
                    viewPricing.FileRecordCount = item.FileRecordCount;
                    viewPricing.MatchedRecordCount = item.MatchedRecordCount;
                    viewPricing.IsAdmin = isAdmin;
                    viewPricingList.Add(viewPricing);


                }


            }
            else
            {

                return Redirect("/ecn.accounts/main/");
            }

            return View(viewPricingList);

        }
        
        public JsonResult GetDataCompareFileNames()
        {
            FrameworkUAS.BusinessLogic.DataComparePricingView dataComparePricingViewBO = new FrameworkUAS.BusinessLogic.DataComparePricingView();
            FrameworkUAS.BusinessLogic.SourceFile sourcefile = new FrameworkUAS.BusinessLogic.SourceFile();

            List<FrameworkUAS.Entity.DataComparePricingView> dataComparePricingView = dataComparePricingViewBO.SelectForClient(CurrentClientID);
            FrameworkUAS.Entity.SourceFile _sfEntity;
            List<FrameworkUAS.Entity.SourceFile> fileNameList = new List<FrameworkUAS.Entity.SourceFile>();
           
            foreach (var item in dataComparePricingView)
            {
                _sfEntity = new FrameworkUAS.Entity.SourceFile();
                _sfEntity = sourcefile.SelectSourceFileID(item.SourceFileID);
                fileNameList.Add(_sfEntity);
            }
            return Json(fileNameList.Select(e =>e.FileName).Distinct(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdatePaymentStatus(int DCViewID=0,string PaymentStatusName="")
        {
            int flag = 0;
            _dcCompareViewBO = new FrameworkUAS.BusinessLogic.DataCompareView();
            List< FrameworkUAS.Entity.DataCompareView> dcViewList =_dcCompareViewBO.SelectForClient(CurrentClientID);
            FrameworkUAS.Entity.DataCompareView dcView = dcViewList.Where(x => x.DcViewId == DCViewID).FirstOrDefault();
            FrameworkUAD_Lookup.Entity.Code payStatusCode = _code.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, PaymentStatusName);
            if (payStatusCode != null && CurrentUser.IsPlatformAdministrator && dcViewList.Count>0)
            {
                dcView.PaymentStatusId = payStatusCode.CodeId;
                flag =_dcCompareViewBO.Save(dcView);
            }
            return Json(flag, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }


}