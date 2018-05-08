using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Fakes;
using System.Web.SessionState.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using KM.Integration.Marketo;
using KM.Integration.Marketo.Process.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPlatform.Object;
using KMPS.MD.Controls.Fakes;
using KMPS.MD.MasterPages.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using KMPSEnum = KMPS.MD.Objects.Enums;
using DownloadPanel = KMPS.MD.Controls.DownloadPanel;
using ShimFrameworkUad = FrameworkUAD_Lookup.BusinessLogic.Fakes;
using KmpsFake = KMPS.MD.Objects.Fakes;
using static FrameworkUAD_Lookup.Enums;
using static KMPlatform.Enums;

namespace KMPS.MD.Tests.Controls
{
    /// <summary>
    /// Unit test for <see cref="DownloadPanel"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DownloadPanelTestsDetailsDownload : BaseControlTests
    {
        private const string DetailsDownload = "DetailsDownload";
        private const string RadioButtonMarketo = "rbMarketo";
        private const string RadioButtonDownload = "rbDownload";
        private const string RadioButtonGroupExport = "rbGroupExport";
        private const string RadioButtonCampaign = "rbCampaign";
        private const string RadioButtonExistingGroup = "rbExistingGroup";
        private const string RadioGroupNewGroup = "rbNewGroup";
        private const string DefaultText = "Unit Test";
        private const string GridViewHttpPost = "gvHttpPost";
        private const string LabelHttpPostParamsId = "lblHttpPostParamsID";
        private const string LabelParamName = "lblParamName";
        private const string LabelParamValue = "lblParamValue";
        private const string LabelCustomValue = "lblCustomValue";
        private const string LabelParamDisplayName = "lblParamDisplayName";
        private const string TextTotalCount = "txtTotalCount";
        private const string TextDownloadCount = "txtDownloadCount";
        private const string ListBoxSelectedFields = "lstSelectedFields";
        private const string DropDownListClient = "drpClient";
        private const string DropDownListExistingGroupName = "drpExistingGroupName";
        private const string DropDownListFolder = "drpFolder1";
        private const string FilterCombination = "Matched NotIn Selected";
        private const string DropDownListIsBillable = "drpIsBillable";
        private const string TestZero = "0";
        private const string TestOne = "1";
        private const string Id = "ID";
        private const string Count = "Count";
        private const string Desc = "Desc";
        private const string SubscriptionID = "SubscriptionID";
        private const string Column1 = "Column1";
        private const string Column2 = "Column2";
        private const string ResultsGrid = "ResultsGrid";
        private const string PlaceHolderKmStaff = "plKmStaff";
        private const string RadioButtonExistingCampaign = "rbExistingCampaign";
        private const string DropDownListExistingCampaign = "drpExistingCampaign";
        private const string RadioButtonNewCampaign = "rbNewCampaign";
        private const string DropDownFolder = "drpFolder";
        private const string TextBoxMarketoBaseUrl = "txtMarketoBaseURL";
        private const string TextBoxMarketoClientId = "txtMarketoClientID";
        private const string TextBoxMarketoClientSecret = "txtMarketoClientSecret";
        private const string TextBoxMarketoPartition = "txtMarketoPartition";
        private const string DropDownListMarketoList = "ddlMarketoList";
        private const string DemoText = "Demo1/";
        private const string Type = "Type";
        private const string Action = "Action";
        private const string Totals = "Totals";
        private const string DefaultSplitterText = "1|Unit Test";
        private readonly string TrueString = bool.TrueString.ToLower();
        private readonly string FalseString = bool.FalseString.ToLower();
        private DownloadPanel _downloadPanel;
        private bool _getSubscriberData;
        private bool _maskData;
        private bool _downloadData;
        private bool _getByClientID;
        private bool _exportToEcn;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _downloadPanel = new DownloadPanel();
            InitializeUserControl(_downloadPanel);
            InitializeAllControls(_downloadPanel);
        }

        [TestCase(KMPSEnum.ViewType.ProductView, RadioButtonMarketo, true, 1)]
        [TestCase(KMPSEnum.ViewType.ConsensusView, RadioButtonMarketo, true, 1)]
        [TestCase(KMPSEnum.ViewType.ProductView, RadioButtonDownload, true, 1)]
        [TestCase(KMPSEnum.ViewType.ConsensusView, RadioButtonDownload, true, 1)]
        [TestCase(KMPSEnum.ViewType.ProductView, RadioButtonDownload, true, 0)]
        [TestCase(KMPSEnum.ViewType.ConsensusView, RadioButtonDownload, false, 1)]
        [TestCase(KMPSEnum.ViewType.ConsensusView, RadioButtonDownload, false, 0)]
        public void DetailsDownload_MarketoIsEnabled_UpdatedControlValues(KMPSEnum.ViewType viewType, string controlName, bool selectedVale, int dcRunId)
        {
            // Arrange
            CreatePageShimObject(true);
            var action = CreateDelMethodActionResult();
            var resultIntegerList = Enumerable.Range(0, 10).ToList();
            _downloadPanel.SubscriptionID = resultIntegerList;
            _downloadPanel.DelMethod = action;
            _downloadPanel.ViewType = viewType;
            _downloadPanel.PubIDs = resultIntegerList;
            _downloadPanel.BrandID = 1;
            _downloadPanel.dcRunID = dcRunId;
            _downloadPanel.filterCombination = FilterCombination;
            GetField<RadioButton>(controlName).Checked = selectedVale;
            GetField<RadioButton>(RadioButtonGroupExport).Checked = true;
            GetField<RadioButton>(RadioButtonCampaign).Checked = selectedVale;
            if (!selectedVale)
            {
                GetField<RadioButton>(RadioGroupNewGroup).Checked = true;
            }
            GetField<RadioButton>(RadioButtonExistingGroup).Checked = selectedVale;
            GetField<TextBox>(TextTotalCount).Text = TestZero;
            GetField<TextBox>(TextDownloadCount).Text = TestZero;
            var lstSelectedFields = GetField<ListBox>(ListBoxSelectedFields);
            lstSelectedFields.DataSource = resultIntegerList;
            lstSelectedFields.SelectedValue = TestOne;
            lstSelectedFields.DataBind();
            var drpClient = GetField<DropDownList>(DropDownListClient);
            drpClient.DataSource = resultIntegerList;
            drpClient.SelectedValue = TestOne;
            drpClient.DataBind();

            var drpExistingGroupName = GetField<DropDownList>(DropDownListExistingGroupName);
            drpExistingGroupName.DataSource = resultIntegerList;
            drpExistingGroupName.SelectedValue = TestOne;
            drpExistingGroupName.DataBind();
            var drpFolder = GetField<DropDownList>(DropDownListFolder);
            drpFolder.DataSource = resultIntegerList;
            drpFolder.SelectedValue = TestOne;
            drpFolder.DataBind();
            var dropDownListFolder = GetField<DropDownList>(DropDownFolder);
            dropDownListFolder.DataSource = resultIntegerList;
            dropDownListFolder.SelectedValue = TestOne;
            dropDownListFolder.DataBind();
            var resultsGrid = GetField<DataGrid>(ResultsGrid);
            var dataview = new DataView(CreateDataTable());
            resultsGrid.Columns.Add(CreateBoundColumn(Id, TestOne));
            resultsGrid.Columns.Add(CreateBoundColumn(Count, TestOne));
            resultsGrid.Columns.Add(CreateBoundColumn(Desc, TestOne));
            resultsGrid.DataSource = dataview;
            resultsGrid.DataBind();
            FindPageControl();
            ShimGroups.ExistsByGroupNameByCustomerIDStringInt32 = (x, y) => selectedVale;

            // Act
            PrivateControl.Invoke(DetailsDownload);

            // Assert
            _getByClientID.ShouldBeTrue();
            _exportToEcn.ShouldBeTrue();

        }

        [TestCase(KMPSEnum.ViewType.ProductView, 1, 1, true)]
        [TestCase(KMPSEnum.ViewType.ProductView, 1, 0, true)]
        [TestCase(KMPSEnum.ViewType.ProductView, 0, 0, false)]
        [TestCase(KMPSEnum.ViewType.ConsensusView, 0, 0, true)]
        [TestCase(KMPSEnum.ViewType.ConsensusView, 1, 1, false)]
        public void DetailsDownload_DownloadIsEnabled_UpdatedControlValues(KMPSEnum.ViewType viewType, int id, int matchCase, bool selectedValue)
        {
            // Arrange
            CreatePageShimObject(true);
            var action = CreateDelMethodActionResult();
            var resultIntegerList = Enumerable.Range(0, 10).ToList();
            _downloadPanel.SubscriptionID = resultIntegerList;
            _downloadPanel.DelMethod = action;
            _downloadPanel.ViewType = viewType;
            _downloadPanel.PubIDs = resultIntegerList;
            _downloadPanel.BrandID = id;
            _downloadPanel.dcRunID = id;
            _downloadPanel.dcTypeCodeID = id;
            _downloadPanel.filterCombination = FilterCombination;
            GetField<RadioButton>(RadioButtonDownload).Checked = true;
            GetField<RadioButton>(RadioButtonMarketo).Checked = selectedValue;
            GetField<PlaceHolder>(PlaceHolderKmStaff).Visible = true;
            BindListBoxSelectedFields();
            var drpIsBillable = GetField<DropDownList>(DropDownListIsBillable);
            drpIsBillable.DataSource = new object[] { TrueString, FalseString };
            drpIsBillable.SelectedValue = TrueString;
            drpIsBillable.DataBind();
            var drpClient = GetField<DropDownList>(DropDownListClient);
            drpClient.DataSource = resultIntegerList;
            drpClient.SelectedValue = TestOne;
            drpClient.DataBind();
            var drpExistingGroupName = GetField<DropDownList>(DropDownListExistingGroupName);
            drpExistingGroupName.DataSource = resultIntegerList;
            drpExistingGroupName.SelectedValue = TestOne;
            drpExistingGroupName.DataBind();
            var drpFolder = GetField<DropDownList>(DropDownListFolder);
            drpFolder.DataSource = resultIntegerList;
            drpFolder.SelectedValue = TestOne;
            drpFolder.DataBind();
            var resultsGrid = GetField<DataGrid>(ResultsGrid);
            var dataview = new DataView(CreateDataTable());
            resultsGrid.Columns.Add(CreateBoundColumn(Id, TestOne));
            resultsGrid.Columns.Add(CreateBoundColumn(Count, TestOne));
            resultsGrid.Columns.Add(CreateBoundColumn(Desc, TestOne));
            resultsGrid.DataSource = dataview;
            resultsGrid.DataBind();
            FindPageControl();
            ShimDataCompareView.AllInstances.SelectForRunInt32 = (sender, dcRunId) => CreateDataCompareViewObject(matchCase);

            // Act
            PrivateControl.Invoke(DetailsDownload);

            // Assert
            _maskData.ShouldBeTrue();
            GetField<DropDownList>(DropDownListIsBillable).Enabled.ShouldBeFalse();
            _downloadData.ShouldBeTrue();
        }

        [TestCase(TestOne)]
        [TestCase(TestZero)]
        public void DetailsDownload_CampaignIsEnabled_SaveToCampaign(string testCase)
        {
            // Arrange
            var campaignFilterExists = false;
            var insertCampaign = false;
            var saveCampaignDetails = false;
            var getCountByCampaignId = false;
            var id = int.Parse(testCase);
            CreatePageShimObject(true);
            var action = CreateDelMethodActionResult();
            var resultIntegerList = Enumerable.Range(0, 10).ToList();
            _downloadPanel.DelMethod = action;
            _downloadPanel.PubIDs = resultIntegerList;
            _downloadPanel.BrandID = id;
            _downloadPanel.dcRunID = id;
            _downloadPanel.dcTypeCodeID = id;
            GetField<RadioButton>(RadioButtonCampaign).Checked = true;
            GetField<RadioButton>(RadioButtonMarketo).Checked = true;
            if (id == 1)
            {
                _downloadPanel.SubscriptionID = resultIntegerList;
                GetField<RadioButton>(RadioButtonExistingCampaign).Checked = true;
            }
            else
            {
                _downloadPanel.SubscriptionID = null;
                GetField<RadioButton>(RadioButtonNewCampaign).Checked = true;
            }
            var drpExistingCampaign = GetField<DropDownList>(DropDownListExistingCampaign);
            drpExistingCampaign.DataSource = Enumerable.Range(0, 10).ToList();
            drpExistingCampaign.SelectedIndex = id;
            drpExistingCampaign.DataBind();
            var resultsGrid = GetField<DataGrid>(ResultsGrid);
            var dataview = new DataView(CreateDataTable());
            resultsGrid.Columns.Add(CreateBoundColumn(Id, TestOne));
            resultsGrid.Columns.Add(CreateBoundColumn(Count, TestOne));
            resultsGrid.Columns.Add(CreateBoundColumn(Desc, TestOne));
            resultsGrid.DataSource = dataview;
            resultsGrid.DataBind();
            FindPageControl();

            ShimCampaigns.CampaignExistsClientConnectionsString = (x, y) => 0;
            ShimCampaignFilter.CampaignFilterExistsClientConnectionsStringInt32 = (x, y, z) =>
            {
                campaignFilterExists = true;
                return 0;
            };
            ShimCampaignFilter.InsertClientConnectionsStringInt32Int32String = (x, y, z, m, b) =>
            {
                insertCampaign = true;
                return 1;
            };
            ShimCampaignFilterDetails.saveCampaignDetailsClientConnectionsInt32String = (x, y, z) =>
            {
                saveCampaignDetails = true;
            };
            ShimCampaigns.GetCountByCampaignIDClientConnectionsInt32 = (x, y) =>
            {
                getCountByCampaignId = true;
                return 1;
            };
            ShimCampaigns.InsertClientConnectionsStringInt32Int32 = (x, y, z, m) => 1;

            // Act
            PrivateControl.Invoke(DetailsDownload);

            // Assert
            _getSubscriberData.ShouldBeTrue();
            campaignFilterExists.ShouldBeTrue();
            insertCampaign.ShouldBeTrue();
            saveCampaignDetails.ShouldBeTrue();
            getCountByCampaignId.ShouldBeTrue();
        }

        [TestCase(TestOne)]
        [TestCase(TestZero)]
        public void DetailsDownload_RadioButtonMarketoIsEnabled_ExportToMarketoLogic(string testCase)
        {
            // Arrange
            var createUpdateLeads = false;
            var id = int.Parse(testCase);
            CreatePageShimObject(true);
            var action = CreateDelMethodActionResult();
            var resultIntegerList = Enumerable.Range(0, 10).ToList();
            _downloadPanel.DelMethod = action;
            _downloadPanel.PubIDs = resultIntegerList;
            _downloadPanel.BrandID = id;
            _downloadPanel.dcRunID = id;
            _downloadPanel.dcTypeCodeID = id;
            GetField<RadioButton>(RadioButtonMarketo).Checked = true;
            GetField<TextBox>(TextTotalCount).Text = TestZero;
            GetField<TextBox>(TextDownloadCount).Text = TestZero;
            var drpExistingCampaign = GetField<DropDownList>(DropDownListExistingCampaign);
            drpExistingCampaign.DataSource = Enumerable.Range(0, 10).ToList();
            drpExistingCampaign.SelectedIndex = id;
            drpExistingCampaign.DataBind();
            var resultsGrid = GetField<DataGrid>(ResultsGrid);
            var dataview = new DataView(CreateDataTableForResultGrid());
            resultsGrid.Columns.Add(CreateBoundColumn(Type, TestOne));
            resultsGrid.Columns.Add(CreateBoundColumn(Action, TestOne));
            resultsGrid.Columns.Add(CreateBoundColumn(Totals, TestOne));
            resultsGrid.DataSource = dataview;
            resultsGrid.DataBind();
            FindPageControl();
            ShimMarketoRestAPIProcess.AllInstances.CreateUpdateLeadsListOfDictionaryOfStringStringStringStringNullableOfInt32 = (x, y, z, m, n) =>
            {
                createUpdateLeads = true;
                return CreateResultObject();
            };

            // Act
            PrivateControl.Invoke(DetailsDownload);

            // Assert
            var resultsDataGrid = GetField<DataGrid>(ResultsGrid);
            createUpdateLeads.ShouldBeTrue();
            resultsDataGrid.ShouldSatisfyAllConditions(
                () => resultsDataGrid.ShouldNotBeNull(),
                () => resultsDataGrid.Items.ShouldNotBeNull(),
                () => resultsDataGrid.Items.Count.ShouldBe(4));
        }

        private void BindListBoxSelectedFields()
        {
            var lstSelectedFields = GetField<ListBox>(ListBoxSelectedFields);
            lstSelectedFields.DataSource = new[] { "FNAME", "LNAME", "ISLATLONVALID", "ADDRESS1", "REGIONCODE", "ZIPCODE", "PUBTRANSACTIONDATE", "QUALIFICATIONDATE", "Unit Test" };
            lstSelectedFields.DataBind();
        }

        private static List<Result> CreateResultObject()
        {
            var types = new[] { "A", "B", "C", "D" };
            var result = new List<Result>();
            foreach (var item in types)
            {
                result.Add(new Result
                {
                    type = item,
                    status = DefaultText,
                    reasons = new List<Reason> { new Reason { code = item, message = DefaultText } }
                });
            }
            return result;
        }

        private static void FindPageControl()
        {
            ShimControl.AllInstances.FindControlString = (sender, controlId) =>
            {
                switch (controlId)
                {
                    case GridViewHttpPost:
                        return CreateGridView(controlId);
                    case LabelHttpPostParamsId:
                        return CreateLabelControl(controlId, sender);
                    case LabelParamName:
                        return CreateLabelControl(controlId, sender);
                    case LabelParamValue:
                        return CreateLabelControl(controlId, sender);
                    case LabelCustomValue:
                        return CreateLabelControl(controlId, sender);
                    case LabelParamDisplayName:
                        return CreateLabelControl(controlId, sender);
                    case TextBoxMarketoBaseUrl:
                        return CreateTextBoxControl(controlId, DemoText);
                    case TextBoxMarketoClientId:
                        return CreateTextBoxControl(controlId, TestOne);
                    case TextBoxMarketoPartition:
                        return CreateTextBoxControl(controlId, TestOne);
                    case TextBoxMarketoClientSecret:
                        return CreateTextBoxControl(controlId, TestOne);
                    case DropDownListMarketoList:
                        return new DropDownList { ID = controlId, DataSource = Enumerable.Range(0, 10), SelectedIndex = 1 };
                    default:
                        return new Control { ID = controlId };
                }
            };
        }

        private static Label CreateLabelControl(string controlId, Control sender)
        {
            if (controlId == LabelParamName || controlId == LabelParamValue)
            {
                var gridViewRow = sender as GridViewRow;
                var colums = new[] { "ADDRESS1", "REGIONCODE", "ZIPCODE", "PUBTRANSACTIONDATE", "QUALIFICATIONDATE", "FNAME",
                "LNAME", "ISLATLONVALID", "STATE",DefaultText};
                return new Label { ID = controlId, Text = colums[gridViewRow.RowIndex] };
            }
            return new Label { ID = controlId, Text = DefaultText };
        }

        private static TextBox CreateTextBoxControl(string controlId, string text)
        {
            return new TextBox { ID = controlId, Text = text };
        }

        private static GridView CreateGridView(string controlId)
        {
            var gridViewHttpPost = new GridView { ID = controlId };
            gridViewHttpPost.DataSource = CreateDataTable();
            gridViewHttpPost.DataBind();
            return gridViewHttpPost;
        }

        private static DataTable CreateDataTable()
        {
            var dataTable = new DataTable(GridViewHttpPost);
            var colums = new[] { "FIRSTNAME", "LASTNAME", "COMPANY", "TITLE", "ADDRESS", "ADDRESS2",
                "ADDRESS3", "CITY", "STATE", "ZIP", "COUNTRY", "PHONE", "FAX", "MOBILE", "sortorder",
                "GeoLocated","TransactionDate","QDate","Unit Test","ADDRESS1","REGIONCODE","PUBTRANSACTIONDATE",
                "QUALIFICATIONDATE","ISLATLONVALID","ZIPCODE","FNAME", "LNAME"};
            dataTable.Columns.Add(new DataColumn(Id));
            dataTable.Columns.Add(new DataColumn(Count));
            dataTable.Columns.Add(new DataColumn(Desc));
            dataTable.Columns.Add(SubscriptionID, typeof(string));
            dataTable.Columns.Add(Column1, typeof(string));
            dataTable.Columns.Add(Column2, typeof(string));

            foreach (var colum in colums)
            {
                dataTable.Columns.Add(colum, typeof(string));
            }
            for (var i = 0; i < 10; i++)
            {
                dataTable.Rows.Add(TestOne, TestOne, TestOne, TestOne, TestOne,
                    TestOne, TestOne, TestOne, TestOne,
                    TestOne, TestOne, TestOne, TestOne,
                    TestOne, TestOne, TestOne, TestOne,
                    TestOne, TestOne, TestOne, TestOne,
                    TestOne, TestOne, TestOne, TestOne, TestOne,
                    TestOne, TestOne, TestOne, TestOne, TestOne,
                    TestOne, TestOne);
            }
            return dataTable;
        }

        private static DataTable CreateDataTableForResultGrid()
        {
            var dataTable = new DataTable(GridViewHttpPost);
            dataTable.Columns.Add(new DataColumn(Type));
            dataTable.Columns.Add(new DataColumn(Action));
            dataTable.Columns.Add(new DataColumn(Totals));
            dataTable.Columns.Add(SubscriptionID, typeof(string));
            dataTable.Columns.Add(Column1, typeof(string));
            dataTable.Columns.Add(Column2, typeof(string));
            dataTable.Rows.Add(TestOne, TestOne, TestOne);
            return dataTable;
        }

        private BoundColumn CreateBoundColumn(String dataFieldValue, String headerTextValue)
        {
            var column = new BoundColumn();
            column.DataField = dataFieldValue;
            column.HeaderText = headerTextValue;
            return column;
        }

        private void CreatePageShimObject(bool isActive = false)
        {
            ShimECNSession.Constructor = (instance) => { };
            var shimSession = new ShimECNSession();
            shimSession.ClearSession = () => { };
            shimSession.Instance.CurrentUser = CreateUserObject(isActive);
            shimSession.ClientIDGet = () => { return 1; };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            ShimBaseControl.AllInstances.UserSessionGet = (sender) => shimSession.Instance;
            ShimBaseControl.AllInstances.clientconnectionsGet = (x) => new ClientConnections();
            ShimControl.AllInstances.PageGet = (sender) => CreateShimPageObject(shimSession);
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, m) => true;
            KM.Platform.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, m) => true;
            ShimCustomer.GetByClientIDInt32Boolean = (x, y) =>
            {
                _getByClientID = true;
                return new ECN_Framework_Entities.Accounts.Customer { CustomerID = 1 };
            };
            ShimFrameworkUad.ShimCode.AllInstances.SelectCodeIdInt32 = (sender, x) => new FrameworkUAD_Lookup.Entity.Code { CodeId = 1, CodeName = DataCompareType.Match.ToString() };
            ShimResponseGroup.GetActiveByPubIDClientConnectionsInt32 = (x, y) => new List<ResponseGroup>() { new ResponseGroup { ResponseGroupID = 1, ResponseGroupName = DefaultText } };
            ShimUserControl.AllInstances.ServerGet = (sender) => new ShimHttpServerUtility { MapPathString = (x) => string.Empty };
            ShimDataCompareDownloadCostDetail.AllInstances.CreateCostDetailInt32Int32StringStringStringInt32 = (x, y, z, m, n, v, k) => new List<DataCompareDownloadCostDetail>();
            KmpsFake.ShimCode.GetUADFieldType = () => CreateCodeListObject();
            KmpsFake.ShimCode.GetDataCompareTarget = () => new List<Code> { new Code { CodeName = KMPSEnum.DataCompareViewType.Consensus.ToString(), CodeID = 1 } };
            ShimDataCompareDownloadField.AllInstances.SaveDataCompareDownloadField = (x, y) => 1;
            ShimPubSubscriptionsExtensionMapper.GetActiveByPubIDClientConnectionsInt32 = (x, y) =>
            new List<PubSubscriptionsExtensionMapper> { new PubSubscriptionsExtensionMapper { CustomField = TestOne, PubSubscriptionsExtensionMapperId = 1 } };
            ShimDownloadPanel.AllInstances.saveDataCompareViewInt32NullableOfInt32Int32EnumsDataCompareTypeInt32 = (x, y, z, m, n, k) => 1;
            KM.Platform.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, m) => true;
            ShimProfileFieldMask.MaskDataClientConnectionsObjectUser = (x, y, z) =>
            {
                _maskData = true;
                return CreateDataTable();
            };
            KmpsFake.ShimFilter.AllInstances.ExecuteClientConnectionsString = (sender, x, y) => { };
            CreateShimFrameworkUadObject();
            CreaterameworkUasObject();
            CreateShimGroupsObject();
            CreateShimUtilitiesObject();
            CreateShimMasterGroupObject();
            CreateShimSubscriberObject();
            CreateShimDataCompareViewObject();
        }

        private static void CreateShimFrameworkUadObject()
        {
            ShimFrameworkUad.ShimCode.AllInstances.SelectEnumsCodeType = (x, y) =>
            {
                return new List<FrameworkUAD_Lookup.Entity.Code>
                {
                    new FrameworkUAD_Lookup.Entity.Code
                    {
                        CodeName=DataCompareType.Match.ToString(),
                        CodeId=1
                    }
                };
            };

            ShimFrameworkUad.ShimCode.AllInstances.SelectCodeNameEnumsCodeTypeString = (x, y, z) =>
            {
                return new FrameworkUAD_Lookup.Entity.Code
                {
                    CodeId = 1,
                    CodeName = DataCompareType.Match.ToString()
                };
            };
        }

        private static void CreaterameworkUasObject()
        {
            ShimSourceFile.AllInstances.SelectInt32Boolean = (sender, clientId, includeCustomProperties) =>
            {
                return new List<SourceFile>
                 {
                    new SourceFile
                    {
                        SourceFileID = 1,
                        FileName= DefaultText
                    }
                 };
            };

            ShimDataCompareRun.AllInstances.SelectForClientInt32 = (sender, clientId) =>
            {
                return new List<DataCompareRun>
                {
                    new DataCompareRun
                    {
                        SourceFileId = 1,
                        DateCreated=DateTime.Now,
                        ProcessCode= Guid.NewGuid().ToString()
                    }
                };
            };
        }

        private void CreateShimGroupsObject()
        {
            ShimGroups.GetGroupByIDInt32 = (groupId) => new Groups { MasterSupression = true, CustomerID = 1, GroupID = 1, GroupName = DefaultText };
            ShimGroups.ExistsByGroupNameByCustomerIDStringInt32 = (x, y) => true;
            ShimGroups.GetGroupByIDInt32 = (groupId) => new Groups { MasterSupression = true, CustomerID = 1, GroupID = 1, GroupName = DefaultText };
        }

        private void CreateShimUtilitiesObject()
        {
            var resultStringList = new List<string> { DefaultSplitterText };
            var resultIntegerList = Enumerable.Range(0, 10).ToList();
            ShimUtilities.GetSelectedPubSubExtMapperExportColumnsClientConnectionsListOfStringInt32 = (x, y, z) => resultStringList;
            ShimUtilities.GetSelectedResponseGroupStandardExportColumnsClientConnectionsListOfStringInt32BooleanBoolean = (x, y, z, m, n) =>
            new Tuple<List<string>, List<string>, List<string>>(resultStringList, resultStringList, resultStringList);
            ShimUtilities.GetSelectedSubExtMapperExportColumnsClientConnectionsListOfString = (x, y) => resultStringList;
            ShimUtilities.GetSelectedMasterGroupExportColumnsClientConnectionsListOfStringInt32 = (x, y, z) =>
            new Tuple<List<string>, List<string>>(resultStringList, resultStringList);
            ShimUtilities.GetSelectedStandardExportColumnsClientConnectionsListOfStringInt32 = (x, y, z) => resultStringList;
            ShimUtilities.GetStandardExportColumnFieldNameIListOfStringEnumsViewTypeInt32Boolean = (x, y, z, m) => resultStringList;
            ShimUtilities.GetSelectedCustomExportColumnsListOfString = (x) => resultStringList;
            ShimUtilities.getNthInt32Int32 = (x, y) => resultIntegerList;
            ShimUtilities.InsertGroupStringInt32Int32 = (x, y, z) => 1;
            ShimUtilities.ExportToECNInt32StringInt32Int32StringStringListOfExportFieldsDataTableInt32EnumsGroupExportSource =
                (groupId, groupName, customerId, folderId, promoCode, jobCode, exportFields, dtSubscribers, userID, source) =>
                {
                    _exportToEcn = true;
                    return new Hashtable { { TestOne, DefaultText } };
                };
            ShimUtilities.getImportedResultHashtableDateTime = (x, y) => CreateDataTable();
            ShimUtilities.DownloadDataCompareInt32DataTableString = (x, y, z) => { _downloadData = true; };
            ShimUtilities.DownloadDataTableStringStringInt32Int32 = (x, y, z, f, h) => { _downloadData = true; };
        }

        private void CreateShimMasterGroupObject()
        {
            ShimMasterGroup.GetActiveByBrandIDClientConnectionsInt32 = (x, y) => new List<MasterGroup> { new MasterGroup { ColumnReference = "1" } };
            ShimMasterGroup.GetActiveMasterGroupsSortedClientConnections = (x) => new List<MasterGroup> { new MasterGroup { ColumnReference = "1" } };
        }

        private void CreateShimSubscriberObject()
        {
            ShimSubscriptionsExtensionMapper.GetActiveClientConnections = (x) =>
            new List<SubscriptionsExtensionMapper> { new SubscriptionsExtensionMapper { CustomField = TestOne, SubscriptionsExtensionMapperId = 1 } };
            ShimSubscriptionsExtensionMapper.GetActiveClientConnections = (x) =>
           new List<SubscriptionsExtensionMapper> { new SubscriptionsExtensionMapper { CustomField = TestOne, SubscriptionsExtensionMapperId = 1 } };
            ShimSubscriber.GetSubscriberDataClientConnectionsStringBuilderListOfStringListOfStringListOfStringListOfStringListOfStringInt32ListOfInt32BooleanInt32String =
                (clientConnection, queries, standardColumnsList, masterGroupColumns, masterGroupDescColumns, subscriptionsExtMapperColumns,
                customColumnList, brandId, pubIds, isMostRecentData, downloadCount, subscriptionIDs) =>
                {
                    _getSubscriberData = true;
                    return CreateDataTable();
                };

            ShimSubscriber.GetProductDimensionSubscriberDataClientConnectionsStringBuilderListOfStringListOfInt32ListOfStringListOfStringListOfStringListOfStringInt32Int32 =
                (clientConnection, queries, standardColumnsList, pubIds, responseGroupIds, responseGroupDescIds, pubSubscriptionsExtMapperColumns,
                customColumnList, brandId, downloadCount) => CreateDataTable();
        }

        private void CreateShimDataCompareViewObject()
        {
            ShimDataCompareView.AllInstances.GetDataCompareCostInt32Int32Int32EnumsDataCompareTypeEnumsDataCompareCost = (x, y, z, n, m, c) => 20;
            ShimDataCompareView.AllInstances.DeleteInt32 = (x, y) => true;
            ShimDataCompareDownload.AllInstances.SaveDataCompareDownload = (sender, resultObject) => 1;
            ShimDataCompareDownloadDetail.AllInstances.SaveInt32String = (sender, x, y) => { };
            ShimDataCompareDownloadFilterGroup.AllInstances.SaveDataCompareDownloadFilterGroup = (x, y) => 1;
            ShimDataCompareDownloadFilterDetail.AllInstances.SaveDataCompareDownloadFilterDetail = (x, y) => 1;
            ShimDataCompareView.AllInstances.SaveDataCompareView = (x, y) => 1;
        }

        private List<Code> CreateCodeListObject()
        {
            return new List<Code>
            {
                new Code { CodeName = KMPSEnum.UADFieldType.Profile.ToString(), CodeID = 1 },
                new Code { CodeName = KMPSEnum.UADFieldType.Custom.ToString(), CodeID = 1 },
                new Code { CodeName = KMPSEnum.UADFieldType.Dimension.ToString(), CodeID = 1 },
                new Code { CodeName = KMPSEnum.UADFieldType.Adhoc.ToString(), CodeID = 1 },
            };
        }

        private static User CreateUserObject(bool isActive)
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
                IsKMStaff = true
            };
        }

        private static ShimPage CreateShimPageObject(ShimECNSession shimSession)
        {
            return new ShimPage
            {
                MasterGet = () => new ShimSite
                {
                    clientconnectionsGet = () =>
                    {
                        return new ClientConnections
                        {
                            ClientLiveDBConnectionString = string.Empty,
                            ClientTestDBConnectionString = string.Empty
                        };
                    },
                    UserSessionGet = () => shimSession.Instance,
                    LoggedInUserGet = () => 1
                },
                SessionGet = () => new ShimHttpSessionState
                {
                    ItemGetString = (x) => Enumerable.Range(0, 10).ToList()
                },
            };
        }

        private static List<DataCompareView> CreateDataCompareViewObject(int matchId)
        {
            return new List<DataCompareView>
            {
                new DataCompareView
                {
                    DcTargetCodeId = matchId,
                    DcTargetIdUad = matchId,
                    PaymentStatusId = matchId,
                    DcTypeCodeId = 1,
                    IsBillable = false
                }
            };
        }

        private static Action CreateDelMethodActionResult()
        {
            return () => Console.WriteLine(DefaultText);
        }
    }
}
