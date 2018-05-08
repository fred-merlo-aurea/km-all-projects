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
	/// Represents the short style for client-side manipulator of a <see cref="CalendarDayStyleShort"/>.
	/// </summary>
	[TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[ToolboxItemAttribute(false)]
	public class CalendarDayStyleShort : Component
	{
		/// <summary>
		/// The foreground color of the element.
		/// </summary>
		private System.Drawing.Color _foreColor;

		/// <summary>
		/// The background color of the element.
		/// </summary>
		private System.Drawing.Color _backColor;

		/// <summary>
		/// The background image of the element.
		/// </summary>
		private string _backgroundImage = "";

		/// <summary>
		/// The default constructor.
		/// </summary>
		public CalendarDayStyleShort()
		{
			_foreColor = System.Drawing.Color.Empty;
			_backColor = System.Drawing.Color.Empty;
			_backgroundImage = "";
		}

		/// <summary>
		/// Gets or sets the foreground color.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("Gets or sets the foreground color."),
			TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
			DefaultValueAttribute(typeof(Color), ""),
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
		/// Gets or sets the background color.
		/// </summary>
		[	Bindable(true),
			Category("Appearance"),
			Description("Gets or sets the background color."),
			TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
			DefaultValueAttribute(typeof(Color), ""),
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
		/// Gets or sets the background image.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("Gets or sets the background image."),
			DefaultValueAttribute(""),
			NotifyParentPropertyAttribute(true)
		]
		public string BackgroundImage
		{
			get
			{
				return _backgroundImage;
			}

			set
			{
				_backgroundImage = value;
			}
		}

		/// <summary>
		/// Adds HTML attributes and styles that need to be rendered to the specified System.Web.UI.HtmlTextWriter.
		/// </summary>
		/// <param name="writer">A System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client.</param>
		public void AddAttributesToRender(HtmlTextWriter writer)
		{
			AddAttributesToRender(writer,"");
		}

		/// <summary>
		/// Adds HTML attributes and styles that need to be rendered to the specified System.Web.UI.HtmlTextWriter.
		/// </summary>
		/// <param name="writer">A System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client.</param>
		/// <param name="imageDir">The directory where the images are located.</param>
		public void AddAttributesToRender(HtmlTextWriter writer,string imageDir)
		{
			writer.AddStyleAttribute(HtmlTextWriterStyle.Color, Utils.Color2Hex(_foreColor));
			writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(_backColor));

			if (_backgroundImage != null && _backgroundImage != string.Empty)
				writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + Utils.ConvertToImageDir(imageDir,_backgroundImage) + ")");
		}

		/// <summary>
		/// Gets a value indicating whether this instance is empty.
		/// </summary>
		/// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool IsEmpty 
		{
			get
			{
				return BackColor.IsEmpty && BackgroundImage.TrimEnd() == string.Empty && ForeColor.IsEmpty;
			}
		}

	}
}
