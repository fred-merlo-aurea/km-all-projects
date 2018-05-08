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
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using AttributeCollection = System.Web.UI.AttributeCollection;
using System.Drawing;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents an item used with <see cref="DateStyleCollection"/>.
	/// </summary>
	[
	ControlBuilderAttribute(typeof(DateStyleCollectionItemControlBuilder)),
	TypeConverterAttribute(typeof(ExpandableObjectConverter))
	]
	public class DateStyleCollectionItem 
	{
		/// <summary>
		/// Date.
		/// </summary>
		private DateTime _date;

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
		public DateStyleCollectionItem() 
		{
			_date = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day);
			_foreColor = System.Drawing.Color.Empty;
			_backColor = System.Drawing.Color.Empty;
			_backgroundImage = "";
		}

		/// <summary>
		/// Create a DateStyleCollectionItem specifying the Date (must be a DateTime object).
		/// </summary>
		/// <param name="date">The date.</param>
		public DateStyleCollectionItem(DateTime date) 
		{
            _date = date;
			_foreColor = System.Drawing.Color.Empty;
			_backColor = System.Drawing.Color.Empty;
			_backgroundImage = "";
		}

		/// <summary>
		/// Gets or sets the date.
		/// </summary>
		public DateTime Date
		{
			get {return _date;}
			set 
			{
				if (value != DateTime.MinValue)
					_date = value;
			}
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
	}
}
