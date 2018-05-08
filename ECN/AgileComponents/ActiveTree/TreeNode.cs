using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
    /// <summary>
    /// Represents a node.
    /// </summary>
    [
	
	ParseChildren(true,"Nodes"),
	Serializable
	]
	public class TreeNode : CoreWebControl, INamingContainer, IPostBackDataHandler, IPostBackEventHandler
	{
		private const string IE = "IE";
		private string _link, _text, _key, _target, _icon, _parentKey;
		//private TreeNodeCollection _nodes;
		private System.Web.UI.WebControls.Style _nodeStyle;
		private TreeView _baseTree;
		private TreeNode _parentNode;
		
		
		private static int _cpt = 0;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TreeNode()
		{
			ID = GetNewID();
			_icon = string.Empty;
			_link = string.Empty;
			_text = string.Empty;
			_key = string.Empty;
			_target = string.Empty;
			_nodeStyle = new System.Web.UI.WebControls.Style();
			_nodeStyle.Font.Size = FontUnit.XSmall;
			
		}
		
		/// <summary>
		/// Gets the new ID.
		/// </summary>
		/// <returns></returns>
		public string GetNewID()
		{
			string newID = "ctl" + _cpt.ToString();
			_cpt++;
			return newID;
		}

		/// <summary>
		/// Gets or sets the access key (underlined letter) that allows you to quickly navigate to the Web server control.
		/// </summary>
		[Browsable(false)]
		public override string AccessKey
		{
			get {return base.AccessKey;}
			set {base.AccessKey = value;}
		}

		/// <summary>
		/// Gets or sets the background color of the Web server control.
		/// </summary>
		[Browsable(false)]
		public override Color BackColor
		{
			get {return base.BackColor;}
			set {base.BackColor = value;}
		}

		/// <summary>
		/// Gets or sets the border color of the Web control.
		/// </summary>
		[Browsable(false)]
		public override Color BorderColor
		{
			get {return base.BorderColor;}
			set {base.BorderColor = value;}
		}

		/// <summary>
		/// Gets or sets the border width of the Web server control.
		/// </summary>
		[Browsable(false)]
		public override Unit BorderWidth
		{
			get {return base.BorderWidth;}
			set {base.BorderWidth = value;}
		}

		/// <summary>
		/// Gets or sets the border style of the Web server control.
		/// </summary>
		[Browsable(false)]
		public override BorderStyle BorderStyle
		{
			get { return base.BorderStyle;}
			set { base.BorderStyle = value;}
		}

		/// <summary>
		/// Gets or sets the tab index of the Web server control.
		/// </summary>
		[Browsable(false)]
		public override short TabIndex
		{
			get { return base.TabIndex;}
			set { base.TabIndex = value;}
		}
		
		/// <summary>
		/// Gets or sets the text displayed when the mouse pointer hovers over the Web server control.
		/// </summary>
		[Browsable(false)]
		public override string ToolTip
		{
			get {return base.ToolTip;}
			set {base.ToolTip = value;}
		}

		/// <summary>
		/// Gets or sets the height of the Web server control.
		/// </summary>
		[Browsable(false)]
		public override Unit Height
		{
			get {return base.Height;}
			set {base.Height = value;}
		}

		/// <summary>
		/// Gets or sets the width of the Web server control.
		/// </summary>
		[Browsable(false)]
		public override Unit Width
		{
			get {return base.Width;}
			set {base.Width = value;}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether a server control is rendered as UI on the page.
		/// </summary>
		[Browsable(false)]
		public override bool Visible
		{
			get {return base.Visible;}
			set {base.Visible = value;}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the server control persists its view state, and the view state of any child controls it contains, to the requesting client.
		/// </summary>
		[Browsable(false)]
		public override bool EnableViewState
		{
			get {return base.EnableViewState;}
			set {base.EnableViewState = value;}
		}

		/// <summary>
		/// Gets or sets the programmatic identifier assigned to the server control.
		/// </summary>
		[Browsable(false)]
		public override string ID
		{
			get {return base.ID;}
			set {base.ID = value;}
		}


		/// <summary>
		/// Gets or sets the selected state.
		/// </summary>
		[
		Bindable(true), 
		Category("TreeView-Node"), 
		DefaultValue(false),
		Description("Selects the current node.")
		] 
		public bool Selected
		{
			get
			{
				object selected;
				selected = ViewState["_selected"];
				if (selected != null)
					return (bool)selected; 
				return false;
			}
			set
			{
				ViewState["_selected"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the expanded state of the node.
		/// </summary>
		[	Bindable(true), 
		Category("TreeView-Node"), 
		DefaultValue(true),
		Description("Expands the current node, it has effect only if it has elements.")
		] 
		public bool Expanded
		{
			get
			{
				object expanded;
				expanded = ViewState["_expanded"];
				if (expanded != null)
					return (bool)expanded; 
				return true;
			}
			set
			{
				ViewState["_expanded"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the displayed icon for the node.
		/// </summary>
		[
		Bindable(true), 
		Category("TreeView-Node"), 
		DefaultValue(""),
		Description("Displayed icon for the current node.")
		] 
		public string Icon
		{
			get
			{	
				return _icon;
			}

			set
			{
				_icon = value;
			}
		}

		/// <summary>
		/// Gets or sets the base tree.
		/// </summary>
		[
		Bindable(true), 
		Category("TreeView"), 
		DefaultValue(""),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public TreeView BaseTree
		{
			get
			{	
				if (_baseTree == null)
				{
					return GetBaseTree(this.Parent);
				}

				else
					return _baseTree;
			}

			set
			{
				_baseTree = value;
			}
		}

		private TreeView GetBaseTree(Control Parent)
		{
			if (Parent is ActiveUp.WebControls.TreeView)
				return (ActiveUp.WebControls.TreeView)Parent;

			if (Parent.Parent == null)
				return null;

			else return GetBaseTree(Parent.Parent);
		}


		/// <summary>
		/// Gets or sets the parent node.
		/// </summary>
		[
		Browsable(false),
		Bindable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public TreeNode ParentNode
		{
			get
			{
				if (_parentNode == null)
				{
					return GetParentNode(this.Parent);
					//return _parentNode;
				}

				else
					return _parentNode;
			}
			set
			{
				_parentNode = value;
			}
		}

		private TreeNode GetParentNode(Control Parent)
		{
			if (Parent is ActiveUp.WebControls.TreeNode)
				return (ActiveUp.WebControls.TreeNode)Parent;

			if (Parent == null || Parent.Parent == null)
				return null;

			else return GetParentNode(Parent.Parent);
			
		}
 
		/// <summary>
		/// Gets or sets the style of the current node.
		/// </summary>
		[
		Bindable(true), 
		Category("TreeView-Node"), 
		DefaultValue(""),
		//PersistenceMode( PersistenceMode.InnerProperty ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Description("Style of the current node.")
		] 
		public System.Web.UI.WebControls.Style NodeStyle
		{
			get
			{	
				if (_nodeStyle == null)
				{
					_nodeStyle = new System.Web.UI.WebControls.Style();
					_nodeStyle.Font.Size = FontUnit.XSmall;
				}
				return _nodeStyle;
			}

			set
			{
				_nodeStyle = value;
			}
		}

		
		/// <summary>
		/// Gets the collection of child nodes.
		/// </summary>
		[
		Browsable( true ),
		DefaultValue( null ),
		//PersistenceMode( PersistenceMode.InnerProperty ),
		PersistenceModeAttribute(PersistenceMode.InnerDefaultProperty),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		//MergablePropertyAttribute(false),
		//NotifyParentProperty(true)
		]
		public ControlCollection Nodes
		{
			get
			{
				return this.Controls;
			}
			
		}
		
		/// <summary>
		/// Gets or sets the displayed text.
		/// </summary>
		[
		Bindable(true), 
		Category("TreeView-Node"), 
		DefaultValue(""),
		Description("Text of the current node.")
		] 
		public string Text 
		{
			get
			{
				return _text;
			}

			set
			{
				_text = value;
			}
		}

		/// <summary>
		/// Gets or sets the link of the current node.
		/// </summary>
		[
		Bindable(true), 
		Category("TreeView-Node"), 
		DefaultValue(""),
		Description("Link of the current node.")
		] 
		public string Link 
		{
			get
			{
				return _link;
			}

			set
			{
				_link = value;
			}
		}

		/// <summary>
		/// Gets or sets the target frame of the link.
		/// </summary>
		[
		Bindable(true), 
		Category("TreeView-Node"), 
		DefaultValue(""),
		Description("Target frame of the link.")
		] 
		public string Target 
		{
			get
			{
				return _target;
			}

			set
			{
				_target = value;
			}
		}

		/// <summary>
		/// Gets or sets the unique key identifies the node.
		/// </summary>
		[
		Bindable(true), 
		Category("TreeView-Node"), 
		DefaultValue(""),
		Browsable(false)
		] 
		public string Key 
		{
			get
			{
				return _key;
			}

			set
			{
				_key = value;
			}
		}

		

		/// <summary>
		/// Gets or sets the parent key to identify the parent node.
		/// </summary>
		[
		Bindable(false),
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public string ParentKey
		{
			get
			{
				return _parentKey;
			}

			set
			{
				_parentKey = value;
			}
		}

		/// <summary>
		/// Add a node in the treeview specifying the node.
		/// </summary>
		/// <param name="treeNode">The node to add.</param>
		/// <returns>The node added.</returns>
		public TreeNode AddNode(TreeNode treeNode)
		{
			treeNode.ParentNode = this;
			this.Nodes.Add(treeNode);
			return treeNode;
		}

		/// <summary>
		/// Add a node in the treeview specifying the text and the link.
		/// </summary>
		/// <param name="text">Displayed text.</param>
		/// <param name="link">Link of the node.</param>
		/// <returns>The node added.</returns>
		public TreeNode AddNode(string text, string link)
		{
			TreeNode newNode = new TreeNode();
			newNode.BaseTree = BaseTree;
			newNode.Text = text;
			newNode.Link = link;
			newNode.ParentNode = this;
			this.Nodes.Add(newNode);
			return newNode;
		}

		/// <summary>
		/// Add a node in the treeview specifying the key, text and link.
		/// </summary>
		/// <param name="uniqueKey">Key of the node (must be unique for each node).</param>
		/// <param name="text">Displayed text.</param>
		/// <param name="link">Link of the node.</param>
		/// <returns>The node added.</returns>
		public TreeNode AddNode(string uniqueKey, string text, string link)
		{
			TreeNode newNode = new TreeNode();
			newNode.BaseTree = BaseTree;
			newNode.Key = uniqueKey;
			newNode.Text = text;
			newNode.Link = link;
			newNode.ParentNode = this;
			this.Nodes.Add(newNode);
			return newNode;
		}

		/// <summary>
		/// Add a node in the treeview specifying the key, text, link and target.
		/// </summary>
		/// <param name="uniqueKey">Key of the node (must be unique for each node).</param>
		/// <param name="text">Displayed text.</param>
		/// <param name="link">Link of the node.</param>
		/// <param name="target">Target frame of the link.</param>
		/// <returns>The node added.</returns>
		public TreeNode AddNode(string uniqueKey, string text, string link,string target)
		{
			TreeNode newNode = new TreeNode();
			newNode.BaseTree = BaseTree;
			newNode.Key = uniqueKey;
			newNode.Text = text;
			newNode.Link = link;
			newNode.Target = target;
			newNode.ParentNode = this;
			this.Nodes.Add(newNode);
			return newNode;
		}

		/// <summary>
		/// Add a node in the treeview specifying the key, text, link, target and expanded.
		/// </summary>
		/// <param name="uniqueKey">Key of the node (must be unique for each node).</param>
		/// <param name="text">Displayed text.</param>
		/// <param name="link">Link of the node.</param>
		/// <param name="target">Target frame of the link.</param>
		/// <param name="expanded">Indicates if the node must be expanded.</param>
		/// <returns>The node added.</returns>
		public TreeNode AddNode(string uniqueKey, string text, string link,string target, bool expanded)
		{
			TreeNode newNode = new TreeNode();
			newNode.BaseTree = BaseTree;
			newNode.Key = uniqueKey;
			newNode.Text = text;
			newNode.Link = link;
			newNode.Target = target;
			newNode.Expanded = expanded;
			newNode.ParentNode = this;
			this.Nodes.Add(newNode);
			return newNode;
		}

		/// <summary>
		/// Add a node in the treeview specifying the key, text, link, target and expanded.
		/// </summary>
		/// <param name="uniqueKey">Key of the node (must be unique for each node).</param>
		/// <param name="text">Displayed text.</param>
		/// <param name="link">Link of the node.</param>
		/// <param name="target">Target frame of the link.</param>
		/// <param name="expanded">Indicates if the node must be expanded.</param>
		/// <param name="selected">Selection of the node.</param>
		/// <returns>The node added.</returns>
		public TreeNode AddNode(string uniqueKey, string text, string link,string target, bool expanded, bool selected)
		{
			TreeNode newNode = new TreeNode();
			newNode.BaseTree = BaseTree;
			newNode.Key = uniqueKey;
			newNode.Text = text;
			newNode.Link = link;
			newNode.Target = target;
			newNode.Expanded = expanded;
			newNode.ParentNode = this;
			newNode.Selected = selected;
			this.Nodes.Add(newNode);
			return newNode;
		}

		/// <summary>
		/// Add a node in the treeview specifying the key, text, link, target, expanded and the icon.
		/// </summary>
		/// <param name="uniqueKey">Key of the node (must be unique for each node).</param>
		/// <param name="text">Displayed text.</param>
		/// <param name="link">Link of the node.</param>
		/// <param name="target">Target frame of the link.</param>
		/// <param name="expanded">Indicates if the node must be expanded.</param>
		/// <param name="icon">Icons of the node.</param>
		/// <returns>The node added.</returns>
		public TreeNode AddNode(string uniqueKey, string text, string link, string target, bool expanded, string icon)
		{
			TreeNode newNode = new TreeNode();
			newNode.BaseTree = BaseTree;
			newNode.Key = uniqueKey;
			newNode.Text = text;
			newNode.Link = link;
			newNode.Target = target;
			newNode.Expanded = expanded;
			newNode.Icon = icon;
			newNode.ParentNode = this;
			this.Nodes.Add(newNode);
			return newNode;
		}

		/// <summary>
		/// Add a node in the treeview specifying the key, text, link, target, expanded and the icon.
		/// </summary>
		/// <param name="uniqueKey">Key of the node (must be unique for each node).</param>
		/// <param name="text">Displayed text.</param>
		/// <param name="link">Link of the node.</param>
		/// <param name="target">Target frame of the link.</param>
		/// <param name="expanded">Indicates if the node must be expanded.</param>
		/// <param name="icon">Icons of the node.</param>
		/// <param name="selected">Selection of the node.</param>
		/// <returns>The node added.</returns>
		public TreeNode AddNode(string uniqueKey, string text, string link, string target, bool expanded, string icon, bool selected)
		{
			TreeNode newNode = new TreeNode();
			newNode.BaseTree = BaseTree;
			newNode.Key = uniqueKey;
			newNode.Text = text;
			newNode.Link = link;
			newNode.Target = target;
			newNode.Expanded = expanded;
			newNode.Icon = icon;
			newNode.ParentNode = this;
			newNode.Selected = selected;
			this.Nodes.Add(newNode);
			return newNode;
		}

		/// <summary>
		/// Add a node in the treeview specifying the key, text, link, target, expanded, icon and the node style.
		/// </summary>
		/// <param name="uniqueKey">Key of the node (must be unique for each node).</param>
		/// <param name="text">Displayed text.</param>
		/// <param name="link">Link of the node.</param>
		/// <param name="target">Target frame of the link.</param>
		/// <param name="expanded">Indicates if the node must be expanded.</param>
		/// <param name="icon">Icons of the node.</param>
		/// <param name="nodeStyle">Style of the node.</param>
		/// <returns>The node added.</returns>
		public TreeNode AddNode(string uniqueKey, string text, string link, string target, bool expanded, string icon, Style nodeStyle)
		{
			TreeNode newNode = new TreeNode();
			newNode.BaseTree = BaseTree;
			newNode.Key = uniqueKey;
			newNode.Text = text;
			newNode.Link = link;
			newNode.Target = target;
			newNode.Expanded = expanded;
			newNode.NodeStyle = nodeStyle;
			newNode.Icon = icon;
			newNode.ParentNode = this;
			this.Nodes.Add(newNode);
			return newNode;
		}

		/// <summary>
		/// Find a node.
		/// </summary>
		/// <param name="key">The unique key identifies the node.</param>
		/// <returns>The treenode found, otherwise null.</returns>
		public TreeNode FindNode(string key)
		{
			if (this.Key == key)
				return this;

			if (this.Nodes.Count > 0)
			{
				/*foreach(TreeNode node in this.Nodes)
				{
					if (node.FindNode(key) != null)
						return node.FindNode(key);
				}*/

				for (int i = 0 ; i < Nodes.Count ; i++)
				{
					if (Nodes[i] is TreeNode)
					{
						if (((TreeNode)Nodes[i]).FindNode(key) != null)
							return ((TreeNode)Nodes[i]).FindNode(key);
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Finds node the node by id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public TreeNode FindNodeById(string id)
		{
			if (this.ID == id)
				return this;

			if (this.Nodes.Count > 0)
			{
				for (int i = 0 ; i < Nodes.Count ; i++)
				{
					if (Nodes[i] is TreeNode)
					{
						if (((TreeNode)Nodes[i]).FindNodeById(id) != null)
							return ((TreeNode)Nodes[i]).FindNodeById(id);
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the collection of node of selected node.
		/// </summary>
		[
		Browsable(false),
		Bindable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public TreeNodeCollection SelectedNodes
		{
			get
			{
				TreeNodeCollection selectedNodes = new TreeNodeCollection();
				
				if (this.Selected)
					selectedNodes.Add(this);

				if (this.Nodes.Count > 0)
				{
					foreach(TreeNode node in this.Nodes)
						selectedNodes += node.SelectedNodes;
				}

				return selectedNodes;
			}

		}

		/// <summary>
		/// Do some work before rendering the control.
		/// </summary>
		/// <param name="e">Event Args</param>
		protected override void OnPreRender(EventArgs e) 
		{
			if (base.Page != null)
				Page.RegisterRequiresPostBack(this);

			base.OnPreRender(e);
		}
		
		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			var singleOk = RenderHeader(output);
			var isLast = RenderMiddle(output, singleOk);

			output.RenderBeginTag(HtmlTextWriterTag.Td); // Open TD 2
			RenderBaseTree(output);

			output.Write("<span id='{0}_text'>", ClientID);

			if(!string.IsNullOrEmpty(_target))
			{
				output.AddAttribute(HtmlTextWriterAttribute.Target, _target);
			}

			if(!Enabled)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Disabled, string.Empty);
			}

			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_nodeText");

			try
			{
				if(Context != null && Page.Request.Browser.Browser == IE)
				{
					output.AddStyleAttribute("cursor", "hand");
				}
				else
				{
					output.AddStyleAttribute("cursor", "pointer");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Exception occured : {ex}");
			}

			if (BaseTree.SelectionType == SelectionType.Single)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Onclick,
					$"selectNode('{BaseTree.ClientID}','{ClientID}')");
			}

			output.RenderBeginTag(HtmlTextWriterTag.A); // Open A 1
			output.Write(_text);
			output.RenderEndTag(); // Close A 1
			output.Write("</span>");
			output.RenderEndTag(); // Close TD 2
			output.RenderEndTag(); // Close TR 1

			// Render the child nodes if any
			RenderNodesEnd(output, isLast);

			output.RenderEndTag(); // Close Table
		}

		private void RenderBaseTree(HtmlTextWriter output)
		{
			if(BaseTree != null && !BaseTree.TextMode && Icon.Length > 0)
			{
				RenderIcon(output, Icon, null);
				output.Write("&nbsp;");
			}
			else if(BaseTree != null && !BaseTree.TextMode)
			{
				RenderIcon(output, BaseTree.Icons.Default, "folder.gif");
				output.Write("&nbsp;");
			}
			else if(BaseTree != null && BaseTree.TextMode)
			{
				output.Write("O");
			}
			else
			{
				output.Write("&nbsp;");
			}

			if(BaseTree != null && BaseTree.SelectionType == SelectionType.CheckBox)
			{
				var objId = ClientID + "_sel";

				if(BaseTree.AutoCheckChildren)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Onclick, $"checkChildren(\'{objId}\');");
				}

				if(BaseTree.SelectedGrayed)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Onclick, $"selectedGrayed(\'{objId}\');");
				}

				output.AddAttribute(HtmlTextWriterAttribute.Id, objId);
				output.AddAttribute(HtmlTextWriterAttribute.Name, objId);
				output.AddAttribute(HtmlTextWriterAttribute.Value, "true");
				output.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
				if(Selected)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Checked, "true");
				}

				if(!Enabled)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Disabled, string.Empty);
				}

				output.RenderBeginTag(HtmlTextWriterTag.Input);
				output.RenderEndTag();
			}

			if(!string.IsNullOrEmpty(_link) && Enabled)
			{
				if(BaseTree != null && BaseTree.LinkFormat.Length > 0)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Href, string.Format(BaseTree.LinkFormat, _link));
				}
				else
				{
					output.AddAttribute(HtmlTextWriterAttribute.Href, _link);
					if(BaseTree != null && BaseTree.SelectionType == SelectionType.Single)
					{
						output.AddAttribute(HtmlTextWriterAttribute.Onclick,
							$"selectNode('{BaseTree.ClientID}','{ClientID}')");
					}
				}
			}
			else if(BaseTree != null && (Click != null || BaseTree.UseSameClickEvent && BaseTree.Click != null))
			{
				output.AddAttribute(HtmlTextWriterAttribute.Href, "#");
				output.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.GetPostBackEventReference(this, string.Empty));
			}
		}

		private bool RenderMiddle(HtmlTextWriter output, bool singleOk)
		{
			var isLast = false;
			output.RenderBeginTag(HtmlTextWriterTag.Tr); // Open TR 1
			if(Nodes.Count > 0)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Onclick,
					BaseTree.LoadOnDemand
						? $"javascript:loadNode(\'{BaseTree.ClientID}\', \'{ID}\', \'{ClientID}\', \'{BaseTree.ClientID}\', \'{BaseTree.Path}\');"
						: $"javascript:toggleNode(\'{ClientID}\', \'{BaseTree.ClientID}\');");
			}

			if(singleOk)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Onclick,
					BaseTree.LoadOnDemand
						? $"javascript:loadNode(\'{BaseTree.ClientID}\', \'{ID}\', \'{ClientID}\', \'{BaseTree.ClientID}\', \'{BaseTree.Path}\');"
						: $"javascript:toggleNode(\'{ClientID}\', \'{BaseTree.ClientID}\');");
			}

			output.RenderBeginTag(HtmlTextWriterTag.Td); // Open TD 1

			// Rendering the connections
			var parentNode = ParentNode;
			if(parentNode != null && parentNode.Nodes[parentNode.Nodes.Count - 1] == this || BaseTree == this)
			{
				isLast = true;
			}

			RenderNodes(output, isLast);

			output.RenderEndTag(); // Close TD 1

			// Rendering the icon
			try
			{
				if(Context != null && Page?.Request != null && Page.Request.Browser.Browser == IE)
				{
					output.AddStyleAttribute("cursor", "hand");
				}
				else
				{
					output.AddStyleAttribute("cursor", "pointer");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Exception occured : {ex}");
			}

			return isLast;
		}

		private void RenderNodesEnd(HtmlTextWriter output, bool isLast)
		{
			if(Nodes.Count > 0)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Tr); // Open TR 2
				if(!isLast && BaseTree.Icons.Connection != string.Empty)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Background, BaseTree.Icons.Connection);
				}

				output.RenderBeginTag(HtmlTextWriterTag.Td); // Open TD 4
				output.RenderEndTag(); // Close TD 4

				output.RenderBeginTag(HtmlTextWriterTag.Td); // Open TD 5

				output.Write($"<div id=\"{$"{ClientID}_div"}\" style=\"display: {(Expanded ? "block" : "none")};\">");
				if(!BaseTree.LoadOnDemand || BaseTree.LoadOnDemand && Expanded)
				{
					RenderChildren(output);
				}

				output.Write("</div>");

				output.RenderEndTag(); // Close TD 5
				output.RenderEndTag(); // Close TR 2
			}
		}

		private bool RenderHeader(HtmlTextWriter output)
		{
			if(Selected && BaseTree.SelectionType == SelectionType.Single)
			{
				output.Write("<script language=\"javascript\">\n");
				output.Write("function ATV_create_{0}(e)\n", ClientID);
				output.Write("{\n");
				output.Write("selectNode('{0}','{1}');", BaseTree.ClientID, ClientID);
				output.Write("}\n");
				output.Write("window.RegisterEvent(\"onload\", ATV_create_{0});\n", ClientID);
				output.Write("\n</script>\n");
			}

			output.AddAttribute(HtmlTextWriterAttribute.Type, "Hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
			output.AddAttribute(HtmlTextWriterAttribute.Value, DateTime.Now.Ticks.ToString());
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
			output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Value, Expanded.ToString());
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_expanded");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_expanded");
			output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Value, Expanded.ToString());
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			NodeStyle.AddAttributesToRender(output);

			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_table");
			output.RenderBeginTag(HtmlTextWriterTag.Table); // Open Table
			var singleOk = false;

			if(Click == null && BaseTree.UseSameClickEvent && BaseTree.Click == null &&
			   (_link == null || _link == string.Empty && Enabled) && Nodes.Count > 0 &&
			   BaseTree.SelectionType != SelectionType.CheckBox)
			{
				if(BaseTree.SelectionType != SelectionType.Single)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Onclick,
						BaseTree.LoadOnDemand
							? $"javascript:loadNode(\'{BaseTree.ClientID}\', \'{ID}\', \'{ClientID}\', \'{BaseTree.ClientID}\', \'{BaseTree.Path}\');"
							: $"javascript:toggleNode(\'{ClientID}\', \'{BaseTree.ClientID}\');");
				}
				else
				{
					singleOk = true;
				}
			}

			return singleOk;
		}

		private void RenderNodes(HtmlTextWriter output, bool isLast)
		{
			if(Nodes.Count > 0)
			{
				if(BaseTree != null && !BaseTree.TextMode)
				{
					if(Expanded)
					{
						RenderIcon(output, BaseTree.Icons.Expanded, "minus.gif");
					}
					else
					{
						RenderIcon(output, BaseTree.Icons.Collapsed, "plus.gif");
					}
				}
				else if(BaseTree != null && BaseTree.TextMode)
				{
					output.Write("+");
				}
				else
				{
					output.Write("&nbsp;");
				}
			}
			else
			{
				if(BaseTree != null && !BaseTree.TextMode)
				{
					if(!isLast)
					{
						RenderIcon(output, BaseTree.Icons.Orphan, "spacer.gif");
					}
					else if(this != BaseTree)
					{
						RenderIcon(output, BaseTree.Icons.Last, "spacer.gif");
					}
				}
				else if(BaseTree != null && BaseTree.TextMode)
				{
					output.Write("|");
				}
				else
				{
					output.Write("&nbsp;");
				}
			}
		}

		private void RenderIcon(HtmlTextWriter output, string image, string imageResource)
		{
			output.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            output.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertToImageDir(this.BaseTree.ImagesDirectory, image, imageResource, Page, this.GetType()));
			output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_img");
			output.RenderBeginTag(HtmlTextWriterTag.Img);
			output.RenderEndTag();
		}

		/// <summary>
		/// A LoadPostData method.
		/// </summary>
		/// <param name="postDataKey">PostDataKey.</param>
		/// <param name="postCollection">postCollection.</param>
		/// <returns>bool</returns>
		public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{
			Page.Trace.Warn(this.ClientID,"LoadPostData...");

			if (BaseTree.SelectionType == SelectionType.CheckBox)
			{
				string presentSelection = this.Selected.ToString().ToUpper();
				try
				{
					string postedSelection = postCollection[postDataKey.Replace(":","_").Replace("$","_") + "_sel"].ToUpper();

					if (presentSelection == null || !presentSelection.Equals(postedSelection)) 
					{
						this.Selected = Convert.ToBoolean(postedSelection);

						OnSelectionChanged(EventArgs.Empty);
					}
				}
				catch (Exception)
				{
					this.Selected = false;
					if (presentSelection == "TRUE")
						OnSelectionChanged(EventArgs.Empty);
				}
			}
			else if (BaseTree.SelectionType == SelectionType.Single)
			{
				try
				{
					string postedSelection = postCollection[string.Format("atv_{0}_curSelNode",BaseTree.ClientID)];
					if (postedSelection == this.ClientID)
					{
						this.Selected = true;
					}
					else
						this.Selected = false;
				}

				catch
				{
					this.Selected = false;
				}
			}

			try
			{
				string presentValue = this.Expanded.ToString().ToUpper();
				string postedValue = string.Empty;
				postedValue = postCollection[postDataKey + "_expanded"].ToUpper();
			
				if (presentValue == null || !presentValue.Equals(postedValue)) 
				{
					this.Expanded = Convert.ToBoolean(postedValue);
					//OnStateChanged(EventArgs.Empty);

					return true;
				}
			}

			catch (Exception)
			{
			}

			return false;
		}


		/// <summary>
		/// A RaisePostDataChangedEvent.
		/// </summary>
		public virtual void RaisePostDataChangedEvent() 
		{
			OnStateChanged(EventArgs.Empty);
		}

		/// <summary>
		/// A RaisePostBackEvent.
		/// </summary>
		/// <param name="eventArgument">eventArgument</param>
		public void RaisePostBackEvent(String eventArgument)
		{
			OnClick(EventArgs.Empty);
		}
      
		/// <summary>
		/// The StateChanged event handler.
		/// </summary>
		public event EventHandler StateChanged;
     
		/// <summary>
		/// A OnStateChanged event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnStateChanged(EventArgs e) 
		{
			if (StateChanged != null)
				StateChanged(this,e);
		}

		/// <summary>
		/// The StateChanged event handler.
		/// </summary>
		public event EventHandler SelectionChanged;
     
		/// <summary>
		/// A OnStateChanged event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSelectionChanged(EventArgs e) 
		{
			if (SelectionChanged != null)
				SelectionChanged(this,e);
		}

		/// <summary>
		/// The Click event handler. Fire when you click on a node.
		/// </summary>
		[Category("TreeView-Node")]
		public event EventHandler Click;
     
		/// <summary>
		/// A OnClick event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnClick(EventArgs e) 
		{
			if (BaseTree.UseSameClickEvent)
			{
				if (BaseTree.Click != null)
					BaseTree.Click(this,e);
			}
			else
			{
				if (Click != null)
					Click(this,e);
			}
		}

	

	}
}
