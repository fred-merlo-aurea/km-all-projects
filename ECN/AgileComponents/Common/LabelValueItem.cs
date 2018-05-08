// Active WebControls v3.0
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

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents an item used with <see cref="LabelValueCollection"/>.
	/// </summary>
	[
	//ControlBuilderAttribute(typeof(DateCollectionItemControlBuilder)),
	TypeConverter(typeof(ExpandableObjectConverter))
	]
	[Serializable]
	public class LabelValueCollectionItem 
	{
		/// <summary>
		/// The Date used as item.
		/// </summary>
		private string _label, _value;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public LabelValueCollectionItem() 
		{
			_label = string.Empty;
			_value = string.Empty;
		}

		/// <summary>
		/// Create a DateCollectionItem specifying the Date (must be a DateTime object).
		/// </summary>
		/// <param name="label">The label.</param>
		/// <param name="svalue">The value.</param>
		public LabelValueCollectionItem(string label, string svalue) 
		{
			_label = label;
			_value = svalue;
		}

		/// <summary>
		/// Gets or sets the label.
		/// </summary>
		[
		Bindable(true),
		]
		public string Label
		{
			get
			{
				return _label;
			}
			set 
			{ 
				_label = value;
			}
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		[
		Bindable(true),
		]
		public string Value
		{
			get
			{
				return _value;
			}
			set 
			{ 
				_value = value;
			}
		}

	}
}
