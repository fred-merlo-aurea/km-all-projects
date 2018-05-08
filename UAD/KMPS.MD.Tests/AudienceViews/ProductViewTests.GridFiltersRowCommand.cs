using System.Collections;
using System.Collections.Generic;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using KMPlatform.Object.Fakes;
using KMPS.MD.Controls.Fakes;
using KMPS.MD.Main.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;
using ShimAdhoc = KMPS.MD.Controls.Fakes.ShimAdhoc;
using TelerikUI = Telerik.Web.UI;

namespace KMPS.MD.Tests.AudienceViews
{
    public partial class ProductViewTests
    {
        private const string CommandCancel = "Cancel";
        private const string CommandEdit = "Edit";
        private const string GridFiltersRowCommand = "grdFilters_RowCommand";
        private const string HiddenFiledFilterNo = "hfFilterNo";
        private const string HiddenFiledFilterName = "hfFilterName";
        private const string HiddenFiledFilterGroupName = "hfFilterGroupName";
        private const string FilterObject = "fc";

        [Test]
        public void GridFiltersRowCommand_GridViewCommandEventArgsHaveCancelCommand_UpdateControlValues()
        {
            // Arrange
            ShimProductView.AllInstances.ResetFilterTabControls = (sender) => { };
            var gridViewRow = CreateGridViewRow();
            ShimControl.AllInstances.NamingContainerGet = (sender) => gridViewRow;
            ShimControl.AllInstances.FindControlString = (sender, controlId) => GetControlById(controlId);
            var gridViewCommandEventArgs = new GridViewCommandEventArgs(
                gridViewRow,
                gridViewRow,
                new CommandEventArgs(CommandCancel, TestOne));
            CreateFiltersObject();
            var parameters = new object[] { this, gridViewCommandEventArgs };

            // Act
            PrivatePage.Invoke(GridFiltersRowCommand, parameters);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<HiddenField>(HiddenFiledFilterNo).Value.ShouldBe(TestOne),
                () => GetField<HiddenField>(HiddenFiledFilterName).Value.ShouldBe(Test),
                () => GetField<HiddenField>(HiddenFiledFilterGroupName).Value.ShouldBe(Test));
        }
        
        [Test]
        public void GridFiltersRowCommand_GridViewCommandEventArgsHaveEditCommand_UpdateControlValues()
        {
            // Arrange
            var gridViewRow = CreateGridViewRow();
            var pubList = CreatePubsListObject();
            ShimProductView.AllInstances.ResetFilterTabControls = (sender) => { };
            ShimPubs.GetSearchEnabledByBrandIDClientConnectionsInt32 = (x, y) => pubList;
            ShimPubs.GetSearchEnabledClientConnections = (x) => pubList;
            ShimControl.AllInstances.NamingContainerGet = (sender) => gridViewRow;
            ShimUtilities.SelectFilterListBoxesListBoxString = (x, y) => { };
            ShimGridView.AllInstances.RowsGet = (x) => CreateGridViewRowCollectionObject();
            ShimRepeater.AllInstances.ItemsGet = (sender) => CreateRepeaterItemCollectionObject();
            ShimDataList.AllInstances.ItemsGet = (sender) => CreateDataListItemCollection();
            ShimControl.AllInstances.FindControlString = (sender, controlId) => GetControlById(controlId);
            var listMasterCodeSheet = CreateMasterCodeSheetListObject();
            ShimMasterCodeSheet.GetSearchEnabledByBrandIDClientConnectionsInt32 = (x, y) => listMasterCodeSheet;
            ShimMasterCodeSheet.GetSearchEnabledClientConnections = (x) => listMasterCodeSheet;
            ShimMasterGroup.GetByIDClientConnectionsInt32 = (x, y) => new MasterGroup { ColumnReference = Test };
            ShimActivity.AllInstances.LoadActivityFiltersField = (sender, filed) => { };
            ShimAdhoc.AllInstances.LoadAdhocFiltersField = (sender, field) => { };
            ShimCirculation.AllInstances.LoadCirculationFiltersField = (sender, field) => { };
            BindRadComboBox();
            CreateFiltersObject();
            var gridViewCommandEventArgs = new GridViewCommandEventArgs(
                gridViewRow,
                gridViewRow,
                new CommandEventArgs(CommandEdit, TestOne));
            var parameters = new object[] { this, gridViewCommandEventArgs };
            CreatePageShimObject(true);
            ShimCodeSheet.GetByResponseGroupIDClientConnectionsInt32 = (_, __) => new List<CodeSheet>
            {
                new CodeSheet
                {
                    CodeSheetID = DummyId,
                    ResponseDesc = DummyString,
                    ResponseValue = DummyString
                }
            };
            ShimResponseGroup.GetByResponseGroupIDClientConnectionsInt32 = (_, __) => new ResponseGroup
            {
                ResponseGroupName = Test
            };
            BindRadComboBoxForGetFilter();

            // Act
            PrivatePage.Invoke(GridFiltersRowCommand, parameters);

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => GetField<HiddenField>(HiddenFiledFilterNo).Value.ShouldBe(TestOne),
                () => GetField<HiddenField>(HiddenFiledFilterName).Value.ShouldBe(Test),
                () => GetField<HiddenField>(HiddenFiledFilterGroupName).Value.ShouldBe(Test));
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

        private void BindRadComboBox()
        {
            var radComboBoxItemCollection = new string[] { "1", "2", "3" };
            var comboBoxIdList = GetComboBoxIdList();
            foreach (var item in comboBoxIdList)
            {
                var radComboBox = GetField<TelerikUI.RadComboBox>(item.ToString());
                radComboBox.DataSource = radComboBoxItemCollection;
                radComboBox.DataBind();
            }
        }

        private void CreateFiltersObject()
        {
            _filters = new Filters(new ShimClientConnections(), DummyId)
            {
                FilterComboList = new List<FilterCombo>
                {
                    new FilterCombo {SelectedFilterNo = TestOne}
                }
            };
            _filters.Add(new Filter
            {
                FilterNo = DummyId,
                FilterName = Test,
                FilterGroupID = DummyId,
                FilterGroupName = Test,
                Fields = CreateFieldListObject()
            });
            ReflectionHelper.SetValue(_testEntity, FilterObject, _filters);
        }
    }
}
