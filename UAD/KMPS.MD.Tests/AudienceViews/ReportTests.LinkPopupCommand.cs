using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using KMPlatform.Entity;
using KMPS.MD.Main;
using KMPS.MD.MasterPages.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using static KMPlatform.Enums;
using ShimReport = KMPS.MD.Main.Fakes.ShimReport;

namespace KMPS.MD.Tests.AudienceViews
{
    /// <summary>
    /// Unit test for <see cref="Report"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ReportTestsLinkPopupCommand : BasePageTests
    {
        private const string TestZero = "0";
        private const string TestOne = "1";
        private const string PubTypeRepeater = "PubTypeRepeater";
        private const string PubTypeListBox = "PubTypeListBox";
        private const string ListResponse = "lstResponse";
        private const string HiddenFiledBrandId = "hfBrandID";
        private const string LinkPopupCommand = "lnkPopup_Command";
        private const string DataListDimensions = "dlDimensions";
        private const string DimensionArguments = "1|DIMENSION|Country Regions";
        private const string PubTypeArguments = "1|PUBTYPE|Country Regions";
        private const string CountryRegionsArguments = "1|Country Regions|Country Regions";
        private const string LabelDimensionControl = "lblDimensionControl";
        private const string LabelDimension = "lblDimension";
        private const string HiddelFieldMasterValue = "hfMasterValue";
        private const string LinkSortByDescription = "lnkSortByDescription";
        private const string LinkSortByValue = "lnkSortByValue";
        private const string RadTextBoxDimSearch = "rtbDimSearch";
        private const string RadTextBoxDimensionSelected = "rlbDimensionSelected";
        private const string RadTextBoxDimensionAvailable = "rlbDimensionAvailable";
        private const string Markets = "MARKETS";
        private const string DefaultText = "Unit Test";
        private Report _testEntity;

        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new Report();
            InitializePage(_testEntity);
            InitializeAllControls(_testEntity);
        }

        [TestCase(TestZero, DimensionArguments)]
        [TestCase(TestOne, DimensionArguments)]
        public void LinkPopupCommand_CommandArgumentIsDimensionType_UpdateControlValue(string brandId, string dimensionArgument)
        {
            // Arrange
            var commandName = TestOne;
            GetField<HiddenField>(HiddenFiledBrandId).Value = brandId;
            var argument = dimensionArgument;
            CreatePageShimObject(true);

            var commandEventArgs = new CommandEventArgs(commandName, argument);
            var parameters = new object[] { this, commandEventArgs };

            // Act
            PrivatePage.Invoke(LinkPopupCommand, parameters);

            // Assert
            var resultArgument = dimensionArgument.Split('|');
            GetField<Label>(LabelDimensionControl).Text.ShouldBe(TestOne);
            GetField<Label>(LabelDimension).Text.ShouldBe(resultArgument[2]);
            GetField<HiddenField>(HiddelFieldMasterValue).Value.ShouldBe(resultArgument[0]);
            GetField<LinkButton>(LinkSortByDescription).Visible.ShouldBeTrue();
            GetField<LinkButton>(LinkSortByValue).Visible.ShouldBeTrue();
            GetField<Telerik.Web.UI.RadTextBox>(RadTextBoxDimSearch).Visible.ShouldBeTrue();
            GetField<Telerik.Web.UI.RadListBox>(RadTextBoxDimensionSelected).Visible.ShouldBeTrue();
            var rlbDimensionAvailable = GetField<Telerik.Web.UI.RadListBox>(RadTextBoxDimensionAvailable);
            rlbDimensionAvailable.ShouldSatisfyAllConditions(
                () => rlbDimensionAvailable.AllowTransfer.ShouldBeTrue(),
                () => rlbDimensionAvailable.TransferToID.ShouldBe(RadTextBoxDimensionSelected),
                () => rlbDimensionAvailable.Width.ShouldBe(Unit.Pixel(465)),
                () => rlbDimensionAvailable.ButtonSettings.ShouldNotBeNull(),
                () => rlbDimensionAvailable.ButtonSettings.AreaWidth.ShouldBe(Unit.Pixel(35)));
        }

        [TestCase(TestZero, PubTypeArguments)]
        [TestCase(TestOne, PubTypeArguments)]
        public void LinkPopupCommand_CommandArgumentIsPubType_UpdateControlValue(string brandId, string pubTypeArgument)
        {
            // Arrange
            var commandName = TestOne;
            GetField<HiddenField>(HiddenFiledBrandId).Value = brandId;
            var argument = pubTypeArgument;
            CreatePageShimObject(true);

            var commandEventArgs = new CommandEventArgs(commandName, argument);
            var parameters = new object[] { this, commandEventArgs };

            // Act
            PrivatePage.Invoke(LinkPopupCommand, parameters);

            // Assert
            var resultArgument = pubTypeArgument.Split('|');
            GetField<Label>(LabelDimensionControl).Text.ShouldBe(TestOne);
            GetField<Label>(LabelDimension).Text.ShouldBe(resultArgument[2]);
            GetField<LinkButton>(LinkSortByDescription).Visible.ShouldBeFalse();
            GetField<LinkButton>(LinkSortByValue).Visible.ShouldBeFalse();
            GetField<Telerik.Web.UI.RadTextBox>(RadTextBoxDimSearch).Visible.ShouldBeTrue();
            GetField<Telerik.Web.UI.RadListBox>(RadTextBoxDimensionSelected).Visible.ShouldBeTrue();
            var rlbDimensionAvailable = GetField<Telerik.Web.UI.RadListBox>(RadTextBoxDimensionAvailable);
            rlbDimensionAvailable.ShouldSatisfyAllConditions(
                () => rlbDimensionAvailable.AllowTransfer.ShouldBeTrue(),
                () => rlbDimensionAvailable.TransferToID.ShouldBe(RadTextBoxDimensionSelected),
                () => rlbDimensionAvailable.Width.ShouldBe(Unit.Pixel(465)),
                () => rlbDimensionAvailable.ButtonSettings.ShouldNotBeNull(),
                () => rlbDimensionAvailable.ButtonSettings.AreaWidth.ShouldBe(Unit.Pixel(35)));
        }

        [TestCase(TestZero)]
        [TestCase(TestOne)]
        public void LinkPopupCommand_CommandArgumentIsCountryRegions_UpdateControlValue(string brandId)
        {
            // Arrange
            var commandName = Markets;
            GetField<HiddenField>(HiddenFiledBrandId).Value = brandId;
            var argument = Markets;
            CreatePageShimObject(true);

            var commandEventArgs = new CommandEventArgs(commandName, argument);
            var parameters = new object[] { this, commandEventArgs };

            // Act
            PrivatePage.Invoke(LinkPopupCommand, parameters);

            // Assert
            GetField<LinkButton>(LinkSortByDescription).Visible.ShouldBeFalse();
            GetField<LinkButton>(LinkSortByValue).Visible.ShouldBeFalse();
            GetField<Telerik.Web.UI.RadTextBox>(RadTextBoxDimSearch).Visible.ShouldBeTrue();
            GetField<Telerik.Web.UI.RadListBox>(RadTextBoxDimensionSelected).Visible.ShouldBeFalse();
            var rlbDimensionAvailable = GetField<Telerik.Web.UI.RadListBox>(RadTextBoxDimensionAvailable);
            rlbDimensionAvailable.ShouldSatisfyAllConditions(
                () => rlbDimensionAvailable.AllowTransfer.ShouldBeFalse(),
                () => rlbDimensionAvailable.TransferToID.ShouldBeNullOrEmpty(),
                () => rlbDimensionAvailable.Width.ShouldBe(Unit.Pixel(900)),
                () => rlbDimensionAvailable.ButtonSettings.ShouldNotBeNull(),
                () => rlbDimensionAvailable.ButtonSettings.AreaWidth.ShouldBe(Unit.Pixel(0)));
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

        private Control GetControlById(string controlId)
        {
            switch (controlId)
            {
                case PubTypeRepeater:
                    return new Repeater { ID = controlId };
                case PubTypeListBox:
                    return new ListBox { ID = controlId };
                case ListResponse:
                    return new ListBox { ID = controlId };
                case TestOne:
                    return null;
                case Markets:
                    return new ListBox { ID = controlId };
                case DataListDimensions:
                    return new DataList { ID = controlId };
                default:
                    return new Control { ID = controlId };
            }
        }

        private void CreatePageShimObject(bool isActive = false)
        {
            var pubList = CreatePubsListObject();
            var listMasterCodeSheet = CreateMasterCodeSheetListObject();
            var listMarkets = CreateMartesObject();
            ShimECNSession.Constructor = (instance) => { };
            var shimSession = new ShimECNSession { ClearSession = () => { } };
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
            shimSession.ClientIDGet = () => 1;
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            ShimReport.AllInstances.MasterGet = (x) =>
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
            ShimMasterCodeSheet.GetSearchEnabledByBrandIDClientConnectionsInt32 = (x, y) => listMasterCodeSheet;
            ShimMasterCodeSheet.GetSearchEnabledClientConnections = (x) => listMasterCodeSheet;
            ShimPubs.GetSearchEnabledByBrandIDClientConnectionsInt32 = (x, y) => pubList;
            ShimPubs.GetSearchEnabledClientConnections = (x) => pubList;
            ShimControl.AllInstances.FindControlString = (sender, controlId) => GetControlById(controlId);
            ShimRepeater.AllInstances.ItemsGet = (sender) => CreateRepeaterItemCollectionObject();
            ShimDataList.AllInstances.ItemsGet = (sender) => CreateDataListItemCollection();
            ShimMarkets.GetByBrandIDClientConnectionsInt32 = (clientconnections, brandId) => listMarkets;
            ShimMarkets.GetNotInBrandClientConnections = (clientconnections) => listMarkets;
        }

        private static List<Objects.Markets> CreateMartesObject()
        {
            return new List<Objects.Markets>
            {
                new Objects.Markets
                {
                    BrandID = 1,
                    BrandName = DefaultText,
                    MarketID = 1,
                    MarketName = DefaultText,
                    MarketXML = string.Empty
                },
                new Objects.Markets
                {
                    BrandID = 2,
                    BrandName = DefaultText,
                    MarketID = 2,
                    MarketName = DefaultText,
                    MarketXML = string.Empty
                }
            };

        }
        private static List<Pubs> CreatePubsListObject()
        {
            return new List<Pubs>
            {
                new Pubs
                {
                    PubID = 1,
                    PubName = DefaultText,
                    PubCode = DefaultText,
                    PubTypeID = 1,
                    EnableSearching = true
                }
            };
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
                    MasterDesc = DefaultText,
                    MasterDesc1 = DefaultText,
                    EnableSearching = true
                }
            };
        }
    }
}
