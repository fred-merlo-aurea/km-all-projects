using System;
using System.Xml.Serialization;
using System.Collections;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms.Design;

namespace ActiveUp.WebControls
{
	#region class Node

	/// <summary>
	/// Represents a node in case of using xml file.
	/// </summary>
	[
		System.Xml.Serialization.XmlTypeAttribute()
	]
	public class Node
	{

		#region Variables

		/// <summary>
		/// Key of the node (must be unique).
		/// </summary>
		private string _key;

		/// <summary>
		/// Key of the parent node, 0 indicate that it's root.
		/// </summary>
		private string _parentKey;

		/// <summary>
		/// Text displayed.
		/// </summary>
		private string _label;

		/// <summary>
		/// Link of the node.
		/// </summary>
		private string _link;

		/// <summary>
		/// Target of the link.
		/// </summary>
		private string _target;

		/// <summary>
		/// Indicates if it must be expanded or not.
		/// </summary>
		private bool _expanded;

		/// <summary>
		/// Indicates if the node is selected (checkbox)
		/// </summary>
		private bool _selected;

		/// <summary>
		/// The icon of the node;
		/// </summary>
		private string _icon;

		#endregion

		#region Constructor

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Node()
		{
			_key = "0";
			_parentKey = "0";
			_label = "";
			_link = "";
			_target = "";
			_expanded = false;
			_selected = false;
			_icon = string.Empty;
		}

		/// <summary>
		/// Create a node from the key.
		/// </summary>
		/// <param name="key">Unique key identifies the node.</param>
		public Node(string key)
		{
			_key = key;
			_parentKey = "0";
			_label = "";
			_link = "";
			_target = "";
			_expanded = false;
			_selected = false;
			_icon = string.Empty;
		}

		/// <summary>
		/// Create a node from the key and the label.
		/// </summary>
		/// <param name="key">Unique key identifies the node.</param>
		/// <param name="label">Text displayed.</param>
		public Node(string key,string label)
		{
			_key = key;
			_parentKey = "0";
			_label = label;
			_link = "";
			_target = "";
			_expanded = false;
			_selected = false;
			_icon = string.Empty;
		}

		/// <summary>
		/// Create a node from the key, label and the link.
		/// </summary>
		/// <param name="key">Unique key identifies the node.</param>
		/// <param name="label">Text displayed.</param>
		/// <param name="link">Link of the node.</param>
		public Node(string key,string label,string link)
		{
			_key = key;
			_parentKey = "0";
			_label = label;
			_link = link;
			_target = "";
			_expanded = false;
			_selected = false;
			_icon = string.Empty;
		}

		/// <summary>
		/// Create a node from the key, parent key, label and link.
		/// </summary>
		/// <param name="key">Uniqe key identifies the node.</param>
		/// <param name="parentKey">Parent key to identify the parent node.</param>
		/// <param name="label">Text displayed.</param>
		/// <param name="link">Link of the node.</param>
		public Node(string key,string parentKey,string label,string link)
		{
			_key = key;
			_parentKey = parentKey;
			_label = label;
			_link = link;
			_target = "";
			_expanded = false;
			_selected = false;
			_icon = string.Empty;
		}

		#endregion

		#region Properties
		
		/// <summary>
		/// Gets or sets the unqiue key.
		/// </summary>
		[
			System.Xml.Serialization.XmlElement("key",DataType="string"),
			Browsable(true)
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
			System.Xml.Serialization.XmlElement("parentkey",DataType="string"),
			Browsable(false)
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
		/// Gets or sets the label to display.
		/// </summary>
		[
			System.Xml.Serialization.XmlElement("label",DataType="string"),
			Description("Label of the current node.")
		]
		public string Label
		{
			get
			{
				return _label;
			}

			set
			{
				_label = value;
			}
		}

		/// <summary>
		/// Gets or sets the link.
		/// </summary>
		[
			System.Xml.Serialization.XmlElement("link",DataType="string"),
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
		/// Gets or sets the target of the link. 
		/// </summary>
		[
			System.Xml.Serialization.XmlElement("target",DataType="string"),
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
		/// Gets or sets if the node must be expanded or not. It has effect only if it has elements.
		/// </summary>
		[
			System.Xml.Serialization.XmlElement("expanded",DataType="boolean"),
			Description("Expands the current node, it has effect only if it has elements")
		]
		public bool Expanded
		{
			get
			{
				return _expanded;
			}

			set
			{
				_expanded = value;
			}
		}

		/// <summary>
		/// Gets or sets if the node must be selected or not.
		/// </summary>
		[
			System.Xml.Serialization.XmlElement("selected",DataType="boolean"),
			Description("Selects the current node.")
		]
		public bool Selected
		{
			get
			{
				return _selected;
			}

			set
			{
				_selected = value;
			}
		}

		/// <summary>
		/// Gets or sets the icons.
		/// </summary>
		[
			System.Xml.Serialization.XmlElement("icon",DataType="string"),
			Description("Icon of the current node.")
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

		#endregion
	}

	#endregion
}
