using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AttributeCollection = System.Web.UI.AttributeCollection;
using System.Drawing;

namespace ActiveUp.WebControls
{
	#region class LayoutPopupContent

	/// <summary>
	/// Contains the layout for the contents of the popup.
	/// </summary>
	[
		TypeConverterAttribute(typeof(ExpandableObjectConverter)),
		ToolboxItem(false),
		Serializable 
	]
	public class LayoutPopupContent : Style
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public LayoutPopupContent()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the background color.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("Background color of the contents."),
			TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
			DefaultValueAttribute(typeof(Color), "#E0E0E0"),
			NotifyParentProperty(true)
		]
		public new Color BackColor
		{
			get 
			{
				object backColor = ViewState["_backColor"];
				if (backColor != null)
					return (Color)backColor;
				else 
					return Color.FromArgb(0xDD,0xDD,0xDD);
			}

			set 
			{
				ViewState["_backColor"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the border color.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("Border color of the contents."),
			TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
			DefaultValueAttribute(typeof(Color), ""),
			NotifyParentProperty(true)
		]
		public new Color BorderColor
		{
			get
			{
				object borderColor = ViewState["_borderColor"];
				if (borderColor != null)
					return (Color)borderColor;
				else 
					return Color.Empty;
			}

			set
			{
				ViewState["_borderColor"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the border style.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("Border style of the contents."),	
			DefaultValueAttribute(BorderStyle.NotSet),
			NotifyParentProperty(true)
		]
		public new BorderStyle BorderStyle
		{
			get
			{
				object borderStyle = ViewState["_borderStyle"];
				if (borderStyle != null)
					return (BorderStyle)borderStyle;
				else 
					return BorderStyle.NotSet;
			}

			set
			{
				ViewState["_borderStyle"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the border width.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("Border width of the contents."),
			DefaultValueAttribute(typeof(Unit), ""),
			NotifyParentProperty(true)
		]
		public new Unit BorderWidth
		{
			get
			{
				object borderWidth = ViewState["_borderWidth"];
				if (borderWidth != null)
					return (Unit)borderWidth;
				else 
					return Unit.Empty;
			}

			set
			{
				ViewState["_borderWidth"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the scroll bar color.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("Scroll bar color of the contents."),
			TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
			DefaultValueAttribute(typeof(Color), "#808080"),
			NotifyParentProperty(true)
		]
		public Color ScrollBarColor
		{
			get 
			{
				object scrollBarColor = ViewState["_scrollBarColor"];
				if (scrollBarColor != null)
					return (Color)scrollBarColor;
				else 
					return Color.FromArgb(0x80,0x80,0x80);
			}

			set 
			{
				ViewState["_scrollBarColor"] = value;
			}
		}

		#endregion
	
	}

	#endregion
}
