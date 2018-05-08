using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using System.Xml.Linq;
using KMPlatform.Entity;
using KMPS.MD.Main;
using KMPS.MD.MasterPages.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using static KMPlatform.Enums;
using ShimFrameworkUad = FrameworkUAD_Lookup.BusinessLogic.Fakes;
using ShimFrameworkUas = FrameworkUAS.BusinessLogic.Fakes;
using ShimReport = KMPS.MD.Main.Fakes.ShimReport;

namespace KMPS.MD.Tests.AudienceViews
{
    /// <summary>
    /// Unit test for <see cref="Report"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ReportTest : BasePageTests
    {
        private const string TestZero = "0";
        private const string TestOne = "1";
        private const string SelectPubsForMarkets = "selectPubsForMarkets";
        private const string PubTypeRepeater = "PubTypeRepeater";
        private const string PubTypeListBox = "PubTypeListBox";
        private const string ListResponse = "lstResponse";
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
        private const string FilterNo = "FilterNo";
        private const string ListBoxMarket = "lstMarket";
        private const string DefaultText = "Unit Test";
        private const string Test = "Test";
        private const string LinkButtonCancel = "lnkCancel";
        private const string LinkButtonEdit = "lnkEdit";
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
        private const string Greater = "GREATER";
        private const string Lesser = "LESSER";
        private const string Level11 = "level11";
        private const string Level12 = "level12";
        private const string LinkButtonDimensionShowHide = "lnkDimensionShowHide";
        private const string PanelDimBody = "pnlDimBody";
        private Report _testEntity;

        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new Report();
            InitializePage(_testEntity);
            InitializeAllControls(_testEntity);
        }

        [TestCase(TestOne)]
        [TestCase(TestZero)]
        public void SelectPubsForMarkets_MarketListIsNotNullAndDimensionsListBoxIsNotNull_UpdatePageControlValues(string brandId)
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
                var orderDictionaly = new OrderedDictionary { { FilterNo, 1 } };
                var dataKey = new DataKey(orderDictionaly);
                var keys = new ArrayList
                {
                    new DataKey(orderDictionaly)
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
                return CreateMarketsObject();
            };
            var lstMarket = GetField<ListBox>(ListBoxMarket);
            lstMarket.Items.Add(new ListItem { Text = TestOne, Value = TestOne, Enabled = true, Selected = true });
            lstMarket.Items.Add(new ListItem { Text = TestZero, Value = TestZero, Enabled = true, Selected = false });
            lstMarket.DataBind();

            // Act
            PrivatePage.Invoke(SelectPubsForMarkets);

            // Assert
            isMarketAvailable.ShouldBeTrue();
            pubSearchEnabled.ShouldBeTrue();
            masterGroupSearchExist.ShouldBeTrue();
            masterCodeSheetExist.ShouldBeTrue();
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

        private static DataListItemCollection CreateDataListItemCollection()
        {
            var arrayList = new ArrayList()
                {
                    new DataListItem(0, ListItemType.Item),
                    new DataListItem(1, ListItemType.Item)
                };
            return new DataListItemCollection(arrayList);
        }

        private List<MasterGroup> CreateMasterGroupObject()
        {
            return new List<MasterGroup>
            {
                new MasterGroup
                {
                    MasterGroupID = 1,
                    ColumnReference = TestOne
                }
            };
        }

        private Objects.Markets CreateMarketsObject()
        {
            return new Objects.Markets
            {
                MarketXML = CreateMarketXML().ToString(),
                BrandID = 1,
                BrandName = Test,
                MarketID = 1,
                MarketName = Test
            };
        }

        private static XDocument CreateMarketXML()
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
                            new XElement(Level12, new XAttribute(Id, Equal))))
                 )
            );
        }

        private Control GetControlById(string controlId)
        {
            switch (controlId)
            {
                case PubTypeRepeater:
                    return new Repeater { ID = controlId };
                case HiddenPubTypeId:
                    return new HiddenField { ID = controlId, Value = TestOne };
                case PubTypeListBox:
                    return new ListBox { ID = controlId };
                case PanelPubTypeBody:
                    return new Panel { ID = controlId };
                case ListResponse:
                    return new ListBox { ID = controlId };
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
                    return new Control { ID = controlId };
            }
        }

        private static DropDownList CreateAdHocDropDownList(string controlId)
        {
            var drpAdhocInt = new DropDownList { ID = controlId };
            drpAdhocInt.Items.Add(new ListItem(Equal, Equal));
            drpAdhocInt.Items.Add(new ListItem(Greater, Greater));
            drpAdhocInt.Items.Add(new ListItem(Lesser, Lesser));
            return drpAdhocInt;
        }

        private void CreatePageShimObject(bool isActive = false)
        {
            ShimECNSession.Constructor = (instance) => { };
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
            ShimReport.AllInstances.MasterGet = (x) =>
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
            ShimUtilities.Log_ErrorStringStringException = (x, y, z) => { };
            ShimReport.AllInstances.DisplayErrorString = (sender, message) => { };
            ShimBrand.GetAllClientConnections = (x) => new List<Brand>();
            ShimReport.AllInstances.LoadPageFilters = (x) => { };
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

        private static List<Pubs> CreatePubsListObject()
        {
            return new List<Pubs>
            {
                new Pubs
                {
                    PubID = 1,
                    PubName = Test,
                    PubCode = Test,
                    PubTypeID = 1,
                    EnableSearching = true
                }
            };
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

        private static List<MasterCodeSheet> CreateMasterCodeSheetListObject()
        {
            return new List<MasterCodeSheet>
            {
                new MasterCodeSheet
                {
                    MasterID = 1,
                    MasterGroupID = 1,
                    MasterValue = TestOne,
                    SortOrder = 1,
                    MasterDesc = Test,
                    MasterDesc1 = Test,
                    EnableSearching = true
                }
            };
        }
    }
}
