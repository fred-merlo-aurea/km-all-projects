using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using KMPlatform.Entity;
using KMPlatform.Object.Fakes;
using KMPS.MD.Controls;
using KMPS.MD.Controls.Fakes;
using KMPS.MD.Main;
using KMPS.MD.Main.Fakes;
using KMPS.MD.MasterPages.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using Telerik.Web.UI.Fakes;
using TestCommonHelpers;
using TelerikUI = Telerik.Web.UI;
using ShimAdhoc = KMPS.MD.Controls.Fakes.ShimAdhoc;
using ObjShimDataFunctions = FrameworkUAD.DataAccess.Fakes.ShimDataFunctions;
using UADBusinessLogicFakes = FrameworkUAD.BusinessLogic.Fakes;
using ObjShimKmps=KMPS.MD.Objects.Fakes;
using KM.Platform.Fakes;
using static KMPlatform.Enums;

namespace KMPS.MD.Tests.AudienceViews
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ProductViewTests : BasePageTests
    {
        private const string TestZero = "0";
        private const string TestOne = "1";
        private const string AllProducts = "All Products";
        private const string MethodPageLoad = "Page_Load";
        private const string MethodSelectStateOnRegion = "SelectStateOnRegion";
        private const string MethodSelectCountryOnRegion = "SelectCountryOnRegion";
        private const string TestNumber = "1";
        private const string ExpectedZipCodeRadiusValues = "1.97101449275362|2.02898550724638|0.970996308707783|1.02900266654272|1.98550724637681|2.01449275362319|0.98549828384275|1.01450145996992";
        private const string TestZipCode = "NY12221";
        private const string CountryRegionsList = "lstCountryRegions";
        private const string CountryList = "lstCountry";
        private const string GeoList = "lstGeoCode";
        private const string StateList = "lstState";
        private const string GetAddlFilter = "GetAddlFilter";
        private const string LabelPermission = "lblPermission";
        private const string LabelCodeSheetID = "lblCodeSheetID";
        private const string LabelPremissionType = "lblPremissionType";
        private const string HiddenFiledRow = "hfRow";
        private const string HiddenFiledColumn = "hfColumn";
        private const string HiddenSelectedCrossTabLink = "hfSelectedCrossTabLink";
        private const string HiddenFiledRowSearchValue = "hfRowSearchValue";
        private const string DropDownCrossTabReport = "drpCrossTabReport";
        private const string HiddenFieldColumnSearchValue = "hfColumnSearchValue";
        private const string FiltersObject = "fc";
        private const string GetFilter = "getFilter";
        private const string TextRadiusMin = "txtRadiusMin";
        private const string TextRadiusMax = "txtRadiusMax";
        private const string ZipcodeRadius = "Zipcode-Radius";
        private const string Product = "Product";
        private const string HfProductID = "hfProductID";
        private const string HiddenFiledBrandID = "hfBrandID";
        private const string State = "STATE";
        private const string Country = "COUNTRY";
        private const string Email = "EMAIL";
        private const string Phone = "PHONE";
        private const string Fax = "FAX";
        private const string Media = "MEDIA";
        private const string GeoLocated = "GEOLOCATED";
        private const string MailPermission = "MAILPERMISSION";
        private const string FaxPermission = "FAXPERMISSION";
        private const string PhonePermission = "PHONEPERMISSION";
        private const string OthersProductsPermission = "OTHERPRODUCTSPERMISSION";
        private const string ThirdPartyPermission = "THIRDPARTYPERMISSION";
        private const string EmailRenewPermission = "EMAILRENEWPERMISSION";
        private const string TxtPermission = "TEXTPERMISSION";
        private const string EmailStatus = "EMAIL STATUS";
        private const string Test = "Test";
        private const string LinkButtonCancel = "lnkCancel";
        private const string LinkButtonEdit = "lnkEdit";
        private const string ListResponse = "lstResponse";
        private const string LabelResponseGroup = "lblResponseGroup";
        private const string FiledValue = "1,2,3";
        private const string GeoSearchConditionWithValidValue = "1|2|3";
        private const string GeoSearchConditionWithInValidValue = "a|2|3";
        private const string DummyString = "DummyString";
        private const string LstResponse = "lstResponse";
        private const string CountryRegions = "Country Regions";
        private const string LstMarket = "LSTMARKET";
        private const string PnlBrand = "pnlBrand";
        private const string DrpBrand = "drpBrand";
        private const string RcbProduct = "rcbProduct";
        private const string DrpDataCompareSourceFile = "drpDataCompareSourceFile";
        private const string RadMtxtboxZipCode = "RadMtxtboxZipCode";
        private const string RblDataCompareOperation = "rblDataCompareOperation";
        private const string ManualLoad = "Manual Load";
        private const string AutoLoad = "Auto Load";
        private const string FilterNo = "FilterNo";
        private const string DefaultAdHocText = "Adhoc";
        private const string RadioButtonListLoadType = "rblLoadType";
        private const string Linkdownload = "lnkdownload";
        private const string LabelBrandID = "lblBrandID";
        private const string HiddenFielsFilterGroupName = "hfFilterGroupName";
        private const string HiddenFieldViewType = "hfViewType";
        private const string LabelFilterText = "lblFilterText";
        private const string LabelFilterValues = "lblFilterValues";
        private const string LabelSearchCondition = "lblSearchCondition";
        private const string LabelFilterType = "lblFilterType";
        private const string LabelGroup = "lblGroup";
        private const string GridViewFilterValues = "grdFilterValues";
        private const string LabelFilterName = "lblFiltername";
        private const string DefaultText = "Unit Test";
        private const string ManualLoadClientIds = "ManualLoad_ClientIDs";
        private const string LnkSavedFilter = "lnkSavedFilter";
        private const string BtnOpenSaveFilterPopup = "btnOpenSaveFilterPopup";
        private const string BtnLoadComboVenn = "btnLoadComboVenn";
        private const string BtnOpenSaveFilterPopupClick = "btnOpenSaveFilterPopup_Click";
        private const string FilterSaveControlName = "FilterSave";
        private const string FilterNoDataKey = "FilterNo";
        private const string AddNewFilterMode = "AddNew";
        private const string LblErrorMsg = "lblErrorMsg";
        private const string DivErrorMsg = "divErrorMsg";
        private const string DrpCountrySelectedIndexChanged = "drpCountry_SelectedIndexChanged";
        private const string DrpCountry = "drpCountry";
        private const string HfResponseGroupID = "hfResponseGroupID";
        private const string LnkDimensionShowHide = "lnkDimensionShowHide";
        private const string PnlDimBody = "pnlDimBody";
        private const string LnkDimensionShowHideText = "(Show...)";
        private ProductView _testEntity;
        private List<Control> _tempControls = new List<Control>
        {
            new Control(),
            new Control(),
            new Control(),
            new Control()
        };

        private Filters _filters;

        [SetUp]
        public override void SetUp()
        {
            UserId = 1;

            base.SetUp();
            _testEntity = new ProductView();
            InitializePage(_testEntity);

            ShimProductView.AllInstances.GetSubscribersQueriesForUserControl = report => new StringBuilder(TestNumber);

            GetField<Label>("lblErrorMsg").Visible = false;

            ShimControl.AllInstances.ParentGet = control =>
            {
                var index = _tempControls.IndexOf(control);
                if (index == -1)
                {
                    return _tempControls[0];
                }
                else
                {
                    if (index + 1 < _tempControls.Count)
                    {
                        return _tempControls[index + 1];
                    }
                }

                return null;
            };

            ShimControl.AllInstances.FindControlString = (control, name) =>
            {
                if (name == "hfHasFilterSegmentation")
                {
                    return new HiddenField();
                }

                return null;
            };

            _filters = new Filters(new ShimClientConnections(), 1);
            _filters.FilterComboList = new List<FilterCombo>
            {
                new FilterCombo {SelectedFilterNo = "1"}
            };

            _filters.Add(new Filter
            {
                FilterNo = 1,
                FilterName = "test"
            });

            ReflectionHelper.SetValue(
                _testEntity,
                FiltersObject,
                new Filters(new ShimClientConnections(), 1)
                {
                    FilterComboList = new List<FilterCombo>
                    {
                        new FilterCombo {SelectedFilterNo = "1"}
                    }
                });

            ObjShimDataFunctions.GetClientSqlConnectionClientConnections = connections => new ShimSqlConnection();
            FrameworkUAD.DataAccess.Fakes.ShimFilterSegmentation.SelectByIDInt32ClientConnections = (_, __) => new FrameworkUAD.Entity.FilterSegmentation();
            ShimMDFilter.LoadFiltersClientConnectionsInt32Int32 = (_, __, ___) => _filters;
            KMPlatform.BusinessLogic.Fakes.ShimClient.AllInstances.SelectInt32Boolean = (_, __, ___) => new Client();
            ShimGridView.AllInstances.DataBind = _ => { };
            ShimProductView.AllInstances.LoadProducts = _ => { };
            ShimProductView.AllInstances.loadStandardFiltersListboxes = _ => { };
            ConfigurationManager.AppSettings["ManualLoad_ClientIDs"] = TestOne;
            ShimEmailStatus.GetAllClientConnections = _ => null;
            KM.Platform.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, feature, ____) => (feature != KMPlatform.Enums.ServiceFeatures.DataCompare);
            GetField<HiddenField>("hfBrandID").Value = TestZero;
            GetField<Panel>("pnlBrand").Visible = false;
            GetField<DropDownList>("drpBrand").Visible = false;
            GetField<Label>("lblBrandName").Visible = false;
            CheckBoxControl = new CheckBox() { Checked = true };
        }

        [Test]
        public void ShowEVDownloadPanel_PanelNotVisible_SetsPanelVisibleAndSetProperties()
        {
            // Arrange
            var rdl = GetField<RadioButtonList>("rblDataCompareOperation");
            rdl.Items.Add(TestNumber);
            rdl.SelectedValue = TestNumber;

            var panel = GetField<DownloadPanel_EV>("EVDownloadPanel");
            panel.Visible = false;

            GetField<HiddenField>("hfBrandID").Value = TestNumber;
            GetField<Label>("lblDownloadCount").Text = TestNumber;
            GetField<HiddenField>("hfDcRunID").Value = TestNumber;
            GetField<HiddenField>("hfDcTargetCodeID").Value = TestNumber;
            GetField<LinkButton>("lnkMatchedRecords").Text = TestNumber;
            GetField<LinkButton>("lnkNonMatchedRecords").Text = TestNumber;
            GetField<Label>("lblTotalFileRecords").Text = TestNumber;

            var isLoadControlsCalled = false;

            ShimDownloadPanel_EV.AllInstances.LoadControls = ev => { isLoadControlsCalled = true; };
            ShimDownloadPanel_EV.AllInstances.LoadDownloadTemplate = ev => { };
            ShimDownloadPanel_EV.AllInstances.loadExportFields = ev => { };
            ShimDownloadPanel_EV.AllInstances.ValidateDownload = ev => { };

            // Act
            PrivatePage.Invoke("ShowEVDownloadPanel");

            // Assert
            panel.Visible.ShouldBeTrue();
            panel.SubscribersQueries.ToString().ShouldBe(TestNumber);
            panel.BrandID.ShouldBe(1);
            panel.filterCombination.ShouldBeEmpty();
            panel.dcRunID.ShouldBe(1);
            panel.dcTypeCodeID.ShouldBe(1);
            panel.dcTargetCodeID.ShouldBe(1);
            panel.matchedRecordsCount.ShouldBe(1);
            panel.nonMatchedRecordsCount.ShouldBe(1);
            panel.TotalFileRecords.ShouldBe(1);
            isLoadControlsCalled.ShouldBeTrue();
        }

        [Test]
        public void ShowCLDownloadPanel_PanelNotVisible_SetsPanelVisibleAndSetProperties()
        {
            // Arrange
            var rdl = GetField<RadioButtonList>("rblDataCompareOperation");
            rdl.Items.Add(TestNumber);
            rdl.SelectedValue = TestNumber;

            var panel = GetField<DownloadPanel_CLV>("CLDownloadPanel");
            panel.Visible = false;

            GetField<HiddenField>("hfBrandID").Value = TestNumber;
            GetField<Label>("lblDownloadCount").Text = TestNumber;
            GetField<HiddenField>("hfDcRunID").Value = TestNumber;
            GetField<HiddenField>("hfDcTargetCodeID").Value = TestNumber;
            GetField<LinkButton>("lnkMatchedRecords").Text = TestNumber;
            GetField<LinkButton>("lnkNonMatchedRecords").Text = TestNumber;
            GetField<Label>("lblTotalFileRecords").Text = TestNumber;

            var isLoadControlsCalled = false;

            ShimDownloadPanel_CLV.AllInstances.LoadControls = cl => { isLoadControlsCalled = true; };
            ShimDownloadPanel_CLV.AllInstances.LoadDownloadTemplate = cl => { };
            ShimDownloadPanel_CLV.AllInstances.loadExportFields = cl => { };
            ShimDownloadPanel_CLV.AllInstances.ValidateDownload = cl => { };

            // Act
            PrivatePage.Invoke("ShowCLDownloadPanel");

            // Assert
            panel.Visible.ShouldBeTrue();
            panel.SubscribersQueries.ToString().ShouldBe(TestNumber);
            panel.BrandID.ShouldBe(1);
            panel.filterCombination.ShouldBeEmpty();
            panel.dcRunID.ShouldBe(1);
            panel.dcTypeCodeID.ShouldBe(1);
            panel.dcTargetCodeID.ShouldBe(1);
            panel.matchedRecordsCount.ShouldBe(1);
            panel.nonMatchedRecordsCount.ShouldBe(1);
            panel.TotalFileRecords.ShouldBe(1);
            isLoadControlsCalled.ShouldBeTrue();
        }

        [Test]
        public void LoadFilterSegmentationData_LoadTypeManual_FilterGetCountsIsCalled()
        {
            // Arrange
            var isGetCountsCalled = false;

            var filterSegmentation = GetField<FilterSegmentation>("FilterSegmentation");
            filterSegmentation.Visible = false;

            var rdl = GetField<RadioButtonList>("rblLoadType");
            rdl.Items.Add("Manual Load");
            rdl.SelectedValue = "Manual Load";

            GetField<HiddenField>("hfProductID").Value = TestNumber;
            GetField<HiddenField>("hfBrandID").Value = TestNumber;

            ShimFilterSegmentation.AllInstances.LoadFilterSegmenationData = _ => { };
            ShimFilter.AllInstances.GetCountsClientConnections = (_, __) => { isGetCountsCalled = true; };

            // Act
            _testEntity.LoadFilterSegmentationData(0);

            // Assert
            isGetCountsCalled.ShouldBeTrue();
            filterSegmentation.Visible.ShouldBeTrue();
            filterSegmentation.ViewType.ShouldBe(Enums.ViewType.ProductView);
            filterSegmentation.UserID.ShouldBe(1);
            filterSegmentation.PubID.ShouldBe(1);
            GetField<DropDownList>("drpDataCompareSourceFile").Enabled.ShouldBeFalse();
            GetField<RadioButtonList>("rblDataCompareOperation").Enabled.ShouldBeFalse();
            GetField<DropDownList>("drpBrand").Enabled.ShouldBeTrue();
            GetField<Label>("lblErrorMsg").Text.ShouldBeEmpty();
            GetField<Label>("lblErrorMsg").Visible.ShouldBeFalse();
        }

        [Test]
        public void LoadFilterSegmentationData_LoadTypeAuto_FilterGetCountsIsNotCalled()
        {
            // Arrange
            var isGetCountsCalled = false;

            var filterSegmentation = GetField<FilterSegmentation>("FilterSegmentation");
            filterSegmentation.Visible = false;

            var rdl = GetField<RadioButtonList>("rblLoadType");
            rdl.Items.Add("Auto Load");
            rdl.SelectedValue = "Auto Load";

            GetField<HiddenField>("hfProductID").Value = TestNumber;
            GetField<HiddenField>("hfBrandID").Value = TestNumber;

            ShimFilterSegmentation.AllInstances.LoadFilterSegmenationData = _ => { };
            ShimFilter.AllInstances.GetCountsClientConnections = (_, __) => { isGetCountsCalled = true; };
            ShimFilters.AllInstances.Execute = _ => { };
            ShimFilterVennDiagram.AllInstances.CreateVennFilters = (_, __) => { };

            // Act
            _testEntity.LoadFilterSegmentationData(0);

            // Assert
            isGetCountsCalled.ShouldBeFalse();
            filterSegmentation.Visible.ShouldBeTrue();
            filterSegmentation.ViewType.ShouldBe(Enums.ViewType.ProductView);
            filterSegmentation.UserID.ShouldBe(1);
            filterSegmentation.PubID.ShouldBe(1);
            GetField<DropDownList>("drpDataCompareSourceFile").Enabled.ShouldBeFalse();
            GetField<RadioButtonList>("rblDataCompareOperation").Enabled.ShouldBeFalse();
            GetField<DropDownList>("drpBrand").Enabled.ShouldBeTrue();
            GetField<Label>("lblErrorMsg").Text.ShouldBeEmpty();
            GetField<Label>("lblErrorMsg").Visible.ShouldBeFalse();
        }

        [Test]
        public void LoadFilterSegmentationData_DataCompareSourceFile_LoadFilterWithField()
        {
            // Arrange
            var isGetCountsCalled = false;
            var isLoadFilterWithFieldCalled = false;
            var filterSegmentation = GetField<FilterSegmentation>("FilterSegmentation");
            filterSegmentation.Visible = false;

            var rdl = GetField<RadioButtonList>("rblLoadType");
            rdl.Items.Add("Manual Load");
            rdl.SelectedValue = "Manual Load";

            GetField<HiddenField>("hfProductID").Value = TestNumber;
            GetField<HiddenField>("hfBrandID").Value = TestNumber;
            GetField<DropDownList>("drpDataCompareSourceFile").Items.Add(TestNumber);
            GetField<RadioButtonList>("rblDataCompareOperation").Items.Add(TestNumber);
            GetField<RadioButtonList>("rblDataCompareOperation").SelectedValue = TestNumber;

            ShimFilterSegmentation.AllInstances.LoadFilterSegmenationData = _ => { };
            ShimFilter.AllInstances.GetCountsClientConnections = (_, __) => { isGetCountsCalled = true; };
            ShimMDFilter.LoadFiltersClientConnectionsInt32Int32Field = (_, __, ___, ____) =>
            {
                isLoadFilterWithFieldCalled = true;
                return _filters;
            };

            // Act
            _testEntity.LoadFilterSegmentationData(0);

            // Assert
            isLoadFilterWithFieldCalled.ShouldBeTrue();
            isGetCountsCalled.ShouldBeTrue();
            filterSegmentation.Visible.ShouldBeTrue();
            filterSegmentation.ViewType.ShouldBe(Enums.ViewType.ProductView);
            filterSegmentation.UserID.ShouldBe(1);
            filterSegmentation.PubID.ShouldBe(1);
            GetField<DropDownList>("drpDataCompareSourceFile").Enabled.ShouldBeFalse();
            GetField<RadioButtonList>("rblDataCompareOperation").Enabled.ShouldBeFalse();
            GetField<DropDownList>("drpBrand").Enabled.ShouldBeTrue();
            GetField<Label>("lblErrorMsg").Text.ShouldBeEmpty();
            GetField<Label>("lblErrorMsg").Visible.ShouldBeFalse();
        }

        [Test]
        public void LoadFilterData_LoadTypeManual_FilterGetCountsIsCalled()
        {
            // Arrange
            var filterIDs = new List<int> { 1 };
            var isGetCountsCalled = false;

            var filterSegmentation = GetField<FilterSegmentation>("FilterSegmentation");
            filterSegmentation.Visible = false;

            var rdl = GetField<RadioButtonList>("rblLoadType");
            rdl.Items.Add("Manual Load");
            rdl.SelectedValue = "Manual Load";

            GetField<HiddenField>("hfProductID").Value = TestNumber;
            GetField<HiddenField>("hfBrandID").Value = TestNumber;

            ShimFilterSegmentation.AllInstances.LoadFilterSegmenationData = _ => { };
            ShimFilter.AllInstances.GetCountsClientConnections = (_, __) => { isGetCountsCalled = true; };

            // Act
            _testEntity.LoadFilterData(filterIDs);

            // Assert
            isGetCountsCalled.ShouldBeTrue();
            filterSegmentation.Visible.ShouldBeFalse();
            GetField<DropDownList>("drpDataCompareSourceFile").Enabled.ShouldBeFalse();
            GetField<RadioButtonList>("rblDataCompareOperation").Enabled.ShouldBeFalse();
            GetField<DropDownList>("drpBrand").Enabled.ShouldBeTrue();
            GetField<Label>("lblErrorMsg").Text.ShouldBeEmpty();
            GetField<Label>("lblErrorMsg").Visible.ShouldBeFalse();
        }

        [Test]
        public void LoadFilterData_LoadTypeAuto_FilterGetCountsIsNotCalled()
        {
            // Arrange
            var filterIDs = new List<int> { 1 };
            var isGetCountsCalled = false;

            var filterSegmentation = GetField<FilterSegmentation>("FilterSegmentation");
            filterSegmentation.Visible = false;

            var rdl = GetField<RadioButtonList>("rblLoadType");
            rdl.Items.Add("Auto Load");
            rdl.SelectedValue = "Auto Load";

            GetField<HiddenField>("hfProductID").Value = TestNumber;
            GetField<HiddenField>("hfBrandID").Value = TestNumber;

            ShimFilterSegmentation.AllInstances.LoadFilterSegmenationData = _ => { };
            ShimFilter.AllInstances.GetCountsClientConnections = (_, __) => { isGetCountsCalled = true; };
            ShimFilters.AllInstances.Execute = _ => { };
            ShimFilterVennDiagram.AllInstances.CreateVennFilters = (_, __) => { };

            // Act
            _testEntity.LoadFilterData(filterIDs);

            // Assert
            isGetCountsCalled.ShouldBeFalse();
            filterSegmentation.Visible.ShouldBeFalse();
            GetField<DropDownList>("drpDataCompareSourceFile").Enabled.ShouldBeFalse();
            GetField<RadioButtonList>("rblDataCompareOperation").Enabled.ShouldBeFalse();
            GetField<DropDownList>("drpBrand").Enabled.ShouldBeTrue();
            GetField<Label>("lblErrorMsg").Text.ShouldBeEmpty();
            GetField<Label>("lblErrorMsg").Visible.ShouldBeFalse();
        }

        [Test]
        public void LoadFilterData_DataCompareSourceFile_LoadFilterWithField()
        {
            // Arrange
            var filterIDs = new List<int> { 1 };
            var isGetCountsCalled = false;
            var isLoadFilterWithFieldCalled = false;
            var filterSegmentation = GetField<FilterSegmentation>("FilterSegmentation");
            filterSegmentation.Visible = false;

            var rdl = GetField<RadioButtonList>("rblLoadType");
            rdl.Items.Add("Manual Load");
            rdl.SelectedValue = "Manual Load";

            GetField<HiddenField>("hfProductID").Value = TestNumber;
            GetField<HiddenField>("hfBrandID").Value = TestNumber;
            GetField<DropDownList>("drpDataCompareSourceFile").Items.Add(TestNumber);
            GetField<RadioButtonList>("rblDataCompareOperation").Items.Add(TestNumber);
            GetField<RadioButtonList>("rblDataCompareOperation").SelectedValue = TestNumber;

            ShimFilterSegmentation.AllInstances.LoadFilterSegmenationData = _ => { };
            ShimFilter.AllInstances.GetCountsClientConnections = (_, __) => { isGetCountsCalled = true; };
            ShimMDFilter.LoadFiltersClientConnectionsInt32Int32Field = (_, __, ___, ____) =>
            {
                isLoadFilterWithFieldCalled = true;
                return _filters;
            };

            // Act
            _testEntity.LoadFilterData(filterIDs);

            // Assert
            isLoadFilterWithFieldCalled.ShouldBeTrue();
            isGetCountsCalled.ShouldBeTrue();
            filterSegmentation.Visible.ShouldBeFalse();
            GetField<DropDownList>("drpDataCompareSourceFile").Enabled.ShouldBeFalse();
            GetField<RadioButtonList>("rblDataCompareOperation").Enabled.ShouldBeFalse();
            GetField<DropDownList>("drpBrand").Enabled.ShouldBeTrue();
            GetField<Label>("lblErrorMsg").Text.ShouldBeEmpty();
            GetField<Label>("lblErrorMsg").Visible.ShouldBeFalse();
        }

        [Test]
        public void getFilter_RadiusAndZipCodeSupplied_CorrectFilterValues()
        {
            // Arrange
            GetField<HiddenField>("hfProductID").Value = TestNumber;
            GetField<HiddenField>("hfBrandID").Value = TestNumber;
            GetField<Panel>("pnlBrand").Visible = false;
            GetField<TelerikUI.RadComboBox>("rcbProduct").Items.Add(string.Empty);
            GetField<TelerikUI.RadComboBox>("rcbProduct").SelectedValue = string.Empty;
            GetField<TextBox>("txtRadiusMin").Text = "1";
            GetField<TextBox>("txtRadiusMax").Text = "2";

            ShimResponseGroup.GetActiveByPubIDClientConnectionsInt32 = (_, __) => null;
            ShimActivity.AllInstances.GetActivityFilters = _ => new List<Field>();
            ShimCirculation.AllInstances.GetCirculationFilters = _ => new List<Field>();
            ShimRadMaskedTextBox.AllInstances.TextWithLiteralsGet = _ => TestZipCode;
            FrameworkUAD.Object.Fakes.ShimLocation.ValidateBingAddressLocationString = (_, __) => new FrameworkUAD.Object.Location() { Longitude = 1, Latitude = 2, IsValid = true };
            ShimMasterGroup.GetSearchEnabledByBrandIDClientConnectionsInt32 = (_, __) => null;

            // Act
            var result = PrivatePage.Invoke("getFilter", new Type[0], new object[0]) as Filter;

            // Assert
            result.ShouldNotBeNull();
            var field = result.Fields.FirstOrDefault(f => f.Name == "Zipcode-Radius");
            field.ShouldSatisfyAllConditions(
                () => field.ShouldNotBeNull(),
                () => field.Values.ShouldBe(ExpectedZipCodeRadiusValues));
        }

        [Test]
        public void Page_Load_ZeroBrands_HideAll()
        {
            // Arrange
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>();
            ShimBrand.GetAllClientConnections = _ => new List<Brand>();

            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Panel>("pnlBrand").Visible.ShouldBeFalse(),
                () => GetField<DropDownList>("drpBrand").Visible.ShouldBeFalse(),
                () => GetField<Label>("lblBrandName").Visible.ShouldBeFalse());
        }

        [Test]
        public void Page_Load_HasAccessIsFalse_ShowBrandDropDown()
        {
            // Arrange
            var brandDropDown = GetField<DropDownList>(DrpBrand);
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>();
            ShimBrand.GetAllClientConnections = _ => new List<Brand>
            {
                new Brand {BrandID = 1, BrandName = DummyString}
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => false;
            ConfigurationManager.AppSettings[ManualLoadClientIds] = TestZero;

            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<LinkButton>(LnkSavedFilter).Visible.ShouldBeFalse(),
                () => brandDropDown.Visible.ShouldBeTrue(),
                () => brandDropDown.Items.Count.ShouldBe(2),
                () => brandDropDown.Items[0].Text.ShouldBe(AllProducts),
                () => GetField<Button>(BtnOpenSaveFilterPopup).Visible.ShouldBeFalse(),
                () => GetField<Button>(BtnLoadComboVenn).Visible.ShouldBeTrue()
            );
        }

        [Test]
        [TestCase(ManualLoad, DefaultAdHocText)]
        [TestCase(AutoLoad, DummyString)]
        public void Page_Load_OnPostBack_AddFilter(string rblLoadType, string labelFilterName)
        {
            // Arrange
            ShimFilters.AllInstances.Execute = _ => { };
            ShimPage.AllInstances.IsPostBackGet = _ => true;
            var radioButtonList = new RadioButtonList();
            radioButtonList.Items.Add(new ListItem(DummyString, rblLoadType));
            radioButtonList.SelectedIndex = 0;
            PrivatePage.SetField(RadioButtonListLoadType, radioButtonList);
            ShimGridView.AllInstances.RowsGet = (x) =>
            {
                var gridViewRow = CreateGridViewRow();
                var arrayList = new ArrayList
                {
                    gridViewRow
                };
                return new GridViewRowCollection(arrayList);
            };
            ShimGridView.AllInstances.DataKeysGet = (sender) =>
            {
                var orderDictionary = new OrderedDictionary { { FilterNo, 1 } };
                var dataKey = new System.Web.UI.WebControls.DataKey(orderDictionary);
                var keys = new ArrayList
                {
                    dataKey
                };
                return new DataKeyArray(keys);
            };
            ShimControl.AllInstances.FindControlString = (sender, controlId) =>
            {
                if (controlId.Equals(LabelFilterName))
                {
                    return new Label { ID = controlId, Text = labelFilterName };
                }
                return GetControlById(controlId);
            };
            var drpDataCompareSourceFile = GetField<DropDownList>(DrpDataCompareSourceFile);
            drpDataCompareSourceFile.Items.Add(new ListItem(DummyString, DummyString));
            drpDataCompareSourceFile.SelectedIndex = 0;
            var rblDataCompareOperation = GetField<RadioButtonList>(RblDataCompareOperation);
            rblDataCompareOperation.Items.Add(new ListItem(DummyString, DummyString));
            rblDataCompareOperation.SelectedIndex = 0;
            ShimDataFunctions.GetClientSqlConnectionClientConnections = connections => new ShimSqlConnection();
            ShimFilter.AllInstances.GetCountsClientConnections = (_, __) => { };

            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);
            var filterConditions = _testEntity.FilterCollection;

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => filterConditions.ShouldNotBeNull(),
                () => filterConditions.ShouldNotBeEmpty()
            );
        }

        [Test]
        public void Page_Load_OneBrandAssigned_ShowBrandLabel()
        {
            // Arrange
            var brandLabel = GetField<Label>("lblBrandName");
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>
            {
                new Brand {BrandID = 1, BrandName = "test"}
            };
            ShimBrand.GetAllClientConnections = _ => new List<Brand>();

            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Panel>("pnlBrand").Visible.ShouldBeTrue(),
                () => GetField<DropDownList>("drpBrand").Visible.ShouldBeFalse(),
                () => brandLabel.Visible.ShouldBeTrue(),
                () => brandLabel.Text.ShouldBe("test"),
                () => GetField<HiddenField>("hfBrandID").Value.ShouldBe(TestOne));
        }

        [Test]
        public void Page_Load_OneBrandNotAssigned_ShowBrandDropDown()
        {
            // Arrange
            var brandDropDown = GetField<DropDownList>("drpBrand");
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>();
            ShimBrand.GetAllClientConnections = _ => new List<Brand>
            {
                new Brand {BrandID = 1, BrandName = "test"}
            };

            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Panel>("pnlBrand").Visible.ShouldBeTrue(),
                () => brandDropDown.Visible.ShouldBeTrue(),
                () => brandDropDown.Items.Count.ShouldBe(2),
                () => brandDropDown.Items[0].Text.ShouldBe(AllProducts),
                () => GetField<Label>("lblBrandName").Visible.ShouldBeFalse());
        }

        [Test]
        public void Page_Load_TwoBrandsAssigned_ShowBrandDropDown()
        {
            // Arrange
            var brandDropDown = GetField<DropDownList>("drpBrand");
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>
            {
                new Brand {BrandID = 1, BrandName = "test"},
                new Brand {BrandID = 2, BrandName = "test2"}
            };
            ShimBrand.GetAllClientConnections = _ => new List<Brand>();

            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Panel>("pnlBrand").Visible.ShouldBeTrue(),
                () => brandDropDown.Visible.ShouldBeTrue(),
                () => brandDropDown.Items.Count.ShouldBe(3),
                () => brandDropDown.Items[0].Text.ShouldBe(string.Empty),
                () => GetField<Label>("lblBrandName").Visible.ShouldBeFalse());
        }

        [Test]
        public void Page_Load_TwoBrandsNotAssigned_ShowBrandDropDown()
        {
            // Arrange
            var brandDropDown = GetField<DropDownList>("drpBrand");
            ShimBrand.GetByUserIDClientConnectionsInt32 = (_, __) => new List<Brand>();
            ShimBrand.GetAllClientConnections = _ => new List<Brand>
            {
                new Brand {BrandID = 1, BrandName = "test"},
                new Brand {BrandID = 2, BrandName = "test2"}
            };

            // Act
            PrivatePage.Invoke(MethodPageLoad, this, EventArgs.Empty);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Panel>("pnlBrand").Visible.ShouldBeTrue(),
                () => brandDropDown.Visible.ShouldBeTrue(),
                () => brandDropDown.Items.Count.ShouldBe(3),
                () => brandDropDown.Items[0].Text.ShouldBe(AllProducts),
                () => GetField<Label>("lblBrandName").Visible.ShouldBeFalse());
        }

        [Test]
        public void SelectStateOnRegion_GetByRegionReturns2And3_States2And3AreSelectedInListBox()
        {
            // Arrange
            var geo = GetField<ListBox>(GeoList);
            var states = GetField<ListBox>(StateList);

            geo.Items.Add("1");
            geo.Items[0].Selected = true;

            states.Items.Add("1");
            states.Items.Add("2");
            states.Items.Add("3");
            states.Items.Add("4");
            states.Items.Add("5");

            ShimRegion.GetByRegionGroupIDInt32 = _ => new List<Region>
            {
                new Region {RegionCode = "2"},
                new Region {RegionCode = "3"}
            };

            // Act
            PrivatePage.Invoke(MethodSelectStateOnRegion);

            // Assert
            states.ShouldSatisfyAllConditions(
                () => states.Items[0].Selected.ShouldBeFalse(),
                () => states.Items[1].Selected.ShouldBeTrue(),
                () => states.Items[2].Selected.ShouldBeTrue(),
                () => states.Items[3].Selected.ShouldBeFalse(),
                () => states.Items[4].Selected.ShouldBeFalse());
        }

        [Test]
        public void SelectCountryOnRegion_GetByAreaReturns2And3_Countries2And3AreSelectedInListBox()
        {
            // Arrange
            var regions = GetField<ListBox>(CountryRegionsList);
            var countries = GetField<ListBox>(CountryList);

            regions.Items.Add("1");
            regions.Items[0].Selected = true;

            countries.Items.Add("1");
            countries.Items.Add("2");
            countries.Items.Add("3");
            countries.Items.Add("4");
            countries.Items.Add("5");

            ShimCountry.GetByAreaString = _ => new List<Country>
            {
                new Country {CountryID = 2},
                new Country {CountryID = 3}
            };

            // Act
            PrivatePage.Invoke(MethodSelectCountryOnRegion);

            // Assert
            countries.ShouldSatisfyAllConditions(
                () => countries.Items[0].Selected.ShouldBeFalse(),
                () => countries.Items[1].Selected.ShouldBeTrue(),
                () => countries.Items[2].Selected.ShouldBeTrue(),
                () => countries.Items[3].Selected.ShouldBeFalse(),
                () => countries.Items[4].Selected.ShouldBeFalse());
        }

        [TestCase("Permission", "-1", "MAIL")]
        [TestCase("Permission", "-1", "EMAIL")]
        [TestCase("Permission", "-1", "PHONE")]
        [TestCase("Permission", "-1", "MAIL_PHONE")]
        [TestCase("Permission", "-1", "EMAIL_PHONE")]
        [TestCase("Permission", "-1", "MAIL_EMAIL")]
        [TestCase("Permission", "-1", "ALL_RECORDS")]
        [TestCase("Permission", "1", " ")]
        public void GetAddlFilter_PermissionTabIsEnabled_ReturnsResultObject(string permission, string codeSheetID, string premissionType)
        {
            // Arrange
            var lblPermission = GetField<Label>(LabelPermission);
            lblPermission.Text = permission;
            var lblCodeSheetID = GetField<Label>(LabelCodeSheetID);
            lblCodeSheetID.Text = codeSheetID;
            var lblPremissionType = GetField<Label>(LabelPremissionType);
            lblPremissionType.Text = premissionType;
            CreatePageShimObject();

            // Act
            var result = PrivatePage.Invoke(GetAddlFilter) as string;

            // Assert
            result.ShouldNotBeNullOrEmpty();
        }

        [TestCase("CrossTab", "Zip", "Grand Total|Grand Total", "A|B")]
        [TestCase("CrossTab", "State", "Grand Total|Grand Total", "")]
        [TestCase("CrossTab", "Country", "Grand Total|Grand Total", "")]
        [TestCase("CrossTab", "Title", "Grand Total|Grand Total", "")]
        [TestCase("CrossTab", "Title", "Grand Total|Grand Total", "A_B,B_C,C_D")]
        [TestCase("CrossTab", "Zip", "ZZZ. NO RESPONSE|ZZZ. NO RESPONSE", "A|B")]
        [TestCase("CrossTab", "State", "ZZZ. NO RESPONSE|ZZZ. NO RESPONSE", "")]
        [TestCase("CrossTab", "Country", "ZZZ. NO RESPONSE|ZZZ. NO RESPONSE", "")]
        [TestCase("CrossTab", "Title", "ZZZ. NO RESPONSE|ZZZ. NO RESPONSE", "")]
        [TestCase("CrossTab", "Title", "ZZZ. NO RESPONSE|ZZZ. NO RESPONSE", "A_B,B_C,C_D")]
        [TestCase("CrossTab", "Country", "Country|Country", "Test|Test")]
        [TestCase("CrossTab", "State", "State|State", "Test|Test")]
        [TestCase("CrossTab", "DefaultCase", "ZZZ. NO RESPONSE|ZZZ. NO RESPONSE", "A_B,B_C,C_D")]
        [TestCase("CrossTab", "DefaultCase", "ZZZ. NO RESPONSE|ZZZ. NO RESPONSE", "")]
        [TestCase("CrossTab", "DefaultCase", "Grand Total|Grand Total", "A_B,B_C,C_D")]
        [TestCase("CrossTab", "DefaultCase", "Test|Test", "Test|Test")]
        public void GetAddlFilter_CrossTabIsEnabled_ReturnsResultObject(string permission, string hfRow, string hfSelectedCrossTabLink, string hfRowSearchValue)
        {
            // Arrange 
            var lblPermission = GetField<Label>(LabelPermission);
            lblPermission.Text = permission;
            var hiddenFiledRow = GetField<HiddenField>(HiddenFiledRow);
            hiddenFiledRow.Value = hfRow;
            var hiddenFiledColumn = GetField<HiddenField>(HiddenFiledColumn);
            hiddenFiledColumn.Value = hfRow;
            var hiddenSelectedCrossTabLink = GetField<HiddenField>(HiddenSelectedCrossTabLink);
            hiddenSelectedCrossTabLink.Value = hfSelectedCrossTabLink;
            var hiddenFiledRowSearchValue = GetField<HiddenField>(HiddenFiledRowSearchValue);
            hiddenFiledRowSearchValue.Value = hfRowSearchValue;
            var hfColumnSearchValue = GetField<HiddenField>(HiddenFieldColumnSearchValue);
            hfColumnSearchValue.Value = hfRowSearchValue;
            var drpCrossTabReport = GetField<DropDownList>(DropDownCrossTabReport);
            drpCrossTabReport.Items.Add(new ListItem("1", "1"));
            drpCrossTabReport.Items.Add(new ListItem("2", "2"));
            drpCrossTabReport.Items.Add(new ListItem("3", "3"));
            drpCrossTabReport.Items.Add(new ListItem("4", "4"));
            drpCrossTabReport.DataBind();
            drpCrossTabReport.SelectedIndex = 1;
            CreatePageShimObject();

            // Act
            var result = PrivatePage.Invoke(GetAddlFilter) as string;

            // Assert
            result.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void GetFilter_ByPageControlObject_ReturnsFilterObject()
        {
            // Arrange
            GetField<Panel>(PnlBrand).Visible = true;
            var drpBrand = GetField<DropDownList>(DrpBrand);
            drpBrand.Visible = true;
            drpBrand.Items.Add(new ListItem(DummyString, TestOne));
            drpBrand.SelectedIndex = 0;
            GetField<HiddenField>(HiddenFiledBrandID).Value = TestOne;
            GetField<TextBox>(TextRadiusMin).Text = TestZero;
            GetField<TextBox>(TextRadiusMax).Text = TestOne;
            BindRadComboBoxForGetFilter();
            CheckAllComboBoxItem();
            SetupShimsForGetFilter();

            // Act
            var filter = PrivatePage.Invoke(GetFilter) as Filter;

            // Assert
            filter.ShouldSatisfyAllConditions(
                () => filter.ShouldNotBeNull(),
                () => filter.Fields.ShouldNotBeNull(),
                () => filter.Fields.Any().ShouldBeTrue(),
                () => filter.Fields.ShouldNotBeEmpty(),
                () => filter.Executed.ShouldBeFalse(),
                () => filter.FilterGroupID.ShouldBe(0),
                () => filter.FilterGroupName.ShouldBeEmpty(),
                () => filter.FilterNo.ShouldBe(0),
                () => filter.Count.ShouldBe(0),
                () => filter.FilterName.ShouldBeNullOrEmpty(),
                () => filter.Fields.Exists(x => x.Name.Equals(Product)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(State)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(Country)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(Email)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(Phone)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(Fax)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(Media)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(GeoLocated)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(MailPermission)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(FaxPermission)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(PhonePermission)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(OthersProductsPermission)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(ThirdPartyPermission)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(EmailRenewPermission)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(TxtPermission)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(EmailStatus)).ShouldBeTrue(),
                () => filter.Fields.Exists(x => x.Name.Equals(ZipcodeRadius)).ShouldBeTrue()
            );
        }

        [Test]
        public void BtnOpenSaveFilterPopupClick_FalseIsChecked_ReturnsError()
        {
            // Arrange
            GetField<Label>(LblErrorMsg).Text = string.Empty;
            GetField<HtmlGenericControl>(DivErrorMsg).Visible = false;
            var service = Services.CIRCFILEMAPPER;
            var serviceFeatures = ServiceFeatures.ABSummaryReport;
            var access = KMPlatform.Enums.Access.AddEmails;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess =
                (_, serviceParameter, serviceFeaturesParameter, accessParameter) =>
                {
                    service = serviceParameter;
                    serviceFeatures = serviceFeaturesParameter;
                    access = accessParameter;
                    return true;
                };

            // Act
            var parameters = new object[] { null, EventArgs.Empty };
            PrivatePage.Invoke(BtnOpenSaveFilterPopupClick, parameters);

            // Assert
            service.ShouldSatisfyAllConditions(
                () => service.ShouldBe(Services.UAD),
                () => serviceFeatures.ShouldBe(ServiceFeatures.UADFilter),
                () => access.ShouldBe(KMPlatform.Enums.Access.Edit),
                () => GetField<Label>(LblErrorMsg).Text.ShouldBe("Please select a checkbox."),
                () => GetField<HtmlGenericControl>(DivErrorMsg).Visible.ShouldBeTrue());
        }

        [Test]
        public void BtnOpenSaveFilterPopupClick_TrueIsChecked_UpdateFilterSaveProperties()
        {
            // Arrange
            GetField<Label>(LblErrorMsg).Text = string.Empty;
            GetField<HtmlGenericControl>(DivErrorMsg).Visible = false;
            GetField<HiddenField>(HiddenFiledBrandID).Value = TestOne;
            GetField<HiddenField>(HfProductID).Value = TestOne;
            SetupShimsForBtnOpenSaveFilterPopupClick();
            var service = Services.CIRCFILEMAPPER;
            var serviceFeatures = ServiceFeatures.ABSummaryReport;
            var access = KMPlatform.Enums.Access.AddEmails;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess =
                (_, serviceParameter, serviceFeaturesParameter, accessParameter) =>
                {
                    service = serviceParameter;
                    serviceFeatures = serviceFeaturesParameter;
                    access = accessParameter;
                    return true;
                };

            // Act
            var parameters = new object[] { null, EventArgs.Empty };
            PrivatePage.Invoke(BtnOpenSaveFilterPopupClick, parameters);

            // Assert
            var filterSave = GetField<FilterPanel>(FilterSaveControlName);
            filterSave.ShouldSatisfyAllConditions(
                () => filterSave.Mode.ShouldBe(AddNewFilterMode),
                () => filterSave.BrandID.ShouldBe(1),
                () => filterSave.PubID.ShouldBe(1),
                () => filterSave.FilterIDs.ShouldBe(string.Format("{0},{1}", TestZero, TestOne)),
                () => filterSave.UserID.ShouldBe(1),
                () => filterSave.FilterCollections.ShouldBeEmpty(),
                () => filterSave.ViewType.ShouldBe(Enums.ViewType.ProductView),
                () => filterSave.Visible.ShouldBeTrue(),
                () => service.ShouldBe(Services.UAD),
                () => serviceFeatures.ShouldBe(ServiceFeatures.UADFilter),
                () => access.ShouldBe(KMPlatform.Enums.Access.Edit),
                () => GetField<Label>(LblErrorMsg).Text.ShouldBeEmpty(),
                () => GetField<HtmlGenericControl>(DivErrorMsg).Visible.ShouldBeFalse());
        }

        [Test]
        public void DrpCountrySelectedIndexChanged_UnitedStates_5HashesMask()
        {
            // Arrange
            GetField<TelerikUI.RadMaskedTextBox>(RadMtxtboxZipCode).Text = string.Empty;
            SetSelectedValue(DrpCountry, "United States");

            // Act
            var parameters = new object[] { null, EventArgs.Empty };
            PrivatePage.Invoke(DrpCountrySelectedIndexChanged, parameters);

            // Assert
            var radMaskedTextBox = GetField<TelerikUI.RadMaskedTextBox>(RadMtxtboxZipCode);
            radMaskedTextBox.ShouldSatisfyAllConditions(
                () => radMaskedTextBox.Text.ShouldBeEmpty(),
                () => radMaskedTextBox.Mask.ShouldBe("#####"));
        }

        [Test]
        public void DrpCountrySelectedIndexChanged_NotUnitedStates_LHashesMask()
        {
            // Arrange
            GetField<TelerikUI.RadMaskedTextBox>(RadMtxtboxZipCode).Text = string.Empty;
            SetSelectedValue(DrpCountry, DummyString);

            // Act
            var parameters = new object[] { null, EventArgs.Empty };
            PrivatePage.Invoke(DrpCountrySelectedIndexChanged, parameters);

            // Assert
            var radMaskedTextBox = GetField<TelerikUI.RadMaskedTextBox>(RadMtxtboxZipCode);
            radMaskedTextBox.ShouldSatisfyAllConditions(
                () => radMaskedTextBox.Text.ShouldBeEmpty(),
                () => radMaskedTextBox.Mask.ShouldBe("L#L #L#"));
        }

        private void SetSelectedValue(string dropDownName, string selectedValue)
        {
            var dropDownControl = GetField<DropDownList>(dropDownName);
            dropDownControl.Items.Add(selectedValue);
            dropDownControl.SelectedValue = selectedValue;
        }

        private void SetupShimsForBtnOpenSaveFilterPopupClick()
        {
            ShimGridView.AllInstances.RowsGet = (_) =>
            {
                return new GridViewRowCollection(new ArrayList(
                    new GridViewRow[]
                    {
                        new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal),
                        new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal)
                    }));
            };
            ShimGridView.AllInstances.DataKeysGet = (_) =>
            {
                return new DataKeyArray(
                    new ArrayList(
                        new DataKey[]
                        {
                            new DataKey(
                                new OrderedDictionary()
                                {
                                    [FilterNoDataKey] = TestZero,
                                },
                                new string[] { FilterNoDataKey }),
                            new DataKey(
                                new OrderedDictionary()
                                {
                                    [FilterNoDataKey] = TestOne,
                                },
                                new string[] { FilterNoDataKey })
                        }));
            };
            ShimControl.AllInstances.FindControlString = (_, __) => CheckBoxControl;
            ShimFilterPanel.AllInstances.LoadControls = (_) => { };
        }

        private void BindRadComboBoxForGetFilter()
        {
            var radComboBoxItemCollection = new[] { "1", "2", "3" };
            var comboBoxIdList = new[]
            {
                "rcbEmail", "rcbPhone", "rcbFax", "rcbMedia",
                "rcbMailPermission", "rcbFaxPermission", "rcbPhonePermission", "rcbOtherProductsPermission",
                "rcbThirdPartyPermission", "rcbEmailRenewPermission", "rcbTextPermission", "rcbEmailStatus", "rcbProduct"
            };
            foreach (var item in comboBoxIdList)
            {
                var radComboBox = GetField<TelerikUI.RadComboBox>(item);
                radComboBox.DataSource = radComboBoxItemCollection;
                radComboBox.DataBind();
                radComboBox.SelectedIndex = 0;
            }
        }

        private static DataListItemCollection CreateDataListItemCollection()
        {
            var arrayList = new ArrayList()
            {
                new DataListItem(0, ListItemType.Item),
                new DataListItem(1, ListItemType.Item)
            };
            return new DataListItemCollection(arrayList);
        }

        private void SetupShimsForGetFilter()
        {
            GetField<HiddenField>(HfProductID).Value = TestNumber;
            var rblDataCompareOperation = GetField<RadioButtonList>(RblDataCompareOperation);
            rblDataCompareOperation.Items.Add(new ListItem(DummyString, DummyString));
            rblDataCompareOperation.SelectedIndex = 0;
            var drpDataCompareSourceFile = GetField<DropDownList>(DrpDataCompareSourceFile);
            drpDataCompareSourceFile.Items.Add(new ListItem(DummyString, DummyString));
            drpDataCompareSourceFile.SelectedIndex = 0;
            GetField<TelerikUI.RadMaskedTextBox>(RadMtxtboxZipCode).TextWithLiterals = DummyString;
            var gridViewRow = CreateGridViewRow();
            var pubList = CreatePubsListObject();
            ShimRadMaskedTextBox.AllInstances.TextWithLiteralsGet = _ => TestZipCode;
            ShimProductView.AllInstances.ResetAllFilterControls = (sender) => { };
            ShimPubs.GetSearchEnabledByBrandIDClientConnectionsInt32 = (x, y) => pubList;
            ShimPubs.GetSearchEnabledClientConnections = (x) => pubList;
            ShimControl.AllInstances.NamingContainerGet = (sender) => gridViewRow;
            ShimUtilities.SelectFilterListBoxesListBoxString = (x, y) => { };
            ShimDataList.AllInstances.ItemsGet = (sender) => CreateDataListItemCollection();
            ShimControl.AllInstances.FindControlString = (sender, controlId) =>
            {
                if (controlId.Equals(LabelResponseGroup))
                {
                    var control = GetControlById(controlId) as Label;
                    control.Text = DummyString;
                    return control;
                }
                if (controlId.Equals(ListResponse))
                {
                    var control = GetControlById(controlId) as ListBox;
                    control.Text = DummyString;
                    control.Items.Add(new ListItem(DummyString, DummyString));
                    control.Items[0].Selected = true;
                    return control;
                }
                return GetControlById(controlId);
            };
            ShimUtilities.getListboxSelectedValuesListBox = (lstResponse) => TestOne;
            ShimProductView.AllInstances.SelectStateOnRegion = (sender) => { };
            ShimUtilities.getRadComboBoxSelectedExportFieldsRadComboBox =
                (resultList) => new Tuple<string, string>(TestOne, TestZero);
            UADBusinessLogicFakes::ShimFilterMVC.CalculateZipCodeRadiusInt32Int32StringStringDoubleOutDoubleOut =
                (int radiusMin, int radiusMax, string zipCode, string country, out double locationLat,
                    out double locationLon) =>
                {
                    locationLat = 20;
                    locationLon = 30;
                    return new double[] { 100, 200 };
                };
            ShimAdhoc.AllInstances.GetAdhocFilters = (sender) => CreateFieldListObject();
            ShimActivity.AllInstances.GetActivityFilters = (sender) => CreateFieldListObject();
            ShimCirculation.AllInstances.GetCirculationFilters = (sender) => CreateFieldListObject();
            ShimMasterGroup.GetSearchEnabledByBrandIDClientConnectionsInt32 =
                (clientconnections, id) => CreateMasterGroupList();
            ShimMasterGroup.GetSearchEnabledClientConnections = (clientconnections) => CreateMasterGroupList();
            ShimResponseGroup.GetActiveByPubIDClientConnectionsInt32 = (_, __) => new List<ResponseGroup>
            {
                new ResponseGroup
                {
                    DisplayName = DummyString
                }
            };
        }

        private void CheckAllComboBoxItem()
        {
            var comboBoxIdList = GetComboBoxIdList();
            foreach (var items in comboBoxIdList)
            {
                var radComboBox = GetField<Telerik.Web.UI.RadComboBox>(items.ToString());
                foreach (Telerik.Web.UI.RadComboBoxItem item in radComboBox.Items)
                {
                    item.Checked = true;
                }
            }
        }

        private static IEnumerable GetComboBoxIdList()
        {
            return new[]
            {
                "rcbEmail",
                "rcbPhone",
                "rcbFax",
                "rcbMedia",
                "rcbMailPermission",
                "rcbFaxPermission",
                "rcbPhonePermission",
                "rcbOtherProductsPermission",
                "rcbThirdPartyPermission",
                "rcbEmailRenewPermission",
                "rcbTextPermission",
                "rcbEmailStatus"
            };
        }

        private List<MasterGroup> CreateMasterGroupList()
        {
            return new List<MasterGroup>
            {
                new MasterGroup
                {
                    DisplayName = Test,
                    IsActive = true,
                    Name = TestOne,
                    MasterGroupID = 1,
                    ColumnReference = Test
                }
            };
        }

        private List<Field> CreateFieldListObject()
        {
            return new List<Field>
            {
                new Field { Values = FiledValue, FilterType = Enums.FiltersType.Product },
                new Field { Values = FiledValue, Group = Test, FilterType = Enums.FiltersType.Dimension },
                new Field { Values = FiledValue, FilterType = Enums.FiltersType.Activity },
                new Field { Values = FiledValue, FilterType = Enums.FiltersType.Adhoc },
                new Field { Values = FiledValue, FilterType = Enums.FiltersType.Circulation },
                new Field { Values = FiledValue, SearchCondition = GeoSearchConditionWithValidValue, FilterType = Enums.FiltersType.Geo },
                new Field { Values = FiledValue, SearchCondition = GeoSearchConditionWithInValidValue, FilterType = Enums.FiltersType.Geo },
                new Field { Values = FiledValue, Name = State, FilterType = Enums.FiltersType.Standard },
                new Field { Values = FiledValue, Name = Country, FilterType = Enums.FiltersType.Standard },
                new Field { Values = FiledValue, Name = Email, FilterType = Enums.FiltersType.Standard },
                new Field { Values = FiledValue, Name = Phone, FilterType = Enums.FiltersType.Standard },
                new Field { Values = FiledValue, Name = Fax, FilterType = Enums.FiltersType.Standard },
                new Field { Values = FiledValue, Name = Media, FilterType = Enums.FiltersType.Standard },
                new Field { Values = FiledValue, Name = GeoLocated, FilterType = Enums.FiltersType.Standard },
                new Field { Values = FiledValue, Name = MailPermission, FilterType = Enums.FiltersType.Standard },
                new Field { Values = FiledValue, Name = FaxPermission, FilterType = Enums.FiltersType.Standard },
                new Field { Values = FiledValue, Name = PhonePermission, FilterType = Enums.FiltersType.Standard },
                new Field { Values = FiledValue, Name = OthersProductsPermission, FilterType = Enums.FiltersType.Standard },
                new Field { Values = FiledValue, Name = ThirdPartyPermission, FilterType = Enums.FiltersType.Standard },
                new Field { Values = FiledValue, Name = EmailRenewPermission, FilterType = Enums.FiltersType.Standard },
                new Field { Values = FiledValue, Name = TxtPermission, FilterType = Enums.FiltersType.Standard },
                new Field { Values = FiledValue, Name = EmailStatus, FilterType = Enums.FiltersType.Standard },
            };
        }

        private static List<Pubs> CreatePubsListObject()
        {
            var pubList = new List<Pubs>();
            pubList.Add(new Pubs
            {
                PubID = 1,
                PubName = Test,
                PubCode = Test,
                PubTypeID = 1,
                EnableSearching = true
            });
            return pubList;
        }

        private static GridViewRow CreateGridViewRow()
        {
            var gridViewRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
            var cell1 = new TableCell();
            var cell2 = new TableCell();
            cell1.Text = string.Empty;
            cell2.Text = string.Empty;
            gridViewRow.Cells.Add(cell1);
            gridViewRow.Cells.Add(cell2);
            gridViewRow.Cells[0].Controls.Add(new LinkButton { ID = LinkButtonCancel, Visible = true });
            gridViewRow.Cells[1].Controls.Add(new LinkButton { ID = LinkButtonEdit, Visible = true });
            return gridViewRow;
        }

        private static Control GetControlById(string controlId)
        {
            switch (controlId)
            {
                case DummyString:
                case LstMarket:
                case CountryRegions:
                case LstResponse:
                    return new ListBox {ID = controlId};
                case LabelResponseGroup:
                    return new Label {ID = controlId};
                case GridViewFilterValues:
                    return new GridView {ID = controlId};
                case Linkdownload:
                    return new LinkButton {ID = controlId, Text = TestOne};
                case LabelBrandID:
                    return new Label {ID = controlId, Text = TestOne};
                case LabelFilterName:
                    return new Label {ID = controlId, Text = DefaultText};
                case HiddenFielsFilterGroupName:
                    return new HiddenField {ID = controlId, Value = DefaultText};
                case HiddenFieldViewType:
                    return new HiddenField {ID = controlId, Value = Enums.ViewType.ConsensusView.ToString()};
                case LabelFilterText:
                    return new Label {ID = controlId, Text = DefaultText};
                case LabelFilterValues:
                    return new Label {ID = controlId, Text = DefaultText};
                case LabelSearchCondition:
                    return new Label {ID = controlId, Text = DefaultText};
                case LabelFilterType:
                    return new Label {ID = controlId, Text = Enums.FiltersType.Activity.ToString()};
                case LabelGroup:
                    return new Label {ID = controlId, Text = DefaultText};
                case LinkButtonCancel:
                    return new LinkButton {ID = controlId, Visible = true};
                case LinkButtonEdit:
                    return new LinkButton { ID = controlId };
                case HfResponseGroupID:
                    return new HiddenField { ID = controlId, Value = TestOne };
                case LnkDimensionShowHide:
                    return new LinkButton { ID = controlId, Text = LnkDimensionShowHideText };
                case PnlDimBody:
                    return new Panel { ID = controlId };
                default:
                    return new Control {ID = controlId};
            }
        }

        private void CreatePageShimObject()
        {
            ShimProductView.AllInstances.MasterGet = (x) =>
            {
                MasterPages.Site site = new ShimSite
                {
                    clientconnectionsGet = () =>
                    {
                        return new KMPlatform.Object.ClientConnections
                        {
                            ClientLiveDBConnectionString = string.Empty,
                            ClientTestDBConnectionString = string.Empty
                        };
                    }
                };
                return site;
            };
            ObjShimKmps.ShimDataFunctions.GetClientSqlConnectionClientConnections = (x) => { return new System.Data.SqlClient.SqlConnection(); };
        }
    }
}
