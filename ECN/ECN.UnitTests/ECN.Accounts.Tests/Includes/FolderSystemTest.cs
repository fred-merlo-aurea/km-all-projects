using System;
using System.Collections.Generic;
using System.IO.Fakes;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.accounts.includes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KM.Framework.Web.WebForms.FolderSystem.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.Accounts.Tests.includes
{
    [TestFixture]
    public partial class FolderSystemTest
    {
        private folderSystem _testEntity;
        private PrivateObject _testEntityPrivate;
        private TreeView _treeViewFolders;
        private TreeNode _treeNode01;
        private TreeNode _treeNode02;
        private List<Folder> _foldersList;
        private IDisposable _shimsContext;

        [SetUp]
        public void SetUp()
        {
            _shimsContext = ShimsContext.Create();

            _testEntity = new folderSystem();
            _testEntityPrivate = new PrivateObject(_testEntity);

            var shimHttpServerUtility = new ShimHttpServerUtility();
            shimHttpServerUtility.MapPathString = (path) => { return string.Empty; };

            ShimUserControl.AllInstances.ServerGet = (userControl) => { return shimHttpServerUtility.Instance; };

            _treeViewFolders = new TreeView();
            _treeNode01 = new TreeNode(DummyTreeNode01Text, DummyTreeNode01Value);
            _treeNode02 = new TreeNode(DummyTreeNode02Text, DummyTreeNode02Value);
            _treeViewFolders.Nodes.Add(_treeNode01);
            _treeViewFolders.Nodes.Add(_treeNode02);

            _foldersList = new List<Folder>();

            _testEntityPrivate.SetField(TreeViewFoldersFieldName, _treeViewFolders);
            _testEntityPrivate.SetField(FolderListFieldName, _foldersList);
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
            _treeViewFolders?.Dispose();
            _testEntity?.Dispose();
        }

        [Test]
        public void CustomerId_SetAndGetValue_ReturnsExpectedValue()
        {
            // Arrange
            _testEntity.CustomerID = DefaultIntegerValue;

            // Act
            _testEntity.CustomerID = ExpectedIntegerValue;
            var returnedValue = _testEntity.CustomerID;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBe(ExpectedIntegerValue),
                () => returnedValue.ShouldNotBe(DefaultIntegerValue));
        }

        [Test]
        public void Width_SetAndGetValue_ReturnsExpectedValue()
        {
            // Arrange
            _testEntity.Width = DefaultIntegerValue;

            // Act
            _testEntity.Width = ExpectedIntegerValue;
            var returnedValue = _testEntity.Width;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBe(ExpectedIntegerValue),
                () => returnedValue.ShouldNotBe(DefaultIntegerValue));
        }

        [Test]
        public void Height_SetAndGetValue_ReturnsExpectedValue()
        {
            // Arrange
            _testEntity.Height = DefaultIntegerValue;

            // Act
            _testEntity.Height = ExpectedIntegerValue;
            var returnedValue = _testEntity.Height;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBe(ExpectedIntegerValue),
                () => returnedValue.ShouldNotBe(DefaultIntegerValue));
        }

        [Test]
        public void CssClass_SetAndGetValue_ReturnsExpectedValue()
        {
            // Arrange
            _testEntity.CssClass = DefaultStringValue;

            // Act
            _testEntity.CssClass = ExpectedStringValue;
            var returnedValue = _testEntity.CssClass;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBe(ExpectedStringValue),
                () => returnedValue.ShouldNotBe(DefaultStringValue));
        }
        
        [Test]
        public void FolderType_SetAndGetValue_ReturnsExpectedValue()
        {
            // Arrange
            _testEntity.FolderType = DefaultStringValue;

            // Act
            _testEntity.FolderType = ExpectedStringValue;
            var returnedValue = _testEntity.FolderType;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBe(ExpectedStringValue),
                () => returnedValue.ShouldNotBe(DefaultStringValue));
        }

        [Test]
        public void SelectedFolderId_GetValue_ReturnsTreeViewFoldersSelectedNodeValue()
        {
            // Arrange
            _treeNode02.Selected = true;

            // Act
            var returnedValue = _testEntity.SelectedFolderID;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBe(_treeNode02.Value),
                () => returnedValue.ShouldNotBe(_treeNode01.Value));
        }

        [Test]
        public void NodesExpanded_SetAndGetValue_ReturnsExpectedValue()
        {
            // Arrange
            _testEntity.NodesExpanded = DefaultBooleanValue;

            // Act
            _testEntity.NodesExpanded = ExpectedBooleanValue;
            var returnedValue = _testEntity.NodesExpanded;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBe(ExpectedBooleanValue),
                () => returnedValue.ShouldNotBe(DefaultBooleanValue));
        }

        [Test]
        public void ChildrenExpanded_SetAndGetValue_ReturnsExpectedValue()
        {
            // Arrange
            _testEntity.ChildrenExpanded = DefaultBooleanValue;

            // Act
            _testEntity.ChildrenExpanded = ExpectedBooleanValue;
            var returnedValue = _testEntity.ChildrenExpanded;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBe(ExpectedBooleanValue),
                () => returnedValue.ShouldNotBe(DefaultBooleanValue));
        }

        [Test]
        public void CreateTreeNode_WhenCalled_ReturnsNewTreeNode()
        {
            // Arrange
            const string nodeText = "text";
            const string nodeValue = "value";

            // Act
            var returnedValue = _testEntityPrivate.Invoke(CreateTreeNodeMethodName, nodeValue, nodeText) as TreeNode;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldNotBeNull(),
                () => returnedValue.ShouldBeOfType<TreeNode>(),
                () => returnedValue.Text.ShouldBe(nodeText),
                () => returnedValue.Value.ShouldBe(nodeValue));
        }

        [Test]
        public void GetFolders_IfFolderTypeEqualsToImg_ReturnsFolders()
        {
            // Arrange
            var directories = new string[] { "directory01", "directory02" };
            ShimDirectory.GetDirectoriesString = (path) => { return directories; };
            _testEntity.FolderType = FolderTypes.IMG.ToString();

            // Act
            var returnedValue = _testEntityPrivate.Invoke(GetFoldersMethodName) as List<Folder>;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldNotBeNull(),
                () => returnedValue.ShouldBeOfType<List<Folder>>(),
                () => returnedValue.Count.ShouldBe(directories.Length),
                () => returnedValue.ForEach(fld => directories.ShouldContain(fld.FolderName))); ;
        }

        [Test]
        public void GetFolders_IfFolderTypeDoesNotEqualToImg_ReturnsFoldersFromBusiness()
        {
            // Arrange
            var expectedFolders = new List<Folder>();
            expectedFolders.Add(new Folder() { FolderName = "Folder 01" });
            expectedFolders.Add(new Folder() { FolderName = "Folder 02" });
            ShimFolder.GetByTypeInt32StringUser = (customerId, folderType, user) => { return expectedFolders; };

            var shimEcnSession = new ShimECNSession();
            ShimECNSession.CurrentSession = () => { return shimEcnSession.Instance; };

            _testEntity.FolderType = string.Empty;

            // Act
            var returnedValue = _testEntityPrivate.Invoke(GetFoldersMethodName) as List<Folder>;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldNotBeNull(),
                () => returnedValue.ShouldBeOfType<List<Folder>>(),
                () => returnedValue.Count.ShouldBe(expectedFolders.Count),
                () => returnedValue.ForEach(fld => expectedFolders.ShouldContain(fld))); ;
        }

        [Test]
        public void LoadFolderTree_WhenCalled_LoadsFolderTree()
        {
            // Arrange
            var getFoldersMethodCalled = false;
            var loadChildrenMethodCalled = false;

            ShimFolderSystemBase.AllInstances.GetFolders = (_) =>
            {
                getFoldersMethodCalled = true;
                return null;
            };
            ShimFolderSystemBase.AllInstances.LoadChildrenTreeNode = (_, __) =>
            {
                loadChildrenMethodCalled = true;
            };

            // Act
            _testEntity.LoadFolderTree();

            // Assert
            _treeViewFolders.ShouldSatisfyAllConditions(
                () => getFoldersMethodCalled.ShouldBeTrue(),
                () => loadChildrenMethodCalled.ShouldBeTrue(),
                () => _treeViewFolders.Nodes.Count.ShouldBe(1));
        }

        [Test]
        public void LoadChildren_IfParentNodeDoesNotHaveChildren_DoesNotLoadChildren()
        {
            // Arrange
            var nodesCountBeforeAct = _treeViewFolders.Nodes.Count;

            // Act
            _testEntityPrivate.Invoke(LoadChildrenMethodName, _treeNode01);

            // Assert
            _treeViewFolders.Nodes.Count.ShouldBe(nodesCountBeforeAct);
        }

        [Test]
        public void LoadChildren_IfParentNodeHasChildren_LoadsChildren()
        {
            // Arrange
            int parentId;
            int.TryParse(_treeNode01.Value, out parentId);

            var childFolder = new Folder();
            childFolder.ParentID = parentId;
            _foldersList.Add(childFolder);

            var childNodesCountBefore = _treeNode01.ChildNodes.Count;
            var childNodesCountAfter = childNodesCountBefore + 1;

            // Act
            _testEntityPrivate.Invoke(LoadChildrenMethodName, _treeNode01);

            // Assert
            _treeNode01.ShouldSatisfyAllConditions(
                () => _treeNode01.ChildNodes.Count.ShouldNotBe(childNodesCountBefore),
                () => _treeNode01.ChildNodes.Count.ShouldBe(childNodesCountAfter));
        }

        [Test]
        public void TreeViewFoldersSelectedIndexChanged_IfFolderEventIsNotNull_TriggersFolderEvent()
        {
            // Arrange
            var folderEventTriggered = false;
            var folderEventMethod = new EventHandler((sender, eventArgs) => { folderEventTriggered = true; });
            _testEntity.FolderEvent += folderEventMethod;

            // Act
            _testEntityPrivate.Invoke(TreeViewFoldersSelectedIndexChangedMethodName, null, EventArgs.Empty);

            // Assert
            folderEventTriggered.ShouldBeTrue();
        }
    }
}
