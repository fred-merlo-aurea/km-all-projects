using System;
using System.Web.UI;
using System.Collections;
using System.ComponentModel;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	#region class ToolItem

	/// <summary>
	/// Represents a <see cref="ToolItem"/>.
	/// </summary>
	[
		ControlBuilderAttribute(typeof(ToolItemControlBuilder)),
		TypeConverterAttribute(typeof(ExpandableObjectConverter)),
		Serializable 
	]
	public class ToolItem : IParserAccessor, IStateManager
	{
		private StateManagerImpl stateManagerImpl = new StateManagerImpl();

		#region Constructor

		/// <summary>
		/// The default constructor.
		/// </summary>
		public ToolItem() 
		{
			Selected = false;
			Text = string.Empty;
			Value = string.Empty;
		}

		/// <summary>
		/// Create a <see cref="ToolItem"/> by specifying the text and the value.
		/// </summary>
		/// <param name="text">Text displayed.</param>
		/// <param name="val">The value.</param>
		public ToolItem(string text, string val)
		{
			Selected = false;
			Text = text;
			Value = val;
		}

		/// <summary>
		/// Create a <see cref="ToolItem"/> by specifying an embedded control.
		/// </summary>
		/// <param name="embeddedControl"></param>
		public ToolItem(System.Web.UI.WebControls.WebControl embeddedControl)
		{
			EmbeddedControl = embeddedControl;
		}


		#endregion
		
		#region Properties

		/// <summary>
		/// Gets or sets if the <see cref="ToolItem"/> is selected or not.
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
		/// Gets or sets the text.
		/// </summary>
		[
		Bindable(true),
		DefaultValue(""),
		Description("The text without HTML of the item.")
		]
		public string TextOnly
		{
			get
			{
				object text = ViewState["_toolItemTextOnly"];
				if (text != null)
					return (string)text;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_toolItemTextOnly"] = value;
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
		/// Gets or sets the embedde control.
		/// </summary>
		[
		Bindable(true),
		DefaultValue(""),
		Description("Embedded control."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public System.Web.UI.WebControls.WebControl EmbeddedControl
		{
			get
			{
				object embeddedControl = ViewState["_toolItemEmbeddedControl"];
				if (embeddedControl != null)
					return (System.Web.UI.WebControls.WebControl)embeddedControl;
				else
					return null;
			}

			set
			{
				ViewState["_toolItemEmbeddedControl"] = value;
			}
		}

		/// <summary>
		/// Gets the view state.
		/// </summary>
		/// <value>The view state.</value>
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
		/// Notifies the <see cref="ToolItem"/> that an element, either XML or HTML, was parsed, and adds the element to the server control's ControlCollection object.
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
