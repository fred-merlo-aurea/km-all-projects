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
	ControlBuilderAttribute(typeof(ToolButtonMenuItemControlBuilder)),
	TypeConverterAttribute(typeof(ExpandableObjectConverter)),
	Serializable 
	]
	public class ToolButtonMenuItem : IParserAccessor,IStateManager
	{
		private StateManagerImpl stateManagerImpl = new StateManagerImpl();

		#region Constructor


		/// <summary>
		/// Initializes a new instance of the <see cref="ToolButtonMenuItem"/> class.
		/// </summary>
		public ToolButtonMenuItem() 
		{
			_Init(string.Empty,string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolButtonMenuItem"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		public ToolButtonMenuItem(string text)
		{
			_Init(text,string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolButtonMenuItem"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="image">The image.</param>
		public ToolButtonMenuItem(string text,string image)
		{
			_Init(text,image);
		}

		private void _Init(string text,string image)
		{
			Text = text;
			Image = image;
		}
		
		#endregion
		
		#region Properties

		/// <summary>
		/// Gets or sets the Image.
		/// </summary>
		[
		Bindable(true),
		DefaultValue(""),
		Description("The Image of the item.")
		]
		public string Image
		{
			get
			{
				object image = ViewState["_image"];
				if (image != null)
					return (string)image;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_image"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text of the item.
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
				object text = ViewState["_text"];
				if (text != null)
					return (string)text;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_text"] = value;
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
		/// Notifies the <see cref="ToolButtonMenuItem"/> that an element, either XML or HTML, was parsed, and adds the element to the server control's ControlCollection object.
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
