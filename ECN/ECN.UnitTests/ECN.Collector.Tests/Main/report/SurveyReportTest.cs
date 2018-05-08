using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using ecn.collector.main.report;
using ecn.collector.main.report.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Collector.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Objects.Fakes;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using BusinessLayerFakes = ECN_Framework_BusinessLayer.Communicator.Fakes;
using CollectorEntities = ECN_Framework_Entities.Collector;
using DataLayerFakes = ECN_Framework_DataLayer.Collector.Fakes;
using Assert = NUnit.Framework.Assert;
using Group = ECN_Framework_Entities.Communicator.Group;
using MasterPage = ecn.collector.MasterPages.Collector;
using MasterPageFakes = ecn.collector.MasterPages.Fakes.ShimCollector;
using ShimGroup = ECN_Framework_DataLayer.Communicator.Fakes.ShimGroup;
using ShimPage = System.Web.UI.Fakes.ShimPage;

namespace ECN.Collector.Tests.Main.report
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.collector.main.report.SurveyReport"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class SurveyReportTest : PageHelper
    {
        private const int StatementId = 5;
        private const string DefaultQuestionId = "100";
        private const string QuestionTotalValue = "10";
        private const string QuestionId = "10";
        private const string TotalRespondentsCount = "10";
        private const string PageLoadMethod = "Page_Load";
        private const string GenerateReportMethod = "GenerateReport";
        private const string PageUnloadMethod = "Page_Unload";
        private const string RespondentPagerIndexChangedMethod = "RespondentPager_IndexChanged";
        private const string BtnExportToGroupClickMethod = "btnExportToGroup_Click";
        private const string BtnExportClickMethod = "btnExport_Click";
        private const string LnkToPDFClickMethod = "lnkToPDF_Click";
        private const string LnkToEXLClickMethod = "lnktoExl_Click";
        private const string ShowDivAddToGroupMethod = "showDivAddToGroup";
        private const string DgFilterItemCommandMethod = "dgFilter_ItemCommand";
        private const string CreateCheckboxControlMethod = "CreateCheckboxControl";
        private const string DgRespondentSortCommandMethod = "dgRespondent_SortCommand";
        private const string DgGridResponseItemDataBoundMethod = "dgGridResponse_ItemDataBound";
        private const string GetRespondentsForQuestionMethod = "getrespondentsforQuestion";
        private const string RbpercentusingSelectedIndexChangedMethod = "rbpercentusing_SelectedIndexChanged";
        private const string RbNewGroupCheckedChangedMethod = "rbNewGroup_CheckedChanged";
        private const string RepQuestionsItemCommandMethod = "repQuestions_ItemCommand";
        private const string RepQuestionsItemDataBoundMethod = "repQuestions_ItemDataBound";
        private const string RepAnswersItemDataBoundMethod = "repAnswers_ItemDataBound";
        private const string RbExistingGroupCheckedChangedMethod = "rbExistingGroup_CheckedChanged";
        private const string BtnSaveToGroupClickMethod = "btnSaveToGroup_Click";
        private const string LblTotalRespondentsId = "lblTotalRespondents";
        private const string PHRespondentId = "phRespondent";
        private const string RbNewGroupId = "rbNewGroup";
        private const string RbExistingGroupId = "rbExistingGroup";
        private const string LblErrorMessageId = "lblErrorMessage";
        private const string RbpercentusingId = "rbpercentusing";
        private const string TotalCompletedValue = "10";
        private const string DrpGroupId = "drpGroup";
        private const string LblFilterCountId = "lblFilterCount";
        private const string PlotherformatId = "plotherformat";
        private const string PlgridformatId = "plgridformat";
        private const string RepAnswersId = "repAnswers";
        private const string DgGridResponseId = "dgGridResponse";
        private const string LblQuestiontypeId = "lblQuestiontype";
        private const string LblQuestionIDId = "lblQuestionID";
        private const string LblTotalRespondentsCountId = "lblTotalRespondentsCount";
        private const string LblSurveyTitleId = "lblSurveyTitle";
        private const string LblRatioId = "lblRatio";
        private const string LblQIDId = "lblQID";
        private const string LblOIDId = "lblOID";
        private const string LblHasOtherResponseId = "lblHasOtherResponse";
        private const string LbloptionvalueId = "lbloptionvalue";
        private const string PlCheckboxId = "plCheckbox";
        private const string PlbarId = "plbar";
        private const string PhResultsId = "phResults";
        private const string CrystalReportViewerId = "crv";
        private const string PlExistingGroupId = "plExistingGroup";
        private const string PlNewGroupId = "plNewGroup";
        private const string MPEExportToGroupId = "mpeExportToGroup";
        private PrivateObject _surveyReportPrivateObject;
        private SurveyReport _surveyReportInstance;
        private ShimSurveyReport _shimSurveyReport;
        private MasterPage _masterPage;
        private MasterPageFakes _masterPageFakes;
        private StateBag _viewState;
        private Label _lblErrorMessage;
        private bool _reportExported;

        [Test]
        public void Page_Unload_NoErrors()
        {
            // Act
            var action = (TestDelegate)(() => _surveyReportPrivateObject.Invoke(PageUnloadMethod, new object[] { null, EventArgs.Empty }));

            // Assert
            Assert.DoesNotThrow(action);
        }

        [Test]
        public void RespondentPager_IndexChanged_LoadGridCalled()
        {
            // Arrange
            var loadRespondentGridCalled = false;
            ShimSurveyReport.AllInstances.LoadRespondentGrid = (p) => loadRespondentGridCalled = true;
            // Act
            _surveyReportPrivateObject.Invoke(RespondentPagerIndexChangedMethod, new object[] { null, EventArgs.Empty });

            // Assert
            loadRespondentGridCalled.ShouldBeTrue();
        }

        [Test]
        public void BtnExportToGroup_Click_LoadGridCalled()
        {
            // Arrange
            var loadRespondentGridCalled = false;
            ShimSurveyReport.AllInstances.LoadRespondentGrid = (p) => loadRespondentGridCalled = true;

            // Act
            _surveyReportPrivateObject.Invoke(BtnExportToGroupClickMethod, new object[] { null, EventArgs.Empty });

            // Assert
            loadRespondentGridCalled.ShouldBeTrue();
        }

        [Test]
        public void BtnExport_Click_LoadGridCalled()
        {
            // Arrange
            InitCommon();
            _surveyReportInstance.Master.UserSession.CurrentCustomer = new Customer();
            _surveyReportInstance.Master.UserSession.CurrentBaseChannel = new BaseChannel();
            var loadRespondentGridCalled = false;
            ShimSurveyReport.AllInstances.LoadRespondentGrid = (p) => loadRespondentGridCalled = true;
            ShimSurvey.GetBySurveyIDInt32User = (id, user) => new CollectorEntities.Survey();

            // Act
            _surveyReportPrivateObject.Invoke(BtnExportClickMethod, new object[] { null, EventArgs.Empty });

            // Assert
            loadRespondentGridCalled.ShouldBeTrue();
        }

        [Test]
        public void LnkToEXL_Click_GenerateReportCalled()
        {
            // Arrange
            var generateReportCalled = false;
            ShimSurveyReport.AllInstances.GenerateReportCRExportEnum = (p, exportEnum) => generateReportCalled = true;

            // Act
            _surveyReportPrivateObject.Invoke(LnkToEXLClickMethod, new object[] { null, new ImageClickEventArgs(0, 0) });

            // Assert
            generateReportCalled.ShouldBeTrue();
        }

        [Test]
        public void LnkToPDF_Click_GenerateReportCalled()
        {
            // Arrange
            var generateReportCalled = false;
            ShimSurveyReport.AllInstances.GenerateReportCRExportEnum = (p, exportEnum) => generateReportCalled = true;

            // Act
            _surveyReportPrivateObject.Invoke(LnkToPDFClickMethod, new object[] { null, new ImageClickEventArgs(0, 0) });

            // Assert
            generateReportCalled.ShouldBeTrue();
        }

        [Test]
        public void ShowDivAddToGroup_RegisterScriptCalled()
        {
            // Arrange
            var registerScriptCalled = false;
            ShimClientScriptManager.AllInstances.RegisterStartupScriptTypeStringString = (m, t, key, script) => registerScriptCalled = true;

            // Act
            _surveyReportPrivateObject.Invoke(ShowDivAddToGroupMethod, new object[] { });

            // Assert
            registerScriptCalled.ShouldBeTrue();
        }

        [TestCase(1, 10, "5|10")]
        [TestCase(1, 10, "2|10")]
        [TestCase(1, 10, "5|3")]
        [TestCase(1, 10, "10")]
        public void CreateCheckboxControl_CheckBoxCreated(int questionId, int optionId, string statementOptionId)
        {
            // Arrange
            var filters = new Hashtable();
            filters[questionId] = statementOptionId;
            _surveyReportPrivateObject.SetField("htFilters", BindingFlags.NonPublic | BindingFlags.Instance, filters);

            // Act
            var chkbox = _surveyReportPrivateObject.Invoke(CreateCheckboxControlMethod, new object[] { questionId, StatementId, optionId }) as CheckBox;

            // Assert
            chkbox.ShouldSatisfyAllConditions(
                () => chkbox.ShouldNotBeNull(),
                () => chkbox.ID.ShouldBe(string.Format("checkbox_{0}_{1}_{2}", questionId, StatementId, optionId)),
                () => chkbox.Visible.ShouldBeTrue());
        }

        [Test]
        public void DgFilter_ItemCommand_FiltersInitialized()
        {
            // Arrange
            ShimSurveyReport.AllInstances.LoadQuestionGrid = (p) => { };
            ShimSurveyReport.AllInstances.LoadRespondentGrid = (p) => { };
            _viewState["Filters"] = "10|1|6|7,5|1|2,8|3|4|5,7|1|2";
            var arg = new DataGridCommandEventArgs(null, null, new CommandEventArgs("remove", "cmdArg"));

            // Act
            var result = _surveyReportPrivateObject.Invoke(DgFilterItemCommandMethod, new object[] { null, arg });

            // Assert
            _viewState["Filters"].ToString().ShouldBe("10|1,5|1|2,8|3,7|1|2");
        }

        [TestCase(ListItemType.Item)]
        [TestCase(ListItemType.AlternatingItem)]
        [TestCase(ListItemType.Header)]
        [TestCase(ListItemType.Footer)]
        public void DgGridResponse_ItemDataBound_ControlsInitialized(ListItemType itemType)
        {
            // Arrange
            InitCommon();
            var dataGridItem = new DataGridItem(0, 0, itemType);
            for (var i = 0; i < 10; i++)
            {
                var tableCell = new TableCell();
                tableCell.Text = i > 6
                    ? "0"
                    : "1";
                dataGridItem.Cells.Add(tableCell);
            }
            var arg = new DataGridItemEventArgs(dataGridItem);

            // Act
            var result = _surveyReportPrivateObject.Invoke(DgGridResponseItemDataBoundMethod, new object[] { null, arg });

            // Assert
            dataGridItem.ShouldSatisfyAllConditions(
                () => dataGridItem.Cells[0].Visible.ShouldBeFalse(),
                () => dataGridItem.Cells[1].Visible.ShouldBeFalse(),
                () => dataGridItem.Cells[4].Controls.Count.ShouldBe(itemType == ListItemType.AlternatingItem || itemType == ListItemType.Item
                        ? 2
                        : 0),
                () => dataGridItem.Cells[6].Controls.Count.ShouldBe(itemType == ListItemType.AlternatingItem || itemType == ListItemType.Item
                        ? 2
                        : 0));
        }

        [TestCase("DESC", "field1", "field1")]
        [TestCase("ASC", "field2", "field2")]
        [TestCase("DESC", "field1", "field2")]
        public void DgRespondent_SortCommand_SortDirectInitialized(string viewStateSortDirection, string viewStateSortField, string gridSortExpression)
        {
            // Arrange
            InitCommon();
            var arg = new DataGridSortCommandEventArgs(null, new DataGridCommandEventArgs(null, null, new CommandEventArgs("", null)));
            ShimSurveyReport.AllInstances.LoadRespondentGrid = (p) => { };
            ShimDataGridSortCommandEventArgs.AllInstances.SortExpressionGet = (a) => gridSortExpression;
            _viewState["SortDirection"] = viewStateSortDirection;
            _viewState["SortField"] = viewStateSortField;
            var expectedSortDirection = "ASC";
            if (viewStateSortField == gridSortExpression)
            {
                expectedSortDirection = viewStateSortDirection == "ASC"
                    ? "DESC"
                    : "ASC";
            }

            // Act
            var result = _surveyReportPrivateObject.Invoke(DgRespondentSortCommandMethod, new object[] { null, arg });

            // Assert
            var currentSortDirect = _viewState["SortDirection"];
            currentSortDirect.ShouldBe(expectedSortDirection);
        }

        [TestCase(true, ListItemType.Item)]
        [TestCase(false, ListItemType.Item)]
        [TestCase(false, ListItemType.AlternatingItem)]
        [TestCase(false, ListItemType.Footer)]
        public void GetRespondentsForQuestion_CorrectTotalRespondents(bool withException, ListItemType itemType)
        {
            // Arrange
            InitTestGetRespondentsForQuestion(withException, itemType);

            // Act
            var result = _surveyReportPrivateObject.Invoke(GetRespondentsForQuestionMethod, new object[] { int.Parse(QuestionId) });

            // Assert
            result.ShouldBe(withException || (itemType != ListItemType.Item && itemType != ListItemType.AlternatingItem)
                    ? "0"
                    : QuestionTotalValue);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Rbpercentusing_SelectedIndexChanged_ControlsInitialized(bool phResultsVisible)
        {
            // Arrange
            InitTestRbpercentusingSelectedIndexChanged(phResultsVisible);
            var rbExistingGroup = GetPropertyOrField<RadioButton>(RbExistingGroupId);
            var phRespondent = GetPropertyOrField<PlaceHolder>(PHRespondentId);
            var rbNewGroup = GetPropertyOrField<RadioButton>(RbNewGroupId);
            var plNewGroup = GetPropertyOrField<PlaceHolder>(PlNewGroupId);
            rbExistingGroup.Checked = true;
            rbNewGroup.Checked = true;

            // Act
            _surveyReportPrivateObject.Invoke(RbpercentusingSelectedIndexChangedMethod, new object[] { null, EventArgs.Empty });

            // Assert
            phRespondent.ShouldSatisfyAllConditions(
                () => phRespondent.Visible.ShouldBe(!phResultsVisible),
                () => rbExistingGroup.Checked.ShouldBe(phResultsVisible),
                () => rbNewGroup.Checked.ShouldBe(phResultsVisible),
                () => plNewGroup.Visible.ShouldBe(phResultsVisible));
        }

        [Test]
        public void RbNewGroup_CheckedChanged__ControlsInitialized()
        {
            // Arrange
            InitCommon();
            ShimSurveyReport.AllInstances.LoadRespondentGrid = (p) => { };
            var plExistingGroup = GetPropertyOrField<PlaceHolder>(PlExistingGroupId);
            var plNewGroup = GetPropertyOrField<PlaceHolder>(PlNewGroupId);
            plNewGroup.Visible = false;

            // Act
            _surveyReportPrivateObject.Invoke(RbNewGroupCheckedChangedMethod, new object[] { null, EventArgs.Empty });

            // Assert
            plExistingGroup.ShouldSatisfyAllConditions(
                () => plExistingGroup.Visible.ShouldBeFalse(),
                () => plNewGroup.Visible.ShouldBeTrue());
        }

        [Test]
        public void RbExistingGroup_CheckedChanged_ControlsInitialized()
        {
            // Arrange
            InitCommon();
            ShimSurveyReport.AllInstances.LoadRespondentGrid = (p) => { };
            var plExistingGroup = GetPropertyOrField<PlaceHolder>(PlExistingGroupId);
            var plNewGroup = GetPropertyOrField<PlaceHolder>(PlNewGroupId);
            plExistingGroup.Visible = false;
            ShimGroup.GetListSqlCommand = (cmd) => new List<Group>()
            {
                new Group() { GroupName="grpName" }
            };

            // Act
            _surveyReportPrivateObject.Invoke(RbExistingGroupCheckedChangedMethod, new object[] { null, EventArgs.Empty });

            // Assert
            plExistingGroup.ShouldSatisfyAllConditions(
                () => plExistingGroup.Visible.ShouldBeTrue(),
                () => plNewGroup.Visible.ShouldBeFalse());
        }

        [TestCase(CRExportEnum.PDF, true)]
        [TestCase(CRExportEnum.XLS, false)]
        [TestCase(CRExportEnum.XLSDATA, false)]
        [TestCase(CRExportEnum.DOC, false)]
        public void GenerateReport_ReportExported(CRExportEnum exportFormat, bool phResultsVisible)
        {
            // Arrange
            InitTestGenerateReport(phResultsVisible);
            var reportViewer = GetPropertyOrField<CrystalReportViewer>(CrystalReportViewerId);
            reportViewer.Visible = false;

            // Act
            _surveyReportPrivateObject.Invoke(GenerateReportMethod, new object[] { exportFormat });

            // Assert
            _reportExported.ShouldSatisfyAllConditions(
                () => _reportExported.ShouldBeTrue(),
                () => reportViewer.Visible.ShouldBeTrue());
        }

        [TestCase("radio")]
        [TestCase("checkbox")]
        [TestCase("dropdown")]
        [TestCase("textbox")]
        [TestCase("grid")]
        public void RepQuestions_ItemCommand_FiltersInitialized(string lblQuestiontypeValue)
        {
            // Arrange
            var arg = new RepeaterCommandEventArgs(null, null, new CommandEventArgs("filter", null));
            var expectedFilterValue = InitTestRepQuestionsItemCommand(lblQuestiontypeValue);

            // Act
            _surveyReportPrivateObject.Invoke(RepQuestionsItemCommandMethod, new object[] { null, arg });

            // Assert
            var filter = _viewState["Filters"] as string;
            filter.ShouldSatisfyAllConditions(
                () => filter.ShouldNotBeNullOrEmpty(),
                () => filter.ShouldBe(expectedFilterValue));
        }

        [TestCase(ListItemType.Item, "0", 1, "0")]
        [TestCase(ListItemType.AlternatingItem, "1", 2, "1")]
        [TestCase(ListItemType.AlternatingItem, "1", 3, "1")]
        [TestCase(ListItemType.AlternatingItem, "1", 4, "1")]
        [TestCase(ListItemType.AlternatingItem, "1", 5, "1")]
        public void RepAnswers_ItemDataBound_RowCountValueChanged(ListItemType itemType, string lblOIDValue, int rowCount, string bHasOtherResponseText)
        {
            // Arrange
            var arg = new RepeaterItemEventArgs(new RepeaterItem(0, itemType));
            InitTestRepAnswersItemDataBound(lblOIDValue, rowCount, bHasOtherResponseText, out int expectedRowCountValue);

            // Act
            _surveyReportPrivateObject.Invoke(RepAnswersItemDataBoundMethod, new object[] { null, arg });

            // Assert
            var rowCountValue = Get<int>(_surveyReportPrivateObject, "rowcount");
            rowCountValue.ShouldBe(expectedRowCountValue);
        }

        [TestCase(ListItemType.Separator, "1", "NA")]
        [TestCase(ListItemType.Item, "1", "radio")]
        [TestCase(ListItemType.AlternatingItem, "2", "checkbox")]
        [TestCase(ListItemType.AlternatingItem, "2", "dropdown")]
        [TestCase(ListItemType.AlternatingItem, "2", "textbox")]
        [TestCase(ListItemType.AlternatingItem, "2", "grid")]
        public void RepQuestions_ItemDataBound_ControlsInitialized(ListItemType itemType, string rbpercentusingValue, string lblQuestiontypeValue)
        {
            // Arrange
            var arg = new RepeaterItemEventArgs(new RepeaterItem(0, itemType));
            var plotherFormatPH = new PlaceHolder();
            var gridformatPH = new PlaceHolder();
            gridformatPH.Visible = false;
            var lblTotalRespondentsCount = new Label();
            InitTestRepQuestionsItemDataBound(gridformatPH, plotherFormatPH, lblTotalRespondentsCount, rbpercentusingValue, lblQuestiontypeValue);

            // Act
            _surveyReportPrivateObject.Invoke(RepQuestionsItemDataBoundMethod, new object[] { null, arg });

            // Assert
            lblTotalRespondentsCount.ShouldSatisfyAllConditions(
                () => lblTotalRespondentsCount.Text.ShouldBe(lblQuestiontypeValue == "NA"
                          ? string.Empty
                          : TotalRespondentsCount),
                () => plotherFormatPH.Visible.ShouldBe(lblQuestiontypeValue != "grid"),
                () => gridformatPH.Visible.ShouldBe(lblQuestiontypeValue == "grid"));
        }

        [Test]
        public void BtnSaveToGroup_Click_NewAndExistingGroupNotChecked_Error()
        {
            // Arrange
            InitCommon();
            var rbNewGroup = GetPropertyOrField<RadioButton>(RbNewGroupId);
            var rbExistingGroup = GetPropertyOrField<RadioButton>(RbExistingGroupId);
            rbNewGroup.Checked = false;
            rbExistingGroup.Checked = false;

            // Act
            _surveyReportPrivateObject.Invoke(BtnSaveToGroupClickMethod, new object[] { null, EventArgs.Empty });

            // Assert
            _lblErrorMessage.Visible.ShouldBeTrue();
        }

        [Test]
        public void BtnSaveToGroup_Click_GroupSaveException_Error()
        {
            // Arrange
            InitCommon();
            var rbNewGroup = GetPropertyOrField<RadioButton>(RbNewGroupId);
            rbNewGroup.Checked = true;
            BusinessLayerFakes.ShimGroup.SaveGroupUser = (grp, user) => throw new ECNException(null);

            // Act
            _surveyReportPrivateObject.Invoke(BtnSaveToGroupClickMethod, new object[] { null, EventArgs.Empty });

            // Assert
            _lblErrorMessage.Visible.ShouldBeTrue();
        }

        [TestCase("DESC")]
        [TestCase("Asc")]
        public void BtnSaveToGroup_Click_RbNewGroupChecked_UserGroupSaved(string sortDirection)
        {
            // Arrange
            InitTestBtnSaveToGroupClickRbNewGroupChecked(sortDirection);
            var userGroupSaved = false;
            BusinessLayerFakes.ShimUserGroup.SaveUserGroupUser = (grp, user) =>
            {
                userGroupSaved = true;
                return 1;
            };

            // Act
            _surveyReportPrivateObject.Invoke(BtnSaveToGroupClickMethod, new object[] { null, EventArgs.Empty });

            // Assert
            userGroupSaved.ShouldBeTrue();
        }

        [TestCase(false)]
        [TestCase(true)]
        public void BtnSaveToGroup_Click_RbExistingGroupCheckedGroupIdNotZero_Error(bool hasEmails)
        {
            // Arrange
            InitTestBtnSaveToGroupClickRbExistingGroupChecked(hasEmails);

            // Act
            _surveyReportPrivateObject.Invoke(BtnSaveToGroupClickMethod, new object[] { null, EventArgs.Empty });

            // Assert
            _lblErrorMessage.Visible.ShouldBeTrue();
        }

        [Test]
        public void Page_Load_ControlsInitialized()
        {
            // Arrange
            InitTestPageLoad();
            var lblTotalRespondents = GetPropertyOrField<Label>(LblTotalRespondentsId);
            var phRespondent = GetPropertyOrField<PlaceHolder>(PHRespondentId);

            // Act
            _surveyReportPrivateObject.Invoke(PageLoadMethod, new object[] { null, EventArgs.Empty });

            // Assert
            lblTotalRespondents.ShouldSatisfyAllConditions(
                () => lblTotalRespondents.Text.ShouldBe((int.Parse(TotalCompletedValue) * 2).ToString()),
                () => phRespondent.Visible.ShouldBeFalse());
        }

        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                                   new HttpStaticObjectsCollection(), 10, true,
                                                   HttpCookieMode.AutoDetect,
                                                   SessionStateMode.InProc, false);
            var sessionState = typeof(HttpSessionState).GetConstructor(
                                     BindingFlags.NonPublic | BindingFlags.Instance,
                                     null, CallingConventions.Standard,
                                     new[] { typeof(HttpSessionStateContainer) },
                                     null)
                                .Invoke(new object[] { sessionContainer }) as HttpSessionState;
            ShimUserControl.AllInstances.SessionGet = (p) =>
            {
                return sessionState;
            };
            _surveyReportInstance = new SurveyReport();
            _shimSurveyReport = new ShimSurveyReport(_surveyReportInstance);
            _surveyReportPrivateObject = new PrivateObject(_surveyReportInstance);
            InitializeAllControls(_surveyReportInstance);
            var shimEcnSession = new ShimECNSession();
            ShimECNSession.CurrentSession = () => shimEcnSession;
            ECNSession.CurrentSession().CurrentUser = new User() { UserID = 10 };
            _viewState = GetPropertyOrField<StateBag>("ViewState");
        }

        private void InitTestRepQuestionsItemDataBound(
          PlaceHolder gridformatPH,
          PlaceHolder plotherFormatPH,
          Label lblTotalRespondentsCount,
          string rbpercentusingValue,
          string lblQuestiontypeValue)
        {
            InitCommon();
            var rbpercentusing = GetPropertyOrField<RadioButtonList>(RbpercentusingId);
            rbpercentusing.Items.Add(new ListItem("1", "1") { Selected = rbpercentusingValue == "1" });
            rbpercentusing.Items.Add(new ListItem("2", "2") { Selected = rbpercentusingValue == "2" });
            GetPropertyOrField<Label>(LblFilterCountId).Text = TotalRespondentsCount;
            ShimSurveyReport.AllInstances.getrespondentsforQuestionInt32 = (p, id) => TotalRespondentsCount;
            ShimDataFunctions.GetDataTableSqlCommandString = (cmd, conn) => new DataTable();
            ShimControl.AllInstances.FindControlString = (c, id) =>
            {
                switch (id)
                {
                    case PlotherformatId: return plotherFormatPH;
                    case PlgridformatId: return gridformatPH;
                    case RepAnswersId: return new Repeater();
                    case DgGridResponseId: return new DataGrid();
                    case LblQuestiontypeId: return new Label() { Text = lblQuestiontypeValue };
                    case LblQuestionIDId: return new Label() { Text = "10" };
                    case LblTotalRespondentsCountId: return lblTotalRespondentsCount;
                    default: return null;
                }
            };
        }

        private void InitTestRepAnswersItemDataBound(string lblOIDValue, int rowCount, string bHasOtherResponseText, out int expectedRowCount)
        {
            InitCommon();
            _surveyReportPrivateObject.SetField("rowcount", rowCount);
            ShimControl.AllInstances.FindControlString = (c, id) =>
            {
                switch (id)
                {
                    case LblRatioId: return new Label();
                    case LblQIDId: return new Label() { Text = "1" };
                    case LblOIDId: return new Label() { Text = lblOIDValue };
                    case LblHasOtherResponseId: return new Label() { Text = bHasOtherResponseText };
                    case LbloptionvalueId: return new Label();
                    case PlCheckboxId: return new PlaceHolder();
                    case PlbarId: return new PlaceHolder();
                    default: return null;
                }
            };
            switch (rowCount)
            {
                case 1:
                case 2:
                case 3:
                case 4: expectedRowCount = ++rowCount; break;
                default: expectedRowCount = 1; break;
            }
        }

        private void InitTestGetRespondentsForQuestion(bool withException, ListItemType itemType)
        {
            InitCommon();
            var dataGridItemsList = new ArrayList();
            dataGridItemsList.Add(new DataGridItem(2, 2, ListItemType.Pager));
            dataGridItemsList.Add(new DataGridItem(1, 1, itemType));
            dataGridItemsList.Add(new DataGridItem(0, 0, itemType));
            var dataGridItems = new DataGridItemCollection(dataGridItemsList);
            ShimDataGrid.AllInstances.ItemsGet = (grid) => dataGridItems;
            ShimBaseDataList.AllInstances.DataKeysGet = (d) => new ShimDataKeyCollection();
            ShimDataKeyCollection.AllInstances.ItemGetInt32 = (col, id) =>
            {
                return id == 0
                     ? QuestionId
                     : DefaultQuestionId;
            };
            ShimControl.AllInstances.FindControlString = (c, name) =>
            {
                if (name == "lblQuestionTotal")
                {
                    if (withException)
                    {
                        return new TextBox();
                    }
                    else
                    {
                        return new Label() { Text = QuestionTotalValue };
                    }
                }
                return null;
            };
        }

        private void InitTestBtnSaveToGroupClickRbExistingGroupChecked(bool hasEmails)
        {
            InitCommon();
            var rbExistingGroup = GetPropertyOrField<RadioButton>(RbExistingGroupId);
            rbExistingGroup.Checked = true;
            var drpGroup = GetPropertyOrField<DropDownList>(DrpGroupId);
            drpGroup.Items.Add(new ListItem("1", "1") { Selected = true });
            ShimSurveyReport.AllInstances.LoadRespondentGrid = (p) => { };
            BusinessLayerFakes.ShimGroup.GetByGroupIDInt32User = (id, user) => new Group();
            BusinessLayerFakes.ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString =
                (user, custId, groupId, xmlProfile, xmlUDF, formatTypeCode, subscribeTypeCode, emailAddressOnly, filename, source) => new DataTable();
            var emailList = new List<Email>();
            var email = new Email()
            {
                EmailAddress = "emailAddress"
            };
            if (hasEmails)
            {
                emailList.Add(email);
                emailList.Add(email);
            }
            ShimParticipant.GetParticipantsInt32String = (id, filters) => emailList;
        }

        private void InitTestBtnSaveToGroupClickRbNewGroupChecked(string sortDirection)
        {
            InitCommon();
            var rbNewGroup = GetPropertyOrField<RadioButton>(RbNewGroupId);
            rbNewGroup.Checked = true;
            BusinessLayerFakes.ShimUserGroup.ExistsInt32 = (id) => true;
            DataLayerFakes.ShimSurvey.GetEmailListSqlCommand = (cmd) => new List<Email>()
             {
                 new Email()
                 {
                     EmailAddress = "@survey_0",
                     EmailID =1
                 },
                  new Email()
                 {
                     EmailAddress = "emailAddress"
                 }
             };
            _viewState["SortDirection"] = sortDirection;
        }

        private void InitTestPageLoad()
        {
            InitCommon();
            ShimSurvey.GetBySurveyIDInt32User = (id, user) => new CollectorEntities.Survey()
            {
                SurveyTitle = "title"
            };
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) => TotalCompletedValue;
            ShimDataFunctions.GetDataTableSqlCommandString = (cmd, conn) => new DataTable();
            _viewState["Filters"] = "10|1,5|1|2";
        }

        private void InitTestGenerateReport(bool phResultsVisible)
        {
            InitCommon();
            _reportExported = false;
            var appSettings = new NameValueCollection();
            appSettings["Server"] = "Server";
            appSettings["Database"] = "Database";
            appSettings["UserID"] = "UserID";
            appSettings["Password"] = "Password";
            var rbPercentUsing = GetPropertyOrField<RadioButtonList>(RbpercentusingId);
            rbPercentUsing.Items.Add(new ListItem("", "") { Selected = true });
            ShimPage.AllInstances.ServerGet = (p) => new ShimHttpServerUtility();
            ShimHttpServerUtility.AllInstances.MapPathString = (su, pathValue) => "fakePath";
            var phResults = GetPropertyOrField<PlaceHolder>(PhResultsId);
            phResults.Visible = phResultsVisible;
            ShimConfigurationManager.AppSettingsGet = () => appSettings;
            ShimCRReport.GetReportStringHashtable = (name, repParam) => new ReportDocument();
            ShimCRReport.ExportReportDocumentCRExportEnumString = (rep, formate, fileName) =>
            {
                _reportExported = true;
            };
            ShimHttpContext.AllInstances.ApplicationInstanceGet = (ctx) => new ShimHttpApplication();
            ShimHttpResponse.AllInstances.WriteFileString = (r, pathValue) => { };
            ShimHttpResponse.AllInstances.Flush = (r) => { };
            ShimHttpApplication.AllInstances.CompleteRequest = (app) => { };
            ShimPath.GetTempFileName = () => "fakePath";
            ShimFile.DeleteString = (pathValue) => { };
        }

        private string InitTestRepQuestionsItemCommand(string lblQuestiontypeValue)
        {
            InitCommon();
            var repItemsList = new ArrayList();
            repItemsList.Add(new RepeaterItem(0, ListItemType.Item));
            repItemsList.Add(new RepeaterItem(0, ListItemType.Item));
            var repItemCollection = new RepeaterItemCollection(repItemsList);
            var dataGridItemsList = new ArrayList();
            dataGridItemsList.Add(new DataGridItem(0, 0, ListItemType.Item));
            dataGridItemsList.Add(new DataGridItem(0, 0, ListItemType.Item));
            var dataGridItemsCollections = new DataGridItemCollection(dataGridItemsList);
            ShimDataGrid.AllInstances.ItemsGet = (g) => dataGridItemsCollections;
            ShimTableRow.AllInstances.CellsGet = (t) => new ShimTableCellCollection();
            ShimTableCellCollection.AllInstances.CountGet = (t) => 6;
            ShimTableCellCollection.AllInstances.ItemGetInt32 = (t, id) => new TableCell();
            ShimRepeater.AllInstances.ItemsGet = (r) => repItemCollection;
            ShimTableCell.AllInstances.TextGet = (c) => "0";
            ShimSurveyReport.AllInstances.LoadQuestionGrid = (p) => { };
            ShimSurveyReport.AllInstances.LoadRespondentGrid = (p) => { };
            ShimControl.AllInstances.FindControlString = (c, id) =>
            {
                switch (id)
                {
                    case LblQIDId: return new Label() { Text = "1" };
                    case LblOIDId: return new Label() { Text = "1" };
                    case RepAnswersId: return new Repeater();
                    case DgGridResponseId: return new DataGrid();
                    case LblQuestiontypeId: return new Label() { Text = lblQuestiontypeValue };
                    case LblQuestionIDId: return new Label() { Text = "10" };
                    case PlCheckboxId: return new PlaceHolder();
                    case "checkbox_0_0_0":
                    case "checkbox_1_1": return new CheckBox() { Checked = true };
                    default: return null;
                }
            };
            return lblQuestiontypeValue == "grid"
                ? "0|0|0,0|0|0,0|0|0,0|0|0"
                : "1|1,1|1,1|1,1|1";
        }

        private void InitTestRbpercentusingSelectedIndexChanged(bool phResultsVisible)
        {
            InitCommon();
            ShimSurveyReport.AllInstances.LoadRespondentGrid = (p) => { };
            ShimSurveyReport.AllInstances.LoadQuestionGrid = (p) => { };
            var phResults = GetPropertyOrField<PlaceHolder>(PhResultsId);
            phResults.Visible = phResultsVisible;
        }

        private void InitCommon()
        {
            _masterPage = new MasterPage();
            _masterPageFakes = new MasterPageFakes(_masterPage);
            ShimSurveyReport.AllInstances.MasterGet = (p) => _masterPage;
            ShimBaseDataList.AllInstances.DataBind = (dataList) => { };
            ShimPage.AllInstances.IsValidGet = (p) => true;
            _lblErrorMessage = GetPropertyOrField<Label>(LblErrorMessageId);
            _lblErrorMessage.Visible = false;
            BusinessLayerFakes.ShimGroup.SaveGroupUser = (grp, user) => 1;
            BusinessLayerFakes.ShimUserGroup.SaveUserGroupUser = (grp, user) => 1;
            KM.Platform.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, code, feature, view) => true;
            BusinessLayerFakes.ShimAccessCheck.CanAccessByCustomerListOf1IListOfM0User<Group>((groupList, user) => true);
        }

        private T GetPropertyOrField<T>(string name)
        {
            return Get<T>(_surveyReportPrivateObject, name);
        }
    }
}
