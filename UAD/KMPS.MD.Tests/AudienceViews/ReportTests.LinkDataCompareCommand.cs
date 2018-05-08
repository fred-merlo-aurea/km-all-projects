﻿using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using KMPS.MD.Controls;
using KMPS.MD.Controls.Fakes;
using KMPS.MD.Main;
using KMPS.MD.MasterPages.Fakes;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using static KMPlatform.Enums;
using FrameworkUADLookupEntity = FrameworkUAD_Lookup.Entity;
using FrameworkUADShimCode = FrameworkUAD_Lookup.BusinessLogic.Fakes.ShimCode;
using ShimReport = KMPS.MD.Main.Fakes.ShimReport;

namespace KMPS.MD.Tests.AudienceViews
{
    /// <summary>
    /// Unit test for <see cref="Report"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ReportTestsLinkDataCompareCommand : BasePageTests
    {
        private const string TestOne = "1";
        private const string TestZero = "0";
        private const string LinkDataCompareCommand = "lnkDataCompare_Command";
        private const string HiddenFiledBrandId = "hfBrandID";
        private const string HiddenFiledDcRunId = "hfDcRunID";
        private const string HiddenFieldDcTargetCodeId = "hfDcTargetCodeID";
        private const string LinkButtonMatchedRecords = "lnkMatchedRecords";
        private const string LinkButtonNonMatchedRecords = "lnkNonMatchedRecords";
        private const string LabelTotalFileRecords = "lblTotalFileRecords";
        private const string DefaultText = "Unit Test";
        private const string DownloadPanel = "DownloadPanel1";
        private const string Id = "ID";
        private const string Count = "Count";
        private const string Desc = "Desc";
        private const string SubscriptionID = "SubscriptionID";
        private const string Column1 = "Column1";
        private const string Column2 = "Column2";
        private const string GridViewSubReport = "grdSubReport";
        private const string MatchedRecords = "MATCHEDRECORDS";
        private const string MatchDoNotInBrand = "MATCHEDNOTINBRAND";
        private const string NoMatchRecords = "NONMATCHEDRECORDS";
        private const string ButtonDcDownload = "btnDCDownload";
        private const string ButtonDcDownloadText = "return confirmPopupPurchase();";
        private const string PanelKmStaff = "plKmStaff";
        private const string PanelNotes = "plNotes";
        private const string DropDownListIsBillable = "drpIsBillable";

        private Report _testEntity;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new Report();
            InitializePage(_testEntity);
            InitializeAllControls(_testEntity);
        }

        [TestCase(TestOne, MatchedRecords)]
        [TestCase(TestZero, MatchedRecords)]
        [TestCase(TestOne, MatchDoNotInBrand)]
        [TestCase(TestZero, MatchDoNotInBrand)]
        public void LinkDataCompareCommand_CommandArgumentIsNotNull_AppendQueriesToDownloadPanel(string testCase, string commandArgument)
        {
            // Arrange
            int matchId = int.Parse(testCase);
            var commandName = string.Empty;
            var commandEventArgs = new CommandEventArgs(commandName, commandArgument);
            var parameters = new object[] { this, commandEventArgs };
            SetPagePrivateFiledValue(testCase);
            CreatePageShimObject(true);
            FrameworkUADShimCode.AllInstances.SelectCodeNameEnumsCodeTypeString =
                (x, y, z) =>
                new FrameworkUADLookupEntity.Code { CodeId = matchId, CodeName = testCase, IsActive = true };
            ShimDataCompareView.AllInstances.SelectForRunInt32 = (sender, dcRunId) => CreateDataCompareViewObject(matchId);

            // Act
            PrivatePage.Invoke(LinkDataCompareCommand, parameters);

            // Assert
            var downloadPanel = GetField<DownloadPanel>(DownloadPanel);
            downloadPanel.ShouldSatisfyAllConditions(
                () => downloadPanel.SubscriptionID.ShouldBeNull(),
                () => downloadPanel.SubscribersQueries.ShouldNotBeNull(),
                () => downloadPanel.Visible.ShouldBeTrue(),
                () => downloadPanel.HeaderText.ShouldBeNullOrEmpty(),
                () => downloadPanel.ShowHeaderCheckBox.ShouldBeFalse(),
                () => downloadPanel.PubIDs.ShouldBeNull(),
                () => downloadPanel.BrandID.ShouldBe(matchId),
                () => downloadPanel.VisibleCbIsRecentData.ShouldBeFalse(),
                () => downloadPanel.dcRunID.ShouldBe(matchId),
                () => downloadPanel.dcTargetCodeID.ShouldBe(matchId),
                () => downloadPanel.matchedRecordsCount.ShouldBe(matchId),
                () => downloadPanel.nonMatchedRecordsCount.ShouldBe(matchId),
                () => downloadPanel.TotalFileRecords.ShouldBe(matchId),
                () => downloadPanel.dcTypeCodeID.ShouldBe(matchId));
        }

        [TestCase(TestOne, NoMatchRecords, true)]
        [TestCase(TestZero, NoMatchRecords, false)]
        public void LinkDataCompareCommand_CommandArgumentIsNoMatchRecords_AppendQueriesToDownloadPanel(string testCase, string commandArgument, bool IsKMStaff)
        { 
            // Arrange
            var matchId = int.Parse(testCase);
            var commandName = string.Empty;
            SetPagePrivateFiledValue(testCase);
            var commandEventArgs = new CommandEventArgs(commandName, commandArgument);
            var parameters = new object[] { this, commandEventArgs };
            CreatePageShimObject(IsKMStaff);
            ShimDataCompareView.AllInstances.SelectForRunInt32 = (sender, dcRunId) => CreateDataCompareViewObject(matchId);

            // Act
            PrivatePage.Invoke(LinkDataCompareCommand, parameters);

            // Assert
            if (IsKMStaff)
            {
                GetField<Button>(ButtonDcDownload).OnClientClick.ShouldBe(ButtonDcDownloadText);
                GetField<PlaceHolder>(PanelKmStaff).Visible.ShouldBeTrue();
                GetField<PlaceHolder>(PanelNotes).Visible.ShouldBeFalse();
                GetField<DropDownList>(DropDownListIsBillable).Enabled.ShouldBeFalse();
            }
            else
            {
                GetField<Button>(ButtonDcDownload).OnClientClick.ShouldBeNullOrEmpty();
                GetField<PlaceHolder>(PanelKmStaff).Visible.ShouldBeFalse();
                GetField<PlaceHolder>(PanelNotes).Visible.ShouldBeFalse();
            }
        }

        private static List<DataCompareView> CreateDataCompareViewObject(int matchId)
        {
            return new List<DataCompareView>
            {
                new DataCompareView
                {
                    DcTargetCodeId = matchId,
                    DcTargetIdUad = matchId,
                    IsBillable = true
                }
            };
        }

        private static DataTable CreateDataTable()
        {
            var dataTable = new DataTable(GridViewSubReport);
            dataTable.Columns.Add(new DataColumn(Id));
            dataTable.Columns.Add(new DataColumn(Count));
            dataTable.Columns.Add(new DataColumn(Desc));
            dataTable.Columns.Add(SubscriptionID, typeof(string));
            dataTable.Columns.Add(Column1, typeof(string));
            dataTable.Columns.Add(Column2, typeof(string));
            dataTable.Rows.Add(1, 1, "1", "1", string.Empty, string.Empty);
            dataTable.Rows.Add(1, 1, "1", "1", string.Empty, string.Empty);
            return dataTable;
        }

        private void SetPagePrivateFiledValue(string testCase)
        {
            GetField<HiddenField>(HiddenFiledBrandId).Value = testCase;
            GetField<HiddenField>(HiddenFiledDcRunId).Value = testCase;
            GetField<HiddenField>(HiddenFieldDcTargetCodeId).Value = testCase;
            GetField<LinkButton>(LinkButtonMatchedRecords).Text = testCase;
            GetField<LinkButton>(LinkButtonNonMatchedRecords).Text = testCase;
            GetField<Label>(LabelTotalFileRecords).Text = testCase;
        }

        private void CreatePageShimObject(bool isActive)
        {
            var shimSession = CreateShimEcnSessionObject(isActive);
            CreateMasterPageObject(shimSession);
            ShimDownloadPanel.AllInstances.LoadDownloadTemplate = (sender) => { };
            ShimDownloadPanel.AllInstances.loadExportFields = (sender) => { };
            ShimDownloadPanel.AllInstances.ValidateExportPermission = (sender) => { };
            ShimCode.GetDataCompareTarget = () => new List<Objects.Code>();
            ShimDataCompareProfile.AllInstances.GetDataCompareDataClientConnectionsStringInt32StringInt32 =
                (sender, client, processCode, dcTargetCodeId, matchType, id) => CreateDataTable();
        }

        private static void CreateMasterPageObject(ShimECNSession shimSession)
        {
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
        }

        private static ShimECNSession CreateShimEcnSessionObject(bool isActive)
        {
            ShimECNSession.Constructor = (instance) => { };
            var shimSession = new ShimECNSession
            {
                ClearSession = () => { },
                ClientIDGet = () => 1,
                UserIDGet = () => 1,
                BaseChannelIDGet = () => 1,
                ClientGroupIDGet = () => 1,
                CustomerIDGet = () => 1,
            };
            shimSession.Instance.CurrentUser = CreateUserObject(isActive, shimSession);
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            return shimSession;
        }

        private static User CreateUserObject(bool isActive, ShimECNSession shimSession)
        {
            return new User
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
                CurrentClient = new Client
                {
                    ClientTestDBConnectionString = string.Empty,
                    ClientLiveDBConnectionString = string.Empty
                },
                IsKMStaff = isActive
            };
        }
    }
}