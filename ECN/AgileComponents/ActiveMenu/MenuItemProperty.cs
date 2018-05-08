using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace ActiveUp.WebControls
{
    #region MenuItemProperty

	/// <summary>
	/// Represents a <see cref="MenuItemProperty"/> object.
	/// </summary>
    public class MenuItemProperty
    {

        #region Variables

        private string _uniqueID;
        private string _text;
        private Unit _width, _height;
        private string _image, _imageOver;
        private string _target, _navigateURL;
        private HorizontalAlign _align;
        private string _onClickClient;

        #endregion

        #region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="MenuItemProperty"/> class.
		/// </summary>
        public MenuItemProperty()
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        [
        Browsable(false),
        Description("Unique id the current item."),
        DefaultValue(""),
        Category("Appearance")
        ]
        public string UniqueID
        {
            get { return _uniqueID; }
            set { _uniqueID = value; }
        }

        /// <summary>
        /// Gets or sets the displayed text.
        /// </summary>
        [
        Browsable(true),
        Description("Text of the current item."),
        DefaultValue(""),
        Category("Appearance")
        ]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Gets or sets the height of the menu item
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("Height of the menu item."),
        Category("Appearance")
        ]
        public Unit Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Gets or sets the width of the menu item
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("Width of the menu item."),
        Category("Appearance")
        ]
        public Unit Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Gets or sets the image of the item.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("The image of the item."),
        Category("Appearance")
        ]
        public string Image
        {
            get { return _image; }
            set { _image = value; }
        }

        /// <summary>
        /// Gets or sets the image of the item.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("The image over of the item."),
        Category("Appearance")
        ]
        public string ImageOver
        {
            get { return _imageOver; }
            set { _imageOver = value; }
        }

        /// <summary>
        /// Gets or sets the target of the item.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("The target of the item."),
        Category("Appearance")
        ]
        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }

        /// <summary>
        /// Gets or sets the image over of the item.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("The navigation link of the item."),
        Category("Appearance")
        ]
        public string NavigateURL
        {
            get { return _navigateURL; }
            set { _navigateURL = value; }
        }

        /// <summary>
        /// Gets or sets the align of the item.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("The align of the item."),
        Category("Appearance")
        ]
        public HorizontalAlign Align
        {
            get { return _align; }
            set { _align = value; }
        }

        /// <summary>
        /// Gets or sets the on click client side event.
        /// </summary>
        [
        Bindable(true),
        DefaultValue(""),
        Description("The on click client side event."),
        Category("Appearance")
        ]
        public string OnClickClient
        {
            get { return _onClickClient; }
            set { _onClickClient = value; }
        }

        #endregion
    }

    #endregion
}
