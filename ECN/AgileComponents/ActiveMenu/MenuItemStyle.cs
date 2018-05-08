using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web;

namespace ActiveUp.WebControls
{
	#region MenuItemStyle

	/// <summary>
	/// Represents a <see cref="MenuItemStyle"/> object.
	/// </summary>
    [
        ToolboxItemAttribute(false),
        Serializable
    ]
    public class MenuItemStyle : Component, IStateManager
    {
        #region Variable

        private bool _isTrackingViewState;
        private StateBag _viewState;

        #endregion

        #region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="MenuItemStyle"/> class.
		/// </summary>
        public MenuItemStyle()
        {
        }

        #endregion

        #region Properties

        #region Appearance

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
        /// Gets or sets the background color.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Background color of a item menu."),
        TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
        DefaultValueAttribute(typeof(Color), ""),
        NotifyParentProperty(true)
        ]
        public Color BackColor
        {
            get
            {
                object backColor = ViewState["_backColor"];
                if (backColor != null)
                    return (Color)backColor;
                else
                    return Color.Empty;
            }

            set
            {
                ViewState["_backColor"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the background color rollover.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Background color rollover of a item menu."),
        TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
        DefaultValueAttribute(typeof(Color), ""),
        NotifyParentProperty(true)
        ]
        public Color BackColorOver
        {
            get
            {
                object backColorOver = ViewState["_backColorOver"];
                if (backColorOver != null)
                    return (Color)backColorOver;
                else
                    return Color.Empty;
            }

            set
            {
                ViewState["_backColorOver"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the border color.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Border color of a item menu."),
        TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
        DefaultValueAttribute(typeof(Color), ""),
        NotifyParentProperty(true)
        ]
        public Color BorderColor
        {
            get
            {
                object borderColor = ViewState["_borderColor"];
                if (borderColor != null)
                    return (Color)borderColor;
                else
                    return Color.Empty;
            }

            set
            {
                ViewState["_borderColor"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the border color rollover.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Border color rollover of a item menu."),
        TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
        DefaultValueAttribute(typeof(Color), ""),
        NotifyParentProperty(true)
        ]
        public Color BorderColorOver
        {
            get
            {
                object borderColorOver = ViewState["_borderColorOver"];
                if (borderColorOver != null)
                    return (Color)borderColorOver;
                else
                    return Color.Empty;
            }

            set
            {
                ViewState["_borderColorOver"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the border style of a item menu.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Border style of a item menu."),
        DefaultValueAttribute(BorderStyle.NotSet),
        NotifyParentProperty(true)
        ]
        public BorderStyle BorderStyle
        {
            get
            {
                object borderStyle = ViewState["_borderStyle"];
                if (borderStyle != null)
                    return (BorderStyle)borderStyle;
                else
                    return BorderStyle.NotSet;
            }

            set
            {
                ViewState["_borderStyle"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the border style rollover of a item menu.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Border style rollover of a item menu."),
        DefaultValueAttribute(BorderStyle.NotSet),
        NotifyParentProperty(true)
        ]
        public BorderStyle BorderStyleOver
        {
            get
            {
                object borderStyleOver = ViewState["_borderStyleOver"];
                if (borderStyleOver != null)
                    return (BorderStyle)borderStyleOver;
                else
                    return BorderStyle.NotSet;
            }

            set
            {
                ViewState["_borderStyleOver"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the border width of a item menu.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Border width of the contents."),
        DefaultValueAttribute(typeof(Unit), ""),
        NotifyParentProperty(true)
        ]
        public Unit BorderWidth
        {
            get
            {
                object borderWidth = ViewState["_borderWidth"];
                if (borderWidth != null)
                    return (Unit)borderWidth;
                else
                    return Unit.Empty;
            }

            set
            {
                ViewState["_borderWidth"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the border width of a item menu.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Border width of the contents."),
        DefaultValueAttribute(typeof(Unit), ""),
        NotifyParentProperty(true)
        ]
        public Unit BorderWidthOver
        {
            get
            {
                object borderWidthOver = ViewState["_borderWidthOver"];
                if (borderWidthOver != null)
                    return (Unit)borderWidthOver;
                else
                    return Unit.Empty;
            }

            set
            {
                ViewState["_borderWidthOver"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the CSS class.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Css class of a item menu."),
        DefaultValueAttribute(""),
        NotifyParentProperty(true)
        ]
        public string CssClass
        {
            get
            {
                object cssClass = ViewState["_cssClass"];
                if (cssClass != null)
                    return (string)cssClass;
                else
                    return string.Empty;
            }

            set
            {
                ViewState["_cssClass"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the CSS rollover class.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Css rollover class of a item menu."),
        DefaultValueAttribute(""),
        NotifyParentProperty(true)
        ]
        public string CssClassOver
        {
            get
            {
                object cssClassOver = ViewState["_cssClassOver"];
                if (cssClassOver != null)
                    return (string)cssClassOver;
                else
                    return string.Empty;
            }

            set
            {
                ViewState["_cssClassOver"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the fore color of an item menu.
        /// </summary>
        [
		Bindable(true),
		Category("Appearance"),
		Description("Fore color of a item menu."),
		DefaultValueAttribute(typeof(Color), ""),
		NotifyParentProperty(true)
		]
		public Color ForeColor
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(ForeColor), Color.Empty);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(ForeColor), value);
			}
		}

        /// <summary>
        /// Gets or sets the fore color rollover of an item menu.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Fore color rollover of a item menu."),
        DefaultValueAttribute(typeof(Color), ""),
        NotifyParentProperty(true)
        ]
        public Color ForeColorOver
        {
            get
            {
                object foreColorOver = ViewState["_foreColorOver"];
                if (foreColorOver != null)
                    return (Color)foreColorOver;
                else
                    return Color.Empty;
            }

            set
            {
                ViewState["_foreColorOver"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the margin of a item menu.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("margin of the contents."),
        DefaultValueAttribute(typeof(Unit), ""),
        NotifyParentProperty(true)
        ]
        public Unit Margin
        {
            get
            {
                object margin = ViewState["_margin"];
                if (margin != null)
                    return (Unit)margin;
                else
                    return Unit.Empty;
            }

            set
            {
                ViewState["_margin"] = value;
            }
        }

        #endregion

        #region ViewState

		/// <summary>
		/// Gets a dictionary of state information that allows you to save and restore the view
		/// state of a server control across multiple requests for the same page.
		/// </summary>
		/// <value></value>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        protected StateBag ViewState
        {
            get
            {
                if (_viewState == null)
                {
                    _viewState = new StateBag(false);
                    if (_isTrackingViewState) ((IStateManager)_viewState).TrackViewState();
                }
                return _viewState;
            }
        }

        #endregion

        #endregion

        #region Methods

		/// <summary>
		/// Creates the HTML CSS.
		/// </summary>
		/// <param name="clientID">The client ID.</param>
		/// <param name="Name">The name.</param>
		/// <returns></returns>
        public string CreateHtmlCss(string clientID, string Name)
        {
            string style = string.Empty;
            style += string.Format(".AME_{0}{1}\n", clientID, Name);
            style += "{\n";
            if (BackColor != Color.Empty)
                style += string.Format("background-color: {0};\n", Utils.Color2Hex(BackColor));
            if (BorderStyle != BorderStyle.NotSet)
            {
                style += string.Format("border-style : {0};\n", BorderStyle.ToString());
                style += string.Format("border-width : {0};\n", BorderWidth);
                style += string.Format("border-color : {0};\n", Utils.Color2Hex(BorderColor));
            }

            if (ForeColor != Color.Empty)
                style += string.Format("color : {0};\n", Utils.Color2Hex(ForeColor));

            if (AllowRollOver && BorderStyleOver != BorderStyle.NotSet && BorderWidthOver.Value > 0)
            {
                int margin = 0;
                if (BorderWidth.Value > BorderWidthOver.Value)
                    margin = 0;
                else if (BorderWidth.Value < BorderWidthOver.Value)
                    margin = (int)(BorderWidthOver.Value - BorderWidth.Value);
                else
                    margin = 0;

                margin += (int)Margin.Value;

                style += string.Format("margin : {0}px;\n", margin);
            }
            else
                style += string.Format("margin : {0}px;\n", (int)Margin.Value);

            style += "}\n\n";

            if (AllowRollOver)
            {
                style += string.Format(".AME_{0}{1}Over\n", clientID, Name);
                style += "{\n";
                if (BackColorOver != Color.Empty)
                    style += string.Format("background-color: {0};\n", Utils.Color2Hex(BackColorOver));
                if (BorderStyleOver != BorderStyle.NotSet)
                {
                    style += string.Format("border-style : {0};\n", BorderStyleOver.ToString());
                    style += string.Format("border-width : {0};\n", BorderWidthOver);
                    style += string.Format("border-color : {0};\n", Utils.Color2Hex(BorderColorOver));
                }

                if (AllowRollOver && BorderStyleOver != BorderStyle.NotSet && BorderWidthOver.Value > 0)
                {
                    int margin = 0;
                    if (BorderWidthOver.Value > BorderWidth.Value)
                        margin = 0;
                    else if (BorderWidthOver.Value < BorderWidth.Value)
                        margin = (int)(BorderWidth.Value - BorderWidthOver.Value);
                    else
                        margin = 0;

                    margin += (int)Margin.Value;

                    style += string.Format("margin : {0}px;\n", margin);
                }
                else
                    style += string.Format("margin : {0}px;\n", Margin);

                if (ForeColorOver != Color.Empty)
                    style += string.Format("color : {0};\n", Utils.Color2Hex(ForeColorOver));

                style += "}\n\n";
            }

            return style;
        }

        #endregion

        #region ViewState

        bool IStateManager.IsTrackingViewState
        {
            get
            {
                return _isTrackingViewState;
            }
        }

        void IStateManager.LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                ((IStateManager)ViewState).LoadViewState(savedState);
            }
        }

        object IStateManager.SaveViewState()
        {
            if (_viewState != null)
            {
                return ((IStateManager)_viewState).SaveViewState();
            }
            return null;
        }

        void IStateManager.TrackViewState()
        {
            _isTrackingViewState = true;

            if (_viewState != null)
            {
                ((IStateManager)_viewState).TrackViewState();
            }
        }

        #endregion

    }

    #endregion
}
