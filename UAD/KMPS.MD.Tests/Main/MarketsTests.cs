using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using KM.Platform.Fakes;
using KMPS.MD.Main;
using KMPS.MD.Main.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using Markets = KMPS.MD.Main.Markets;
using ShimMarkets = KMPS.MD.Main.Fakes.ShimMarkets;

namespace KMPS.MD.Tests.Main
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class MarketsTests : BasePageTests
    {
        private const string DummyString = "DummyString";
        private const string TestZero = "0";
        private const string TestOne = "1";
        private const string AllProducts = "All Products";
        private const string MethodPageLoad = "Page_Load";
        private const string MethodLoadItemGridFromDB = "LoadItemGridFromDB";
        private const string GrdItems = "grdItems";
        private const string PubTypeDisplayName = "PubTypeDisplayName";
        private const string PubTypeId = "PubTypeId";
        private const string Title = "Title";
        private const string DisplayName = "DisplayName";
        private const string EntryItems = "EntryItems";
        private const string TxtAdhocIntTo = "txtAdhocIntTo";
        private const string DrpAdhocInt = "drpAdhocInt";
        private Markets _testEntity = new Markets();
        
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            InitializePage(_testEntity);

            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => true;
            ShimMarkets.AllInstances.LoadMarkets = _ => { };
            ShimMarkets.AllInstances.LoadPubTypes = _ => { };
            ShimMarkets.AllInstances.LoadPubs = _ => { };
            GetField<HiddenField>("hfBrandID").Value = TestZero;
            GetField<Panel>("pnlBrand").Visible = false;
            GetField<DropDownList>("drpBrand").Visible = false;
            GetField<Label>("lblBrandName").Visible = false;
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
        public void LoadItemGridFromDB_OnEmptyDocument_GridViewShouldBeEmpty()
        {
            // Arrange
            var xmlDocument = new XmlDocument();
            SetupForLoadItemGridFromDB();

            // Act	
            PrivatePage.Invoke(MethodLoadItemGridFromDB, xmlDocument);
            var gridView = PrivatePage.GetField(GrdItems) as GridView;

            // Assert
            gridView.ShouldSatisfyAllConditions(
                () => gridView.ShouldNotBeNull(),
                () =>
                {
                    var rows = gridView.Rows.Cast<GridViewRow>();
                    rows.ShouldNotBeNull();
                    rows.ShouldBeEmpty();
                }
            );
        }

        [Test]
        [TestCase("P")]
        [TestCase(DummyString)]
        public void LoadItemGridFromDB_OnMarketNode_FillGridView(string itemType)
        {
            // Arrange
            var xmlDocument = new XmlDocument();
            var nodeMarket = xmlDocument.CreateElement("Market");

            var nodeMarketType = xmlDocument.CreateElement("MarketType");
            var attributeItemType = xmlDocument.CreateAttribute("ID");
            attributeItemType.Value = itemType;
            nodeMarketType.Attributes.Append(attributeItemType);

            var nodePubType = xmlDocument.CreateElement("PubType");
            var attributeId = xmlDocument.CreateAttribute("ID");
            attributeId.Value = itemType;
            nodePubType.Attributes.Append(attributeId);

            var nodeGroup = xmlDocument.CreateElement("Group");
            var attributeGroupId = xmlDocument.CreateAttribute("ID");
            attributeGroupId.Value = itemType;
            nodeGroup.Attributes.Append(attributeGroupId);

            nodePubType.AppendChild(nodeGroup);
            nodeMarketType.AppendChild(nodePubType);
            nodeMarket.AppendChild(nodeMarketType);
            xmlDocument.AppendChild(nodeMarket);
            SetupForLoadItemGridFromDB();

            // Act	
            PrivatePage.Invoke(MethodLoadItemGridFromDB, xmlDocument);
            var gridView = PrivatePage.GetField(GrdItems) as GridView;

            // Assert
            gridView.ShouldSatisfyAllConditions(
                () => gridView.ShouldNotBeNull(),
                () =>
                {
                    var rows = gridView.Rows.Cast<GridViewRow>();
                    rows.ShouldNotBeNull();
                    rows.ShouldNotBeEmpty();
                    rows.Count().ShouldBe(1);
                    rows.First().Cells.ShouldNotBeNull();
                    rows.First().Cells[1].ShouldNotBeNull();
                    rows.First().Cells[1].Text.ShouldBe(itemType);
                }
            );
        }

        [Test]
        [TestCase("Equal", false)]
        [TestCase(DummyString, true)]
        public void LoadItemGridFromDB_OnFilterType_ChangeTxtAdhocIntToEnabled(string itemType, bool textBoxIsEnabled)
        {
            // Arrange
            var xmlDocument = new XmlDocument();
            var nodeMarket = xmlDocument.CreateElement("Market");

            var nodeFilterType = xmlDocument.CreateElement("FilterType");
            var attributeItemType = xmlDocument.CreateAttribute("ID");
            attributeItemType.Value = "A";
            nodeFilterType.Attributes.Append(attributeItemType);

            var nodeFilters = xmlDocument.CreateElement("Filters");

            var nodeFilter = xmlDocument.CreateElement("Filter");
            var attributeId = xmlDocument.CreateAttribute("ID");
            attributeId.Value = $"{DummyString}|{DummyString}";
            nodeFilter.Attributes.Append(attributeId);

            var nodeFilter2 = xmlDocument.CreateElement("Filter");
            var attributeId2 = xmlDocument.CreateAttribute("ID");
            attributeId2.Value = itemType;
            nodeFilter2.Attributes.Append(attributeId2);

            nodeFilters.AppendChild(nodeFilter);
            nodeFilters.AppendChild(nodeFilter2);
            nodeFilterType.AppendChild(nodeFilters);
            nodeMarket.AppendChild(nodeFilterType);
            xmlDocument.AppendChild(nodeMarket);
            SetupForLoadItemGridFromDB();
            var dropDownList = new DropDownList();
            dropDownList.Items.Add(new ListItem(DummyString, itemType));
            PrivatePage.SetField(DrpAdhocInt, dropDownList);

            // Act	
            PrivatePage.Invoke(MethodLoadItemGridFromDB, xmlDocument);
            var gridView = PrivatePage.GetField(GrdItems) as GridView;
            var txtAdhocIntTo = PrivatePage.GetField(TxtAdhocIntTo) as TextBox;

            // Assert
            gridView.ShouldSatisfyAllConditions(
                () => gridView.ShouldNotBeNull(),
                () =>
                {
                    var rows = gridView.Rows.Cast<GridViewRow>();
                    rows.ShouldNotBeNull();
                    rows.ShouldBeEmpty();
                    txtAdhocIntTo.ShouldNotBeNull();
                    txtAdhocIntTo.Enabled.ShouldBe(textBoxIsEnabled);
                    txtAdhocIntTo.Text.ShouldContain(DummyString);
                }
            );
        }

        private void SetupForLoadItemGridFromDB()
        {
            PrivatePage.SetField(EntryItems, new ArrayList());
            ShimForMasterPage();
            ShimMarkets.AllInstances.LoadPubs = (_) => { };
            ShimDataFunctions.GetClientSqlConnectionClientConnections = (_) => new SqlConnection();
            ShimDataFunctions.getDataTableStringSqlConnection = (cmd, __) =>
            {
                if (cmd.Contains("SELECT PubTypeId, PubTypeDisplayName"))
                {
                    return GetPubTypesDataTable();
                }
                if (cmd.Contains("SELECT PubName as 'Title'"))
                {
                    return GetPubNamesDataTable();
                }
                if (cmd.Contains("SELECT DisplayName FROM MasterGroups"))
                {
                    return GetDisplayNamesDataTable();
                }
                if (cmd.Contains("SELECT mastervalue"))
                {
                    return GetMasterCodeSheetDataTable();
                } 
                return new DataTable();
            };
        }

        private static DataTable GetPubTypesDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(PubTypeDisplayName, typeof(string));
            dataTable.Columns.Add(PubTypeId, typeof(string));
            var row = dataTable.NewRow();
            row[0] = DummyString;
            row[0] = DummyString;
            dataTable.Rows.Add(row);
            return dataTable;
        }

        private static DataTable GetPubNamesDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(Title, typeof(string));
            var row = dataTable.NewRow();
            row[0] = DummyString;
            dataTable.Rows.Add(row);
            return dataTable;
        }

        private static DataTable GetDisplayNamesDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(DisplayName, typeof(string));
            var row = dataTable.NewRow();
            row[0] = DummyString;
            dataTable.Rows.Add(row);
            return dataTable;
        }

        private static DataTable GetMasterCodeSheetDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(Title, typeof(string));
            var row = dataTable.NewRow();
            row[0] = DummyString;
            dataTable.Rows.Add(row);
            return dataTable;
        }
    }
}
