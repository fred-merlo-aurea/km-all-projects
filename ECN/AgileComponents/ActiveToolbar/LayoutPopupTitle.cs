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
	#region class LayoutPopupTitle

	/// <summary>
	/// Contains the layout for the title of the popup.
	/// </summary>
	[
		TypeConverterAttribute(typeof(ExpandableObjectConverter)),
		ToolboxItem(false),
		Serializable 
	]
	public class LayoutPopupTitle : Style
	{
		#region Constrcutors

		/// <summary>
		/// Default constrcutor.
		/// </summary>
		public LayoutPopupTitle()
		{
			BackGradientFirstColor = Color.FromArgb(0x0A,0x24,0x6A);
			BackGradientLastColor = Color.FromArgb(0xA6,0xCA,0xF0);
			Font.Bold = true;
			Font.Name = "Verdana";
			Font.Size = FontUnit.Parse("10pt");
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the background color.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("Background color of the title."),
			TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
			DefaultValueAttribute(typeof(Color), "#4F6DA5"),
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
					return Color.FromArgb(0x4F,0x6D,0xA5);
			}

			set 
			{
				ViewState["_backColor"] = value;
			}
		}

		/// <summary>
		/// Gets of sets the first gradient color for the title.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("First gradient color for the title."),
			TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
			DefaultValueAttribute(typeof(Color),"#0A246A"),
			NotifyParentProperty(true)
		]
		public Color BackGradientFirstColor
		{
			get
			{
				object backGradientFirstColor = ViewState["_backGradientFirstColor"];
				if (backGradientFirstColor != null)
					return (Color)backGradientFirstColor;
				else
					return Color.Empty;
			}

			set
			{
				ViewState["_backGradientFirstColor"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the last color for the title.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Last gradient color for the title."),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		DefaultValueAttribute(typeof(Color), "#A6CAF0"),
		NotifyParentProperty(true)
		]
		public Color BackGradientLastColor
		{
			get
			{
				object backGradientLastColor = ViewState["_backGradientLastColor"];
				if (backGradientLastColor != null)
					return (Color)backGradientLastColor;
				else
					return Color.Empty;
			}

			set
			{
				ViewState["_backGradientLastColor"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the border color.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("Border color of the title."),
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
		/// Gets or sets the inactive background color.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("Inactive background color of the title."),
			TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
			DefaultValueAttribute(typeof(Color), "#808080"),
			NotifyParentProperty(true)
		]
		public Color InactiveBackColor
		{
			get
			{
				object inactiveBackColor = ViewState["_inactiveBackColor"];
				if (inactiveBackColor != null)
					return (Color)inactiveBackColor;
				else 
					return Color.FromArgb(0x80,0x80,0x80);
			}

			set
			{
				ViewState["_inactiveBackColor"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the border style.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("Border style of the title."),	
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
			Description("Border width of the title."),
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
		/// Gets or sets the text fore color.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("Text fore color of the title."),
			DefaultValueAttribute(typeof(Color), "White"),
			NotifyParentProperty(true)
		]
		public new Color ForeColor
		{
			get
			{
				object foreColor = ViewState["_foreColor"];
				if (foreColor != null)
					return (Color)foreColor;
				else 
					return Color.White;
			}

			set
			{
				ViewState["_foreColor"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the font title of the popup.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("Gets or sets the font title of the popup."),
			DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
			DefaultValue(typeof(FontInfo),"Verdana, 10pt"),
			NotifyParentProperty(true)
		]
		public new FontInfo Font
		{
			get {return base.Font;}
		}

		#endregion
	}

	#endregion
}
