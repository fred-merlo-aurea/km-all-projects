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
using AjaxControlToolkit;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using FrameworkUAD.BusinessLogic;
using FrameworkUAS.BusinessLogic;
using KM.Common.Extensions;
using KMPlatform.Object;
using KMPS.MD.Controls;
using KMPS.MD.Objects;
using Microsoft.Reporting.WebForms;
using Telerik.Web.UI;
using Brand = KMPS.MD.Objects.Brand;
using CampaignFilter = KMPS.MD.Objects.CampaignFilter;
using CodeSheet = KMPS.MD.Objects.CodeSheet;
using Enums = KMPS.MD.Objects.Enums;
using EmailStatus = KMPS.MD.Objects.EmailStatus;
using Filter = KMPS.MD.Objects.Filter;
using ResponseGroup = KMPS.MD.Objects.ResponseGroup;
using Subscriber = KMPS.MD.Objects.Subscriber;
using ViewType = KMPS.MD.Objects.Enums.ViewType;
using MDControls = KMPS.MD.Controls;

namespace KMPS.MD.Main
{
    public partial class ProductView : AudienceViewBase
    {
        protected override Panel PnlDataCompare => pnlDataCompare;
        protected override DropDownList DrpDataCompareSourceFile => drpDataCompareSourceFile;
        protected override RadioButtonList RblLoadType => rblLoadType;
        protected override RadioButtonList RblDataCompareOperation => rblDataCompareOperation;
        protected override Panel BrandPanel => pnlBrand;
        protected override DropDownList BrandDropDown => drpBrand;
        protected override HiddenField BrandIdHiddenField => hfBrandID;
        protected override Label BrandNameLabel => lblBrandName;
        protected override ListBox LstCountryRegions => lstCountryRegions;
        protected override ListBox LstGeoCode => lstGeoCode;
        protected override ListBox LstState => lstState;
        protected override ListBox LstCountry => lstCountry;
        protected override RadComboBox RcbEmail => rcbEmail;
        protected override RadComboBox RcbPhone => rcbPhone;
        protected override RadComboBox RcbFax => rcbFax;
        protected override RadComboBox RcbIsLatLonValid => null;
        protected override RadComboBox RcbMailPermission => rcbMailPermission;
        protected override DataList DataListDimensions => dlDimensions;
        protected override RadComboBox RcbMedia => rcbMedia;
        protected override RadComboBox RcbFaxPermission => rcbFaxPermission;
        protected override RadComboBox RcbPhonePermission => rcbPhonePermission;
        protected override RadComboBox RcbOtherProductsPermission => rcbOtherProductsPermission;
        protected override RadComboBox RcbThirdPartyPermission => rcbThirdPartyPermission;
        protected override RadComboBox RcbEmailRenewPermission => rcbEmailRenewPermission;
        protected override RadComboBox RcbTextPermission => rcbTextPermission;
        protected override Controls.Adhoc AdhocFilterBase => AdhocFilter;
        protected override Controls.Activity ActivityFilterBase => ActivityFilter;
        protected override Controls.Circulation CirculationFilterBase => CirculationFilter;
        protected override DropDownList DrpCountry => drpCountry;
        protected override RadMaskedTextBox RadMaskedTxtBoxZipCode => RadMtxtboxZipCode;
        protected override RadComboBox RcbEmailStatus => rcbEmailStatus;
        protected override TextBox TxtRadiusMin => txtRadiusMin;
        protected override TextBox TxtRadiusMax => txtRadiusMax;
        protected override RadMaskedTextBox RadMtxtboxZipCodeCtrl => RadMtxtboxZipCode;
        protected override Activity ActivityFilterCtrl => ActivityFilter;
        protected override MDControls.Adhoc AdhocFilterCtrl => AdhocFilter;
        protected override Circulation CirculationFilterCtrl => CirculationFilter;
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

        private void FilterSaveHide()
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
                CheckBox cb = r.FindControl("chkSelectFilter") as CheckBox;
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
                    FilterSegmentation.ViewType = Enums.ViewType.ProductView; 
                    FilterSegmentation.UserID = Master.LoggedInUser;
                    FilterSegmentation.fcSessionName = fcSessionName;
                    FilterSegmentation.PubID = Convert.ToInt32(hfProductID.Value);
                    FilterSegmentation.BrandID = Convert.ToInt32(hfBrandID.Value);
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
                DownloadPanel1.ViewType = Enums.ViewType.ProductView;
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
                Enums.ViewType.ProductView,
                lblFilterCombination.Text,
                downloadCount,
                plDataCompareData.Visible,
                dcRunID,
                dcTypeCodeID,
                dcTargetCodeID,
                matchedRecordsCount,
                nonMatchedRecordsCount,
                totalFileRecords);
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
                Enums.ViewType.ProductView,
                lblFilterCombination.Text,
                downloadCount,
                plDataCompareData.Visible,
                dcRunID,
                dcTypeCodeID,
                dcTargetCodeID,
                matchedRecordsCount,
                nonMatchedRecordsCount,
                totalFileRecords);
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
                            Queries.Append("<xml><Queries>");
                            Queries.Append(string.Format("<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>", 1,
                                            " select distinct 1,  s.SubscriptionID " +
                                            " from DataCompareProfile d with(nolock) " +
                                            " join Subscriptions s with(Nolock) on d.IGRP_NO = s.IGrp_No " +
                                            " join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID " +
                                            " where d.ProcessCode = '" + hfDataCompareProcessCode.Value + "' " +
                                            " and ps.PubID = " + Convert.ToInt32(hfProductID.Value)
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
                                            "        where d1.ProcessCode = '" + hfDataCompareProcessCode.Value + "' " +
                                            "        and ps.PubID = " + Convert.ToInt32(hfProductID.Value) +
                                            "    ) x on s.SubscriptionID = x.SubscriptionID " +
                                            "    where d.ProcessCode = '" + hfDataCompareProcessCode.Value + "' and " +
                                            "    x.SubscriptionID is null "
                                            ));
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
                if (int.TryParse(drpCrossTabReport.SelectedValue, out selectedId) && selectedId > 0)
                {
                    query = GetCrossTabFilters(query);
                }
            }

            return query; 
        }

        private string GetPermissionFilters()
        {
            string query;
            if (lblCodeSheetID.Text == "-1")
            {
                query = string.Concat(" join (select distinct ps2.subscriptionID from PubSubscriptions ps2 join PubSubscriptionDetail psd2 ",
                    "on ps2.SubscriptionID = psd2.SubscriptionID join codesheet c on c.codesheetID = psd2.codesheetID where c.ResponseGroupID = ",
                    hfReportFor.Value);
            }
            else
            {
                query = string.Concat(" join (select distinct ps2.subscriptionID from PubSubscriptions ps2 join PubSubscriptionDetail psd2 ",
                    "on ps2.SubscriptionID = psd2.SubscriptionID where psd2.CodesheetID in (",
                    lblCodeSheetID.Text,
                    ")");
            }

            switch (lblPremissionType.Text)
                {
                    case "MAIL":
                        query += " and MailPermission=1";
                        break;
                    case "EMAIL":
                        query += " and OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps2.email, '') != ''";
                        break;
                    case "PHONE":
                        query += " and PhonePermission=1 and isnull(ps2.Phone, '') != ''";
                        break;
                    case "MAIL_PHONE":
                        query += " and MailPermission=1 and PhonePermission=1 and isnull(ps2.Phone, '') != ''";
                        break;
                    case "EMAIL_PHONE":
                        query += " and PhonePermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps2.email, '') != '' and isnull(ps2.Phone, '') != ''";
                        break;
                    case "MAIL_EMAIL":
                        query += " and MailPermission = 1 and OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps2.email, '') != ''";
                        break;
                    case "ALL_RECORDS":
                        query += " and MailPermission=1 and PhonePermission=1 and OtherProductsPermission=1 and EmailRenewPermission=1 and isnull(ps2.email, '') != '' and isnull(ps2.Phone, '') != ''";
                        break;
                }

                query += ") x5 on x5.SubscriptionID = s.SubscriptionID";

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
                case "Company":
                case "Title":
                case "City":
                case "State":
                case "Zip":
                case "Country":
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
                        if (hfRow.Value.Equals("Country", StringComparison.OrdinalIgnoreCase))
                        {
                            query = " join (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps2.CountryID where ct.ShortName = '" +
                                crossTab + "'";
                        }
                        else
                        {
                            query = " join (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) where ps2." + hfRow.Value +
                                    " = '" + crossTab + "'";
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
            if (hfRow.Value.Equals("Zip", StringComparison.OrdinalIgnoreCase))
            {
                var zipRange = hfRowSearchValue.Value.Split(VBarChar);

                query = " join (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) where substring(ps2.ZipCode,1,5) between '" +
                    zipRange[0] + "' and '" + zipRange[1] + "'";
                query += " union select SubscriptionID from PubSubscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) where substring(ps2.ZipCode,1,5) between '" +
                    zipRange[0] + "' and '" + zipRange[1] + "')";
            }
            else if (hfRow.Value.Equals("State", StringComparison.OrdinalIgnoreCase))
            {
                query = " join (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps2.CountryID where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES'";
                query += " union select SubscriptionID from PubSubscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps2.CountryID where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES')";
            }
            else if (hfRow.Value.Equals("Country", StringComparison.OrdinalIgnoreCase))
            {
                query = " join (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps2.CountryID";
                query += " union select SubscriptionID from PubSubscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps2.CountryID)";
            }
            else
            {
                rowSearch = true;

                if (hfRowSearchValue.Value == string.Empty)
                {
                    query += " CROSS APPLY (SELECT TOP 100 " + hfRow.Value +
                             ", SubscriptionID from PubSubscriptions WITH (nolock) where subscriptionID = ps.SubscriptionID GROUP BY company , SubscriptionID  ORDER BY Count(subscriptionid) DESC ) x6 on x6.SubscriptionID = s.SubscriptionID";
                    query += " union SELECT TOP 100 " + hfRow.Value +
                             ", SubscriptionID from PubSubscriptions WITH (nolock) where subscriptionID = ps.SubscriptionID and " +
                             hfRow.Value + " is null GROUP BY " + hfRow.Value + " , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                }
                else
                {
                    var strRowSearchValue = hfRowSearchValue.Value.Split(CommaChar);
                    var whereCondition = "(";

                    for (var i = 0; i < strRowSearchValue.Length; i++)
                    {
                        whereCondition += $"{(i > 0 ? " OR " : string.Empty)}{hfRow.Value} like '%{strRowSearchValue[i].Replace("_", "[_]")}%'";
                    }

                    whereCondition += ")";

                    query += " CROSS APPLY (SELECT TOP 100 " + hfRow.Value +
                             ", SubscriptionID from PubSubscriptions WITH (nolock) where subscriptionID = ps.SubscriptionID and " +
                             whereCondition + " GROUP BY company , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                    query += " union SELECT TOP 100 " + hfRow.Value +
                             ", SubscriptionID from PubSubscriptions WITH (nolock) where subscriptionID = ps.SubscriptionID and " +
                             whereCondition + " GROUP BY company , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                }
            }

            return query;
        }

        private string GetCrossTabRowGeoNoResponseQuery(string query, ref bool rowSearch)
        {
            if (hfRow.Value.Equals("Zip", StringComparison.OrdinalIgnoreCase))
            {
                var zipRange = hfRowSearchValue.Value.Split(VBarChar);

                query = " join (select SubscriptionID from PubSubscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) where substring(ps2.ZipCode,1,5) between '" +
                    zipRange[0] + "' and '" + zipRange[1] + "')";
            }
            else if (hfRow.Value.Equals("State", StringComparison.OrdinalIgnoreCase))
            {
                query = " join (select SubscriptionID from PubSubscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps2.CountryID where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES')";
            }
            else if (hfRow.Value.Equals("Country", StringComparison.OrdinalIgnoreCase))
            {
                query = " join (select SubscriptionID from PubSubscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps2.CountryID)";
            }
            else
            {
                rowSearch = true;

                if (hfRowSearchValue.Value == string.Empty)
                    query += " CROSS APPLY (SELECT TOP 100 " + hfRow.Value +
                             ", SubscriptionID from PubSubscriptions WITH (nolock) where subscriptionID = ps.SubscriptionID and " +
                             hfRow.Value + " is null GROUP BY " + hfRow.Value + " , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                else
                {
                    var strRowSearchValue = hfRowSearchValue.Value.Split(CommaChar);
                    var whereCondition = "(";

                    for (var i = 0; i < strRowSearchValue.Length; i++)
                    {
                        var escaped = strRowSearchValue[i].Replace("_", "[_]");
                        whereCondition += $"{(i > 0 ? " OR " : string.Empty)}{hfRow.Value} not like '%{escaped}%'";
                    }

                    whereCondition += ")";

                    query += " CROSS APPLY (SELECT TOP 100 " + hfRow.Value +
                             ", SubscriptionID from PubSubscriptions WITH (nolock) where subscriptionID = ps.SubscriptionID and " +
                             whereCondition + " GROUP BY company , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                }
            }

            return query;
        }

        private string GetCrossTabRowDefaultQuery(string query, string crossTab)
        {
            if (crossTab.Equals(NameNoResponse, StringComparison.OrdinalIgnoreCase))
            {
                var whereConditionDesc = string.Empty;
                if (hfRowSearchValue.Value != string.Empty)
                {
                    var strRowSearchValue = hfRowSearchValue.Value.Split(CommaChar);
                    whereConditionDesc = "(";

                    for (var i = 0; i < strRowSearchValue.Length; i++)
                    {
                        var escaped = strRowSearchValue[i].Replace("_", "[_]");
                        whereConditionDesc += $"{(i > 0 ? " OR " : string.Empty)} responsedesc  like '%{escaped}%'";
                    }

                    whereConditionDesc += ")";
                }

                if (hfRowSearchValue.Value == string.Empty)
                {
                    query = " join (select SubscriptionID from Subscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 join PubSubscriptionDetail psd2 on ps2.SubscriptionID = psd2.SubscriptionID join codesheet c on c.codesheetID = psd2.codesheetID where c.ResponseGroupID = " +
                        hfRow.Value + ")";
                }
                else
                {
                    query = " join (select SubscriptionID from Subscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 join PubSubscriptionDetail psd2 on ps2.SubscriptionID = psd2.SubscriptionID join codesheet c on c.codesheetID = psd2.codesheetID where " +
                        whereConditionDesc + ")";
                }
            }
            else if (crossTab.Equals(NameGrandTotal, StringComparison.OrdinalIgnoreCase))
            {
                query = " join (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join PubSubscriptionDetail psd2 WITH (nolock) on ps2.SubscriptionID  = psd2.SubscriptionID join CodeSheet c with (nolock) on c.codesheetID = psd2.codesheetID  where c.ResponseGroupID = " +
                    hfRow.Value;
                query += " union select SubscriptionID from Subscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 join PubSubscriptionDetail psd2 on ps2.SubscriptionID = psd2.SubscriptionID join codesheet c on c.codesheetID = psd2.codesheetID where c.ResponseGroupID = " +
                    hfRow.Value + ")";
            }
            else
            {
                query = " join (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join PubSubscriptionDetail psd2 WITH (nolock) on ps2.SubscriptionID  = psd2.SubscriptionID join CodeSheet c with (nolock) on c.codesheetID = psd2.codesheetID  where c.ResponseDesc = '" +
                    crossTab + "' and c.ResponseGroupID = " + hfRow.Value;
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
                            query += " join (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps2.CountryID where ct.ShortName = '" +
                                crossTab + "'";
                        }
                        else
                        {
                            query += " join (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) where ps2." + hfColumn.Value +
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

                query += " join (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) where substring(ps2.ZipCode,1,5) between '" +
                    zipRange[0] + "' and '" + zipRange[1] + "'";
                query += " union select SubscriptionID from PubSubscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) where substring(ps2.ZipCode,1,5) between '" +
                    zipRange[0] + "' and '" + zipRange[1] + "')";
            }
            else if (hfColumn.Value.Equals(NameState, StringComparison.OrdinalIgnoreCase))
            {
                query += " join (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps2.CountryID where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES'";
                query += " union select SubscriptionID from PubSubscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps2.CountryID where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES')";
            }
            else if (hfColumn.Value.Equals(NameCountry, StringComparison.OrdinalIgnoreCase))
            {
                query += " join (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps2.CountryID";
                query += " union select SubscriptionID from PubSubscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps2.CountryID)";
            }
            else
            {
                columnSearch = true;
                if (hfColumnSearchValue.Value == string.Empty)
                {
                    query += " CROSS APPLY (SELECT TOP 100 " + hfColumn.Value +
                             ", SubscriptionID from PubSubscriptions WITH (nolock) where subscriptionID = ps.SubscriptionID GROUP BY company , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                    query += " union SELECT TOP 100 " + hfColumn.Value +
                             ", SubscriptionID from PubSubscriptions WITH (nolock) where subscriptionID = ps.SubscriptionID and " +
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
                        whereCondition += $"{(i > 0 ? " OR " : string.Empty)}{hfColumn.Value} like '%{escaped}%'";
                    }

                    whereCondition += ")";

                    query += " CROSS APPLY (SELECT TOP 100 " + hfColumn.Value +
                             ", SubscriptionID from PubSubscriptions WITH (nolock) where subscriptionID = ps.SubscriptionID and " +
                             whereCondition + " GROUP BY company , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";

                    var notWhereCondition = "(";

                    for (var i = 0; i < strColumnSearchValue.Length; i++)
                    {
                        var escaped = strColumnSearchValue[i].Replace("_", "[_]");
                        notWhereCondition += $"{(i > 0 ? " OR " : string.Empty)}{hfColumn.Value} not like '%{escaped}%'";
                    }

                    notWhereCondition += ")";

                    query += " union SELECT TOP 100 " + hfColumn.Value +
                             ", SubscriptionID from PubSubscriptions WITH (nolock) where subscriptionID = ps.SubscriptionID and " +
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

                query += " join (select SubscriptionID from PubSubscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) where substring(ps2.ZipCode,1,5) between '" +
                    zipRange[0] + "' and '" + zipRange[1] + "')";
            }
            else if (hfColumn.Value.Equals(NameState, StringComparison.OrdinalIgnoreCase))
            {
                query += " join (select SubscriptionID from PubSubscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps2.CountryID where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES')";
            }
            else if (hfColumn.Value.Equals(NameCountry, StringComparison.OrdinalIgnoreCase))
            {
                query += " join (select SubscriptionID from PubSubscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps2.CountryID)";
            }
            else
            {
                columnSearch = true;
                if (hfColumnSearchValue.Value == string.Empty)
                {
                    query += " CROSS APPLY (SELECT TOP 100 " + hfColumn.Value +
                             ", SubscriptionID from PubSubscriptions WITH (nolock) where subscriptionID = ps.SubscriptionID and " +
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
                        whereCondition += $"{(i > 0 ? " OR " : string.Empty)}{hfColumn.Value} not like '%{escaped}%'";
                    }

                    whereCondition += ")";

                    query += " CROSS APPLY (SELECT TOP 100 " + hfColumn.Value +
                             ", SubscriptionID from PubSubscriptions WITH (nolock) where subscriptionID = ps.SubscriptionID and " +
                             whereCondition + " GROUP BY " + hfColumn.Value + " , SubscriptionID  ORDER BY Count(subscriptionid) DESC ";
                }
            }

            return query;
        }

        private string GetCrossTabColumnDefaultQuery(string query, string crossTab)
        {
            if (crossTab.Equals(NameNoResponse, StringComparison.OrdinalIgnoreCase))
            {
                var whereConditionDesc = string.Empty;
                if (hfColumnSearchValue.Value != string.Empty)
                {
                    var strColumnSearchValue = hfColumnSearchValue.Value.Split(CommaChar);
                    whereConditionDesc = "(";

                    for (var i = 0; i < strColumnSearchValue.Length; i++)
                    {
                        var escaped = strColumnSearchValue[i].Replace("_", "[_]");
                        whereConditionDesc += $"{(i > 0 ? " OR " : string.Empty)} responsedesc  like '%{escaped}%'";
                    }

                    whereConditionDesc += ")";
                }

                if (hfColumnSearchValue.Value == string.Empty)
                {
                    query += " join (select SubscriptionID from PubSubscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 join PubSubscriptionDetail psd2 on ps2.SubscriptionID = psd2.SubscriptionID join codesheet c on c.codesheetID = psd2.codesheetID where c.ResponseGroupID = " +
                        hfColumn.Value + ")";
                }
                else
                {
                    query += " join (select SubscriptionID from PubSubscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 join PubSubscriptionDetail psd2 on ps2.SubscriptionID = psd2.SubscriptionID join codesheet c on c.codesheetID = psd2.codesheetID where " +
                        whereConditionDesc + ")";
                }
            }
            else if (crossTab.Equals(NameGrandTotal, StringComparison.OrdinalIgnoreCase))
            {
                query += " join (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join PubSubscriptionDetail psd2 WITH (nolock) on ps2.SubscriptionID  = psd2.SubscriptionID join CodeSheet c with (nolock) on c.codesheetID = psd2.codesheetID  where c.ResponseGroupID = " +
                    hfColumn.Value;
                query += " union select SubscriptionID from PubSubscriptions where SubscriptionID not in (select distinct ps2.subscriptionID from PubSubscriptions ps2 join PubSubscriptionDetail psd2 on ps2.SubscriptionID = psd2.SubscriptionID join codesheet c on c.codesheetID = psd2.codesheetID where c.ResponseGroupID = " +
                    hfColumn.Value + ")";
            }
            else
                query += " join (select distinct ps2.subscriptionID from PubSubscriptions ps2 with (nolock) join PubSubscriptionDetail psd2 WITH (nolock) on ps2.SubscriptionID  = psd2.SubscriptionID join CodeSheet c with (nolock) on c.codesheetID = psd2.codesheetID  where c.ResponseDesc = '" +
                    crossTab + "' and c.ResponseGroupID = " + hfColumn.Value;

            return query;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = NameAudienceViews;
            Master.SubMenu = NameProduct;

            InitializeCommonControls();

            if (!IsPostBack)
            {
                InitializeForHttpGet();
            }
            else
            {
                InitializeForPostBack(grdFilters);
            }
        }

        private void InitializeCommonControls()
        {
            rpgCrossTab.EnableAjaxSkinRendering = true;

            DownloadPanel1.DelMethod = new RebuildSubscriberList(Reload);
            DownloadPanel1.hideDownloadPopup = new HidePanel(DownloadPopupHide);
            AdhocFilter.hideAdhocPopup = new HidePanel(AdhocPopupHide);
            ActivityFilter.hideActivityPopup = new HidePanel(ActivityPopupHide);
            FilterSave.hideFilterSavePopup = new HidePanel(FilterSaveHide);
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

            lblErrorMsg.Text = string.Empty;
            divErrorMsg.Visible = false;
            txtShowQuery.Text = string.Empty;
            lblSaveCampaignPopupError.Text = string.Empty;
            divSaveCampaignPopupError.Visible = false;

            fc = FilterCollection;
        }

        private void InitializeForHttpGet()
        {
            RedirectIfNoViewAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.ServiceFeatures.ProductView);

            if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter,
                KMPlatform.Enums.Access.View))
            {
                lnkSavedFilter.Visible = false;
            }

            if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter,
                KMPlatform.Enums.Access.Edit))
            {
                btnOpenSaveFilterPopup.Visible = false;
            }

            var clientId = ConfigurationManager.AppSettings[NameManualLoadClientIds].Split(CommaChar);

            if (clientId.Contains(Master.UserSession.ClientID.ToString()))
            {
                rblLoadType.SelectedValue = TextManualLoad;
                btnLoadComboVenn.Visible = true;
            }

            bpResults.Visible = false;
            SortField = TextValue;
            SortDirection = SortDirectionAscending;

            LoadBrands();
            loadStandardFiltersListboxes();

            rcbEmailStatus.DataSource = EmailStatus.GetAll(Master.clientconnections);
            rcbEmailStatus.DataBind();

            DownloadPanel1.Showexporttoemailmarketing = true;
            DownloadPanel1.Showsavetocampaign = true;
            DownloadPanel1.Showexporttomarketo = true;
            DownloadPanel1.error = false;
            CLDownloadPanel.error = false;
            EVDownloadPanel.error = false;
            FilterSegmentation.UserID = Master.LoggedInUser;
            FilterSegmentation.fcSessionName = fcSessionName;
        }

        private static int GetInt32(string str)
        {
            int result;
            if (!int.TryParse(str, out result))
            {
                throw new InvalidOperationException($"Unable parse int from '{str}'");
            }

            return result;
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

        public void getResponseGroup()
        {
            if (hfProductID.Value != string.Empty)
            {
                dlDimensions.DataSource = ResponseGroup.GetActiveByPubID(Master.clientconnections, Convert.ToInt32(hfProductID.Value));
                dlDimensions.DataBind();

                AdhocFilter.PubID = Convert.ToInt32(hfProductID.Value);
                AdhocFilter.BrandID = Convert.ToInt32(hfBrandID.Value);
                lnkAdhoc.Enabled = true;
                ActivityFilter.PubID = Convert.ToInt32(hfProductID.Value);
                lnkActivity.Enabled = true;
                lnkSavedFilter.Enabled = true;
                lnkCirculation.Enabled = true;
                AdhocFilter.LoadAdhocGrid();
            }
        }

        private void loadStandardFiltersListboxes()
        {
            lstCountryRegions.DataSource = Country.GetArea();
            lstCountryRegions.DataBind();

            lstCountry.DataSource = Country.GetAll();
            lstCountry.DataBind();

            lstState.DataSource = Region.GetAll();
            lstState.DataBind();

            lstGeoCode.DataSource = RegionGroup.GetAll();
            lstGeoCode.DataBind();
        }

        protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc.Clear();
            FilterCollection.Clear();
            ResetAllFilterControls();
            LoadGridFilters();

            imglogo.ImageUrl = string.Empty;
            imglogo.Visible = false;
            hfBrandID.Value = drpBrand.SelectedValue;
            hfBrandName.Value = drpBrand.SelectedItem.Text;
            dlDimensions.DataSource = null;
            dlDimensions.DataBind();

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
                    hfBrandID.Value = drpBrand.SelectedValue;
                }
            }

            if (Convert.ToInt32(hfBrandID.Value) >= 0)
                LoadProducts();
        }

        protected void LoadProducts()
        {
            List<Pubs> lpubs = new List<Pubs>();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
                lpubs = Pubs.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
            else
                lpubs = Pubs.GetSearchEnabled(Master.clientconnections);

            lpubs = (from p in lpubs
                     orderby p.PubName ascending
                     select p).ToList(); ;

            rcbProduct.DataSource = lpubs;
            rcbProduct.DataBind();
            rcbProduct.Items.Insert(0, new RadComboBoxItem("", "0"));
        }

        protected void rcbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rcbProduct.SelectedValue != "")
            {
                hfProductID.Value = rcbProduct.SelectedValue;
                getResponseGroup();
                DownloadPanel1.BrandID = Convert.ToInt32(hfBrandID.Value);
                DownloadPanel1.PubIDs = rcbProduct.SelectedValue.Split(',').Select(int.Parse).ToList();
                DownloadPanel1.ViewType = Enums.ViewType.ProductView;
                CLDownloadPanel.BrandID = Convert.ToInt32(hfBrandID.Value);
                CLDownloadPanel.PubIDs = rcbProduct.SelectedValue.Split(',').Select(int.Parse).ToList();
                CLDownloadPanel.ViewType = Enums.ViewType.ProductView;
                EVDownloadPanel.BrandID = Convert.ToInt32(hfBrandID.Value);
                EVDownloadPanel.PubIDs = rcbProduct.SelectedValue.Split(',').Select(int.Parse).ToList();
                EVDownloadPanel.ViewType = Enums.ViewType.ProductView;

                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.DataCompare, KMPlatform.Enums.Access.Yes))
                {
                    hfDataCompareProcessCode.Value = string.Empty;
                    ResetDataCompareControls();

                    List<FrameworkUAS.Entity.SourceFile> lsf = new FrameworkUAS.BusinessLogic.SourceFile().Select(Master.UserSession.ClientID, false);
                    List<FrameworkUAS.Entity.DataCompareRun> ldcr = new FrameworkUAS.BusinessLogic.DataCompareRun().SelectForClient(Master.UserSession.ClientID);

                    var query = (from s in lsf
                                 join d in ldcr on s.SourceFileID equals d.SourceFileId
                                 orderby d.DateCreated
                                 select new { FileName = s.FileName + "_" + d.DateCreated, ProcessCode = d.ProcessCode }
                               ).ToList();

                    drpDataCompareSourceFile.DataSource = query;
                    drpDataCompareSourceFile.DataBind();
                    drpDataCompareSourceFile.Items.Insert(0, new ListItem("", ""));

                    List<FrameworkUAD_Lookup.Entity.Code> lCode = new FrameworkUAD_Lookup.BusinessLogic.Code().Select(FrameworkUAD_Lookup.Enums.CodeType.Data_Compare_Type);
                    lCode.Where(w => w.CodeName == FrameworkUAD_Lookup.Enums.DataCompareType.Match.ToString()).ToList().ForEach(i => i.CodeName = "Standard");

                    rblDataCompareOperation.DataSource = lCode;
                    rblDataCompareOperation.DataBind();

                    try
                    {
                        rblDataCompareOperation.SelectedValue = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Data_Compare_Type, FrameworkUAD_Lookup.Enums.DataCompareType.Match.ToString()).CodeId.ToString();
                    }
                    catch { }

                    pnlDataCompare.Visible = true;
                }
            }
        }

        protected void rcbProduct_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            List<Pubs> lpubs = new List<Pubs>();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
                lpubs = Pubs.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
            else
                lpubs = Pubs.GetSearchEnabled(Master.clientconnections);

            if (e.Text.Trim() != "")
            { 
                lpubs = (from p in lpubs
                         where ( p.PubName.ToLower().Contains(e.Text.ToLower().Trim()) || p.PubCode.ToLower().Contains(e.Text.ToLower().Trim()))
                         orderby p.PubName ascending
                         select p).ToList(); ;
            }

            rcbProduct.DataSource = lpubs;
            rcbProduct.DataBind();
            rcbProduct.Items.Insert(0, new RadComboBoxItem("", "0"));
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
                int MatchedProductRecordCount = 0;

                List<Code> c = Code.GetDataCompareTarget();
                targetCodeID = c.Find(x => x.CodeName == Enums.DataCompareViewType.Product.ToString()).CodeID;
                int consensustargetCodeID = c.Find(x => x.CodeName == Enums.DataCompareViewType.Consensus.ToString()).CodeID;
                hfDcTargetCodeID.Value = targetCodeID.ToString();

                MatchedProductRecordCount = new FrameworkUAD.BusinessLogic.DataCompareProfile().GetDataCompareCount(new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient), hfDataCompareProcessCode.Value, targetCodeID, Convert.ToInt32(hfProductID.Value));
                int MatchedConsensusRecordCount = new FrameworkUAD.BusinessLogic.DataCompareProfile().GetDataCompareCount(new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient), hfDataCompareProcessCode.Value, consensustargetCodeID, 0);

                FrameworkUAS.Entity.DataCompareRun dcr = new FrameworkUAS.BusinessLogic.DataCompareRun().SelectForClient(Master.UserSession.ClientID).Find(x => x.ProcessCode == hfDataCompareProcessCode.Value);
                hfDcRunID.Value = dcr.DcRunId.ToString();

                List<FrameworkUAD.Entity.DataCompareProfile> dcp = new FrameworkUAD.BusinessLogic.DataCompareProfile().Select(new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient), hfDataCompareProcessCode.Value);

                int TotalFileRecord = dcp.Count;

                lblTotalFileRecords.Text = TotalFileRecord.ToString();

                lnkMatchedRecords.Text = MatchedProductRecordCount.ToString();
                lnkNonMatchedRecords.Text = (TotalFileRecord - MatchedProductRecordCount).ToString();
                lnkMatchedNotIn.Text = (MatchedConsensusRecordCount - MatchedProductRecordCount).ToString();

                plDataCompareData.Visible = true;
            }
        }

        protected void lnkDataCompare_Command(object sender, CommandEventArgs e)
        {
            var dataTable = new DataTable();
            var dataCompareTarget = Code.GetDataCompareTarget();
            var queries = new StringBuilder();
            hfDataCompareLinkSelected.Value = e.CommandArgument.ToString();
            dataTable = ProcessCommandArgument(e, dataTable, queries);

            if (dataTable.Rows.Count > 0
                && (e.CommandArgument.ToString().Equals(MatchedRecords, StringComparison.OrdinalIgnoreCase)
                    || e.CommandArgument.ToString().Equals(MatchedNotInProduct, StringComparison.OrdinalIgnoreCase)))
            {
                DownloadPanel1.SubscriptionID = null;
                DownloadPanel1.SubscribersQueries = queries;
                DownloadPanel1.downloadCount = dataTable.Rows.Count;
                DownloadPanel1.Visible = true;
                DownloadPanel1.HeaderText = string.Empty;
                DownloadPanel1.ShowHeaderCheckBox = false;
                DownloadPanel1.BrandID = IntTryParse(hfBrandID.Value);
                DownloadPanel1.PubIDs = rcbProduct.SelectedValue.Split(',').Select(int.Parse).ToList();
                DownloadPanel1.ViewType = Enums.ViewType.ProductView;
                DownloadPanel1.filterCombination = e.CommandArgument.ToString().Equals(MatchedRecords, StringComparison.OrdinalIgnoreCase) ? Matched : MatchedNotInSelected;
                DownloadPanel1.dcRunID = IntTryParse(hfDcRunID.Value);
                DownloadPanel1.dcTypeCodeID = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(
                        FrameworkUAD_Lookup.Enums.CodeType.Data_Compare_Type,
                        FrameworkUAD_Lookup.Enums.DataCompareType.Match.ToString())
                    .CodeId;
                DownloadPanel1.dcTargetCodeID = IntTryParse(hfDcTargetCodeID.Value);
                DownloadPanel1.matchedRecordsCount = IntTryParse(lnkMatchedRecords.Text);
                DownloadPanel1.nonMatchedRecordsCount = IntTryParse(lnkNonMatchedRecords.Text);
                DownloadPanel1.TotalFileRecords = IntTryParse(lblTotalFileRecords.Text);
                DownloadPanel1.LoadControls();
                DownloadPanel1.LoadDownloadTemplate();
                DownloadPanel1.loadExportFields();
                DownloadPanel1.ValidateExportPermission();
            }
            else if (dataTable.Rows.Count > 0)
            {
                btnDCDownload.OnClientClick = ReturnConfirmPopupPurchase;

                if (Master.UserSession.CurrentUser.IsKMStaff)
                {
                    plKmStaff.Visible = true;
                    plNotes.Visible = false;
                    drpIsBillable.SelectedIndex = -1;

                    var datacv = new DataCompareView().SelectForRun(IntTryParse(hfDcRunID.Value));
                    int? targetId = IntTryParse(hfProductID.Value);
                    var targetCodeID = 0;

                    if (datacv.Exists(dataCompareView => dataCompareView.DcTargetCodeId == targetCodeID && dataCompareView.DcTargetIdUad == targetId))
                    {
                        var dataCompareView = datacv.Find(u => u.DcTargetCodeId == IntTryParse(hfDcTargetCodeID.Value) && u.DcTargetIdUad == targetId);

                        if (dataCompareView.IsBillable)
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

        private DataTable ProcessCommandArgument(CommandEventArgs commandEventArgs, DataTable dataTable, StringBuilder queries)
        {
            switch (commandEventArgs.CommandArgument.ToString().ToUpper())
            {
                case MatchedRecords:
                    dataTable = new DataCompareProfile().GetDataCompareData(
                        new ClientConnections(Master.UserSession.CurrentUser.CurrentClient),
                        hfDataCompareProcessCode.Value,
                        IntTryParse(hfDcTargetCodeID.Value),
                        Matched,
                        IntTryParse(hfProductID.Value));
                    queries.Append(XmlQueriesOpenTag);
                    queries.Append(
                            $"<Query filterno=\"{1}\" ><![CDATA[{SelectDistinctSubscriptionIdQuery} join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID "
                            + $" where d.ProcessCode = \'{hfDataCompareProcessCode.Value}\'  and ps.PubID = {IntTryParse(hfProductID.Value)}]]></Query>")
                        .Append(QueriesCloseResultsOpenTag)
                        .Append(
                            $"<Result linenumber=\"{1}\"  selectedfilterno=\"1\" selectedfilteroperation=\"\" suppressedfilterno=\"\" suppressedfilteroperation=\"\"  filterdescription=\"\"></Result>")
                        .Append(ResultsXmlCloseTag);

                    break;
                case NonMatchedRecords:
                    dataTable = new DataCompareProfile().GetDataCompareData(
                        new ClientConnections(Master.UserSession.CurrentUser.CurrentClient),
                        hfDataCompareProcessCode.Value,
                        IntTryParse(hfDcTargetCodeID.Value),
                        NonMatched,
                        IntTryParse(hfProductID.Value));

                    break;
                case MatchedNotInProduct:
                    dataTable = new DataCompareProfile().GetDataCompareData(
                        new ClientConnections(Master.UserSession.CurrentUser.CurrentClient),
                        hfDataCompareProcessCode.Value,
                        IntTryParse(hfDcTargetCodeID.Value),
                        MatchedNotInProductSmall,
                        IntTryParse(hfProductID.Value));
                    queries.Append(XmlQueriesOpenTag)
                        .Append(
                            $"<Query filterno=\"{1}\" ><![CDATA[{SelectDistinctSubscriptionIdQuery} left outer join  (select distinct s1.SubscriptionID from DataCompareProfile d1 with(nolock)"
                            + $"join Subscriptions s1 with(Nolock) on d1.IGRP_NO = s1.IGrp_No join PubSubscriptions ps with(nolock) on s1.SubscriptionID = ps.SubscriptionID  "
                            + $"where d1.ProcessCode = \'{hfDataCompareProcessCode.Value}\'and ps.PubID = {IntTryParse(hfProductID.Value)} ) x on s.SubscriptionID = x.SubscriptionID "
                            + $"where d.ProcessCode = \'{hfDataCompareProcessCode.Value}\' and x.SubscriptionID is null ]]></Query>")
                        .Append(QueriesCloseResultsOpenTag)
                        .Append(
                            $"<Result linenumber=\"{1}\"  selectedfilterno=\"1\" selectedfilteroperation=\"\" suppressedfilterno=\"\" suppressedfilteroperation=\"\"  filterdescription=\"\"></Result>")
                        .Append(ResultsXmlCloseTag);

                    break;
            }

            return dataTable;
        }

        protected void lnkDataCompareSummary_Command(object sender, CommandEventArgs e)
        {
            DataCompareSummary.BrandID = Convert.ToInt32(hfBrandID.Value);
            DataCompareSummary.PubID = Convert.ToInt32(hfProductID.Value);
            DataCompareSummary.UserID = Master.LoggedInUser;
            DataCompareSummary.ViewType = Enums.ViewType.ProductView;
            DataCompareSummary.ProcessCode = hfDataCompareProcessCode.Value;
            DataCompareSummary.TotalRecords = Convert.ToInt32(lblTotalFileRecords.Text);
            DataCompareSummary.DcTargetCodeID = Convert.ToInt32(hfDcTargetCodeID.Value);
            DataCompareSummary.MatchType = e.CommandArgument.ToString();

            if (e.CommandArgument.ToString().ToUpper() == "MATCHED")
                DataCompareSummary.MatchedNonMatchedRecords = Convert.ToInt32(lnkMatchedRecords.Text);
            else
                DataCompareSummary.MatchedNonMatchedRecords = Convert.ToInt32(lnkMatchedNotIn.Text);

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

                dt = new FrameworkUAD.BusinessLogic.DataCompareProfile().GetDataCompareData(new KMPlatform.Object.ClientConnections(Master.UserSession.CurrentUser.CurrentClient), hfDataCompareProcessCode.Value, Convert.ToInt32(hfDcTargetCodeID.Value), "NonMatched", Convert.ToInt32(hfProductID.Value));

                //Save DataCompare view details

                int dcViewID = 0;

                if (drpDataCompareSourceFile.SelectedValue != "")
                {
                    int? targetID = Convert.ToInt32(hfProductID.Value);
                    int typeCodeID = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Data_Compare_Type, FrameworkUAD_Lookup.Enums.DataCompareType.Match.ToString()).CodeId;

                    //Calculating Total UAD Records count
                    Filter filter = new Filter();
                    filter.ViewType = Enums.ViewType.ProductView;
                    filter.PubID = Convert.ToInt32(hfProductID.Value);

                    if (Convert.ToInt32(hfBrandID.Value) > 0)
                    {
                        filter.BrandID = Convert.ToInt32(hfBrandID.Value);
                        filter.Fields.Add(new Field("Brand", Convert.ToInt32(hfBrandID.Value).ToString(), hfBrandName.Value, "", Enums.FiltersType.Brand, "BRAND"));
                    }

                    if (rcbProduct.SelectedItem.Value != string.Empty)
                        filter.Fields.Add(new Field("Product", rcbProduct.SelectedItem.Value, rcbProduct.SelectedItem.Text, "", Enums.FiltersType.Product, "PRODUCT"));

                    filter.Execute(Master.clientconnections, "");

                    List<FrameworkUAS.Entity.DataCompareView> datacv = new FrameworkUAS.BusinessLogic.DataCompareView().SelectForRun(Convert.ToInt32(hfDcRunID.Value));

                    int tcID = Code.GetDataCompareTarget().Find(x => x.CodeName == Enums.DataCompareViewType.Consensus.ToString()).CodeID;

                    int paymentStatusPendingID = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Pending.ToString()).CodeId;
                    int paymentStatusNon_BilledID = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Non_Billed.ToString()).CodeId;

                    if (datacv.Exists(y => y.DcTargetCodeId == tcID && (y.PaymentStatusId == paymentStatusPendingID || y.PaymentStatusId == paymentStatusNon_BilledID)))
                    {
                        int id = datacv.Find(y => y.DcTargetCodeId == tcID && (y.PaymentStatusId == paymentStatusPendingID || y.PaymentStatusId == paymentStatusNon_BilledID)).DcViewId;
                        new FrameworkUAS.BusinessLogic.DataCompareView().Delete(id);
                        saveDataCompareView(Convert.ToInt32(hfDcTargetCodeID.Value), targetID, typeCodeID, filter.Count);
                    }
                    else
                    {
                        if (!datacv.Exists(u => u.DcTargetCodeId == Convert.ToInt32(hfDcTargetCodeID.Value) && u.DcTargetIdUad == targetID && u.DcTypeCodeId == typeCodeID))
                        {
                            saveDataCompareView(Convert.ToInt32(hfDcTargetCodeID.Value), targetID, typeCodeID, filter.Count);
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

                    List<FrameworkUAS.Entity.DataCompareDownloadCostDetail> cd = new FrameworkUAS.BusinessLogic.DataCompareDownloadCostDetail().CreateCostDetail(dcViewID, Convert.ToInt32(rblDataCompareOperation.SelectedValue), dt.Rows.Count.ToString(), string.Join(",", colList), string.Empty, Master.UserSession.CurrentUser.UserID);

                    FrameworkUAS.Entity.DataCompareDownload dcd = new FrameworkUAS.Entity.DataCompareDownload();

                    dcd.DcViewId = dcViewID;
                    dcd.WhereClause = "NonMatched";
                    dcd.DcTypeCodeId = new FrameworkUAD_Lookup.BusinessLogic.Code().SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Data_Compare_Type, FrameworkUAD_Lookup.Enums.DataCompareType.Match.ToString()).CodeId; 
                    dcd.ProfileCount = dt.Rows.Count;
                    dcd.TotalBilledCost = new FrameworkUAS.BusinessLogic.DataCompareView().GetDataCompareCost(Master.UserSession.CurrentUser.UserID, Master.UserSession.ClientID, Convert.ToInt32(lnkNonMatchedRecords.Text), FrameworkUAD_Lookup.Enums.DataCompareType.Match, FrameworkUAD_Lookup.Enums.DataCompareCost.Download);
                    dcd.IsPurchased = plKmStaff.Visible ? (Convert.ToBoolean(drpIsBillable.SelectedValue)) : Convert.ToBoolean(hfIsBillable.Value);
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

        public void DisplayError(string errorMessage)
        {
            lblErrorMsg.Text = errorMessage;
            divErrorMsg.Visible = true;
        }

        public void DisplaySaveCampaignPopupError(string errorMessage)
        {
            lblSaveCampaignPopupError.Text = errorMessage;
            divSaveCampaignPopupError.Visible = true;
        }

        private string GetLinkTitle(int pubtypeid)
        {
            string title = string.Empty;
            DataTable tbl = DataFunctions.getDataTable("select PubTypeDisplayName from PubTypes where pubtypeid = " + pubtypeid.ToString(), DataFunctions.GetClientSqlConnection(Master.clientconnections));
            foreach (DataRow dr in tbl.Rows)
            {
                title = dr["PubTypeDisplayName"].ToString();
                break;
            }

            return title;
        }

        protected void dlDimensions_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton linkButton = e.Item.FindControl("lnkDimensionPopup") as LinkButton;
                linkButton.CommandName = e.Item.ItemIndex.ToString();

                LinkButton lnkDimensionShowHide = e.Item.FindControl("lnkDimensionShowHide") as LinkButton;
                lnkDimensionShowHide.CommandName = e.Item.ItemIndex.ToString();
            }
        }

        private void LoadGridFilters()
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

        protected void btnAddFitler_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(hfProductID.Value) > 0)
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

                    drpBrand.Enabled = false;
                    rcbProduct.Enabled = false;

                    if (pnlDataCompare.Visible)
                    {
                        drpDataCompareSourceFile.Enabled = false;
                        rblDataCompareOperation.Enabled = false;
                    }

                    Filter f = getFilter();

                    if (f.Fields.Count > 0)
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
                            //getResponseGroup();
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
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }
            }
            else
            {
                DisplayError("Please select a product.");
            }

            TabContainer.ActiveTabIndex = 0;
            FilterSegmentation.LoadControls();
            FilterSegmentation.UpdateFiltersegmentationCounts();
        }

        private Filter getFilter()
        {
            int hiddenFieldProductId;

            if (!int.TryParse(hfProductID.Value, out hiddenFieldProductId))
            {
                throw new InvalidOperationException($"Unable to parse {hiddenFieldProductId}");
            }

            var filter = new Filter { ViewType = ViewType.ProductView, PubID = hiddenFieldProductId };
            var responseGroup = ResponseGroup.GetActiveByPubID(Master.clientconnections, hiddenFieldProductId);

            this.AddFilterFieldsListItems(responseGroup, filter);

            int hiddenFieldBrandID;

            if (!int.TryParse(hfBrandID.Value, out hiddenFieldBrandID))
            {
                throw new InvalidOperationException($"Unable to parse {hiddenFieldBrandID}");
            }

            if (pnlBrand.Visible && hiddenFieldBrandID > 0)
            {
                filter.BrandID = hiddenFieldBrandID;
                filter.Fields.Add(new Field(NameBrand, hfBrandID.Value, hfBrandName.Value, string.Empty, Enums.FiltersType.Brand, NameBrand.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(rcbProduct.SelectedItem.Value))
            {
                filter.Fields.Add(new Field(NameProduct, rcbProduct.SelectedItem.Value, rcbProduct.SelectedItem.Text, string.Empty, Enums.FiltersType.Product, NameProduct.ToUpper()));
            }

            if (pnlDataCompare.Visible && !string.IsNullOrWhiteSpace(drpDataCompareSourceFile.SelectedValue))
            {
                filter.Fields.Add(
                    new Field(
                        NameDataCompare,
                        $"{this.drpDataCompareSourceFile.SelectedValue.Split('|').First()}|{this.rblDataCompareOperation.SelectedValue}",
                        $"{this.drpDataCompareSourceFile.SelectedItem.Text}|{this.rblDataCompareOperation.SelectedItem.Text}",
                        string.Empty,
                        Enums.FiltersType.DataCompare,
                        NameDataCompare));
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

        private void AddFilterFieldsGeoAdhocActivityCirculationPopup(Filter filter)
        {
            if (!string.IsNullOrWhiteSpace(this.RadMtxtboxZipCode.TextWithLiterals))
            {
                if (!string.IsNullOrWhiteSpace(this.txtRadiusMin.Text) && !string.IsNullOrWhiteSpace(this.txtRadiusMax.Text) && int.Parse(this.txtRadiusMin.Text) < int.Parse(this.txtRadiusMax.Text))
                {
                    double locationLat;
                    double locationLon;
                    var radiusMin = 0;
                    var radiusMax = 0;
                    int.TryParse(this.txtRadiusMin.Text, out radiusMin);
                    int.TryParse(this.txtRadiusMax.Text, out radiusMax);

                    var values = FilterMVC.CalculateZipCodeRadius(radiusMin, radiusMax, this.RadMtxtboxZipCode.TextWithLiterals, this.drpCountry.SelectedValue, out locationLat, out locationLon);

                    filter.Fields.Add(
                        new Field(
                            ZipcodeRadius,
                            string.Join("|", values),
                            $"{this.RadMtxtboxZipCode.TextWithLiterals} & {this.txtRadiusMin.Text} miles - {this.txtRadiusMax.Text} miles",
                            $"{this.RadMtxtboxZipCode.TextWithLiterals}|{this.txtRadiusMin.Text}|{this.txtRadiusMax.Text}",
                            Enums.FiltersType.Geo,
                            ZipcodeRadius.ToUpper()));
                }
            }

            foreach (var field in this.AdhocFilter.GetAdhocFilters())
            {
                filter.Fields.Add(field);
            }

            foreach (var fd in this.ActivityFilter.GetActivityFilters())
            {
                filter.Fields.Add(fd);
            }

            foreach (var fd in this.CirculationFilter.GetCirculationFilters())
            {
                filter.Fields.Add(fd);
            }
        }

        private void AddFilterFieldsComboBox(Filter filter)
        {
            Action<RadComboBox, string> addComboBoxItems = (radComboBox, fieldName) =>
                {
                    if (radComboBox.CheckedItems.Count > 0)
                    {
                        var selectedItem = Utilities.getRadComboBoxSelectedExportFields(radComboBox);
                        filter.Fields.Add(new Field(fieldName, selectedItem.Item1, selectedItem.Item2, string.Empty, Enums.FiltersType.Standard, string.Concat(fieldName.Split(' ')).ToUpper()));
                    }
                };

            addComboBoxItems(this.rcbEmail, NameEmail);
            addComboBoxItems(this.rcbPhone, NamePhone);
            addComboBoxItems(this.rcbFax, NameFax);
            addComboBoxItems(this.rcbMedia, NameMedia);
            addComboBoxItems(this.rcbMailPermission, NameMailPermission);
            addComboBoxItems(this.rcbFaxPermission, NameFaxPermission);
            addComboBoxItems(this.rcbPhonePermission, NamePhonePermission);
            addComboBoxItems(this.rcbOtherProductsPermission, NameOtherProductsPermission);
            addComboBoxItems(this.rcbThirdPartyPermission, NameThirdPartyPermission);
            addComboBoxItems(this.rcbEmailRenewPermission, NameEmailRenewPermission);
            addComboBoxItems(this.rcbTextPermission, NameTextPermission);
            addComboBoxItems(this.rcbEmailStatus, NameEmailStatus);
        }

        private void AddFilterFieldsListItems(List<ResponseGroup> responseGroup, Filter filter)
        {
            foreach (DataListItem dataListItem in this.dlDimensions.Items)
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

        protected void lnkResetAll_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(hfBrandID.Value) > 0)
                Response.Redirect("ProductView.aspx?brandID=" + Convert.ToInt32(hfBrandID.Value));
            else
                Response.Redirect("ProductView.aspx");
        }

        protected void lnkPermissionDownload_Command(object sender, CommandEventArgs e)
        {
            CampaignID = 0;
            CampaignFilterID = 0;
            lblPermission.Text = "Permission";
            string temp = e.CommandArgument.ToString();
            string[] args = temp.Split('/');
            lblCodeSheetID.Text = args[0];
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
            lblCodeSheetID.Text = string.Empty;
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
            lblCodeSheetID.Text = string.Empty;
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
            lblCodeSheetID.Text = string.Empty;
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
            lblCodeSheetID.Text = string.Empty;
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

            if (fc.Count <= 1)
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
                Filter f = fc.SingleOrDefault(filter => filter.FilterNo == Convert.ToInt32(e.CommandArgument.ToString()));
                hfFilterNo.Value = e.CommandArgument.ToString();
                hfFilterName.Value = f.FilterName;
                hfFilterGroupName.Value = f.FilterGroupName;
                //getResponseGroup();
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
                //hfFilterID.Value = e.CommandArgument.ToString();
                hfFilterNo.Value = e.CommandArgument.ToString();
                hfFilterName.Value = f.FilterName;
                hfFilterGroupName.Value = f.FilterGroupName;

                string[] strvalues;
                //string[] strCond;

                foreach (Field field in f.Fields)
                {
                    switch (field.FilterType)
                    {
                        case Enums.FiltersType.Dimension:
                            foreach (DataListItem di in dlDimensions.Items)
                            {
                                Label lblResponseGroup = (Label)di.FindControl("lblResponseGroup");
                                ListBox lstResponse = (ListBox)di.FindControl("lstResponse");
                                HiddenField hfResponseGroupID = (HiddenField)di.FindControl("hfResponseGroupID");
                                LinkButton lnkDimensionShowHide = (LinkButton)di.FindControl("lnkDimensionShowHide");
                                Panel pnlDimBody = (Panel)di.FindControl("pnlDimBody");

                                string resGroupName = ResponseGroup.GetByResponseGroupID(Master.clientconnections, Convert.ToInt32(hfResponseGroupID.Value)).ResponseGroupName;

                                if (resGroupName.ToUpper() == field.Group.ToUpper())
                                {
                                    if (lnkDimensionShowHide.Text == "(Show...)")
                                    {
                                        pnlDimBody.Visible = true;

                                        if (lstResponse.Items.Count == 0)
                                        {
                                            lstResponse.DataTextField = "ResponseDesc";
                                            lstResponse.DataValueField = "CodeSheetID";

                                            List<CodeSheet> codesheet = CodeSheet.GetByResponseGroupID(Master.clientconnections, Convert.ToInt32(hfResponseGroupID.Value));

                                            var CodeSheetQuery = (from cs in codesheet
                                                                  select new { cs.CodeSheetID, ResponseDesc = cs.ResponseDesc + ' ' + '(' + cs.ResponseValue + ')' });

                                            lstResponse.DataSource = CodeSheetQuery;
                                            lstResponse.DataBind();
                                        }

                                        lnkDimensionShowHide.Text = "(Hide...)";
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
                    var chkSelectFilter = (CheckBox)e.Row.FindControl("chkSelectFilter");
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

                //    int AllUnionCount = fc.AllUnion;

                //    if (AllUnionCount <= 0)
                //        lnkdownloadAllUnion.Enabled = false;

                //    lnkdownloadAllUnion.Text = AllUnionCount.ToString();
                //    lnkdownloadAllUnion.CommandArgument = AllUnionCount.ToString();
                //    lnkdownloadAllUnion.CommandName = "download";

                //    int AllIntersectCount = fc.AllInterSect;

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
            //        if (i >= 4)
            //        {
            //            grdFilters.FooterRow.Cells.RemoveAt(i);
            //        }
            //    }
            //    grdFilters.FooterRow.Cells[3].ColumnSpan = 5;
            //}
        }

        protected void grdFilterValues_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells[0].Text == "DataCompare") e.Row.Visible = false;
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
        
        protected void SelectStateOnRegion()
        {
            SelectStateOnRegion(lstGeoCode, lstState);
        }

        protected void SelectCountryOnRegion()
        {
            SelectCountryOnRegion(lstCountryRegions, lstCountry);
        }

        #region Dimension Report Events

        protected void chkIncludePermissions_CheckedChanged(object sender, EventArgs e)
        {
            LoadGrid();

            if (chkIncludePermissions.Checked)
            {
                grdSubReport.Columns[3].Visible = true;
                grdSubReport.Columns[4].Visible = true;
                grdSubReport.Columns[5].Visible = true;
                grdSubReport.Columns[6].Visible = true;
                grdSubReport.Columns[7].Visible = true;
                grdSubReport.Columns[8].Visible = true;
                grdSubReport.Columns[9].Visible = true;
                mdlPopupDimensionReport.Show();
            }
            else
            {
                grdSubReport.Columns[3].Visible = false;
                grdSubReport.Columns[4].Visible = false;
                grdSubReport.Columns[5].Visible = false;
                grdSubReport.Columns[6].Visible = false;
                grdSubReport.Columns[7].Visible = false;
                grdSubReport.Columns[8].Visible = false;
                grdSubReport.Columns[9].Visible = false;
                mdlPopupDimensionReport.Show();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void LoadGrid()
        {
            List<DemographicReport> CSReportDetails = null;

            if (chkIncludePermissions.Checked)
                CSReportDetails = DemographicReport.GetProductDemographicDataWithPermission(Master.clientconnections, Filter.generateCombinationQuery(fc, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, "", 0, 0, Master.clientconnections), Convert.ToInt32(hfReportFor.Value), txtDescription.Text, lblSelectedFilterNos.Text + lblSelectedFilterOperation.Text + lblSuppressedFilterNos.Text + lblSuppressedFilterOperation.Text, Convert.ToInt32(hfProductID.Value));
             else
                CSReportDetails = DemographicReport.GetProductDemographicData(Master.clientconnections, Filter.generateCombinationQuery(fc, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, "", 0, 0, Master.clientconnections), Convert.ToInt32(hfReportFor.Value), txtDescription.Text, lblSelectedFilterNos.Text + lblSelectedFilterOperation.Text + lblSuppressedFilterNos.Text + lblSuppressedFilterOperation.Text, Convert.ToInt32(hfProductID.Value));

            if (txtDescription.Text != string.Empty)
            {
                CSReportDetails = CSReportDetails.Where(x => x.Desc.ToLower().Contains(txtDescription.Text.ToLower())).ToList();
            }

            List<DemographicReport> lst = null;

            if (CSReportDetails != null && CSReportDetails.Count > 0)
            {
                switch (SortField.ToUpper())
                {
                    case "VALUE":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = CSReportDetails.OrderBy(o => o.Value).ToList();
                        else
                            lst = CSReportDetails.OrderByDescending(o => o.Value).ToList();
                        break;

                    case "DESC":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = CSReportDetails.OrderBy(o => o.Desc).ToList();
                        else
                            lst = CSReportDetails.OrderByDescending(o => o.Desc).ToList();
                        break;

                    case "COUNT":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = CSReportDetails.OrderBy(o => o.Count).ToList();
                        else
                            lst = CSReportDetails.OrderByDescending(o => o.Count).ToList();
                        break;

                    case "MAIL":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = CSReportDetails.OrderBy(o => o.Mail).ToList();
                        else
                            lst = CSReportDetails.OrderByDescending(o => o.Mail).ToList();
                        break;

                    case "EMAIL":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = CSReportDetails.OrderBy(o => o.Email).ToList();
                        else
                            lst = CSReportDetails.OrderByDescending(o => o.Email).ToList();
                        break;

                    case "PHONE":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = CSReportDetails.OrderBy(o => o.Phone).ToList();
                        else
                            lst = CSReportDetails.OrderByDescending(o => o.Phone).ToList();
                        break;

                    case "MAIL_PHONE":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = CSReportDetails.OrderBy(o => o.Mail_Phone).ToList();
                        else
                            lst = CSReportDetails.OrderByDescending(o => o.Mail_Phone).ToList();
                        break;

                    case "EMAIL_PHONE":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = CSReportDetails.OrderBy(o => o.Email_Phone).ToList();
                        else
                            lst = CSReportDetails.OrderByDescending(o => o.Email_Phone).ToList();
                        break;

                    case "MAIL_EMAIL":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = CSReportDetails.OrderBy(o => o.Mail_Email).ToList();
                        else
                            lst = CSReportDetails.OrderByDescending(o => o.Mail_Email).ToList();
                        break;

                    case "ALL_RECORDS":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = CSReportDetails.OrderBy(o => o.All_Records).ToList();
                        else
                            lst = CSReportDetails.OrderByDescending(o => o.All_Records).ToList();
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
                CheckBox chk = r.FindControl("chkSelectDownload") as CheckBox;

                if (chk != null && chk.Checked)
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
                lblAddFilters.Text = " s.subscriptionID in (select distinct ss.subscriptionID from subscriptions ss join pubSubscriptionDetail psd2 on ss.subscriptionID = psd2.subscriptionID where psd2.codesheetID in (" + IDs + "))";
            }

            lblCodeSheetID.Text = IDs;
            lblPremissionType.Text = string.Empty;
            lblPermission.Text = "Permission";
            mdlPopupProgress.Hide();
            mdlPopupDimensionReport.Show();

            string addFilters = GetAddlFilter();

            FilterViews fv = new FilterViews(Master.clientconnections, Master.LoggedInUser);
            filterView fv1 = new filterView();
            fv1.SelectedFilterNo = lblSelectedFilterNos.Text;
            fv1.SuppressedFilterNo = lblSuppressedFilterNos.Text;
            fv1.SelectedFilterOperation = lblSelectedFilterOperation.Text;
            fv1.SuppressedFilterOperation = lblSuppressedFilterOperation.Text;
            fv.Add(fv1);
            fv.Execute(fc, addFilters);
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
            lblCodeSheetID.Text = string.Empty;
            lblReportFor.Text = "Demographic Report";
            phReport.Visible = false;
            btnDownloadDetails.Visible = false;
            btnSaveCampaign.Visible = false;
            chkIncludePermissions.Checked = false;
            grdSubReport.DataSource = null;
            grdSubReport.DataBind();
            txtDescription.Text = string.Empty;
            Session.Remove(lblSelectedFilterNos.Text + lblSelectedFilterOperation.Text + lblSuppressedFilterNos.Text + lblSuppressedFilterOperation.Text + "ProductDemographicData" + drpDimension.SelectedValue);
            Session.Remove(lblSelectedFilterNos.Text + lblSelectedFilterOperation.Text + lblSuppressedFilterNos.Text + lblSuppressedFilterOperation.Text + "ProductDemographicDataWithPermission" + drpDimension.SelectedValue);
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

            drpDimension.DataSource = ResponseGroup.GetActiveByPubID(Master.clientconnections, Convert.ToInt32(hfProductID.Value));
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
                grdSubReport.Columns[3].Visible = false;
                grdSubReport.Columns[4].Visible = false;
                grdSubReport.Columns[5].Visible = false;
                grdSubReport.Columns[6].Visible = false;
                grdSubReport.Columns[7].Visible = false;
                grdSubReport.Columns[8].Visible = false;
                grdSubReport.Columns[9].Visible = false;

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
                    CheckBox chk = r.FindControl("chkSelectDownload") as CheckBox;

                    if (chk.Checked)
                    {
                        strIDs += strIDs == string.Empty ? grdSubReport.DataKeys[r.RowIndex].Values[0].ToString() : "," + grdSubReport.DataKeys[r.RowIndex].Values[0].ToString();
                    }
                }

                string ReportFor = lblReportFor.Text;

                List<DemographicReport> csReportData = null;

                if (chkIncludePermissions.Checked)
                    csReportData = DemographicReport.GetProductDemographicDataWithPermission(Master.clientconnections, Filter.generateCombinationQuery(fc, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, "", 0, 0, Master.clientconnections), Convert.ToInt32(hfReportFor.Value), txtDescription.Text, lblSelectedFilterNos.Text + lblSuppressedFilterOperation.Text + lblSelectedFilterNos.Text + lblSuppressedFilterNos.Text, Convert.ToInt32(hfProductID.Value));
                else
                    csReportData = DemographicReport.GetProductDemographicData(Master.clientconnections, Filter.generateCombinationQuery(fc, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, "", 0, 0, Master.clientconnections), Convert.ToInt32(hfReportFor.Value), txtDescription.Text, lblSelectedFilterNos.Text + lblSuppressedFilterOperation.Text + lblSelectedFilterNos.Text + lblSuppressedFilterNos.Text, Convert.ToInt32(hfProductID.Value));

                if (strIDs != string.Empty)
                {
                    IEnumerable<int> ids = strIDs.Split(',').Select(str => int.Parse(str));
                    csReportData = csReportData.Where(r => ids.Contains(r.ID)).ToList();
                }

                if (txtDescription.Text != string.Empty)
                {
                    csReportData = csReportData.Where(x => x.Desc.ToLower().Contains(txtDescription.Text.ToLower())).ToList();
                }

                DataTable dt = ListToDataTable(csReportData);

                Hashtable cParams = new Hashtable();
                cParams.Add("@ReportFor", hfReportFor.Value);
                cParams.Add("@Filters", "");
                cParams.Add("@Igrp", "1");

                report = new ReportDocument();
                if (chkIncludePermissions.Checked)
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

        #endregion

        #region CrossTab Report Events

        protected void btnCloseCrossTab_Click(object sender, EventArgs e)
        {
            ResetCrossTabModalControls();
            drpCrossTabReport.ClearSelection();
        }

        protected void btnCrossTabXls_Click(object sender, EventArgs e)
        {
            rpgCrossTab.ExportSettings.Excel.Format = (PivotGridExcelFormat)Enum.Parse(typeof(PivotGridExcelFormat), "Xlsx");

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
                ctr = CrossTabReport.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value)).FindAll(x => x.View_Type == Enums.ViewType.ProductView && x.PubID == Convert.ToInt32(hfProductID.Value));
            else
                ctr = CrossTabReport.GetNotInBrand(Master.clientconnections).FindAll(x => x.View_Type == Enums.ViewType.ProductView && x.PubID == Convert.ToInt32(hfProductID.Value));      

            var query = (from c in ctr
                         where c.PubID == Convert.ToInt32(hfProductID.Value)
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
                List<ResponseGroup> rg = ResponseGroup.GetActiveByPubID(Master.clientconnections, Convert.ToInt32(hfProductID.Value));
                string Column = string.Empty;
                string Row = string.Empty;

                if (int.TryParse(ctr.Column, out i))
                    Column = rg.Find(x => x.ResponseGroupID == Convert.ToInt32(ctr.Column)).DisplayName;
                else
                    Column = ctr.Column;

                if (int.TryParse(ctr.Row, out j))
                    Row = rg.Find(x => x.ResponseGroupID == Convert.ToInt32(ctr.Row)).DisplayName;
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
                else if (ctr.Row.Equals("Title", StringComparison.OrdinalIgnoreCase) || ctr.Row.Equals("Company", StringComparison.OrdinalIgnoreCase) || ctr.Row.Equals("City", StringComparison.OrdinalIgnoreCase))
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
            IList<CrossTab> csReportData = null;

            int i;

            XmlDocument xmlColDoc = new XmlDocument();
            XmlElement xmlColNode = xmlColDoc.CreateElement("XML");
            xmlColDoc.AppendChild(xmlColNode);

            if (int.TryParse(hfColumn.Value, out i))
            {
                XmlElement xmlColType;
                xmlColType = xmlColDoc.CreateElement("ColType");
                xmlColType.InnerText = "ResponseGroup";
                xmlColNode.AppendChild(xmlColType);

                XmlElement xmlColMasterGroupID;
                xmlColMasterGroupID = xmlColDoc.CreateElement("ColResponseGroupID");
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
                xmlRowType.InnerText = "ResponseGroup";
                xmlRowNode.AppendChild(xmlRowType);

                XmlElement xmlRowMasterGroupID;
                xmlRowMasterGroupID = xmlRowDoc.CreateElement("RowResponseGroupID");
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

            csReportData = CrossTab.GetProductCrossTabData(Master.clientconnections, Filter.generateCombinationQuery(fc, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, "", 0, 0, Master.clientconnections), xmlColDoc.OuterXml.ToString(), xmlRowDoc.OuterXml.ToString(), Convert.ToInt32(hfProductID.Value));

            rpgCrossTab.DataSource = csReportData;
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
            lblCodeSheetID.Text = string.Empty;
            string[] args = e.CommandArgument.ToString().Split('|');
            hfSelectedCrossTabLink.Value = e.CommandArgument.ToString();
            lblDownloadCount.Text = args[2];
            lblIsPopupCrossTab.Text = "true";
            lblIsPopupDimension.Text = "false";
            ShowDownloadPanel();
        }
        #endregion

        #region GeoReport Events
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
            dt1= DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(Master.clientconnections));

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
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetProductGeographicalReportData(Master.clientconnections, "sp_rpt_Product_Qualified_Breakdown_Canada", Queries, Convert.ToInt32(hfProductID.Value))));
                    break;
                case "GeoDomestic":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoBreakdown_Domestic.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetProductGeographicalReportData(Master.clientconnections, "sp_rpt_Product_Qualified_Breakdown_Domestic", Queries, Convert.ToInt32(hfProductID.Value))));
                    break;
                case "GeoInternational":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoBreakdown_by_Country.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetProductGeographicalReportData(Master.clientconnections, "sp_rpt_Product_Qualified_Breakdown_by_country", Queries, Convert.ToInt32(hfProductID.Value))));
                    break;
                case "GeoPermissionCanada":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoPermission_Canada.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetProductGeographicalReportData(Master.clientconnections, "sp_rpt_Product_Qualified_Breakdown_Canada", Queries, Convert.ToInt32(hfProductID.Value))));
                    break;
                case "GeoPermissionDomestic":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoPermission_Domestic.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetProductGeographicalReportData(Master.clientconnections, "sp_rpt_Product_Qualified_Breakdown_Domestic", Queries, Convert.ToInt32(hfProductID.Value))));
                    break;
                case "GeoPermissionInternational":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoPermission_by_Country.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetProductGeographicalReportData(Master.clientconnections, "sp_rpt_Product_Qualified_Breakdown_by_country", Queries, Convert.ToInt32(hfProductID.Value))));
                    break;
            }
            ReportViewer1.LocalReport.Refresh();
        }

        #endregion

        protected void lnkCompanyLocationView_Command(object sender, CommandEventArgs e)
        {
            lblPermission.Text = string.Empty;
            lblCodeSheetID.Text = string.Empty;
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
            lblCodeSheetID.Text = string.Empty;
            string[] args = e.CommandArgument.ToString().Split('/');
            lblSelectedFilterNos.Text = args[0];
            lblSuppressedFilterNos.Text = args[1];
            lblSelectedFilterOperation.Text = args[2];
            lblSuppressedFilterOperation.Text = args[3];
            lblFilterCombination.Text = args[4];
            lblDownloadCount.Text = args[5];
            ShowEVDownloadPanel();
        }

        #region RESET

        protected void btnResetAll_Click(object sender, EventArgs e)
        {
            fc.Clear();
            FilterCollection.Clear();
            ResetAllFilterControls();
            LoadGridFilters();

            if (pnlBrand.Visible)
            {
                drpBrand.Enabled = true;
            }

            TabContainer.ActiveTabIndex = 0;
        }

        private void ResetAllFilterControls()
        {
            pnlDataCompare.Visible = false;
            drpDataCompareSourceFile.SelectedIndex = -1;
            hfDataCompareProcessCode.Value = string.Empty;
            ResetDataCompareControls();
            
            rcbProduct.Enabled = true;
            rcbProduct.Text = "";
            rcbProduct.ClearSelection();
            lnkAdhoc.Enabled = false;
            lnkActivity.Enabled = false;
            lnkSavedFilter.Enabled = false;
            lnkCirculation.Enabled = false;
            ResetFilterControls();
 
            dlDimensions.DataSource = null;
            dlDimensions.DataBind();
        }

        private void ResetDataCompareControls()
        {
            hfDataCompareLinkSelected.Value = string.Empty;
            plDataCompareData.Visible = false;
            hfDcTargetCodeID.Value = "0";
            hfDcRunID.Value = "0";
            hfDcViewID.Value = "0";
            lblTotalFileRecords.Text = string.Empty;
            lnkMatchedRecords.Text = string.Empty;
            lnkNonMatchedRecords.Text = string.Empty;
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
        #endregion

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
            if (!HandleBtnOpenSaveFilterPopupClick(
                grdFilters,
                FilterSave,
                hfBrandID.Value,
                Enums.ViewType.ProductView,
                "chkSelectFilter",
                hfProductID.Value))
            {
                DisplayError("Please select a checkbox.");
            }
        }

        #region Dimension Popup Events

        protected void lnkDimensionShowHide_Command(object sender, CommandEventArgs e)
        {
            string[] args = e.CommandArgument.ToString().Split('|');

            LinkButton lnkDimensionShowHide = (LinkButton)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(e.CommandName)].FindControl("lnkDimensionShowHide");
            Panel pnlDimBody = (Panel)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(e.CommandName)].FindControl("pnlDimBody");

            if (lnkDimensionShowHide.Text == "(Show...)")
            {
                pnlDimBody.Visible = true;
                ListBox lstResponse = (ListBox)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(e.CommandName)].FindControl("lstResponse");
                HiddenField hfResponseGroupID = (HiddenField)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(e.CommandName)].FindControl("hfResponseGroupID");

                if (lstResponse.Items.Count == 0)
                {
                    lstResponse.DataTextField = "ResponseDesc";
                    lstResponse.DataValueField = "CodeSheetID";

                    List<CodeSheet> codesheet = CodeSheet.GetByResponseGroupID(Master.clientconnections, Convert.ToInt32(hfResponseGroupID.Value));

                    var CodeSheetQuery = (from cs in codesheet
                                          select new { cs.CodeSheetID, ResponseDesc = cs.ResponseDesc + ' ' + '(' + cs.ResponseValue + ')' });

                    lstResponse.DataSource = CodeSheetQuery;
                    lstResponse.DataBind();
                }

                lnkDimensionShowHide.Text = "(Hide...)";
            }
            else
            {
                lnkDimensionShowHide.Text = "(Show...)";
                pnlDimBody.Visible = false;
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

                if (args[1].ToString().ToUpper() == "DIMENSION")
                {
                    lst = (ListBox)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(e.CommandName)].FindControl("lstResponse");
                    HiddenField hfResponseGroupID = (HiddenField)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(e.CommandName)].FindControl("hfResponseGroupID");
                    hfResponseValue.Value = args[0];

                    if (lst.Items.Count == 0)
                    {
                        lst.DataTextField = "ResponseDesc";
                        lst.DataValueField = "CodeSheetID";

                        List<CodeSheet> codesheet = CodeSheet.GetByResponseGroupID(Master.clientconnections, Convert.ToInt32(hfResponseGroupID.Value));

                        var CodeSheetQuery = (from cs in codesheet
                                              select new { cs.CodeSheetID, ResponseDesc = cs.ResponseDesc + ' ' + '(' + cs.ResponseValue + ')' });

                        lst.DataSource = CodeSheetQuery;
                        lst.DataBind();
                    }
                }
            }
            else
                lblDimension.Text = e.CommandArgument.ToString();

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

                List<CodeSheet> codesheet = CodeSheet.GetByResponseGroupID(Master.clientconnections, Convert.ToInt32(hfResponseValue.Value));

                var CodeSheetQuery = (from cs in codesheet
                                      orderby cs.ResponseValue ascending
                                      select new { cs.CodeSheetID, ResponseDesc = cs.ResponseDesc + ' ' + '(' + cs.ResponseValue + ')' , cs.ResponseValue}).AsEnumerable().OrderBy(s => s.ResponseValue, new CustomComparer<string>()).ToList();

                rlbDimensionAvailable.DataValueField = "CodeSheetID";
                rlbDimensionAvailable.DataTextField = "ResponseDesc";
                rlbDimensionAvailable.DataSource = CodeSheetQuery;
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
                if (lblDimensionType.Text == "DIMENSION")
                {
                    lst = (ListBox)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(lblDimensionControl.Text)].FindControl("lstResponse");
                    LinkButton lnkDimensionShowHide = (LinkButton)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(lblDimensionControl.Text)].FindControl("lnkDimensionShowHide");
                    Panel pnlDimBody = (Panel)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(lblDimensionControl.Text)].FindControl("pnlDimBody");

                    lnkDimensionShowHide.Text = "(Hide...)";
                    pnlDimBody.Visible = true;
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

            if (lblDimension.Text.Equals("Country Regions", StringComparison.OrdinalIgnoreCase))
            {
                lstCountry.ClearSelection();
                SelectCountryOnRegion();
            }

        }

        #endregion
        
        #region Adhoc Popup Events

        protected void lnkAdhoc_Command(object sender, CommandEventArgs e)
        {
            AdhocFilter.Visible = true;
            AdhocFilter.LoadControls();
        }

        #endregion

        #region Activity Popup Events

        protected void lnkActivity_Command(object sender, CommandEventArgs e)
        {
            ActivityFilter.Visible = true;
            ActivityFilter.LoadControls();
        }

        #endregion

        #region Circulation Popup Events

        protected void lnkCirculation_Command(object sender, CommandEventArgs e)
        {
            CirculationFilter.Visible = true;
            CirculationFilter.ViewType = Enums.ViewType.ProductView;
            CirculationFilter.LoadCirculationControls();
        }

        #endregion

        #region Filter Popup Events

        protected void lnkSavedFilter_Command(object sender, CommandEventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.View))
            {
                ShowFilter.ShowFilterSegmentation = true;
                ShowFilter.BrandID = Convert.ToInt32(hfBrandID.Value);
                ShowFilter.PubID = Convert.ToInt32(hfProductID.Value);
                ShowFilter.UserID = Master.LoggedInUser;
                ShowFilter.ViewType = Enums.ViewType.ProductView;
                ShowFilter.Mode = "Load";
                ShowFilter.AllowMultiRowSelection = true;
                ShowFilter.LoadControls();
                ShowFilter.Visible = true;
            }
        }

        #endregion

        #region Save to Campaign Events

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
            string AddlFilters = string.Empty;
            SqlConnection conn = DataFunctions.GetClientSqlConnection(Master.clientconnections);

            DataTable dtRecords = null;
            bool ischecked = false;

            foreach (GridViewRow r in grdSubReport.Rows)
            {
                CheckBox chk = r.FindControl("chkSelectDownload") as CheckBox;

                if (chk.Checked)
                    ischecked = true;
            }

            if (!ischecked)
            {
                DisplaySaveCampaignPopupError("Please select a checkbox.");
                mdlPopupSaveCampaign.Show();
                mdlPopupDimensionReport.Show();
                return;
            }

            foreach (GridViewRow r in grdSubReport.Rows)
            {
                CheckBox chk = r.FindControl("chkSelectDownload") as CheckBox;

                if (chk.Checked)
                {
                    #region for selected items
                  
                    AddlFilters = " and s.SubscriptionID in (select distinct ss.SubscriptionID from Subscriptions ss join pubSubscriptionDetail psd2 on ss.SubscriptionID = psd2.SubscriptionID where psd2.codesheetID in (" + grdSubReport.DataKeys[r.RowIndex].Values[0].ToString() + "))";

                    dtRecords = Subscriber.GetSubscriberData(Master.clientconnections, Filter.generateCombinationQuery(fc, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, AddlFilters, Convert.ToInt32(hfProductID.Value), Convert.ToInt32(hfBrandID.Value), Master.clientconnections), "s.SubscriptionID");

                    if (chk.Checked && dtRecords.Rows.Count > 0)
                    {
                        if (rbPopupExistingCampaign.Checked)
                        {
                            PopupCampaignID = Convert.ToInt32(drpPopupCampaignName.SelectedValue);
                        }
                        else if (rbPopupNewCampaign.Checked)
                        {
                            if (PopupCampaignID == 0)
                            {
                                if (Campaigns.CampaignExists(Master.clientconnections, txtPopupCampaignName.Text) == 0)
                                    PopupCampaignID = Campaigns.Insert(Master.clientconnections, txtPopupCampaignName.Text, Master.LoggedInUser, Convert.ToInt32(hfBrandID.Value));
                                else
                                {
                                    DisplaySaveCampaignPopupError("ERROR - <font color='#000000'>\"" + txtPopupCampaignName.Text + "\"</font> already exists. Please enter a different name.");
                                    mdlPopupSaveCampaign.Show();
                                    mdlPopupDimensionReport.Show();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            DisplaySaveCampaignPopupError("Select existing Campaign or new Campaign");
                            mdlPopupSaveCampaign.Show();
                            mdlPopupDimensionReport.Show();
                            return;
                        }

                        if (PopupCampaignFilterID == 0)
                        {
                            string filtername = grdSubReport.DataKeys[r.RowIndex].Values[2].ToString();

                            PopupCampaignFilterID = CampaignFilter.CampaignFilterExists(Master.clientconnections, filtername, PopupCampaignID);

                            if (CampaignFilter.CampaignFilterExists(Master.clientconnections, filtername, PopupCampaignID) == 0)
                                PopupCampaignFilterID = CampaignFilter.Insert(Master.clientconnections, filtername, Master.LoggedInUser, PopupCampaignID, txtPromocode.Text);
                        }

                        StringBuilder xmlSubscriber = new StringBuilder("");
                        int cnt = 0;
                        List<int> LNth = Utilities.getNth(dtRecords.Rows.Count, dtRecords.Rows.Count);

                        try
                        {
                            foreach (int n in LNth)
                            {
                                DataRow dr = dtRecords.Rows[n];

                                xmlSubscriber.Append("<sID>" + Utilities.cleanXMLString(dr["SubscriptionID"].ToString()) + "</sID>");

                                if ((cnt != 0) && (cnt % 10000 == 0) || (cnt == LNth.Count - 1))
                                {
                                    CampaignFilterDetails.saveCampaignDetails(Master.clientconnections, PopupCampaignFilterID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlSubscriber.ToString() + "</XML>");
                                    xmlSubscriber = new StringBuilder("");
                                }

                                cnt++;
                            }

                        }
                        catch (Exception ex)
                        {
                            DisplaySaveCampaignPopupError("ERROR - " + ex.Message);
                        }

                        PopupCampaignFilterID = 0;
                     }

                    PopupCampaignFilterAllID = 0;

                    #endregion
                }
            }

            lblPopupResult.Visible = true;
            lblPopupResult.Text = "Total subscriber in the campaign : " + Campaigns.GetCountByCampaignID(Master.clientconnections, PopupCampaignID);
            PopupCampaignID = 0;
            mdlPopupDimensionReport.Show();
            this.mdlPopupSaveCampaign.Show();
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

        #endregion

        protected void TabContainer_OnActiveTabChanged(object sender, EventArgs e)
        {
            if (TabContainer.ActiveTabIndex == 1)
            {
                FilterSegmentation.Visible = true;
                FilterSegmentation.ViewType = Enums.ViewType.ProductView;
                FilterSegmentation.UserID = Master.LoggedInUser;
                FilterSegmentation.fcSessionName = fcSessionName;
                FilterSegmentation.BrandID = Convert.ToInt32(hfBrandID.Value);
                FilterSegmentation.PubID = Convert.ToInt32(hfProductID.Value);
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
            LoadProducts();
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

        private int IntTryParse(string input)
        {
            int returnValue;

            if (!int.TryParse(input, out returnValue))
            {
                throw new InvalidOperationException($"{input} cannot be parsed to integet");
            }

            return returnValue;
        }
    }
}
