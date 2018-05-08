using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Group;
using ecn.communicator.main.ECNWizard.Group.Fakes;
using ecn.controls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using CommunicatorEntites = ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Group
{
    public partial class GroupExplorerTest
    {
        private const string MethodGvSupressionCommand = "gvSupression_Command"; 
        private const string SupressionGroupsKey = "SupressionGroups_List";
        private const string GvSupressionPropertyName = "gvSupression";
        private const string SupressoinGridLabel = "lblEmptyGrid_Supression";
        private const string GvSupressionGroupsPropertyName = "gvSupression"; 
        private const string Suppress = "suppress"; 
        private const string ShowSelectPropertyName = "hfShowSelect"; 
        private const string FilterEdit1PropertyName = "filterEdit1"; 
        private const string Zero = "0";
        private const string GroupId = "1";

        [Test]
        public void GvSupression_Command_WhenImportsubsCommand_SetPageControlValues()
        {
            // Arrange
            var groupId = GroupId;
            var commandArgs = GetViewCommandEventArgs(ImportsubsCommand, groupId);
            SetFakesForGvSelectedGroupCommandMethod();

            // Act
            _privateGroupExplorerObj.Invoke(MethodGvSupressionCommand, this, commandArgs);

            // Assert
            _privateGroupExplorerObj.ShouldSatisfyAllConditions(
                () => Get<HiddenField>(_privateGroupExplorerObj, SelectedGroupIDPropertyName).Value.ShouldBe(groupId),
                () => Get<newGroup_Import>(_privateGroupExplorerObj, ImportSubscribers1).GroupID.ShouldBe(1));
        }

        [Test]
        public void GvSupression_Command_WhenAddsubsCommand_SetPageControlValues()
        {
            // Arrange
            var groupId = GroupId;
            var commandArgs = GetViewCommandEventArgs(AddsubsCommand, groupId);
            SetFakesForGvSelectedGroupCommandMethod();

            // Act
            _privateGroupExplorerObj.Invoke(MethodGvSupressionCommand, this, commandArgs);

            // Assert
            _privateGroupExplorerObj.ShouldSatisfyAllConditions(
                () => Get<HiddenField>(_privateGroupExplorerObj, SelectedGroupIDPropertyName).Value.ShouldBe(groupId),
                () => Get<newGroup_add>(_privateGroupExplorerObj, AddSubscribers1).GroupID.ShouldBe(1),
                () => _isNewGroupDropdownLoaded.ShouldBeTrue());
        }

        [Test]
        public void GvSupression_Command_WhenRemovedGroupCommand_SetPageControlValues()
        {
            // Arrange
            var groupId = GroupId;
            var commandArgs = GetViewCommandEventArgs(RemoveGroupCommand, groupId);
            SetFakesForGvSelectedGroupCommandMethod();
            SetGvSelectedGroupsGrid();
            var viewState = (StateBag)_privateGroupExplorerObj.GetProperty(ViewStatePropertyName);
            viewState.Add(SupressionGroupsKey, GetGroupList());

            // Act
            _privateGroupExplorerObj.Invoke(MethodGvSupressionCommand, this, commandArgs);
            var grid = Get<ecnGridView>(_privateGroupExplorerObj, GvSupressionPropertyName);


            // Assert
            _privateGroupExplorerObj.ShouldSatisfyAllConditions(
                () => grid.Visible.ShouldBeFalse(),
                () => Get<Label>(_privateGroupExplorerObj, SupressoinGridLabel).Visible.ShouldBeTrue());
        }

        [Test]
        public void GvSupression_Command_WhenAddFilterCommand_SetPageControlValues()
        {
            // Arrange
            var groupId = "0";
            var commandArgs = GetViewCommandEventArgs(AddFilterCommand, groupId);
            SetFakesForGvSelectedGroupCommandMethod();
            SetGvSupressionGroupsGrid();
            var viewState = (StateBag)_privateGroupExplorerObj.GetProperty(ViewStatePropertyName);
            viewState.Add(SupressionGroupsKey, GetGroupList());

            // Act
            _privateGroupExplorerObj.Invoke(MethodGvSupressionCommand, this, commandArgs);
            var grid = Get<ecnGridView>(_privateGroupExplorerObj, GvSupressionPropertyName);

            // Assert
            _privateGroupExplorerObj.ShouldSatisfyAllConditions(
                () => grid.Visible.ShouldBeTrue(),
                () => Get<Label>(_privateGroupExplorerObj, SupressoinGridLabel).Visible.ShouldBeTrue(),
                () => Get<Button>(_privateGroupExplorerObj, BtnFilterEditClose).CommandName.ShouldBe(Suppress));
        }

        [Test]
        public void GvSupression_Command_WhenEditCustomFilterCommand_SetPageControlValues()
        {
            // Arrange
            var groupId = "7";
            var commandArgs = GetViewCommandEventArgs(EditCustomFilterCommand, groupId);
            SetFakesForGvSelectedGroupCommandMethod();
            SetGvSupressionGroupsGrid();
            var viewState = (StateBag)_privateGroupExplorerObj.GetProperty(ViewStatePropertyName);
            viewState.Add(SupressionGroupsKey, GetGroupList());

            // Act
            _privateGroupExplorerObj.Invoke(MethodGvSupressionCommand, this, commandArgs);
            var grid = Get<ecnGridView>(_privateGroupExplorerObj, GvSupressionPropertyName);

            // Assert
            _privateGroupExplorerObj.ShouldSatisfyAllConditions(
                () => grid.Visible.ShouldBeTrue(),
                () => Get<HiddenField>(_privateGroupExplorerObj, ShowSelectPropertyName).Value.ShouldBe(Zero),
                () => Get<filters>(_privateGroupExplorerObj, FilterEdit1PropertyName).selectedFilterID.ShouldBe(7));
        }

        [TestCase(DeletesfilterCommand)]
        [TestCase(Deletecustomfilter)]
        public void GvSupression_Command_WhenDeleteFilterCommand_SetPageControlValues(string commandName)
        {
            // Arrange
            var groupId = "1_1";
            var commandArgs = GetViewCommandEventArgs(commandName, groupId);
            SetFakesForGvSelectedGroupCommandMethod();
            SetGvSupressionGroupsGrid();
            var viewState = (StateBag)_privateGroupExplorerObj.GetProperty(ViewStatePropertyName);
            viewState.Add(SupressionGroupsKey, GetGroupList());

            // Act
            _privateGroupExplorerObj.Invoke(MethodGvSupressionCommand, this, commandArgs);
            var grid = Get<ecnGridView>(_privateGroupExplorerObj, GvSupressionPropertyName);

            // Assert
            _privateGroupExplorerObj.ShouldSatisfyAllConditions(
                () => grid.ShouldNotBeNull(),
                () => grid.Visible.ShouldBeTrue());
        }

        private void SetGvSupressionGroupsGrid()
        {
            var gvSelectedGroups = (ecnGridView)_privateGroupExplorerObj.GetFieldOrProperty(GvSupressionGroupsPropertyName);
            gvSelectedGroups.DataSource = new List<GroupObject>
            {
                new GroupObject { GroupID = 1, GroupName = SampleGroup }
            };
            gvSelectedGroups.DataBind();
            var tableCell = new TableCell();
            tableCell.Controls.Add(new Label { ID = LabelGroupID, Text = "1" });
            var filterstableCell = new TableCell();
            filterstableCell.Controls.Add(new filtergrid() { ID = FilterGridID });
            gvSelectedGroups.Rows[0].Controls.Add(tableCell);
            gvSelectedGroups.Rows[0].Controls.Add(filterstableCell);
        }
    }
}
