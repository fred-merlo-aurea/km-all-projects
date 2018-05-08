using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using KMPlatform.Object;
using KMPlatform.Object.Fakes;
using KMPS.MD.Controls;
using KMPS.MD.Controls.Fakes;
using KMPS.MD.MasterPages;
using MDMain = KMPS.MD.Main;
using MainFakes = KMPS.MD.Main.Fakes;
using KMPS.MD.MasterPages.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using Telerik.Web.UI;
using Telerik.Web.UI.Fakes;
using TestCommonHelpers;
using ShimAdhoc = KMPS.MD.Controls.Fakes.ShimAdhoc;
using UADBusinessLogicFakes = FrameworkUAD.BusinessLogic.Fakes;
using ShimDataFunctions = FrameworkUAD.DataAccess.Fakes.ShimDataFunctions;

namespace KMPS.MD.Tests.AudienceViews
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class CustomerServiceTests : BasePageTests
    {
        private const string TestZero = "0";
        private const string TestOne = "1";
        private const string AllProducts = "All Products";
        private const string MethodPageLoad = "Page_Load";
        private const string MethodLnkPopupCommand = "lnkPopup_Command";
        private const string TestNumber = "1";
        private const string ExpectedZipCodeRadiusValues = "1.97101449275362|2.02898550724638|0.970996308707783|1.02900266654272|1.98550724637681|2.01449275362319|0.98549828384275|1.01450145996992";
        private const string TestZipCode = "NY12221";
        private const string FirstName = "fName";
        private const string LastName = "lName";
        private const string CompanyName = "company";
        private const string AddressName = "address";
        private const string MailStopName = "mailStop";
        private const string Address3Name = "address3";
        private const string CityName = "city";
        private const string StateName = "state";
        private const string MasterDes = "desc";
        private const string MasterValue = "value";
        private const string Visible = "visible";
        private const string NotVisible = "notVisible";
        private const string ValueTen = "10";
        private const string ValueOne = "1";
        private const string ValueZero = "0";
        private const string Open = "open";
        private const string DrpCountry = "drp_Country";
        private const string DrpState = "drp_State";
        private const string PnlBrand = "pnlBrand";
        private const string DrpBrand = "drpBrand";
        private const string LblBrandName = "lblBrandName";
        private const string DlDetails = "dlDetails";
        private const string DlAdhocDetails = "dlAdhocDetails";
        private const string PlDrpState = "pldrp_State";
        private const string HfBrandId = "hfBrandID";
        private const string HfSubscriptionId = "hfSubscriptionID";
        private const string LoadMethod = "LoadSubDetails";
        private const string DummyString = "DummyString";
        private const string MarketsString = "Markets";
        private const int CountryId = 10;
        private const int TestUserId = 1;
        private const string Dimension = "Dimension";
        private const string PubType = "PubType";
        private const string LblDimensionControl = "lblDimensionControl";
        private const string CountryRegions = "Country Regions";
        private const string LstMarket = "LSTMARKET";
        private const string LnkSortByDescription = "lnkSortByDescription";
        private const string LnkSortByValue = "lnkSortByValue";
        private const string RtbDimSearch = "rtbDimSearch";
        private const string PubTypeRepeater = "PubTypeRepeater";
        private const string PubTypeListBox = "PubTypeListBox";
        private const string LstResponse = "lstResponse";
        private const string DlDimensions = "dlDimensions";
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
        private const string TxtLastName = "txtLastName";
        private const string TxtFirstName = "txtFirstName";
        private const string TxtCompany = "txtCompany";
        private const string TxtPhone = "txtPhone";
        private const string TxtEmail = "txtEmail";
        private MDMain::CustomerService _testEntity;
        private List<Subscriber> subscribers;

        [SetUp]
        public override void SetUp()
        {
            UserId = TestUserId;

            base.SetUp();
            _testEntity = new MDMain::CustomerService();
            InitializePage(_testEntity);

            ShimGridView.AllInstances.DataBind = _ => { };
            ShimFilterSegmentation.AllInstances.FilterViewCollectionGet = _ => new FilterViews(new ShimClientConnections(), 1);
            MainFakes::ShimCustomerService.AllInstances.loadProductandDimensions = _ => { };
            MainFakes::ShimCustomerService.AllInstances.loadStandardFiltersListboxes = _ => { };
            ShimAdhoc.AllInstances.LoadAdhocGrid = _ => { };
            ShimEmailStatus.GetAllClientConnections = _ => null;
            ShimUserDataMask.GetByUserIDClientConnectionsInt32 = (_, __) => new List<UserDataMask>();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => true;
            GetField<HiddenField>("hfBrandID").Value = TestZero;
            GetField<Panel>("pnlBrand").Visible = false;
            GetField<DropDownList>("drpBrand").Visible = false;
            GetField<Label>("lblBrandName").Visible = false;
        }
        
        [Test]
        public void getFilter_RadiusAndZipCodeSupplied_CorrectFilterValues()
        {
            // Arrange
            GetField<HiddenField>("hfBrandID").Value = TestNumber;
            GetField<Panel>("pnlBrand").Visible = false;
            GetField<TextBox>("txtRadiusMin").Text = "1";
            GetField<TextBox>("txtRadiusMax").Text = "2";
            
            ShimResponseGroup.GetActiveByPubIDClientConnectionsInt32 = (_, __) => null;
            ShimActivity.AllInstances.GetActivityFilters = _ => new List<Field>();
            ShimCirculation.AllInstances.GetCirculationFilters = _ => new List<Field>();
            ShimRadMaskedTextBox.AllInstances.TextWithLiteralsGet = _ => TestZipCode;
            FrameworkUAD.Object.Fakes.ShimLocation.ValidateBingAddressLocationString = (_, __) => new FrameworkUAD.Object.Location() {Longitude = 1, Latitude = 2, IsValid = true};
            ShimMasterGroup.GetSearchEnabledByBrandIDClientConnectionsInt32 = (_, __) => null;
            MainFakes::ShimCustomerService.AllInstances.getPubsValues = _ => string.Empty;
            ShimSite.AllInstances.clientconnectionsGet = _ => null;

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
        [TestCase(Visible)]
        [TestCase(NotVisible)]
        public void LoadSubDetails_ForPldrpStateVisible_ShouldLoad(string param)
        {
            // Arrange
            SetUpLoad(param);
            var privateObject = new PrivateObject(_testEntity);

            // Act
            privateObject.Invoke(LoadMethod);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Panel>(PnlBrand).Visible.ShouldBeTrue(),
                () => GetField<DropDownList>(DrpBrand).Visible.ShouldBeTrue(),
                () => GetField<Label>(LblBrandName).Visible.ShouldBeTrue());
        }

        [Test]
        [TestCase(DummyString, MarketsString, TestOne)]
        [TestCase(DummyString, MarketsString, TestZero)]
        [TestCase(LstMarket, LstMarket, TestOne)]
        [TestCase(DummyString, CountryRegions, TestOne)]
        public void LnkPopup_Command_ListBoxIsNotNull_Return(string commandName, string commandType, string hfBrandId)
        {
            // Arrange
            ShimControl.AllInstances.FindControlString = (sender, controlId) => GetControlById(controlId);
            SetupForLnkPopup(commandType, hfBrandId);

            // Act	
            PrivatePage.Invoke(MethodLnkPopupCommand, null, new CommandEventArgs(commandName, commandType));

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Label>(LblDimensionControl).Text.ShouldContain(commandName),
                () => GetField<LinkButton>(LnkSortByDescription).Visible.ShouldBeFalse(),
                () => GetField<LinkButton>(LnkSortByValue).Visible.ShouldBeFalse(),
                () => GetField<RadTextBox>(RtbDimSearch).Visible.ShouldBeTrue());
        }

        [Test]
        [TestCase(PubType, TestOne)]
        [TestCase(PubType, TestZero)]
        public void LnkPopup_Command_ListBoxIsNullAndPubType_Return(string commandType, string hfBrandId)
        {
            // Arrange
            var commandArgument = $"{TestOne}|{commandType}|{DummyString}";
            ShimControl.AllInstances.FindControlString = (sender, controlId) =>
            {
                if (controlId.Equals(TestOne))
                {
                    return null;
                }
                return GetControlById(controlId);
            };
            SetupForLnkPopup(commandType, hfBrandId);
            ShimPubs.GetSearchEnabledClientConnections = _ => new List<Pubs>();
            ShimPubs.GetSearchEnabledByBrandIDClientConnectionsInt32 = (_, __) => new List<Pubs>();
            ShimRepeater.AllInstances.ItemsGet = (sender) =>
            {
                var arrayList = new ArrayList
                {
                    new RepeaterItem(0, ListItemType.Item),
                    new RepeaterItem(1, ListItemType.Item)
                };
                return new RepeaterItemCollection(arrayList);
            };

            // Act	
            PrivatePage.Invoke(MethodLnkPopupCommand, null, new CommandEventArgs(TestOne, commandArgument));

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Label>(LblDimensionControl).Text.ShouldContain(TestOne),
                () => GetField<LinkButton>(LnkSortByDescription).Visible.ShouldBeFalse(),
                () => GetField<LinkButton>(LnkSortByValue).Visible.ShouldBeFalse(),
                () => GetField<RadTextBox>(RtbDimSearch).Visible.ShouldBeTrue());
        }

        [Test]
        [TestCase(Dimension, TestOne, false)]
        [TestCase(Dimension, TestZero, false)]
        [TestCase(Dimension, TestZero, true)]
        public void LnkPopup_Command_ListBoxIsNullAndDimension_Return(string commandType, string hfBrandId, bool fillList)
        {
            // Arrange
            var commandArgument = $"{TestOne}|{commandType}|{DummyString}";
            ShimControl.AllInstances.FindControlString = (sender, controlId) =>
            {
                if (controlId.Equals(TestOne))
                {
                    return null;
                }
                return GetControlById(controlId);
            };
            SetupForLnkPopup(commandType, hfBrandId);
            ShimMasterCodeSheet.GetSearchEnabledClientConnections = _ => new List<MasterCodeSheet>();
            ShimMasterCodeSheet.GetSearchEnabledByBrandIDClientConnectionsInt32 = (_, __) => new List<MasterCodeSheet>();
            ShimDataList.AllInstances.ItemsGet = (sender) =>
            {
                var arrayList = new ArrayList
                {
                    new DataListItem(0, ListItemType.Item),
                    new DataListItem(1, ListItemType.Item)
                };
                return new DataListItemCollection(arrayList);
            };
            ShimListControl.AllInstances.ItemsGet = (sender) =>
            {
                if (!fillList && commandType.Equals(Dimension))
                {
                    return new ListItemCollection();
                }
                var listItemCollection = new ListItemCollection
                {
                    new ListItem(DummyString, DummyString)
                    {
                        Selected = true
                    },
                    new ListItem(DummyString, TestNumber)
                };
                return listItemCollection;
            };

            // Act	
            PrivatePage.Invoke(MethodLnkPopupCommand, null, new CommandEventArgs(TestOne, commandArgument));

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Label>(LblDimensionControl).Text.ShouldContain(TestOne),
                () => GetField<LinkButton>(LnkSortByDescription).Visible.ShouldBeTrue(),
                () => GetField<LinkButton>(LnkSortByValue).Visible.ShouldBeTrue(),
                () => GetField<RadTextBox>(RtbDimSearch).Visible.ShouldBeTrue());
        }

        [TestCase(TestOne, 91, true)]
        [TestCase(TestOne, 91, false)]
        [TestCase(TestZero, 90, true)]
        public void GetFilter_ByPageControlObject_ReturnsFilterObject(string brandId, int count, bool drpBrandVisible)
        {
            // Arrange
            GetField<Panel>(PnlBrand).Visible = true;
            var drpBrand = GetField<DropDownList>(DrpBrand);
            drpBrand.Visible = drpBrandVisible;
            drpBrand.Items.Add(new ListItem(DummyString, TestOne));
            drpBrand.SelectedIndex = 0;
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

        private void BindRadComboBoxForGetFilter()
        {
            var radComboBoxItemCollection = new[] { "1", "2", "3" };
            var comboBoxIdList = new[]
            {
                "rcbEmail", "rcbPhone", "rcbFax", "rcbMedia", "rcbIsLatLonValid",
                "rcbMailPermission", "rcbFaxPermission", "rcbPhonePermission", "rcbOtherProductsPermission",
                "rcbThirdPartyPermission", "rcbEmailRenewPermission", "rcbTextPermission", "rcbEmailStatus"
            };
            foreach (var item in comboBoxIdList)
            {
                var radComboBox = GetField<RadComboBox>(item);
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
            GetField<TextBox>(TxtLastName).Text = DummyString;
            GetField<TextBox>(TxtFirstName).Text = DummyString;
            GetField<TextBox>(TxtCompany).Text = DummyString;
            GetField<TextBox>(TxtPhone).Text = DummyString;
            GetField<TextBox>(TxtEmail).Text = DummyString;
            var gridViewRow = CreateGridViewRow();
            var pubList = CreatePubsListObject();
            ShimRadMaskedTextBox.AllInstances.TextWithLiteralsGet = _ => TestZipCode;
            MainFakes::ShimCustomerService.AllInstances.ResetFitlerControls = (sender) => { };
            MainFakes::ShimCustomerService.AllInstances.getListboxValuesListBox = (_, __) => DummyString;
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
                    control.Text = Test;
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
            MainFakes::ShimCustomerService.AllInstances.selectStateOnRegion = (sender) => { };
            MainFakes::ShimCustomerService.AllInstances.getPubsValues = _ => DummyString;
            MainFakes::ShimCustomerService.AllInstances.getPubsListboxText = _ => DummyString;
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

        private void SetupForLnkPopup(string commandType, string hfBrandId)
        {
            GetField<HiddenField>(HfBrandId).Value = hfBrandId;
            ShimListControl.AllInstances.ItemsGet = (sender) =>
            {
                if (commandType.Equals(MarketsString) || commandType.Equals(PubType) || commandType.Equals(Dimension))
                {
                    return new ListItemCollection();
                }

                var listItemCollection = new ListItemCollection
                {
                    new ListItem(DummyString, DummyString)
                    {
                        Selected = true
                    },
                    new ListItem(DummyString, TestNumber)
                };
                return listItemCollection;
            };
            ShimMarkets.GetByBrandIDClientConnectionsInt32 = (_, __) => new List<Markets>();
            ShimMarkets.GetNotInBrandClientConnections = (_) => new List<Markets>();
            MainFakes::ShimCustomerService.AllInstances.MasterGet = (x) =>
            {
                Site site = new ShimSite
                {
                    clientconnectionsGet = () => new ClientConnections
                    {
                        ClientLiveDBConnectionString = string.Empty,
                        ClientTestDBConnectionString = string.Empty
                    }
                };
                return site;
            };
            MainFakes::ShimCustomerService.AllInstances.selectCountryOnRegion = _ => { };
        }

        private static Control GetControlById(string controlId)
        {
            switch (controlId)
            {
                case DummyString:
                case LstMarket:
                case CountryRegions:
                    return new ListBox {ID = controlId};
                case PubTypeRepeater:
                    return new Repeater {ID = controlId};
                case PubTypeListBox:
                case LstResponse:
                    return new ListBox {ID = controlId};
                case DlDimensions:
                    return new DataList {ID = controlId};
                case LabelResponseGroup:
                    return new Label { ID = controlId };
                case HiddenPubTypeId:
                    return new HiddenField { ID = controlId, Value = TestOne };
                case PanelPubTypeBody:
                    return new Panel { ID = controlId };
                case HiddenFiledMasterGroup:
                    return new HiddenField { ID = controlId, Value = TestOne };
                case LinkPubTypeShowHide:
                    return new LinkButton { ID = controlId };
                case GridViewCategory:
                    return new GridView { ID = controlId };
                case DataListAdhocFilter:
                    return new DataList { ID = controlId };
                case LabelAdhocColumnValue:
                    return new Label { ID = controlId, Text = SplitterValue };
                case DropDownAdhocInt:
                    return CreateAdHocDropDownList(controlId);
                case TextBoxAdhocIntFrom:
                    return new TextBox { ID = controlId };
                case TextAdhocIntTo:
                    return new TextBox { ID = controlId };
                case LinkButtonDimensionShowHide:
                    return new LinkButton { ID = controlId };
                case PanelDimBody:
                    return new Panel { ID = controlId };
                default:
                    return new Control {ID = controlId};
            }
        }

        private void SetUpLoad(string param)
        {
            UserId = TestUserId;
            base.SetUp();
            _testEntity = new MDMain::CustomerService();
            InitializePage(_testEntity);
            GetField<HiddenField>(HfSubscriptionId).Value = ValueTen;
            GetField<HiddenField>(HfBrandId).Value = ValueTen;
            if (param.Equals(Visible))
            {
                ShimControl.AllInstances.VisibleGet = (x) => true;
                GetField<PlaceHolder>(PlDrpState).Visible = true;
                GetField<DropDownList>(DrpCountry).SelectedValue = ValueOne;
                ShimListControl.AllInstances.SelectedValueGet = (x) => ValueOne;
                ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => false;
                var userData = new UserDataMask()
                {
                    MaskField = StateName
                };
                var userList = new List<UserDataMask>();
                userList.Add(userData);
                ShimUserDataMask.GetByUserIDClientConnectionsInt32 = (x, y) => userList;
            }
            else if (param.Equals(NotVisible))
            {
                ShimListControl.AllInstances.SelectedValueGet = (x) => ValueZero;
                GetField<DropDownList>(DrpCountry).SelectedValue = ValueZero;
            }
            var subscriber = new Subscriber()
            {
                FName = FirstName,
                LName = LastName,
                Company = CompanyName,
                Address = AddressName,
                MailStop = MailStopName,
                Address3 = Address3Name,
                City = CityName,
                State = StateName,
                CountryID = CountryId
            };
            subscribers = new List<Subscriber>();
            subscribers.Add(subscriber); 
            ShimSubscriber.GetClientConnectionsInt32Int32 = (x, y, z) => subscribers;
            ShimDropDownList.AllInstances.SelectedIndexGet = (x) => 0;
            GetField<DropDownList>(DrpState).SelectedValue = StateName;
            GetField<DropDownList>(DrpState).SelectedIndex = 0;
            ShimBaseDataBoundControl.AllInstances.DataBind = _ => { };
            ShimBaseDataList.AllInstances.DataBind = _ => { };
            var region = new Region()
            {
                CountryID = CountryId
            };
            var regions = new List<Region>();
            regions.Add(region);
            ShimRegion.GetByCountryIDInt32 = (x) => regions;
            var subscriberDimension = new SubscriberDimension()
            {
                DisplayName = FirstName,
                MasterDesc = MasterDes,
                MasterValue = MasterValue
            };
            var subDescList = new List<SubscriberDimension>();
            subDescList.Add(subscriberDimension);
            subDescList.Add(subscriberDimension);
            ShimSubscriberDimension.GetSubscriberDimensionClientConnectionsInt32Int32 = (x, y, z) => subDescList;
            var dlDetails = GetField<DataList>(DlDetails);
            ShimBaseDataList.AllInstances.DataSourceGet = (x) => dlDetails;
            var subAdhoc = new SubscriberAdhoc()
            {
                AdhocField = FirstName,
                AdhocValue = CompanyName
            };
            var subAdhocList = new List<SubscriberAdhoc>();
            subAdhocList.Add(subAdhoc);
            ShimSubscriberAdhoc.GetClientConnectionsInt32 = (x, y) => subAdhocList;
            var dlAdhokDetails = GetField<DataList>(DlAdhocDetails);
            ShimBaseDataList.AllInstances.DataSourceGet = (x) => dlAdhokDetails;
            var subPub = new SubscriberPubs()
            {
                Address1 = AddressName
            };
            var subPubList = new List<SubscriberPubs>();
            subPubList.Add(subPub);
            ShimSubscriberPubs.GetSubscriberPubsClientConnectionsInt32Int32 = (x, y, z) => subPubList;
            ShimGridView.AllInstances.DataBind = (_) => { };
            var subActivity = new SubscriberActivity()
            {
                Activity = Open
            };
            var subActivityList = new List<SubscriberActivity>();
            subActivityList.Add(subActivity);
            ShimSubscriberActivity.GetClientConnectionsInt32Int32 = (x, y, z) => subActivityList;
            var subVisitActivity = new SubscriberVisitActivity()
            {
                DomainName = FirstName
            };
            var subVisitActivityList = new List<SubscriberVisitActivity>();
            subVisitActivityList.Add(subVisitActivity);
            ShimSubscriberVisitActivity.GetClientConnectionsInt32 = (x, y) => subVisitActivityList;
        }
    }
}
