using System;
using WinForms = System.Windows.Forms;
using ActiveUp.WebControls.WinControls;
using ActiveUp.WebControls.Design;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveTree.Design
{
	[TestFixture]
	public class TreeViewMenuPropertyBuilderFormTest
	{
		private const string ConnectionString = "data source=127.0.0.1;Integrated Security=SSPI;";
		private PropertyBuilderForm _propertyBuilderObject;
		private PrivateObject _propertyBuilderPrivateObject;
		protected IDisposable _shimObject;

        private WinForms.PropertyGrid _propertyGridTreeView;
        private NodeDesign _nodeDesign;

        private WinForms.TreeView _treeViewMenu;
        private ButtonXP _btnMoveUp;
        private ButtonXP _btnMoveDown;
        private ButtonXP _btnChildSibling;
        private ButtonXP _btnSiblingParent;
        private ButtonXP _btnAddChild;
        private ButtonXP _btnRemoveNode;

        [SetUp]
		public void Setup()
		{
			_shimObject = ShimsContext.Create();

            _propertyBuilderObject = new PropertyBuilderForm();
            _propertyBuilderPrivateObject = new PrivateObject(_propertyBuilderObject);

            _propertyGridTreeView = (WinForms.PropertyGrid)_propertyBuilderPrivateObject.GetField("_pgTreeView");
            _nodeDesign = new NodeDesign();

            _treeViewMenu = (WinForms.TreeView)_propertyBuilderPrivateObject.GetField("_tvTree");
            _btnMoveUp = (ButtonXP)_propertyBuilderPrivateObject.GetField("_bMoveUp");
            _btnMoveDown = (ButtonXP)_propertyBuilderPrivateObject.GetField("_bMoveDown");
            _btnChildSibling = (ButtonXP)_propertyBuilderPrivateObject.GetField("_bChildSlibing");
            _btnSiblingParent = (ButtonXP)_propertyBuilderPrivateObject.GetField("_bSlibingParent");
            _btnAddChild = (ButtonXP)_propertyBuilderPrivateObject.GetField("_bAddChild");
            _btnRemoveNode = (ButtonXP)_propertyBuilderPrivateObject.GetField("_bDelete");
        }

		[TearDown]
		public void DisposeContext()
		{
			_shimObject.Dispose();
		}

		[Test]
		public void InitializeComponent_LoadDefaultValue_ValidatePageControlLoadSuccessfully()
		{
			// Arrange
			CreateClassObject();

			// Act
			_propertyBuilderPrivateObject.Invoke("InitializeComponent", null);

			// Assert
			AssertMethodResult(_propertyBuilderObject);
		}

		[Test]
		public void InitializeComponent_LoadDefaultValue_ValidateBuilderNameIsAssignedSuccessfully()
		{
			// Arrange
			CreateClassObject();

			// Act
			_propertyBuilderPrivateObject.Invoke("InitializeComponent", null);

			// Assert
			var menBuilderName = (string)_propertyBuilderPrivateObject.GetFieldOrProperty("Name");
			var menuBuilderText = (string)_propertyBuilderPrivateObject.GetFieldOrProperty("Text");
			menBuilderName.ShouldSatisfyAllConditions(
				() => menBuilderName.ShouldNotBeNullOrWhiteSpace(),
				() => menuBuilderText.ShouldNotBeNullOrWhiteSpace(),
				() => menBuilderName.ShouldBe(PropertyHelper.BuilderName),
				() => menuBuilderText.ShouldBe(PropertyHelper.BuilderText));
		}

        [Test]
        public void TreeViewMenu_AfterSelect_SetsButtonsEnabledStatus_WhenParentOfSelectedNodeIsNullAndNodesCountIsOne()
        {
            // Arrange
            var node01 = new WinForms.TreeNode();
            _treeViewMenu.Nodes.Add(node01);
            _treeViewMenu.SelectedNode = node01;
            _treeViewMenu.SelectedNode.Tag = _nodeDesign;

            // Act
            _propertyBuilderPrivateObject.Invoke("_tvTree_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeFalse();
            _btnMoveDown.Enabled.ShouldBeFalse();
            _btnChildSibling.Enabled.ShouldBeFalse();
            _btnSiblingParent.Enabled.ShouldBeFalse();
            _btnAddChild.Enabled.ShouldBeTrue();
            _btnRemoveNode.Enabled.ShouldBeTrue();
            _propertyGridTreeView.SelectedObject.ShouldBeSameAs(_nodeDesign);
        }

        [Test]
        public void TreeViewMenu_AfterSelect_SetsButtonsEnabledStatus_WhenParentOfSelectedNodeIsNullAndFirstNodeSelected()
        {
            // Arrange
            var node01 = new WinForms.TreeNode();
            var node02 = new WinForms.TreeNode();

            _treeViewMenu.Nodes.Add(node01);
            _treeViewMenu.Nodes.Add(node02);

            _treeViewMenu.SelectedNode = node01;
            _treeViewMenu.SelectedNode.Tag = _nodeDesign;

            // Act
            _propertyBuilderPrivateObject.Invoke("_tvTree_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeFalse();
            _btnMoveDown.Enabled.ShouldBeTrue();
            _btnChildSibling.Enabled.ShouldBeFalse();
            _btnSiblingParent.Enabled.ShouldBeFalse();
            _btnAddChild.Enabled.ShouldBeTrue();
            _btnRemoveNode.Enabled.ShouldBeTrue();
            _propertyGridTreeView.SelectedObject.ShouldBeSameAs(_nodeDesign);
        }

        [Test]
        public void TreeViewMenu_AfterSelect_SetsButtonsEnabledStatus_WhenParentOfSelectedNodeIsNullAndLastNodeSelected()
        {
            // Arrange
            var node01 = new WinForms.TreeNode();
            var node02 = new WinForms.TreeNode();

            _treeViewMenu.Nodes.Add(node01);
            _treeViewMenu.Nodes.Add(node02);

            _treeViewMenu.SelectedNode = node02;
            _treeViewMenu.SelectedNode.Tag = _nodeDesign;

            // Act
            _propertyBuilderPrivateObject.Invoke("_tvTree_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeTrue();
            _btnMoveDown.Enabled.ShouldBeFalse();
            _btnChildSibling.Enabled.ShouldBeTrue();
            _btnSiblingParent.Enabled.ShouldBeFalse();
            _btnAddChild.Enabled.ShouldBeTrue();
            _btnRemoveNode.Enabled.ShouldBeTrue();
            _propertyGridTreeView.SelectedObject.ShouldBeSameAs(_nodeDesign);
        }

        [Test]
        public void TreeViewMenu_AfterSelect_SetsButtonsEnabledStatus_WhenParentOfSelectedNodeIsNullAndMiddleNodeSelected()
        {
            // Arrange
            var node01 = new WinForms.TreeNode();
            var node02 = new WinForms.TreeNode();
            var node03 = new WinForms.TreeNode();

            _treeViewMenu.Nodes.Add(node01);
            _treeViewMenu.Nodes.Add(node02);
            _treeViewMenu.Nodes.Add(node03);

            _treeViewMenu.SelectedNode = node02;
            _treeViewMenu.SelectedNode.Tag = _nodeDesign;

            // Act
            _propertyBuilderPrivateObject.Invoke("_tvTree_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeTrue();
            _btnMoveDown.Enabled.ShouldBeTrue();
            _btnChildSibling.Enabled.ShouldBeTrue();
            _btnSiblingParent.Enabled.ShouldBeFalse();
            _btnAddChild.Enabled.ShouldBeTrue();
            _btnRemoveNode.Enabled.ShouldBeTrue();
            _propertyGridTreeView.SelectedObject.ShouldBeSameAs(_nodeDesign);
        }

        [Test]
        public void TreeViewMenu_AfterSelect_SetsButtonsEnabledStatus_WhenParentOfSelectedNodeIsNotNullAndNodesCountIsOne()
        {
            // Arrange
            var parentNode = new WinForms.TreeNode();
            var node01 = new WinForms.TreeNode();
            parentNode.Nodes.Add(node01);

            _treeViewMenu.Nodes.Add(parentNode);
            _treeViewMenu.SelectedNode = node01;
            _treeViewMenu.SelectedNode.Tag = _nodeDesign;

            // Act
            _propertyBuilderPrivateObject.Invoke("_tvTree_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeFalse();
            _btnMoveDown.Enabled.ShouldBeFalse();
            _btnChildSibling.Enabled.ShouldBeFalse();
            _btnSiblingParent.Enabled.ShouldBeTrue();
            _propertyGridTreeView.SelectedObject.ShouldBeSameAs(_nodeDesign);
        }

        [Test]
        public void TreeViewMenu_AfterSelect_SetsButtonsEnabledStatus_WhenParentOfSelectedNodeIsNotNullAndFirstNodeSelected()
        {
            // Arrange
            var parentNode = new WinForms.TreeNode();
            var node01 = new WinForms.TreeNode();
            var node02 = new WinForms.TreeNode();

            parentNode.Nodes.Add(node01);
            parentNode.Nodes.Add(node02);

            _treeViewMenu.Nodes.Add(parentNode);
            _treeViewMenu.SelectedNode = node01;
            _treeViewMenu.SelectedNode.Tag = _nodeDesign;

            // Act
            _propertyBuilderPrivateObject.Invoke("_tvTree_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeFalse();
            _btnMoveDown.Enabled.ShouldBeTrue();
            _btnChildSibling.Enabled.ShouldBeFalse();
            _btnSiblingParent.Enabled.ShouldBeTrue();
            _propertyGridTreeView.SelectedObject.ShouldBeSameAs(_nodeDesign);
        }

        [Test]
        public void TreeViewMenu_AfterSelect_SetsButtonsEnabledStatus_WhenParentOfSelectedNodeIsNotNullAndLastNodeSelected()
        {
            // Arrange
            var parentNode = new WinForms.TreeNode();
            var node01 = new WinForms.TreeNode();
            var node02 = new WinForms.TreeNode();

            parentNode.Nodes.Add(node01);
            parentNode.Nodes.Add(node02);

            _treeViewMenu.Nodes.Add(parentNode);
            _treeViewMenu.SelectedNode = node02;
            _treeViewMenu.SelectedNode.Tag = _nodeDesign;

            // Act
            _propertyBuilderPrivateObject.Invoke("_tvTree_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeTrue();
            _btnMoveDown.Enabled.ShouldBeFalse();
            _btnChildSibling.Enabled.ShouldBeTrue();
            _btnSiblingParent.Enabled.ShouldBeTrue();
            _propertyGridTreeView.SelectedObject.ShouldBeSameAs(_nodeDesign);
        }

        [Test]
        public void TreeViewMenu_AfterSelect_SetsButtonsEnabledStatus_WhenParentOfSelectedNodeIsNotNullAndMiddleNodeSelected()
        {
            // Arrange
            var parentNode = new WinForms.TreeNode();
            var node01 = new WinForms.TreeNode();
            var node02 = new WinForms.TreeNode();
            var node03 = new WinForms.TreeNode();

            parentNode.Nodes.Add(node01);
            parentNode.Nodes.Add(node02);
            parentNode.Nodes.Add(node03);

            _treeViewMenu.Nodes.Add(parentNode);
            _treeViewMenu.SelectedNode = node02;
            _treeViewMenu.SelectedNode.Tag = _nodeDesign;

            // Act
            _propertyBuilderPrivateObject.Invoke("_tvTree_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeTrue();
            _btnMoveDown.Enabled.ShouldBeTrue();
            _btnChildSibling.Enabled.ShouldBeFalse();
            _btnSiblingParent.Enabled.ShouldBeTrue();
            _propertyGridTreeView.SelectedObject.ShouldBeSameAs(_nodeDesign);
        }

        [Test]
        public void MoveUpButton_Click_WhenParentOfSelectedNodeIsNull_MovesUpTheSelectedNode()
        {
            // Arrange
            var node01 = new WinForms.TreeNode("node01");
            var node02 = new WinForms.TreeNode("node02");
            var node03 = new WinForms.TreeNode("node03");

            _treeViewMenu.Nodes.Add(node01);
            _treeViewMenu.Nodes.Add(node02);
            _treeViewMenu.Nodes.Add(node03);

            _treeViewMenu.SelectedNode = node02;

            var expectedSelectedNodeIndex = _treeViewMenu.SelectedNode.Index - 1;
            var expectedSelectedNodeText = _treeViewMenu.SelectedNode.Text;

            // Act
            _propertyBuilderPrivateObject.Invoke("_bMoveUp_Click", null, EventArgs.Empty);

            // Assert
            _treeViewMenu.SelectedNode.Index.ShouldBe(expectedSelectedNodeIndex);
            _treeViewMenu.SelectedNode.Text.ShouldBe(expectedSelectedNodeText);
        }

        [Test]
        public void MoveUpButton_Click_WhenParentOfSelectedNodeIsNotNull_MovesUpTheSelectedNodeInsideItsParent()
        {
            // Arrange
            var parentNode = new WinForms.TreeNode("parentNode");
            var node01 = new WinForms.TreeNode("node01");
            var node02 = new WinForms.TreeNode("node02");
            var node03 = new WinForms.TreeNode("node03");

            parentNode.Nodes.Add(node01);
            parentNode.Nodes.Add(node02);
            parentNode.Nodes.Add(node03);

            _treeViewMenu.Nodes.Add(parentNode);
            _treeViewMenu.SelectedNode = node02;

            var expectedSelectedNodeIndex = _treeViewMenu.SelectedNode.Index - 1;
            var expectedSelectedNodeText = _treeViewMenu.SelectedNode.Text;

            // Act
            _propertyBuilderPrivateObject.Invoke("_bMoveUp_Click", null, EventArgs.Empty);

            // Assert
            _treeViewMenu.SelectedNode.Index.ShouldBe(expectedSelectedNodeIndex);
            _treeViewMenu.SelectedNode.Text.ShouldBe(expectedSelectedNodeText);
        }

        [Test]
        public void MoveDownButton_Click_WhenParentOfSelectedNodeIsNull_MovesDownTheSelectedNode()
        {
            // Arrange
            var node01 = new WinForms.TreeNode("node01");
            var node02 = new WinForms.TreeNode("node02");
            var node03 = new WinForms.TreeNode("node03");

            _treeViewMenu.Nodes.Add(node01);
            _treeViewMenu.Nodes.Add(node02);
            _treeViewMenu.Nodes.Add(node03);

            _treeViewMenu.SelectedNode = node02;

            var expectedSelectedNodeIndex = _treeViewMenu.SelectedNode.Index + 1;
            var expectedSelectedNodeText = _treeViewMenu.SelectedNode.Text;

            // Act
            _propertyBuilderPrivateObject.Invoke("_bMoveDown_Click", null, EventArgs.Empty);

            // Assert
            _treeViewMenu.SelectedNode.Index.ShouldBe(expectedSelectedNodeIndex);
            _treeViewMenu.SelectedNode.Text.ShouldBe(expectedSelectedNodeText);
        }

        [Test]
        public void MoveDownButton_Click_WhenParentOfSelectedNodeIsNotNull_MovesDownTheSelectedNodeInsideItsParent()
        {
            // Arrange
            var parentNode = new WinForms.TreeNode("parentNode");
            var node01 = new WinForms.TreeNode("node01");
            var node02 = new WinForms.TreeNode("node02");
            var node03 = new WinForms.TreeNode("node03");

            parentNode.Nodes.Add(node01);
            parentNode.Nodes.Add(node02);
            parentNode.Nodes.Add(node03);

            _treeViewMenu.Nodes.Add(parentNode);
            _treeViewMenu.SelectedNode = node02;

            var expectedSelectedNodeIndex = _treeViewMenu.SelectedNode.Index + 1;
            var expectedSelectedNodeText = _treeViewMenu.SelectedNode.Text;

            // Act
            _propertyBuilderPrivateObject.Invoke("_bMoveDown_Click", null, EventArgs.Empty);

            // Assert
            _treeViewMenu.SelectedNode.Index.ShouldBe(expectedSelectedNodeIndex);
            _treeViewMenu.SelectedNode.Text.ShouldBe(expectedSelectedNodeText);
        }

        [Test]
        public void SiblingParentButton_Click_WhenGrandParentOfSelectedNodeIsNull_MovesTheSelectedNodeToTheRoot()
        {
            // Arrange
            var parentNode = new WinForms.TreeNode("parentNode");
            var node01 = new WinForms.TreeNode("node01");
            var node02 = new WinForms.TreeNode("node02");
            var node03 = new WinForms.TreeNode("node03");

            parentNode.Nodes.Add(node01);
            parentNode.Nodes.Add(node02);
            parentNode.Nodes.Add(node03);

            _treeViewMenu.Nodes.Add(parentNode);
            _treeViewMenu.SelectedNode = node02;

            var parentIndexOfSelectedNode = _treeViewMenu.SelectedNode.Parent.Index;

            var expectedSelectedNodeIndex = parentIndexOfSelectedNode + 1;
            var expectedSelectedNodeText = _treeViewMenu.SelectedNode.Text;

            // Act
            _propertyBuilderPrivateObject.Invoke("_bSlibingParent_Click", null, EventArgs.Empty);

            // Assert
            var nodesOfTreeViewMenu = new WinForms.TreeNode[_treeViewMenu.Nodes.Count];
            _treeViewMenu.Nodes.CopyTo(nodesOfTreeViewMenu, 0);

            _treeViewMenu.SelectedNode.ShouldBeOneOf(nodesOfTreeViewMenu);
            _treeViewMenu.SelectedNode.Index.ShouldBe(expectedSelectedNodeIndex);
            _treeViewMenu.SelectedNode.Text.ShouldBe(expectedSelectedNodeText);
        }

        [Test]
        public void SiblingParentButton_Click_WhenGrandParentOfSelectedNodeIsNotNull_MovesTheSelectedNodeInsideTheGrandParent()
        {
            // Arrange
            var grandParentNode = new WinForms.TreeNode("grandParentNode");
            var parentNode = new WinForms.TreeNode("parentNode");
            var node01 = new WinForms.TreeNode("node01");
            var node02 = new WinForms.TreeNode("node02");
            var node03 = new WinForms.TreeNode("node03");

            grandParentNode.Nodes.Add(parentNode);
            parentNode.Nodes.Add(node01);
            parentNode.Nodes.Add(node02);
            parentNode.Nodes.Add(node03);

            _treeViewMenu.Nodes.Add(grandParentNode);
            _treeViewMenu.SelectedNode = node02;

            var expectedSelectedNodeIndex = grandParentNode.Nodes.Count;
            var expectedSelectedNodeText = _treeViewMenu.SelectedNode.Text;

            // Act
            _propertyBuilderPrivateObject.Invoke("_bSlibingParent_Click", null, EventArgs.Empty);

            // Assert
            var nodesOfGrandParentNode = new WinForms.TreeNode[grandParentNode.Nodes.Count];
            grandParentNode.Nodes.CopyTo(nodesOfGrandParentNode, 0);

            _treeViewMenu.SelectedNode.ShouldBeOneOf(nodesOfGrandParentNode);
            _treeViewMenu.SelectedNode.Index.ShouldBe(expectedSelectedNodeIndex);
            _treeViewMenu.SelectedNode.Text.ShouldBe(expectedSelectedNodeText);
        }

        [Test]
        public void ChildSiblingButton_Click_WhenParentOfSelectedNodeIsNull_MovesTheSelectedNodeInsideItsPreviousSibling()
        {
            // Arrange
            var node01 = new WinForms.TreeNode("node01");
            var node02 = new WinForms.TreeNode("node02");
            var node03 = new WinForms.TreeNode("node03");

            _treeViewMenu.Nodes.Add(node01);
            _treeViewMenu.Nodes.Add(node02);
            _treeViewMenu.Nodes.Add(node03);

            _treeViewMenu.SelectedNode = node02;

            var previousNode = _treeViewMenu.Nodes[_treeViewMenu.SelectedNode.Index - 1];

            var expectedSelectedNodeIndex = previousNode.Nodes.Count;
            var expectedSelectedNodeText = _treeViewMenu.SelectedNode.Text;

            // Act
            _propertyBuilderPrivateObject.Invoke("_bChildSlibing_Click", null, EventArgs.Empty);

            // Assert
            var nodesOfPreviousNode = new WinForms.TreeNode[previousNode.Nodes.Count];
            previousNode.Nodes.CopyTo(nodesOfPreviousNode, 0);

            _treeViewMenu.SelectedNode.ShouldBeOneOf(nodesOfPreviousNode);
            _treeViewMenu.SelectedNode.Index.ShouldBe(expectedSelectedNodeIndex);
            _treeViewMenu.SelectedNode.Text.ShouldBe(expectedSelectedNodeText);
        }

        [Test]
        public void ChildSiblingButton_Click_WhenParentOfSelectedNodeIsNotNull_MovesTheSelectedNodeInsideItsPreviousSibling()
        {
            // Arrange
            var parentNode = new WinForms.TreeNode("parentNode");
            var node01 = new WinForms.TreeNode("node01");
            var node02 = new WinForms.TreeNode("node02");
            var node03 = new WinForms.TreeNode("node03");

            parentNode.Nodes.Add(node01);
            parentNode.Nodes.Add(node02);
            parentNode.Nodes.Add(node03);

            _treeViewMenu.Nodes.Add(parentNode);
            _treeViewMenu.SelectedNode = node02;

            var previousNode = _treeViewMenu.SelectedNode.Parent.Nodes[_treeViewMenu.SelectedNode.Index - 1];

            var expectedSelectedNodeIndex = previousNode.Nodes.Count;
            var expectedSelectedNodeText = _treeViewMenu.SelectedNode.Text;

            // Act
            _propertyBuilderPrivateObject.Invoke("_bChildSlibing_Click", null, EventArgs.Empty);

            // Assert
            var nodesOfPreviousNode = new WinForms.TreeNode[previousNode.Nodes.Count];
            previousNode.Nodes.CopyTo(nodesOfPreviousNode, 0);

            _treeViewMenu.SelectedNode.ShouldBeOneOf(nodesOfPreviousNode);
            _treeViewMenu.SelectedNode.Index.ShouldBe(expectedSelectedNodeIndex);
            _treeViewMenu.SelectedNode.Text.ShouldBe(expectedSelectedNodeText);
        }

        private object GetTextbox(string tbName)
		{
			var txt = string.Empty;
			var textBox = _propertyBuilderPrivateObject.GetFieldOrProperty(tbName) as WinForms.TextBox;
			if (textBox != null)
			{
				txt = textBox.Text;
			}
			return txt;
		}

		private object GetLabel(string tbName)
		{
			var txt = string.Empty;
			var label = _propertyBuilderPrivateObject.GetFieldOrProperty(tbName) as WinForms.Label;
			if (label != null)
			{
				txt = label.Text;
			}
			return txt;
		}

		private object GetButton(string tbName)
		{
			var txt = string.Empty;
			var btn = _propertyBuilderPrivateObject.GetFieldOrProperty(tbName) as ButtonXP;
			if (btn != null)
			{
				txt = btn.Text;
			}
			return txt;
		}

		private object GetRadioButton(string tbName)
		{
			var txt = string.Empty;
			var radioBtn = _propertyBuilderPrivateObject.GetFieldOrProperty(tbName) as WinForms.RadioButton;
			if (radioBtn != null)
			{
				txt = radioBtn.Text;
			}
			return txt;
		}

		private object GetCheckbox(string tbName)
		{
			var txt = string.Empty;
			var ckBox = _propertyBuilderPrivateObject.GetFieldOrProperty(tbName) as WinForms.CheckBox;
			if (ckBox != null)
			{
				txt = ckBox.Text;
			}
			return txt;
		}

		private T Get<T>(string propName)
		{
			var val = (T)_propertyBuilderPrivateObject.GetFieldOrProperty(propName);
			return val;
		}

		private void CreateClassObject()
		{
			_propertyBuilderObject = new PropertyBuilderForm();
			_propertyBuilderPrivateObject = new PrivateObject(_propertyBuilderObject);
		}

		private void AssertMethodResult(PropertyBuilderForm result)
		{
			result.ShouldSatisfyAllConditions(
				() => GetLabel(PropertyHelper.Label1).ShouldBe(PropertyHelper.Label1Text),
				() => GetLabel(PropertyHelper.Label2).ShouldBe(PropertyHelper.Label2Text),
				() => GetButton(PropertyHelper.ButtonOk).ShouldBe(PropertyHelper.ButtonOkText),
				() => GetButton(PropertyHelper.ButtonCancel1).ShouldBe(PropertyHelper.ButtonCancel1Text));
		}
	}
}
