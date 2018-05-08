// ActiveToolbar 1.x
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
using System.Web.UI;
using System.Web.UI.WebControls;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{

    /// <summary>
    /// Base class for each item in the <see cref="Toolbar"/>.
    /// </summary>
    [
		TypeConverter(typeof(ExpandableObjectConverter)),
		Serializable,
		//ToolboxItem(false)
	]
    public abstract class ToolBase : CommonControlProperties, INamingContainer
    {
        private const string DropDownImageKey = "_dropDownImage";
        private const string IndentTextKey = "_indentText";
        private const string AutoPostBackKey = "_autoPostBack";
        private const string ChangeToSelectedTextKey = "_changeToSelectedText";
        private const string IndentTextDotValue = ". . . ";
        private const string WindowBorderStyleKey = "_windowBorderStyle";
        private const string WindowBorderWidthKey = "_windowBorderWidth";
        private const string ItemBorderColorKey = "_itemBorderColor";
        private const string ItemBorderColorRollOverKey = "_itemBorderColorRollOver";
        private const string ItemAlignKey = "_itemAlign";
        private const string BorderColorRollOverDefaultValue = "#316AC5";
        private const string BorderWidthDefaultValue = "1";
        private const string BorderStyleDefaultValue = "Solid";
        private const string BackColorItemsDefaultValue = "White";
        private const string BackColorItemsKey = "_backColorItems";
        private const string BorderWidthRollOverKey = "_borderWidthRollOver";
        private const string BackColorRollOverKey = "_backColorRollOver";
        private const string BackColorClickedKey = "_backColorClicked";
        private const string BackImageClickedKey = "_backImageClicked";
        private const string TextDefaultIdIfNullKey = "_text";

        private Color _borderColorRollOver = Color.Empty;
        private Unit _cellspacing = Unit.Empty;
        private Unit _cellpadding = Unit.Empty;

        /// <summary>
        /// 
        /// </summary>
        private string _clientScriptBlock;

		/// <summary>
		/// 
		/// </summary>
		private string _clientScriptKey;

		/// <summary>
		/// 
		/// </summary>
		private string _startupScriptBlock;

		/// <summary>
		/// 
		/// </summary>
		private string _startupScriptKey;

	    /// <summary>
	    /// Collection of items present in the dropdown.
	    /// </summary>
	    protected ToolItemCollection _items;


        /// <summary>
        /// Default constructor.
        /// </summary>
        public ToolBase() : base()
		{
			_clientScriptBlock = string.Empty;
			_clientScriptKey = string.Empty;
			_startupScriptBlock = string.Empty;
			_startupScriptKey = string.Empty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolBase"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolBase(string id) : base()
		{
			_clientScriptBlock = string.Empty;
			_clientScriptKey = string.Empty;
			_startupScriptBlock = string.Empty;
			_startupScriptKey = string.Empty;
			ID = id;
		}

		/*[
		Browsable(false),
		Bindable(false)
		]
		public Toolbar ParentToolbar
		{
			get
			{
				if (this.Parent != null && this.Parent.GetType().ToString() == "ActiveUp.WebControls.Toolbar")
					return (Toolbar)this.Parent;
				else
					return null;
			}
		}*/

		/// <summary>
		/// Javascript code to execture client-side when the tool is clicked.
		/// </summary>
		[
		Bindable(true), 
		Category("Event"), 
		DefaultValue(""),
		Description("The javascript to execute client-side when the tool is clicked.")
		] 
		public virtual string ClientSideClick
		{
			get
			{
				string _clientSideClick;
				_clientSideClick = ((string)ViewState["_clientSideClick"]);
				if (_clientSideClick != null)
					return _clientSideClick; 
				return string.Empty;
			}
			set
			{
				ViewState["_clientSideClick"] = value;
			}
		}

		public virtual string TextDefaultIdIfNull
		{
			get
			{
				var text = base.ViewState[TextDefaultIdIfNullKey] as string;
				if (text != null)
				{
					return text;
				}
				else
				{
					return this.ID.ToString();
				}
			}
			set
			{
				ViewState[TextDefaultIdIfNullKey] = value;
			}
		}

		/// <summary>
		/// Text to display with the tool.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("The text to display with the tool.")
		] 
		public virtual string Text
		{
			get
			{
				string _text;
				_text = ((string) base.ViewState["_text"]);
				if (_text != null)
					return _text; 
				return string.Empty;
			}
			set
			{
				ViewState["_text"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the key that will be used when registering the client-side script on the ASPX page.
		/// </summary>
		[Bindable(true),
		Browsable(false),
		Category("Appearance"), 
		DefaultValue("Gets or sets the key that will be used when registering the client-side script on the ASPX page.")]
		public string ClientScriptKey 
		{
			get
			{
				return _clientScriptKey;
			}

			set
			{
				_clientScriptKey = value;
			}
		}

		/// <summary>
		/// Gets or sets the client-side script block to register with the tool.
		/// </summary>
		[Bindable(true), 
		Browsable(false),
		Category("Appearance"), 
		DefaultValue("Gets or sets the client-side script block to register with the tool.")] 
		public string ClientScriptBlock 
		{
			get
			{
				return _clientScriptBlock;
			}

			set
			{
				_clientScriptBlock = value;
			}
		}

		/// <summary>
		/// Gets or sets the startup client-side script to register with the tool.
		/// </summary>
		[Bindable(true), 
		Browsable(false),
		Category("Appearance"), 
		DefaultValue("Gets or sets the startup client-side script to register with the tool.")]
		public string StartupScriptBlock
		{
			get
			{
				return _startupScriptBlock;
			}

			set
			{
				_startupScriptBlock = value;
			}
		}

		/// <summary>
		/// Gets or sets the key that will be used when registering the startup client-side script on the ASPX page.
		/// </summary>
		[Bindable(true), 
		Browsable(false),
		Category("Appearance"), 
		DefaultValue("Gets or sets the key that will be used when registering the startup client-side script on the ASPX page.")]
		public string StartupScriptKey
		{
			get
			{
				return _startupScriptKey;
			}

			set
			{
				_startupScriptKey = value;
			}
		}

		/// <summary>
		/// Indicates if you want to allow rollover.
		/// </summary>
		[
		Bindable(true),
		Browsable(true),
		Category("Behavior"),
		DefaultValue(true),
		Description("Allow rollover."),
		NotifyParentProperty(true)
		]
		public bool AllowRollOver
		{
			get
			{
				object allowRollOver = ViewState["_allowRollOver"];
				if (allowRollOver != null)
					return (bool)allowRollOver;
				else
					return true;
			}

			set
			{
				ViewState["_allowRollOver"] = value;
			}
		}

		/// <summary>
		/// Image used as background of the tool.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(""),
		Browsable(true),
		NotifyParentProperty(true),
		Description("Image used as background of the tool.")
		]
		public string BackImage
		{
			get
			{
				object backImage = ViewState["_backImage"];
				if (backImage != null)
					return (string)backImage;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_backImage"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the collection containing the items.
		/// </summary>
		[
			DefaultValue(null),
			MergableProperty(false),
			PersistenceMode(PersistenceMode.InnerDefaultProperty),
			Description("Items of the contol.")
		]
		public ToolItemCollection Items
		{
			get
			{
				if (_items == null)
				{
					_items = new ToolItemCollection();
					if (IsTrackingViewState)
					{
						((IStateManager)_items).TrackViewState();
					}
				}
				return _items;
			}
		}

		/// <summary>
		/// Gets or sets the relative link to the dropdown image to use.
		/// </summary>
		[
			Category("Appearance"),
			BindableAttribute(true),
			DefaultValue(""),
			Description("The relative link to the dropdown image to use.")
		]
		public string DropDownImage
		{
			get
			{
				var dropDownImage = ViewState[DropDownImageKey];
				if (dropDownImage != null)
				{
					return (string)dropDownImage;
				}
				else
				{
					return string.Empty;
				}
			}
			set
			{
				ViewState[DropDownImageKey] = value;
			}
		}

		protected string IndentText
		{
			get
			{
				var indentText = ViewState[IndentTextKey];
				if (indentText != null)
				{
					return (string)indentText;
				}
				else
				{
					return IndentTextDotValue;
				}
			}
			set
			{
				ViewState[IndentTextKey] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether or not the control posts back to the server each time a user interacts with the control. 
		/// </summary>
		[
			Bindable(true),
			Category("Behavior"),
			DefaultValue(true),
			Browsable(true),
			Description("Indicates whether or not the control posts back to the server each time a user interacts with the control.")
		]
		public bool AutoPostBack
		{
			get
			{
				var autopostBack = ViewState[AutoPostBackKey];
				if (autopostBack != null)
				{
					return (bool)autopostBack;
				}
				return false;
			}
			set
			{
				ViewState[AutoPostBackKey] = value;
			}
		}

		protected SelectedText ChangeToSelectedText
		{
			get
			{
				var changeToSelectedText = ViewState[ChangeToSelectedTextKey];
				if (changeToSelectedText != null)
				{
					return (SelectedText)changeToSelectedText;
				}
				else
				{
					return SelectedText.None;
				}
			}
			set
			{
				ViewState[ChangeToSelectedTextKey] = value;
			}
		}

		/// <summary>
		/// Gets or sets the window border style.
		/// </summary>
		/// <value>The window border style.</value>
		[
			Category("Appearance"),
			BindableAttribute(true),
			DefaultValue(typeof(BorderStyle), "Solid"),
			Description("The window border style.")
		]
		public BorderStyle WindowBorderStyle
		{
			get
			{
				var winBorderStyle = ViewState[WindowBorderStyleKey];
				if (winBorderStyle != null)
				{
					return (BorderStyle)winBorderStyle;
				}
				else
				{
					return BorderStyle.None;
				}
			}
			set
			{
				ViewState[WindowBorderStyleKey] = value;
			}
		}

		/// <summary>
		/// Gets or sets the window border style.
		/// </summary>
		/// <value>The window border style.</value>
		[
			Category("Appearance"),
			BindableAttribute(true),
			DefaultValue(typeof(Unit), "1"),
			Description("The window border style.")
		]
		public Unit WindowBorderWidth
		{
			get
			{
				var winBorderWidth = ViewState[WindowBorderWidthKey];
				if (winBorderWidth != null)
				{
					return (Unit)winBorderWidth;
				}
				else
				{
					return Unit.Empty;
				}
			}
			set
			{
				ViewState[WindowBorderWidthKey] = value;
			}
		}

		protected Color ItemBorderColor
		{
			get
			{
				var itemBorderColor = ViewState[ItemBorderColorKey];
				if (itemBorderColor != null)
				{
					return (Color)itemBorderColor;
				}
				else
				{
					return Color.Empty;
				}
			}
			set
			{
				ViewState[ItemBorderColorKey] = value;
			}
		}

		protected Color ItemBorderColorRollOver
		{
			get
			{
				var itemBorderColorRollOver = ViewState[ItemBorderColorRollOverKey];
				if (itemBorderColorRollOver != null)
				{
					return (Color)itemBorderColorRollOver;
				}
				else
				{
					return Color.Empty;
				}
			}
			set
			{
				ViewState[ItemBorderColorRollOverKey] = value;
			}
		}

		protected Align ItemAlign
		{
			get
			{
				var itemAlign = ViewState[ItemAlignKey];
				if (itemAlign != null)
				{
					return (Align)itemAlign;
				}
				else
				{
					return Align.Left;
				}
			}
			set
			{
				ViewState[ItemAlignKey] = value;
			}
		}

        /// <summary>
		/// Gets or sets the boder color rollover.
		/// </summary>
		[
        Bindable(true),
        TypeConverterAttribute(typeof(WebColorConverter)),
        DefaultValue(typeof(Color), BorderColorRollOverDefaultValue),
        Category("Appearance"),
        Description("The boder color rollover.")
        ]
        public Color BorderColorRollOver
        {
            get
            {
                return _borderColorRollOver;
            }
            set
            {
                _borderColorRollOver = value;
            }
        }

        /// <summary>
        /// Gets or sets the border width.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(typeof(Unit), BorderWidthDefaultValue),
        Category("Appearance"),
        Description("Border width.")
        ]
        public override Unit BorderWidth
        {
            get
            {
                return base.BorderWidth;
            }
            set
            {
                base.BorderWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets the border style.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(typeof(BorderStyle), BorderStyleDefaultValue),
        Category("Appearance"),
        Description("Boder style.")
        ]
        public override BorderStyle BorderStyle
        {
            get
            {
                return base.BorderStyle;
            }
            set
            {
                base.BorderStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount of space between the contents of a cell and the cell's border.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(0),
        Category("Appearance"),
        Description("The amount of space between the contents of a cell and the cell's border.")
        ]
        public Unit Cellpadding
        {
            get
            {
                return _cellpadding;
            }
            set
            {
                _cellpadding = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount of space between cells.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(0),
        Category("Appearance"),
        Description("The amount of space between cells.")
        ]
        public Unit Cellspacing
        {
            get
            {
                return _cellspacing;
            }
            set
            {
                _cellspacing = value;
            }
        }

        /// <summary>
        /// Gets or sets the background color items.
        /// </summary>
        /// <value>The background color items.</value>
        [
        Bindable(true),
        DefaultValue(typeof(Color), BackColorItemsDefaultValue),
        Category("Appearance"),
        Description("The background color items.")
        ]
        public Color BackColorItems
        {
            get
            {
                var bgColorItems = ViewState[BackColorItemsKey];
                if (bgColorItems != null)
                {
                    return (Color)bgColorItems;
                }
                else
                {
                    return Color.Empty;
                }
            }
            set
            {
                ViewState[BackColorItemsKey] = value;
            }
        }

		/// <summary>
		/// Border width of the tool when the mouse is over.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(""),
		Browsable(true),
		NotifyParentProperty(true),
		Description("Border width of the tool when the mouse is over.")
		]
		public Unit BorderWidthRollOver
		{
			get
			{
				var borderWidthRollOver = ViewState[BorderWidthRollOverKey];
				if (borderWidthRollOver != null)
				{
					return (Unit)borderWidthRollOver;
				}
				else
				{
					return Unit.Empty;
				}
			}
			set
			{
				ViewState[BorderWidthRollOverKey] = value;
			}
		}

		/// <summary>
		/// Background color of the tool when the mouse is over.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(""),
		Browsable(true),
		NotifyParentProperty(true),
		Description("Background color of the tool when the mouse is over."),
		TypeConverter(typeof(WebColorConverter))
		]
		public Color BackColorRollOver
		{
			get
			{
				var backColorRollOver = ViewState[BackColorRollOverKey];
				if (backColorRollOver != null)
				{
					return (Color)backColorRollOver;
				}
				else
				{
					return Color.Empty;
				}
			}
			set
			{
				ViewState[BackColorRollOverKey] = value;
			}
		}

		/// <summary>
		/// "Background color of the tool when a click occurs.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(""),
		Browsable(true),
		NotifyParentProperty(true),
		Description("Background color of the tool when a click occurs."),
		TypeConverter(typeof(WebColorConverter))
		]
		public Color BackColorClicked
		{
			get
			{
				var backColorClicked = ViewState[BackColorClickedKey];
				if (backColorClicked != null)
				{
					return (Color)backColorClicked;
				}
				else
				{
					return Color.Empty;
				}
			}
			set
			{
				ViewState[BackColorClickedKey] = value;
			}
		}

		/// <summary>
		/// Image used as background of the tool when a click occurs.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(""),
		Browsable(true),
		NotifyParentProperty(true),
		Description("Image used as background of the tool when a click occurs")
		]
		public string BackImageClicked
		{
			get
			{
				var backImageClicked = ViewState[BackImageClickedKey];
				if (backImageClicked != null)
				{
					return (string)backImageClicked;
				}
				else
				{
					return string.Empty;
				}
			}
			set
			{
				ViewState[BackImageClickedKey] = value;
			}
		}

        /// <summary>
        /// Renders at the design time.
        /// </summary>
        /// <param name="output">The output.</param>
        public virtual void RenderDesign(HtmlTextWriter output)
		{
			this.RenderControl(output);
		}

		/// <summary>
		/// Notifies the Popup control to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{
			base.OnPreRender(e);

			if (this.Enabled)
			{
				if(ClientScriptBlock != string.Empty && !Page.IsClientScriptBlockRegistered(ClientScriptKey))
					Page.RegisterClientScriptBlock(ClientScriptKey, ClientScriptBlock);

				if(StartupScriptBlock != string.Empty && !Page.IsStartupScriptRegistered(StartupScriptKey))
					Page.RegisterStartupScript(StartupScriptKey, StartupScriptBlock);

			}
		}

		/// <summary>
		/// Loads the view state.
		/// </summary>
		/// <param name="savedState">The saved view state.</param>
		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState(savedState, (state) => ((IStateManager)Items).LoadViewState(state));
		}

		/// <summary>
		/// Saves the view state.
		/// </summary>
		/// <returns></returns>
		protected override object SaveViewState()
		{
			return base.SaveViewState(_items);
	    }

		/// <summary>
		/// Tracks the view state.
		/// </summary>
		protected override void TrackViewState()
		{
			base.TrackViewState(_items);
		}

		/// <summary>
		/// Remove the selection to all items.
		/// </summary>
		public virtual void ClearSelection()
		{
			for (var count = 0; count < this.Items.Count; count++)
			{
				this.Items[count].Selected = false;
			}
		}
	}
}