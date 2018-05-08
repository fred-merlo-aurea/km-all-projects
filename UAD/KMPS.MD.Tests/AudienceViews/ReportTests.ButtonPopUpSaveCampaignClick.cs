using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using KMPlatform.Entity;
using KMPS.MD.Main;
using KMPS.MD.MasterPages.Fakes;
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
    public class ReportTestsButtonPopUpSaveCampaignClick : BasePageTests
    {
        private const string TestOne = "1";
        private const string TestZero = "0";
        private const string ErrorMessage = "Please select a checkbox.";
        private const string CampaignErroMessage = "Please enter a different name.";
        private const string NoCampainExist = "Select existing Campaign or new Campaign";
        private const string Id = "ID";
        private const string Count = "Count";
        private const string Desc = "Desc";
        private const string SubscriptionId = "SubscriptionID";
        private const string Column1 = "Column1";
        private const string Column2 = "Column2";
        private const string PopupCampaignId = "PopupCampaignID";
        private const string LabelPopupResult = "lblPopupResult";
        private const string ButtonPopUpSaveCampaignClick = "btnPopupSaveCampaign_click";
        private const string DefaultText = "Unit Test";
        private const string HiddenFiledBrandId = "hfBrandID";
        private const string GridViewSubReport = "grdSubReport";
        private const string CheckBoxSelectDownload = "cbSelectDownload";
        private const string RadioButtonPopupExistingCampaign = "rbPopupExistingCampaign";
        private const string DropDownListPopupCampaignName = "drpPopupCampaignName";
        private const string TextPromocode = "txtPromocode";
        private const string ViewType = "ViewType";
        private const string RadioButtonPopupNewCampaign = "rbPopupNewCampaign";
        private bool _getClientSqlConnection;
        private Report _testEntity;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new Report();
            InitializePage(_testEntity);
            InitializeAllControls(_testEntity);
            QueryString.Clear();
        }

        [TestCase(TestOne, Objects.Enums.ViewType.ConsensusView, true)]
        [TestCase(TestZero, Objects.Enums.ViewType.ConsensusView, false)]
        [TestCase(TestOne, Objects.Enums.ViewType.RecencyView, true)]
        [TestCase(TestZero, Objects.Enums.ViewType.RecencyView, false)]
        public void ButtonPopUpSaveCampaignClick_GridSubReportHaveRowCount_UpdatePageControlValue(string brandId, Objects.Enums.ViewType viewType, bool selectedValue)
        {
            // Arrange
            QueryString.Add(ViewType, viewType.ToString());
            GetField<HiddenField>(HiddenFiledBrandId).Value = brandId;
            GetField<TextBox>(TextPromocode).Text = brandId;
            GetField<RadioButton>(RadioButtonPopupExistingCampaign).Checked = selectedValue;
            GetField<RadioButton>(RadioButtonPopupNewCampaign).Checked = !selectedValue;
            var drpIsBillable = GetField<DropDownList>(DropDownListPopupCampaignName);
            drpIsBillable.DataSource = Enumerable.Range(0, 5).ToArray();
            drpIsBillable.SelectedValue = brandId;
            drpIsBillable.DataBind();
            CreatePageShimObject(true);
            var parameters = new object[] { this, EventArgs.Empty };
            var dataTable = CreateDataTable();
            var grdSubReport = GetField<GridView>(GridViewSubReport);
            grdSubReport.DataSource = dataTable;
            grdSubReport.DataBind();
            grdSubReport.Rows[0].Cells[4].Controls.Add(new CheckBox { ID = CheckBoxSelectDownload, Checked = true });
            grdSubReport.Rows[1].Cells[4].Controls.Add(new CheckBox { ID = CheckBoxSelectDownload, Checked = true });
            ShimSubscriber.GetSubscriberDataClientConnectionsStringBuilderString = (x, y, z) => dataTable;
            ShimCampaigns.CampaignExistsClientConnectionsString = (x, y) => 0;

            // Act
            PrivatePage.Invoke(ButtonPopUpSaveCampaignClick, parameters);

            // Assert
            _getClientSqlConnection.ShouldSatisfyAllConditions(
                () => _getClientSqlConnection.ShouldBeTrue(),
                () => GetField<Label>(LabelPopupResult).Visible.ShouldBeTrue(),
                () => PrivatePage.GetFieldOrProperty(PopupCampaignId).ShouldBe(0));
        }

        [Test]
        public void ButtonPopUpSaveCampaignClick_SelectDownloadIsNotChecked_ReturnMessage()
        {
            // Arrange
            var messageResult = string.Empty;
            CreatePageShimObject(true);
            var parameters = new object[] { this, EventArgs.Empty };
            var dataTable = CreateDataTable();
            var grdSubReport = GetField<GridView>(GridViewSubReport);
            grdSubReport.DataSource = dataTable;
            grdSubReport.DataBind();
            grdSubReport.Rows[0].Cells[4].Controls.Add(new CheckBox { ID = CheckBoxSelectDownload, Checked = false });
            grdSubReport.Rows[1].Cells[4].Controls.Add(new CheckBox { ID = CheckBoxSelectDownload, Checked = false });
            ShimReport.AllInstances.DisplaySaveCampaignPopupErrorString = (sender, message) => messageResult = message;

            // Act
            PrivatePage.Invoke(ButtonPopUpSaveCampaignClick, parameters);

            // Assert
            messageResult.ShouldSatisfyAllConditions(
                () => messageResult.ShouldNotBeNullOrEmpty(),
                () => messageResult.ShouldBe(ErrorMessage));
        }

        [Test]
        public void ButtonPopUpSaveCampaignClick_CampaignAlreadyExist_ReturnErrorMessage()
        {
            // Arrange
            var messageResult = string.Empty;
            var brandId = TestZero;
            var selectedValue = false;
            QueryString.Add(ViewType, Objects.Enums.ViewType.RecencyView.ToString());
            GetField<HiddenField>(HiddenFiledBrandId).Value = brandId;
            GetField<TextBox>(TextPromocode).Text = brandId;
            GetField<RadioButton>(RadioButtonPopupExistingCampaign).Checked = selectedValue;
            GetField<RadioButton>(RadioButtonPopupNewCampaign).Checked = selectedValue;
            var drpIsBillable = GetField<DropDownList>(DropDownListPopupCampaignName);
            drpIsBillable.DataSource = Enumerable.Range(0, 5).ToArray();
            drpIsBillable.SelectedValue = brandId;
            drpIsBillable.DataBind();
            CreatePageShimObject(true);
            var parameters = new object[] { this, EventArgs.Empty };
            var dataTable = CreateDataTable();
            var grdSubReport = GetField<GridView>(GridViewSubReport);
            grdSubReport.DataSource = dataTable;
            grdSubReport.DataBind();
            grdSubReport.Rows[0].Cells[4].Controls.Add(new CheckBox { ID = CheckBoxSelectDownload, Checked = true });
            grdSubReport.Rows[1].Cells[4].Controls.Add(new CheckBox { ID = CheckBoxSelectDownload, Checked = true });
            ShimSubscriber.GetSubscriberDataClientConnectionsStringBuilderString = (x, y, z) => dataTable;
            ShimCampaigns.CampaignExistsClientConnectionsString = (x, y) => 1;
            ShimReport.AllInstances.DisplaySaveCampaignPopupErrorString = (sender, message) => messageResult = message;

            // Act
            PrivatePage.Invoke(ButtonPopUpSaveCampaignClick, parameters);

            // Assert
            messageResult.ShouldSatisfyAllConditions(
                () => messageResult.ShouldNotBeNullOrEmpty(),
                () => messageResult.ShouldContain(NoCampainExist));
        }

        [TestCase(TestZero, Objects.Enums.ViewType.RecencyView, true)]
        public void ButtonPopUpSaveCampaignClick_CampaignAlreadyExist_ReturnErrorMessage(string brandId, Objects.Enums.ViewType viewType, bool selectedValue)
        {
            // Arrange
            var messageResult = string.Empty;
            QueryString.Add(ViewType, viewType.ToString());
            GetField<HiddenField>(HiddenFiledBrandId).Value = brandId;
            GetField<TextBox>(TextPromocode).Text = brandId;
            GetField<RadioButton>(RadioButtonPopupExistingCampaign).Checked = !selectedValue;
            GetField<RadioButton>(RadioButtonPopupNewCampaign).Checked = selectedValue;
            var drpIsBillable = GetField<DropDownList>(DropDownListPopupCampaignName);
            drpIsBillable.DataSource = Enumerable.Range(0, 5).ToArray();
            drpIsBillable.SelectedValue = brandId;
            drpIsBillable.DataBind();
            CreatePageShimObject(true);
            var parameters = new object[] { this, EventArgs.Empty };
            var dataTable = CreateDataTable();
            var grdSubReport = GetField<GridView>(GridViewSubReport);
            grdSubReport.DataSource = dataTable;
            grdSubReport.DataBind();
            grdSubReport.Rows[0].Cells[4].Controls.Add(new CheckBox { ID = CheckBoxSelectDownload, Checked = true });
            grdSubReport.Rows[1].Cells[4].Controls.Add(new CheckBox { ID = CheckBoxSelectDownload, Checked = true });
            ShimSubscriber.GetSubscriberDataClientConnectionsStringBuilderString = (x, y, z) => dataTable;
            ShimCampaigns.CampaignExistsClientConnectionsString = (x, y) => 1;
            ShimReport.AllInstances.DisplaySaveCampaignPopupErrorString = (sender, message) => messageResult = message;

            // Act
            PrivatePage.Invoke(ButtonPopUpSaveCampaignClick, parameters);

            // Assert
            messageResult.ShouldSatisfyAllConditions(
                () => messageResult.ShouldNotBeNullOrEmpty(),
                () => messageResult.ShouldContain(CampaignErroMessage));
        }

        private static DataTable CreateDataTable()
        {
            var dataTable = new DataTable(GridViewSubReport);
            dataTable.Columns.Add(new DataColumn(Id));
            dataTable.Columns.Add(new DataColumn(Count));
            dataTable.Columns.Add(new DataColumn(Desc));
            dataTable.Columns.Add(SubscriptionId, typeof(string));
            dataTable.Columns.Add(Column1, typeof(string));
            dataTable.Columns.Add(Column2, typeof(string));
            dataTable.Rows.Add(1, 1, "1", "1", string.Empty, string.Empty);
            dataTable.Rows.Add(1, 1, "1", "1", string.Empty, string.Empty);
            return dataTable;
        }

        private void CreatePageShimObject(bool isActive)
        {
            var shimSession = CreateShimEcnSessionObject(isActive);
            CreateMasterPageShimObject(shimSession);
            ShimDataFunctions.GetClientSqlConnectionClientConnections = (x) =>
            {
                _getClientSqlConnection = true;
                return new SqlConnection();
            };

            ShimGridView.AllInstances.DataKeysGet = (sender) =>
            {
                var orderDictionary = new OrderedDictionary
                {
                    { Id,TestOne},
                    { Count,TestOne},
                     { Desc,TestOne}
                };
                var arrayList = new ArrayList
                 {
                    new DataKey(orderDictionary),
                    new DataKey(orderDictionary)
                };
                return new DataKeyArray(arrayList);
            };
            ShimFilter.generateCombinationQueryFiltersStringStringStringStringStringInt32Int32ClientConnections =
                (fc, selectedFilterOperation, suppressedFilterOperation, selectedFilterNo, suppressedFilterNo, addlFilters, pubID, brandID, clientconnection) =>
                new System.Text.StringBuilder();

            ShimCampaignFilter.CampaignFilterExistsClientConnectionsStringInt32 = (x, y, z) => 0;
            ShimCampaignFilter.InsertClientConnectionsStringInt32Int32String = (x, y, z, m, n) => 1;
            ShimCampaignFilterDetails.saveCampaignDetailsClientConnectionsInt32String = (clientconnection, campaignFilterId, xmlSubscriber) => { };
            ShimCampaigns.GetCountByCampaignIDClientConnectionsInt32 = (x, y) => 2;
            ShimHttpRequest.AllInstances.QueryStringGet = _ => QueryString;
            ShimCampaigns.InsertClientConnectionsStringInt32Int32 = (x, y, z, m) => 0;
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
                }
            };
        }
    }
}
