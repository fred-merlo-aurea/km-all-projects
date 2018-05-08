using System;
using System.Xml.Serialization;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;

namespace ActiveUp.WebControls
{
	
	#region XmlSubMenu

	/// <summary>
	/// Represents a <see cref="XmlSubMenu"/> object.
	/// </summary>
	[
	System.Xml.Serialization.XmlTypeAttribute()
	]
	public class XmlSubMenu
	{
		#region Fields

		private ArrayList _items = new ArrayList();
		private XmlMenuStyle _menuStyle = new XmlMenuStyle();
		private XmlMenuItemStyle _menuItemStyle = new XmlMenuItemStyle();

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlSubMenu"/> class.
		/// </summary>
		public XmlSubMenu()
		{
	
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the menu items.
		/// </summary>
		/// <value>The menu items.</value>
		[System.Xml.Serialization.XmlArray("items")]
		[System.Xml.Serialization.XmlArrayItem("item",typeof(XmlMenuItem))]
		public ArrayList MenuItems
		{
			get
			{
				return _items;
			}

			set
			{
				_items = value;
			}
		}

		/// <summary>
		/// Gets or sets the style menu.
		/// </summary>
		/// <value>The style menu.</value>
		[
		System.Xml.Serialization.XmlElement("stylemenu",typeof(XmlMenuStyle)),
		Browsable(true)
		]
		public XmlMenuStyle StyleMenu
		{
			get {return _menuStyle;}
			set {_menuStyle = value;}
		}

		/// <summary>
		/// Gets or sets the style items.
		/// </summary>
		/// <value>The style items.</value>
		[
		System.Xml.Serialization.XmlElement("styleitems",typeof(XmlMenuItemStyle)),
		Browsable(true)
		]
		public XmlMenuItemStyle StyleItems
		{
			get {return _menuItemStyle;}
			set {_menuItemStyle = value;}
		}

		#endregion
	}

	#endregion
}
