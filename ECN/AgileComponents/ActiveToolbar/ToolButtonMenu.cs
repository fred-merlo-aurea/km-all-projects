using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolButtonMenu"/> object.
	/// </summary>
	[
    ToolboxItem(false),
	ParseChildren(true, "Items"),
	Serializable 
	]
	public class ToolButtonMenu : ToolBase,INamingContainer ,IPostBackEventHandler, IPostBackDataHandler
	{
		//private readonly string TOOLBUTTONMENUKEY = "ToolButtonMenu";

		/// <summary>
		/// Collection of items present in the button menu.
		/// </summary>
		private ToolButtonMenuItemCollection _items = null;


		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolButtonMenu"/> class.
		/// </summary>
		public ToolButtonMenu() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolButtonMenu"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolButtonMenu(string id) : base(id)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the relative link to the image to use for the tool.
		/// </summary>
		[
		Bindable(true), 
		Category("Appearance"), 
		DefaultValue(""),
		Description("The relative link to the image to use for the tool.")
		] 
		public string ImageURL
		{
			get
			{
				string _imageURL;
				_imageURL = ((string) base.ViewState["_imageURL"]);
				if (_imageURL != null)
				{
					return _imageURL; 
				}
				return string.Empty;
			}
			set
			{
				ViewState["_imageURL"] = value; 
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
				object _autoPostBack;
				_autoPostBack = ViewState["_autoPostBack"];
				if (_autoPostBack != null)
				{
					return ((bool) _autoPostBack); 
				}
				return true;
			}
			set
			{
				ViewState["_autoPostBack"] = value;
			}
		}

		/// <summary>
		/// Text alignement of the tool.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(typeof(TextAlign),"Right"),
		Browsable(true),
		NotifyParentProperty(true),
		Description("Text alignement of the tool.")
		]
		public TextAlign TextAlign
		{
			get
			{
				object textAlign = ViewState["_textAlign"];
				if (textAlign != null)
					return (TextAlign)textAlign;
				else
					return TextAlign.Right;
			}

			set
			{
				ViewState["_textAlign"] = value;
			}
		}

		/// <summary>
		/// Image use as background when the mouse is over.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(""),
		Browsable(true),
		NotifyParentProperty(true),
		Description("Image used as background of the tool.")
		]
		public string BackImageRollOver
		{
			get
			{
				object backImageRollOver = ViewState["_backImageRollOver"];
				if (backImageRollOver != null)
					return (string)backImageRollOver;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_backImageRollOver"] = value;
			}
		}

		/// <summary>
		/// Border color of the tool when the mouse is over.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(""),
		Browsable(true),
		NotifyParentProperty(true),
		Description("Border color of the tool when the mouse is over."),
		TypeConverter(typeof(System.Web.UI.WebControls.WebColorConverter))
		]
		public Color BorderColorRollOver
		{
			get
			{
				object borderColorRollOver = ViewState["_borderColorRollOver"];
				if (borderColorRollOver != null)
					return (Color)borderColorRollOver;
				else
					return Color.Empty;
			}

			set
			{
				ViewState["_borderColorRollOver"] = value;
			}
		}

		/// <summary>
		/// Border style of the tool when the mouse is over.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(typeof(BorderStyle),"NotSet"),
		Browsable(true),
		NotifyParentProperty(true),
		Description("Border style of the tool when the mouse is over."),
		]
		public BorderStyle BorderStyleRollOver
		{
			get
			{
				object borderStyleRollOver = ViewState["_borderStyleRollOver"];
				if (borderStyleRollOver != null)
					return (BorderStyle)borderStyleRollOver;
				else
					return BorderStyle.NotSet;
			}

			set
			{
				ViewState["_borderStyleRollOver"] = value;
			}
		}

		/// <summary>
		/// Image used for the dropdown menu.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(""),
		Browsable(true),
		NotifyParentProperty(true),
		Description("Image used for the dropdown menu.")
		]
		public string DropImage
		{
			get
			{
				object dropImage = ViewState["_dropImage"];
				if (dropImage != null)
					return (string)dropImage;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_dropImage"] = value;
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
		public ToolButtonMenuItemCollection Items 
		{
			get 
			{
				if (_items == null) 
				{
					_items = new ToolButtonMenuItemCollection();
					if (IsTrackingViewState) 
					{
						((IStateManager)_items).TrackViewState();
					}
				}
				return _items;
			}
		}

		/// <summary>
		/// Gets or sets the left background image.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(""),
		Browsable(true),
		NotifyParentProperty(true),
		Description("Left bakcground image of the menu.")
		]
		public string LeftBackImage
		{
			get
			{
				object leftBackImage = ViewState["_leftBackImage"];
				if (leftBackImage != null)
					return (string)leftBackImage;
				else
					return null;
			}

			set
			{
				ViewState["_leftBackImage"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the window border color.
		/// </summary>
		/// <value>The window border color..</value>
		[
		Category("Appearance"),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		BindableAttribute(true),
		DefaultValue(typeof(Color),"#B4B1A3"),
		Description("The window border color.")
		]
		public Color WindowBorderColor
		{
			get
			{
				object winBorderColor;
				winBorderColor = ViewState["_windowBorderColor"];
				if (winBorderColor != null)
					return (Color)winBorderColor;
				else
					return Color.Empty;
			}

			set
			{
				ViewState["_windowBorderColor"] = value;
			}

		}
		#endregion

		#region Methods
		/// <summary>
		/// Render the tool to the specified HtmlTextWriter object. Usually a Page.
		/// </summary>
		protected void RenderButtonMenu(HtmlTextWriter output)
		{
			/*output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Value,AllowRollOver.ToString());
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_AllowRollOver");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_AllowRollOver");
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Value,"Normal");
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_Type");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_Type");
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();


			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0");
			output.RenderBeginTag(HtmlTextWriterTag.Table);
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.RenderBeginTag(HtmlTextWriterTag.Td);

			Unit margin = new Unit("0");
			Unit marginRollOver = new Unit("0");

			if (BorderWidth.Value == BorderWidthRollOver.Value)
			{
				margin = new Unit("1");
				marginRollOver = new Unit("1");
			}
			else if (BorderWidth.Value < BorderWidthRollOver.Value)
			{
				margin = new Unit((BorderWidthRollOver.Value - BorderWidth.Value + 1).ToString());
				marginRollOver = new Unit("1");
			}

			else if (BorderWidth.Value > BorderWidthRollOver.Value)
			{
				margin = new Unit("1");
				marginRollOver = new Unit((BorderWidth.Value - BorderWidthRollOver.Value + 1).ToString());
			}

			string backColor = string.Empty;
			if (BackColor != Color.Empty)
				backColor = Utils.Color2Hex(BackColor);

			string borderColor = string.Empty;
			if (BorderColor != Color.Empty)
				borderColor = Utils.Color2Hex(BorderColor);

			string backColorClicked = string.Empty;
			if (BackColorClicked != Color.Empty)
				backColorClicked = Utils.Color2Hex(BackColorClicked);

			string borderColorRollOver = string.Empty;
			if (BorderColorRollOver != Color.Empty)
				borderColorRollOver = Utils.Color2Hex(BorderColorRollOver);	

			string backImage = string.Empty;
			if (BackImage != string.Empty)
			{
				if (Parent != null && Parent is ActiveUp.WebControls.Toolbar)
					backImage = Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory,BackImage);
				else
					backImage = BackImage;
			}

			string backImageClicked = string.Empty;
			if (BackImageClicked != string.Empty)
			{
				if (Parent != null && Parent is ActiveUp.WebControls.Toolbar)
					backImageClicked = Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory,BackImageClicked);

				else
					backImageClicked = BackImageClicked;
			}

			output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Value,string.Format("{0},{1},{2},{3},{4},{5}",backColor,backImage,borderColor,BorderStyle,BorderWidth,margin.ToString()));
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_MouseOut");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_MouseOut");
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Value,string.Format("{0},{1},{2},{3},{4},{5}",backColorClicked,backImageClicked,borderColorRollOver,BorderStyleRollOver,BorderWidthRollOver,marginRollOver.ToString()));
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_MouseDown");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_MouseDown");
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			string backImageRollOver = string.Empty;
			string backColorRollOver = string.Empty;
			if (AllowRollOver)
			{
				if (BackImageRollOver != string.Empty)
				{
					if (Parent != null && Parent is ActiveUp.WebControls.Toolbar)
						backImageRollOver = Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory,BackImageRollOver);
					else
						backImageRollOver = BackImageRollOver;
				}

				if (BackColorRollOver != Color.Empty)
					backColorRollOver = Utils.Color2Hex(BackColorRollOver);

				output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
				output.AddAttribute(HtmlTextWriterAttribute.Value,string.Format("{0},{1},{2},{3},{4},{5}",backColorRollOver,backImageRollOver,borderColorRollOver,BorderStyleRollOver,BorderWidthRollOver,marginRollOver.ToString()));
				output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_MouseOver");
				output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_MouseOver");
				output.RenderBeginTag(HtmlTextWriterTag.Input);
				output.RenderEndTag();

				output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
				output.AddAttribute(HtmlTextWriterAttribute.Value,string.Format("{0},{1},{2},{3},{4},{5}",backColorRollOver,backImageRollOver,borderColorRollOver,BorderStyleRollOver,BorderWidthRollOver,marginRollOver.ToString()));
				output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_MouseUp");
				output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_MouseUp");
				output.RenderBeginTag(HtmlTextWriterTag.Input);
				output.RenderEndTag();

				output.AddAttribute("onmouseover",string.Format("ATB_toolButtonMouseOver('{0}');ATB_toolButtonDownMouseOver('{0}');",ClientID));
			}

			output.AddAttribute("onmouseout",string.Format("ATB_toolButtonMouseOut('{0}');ATB_toolButtonDownMouseOut('{0}');",ClientID));
			output.AddAttribute("onmousedown",string.Format("ATB_toolButtonMouseDown('{0}');ATB_toolButtonDownMouseDown('{0}');",ClientID));
			output.AddAttribute("onmouseup",string.Format("ATB_toolButtonMouseUp('{0}');ATB_toolButtonDownMouseUp('{0}');",ClientID));
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Border,"0");

			output.AddAttribute("onselectstart", "return false");

			string onclick = string.Empty;

			if (this.ClientSideClick != string.Empty)
				onclick += this.ClientSideClick;

			if (this.Click != null)
				onclick += this.Page.GetPostBackEventReference(this);
			
			if (onclick != string.Empty)
				output.AddAttribute(HtmlTextWriterAttribute.Onclick,onclick);

			output.AddStyleAttribute("margin",margin.ToString());
			output.AddStyleAttribute("cursor","hand");
			output.AddAttribute(HtmlTextWriterAttribute.Id,ClientID + "_toolButtonTable");

			output.RenderBeginTag(HtmlTextWriterTag.Table);
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			
			string fullImagePath = string.Empty;

			if (Text != string.Empty && TextAlign == TextAlign.Left)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.Write(Text);
				output.RenderEndTag();
			}

			if (Text == string.Empty || ImageURL != string.Empty)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Td);

				if (Parent != null && Parent is ActiveUp.WebControls.Toolbar)
				{
					fullImagePath = Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory,this.ImageURL);
				}
				else
					fullImagePath = ImageURL;

				string clientID = this.ClientID.Replace(":", "_");

				this.ControlStyle.AddAttributesToRender(output);
				Unit borderWidth = base.BorderWidth;
				if (borderWidth.IsEmpty)
					output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				output.AddAttribute(HtmlTextWriterAttribute.Name, clientID);
				output.AddAttribute(HtmlTextWriterAttribute.Alt, this.ToolTip);
				output.AddAttribute(HtmlTextWriterAttribute.Src, fullImagePath);

				output.RenderBeginTag(HtmlTextWriterTag.Img);
				output.RenderEndTag();

				output.RenderEndTag();
			}

			if (Text != string.Empty && TextAlign == TextAlign.Right)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.Write(Text);
				output.RenderEndTag();
			}

			output.RenderEndTag();
			output.RenderEndTag();

			output.RenderEndTag();

			if (DropImage != string.Empty)
			{

				output.RenderBeginTag(HtmlTextWriterTag.Td);
			
				output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
				output.AddAttribute(HtmlTextWriterAttribute.Value,string.Format("{0},{1},{2},{3},{4},{5}",backColor,backImage,borderColor,BorderStyle,BorderWidth,margin.ToString()));
				output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_DownMouseOut");
				output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_DownMouseOut");
				output.RenderBeginTag(HtmlTextWriterTag.Input);
				output.RenderEndTag();

				if (AllowRollOver)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
					output.AddAttribute(HtmlTextWriterAttribute.Value,string.Format("{0},{1},{2},{3},{4},{5}",backColorRollOver,backImageRollOver,borderColorRollOver,BorderStyleRollOver,BorderWidthRollOver,marginRollOver.ToString()));
					output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_DownMouseOver");
					output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_DownMouseOver");
					output.RenderBeginTag(HtmlTextWriterTag.Input);
					output.RenderEndTag();

					output.AddAttribute("onmouseover",string.Format("ATB_toolButtonMouseOver('{0}');ATB_toolButtonDownMouseOver('{0}');",ClientID));
				}

				output.AddAttribute("onmouseout",string.Format("ATB_toolButtonMouseOut('{0}');ATB_toolButtonDownMouseOut('{0}');",ClientID));
				output.AddAttribute("onmousedown","");
				output.AddAttribute("onmouseup",string.Format("ATB_toolButtonMouseUp('{0}');ATB_toolButtonDownMouseUp('{0}');",ClientID));

				output.AddAttribute("onselectstart", "return false");
				output.AddStyleAttribute("margin",margin.ToString());
				output.AddStyleAttribute("cursor","hand");
				output.AddStyleAttribute("padding","0px 0px 0px 0px");
				output.AddAttribute(HtmlTextWriterAttribute.Border,"0");
				output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
				output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0");
				output.AddAttribute(HtmlTextWriterAttribute.Id,ClientID + "_toolButtonDownTable");
				output.RenderBeginTag(HtmlTextWriterTag.Table);
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.AddStyleAttribute("padding","0px 0px 0px 0px");
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				if (Parent != null && Parent is ActiveUp.WebControls.Toolbar)
				{
					fullImagePath = Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory,DropImage);
				}
				else
					fullImagePath = DropImage;
				output.AddAttribute(HtmlTextWriterAttribute.Src,fullImagePath);
				output.RenderBeginTag(HtmlTextWriterTag.Img);
				output.RenderEndTag();
				output.RenderEndTag();
				output.RenderEndTag();
				output.RenderEndTag();

				output.RenderEndTag();
			}

			output.RenderEndTag();
			output.RenderEndTag();*/

			if (Items.Count > 0)
				RenderItems(output);

			
		}

		private void RenderItems(HtmlTextWriter output)
		{
			// render the left back images
			output.AddStyleAttribute("position","absolute");
			//output.AddStyleAttribute("display","none");
			output.AddStyleAttribute("left","0px");
			output.AddStyleAttribute("top","100px");
			output.AddAttribute(HtmlTextWriterAttribute.Id,ClientID + "_dropMenuBackgroundImage");
			output.RenderBeginTag(HtmlTextWriterTag.Div);

			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0");
			output.RenderBeginTag(HtmlTextWriterTag.Table);

			for(int i = 0 ; i <  Items.Count ; i++)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				
				output.AddAttribute(HtmlTextWriterAttribute.Id,ClientID + "_dropMenuItemImg" + i.ToString());
				string leftBackImage = string.Empty;
				/*if (Parent != null && Parent is ActiveUp.WebControls.Toolbar)
					leftBackImage = Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory,LeftBackImage);
				else
					leftBackImage = LeftBackImage;					*/
				output.AddAttribute(HtmlTextWriterAttribute.Src,leftBackImage);
				output.RenderBeginTag(HtmlTextWriterTag.Img);
				output.RenderEndTag();

				output.RenderEndTag();
				output.RenderEndTag();
			}

			output.RenderEndTag();
			output.RenderEndTag();

			// render the items
			/*output.AddStyleAttribute("position","absolute");
			output.AddStyleAttribute("display","none");
			output.AddStyleAttribute("left","0px");
			output.AddStyleAttribute("top","0px");
			output.AddAttribute(HtmlTextWriterAttribute.Id,ClientID + "_dropMenuItems");
			output.RenderBeginTag(HtmlTextWriterTag.Div);
			output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor,Utils.Color2Hex(this.WindowBorderColor));
			output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, this.WindowBorderStyle.ToString());
			output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, this.WindowBorderWidth.ToString());
			output.AddStyleAttribute("cursor","hand");
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0");
			output.AddAttribute("onselectstart","return false;");
			output.AddAttribute(HtmlTextWriterAttribute.Id,ClientID + "_dropMenuTableItems");
			output.RenderBeginTag(HtmlTextWriterTag.Table);
			
			for(int i = 0 ; i < Items.Count ; i++)
			{
				string padding = string.Empty;
				if (i == 0)
					padding = "2px 2px 0px 2px";
				else if (i+1 == Items.Count)
					padding = "0px 2px 2px 2px";
				else if (i == 1 && Items.Count == 1)
					padding = "2px 2px 2px 2px";
				else
					padding = "0px 2px 0px 2px";
				output.AddStyleAttribute("padding",padding);
				output.AddAttribute(HtmlTextWriterAttribute.Valign,"middle");
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.RenderBeginTag(HtmlTextWriterTag.Td);

				output.AddStyleAttribute("padding","0px 0px 0px 0px");
				output.AddStyleAttribute("margin","1px");
				output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
				output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0");
				output.AddAttribute(HtmlTextWriterAttribute.Id,ClientID + "_dropMenuItem" + i.ToString());
                output.RenderBeginTag(HtmlTextWriterTag.Table);
				output.RenderBeginTag(HtmlTextWriterTag.Tr);

				output.AddAttribute(HtmlTextWriterAttribute.Id,ClientID + "_dropMenuImage" + i.ToString());
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				if (Items[i].Image != string.Empty)
				{
					string imageItem = string.Empty;
					if (Parent != null && Parent is ActiveUp.WebControls.Toolbar)
						imageItem = Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory,Items[i].Image);
					else
						imageItem = Items[i].Image;		

					output.AddAttribute(HtmlTextWriterAttribute.Src,imageItem);
					output.RenderBeginTag(HtmlTextWriterTag.Img);
					output.RenderEndTag();
				}
				output.RenderEndTag();

				output.AddStyleAttribute(HtmlTextWriterStyle.Width,"100%");
				output.AddStyleAttribute("padding","0px 0px 0px 10px");
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.Write(Items[i].Text);
				output.RenderEndTag();

				output.RenderEndTag();
				output.RenderEndTag();

				output.RenderEndTag();
				output.RenderEndTag();
			}

			output.RenderEndTag();
			output.RenderEndTag();*/

		}

		/// <summary>
		/// Sends the Popup content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
			if (this.Enabled)
			{
				RenderButtonMenu(output);
				
			}

		}
		
		/// <summary>
		/// Notifies the tool to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{
			if (base.Page != null)
				Page.RegisterRequiresPostBack(this);
			base.OnPreRender(e);
		
		}

		#endregion

		#region Events
		
		/// <summary>
		/// The Click event handler. Fire when you click on a node.
		/// </summary>
		[Category("Event")]
		public event EventHandler Click;

		/// <summary>
		/// A OnClick event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnClick(EventArgs e) 
		{
			if (Click != null)
				Click(this,e);
		}
		
		#endregion

		#region Interface IPostBack

		/// <summary>
		/// A RaisePostBackEvent.
		/// </summary>
		/// <param name="eventArgument">eventArgument</param>
		public void RaisePostBackEvent(String eventArgument)
		{
			OnClick(EventArgs.Empty);
		}

		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{
			return false;			
		}

		/// <summary>
		/// When implemented by a class, signals the server control object to notify the ASP.NET application that the state of the
		/// control has changed.
		/// </summary>
		public virtual void RaisePostDataChangedEvent() 
		{
		}

		#endregion			

		#region ViewState

		/// <summary>
		/// Loads the saved view state.
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

		#endregion
	}
}