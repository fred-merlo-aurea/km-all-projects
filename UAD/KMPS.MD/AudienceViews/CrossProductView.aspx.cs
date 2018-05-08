using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrameworkUAD.BusinessLogic;
using KM.Common;
using KM.Common.Extensions;
using KMPS.MD.Helpers;
using KMPS.MD.Objects;
using Microsoft.Reporting.WebForms;
using Telerik.Web.UI;
using Brand = KMPS.MD.Objects.Brand;
using CodeSheet = KMPS.MD.Objects.CodeSheet;
using DataFunctions = KMPS.MD.Objects.DataFunctions;
using EmailStatus = KMPS.MD.Objects.EmailStatus;
using Enums = KMPS.MD.Objects.Enums;
using Filter = KMPS.MD.Objects.Filter;
using ResponseGroup = KMPS.MD.Objects.ResponseGroup;
using MDControls = KMPS.MD.Controls;

namespace KMPS.MD.Main
{
    public partial class CrossProductView : AudienceViewBase
    {
        private const string NameCancel = "Cancel";
        private const string NameEdit = "Edit";
        private const string NameLnkCancel = "lnkCancel";
        private const string NameLnkEdit = "lnkEdit";
        private const string NameCrossProduct = "Cross Product";
        private const string ResponseValueField = "ResponseValue";

        int GrdRowCount = 0;
        protected override Panel PnlDataCompare => null;
        protected override DropDownList DrpDataCompareSourceFile => null;
        protected override RadioButtonList RblLoadType => rblLoadType;
        protected override RadioButtonList RblDataCompareOperation => null;
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
                    if (pnlBrand.Visible)
                    {
                        drpBrand.Enabled = false;
                    }

                    LoadGridFilters();
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
                    if (pnlBrand.Visible)
                    {
                        drpBrand.Enabled = false;
                    }

                    LoadGridFilters();

                    FilterSegmentation.Visible = true;
                    FilterSegmentation.ViewType = Enums.ViewType.CrossProductView;
                    FilterSegmentation.UserID = Master.LoggedInUser;
                    FilterSegmentation.fcSessionName = fcSessionName;
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
            DownloadPanel1.SubscriptionID = null;
            DownloadPanel1.SubscribersQueries = GetSubscribersQueriesForUserControl();
            if (Convert.ToInt32(lblDownloadCount.Text) > 0)
            {
                DownloadPanel1.Visible = true;
                DownloadPanel1.HeaderText = Utilities.GetHeaderText(fc, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, TabContainer.ActiveTabIndex == 1 ? true : false);
                DownloadPanel1.ShowHeaderCheckBox = true;
                DownloadPanel1.PubIDs = null;
                DownloadPanel1.ViewType = Enums.ViewType.CrossProductView;
                DownloadPanel1.filterCombination = lblFilterCombination.Text;
                DownloadPanel1.downloadCount = Convert.ToInt32(lblDownloadCount.Text);
                DownloadPanel1.LoadControls();
                DownloadPanel1.ValidateExportPermission();
                DownloadPanel1.LoadDownloadTemplate();
                DownloadPanel1.loadExportFields();

                if (lblSelectedFilterOperation.Text.ToUpper() == "SINGLE")
                {
                    foreach (Filter f in fc)
                    {
                        if (f.FilterNo == Convert.ToInt32(lblSelectedFilterNos.Text))
                            DownloadPanel1.PubIDs = f.PubID.ToString().Split(',').Select(int.Parse).ToList();
                    }
                }
            }
        }

        private void ShowCLDownloadPanel()
        {
            CLDownloadPanel.SubscribersQueries = GetSubscribersQueriesForUserControl();
            CLDownloadPanel.Visible = true;
            CLDownloadPanel.HeaderText = Utilities.GetHeaderText(fc, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, TabContainer.ActiveTabIndex == 1 ? true : false);
            CLDownloadPanel.ShowHeaderCheckBox = true;
            CLDownloadPanel.PubIDs = null;
            CLDownloadPanel.ViewType = Enums.ViewType.CrossProductView;
            CLDownloadPanel.filterCombination = lblFilterCombination.Text;
            CLDownloadPanel.downloadCount = Convert.ToInt32(lblDownloadCount.Text);
            CLDownloadPanel.LoadControls();
            CLDownloadPanel.LoadDownloadTemplate();
            CLDownloadPanel.loadExportFields();
            CLDownloadPanel.ValidateDownload();
            if (lblSelectedFilterOperation.Text.ToUpper() == "SINGLE")
            {
                foreach (Filter f in fc)
                {
                    if (f.FilterNo == Convert.ToInt32(lblSelectedFilterNos.Text))
                        CLDownloadPanel.PubIDs = f.PubID.ToString().Split(',').Select(int.Parse).ToList();
                }
            }
        }

        private void ShowEVDownloadPanel()
        {
            EVDownloadPanel.SubscribersQueries = GetSubscribersQueriesForUserControl();
            EVDownloadPanel.Visible = true;
            EVDownloadPanel.HeaderText = Utilities.GetHeaderText(fc, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, TabContainer.ActiveTabIndex == 1 ? true : false);
            EVDownloadPanel.ShowHeaderCheckBox = true;
            EVDownloadPanel.PubIDs = null;
            EVDownloadPanel.ViewType = Enums.ViewType.CrossProductView;
            EVDownloadPanel.filterCombination = lblFilterCombination.Text;
            EVDownloadPanel.downloadCount = Convert.ToInt32(lblDownloadCount.Text);
            EVDownloadPanel.LoadControls();
            EVDownloadPanel.LoadDownloadTemplate();
            EVDownloadPanel.loadExportFields();
            EVDownloadPanel.ValidateDownload();
            if (lblSelectedFilterOperation.Text.ToUpper() == "SINGLE")
            {
                foreach (Filter f in fc)
                {
                    if (f.FilterNo == Convert.ToInt32(lblSelectedFilterNos.Text))
                        EVDownloadPanel.PubIDs = f.PubID.ToString().Split(',').Select(int.Parse).ToList();
                }
            }
        }

        private StringBuilder GetSubscribersQueriesForUserControl()
        {
            StringBuilder Queries = new StringBuilder();
            string addFilters = string.Empty;

            try
            {
                //DownloadList = Filter.getSubscriptionIDforFilterOperation(Master.clientconnections, fc, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, Selected_FilterID, Suppressed_FilterID, AddlFilters, lblAddFilters2.Text, 0, Convert.ToInt32(hfBrandID.Value), lblMasterID.Text);

                Queries = Filter.generateCombinationQuery(fc, lblSelectedFilterOperation.Text, lblSuppressedFilterOperation.Text, lblSelectedFilterNos.Text, lblSuppressedFilterNos.Text, addFilters,  0, Convert.ToInt32(hfBrandID.Value), Master.clientconnections);
            }
            catch (Exception ex)
            {
                Utilities.Log_Error(Request.RawUrl.ToString(), "GetSubscribersQueriesForUserControl", ex);
                DisplayError(ex.Message);
            }

            lblAddFilters.Text = string.Empty;

            return Queries;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = NameAudienceViews;
            Master.SubMenu = NameCrossProduct;

            InitializeCommonControls();

            if (!IsPostBack)
            {
                RedirectIfNoViewAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.ServiceFeatures.CrossProductView);
                InitializeForHttpGet(ResponseValueField, Enums.ViewType.CrossProductView);
            }
            else
            {
                InitializeForPostBack(grdFilters, hfFilterGroupName);
            }
        }

        // This method is left empty intentionly, to override the default implementation.
        // There is no need to AddDataCompareField in CrossProductView.
        protected override void AddDataCompareFieldIfSourceFileSelected(Filter filter)
        {

        }

        private void InitializeCommonControls()
        {
            SetDelegatesAndCommands();

            lblErrorMsg.Text = string.Empty;
            divErrorMsg.Visible = false;
            txtShowQuery.Text = string.Empty;

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

            FilterSegmentation.lnkCountCommand += lnkCount_Command;
            FilterSegmentation.lnkCompanyLocationViewCommand += lnkCompanyLocationView_Command;
            FilterSegmentation.lnkEmailViewCommand += lnkEmailView_Command;
            FilterSegmentation.lnkGeoMapsCommand += lnkGeoMaps_Command;
            FilterSegmentation.lnkGeoReportCommand += lnkGeoReport_Command;
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

        protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            fc.Clear();
            FilterCollection.Clear();
            ResetAllFitlerControls();
            LoadGridFilters();

            imglogo.ImageUrl = string.Empty;
            imglogo.Visible = false;
            hfBrandID.Value = drpBrand.SelectedValue;
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
            {
                LoadProducts();
            }

            DownloadPanel1.BrandID = Convert.ToInt32(hfBrandID.Value);
            DownloadPanel1.ViewType = Enums.ViewType.CrossProductView;
            CLDownloadPanel.BrandID = Convert.ToInt32(hfBrandID.Value);
            DownloadPanel1.ViewType = Enums.ViewType.CrossProductView;
            EVDownloadPanel.BrandID = Convert.ToInt32(hfBrandID.Value);
            DownloadPanel1.ViewType = Enums.ViewType.CrossProductView;
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
                         where (p.PubName.ToLower().Contains(e.Text.ToLower().Trim()) || p.PubCode.ToLower().Contains(e.Text.ToLower().Trim()))
                         orderby p.PubName ascending
                         select p).ToList(); ;
            }

            rcbProduct.DataSource = lpubs;
            rcbProduct.DataBind();
            rcbProduct.Items.Insert(0, new RadComboBoxItem("", "0"));
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMsg.Text = errorMessage;
            divErrorMsg.Visible = true;
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
                GrdRowCount = 0;
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
            Filter filter = fc.SingleOrDefault(f => f.FilterNo == filterNo);
            return filter.Fields;
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
                        ClearAndResetFilterTabControls();
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

            var filter = new Filter { ViewType = Enums.ViewType.CrossProductView, PubID = hiddenFieldProductId };
            var responseGroup = ResponseGroup.GetActiveByPubID(Master.clientconnections, hiddenFieldProductId);
            this.AddFilterFieldsListItems(responseGroup, filter);
            int hiddenFieldBrandId;

            if (!int.TryParse(hfBrandID.Value, out hiddenFieldBrandId))
            {
                throw new InvalidOperationException($"Unable to parse {hiddenFieldBrandId}");
            }

            if (pnlBrand.Visible && hiddenFieldBrandId > 0)
            {
                filter.BrandID = drpBrand.Visible ? IntTryParse(drpBrand.SelectedItem.Value) : hiddenFieldBrandId;
                filter.Fields.Add(
                    drpBrand.Visible
                        ? new Field(NameBrand, drpBrand.SelectedItem.Value, drpBrand.SelectedItem.Text, string.Empty, Enums.FiltersType.Brand, NameBrand)
                        : new Field(NameBrand, hfBrandID.Value, lblBrandName.Text, string.Empty, Enums.FiltersType.Brand, NameBrand));
            }

            if (!string.IsNullOrWhiteSpace(rcbProduct.SelectedItem.Value))
            {
                filter.Fields.Add(new Field(NameProduct, rcbProduct.SelectedItem.Value, rcbProduct.SelectedItem.Text, string.Empty, Enums.FiltersType.Product, NameProduct.ToUpper()));
            }

            var listValues = Utilities.getListboxSelectedValues(lstGeoCode);

            if (!string.IsNullOrWhiteSpace(listValues))
            {
                SelectStateOnRegion();
            }

            listValues = Utilities.getListboxSelectedValues(lstState);

            if (!string.IsNullOrWhiteSpace(listValues))
            {
                filter.Fields.Add(new Field(NameState, listValues, Utilities.getListboxText(lstState), string.Empty, Enums.FiltersType.Standard, NameState.ToUpper()));
            }

            listValues = Utilities.getListboxSelectedValues(lstCountry);

            if (!string.IsNullOrWhiteSpace(listValues))
            {
                filter.Fields.Add(new Field(NameCountry, listValues, Utilities.getListboxText(lstCountry), string.Empty, Enums.FiltersType.Standard, NameCountry.ToUpper()));
            }

            this.AddFilterFieldsComboBox(filter);
            this.AddFilterFieldsGeoAdhocActivityCirculationPopup(filter);

            return filter;
        }

        protected void lnkResetAll_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(hfBrandID.Value) > 0)
                Response.Redirect("CrossProductView.aspx?brandID=" + Convert.ToInt32(hfBrandID.Value));
            else
                Response.Redirect("CrossProductView.aspx");
        }

        protected void lnkdownload_Command(object sender, CommandEventArgs e)
        {
            CampaignID = 0;
            CampaignFilterID = 0;
            string temp = e.CommandArgument.ToString();
            string[] args = temp.Split('/');
            lblSelectedFilterNos.Text = args[0];
            lblSuppressedFilterNos.Text = args[1];
            lblSelectedFilterOperation.Text = args[2];
            lblSuppressedFilterOperation.Text = args[3];
            lblFilterCombination.Text = args[4];
            lblDownloadCount.Text = args[5];
            ShowDownloadPanel();
        }

        protected void lnkdownloadAllUnion_Command(object sender, CommandEventArgs e)
        {
            CampaignID = 0;
            CampaignFilterID = 0;
            lblSelectedFilterNos.Text = string.Join(",", fc.Select(f => f.FilterNo));
            lblSuppressedFilterNos.Text = string.Empty;
            lblSelectedFilterOperation.Text = "Union";
            lblSuppressedFilterOperation.Text = string.Empty;
            lblFilterCombination.Text = "All Union";
            lblDownloadCount.Text = e.CommandArgument.ToString();
            ShowDownloadPanel();
        }

        protected void lnkdownloadAllIntersect_Command(object sender, CommandEventArgs e)
        {
            CampaignID = 0;
            CampaignFilterID = 0;
            lblSelectedFilterNos.Text = string.Join(",", fc.Select(f => f.FilterNo));
            lblSuppressedFilterNos.Text = string.Empty;
            lblSelectedFilterOperation.Text = "Intersect";
            lblSuppressedFilterOperation.Text = string.Empty;
            lblFilterCombination.Text = "All Intersect";
            lblDownloadCount.Text = e.CommandArgument.ToString();
            ShowDownloadPanel();
        }

        protected void lnkCount_Command(object sender, CommandEventArgs e)
        {
            string temp = e.CommandArgument.ToString();
            string[] args = temp.Split('/');
            lblSelectedFilterNos.Text = args[0];
            lblSuppressedFilterNos.Text = args[1];
            lblSelectedFilterOperation.Text = args[2];
            lblSuppressedFilterOperation.Text = args[3];
            lblFilterCombination.Text = args[4];
            lblDownloadCount.Text = args[5];
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
            if (e.CommandName == NameCancel)
            {
                var row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                SetChildVisibility(row, NameLnkCancel, false);
                SetChildVisibility(row, NameLnkEdit, true);
                ClearAndResetFilterTabControls();
                hfFilterNo.Value = string.Empty;
                hfFilterName.Value = string.Empty;
                hfFilterGroupName.Value = string.Empty;
            }
            else if (e.CommandName == NameEdit)
            {
                foreach (GridViewRow filterRow in grdFilters.Rows)
                {
                    SetChildVisibility(filterRow, NameLnkCancel, false);
                    SetChildVisibility(filterRow, NameLnkEdit, true);
                }

                var row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                SetChildVisibility(row, NameLnkCancel, true);
                SetChildVisibility(row, NameLnkEdit, false);
                ClearAndResetFilterTabControls();

                var filter = fc.Single(f => f.FilterNo == Convert.ToInt32(e.CommandArgument.ToString()));
                hfFilterNo.Value = e.CommandArgument.ToString();
                hfFilterName.Value = filter.FilterName;
                hfFilterGroupName.Value = filter.FilterGroupName;
                rcbProduct.SelectedValue = filter.PubID.ToString();
                hfProductID.Value = rcbProduct.SelectedValue;
                getResponseGroup();
                AdhocFilter.BrandID = Convert.ToInt32(hfBrandID.Value);
                AdhocFilter.LoadAdhocGrid();
                lnkActivity.Enabled = true;
                lnkAdhoc.Enabled = true;

                EditByFilterType(filter);
            }
        }

        private void EditByFilterType(Filter filter)
        {
            Guard.NotNull(filter, nameof(filter));
            foreach (var field in filter.Fields)
            {
                switch (field.FilterType)
                {
                    case Enums.FiltersType.Dimension:
                        EditByFilterTypeDimension(field);
                        break;

                    case Enums.FiltersType.Standard:
                        EditByFilterTypeStandard(field);
                        break;

                    case Enums.FiltersType.Geo:
                        EditByFilterTypeGeo(field);
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

        private void EditByFilterTypeDimension(Field field)
        {
            Guard.NotNull(field, nameof(field));
            foreach (DataListItem dimensionItem in dlDimensions.Items)
            {
                var lstResponse = (ListBox)dimensionItem.FindControl("lstResponse");
                var hfResponseGroupId = (HiddenField)dimensionItem.FindControl("hfResponseGroupID");
                var lnkDimensionShowHide = (LinkButton)dimensionItem.FindControl("lnkDimensionShowHide");
                var pnlDimBody = (Panel)dimensionItem.FindControl("pnlDimBody");

                var resGroupName = ResponseGroup
                    .GetByResponseGroupID(Master.clientconnections, Convert.ToInt32(hfResponseGroupId.Value)).ResponseGroupName;
                if (resGroupName.EqualsIgnoreCase(field.Group))
                {
                    if (lnkDimensionShowHide.Text == "(Show...)")
                    {
                        pnlDimBody.Visible = true;

                        if (lstResponse.Items.Count == 0)
                        {
                            lstResponse.DataTextField = "ResponseDesc";
                            lstResponse.DataValueField = "CodeSheetID";

                            var codeSheet = CodeSheet.GetByResponseGroupID(Master.clientconnections,
                                Convert.ToInt32(hfResponseGroupId.Value));

                            var codeSheetQuery = codeSheet.Select(cs => new
                            {
                                cs.CodeSheetID,
                                ResponseDesc = $"{cs.ResponseDesc} ({cs.ResponseValue}{')'}"
                            });

                            lstResponse.DataSource = codeSheetQuery;
                            lstResponse.DataBind();
                        }

                        lnkDimensionShowHide.Text = "(Hide...)";
                    }

                    Utilities.SelectFilterListBoxes(lstResponse, field.Values);
                }
            }
        }

        private void EditByFilterTypeStandard(Field field)
        {
            Guard.NotNull(field, nameof(field));
            switch (field.Name.ToUpper())
            {
                case NameStateUpper:
                    Utilities.SelectFilterListBoxes(lstState, field.Values);
                    break;
                case NameCountryUpper:
                    Utilities.SelectFilterListBoxes(lstCountry, field.Values);
                    break;
                case NameEmailUpper:
                    Utilities.SelectFilterRadComboBox(rcbEmail, field.Values);
                    break;
                case NamePhoneUpper:
                    Utilities.SelectFilterRadComboBox(rcbPhone, field.Values);
                    break;
                case NameFaxUpper:
                    Utilities.SelectFilterRadComboBox(rcbFax, field.Values);
                    break;
                case NameMediaUpper:
                    Utilities.SelectFilterRadComboBox(rcbMedia, field.Values);
                    break;
                case NameGeoLocatedUpper:
                    Utilities.SelectFilterRadComboBox(rcbIsLatLonValid, field.Values);
                    break;
                case NameMailPermissionUpper:
                    Utilities.SelectFilterRadComboBox(rcbMailPermission, field.Values);
                    break;
                case NameFaxPermissionUpper:
                    Utilities.SelectFilterRadComboBox(rcbFaxPermission, field.Values);
                    break;
                case NamePhonePermissionUpper:
                    Utilities.SelectFilterRadComboBox(rcbPhonePermission, field.Values);
                    break;
                case NameOtherProductsPermissionUpper:
                    Utilities.SelectFilterRadComboBox(rcbOtherProductsPermission, field.Values);
                    break;
                case NameThirdPartyPermissionUpper:
                    Utilities.SelectFilterRadComboBox(rcbThirdPartyPermission, field.Values);
                    break;
                case NameEmailRenewPermissionUpper:
                    Utilities.SelectFilterRadComboBox(rcbEmailRenewPermission, field.Values);
                    break;
                case NameTextPermissionUpper:
                    Utilities.SelectFilterRadComboBox(rcbTextPermission, field.Values);
                    break;
                case NameEmailStatusPermissionUpper:
                    Utilities.SelectFilterRadComboBox(rcbEmailStatus, field.Values);
                    break;
            }
        }

        private void EditByFilterTypeGeo(Field field)
        {
            Guard.NotNull(field, nameof(field));
            var fieldValues = field.SearchCondition.Split('|');

            if (fieldValues.Length < 3)
            {
                throw new InvalidOperationException($"Unable to get 3 values for GEO edit from string '{field.SearchCondition}'");
            }

            txtRadiusMax.Text = fieldValues[2];
            txtRadiusMin.Text = fieldValues[1];

            int number;
            if (int.TryParse(fieldValues[0], out number))
            {
                RadMtxtboxZipCode.Mask = "#####";
                drpCountry.SelectedValue = NameUnitedStates;
            }
            else
            {
                RadMtxtboxZipCode.Mask = "L#L #L#";
                drpCountry.SelectedValue = NameCanadaUpper;
            }

            RadMtxtboxZipCode.Text = fieldValues[0];
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

        void btn_Click(object sender, CommandEventArgs e)
        {
            string test = e.ToString();
        }

        protected void grdFilters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GrdRowCount++;

                Label lblID = (Label)e.Row.FindControl("lblID");
                lblID.Text = GrdRowCount.ToString();

                GridView grdFilterValues = (GridView)e.Row.FindControl("grdFilterValues");
                List<Field> grdFilterList = LoadGridFilterValues(Convert.ToInt32(grdFilters.DataKeys[e.Row.RowIndex].Value.ToString()));
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
                LinkButton lnkdownloadAllUnion = (LinkButton)e.Row.FindControl("lnkdownloadAllUnion");
                LinkButton lnkdownloadAllIntersect = (LinkButton)e.Row.FindControl("lnkdownloadAllIntersect");

                if (fc.Count > 1)
                {
                    //Label lblAllUnion = (Label)e.Row.FindControl("lblAllUnion");
                    //Label lblAllIntersect = (Label)e.Row.FindControl("lblAllIntersect");

                    //lblAllUnion.Text = "All Union : ";
                    //lblAllIntersect.Text = "All Intersect : ";

                    //int AllUnionCount = fc.AllUnion;

                    //if (AllUnionCount <= 0)
                    //    lnkdownloadAllUnion.Enabled = false;

                    //lnkdownloadAllUnion.Text = AllUnionCount.ToString();
                    //lnkdownloadAllUnion.CommandArgument = AllUnionCount.ToString();
                    //lnkdownloadAllUnion.CommandName = "download";

                    //int AllIntersectCount = fc.AllInterSect;

                    //if (AllIntersectCount <= 0)
                    //    lnkdownloadAllIntersect.Enabled = false;

                    //lnkdownloadAllIntersect.Text = AllIntersectCount.ToString();
                    //lnkdownloadAllIntersect.CommandArgument = AllIntersectCount.ToString();
                    //lnkdownloadAllIntersect.CommandName = "download";
                }
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
                var json = JsonHelper.GetJsonStringFromDataTable(locations);
                myMapCoords.Text = json;
            }
            this.mdlPopupGeoMap.Show();
        }
        
        private DataTable LoadGeoData(string procname, StringBuilder Queries)
        {
            SqlCommand cmd = new SqlCommand(procname, DataFunctions.GetClientSqlConnection(Master.clientconnections));
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Queries", SqlDbType.Text)).Value = Queries.ToString();
            return DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(Master.clientconnections));
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
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], LoadGeoData("sp_rpt_Qualified_Breakdown_Canada", Queries)));
                    break;
                case "GeoDomestic":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoBreakdown_Domestic.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], LoadGeoData("sp_rpt_Qualified_Breakdown_Domestic", Queries)));
                    break;
                case "GeoInternational":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoBreakdown_by_Country.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], LoadGeoData("sp_rpt_Qualified_Breakdown_by_country", Queries)));
                    break;
                case "GeoPermissionCanada":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoPermission_Canada.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], LoadGeoData("sp_rpt_Qualified_Breakdown_Canada", Queries)));
                    break;
                case "GeoPermissionDomestic":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoPermission_Domestic.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], LoadGeoData("sp_rpt_Qualified_Breakdown_Domestic", Queries)));
                    break;
                case "GeoPermissionInternational":
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoPermission_by_Country.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], LoadGeoData("sp_rpt_Qualified_Breakdown_by_country", Queries)));
                    break;
            }
            ReportViewer1.LocalReport.Refresh();
        }

        protected void lnkCompanyLocationView_Command(object sender, CommandEventArgs e)
        {
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
            ResetAllFitlerControls();
            LoadGridFilters();

            if (pnlBrand.Visible)
                drpBrand.Enabled = true;

            TabContainer.ActiveTabIndex = 0;
        }

        private void ResetFilterControls()
        {
            ClearAndResetFilterTabControls();
            FilterSegmentation.ResetControls();
        }

        private void ResetAllFitlerControls()
        {
            rcbProduct.Enabled = true;
            rcbProduct.Text = "";
            rcbProduct.ClearSelection();
            lnkAdhoc.Enabled = false;
            lnkActivity.Enabled = false;
            lnkCirculation.Enabled = false;
            ResetFilterControls();

            dlDimensions.DataSource = null;
            dlDimensions.DataBind();
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
            if (!HandleBtnOpenSaveFilterPopupClick(grdFilters, FilterSave, hfBrandID.Value, Enums.ViewType.CrossProductView))
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
                                      select new { cs.CodeSheetID, ResponseDesc = cs.ResponseDesc + ' ' + '(' + cs.ResponseValue + ')', cs.ResponseValue }).AsEnumerable().OrderBy(s => s.ResponseValue, new CustomComparer<string>()).ToList();

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
            CirculationFilter.ViewType = Enums.ViewType.CrossProductView;
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
                ShowFilter.ViewType = Enums.ViewType.CrossProductView;
                ShowFilter.Mode = "Load";
                ShowFilter.AllowMultiRowSelection = true;
                ShowFilter.LoadControls();
                ShowFilter.Visible = true;
            }
        }

        #endregion

        protected void TabContainer_OnActiveTabChanged(object sender, EventArgs e)
        {
            if (TabContainer.ActiveTabIndex == 1)
            {
                FilterSegmentation.Visible = true;
                FilterSegmentation.ViewType = Enums.ViewType.CrossProductView;
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
    }
}