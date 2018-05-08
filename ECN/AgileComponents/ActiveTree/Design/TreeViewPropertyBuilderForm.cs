using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using WinForms = System.Windows.Forms;
using ActiveUp.WebControls.WinControls;

namespace ActiveUp.WebControls.Design
{
	internal enum TypeSearch
	{
		searchID,
		searchText
	}

	/// <summary>
	/// Represents a <see cref="TreeViewPropertyBuilderForm"/> object.
	/// </summary>
	public class PropertyBuilderForm : TreeViewPropertyBuilderFormBase
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TreeView _tvTree;
		private ActiveUp.WebControls.WinControls.ButtonXP _bAddRoot;
		private ActiveUp.WebControls.WinControls.ButtonXP _bAddChild;
		private ActiveUp.WebControls.WinControls.ButtonXP _bDelete;
		private ActiveUp.WebControls.WinControls.ButtonXP _bMoveUp;
		private ActiveUp.WebControls.WinControls.ButtonXP _bSlibingParent;
		private ActiveUp.WebControls.WinControls.ButtonXP _bChildSlibing;
		private System.Windows.Forms.PropertyGrid _pgTreeView;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private ActiveUp.WebControls.TreeView _treeView = null;
		private ActiveUp.WebControls.WinControls.ButtonXP _bOK;
		private ActiveUp.WebControls.WinControls.ButtonXP _bCancel;
		private ActiveUp.WebControls.WinControls.ButtonXP _bMoveDown;
		private ActiveUp.WebControls.WinControls.ButtonXP _bCancel1;
		private static int _nodeNumber = 0;
		private bool _isInitialized = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="TreeViewPropertyBuilderForm"/> class.
		/// </summary>
		public PropertyBuilderForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TreeViewPropertyBuilderForm"/> class.
		/// </summary>
		/// <param name="treeView">The tree view.</param>
		public PropertyBuilderForm(ActiveUp.WebControls.TreeView treeView)
		{
			InitializeComponent();

			_treeView = treeView;

			InitUI();

			_tvTree.Focus();
			this.ActiveControl = _tvTree;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyBuilderForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._tvTree = new System.Windows.Forms.TreeView();
            this._bAddRoot = new ActiveUp.WebControls.WinControls.ButtonXP();
            this._bAddChild = new ActiveUp.WebControls.WinControls.ButtonXP();
            this._bDelete = new ActiveUp.WebControls.WinControls.ButtonXP();
            this._bMoveUp = new ActiveUp.WebControls.WinControls.ButtonXP();
            this._bMoveDown = new ActiveUp.WebControls.WinControls.ButtonXP();
            this._bSlibingParent = new ActiveUp.WebControls.WinControls.ButtonXP();
            this._bChildSlibing = new ActiveUp.WebControls.WinControls.ButtonXP();
            this._pgTreeView = new System.Windows.Forms.PropertyGrid();
            this._bOK = new ActiveUp.WebControls.WinControls.ButtonXP();
            this._bCancel = new ActiveUp.WebControls.WinControls.ButtonXP();
            this._bCancel1 = new ActiveUp.WebControls.WinControls.ButtonXP();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Items :";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(257, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Properties :";
            // 
            // _tvTree
            // 
            this._tvTree.Location = new System.Drawing.Point(7, 53);
            this._tvTree.Name = "_tvTree";
            this._tvTree.Size = new System.Drawing.Size(243, 216);
            this._tvTree.TabIndex = 2;
            this._tvTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this._tvTree_AfterCheck);
            this._tvTree.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this._tvTree_AfterCollapse);
            this._tvTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._tvTree_AfterSelect);
            this._tvTree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this._tvTree_AfterExpand);
            // 
            // _bAddRoot
            // 
            this._bAddRoot.DefaultScheme = false;
            this._bAddRoot.Image = ((System.Drawing.Image)(resources.GetObject("_bAddRoot.Image")));
            this._bAddRoot.Location = new System.Drawing.Point(6, 24);
            this._bAddRoot.Name = "_bAddRoot";
            this._bAddRoot.Scheme = ActiveUp.WebControls.WinControls.ButtonXP.Schemes.Silver;
            this._bAddRoot.Size = new System.Drawing.Size(25, 25);
            this._bAddRoot.SizeImgButton = new System.Drawing.Size(0, 0);
            this._bAddRoot.TabIndex = 3;
            this._bAddRoot.Click += new System.EventHandler(this._bAddRoot_Click);
            // 
            // _bAddChild
            // 
            this._bAddChild.DefaultScheme = false;
            this._bAddChild.Image = ((System.Drawing.Image)(resources.GetObject("_bAddChild.Image")));
            this._bAddChild.Location = new System.Drawing.Point(32, 24);
            this._bAddChild.Name = "_bAddChild";
            this._bAddChild.Scheme = ActiveUp.WebControls.WinControls.ButtonXP.Schemes.Silver;
            this._bAddChild.Size = new System.Drawing.Size(25, 25);
            this._bAddChild.SizeImgButton = new System.Drawing.Size(0, 0);
            this._bAddChild.TabIndex = 4;
            this._bAddChild.Click += new System.EventHandler(this._bAddChild_Click);
            // 
            // _bDelete
            // 
            this._bDelete.DefaultScheme = false;
            this._bDelete.Image = ((System.Drawing.Image)(resources.GetObject("_bDelete.Image")));
            this._bDelete.Location = new System.Drawing.Point(58, 24);
            this._bDelete.Name = "_bDelete";
            this._bDelete.Scheme = ActiveUp.WebControls.WinControls.ButtonXP.Schemes.Silver;
            this._bDelete.Size = new System.Drawing.Size(25, 25);
            this._bDelete.SizeImgButton = new System.Drawing.Size(0, 0);
            this._bDelete.TabIndex = 5;
            this._bDelete.Click += new System.EventHandler(this._bDelete_Click);
            // 
            // _bMoveUp
            // 
            this._bMoveUp.DefaultScheme = false;
            this._bMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("_bMoveUp.Image")));
            this._bMoveUp.Location = new System.Drawing.Point(84, 24);
            this._bMoveUp.Name = "_bMoveUp";
            this._bMoveUp.Scheme = ActiveUp.WebControls.WinControls.ButtonXP.Schemes.Silver;
            this._bMoveUp.Size = new System.Drawing.Size(25, 25);
            this._bMoveUp.SizeImgButton = new System.Drawing.Size(0, 0);
            this._bMoveUp.TabIndex = 6;
            this._bMoveUp.Click += new System.EventHandler(this._bMoveUp_Click);
            // 
            // _bMoveDown
            // 
            this._bMoveDown.DefaultScheme = false;
            this._bMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("_bMoveDown.Image")));
            this._bMoveDown.Location = new System.Drawing.Point(110, 24);
            this._bMoveDown.Name = "_bMoveDown";
            this._bMoveDown.Scheme = ActiveUp.WebControls.WinControls.ButtonXP.Schemes.Silver;
            this._bMoveDown.Size = new System.Drawing.Size(25, 25);
            this._bMoveDown.SizeImgButton = new System.Drawing.Size(0, 0);
            this._bMoveDown.TabIndex = 7;
            this._bMoveDown.Click += new System.EventHandler(this._bMoveDown_Click);
            // 
            // _bSlibingParent
            // 
            this._bSlibingParent.DefaultScheme = false;
            this._bSlibingParent.Image = ((System.Drawing.Image)(resources.GetObject("_bSlibingParent.Image")));
            this._bSlibingParent.Location = new System.Drawing.Point(137, 24);
            this._bSlibingParent.Name = "_bSlibingParent";
            this._bSlibingParent.Scheme = ActiveUp.WebControls.WinControls.ButtonXP.Schemes.Silver;
            this._bSlibingParent.Size = new System.Drawing.Size(25, 25);
            this._bSlibingParent.SizeImgButton = new System.Drawing.Size(0, 0);
            this._bSlibingParent.TabIndex = 8;
            this._bSlibingParent.Click += new System.EventHandler(this._bSlibingParent_Click);
            // 
            // _bChildSlibing
            // 
            this._bChildSlibing.DefaultScheme = false;
            this._bChildSlibing.Image = ((System.Drawing.Image)(resources.GetObject("_bChildSlibing.Image")));
            this._bChildSlibing.Location = new System.Drawing.Point(164, 24);
            this._bChildSlibing.Name = "_bChildSlibing";
            this._bChildSlibing.Scheme = ActiveUp.WebControls.WinControls.ButtonXP.Schemes.Silver;
            this._bChildSlibing.Size = new System.Drawing.Size(25, 25);
            this._bChildSlibing.SizeImgButton = new System.Drawing.Size(0, 0);
            this._bChildSlibing.TabIndex = 9;
            this._bChildSlibing.Click += new System.EventHandler(this._bChildSlibing_Click);
            // 
            // _pgTreeView
            // 
            this._pgTreeView.LineColor = System.Drawing.SystemColors.ScrollBar;
            this._pgTreeView.Location = new System.Drawing.Point(257, 26);
            this._pgTreeView.Name = "_pgTreeView";
            this._pgTreeView.Size = new System.Drawing.Size(243, 244);
            this._pgTreeView.TabIndex = 10;
            this._pgTreeView.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this._pgTreeView_PropertyValueChanged);
            // 
            // _bOK
            // 
            this._bOK.DefaultScheme = false;
            this._bOK.Image = null;
            this._bOK.Location = new System.Drawing.Point(343, 273);
            this._bOK.Name = "_bOK";
            this._bOK.Scheme = ActiveUp.WebControls.WinControls.ButtonXP.Schemes.Silver;
            this._bOK.Size = new System.Drawing.Size(75, 23);
            this._bOK.SizeImgButton = new System.Drawing.Size(0, 0);
            this._bOK.TabIndex = 11;
            this._bOK.Text = "OK";
            this._bOK.Click += new System.EventHandler(this._bOK_Click);
            // 
            // _bCancel
            // 
            this._bCancel.DefaultScheme = false;
            this._bCancel.Image = null;
            this._bCancel.Location = new System.Drawing.Point(0, 0);
            this._bCancel.Name = "_bCancel";
            this._bCancel.Scheme = ActiveUp.WebControls.WinControls.ButtonXP.Schemes.Silver;
            this._bCancel.Size = new System.Drawing.Size(0, 0);
            this._bCancel.SizeImgButton = new System.Drawing.Size(0, 0);
            this._bCancel.TabIndex = 0;
            // 
            // _bCancel1
            // 
            this._bCancel1.DefaultScheme = false;
            this._bCancel1.Image = null;
            this._bCancel1.Location = new System.Drawing.Point(423, 273);
            this._bCancel1.Name = "_bCancel1";
            this._bCancel1.Scheme = ActiveUp.WebControls.WinControls.ButtonXP.Schemes.Silver;
            this._bCancel1.Size = new System.Drawing.Size(75, 23);
            this._bCancel1.SizeImgButton = new System.Drawing.Size(0, 0);
            this._bCancel1.TabIndex = 12;
            this._bCancel1.Text = "Cancel";
            this._bCancel1.Click += new System.EventHandler(this._bCancel1_Click);
            // 
            // PropertyBuilderForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(509, 299);
            this.Controls.Add(this._bCancel1);
            this.Controls.Add(this._bCancel);
            this.Controls.Add(this._bOK);
            this.Controls.Add(this._pgTreeView);
            this.Controls.Add(this._bChildSlibing);
            this.Controls.Add(this._bSlibingParent);
            this.Controls.Add(this._bMoveDown);
            this.Controls.Add(this._bMoveUp);
            this.Controls.Add(this._bDelete);
            this.Controls.Add(this._bAddChild);
            this.Controls.Add(this._bAddRoot);
            this.Controls.Add(this._tvTree);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PropertyBuilderForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Property Builder";
            this.Load += new System.EventHandler(this.TreeViewPropertyBuilderForm_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void InitUI()
		{
			if (_treeView.SelectionType == SelectionType.CheckBox)
				_tvTree.CheckBoxes = true;

			AddNodeTreeWindows(_treeView.Nodes,null);
		}

		private System.Windows.Forms.TreeNode AddNodeTreeWindows(System.Web.UI.ControlCollection parentCollection, TreeNode Parent)
		{
			System.Windows.Forms.TreeNode ret = null;

			foreach(object control in parentCollection)
			{

				ActiveUp.WebControls.TreeNode child  = control as ActiveUp.WebControls.TreeNode;
				if (child != null)

				{

					System.Windows.Forms.TreeNode newNode = new System.Windows.Forms.TreeNode(child.Text);
					if (child.Expanded == true)
						newNode.Expand();

					NodeDesign nodeList = null;

					if (Parent == null)
					{
						_tvTree.Nodes.Add(newNode);
					
						nodeList = new NodeDesign(child.Key,"0",child.Text,child.Link);
					
					}

					else
					{
						System.Windows.Forms.TreeNode parentNode = GetNode(Parent.Key,_tvTree.Nodes,TypeSearch.searchID);
						if (parentNode != null)
							parentNode.Nodes.Add(newNode);

						nodeList = new NodeDesign(child.Key,Parent.Key,child.Text,child.Link);
					
					}

					nodeList.Target = child.Target;
					nodeList.Expanded = child.Expanded;
					nodeList.Selected = child.Selected;
					nodeList.Icon = child.Icon;
					nodeList.NodeStyle = child.NodeStyle;
				
					if (child.Selected == true)
						newNode.Checked = true;

					newNode.Tag = nodeList;
					
					if (child.Nodes.Count > 0)
						ret = AddNodeTreeWindows(child.Nodes,child);
				
					if (ret != null) break;
				}
			}

			return ret;
			
		}

		private System.Windows.Forms.TreeNode GetNode(string node, System.Windows.Forms.TreeNodeCollection parentCollection,TypeSearch typeSearch)
		{
			System.Windows.Forms.TreeNode ret = null;

			foreach(System.Windows.Forms.TreeNode child in parentCollection)
			{
				if (typeSearch == TypeSearch.searchID)
				{
					if ((string)((NodeDesign)child.Tag).Key == node)
						ret = child;
				
					else if (child.GetNodeCount(false) > 0)
						ret = GetNode(node,child.Nodes,TypeSearch.searchID);
				}

				else if (typeSearch == TypeSearch.searchText)
				{
					if (child.Text == node)
						ret = child;
				
					else if (child.GetNodeCount(false) > 0)
						ret = GetNode(node,child.Nodes,TypeSearch.searchText);
				}

				if (ret != null) break;
			}

			return ret;
		}

		private System.Windows.Forms.TreeNode GetLastNode(System.Windows.Forms.TreeNodeCollection parentCollection)
		{
			return parentCollection[parentCollection.Count - 1];
		}

		private void _bAddRoot_Click(object sender, System.EventArgs e)
		{
			string defaultNodeName = GetNextDefaultNodeName();

			System.Windows.Forms.TreeNode newNodeWin = new System.Windows.Forms.TreeNode(defaultNodeName);
			newNodeWin.Tag = new NodeDesign();
			((NodeDesign)newNodeWin.Tag).Text = defaultNodeName;

			_tvTree.Nodes.Add(newNodeWin);
					
			_tvTree.SelectedNode = GetLastNode(_tvTree.Nodes);
			_tvTree.Focus();

			((NodeDesign)_tvTree.SelectedNode.Tag).Key = "ATV_" + DateTime.Now.Ticks.ToString();
			
			if (_bAddChild.Enabled == false)
				_bAddChild.Enabled = true;

			if (_bDelete.Enabled == false)
				_bDelete.Enabled = true;

			_pgTreeView.SelectedObject = (NodeDesign)newNodeWin.Tag;
		}

		private void _bAddChild_Click(object sender, System.EventArgs e)
		{
			string defaultNodeName = GetNextDefaultNodeName();

			if (_tvTree.SelectedNode != null)
			{
				System.Windows.Forms.TreeNode newNode = new System.Windows.Forms.TreeNode(defaultNodeName);
				newNode.Tag = new NodeDesign();
				((NodeDesign)newNode.Tag).Text = defaultNodeName;
				_tvTree.SelectedNode.Nodes.Add(newNode);
				_tvTree.SelectedNode.Expand();
				_tvTree.SelectedNode = _tvTree.SelectedNode.LastNode;
				_tvTree.Focus();

				((NodeDesign)_tvTree.SelectedNode.Tag).Key = "ATV_" + DateTime.Now.Ticks.ToString();
			}

			if (_bDelete.Enabled == false)
				_bDelete.Enabled= true;
		}

		private void _bDelete_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.TreeNode newSelectedTreeNode = new System.Windows.Forms.TreeNode();

			if (_tvTree.SelectedNode != null)
			{
				if (_tvTree.SelectedNode.Parent != null)
					newSelectedTreeNode = _tvTree.SelectedNode.Parent;

				_tvTree.SelectedNode.Remove();

				_tvTree.SelectedNode = newSelectedTreeNode;
				_tvTree.Select();
			}

			if (_tvTree.Nodes.Count == 0)
			{
				_bAddChild.Enabled = false;
				_bDelete.Enabled = false;

				_pgTreeView.SelectedObject = null;
			}
		}

		private void _bMoveUp_Click(object sender, System.EventArgs e)
		{
            PerformMoveUpButtonClickEventOperations();
		}

		private void _bMoveDown_Click(object sender, System.EventArgs e)
		{
            PerformMoveDownButtonClickEventOperations();
		}

		private void _bSlibingParent_Click(object sender, System.EventArgs e)
		{
            PerformSiblingParentButtonClickEventOperations();
		}

		private void _bChildSlibing_Click(object sender, System.EventArgs e)
		{
            PerformChildSiblingButtonClickEventOperations();
		}

		private string GetNextDefaultNodeName()
		{
			string defaultNodeName = "Node" + _nodeNumber;
			_nodeNumber++;

			return defaultNodeName;
		}

		private void _tvTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
            PerformTreeViewMenuAfterSelectEventOperations();

			if (_tvTree.SelectedNode != null)
				_pgTreeView.SelectedObject = (NodeDesign)_tvTree.SelectedNode.Tag;
		}

		private void _bCancel1_Click(object sender, System.EventArgs e)
		{
			DialogResult = WinForms.DialogResult.Cancel;
			Close();
		}

		private void _bOK_Click(object sender, System.EventArgs e)
		{
			_treeView.Nodes.Clear();

			AddNodes(_tvTree.Nodes);

			DialogResult = WinForms.DialogResult.OK;
			Close();
		}

		private System.Windows.Forms.TreeNode AddNodes(System.Windows.Forms.TreeNodeCollection parentCollection)
		{
			System.Windows.Forms.TreeNode ret = null;

			foreach(System.Windows.Forms.TreeNode child in parentCollection)
			{
				TreeNode treeNode = _treeView.FindNode((string)((NodeDesign)child.Tag).Key);

				NodeDesign currentNode = (NodeDesign)child.Tag;
				if (treeNode == null)
				{
					TreeNode newNode = null;

					if (child.Parent == null)
					{
						newNode = _treeView.AddNode(currentNode.Key,currentNode.Text,currentNode.Link,currentNode.Target, currentNode.Expanded, currentNode.Icon, currentNode.NodeStyle);
					}

					else
					{
						newNode = _treeView.FindNode((string)((NodeDesign)child.Parent.Tag).Key).AddNode(currentNode.Key,currentNode.Text,currentNode.Link,currentNode.Target, currentNode.Expanded, currentNode.Icon, currentNode.NodeStyle);
					}

					if (_treeView.SelectionType == SelectionType.CheckBox)
						newNode.Selected = child.Checked;
					else
						newNode.Selected = false;
				}

				if (child.GetNodeCount(false) > 0)
					ret = AddNodes(child.Nodes);
				
				if (ret != null) break;
			}

			return null;
		}

		private void _pgTreeView_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			switch (e.ChangedItem.Label)
			{
				case "Expanded":
				{
					if ((bool)e.ChangedItem.Value == true)
					{
						_tvTree.SelectedNode.Expand();
					}

					else if ((bool)e.ChangedItem.Value == false)
					{
						_tvTree.SelectedNode.Collapse();
					}

				} break;

				case "Text":
				{
					_tvTree.SelectedNode.Text = (string)e.ChangedItem.Value;
				} break;

				case "Selected":
				{
					if (_treeView.SelectionType == SelectionType.CheckBox)
					{
						if ((bool)e.ChangedItem.Value == true)
						{
							_tvTree.SelectedNode.Checked = true;
						}

						else if ((bool)e.ChangedItem.Value == false)
						{
							_tvTree.SelectedNode.Checked = false;
						}
					}

				} break;

				default : break;
			}
		}

		private void _tvTree_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (_isInitialized == true)
			{
				_tvTree.SelectedNode =  e.Node;

				NodeDesign selNode = (NodeDesign)_tvTree.SelectedNode.Tag;

				if (selNode != null)
				{
					if (e.Node.Checked == true)
						selNode.Selected = true;
					else
						selNode.Selected = false;

					_pgTreeView.SelectedObject = selNode;
				}
			}
		}

		private void TreeViewPropertyBuilderForm_Load(object sender, System.EventArgs e)
		{
			_isInitialized = true;
			_bAddChild.Enabled = false;
			_bDelete.Enabled = false;
			_bChildSlibing.Enabled = false;
			_bSlibingParent.Enabled = false;
			_bMoveUp.Enabled = false;
			_bMoveDown.Enabled = false;
		}

		private void _tvTree_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (_isInitialized == true)
			{
				_tvTree.SelectedNode =  e.Node;

				NodeDesign selNode = (NodeDesign)_tvTree.SelectedNode.Tag;

				if (selNode != null)
				{
					selNode.Expanded = false;
					_pgTreeView.SelectedObject = selNode;
				}
			}
		}

		private void _tvTree_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (_isInitialized == true)
			{
				
				_tvTree.SelectedNode =  e.Node;

				NodeDesign selNode = (NodeDesign)_tvTree.SelectedNode.Tag;

				if (selNode != null)
				{
					selNode.Expanded = true;
					_pgTreeView.SelectedObject = selNode;
				}
			}
		}

        public override WinForms.TreeView TreeViewMenu { get { return _tvTree; } }
        public override ButtonXP MoveUpButton { get { return _bMoveUp; } }
        public override ButtonXP MoveDownButton { get { return _bMoveDown; } }
        public override ButtonXP ChildSiblingButton { get { return _bChildSlibing; } }
        public override ButtonXP SiblingParentButton { get { return _bSlibingParent; } }
        public override ButtonXP AddChildButton { get { return _bAddChild; } }
        public override ButtonXP RemoveNodeButton { get { return _bDelete; } }
    }
}
