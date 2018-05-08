using System;
using System.Linq;
using System.Linq.Expressions;
using WinForms = System.Windows.Forms;
using ActiveUp.WebControls.WinControls;
using ActiveUp.WebControls.Design;
using ActiveUp.WebControls.Tests.ActiveTree.Design;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveMenu
{
	[TestFixture]
	public class MenuPropertyBuilderFormTest
	{
		private const string ConnectionString = "data source=127.0.0.1;Integrated Security=SSPI;";
		private MenuPropertyBuilderForm _menuBuilderObject;
		private PrivateObject _menuBuilderPrivateObject;
		protected IDisposable _shimObject;

        private WinForms.PropertyGrid _propertyGridMenu;
        private MenuItemProperty _menuItemProperty;

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

            _menuBuilderObject = new MenuPropertyBuilderForm();
            _menuBuilderPrivateObject = new PrivateObject(_menuBuilderObject);

            _propertyGridMenu = (WinForms.PropertyGrid)_menuBuilderPrivateObject.GetField("_pgMenu");
            _menuItemProperty = new MenuItemProperty();

            _treeViewMenu = (WinForms.TreeView)_menuBuilderPrivateObject.GetField("_tvMenu");
            _btnMoveUp = (ButtonXP)_menuBuilderPrivateObject.GetField("_bMoveUp");
            _btnMoveDown = (ButtonXP)_menuBuilderPrivateObject.GetField("_bMoveDown");
            _btnChildSibling = (ButtonXP)_menuBuilderPrivateObject.GetField("_bChildSibling");
            _btnSiblingParent = (ButtonXP)_menuBuilderPrivateObject.GetField("_bSlibingParent");
            _btnAddChild = (ButtonXP)_menuBuilderPrivateObject.GetField("_bAddChild");
            _btnRemoveNode = (ButtonXP)_menuBuilderPrivateObject.GetField("_bRemoveNode");
        }

		[TearDown]
		public void DisposeContext()
		{
			_shimObject.Dispose();
            _treeViewMenu.Dispose();
        }

		[Test]
		public void InitializeComponent_LoadDefaultValue_ValidatePageControlLoadSuccessfully()
		{
			// Arrange
			CreateClassObject();

			// Act
			_menuBuilderPrivateObject.Invoke("InitializeComponent", null);

			// Assert
			AssertMethodResult(_menuBuilderObject);
		}

		[Test]
		public void InitializeComponent_LoadDefaultValue_ValidateBuilderNameIsAssignedSuccessfully()
		{
			// Arrange
			CreateClassObject();

			// Act
			_menuBuilderPrivateObject.Invoke("InitializeComponent", null);

			// Assert
			var propertyBuilderName = (string)_menuBuilderPrivateObject.GetFieldOrProperty("Name");
			var propertyBuilderText = (string)_menuBuilderPrivateObject.GetFieldOrProperty("Text");
			propertyBuilderName.ShouldSatisfyAllConditions(
				() => propertyBuilderName.ShouldNotBeNullOrWhiteSpace(),
				() => propertyBuilderText.ShouldNotBeNullOrWhiteSpace(),
				() => propertyBuilderName.ShouldBe(MenuHelper.MenuName),
				() => propertyBuilderText.ShouldBe(MenuHelper.MenuText));
		}

        [Test]
        public void TreeViewMenu_AfterSelect_SetsButtonsEnabledStatus_WhenParentOfSelectedNodeIsNullAndNodesCountIsOne()
        {
            // Arrange
            var node01 = new WinForms.TreeNode();
            _treeViewMenu.Nodes.Add(node01);
            _treeViewMenu.SelectedNode = node01;
            _treeViewMenu.SelectedNode.Tag = _menuItemProperty;

            // Act
            _menuBuilderPrivateObject.Invoke("_tvMenu_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeFalse();
            _btnMoveDown.Enabled.ShouldBeFalse();
            _btnChildSibling.Enabled.ShouldBeFalse();
            _btnSiblingParent.Enabled.ShouldBeFalse();
            _btnAddChild.Enabled.ShouldBeTrue();
            _btnRemoveNode.Enabled.ShouldBeTrue();
            _propertyGridMenu.SelectedObject.ShouldBeSameAs(_menuItemProperty);
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
            _treeViewMenu.SelectedNode.Tag = _menuItemProperty;

            // Act
            _menuBuilderPrivateObject.Invoke("_tvMenu_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeFalse();
            _btnMoveDown.Enabled.ShouldBeTrue();
            _btnChildSibling.Enabled.ShouldBeFalse();
            _btnSiblingParent.Enabled.ShouldBeFalse();
            _btnAddChild.Enabled.ShouldBeTrue();
            _btnRemoveNode.Enabled.ShouldBeTrue();
            _propertyGridMenu.SelectedObject.ShouldBeSameAs(_menuItemProperty);
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
            _treeViewMenu.SelectedNode.Tag = _menuItemProperty;

            // Act
            _menuBuilderPrivateObject.Invoke("_tvMenu_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeTrue();
            _btnMoveDown.Enabled.ShouldBeFalse();
            _btnChildSibling.Enabled.ShouldBeTrue();
            _btnSiblingParent.Enabled.ShouldBeFalse();
            _btnAddChild.Enabled.ShouldBeTrue();
            _btnRemoveNode.Enabled.ShouldBeTrue();
            _propertyGridMenu.SelectedObject.ShouldBeSameAs(_menuItemProperty);
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
            _treeViewMenu.SelectedNode.Tag = _menuItemProperty;

            // Act
            _menuBuilderPrivateObject.Invoke("_tvMenu_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeTrue();
            _btnMoveDown.Enabled.ShouldBeTrue();
            _btnChildSibling.Enabled.ShouldBeTrue();
            _btnSiblingParent.Enabled.ShouldBeFalse();
            _btnAddChild.Enabled.ShouldBeTrue();
            _btnRemoveNode.Enabled.ShouldBeTrue();
            _propertyGridMenu.SelectedObject.ShouldBeSameAs(_menuItemProperty);
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
            _treeViewMenu.SelectedNode.Tag = _menuItemProperty;

            // Act
            _menuBuilderPrivateObject.Invoke("_tvMenu_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeFalse();
            _btnMoveDown.Enabled.ShouldBeFalse();
            _btnChildSibling.Enabled.ShouldBeFalse();
            _btnSiblingParent.Enabled.ShouldBeTrue();
            _propertyGridMenu.SelectedObject.ShouldBeSameAs(_menuItemProperty);
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
            _treeViewMenu.SelectedNode.Tag = _menuItemProperty;

            // Act
            _menuBuilderPrivateObject.Invoke("_tvMenu_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeFalse();
            _btnMoveDown.Enabled.ShouldBeTrue();
            _btnChildSibling.Enabled.ShouldBeFalse();
            _btnSiblingParent.Enabled.ShouldBeTrue();
            _propertyGridMenu.SelectedObject.ShouldBeSameAs(_menuItemProperty);
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
            _treeViewMenu.SelectedNode.Tag = _menuItemProperty;

            // Act
            _menuBuilderPrivateObject.Invoke("_tvMenu_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeTrue();
            _btnMoveDown.Enabled.ShouldBeFalse();
            _btnChildSibling.Enabled.ShouldBeTrue();
            _btnSiblingParent.Enabled.ShouldBeTrue();
            _propertyGridMenu.SelectedObject.ShouldBeSameAs(_menuItemProperty);
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
            _treeViewMenu.SelectedNode.Tag = _menuItemProperty;

            // Act
            _menuBuilderPrivateObject.Invoke("_tvMenu_AfterSelect", null, new WinForms.TreeViewEventArgs(null));

            // Assert
            _btnMoveUp.Enabled.ShouldBeTrue();
            _btnMoveDown.Enabled.ShouldBeTrue();
            _btnChildSibling.Enabled.ShouldBeFalse();
            _btnSiblingParent.Enabled.ShouldBeTrue();
            _propertyGridMenu.SelectedObject.ShouldBeSameAs(_menuItemProperty);
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
            _menuBuilderPrivateObject.Invoke("_bMoveUp_Click", null, EventArgs.Empty);

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
            _menuBuilderPrivateObject.Invoke("_bMoveUp_Click", null, EventArgs.Empty);

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
            _menuBuilderPrivateObject.Invoke("_bMoveDown_Click", null, EventArgs.Empty);

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
            _menuBuilderPrivateObject.Invoke("_bMoveDown_Click", null, EventArgs.Empty);

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
            _menuBuilderPrivateObject.Invoke("_bSlibingParent_Click", null, EventArgs.Empty);

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
            _menuBuilderPrivateObject.Invoke("_bSlibingParent_Click", null, EventArgs.Empty);

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
            _menuBuilderPrivateObject.Invoke("_bChildSibling_Click", null, EventArgs.Empty);

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
            _menuBuilderPrivateObject.Invoke("_bChildSibling_Click", null, EventArgs.Empty);

            // Assert
            var nodesOfPreviousNode = new WinForms.TreeNode[previousNode.Nodes.Count];
            previousNode.Nodes.CopyTo(nodesOfPreviousNode, 0);

            _treeViewMenu.SelectedNode.ShouldBeOneOf(nodesOfPreviousNode);
            _treeViewMenu.SelectedNode.Index.ShouldBe(expectedSelectedNodeIndex);
            _treeViewMenu.SelectedNode.Text.ShouldBe(expectedSelectedNodeText);
        }

        private object GetTextbox(string tbName)
		{
			var txt = "";
			var textBox = (WinForms.TextBox)_menuBuilderPrivateObject.GetFieldOrProperty(tbName);
			if (textBox != null)
			{
				txt = textBox.Text;
			}
			return txt;
		}

		private object GetLabel(string tbName)
		{
			var txt = "";
			var label = (WinForms.Label)_menuBuilderPrivateObject.GetFieldOrProperty(tbName);
			if (label != null)
			{
				txt = label.Text;
			}
			return txt;
		}

		private object GetButton(string tbName)
		{
			var txt = "";
			var btn = (ButtonXP)_menuBuilderPrivateObject.GetFieldOrProperty(tbName);
			if (btn != null)
			{
				txt = btn.Text;
			}
			return txt;
		}

		private object GetRadioButton(string tbName)
		{
			var txt = "";
			var radioBtn = (WinForms.RadioButton)_menuBuilderPrivateObject.GetFieldOrProperty(tbName);
			if (radioBtn != null)
			{
				txt = radioBtn.Text;
			}
			return txt;
		}

		private object GetCheckbox(string tbName)
		{
			var txt = "";
			var ckBox = (WinForms.CheckBox)_menuBuilderPrivateObject.GetFieldOrProperty(tbName);
			if (ckBox != null)
			{
				txt = ckBox.Text;
			}
			return txt;
		}

		private T Get<T>(string propName)
		{
			var val = (T)_menuBuilderPrivateObject.GetFieldOrProperty(propName);
			return val;
		}

		private void CreateClassObject()
		{
			_menuBuilderObject = new MenuPropertyBuilderForm();
			_menuBuilderPrivateObject = new PrivateObject(_menuBuilderObject);
		}

		private void AssertMethodResult(MenuPropertyBuilderForm result)
		{
			result.ShouldSatisfyAllConditions(
				() => GetLabel(MenuHelper.Label1).ShouldBe(MenuHelper.Label1Text),
				() => GetLabel(MenuHelper.Label2).ShouldBe(MenuHelper.Label2Text),
				() => GetButton(MenuHelper.ButtonOk).ShouldBe(MenuHelper.ButtonOkText),
				() => GetButton(MenuHelper.ButtonCancel).ShouldBe(MenuHelper.ButtonCancelText)
			);
		}
	}
}
