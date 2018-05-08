using System;
using System.Xml.Serialization;
using System.Collections;

namespace ActiveUp.WebControls
{
	#region XmlMenuItemCollection

	/// <summary>
	/// Represents a <see cref="XmlMenuItemCollection"/> object.
	/// </summary>
	public class XmlMenuItemCollection
	{
		#region Fields

		private ArrayList _menuItems = new ArrayList();
        
		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlMenuItemCollection"/> class.
		/// </summary>
		public XmlMenuItemCollection()
		{
			
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the menu items.
		/// </summary>
		/// <value>The menu items.</value>
		[System.Xml.Serialization.XmlArray("menuitems")]
		[System.Xml.Serialization.XmlArrayItem("menuitem",typeof(XmlMenuItem))]
		public ArrayList MenuItems
		{
			get
			{
				return _menuItems;
			}

			set
			{
				_menuItems = value;
			}
		}

		#endregion

	}

	#endregion
}
