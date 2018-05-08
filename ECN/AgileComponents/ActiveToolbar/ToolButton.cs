// ActiveToolbar 1.x
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.
#if !FX1_1
#define NOT_FX1_1
#endif

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a button.
	/// </summary>
	[
		Serializable,
		ToolboxItem(false)
	]
	public class ToolButton : ToolBase,INamingContainer ,IPostBackEventHandler, IPostBackDataHandler
	{
		protected internal const string ClientSideEditorId = "$EDITOR_ID$";
		protected internal const string ToolText = "_tool";
		private const string JavaScriptOpenTag = "<script language='javascript'>\n";
		private const string Normal = "Normal";
		private const string Zero = "0";
		private const string One = "1";
		private const string JavaScriptEmpty = "<script language='javascript'></script>";
		private const string NotSet1Px = ",,,NotSet,,1px";
		private const string ScriptClose = "</script>";
		private Popup _popupContents; 
		private bool _checked = false;		
		private readonly string TOOLBUTTONKEY = "ToolButton";

		private Unit margin;
		private string backImage = string.Empty;
		private string backImageClicked = string.Empty;
			

		#region Constructors

		 /// <summary>
		 /// Initializes a new instance of the <see cref="ToolButton"/> class.
		 /// </summary>
		 public ToolButton() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolButton"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolButton(string id) : base(id)
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
		public virtual string ImageURL
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
		/// The relative link to the image to use when the mouse is over the tool.
		/// </summary>
		[
		Bindable(true), 
		Category("Appearance"), 
		DefaultValue(""),
		Description("The relative link to the image to use when the mouse is over the tool.")
		] 
		public virtual string OverImageURL
		{
			get
			{
				string _overImageURL;
				_overImageURL = ((string) base.ViewState["_overImageURL"]);
				if (_overImageURL != null)
				{
					return _overImageURL; 
				}
				return string.Empty;
			}
			set
			{
				ViewState["_overImageURL"] = value;
			}
		}

		/// <summary>
		/// Target frame of the link.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("Target frame of the link.")
		] 
		public string NavigateURL
		{
			get
			{
				string _navigateURL;
				_navigateURL = ((string) base.ViewState["_navigateURL"]);
				if (_navigateURL != null)
				{
					return _navigateURL; 
				}
				return string.Empty;
			}
			set
			{
				ViewState["_navigateURL"] = value;
			}
		}

		/// <summary>
		/// Target frame of the link.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("Target frame of the link.")
		] 
		public string Target
		{
			get
			{
				string _target;
				_target = ((string) ViewState["_target"]);
				if (_target != null)
				{
					return _target; 
				}
				return string.Empty; 
			}
			set
			{
				ViewState["_target"] = value;
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
		/// Gets or sets the <see cref="Popup"/> associated with the tool.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(null),
		Browsable(true),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)
		]
		public virtual Popup PopupContents
		{
			get
			{
				if (_popupContents == null)
				{
					_popupContents = new Popup();
				}
				return _popupContents;
			}

			set {_popupContents = value;}

		}

		/// <summary>
		/// Indicates if you want to use the <see cref="Popup"/> when a click occurs on the tool.
		/// </summary>
		[
		Bindable(true),
		Category("Event"),
		DefaultValue(false),
		Browsable(true),
		NotifyParentProperty(true),
		Description("Indicates if you want to use the Popup when a click occurs on the tool.")
		]
		public bool UsePopupOnClick
		{
			get
			{
				object usePopupOnClick;
				usePopupOnClick = ViewState["_usePopupOnClick"];
				if (usePopupOnClick != null)
					return (bool)usePopupOnClick;
				else
					return false;
			}

			set
			{
				ViewState["_usePopupOnClick"] = value;
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
		/// Indicates the type of the tool.
		/// </summary>
		[
			Bindable(true),
			Category("Behavior"),
			DefaultValue(typeof(ToolButtonType),"Normal"),
			Browsable(true),
			NotifyParentProperty(true),
			Description("Indicates the type of the tool.")
		]
		public ToolButtonType Type
		{
			get
			{
				object type = ViewState["_type"];
				if (type != null)
					return (ToolButtonType)type;
				else
					return ToolButtonType.Normal;
			}

			set
			{
				ViewState["_type"] = value;
			}
        }

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="ToolButton"/> control is checked.
		/// </summary>
		[
		Bindable(true), 
		DefaultValue(false),
		Description("A value indicating whether the ToolButton control is checked."),
		Category("Data")
		] 
		public bool Checked
		{
			get
			{
				object isChecked = ViewState["_checked"];
				if (isChecked != null)
				{
					return (bool)isChecked;
				}
				else
					return false;
			}

			set
			{
				ViewState["_checked"] = value;
			}
		}

		/// <summary>
		/// Group name used only when you use the tool as checkbox.
		/// </summary>
		[
			Bindable(true),
			DefaultValue(""),
			Category("Behavior"),
			Description("Group name used only when you use the tool as checkbox.")
		]
		public string GroupName
		{
			get
			{
				object groupName = ViewState["_groupName"];
				if (groupName != null)
					return (string)groupName;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_groupName"] = value;
			}
		}

		#endregion

		#region Methods
		/// <summary>
		/// Render the tool to the specified HtmlTextWriter object. Usually a Page.
		/// </summary>
		protected virtual void RenderImage(HtmlTextWriter output)
		{
			if (Type == ToolButtonType.Checkbox)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
				output.AddAttribute(HtmlTextWriterAttribute.Value,Checked.ToString());
				output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_Checked");
				output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_Checked");
				output.RenderBeginTag(HtmlTextWriterTag.Input);
				output.RenderEndTag();

				output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
				output.AddAttribute(HtmlTextWriterAttribute.Value,GroupName);
				output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_GroupName");
				output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_GroupName");
				output.RenderBeginTag(HtmlTextWriterTag.Input);
				output.RenderEndTag();
			}

			output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Value,"True");
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_Enabled");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_Enabled");
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			if (AllowRollOver /*&& this.OverImageURL != string.Empty*/)
				output.AddAttribute("onmouseover",string.Format("ATB_toolButtonMouseOver('{0}');",ClientID));
	
			output.AddAttribute("onmouseout",string.Format("ATB_toolButtonMouseOut('{0}');",ClientID));
			output.AddAttribute("onmousedown",string.Format("ATB_toolButtonMouseDown('{0}');",ClientID));
			output.AddAttribute("onmouseup",string.Format("ATB_toolButtonMouseUp('{0}');",ClientID));
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Border,"0");

			output.AddAttribute("onselectstart", "return false");

			string onclick = string.Empty;

			if (!UsePopupOnClick)
			{
				if (this.ClientSideClick != string.Empty)
					onclick += this.ClientSideClick;

				if (this.Click != null)
					onclick += this.Page.GetPostBackEventReference(this);

			}
			else
			{
				if (this.ClientSideClick != string.Empty)
					onclick += this.ClientSideClick;
				onclick += string.Format("return ATB_showPopup('{0}');",PopupContents.ID);

			}

			if (onclick != string.Empty)
				output.AddAttribute(HtmlTextWriterAttribute.Onclick,onclick);

			output.AddStyleAttribute("margin",margin.ToString());

            if (System.Web.HttpContext.Current != null)
            {
                if (Page.Request.Browser.Browser.ToUpper() == "IE")
                    output.AddStyleAttribute("cursor", "hand");
                else
                    output.AddStyleAttribute("cursor", "pointer");
            }
			output.AddAttribute(HtmlTextWriterAttribute.Id,ClientID + "_toolButtonTable");

			if (Checked)
			{
				if (BackColorClicked != Color.Empty)
					output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor,Utils.Color2Hex(BackColorClicked));

				if (backImageClicked != string.Empty)
				{
					output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, backImageClicked);
				}
				
			}

			if (backImage != string.Empty)
				output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage,backImage);

			output.RenderBeginTag(HtmlTextWriterTag.Table);
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			
			if (Text != string.Empty && TextAlign == TextAlign.Left)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.Write(Text);
				output.RenderEndTag();
			}

			if (Text == string.Empty || ImageURL != string.Empty)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Td);

				string fullImagePath = string.Empty;
				if (Parent != null && Parent is ActiveUp.WebControls.Toolbar)
				{
					PopupContents.ImagesDirectory = ((Toolbar)Parent).ImagesDirectory;
					fullImagePath = Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory,this.ImageURL);
				}
				else
					fullImagePath = ImageURL;
#if (!FX1_1)
                if (fullImagePath == string.Empty)
                {
                    fullImagePath = Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory == string.Empty ? string.Empty : ((Toolbar)Parent).ImagesDirectory, ImageURL, "button.gif", Page, this.GetType());
                }
#endif
				string clientID = this.ClientID.Replace(":", "_");

				//this.ClientSideClick += "this.disabled=true;";

				if (this.NavigateURL != string.Empty)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Href, this.NavigateURL);
					if (this.Target != string.Empty)
						output.AddAttribute(HtmlTextWriterAttribute.Target, this.Target);
					output.RenderBeginTag(HtmlTextWriterTag.A); // A
				}

				this.ControlStyle.AddAttributesToRender(output);
				Unit borderWidth = base.BorderWidth;
				if (borderWidth.IsEmpty)
					output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				output.AddAttribute(HtmlTextWriterAttribute.Name, clientID);
				output.AddAttribute(HtmlTextWriterAttribute.Alt, this.ToolTip);
				output.AddAttribute(HtmlTextWriterAttribute.Src, fullImagePath);

		
				if (AllowRollOver && Text == string.Empty && OverImageURL != string.Empty)
                    if (Parent != null && Parent is ActiveUp.WebControls.Toolbar)
                    {
                        string imageOver = string.Empty;

                        if (((Toolbar)Parent).ImagesDirectory == string.Empty)
                            imageOver = OverImageURL;
                        else
                            imageOver = Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory, this.OverImageURL);

                        output.AddAttribute("onmouseover", "ATB_swapButton('" + clientID + "', '" + imageOver + "');");
                    }
                    else
                        output.AddAttribute("onmouseover", "ATB_swapButton('" + clientID + "', '" + this.OverImageURL + "');");

                if (Parent != null && Parent is ActiveUp.WebControls.Toolbar)
                {
                    string imageURL = string.Empty;

                    
#if (!FX1_1)
                    if (imageURL.TrimEnd() == string.Empty)
                    {
                        imageURL = Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory == string.Empty ? string.Empty : ((Toolbar)Parent).ImagesDirectory, ImageURL, "button.gif", Page, this.GetType());
                    }
#else
                    if (((Toolbar)Parent).ImagesDirectory == string.Empty)
                        imageURL = ImageURL;
                    else
                        imageURL = Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory, this.ImageURL);
#endif
                   output.AddAttribute("onmouseout", "ATB_swapButton('" + clientID + "', '" + imageURL + "');");
                }
                else
                    output.AddAttribute("onmouseout", "ATB_swapButton('" + clientID + "', '" + this.ImageURL + "');");

				output.RenderBeginTag(HtmlTextWriterTag.Img);
				output.RenderEndTag();

				if (this.NavigateURL != string.Empty)
					output.RenderEndTag(); // /A

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
		}

		/// <summary>
		/// Renders disabled image.
		/// </summary>
		/// <param name="output">The output.</param>
		protected void RenderDisableImage(HtmlTextWriter output)
		{
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Border,"0");
			output.AddAttribute("onselectstart", "return false");
			output.AddStyleAttribute("margin",margin.ToString());
			output.AddStyleAttribute("cursor","normal");
			output.AddStyleAttribute("filter","alpha(opacity=30)");
			output.AddStyleAttribute("opacity",".30");
			output.AddAttribute(HtmlTextWriterAttribute.Id,ID + "_toolButtonTable");
			if (backImage != string.Empty)
				output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage,backImage);

			output.RenderBeginTag(HtmlTextWriterTag.Table);
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			
			if (Text != string.Empty && TextAlign == TextAlign.Left)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.Write(Text);
				output.RenderEndTag();
			}

			if (Text == string.Empty || ImageURL != string.Empty)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Td);

				string fullImagePath = string.Empty;
				if (Parent != null && Parent is ActiveUp.WebControls.Toolbar)
				{
					PopupContents.ImagesDirectory = ((Toolbar)Parent).ImagesDirectory;
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
				output.AddAttribute(HtmlTextWriterAttribute.Src, fullImagePath);

				output.RenderBeginTag(HtmlTextWriterTag.Img);
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
		}

		/// <summary>
		/// Sends the Popup content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
			if (this.Enabled)
			{
				this.RenderImage(output);
					
				if (UsePopupOnClick)
				{
					this.PopupContents.EnableSsl = this.EnableSsl;
#if (!FX1_1)
                    if (this.PopupContents.CloseImage == string.Empty)
                    {
                        this.PopupContents.CloseImage = Utils.ConvertToImageDir(this.PopupContents.ImagesDirectory, "", "close.gif", this.Page, this.GetType());
                    }
#endif
					this.PopupContents.RenderControl(output);
				}
			}
			else
			{
				RenderDisableImage(output);
			}

		}
		
		/// <summary>
		/// Notifies the tool to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			RegisterScriptBlock();

			Page?.RegisterRequiresPostBack(this);

			var scriptKey = $"{ClientID}_{TOOLBUTTONKEY}_Init";
			var initValues = new StringBuilder();
			initValues.Append(JavaScriptOpenTag);
			// Type
			if(Type.ToString() != Normal)
			{
				initValues.Append($"var {ClientID}_Type = \'{Type}\';\n");
			}

			// Allow Roll Over
			initValues.Append($"var {ClientID}_AllowRollOver = \'{AllowRollOver}\';\n");

			margin = new Unit(Zero);
			var marginRollOver = new Unit(Zero);

			if(BorderWidth.Value == BorderWidthRollOver.Value)
			{
				margin = new Unit(One);
				marginRollOver = new Unit(One);
			}
			else if(BorderWidth.Value < BorderWidthRollOver.Value)
			{
				margin = new Unit((BorderWidthRollOver.Value - BorderWidth.Value + 1).ToString());
				marginRollOver = new Unit(One);
			}
			else if(BorderWidth.Value > BorderWidthRollOver.Value)
			{
				margin = new Unit(One);
				marginRollOver = new Unit((BorderWidth.Value - BorderWidthRollOver.Value + 1).ToString());
			}

			var backColor = string.Empty;
			if(BackColor != Color.Empty) { backColor = Utils.Color2Hex(BackColor); }

			var borderColor = string.Empty;
			if(BorderColor != Color.Empty) { borderColor = Utils.Color2Hex(BorderColor); }

			var backColorClicked = string.Empty;
			if(BackColorClicked != Color.Empty) { backColorClicked = Utils.Color2Hex(BackColorClicked); }

			var borderColorRollOver = string.Empty;
			if(BorderColorRollOver != Color.Empty) { borderColorRollOver = Utils.Color2Hex(BorderColorRollOver); }

			if(BackImage != string.Empty)
			{
				var parentToolbar = Parent as Toolbar;
				backImage = parentToolbar != null ? $"url({Utils.ConvertToImageDir(parentToolbar.ImagesDirectory, BackImage)})" : $"url({BackImage})";
			}

			if(BackImageClicked != string.Empty)
			{
				var parentToolbar = Parent as Toolbar;
				backImageClicked = parentToolbar != null ? $"url({Utils.ConvertToImageDir(parentToolbar.ImagesDirectory, BackImageClicked)})" : $"url({BackImageClicked})";
			}

			PreRenderMouseEvents(backColor, borderColor, initValues, backColorClicked, borderColorRollOver, marginRollOver);

			initValues.Append(ScriptClose);

			if(initValues.ToString().Replace("\n", string.Empty) != JavaScriptEmpty)
			{
				Page.RegisterStartupScript(scriptKey, initValues.ToString());
			}

			base.OnPreRender(e);
		}

		private void PreRenderMouseEvents(string backColor, string borderColor, StringBuilder initValues, string backColorClicked, string borderColorRollOver, Unit marginRollOver)
		{
			var mouseOut = $"{backColor},{backImage},{borderColor},{BorderStyle},{BorderWidth},{margin}";
			if(mouseOut != NotSet1Px) { initValues.Append($"var {ClientID}_MouseOut = \'{mouseOut}\';\n"); }

			var mouseDown = $"{backColorClicked},{backImageClicked},{borderColorRollOver},{BorderStyleRollOver},{BorderWidthRollOver},{marginRollOver}";
			if(mouseDown != NotSet1Px) { initValues.Append($"var {ClientID}_MouseDown = \'{mouseDown}\';\n"); }

			if(AllowRollOver)
			{
				var backImageRollOver = string.Empty;
				if(BackImageRollOver != string.Empty)
				{
					var parentToolbar = Parent as Toolbar;
					backImageRollOver = parentToolbar != null ? $"url({Utils.ConvertToImageDir(parentToolbar.ImagesDirectory, BackImageRollOver)})" : $"url({BackImageRollOver})";
				}

				var backColorRollOver = string.Empty;

				if(BackColorRollOver != Color.Empty) { backColorRollOver = Utils.Color2Hex(BackColorRollOver); }

				var mouseOver = $"{backColorRollOver},{backImageRollOver},{borderColorRollOver},{BorderStyleRollOver},{BorderWidthRollOver},{marginRollOver}";
				if(mouseOver != NotSet1Px) { initValues.Append($"var {ClientID}_MouseOver = \'{mouseOver}\';\n"); }

				var mouseUp = $"{backColorRollOver},{backImageRollOver},{borderColorRollOver},{BorderStyleRollOver},{BorderWidthRollOver},{marginRollOver}";
				if(mouseUp != NotSet1Px) { initValues.Append($"var {ClientID}_MouseUp = \'{mouseUp}\';\n"); }
			}
		}

		protected void InitToolButton(string controlId, string imageUrl, string overImageUrl, string toolTip, bool usePopupOnClick)
		{
			this.ID = controlId;
			this.ImageURL = imageUrl;
			this.OverImageURL = overImageUrl;
			this.ToolTip = toolTip;
			this.UsePopupOnClick = usePopupOnClick;
		}

		protected void SetPopupContents(int height, int width, bool autoContent, bool showShadow)
		{
			this.PopupContents.ID = $"{ID}{nameof(Popup)}";
			this.PopupContents.TitleText = ToolTip;
			SetContentsHeightWidthAutoContent(height, width, autoContent);
			this.PopupContents.ShowShadow = showShadow;
		}

		protected void SetContentsHeightWidthAutoContent(int height, int width, bool? autoContent = null)
		{
			this.PopupContents.Height = height;
			this.PopupContents.Width = width;

			if(autoContent.HasValue)
			{
				this.PopupContents.AutoContent = autoContent.Value;
			}
		}

		private void RegisterScriptBlock()
		{
			if (Type == ToolButtonType.Checkbox && GroupName != string.Empty)
			{

				if (!Page.IsClientScriptBlockRegistered(TOOLBUTTONKEY + "_" + ClientID))
				{

					string CLIENTSIDE_API = string.Empty;

					CLIENTSIDE_API += "<script language=\"javascript\">\n";
					CLIENTSIDE_API += string.Format("function Init_{0}(e)\n",ClientID);
					CLIENTSIDE_API += "{\n";
					CLIENTSIDE_API += string.Format("ATB_addGroupName('{0}');\n",GroupName);
					CLIENTSIDE_API += "\n}\n";
					CLIENTSIDE_API += string.Format("window.RegisterEvent(\"onload\", Init_{0});\n",ClientID);
					CLIENTSIDE_API += "\n</script>\n";
				
					Page.RegisterStartupScript(TOOLBUTTONKEY + "_" + ClientID, CLIENTSIDE_API);
				}
			}
		}

		#endregion

		#region Events
		
		/// <summary>
		/// The Click event handler. Fire when you click on a node.
		/// </summary>
		[Category("Event")]
		public event EventHandler Click;

		/// <summary>
		/// The CheckedChanged event handler. Fire when state of the checkbox change.
		/// </summary>
		[Category("Event")]
		public event EventHandler CheckedChanged;		

		/// <summary>
		/// A OnClick event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnClick(EventArgs e) 
		{
			if (Click != null)
				Click(this,e);
		}

		/// <summary>
		/// Raises the checked changed event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnCheckedChanged(EventArgs e)
		{
			if (CheckedChanged != null)
				CheckedChanged(this,e);
		}

		#endregion

		#region Interface IPostBack

		/// <summary>
		/// A RaisePostBackEvent.
		/// </summary>
		/// <param name="eventArgument">eventArgument</param>
		public void RaisePostBackEvent(String eventArgument)
		{
			Page.Trace.Write(ID,"RaisePostBackEvent");
			OnClick(EventArgs.Empty);
		}

		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{
			Page.Trace.Write(ID,"LoadPostData");
			if (Type == ToolButtonType.Checkbox)
			{
				_checked = bool.Parse(postCollection[UniqueID + "_Checked"]);
				bool result = true;

				if (Checked == _checked)
					result =  false;
				else
					Checked = _checked;

				return result;
			}

			return false;
		}

		/// <summary>
		/// When implemented by a class, signals the server control object to notify the ASP.NET application that the state of the
		/// control has changed.
		/// </summary>
		public virtual void RaisePostDataChangedEvent() 
		{
			if (Type == ToolButtonType.Checkbox)
				OnCheckedChanged(EventArgs.Empty);
		}


		public void SetImageUrl(string imageUrlResourceName, string overImageUrlResourceName)
		{
			SetImageUrlNotFx1(imageUrlResourceName, overImageUrlResourceName);
		}

		[Conditional("NOT_FX1_1")]
		public void SetImageUrlNotFx1(string imageUrlResourceName, string overImageUrlResourceName)
		{
			if (ImageURL == string.Empty)
			{
				this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), imageUrlResourceName);
			}
			if (OverImageURL == string.Empty)
			{
				this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), overImageUrlResourceName);
			}
		}

		protected void ToolOnPreRender(string imageUrlResourceName, string overImageUrlResourceName, string clientId)
		{
			this.ClientSideClick = this.ClientSideClick.Replace(ClientSideEditorId, clientId);
			SetImageUrl(imageUrlResourceName, overImageUrlResourceName);
		}
		#endregion
	}
}
