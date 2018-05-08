using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using System.Xml.Linq;
using KMPlatform.Entity;
using KMPS.MD.Main;
using KMPS.MD.Main.Fakes;
using KMPS.MD.MasterPages.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using static KMPlatform.Enums;
using ShimFrameworkUad = FrameworkUAD_Lookup.BusinessLogic.Fakes;
using ShimFrameworkUas = FrameworkUAS.BusinessLogic.Fakes;
using ShimMarkets = KMPS.MD.Objects.Fakes.ShimMarkets;
using ShimReport = KMPS.MD.Main.Fakes.ShimReport;

namespace KMPS.MD.Tests.AudienceViews
{
    public partial class CustomerServiceTests
    {
        private const string MethodSelectPubsForMarkets = "selectPubsForMarkets";
        private const string FilterNo = "FilterNo";
        private const string ListBoxMarket = "lstMarket";
        private const string DefaultText = "Unit Test";
        private const string HiddenFiledBrandId = "hfBrandID";
        private const string Market = "Market";
        private const string MarketType = "MarketType";
        private const string FilterType = "FilterType";
        private const string Level1 = "level1";
        private const string Level2 = "level2";
        private const string TestTwo = "2";
        private const string Id = "ID";
        private const string Group = "Group";
        private const string MarketTypeAttribute = "P";
        private const string GroupTypeAttribute = "D";
        private const string FilterTypeAttribute = "A";
        private const string SplitterValue = "1|2";
        private const string Equal = "EQUAL";
        private const string Level11 = "level11";
        private const string Level12 = "level12";
        private const string HiddenPubTypeId = "hfPubTypeID";
        private const string LinkPubTypeShowHide = "lnkPubTypeShowHide";
        private const string PanelPubTypeBody = "pnlPubTypeBody";
        private const string HiddenFiledMasterGroup = "hfMasterGroup";
        private const string GridViewCategory = "gvCategory";
        private const string DataListAdhocFilter = "dlAdhocFilter";
        private const string LabelAdhocColumnValue = "lbAdhocColumnValue";
        private const string DropDownAdhocInt = "drpAdhocInt";
        private const string TextBoxAdhocIntFrom = "txtAdhocIntFrom";
        private const string TextAdhocIntTo = "txtAdhocIntTo";
        private const string Greater = "GREATER";
        private const string Lesser = "LESSER";
        private const string LinkButtonDimensionShowHide = "lnkDimensionShowHide";
        private const string PanelDimBody = "pnlDimBody";
        public const int DummyId = 1;
        private const string LblErrorMessage = "lblErrorMessage";
        private const string DivError = "divError";

        [TestCase(TestOne, Equal)]
        [TestCase(TestZero, DummyString)]
        public void SelectPubsForMarkets_MarketListIsNotNullAndDimensionsListBoxIsNotNull_UpdatePageControlValues(
            string brandId,
            string nodeAdhocChildValue)
        {
            // Arrange
            var isMarketAvailable = false;
            var pubSearchEnabled = false;
            var masterGroupSearchExist = false;
            var masterCodeSheetExist = false;
            GetField<HiddenField>(HiddenFiledBrandId).Value = brandId;
            CreatePageShimObject(true);
            var gridViewRow = CreateGridViewRow();
            var pubList = CreatePubsListObject();
            ShimControl.AllInstances.FindControlString = (sender, controlId) => GetControlById(controlId);
            ShimGridView.AllInstances.RowsGet = (x) => CreateGridViewRowCollectionObject();
            ShimGridView.AllInstances.DataKeysGet = (sender) =>
            {
                var orderDictionary = new OrderedDictionary { { FilterNo, 1 } };
                var keys = new ArrayList
                {
                    new DataKey(orderDictionary)
                };
                return new DataKeyArray(keys);
            };
            ShimRepeater.AllInstances.ItemsGet = (sender) => CreateRepeaterItemCollectionObject();
            ShimDataList.AllInstances.ItemsGet = (sender) => CreateDataListItemCollection();
            var listMasterCodeSheet = CreateMasterCodeSheetListObject();
            ShimMasterCodeSheet.GetSearchEnabledByBrandIDClientConnectionsInt32 = (x, y) =>
            {
                masterCodeSheetExist = true;
                return listMasterCodeSheet;
            };
            ShimMasterCodeSheet.GetSearchEnabledClientConnections = (x) =>
            {
                masterCodeSheetExist = true;
                return listMasterCodeSheet;
            };
            ShimPubs.GetSearchEnabledByBrandIDClientConnectionsInt32 = (x, y) =>
            {
                pubSearchEnabled = true;
                return pubList;
            };
            ShimPubs.GetSearchEnabledClientConnections = (x) =>
            {
                pubSearchEnabled = true;
                return pubList;
            };
            ShimControl.AllInstances.NamingContainerGet = (sender) => gridViewRow;
            ShimUtilities.SelectFilterListBoxesListBoxString = (x, y) => { };
            ShimMasterGroup.GetSearchEnabledByBrandIDClientConnectionsInt32 = (x, y) =>
            {
                masterGroupSearchExist = true;
                return CreateMasterGroupObject();
            };
            ShimMasterGroup.GetSearchEnabledClientConnections = (x) =>
            {
                masterGroupSearchExist = true;
                return CreateMasterGroupObject();
            };
            ShimMarkets.GetByIDClientConnectionsInt32 = (clientconnections, listValue) =>
            {
                isMarketAvailable = true;
                return CreateMarketsObject(nodeAdhocChildValue);
            };
            var lstMarket = GetField<ListBox>(ListBoxMarket);
            lstMarket.Items.Add(new ListItem { Text = TestOne, Value = TestOne, Enabled = true, Selected = true });
            lstMarket.Items.Add(new ListItem { Text = TestZero, Value = TestZero, Enabled = true, Selected = false });
            lstMarket.DataBind();

            // Act
            PrivatePage.Invoke(MethodSelectPubsForMarkets);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => isMarketAvailable.ShouldBeTrue(),
                () => pubSearchEnabled.ShouldBeTrue(),
                () => masterGroupSearchExist.ShouldBeTrue(),
                () => masterCodeSheetExist.ShouldBeTrue());
        }

        [Test]
        public void SelectPubsForMarkets_OnException_DisplayErrorMessage()
        {
            // Arrange
            const string ExceptionMessage = "ExceptionMessage";
            ShimControl.AllInstances.FindControlString = (sender, controlId) => GetControlById(controlId);
            ShimGridView.AllInstances.RowsGet = (x) => CreateGridViewRowCollectionObject();
            ShimRepeater.AllInstances.ItemsGet = (sender) => CreateRepeaterItemCollectionObject();
            ShimDataList.AllInstances.ItemsGet = (sender) => throw new Exception(ExceptionMessage);

            // Act
            PrivatePage.Invoke(MethodSelectPubsForMarkets);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<Label>(LblErrorMessage).Text.ShouldContain(ExceptionMessage),
                () => GetField<HtmlGenericControl>(DivError).Visible.ShouldBeTrue());
        }

        private static DropDownList CreateAdHocDropDownList(string controlId)
        {
            var drpAdhocInt = new DropDownList { ID = controlId };
            drpAdhocInt.Items.Add(new ListItem(Equal, Equal));
            drpAdhocInt.Items.Add(new ListItem(Greater, Greater));
            drpAdhocInt.Items.Add(new ListItem(Lesser, Lesser));
            return drpAdhocInt;
        }

        private static GridViewRowCollection CreateGridViewRowCollectionObject()
        {
            var arrayList = new ArrayList
            {
                new GridViewRow(0, 1, DataControlRowType.DataRow, DataControlRowState.Normal)
             };
            return new GridViewRowCollection(arrayList);
        }

        private static RepeaterItemCollection CreateRepeaterItemCollectionObject()
        {
            var arrayList = new ArrayList
            {
                new RepeaterItem(0, ListItemType.Item),
                new RepeaterItem(1, ListItemType.Item)
            };
            return new RepeaterItemCollection(arrayList);
        }

        private static List<MasterGroup> CreateMasterGroupObject()
        {
            return new List<MasterGroup>
            {
                new MasterGroup
                {
                    MasterGroupID = DummyId,
                    ColumnReference = TestOne
                }
            };
        }

        private static Objects.Markets CreateMarketsObject(string nodeAdhocChildValue)
        {
            return new Objects.Markets
            {
                MarketXML = CreateMarketXml(nodeAdhocChildValue).ToString(),
                BrandID = DummyId,
                BrandName = Test,
                MarketID = DummyId,
                MarketName = Test
            };
        }

        private static XDocument CreateMarketXml(string nodeAdhocChildValue)
        {
            return new XDocument
            (
                new XElement
                 (Market,
                    new XElement
                         (MarketType,
                            new XAttribute(Id, MarketTypeAttribute),
                            new XElement(Level1, TestOne, new XAttribute(Id, TestOne)),
                            new XElement(Level2, TestTwo, new XAttribute(Id, TestOne))),
                    new XElement
                         (MarketType,
                            new XAttribute(Id, GroupTypeAttribute),
                            new XElement(Group, new XAttribute(Id, TestOne),
                            new XElement(Level1, TestOne, new XAttribute(Id, TestOne)),
                            new XElement(Level2, TestTwo, new XAttribute(Id, TestTwo)))
                        ),
                    new XElement
                         (FilterType,
                            new XAttribute(Id, FilterTypeAttribute),
                            new XElement(Level1, new XAttribute(Id, SplitterValue),
                            new XElement(Level11, new XAttribute(Id, SplitterValue)),
                            new XElement(Level12, new XAttribute(Id, nodeAdhocChildValue))))
                 )
            );
        }
        
        private static void CreatePageShimObject(bool isActive = false)
        {
            ShimECNSession.Constructor = (instance) => { };
            var shimSession = new ShimECNSession {ClearSession = () => { }};
            shimSession.Instance.CurrentUser = new User
            {
                UserID = DummyId,
                UserName = DefaultText,
                IsActive = isActive,
                CurrentSecurityGroup = new SecurityGroup
                {
                    AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                    IsActive = true
                },
                IsPlatformAdministrator = true,
            };
            shimSession.ClientIDGet = () => 1;
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            ShimCustomerService.AllInstances.MasterGet = (x) =>
            {
                MasterPages.Site site = new ShimSite
                {
                    clientconnectionsGet = () => new KMPlatform.Object.ClientConnections
                    {
                        ClientLiveDBConnectionString = string.Empty,
                        ClientTestDBConnectionString = string.Empty
                    },
                    UserSessionGet = () => shimSession.Instance,
                    LoggedInUserGet = () => 1
                };
                return site;
            };
            ShimUtilities.Log_ErrorStringStringException = (x, y, z) => { };
            ShimCustomerService.AllInstances.DisplayErrorString = (sender, message) => { };
            ShimBrand.GetAllClientConnections = (x) => new List<Brand>();
            ShimCustomerService.AllInstances.LoadPageFilters = (x) => { };
            ShimFrameworkUad.ShimCode.AllInstances.SelectEnumsCodeType = (x, y) =>
                new List<FrameworkUAD_Lookup.Entity.Code>
                {
                    new FrameworkUAD_Lookup.Entity.Code
                    {
                        CodeName = FrameworkUAD_Lookup.Enums.DataCompareType.Match.ToString(),
                        CodeId = DummyId
                    }
                };

            ShimFrameworkUad.ShimCode.AllInstances.SelectCodeNameEnumsCodeTypeString = (x, y, z) =>
                new FrameworkUAD_Lookup.Entity.Code
                {
                    CodeId = DummyId,
                    CodeName = FrameworkUAD_Lookup.Enums.DataCompareType.Match.ToString()
                };

            ShimFrameworkUas.ShimSourceFile.AllInstances.SelectInt32Boolean =
                (sender, clientId, includeCustomProperties) => new List<FrameworkUAS.Entity.SourceFile>
                {
                    new FrameworkUAS.Entity.SourceFile
                    {
                        SourceFileID = DummyId,
                        FileName = DefaultText
                    }
                };

            ShimFrameworkUas.ShimDataCompareRun.AllInstances.SelectForClientInt32 = (sender, clientId) =>
                new List<FrameworkUAS.Entity.DataCompareRun>
                {
                    new FrameworkUAS.Entity.DataCompareRun
                    {
                        SourceFileId = DummyId,
                        DateCreated = DateTime.Now,
                        ProcessCode = Guid.NewGuid().ToString()
                    }
                };
        }

        private static List<MasterCodeSheet> CreateMasterCodeSheetListObject()
        {
            return new List<MasterCodeSheet>
            {
                new MasterCodeSheet
                {
                    MasterID = DummyId,
                    MasterGroupID = DummyId,
                    MasterValue = TestOne,
                    SortOrder = DummyId,
                    MasterDesc = Test,
                    MasterDesc1 = Test,
                    EnableSearching = true
                }
            };
        }
    }
}
