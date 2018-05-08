using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;

namespace ActiveUp.WebControls
{
    #region MenuItem


	/// <summary>
	/// Represents a <see cref="MenuItem"/> object.
	/// </summary>
    [
        /*PersistChildrenAttribute(false),
        ParseChildrenAttribute(true),*/
        TypeConverterAttribute(typeof(ExpandableObjectConverter)),
        //ToolboxItemAttribute(false),
        Serializable
    ]
    public class MenuItem : WebControl, IStateManager, IPostBackDataHandler, IPostBackEventHandler
    {
        #region Variables

        private bool _isTrackingViewState;
        private StateBag _viewState;
        private ToolSubMenu _subMenu = new ToolSubMenu();
       
        #endregion

        #region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MenuItem"/> class.
		/// </summary>
        public MenuItem()
        {
            _Init(string.Empty);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="MenuItem"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
        public MenuItem(string id)
        {
            _Init(id);
        }

        private void _Init(string id)
        {
            if (id != string.Empty)
                ID = id;

            this.Controls.Add(_subMenu);
        }

        #endregion

        #region Properties

        #region Appearance

        /// <summary>
        /// Gets or sets the text of the item.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("The text of the item."),
        Category("Appearance")
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
        /// Gets or sets the width of the menu item
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("Alignment of the menu item."),
        Category("Appearance")
        ]
        public HorizontalAlign Align
        {
            get
            {
                object align = ViewState["_align"];
                if (align != null)
                    return (HorizontalAlign)align;
                else
                    return HorizontalAlign.NotSet;
            }

            set
            {
                ViewState["_align"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the height of an item menu.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Height of a item menu."),
        DefaultValueAttribute(typeof(Unit), ""),
        NotifyParentProperty(true)
        ]
        public override Unit Height
        {
            get
            {
                object height = ViewState["_height"];
                if (height != null)
                    return (Unit)height;
                else
                    return Unit.Empty;
            }

            set
            {
                ViewState["_height"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the height of an item menu.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Width of a item menu."),
        DefaultValueAttribute(typeof(Unit), ""),
        NotifyParentProperty(true)
        ]
        public override Unit Width
        {
            get
            {
                object width = ViewState["_width"];
                if (width != null)
                    return (Unit)width;
                else
                    return Unit.Empty;
            }

            set
            {
                ViewState["_width"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the image of the item.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("The image of the item."),
        Category("Appearance"),
        NotifyParentProperty(true)
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
        /// Gets or sets the image of the item.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("The image over of the item."),
        Category("Appearance"),
        NotifyParentProperty(true)
        ]
        public string ImageOver
        {
            get
            {
                object imageOver = ViewState["_imageOver"];
                if (imageOver != null)
                    return (string)imageOver;
                else
                    return string.Empty;
            }

            set
            {
                ViewState["_imageOver"] = value;
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

        #region Data

        /// <summary>
        /// Gets or sets the sub menu.
        /// </summary>
        [
        Category("Data"),
        Description("The sub menu"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerProperty),
        Browsable(true)
        ]
        public ToolSubMenu SubMenu
        {
            get
            {
                if (IsTrackingViewState)
                {
                    ((IStateManager)_subMenu).TrackViewState();
                }
                return _subMenu;
            }
        }

        /// <summary>
        /// Gets or sets the embedded object of the item.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("The embedded object of the item."),
        Category("Appearance"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public WebControl EmbeddedObject
        {
            get
            {
                object embeddedObject = ViewState["_embeddedObject"];
                if (embeddedObject != null)
                    return (WebControl)embeddedObject;
                else
                    return null;
            }

            set
            {
                ViewState["_embeddedObject"] = value;
            }
        }

        #endregion

        #region Naviagation

        /// <summary>
        /// Gets or sets the navigation link when a click occurs.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("The navigation click when a click occurs."),
        Category("Appearance")
        ]
        public string NavigateURL
        {
            get
            {
                object navigateURL = ViewState["_navigateURL"];
                if (navigateURL != null)
                    return (string)navigateURL;
                else
                    return string.Empty;
            }

            set
            {
                ViewState["_navigateURL"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the target when a click occurs.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("The target when a click occurs."),
        Category("Appearance")
        ]
        public string Target
        {
            get
            {
                object target = ViewState["_target"];
                if (target != null)
                    return (string)target;
                else
                    return string.Empty;
            }

            set
            {
                ViewState["_target"] = value;
            }
        }

        #endregion

        #region JScript

        /// <summary>
        /// Gets or sets the onclick event client side.
        /// </summary>
        [
        Bindable(false),
        Category("Behavior"),
        Description("Gets or sets the onclick event client side."),
        DefaultValue("")
        ]
        public string OnClickClient
        {
            get
            {
                object onClickClient = ViewState["_onClickClient"];
                if (onClickClient != null)
                    return (string)onClickClient;
                else
                    return string.Empty;
            }

            set
            {
                ViewState["_onClickClient"] = value;
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

        /// <summary> 
        /// Render this control to the output parameter specified.
        /// </summary>
        /// <param name="output">Output stream that contains the HTML used to represent the control.</param>
        protected override void Render(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Type, "Hidden");
            output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            output.AddAttribute(HtmlTextWriterAttribute.Value, DateTime.Now.Ticks.ToString());
            output.RenderBeginTag(HtmlTextWriterTag.Input);
            output.RenderEndTag();
        }

        #endregion

        #region Interface IPostBack

        /// <summary>
        /// A RaisePostBackEvent.
        /// </summary>
        /// <param name="eventArgument">eventArgument</param>
        public void RaisePostBackEvent(String eventArgument)
        {
            Page.Trace.Warn(ID, "RaisePostBackEvent");
        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            Page.Trace.Warn(ID, "LoadPostData");

            return true;
        }

		/// <summary>
		/// When implemented by a class, signals the server control object to notify the ASP.NET application that the state of the
		/// control has changed.
		/// </summary>
        public virtual void RaisePostDataChangedEvent()
        {
            Page.Trace.Warn(ID, "RaisePostDataChangedEvent");
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
