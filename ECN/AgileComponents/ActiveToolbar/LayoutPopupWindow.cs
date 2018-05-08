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
	#region class LayoutPopupWindow

	/// <summary>
	/// Contains the layout for the window of the popup.
	/// </summary>
	[
		TypeConverterAttribute(typeof(ExpandableObjectConverter)),
		ToolboxItem(false),
		Serializable 

	]
	public class LayoutPopupWindow : Control
	{
		#region Constructor

		/// <summary>
		/// Default constructor.
		/// </summary>
		public LayoutPopupWindow()
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
			Description("Background color of the window."),
			TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
			DefaultValueAttribute(typeof(Color), "#DDDDDD"),
			NotifyParentProperty(true)
		]
		public Color BackColor
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
			Description("Border color of the window."),
			TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
			DefaultValueAttribute(typeof(Color), "#DDDDDD"),
			NotifyParentProperty(true)
		]
		public Color BorderColor
		{
			get 
			{
				object borderColor = ViewState["_borderColor"];
				if (borderColor != null)
					return (Color)borderColor;
				else 
					return Color.FromArgb(0xDD,0xDD,0xDD);
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
			Description("Border style of the window."),	
			DefaultValueAttribute(BorderStyle.Outset),
			NotifyParentProperty(true)
		]
		public BorderStyle BorderStyle
		{
			get
			{
				object borderStyle = ViewState["_borderStyle"];
				if (borderStyle != null)
					return (BorderStyle)borderStyle;
				else 
					return BorderStyle.Outset;
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
			Description("Border width of the window."),
			DefaultValueAttribute(typeof(Unit), "2px"),
			NotifyParentProperty(true)
		]
		public Unit BorderWidth
		{
			get 
			{
				object borderWidth = ViewState["_borderWidth"];
				if (borderWidth != null)
					return (Unit)borderWidth;
				else
					return new Unit(2);
			}
			
			set 
			{
				if (value.Type == UnitType.Percentage || value.Value < 0.0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				ViewState["_borderWidth"] = value;
			}
		}

		#endregion
	}

	#endregion
}
