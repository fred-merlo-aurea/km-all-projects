using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections;
using System.IO;
using ActiveUp.WebControls.Common;


namespace ActiveUp.WebControls
{
	#region Popup

	/// <summary>
	/// Represents a Popup control.
	/// </summary>
	[
		ToolboxData("<{0}:Popup runat=server></{0}:Popup>"),
		Designer(typeof(PopupDesigner)),
		TypeConverter(typeof(ExpandableObjectConverter)),
        ToolboxBitmap(typeof(Popup), "ToolBoxBitmap.Popup.bmp"),
		Serializable 
	]
	public class Popup : CoreWebControl
	{
		#region Variables

		/// <summary>
		/// Layout of the window.
		/// </summary>
		private LayoutPopupWindow _layoutWindow = new LayoutPopupWindow();

		/// <summary>
		/// Layout of the title.
		/// </summary>
		private LayoutPopupTitle _layoutTitle = new LayoutPopupTitle();

		/// <summary>
		/// Layout of the contents.
		/// </summary>
		private LayoutPopupContent _layoutContent = new LayoutPopupContent();

		/// <summary>
		/// Unique client script key.
		/// </summary>
		private const string SCRIPTKEY = "ActiveToolbar";

		/// <summary>
		/// 
		/// </summary>
		private string CLIENTSIDE_API = null;
		private string _scriptDirectory;

#if (LICENSE)
		private static int _useCounter;
		//private string _license = string.Empty;
#endif

		#endregion

		#region Constructor

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Popup()
		{
			//ImagesDirectory = "icons/";
#if (!FX1_1)
            ImagesDirectory = string.Empty;
            _scriptDirectory = string.Empty;
            CloseImage = string.Empty;
            ResizeImage = string.Empty;
#else
            ImagesDirectory = Define.IMAGES_DIRECTORY;
			_scriptDirectory = Define.SCRIPT_DIRECTORY;
			CloseImage = "close.gif";
			ResizeImage = "resize.gif";
#endif

            
		}

		#endregion

		#region Properties
/*
#if (LICENSE)
		/// <summary>
		/// Gets or sets the license key.
		/// </summary>
		[
		Bindable(false),
		Category("Data"),
		Description("Lets you specify the license key.")
		]
		public string License
		{
			get
			{
				return _license;
			}
		
			set
			{
				_license = value;
			}
		}
#endif
*/
		/// <summary>
		/// Gets or sets the relative or absolute path to the directory where the API javascript file is.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the control render.</remarks>
		[Bindable(false),
		Category("Behavior"),
		Description("Gets or sets the relative or absolute path to the directory where the API javascript file is."),
#if (!FX1_1)
        DefaultValue("")
#else
		DefaultValue(Define.SCRIPT_DIRECTORY)
#endif
		]
		public string ScriptDirectory
		{
			get
			{
                if (_scriptDirectory == null)
#if (!FX1_1)
                    return string.Empty;
#else
					return Define.SCRIPT_DIRECTORY;
#endif
				return _scriptDirectory;
			}
			set
			{
				_scriptDirectory = value;
			}
		}

		/// <summary>
		/// Gets or sets the access key (underlined letter) that allows you to quickly navigate.
		/// </summary>
		[Browsable(false)]
		public override string AccessKey
		{
			get {return base.AccessKey;}
			set {base.AccessKey = value;}
		}

		/// <summary>
		/// Gets or sets the background color.
		/// </summary>
		[Browsable(false)]
		public override Color BackColor
		{
			get {return base.BackColor;}
			set {base.BackColor = value;}
		}

		/// <summary>
		/// Gets or sets the border color.
		/// </summary>
		[Browsable(false)]
		public override Color BorderColor
		{
			get {return base.BorderColor;}
			set {base.BorderColor = value;}
		}

		/// <summary>
		/// Gets or sets the border width.
		/// </summary>
		[Browsable(false)]
		public override Unit BorderWidth
		{
			get {return base.BorderWidth;}
			set {base.BorderWidth = value;}
		}

		/// <summary>
		/// Gets or sets the border style.
		/// </summary>
		[Browsable(false)]
		public override BorderStyle BorderStyle
		{
			get { return base.BorderStyle;}
			set { base.BorderStyle = value;}
		}

		/// <summary>
		/// Gets or sets the value indicates if you want to use the startup position or not.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(false),
		NotifyParentProperty(true),
		Description("value indicating if you want to use the startup position or not.")
		]
		public bool UseStartupPosition
		{
			get
			{
				object useStartupPosition;
				useStartupPosition = ViewState["UseStartupPosition"];
				if (useStartupPosition != null)
				{
					return (bool)useStartupPosition;
				}
				else
					return false;
			}

			set
			{
				ViewState["UseStartupPosition"] = value;
			}
		}

		/// <summary>
		/// Gets or sets if the popup must be transparent when moving.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(false),
		NotifyParentProperty(true),
		Description("If the popup must be transparent when moving.")
		]
		public bool TransparentOnMove
		{
			get
			{
				object transparentOnMove;
				transparentOnMove = ViewState["TransparentOnMove"];
				if (transparentOnMove != null)
				{
					return (bool)transparentOnMove;
				}
				else
					return false;
			}

			set
			{
				ViewState["TransparentOnMove"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the startup position.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(typeof(Point),"0; 0"),
		NotifyParentProperty(true),
		Description("Startup position of the popup.")
		]
		public Point StartupPosition
		{
			get
			{
				object startupPosition;
				startupPosition = ViewState["StartupPosition"];
				if (startupPosition != null)
				{
					return (Point)startupPosition;
				}
				else
					return Point.Empty;
			}

			set
			{
				ViewState["StartupPosition"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(typeof(Unit),""),
		NotifyParentProperty(true),
		Description("Width of the popup.")
		]
		public override Unit Width
		{
			get
			{
				object width;
				width = ViewState["_width"];
				if (width != null)
				{
					if (((Unit)width).Value < 100)
						return new Unit(100);
					else
						return (Unit)width;
				}
				else
					return new Unit(100);
			}

			set
			{
				ViewState["_width"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(typeof(Unit),""),
		NotifyParentProperty(true),
		Description("Height of the popup.")
		]
		public override Unit Height
		{
			get
			{
				object height;
				height = ViewState["_height"];
				if (height != null)
				{
					if (((Unit)height).Value < 80)
						return new Unit(80);
					else
						return (Unit)height;
				}
				else
					return new Unit(80);
				
			}

			set 
			{
				ViewState["_height"] = value;
			}
		}

		/// <summary>
		/// Gets or sets if the popup can be dragged.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(true),
		NotifyParentProperty(true),
		Description("Indicates if the popup can be dragged or not.")
		]
		public bool Dragable
		{
			get
			{
				object dragable;
				dragable = ViewState["_dragable"];
				if (dragable != null)
					return (bool)dragable;
				else
					return true;

			}

			set
			{
				ViewState["_dragable"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the overflow of the popup.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(true),
		NotifyParentProperty(true),
		Description("Gets or sets the overflow of the popup.")
		]
		public OverflowType Overflow
		{
			get
			{
				object overflow;
				overflow = ViewState["_overflow"];
				if (overflow != null)
					return (OverflowType)overflow;
				else
					return OverflowType.Auto;

			}

			set
			{
				ViewState["_overflow"] = value;
			}
		}

		/// <summary>
		/// Gets or sets if the popup must be showed when the application starts.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(false),
		NotifyParentProperty(true),
		Description("Indicates if the popup must be showed when the application starts.")
		]
		public bool ShowedOnStart
		{
			get
			{
				object dragable;
				dragable = ViewState["_showed"];
				if (dragable != null)
					return (bool)dragable;
				else
					return false;

			}

			set
			{
				ViewState["_showed"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the layout of the window.
		/// </summary>
		[
		Bindable(true),
		Category("Style"),
		DefaultValue(typeof(LayoutPopupWindow),""),
		Description("Layout of the window."),		
		NotifyParentProperty(true),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)
		]
		public LayoutPopupWindow LayoutWindow
		{
			get { return _layoutWindow;}
			set	{ _layoutWindow = value;}
		}

		/// <summary>
		/// Gets or sets the title layout.
		/// </summary>
		[
		Bindable(true),
		Category("Style"),
		DefaultValue(typeof(LayoutPopupTitle),""),
		Description("Title layout of the popup."),		
		NotifyParentProperty(true),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)
		]
		public LayoutPopupTitle LayoutTitle
		{
			get { return _layoutTitle;}
			set	{ _layoutTitle = value;}
		}

		/// <summary>
		/// Gets or sets the layout contents.
		/// </summary>
		[
		Bindable(true),
		Category("Style"),
		DefaultValue(typeof(LayoutPopupContent),""),
		Description("Layout contents of the popup."),
		NotifyParentProperty(true),
		PersistenceModeAttribute(PersistenceMode.InnerProperty),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)
		]
		public LayoutPopupContent LayoutContent
		{
			get {return _layoutContent;}
			set {_layoutContent = value;}
		}

		/// <summary>
		/// Gets or sets the filename of the external script file.
		/// </summary>
		public string ExternalScript
		{
			get
			{
				string _externalScript;
				_externalScript = ((string) base.ViewState["_externalScript"]);
				if (_externalScript != null)
					return _externalScript; 
				return string.Empty;
			}
			set
			{
				ViewState["_externalScript"] = value;
			}
		}

		/// <summary>
		/// Indicates if the title must be displayed or not.
		/// </summary>
		[
			Browsable(true),
			DefaultValue(true),
			Category("Behavior"),
			Description("Indicates if the title must be displayed or not.")
		]	
		public bool ShowTitle
		{
			get
			{
				object showTitle = ViewState["_showTitle"];
				if (showTitle != null)
					return (bool)showTitle;
				return true;
			}

			set
			{
				ViewState["_showTitle"] = value;
			}
		}

		/// <summary>
		/// Indicates if the popup must be resized.
		/// </summary>
		[
		Browsable(true),
		DefaultValue(false),
		Category("Behavior"),
		Description("Indicates if the popup must be resized.")
		]	
		public bool AllowResize
		{
			get
			{
				object allowResize = ViewState["_allowResize"];
				if (allowResize != null)
					return (bool)allowResize;
				return false;
			}

			set
			{
				ViewState["_allowResize"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the shadow must be displayed.
		/// </summary>
		/// <value><c>true</c> if the shadow must be displayed; otherwise, <c>false</c>.</value>
		[
		Browsable(true),
		DefaultValue(false),
		Category("Behavior"),
		Description("Indicates if the popup must be displayed the shadow.")
		]	
		public bool ShowShadow
		{
			get
			{
				object showShadow = ViewState["_showShadow"];
				if (showShadow != null)
					return (bool)showShadow;
				return false;
			}

			set
			{
				ViewState["_showShadow"] = value;
			}
		}

		/// <summary>
		/// Indicates if you want to allow drag and drop from the content.
		/// </summary>
		[
			Browsable(true),
			DefaultValue(false),
			Category("Behavior"),
			Description("Indicates if you want to allow drag and drop from the content.")
		]
		public bool DragFromContent
		{
			get
			{
				object dragFromContent = ViewState["_dragFromContent"];
				if (dragFromContent != null)
					return (bool)dragFromContent;
				else
					return false;
			}

			set
			{
				ViewState["_dragFromContent"] = value;
			}
		}

		/// <summary>
		/// Indicates if the window must be displayed or not.
		/// </summary>
		[
			Browsable(true),
			DefaultValue(true),
			Category("Behavior"),
			Description("Indicates if the window must be displayed or not. In case of the window doesn't be displayed, the title doesn't displayed too.")
		]
		public bool ShowWindow
		{
			get
			{
				object showWindow = ViewState["_showWindow"];
				if (showWindow != null)
					return (bool)showWindow;
				else
					return true;
			}

			set
			{
				ViewState["_showWindow"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the title text.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		Description("Title text."),
		DefaultValueAttribute(""),
		NotifyParentProperty(true)
		]
		public string TitleText
		{
			get
			{
				object titleText = ViewState["_titleText"];
				if (titleText != null)
					return (string)titleText;
				else 
					return string.Empty;
			}

			set
			{
				ViewState["_titleText"] = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the content text.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		Description("Content text."),
		DefaultValueAttribute(""),
		NotifyParentProperty(true)
		]
		public string ContentText
		{
			get
			{
				object contentText = ViewState["_contentText"];
				if (contentText != null)
					return (string)contentText;
				else 
					return string.Empty;
			}

			set
			{
				ViewState["_contentText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the directory where all the images are.
		/// </summary>
		[
		Bindable(true), 
		Category("Appearance"),
#if (!FX1_1)
        DefaultValue(""),
#else 
		DefaultValue(Define.IMAGES_DIRECTORY),
#endif
		Description("The directory where all the images are. If they are difference, please leave this empty.")
		] 
		public string ImagesDirectory
		{
			get
			{
				string imagesDirectory;
				imagesDirectory = ((string) base.ViewState["_imagesDirectory"]);
				if (imagesDirectory != null)
					return imagesDirectory; 
#if (!FX1_1)
                return string.Empty;
#else
                return Define.IMAGES_DIRECTORY;
#endif
			}
			set
			{
				ViewState["_imagesDirectory"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the image for the close button of the popup.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			Description("Gets or sets the image for the close button of the popup."),
#if (!FX1_1)			
	DefaultValue("")
#else
			DefaultValue("close.gif")
#endif			
		]
		public string CloseImage
		{
			get
			{
				string closeImage;
				closeImage = ((string) base.ViewState["_closeImage"]);
				if (closeImage != null)
					return closeImage; 
#if (!FX1_1)
	return string.Empty;
#else					
				return "close.gif";
#endif				
			}
			set
			{
				ViewState["_closeImage"] = value;
			}

		}

		/// <summary>
		/// Gets or sets the image for the resize image of the popup.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the image for the resize of the popup."),
#if (!FX1_1)
	DefaultValue("")
#else
	DefaultValue("resize.gif")
#endif	
		]
		public string ResizeImage
		{
			get
			{
				string resizeImage;
				resizeImage = ((string) base.ViewState["_resizeImage"]);
				if (resizeImage != null)
					return resizeImage; 
#if (!FX1_1)
	return string.Empty;
#else					
				return "resize.gif";
#endif				
			}
			set
			{
				ViewState["_resizeImage"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the id of a hidden div to use as content.
		/// </summary>
		[
			Bindable(true),
			Category("Data"),
			DefaultValue(""),
			Description("Gets or sets the id of a hidden div to use as content.")
		]
		public string ContentDivId
		{
			get
			{
				object contentDivId = base.ViewState["_contentDivId"];
				if (contentDivId == null)
					return string.Empty;
				else
					return (string)contentDivId;
			}

			set
			{
				base.ViewState["_contentDivId"] = value;
			}
		}

		/// <summary>
		/// Indicates if the size of the popup depends of the contents.
		/// </summary>
		[
		Browsable(true),
		DefaultValue(false),
		Category("Behavior"),
		Description("Indicates if the size of the popup depends of the contents.")
		]	
		public bool AutoContent
		{
			get
			{
				object autoContent = ViewState["_autoContent"];
				if (autoContent != null)
					return (bool)autoContent;
				return false;
			}

			set
			{
				ViewState["_autoContent"] = value;
			}
		}

		/// <summary>
		/// Set to true if you need to use the control in a secure web page.
		/// </summary>
		[Bindable(false),
		Category("Behavior"),
		DefaultValue(false),
		Description("Set it to true if you need to use the control in a secure web page.")	]
		public bool EnableSsl
		{
			get
			{
				object enableSsl = ViewState["_enableSsl"];
				if (enableSsl != null)
					return (bool)enableSsl;
				return false;
			}

			set
			{
				ViewState["_enableSsl"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicates if you want to select the contents of the popup.
		/// </summary>
		[
			Bindable(true),
			Category("Behavior"),
			DefaultValue(false),
			Description("Set it to true if you want to select the contents of the popup.")
		]
		public bool EnableSelection
		{
			get
			{
				object enableSelection = ViewState["EnableSelection"];

				if (enableSelection == null)
					return false;

				return (bool)enableSelection;
			}

			set
			{
				ViewState["EnableSelection"]= value;
			}
		}

		#endregion

		#region Methods
	
		/// <summary>
		/// Notifies the Popup control to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{
			base.OnPreRender(e);   
			RegisterAPIScriptBlock();
		} 

		/// <summary>
		/// Sends the Popup content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
#if (LICENSE)
			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.ATB, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.HTB, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.AIE, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.APP, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

			_useCounter++;

			if (!(this.Parent is ActiveUp.WebControls.ToolButton) /*&& !licenseStatus.IsRegistered*/ && Page != null && _useCounter == StaticContainer.UsageCount)
			{
				_useCounter = 0;
				output.Write(StaticContainer.TrialMessage);
			}
			else
			{
				RenderPopup(output);
			}

#else
			RenderPopup(output);
#endif
		}

		/// <summary>
		/// Renders the popup.
		/// </summary>
		/// <param name="output">The output.</param>
		protected void RenderPopup(HtmlTextWriter output) 
		{
			double left = 0, top = 0;
			IEnumerator enumerator = this.Style.Keys.GetEnumerator();
			while(enumerator.MoveNext())
			{
				string val = (string)enumerator.Current;
				if (val.ToUpper() == "LEFT")
					left = (new Unit(this.Style[val])).Value;
				else if (val.ToUpper() == "TOP")
					top = (new Unit(this.Style[val])).Value;
			}

			if (this.Controls.Count > 0) 
			{
				StringWriter stringWriter = new StringWriter();
				HtmlTextWriter outputEmbedded = new HtmlTextWriter(stringWriter);
				this.RenderChildren(outputEmbedded);
				string s = stringWriter.ToString();

				output.Write("<div id='{0}_embeddedControls' style='visibility:hidden;'>",this.ClientID);
				output.Write(s);
				output.Write("</div>");
			}

			output.Write("<script language=\"javascript\">\n");
			output.Write(string.Format("function ATB_create_{0}(e)\n",ClientID));
			output.Write("{\n"); 
			output.Write(string.Format("ATB_createPopup(\"{0}\",{1}, {2}, {3}, {4}, \"{5}\", \"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\",\"{16}\",\"{17}\",\"{18}\",\"{19}\",\"{20}\", \"{21}\",\"{22}\",\"True\",\"{23}\",\"{24}\",\"{25}\",\"{26}\");\n", ClientID, left, top, Width.Value, Height.Value, TitleText == string.Empty ? "&nbsp;" : TitleText, ContentText,Utils.Color2Hex(_layoutWindow.BackColor),Utils.Color2Hex(_layoutWindow.BorderColor),_layoutWindow.BorderStyle.ToString(),_layoutWindow.BorderWidth.Value,Utils.Color2Hex(_layoutTitle.BackColor),Utils.Color2Hex(_layoutTitle.InactiveBackColor),Utils.Color2Hex(_layoutTitle.BorderColor),_layoutTitle.BorderStyle.ToString(),_layoutTitle.BorderWidth.Value, Utils.Color2Hex(_layoutTitle.ForeColor), Utils.Color2Hex(_layoutContent.BackColor), Utils.Color2Hex(_layoutContent.BorderColor), _layoutContent.BorderStyle.ToString(), _layoutContent.BorderWidth.Value, Utils.Color2Hex(_layoutContent.ScrollBarColor), Dragable, Overflow.ToString().ToLower(),EnableSsl,AllowResize,ShowShadow));
			if (LayoutTitle.BackGradientFirstColor != Color.Empty && LayoutTitle.BackGradientLastColor != Color.Empty)
				output.Write(string.Format("\nATB_setTitleGradient(\"{0}\",\"{1}\",\"{2}\");\n",ClientID,Utils.Color2Hex(LayoutTitle.BackGradientFirstColor), Utils.Color2Hex(LayoutTitle.BackGradientLastColor)));

			string fontFamily = string.Empty; 
			if (LayoutTitle.Font.Names.Length > 0)
				fontFamily = Utils.FormatStringArray(LayoutTitle.Font.Names, ',');
			string textDecoration = Utils.GetTextDecoration(LayoutTitle.Font.Underline,LayoutTitle.Font.Overline,LayoutTitle.Font.Strikeout);
			output.Write(string.Format("ATB_setTitleFont(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\");",ClientID,fontFamily,LayoutTitle.Font.Size.ToString(),LayoutTitle.Font.Bold,LayoutTitle.Font.Italic,textDecoration));

			fontFamily = string.Empty; 
			if (LayoutContent.Font.Names.Length > 0)
				fontFamily = Utils.FormatStringArray(LayoutContent.Font.Names, ',');
			textDecoration = Utils.GetTextDecoration(LayoutContent.Font.Underline,LayoutContent.Font.Overline,LayoutContent.Font.Strikeout);
			output.Write(string.Format("ATB_setContentFont(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\");",ClientID,fontFamily,LayoutContent.Font.Size.ToString(),LayoutContent.Font.Bold,LayoutContent.Font.Italic,textDecoration));

#if (!FX1_1)
            if (CloseImage == string.Empty)
            {
                CloseImage = Utils.ConvertToImageDir(ImagesDirectory, "", "close.gif", this.Page, this.GetType());
            }
            output.Write(string.Format("ATB_setCloseImage(\"{0}\",\"{1}\");", ClientID, CloseImage));
#else
            if (CloseImage != string.Empty)
            {
                output.Write(string.Format("ATB_setCloseImage(\"{0}\",\"{1}\");", ClientID, Utils.ConvertToImageDir(ImagesDirectory, CloseImage, "close.gif", Page, this.GetType())));
            }
#endif
            if (ResizeImage != string.Empty && AllowResize == true)
            {
                output.Write(string.Format("ATB_setResizeImage(\"{0}\",\"{1}\");", ClientID, Utils.ConvertToImageDir(ImagesDirectory, ResizeImage, "resize.gif", Page, this.GetType())));
            }

			if (ShowWindow == true && ShowTitle == false)
				output.Write(string.Format("\nATB_showTitle('{0}','False');",ClientID));
			else if (ShowWindow == false)
				output.Write(string.Format("\nATB_showWindow('{0}','False');",ClientID));
			if (DragFromContent)
				output.Write(string.Format("\nATB_dragContent('{0}','True');",ClientID));

			if (ContentDivId != string.Empty && this.Controls.Count == 0)
				output.Write(string.Format("\nATB_setContentFromDiv('{0}','{1}');",ClientID,ContentDivId));
			else if (this.Controls.Count > 0)
				output.Write(string.Format("\nATB_setContentFromDiv('{0}','{0}_embeddedControls');",ClientID));

			if (ShowTitle == false && ShowWindow == false)
				output.Write(string.Format("\nATB_setZeroPaddingContents('{0}');",ClientID));

			if (AutoContent == true)
			{
				output.Write(string.Format("\nATB_autoContent('{0}','True');",ClientID));
			}

			if (ShowedOnStart == false)
			{
				output.Write(string.Format("\nATB_hidePopup('{0}');",ClientID));
			}

			if (UseStartupPosition == true) 
			{
				output.Write(string.Format("\nATB_movePopupTo('{0}',{1},{2});",ClientID,StartupPosition.X,StartupPosition.Y));
			}

			if (TransparentOnMove == true) 
			{
				output.Write(string.Format("\nATB_enableTransparentOnMove('{0}');",ClientID));
			}

			output.Write(string.Format("\nATB_enableSelection('{0}','{1}');",ClientID,EnableSelection));
			output.Write("}\n");
			output.Write(string.Format("window.RegisterEvent(\"onload\", ATB_create_{0});\n",ClientID));
			output.Write("\n</script>\n");

		}

		/// <summary>
		/// Register the Client-Side script block in the ASPX page.
		/// </summary>
		protected void RegisterAPIScriptBlock()
		{
			// Register the script block is not allready done.
			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
				if ((this.ExternalScript == null || this.ExternalScript == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
				{
#if (!FX1_1)
            Page.ClientScript.RegisterClientScriptInclude(SCRIPTKEY, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.ActiveToolbar.js"));
#else					
					if (CLIENTSIDE_API == null)
						CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls._resources.ActiveToolbar.js");
					
					if (!CLIENTSIDE_API.StartsWith("<script"))
						CLIENTSIDE_API = "<script language=\"javascript\">\n" + CLIENTSIDE_API;

					CLIENTSIDE_API += "\n</script>\n";

					Page.RegisterHiddenField("Containers","");

					Page.RegisterClientScriptBlock(SCRIPTKEY, CLIENTSIDE_API);
#endif					
				}
				else
				{
					if (this.ScriptDirectory.StartsWith("~"))
						this.ScriptDirectory = this.ScriptDirectory.Replace("~", System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActiveToolbar.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
			}

        }


		/*private string GetLicenseMessage()
		{
			Exception lastException = new Exception();
			LicenseStatus status, status2;
			
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License("ActiveCalendarV2");

			status = license.CheckLicense(ProductCode.ACL,2,this.License);
			status2 = license.CheckLicense(ProductCode.AWC,3,this.License);
				
			if (status.IsRegistered || status2.IsRegistered)
				return status.ErrorMessage;
			else if (status.ErrorType == LicenseError.Invalid || status.ErrorType == LicenseError.TrialExpired)
				return status.ErrorMessage;
			
			return status2.ErrorMessage;
		}*/

		/*private LicenseType GetLicenseType()
		{
			Exception lastException = new Exception();
			LicenseStatus status, status2, status3;
			
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License("ActiveProtectorV1");

			status = license.CheckLicense(ProductCode.HPT,1,this.License);
			status2 = license.CheckLicense(ProductCode.AWC,3,this.License);
			status3 = license.CheckLicense(ProductCode.APP,1,this.License);
				
			if (status.IsRegistered || status2.IsRegistered || status3.IsRegistered)
				return LicenseType.Registered;
			else if ((status.ErrorType == LicenseError.Invalid || status.ErrorType == LicenseError.FileNotFound)
				&& (status2.ErrorType == LicenseError.Invalid || status2.ErrorType == LicenseError.FileNotFound)
				&& (status3.ErrorType == LicenseError.Invalid || status3.ErrorType == LicenseError.FileNotFound))
				return LicenseType.Error;
			else if (status.ErrorType == LicenseError.TrialExpired || status2.ErrorType == LicenseError.TrialExpired
				|| status3.ErrorType == LicenseError.TrialExpired)
				return LicenseType.TrialExpired;
			else
				return LicenseType.TrialValid;
		}*/

		#endregion
	}

	#endregion
}
 
