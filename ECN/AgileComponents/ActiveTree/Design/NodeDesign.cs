using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace ActiveUp.WebControls.Design
{
	/// <summary>
	/// Used to create node in the property builder.
	/// </summary>
	public class NodeDesign
	{
		private bool _selected;
		private bool _expanded;
		private string _icon;
		private System.Web.UI.WebControls.Style _nodeStyle;
		private string _text;
		private string _link;
		private string _target;
		private string _key;
		private string _parentKey;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NodeDesign()
		{
			//
			// TODO : ajoutez ici la logique du constructeur
			//
			_Init("","0","","","");
		}

		/// <summary>
		/// Create a Node design object from the key, parent key and text.
		/// </summary>
		/// <param name="key">Unique key identifies the node.</param>
		/// <param name="parentKey">Parent key to identify the parent node.</param>
		/// <param name="text">Text of the current node.</param>
		public NodeDesign(string key, string parentKey, string text)
		{
			_Init(key,parentKey,text,"","");
		}

		/// <summary>
		/// Create a Node design object from the key, parent key, text and link.
		/// </summary>
		/// <param name="key">Unique key identifies the node.</param>
		/// <param name="parentKey">Parent key to identify the parent node.</param>
		/// <param name="text">Text of the current node.</param>
		/// <param name="link">Link of the current node.</param>
		public NodeDesign(string key,string parentKey, string text, string link)
		{
			_Init(key,parentKey,text,link,"");
		}

		/// <summary>
		/// Create a Node design object from the key, parent key, text, link and target.
		/// </summary>
		/// <param name="key">Unique key identifies the node.</param>
		/// <param name="parentKey">Parent key to identify the parent node.</param>
		/// <param name="text">Text of the current node.</param>
		/// <param name="link">Link of the current node.</param>
		/// <param name="target">Target frame of the link.</param>
		public NodeDesign(string key,string parentKey,string text,string link, string target)
		{
			_Init(key,parentKey,text,link,target);
		}

		/// <summary>
		/// Initalize a <see cref="NodeDesign"/> objet.
		/// </summary>
		/// <param name="key">Unique key identifies the node.</param>
		/// <param name="parentKey">Parent key to identify the parent node.</param>
		/// <param name="text">Text of the current node.</param>
		/// <param name="link">Link of the current node.</param>
		/// <param name="target">Target frame of the link.</param>
		private void _Init(string key,string parentKey,string text,string link, string target)
		{
			_key = key;
			_parentKey = parentKey;
			_text = text;
			_link = link;
			_selected = false;
			_expanded = false;
			_nodeStyle = new System.Web.UI.WebControls.Style();
			_nodeStyle.Font.Size = FontUnit.XSmall;
			_target = target;
			_icon = "";
		}

		/// <summary>
		/// Gets or sets the selected state.
		/// </summary>
		[
			Browsable(true),
			Description("Selects the current node."),
			DefaultValue("")
		]
		public bool Selected
		{
			get {return _selected;}
			set {_selected = value;}
		}
        
		/// <summary>
		/// Gets or sets the expanded state of the node.
		/// </summary>
		[
			Browsable(true),
			Description("Expands the current node, it has effect only if it has elements."),
			DefaultValue("")
		]
		public bool Expanded
		{
			get {return _expanded;}
			set {_expanded = value;}
		}

		/// <summary>
		/// Gets or sets the displayed icon for the node.
		/// </summary>
		[
			Browsable(true),
			Description("Displayed icon for the current node."),
			DefaultValue("")
		]
		public string Icon
		{
			get {return _icon;}
			set {_icon = value;}
		}

		/// <summary>
		/// Gets or sets the style of the current node.
		/// </summary>
		[
			Browsable(true),
			Description("Style of the current node."),
			DefaultValue("")
		]
		public System.Web.UI.WebControls.Style NodeStyle
		{
			get {return _nodeStyle;}
			set {_nodeStyle = value;}
		}

		/// <summary>
		/// Gets or sets the displayed text.
		/// </summary>
		[
			Browsable(true),
			Description("Text of the current node."),
			DefaultValue("")
		]
		public string Text
		{
			get {return _text;}
			set {_text = value;}
		}

		/// <summary>
		/// Gets or sets the link of the current node.
		/// </summary>
		[
			Browsable(true),
			Description("Link of the current node."),
			DefaultValue("")
		]
		public string Link
		{
			get {return _link;}
			set {_link = value;}
		}

		/// <summary>
		/// Gets or sets the target frame of the link.
		/// </summary>
		[
			Browsable(true),
			Description("Target frame of the link."),
			DefaultValue("")
		]
		public string Target
		{
			get {return _target;}
			set {_target = value;}
		}

		/// <summary>
		/// Gets or sets the unique key identifies the node.
		/// </summary>
		[
			Browsable(false),
			DefaultValue("")
		]
		public string Key
		{
			get {return _key;}
			set {_key = value;}
		}

		/// <summary>
		/// Gets or sets the parent key to identify the parent node.
		/// </summary>
		[
			Browsable(false),
			DefaultValue("")
		]
		public string ParentKey
		{
			get {return _parentKey;}
			set {_parentKey = value;}
		}
	}
}
