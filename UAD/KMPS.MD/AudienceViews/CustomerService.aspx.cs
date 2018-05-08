using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using FrameworkUAD.BusinessLogic;
using KM.Common;
using KM.Common.Extensions;
using KMPS.MD.Objects;
using Microsoft.Reporting.WebForms;
using Telerik.Web.UI;
using Brand = KMPS.MD.Objects.Brand;
using DiagnosticsTrace = System.Diagnostics.Trace;
using EmailStatus = KMPS.MD.Objects.EmailStatus;
using Enums = KMPS.MD.Objects.Enums;
using Filter = KMPS.MD.Objects.Filter;
using MasterCodeSheet = KMPS.MD.Objects.MasterCodeSheet;
using MasterGroup = KMPS.MD.Objects.MasterGroup;
using Subscriber = KMPS.MD.Objects.Subscriber;
using UserDataMask = KMPS.MD.Objects.UserDataMask;

namespace KMPS.MD.Main
{
    public partial class CustomerService : BrandsPageBase
    {
        private const string MonthDateYearFormat = "MM/dd/yyyy";
        private const string Zero = "0";
        private const string One = "1";
        private const string Two = "2";
        private const string FourHundredTwentyNine = "429";
        private const string MinusOne = "-1";
        private const string BlackCircle = "\u25cf\u25cf";
        private const string ControlRblDimensionSelected = "rlbDimensionSelected";
        private const string TypeMarket = "MARKETS";
        private const string TypeDimension = "DIMENSION";
        private const string TypeLstMarket = "LSTMARKET";
        private const string TypePubType = "PUBTYPE";
        private const string DimensionCountryRegion = "Country Regions";
        private const char DelimiterPipeChar = '|';
        private const int ArgIndexDimensionType = 1;
        private const int ArgIndexDimensionValue = 2;
        private const int DimensionsAvailableWideWidth = 900;
        private const int DimensionsAvailableNarrowWidth = 465;
        private const int ButtonSettingsWidth = 35;
        private const string FieldMasterDesc = "MASTERDESC";
        private const string FieldMasterId = "MasterID";
        private const string FieldPubId = "PubID";
        private const string FieldPubName = "PubName";
        Filters fc;
        delegate void LoadSelectedFilterData(List<int> filterIDs);
        
        protected override Panel BrandPanel => pnlBrand;
        protected override DropDownList BrandDropDown => drpBrand;
        protected override HiddenField BrandIdHiddenField => hfBrandID;
        protected override Label BrandNameLabel => lblBrandName;

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

        private string fcSessionName
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

        private string guid
        {
            get
            {
                return ViewState["guid"].ToString();
            }
            set
            {
                ViewState["guid"] = value;
            }
        }

        private string SortField
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

        private string SortDirection
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

        delegate void HidePanel();

        private void AdhocPopupHide()
        {
            AdhocFilter.Visible = false;
        }

        private void ActivityPopupHide()
        {
            ActivityFilter.Visible = false;
        }

        private void ShowFilterPopupHide()
        {
            ShowFilter.Visible = false;
        }

        private void CirculationPopupHide()
        {
            CirculationFilter.Visible = false;
        }

        public void LoadFilterData(List<int> filterIDs)
        {
            ResetFitlerControls();

            try
            {
                Cache.Remove(guid);
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }

            Filters fcNew;

            try
            {
                foreach (int fID in filterIDs)
                {
                    fcNew = MDFilter.LoadFilters(Master.clientconnections, fID, Master.LoggedInUser);

                    foreach (Filter f in fcNew)
                    {
                        f.FilterNo = fc.Count + 1;
                        fc.Add(f);
                    }

                }

                if (fc.Count > 0)
                {
                    fc.Execute();
                    if (pnlBrand.Visible)
                        drpBrand.Enabled = false;

                    FilterCollection = fc;

                    List<Subscriber> ls = LoadGrid();

                    rgSubscriberList.DataSource = ls;
                    rgSubscriberList.DataBind();

                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecordDetails, KMPlatform.Enums.Access.Edit))
                        rgSubscriberList.Columns[9].Visible = false;

                    if (ls.Count > 0)
                        dvResults.Style.Add("Display", "inline");
                    else
                        dvResults.Style.Add("Display", "none");
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Audience Views";
            Master.SubMenu = "Record View";
            divError.Visible = false;
            lblErrorMessage.Text = string.Empty;
            lblMsgEditAddress.Text = "";
            divEditAddress.Visible = false;

            HidePanel delAdhoc = new HidePanel(AdhocPopupHide);
            this.AdhocFilter.hideAdhocPopup = delAdhoc;

            HidePanel delActivity = new HidePanel(ActivityPopupHide);
            this.ActivityFilter.hideActivityPopup = delActivity;

            HidePanel delShowFilter = new HidePanel(ShowFilterPopupHide);
            this.ShowFilter.hideShowFilterPopup = delShowFilter;

            HidePanel delCirculation = new HidePanel(CirculationPopupHide);
            this.CirculationFilter.hideCirculationPopup = delCirculation;

            LoadSelectedFilterData delNoParamFilterID = new LoadSelectedFilterData(LoadFilterData);
            this.ShowFilter.LoadSelectedFilterData = delNoParamFilterID;

            fc = FilterCollection;

            if (!IsPostBack)
            {
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecordDetails, KMPlatform.Enums.Access.View))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.View))
                {
                    lnkSavedFilter.Visible = false;
                }

                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecordDetails, KMPlatform.Enums.Access.Edit))
                {
                    txt_FirstName.ReadOnly = false;
                    txt_LastName.ReadOnly = false;
                    txt_Company.ReadOnly = false;
                    txt_Address.ReadOnly = false;
                    txt_Address2.ReadOnly = false;
                    txt_Address3.ReadOnly = false;
                    txt_City.ReadOnly = false;
                    drp_State.Enabled = true;
                    txt_State.ReadOnly = false;
                    txt_Zip.ReadOnly = false;
                    drp_Country.Enabled = true;
                    txt_Title.ReadOnly = false;
                    txt_ForZip.ReadOnly = false;
                    txt_Plus4.ReadOnly = false;
                    txt_Phone.ReadOnly = false;
                    txt_Fax.ReadOnly = false;
                    txt_Mobile.ReadOnly = false;
                    txt_Email.ReadOnly = false;
                    txt_Notes.ReadOnly = false;
                    drp_MailPermission.Enabled = true;
                    drp_FaxPermission.Enabled = true;
                    drp_PhonePermission.Enabled = true;
                    drp_OtherProductsPermission.Enabled = true;
                    drp_ThirdPartyPermission.Enabled = true;
                    drp_EmailRenewPermission.Enabled = true;
                    drp_TextPermission.Enabled = true;
                    btnSave.Visible = true;
                }

                if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    List<UserDataMask> udm = UserDataMask.GetByUserID(Master.clientconnections, Master.UserSession.UserID);
                    string maskField = string.Empty;

                    foreach (UserDataMask u in udm)
                    {
                        TextBox tb = (TextBox)TabSubscriberDetails.FindControl("txt_" + u.MaskField);
                        tb.Attributes["type"] = "password";
                        tb.ReadOnly = true;

                        if (u.MaskField.ToUpper() == "EMAIL")
                        {
                            revEmail.Enabled = false;
                        }

                        if (u.MaskField.ToUpper() == "EMAIL" || u.MaskField.ToUpper() == "FIRSTNAME" || u.MaskField.ToUpper() == "LASTNAME" || u.MaskField.ToUpper() == "FIRSTNAME" || u.MaskField.ToUpper() == "COMPANY" || u.MaskField.ToUpper() == "PHONE")
                        {
                            PlaceHolder phlbl = (PlaceHolder)pnlGlobalFilterBody.FindControl("phlbl" + u.MaskField);
                            phlbl.Visible = false;

                            PlaceHolder phtxt = (PlaceHolder)pnlGlobalFilterBody.FindControl("phtxt" + u.MaskField);
                            phtxt.Visible = false;
                        }
                    }
                }

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecordDetails, KMPlatform.Enums.Access.Download))
                {

                    btnDownload.Visible = false;
                }

                #region Load listbox & dropdowns

                LoadBrands();
                loadStandardFiltersListboxes();

                rcbEmailStatus.DataSource = EmailStatus.GetAll(Master.clientconnections);
                rcbEmailStatus.DataBind();

                #endregion

                SortField = "Magazine_Name";
                SortDirection = "ASC";
                guid = System.Guid.NewGuid().ToString();

            }
        }

        private void loadStandardFiltersListboxes()
        {
            lstCountryRegions.DataSource = Country.GetArea();
            lstCountryRegions.DataBind();

            lstCountry.DataSource = Country.GetAll();
            lstCountry.DataBind();

            List<Country> c = Country.GetSelectedCountries();

            //Load country in Subscription details popup
            drp_Country.DataSource = c;
            drp_Country.DataBind();
            drp_Country.Items.Insert(0, new ListItem("Select Country", ""));

            //Load country in Edit Address popup
            drpMasterCountry.DataSource = c;
            drpMasterCountry.DataBind();
            drpMasterCountry.Items.Insert(0, new ListItem("", ""));

            lstState.DataSource = Region.GetAll();
            lstState.DataBind();

            lstGeoCode.DataSource = RegionGroup.GetAll();
            lstGeoCode.DataBind();
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
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

            List<MasterGroup> masterGroupList = new List<MasterGroup>();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
                masterGroupList = MasterGroup.GetActiveByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
            else
                masterGroupList = MasterGroup.GetActiveMasterGroupsSorted(Master.clientconnections);

            dlDimensions.DataSource = masterGroupList;
            dlDimensions.DataBind();
        }

        private string GetLinkTitle(int pubtypeid)
        {
            return PubTypes.GetByID(Master.clientconnections, pubtypeid).PubTypeDisplayName;
        }

        protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetFitlerControls();

            imglogo.ImageUrl = string.Empty;
            imglogo.Visible = false;
            hfBrandID.Value = drpBrand.SelectedValue;

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
                    hfBrandID.Value = drpBrand.SelectedValue;
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
        }

        #region Dimensions Popup

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

                    lnkMarketShowHide.Text = "(Hide...)";
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
                LinkButton lnkPubTypeShowHide = (LinkButton)((Repeater)UpdatePanel1.FindControl("PubTypeRepeater")).Items[int.Parse(e.CommandName)].FindControl("lnkPubTypeShowHide");
                Panel pnlPubTypeBody = (Panel)((Repeater)UpdatePanel1.FindControl("PubTypeRepeater")).Items[int.Parse(e.CommandName)].FindControl("pnlPubTypeBody");

                if (lnkPubTypeShowHide.Text == "(Show...)")
                {

                    ListBox PubTypeListBox = (ListBox)((Repeater)UpdatePanel1.FindControl("PubTypeRepeater")).Items[int.Parse(e.CommandName)].FindControl("PubTypeListBox");
                    if (PubTypeListBox.Items.Count == 0)
                    {
                        int pubTypeID = Convert.ToInt32(args[0]);

                        // get pubs   
                        List<Pubs> lpubs = new List<Pubs>();
                        if (Convert.ToInt32(hfBrandID.Value) > 0)
                            lpubs = Pubs.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                        else
                            lpubs = Pubs.GetSearchEnabled(Master.clientconnections);

                        var pubsQuery = (from p in lpubs
                                         where p.PubTypeID == pubTypeID && p.EnableSearching == true
                                         select new { p.PubID, p.PubName });

                        PubTypeListBox.DataSource = pubsQuery;
                        PubTypeListBox.DataValueField = "PubID";
                        PubTypeListBox.DataTextField = "PubName";
                        PubTypeListBox.DataBind();
                    }

                    lnkPubTypeShowHide.Text = "(Hide...)";
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
                LinkButton lnkDimensionShowHide = (LinkButton)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(e.CommandName)].FindControl("lnkDimensionShowHide");
                Panel pnlDimBody = (Panel)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(e.CommandName)].FindControl("pnlDimBody");

                if (lnkDimensionShowHide.Text == "(Show...)")
                {
                    pnlDimBody.Visible = true;
                    ListBox lstResponse = (ListBox)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(e.CommandName)].FindControl("lstResponse");

                    if (lstResponse.Items.Count == 0)
                    {
                        lstResponse.DataTextField = "MASTERDESC";
                        lstResponse.DataValueField = "MasterID";

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

                    lnkDimensionShowHide.Text = "(Hide...)";
                }
                else
                {
                    lnkDimensionShowHide.Text = "(Show...)";
                    pnlDimBody.Visible = false;
                }
            }
        }

        protected void lnkPopup_Command(object sender, CommandEventArgs args)
        {
            rlbDimensionAvailable.Items.Clear();
            rlbDimensionSelected.Items.Clear();
            rtbDimSearch.Text = string.Empty;

            lblDimensionControl.Text = args.CommandName;

            var listBox = (ListBox)UpdatePanel1.FindControl(args.CommandName);
            var type = string.Empty;

            if (listBox == null)
            {
                type = ProcessNoListBox(args, ref listBox);
            }
            else
            {
                lblDimension.Text = args.CommandArgument.ToString();

                if (TypeMarket.Equals(args.CommandArgument.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    type = ProcessMarkets(args, listBox);
                }
            }

            if (TypeDimension.Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                ProcessTypeDimension(listBox);
            }
            else if (TypeLstMarket.Equals(lblDimensionControl.Text, StringComparison.OrdinalIgnoreCase) ||
                     TypePubType.Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                ProcessTypeLstMarketOrPubType(listBox);
            }
            else
            {
                ProcessTypeOther(listBox);
            }

            if (DimensionCountryRegion.Equals(lblDimension.Text, StringComparison.Ordinal))
            {
                lstCountry.ClearSelection();
                selectCountryOnRegion();
            }

            mdlPopupDimension.Show();
        }

        private string ProcessNoListBox(CommandEventArgs eventArgs, ref ListBox listBox)
        {
            Guard.NotNull(eventArgs, nameof(eventArgs));

            var args = eventArgs.CommandArgument
                .ToString()
                .Split(DelimiterPipeChar);
            lblDimensionType.Text = args[ArgIndexDimensionType].ToUpper();
            var type = args[ArgIndexDimensionType].ToUpper();
            lblDimension.Text = args[ArgIndexDimensionValue];

            var argPubType = args[ArgIndexDimensionType];
            if (TypePubType.Equals(argPubType, StringComparison.OrdinalIgnoreCase))
            {
                listBox = ProcessArgPubType(eventArgs, args);
            }
            else if (TypeDimension.Equals(argPubType, StringComparison.OrdinalIgnoreCase))
            {
                listBox = ProcessArgDimension(eventArgs, args);
            }

            if (TypeDimension.Equals(argPubType, StringComparison.OrdinalIgnoreCase))
            {
                hfMasterValue.Value = args[0];
            }

            return type;
        }

        private void ProcessTypeOther(ListControl listBox)
        {
            Guard.NotNull(listBox, nameof(listBox));

            lnkSortByDescription.Visible = false;
            lnkSortByValue.Visible = false;
            rtbDimSearch.Visible = true;
            rlbDimensionSelected.Visible = false;
            rlbDimensionAvailable.AllowTransfer = false;
            rlbDimensionAvailable.TransferToID = string.Empty;
            rlbDimensionAvailable.Width = Unit.Pixel(DimensionsAvailableWideWidth);
            rlbDimensionAvailable.ButtonSettings.AreaWidth = Unit.Pixel(0);

            foreach (ListItem item in listBox.Items)
            {
                rlbDimensionAvailable.Items.Add(new RadListBoxItem(item.Text, item.Value));
                if (item.Selected)
                {
                    rlbDimensionAvailable.FindItemByValue(item.Value).Selected = true;
                }
            }
        }

        private void ProcessTypeLstMarketOrPubType(ListBox listBox)
        {
            Guard.NotNull(listBox, nameof(listBox));

            lnkSortByDescription.Visible = false;
            lnkSortByValue.Visible = false;
            rtbDimSearch.Visible = true;
            rlbDimensionSelected.Visible = true;
            rlbDimensionAvailable.AllowTransfer = true;
            rlbDimensionAvailable.TransferToID = ControlRblDimensionSelected;
            rlbDimensionAvailable.Width = Unit.Pixel(DimensionsAvailableNarrowWidth);
            rlbDimensionAvailable.ButtonSettings.AreaWidth = Unit.Pixel(ButtonSettingsWidth);

            foreach (ListItem item in listBox.Items)
            {
                if (item.Selected)
                {
                    rlbDimensionSelected.Items.Add(new RadListBoxItem(item.Text, item.Value));
                }
                else
                {
                    rlbDimensionAvailable.Items.Add(new RadListBoxItem(item.Text, item.Value));
                }
            }
        }

        private void ProcessTypeDimension(ListBox listBox)
        {
            Guard.NotNull(listBox, nameof(listBox));

            lnkSortByDescription.Visible = true;
            lnkSortByValue.Visible = true;
            rtbDimSearch.Visible = true;
            rlbDimensionSelected.Visible = true;
            rlbDimensionAvailable.AllowTransfer = true;
            rlbDimensionAvailable.TransferToID = ControlRblDimensionSelected;
            rlbDimensionAvailable.Width = Unit.Pixel(DimensionsAvailableNarrowWidth);
            rlbDimensionAvailable.ButtonSettings.AreaWidth = Unit.Pixel(ButtonSettingsWidth);

            foreach (ListItem item in listBox.Items)
            {
                if (item.Selected)
                {
                    rlbDimensionSelected.Items.Add(new RadListBoxItem(item.Text, item.Value));
                }
                else
                {
                    rlbDimensionAvailable.Items.Add(new RadListBoxItem(item.Text, item.Value));
                }
            }
        }

        private string ProcessMarkets(CommandEventArgs eventArgs, ListBox listBox)
        {
            Guard.NotNull(eventArgs, nameof(eventArgs));
            Guard.NotNull(listBox, nameof(listBox));

            lblDimensionType.Text = eventArgs.CommandArgument.ToString().ToUpper();
            var type = eventArgs.CommandArgument.ToString().ToUpper();

            if (listBox.Items.Count == 0)
            {
                List<Objects.Markets> markets;

                int brandValue;
                int.TryParse(hfBrandID.Value, out brandValue);
                if (brandValue > 0)
                {
                    markets = Objects.Markets.GetByBrandID(Master.clientconnections, brandValue);
                }
                else
                {
                    markets = Objects.Markets.GetNotInBrand(Master.clientconnections);
                }

                listBox.DataSource = markets;
                listBox.DataBind();
            }

            return type;
        }

        private ListBox ProcessArgDimension(CommandEventArgs eventArgs, IReadOnlyList<string> args)
        {
            Guard.NotNull(eventArgs, nameof(eventArgs));

            var listBox = (ListBox)((DataList)UpdatePanel1
                    .FindControl("dlDimensions"))
                    .Items[int.Parse(eventArgs.CommandName)]
                    .FindControl("lstResponse");

            if (listBox.Items.Count == 0)
            {
                listBox.DataTextField = FieldMasterDesc;
                listBox.DataValueField = FieldMasterId;

                List<MasterCodeSheet> masterCodeSheets;
                int brandValue;
                int.TryParse(hfBrandID.Value, out brandValue);
                if (brandValue > 0)
                {
                    masterCodeSheets = MasterCodeSheet.GetSearchEnabledByBrandID(Master.clientconnections, brandValue);
                }
                else
                {
                    masterCodeSheets = MasterCodeSheet.GetSearchEnabled(Master.clientconnections);
                }

                var masterCodeSheetQuery = (from sheet in masterCodeSheets
                    where sheet.MasterGroupID == ToInt32(args[0])
                    orderby sheet.SortOrder ascending
                    select new {sheet.MasterID, masterdesc = $"{sheet.MasterDesc}' ''('{sheet.MasterValue}')'"});

                listBox.DataSource = masterCodeSheetQuery;
                listBox.DataBind();
            }

            return listBox;
        }

        private ListBox ProcessArgPubType(CommandEventArgs eventArgs, IReadOnlyList<string> args)
        {
            Guard.NotNull(eventArgs, nameof(eventArgs));

            var listBox = (ListBox)((Repeater)UpdatePanel1
                    .FindControl("PubTypeRepeater"))
                    .Items[int.Parse(eventArgs.CommandName)]
                    .FindControl("PubTypeListBox");

            if (listBox.Items.Count == 0)
            {
                var pubTypeId = ToInt32(args[0]);

                List<Pubs> list;
                int brandValue;
                int.TryParse(hfBrandID.Value, out brandValue);
                if (brandValue > 0)
                {
                    list = Pubs.GetSearchEnabledByBrandID(Master.clientconnections, brandValue);
                }
                else
                {
                    list = Pubs.GetSearchEnabled(Master.clientconnections);
                }

                var pubsQuery = (from publication in list
                    where publication.PubTypeID == pubTypeId && publication.EnableSearching == true
                    select new {publication.PubID, publication.PubName});

                listBox.DataSource = pubsQuery;
                listBox.DataValueField = FieldPubId;
                listBox.DataTextField = FieldPubName;
                listBox.DataBind();
            }

            return listBox;
        }

        private static int ToInt32(string numberStr)
        {
            int result;
            int.TryParse(numberStr, out result);
            return result;
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

                rlbDimensionAvailable.DataValueField = "MasterID";
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
                    lst = (ListBox)((Repeater)UpdatePanel1.FindControl("PubTypeRepeater")).Items[int.Parse(lblDimensionControl.Text)].FindControl("PubTypeListBox");
                    LinkButton lnkPubTypeShowHide = (LinkButton)((Repeater)UpdatePanel1.FindControl("PubTypeRepeater")).Items[int.Parse(lblDimensionControl.Text)].FindControl("lnkPubTypeShowHide");
                    Panel pnlPubTypeBody = (Panel)((Repeater)UpdatePanel1.FindControl("PubTypeRepeater")).Items[int.Parse(lblDimensionControl.Text)].FindControl("pnlPubTypeBody");

                    lnkPubTypeShowHide.Text = "(Hide...)";
                    pnlPubTypeBody.Visible = true;
                }
                else if (lblDimensionType.Text == "DIMENSION")
                {
                    lst = (ListBox)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(lblDimensionControl.Text)].FindControl("lstResponse");
                    LinkButton lnkDimensionShowHide = (LinkButton)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(lblDimensionControl.Text)].FindControl("lnkDimensionShowHide");
                    Panel pnlDimBody = (Panel)((DataList)UpdatePanel1.FindControl("dlDimensions")).Items[int.Parse(lblDimensionControl.Text)].FindControl("pnlDimBody");

                    lnkDimensionShowHide.Text = "(Hide...)";
                    pnlDimBody.Visible = true;
                }
            }
            else
            {
                if (lblDimensionType.Text == "MARKETS")
                {
                    lnkMarketShowHide.Text = "(Hide...)";
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
                selectCountryOnRegion();
            }

        }

        #endregion

        protected void selectStateOnRegion()
        {
            // Get all the geocodes, see if they are selected, then select the appropriate state if so
            foreach (int i in lstGeoCode.GetSelectedIndices())
            {
                List<Region> lregion = Region.GetByRegionGroupID(Convert.ToInt32(lstGeoCode.Items[i].Value));

                foreach (Region r in lregion)
                {
                    foreach (ListItem li in lstState.Items)
                    {
                        if (r.RegionCode.Equals(li.Value, StringComparison.OrdinalIgnoreCase))
                            li.Selected = true;
                    }
                }
            }
        }

        protected void selectCountryOnRegion()
        {
            // Get all the geocodes, see if they are selected, then select the appropriate state if so
            foreach (int i in lstCountryRegions.GetSelectedIndices())
            {
                List<Country> lCountry = Country.GetByArea(lstCountryRegions.Items[i].Value);

                foreach (Country c in lCountry)
                {
                    foreach (ListItem li in lstCountry.Items)
                    {
                        if (c.CountryID == Convert.ToInt32(li.Value))
                            li.Selected = true;
                    }
                }
            }
        }

        protected void selectPubsForMarkets()
        {
            Repeater pubTypeRepeater = UpdatePanel1.FindControl("PubTypeRepeater") as Repeater;
            foreach (RepeaterItem repeaterItem in pubTypeRepeater.Items)
            {
                ListBox listBox = repeaterItem.FindControl("PubTypeListBox") as ListBox;
                listBox.ClearSelection();
            }

            try
            {
                foreach (DataListItem di in dlDimensions.Items)
                {
                    ListBox lstResponse = (ListBox)di.FindControl("lstResponse");
                    if (lstResponse != null)
                        lstResponse.ClearSelection();
                }

                // Get all the geocodes, see if they are selected, then select the appropriate state if so
                foreach (ListItem mylistvalue in lstMarket.Items)
                {
                    #region Preselect pubs and Dimensions from Market
                    if (mylistvalue.Selected)
                    {
                        KMPS.MD.Objects.Markets markets = KMPS.MD.Objects.Markets.GetByID(Master.clientconnections, Convert.ToInt32(mylistvalue.Value));

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(markets.MarketXML);

                        int pubTypeID = 0;

                        // get pubs    
                        List<Pubs> lpubs = new List<Pubs>();
                        if (Convert.ToInt32(hfBrandID.Value) > 0)
                            lpubs = Pubs.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                        else
                            lpubs = Pubs.GetSearchEnabled(Master.clientconnections);

                        Dictionary<int, string> dSelected = new Dictionary<int, string>();

                        //Pubs
                        XmlNode node = doc.SelectSingleNode("//Market/MarketType[@ID ='P']");

                        if (node != null)
                        {
                            foreach (XmlNode child in node.ChildNodes)
                            {
                                try
                                {
                                    pubTypeID = lpubs.Find(x => x.PubID == Convert.ToInt32(child.Attributes["ID"].Value)).PubTypeID;

                                    if (dSelected.ContainsKey(pubTypeID))
                                        dSelected[pubTypeID] = dSelected[pubTypeID] + "," + child.Attributes["ID"].Value;
                                    else
                                        dSelected.Add(pubTypeID, child.Attributes["ID"].Value);
                                }
                                catch
                                {
                                }
                            }
                        }

                        foreach (RepeaterItem repeaterItem in pubTypeRepeater.Items)
                        {
                            HiddenField hfPubTypeID = (HiddenField)repeaterItem.FindControl("hfPubTypeID");

                            if (dSelected.ContainsKey(int.Parse(hfPubTypeID.Value)))
                            {
                                ListBox PubTypeListBox = repeaterItem.FindControl("PubTypeListBox") as ListBox;

                                LinkButton lnkPubTypeShowHide = (LinkButton)repeaterItem.FindControl("lnkPubTypeShowHide");
                                Panel pnlPubTypeBody = (Panel)repeaterItem.FindControl("pnlPubTypeBody");

                                if (PubTypeListBox.Items.Count == 0)
                                {
                                    var pubsbyPubTypeID = (from p in lpubs
                                                            where p.PubTypeID == Convert.ToInt32(hfPubTypeID.Value) && p.EnableSearching == true
                                                            select new { p.PubID, p.PubName });

                                    PubTypeListBox.DataSource = pubsbyPubTypeID;
                                    PubTypeListBox.DataValueField = "PubID";
                                    PubTypeListBox.DataTextField = "PubName";
                                    PubTypeListBox.DataBind();
                                }

                                lnkPubTypeShowHide.Text = "(Hide...)";
                                pnlPubTypeBody.Visible = true;

                                Utilities.SelectFilterListBoxes(PubTypeListBox, dSelected[int.Parse(hfPubTypeID.Value)]);
                            }
                        }

                        //dimension

                        List<MasterGroup> masterGroup = new List<MasterGroup>();

                        if (Convert.ToInt32(hfBrandID.Value) > 0)
                            masterGroup = MasterGroup.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                        else
                            masterGroup = MasterGroup.GetSearchEnabled(Master.clientconnections);

                        List<MasterCodeSheet> mastercodesheet = new List<MasterCodeSheet>();

                        if (Convert.ToInt32(hfBrandID.Value) > 0)
                            mastercodesheet = MasterCodeSheet.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                        else
                            mastercodesheet = MasterCodeSheet.GetSearchEnabled(Master.clientconnections);

                        foreach (DataListItem di in dlDimensions.Items)
                        {
                            HiddenField hfMasterGroup = (HiddenField)di.FindControl("hfMasterGroup");
                            ListBox lstResponse = (ListBox)di.FindControl("lstResponse");

                            MasterGroup mg = masterGroup.SingleOrDefault(m => m.MasterGroupID == Convert.ToInt32(hfMasterGroup.Value));
                            if (mg != null)
                            {
                                XmlNode dnode = doc.SelectSingleNode("//Market/MarketType[@ID ='D']/Group[@ID = '" + mg.ColumnReference.ToString() + "']");
                                if (dnode != null)
                                {
                                    LinkButton lnkDimensionShowHide = (LinkButton)di.FindControl("lnkDimensionShowHide");
                                    Panel pnlDimBody = (Panel)di.FindControl("pnlDimBody");

                                        pnlDimBody.Visible = true;

                                        if (lstResponse.Items.Count == 0)
                                        {
                                            lstResponse.DataTextField = "MASTERDESC";
                                            lstResponse.DataValueField = "MasterID";

                                            var MasterCodeSheetQuery = (from m in mastercodesheet
                                                                        where m.MasterGroupID == Convert.ToInt32(hfMasterGroup.Value)
                                                                        orderby m.SortOrder ascending
                                                                        select new { m.MasterID, masterdesc = m.MasterDesc + ' ' + '(' + m.MasterValue + ')' });

                                            lstResponse.DataSource = MasterCodeSheetQuery;
                                            lstResponse.DataBind();
                                        }

                                        lnkDimensionShowHide.Text = "(Hide...)";

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

                                    Utilities.SelectFilterListBoxes(lstResponse, selectedValues);
                                }
                            }
                        }

                        //Adhoc
                        XmlNode nodeAdhoc = doc.SelectSingleNode("//Market/FilterType[@ID ='A']");

                        if (nodeAdhoc != null)
                        {
                            foreach (XmlNode nodeEntry in nodeAdhoc.ChildNodes)
                            {
                                GridView gvCategory = (GridView)AdhocFilter.FindControl("gvCategory");

                                foreach (GridViewRow gv in gvCategory.Rows)
                                {
                                    DataList dl = (DataList)gv.FindControl("dlAdhocFilter");

                                    if (dl != null)
                                    {
                                        foreach (DataListItem di in dl.Items)
                                        {
                                            Label lbAdhocColumnValue = (Label)di.FindControl("lbAdhocColumnValue");

                                            DropDownList drpAdhocInt = (DropDownList)di.FindControl("drpAdhocInt");
                                            TextBox txtAdhocIntFrom = (TextBox)di.FindControl("txtAdhocIntFrom");
                                            TextBox txtAdhocIntTo = (TextBox)di.FindControl("txtAdhocIntTo");

                                            if (lbAdhocColumnValue.Text == nodeEntry.Attributes["ID"].Value)
                                            {
                                                string[] strValue = nodeEntry.ChildNodes[0].Attributes["ID"].Value.Split('|');
                                                txtAdhocIntFrom.Text = strValue[0];
                                                txtAdhocIntTo.Text = strValue[1];

                                                if (nodeEntry.ChildNodes[1].Attributes["ID"].Value.ToUpper() == "EQUAL" || nodeEntry.ChildNodes[1].Attributes["ID"].Value.ToUpper() == "GREATER" || nodeEntry.ChildNodes[1].Attributes["ID"].Value.ToUpper() == "LESSER")
                                                    txtAdhocIntTo.Enabled = false;
                                                else
                                                    txtAdhocIntTo.Enabled = true;

                                                drpAdhocInt.SelectedIndex = -1;

                                                if (drpAdhocInt.Items.FindByValue(nodeEntry.ChildNodes[1].Attributes["ID"].Value) != null)
                                                {
                                                    drpAdhocInt.Items.FindByValue(nodeEntry.ChildNodes[1].Attributes["ID"].Value).Selected = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        protected void MoveListItems(ListItem li, ListBox listbox, int i)
        {
            li.Selected = true;
            //remove item
            listbox.Items.Remove(li);
            //insert it in top
            listbox.Items.Insert(i, li);
        }

        protected void lstMarket_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectPubsForMarkets();
        }

        protected void lstGeoCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstState.ClearSelection();
            selectStateOnRegion();
        }

        protected void lstCountryRegions_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstCountry.ClearSelection();
            selectCountryOnRegion();
        }

        protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadMtxtboxZipCode.Text = "";
            if (drpCountry.SelectedValue.Equals("UNITED STATES", StringComparison.OrdinalIgnoreCase))
                RadMtxtboxZipCode.Mask = "#####";
            else
                RadMtxtboxZipCode.Mask = "L#L #L#";
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

        #region RESET

        protected void btnResetAll_Click(object sender, EventArgs e)
        {
            if (pnlBrand.Visible)
                drpBrand.Enabled = true;

            ResetFitlerControls();
        }

        private void ResetFitlerControls()
        {
            FilterCollection.Clear();
            lstCountryRegions.ClearSelection();
            lstGeoCode.ClearSelection();
            lstState.ClearSelection();
            lstCountry.ClearSelection();
            rcbEmail.ClearCheckedItems();
            rcbPhone.ClearCheckedItems();
            rcbFax.ClearCheckedItems();
            rcbIsLatLonValid.ClearCheckedItems();
            rcbMailPermission.ClearCheckedItems();
            rcbFaxPermission.ClearCheckedItems();
            rcbPhonePermission.ClearCheckedItems();
            rcbOtherProductsPermission.ClearCheckedItems();
            rcbThirdPartyPermission.ClearCheckedItems();
            rcbEmailRenewPermission.ClearCheckedItems();
            rcbTextPermission.ClearCheckedItems();
            rcbMedia.ClearCheckedItems();
            rcbEmailStatus.ClearCheckedItems();

            Repeater pubTypeRepeater = UpdatePanel1.FindControl("PubTypeRepeater") as Repeater;
            foreach (RepeaterItem repeaterItem in pubTypeRepeater.Items)
            {
                ListBox listBox = repeaterItem.FindControl("PubTypeListBox") as ListBox;
                listBox.ClearSelection();
            }

            lstMarket.ClearSelection();

            dvResults.Style.Add("Display", "none");
            txtLastName.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtCompany.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;

            foreach (DataListItem di in dlDimensions.Items)
            {
                ListBox lstResponse = (ListBox)di.FindControl("lstResponse");
                if (lstResponse != null)
                    lstResponse.ClearSelection();
            }

            txtRadiusMax.Text = string.Empty;
            txtRadiusMin.Text = string.Empty;
            drpCountry.ClearSelection();
            RadMtxtboxZipCode.Mask = "#####";
            RadMtxtboxZipCode.Text = "";

            // Activity popup

            ActivityFilter.Reset();

            // Adhoc popup

            AdhocFilter.Reset();

            //Circulation Popup

            CirculationFilter.Reset();
        }

        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FilterCollection.Clear();
            rgSubscriberList.DataSource = null;
            rgSubscriberList.DataBind();
            dvResults.Style.Add("Display", "none");

            try
            {
                Cache.Remove(guid);
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }

            if (pnlBrand.Visible)
            {
                if (Convert.ToInt32(hfBrandID.Value) < 0)
                {
                    DisplayError("Please select Brand");
                    return;
                }
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
                    DisplayError("Please select at least one product or change search to 'Search All' in  Open Criteria");
                    return;
                }
            }

            if (drpClickActivity.SelectedValue != string.Empty)
            {
                if (string.Equals("Search Selected Products", rblClickSearchType.SelectedValue, StringComparison.OrdinalIgnoreCase) && Convert.ToInt32(drpClickActivity.SelectedValue) >= 0 && PubIDs == string.Empty)
                {
                    DisplayError("Please select at least one product or change search to 'Search All' in Click Criteria");
                    return;
                }
            }

            Filter filter = getFilter();
            filter.FilterNo = 1;
            fc.Add(filter);
            FilterCollection = fc;

            if (txtFirstName.Text.Trim() != string.Empty || txtLastName.Text.Trim() != string.Empty || txtCompany.Text.Trim() != string.Empty || txtPhone.Text.Trim() != string.Empty || txtEmail.Text.Trim() != string.Empty || filter.Fields.Count > 0)
            {
                List<Subscriber> ls = LoadGrid();

                rgSubscriberList.DataSource = ls;
                rgSubscriberList.DataBind();

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecordDetails, KMPlatform.Enums.Access.Edit))
                    rgSubscriberList.Columns[9].Visible = false;

                if (ls.Count > 0)
                    dvResults.Style.Add("Display", "inline");
                else
                    dvResults.Style.Add("Display", "none");
            }
            else
                DisplayError("Please enter values in one of above fields");
        }

        private string getListboxValues(ListBox lst)
        {
            //return Request[formName];

            string selectedvalues = string.Empty;
            foreach (ListItem item in lst.Items)
            {
                if (item.Selected)
                    selectedvalues += selectedvalues == string.Empty ? item.Value : "," + item.Value;
            }
            return selectedvalues;

        }

        private string getPubsValues()
        {
            string selectedvalues = string.Empty;

            Repeater pubTypeRepeater = UpdatePanel1.FindControl("PubTypeRepeater") as Repeater;

            foreach (RepeaterItem repeaterItem in pubTypeRepeater.Items)
            {
                ListBox pubTypeListBox = repeaterItem.FindControl("PubTypeListBox") as ListBox;
                string tempString = Utilities.getListboxSelectedValues(pubTypeListBox);

                if (!string.IsNullOrEmpty(tempString))
                    selectedvalues += (selectedvalues == string.Empty ? tempString : "," + tempString);
            }

            return selectedvalues;
        }

        private string getListboxText(ListBox lst)
        {
            string text = string.Empty;

            string selectedvalues = string.Empty;
            foreach (ListItem item in lst.Items)
            {
                if (item.Selected)
                {
                    text = item.Text;

                    if (text.IndexOf(".") > -1)
                        text = text.Substring(0, text.IndexOf("."));

                    selectedvalues += selectedvalues == string.Empty ? text : "," + text;
                }
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
                ListBox pubTypeListBox = repeaterItem.FindControl("PubTypeListBox") as ListBox;

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

        private string ClearXML(string val)
        {
            val = val.Replace("'", "");
            val = val.Replace("&", "&amp;");
            val = val.Replace("\"", "");

            return val;
        }

        private Filter getFilter()
        {
            var filter = new Filter { ViewType = Enums.ViewType.RecordDetails };
            var pubsValue = getPubsValues();

            if (pubsValue != string.Empty)
            {
                var pubsListBoxText = getPubsListboxText();
                filter.Fields.Add(new Field("Product", pubsValue, pubsListBoxText, string.Empty, Enums.FiltersType.Product, "Product"));
            }

            filter.Fields.AddRange(GetDimensionFields());
            filter.Fields.AddRange(GetStandardFields(filter));
            LoadGeoFields(filter);
            filter.Fields.AddRange(AdhocFilter.GetAdhocFilters());
            filter.Fields.AddRange(ActivityFilter.GetActivityFilters());
            filter.Fields.AddRange(CirculationFilter.GetCirculationFilters());

            //add lastname, fname, ....etc
            var lastNameText = txtLastName.Text;
            if (lastNameText != string.Empty)
            {
                filter.Fields.Add(new Field("Last Name", lastNameText, lastNameText, string.Empty, Enums.FiltersType.Standard, "LNAME"));
            }

            var firstNameText = txtFirstName.Text;
            if (firstNameText != string.Empty)
            {
                filter.Fields.Add(new Field("First Name", firstNameText, firstNameText, string.Empty, Enums.FiltersType.Standard, "FNAME"));
            }

            var companyText = txtCompany.Text;
            if (companyText != string.Empty)
            {
                filter.Fields.Add(new Field("Company", companyText, companyText, string.Empty, Enums.FiltersType.Standard, "COMPANY"));
            }

            var phoneText = txtPhone.Text;
            if (phoneText != string.Empty)
            {
                filter.Fields.Add(new Field("Phoneno", phoneText, phoneText, string.Empty, Enums.FiltersType.Standard, "PHONE"));
            }

            var emailText = txtEmail.Text;
            if (emailText != string.Empty)
            {
                filter.Fields.Add(new Field("EmailID", emailText, emailText, string.Empty, Enums.FiltersType.Standard, "EMAIL"));
            }

            return filter;
        }

        private IEnumerable<Field> GetDimensionFields()
        {
            var list = new List<Field>();
            int brandId;

            int.TryParse(hfBrandID.Value, out brandId);
            var masterGroups = brandId > 0
                ? MasterGroup.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value))
                : MasterGroup.GetSearchEnabled(Master.clientconnections);

            foreach (DataListItem dimensionItem in dlDimensions.Items)
            {
                var lblResponseGroup = (Label)dimensionItem.FindControl("lblResponseGroup");
                var lstResponse = dimensionItem.FindControl("lstResponse") as ListBox;
                if (lstResponse != null)
                {
                    var selectedValues = getListboxValues(lstResponse);

                    if (selectedValues.Length > 0)
                    {
                        foreach (var masterGroup in masterGroups)
                        {
                            var responseGroup = lblResponseGroup.Text;
                            if (responseGroup.EqualsIgnoreCase(masterGroup.DisplayName))
                            {
                                var listBoxText = Utilities.getListboxText(lstResponse);
                                list.Add(new Field(
                                    responseGroup,
                                    selectedValues,
                                    listBoxText,
                                    string.Empty,
                                    Enums.FiltersType.Dimension,
                                    masterGroup.ColumnReference));
                                break;
                            }
                        }
                    }
                }
            }

            return list;
        }

        private IEnumerable<Field> GetStandardFields(Filter filter)
        {
            Guard.NotNull(filter, nameof(filter));
            var list = new List<Field>();
            string listValues;
            int brandId;

            if (pnlBrand.Visible)
            {
                int.TryParse(hfBrandID.Value, out brandId);

                if (brandId > 0)
                {
                    if (drpBrand.Visible)
                    {
                        var selectedItem = drpBrand.SelectedItem;
                        int.TryParse(selectedItem.Value, out brandId);
                        filter.BrandID = brandId;
                        list.Add(new Field("Brand", selectedItem.Value, selectedItem.Text, string.Empty, Enums.FiltersType.Brand, "BRAND"));
                    }
                    else
                    {
                        var idText = hfBrandID.Value;
                        int.TryParse(idText, out brandId);
                        filter.BrandID = brandId;
                        list.Add(new Field("Brand", idText, lblBrandName.Text, string.Empty, Enums.FiltersType.Brand, "BRAND"));
                    }
                }
            }

            listValues = getListboxValues(lstGeoCode);
            if (listValues != string.Empty)
            {
                selectStateOnRegion();
            }

            listValues = getListboxValues(lstState);
            if (listValues != string.Empty)
            {
                var listBoxText = getListboxText(lstState);
                filter.Fields.Add(new Field("State", listValues, listBoxText, string.Empty, Enums.FiltersType.Standard, "STATE"));
            }

            listValues = getListboxValues(lstCountry);
            if (listValues != string.Empty)
            {
                var listBoxText = getListboxText(lstCountry);
                filter.Fields.Add(new Field("Country", listValues, listBoxText, string.Empty, Enums.FiltersType.Standard, "COUNTRY"));
            }

            LoadStandardFields(rcbEmail, list, "Email", "EMAIL");
            LoadStandardFields(rcbPhone, list, "Phone", "PHONE");
            LoadStandardFields(rcbFax, list, "Fax", "FAX");
            LoadStandardFields(rcbMedia, list, "Media", "MEDIA");
            LoadStandardFields(rcbIsLatLonValid, list, "GeoLocated", "ISLATLONVALID");
            LoadStandardFields(rcbMailPermission, list, "MailPermission", "MAILPERMISSION");
            LoadStandardFields(rcbFaxPermission, list, "FaxPermission", "FAXPERMISSION");
            LoadStandardFields(rcbPhonePermission, list, "PhonePermission", "PHONEPERMISSION");
            LoadStandardFields(rcbOtherProductsPermission, list, "OtherProductsPermission", "OTHERPRODUCTSPERMISSION");
            LoadStandardFields(rcbThirdPartyPermission, list, "ThirdPartyPermission", "THIRDPARTYPERMISSION");
            LoadStandardFields(rcbEmailRenewPermission, list, "EmailRenewPermission", "EMAILRENEWPERMISSION");
            LoadStandardFields(rcbTextPermission, list, "TextPermission", "TEXTPERMISSION");
            LoadStandardFields(rcbEmailStatus, list, "Email Status", "EMAILSTATUS");

            return list;
        }

        private void LoadGeoFields(Filter filter)
        {
            Guard.NotNull(filter, nameof(filter));

            if (RadMtxtboxZipCode.TextWithLiterals != string.Empty)
            {
                int radiusMin;
                int radiusMax;
                int.TryParse(txtRadiusMin.Text, out radiusMin);
                int.TryParse(txtRadiusMax.Text, out radiusMax);

                if (txtRadiusMin.Text != string.Empty && txtRadiusMax.Text != string.Empty && radiusMin < radiusMax)
                {
                    double locationLat;
                    double locationLon;

                    var values = FilterMVC.CalculateZipCodeRadius(
                        radiusMin,
                        radiusMax,
                        RadMtxtboxZipCode.TextWithLiterals,
                        drpCountry.SelectedValue,
                        out locationLat,
                        out locationLon);
                    var valuesText = string.Join("|", values);

                    filter.Fields.Add(new Field(
                        "Zipcode-Radius",
                        valuesText,
                        $"{RadMtxtboxZipCode.TextWithLiterals} & {txtRadiusMin.Text} miles - {txtRadiusMax.Text} miles",
                        $"{RadMtxtboxZipCode.TextWithLiterals}|{txtRadiusMin.Text}|{txtRadiusMax.Text}",
                        Enums.FiltersType.Geo,
                        "ZIPCODE-RADIUS"));
                }
            }
        }

        private static void LoadStandardFields(RadComboBox comboBox, IList<Field> fields, string name, string fieldGroup)
        {
            Guard.NotNull(comboBox, nameof(comboBox));
            Guard.NotNull(fields, nameof(fields));
            if (comboBox.CheckedItems.Count > 0)
            {
                var selectedItem = Utilities.getRadComboBoxSelectedExportFields(comboBox);

                fields.Add(new Field(name, selectedItem.Item1, selectedItem.Item2, string.Empty, Enums.FiltersType.Standard, fieldGroup));
            }
        }

        private List<Subscriber> LoadGrid()
        {
            List<Subscriber> subscriberdetails = null;

            try
            {
                subscriberdetails = (List<Subscriber>)Cache[guid];
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }

            if (subscriberdetails == null)
            {
                try
                {
                    if (FilterCollection.Count > 0)
                    {
                        List<string> Selected_FilterNo = new List<string>();
                        foreach (Filter f in FilterCollection)
                        {
                            Selected_FilterNo.Add(f.FilterNo.ToString());
                        }

                        StringBuilder Queries = Filter.generateCombinationQuery(fc, "UNION", "", string.Join(",", Selected_FilterNo), "", string.Empty, 0, Convert.ToInt32(hfBrandID.Value), Master.clientconnections);

                        List<string> standardColumnList = new List<string>();
                        standardColumnList.Add("s.SubscriptionID|Default");
                        standardColumnList.Add("s.email|Default");
                        standardColumnList.Add("s.fname|Default");
                        standardColumnList.Add("s.lname|Default");
                        standardColumnList.Add("s.company|Default");
                        standardColumnList.Add("s.phone|Default");
                        standardColumnList.Add("s.Title|Default");
                        standardColumnList.Add("s.email|Default");

                        if (Convert.ToInt32(hfBrandID.Value) > 0)
                            standardColumnList.Add("bs.score|Default");
                        else
                            standardColumnList.Add("s.score|Default");

                        DataTable dtSubscription = Subscriber.GetSubscriberData(Master.clientconnections, Queries, standardColumnList, new List<string>(), new List<string>(), new List<string>(), new List<string>(), Convert.ToInt32(hfBrandID.Value), new List<int>(), false, 0);

                        var myEnumerable = dtSubscription.AsEnumerable();

                        subscriberdetails =
                            (from item in myEnumerable
                             select new Subscriber
                             {
                                 LName = item.Field<string>("lname"),
                                 FName = item.Field<string>("fname"),
                                 Company = item.Field<string>("company"),
                                 Email = item.Field<string>("email"),
                                 Phone = item.Field<string>("phone"),
                                 SubscriptionID = item.Field<int>("SubscriptionID"),
                                 Title = item.Field<string>("Title"),
                                 Score = item.Field<int?>("Score") ?? 0
                             }).ToList();

                        Cache.Add(guid, subscriberdetails, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(12, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
                    }
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }
            }

            if (subscriberdetails != null && subscriberdetails.Count > 0)
            {
                subscriberdetails = (List<Subscriber>)ProfileFieldMask.MaskData(Master.clientconnections, subscriberdetails, Master.UserSession.CurrentUser);
            }
            else
            {
                subscriberdetails = new List<Subscriber>();
            }

            return subscriberdetails;
        }

        protected void LoadSubDetails()
        {
            ResetPopupControls();

            try
            {
                var subscriber = Subscriber.Get(Master.clientconnections, IntTryParse(hfSubscriptionID.Value), IntTryParse(hfBrandID.Value))
                    .FirstOrDefault();

                if (subscriber == null)
                {
                    throw new InvalidOperationException($"subscriber is null");
                }

                SetTextValues(subscriber);

                var subscriberDimensionList = SubscriberDimension.GetSubscriberDimension(Master.clientconnections, IntTryParse(hfSubscriptionID.Value), IntTryParse(hfBrandID.Value));
                var demo = new Dictionary<string, string>();

                foreach (var subscriberDimension in subscriberDimensionList)
                {
                    if (!demo.ContainsKey(subscriberDimension.DisplayName.ToUpper()))
                    {
                        demo.Add(subscriberDimension.DisplayName.ToUpper(), $"{subscriberDimension.MasterDesc} ({subscriberDimension.MasterValue})");
                    }
                    else
                    {
                        demo[subscriberDimension.DisplayName.ToUpper()] = $"{demo[subscriberDimension.DisplayName.ToUpper()]}, {subscriberDimension.MasterDesc} ({subscriberDimension.MasterValue})";
                    }
                }

                if (demo.Count > 0)
                {
                    dlDetails.DataSource = demo;
                    dlDetails.DataBind();
                    dlDetails.Visible = true;
                }
                else
                {
                    dlDetails.Visible = false;
                }

                LoadSubscriberDetailsAdhocPubsAcivities();
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void LoadSubscriberDetailsAdhocPubsAcivities()
        {
            var subscriberAdhocList = SubscriberAdhoc.Get(Master.clientconnections, IntTryParse(hfSubscriptionID.Value));
            var adhoc = new Dictionary<string, string>();

            foreach (var subscriberAdhoc in subscriberAdhocList)
            {
                if (!string.IsNullOrWhiteSpace(subscriberAdhoc.AdhocValue))
                {
                    adhoc.Add(subscriberAdhoc.AdhocField, subscriberAdhoc.AdhocValue);
                }
            }

            if (adhoc.Count > 0)
            {
                lblAdhocDetails.Visible = true;
                dlAdhocDetails.DataSource = adhoc;
                dlAdhocDetails.DataBind();
                dlAdhocDetails.Visible = true;
            }
            else
            {
                lblAdhocDetails.Visible = false;
                dlAdhocDetails.Visible = false;
            }

            grdProduct.DataSource = SubscriberPubs.GetSubscriberPubs(Master.clientconnections, IntTryParse(hfSubscriptionID.Value), IntTryParse(hfBrandID.Value));
            grdProduct.DataBind();

            var subscriberActivities = SubscriberActivity.Get(Master.clientconnections, IntTryParse(hfSubscriptionID.Value), IntTryParse(hfBrandID.Value));

            gvActivityHistory.DataSource = subscriberActivities;
            gvActivityHistory.DataBind();

            Func<string, int, string> activitiesHistory = (activityName, numberOfDays) =>
                subscriberActivities.Count(activity => activity.Activity == activityName && activity.ActivityDate >= DateTime.Now.AddDays(-numberOfDays) && activity.ActivityDate <= DateTime.Now)
                + $" last {numberOfDays} days";

            lbl_Opens.Text = activitiesHistory("open", 60);
            lbl_Clicks.Text = activitiesHistory("click", 60);

            gvWebSiteTracking.DataSource = SubscriberVisitActivity.Get(Master.clientconnections, IntTryParse(hfSubscriptionID.Value));
            gvWebSiteTracking.DataBind();
        }

        private void SetTextValues(Subscriber subscriber)
        {
            txt_FirstName.Text = subscriber.FName;
            txt_LastName.Text = subscriber.LName;
            txt_Company.Text = subscriber.Company;
            txt_Address.Text = subscriber.Address;
            txt_Address2.Text = subscriber.MailStop;
            txt_Address3.Text = subscriber.Address3;
            txt_City.Text = subscriber.City;

            drp_Country.SelectedValue = subscriber.CountryID.ToString();

            try
            {
                loadDrp_State();

                if (pldrp_State.Visible)
                {
                    drp_State.SelectedValue = subscriber.State;

                    if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                    {
                        var udm = UserDataMask.GetByUserID(Master.clientconnections, Master.UserSession.UserID);
                        var hasState = udm.Any(u => u.MaskField.EqualsIgnoreCase("STATE"));

                        if (hasState)
                        {
                            var i = drp_State.SelectedIndex;
                            drp_State.Items[i].Text = BlackCircle;
                            drp_State.Enabled = false;
                        }
                    }
                }
                else
                {
                    txt_State.Text = subscriber.State;
                }
            }
            catch (Exception ex)
            {
                DiagnosticsTrace.TraceError(ex.ToString());
            }

            Func<bool?, string> determineSelectedValue = (permission) => permission == true
                                                                             ? One
                                                                             : permission == false
                                                                                 ? Zero
                                                                                 : MinusOne;

            txt_Zip.Text = subscriber.Zip;
            txt_Title.Text = subscriber.Title;
            txt_ForZip.Text = subscriber.ForZip;
            txt_Plus4.Text = subscriber.Plus4;
            txt_Phone.Text = subscriber.Phone;
            txt_Fax.Text = subscriber.Fax;
            txt_Mobile.Text = subscriber.Mobile;
            txt_Email.Text = subscriber.Email;
            txt_Notes.Text = subscriber.Notes;
            lbl_QDate.Text = subscriber.QDate.ToString(MonthDateYearFormat);
            lbl_TDate.Text = subscriber.TransactionDate.ToString(MonthDateYearFormat);
            lbl_DateCreated.Text = subscriber.DateCreated?.ToString(MonthDateYearFormat) ?? string.Empty;
            lbl_DateUpdated.Text = subscriber.DateUpdated?.ToString(MonthDateYearFormat) ?? string.Empty;
            lbl_Score.Text = subscriber.Score.ToString();
            drp_MailPermission.SelectedValue = determineSelectedValue(subscriber.MailPermission);
            drp_FaxPermission.SelectedValue = determineSelectedValue(subscriber.FaxPermission);
            drp_PhonePermission.SelectedValue = determineSelectedValue(subscriber.PhonePermission);
            drp_OtherProductsPermission.SelectedValue = determineSelectedValue(subscriber.OtherProductsPermission);
            drp_ThirdPartyPermission.SelectedValue = determineSelectedValue(subscriber.ThirdPartyPermission);
            drp_EmailRenewPermission.SelectedValue = determineSelectedValue(subscriber.EmailRenewPermission);
            drp_TextPermission.SelectedValue = determineSelectedValue(subscriber.TextPermission);
            lbl_IGrp_No.Text = subscriber.IGrp_No.ToString();
        }

        protected void rgSubscriberList_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            rgSubscriberList.DataSource = LoadGrid();

            if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecordDetails, KMPlatform.Enums.Access.Edit))
            {
                rgSubscriberList.Columns[9].Visible = false;
            }
        }

        protected void lnkShow_Command(object sender, CommandEventArgs e)
        {
            GridDataItem item = (sender as LinkButton).NamingContainer as GridDataItem;
            lblRowIndex.Text = item.ItemIndex.ToString();
            hfSubscriptionID.Value = item.GetDataKeyValue("SubscriptionID").ToString();
            int index = Convert.ToInt32(item.RowIndex.ToString());

            List<Subscriber> subscriberdetails = null;

            try
            {
                subscriberdetails = (List<Subscriber>)Cache[guid];
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }

            int pageIndex = rgSubscriberList.CurrentPageIndex;

            lnkPrevious.Visible = ((pageIndex * rgSubscriberList.PageSize + (index + 1)) <= 1 ? false : true);
            lnkNext.Visible = ((pageIndex * rgSubscriberList.PageSize + (index + 1)) > subscriberdetails.Count - 1 ? false : true);

            LoadSubDetails();
            this.mdlPopupEditSubscriber.Show();
        }

        protected void lnkEditAddress_Command(object sender, CommandEventArgs e)
        {
            GridDataItem item = (sender as LinkButton).NamingContainer as GridDataItem;
            lblRowIndex.Text = item.ItemIndex.ToString();
            hfSubID.Value = item.GetDataKeyValue("SubscriptionID").ToString();

            LoadAddressDetails();
            this.mdlPopupUpdateAddress.Show();
        }

        protected void lnkPrevious_click(Object sender, EventArgs e)
        {
            List<Subscriber> subscriberdetails = null;

            try
            {
                subscriberdetails = (List<Subscriber>)Cache[guid];
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }

            int index = Convert.ToInt32(lblRowIndex.Text) - 1;
            int pageIndex = rgSubscriberList.CurrentPageIndex;
            lnkNext.Visible = ((pageIndex * rgSubscriberList.PageSize + (index + 1)) > subscriberdetails.Count - 1 ? false : true);

            if (pageIndex * rgSubscriberList.PageSize + (index + 1) <= 1)
                lnkPrevious.Visible = false;
            else
            {
                lnkPrevious.Visible = true;

                if (index == -1)
                {
                    rgSubscriberList.CurrentPageIndex = pageIndex - 1;
                    LoadGrid();
                    index = rgSubscriberList.PageSize - 1;
                }
            }
            lblRowIndex.Text = index.ToString();

            GridDataItem item = (GridDataItem)rgSubscriberList.MasterTableView.Items[index] as GridDataItem;
            hfSubscriptionID.Value = item.GetDataKeyValue("SubscriptionID").ToString();

            LoadSubDetails();

            this.mdlPopupEditSubscriber.Show();
        }

        protected void lnkNext_click(Object sender, EventArgs e)
        {
            List<Subscriber> subscriberdetails = null;

            try
            {
                subscriberdetails = (List<Subscriber>)Cache[guid];
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }

            int index = Convert.ToInt32(lblRowIndex.Text) + 1;
            int pageIndex = rgSubscriberList.CurrentPageIndex;

            lnkPrevious.Visible = (index == 0 ? false : true);

            if (pageIndex * rgSubscriberList.PageSize + (index) >= subscriberdetails.Count - 1)
                lnkNext.Visible = false;
            else
            {
                lnkNext.Visible = true;
                if (index == rgSubscriberList.PageSize)
                {
                    rgSubscriberList.CurrentPageIndex = pageIndex + 1;
                    LoadGrid();
                    index = 0;
                }
            }

            lblRowIndex.Text = index.ToString();
            GridDataItem item = (GridDataItem)rgSubscriberList.MasterTableView.Items[index] as GridDataItem;
            hfSubscriptionID.Value = item.GetDataKeyValue("SubscriptionID").ToString();
            LoadSubDetails();

            this.mdlPopupEditSubscriber.Show();
        }

        #region Subscription Details

        protected void drp_Country_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadDrp_State();
            mdlPopupEditSubscriber.Show();
        }

        protected void loadDrp_State()
        {
            if (drp_Country.SelectedValue == One || drp_Country.SelectedValue == Two || drp_Country.SelectedValue == FourHundredTwentyNine)
            {
                pldrp_State.Visible = true;
                pltxt_State.Visible = false;
                drp_State.DataSource = Region.GetByCountryID(Convert.ToInt32(drp_Country.SelectedValue));
                drp_State.DataBind();
                drp_State.Items.Insert(0, new ListItem("Select State", ""));
            }
            else
            {
                pldrp_State.Visible = false;
                pltxt_State.Visible = true;
            }
        }

        protected void btnSave_click(Object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecordDetails, KMPlatform.Enums.Access.Edit))
            {
                try
                {
                    foreach (GridViewRow row in grdProduct.Rows)
                    {
                        DropDownList drpPubEmailStatus = (DropDownList)grdProduct.Rows[row.RowIndex].FindControl("drpPubEmailStatus");
                        TextBox txtPubEmail = (TextBox)grdProduct.Rows[row.RowIndex].FindControl("txtPubEmail");

                        if (txtPubEmail.Text == string.Empty)
                        {
                            if (drpPubEmailStatus.SelectedItem.Value != string.Empty)
                            {
                                dvMessage.Visible = true;
                                lblMessage.Text = "Please select blank Email Status when Email is empty.";
                                mdlPopupEditSubscriber.Show();
                                return;
                            }
                        }
                        else
                        {
                            if (drpPubEmailStatus.SelectedItem.Value == string.Empty)
                            {
                                dvMessage.Visible = true;
                                lblMessage.Text = "Email Status cannot be blank. Please select a value.";
                                mdlPopupEditSubscriber.Show();
                                return;
                            }
                        }
                    }

                    Subscriber s = new Subscriber();

                    s.SubscriptionID = Convert.ToInt32(hfSubscriptionID.Value);
                    s.FName = txt_FirstName.Text;
                    s.LName = txt_LastName.Text;
                    s.Company = txt_Company.Text;
                    s.Address = txt_Address.Text;
                    s.MailStop = txt_Address2.Text;
                    s.Address3 = txt_Address3.Text;
                    s.City = txt_City.Text;

                    if (pldrp_State.Visible)
                        s.State = drp_State.SelectedValue;
                    else
                        s.State = txt_State.Text;

                    s.Zip = txt_Zip.Text;
                    s.CountryID = drp_Country.SelectedValue != string.Empty ? Convert.ToInt32(drp_Country.SelectedValue) : (int?)null;
                    s.Title = txt_Title.Text;
                    s.ForZip = txt_ForZip.Text;
                    s.Plus4 = txt_Plus4.Text;
                    s.Phone = txt_Phone.Text;
                    s.Fax = txt_Fax.Text;
                    s.Mobile = txt_Mobile.Text;
                    s.Email = txt_Email.Text;
                    s.Score = Int32.Parse(lbl_Score.Text);
                    s.MailPermission = drp_MailPermission.SelectedItem.Value == MinusOne ? (bool?)null : drp_MailPermission.SelectedItem.Value == One ? true : false;
                    s.FaxPermission = drp_FaxPermission.SelectedItem.Value == MinusOne ? (bool?)null : drp_FaxPermission.SelectedItem.Value == One ? true : false;
                    s.PhonePermission = drp_PhonePermission.SelectedItem.Value == MinusOne ? (bool?)null : drp_PhonePermission.SelectedItem.Value == One ? true : false;
                    s.OtherProductsPermission = drp_OtherProductsPermission.SelectedItem.Value == MinusOne ? (bool?)null : drp_OtherProductsPermission.SelectedItem.Value == One ? true : false;
                    s.ThirdPartyPermission = drp_ThirdPartyPermission.SelectedItem.Value == MinusOne ? (bool?)null : drp_ThirdPartyPermission.SelectedItem.Value == One ? true : false;
                    s.EmailRenewPermission = drp_EmailRenewPermission.SelectedItem.Value == MinusOne ? (bool?)null : drp_EmailRenewPermission.SelectedItem.Value == One ? true : false;
                    s.TextPermission = drp_TextPermission.SelectedItem.Value == MinusOne ? (bool?)null : drp_TextPermission.SelectedItem.Value == One ? true : false;
                    s.Notes = txt_Notes.Text;
                    s.UpdatedByUserID = Master.LoggedInUser;

                    s.Save(Master.clientconnections);

                    foreach (GridViewRow row in grdProduct.Rows)
                    {
                        DropDownList drpPubEmailStatus = (DropDownList)grdProduct.Rows[row.RowIndex].FindControl("drpPubEmailStatus");
                        TextBox txtPubEmail = (TextBox)grdProduct.Rows[row.RowIndex].FindControl("txtPubEmail");

                        if (drpPubEmailStatus.SelectedItem.Value != string.Empty)
                            PubSubscriptions.UpdateEmailStatus(Master.clientconnections, drpPubEmailStatus.SelectedItem.Value, Convert.ToInt32(grdProduct.DataKeys[row.RowIndex].Value), txtPubEmail.Text);
                    }

                    LoadSubDetails();
                    dvMessage.Visible = true;
                    lblMessage.Text = "Saved";
                    mdlPopupEditSubscriber.Show();
                }
                catch (Exception ex)
                {
                    dvMessage.Visible = true;
                    lblMessage.Text = ex.Message;
                    mdlPopupEditSubscriber.Show();
                }
            }
        }

        private void ResetPopupControls()
        {
            txt_FirstName.Text = string.Empty;
            txt_LastName.Text = string.Empty;
            txt_Company.Text = string.Empty;
            txt_Address.Text = string.Empty;
            txt_Address2.Text = string.Empty;
            txt_Address3.Text = string.Empty;
            txt_City.Text = string.Empty;
            drp_State.ClearSelection();
            txt_State.Text = string.Empty;
            pldrp_State.Visible = false;
            pltxt_State.Visible = false;
            txt_Zip.Text = string.Empty;
            drp_Country.ClearSelection();
            txt_Title.Text = string.Empty;
            txt_ForZip.Text = string.Empty;
            txt_Plus4.Text = string.Empty;
            txt_Phone.Text = string.Empty;
            txt_Fax.Text = string.Empty;
            txt_Mobile.Text = string.Empty;
            txt_Email.Text = string.Empty;
            lbl_QDate.Text = string.Empty;
            lbl_TDate.Text = string.Empty;
            lbl_DateCreated.Text = string.Empty;
            lbl_DateUpdated.Text = string.Empty;
            lbl_Score.Text = string.Empty;
            drp_MailPermission.ClearSelection();
            drp_FaxPermission.ClearSelection();
            drp_PhonePermission.ClearSelection();
            drp_OtherProductsPermission.ClearSelection();
            drp_ThirdPartyPermission.ClearSelection();
            drp_EmailRenewPermission.ClearSelection();
            drp_TextPermission.ClearSelection();
            txt_Notes.Text = string.Empty;
            lbl_IGrp_No.Text = string.Empty;
            dvMessage.Visible = false;
            lblMessage.Text = string.Empty;
            gvActivityHistory.DataSource = null;
            gvActivityHistory.DataBind();
            gvWebSiteTracking.DataSource = null;
            gvWebSiteTracking.DataBind();
        }

        protected void grdProduct_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList drpPubEmailStatus = (DropDownList)e.Row.FindControl("drpPubEmailStatus");
                if (drpPubEmailStatus != null)
                {
                    drpPubEmailStatus.DataSource = EmailStatus.GetAll(Master.clientconnections);
                    drpPubEmailStatus.DataBind();
                    drpPubEmailStatus.Items.Insert(0, new ListItem("", ""));
                }

                Label lblPubEmailStatusID = (Label)e.Row.FindControl("lblPubEmailStatusID");

                try
                {
                    drpPubEmailStatus.Items.FindByValue(lblPubEmailStatusID.Text).Selected = true;
                }
                catch
                {
                }

                if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    UserDataMask udm = UserDataMask.GetByUserID(Master.clientconnections, Master.UserSession.UserID).Find(x => x.MaskField.ToUpper() == "EMAIL");

                    if (udm != null)
                    {
                        TextBox tb = (TextBox)e.Row.FindControl("txtPubEmail");
                        tb.Attributes["type"] = "password";
                        tb.ReadOnly = true;

                        RegularExpressionValidator revPubEmail = (RegularExpressionValidator)e.Row.FindControl("revPubEmail");
                        revPubEmail.Enabled = false;
                    }
                }
            }
        }

        #endregion

        #region EditAddress

        protected void drpMasterCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadMasterDrp_State();
            mdlPopupUpdateAddress.Show();
        }

        protected void loadMasterDrp_State()
        {
            if (drpMasterCountry.SelectedValue == One || drpMasterCountry.SelectedValue == Two || drpMasterCountry.SelectedValue == FourHundredTwentyNine)
            {
                pldrpMasterState.Visible = true;
                pltxtMasterState.Visible = false;
                drpMasterState.DataSource = Region.GetByCountryID(Convert.ToInt32(drpMasterCountry.SelectedValue));
                drpMasterState.DataBind();
                drpMasterState.Items.Insert(0, new ListItem("", ""));
            }
            else
            {
                pldrpMasterState.Visible = false;
                pltxtMasterState.Visible = true;
            }
        }

        private void ResetMasterControls()
        {
            txtMasterAddress.Text = string.Empty;
            txtMasterAddress2.Text = string.Empty;
            txtMasterAddress3.Text = string.Empty;
            txtMasterCity.Text = string.Empty;
            drpMasterCountry.ClearSelection();
            drpMasterState.ClearSelection();
            txtMasterState.Text = string.Empty;
            pldrpMasterState.Visible = false;
            pltxtMasterState.Visible = false;
            txtMasterZip.Text = string.Empty;
            txtMasterPhone.Text = string.Empty;
            txtMasterFax.Text = string.Empty;
            txtMasterEmail.Text = string.Empty;
        }

        private void LoadAddressDetails()
        {
            try
            {
                //Master Address

                ResetMasterControls();

                Subscriber sub = Subscriber.Get(Master.clientconnections, Convert.ToInt32(hfSubID.Value), Convert.ToInt32(hfBrandID.Value)).FirstOrDefault();

                lblMasterFirstName.Text = sub.FName;
                lblMasterLastName.Text = sub.LName;
                txtMasterAddress.Text = sub.Address;
                txtMasterAddress2.Text = sub.MailStop;
                txtMasterAddress3.Text = sub.Address3;
                txtMasterCity.Text = sub.City;

                drpMasterCountry.SelectedValue = sub.CountryID.ToString();

                try
                {
                    loadMasterDrp_State();

                    if (pldrpMasterState.Visible)
                        drpMasterState.SelectedValue = sub.State;
                    else
                        txtMasterState.Text = sub.State;
                }
                catch
                {
                }

                txtMasterZip.Text = sub.Zip;
                txtMasterPhone.Text = sub.Phone;
                txtMasterFax.Text = sub.Fax;
                txtMasterEmail.Text = sub.Email;

                if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    List<UserDataMask> udm = UserDataMask.GetByUserID(Master.clientconnections, Master.UserSession.UserID);
                    string maskField = string.Empty;

                    foreach (UserDataMask u in udm)
                    {
                        if (u.MaskField.ToUpper() != "COMPANY" && u.MaskField.ToUpper() != "TITLE" && u.MaskField.ToUpper() != "FIRSTNAME" && u.MaskField.ToUpper() != "LASTNAME")
                        {
                            TextBox tb = (TextBox)pnlPopupAddress.FindControl("txtMaster" + u.MaskField);
                            tb.Attributes["type"] = "password";
                            tb.ReadOnly = true;

                            if (pldrpMasterState.Visible)
                            {
                                int i = drpMasterState.SelectedIndex;
                                drpMasterState.Items[i].Text = BlackCircle;
                                drpMasterState.Enabled = false;
                            }
                        }
                        else if (u.MaskField.ToUpper() == "FIRSTNAME" || u.MaskField.ToUpper() == "LASTNAME")
                        {
                            lblMasterFirstName.Text = "\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf";
                            lblMasterLastName.Text = "\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf";
                        }

                        if (u.MaskField.ToUpper() == "EMAIL")
                        {
                            revMasterEmail.Enabled = false;
                        }
                    }
                }

                List<SubscriberPubs> sp = SubscriberPubs.GetSubscriberPubs(Master.clientconnections, Convert.ToInt32(hfSubID.Value), Convert.ToInt32(hfBrandID.Value));

                //sp = (from s in sp
                //      where s.IsCirc == false
                //      select s).ToList();

                gvProductAddress.DataSource = sp;
                gvProductAddress.DataBind();
            }
            catch
            {

            }
        }

        protected void gvProductAddress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox cbSelectAllProduct = e.Row.FindControl("cbSelectAllProduct") as CheckBox;

                cbSelectAllProduct.Attributes.Add("onclick", "cbSelectAllProduct_CheckedChanged()");
            }
            else
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList drpProductEmailStatus = (DropDownList)e.Row.FindControl("drpProductEmailStatus");
                if (drpProductEmailStatus != null)
                {
                    drpProductEmailStatus.DataSource = EmailStatus.GetAll(Master.clientconnections);
                    drpProductEmailStatus.DataBind();
                    drpProductEmailStatus.Items.Insert(0, new ListItem("", ""));
                }

                HiddenField hfProductEmailStatusID = (HiddenField)e.Row.FindControl("hfProductEmailStatusID");

                try
                {
                    drpProductEmailStatus.Items.FindByValue(hfProductEmailStatusID.Value).Selected = true;
                }
                catch
                {
                }

                DropDownList drpProductCountry = (DropDownList)e.Row.FindControl("drpProductCountry");
                drpProductCountry.DataSource = Country.GetSelectedCountries();
                drpProductCountry.DataBind();
                drpProductCountry.Items.Insert(0, new ListItem("", ""));

                HiddenField hfProductCountry = (HiddenField)e.Row.FindControl("hfProductCountry");

                try
                {
                    drpProductCountry.Items.FindByValue(hfProductCountry.Value).Selected = true;
                }
                catch
                {
                }

                PlaceHolder pldrpProductState = (PlaceHolder)e.Row.FindControl("pldrpProductState");
                PlaceHolder pltxtProductState = (PlaceHolder)e.Row.FindControl("pltxtProductState");
                DropDownList drpProductState = (DropDownList)e.Row.FindControl("drpProductState");
                TextBox txtProductState = (TextBox)e.Row.FindControl("txtProductState");
                HiddenField hfProductState = (HiddenField)e.Row.FindControl("hfProductState");

                if (drpProductCountry.SelectedValue == One || drpProductCountry.SelectedValue == Two || drpProductCountry.SelectedValue == FourHundredTwentyNine)
                {
                    pldrpProductState.Visible = true;
                    pltxtProductState.Visible = false;
                    drpProductState.DataSource = Region.GetByCountryID(Convert.ToInt32(drpProductCountry.SelectedValue));
                    drpProductState.DataBind();
                    drpProductState.Items.Insert(0, new ListItem("", ""));
                }
                else
                {
                    pldrpProductState.Visible = false;
                    pltxtProductState.Visible = true;
                }

                try
                {
                    if (pldrpProductState.Visible)
                        drpProductState.SelectedValue = hfProductState.Value;

                    else
                        txtProductState.Text = hfProductState.Value;
                }
                catch
                {
                }

                if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    List<UserDataMask> udm = UserDataMask.GetByUserID(Master.clientconnections, Master.UserSession.UserID);
                    string maskField = string.Empty;

                    foreach (UserDataMask u in udm)
                    {
                        if (u.MaskField.ToUpper() != "COMPANY" && u.MaskField.ToUpper() != "TITLE" && u.MaskField.ToUpper() != "FIRSTNAME" && u.MaskField.ToUpper() != "LASTNAME")
                        {
                            TextBox tb = (TextBox)e.Row.FindControl("txtProduct" + u.MaskField);
                            tb.Attributes["type"] = "password";
                            tb.ReadOnly = true;

                            if (pldrpProductState.Visible)
                            {
                                int i = drpProductState.SelectedIndex;
                                drpProductState.Items[i].Text = BlackCircle;
                                drpProductState.Enabled = false;
                            }
                        }
                        else if (u.MaskField.ToUpper() == "FIRSTNAME" || u.MaskField.ToUpper() == "LASTNAME")
                        {
                            Label lblProductFirstName = (Label)e.Row.FindControl("lblProductFirstName");
                            lblProductFirstName.Text = "\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf";
                            Label lblProductLastName = (Label)e.Row.FindControl("lblProductLastName");
                            lblProductLastName.Text = "\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf";
                        }

                        if (u.MaskField.ToUpper() == "EMAIL")
                        {
                            RegularExpressionValidator revProductEmail = (RegularExpressionValidator)e.Row.FindControl("revProductEmail");
                            revProductEmail.Enabled = false;
                        }
                    }
                }
            }
        }

        protected void drpProductCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drpProductCountry = (DropDownList)sender;
            GridViewRow row = (GridViewRow)drpProductCountry.NamingContainer;
            PlaceHolder pldrpProductState = (PlaceHolder)row.FindControl("pldrpProductState");
            PlaceHolder pltxtProductState = (PlaceHolder)row.FindControl("pltxtProductState");
            DropDownList drpProductState = (DropDownList)row.FindControl("drpProductState");
            TextBox txtProductState = (TextBox)row.FindControl("txtProductState");

            if (drpProductCountry.SelectedValue == One || drpProductCountry.SelectedValue == Two || drpProductCountry.SelectedValue == FourHundredTwentyNine)
            {
                pldrpProductState.Visible = true;
                pltxtProductState.Visible = false;
                drpProductState.DataSource = Region.GetByCountryID(Convert.ToInt32(drpProductCountry.SelectedValue));
                drpProductState.DataBind();
                drpProductState.Items.Insert(0, new ListItem("", ""));
            }
            else
            {
                pldrpProductState.Visible = false;
                pltxtProductState.Visible = true;
            }

            mdlPopupUpdateAddress.Show();
        }

        protected void gvProductAddress_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ProductAddressUpdate")
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecordDetails, KMPlatform.Enums.Access.Edit))
                {
                    try
                    {
                        GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                        TextBox txtProductAddress = (TextBox)row.FindControl("txtProductAddress");
                        TextBox txtProductAddress2 = (TextBox)row.FindControl("txtProductAddress2");
                        TextBox txtProductAddress3 = (TextBox)row.FindControl("txtProductAddress3");
                        TextBox txtProductCity = (TextBox)row.FindControl("txtProductCity");
                        PlaceHolder pldrpProductState = (PlaceHolder)row.FindControl("pldrpProductState");
                        DropDownList drpProductCountry = (DropDownList)row.FindControl("drpProductCountry");
                        DropDownList drpProductState = (DropDownList)row.FindControl("drpProductState");
                        TextBox txtProductState = (TextBox)row.FindControl("txtProductState");
                        TextBox txtProductZip = (TextBox)row.FindControl("txtProductZip");
                        TextBox txtProductCountry = (TextBox)row.FindControl("txtProductCountry");
                        TextBox txtProductPhone = (TextBox)row.FindControl("txtProductPhone");
                        TextBox txtProductFax = (TextBox)row.FindControl("txtProductFax");
                        TextBox txtProductEmail = (TextBox)row.FindControl("txtProductEmail");
                        DropDownList drpProductEmailStatus = (DropDownList)row.FindControl("drpProductEmailStatus");
                        Label lblProductFirstName = (Label)row.FindControl("lblProductFirstName");
                        Label lblProductLastName = (Label)row.FindControl("lblProductLastName");

                        bool IsValid = false;

                        if (!string.IsNullOrWhiteSpace(txtProductEmail.Text))
                            IsValid = true;

                        if (!string.IsNullOrWhiteSpace(lblProductFirstName.Text) && !string.IsNullOrWhiteSpace(txtProductAddress.Text) && !string.IsNullOrWhiteSpace(drpProductCountry.SelectedValue))
                            IsValid = true;

                        if (!string.IsNullOrWhiteSpace(lblProductLastName.Text) && !string.IsNullOrWhiteSpace(txtProductAddress.Text) && !string.IsNullOrWhiteSpace(drpProductCountry.SelectedValue))
                            IsValid = true;

                        if (!string.IsNullOrWhiteSpace(lblProductFirstName.Text) && !string.IsNullOrWhiteSpace(txtProductPhone.Text))
                            IsValid = true;

                        if (!string.IsNullOrWhiteSpace(lblProductLastName.Text) && !string.IsNullOrWhiteSpace(txtProductPhone.Text))
                            IsValid = true;

                        if (!IsValid)
                        {
                            divEditAddress.Visible = true;
                            lblMsgEditAddress.Text = "Either Email or FirstName with Address1 and Country or LastName with Address1 and Country or FirstName with Phone or LastName with Phone is required";
                            mdlPopupUpdateAddress.Show();
                            return;
                        }

                        PubSubscriptions ps = new PubSubscriptions();
                        ps.PubSubscriptionID = Convert.ToInt32(e.CommandArgument.ToString());
                        ps.Address1 = txtProductAddress.Text;
                        ps.Address2 = txtProductAddress2.Text;
                        ps.Address3 = txtProductAddress3.Text;
                        ps.City = txtProductCity.Text;
                        ps.ZipCode = txtProductZip.Text;
                        ps.CountryID = drpProductCountry.SelectedValue != string.Empty ? Convert.ToInt32(drpProductCountry.SelectedValue) : (int?)null;

                        if (pldrpProductState.Visible)
                            ps.RegionCode = drpProductState.SelectedValue;
                        else
                            ps.RegionCode = txtProductState.Text;

                        ps.Phone = txtProductPhone.Text;
                        ps.Fax = txtProductFax.Text;
                        ps.Email = txtProductEmail.Text;
                        ps.UpdatedByUserID = Master.LoggedInUser;
                        PubSubscriptions.UpdateAddress(Master.clientconnections, ps, drpProductEmailStatus.SelectedValue);

                        Subscriber ss = new Subscriber();
                        ss.SubscriptionID = Convert.ToInt32(hfSubID.Value);
                        ss.Address = txtProductAddress.Text;
                        ss.MailStop = txtProductAddress2.Text;
                        ss.Address3 = txtProductAddress3.Text;
                        ss.City = txtProductCity.Text;
                        ss.CountryID = drpProductCountry.SelectedValue != string.Empty ? Convert.ToInt32(drpProductCountry.SelectedValue) : (int?)null;

                        if (pldrpProductState.Visible)
                            ss.State = drpProductState.SelectedValue;
                        else
                            ss.State = txtProductState.Text;

                        ss.Zip = txtProductZip.Text;
                        ss.Phone = txtProductPhone.Text;
                        ss.Fax = txtProductFax.Text;
                        ss.Email = txtProductEmail.Text;
                        ss.UpdatedByUserID = Master.LoggedInUser;

                        Subscriber.UpdateAddress(Master.clientconnections, ss, false);

                        divEditAddress.Visible = true;
                        lblMsgEditAddress.Text = "Saved";

                        loadMasterProductData();

                        mdlPopupUpdateAddress.Show();
                    }
                    catch
                    {
                    }
                }
            }
        }

        protected void cbSelectAllProduct_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbSelectAllProduct = gvProductAddress.HeaderRow.FindControl("cbSelectAllProduct") as CheckBox;

            foreach (GridViewRow r in gvProductAddress.Rows)
            {
                CheckBox cb = r.FindControl("cbSelectProduct") as CheckBox;
                cb.Checked = cbSelectAllProduct.Checked;
            }

            mdlPopupUpdateAddress.Show();
        }

        protected void btnUpdateInSelected_click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecordDetails, KMPlatform.Enums.Access.Edit))
            {
                try
                {
                    bool ischecked = false;

                    foreach (GridViewRow r in gvProductAddress.Rows)
                    {
                        CheckBox cb = r.FindControl("cbSelectProduct") as CheckBox;

                        if (cb != null && cb.Checked)
                        {
                            ischecked = true;
                        }
                    }

                    if (!ischecked)
                    {
                        lblMsgEditAddress.Text = "Please select a checkbox.";
                        divEditAddress.Visible = true;
                        mdlPopupUpdateAddress.Show();
                        return;
                    }

                    bool IsValid = false;

                    if (!string.IsNullOrWhiteSpace(txtMasterEmail.Text))
                        IsValid = true;

                    if (!string.IsNullOrWhiteSpace(lblMasterFirstName.Text) && !string.IsNullOrWhiteSpace(txtMasterAddress.Text) && !string.IsNullOrWhiteSpace(drpMasterCountry.SelectedValue))
                        IsValid = true;

                    if (!string.IsNullOrWhiteSpace(lblMasterLastName.Text) && !string.IsNullOrWhiteSpace(txtMasterAddress.Text) && !string.IsNullOrWhiteSpace(drpMasterCountry.SelectedValue))
                        IsValid = true;

                    if (!string.IsNullOrWhiteSpace(lblMasterFirstName.Text) && !string.IsNullOrWhiteSpace(txtMasterPhone.Text))
                        IsValid = true;

                    if (!string.IsNullOrWhiteSpace(lblMasterLastName.Text) && !string.IsNullOrWhiteSpace(txtMasterPhone.Text))
                        IsValid = true;

                    if (!IsValid)
                    {
                        divEditAddress.Visible = true;
                        lblMsgEditAddress.Text = "Either Email or FirstName with Address1 and Country or LastName with Address1 and Country or FirstName with Phone or LastName with Phone is required";
                        mdlPopupUpdateAddress.Show();
                        return;
                    }


                    Subscriber sub = new Subscriber();
                    sub.SubscriptionID = Convert.ToInt32(hfSubID.Value);
                    sub.Address = txtMasterAddress.Text;
                    sub.MailStop = txtMasterAddress2.Text;
                    sub.Address3 = txtMasterAddress3.Text;
                    sub.City = txtMasterCity.Text;
                    sub.Zip = txtMasterZip.Text;
                    sub.CountryID = drpMasterCountry.SelectedValue != string.Empty ? Convert.ToInt32(drpMasterCountry.SelectedValue) : (int?)null;

                    if (pldrpMasterState.Visible)
                        sub.State = drpMasterState.SelectedValue;
                    else
                        sub.State = txtMasterState.Text;

                    sub.Phone = txtMasterPhone.Text;
                    sub.Fax = txtMasterFax.Text;
                    sub.Email = txtMasterEmail.Text;
                    sub.UpdatedByUserID = Master.LoggedInUser;

                    Subscriber.UpdateAddress(Master.clientconnections, sub, true);

                    int i = 0;
                    foreach (GridViewRow r in gvProductAddress.Rows)
                    {
                        CheckBox cb = (CheckBox)r.FindControl("cbSelectProduct");
                        DropDownList drpProductEmailStatus = (DropDownList)r.FindControl("drpProductEmailStatus");

                        if (cb != null && cb.Checked)
                        {
                            PubSubscriptions ps = new PubSubscriptions();
                            ps.PubSubscriptionID = Convert.ToInt32(gvProductAddress.DataKeys[i].Values["PubSubscriptionID"].ToString());
                            ps.Address1 = txtMasterAddress.Text;
                            ps.Address2 = txtMasterAddress2.Text;
                            ps.Address3 = txtMasterAddress3.Text;
                            ps.City = txtMasterCity.Text;
                            ps.ZipCode = txtMasterZip.Text;
                            ps.CountryID = drpMasterCountry.SelectedValue != string.Empty ? Convert.ToInt32(drpMasterCountry.SelectedValue) : (int?)null;

                            if (pldrpMasterState.Visible)
                                ps.RegionCode = drpMasterState.SelectedValue;
                            else
                                ps.RegionCode = txtMasterState.Text;

                            ps.Phone = txtMasterPhone.Text;
                            ps.Fax = txtMasterFax.Text;
                            ps.Email = txtMasterEmail.Text;
                            ps.UpdatedByUserID = Master.LoggedInUser;

                            PubSubscriptions.UpdateAddress(Master.clientconnections, ps, drpProductEmailStatus.SelectedValue);
                        }

                        i++;
                    }

                    divEditAddress.Visible = true;
                    lblMsgEditAddress.Text = "Saved in selected products.";

                    loadMasterProductData();

                    mdlPopupUpdateAddress.Show();

                }
                catch
                {
                }
            }
        }

        //protected void btnUpdateAllProducts_click(object sender, EventArgs e)
        //{
        //    if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecordDetails, KMPlatform.Enums.Access.Edit))
        //    {
        //        try
        //        {
        //            Subscriber sub = new Subscriber();
        //            sub.SubscriptionID = Convert.ToInt32(hfSubID.Value);
        //            sub.Address = txtMasterAddress1.Text;
        //            sub.MailStop = txtMasterAddress2.Text;
        //            sub.Address3 = txtMasterAddress3.Text;
        //            sub.City = txtMasterCity.Text;
        //            sub.Zip = txtMasterZip.Text;
        //            sub.CountryID = drpMasterCountry.SelectedValue != string.Empty ? Convert.ToInt32(drpMasterCountry.SelectedValue) : (int?)null; 

        //            if (pldrpMasterState.Visible)
        //                sub.State = drpMasterState.SelectedValue;
        //            else
        //                sub.State = txtMasterState.Text;

        //            sub.Phone = txtMasterPhone.Text;
        //            sub.Fax = txtMasterFax.Text;
        //            sub.Email = txtMasterEmail.Text;
        //            sub.UpdatedByUserID = Master.LoggedInUser;
        //            Subscriber.UpdateAddress(sub, true);

        //            int i = 0;
        //            foreach (GridViewRow r in gvProductAddress.Rows)
        //            {
        //                DropDownList drpProductEmailStatus = (DropDownList)r.FindControl("drpProductEmailStatus");

        //                PubSubscriptions ps = new PubSubscriptions();
        //                ps.PubSubscriptionID = Convert.ToInt32(gvProductAddress.DataKeys[i].Values["PubSubscriptionID"].ToString());
        //                ps.Address1 = txtMasterAddress1.Text;
        //                ps.Address2 = txtMasterAddress2.Text;
        //                ps.Address3 = txtMasterAddress3.Text;
        //                ps.City = txtMasterCity.Text;
        //                ps.ZipCode = txtMasterZip.Text;
        //                ps.CountryID = drpMasterCountry.SelectedValue != string.Empty ? Convert.ToInt32(drpMasterCountry.SelectedValue) : (int?)null; 

        //                if (pldrpMasterState.Visible)
        //                    ps.RegionCode = drpMasterState.SelectedValue;
        //                else
        //                    ps.RegionCode = txtMasterState.Text;

        //                ps.Phone = txtMasterPhone.Text;
        //                ps.Fax = txtMasterFax.Text;
        //                ps.Email = txtMasterEmail.Text;
        //                ps.UpdatedByUserID = Master.LoggedInUser;

        //                PubSubscriptions.UpdateAddress(ps, Convert.ToInt32(drpProductEmailStatus.SelectedValue));

        //                i++;
        //            }
        //            divEditAddress.Visible = true;
        //            lblMsgEditAddress.Text = "Saved in all products.";

        //            loadMasterProductData();

        //            mdlPopupUpdateAddress.Show();

        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //}

        private void loadMasterProductData()
        {
            ResetMasterControls();

            List<SubscriberPubs> sp = SubscriberPubs.GetSubscriberPubs(Master.clientconnections, Convert.ToInt32(hfSubID.Value), Convert.ToInt32(hfBrandID.Value));

            //sp = (from s in sp
            //      where s.IsCirc == false
            //      select s).ToList();

            gvProductAddress.DataSource = sp;
            gvProductAddress.DataBind();

            Subscriber ss = Subscriber.Get(Master.clientconnections, Convert.ToInt32(hfSubID.Value), Convert.ToInt32(hfBrandID.Value)).FirstOrDefault();

            lblMasterFirstName.Text = ss.FName;
            lblMasterLastName.Text = ss.LName;
            txtMasterAddress.Text = ss.Address;
            txtMasterAddress2.Text = ss.MailStop;
            txtMasterAddress3.Text = ss.Address3;
            txtMasterCity.Text = ss.City;
            txtMasterZip.Text = ss.Zip;
            drpMasterCountry.SelectedValue = ss.CountryID.ToString();

            try
            {
                loadMasterDrp_State();

                if (pldrpMasterState.Visible)
                    drpMasterState.SelectedValue = ss.State;
                else
                    txtMasterState.Text = ss.State;
            }
            catch
            {
            }

            txtMasterPhone.Text = ss.Phone;
            txtMasterFax.Text = ss.Fax;
            txtMasterEmail.Text = ss.Email;

            if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            {
                List<UserDataMask> udm = UserDataMask.GetByUserID(Master.clientconnections, Master.UserSession.UserID);
                string maskField = string.Empty;
                foreach (UserDataMask u in udm)
                {
                    if (u.MaskField.ToUpper() == "FIRSTNAME" || u.MaskField.ToUpper() == "LASTNAME")
                    {
                        lblMasterFirstName.Text = "\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf";
                        lblMasterLastName.Text = "\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf\u25cf";
                    }
                    else if (u.MaskField.ToUpper() == "STATE")
                    {
                        int i = drpMasterState.SelectedIndex;
                        drpMasterState.Items[i].Text = BlackCircle;
                        drpMasterState.Enabled = false;
                    }
                }
            }
        }

        #endregion

        protected void btnDownload_click(Object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecordDetails, KMPlatform.Enums.Access.Download))
            {
                string companylogo = string.Empty;
                int customerID = Master.UserSession.CustomerID;

                Config c = Config.getCustomerLogo(Master.clientconnections);
                if (c.ConfigID != 0)
                    companylogo = "File:///" + Server.MapPath("../Images/logo/" + customerID + "/") + c.Value;

                string brandlogo = string.Empty;
                if (Convert.ToInt32(hfBrandID.Value) > 0)
                {
                    Brand b = Brand.GetByID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                    if (b.Logo != string.Empty)
                        brandlogo = "File:///" + Server.MapPath("../Images/logo/" + customerID + "/") + b.Logo;
                }

                Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource("DS_Subscriber", Subscriber.Get(Master.clientconnections, Convert.ToInt32(hfSubscriptionID.Value), Convert.ToInt32(hfBrandID.Value)));
                Microsoft.Reporting.WebForms.ReportDataSource rds1 = new Microsoft.Reporting.WebForms.ReportDataSource("DS_SubscriberDimension", SubscriberDimension.GetSubscriberDimensionForExport(Master.clientconnections, Convert.ToInt32(hfSubscriptionID.Value), Convert.ToInt32(hfBrandID.Value)));
                Microsoft.Reporting.WebForms.ReportDataSource rds2 = new Microsoft.Reporting.WebForms.ReportDataSource("DS_SubscriberPubs", SubscriberPubs.GetSubscriberPubsForExport(Master.clientconnections, Convert.ToInt32(hfSubscriptionID.Value), Convert.ToInt32(hfBrandID.Value)));
                Microsoft.Reporting.WebForms.ReportDataSource rds3 = new Microsoft.Reporting.WebForms.ReportDataSource("DS_SubscriberOpenActivity", SubscriberActivity.GetOpenActivity(Master.clientconnections, Convert.ToInt32(hfSubscriptionID.Value), Convert.ToInt32(hfBrandID.Value)).Take(5));
                Microsoft.Reporting.WebForms.ReportDataSource rds4 = new Microsoft.Reporting.WebForms.ReportDataSource("DS_SubscriberClickActivity", SubscriberActivity.GetClickActivity(Master.clientconnections, Convert.ToInt32(hfSubscriptionID.Value), Convert.ToInt32(hfBrandID.Value)).Take(5));
                Microsoft.Reporting.WebForms.ReportDataSource rds5 = new Microsoft.Reporting.WebForms.ReportDataSource("DS_SubscriberVisitActivity", SubscriberVisitActivity.Get(Master.clientconnections, Convert.ToInt32(hfSubscriptionID.Value)).Take(5));
                Microsoft.Reporting.WebForms.ReportDataSource rds6 = new Microsoft.Reporting.WebForms.ReportDataSource("DS_SubscriberAdhoc", SubscriberAdhoc.GetForRecordView(Master.clientconnections, Convert.ToInt32(hfSubscriptionID.Value)));
                Microsoft.Reporting.WebForms.ReportDataSource rds7 = new Microsoft.Reporting.WebForms.ReportDataSource("DS_SubscriberActivity", SubscriberActivity.Get(Master.clientconnections, Convert.ToInt32(hfSubscriptionID.Value), Convert.ToInt32(hfBrandID.Value)).Take(10));

                ReportViewer1.Visible = false;
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.DataSources.Add(rds1);
                ReportViewer1.LocalReport.DataSources.Add(rds2);
                ReportViewer1.LocalReport.DataSources.Add(rds3);
                ReportViewer1.LocalReport.DataSources.Add(rds4);
                ReportViewer1.LocalReport.DataSources.Add(rds5);
                ReportViewer1.LocalReport.DataSources.Add(rds6);
                ReportViewer1.LocalReport.DataSources.Add(rds7);

                ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/main/reports/" + "rpt_SubscriberDetails.rdlc");
                ReportParameter[] parameters = new ReportParameter[4];
                parameters[0] = new ReportParameter("SubscriptionID", hfSubscriptionID.Value);
                parameters[1] = new ReportParameter("CompanyLogo", companylogo);
                parameters[2] = new ReportParameter("BrandLogo", brandlogo);
                parameters[3] = new ReportParameter("BrandID", hfBrandID.Value);
                ReportViewer1.LocalReport.SetParameters(parameters);

                ReportViewer1.LocalReport.Refresh();

                Warning[] warnings = null;
                string[] streamids = null;
                String mimeType = null;
                String encoding = null;
                String extension = null;
                Byte[] bytes = null;

                bytes = ReportViewer1.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Response.ContentType = "application/pdf";

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=SubscriberDetails.pdf");
                Response.BinaryWrite(bytes);
                Response.End();
            }
        }

        protected void PubTypeRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            LinkButton PubTypeLinkButton = e.Item.FindControl("PubTypeLinkButton") as LinkButton;
            PubTypeLinkButton.CommandName = e.Item.ItemIndex.ToString();

            LinkButton lnkPubTypeShowHide = e.Item.FindControl("lnkPubTypeShowHide") as LinkButton;
            lnkPubTypeShowHide.CommandName = e.Item.ItemIndex.ToString();
        }

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

        #endregion

        #region Circulation Popup Events

        protected void lnkCirculation_Command(object sender, CommandEventArgs e)
        {
            CirculationFilter.Visible = true;
            CirculationFilter.ViewType = Enums.ViewType.ConsensusView;
            CirculationFilter.LoadCirculationControls();
        }

        #endregion

        #region Filter Popup Events

        protected void lnkSavedFilter_Command(object sender, CommandEventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.View))
            {
                ShowFilter.BrandID = Convert.ToInt32(hfBrandID.Value);
                ShowFilter.PubID = 0;
                ShowFilter.UserID = Master.LoggedInUser;
                ShowFilter.ViewType = Enums.ViewType.ConsensusView;
                ShowFilter.Mode = "Load";
                ShowFilter.AllowMultiRowSelection = true;
                ShowFilter.LoadControls();
                ShowFilter.Visible = true;
            }
        }

        #endregion

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
