using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Collections;
using System.Drawing;

namespace ActiveUp.WebControls
{
    #region class ToolSubMenu

	/// <summary>
	/// Represents a <see cref="ToolSubMenu"/> object.
	/// </summary>
    [
    PersistChildrenAttribute(false),
    ParseChildrenAttribute(true, "Items"),
    //TypeConverterAttribute(typeof(ExpandableObjectConverter)),
    Serializable
    ]
    public class ToolSubMenu : Control , IStateManager
    {

        #region Variables

        private bool _isTrackingViewState;
        private StateBag _viewState;
        private MenuItemCollection _items;
        private MenuItemStyle _styleItems;
        private MenuStyle _styleMenu;

        #endregion

        #region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolSubMenu"/> class.
		/// </summary>
        public ToolSubMenu()
        {
            _items = new MenuItemCollection(this.Controls);

            _styleMenu = new MenuStyle();
            _styleMenu.BorderColor = Color.FromArgb(0xB4,0xB1,0xA3);
            _styleMenu.BackColor = Color.FromArgb(0xE0,0xE0,0xE0);
            _styleMenu.BorderStyle = BorderStyle.Solid;
            _styleMenu.BorderWidth = Unit.Parse("1px");
            
            _styleItems = new MenuItemStyle();
            _styleItems.BorderColorOver = Color.FromArgb(0x31,0x6A,0xC5);
            _styleItems.BackColorOver = Color.FromArgb(0x9D,0xB9,0xEB);
            _styleItems.BorderStyleOver = BorderStyle.Solid;
            _styleItems.BorderWidthOver = Unit.Parse("1px");
        }

        #endregion

        #region Properties

        #region Appearence

        /// <summary>
        /// Gets or sets the cell spacing of the table representing the menu.
        /// </summary>
        [
        Bindable(false),
        Category("Appearence"),
        Description("Gets or sets the cell spacing of the table representing the menu."),
        DefaultValue("1")
        ]
        public int CellSpacing
        {
            get
            {
                object _cellSpacing;
                _cellSpacing = ViewState["_cellSpacing"];
                if (_cellSpacing != null)
                    return (int)_cellSpacing;
                return 1;
            }
            set
            {
                ViewState["_cellSpacing"] = value;
            }
        }


        /// <summary>
        /// Gets or sets the cell padding of the table representing the toolbar.
        /// </summary>
        [
        Bindable(false),
        Category("Appearence"),
        Description("Gets or sets the cell padding of the table representing the toolbar."),
        DefaultValue("1")
        ]
        public int CellPadding
        {
            get
            {
                object _cellSpacing;
                _cellSpacing = ViewState["_cellPadding"];
                if (_cellSpacing != null)
                    return (int)_cellSpacing;
                return 1;
            }
            set
            {
                ViewState["_cellPadding"] = value;
            }
        }

		/// <summary>
		/// Gets or sets the style menu.
		/// </summary>
		/// <value>The style menu.</value>
        [
        Bindable(true),
        DefaultValue(""),
        Description("Style of the sub menu."),
        DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
        Category("Appearance")
        ]
        public MenuStyle StyleMenu
        {
            get
            {
                return _styleMenu;
            }

			set
			{
				_styleMenu = value;
			}

        }

		/// <summary>
		/// Gets or sets the sub menu style items.
		/// </summary>
		/// <value>The sub menu style items.</value>
        [
        Bindable(true),
        DefaultValue(""),
        Description("Style of the sub menu item."),
        DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
        Category("Appearance")
        ]
        public MenuItemStyle StyleItems
        {
            get
            {
                return _styleItems;
            }

			set
			{
				_styleItems = value;
			}
        }

        #endregion

        #region Data

        /// <summary>
        /// Gets or sets the collection containing the sub menu items.
        /// </summary>
        [
        Category("Data"),
        Description("The collection of sub menu items"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerDefaultProperty)
        ]
        public MenuItemCollection Items
        {
            get
            {
                if (IsTrackingViewState)
                {
                    ((IStateManager)_items).TrackViewState();
                }
                return _items;
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
        protected override StateBag ViewState
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

        internal void SetDirty()
        {
            if (_viewState != null)
            {
                ICollection Keys = _viewState.Keys;
                foreach (string key in Keys)
                {
                    _viewState.SetItemDirty(key, true);
                }
            }
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
