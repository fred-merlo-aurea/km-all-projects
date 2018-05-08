using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Fakes;
using System.Web.SessionState.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.communicator.main.ECNWizard.Group.Fakes;
using ecn.controls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Fakes;
using NUnit.Framework;
using Shouldly;
using CommFakeDataLayer = ECN_Framework_DataLayer.Communicator.Fakes;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;
using KmPlatformBusinessFakes = KMPlatform.BusinessLogic.Fakes;
using KmPlatformFakes = KM.Platform.Fakes;

namespace ECN.Communicator.Tests.Main.ECNWizard.Group
{
    public partial class FiltersTest
    {
        private const int FilterId = 10;
        private const int FilterGroupId = 100;
        private const string MethodNodeChanged = "NodeChanged";
        private const string MethodBtnAddFilterConditionClick = "btnAddFilterCondition_Click";
        private const string MethodGvFilterConditionRowCommand = "gvFilterCondition_RowCommand";
        private const string MethodDdlFieldSelectedIndexChanged = "ddlField_SelectedIndexChanged";
        private const string MethodDdlFieldTypeSelectedIndexChanged = "ddlFieldType_SelectedIndexChanged";
        private const string MethodDdlDatePartSelectedIndexChanged = "ddlDatePart_SelectedIndexChanged";
        private const string MethodDdlComparatorSelectedIndexChanged = "ddlComparator_SelectedIndexChanged";
        private const string MethodEvalWithMaxLength = "EvalWithMaxLength";
        private const string MethodPageLoad = "Page_Load";
        private const string MethodBtnAddFilterGroupClick = "btnAddFilterGroup_Click";
        private const string MethodBtnAddFilterClick = "btnAddFilter_Click";
        private const string MethodBtnPreviewClick = "btnPreview_Click";
        private const string MethodBtnSaveDateClick = "btnSaveDate_Click";
        private const string MethodGvFilterGroupRowCommand = "gvFilterGroup_RowCommand";
        private const string MethodLbAddConditionClick = "lbAddCondition_Click";
        private const string MethodLbAddClick = "lbAdd_Click";
        private const string MethodRbSelectCheckedChanged = "rbSelect_CheckedChanged";
        private const string MethodRbTodayPlusCheckedChanged = "rbTodayPlus_CheckedChanged";
        private const string MethodRbTodayCheckedChanged = "rbToday_CheckedChanged";
        private const string MethodReset = "reset";
        private const string PnlEMessageId = "pnlEMessage";
        private const string TxtFilterGroupNameId = "txtFilterGroupName";
        private const string PhErrorFilterGroupId = "phError_FilterGroup";
        private const string DDLConditionCompareTypeId = "ddlConditionCompareType";
        private const string BtnAddFilterConditionId = "btnAddFilterCondition";
        private const string LblErrorMessageFilterConditionId = "lblErrorMessage_FilterCondition";
        private const string BtnAddFilterGroupId = "btnAddFilterGroup";
        private const string BtnAddFilterId = "btnAddFilter";
        private const string TvFilterId = "tvFilter";
        private const string PnlConditionId = "pnlCondition";
        private const string ChkboxNotId = "cbxNot";
        private const string TxtFilterNameId = "txtFilterName";
        private const string DDLFieldId = "ddlField";
        private const string DDLComparatorId = "ddlComparator";
        private const string DDLDatePartId = "ddlDatePart";
        private const string DDLFieldTypeId = "ddlFieldType";
        private const string LblCurrentNameId = "lblCurrentName";
        private const string GvFilterGroupId = "gvFilterGroup";
        private const string GvFilterConditionId = "gvFilterCondition";
        private const string IBChooseDateId = "ibChooseDate";
        private const string TxtCompareValueId = "txtCompareValue";
        private const string LblFilterGroupId = "lblFilterGroupID";
        private const string LblRefGroupId = "lblRefGroupID";
        private const string LblFilterConditionId = "lblFilterConditionID";
        private const string DDLGroupCompareTypeId = "ddlGroupCompareType";
        private const string PhErrorId = "phError";
        private const string RbSelectId = "rbSelect";
        private const string RbTodayPlusId = "rbTodayPlus";
        private const string RbTodayId = "rbToday";
        private const string RfvDaysId = "rfvDays";
        private const string RvDaysId = "rvDays";
        private const string TxtDaysId = "txtDays";
        private const string DdlPlusMinusId = "ddlPlusMinus";
        private const string TxtDatePickerId = "txtDatePicker";
        private const string FilterGroupName = "FilterGroupName";
        private CommunicatorEntities.FilterCondition _savedFilterCondition;
        private CommunicatorEntities.FilterGroup _savedFitlerGroup;
        private CommunicatorEntities.Filter _savedFilter;
        private bool _deleteFilterConditionCalled;
        private bool _deleteFilterGroupCalled;

        [TestCase("Filter", "Date", "full")]
        [TestCase("FilterGroup", "String", "")]
        [TestCase("FilterGroup", "Date", "")]
        [TestCase("FilterGroup", "Date", "full")]
        [TestCase("FilterCondition", "", "full")]
        [TestCase("FilterCondition", "Date", "full")]
        [TestCase("FilterCondition", "String", "")]
        public void NodeChanged_DifferentFilterTypes_ProperGridBindCalled(string tvSelectedToolTip, string selectedFieldType, string selectedDatePart)
        {
            // Arrange
            InitTestNodeChanged(tvSelectedToolTip: tvSelectedToolTip, selectedDatePart: selectedDatePart, selectedFieldType: selectedFieldType);
            var lblCurrentName = Get<Label>(_privateFilterObj, LblCurrentNameId);
            var gvCondition = Get<ecnGridView>(_privateFilterObj, GvFilterConditionId);
            var gvGroup = Get<ecnGridView>(_privateFilterObj, GvFilterGroupId);
            var grdConditionDataBound = false;
            var grdGroupDataBound = false;
            ShimGridView.AllInstances.DataBind = (gridview) =>
            {
                if (gridview == gvGroup)
                {
                    grdGroupDataBound = true;
                }
                else if (gridview == gvCondition)
                {
                    grdConditionDataBound = true;
                }
            };
            var expectedCurrentName = tvSelectedToolTip == "Filter"
                ? "Filter Groups"
                : tvSelectedToolTip == "FilterGroup"
                    ? "Filter Conditions"
                    : "Filter Condition";
            var txtCompareValue = Get<TextBox>(_privateFilterObj, TxtCompareValueId);
            var ibDate = Get<ImageButton>(_privateFilterObj, IBChooseDateId);

            // Act
            _privateFilterObj.Invoke(MethodNodeChanged, new object[] { });

            // Assert
            lblCurrentName.ShouldSatisfyAllConditions(
                () => lblCurrentName.Text.ShouldBe(expectedCurrentName),
                () => grdGroupDataBound.ShouldBe(tvSelectedToolTip == "Filter"),
                () => grdConditionDataBound.ShouldBe(tvSelectedToolTip == "FilterGroup" || tvSelectedToolTip == "FilterCondition"),
                () => ibDate.Visible.ShouldBe(tvSelectedToolTip == "Filter"),
                () => txtCompareValue.Enabled.ShouldBe(tvSelectedToolTip == "Filter"));
        }

        [TestCase("Add", true, true, FilterId)]
        [TestCase("Add", true, false, FilterId)]
        [TestCase("Add", true, false, -10)]
        [TestCase("", false, false, FilterId)]
        public void BtnAddFilterCondition_Click_DifferentCondition_NoErrors(string btnFilterConditionText, bool chkBoxNotValue, bool emptyFilterConditionList, int selectedFilterId)
        {
            // Arrange
            InitTestBtnAddFilterConditionClick(
                btnFilterConditionText: btnFilterConditionText,
                chkBoxNotValue: chkBoxNotValue,
                emptyFilterConditionList: emptyFilterConditionList,
                selectedFilterId: selectedFilterId);
            var errorLabel = Get<Label>(_privateFilterObj, LblErrorMessageFilterConditionId);
            var tvFilter = Get<TreeView>(_privateFilterObj, TvFilterId);

            // Act
            _privateFilterObj.Invoke(MethodBtnAddFilterConditionClick, new object[] { null, EventArgs.Empty });

            // Assert
            _savedFilterCondition.ShouldSatisfyAllConditions(
                  () => errorLabel.Text.ShouldBeNullOrEmpty(),
                  () => _savedFilterCondition.ShouldNotBeNull(),
                  () => _savedFilterCondition.NotComparator.ShouldNotBeNull(),
                  () => _savedFilterCondition.NotComparator.ShouldBe(chkBoxNotValue
                  ? 1
                  : 0),
                  () => _savedFilterCondition.SortOrder.ShouldBe(1),
                  () => tvFilter.Visible.ShouldBe(selectedFilterId > 0));
        }

        [Test]
        public void BtnAddFilterCondition_Click_SaveError_Error()
        {
            // Arrange
            InitTestBtnAddFilterConditionClick(btnFilterConditionText: "Add", saveError: true);
            var errorLabel = Get<Label>(_privateFilterObj, LblErrorMessageFilterConditionId);

            // Act
            _privateFilterObj.Invoke(MethodBtnAddFilterConditionClick, new object[] { null, EventArgs.Empty });

            // Assert
            errorLabel.Text.ShouldNotBeNullOrEmpty();
        }

        [TestCase("Edit", false)]
        [TestCase("Delete", false)]
        [TestCase("Delete", true)]
        public void GvFilterCondition_RowCommand_DifferentCommand_CommandHandledCorrectly(string cmdName, bool deleteError)
        {
            // Arrange
            InitTestGvFilterConditionRowCommand(deleteError: deleteError);
            var phError = Get<PlaceHolder>(_privateFilterObj, PhErrorId);
            phError.Visible = false;

            // Act
            _privateFilterObj.Invoke(MethodGvFilterConditionRowCommand, new object[] { null, new GridViewCommandEventArgs(null, new CommandEventArgs(cmdName, 10)) });

            // Assert
            phError.ShouldSatisfyAllConditions(
               () => _deleteFilterConditionCalled.ShouldBe(cmdName == "Delete" && !deleteError),
               () => phError.Visible.ShouldBe(deleteError));
        }

        [TestCase("String")]
        [TestCase("Date")]
        [TestCase("Number")]
        [TestCase("Money")]
        public void DdlField_SelectedIndexChanged_DifferentFieldType_ComparatorItemsInitialized(string selectedFieldType)
        {
            // Arrange
            var setupValueEntryCalled = false;
            var setupFieldTypeCalled = false;
            InitTestDdlFieldSelectedIndexChanged(out DropDownList ddlComparator, selectedFieldType: selectedFieldType);
            Shimfilters.AllInstances.SetupValueEntryString = (p, field) => { setupValueEntryCalled = true; };
            Shimfilters.AllInstances.SetupFieldTypeString = (p, filed) => { setupFieldTypeCalled = true; };
            var expectedComparatorItemsCount = selectedFieldType == "String"
                ? 6
                : selectedFieldType == "Date"
                ? 4
                : 5;

            // Act
            _privateFilterObj.Invoke(MethodDdlFieldSelectedIndexChanged, new object[] { null, EventArgs.Empty });

            // Assert
            setupValueEntryCalled.ShouldSatisfyAllConditions(
                () => setupFieldTypeCalled.ShouldBeTrue(),
                () => setupValueEntryCalled.ShouldBeTrue(),
                () => ddlComparator.Items.Count.ShouldBe(expectedComparatorItemsCount));
        }

        [Test]
        public void DdlFieldType_SelectedIndexChanged_ProperMethodCalled()
        {
            // Arrange
            InitCommon();
            var setupValueEntryCalled = false;
            var loadComparatorsCalled = false;
            Shimfilters.AllInstances.SetupValueEntryString = (p, field) => { setupValueEntryCalled = true; };
            Shimfilters.AllInstances.LoadComparatorsString = (p, filed) => { loadComparatorsCalled = true; };

            // Act
            _privateFilterObj.Invoke(MethodDdlFieldTypeSelectedIndexChanged, new object[] { null, EventArgs.Empty });

            // Assert
            loadComparatorsCalled.ShouldSatisfyAllConditions(
              () => loadComparatorsCalled.ShouldBeTrue(),
              () => setupValueEntryCalled.ShouldBeTrue());
        }

        [Test]
        public void DdlDatePart_SelectedIndexChanged_ProperMethodCalled()
        {
            InitCommon();
            var setupValueEntryCalled = false;
            var loadComparatorsCalled = false;
            Shimfilters.AllInstances.SetupValueEntryString = (p, field) => { setupValueEntryCalled = true; };
            Shimfilters.AllInstances.LoadComparatorsString = (p, filed) => { loadComparatorsCalled = true; };

            // Act
            _privateFilterObj.Invoke(MethodDdlDatePartSelectedIndexChanged, new object[] { null, EventArgs.Empty });

            // Assert
            loadComparatorsCalled.ShouldSatisfyAllConditions(
              () => loadComparatorsCalled.ShouldBeTrue(),
              () => setupValueEntryCalled.ShouldBeTrue());
        }

        [TestCase("is empty", "", "")]
        [TestCase("", "Date", "full")]
        [TestCase("", "Date", "")]
        [TestCase("", "String", "")]
        [TestCase("", "", "")]
        public void DdlComparator_SelectedIndexChanged_ControlsInitializedCorrectly(string comparatorSelectedValue, string fieldTypeSelectedValue, string ddlDatePartSelectedValue)
        {
            // Arrange
            InitTestDdlComparatorSelectedIndexChanged(comparatorSelectedValue: comparatorSelectedValue, fieldTypeSelectedValue: fieldTypeSelectedValue, ddlDatePartSelectedValue: ddlDatePartSelectedValue);
            var txtCompareValue = Get<TextBox>(_privateFilterObj, TxtCompareValueId);
            var ibDate = Get<ImageButton>(_privateFilterObj, IBChooseDateId);

            // Act
            _privateFilterObj.Invoke(MethodDdlComparatorSelectedIndexChanged, new object[] { null, EventArgs.Empty });

            // Assert
            ibDate.ShouldSatisfyAllConditions(
                () => ibDate.Visible.ShouldBe(comparatorSelectedValue == string.Empty && fieldTypeSelectedValue == "Date" && ddlDatePartSelectedValue == "full"),
                () => txtCompareValue.Enabled
                .ShouldBe(comparatorSelectedValue == string.Empty
                          && (fieldTypeSelectedValue == "String"
                          || fieldTypeSelectedValue == string.Empty
                          || (fieldTypeSelectedValue == "Date" && ddlDatePartSelectedValue == string.Empty))));
        }

        [TestCase(null, 1, null)]
        [TestCase("12345", 5, "12345")]
        [TestCase("123456789", 5, "12...")]
        [TestCase("123456789", 6, "123...")]
        public void EvalWithMaxLength_SubstringMaxLength(string fieldName, int maxLength, string expectedResult)
        {
            // Arrange
            ShimTemplateControl.AllInstances.EvalString = (page, text) => fieldName;

            // Act
            var result = _privateFilterObj.Invoke(MethodEvalWithMaxLength, new object[] { fieldName, maxLength }) as string;

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void Page_Load_LoadDataCalled()
        {
            // Arrange
            InitCommon();
            bool loadDataCalled = false;
            Shimfilters.AllInstances.loadData = (p) => loadDataCalled = true;
            QueryString.Add("FilterId", "10");

            // Act
            _privateFilterObj.Invoke(MethodPageLoad, new object[] { null, EventArgs.Empty });

            // Assert
            loadDataCalled.ShouldBeTrue();
        }

        [Test]
        public void BtnAddFilterGroup_Click_SaveError_Error()
        {
            // Arrange
            InitTestBtnAddFilterGroupClick(btnFilterGroupText: "Add", saveError: true);
            var phError = Get<PlaceHolder>(_privateFilterObj, PhErrorFilterGroupId);
            phError.Visible = false;

            // Act
            _privateFilterObj.Invoke(MethodBtnAddFilterGroupClick, new object[] { null, EventArgs.Empty });

            // Assert
            phError.Visible.ShouldBeTrue();
        }

        [TestCase("Add", true)]
        [TestCase("", true)]
        [TestCase("", false)]
        public void BtnAddFilterGroup_Click_DifferentFilterGroups_NoErrors(string btnFilterGroupText, bool emptyFilterGroupList)
        {
            // Arrange
            InitTestBtnAddFilterGroupClick(btnFilterGroupText: btnFilterGroupText, emptyFilterGroupList: emptyFilterGroupList);
            var phError = Get<PlaceHolder>(_privateFilterObj, PhErrorFilterGroupId);
            phError.Visible = false;
            var txtFilterGroupName = Get<TextBox>(_privateFilterObj, TxtFilterGroupNameId);
            txtFilterGroupName.Text = "GroupName";

            // Act
            _privateFilterObj.Invoke(MethodBtnAddFilterGroupClick, new object[] { null, EventArgs.Empty });

            // Assert
            _savedFitlerGroup.ShouldSatisfyAllConditions(
                    () => phError.Visible.ShouldBeFalse(),
                    () => _savedFitlerGroup.ShouldNotBeNull(),
                    () => _savedFitlerGroup.Name.ShouldNotBeNull(txtFilterGroupName.Text),
                    () => _savedFitlerGroup.SortOrder.ShouldBe(1));
        }

        [Test]
        public void BtnAddFilter_Click_SaveError_Error()
        {
            // Arrange
            InitTestBtnAddFilterClick(btnFilterText: "Update", saveError: true);
            var phError = Get<PlaceHolder>(_privateFilterObj, PhErrorId);
            phError.Visible = false;

            // Act
            _privateFilterObj.Invoke(MethodBtnAddFilterClick, new object[] { null, EventArgs.Empty });

            // Assert
            phError.Visible.ShouldBeTrue();
        }

        [Test]
        public void BtnAddFilter_Click_SaveValidationError_Error()
        {
            // Arrange
            InitTestBtnAddFilterClick(btnFilterText: "Update", validationError: true);
            var phError = Get<PlaceHolder>(_privateFilterObj, PhErrorId);
            phError.Visible = false;

            // Act
            _privateFilterObj.Invoke(MethodBtnAddFilterClick, new object[] { null, EventArgs.Empty });

            // Assert
            phError.Visible.ShouldBeTrue();
        }

        [TestCase("Add", "")]
        [TestCase("Update", "filtersplusedit")]
        public void BtnAddFilter_Click_DifferentFilters_NoError(string filterText, string absUrl)
        {
            // Arrange
            InitTestBtnAddFilterClick(btnFilterText: filterText, requestUri: absUrl);
            var phError = Get<PlaceHolder>(_privateFilterObj, PhErrorId);
            var txtFilterName = Get<TextBox>(_privateFilterObj, TxtFilterNameId);
            txtFilterName.Text = "filterName";
            phError.Visible = false;
            var responseRedirectCalled = false;
            ShimHttpResponse.AllInstances.RedirectString = (response, url) => { responseRedirectCalled = true; };

            // Act
            _privateFilterObj.Invoke(MethodBtnAddFilterClick, new object[] { null, EventArgs.Empty });

            // Assert
            _savedFilter.ShouldSatisfyAllConditions(
                () => phError.Visible.ShouldBeFalse(),
                () => _savedFilter.ShouldNotBeNull(),
                () => _savedFilter.FilterName.ShouldBe(txtFilterName.Text),
                () => responseRedirectCalled.ShouldBe(absUrl == "filtersplusedit"));
        }

        [Test]
        public void LbAdd_Click_ControlsInitialized()
        {
            // Arrange
            InitCommon();
            Shimfilters.AllInstances.SetupAddFilterGroupNullableOfInt32 = (p, id) => { };
            Shimfilters.AllInstances.LoadGroupGridInt32 = (p, id) => { };
            var lblCurrentName = Get<Label>(_privateFilterObj, LblCurrentNameId);
            var pnlEMessage = Get<Panel>(_privateFilterObj, PnlEMessageId);

            // Act
            _privateFilterObj.Invoke(MethodLbAddClick, new object[] { null, EventArgs.Empty });

            //Assert
            lblCurrentName.ShouldSatisfyAllConditions(
                () => lblCurrentName.Text.ShouldBe("Filter Groups"),
                () => pnlEMessage.Visible.ShouldBeFalse());
        }

        [Test]
        public void LbAddCondition_Click_ControlsInitialized()
        {
            // Arrange
            InitCommon();
            Shimfilters.AllInstances.SetupAddFilterConditionNullableOfInt32Int32 = (p, id, groupId) => { };
            Shimfilters.AllInstances.LoadConditionGridInt32 = (p, id) => { };
            var lblCurrentName = Get<Label>(_privateFilterObj, LblCurrentNameId);
            var pnlCondition = Get<Panel>(_privateFilterObj, PnlConditionId);
            var lblFilterGroup = Get<Label>(_privateFilterObj, LblFilterGroupId);
            lblFilterGroup.Text = "10";
            pnlCondition.Visible = false;

            // Act
            _privateFilterObj.Invoke(MethodLbAddConditionClick, new object[] { null, EventArgs.Empty });

            //Assert
            lblCurrentName.ShouldSatisfyAllConditions(
                () => lblCurrentName.Text.ShouldBe("Filter Conditions"),
                () => pnlCondition.Visible.ShouldBeTrue());
        }

        [Test]
        public void Reset_FieldsInitialized()
        {
            // Arrange
            InitCommon();
            Shimfilters.AllInstances.loadData = (p) => { };

            // Act
            _privateFilterObj.Invoke(MethodReset, new object[] { });

            //Assert
            var selectedGroupID = Get<int>(_privateFilterObj, "selectedGroupID");
            var selectedFilterID = Get<int>(_privateFilterObj, "selectedFilterID");
            selectedFilterID.ShouldSatisfyAllConditions(
                () => selectedFilterID.ShouldBe(0),
                () => selectedGroupID.ShouldBe(0));
        }

        [Test]
        public void RbSelect_CheckedChanged_ControlsInitialized()
        {
            // Arrange
            InitCommon();
            var rbSelect = Get<RadioButton>(_privateFilterObj, RbSelectId);
            rbSelect.Checked = true;
            var rfvDays = Get<RequiredFieldValidator>(_privateFilterObj, RfvDaysId);
            var rvDays = Get<RangeValidator>(_privateFilterObj, RvDaysId);
            var txtDays = Get<TextBox>(_privateFilterObj, TxtDaysId);
            var ddlPlusMinus = Get<DropDownList>(_privateFilterObj, DdlPlusMinusId);

            // Act
            _privateFilterObj.Invoke(MethodRbSelectCheckedChanged, new object[] { null, EventArgs.Empty });

            //Assert
            ddlPlusMinus.ShouldSatisfyAllConditions(
                () => ddlPlusMinus.Enabled.ShouldBeFalse(),
                () => txtDays.Enabled.ShouldBeFalse(),
                () => rvDays.Enabled.ShouldBeFalse(),
                () => rfvDays.Enabled.ShouldBeFalse());
        }

        [Test]
        public void RbTodayPlus_CheckedChanged_ControlsInitialized()
        {
            // Arrange
            InitCommon();
            var rbTodayPlus = Get<RadioButton>(_privateFilterObj, RbTodayPlusId);
            rbTodayPlus.Checked = true;
            var rfvDays = Get<RequiredFieldValidator>(_privateFilterObj, RfvDaysId);
            var rvDays = Get<RangeValidator>(_privateFilterObj, RvDaysId);
            var txtDays = Get<TextBox>(_privateFilterObj, TxtDaysId);
            var ddlPlusMinus = Get<DropDownList>(_privateFilterObj, DdlPlusMinusId);
            ddlPlusMinus.Enabled = false;
            txtDays.Enabled = false;
            rvDays.Enabled = false;
            rfvDays.Enabled = false;

            // Act
            _privateFilterObj.Invoke(MethodRbTodayPlusCheckedChanged, new object[] { null, EventArgs.Empty });

            //Assert
            ddlPlusMinus.ShouldSatisfyAllConditions(
                () => ddlPlusMinus.Enabled.ShouldBeTrue(),
                () => txtDays.Enabled.ShouldBeTrue(),
                () => rvDays.Enabled.ShouldBeTrue(),
                () => rfvDays.Enabled.ShouldBeTrue());
        }

        [Test]
        public void RbToday_CheckedChanged_ControlsInitialized()
        {
            // Arrange
            InitCommon();
            var rbToday = Get<RadioButton>(_privateFilterObj, RbTodayId);
            rbToday.Checked = true;
            var rfvDays = Get<RequiredFieldValidator>(_privateFilterObj, RfvDaysId);
            var rvDays = Get<RangeValidator>(_privateFilterObj, RvDaysId);
            var txtDays = Get<TextBox>(_privateFilterObj, TxtDaysId);
            var ddlPlusMinus = Get<DropDownList>(_privateFilterObj, DdlPlusMinusId);

            // Act
            _privateFilterObj.Invoke(MethodRbTodayCheckedChanged, new object[] { null, EventArgs.Empty });

            //Assert
            ddlPlusMinus.ShouldSatisfyAllConditions(
                () => ddlPlusMinus.Enabled.ShouldBeFalse(),
                () => txtDays.Enabled.ShouldBeFalse(),
                () => rvDays.Enabled.ShouldBeFalse(),
                () => rfvDays.Enabled.ShouldBeFalse());
        }

        [TestCase("today", false, "10")]
        [TestCase("plus", true, "10")]
        [TestCase("plus", false, "10")]
        [TestCase("plus", false, "notNumber")]
        [TestCase("select", false, "10")]
        public void BtnSaveDate_Click_TextCompareValueInitialized(string checkedRadio, bool selectPlus, string textDays)
        {
            // Arrange
            InitTestBtnSaveDateClick(checkedRadio: checkedRadio, selectPlus: selectPlus, textDays: textDays);
            var txtCompareValue = Get<TextBox>(_privateFilterObj, TxtCompareValueId);
            var txtDatePicker = Get<TextBox>(_privateFilterObj, TxtDatePickerId);
            txtDatePicker.Text = "txtDatePicker";
            var expectedCompareValueText = string.Empty;
            var isValidDays = int.TryParse(textDays, out int days);
            if (checkedRadio == "today" && isValidDays)
            {
                expectedCompareValueText = "EXP:Today";
            }
            else if (checkedRadio == "select" && isValidDays)
            {
                expectedCompareValueText = txtDatePicker.Text;
            }
            else if (checkedRadio == "plus" && isValidDays && !selectPlus)
            {
                expectedCompareValueText = $"EXP:Today[-{textDays}]";
            }
            else if (checkedRadio == "plus" && isValidDays && selectPlus)
            {
                expectedCompareValueText = $"EXP:Today[+{textDays}]";
            }

            // Act
            _privateFilterObj.Invoke(MethodBtnSaveDateClick, new object[] { null, EventArgs.Empty });

            // Assert
            txtCompareValue.Text.ShouldBe(expectedCompareValueText);
        }

        [TestCase(0)]
        [TestCase(10)]
        public void btnPreview_Click_ErrorWhenInvalidSelectedFilterId(int selectedFilterId)
        {
            // Arrange
            InitCommon();
            var phError = Get<PlaceHolder>(_privateFilterObj, PhErrorId);
            phError.Visible = false;
            QueryString.Add("FilterId", null);
            Shimfilters.AllInstances.selectedFilterIDGet = (p) => selectedFilterId;

            //Act
            _privateFilterObj.Invoke(MethodBtnPreviewClick, new object[] { null, EventArgs.Empty });

            // Assert
            phError.Visible.ShouldBe(selectedFilterId <= 0);
        }

        [Test]
        public void GvFilterGroup_RowCommand_Edit_ControlsInitialized()
        {
            //Arrange
            var arg = new GridViewCommandEventArgs(null, new CommandEventArgs("Edit", FilterGroupId));
            InitTestGvFilterGroupRowCommand();
            var btnAddFilterGroup = Get<Button>(_privateFilterObj, BtnAddFilterGroupId);
            var txtFilterGroupName = Get<TextBox>(_privateFilterObj, TxtFilterGroupNameId);
            var pnlEMessage = Get<Panel>(_privateFilterObj, PnlEMessageId);

            //Act
            _privateFilterObj.Invoke(MethodGvFilterGroupRowCommand, new object[] { null, arg });

            // Assert
            btnAddFilterGroup.ShouldSatisfyAllConditions(
                () => btnAddFilterGroup.Text.ShouldBe("Update"),
                () => txtFilterGroupName.Text.ShouldBe(FilterGroupName),
                () => pnlEMessage.Visible.ShouldBeFalse());
        }

        [Test]
        public void GvFilterGroup_RowCommand_AddFilterCondition_ControlsInitialized()
        {
            //Arrange
            var arg = new GridViewCommandEventArgs(null, new CommandEventArgs("AddFilterCondition", FilterGroupId));
            InitTestGvFilterGroupRowCommand();
            var lblCurrentName = Get<Label>(_privateFilterObj, LblCurrentNameId);
            var pnlCondition = Get<Panel>(_privateFilterObj, PnlConditionId);
            pnlCondition.Visible = false;
            var lblFilterGroup = Get<Label>(_privateFilterObj, LblFilterGroupId);
            Shimfilters.AllInstances.LoadConditionGridInt32 = (p, id) => { };
            Shimfilters.AllInstances.SetupAddFilterConditionNullableOfInt32Int32 = (p, id, groupId) => { };

            //Act
            _privateFilterObj.Invoke(MethodGvFilterGroupRowCommand, new object[] { null, arg });

            // Assert
            lblCurrentName.ShouldSatisfyAllConditions(
                () => lblCurrentName.Text.ShouldBe("Filter Conditions"),
                () => lblFilterGroup.Text.ShouldBe(FilterGroupId.ToString()),
                () => pnlCondition.Visible.ShouldBeTrue());
        }

        [Test]
        public void GvFilterGroup_RowCommand_DeleteError_Error()
        {
            //Arrange
            var arg = new GridViewCommandEventArgs(null, new CommandEventArgs("Delete", FilterGroupId));
            InitTestGvFilterGroupRowCommand(deleteError: true);
            var phError = Get<PlaceHolder>(_privateFilterObj, PhErrorId);
            phError.Visible = false;

            //Act
            _privateFilterObj.Invoke(MethodGvFilterGroupRowCommand, new object[] { null, arg });

            // Assert
            phError.Visible.ShouldBeTrue();
        }

        [Test]
        public void GvFilterGroup_RowCommand_Delete_NoError()
        {
            //Arrange
            var arg = new GridViewCommandEventArgs(null, new CommandEventArgs("Delete", FilterGroupId));
            InitTestGvFilterGroupRowCommand();
            var phError = Get<PlaceHolder>(_privateFilterObj, PhErrorId);
            phError.Visible = false;

            //Act
            _privateFilterObj.Invoke(MethodGvFilterGroupRowCommand, new object[] { null, arg });

            // Assert
            phError.ShouldSatisfyAllConditions(
                () => phError.Visible.ShouldBeFalse(),
                () => _deleteFilterGroupCalled.ShouldBeTrue());
        }

        private void InitTestGvFilterGroupRowCommand(bool deleteError = false)
        {
            InitCommon();
            Shimfilters.AllInstances.loadData = (p) => { };
            var ddlConditionCompare = Get<DropDownList>(_privateFilterObj, DDLConditionCompareTypeId);
            ddlConditionCompare.Items.Add(new ListItem("OR", "OR"));
            ShimFilterGroup.GetByFilterGroupIDInt32User = (id, user) =>
            {
                return new CommunicatorEntities.FilterGroup()
                {
                    Name = FilterGroupName,
                    ConditionCompareType = "OR"
                };
            };
            if (deleteError)
            {
                ShimFilterGroup.DeleteInt32Int32User = (filterId, groupId, user) =>
                    {
                        throw new ECNException(new List<ECNError>()
                         {
                           new ECNError()
                         });
                    };
            }
            ShimFilter.CreateWhereClauseFilter = (filter) => string.Empty;
            ShimFilter.CreateDynamicWhereFilter = (filter) => string.Empty;
            ShimFilter.UpdateWhereClauseInt32StringUser = (id, filter, user) => { };
            ShimFilter.UpdateDynamicWhereInt32StringUser = (id, filter, user) => { };
            ShimFilter.GetByFilterIDInt32User = (id, user) => new CommunicatorEntities.Filter();
            ShimFilterGroup.ReSortInt32User = (id, user) => { };
            ShimFilterCondition.ReSortInt32User = (id, user) => { };
            ShimFilterCondition.GetByFilterGroupIDInt32User = (id, user) => new List<CommunicatorEntities.FilterCondition>();
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (cmd, conn) =>
            {
                if (cmd.CommandText == "e_FilterGroup_Delete_FilterGroupID")
                {
                    _deleteFilterGroupCalled = true;
                }
                return true;
            };
            ShimFilterGroup.ExistsInt32Int32 = (id, customerId) => true;
        }

        private void InitTestBtnSaveDateClick(string checkedRadio = "", bool selectPlus = true, string textDays = "10")
        {
            InitCommon();
            var rbToday = Get<RadioButton>(_privateFilterObj, RbTodayId);
            rbToday.Checked = checkedRadio == "today";
            var rbTodayPlus = Get<RadioButton>(_privateFilterObj, RbTodayPlusId);
            rbTodayPlus.Checked = checkedRadio == "plus";
            var rbSelect = Get<RadioButton>(_privateFilterObj, RbSelectId);
            rbSelect.Checked = checkedRadio == "select";
            var ddlPlusMinus = Get<DropDownList>(_privateFilterObj, DdlPlusMinusId);
            ddlPlusMinus.Items.Add(new ListItem("Plus", "Plus") { Selected = selectPlus });
            ddlPlusMinus.Items.Add(new ListItem("", "") { Selected = !selectPlus });
            var txtDays = Get<TextBox>(_privateFilterObj, TxtDaysId);
            txtDays.Text = textDays;
        }

        private void InitTestBtnAddFilterClick(string btnFilterText = "", bool saveError = false, string requestUri = "", bool validationError = false)
        {
            if (!saveError && !validationError)
            {
                ShimFilter.SaveFilterUser = (filter, user) =>
                {
                    _savedFilter = filter;
                    return 0;
                };
            }
            else if (saveError)
            {
                ShimFilter.ExistsInt32Int32 = (filterId, customerId) => false;
            }
            if (!string.IsNullOrWhiteSpace(requestUri))
            {
                ShimUserControl.AllInstances.RequestGet = (p) =>
                {
                    return new HttpRequest(string.Empty, $"{TestUrl}/{requestUri}", string.Empty);
                };
            }
            if (validationError)
            {
                ShimFilter.ExistsInt32Int32 = (id, custId) => true;
                ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer.ExistsInt32 = (id) => false;
                KmPlatformBusinessFakes.ShimUser.ExistsInt32Int32 = (userId, custId) => false;
                KmPlatformBusinessFakes.ShimUser.GetByUserIDInt32Boolean = (id, getChild) => new KMPlatform.Entity.User();
                KmPlatformFakes.ShimUser.IsSystemAdministratorUser = (user) => false;
                ShimGroup.ExistsInt32Int32 = (groupId, custId) => false;
            }
            var btnAddFilter = Get<Button>(_privateFilterObj, BtnAddFilterId);
            btnAddFilter.Text = btnFilterText;
            ShimFilter.GetByFilterIDInt32User = (id, user) => new CommunicatorEntities.Filter()
            {
                FilterID = FilterId,
                CustomerID = 10,
                CreatedUserID = 10,
                GroupID = 10,
            };
            Shimfilters.AllInstances.loadData = (p) => { };
        }

        private void InitTestBtnAddFilterGroupClick(string btnFilterGroupText = "", bool emptyFilterGroupList = false, bool saveError = false)
        {
            InitCommon();
            var lblFilterGroupID = Get<Label>(_privateFilterObj, LblFilterGroupId);
            lblFilterGroupID.Text = "20";
            var btnAddFilterGroup = Get<Button>(_privateFilterObj, BtnAddFilterGroupId);
            btnAddFilterGroup.Text = btnFilterGroupText;
            Shimfilters.AllInstances.loadData = (p) => { };
            ShimFilterGroup.SaveFilterGroupUser = (filterGroup, user) =>
            {
                _savedFitlerGroup = filterGroup;
                return 1;
            };
            ShimFilterGroup.GetByFilterGroupIDInt32User = (id, user) =>
            {
                return new CommunicatorEntities.FilterGroup();

            };
            ShimFilter.GetByFilterIDInt32User = (id, user) =>
            {
                var filterGroup = new CommunicatorEntities.Filter()
                {
                    FilterGroupList = new List<CommunicatorEntities.FilterGroup>()
                };
                if (!emptyFilterGroupList)
                {
                    filterGroup.FilterGroupList.Add(new CommunicatorEntities.FilterGroup() { SortOrder = 0 });
                }
                return filterGroup;
            };
            ShimFilterGroup.SaveFilterGroupUser = (group, user) =>
            {
                if (saveError)
                {
                    var errors = new List<ECNError>() { new ECNError() };
                    throw new ECNException(errors);
                }
                _savedFitlerGroup = group;
                return 0;
            };
        }

        private void InitTestDdlComparatorSelectedIndexChanged(string comparatorSelectedValue = "", string fieldTypeSelectedValue = "", string ddlDatePartSelectedValue = "")
        {
            InitCommon();
            var ddlComparator = Get<DropDownList>(_privateFilterObj, DDLComparatorId);
            ddlComparator.Items.Add(new ListItem(comparatorSelectedValue, comparatorSelectedValue) { Selected = true });
            var ddlFieldType = Get<DropDownList>(_privateFilterObj, DDLFieldTypeId);
            ddlFieldType.Items.Add(new ListItem("Date", "Date") { Selected = fieldTypeSelectedValue == "Date" });
            ddlFieldType.Items.Add(new ListItem("String", "String") { Selected = fieldTypeSelectedValue == "String" });
            ddlFieldType.Items.Add(new ListItem(string.Empty, string.Empty) { Selected = fieldTypeSelectedValue == string.Empty });
            var ddlDate = Get<DropDownList>(_privateFilterObj, DDLDatePartId);
            ddlDate.Items.Add(new ListItem("full", "full") { Selected = ddlDatePartSelectedValue == "full" });
            ddlDate.Items.Add(new ListItem(string.Empty, string.Empty) { Selected = ddlDatePartSelectedValue == string.Empty });
        }

        private void InitTestDdlFieldSelectedIndexChanged(out DropDownList ddlComparator, string selectedFieldType = "")
        {
            InitCommon();
            var ddlFieldType = Get<DropDownList>(_privateFilterObj, DDLFieldTypeId);
            ddlFieldType.Items.Add(new ListItem("Date", "Date") { Selected = selectedFieldType == "Date" });
            ddlFieldType.Items.Add(new ListItem("String", "String") { Selected = selectedFieldType == "String" });
            ddlFieldType.Items.Add(new ListItem("Number", "Number") { Selected = selectedFieldType == "Number" });
            ddlFieldType.Items.Add(new ListItem("Money", "Money") { Selected = selectedFieldType == "Money" });
            ddlComparator = Get<DropDownList>(_privateFilterObj, DDLComparatorId);
        }

        private void InitTestGvFilterConditionRowCommand(bool deleteError = false)
        {
            InitCommon();
            Shimfilters.AllInstances.SetupAddFilterConditionNullableOfInt32Int32 = (page, filterId, groupId) => { };
            Shimfilters.AllInstances.loadData = (page) => { };
            if (deleteError)
            {
                ShimFilterCondition.DeleteInt32Int32User = (grpId, filterId, user) =>
                throw new ECNException(new List<ECNError>()
                {
                    new ECNError()
                });
            }
            ShimFilterCondition.ReSortInt32User = (id, user) => { };
            ShimFilterGroup.GetByFilterGroupIDInt32User = (id, user) => new CommunicatorEntities.FilterGroup();
            ShimFilter.GetByFilterIDInt32User = (id, user) => new CommunicatorEntities.Filter();
            ShimFilter.CreateWhereClauseFilter = (filter) => string.Empty;
            ShimFilter.CreateDynamicWhereFilter = (filter) => string.Empty;
            ShimFilter.UpdateWhereClauseInt32StringUser = (id, filter, user) => { };
            ShimFilter.UpdateDynamicWhereInt32StringUser = (id, filter, user) => { };
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (cmd, connString) =>
            {
                if (cmd.CommandText == "e_FilterCondition_Delete_FilterConditionID")
                {
                    _deleteFilterConditionCalled = true;
                }
                return true;
            };
            CommFakeDataLayer.ShimFilterCondition.GetSqlCommand = (cmd) =>
            {

                if (cmd.CommandText == "e_FilterCondition_Select_FilterConditionID")
                {
                    return new CommunicatorEntities.FilterCondition();
                }
                return null;
            };
        }

        private void InitTestBtnAddFilterConditionClick(string btnFilterConditionText = "", bool chkBoxNotValue = true, bool emptyFilterConditionList = false, bool saveError = false, int selectedFilterId = FilterId)
        {
            InitCommon();
            ShimFilterCondition.GetByFilterConditionIDInt32User = (id, user) => new CommunicatorEntities.FilterCondition();
            var btnAddFilterCondition = Get<Button>(_privateFilterObj, BtnAddFilterConditionId);
            btnAddFilterCondition.Text = btnFilterConditionText;
            var ddlFieldType = Get<DropDownList>(_privateFilterObj, DDLFieldTypeId);
            ddlFieldType.Items.Add(new ListItem("Date", "Date") { Selected = true });
            var chkboxNot = Get<CheckBox>(_privateFilterObj, ChkboxNotId);
            chkboxNot.Checked = chkBoxNotValue;
            ShimFilterGroup.GetByFilterGroupIDInt32User = (id, user) =>
            {
                var filterGroup = new CommunicatorEntities.FilterGroup()
                {
                    FilterConditionList = new List<CommunicatorEntities.FilterCondition>()
                };
                if (!emptyFilterConditionList)
                {
                    filterGroup.FilterConditionList.Add(new CommunicatorEntities.FilterCondition() { SortOrder = 0 });
                }
                return filterGroup;
            };
            ShimFilterCondition.SaveFilterConditionUser = (filter, user) =>
            {
                if (saveError)
                {
                    var errors = new List<ECNError>() { new ECNError() };
                    throw new ECNException(errors);
                }
                _savedFilterCondition = filter;
                return 0;
            };
            ShimFilter.GetByFilterIDInt32User = (id, user) => new CommunicatorEntities.Filter();
            Shimfilters.AllInstances.selectedFilterIDGet = (p) => selectedFilterId;
            CommFakeDataLayer.ShimGroupDataFields.GetListSqlCommand = (cmd) =>
            {
                return new List<CommunicatorEntities.GroupDataFields>()
                {
                    new CommunicatorEntities.GroupDataFields() { ShortName= "shortName" }
                };
            };
            Shimfilters.AllInstances.NodeChanged = (p) => { };
            var ddlGroupCompareType = Get<DropDownList>(_privateFilterObj, DDLGroupCompareTypeId);
            ddlGroupCompareType.Items.Add(new ListItem("CompareType", "CompareType"));
            ddlGroupCompareType.Items.Add(new ListItem("OR", "OR"));
        }

        private void InitTestNodeChanged(string tvSelectedToolTip = "", string selectedFieldType = "", string selectedDatePart = "")
        {
            InitCommon();
            var tvFilter = Get<TreeView>(_privateFilterObj, TvFilterId);
            ShimTreeNode.AllInstances.ParentGet = (tree) => new TreeNode("parent")
            {
                Value = "10"
            };
            tvFilter.Nodes.Add(new TreeNode("filterId", "1")
            {
                Selected = true,
                ToolTip = tvSelectedToolTip
            });
            var ddlConditionCompare = Get<DropDownList>(_privateFilterObj, DDLConditionCompareTypeId);
            ddlConditionCompare.Items.Add(new ListItem("OR", "OR"));
            ShimFilterGroup.GetByFilterGroupIDInt32User = (id, user) =>
            {
                return new CommunicatorEntities.FilterGroup()
                {
                    FilterConditionList = new List<CommunicatorEntities.FilterCondition>()
                };
            };
            Shimfilters.AllInstances.SetupFieldTypeString = (p, fieldName) => { };
            var ddlFields = Get<DropDownList>(_privateFilterObj, DDLFieldId);
            ddlFields.Items.Add(new ListItem("Date", "Date") { Selected = selectedFieldType == "Date" });
            ddlFields.Items.Add(new ListItem("String", "String") { Selected = selectedFieldType == "String" });
            ddlFields.Items.Add(new ListItem(string.Empty, string.Empty) { Selected = selectedFieldType == string.Empty });
            var ddlFieldSelected = ddlFields.SelectedItem;
            ddlFields.Items.Remove(ddlFieldSelected);
            ddlFields.Items.Insert(0, ddlFieldSelected);
            var ddlComparator = Get<DropDownList>(_privateFilterObj, DDLComparatorId);
            ddlComparator.Items.Add(new ListItem("is empty", "is empty") { Selected = true });
            var ddlDate = Get<DropDownList>(_privateFilterObj, DDLDatePartId);
            ddlDate.Items.Add(new ListItem("full", "full") { Selected = selectedDatePart == "full" });
            ddlDate.Items.Add(new ListItem(string.Empty, string.Empty) { Selected = selectedDatePart == string.Empty });
            var ddlDateSelected = ddlDate.SelectedItem;
            ddlDate.Items.Remove(ddlDate.SelectedItem);
            ddlDate.Items.Insert(0, ddlDateSelected);
            var ddlFieldType = Get<DropDownList>(_privateFilterObj, DDLFieldTypeId);
            ddlFieldType.Items.Add(new ListItem("Date", "Date") { Selected = selectedFieldType == "Date" });
            ddlFieldType.Items.Add(new ListItem("String", "String") { Selected = selectedFieldType == "String" });
            ddlFieldType.Items.Add(new ListItem(string.Empty, string.Empty) { Selected = selectedFieldType == string.Empty });
            var ddlFieldTypeSelected = ddlFieldType.SelectedItem;
            ddlFieldType.Items.Remove(ddlFieldTypeSelected);
            ddlFieldType.Items.Insert(0, ddlFieldTypeSelected);
            var filterCondition = new CommunicatorEntities.FilterCondition()
            {
                DatePart = "full",
                Comparator = "is empty"
            };
            CommFakeDataLayer.ShimFilterCondition.GetSqlCommand = (cmd) =>
            {
                if (cmd.CommandText == "e_FilterCondition_Select_FilterConditionID")
                {
                    return filterCondition;
                }
                return null;
            };
            Shimfilters.AllInstances.LoadComparatorsString = (p, type) => { };
        }

        private void InitCommon()
        {
            _deleteFilterConditionCalled = false;
            _deleteFilterGroupCalled = false;
            _savedFilterCondition = null;
            KmPlatformFakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, code, feature, view) => true;
            ShimAccessCheck.CanAccessByCustomerOf1M0User<CommunicatorEntities.FilterCondition>((filter, user) => true);
            ShimAccessCheck.CanAccessByCustomerOf1M0User<CommunicatorEntities.FilterGroup>((filter, user) => true);
            ShimHttpSessionState.AllInstances.ItemGetString = (session, key) =>
            {
                if (key == "Filter")
                {
                    return new CommunicatorEntities.Filter()
                    {
                        GroupCompareType = "CompareType",
                        FilterID = FilterId,
                        FilterGroupList = new List<CommunicatorEntities.FilterGroup>()
                        {
                            new CommunicatorEntities.FilterGroup()
                            {
                                SortOrder = 1,
                            },
                            new CommunicatorEntities.FilterGroup()
                            {
                                 SortOrder = 10,
                            },
                        }
                    };
                };
                return null;
            };
            var lblRefGroupID = Get<Label>(_privateFilterObj, LblRefGroupId);
            var lblFilterConditionID = Get<Label>(_privateFilterObj, LblFilterConditionId);
            lblRefGroupID.Text = "10";
            lblFilterConditionID.Text = "10";
        }
    }
}
