using FrameworkUAD.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
namespace UAS.Web.Models.UAD.Filter
{
    public class FilterViewModel
    {
        public string ViewType { get; set; }
        public bool showSavedLink { get; set; }
        public bool showMarketPanel { get; set; }
        public bool showDimensionPanel { get; set; }
        public bool showDemoPanel { get; set; }
        public bool showBrandPanel { get; set; }
        public bool showProductPanel { get; set; }
        public bool showDCPanel { get; set; }
        public bool showPubTypePanel { get; set; }
        public bool HasEditAccessToSaved { get; set; }
        public bool IsCirclinkEnabled { get; set; }
        public bool IsActivitylinkEnabled { get; set; }
        public bool IsAdhoclinkEnabled { get; set; }
        public bool IsSavedlinkEnabled { get; set; }
        public bool IsCirc { get; set; }
        public List<SelectListItem> BrandFilterList { get; set; }
        public List<SelectListItem> DcFilterList { get; set; }
        public DynamicFilter DynamicData { get; set; }
        public CirculationFilter CircFilter { get; set; }
        public List<SelectListItem> ProductFilterList { get; set; }
        public GeoFilter geoFilter { get; set; }
        public StandardFilter standardFilter { get; set; }
        public IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> filterCategoryTree { get; set; }
        //public Filters filters = null;
        public FrameworkUAD.Object.FilterCollection filters = null;
       
        public FilterViewModel()
        {
            showDimensionPanel = true;
            showDemoPanel = false;
            showMarketPanel = true;
            showSavedLink = true;
            showBrandPanel = true;
            showDCPanel = true;
            showPubTypePanel = true;
            IsSavedlinkEnabled = true;
            HasEditAccessToSaved = true;
            this.geoFilter = new GeoFilter();
            this.standardFilter = new StandardFilter();
            DynamicData = new DynamicFilter();
            CircFilter = new CirculationFilter();
            BrandFilterList = new List<SelectListItem>();
            DcFilterList = new List<SelectListItem>();
            ProductFilterList = new List<SelectListItem>();
            filterCategoryTree = new List<Kendo.Mvc.UI.TreeViewItemModel>();
           

        }
    }
    public class PubTypeFilter
    {
        public int PubTypeID { get; set; }
        public string PubTypeDisplayName { get; set; }
        public string ColumnReference { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public List<FrameworkUAD.Entity.Product> PubList { get; set; }
        public PubTypeFilter()
        {
            PubList = new List<FrameworkUAD.Entity.Product>();
        }
    }
    public class DCFileFilter
    {
        public string FileName { get; set; }
        public string ProcessCode { get; set; }

    }
    public class DynamicFilter
    {
        public List<PubTypeFilter> PubTypeFilterList { get; set; }
        public List<Dimension> DimensionFilterList { get; set; }
        public List<SelectListItem> MarketFilterList { get; set; }
        public List<Demos> DemosFilterList { get; set; }
        public DynamicFilter()
        {
            DemosFilterList = new List<Demos>();
            PubTypeFilterList = new List<PubTypeFilter>();
            DimensionFilterList = new List<Dimension>();
            MarketFilterList = new List<SelectListItem>();
        }
    }
    public class GeoFilter
    {

        public List<SelectListItem> RegionGroupsList { get; set; }
        public List<SelectListItem> RegionList { get; set; }
        public List<SelectListItem> CountryList { get; set; }
        public List<SelectListItem> AreaList { get; set; }

        public GeoFilter()
        {
            RegionGroupsList = new List<SelectListItem>();
            RegionList = new List<SelectListItem>();
            CountryList = new List<SelectListItem>();
            AreaList = new List<SelectListItem>();
        }
    }
    public class StandardFilter
    {
        public List<SelectListItem> EmailStatusList { get; set; }
        public List<SelectListItem> MediaStdSelectList { get; set; }
        public List<SelectListItem> YesNoSelectList { get; set; }
        public List<SelectListItem> YesNoBlankSelectList { get; set; }
        public StandardFilter()
        {
            EmailStatusList = new List<SelectListItem>();
            MediaStdSelectList = new List<SelectListItem>() { new SelectListItem() { Text = "PRINT", Value = "A" }, new SelectListItem() { Text = "DIGITAL", Value = "B" }, new SelectListItem() { Text = "BOTH", Value = "C" } , new SelectListItem() { Text = "OPT OUT", Value = "O" } };
            YesNoSelectList = new List<SelectListItem>() { new SelectListItem() { Text = "Yes", Value = "1" }, new SelectListItem() { Text = "No", Value = "0" } };
            YesNoBlankSelectList = new List<SelectListItem>() { new SelectListItem() { Text = "Yes", Value = "1" }, new SelectListItem() { Text = "No", Value = "0" }, new SelectListItem() { Text = "Blank", Value = "-1" } };

        }
    }
    public class CirculationFilter
    {
        public List<SelectListItem> QSourceSelectList { get; set; }
        public List<SelectListItem> TransactionCodeTypeSelectList { get; set; }
        public List<SelectListItem> TransactionCodeSelectList { get; set; }
        public List<SelectListItem> CategoryCodeSelectList { get; set; }
        public List<SelectListItem> CategoryCodeTypeSelectList { get; set; }
        public List<SelectListItem> QSourceTypeSelectList { get; set; }
        public List<SelectListItem> MediaTypesSelectList { get; set; }
        public CirculationFilter()
        {
            TransactionCodeSelectList = new List<SelectListItem>();
            MediaTypesSelectList = new List<SelectListItem>();
            QSourceTypeSelectList = new List<SelectListItem>();
            QSourceSelectList = new List<SelectListItem>();
            CategoryCodeSelectList = new List<SelectListItem>();
            TransactionCodeTypeSelectList = new List<SelectListItem>();
            CategoryCodeTypeSelectList = new List<SelectListItem>();
        }
    }

    public class AdHocFilterViewModel
    {
        public List<AdhocFilterVM> AdHocDetails { get; set; }
        public List<SelectListItem> VarCharList { get; set; }
        public List<SelectListItem> DateRangeList { get; set; }
        public List<SelectListItem> XDaysList { get; set; }
        public List<SelectListItem> NumericList { get; set; }
        public List<SelectListItem> ZipVarCharList { get; internal set; }

        public AdHocFilterViewModel()
        {
            AdHocDetails = new List<AdhocFilterVM>();
            VarCharList = new List<SelectListItem>();
            DateRangeList = new List<SelectListItem>();
            XDaysList = new List<SelectListItem>();
            NumericList = new List<SelectListItem>();
            ZipVarCharList = new List<SelectListItem>();
        }
    }
    public class AdhocFilterVM
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public  List<FrameworkUAD.Entity.Adhoc> AdHocFilter { get; set; }
        public AdhocFilterVM()
        {
            AdHocFilter = new List<FrameworkUAD.Entity.Adhoc>();
        }
    }
    public class Dimension
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string RefColumn { get; set; }
        public List<FrameworkUAD.Entity.MasterCodeSheet> DimList { get; set; }
        public Dimension()
        {
            DimList = new List<FrameworkUAD.Entity.MasterCodeSheet>();
        }

    }
    public class Demos
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string RefColumn { get; set; }
        public List<FrameworkUAD.Entity.CodeSheet> DimList { get; set; }
        public Demos()
        {
            DimList = new List<FrameworkUAD.Entity.CodeSheet>();
        }

    }
    public class DownLoadPopupViewModel
    {

        public bool PanelUADExportVisible { get; set; }
        public bool ViewDownloadVisible { get; set; }
        public bool ViewSaveToCampaignVisible { get; set; }
        public bool ViewExportToGroupVisible { get; set; }
        public bool ViewMarketoVisible { get; set; }
        public bool NewGroupVisible { get; set; }
        public bool ExistingGroupVisible { get; set; }
        public bool PromoCodeVisible { get; set; }
        public bool ExportFieldsVisible { get; set; }
        public bool DownloadCountVisible { get; set; }
        public bool IsPopupCrossTab { get; set; }
        public bool IsPopupDimension { get; set; }
        public bool IsMostRecentData { get; set; }
        public bool IsError { get; set; }
        public bool IsCirc { get; set; }
        public bool IsQueryDetailsIncluded { get; set; }
        public bool IsExistingGroupChecked { get; set; }
        public string PermissionText { get; set; }
        public string CodeSheetIDText { get; set; }
        public string HeaderText { get; set; }
        public string SelectedFilterNos { get; set; }
        public string SuppressedFilterNos { get; set; }
        public string SelectedFilterOperation { get; set; }
        public string SuppressedFilterOperation { get; set; }
        public string MasterOptionSelected { get; set; }
        public string filterCombination { get; set; }
        public string PromoCode { get; set; }
        public int DownloadTemplateID { get; set; }
        public int CustomerClientID { get; set; }
        public int CustomerClientGroupID { get; set; }
        public int FolderID { get; set; }
        public string GroupName { get; set; }
       
        public int GroupID { get; set; }
        public int DownloadCount { get; set; }
        public int TotalCount { get; set; }
        public int BrandID { get; set; }
        public int CampaignID { get; set; }
        public string CampaignName { get; set; }
        public string FilterName { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorType { get; set; }
        public List<int> PubIDs { get; set; }
       

        #region Data Compare Properties
        public bool IsBillable { get; set; }
        public string DataCompareSelectedValue { get; set; } 
        public int DCRunID { get; set; }
        public int dcTypeCodeID { get; set; }
        public int dcTargetCodeID { get; set; }
        public int matchedRecordsCount { get; set; }
        public int nonMatchedRecordsCount { get; set; }
        public int TotalFileRecords { get; set; }
        public string Notes { get; set; }
        #endregion

       
        public FrameworkUAD.BusinessLogic.Enums.ViewType ViewType { get; set; }
        public List<int> SubscriptionIDs { get; set; }
        public string SubscribersQueries { get; set; }
        public List<SelectListItem> AvailableProfileFields { get; set; }
        public List<SelectListItem> AvailableDemoFields { get; set; }
        public List<SelectListItem> AvailableAdhocFields { get; set; }
        public List<SelectListItem> SelectedItems { get; set; }
        public List<SelectListItem> DownLoadTemplates { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public bool IsNewGroupChecked { get;  set; }
        public string JobCode { get;  set; }
        public bool IsNewCampaign { get;  set; }
        public bool IsExistingCampaign { get;  set; }
        public int IssueID { get; set; }
        public FrameworkUAD.Object.FilterMVC filtermvc { get; set; }
        public DownLoadPopupViewModel()
        {
            Customers = new List<SelectListItem>();
            DownLoadTemplates = new List<SelectListItem>();
            filtermvc = new FrameworkUAD.Object.FilterMVC();
            SubscriptionIDs = new List<int>();
            AvailableProfileFields = new List<SelectListItem>();
            AvailableDemoFields = new List<SelectListItem>();
            AvailableAdhocFields = new List<SelectListItem>();
            SelectedItems = new List<SelectListItem>();
            PubIDs = new List<int>();
        }
    }
    public class ReportViewModel
    {
        public bool IsAddRemove { get; set; }
        public int ReportID { get; set; }
        public int ClientID { get; set; }
        public int PubID { get; set; }
        public string filterquery { get; set; }
        public string reportname { get; set; }
        public string ProductName { get; set; }
        public string IssueName { get; set; }
        public int IssueID { get; set; }
        public Telerik.Reporting.TypeReportSource reportSource { get; set; }
        public List<int> ReportIDs { get; set; }
        public ReportViewModel()
        {
            IsAddRemove = false;
            reportSource = new Telerik.Reporting.TypeReportSource();
            ReportIDs = new List<int>();
        }
    }
    
    public class SaveFilterViewModel
    {
        public bool IsCirc { get; set; }
        public bool IsNew { get; set; }
        public bool IsLockedForSharing { get; set; }
        public bool IsAddedToSalesView { get; set; }
        public int PubID { get; set; }
        public int FilterID { get; set; }
        public string FilterIDs { get; set; }
        public string FilterName { get; set; }
        public int FilterCategoryID { get; set; }
        public string QuestionName { get; set; }
        public string NewExisting { get; set; }
        public int QuestionCategoryID { get; set; }
        public int BrandID { get; set; }
        public string Mode { get; set; }
        public string viewType { get; set; }
        public string Notes { get; set; }
        public string SourcePage { get; set; }
        public FrameworkUAD.Object.FilterMVC CurrentFilter { get; set; }
        public IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> FilterCategoryTree { get; set; }
        public IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> QuestionCategoryTree { get; set; }

        public SaveFilterViewModel()
        {
            FilterID = 0;
            PubID = 0;
            BrandID = 0;
            FilterIDs = string.Empty;
            Mode = "AddNew";
            viewType = "";
            FilterCategoryTree = null;
            QuestionCategoryTree = null;
            CurrentFilter = new FrameworkUAD.Object.FilterMVC();
        }

    }

}