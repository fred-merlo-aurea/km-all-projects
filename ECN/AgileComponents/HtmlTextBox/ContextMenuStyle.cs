// Active Calendar v2.0
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents the short style for client-side manipulator of a <see cref="ContextMenuStyle"/>.
	/// </summary>
	[TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[ToolboxItemAttribute(false)]
	public class ContextMenuStyle : Component
	{
		/// <summary>
		/// The foreground color of the element.
		/// </summary>
		private System.Drawing.Color _foreColor;

		/// <summary>
		/// The foreground color rollover of the element.
		/// </summary>
		private System.Drawing.Color _foreColorRollOver;

		/// <summary>
		/// The background color of the element.
		/// </summary>
		private System.Drawing.Color _backColor;

		/// <summary>
		/// The background color rollover of the element.
		/// </summary>
		private System.Drawing.Color _backColorRollOver;

		/// <summary>
		/// The border color of the element.
		/// </summary>
		private System.Drawing.Color _borderColor;

		/// <summary>
		/// The border width of the element.
		/// </summary>
		private Unit _borderWidth;

		/// <summary>
		/// The border style of the element.
		/// </summary>
		private BorderStyle _borderStyle;

		/// <summary>
		/// The CSS class.
		/// </summary>
		private string _cssClass;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public ContextMenuStyle()
		{
			_foreColor = Color.FromArgb(0x40,0x40,0x40);
			_foreColorRollOver = Color.FromArgb(0x40,0x40,0x40);
			_backColor = Color.FromArgb(0xE0,0xE0,0xE0);
			_backColorRollOver = Color.FromArgb(0xB6,0xBD,0xD2);
			_borderColor = Color.FromArgb(0xDD,0xDD,0xDD);
			_borderWidth = Unit.Parse("2px");
			_borderStyle = BorderStyle.Outset;
			
		}

		/// <summary>
		/// Gets or sets the foreground color.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the foreground color."),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		DefaultValueAttribute(typeof(Color), "#404040"),
		NotifyParentPropertyAttribute(true)
		]
		public System.Drawing.Color ForeColor
		{
			get
			{
				return _foreColor;
			}
			set
			{
				_foreColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the foreground color.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the foreground color rollover."),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		DefaultValueAttribute(typeof(Color), "#404040"),
		NotifyParentPropertyAttribute(true)
		]
		public System.Drawing.Color ForeColorRollOver
		{
			get
			{
				return _foreColorRollOver;
			}
			set
			{
				_foreColorRollOver = value;
			}
		}

		/// <summary>
		/// Gets or sets the background color.
		/// </summary>
		[	
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the background color."),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		DefaultValueAttribute(typeof(Color), "#E0E0E0"),
		NotifyParentPropertyAttribute(true)
		]
		public System.Drawing.Color BackColor
		{
			get
			{
				return _backColor;
			}
			set
			{
				_backColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the background color rollover.
		/// </summary>
		[	
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the background color rollover."),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		DefaultValueAttribute(typeof(Color), "#B6BDD2"),
		NotifyParentPropertyAttribute(true)
		]
		public System.Drawing.Color BackColorRollOver
		{
			get
			{
				return _backColorRollOver;
			}
			set
			{
				_backColorRollOver = value;
			}
		}


		/// <summary>
		/// Gets or sets the border color.
		/// </summary>
		[	
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the border color."),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		DefaultValueAttribute(typeof(Color), "#DDDDDD"),
		NotifyParentPropertyAttribute(true)
		]
		public System.Drawing.Color BorderColor
		{
			get
			{
				return _borderColor;
			}
			set
			{
				_borderColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the border width.
		/// </summary>
		[	
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the border width."),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.UnitConverter)),
		DefaultValueAttribute(typeof(Unit), "2px"),
		NotifyParentPropertyAttribute(true)
		]
		public Unit BorderWidth
		{
			get
			{
				return _borderWidth;
			}
			set
			{
				_borderWidth = value;
			}
		}

		/// <summary>
		/// Gets or sets the border style.
		/// </summary>
		[	
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the border style."),
		DefaultValueAttribute(typeof(BorderStyle), "Outset"),
		NotifyParentPropertyAttribute(true)
		]
		public BorderStyle BorderStyle
		{
			get
			{
				return _borderStyle;
			}
			set
			{
				_borderStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the CSS class
		/// </summary>
		[	
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the CSS class."),
		DefaultValueAttribute(""),
		NotifyParentPropertyAttribute(true)
		]
		public string CssClass
		{
			get 
			{
				return _cssClass;
			}

			set
			{
				_cssClass = value;
			}
		}

		/// <summary>
		/// Adds HTML attributes and styles that need to be rendered to the specified System.Web.UI.HtmlTextWriter.
		/// </summary>
		/// <param name="writer">A System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client.</param>
		public void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (_cssClass != null && _cssClass != string.Empty)
				writer.AddAttribute(HtmlTextWriterAttribute.Class, _cssClass);

			if (_foreColor != Color.Empty)
				writer.AddStyleAttribute(HtmlTextWriterStyle.Color, Utils.Color2Hex(_foreColor));
			if (_backColor != Color.Empty)
				writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(_backColor));

			if (_borderColor != Color.Empty)
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(_borderColor));
			if (_borderWidth != Unit.Empty)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle,_borderWidth.ToString());
				if (_borderStyle != BorderStyle.NotSet)
					writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle,_borderStyle.ToString());
				else
					writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle,"solid");
			}
		}
	}
}
