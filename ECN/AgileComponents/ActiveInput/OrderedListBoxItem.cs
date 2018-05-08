// ActiveInput
// Copyright (c) 2005 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Drawing;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	#region class OrderedListBoxItem

	/// <summary>
	/// Represents a <see cref="OrderedListBoxItem"/>.
	/// </summary>
	[
	TypeConverterAttribute(typeof(ExpandableObjectConverter)),
	Serializable 
	]
	public class OrderedListBoxItem : IParserAccessor, IStateManager
	{
		private StateManagerImpl stateManagerImpl = new StateManagerImpl();

		#region Constructor

		/// <summary>
		/// The default constructor.
		/// </summary>
		public OrderedListBoxItem() 
		{
			Selected = false;
			Text = string.Empty;
			Value = string.Empty;
		}

		/// <summary>
		/// Create a <see cref="OrderedListBoxItem"/> by specifying the text and the value.
		/// </summary>
		/// <param name="text">Text displayed.</param>
		/// <param name="val">Value associated to the text.</param>
		public OrderedListBoxItem(string text, string val)
		{
			Selected = false;
			Text = text;
			Value = val;
		}
		#endregion
		
		#region Properties

		/// <summary>
		/// Gets or sets if the <see cref="OrderedListBoxItem"/> is selected or not.
		/// </summary>
		[
		Bindable(true),
		DefaultValue(false),
		Description("Indicates if the ToolItem is selected or not.")
		]
		public bool Selected
		{
			get
			{
				object selected = ViewState["_toolItemSelected"];
				if (selected != null)
					return (bool)selected;
				else
					return false;
			}

			set
			{
				ViewState["_toolItemSelected"] = value;
			}
		}

		/// <summary>
		/// Gets or sets if the <see cref="OrderedListBoxItem"/> is locked or not.
		/// </summary>
		[
		Bindable(true),
		DefaultValue(false),
		Description("Indicates if the ToolItem is selected or not.")
		]
		public bool Locked
		{
			get
			{
				object locked = ViewState["_locked"];
				if (locked != null)
					return (bool)locked;
				else
					return false;
			}

			set
			{
				ViewState["_locked"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		[
		Bindable(true),
		DefaultValue(""),
		Description("The text of the item.")
		]
		public string Text
		{
			get
			{
				object text = ViewState["_toolItemText"];
				if (text != null)
					return (string)text;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_toolItemText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		[
		Bindable(true),
		DefaultValue(""),
		Description("Value.")
		]
		public string Value
		{
			get
			{
				object val = ViewState["_toolItemValue"];
				if (val != null)
					return (string)val;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_toolItemValue"] = value;
			}

		}

		/// <summary>
		/// Gets or sets the style.
		/// </summary>
		[
		Bindable(true),
		DefaultValue(""),
		Description("The style of the item.")
		]
		public string Style
		{
			get
			{
				object style = ViewState["_xstyle"];
				if (style != null)
					return (string)style;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_xstyle"] = value;
			}
		}

		/// <summary>
		/// Gets the view state of the item.
		/// </summary>
		/// <value>The view state of the item.</value>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		protected StateBag ViewState 
		{
			get 
			{
				return stateManagerImpl.ViewState;
			}
		}

		#endregion

		#region Methods

		internal void SetDirty() 
		{
			stateManagerImpl.SetDirty();
		}

		#endregion

		#region Interface IParserAccessor

		/// <summary>
		/// Notifies the <see cref="OrderedListBoxItem"/> that an element, either XML or HTML, was parsed, and adds the element to the server control's ControlCollection object.
		/// </summary>
		/// <param name="obj">An Object that represents the parsed element.</param>
		void IParserAccessor.AddParsedSubObject(object obj)
		{
			stateManagerImpl.AddParsedSubObject(obj, (text) => Text = text);
		}

		#endregion

		#region ViewState

		bool IStateManager.IsTrackingViewState 
		{
			get 
			{
				return stateManagerImpl.IsTrackingViewState;
			}
		}

		void IStateManager.LoadViewState(object savedState)
		{
			stateManagerImpl.LoadViewState(savedState);
		}

		object IStateManager.SaveViewState()
		{
			return stateManagerImpl.SaveViewState();
		}

		void IStateManager.TrackViewState()
		{
			stateManagerImpl.TrackViewState();
		}

		#endregion
	}

	#endregion
}
