using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using WinForms = System.Windows.Forms;
using System.Data;
using System.Web.UI.WebControls;
using ActiveUp.WebControls.WinControls;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="MenuPropertyBuilderForm"/> object.
	/// </summary>
    public class MenuPropertyBuilderForm : TreeViewPropertyBuilderFormBase
    {
        private const string ItemsText = "Items : ";
        private const string PropertiesText = "Properties :";
        private const string ToolbarButton1Text = "aaaaaa";
        private const string CancelText = "Cancel";
        private const string OkText = "OK";
        private const string MenuPropertyBuilderText = "ToolMenu Property Builder";
        private System.Windows.Forms.ToolBarButton toolBarButton1;
        private System.Windows.Forms.TreeView _tvMenu;

        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

        private Menu _menu = null;
        private ActiveUp.WebControls.WinControls.ButtonXP _bAddRoot;
        private ActiveUp.WebControls.WinControls.ButtonXP _bAddChild;
        private ActiveUp.WebControls.WinControls.ButtonXP _bRemoveNode;
        private ActiveUp.WebControls.WinControls.ButtonXP _bMoveUp;
        private ActiveUp.WebControls.WinControls.ButtonXP _bMoveDown;
        private ActiveUp.WebControls.WinControls.ButtonXP _bSlibingParent;
        private ActiveUp.WebControls.WinControls.ButtonXP _bChildSibling;
        private ActiveUp.WebControls.WinControls.ButtonXP _bOK;
        private ActiveUp.WebControls.WinControls.ButtonXP _bCancel;
        private System.Windows.Forms.PropertyGrid _pgMenu;
        private static int _nodeNumber = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="MenuPropertyBuilderForm"/> class.
		/// </summary>
        public MenuPropertyBuilderForm()
        {
            //
            // Requis pour la prise en charge du Concepteur Windows Forms
            //
            InitializeComponent();
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="MenuPropertyBuilderForm"/> class.
		/// </summary>
		/// <param name="menu">The menu.</param>
        public MenuPropertyBuilderForm(Menu menu)
        {
            InitializeComponent();

            _menu = menu;

            InitUI();
        }

        private void InitUI()
        {
            foreach (MenuItem menuItem in _menu.MenuItems)
            {
                System.Windows.Forms.TreeNode newBaseNode = new System.Windows.Forms.TreeNode();
                newBaseNode.Text = menuItem.Text;
                newBaseNode.Tag = ConvertToItemProperty(menuItem);
                _tvMenu.Nodes.Add(newBaseNode);

                /*if (menuItem.SubMenu.SubMenuItems.Count > 0)
                {
                    foreach (ToolSubMenuItem subMenuItem in menuItem.SubMenu.SubMenuItems)
                    {
                        TreeNode newNode = new TreeNode();
                        newNode.Text = subMenuItem.Text;
                        newNode.Tag = ConvertToItemProperty(subMenuItem);
                        newBaseNode.Nodes.Add(newNode);

                        if (subMenuItem.SubMenu.SubMenuItems.Count > 0)
                            InitUISubMenuItems(subMenuItem, newNode);
                    }
                }  */

                if (menuItem.SubMenu.Items.Count > 0)
                {
                    foreach (MenuItem item in menuItem.SubMenu.Items)
                    {
                        System.Windows.Forms.TreeNode newNode = new System.Windows.Forms.TreeNode();
                        newNode.Text = item.Text;
                        newNode.Tag = ConvertToItemProperty(item);
                        newBaseNode.Nodes.Add(newNode);

                        if (item.SubMenu.Items.Count > 0)
                            InitUISubMenuItems(item, newNode);
                    }
                }
            }
        }

        private void InitUISubMenuItems(MenuItem parent, System.Windows.Forms.TreeNode current)
        {
            /*foreach (ToolSubMenuItem subMenuItem in parent.SubMenu.SubMenuItems)
            {
                TreeNode newNode = new TreeNode();
                newNode.Text = subMenuItem.Text;
                newNode.Tag = ConvertToItemProperty(subMenuItem);
                current.Nodes.Add(newNode);

                if (subMenuItem.SubMenu.SubMenuItems.Count > 0)
                    InitUISubMenuItems(subMenuItem, newNode);
            } */

            foreach (MenuItem item in parent.SubMenu.Items)
            {
                System.Windows.Forms.TreeNode newNode = new System.Windows.Forms.TreeNode();
                newNode.Text = item.Text;
                newNode.Tag = ConvertToItemProperty(item);
                current.Nodes.Add(newNode);

                if (item.SubMenu.Items.Count > 0)
                    InitUISubMenuItems(item, newNode);
            }
        }

        private MenuItemProperty ConvertToItemProperty(MenuItem source)
        {
            MenuItemProperty item = new MenuItemProperty();
            item.Text = source.Text;
            item.UniqueID = source.ClientID;
            item.Align = source.Align;
            item.Width = source.Width;
            item.Height = source.Height;
            item.Image = source.Image;
            item.ImageOver = source.ImageOver;
            item.NavigateURL = source.NavigateURL;
            item.Target = source.Target;
            item.OnClickClient = source.OnClickClient;

            return item;
        }
        
        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new ComponentResourceManager(typeof(MenuPropertyBuilderForm));
            this._pgMenu = new WinForms.PropertyGrid();
            this.toolBarButton1 = new WinForms.ToolBarButton();
            this._tvMenu = new WinForms.TreeView();
            this.label1 = new WinForms.Label();
            this.label2 = new WinForms.Label();
            _bCancel = new ButtonXP();
            _bOK = new ButtonXP();
            _bMoveUp = new ButtonXP();
            _bMoveDown = new ButtonXP();
            _bAddChild = new ButtonXP();
            _bRemoveNode = new ButtonXP();
            _bAddRoot = new ButtonXP();
            _bChildSibling = new ButtonXP();
            _bSlibingParent = new ButtonXP();

            this.SuspendLayout();

            SetControlProperties(_pgMenu, nameof(_pgMenu), new Point(250, 30), new Size(245, 228), 6);
            SetControlProperties(_tvMenu, nameof(_tvMenu), new Point(9, 53), new Size(232, 203), 7);
            SetControlProperties(label1, nameof(label1), new Point(10, 8), new Size(189, 16), 15);
            SetControlProperties(label2, nameof(label2), new Point(250, 11), new Size(189, 16), 16);
            SetControlProperties(_bCancel, nameof(_bCancel), new Point(75, 23), 25, _bCancel_Click, null);
            SetControlProperties(_bOK, nameof(_bOK), new Point(337, 265), 24, _bOK_Click, null);
            SetControlProperties(_bMoveUp, nameof(_bMoveUp), new Point(89, 24), 23, _bMoveUp_Click, resources);
            SetControlProperties(_bMoveDown, nameof(_bMoveDown), new Point(116, 24), 22, _bMoveDown_Click, resources);
            SetControlProperties(_bAddChild, nameof(_bAddChild), new Point(35, 24), 21, _bAddChild_Click, resources);
            SetControlProperties(_bRemoveNode, nameof(_bRemoveNode), new Point(62, 24), 20, _bRemoveNode_Click, resources);
            SetControlProperties(_bAddRoot, nameof(_bAddRoot), new Point(8, 24), 19, _bAddRoot_Click, resources);
            SetControlProperties(_bChildSibling, nameof(_bChildSibling), new Point(169, 24), 18, _bChildSibling_Click, resources);
            SetControlProperties(_bSlibingParent, nameof(_bSlibingParent), new Point(142, 24), 17, _bSlibingParent_Click, resources);

            this._pgMenu.LineColor = SystemColors.ScrollBar;
            this._pgMenu.PropertyValueChanged += this._pgMenu_PropertyValueChanged;
            this.toolBarButton1.Text = ToolbarButton1Text;
            this._tvMenu.AfterSelect += this._tvMenu_AfterSelect;
            this.label1.Text = ItemsText;
            this.label2.Text = PropertiesText;
            this._bCancel.Text = CancelText;
            this._bOK.Text = OkText;

            this.AutoScaleBaseSize = new Size(5, 13);
            this.ClientSize = new Size(508, 290);
            this.Controls.AddRange(new []{ _bCancel, _bOK, _bMoveUp, _bMoveDown, _bAddChild, _bRemoveNode, _bAddRoot, _bChildSibling, _bSlibingParent});
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._pgMenu);
            this.Controls.Add(this._tvMenu);
            this.FormBorderStyle = WinForms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = $"Tool{nameof(MenuPropertyBuilderForm)}";
            this.StartPosition = WinForms.FormStartPosition.CenterScreen;
            this.Text = MenuPropertyBuilderText;
            this.Load += this.ToolMenuPropertyBuilderForm_Load;
            this.ResumeLayout(false);
        }

        private void SetControlProperties(ButtonXP button, string buttonName, Point location, int tabIndex, EventHandler eventHandler, ComponentResourceManager resources)
        {
            button.DefaultScheme = false;
            button.Image = resources?.GetObject($"{buttonName}.Image") as System.Drawing.Image;
            button.Scheme = ButtonXP.Schemes.Silver;
            button.SizeImgButton = new Size(0, 0);
            SetControlProperties(button, buttonName, location, new Size(25, 25), tabIndex);
            button.Click += eventHandler;
        }

        private void SetControlProperties(WinForms.Control control, string controlName, Point location, Size size, int tabIndex)
        {
            control.Name = controlName;
            control.Location = location;
            control.Size = size;
            control.TabIndex = tabIndex;
        }

        #endregion

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            WinForms.Application.Run(new MenuPropertyBuilderForm());
        }

        private string GetNextDefaultNodeName()
        {
            string defaultNodeName = "New Item" + _nodeNumber;
            _nodeNumber++;

            return defaultNodeName;
        }

        private System.Windows.Forms.TreeNode GetLastNode(System.Windows.Forms.TreeNodeCollection parentCollection)
        {
            return parentCollection[parentCollection.Count - 1];
        }

        private void _tvMenu_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            PerformTreeViewMenuAfterSelectEventOperations();

            if (_tvMenu.SelectedNode != null)
                _pgMenu.SelectedObject = (MenuItemProperty)_tvMenu.SelectedNode.Tag;
        }

        private void _bAddRoot_Click(object sender, System.EventArgs e)
        {
            string defaultNodeName = GetNextDefaultNodeName();

            System.Windows.Forms.TreeNode newNode = new System.Windows.Forms.TreeNode(defaultNodeName);

            MenuItemProperty menuItemProperty = new MenuItemProperty();
            menuItemProperty.UniqueID = Utils.GetUniqueID("AME_");
            menuItemProperty.Text = defaultNodeName;
            newNode.Tag = menuItemProperty;

            _tvMenu.Nodes.Add(newNode);

            _tvMenu.SelectedNode = GetLastNode(_tvMenu.Nodes);
            _tvMenu.Focus();

            if (_bAddChild.Enabled == false)
                _bAddChild.Enabled = true;
            if (_bRemoveNode.Enabled == false)
                _bRemoveNode.Enabled = true;
        }

        private void ToolMenuPropertyBuilderForm_Load(object sender, System.EventArgs e)
        {
            if (_tvMenu.SelectedNode != null)
            {
                _bAddChild.Enabled = true;
                _bRemoveNode.Enabled = true;
                _bMoveUp.Enabled = true;
                _bMoveDown.Enabled = true;
                _bChildSibling.Enabled = true;
                _bSlibingParent.Enabled = true;
            }

            else
            {
                _bAddChild.Enabled = false;
                _bRemoveNode.Enabled = false;
                _bMoveUp.Enabled = false;
                _bMoveDown.Enabled = false;
                _bChildSibling.Enabled = false;
                _bSlibingParent.Enabled = false;
            }
        }

        private void _bAddChild_Click(object sender, System.EventArgs e)
        {
            string defaultNodeName = GetNextDefaultNodeName();

            if (_tvMenu.SelectedNode != null)
            {
                System.Windows.Forms.TreeNode newNode = new System.Windows.Forms.TreeNode(defaultNodeName);

                MenuItemProperty menuItemProperty = new MenuItemProperty();
                menuItemProperty.UniqueID = Utils.GetUniqueID("AME_");
                menuItemProperty.Text = defaultNodeName;
                newNode.Tag = menuItemProperty;

                _tvMenu.SelectedNode.Nodes.Add(newNode);

                _tvMenu.SelectedNode.Expand();
                _tvMenu.SelectedNode = _tvMenu.SelectedNode.LastNode;
                _tvMenu.Focus();
            }

            if (_bRemoveNode.Enabled == false)
                _bRemoveNode.Enabled = true;
        }

        private void _bRemoveNode_Click(object sender, System.EventArgs e)
        {
            System.Windows.Forms.TreeNode newSelectedTreeNode = new System.Windows.Forms.TreeNode();

            if (_tvMenu.SelectedNode != null)
            {
                if (_tvMenu.SelectedNode.Parent != null)
                    newSelectedTreeNode = _tvMenu.SelectedNode.Parent;

                _tvMenu.SelectedNode.Remove();

                _tvMenu.SelectedNode = newSelectedTreeNode;
                _tvMenu.Select();
            }

            if (_tvMenu.Nodes.Count == 0)
            {
                _bAddChild.Enabled = false;
                _bRemoveNode.Enabled = false;
                _bMoveUp.Enabled = false;
                _bMoveDown.Enabled = false;
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

        private void _bChildSibling_Click(object sender, System.EventArgs e)
        {
            PerformChildSiblingButtonClickEventOperations();
        }

        private void _bOK_Click(object sender, System.EventArgs e)
        {
            _menu.MenuItems.Clear();

            AddMenuItems(_tvMenu.Nodes);

            DialogResult = WinForms.DialogResult.OK;
            Close();
        }

        private void AddMenuItems(System.Windows.Forms.TreeNodeCollection parentCollection)
        {
            foreach (System.Windows.Forms.TreeNode child in parentCollection)
            {
                MenuItemProperty menuItemProperty = (MenuItemProperty)child.Tag;
                MenuItem newMenuItem = new MenuItem();
                newMenuItem.ID = menuItemProperty.UniqueID;
                newMenuItem.Text = menuItemProperty.Text;
                if (menuItemProperty.Align != HorizontalAlign.NotSet)
                    newMenuItem.Align = menuItemProperty.Align;
                if (menuItemProperty.Height.Value > 0)
                    newMenuItem.Height = menuItemProperty.Height;
                if (menuItemProperty.Width.Value > 0)
                    newMenuItem.Width = menuItemProperty.Width;
                newMenuItem.SubMenu.ID = Utils.GetUniqueID("AME_");
                if (menuItemProperty.Image != string.Empty)
                    newMenuItem.Image = menuItemProperty.Image;
                if (menuItemProperty.ImageOver != string.Empty)
                    newMenuItem.ImageOver = menuItemProperty.ImageOver;
                if (menuItemProperty.NavigateURL != string.Empty)
                    newMenuItem.NavigateURL = menuItemProperty.NavigateURL;
                if (menuItemProperty.Target != string.Empty)
                    newMenuItem.Target = menuItemProperty.Target;
                if (menuItemProperty.OnClickClient != string.Empty)
                    newMenuItem.OnClickClient = menuItemProperty.OnClickClient;

                _menu.MenuItems.Add(newMenuItem);

                if (child.Nodes.Count > 0)
                    AddSubMenu(child.Nodes, newMenuItem);
            }
        }

        private void AddSubMenu(System.Windows.Forms.TreeNodeCollection parentCollection, MenuItem parent)
        {
            /*foreach (System.Windows.Forms.TreeNode item in parentCollection)
            {
                ToolMenuItemProperty menuItemProperty = (ToolMenuItemProperty)item.Tag;
                ToolSubMenuItem newSubMenuItem = new ToolSubMenuItem();

                if (menuItemProperty.Align != HorizontalAlign.NotSet)
                    newSubMenuItem.Align = menuItemProperty.Align;
                if (menuItemProperty.Height.Value > 0)
                    newSubMenuItem.Height = menuItemProperty.Height;
                if (menuItemProperty.Width.Value > 0)
                    newSubMenuItem.Width = menuItemProperty.Width;
                if (menuItemProperty.Image != string.Empty)
                    newSubMenuItem.Image = menuItemProperty.Image;
                if (menuItemProperty.ImageOver != string.Empty)
                    newSubMenuItem.ImageOver = menuItemProperty.ImageOver;
                if (menuItemProperty.NavigateURL != string.Empty)
                    newSubMenuItem.NavigateURL = menuItemProperty.NavigateURL;
                if (menuItemProperty.Target != string.Empty)
                    newSubMenuItem.Target = menuItemProperty.Target;
                if (menuItemProperty.OnClick != string.Empty)
                    newSubMenuItem.OnClick = menuItemProperty.OnClick;

                newSubMenuItem.ID = menuItemProperty.UniqueID;

                newSubMenuItem.Text = menuItemProperty.Text;
                newSubMenuItem.Align = menuItemProperty.Align;

                parent.SubMenu.ID = Utils.GetUniqueID("AME_");
                parent.SubMenu.SubMenuItems.Add(newSubMenuItem);

                if (item.Nodes.Count > 0)
                    AddSubMenuItems(item.Nodes, newSubMenuItem);
               }    */

            foreach (System.Windows.Forms.TreeNode item in parentCollection)
            {
                MenuItemProperty menuItemProperty = (MenuItemProperty)item.Tag;
                MenuItem newMenuItem = new MenuItem();

                if (menuItemProperty.Align != HorizontalAlign.NotSet)
                    newMenuItem.Align = menuItemProperty.Align;
                if (menuItemProperty.Height.Value > 0)
                    newMenuItem.Height = menuItemProperty.Height;
                if (menuItemProperty.Width.Value > 0)
                    newMenuItem.Width = menuItemProperty.Width;
                if (menuItemProperty.Image != string.Empty)
                    newMenuItem.Image = menuItemProperty.Image;
                if (menuItemProperty.ImageOver != string.Empty)
                    newMenuItem.ImageOver = menuItemProperty.ImageOver;
                if (menuItemProperty.NavigateURL != string.Empty)
                    newMenuItem.NavigateURL = menuItemProperty.NavigateURL;
                if (menuItemProperty.Target != string.Empty)
                    newMenuItem.Target = menuItemProperty.Target;
                if (menuItemProperty.OnClickClient != string.Empty)
                    newMenuItem.OnClickClient = menuItemProperty.OnClickClient;

                newMenuItem.ID = menuItemProperty.UniqueID;

                newMenuItem.Text = menuItemProperty.Text;
                newMenuItem.Align = menuItemProperty.Align;

                parent.SubMenu.ID = Utils.GetUniqueID("AME_");
                parent.SubMenu.Items.Add(newMenuItem);

                if (item.Nodes.Count > 0)
                    AddSubMenuItems(item.Nodes, newMenuItem);
            }
        }   

        private void AddSubMenuItems(System.Windows.Forms.TreeNodeCollection parentCollection, MenuItem parent)
        {
           /* foreach (TreeNode item in parentCollection)
            {
                ToolMenuItemProperty menuItemProperty = (ToolMenuItemProperty)item.Tag;
                ToolSubMenuItem newSubMenuItem = new ToolSubMenuItem();
                newSubMenuItem.ID = menuItemProperty.UniqueID;
                newSubMenuItem.Text = menuItemProperty.Text;
                newSubMenuItem.Align = menuItemProperty.Align;

                parent.SubMenu.ID = Utils.GetUniqueID("AME_");
                parent.SubMenu.SubMenuItems.Add(newSubMenuItem);

                if (item.Nodes.Count > 0)
                    AddSubMenuItems(item.Nodes, newSubMenuItem);
            }*/

            foreach (System.Windows.Forms.TreeNode item in parentCollection)
            {
                MenuItemProperty menuItemProperty = (MenuItemProperty)item.Tag;
                MenuItem newMenuItem = new MenuItem();
                newMenuItem.ID = menuItemProperty.UniqueID;
                newMenuItem.Text = menuItemProperty.Text;
                newMenuItem.Align = menuItemProperty.Align;

                parent.SubMenu.ID = Utils.GetUniqueID("AME_");
                parent.SubMenu.Items.Add(newMenuItem);

                if (item.Nodes.Count > 0)
                    AddSubMenuItems(item.Nodes, newMenuItem);
            }
        }      

        private void _bCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = WinForms.DialogResult.Cancel;
            Close();
        }

        private void _pgMenu_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label)
            {
                case "Text":
                    {
                        _tvMenu.SelectedNode.Text = (string)e.ChangedItem.Value;
                    } break;

                default: break;
            }
        }

        public override WinForms.TreeView TreeViewMenu { get { return _tvMenu; } }
        public override ButtonXP MoveUpButton { get { return _bMoveUp; } }
        public override ButtonXP MoveDownButton { get { return _bMoveDown; } }
        public override ButtonXP ChildSiblingButton { get { return _bChildSibling; } }
        public override ButtonXP SiblingParentButton { get { return _bSlibingParent; } }
        public override ButtonXP AddChildButton { get { return _bAddChild; } }
        public override ButtonXP RemoveNodeButton { get { return _bRemoveNode; } }
    }
}
