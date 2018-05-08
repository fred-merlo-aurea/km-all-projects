using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using KM.Common;
using UAS.Web.Helpers;
using UAS.Web.Models.Common;
using UAS.Web.Models.UAD.Filter;
using UAS.Web.Service.Filter;
using CampaignFilterWorker = FrameworkUAD.BusinessLogic.CampaignFilter;
using CampaignWorker = FrameworkUAD.BusinessLogic.Campaign;
using ClientWorker = KMPlatform.BusinessLogic.Client;
using DownloadTemplateWorker = FrameworkUAD.BusinessLogic.DownloadTemplate;
using EntityExportFields = FrameworkUAD.Object.ExportFields;
using EntityFilterCollection = FrameworkUAD.Object.FilterCollection;
using EntityFilterGroup = FrameworkUAD.Entity.FilterGroup;
using EntityFilterMvc = FrameworkUAD.Object.FilterMVC;
using EnumAccess = KMPlatform.Enums.Access;
using EnumFeatures = KMPlatform.Enums.ServiceFeatures;
using EnumServices = KMPlatform.Enums.Services;
using FilterDetailsWorker = FrameworkUAD.BusinessLogic.FilterDetails;
using FilterGroupWorker = FrameworkUAD.BusinessLogic.FilterGroup;
using FilterMvcWorker = FrameworkUAD.BusinessLogic.FilterMVC;
using GroupWorker = ECN_Framework_BusinessLayer.Communicator.Group;
using PlatformUser = KM.Platform.User;
using ReportWorker = FrameworkUAD.BusinessLogic.Report;
using StringFunctions = Core_AMS.Utilities.StringFunctions;
using UadFilterWorker = FrameworkUAD.BusinessLogic.UADFilter;

namespace UAS.Web.Controllers.Common
{
    public class FilterController : BaseController
    {
        public static readonly string ProductViewType = "ProductView";
        public static readonly string ConsensusViewType = "ConsensusView";
        public static readonly string RecencyViewType = "RecencyView";
        public static readonly string CrossProductViewType = "CrossProductView";

        private const string FilterPartialView = "Partials/_Filter";
        private const string ActionNameError = "Error";
        private const string ControllerNameError = "Error";

        private const string ErrorUnAuthorized = "UnAuthorized";
        private const string KeyFilterCollectionModel = "FilterCollectionModel";
        private const string TextAllProducts = "All Products";
        private const string ItemValueZero = "0";
        private const string ItemValueDefault = "-1";
        private const string MessageEnterValidFilterName = "Please enter a valid filter name.";
        private const string MessageFilterNameExists = "The filter Name you entered already exists. Please save under a different name.";
        private const string MessageSelectFilter = "Please select filter.";
        private const string MessageSelectQuestionCategory = "Please select Question Category.";
        private const string MessageQuestionNameExists = "The Question Name you entered already exists. Please save under a different question name.";
        private const string MessageFilterSegmentCountNotEqual = "Cannot overwrite existing filter. Existing filter data segment and edited data segment counts are not same.";
        private const string MessageFilterSaved = "Filter has been saved successfully.";
        private const string SaveModeAddNew = "AddNew";
        private const string SaveModeEdit = "Edit";
        private const string SaveModeExisting = "Existing";
        private const string NameDataCompare = "DataCompare";
        private const string DoubleSpace = "  ";
        private const char IdSeparator = ',';
        private const string DownloadForReport = "Report";
        private const string DownloadForAddRemove = "AddRemove";
        private const string DownloadForIssueSplit = "IssueSplit";
        private const string DownloadForRecordUpdate = "RecordUpdate";
        private const string KeySubscriberIds = "SubscriberIds";
        private const string KeyCurrentFilters = "CurrentFilters";
        private const string KeyAddRemoveSubscriberIds = "AddRemoveSubscriberIds";
        private const string ExportDetailsPopupPartialView = "Partials/Common/_exportDetailsPopup";
        private const string OptionDownload = "Download";
        private const string OptionExport = "Export";
        private const string OptionSaveToCampaign = "SaveToCampaign";
        private const string MessageNoFieldSelectedForDownload = "Please select atleast one field for download or export.";
        private const string MessageDemoFieldShouldNotMoreThan5 = "Demofields should not be more than 5.";
        private const string MessageAdhocFieldShouldNotMoreThan5 = "AdhocFields should not be more than 5.";
        private const string MessageSelectGroupForExportData = "<font color='red'>Please select New Group or Existing group.</font>";
        private const string MessageGroupNameExists = "<font color='#000000'>\"{0}\"</font> listname already exists. Please enter a different name.";
        private const string ExportResultPartialView = "Partials/Common/_exportResult";
        private const string DownloadRootPath = "../main/temp/";
        private const string DownloadAddRemoveRootPath = "../addkilldownloads/main/";
        private const string MessageTotalCampaignSubscriber = "Total subscriber in the campaign : ";
        private const int MaxExportFieldCount = 5;
        private const char FieldSeparator = '|';
        private const string FieldSubscriptionId = "SubscriptionID";
        private const string TsvExtension = ".tsv";
        private const string FieldAddress1 = "ADDRESS1";
        private const string FieldRegionCode = "REGIONCODE";
        private const string FieldZipCode = "ZIPCODE";
        private const string FieldPubTransactionDate = "PUBTRANSACTIONDATE";
        private const string FieldQualificationDate = "QUALIFICATIONDATE";
        private const string ColumnAddress = "Address";
        private const string ColumnState = "State";
        private const string ColumnZip = "Zip";
        private const string ColumnTransactionDate = "TransactionDate";
        private const string ColumnQDate = "QDate";
        private const string ColumnPromoCode = "PromoCode";
        private const string ColumnSubIdNone = "s.SubscriptionID|None";
        private const string ColumnGroupNoNone = "s.CGRP_NO|None";
        private const char LeftParentheses = '(';
        private const string DistinctSubIdQuery = "distinct 1, ps.SubscriptionID ";
        private const string SortOrderAsc = "sortorder asc";
        private const int RandomFileNameLength = 5;
        private const string MessageNameAlreadyExists = "ERROR - <font color='#000000'>\"{0}\"</font> already exists. Please enter a different name.";
        private const string XmlHeaderLine = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>";
        private const string XmlCloseTag = "</XML>";
        private const string SelectedFilterOperation = "Single";
        private const string SelectedFilterNo = "1";
        private const int CampaignDetailsBatchSize = 10000;
        private const string XmlSidFormat = "<sID>{0}</sID>";
        private const string ErrorNoPermissionToDownloadData = "You do not have permission to download/export the data.";
        private const string ErrorTypeNotAuthorized = "NotAuthorized";
        private const string ErrorTypeUnAuthorized = "UnAuthorized";
        private const string MaskFieldEmail = "EMAIL";

        private static readonly string[] ProfileFields =
        {
            "FIRSTNAME", "LASTNAME", "COMPANY", "TITLE", "ADDRESS", "ADDRESS2", "ADDRESS3",
            "CITY", "STATE", "ZIP", "COUNTRY", "PHONE", "FAX", "MOBILE"
        };

        private static readonly string[] NonUdfFields = 
        {
            "SUBSCRIPTIONID", "EMAIL", "FIRSTNAME", "LASTNAME", "COMPANY", "TITLE", "ADDRESS",
            "ADDRESS2", "ADDRESS3", "CITY", "STATE", "ZIP", "COUNTRY", "PHONE", "FAX", "MOBILE"
        };

        #region Public Action Methods

        public ActionResult GetFilterViewModel(int brandId = 0, bool IsCirc = true, int pubID = 1, string vwType = "ProductView")
        {
            if (!PlatformUser.HasService(CurrentUser, EnumServices.FULFILLMENT) &&
                !PlatformUser.HasService(CurrentUser, EnumServices.UAD))
            {
                return RedirectToAction(ActionNameError, ControllerNameError, new {errorType = ErrorUnAuthorized});
            }

            var viewModel = new FilterViewModel
            {
                ViewType = vwType,
                IsCirc = IsCirc
            };

            if (viewModel.IsCirc)
            {
                UpdateCircFilterViewModel(viewModel, pubID);
            }
            else
            {
                if (!CheckViewAccess(viewModel.ViewType))
                {
                    return RedirectToAction(ActionNameError, ControllerNameError, new {errorType = ErrorUnAuthorized});
                }

                UpdateUadFilterModel(viewModel, brandId);
            }

            viewModel.CircFilter = GetCircFilter(viewModel.IsCirc);
            viewModel.geoFilter = GetGeoFilter();
            viewModel.standardFilter = GetStandardFilter();
            viewModel.filterCategoryTree = GetAllFilterCategories();
            viewModel.filters = new FrameworkUAD.Object.FilterCollection(CurrentClient.ClientConnections, CurrentUser.UserID);
            Session[KeyFilterCollectionModel] = viewModel;

            return PartialView(FilterPartialView, viewModel);
        }

        public ActionResult GetSelectedContryFiltersByCountryRegion(string areaNames = "")
        {
            var countries = FilterService.GetAllCountries();
            var areas = areaNames.Split(',');
            var list = new List<string>(areas.Length);
            areas.ToList().ForEach(i => list.Add(i));
            countries = countries.Select(x => x).Where(x => list.Contains(x.Area)).ToList();


            var countryList = countries.OrderBy(x => x.SortOrder).ToList();
            List<SelectListItem> selectList = new List<SelectListItem>();
            countryList = countryList.Where(x => x.SortOrder != 0).OrderByDescending(o => o.CountryID == 1)
                .ThenByDescending(o => o.CountryID == 2)
                .ThenByDescending(o => o.CountryID == 429)
                .ThenBy(x => x.ShortName).ToList();
            countryList.ForEach(c => selectList.Add(new SelectListItem() { Text = c.ShortName, Value = c.CountryID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadFilter(bool allfilters = false, string drpSearch = "CONTAINS", string txtSearch = "", int filtercategory = 0, string Mode = "Load", int BrandID = 0, int PubID = 0, string vwType = "ProductView")
        {
            List<FrameworkUAD.Object.UADFilter> lmf = new List<FrameworkUAD.Object.UADFilter>();
            lmf = new FrameworkUAD.BusinessLogic.UADFilter().GetFilterByUserIDType(CurrentClient.ClientConnections, allfilters ? 0 : CurrentUser.UserID, (FrameworkUAD.BusinessLogic.Enums.ViewType) Enum.Parse(typeof(FrameworkUAD.BusinessLogic.Enums.ViewType), vwType, true), PubID, BrandID, KM.Platform.User.IsAdministrator(CurrentUser) ? true : false, Mode.Equals("Load", StringComparison.OrdinalIgnoreCase) ? true : false);
            lmf = lmf.FindAll(x => (x.FilterCategoryID ?? 0) == Convert.ToInt32(filtercategory));
            if (lmf != null)
            {
                if (!string.IsNullOrEmpty(txtSearch))
                {
                    switch (drpSearch.ToUpper())
                    {
                        case "EQUAL":
                            lmf = lmf.FindAll(x => x.Name.ToLower().Equals(txtSearch.ToLower()) || (x.QuestionName ?? "").ToLower().Equals(txtSearch.ToLower()));
                            break;
                        case "START WITH":
                            lmf = lmf.FindAll(x => x.Name.ToLower().StartsWith(txtSearch.ToLower()) || (x.QuestionName ?? "").ToLower().StartsWith(txtSearch.ToLower()));
                            break;
                        case "END WITH":
                            lmf = lmf.FindAll(x => x.Name.ToLower().EndsWith(txtSearch.ToLower()) || (x.QuestionName ?? "").ToLower().EndsWith(txtSearch.ToLower()));
                            break;
                        case "CONTAINS":
                            lmf = lmf.FindAll(x => x.Name.ToLower().Contains(txtSearch.ToLower()) || (x.QuestionName ?? "").ToLower().Contains(txtSearch.ToLower()));
                            break;
                    }
                }
            }
            List<KMPlatform.Entity.User> lusers = new KMPlatform.BusinessLogic.User().Select();
            List<FrameworkUAD.Entity.FilterCategory> fcat = new FrameworkUAD.BusinessLogic.FilterCategory().Select(CurrentClient.ClientConnections);
            var query = (from m in lmf
                             //join fc in fcat on m.FilterCategoryID equals fc.FilterCategoryID
                         join u in lusers on m.CreatedUserID equals u.UserID into mu
                         from f in mu.DefaultIfEmpty()
                         select new FrameworkUAD.Object.UADFilter { FilterId = m.FilterId, Name = m.Name, Notes = m.Notes, CreatedDate = m.CreatedDate, FilterCategoryName = fcat.Where(x => x.FilterCategoryID == m.FilterCategoryID).Select(x => x.CategoryName).FirstOrDefault(), CreatedUserName = f == null ? "" : f.UserName, FilterCategoryID = m.FilterCategoryID }).ToList();
            return PartialView("Partials/Filter/_savedFilterGrid", query);
            //return query;
        }
        public ActionResult GetAdHocListFilter(int brandID = 0, int pubID = 0, string Viewtype = "")
        {
            List<AdhocFilterVM> lstAdhocCatList = new List<AdhocFilterVM>();
            List<FrameworkUAD.Entity.AdhocCategory> adct = new List<FrameworkUAD.Entity.AdhocCategory>();

            var adlst = new FrameworkUAD.BusinessLogic.AdhocCategory().SelectAll(CurrentClient.ClientConnections);
            adct.AddRange(adlst);
            adct.Add(new FrameworkUAD.Entity.AdhocCategory() { CategoryID = 0, CategoryName = "Others" });
            foreach (var c in adct)
            {
                var adFVM = new AdhocFilterVM();
                adFVM.CategoryID = c.CategoryID;
                adFVM.CategoryName = c.CategoryName;
                adFVM.AdHocFilter = new FrameworkUAD.BusinessLogic.Adhoc().GetByCategoryID(c.CategoryID, CurrentClient.ClientConnections, brandID, pubID);
                lstAdhocCatList.Add(adFVM);

            }

            //}
            AdHocFilterViewModel adhocmodel = new AdHocFilterViewModel();
            adhocmodel.AdHocDetails = lstAdhocCatList;
            adhocmodel.VarCharList = new List<SelectListItem>();
            adhocmodel.VarCharList.Add(new SelectListItem() { Text = "CONTAINS", Value = "Contains" });
            adhocmodel.VarCharList.Add(new SelectListItem() { Text = "EQUAL", Value = "Equal" });
            adhocmodel.VarCharList.Add(new SelectListItem() { Text = "START WITH", Value = "Start With" });
            adhocmodel.VarCharList.Add(new SelectListItem() { Text = "END WITH", Value = "End With" });
            adhocmodel.VarCharList.Add(new SelectListItem() { Text = "DOES NOT CONTAIN", Value = "Does Not Contain" });
            adhocmodel.VarCharList.Add(new SelectListItem() { Text = "IS EMPTY", Value = "Is Empty" });
            adhocmodel.VarCharList.Add(new SelectListItem() { Text = "IS NOT EMPTY", Value = "Is Not Empty" });

            adhocmodel.ZipVarCharList = new List<SelectListItem>();
            adhocmodel.ZipVarCharList.Add(new SelectListItem() { Text = "CONTAINS", Value = "Contains" });
            adhocmodel.ZipVarCharList.Add(new SelectListItem() { Text = "EQUAL", Value = "Equal" });
            adhocmodel.ZipVarCharList.Add(new SelectListItem() { Text = "START WITH", Value = "Start With" });
            adhocmodel.ZipVarCharList.Add(new SelectListItem() { Text = "END WITH", Value = "End With" });
            adhocmodel.ZipVarCharList.Add(new SelectListItem() { Text = "DOES NOT CONTAIN", Value = "Does Not Contain" });
            adhocmodel.ZipVarCharList.Add(new SelectListItem() { Text = "IS EMPTY", Value = "Is Empty" });
            adhocmodel.ZipVarCharList.Add(new SelectListItem() { Text = "IS NOT EMPTY", Value = "Is Not Empty" });
            adhocmodel.ZipVarCharList.Add(new SelectListItem() { Text = "RANGE", Value = "Range" });

            adhocmodel.XDaysList = new List<SelectListItem>();
            adhocmodel.XDaysList.Add(new SelectListItem() { Text = "7 days", Value = "7" });
            adhocmodel.XDaysList.Add(new SelectListItem() { Text = "14 days", Value = "14" });
            adhocmodel.XDaysList.Add(new SelectListItem() { Text = "21 days", Value = "21" });
            adhocmodel.XDaysList.Add(new SelectListItem() { Text = "30 days", Value = "30" });
            adhocmodel.XDaysList.Add(new SelectListItem() { Text = "60 days", Value = "60" });
            adhocmodel.XDaysList.Add(new SelectListItem() { Text = "90 days", Value = "90" });
            adhocmodel.XDaysList.Add(new SelectListItem() { Text = "120 days", Value = "120" });
            adhocmodel.XDaysList.Add(new SelectListItem() { Text = "150 days", Value = "150" });
            adhocmodel.XDaysList.Add(new SelectListItem() { Text = "6 months", Value = "6mon" });
            adhocmodel.XDaysList.Add(new SelectListItem() { Text = "Custom", Value = "Custom" });

            adhocmodel.DateRangeList = new List<SelectListItem>();
            adhocmodel.DateRangeList.Add(new SelectListItem() { Text = "DATERANGE", Value = "DateRange" });
            adhocmodel.DateRangeList.Add(new SelectListItem() { Text = "LAST X DAYS", Value = "XDays" });
            adhocmodel.DateRangeList.Add(new SelectListItem() { Text = "YEAR", Value = "Year" });

            adhocmodel.NumericList = new List<SelectListItem>();
            adhocmodel.NumericList.Add(new SelectListItem() { Text = "RANGE", Value = "Range" });
            adhocmodel.NumericList.Add(new SelectListItem() { Text = "EQUAL", Value = "Equal" });
            adhocmodel.NumericList.Add(new SelectListItem() { Text = "GREATER THAN", Value = "Greater" });
            adhocmodel.NumericList.Add(new SelectListItem() { Text = "LESSER THAN", Value = "Month" });
            return PartialView("Partials/Filter/_adHocFilter", adhocmodel);

        }
        public ActionResult GetSaveFilterPanel(SaveFilterViewModel sfVM)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.Edit))
            {

                sfVM.FilterCategoryTree = GetAllFilterCategories();
                sfVM.QuestionCategoryTree = GetAllQuestionCategories();
                return PartialView("Partials/Filter/_saveFilterPanel", sfVM);
            }
            else
            {
                return Json("User does not have access to Edit/Save filters.");
            }
        }
        public ActionResult GetSavedFiltersPanel(SaveFilterViewModel sfVM)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.Edit))
            {

                sfVM.FilterCategoryTree = GetAllFilterCategories();
                return PartialView("Partials/Filter/_savedFilter", sfVM);
            }
            else
            {
                return Json("User does not have access to Edit/Save filters.");
            }
        }
        public ActionResult GetDefaultFilters()
        {
            if (KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT))
            {
                List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catCodeType = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType().Select();
                var QualFreeCatType = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType().Select("Qualified Free");
                var QualPaidCatType = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType().Select("Qualified Paid");
                List<SelectListItem> selectCatTypeList = new List<SelectListItem>();
                selectCatTypeList.Add(new SelectListItem()
                {
                    Text = QualFreeCatType.CategoryCodeTypeName.ToUpper(),
                    Value = QualFreeCatType.CategoryCodeTypeID.ToString(),
                    Selected = true
                });
                selectCatTypeList.Add(new SelectListItem()
                {
                    Text = QualPaidCatType.CategoryCodeTypeName.ToUpper(),
                    Value = QualPaidCatType.CategoryCodeTypeID.ToString(),
                    Selected = true
                });

                List<FrameworkUAD_Lookup.Entity.CategoryCode> catCode = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode().Select().Where(x => (x.CategoryCodeTypeID == QualFreeCatType.CategoryCodeTypeID || x.CategoryCodeTypeID == QualPaidCatType.CategoryCodeTypeID) && x.CategoryCodeValue != 70).ToList();
                List<SelectListItem> selectListcatCode = new List<SelectListItem>();
                catCode.ForEach(c => selectListcatCode.Add(new SelectListItem()
                {
                    Text = c.CategoryCodeValue + ". " + c.CategoryCodeName.ToUpper(),
                    Value = c.CategoryCodeID.ToString(),
                    Selected = true

                }));

                List<FrameworkUAD_Lookup.Entity.TransactionCodeType> xCodeTypeList = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType().Select();
                FrameworkUAD_Lookup.Entity.TransactionCodeType xCodeTypeActiveFree = xCodeTypeList.Where(x => x.IsActive == true && x.IsFree == true).First();
                FrameworkUAD_Lookup.Entity.TransactionCodeType xCodeTypeActivePaid = xCodeTypeList.Where(x => x.IsActive == true && x.IsFree == false).First();
                List<SelectListItem> selectListXCodeType = new List<SelectListItem>();
                selectListXCodeType.Add(new SelectListItem()
                {
                    Text = xCodeTypeActiveFree.TransactionCodeTypeName.ToUpper(),
                    Value = xCodeTypeActiveFree.TransactionCodeTypeID.ToString(),
                    Selected = true
                });
                selectListXCodeType.Add(new SelectListItem()
                {
                    Text = xCodeTypeActivePaid.TransactionCodeTypeName.ToUpper(),
                    Value = xCodeTypeActivePaid.TransactionCodeTypeID.ToString(),
                    Selected = true
                });


                List<FrameworkUAD_Lookup.Entity.TransactionCode> xCodeList = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode().Select().Where(x => x.TransactionCodeTypeID == xCodeTypeActiveFree.TransactionCodeTypeID || x.TransactionCodeTypeID == xCodeTypeActivePaid.TransactionCodeTypeID).ToList();
                List<SelectListItem> selectListxCodeList = new List<SelectListItem>();
                xCodeList.ForEach(c => selectListxCodeList.Add(new SelectListItem()
                {
                    Text = c.TransactionCodeValue + ". " + c.TransactionCodeName.ToUpper(),
                    Value = c.TransactionCodeID.ToString(),
                    Selected = true
                }));

                return Json(new { cc = selectListcatCode, ct = selectCatTypeList, xc = selectListxCodeList, xt = selectListXCodeType }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("User does not have access to Edit/Save filters.");
            }
        }
        public ActionResult GetCategoryCodesByCategory(List<int> catIdlst = null)
        {
            if (KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT))
            {
                List<SelectListItem> selectListcatCode = new List<SelectListItem>();
                if (catIdlst != null)
                {
                    List<FrameworkUAD_Lookup.Entity.CategoryCode> catCode = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode().Select().Where(x => catIdlst.Contains(x.CategoryCodeTypeID)).ToList();

                    catCode.ForEach(c => selectListcatCode.Add(new SelectListItem()
                    {
                        Text = c.CategoryCodeValue + ". " + c.CategoryCodeName.ToUpper(),
                        Value = c.CategoryCodeID.ToString(),
                        Selected = true

                    }));
                }

                return Json(selectListcatCode, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("User does not have access to Edit/Save filters.");
            }
        }
        public ActionResult GetQSourceCodesByQSourceType(List<int> QTypeIdlst = null)
        {
            if (KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT))
            {
                List<SelectListItem> selectList = new List<SelectListItem>();
                if (QTypeIdlst != null)
                {
                    List<FrameworkUAD_Lookup.Entity.Code> codeList = new FrameworkUAD_Lookup.BusinessLogic.Code().Select();
                    var qSourceList = codeList.Where(x => QTypeIdlst.Contains(x.ParentCodeId.HasValue ? (int) x.ParentCodeId : 0)).OrderBy(x => x.DisplayOrder).ToList();
                    qSourceList.ForEach(c => selectList.Add(new SelectListItem() { Text = c.DisplayName.ToUpper(), Value = c.CodeId.ToString(), Selected = true }));

                }
                return Json(selectList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("User does not have access to Edit/Save filters.");
            }
        }
        public ActionResult GetXCodesByXType(List<int> xTypeIdlst = null)
        {
            if (KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT) || KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.UAD))
            {
                List<SelectListItem> selectListxCodeList = new List<SelectListItem>();
                if (xTypeIdlst != null)
                {
                    List<FrameworkUAD_Lookup.Entity.TransactionCode> xCodeList = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode().Select().Where(x => xTypeIdlst.Contains(x.TransactionCodeTypeID)).ToList();
                    xCodeList.ForEach(c => selectListxCodeList.Add(new SelectListItem()
                    {
                        Text = c.TransactionCodeValue + ". " + c.TransactionCodeName.ToUpper(),
                        Value = c.TransactionCodeID.ToString(),
                        Selected = true
                    }));
                }

                return Json(selectListxCodeList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        public ActionResult GetXCodesByTransaction(int transactionTypeID = 0)
        {
            if (KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT) || KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.UAD))
            {
                List<FrameworkUAD_Lookup.Entity.TransactionCode> xCodeList = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode().Select().Where(x => x.TransactionCodeTypeID == transactionTypeID).ToList();
                List<SelectListItem> selectListxCodeList = new List<SelectListItem>();
                xCodeList.ForEach(c => selectListxCodeList.Add(new SelectListItem()
                {
                    Text = c.TransactionCodeValue + ". " + c.TransactionCodeName.ToUpper(),
                    Value = c.TransactionCodeID.ToString(),
                    Selected = true
                }));
                return Json(selectListxCodeList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        [HttpGet]
        public ActionResult GetFilterGridSearchDropDown()
        {
            List<SelectListItem> values = new List<SelectListItem>();
            values.Add(new SelectListItem() { Text = "CONTAINS", Value = "Contains" });
            values.Add(new SelectListItem() { Text = "EQUAL", Value = "Equal" });
            values.Add(new SelectListItem() { Text = "START WITH", Value = "Start With" });
            values.Add(new SelectListItem() { Text = "END WITH", Value = "End With" });
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetVarCharFieldDropDown()
        {
            List<SelectListItem> values = new List<SelectListItem>();
            values.Add(new SelectListItem() { Text = "CONTAINS", Value = "Contains" });
            values.Add(new SelectListItem() { Text = "EQUAL", Value = "Equal" });
            values.Add(new SelectListItem() { Text = "START WITH", Value = "Start With" });
            values.Add(new SelectListItem() { Text = "END WITH", Value = "End With" });
            values.Add(new SelectListItem() { Text = "DOES NOT CONTAIN", Value = "Does Not Contain" });
            values.Add(new SelectListItem() { Text = "IS EMPTY", Value = "Is Empty" });
            values.Add(new SelectListItem() { Text = "IS NOT EMPTY", Value = "Is Not Empty" });
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetDateFieldDropDown()
        {
            List<SelectListItem> values = new List<SelectListItem>();
            values.Add(new SelectListItem() { Text = "DATERANGE", Value = "DateRange" });
            values.Add(new SelectListItem() { Text = "LAST X DAYS", Value = "XDays" });
            values.Add(new SelectListItem() { Text = "YEAR", Value = "Year" });

            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetNumericFieldDropDown()
        {
            List<SelectListItem> values = new List<SelectListItem>();
            values.Add(new SelectListItem() { Text = "RANGE", Value = "Range" });
            values.Add(new SelectListItem() { Text = "EQUAL", Value = "Equal" });
            values.Add(new SelectListItem() { Text = "GREATER THAN", Value = "Greater" });
            values.Add(new SelectListItem() { Text = "LESSER THAN", Value = "Month" });

            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetBitFieldDropDown()
        {
            List<SelectListItem> values = new List<SelectListItem>();
            values.Add(new SelectListItem() { Text = "YES", Value = "1" });
            values.Add(new SelectListItem() { Text = "NO", Value = "0" });

            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetNoOfDaysDropDown()
        {
            List<SelectListItem> values = new List<SelectListItem>();
            values.Add(new SelectListItem() { Text = "7 days", Value = "7" });
            values.Add(new SelectListItem() { Text = "14 days", Value = "14" });
            values.Add(new SelectListItem() { Text = "21 days", Value = "21" });
            values.Add(new SelectListItem() { Text = "30 days", Value = "30" });
            values.Add(new SelectListItem() { Text = "60 days", Value = "60" });
            values.Add(new SelectListItem() { Text = "90 days", Value = "90" });
            values.Add(new SelectListItem() { Text = "120 days", Value = "120" });
            values.Add(new SelectListItem() { Text = "150 days", Value = "150" });
            values.Add(new SelectListItem() { Text = "6 months", Value = "6mon" });
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetOpenActivityDropDown()
        {
            List<SelectListItem> values = new List<SelectListItem>();
            values.Add(new SelectListItem() { Text = "", Value = "" });
            values.Add(new SelectListItem() { Text = "No Opens", Value = "0" });
            values.Add(new SelectListItem() { Text = "Opened 1+", Value = "1" });
            values.Add(new SelectListItem() { Text = "Opened 2+", Value = "2" });
            values.Add(new SelectListItem() { Text = "Opened 3+", Value = "3" });
            values.Add(new SelectListItem() { Text = "Opened 4+", Value = "4" });
            values.Add(new SelectListItem() { Text = "Opened 5+", Value = "5" });
            values.Add(new SelectListItem() { Text = "Opened 10+", Value = "10" });
            values.Add(new SelectListItem() { Text = "Opened 15+", Value = "15" });
            values.Add(new SelectListItem() { Text = "Opened 20+", Value = "20" });
            values.Add(new SelectListItem() { Text = "Opened 30+", Value = "30" });
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetClickedActivityDropDown()
        {
            List<SelectListItem> values = new List<SelectListItem>();
            values.Add(new SelectListItem() { Text = "", Value = "" });
            values.Add(new SelectListItem() { Text = "No Clicks", Value = "0" });
            values.Add(new SelectListItem() { Text = "Clicked 1+", Value = "1" });
            values.Add(new SelectListItem() { Text = "Clicked 2+", Value = "2" });
            values.Add(new SelectListItem() { Text = "Clicked 3+", Value = "3" });
            values.Add(new SelectListItem() { Text = "Clicked 4+", Value = "4" });
            values.Add(new SelectListItem() { Text = "Clicked 5+", Value = "5" });
            values.Add(new SelectListItem() { Text = "Clicked 10+", Value = "10" });
            values.Add(new SelectListItem() { Text = "Clicked 15+", Value = "15" });
            values.Add(new SelectListItem() { Text = "Clicked 20+", Value = "20" });
            values.Add(new SelectListItem() { Text = "Clicked 30+", Value = "30" });
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetVisitedActivityDropDown()
        {
            List<SelectListItem> values = new List<SelectListItem>();
            values.Add(new SelectListItem() { Text = "", Value = "" });
            values.Add(new SelectListItem() { Text = "No Visits", Value = "0" });
            values.Add(new SelectListItem() { Text = "Visited 1+", Value = "1" });
            values.Add(new SelectListItem() { Text = "Visited 2+", Value = "2" });
            values.Add(new SelectListItem() { Text = "Visited 3+", Value = "3" });
            values.Add(new SelectListItem() { Text = "Visited 4+", Value = "4" });
            values.Add(new SelectListItem() { Text = "Visited 5+", Value = "5" });
            values.Add(new SelectListItem() { Text = "Visited 10+", Value = "10" });
            values.Add(new SelectListItem() { Text = "Visited 15+", Value = "15" });
            values.Add(new SelectListItem() { Text = "Visited 20+", Value = "20" });
            values.Add(new SelectListItem() { Text = "Visited 30+", Value = "30" });
            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveFilter(SaveFilterViewModel saveModel)
        {
            try
            {
                var errorResult = CheckSaveFilterModelError(saveModel);

                if (errorResult != null)
                {
                    return errorResult;
                }

                var savedFilterId = SaveUadFilter(saveModel);
                var filterGroups = new List<EntityFilterGroup>();
                var deleteGroup = false;

                if (saveModel.NewExisting.Equals(SaveModeExisting, StringComparison.OrdinalIgnoreCase)
                    && !saveModel.Mode.Equals(SaveModeEdit, StringComparison.OrdinalIgnoreCase))
                {
                    filterGroups = new FilterGroupWorker().getByFilterID(CurrentClient.ClientConnections, saveModel.FilterID);
                    var idArray = saveModel.FilterIDs.Split(IdSeparator);

                    if (new FrameworkUAD.BusinessLogic.FilterSchedule().ExistsByFilterID(CurrentClient.ClientConnections, saveModel.FilterID))
                    {
                        if (filterGroups.Count != idArray.Length)
                        {
                            return ErrorJsonResult(MessageFilterSegmentCountNotEqual);
                        }

                        new FilterDetailsWorker().Delete_ByFilterID(CurrentClient.ClientConnections, saveModel.FilterID);
                    }
                    else
                    {
                        new FilterDetailsWorker().Delete_ByFilterID(CurrentClient.ClientConnections, saveModel.FilterID);
                        new FilterGroupWorker().Delete_ByFilterID(CurrentClient.ClientConnections, saveModel.FilterID);
                        deleteGroup = true;
                    }
                }

                if (!saveModel.Mode.Equals(SaveModeEdit, StringComparison.OrdinalIgnoreCase))
                {
                    SaveFilterDetails(saveModel, savedFilterId, filterGroups, deleteGroup);
                }
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ex.Message);
            }

            return ErrorJsonResult(MessageFilterSaved, false);
        }

        public ActionResult AddFilterForReports(FrameworkUAD.Object.FilterMVC filter)
        {
            filter.FilterQuery = FrameworkUAD.BusinessLogic.FilterMVC.getProductFilterQuery(filter, CurrentClient.ClientConnections);
            return Json(filter.FilterQuery, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetFilter(int filterID)
        {
            var filter = FrameworkUAD.BusinessLogic.FilterMVC.GetFilterByID(CurrentClient.ClientConnections, filterID);
            return Json(filter, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetCustomers(int ClientGroupID)
        {
            var clients = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroup(ClientGroupID).ToList();
            return Json(clients, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDownloadTemplates(int PubID = 0, int BrandID = 0)
        {
            List<FrameworkUAD.Entity.DownloadTemplate> clientTemplates = new List<FrameworkUAD.Entity.DownloadTemplate>();
            clientTemplates.AddRange(FrameworkUAD.BusinessLogic.DownloadTemplate.GetByPubIDBrandID(CurrentClient.ClientConnections, PubID, BrandID));
            return Json(clientTemplates, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExportDetailsPopUp(DownloadViewModel dvm)
        {
            Session[KeySubscriberIds] = null;  //Store your Current Subscriber IDs
            Session[KeyCurrentFilters] = null; //Store your Filter
            var subscriberIds = new List<int>();

            UpdateDownloadViewModel(dvm);

            // Implementation based on the where Download Popup is used
            switch (dvm.DownloadFor)
            {
                case DownloadForReport:
                    UpdateDownloadViewModelForReport(dvm, subscriberIds);
                    break;
                case DownloadForAddRemove:
                    UpdateDownloadViewModelForAddRemove(dvm, subscriberIds);
                    break;
                case DownloadForIssueSplit:
                    break;
                case DownloadForRecordUpdate:
                    break;
            }

            subscriberIds = subscriberIds.Distinct().ToList();
            dvm = ExportFields(dvm);
            dvm.DownloadCount = subscriberIds.Count;
            dvm.TotalCount = subscriberIds.Count;

            Session[KeySubscriberIds] = subscriberIds;
            Session[KeyCurrentFilters] = dvm.FilterList;
            return PartialView(ExportDetailsPopupPartialView, dvm);
        }

        public ActionResult ExportData(DownloadViewModel dp)
        {
            Guard.NotNull(dp, nameof(dp));
            var stdColumnList = new List<string>();
            var pubSubscriptionsExtMapperValueList = new List<string>();
            var responseGroupIdList = new List<string>();
            var responseGroupDescIdList = new List<string>();
            var selectedItem = new List<string>();
            var customColumnList = new List<string>();
            var filePath = string.Empty;

            dp.ViewType = FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView;
            UpdateDownloadViewModelForArchive(dp);

            var errorMessage = CheckErrorForExportData(dp);
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                return Json(new {error = true, errormessage = errorMessage}, JsonRequestBehavior.AllowGet);
            }

            GenerateColumnsForDownloadAndExport(
                dp,
                selectedItem,
                ref pubSubscriptionsExtMapperValueList,
                ref responseGroupIdList,
                ref responseGroupDescIdList,
                ref stdColumnList,
                ref customColumnList);

            switch (dp.DownloadFor)
            {
                case DownloadForReport:
                    return ExportDataForReport(
                        dp,
                        pubSubscriptionsExtMapperValueList,
                        responseGroupIdList,
                        responseGroupDescIdList,
                        stdColumnList,
                        customColumnList);

                case DownloadForAddRemove:
                    return ExportDataForAddRemove(
                        dp,
                        pubSubscriptionsExtMapperValueList,
                        responseGroupIdList,
                        responseGroupDescIdList,
                        stdColumnList,
                        customColumnList);

                case DownloadForRecordUpdate:
                    break;
                case DownloadForIssueSplit:
                    break;
            }

            return Json(filePath);
        }

        public ActionResult GetExportColumns(DownloadViewModel dp)
        {
            dp.ViewType = FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView;
            List<int> PubIDs =new List<int>() { dp.PubID };

            #region Check if Data required from Archive
            if (dp.IssueID == 0)
            {
                dp.IsArchived = false;
            }
            else
            {
                var issuelist = new FrameworkUAD.BusinessLogic.Issue().Select(CurrentClient.ClientConnections);
                var i = issuelist.Where(x => x.IssueId == dp.IssueID).First();
                if (i != null && i.IsComplete == true)
                {
                    dp.IsArchived = true;
                }
            }
            #endregion
            Dictionary<string, string> exportfields = new Dictionary<string, string>();
            Dictionary<string, string> exportProfileFields = new Dictionary<string, string>();
            Dictionary<string, string> exportDemoFields = new Dictionary<string, string>();
            Dictionary<string, string> exportAdhocFields = new Dictionary<string, string>();

            FrameworkUAD.BusinessLogic.Enums.ExportType exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.FTP;
            if (dp.MasterOptionSelected == "Download")
                exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.FTP;
            else if (dp.MasterOptionSelected == "Export")
                exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.ECN;
            else if (dp.MasterOptionSelected == "Campaign")
                exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.Campaign;

            exportProfileFields = Helpers.Utilities.GetExportingFields(CurrentClient.ClientConnections, dp.PubID, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Profile, dp.DownloadFor, dp.IsArchived);
            exportDemoFields = Helpers.Utilities.GetExportingFields(CurrentClient.ClientConnections, dp.PubID, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Demo, dp.DownloadFor, dp.IsArchived);
            exportAdhocFields = Helpers.Utilities.GetExportingFields(CurrentClient.ClientConnections, dp.PubID, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Adhoc, dp.DownloadFor, dp.IsArchived);

            foreach (var e in exportProfileFields)
                exportfields.Add(e.Key, e.Value);

            foreach (var e in exportDemoFields)
                exportfields.Add(e.Key, e.Value);

            foreach (var e in exportAdhocFields)
                exportfields.Add(e.Key, e.Value);

            List<FrameworkUAD.Entity.DownloadTemplateDetails> dtd = FrameworkUAD.BusinessLogic.DownloadTemplateDetails.GetByDownloadTemplateID(CurrentClient.ClientConnections, Convert.ToInt32(dp.DownloadTemplateID));
            dp.AvailableProfileFields.Clear();
            dp.AvailableDemoFields.Clear();
            dp.AvailableAdhocFields.Clear();
            dp.SelectedItems.Clear();
            foreach (var item in exportProfileFields)
            {
                if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                    dp.AvailableProfileFields.Add(new SelectListItem() { Text = item.Value, Value = item.Key });
            }

            foreach (var item in exportDemoFields)
            {
                if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                    dp.AvailableDemoFields.Add(new SelectListItem() { Text = item.Value, Value = item.Key });
            }

            foreach (var item in exportAdhocFields)
            {
                if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                    dp.AvailableAdhocFields.Add(new SelectListItem() { Text = item.Value, Value = item.Key });

            }
            foreach (var td in dtd)
            {
                var item = exportfields.Where(x => x.Key.Split('|')[0].Equals(td.ExportColumn)).FirstOrDefault();
                if (!string.IsNullOrEmpty(item.Key))
                {
                    dp.SelectedItems.Add(new SelectListItem()
                    {
                        Text = item.Value +
                         (
                         item.Key.Split('|')[1].ToUpper() == FrameworkUAD.BusinessLogic.Enums.FieldType.Varchar.ToString().ToUpper()
                         ? "(" + (td.FieldCase == null ? FrameworkUAD.BusinessLogic.Enums.FieldCase.Default.ToString() : td.FieldCase) +
                               ")"
                               : ""
                         ),
                        Value = item.Key + "|" +
                         (td.FieldCase == null
                         ? (item.Key.Split('|')[1].ToUpper() == FrameworkUAD.BusinessLogic.Enums.FieldType.Varchar.ToString().ToUpper()
                             ? FrameworkUAD.BusinessLogic.Enums.FieldCase.Default.ToString()
                             : FrameworkUAD.BusinessLogic.Enums.FieldCase.None.ToString())
                          : td.FieldCase)
                    });
                }
            }
            return Json(dp);
        }
        public ActionResult GetFolderList(int ClientID)
        {
            int CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(ClientID, false).CustomerID;
            List<ECN_Framework_Entities.Communicator.Folder> folders = ECN_Framework_BusinessLayer.Communicator.Folder.GetByCustomerID(CustomerID, CurrentUser).Where(x => x.FolderType == "GRP").ToList();
            return Json(folders, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetExistingExportList(int FolderID = 0)
        {
            List<ECN_Framework_Entities.Communicator.Group> groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByFolderID(FolderID,CurrentUser);
            return Json(groupList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExistingCampaigns()
        {
            List<FrameworkUAD.Entity.Campaign> cmpnList = new FrameworkUAD.BusinessLogic.Campaign().Select(CurrentClient.ClientConnections);
            return Json(cmpnList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DownLoadFile(string fileloc)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(fileloc);
            //System.IO.File.Delete(pathDownload);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "filter_report.tsv");
        }
        public ActionResult GetSelectedRegionFiltersByRegionGroups(string regionGroupIDs = "")
        {
            var regionGroupIds = regionGroupIDs.Split(',');
            var list = new List<int>(regionGroupIds.Length);
            regionGroupIds.ToList().ForEach(i => list.Add(Convert.ToInt32(i)));
            List<SelectListItem> selectList = new List<SelectListItem>();
            var regions = new List<FrameworkUAD_Lookup.Entity.Region>();
            if (regionGroupIDs == null || regionGroupIDs.Count() == 0)
            {
                regions = FilterService.GetAllRegions();
            }
            else
            {
                regions = FilterService.GetAllRegionByRegionGroups(list);
            }
            regions.ForEach(c => selectList.Add(new SelectListItem() { Text = c.RegionName, Value = c.RegionCode.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMasterGroupCodeSheetAjax(int masterGroupId = 0, string OrderBy = "")
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            List<FrameworkUAD.Entity.MasterCodeSheet> mastercodesheet = new List<FrameworkUAD.Entity.MasterCodeSheet>();
            if (masterGroupId > 0)
                mastercodesheet = new FrameworkUAD.BusinessLogic.MasterCodeSheet().SelectMasterGroupID(CurrentClient.ClientConnections, masterGroupId);
            else
                mastercodesheet = new FrameworkUAD.BusinessLogic.MasterCodeSheet().Select(CurrentClient.ClientConnections);

            if (OrderBy == "v")
                mastercodesheet = mastercodesheet.OrderBy(x => x.MasterValue).ToList();
            else if (OrderBy == "d")
                mastercodesheet = mastercodesheet.OrderBy(x => x.MasterDesc).ToList();
            else
                mastercodesheet = mastercodesheet.OrderBy(x => x.SortOrder).ToList();
            mastercodesheet.ForEach(c => selectList.Add(new SelectListItem() { Text = c.MasterDesc + ' ' + '(' + c.MasterValue + ')', Value = c.MasterID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetResponseGroupCodeSheetAjax(int pubID, int responseGroupId = 0, string OrderBy = "", string ResponseGroup = "")
        {
            List<dynamic> selectList = new List<dynamic>();
            //List<SelectListItem> selectList = new List<SelectListItem>();
            List<FrameworkUAD.Entity.CodeSheet> mastercodesheet = new List<FrameworkUAD.Entity.CodeSheet>();
            if (responseGroupId > 0)
                mastercodesheet = new FrameworkUAD.BusinessLogic.CodeSheet().Select(pubID, CurrentClient.ClientConnections).Where(x => x.ResponseGroupID == responseGroupId && x.IsActive == true).ToList();
            else if (!string.IsNullOrEmpty(ResponseGroup))
                mastercodesheet = new FrameworkUAD.BusinessLogic.CodeSheet().Select(pubID, CurrentClient.ClientConnections).Where(x => x.ResponseGroup == ResponseGroup && x.IsActive == true).ToList();
            else
                mastercodesheet = new FrameworkUAD.BusinessLogic.CodeSheet().Select(pubID, CurrentClient.ClientConnections).Where(x => x.IsActive == true).ToList();

            if (OrderBy == "v")
                mastercodesheet = mastercodesheet.OrderBy(x => x.ResponseValue).ToList();
            else if (OrderBy == "d")
                mastercodesheet = mastercodesheet.OrderBy(x => x.ResponseDesc).ToList();
            else
                mastercodesheet = mastercodesheet.OrderBy(x => x.DisplayOrder).ToList();
            mastercodesheet.ForEach(c => selectList.Add(new { Text = "(" + c.ResponseValue + ")" + c.ResponseDesc, Value = c.CodeSheetID.ToString(), ResponseGroupID = c.ResponseGroupID }));
            //mastercodesheet.ForEach(c => selectList.Add(new SelectListItem() { Text = "(" + c.ResponseValue + ")"+c.ResponseDesc , Value = c.CodeSheetID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSelectedListByMarketID(int brandID = 0, string MarketIDs = "")
        {
            var marketIds = MarketIDs.Split(',');
            var list = new List<int>(marketIds.Length);
            marketIds.ToList().ForEach(i => list.Add(Convert.ToInt32(i))); // maybe Convert.ToInt32() is better?
            XmlDocument doc = new XmlDocument();
            var markets = FilterService.GetAllMarket(CurrentClient.ClientConnections);

            foreach (var m in markets)
            {
                if (list.Contains(m.MarketID))
                    doc.LoadXml(m.MarketXML);
            }

            int pubTypeID = 0;

            var lstpubtypes = FilterService.GetAllProductTypes(CurrentClient.ClientConnections);

            var lpubs = FilterService.GetAllProducts(CurrentClient.ClientConnections);

            List<Dictionary<string, string>> lstDict = new List<Dictionary<string, string>>();
            //Pubs

            XmlNode node = doc.SelectSingleNode("//Market/MarketType[@ID ='P']");
            foreach (var p in lstpubtypes)
            {
                var dSelected = new Dictionary<string, string>();
                if (node != null)
                {
                    string selectedPubsValues = string.Empty;

                    foreach (XmlNode child in node.ChildNodes)
                    {
                        try
                        {
                            var pub = lpubs.Where(x => x.PubID == Convert.ToInt32(child.Attributes["ID"].Value)).FirstOrDefault();
                            if (pub.PubTypeID == p.PubTypeID)
                                selectedPubsValues += selectedPubsValues != string.Empty ? "," + child.Attributes["ID"].Value : "" + child.Attributes["ID"].Value;

                        }
                        catch
                        {
                        }
                    }
                    if (selectedPubsValues.Length > 0)
                    {
                        dSelected.Add("lstPubType_" + p.PubTypeID.ToString(), selectedPubsValues);
                        lstDict.Add(dSelected);
                    }

                }

            }

            // Dimensions
            List<FrameworkUAD.Entity.MasterGroup> masterGroup = new List<FrameworkUAD.Entity.MasterGroup>();
            if (brandID > 0)
                masterGroup = FilterService.GetAllMasterGroupsByBrand(CurrentClient.ClientConnections, brandID);
            else
                masterGroup = FilterService.GetAllMasterGroups(CurrentClient.ClientConnections);

            List<FrameworkUAD.Entity.MasterCodeSheet> mastercodesheet = new List<FrameworkUAD.Entity.MasterCodeSheet>();

            if (brandID > 0)
                mastercodesheet = new FrameworkUAD.BusinessLogic.MasterCodeSheet().SelectMasterBrandID(CurrentClient.ClientConnections, brandID);
            else
                mastercodesheet = new FrameworkUAD.BusinessLogic.MasterCodeSheet().Select(CurrentClient.ClientConnections);

            foreach (var mg in masterGroup)
            {

                if (mg != null)
                {
                    XmlNode dnode = doc.SelectSingleNode("//Market/MarketType[@ID ='D']/Group[@ID = '" + mg.ColumnReference.ToString() + "']");
                    if (dnode != null)
                    {
                        var dSelectedDim = new Dictionary<string, string>();

                        string selectedValues = string.Empty;

                        foreach (XmlNode child in dnode.ChildNodes)
                        {
                            try
                            {
                                selectedValues += selectedValues != string.Empty ? "," + child.Attributes["ID"].Value : "" + child.Attributes["ID"].Value;

                            }
                            catch
                            {
                            }
                        }
                        if (selectedValues.Length > 0)
                        {
                            dSelectedDim.Add("lstDimension_" + mg.MasterGroupID, selectedValues);
                            lstDict.Add(dSelectedDim);
                        }



                    }

                }
            }


            return Json(lstDict, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetProductsByProductType(int pubTypeID = 0, string OrderBy = "")
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            var p = FilterService.GetAllProducts(CurrentClient.ClientConnections);
            var pubsbypubtype = p.Where(x => x.PubTypeID == pubTypeID).ToList();
            if (OrderBy == "v")
                pubsbypubtype = pubsbypubtype.OrderBy(x => x.PubCode).ToList();
            else if (OrderBy == "d")
                pubsbypubtype = pubsbypubtype.OrderBy(x => x.PubName).ToList();
            else
                pubsbypubtype = pubsbypubtype.OrderBy(x => x.SortOrder).ToList();
            pubsbypubtype.ForEach(c => selectList.Add(new SelectListItem() { Text = c.PubName, Value = c.PubID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetQSourceListAjax()
        {
            List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new FrameworkUAD_Lookup.BusinessLogic.CodeType().Select();
            List<FrameworkUAD_Lookup.Entity.Code> codeList = new FrameworkUAD_Lookup.BusinessLogic.Code().Select();
            int qSourceType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source.ToString().Replace("_", " "))).CodeTypeId;
            var qSourceList = codeList.Where(x => x.CodeTypeId == qSourceType).OrderBy(x => x.DisplayOrder).ToList();
            List<SelectListItem> selectList = new List<SelectListItem>();
            qSourceList.ForEach(c => selectList.Add(new SelectListItem() { Text = c.DisplayName, Value = c.CodeId.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetCampaign()
        {
            List<FrameworkUAD.Entity.Campaign> lstCmpn = new FrameworkUAD.BusinessLogic.Campaign().Select(CurrentClient.ClientConnections);
            List<SelectListItem> selectList = new List<SelectListItem>();
            lstCmpn.ForEach(c => selectList.Add(new SelectListItem() { Text = c.CampaignName, Value = c.CampaignID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetDomains()
        {
            List<FrameworkUAD.Entity.DomainTracking> lstDomain = new FrameworkUAD.BusinessLogic.DomainTracking().Select(CurrentClient.ClientConnections);
            List<SelectListItem> selectList = new List<SelectListItem>();
            lstDomain.ForEach(c => selectList.Add(new SelectListItem() { Text = c.DomainName, Value = c.DomainTrackingID.ToString() }));
            return Json(selectList, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Private Methods

        private void UpdateCircFilterViewModel(FilterViewModel model, int pubId)
        {
            Guard.NotNull(model, nameof(model));
            if (PlatformUser.HasService(CurrentUser, EnumServices.FULFILLMENT))
            {
                model.showDimensionPanel = false;
                model.showPubTypePanel = false;
                model.showDCPanel = false;
                model.showBrandPanel = false;
                model.showProductPanel = false;
                model.showSavedLink = false;
                model.showMarketPanel = false;
                model.showDemoPanel = true;
                model.DynamicData = new DynamicFilter();
                model.DynamicData.DemosFilterList = GetAllResponseGroup(pubId);
            }
        }

        private bool CheckViewAccess(string viewType)
        {
            Guard.NotNullOrWhitespace(viewType, nameof(viewType));
            var hasAccess = true;

            if (viewType.Equals(ConsensusViewType, StringComparison.OrdinalIgnoreCase))
            {
                if (!PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.ConsensusView, EnumAccess.View))
                {
                    hasAccess = false;
                }
            }
            else if (viewType.Equals(RecencyViewType, StringComparison.OrdinalIgnoreCase))
            {
                if (!PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.RecencyView, EnumAccess.View))
                {
                    hasAccess = false;
                }
            }
            else if (viewType.Equals(ProductViewType, StringComparison.OrdinalIgnoreCase))
            {
                if (!PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.ProductView, EnumAccess.View))
                {
                    hasAccess = false;
                }
            }
            else if (viewType.Equals(CrossProductViewType, StringComparison.OrdinalIgnoreCase))
            {
                if (!PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.CrossProductView, EnumAccess.View))
                {
                    hasAccess = false;
                }
            }

            return hasAccess;
        }

        private void UpdateUadFilterModel(FilterViewModel model, int brandId)
        {
            Guard.NotNull(model, nameof(model));

            if (model.ViewType.Equals(ConsensusViewType, StringComparison.OrdinalIgnoreCase)
                || model.ViewType.Equals(RecencyViewType, StringComparison.OrdinalIgnoreCase))
            {
                model.showProductPanel = false;
            }
            else if (model.ViewType.Equals(ProductViewType, StringComparison.OrdinalIgnoreCase)
                     || model.ViewType.Equals(CrossProductViewType, StringComparison.OrdinalIgnoreCase))
            {
                model.showPubTypePanel = false;
                model.showProductPanel = true;
            }

            //Enable Disable Saved Links
            if (!PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.UADFilter, EnumAccess.View))
            {
                model.IsSavedlinkEnabled = false;
            }

            if (!PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.UADFilter, EnumAccess.Edit))
            {
                model.HasEditAccessToSaved = false;
            }

            UpdateBrandFilterList(model, brandId);

            //Get DC filter
            if (PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.DataCompare, EnumAccess.Yes))
            {
                model.DcFilterList = GetDCFileFilter(CurrentClientID);
            }
        }

        private void UpdateBrandFilterList(FilterViewModel model, int brandId)
        {
            Guard.NotNull(model, nameof(model));
            var isBrandAssignedUser = false;
            var brandSelectList = new List<SelectListItem>();
            if (!PlatformUser.IsAdministrator(CurrentUser))
            {
                brandSelectList = GetAllActiveBrandsByUser();
                if (brandSelectList.Count > 0)
                {
                    isBrandAssignedUser = true;
                }
            }

            if (brandSelectList.Count == 0)
            {
                brandSelectList = GetAllActiveBrands();
            }

            if (brandSelectList.Count > 0)
            {
                if (PlatformUser.IsAdministrator(CurrentUser) || !isBrandAssignedUser)
                {
                    model.BrandFilterList.Add(new SelectListItem { Text = TextAllProducts, Value = ItemValueZero });
                    model.BrandFilterList.AddRange(brandSelectList);
                }

                if (brandSelectList.Count > 1)
                {
                    if (PlatformUser.IsAdministrator(CurrentUser) || !isBrandAssignedUser)
                    {
                        UpdateDynamicDataOrProductFilterListByViewType(model, brandId);
                    }
                    else
                    {
                        model.BrandFilterList.Add(new SelectListItem { Text = string.Empty, Value = ItemValueDefault });
                        model.IsAdhoclinkEnabled = false;
                        model.IsActivitylinkEnabled = false;
                    }
                }
                else
                {
                    UpdateDynamicDataOrProductFilterListByViewType(model, brandId);
                }

                model.showBrandPanel = true;
            }
            else
            {
                UpdateDynamicDataOrProductFilterListByViewType(model, brandId);
            }
        }

        private void UpdateDynamicDataOrProductFilterListByViewType(FilterViewModel model, int brandId)
        {
            Guard.NotNull(model, nameof(model));
            if (model.ViewType.Equals(ConsensusViewType, StringComparison.OrdinalIgnoreCase)
                || model.ViewType.Equals(RecencyViewType, StringComparison.OrdinalIgnoreCase))
            {
                model.DynamicData = GetConsAndRecDynamicData(brandId);
            }
            else if (model.ViewType.Equals(ProductViewType, StringComparison.OrdinalIgnoreCase)
                     || model.ViewType.Equals(CrossProductViewType, StringComparison.OrdinalIgnoreCase))
            {
                model.ProductFilterList = GetAllSearchEnabledProducts(brandId);
            }
        }

        private JsonResult CheckSaveFilterModelError(SaveFilterViewModel saveModel)
        {
            Guard.NotNull(saveModel, nameof(saveModel));
            var localFilterId = 0;

            if (saveModel.Mode.Equals(SaveModeAddNew, StringComparison.OrdinalIgnoreCase)
                || saveModel.Mode.Equals(SaveModeEdit, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(saveModel.FilterName))
                {
                    return ErrorJsonResult(MessageEnterValidFilterName);
                }

                saveModel.FilterName = saveModel.FilterName.Trim();

                if (saveModel.Mode.Equals(SaveModeEdit, StringComparison.OrdinalIgnoreCase))
                {
                    localFilterId = saveModel.FilterID;
                }

                if (new UadFilterWorker().ExistsByFilterName(CurrentClient.ClientConnections, localFilterId, saveModel.FilterName))
                {
                    return ErrorJsonResult(MessageFilterNameExists);
                }
            }
            else
            {
                if (saveModel.FilterID <= 0)
                {
                    return ErrorJsonResult(MessageSelectFilter);
                }

                localFilterId = saveModel.FilterID;
            }

            if (saveModel.IsAddedToSalesView)
            {
                if (saveModel.QuestionCategoryID <= 0)
                {
                    return ErrorJsonResult(MessageSelectQuestionCategory);
                }

                if (new UadFilterWorker().ExistsQuestionName(CurrentClient.ClientConnections, localFilterId, saveModel.QuestionName))
                {
                    return ErrorJsonResult(MessageQuestionNameExists);
                }
            }

            return null;
        }

        private int SaveUadFilter(SaveFilterViewModel saveModel)
        {
            Guard.NotNull(saveModel, nameof(saveModel));

            var uadFilter = new FrameworkUAD.Object.UADFilter
            {
                Name = saveModel.FilterName,
                IsLocked = saveModel.IsLockedForSharing,
                FilterCategoryID = saveModel.FilterCategoryID,
                AddtoSalesView = saveModel.IsAddedToSalesView
            };

            if (saveModel.Mode.Equals(SaveModeAddNew, StringComparison.OrdinalIgnoreCase))
            {
                uadFilter.CreatedUserID = UserID;
                uadFilter.CreatedDate = DateTime.Now;
                uadFilter.UpdatedUserID = UserID;
                uadFilter.UpdatedDate = DateTime.Now;
                uadFilter.FilterType = saveModel.viewType;
                uadFilter.PubID = saveModel.PubID;
                uadFilter.BrandID = saveModel.BrandID;
                uadFilter.IsDeleted = false;
                uadFilter.Notes = saveModel.Notes;
            }
            else
            {
                uadFilter = new UadFilterWorker().GetByID(CurrentClient.ClientConnections, saveModel.FilterID);
                uadFilter.UpdatedUserID = UserID;
                uadFilter.UpdatedDate = DateTime.Now;
                uadFilter.Notes = saveModel.Notes;
            }

            if (saveModel.IsAddedToSalesView)
            {
                uadFilter.QuestionName = saveModel.QuestionName;
                uadFilter.QuestionCategoryID = Convert.ToInt32(saveModel.QuestionCategoryID);
            }
            else
            {
                uadFilter.QuestionName = string.Empty;
                uadFilter.QuestionCategoryID = 0;
            }

            return new UadFilterWorker().insert(CurrentClient.ClientConnections, uadFilter);
        }

        private JsonResult ErrorJsonResult(string message, bool isError = true)
        {
            return Json(new { error = isError, errormessage = message }, JsonRequestBehavior.AllowGet);
        }

        private void SaveFilterDetails(
            SaveFilterViewModel saveModel,
            int savedFilterId,
            ICollection<EntityFilterGroup> filterGroups,
            bool deleteGroup)
        {
            Guard.NotNull(saveModel, nameof(saveModel));
            Guard.NotNull(filterGroups, nameof(filterGroups));

            var index = 1;
            var idArray = saveModel.FilterIDs.Split(IdSeparator);

            foreach (var fId in idArray)
            {
                var filter = saveModel.CurrentFilter;
                int filterGroupId;

                if (saveModel.NewExisting.Equals(SaveModeExisting, StringComparison.OrdinalIgnoreCase) && !deleteGroup)
                {
                    filterGroupId = filterGroups.ElementAtOrDefault(index - 1)?.FilterGroupID ?? 0;
                }
                else
                {
                    filterGroupId = new FilterGroupWorker().Save(CurrentClient.ClientConnections, savedFilterId, index);
                }

                foreach (var field in filter.Fields)
                {
                    if (!field.Name.Equals(NameDataCompare, StringComparison.OrdinalIgnoreCase))
                    {
                        var details = new FrameworkUAD.Object.FilterDetails
                        {
                            FilterType = field.FilterType,
                            Group = field.Group,
                            Name = field.Name,
                            Values = field.Values,
                            SearchCondition = string.IsNullOrWhiteSpace(field.SearchCondition) ? DoubleSpace : field.SearchCondition,
                            FilterGroupID = filterGroupId
                        };

                        new FilterDetailsWorker().Save(CurrentClient.ClientConnections, details);
                    }
                }

                index++;
            }
        }

        private void UpdateDownloadViewModel(DownloadViewModel model)
        {
            Guard.NotNull(model, nameof(model));
            model.IsArchived = false;
            model.DownloadVisible = true; //Always true for every option

            // Check if Data required from Archive
            if (model.IssueID == 0)
            {
                model.IsArchived = false;
            }
            else
            {
                var issueList = new FrameworkUAD.BusinessLogic.Issue().Select(CurrentClient.ClientConnections);
                var issue = issueList.First(x => x.IssueId == model.IssueID);
                if (issue.IsComplete)
                {
                    model.IsArchived = true;
                }
            }

            // Remain False for Add remove ,Record Update and IssueSplit
            model.ExportToGroupVisible = false;
            model.SaveToCampaignVisible = false;
            model.PromoCodeVisible = false;
            model.IsQueryDetailsIncluded = false;
            model.QueryDetailsCheckboxVisible = false;
        }

        private void UpdateDownloadViewModelForReport(DownloadViewModel model, List<int> subscriberIds)
        {
            Guard.NotNull(model, nameof(model));
            Guard.NotNull(subscriberIds, nameof(subscriberIds));

            model.PromoCodeVisible = true;
            model.QueryDetailsCheckboxVisible = true;
            if (model.IsArchived)
            {
                model.ExportToGroupVisible = false;
                model.SaveToCampaignVisible = false;
                model.DisplayDownLoad = false;
            }
            else
            {
                model.ExportToGroupVisible = true;
                model.SaveToCampaignVisible = true;
            }

            foreach (var filter in model.FilterList)
            {
                var filterQuery = model.IsArchived
                    ? FilterMvcWorker.getProductArchiveFilterQuery(filter, model.IssueID, CurrentClient.ClientConnections)
                    : FilterMvcWorker.getProductFilterQuery(filter, CurrentClient.ClientConnections);

                var list = new ReportWorker().SelectSubscriberCountMVC(filterQuery, CurrentClient.ClientConnections);

                if (list?.Count > 0)
                {
                    subscriberIds.AddRange(list);
                }
            }

            var clients = new ClientWorker().SelectActiveForClientGroup(model.CustomerClientGroupID).ToList();
            clients.ForEach(x => model.Customers.Add(new SelectListItem
            {
                Text = x.ClientName,
                Value = x.ClientID.ToString()
            }));

            var clientTemplates = DownloadTemplateWorker.GetByPubIDBrandID(CurrentClient.ClientConnections, model.PubID, 0);
            clientTemplates.ForEach(x => model.DownLoadTemplates.Add(new SelectListItem
            {
                Text = x.DownloadTemplateName,
                Value = x.DownloadTemplateID.ToString()
            }));
        }

        private void UpdateDownloadViewModelForAddRemove(DownloadViewModel model, ICollection<int> subscriberIds)
        {
            Guard.NotNull(model, nameof(model));
            Guard.NotNull(subscriberIds, nameof(subscriberIds));

            model.DownloadVisible = false;

            //Convert subscriberIds list from PubSubscriptionIDs to SubscriptionIDs as AddRemove uses PubSubscriptionIDs
            var pubSubIds = (List<int>)Session[KeyAddRemoveSubscriberIds];

            var subscriptionWorker = new FrameworkUAD.BusinessLogic.ProductSubscription();
            var subscriptionList = subscriptionWorker.SelectProductID(model.PubID, CurrentClient.ClientConnections);
            foreach(var subId in pubSubIds)
            {
                var subscription = subscriptionList.FirstOrDefault(sub => sub.PubSubscriptionID == subId);
                if (subscription != null)
                {
                    subscriberIds.Add(subscription.SubscriptionID);
                }
            }

            Session[KeyAddRemoveSubscriberIds] = null;

            var addRemoveClientTemplates = DownloadTemplateWorker.GetByPubIDBrandID(CurrentClient.ClientConnections, model.PubID, 0);
            addRemoveClientTemplates.ForEach(template => model.DownLoadTemplates.Add(new SelectListItem
            {
                Text = template.DownloadTemplateName,
                Value = template.DownloadTemplateID.ToString()
            }));
        }

        private void UpdateDownloadViewModelForArchive(DownloadViewModel model)
        {
            Guard.NotNull(model, nameof(model));
            if (model.IssueID == 0)
            {
                model.IsArchived = false;
            }
            else
            {
                var issueList = new FrameworkUAD.BusinessLogic.Issue().Select(CurrentClient.ClientConnections);
                var issue = issueList.First(x => x.IssueId == model.IssueID);
                if (issue.IsComplete)
                {
                    model.IsArchived = true;
                }
            }
        }

        private string CheckErrorForExportData(DownloadViewModel model)
        {
            Guard.NotNull(model, nameof(model));
            var message = string.Empty;

            if (OptionDownload.Equals(model.MasterOptionSelected, StringComparison.OrdinalIgnoreCase)
                || OptionExport.Equals(model.MasterOptionSelected, StringComparison.OrdinalIgnoreCase))
            {
                if (model.SelectedItems.Count == 0)
                {
                    message = MessageNoFieldSelectedForDownload;
                }

                if (OptionExport.Equals(model.MasterOptionSelected, StringComparison.OrdinalIgnoreCase))
                {
                    var exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.ECN;
                    var exportDemoFields = Helpers.Utilities.GetExportingFields(
                        CurrentClient.ClientConnections,
                        model.PubID,
                        exportType,
                        CurrentUser.UserID,
                        FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Demo,
                        model.DownloadFor,
                        model.IsArchived);
                    var exportAdhocFields = Helpers.Utilities.GetExportingFields(
                        CurrentClient.ClientConnections,
                        model.PubID,
                        exportType,
                        CurrentUser.UserID,
                        FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Adhoc,
                        model.DownloadFor,
                        model.IsArchived);
                    var demoFieldsCount = 0;
                    var adhocFieldsCount = 0;

                    foreach (var item in model.SelectedItems.Where(s => s.Selected))
                    {
                        if (exportDemoFields.Any(x => x.Key.Split(FieldSeparator)[0] == item.Value.Split(FieldSeparator)[0]))
                        {
                            demoFieldsCount++;
                        }
                        else if (exportAdhocFields.Any(x => x.Key.Split(FieldSeparator)[0] == item.Value.Split(FieldSeparator)[0]))
                        {
                            adhocFieldsCount++;
                        }
                    }

                    if (demoFieldsCount > MaxExportFieldCount)
                    {
                        message = MessageDemoFieldShouldNotMoreThan5;
                    }

                    if (adhocFieldsCount > MaxExportFieldCount)
                    {
                        message = MessageAdhocFieldShouldNotMoreThan5;
                    }
                }
            }

            return message;
        }

        private void GenerateColumnsForDownloadAndExport(
            DownloadViewModel model,
            List<string> selectedItems,
            ref List<string> pubSubscriptionsExtMapperValueList,
            ref List<string> responseGroupIdList,
            ref List<string> responseGroupDescIdList,
            ref List<string> stdColumnList,
            ref List<string> customColumnList)
        {
            Guard.NotNull(model, nameof(model));
            Guard.NotNull(selectedItems, nameof(selectedItems));

            if (OptionDownload.Equals(model.MasterOptionSelected, StringComparison.OrdinalIgnoreCase)
                || OptionExport.Equals(model.MasterOptionSelected, StringComparison.OrdinalIgnoreCase))
            {
                foreach (var item in model.SelectedItems)
                {
                    selectedItems.Add(item.Value);
                }

                var standardColumnList = new List<string>();

                if (model.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                {
                    //Get selected adhoc columns
                    pubSubscriptionsExtMapperValueList = Helpers.Utilities.GetSelectedPubSubExtMapperExportColumns(
                        CurrentClient.ClientConnections,
                        selectedItems,
                        model.PubID);

                    //Get selected response group value and response group desc columns
                    var groups = Helpers.Utilities.GetSelectedResponseGroupStandardExportColumns(
                        CurrentClient.ClientConnections,
                        selectedItems,
                        model.PubID,
                        model.MasterOptionSelected == OptionExport);
                    responseGroupIdList = groups.Item1;
                    responseGroupDescIdList = groups.Item2;

                    standardColumnList = groups.Item3;
                }

                stdColumnList = Helpers.Utilities.GetStandardExportColumnFieldName(
                    standardColumnList,
                    model.ViewType,
                    0,
                    model.MasterOptionSelected == OptionExport,
                    model.DownloadFor == DownloadForIssueSplit)
                    .ToList();

                customColumnList = Helpers.Utilities.GetSelectedCustomExportColumns(selectedItems);
            }
        }

        private ActionResult ExportDataForReport(
            DownloadViewModel model,
            List<string> pubSubscriptionsExtMapperValueList,
            List<string> responseGroupIdList,
            List<string> responseGroupDescIdList,
            List<string> stdColumnList,
            List<string> customColumnList)
        {
            Guard.NotNull(model, nameof(model));
            var filePath = string.Empty;
            var dtSubscription = new DataTable();
            var filterList = (List<EntityFilterMvc>)Session[KeyCurrentFilters];
            var filterCollection = new EntityFilterCollection(CurrentClient.ClientConnections, CurrentUser.UserID);
            var filter = filterList.First();
            filter.FilterNo = 1;
            filterCollection.Add(filter);
            var queries = GetExportReportQueries(model.PubID, filterCollection);

            if (OptionDownload.Equals(model.MasterOptionSelected, StringComparison.OrdinalIgnoreCase)
                || OptionExport.Equals(model.MasterOptionSelected, StringComparison.OrdinalIgnoreCase))
            {
                dtSubscription = GetExportSubscriberData(
                    model,
                    pubSubscriptionsExtMapperValueList,
                    responseGroupIdList,
                    responseGroupDescIdList,
                    stdColumnList,
                    customColumnList,
                    filter,
                    queries);

                model.HeaderText = Helpers.Utilities.GetHeaderText(
                    filterCollection,
                    SelectedFilterNo,
                    string.Empty,
                    SelectedFilterOperation,
                    string.Empty,
                    false);
            }

            if (OptionExport.Equals(model.MasterOptionSelected, StringComparison.OrdinalIgnoreCase))
            {
                return ExportReportToGroup(model, dtSubscription);
            }

            if (OptionDownload.Equals(model.MasterOptionSelected, StringComparison.OrdinalIgnoreCase))
            {
                return ExportReportForDownload(model, dtSubscription);
            }

            if (model.MasterOptionSelected == OptionSaveToCampaign)
            {
                return ExportReportForSaveToCampaign(model, pubSubscriptionsExtMapperValueList, customColumnList, queries);
            }

            return Json(filePath);
        }

        private StringBuilder GetExportReportQueries(int pubId, EntityFilterCollection filterCollection)
        {
            Guard.NotNull(filterCollection, nameof(filterCollection));
            var queries = FilterMvcWorker.generateCombinationQuery(
                filterCollection,
                SelectedFilterOperation,
                string.Empty,
                SelectedFilterNo,
                string.Empty,
                string.Empty,
                pubId,
                0,
                CurrentClient.ClientConnections);

            return queries;
        }

        private DataTable GetExportSubscriberData(
            DownloadViewModel model,
            List<string> pubSubscriptionsExtMapperValueList,
            List<string> responseGroupIdList,
            List<string> responseGroupDescIdList,
            List<string> stdColumnList,
            List<string> customColumnList,
            EntityFilterMvc filter,
            StringBuilder queryBuilder)
        {
            Guard.NotNull(model, nameof(model));
            DataTable dataTable;

            if (model.IsArchived)
            {
                var query = FilterMvcWorker.getProductArchiveFilterQuery(
                    filter,
                    DistinctSubIdQuery,
                    string.Empty,
                    model.IssueID,
                    CurrentClient.ClientConnections);
                dataTable = new FrameworkUAD.BusinessLogic.Subscriber().GetArchivedProductDimensionSubscriberData(
                        CurrentClient.ClientConnections,
                        query,
                        stdColumnList,
                        new List<int>{model.PubID},
                        responseGroupIdList,
                        responseGroupDescIdList,
                        pubSubscriptionsExtMapperValueList,
                        customColumnList,
                        0,
                        model.IssueID,
                        model.DownloadCount);
            }
            else
            {
                dataTable = new FrameworkUAD.BusinessLogic.Subscriber().GetProductDimensionSubscriberData(
                        CurrentClient.ClientConnections,
                        queryBuilder,
                        stdColumnList,
                        new List<int>{model.PubID},
                        responseGroupIdList,
                        responseGroupDescIdList,
                        pubSubscriptionsExtMapperValueList,
                        customColumnList,
                        0,
                        model.DownloadCount);
            }

            return dataTable;
        }

        private ActionResult ExportReportToGroup(DownloadViewModel model, DataTable subscriptionTable)
        {
            Guard.NotNull(model, nameof(model));
            Guard.NotNull(subscriptionTable, nameof(subscriptionTable));
            var filePath = string.Empty;
            if (!PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.UADExport, EnumAccess.ExportToGroup))
            {
                return Json(filePath);
            }

            var groupId = 0;
            var suppressMaster = false;
            var folderId = 0;
            var customerId = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(model.CustomerClientID, false).CustomerID;

            if (model.IsExistingGroupChecked)
            {
                groupId = model.GroupID;
                var modelGroup = GroupWorker.GetByGroupID(groupId, CurrentUser);
                suppressMaster = modelGroup.MasterSupression == 1;
                folderId = model.FolderID;
            }
            else if (model.IsNewGroupChecked)
            {
                folderId = model.FolderID;
                var groupName = CleanString(model.GroupName);

                if (GroupWorker.ExistsByGroupNameByCustomerID(groupName, customerId))
                {
                    return Json(new { error = true, errormessage = string.Format(MessageGroupNameExists, groupName) });
                }

                groupId = Helpers.Utilities.InsertGroup(groupName, customerId, folderId);
            }
            else
            {
                return Json(new {error = true, errormessage = MessageSelectGroupForExportData});
            }

            try
            {
                if (suppressMaster)
                {
                    RemoveNonProfileColumns(subscriptionTable);
                }

                var updatedRecords = GetExportDataUpdatedRecords(model, subscriptionTable, customerId, groupId, folderId);
                if (updatedRecords.Count > 0)
                {
                    var dtLocal = Helpers.Utilities.getImportedResult(updatedRecords, DateTime.Now);
                    var view = dtLocal.DefaultView;
                    view.Sort = SortOrderAsc;
                    dtLocal = view.ToTable();

                    return PartialView(ExportResultPartialView, dtLocal);
                }
            }
            catch (Exception ex)
            {
                Helpers.Utilities.Log_Error(Request.RawUrl, "DetailsDownload - group export", ex);
                return Json(new {error = true, errormessage = ex.Message});
            }

            return Json(filePath);
        }

        private ActionResult ExportReportForDownload(DownloadViewModel model, DataTable subscriptionTable)
        {
            Guard.NotNull(model, nameof(model));
            Guard.NotNull(subscriptionTable, nameof(subscriptionTable));
            var filePath = string.Empty;
            if (!PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.UADExport, EnumAccess.Download))
            {
                return Json(filePath);
            }

            var hasSubId = false;
            SetExportColumnOrder(model, subscriptionTable, out hasSubId);

            subscriptionTable = (DataTable)FrameworkUAD.Object.ProfileFieldMask.MaskData(CurrentClient.ClientConnections, subscriptionTable, CurrentUser);

            var path = Server.MapPath(DownloadRootPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileName = $"{Guid.NewGuid().ToString().Substring(0, RandomFileNameLength)}{TsvExtension}";
            filePath = path + fileName;

            // DownLoad File to Output Path On Server
            if (!string.IsNullOrWhiteSpace(model.PromoCode.Trim()))
            {
                var newColumn = new DataColumn(ColumnPromoCode, typeof(string));
                newColumn.DefaultValue = model.PromoCode;
                subscriptionTable.Columns.Add(newColumn);
            }

            if (!hasSubId && subscriptionTable.Columns.Contains(FieldSubscriptionId))
            {
                subscriptionTable.Columns.Remove(FieldSubscriptionId);
            }

            if (!model.IsQueryDetailsIncluded)
            {
                model.HeaderText = string.Empty;
            }

            Helpers.Utilities.Download(subscriptionTable, filePath, model.HeaderText, model.TotalCount, model.DownloadCount);

            return Json(filePath);
        }

        private ActionResult ExportReportForSaveToCampaign(
            DownloadViewModel model,
            List<string> pubSubscriptionsExtMapperValueList,
            List<string> customColumnList,
            StringBuilder queries)
        {
            Guard.NotNull(model, nameof(model));
            if (!PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.UADExport, EnumAccess.SaveToCampaign))
            {
                return Json(string.Empty);
            }

            var masterGroupColumnList = new List<string>();
            var masterGroupColumnDescList = new List<string>();
            var columnList = new List<string> { ColumnSubIdNone, ColumnGroupNoNone };
            var subscriptionTable = new FrameworkUAD.BusinessLogic.Subscriber().GetSubscriberData(
                CurrentClient.ClientConnections,
                queries,
                columnList,
                masterGroupColumnList,
                masterGroupColumnDescList,
                pubSubscriptionsExtMapperValueList,
                customColumnList,
                0,
                new List<int> {model.PubID},
                false,
                model.DownloadCount);

            var campaignId = 0;
            if (!TrySaveNewCampaign(model, out campaignId))
            {
                return ErrorJsonResult(string.Format(MessageNameAlreadyExists, model.CampaignName));
            }

            var campaignFilterId = 0;
            if (new CampaignFilterWorker().CampaignFilterExists(CurrentClient.ClientConnections, model.FilterName, campaignId) == 0)
            {
                campaignFilterId = new CampaignFilterWorker().Insert(
                    CurrentClient.ClientConnections,
                    model.FilterName,
                    CurrentUser.UserID,
                    campaignId,
                    model.PromoCode);
            }
            else
            {
                return ErrorJsonResult(string.Format(MessageNameAlreadyExists, model.FilterName));
            }

            return SaveCampaignDetails(subscriptionTable, campaignFilterId, campaignId);
        }

        private bool TrySaveNewCampaign(DownloadViewModel model, out int campaignId)
        {
            Guard.NotNull(model, nameof(model));

            campaignId = 0;
            if (model.IsNewCampaign)
            {
                if (new CampaignWorker().CampaignExists(CurrentClient.ClientConnections, model.CampaignName) == 0)
                {
                    campaignId = new CampaignWorker().Save(new FrameworkUAD.Entity.Campaign
                        {
                            CampaignName = model.CampaignName,
                            AddedBy = CurrentUser.UserID,
                            BrandID = 0,
                            DateAdded = DateTime.Now,
                            DateUpdated = DateTime.Now
                        },
                        CurrentClient.ClientConnections);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                campaignId = model.CampaignID;
            }

            return true;
        }

        private ActionResult SaveCampaignDetails(DataTable subscriptionTable, int campaignFilterId, int campaignId)
        {
            Guard.NotNull(subscriptionTable, nameof(subscriptionTable));
            var xmlBuilder = new StringBuilder();
            var count = 0;

            try
            {
                foreach (DataRow dr in subscriptionTable.Rows)
                {
                    xmlBuilder.AppendFormat(XmlSidFormat, Helpers.Utilities.cleanXMLString(dr[FieldSubscriptionId].ToString()));

                    if (count != 0 && count % CampaignDetailsBatchSize == 0
                        || count == subscriptionTable.Rows.Count - 1)
                    {
                        new FrameworkUAD.BusinessLogic.CampaignFilterDetail().saveCampaignDetails(
                            CurrentClient.ClientConnections,
                            campaignFilterId,
                            $"{XmlHeaderLine}{xmlBuilder}{XmlCloseTag}");
                        xmlBuilder = new StringBuilder();
                    }

                    count++;
                }

                var campaignCount = new CampaignWorker().GetCountByCampaignID(CurrentClient.ClientConnections, campaignId);
                return SuccessJsonResult($"{MessageTotalCampaignSubscriber}{campaignCount}");
            }
            catch (Exception ex)
            {
                Helpers.Utilities.Log_Error(Request.RawUrl, "DetailsDownload - campaign save", ex);
                return ErrorJsonResult(ex.Message);
            }
        }

        private JsonResult SuccessJsonResult(string message)
        {
            return Json(new { success = true, successmessge = message }, JsonRequestBehavior.AllowGet);
        }

        private void RemoveNonProfileColumns(DataTable dataTable)
        {
            Guard.NotNull(dataTable, nameof(dataTable));
            var columnsToDelete = new List<DataColumn>();

            foreach (DataColumn column in dataTable.Columns)
            {
                if (ProfileFields.Any(field => field.Equals(column.ColumnName, StringComparison.OrdinalIgnoreCase)))
                {
                    columnsToDelete.Add(column);
                }
            }

            foreach (var column in columnsToDelete)
            {
                dataTable.Columns.Remove(column);
            }
        }

        private Hashtable GetExportDataUpdatedRecords(
            DownloadViewModel model,
            DataTable dataTable,
            int customerId,
            int groupId,
            int folderId)
        {
            Guard.NotNull(model, nameof(model));
            Guard.NotNull(dataTable, nameof(dataTable));
            var exportFields = new List<EntityExportFields>();

            foreach (DataColumn column in dataTable.Columns)
            {
                var isUdf = !NonUdfFields.Any(c => c.Equals(column.ColumnName, StringComparison.OrdinalIgnoreCase));
                exportFields.Add(new EntityExportFields(column.ColumnName.ToUpper(), string.Empty, isUdf, 0));
            }

            var promoCode = string.IsNullOrWhiteSpace(model.PromoCode) ? string.Empty : model.PromoCode;
            var updatedRecords = Helpers.Utilities.ExportToECN(
                groupId,
                model.GroupName,
                customerId,
                folderId,
                promoCode,
                model.JobCode,
                exportFields,
                dataTable,
                CurrentUser.UserID,
                FrameworkUAD.BusinessLogic.Enums.GroupExportSource.UADManualExport);

            return updatedRecords;
        }

        private ActionResult ExportDataForAddRemove(
            DownloadViewModel model,
            List<string> pubSubscriptionsExtMapperValueList,
            List<string> responseGroupIdList,
            List<string> responseGroupDescIdList,
            List<string> stdColumnList,
            List<string> customColumnList)
        {
            Guard.NotNull(model, nameof(model));
            var filePath = string.Empty;
            if (!PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.UADExport, EnumAccess.Download))
            {
                return Json(filePath);
            }

            var subIds = (List<int>)Session[KeySubscriberIds];
            var queries = new StringBuilder();
            var master = new FrameworkUAD.BusinessLogic.Subscriber().GetProductDimensionSubscriberData(
                CurrentClient.ClientConnections, queries, stdColumnList, new List<int>() {model.PubID},
                responseGroupIdList, responseGroupDescIdList, pubSubscriptionsExtMapperValueList,
                customColumnList, 0, model.DownloadCount, false, subIds);

            var hasSubId = false;
            SetExportColumnOrder(model, master, out hasSubId);

            if (!hasSubId && master.Columns.Contains(FieldSubscriptionId))
            {
                master.Columns.Remove(FieldSubscriptionId);
            }

            master = (DataTable)FrameworkUAD.Object.ProfileFieldMask.MaskData(CurrentClient.ClientConnections, master, CurrentUser);

            var content = master.DataTableToCSV(IdSeparator);
            var sanitizedName = StringFunctions.CleanProcessCodeForFileName(StringFunctions.GenerateProcessCode());
            AddRemoveCreateTSVFile(sanitizedName, content, master);
            filePath = Server.MapPath($"{DownloadAddRemoveRootPath}{sanitizedName}{TsvExtension}");

            return Json(filePath);
        }

        private void SetExportColumnOrder(DownloadViewModel model, DataTable dataTable, out bool hasSubscriptionId)
        {
            Guard.NotNull(model, nameof(model));
            Guard.NotNull(dataTable, nameof(dataTable));
            var fieldNames = new List<string>();
            hasSubscriptionId = false;
            model.SelectedItems.ForEach(x => fieldNames.Add(x.Text));

            var columnsOrder = new string[fieldNames.Count + 1];
            var i = 0;

            foreach (var item in fieldNames)
            {
                if (item.Equals(FieldSubscriptionId, StringComparison.InvariantCultureIgnoreCase))
                {
                    hasSubscriptionId = true;
                }

                if (model.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                {
                    if (FieldAddress1.Equals(item, StringComparison.OrdinalIgnoreCase))
                    {
                        columnsOrder[i] = ColumnAddress;
                    }
                    else if (FieldRegionCode.Equals(item, StringComparison.OrdinalIgnoreCase))
                    {
                        columnsOrder[i] = ColumnState;
                    }
                    else if (FieldZipCode.Equals(item, StringComparison.OrdinalIgnoreCase))
                    {
                        columnsOrder[i] = ColumnZip;
                    }
                    else if (FieldPubTransactionDate.Equals(item, StringComparison.OrdinalIgnoreCase))
                    {
                        columnsOrder[i] = ColumnTransactionDate;
                    }
                    else if (FieldQualificationDate.Equals(item, StringComparison.OrdinalIgnoreCase))
                    {
                        columnsOrder[i] = ColumnQDate;
                    }
                    else
                    {
                        columnsOrder[i] = item;
                    }

                    i++;
                }
            }

            for (var j = 0; j < columnsOrder.Length; j++)
            {
                if (!string.IsNullOrWhiteSpace(columnsOrder[j]))
                {
                    dataTable.Columns[columnsOrder[j].Split(LeftParentheses)[0]].SetOrdinal(j);
                }
            }
        }

        private GeoFilter GetGeoFilter()
        {
            GeoFilter gf = new GeoFilter();
            gf.AreaList = GetAllAreaFilters();
            gf.CountryList = GetAllCountryFilters();
            gf.RegionGroupsList = GetAllRegionGroupFilters();
            gf.RegionList = GetAllRegionFilters();
            return gf;
        }
        private StandardFilter GetStandardFilter()
        {
            StandardFilter sf = new StandardFilter();
            sf.EmailStatusList = GetAllEmailStatuses();

            return sf;
        }
        private DynamicFilter GetConsAndRecDynamicData(int brandId = 0)
        {
            DynamicFilter dFilter = new DynamicFilter();
            if (KM.Platform.User.IsAdministrator(CurrentUser))
            {

                if (brandId > 0)
                {
                    dFilter.PubTypeFilterList = GetAllProductTypesByBrand(brandId);
                    dFilter.DimensionFilterList = GetAllMasterGroupByBrand(brandId);
                    dFilter.MarketFilterList = GetAllMarketByBrands(brandId);
                }
                else
                {
                    dFilter.PubTypeFilterList = GetAllProductTypes();
                    dFilter.DimensionFilterList = GetAllMasterGroup();
                    dFilter.MarketFilterList = GetAllMarket();
                }

            }

            return dFilter;
        }
        //Get All Area Filter
        private List<SelectListItem> GetAllAreaFilters()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            var countries = FilterService.GetAllAreas();
            var distinctArea = countries.OrderBy(x => x.Area).Where(y => y.Area != "").Select(x => x.Area).Distinct().ToList();
            distinctArea.ForEach(x => selectList.Add(new SelectListItem() { Text = x, Value = x }));
            return selectList;
        }
        //Get All Countries Filter
        private List<SelectListItem> GetAllCountryFilters()
        {
            var countries = FilterService.GetAllCountries();
            var countryList = countries.OrderBy(x => x.SortOrder).ToList();
            List<SelectListItem> selectList = new List<SelectListItem>();
            countryList = countryList.Where(x => x.SortOrder != 0).OrderByDescending(o => o.CountryID == 1)
                .ThenByDescending(o => o.CountryID == 2)
                .ThenByDescending(o => o.CountryID == 429)
                .ThenBy(x => x.ShortName).ToList();
            countryList.ForEach(c => selectList.Add(new SelectListItem() { Text = c.ShortName, Value = c.CountryID.ToString() }));
            return selectList;
        }

        //Get All RegionGroups Filter
        private List<SelectListItem> GetAllRegionGroupFilters()
        {
            var regionGroups = FilterService.GetAllRegionGroups();
            List<SelectListItem> selectList = new List<SelectListItem>();
            regionGroups = regionGroups.OrderBy(x => x.Sortorder).ToList();
            regionGroups.ForEach(c => selectList.Add(new SelectListItem() { Text = c.RegionGroupName, Value = c.RegionGroupID.ToString() }));
            return selectList;
        }
        //Get All RegionsFilters
        private List<SelectListItem> GetAllRegionFilters()
        {
            var regions = FilterService.GetAllRegions();
            List<SelectListItem> selectList = new List<SelectListItem>();
            regions.ForEach(c => selectList.Add(new SelectListItem() { Text = c.RegionName, Value = c.RegionCode.ToString() }));
            return selectList;
        }


        //Get Email Status Filter
        private List<SelectListItem> GetAllEmailStatuses()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            List<FrameworkUAD.Entity.EmailStatus> lstEmailStatus = new FrameworkUAD.BusinessLogic.EmailStatus().Select(CurrentClient.ClientConnections);
            lstEmailStatus.ForEach(c => selectList.Add(new SelectListItem() { Text = c.Status.ToUpper(), Value = c.EmailStatusID.ToString() }));
            return selectList;
        }
        //Get DC Filter
        private List<SelectListItem> GetDCFileFilter(int currentclientID)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            List<FrameworkUAS.Entity.SourceFile> lsf = new FrameworkUAS.BusinessLogic.SourceFile().Select(currentclientID, false);
            List<FrameworkUAS.Entity.DataCompareRun> ldcr = new FrameworkUAS.BusinessLogic.DataCompareRun().SelectForClient(currentclientID);
            var dcFileFilterList = (from s in lsf
                                    join d in ldcr on s.SourceFileID equals d.SourceFileId
                                    orderby d.DateCreated
                                    select new DCFileFilter { FileName = s.FileName + "_" + d.DateCreated, ProcessCode = d.ProcessCode }
                       ).ToList();
            dcFileFilterList.ForEach(c => selectList.Add(new SelectListItem() { Text = c.FileName, Value = c.ProcessCode.ToString() }));

            return selectList;
        }
        //Get All Brands Filter
        private List<SelectListItem> GetAllActiveBrands()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            var brands = FilterService.GetAllActiveBrands(CurrentClient.ClientConnections);
            brands.ForEach(c => selectList.Add(new SelectListItem() { Text = c.BrandName, Value = c.BrandID.ToString() }));
            return selectList;
        }
        //Get Brand Filter by User
        private List<SelectListItem> GetAllActiveBrandsByUser()
        {
            var brands = FilterService.GetAllActiveBrandsByUser(CurrentUser.UserID, CurrentClient.ClientConnections);
            List<SelectListItem> selectList = new List<SelectListItem>();
            brands.ForEach(c => selectList.Add(new SelectListItem() { Text = c.BrandName, Value = c.BrandID.ToString() }));
            return selectList;
        }
        //Get All Market Filter
        private List<SelectListItem> GetAllMarket()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            var markets = FilterService.GetAllMarket(CurrentClient.ClientConnections);
            markets.ForEach(c => selectList.Add(new SelectListItem() { Text = c.MarketName, Value = c.MarketID.ToString() }));
            return selectList;
        }
        //Get All Market Filter By Brands
        private List<SelectListItem> GetAllMarketByBrands(int brandId)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            var markets = FilterService.GetMarketByBrand(CurrentClient.ClientConnections, brandId);
            markets.ForEach(c => selectList.Add(new SelectListItem() { Text = c.MarketName, Value = c.MarketID.ToString() }));
            return selectList;

        }
        //Get All Product Types
        private List<PubTypeFilter> GetAllProductTypes()
        {
            var productTypesFilterList = new List<PubTypeFilter>();
            var p = FilterService.GetAllProducts(CurrentClient.ClientConnections);
            var productTypes = FilterService.GetAllProductTypes(CurrentClient.ClientConnections);
            productTypes.ForEach(x => productTypesFilterList.Add(
                 new PubTypeFilter()
                 {
                     ColumnReference = x.ColumnReference,
                     IsActive = x.IsActive,
                     PubTypeDisplayName = x.PubTypeDisplayName,
                     PubTypeID = x.PubTypeID,
                     SortOrder = x.SortOrder,
                     PubList = p.Select(y => y).Where(y => y.PubTypeID == x.PubTypeID).ToList()

                 }));

            return productTypesFilterList;
        }
        //Get All Product Types by Brand
        private List<PubTypeFilter> GetAllProductTypesByBrand(int brandID)
        {
            var productTypesFilterList = new List<PubTypeFilter>();
            var p = FilterService.GetAllProducts(CurrentClient.ClientConnections);
            var productTypes = FilterService.GetAllProductTypesByBrand(CurrentClient.ClientConnections, brandID);
            productTypes.ForEach(x => productTypesFilterList.Add(
                 new PubTypeFilter()
                 {
                     ColumnReference = x.ColumnReference,
                     IsActive = x.IsActive,
                     PubTypeDisplayName = x.PubTypeDisplayName,
                     PubTypeID = x.PubTypeID,
                     SortOrder = x.SortOrder,
                     PubList = p.Select(y => y).Where(y => y.PubTypeID == x.PubTypeID).ToList()

                 }));

            return productTypesFilterList;
        }
        //Get All MasterGroups 
        private List<Dimension> GetAllMasterGroup()
        {

            var mastergroups = FilterService.GetAllMasterGroups(CurrentClient.ClientConnections);
            List<Dimension> selectList = new List<Dimension>();
            foreach (var mg in mastergroups)
            {
                var dm = new Dimension();
                dm.RefColumn = mg.ColumnReference;
                dm.Text = mg.DisplayName;
                dm.Value = mg.MasterGroupID.ToString();
                dm.DimList = new FrameworkUAD.BusinessLogic.MasterCodeSheet().SelectMasterGroupID(CurrentClient.ClientConnections, mg.MasterGroupID);
                selectList.Add(dm);
            }
            return selectList;

        }
        //Get All Search Enabled Products
        private List<SelectListItem> GetAllSearchEnabledProducts(int brandID)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            if (brandID == 0)
            {
                var pubs = FilterService.GetAllSearchEnabledProducts(CurrentClient.ClientConnections);
                pubs.ForEach(c => selectList.Add(new SelectListItem() { Text = c.PubName, Value = c.PubID.ToString() }));

            }
            else
            {
                var pubs = FilterService.GetAllSearchEnabledProductsByBrand(CurrentClient.ClientConnections, brandID);
                pubs.ForEach(c => selectList.Add(new SelectListItem() { Text = c.PubName, Value = c.PubID.ToString() }));
            }

            return selectList;
        }

        //Get All MasterGroups By Brands
        private List<Dimension> GetAllMasterGroupByBrand(int brandId)
        {
            var mastergroups = FilterService.GetAllMasterGroupsByBrand(CurrentClient.ClientConnections, brandId);
            List<Dimension> selectList = new List<Dimension>();
            foreach (var mg in mastergroups)
            {
                var dm = new Dimension();
                dm.RefColumn = mg.ColumnReference;
                dm.Text = mg.DisplayName;
                dm.Value = mg.MasterGroupID.ToString();
                dm.DimList = new FrameworkUAD.BusinessLogic.MasterCodeSheet().SelectMasterGroupID(CurrentClient.ClientConnections, mg.MasterGroupID);
                selectList.Add(dm);
            }
            return selectList;
        }
        //Get All ResponseGroup By Pub ID
        private List<Demos> GetAllResponseGroup(int PubID)
        {
            var responsegroups = FilterService.GetAllResponseGroup(PubID, CurrentClient.ClientConnections);
            List<Demos> selectList = new List<Demos>();
            foreach (var mg in responsegroups)
            {
                var dm = new Demos();
                dm.RefColumn = mg.ResponseGroupName;
                dm.Text = mg.DisplayName;
                dm.Value = mg.ResponseGroupID.ToString();
                //dm.DimList = new FrameworkUAD.BusinessLogic.CodeSheet().Select(PubID, CurrentClient.ClientConnections).Where(x => x.ResponseGroupID == mg.ResponseGroupID).ToList();
                selectList.Add(dm);
            }
            return selectList;
        }
        private CirculationFilter GetCircFilter(bool iscirc)
        {
            CirculationFilter cf = new CirculationFilter();
            if (iscirc)
            {
                cf.CategoryCodeTypeSelectList = GetCategoryCodeTypeList(true);
                cf.TransactionCodeTypeSelectList = GetTransactionCodeTypeList(true);
                cf.TransactionCodeSelectList = GetTransactionCodeList(true);
                cf.CategoryCodeSelectList = GetCategoryCodeList(true);
            }
            else
            {
                cf.CategoryCodeTypeSelectList = GetCategoryCodeTypeList(false);
                cf.TransactionCodeTypeSelectList = GetTransactionCodeTypeList(false);
                cf.TransactionCodeSelectList = GetTransactionCodeList(false);
                cf.CategoryCodeSelectList = GetCategoryCodeList(false);

            }
            cf.MediaTypesSelectList = GetMediaTypeList();
            cf.QSourceSelectList = GetQSourceList();
            cf.QSourceTypeSelectList = GetQSourceTypList();
            return cf;
        }
        private IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> GetAllFilterCategories()
        {
            var filtercategories = FilterService.GetAllFilterCategories(CurrentClient.ClientConnections);
            var sortedfiltercategories = (from src in filtercategories
                                          orderby src.CategoryName
                                          select new FrameworkUAD.Entity.FilterCategory { FilterCategoryID = src.FilterCategoryID, CategoryName = src.CategoryName, ParentID = src.ParentID.HasValue ? src.ParentID : 0 }).ToList();
            IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> filtercategoriesTree = sortedfiltercategories.ToKendoFilterCategoryTree();
            return filtercategoriesTree;
        }
        private IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> GetAllQuestionCategories()
        {
            var questionCategories = new FrameworkUAD.BusinessLogic.QuestionCategory().Select(CurrentClient.ClientConnections);
            var sortedquestionCategories = (from src in questionCategories
                                            orderby src.CategoryName
                                            select new FrameworkUAD.Entity.QuestionCategory { QuestionCategoryID = src.QuestionCategoryID, CategoryName = src.CategoryName, ParentID = 0 }).ToList();
            IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> questionCategoriesTree = sortedquestionCategories.ToKendoQuestionCategoryTree();
            return questionCategoriesTree;
        }
        private List<SelectListItem> GetMediaTypeList(bool selected = false)
        {
            var selectList = new List<SelectListItem>() { new SelectListItem() { Text = "PRINT", Value = "A" }, new SelectListItem() { Text = "DIGITAL", Value = "B" }, new SelectListItem() { Text = "BOTH", Value = "C" }, new SelectListItem() { Text = "OPT OUT", Value = "O" } };
            return selectList;
        }
        private List<SelectListItem> GetTransactionCodeTypeList(bool selected = false)
        {

            List<FrameworkUAD_Lookup.Entity.TransactionCodeType> xCodeTypeList = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType().Select();
            FrameworkUAD_Lookup.Entity.TransactionCodeType xCodeTypeActiveFree = xCodeTypeList.Where(x => x.IsActive == true && x.IsFree == true).First();
            FrameworkUAD_Lookup.Entity.TransactionCodeType xCodeTypeActivePaid = xCodeTypeList.Where(x => x.IsActive == true && x.IsFree == false).First();
            List<SelectListItem> selectList = new List<SelectListItem>();
            xCodeTypeList.ForEach(c => selectList.Add(new SelectListItem()
            {
                Text = c.TransactionCodeTypeName.ToUpper(),
                Value = c.TransactionCodeTypeID.ToString(),
                Selected = (xCodeTypeActiveFree.TransactionCodeTypeID == c.TransactionCodeTypeID || xCodeTypeActivePaid.TransactionCodeTypeID == c.TransactionCodeTypeID)
                ? selected : false
            }));
            return selectList;
        }
        private List<SelectListItem> GetCategoryCodeTypeList(bool selected = false)
        {
            List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catCodeType = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType().Select();
            var QualFreeCatType = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType().Select("Qualified Free");
            var QualPaidCatType = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType().Select("Qualified Paid");
            List<SelectListItem> selectList = new List<SelectListItem>();
            catCodeType.ForEach(c => selectList.Add(new SelectListItem()
            {
                Text = c.CategoryCodeTypeName.ToUpper(),
                Value = c.CategoryCodeTypeID.ToString(),
                Selected = (c.CategoryCodeTypeID == QualFreeCatType.CategoryCodeTypeID || c.CategoryCodeTypeID == QualPaidCatType.CategoryCodeTypeID) ? selected : false
            }));
            return selectList;
        }
        private List<SelectListItem> GetTransactionCodeList(bool selected = false)
        {
            List<FrameworkUAD_Lookup.Entity.TransactionCodeType> xCodeTypeList = new FrameworkUAD_Lookup.BusinessLogic.TransactionCodeType().Select();
            FrameworkUAD_Lookup.Entity.TransactionCodeType xCodeTypeActiveFree = xCodeTypeList.Where(x => x.IsActive == true && x.IsFree == true).First();
            FrameworkUAD_Lookup.Entity.TransactionCodeType xCodeTypeActivePaid = xCodeTypeList.Where(x => x.IsActive == true && x.IsFree == false).First();
            List<FrameworkUAD_Lookup.Entity.TransactionCode> xCodeList = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode().Select();
            List<SelectListItem> selectList = new List<SelectListItem>();
            xCodeList.ForEach(c => selectList.Add(new SelectListItem()
            {
                Text = c.TransactionCodeValue + ". " + c.TransactionCodeName.ToUpper(),
                Value = c.TransactionCodeID.ToString(),
                Selected = (xCodeTypeActiveFree.TransactionCodeTypeID == c.TransactionCodeTypeID || xCodeTypeActivePaid.TransactionCodeTypeID == c.TransactionCodeTypeID)
                ? selected : false
            }));
            return selectList;
        }
        private List<SelectListItem> GetCategoryCodeList(bool selected = false)
        {
            List<FrameworkUAD_Lookup.Entity.CategoryCode> catCode = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode().Select();
            var QualFreeCatType = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType().Select("Qualified Free");
            var QualPaidCatType = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType().Select("Qualified Paid");
            List<SelectListItem> selectList = new List<SelectListItem>();
            catCode.ForEach(c => selectList.Add(new SelectListItem()
            {
                Text = c.CategoryCodeValue + ". " + c.CategoryCodeName.ToUpper(),
                Value = c.CategoryCodeID.ToString(),
                Selected = ((c.CategoryCodeTypeID == QualFreeCatType.CategoryCodeTypeID || c.CategoryCodeTypeID == QualPaidCatType.CategoryCodeTypeID) && c.CategoryCodeValue != 70) ? selected : false

            }));
            return selectList;
        }
        private List<SelectListItem> GetQSourceList()
        {
            List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new FrameworkUAD_Lookup.BusinessLogic.CodeType().Select();
            List<FrameworkUAD_Lookup.Entity.Code> codeList = new FrameworkUAD_Lookup.BusinessLogic.Code().Select();
            int qSourceType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source.ToString().Replace("_", " "))).CodeTypeId;
            var qSourceList = codeList.Where(x => x.CodeTypeId == qSourceType).OrderBy(x => x.DisplayOrder).ToList();
            List<SelectListItem> selectList = new List<SelectListItem>();
            qSourceList.ForEach(c => selectList.Add(new SelectListItem() { Text = c.DisplayName.ToUpper(), Value = c.CodeId.ToString() }));
            return selectList;
        }
        private List<SelectListItem> GetQSourceTypList()
        {
            List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new FrameworkUAD_Lookup.BusinessLogic.CodeType().Select();
            List<FrameworkUAD_Lookup.Entity.Code> codeList = new FrameworkUAD_Lookup.BusinessLogic.Code().Select();
            int qSourceType = codeTypeList.SingleOrDefault(s => s.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source_Type.ToString().Replace("_", " "))).CodeTypeId;
            var qSourceList = codeList.Where(x => x.CodeTypeId == qSourceType).OrderBy(x => x.DisplayOrder).ToList();
            List<SelectListItem> selectList = new List<SelectListItem>();
            qSourceList.ForEach(c => selectList.Add(new SelectListItem() { Text = c.DisplayName.ToUpper(), Value = c.CodeId.ToString() }));
            return selectList;
        }

        private static string CleanString(string DirtyOne)
        {
            string CleanOne = StringFunctions.Replace(DirtyOne, "'", "''");
            CleanOne = StringFunctions.Replace(CleanOne, "’", "''");
            CleanOne = StringFunctions.Replace(CleanOne, "–", "-");
            CleanOne = StringFunctions.Replace(CleanOne, "“", "\"");
            CleanOne = StringFunctions.Replace(CleanOne, "”", "\"");
            CleanOne = StringFunctions.Replace(CleanOne, "…", "...");
            return CleanOne;
        }
       
        private DownloadViewModel ExportFields(DownloadViewModel dp)
        {
            Dictionary<string, string> exportfields = new Dictionary<string, string>();
            Dictionary<string, string> exportProfileFields = new Dictionary<string, string>();
            Dictionary<string, string> exportDemoFields = new Dictionary<string, string>();
            Dictionary<string, string> exportAdhocFields = new Dictionary<string, string>();
            //Dictionary<string, string> selectedfields = new Dictionary<string, string>();

            FrameworkUAD.BusinessLogic.Enums.ExportType exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.FTP;
            if (dp.MasterOptionSelected == "Download")
                exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.FTP;
            else if (dp.MasterOptionSelected == "Export")
                exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.ECN;
            else if (dp.MasterOptionSelected == "Campaign")
                exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.Campaign;

            exportProfileFields = Helpers.Utilities.GetExportingFields(CurrentClient.ClientConnections, dp.PubID, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Profile, dp.DownloadFor, dp.IsArchived);
            exportDemoFields = Helpers.Utilities.GetExportingFields(CurrentClient.ClientConnections, dp.PubID, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Demo, dp.DownloadFor, dp.IsArchived);
            exportAdhocFields = Helpers.Utilities.GetExportingFields(CurrentClient.ClientConnections, dp.PubID, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Adhoc, dp.DownloadFor, dp.IsArchived);

            List<FrameworkUAD.Entity.DownloadTemplateDetails> dtd = FrameworkUAD.BusinessLogic.DownloadTemplateDetails.GetByDownloadTemplateID(CurrentClient.ClientConnections, Convert.ToInt32(dp.DownloadTemplateID));

            foreach (var item in exportProfileFields)
            {
                if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                    dp.AvailableProfileFields.Add(new SelectListItem() { Text = item.Value, Value = item.Key });

            }

            foreach (var item in exportDemoFields)
            {
                if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                    dp.AvailableDemoFields.Add(new SelectListItem() { Text = item.Value, Value = item.Key });

            }

            foreach (var item in exportAdhocFields)
            {
                if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                    dp.AvailableAdhocFields.Add(new SelectListItem() { Text = item.Value, Value = item.Key });

            }

            return dp;

        }

        #endregion

        #region Not Used Code

        public ActionResult GetExportField(DownLoadPopupViewModel dp)
        {

            List<int> PubIDs = dp.PubIDs;

            Dictionary<string, string> exportfields = new Dictionary<string, string>();
            Dictionary<string, string> exportProfileFields = new Dictionary<string, string>();
            Dictionary<string, string> exportDemoFields = new Dictionary<string, string>();
            Dictionary<string, string> exportAdhocFields = new Dictionary<string, string>();
            //Dictionary<string, string> selectedfields = new Dictionary<string, string>();

            FrameworkUAD.BusinessLogic.Enums.ExportType exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.FTP;
            if (dp.MasterOptionSelected == "Download")
                exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.FTP;
            else if (dp.MasterOptionSelected == "Export")
                exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.ECN;
            else if (dp.MasterOptionSelected == "Campaign")
                exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.Campaign;
            else if (dp.MasterOptionSelected == "Marketo")
                exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.Marketo;


            if (dp.DCRunID > 0 && dp.filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase) && dp.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
            {
                exportProfileFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, FrameworkUAD.BusinessLogic.Enums.ViewType.ConsensusView, 0, null, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Profile);
                exportDemoFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, FrameworkUAD.BusinessLogic.Enums.ViewType.ConsensusView, 0, null, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Demo);
                exportAdhocFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, FrameworkUAD.BusinessLogic.Enums.ViewType.ConsensusView, 0, null, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Adhoc);
            }
            else
            {
                exportProfileFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, dp.ViewType, dp.BrandID, dp.PubIDs, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Profile);
                exportDemoFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, dp.ViewType, dp.BrandID, dp.PubIDs, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Demo);
                exportAdhocFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, dp.ViewType, dp.BrandID, dp.PubIDs, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Adhoc);
            }

            foreach (var e in exportProfileFields)
                exportfields.Add(e.Key, e.Value);

            foreach (var e in exportDemoFields)
                exportfields.Add(e.Key, e.Value);

            foreach (var e in exportAdhocFields)
                exportfields.Add(e.Key, e.Value);

            List<FrameworkUAD.Entity.DownloadTemplateDetails> dtd = FrameworkUAD.BusinessLogic.DownloadTemplateDetails.GetByDownloadTemplateID(CurrentClient.ClientConnections, Convert.ToInt32(dp.DownloadTemplateID));
            dp.AvailableProfileFields.Clear();
            dp.AvailableDemoFields.Clear();
            dp.AvailableAdhocFields.Clear();
            dp.SelectedItems.Clear();
            foreach (var item in exportProfileFields)
            {
                if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                    dp.AvailableProfileFields.Add(new SelectListItem() { Text = item.Value, Value = item.Key });
            }

            foreach (var item in exportDemoFields)
            {
                if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                    dp.AvailableDemoFields.Add(new SelectListItem() { Text = item.Value, Value = item.Key });
            }

            foreach (var item in exportAdhocFields)
            {
                if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                    dp.AvailableAdhocFields.Add(new SelectListItem() { Text = item.Value, Value = item.Key });

            }
            foreach (var td in dtd)
            {
                var item = exportfields.Where(x => x.Key.Split('|')[0].Equals(td.ExportColumn)).FirstOrDefault();
                if (!string.IsNullOrEmpty(item.Key))
                {
                    dp.SelectedItems.Add(new SelectListItem()
                    {
                        Text = item.Value +
                         (
                         item.Key.Split('|')[1].ToUpper() == FrameworkUAD.BusinessLogic.Enums.FieldType.Varchar.ToString().ToUpper()
                         ? "(" + (td.FieldCase == null ? FrameworkUAD.BusinessLogic.Enums.FieldCase.Default.ToString() : td.FieldCase) +
                               ")"
                               : ""
                         ),
                        Value = item.Key + "|" +
                         (td.FieldCase == null
                         ? (item.Key.Split('|')[1].ToUpper() == FrameworkUAD.BusinessLogic.Enums.FieldType.Varchar.ToString().ToUpper()
                             ? FrameworkUAD.BusinessLogic.Enums.FieldCase.Default.ToString()
                             : FrameworkUAD.BusinessLogic.Enums.FieldCase.None.ToString())
                          : td.FieldCase)
                    });
                }
            }
            return Json(dp);
        }

        private int saveDataCompareView(int targetCodeID, int? targetID, int typeCodeID, FrameworkUAD_Lookup.Enums.DataCompareType typeCodeName, int UadNetCount)
        {
            FrameworkUAS.Entity.DataCompareView dcv = new FrameworkUAS.Entity.DataCompareView();

            //Fake Data Start
            int dcTargetCodeID = 0;
            int dcRunID = 0;
            int matchedRecordsCount = 0;
            int nonMatchedRecordsCount = 0;
            int TotalFileRecords = 0;
            string plNotes = "";
            bool IsBillable = false;
            bool IsKMStaff = false;
            //Fake Data End

            dcv.DcTargetCodeId = dcTargetCodeID;
            dcv.DcTargetIdUad = targetID;
            dcv.DcTypeCodeId = typeCodeID;
            dcv.DcRunId = dcRunID;
            dcv.UadNetCount = UadNetCount;
            dcv.MatchedCount = matchedRecordsCount;
            dcv.NoDataCount = nonMatchedRecordsCount;
            dcv.Cost = new FrameworkUAS.BusinessLogic.DataCompareView().GetDataCompareCost(CurrentUser.UserID, CurrentClient.ClientID, (UadNetCount + TotalFileRecords), typeCodeName, FrameworkUAD_Lookup.Enums.DataCompareCost.MergePurge);

            dcv.Notes = plNotes;

            dcv.IsBillable = IsBillable;

            if (IsKMStaff && !Convert.ToBoolean(IsBillable))
                dcv.PaymentStatusId = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Non_Billed.ToString()).CodeId;
            else
                dcv.PaymentStatusId = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Unpaid.ToString()).CodeId;

            dcv.DateCreated = DateTime.Now;
            dcv.CreatedByUserID = CurrentUser.UserID;

            int dcViewID = new FrameworkUAS.BusinessLogic.DataCompareView().Save(dcv);

            return dcViewID;
        }

        private DownLoadPopupViewModel ValidateExportPermission(DownLoadPopupViewModel dp)
        {
            Guard.NotNull(dp, nameof(dp));
            if (!PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.UADExport, EnumAccess.Download)
                && !PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.UADExport, EnumAccess.ExportToGroup)
                && !PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.UADExport, EnumAccess.SaveToCampaign))
            {
                dp.IsError = true;
                dp.ErrorMessage = ErrorNoPermissionToDownloadData;
                dp.ErrorType = ErrorTypeNotAuthorized;
                dp.PanelUADExportVisible = false;
            }
            else
            {
                dp.IsError = false;
                dp.ViewDownloadVisible = PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.UADExport, EnumAccess.Download);

                if (!PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.DataCompare, EnumAccess.Yes)
                    || dp.DCRunID == 0)
                {
                    dp.ViewSaveToCampaignVisible = PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.UADExport, EnumAccess.SaveToCampaign);

                    if (!PlatformUser.HasAccess(CurrentUser, EnumServices.UAD, EnumFeatures.UADExport, EnumAccess.ExportToGroup))
                    {
                        dp.ViewExportToGroupVisible = false;
                    }
                    else
                    {
                        UpdateViewExportToGroup(dp);
                    }

                    if (!dp.ViewDownloadVisible && !dp.ViewExportToGroupVisible)
                    {
                        dp.ErrorMessage = ErrorNoPermissionToDownloadData;
                        dp.IsError = true;
                        dp.ErrorType = ErrorTypeUnAuthorized;
                    }
                    else
                    {
                        dp.ExportFieldsVisible = true;
                        dp.DownloadCountVisible = true;
                        dp.PromoCodeVisible = true;
                    }
                }
                else
                {
                    UpdateIsBillable(dp);
                }
            }

            return dp;
        }

        private void UpdateViewExportToGroup(DownLoadPopupViewModel model)
        {
            Guard.NotNull(model, nameof(model));
            if (!PlatformUser.HasAccess(CurrentUser, EnumServices.EMAILMARKETING, EnumFeatures.Email, EnumAccess.ExternalImport))
            {
                model.ViewExportToGroupVisible = false;
            }
            else
            {
                var udm = FrameworkUAD.BusinessLogic.UserDataMask.GetByUserID(CurrentClient.ClientConnections, CurrentUser.UserID);

                if (udm.Exists(u => MaskFieldEmail.Equals(u.MaskField, StringComparison.OrdinalIgnoreCase)) && !PlatformUser.IsAdministrator(CurrentUser))
                {
                    model.ViewExportToGroupVisible = false;
                }
                else
                {
                    model.ViewExportToGroupVisible = true;
                    model.NewGroupVisible = PlatformUser.HasAccess(CurrentUser, EnumServices.EMAILMARKETING, EnumFeatures.Groups, EnumAccess.Edit);
                    model.ExistingGroupVisible = PlatformUser.HasAccess(CurrentUser, EnumServices.EMAILMARKETING, EnumFeatures.Groups, EnumAccess.View);
                }
            }
        }

        private void UpdateIsBillable(DownLoadPopupViewModel model)
        {
            Guard.NotNull(model, nameof(model));

            if (model.DCRunID <= 0 || !CurrentUser.IsKMStaff)
            {
                return;
            }

            var viewList = new FrameworkUAS.BusinessLogic.DataCompareView().SelectForRun(model.DCRunID);

            int? targetId;

            if (model.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
            {
                targetId = model.PubIDs.First();
            }
            else
            {
                targetId = model.BrandID > 0 ? (int?) model.BrandID : null;
            }

            var compareView = viewList.FirstOrDefault(u =>
                u.DcTargetCodeId == model.dcTypeCodeID
                && u.DcTargetIdUad == targetId
                && u.DcTypeCodeId == model.dcTypeCodeID);
            if (compareView != null && compareView.IsBillable)
            {
                model.IsBillable = false;
            }
        }

        public ActionResult DetailsDownload(DownLoadPopupViewModel dp)
        {

            List<string> StandardColumnsList = new List<string>();
            List<string> stdColumnList = new List<string>();
            List<string> MasterGroupColumnList = new List<string>();
            List<string> MasterGroupColumnDescList = new List<string>();
            List<string> SubscriptionsExtMapperValueList = new List<string>();
            List<string> PubSubscriptionsExtMapperValueList = new List<string>();
            List<string> StandardColumns = new List<string>();
            DataTable dtSubscription = new DataTable();
            List<int> SubID = new List<int>();
            List<string> ResponseGroupIDList = new List<string>();
            List<string> ResponseGroupDescIDList = new List<string>();
            List<string> selectedItem = new List<string>();
            List<string> customColumnList = new List<string>();
            StringBuilder subQuries = new StringBuilder(dp.SubscribersQueries);
            string outfilepath = string.Empty;
            string fileName = string.Empty;
            string path = string.Empty;

            #region Validation of Business Rules
            if (dp.MasterOptionSelected == "Download" || dp.MasterOptionSelected == "Export")
            {
                if (dp.SelectedItems.Count == 0)
                {
                    return Json(new { error = true, errormessage = "Please select atleast one field for download or export." });

                }
                if (dp.MasterOptionSelected == "Export")
                {
                    FrameworkUAD.BusinessLogic.Enums.ExportType exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.ECN;

                    Dictionary<string, string> exportDemoFields = new Dictionary<string, string>();
                    Dictionary<string, string> exportAdhocFields = new Dictionary<string, string>();

                    if (dp.DCRunID > 0 && dp.filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase) && dp.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                    {
                        exportDemoFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, FrameworkUAD.BusinessLogic.Enums.ViewType.ConsensusView, 0, null, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Demo);
                        exportAdhocFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, FrameworkUAD.BusinessLogic.Enums.ViewType.ConsensusView, 0, null, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Adhoc);
                    }
                    else
                    {
                        exportDemoFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, dp.ViewType, dp.BrandID, dp.PubIDs, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Demo);
                        exportAdhocFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, dp.ViewType, dp.BrandID, dp.PubIDs, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Adhoc);
                    }

                    int demoFieldsCount = 0;
                    int AdhocFieldsCount = 0;

                    for (int i = 0; i < dp.SelectedItems.Count; i++)
                    {
                        if (dp.SelectedItems[i].Selected)
                        {
                            if (exportDemoFields.Any(x => x.Key.Split('|')[0] == dp.SelectedItems[i].Value.ToString().Split('|')[0]))
                            {
                                demoFieldsCount = demoFieldsCount + 1;
                            }
                            else if (exportAdhocFields.Any(x => x.Key.Split('|')[0] == dp.SelectedItems[i].Value.ToString().Split('|')[0]))
                            {
                                AdhocFieldsCount = AdhocFieldsCount + 1;
                            }
                        }
                    }
                    if (demoFieldsCount > 5)
                    {

                        return Json(new { error = true, errormessage = "Demofields should not be more than 5." });
                    }
                    else if (AdhocFieldsCount > 5)
                    {
                        return Json(new { error = true, errormessage = "AdhocFields should not be more than 5." });
                    }
                }
            }
            #endregion

            #region Generates columns for Download and export
            if (dp.MasterOptionSelected == "Download" || dp.MasterOptionSelected == "Export")
            {
                foreach (var item in dp.SelectedItems)
                {
                    selectedItem.Add(item.Value);
                }

                if (dp.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                {
                    if (dp.DCRunID > 0 && dp.filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase))
                    {
                        //Get selected adhoc columns
                        SubscriptionsExtMapperValueList = Helpers.Utilities.GetSelectedSubExtMapperExportColumns(CurrentClient.ClientConnections, selectedItem);

                        //Get selected mastergroup value and mastergroup desc columns
                        Tuple<List<string>, List<string>> mg = Helpers.Utilities.GetSelectedMasterGroupExportColumns(CurrentClient.ClientConnections, selectedItem, dp.BrandID);
                        MasterGroupColumnList = mg.Item1;
                        MasterGroupColumnDescList = mg.Item2;

                        //Get selected standard columns
                        StandardColumnsList = Helpers.Utilities.GetSelectedStandardExportColumns(CurrentClient.ClientConnections, selectedItem, dp.BrandID);
                    }
                    else
                    {
                        //Get selected adhoc columns
                        PubSubscriptionsExtMapperValueList = Helpers.Utilities.GetSelectedPubSubExtMapperExportColumns(CurrentClient.ClientConnections, selectedItem, dp.PubIDs.First());

                        //Get selected responsegroup value and responsegroup desc columns
                        Tuple<List<string>, List<string>, List<string>> rg = Helpers.Utilities.GetSelectedResponseGroupStandardExportColumns(CurrentClient.ClientConnections, selectedItem, dp.PubIDs.First(), dp.MasterOptionSelected == "Export" ? true : false);
                        ResponseGroupIDList = rg.Item1;
                        ResponseGroupDescIDList = rg.Item2;

                        //Get selected standard columns
                        StandardColumnsList = rg.Item3;
                    }
                }
                else
                {
                    //Get selected adhoc columns
                    SubscriptionsExtMapperValueList = Helpers.Utilities.GetSelectedSubExtMapperExportColumns(CurrentClient.ClientConnections, selectedItem);

                    //Get selected mastergroup value and mastergroup desc columns
                    Tuple<List<string>, List<string>> mg = Helpers.Utilities.GetSelectedMasterGroupExportColumns(CurrentClient.ClientConnections, selectedItem, dp.BrandID);
                    MasterGroupColumnList = mg.Item1;
                    MasterGroupColumnDescList = mg.Item2;

                    //Get selected standard columns
                    StandardColumnsList = Helpers.Utilities.GetSelectedStandardExportColumns(CurrentClient.ClientConnections, selectedItem, dp.BrandID);
                }

                if (dp.DCRunID > 0 && dp.filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase) && dp.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                    stdColumnList = Helpers.Utilities.GetStandardExportColumnFieldName(StandardColumnsList, FrameworkUAD.BusinessLogic.Enums.ViewType.ConsensusView, dp.BrandID, dp.MasterOptionSelected == "Export" ? true : false).ToList();
                else
                    stdColumnList = Helpers.Utilities.GetStandardExportColumnFieldName(StandardColumnsList, dp.ViewType, dp.BrandID, dp.MasterOptionSelected == "Export" ? true : false).ToList();

                //Get custom selected columns
                customColumnList = Helpers.Utilities.GetSelectedCustomExportColumns(selectedItem);
            }
            #endregion

            #region Assign Subscription IDs to list
            if (dp.SubscriptionIDs != null && dp.SubscriptionIDs.Count > 0)
            {
                List<int> LNth = Helpers.Utilities.getNth(dp.TotalCount, dp.DownloadCount);
                List<int> SubscriptionIDs = dp.SubscriptionIDs;

                foreach (int n in LNth)
                {
                    SubID.Add(SubscriptionIDs.ElementAt(n));
                }
            }
            #endregion

            #region Get Subcriber data
            if (dp.MasterOptionSelected == "Export" || dp.MasterOptionSelected == "Download")
            {
                #region For ProductView
                if (dp.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                {
                    if (dp.DCRunID > 0 && dp.filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase))
                        dtSubscription = new FrameworkUAD.BusinessLogic.Subscriber().GetSubscriberData(CurrentClient.ClientConnections, subQuries, stdColumnList, MasterGroupColumnList, MasterGroupColumnDescList, SubscriptionsExtMapperValueList, customColumnList, dp.BrandID, dp.PubIDs, dp.IsMostRecentData ? true : false, dp.DownloadCount);
                    else if (dp.IssueID > 0)
                    {
                        var query = FrameworkUAD.BusinessLogic.FilterMVC.getProductArchiveFilterQuery(dp.filtermvc, "distinct 1, ps.SubscriptionID ", "", dp.IssueID, CurrentClient.ClientConnections);
                        dtSubscription = new FrameworkUAD.BusinessLogic.Subscriber().GetArchivedProductDimensionSubscriberData(CurrentClient.ClientConnections, query, stdColumnList, dp.PubIDs, ResponseGroupIDList, ResponseGroupDescIDList, PubSubscriptionsExtMapperValueList, customColumnList, dp.BrandID, dp.IssueID, dp.DownloadCount);
                    }
                    else
                        dtSubscription = new FrameworkUAD.BusinessLogic.Subscriber().GetProductDimensionSubscriberData(CurrentClient.ClientConnections, subQuries, stdColumnList, dp.PubIDs, ResponseGroupIDList, ResponseGroupDescIDList, PubSubscriptionsExtMapperValueList, customColumnList, dp.BrandID, dp.DownloadCount);
                }
                #endregion

                #region Other Views
                else
                {
                    if (dp.SubscriptionIDs != null && dp.SubscriptionIDs.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < SubID.Count; i++)
                        {
                            sb.AppendLine("<Subscriptions SubscriptionID=\"" + SubID[i] + "\"/>");
                        }

                        dtSubscription = new FrameworkUAD.BusinessLogic.Subscriber().GetSubscriberData(CurrentClient.ClientConnections, new StringBuilder(), stdColumnList, MasterGroupColumnList, MasterGroupColumnDescList, SubscriptionsExtMapperValueList, customColumnList, dp.BrandID, dp.PubIDs, dp.IsMostRecentData ? true : false, dp.DownloadCount, sb.ToString());
                    }
                    else
                        dtSubscription = new FrameworkUAD.BusinessLogic.Subscriber().GetSubscriberData(CurrentClient.ClientConnections, subQuries, stdColumnList, MasterGroupColumnList, MasterGroupColumnDescList, SubscriptionsExtMapperValueList, customColumnList, dp.BrandID, dp.PubIDs, dp.IsMostRecentData ? true : false, dp.DownloadCount);
                }
                #endregion
            }
            #endregion

            if (dp.MasterOptionSelected == "Export")
            {
                #region Export to Group Logic
                if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.ExportToGroup))
                {


                    int GroupID = 0;
                    bool mastersupression = false;
                    int folderID = 0;
                    int CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(dp.CustomerClientID, false).CustomerID;

                    if (dp.IsExistingGroupChecked)
                    {
                        GroupID = Convert.ToInt32(dp.GroupID);
                        var grp = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(GroupID, CurrentUser);
                        mastersupression = grp.MasterSupression.HasValue ? ((int) grp.MasterSupression == 1 ? true : false) : false;
                        folderID = Convert.ToInt32(dp.FolderID);
                    }
                    else if (dp.IsNewGroupChecked)
                    {
                        folderID = dp.FolderID;

                        string gname = CleanString(dp.GroupName);

                        if (ECN_Framework_BusinessLayer.Communicator.Group.ExistsByGroupNameByCustomerID(gname, CustomerID))
                        {
                            return Json(new { error = true, errormessage = "<font color='#000000'>\"" + gname + "\"</font> listname already exists. Please enter a different name." });

                        }
                        else
                        {
                            GroupID = Helpers.Utilities.InsertGroup(gname, CustomerID, folderID);
                        }
                    }
                    else
                    {
                        return Json(new { error = true, errormessage = "<font color='red'>Please select New Group or Existing group.</font>" });

                    }

                    try
                    {
                        //if group is mastersupression then only profile field exported
                        if (mastersupression)
                        {
                            string[] cols = { "FIRSTNAME", "LASTNAME", "COMPANY", "TITLE", "ADDRESS", "ADDRESS2", "ADDRESS3", "CITY", "STATE", "ZIP", "COUNTRY", "PHONE", "FAX", "MOBILE" };

                            List<DataColumn> columnsToDelete = new List<DataColumn>();
                            foreach (DataColumn column in dtSubscription.Columns)
                            {
                                if (!cols.Contains(column.ColumnName.ToUpper()))
                                    columnsToDelete.Add(column);
                            }

                            foreach (DataColumn col in columnsToDelete)

                                dtSubscription.Columns.Remove(col);
                        }

                        // Non UDF fields 
                        string[] NotUDFcols = { "SUBSCRIPTIONID", "EMAIL", "FIRSTNAME", "LASTNAME", "COMPANY", "TITLE", "ADDRESS", "ADDRESS2", "ADDRESS3", "CITY", "STATE", "ZIP", "COUNTRY", "PHONE", "FAX", "MOBILE" };

                        List<FrameworkUAD.Object.ExportFields> ExportFields = new List<FrameworkUAD.Object.ExportFields>();

                        foreach (DataColumn column in dtSubscription.Columns)
                        {
                            if (NotUDFcols.Contains(column.ColumnName.ToUpper()))
                                ExportFields.Add(new FrameworkUAD.Object.ExportFields(column.ColumnName.ToUpper(), "", false, 0));
                            else
                                ExportFields.Add(new FrameworkUAD.Object.ExportFields(column.ColumnName.ToUpper(), "", true, 0));
                        }

                        Hashtable hUpdatedRecords = new Hashtable();
                        DateTime startDateTime = DateTime.Now;
                        string promocode = String.IsNullOrEmpty(dp.PromoCode) ? "" : dp.PromoCode;
                        hUpdatedRecords = Helpers.Utilities.ExportToECN(GroupID, dp.GroupName, CustomerID, folderID, promocode, dp.JobCode, ExportFields, dtSubscription, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.GroupExportSource.UADManualExport);

                        if (hUpdatedRecords.Count > 0)
                        {
                            DataTable dt = Helpers.Utilities.getImportedResult(hUpdatedRecords, startDateTime);
                            DataView dv = dt.DefaultView;
                            dv.Sort = "sortorder asc";
                            dt = dv.ToTable();
                            return PartialView("Partials/Common/_exportResult", dt);

                        }
                    }
                    catch (Exception ex)
                    {
                        Helpers.Utilities.Log_Error(Request.RawUrl.ToString(), "DetailsDownload - group export", ex);
                        Json(new { error = true, errormessage = ex.Message });
                    }

                    //mdlDownloads.Show();
                    //return;

                }
                #endregion
            }
            else if (dp.MasterOptionSelected == "Download")
            {
                #region Download to file Logic

                if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.Download))
                {
                    List<dynamic> fefName = new List<dynamic>();

                    dp.SelectedItems.ForEach(x => fefName.Add(new { text = x.Text, value = x.Value }));

                    string[] columnsOrder = new string[fefName.Count + 1];
                    int i = 0;

                    columnsOrder[0] = "SubscriptionID";

                    foreach (dynamic e in fefName)
                    {
                        i++;
                        #region For Product View
                        if (dp.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                        {
                            #region For DataCompare
                            if (dp.DCRunID > 0 && dp.filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase))
                            {
                                switch ((string) e.GetType().GetProperty("text").GetValue(e, null))
                                {
                                    case "FNAME":
                                        columnsOrder[i] = "FirstName";
                                        break;
                                    case "LNAME":
                                        columnsOrder[i] = "LastName";
                                        break;
                                    case "ISLATLONVALID":
                                        columnsOrder[i] = "GeoLocated";
                                        break;
                                    default:
                                        columnsOrder[i] = (string) e.GetType().GetProperty("text").GetValue(e, null);
                                        break;
                                }
                            }
                            #endregion

                            #region Non DataCompare
                            else
                            {
                                switch ((string) e.GetType().GetProperty("text").GetValue(e, null).ToUpper())
                                {
                                    case "ADDRESS1":
                                        columnsOrder[i] = "Address";
                                        break;
                                    case "REGIONCODE":
                                        columnsOrder[i] = "State";
                                        break;
                                    case "ZIPCODE":
                                        columnsOrder[i] = "Zip";
                                        break;
                                    case "PUBTRANSACTIONDATE":
                                        columnsOrder[i] = "TransactionDate";
                                        break;
                                    case "QUALIFICATIONDATE":
                                        columnsOrder[i] = "QDate";
                                        break;
                                    default:
                                        columnsOrder[i] = (string) e.GetType().GetProperty("text").GetValue(e, null);
                                        break;
                                }
                            }
                            #endregion
                        }
                        #endregion

                        #region For All Other Views
                        else
                        {

                            switch ((string) e.GetType().GetProperty("text").GetValue(e, null))
                            {
                                case "FNAME":
                                    columnsOrder[i] = "FirstName";
                                    break;
                                case "LNAME":
                                    columnsOrder[i] = "LastName";
                                    break;
                                case "ISLATLONVALID":
                                    columnsOrder[i] = "GeoLocated";
                                    break;
                                default:
                                    columnsOrder[i] = (string) e.GetType().GetProperty("text").GetValue(e, null);
                                    break;
                            }
                        }
                        #endregion
                    }

                    for (int j = 0; j < columnsOrder.Length; j++)
                    {
                        dtSubscription.Columns[columnsOrder[j].Split('(')[0]].SetOrdinal(j);
                    }

                    dtSubscription = (DataTable) FrameworkUAD.Object.ProfileFieldMask.MaskData(CurrentClient.ClientConnections, dtSubscription, CurrentUser);

                    //Save DataCompare view details
                    #region DataCompare
                    int dcViewID = 0;

                    FrameworkUAD_Lookup.Enums.DataCompareType typeCodeName = FrameworkUAD_Lookup.Enums.DataCompareType.Match;

                    if (dp.dcTypeCodeID > 0)
                        typeCodeName = (FrameworkUAD_Lookup.Enums.DataCompareType) Enum.Parse(typeof(FrameworkUAD_Lookup.Enums.DataCompareType), new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeId(dp.dcTypeCodeID).CodeName);


                    if (dp.DCRunID > 0)
                    {
                        int targetCodeID = dp.dcTargetCodeID;
                        int? targetID = null;
                        FrameworkUAD.Object.FilterMVC filter = new FrameworkUAD.Object.FilterMVC();

                        if (dp.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                            targetID = dp.PubIDs.First();
                        else
                            targetID = dp.BrandID > 0 ? (int?) dp.BrandID : null;

                        //Calculating Total UAD Records count

                        filter.ViewType = dp.ViewType;

                        if (dp.BrandID > 0)
                        {
                            filter.BrandID = dp.BrandID;
                            filter.Fields.Add(new FrameworkUAD.Object.FilterDetails("Brand", dp.BrandID.ToString(), "", "", FrameworkUAD.BusinessLogic.Enums.FiltersType.Brand, "BRAND"));
                        }

                        if (dp.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                        {
                            filter.PubID = dp.PubIDs.First();
                            filter.Fields.Add(new FrameworkUAD.Object.FilterDetails("Product", dp.PubIDs.First().ToString(), "", "", FrameworkUAD.BusinessLogic.Enums.FiltersType.Product, "PRODUCT"));
                        }

                        filter = FrameworkUAD.BusinessLogic.FilterMVC.Execute(CurrentClient.ClientConnections, filter, "");

                        List<FrameworkUAS.Entity.DataCompareView> datacv = new FrameworkUAS.BusinessLogic.DataCompareView().SelectForRun(dp.DCRunID);


                        var codes = new FrameworkUAD_Lookup.BusinessLogic.Code().Select();
                        var tcID = codes.Find(x => x.CodeName == FrameworkUAD.BusinessLogic.Enums.DataCompareViewType.Consensus.ToString()).CodeId;


                        int paymentStatusID = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Pending.ToString()).CodeId;

                        if (datacv.Exists(y => y.DcTargetCodeId == tcID && y.PaymentStatusId == paymentStatusID))
                        {
                            int id = datacv.Find(y => y.DcTargetCodeId == tcID && y.PaymentStatusId == paymentStatusID).DcViewId;
                            new FrameworkUAS.BusinessLogic.DataCompareView().Delete(id);
                            dcViewID = saveDataCompareView(targetCodeID, targetID, dp.dcTypeCodeID, typeCodeName, filter.Count);
                        }
                        else
                        {
                            if (!datacv.Exists(u => u.DcTargetCodeId == targetCodeID && u.DcTargetIdUad == targetID && u.DcTypeCodeId == dp.dcTypeCodeID))
                            {
                                dcViewID = saveDataCompareView(targetCodeID, targetID, dp.dcTypeCodeID, typeCodeName, filter.Count);
                            }
                            else
                            {
                                if (CurrentUser.IsKMStaff)
                                {
                                    FrameworkUAS.Entity.DataCompareView dcv = datacv.Find(u => u.DcTargetCodeId == targetCodeID && u.DcTargetIdUad == targetID && u.DcTypeCodeId == dp.dcTypeCodeID);

                                    if (!dcv.IsBillable && (CurrentUser.IsKMStaff && Convert.ToBoolean(dp.IsBillable)) || !Convert.ToBoolean(dp.IsBillable))
                                    {
                                        dcViewID = dcv.DcViewId;
                                        dcv.UadNetCount = filter.Count;
                                        dcv.MatchedCount = dp.matchedRecordsCount;
                                        dcv.NoDataCount = dp.nonMatchedRecordsCount;
                                        dcv.Cost = new FrameworkUAS.BusinessLogic.DataCompareView().GetDataCompareCost(CurrentUser.UserID, CurrentClient.ClientID, (filter.Count + dp.TotalFileRecords), typeCodeName, FrameworkUAD_Lookup.Enums.DataCompareCost.MergePurge);

                                        dcv.Notes = dp.Notes;

                                        dcv.IsBillable = dp.IsBillable;

                                        if (CurrentUser.IsKMStaff && !Convert.ToBoolean(dp.IsBillable))
                                            dcv.PaymentStatusId = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Non_Billed.ToString()).CodeId;
                                        else
                                            dcv.PaymentStatusId = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Unpaid.ToString()).CodeId;

                                        dcv.DateUpdated = DateTime.Now;
                                        dcv.UpdatedByUserID = CurrentUser.UserID;

                                        new FrameworkUAS.BusinessLogic.DataCompareView().Save(dcv);
                                    }
                                    else
                                    {
                                        dcViewID = dcv.DcViewId;
                                    }
                                }
                            }
                        }
                    }


                    //Save DataCompare Details

                    List<FrameworkUAD.Entity.MasterGroup> masterGroupList = new List<FrameworkUAD.Entity.MasterGroup>();


                    if (dcViewID > 0)
                    {
                        string demoColumns = string.Empty;

                        if (dp.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                        {
                            List<FrameworkUAD.Entity.ResponseGroup> responseGroupList = new FrameworkUAD.BusinessLogic.ResponseGroup().Select(dp.PubIDs.First(), CurrentClient.ClientConnections);
                            foreach (string s in ResponseGroupIDList)
                            {
                                demoColumns += demoColumns == string.Empty ? responseGroupList.Find(x => x.ResponseGroupID == Convert.ToInt32(s.Split('|')[0])).ResponseGroupName : "," + responseGroupList.Find(x => x.ResponseGroupID == Convert.ToInt32(s.Split('|')[0])).ResponseGroupName;
                            }

                            foreach (string s in PubSubscriptionsExtMapperValueList)
                            {
                                demoColumns += demoColumns == string.Empty ? s.Split('|')[0] : "," + s.Split('|')[0];
                            }
                        }
                        else
                        {
                            if (dp.BrandID > 0)
                                masterGroupList = new FrameworkUAD.BusinessLogic.MasterGroup().SelectByBrandID(dp.BrandID, CurrentClient.ClientConnections);
                            else
                                masterGroupList = new FrameworkUAD.BusinessLogic.MasterGroup().Select(CurrentClient.ClientConnections);

                            foreach (string s in MasterGroupColumnList)
                            {
                                demoColumns += demoColumns == string.Empty ? masterGroupList.Find(x => x.ColumnReference == s.Split('|')[0]).Name : "," + masterGroupList.Find(x => x.ColumnReference == s.Split('|')[0]).Name;
                            }

                            foreach (string s in SubscriptionsExtMapperValueList)
                            {
                                demoColumns += demoColumns == string.Empty ? s.Split('|')[0] : "," + s.Split('|')[0];
                            }
                        }


                        Guid g = System.Guid.NewGuid();
                        fileName = "filter_report_" + g.ToString() + ".tsv";
                        outfilepath = Server.MapPath("../downloads/datacompare/") + CurrentClient.ClientID + "/" + fileName;
                        path = Server.MapPath("../downloads/datacompare/") + CurrentClient.ClientID + "/";

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        List<string> sColumnsList = new List<string>();
                        sColumnsList.AddRange(StandardColumnsList.Select(x => x.Split('|')[0]));

                        List<FrameworkUAS.Entity.DataCompareDownloadCostDetail> cd = new FrameworkUAS.BusinessLogic.DataCompareDownloadCostDetail().CreateCostDetail(dcViewID, dp.dcTypeCodeID, dp.TotalCount.ToString(), string.Join(",", sColumnsList), demoColumns, CurrentUser.UserID);

                        FrameworkUAS.Entity.DataCompareDownload dcd = new FrameworkUAS.Entity.DataCompareDownload();

                        dcd.DcViewId = dcViewID;
                        dcd.WhereClause = dp.filterCombination;
                        dcd.DcTypeCodeId = dp.dcTypeCodeID;
                        dcd.ProfileCount = dtSubscription.Rows.Count;
                        dcd.TotalBilledCost = new FrameworkUAS.BusinessLogic.DataCompareView().GetDataCompareCost(CurrentUser.UserID, CurrentClient.ClientID, dtSubscription.Rows.Count, typeCodeName, FrameworkUAD_Lookup.Enums.DataCompareCost.Download);
                        dcd.IsPurchased = CurrentUser.IsKMStaff ? (Convert.ToBoolean(dp.IsBillable)) : true;
                        dcd.PurchasedByUserId = CurrentUser.UserID;
                        dcd.PurchasedDate = DateTime.Now;
                        dcd.DownloadFileName = fileName;

                        int dcDownloadID = new FrameworkUAS.BusinessLogic.DataCompareDownload().Save(dcd);

                        XDocument xDoc = new XDocument(
                             new XElement("SubcriptionIDs", from sub in dtSubscription.AsEnumerable()
                                                            select
                                                                  new XElement("SubcriptionID", sub["SubscriptionID"])
                            ));

                        new FrameworkUAS.BusinessLogic.DataCompareDownloadDetail().Save(dcDownloadID, xDoc.ToString());

                        //save filters
                        if (!(dp.filterCombination.Equals("Matched", StringComparison.OrdinalIgnoreCase) || dp.filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase)))
                        {
                            FrameworkUAD.Object.FilterCollection filters = (FrameworkUAD.Object.FilterCollection) Session["filterCollection"];

                            foreach (FrameworkUAD.Object.FilterMVC f in filters)
                            {
                                FrameworkUAS.Entity.DataCompareDownloadFilterGroup fg = new FrameworkUAS.Entity.DataCompareDownloadFilterGroup();
                                fg.DcDownloadId = dcDownloadID;
                                int dcFilterGroupID = new FrameworkUAS.BusinessLogic.DataCompareDownloadFilterGroup().Save(fg);

                                foreach (FrameworkUAD.Object.FilterDetails field in f.Fields)
                                {
                                    if (!field.Name.Equals("DataCompare", StringComparison.OrdinalIgnoreCase))
                                    {
                                        FrameworkUAS.Entity.DataCompareDownloadFilterDetail fd = new FrameworkUAS.Entity.DataCompareDownloadFilterDetail();

                                        fd.DcFilterGroupId = dcFilterGroupID;
                                        fd.FilterType = (int) field.FilterType;
                                        fd.Group = field.Group;
                                        fd.Name = field.Name;
                                        fd.Values = field.Values;
                                        fd.SearchCondition = field.SearchCondition;

                                        new FrameworkUAS.BusinessLogic.DataCompareDownloadFilterDetail().Save(fd);
                                    }
                                }
                            }
                        }

                        //Save download fields
                        List<FrameworkUAD_Lookup.Entity.Code> c = new FrameworkUAD_Lookup.BusinessLogic.Code().Select(FrameworkUAD_Lookup.Enums.CodeType.UAD_Field_Type);

                        foreach (string s in StandardColumnsList)
                        {
                            FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                            f.DcDownloadId = dcDownloadID;
                            var uadprofcode = c.Find(x => x.CodeName == FrameworkUAD_Lookup.Enums.UADFieldType.Profile.ToString());
                            f.DcDownloadFieldCodeId = uadprofcode.CodeId;
                            f.ColumnName = s.Split('|')[0];
                            f.IsDescription = false;
                            new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                        }

                        foreach (string s in customColumnList)
                        {
                            FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                            f.DcDownloadId = dcDownloadID;
                            var uadcustomcode = c.Find(x => x.CodeName == FrameworkUAD_Lookup.Enums.UADFieldType.Custom.ToString());
                            f.DcDownloadFieldCodeId = uadcustomcode.CodeId;
                            f.ColumnName = s.Split('|')[0];
                            f.IsDescription = false;
                            new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                        }

                        if (dp.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
                        {
                            foreach (string s in ResponseGroupIDList)
                            {
                                FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                                f.DcDownloadId = dcDownloadID;
                                var uadcode = c.Find(x => x.CodeName == FrameworkUAD_Lookup.Enums.UADFieldType.Dimension.ToString());
                                f.DcDownloadFieldCodeId = uadcode.CodeId;
                                f.ColumnID = Convert.ToInt32(s.Split('|')[0]);
                                f.IsDescription = false;
                                new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                            }

                            foreach (string s in ResponseGroupDescIDList)
                            {
                                FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                                f.DcDownloadId = dcDownloadID;
                                var uadcode = c.Find(x => x.CodeName == FrameworkUAD_Lookup.Enums.UADFieldType.Dimension.ToString());
                                f.DcDownloadFieldCodeId = uadcode.CodeId;
                                f.ColumnID = Convert.ToInt32(s.Split('|')[0]);
                                f.IsDescription = true;
                                new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                            }

                            foreach (string s in PubSubscriptionsExtMapperValueList)
                            {
                                List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> PubSubExtensionMapperList = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper().SelectAll(CurrentClient.ClientConnections).Where(x => x.PubID == dp.PubIDs.First()).ToList();

                                FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                                f.DcDownloadId = dcDownloadID;
                                var uadcode = c.Find(x => x.CodeName == FrameworkUAD_Lookup.Enums.UADFieldType.Adhoc.ToString());
                                f.DcDownloadFieldCodeId = uadcode.CodeId;
                                var pe = PubSubExtensionMapperList.Find(x => x.CustomField.ToString() == s.Split('|')[0]);
                                f.ColumnID = pe.PubSubscriptionsExtensionMapperID;
                                f.IsDescription = false;
                                new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                            }
                        }
                        else
                        {
                            foreach (string s in MasterGroupColumnList)
                            {
                                FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                                f.DcDownloadId = dcDownloadID;
                                var uadcode = c.Find(x => x.CodeName == FrameworkUAD_Lookup.Enums.UADFieldType.Dimension.ToString());
                                f.DcDownloadFieldCodeId = uadcode.CodeId;
                                f.ColumnID = masterGroupList.Find(x => x.ColumnReference == s.Split('|')[0]).MasterGroupID;
                                f.IsDescription = false;
                                new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                            }

                            foreach (string s in MasterGroupColumnDescList)
                            {
                                FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                                f.DcDownloadId = dcDownloadID;
                                var uadcode = c.Find(x => x.CodeName == FrameworkUAD_Lookup.Enums.UADFieldType.Dimension.ToString());
                                f.DcDownloadFieldCodeId = uadcode.CodeId;
                                f.ColumnID = masterGroupList.Find(x => x.ColumnReference == s.Split('|')[0]).MasterGroupID;
                                f.IsDescription = true;
                                new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                            }

                            foreach (string s in SubscriptionsExtMapperValueList)
                            {
                                List<FrameworkUAD.Entity.SubscriptionsExtensionMapper> SubExtensionMapperList = new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper().SelectAll(CurrentClient.ClientConnections).Where(x => x.Active == true).ToList();

                                FrameworkUAS.Entity.DataCompareDownloadField f = new FrameworkUAS.Entity.DataCompareDownloadField();
                                f.DcDownloadId = dcDownloadID;
                                var uadcode = c.Find(x => x.CodeName == FrameworkUAD_Lookup.Enums.UADFieldType.Adhoc.ToString());
                                f.DcDownloadFieldCodeId = uadcode.CodeId;
                                f.ColumnID = SubExtensionMapperList.Find(x => x.CustomField.ToString() == s.Split('|')[0]).SubscriptionsExtensionMapperID;
                                f.IsDescription = false;
                                new FrameworkUAS.BusinessLogic.DataCompareDownloadField().Save(f);
                            }
                        }
                    }
                    #endregion

                    #region Create File On Server
                    else
                    {
                        path = Server.MapPath("../main/temp/");
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        fileName = System.Guid.NewGuid().ToString().Substring(0, 5) + ".tsv";
                        outfilepath = path + fileName;

                    }
                    #endregion

                    #region DownLoad File to Output Path On Server
                    DataColumn newColumn = new DataColumn("PromoCode", typeof(System.String));
                    newColumn.DefaultValue = dp.PromoCode;
                    dtSubscription.Columns.Add(newColumn);

                    if (!dp.IsQueryDetailsIncluded)
                    {
                        dp.HeaderText = string.Empty;
                    }

                    Helpers.Utilities.Download(dtSubscription, outfilepath, dp.HeaderText, dp.TotalCount, dp.DownloadCount);
                    #endregion
                }
                return Json(outfilepath);
                #endregion
            }
            else if (dp.MasterOptionSelected == "SaveToCampaign")
            {
                #region Save to campaign Logic
                if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.SaveToCampaign))
                {


                    List<string> columnList = new List<string>();
                    columnList.Add("s.SubscriptionID|None");
                    columnList.Add("s.CGRP_NO|None");

                    if (dp.SubscriptionIDs != null && dp.SubscriptionIDs.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < SubID.Count; i++)
                        {
                            sb.AppendLine("<Subscriptions SubscriptionID=\"" + SubID[i] + "\"/>");
                        }

                        dtSubscription = new FrameworkUAD.BusinessLogic.Subscriber().GetSubscriberData(CurrentClient.ClientConnections, new StringBuilder(), columnList, MasterGroupColumnList, MasterGroupColumnDescList, SubscriptionsExtMapperValueList, customColumnList, dp.BrandID, dp.PubIDs, false, Convert.ToInt32(dp.DownloadCount), sb.ToString());
                    }
                    else
                    {
                        dtSubscription = new FrameworkUAD.BusinessLogic.Subscriber().GetSubscriberData(CurrentClient.ClientConnections, subQuries, columnList, MasterGroupColumnList, MasterGroupColumnDescList, SubscriptionsExtMapperValueList, customColumnList, dp.BrandID, dp.PubIDs, false, Convert.ToInt32(dp.DownloadCount));
                    }
                    int CampID = 0;
                    if (!dp.IsNewCampaign)
                    {
                        CampID = Convert.ToInt32(dp.CampaignID);
                    }
                    else if (dp.IsNewCampaign)
                    {
                        if (CampID == 0)
                        {
                            if (new FrameworkUAD.BusinessLogic.Campaign().CampaignExists(CurrentClient.ClientConnections, dp.CampaignName) == 0)
                            {
                                CampID = new FrameworkUAD.BusinessLogic.Campaign().Save(new FrameworkUAD.Entity.Campaign() { CampaignName = dp.CampaignName, AddedBy = CurrentUser.UserID, BrandID = dp.BrandID, DateAdded = DateTime.Now, DateUpdated = DateTime.Now }, CurrentClient.ClientConnections);
                            }
                            else
                            {
                                return Json(new { error = true, errormessage = "ERROR - <font color='#000000'>\"" + dp.CampaignName + "\"</font> already exists. Please enter a different name." });

                            }
                        }
                    }
                    else
                    {
                        return Json(new { error = true, errormessage = "Please select new campaign or existing campaign." });

                    }
                    int CampFilterID = 0;
                    if (CampFilterID == 0)
                    {
                        if (new FrameworkUAD.BusinessLogic.CampaignFilter().CampaignFilterExists(CurrentClient.ClientConnections, dp.FilterName, CampID) == 0)
                        {
                            CampFilterID = new FrameworkUAD.BusinessLogic.CampaignFilter().Insert(CurrentClient.ClientConnections, dp.FilterName, CurrentUser.UserID, CampID, dp.PromoCode);
                        }
                        else
                        {
                            return Json(new { error = true, errormessage = "ERROR - <font color='#000000'>\"" + dp.FilterName + "\"</font> already exists. Please enter a different name." });

                        }
                    }

                    StringBuilder xmlSubscriber = new StringBuilder("");
                    int cnt = 0;

                    try
                    {
                        foreach (DataRow dr in dtSubscription.Rows)
                        {
                            xmlSubscriber.Append("<sID>" + Helpers.Utilities.cleanXMLString(dr["SubscriptionID"].ToString()) + "</sID>");

                            if ((cnt != 0) && (cnt % 10000 == 0) || (cnt == dtSubscription.Rows.Count - 1))
                            {
                                new FrameworkUAD.BusinessLogic.CampaignFilterDetail().saveCampaignDetails(CurrentClient.ClientConnections, CampFilterID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlSubscriber.ToString() + "</XML>");
                                xmlSubscriber = new StringBuilder("");
                            }

                            cnt++;
                        }

                        //lblResults.Visible = true;
                        return Json(new { success = true, successmessge = "Total subscriber in the campaign : " + new FrameworkUAD.BusinessLogic.Campaign().GetCountByCampaignID(CurrentClient.ClientConnections, CampID) });
                    }
                    catch (Exception ex)
                    {
                        Helpers.Utilities.Log_Error(Request.RawUrl.ToString(), "DetailsDownload - campaign save", ex);
                        return Json(new { error = true, errormessage = "ERROR - " + ex.Message });
                    }

                    //CampID = 0;
                    //CampFilterID = 0;

                    //mdlDownloads.Show();

                }
                #endregion
            }


            return Json(outfilepath);


        }

        private DownLoadPopupViewModel ExportFields(DownLoadPopupViewModel dp)
        {
            List<int> PubIDs = dp.PubIDs;

            Dictionary<string, string> exportfields = new Dictionary<string, string>();
            Dictionary<string, string> exportProfileFields = new Dictionary<string, string>();
            Dictionary<string, string> exportDemoFields = new Dictionary<string, string>();
            Dictionary<string, string> exportAdhocFields = new Dictionary<string, string>();
            //Dictionary<string, string> selectedfields = new Dictionary<string, string>();

            FrameworkUAD.BusinessLogic.Enums.ExportType exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.FTP;
            if (dp.MasterOptionSelected == "Download")
                exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.FTP;
            else if (dp.MasterOptionSelected == "Export")
                exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.ECN;
            else if (dp.MasterOptionSelected == "Campaign")
                exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.Campaign;
            else if (dp.MasterOptionSelected == "Marketo")
                exportType = FrameworkUAD.BusinessLogic.Enums.ExportType.Marketo;


            if (dp.DCRunID > 0 && dp.filterCombination.Equals("Matched NotIn Selected", StringComparison.OrdinalIgnoreCase) && dp.ViewType == FrameworkUAD.BusinessLogic.Enums.ViewType.ProductView)
            {
                exportProfileFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, FrameworkUAD.BusinessLogic.Enums.ViewType.ConsensusView, 0, null, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Profile);
                exportDemoFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, FrameworkUAD.BusinessLogic.Enums.ViewType.ConsensusView, 0, null, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Demo);
                exportAdhocFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, FrameworkUAD.BusinessLogic.Enums.ViewType.ConsensusView, 0, null, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Adhoc);
            }
            else
            {
                exportProfileFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, dp.ViewType, dp.BrandID, dp.PubIDs, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Profile);
                exportDemoFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, dp.ViewType, dp.BrandID, dp.PubIDs, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Demo);
                exportAdhocFields = Helpers.Utilities.GetExportFields(CurrentClient.ClientConnections, dp.ViewType, dp.BrandID, dp.PubIDs, exportType, CurrentUser.UserID, FrameworkUAD.BusinessLogic.Enums.ExportFieldType.Adhoc);
            }



            List<FrameworkUAD.Entity.DownloadTemplateDetails> dtd = FrameworkUAD.BusinessLogic.DownloadTemplateDetails.GetByDownloadTemplateID(CurrentClient.ClientConnections, Convert.ToInt32(dp.DownloadTemplateID));

            foreach (var item in exportProfileFields)
            {
                if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                    dp.AvailableProfileFields.Add(new SelectListItem() { Text = item.Value, Value = item.Key });

            }

            foreach (var item in exportDemoFields)
            {
                if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                    dp.AvailableDemoFields.Add(new SelectListItem() { Text = item.Value, Value = item.Key });

            }

            foreach (var item in exportAdhocFields)
            {
                if (!dtd.Exists(x => x.ExportColumn.ToUpper() == item.Key.Split('|')[0].ToUpper()))
                    dp.AvailableAdhocFields.Add(new SelectListItem() { Text = item.Value, Value = item.Key });

            }

            return dp;

        }



        private StringBuilder GetSubscribersQueriesForUserControl(DownLoadPopupViewModel dp)
        {
            var fmv = (FilterViewModel) Session["FilterCollectionModel"];
            string DataCompareProcessCode = string.Empty;
            string DataCompareLinkSelected = string.Empty;
            StringBuilder Queries = new StringBuilder();
            string addFilters = "";
            try
            {
                if (!string.IsNullOrEmpty(DataCompareLinkSelected))
                {
                    switch (DataCompareLinkSelected)
                    {
                        case "MATCHEDRECORDS":
                            Queries.Append("<xml><Queries>");
                            Queries.Append(string.Format("<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>", 1,
                                            " select distinct 1,  s.SubscriptionID " +
                                            " from DataCompareProfile d with(nolock) " +
                                            " join Subscriptions s with(Nolock) on d.IGRP_NO = s.IGrp_No " +
                                            " join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID " +
                                            " where d.ProcessCode = '" + DataCompareProcessCode + "' " +
                                            " and ps.PubID = " + Convert.ToInt32(dp.PubIDs.First())
                                ));
                            Queries.Append("</Queries><Results>");
                            Queries.Append(string.Format("<Result linenumber=\"{0}\"  selectedfilterno=\"{1}\" selectedfilteroperation=\"{2}\" suppressedfilterno=\"{3}\" suppressedfilteroperation=\"{4}\"  filterdescription=\"{5}\"></Result>", 1, "1", "", "", "", ""));
                            Queries.Append("</Results></xml>");
                            break;
                        case "MATCHEDNOTINPRODUCT":
                            Queries.Append("<xml><Queries>");
                            Queries.Append(string.Format("<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>", 1,
                                            " select distinct 1, s.SubscriptionID " +
                                            " from DataCompareProfile d with(nolock) " +
                                            " join Subscriptions s with(Nolock) on d.IGRP_NO = s.IGrp_No " +
                                            " left outer join " +
                                            " ( " +
                                            "      select distinct s1.SubscriptionID " +
                                            "        from DataCompareProfile d1 with(nolock) " +
                                            "        join Subscriptions s1 with(Nolock) on d1.IGRP_NO = s1.IGrp_No " +
                                            "        join PubSubscriptions ps with(nolock) on s1.SubscriptionID = ps.SubscriptionID " +
                                            "        where d1.ProcessCode = '" + DataCompareProcessCode + "' " +
                                            "        and ps.PubID = " + Convert.ToInt32(dp.PubIDs.First()) +
                                            "    ) x on s.SubscriptionID = x.SubscriptionID " +
                                            "    where d.ProcessCode = '" + DataCompareProcessCode + "' and " +
                                            "    x.SubscriptionID is null "
                                            ));
                            Queries.Append("</Queries><Results>");
                            Queries.Append(string.Format("<Result linenumber=\"{0}\"  selectedfilterno=\"{1}\" selectedfilteroperation=\"{2}\" suppressedfilterno=\"{3}\" suppressedfilteroperation=\"{4}\"  filterdescription=\"{5}\"></Result>", 1, "1", "", "", "", ""));
                            Queries.Append("</Results></xml>");
                            break;
                    }
                }
                else
                {
                    if (dp.IssueID > 0)
                    {
                        Queries = FrameworkUAD.BusinessLogic.FilterMVC.generateCombinationQueryForIssueArchived(fmv.filters, dp.SelectedFilterOperation, string.IsNullOrEmpty(dp.SuppressedFilterOperation) ? "" : dp.SuppressedFilterOperation, string.IsNullOrEmpty(dp.SelectedFilterNos) ? "" : dp.SelectedFilterNos, string.IsNullOrEmpty(dp.SuppressedFilterNos) ? "" : dp.SuppressedFilterNos, string.IsNullOrEmpty(addFilters) ? "" : addFilters, dp.PubIDs.First(), dp.BrandID, dp.IssueID, CurrentClient.ClientConnections);

                    }
                    else
                    {
                        Queries = FrameworkUAD.BusinessLogic.FilterMVC.generateCombinationQuery(fmv.filters, dp.SelectedFilterOperation, string.IsNullOrEmpty(dp.SuppressedFilterOperation) ? "" : dp.SuppressedFilterOperation, string.IsNullOrEmpty(dp.SelectedFilterNos) ? "" : dp.SelectedFilterNos, string.IsNullOrEmpty(dp.SuppressedFilterNos) ? "" : dp.SuppressedFilterNos, string.IsNullOrEmpty(addFilters) ? "" : addFilters, dp.PubIDs.First(), dp.BrandID, CurrentClient.ClientConnections);

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



            return Queries;
        }

        #endregion

        #region AddRemove File Creation
        private void AddRemoveCreateTSVFile(string splitName, string str, System.Data.DataTable masterClone)
        {
            string path = Server.MapPath("../addkilldownloads/main/");
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            string filePath = path + "\\" + splitName + ".tsv";
            if (System.IO.File.Exists(filePath))
            {
                int i = 1;
                while (System.IO.File.Exists(filePath))
                {
                    filePath = path + "\\" + splitName + "_" + i + ".tsv";
                    i++;
                }
            }
            new Core_AMS.Utilities.FileFunctions().CreateTSVFromDataTable(masterClone, filePath, true);
        }
        #endregion
    }


}
