using System;
using System.Web.UI;
using System.ComponentModel;
using System.Collections;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	#region class TabPage

	/// <summary>
	/// Represents a <see cref="TabPage"/>.
	/// </summary>
	[
	TypeConverterAttribute(typeof(ExpandableObjectConverter)),
	Serializable 
	]
	public class TabPage : IParserAccessor, IStateManager
	{
		private StateManagerImpl stateManagerImpl = new StateManagerImpl();

		#region Constructor

		/// <summary>
		/// The default constructor.
		/// </summary>
		public TabPage() 
		{
			Selected = false;
			Text = string.Empty;
			Value = string.Empty;
		}

		/// <summary>
		/// Create a <see cref="TabPage"/> by specifying the text and the value.
		/// </summary>
		/// <param name="text">Text displayed.</param>
		/// <param name="value">Value associated to the text.</param>
		public TabPage(string text, string val)
		{
			Selected = false;
			Text = text;
			Value = val;
		}
		#endregion
		
		#region Properties

		/// <summary>
		/// Gets or sets if the <see cref="TabPage"/> is selected or not.
		/// </summary>
		[
		Bindable(true),
		DefaultValue(false),
		Description("Indicates if the tabPage is selected or not.")
		]
		public bool Selected
		{
			get
			{
				object selected = ViewState["_tabPageSelected"];
				if (selected != null)
					return (bool)selected;
				else
					return false;
			}

			set
			{
				ViewState["_tabPageSelected"] = value;
			}
		}

		/// <summary>
		/// Gets or sets if the <see cref="TabPage"/> is locked or not.
		/// </summary>
		[
		Bindable(true),
		DefaultValue(false),
		Description("Indicates if the tabPage is selected or not.")
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
				object text = ViewState["_tabPageText"];
				if (text != null)
					return (string)text;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_tabPageText"] = value;
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
				object val = ViewState["_tabPageValue"];
				if (val != null)
					return (string)val;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_tabPageValue"] = value;
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
		/// Notifies the <see cref="TabPage"/> that an element, either XML or HTML, was parsed, and adds the element to the server control's ControlCollection object.
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
