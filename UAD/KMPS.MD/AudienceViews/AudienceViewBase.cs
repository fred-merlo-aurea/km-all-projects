using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using KMPS.MD.Controls;
using KMPS.MD.Objects;
using Telerik.Web.UI;
using static KMPlatform.Enums;
using FilterMVC = FrameworkUAD.BusinessLogic.FilterMVC;
using Guard = KM.Common.Guard;
using KMPlatformUser = KM.Platform.User;
using MDControls = KMPS.MD.Controls;


namespace KMPS.MD.Main
{
    public abstract class AudienceViewBase : BrandsPageBase
    {
        internal const string NamePermission = "Permission";
        internal const string NameCrossTab = "CrossTab";
        internal const string NameGrandTotal = "Grand Total";
        internal const string NameNoResponse = "ZZZ. NO RESPONSE";
        internal const char VBarChar = '|';
        internal const char CommaChar = ',';
        internal const string NameCompany = "Company";
        internal const string NameTitle = "Title";
        internal const string NameCity = "City";
        internal const string NameState = "State";
        internal const string NameZip = "Zip";
        internal const string NameCountry = "Country";
        internal const string NameAudienceViews = "Audience Views";
        internal const string NameProduct = "Product";
        internal const string NameManualLoadClientIds = "ManualLoad_ClientIDs";
        internal const string NameAutoLoad = "Auto Load";
        internal const string NameBrand = "Brand";
        internal const string NameEmail = "Email";
        internal const string NamePhone = "Phone";
        internal const string NameFax = "Fax";
        internal const string NameMedia = "Media";
        internal const string NameMailUpper = "MAIL";
        internal const string NameMailPermission = "MailPermission";
        internal const string NameFaxPermission = "FaxPermission";
        internal const string NamePhonePermission = "PhonePermission";
        internal const string NameOtherProductsPermission = "OtherProductsPermission";
        internal const string NameThirdPartyPermission = "ThirdPartyPermission";
        internal const string NameEmailRenewPermission = "EmailRenewPermission";
        internal const string NameTextPermission = "TextPermission";
        internal const string NameEmailStatus = "Email Status";
        internal const string TextManualLoad = "Manual Load";
        internal const string TextAdhoc = "Adhoc";
        internal const string NameDataCompare = "DataCompare";
        internal const string SortDirectionAscending = "ASC";
        internal const string TextValue = "Value";
        internal const string LblResponseGroup = "lblResponseGroup";
        internal const string LstResponse = "lstResponse";
        internal const string ZipcodeRadius = "Zipcode-Radius";
        internal const string GeoLocated = "GeoLocated";
        internal const string IsolationValid = "ISLATLONVALID";
        internal const string NameUnitedStates = "UNITED STATES";
        internal const string NameCanadaUpper = "CANADA";
        internal const string NameStateUpper = "STATE";
        internal const string NameCountryUpper = "COUNTRY";
        internal const string NameEmailUpper = "EMAIL";
        internal const string NamePhoneUpper = "PHONE";
        internal const string NameFaxUpper = "FAX";
        internal const string NameMediaUpper = "MEDIA";
        internal const string NameGeoLocatedUpper = "GEOLOCATED";
        internal const string NameMailPermissionUpper = "MAILPERMISSION";
        internal const string NameFaxPermissionUpper = "FAXPERMISSION";
        internal const string NamePhonePermissionUpper = "PHONEPERMISSION";
        internal const string NameOtherProductsPermissionUpper = "OTHERPRODUCTSPERMISSION";
        internal const string NameThirdPartyPermissionUpper = "THIRDPARTYPERMISSION";
        internal const string NameEmailRenewPermissionUpper = "EMAILRENEWPERMISSION";
        internal const string NameTextPermissionUpper = "TEXTPERMISSION";
        internal const string NameEmailStatusPermissionUpper = "EMAIL STATUS";
        internal const string MatchedRecords = "MATCHEDRECORDS";
        internal const string Matched = "Matched";
        internal const string NonMatchedRecords = "NONMATCHEDRECORDS";
        internal const string NonMatched = "NonMatched";
        internal const string MatchedNotInProduct = "MATCHEDNOTINPRODUCT";
        internal const string MatchedNotInProductSmall = "MatchedNotInProduct";
        internal const string SelectDistinctSubscriptionIdQuery = " select distinct 1, s.SubscriptionID  from DataCompareProfile d with(nolock)  join Subscriptions s with(Nolock) on d.IGRP_NO = s.IGrp_No ";
        internal const string XmlQueriesOpenTag = "<xml><Queries>";
        internal const string ResultsXmlCloseTag = "</Results></xml>";
        internal const string QueriesCloseResultsOpenTag = "</Queries><Results>";
        internal const string MatchedNotInSelected = "Matched NotIn Selected";
        internal const string ReturnConfirmPopupPurchase = "return confirmPopupPurchase();";
        internal const string UnitedStatesMask = "#####";
        internal const string NonUnitedStatesMask = "L#L #L#";
        internal const string NameAsc = "ASC";

        protected abstract Panel PnlDataCompare { get; }
        protected abstract DropDownList DrpDataCompareSourceFile { get; }
        protected abstract DropDownList DrpCountry { get; }
        protected abstract RadioButtonList RblLoadType { get; }
        protected abstract RadioButtonList RblDataCompareOperation { get; }
        protected abstract ListBox LstCountryRegions { get; }
        protected abstract ListBox LstGeoCode { get; }
        protected abstract ListBox LstState { get; }
        protected abstract ListBox LstCountry { get; }
        protected abstract RadComboBox RcbEmail { get; }
        protected abstract RadComboBox RcbPhone { get; }
        protected abstract RadComboBox RcbFax { get; }

        protected abstract RadComboBox RcbIsLatLonValid { get; }
        protected abstract RadComboBox RcbMailPermission { get; }
        protected abstract RadComboBox RcbFaxPermission { get; }
        protected abstract RadComboBox RcbPhonePermission { get; }
        protected abstract RadComboBox RcbOtherProductsPermission { get; }
        protected abstract RadComboBox RcbThirdPartyPermission { get; }
        protected abstract RadComboBox RcbEmailRenewPermission { get; }
        protected abstract RadComboBox RcbTextPermission { get; }
        protected abstract RadMaskedTextBox RadMaskedTxtBoxZipCode { get; }
        protected abstract MD.Controls.Adhoc AdhocFilterBase { get; }
        protected abstract MD.Controls.Activity ActivityFilterBase { get; }
        protected abstract MD.Controls.Circulation CirculationFilterBase { get; }
        protected abstract DataList DataListDimensions { get; }
        protected abstract RadComboBox RcbMedia { get; }
        protected abstract RadComboBox RcbEmailStatus { get; }
        protected abstract TextBox TxtRadiusMin { get; }
        protected abstract TextBox TxtRadiusMax { get; }
        protected abstract RadMaskedTextBox RadMtxtboxZipCodeCtrl { get; }
        protected abstract MDControls.Activity ActivityFilterCtrl { get; }
        protected abstract MDControls.Adhoc AdhocFilterCtrl { get; }
        protected abstract MDControls.Circulation CirculationFilterCtrl { get; }
        protected abstract DataList DlDimensions { get; }
        protected abstract LinkButton LnkSavedFilter { get; }
        protected abstract Button BtnOpenSaveFilterPopup { get; }
        protected abstract Button BtnLoadComboVenn { get; }
        protected abstract Panel BpResults { get; }
        protected abstract HiddenField HfBrandID { get; }
        protected abstract DownloadPanel DownloadPanel1Ctrl { get; }
        protected abstract DownloadPanel_CLV CLDownloadPanelCtrl { get; }
        protected abstract DownloadPanel_EV EVDownloadPanelCtrl { get; }
        protected abstract FilterSegmentation FilterSegmentationCtrl { get; }

        protected Filters fc;
        protected List<Pubs> lpubs = new List<Pubs>();

        protected delegate void RebuildSubscriberList();
        protected delegate void HidePanel();
        protected delegate void LoadSelectedFilterData(List<int> filterIDs);
        protected delegate void  LoadSelectedFilterSegmentationData(int filtersegmentationIDs);
        protected delegate void LoadSavedFilterName(string filtername);
        
        public int grdCrossTabIntersectRowCount = 0;
        public int grdCrossTabUnionRowCount = 0;

        public Filters FilterCollection
        {
            get
            {
                if (Session[fcSessionName] == null)
                {
                    Session[fcSessionName] = new Filters(Master.clientconnections, Master.LoggedInUser);
                }

                return (Filters)Session[fcSessionName];
            }
            set
            {
                Session[fcSessionName] = value;
            }
        }

        protected string fcSessionName
        {
            get
            {
                if (ViewState["fcSessionName"] == null)
                {
                    ViewState["fcSessionName"] = "filtercollection_" + Guid.NewGuid();
                }

                return ViewState["fcSessionName"].ToString();
            }
            set
            {
                ViewState["fcSessionName"] = value;
            }
        }

        protected string SortField
        {
            get
            {
                return ViewState["SortField"].ToString();
            }
            set
            {
                ViewState["SortField"] = value;
            }
        }

        protected string SortDirection
        {
            get
            {
                return ViewState["SortDirection"].ToString();
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }

        protected int CampaignID
        {
            get
            {
                return Convert.ToInt32(ViewState["CampaignID"]);
            }
            set
            {
                ViewState["CampaignID"] = value;
            }
        }

        protected int CampaignFilterID
        {
            get
            {
                return Convert.ToInt32(ViewState["CampaignFilterID"]);
            }
            set
            {
                ViewState["CampaignFilterID"] = value;
            }
        }

        protected int PopupCampaignID
        {
            get
            {
                return Convert.ToInt32(ViewState["PopupCampaignID"]);
            }
            set
            {
                ViewState["PopupCampaignID"] = value;
            }
        }

        protected int PopupCampaignFilterID
        {
            get
            {
                return Convert.ToInt32(ViewState["PopupCampaignFilterID"]);
            }
            set
            {
                ViewState["PopupCampaignFilterID"] = value;
            }
        }

        protected int PopupCampaignFilterAllID
        {
            get
            {
                return Convert.ToInt32(ViewState["PopupCampaignFilterAllID"]);
            }
            set
            {
                ViewState["PopupCampaignFilterAllID"] = value;
            }
        }

        public Enums.ViewType ViewType
        {
            get
            {
                try
                {
                    return (Enums.ViewType)Enum.Parse(typeof(Enums.ViewType), Request.QueryString["ViewType"].ToString());
                }
                catch
                {
                    return Enums.ViewType.ConsensusView;
                }
            }
        }

        protected new MasterPages.Site Master 
        {
            get 
            {
                return (MasterPages.Site)base.Master;
            }
        }

        protected void AddFilter(int filterId, bool manualLoad, bool dataCompare = false, Field dataComparefield = null)
        {
            Filters filters;
            if (dataCompare)
            {
                filters = MDFilter.LoadFilters(Master.clientconnections, filterId, Master.LoggedInUser, dataComparefield);
            }
            else
            {
                filters = MDFilter.LoadFilters(Master.clientconnections, filterId, Master.LoggedInUser);
            }

            foreach (var filter in filters)
            {
                filter.FilterNo = fc.Count + 1;

                if (manualLoad)
                {
                    fc.Add(filter, true);
                }
                else
                {
                    fc.Add(filter);
                }
            }
        }

        protected void LoadFilters(List<int> filterIDs)
        {
            var dataCompare = false;

            if (PnlDataCompare != null)
            {
                dataCompare = PnlDataCompare.Visible && DrpDataCompareSourceFile.SelectedValue != string.Empty;
            }

            var manualLoad = RblLoadType.SelectedValue.Equals("Manual Load", StringComparison.OrdinalIgnoreCase);
            var dataCompareField = new Field();
            if (dataCompare)
            {
                var compareValues = DrpDataCompareSourceFile.SelectedValue.Split('|').First() + "|" + RblDataCompareOperation.SelectedValue;
                var compareText = DrpDataCompareSourceFile.SelectedItem.Text + "|" + RblDataCompareOperation.SelectedItem.Text;
                dataCompareField = new Field("DataCompare", compareValues, compareText, string.Empty, Enums.FiltersType.DataCompare, "DataCompare");
            }
            
            foreach (var filterId in filterIDs)
            {
                AddFilter(filterId, manualLoad, dataCompare, dataCompareField);
            }

            if (fc.Count > 0)
            {
                if (manualLoad == false)
                {
                    fc.Execute();
                }
                else
                {
                    fc.FilterComboList = null;
                }

                FilterCollection = fc;

                if (PnlDataCompare != null && PnlDataCompare.Visible)
                {
                    DrpDataCompareSourceFile.Enabled = false;
                    RblDataCompareOperation.Enabled = false;
                }
            }
        }

        protected void SelectStateOnRegion(ListBox geoCodeList, ListBox statesList)
        {
            // Get all the geocodes, see if they are selected, then select the appropriate state if so
            foreach (var index in geoCodeList.GetSelectedIndices())
            {
                var selectedRegions = Region.GetByRegionGroupID(Convert.ToInt32(geoCodeList.Items[index].Value));

                foreach (var region in selectedRegions)
                {
                    foreach (ListItem listItem in statesList.Items)
                    {
                        if (region.RegionCode.Equals(listItem.Value, StringComparison.OrdinalIgnoreCase))
                        {
                            listItem.Selected = true;
                        }
                    }
                }
            }
        }

        protected void SelectCountryOnRegion(ListBox countryRegionsList, ListBox countriesList)
        {
            // Get all the geocodes, see if they are selected, then select the appropriate state if so
            foreach (var index in countryRegionsList.GetSelectedIndices())
            {
                var selectedCountries = Country.GetByArea(countryRegionsList.Items[index].Value);

                foreach (var country in selectedCountries)
                {
                    foreach (ListItem listItem in countriesList.Items)
                    {
                        if (country.CountryID == Convert.ToInt32(listItem.Value))
                        {
                            listItem.Selected = true;
                        }
                    }
                }
            }
        }

        protected void AddFilterFieldsComboBox(Filter filter)
        {
            Action<RadComboBox, string> addComboBoxItems = (radComboBox, fieldName) =>
                {
                    if (radComboBox.CheckedItems.Count > 0)
                    {
                        var selectedItem = Utilities.getRadComboBoxSelectedExportFields(radComboBox);
                        filter.Fields.Add(new Field(fieldName, selectedItem.Item1, selectedItem.Item2, string.Empty, Enums.FiltersType.Standard, string.Concat(fieldName.Split(' ')).ToUpper()));
                    }
                };
            addComboBoxItems(this.RcbEmail, NameEmail);
            addComboBoxItems(this.RcbPhone, NamePhone);
            addComboBoxItems(this.RcbFax, NameFax);
            addComboBoxItems(this.RcbMedia, NameMedia);

            if (RcbIsLatLonValid != null && this.RcbIsLatLonValid.CheckedItems.Count > 0)
            {
                var selectedItem = Utilities.getRadComboBoxSelectedExportFields(this.RcbIsLatLonValid);
                filter.Fields.Add(new Field(GeoLocated, selectedItem.Item1, selectedItem.Item2, string.Empty, Enums.FiltersType.Standard, IsolationValid));
            }

            addComboBoxItems(this.RcbMailPermission, NameMailPermission);
            addComboBoxItems(this.RcbFaxPermission, NameFaxPermission);
            addComboBoxItems(this.RcbPhonePermission, NamePhonePermission);
            addComboBoxItems(this.RcbOtherProductsPermission, NameOtherProductsPermission);
            addComboBoxItems(this.RcbThirdPartyPermission, NameThirdPartyPermission);
            addComboBoxItems(this.RcbEmailRenewPermission, NameEmailRenewPermission);
            addComboBoxItems(this.RcbTextPermission, NameTextPermission);
            addComboBoxItems(this.RcbEmailStatus, NameEmailStatus);
        }

        protected void AddFilterFieldsGeoAdhocActivityCirculationPopup(Filter filter)
        {
            if (!string.IsNullOrWhiteSpace(this.RadMaskedTxtBoxZipCode.TextWithLiterals))
            {
                if (!string.IsNullOrWhiteSpace(this.TxtRadiusMin.Text) && !string.IsNullOrWhiteSpace(this.TxtRadiusMax.Text) && int.Parse(this.TxtRadiusMin.Text) < int.Parse(this.TxtRadiusMax.Text))
                {
                    double locationLat;
                    double locationLon;
                    var radiusMin = IntTryParse(this.TxtRadiusMin.Text);
                    var radiusMax = IntTryParse(this.TxtRadiusMax.Text);
                    var values = FilterMVC.CalculateZipCodeRadius(radiusMin, radiusMax, this.RadMaskedTxtBoxZipCode.TextWithLiterals, this.DrpCountry.SelectedValue, out locationLat, out locationLon);
                    filter.Fields.Add(
                        new Field(
                            ZipcodeRadius,
                            string.Join("|", values),
                            $"{this.RadMaskedTxtBoxZipCode.TextWithLiterals} & {this.TxtRadiusMin.Text} miles - {this.TxtRadiusMax.Text} miles",
                            $"{this.RadMaskedTxtBoxZipCode.TextWithLiterals}|{this.TxtRadiusMin.Text}|{this.TxtRadiusMax.Text}",
                            Enums.FiltersType.Geo,
                            ZipcodeRadius.ToUpper()));
                }
            }

            foreach (var field in this.AdhocFilterBase.GetAdhocFilters())
            {
                filter.Fields.Add(field);
            }

            foreach (var field in this.ActivityFilterBase.GetActivityFilters())
            {
                filter.Fields.Add(field);
            }

            foreach (var field in this.CirculationFilterBase.GetCirculationFilters())
            {
                filter.Fields.Add(field);
            }
        }

        protected void AddFilterFieldsListItems(List<ResponseGroup> responseGroup, Filter filter)
        {
            foreach (DataListItem dataListItem in this.DataListDimensions.Items)
            {
                var lblResponseGroup = dataListItem.FindControl(LblResponseGroup) as Label;
                var lstResponse = (ListBox)dataListItem.FindControl(LstResponse);

                if (lstResponse != null)
                {
                    var selectedvalues = Utilities.getListboxSelectedValues(lstResponse);

                    if (selectedvalues.Length > 0)
                    {
                        foreach (var responseGroupItem in responseGroup)
                        {
                            if (lblResponseGroup?.Text.Equals(responseGroupItem.DisplayName, StringComparison.OrdinalIgnoreCase) == true)
                            {
                                filter.Fields.Add(
                                    new Field(
                                        lblResponseGroup.Text,
                                        selectedvalues,
                                        Utilities.getListboxText(lstResponse),
                                        string.Empty,
                                        Enums.FiltersType.Dimension,
                                        responseGroupItem.ResponseGroupName));

                                break;
                            }
                        }
                    }
                }
            }
        }

        protected bool HandleBtnOpenSaveFilterPopupClick(
            GridView grdFilters,
            MDControls.FilterPanel filterSave, 
            string brandID, 
            Enums.ViewType viewType,
            string filterCheckBoxName = "cbSelectFilter",
            string productId = "0")
        {
            Guard.NotNull(grdFilters, nameof(grdFilters));
            Guard.NotNull(filterSave, nameof(filterSave));

            if (KMPlatformUser.HasAccess(Master.UserSession.CurrentUser, Services.UAD, ServiceFeatures.UADFilter, Access.Edit))
            {
                var ischecked = false;
                var filterNos = new StringBuilder();
                var i = 0;
                foreach (GridViewRow gridViewRow in grdFilters.Rows)
                {
                    var checkBox = gridViewRow.FindControl(filterCheckBoxName) as CheckBox;
                    if (checkBox != null && checkBox.Checked)
                    {
                        ischecked = true;
                        var dataKeyFilterValue = grdFilters.DataKeys[i]?.Values["FilterNo"]?.ToString();
                        if (filterNos.Length == 0)
                        {
                            filterNos.Append(dataKeyFilterValue);
                        }
                        else
                        {
                            filterNos.AppendFormat(",{0}", dataKeyFilterValue);
                        }
                    }
                    i++;
                }

                if (!ischecked)
                {
                    return false;
                }

                filterSave.Mode = "AddNew";
                filterSave.BrandID = Guard.ParseStringToInt(brandID);
                filterSave.PubID = Guard.ParseStringToInt(productId);
                filterSave.FilterIDs = filterNos.ToString();
                filterSave.UserID = Master.LoggedInUser;
                filterSave.FilterCollections = fc;
                filterSave.ViewType = viewType;
                filterSave.LoadControls();
                filterSave.Visible = true;
            }

            return true;
        }

        protected void HandleDrpCountrySelectedIndexChanged(RadMaskedTextBox radMtxtboxZipCode, DropDownList drpCountry)
        {
            Guard.NotNull(radMtxtboxZipCode, nameof(radMtxtboxZipCode));
            Guard.NotNull(drpCountry, nameof(drpCountry));

            radMtxtboxZipCode.Text = string.Empty;
            radMtxtboxZipCode.Mask = NameUnitedStates.Equals(drpCountry.SelectedValue, StringComparison.OrdinalIgnoreCase)
                                        ? UnitedStatesMask
                                        : NonUnitedStatesMask;
        }

        protected void ClearAndResetFilterTabControls()
        {
            foreach (DataListItem dataListItem in DlDimensions.Items)
            {
                var lstResponse = dataListItem.FindControl("lstResponse") as ListBox;
                if (lstResponse != null)
                {
                    lstResponse.ClearSelection();
                }
            }

            LstCountryRegions.ClearSelection();
            LstGeoCode.ClearSelection();
            LstState.ClearSelection();
            LstCountry.ClearSelection();
            RcbEmail.ClearCheckedItems();
            RcbPhone.ClearCheckedItems();
            RcbFax.ClearCheckedItems();
            RcbIsLatLonValid?.ClearCheckedItems();
            RcbMailPermission.ClearCheckedItems();
            RcbFaxPermission.ClearCheckedItems();
            RcbPhonePermission.ClearCheckedItems();
            RcbOtherProductsPermission.ClearCheckedItems();
            RcbThirdPartyPermission.ClearCheckedItems();
            RcbEmailRenewPermission.ClearCheckedItems();
            RcbTextPermission.ClearCheckedItems();
            RcbMedia.ClearCheckedItems();
            RcbEmailStatus.ClearCheckedItems();
            TxtRadiusMin.Text = string.Empty;
            TxtRadiusMax.Text = string.Empty;
            DrpCountry.ClearSelection();
            RadMtxtboxZipCodeCtrl.Mask = "#####";
            RadMtxtboxZipCodeCtrl.Text = string.Empty;

            // Activity popup
            ActivityFilterCtrl.Reset();

            // Adhoc popup
            AdhocFilterCtrl.Reset();

            //Circulation Popup
            CirculationFilterCtrl.Reset();
        }

        protected void InitializeForPostBack(GridView grdFilters, HiddenField hfFilterGroupName = null)
        {
            Guard.NotNull(grdFilters, nameof(grdFilters));

            if (fc?.Any() == true || grdFilters.Rows.Count <= 0)
            {
                return;
            }

            for (var i = 0; i < grdFilters.Rows.Count; i++)
            {
                if (grdFilters.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    var grdFilterValues = (GridView)grdFilters.Rows[i].FindControl("grdFilterValues");
                    var lnkCount = (LinkButton)grdFilters.Rows[i].FindControl("lnkdownload");
                    var lblBrandId = (Label)grdFilters.Rows[i].FindControl("lblBrandID");
                    var lblDbFilterName = (Label)grdFilters.Rows[i].FindControl("lblFiltername");
                    var hiddenFilterGroupName = hfFilterGroupName == null
                        ? (HiddenField)grdFilters.Rows[i].FindControl("hfFilterGroupName")
                        : hfFilterGroupName;
                    var hfViewType = (HiddenField)grdFilters.Rows[i].FindControl("hfViewType");
                    Enums.ViewType viewType;
                    Enum.TryParse(hfViewType.Value, out viewType);

                    var filter = new Filter
                    {
                        FilterNo = GetInt32(grdFilters.DataKeys[i].Values["FilterNo"].ToString()),
                        FilterName = lblDbFilterName.Text,
                        FilterGroupName = hiddenFilterGroupName.Value,
                        Count = GetInt32(lnkCount.Text),
                        BrandID = GetInt32(lblBrandId.Text),
                        ViewType = viewType
                    };

                    foreach (GridViewRow rowItem in grdFilterValues.Rows)
                    {
                        var lblFilterText = (Label)rowItem.FindControl("lblFilterText");
                        var lblFilterName = (Label)rowItem.FindControl("lblFiltername");
                        var lblFilterValues = (Label)rowItem.FindControl("lblFilterValues");
                        var lblSearchCondition = (Label)rowItem.FindControl("lblSearchCondition");
                        var lblFilterType = (Label)rowItem.FindControl("lblFilterType");
                        var lblGroup = (Label)rowItem.FindControl("lblGroup");
                        var filterType = (Enums.FiltersType)Enum.Parse(typeof(Enums.FiltersType), lblFilterType.Text);

                        if (lblFilterName.Text == TextAdhoc)
                        {
                            filter.Fields.Add(new Field(TextAdhoc, lblFilterValues.Text, lblFilterText.Text, lblSearchCondition.Text,
                                lblFilterType.Text == string.Empty ? Enums.FiltersType.None : filterType, lblGroup.Text));
                        }
                        else
                        {
                            filter.Fields.Add(
                                new Field(
                                    lblFilterName.Text, 
                                    lblFilterValues.Text, 
                                    lblFilterText.Text, 
                                    lblSearchCondition.Text,
                                    lblFilterType.Text == string.Empty ? Enums.FiltersType.None : filterType, lblGroup.Text));
                        }
                    }

                    AddDataCompareFieldIfSourceFileSelected(filter);

                    if (TextManualLoad.Equals(RblLoadType.SelectedValue, StringComparison.OrdinalIgnoreCase))
                    {
                        fc?.Add(filter, true);
                    }
                    else
                    {
                        fc?.Add(filter);
                    }
                }
            }

            if (NameAutoLoad.Equals(RblLoadType.SelectedValue, StringComparison.OrdinalIgnoreCase))
            {
                fc?.Execute();
            }
        }

        protected virtual void AddDataCompareFieldIfSourceFileSelected(Filter filter)
        {
            if (PnlDataCompare.Visible)
            {
                if (DrpDataCompareSourceFile.SelectedValue != string.Empty)
                {
                    filter.Fields.Add(new Field(
                        NameDataCompare,
                        $"{DrpDataCompareSourceFile.SelectedValue.Split(VBarChar).First()}|{RblDataCompareOperation.SelectedValue}",
                        $"{DrpDataCompareSourceFile.SelectedItem.Text}|{RblDataCompareOperation.SelectedItem.Text}",
                        string.Empty,
                        Enums.FiltersType.DataCompare,
                        NameDataCompare));
                }
            }
        }

        // This method is left empty as a default implementation intentionly.
        // It should be overridden by child classes if needed to initialize data binding.
        protected virtual void InitializeDataBinding()
        {

        }

        protected void InitializeForHttpGet(string sortField, Enums.ViewType viewType)
        {
            if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, Services.UAD, ServiceFeatures.UADFilter, Access.View))
            {
                LnkSavedFilter.Visible = false;
            }

            if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, Services.UAD, ServiceFeatures.UADFilter, Access.Edit))
            {
                BtnOpenSaveFilterPopup.Visible = false;
            }

            var clientId = ConfigurationManager.AppSettings[NameManualLoadClientIds].Split(CommaChar);

            if (clientId.Contains(Master.UserSession.ClientID.ToString()))
            {
                RblLoadType.SelectedValue = TextManualLoad;
                BtnLoadComboVenn.Visible = true;
            }

            BpResults.Visible = false;
            SortField = sortField;
            SortDirection = NameAsc;

            InitializeDataBinding();

            LoadBrands();
            LoadStandardFiltersListboxes();

            RcbEmailStatus.DataSource = EmailStatus.GetAll(Master.clientconnections);
            RcbEmailStatus.DataBind();

            DownloadPanel1Ctrl.error = false;
            DownloadPanel1Ctrl.BrandID = GetInt32(HfBrandID.Value);
            DownloadPanel1Ctrl.ViewType = ViewType;
            DownloadPanel1Ctrl.Showexporttoemailmarketing = true;
            DownloadPanel1Ctrl.Showsavetocampaign = true;
            DownloadPanel1Ctrl.Showexporttomarketo = true;
            CLDownloadPanelCtrl.error = false;
            CLDownloadPanelCtrl.BrandID = GetInt32(HfBrandID.Value);
            CLDownloadPanelCtrl.ViewType = ViewType;
            EVDownloadPanelCtrl.error = false;
            EVDownloadPanelCtrl.BrandID = GetInt32(HfBrandID.Value);
            EVDownloadPanelCtrl.ViewType = ViewType;
            FilterSegmentationCtrl.UserID = Master.LoggedInUser;
            FilterSegmentationCtrl.fcSessionName = fcSessionName;
        }

        private void LoadStandardFiltersListboxes()
        {
            LstCountryRegions.DataSource = Country.GetArea();
            LstCountryRegions.DataBind();

            LstCountry.DataSource = Country.GetAll();
            LstCountry.DataBind();

            LstState.DataSource = Region.GetAll();
            LstState.DataBind();

            LstGeoCode.DataSource = RegionGroup.GetAll();
            LstGeoCode.DataBind();
        }
    }
}