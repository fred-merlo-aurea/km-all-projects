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
	public class DateToolTipCollectionItem 
	{
		/// <summary>
		/// Date.
		/// </summary>
		private DateTime _date;
		
		/// <summary>
		/// Tool tip text.
		/// </summary>
		private string _toolTip;
				
		/// <summary>
		/// The default constructor.
		/// </summary>
		public DateToolTipCollectionItem() 
		{
			_date = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day);
			_toolTip = string.Empty;
		}

		/// <summary>
		/// Create a DateStyleCollectionItem specifying the Date (must be a DateTime object).
		/// </summary>
		/// <param name="date">The date.</param>
		public DateToolTipCollectionItem(DateTime date) 
		{
			_date = date;
			_toolTip = string.Empty;
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
		/// Gets or sets the tool tip text.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the tooltip text."),
		DefaultValueAttribute(""),
		NotifyParentPropertyAttribute(true)
		]
		public string ToolTip
		{
			get
			{
				return _toolTip;
			}

			set
			{
				_toolTip = value;
			}
		}
	}
}
