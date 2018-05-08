using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPS.MD.Main;
using KMPS.MD.MasterPages.Fakes;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using static KMPlatform.Enums;
using static KMPS.MD.Objects.Enums;
using ShimReport = KMPS.MD.Main.Fakes.ShimReport;
using ShimFrameworkUADLookUp = FrameworkUAD_Lookup.BusinessLogic.Fakes;
using ShimFilter = KMPS.MD.Objects.Fakes.ShimFilter;

namespace KMPS.MD.Tests.AudienceViews
{
    /// <summary>
    /// Unit test for <see cref="Report"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ReportTestsButtonDcDownloadClick : BasePageTests
    {
        private const string TestOne = "1";
        private const string TestTwo = "2";
        private const string TestZero = "0";
        private const string DefaultText = "Unit Test";
        private const string TestTrueString = "true";
        private const string TestFalseString = "false";
        private const string ButtonDcDownloadClick = "btnDCDownload_Click";
        private const string HiddenFiledBrandId = "hfBrandID";
        private const string HiddenFiledDcTargetCodeId = "hfDcTargetCodeID";
        private const string HiddenFiledDataCompareProcessCode = "hfDataCompareProcessCode";
        private const string HiddenFiledDcRunId = "hfDcRunID";
        private const string LinkNonMatchedRecords = "lnkNonMatchedRecords";
        private const string PlaceHolderKmStaff = "plKmStaff";
        private const string DropDownListDataCompareSourceFile = "drpDataCompareSourceFile";
        private const string DropDownListIsBillable = "drpIsBillable";
        private const string LinkMatchedRecords = "lnkMatchedRecords";
        private const string PlaceholderNotes = "plNotes";
        private const string LabelTotalFileRecords = "lblTotalFileRecords";
        private Report _testEntity;
        private bool _saveDataCompareDownload;
        private bool _selectCodeName;
        private bool _selectForRun;
        private bool _getDataCompareCost;
        private bool _downloadDataCompare;
        private bool _saveDataCompareView;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new Report();
            InitializePage(_testEntity);
            InitializeAllControls(_testEntity);
            ShimPage.AllInstances.IsValidGet = (sender) => true;
            ShimPage.AllInstances.ServerGet = (sender) =>
                new ShimHttpServerUtility
                {
                    MapPathString = (x) => x
                };
        }

        [TestCase(TestOne)]
        [TestCase(TestTwo)]
        [TestCase(TestZero)]
        public void ButtonDCDownloadClick_PageIsValidAndSourceFileDropDownHaveSelectedValue_UpdateControlValues(string brandId)
        {
            // Arrange
            CreatePageShimObject(true, int.Parse(brandId));
            SetPagePrivateObject(brandId);
            var drpIsBillable = GetField<DropDownList>(DropDownListIsBillable);
            drpIsBillable.DataSource = new[] { TestTrueString, TestFalseString };
            drpIsBillable.SelectedValue = TestTrueString;
            drpIsBillable.DataBind();

            var parameters = new object[] { this, EventArgs.Empty };

            // Act
            PrivatePage.Invoke(ButtonDcDownloadClick, parameters);

            // Assert
            _saveDataCompareDownload.ShouldBeTrue();
            _selectCodeName.ShouldBeTrue();
            _selectForRun.ShouldBeTrue();
            _getDataCompareCost.ShouldBeTrue();
            _downloadDataCompare.ShouldBeTrue();
            _saveDataCompareView.ShouldBeTrue();
        }

        [TestCase(TestTwo, TestTrueString)]
        [TestCase(TestTwo, TestFalseString)]
        public void ButtonDCDownloadClick_PageIsValidAndIdNotExistInDataCompareView_UpdateControlValues(string brandId, string selectedVale)
        {
            // Arrange
            var id = int.Parse(brandId);
            CreatePageShimObject(true, id);
            SetPagePrivateObject(brandId);
            var drpIsBillable = GetField<DropDownList>(DropDownListIsBillable);
            drpIsBillable.DataSource = new string[] { TestTrueString, TestFalseString };
            drpIsBillable.SelectedValue = selectedVale;
            drpIsBillable.DataBind();
            ShimDataCompareView.AllInstances.SelectForRunInt32 = (sender, dcRunId) =>
            {
                _selectForRun = true;
                return new List<DataCompareView>
                {
                    new DataCompareView {
                        DcTargetCodeId = id,
                        PaymentStatusId = id,
                        DcTargetIdUad= id,
                        DcTypeCodeId = 1,
                        IsBillable= bool.Parse(selectedVale)
                    }
                };
            };
            ShimDataCompareView.AllInstances.SaveDataCompareView = (sender, dataObject) => 1;
            var parameters = new object[] { this, EventArgs.Empty };

            // Act
            PrivatePage.Invoke(ButtonDcDownloadClick, parameters);

            // Assert
            _selectCodeName.ShouldBeTrue();
            _selectForRun.ShouldBeTrue();
            _downloadDataCompare.ShouldBeTrue();
        }

        private void SetPagePrivateObject(string brandId)
        {
            GetField<HiddenField>(HiddenFiledBrandId).Value = brandId;
            GetField<HiddenField>(HiddenFiledDcTargetCodeId).Value = brandId;
            GetField<HiddenField>(HiddenFiledDataCompareProcessCode).Value = brandId;
            GetField<HiddenField>(HiddenFiledDcRunId).Value = brandId;
            GetField<LinkButton>(LinkNonMatchedRecords).Text = brandId;
            GetField<LinkButton>(LinkMatchedRecords).Text = brandId;
            GetField<PlaceHolder>(PlaceHolderKmStaff).Visible = true;
            GetField<PlaceHolder>(PlaceholderNotes).Visible = true;
            GetField<Label>(LabelTotalFileRecords).Text = brandId;
            var drpDataCompareSourceFile = GetField<DropDownList>(DropDownListDataCompareSourceFile);
            drpDataCompareSourceFile.DataSource = Enumerable.Range(0, 10).ToArray();
            drpDataCompareSourceFile.SelectedValue = brandId;
            drpDataCompareSourceFile.DataBind();
        }

        private void CreatePageShimObject(bool isActive, int brandId)
        {
            var shimSession = CreateShimECNSessionObject(isActive);
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            CreateMasterPageShimObject(shimSession);
            ShimDataCompareProfile.AllInstances.GetDataCompareDataClientConnectionsStringInt32StringInt32 =
                (sender, client, processCode, dcTargetCodeId, matchType, id) => new DataTable();

            ShimFrameworkUADLookUp.ShimCode.AllInstances.SelectCodeNameEnumsCodeTypeString = (sender, codeType, codeName) =>
            {
                _selectCodeName = true;
                return new Code
                {
                    CodeId = 1,
                    CodeValue = TestOne
                };
            };
            ShimFilter.AllInstances.ExecuteClientConnectionsString = (sender, clientconnections, filter) => { };
            ShimCode.GetDataCompareTarget = () =>
            {
                return new List<Objects.Code> {
                new Objects.Code { CodeName = DataCompareViewType.Consensus.ToString(), CodeID = 1 },
                new Objects.Code { CodeName = DataCompareViewType.Consensus.ToString(), CodeID = 2 }};
            };
            ShimDataCompareView.AllInstances.SelectForRunInt32 = (sender, dcRunId) =>
            {
                _selectForRun = true;
                return new List<DataCompareView>
                {
                    new DataCompareView {
                        DcTargetCodeId = brandId,
                        PaymentStatusId = brandId
                    }
                };
            };
            ShimDataCompareView.AllInstances.GetDataCompareCostInt32Int32Int32EnumsDataCompareTypeEnumsDataCompareCost =
                (sender, userId, clientId, count, dcType, mergePurgeorDownload) =>
                {
                    _getDataCompareCost = true;
                    return 100;
                };
            ShimDataCompareDownload.AllInstances.SaveDataCompareDownload = (sender, dataCompareDownloadObject) =>
            {
                _saveDataCompareDownload = true;
                return 1;
            };
            ShimReport.AllInstances.saveDataCompareViewInt32NullableOfInt32Int32Int32 = (x, y, z, v, b) =>
            {
                _saveDataCompareView = true;
                return 1;
            };
            ShimUtilities.DownloadDataCompareInt32DataTableString = (x, y, z) => _downloadDataCompare = true;
            ShimDataCompareView.AllInstances.DeleteInt32 = (sender, id) => true;
        }

        private static void CreateMasterPageShimObject(ShimECNSession shimSession)
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

        private static ShimECNSession CreateShimECNSessionObject(bool isActive)
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
                }
            };
        }
    }
}
