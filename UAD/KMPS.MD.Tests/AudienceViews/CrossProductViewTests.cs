using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Fakes;
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
using TelerikUI = Telerik.Web.UI;
using TestCommonHelpers;
using static KMPlatform.Enums;
using FilterCategory = KMPS.MD.Objects.FilterCategory;
using ShimAdhoc = KMPS.MD.Controls.Fakes.ShimAdhoc;
using ShimDataFunctions = FrameworkUAD.DataAccess.Fakes.ShimDataFunctions;
using ShimFilterCategory = KMPS.MD.Objects.Fakes.ShimFilterCategory;
using ShimFrameworkUad = FrameworkUAD_Lookup.BusinessLogic.Fakes;
using UADBusinessLogicFakes = FrameworkUAD.BusinessLogic.Fakes;
using ShimFrameworkUas = FrameworkUAS.BusinessLogic.Fakes;
using ShimUser = KM.Platform.Fakes.ShimUser;
using ShimDataFunctionsKMPS =  KMPS.MD.Objects.Fakes.ShimDataFunctions;

namespace KMPS.MD.Tests.AudienceViews
{
    /// <summary>
    /// Unit test for <see cref="CrossProductView"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CrossProductViewTests : BasePageTests
    {
        private const string DummyString = "DummyString";
        private const string TestZero = "0";
        private const string TestOne = "1";
        private const string AllProducts = "All Products";
        private const string MethodPageLoad = "Page_Load";
        private const string MethodSelectStateOnRegion = "SelectStateOnRegion";
        private const string MethodSelectCountryOnRegion = "SelectCountryOnRegion";
        private const string TestNumber = "1";
        private const string TestZipCode = "NY12221";
        private const string ExpectedZipCodeRadiusValues = "1.97101449275362|2.02898550724638|0.970996308707783|1.02900266654272|1.98550724637681|2.01449275362319|0.98549828384275|1.01450145996992";
        private const string CountryRegionsList = "lstCountryRegions";
        private const string CountryList = "lstCountry";
        private const string GeoList = "lstGeoCode";
        private const string StateList = "lstState";
        private const string GridViewFilters = "grdFilters";
        private const string PanelDataCompare = "pnlDataCompare";
        private const string Linkdownload = "lnkdownload";
        private const string LabelBrandID = "lblBrandID";
        private const string LabelFilterName = "lblFiltername";
        private const string HiddenFielsFilterGroupName = "hfFilterGroupName";
        private const string HiddenFieldViewType = "hfViewType";
        private const string ColumnDefaultName = "Column";
        private const string LabelFilterText = "lblFilterText";
        private const string LabelFilterValues = "lblFilterValues";
        private const string LabelSearchCondition = "lblSearchCondition";
        private const string LabelFilterType = "lblFilterType";
        private const string LabelGroup = "lblGroup";
        private const string DefaultAdHocText = "Adhoc";
        private const string GridViewFilterValues = "grdFilterValues";
        private const string RadioButtonListLoadType = "rblLoadType";
        private const string ManualLoad = "Manual Load";
        private const string AutoLoad = "Auto Load";
        private const string FilterNo = "FilterNo";
        private const string GridFiltersRowCommand = "grdFilters_RowCommand";
        private const string FiledValue = "1,2,3";
        private const string FilterObject = "fc";
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
        private const string GeoSearchConditionWithValidValue = "1|2|3";
        private const string GeoSearchConditionWithInValidValue = "a|2|3";
        private const string Test = "Test";
        private const string HiddenFiledFilterNo = "hfFilterNo";
        private const string HiddenFiledFilterName = "hfFilterName";
        private const string HiddenFiledFilterGroupName = "hfFilterGroupName";
        private const string LinkButtonCancel = "lnkCancel";
        private const string LinkButtonEdit = "lnkEdit";
        private const string CommandCancel = "Cancel";
        private const string CommandEdit = "Edit";
        private const string PubTypeRepeater = "PubTypeRepeater";
        private const string PanelDimBody = "pnlDimBody";
        private const string LinkDimensionShowHide = "lnkDimensionShowHide";
        private const string HiddenFiledResponseGroupID = "hfResponseGroupID";
        private const string ListResponse = "lstResponse";
        private const string LabelResponseGroup = "lblResponseGroup";
        private const string LinkPubTypeShowHide = "lnkPubTypeShowHide";
        private const string PubTypeListBox = "PubTypeListBox";
        private const string HiddenPubTypeID = "hfPubTypeID";
        private const string PanelPubTypeBody = "pnlPubTypeBody";
        private const string ShowLinkMessageText = "(Show...)";
        private const string DefaultText = "Unit Test";
        private const string PanelBrand = "pnlBrand";
        private const string GetFilter = "getFilter";
        private const string TextRadiusMin = "txtRadiusMin";
        private const string TextRadiusMax = "txtRadiusMax";
        private const string ZipcodeRadius = "Zipcode-Radius";
        private const string Product = "Product";
        private const string HfProductID = "hfProductID";
        private const string DrpBrand = "drpBrand";
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
        private const string HfBrandID = "hfBrandID";
        private const string DrpCountrySelectedIndexChanged = "drpCountry_SelectedIndexChanged";
        private const string RadMtxtboxZipCode = "RadMtxtboxZipCode";
        private const string DrpCountry = "drpCountry";
        private CrossProductView _testEntity;
        private List<Control> _tempControls = new List<Control>
        {
            new Control(),
            new Control(),
            new Control(),
            new Control()
        };

        private Filters _filters;
        private string _url = string.Empty;

        [SetUp]
        public override void SetUp()
        {
            UserId = 1;

            base.SetUp();
            _testEntity = new CrossProductView();
            InitializePage(_testEntity);

            ShimCrossProductView.AllInstances.GetSubscribersQueriesForUserControl = report => new StringBuilder(TestNumber);
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
                "fc",
                new Filters(new ShimClientConnections(), 1)
                {
                    FilterComboList = new List<FilterCombo>
                    {
                        new FilterCombo {SelectedFilterNo = "1"}
                    }
                });

            ShimDataFunctions.GetClientSqlConnectionClientConnections = connections => new ShimSqlConnection();
            FrameworkUAD.DataAccess.Fakes.ShimFilterSegmentation.SelectByIDInt32ClientConnections = (_, __) => new FrameworkUAD.Entity.FilterSegmentation();
            ShimMDFilter.LoadFiltersClientConnectionsInt32Int32 = (_, __, ___) => _filters;
            KMPlatform.BusinessLogic.Fakes.ShimClient.AllInstances.SelectInt32Boolean = (_, __, ___) => new Client();
            ShimGridView.AllInstances.DataBind = _ => { };
            ConfigurationManager.AppSettings["ManualLoad_ClientIDs"] = TestOne;
            ShimFilterCategory.GetAllClientConnections = _ => new List<FilterCategory>();
            ShimCrossProductView.AllInstances.LoadProducts = _ => { };
            ShimAudienceViewBase.AllInstances.LoadStandardFiltersListboxes = _ => { };
            ShimEmailStatus.GetAllClientConnections = _ => null;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => true;
            GetField<HiddenField>("hfBrandID").Value = TestZero;
            GetField<Panel>("pnlBrand").Visible = false;
            GetField<DropDownList>("drpBrand").Visible = false;
            GetField<Label>("lblBrandName").Visible = false;
            CheckBoxControl = new CheckBox { Checked = true };
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
            filterSegmentation.ViewType.ShouldBe(Enums.ViewType.CrossProductView);
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
            filterSegmentation.ViewType.ShouldBe(Enums.ViewType.CrossProductView);
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
            var gridViewRow = CreateGridViewRow();
            ShimGridView.AllInstances.RowsGet = (x) =>
            {
                var arrayList = new ArrayList
                {
                    gridViewRow
                };
                return new GridViewRowCollection(arrayList);
            };
            ShimGridView.AllInstances.DataKeysGet = (sender) =>
            {
                var orderDictionaly = new OrderedDictionary { { FilterNo, 1 } };
                var dataKey = new System.Web.UI.WebControls.DataKey(orderDictionaly);
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
            ShimDataFunctionsKMPS.GetClientSqlConnectionClientConnections = connections => new ShimSqlConnection();
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

        [Test]
        public void GridFiltersRowCommand_GridViewCommandEventArgsHaveCancelCommand_UpdateControlValues()
        {
            // Arrange
            ShimAudienceViewBase.AllInstances.ClearAndResetFilterTabControls = (sender) => { };
            var gridViewRow = CreateGridViewRow();
            ShimControl.AllInstances.NamingContainerGet = (sender) => { return gridViewRow; };
            ShimControl.AllInstances.FindControlString = (sender, controlId) =>
            {
                return GetControlById(controlId);
            };
            var gridViewCommandEventArgs = new GridViewCommandEventArgs(gridViewRow, gridViewRow, new CommandEventArgs(CommandCancel, TestOne));
            var parameters = new object[] { this, gridViewCommandEventArgs };

            // Act
            PrivatePage.Invoke(GridFiltersRowCommand, parameters);

            // Assert
            GetField<HiddenField>(HiddenFiledFilterNo).Value.ShouldBe(string.Empty);
            GetField<HiddenField>(HiddenFiledFilterName).Value.ShouldBe(string.Empty);
            GetField<HiddenField>(HiddenFiledFilterGroupName).Value.ShouldBe(string.Empty);
        }

        [TestCase(TestZero)]
        [TestCase(TestOne)]
        public void GridFiltersRowCommand_GridViewCommandEventArgsHaveEditCommand_UpdateControlValues(string brandId)
        {
            // Arrange
            var gridViewRow = CreateGridViewRow();
            var pubList = CreatePubsListObject();
            ShimAudienceViewBase.AllInstances.ClearAndResetFilterTabControls = (sender) => { };
            ShimCrossProductView.AllInstances.getResponseGroup = (sender) => { };
            ShimPubs.GetSearchEnabledByBrandIDClientConnectionsInt32 = (x, y) => { return pubList; };
            ShimPubs.GetSearchEnabledClientConnections = (x) => { return pubList; };
            ShimControl.AllInstances.NamingContainerGet = (sender) => { return gridViewRow; };
            ShimUtilities.SelectFilterListBoxesListBoxString = (x, y) => { };
            ShimGridView.AllInstances.RowsGet = (x) =>
            {
                var gridVRow = new GridViewRow(0, 1, DataControlRowType.DataRow, DataControlRowState.Normal);
                var arrayList = new ArrayList();
                arrayList.Add(gridViewRow);
                return new GridViewRowCollection(arrayList);
            };
            ShimRepeater.AllInstances.ItemsGet = (sender) =>
            {
                var arrayList = new ArrayList();
                arrayList.Add(new RepeaterItem(0, ListItemType.Item));
                arrayList.Add(new RepeaterItem(1, ListItemType.Item));
                return new RepeaterItemCollection(arrayList);
            };
            ShimDataList.AllInstances.ItemsGet = (sender) =>
            {
                var arrayList = new ArrayList();
                arrayList.Add(new DataListItem(0, ListItemType.Item));
                arrayList.Add(new DataListItem(1, ListItemType.Item));
                return new DataListItemCollection(arrayList);

            };
            ShimResponseGroup.GetByResponseGroupIDClientConnectionsInt32 = (sender, id) => { return new ResponseGroup { ResponseGroupName = Test }; };
            ShimCodeSheet.GetByResponseGroupIDClientConnectionsInt32 = (x, y) => { return CreateCodeSheetListObject(); };
            ShimControl.AllInstances.FindControlString = (sender, controlId) =>
            {
                return GetControlById(controlId);
            };
            ShimMasterGroup.GetByIDClientConnectionsInt32 = (x, y) => { return new MasterGroup { ColumnReference = Test }; };
            ShimActivity.AllInstances.LoadActivityFiltersField = (sender, filed) => { };
            ShimAdhoc.AllInstances.LoadAdhocFiltersField = (sender, field) => { };
            ShimAdhoc.AllInstances.LoadAdhocGrid = (sender) => { };
            ShimCirculation.AllInstances.LoadCirculationFiltersField = (sender, field) => { };
            BindRadComboBox();
            CreateFiltersObject();
            GetField<HiddenField>(HiddenFiledBrandID).Value = brandId;
            var gridViewCommandEventArgs = new GridViewCommandEventArgs(gridViewRow, gridViewRow, new CommandEventArgs(CommandEdit, TestOne));
            var parameters = new object[] { this, gridViewCommandEventArgs };
            CreatePageShimObject(true);

            // Act
            PrivatePage.Invoke(GridFiltersRowCommand, parameters);

            // Assert
            GetField<HiddenField>(HiddenFiledFilterNo).Value.ShouldBe(TestOne);
            GetField<HiddenField>(HiddenFiledFilterName).Value.ShouldBe(Test);
            GetField<HiddenField>(HiddenFiledFilterGroupName).Value.ShouldBe(Test);
        }

        [TestCase(TestOne, 86, true)]
        [TestCase(TestOne, 86, false)]
        [TestCase(TestZero, 85, true)]
        public void GetFilter_ByPageControlObject_ReturnsFilterObject(string brandId, int count, bool drpBrandVisible)
        {
            // Arrange
            GetField<Panel>(PanelBrand).Visible = true;
            var drpBrand = GetField<DropDownList>(DrpBrand);
            drpBrand.Visible = drpBrandVisible;
            drpBrand.Items.Add(new ListItem(DummyString, TestOne));
            drpBrand.SelectedIndex = 0;
            GetField<HiddenField>(HfProductID).Value = TestNumber;
            GetField<HiddenField>(HiddenFiledBrandID).Value = brandId;
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
                () => filter.Fields.Count.ShouldBe(count),
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
            GetField<HiddenField>(HfBrandID).Value = TestOne;
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
                () => filterSave.PubID.ShouldBe(0),
                () => filterSave.FilterIDs.ShouldBe(string.Format("{0},{1}", TestZero, TestOne)),
                () => filterSave.UserID.ShouldBe(1),
                () => filterSave.FilterCollections.ShouldBeEmpty(),
                () => filterSave.ViewType.ShouldBe(Enums.ViewType.CrossProductView),
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
            ShimGridView.AllInstances.RowsGet = (_) => new GridViewRowCollection(new ArrayList(
                new GridViewRow[]
                {
                    new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal),
                    new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal)
                }));

            ShimGridView.AllInstances.DataKeysGet = (_) => new DataKeyArray(
                new ArrayList(new []
                {
                    new DataKey(
                        new OrderedDictionary()
                        {
                            [FilterNoDataKey] = TestZero,
                        },
                        new [] { FilterNoDataKey }),
                    new DataKey(
                        new OrderedDictionary()
                        {
                            [FilterNoDataKey] = TestOne,
                        },
                        new [] { FilterNoDataKey })
                }));
            ShimControl.AllInstances.FindControlString = (_, __) => CheckBoxControl;
            ShimFilterPanel.AllInstances.LoadControls = (_) => { };
        }

        private void BindRadComboBoxForGetFilter()
        {
            var radComboBoxItemCollection = new[] { "1", "2", "3" };
            var comboBoxIdList = new[]
            {
                "rcbEmail", "rcbPhone", "rcbFax", "rcbMedia", "rcbIsLatLonValid",
                "rcbMailPermission", "rcbFaxPermission", "rcbPhonePermission", "rcbOtherProductsPermission",
                "rcbThirdPartyPermission", "rcbEmailRenewPermission", "rcbTextPermission", "rcbEmailStatus",
                "rcbProduct"
            };
            foreach (var item in comboBoxIdList)
            {
                var radComboBox = GetField<TelerikUI.RadComboBox>(item);
                radComboBox.DataSource = radComboBoxItemCollection;
                radComboBox.DataBind();
                radComboBox.SelectedIndex = 0;
            }
        }

        private void SetupShimsForGetFilter()
        {
            var gridViewRow = CreateGridViewRow();
            var pubList = CreatePubsListObject();
            ShimRadMaskedTextBox.AllInstances.TextWithLiteralsGet = _ => TestZipCode;
            ShimAudienceViewBase.AllInstances.ClearAndResetFilterTabControls = (sender) => { };
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
            ShimCrossProductView.AllInstances.SelectStateOnRegion = (sender) => { };
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
                var radComboBox = GetField<TelerikUI.RadComboBox>(items.ToString());
                foreach (TelerikUI.RadComboBoxItem item in radComboBox.Items)
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
                "rcbIsLatLonValid",
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

        private static DataListItemCollection CreateDataListItemCollection()
        {
            var arrayList = new ArrayList()
            {
                new DataListItem(0, ListItemType.Item),
                new DataListItem(1, ListItemType.Item)
            };
            return new DataListItemCollection(arrayList);
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

        private void CreateFiltersObject()
        {
            _filters = new Filters(new ShimClientConnections(), 1);
            _filters.FilterComboList = new List<FilterCombo>
            {
                new FilterCombo {SelectedFilterNo = TestOne},
            };

            _filters.Add(new Filter
            {
                FilterNo = 1,
                FilterName = Test,
                FilterGroupID = 1,
                FilterGroupName = Test,
                Fields = CreateFieldListObject()
            });
            ReflectionHelper.SetValue(_testEntity, FilterObject, _filters);
        }

        private static List<CodeSheet> CreateCodeSheetListObject()
        {
            var listCodeSheet = new List<CodeSheet>();
            listCodeSheet.Add(new CodeSheet
            {
                CodeSheetID = 1,
                ResponseDesc = Test,
                ResponseValue = Test
            });
            return listCodeSheet;
        }

        private void BindRadComboBox()
        {
            var radComboBoxItemCollection = new string[] { "1", "2", "3" };
            var comboBoxIdList = new string[] { "rcbEmail" , "rcbPhone" , "rcbFax", "rcbMedia" , "rcbIsLatLonValid" ,
                "rcbMailPermission" , "rcbFaxPermission" , "rcbPhonePermission" , "rcbOtherProductsPermission" ,
                "rcbThirdPartyPermission","rcbEmailRenewPermission","rcbTextPermission","rcbEmailStatus" };
            foreach (var item in comboBoxIdList)
            {
                var radComboBox = GetField<TelerikUI.RadComboBox>(item);
                radComboBox.DataSource = radComboBoxItemCollection;
                radComboBox.DataBind();
            }
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

        private Control GetControlById(string controlId)
        {
            if (controlId == GridViewFilterValues)
            {
                return new GridView { ID = controlId };
            }
            else if (controlId == Linkdownload)
            {
                return new LinkButton { ID = controlId, Text = TestOne };
            }
            else if (controlId == LabelBrandID)
            {
                return new Label { ID = controlId, Text = TestOne };
            }
            else if (controlId == LabelFilterName)
            {
                return new Label { ID = controlId, Text = DefaultText };
            }
            else if (controlId == HiddenFielsFilterGroupName)
            {
                return new HiddenField { ID = controlId, Value = DefaultText };
            }
            else if (controlId == HiddenFieldViewType)
            {
                return new HiddenField { ID = controlId, Value = Enums.ViewType.ConsensusView.ToString() };
            }
            else if (controlId == LabelFilterText)
            {
                return new Label { ID = controlId, Text = DefaultText };
            }
            else if (controlId == LabelFilterName)
            {
                return new Label { ID = controlId, Text = DefaultAdHocText };
            }
            else if (controlId == LabelFilterValues)
            {
                return new Label { ID = controlId, Text = DefaultText };
            }
            else if (controlId == LabelSearchCondition)
            {
                return new Label { ID = controlId, Text = DefaultText };
            }
            else if (controlId == LabelFilterType)
            {
                return new Label { ID = controlId, Text = Enums.FiltersType.Activity.ToString() };
            }
            else if (controlId == LabelGroup)
            {
                return new Label { ID = controlId, Text = DefaultText };
            }
            else if (controlId == LinkButtonCancel)
            {
                return new LinkButton { ID = controlId, Visible = true };
            }
            else if (controlId == LinkButtonEdit)
            {
                return new LinkButton { ID = controlId, Visible = true };
            }
            else if (controlId == PubTypeRepeater)
            {
                return new Repeater { ID = controlId };
            }
            else if (controlId == HiddenPubTypeID)
            {
                return new HiddenField { ID = controlId, Value = TestOne };
            }
            else if (controlId == PubTypeListBox)
            {
                return new ListBox { ID = controlId };
            }
            else if (controlId == LinkPubTypeShowHide)
            {
                return new LinkButton { ID = controlId, Text = ShowLinkMessageText };
            }
            else if (controlId == PanelPubTypeBody)
            {
                return new Panel { ID = controlId };
            }
            else if (controlId == ListResponse)
            {
                return new ListBox { ID = controlId };
            }
            else if (controlId == HiddenFiledResponseGroupID)
            {
                return new HiddenField { ID = controlId, Value = TestOne };
            }
            else if (controlId == LinkDimensionShowHide)
            {
                return new LinkButton { ID = controlId, Text = ShowLinkMessageText };
            }
            else if (controlId == PanelDimBody)
            {
                return new Panel { ID = controlId };
            }
            else if (controlId == LabelResponseGroup)
            {
                return new Label { ID = controlId };
            }
            return new Control { ID = controlId };
        }

        private void CreatePageShimObject(bool isActive = false)
        {
            ShimECNSession.Constructor = (instance) => { };
            var ecnSession = CreateECNSession();
            var shimSession = new ShimECNSession();
            shimSession.ClearSession = () => { };
            shimSession.Instance.CurrentUser = new User
            {
                UserID = 1,
                UserName = DefaultText,
                IsActive = isActive,
                CurrentSecurityGroup = new SecurityGroup
                {
                    AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                    IsActive = true
                },
                IsPlatformAdministrator = true,
            };
            shimSession.ClientIDGet = () => { return 1; };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            ShimCrossProductView.AllInstances.MasterGet = (x) =>
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
                    },
                    UserSessionGet = () => { return shimSession.Instance; },
                    LoggedInUserGet = () => { return 1; }
                };

                return site;
            };

            ShimPage.AllInstances.ResponseGet = (x) =>
            {
                return new ShimHttpResponse
                {
                    RedirectString = (y) => { _url = y; }
                };
            };
            ShimPage.AllInstances.RequestGet = (x) =>
            {
                return new ShimHttpRequest
                {
                    RawUrlGet = () => { return string.Empty; },
                };
            };
            ShimUtilities.Log_ErrorStringStringException = (x, y, z) => { };
            ShimCrossProductView.AllInstances.DisplayErrorString = (sender, message) => { };
            ShimBrand.GetAllClientConnections = (x) => { return new List<Brand>(); };
            ShimCrossProductView.AllInstances.LoadPageFilters = (x) => { };
            ShimFrameworkUad.ShimCode.AllInstances.SelectEnumsCodeType = (x, y) =>
            {
                return new List<FrameworkUAD_Lookup.Entity.Code>
                {
                    new FrameworkUAD_Lookup.Entity.Code
                    {
                        CodeName=FrameworkUAD_Lookup.Enums.DataCompareType.Match.ToString(),
                        CodeId=1
                    }
                };
            };

            ShimFrameworkUad.ShimCode.AllInstances.SelectCodeNameEnumsCodeTypeString = (x, y, z) =>
            {
                return new FrameworkUAD_Lookup.Entity.Code
                {
                    CodeId = 1,
                    CodeName = FrameworkUAD_Lookup.Enums.DataCompareType.Match.ToString()
                };
            };

            ShimFrameworkUas.ShimSourceFile.AllInstances.SelectInt32Boolean = (sender, clientId, includeCustomProperties) =>
            {
                return new List<FrameworkUAS.Entity.SourceFile>
                 {
                    new FrameworkUAS.Entity.SourceFile
                    {
                        SourceFileID = 1,
                        FileName= DefaultText
                    }
                 };
            };

            ShimFrameworkUas.ShimDataCompareRun.AllInstances.SelectForClientInt32 = (sender, clientId) =>
            {
                return new List<FrameworkUAS.Entity.DataCompareRun>
                {
                    new FrameworkUAS.Entity.DataCompareRun
                    {
                        SourceFileId = 1,
                        DateCreated=DateTime.Now,
                        ProcessCode= Guid.NewGuid().ToString()
                    }
                };
            };
        }

        private ECNSession CreateECNSession()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var result = typeof(ECNSession).GetConstructor(flags, null, new Type[0], null)
                ?.Invoke(new object[0]) as ECNSession;
            return result;
        }
    }
}
