using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core_AMS.Utilities;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using UAS.Web.Models.FileMapperWizard;
using CommonEnums = KM.Common.Enums;
using Enums = FrameworkUAD_Lookup.Enums;

namespace UAS.Web.Controllers.FileMapperWizard
{
    ///<summary>
    ///using a 1 base step index to match tabstrip control
    /// 
    /// 1 _step1 Setup 
    /// 2 _step2 ColumnMapping
    /// 3 _step3 Rules - ADMS | Custom
    /// 4 _step4 Review
    ///</summary>
    public class FileMapperWizardController : Common.BaseController
    {
        private const string Whitespace = " ";
        private const string WhiteSpaceEscaped = "_";
        private const string AllProductsPubCode = "ALL PRODUCTS";
        private const string Comma = ",";
        private const char CommaDelimiter = ',';
        private const string PartialTransformationChangeValueView = "Partials/Transformation/_transformationChangeValue";
        private const string PartialTransformationAssignValueView = "Partials/Transformation/_transformationAssignValue";
        private const string PartialTransformationJoinColumnsView = "Partials/Transformation/_transformationJoinColumns";
        private const string PartialTransformationSplitIntoRowsView = "Partials/Transformation/_transformationSplitIntoRows";
        private const string DefaultProductId = "-1";
        private const string JoinTransformationError = "<li>Error saving Join Transformation.</li>";
        private const string NoValueToAssignError = "<li>Value To Assign must be entered or a product must be selected.</li>";
        private const string DuplicateRowError = "<li>Row {0}: Row has duplicate product used - {1}.</li>";
        private const string DuplicateRowAllProductsError = "<li>Row {0}: Row has duplicate product used - ALL PRODUCTS.</li>";
        private const char QuoteChar = '"';
        private const string SlashChar = "\\";
        private const string ZeroString = "0";

        private Models.FileMapperWizard.FileMapperWizardViewModel fmwModel
        {
            get
            {
                if (Session["fmwModel"] == null)
                    return new Models.FileMapperWizard.FileMapperWizardViewModel();
                else
                    return (Models.FileMapperWizard.FileMapperWizardViewModel)Session["fmwModel"];
            }
            set
            {
                Session["fmwModel"] = value;
            }
        }

        private List<string> TransformationSearchTransformationNames
        {
            get
            {
                if (Session["TransformationSearchTransformationNames"] == null)
                {
                    FrameworkUAS.BusinessLogic.Transformation transformationWorker = new FrameworkUAS.BusinessLogic.Transformation();
                    List<FrameworkUAS.Entity.Transformation> transformations = new List<FrameworkUAS.Entity.Transformation>();
                    transformations = transformationWorker.SelectClient(CurrentClient.ClientID, false);

                    List<string> names = transformations.Select(e => e.TransformationName).Distinct().ToList();
                    Session["TransformationSearchTransformationNames"] = names;
                    return names;
                }
                else
                    return (List<string>)Session["TransformationSearchTransformationNames"];
            }
            set
            {
                Session["TransformationSearchTransformationNames"] = value;
            }
        }
        private List<string> TransformationSearchTransformationDescs
        {
            get
            {
                if (Session["TransformationSearchTransformationDescs"] == null)
                {
                    FrameworkUAS.BusinessLogic.Transformation transformationWorker = new FrameworkUAS.BusinessLogic.Transformation();
                    List<FrameworkUAS.Entity.Transformation> transformations = new List<FrameworkUAS.Entity.Transformation>();
                    transformations = transformationWorker.SelectClient(CurrentClient.ClientID, false);

                    List<string> descs = transformations.Select(e => e.TransformationDescription).Distinct().ToList();
                    Session["TransformationSearchTransformationDescs"] = descs;
                    return descs;
                }
                else
                    return (List<string>)Session["TransformationSearchTransformationDescs"];
            }
            set
            {
                Session["TransformationSearchTransformationDescs"] = value;
            }
        }


        public FileMapperWizardController() { }

        public void intializeViewBag()
        {
            ViewBag.SetupSuccess = false;
            ViewBag.MappingSuccess = false;
            ViewBag.RulesSuccess = false;
            ViewBag.ReviewSuccess = false;
            ViewBag.MessageId = Guid.NewGuid().ToString();
            ViewBag.CurrentStep = 0;
        }
        private void SetStep(int step)
        {
            ViewBag.CurrentStep = step;
            fmwModel.currentStep = step;
        }
        public ActionResult Index(string Type)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FILEVALUAD, KMPlatform.Enums.Access.FullAccess) ||
                 KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.FV, KMPlatform.Enums.Access.FullAccess))
            {
                try
                {
                    intializeViewBag();


                    #region BusinessLogic
                    KMPlatform.BusinessLogic.Client clientWorker = new KMPlatform.BusinessLogic.Client();
                    FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                    #endregion
                    #region Variables
                    List<FrameworkUAD_Lookup.Entity.Code> processingStatusCodeList = new List<FrameworkUAD_Lookup.Entity.Code>();
                    #endregion
                    #region Get Data
                    if (CurrentClient.ClientID != CurrentClientID)
                        CurrentClient = clientWorker.Select(CurrentClientID, false);
                    processingStatusCodeList = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Processing_Status);
                    #endregion
                    bool isCirc = false;
                    if (Type.Equals(FrameworkUAS.BusinessLogic.Enums.RecordSource.CIRC.ToString(), StringComparison.CurrentCultureIgnoreCase))
                        isCirc = true;

                    fmwModel = GetMainModel(isCirc);
                    fmwModel.client = CurrentClient;
                    SetStep(1);
                    fmwModel.setupViewModel.isCirc = isCirc;
                    fmwModel.setupViewModel.IsNewFile = true;
                }
                catch (Exception ex)
                {
                    string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                }

                return View(fmwModel);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        public ActionResult IndexEdit(int ID)
        {
            try
            {
                intializeViewBag();

                #region BusinessLogic
                KMPlatform.BusinessLogic.Client clientWorker = new KMPlatform.BusinessLogic.Client();
                FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                FrameworkUAS.BusinessLogic.SourceFile sourceFileWorker = new FrameworkUAS.BusinessLogic.SourceFile();
                #endregion
                #region Variables
                List<FrameworkUAD_Lookup.Entity.Code> processingStatusCodeList = new List<FrameworkUAD_Lookup.Entity.Code>();
                #endregion
                #region Get Data
                if (CurrentClient.ClientID != CurrentClientID)
                    CurrentClient = clientWorker.Select(CurrentClientID, false);
                processingStatusCodeList = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Processing_Status);
                FrameworkUAS.Entity.SourceFile sourceFile = sourceFileWorker.SelectSourceFileID(ID, false);

                KMPlatform.BusinessLogic.Service serviceWorker = new KMPlatform.BusinessLogic.Service();
                KMPlatform.Entity.Service service = serviceWorker.Select(sourceFile.ServiceID, false);
                bool isCirc = false;
                if (service.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    isCirc = true;
                #endregion                                

                fmwModel = GetMainModel(isCirc);
                #region Set SetupViewModel to SourceFile values for Edit            
                fmwModel.sourceFileId = sourceFile.SourceFileID;
                fmwModel.setupViewModel.isCirc = isCirc;
                fmwModel.setupViewModel.ClientId = sourceFile.ClientID;
                fmwModel.setupViewModel.SourceFileId = sourceFile.SourceFileID;
                fmwModel.setupViewModel.ServiceID = sourceFile.ServiceID;
                fmwModel.setupViewModel.ServiceFeatureID = sourceFile.ServiceFeatureID;
                fmwModel.setupViewModel.FileSaveAsName = sourceFile.FileName;
                fmwModel.setupViewModel.IsFullFile = sourceFile.IsFullFile;
                fmwModel.setupViewModel.Delimeter = sourceFile.Delimiter;
                fmwModel.setupViewModel.HasQuotation = sourceFile.IsTextQualifier;
                fmwModel.setupViewModel.QDateFormat = sourceFile.QDateFormat;
                fmwModel.setupViewModel.DataFile = null;
                fmwModel.setupViewModel.Matching = "Default";
                fmwModel.setupViewModel.IsNewFile = false;
                fmwModel.setupViewModel.Extension = sourceFile.Extension;
                if (sourceFile.PublicationID > 0)
                {
                    int pubid = 0;
                    int.TryParse(sourceFile.PublicationID.ToString(), out pubid);
                    fmwModel.setupViewModel.PublicationID = pubid;
                }
                fmwModel.setupViewModel.DatabaseFileTypeID = sourceFile.DatabaseFileTypeId;
                #endregion
            }
            catch (Exception ex)
            {
                string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            }
            return View("Index", fmwModel);
        }

        public ActionResult ReloadSetup()
        {
            return PartialView("Partials/_setup", fmwModel.setupViewModel);
        }

        #region File Management Beginning Grid
        public ActionResult FileManagement()
        {
            return View();
        }
        public ActionResult CurrentMappings(UAS.Web.Models.FileMapperWizard.FileManagementSearchModel model)
        {
            if (model.type == null)
                model.type = "";

            List<UAS.Web.Models.FileMapperWizard.CurrentMappingModel> cmmList = new List<Models.FileMapperWizard.CurrentMappingModel>();
            UAS.Web.Models.FileMapperWizard.CurrentMappingModel cmm = new Models.FileMapperWizard.CurrentMappingModel();
            FrameworkUAS.BusinessLogic.SourceFile sfWorker = new FrameworkUAS.BusinessLogic.SourceFile();
            List<FrameworkUAS.Entity.SourceFile> sources = new List<FrameworkUAS.Entity.SourceFile>();
            sources = sfWorker.Select(CurrentClient.ClientID, false).Where(x => x.IsDeleted == false).ToList();
            KMPlatform.BusinessLogic.Service servWorker = new KMPlatform.BusinessLogic.Service();
            List<KMPlatform.Entity.Service> services = servWorker.Select(true);
            KMPlatform.Entity.Service uadFileMapperService = services.SingleOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.UADFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
            KMPlatform.Entity.Service circFileMapperService = services.SingleOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
            if (model.type.Equals(FrameworkUAS.BusinessLogic.Enums.RecordSource.CIRC.ToString(), StringComparison.CurrentCultureIgnoreCase))
                sources = sources.Where(x => x.ServiceID == circFileMapperService.ServiceID).ToList();
            else if (model.type.Equals(FrameworkUAS.BusinessLogic.Enums.RecordSource.UAD.ToString(), StringComparison.CurrentCultureIgnoreCase))
                sources = sources.Where(x => x.ServiceID == uadFileMapperService.ServiceID).ToList();

            if (!string.IsNullOrEmpty(model.fileName))
                sources = sources.Where(x => x.FileName.Contains(model.fileName)).ToList();

            if (model.type.Equals(FrameworkUAS.BusinessLogic.Enums.RecordSource.CIRC.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                if (model.pubID > 0)
                    sources = sources.Where(x => x.PublicationID == model.pubID).ToList();

                if (model.fileType > 0)
                    sources = sources.Where(x => x.DatabaseFileTypeId == model.fileType).ToList();

            }
            else if (model.type.Equals(FrameworkUAS.BusinessLogic.Enums.RecordSource.UAD.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                if (model.fileType > 0)
                    sources = sources.Where(x => x.ServiceFeatureID == model.fileType).ToList();

            }
            KMPlatform.BusinessLogic.ServiceFeature servFeatWorker = new KMPlatform.BusinessLogic.ServiceFeature();
            List<KMPlatform.Entity.ServiceFeature> serviceFeatures = servFeatWorker.Select();
            FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> fileTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Database_File).ToList();
            KMPlatform.BusinessLogic.User userWorker = new KMPlatform.BusinessLogic.User();
            List<KMPlatform.Entity.User> users = userWorker.Select(false);
            FrameworkUAD.BusinessLogic.Product prodWorker = new FrameworkUAD.BusinessLogic.Product();
            List<FrameworkUAD.Entity.Product> products = prodWorker.Select(CurrentClient.ClientConnections);

            foreach (FrameworkUAS.Entity.SourceFile sf in sources)
            {
                string fileTypeName = "";
                string createdBy = "";
                string updatedBy = "";
                string pubcode = "N/A";
                bool isCirc = false;
                if (sf.ServiceID.Equals(circFileMapperService.ServiceID))
                    isCirc = true;

                if (isCirc)
                {
                    fileTypeName = fileTypes.FirstOrDefault(x => x.CodeId == sf.DatabaseFileTypeId).CodeName;
                    if (sf.PublicationID != null && products.Count(x => x.PubID == sf.PublicationID) > 0)
                        pubcode = products.FirstOrDefault(x => x.PubID == sf.PublicationID).PubCode;
                }
                else
                {
                    if (serviceFeatures.Count(x => x.ServiceFeatureID == sf.ServiceFeatureID) > 0)
                        fileTypeName = serviceFeatures.FirstOrDefault(x => x.ServiceFeatureID == sf.ServiceFeatureID).SFName;

                }

                //User may not exist, example user received a new user and old was removed/deleted
                if (users.Count(x => x.UserID == sf.CreatedByUserID) > 0)
                    createdBy = users.FirstOrDefault(x => x.UserID == sf.CreatedByUserID).UserName;
                if (sf.UpdatedByUserID != null && users.Count(x => x.UserID == sf.UpdatedByUserID) > 0)
                    updatedBy = users.FirstOrDefault(x => x.UserID == sf.UpdatedByUserID).UserName;

                cmmList.Add(new UAS.Web.Models.FileMapperWizard.CurrentMappingModel(0, isCirc, sf.SourceFileID, sf.DatabaseFileTypeId, fileTypeName, sf.FileName, sf.IsDeleted, sf.Extension, sf.Delimiter, sf.IsTextQualifier, sf.ServiceID, sf.ServiceFeatureID, sf.DateCreated, sf.CreatedByUserID, createdBy, sf.PublicationID, pubcode, sf.DateUpdated, sf.UpdatedByUserID, updatedBy));
            }
            return PartialView("_EditFileSearchResults", cmmList);
        }
        public ActionResult DeleteFile(int sourceFileID)
        {
            FrameworkUAS.BusinessLogic.SourceFile sourceFileWorker = new FrameworkUAS.BusinessLogic.SourceFile();

            int returnValue = 0;
            returnValue = sourceFileWorker.Delete(sourceFileID, true, CurrentUser.UserID);
            bool pass = false;
            if (returnValue == sourceFileID)
                pass = true;

            return Json(pass, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CopyNewFileMapping(int sourceFileID)
        {
            int oldSourceFileID = sourceFileID;
            int newSourceFileID = 0;
            string clientMessage = "";
            bool isError = false;
            if (oldSourceFileID > 0)
            {
                FrameworkUAS.BusinessLogic.SourceFile sourceFileWorker = new FrameworkUAS.BusinessLogic.SourceFile();

                if (!isError)
                {
                    #region SourceFile
                    FrameworkUAS.Entity.SourceFile newSource = new FrameworkUAS.Entity.SourceFile();
                    newSource = sourceFileWorker.SelectSourceFileID(oldSourceFileID, false);
                    newSource.SourceFileID = 0;
                    newSource.FileName = newSource.FileName + "_Copy";
                    newSource.CreatedByUserID = CurrentUser.UserID;
                    newSource.UpdatedByUserID = CurrentUser.UserID;
                    newSource.DateCreated = DateTime.Now;
                    newSource.DateUpdated = DateTime.Now;
                    newSource.IsDeleted = true;

                    newSourceFileID = 0;
                    newSourceFileID = sourceFileWorker.Save(newSource);
                    #endregion

                    if (newSourceFileID > 0)
                    {
                        List<FrameworkUAS.Entity.TransformationFieldMap> allTransFieldMaps = new List<FrameworkUAS.Entity.TransformationFieldMap>();
                        FrameworkUAS.BusinessLogic.TransformationFieldMap transFieldMapWorker = new FrameworkUAS.BusinessLogic.TransformationFieldMap();
                        allTransFieldMaps = transFieldMapWorker.Select().Where(x => x.SourceFileID == oldSourceFileID).ToList();

                        #region FieldMapping
                        List<FrameworkUAS.Entity.FieldMapping> allFieldMaps = new List<FrameworkUAS.Entity.FieldMapping>();
                        FrameworkUAS.BusinessLogic.FieldMapping fieldWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
                        allFieldMaps = fieldWorker.Select(oldSourceFileID);
                        foreach (FrameworkUAS.Entity.FieldMapping thisFieldMap in allFieldMaps)
                        {
                            int oldFileMappingID = thisFieldMap.FieldMappingID;
                            thisFieldMap.FieldMappingID = 0;
                            thisFieldMap.SourceFileID = newSourceFileID;
                            thisFieldMap.CreatedByUserID = CurrentUser.UserID;
                            thisFieldMap.UpdatedByUserID = CurrentUser.UserID;
                            thisFieldMap.DateCreated = DateTime.Now;
                            thisFieldMap.DateUpdated = DateTime.Now;

                            int newFileMappingID = 0;
                            newFileMappingID = fieldWorker.Save(thisFieldMap);

                            #region FieldMultiMap
                            if (thisFieldMap.HasMultiMapping && newFileMappingID > 0)
                            {
                                List<FrameworkUAS.Entity.FieldMultiMap> allFieldMultiMaps = new List<FrameworkUAS.Entity.FieldMultiMap>();
                                FrameworkUAS.BusinessLogic.FieldMultiMap fieldMultiWorker = new FrameworkUAS.BusinessLogic.FieldMultiMap();
                                allFieldMultiMaps = fieldMultiWorker.SelectFieldMappingID(oldFileMappingID);
                                foreach (FrameworkUAS.Entity.FieldMultiMap thisFieldMultiMap in allFieldMultiMaps)
                                {
                                    thisFieldMultiMap.FieldMultiMapID = 0;
                                    thisFieldMultiMap.FieldMappingID = newFileMappingID;
                                    thisFieldMultiMap.CreatedByUserID = CurrentUser.UserID;
                                    thisFieldMultiMap.UpdatedByUserID = CurrentUser.UserID;
                                    thisFieldMultiMap.DateCreated = DateTime.Now;
                                    thisFieldMultiMap.DateUpdated = DateTime.Now;

                                    fieldMultiWorker.Save(thisFieldMultiMap);
                                }
                            }
                            #endregion
                            #region TransformationFieldMap
                            if (allTransFieldMaps.FirstOrDefault(x => x.FieldMappingID == oldFileMappingID) != null)
                            {
                                List<FrameworkUAS.Entity.TransformationFieldMap> fieldMappingTransFieldMaps = allTransFieldMaps.Where(x => x.FieldMappingID == oldFileMappingID).ToList();
                                foreach (FrameworkUAS.Entity.TransformationFieldMap tfm in fieldMappingTransFieldMaps)
                                {
                                    tfm.FieldMappingID = newFileMappingID;
                                    tfm.SourceFileID = newSourceFileID;
                                    tfm.CreatedByUserID = CurrentUser.UserID;
                                    tfm.UpdatedByUserID = CurrentUser.UserID;
                                    tfm.DateCreated = DateTime.Now;
                                    tfm.DateUpdated = DateTime.Now;

                                    transFieldMapWorker.Save(tfm);
                                }
                            }
                            #endregion
                        }
                        clientMessage = "Success.";
                        #endregion
                    }
                    else
                    {
                        clientMessage = "Saving new source file failed. Please contact customer support if the problem persists.";
                        isError = true;
                    }
                }
            }
            else
            {
                clientMessage = "Source file to duplicate was unclear. Please contact customer support if the problem persists.";
                isError = true;
            }

            return Json(new { status = isError.ToString(), message = clientMessage.ToString(), sourceFileID = newSourceFileID }, JsonRequestBehavior.AllowGet);
            //return Json(isError, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FileManagement_Read_Data([DataSourceRequest]DataSourceRequest request, string type = "", string fileName = "", int fileType = 0, int pubID = 0, string PageSize = "10", string PageNumber = "1")
        {
            UAS.Web.Helpers.KendoGridHelper<UAS.Web.Models.FileMapperWizard.CurrentMappingModel> gh = new UAS.Web.Helpers.KendoGridHelper<UAS.Web.Models.FileMapperWizard.CurrentMappingModel>();
            List<UAS.Web.Helpers.GridSort> lstgs = gh.GetGridSort(request, "FileName");
            string sortColumn = lstgs[0].SortColumnName;
            string sortdirection = lstgs[0].SortDirection;

            List<UAS.Web.Models.FileMapperWizard.CurrentMappingModel> listRange = new List<UAS.Web.Models.FileMapperWizard.CurrentMappingModel>();

            #region Copied From Above
            UAS.Web.Models.FileMapperWizard.FileManagementSearchModel model = new Models.FileMapperWizard.FileManagementSearchModel();
            model.type = type;
            model.fileName = fileName;
            model.fileType = fileType;
            model.pubID = pubID;

            if (model.type == null)
                model.type = "";

            List<UAS.Web.Models.FileMapperWizard.CurrentMappingModel> cmmList = new List<Models.FileMapperWizard.CurrentMappingModel>();
            FrameworkUAS.BusinessLogic.SourceFile sfWorker = new FrameworkUAS.BusinessLogic.SourceFile();
            List<FrameworkUAS.Entity.SourceFile> sources = new List<FrameworkUAS.Entity.SourceFile>();

            int currentPage = 1;
            int.TryParse(PageNumber, out currentPage);
            int pageSize = 10;
            int.TryParse(PageSize, out pageSize);

            int serviceID = 0;
            KMPlatform.BusinessLogic.Service workerService = new KMPlatform.BusinessLogic.Service();
            if (model.type.Equals("CIRC", StringComparison.CurrentCultureIgnoreCase))
            {
                KMPlatform.Entity.Service circService = workerService.Select(KMPlatform.Enums.Services.CIRCFILEMAPPER, false);
                serviceID = circService.ServiceID;
            }
            else
            {
                KMPlatform.Entity.Service uadService = workerService.Select(KMPlatform.Enums.Services.UADFILEMAPPER, false);
                serviceID = uadService.ServiceID;
            }

            int totalCount = sfWorker.SelectPagingCount(CurrentClient.ClientID, serviceID, model.type, model.pubID, model.fileType, model.fileName);
            sources = sfWorker.SelectPaging(CurrentClient.ClientID, currentPage, pageSize, serviceID, model.type, model.pubID, model.fileType, model.fileName, sortColumn, sortdirection);
            KMPlatform.BusinessLogic.Service servWorker = new KMPlatform.BusinessLogic.Service();
            List<KMPlatform.Entity.Service> services = servWorker.Select(true);
            //KMPlatform.Entity.Service uadFileMapperService = services.SingleOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.UADFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));
            KMPlatform.Entity.Service circFileMapperService = services.SingleOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase));

            KMPlatform.BusinessLogic.ServiceFeature servFeatWorker = new KMPlatform.BusinessLogic.ServiceFeature();
            List<KMPlatform.Entity.ServiceFeature> serviceFeatures = servFeatWorker.Select();
            FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> fileTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Database_File).ToList();
            KMPlatform.BusinessLogic.User userWorker = new KMPlatform.BusinessLogic.User();
            List<KMPlatform.Entity.User> users = userWorker.Select(false);
            FrameworkUAD.BusinessLogic.Product prodWorker = new FrameworkUAD.BusinessLogic.Product();
            List<FrameworkUAD.Entity.Product> products = prodWorker.Select(CurrentClient.ClientConnections);

            foreach (FrameworkUAS.Entity.SourceFile sf in sources)
            {
                string fileTypeName = "";
                string createdBy = "";
                string updatedBy = "";
                string pubcode = "N/A";
                bool isCirc = false;
                if (sf.ServiceID.Equals(circFileMapperService.ServiceID))
                    isCirc = true;

                if (isCirc)
                {
                    fileTypeName = fileTypes.FirstOrDefault(x => x.CodeId == sf.DatabaseFileTypeId).CodeName;
                    if (sf.PublicationID != null && products.Count(x => x.PubID == sf.PublicationID) > 0)
                        pubcode = products.FirstOrDefault(x => x.PubID == sf.PublicationID).PubCode;
                }
                else
                {
                    if (serviceFeatures.Count(x => x.ServiceFeatureID == sf.ServiceFeatureID) > 0)
                        fileTypeName = serviceFeatures.FirstOrDefault(x => x.ServiceFeatureID == sf.ServiceFeatureID).SFName;

                }

                //User may not exist, example user received a new user and old was removed/deleted
                if (users.Count(x => x.UserID == sf.CreatedByUserID) > 0)
                    createdBy = users.FirstOrDefault(x => x.UserID == sf.CreatedByUserID).UserName;
                if (sf.UpdatedByUserID != null && users.Count(x => x.UserID == sf.UpdatedByUserID) > 0)
                    updatedBy = users.FirstOrDefault(x => x.UserID == sf.UpdatedByUserID).UserName;

                cmmList.Add(new UAS.Web.Models.FileMapperWizard.CurrentMappingModel(totalCount, isCirc, sf.SourceFileID, sf.DatabaseFileTypeId, fileTypeName, sf.FileName, sf.IsDeleted, sf.Extension, sf.Delimiter, sf.IsTextQualifier, sf.ServiceID, sf.ServiceFeatureID, sf.DateCreated, sf.CreatedByUserID, createdBy, sf.PublicationID, pubcode, sf.DateUpdated, sf.UpdatedByUserID, updatedBy));
            }
            #endregion

            listRange = cmmList;

            IQueryable<UAS.Web.Models.FileMapperWizard.CurrentMappingModel> gs = listRange.AsQueryable();
            DataSourceResult result = gs.ToDataSourceResult(request);
            result.Total = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private Models.FileMapperWizard.FileMapperWizardViewModel GetMainModel(bool isCirc = false)
        {
            fmwModel = new Models.FileMapperWizard.FileMapperWizardViewModel(false, isCirc, CurrentClient.ClientID, CurrentUser, CurrentClient);
            return fmwModel;
        }
        #endregion

        #region 1 - Setup
        //could also pass a FormCollection object from View instead of properties
        [HttpGet]
        public ActionResult SaveSetup(bool IsCirculation, int ClientId, int SourceFileID, bool IsNewFile, int ServiceFeatureID, string FileSaveAsName, string FilePath, bool IsFullFile,
                                      string Matching, string Delimeter, bool HasQuotation, string QDateFormat, int ServiceID, int? DatabaseFileTypeID, int? PublicationID, string Extension)
        {

            if (!string.IsNullOrEmpty(FileSaveAsName))
            {
                if (SourceFileID == 0)
                {
                    ViewBag.SetupSuccess = false;
                    SetStep(1);
                    string error = "File name is already in use";
                    Models.Common.UASError er = new Models.Common.UASError() { ErrorMessage = error };
                    if (!fmwModel.setupViewModel.ErrorList.Exists(x => x.ErrorMessage.IsCaseInsensitiveEqual(error)))
                        fmwModel.setupViewModel.ErrorList.Add(er);
                    return PartialView("Partials/_setup", fmwModel.setupViewModel);
                }
                else if (ModelState.IsValid)
                {
                    //save the sourceFile
                    //Certain files can be finished after step one
                    bool finishFileAfterStepOne = false;
                    FrameworkUAS.BusinessLogic.SourceFile sfWrk = new FrameworkUAS.BusinessLogic.SourceFile();
                    //CheckFileName(ClientId, FileSaveAsName, IsNewFile);
                    //int sfId = GetSourceFileId(FileSaveAsName, IsNewFile, ClientId);                      
                    int FileSnippetID = 0;
                    FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                    FrameworkUAD_Lookup.Entity.Code fileSnipCode = new FrameworkUAD_Lookup.Entity.Code();
                    fileSnipCode = codeWorker.SelectCodeValue(FrameworkUAD_Lookup.Enums.CodeType.File_Snippet, FrameworkUAD_Lookup.Enums.FileSnippetTypes.Prefix.ToString());
                    if (fileSnipCode != null)
                        FileSnippetID = fileSnipCode.CodeId;

                    int dbft = 0;
                    int.TryParse(DatabaseFileTypeID.ToString(), out dbft);

                    #region Save SourceFile                    
                    FrameworkUAS.Entity.SourceFile sf = sfWrk.SelectSourceFileID(SourceFileID);
                    sf.ClientID = ClientId;
                    sf.CreatedByUserID = CurrentUser.UserID;
                    sf.DateCreated = DateTime.Now;
                    sf.FileName = FileSaveAsName;
                    sf.Extension = Extension;
                    sf.IsDeleted = false;
                    if (IsCirculation)
                    {
                        sf.ServiceFeatureID = 0;
                        if (dbft > 0)
                        {
                            KMPlatform.BusinessLogic.ServiceFeature sfWorker = new KMPlatform.BusinessLogic.ServiceFeature();
                            List<KMPlatform.Entity.ServiceFeature> features = sfWorker.Select();
                            FrameworkUAD_Lookup.Entity.Code dbftCode = codeWorker.SelectCodeId(dbft);
                            if (dbftCode.CodeName.Equals(FrameworkUAD_Lookup.Enums.FileTypes.ACS.ToString(), StringComparison.CurrentCultureIgnoreCase) || dbftCode.CodeName.Equals(FrameworkUAD_Lookup.Enums.FileTypes.NCOA.ToString(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                finishFileAfterStepOne = true;
                                KMPlatform.Entity.ServiceFeature feature = features.FirstOrDefault(x => x.SFName.Equals(KMPlatform.BusinessLogic.Enums.UADFeatures.Address_Update.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase));
                                sf.ServiceFeatureID = feature.ServiceFeatureID;
                            }
                            else
                            {
                                KMPlatform.Entity.ServiceFeature feature = features.FirstOrDefault(x => x.SFName.Equals(KMPlatform.BusinessLogic.Enums.UADFeatures.File_Import.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase));
                                sf.ServiceFeatureID = feature.ServiceFeatureID;
                            }
                        }
                    }
                    else
                    {
                        FrameworkUAD_Lookup.Entity.Code dbftCode = codeWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Database_File, FrameworkUAD_Lookup.Enums.FileTypes.Audience_Data.ToString().Replace('_', ' '));
                        dbft = dbftCode.CodeId;
                        sf.ServiceFeatureID = ServiceFeatureID;
                    }
                    sf.Delimiter = Delimeter;
                    sf.IsTextQualifier = HasQuotation;
                    sf.QDateFormat = QDateFormat;
                    sf.IsFullFile = IsFullFile;
                    sf.FileSnippetID = FileSnippetID;

                    if (ServiceID > 0)
                        sf.ServiceID = ServiceID;

                    if (dbft > 0)
                        sf.DatabaseFileTypeId = dbft;

                    if (PublicationID != null && PublicationID > 0)
                        sf.PublicationID = PublicationID;

                    SourceFileID = sfWrk.Save(sf);
                    #endregion

                    #region Update SetupViewModel
                    fmwModel.sourceFileId = SourceFileID;
                    fmwModel.setupViewModel.ClientId = fmwModel.client.ClientID;
                    fmwModel.setupViewModel.SourceFileId = SourceFileID;
                    fmwModel.setupViewModel.ServiceFeatureID = ServiceFeatureID;
                    fmwModel.setupViewModel.FileSaveAsName = FileSaveAsName;
                    fmwModel.setupViewModel.Matching = Matching;
                    fmwModel.setupViewModel.Delimeter = Delimeter;
                    fmwModel.setupViewModel.HasQuotation = HasQuotation;
                    fmwModel.setupViewModel.QDateFormat = QDateFormat;
                    fmwModel.setupViewModel.IsFullFile = IsFullFile;
                    if (!string.IsNullOrEmpty(FilePath))
                        fmwModel.setupViewModel.IncomingFile = new System.IO.FileInfo(FilePath);

                    fmwModel.isNewFile = IsNewFile;
                    #endregion

                    //go to next step _columnMapping
                    try
                    {
                        if (string.IsNullOrEmpty(FilePath) && IsNewFile)
                            FilePath = fmwModel.setupViewModel.IncomingFile.FullName;

                        Models.FileMapperWizard.ColumnMappingViewModel cVM = new Models.FileMapperWizard.ColumnMappingViewModel(IsNewFile, FilePath, SourceFileID, fmwModel.client, Delimeter, HasQuotation, CurrentUser.UserID);
                        fmwModel.columnMappingViewModel = cVM;

                        fmwModel = fmwModel;
                        ViewBag.SetupSuccess = true;
                        SetStep(2);
                        return PartialView("Partials/_columnMapping", cVM);

                    }
                    catch (Models.Common.UASException ex)
                    {
                        ViewBag.SetupSuccess = false;
                        SetStep(1);
                        fmwModel.setupViewModel.ErrorList = ex.ErrorList;
                        return PartialView("Partials/_setup", fmwModel.setupViewModel);
                    }
                }
                else
                {
                    ViewBag.SetupSuccess = false;
                    SetStep(1);
                    Models.Common.UASError er = new Models.Common.UASError() { ErrorMessage = "Model not valid" };
                    if (!fmwModel.setupViewModel.ErrorList.Contains(er))
                        fmwModel.setupViewModel.ErrorList.Add(er);
                    return PartialView("Partials/_setup", fmwModel.setupViewModel);
                }
            }
            else
            {
                ViewBag.SetupSuccess = false;
                SetStep(1);
                Models.Common.UASError er = new Models.Common.UASError() { ErrorMessage = "File name is already in use" };
                if (!fmwModel.setupViewModel.ErrorList.Contains(er))
                    fmwModel.setupViewModel.ErrorList.Add(er);
                return PartialView("Partials/_setup", fmwModel.setupViewModel);
            }
        }

        public JsonResult ValidateSetup(string FileSaveAsName, int SourceFileID)
        {
            bool isValid = true;
            string errorMessage = "";
            if (!string.IsNullOrEmpty(FileSaveAsName))
            {
                FrameworkUAS.BusinessLogic.SourceFile sfWrk = new FrameworkUAS.BusinessLogic.SourceFile();
                List<FrameworkUAS.Entity.SourceFile> sources = new List<FrameworkUAS.Entity.SourceFile>();
                sources = sfWrk.Select(fmwModel.setupViewModel.ClientId, false);
                if (sources.Count(x => x.IsDeleted == false && x.SourceFileID != SourceFileID && x.FileName.Equals(FileSaveAsName, StringComparison.CurrentCultureIgnoreCase)) > 0)
                {
                    isValid = false;
                    errorMessage = "File name is already in use";
                }
                else if (SourceFileID == 0)
                {
                    isValid = false;
                    errorMessage = "File name is already in use";
                }
                else if (ModelState.IsValid)
                {
                    isValid = true;
                    errorMessage = "";
                }
                else
                {
                    isValid = false;
                    errorMessage = "Model not valid";
                }
            }
            else
            {
                isValid = false;
                errorMessage = "File name is already in use";
            }

            return Json(new { IsValid = isValid, ErrorMessage = errorMessage }, JsonRequestBehavior.AllowGet);
        }

        private bool IsFileNameUnique(string fileName, bool isNewFile)
        {
            fmwModel.setupViewModel.IsNewFile = isNewFile;
            int sfId = 0;
            sfId = GetSourceFileId(fileName, isNewFile, CurrentClient.ClientID);

            return sfId > 0 ? false : true;
        }
        [HttpPost]
        public ActionResult UploadTempFiles(IEnumerable<HttpPostedFileBase> DataFile, string messageId, bool? isNewFile)
        {

            if (isNewFile == null) isNewFile = true;
            fmwModel.setupViewModel.IsNewFile = isNewFile.Value;
            int sfId = 0;
            string physicalPath = string.Empty;
            string fileName = string.Empty;
            // The Name of the Upload component is "files"
            if (DataFile != null)
            {
                foreach (var file in DataFile)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    fileName = System.IO.Path.GetFileName(file.FileName);

                    //physicalPath = System.IO.Path.Combine(Server.MapPath("~/App_Data"), messageId, fileName);
                    physicalPath = System.IO.Path.Combine(Server.MapPath("~/App_Data"), CurrentClient.ClientID.ToString(), fileName);
                    string folderPath = System.IO.Path.Combine(Server.MapPath("~/App_Data"), CurrentClient.ClientID.ToString());
                    if (!System.IO.Directory.Exists(folderPath))
                    {
                        System.IO.Directory.CreateDirectory(folderPath);
                    }

                    fmwModel.setupViewModel.IncomingFile = new System.IO.FileInfo(physicalPath);
                    //System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Server.MapPath("~/App_Data"), messageId));
                    file.SaveAs(physicalPath);
                    sfId = GetSourceFileId(fileName, isNewFile.Value, CurrentClient.ClientID);
                    fmwModel.setupViewModel.DataFile = file;
                }
            }

            fmwModel.setupViewModel.SourceFileId = sfId;
            fmwModel.setupViewModel.FileSaveAsName = fmwModel.setupViewModel.IncomingFile.Name.Replace(fmwModel.setupViewModel.IncomingFile.Extension, string.Empty);// fNameNoExt;
            ViewBag.Ext = fmwModel.setupViewModel.IncomingFile.Extension.Replace(".", string.Empty);// ext;

            string error = string.Empty;
            if (sfId == 0)
            {
                fmwModel.setupViewModel.IsNewFile = false;
                error = "File name is already in use";
                ViewBag.SetupSuccess = false;
                SetStep(1);
                Models.Common.UASError er = new Models.Common.UASError() { ErrorMessage = error };
                if (!fmwModel.setupViewModel.ErrorList.Exists(x => x.ErrorMessage.IsCaseInsensitiveEqual(error)))
                {
                    fmwModel.setupViewModel.ErrorList.Add(er);
                    fmwModel.setupViewModel.ErrorList.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                }
            }
            return Json(new { status = sfId.ToString() + "|" + physicalPath.ToString() + "|" + fmwModel.setupViewModel.FileSaveAsName + "|" + ViewBag.Ext + "|" + error }, "text/plain");

        }
        public ActionResult DeleteTempFiles(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = System.IO.Path.GetFileName(fullName);
                    var physicalPath = System.IO.Path.Combine(Server.MapPath("~/App_Data"), fileName);

                    // TODO: Verify user permissions
                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }
            // Return an empty string to signify success
            return Content("");
        }
        #endregion
        #region 2 - ColumnMapping
        public ActionResult AdditionalMappings()
        {
            Models.FileMapperWizard.AdditionalColumnMapModel acmm = new Models.FileMapperWizard.AdditionalColumnMapModel(fmwModel.isNewFile, fmwModel.setupViewModel.SourceFileId, fmwModel.client);
            return PartialView("Partials/FileMapping/_additionalColumn", acmm);
        }
        [HttpGet]
        public ActionResult GetDemoUpdates()
        {
            List<SelectListItem> data = new List<SelectListItem>();

            FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> DemoUpdateTypes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);

            foreach (var i in DemoUpdateTypes)
                data.Add(new SelectListItem() { Text = i.DisplayName, Value = i.CodeId.ToString() });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetMAFFields()
        {
            //List<SelectListItem> data = new List<SelectListItem>();
            //data.Add(new SelectListItem() { Text = "Ignore", Value = "Ignore" });

            //FrameworkUAD.BusinessLogic.FileMappingColumn fmcWrk = new FrameworkUAD.BusinessLogic.FileMappingColumn();
            //List<FrameworkUAD.Object.FileMappingColumn> uadColumns = fmcWrk.Select(fmwModel.client.ClientConnections);

            //foreach (var i in uadColumns)
            //    data.Add(new SelectListItem() { Text = i.ColumnName, Value = i.DataTable + "." + i.ColumnName });
            //return Json(data, JsonRequestBehavior.AllowGet);

            List<Models.FileMapperWizard.CustomDropDownList> data = new List<Models.FileMapperWizard.CustomDropDownList>();
            data.Add(new Models.FileMapperWizard.CustomDropDownList() { Text = "Ignore", Value = "Ignore", Group = "" });

            FrameworkUAD.BusinessLogic.FileMappingColumn fmcWrk = new FrameworkUAD.BusinessLogic.FileMappingColumn();
            List<FrameworkUAD.Object.FileMappingColumn> uadColumns = fmcWrk.Select(fmwModel.client.ClientConnections).OrderBy(x => x.ColumnName).ToList();

            foreach (var i in uadColumns)
                data.Add(new Models.FileMapperWizard.CustomDropDownList() { Text = i.ColumnName, Value = i.DataTable + "." + i.ColumnName, Group = i.DataTable });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteAdditionalColumnMapping(int SourceFileID, int FieldMappingID)
        {
            //try to delete the FieldMapping
            bool complete = true;

            FrameworkUAS.BusinessLogic.FieldMapping fieldMappingWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
            fieldMappingWorker.DeleteMapping(FieldMappingID);
            fieldMappingWorker.ColumnReorder(SourceFileID);

            return Json(new { Complete = complete }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteMultiColumnMapping(int FieldMultiMapID)
        {
            bool complete = true;

            FrameworkUAS.BusinessLogic.FieldMultiMap fieldMultiMapWorker = new FrameworkUAS.BusinessLogic.FieldMultiMap();
            fieldMultiMapWorker.DeleteByFieldMultiMapID(FieldMultiMapID);

            return Json(new { Complete = complete }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RefreshColumnMapping()
        {
            if (fmwModel.sourceFileId != fmwModel.columnMappingViewModel.SourceFileID)
            {
                Models.FileMapperWizard.ColumnMappingViewModel cVM = new Models.FileMapperWizard.ColumnMappingViewModel(this.fmwModel.isNewFile, this.fmwModel.filePath, this.fmwModel.sourceFileId, fmwModel.client, this.fmwModel.setupViewModel.Delimeter, this.fmwModel.setupViewModel.HasQuotation, CurrentUser.UserID);
                this.fmwModel.columnMappingViewModel = cVM;
                this.fmwModel = fmwModel;
            }
            return PartialView("Partials/_columnMapping", fmwModel.columnMappingViewModel);
        }

        public JsonResult FormatMultiMapping(string standardMappings, string multiMappings)
        {
            string formattedText = "";
            standardMappings = standardMappings.Replace("\\", "").TrimStart('"').TrimEnd('"');

            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            var standardMaps = jf.FromJson<Models.FileMapperWizard.UserMappedColumn[]>(standardMappings);

            multiMappings = multiMappings.Replace("\\", "").TrimStart('"').TrimEnd('"');
            var multiMaps = jf.FromJson<Models.FileMapperWizard.UserMultiMapColumn[]>(multiMappings);

            foreach (var s in standardMaps)//mappings
            {
                string incoming = s.SourceColumn;
                List<string> multiMapTo = new List<string>();
                multiMapTo.Add(s.MappedColumn.Split('.').Last());

                var multi = multiMaps.Where(x => x.FieldMappingID == s.FieldMapId).ToList();
                if (multi.Count > 0)
                {
                    foreach (var m in multi)
                    {
                        multiMapTo.Add(m.MAFField.Split('.').Last());
                    }

                    if (formattedText.Length > 0)
                        formattedText = formattedText + " and '" + incoming + "' map to (" + String.Join("|", multiMapTo) + ")";
                    else
                        formattedText = formattedText + "'" + incoming + "' map to (" + String.Join("|", multiMapTo) + ")";
                }
            }

            return Json(new { Text = formattedText }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidateMapping(string ValidateColumns)
        {
            bool isValid = true;
            string errorMessage = "";

            ValidateColumns = ValidateColumns.Replace("\\", "").TrimStart('"').TrimEnd('"');

            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            var mappings = jf.FromJson<Models.FileMapperWizard.UserColumnValidation[]>(ValidateColumns);

            FrameworkUAD.BusinessLogic.FileMappingColumn fmcWrk = new FrameworkUAD.BusinessLogic.FileMappingColumn();
            List<FrameworkUAD.Object.FileMappingColumn> uadColumns = fmcWrk.Select(fmwModel.client.ClientConnections);

            KMPlatform.Entity.ServiceFeature sf = new KMPlatform.Entity.ServiceFeature();
            if (fmwModel.setupViewModel.FeaturesList != null)
            {
                sf = fmwModel.setupViewModel.FeaturesList.FirstOrDefault(x => x.ServiceFeatureID == fmwModel.setupViewModel.ServiceFeatureID);
            }
            else
            {
                KMPlatform.BusinessLogic.ServiceFeature sfWorker = new KMPlatform.BusinessLogic.ServiceFeature();
                List<KMPlatform.Entity.ServiceFeature> FeaturesList = sfWorker.Select();
                sf = FeaturesList.FirstOrDefault(x => x.ServiceFeatureID == fmwModel.setupViewModel.ServiceFeatureID);
            }
            if (sf != null && !sf.SFName.Equals("Data Compare", StringComparison.CurrentCultureIgnoreCase))
            {
                var m = mappings.FirstOrDefault(x => x.MappedColumn.Substring(x.MappedColumn.LastIndexOf('.') + 1).Equals("PubCode", StringComparison.CurrentCultureIgnoreCase));
                if (m == null)
                {
                    errorMessage += "Pubcode not detected. Please map one column to pubcode.";
                    isValid = false;
                }
            }

            foreach (var cm in mappings)
            {
                //cm.Type --Regular --Multi --Additional

                //Rules and Laws
                //PubCode mapped
                //No Mapped Column duplicates except ignore
                //Multi and Additional shouldn't be Ignore (No point doing them then)
                //If Mapped Column is not a standard column then DemoUpdateID needs to be available (Additional)

                #region Check Multi/Additional for Ignore
                if (cm.Type.Equals("Multi", StringComparison.CurrentCultureIgnoreCase) && cm.MappedColumn.Equals("Ignore", StringComparison.CurrentCultureIgnoreCase))
                {
                    errorMessage += "Multi Column Mapping cannot be set to ignore.";
                    isValid = false;
                }
                else if (cm.Type.Equals("Additional", StringComparison.CurrentCultureIgnoreCase) && cm.MappedColumn.Equals("Ignore", StringComparison.CurrentCultureIgnoreCase))
                {
                    errorMessage += "Additional Column Mapping cannot be set to ignore.";
                    isValid = false;
                }
                #endregion

                #region Check For Duplicate Mapping
                if (!cm.MappedColumn.Equals("Ignore", StringComparison.CurrentCultureIgnoreCase))
                {
                    var list = mappings.Where(x => x.MappedColumn.Equals(cm.MappedColumn, StringComparison.CurrentCultureIgnoreCase)).ToList();

                    if (list.Count > 1)
                    {
                        if (cm.Type.Equals("Regular", StringComparison.CurrentCultureIgnoreCase))
                            errorMessage += "Source Row " + cm.SourceColumn + ": Row mapped to column has a duplicate entry (" + cm.MappedColumn + ").";
                        else if (cm.Type.Equals("Multi", StringComparison.CurrentCultureIgnoreCase))
                            errorMessage += "Multi Column Mapping: Row mapped to column has a duplicate entry (" + cm.MappedColumn + ").";
                        else if (cm.Type.Equals("Additional", StringComparison.CurrentCultureIgnoreCase))
                            errorMessage += "Additional Column Mapping: Row mapped to column has a duplicate entry (" + cm.MappedColumn + ").";

                        isValid = false;
                    }
                }
                #endregion

                #region Check DemoUpdateID 
                if (cm.Type.Equals("Additional", StringComparison.CurrentCultureIgnoreCase) && (cm.DemoUpdateID == 0 || cm.DemoUpdateID == null))
                {
                    errorMessage += "Additional Column Mapping is missing the update type.";
                    isValid = false;
                }
                #endregion
            }

            return Json(new { IsValid = isValid, ErrorMessage = errorMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveMapping(string MappedColumns)
        {
            MappedColumns = MappedColumns.Replace("\\", "").TrimStart('"').TrimEnd('"');

            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            var mappings = jf.FromJson<Models.FileMapperWizard.UserMappedColumn[]>(MappedColumns);

            FrameworkUAD.BusinessLogic.FileMappingColumn fmcWrk = new FrameworkUAD.BusinessLogic.FileMappingColumn();
            List<FrameworkUAD.Object.FileMappingColumn> uadColumns = fmcWrk.Select(fmwModel.client.ClientConnections);

            FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> fieldMappingTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping);

            //List<FrameworkUAS.Entity.FieldMapping> fieldMappings = new List<FrameworkUAS.Entity.FieldMapping>();
            FrameworkUAS.BusinessLogic.FieldMapping fmWrk = new FrameworkUAS.BusinessLogic.FieldMapping();
            int order = 1;
            foreach (var cm in mappings)//mappings
            {
                string mappedColumnName = cm.MappedColumn.Substring(cm.MappedColumn.LastIndexOf('.') + 1);
                int fieldMappingTypeID = 0;

                #region Determine FieldMappingTypeId
                FrameworkUAD.Object.FileMappingColumn foundColumn = uadColumns.FirstOrDefault(x => x.ColumnName.Equals(mappedColumnName.ToString(), StringComparison.CurrentCultureIgnoreCase));
                fieldMappingTypeID = SelectFieldMappingTypeID(mappedColumnName, foundColumn, fieldMappingTypes);
                #endregion

                FrameworkUAS.Entity.FieldMapping fm = new FrameworkUAS.Entity.FieldMapping();
                //fill out fm object
                fm.ColumnOrder = order;
                fm.CreatedByUserID = CurrentUser.UserID;
                fm.DataType = "varchar";                            //parse for string, int, date
                fm.DateCreated = DateTime.Now;
                fm.DemographicUpdateCodeId = cm.DemoUpdateID;       // "need to set this if a demo column";
                fm.FieldMappingID = cm.FieldMapId;                  //will be 0 if new > 0 if an edit
                fm.FieldMappingTypeID = fieldMappingTypeID;
                //fm.FieldMultiMappings;                            //need someing on CM model
                fm.HasMultiMapping = false;
                fm.IncomingField = cm.SourceColumn;
                fm.IsNonFileColumn = false;                         //need to handle on CM
                fm.MAFField = mappedColumnName;
                fm.PreviewData = cm.PreviewData;
                fm.SourceFileID = cm.SourceFileId;

                if (fmwModel.isNewFile == false)
                {
                    fm.UpdatedByUserID = CurrentUser.UserID;
                    fm.DateUpdated = DateTime.Now;
                }

                fmWrk.Save(fm);
                order++;
            }
            // or could put in a list above and do a bulk save via passing xml

            #region go to Rules
            #region BL helpers
            FrameworkUAS.BusinessLogic.SourceFile sfWrk = new FrameworkUAS.BusinessLogic.SourceFile();
            FrameworkUAS.BusinessLogic.RuleSet rsWrk = new FrameworkUAS.BusinessLogic.RuleSet();
            FrameworkUAS.BusinessLogic.Rule rWrk = new FrameworkUAS.BusinessLogic.Rule();
            //FrameworkUAS.BusinessLogic.RuleField rfWrk = new FrameworkUAS.BusinessLogic.RuleField();
            //FrameworkUAS.BusinessLogic.RuleFieldPredefinedValue rfpdvWrk = new FrameworkUAS.BusinessLogic.RuleFieldPredefinedValue();
            #endregion
            //Models.FileMapperWizard.RulesViewModel rVM = new Models.FileMapperWizard.RulesViewModel();
            //Models.FileMapperWizard.RuleSet viewRuleSet = new Models.FileMapperWizard.RuleSet();
            FrameworkUAS.Model.RuleSet viewRuleSet = new FrameworkUAS.Model.RuleSet();
            FrameworkUAS.Entity.SourceFile sf = new FrameworkUAS.BusinessLogic.SourceFile().SelectSourceFileID(fmwModel.sourceFileId);
            viewRuleSet.ruleSetName = "Rule Set - " + sf.FileName + " " + DateTime.Now.ToString("MMddyyyy");
            viewRuleSet.description = "new rule set created on " + DateTime.Now.ToString() + " by " + CurrentUser.UserName;
            viewRuleSet.sourceFileId = fmwModel.sourceFileId;
            viewRuleSet.isFullFile = sf.IsFullFile;

            //rVM.RuleFields = rfWrk.SelectAll();
            //rVM.RuleFieldValues = rfpdvWrk.SelectAll();
            //rVM.ExistingRuleSets = rsWrk.GetRuleSetsForClient(fmwModel.client.ClientID).ToList();
            //rVM.ExistingRules = rWrk.GetRulesForClient(fmwModel.client.ClientID).ToList();


            if (fmwModel.isNewFile == false)
            {
                FrameworkUAS.Entity.RuleSet myRS = rsWrk.GetSourceFile(sf.SourceFileID);//GetRuleSetsForSourceFile(fmwModel.sourceFileId)
                //FrameworkUAS.Object.RuleSet myRS = rsWrk.GetRuleSetsForSourceFile(fmwModel.sourceFileId).FirstOrDefault();//rule will be SourceFile can only be assigned to one RuleSet
                if (myRS != null && myRS.RuleSetId > 0)
                {
                    viewRuleSet.description = myRS.RuleSetDescription;
                    viewRuleSet.isGlobalRuleSet = myRS.IsGlobal;
                    viewRuleSet.ruleSetId = myRS.RuleSetId;
                    viewRuleSet.ruleSetName = myRS.RuleSetName;

                    viewRuleSet.rules = new FrameworkUAS.BusinessLogic.Model().RulesGetRuleSet(myRS.RuleSetId, sf.SourceFileID);


                    //rVM.NewRuleSetName = myRS.RuleSetName;
                    //rVM.SelectedRuleSetId = myRS.RuleSetId;
                    //FrameworkUAS.BusinessLogic.Rule ruleWrk = new FrameworkUAS.BusinessLogic.Rule();
                    //List<FrameworkUAS.Object.CustomRuleGrid> crGrid = ruleWrk.GetCustomRuleGrid(myRS.RuleSetId);
                    //rVM.CustomRuleGridRules = crGrid;
                }
            }
            else
            {
                viewRuleSet.ruleSetName = "Rule Set - " + sf.FileName + " " + DateTime.Now.ToString("MMddyyyy");
            }

            fmwModel.ruleSet = viewRuleSet;//.rulesViewModel
            fmwModel = fmwModel;
            SetStep(3);
            return PartialView("Partials/_rules", fmwModel.ruleSet);//rulesViewModel
            #endregion
        }

        public int SelectFieldMappingTypeID(string mappedColumnName, FileMappingColumn foundColumn, List<Code> fieldMappingTypes)
        {
            if (fieldMappingTypes == null)
            {
                throw new ArgumentNullException(nameof(fieldMappingTypes));
            }

            var fmlIgnoredId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString(), StringComparison.CurrentCultureIgnoreCase)).CodeId;
            var fmlKmTransformId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.kmTransform.ToString(), StringComparison.CurrentCultureIgnoreCase)).CodeId;
            var fmlStandardId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString(), StringComparison.CurrentCultureIgnoreCase)).CodeId;
            var fmlDemoId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString(), StringComparison.CurrentCultureIgnoreCase)).CodeId;
            var fmlDemoOtherId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Other.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase)).CodeId;
            var fmlDemoDateId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Date.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase)).CodeId;

            if (foundColumn == null || string.IsNullOrWhiteSpace(foundColumn.ColumnName))
            {
                return mappedColumnName.Equals("kmTransform", StringComparison.CurrentCultureIgnoreCase) ? fmlKmTransformId : fmlIgnoredId;
            }

            return GetFieldMappingTypeId(fmlStandardId, fmlDemoId, fmlDemoOtherId, fmlDemoDateId, foundColumn);
        }

        public int GetFieldMappingTypeId(int fmlStandardId, int fmlDemoId, int fmlDemoOtherId, int fmlDemoDateId,
            FrameworkUAD.Object.FileMappingColumn foundColumn)
        {
            if (foundColumn == null)
            {
                throw new ArgumentNullException(nameof(foundColumn));
            }

            if (foundColumn.IsDemographicDate)
            {
                return fmlDemoDateId;
            }

            if (foundColumn.IsDemographic == false)
            {
                return fmlStandardId;
            }

            return !foundColumn.IsDemographicOther ? fmlDemoId : fmlDemoOtherId;
        }

        public JsonResult SaveAdditionalMapping(string MappedColumns)
        {
            MappedColumns = MappedColumns.Replace("\\", "").TrimStart('"').TrimEnd('"');

            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            var mappings = jf.FromJson<Models.FileMapperWizard.UserAdditionalColumn[]>(MappedColumns);
            if (mappings != null)
            {
                FrameworkUAD.BusinessLogic.FileMappingColumn fmcWrk = new FrameworkUAD.BusinessLogic.FileMappingColumn();
                List<FrameworkUAD.Object.FileMappingColumn> uadColumns = fmcWrk.Select(fmwModel.client.ClientConnections);

                FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                List<FrameworkUAD_Lookup.Entity.Code> fieldMappingTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping);

                //List<FrameworkUAS.Entity.FieldMapping> fieldMappings = new List<FrameworkUAS.Entity.FieldMapping>();
                FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
                int sourceFileID = mappings.First().SourceFileID;
                List<FrameworkUAS.Entity.FieldMapping> fmList = fmWorker.Select(sourceFileID);
                int order = fmList.Count(x => x.IsNonFileColumn == false) + 1;
                foreach (var cm in mappings)//mappings
                {
                    int fieldMappingTypeID = 0;
                    string mappedColumnName = cm.MAFField.Substring(cm.MAFField.LastIndexOf('.') + 1);

                    #region Determine FieldMappingTypeId
                    FrameworkUAD.Object.FileMappingColumn foundColumn = uadColumns.FirstOrDefault(x => x.ColumnName.Equals(mappedColumnName.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    fieldMappingTypeID = SelectFieldMappingTypeID(mappedColumnName, foundColumn, fieldMappingTypes);
                    #endregion

                    FrameworkUAS.Entity.FieldMapping fm = new FrameworkUAS.Entity.FieldMapping();
                    //fill out fm object
                    fm.ColumnOrder = cm.ColumnOrder;
                    fm.CreatedByUserID = CurrentUser.UserID;
                    fm.DataType = "varchar";                                   //parse for string, int, date
                    fm.DateCreated = DateTime.Now;
                    fm.DemographicUpdateCodeId = cm.DemographicUpdateCodeId;       // "need to set this if a demo column";
                    fm.FieldMappingID = cm.FieldMappingID;                  //will be 0 if new > 0 if an edit
                    fm.FieldMappingTypeID = fieldMappingTypeID;
                    fm.HasMultiMapping = false;
                    string incoming = Core_AMS.Utilities.StringFunctions.RandomAlphaString(5) + "_" + mappedColumnName;
                    fm.IncomingField = incoming;
                    fm.IsNonFileColumn = true;                         //need to handle on CM
                    fm.MAFField = mappedColumnName;
                    fm.PreviewData = "";
                    fm.SourceFileID = cm.SourceFileID;

                    if (cm.FieldMappingID > 0)
                    {
                        fm.UpdatedByUserID = CurrentUser.UserID;
                        fm.DateUpdated = DateTime.Now;
                    }

                    fmWorker.Save(fm);
                    order++;
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveMultiMapping(string MappedColumns)
        {
            MappedColumns = MappedColumns.Replace("\\", "").TrimStart('"').TrimEnd('"');

            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            var mappings = jf.FromJson<Models.FileMapperWizard.UserMultiMapColumn[]>(MappedColumns);
            if (mappings != null)
            {
                FrameworkUAD.BusinessLogic.FileMappingColumn fmcWrk = new FrameworkUAD.BusinessLogic.FileMappingColumn();
                List<FrameworkUAD.Object.FileMappingColumn> uadColumns = fmcWrk.Select(fmwModel.client.ClientConnections);

                FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                List<FrameworkUAD_Lookup.Entity.Code> fieldMappingTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping);

                int fmlIgnoredId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString(), StringComparison.CurrentCultureIgnoreCase)).CodeId;
                int fmlKmTransformId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.kmTransform.ToString(), StringComparison.CurrentCultureIgnoreCase)).CodeId;
                int fmlStandardId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString(), StringComparison.CurrentCultureIgnoreCase)).CodeId;
                int fmlDemoId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString(), StringComparison.CurrentCultureIgnoreCase)).CodeId;
                int fmlDemoOtherId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Other.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase)).CodeId;
                int fmlDemoDateId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Date.ToString().Replace('_', ' '), StringComparison.CurrentCultureIgnoreCase)).CodeId;

                FrameworkUAS.BusinessLogic.FieldMultiMap fmmWorker = new FrameworkUAS.BusinessLogic.FieldMultiMap();
                FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
                int order = 1;
                foreach (var cm in mappings)//mappings
                {
                    int fieldMappingTypeID = 0;

                    #region Determine FieldMappingTypeId
                    FrameworkUAD.Object.FileMappingColumn foundColumn = uadColumns.FirstOrDefault(x => x.ColumnName.Equals(cm.MAFField.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    if (foundColumn != null && !String.IsNullOrEmpty(foundColumn.ColumnName))
                    {
                        if (foundColumn.IsDemographicDate == true)
                            fieldMappingTypeID = fmlDemoDateId;
                        else if (foundColumn.IsDemographic == false)
                            fieldMappingTypeID = fmlStandardId;
                        else
                        {
                            if (foundColumn.IsDemographicOther == false)
                                fieldMappingTypeID = fmlDemoId;
                            else
                                fieldMappingTypeID = fmlDemoOtherId;
                        }
                    }
                    else if (cm.MAFField.ToString().Equals("kmTransform", StringComparison.CurrentCultureIgnoreCase))
                        fieldMappingTypeID = fmlKmTransformId;
                    else
                        fieldMappingTypeID = fmlIgnoredId;

                    #endregion

                    FrameworkUAS.Entity.FieldMultiMap fm = new FrameworkUAS.Entity.FieldMultiMap
                    {
                        FieldMultiMapID = cm.FieldMultiMapID,
                        FieldMappingID = cm.FieldMappingID,
                        FieldMappingTypeID = fieldMappingTypeID,
                        MAFField = cm.MAFField,
                        DataType = "varchar",
                        PreviewData = "",
                        ColumnOrder = order,
                        DateCreated = DateTime.Now,
                        DateUpdated = DateTime.Now,
                        CreatedByUserID = CurrentUser.UserID,
                        UpdatedByUserID = CurrentUser.UserID
                    };
                    fmmWorker.Save(fm);
                    order++;

                    //Update FieldMapping to show there the field has multiple mapping
                    FrameworkUAS.Entity.FieldMapping fieldMap = fmWorker.SelectFieldMappingID(cm.FieldMappingID, false);
                    fieldMap.HasMultiMapping = true;
                    fmWorker.Save(fieldMap);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult AddSoloAdditionalColumn(int SourceFileID)
        {
            FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
            List<FrameworkUAS.Entity.FieldMapping> fmList = fmWorker.Select(SourceFileID);
            int order = fmList.Count + 1;

            FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> fieldMappingTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping);
            List<FrameworkUAD_Lookup.Entity.Code> demographicUpdateCodeTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);

            int fmlIgnoredId = fieldMappingTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString(), StringComparison.CurrentCultureIgnoreCase)).CodeId;
            FrameworkUAD_Lookup.Entity.Code demographicUpdateCode = demographicUpdateCodeTypes.First(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.DemographicUpdate.Append.ToString(), StringComparison.CurrentCultureIgnoreCase));

            FrameworkUAS.Entity.FieldMapping fm = new FrameworkUAS.Entity.FieldMapping();
            //fill out fm object
            fm.ColumnOrder = order;
            fm.CreatedByUserID = CurrentUser.UserID;
            fm.DataType = "varchar";                                    //parse for string, int, date
            fm.DateCreated = DateTime.Now;
            fm.DemographicUpdateCodeId = demographicUpdateCode.CodeId;       //"need to set this if a demo column";
            fm.FieldMappingID = 0;                                      //will be 0 if new > 0 if an edit
            fm.FieldMappingTypeID = fmlIgnoredId;
            fm.HasMultiMapping = false;
            fm.IncomingField = "Ignore";
            fm.IsNonFileColumn = true;                                  //need to handle on CM
            fm.MAFField = "Ignore";
            fm.PreviewData = "";
            fm.SourceFileID = SourceFileID;

            fm.FieldMappingID = fmWorker.Save(fm);

            bool complete = false;
            if (fm.FieldMappingID > 0)
                complete = true;

            KMPlatform.Entity.Client client = fmwModel.client;

            FrameworkUAD.BusinessLogic.FileMappingColumn fmcWrk = new FrameworkUAD.BusinessLogic.FileMappingColumn();
            List<FrameworkUAD.Object.FileMappingColumn> uadColumns = fmcWrk.Select(client.ClientConnections);
            List<FrameworkUAD.Object.FileMappingColumn> MappingColumns = uadColumns;

            FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> DemoUpdateTypes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);

            Models.FileMapperWizard.AddNewColumn anc = new Models.FileMapperWizard.AddNewColumn(SourceFileID, MappingColumns, false, fm.IncomingField, fm.PreviewData, DemoUpdateTypes, fm.FieldMappingID, fm.FieldMappingTypeID, fm.DemographicUpdateCodeId, fm.ColumnOrder, fm.MAFField, false, false);
            return PartialView("~/Views/Shared/EditorTemplates/AddNewColumn.cshtml", anc);
        }

        public ActionResult LoadMultiMapGrid(int FieldMappingID)
        {
            Models.FileMapperWizard.MultiMapModel mmm = new Models.FileMapperWizard.MultiMapModel(FieldMappingID);

            return PartialView("Partials/FileMapping/_multiMapColumn", mmm);
        }

        public JsonResult RemoveTransformationsAndMultiMappingsForFieldMappingID(int FieldMappingID)
        {
            bool complete = true;
            try
            {
                //Remove All Transformations for FieldMappingID
                FrameworkUAS.BusinessLogic.TransformationFieldMap transformationFieldMapWorker = new FrameworkUAS.BusinessLogic.TransformationFieldMap();
                transformationFieldMapWorker.DeleteFieldMappingID(FieldMappingID);

                //Remove All MultiMappings for FieldMappingID
                FrameworkUAS.BusinessLogic.FieldMultiMap multiMapWorker = new FrameworkUAS.BusinessLogic.FieldMultiMap();
                multiMapWorker.DeleteByFieldMappingID(FieldMappingID);

                complete = true;
            }
            catch (Exception ex)
            {
                complete = false;
            }
            return Json(complete, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveAdditionalMapping(int FieldMappingID)
        {
            bool complete = true;
            try
            {
                //Remove All Transformations for FieldMappingID
                FrameworkUAS.BusinessLogic.TransformationFieldMap transformationFieldMapWorker = new FrameworkUAS.BusinessLogic.TransformationFieldMap();
                transformationFieldMapWorker.DeleteFieldMappingID(FieldMappingID);

                //Remove All MultiMappings for FieldMappingID
                FrameworkUAS.BusinessLogic.FieldMultiMap multiMapWorker = new FrameworkUAS.BusinessLogic.FieldMultiMap();
                multiMapWorker.DeleteByFieldMappingID(FieldMappingID);

                //Remove The Mapping
                FrameworkUAS.BusinessLogic.FieldMapping fieldMappingWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
                fieldMappingWorker.DeleteMapping(FieldMappingID);

                complete = true;
            }
            catch (Exception ex)
            {
                complete = false;
            }
            return Json(complete, JsonRequestBehavior.AllowGet);
        }

        #region Transformation Functions
        //GRID that displays current transformations applied to Field Mapping. Displays under the Field Mapping row on step 2
        public ActionResult LoadTransformationGrid(int SourceFileID, int FieldMappingID)
        {
            //Models.FileMapperWizard.FileMapperWizardViewModel myvm = fmwModel;
            FrameworkUAS.BusinessLogic.TransformationFieldMap tfmWorker = new FrameworkUAS.BusinessLogic.TransformationFieldMap();
            List<FrameworkUAS.Entity.TransformationFieldMap> transformFieldMaps = new List<FrameworkUAS.Entity.TransformationFieldMap>();
            transformFieldMaps = tfmWorker.Select(SourceFileID).Where(x => x.FieldMappingID == FieldMappingID).ToList();

            Models.FileMapperWizard.TransformationsViewModel tvm = new Models.FileMapperWizard.TransformationsViewModel();
            tvm.transformations = new List<Models.FileMapperWizard.TransformationMap>();
            tvm.SourceFileId = SourceFileID;
            tvm.FieldMappingId = FieldMappingID;

            FrameworkUAS.BusinessLogic.Transformation tWorker = new FrameworkUAS.BusinessLogic.Transformation();
            List<FrameworkUAS.Entity.Transformation> transformList = new List<FrameworkUAS.Entity.Transformation>();
            transformList = tWorker.SelectClient(CurrentClient.ClientID);

            FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> codeList = new List<FrameworkUAD_Lookup.Entity.Code>();
            codeList = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Transformation);

            foreach (FrameworkUAS.Entity.TransformationFieldMap tfm in transformFieldMaps)
            {
                Models.FileMapperWizard.TransformationMap tm = new Models.FileMapperWizard.TransformationMap();

                tm.TransformationFieldMapId = tfm.TransformationFieldMapID;
                tm.TransformationId = tfm.TransformationID;
                tm.SourceFileId = tfm.SourceFileID;
                tm.FieldMappingId = tfm.FieldMappingID;

                FrameworkUAS.Entity.Transformation singleTransform = transformList.FirstOrDefault(x => x.TransformationID == tfm.TransformationID);
                FrameworkUAD_Lookup.Entity.Code code = codeList.FirstOrDefault(x => x.CodeId == singleTransform.TransformationTypeID);

                tm.TransformationName = "TRANSFORMATION: " + code.DisplayName.ToUpper() + " - " + singleTransform.TransformationName;

                tvm.transformations.Add(tm);
            }

            return PartialView("Partials/Transformation/_displayTransformations", tvm);
        }

        public JsonResult RemoveTransformationFieldMapping(int TransformationFieldMapId, int SourceFileId, int FieldMapId)
        {
            FrameworkUAS.BusinessLogic.TransformationFieldMap tfmWorker = new FrameworkUAS.BusinessLogic.TransformationFieldMap();
            int del = tfmWorker.DeleteTransformationFieldMapID(TransformationFieldMapId);

            bool hasTransformation = false;
            List<FrameworkUAS.Entity.TransformationFieldMap> transformationFieldMaps = new List<FrameworkUAS.Entity.TransformationFieldMap>();
            transformationFieldMaps = tfmWorker.Select(SourceFileId);
            if (transformationFieldMaps != null && transformationFieldMaps.Count(x => x.FieldMappingID == FieldMapId) > 0)
                hasTransformation = true;

            return Json(hasTransformation, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DataTransformationSetup(int SourceFileId, int FieldMappingId, string FieldMappingName)
        {
            Models.FileMapperWizard.StandardTransformationDataModel stdm = new Models.FileMapperWizard.StandardTransformationDataModel(SourceFileId, FieldMappingId, FieldMappingName);

            Models.FileMapperWizard.FileMapperWizardViewModel myvm = fmwModel;
            fmwModel.standardTransformationDataModel = stdm;

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditTransformationDataTransformationSetup(int FieldMappingId)
        {
            FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
            FrameworkUAS.Entity.FieldMapping fm = fmWorker.SelectFieldMappingID(FieldMappingId, false);

            Models.FileMapperWizard.StandardTransformationDataModel stdm = new Models.FileMapperWizard.StandardTransformationDataModel(fm.SourceFileID, FieldMappingId, fm.IncomingField);

            Models.FileMapperWizard.FileMapperWizardViewModel myvm = fmwModel;
            fmwModel.standardTransformationDataModel = stdm;

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadTransformationSetup(int TransformationID)
        {
            Models.FileMapperWizard.FileMapperWizardViewModel myvm = fmwModel;
            Models.FileMapperWizard.StandardTransformationDataModel stdm = fmwModel.standardTransformationDataModel;
            Models.FileMapperWizard.TransformationModel m = new Models.FileMapperWizard.TransformationModel(stdm.FieldMappingName);
            if (TransformationID != null && TransformationID > 0)
            {
                FrameworkUAS.BusinessLogic.Transformation transformationWorker = new FrameworkUAS.BusinessLogic.Transformation();
                FrameworkUAS.Entity.Transformation transformation = new FrameworkUAS.Entity.Transformation();
                transformation = transformationWorker.SelectTransformationByID(TransformationID);

                List<FrameworkUAD_Lookup.Entity.Code> transformationTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
                FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                transformationTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Transformation);

                string type = "";
                if (transformationTypes.Count(x => x.CodeId == transformation.TransformationTypeID) > 0)
                    type = transformationTypes.FirstOrDefault(x => x.CodeId == transformation.TransformationTypeID).DisplayName;

                m.TotalRecordCounts = 1;
                m.TransformationId = transformation.TransformationID;
                m.TransformationName = transformation.TransformationName;
                m.TransformationDescription = transformation.TransformationDescription;
                m.TransformationTypeId = transformation.TransformationTypeID;
                m.TransformationType = type;
                m.MapsPubCode = transformation.MapsPubCode;
                m.LastStepDataMap = transformation.LastStepDataMap;
                m.IsTemplate = transformation.IsTemplate;
                m.IsEdit = true;
            }
            return PartialView("Partials/Transformation/_transformationSetup", m);
        }

        public JsonResult CopyTransformationSetup(int TransformationID, string TransformationName)
        {
            var complete = false;
            var newTID = 0;
            try
            {
                var code = CloneTransformation(TransformationID, out newTID);
                if (Is(Enums.TransformationTypes.Assign_Value, code))
                {
                    CopyAssignValueDetails(TransformationID, newTID);
                }
                else if (Is(Enums.TransformationTypes.Data_Mapping, code))
                {
                    CopyDataMapDetails(TransformationID, newTID);
                }
                else if (Is(Enums.TransformationTypes.Join_Columns, code))
                {
                    CopyJoinTransformDetails(TransformationID, newTID);
                }
                else if (Is(Enums.TransformationTypes.Split_Into_Rows, code))
                {
                    CopyTransformSplitDetails(TransformationID, newTID);
                }
                else if (Is(Enums.TransformationTypes.Split_Transform, code))
                {
                    CopyTransformSplitTransDetails(TransformationID, newTID);
                }
                complete = true;
            }
            catch (Exception ex)
            {
                complete = false;
            }

            return Json(new { Complete = complete, TransformationID = newTID }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteTransformationSetup(int TransformationID, string TransformationName)
        {
            bool complete = false;
            try
            {
                #region Transformation
                FrameworkUAS.BusinessLogic.Transformation transformationWorker = new FrameworkUAS.BusinessLogic.Transformation();
                FrameworkUAS.Entity.Transformation transformation = new FrameworkUAS.Entity.Transformation();
                int tid = transformationWorker.Delete(TransformationID);
                #endregion                

                complete = true;
            }
            catch (Exception ex)
            {
                complete = false;
            }

            return Json(complete, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadTransformationSearch(string TransformationTypeID = "", bool isTemplate = true)
        {
            UAS.Web.Models.FileMapperWizard.TransformationSearchFilteredViewModel tsfvm = new Models.FileMapperWizard.TransformationSearchFilteredViewModel();
            tsfvm.TransformationTypeId = TransformationTypeID;
            return PartialView("Partials/Transformation/_transformationSearch", tsfvm);
        }

        public ActionResult Transformation_Read_Data([DataSourceRequest]DataSourceRequest request, bool IsTemplate = true, int TransformationTypeId = 0, string PageSize = "10", string PageNumber = "1")
        {
            UAS.Web.Helpers.KendoGridHelper<UAS.Web.Models.FileMapperWizard.TransformationModel> gh = new UAS.Web.Helpers.KendoGridHelper<UAS.Web.Models.FileMapperWizard.TransformationModel>();
            List<UAS.Web.Helpers.GridSort> lstgs = gh.GetGridSort(request, "TransformationName");
            string sortColumn = lstgs[0].SortColumnName;
            string sortdirection = lstgs[0].SortDirection;

            List<UAS.Web.Models.FileMapperWizard.TransformationModel> listRange = new List<UAS.Web.Models.FileMapperWizard.TransformationModel>();

            int currentPage = 1;
            int.TryParse(PageNumber, out currentPage);
            int pageSize = 10;
            int.TryParse(PageSize, out pageSize);

            #region Copied From Above
            UAS.Web.Models.FileMapperWizard.TransformationSearchViewModel model = new Models.FileMapperWizard.TransformationSearchViewModel();
            model.transformations = new List<Models.FileMapperWizard.TransformationModel>();

            int TransformTypeId = 0;
            int.TryParse(TransformationTypeId.ToString(), out TransformTypeId);

            FrameworkUAS.BusinessLogic.Transformation transformationWorker = new FrameworkUAS.BusinessLogic.Transformation();
            List<FrameworkUAS.Entity.Transformation> transformations = new List<FrameworkUAS.Entity.Transformation>();

            List<FrameworkUAD_Lookup.Entity.Code> transformationTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
            FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            transformationTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Transformation);

            //Need to remove Split Transformation. Not needed as of MVC
            FrameworkUAD_Lookup.Entity.Code adminTransform = transformationTypes.FirstOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Transform.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase));
            bool isTemplate = IsTemplate;
            bool isActive = true;
            bool ignoreAdminTransformationTypes = true;
            int totalCount = transformationWorker.SelectPagingCount(CurrentClient.ClientID, isTemplate, isActive, TransformationTypeId, ignoreAdminTransformationTypes, adminTransform.CodeId);
            transformations = transformationWorker.SelectPaging(CurrentClient.ClientID, currentPage, pageSize, isTemplate, isActive, TransformationTypeId, ignoreAdminTransformationTypes, adminTransform.CodeId, sortColumn, sortdirection);

            Models.FileMapperWizard.FileMapperWizardViewModel myvm = fmwModel;
            Models.FileMapperWizard.StandardTransformationDataModel stdm = fmwModel.standardTransformationDataModel;

            foreach (FrameworkUAS.Entity.Transformation t in transformations)
            {
                string type = "";
                if (transformationTypes.Count(x => x.CodeId == t.TransformationTypeID) > 0)
                    type = transformationTypes.FirstOrDefault(x => x.CodeId == t.TransformationTypeID).DisplayName;

                UAS.Web.Models.FileMapperWizard.TransformationModel tsvm = new Models.FileMapperWizard.TransformationModel(totalCount, t.TransformationID, t.TransformationName, t.TransformationDescription, type, stdm.FieldMappingName);
                model.transformations.Add(tsvm);
            }
            #endregion

            listRange = model.transformations;

            IQueryable<UAS.Web.Models.FileMapperWizard.TransformationModel> gs = listRange.AsQueryable();
            DataSourceResult result = gs.ToDataSourceResult(request);
            result.Total = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadTransformationSearchResults(bool IsTemplate = true, string TransformationTypeID = "")
        {
            UAS.Web.Models.FileMapperWizard.TransformationSearchViewModel model = new Models.FileMapperWizard.TransformationSearchViewModel();
            model.transformations = new List<Models.FileMapperWizard.TransformationModel>();

            int TransformTypeId = 0;
            int.TryParse(TransformationTypeID, out TransformTypeId);

            FrameworkUAS.BusinessLogic.Transformation transformationWorker = new FrameworkUAS.BusinessLogic.Transformation();
            List<FrameworkUAS.Entity.Transformation> transformations = new List<FrameworkUAS.Entity.Transformation>();
            transformations = transformationWorker.SelectClient(CurrentClient.ClientID, false).Where(x => x.IsTemplate == IsTemplate && x.IsActive == true).OrderBy(x => x.TransformationName).ToList();
            if (TransformTypeId > 0)
                transformations = transformations.Where(x => x.TransformationTypeID == TransformTypeId).ToList();

            List<FrameworkUAD_Lookup.Entity.Code> transformationTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
            FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            transformationTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Transformation);

            //Need to remove Split Transformation. Not needed as of MVC
            FrameworkUAD_Lookup.Entity.Code adminTransformationTypes = transformationTypes.FirstOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Transform.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase));
            if (adminTransformationTypes != null)
                transformations.RemoveAll(x => x.TransformationTypeID == adminTransformationTypes.CodeId);

            Models.FileMapperWizard.FileMapperWizardViewModel myvm = fmwModel;
            Models.FileMapperWizard.StandardTransformationDataModel stdm = fmwModel.standardTransformationDataModel;

            foreach (FrameworkUAS.Entity.Transformation t in transformations)
            {
                string type = "";
                if (transformationTypes.Count(x => x.CodeId == t.TransformationTypeID) > 0)
                    type = transformationTypes.FirstOrDefault(x => x.CodeId == t.TransformationTypeID).DisplayName;

                UAS.Web.Models.FileMapperWizard.TransformationModel tsvm = new Models.FileMapperWizard.TransformationModel(transformations.Count(), t.TransformationID, t.TransformationName, t.TransformationDescription, type, stdm.FieldMappingName);
                model.transformations.Add(tsvm);
            }

            return PartialView("Partials/Transformation/_transformationSearchResults", model);
        }

        public ActionResult LoadTransformationDetail(int transformationTypeId, int transformationId)
        {
            var mapperWizardViewModel = fmwModel;
            var codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            var transformationTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Transformation);
            if (transformationTypes.Any(x => x.CodeId == transformationTypeId))
            {
                var enableTransformationEdit = mapperWizardViewModel.userCreatedTransformations
                                                .transformationIds.Any(x => x == transformationId);

                var transformationType = transformationTypes.First(x => x.CodeId == transformationTypeId);
                if (Is(Enums.TransformationTypes.Assign_Value, transformationType))
                {
                    return PartialAssignValue(transformationId, enableTransformationEdit);
                }

                if (Is(Enums.TransformationTypes.Data_Mapping, transformationType))
                {
                    return PartialChangeValue(transformationId, enableTransformationEdit);
                }

                if (Is(Enums.TransformationTypes.Join_Columns, transformationType))
                {
                    var sourceFileId = 0;
                    int.TryParse(mapperWizardViewModel.setupViewModel.SourceFileId.ToString(), out sourceFileId);
                    return PartialJoinColumns(transformationId, sourceFileId, enableTransformationEdit);
                }

                if (Is(Enums.TransformationTypes.Split_Into_Rows, transformationType))
                {
                    return PartialSplitIntoRows(transformationId, enableTransformationEdit);
                }
            }

            return PartialView(string.Empty, new object());
        }

        public ActionResult Transformation_Search_Filter_TransformationName(List<Models.FileMapperWizard.TransformationModel> tList)
        {
            if (tList == null)
            {
                return Json(TransformationSearchTransformationNames, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(tList.Select(e => e.TransformationName).Distinct(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Transformation_Search_Filter_TransformationDesc(List<Models.FileMapperWizard.TransformationModel> tList)
        {
            if (tList == null)
            {
                return Json(TransformationSearchTransformationDescs, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(tList.Select(e => e.TransformationDescription).Distinct(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save Transformation and Detail
        public JsonResult SaveTransformation(int TransformationID, int TransformationTypeID, string TransformationName, string TransformationDesc, bool IsTemplate, bool MapsPubCode = false, bool LastStepDataMap = false)
        {
            bool complete = true;
            string errorMessage = "";
            try
            {
                #region Transformation
                if (TransformationDesc == "" || TransformationName == "")
                {
                    errorMessage = "<li>Description and/or Transformation Name were blank. Please fill these out and save again.</li>";
                    complete = false;
                }

                FrameworkUAS.BusinessLogic.Transformation transformationWorker = new FrameworkUAS.BusinessLogic.Transformation();
                List<FrameworkUAS.Entity.Transformation> allTrans = transformationWorker.SelectClient(CurrentClient.ClientID, false);
                FrameworkUAS.Entity.Transformation found = allTrans.FirstOrDefault(x => x.TransformationName.Equals(TransformationName, StringComparison.CurrentCultureIgnoreCase));
                if (TransformationID > 0 && found != null)
                {
                    if (found.TransformationID != TransformationID)
                    {
                        errorMessage = "<li>Transformation name exists. Could not rename transformation. Rename and save again.</li>";
                        complete = false;
                    }
                    //MessageBoxResult res = MessageBox.Show("Do you wish to overwrite this transformation?", "Confirm Overwrite", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    //if (res == MessageBoxResult.No)
                    //    return false;
                }
                else if (TransformationID == 0 && found != null)
                {
                    errorMessage = "<li>Transformation name exists. Rename and save again.</li>";
                    complete = false;
                }

                if (complete)
                {
                    //Save to the Database the MappingName and Description
                    FrameworkUAS.Entity.Transformation transformation = new FrameworkUAS.Entity.Transformation();
                    transformation.TransformationID = TransformationID;
                    transformation.TransformationTypeID = TransformationTypeID;
                    transformation.TransformationName = TransformationName;
                    transformation.TransformationDescription = TransformationDesc;
                    transformation.ClientID = CurrentClient.ClientID;
                    transformation.IsActive = true;
                    transformation.MapsPubCode = MapsPubCode;
                    transformation.LastStepDataMap = LastStepDataMap;
                    transformation.DateCreated = DateTime.Now;
                    transformation.DateUpdated = DateTime.Now;
                    transformation.CreatedByUserID = CurrentUser.UserID;
                    transformation.UpdatedByUserID = CurrentUser.UserID;
                    transformation.IsTemplate = IsTemplate;

                    int result = transformationWorker.Save(transformation);
                    if (!(result > 0))
                    {
                        errorMessage = "<li>An error occurred and we could not save.</li>";
                        complete = false;
                    }
                    if (TransformationID == 0 && result > 0)
                    {
                        Models.FileMapperWizard.UserCreatedTransformations uct = fmwModel.userCreatedTransformations;
                        uct.transformationIds.Add(result);

                        Models.FileMapperWizard.FileMapperWizardViewModel myvm = fmwModel;
                        fmwModel.userCreatedTransformations = uct;
                    }

                    TransformationID = result;
                }
                #endregion
            }
            catch (Exception ex)
            {
                complete = false;
            }

            return Json(new { CurrentTransformationID = TransformationID, IsComplete = complete, Message = errorMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveJoinTransformation(int TransformJoinID, int TransformationID, string ColumnsToJoin, string Delimiter, List<string> PubIDs)
        {
            bool complete = true;
            string errorMessage = "";
            int fieldMappingID = 0;
            int sourceFileID = 0;

            #region Validate Data Exists
            if (string.IsNullOrEmpty(Delimiter))
            {
                errorMessage = "<li>Delimiter must be selected. Please select and save again.</li>";
                complete = false;
            }

            PubIDs.Remove("");
            if (PubIDs.Count() < 1)
            {
                errorMessage = "<li>Please select a Pub Code before saving.</li>";
                complete = false;
            }

            if (string.IsNullOrEmpty(ColumnsToJoin))
            {
                errorMessage = "<li>Columns were not set. Please fill these out and save again.</li>";
                complete = false;
            }
            #endregion

            #region TransformJoin
            if (complete)
            {
                var del = CommonEnums.GetDelimiterSymbol(Delimiter).GetValueOrDefault(',').ToString();

                try
                {
                    FrameworkUAS.Entity.TransformJoin x = new FrameworkUAS.Entity.TransformJoin();
                    x.TransformJoinID = TransformJoinID;
                    x.TransformationID = TransformationID;
                    x.ColumnsToJoin = ColumnsToJoin;
                    x.Delimiter = del;
                    x.IsActive = true;
                    x.DateCreated = DateTime.Now;
                    x.DateUpdated = DateTime.Now;
                    x.CreatedByUserID = CurrentUser.UserID;
                    x.UpdatedByUserID = CurrentUser.UserID;

                    FrameworkUAS.BusinessLogic.TransformJoin transformJoinWorker = new FrameworkUAS.BusinessLogic.TransformJoin();
                    int TransJoinID = transformJoinWorker.Save(x);

                    TransformJoinID = TransJoinID;
                }
                catch (Exception ex)
                {
                    errorMessage = "<li>Error saving Join Transformation.</li>";
                    complete = false;
                }
            }
            #endregion
            #region TransformationPubMap
            if (complete)
            {
                errorMessage = ApplyTransformationPubMap(TransformationID, PubIDs);
                if (!string.IsNullOrEmpty(errorMessage))
                    complete = false;

            }
            #endregion
            #region TransformationFieldMap
            if (complete)
            {
                complete = VerifyIfTransformationIsComplete(TransformationID, out errorMessage, out fieldMappingID, out sourceFileID);
            }
            #endregion

            return Json(new { TransformationJoinID = TransformJoinID, IsComplete = complete, Message = errorMessage, SourceFileID = sourceFileID, FieldMappingID = fieldMappingID }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveSplitTransformation(int TransformSplitID, int TransformationID, string Delimiter, List<string> PubIDs)
        {
            bool complete = true;
            string errorMessage = "";
            int fieldMappingID = 0;
            int sourceFileID = 0;

            #region Validate Data Exists
            if (string.IsNullOrEmpty(Delimiter))
            {
                errorMessage = "<li>Delimiter must be selected. Please select and save again.</li>";
                complete = false;
            }

            PubIDs.Remove("");
            if (PubIDs.Count() < 1)
            {
                errorMessage = "<li>Please select a Pub Code before saving.</li>";
                complete = false;
            }
            #endregion

            #region TransformSplit
            if (complete)
            {
                try
                {
                    FrameworkUAS.Entity.TransformSplit x = new FrameworkUAS.Entity.TransformSplit();
                    x.TransformSplitID = TransformSplitID;
                    x.TransformationID = TransformationID;
                    x.Delimiter = Delimiter;
                    x.IsActive = true;
                    x.DateCreated = DateTime.Now;
                    x.DateUpdated = DateTime.Now;
                    x.CreatedByUserID = CurrentUser.UserID;
                    x.UpdatedByUserID = CurrentUser.UserID;

                    FrameworkUAS.BusinessLogic.TransformSplit transformSplitWorker = new FrameworkUAS.BusinessLogic.TransformSplit();
                    int TransSplitID = transformSplitWorker.Save(x);

                    TransformSplitID = TransSplitID;
                }
                catch (Exception ex)
                {
                    errorMessage = "<li>Error saving Join Transformation.</li>";
                    complete = false;
                }
            }
            #endregion
            #region TransformationPubMap
            if (complete)
            {
                errorMessage = ApplyTransformationPubMap(TransformationID, PubIDs);
                if (!string.IsNullOrEmpty(errorMessage))
                    complete = false;

            }
            #endregion
            #region TransformationFieldMap
            if (complete)
            {
                complete = VerifyIfTransformationIsComplete(TransformationID, out errorMessage, out fieldMappingID, out sourceFileID);
            }
            #endregion

            return Json(new { TransformationSplitID = TransformSplitID, IsComplete = complete, Message = errorMessage, SourceFileID = sourceFileID, FieldMappingID = fieldMappingID }, JsonRequestBehavior.AllowGet);
        }

        public bool VerifyIfTransformationIsComplete(int transformationId, out string errorMessage, out int fieldMappingId, out int sourceFileId)
        {
            if (fmwModel == null)
            {
                throw new ArgumentNullException(nameof(fmwModel));
            }

            if (fmwModel.standardTransformationDataModel == null)
            {
                errorMessage = string.Empty;
                fieldMappingId = 0;
                sourceFileId = 0;
                return false;
            }

            int.TryParse(fmwModel.standardTransformationDataModel.FieldMappingId.ToString(), out fieldMappingId);
            int.TryParse(fmwModel.standardTransformationDataModel.SourceFileId.ToString(), out sourceFileId);

            errorMessage = ApplyTransformationFieldMap(transformationId, sourceFileId, fieldMappingId);
            return string.IsNullOrEmpty(errorMessage);
        }

        public JsonResult SaveAssignTransformation(int transformAssignId, int transformationId, string dataMappings)
        {
            var complete = true;
            var errorMessage = string.Empty;
            var fieldMappingId = 0;
            var sourceFileId = 0;

            var productListUad = new FrameworkUAD.BusinessLogic.Product().Select(CurrentClient.ClientConnections);
            var pubIDs = new List<string>();

            if (!string.IsNullOrEmpty(dataMappings))
            {
                List<TransformAssign> taList = null;
                dataMappings = UnescapeDataMappings(dataMappings);
                var jf = new Core_AMS.Utilities.JsonFunctions();
                var mappings = jf.FromJson<UserTransformAssign[]>(dataMappings);
                complete = CheckDataExists(mappings, ref errorMessage, productListUad) &&
                           SetupTransformAssigns(out taList, transformAssignId, transformationId, mappings, pubIDs, ref errorMessage);

                if (complete)
                {
                    ModifyTransformAssigns(transformationId, taList);
                    errorMessage = ApplyTransformationPubMap(transformationId, pubIDs);
                    complete = string.IsNullOrEmpty(errorMessage) &&
                               VerifyIfTransformationIsComplete(transformationId, out errorMessage, out fieldMappingId, out sourceFileId);
                }
            }

            var transformResult = new
            {
                TransformationAssignID = transformAssignId,
                IsComplete = complete,
                Message = errorMessage,
                SourceFileID = sourceFileId,
                FieldMappingID = fieldMappingId
            };
            return Json(transformResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDataMapTransformation(int transformationId, bool mapsProductCode, string dataMappings)
        {
            var successProcessing = true;
            var errorMessage = string.Empty;
            var fieldMappingId = 0;
            var sourceFileId = 0;
            var pubIds = new List<string>();

            if (!string.IsNullOrWhiteSpace(dataMappings))
            {
                dataMappings = UnescapeDataMappings(dataMappings);

                var jf = new JsonFunctions();
                var mappings = jf.FromJson<UserTransformDataMap[]>(dataMappings);

                successProcessing = CheckDataExists(mapsProductCode, mappings, ref errorMessage);
                var tdmList = SaveDataMapTransformSetup(transformationId, mappings, pubIds);

                if (successProcessing)
                {
                    try
                    {
                        var transformDataMapWorker = new FrameworkUAS.BusinessLogic.TransformDataMap();
                        transformDataMapWorker.Select(transformationId).Each(tdm=> transformDataMapWorker.Delete(tdm.TransformDataMapID));
                        tdmList.Each(tdm => transformDataMapWorker.Save(tdm));
                    }
                    catch (Exception ex)
                    {
                        errorMessage = "<li>Error saving Change Value Transformation.</li>";
                        successProcessing = false;
                    }
                }

                if (successProcessing)
                {
                    errorMessage = ApplyTransformationPubMap(transformationId, pubIds);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        successProcessing = false;
                    }
                }

                if (successProcessing)
                {
                    successProcessing = VerifyIfTransformationIsComplete(transformationId,
                        out errorMessage,
                        out fieldMappingId,
                        out sourceFileId);
                }
            }
            else
            {
                errorMessage += "<li>No conditions were added.</li>";
                successProcessing = false;
            }

            var response = new
            {
                IsComplete = successProcessing,
                Message = errorMessage,
                SourceFileID = sourceFileId,
                FieldMappingID = fieldMappingId
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public string ApplyTransformationFieldMap(int TransformationID, int sourceFileID, int fieldMappingID)
        {
            string errorMessage = "";
            bool complete = true;

            #region TransformationFieldMap
            if (complete)
            {
                try
                {
                    if (TransformationID > 0)
                    {
                        if (fieldMappingID > 0)
                        {
                            if (sourceFileID > 0)
                            {
                                FrameworkUAS.BusinessLogic.TransformationFieldMap tfmWorker = new FrameworkUAS.BusinessLogic.TransformationFieldMap();
                                FrameworkUAS.Entity.TransformationFieldMap findCurrent = tfmWorker.Select().FirstOrDefault(x => x.SourceFileID == sourceFileID && x.TransformationID == TransformationID && x.FieldMappingID == fieldMappingID);
                                if (findCurrent == null)
                                {
                                    FrameworkUAS.Entity.TransformationFieldMap tfm = new FrameworkUAS.Entity.TransformationFieldMap()
                                    {
                                        TransformationFieldMapID = 0,
                                        TransformationID = TransformationID,
                                        SourceFileID = sourceFileID,
                                        FieldMappingID = fieldMappingID,
                                        IsActive = true,
                                        DateCreated = DateTime.Now,
                                        DateUpdated = DateTime.Now,
                                        CreatedByUserID = CurrentUser.UserID,
                                        UpdatedByUserID = CurrentUser.UserID
                                    };

                                    int i = tfmWorker.Save(tfm);
                                }
                            }
                            else
                            {
                                errorMessage = "<li>Failed to Add Transformation. Source was unclear.</li>";
                                complete = false;
                            }
                        }
                        else
                        {
                            errorMessage = "<li>Failed to Add Transformation. Column selected was unclear.</li>";
                            complete = false;
                        }
                    }
                    else
                    {
                        errorMessage = "<li>Failed to Add Transformation. Transformation to add unclear.</li>";
                        complete = false;
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = "<li>Error applying transformation.</li>";
                    complete = false;
                }
            }
            #endregion

            return errorMessage;
        }

        public string ApplyTransformationPubMap(int TransformationID, List<string> PubIDs)
        {
            string errorMessage = "";
            bool complete = true;

            #region TransformationPubMap
            try
            {
                List<int> newSavedPubCodes = new List<int>();
                List<int> oldSavedPubCodes = new List<int>();

                #region Save New PubCodes
                FrameworkUAS.BusinessLogic.TransformationPubMap tpmWorker = new FrameworkUAS.BusinessLogic.TransformationPubMap();
                List<FrameworkUAS.Entity.TransformationPubMap> tpmList = tpmWorker.Select(TransformationID);
                oldSavedPubCodes.AddRange(tpmList.Select(x => x.PubID).Distinct().ToList());

                foreach (string pubid in PubIDs)
                {
                    int transformationPubMapID = 0;
                    int selectedPubCode = 0;
                    int.TryParse(pubid, out selectedPubCode);
                    if (selectedPubCode > -1)
                    {
                        newSavedPubCodes.Add(selectedPubCode);
                        FrameworkUAS.Entity.TransformationPubMap t = tpmList.FirstOrDefault(a => a.TransformationID == TransformationID && a.PubID == selectedPubCode);
                        if (t != null)
                            transformationPubMapID = t.TransformationPubMapID;

                        if (transformationPubMapID == 0)
                        {
                            FrameworkUAS.Entity.TransformationPubMap tpm = new FrameworkUAS.Entity.TransformationPubMap()
                            {
                                TransformationPubMapID = 0,
                                TransformationID = TransformationID,
                                PubID = selectedPubCode,
                                IsActive = true,
                                DateCreated = DateTime.Now,
                                DateUpdated = DateTime.Now,
                                CreatedByUserID = CurrentUser.UserID,
                                UpdatedByUserID = CurrentUser.UserID
                            };
                            tpmWorker.Save(tpm);
                        }
                    }
                }
                #endregion
                #region Delete Old PubCodes
                //Delete the old
                List<int> deleteThese = new List<int>();
                deleteThese = oldSavedPubCodes.Except(newSavedPubCodes).ToList();
                if (deleteThese.Count > 0)
                {
                    foreach (int pub in deleteThese)
                    {
                        tpmWorker.Delete(TransformationID, pub);
                    }
                }
                oldSavedPubCodes.Clear();
                newSavedPubCodes.Clear();
                #endregion
            }
            catch (Exception ex)
            {
                errorMessage = "<li>Error associating products with transformation.</li>";
                complete = false;
            }
            #endregion

            return errorMessage;
        }
        #endregion

        #region Delete
        public JsonResult DeleteTransformationAssign(int TransformationId, string IDs)
        {
            bool complete = true;

            try
            {
                #region Delete TransformAssign
                List<string> transformAssignIds = new List<string>();
                transformAssignIds = IDs.Split(',').ToList();

                FrameworkUAS.BusinessLogic.TransformAssign transformAssignWorker = new FrameworkUAS.BusinessLogic.TransformAssign();

                foreach (string id in transformAssignIds)
                {
                    int transformAssignId = 0;
                    int.TryParse(id, out transformAssignId);
                    if (transformAssignId > 0)
                        transformAssignWorker.Delete(transformAssignId);
                }
                #endregion
                #region CleanUp TransformationPubMap
                if (TransformationId > 0)
                {
                    DeleteOldSavedPubs(TransformationId, transformAssignWorker, null);
                }
                #endregion
            }
            catch (Exception ex)
            {
                complete = false;
            }

            return Json(complete, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteTransformationChangeValue(int TransformationId, string IDs)
        {
            bool complete = true;

            try
            {
                #region Delete TransformDataMap
                List<string> transformDataMapIds = new List<string>();
                transformDataMapIds = IDs.Split(',').ToList();

                FrameworkUAS.BusinessLogic.TransformDataMap transformDataMapWorker = new FrameworkUAS.BusinessLogic.TransformDataMap();

                foreach (string id in transformDataMapIds)
                {
                    int transformDataMapId = 0;
                    int.TryParse(id, out transformDataMapId);
                    if (transformDataMapId > 0)
                        transformDataMapWorker.Delete(transformDataMapId);
                }
                #endregion
                #region CleanUp TransformationPubMap
                if (TransformationId > 0)
                {
                    DeleteOldSavedPubs(TransformationId, null, transformDataMapWorker);
                }
                #endregion
            }
            catch (Exception ex)
            {
                complete = false;
            }

            return Json(complete, JsonRequestBehavior.AllowGet);
        }

        public int DeleteOldSavedPubs(
            int transformationId, 
            FrameworkUAS.BusinessLogic.TransformAssign transformAssignWorker, 
            FrameworkUAS.BusinessLogic.TransformDataMap transformDataMapWorker)
        {
            var newSavedPubCodes = new List<int>();
            var oldSavedPubCodes = new List<int>();
            var transformationPubMapWorker = new FrameworkUAS.BusinessLogic.TransformationPubMap();

            var transformationPubMapList = transformationPubMapWorker.Select(transformationId);
            transformationPubMapList.ForEach(pubMap => oldSavedPubCodes.Add(pubMap.PubID));

            if (transformAssignWorker != null)
            {
                var transformAssignWorkerList = transformAssignWorker.Select(transformationId);
                if (transformAssignWorkerList == null)
                {
                    throw new InvalidOperationException(string.Format(
                        "the state of an object {0} cannot support the method call.",
                        nameof(transformAssignWorkerList)));
                }

                transformAssignWorkerList.ForEach(assign => newSavedPubCodes.Add(assign.PubID));
            }

            if (transformDataMapWorker != null)
            {
                var transformDataMapWorkerList = transformDataMapWorker.Select(transformationId);
                if (transformDataMapWorkerList == null)
                {
                    throw new InvalidOperationException(string.Format(
                        "the state of an object {0} cannot support the method call.",
                        nameof(transformDataMapWorkerList)));
                }

                transformDataMapWorkerList.ForEach(dataMap => newSavedPubCodes.Add(dataMap.PubID));
            }

            var deleteThese = oldSavedPubCodes.Except(newSavedPubCodes).ToList();
            foreach (var pub in deleteThese)
            {
                transformationPubMapWorker.Delete(transformationId, pub);
            }

            return deleteThese.Count;
        }
        #endregion
        #endregion

        #region 4 - Transformations
        [HttpGet]
        public ActionResult GetMatchTypes()
        {
            List<SelectListItem> data = new List<SelectListItem>();
            foreach (FrameworkUAD_Lookup.Enums.MatchTypes pt in (FrameworkUAD_Lookup.Enums.MatchTypes[])Enum.GetValues(typeof(FrameworkUAD_Lookup.Enums.MatchTypes)))
            {
                string value = pt.ToString().Replace("_", " ");
                data.Add(new SelectListItem() { Text = value, Value = value });
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllProductsForTransformation()
        {
            KMPlatform.Entity.Client _client = new KMPlatform.Entity.Client();
            if (CurrentClient != null)
            {
                _client = CurrentClient;
            }
            else
            {
                _client = new KMPlatform.BusinessLogic.Client().Select(CurrentClientID);
            }

            List<SelectListItem> products = new List<SelectListItem>();
            products.Add(new SelectListItem() { Text = AllProductsPubCode, Value = "0", Selected = true });
            products.Add(new SelectListItem() { Text = "", Value = "-1" });

            var productListUAD = new FrameworkUAD.BusinessLogic.Product().Select(_client.ClientConnections);
            var prodList = productListUAD.OrderBy(x => x.PubCode).Where(x => x.IsActive).ToList();
            prodList.ForEach(c => products.Add(new SelectListItem() { Text = c.PubCode, Value = c.PubID.ToString() }));

            MultiSelectList productSelectList = new MultiSelectList(products, "Value", "Text", new List<int>() { 0 });

            return Json(productSelectList, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region 5 - Rules
        public ActionResult RulesFormValidate()
        {
            return Json(0);
        }
        public ActionResult OrderRulesValidate()
        {
            return Json(0);
        }
        public ActionResult CheckRuleSetName(string RuleSetName)
        {
            fmwModel.ruleSet.ruleSetName = RuleSetName;
            //return PartialView("Rules/_orderRules", this.fmwModel.rulesViewModel);
            ModelState.Clear();
            return Json(RuleSetName);// PartialView("Rules/_orderRules", this.fmwModel.rulesViewModel);
        }
        public ActionResult CheckRuleName(string RuleName)
        {
            //fmwModel.rulesViewModel.NewRuleName = RuleName;
            ModelState.Clear();
            return Json(RuleName);
        }
        [HttpGet]
        public ActionResult AddNewRule(int SourceFileId, string RuleType, string RuleSetName, bool IsGlobalRuleSet)
        {
            //RuleType = Custom Import Rule	valid types are Insert, Update, Delete, ADMS

            //make sure RuleSetName is created
            if (fmwModel.ruleSet != null)
            {
                #region RuleSet - create / update
                if (fmwModel.setupViewModel.IsNewFile)
                {
                    bool rsNameExists = new FrameworkUAS.BusinessLogic.RuleSet().RuleSetNameExists(RuleSetName, CurrentClient.ClientID);
                    if (fmwModel.ruleSet.ruleSetId == 0 && rsNameExists == false)
                    {
                        #region create new RuleSet
                        FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
                        List<FrameworkUAD_Lookup.Entity.Code> customImportRuleTypes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Custom_Import_Rule);
                        FrameworkUAD_Lookup.Entity.Code cir = customImportRuleTypes.SingleOrDefault(x => x.CodeName.IsCaseInsensitiveEqual(RuleType));

                        //create the new RuleSet - save - set SelectedRuleSetId / NewRuleSetName
                        FrameworkUAS.Entity.RuleSet rs = new FrameworkUAS.Entity.RuleSet();
                        rs.ClientId = fmwModel.client.ClientID;
                        rs.CreatedByUserId = CurrentUser.UserID;
                        rs.CustomImportRuleId = cir.CodeId;
                        rs.DisplayName = RuleSetName;
                        rs.IsActive = true;
                        rs.IsDateSpecific = false;
                        rs.IsGlobal = IsGlobalRuleSet;
                        rs.IsSystem = false;
                        rs.RuleSetName = RuleSetName;

                        FrameworkUAS.BusinessLogic.RuleSet rsWrk = new FrameworkUAS.BusinessLogic.RuleSet();
                        rs.RuleSetId = rsWrk.Save(rs);
                        //add to ExistingRuleSets / TabFilteredRuleSets
                        //fmwModel.rulesViewModel.ExistingRuleSets.Add(rs);
                        //fmwModel.rulesViewModel.SelectedRuleSetId = rs.RuleSetId;
                        //fmwModel.rulesViewModel.NewRuleType = RuleType;
                        //fmwModel.rulesViewModel.NewRuleSetName = RuleSetName;
                        //fmwModel.rulesViewModel.TabFilteredRuleSets.Add(rs);
                        fmwModel.ruleSet.ruleSetId = rs.RuleSetId;
                        fmwModel.ruleSet.ruleSetName = RuleSetName;
                        #endregion
                    }
                    else if (fmwModel.ruleSet.ruleSetId > 0)
                    {
                        #region update RuleSet name / isTemplate
                        //only thing they may have done is change the RuleSetName / IsGlobal
                        if (!fmwModel.ruleSet.ruleSetName.IsCaseInsensitiveEqual(RuleSetName)
                            || fmwModel.ruleSet.isGlobalRuleSet != IsGlobalRuleSet)
                        {
                            FrameworkUAS.BusinessLogic.RuleSet rsWrk = new FrameworkUAS.BusinessLogic.RuleSet();
                            rsWrk.UpdateRuleSet_Name_IsGlobal(fmwModel.ruleSet.ruleSetId, RuleSetName, IsGlobalRuleSet, CurrentUser.UserID);
                        }
                        #endregion
                    }
                }
                else
                {
                    #region Edit mode so load existing
                    //what would we do

                    //this gets us what a new would setup
                    FrameworkUAS.Entity.RuleSet rs = new FrameworkUAS.BusinessLogic.RuleSet().GetRuleSetName(RuleSetName, CurrentClient.ClientID);//fmwModel.ruleSet.ExistingRuleSets.SingleOrDefault(x => x.RuleSetName.IsCaseInsensitiveEqual(RuleSetName));
                    int ruleSetId = 0;
                    if (rs != null) ruleSetId = rs.RuleSetId;

                    fmwModel.ruleSet.ruleSetId = ruleSetId;
                    // fmwModel.ruleSet.ruleType = RuleType;
                    fmwModel.ruleSet.ruleSetName = RuleSetName;
                    //if (rs != null) fmwModel.rulesViewModel.TabFilteredRuleSets.Add(rs);
                    #endregion
                }
                #endregion

                //RuleAction: valid types are Do Not Import, Import, Update New, Delete, Delete All, Update Existing and File, Update Existing, Update File, Update Existing All, Update File All, Update All
                //RuleType = Custom Import Rule	valid types are Insert, Update, Delete, ADMS

                //Models.FileMapperWizard.RulesPostDQMViewModel pvm = new Models.FileMapperWizard.RulesPostDQMViewModel(fmwModel.client, fmwModel.columnMappingViewModel.IncomingColumns, RuleType, SourceFileId, RuleSetName, string.Empty, fmwModel.rulesViewModel.SelectedRuleSetId, 0);
                int ruleCount = new FrameworkUAS.BusinessLogic.RuleSetRuleOrder().GetRuleCount(fmwModel.ruleSet.ruleSetId) + 1;
                string ruleName = "Rule " + ruleCount.ToString();
                //See if I can get RuleType now
                FrameworkUAS.Model.Rule ruleModel = new FrameworkUAS.Model.Rule(ruleName, RuleType, string.Empty, ruleCount, false, fmwModel.ruleSet.ruleSetId, fmwModel.ruleSet.sourceFileId);



                //pvm.NewRuleType = RuleType;
                //pvm.NewRuleAction = string.Empty;//this is unkown as it is set via a drop down on _postDQM.cshtml
                //pvm.RuleActions.Add(new SelectListItem() { Text = "- Select -", Value = "- Select -" });
                //pvm.NewRuleName = "Rule " + ruleCount.ToString();//change this to RuleSet

                //those ending in 'All' do not require conditions
                ruleModel.ruleActions.Add(new SelectListItem() { Text = "- Select -", Value = "0" });
                if (RuleType.IsCaseInsensitiveEqual("Insert"))
                    ruleModel.ruleActions = new FrameworkUAD_Lookup.BusinessLogic.Code().GetDropDownList(FrameworkUAD_Lookup.Enums.CodeType.Custom_Import_Rule, FrameworkUAD_Lookup.Enums.CustomImportRule.Insert.ToString());
                else if (RuleType.IsCaseInsensitiveEqual("Update"))
                    ruleModel.ruleActions = new FrameworkUAD_Lookup.BusinessLogic.Code().GetDropDownList(FrameworkUAD_Lookup.Enums.CodeType.Custom_Import_Rule, FrameworkUAD_Lookup.Enums.CustomImportRule.Update.ToString());
                else if (RuleType.IsCaseInsensitiveEqual("Delete"))
                    ruleModel.ruleActions = new FrameworkUAD_Lookup.BusinessLogic.Code().GetDropDownList(FrameworkUAD_Lookup.Enums.CodeType.Custom_Import_Rule, FrameworkUAD_Lookup.Enums.CustomImportRule.Delete.ToString());
                else if (RuleType.IsCaseInsensitiveEqual("ADMS"))//not used
                {
                    //not used
                    Models.FileMapperWizard.RulesAdmsProcessingViewModel rvm = new Models.FileMapperWizard.RulesAdmsProcessingViewModel(RuleType, SourceFileId);
                    rvm.NewRuleType = RuleType;
                    rvm.NewRuleAction = string.Empty;//NA for ADMS engine rules
                    rvm.SourceFileId = SourceFileId;
                    fmwModel = fmwModel;
                    return PartialView("Rules/_admsProcessing", rvm);
                }

                ruleModel.sourceFileId = SourceFileId;
                //pvm.ExistingRuleId = ExistingRuleId;
                //pvm.NewRuleName = RuleName;

                fmwModel.ruleSet.rules.Add(ruleModel);

                //fmwModel.rulesViewModel.postDQMViewModel = pvm;
                fmwModel = fmwModel;
                return PartialView("Rules/_postDQM", ruleModel);
            }
            else
                return PartialView("Rules/_postDQM");
        }
        [HttpPost]
        public ActionResult EditRule(int ruleSetId, int ruleId)
        {
            FrameworkUAS.BusinessLogic.RuleSet rsWrk = new FrameworkUAS.BusinessLogic.RuleSet();
            FrameworkUAS.BusinessLogic.Rule rWrk = new FrameworkUAS.BusinessLogic.Rule();
            //var rs = rsWrk.GetRuleSetsForSourceFile(sourceFileId).First();
            var myRuleSet = rsWrk.GetRuleSetObject(ruleSetId);
            var myRule = rWrk.GetRule(ruleId);

            //RuleAction:  Insert  Update  Delete ADMS
            FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> customImportRuleTypes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Custom_Import_Rule);
            FrameworkUAD_Lookup.Entity.Code cir = customImportRuleTypes.SingleOrDefault(x => x.CodeId == myRule.CustomImportRuleId);

            List<FrameworkUAD_Lookup.Entity.Code> ruleActions = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Rule_Action);
            FrameworkUAD_Lookup.Entity.Code ruleAction = ruleActions.SingleOrDefault(x => x.CodeId == myRule.RuleActionId);


            //string ruleName = "Rule " + ruleCount.ToString();
            //See if I can get RuleType now
            //need to pass ruleId
            //RuleType = Custom Import Rule	valid types are Insert, Update, Delete, ADMS

            var cr = new FrameworkUAS.BusinessLogic.Rule().GetCustomRule(myRule.RuleId);
            FrameworkUAS.Model.Rule ruleModel = new FrameworkUAS.Model.Rule(myRule.RuleId, myRule.RuleName, cir.DisplayName, ruleAction.DisplayName, cr.ExecutionOrder, myRule.IsGlobal, fmwModel.ruleSet.ruleSetId, fmwModel.ruleSet.sourceFileId);

            //FrameworkUAS.Model.Rule ruleModel = new Models.FileMapperWizard.RulesPostDQMViewModel(fmwModel.client, fmwModel.columnMappingViewModel.IncomingColumns, cir.DisplayName, fmwModel.sourceFileId, myRule.DisplayName, string.Empty, ruleSetId, ruleId);
            //Models.FileMapperWizard.RulesPostDQMViewModel pvm = new Models.FileMapperWizard.RulesPostDQMViewModel(fmwModel.client, fmwModel.columnMappingViewModel.IncomingColumns, cir.DisplayName, fmwModel.sourceFileId, myRule.DisplayName, string.Empty, ruleSetId, ruleId);

            //those ending in 'All' do not require conditions
            ruleModel.ruleActions.Add(new SelectListItem() { Text = "- Select -", Value = "0" });
            if (cir.DisplayName.IsCaseInsensitiveEqual("Insert"))
                ruleModel.ruleActions = new FrameworkUAD_Lookup.BusinessLogic.Code().GetDropDownList(FrameworkUAD_Lookup.Enums.CodeType.Custom_Import_Rule, FrameworkUAD_Lookup.Enums.CustomImportRule.Insert.ToString());
            else if (cir.DisplayName.IsCaseInsensitiveEqual("Update"))
                ruleModel.ruleActions = new FrameworkUAD_Lookup.BusinessLogic.Code().GetDropDownList(FrameworkUAD_Lookup.Enums.CodeType.Custom_Import_Rule, FrameworkUAD_Lookup.Enums.CustomImportRule.Update.ToString());
            else if (cir.DisplayName.IsCaseInsensitiveEqual("Delete"))
                ruleModel.ruleActions = new FrameworkUAD_Lookup.BusinessLogic.Code().GetDropDownList(FrameworkUAD_Lookup.Enums.CodeType.Custom_Import_Rule, FrameworkUAD_Lookup.Enums.CustomImportRule.Delete.ToString());
            else if (cir.DisplayName.IsCaseInsensitiveEqual("ADMS"))//not used
            {
                //not used
                Models.FileMapperWizard.RulesAdmsProcessingViewModel rvm = new Models.FileMapperWizard.RulesAdmsProcessingViewModel(cir.DisplayName, myRuleSet.SourceFileId);
                rvm.NewRuleType = cir.DisplayName;
                rvm.NewRuleAction = string.Empty;//NA for ADMS engine rules
                rvm.SourceFileId = myRuleSet.SourceFileId;
                fmwModel = fmwModel;
                return PartialView("Rules/_admsProcessing", rvm);
            }

            ruleModel.ruleActions.ForEach(x =>
            {
                if (x.Text.ToString().IsCaseInsensitiveEqual(ruleAction.DisplayName))
                    x.Selected = true;
            });
            ruleModel.sourceFileId = myRuleSet.SourceFileId;

            int remIndex = fmwModel.ruleSet.rules.FindIndex(x => x.ruleId == ruleModel.ruleId);
            fmwModel.ruleSet.rules.RemoveAt(remIndex);
            fmwModel.ruleSet.rules.Add(ruleModel);

            fmwModel = fmwModel;
            return PartialView("Rules/_postDQM", ruleModel);
        }
        [HttpGet]
        public ActionResult GetOrderRulesView(string NewRuleType, int SourceFileId)
        {
            fmwModel.ruleSet.ruleTypeTab = NewRuleType;
            ModelState.Clear();
            return PartialView("Rules/_orderRules", this.fmwModel.ruleSet);
        }
        [HttpGet]
        public ActionResult LoadSessionOrderRulesView()
        {
            return PartialView("Rules/_orderRules", fmwModel.ruleSet);
        }

        [HttpPost]
        public ActionResult AddEditInsertCondition(FrameworkUAS.Model.Condition condition)
        {
            condition.databaseFields = new FrameworkUAD.BusinessLogic.FileMappingColumn().GetFields(CurrentClient.ClientID, CurrentClient.ClientConnections);//Field.GetFields();
            condition.mappedFields = new List<FrameworkUAS.Model.Field>();

            fmwModel.columnMappingViewModel.IncomingColumns.ForEach(x =>
            {
                if (!x.MappedColumn.Equals("Ignore", StringComparison.CurrentCultureIgnoreCase))
                {
                    int l = x.MappedColumn.IndexOf(".") + 1;
                    string mc = x.MappedColumn.Substring(l, x.MappedColumn.Length - l);
                    if (condition.databaseFields.Exists(c => c.ColumnName.Equals(mc, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        FrameworkUAS.Model.Field f = condition.databaseFields.Single(c => c.ColumnName.Equals(mc, StringComparison.CurrentCultureIgnoreCase));
                        condition.mappedFields.Add(f);
                    }
                }
            });

            if (!String.IsNullOrEmpty(condition.mafField))
            {
                condition.fieldType = condition.mappedFields.Find(x => x.ColumnName.Equals(condition.mafField, StringComparison.CurrentCultureIgnoreCase)).FieldType;
                condition.fieldDataType = FrameworkUAD_Lookup.Enums.GetFieldDataType(condition.mappedFields.Find(x => x.ColumnName.Equals(condition.mafField, StringComparison.CurrentCultureIgnoreCase)).DataType);
                var opsList = new FrameworkUAD_Lookup.BusinessLogic.Code().GetOperators();

                condition.operators = opsList.FindAll(x => x.fieldDataType == condition.fieldDataType).OrderBy(s => s.sortOrder).ToList();

                if (String.IsNullOrEmpty(condition.Operator))
                {
                    condition.Operator = condition.operators[0].operatorValue;
                }

                switch (condition.fieldType)
                {
                    case FrameworkUAD_Lookup.Enums.FieldType.Lookup_Country:
                        condition.lookupData = FrameworkUAD_Lookup.BusinessLogic.Country.GetCountriesforSelectList();//Country.GetCountriesforSelectList();
                        break;
                    case FrameworkUAD_Lookup.Enums.FieldType.Lookup_State:
                        condition.lookupData = FrameworkUAD_Lookup.BusinessLogic.Region.GetStatesforSelectList();
                        break;
                    case FrameworkUAD_Lookup.Enums.FieldType.Lookup_Transaction:
                        condition.lookupData = FrameworkUAD_Lookup.BusinessLogic.TransactionCode.GetTransactionCodesforSelectList();
                        break;
                    case FrameworkUAD_Lookup.Enums.FieldType.Lookup_Category:
                        condition.lookupData = FrameworkUAD_Lookup.BusinessLogic.CategoryCode.GetCategoryCodesforSelectList();
                        break;
                }
            }

            return PartialView("EditorTemplates/Condition", condition);
        }

        [HttpPost]
        public ActionResult SavePostDQMRule(string Conditions, string NewRuleType, string RuleAction, int SourceFileId, string NewRuleName, int _ruleId, bool IsGlobalRule, string RuleResult)
        {
            Conditions = Conditions.Replace("[", "").Replace("]", "").Replace(@"\", "").Replace("\"0\"", "");
            RuleResult = RuleResult.Replace("[", "").Replace("]", "").Replace(@"\", "").Replace("\"0\"", "");

            //IsGrouped on Conditions - make sure set correctly - coming over as not grouped

            //RuleSet
            int ruleSetId = fmwModel.ruleSet.ruleSetId;//this is good - handled in AddNewRule method
            #region Save Rule - ruleId
            //Rule - if ruleName is not unique for client then make it unique
            FrameworkUAS.BusinessLogic.Rule rWrk = new FrameworkUAS.BusinessLogic.Rule();
            //first time hit save on a new rule
            if (fmwModel.setupViewModel.IsNewFile)
            {
                if (rWrk.IsRuleNameUnique(fmwModel.client.ClientID, NewRuleName) == false && _ruleId == 0)
                    NewRuleName = NewRuleName + "_" + Core_AMS.Utilities.StringFunctions.RandomAlphaString(8);
            }

            FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> customImportRuleTypes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Custom_Import_Rule);
            FrameworkUAD_Lookup.Entity.Code cir = customImportRuleTypes.SingleOrDefault(x => x.CodeName.IsCaseInsensitiveEqual(NewRuleType));

            List<FrameworkUAD_Lookup.Entity.Code> ruleActionTypes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Rule_Action);
            FrameworkUAD_Lookup.Entity.Code ruleAction = ruleActionTypes.SingleOrDefault(x => x.CodeName.IsCaseInsensitiveEqual(RuleAction));

            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            List<Models.FileMapperWizard.RuleConditionViewModel> rcvm = jf.FromJson<List<Models.FileMapperWizard.RuleConditionViewModel>>(Conditions);

            int ruleId = rcvm.First().ruleId;
            if (fmwModel.setupViewModel.IsNewFile)
            {
                FrameworkUAS.Entity.Rule newRule = new FrameworkUAS.Entity.Rule();
                newRule.ClientId = CurrentClient.ClientID;
                newRule.CreatedByUserId = CurrentUser.UserID;
                newRule.CustomImportRuleId = cir.CodeId;
                newRule.DateCreated = DateTime.Now;
                newRule.DisplayName = NewRuleName;
                newRule.IsActive = true;
                if (NewRuleType == FrameworkUAD_Lookup.Enums.CustomImportRule.Delete.ToString())
                    newRule.IsDeleteRule = true;
                else
                    newRule.IsDeleteRule = false;
                newRule.IsGlobal = IsGlobalRule;
                newRule.IsSystem = false;
                //get the ruleAction by RuleAction
                string sfName = new FrameworkUAS.BusinessLogic.SourceFile().SelectSourceFileID(fmwModel.ruleSet.sourceFileId).FileName;

                newRule.RuleActionId = ruleAction.CodeId;
                newRule.RuleDescription = ruleAction.CodeName + " rule created on " + DateTime.Now.ToString() + " by " + CurrentUser.UserName + " for file " + sfName;
                newRule.RuleName = NewRuleName;
                newRule.RuleId = rWrk.Save(newRule);
                ruleId = newRule.RuleId;

                FrameworkUAS.Model.Rule rm = new FrameworkUAS.Model.Rule(newRule.RuleId, newRule.RuleName, NewRuleType, ruleAction.DisplayName, 1, newRule.IsGlobal, fmwModel.ruleSet.ruleSetId, fmwModel.sourceFileId);
                fmwModel.ruleSet.rules.Add(rm);
            }
            else
            {
                //Conditions will have RuleId / LineNumber

            }
            #endregion
            #region Save Rule Conditions
            //RuleCondition
            //pass data via a json object - deserialize and save - add new rule to Model object - return to orderRules view - close pop up window
            List<FrameworkUAD_Lookup.Entity.Code> ruleChains = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Rule_Chain);

            //this will either create or update
            List<FrameworkUAS.Model.Condition> convertToModel = new List<FrameworkUAS.Model.Condition>();
            rcvm.ForEach(x =>
            {
                x.ruleId = ruleId;
                #region Save FrameworkUAS.Entity.RuleCondition
                FrameworkUAS.Entity.RuleCondition rc = new FrameworkUAS.Entity.RuleCondition();
                rc.RuleId = ruleId;
                rc.Line = x.lineNumber;
                rc.IsGrouped = x.isGrouped;
                rc.GroupNumber = x.groupNumber;
                if (x.selectedConnector.Text.Trim() == string.Empty)
                    rc.ChainId = 0;
                else if (x.selectedConnector.Text.Trim().IsCaseInsensitiveEqual("and"))
                    rc.ChainId = ruleChains.Single(r => r.CodeName == "And").CodeId;
                else if (x.selectedConnector.Text.Trim().IsCaseInsensitiveEqual("or"))
                    rc.ChainId = ruleChains.Single(r => r.CodeName == "Or").CodeId;
                if (x.selectedDataBaseField.Value.Contains("."))
                {
                    rc.CompareField = x.selectedDataBaseField.Value.Trim().Split('.')[1];//this should be value which is TableName.FieldName or Delete or Ignore
                    switch (x.selectedDataBaseField.Value.Trim().Split('.')[0])
                    {
                        case "Subscriptions":
                            rc.CompareFieldPrefix = "s";
                            rc.IsClientField = false;
                            break;
                        case "SubscriptionsExtensionMapper":
                            rc.CompareFieldPrefix = "sem";
                            rc.IsClientField = true;
                            break;
                        case "PubSubscriptions":
                            rc.CompareFieldPrefix = "ps";
                            rc.IsClientField = false;
                            break;
                        case "PubSubscriptionsExtensionMapper":
                            rc.CompareFieldPrefix = "psem";
                            rc.IsClientField = true;
                            break;
                        case "ResponseGroups":
                            rc.CompareFieldPrefix = "rg";
                            rc.IsClientField = true;
                            break;
                        case "SubscriberFinal":
                            rc.CompareFieldPrefix = "sf";
                            rc.IsClientField = false;
                            break;
                    }
                }
                else
                {
                    rc.CompareField = x.selectedDataBaseField.Text.Trim();
                    rc.CompareFieldPrefix = string.Empty;
                    rc.IsClientField = false;
                }


                DataTable dt = cWrk.dtGetCode(FrameworkUAD_Lookup.Enums.CodeType.Operators);//make sure this gets cached TODO
                Dictionary<int, string> Operators = new Dictionary<int, string>();
                foreach (DataRow dr in dt.Rows)
                    Operators.Add(Convert.ToInt32(dr["CodeId"].ToString()), dr["DisplayName"].ToString());

                if (Operators.AsQueryable().ToList().Exists(s => s.Value.Equals(x.selectedOperator.Value.Trim(), StringComparison.CurrentCultureIgnoreCase)))
                {
                    var op = Operators.Single(s => s.Value.Equals(x.selectedOperator.Value.Trim()));
                    rc.OperatorId = op.Key;
                }

                // Convert.ToInt32(x.Operators.Single(o => o.Text.IsCaseInsensitiveEqual(x.selectedOperator.Text)).Value)
                //if (Core_AMS.Utilities.StringFunctions.IsNumeric(x.selectedOperator.Value.Trim()))
                //    rc.OperatorId = Convert.ToInt32(x.selectedOperator.Value.Trim());//Convert.ToInt32(x.selectedOperator);//should be codeId
                //else
                //    rc.OperatorId = Convert.ToInt32(x.Operators.Single(o => o.Text.IsCaseInsensitiveEqual(x.selectedOperator.Text.Trim())).Value);

                rc.CompareValue = x.compareValue;
                rc.IsActive = true;
                rc.CreatedDate = DateTime.Now;
                rc.CreatedByUserId = CurrentUser.UserID;

                FrameworkUAS.BusinessLogic.RuleCondition rcW = new FrameworkUAS.BusinessLogic.RuleCondition();
                rcW.Save(rc);
                #endregion



                FrameworkUAS.Model.Condition c = new FrameworkUAS.Model.Condition();
                c.connector = x.selectedConnector.Text.Trim();//and or
                c.mafField = rc.CompareField;
                //c.fieldType;////Profile,Demo,Custom,Lookup_State,Lookup_Country,Lookup_Code,Lookup_Category,Lookup_Transaction
                //c.fieldDataType;//Int,String,Date,Datetime,Time,Float,Decimal,Bit,Lookup,Demo
                if (Operators.AsQueryable().ToList().Exists(s => s.Value.Equals(x.selectedOperator.Value.Trim(), StringComparison.CurrentCultureIgnoreCase)))
                {
                    var op = Operators.Single(s => s.Value.Equals(x.selectedOperator.Value.Trim()));
                    c.Operator = op.Value;
                }
                //c.OperatorFunction;
                c.values = x.compareValue;
                c.lineNumber = x.lineNumber;
                c.isGrouped = x.isGrouped;
                c.ruleId = ruleId;

                c.ruleCondition = rc;


                convertToModel.Add(c);

                //c.lookupData;
                //c.mappedFields;
                //c.operators;
            });

            fmwModel.ruleSet.rules.Single(x => x.ruleId == ruleId).conditions.AddRange(convertToModel);
            #endregion
            #region Save RuleResult - only if an Update
            if (!string.IsNullOrEmpty(RuleResult) && NewRuleType == "Insert" && ruleAction.CodeName.StartsWith("Update"))//need a better flag - check what the NewRuleType is
            {
                List<Models.FileMapperWizard.InsertUpdateNew> rrList = jf.FromJson<List<Models.FileMapperWizard.InsertUpdateNew>>(RuleResult);
                foreach (var r in rrList)
                {
                    //there no longer is RuleField / RuleFieldValue tables - all dynamic and kept in memory

                    //FrameworkUAS.Entity.RuleField rf = new FrameworkUAS.BusinessLogic.RuleField().Select(r.ruleFieldId);

                    FrameworkUAS.BusinessLogic.RuleResult rrWrk = new FrameworkUAS.BusinessLogic.RuleResult();
                    FrameworkUAS.Entity.RuleResult rr = new FrameworkUAS.Entity.RuleResult();
                    rr.RuleId = ruleId;
                    rr.RuleFieldId = r.ruleFieldId;
                    //UpdateField.UpdateFieldPrefix.IsClientField
                    rr.UpdateField = r.columnName;

                    string prefix = "p";
                    switch (r.dataTable)
                    {
                        case "PubSubscriptions":
                            prefix = "ps";
                            break;
                        case "PubSubscriptionsExtensionMapper":
                            prefix = "psem";
                            break;
                        case "ResponseGroups":
                            prefix = "rg";
                            break;
                        case "Subscriptions":
                            prefix = "s";
                            break;
                        case "SubscriptionsExtensionMapper":
                            prefix = "sem";
                            break;
                    }
                    rr.UpdateFieldPrefix = prefix;
                    rr.UpdateFieldValue = r.updateText;//r.updateValue 
                    rr.IsClientField = r.isClientField;//rf.IsClientField;

                    rr.IsActive = true;
                    rr.CreatedDate = DateTime.Now;
                    rr.CreatedByUserId = CurrentUser.UserID;
                    rr.UpdatedByUserId = CurrentUser.UserID;

                    rr.RuleResultId = rrWrk.Save(rr);
                }
            }
            #endregion

            #region Save RuleSet_File_Map
            //RuleSet_File_Map - in setup step 1 ADMS default rules are added
            FrameworkUAS.BusinessLogic.RuleSetFileMap rsfmWrk = new FrameworkUAS.BusinessLogic.RuleSetFileMap();
            rsfmWrk.Save(ruleSetId, SourceFileId, CurrentUser.UserID);
            #endregion

            #region Save RuleSetRuleOrder - mainly turning conditions into a where / Update / Delete statement
            //RuleSetRuleOrder - RuleScript should be full sql where - if update will have update / where - if DELETE will have full delete statement
            //order will be adjusted on _orderRule ui grid - drag/drop
            FrameworkUAS.BusinessLogic.RuleResult ruleResultWrk = new FrameworkUAS.BusinessLogic.RuleResult();
            FrameworkUAS.BusinessLogic.RuleSetRuleOrder rsroWrk = new FrameworkUAS.BusinessLogic.RuleSetRuleOrder();
            FrameworkUAS.Entity.RuleSetRuleOrder rsro = new FrameworkUAS.Entity.RuleSetRuleOrder()
            {
                RuleSetId = ruleSetId,
                RuleId = ruleId,
                ExecutionOrder = rsroWrk.GetExecutionOrder(ruleSetId),
                RuleScript = ruleResultWrk.CreateRuleScript(ruleId).ToString(),
                DateCreated = DateTime.Now,
                CreatedByUserId = CurrentUser.UserID
            };
            rsroWrk.Save(rsro);
            #endregion

            //List<FrameworkUAS.Object.CustomRuleGrid> crGrid = rWrk.GetCustomRuleGrid(ruleSetId);// new List<FrameworkUAS.Object.CustomRuleGrid>();
            //fmwModel.rulesViewModel.CustomRuleGridRules = crGrid;
            //return Json(crGrid, JsonRequestBehavior.AllowGet);
            //UAS.Web.Models.FileMapperWizard.RuleOrderGridViewModel rogModel = new Models.FileMapperWizard.RuleOrderGridViewModel();
            //return PartialView("Rules/_orderHtml", rogModel);//_orderGrid


            FrameworkUAS.Model.RuleSet rsMod = new FrameworkUAS.Model.RuleSet(SourceFileId, fmwModel.setupViewModel.IsFullFile, NewRuleType, ruleSetId, fmwModel.ruleSet.ruleSetName, fmwModel.ruleSet.description, true);
            fmwModel.ruleSet = rsMod;
            return PartialView("Rules/_orderRules", rsMod);

        }
        public ActionResult DeleteCondition(int RuleId, int LineNumber)
        {
            //try to delete the RuleCondition - may not exist
            FrameworkUAS.BusinessLogic.RuleCondition rcWrk = new FrameworkUAS.BusinessLogic.RuleCondition();
            rcWrk.Delete(RuleId, LineNumber);
            return Content(Server.HtmlEncode("success"));
        }

        [HttpPost]//_orderRules
        public ActionResult NewRuleSetFromTemplate(int SourceFileId, int TemplateRuleSetId, string RuleType, string RuleSetName, bool IsGlobalRuleSet)
        {
            //what if they load multiple RuleSet templates??
            FrameworkUAS.BusinessLogic.RuleSet rsWrk = new FrameworkUAS.BusinessLogic.RuleSet();
            bool done = true;
            bool ruleSetNameExists = rsWrk.RuleSetNameExists(RuleSetName, CurrentClient.ClientID);
            FrameworkUAS.Entity.RuleSet rs = new FrameworkUAS.Entity.RuleSet();

            if (fmwModel.ruleSet.ruleSetId == 0 && ruleSetNameExists == false)
            {
                FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
                List<FrameworkUAD_Lookup.Entity.Code> customImportRuleTypes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Custom_Import_Rule);
                FrameworkUAD_Lookup.Entity.Code cir = customImportRuleTypes.SingleOrDefault(x => x.CodeName.IsCaseInsensitiveEqual(RuleType));

                //create the new RuleSet - save - set SelectedRuleSetId / NewRuleSetName
                rs = new FrameworkUAS.Entity.RuleSet();
                rs.ClientId = fmwModel.client.ClientID;
                rs.CreatedByUserId = CurrentUser.UserID;
                rs.CustomImportRuleId = cir.CodeId;
                rs.DisplayName = RuleSetName;
                rs.IsActive = true;
                rs.IsDateSpecific = false;
                rs.IsGlobal = IsGlobalRuleSet;
                rs.IsSystem = false;
                rs.RuleSetName = RuleSetName;
                rs.RuleSetId = rsWrk.Save(rs);

                //create rules for new RuleSet that are on template rule set
                rsWrk.CopyRuleSet(TemplateRuleSetId, rs.RuleSetId, CurrentUser.UserID);//not creating values - run manually works fine.

                //add to ExistingRuleSets / TabFilteredRuleSets
                //fmwModel.rulesViewModel.ExistingRuleSets.Add(rs);
            }
            else if (fmwModel.ruleSet.ruleSetId > 0)
            {
                //only thing they may have done is change the RuleSetName / IsGlobal
                if (!fmwModel.ruleSet.ruleSetName.IsCaseInsensitiveEqual(RuleSetName)
                            || fmwModel.ruleSet.isGlobalRuleSet != IsGlobalRuleSet)
                    rsWrk.UpdateRuleSet_Name_IsGlobal(fmwModel.ruleSet.ruleSetId, RuleSetName, IsGlobalRuleSet, CurrentUser.UserID);

                //copy Rules from this RuleSet to the new one  
                //use case is add RuleSet --> add another RuleSet
                done = rsWrk.CopyRuleSet(TemplateRuleSetId, fmwModel.ruleSet.ruleSetId, CurrentUser.UserID);
            }


            //create our Model.RuleSet
            FrameworkUAS.Model.RuleSet rsMod = new FrameworkUAS.Model.RuleSet(SourceFileId, fmwModel.setupViewModel.IsFullFile, RuleType, rs.RuleSetId, rs.RuleSetName, rs.RuleSetDescription, true);

            fmwModel.ruleSet = rsMod;
            fmwModel.ruleSet.ruleSetId = rs.RuleSetId;
            fmwModel.ruleSet.ruleTypeTab = RuleType;
            fmwModel.ruleSet.ruleSetName = RuleSetName;


            //FrameworkUAS.BusinessLogic.Rule rWrk = new FrameworkUAS.BusinessLogic.Rule();
            //List<FrameworkUAS.Object.CustomRuleGrid> crGrid = new FrameworkUAS.BusinessLogic.Rule().GetCustomRuleGrid(fmwModel.rulesViewModel.SelectedRuleSetId);

            //UAS.Web.Models.FileMapperWizard.RuleOrderGridViewModel rogModel = new UAS.Web.Models.FileMapperWizard.RuleOrderGridViewModel();//new Models.FileMapperWizard.RuleOrderGridViewModel(crGrid);

            //return PartialView("Rules/_orderHtml", rogModel);//_orderGrid


            return PartialView("Rules/_orderRules", rsMod);

        }
        [HttpGet]//_postDQM
        public ActionResult SelectLoadRuleFromTemplate(int ExistingRuleId)
        {
            //just like the save process but it is a copy
            //RuleSet
            int ruleSetId = fmwModel.ruleSet.ruleSetId;//this is good - handled in AddNewRule method

            #region Save Rule - ruleId
            //Rule - if ruleName is not unique for client then make it unique
            FrameworkUAS.BusinessLogic.Rule rWrk = new FrameworkUAS.BusinessLogic.Rule();
            int newRuleId = rWrk.CopyRule(ExistingRuleId, ruleSetId, CurrentUser.UserID);
            //FrameworkUAS.Entity.Rule newRule = rWrk.GetRule(newRuleId);
            //fmwModel.ruleSet.ExistingRules.Add(newRule);
            #endregion

            List<FrameworkUAS.Object.CustomRuleGrid> crGrid = new List<FrameworkUAS.Object.CustomRuleGrid>();// rWrk.GetCustomRuleGrid(fmwModel.rulesViewModel.SelectedRuleSetId);
            fmwModel = fmwModel;
            return Json(crGrid, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadData_CustomRuleGrid([DataSourceRequest]DataSourceRequest request, int ruleSetId)
        {
            //should not get called - no longer used
            if (ruleSetId != fmwModel.ruleSet.ruleSetId)
                ruleSetId = fmwModel.ruleSet.ruleSetId;
            FrameworkUAS.BusinessLogic.Rule ruleWrk = new FrameworkUAS.BusinessLogic.Rule();
            List<FrameworkUAS.Object.CustomRuleGrid> crGrid = new List<FrameworkUAS.Object.CustomRuleGrid>();//ruleWrk.GetCustomRuleGrid(ruleSetId);
            //fmwModel.ruleSet.CustomRuleGridRules = crGrid;
            fmwModel = fmwModel;
            DataSourceResult result = crGrid.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateRuleExecutionOrder(int RuleSetId, int RuleId, int SortOrder)
        {
            FrameworkUAS.BusinessLogic.RuleSetRuleOrder rsroWrk = new FrameworkUAS.BusinessLogic.RuleSetRuleOrder();
            bool done = rsroWrk.UpdateExecutionOrder(RuleSetId, RuleId, SortOrder);
            //FrameworkUAS.BusinessLogic.Rule rWrk = new FrameworkUAS.BusinessLogic.Rule();

            fmwModel.ruleSet.rules = new FrameworkUAS.BusinessLogic.Model().RulesGetRuleSet(RuleSetId, fmwModel.ruleSet.sourceFileId);
            //List<FrameworkUAS.Object.CustomRuleGrid> crGrid = new List<FrameworkUAS.Object.CustomRuleGrid>();// rWrk.GetCustomRuleGrid(RuleSetId);
            //UAS.Web.Models.FileMapperWizard.RuleOrderGridViewModel rogModel = new Models.FileMapperWizard.RuleOrderGridViewModel(crGrid);
            //fmwModel.rulesViewModel.CustomRuleGridRules = crGrid;
            fmwModel = fmwModel;
            //return Json(crGrid, JsonRequestBehavior.AllowGet);
            //return PartialView("Rules/_orderHtml", rogModel);//_orderGrid
            return PartialView("Rules/_orderRules", fmwModel.ruleSet);//_orderGrid

        }
        [HttpPost]
        public ActionResult DeleteRule(int ruleSetId, int ruleId)
        {
            FrameworkUAS.BusinessLogic.RuleSetRuleOrder rsroWrk = new FrameworkUAS.BusinessLogic.RuleSetRuleOrder();
            bool done = rsroWrk.DeleteRule(ruleSetId, ruleId, CurrentUser.UserID);
            //FrameworkUAS.BusinessLogic.Rule rWrk = new FrameworkUAS.BusinessLogic.Rule();
            //List<FrameworkUAS.Object.CustomRuleGrid> crGrid = new List<FrameworkUAS.Object.CustomRuleGrid>();// rWrk.GetCustomRuleGrid(ruleSetId);
            //UAS.Web.Models.FileMapperWizard.RuleOrderGridViewModel rogModel = new Models.FileMapperWizard.RuleOrderGridViewModel(crGrid);
            //fmwModel.rulesViewModel.CustomRuleGridRules = crGrid;

            fmwModel.ruleSet.rules = new FrameworkUAS.BusinessLogic.Model().RulesGetRuleSet(ruleSetId, fmwModel.ruleSet.sourceFileId);
            fmwModel = fmwModel;
            // return Json(crGrid, JsonRequestBehavior.AllowGet);
            //return PartialView("Rules/_orderHtml", rogModel);//_orderGrid
            return PartialView("Rules/_orderRules", fmwModel.ruleSet);
        }
        [HttpGet]
        public ActionResult GetInsertUpdateNewGrid(int ruleFieldId)//string dataTableField
        {
            //if (!string.IsNullOrEmpty(dataTableField))
            //{
            //    FrameworkUAS.Entity.RuleField rf = new FrameworkUAS.BusinessLogic.RuleField().Select(CurrentClient.ClientID, dataTableField.Split('.')[0].ToString(), dataTableField.Split('.')[1].ToString(), true);
            //    UAS.Web.Models.FileMapperWizard.InsertUpdateNewModel um = new Models.FileMapperWizard.InsertUpdateNewModel();
            //    var newItem = new Models.FileMapperWizard.InsertUpdateNew()
            //    {
            //        dataTableColumnName = dataTableField,
            //        uiControl = rf.UIControl,
            //        isClientField = rf.IsClientField,
            //        dataType = rf.DataType,
            //        ruleFieldId = rf.RuleFieldId,
            //        isMultiSelect = rf.IsMultiSelect,
            //        updateText = string.Empty,
            //        updateValue = string.Empty
            //    };

            //    fmwModel.rulesViewModel.postDQMViewModel.insertUpdateNewModel.updateList.Add(newItem);
            //}

            //FrameworkUAS.Entity.RuleField rf = new FrameworkUAS.BusinessLogic.RuleField().Select(ruleFieldId);
            UAS.Web.Models.FileMapperWizard.InsertUpdateNewModel um = new Models.FileMapperWizard.InsertUpdateNewModel();
            //var newItem = new Models.FileMapperWizard.InsertUpdateNew()
            //{
            //    dataTableColumnName = rf.DataTable + "." + rf.Field,
            //    uiControl = rf.UIControl,
            //    isClientField = rf.IsClientField,
            //    dataType = rf.DataType,
            //    ruleFieldId = rf.RuleFieldId,
            //    isMultiSelect = rf.IsMultiSelect,
            //    updateText = string.Empty,
            //    updateValue = string.Empty
            //};

            //fmwModel.rulesViewModel.postDQMViewModel.insertUpdateNewModel.updateList.Add(newItem);
            return PartialView("Rules/_insertUpdateNew");//, fmwModel.rulesViewModel.postDQMViewModel.insertUpdateNewModel);
        }
        #region Shared - EditorTemplates - DropDownList Get methods
        //Ajax Methods for DropDownLists
        [HttpGet]
        public ActionResult GetOperators(string DbField)
        {
            //List<SelectListItem> values = new List<SelectListItem>();
            List<Models.FileMapperWizard.RuleSelectListItem> values = new List<Models.FileMapperWizard.RuleSelectListItem>();
            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = "- Select -", Value = "0", RuleFieldId = 0, UIControl = "textbox" });
            //FrameworkUAS.Entity.RuleField myRF = null;
            ViewBag.myUIControl = "textbox";//default

            #region dynamically get operators based on DbField
            if (!string.IsNullOrEmpty(DbField))
            {
                //jwag:5/10/17
                //get the operators setup for the selected DB Field
                //need to know field type - lets call RuleField with 0/DbField 
                //FrameworkUAS.BusinessLogic.RuleField rfWrk = new FrameworkUAS.BusinessLogic.RuleField();
                //myRF = rfWrk.Select(0, DbField, true);



                //if no result then call CurrentClient.ClientId/DbField (need to know if adhoc or demo) 
                //---- could have a duplicate name between an adhoc and demo - doubtful but possible
                //---- if becomes an issue a quick fix is add TOP 1 to the select sproc
                //if (myRF == null || myRF.RuleFieldId == 0)
                //    myRF = rfWrk.Select(CurrentClient.ClientID, DbField, true);

                //if (myRF != null && myRF.RuleFieldId > 0)
                //{
                //    #region  now based on the DataType of the RuleField add each operator
                //    switch (myRF.DataType)
                //    {
                //        case "BIT":
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            break;
                //        case "DATE":
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than_or_equal_to.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than_or_equal_to.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than_or_equal_to.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than_or_equal_to.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.range.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.range.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            break;
                //        case "DATETIME":
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than_or_equal_to.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than_or_equal_to.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than_or_equal_to.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than_or_equal_to.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.range.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.range.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            break;
                //        case "DECIMAL":
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_null.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_null.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not_null.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not_null.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            break;
                //        case "INT":
                //            #region specific fields
                //            if (DbField.IsCaseInsensitiveEqual("EmailStatusID")
                //               || DbField.IsCaseInsensitiveEqual("Par3CID")
                //               || DbField.IsCaseInsensitiveEqual("PubCategoryID")
                //               || DbField.IsCaseInsensitiveEqual("PubQSourceID")
                //               || DbField.IsCaseInsensitiveEqual("PubTransactionID")
                //               || DbField.IsCaseInsensitiveEqual("SubscriptionStatusID")
                //               || DbField.IsCaseInsensitiveEqual("SubsrcID")
                //               || string.IsNullOrEmpty(DbField))
                //            {
                //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_null.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_null.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not_null.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not_null.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            }
                //            else if (DbField.IsCaseInsensitiveEqual("Copies")
                //               || DbField.IsCaseInsensitiveEqual("GraceIssues"))
                //            {
                //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than_or_equal_to.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than_or_equal_to.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than_or_equal_to.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than_or_equal_to.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.range.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.range.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            }
                //            #endregion
                //            break;
                //        case "VARCHAR":
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.contains.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.contains.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.starts_with.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.starts_with.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.ends_with.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.ends_with.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = "does " + FrameworkUAD_Lookup.Enums.OperatorTypes.not_contain.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.not_contain.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_null.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_null.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not_null.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not_null.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.in_.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.in_.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.not_in.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.not_in.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
                //            break;
                //    }
                //    #endregion

                //    #region set Value UI code variable
                //    //on the ui controls add attribute for RuleFieldId
                //    ViewBag.myRuleFieldId = myRF.RuleFieldId;
                //    ViewBag.myUIControl = myRF.UIControl;
                //    #endregion
                //}
            }
            #endregion

            if (values.Count == 0)
            {
                FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
                DataTable dt = cWrk.dtGetCode(FrameworkUAD_Lookup.Enums.CodeType.Operators);
                foreach (DataRow dr in dt.Rows)
                {
                    var newItem = new Models.FileMapperWizard.RuleSelectListItem() { Text = dr["DisplayName"].ToString(), Value = dr["DisplayName"].ToString(), RuleFieldId = 0, UIControl = string.Empty };
                    values.Add(newItem);
                }
            }

            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetOperatorsRuleFieldId(int ruleFieldId)
        {
            //List<SelectListItem> values = new List<SelectListItem>();
            List<Models.FileMapperWizard.RuleSelectListItem> values = new List<Models.FileMapperWizard.RuleSelectListItem>();
            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = "- Select -", Value = "0", RuleFieldId = 0, UIControl = "textbox" });
            FrameworkUAS.BusinessLogic.RuleField rfWrk = new FrameworkUAS.BusinessLogic.RuleField();
            //FrameworkUAS.Entity.RuleField myRF = rfWrk.Select(ruleFieldId);
            ViewBag.myUIControl = "textbox";//default

            #region dynamically get operators based on DbField
            //if (myRF != null && myRF.RuleFieldId > 0)
            //{
            //    #region  now based on the DataType of the RuleField add each operator
            //    switch (myRF.DataType)
            //    {
            //        case "BIT":
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            break;
            //        case "DATE":
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than_or_equal_to.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than_or_equal_to.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than_or_equal_to.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than_or_equal_to.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.range.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.range.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            break;
            //        case "DATETIME":
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than_or_equal_to.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than_or_equal_to.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than_or_equal_to.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than_or_equal_to.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.range.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.range.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            break;
            //        case "DECIMAL":
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_null.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_null.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not_null.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not_null.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            break;
            //        case "INT":
            //            #region specific fields
            //            if (myRF.Field.IsCaseInsensitiveEqual("EmailStatusID")
            //               || myRF.Field.IsCaseInsensitiveEqual("Par3CID")
            //               || myRF.Field.IsCaseInsensitiveEqual("PubCategoryID")
            //               || myRF.Field.IsCaseInsensitiveEqual("PubQSourceID")
            //               || myRF.Field.IsCaseInsensitiveEqual("PubTransactionID")
            //               || myRF.Field.IsCaseInsensitiveEqual("SubscriptionStatusID")
            //               || myRF.Field.IsCaseInsensitiveEqual("SubsrcID")
            //               || string.IsNullOrEmpty(myRF.Field))
            //            {
            //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_null.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_null.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not_null.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not_null.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            }
            //            else if (myRF.Field.IsCaseInsensitiveEqual("Copies")
            //               || myRF.Field.IsCaseInsensitiveEqual("GraceIssues"))
            //            {
            //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than_or_equal_to.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.greater_than_or_equal_to.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than_or_equal_to.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.less_than_or_equal_to.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //                values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.range.ToString().Replace("_", ""), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.range.ToString().Replace("_", ""), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            }
            //            #endregion
            //            break;
            //        case "VARCHAR":
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.contains.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.contains.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.starts_with.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.starts_with.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.ends_with.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.ends_with.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = "does " + FrameworkUAD_Lookup.Enums.OperatorTypes.not_contain.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.not_contain.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_null.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_null.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not_null.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.is_not_null.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.in_.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.in_.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            values.Add(new Models.FileMapperWizard.RuleSelectListItem() { Text = FrameworkUAD_Lookup.Enums.OperatorTypes.not_in.ToString().Replace("_", " "), Value = FrameworkUAD_Lookup.Enums.OperatorTypes.not_in.ToString().Replace("_", " "), RuleFieldId = myRF.RuleFieldId, UIControl = myRF.UIControl });
            //            break;
            //    }
            //    #endregion

            //    #region set Value UI code variable
            //    //on the ui controls add attribute for RuleFieldId
            //    ViewBag.myRuleFieldId = myRF.RuleFieldId;
            //    ViewBag.myUIControl = myRF.UIControl;
            //    #endregion
            //}

            #endregion

            if (values.Count == 0)
            {
                FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
                DataTable dt = cWrk.dtGetCode(FrameworkUAD_Lookup.Enums.CodeType.Operators);
                foreach (DataRow dr in dt.Rows)
                {
                    var newItem = new Models.FileMapperWizard.RuleSelectListItem() { Text = dr["DisplayName"].ToString(), Value = dr["DisplayName"].ToString(), RuleFieldId = 0, UIControl = string.Empty };
                    values.Add(newItem);
                }
            }

            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetRuleSetTemplates()
        {
            List<FrameworkUAS.Entity.RuleSet> ruleSets = new FrameworkUAS.BusinessLogic.RuleSet().GetRuleSetsForClient(fmwModel.client.ClientID).ToList();
            List<SelectListItem> data = new List<SelectListItem>();
            foreach (var i in ruleSets)
                data.Add(new SelectListItem() { Value = i.RuleSetId.ToString(), Text = i.RuleSetName });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetRuleFieldValue(int RuleFieldId)
        {
            List<SelectListItem> values = new List<SelectListItem>();
            //List<FrameworkUAS.Entity.RuleFieldPredefinedValue> rfpValues = new List<FrameworkUAS.Entity.RuleFieldPredefinedValue>();
            //FrameworkUAS.BusinessLogic.RuleFieldPredefinedValue rfpWrk = new FrameworkUAS.BusinessLogic.RuleFieldPredefinedValue();
            //rfpValues = new FrameworkUAS.BusinessLogic.RuleFieldPredefinedValue().Select(RuleFieldId);

            //foreach (var rfp in rfpValues)
            //    values.Add(new SelectListItem() { Text = rfp.ItemText, Value = rfp.ItemValue });

            return Json(values, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public ActionResult GetRuleFields()
        //{
        //    List<FrameworkUAS.Object.RuleFieldSelectListItem> data = new FrameworkUAS.BusinessLogic.RuleField().GetDropDownList(CurrentClient.ClientID);
        //    //foreach (var i in fmwModel.rulesViewModel.postDQMViewModel.DataBaseColumns)//changed to include demographics - 5/30/17 19:50
        //    //    data.Add(new SelectListItem() { Text = i.ColumnName, Value = i.DataTable + "." + i.ColumnName });
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public ActionResult GetUiControlRuleFieldId(int ruleFieldId)
        //{
        //    FrameworkUAS.Entity.RuleField rf = new FrameworkUAS.BusinessLogic.RuleField().Select(ruleFieldId);
        //    return Json(rf.UIControl, JsonRequestBehavior.AllowGet);//doing this cause the template editor then should have correct uiControl value
        //}
        //[HttpGet]
        //public ActionResult GetUiControlDataTableField(string dataTableField)
        //{
        //    FrameworkUAS.Entity.RuleField rf = new FrameworkUAS.BusinessLogic.RuleField().Select(CurrentClient.ClientID, dataTableField.Split('.')[0], dataTableField.Split('.')[1], true);
        //    return Json(new { rf.UIControl, rf.RuleFieldId }, JsonRequestBehavior.AllowGet);//doing this cause the template editor then should have correct uiControl value
        //}
        [HttpGet]
        public ActionResult GetConnectors()
        {
            var data = new List<SelectListItem>();
            data.Add(new SelectListItem() { Text = "", Value = "" });
            data.Add(new SelectListItem() { Text = "And", Value = "And" });
            data.Add(new SelectListItem() { Text = "Or", Value = "Or" });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetIncomingField()
        {
            List<SelectListItem> data = new List<SelectListItem>();
            foreach (var i in fmwModel.columnMappingViewModel.IncomingColumns.Where(x => x.MappedColumn != "Ignored").ToList())//fmwModel.rulesViewModel.postDQMViewModel.IncomingColumns)
                data.Add(new SelectListItem() { Value = i.SourceColumn, Text = i.SourceColumn });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetDataBaseFields()
        {
            List<SelectListItem> data = new List<SelectListItem>();
            foreach (var i in fmwModel.columnMappingViewModel.IncomingColumns)//.postDQMViewModel.DataBaseColumns)//changed to include demographics - 5/30/17 19:50
                data.Add(new SelectListItem() { Text = i.MappedColumn, Value = i.MappedColumn });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetExistingRules(int sourceFileId, int ruleSetId)
        {
            List<SelectListItem> data = new List<SelectListItem>();
            var rules = new FrameworkUAS.BusinessLogic.Model().RulesGetClientTemplates(CurrentClient.ClientID, sourceFileId);//.RulesGetRuleSet(ruleSetId, fmwModel.sourceFileId);
            foreach (var i in rules.Where(x => x.isTemplateRule == true).ToList())
            {
                if (!data.Exists(x => x.Text.IsCaseInsensitiveEqual(i.ruleName)))
                    data.Add(new SelectListItem() { Value = i.ruleId.ToString(), Text = i.ruleName });
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        //from _review.cshtml - previous button function call 
        [HttpGet]
        public ActionResult LoadRuleSet(int sourceFileId)
        {
            //get the data by sourceFileId
            if (fmwModel.ruleSet.sourceFileId != sourceFileId)
            {
                //Models.FileMapperWizard.RulesViewModel rVM = new Models.FileMapperWizard.RulesViewModel();
                FrameworkUAS.Model.RuleSet rs = new FrameworkUAS.Model.RuleSet();
                if (fmwModel.sourceFileId != sourceFileId)
                    fmwModel.sourceFileId = sourceFileId;
                rs.sourceFileId = fmwModel.sourceFileId;
                //FrameworkUAS.BusinessLogic.SourceFile sfWrk = new FrameworkUAS.BusinessLogic.SourceFile();
                //rVM.sourceFile = sfWrk.SelectSourceFileID(rVM.SourceFileId);
                //FrameworkUAS.BusinessLogic.Rule rWrk = new FrameworkUAS.BusinessLogic.Rule();
                //rVM.ExistingRules = rWrk.GetRulesForClient(fmwModel.client.ClientID).ToList();
                //FrameworkUAS.BusinessLogic.RuleSet rsWrk = new FrameworkUAS.BusinessLogic.RuleSet();
                //rVM.ExistingRuleSets = rsWrk.GetRuleSetsForClient(fmwModel.client.ClientID).ToList();

                fmwModel.ruleSet = rs;
                fmwModel = fmwModel;
            }
            //return PartialView("Partials/_rules", fmwModel.rulesViewModel);
            SetStep(3);
            return PartialView("Partials/_rules");
        }

        [HttpPost]
        public ActionResult LoadMapping(int SourceFileId)
        {
            string FilePath = "";
            if (fmwModel.setupViewModel.IncomingFile != null)
                FilePath = fmwModel.setupViewModel.IncomingFile.FullName;

            string Delimeter = fmwModel.setupViewModel.Delimeter;
            bool HasQuotation = fmwModel.setupViewModel.HasQuotation;

            Models.FileMapperWizard.ColumnMappingViewModel cVM = new Models.FileMapperWizard.ColumnMappingViewModel(false, FilePath, SourceFileId, fmwModel.client, Delimeter, HasQuotation, CurrentUser.UserID);
            fmwModel.columnMappingViewModel = cVM;

            fmwModel = fmwModel;
            ViewBag.SetupSuccess = true;
            SetStep(2);
            return PartialView("Partials/_columnMapping", cVM);
        }

        [HttpPost]
        public ActionResult GoToReview(int SelectedRuleSetId, int SourceFileId, string RuleSetName, bool IsGlobalRuleSet)
        {
            if (!string.IsNullOrEmpty(RuleSetName))
            {
                FrameworkUAS.BusinessLogic.RuleSet rsWrk = new FrameworkUAS.BusinessLogic.RuleSet();
                rsWrk.UpdateRuleSet_Name_IsGlobal(SelectedRuleSetId, RuleSetName, IsGlobalRuleSet, CurrentUser.UserID);
            }

            //FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
            //FrameworkUAS.BusinessLogic.TransformationFieldMap tfmWorker = new FrameworkUAS.BusinessLogic.TransformationFieldMap();
            //FrameworkUAS.BusinessLogic.Transformation tWorker = new FrameworkUAS.BusinessLogic.Transformation();
            //FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();

            //List<FrameworkUAS.Entity.FieldMapping> fieldMappingList = fmWorker.Select(SourceFileId, true);
            //List<FrameworkUAS.Entity.TransformationFieldMap> transformationFieldMappingList = tfmWorker.Select(SourceFileId);
            //List<FrameworkUAS.Entity.Transformation> transformationList = tWorker.SelectClient(CurrentClient.ClientID, false);
            //List<FrameworkUAD_Lookup.Entity.Code> transformationTypeList = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Transformation);
            //List<FrameworkUAD_Lookup.Entity.Code> demoUpdateList = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);
            //List<FrameworkUAD_Lookup.Entity.Code> fieldMappingTypeList = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping);

            //Models.FileMapperWizard.ReviewViewModel rvm = new Models.FileMapperWizard.ReviewViewModel(SourceFileId, fieldMappingList, transformationFieldMappingList, transformationList,
            //                                                                                transformationTypeList, demoUpdateList, fieldMappingTypeList);

            //fmwModel.reviewViewModel = rvm;
            //FrameworkUAS.BusinessLogic.SourceFile sfWrk = new FrameworkUAS.BusinessLogic.SourceFile();
            ////everything should be saved - just get the review model and display it
            //fmwModel.reviewViewModel.SourceFileId = SourceFileId;
            //SetStep(4);
            //return PartialView("Partials/_review", fmwModel.reviewViewModel);


            fmwModel = null;
            //return PartialView("Partials/_review", fmwModel.reviewViewModel);
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region 6 - Review
        public ActionResult Review(Models.FileMapperWizard.FileMapperWizardViewModel fmwModel)
        {
            FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
            FrameworkUAS.BusinessLogic.TransformationFieldMap tfmWorker = new FrameworkUAS.BusinessLogic.TransformationFieldMap();
            FrameworkUAS.BusinessLogic.Transformation tWorker = new FrameworkUAS.BusinessLogic.Transformation();
            FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();

            int SourceFileId = fmwModel.sourceFileId;
            List<FrameworkUAS.Entity.FieldMapping> fieldMappingList = fmWorker.Select(SourceFileId, true);
            List<FrameworkUAS.Entity.TransformationFieldMap> transformationFieldMappingList = tfmWorker.Select(SourceFileId);
            List<FrameworkUAS.Entity.Transformation> transformationList = tWorker.SelectClient(CurrentClient.ClientID, false);
            List<FrameworkUAD_Lookup.Entity.Code> transformationTypeList = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Transformation);
            List<FrameworkUAD_Lookup.Entity.Code> demoUpdateList = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);
            List<FrameworkUAD_Lookup.Entity.Code> fieldMappingTypeList = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping);

            Models.FileMapperWizard.ReviewViewModel rvm = new Models.FileMapperWizard.ReviewViewModel(SourceFileId, fieldMappingList, transformationFieldMappingList, transformationList,
                                                                                            transformationTypeList, demoUpdateList, fieldMappingTypeList);
            return PartialView("Partials/_review", rvm);
        }
        public ActionResult ReviewSave()
        {
            //save everything then destroy the Session object

            fmwModel = null;
            //return PartialView("Partials/_review", fmwModel.reviewViewModel);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Helpers
        [HttpPost]
        public ActionResult CheckFileName(int ClientId, string FileName, bool IsNewFile)
        {
            int sfId = GetSourceFileId(FileName, IsNewFile, ClientId);
            fmwModel.setupViewModel.SourceFileId = sfId;
            fmwModel.setupViewModel.FileSaveAsName = FileName;
            string FullName = "";

            if (IsNewFile)
            {
                FullName = fmwModel.setupViewModel.IncomingFile.FullName;
                ViewBag.Ext = fmwModel.setupViewModel.IncomingFile.Extension.Replace(".", string.Empty);// ext;
            }
            else
                ViewBag.Ext = fmwModel.setupViewModel.Extension.Replace(".", string.Empty);// ext;


            string error = string.Empty;
            if (sfId == 0)
            {
                error = "File name is already in use";
                ViewBag.SetupSuccess = false;
                SetStep(1);
                Models.Common.UASError er = new Models.Common.UASError() { ErrorMessage = error };
                if (!fmwModel.setupViewModel.ErrorList.Exists(x => x.ErrorMessage.IsCaseInsensitiveEqual(error)))
                    fmwModel.setupViewModel.ErrorList.Add(er);
            }

            //return PartialView("Partials/_setup", fmwModel.setupViewModel);
            return Json(new { status = sfId.ToString() + "|" + FullName + "|" + FileName + "|" + ViewBag.Ext + "|" + error }, "text/plain");
        }
        private int GetSourceFileId(string FileName, bool isNewFile, int ClientId = 0)
        {
            fmwModel.setupViewModel.IsNewFile = isNewFile;
            //going to need to check IsNewFile - if yes check name
            //if user selects existings then on UI need to get a ddl and load existing files into a ddl for selection
            if (ClientId == 0)
                ClientId = fmwModel.client.ClientID;

            int sourceFileId = 0;
            KeyValuePair<string, string> kvp = GetFileName(FileName);
            FrameworkUAS.BusinessLogic.SourceFile sfWrk = new FrameworkUAS.BusinessLogic.SourceFile();

            if (!isNewFile)//edit mode
            {
                sourceFileId = sfWrk.Select(ClientId, FileName).SourceFileID;
            }
            else
            {
                bool isUnique = sfWrk.IsFileNameUnique(ClientId, kvp.Key);
                if (isUnique)
                {
                    FrameworkUAS.Entity.SourceFile sf = new FrameworkUAS.Entity.SourceFile();
                    sf.ClientID = ClientId;
                    sf.CreatedByUserID = CurrentUser.UserID;
                    sf.DateCreated = DateTime.Now;
                    sf.FileName = kvp.Key;
                    sf.IsDeleted = true;
                    if (!string.IsNullOrEmpty(kvp.Value))
                        sf.Extension = kvp.Value;

                    sourceFileId = sfWrk.Save(sf);//when save SF will create the default ADMS rules in RuleSet_File_Map - these use system RuleSetId
                    fmwModel.sourceFileId = sourceFileId;
                    if (fmwModel.ruleSet == null)
                        fmwModel.ruleSet = new FrameworkUAS.Model.RuleSet(); //new Models.FileMapperWizard.RulesViewModel();
                    fmwModel.ruleSet.sourceFileId = sourceFileId;
                    //fmwModel.rulesViewModel.sourceFile = sf;
                    if (fmwModel.setupViewModel == null)
                        fmwModel.setupViewModel = new Models.FileMapperWizard.SetupViewModel();
                    fmwModel.setupViewModel.SourceFileId = sourceFileId;
                }
            }
            return sourceFileId;
        }
        public KeyValuePair<string, string> GetFileName(string fullFileName)
        {
            string fName = fullFileName;
            string ext = string.Empty;

            int lastIndex = fullFileName.LastIndexOf('.');
            if (lastIndex > 0)
            {
                int length = fullFileName.Length;
                int take = length - lastIndex;
                ext = fullFileName.Substring(lastIndex, take);
                fName = fullFileName.Substring(0, length - take);
            }
            KeyValuePair<string, string> file = new KeyValuePair<string, string>(fName, ext);
            return file;
        }
        #endregion

        #region Privates
        private static bool Is(Enums.TransformationTypes transformationType, Code code)
        {
            return code.CodeName.Equals(transformationType.ToString().Replace(WhiteSpaceEscaped, Whitespace), StringComparison.CurrentCultureIgnoreCase);
        }

        private void CopyTransformSplitTransDetails(int transformationId, int newTid)
        {
            var transformSplitTransWorker = new FrameworkUAS.BusinessLogic.TransformSplitTrans();
            var transSplitList = transformSplitTransWorker.Select(transformationId);

            foreach (var transformSplitTrans in transSplitList)
            {
                transformSplitTrans.SplitTransformID = 0;
                transformSplitTrans.TransformationID = newTid;
                transformSplitTrans.DateCreated = DateTime.Now;
                transformSplitTrans.CreatedByUserID = CurrentUser.UserID;
                transformSplitTransWorker.Save(transformSplitTrans);
            }
        }

        private void CopyTransformSplitDetails(int transformationId, int newTid)
        {
            var transformSplitWorker = new FrameworkUAS.BusinessLogic.TransformSplit();
            var transSplitList = transformSplitWorker.Select(transformationId);

            foreach (var transformSplit in transSplitList)
            {
                transformSplit.TransformSplitID = 0;
                transformSplit.TransformationID = newTid;
                transformSplit.DateCreated = DateTime.Now;
                transformSplit.CreatedByUserID = CurrentUser.UserID;
                transformSplitWorker.Save(transformSplit);
            }
        }

        private void CopyJoinTransformDetails(int transformationId, int newTid)
        {
            var transformJoinWorker = new FrameworkUAS.BusinessLogic.TransformJoin();
            var transJoinList = transformJoinWorker.Select(transformationId);

            foreach (var transformJoin in transJoinList)
            {
                transformJoin.TransformJoinID = 0;
                transformJoin.TransformationID = newTid;
                transformJoin.DateCreated = DateTime.Now;
                transformJoin.CreatedByUserID = CurrentUser.UserID;
                transformJoinWorker.Save(transformJoin);
            }
        }

        private void CopyDataMapDetails(int transformationId, int newTid)
        {
            var transformDataMapWorker = new FrameworkUAS.BusinessLogic.TransformDataMap();
            var transDataMapList = transformDataMapWorker.Select(transformationId);

            foreach (var transformDataMap in transDataMapList)
            {
                transformDataMap.TransformDataMapID = 0;
                transformDataMap.TransformationID = newTid;
                transformDataMap.DateCreated = DateTime.Now;
                transformDataMap.CreatedByUserID = CurrentUser.UserID;
                transformDataMapWorker.Save(transformDataMap);
            }
        }

        private void CopyAssignValueDetails(int transformationId, int newTid)
        {
            var transformAssignWorker = new FrameworkUAS.BusinessLogic.TransformAssign();
            var transAssignList = transformAssignWorker.Select(transformationId);

            foreach (var transformAssign in transAssignList)
            {
                transformAssign.TransformAssignID = 0;
                transformAssign.TransformationID = newTid;
                transformAssign.DateCreated = DateTime.Now;
                transformAssign.CreatedByUserID = CurrentUser.UserID;
                transformAssignWorker.Save(transformAssign);
            }
        }

        private void CloneTransformationPubMap(int transformationId, int newTid)
        {
            var transformationPubMapWorker = new FrameworkUAS.BusinessLogic.TransformationPubMap();
            var transformationPubMaps = transformationPubMapWorker.Select(transformationId);
            foreach (var transformationPubMap in transformationPubMaps)
            {
                transformationPubMap.TransformationPubMapID = 0;
                transformationPubMap.TransformationID = newTid;
                transformationPubMap.DateCreated = DateTime.Now;
                transformationPubMap.CreatedByUserID = CurrentUser.UserID;
                transformationPubMapWorker.Save(transformationPubMap);
            }
        }

        private Code CloneTransformation(int transformationId, out int newTid)
        {
            var transformationWorker = new FrameworkUAS.BusinessLogic.Transformation();
            var codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            var transformationTypes = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Transformation);
            var transformation = transformationWorker.SelectTransformationByID(transformationId);
            var code = transformationTypes.FirstOrDefault(x => x.CodeId == transformation.TransformationTypeID);

            transformation.TransformationID = 0;
            transformation.TransformationName = $"{transformation.TransformationName}_Copy";
            transformation.IsActive = true;
            transformation.DateCreated = DateTime.Now;
            transformation.CreatedByUserID = CurrentUser.UserID;
            transformation.IsTemplate = false;

            newTid = transformationWorker.Save(transformation);

            CloneTransformationPubMap(transformationId, newTid);
            return code;
        }

        private ActionResult PartialSplitIntoRows(int transformationId, bool enableTransformationEdit)
        {
            object model;
            var prodList = GetUadProductList();
            if (transformationId > 0)
            {
                var transformSplit = new FrameworkUAS.BusinessLogic.TransformSplit().Select(transformationId).FirstOrDefault();
                if (transformSplit != null)
                {
                    model = new TransformationSplitIntoRowModel(
                        transformSplit.TransformSplitID,
                        transformSplit.Delimiter,
                        prodList,
                        GetSelectedProductsForTransformation(transformationId),
                        enableTransformationEdit);
                }
                else
                {
                    model = new TransformationSplitIntoRowModel(prodList);
                }
            }
            else
            {
                model = new TransformationSplitIntoRowModel(prodList, true);
            }

            return PartialView(PartialTransformationSplitIntoRowsView, model);
        }

        private ActionResult PartialJoinColumns(int transformationId, int sourceFileId, bool enableTransformationEdit)
        {
            object model;
            var fieldMappings = new List<FieldMapping>();
            if (sourceFileId > 0)
            {
                fieldMappings.AddRange(
                    new FrameworkUAS.BusinessLogic.FieldMapping().Select(sourceFileId)
                        .Where(x => !x.MAFField.Equals("ignore", StringComparison.CurrentCultureIgnoreCase))
                );
            }

            var fileColumns = fieldMappings.Select(x => x.IncomingField).ToList();
            var prodList = GetUadProductList();

            if (transformationId > 0)
            {
                var transformJoin = new FrameworkUAS.BusinessLogic.TransformJoin()
                                            .Select(transformationId).FirstOrDefault();
                if (transformJoin != null)
                {
                    model = new TransformationJoinColumnModel(
                        transformJoin.TransformJoinID,
                        transformJoin.ColumnsToJoin,
                        transformJoin.Delimiter,
                        fileColumns,
                        prodList,
                        GetSelectedProductsForTransformation(transformationId),
                        enableTransformationEdit);
                }
                else
                    model = new TransformationJoinColumnModel(fileColumns, prodList);
            }
            else
            {
                model = new TransformationJoinColumnModel(fileColumns, prodList, true);
            }

            return PartialView(PartialTransformationJoinColumnsView, model);
        }

        private ActionResult PartialAssignValue(int transformationId, bool enableTransformationEdit)
        {
            var prodList = GetUadProductList();
            var assignMaps = new List<TransformAssignModel>();

            if (transformationId > 0)
            {
                var transformAssigns = new FrameworkUAS.BusinessLogic.TransformAssign().Select(transformationId);
                if (transformAssigns != null)
                {
                    assignMaps.AddRange(
                        GetTransformAssignmentsModel(transformAssigns, prodList));
                }
                else
                {
                    enableTransformationEdit = false;
                }
            }
            else
            {
                enableTransformationEdit = true;
            }

            var model = new TransformationAssignModel(assignMaps, prodList, enableTransformationEdit);
            return PartialView(PartialTransformationAssignValueView, model);
        }

        private ActionResult PartialChangeValue(int transformationId, bool enableTransformationEdit)
        {
            var prodList = GetUadProductList();
            var dataMaps = new List<TransformDataMapModel>();

            if (transformationId > 0)
            {
                var transformDataMap = new FrameworkUAS.BusinessLogic.TransformDataMap().Select(transformationId);

                if (transformDataMap != null)
                {
                    dataMaps.AddRange(GetTransformDataMapModels(transformDataMap, prodList));
                }
                else
                {
                    enableTransformationEdit = false;
                }
            }
            else
            {
                enableTransformationEdit = true;
            }

            var model = new TransformationChangeValueModel(dataMaps, prodList, enableTransformationEdit);
            return PartialView(PartialTransformationChangeValueView, model);
        }

        private static IEnumerable<TransformAssignModel> GetTransformAssignmentsModel(IList<TransformAssign> transformAssigns, List<Product> prodList)
        {
            var i = 0;

            var groupedTransformAssign = transformAssigns
                .GroupBy(x => new { x.Value })
                .Select(y => y.First())
                .ToList();
            foreach (var gta in groupedTransformAssign)
            {
                var pubIDs = new List<int>();
                var groupedIds = new List<string>();

                var filteredTransformDataMap = transformAssigns.Where(x => x.Value == gta.Value).ToList();
                foreach (var tdm in filteredTransformDataMap)
                {
                    if (tdm.PubID == 0)
                    {
                        pubIDs.Add(0);
                        groupedIds.Add(tdm.TransformAssignID.ToString());
                    }
                    else
                    {
                        var prod = prodList.First(x => x.PubID == tdm.PubID);
                        pubIDs.Add(prod.PubID);
                        groupedIds.Add(tdm.TransformAssignID.ToString());
                    }
                }

                var groupedTransformAssignIds = string.Join(Comma, groupedIds);

                yield return new TransformAssignModel(
                    0,
                    groupedTransformAssignIds,
                    i++,
                    pubIDs,
                    string.Empty,
                    gta.Value, prodList);
            }
        }

        private static IEnumerable<TransformDataMapModel> GetTransformDataMapModels(List<TransformDataMap> transformDataMap, List<Product> prodList)
        {
            var i = 0;
            var groupedTransformDataMap = transformDataMap
                .GroupBy(x => new { x.MatchType, x.SourceData, x.DesiredData })
                .Select(y => y.First())
                .ToList();
            foreach (var map in groupedTransformDataMap)
            {
                var pubCode = string.Empty;
                var pubIDs = new List<int>();
                var pubCodeList = new List<string>();
                var groupedIds = new List<string>();
                var isAllProducts = false;

                var filteredTransformDataMap = transformDataMap.Where(x =>
                    x.MatchType == map.MatchType && x.SourceData == map.SourceData &&
                    x.DesiredData == map.DesiredData).ToList();
                foreach (var tdm in filteredTransformDataMap)
                {
                    if (tdm.PubID == 0)
                    {
                        pubCode = AllProductsPubCode;
                        pubIDs.Add(0);
                        groupedIds.Add(tdm.TransformDataMapID.ToString());
                    }
                    else
                    {
                        var prod = prodList.First(x => x.PubID == tdm.PubID);
                        pubIDs.Add(prod.PubID);
                        pubCodeList.Add(prod.PubCode);
                        groupedIds.Add(tdm.TransformDataMapID.ToString());
                        isAllProducts = true;
                    }
                }

                yield return new TransformDataMapModel(
                        0,
                        string.Join(Comma, groupedIds),
                        i++,
                        pubIDs,
                        isAllProducts ? string.Join(Comma, pubCodeList) : pubCode,
                        map.MatchType,
                        map.SourceData,
                        map.DesiredData,
                        prodList);
            }
        }

        private static List<int> GetSelectedProductsForTransformation(int transformationId)
        {
            var selProducts = new FrameworkUAS.BusinessLogic.TransformationPubMap()
                .Select(transformationId)
                .Select(x => x.PubID)
                .Distinct()
                .ToList();
            return selProducts;
        }

        private List<Product> GetUadProductList()
        {
            var prodList = new FrameworkUAD.BusinessLogic.Product().Select(CurrentClient.ClientConnections)
                .OrderBy(x => x.PubCode)
                .Where(x => x.IsActive)
                .ToList();
            return prodList;
        }

        private static void ModifyTransformAssigns(int transformationId, IEnumerable<TransformAssign> taList)
        {
            var transformAssignWorker = new FrameworkUAS.BusinessLogic.TransformAssign();
            foreach (var transformAssign in transformAssignWorker.Select(transformationId))
            {
                transformAssignWorker.Delete(transformAssign.TransformAssignID);
            }

            foreach (var transformAssign in taList)
            {
                transformAssignWorker.Save(transformAssign);
            }
        }

        private bool SetupTransformAssigns(out List<TransformAssign> taList, int transformAssignId, int transformationId,
            IEnumerable<UserTransformAssign> mappings, ICollection<string> pubIds, ref string errorMessage)
        {
            var complete = true;
            taList = new List<TransformAssign>();
            foreach (var cm in mappings)
            {
                //Products will be in a comma separated list. Split and save each.
                var products = cm.PubID.Split(CommaDelimiter).ToList();
                if (!products.Any())
                {
                    products.Add(DefaultProductId);
                }

                if (products.Count(x => x == string.Empty) > 0)
                {
                    products.Remove(string.Empty);
                    products.Add(DefaultProductId);
                }

                foreach (var p in products)
                {
                    int pubId;
                    int.TryParse(p, out pubId);
                    var hasPub = pubId != -1;

                    try
                    {
                        taList.Add(new TransformAssign
                        {
                            TransformAssignID = transformAssignId,
                            TransformationID = transformationId,
                            Value = cm.Value,
                            HasPubID = hasPub,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateUpdated = DateTime.Now,
                            CreatedByUserID = CurrentUser.UserID,
                            UpdatedByUserID = CurrentUser.UserID,
                            PubID = pubId
                        });
                    }
                    catch (Exception)
                    {
                        errorMessage = JoinTransformationError;
                        complete = false;
                    }

                    if (!pubIds.Contains(pubId.ToString()))
                    {
                        pubIds.Add(pubId.ToString());
                    }
                }
            }

            return complete;
        }

        private static bool CheckDataExists(UserTransformAssign[] mappings, ref string errorMessage, List<Product> productListUad)
        {
            var dataExists = true;
            var usedProducts = new List<string>();
            foreach (var up in mappings)
            {
                usedProducts.AddRange(up.PubID.Split(CommaDelimiter).ToList());
            }

            foreach (var columnMapping in mappings)
            {
                if (string.IsNullOrWhiteSpace(columnMapping.PubID))
                {
                    if (string.IsNullOrWhiteSpace(columnMapping.Value))
                    {
                        errorMessage = NoValueToAssignError;
                        dataExists = false;
                    }
                }

                var products = columnMapping.PubID.Split(CommaDelimiter).ToList();

                foreach (var prd in products)
                {
                    var list = mappings.Where(x => x.PubID.Contains(prd)).ToList();

                    if (list.Count > 1 && usedProducts.Count(x => x.Equals(prd)) > 1)
                    {
                        int id;
                        int.TryParse(prd, out id);
                        if (id > 0)
                        {
                            var soloProd = productListUad.First(x => x.PubID == id);
                            errorMessage += string.Format(DuplicateRowError, columnMapping.RowID, soloProd.PubCode);
                        }
                        else
                        {
                            errorMessage += string.Format(DuplicateRowAllProductsError, columnMapping.RowID);
                        }

                        dataExists = false;
                    }
                }
            }

            return dataExists;
        }

        private static bool CheckDataExists(bool mapsProductCode, UserTransformDataMap[] mappings, ref string errorMessage)
        {
            var dataExists = true;
            foreach (var columnMapping in mappings)
            {
                if (string.IsNullOrWhiteSpace(columnMapping.PubID) && !mapsProductCode)
                {
                    errorMessage += $"<li>Row {columnMapping .RowID}: Product must be selected.</li>";
                    dataExists = false;
                }

                var products = columnMapping.PubID.Split(CommaDelimiter).ToList();
                if (!products.Any())
                {
                    products.Add(DefaultProductId);
                }

                foreach (var product in products)
                {
                    var errorTokenString = $"<li>Row {columnMapping.RowID}: Row has duplicate entry.</li>";

                    var exactRowsDuplicates = mappings.Where(x =>
                            x.PubID == product &&
                            x.MatchType.Equals(columnMapping.MatchType, StringComparison.CurrentCultureIgnoreCase) &&
                            x.SourceData.Equals(columnMapping.SourceData, StringComparison.CurrentCultureIgnoreCase) &&
                            x.DesiredData.Equals(columnMapping.DesiredData, StringComparison.CurrentCultureIgnoreCase))
                        .ToList();
                    if (exactRowsDuplicates.Count > 1)
                    {
                        errorMessage += errorTokenString;
                        dataExists = false;
                    }

                    var productMatchAndSourceDuplicates = mappings.Where(x =>
                            x.PubID == product &&
                            x.MatchType.Equals(columnMapping .MatchType, StringComparison.CurrentCultureIgnoreCase) &&
                            x.SourceData.Equals(columnMapping .SourceData, StringComparison.CurrentCultureIgnoreCase))
                        .ToList();
                    if (productMatchAndSourceDuplicates.Count > 1)
                    {
                        errorMessage += errorTokenString;
                        dataExists = false;
                    }

                    var allProductsWithAreasDuplicates = mappings.Where(x =>
                            x.MatchType.Equals(columnMapping .MatchType, StringComparison.CurrentCultureIgnoreCase) &&
                            x.SourceData.Equals(columnMapping .SourceData, StringComparison.CurrentCultureIgnoreCase))
                        .ToList();
                    if (allProductsWithAreasDuplicates.Any(x => x.PubID == ZeroString) &&
                        product != ZeroString &&
                        allProductsWithAreasDuplicates.Any(x => x.PubID == product))
                    {
                        errorMessage += errorTokenString;
                        dataExists = false;
                    }
                }
            }

            return dataExists;
        }

        private static string UnescapeDataMappings(string dataMappings)
        {
            return dataMappings.Replace(SlashChar, string.Empty).TrimStart(QuoteChar).TrimEnd(QuoteChar);
        }

        private IEnumerable<TransformDataMap> SaveDataMapTransformSetup(int transformationId, IEnumerable<UserTransformDataMap> mappings, ICollection<string> pubIds)
        {
            foreach (var columnMapping in mappings)
            {
                foreach (var p in columnMapping.PubID.Split(CommaDelimiter))
                {
                    var pubid = -1;
                    int.TryParse(p, out pubid);
                    var tdm =new TransformDataMap
                        {
                            TransformDataMapID = columnMapping.TransformDataMapID,
                            TransformationID = transformationId,
                            PubID = pubid,
                            MatchType = columnMapping.MatchType,
                            SourceData = columnMapping.SourceData,
                            DesiredData = columnMapping.DesiredData,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateUpdated = DateTime.Now,
                            CreatedByUserID = CurrentUser.UserID,
                            UpdatedByUserID = CurrentUser.UserID
                        };

                    if (!pubIds.Contains(pubid.ToString()))
                    {
                        pubIds.Add(pubid.ToString());
                    }

                    yield return tdm;
                }
            }
        }
        #endregion
    }
}