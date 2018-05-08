using System;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections;
using System.Web.UI.WebControls;

namespace ActiveUp.WebControls
{
	#region XmlMenu

	/// <summary>
	/// Represents a <see cref="XmlMenu"/> object.
	/// </summary>
	[
		System.Xml.Serialization.XmlRootAttribute("activemenu", IsNullable=false),
		Serializable
	]
    public class XmlMenu
	{
		#region Fields

		private string _imageArrowSubItemMenu;
		private XmlMenuStyle _styleMenu;
		private XmlMenuItemStyle _styleMenuItems;
		private ArrayList _menuItems = new ArrayList();

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlMenu"/> class.
		/// </summary>
		public XmlMenu()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the image arrow sub item menu.
		/// </summary>
		/// <value>The image arrow sub item menu.</value>
		[
		System.Xml.Serialization.XmlElement("imagearrowsubitemmenu",DataType="string"),
		Browsable(true)
		]
		public string ImageArrowSubItemMenu
		{
			get {return _imageArrowSubItemMenu;}
			set {_imageArrowSubItemMenu = value;}
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
			get {return _styleMenu;}
			set {_styleMenu = value;}
		}

		/// <summary>
		/// Gets or sets the style menu items.
		/// </summary>
		/// <value>The style menu items.</value>
		[
		System.Xml.Serialization.XmlElement("stylemenuitems",typeof(XmlMenuItemStyle)),
		Browsable(true)
		]
		public XmlMenuItemStyle StyleMenuItems
		{
			get {return _styleMenuItems;}
			set {_styleMenuItems = value;}
		}

		/// <summary>
		/// Gets or sets the menu items.
		/// </summary>
		/// <value>The menu items.</value>
		[System.Xml.Serialization.XmlArray("menuitems")]
		[System.Xml.Serialization.XmlArrayItem("menuitem",typeof(XmlMenuItem))]
		public ArrayList MenuItems
		{
			get {return _menuItems;}
			set {_menuItems = value;}
		} 

		#endregion
	}

	#endregion
}
