using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using KM.Common;
using KMPlatform.BusinessLogic;
using UAS.Web.Controllers.Common;
using UAS.Web.Models.Circulations;
using UAS.Web.Models.Dashboard;
using static Core_AMS.Utilities.StringFunctions;
using static FrameworkUAD_Lookup.Enums;
using static FrameworkUAS.BusinessLogic.AdmsLog;
using static KMPlatform.Enums;
using Enums = FrameworkUAD_Lookup.Enums;
using KMBusiness = KMPlatform.BusinessLogic;
using KMEnums = KMPlatform.BusinessLogic.Enums;
using KMP = KM.Platform;
using KMPEnums = KMPlatform.Enums;
using KMUser = KM.Platform.User;
using Product = KMPlatform.Object.Product;
using UadBusiness = FrameworkUAD.BusinessLogic;
using UADBusiness = FrameworkUAD_Lookup.BusinessLogic;
using UADCode = FrameworkUAD_Lookup.BusinessLogic.Code;
using UADEntity = FrameworkUAD_Lookup.Entity;
using UADEnums = FrameworkUAD_Lookup.Enums;
using UADSourceFile = FrameworkUAS.BusinessLogic.SourceFile;
using UasBusiness = FrameworkUAS.BusinessLogic;
using UASEntity = FrameworkUAS.Entity;

namespace UAS.Web.Controllers.Dashboard
{
    public class DashboardController : BaseController
    {
        public static readonly string DelimiterUnderscore = "_";
        public static readonly string DelimiterSpace = " ";
        private const string ActionError = "Error";
        private const string ControllerError = "Error";
        private const string ErrorTypeUnauthorized = "UnAuthorized";
        private const string PubCodeNotAvailable = "N/A";
        private const int RecordCountThreshold15k = 15000;
        private const int RecordCountThreshold50k = 50000;
        private const int RecordCountThreshold100k = 100000;

        //private List<string> _fileHistoryPubCodes;
        private List<string> FileHistoryPubCodes
        {

            get
            {
                if (Session["FileHistoryPubCodes"] == null)
                {
                    FrameworkUAD.BusinessLogic.Product productWrk = new FrameworkUAD.BusinessLogic.Product();
                    List<FrameworkUAD.Entity.Product> products = new List<FrameworkUAD.Entity.Product>();
                    KMPlatform.BusinessLogic.Client clientWrk = new KMPlatform.BusinessLogic.Client();
                    KMPlatform.Entity.Client client = clientWrk.Select(CurrentClientID, false);
                    products = productWrk.Select(client.ClientConnections, false).OrderBy(x => x.PubCode).ToList();
                    List<string> pubs = products.Select(e => e.PubCode).Distinct().ToList();
                    Session["FileHistoryPubCodes"] = pubs;
                    return pubs;
                }
                else
                    return (List<string>) Session["FileHistoryPubCodes"];
            }
            set
            {
                Session["FileHistoryPubCodes"] = value;
            }
        }
        private List<string> FileHistoryFileTypes
        {
            get
            {
                if (Session["FileHistoryFileTypes"] == null)
                {
                    FrameworkUAD_Lookup.BusinessLogic.Code codeWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
                    List<string> pubs = codeWrk.Select(Enums.CodeType.Database_File).Select(e => e.CodeName).Distinct().ToList();
                    Session["FileHistoryFileTypes"] = pubs;
                    return pubs;
                }
                else
                    return (List<string>) Session["FileHistoryFileTypes"];
            }
            set
            {
                Session["FileHistoryFileTypes"] = value;
            }
        }
        private int LoadedClientId
        {
            get
            {
                if (Session["FileHistoryLoadedClientId"] == null)
                {
                    Session["FileHistoryLoadedClientId"] = 0;
                    return 0;
                }
                else
                    return (int) Session["FileHistoryLoadedClientId"];
            }
            set
            {
                Session["FileHistoryLoadedClientId"] = value;
            }
        }
        private int SearchedClientId
        {
            get
            {
                if (Session["FileHistorySearchedClientId"] == null)
                {
                    Session["FileHistorySearchedClientId"] = 0;
                    return 0;
                }
                else
                    return (int) Session["FileHistorySearchedClientId"];
            }
            set
            {
                Session["FileHistorySearchedClientId"] = value;
            }
        }
        private string selectedRecordSource
        {
            get
            {
                if (Session["FileHistoryRecordSource"] == null)
                {
                    Session["FileHistoryRecordSource"] = string.Empty;
                    return string.Empty;
                }
                else
                    return (string) Session["FileHistoryRecordSource"];
            }
            set
            {
                Session["FileHistoryRecordSource"] = value;
            }
        }
        private DateTime? startDate
        {
            get
            {
                return (DateTime?) Session["FileHistoryStartDate"];
            }
            set
            {
                Session["FileHistoryStartDate"] = value;
            }
        }
        private DateTime? endDate
        {
            get
            {
                 return (DateTime?) Session["FileHistoryEndDate"];
            }
            set
            {
                Session["FileHistoryEndDate"] = value;
            }
        }
        private List<FileHistory> viewFileHistoryList
        {
            get
            {
                if (Session["FileHistory_ViewFileHistoryList"] == null)
                    return new List<Models.Circulations.FileHistory>();
                else
                    return (List<FileHistory>) Session["FileHistory_ViewFileHistoryList"];
            }
            set
            {
                Session["FileHistory_ViewFileHistoryList"] = value;
            }
        }
        private FileHistorySearch fileHistSearch
        {
            get
            {
                if (Session["FileHistory_fileHistSearch"] == null)
                    Session["FileHistory_fileHistSearch"] = new FileHistorySearch();

                 return (FileHistorySearch) Session["FileHistory_fileHistSearch"];
            }
            set
            {
                Session["FileHistory_fileHistSearch"] = value;
            }
        }

        #region FileStatus
        // GET: Dashboard
        public ActionResult Index()
        {
            if (HasFsUadFullAccess() || HasFscFullAccess())
            {
                var engineLogWorker = new UasBusiness.EngineLog();
                var engineLogs = engineLogWorker.Select(CurrentClientID);

                var circList = new List<FileStatus>();
                var uadList = new List<FileStatus>();
                var apiList = new List<FileStatus>();

                ProcessAdmsLogs(circList, uadList, apiList);

                var dashboardModel = new DashboardModel
                {
                    EngineLogs = engineLogs,
                    CircList = circList,
                    UadList = uadList,
                    ApiList = apiList,
                    Products = CurrentClient.Products.Where(x => x.IsCirc == true).ToList(),
                    isUAD = HasFsUadFullAccess(),
                    isCirc = HasFscFullAccess()
                };

                return View(dashboardModel);
            }
            else
            {
                return RedirectToAction(
                    ActionError, 
                    ControllerError, 
                    new
                    {
                        errorType = ErrorTypeUnauthorized
                    });
            }
        }

        private void ProcessAdmsLogs(
            ICollection<FileStatus> circList, 
            ICollection<FileStatus> uadList, 
            ICollection<FileStatus> apiList)
        {
            var clientWorker = new KMBusiness.Client();
            var codeWorker = new UADBusiness.Code();
            var sourceFileWorker = new UasBusiness.SourceFile();
            var admsLogWorker = new UasBusiness.AdmsLog();

            List<UASEntity.SourceFile> sourceFiles;
            var recordProcessTimeCodeList = codeWorker.Select(UADEnums.CodeType.Record_Process_Time);
            var admsLogs = FillSourceFiles(clientWorker, admsLogWorker, sourceFileWorker, out sourceFiles);

            foreach (var admsLog in admsLogs)
            {
                var sourceFile = sourceFiles.FirstOrDefault(x => x.SourceFileID == admsLog.SourceFileId);
                var prod = CurrentClient.Products.FirstOrDefault(x => x.ProductID == sourceFile.PublicationID);
                var fileType = CodeList.FirstOrDefault(x => x.CodeId == sourceFile.DatabaseFileTypeId);
                var status = CodeList.FirstOrDefault(x => x.CodeId == admsLog.ProcessingStatusId);

                var fileStatus = SetFileStatus(admsLog, prod, fileType, status, recordProcessTimeCodeList);

                if (admsLog.RecordSource != 
                        UADEnums.FileTypes.API.ToString() &&
                    admsLog.RecordSource != 
                        UADEnums.FileTypes.Audience_Data.ToString().Replace(DelimiterUnderscore, DelimiterSpace) &&
                    admsLog.RecordSource != 
                        UADEnums.FileTypes.Data_Compare.ToString().Replace(DelimiterUnderscore, DelimiterSpace))
                {
                    circList.Add(fileStatus);
                }
                else if (admsLog.RecordSource == 
                            UADEnums.FileTypes.Audience_Data.ToString().Replace(DelimiterUnderscore, DelimiterSpace) ||
                         admsLog.RecordSource == 
                            UADEnums.FileTypes.Data_Compare.ToString().Replace(DelimiterUnderscore, DelimiterSpace))
                {
                    uadList.Add(fileStatus);
                }
                else if (admsLog.RecordSource == UADEnums.FileTypes.API.ToString())
                {
                    apiList.Add(fileStatus);
                }
            }
        }

        private bool HasFscFullAccess()
        {
            return KMP.User.HasAccess(
                CurrentUser,
                KMPEnums.Services.FULFILLMENT,
                KMPEnums.ServiceFeatures.FSC,
                KMPEnums.Access.FullAccess);
        }

        private bool HasFsUadFullAccess()
        {
            return KMP.User.HasAccess(
                CurrentUser,
                KMPEnums.Services.UAD,
                KMPEnums.ServiceFeatures.FSUAD,
                KMPEnums.Access.FullAccess);
        }

        private FileStatus SetFileStatus(UASEntity.AdmsLog admsLog, Product prod, UADEntity.Code fileType, UADEntity.Code status,
            List<UADEntity.Code> recordProcessTimeCodeList)
        {
            var fileStatus = new FileStatus
            {
                FileName = admsLog.FileNameExact,
                PubCode = string.IsNullOrWhiteSpace(prod?.ProductCode) ? PubCodeNotAvailable : prod.ProductCode,
                FileType = string.IsNullOrWhiteSpace(fileType?.CodeName) ? string.Empty : fileType.CodeName,
                Totalsteps = status != null ? CodeList.Count(x => x.CodeTypeId == status.CodeTypeId) : 0,
                StepsCompleted = status?.DisplayOrder ?? 0,
                Status = (status == null) ? string.Empty : status.DisplayName,
                StartTime = admsLog.FileStart,
                OriginalRecordCount = admsLog.OriginalRecordCount,
                isPassMaxProcessingTime = false
            };

            DetectPotentialSlowFiles(admsLog, recordProcessTimeCodeList, fileStatus);

            return fileStatus;
        }

        private static void DetectPotentialSlowFiles(
            UASEntity.AdmsLog admsLog, 
            IReadOnlyCollection<UADEntity.Code> recordProcessTimeCodeList, 
            FileStatus fileStatus)
        {
            Guard.NotNull(admsLog, nameof(admsLog));

            var span = DateTime.Now.Subtract(admsLog.FileStart);
            var minutes = span.TotalMinutes;
            var limits = new ProcessingLimits(recordProcessTimeCodeList);
            if (admsLog.OriginalRecordCount >= 0 &&
                admsLog.OriginalRecordCount <= RecordCountThreshold15k &&
                minutes >= limits.Limit015K)
            {
                fileStatus.isPassMaxProcessingTime = true;
            }
            else if (admsLog.OriginalRecordCount > RecordCountThreshold15k && 
                     admsLog.OriginalRecordCount <= RecordCountThreshold50k &&
                     minutes >= limits.Limit1550K)
            { 
                fileStatus.isPassMaxProcessingTime = true;
            }
            else if (admsLog.OriginalRecordCount > RecordCountThreshold50k &&
                     admsLog.OriginalRecordCount <= RecordCountThreshold100k &&
                     minutes >= limits.Limit50100K)
            {
                fileStatus.isPassMaxProcessingTime = true;
            }
            else if (admsLog.OriginalRecordCount > RecordCountThreshold100k &&
                     minutes >= limits.Limit100Max)
            {
                fileStatus.isPassMaxProcessingTime = true;
            }
        }

        private IEnumerable<UASEntity.AdmsLog> FillSourceFiles(
            KMBusiness.Client clientWorker, 
            UasBusiness.AdmsLog admsLogWorker, 
            UasBusiness.SourceFile sourceFileWorker,
            out List<UASEntity.SourceFile> sourceFiles)
        {
            Guard.NotNull(admsLogWorker, nameof(admsLogWorker));

            if (CurrentClient.ClientID != CurrentClientID)
            {
                CurrentClient = clientWorker.Select(CurrentClientID);
            }

            if (CurrentClient.Products == null || CurrentClient.Products.Count == 0)
            {
                CurrentClient.Products =
                    KMPlatform.DataAccess.Client.SelectProduct(CurrentClient).ToList();
            }

            var admsLogs = admsLogWorker.SelectNotCompleteNotFailed(CurrentClientID);
            var sourceFileIds = new List<int>();
            admsLogs.ForEach(
                x =>
                {
                    if (!sourceFileIds.Contains(x.SourceFileId))
                    {
                        sourceFileIds.Add(x.SourceFileId);
                    }
                });
            sourceFiles = sourceFileWorker.Select(sourceFileIds);

            return admsLogs;
        }

        public ActionResult GetFileStatus(List<FileStatus> filteredList, string listName)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FSUAD, KMPlatform.Enums.Access.FullAccess) ||
                KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FSC, KMPlatform.Enums.Access.FullAccess))
            {
                //List<FileStatus> lfs = UAS.Web.Models.SampleData.getFileStatus(name);
                List<FileStatus> lfs = new List<FileStatus>();
                lfs = filteredList.OrderByDescending(x => x.isPassMaxProcessingTime).ThenByDescending(x => x.StartTime).ToList();

                UAS.Web.Models.Circulations.FileStatusWithName list = new FileStatusWithName();
                list.FileStatusIEnum = lfs;
                list.FileStatusName = listName;
                return PartialView("_ShowFileStatus", list);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
           
        }        
        #endregion

        #region FileHistory
        public ActionResult FileHistory()
        {
            if (KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.View) ||
                  KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.View)||
                  KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.Download) ||
                  KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.Download))
            {
                if (LoadedClientId != CurrentClientID)
                {
                    fileHistSearch = new FileHistorySearch();
                    LoadedClientId = CurrentClientID;
                    fileHistSearch.Pubs = new List<Publication>();
                    fileHistSearch.FileTypes = new List<FileType>();

               
                   
                    if (KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.Download) || KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.View))
                    {
                        fileHistSearch.isUAD = true;
                    }
                    else
                    {
                        fileHistSearch.isUAD = false;
                    }
                    if (KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.Download) || KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.View))
                    {
                        fileHistSearch.isCirc = true;
                    }
                    else
                    {
                        fileHistSearch.isCirc = false;
                    }
                    #region BusinessLogic
                    KMPlatform.BusinessLogic.Client clientWorker = new KMPlatform.BusinessLogic.Client();
                    FrameworkUAD.BusinessLogic.Product productWorker = new FrameworkUAD.BusinessLogic.Product();
                    FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                    #endregion
                    #region Variables
                    //KMPlatform.Entity.Client client = new KMPlatform.Entity.Client();
                    List<FrameworkUAD.Entity.Product> products = new List<FrameworkUAD.Entity.Product>();
                    List<FrameworkUAD_Lookup.Entity.Code> circFileTypeList = new List<FrameworkUAD_Lookup.Entity.Code>();
                    #endregion
                    #region GetData
                    fileHistSearch.Client = clientWorker.Select(CurrentClientID, false);
                    products = productWorker.Select(fileHistSearch.Client.ClientConnections, false);
                    circFileTypeList = codeWorker.Select(Enums.CodeType.Database_File);
                    #endregion

                    //fhs.Pubs = UAS.Web.Models.SampleData.getPublication();
                    //fhs.FileTypes = UAS.Web.Models.SampleData.getFileTypes();                     

                    fileHistSearch.FileTypeName = "Audience Data";

                    foreach (FrameworkUAD.Entity.Product p in products)
                    {
                        bool isCirc = false;
                        bool.TryParse(p.IsCirc.ToString(), out isCirc);

                        fileHistSearch.Pubs.Add(new Publication(p.PubID, p.PubCode, isCirc));
                    }

                    //will change the DisplayName of Audience Data to Standard
                    //fhs.FileTypes.Add(new FileType(0, "Standard", false));
                    foreach (FrameworkUAD_Lookup.Entity.Code c in circFileTypeList)
                    {
                        if (c.CodeName == "API")
                            fileHistSearch.FileTypes.Add(new FileType(c.CodeId, c.DisplayName, true, "API"));
                        else if (c.CodeName == "Audience Data")
                            fileHistSearch.FileTypes.Add(new FileType(c.CodeId, c.DisplayName, true, "UAD"));
                        else if (c.CodeName == "API")
                            fileHistSearch.FileTypes.Add(new FileType(c.CodeId, c.DisplayName, true, "CIRC"));
                    }

                    fileHistSearch.StartDate = DateTime.Now.AddDays(-7);
                    fileHistSearch.EndDate = DateTime.Now;

                    //fhs.FileHistoryResults = new List<Models.FileHistory>();
                    //fhs.FileHistoryResults = UAS.Web.Models.SampleData.getFileHistory();

                    return View(fileHistSearch);
                }
                else
                    return View(fileHistSearch);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
           
        }

        [HttpPost]
        public ActionResult FileHistory(FileHistorySearch fileHistorySearch)
        {
            if (!KMUser.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPEnums.Access.View)
                && !KMUser.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPEnums.Access.View)
                && !KMUser.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPEnums.Access.Download)
                && !KMUser.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPEnums.Access.Download))
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

            if (fileHistorySearch.RecordSource == null)
            {
                return null;
            }

            try
            {
                SearchedClientId = CurrentClient.ClientID;
                selectedRecordSource = fileHistorySearch.RecordSource;
                startDate = fileHistorySearch.StartDate;
                endDate = fileHistorySearch.EndDate;
                var hasdownloadAccess = false;
                viewFileHistoryList = new List<FileHistory>();

                if (KMUser.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPEnums.Access.Download)
                    || KMUser.HasAccess(
                        CurrentUser,
                        Services.FULFILLMENT,
                        ServiceFeatures.FHC,
                        KMPEnums.Access.Download))
                {
                    hasdownloadAccess = true;
                }

                if (fileHistorySearch.Client == null)
                {
                    fileHistorySearch.Client = CurrentClient;
                }

                AddPublications(fileHistorySearch);

                var fileStatusCodeList = new UADCode().Select(Enums.CodeType.File_Status);

                var recordSource = RecordSource.UAD;
                switch (fileHistorySearch.RecordSource)
                {
                    case "CIRC":
                        recordSource = RecordSource.CIRC;
                        break;
                    case "API":
                        recordSource = RecordSource.API;
                        break;
                }

                var admsLogs = new UasBusiness.AdmsLog().Select(
                    CurrentClientID,
                    recordSource,
                    fileHistorySearch.StartDate,
                    fileHistorySearch.EndDate);
                ProcessAdmsLogs(fileHistorySearch, admsLogs, fileStatusCodeList, hasdownloadAccess);

                FileHistoryPubCodes = viewFileHistoryList.Select(e => e.PubCode)
                    .Distinct()
                    .ToList();
                return PartialView("_FileHistoryResults", viewFileHistoryList);
            }
            catch (Exception exception)
            {
                WriteApplicationLog(exception);
                return null;
            }
        }

        private void AddPublications(FileHistorySearch fileHistorySearch)
        {
            fileHistorySearch.Pubs = new List<Publication>();
            var products = new UadBusiness.Product().Select(CurrentClient.ClientConnections);
            foreach (var product in products)
            {
                if (product.IsCirc == null)
                {
                    product.IsCirc = false;
                }

                fileHistorySearch.Pubs.Add(new Publication(product.PubID, product.PubCode, product.IsCirc.Value));
            }
        }

        private void WriteApplicationLog(Exception exception)
        {
            var worker = new ApplicationLog();
            var accessKey = CurrentUser.AccessKey;
            var app = KMEnums.GetApplication("AMS_Web");
            var logClientId = CurrentClient.ClientID;
            var formatException = FormatException(exception);
            worker.LogCriticalError(accessKey.ToString(), formatException, app, $"{GetType() .Name}.FileHistory", logClientId);
        }

        private void ProcessAdmsLogs(
            FileHistorySearch fileHistorySearch,
            IReadOnlyCollection<AdmsLog> admsLogs,
            IReadOnlyCollection<Code> fileStatusCodeList,
            bool hasdownloadAccess)
        {
            if (admsLogs == null || admsLogs.Count <= 0)
            {
                return;
            }

            var sourceFileIds = new List<int>();
            foreach (var admsLog in admsLogs)
            {
                if (!sourceFileIds.Contains(admsLog.SourceFileId))
                {
                    sourceFileIds.Add(admsLog.SourceFileId);
                }
            }

            var sourceFiles = new UADSourceFile().Select(sourceFileIds);

            foreach (var admsLog in admsLogs)
            {
                AddToFileHistoryList(fileHistorySearch, fileStatusCodeList, hasdownloadAccess, sourceFiles, admsLog);
            }

            // UAD files don't have a PubID associated with the file, only use PubID when Circ
            if (fileHistorySearch.isCirc && fileHistorySearch.PubID > 0)
            {
                viewFileHistoryList = viewFileHistoryList.FindAll(x => x.PubID.Equals(fileHistorySearch.PubID));
            }

            // UAD files are Standard which is set to filetypeid = 0, only use for Circ
            if (fileHistorySearch.FileTypeID > 0)
            {
                viewFileHistoryList = viewFileHistoryList.FindAll(x => x.FileTypeID.Equals(fileHistorySearch.FileTypeID));
            }
        }

        private void AddToFileHistoryList(
            FileHistorySearch fileHistorySearch,
            IEnumerable<Code> fileStatusCodeList,
            bool hasdownloadAccess,
            IEnumerable<SourceFile> sourceFiles,
            AdmsLog admsLog)
        {
            var sourceFile = sourceFiles.FirstOrDefault(file => file.SourceFileID == admsLog.SourceFileId);

            var fileNameValid = !string.IsNullOrWhiteSpace(fileHistorySearch.FileName);
           
            if (sourceFile == null || fileNameValid && !sourceFile.FileName.StartsWith(fileHistorySearch.FileName, StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }

            var pubCode = "N/A";
            var pubId = 0;
            if (fileHistorySearch.PubID > 0)
            {
                pubId = fileHistorySearch.PubID;
                if (fileHistorySearch.Pubs?.Count > 0
                    && fileHistorySearch.Pubs.Exists(publication => publication.PubID == pubId))
                {
                    pubCode = fileHistorySearch.Pubs.First(publication => publication.PubID == fileHistorySearch.PubID)
                        .Pubcode;
                }
            }
            else
            {
                if (fileHistorySearch.Pubs != null && fileHistorySearch.Pubs.Count > 0 && sourceFile.PublicationID.HasValue)
                {
                    if (fileHistorySearch.Pubs.Exists(publication => publication.PubID == sourceFile.PublicationID.Value))
                    {
                        var pubcode = fileHistorySearch.Pubs
                            .First(publication => publication.PubID == sourceFile.PublicationID.Value)
                            .Pubcode;
                        pubCode = pubcode;
                        pubId = sourceFile.PublicationID.Value;
                    }
                }
            }

            var fileHistory = CreateFileHistory(hasdownloadAccess, admsLog, pubId, pubCode, sourceFile, fileStatusCodeList);
            viewFileHistoryList.Add(fileHistory);
        }

        private FileHistory CreateFileHistory(
            bool hasdownloadAccess,
            AdmsLog admsLog,
            int pubId,
            string pubCode,
            SourceFileBase sourceFile,
            IEnumerable<Code> fileStatusCodeList)
        {
            var fileTypeId = sourceFile.DatabaseFileTypeId;
            var fileType = admsLog.RecordSource;

            var fileCodeStatus = fileStatusCodeList.FirstOrDefault(statusCode => statusCode.CodeId == admsLog.FileStatusId);
            var status = string.Empty;
            if (fileCodeStatus != null)
            {
                status = fileCodeStatus.CodeName;
            }

            var failureReason = admsLog.StatusMessage.Contains("Rejecting file due to error threshold")
                                    ? "Error Threshold Met."
                                    : "Engine Inactivity.";

            var fileHistory = new FileHistory(
                CurrentClient.ClientID,
                admsLog.ProcessCode,
                string.Empty,
                pubId,
                pubCode,
                admsLog.FileNameExact,
                fileTypeId,
                fileType,
                admsLog.FileStart,
                status,
                failureReason,
                admsLog.OriginalRecordCount,
                admsLog.FailedRecordCount,
                admsLog.TransformedRecordCount,
                admsLog.DuplicateRecordCount,
                admsLog.IgnoredRecordCount,
                admsLog.FinalRecordCount,
                admsLog.DimensionProfileCount,
                admsLog.DimensionRecordCount,
                hasdownloadAccess);

            return fileHistory;
        }

        public ActionResult SingleFileHistory(int SourceFileID)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.View) ||
                  KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.View) ||
                  KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.Download) ||
                  KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.Download))
            {
                //endDate startDate recordSource
                if (SourceFileID > 0)
                {
                    SearchedClientId = CurrentClient.ClientID;
                    bool hasdownloadAccess = false;
                    viewFileHistoryList = new List<FileHistory>();
                    if (KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.Download) || KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.Download))
                    {
                        hasdownloadAccess = true;
                    }
                    else
                    {
                        hasdownloadAccess = false;
                    }

                    KMPlatform.BusinessLogic.Client clientWorker = new KMPlatform.BusinessLogic.Client();
                    FrameworkUAS.BusinessLogic.AdmsLog admsLogWorker = new FrameworkUAS.BusinessLogic.AdmsLog();
                    FrameworkUAS.BusinessLogic.SourceFile sourceFileWorker = new FrameworkUAS.BusinessLogic.SourceFile();
                    FrameworkUAD.BusinessLogic.Product productWorker = new FrameworkUAD.BusinessLogic.Product();
                    FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                    KMPlatform.BusinessLogic.Service servData = new KMPlatform.BusinessLogic.Service();
                    KMPlatform.BusinessLogic.Service serviceWorker = new KMPlatform.BusinessLogic.Service();
                    KMPlatform.BusinessLogic.ServiceFeature serviceFeatureWorker = new KMPlatform.BusinessLogic.ServiceFeature();

                    List<FrameworkUAS.Entity.AdmsLog> admsLogs = new List<FrameworkUAS.Entity.AdmsLog>();
                    List<FrameworkUAD_Lookup.Entity.Code> fileStatusCodeList = new List<FrameworkUAD_Lookup.Entity.Code>();
                   
                    FrameworkUAD_Lookup.Entity.Code fileStatusCompleted = new FrameworkUAD_Lookup.Entity.Code();
                    FrameworkUAD_Lookup.Entity.Code fileStatusFailed = new FrameworkUAD_Lookup.Entity.Code();
                    List<Publication> Pubs = new List<Publication>();

                    List<KMPlatform.Entity.Service> serviceList = servData.Select();
                    List<FrameworkUAD.Entity.Product> products = productWorker.Select(CurrentClient.ClientConnections, false);
                    products.ForEach(x =>
                    {
                        if (x.IsCirc == null) x.IsCirc = false;
                        Pubs.Add(new Publication(x.PubID, x.PubCode, x.IsCirc.Value));
                    });                    

                    fileStatusCodeList = codeWorker.Select(Enums.CodeType.File_Status);
                    fileStatusCompleted = fileStatusCodeList.FirstOrDefault(x => x.CodeName == FileStatusType.Completed.ToString());
                    fileStatusFailed = fileStatusCodeList.FirstOrDefault(x => x.CodeName == FileStatusType.Failed.ToString());

                    admsLogs = admsLogWorker.Select(CurrentClient.ClientID, SourceFileID);
                    List<int> sourceFileIds = new List<int>();
                    admsLogs.ForEach(x => { if (!sourceFileIds.Contains(x.SourceFileId)) sourceFileIds.Add(x.SourceFileId); });
                    List<FrameworkUAS.Entity.SourceFile> sourceFiles = sourceFileWorker.Select(sourceFileIds, false);

                    foreach (FrameworkUAS.Entity.AdmsLog al in admsLogs)
                    {
                        FrameworkUAS.Entity.SourceFile sf = sourceFiles.FirstOrDefault(x => x.SourceFileID == al.SourceFileId);                        
                        string pubCode = "N/A";
                        int pubID = 0;                            
                        if (Pubs.Exists(x => x.PubID == sf.PublicationID.Value))
                        {
                            string sfPubCode = Pubs.FirstOrDefault(x => x.PubID == sf.PublicationID.Value).Pubcode;
                            pubCode = sfPubCode;
                            pubID = sf.PublicationID.Value;
                        }                            

                        //FrameworkUAD_Lookup.Entity.Code c1 = codeList.FirstOrDefault(x => x.CodeId == sf.DatabaseFileTypeId);
                        int fileTypeID = sf.DatabaseFileTypeId;
                        string fileType = al.RecordSource;//c1.CodeName;

                        FrameworkUAD_Lookup.Entity.Code c2 = fileStatusCodeList.FirstOrDefault(x => x.CodeId == al.FileStatusId);//codeList.FirstOrDefault(x => x.CodeId == al.FileStatusId);
                        string status = "";
                        if (c2 != null)
                            status = c2.CodeName;

                        string failureReason = "";
                        if (al.StatusMessage.Contains("Rejecting file due to error threshold"))
                            failureReason = "Error Threshold Met.";
                        else
                            failureReason = "Engine Inactivity.";

                        FileHistory fh = new FileHistory(CurrentClient.ClientID, al.ProcessCode, "", pubID, pubCode, al.FileNameExact, fileTypeID, fileType, al.FileStart, status, failureReason,
                            al.OriginalRecordCount, al.FailedRecordCount, al.TransformedRecordCount, al.DuplicateRecordCount, al.IgnoredRecordCount, al.FinalRecordCount, al.DimensionProfileCount, al.DimensionRecordCount, hasdownloadAccess);

                        viewFileHistoryList.Add(fh);                        
                    }

                    FileHistoryPubCodes = viewFileHistoryList.Select(e => e.PubCode).Distinct().ToList();

                    return PartialView("_FileHistoryResults", viewFileHistoryList);
                }
                else
                    return null;
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

        }

        public ActionResult ResultsDownload(string processcode, string type, int clientid)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.Download) ||
                 KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.Download))
            {

                #region Variables
            string file = "";
            string remoteFile = "";
            string remotePath = "";
            byte[] fileBytes = new byte[] { };
            string contentType = "";
            string localPath = Server.MapPath("~/App_Data") + "\\" + clientid;
            #endregion

            try
            {                
                #region Create Path
                try
                {
                    if (!System.IO.Directory.Exists(localPath))
                    {
                        System.IO.Directory.CreateDirectory(localPath);
                    }
                }
                catch (Exception ex)
                {
                    //Report error message
                    TempData["FileHistoryMessage"] = "An exception occurred while attempting to retrieve the file.";
                    return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
                }
                #endregion
                #region Get File For Download
                string host = @"ftp://ftp.knowledgemarketing.com";
                string user = "adms";
                string pass = "Team_KM";

                Core_AMS.Utilities.FtpFunctions ftp = new Core_AMS.Utilities.FtpFunctions(host, user, pass);
                               
                string[] checkList = new string[] { };
                
                string cleanProcessCode = CleanProcessCodeForFileName(processcode);

                string clientName = "";
                if (AdminClientList.Count(x => x.ClientID == clientid) > 0)
                    clientName = AdminClientList.SingleOrDefault(x => x.ClientID == clientid).FtpFolder;

                string FTPPath = "/Client Archive/" + clientName + "/";

                string ftpProcessedPath = FTPPath + "Processed";
                string ftpInvalidPath = FTPPath + "Invalid";
                string ftpReportsPath = FTPPath + "Reports";

                switch (type)
                {
                    #region Number of Records
                    case "all":
                        //First Check for the file in the Processed Path. If not there possibly in Invalid
                        checkList = ftp.DirectoryListSimple(ftpProcessedPath);
                        if (checkList.ToList().Any(x => x.Contains(cleanProcessCode)))
                        {
                            for (int i = 0; i < checkList.Length; i++)
                            {
                                if (checkList[i].Contains(cleanProcessCode))
                                {
                                    file = checkList[i];
                                    remoteFile = ftpProcessedPath + "/" + file;
                                    break;
                                }
                            }                            
                        }
                        else
                        {
                            checkList = ftp.DirectoryListSimple(ftpInvalidPath);
                            if (checkList.ToList().Any(x => x.Contains(cleanProcessCode)))
                            {
                                for (int i = 0; i < checkList.Length; i++)
                                {
                                    if (checkList[i].Contains(cleanProcessCode))
                                    {
                                        file = checkList[i];
                                        remoteFile = ftpInvalidPath + "/" + file;
                                        break;
                                    }
                                }                                
                            }
                        }
                        break;
                    #endregion
                    #region Invalid
                    case "invalid":
                        checkList = ftp.DirectoryListSimple(ftpReportsPath);                        
                        if (checkList.ToList().Any(x => x.Contains(cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._InvalidReport.ToString())))
                        {
                            for (int i = 0; i < checkList.Length; i++)
                            {
                                if (checkList[i].Contains(cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._InvalidReport.ToString()))
                                {
                                    file = checkList[i];
                                    remoteFile = ftpReportsPath + "/" + file;
                                    break;
                                }
                            }                            
                        }
                        break;
                    #endregion
                    #region Transformed
                    case "transformed":
                        checkList = ftp.DirectoryListSimple(ftpReportsPath);                        
                        if (checkList.ToList().Any(x => x.Contains(cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._TransformedReport.ToString())))
                        {
                            for (int i = 0; i < checkList.Length; i++)
                            {
                                if (checkList[i].Contains(cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._TransformedReport.ToString()))
                                {
                                    file = checkList[i];
                                    remoteFile = ftpReportsPath + "/" + file;
                                    break;
                                }
                            }                            
                        }
                        break;
                    #endregion
                    #region Duplicates
                    case "duplicates":
                        checkList = ftp.DirectoryListSimple(ftpReportsPath);                        
                        if (checkList.ToList().Any(x => x.Contains(cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._DuplicatesReport.ToString())))
                        {
                            for (int i = 0; i < checkList.Length; i++)
                            {
                                if (checkList[i].Contains(cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._DuplicatesReport.ToString()))
                                {
                                    file = checkList[i];
                                    remoteFile = ftpReportsPath + "/" + file;
                                    break;
                                }
                            }                            
                        }
                        break;
                    #endregion
                    #region Ignored
                    case "ignored":
                        checkList = ftp.DirectoryListSimple(ftpReportsPath);                        
                        if (checkList.ToList().Any(x => x.Contains(cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._IgnoredReport.ToString())))
                        {
                            for (int i = 0; i < checkList.Length; i++)
                            {
                                if (checkList[i].Contains(cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._IgnoredReport.ToString()))
                                {
                                    file = checkList[i];
                                    remoteFile = ftpReportsPath + "/" + file;
                                    break;
                                }
                            }                            
                        }
                        break;
                    #endregion
                    #region Processed
                    case "processed":
                        checkList = ftp.DirectoryListSimple(ftpReportsPath);                        
                        if (checkList.ToList().Any(x => x.Contains(cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._ProcessedReport.ToString())))
                        {
                            for (int i = 0; i < checkList.Length; i++)
                            {
                                if (checkList[i].Contains(cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._ProcessedReport.ToString()))
                                {
                                    file = checkList[i];
                                    remoteFile = ftpReportsPath + "/" + file;
                                    break;
                                }
                            }                            
                        }
                        break;
                    #endregion
                    #region Dimension Summary
                    case "dimensionsummary":
                        checkList = ftp.DirectoryListSimple(ftpReportsPath);                        
                        if (checkList.ToList().Any(x => x.Contains(cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._DimensionErrorsSummaryReport.ToString())))
                        {
                            for (int i = 0; i < checkList.Length; i++)
                            {
                                if (checkList[i].Contains(cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._DimensionErrorsSummaryReport.ToString()))
                                {
                                    file = checkList[i];
                                    remoteFile = ftpReportsPath + "/" + file;
                                    break;
                                }
                            }                            
                        }
                        break;
                    #endregion
                    #region Dimension Subscriber
                    case "dimensionsubscriber":
                        checkList = ftp.DirectoryListSimple(ftpReportsPath);                        
                        if (checkList.ToList().Any(x => x.Contains(cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._DimensionSubscriber.ToString())))
                        {
                            for (int i = 0; i < checkList.Length; i++)
                            {
                                if (checkList[i].Contains(cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._DimensionSubscriber.ToString()))
                                {
                                    file = checkList[i];
                                    remoteFile = ftpReportsPath + "/" + file;
                                    break;
                                }
                            }                            
                        }
                        break;
                    #endregion                        
                }
                #endregion
                #region Download from FTP
                if (!string.IsNullOrEmpty(remoteFile))
                {
                    //Download then create bytes, content type for return File below. This allows to delete the file downloaded to the server to prevent clutter.
                    ftp.Download(remoteFile, localPath + "/" + file, false);
                    fileBytes = System.IO.File.ReadAllBytes(localPath + "/" + file);
                    contentType = MimeMapping.GetMimeMapping(localPath + "/" + file);
                    #region Delete Local File
                    System.IO.File.Delete(localPath + "/" + file);
                    #endregion
                }
                #endregion                
            }
            catch (Exception ex)
            {
                //Report error message
                TempData["FileHistoryMessage"] = "An exception occurred while attempting to retrieve the file.";
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }

            //if true this means the file wasn't available or couldn't be downloaded. Temp error will be recorded and redirect to display.
            bool hasAllZeroes = fileBytes.All(singleByte => singleByte == 0);
            if (hasAllZeroes)
            {
                    double totaldaysdiff = 0;
                    int mm, dd, yyyy;
                    int.TryParse(processcode.Split('_')[1].Substring(0, 2), out mm);
                    int.TryParse(processcode.Split('_')[1].Substring(2, 2), out dd);
                    int.TryParse(processcode.Split('_')[1].Substring(4, 4), out yyyy);
                    totaldaysdiff = (DateTime.Now - new DateTime(yyyy, mm, dd)).TotalDays;
                    if (totaldaysdiff > 14)
                    {
                        TempData["FileHistoryMessage"] = "File results are available for 2 weeks from the file processed date. You are trying to download file more than 2 weeks older. You are trying to download file that is over 14 days old.";
                    }
                    else
                    {
                        TempData["FileHistoryMessage"] = "File results are available for 2 weeks from the file processed date. File may be removed or deleted from the FTP location if over than 14 Days. If you feel you are getting this error message in error, please contact your account manager.";
                    }
                    return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);                
            }
            else
            {
                //Read somewhere that this might be needed? Chrome it is not.
                //var cd = new System.Net.Mime.ContentDisposition
                //{
                //    FileName = file,
                //    Inline = true,
                //};
                //Response.AppendHeader("Content-Disposition", cd.ToString());
                return File(fileBytes, contentType, file);
            }
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        public ActionResult FileHistory_Filter_PubCode(List<FileHistory> fhList)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.View) ||
                KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.View) ||
                KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.Download) ||
                KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.Download))
            {
                if (fhList == null)
                {
                    return Json(FileHistoryPubCodes, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(fhList.Select(e => e.PubCode).Distinct(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        public ActionResult FileHistory_Filter_FileType(List<FileHistory> fhList)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.View) ||
               KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.View) ||
               KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.Download) ||
               KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.Download))
            {
                if (fhList == null)
                {
                    return Json(FileHistoryFileTypes, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(fhList.Select(e => e.FileType).Distinct(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

        }

        public ActionResult ProcessedRecords(int ClientID, string ProcessCode, int FinalProcessCount)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.View) ||
               KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.View) ||
               KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.Download) ||
               KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.Download))
            {
                FileHistory fh = new FileHistory(ClientID, ProcessCode, FinalProcessCount);
            return PartialView("_FileHistoryProcessedRecords", fh);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        public ActionResult DimensionErrors(int ClientID, string ProcessCode, int DimensionProfileCount, int DimensionRecordCount)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.View) ||
               KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.View) ||
               KM.Platform.User.HasAccess(CurrentUser, Services.UAD, ServiceFeatures.FHUAD, KMPlatform.Enums.Access.Download) ||
               KM.Platform.User.HasAccess(CurrentUser, Services.FULFILLMENT, ServiceFeatures.FHC, KMPlatform.Enums.Access.Download))
            {
                FileHistory fh = new FileHistory(ClientID, ProcessCode, DimensionProfileCount, DimensionRecordCount);
                return PartialView("_FileHistoryDimensionErrors", fh);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        #endregion

        private int CurrentClientID
        {
            //get { return 25; }
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID; }
        }

        private List<KMPlatform.Entity.Client> AdminClientList
        {
            get
            {
                if (Session["BaseControlller_ClientList"] == null)
                {
                    KMPlatform.BusinessLogic.Client cWrk = new KMPlatform.BusinessLogic.Client();
                    Session["BaseControlller_ClientList"] = cWrk.SelectActiveForClientGroupLite(CurrentClientGroupID).OrderBy(x => x.ClientName).ToList();
                }

                return (List<KMPlatform.Entity.Client>) Session["BaseControlller_ClientList"];
            }
        }       
    }
}