using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using FrameworkUAD.BusinessLogic;
using KM.Common;
using KM.Common.Extensions;
using KMPS.MD.Objects;
using Microsoft.Reporting.WebForms;
using Telerik.Web.UI;
using Brand = KMPS.MD.Objects.Brand;
using CampaignFilter = KMPS.MD.Objects.CampaignFilter;
using DataFunctions = KMPS.MD.Objects.DataFunctions;
using EmailStatus = KMPS.MD.Objects.EmailStatus;
using Enums = KMPS.MD.Objects.Enums;
using Filter = KMPS.MD.Objects.Filter;
using MasterCodeSheet = KMPS.MD.Objects.MasterCodeSheet;
using MasterGroup = KMPS.MD.Objects.MasterGroup;
using Subscriber = KMPS.MD.Objects.Subscriber;
using UadLookupEnums = FrameworkUAD_Lookup.Enums;
using UadLookupWorkers = FrameworkUAD_Lookup.BusinessLogic;
using UasWorkers = FrameworkUAS.BusinessLogic;
using MDControls = KMPS.MD.Controls;

namespace KMPS.MD.Main
{
    public partial class Report : AudienceViewBase
    {
        private const string NameConsensus = "Consensus";
        private const string NameRecency = "Recency";
        private const string NameStandard = "Standard";
        private const string NameValue = "Value";
        private const string NameAddNew = "AddNew";
        private const string NameMailPhoneUpper = "MAIL_PHONE";
        private const string NameEmailPhoneUpper = "EMAIL_PHONE";
        private const string NameMailEmailUpper = "MAIL_EMAIL";
        private const string NameAllRecordsUpper = "ALL_RECORDS";
        private const string DefaultFilterNo = "1";
        private const string NameMatchedRecords = "MATCHEDRECORDS";
        private const string NameNonMatchedRecords = "NONMATCHEDRECORDS";
        private const string NameMatchedNotInBrand = "MatchedNotInBrand";
        private const string NameMatched = "Matched";
        private const string NameNonMatched = "NonMatched";
        private const string NameMatchedNotInSelected = "Matched NotIn Selected";
        private const string PubTypeListBox = "PubTypeListBox";
        private const string Id = "ID";
        private const string MarketTypeXPath = "//Market/MarketType[@ID ='P']";
        private const string FilterTypeXPath = "//Market/FilterType[@ID ='A']";
        private const string HiddenFieldPubTypeId = "hfPubTypeID";
        private const string LinkPubTypeShowHide = "lnkPubTypeShowHide";
        private const string PanelPubTypeBody = "pnlPubTypeBody";
        private const string PubId = "PubID";
        private const string PubName = "PubName";
        private const string TextHide = "(Hide...)";
        private const string HiddenFieldMasterGroup = "hfMasterGroup";
        private const string LinkDimensionsShowHide = "lnkDimensionShowHide";
        private const string PanelDimBody = "pnlDimBody";
        private const string MasterDesc = "MASTERDESC";
        private const string MasterId = "MasterID";
        private const string GridViewCategory = "gvCategory";
        private const string DataListAdhocFilter = "dlAdhocFilter";
        private const string LabelAdhocColumnValue = "lbAdhocColumnValue";
        private const string DropDownAdhocInt = "drpAdhocInt";
        private const string TextBoxAdhocIntFrom = "txtAdhocIntFrom";
        private const string TextBoxAdhocIntTo = "txtAdhocIntTo";
        private const string Equal = "EQUAL";
        private const string Greater = "GREATER";
        private const string Lesser = "LESSER";
        private const string CheckBoxSelectDownload = "cbSelectDownload";
        private const string SelectCheckBox = "Please select a checkbox.";
        private const string SelectExistingOrNewCampaign = "Select existing Campaign or new Campaign";
        private const string SubscriptionId = "SubscriptionID";

        protected override Panel PnlDataCompare => pnlDataCompare;
        protected override DropDownList DrpDataCompareSourceFile => drpDataCompareSourceFile;
        protected override RadioButtonList RblLoadType => rblLoadType;
        protected override RadioButtonList RblDataCompareOperation => rblDataCompareOperation;
        protected override Panel BrandPanel => pnlBrand;
        protected override DropDownList BrandDropDown => drpBrand;
        protected override HiddenField BrandIdHiddenField => hfBrandID;
        protected override Label BrandNameLabel => lblBrandName;
        protected override RadComboBox RcbEmail => rcbEmail;
        protected override RadComboBox RcbPhone => rcbPhone;
        protected override RadComboBox RcbFax => rcbFax;
        protected override RadComboBox RcbMedia => rcbMedia;
        protected override RadComboBox RcbIsLatLonValid => rcbIsLatLonValid;
        protected override RadComboBox RcbMailPermission => rcbMailPermission;
        protected override RadComboBox RcbFaxPermission => rcbFaxPermission;
        protected override RadComboBox RcbPhonePermission => rcbPhonePermission;
        protected override RadComboBox RcbOtherProductsPermission => rcbOtherProductsPermission;
        protected override RadComboBox RcbThirdPartyPermission => rcbThirdPartyPermission;
        protected override RadComboBox RcbEmailRenewPermission => rcbEmailRenewPermission;
        protected override RadComboBox RcbTextPermission => rcbTextPermission;
        protected override RadComboBox RcbEmailStatus => rcbEmailStatus;
        protected override Controls.Adhoc AdhocFilterBase => AdhocFilter;
        protected override Controls.Activity ActivityFilterBase => ActivityFilter;
        protected override Controls.Circulation CirculationFilterBase => CirculationFilter;
        protected override TextBox TxtRadiusMin => txtRadiusMin;
        protected override TextBox TxtRadiusMax => txtRadiusMax;
        protected override DropDownList DrpCountry => drpCountry;
        protected override RadMaskedTextBox RadMaskedTxtBoxZipCode => RadMtxtboxZipCode;
        protected override DataList DataListDimensions => dlDimensions;
        protected override ListBox LstCountryRegions => lstCountryRegions;
        protected override ListBox LstGeoCode => lstGeoCode;
        protected override ListBox LstState => lstState;
        protected override ListBox LstCountry => lstCountry;
        protected override RadMaskedTextBox RadMtxtboxZipCodeCtrl => RadMtxtboxZipCode;
        protected override MDControls.Activity ActivityFilterCtrl => ActivityFilter;
        protected override MDControls.Adhoc AdhocFilterCtrl => AdhocFilter;
        protected override MDControls.Circulation CirculationFilterCtrl => CirculationFilter;
        protected override DataList DlDimensions => dlDimensions;
        protected override LinkButton LnkSavedFilter => lnkSavedFilter;
        protected override Button BtnOpenSaveFilterPopup => btnOpenSaveFilterPopup;
        protected override Button BtnLoadComboVenn => btnLoadComboVenn;
        protected override Panel BpResults => bpResults;
        protected override HiddenField HfBrandID => hfBrandID;
        protected override MDControls.DownloadPanel DownloadPanel1Ctrl => DownloadPanel1;
        protected override MDControls.DownloadPanel_CLV CLDownloadPanelCtrl => CLDownloadPanel;
        protected override MDControls.DownloadPanel_EV EVDownloadPanelCtrl => EVDownloadPanel;
        protected override MDControls.FilterSegmentation FilterSegmentationCtrl => FilterSegmentation;

        private void Reload()
        {
            DownloadPanel1.SubscribersQueries = GetSubscribersQueriesForUserControl();
        }

        private void DownloadPopupHide()
        {
            DownloadPanel1.Visible = false;
        }

        private void CLReload()
        {
            CLDownloadPanel.SubscribersQueries = GetSubscribersQueriesForUserControl();
        }

        private void CLDownloadPopupHide()
        {
            CLDownloadPanel.Visible = false;
        }

        private void EVReload()
        {
            EVDownloadPanel.SubscribersQueries = GetSubscribersQueriesForUserControl();
        }

        private void EVDownloadPopupHide()
        {
            EVDownloadPanel.Visible = false;
        }

        private void AdhocPopupHide()
        {
            AdhocFilter.Visible = false;
        }

        private void ActivityPopupHide()
        {
            ActivityFilter.Visible = false;
        }

        private void FilterSavePopupHide()
        {
            FilterSave.Visible = false;
        }

        private void ShowFilterPopupHide()
        {
            ShowFilter.Visible = false;
        }

        private void CirculationPopupHide()
        {
            CirculationFilter.Visible = false;
        }

        private void DataCompareSummaryPopupHide()
        {
            DataCompareSummary.Visible = false;
        }

        public void LoadFilterName(string filtername)
        {
            foreach (GridViewRow r in grdFilters.Rows)
            {
                CheckBox cb = r.FindControl("cbSelectFilter") as CheckBox;
                int filterNo = Convert.ToInt32(grdFilters.DataKeys[r.RowIndex].Value.ToString());

                if (cb != null && cb.Checked)
                {
                    Filter filter = fc.First(f => f.FilterNo == filterNo);
                    filter.FilterName = filtername.ToString();
                    fc.Update(filter);

                    cb.Checked = false;
                }
            }

            FilterCollection = fc;
            LoadGridFilters();
            FilterSegmentation.FilterViewCollection.Clear();
            FilterSegmentation.LoadControls();
        }

        public void LoadFilterData(List<int> filterIDs)
        {
            ResetFilterControls();
            
            try
            {
                LoadFilters(filterIDs);

                if (fc.Count > 0)
                {
                    LoadGridFilters();

                    if (pnlBrand.Visible)
                    {
                        drpBrand.Enabled = false;
                    }
                }
                else
                {
                    DisplayError("No Records");
                }
            }
            catch (Exception ex)
            {
                Utilities.Log_Error(Request.RawUrl.ToString(), "LoadFilterData", ex);
                DisplayError(ex.Message);
            }

            TabContainer.ActiveTabIndex = 0;
            FilterSegmentation.FilterViewCollection.Clear();
            FilterSegmentation.LoadControls();
        }

        public void LoadFilterSegmentationData(int filtersegmentationID)
        {
            ResetFilterControls();
            fc.Clear();

            try
            {
                var filterID = new FrameworkUAD.BusinessLogic.FilterSegmentation().SelectByID(filtersegmentationID, new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient)).FilterID;
                LoadFilters(new List<int> {filterID});

                if (fc.Count > 0)
                {
                    LoadGridFilters();

                    if (pnlBrand.Visible)
                        drpBrand.Enabled = false;
                    
                    FilterSegmentation.Visible = true;
                    FilterSegmentation.ViewType = ViewType;
                    FilterSegmentation.UserID = Master.LoggedInUser;
                    FilterSegmentation.BrandID = Convert.ToInt32(hfBrandID.Value);
                    FilterSegmentation.fcSessionName = fcSessionName;
                    FilterSegmentation.FilterViewCollection.Clear();
                    FilterSegmentation.LoadControls();
                    FilterSegmentation.FilterSegmentationID = filtersegmentationID;
                    FilterSegmentation.LoadFilterSegmenationData();
                }
                else
                {
                    DisplayError("No Records");
                }
            }
            catch (Exception ex)
            {
                Utilities.Log_Error(Request.RawUrl.ToString(), "LoadFilterSegmentationData", ex);
                DisplayError(ex.Message);
            }

            TabContainer.ActiveTabIndex = 1;

        }

        private void ShowDownloadPanel()
        {
            if (lblPermission.Text == "CrossTab")
            {
                lblCrossTabMsg.Visible = false;
                lblCrossTabMsg.Text = string.Empty;
            }

            hfDataCompareLinkSelected.Value = string.Empty;
            DownloadPanel1.SubscriptionID = null;
            DownloadPanel1.SubscribersQueries = GetSubscribersQueriesForUserControl();

            if (Convert.ToInt32(lblDownloadCount.Text) > 0)
            {
                DownloadPanel1.Visible = true;
                DownloadPanel1.HeaderText = Utilities.GetHeaderText(fc, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, TabContainer.ActiveTabIndex == 1 ? true : false);
                DownloadPanel1.ShowHeaderCheckBox = true;
                DownloadPanel1.BrandID = Convert.ToInt32(hfBrandID.Value);
                DownloadPanel1.PubIDs = null;
                DownloadPanel1.ViewType = ViewType;
                DownloadPanel1.VisibleCbIsRecentData = ViewType == Enums.ViewType.ConsensusView ? false : true;
                DownloadPanel1.filterCombination = lblFilterCombination.Text;
                DownloadPanel1.downloadCount = Convert.ToInt32(lblDownloadCount.Text);
                DownloadPanel1.IsPopupCrossTab = Convert.ToBoolean(lblIsPopupCrossTab.Text);
                DownloadPanel1.IsPopupDimension = Convert.ToBoolean(lblIsPopupDimension.Text);
                if (plDataCompareData.Visible)
                {
                    DownloadPanel1.dcRunID = Convert.ToInt32(hfDcRunID.Value);
                    DownloadPanel1.dcTypeCodeID = Convert.ToInt32(rblDataCompareOperation.SelectedValue);
                    DownloadPanel1.dcTargetCodeID = Convert.ToInt32(hfDcTargetCodeID.Value);
                    DownloadPanel1.matchedRecordsCount = Convert.ToInt32(lnkMatchedRecords.Text);
                    DownloadPanel1.nonMatchedRecordsCount = Convert.ToInt32(lnkNonMatchedRecords.Text);
                    DownloadPanel1.TotalFileRecords = Convert.ToInt32(lblTotalFileRecords.Text);
                }
                DownloadPanel1.LoadControls();
                DownloadPanel1.LoadDownloadTemplate();
                DownloadPanel1.loadExportFields();
                DownloadPanel1.ValidateExportPermission();

                if (lblSelectedFilterOperation.Text.ToUpper() == "SINGLE")
                {
                    foreach (Filter f in fc)
                    {
                        if (f.FilterNo == Convert.ToInt32(lblSelectedFilterNos.Text))
                        {
                            var field = f.Fields.Find(x => x.Group.ToUpper() == "OPENCRITERIA" && x.SearchCondition.ToUpper() == "SEARCH SELECTED PRODUCTS");

                            if (field != null)
                                DownloadPanel1.PubIDs = f.Fields.Find(x => x.Name.ToUpper() == "PRODUCT").Values.Split(',').Select(int.Parse).ToList();
                        }
                    }
                }
            }
            else
            {
                if (lblPermission.Text == "CrossTab")
                {
                    lblCrossTabMsg.Visible = true;
                    lblCrossTabMsg.Text = "No Records";
                }
            }
        }

        private void ShowCLDownloadPanel()
        {
            hfDataCompareLinkSelected.Value = string.Empty;
            
            int brandId, downloadCount, dcRunID, dcTypeCodeID, dcTargetCodeID, matchedRecordsCount, nonMatchedRecordsCount, totalFileRecords = 0;
            int.TryParse(hfBrandID.Value, out brandId);
            int.TryParse(lblDownloadCount.Text, out downloadCount);
            int.TryParse(hfDcRunID.Value, out dcRunID);
            int.TryParse(rblDataCompareOperation.SelectedValue, out dcTypeCodeID);
            int.TryParse(hfDcTargetCodeID.Value, out dcTargetCodeID);
            int.TryParse(lnkMatchedRecords.Text, out matchedRecordsCount);
            int.TryParse(lnkNonMatchedRecords.Text, out nonMatchedRecordsCount);
            int.TryParse(lblTotalFileRecords.Text, out totalFileRecords);
            
            CLDownloadPanel.Show(
                Utilities.GetHeaderText(fc, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, TabContainer.ActiveTabIndex == 1),
                GetSubscribersQueriesForUserControl(),
                brandId,
                ViewType,
                lblFilterCombination.Text,
                downloadCount,
                plDataCompareData.Visible,
                dcRunID,
                dcTypeCodeID,
                dcTargetCodeID,
                matchedRecordsCount,
                nonMatchedRecordsCount,
                totalFileRecords);

            if (lblSelectedFilterOperation.Text.ToUpper() == "SINGLE")
            {
                foreach (Filter f in fc)
                {
                    if (f.FilterNo == Convert.ToInt32(lblSelectedFilterNos.Text))
                    {
                        var field = f.Fields.Find(x => x.Group.ToUpper() == "OPENCRITERIA" && x.SearchCondition.ToUpper() == "SEARCH SELECTED PRODUCTS");

                        if (field != null)
                            CLDownloadPanel.PubIDs = f.Fields.Find(x => x.Name.ToUpper() == "PRODUCT").Values.Split(',').Select(int.Parse).ToList();
                    }
                }
            }
        }

        private void ShowEVDownloadPanel()
        {
            hfDataCompareLinkSelected.Value = string.Empty;
            
            int brandId, downloadCount, dcRunID, dcTypeCodeID, dcTargetCodeID, matchedRecordsCount, nonMatchedRecordsCount, totalFileRecords = 0;
            int.TryParse(hfBrandID.Value, out brandId);
            int.TryParse(lblDownloadCount.Text, out downloadCount);
            int.TryParse(hfDcRunID.Value, out dcRunID);
            int.TryParse(rblDataCompareOperation.SelectedValue, out dcTypeCodeID);
            int.TryParse(hfDcTargetCodeID.Value, out dcTargetCodeID);
            int.TryParse(lnkMatchedRecords.Text, out matchedRecordsCount);
            int.TryParse(lnkNonMatchedRecords.Text, out nonMatchedRecordsCount);
            int.TryParse(lblTotalFileRecords.Text, out totalFileRecords);
            
            EVDownloadPanel.Show(
                Utilities.GetHeaderText(fc, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, TabContainer.ActiveTabIndex == 1),
                GetSubscribersQueriesForUserControl(),
                brandId,
                ViewType,
                lblFilterCombination.Text,
                downloadCount,
                plDataCompareData.Visible,
                dcRunID,
                dcTypeCodeID,
                dcTargetCodeID,
                matchedRecordsCount,
                nonMatchedRecordsCount,
                totalFileRecords);

            if (lblSelectedFilterOperation.Text.ToUpper() == "SINGLE")
            {
                foreach (Filter f in fc)
                {
                    if (f.FilterNo == Convert.ToInt32(lblSelectedFilterNos.Text))
                    {
                        var field = f.Fields.Find(x => x.Group.ToUpper() == "OPENCRITERIA" && x.SearchCondition.ToUpper() == "SEARCH SELECTED PRODUCTS");

                        if (field != null)
                            EVDownloadPanel.PubIDs = f.Fields.Find(x => x.Name.ToUpper() == "PRODUCT").Values.Split(',').Select(int.Parse).ToList();
                    }
                }
            }
        }

        private StringBuilder GetSubscribersQueriesForUserControl()
        {
            StringBuilder Queries = new StringBuilder();
            string addFilters = GetAddlFilter();

            try
            {
                if (hfDataCompareLinkSelected.Value != "")
                {
                    switch (hfDataCompareLinkSelected.Value.ToUpper())
                    {
                        case "MATCHEDRECORDS":
                            if (Convert.ToInt32(hfBrandID.Value) > 0)
                            {
                                Queries.Append("<xml><Queries>");
                                Queries.Append(string.Format("<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>", 1,
                                                "select distinct 1, s.SubscriptionID " +
                                                " from DataCompareProfile d with(nolock) " +
                                                " join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No " +
                                                " join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID " +
                                                " join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  " +
                                                " where d.ProcessCode = '" + hfDataCompareProcessCode.Value + "' " +
                                                " and bd.BrandID = " + Convert.ToInt32(hfBrandID.Value)
                                    ));
                                Queries.Append("</Queries><Results>");
                                Queries.Append(string.Format("<Result linenumber=\"{0}\"  selectedfilterno=\"{1}\" selectedfilteroperation=\"{2}\" suppressedfilterno=\"{3}\" suppressedfilteroperation=\"{4}\"  filterdescription=\"{5}\"></Result>", 1, "1", "", "", "", ""));
                                Queries.Append("</Results></xml>");
                            }
                            else
                            {
                                Queries.Append("<xml><Queries>");
                                Queries.Append(string.Format("<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>", 1,
                                                "select distinct 1, s.SubscriptionID from DataCompareProfile d with(nolock) join Subscriptions s with (Nolock)on d.IGRP_NO = s.IGrp_No where d.ProcessCode = '" + hfDataCompareProcessCode.Value + "'"));
                                Queries.Append("</Queries><Results>");
                                Queries.Append(string.Format("<Result linenumber=\"{0}\"  selectedfilterno=\"{1}\" selectedfilteroperation=\"{2}\" suppressedfilterno=\"{3}\" suppressedfilteroperation=\"{4}\"  filterdescription=\"{5}\"></Result>", 1, "1", "", "", "", ""));
                                Queries.Append("</Results></xml>");
                            }
                            break;
                        case "MATCHEDNOTINBRAND":
                            Queries.Append("<xml><Queries>");
                            Queries.Append(string.Format("<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>", 1,
                                            "select distinct 1, s.SubscriptionID " +
                                            "from DataCompareProfile d with(nolock) " +
                                            "join Subscriptions s with(Nolock) on d.IGRP_NO = s.IGrp_No " +
                                            "left outer join( " +
                                            "            select distinct s1.SubscriptionID " +
                                            "            from DataCompareProfile d1 with(nolock) " +
                                            "            join Subscriptions s1 with(Nolock) on d1.IGRP_NO = s1.IGrp_No " +
                                            "            join PubSubscriptions ps with(nolock) on s1.SubscriptionID = ps.SubscriptionID " +
                                            "            join branddetails bd with(nolock) on bd.pubID = ps.pubID " +
                                            "            where d1.ProcessCode = '" + hfDataCompareProcessCode.Value + "' " +
                                            "            and bd.BrandID = " + Convert.ToInt32(hfBrandID.Value) +
                                            "        ) x on s.SubscriptionID = x.SubscriptionID " +
                                            "where d.ProcessCode = '" + hfDataCompareProcessCode.Value + "' and x.SubscriptionID is null "));
                            Queries.Append("</Queries><Results>");
                            Queries.Append(string.Format("<Result linenumber=\"{0}\"  selectedfilterno=\"{1}\" selectedfilteroperation=\"{2}\" suppressedfilterno=\"{3}\" suppressedfilteroperation=\"{4}\"  filterdescription=\"{5}\"></Result>", 1, "1", "", "", "", ""));
                            Queries.Append("</Results></xml>");
                            break;
                    }
                }
                else
                    Queries = Filter.generateCombinationQuery(fc, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, addFilters, 0, Convert.ToInt32(hfBrandID.Value), Master.clientconnections);
            }
            catch (Exception ex)
            {
                Utilities.Log_Error(Request.RawUrl.ToString(), "GetSubscribersQueriesForUserControl", ex);
                DisplayError(ex.Message);
            }

            lblAddFilters.Text = string.Empty;

            return Queries;
        }

        private string GetAddlFilter()
        {
            var query = lblAddFilters.Text;

            if (lblPermission.Text == NamePermission)
            {
                query = GetPermissionFilters();
            }
            else if (lblPermission.Text == NameCrossTab)
            {
                int selectedId;
                int.TryParse(drpCrossTabReport.SelectedValue, out selectedId);
                if (selectedId > 0)
                {
                    query = GetCrossTabFilters(query);
                }
            }

            return query;
        }

        private string GetPermissionFilters()
        {
            var query = GetPermissionFiltersByMasterId();

            switch (lblPremissionType.Text)
            {
                case NameMailUpper:
                    query += " and MailPermission=1";
                    break;
                case NameEmailUpper:
                    query += " AND OtherProductsPermission=1 and EmailRenewPermission=1 and EmailExists=1";
                    break;
                case NamePhoneUpper:
                    query += " and PhonePermission=1 and PhoneExists=1";
                    break;
                case NameMailPhoneUpper:
                    query += " and MailPermission=1 and PhonePermission=1 and PhoneExists=1";
                    break;
                case NameEmailPhoneUpper:
                    query += " and PhonePermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and EmailExists=1 and PhoneExists=1";
                    break;
                case NameMailEmailUpper:
                    query += " and MailPermission = 1 and OtherProductsPermission=1 and EmailRenewPermission=1";
                    break;
                case NameAllRecordsUpper:
                    query += " and MailPermission=1 and PhonePermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and EmailExists=1 and PhoneExists=1";
                    break;
            }

            query += ") x5 on x5.SubscriptionID = s.SubscriptionID";

            return query;
        }

        private string GetPermissionFiltersByMasterId()
        {
            string query;
            int brandId;
            int.TryParse(hfBrandID.Value, out brandId);

            switch (lblMasterID.Text)
            {
                case "0":
                    if (ViewType == Enums.ViewType.RecencyView)
                    {
                        if (brandId > 0)
                        {
                            query = string.Concat(" join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentBrandConsensus vrbc2 with (nolock) on ss.subscriptionid = vrbc2.subscriptionid and vrbc2.BrandID = ",
                                hfBrandID.Value,
                                " and vrbc2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where MasterGroupID = ",
                                hfReportFor.Value,
                                ") where  vrbc2.SubscriptionID is null");
                        }
                        else
                        {
                            query = string.Concat(" join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentConsensus vrc2 with (nolock) on ss.subscriptionid = vrc2.subscriptionid and vrc2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where MasterGroupID = ",
                                hfReportFor.Value,
                                ") where vrc2.SubscriptionID is null");
                        }
                    }
                    else
                    {
                        if (brandId > 0)
                        {
                            query = string.Concat(" join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_BrandConsensus v2 with (nolock) on ss.subscriptionid = v2.subscriptionid and v2.BrandID = ",
                                hfBrandID.Value,
                                " and v2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where MasterGroupID = ",
                                hfReportFor.Value,
                                ") where  v2.SubscriptionID is null");
                        }
                        else
                        {
                            query = string.Concat(" join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join subscribermastervalues smv with (nolock) on ss.SubscriptionID = smv.SubscriptionID  and smv.MasterGroupID = ",
                                hfReportFor.Value,
                                " where smv.MastercodesheetValues is null");
                        }
                    }

                    break;
                case "-1":
                    if (ViewType == Enums.ViewType.RecencyView)
                    {
                        if (brandId > 0)
                        {
                            query = string.Concat(" join (select distinct vrbc2.subscriptionID from Subscriptions ss with (nolock) join vw_RecentBrandConsensus vrbc2 on ss.SubscriptionID  = vrbc2.SubscriptionID join mastercodesheet m ON m.masterid = vrbc2.masterid WHERE vrbc2.BrandID = ",
                                $"{hfBrandID.Value} and m.MasterGroupID = {hfReportFor.Value}");
                        }
                        else
                        {
                            query = string.Concat(" join (select distinct vrc2.subscriptionID from Subscriptions ss with (nolock) join vw_RecentConsensus vrc2 on ss.SubscriptionID  = vrc2.SubscriptionID where vrc2.mastergroupid = ",
                                hfReportFor.Value);
                        }

                    }
                    else
                    {
                        if (brandId > 0)
                        {
                            query = string.Concat(" join (select distinct v2.subscriptionID from Subscriptions ss with (nolock) join vw_BrandConsensus v2 on ss.SubscriptionID  = v2.SubscriptionID join mastercodesheet m ON m.masterid = v2.masterid WHERE v2.BrandID = ",
                                $"{hfBrandID.Value} and m.MasterGroupID = {hfReportFor.Value}");
                        }
                        else
                        {
                            query = string.Concat(" join (select distinct sd2.subscriptionID from Subscriptions ss with (nolock) join SubscriptionDetails sd2 WITH (nolock) on ss.SubscriptionID  = sd2.SubscriptionID join mastercodesheet m with (nolock) on m.masterid = sd2.masterid  where m.mastergroupid = ",
                                hfReportFor.Value);
                        }
                    }

                    break;
                default:
                    query = GetPermissionFiltersByMasterIdDefault(brandId);
                    break;
            }

            return query;
        }

        private string GetPermissionFiltersByMasterIdDefault(int brandId)
        {
            string query;

            if (ViewType == Enums.ViewType.RecencyView)
            {
                if (brandId > 0)
                {
                    query = string.Concat(" join (select distinct vrbc2.subscriptionID from Subscriptions ss with (nolock) join vw_RecentBrandConsensus vrbc2 on ss.SubscriptionID  = vrbc2.SubscriptionID where vrbc2.BrandID = ",
                        $"{hfBrandID.Value} and vrbc2.masterid in ({lblMasterID.Text})");
                }
                else
                {
                    query = string.Concat(" join (select distinct vrc2.subscriptionID from Subscriptions ss with (nolock) join vw_RecentConsensus vrc2  on ss.SubscriptionID  = vrc2.SubscriptionID where vrc2.masterid in (",
                        lblMasterID.Text,
                        ")");
                }
            }
            else
            {
                if (brandId > 0)
                {
                    query = string.Concat(" join (select distinct v2.subscriptionID from Subscriptions ss with (nolock) join vw_BrandConsensus v2 on ss.SubscriptionID  = v2.SubscriptionID where v2.BrandID = ",
                        $"{hfBrandID.Value} and v2.masterid in ({lblMasterID.Text})");
                }
                else
                {
                    query = string.Concat(" join (select distinct sd2.subscriptionID from Subscriptions ss with (nolock) join SubscriptionDetails sd2 with (nolock) on ss.SubscriptionID  = sd2.SubscriptionID where sd2.masterid in (",
                        lblMasterID.Text,
                        ")");
                }
            }

            return query;
        }

        private string GetCrossTabFilters(string query)
        {
            var args = hfSelectedCrossTabLink.Value.Split(VBarChar);
            if (args.Length < 2)
            {
                throw new InvalidOperationException("Unable get enough CrossTab link values.");
            }

            query = GetCrossTabRowQuery(query, args[0]);
            query = GetCrossTabColumnQuery(query, args[1]);

            return query;
        }

        private string GetCrossTabRowQuery(string query, string crossTab)
        {
            var rowSearch = false;

            switch (hfRow.Value)
            {
                case NameCompany:
                case NameTitle:
                case NameCity:
                case NameState:
                case NameZip:
                case NameCountry:
                    if (crossTab.Equals(NameGrandTotal, StringComparison.OrdinalIgnoreCase))
                    {
                        query = GetCrossTabRowGeoGrandTotalQuery(query, ref rowSearch);
                    }
                    else if (crossTab.Equals(NameNoResponse, StringComparison.OrdinalIgnoreCase))
                    {
                        query = GetCrossTabRowGeoNoResponseQuery(query, ref rowSearch);
                    }
                    else
                    {
                        if (hfRow.Value.Equals(NameCountry, StringComparison.OrdinalIgnoreCase))
                        {
                            query = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID where ct.ShortName = '" +
                                crossTab + "'";
                        }
                        else
                        {
                            query = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) where ss." + hfRow.Value + " = '" +
                                    crossTab + "'";
                        }
                    }
                    break;
                default:
                    query = GetCrossTabRowDefaultQuery(query, crossTab);
                    break;
            }

            if (rowSearch)
            {
                query += ") x6";
            }
            else
            {
                query += ") x6 on x6.SubscriptionID = s.SubscriptionID";
            }

            return query;
        }

        private string GetCrossTabRowGeoGrandTotalQuery(string query, ref bool rowSearch)
        {
            if (hfRow.Value.Equals(NameZip, StringComparison.OrdinalIgnoreCase))
            {
                var zipRange = hfRowSearchValue.Value.Split(VBarChar);

                query = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) where substring(ss." + hfRow.Value +
                        ",1,5) between '" + zipRange[0] + "' and '" + zipRange[1] + "'";
                query += " union select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) where substring(ss." +
                    hfRow.Value + ",1,5) between '" + zipRange[0] + "' and '" + zipRange[1] + "')";
            }
            else if (hfRow.Value.Equals(NameState, StringComparison.OrdinalIgnoreCase))
            {
                query = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES'";
                query += " union select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES')";
            }
            else if (hfRow.Value.Equals(NameCountry, StringComparison.OrdinalIgnoreCase))
            {
                query = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID";
                query += " union select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID)";
            }
            else
            {
                rowSearch = true;

                if (hfRowSearchValue.Value == string.Empty)
                {
                    query += " CROSS APPLY (SELECT TOP 100 " + hfRow.Value +
                             ", SubscriptionID from subscriptions WITH (nolock) where subscriptionID = s.SubscriptionID GROUP BY " +
                             hfRow.Value + " , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                    query += " union SELECT TOP 100 " + hfRow.Value +
                             ", SubscriptionID from subscriptions WITH (nolock) where subscriptionID = s.SubscriptionID and " +
                             hfRow.Value + " is null GROUP BY " + hfRow.Value + " , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                }
                else
                {
                    var strRowSearchValue = hfRowSearchValue.Value.Split(CommaChar);
                    var whereCondition = "(";

                    for (var i = 0; i < strRowSearchValue.Length; i++)
                    {
                        var escaped = strRowSearchValue[i].Replace("_", "[_]");
                        whereCondition += $"{(i > 0 ? " OR " : string.Empty)}{hfRow.Value} like '%{escaped}%'";
                    }

                    whereCondition += ")";
                    query += " CROSS APPLY (SELECT TOP 100 " + hfRow.Value +
                             ", SubscriptionID from subscriptions WITH (nolock) where subscriptionID = s.SubscriptionID and " +
                             whereCondition + " GROUP BY " + hfRow.Value + " , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";

                    var notWhereCondition = "(";

                    for (var i = 0; i < strRowSearchValue.Length; i++)
                    {
                        var escaped = strRowSearchValue[i].Replace("_", "[_]");
                        notWhereCondition += $"{(i > 0 ? " OR " : string.Empty)}{hfRow.Value} not like '%{escaped}%'";
                    }

                    notWhereCondition += ")";
                    query += " union SELECT TOP 100 " + hfRow.Value +
                             ", SubscriptionID from subscriptions WITH (nolock) where subscriptionID = s.SubscriptionID and " +
                             notWhereCondition + " GROUP BY " + hfRow.Value + " , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                }
            }

            return query;
        }

        private string GetCrossTabRowGeoNoResponseQuery(string query, ref bool rowSearch)
        {
            if (hfRow.Value.Equals(NameZip, StringComparison.OrdinalIgnoreCase))
            {
                var zipRange = hfRowSearchValue.Value.Split(VBarChar);
                query = " join (select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) where substring(ss." +
                    hfRow.Value + ",1,5) between '" + zipRange[0] + "' and '" + zipRange[1] + "')";
            }
            else if (hfRow.Value.Equals(NameState, StringComparison.OrdinalIgnoreCase))
            {
                query = " join (select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES')";
            }
            else if (hfRow.Value.Equals(NameCountry, StringComparison.OrdinalIgnoreCase))
            {
                query = " join (select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID)";
            }
            else
            {
                rowSearch = true;

                if (hfRowSearchValue.Value == string.Empty)
                {
                    query += " CROSS APPLY (SELECT TOP 100 " + hfRow.Value +
                             ", SubscriptionID from subscriptions WITH (nolock) where subscriptionID = s.SubscriptionID and " +
                             hfRow.Value + " is null GROUP BY " + hfRow.Value + " , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                }
                else
                {
                    var strRowSearchValue = hfRowSearchValue.Value.Split(CommaChar);
                    var whereCondition = "(";

                    for (var i = 0; i < strRowSearchValue.Length; i++)
                    {
                        var escaped = strRowSearchValue[i].Replace("_", "[_]");
                        whereCondition += $"{(i > 0 ? " OR " : "")}{hfRow.Value} not like '%{escaped}%'";
                    }

                    whereCondition += ")";

                    query += " CROSS APPLY (SELECT TOP 100 " + hfRow.Value +
                             ", SubscriptionID from subscriptions WITH (nolock) where subscriptionID = s.SubscriptionID and " +
                             whereCondition + " GROUP BY " + hfRow.Value + " , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                }
            }

            return query;
        }

        private string GetCrossTabRowDefaultQuery(string query, string crossTab)
        {
            int brandId;
            int.TryParse(hfBrandID.Value, out brandId);
            if (crossTab.Equals(NameNoResponse, StringComparison.OrdinalIgnoreCase))
            {
                query = GetCrossTabRowDefaultNoResponseQuery(brandId);
            }
            else
            {
                if (ViewType == Enums.ViewType.RecencyView)
                {
                    if (brandId > 0)
                    {
                        if (crossTab.Equals(NameGrandTotal, StringComparison.OrdinalIgnoreCase))
                        {
                            query += " join (select distinct vrbc2.subscriptionID from Subscriptions ss with (nolock) join vw_RecentBrandConsensus vrbc2 on ss.SubscriptionID  = vrbc2.SubscriptionID join mastercodesheet m ON m.masterid = vrbc2.masterid WHERE vrbc2.BrandID = " +
                                hfBrandID.Value + " and m.MasterGroupID = " + hfRow.Value;
                            query += " union select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentBrandConsensus vrbc2 with (nolock) on ss.subscriptionid = vrbc2.subscriptionid and vrbc2.BrandID = " +
                                hfBrandID.Value +
                                " and vrbc2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where  MasterGroupID = " +
                                hfRow.Value + ") where  vrbc2.SubscriptionID is null";
                        }
                        else
                        {
                            query += " join (select distinct vrbc2.subscriptionID from Subscriptions ss with (nolock) join vw_RecentBrandConsensus vrbc2 on ss.SubscriptionID  = vrbc2.SubscriptionID join mastercodesheet m ON m.masterid = vrbc2.masterid WHERE vrbc2.BrandID = " +
                                hfBrandID.Value + " and m.MasterDesc = '" + crossTab + "' and m.MasterGroupID = " + hfRow.Value;
                        }
                    }
                    else
                    {
                        if (crossTab.Equals(NameGrandTotal, StringComparison.OrdinalIgnoreCase))
                        {
                            query += " join (select distinct vrc2.subscriptionID from Subscriptions ss with (nolock) join vw_RecentConsensus vrc2 on ss.SubscriptionID  = vrc2.SubscriptionID join mastercodesheet m ON m.masterid = vrc2.masterid where m.MasterGroupID = " +
                                hfRow.Value;
                            query += " union select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentConsensus vrc2 with (nolock) on ss.subscriptionid = vrc2.subscriptionid and vrc2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where  MasterGroupID = " +
                                hfRow.Value + ") where vrc2.SubscriptionID is null";
                        }
                        else
                        {
                            query += " join (select distinct vrc2.subscriptionID from Subscriptions ss with (nolock) join vw_RecentConsensus vrc2 on ss.SubscriptionID  = vrc2.SubscriptionID join mastercodesheet m ON m.masterid = vrc2.masterid where m.MasterDesc = '" +
                                crossTab + "' and m.MasterGroupID = " + hfRow.Value;
                        }
                    }
                }
                else
                {
                    if (brandId > 0)
                    {
                        if (crossTab.Equals(NameGrandTotal, StringComparison.OrdinalIgnoreCase))
                        {
                            query += " join (select distinct v2.subscriptionID from Subscriptions ss with (nolock) join vw_BrandConsensus v2 on ss.SubscriptionID  = v2.SubscriptionID join mastercodesheet m ON m.masterid = v2.masterid WHERE v2.BrandID = " +
                                hfBrandID.Value + " and m.MasterGroupID = " + hfRow.Value;
                            query += " union select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_BrandConsensus v2 with (nolock) on ss.subscriptionid = v2.subscriptionid and v2.BrandID = " +
                                hfBrandID.Value +
                                " and v2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where MasterGroupID = " +
                                hfRow.Value + ") where  v2.SubscriptionID is null";
                        }
                        else
                        {
                            query += " join (select distinct v2.subscriptionID from Subscriptions ss with (nolock) join vw_BrandConsensus v2 on ss.SubscriptionID  = v2.SubscriptionID join mastercodesheet m ON m.masterid = v2.masterid WHERE v2.BrandID = " +
                                hfBrandID.Value + " and m.MasterDesc = '" + crossTab + "' and m.MasterGroupID = " + hfRow.Value;
                        }
                    }
                    else
                    {
                        if (crossTab.Equals(NameGrandTotal, StringComparison.OrdinalIgnoreCase))
                        {
                            query += " join (select distinct sd2.subscriptionID from SubscriptionDetails sd2 WITH (nolock) join mastercodesheet m with (nolock) on m.masterid = sd2.masterid  where m.MasterGroupID = " +
                                hfRow.Value;
                            query += " union select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct sd2.subscriptionID from Subscriptions ss with (nolock) join SubscriptionDetails sd2 WITH (nolock) on ss.SubscriptionID  = sd2.SubscriptionID join mastercodesheet m with (nolock) on m.masterid = sd2.masterid  where m.MasterGroupID = " +
                                hfRow.Value + ")";
                        }
                        else
                        {
                            query += " join (select distinct sd2.subscriptionID from SubscriptionDetails sd2 WITH (nolock) join mastercodesheet m with (nolock) on m.masterid = sd2.masterid  where m.MasterDesc = '" +
                                crossTab + "' and m.MasterGroupID = " + hfRow.Value;
                        }
                    }
                }
            }

            return query;
        }

        private string GetCrossTabRowDefaultNoResponseQuery(int brandId)
        {
            string query;
            var whereConditionDesc = string.Empty;
            if (hfRowSearchValue.Value != string.Empty)
            {
                var strRowSearchValue = hfRowSearchValue.Value.Split(CommaChar);
                whereConditionDesc = "(";

                for (var i = 0; i < strRowSearchValue.Length; i++)
                {
                    var escaped = strRowSearchValue[i].Replace("_", "[_]");
                    whereConditionDesc += $"{(i > 0 ? " OR " : string.Empty)} masterdesc  like '%{escaped}%'";
                }

                whereConditionDesc += ")";
            }

            if (ViewType == Enums.ViewType.RecencyView)
            {
                if (brandId > 0)
                {
                    if (hfRowSearchValue.Value == string.Empty)
                    {
                        query = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentBrandConsensus vrbc2 with (nolock) on ss.subscriptionid = vrbc2.subscriptionid and vrbc2.BrandID = " +
                            hfBrandID.Value +
                            " and vrbc2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where  MasterGroupID = " +
                            hfRow.Value + ") where  vrbc2.SubscriptionID is null";
                    }
                    else
                    {
                        query = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentBrandConsensus vrbc2 with (nolock) on ss.subscriptionid = vrbc2.subscriptionid and vrbc2.BrandID = " +
                            hfBrandID.Value + " and vrbc2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where " +
                            whereConditionDesc + ") where  vrbc2.SubscriptionID is null";
                    }
                }
                else
                {
                    if (hfRowSearchValue.Value == string.Empty)
                    {
                        query = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentConsensus vrc2 with (nolock) on ss.subscriptionid = vrc2.subscriptionid and vrc2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where  MasterGroupID = " +
                            hfRow.Value + ") where vrc2.SubscriptionID is null";
                    }
                    else
                    {
                        query = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentConsensus vrc2 with (nolock) on ss.subscriptionid = vrc2.subscriptionid and vrc2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where  " +
                            whereConditionDesc + ") where vrc2.SubscriptionID is null";
                    }
                }
            }
            else
            {
                if (brandId > 0)
                {
                    if (hfRowSearchValue.Value == string.Empty)
                    {
                        query = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_BrandConsensus v2 with (nolock) on ss.subscriptionid = v2.subscriptionid and v2.BrandID = " +
                            hfBrandID.Value +
                            " and v2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where MasterGroupID = " +
                            hfRow.Value + ") where  v2.SubscriptionID is null";
                    }
                    else
                    {
                        query = " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_BrandConsensus v2 with (nolock) on ss.subscriptionid = v2.subscriptionid and v2.BrandID = " +
                            hfBrandID.Value + " and v2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where  " +
                            whereConditionDesc + ") where  v2.SubscriptionID is null";
                    }
                }
                else
                {
                    if (hfRowSearchValue.Value == string.Empty)
                    {
                        query = " join (select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct sd2.subscriptionID from Subscriptions ss with (nolock) join SubscriptionDetails sd2 WITH (nolock) on ss.SubscriptionID  = sd2.SubscriptionID join mastercodesheet m with (nolock) on m.masterid = sd2.masterid  where m.MasterGroupID = " +
                            hfRow.Value + ")";
                    }
                    else
                    {
                        query = " join (select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct sd2.subscriptionID from Subscriptions ss with (nolock) join SubscriptionDetails sd2 WITH (nolock) on ss.SubscriptionID  = sd2.SubscriptionID join mastercodesheet m with (nolock) on m.masterid = sd2.masterid  where " +
                            whereConditionDesc + ")";
                    }
                }
            }

            return query;
        }

        private string GetCrossTabColumnQuery(string query, string crossTab)
        {
            var columnSearch = false;

            switch (hfColumn.Value)
            {
                case NameCompany:
                case NameTitle:
                case NameCity:
                case NameState:
                case NameZip:
                case NameCountry:
                    if (crossTab.Equals(NameGrandTotal, StringComparison.OrdinalIgnoreCase))
                    {
                        query = GetCrossTabColumnGeoGrandTotalQuery(query, ref columnSearch);
                    }
                    else if (crossTab.Equals(NameNoResponse, StringComparison.OrdinalIgnoreCase))
                    {
                        query = GetCrossTabColumnGeoNoResponseQuery(query, ref columnSearch);
                    }
                    else
                    {
                        if (hfColumn.Value.Equals(NameCountry, StringComparison.OrdinalIgnoreCase))
                        {
                            query += " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID where ct.ShortName = '" +
                                crossTab + "'";
                        }
                        else
                        {
                            query += " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) where ss." + hfColumn.Value +
                                     " = '" + crossTab + "'";
                        }
                    }

                    break;
                default:
                    query = GetCrossTabColumnDefaultQuery(query, crossTab);
                    break;
            }

            if (columnSearch)
            {
                query += ") x7";
            }
            else
            {
                query += ") x7 on x7.SubscriptionID = s.SubscriptionID";
            }

            return query;
        }

        private string GetCrossTabColumnGeoGrandTotalQuery(string query, ref bool columnSearch)
        {
            if (hfColumn.Value.Equals(NameZip, StringComparison.OrdinalIgnoreCase))
            {
                var zipRange = hfColumnSearchValue.Value.Split(VBarChar);

                query += " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) where substring(ss." +
                         hfColumn.Value + ",1,5) between '" + zipRange[0] + "' and '" + zipRange[1] + "'";
                query += " union select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) where substring(ss." +
                    hfColumn.Value + ",1,5) between '" + zipRange[0] + "' and '" + zipRange[1] + "')";
            }
            else if (hfColumn.Value.Equals(NameState, StringComparison.OrdinalIgnoreCase))
            {
                query += " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES'";
                query += " union select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES')";
            }
            else if (hfColumn.Value.Equals(NameCountry, StringComparison.OrdinalIgnoreCase))
            {
                query += " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID";
                query += " union select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID)";
            }
            else
            {
                columnSearch = true;
                if (hfColumnSearchValue.Value == string.Empty)
                {
                    query += " CROSS APPLY (SELECT TOP 100 " + hfColumn.Value +
                             ", SubscriptionID from subscriptions WITH (nolock) where subscriptionID = s.SubscriptionID GROUP BY " +
                             hfColumn.Value + " , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                    query += " union SELECT TOP 100 " + hfColumn.Value +
                             ", SubscriptionID from subscriptions WITH (nolock) where subscriptionID = s.SubscriptionID and " +
                             hfColumn.Value + " is null GROUP BY " + hfColumn.Value +
                             " , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                }
                else
                {
                    var strColumnSearchValue = hfColumnSearchValue.Value.Split(CommaChar);
                    var whereCondition = "(";

                    for (var i = 0; i < strColumnSearchValue.Length; i++)
                    {
                        var escaped = strColumnSearchValue[i].Replace("_", "[_]");
                        whereCondition += $"{(i > 0 ? " OR " : "")}{hfColumn.Value} like '%{escaped}%'";
                    }

                    whereCondition += ")";
                    query += " CROSS APPLY (SELECT TOP 100 " + hfColumn.Value +
                             ", SubscriptionID from subscriptions WITH (nolock) where subscriptionID = s.SubscriptionID and " +
                             whereCondition + " GROUP BY " + hfColumn.Value + " , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";

                    var notWhereCondition = "(";

                    for (var i = 0; i < strColumnSearchValue.Length; i++)
                    {
                        var escaped = strColumnSearchValue[i].Replace("_", "[_]");
                        notWhereCondition += $"{(i > 0 ? " OR " : "")}{hfColumn.Value} not like \'%{escaped}%\'";
                    }

                    notWhereCondition += ")";
                    query += " union SELECT TOP 100 " + hfColumn.Value +
                             ", SubscriptionID from subscriptions WITH (nolock) where subscriptionID = s.SubscriptionID and " +
                             notWhereCondition + " GROUP BY " + hfColumn.Value +
                             " , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                }
            }

            return query;
        }

        private string GetCrossTabColumnGeoNoResponseQuery(string query, ref bool columnSearch)
        {
            if (hfColumn.Value.Equals(NameZip, StringComparison.OrdinalIgnoreCase))
            {
                var zipRange = hfColumnSearchValue.Value.Split(VBarChar);
                query += " join (select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) where substring(ss." +
                    hfColumn.Value + ",1,5) between '" + zipRange[0] + "' and '" + zipRange[1] + "')";
            }
            else if (hfColumn.Value.Equals(NameState, StringComparison.OrdinalIgnoreCase))
            {
                query += " join (select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES')";
            }
            else if (hfColumn.Value.Equals("Country", StringComparison.OrdinalIgnoreCase))
            {
                query += " join (select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ss.CountryID)";
            }
            else
            {
                columnSearch = true;
                if (hfColumnSearchValue.Value == string.Empty)
                {
                    query += " CROSS APPLY (SELECT TOP 100 " + hfColumn.Value +
                             ", SubscriptionID from subscriptions WITH (nolock) where subscriptionID = s.SubscriptionID and " +
                             hfColumn.Value + " is null GROUP BY " + hfColumn.Value +
                             " , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                }
                else
                {
                    var strColumnSearchValue = hfColumnSearchValue.Value.Split(CommaChar);
                    var whereCondition = "(";

                    for (var i = 0; i < strColumnSearchValue.Length; i++)
                    {
                        var escaped = strColumnSearchValue[i].Replace("_", "[_]");
                        whereCondition += $"{(i > 0 ? " OR " : "")}{hfColumn.Value} not like '%{escaped}%'";
                    }

                    whereCondition += ")";
                    query += " CROSS APPLY (SELECT TOP 100 " + hfColumn.Value +
                             ", SubscriptionID from subscriptions WITH (nolock) where subscriptionID = s.SubscriptionID and " +
                             whereCondition + " GROUP BY " + hfColumn.Value + " , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                }
            }

            return query;
        }

        private string GetCrossTabColumnDefaultQuery(string query, string crossTab)
        {
            int brandId;
            int.TryParse(hfBrandID.Value, out brandId);
            if (crossTab.Equals(NameNoResponse, StringComparison.OrdinalIgnoreCase))
            {
                query = GetCrossTabColumnDefaultNoResponseQuery(query, brandId);
            }
            else
            {
                if (ViewType == Enums.ViewType.RecencyView)
                {
                    if (brandId > 0)
                    {
                        if (crossTab.Equals(NameGrandTotal, StringComparison.OrdinalIgnoreCase))
                        {
                            query += " join (select distinct vrbc2.subscriptionID from Subscriptions ss with (nolock) join vw_RecentBrandConsensus vrbc2 on ss.SubscriptionID  = vrbc2.SubscriptionID join mastercodesheet m ON m.masterid = vrbc2.masterid WHERE vrbc2.BrandID = " +
                                hfBrandID.Value + " and m.MasterGroupID = " + hfColumn.Value;
                            query += " union select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentBrandConsensus vrbc2 with (nolock) on ss.subscriptionid = vrbc2.subscriptionid and vrbc2.BrandID = " +
                                hfBrandID.Value +
                                " and vrbc2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where  MasterGroupID = " +
                                hfColumn.Value + ") where  vrbc2.SubscriptionID is null";
                        }
                        else
                        {
                            query += " join (select distinct vrbc2.subscriptionID from Subscriptions ss with (nolock) join vw_RecentBrandConsensus vrbc2 on ss.SubscriptionID  = vrbc2.SubscriptionID join mastercodesheet m ON m.masterid = vrbc2.masterid WHERE vrbc2.BrandID = " +
                                hfBrandID.Value + " and m.MasterDesc = '" + crossTab + "' and m.MasterGroupID = " + hfColumn.Value;
                        }
                    }
                    else
                    {
                        if (crossTab.Equals(NameGrandTotal, StringComparison.OrdinalIgnoreCase))
                        {
                            query += " join (select distinct vrc2.subscriptionID from Subscriptions ss with (nolock) join vw_RecentConsensus vrc2 on ss.SubscriptionID  = vrc2.SubscriptionID join mastercodesheet m ON m.masterid = vrc2.masterid where m.MasterGroupID = " +
                                hfColumn.Value;
                            query += " union select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentConsensus vrc2 with (nolock) on ss.subscriptionid = vrc2.subscriptionid and vrc2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where MasterGroupID = " +
                                hfColumn.Value + ") where vrc2.SubscriptionID is null";
                        }
                        else
                        {
                            query += " join (select distinct vrc2.subscriptionID from Subscriptions ss with (nolock) join vw_RecentConsensus vrc2 on ss.SubscriptionID  = vrc2.SubscriptionID join mastercodesheet m ON m.masterid = vrc2.masterid where m.MasterDesc = '" +
                                crossTab + "' and m.MasterGroupID = " + hfColumn.Value;
                        }
                    }
                }
                else
                {
                    if (brandId > 0)
                    {
                        if (crossTab.Equals(NameGrandTotal, StringComparison.OrdinalIgnoreCase))
                        {
                            query += " join (select distinct v2.subscriptionID from Subscriptions ss with (nolock) join vw_BrandConsensus v2 on ss.SubscriptionID  = v2.SubscriptionID join mastercodesheet m ON m.masterid = v2.masterid WHERE v2.BrandID = " +
                                hfBrandID.Value + " and m.MasterGroupID = " + hfColumn.Value;
                            query += " union select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_BrandConsensus v2 with (nolock) on ss.subscriptionid = v2.subscriptionid and v2.BrandID = " +
                                hfBrandID.Value +
                                " and v2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where MasterGroupID = " +
                                hfColumn.Value + ") where  v2.SubscriptionID is null";
                        }
                        else
                        {
                            query += " join (select distinct v2.subscriptionID from Subscriptions ss with (nolock) join vw_BrandConsensus v2 on ss.SubscriptionID  = v2.SubscriptionID join mastercodesheet m ON m.masterid = v2.masterid WHERE v2.BrandID = " +
                                hfBrandID.Value + " and m.MasterDesc = '" + crossTab + "' and m.MasterGroupID = " + hfColumn.Value;
                        }
                    }
                    else
                    {
                        if (crossTab.Equals(NameGrandTotal, StringComparison.OrdinalIgnoreCase))
                        {
                            query += " join (select distinct sd2.subscriptionID from SubscriptionDetails sd2 WITH (nolock) join mastercodesheet m with (nolock) on m.masterid = sd2.masterid  where m.MasterGroupID = " +
                                hfColumn.Value;
                            query += " union select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct sd2.subscriptionID from Subscriptions ss with (nolock) join SubscriptionDetails sd2 WITH (nolock) on ss.SubscriptionID  = sd2.SubscriptionID join mastercodesheet m with (nolock) on m.masterid = sd2.masterid  where m.MasterGroupID = " +
                                hfColumn.Value + ")";
                        }
                        else
                        {
                            query += " join (select distinct sd2.subscriptionID from SubscriptionDetails sd2 WITH (nolock) join mastercodesheet m with (nolock) on m.masterid = sd2.masterid  where m.MasterDesc = '" +
                                crossTab + "' and m.MasterGroupID = " + hfColumn.Value;
                        }
                    }
                }
            }

            return query;
        }

        private string GetCrossTabColumnDefaultNoResponseQuery(string query, int brandId)
        {
            var whereConditionDesc = string.Empty;
            if (hfColumnSearchValue.Value != string.Empty)
            {
                var strColumnSearchValue = hfColumnSearchValue.Value.Split(CommaChar);
                whereConditionDesc = "(";

                for (var i = 0; i < strColumnSearchValue.Length; i++)
                {
                    var escaped = strColumnSearchValue[i].Replace("_", "[_]");
                    whereConditionDesc += $"{(i > 0 ? " OR " : "")} masterdesc  like '%{escaped}%'";
                }

                whereConditionDesc += ")";
            }

            if (ViewType == Enums.ViewType.RecencyView)
            {
                if (brandId > 0)
                {
                    if (hfColumnSearchValue.Value == string.Empty)
                    {
                        query += " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentBrandConsensus vrbc2 with (nolock) on ss.subscriptionid = vrbc2.subscriptionid and vrbc2.BrandID = " +
                            hfBrandID.Value +
                            " and vrbc2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where  MasterGroupID = " +
                            hfColumn.Value + ") where  vrbc2.SubscriptionID is null";
                    }
                    else
                    {
                        query += " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentBrandConsensus vrbc2 with (nolock) on ss.subscriptionid = vrbc2.subscriptionid and vrbc2.BrandID = " +
                            hfBrandID.Value + " and vrbc2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where  " +
                            whereConditionDesc + ") where  vrbc2.SubscriptionID is null";
                    }
                }
                else
                {
                    if (hfColumnSearchValue.Value == string.Empty)
                    {
                        query += " join (selct distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentConsensus vrc2 with (nolock) on ss.subscriptionid = vrc2.subscriptionid and vrc2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where MasterGroupID = " +
                            hfColumn.Value + ") where vrc2.SubscriptionID is null";
                    }
                    else
                    {
                        query += " join (selct distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_RecentConsensus vrc2 with (nolock) on ss.subscriptionid = vrc2.subscriptionid and vrc2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where " +
                            whereConditionDesc + ") where vrc2.SubscriptionID is null";
                    }
                }
            }
            else
            {
                if (brandId > 0)
                {
                    if (hfColumnSearchValue.Value == string.Empty)
                    {
                        query += " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_BrandConsensus v2 with (nolock) on ss.subscriptionid = v2.subscriptionid and v2.BrandID = " +
                            hfBrandID.Value +
                            " and v2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where MasterGroupID = " +
                            hfColumn.Value + ") where  v2.SubscriptionID is null";
                    }
                    else
                    {
                        query += " join (select distinct ss.subscriptionID from Subscriptions ss with (nolock) left outer join vw_BrandConsensus v2 with (nolock) on ss.subscriptionid = v2.subscriptionid and v2.BrandID = " +
                            hfBrandID.Value + " and v2.masterid in (select mc.MasterID from Mastercodesheet mc with (nolock) where " +
                            whereConditionDesc + ") where  v2.SubscriptionID is null";
                    }
                }
                else
                {
                    if (hfColumnSearchValue.Value == string.Empty)
                    {
                        query += " join (select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct sd2.subscriptionID from Subscriptions ss with (nolock) join SubscriptionDetails sd2 WITH (nolock) on ss.SubscriptionID  = sd2.SubscriptionID join mastercodesheet m with (nolock) on m.masterid = sd2.masterid  where m.MasterGroupID = " +
                            hfColumn.Value + ")";
                    }
                    else
                    {
                        query += " join (select SubscriptionID from Subscriptions with (nolock) where SubscriptionID not in (select distinct sd2.subscriptionID from Subscriptions ss with (nolock) join SubscriptionDetails sd2 WITH (nolock) on ss.SubscriptionID  = sd2.SubscriptionID join mastercodesheet m with (nolock) on m.masterid = sd2.masterid  where " +
                            whereConditionDesc + ")";
                    }
                }
            }

            return query;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Master.Menu = NameAudienceViews;

                if (ViewType == Enums.ViewType.ConsensusView)
                {
                    Master.SubMenu = NameConsensus;
                    RedirectIfNoViewAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.ServiceFeatures.ConsensusView);
                }
                else
                {
                    Master.SubMenu = NameRecency;
                    RedirectIfNoViewAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.ServiceFeatures.RecencyView);
                }

                InitializeCommonControls();

                if (!IsPostBack)
                {
                    InitializeForHttpGet(NameValue, ViewType);
                    FilterSave.Mode = NameAddNew;
                }
                else
                {
                    InitializeForPostBack(grdFilters);
                }
            }
            catch (Exception ex)
            {
                Utilities.Log_Error(Request.RawUrl, "Page_Load", ex);
                DisplayError(ex.Message);
            }
        }

        private void InitializeCommonControls()
        {
            rpgCrossTab.EnableAjaxSkinRendering = true;

            SetDelegatesAndCommands();

            lblErrorMsg.Text = string.Empty;
            divErrorMsg.Visible = false;
            lblSaveCampaignPopupError.Text = string.Empty;
            divSaveCampaignPopupError.Visible = false;

            fc = FilterCollection;
        }

        private void SetDelegatesAndCommands()
        {
            DownloadPanel1.DelMethod = new RebuildSubscriberList(Reload);
            DownloadPanel1.hideDownloadPopup = new HidePanel(DownloadPopupHide);
            AdhocFilter.hideAdhocPopup = new HidePanel(AdhocPopupHide);
            ActivityFilter.hideActivityPopup = new HidePanel(ActivityPopupHide);
            FilterSave.hideFilterSavePopup = new HidePanel(FilterSavePopupHide);
            FilterSave.LoadSavedFilterName = new LoadSavedFilterName(LoadFilterName);
            ShowFilter.hideShowFilterPopup = new HidePanel(ShowFilterPopupHide);
            CirculationFilter.hideCirculationPopup = new HidePanel(CirculationPopupHide);
            ShowFilter.LoadSelectedFilterData = new LoadSelectedFilterData(LoadFilterData);
            ShowFilter.LoadSelectedFilterSegmentationData = new LoadSelectedFilterSegmentationData(LoadFilterSegmentationData);
            CLDownloadPanel.DelMethod = new RebuildSubscriberList(CLReload);
            CLDownloadPanel.HideDownloadPopup = new HidePanel(CLDownloadPopupHide);
            EVDownloadPanel.DelMethod = new RebuildSubscriberList(EVReload);
            EVDownloadPanel.HideDownloadPopup = new HidePanel(EVDownloadPopupHide);
            DataCompareSummary.hideDataCompareSummaryPopup = new HidePanel(DataCompareSummaryPopupHide);

            FilterSegmentation.lnkCountCommand += lnkCount_Command;
            FilterSegmentation.lnkCompanyLocationViewCommand += lnkCompanyLocationView_Command;
            FilterSegmentation.lnkCrossTabReportCommand += lnkCrossTabReport_Command;
            FilterSegmentation.lnkDimensionReportCommand += lnkDimensionReport_Command;
            FilterSegmentation.lnkEmailViewCommand += lnkEmailView_Command;
            FilterSegmentation.lnkGeoMapsCommand += lnkGeoMaps_Command;
            FilterSegmentation.lnkGeoReportCommand += lnkGeoReport_Command;
        }

        protected override void InitializeDataBinding()
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD,
                KMPlatform.Enums.ServiceFeatures.DataCompare, KMPlatform.Enums.Access.Yes))
            {
                var sourceFiles = new UasWorkers.SourceFile().Select(Master.UserSession.ClientID, false);
                var dataCompareRuns = new UasWorkers.DataCompareRun().SelectForClient(Master.UserSession.ClientID);

                var query = (from s in sourceFiles
                        join d in dataCompareRuns on s.SourceFileID equals d.SourceFileId
                        orderby d.DateCreated
                        select new {FileName = s.FileName + "_" + d.DateCreated, d.ProcessCode})
                    .ToList();

                drpDataCompareSourceFile.DataSource = query;
                drpDataCompareSourceFile.DataBind();
                drpDataCompareSourceFile.Items.Insert(0, new ListItem(string.Empty, string.Empty));

                var codeList = new UadLookupWorkers.Code().Select(UadLookupEnums.CodeType.Data_Compare_Type);
                codeList.Where(w => w.CodeName == UadLookupEnums.DataCompareType.Match.ToString())
                    .ToList()
                    .ForEach(i => i.CodeName = NameStandard);

                rblDataCompareOperation.DataSource = codeList;
                rblDataCompareOperation.DataBind();

                try
                {
                    rblDataCompareOperation.SelectedValue = new UadLookupWorkers.Code()
                        .SelectCodeName(UadLookupEnums.CodeType.Data_Compare_Type, UadLookupEnums.DataCompareType.Match.ToString())
                        .CodeId.ToString();
                }
                catch (Exception ex)
                {
                    Utilities.Log_Error(Request.RawUrl, "InitializeDataBinding", ex);
                    DisplayError(ex.Message);
                }

                pnlDataCompare.Visible = true;
            }
        }

        private void DisplayError(string errorMessage)
        {
            lblErrorMsg.Text = errorMessage;
            divErrorMsg.Visible = true;
            //txtShowQuery.Text = txtShowQuery.Text + "\r\n" + "\r\n" + ex.Message.ToString() + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.InnerException;
        }

        private void DisplaySaveCampaignPopupError(string errorMessage)
        {
            lblSaveCampaignPopupError.Text = errorMessage;
            divSaveCampaignPopupError.Visible = true;
        }

        protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc.Clear();
            FilterCollection.Clear();
            ResetFilterControls();
            LoadGridFilters();

            imglogo.ImageUrl = string.Empty;
            imglogo.Visible = false;
            hfBrandID.Value = drpBrand.SelectedValue;
            hfBrandName.Value = drpBrand.SelectedItem.Text;
            dlDimensions.DataSource = null;
            dlDimensions.DataBind();
            PubTypeRepeater.DataSource = null;
            PubTypeRepeater.DataBind();
            pnlMarket.Visible = false;
            lstMarket.Items.Clear();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
            {
                Brand b = Brand.GetByID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                if (b != null)
                {
                    if (b.Logo != string.Empty)
                    {
                        int customerID = Master.UserSession.CustomerID;
                        imglogo.ImageUrl = "../Images/logo/" + customerID + "/" + b.Logo;
                        imglogo.Visible = true;
                    }
                }
            }

            if (Convert.ToInt32(hfBrandID.Value) >= 0)
            {
                loadProductandDimensions();
                lnkActivity.Enabled = true;
                lnkAdhoc.Enabled = true;
                AdhocFilter.BrandID = Convert.ToInt32(hfBrandID.Value);
                AdhocFilter.LoadAdhocGrid();
            }
            else
            {
                lnkActivity.Enabled = false;
                lnkAdhoc.Enabled = false;
                lnkCirculation.Enabled = false;
            }

            DownloadPanel1.BrandID = Convert.ToInt32(hfBrandID.Value);
            DownloadPanel1.ViewType = ViewType;
            CLDownloadPanel.BrandID = Convert.ToInt32(hfBrandID.Value);
            CLDownloadPanel.ViewType = ViewType;
            EVDownloadPanel.BrandID = Convert.ToInt32(hfBrandID.Value);
            EVDownloadPanel.ViewType = ViewType;

            drpDataCompareSourceFile.SelectedIndex = -1;
            hfDataCompareProcessCode.Value = string.Empty;
            displayDataCampareData();
        }

        protected void drpDataCompareSourceFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfDataCompareProcessCode.Value = drpDataCompareSourceFile.SelectedValue;

            if (drpDataCompareSourceFile.SelectedValue != "" && (Convert.ToBoolean(drpIsBillable.SelectedValue) == true || plKmStaff.Visible == false))
                btnDCDownload.OnClientClick = "return confirmPopupPurchase();";
            else
                btnDCDownload.OnClientClick = null;

            displayDataCampareData();
        }

        private void displayDataCampareData()
        {
            ResetDataCompareControls();

            if (hfDataCompareProcessCode.Value != "")
            {
                int targetCodeID = 0;
                int MatchedConsensusRecordCount = 0;
                int MatchedBrandRecordCount = 0;

                List<Code> c = Code.GetDataCompareTarget();
                targetCodeID = c.Find(x => x.CodeName == Enums.DataCompareViewType.Consensus.ToString()).CodeID;

                MatchedConsensusRecordCount = new FrameworkUAD.BusinessLogic.DataCompareProfile().GetDataCompareCount(new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient), hfDataCompareProcessCode.Value, targetCodeID, 0);

                if (Convert.ToInt32(hfBrandID.Value) > 0)
                {
                    targetCodeID = c.Find(x => x.CodeName == Enums.DataCompareViewType.Brand.ToString()).CodeID;
                    hfDcTargetCodeID.Value = c.Find(x => x.CodeName == Enums.DataCompareViewType.Brand.ToString()).CodeID.ToString();
                    MatchedBrandRecordCount = new FrameworkUAD.BusinessLogic.DataCompareProfile().GetDataCompareCount(new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient), hfDataCompareProcessCode.Value, targetCodeID, Convert.ToInt32(hfBrandID.Value));
                    lblMatchedRecordHeader.Text = "Matched in Selected Brand : ";
                    lblNonMatchedRecordsHeader.Text = "Non Matched in Selected Brand : ";
                    lblMatchedNotInHeader.Text = "Matched Not in Selected Brand : ";
                    lblMatchedNotInHeader.Visible = true;
                }
                else
                {
                    hfDcTargetCodeID.Value = c.Find(x => x.CodeName == Enums.DataCompareViewType.Consensus.ToString()).CodeID.ToString();
                    lblMatchedRecordHeader.Text = "Matched : ";
                    lblNonMatchedRecordsHeader.Text = "Non Matched : ";
                    lblMatchedNotInHeader.Text = string.Empty;
                }

                FrameworkUAS.Entity.DataCompareRun dcr = new FrameworkUAS.BusinessLogic.DataCompareRun().SelectForClient(Master.UserSession.ClientID).Find(x => x.ProcessCode == hfDataCompareProcessCode.Value);
                hfDcRunID.Value = dcr.DcRunId.ToString();

                List<FrameworkUAD.Entity.DataCompareProfile> dcp = new FrameworkUAD.BusinessLogic.DataCompareProfile().Select(new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient), hfDataCompareProcessCode.Value);

                int TotalFileRecord = dcp.Count;

                lblTotalFileRecords.Text = TotalFileRecord.ToString();

                if (Convert.ToInt32(hfBrandID.Value) > 0)
                {
                    lnkMatchedRecords.Text = MatchedBrandRecordCount.ToString();
                    lnkNonMatchedRecords.Text = (TotalFileRecord - MatchedConsensusRecordCount).ToString();
                    lnkMatchedNotIn.Text = (MatchedConsensusRecordCount - MatchedBrandRecordCount).ToString();
                    lnkMatchedNotIn.Visible = true;
                    lnkMatchedNotInSummary.Visible = true;
                }
                else
                {
                    lnkMatchedRecords.Text = MatchedConsensusRecordCount.ToString();
                    lnkNonMatchedRecords.Text = (TotalFileRecord - MatchedConsensusRecordCount).ToString();
                    lnkMatchedNotIn.Text = string.Empty;
                }

                plDataCompareData.Visible = true;
            }
        }

        protected void lnkDataCompare_Command(object sender, CommandEventArgs e)
        {
            Guard.NotNull(e, nameof(e));
            var argText = e.CommandArgument.ToString();
            var dataTable = new DataTable();
            var queries = new StringBuilder();
            var brandId = GetInt32(hfBrandID.Value);
            var targetCodeId = GetInt32(hfDcTargetCodeID.Value);
            hfDataCompareLinkSelected.Value = e.CommandArgument.ToString();

            if (argText.EqualsIgnoreCase(NameMatchedRecords))
            {
                queries = GetMatchedRecordsQuery(brandId, targetCodeId, out dataTable);
            }
            else if (argText.EqualsIgnoreCase(NameNonMatchedRecords))
            {
                if (brandId > 0)
                {
                    dataTable = new DataCompareProfile().GetDataCompareData(
                        new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient), hfDataCompareProcessCode.Value,
                        targetCodeId,
                        NameNonMatched,
                        brandId);
                }
                else
                {
                    dataTable = new DataCompareProfile().GetDataCompareData(
                        new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient), hfDataCompareProcessCode.Value,
                        targetCodeId,
                        NameNonMatched);
                }
            }
            else if (argText.EqualsIgnoreCase(NameMatchedNotInBrand))
            {
                queries = GetMatchedNotInBrandQuery(brandId, targetCodeId, out dataTable);
            }

            UpdateDownloadControls(argText, dataTable, queries, brandId, targetCodeId);
        }

        private StringBuilder GetMatchedRecordsQuery(int brandId, int targetCodeId, out DataTable dataTable)
        {
            var queries = new StringBuilder();
            if (brandId > 0)
            {
                dataTable = new DataCompareProfile().GetDataCompareData(
                    new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient),
                    hfDataCompareProcessCode.Value,
                    targetCodeId,
                    NameMatched,
                    brandId);
                queries.Append("<xml><Queries>")
                    .AppendFormat(
                        "<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>",
                        DefaultFilterNo,
                        "select distinct 1, s.SubscriptionID " +
                        " from DataCompareProfile d with(nolock) " +
                        " join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No " +
                        " join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID " +
                        " join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  " +
                        $" where d.ProcessCode = '{hfDataCompareProcessCode.Value}' " +
                        $" and bd.BrandID = {brandId}")
                    .Append("</Queries><Results>")
                    .AppendFormat(
                        "<Result linenumber=\"{0}\"  selectedfilterno=\"{1}\" selectedfilteroperation=\"{2}\" suppressedfilterno=\"{3}\" suppressedfilteroperation=\"{4}\"  filterdescription=\"{5}\"></Result>",
                        DefaultFilterNo,
                        DefaultFilterNo,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty)
                    .Append("</Results></xml>");
            }
            else
            {
                dataTable = new DataCompareProfile().GetDataCompareData(
                    new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient),
                    hfDataCompareProcessCode.Value,
                    targetCodeId,
                    NameMatched);
                queries.Append("<xml><Queries>")
                    .AppendFormat(
                        "<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>",
                        DefaultFilterNo,
                        $"select distinct 1, s.SubscriptionID from DataCompareProfile d with(nolock) join Subscriptions s with (Nolock)on d.IGRP_NO = s.IGrp_No where d.ProcessCode = '{hfDataCompareProcessCode.Value}'")
                    .Append("</Queries><Results>")
                    .AppendFormat(
                        "<Result linenumber=\"{0}\"  selectedfilterno=\"{1}\" selectedfilteroperation=\"{2}\" suppressedfilterno=\"{3}\" suppressedfilteroperation=\"{4}\"  filterdescription=\"{5}\"></Result>",
                        DefaultFilterNo,
                        DefaultFilterNo,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty)
                    .Append("</Results></xml>");
            }

            return queries;
        }

        private StringBuilder GetMatchedNotInBrandQuery(int brandId, int targetCodeId, out DataTable dataTable)
        {
            var queries = new StringBuilder();
            dataTable = new DataCompareProfile().GetDataCompareData(
                new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient),
                hfDataCompareProcessCode.Value,
                targetCodeId,
                NameMatchedNotInBrand,
                brandId);
            queries.Append("<xml><Queries>")
                .AppendFormat(
                    "<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>",
                    DefaultFilterNo,
                    "select distinct 1, s.SubscriptionID " +
                    "from DataCompareProfile d with(nolock) " +
                    "join Subscriptions s with(Nolock) on d.IGRP_NO = s.IGrp_No " +
                    "left outer join( " +
                    "            select distinct s1.SubscriptionID " +
                    "            from DataCompareProfile d1 with(nolock) " +
                    "            join Subscriptions s1 with(Nolock) on d1.IGRP_NO = s1.IGrp_No " +
                    "            join PubSubscriptions ps with(nolock) on s1.SubscriptionID = ps.SubscriptionID " +
                    "            join branddetails bd with(nolock) on bd.pubID = ps.pubID " +
                    $"            where d1.ProcessCode = '{hfDataCompareProcessCode.Value}' " +
                    $"            and bd.BrandID = {brandId}" +
                    "        ) x on s.SubscriptionID = x.SubscriptionID " +
                    $"where d.ProcessCode = '{hfDataCompareProcessCode.Value}' and x.SubscriptionID is null ")
                .Append("</Queries><Results>")
                .AppendFormat(
                    "<Result linenumber=\"{0}\"  selectedfilterno=\"{1}\" selectedfilteroperation=\"{2}\" suppressedfilterno=\"{3}\" suppressedfilteroperation=\"{4}\"  filterdescription=\"{5}\"></Result>",
                    DefaultFilterNo,
                    DefaultFilterNo,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty)
                .Append("</Results></xml>");

            return queries;
        }

        private void UpdateDownloadControls(string argText, DataTable dataTable, StringBuilder queries, int brandId, int targetCodeId)
        {
            Guard.NotNull(argText, nameof(argText));
            Guard.NotNull(dataTable, nameof(dataTable));
            Guard.NotNull(queries, nameof(queries));

            if (dataTable.Rows.Count > 0 && argText.EqualsAnyIgnoreCase(NameMatchedRecords, NameMatchedNotInBrand))
            {
                DownloadPanel1.SubscriptionID = null;
                DownloadPanel1.SubscribersQueries = queries;
                DownloadPanel1.downloadCount = dataTable.Rows.Count;
                DownloadPanel1.Visible = true;
                DownloadPanel1.HeaderText = string.Empty;
                DownloadPanel1.ShowHeaderCheckBox = false;
                DownloadPanel1.PubIDs = null;
                DownloadPanel1.BrandID = brandId;
                DownloadPanel1.ViewType = ViewType;
                DownloadPanel1.VisibleCbIsRecentData = ViewType != Enums.ViewType.ConsensusView;
                DownloadPanel1.filterCombination = argText.EqualsIgnoreCase(NameMatchedRecords)
                    ? NameMatched
                    : NameMatchedNotInSelected;
                DownloadPanel1.dcRunID = GetInt32(hfDcRunID.Value);
                DownloadPanel1.dcTypeCodeID = new UadLookupWorkers.Code()
                    .SelectCodeName(UadLookupEnums.CodeType.Data_Compare_Type, UadLookupEnums.DataCompareType.Match.ToString())
                    .CodeId;
                DownloadPanel1.dcTargetCodeID = targetCodeId;
                DownloadPanel1.matchedRecordsCount = GetInt32(lnkMatchedRecords.Text);
                DownloadPanel1.nonMatchedRecordsCount = GetInt32(lnkNonMatchedRecords.Text);
                DownloadPanel1.TotalFileRecords = GetInt32(lblTotalFileRecords.Text);
                DownloadPanel1.LoadControls();
                DownloadPanel1.LoadDownloadTemplate();
                DownloadPanel1.loadExportFields();
                DownloadPanel1.ValidateExportPermission();
            }
            else if (dataTable.Rows.Count > 0)
            {
                btnDCDownload.OnClientClick = "return confirmPopupPurchase();";

                if (Master.UserSession.CurrentUser.IsKMStaff)
                {
                    plKmStaff.Visible = true;
                    plNotes.Visible = false;
                    drpIsBillable.SelectedIndex = -1;

                    var compareViews = new UasWorkers
                        .DataCompareView()
                        .SelectForRun(GetInt32(hfDcRunID.Value));
                    var targetId = brandId > 0 ? (int?)brandId : null;
                    var compareView = compareViews.FirstOrDefault(u => u.DcTargetCodeId == targetCodeId && u.DcTargetIdUad == targetId);

                    if (compareView != null)
                    {
                        if (compareView.IsBillable)
                        {
                            drpIsBillable.Enabled = false;
                        }
                    }
                }
                else
                {
                    plKmStaff.Visible = false;
                    btnDCDownload.OnClientClick = null;
                    plNotes.Visible = false;
                }

                mdlPopupDCDownload.Show();
            }
        }

        protected void lnkDataCompareSummary_Command(object sender, CommandEventArgs e)
        {
            DataCompareSummary.BrandID = Convert.ToInt32(hfBrandID.Value);
            DataCompareSummary.PubID = 0;
            DataCompareSummary.UserID = Master.LoggedInUser;
            DataCompareSummary.ViewType = ViewType;
            DataCompareSummary.ProcessCode = hfDataCompareProcessCode.Value;
            DataCompareSummary.TotalRecords = Convert.ToInt32(lblTotalFileRecords.Text);
            DataCompareSummary.DcTargetCodeID = Convert.ToInt32(hfDcTargetCodeID.Value);
            DataCompareSummary.MatchType = e.CommandArgument.ToString();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
            {
                if (e.CommandArgument.ToString().ToUpper() == "MATCHED")
                    DataCompareSummary.MatchedNonMatchedRecords = Convert.ToInt32(lnkMatchedRecords.Text);
                else
                    DataCompareSummary.MatchedNonMatchedRecords = Convert.ToInt32(lnkMatchedNotIn.Text);
            }
            else
            {
                DataCompareSummary.MatchedNonMatchedRecords = Convert.ToInt32(lnkMatchedRecords.Text);
            }

            DataCompareSummary.loadControls();
            DataCompareSummary.Visible = true;
        }

        protected void drpIsBillable_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(drpIsBillable.SelectedValue) == true)
            {
                btnDCDownload.OnClientClick = "return confirmPopupPurchase();";
                plNotes.Visible = false;
            }
            else
            {
                btnDCDownload.OnClientClick = null;
                plNotes.Visible = true;
            }

            mdlPopupDCDownload.Show();
        }

        protected void btnDCDownload_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                DataTable dt = new DataTable();

                if (Convert.ToInt32(hfBrandID.Value) > 0)
                    dt = new FrameworkUAD.BusinessLogic.DataCompareProfile().GetDataCompareData(new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient), hfDataCompareProcessCode.Value, Convert.ToInt32(hfDcTargetCodeID.Value), "NonMatched", Convert.ToInt32(hfBrandID.Value));
                else
                    dt = new FrameworkUAD.BusinessLogic.DataCompareProfile().GetDataCompareData(new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient), hfDataCompareProcessCode.Value, Convert.ToInt32(hfDcTargetCodeID.Value), "NonMatched", 0);

                //Save DataCompare view details

                int dcViewID = 0;

                if (drpDataCompareSourceFile.SelectedValue != "")
                {
                    int? targetID = Convert.ToInt32(hfBrandID.Value) > 0 ? (int?)Convert.ToInt32(hfBrandID.Value) : null;
                    int typeCodeID = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Data_Compare_Type, FrameworkUAD_Lookup.Enums.DataCompareType.Match.ToString()).CodeId;

                    //Calculating Total UAD Records count
                    Filter filter = new Filter();
                    filter.ViewType = ViewType;

                    if (Convert.ToInt32(hfBrandID.Value) > 0)
                    {
                        filter.BrandID = Convert.ToInt32(hfBrandID.Value);
                        filter.Fields.Add(new Field("Brand", Convert.ToInt32(hfBrandID.Value).ToString(), "", "", Enums.FiltersType.Brand, "BRAND"));
                    }

                    filter.Execute(Master.clientconnections, "");

                    List<FrameworkUAS.Entity.DataCompareView> datacv = new FrameworkUAS.BusinessLogic.DataCompareView().SelectForRun(Convert.ToInt32(hfDcRunID.Value));

                    int tcID = Code.GetDataCompareTarget().Find(x => x.CodeName == Enums.DataCompareViewType.Consensus.ToString()).CodeID;

                    int paymentStatusPendingID = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Pending.ToString()).CodeId;
                    int paymentStatusNon_BilledID = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Non_Billed.ToString()).CodeId;

                    if (datacv.Exists(y => y.DcTargetCodeId == tcID && (y.PaymentStatusId == paymentStatusPendingID || y.PaymentStatusId == paymentStatusNon_BilledID)))
                    {
                        int id = datacv.Find(y => y.DcTargetCodeId == tcID && (y.PaymentStatusId == paymentStatusPendingID || y.PaymentStatusId == paymentStatusNon_BilledID)).DcViewId;
                        new FrameworkUAS.BusinessLogic.DataCompareView().Delete(id);
                        dcViewID = saveDataCompareView(Convert.ToInt32(hfDcTargetCodeID.Value), targetID, typeCodeID, filter.Count);
                    }
                    else
                    {

                        if (!datacv.Exists(u => u.DcTargetCodeId == Convert.ToInt32(hfDcTargetCodeID.Value) && u.DcTargetIdUad == targetID && u.DcTypeCodeId == typeCodeID))
                        {
                            dcViewID = saveDataCompareView(Convert.ToInt32(hfDcTargetCodeID.Value), targetID, typeCodeID, filter.Count);
                        }
                        else
                        {
                            if (plKmStaff.Visible)
                            {
                                FrameworkUAS.Entity.DataCompareView dcv = datacv.Find(u => u.DcTargetCodeId == Convert.ToInt32(hfDcTargetCodeID.Value) && u.DcTargetIdUad == targetID && u.DcTypeCodeId == typeCodeID);

                                if (!dcv.IsBillable && (plKmStaff.Visible && Convert.ToBoolean(drpIsBillable.SelectedValue)) || !Convert.ToBoolean(drpIsBillable.SelectedValue))
                                {
                                    dcViewID = dcv.DcViewId;
                                    hfDcViewID.Value = dcViewID.ToString();

                                    dcv.UadNetCount = filter.Count;
                                    dcv.MatchedCount = Convert.ToInt32(lnkMatchedRecords.Text);
                                    dcv.NoDataCount = Convert.ToInt32(lnkNonMatchedRecords.Text);
                                    dcv.Cost = new FrameworkUAS.BusinessLogic.DataCompareView().GetDataCompareCost(Master.UserSession.CurrentUser.UserID, Master.UserSession.ClientID, (filter.Count + Convert.ToInt32(lblTotalFileRecords.Text)), FrameworkUAD_Lookup.Enums.DataCompareType.Match, FrameworkUAD_Lookup.Enums.DataCompareCost.MergePurge);

                                    if (plNotes.Visible)
                                        dcv.Notes = txtNotes.Text;

                                    dcv.IsBillable = plKmStaff.Visible ? (Convert.ToBoolean(drpIsBillable.SelectedValue)) : true;

                                    if (plKmStaff.Visible && !Convert.ToBoolean(drpIsBillable.SelectedValue))
                                        dcv.PaymentStatusId = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Non_Billed.ToString()).CodeId;
                                    else
                                        dcv.PaymentStatusId = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Unpaid.ToString()).CodeId;

                                    dcv.DateUpdated = DateTime.Now;
                                    dcv.UpdatedByUserID = Master.UserSession.UserID;

                                    new FrameworkUAS.BusinessLogic.DataCompareView().Save(dcv).ToString();
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
                
                string outfilepath = string.Empty;

                if (dcViewID > 0)
                {
                    string filename = string.Empty;
                    Guid g = System.Guid.NewGuid();
                    filename = "filter_report_" + g.ToString() + ".tsv";
                    outfilepath = Server.MapPath("../downloads/datacompare/") + Master.ClientID + "/" + filename;
                    string path = Server.MapPath("../downloads/datacompare/") + Master.ClientID + "/";

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    var colList = dt.Columns.OfType<DataColumn>().Select(col => col.ColumnName);

                    FrameworkUAS.Entity.DataCompareDownload dcd = new FrameworkUAS.Entity.DataCompareDownload();

                    dcd.DcViewId = dcViewID;
                    dcd.WhereClause = "Non Matched";
                    dcd.DcTypeCodeId = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Data_Compare_Type, FrameworkUAD_Lookup.Enums.DataCompareType.Match.ToString()).CodeId;
                    dcd.ProfileCount = dt.Rows.Count;
                    dcd.TotalBilledCost = new FrameworkUAS.BusinessLogic.DataCompareView().GetDataCompareCost(Master.UserSession.CurrentUser.UserID, Master.UserSession.ClientID, Convert.ToInt32(lnkNonMatchedRecords.Text), FrameworkUAD_Lookup.Enums.DataCompareType.Match, FrameworkUAD_Lookup.Enums.DataCompareCost.Download);
                    dcd.IsPurchased = plKmStaff.Visible ? (Convert.ToBoolean(drpIsBillable.SelectedValue)) : true;
                    dcd.PurchasedByUserId = Master.UserSession.UserID;
                    dcd.PurchasedDate = DateTime.Now;
                    dcd.DownloadFileName = filename;

                    int dcDownloadID = new FrameworkUAS.BusinessLogic.DataCompareDownload().Save(dcd);
                }
                else
                {
                    outfilepath = Server.MapPath("../main/temp/") + System.Guid.NewGuid().ToString().Substring(0, 5) + ".tsv";
                }

                Utilities.DownloadDataCompare(dt.Rows.Count, dt, outfilepath);
            }
            else
            {
                mdlPopupDCDownload.Show();
            }
        }

        private int saveDataCompareView(int targetCodeID, int? targetID, int typeCodeID, int UadNetCount)
        {
            FrameworkUAS.Entity.DataCompareView dcv = new FrameworkUAS.Entity.DataCompareView();

            dcv.DcTargetCodeId = targetCodeID;
            dcv.DcTargetIdUad = targetID;
            dcv.DcTypeCodeId = typeCodeID;
            dcv.DcRunId = Convert.ToInt32(hfDcRunID.Value);
            dcv.UadNetCount = UadNetCount;
            dcv.MatchedCount = Convert.ToInt32(lnkMatchedRecords.Text);
            dcv.NoDataCount = Convert.ToInt32(lnkNonMatchedRecords.Text);
            dcv.Cost = new FrameworkUAS.BusinessLogic.DataCompareView().GetDataCompareCost(Master.UserSession.CurrentUser.UserID, Master.UserSession.ClientID, (UadNetCount + Convert.ToInt32(lblTotalFileRecords.Text)), FrameworkUAD_Lookup.Enums.DataCompareType.Match, FrameworkUAD_Lookup.Enums.DataCompareCost.MergePurge);

            if (plNotes.Visible)
                dcv.Notes = txtNotes.Text;

            dcv.IsBillable = plKmStaff.Visible ? (Convert.ToBoolean(drpIsBillable.SelectedValue)) : true;

            if (plKmStaff.Visible && !Convert.ToBoolean(drpIsBillable.SelectedValue))
                dcv.PaymentStatusId = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Non_Billed.ToString()).CodeId;
            else
                dcv.PaymentStatusId = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Unpaid.ToString()).CodeId;

            dcv.DateCreated = DateTime.Now;
            dcv.CreatedByUserID = Master.UserSession.UserID;

            int dcViewID = new FrameworkUAS.BusinessLogic.DataCompareView().Save(dcv);
            hfDcViewID.Value = dcViewID.ToString();

            return dcViewID;
        }

        private void loadProductandDimensions()
        {
            List<PubTypes> pt = new List<PubTypes>();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
                pt = PubTypes.GetActiveByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
            else
                pt = PubTypes.GetActive(Master.clientconnections);

            PubTypeRepeater.DataSource = pt;
            PubTypeRepeater.DataBind();

            List<MasterGroup> masterGroup = new List<MasterGroup>();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
                masterGroup = MasterGroup.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
            else
                masterGroup = MasterGroup.GetSearchEnabled(Master.clientconnections);

            dlDimensions.DataSource = masterGroup;
            dlDimensions.DataBind();
        }

        protected void PubTypeRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            LinkButton PubTypeLinkButton = e.Item.FindControl("PubTypeLinkButton") as LinkButton;
            PubTypeLinkButton.CommandName = e.Item.ItemIndex.ToString();

            LinkButton lnkPubTypeShowHide = e.Item.FindControl(LinkPubTypeShowHide) as LinkButton;
            lnkPubTypeShowHide.CommandName = e.Item.ItemIndex.ToString();
        }

        private string GetLinkTitle(int pubtypeid)
        {
            return PubTypes.GetByID(Master.clientconnections, pubtypeid).PubTypeDisplayName;
        }

        protected void dlDimensions_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDimensionPopup = e.Item.FindControl("lnkDimensionPopup") as LinkButton;
                lnkDimensionPopup.CommandName = e.Item.ItemIndex.ToString();

                LinkButton lnkDimensionShowHide = e.Item.FindControl(LinkDimensionsShowHide) as LinkButton;
                lnkDimensionShowHide.CommandName = e.Item.ItemIndex.ToString();
            }
        }

        public void LoadGridFilters()
        {
            bpResults.Visible = false;
            grdFilters.DataSource = null;
            grdFilters.DataBind();

            grdFilterCounts.DataSource = null;
            grdFilterCounts.DataBind();
            grdFilterCounts.Visible = false;

            ctrlVenn1.Clear();
            ctrlVenn1.Visible = false;

            if (fc.Count > 0)
            {
                if (ConfigurationManager.AppSettings["ShowQuery"].ToUpper() == "YES")
                {
                    txtShowQuery.Text = string.Empty;
                    foreach (Filter fil in fc)
                    {
                        txtShowQuery.Text = txtShowQuery.Text + "\r\n\r\n" + fil.getFilterQuery(Master.clientconnections);
                        txtShowQuery.Visible = true;
                    }
                }

                bpResults.Visible = true;
                grdFilters.DataSource = fc;
                grdFilters.DataBind();

                if (rblLoadType.SelectedValue.Equals("Auto Load", StringComparison.OrdinalIgnoreCase))
                {
                    grdFilterCounts.DataSource = fc.FilterComboList.Where(x => (x.SelectedFilterNo.Split(',').Length > 1 ? Convert.ToInt32(x.SelectedFilterNo.Split(',')[1]) <= 5 : true && Convert.ToInt32(x.SelectedFilterNo.Split(',')[0]) <= 5) && (x.SuppressedFilterNo == null || x.SuppressedFilterNo == "" ? true : Convert.ToInt32(x.SuppressedFilterNo) <= 5));
                    grdFilterCounts.DataBind();
                    grdFilterCounts.Visible = true;

                    ctrlVenn1.CreateVenn(fc);
                    ctrlVenn1.Visible = true;
                }
            }
        }

        private List<Field> LoadGridFilterValues(int filterNo)
        {
            return fc.SingleOrDefault(f => f.FilterNo == filterNo).Fields;
        }

        protected void btnAddFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (rblLoadType.SelectedValue.Equals("Manual Load", StringComparison.OrdinalIgnoreCase))
                {
                    grdFilterCounts.DataSource = null;
                    grdFilterCounts.DataBind();
                    grdFilterCounts.Visible = false;

                    ctrlVenn1.Clear();
                    ctrlVenn1.Visible = false;
                }

                if (pnlBrand.Visible)
                {
                    if (Convert.ToInt32(hfBrandID.Value) < 0)
                    {
                        DisplayError("Please Select Brand.");
                        return;
                    }
                    else
                        drpBrand.Enabled = false;
                }

                if (pnlDataCompare.Visible)
                {
                    drpDataCompareSourceFile.Enabled = false;
                    rblDataCompareOperation.Enabled = false;
                }

                DropDownList drpOpenActivity = (DropDownList)ActivityFilter.FindControl("drpOpenActivity");
                RadioButtonList rblOpenSearchType = (RadioButtonList)ActivityFilter.FindControl("rblOpenSearchType");
                RadioButtonList rblClickSearchType = (RadioButtonList)ActivityFilter.FindControl("rblClickSearchType");
                DropDownList drpClickActivity = (DropDownList)ActivityFilter.FindControl("drpClickActivity");

                string PubIDs = getPubsValues();

                if (drpOpenActivity.SelectedValue != string.Empty)
                {
                    if (string.Equals("Search Selected Products", rblOpenSearchType.SelectedValue, StringComparison.OrdinalIgnoreCase) && Convert.ToInt32(drpOpenActivity.SelectedValue) >= 0 && PubIDs == string.Empty)
                    {
                        DisplayError("Please select at least one product or change search to 'Search All' in  Open Criteria.");
                        return;
                    }
                }

                if (drpClickActivity.SelectedValue != string.Empty)
                {
                    if (string.Equals("Search Selected Products", rblClickSearchType.SelectedValue, StringComparison.OrdinalIgnoreCase) && Convert.ToInt32(drpClickActivity.SelectedValue) >= 0 && PubIDs == string.Empty)
                    {
                        DisplayError("Please select at least one product or change search to 'Search All' in Click Criteria.");
                        return;
                    }
                }

                Filter f = getFilter();

                if (f.Fields.Count > 0 && (!(f.Fields.Count == 1 && f.Fields.FirstOrDefault().Name.Equals("DataCompare", StringComparison.OrdinalIgnoreCase))))
                {
                    if (hfFilterNo.Value != string.Empty)
                    {
                        f.FilterNo = Convert.ToInt32(hfFilterNo.Value);
                        f.FilterName = hfFilterName.Value;
                        f.FilterGroupName = hfFilterGroupName.Value;

                        if (rblLoadType.SelectedValue.Equals("Manual Load", StringComparison.OrdinalIgnoreCase))
                            fc.Update(f, true);
                        else
                            fc.Update(f);

                        hfFilterNo.Value = string.Empty;
                        //loadProductandDimensions();
                        //loadStandardFiltersListboxes();
                    }
                    else
                    {
                        f.FilterNo = fc.Count + 1;

                        if (rblLoadType.SelectedValue.Equals("Manual Load", StringComparison.OrdinalIgnoreCase))
                            fc.Add(f, true);
                        else
                            fc.Add(f);
                    }

                    if (rblLoadType.SelectedValue.Equals("Auto Load", StringComparison.OrdinalIgnoreCase))
                        fc.Execute();
                    else
                        fc.FilterComboList = null;

                    FilterCollection = fc;
                    LoadGridFilters();
                    ResetFilterTabControls();
                }
                else
                {
                    DisplayError("Please select any filter.");
                }
            }
            catch (Exception ex)
            {
                Utilities.Log_Error(Request.RawUrl.ToString(), "btnAddFilter_Click", ex);
                DisplayError(ex.Message);
            }

            TabContainer.ActiveTabIndex = 0;
            FilterSegmentation.LoadControls();
            FilterSegmentation.UpdateFiltersegmentationCounts();
        }

        private Filter getFilter()
        {
            var filter = new Filter { ViewType = ViewType };

            if (pnlDataCompare.Visible && drpDataCompareSourceFile.SelectedValue != string.Empty)
            {
                filter.Fields.Add(
                    new Field(
                        NameDataCompare,
                        $"{drpDataCompareSourceFile.SelectedValue.Split('|').First()}|{rblDataCompareOperation.SelectedValue}",
                        $"{drpDataCompareSourceFile.SelectedItem.Text}|{rblDataCompareOperation.SelectedItem.Text}",
                        string.Empty,
                        Enums.FiltersType.DataCompare,
                        NameDataCompare));
            }

            if (getPubsValues() != string.Empty)
            {
                filter.Fields.Add(new Field(NameProduct, getPubsValues(), getPubsListboxText(), string.Empty, Enums.FiltersType.Product, NameProduct));
            }

            var hiddenFieldBrandId = IntTryParse(hfBrandID.Value);
            var masterGroup = hiddenFieldBrandId > 0 
                                  ? MasterGroup.GetSearchEnabledByBrandID(Master.clientconnections, hiddenFieldBrandId) 
                                  : MasterGroup.GetSearchEnabled(Master.clientconnections);

            this.AddFilterFieldsMasterGroup(masterGroup, filter);

            if (pnlBrand.Visible && hiddenFieldBrandId > 0)
            {
                filter.BrandID = hiddenFieldBrandId;
                filter.Fields.Add(new Field(NameBrand, hfBrandID.Value, hfBrandName.Value, string.Empty, Enums.FiltersType.Brand, NameBrand.ToUpper()));
            }

            var listvalues = Utilities.getListboxSelectedValues(lstGeoCode);

            if (!string.IsNullOrWhiteSpace(listvalues))
            {
                SelectStateOnRegion();
            }

            listvalues = Utilities.getListboxSelectedValues(lstState);

            if (!string.IsNullOrWhiteSpace(listvalues))
            {
                filter.Fields.Add(new Field(NameState, listvalues, Utilities.getListboxText(lstState), string.Empty, Enums.FiltersType.Standard, NameState.ToUpper()));
            }

            listvalues = Utilities.getListboxSelectedValues(lstCountry);

            if (!string.IsNullOrWhiteSpace(listvalues))
            {
                filter.Fields.Add(new Field(NameCountry, listvalues, Utilities.getListboxText(lstCountry), string.Empty, Enums.FiltersType.Standard, NameCountry.ToUpper()));
            }

            this.AddFilterFieldsComboBox(filter);
            this.AddFilterFieldsGeoAdhocActivityCirculationPopup(filter);

            return filter;
        }

        private void AddFilterFieldsMasterGroup(List<MasterGroup> masterGroups, Filter filter)
        {
            foreach (DataListItem dataListItem in this.dlDimensions.Items)
            {
                var lblResponseGroup = dataListItem.FindControl(LblResponseGroup) as Label;
                var lstResponse = dataListItem.FindControl(LstResponse) as ListBox;

                if (lstResponse != null)
                {
                    var selectedvalues = Utilities.getListboxSelectedValues(lstResponse);

                    if (selectedvalues.Length > 0)
                    {
                        foreach (var grouptItem in masterGroups)
                        {
                            if (lblResponseGroup?.Text.Equals(grouptItem.DisplayName, StringComparison.OrdinalIgnoreCase) == true)
                            {
                                filter.Fields.Add(
                                    new Field(
                                        lblResponseGroup.Text,
                                        selectedvalues,
                                        Utilities.getListboxText(lstResponse),
                                        string.Empty,
                                        Enums.FiltersType.Dimension,
                                        grouptItem.ColumnReference));

                                break;
                            }
                        }
                    }
                }
            }
        }

        protected void lnkResetAll_Click(object sender, EventArgs e)
        {
            if (ViewType == Enums.ViewType.ConsensusView)
            {
                if (Convert.ToInt32(hfBrandID.Value) > 0)
                    Response.Redirect("report.aspx?ViewType=ConsensusView&brandID=" + Convert.ToInt32(hfBrandID.Value));
                else
                    Response.Redirect("report.aspx?ViewType=ConsensusView");
            }
            else
            {
                if (Convert.ToInt32(hfBrandID.Value) > 0)
                    Response.Redirect("report.aspx?ViewType=RecencyView&brandID=" + Convert.ToInt32(hfBrandID.Value));
                else
                    Response.Redirect("report.aspx?ViewType=RecencyView");
            }
        }

        protected void lnkPermissionDownload_Command(object sender, CommandEventArgs e)
        {
            CampaignID = 0;
            CampaignFilterID = 0;
            lblPermission.Text = "Permission";
            string temp = e.CommandArgument.ToString();
            string[] args = temp.Split('/');
            lblMasterID.Text = args[0];
            lblPremissionType.Text = args[1].ToUpper();
            lblDownloadCount.Text = args[2];
            lblIsPopupCrossTab.Text = "false";
            lblIsPopupDimension.Text = "true";
            ShowDownloadPanel();
            mdlPopupDimensionReport.Show();
        }

        protected void lnkdownload_Command(object sender, CommandEventArgs e)
        {
            CampaignID = 0;
            CampaignFilterID = 0;
            lblPermission.Text = string.Empty;
            lblMasterID.Text = string.Empty;
            string temp = e.CommandArgument.ToString();
            string[] args = temp.Split('/');
            lblSelectedFilterNos.Text = args[0];
            lblSuppressedFilterNos.Text = args[1];
            lblSelectedFilterOperation.Text = args[2];
            lblSuppressedFilterOperation.Text = args[3];
            lblFilterCombination.Text = args[4];
            lblDownloadCount.Text = args[5];
            lblIsPopupCrossTab.Text = "false";
            lblIsPopupDimension.Text = "false";
            ShowDownloadPanel();
        }

        protected void lnkdownloadAllUnion_Command(object sender, CommandEventArgs e)
        {
            CampaignID = 0;
            CampaignFilterID = 0;
            lblPermission.Text = string.Empty;
            lblMasterID.Text = string.Empty;
            lblSelectedFilterNos.Text = string.Join(",", fc.Select(f => f.FilterNo));
            lblSuppressedFilterNos.Text = string.Empty;
            lblSelectedFilterOperation.Text = "Union";
            lblSuppressedFilterOperation.Text = string.Empty;
            lblFilterCombination.Text = "All Union";
            lblDownloadCount.Text = e.CommandArgument.ToString();
            lblIsPopupCrossTab.Text = "false";
            lblIsPopupDimension.Text = "false";
            ShowDownloadPanel();
        }

        protected void lnkdownloadAllIntersect_Command(object sender, CommandEventArgs e)
        {
            CampaignID = 0;
            CampaignFilterID = 0;
            lblPermission.Text = string.Empty;
            lblMasterID.Text = string.Empty;
            lblSelectedFilterNos.Text = string.Join(",", fc.Select(f => f.FilterNo));
            lblSuppressedFilterNos.Text = string.Empty;
            lblSelectedFilterOperation.Text = "Intersect";
            lblSuppressedFilterOperation.Text = string.Empty;
            lblFilterCombination.Text = "All Intersect";
            lblDownloadCount.Text = e.CommandArgument.ToString();
            lblIsPopupCrossTab.Text = "false";
            lblIsPopupDimension.Text = "false";
            ShowDownloadPanel();
        }

        protected void lnkCount_Command(object sender, CommandEventArgs e)
        {
            lblPermission.Text = string.Empty;
            lblMasterID.Text = string.Empty;
            string temp = e.CommandArgument.ToString();
            string[] args = temp.Split('/');
            lblSelectedFilterNos.Text = args[0];
            lblSuppressedFilterNos.Text = args[1];
            lblSelectedFilterOperation.Text = args[2];
            lblSuppressedFilterOperation.Text = args[3];
            lblFilterCombination.Text = args[4];
            lblDownloadCount.Text = args[5];
            lblIsPopupCrossTab.Text = "false";
            lblIsPopupDimension.Text = "false";
            ShowDownloadPanel();
        }

        protected void grdFilters_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Filter filter = fc.SingleOrDefault(f => f.FilterNo == Convert.ToInt32(grdFilters.DataKeys[e.RowIndex].Value.ToString()));
            fc.Remove(filter);

            //Update filterNo after delete
            int i = 1;
            foreach (Filter f in fc)
            {
                f.FilterNo = i++;

                //if (rblLoadType.SelectedValue.Equals("Manual Load", StringComparison.OrdinalIgnoreCase))
                //    fc.Update(f, true);
                //else
                //    fc.Update(f);
            }

            if (rblLoadType.SelectedValue.Equals("Auto Load", StringComparison.OrdinalIgnoreCase))
                fc.Execute();
            else
                fc.FilterComboList = null;

            FilterCollection = fc;
            LoadGridFilters();
            
            if(fc.Count <= 1)
                hfHasFilterSegmentation.Value = "false";

            FilterSegmentation.FilterViewCollection.Clear();
            FilterSegmentation.LoadControls();
        }

        protected void grdFilters_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }

        protected void grdFilters_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
        }

        protected void grdFilters_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                LinkButton lnkCancel = (LinkButton)row.FindControl("lnkCancel");
                lnkCancel.Visible = false;
                LinkButton lnkEdit = (LinkButton)row.FindControl("lnkEdit");
                lnkEdit.Visible = true;
                ResetFilterTabControls();
                hfFilterNo.Value = string.Empty;
                hfFilterName.Value = string.Empty;
                hfFilterGroupName.Value = string.Empty;
                //loadProductandDimensions();
                //loadStandardFiltersListboxes();
            }
            else if (e.CommandName == "Edit")
            {
                foreach (GridViewRow r in grdFilters.Rows)
                {
                    LinkButton lCancel = (LinkButton)r.FindControl("lnkCancel");
                    lCancel.Visible = false;
                    LinkButton lEdit = (LinkButton)r.FindControl("lnkEdit");
                    lEdit.Visible = true;
                }

                GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                LinkButton lnkCancel = (LinkButton)row.FindControl("lnkCancel");
                lnkCancel.Visible = true;
                LinkButton lnkEdit = (LinkButton)row.FindControl("lnkEdit");
                lnkEdit.Visible = false;
                ResetFilterTabControls();
                Filter f = fc.SingleOrDefault(filter => filter.FilterNo == Convert.ToInt32(e.CommandArgument.ToString()));
                hfFilterNo.Value = e.CommandArgument.ToString();
                hfFilterName.Value = f.FilterName;
                hfFilterGroupName.Value = f.FilterGroupName;

                string[] strvalues;

                foreach (Field field in f.Fields)
                {
                    switch (field.FilterType)
                    {
                        case Enums.FiltersType.Product:

                            int pubTypeID = 0;

                            // get pubs          
                            if (Convert.ToInt32(hfBrandID.Value) > 0)
                                lpubs = Pubs.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                            else
                                lpubs = Pubs.GetSearchEnabled(Master.clientconnections);

                            Dictionary<int, string> dSelected = new Dictionary<int, string>();

                            foreach (string s in field.Values.Split(','))
                            {
                                try
                                {
                                    pubTypeID = lpubs.Find(x => x.PubID == Convert.ToInt32(s)).PubTypeID;
                                }
                                catch
                                {
                                }

                                if (dSelected.ContainsKey(pubTypeID))
                                    dSelected[pubTypeID] = dSelected[pubTypeID] + "," + s;
                                else
                                    dSelected.Add(pubTypeID, s);
                            }

                            Repeater pubTypeRepeater = Panel1.FindControl("PubTypeRepeater") as Repeater;

                            foreach (RepeaterItem repeaterItem in pubTypeRepeater.Items)
                            {
                                var hfPubTypeID = repeaterItem.FindControl(HiddenFieldPubTypeId) as HiddenField;

                                if (dSelected.ContainsKey(int.Parse(hfPubTypeID.Value)))
                                {
                                    ListBox PubTypeListBox = repeaterItem.FindControl("PubTypeListBox") as ListBox;

                                    LinkButton lnkPubTypeShowHide = (LinkButton)repeaterItem.FindControl(LinkPubTypeShowHide);
                                    Panel pnlPubTypeBody = (Panel)repeaterItem.FindControl(PanelPubTypeBody);

                                    if (lnkPubTypeShowHide.Text == "(Show...)")
                                    {
                                        if (PubTypeListBox.Items.Count == 0)
                                        {
                                            var pubsbyPubTypeID = (from p in lpubs
                                                                   where p.PubTypeID == Convert.ToInt32(hfPubTypeID.Value) && p.EnableSearching == true
                                                                   select new { p.PubID, p.PubName });

                                            PubTypeListBox.DataSource = pubsbyPubTypeID;
                                            PubTypeListBox.DataValueField = PubId;
                                            PubTypeListBox.DataTextField = PubName;
                                            PubTypeListBox.DataBind();
                                        }
                                    }

                                    lnkPubTypeShowHide.Text = TextHide;
                                    pnlPubTypeBody.Visible = true;

                                    Utilities.SelectFilterListBoxes(PubTypeListBox, dSelected[int.Parse(hfPubTypeID.Value)]);
                                }
                            }
                            break;

                        case Enums.FiltersType.Dimension:

                            List<MasterCodeSheet> mastercodesheet = new List<MasterCodeSheet>();

                            if (Convert.ToInt32(hfBrandID.Value) > 0)
                                mastercodesheet = MasterCodeSheet.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                            else
                                mastercodesheet = MasterCodeSheet.GetSearchEnabled(Master.clientconnections);

                            foreach (DataListItem di in dlDimensions.Items)
                            {
                                ListBox lstResponse = (ListBox)di.FindControl(LstResponse);
                                HiddenField hfMasterGroup = (HiddenField)di.FindControl(HiddenFieldMasterGroup);
                                LinkButton lnkDimensionShowHide = (LinkButton)di.FindControl(LinkDimensionsShowHide);
                                Panel pnlDimBody = (Panel)di.FindControl(PanelDimBody);

                                string colRef = MasterGroup.GetByID(Master.clientconnections, Convert.ToInt32(hfMasterGroup.Value)).ColumnReference;

                                if (colRef.ToUpper() == field.Group.ToUpper())
                                {
                                    if (lnkDimensionShowHide.Text == "(Show...)")
                                    {
                                        pnlDimBody.Visible = true;

                                        if (lstResponse.Items.Count == 0)
                                        {
                                            lstResponse.DataTextField = MasterDesc;
                                            lstResponse.DataValueField = MasterId;

                                            var MasterCodeSheetQuery = (from m in mastercodesheet
                                                                        where m.MasterGroupID == Convert.ToInt32(hfMasterGroup.Value)
                                                                        orderby m.SortOrder ascending
                                                                        select new { m.MasterID, masterdesc = m.MasterDesc + ' ' + '(' + m.MasterValue + ')' });

                                            lstResponse.DataSource = MasterCodeSheetQuery;
                                            lstResponse.DataBind();
                                        }

                                        lnkDimensionShowHide.Text = TextHide;
                                    }

                                    Utilities.SelectFilterListBoxes(lstResponse, field.Values);
                                }
                            }
                            break;

                        case Enums.FiltersType.Standard:
                            switch (field.Name.ToUpper())
                            {

                                case "STATE":
                                    Utilities.SelectFilterListBoxes(lstState, field.Values);
                                    break;
                                case "COUNTRY":
                                    Utilities.SelectFilterListBoxes(lstCountry, field.Values);
                                    break;
                                case "EMAIL":
                                    if (!string.IsNullOrWhiteSpace(field.Values))
                                    {
                                        string[] items = field.Values.Split(',');
                                        foreach (RadComboBoxItem item in rcbEmail.Items)
                                        {
                                            item.Selected = items.Where(c => c.Equals(item.Value)).FirstOrDefault() != null;
                                            item.Checked = item.Selected;
                                        }
                                    }
                                    break;
                                case "PHONE":
                                    if (!string.IsNullOrWhiteSpace(field.Values))
                                    {
                                        string[] items = field.Values.Split(',');
                                        foreach (RadComboBoxItem item in rcbPhone.Items)
                                        {
                                            item.Selected = items.Where(c => c.Equals(item.Value)).FirstOrDefault() != null;
                                            item.Checked = item.Selected;
                                        }
                                    }
                                    break;
                                case "FAX":
                                    if (!string.IsNullOrWhiteSpace(field.Values))
                                    {
                                        string[] items = field.Values.Split(',');
                                        foreach (RadComboBoxItem item in rcbFax.Items)
                                        {
                                            item.Selected = items.Where(c => c.Equals(item.Value)).FirstOrDefault() != null;
                                            item.Checked = item.Selected;
                                        }
                                    }
                                    break;
                                case "MEDIA":
                                    if (!string.IsNullOrWhiteSpace(field.Values))
                                    {
                                        string[] items = field.Values.Split(',');
                                        foreach (RadComboBoxItem item in rcbMedia.Items)
                                        {
                                            item.Selected = items.Where(c => c.Equals(item.Value)).FirstOrDefault() != null;
                                            item.Checked = item.Selected;
                                        }
                                    }
                                    break;
                                case "GEOLOCATED":
                                    if (!string.IsNullOrWhiteSpace(field.Values))
                                    {
                                        string[] items = field.Values.Split(',');
                                        foreach (RadComboBoxItem item in rcbIsLatLonValid.Items)
                                        {
                                            item.Selected = items.Where(c => c.Equals(item.Value)).FirstOrDefault() != null;
                                            item.Checked = item.Selected;
                                        }
                                    }
                                    break;
                                case "MAILPERMISSION":
                                    if (!string.IsNullOrWhiteSpace(field.Values))
                                    {
                                        string[] items = field.Values.Split(',');
                                        foreach (RadComboBoxItem item in rcbMailPermission.Items)
                                        {
                                            item.Selected = items.Where(c => c.Equals(item.Value)).FirstOrDefault() != null;
                                            item.Checked = item.Selected;
                                        }
                                    }
                                    break;
                                case "FAXPERMISSION":
                                    if (!string.IsNullOrWhiteSpace(field.Values))
                                    {
                                        string[] items = field.Values.Split(',');
                                        foreach (RadComboBoxItem item in rcbFaxPermission.Items)
                                        {
                                            item.Selected = items.Where(c => c.Equals(item.Value)).FirstOrDefault() != null;
                                            item.Checked = item.Selected;
                                        }
                                    }
                                    break;
                                case "PHONEPERMISSION":
                                    if (!string.IsNullOrWhiteSpace(field.Values))
                                    {
                                        string[] items = field.Values.Split(',');
                                        foreach (RadComboBoxItem item in rcbPhonePermission.Items)
                                        {
                                            item.Selected = items.Where(c => c.Equals(item.Value)).FirstOrDefault() != null;
                                            item.Checked = item.Selected;
                                        }
                                    }
                                    break;
                                case "OTHERPRODUCTSPERMISSION":
                                    if (!string.IsNullOrWhiteSpace(field.Values))
                                    {
                                        string[] items = field.Values.Split(',');
                                        foreach (RadComboBoxItem item in rcbOtherProductsPermission.Items)
                                        {
                                            item.Selected = items.Where(c => c.Equals(item.Value)).FirstOrDefault() != null;
                                            item.Checked = item.Selected;
                                        }
                                    }
                                    break;
                                case "THIRDPARTYPERMISSION":
                                    if (!string.IsNullOrWhiteSpace(field.Values))
                                    {
                                        string[] items = field.Values.Split(',');
                                        foreach (RadComboBoxItem item in rcbThirdPartyPermission.Items)
                                        {
                                            item.Selected = items.Where(c => c.Equals(item.Value)).FirstOrDefault() != null;
                                            item.Checked = item.Selected;
                                        }
                                    }
                                    break;
                                case "EMAILRENEWPERMISSION":
                                    if (!string.IsNullOrWhiteSpace(field.Values))
                                    {
                                        string[] items = field.Values.Split(',');
                                        foreach (RadComboBoxItem item in rcbEmailRenewPermission.Items)
                                        {
                                            item.Selected = items.Where(c => c.Equals(item.Value)).FirstOrDefault() != null;
                                            item.Checked = item.Selected;
                                        }
                                    }
                                    break;
                                case "TEXTPERMISSION":
                                    if (!string.IsNullOrWhiteSpace(field.Values))
                                    {
                                        string[] items = field.Values.Split(',');
                                        foreach (RadComboBoxItem item in rcbTextPermission.Items)
                                        {
                                            item.Selected = items.Where(c => c.Equals(item.Value)).FirstOrDefault() != null;
                                            item.Checked = item.Selected;
                                        }
                                    }
                                    break;
                                case "EMAIL STATUS":
                                    if (!string.IsNullOrWhiteSpace(field.Values))
                                    {
                                        string[] items = field.Values.Split(',');
                                        foreach (RadComboBoxItem item in rcbEmailStatus.Items)
                                        {
                                            item.Selected = items.Where(c => c.Equals(item.Value)).FirstOrDefault() != null;
                                            item.Checked = item.Selected;
                                        }
                                    }
                                    break;
                            }
                            break;

                        case Enums.FiltersType.Geo:
                            strvalues = field.SearchCondition.Split('|');
                            txtRadiusMax.Text = strvalues[2];
                            txtRadiusMin.Text = strvalues[1];

                            int number;

                            if (int.TryParse(strvalues[0], out number))
                            {
                                RadMtxtboxZipCode.Mask = "#####";
                                drpCountry.SelectedValue = "UNITED STATES";
                            }
                            else
                            {
                                RadMtxtboxZipCode.Mask = "L#L #L#";
                                drpCountry.SelectedValue = "CANADA";
                            }

                            RadMtxtboxZipCode.Text = strvalues[0];
                            break;

                        case Enums.FiltersType.Activity:
                            ActivityFilter.LoadActivityFilters(field);
                            break;

                        case Enums.FiltersType.Adhoc:
                            AdhocFilter.LoadAdhocFilters(field);
                            break;

                        case Enums.FiltersType.Circulation:
                            CirculationFilter.LoadCirculationFilters(field);
                            break;
                    }
                }
            }
        }

        protected void grdFilterCounts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void grdFilterCounts_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }
        protected void grdFilterCounts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grdFilters.Rows.Count > 1)
            {
                if (ctrlVenn1.VennParams != string.Empty)
                {
                    if (ScriptManager.GetCurrent(this).IsInAsyncPostBack)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), this.ctrlVenn1.VennDivID, "renderVenn('#" + this.ctrlVenn1.VennDivID + "', [" + ctrlVenn1.VennParams + "]);", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(typeof(Page), this.ctrlVenn1.VennDivID, "renderVenn('#" + this.ctrlVenn1.VennDivID + "', [" + ctrlVenn1.VennParams + "]);", true);
                    }
                }
            }
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {

        }

        void btn_Click(object sender, CommandEventArgs e)
        {
            string test = e.ToString();
        }

        protected void grdFilters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView grdFilterValues = (GridView)e.Row.FindControl("grdFilterValues");
                List<Field> grdFilterList = LoadGridFilterValues(Convert.ToInt32(grdFilters.DataKeys[e.Row.RowIndex].Value));
                grdFilterValues.DataSource = grdFilterList.Distinct().ToList();
                grdFilterValues.DataBind();

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.Edit))
                {
                    var chkSelectFilter = (CheckBox)e.Row.FindControl("cbSelectFilter");
                    chkSelectFilter.Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //LinkButton lnkdownloadAllUnion = (LinkButton)e.Row.FindControl("lnkdownloadAllUnion");
                //LinkButton lnkdownloadAllIntersect = (LinkButton)e.Row.FindControl("lnkdownloadAllIntersect");

                //if (fc.Count > 1)
                //{
                //    Label lblAllUnion = (Label)e.Row.FindControl("lblAllUnion");
                //    Label lblAllIntersect = (Label)e.Row.FindControl("lblAllIntersect");

                //    lblAllUnion.Text = "All Union : ";
                //    lblAllIntersect.Text = "All Intersect : ";

                //    int AllUnionCount = 0, AllIntersectCount = 0;

                //    if (rblLoadType.SelectedValue.Equals("Manual Load", StringComparison.OrdinalIgnoreCase))
                //    {
                //        List<string> allFilterIDs = new List<string>();

                //        foreach (Filter f in fc)
                //        {
                //            allFilterIDs.Add(f.FilterNo.ToString());
                //        }

                //        AllUnionCount = 0;

                //        AllIntersectCount = 0;
                //    }
                //    else
                //    {

                //        AllUnionCount = fc.AllUnion;

                //        AllIntersectCount = fc.AllInterSect;
                //    }


                //    if (AllUnionCount <= 0)
                //        lnkdownloadAllUnion.Enabled = false;

                //    lnkdownloadAllUnion.Text = AllUnionCount.ToString();
                //    lnkdownloadAllUnion.CommandArgument = AllUnionCount.ToString();
                //    lnkdownloadAllUnion.CommandName = "download";

                //    if (AllIntersectCount <= 0)
                //        lnkdownloadAllIntersect.Enabled = false;

                //    lnkdownloadAllIntersect.Text = AllIntersectCount.ToString();
                //    lnkdownloadAllIntersect.CommandArgument = AllIntersectCount.ToString();
                //    lnkdownloadAllIntersect.CommandName = "download";
                //}
            }
        }

        protected void grdFilters_PreRender(object sender, System.EventArgs e)
        {
            //if (grdFilters.Rows.Count > 0)
            //{
            //    for (int i = grdFilters.FooterRow.Cells.Count - 1; i >= 1; i += -1)
            //    {
            //        if (i >= 3)
            //        {
            //            grdFilters.FooterRow.Cells.RemoveAt(i);
            //        }
            //    }
            //    grdFilters.FooterRow.Cells[2].ColumnSpan = 5;
            //}
        }

        protected void grdFilterValues_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells[0].Text == "DataCompare") e.Row.Visible = false;
        }

        private string getPubsValues()
        {
            string selectedvalues = string.Empty;

            Repeater pubTypeRepeater = UpdatePanel1.FindControl("PubTypeRepeater") as Repeater;

            foreach (RepeaterItem repeaterItem in pubTypeRepeater.Items)
            {
                ListBox pubTypeListBox = repeaterItem.FindControl(PubTypeListBox) as ListBox;
                string tempString = Utilities.getListboxSelectedValues(pubTypeListBox);

                if (!string.IsNullOrEmpty(tempString))
                    selectedvalues += (selectedvalues == string.Empty ? tempString : "," + tempString);
            }

            return selectedvalues;
        }

        private string getStateValues()
        {
            string selectedvalues = string.Empty;
            foreach (ListItem item in lstState.Items)
            {
                if (item.Selected)
                    selectedvalues += selectedvalues == string.Empty ? "'" + item.Value + "'" : "," + "'" + item.Value + "'";
            }

            return selectedvalues;
        }

        private string getPubsListboxText()
        {
            string text = string.Empty;
            string selectedvalues = string.Empty;

            Repeater pubTypeRepeater = UpdatePanel1.FindControl("PubTypeRepeater") as Repeater;

            foreach (RepeaterItem repeaterItem in pubTypeRepeater.Items)
            {
                ListBox pubTypeListBox = repeaterItem.FindControl(PubTypeListBox) as ListBox;

                foreach (ListItem listItem in pubTypeListBox.Items)
                {
                    if (listItem.Selected)
                    {
                        text = listItem.Text;

                        if (text.IndexOf(".") > -1)
                            text = text.Substring(0, text.IndexOf("."));

                        selectedvalues += selectedvalues == string.Empty ? text : "," + text;
                    }
                }
            }

            return selectedvalues;
        }
        
        protected void SelectStateOnRegion()
        {
            SelectStateOnRegion(lstGeoCode, lstState);
        }

        protected void SelectCountryOnRegion()
        {
            SelectCountryOnRegion(lstCountryRegions, lstCountry);
        }

        protected void selectPubsForMarkets()
        {
            var pubTypeRepeater = UpdatePanel1.FindControl(nameof(PubTypeRepeater)) as Repeater;

            if (pubTypeRepeater == null)
            {
                throw new InvalidOperationException($"{nameof(pubTypeRepeater)} is null");
            }

            foreach (RepeaterItem repeaterItem in pubTypeRepeater.Items)
            {
                var listBox = repeaterItem.FindControl(PubTypeListBox) as ListBox;
                listBox?.ClearSelection();
            }

            try
            {
                foreach (DataListItem di in dlDimensions.Items)
                {
                    var lstResponse = (ListBox)di.FindControl(LstResponse);
                    lstResponse?.ClearSelection();
                }

                foreach (ListItem mylistvalue in lstMarket.Items)
                {
                    if (mylistvalue.Selected)
                    {
                        var markets = Objects.Markets.GetByID(Master.clientconnections, IntTryParse(mylistvalue.Value));
                        var doc = new XmlDocument();
                        doc.LoadXml(markets.MarketXML);
                        lpubs = IntTryParse(hfBrandID.Value) > 0 
                                    ? Pubs.GetSearchEnabledByBrandID(Master.clientconnections, IntTryParse(hfBrandID.Value)) 
                                    : Pubs.GetSearchEnabled(Master.clientconnections);
                        var selectedPubType = new Dictionary<int, string>();
                        var node = doc.SelectSingleNode(MarketTypeXPath);
                        SelectPubsForMarketsNodeRepeater(node, IntTryParse, selectedPubType, pubTypeRepeater);
                        var masterGroup = IntTryParse(hfBrandID.Value) > 0 
                                              ? MasterGroup.GetSearchEnabledByBrandID(Master.clientconnections, IntTryParse(hfBrandID.Value)) 
                                              : MasterGroup.GetSearchEnabled(Master.clientconnections);
                        var masterCodeSheets = IntTryParse(hfBrandID.Value) > 0 
                                                   ? MasterCodeSheet.GetSearchEnabledByBrandID(Master.clientconnections, IntTryParse(hfBrandID.Value)) 
                                                   : MasterCodeSheet.GetSearchEnabled(Master.clientconnections);
                        SelectPubsForMarketDataListDimensions(masterGroup, doc, masterCodeSheets);
                        SelectPubsForMarketNodeAdhoc(doc);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }

        private void SelectPubsForMarketsNodeRepeater(XmlNode node, Func<string, int> intTryParse, Dictionary<int, string> selectedPubType, Repeater pubTypeRepeater)
        {
            if (node != null)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    try
                    {
                        var pubTypeId = lpubs.Find(x => x.PubID == intTryParse(child.Attributes?[Id].Value)).PubTypeID;

                        if (selectedPubType.ContainsKey(pubTypeId))
                        {
                            if (child.Attributes != null)
                            {
                                selectedPubType[pubTypeId] = $"{selectedPubType[pubTypeId]},{child.Attributes[Id].Value}";
                            }
                        }
                        else
                        {
                            if (child.Attributes != null)
                            {
                                selectedPubType.Add(pubTypeId, child.Attributes[Id].Value);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError(ex.Message);
                    }
                }
            }

            foreach (RepeaterItem repeaterItem in pubTypeRepeater.Items)
            {
                var hiddenFieldPubTypeId = (HiddenField)repeaterItem.FindControl(HiddenFieldPubTypeId);

                if (selectedPubType.ContainsKey(int.Parse(hiddenFieldPubTypeId.Value)))
                {
                    var pubTypeListBox = repeaterItem.FindControl(PubTypeListBox) as ListBox;
                    var lnkPubTypeShowHide = (LinkButton)repeaterItem.FindControl(LinkPubTypeShowHide);
                    var pnlPubTypeBody = (Panel)repeaterItem.FindControl(PanelPubTypeBody);

                    if (pubTypeListBox != null && pubTypeListBox.Items.Count == 0)
                    {
                        pubTypeListBox.DataSource = from p in lpubs where p.PubTypeID == intTryParse(hiddenFieldPubTypeId.Value) && p.EnableSearching == true select new { p.PubID, p.PubName };
                        pubTypeListBox.DataValueField = PubId;
                        pubTypeListBox.DataTextField = PubName;
                        pubTypeListBox.DataBind();
                    }

                    lnkPubTypeShowHide.Text = TextHide;
                    pnlPubTypeBody.Visible = true;
                    Utilities.SelectFilterListBoxes(pubTypeListBox, selectedPubType[int.Parse(hiddenFieldPubTypeId.Value)]);
                }
            }
        }

        private void SelectPubsForMarketDataListDimensions(List<MasterGroup> masterGroups, XmlDocument doc, List<MasterCodeSheet> mastercodesheet)
        {
            foreach (DataListItem dataListItem in dlDimensions.Items)
            {
                var hiddenFieldMasterGroup = (HiddenField)dataListItem.FindControl(HiddenFieldMasterGroup);
                var lstResponse = (ListBox)dataListItem.FindControl(LstResponse);
                var masterGroup = masterGroups.SingleOrDefault(m => m.MasterGroupID == IntTryParse(hiddenFieldMasterGroup.Value));

                if (masterGroup == null)
                {
                    continue;
                }

                var dnode = doc.SelectSingleNode($"//Market/MarketType[@ID =\'D\']/Group[@ID = \'{masterGroup.ColumnReference}\']");

                if (dnode != null)
                {
                    var lnkDimensionShowHide = (LinkButton)dataListItem.FindControl(LinkDimensionsShowHide);
                    var pnlDimBody = (Panel)dataListItem.FindControl(PanelDimBody);
                    pnlDimBody.Visible = true;

                    if (lstResponse.Items.Count == 0)
                    {
                        lstResponse.DataTextField = MasterDesc;
                        lstResponse.DataValueField = MasterId;
                        lstResponse.DataSource = from m in mastercodesheet
                                                 where m.MasterGroupID == IntTryParse(hiddenFieldMasterGroup.Value)
                                                 orderby m.SortOrder
                                                 select new { m.MasterID, masterdesc = $"{m.MasterDesc}{' '}{'('}{m.MasterValue}{')'}" };
                        lstResponse.DataBind();
                    }

                    lnkDimensionShowHide.Text = TextHide;
                    var selectedValues = new StringBuilder();

                    foreach (XmlNode child in dnode.ChildNodes)
                    {
                        try
                        {
                            selectedValues.Append(!string.IsNullOrWhiteSpace(selectedValues.ToString()) ? $",{child.Attributes?[Id].Value}" : child.Attributes?[Id].Value);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.TraceError(ex.Message);
                        }
                    }

                    Utilities.SelectFilterListBoxes(lstResponse, selectedValues.ToString());
                }
            }
        }

        private void SelectPubsForMarketNodeAdhoc(XmlDocument doc)
        {
            var nodeAdhoc = doc.SelectSingleNode(FilterTypeXPath);

            if (nodeAdhoc == null)
            {
                return;
            }

            foreach (XmlNode nodeEntry in nodeAdhoc.ChildNodes)
            {
                var gridViewCategory = (GridView)AdhocFilter.FindControl(GridViewCategory);

                foreach (GridViewRow gridViewRow in gridViewCategory.Rows)
                {
                    var dataList = gridViewRow.FindControl(DataListAdhocFilter) as DataList;

                    if (dataList == null)
                    {
                        continue;
                    }

                    foreach (DataListItem dataListItem in dataList.Items)
                    {
                        var adhocColumnValue = (Label)dataListItem.FindControl(LabelAdhocColumnValue);
                        var drpAdhocInt = (DropDownList)dataListItem.FindControl(DropDownAdhocInt);
                        var txtAdhocIntFrom = (TextBox)dataListItem.FindControl(TextBoxAdhocIntFrom);
                        var txtAdhocIntTo = (TextBox)dataListItem.FindControl(TextBoxAdhocIntTo);

                        if (adhocColumnValue.Text == nodeEntry.Attributes?[Id].Value)
                        {
                            var firstAttributeValue = nodeEntry.ChildNodes[0].Attributes?[Id].Value;
                            var secondAttributeValue = nodeEntry.ChildNodes[1].Attributes?[Id].Value;

                            if (firstAttributeValue == null || secondAttributeValue == null)
                            {
                                throw new InvalidOperationException($"{nodeEntry.ChildNodes} attributes value is null");
                            }

                            var strValue = firstAttributeValue.Split('|');

                            if (strValue.Length > 1)
                            {
                                txtAdhocIntFrom.Text = strValue[0];
                                txtAdhocIntTo.Text = strValue[1];
                            }

                            txtAdhocIntTo.Enabled = !secondAttributeValue.EqualsAnyIgnoreCase(Equal)
                                                    && !secondAttributeValue.EqualsAnyIgnoreCase(Greater)
                                                    && !secondAttributeValue.EqualsAnyIgnoreCase(Lesser);
                            drpAdhocInt.SelectedIndex = -1;

                            if (drpAdhocInt.Items.FindByValue(secondAttributeValue) != null)
                            {
                                drpAdhocInt.Items.FindByValue(secondAttributeValue).Selected = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        protected void cbIncludePermissions_CheckedChanged(object sender, EventArgs e)
        {
            LoadGrid();

            if (cbIncludePermissions.Checked)
            {
                grdSubReport.Columns[4].Visible = true;
                grdSubReport.Columns[5].Visible = true;
                grdSubReport.Columns[6].Visible = true;
                grdSubReport.Columns[7].Visible = true;
                grdSubReport.Columns[8].Visible = true;
                grdSubReport.Columns[9].Visible = true;
                grdSubReport.Columns[10].Visible = true;
                mdlPopupDimensionReport.Show();
            }
            else
            {
                grdSubReport.Columns[4].Visible = false;
                grdSubReport.Columns[5].Visible = false;
                grdSubReport.Columns[6].Visible = false;
                grdSubReport.Columns[7].Visible = false;
                grdSubReport.Columns[8].Visible = false;
                grdSubReport.Columns[9].Visible = false;
                grdSubReport.Columns[10].Visible = false;
                mdlPopupDimensionReport.Show();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void LoadGrid()
        {
            List<DemographicReport> MCSReportDetails = null;

            if (cbIncludePermissions.Checked)
                MCSReportDetails = DemographicReport.GetMasterDemographicDataWithPermission(Master.clientconnections, Filter.generateCombinationQuery(fc, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, "", 0, 0, Master.clientconnections), Convert.ToInt32(hfReportFor.Value), txtDescription.Text, Convert.ToInt32(hfBrandID.Value), lblSelectedFilterNos.Text + lblSelectedFilterOperation.Text + lblSuppressedFilterNos.Text + lblSuppressedFilterOperation.Text, ViewType == Enums.ViewType.RecencyView ? true : false);
            else
                MCSReportDetails = DemographicReport.GetMasterDemographicData(Master.clientconnections, Filter.generateCombinationQuery(fc, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, "", 0, 0, Master.clientconnections), Convert.ToInt32(hfReportFor.Value), txtDescription.Text, Convert.ToInt32(hfBrandID.Value), lblSelectedFilterNos.Text + lblSelectedFilterOperation.Text + lblSuppressedFilterNos.Text + lblSuppressedFilterOperation.Text, ViewType == Enums.ViewType.RecencyView ? true : false);

            if (txtDescription.Text != string.Empty)
            {
                MCSReportDetails = MCSReportDetails.Where(x => x.Desc.ToLower().Contains(txtDescription.Text.ToLower())).ToList();
            }

            List<DemographicReport> lst = null;

            if (MCSReportDetails != null && MCSReportDetails.Count > 0)
            {
                switch (SortField.ToUpper())
                {
                    case "VALUE":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = MCSReportDetails.OrderBy(o => o.Value).ToList();
                        else
                            lst = MCSReportDetails.OrderByDescending(o => o.Value).ToList();
                        break;

                    case "DESC":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = MCSReportDetails.OrderBy(o => o.Desc).ToList();
                        else
                            lst = MCSReportDetails.OrderByDescending(o => o.Desc).ToList();
                        break;

                    case "DESC1":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = MCSReportDetails.OrderBy(o => o.Desc1).ToList();
                        else
                            lst = MCSReportDetails.OrderByDescending(o => o.Desc1).ToList();
                        break;

                    case "COUNT":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = MCSReportDetails.OrderBy(o => o.Count).ToList();
                        else
                            lst = MCSReportDetails.OrderByDescending(o => o.Count).ToList();
                        break;

                    case "MAIL":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = MCSReportDetails.OrderBy(o => o.Mail).ToList();
                        else
                            lst = MCSReportDetails.OrderByDescending(o => o.Mail).ToList();
                        break;

                    case "EMAIL":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = MCSReportDetails.OrderBy(o => o.Email).ToList();
                        else
                            lst = MCSReportDetails.OrderByDescending(o => o.Email).ToList();
                        break;

                    case "PHONE":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = MCSReportDetails.OrderBy(o => o.Phone).ToList();
                        else
                            lst = MCSReportDetails.OrderByDescending(o => o.Phone).ToList();
                        break;

                    case "MAIL_PHONE":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = MCSReportDetails.OrderBy(o => o.Mail_Phone).ToList();
                        else
                            lst = MCSReportDetails.OrderByDescending(o => o.Mail_Phone).ToList();
                        break;

                    case "EMAIL_PHONE":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = MCSReportDetails.OrderBy(o => o.Email_Phone).ToList();
                        else
                            lst = MCSReportDetails.OrderByDescending(o => o.Email_Phone).ToList();
                        break;

                    case "MAIL_EMAIL":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = MCSReportDetails.OrderBy(o => o.Mail_Email).ToList();
                        else
                            lst = MCSReportDetails.OrderByDescending(o => o.Mail_Email).ToList();
                        break;

                    case "ALL_RECORDS":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = MCSReportDetails.OrderBy(o => o.All_Records).ToList();
                        else
                            lst = MCSReportDetails.OrderByDescending(o => o.All_Records).ToList();
                        break;
                }
            }

            grdSubReport.PageSize = 100;
            grdSubReport.DataSource = lst;
            grdSubReport.DataBind();
            this.mdlPopupDimensionReport.Show();
        }

        protected void grdSubReport_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString() == SortField.ToString())
            {
                switch (SortDirection)
                {
                    case "ASC":
                        SortDirection = "DESC";
                        break;
                    case "DESC":
                        SortDirection = "ASC";
                        break;
                }
            }
            else
            {
                SortField = e.SortExpression;
                SortDirection = "ASC";
            }

            LoadGrid();
        }

        protected void grdSubReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSubReport.PageIndex = e.NewPageIndex;
            LoadGrid();
        }

        protected void btnDownloadDetails_Click(object sender, EventArgs e)
        {
            string strIDs = string.Empty;
            string strIDsAll = string.Empty;
            string IDs = string.Empty;

            foreach (GridViewRow r in grdSubReport.Rows)
            {
                CheckBox cb = r.FindControl(CheckBoxSelectDownload) as CheckBox;

                if (cb.Checked)
                {
                    strIDs += strIDs == string.Empty ? grdSubReport.DataKeys[r.RowIndex].Values[0].ToString() : "," + grdSubReport.DataKeys[r.RowIndex].Values[0].ToString();
                }

                strIDsAll += strIDsAll == string.Empty ? grdSubReport.DataKeys[r.RowIndex].Values[0].ToString() : "," + grdSubReport.DataKeys[r.RowIndex].Values[0].ToString();
            }

            if (strIDs != string.Empty)
            {
                IDs = strIDs;
            }
            else
            {
                IDs = strIDsAll;
            }

            if (IDs.Contains(","))
            {
                if (ViewType == Enums.ViewType.RecencyView)
                {
                    if (Convert.ToInt32(hfBrandID.Value) > 0)
                        lblAddFilters.Text = " s.SubscriptionID in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join vw_BrandConsensus v2 with (nolock) on ss.subscriptionid = v2.subscriptionid join branddetails bd2 ON bd2.BrandID = v2.BrandID WHERE bd2.BrandID = " + hfBrandID.Value + " AND v2.masterid in (" + IDs + "))";
                    else
                        lblAddFilters.Text = " s.subscriptionID in (select distinct vrc2.subscriptionID from vw_RecentConsensus vrc2 where vrc2.masterid in (" + IDs + "))";
                }
                else
                {
                    if (Convert.ToInt32(hfBrandID.Value) > 0)
                        lblAddFilters.Text = " s.SubscriptionID in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join vw_BrandConsensus v2 with (nolock) on ss.subscriptionid = v2.subscriptionid join branddetails bd2 ON bd2.BrandID = v2.BrandID WHERE bd2.BrandID = " + hfBrandID.Value + " AND v2.masterid in (" + IDs + "))";
                    else
                        lblAddFilters.Text = " s.subscriptionID in (select distinct sd2.subscriptionID from SubscriptionDetails sd2 with (nolock) on ss.subscriptionID = sd2.subscriptionID where sd2.masterid in (" + IDs + "))";
                }
            }

            lblMasterID.Text = IDs;
            lblPremissionType.Text = string.Empty;
            lblPermission.Text = "Permission";
            mdlPopupProgress.Hide();
            mdlPopupDimensionReport.Show();

            string addFilters1 = GetAddlFilter();

            FilterViews fv = new FilterViews(Master.clientconnections, Master.LoggedInUser);
            filterView fv1 = new filterView();
            fv1.SelectedFilterNo = lblSelectedFilterNos.Text;
            fv1.SuppressedFilterNo = lblSuppressedFilterNos.Text;
            fv1.SelectedFilterOperation = lblSelectedFilterOperation.Text;
            fv1.SuppressedFilterOperation = lblSuppressedFilterOperation.Text;
            fv.Add(fv1);
            fv.Execute(fc, addFilters1);
            lblDownloadCount.Text = fv.FirstOrDefault().Count.ToString();

            ShowDownloadPanel();
        }

        protected void btnPopupCloseDimension_Click(object sender, EventArgs e)
        {
            ResetDimensionModalControls();
            drpDimension.ClearSelection();
            ResetSavePopupControls();
        }

        private void ResetDimensionModalControls()
        {
            lblPermission.Text = string.Empty;
            lblMasterID.Text = string.Empty;
            lblReportFor.Text = "Demographic Report";
            phReport.Visible = false;
            btnDownloadDetails.Visible = false;
            btnSaveCampaign.Visible = false;
            cbIncludePermissions.Checked = false;
            grdSubReport.DataSource = null;
            grdSubReport.DataBind();
            txtDescription.Text = string.Empty;
            Session.Remove(lblSelectedFilterNos.Text + lblSelectedFilterOperation.Text + lblSuppressedFilterNos.Text + lblSuppressedFilterOperation.Text + "MasterDemographicData" + drpDimension.SelectedValue);
            Session.Remove(lblSelectedFilterNos.Text + lblSelectedFilterOperation.Text + lblSuppressedFilterNos.Text + lblSuppressedFilterOperation.Text + "MasterDemographicDataWithPermission" + drpDimension.SelectedValue);
        }

        protected void lnkDimensionReport_Command(object sender, CommandEventArgs e)
        {
            ResetDimensionModalControls();
            drpDimension.ClearSelection();
            ResetSavePopupControls();

            string[] comargs = e.CommandArgument.ToString().Split('/');
            lblSelectedFilterNos.Text = comargs[0];
            lblSuppressedFilterNos.Text = comargs[1];
            lblSelectedFilterOperation.Text = comargs[2];
            lblSuppressedFilterOperation.Text = comargs[3];
            lblFilterCombination.Text = comargs[4];
            lblDownloadCount.Text = comargs[5];

            List<MasterGroup> masterGroup = new List<MasterGroup>();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
                masterGroup = MasterGroup.GetSubReportEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
            else
                masterGroup = MasterGroup.GetSubReportEnabled(Master.clientconnections);

            drpDimension.DataSource = masterGroup;
            drpDimension.DataBind();
            drpDimension.Items.Insert(0, new ListItem("Select Report", "0"));

            this.mdlPopupDimensionReport.Show();
        }

        protected void drpDimension_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetDimensionModalControls();

            hfReportFor.Value = drpDimension.SelectedValue;

            if (Convert.ToInt32(hfReportFor.Value) > 0)
            {
                lblReportFor.Text = drpDimension.SelectedItem.Text + " Report";

                RenderReport("HTML");
                grdSubReport.Columns[4].Visible = false;
                grdSubReport.Columns[5].Visible = false;
                grdSubReport.Columns[6].Visible = false;
                grdSubReport.Columns[7].Visible = false;
                grdSubReport.Columns[8].Visible = false;
                grdSubReport.Columns[9].Visible = false;
                grdSubReport.Columns[10].Visible = false;

                phReport.Visible = true;
                btnDownloadDetails.Visible = true;

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADExport, KMPlatform.Enums.Access.SaveToCampaign))
                    btnSaveCampaign.Visible = false;
                else
                    btnSaveCampaign.Visible = true;
            }
            else
            {
                lblReportFor.Text = "Demographic Report";
            }

            this.mdlPopupDimensionReport.Show();
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            RenderReport(drpExport.SelectedValue.ToString());
            this.mdlPopupDimensionReport.Show();
        }

        ReportDocument report;

        protected void Page_Unload(object sender, System.EventArgs e)
        {
            if (report != null)
            {
                report.Close();
                report.Dispose();
            }
        }

        private void RenderReport(string exportFormat)
        {
            if (exportFormat.ToUpper() != "HTML")
            {
                string strIDs = string.Empty;

                foreach (GridViewRow r in grdSubReport.Rows)
                {
                    CheckBox cb = r.FindControl(CheckBoxSelectDownload) as CheckBox;

                    if (cb.Checked)
                    {
                        strIDs += strIDs == string.Empty ? grdSubReport.DataKeys[r.RowIndex].Values[0].ToString() : "," + grdSubReport.DataKeys[r.RowIndex].Values[0].ToString();
                    }
                }

                string ReportFor = lblReportFor.Text;

                List<DemographicReport> mcsReportData = null;

                if (cbIncludePermissions.Checked)
                    mcsReportData = DemographicReport.GetMasterDemographicDataWithPermission(Master.clientconnections, Filter.generateCombinationQuery(fc, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, "", 0, 0, Master.clientconnections), Convert.ToInt32(hfReportFor.Value), txtDescription.Text, Convert.ToInt32(hfBrandID.Value), lblSelectedFilterNos.Text + lblSelectedFilterOperation.Text + lblSuppressedFilterNos.Text + lblSuppressedFilterOperation.Text, ViewType == Enums.ViewType.RecencyView ? true : false);
                else
                    mcsReportData = DemographicReport.GetMasterDemographicData(Master.clientconnections, Filter.generateCombinationQuery(fc, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, "", 0, 0, Master.clientconnections), Convert.ToInt32(hfReportFor.Value), txtDescription.Text, Convert.ToInt32(hfBrandID.Value), lblSelectedFilterNos.Text + lblSelectedFilterOperation.Text + lblSuppressedFilterNos.Text + lblSuppressedFilterOperation.Text, ViewType == Enums.ViewType.RecencyView ? true : false);

                if (strIDs != string.Empty)
                {
                    IEnumerable<int> ids = strIDs.Split(',').Select(str => int.Parse(str));
                    mcsReportData = mcsReportData.Where(r => ids.Contains(r.ID)).ToList();
                }

                if (txtDescription.Text != string.Empty)
                {
                    mcsReportData = mcsReportData.Where(x => x.Desc.ToLower().Contains(txtDescription.Text.ToLower())).ToList();
                }

                DataTable dt = ListToDataTable(mcsReportData);

                Hashtable cParams = new Hashtable();
                cParams.Add("@ReportFor", hfReportFor.Value);
                cParams.Add("@Filters", "");
                cParams.Add("@Igrp", "1");

                report = new ReportDocument();
                if (cbIncludePermissions.Checked)
                {
                    report.Load(Server.MapPath("crystalreports/DefaultReport.rpt"));
                }
                else
                {
                    report.Load(Server.MapPath("crystalreports/DefaultReportSTD.rpt"));
                }

                // Setup parameters
                ParameterDiscreteValue crParamDiscreteValue;

                report.SetDataSource(dt);

                //crv.DisplayGroupTree = false;
                crv.ReportSource = report;
                crv.DataBind();

                // ReportFor Param
                ParameterFields pfields = crv.ParameterFieldInfo;
                ParameterField my_report_for_param = new ParameterField();
                my_report_for_param.Name = "reportFor";
                crParamDiscreteValue = new ParameterDiscreteValue();
                crParamDiscreteValue.Value = ReportFor + " REPORT";
                my_report_for_param.CurrentValues.Add(crParamDiscreteValue);
                pfields.Add(my_report_for_param);

                // ConsensusType Param
                ParameterField my_consensus_type_param = new ParameterField();
                my_consensus_type_param.Name = "consensusType";
                crParamDiscreteValue = new ParameterDiscreteValue();
                crParamDiscreteValue.Value = "INDIVIDUAL CONSENSUS";

                my_consensus_type_param.CurrentValues.Add(crParamDiscreteValue);
                pfields.Add(my_consensus_type_param);

                //XML Filter to Pass along
                ParameterField my_display_filter_param = new ParameterField();
                my_display_filter_param.Name = "displayFilter";

                crParamDiscreteValue = new ParameterDiscreteValue();
                crParamDiscreteValue.Value = string.Empty;// getFilters();

                my_display_filter_param.CurrentValues.Add(crParamDiscreteValue);
                pfields.Add(my_display_filter_param);

                //crv.RefreshReport();
                crv.Visible = true;

                report.SetParameterValue("reportFor", ReportFor + " REPORT");
                report.SetParameterValue("consensusType", "INDIVIDUAL CONSENSUS");
                report.SetParameterValue("displayFilter", "");

                CRReport.Export(report, (CRExportEnum)Enum.Parse(typeof(CRExportEnum), exportFormat.ToUpper()), ReportFor + "." + exportFormat);
            }
            else
            {
                grdSubReport.PageIndex = 0;
                LoadGrid();
            }
        }

        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();
            foreach (PropertyInfo info in typeof(T).GetProperties())
                dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                    row[info.Name] = info.GetValue(t, null);
                dt.Rows.Add(row);
            }
            return dt;
        }




        protected void btnCloseCrossTab_Click(object sender, EventArgs e)
        {
            ResetCrossTabModalControls();
            drpCrossTabReport.ClearSelection();
        }

        protected void btnCrossTabXls_Click(object sender, EventArgs e)
        {
            rpgCrossTab.ExportSettings.Excel.Format = (PivotGridExcelFormat)Enum.Parse(typeof(PivotGridExcelFormat), "Xlsx");
            rpgCrossTab.ExportSettings.FileName = lblCrossTabReportName.Text;
            rpgCrossTab.ExportToExcel();

            this.ModalPopupCrossTabReport.Show();
        }

        private void ResetCrossTabModalControls()
        {
            rpgCrossTab.DataSource = null;
            rpgCrossTab.DataBind();
            PhCrossTabReport.Visible = false;
            lblCrossTabReportName.Text = string.Empty;
            hfRow.Value = string.Empty;
            hfColumn.Value = string.Empty;
            hfRowSearchValue.Value = string.Empty;
            hfColumnSearchValue.Value = string.Empty;
            phColFields.Visible = false;
            phColZipFields.Visible = false;
            phRowFields.Visible = false;
            phRowZipFields.Visible = false;
            txtColSearchText.Text = string.Empty;
            txtColZipFrom.Text = string.Empty;
            txtColZipTo.Text = string.Empty;
            txtRowSearchText.Text = string.Empty;
            txtRowZipFrom.Text = string.Empty;
            txtRowZipTo.Text = string.Empty;
            lblColField.Text = string.Empty;
            lblColZipField.Text = string.Empty;
            lblRowField.Text = string.Empty;
            lblRowZipField.Text = string.Empty;
            btnLoadReport.Visible = false;
            lblCrossTabMsg.Visible = false;
            lblCrossTabMsg.Text = string.Empty;
        }

        protected void lnkCrossTabReport_Command(object sender, CommandEventArgs e)
        {
            ResetCrossTabModalControls();
            drpCrossTabReport.ClearSelection();

            string[] comargs = e.CommandArgument.ToString().Split('/');
            lblSelectedFilterNos.Text = comargs[0];
            lblSuppressedFilterNos.Text = comargs[1];
            lblSelectedFilterOperation.Text = comargs[2];
            lblSuppressedFilterOperation.Text = comargs[3];
            lblFilterCombination.Text = comargs[4];
            lblDownloadCount.Text = comargs[5];

            List<CrossTabReport> ctr = new List<CrossTabReport>();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
                ctr = CrossTabReport.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value)).FindAll(x => x.View_Type == Enums.ViewType.ConsensusView);
            else
                ctr = CrossTabReport.GetNotInBrand(Master.clientconnections).FindAll(x => x.View_Type == Enums.ViewType.ConsensusView);

            var query = (from c in ctr
                         where c.PubID == 0
                         select new { CrossTabReportName = c.CrossTabReportName + " (" + c.RowDisplayName + " vs " + c.ColumnDisplayName + ")", c.CrossTabReportID }).ToList();

            drpCrossTabReport.DataSource = query;
            drpCrossTabReport.DataBind();
            drpCrossTabReport.Items.Insert(0, new ListItem("Select Report", "0"));

            PhCrossTabReport.Visible = false;

            this.ModalPopupCrossTabReport.Show();
        }

        protected void drpCrossTabReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetCrossTabModalControls();

            if (Convert.ToInt32(drpCrossTabReport.SelectedValue) > 0)
            {
                CrossTabReport ctr = CrossTabReport.GetByID(Master.clientconnections, Convert.ToInt32(drpCrossTabReport.SelectedValue));

                int i, j;
                List<MasterGroup> mg = MasterGroup.GetAll(Master.clientconnections);
                string Column = string.Empty;
                string Row = string.Empty;

                if (int.TryParse(ctr.Column, out i))
                    Column = mg.Find(x => x.MasterGroupID == Convert.ToInt32(ctr.Column)).DisplayName;
                else
                    Column = ctr.Column;

                if (int.TryParse(ctr.Row, out j))
                    Row = mg.Find(x => x.MasterGroupID == Convert.ToInt32(ctr.Row)).DisplayName;
                else
                    Row = ctr.Row;

                lblCrossTabReportName.Text = Row + " vs " + Column;

                hfRow.Value = ctr.Row.ToString();
                hfColumn.Value = ctr.Column.ToString();

                if (ctr.Column.Equals("Zip", StringComparison.OrdinalIgnoreCase))
                {
                    phColFields.Visible = false;
                    phColZipFields.Visible = true;
                    lblColZipField.Text = ctr.Column;
                }
                else if (ctr.Column.Equals("Title", StringComparison.OrdinalIgnoreCase) || ctr.Column.Equals("Company", StringComparison.OrdinalIgnoreCase) || ctr.Column.Equals("City", StringComparison.OrdinalIgnoreCase))
                {
                    phColFields.Visible = true;
                    phColZipFields.Visible = false;
                    lblColField.Text = ctr.Column;
                }
                else
                {
                    phColFields.Visible = true;
                    phColZipFields.Visible = false;
                    lblColField.Text = Column;
                }

                if (ctr.Row.Equals("Zip", StringComparison.OrdinalIgnoreCase))
                {
                    phRowFields.Visible = false;
                    phRowZipFields.Visible = true;
                    lblColZipField.Text = ctr.Row;
                }
                else if(ctr.Row.Equals("Title", StringComparison.OrdinalIgnoreCase) || ctr.Row.Equals("Company", StringComparison.OrdinalIgnoreCase) || ctr.Row.Equals("City", StringComparison.OrdinalIgnoreCase))
                {
                    phRowFields.Visible = true;
                    phRowZipFields.Visible = false;
                    lblRowField.Text = ctr.Row;
                }
                else
                {
                    phRowFields.Visible = true;
                    phRowZipFields.Visible = false;
                    lblRowField.Text = Row;
                }

                btnLoadReport.Visible = true;
            }

            this.ModalPopupCrossTabReport.Show();
        }

        protected void btnLoadReport_Click(object sender, EventArgs e)
        {
            PhCrossTabReport.Visible = true;
            LoadCrossTabGrid();
            this.ModalPopupCrossTabReport.Show();
        }

        protected void LoadCrossTabGrid()
        {
            IList<CrossTab> MCSReportDetails;

            int i;

            XmlDocument xmlColDoc = new XmlDocument();
            XmlElement xmlColNode = xmlColDoc.CreateElement("XML");
            xmlColDoc.AppendChild(xmlColNode);

            if (int.TryParse(hfColumn.Value, out i))
            {
                XmlElement xmlColType;
                xmlColType = xmlColDoc.CreateElement("ColType");
                xmlColType.InnerText = "MasterGroup";
                xmlColNode.AppendChild(xmlColType);

                XmlElement xmlColMasterGroupID;
                xmlColMasterGroupID = xmlColDoc.CreateElement("ColMasterGroupID");
                xmlColMasterGroupID.InnerText = hfColumn.Value;
                xmlColNode.AppendChild(xmlColMasterGroupID);

                XmlElement xmlColFilters;
                xmlColFilters = xmlColDoc.CreateElement("ColFilters");
                xmlColFilters.InnerText = txtColSearchText.Text;
                hfColumnSearchValue.Value = txtColSearchText.Text;
                xmlColNode.AppendChild(xmlColFilters);
            }
            else
            {
                XmlElement xmlColType;
                xmlColType = xmlColDoc.CreateElement("ColType");
                xmlColType.InnerText = "Profile";
                xmlColNode.AppendChild(xmlColType);

                XmlElement xmlColField;
                xmlColField = xmlColDoc.CreateElement("ColField");
                xmlColField.InnerText = hfColumn.Value;
                xmlColNode.AppendChild(xmlColField);

                XmlElement xmlColFilters;
                xmlColFilters = xmlColDoc.CreateElement("ColFilters");

                if (hfColumn.Value.Equals("Zip", StringComparison.OrdinalIgnoreCase))
                {
                    xmlColFilters.InnerText = txtColZipFrom.Text + "|" + txtColZipTo.Text;
                    hfColumnSearchValue.Value = txtColZipFrom.Text + "|" + txtColZipTo.Text;
                }
                else
                {
                    xmlColFilters.InnerText = txtColSearchText.Text;
                    hfColumnSearchValue.Value = txtColSearchText.Text;
                }

                xmlColNode.AppendChild(xmlColFilters);
            }

            XmlDocument xmlRowDoc = new XmlDocument();
            XmlElement xmlRowNode = xmlRowDoc.CreateElement("XML");
            xmlRowDoc.AppendChild(xmlRowNode);

            if (int.TryParse(hfRow.Value, out i))
            {
                XmlElement xmlRowType;
                xmlRowType = xmlRowDoc.CreateElement("RowType");
                xmlRowType.InnerText = "MasterGroup";
                xmlRowNode.AppendChild(xmlRowType);

                XmlElement xmlRowMasterGroupID;
                xmlRowMasterGroupID = xmlRowDoc.CreateElement("RowMasterGroupID");
                xmlRowMasterGroupID.InnerText = hfRow.Value;
                xmlRowNode.AppendChild(xmlRowMasterGroupID);

                XmlElement xmlRowFilters;
                xmlRowFilters = xmlRowDoc.CreateElement("RowFilters");
                xmlRowFilters.InnerText = txtRowSearchText.Text;
                hfRowSearchValue.Value = txtRowSearchText.Text;
                xmlRowNode.AppendChild(xmlRowFilters);
            }
            else
            {
                XmlElement xmlRowType;
                xmlRowType = xmlRowDoc.CreateElement("RowType");
                xmlRowType.InnerText = "Profile";
                xmlRowNode.AppendChild(xmlRowType);

                XmlElement xmlRowField;
                xmlRowField = xmlRowDoc.CreateElement("RowField");
                xmlRowField.InnerText = hfRow.Value;
                xmlRowNode.AppendChild(xmlRowField);

                XmlElement xmlRowFilters;
                xmlRowFilters = xmlRowDoc.CreateElement("RowFilters");

                if (hfRow.Value.Equals("Zip", StringComparison.OrdinalIgnoreCase))
                {
                    xmlRowFilters.InnerText = txtRowZipFrom.Text + "|" + txtRowZipTo.Text;
                    hfRowSearchValue.Value = txtRowZipFrom.Text + "|" + txtRowZipTo.Text;
                }
                else
                {
                    xmlRowFilters.InnerText = txtRowSearchText.Text;
                    hfRowSearchValue.Value = txtRowSearchText.Text;
                }

                xmlRowNode.AppendChild(xmlRowFilters);
            }

            MCSReportDetails = CrossTab.GetMasterCrossTabData(Master.clientconnections, Filter.generateCombinationQuery(fc, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, "", 0, 0, Master.clientconnections), xmlColDoc.OuterXml.ToString(), xmlRowDoc.OuterXml.ToString(), Convert.ToInt32(hfBrandID.Value), ViewType == Enums.ViewType.RecencyView ? true : false);

            rpgCrossTab.DataSource = MCSReportDetails;
        }

        protected void rpgCrossTab_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
        {
            LoadCrossTabGrid();
        }

        protected void rpgCrossTab_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
        {
            try
            {
                PivotGridDataCell cell = e.Cell as PivotGridDataCell;

                if (cell != null)
                {
                    if (!cell.IsGrandTotalCell && !cell.IsTotalCell)
                    {
                        LinkButton button = cell.FindControl("lnkCrossTabCounts") as LinkButton;

                        button.CommandArgument = cell.ParentRowIndexes[0] + "|" + cell.ParentColumnIndexes[0] + "|" + button.Text;

                        if (cell.DataItem == null || cell.DataItem.ToString() == "0")
                        {
                            button.Enabled = false;
                        }
                    }
                    else
                    {
                        if (cell.IsGrandTotalCell && cell.CellType.Equals(PivotGridDataCellType.ColumnTotalDataCell | PivotGridDataCellType.RowTotalDataCell))
                        {
                            LinkButton button = cell.FindControl("lnkCrossTabColumnTotalCounts") as LinkButton;

                            button.CommandArgument = cell.ParentRowIndexes[0] + "|" + cell.ParentColumnIndexes[0] + "|" + button.Text;
                        }
                        else if (cell.IsGrandTotalCell && cell.CellType.Equals(PivotGridDataCellType.ColumnGrandTotalDataCell))
                        {
                            LinkButton button = cell.FindControl("lnkCrossTabRowTotalCounts") as LinkButton;

                            button.CommandArgument = cell.ParentRowIndexes[0] + "|" + cell.ParentColumnIndexes[0] + "|" + button.Text;
                        }
                    }
                }

            }
            catch
            {

            }
        }

        protected void lnkCrossTabReportCounts_Command(object sender, CommandEventArgs e)
        {
            this.ModalPopupCrossTabReport.Show();
            lblPermission.Text = "CrossTab";
            lblMasterID.Text = string.Empty;
            string[] args = e.CommandArgument.ToString().Split('|');
            hfSelectedCrossTabLink.Value = e.CommandArgument.ToString();
            lblDownloadCount.Text = args[2];
            lblIsPopupCrossTab.Text = "true";
            lblIsPopupDimension.Text = "false";
            ShowDownloadPanel();
        }


        protected void lnkGeoReport_Command(object sender, CommandEventArgs e)
        {
            string[] args = e.CommandArgument.ToString().Split('/');
            lblSelectedFilterNos.Text = args[0];
            lblSuppressedFilterNos.Text = args[1];
            lblSelectedFilterOperation.Text = args[2];
            lblSuppressedFilterOperation.Text = args[3];
            lblFilterCombination.Text = args[4];
            lblDownloadCount.Text = args[5];

            loadReportGeoData(e.CommandName, Filter.generateCombinationQuery(fc, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, "", 0, 0, Master.clientconnections));

            ReportViewer1.Visible = true;
            this.mdlPopupGeo.Show();
        }

        protected void btnCloseGeoMap_Click(object sender, EventArgs e)
        {
            this.mdlPopupGeo.Hide();
        }

        protected void lnkGeoMaps_Command(object sender, CommandEventArgs e)
        {
            string[] args = e.CommandArgument.ToString().Split('/');
            DataTable locations = GetMap(Filter.generateCombinationQuery(fc, args[2], args[3], args[0], args[1], "", 0, 0, Master.clientconnections));

            if (locations.Rows.Count > 0)
            {
                string dt = GetJSONString(locations);
                myMapCoords.Text = dt;
            }
            this.mdlPopupGeoMap.Show();
        }

        private string GetJSONString(DataTable Dt)
        {
            string[] StrDc = new string[Dt.Columns.Count];
            string HeadStr = string.Empty;

            for (int i = 0; i < Dt.Columns.Count; i++)
            {
                StrDc[i] = Dt.Columns[i].Caption;

                HeadStr += "\"" + StrDc[i] + "\":\"" + StrDc[i] + i.ToString() + "¾" + "\",";
            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);

            StringBuilder Sb = new StringBuilder();
            Sb.Append("{\"" + Dt.TableName + "\":[");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string TempStr = HeadStr;
                Sb.Append("{");

                for (int j = 0; j < Dt.Columns.Count; j++)
                {
                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString());
                }

                Sb.Append(TempStr + "},");
            }

            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
            Sb.Append("]}");

            return Sb.ToString();
        }

        private DataTable GetMap(StringBuilder Queries)
        {
            SqlCommand cmd = new SqlCommand("sp_GetSubscriberGLBySubscriptionID", DataFunctions.GetClientSqlConnection(Master.clientconnections));
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@Queries", SqlDbType.Text)).Value = Queries.ToString();

            DataTable dt1 = new DataTable();
            dt1 = DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(Master.clientconnections));

            string blue = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/KMPS.MD/Images/blue-pin.png";
            List<MapItem> listMap = new List<MapItem>();
            foreach (DataRow dr in dt1.Rows)
            {
                MapItem mi = new MapItem();
                //mi.SubscriberID = Convert.ToInt32(dr["SubscriberID"].ToString());
                mi.SubscriberID = dr["SubscriberID"].ToString();
                mi.Latitude = TruncateDecimal(Convert.ToDecimal(dr["Latitude"].ToString()), 6);
                mi.Longitude = TruncateDecimal(Convert.ToDecimal(dr["Longitude"].ToString()), 6);
                //mi.MarkerImage = blue;
                listMap.Add(mi);
            }
            listMap = listMap.OrderBy(x => x.ZipCode).ToList();
            DataTable dtNew = MapStops(listMap);
            return dtNew;
        }

        public decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            int tmp = (int)Math.Truncate(step * value);
            return tmp / step;
        }

        private DataTable MapStops(List<MapItem> items)
        {
            //SubscriberID, MapAddress, Latitude, Longitude, '/Images/blue-pin.png' AS 'markerImage', ZipCode 
            DataTable dtNew = new DataTable();
            DataColumn dcSubscriberID = new DataColumn("Sc");
            dtNew.Columns.Add(dcSubscriberID);
            //DataColumn dcMapAddress = new DataColumn("MapAddress");
            //dtNew.Columns.Add(dcMapAddress);
            DataColumn dcLatitude = new DataColumn("Lt");
            dtNew.Columns.Add(dcLatitude);
            DataColumn dcLongitude = new DataColumn("Lg");
            dtNew.Columns.Add(dcLongitude);
            //DataColumn dcmarkerImage = new DataColumn("MI");
            //dtNew.Columns.Add(dcmarkerImage);
            //DataColumn dcZipCode = new DataColumn("ZipCode");
            //dtNew.Columns.Add(dcZipCode);
            dtNew.AcceptChanges();
            foreach (MapItem mi in items)
            {
                DataRow dr = dtNew.NewRow();
                dr["Sc"] = mi.SubscriberID;
                //dr["MapAddress"] = mi.MapAddress;
                dr["Lt"] = mi.Latitude;
                dr["Lg"] = mi.Longitude;
                //dr["MI"] = mi.MarkerImage;
                //dr["ZipCode"] = mi.ZipCode;
                dtNew.Rows.Add(dr);
            }
            dtNew.AcceptChanges();

            StringBuilder sb = new StringBuilder();
            Dictionary<string, object> resultMain = new Dictionary<string, object>();
            int index = 0;
            foreach (DataRow dr in dtNew.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dtNew.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                resultMain.Add(index.ToString(), result);
                index++;
            }
            dtNew.TableName = "MapPoints";

            return dtNew;
        }

        protected void loadReportGeoData(string type, StringBuilder Queries)
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.DataSources.Clear();
            switch (type)
            {
                case "GeoCanada":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoBreakdown_Canada.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetGeographicalReportData(Master.clientconnections, "sp_rpt_Qualified_Breakdown_Canada", Queries)));
                    break;
                case "GeoDomestic":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoBreakdown_Domestic.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetGeographicalReportData(Master.clientconnections, "sp_rpt_Qualified_Breakdown_Domestic", Queries)));
                    break;
                case "GeoInternational":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoBreakdown_by_Country.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetGeographicalReportData(Master.clientconnections, "sp_rpt_Qualified_Breakdown_by_country", Queries)));
                    break;
                case "GeoPermissionCanada":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoPermission_Canada.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetGeographicalReportData(Master.clientconnections, "sp_rpt_Qualified_Breakdown_Canada", Queries)));
                    break;
                case "GeoPermissionDomestic":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoPermission_Domestic.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetGeographicalReportData(Master.clientconnections, "sp_rpt_Qualified_Breakdown_Domestic", Queries)));
                    break;
                case "GeoPermissionInternational":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoPermission_by_Country.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetGeographicalReportData(Master.clientconnections, "sp_rpt_Qualified_Breakdown_by_country", Queries)));
                    break;
            }
            ReportViewer1.LocalReport.Refresh();
        }


        protected void lnkCompanyLocationView_Command(object sender, CommandEventArgs e)
        {
            lblPermission.Text = string.Empty;
            lblMasterID.Text = string.Empty;
            string[] args = e.CommandArgument.ToString().Split('/');
            lblSelectedFilterNos.Text = args[0];
            lblSuppressedFilterNos.Text = args[1];
            lblSelectedFilterOperation.Text = args[2];
            lblSuppressedFilterOperation.Text = args[3];
            lblFilterCombination.Text = args[4];
            lblDownloadCount.Text = args[5];
            ShowCLDownloadPanel();
        }

        protected void lnkEmailView_Command(object sender, CommandEventArgs e)
        {
            lblPermission.Text = string.Empty;
            lblMasterID.Text = string.Empty;
            string[] args = e.CommandArgument.ToString().Split('/');
            lblSelectedFilterNos.Text = args[0];
            lblSuppressedFilterNos.Text = args[1];
            lblSelectedFilterOperation.Text = args[2];
            lblSuppressedFilterOperation.Text = args[3];
            lblFilterCombination.Text = args[4];
            lblDownloadCount.Text = args[5];
            ShowEVDownloadPanel();
        }


        protected void btnResetAll_Click(object sender, EventArgs e)
        {
            txtShowQuery.Text = string.Empty;
            txtShowQuery.Visible = false;

            fc.Clear();
            FilterCollection.Clear();
            ResetAllFitlerControls();
            LoadGridFilters();

            if (pnlBrand.Visible)
                drpBrand.Enabled = true;

            if (pnlDataCompare.Visible)
            {
                drpDataCompareSourceFile.Enabled = true;
                rblDataCompareOperation.Enabled = true;
            }

            TabContainer.ActiveTabIndex = 0;
        }

        private void ResetAllFitlerControls()
        {
            pnlDataCompare.Visible = false;
            drpDataCompareSourceFile.SelectedIndex = -1;
            hfDataCompareProcessCode.Value = string.Empty;
            ResetDataCompareControls();

            ResetFilterControls();
        }

        private void ResetDataCompareControls()
        {
            hfDataCompareLinkSelected.Value = string.Empty;
            plDataCompareData.Visible = false;
            hfDcTargetCodeID.Value = "0";
            hfDcRunID.Value = "0";
            hfDcViewID.Value = "0";
            drpIsBillable.SelectedIndex = -1;
            lblTotalFileRecords.Text = string.Empty;
            lnkMatchedRecords.Text = string.Empty;
            lnkNonMatchedRecords.Text = string.Empty;
            lnkMatchedNotIn.Text = string.Empty;
            lnkMatchedNotIn.Visible = false;
            lnkMatchedNotInSummary.Visible = false;
            lblMatchedRecordHeader.Text = string.Empty;
            lblNonMatchedRecordsHeader.Text = string.Empty;
            lblMatchedNotInHeader.Text = string.Empty;
            lblMatchedNotInHeader.Visible = false;
            plNotes.Visible = false;
            txtNotes.Text = string.Empty;
        }

        private void ResetFilterControls()
        {
            ResetFilterTabControls();
            FilterSegmentation.ResetControls();
        }

        private void ResetFilterTabControls()
        {
            Repeater pubTypeRepeater = UpdatePanel1.FindControl("PubTypeRepeater") as Repeater;
            foreach (RepeaterItem repeaterItem in pubTypeRepeater.Items)
            {
                ListBox listBox = repeaterItem.FindControl(PubTypeListBox) as ListBox;
                listBox.ClearSelection();
            }

            lstMarket.ClearSelection();
            ClearAndResetFilterTabControls();

            lblSelectedFilterNos.Text = string.Empty;
            lblSuppressedFilterNos.Text = string.Empty;
            lblSelectedFilterOperation.Text = string.Empty;
            lblSuppressedFilterOperation.Text = string.Empty;
            lblFilterCombination.Text = string.Empty;
            lblDownloadCount.Text = "0";
            lblIsPopupCrossTab.Text = "false";
            lblIsPopupDimension.Text = "false";
        }


        protected void lstMarket_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectPubsForMarkets();
        }

        protected void lstGeoCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstState.ClearSelection();
            SelectStateOnRegion();
        }

        protected void lstCountryRegions_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstCountry.ClearSelection();
            SelectCountryOnRegion();
        }

        protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDrpCountrySelectedIndexChanged(RadMtxtboxZipCode, drpCountry);
        }

        protected void btnOpenSaveFilterPopup_Click(object sender, EventArgs e)
        {
            if (!HandleBtnOpenSaveFilterPopupClick(grdFilters, FilterSave, hfBrandID.Value, ViewType))
            {
                DisplayError("Please select a checkbox.");
                bool ischecked = false;
                string FilterNos = string.Empty;
                int i = 0;
                foreach (GridViewRow r in grdFilters.Rows)
                {
                    CheckBox cb = r.FindControl("cbSelectFilter") as CheckBox;

                    if (cb != null && cb.Checked)
                    {
                        ischecked = true;
                        FilterNos += FilterNos == string.Empty ? grdFilters.DataKeys[i].Values["FilterNo"].ToString() : "," + grdFilters.DataKeys[i].Values["FilterNo"].ToString();
                    }
                    i++;
                }

                if (!ischecked)
                {
                    DisplayError(SelectCheckBox);
                    return;
                }

                FilterSave.Mode = "AddNew";
                FilterSave.BrandID = Convert.ToInt32(hfBrandID.Value);
                FilterSave.PubID = 0;
                FilterSave.FilterIDs = FilterNos;
                FilterSave.UserID = Master.LoggedInUser;
                FilterSave.FilterCollections = fc;
                FilterSave.ViewType = ViewType;
                FilterSave.LoadControls();
                FilterSave.Visible = true;
            }
        }

        protected void lnkDimensionShowHide_Command(object sender, CommandEventArgs e)
        {
            string[] args = e.CommandArgument.ToString().Split('|');

            if (args[1].ToString().ToUpper() == "MARKETS")
            {
                if (lnkMarketShowHide.Text == "(Show...)")
                {
                    if (lstMarket.Items.Count == 0)
                    {
                        List<KMPS.MD.Objects.Markets> markets = new List<KMPS.MD.Objects.Markets>();

                        if (Convert.ToInt32(hfBrandID.Value) > 0)
                            markets = Objects.Markets.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                        else
                            markets = Objects.Markets.GetNotInBrand(Master.clientconnections);

                        lstMarket.DataSource = markets;
                        lstMarket.DataBind();
                    }

                    lnkMarketShowHide.Text = TextHide;
                    pnlMarket.Visible = true;
                }
                else
                {
                    lnkMarketShowHide.Text = "(Show...)";
                    pnlMarket.Visible = false;
                }
            }
            else if (args[1].ToString().ToUpper() == "PUBTYPE")
            {
                LinkButton lnkPubTypeShowHide = (LinkButton)((Repeater)UpdatePanel1.FindControl("PubTypeRepeater")).Items[int.Parse(e.CommandName)].FindControl(LinkPubTypeShowHide);
                Panel pnlPubTypeBody = (Panel)((Repeater)UpdatePanel1.FindControl("PubTypeRepeater")).Items[int.Parse(e.CommandName)].FindControl(PanelPubTypeBody);

                if (lnkPubTypeShowHide.Text == "(Show...)")
                {
                  
                    ListBox PubTypeListBox = (ListBox)((Repeater)UpdatePanel1.FindControl("PubTypeRepeater")).Items[int.Parse(e.CommandName)].FindControl("PubTypeListBox");
                    if (PubTypeListBox.Items.Count == 0)
                    {
                        int pubTypeID = Convert.ToInt32(args[0]);

                        // get pubs          
                        if (Convert.ToInt32(hfBrandID.Value) > 0)
                            lpubs = Pubs.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                        else
                            lpubs = Pubs.GetSearchEnabled(Master.clientconnections);

                        var pubsQuery = (from p in lpubs
                                         where p.PubTypeID == pubTypeID && p.EnableSearching == true
                                         select new { p.PubID, p.PubName });

                        PubTypeListBox.DataSource = pubsQuery;
                        PubTypeListBox.DataValueField = PubId;
                        PubTypeListBox.DataTextField = PubName;
                        PubTypeListBox.DataBind();
                    }

                    lnkPubTypeShowHide.Text = TextHide;
                    pnlPubTypeBody.Visible = true;
                }
                else
                {
                    lnkPubTypeShowHide.Text = "(Show...)";
                    pnlPubTypeBody.Visible = false;
                }
            }
            else
            {
                LinkButton lnkDimensionShowHide = (LinkButton)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(e.CommandName)].FindControl(LinkDimensionsShowHide);
                Panel pnlDimBody = (Panel)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(e.CommandName)].FindControl(PanelDimBody);

                if (lnkDimensionShowHide.Text == "(Show...)")
                {
                    pnlDimBody.Visible = true;
                    ListBox lstResponse = (ListBox)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(e.CommandName)].FindControl(LstResponse);

                    if (lstResponse.Items.Count == 0)
                    {
                        lstResponse.DataTextField = MasterDesc;
                        lstResponse.DataValueField = MasterId;

                        List<MasterCodeSheet> mastercodesheet = new List<MasterCodeSheet>();

                        if (Convert.ToInt32(hfBrandID.Value) > 0)
                            mastercodesheet = MasterCodeSheet.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                        else
                            mastercodesheet = MasterCodeSheet.GetSearchEnabled(Master.clientconnections);

                        var MasterCodeSheetQuery = (from m in mastercodesheet
                                                    where m.MasterGroupID == Convert.ToInt32(args[0])
                                                    orderby m.SortOrder ascending
                                                    select new { m.MasterID, masterdesc = m.MasterDesc + ' ' + '(' + m.MasterValue + ')' });

                        lstResponse.DataSource = MasterCodeSheetQuery;
                        lstResponse.DataBind();
                    }

                    lnkDimensionShowHide.Text = TextHide;
                }
                else
                {
                    lnkDimensionShowHide.Text = "(Show...)";
                    pnlDimBody.Visible = false;
                }
            }
        }

        protected void lnkPopup_Command(object sender, CommandEventArgs e)
        {
            rlbDimensionAvailable.Items.Clear();
            rlbDimensionSelected.Items.Clear();
            rtbDimSearch.Text = string.Empty;

            lblDimensionControl.Text = e.CommandName;

            ListBox lst = (ListBox)UpdatePanel1.FindControl(e.CommandName);
            string type = string.Empty;

            if (lst == null)
            {
                string[] args = e.CommandArgument.ToString().Split('|');
                lblDimensionType.Text = args[1].ToString().ToUpper();
                type = args[1].ToString().ToUpper();
                lblDimension.Text = args[2];

                if (args[1].ToString().ToUpper() == "PUBTYPE")
                {
                    lst = (ListBox)((Repeater)UpdatePanel1.FindControl("PubTypeRepeater")).Items[int.Parse(e.CommandName)].FindControl(PubTypeListBox);

                    if (lst.Items.Count == 0)
                    {
                        int pubTypeID = Convert.ToInt32(args[0]);

                        // get pubs          
                        if (Convert.ToInt32(hfBrandID.Value) > 0)
                            lpubs = Pubs.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                        else
                            lpubs = Pubs.GetSearchEnabled(Master.clientconnections);

                        var pubsQuery = (from p in lpubs
                                         where p.PubTypeID == pubTypeID && p.EnableSearching == true
                                         select new { p.PubID, p.PubName });

                        lst.DataSource = pubsQuery;
                        lst.DataValueField = PubId;
                        lst.DataTextField = PubName;
                        lst.DataBind();
                    }
                }
                else if (args[1].ToString().ToUpper() == "DIMENSION")
                {
                    lst = (ListBox)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(e.CommandName)].FindControl(LstResponse);

                    if (lst.Items.Count == 0)
                    {
                        lst.DataTextField = MasterDesc;
                        lst.DataValueField = MasterId;

                        List<MasterCodeSheet> mastercodesheet = new List<MasterCodeSheet>();

                        if (Convert.ToInt32(hfBrandID.Value) > 0)
                            mastercodesheet = MasterCodeSheet.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                        else
                            mastercodesheet = MasterCodeSheet.GetSearchEnabled(Master.clientconnections);

                        var MasterCodeSheetQuery = (from m in mastercodesheet
                                                    where m.MasterGroupID == Convert.ToInt32(args[0])
                                                    orderby m.SortOrder ascending
                                                    select new { m.MasterID, masterdesc = m.MasterDesc + ' ' + '(' + m.MasterValue + ')' });

                        lst.DataSource = MasterCodeSheetQuery;
                        lst.DataBind();
                    }
                }

                if (args[1].ToString().ToUpper() == "DIMENSION")
                    hfMasterValue.Value = args[0];
            }
            else
            {
                lblDimension.Text = e.CommandArgument.ToString();

                if (e.CommandArgument.ToString().ToUpper() == "MARKETS")
                {
                    lblDimensionType.Text = e.CommandArgument.ToString().ToUpper();
                    type = e.CommandArgument.ToString().ToUpper();

                    if (lst.Items.Count == 0)
                    {
                        List<KMPS.MD.Objects.Markets> markets = new List<KMPS.MD.Objects.Markets>();

                        if (Convert.ToInt32(hfBrandID.Value) > 0)
                            markets = Objects.Markets.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                        else
                            markets = Objects.Markets.GetNotInBrand(Master.clientconnections);

                        lst.DataSource = markets;
                        lst.DataBind();
                    }
                }
            }

            if (type.ToUpper() == "DIMENSION")
            {
                lnkSortByDescription.Visible = true;
                lnkSortByValue.Visible = true;
                rtbDimSearch.Visible = true;
                rlbDimensionSelected.Visible = true;
                rlbDimensionAvailable.AllowTransfer = true;
                rlbDimensionAvailable.TransferToID = "rlbDimensionSelected";
                rlbDimensionAvailable.Width = Unit.Pixel(465);
                rlbDimensionAvailable.ButtonSettings.AreaWidth = Unit.Pixel(35);

                foreach (ListItem li in lst.Items)
                {
                    if (li.Selected)
                        rlbDimensionSelected.Items.Add(new RadListBoxItem(li.Text, li.Value));
                    else
                        rlbDimensionAvailable.Items.Add(new RadListBoxItem(li.Text, li.Value));
                }
            }
            else if (lblDimensionControl.Text.ToUpper() == "LSTMARKET" || type.ToUpper() == "PUBTYPE")
            {
                lnkSortByDescription.Visible = false;
                lnkSortByValue.Visible = false;
                rtbDimSearch.Visible = true;
                rlbDimensionSelected.Visible = true;
                rlbDimensionAvailable.AllowTransfer = true;
                rlbDimensionAvailable.TransferToID = "rlbDimensionSelected";
                rlbDimensionAvailable.Width = Unit.Pixel(465);
                rlbDimensionAvailable.ButtonSettings.AreaWidth = Unit.Pixel(35);

                foreach (ListItem li in lst.Items)
                {
                    if (li.Selected)
                        rlbDimensionSelected.Items.Add(new RadListBoxItem(li.Text, li.Value));
                    else
                        rlbDimensionAvailable.Items.Add(new RadListBoxItem(li.Text, li.Value));
                }
            }
            else
            {
                lnkSortByDescription.Visible = false;
                lnkSortByValue.Visible = false;
                rtbDimSearch.Visible = true;
                rlbDimensionSelected.Visible = false;
                rlbDimensionAvailable.AllowTransfer = false;
                rlbDimensionAvailable.TransferToID = "";
                rlbDimensionAvailable.Width = Unit.Pixel(900);
                rlbDimensionAvailable.ButtonSettings.AreaWidth = Unit.Pixel(0);

                foreach (ListItem li in lst.Items)
                {
                    rlbDimensionAvailable.Items.Add(new RadListBoxItem(li.Text, li.Value));

                    if (li.Selected)
                    {
                        rlbDimensionAvailable.FindItemByValue(li.Value).Selected = true;
                    }
                }
            }

            if (lblDimension.Text == "Country Regions")
            {
                lstCountry.ClearSelection();
                SelectCountryOnRegion();
            }

            this.mdlPopupDimension.Show();
        }

        protected void lnkSort_Command(object sender, CommandEventArgs e)
        {
            rtbDimSearch.Text = "";

            if (e.CommandName.Equals("SortByDescription", StringComparison.OrdinalIgnoreCase))
            {
                rlbDimensionAvailable.Sort = RadListBoxSort.Ascending;
                rlbDimensionAvailable.SortItems();
            }
            else if (e.CommandName.Equals("SortByValue", StringComparison.OrdinalIgnoreCase))
            {
                List<string> selectedID = new List<string>();
                RadListBox lst = (RadListBox)rlbDimensionAvailable;

                selectedID = lst.GetSelectedIndices()
                       .Select(j => lst.Items[j].Value)
                       .ToList();

                List<MasterCodeSheet> mastercodesheet = new List<MasterCodeSheet>();

                if (Convert.ToInt32(hfBrandID.Value) > 0)
                    mastercodesheet = MasterCodeSheet.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                else
                    mastercodesheet = MasterCodeSheet.GetSearchEnabled(Master.clientconnections);

                var MasterCodeSheetQuery = (from m in mastercodesheet
                                            where m.MasterGroupID == Convert.ToInt32(hfMasterValue.Value)
                                            orderby m.MasterValue ascending
                                            select new { m.MasterID, masterdesc = m.MasterDesc + ' ' + '(' + m.MasterValue + ')', m.MasterValue }).AsEnumerable().OrderBy(s => s.MasterValue, new CustomComparer<string>()).ToList();

                rlbDimensionAvailable.DataValueField = MasterId;
                rlbDimensionAvailable.DataTextField = "masterdesc";
                rlbDimensionAvailable.DataSource = MasterCodeSheetQuery;
                rlbDimensionAvailable.DataBind();

                if (lblDimensionType.Text == "DIMENSION")
                {
                    foreach (RadListBoxItem li in rlbDimensionSelected.Items)
                    {
                        RadListBoxItem itemToRemove = rlbDimensionAvailable.FindItemByValue(li.Value);
                        rlbDimensionAvailable.Items.Remove(itemToRemove);
                    }
                }
                else
                {
                    foreach (string l in selectedID)
                    {
                        rlbDimensionAvailable.FindItemByValue(l).Selected = true;
                    }
                }
            }

            this.mdlPopupDimension.Show();
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            ListBox lst = (ListBox)UpdatePanel1.FindControl(lblDimensionControl.Text);

            if (lst == null)
            {
                if (lblDimensionType.Text == "PUBTYPE")
                {
                    lst = (ListBox)((Repeater)UpdatePanel1.FindControl("PubTypeRepeater")).Items[int.Parse(lblDimensionControl.Text)].FindControl(PubTypeListBox);
                    LinkButton lnkPubTypeShowHide = (LinkButton)((Repeater)UpdatePanel1.FindControl("PubTypeRepeater")).Items[int.Parse(lblDimensionControl.Text)].FindControl(LinkPubTypeShowHide);
                    Panel pnlPubTypeBody = (Panel)((Repeater)UpdatePanel1.FindControl("PubTypeRepeater")).Items[int.Parse(lblDimensionControl.Text)].FindControl(PanelPubTypeBody);

                    lnkPubTypeShowHide.Text = TextHide;
                    pnlPubTypeBody.Visible = true;

                }
                else if (lblDimensionType.Text == "DIMENSION")
                {
                    lst = (ListBox)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(lblDimensionControl.Text)].FindControl(LstResponse);
                    LinkButton lnkDimensionShowHide = (LinkButton)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(lblDimensionControl.Text)].FindControl(LinkDimensionsShowHide);
                    Panel pnlDimBody = (Panel)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(lblDimensionControl.Text)].FindControl(PanelDimBody);

                    lnkDimensionShowHide.Text = TextHide;
                    pnlDimBody.Visible = true;
                }
            }
            else
            {
                if (lblDimensionType.Text == "MARKETS")
                {
                    lnkMarketShowHide.Text = TextHide;
                    pnlMarket.Visible = true;
                }
            }

            lst.ClearSelection();

            if (rlbDimensionSelected.Visible == true)
            {
                foreach (RadListBoxItem li in rlbDimensionSelected.Items)
                {
                    lst.Items.FindByValue(li.Value).Selected = true;
                }
            }
            else
            {
                foreach (RadListBoxItem li in rlbDimensionAvailable.Items)
                {
                    if (li.Selected)
                        lst.Items.FindByValue(li.Value).Selected = true;
                }
            }

            if (lblDimension.Text.Equals("Markets", StringComparison.OrdinalIgnoreCase))
                selectPubsForMarkets();

            if (lblDimension.Text.Equals("Country Regions", StringComparison.OrdinalIgnoreCase))
            {
                lstCountry.ClearSelection();
                SelectCountryOnRegion();
            }
        }


        protected void lnkAdhoc_Command(object sender, CommandEventArgs e)
        {
            AdhocFilter.Visible = true;
            AdhocFilter.LoadControls();
        }



        protected void lnkActivity_Command(object sender, CommandEventArgs e)
        {
            ActivityFilter.Visible = true;
            RadioButtonList rblOpenSearchType = (RadioButtonList)ActivityFilter.FindControl("rblOpenSearchType");
            rblOpenSearchType.Visible = true;
            RadioButtonList rblClickSearchType = (RadioButtonList)ActivityFilter.FindControl("rblClickSearchType");
            rblClickSearchType.Visible = true;
            ActivityFilter.Visible = true;
            Panel pnlOpenSearchType = (Panel)ActivityFilter.FindControl("pnlOpenSearchType");
            pnlOpenSearchType.Visible = true;
            Panel pnlClickSearchType = (Panel)ActivityFilter.FindControl("pnlClickSearchType");
            pnlClickSearchType.Visible = true;
            ActivityFilter.LoadControls();
        }



        protected void lnkCirculation_Command(object sender, CommandEventArgs e)
        {
            CirculationFilter.Visible = true;
            CirculationFilter.ViewType = ViewType;
            CirculationFilter.LoadCirculationControls();
        }



        protected void lnkSavedFilter_Command(object sender, CommandEventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.View))
            {
                ShowFilter.ShowFilterSegmentation = true;
                ShowFilter.BrandID = Convert.ToInt32(hfBrandID.Value);
                ShowFilter.PubID = 0;
                ShowFilter.UserID = Master.LoggedInUser;
                ShowFilter.ViewType = ViewType;
                ShowFilter.Mode = "Load";
                ShowFilter.AllowMultiRowSelection = true;
                ShowFilter.LoadControls();
                ShowFilter.Visible = true;
            }
        }



        protected void btnSaveToCampaign_Click(object sender, EventArgs e)
        {
            mdlPopupProgress.Hide();
            mdlPopupSaveCampaign.Show();
            mdlPopupDimensionReport.Show();
        }

        protected void rbPopupNewCampaign_CheckedChanged(object sender, EventArgs e)
        {
            plPopupNewCampaign.Visible = true;
            plPopupExistingCampaign.Visible = false;

            PopupCampaignFilterID = 0;
            PopupCampaignID = 0;
            PopupCampaignFilterAllID = 0;

            mdlPopupDimensionReport.Show();
            this.mdlPopupSaveCampaign.Show();
        }

        protected void rbPopupExistingCampaign_CheckedChanged(object sender, EventArgs e)
        {
            plPopupNewCampaign.Visible = false;
            plPopupExistingCampaign.Visible = true;

            PopupCampaignFilterID = 0;
            PopupCampaignID = 0;
            PopupCampaignFilterAllID = 0;

            drpPopupCampaignName.ClearSelection();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
                drpPopupCampaignName.DataSource = Campaigns.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
            else
                drpPopupCampaignName.DataSource = Campaigns.GetNotInBrand(Master.clientconnections);

            drpPopupCampaignName.DataBind();
            drpPopupCampaignName.Items.Insert(0, new ListItem("", "0"));

            mdlPopupDimensionReport.Show();
            this.mdlPopupSaveCampaign.Show();
        }

        protected void btnPopupCloseCampaign_Click(object sender, EventArgs e)
        {
            ResetSavePopupControls();
            mdlPopupDimensionReport.Show();
            this.mdlPopupSaveCampaign.Hide();
        }

        protected void btnPopupSaveCampaign_click(object sender, EventArgs e)
        {
            var conn = DataFunctions.GetClientSqlConnection(Master.clientconnections);
            var ischecked = false;

            foreach (GridViewRow gridViewRow in grdSubReport.Rows)
            {
                var checkBox = gridViewRow.FindControl(CheckBoxSelectDownload) as CheckBox;

                if (checkBox?.Checked == true)
                {
                    ischecked = true;
                }
            }

            if (!ischecked)
            {
                DisplaySaveCampaignPopupError(SelectCheckBox);
                mdlPopupSaveCampaign.Show();
                mdlPopupDimensionReport.Show();

                return;
            }

            if (SaveCampaignGridView())
            {
                return;
            }

            lblPopupResult.Visible = true;
            lblPopupResult.Text = $"Total subscriber in the campaign : {Campaigns.GetCountByCampaignID(Master.clientconnections, PopupCampaignID)}";
            PopupCampaignID = 0;
            mdlPopupDimensionReport.Show();
            mdlPopupSaveCampaign.Show();
        }

        private bool SaveCampaignGridView()
        {
            foreach (GridViewRow gridViewRow in grdSubReport.Rows)
            {
                var checkBox = gridViewRow.FindControl(CheckBoxSelectDownload) as CheckBox;

                if (checkBox?.Checked == true)
                {
                    var addlFilters = new StringBuilder();

                    if (grdSubReport?.DataKeys?[gridViewRow.RowIndex]?.Values == null)
                    {
                        throw new InvalidOperationException($"{nameof(grdSubReport)} is null");
                    }

                    if (IntTryParse(hfBrandID.Value) > 0)
                    {
                        addlFilters.Append(" and s.SubscriptionID in (select distinct ss.subscriptionID from Subscriptions ss with (nolock) join vw_BrandConsensus v2 with (nolock) ")
                            .Append("on ss.subscriptionid = v2.subscriptionid join branddetails bd2 ON bd2.BrandID = v2.BrandID WHERE bd2.BrandID = ")
                            .Append($"{hfBrandID.Value} AND v2.masterid in ({grdSubReport.DataKeys[gridViewRow.RowIndex].Values[0]}))");
                    }
                    else
                    {
                        addlFilters.Append(" and s.subscriptionID in (select distinct vrc2.subscriptionID from ");
                        addlFilters.Append(
                            ViewType == Enums.ViewType.RecencyView
                                ? "vw_RecentConsensus vrc2 where vrc2.masterid in ("
                                : "SubscriptionDetails sd2 with (nolock) where sd2.masterid in (");
                        addlFilters.Append(grdSubReport.DataKeys[gridViewRow.RowIndex].Values[0]).Append("))");
                    }

                    var query = Filter.generateCombinationQuery(
                        fc,
                        lblSelectedFilterOperation.Text,
                        lblSuppressedFilterOperation.Text,
                        lblSelectedFilterNos.Text,
                        lblSuppressedFilterNos.Text,
                        addlFilters.ToString(),
                        0,
                        0,
                        Master.clientconnections);
                    var dataTable = Subscriber.GetSubscriberData(Master.clientconnections, query, "s.SubscriptionID");

                    if (SaveCampaignDetails(checkBox, dataTable, gridViewRow))
                    {
                        return true;
                    }

                    PopupCampaignFilterAllID = 0;
                }
            }

            return false;
        }

        private bool SaveCampaignDetails(CheckBox checkBox, DataTable dataTable, GridViewRow gridViewRow)
        {
            if (checkBox.Checked && dataTable.Rows.Count > 0)
            {
                if (rbPopupExistingCampaign.Checked)
                {
                    PopupCampaignID = IntTryParse(drpPopupCampaignName.SelectedValue);
                }
                else if (rbPopupNewCampaign.Checked)
                {
                    if (PopupCampaignID == 0)
                    {
                        if (Campaigns.CampaignExists(Master.clientconnections, txtPopupCampaignName.Text) == 0)
                        {
                            PopupCampaignID = Campaigns.Insert(Master.clientconnections, txtPopupCampaignName.Text, Master.LoggedInUser, IntTryParse(hfBrandID.Value));
                        }
                        else
                        {
                            DisplaySaveCampaignPopupError($"ERROR - <font color=\'#000000\'>\"{txtPopupCampaignName.Text}\"</font> already exists. Please enter a different name.");
                            mdlPopupSaveCampaign.Show();
                            mdlPopupDimensionReport.Show();

                            return true;
                        }
                    }
                }
                else
                {
                    DisplaySaveCampaignPopupError(SelectExistingOrNewCampaign);
                    mdlPopupSaveCampaign.Show();
                    mdlPopupDimensionReport.Show();

                    return true;
                }

                if (PopupCampaignFilterID == 0)
                {
                    var filtername = grdSubReport.DataKeys[gridViewRow.RowIndex].Values[2].ToString();
                    PopupCampaignFilterID = CampaignFilter.CampaignFilterExists(Master.clientconnections, filtername, PopupCampaignID);

                    if (CampaignFilter.CampaignFilterExists(Master.clientconnections, filtername, PopupCampaignID) == 0)
                    {
                        PopupCampaignFilterID = CampaignFilter.Insert(Master.clientconnections, filtername, Master.LoggedInUser, PopupCampaignID, txtPromocode.Text);
                    }
                }

                var xmlSubscriber = new StringBuilder(string.Empty);
                var cnt = 0;
                var listNth = Utilities.getNth(dataTable.Rows.Count, dataTable.Rows.Count);

                try
                {
                    foreach (var n in listNth)
                    {
                        var dataRow = dataTable.Rows[n];
                        xmlSubscriber.Append($"<sID>{Utilities.cleanXMLString(dataRow[SubscriptionId].ToString())}</sID>");

                        if ((cnt != 0 && cnt % 10000 == 0) || cnt == listNth.Count - 1)
                        {
                            CampaignFilterDetails.saveCampaignDetails(Master.clientconnections, PopupCampaignFilterID, $"<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{xmlSubscriber}</XML>");
                            xmlSubscriber = new StringBuilder(string.Empty);
                        }

                        cnt++;
                    }
                }
                catch (Exception ex)
                {
                    DisplaySaveCampaignPopupError($"ERROR - {ex.Message}");
                }

                PopupCampaignFilterID = 0;
            }

            return false;
        }

        private void ResetSavePopupControls()
        {
            plPopupNewCampaign.Visible = false;
            plPopupExistingCampaign.Visible = false;
            txtPopupCampaignName.Text = string.Empty;
            rbPopupExistingCampaign.Checked = false;
            rbPopupNewCampaign.Checked = false;
            drpPopupCampaignName.ClearSelection();
            divSaveCampaignPopupError.Visible = false;
            lblSaveCampaignPopupError.Text = string.Empty;
            lblPopupResult.Text = string.Empty;
        }

        protected void TabContainer_OnActiveTabChanged(object sender, EventArgs e)
        {
            if (TabContainer.ActiveTabIndex == 1)
            {
                FilterSegmentation.Visible = true;
                FilterSegmentation.ViewType = ViewType;
                FilterSegmentation.UserID = Master.LoggedInUser;
                FilterSegmentation.fcSessionName = fcSessionName;
                FilterSegmentation.BrandID = Convert.ToInt32(hfBrandID.Value);
            }
            else if (TabContainer.ActiveTabIndex == 2)
            {
                grdCrossTabIntersect.DataSource = null;
                grdCrossTabIntersect.DataBind();

                grdFilterInterSectCount.DataSource = null;
                grdFilterInterSectCount.DataBind();

                grdCrossTabUnion.DataSource = null;
                grdCrossTabUnion.DataBind();

                grdFilterUnionCount.DataSource = null;
                grdFilterUnionCount.DataBind();

                grdCrossTabNotIn.DataSource = null;
                grdCrossTabNotIn.DataBind();

                grdFilterNotInCount.DataSource = null;
                grdFilterNotInCount.DataBind();

                if (fc.Count > 1)
                {
                    if (fc.FilterComboList == null || fc.FilterComboList.Count == 0)
                    {
                        fc.Execute();
                    }

                    //Intersect
                    DataTable dtIntersect = fc.GetIntersectCrossTabData();

                    if (dtIntersect.Rows.Count > 0)
                        grdCrossTabIntersectRowCount = dtIntersect.Rows.Count;
                    else
                        grdCrossTabIntersectRowCount = 0;

                    grdCrossTabIntersect.DataSource = dtIntersect;
                    grdCrossTabIntersect.DataBind();
                    grdCrossTabIntersect.Visible = true;

                    grdFilterInterSectCount.DataSource = fc.FilterComboList.Where(x => x.SelectedFilterOperation.ToUpper() == "INTERSECT");
                    grdFilterInterSectCount.DataBind();
                    grdFilterInterSectCount.Visible = true;

                    // Union
                    DataTable dtUnion = fc.GetUnionCrossTabData();

                    if (dtUnion.Rows.Count > 0)
                        grdCrossTabUnionRowCount = dtUnion.Rows.Count;
                    else
                        grdCrossTabUnionRowCount = 0;

                    grdCrossTabUnion.DataSource = dtUnion;
                    grdCrossTabUnion.DataBind();
                    grdCrossTabUnion.Visible = true;

                    grdFilterUnionCount.DataSource = fc.FilterComboList.Where(x => x.SelectedFilterOperation.ToUpper() == "UNION");
                    grdFilterUnionCount.DataBind();
                    grdFilterUnionCount.Visible = true;

                    //NotIn
                    DataTable dtNotIn = fc.GetNotInCrossTabData();

                    grdCrossTabNotIn.DataSource = dtNotIn;
                    grdCrossTabNotIn.DataBind();
                    grdCrossTabNotIn.Visible = true;

                    grdFilterNotInCount.DataSource = fc.FilterComboList.Where(x => x.SuppressedFilterNo != "" && x.FilterDescription.ToUpper() != "ALL INTERSECT" && x.FilterDescription.ToUpper() != "ALL UNION");
                    grdFilterNotInCount.DataBind();
                    grdFilterNotInCount.Visible = true;
                }
            }
        }

        protected void grdCrossTabIntersect_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int totalcellcount = e.Row.Cells.Count;

                if (e.Row.RowIndex < grdCrossTabIntersectRowCount - 1)
                    e.Row.Cells[totalcellcount - 1].Text = "";
                else
                {
                    for (int i = 1; i < e.Row.Cells.Count - 1; i++)
                    {
                        e.Row.Cells[i].Text = "";
                    }
                }
            }
        }

        protected void grdCrossTabUnion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int totalcellcount = e.Row.Cells.Count;

                if (e.Row.RowIndex < grdCrossTabUnionRowCount - 1)
                    e.Row.Cells[totalcellcount - 1].Text = "";
                else
                {
                    for (int i = 1; i < e.Row.Cells.Count - 1; i++)
                    {
                        e.Row.Cells[i].Text = "";
                    }
                }
            }
        }

        protected void rblLoadType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblLoadType.SelectedValue.Equals("Manual Load", StringComparison.OrdinalIgnoreCase))
            {
                btnLoadComboVenn.Visible = true;
            }
            else
            {
                btnLoadComboVenn.Visible = false;
            }
        }

        protected void btnLoadComboVenn_Click(object sender, EventArgs e)
        {
            if (rblLoadType.SelectedValue.Equals("Manual Load", StringComparison.OrdinalIgnoreCase))
            {
                fc.Execute();

                grdFilterCounts.DataSource = fc.FilterComboList.Where(x => (x.SelectedFilterNo.Split(',').Length > 1 ? Convert.ToInt32(x.SelectedFilterNo.Split(',')[1]) <= 5 : true && Convert.ToInt32(x.SelectedFilterNo.Split(',')[0]) <= 5) && (x.SuppressedFilterNo == null || x.SuppressedFilterNo == "" ? true : Convert.ToInt32(x.SuppressedFilterNo) <= 5));
                grdFilterCounts.DataBind();
                grdFilterCounts.Visible = true;
                
                ctrlVenn1.CreateVenn(fc);
                ctrlVenn1.Visible = true;
            }
        }

        protected override void LoadPageFilters()
        {
            loadProductandDimensions();
            int brandId;
            if (Int32.TryParse(hfBrandID.Value, out brandId))
            {
                AdhocFilter.BrandID = brandId;
                AdhocFilter.LoadAdhocGrid();
            }
        }

        protected override void ShowBrandUI(Brand brand)
        {
            lblColon.Visible = true;
            
            if (brand.Logo != string.Empty)
            {
                var customerID = Master.UserSession.CustomerID;
                imglogo.ImageUrl = $"../Images/logo/{customerID}/{brand.Logo}";
                imglogo.Visible = true;
            }
        }
    }
}

