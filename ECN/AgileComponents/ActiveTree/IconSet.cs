using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web;


namespace ActiveUp.WebControls
{
	/// <summary>
	/// The icons necessary to display the treeview.
	/// </summary>
	[
	 	TypeConverterAttribute(typeof(ExpandableObjectConverter)),
		Description("Icons used to display the treeview."),
		Serializable
	]
	public class IconSet
	{
		private string _expanded;
		private string _collapsed;
		private string _orphan;
		private string _last;
		private string _default;
		private string _connection;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public IconSet()
		{
			/*Assembly asm = Assembly.GetExecutingAssembly();

			_expanded = new Icon(asm.GetManifestResourceStream("ActiveTreeView.Icons.co.gif"),"ActiveTreeView.Icons.co.gif");
			_collapsed = new Icon(asm.GetManifestResourceStream("ActiveTreeView.Icons.ex.gif"),"ActiveTreeView.Icons.ex.gif");
            _orphan = new Icon(asm.GetManifestResourceStream("ActiveTreeView.Icons.or.gif"),"ActiveTreeView.Icons.or.gif");
			_default = new Icon(asm.GetManifestResourceStream("ActiveTreeView.Icons.de.gif"),"ActiveTreeView.Icons.de.gif");
			_last = new Icon(asm.GetManifestResourceStream("ActiveTreeView.Icons.li.gif"),"ActiveTreeView.Icons.li.gif");
			_connection = new Icon(asm.GetManifestResourceStream("ActiveTreeView.Icons.cn.gif"),"ActiveTreeView.Icons.cn.gif");*/
#if (!FX1_1)
            _expanded = string.Empty;
            _collapsed = string.Empty;
            _orphan = string.Empty;
            _default = string.Empty;
            _last = string.Empty;
            _connection = string.Empty;
#else
			_expanded = @"minus.gif";
			_collapsed = @"plus.gif";
			_orphan = @"spacer.gif";
			_default = @"folder.gif";
			_last = @"spacer.gif";
			_connection = @"spacer.gif";
#endif


		}
	
		/// <summary>
		/// Gets or sets the image path for the expanded element.
		/// </summary>
		[
			NotifyParentPropertyAttribute(true),
			Description("Image path for the expanded element.")
		]
		public string Expanded
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
		/// Gets or sets the image path for the collapsed element.
		/// </summary>
		[
			NotifyParentPropertyAttribute(true),
			Description("Image path for the collapsed element.")
		]
		public string Collapsed
		{
			get
			{	
				return _collapsed;
			}

			set
			{
				_collapsed = value;
			}
		}

		/// <summary>
		/// Gets or sets the image path for the connection element.
		/// </summary>
		[
			NotifyParentPropertyAttribute(true),
			Description("Image path for the connection element.")
		]
		public string Connection
		{
			get
			{	
				return _connection;
			}

			set
			{
				_connection = value;
			}
		}

		/// <summary>
		/// Gets or sets the image path for the default element item.
		/// </summary>
		[
			NotifyParentPropertyAttribute(true),
			Description("Image path for the default element item.")
		]
		public string Default
		{
			get
			{	
				return _default;
			}

			set
			{
				_default = value;
			}
		}

		/// <summary>
		/// Gets or sets the image path for the orphan element.
		/// </summary>
		[
			NotifyParentPropertyAttribute(true),
			Description("Image path for the orphan element.")
		]
		public string Orphan
		{
			get
			{	
				return _orphan;
			}

			set
			{
				_orphan = value;
			}
		}

		/// <summary>
		/// Gets or sets the image path for the last element.
		/// </summary>
		[
			NotifyParentPropertyAttribute(true),
			Description("Image path for the last element.")
		]
		public string Last
		{
			get
			{	
				return _last;
			}

			set
			{
				_last = value;	
			}
		}
	}
}
