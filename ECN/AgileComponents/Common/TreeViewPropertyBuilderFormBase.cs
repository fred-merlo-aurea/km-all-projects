using ActiveUp.WebControls.WinControls;
using System;
using System.Collections.Generic;
using System.Text;
using WinForms = System.Windows.Forms;

namespace ActiveUp.WebControls
{
    public class TreeViewPropertyBuilderFormBase : WinForms.Form
    {
        private WinForms.TreeView _treeViewMenu;
        private ButtonXP _moveUpButton;
        private ButtonXP _moveDownButton;
        private ButtonXP _childSiblingButton;
        private ButtonXP _siblingParentButton;
        private ButtonXP _addChildButton;
        private ButtonXP _removeNodeButton;

        public virtual WinForms.TreeView TreeViewMenu { get { return _treeViewMenu; } }
        public virtual ButtonXP MoveUpButton { get { return _moveUpButton; } }
        public virtual ButtonXP MoveDownButton { get { return _moveDownButton; } }
        public virtual ButtonXP ChildSiblingButton { get { return _childSiblingButton; } }
        public virtual ButtonXP SiblingParentButton { get { return _siblingParentButton; } }
        public virtual ButtonXP AddChildButton { get { return _addChildButton; } }
        public virtual ButtonXP RemoveNodeButton { get { return _removeNodeButton; } }

        public TreeViewPropertyBuilderFormBase()
        {
            _treeViewMenu = new WinForms.TreeView();
            _moveUpButton = new ButtonXP();
            _moveDownButton = new ButtonXP();
            _childSiblingButton = new ButtonXP();
            _siblingParentButton = new ButtonXP();
            _addChildButton = new ButtonXP();
            _removeNodeButton = new ButtonXP();
        }

        public virtual void PerformTreeViewMenuAfterSelectEventOperations()
        {
            if (TreeViewMenu.SelectedNode.Parent == null)
            {
                if (TreeViewMenu.Nodes.Count == 1)
                {
                    MoveUpButton.Enabled = false;
                    MoveDownButton.Enabled = false;
                    ChildSiblingButton.Enabled = false;
                    SiblingParentButton.Enabled = false;
                    AddChildButton.Enabled = true;
                    RemoveNodeButton.Enabled = true;
                }
                else if (TreeViewMenu.SelectedNode.Index == 0)
                {
                    MoveUpButton.Enabled = false;
                    MoveDownButton.Enabled = true;
                    ChildSiblingButton.Enabled = false;
                    SiblingParentButton.Enabled = false;
                    AddChildButton.Enabled = true;
                    RemoveNodeButton.Enabled = true;
                }
                else if (TreeViewMenu.SelectedNode.Index == TreeViewMenu.Nodes.Count - 1)
                {
                    MoveUpButton.Enabled = true;
                    MoveDownButton.Enabled = false;
                    ChildSiblingButton.Enabled = true;
                    SiblingParentButton.Enabled = false;
                    AddChildButton.Enabled = true;
                    RemoveNodeButton.Enabled = true;
                }
                else
                {
                    MoveUpButton.Enabled = true;
                    MoveDownButton.Enabled = true;
                    ChildSiblingButton.Enabled = true;
                    SiblingParentButton.Enabled = false;
                    AddChildButton.Enabled = true;
                    RemoveNodeButton.Enabled = true;
                }
            }
            else
            {
                if (TreeViewMenu.SelectedNode.Parent.Nodes.Count == 1)
                {
                    MoveUpButton.Enabled = false;
                    MoveDownButton.Enabled = false;
                    ChildSiblingButton.Enabled = false;
                    SiblingParentButton.Enabled = true;
                }
                else if (TreeViewMenu.SelectedNode.Index == 0)
                {
                    MoveUpButton.Enabled = false;
                    MoveDownButton.Enabled = true;
                    ChildSiblingButton.Enabled = false;
                    SiblingParentButton.Enabled = true;
                }
                else if (TreeViewMenu.SelectedNode.Index == TreeViewMenu.SelectedNode.Parent.Nodes.Count - 1)
                {
                    MoveUpButton.Enabled = true;
                    MoveDownButton.Enabled = false;
                    ChildSiblingButton.Enabled = true;
                    SiblingParentButton.Enabled = true;
                }
                else
                {
                    MoveUpButton.Enabled = true;
                    MoveDownButton.Enabled = true;
                    ChildSiblingButton.Enabled = false;
                    SiblingParentButton.Enabled = true;
                }
            }
        }

        public virtual void PerformMoveUpButtonClickEventOperations()
        {
            var selectedIndex = TreeViewMenu.SelectedNode.Index;
            var selectedNode = (WinForms.TreeNode)TreeViewMenu.SelectedNode.Clone();

            if (TreeViewMenu.SelectedNode.Parent == null)
            {
                TreeViewMenu.Nodes.Insert(selectedIndex - 1, selectedNode);
                TreeViewMenu.Nodes.RemoveAt(selectedIndex + 1);
            }
            else
            {
                TreeViewMenu.SelectedNode.Parent.Nodes.Insert(selectedIndex - 1, selectedNode);
                TreeViewMenu.SelectedNode.Parent.Nodes.RemoveAt(selectedIndex + 1);
            }

            TreeViewMenu.SelectedNode = selectedNode;
            TreeViewMenu.Focus();
        }

        public virtual void PerformMoveDownButtonClickEventOperations()
        {
            var selectedIndex = TreeViewMenu.SelectedNode.Index;
            var selectedNode = (WinForms.TreeNode)TreeViewMenu.SelectedNode.Clone();

            if (TreeViewMenu.SelectedNode.Parent == null)
            {
                TreeViewMenu.Nodes.Insert(selectedIndex + 2, selectedNode);
                TreeViewMenu.Nodes.RemoveAt(selectedIndex);
            }
            else
            {
                TreeViewMenu.SelectedNode.Parent.Nodes.Insert(selectedIndex + 2, selectedNode);
                TreeViewMenu.SelectedNode.Parent.Nodes.RemoveAt(selectedIndex);
            }

            TreeViewMenu.SelectedNode = selectedNode;
            TreeViewMenu.Focus();
        }

        public virtual void PerformSiblingParentButtonClickEventOperations()
        {
            var selecteIndex = TreeViewMenu.SelectedNode.Index;
            var selectedNode = (WinForms.TreeNode)TreeViewMenu.SelectedNode.Clone();

            if (TreeViewMenu.SelectedNode.Parent.Parent == null)
            {
                var parentIndex = TreeViewMenu.SelectedNode.Parent.Index;
                TreeViewMenu.Nodes.Insert(parentIndex + 1, selectedNode);
                TreeViewMenu.SelectedNode.Remove();
            }
            else
            {
                TreeViewMenu.SelectedNode.Parent.Parent.Nodes.Add(selectedNode);
                TreeViewMenu.SelectedNode.Remove();
            }

            TreeViewMenu.SelectedNode = selectedNode;
            TreeViewMenu.Focus();
        }

        public virtual void PerformChildSiblingButtonClickEventOperations()
        {
            var selectedIndex = TreeViewMenu.SelectedNode.Index;
            var selectedNode = (WinForms.TreeNode)TreeViewMenu.SelectedNode.Clone();

            if (TreeViewMenu.SelectedNode.Parent == null)
            {
                WinForms.TreeNode previousNode = TreeViewMenu.Nodes[selectedIndex - 1];
                previousNode.Nodes.Add(selectedNode);
                previousNode.Expand();
                TreeViewMenu.Nodes.RemoveAt(selectedIndex);
            }
            else
            {
                WinForms.TreeNode previousNode = TreeViewMenu.SelectedNode.Parent.Nodes[selectedIndex - 1];
                previousNode.Nodes.Add(selectedNode);
                previousNode.Expand();
                TreeViewMenu.SelectedNode.Parent.Nodes.RemoveAt(selectedIndex);
            }

            TreeViewMenu.SelectedNode = selectedNode;
            TreeViewMenu.Focus();
        }
    }
}
