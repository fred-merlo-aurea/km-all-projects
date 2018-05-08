using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActiveUp.WebControls.Common;
using ActiveUp.WebControls.Common.Extension;

namespace ActiveUp.WebControls
{
    #region Panel

    /// <summary>
    /// Represents a <see cref="Panel"/> object.
    /// </summary>
    [
	ToolboxData("<{0}:Panel runat=server></{0}:Panel>"),
	Designer(typeof(PanelDesigner)),
	ToolboxBitmap(typeof(Panel), "ToolBoxBitmap.Panel.bmp"),
	Serializable,
	]
	public class Panel : CoreWebControl, IPostBackDataHandler, IPostBackEventHandler
	{
		#region Fields

		/// <summary>
		/// Unique client script key.
		/// </summary>
		private const string SCRIPTKEY = "ActivePanel";

#if (LICENSE)
		//private string _license = string.Empty;
		private int _useCounter = 0;
#endif

		#endregion

		#region Handler Declaration

		/// <summary>
		/// Event handler used for the <see cref="StateChanged"/> event.
		/// </summary>
		public delegate void StateEventHandler(object sender, StateEventArgs e); 

		#endregion

		#region Events Declaration

		/// <summary>
		/// Server event occurs when the state of the panel changes.
		/// </summary>
		public event StateEventHandler StateChanged;

		/// <summary>
		/// Server event occurs when the click of the panel title occurs.
		/// </summary>
		public event EventHandler Clicked;

		#endregion

		#region Constructors

		/// <summary>
		/// The default constructor.
		/// </summary>
		public Panel()
		{
			
		}

		#endregion

		#region Properties

		#region Appearance

		/// <summary>
		/// Gets or sets the color of the border around the panel.
		/// </summary>
		[
		Bindable(false),
		Category("Appearance"),
		Description("Color of the border around the panel."),
		DefaultValue(typeof(Color),"#B2B3B5")
		]
		public new Color BorderColor
		{
			get
			{
				object borderColor = ViewState["BorderColor"];

				if (borderColor != null)
					return (Color)borderColor;

				return Color.FromArgb(0xB2,0xB3,0xB5);
			}

			set
			{
				ViewState["BorderColor"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the style of the border around the panel.
		/// </summary>
		[
		Bindable(false),
		Category("Appearance"),
		Description("Style of the border around the panel."),
		DefaultValue(typeof(BorderStyle),"Solid")
		]
		public new BorderStyle BorderStyle
		{
			get
			{
				object borderStyle = ViewState["BorderStyle"];

				if (borderStyle != null)
					return (BorderStyle)borderStyle;

				return BorderStyle.Solid;
			}

			set
			{
				ViewState["BorderStyle"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the width of the border around the panel.
		/// </summary>
		[
		Bindable(false),
		Category("Appearance"),
		Description("Width of the border around the panel."),
		DefaultValue(typeof(Unit),"1px")
		]
		public new Unit BorderWidth
		{
			get
			{
				object borderWidth = ViewState["BorderWidth"];

				if (borderWidth != null)
					return (Unit)borderWidth;

				return Unit.Parse("1px");
			}

			set
			{
				ViewState["BorderWidth"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the cell spacing of the panel.
		/// </summary>
		[
		Bindable(false),
		Category("Appearance"),
		Description("The cell spacing of the panel."),
		DefaultValue("0")
		]
		public int CellSpacing
		{
			get
			{
				object cellSpacing  = ViewState["CellSpacing"];

				if (cellSpacing != null)
					return (int)cellSpacing;

				return 0;
			}
			set
			{
				ViewState["CellSpacing"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the cell padding of the panel.
		/// </summary>
		[
		Bindable(false),
		Category("Appearance"),
		Description("The cell padding of the panel."),
		DefaultValue("0")
		]
		public int CellPadding
		{
			get
			{
				object cellSpacing = ViewState["CellPadding"];

				if (cellSpacing != null)
					return (int)CellSpacing;

				return 0;
			}
			set
			{
				ViewState["CellPadding"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the background color of the panel when it collapsed.
		/// </summary>
		[
		Bindable(false),
		Category("Appearance"),
		Description("The background color of the panel when it collapsed."),
		DefaultValue(typeof(Color),"#D3D3D3")
		]
		public Color TitleBackColorCollapsed
		{
			get
			{
				object titleBackColorCollapsed = ViewState["TitleBackColorCollapsed"];
				
				if (titleBackColorCollapsed != null)
					return (Color)titleBackColorCollapsed;
	
				return Color.FromArgb(0xD3,0xD3,0xD3);
			}

			set
			{
				ViewState["TitleBackColorCollapsed"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the background color of the panel when it expanded.
		/// </summary>
		[
		Bindable(false),
		Category("Appearance"),
		Description("The background color of the panel when it expanded."),
		DefaultValue(typeof(Color),"#E7E8EA")
		]
		public Color TitleBackColorExpanded
		{
			get
			{
				object titleBackColorExpanded = ViewState["TitleBackColorExpanded"];

				if (titleBackColorExpanded != null)
					return (Color)titleBackColorExpanded;

				return Color.FromArgb(0xE7,0xE8,0xEA);
			}

			set
			{
				ViewState["TitleBackColorExpanded"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the margin left of the title.
		/// </summary>
		[
		Bindable(false),
		Category("Appearance"),
		Description("Margin left of the title."),
		DefaultValue(typeof(Unit),"3px")
		]
		public Unit TitleMarginLeft
		{
			get
			{
				object titleMarginLeft = ViewState["TitleMarginLeft"];

				if (titleMarginLeft != null)
					return (Unit)titleMarginLeft;

				return Unit.Parse("3px");
			}

			set
			{
				ViewState["TitleMarginLeft"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the expanded image url.
		/// </summary>
		[
		Bindable(false),
		Category("Appearance"),
		Description("The expanded image url."),
#if (!FX1_1)
	DefaultValue("")
#else		
		DefaultValue("Expanded.gif")
#endif		
		]
		public string ExpandedImage 
		{
			get
			{
				object expandedImage = ViewState["ExpandedImage"];

				if (expandedImage != null)
					return (string)expandedImage;
#if (!FX1_1)
	return string.Empty;
#else		
				return "Expanded.gif";
#endif				
			}

			set
			{
				ViewState["ExpandedImage"] = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the collapsed image url.
		/// </summary>
		[
		Bindable(false),
		Category("Appearance"),
		Description("The collapsed image url."),
#if (!FX1_1)
	DefaultValue("")
#else				
		DefaultValue("Collapsed.gif")
#endif		
		]
		public string CollapsedImage 
		{
			get
			{
				object collapsedImage = ViewState["CollapsedImage"];

				if (collapsedImage != null)
					return (string)collapsedImage;
					
#if (!FX1_1)
	return string.Empty;
#else		
				return "Collapsed.gif";
#endif				
			}

			set
			{
				ViewState["CollapsedImage"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the background image url.
		/// </summary>
		[
		Bindable(false),
		Category("Appearance"),
		Description("The background image url."),
		DefaultValue("")
		]
		public string BackgroundImage 
		{
			get
			{
				object backgroundImage = ViewState["BackgroundImage"];

				if (backgroundImage != null)
					return (string)backgroundImage;

				return string.Empty;
			}

			set
			{
				ViewState["BackgroundImage"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the height of the title bar.
		/// </summary>
		[
		Bindable(false),
		Category("Appearance"),
		Description("The height of the title bar."),
		DefaultValue(typeof (Unit),"22px")
		]
		public Unit TitleHeight
		{
			get
			{
				object titleHeight = ViewState["TitleHeight"];

				if (titleHeight != null)
					return (Unit)titleHeight;

				return Unit.Parse("22px");
			}

			set
			{
				ViewState["TitleHeight"] = value;
			}
		}


		#endregion

		#region Script

		/// <summary>
		/// Gets or sets the relative or absolute path to the directory where the API javascript file is.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the control render.</remarks>
		[Bindable(false),
		Category("Scrip"),
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
				object scriptDirectory = ViewState["ScriptDirectory"];

				if (scriptDirectory != null)
					return (string)scriptDirectory;
#if (!FX1_1)
                return string.Empty;
#else
				return Define.SCRIPT_DIRECTORY;
#endif
			}
			set
			{
				ViewState["ScriptDirectory"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the relative or absolute path to the external Html TextBox API javascript file.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the Html TextBox render.</remarks>
		[Bindable(false),
		Category("Script"),
		Description("Gets or sets the relative or absolute path to the external Html TextBox API javascript file."),
#if (!FX1_1)
        DefaultValue("")
#else
		DefaultValue(Define.IMAGES_DIRECTORY)
#endif
		]
		public string ImagesDirectory
		{
			get
			{
				object imagesDirectory = ViewState["ImagesDirectory"];

				if (imagesDirectory != null)
					return (string)imagesDirectory;
#if (!FX1_1)
                return string.Empty;
#else
				return Define.IMAGES_DIRECTORY;
#endif
			}
			set
			{
				ViewState["ImagesDirectory"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the filename of the external script file.
		/// </summary>
		/// 
		[
		Bindable(false),
		Category("Script"),
		Description("The filename of the external script file."),
		DefaultValue("")
		]
		public string ExternalScript
		{
			get
			{
			    return ViewStateHelper.GetFromViewState(ViewState, nameof(ExternalScript), string.Empty);
            }
			set
			{
			    ViewStateHelper.SetViewState(ViewState, nameof(ExternalScript), value);
            }
		}

		#endregion

		#region Behavior

		/// <summary>
		/// Gets or set the state of the pannel (Expanded or Collapsed).
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("The state of the panel (Expanded or Collapsed)."),
		DefaultValue("Expanded")
		]
		public PanelState State
		{
			get
			{
				object state = ViewState["State"];
				
				if (state != null)
					return (PanelState)state;

				return PanelState.Expanded;
			}

			set
			{
			  ViewState["State"]	= value;
			}
		}

		/// <summary>
		/// Gets or sets if the panel can be expanded or not.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Indicates if the panel can be expanded or not."),
		DefaultValue(true)
		]
		public bool CanBeExpanded
		{
			get
			{
				object canBeExpanded = ViewState["CanBeExpanded"];
				
				if (canBeExpanded != null)
					return (bool)canBeExpanded;

				return true;
			}

			set
			{
				ViewState["CanBeExpanded"]	= value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the panel use effect when it expands or collapses.
		/// </summary>
		/// <value><c>true</c> if the panel use effect when it expands or collapses; otherwise, <c>false</c>.</value>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Indicates if the panel use effect when it expands or collapses."),
		DefaultValue(false),
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool ScrollEffect 
		{
			get
			{
				object scrollEffect = ViewState["ScrollEffect"];
				
				if (scrollEffect != null)
					return (bool)scrollEffect;

				return false;
			}

			set
			{
				ViewState["ScrollEffect"]	= value;
			}
		}

		
		/// <summary>
		/// Gets or sets the speed of the scroll effect.
		/// </summary>
		/// <value>The speed of the scroll effect.</value>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Indicates the speed of the scroll effect."),
		DefaultValue(100)
		]
		public int Speed 
		{
			get
			{
				object speed = ViewState["Speed"];
				
				if (speed != null)
					return (int)speed;

				return 100;
			}

			set
			{
				ViewState["Speed"]	= value;
			}
		}

		/// <summary>
		/// Gets or sets the frame.
		/// </summary>
		/// <value>The frame.</value>
		[
		Bindable(false),
		Category("Behavior"),
		Description(""),
		DefaultValue(1)
		]
		public int Frame 
		{
			get
			{
				object frame = ViewState["Frame"];
				
				if (frame != null)
					return (int)frame;

				return 1;
			}

			set
			{
				ViewState["Frame"]	= value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Panel"/> use fade effect when it expands or collapses.
		/// </summary>
		/// <value><c>true</c> if fade; otherwise, <c>false</c>.</value>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Indicates the panel use fade effect when it expands or collapses."),
		DefaultValue(false)
		]
		public bool Fade 
		{
			get
			{ 
				object fade = ViewState["Fade"];

				if (fade != null) 
				{
					return (bool)fade;
				}

				return false;
			}

			set
			{
				ViewState["Fade"] = value;
			}
		}

		#endregion

		#region Data
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

#endif*/

		/// <summary>
		/// Gets or sets the title text of the panel.
		/// </summary>
		[
		Bindable(false),
		Category("Data"),
		Description("The title text of the panel."),
		DefaultValue("")
		]
		public string TitleText 
		{
			get
			{
				object titleText = ViewState["TitleText"];

				if (titleText != null)
					return (string)titleText;

				return string.Empty;
			}

			set
			{
				ViewState["TitleText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Html contents text.
		/// </summary>
		[
		Bindable(false),
		Category("Data"),
		Description("The Html contents text."),
		DefaultValue("")
		]
		public string ContentsText
		{
			get
			{
				object contentsText = ViewState["ContentsText"];

				if (contentsText != null)
					return (string)contentsText;

				return string.Empty;
			}

			set
			{
				ViewState["ContentsText"] = value;
			}
		}
			
		#endregion

		#region Client Side

		/// <summary>
		/// Gets or sets the additional JScript client-side functions to call when the state of the panel changed.
		/// </summary>
		[
		Bindable(true),
		Category("Client Side"),
		Description("Additional JScript client-side functions to call when the state of the panel changed."),
		DefaultValue(true)
		]
		public string OnTitleClickClientSide
		{
			get
			{
				object onTitleClickClientSide = ViewState["OnTitleClickClientSide"];

				if (onTitleClickClientSide != null)
					return (string)onTitleClickClientSide;

				return string.Empty;
			}

			set
			{
				ViewState["OnTitleClickClientSide"] = value;
			}
		}

		#endregion

		#region Layout

		/// <summary>
		/// Gets or sets the width of the control.
		/// </summary>
		[
		Bindable(false),
		Category("Layout"),
		Description("The width of the control."),
		DefaultValue(typeof(Unit),"215px")
		]
		public new Unit Width 
		{
			get
			{
				object width = ViewState["Width"];

				if (width != null)
					return (Unit)width;

				return Unit.Parse("215px");
			}

			set
			{
				ViewState["Width"] = value;
			}
		}


		#endregion

		#endregion

		#region Methods

		#region Script

		/// <summary>
		/// Register the client-side script block in the ASPX page.
		/// </summary>
		public void RegisterAPIScriptBlock() 
		{
			// Register the script block is not allready done.

			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
        if ((this.ExternalScript == null || this.ExternalScript == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
				{
#if (!FX1_1)
            Page.ClientScript.RegisterClientScriptInclude(SCRIPTKEY, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.ActivePanel.js"));
#else
					string CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls._resources.ActivePanel.js");
					//string CLIENTSIDE_API = EditorHelper.GetResource("ActivePanel.ActivePanel.js");
					if (!CLIENTSIDE_API.StartsWith("<script"))
						CLIENTSIDE_API = "<script language=\"javascript\">\n" + CLIENTSIDE_API;

					CLIENTSIDE_API += "\n</script>\n";

					Page.RegisterClientScriptBlock(SCRIPTKEY, CLIENTSIDE_API);
#endif					
				}
				else
				{
					if (this.ScriptDirectory.StartsWith("~"))
						this.ScriptDirectory = this.ScriptDirectory.Replace("~", System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActivePanel.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
			}

		    Page.TestAndRegisterScriptBlock(SCRIPTKEY, ScriptDirectory, "APN_TestIfScriptPresent()");

			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY + "_Var_" + ClientID)) 
			{
				string varString = string.Empty;
				varString += "<script>\n";
				varString += "\n// Variable declaration related to the panel '" + ClientID + "'\n";
                varString += ClientID + "_CollapsedImage='" + Utils.ConvertToImageDir(ImagesDirectory, CollapsedImage, "collapsed.gif", Page, this.GetType()) + "';\n";
                varString += ClientID + "_ExpandedImage='" + Utils.ConvertToImageDir(ImagesDirectory, ExpandedImage, "expanded.gif", Page, this.GetType()) + "';\n";
				varString += ClientID + "_OnTitleClickClientSide=\"" + OnTitleClickClientSide + "\";\n";
				varString += ClientID + "_TitleBackColorCollapsed=\"" + Utils.Color2Hex(TitleBackColorCollapsed) + "\";\n";
				varString += ClientID + "_TitleBackColorExpanded=\"" + Utils.Color2Hex(TitleBackColorExpanded) + "\";\n";
				varString += ClientID + "_ScrollEffect=\"" + ScrollEffect + "\";\n";
                if (ScrollEffect) 
				{
					varString += ClientID + "_Fade=\"" + Fade + "\";\n";
					varString += ClientID + "_Speed=\"" + Speed + "\";\n";
					varString += ClientID + "_Frame=\"" + Frame + "\";\n";
				}
				varString += "\n</script>\n";

				Page.RegisterClientScriptBlock(SCRIPTKEY + "_Var_" + ClientID,varString);
			}

			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY + "_Init" + ClientID) && ScrollEffect)
			{
				string varString = string.Empty;

				varString += "<script language=\"javascript\">\n";
				varString += $"function Init_{ClientID}(e)\n";
				varString += "{\n";

				varString += "var realSize = document.createElement(\"input\");\n";
				varString += "realSize.setAttribute(\"type\",\"hidden\");\n";
				varString += $"realSize.setAttribute(\"name\",\"{ClientID}_RealSize\");\n";
				varString += $"realSize.setAttribute(\"id\",\"{ClientID}_RealSize\");\n";
				varString += $"realSize.setAttribute(\"value\", document.getElementById('{ClientID}_Temp').clientHeight);\n";
				varString += "document.body.appendChild(realSize);\n";
				varString += $"document.getElementById('{ClientID}_Temp').removeNode(false);\n";
	
				if (Fade)
				{
					varString += string.Format("var valueStep = 100 / (parseInt(document.getElementById('{0}_RealSize').value) / parseInt(eval('{0}_Frame')));  \n",ClientID);
					varString += "var fadeStep = document.createElement(\"input\");\n";
					varString += "fadeStep.setAttribute(\"type\",\"hidden\");\n";
					varString += $"fadeStep.setAttribute(\"name\",\"{ClientID}_FadeStep\");\n";
					varString += $"fadeStep.setAttribute(\"id\",\"{ClientID}_FadeStep\");\n";
					varString += string.Format("fadeStep.setAttribute(\"value\", Math.round(valueStep));\n",ClientID);
					varString += "document.body.appendChild(fadeStep);\n";
				}
				varString += "}\n";
				varString += $"window.RegisterEvent(\"onload\", Init_{ClientID});\n";
				varString += "\n</script>\n";

				Page.RegisterClientScriptBlock(SCRIPTKEY + "_Init" + ClientID, varString);
			}
		}

		#endregion

		#region Render

		/// <summary>
		/// Notifies the <see cref="Panel"/> control to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{
			this.RegisterAPIScriptBlock();
			if (base.Page != null)
				Page.RegisterRequiresPostBack(this);
			
			base.OnPreRender(e);
		
		}

		/// <summary> 
		/// Render the <see cref="Panel"/> to the output parameter specified.
		/// </summary>
		/// <param name="output">Output stream that contains the HTML used to represent the <see cref="Panel"/>.</param>
		protected override void Render(HtmlTextWriter output)
		{
#if (LICENSE)

			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.APN, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

			_useCounter++;

			if (/*!licenseStatus.IsRegistered &&*/ Page != null && _useCounter == StaticContainer.UsageCount)
			{
				_useCounter = 0;
				output.Write(StaticContainer.TrialMessage);
			}
			else 
				RenderPanel(output);
#else
			RenderPanel(output);
#endif
		}
		/// <summary> 
		/// Render the <see cref="Panel"/> to the output parameter specified.
		/// </summary>
		/// <param name="output">Output stream that contains the HTML used to represent the <see cref="Panel"/>.</param>
		internal void RenderPanel(HtmlTextWriter output)
		{
			var style = RenderPanelHeader(output);

			RenderPanelMiddle(output, style);

			RenderPanelFooter(output);
		}

		private void RenderPanelMiddle(HtmlTextWriter output, string style)
		{
			var onClick = string.Empty;
			if(CanBeExpanded)
			{
				onClick += $"APN_CollapseExpand('{ClientID}');";
			}

			if(OnTitleClickClientSide != string.Empty)
			{
				onClick += $"APN_OnTitleClientSide('{ClientID}');";
			}

			if(Clicked != null && Page != null)
			{
				onClick += Page.GetPostBackEventReference(this, string.Empty);
			}

			output.Write(
				$"<tr id=\"{ClientID + "_Title"}\" valign=\"middle\"{(style != string.Empty ? $" style=\"{style}\"" : string.Empty)}{(onClick != string.Empty ? $" onclick=\"{onClick}\"" : string.Empty)}>"); // OPEN TR 1

			output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			output.AddAttribute(HtmlTextWriterAttribute.Align, HorizontalAlign.Left.ToString());

			output.RenderBeginTag(HtmlTextWriterTag.Td); // OPEN TD 1

			style = string.Empty;
			if(TitleMarginLeft.Value > 0)
			{
				style += $"margin-left:{TitleMarginLeft};";
			}

			output.Write($"<span{(style != string.Empty ? $" style=\"{style}\"" : string.Empty)}>");
			output.Write(TitleText);
			output.Write("</span>");

			output.RenderEndTag(); // CLOSE TD 1

			output.RenderBeginTag(HtmlTextWriterTag.Td); // OPEN TD 2

			var imgButton = string.Empty;
			imgButton += "<img";
			imgButton += $" id={ClientID + "_Button"}";
			imgButton += " border=0;";
			if(State == PanelState.Collapsed)
			{
				imgButton +=
					$" src=\"{Utils.ConvertToImageDir(ImagesDirectory, CollapsedImage, "collapsed.gif", Page, GetType())}\"";
			}
			else
			{
				imgButton += $" src=\"{Utils.ConvertToImageDir(ImagesDirectory, ExpandedImage, "expanded.gif", Page, GetType())}\"";
			}

			imgButton += " style=\"margin-right:3px;\"";
			output.Write(imgButton);
			output.Write("</img>");
			output.RenderEndTag(); // CLOSE TD 2
			output.Write("</tr>");
		}

		private void RenderPanelFooter(HtmlTextWriter output)
		{
			// Collapse section
			var display = State == PanelState.Expanded ? string.Empty : "none";

			if(display != string.Empty)
			{
				output.AddStyleAttribute(nameof(display), display);
			}

			output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_CollapseArea");
			output.RenderBeginTag(HtmlTextWriterTag.Tr); // OPEN TR 2

			if(BackgroundImage != string.Empty)
			{
				output.AddStyleAttribute("background-image",
					$"url({Utils.ConvertToImageDir(ImagesDirectory, BackgroundImage)});");
			}

			output.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
			output.RenderBeginTag(HtmlTextWriterTag.Td); // OPEN TD 3

			if(ScrollEffect)
			{
				output.Write(
					$"<div id=\"{ClientID}_Scroll\" style=\"overflow:hidden;background-color:{Utils.Color2Hex(BackColor)};\">");
			}

			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "1");
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			output.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			output.RenderBeginTag(HtmlTextWriterTag.Table); // OPEN TABLE 2
			output.RenderBeginTag(HtmlTextWriterTag.Tr); // OPEN TR 3
			output.RenderBeginTag(HtmlTextWriterTag.Td); // OPEN TD 4

			if(Controls.Count > 0)
			{
				RenderChildren(output);
			}
			else
			{
				output.Write(ContentsText == string.Empty ? "<br><br>" : ContentsText);
			}

			output.RenderEndTag(); // CLOSE TD 4
			output.RenderEndTag(); // CLOSE TR 3
			output.RenderEndTag(); // CLOSE TABLE 2

			if(ScrollEffect)
			{
				output.Write("</div>");
			}

			output.RenderEndTag(); // CLOSE TD 3
			output.RenderEndTag(); // CLOSE TR 2
			output.Write("</table>"); // CLOSE TABLE 1
			output.Write("</div>");

			if(Height == Unit.Empty && HttpContext.Current != null && ScrollEffect)
			{
				output.AddStyleAttribute(nameof(display), "none");
				output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_Temp");
				output.RenderBeginTag(HtmlTextWriterTag.Table); // OPEN TABLE 1
				output.RenderBeginTag(HtmlTextWriterTag.Tr); // OPEN TR 1
				output.RenderBeginTag(HtmlTextWriterTag.Td); // OPEN TD 1

				if(Controls.Count > 0)
				{
					RenderChildren(output);
				}
				else
				{
					output.Write(ContentsText == string.Empty ? "<br><br>" : ContentsText);
				}

				output.RenderEndTag(); // CLOSE TD 1
				output.RenderEndTag(); // CLOSE TR 1
				output.RenderEndTag(); // CLOSE TABLE 1
			}
		}

		private string RenderPanelHeader(HtmlTextWriter output)
		{
			output.AddAttribute(HtmlTextWriterAttribute.Type, "Hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
			output.AddAttribute(HtmlTextWriterAttribute.Value, DateTime.Now.Ticks.ToString());
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			output.Write($"<div id=\"{ClientID}_Panel\" onselectstart=\"return false;\" ondragstart=\"return false\">");

			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_State");
			output.AddAttribute(HtmlTextWriterAttribute.Name, ClientID + "_State");
			output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Value, State.ToString());
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			var style = new StringBuilder();

			if (Width.Value > 0)
			{
				style.Append($"width:{Width};");
			}

			if (Height.Value > 0)
			{
				style.Append($"height:{Height};");
			}

			if (BackColor != Color.Empty)
			{
				style.Append($"background-color:{Utils.Color2Hex(BackColor)};");
			}

			if (BorderStyle != BorderStyle.NotSet && BorderStyle != BorderStyle.None)
			{
				style.Append($"border-color:{Utils.Color2Hex(BorderColor)};");
				style.Append($"border-style:{BorderStyle.ToString()};");
				style.Append($"border-width:{BorderWidth};");
			}

			output.Write(
				$"<table{(style.ToString() != string.Empty ? $" style=\"{style}\"" : string.Empty)} cellpadding={CellPadding} cellspacing={CellSpacing}>"); // OPEN TABLE 1 

			style.Length = 0;
			style.Capacity = 0;

			if (TitleHeight.Value > 0)
			{
				style.Append($"height:{TitleHeight};");
			}

			if (State == PanelState.Collapsed)
			{
				if (TitleBackColorCollapsed != Color.Empty)
				{
					style.Append($"background-color:{Utils.Color2Hex(TitleBackColorCollapsed)};");
				}
			}
			else
			{
				if (TitleBackColorExpanded != Color.Empty)
				{
					style.Append($"background-color:{Utils.Color2Hex(TitleBackColorExpanded)};");
				}
			}

			return style.ToString();
		}

		#endregion

		#endregion

		#region Events

		/// <summary>
		/// Server event occurs when the state of the panel changes.
		/// </summary>
		/// <param name="e">The new state of the panel.</param>
		protected virtual void OnStateChanged(StateEventArgs e) 
		{
			if (StateChanged != null)
				StateChanged(this,e);
		}

		/// <summary>
		/// Server event occurs when a click of the title occurs.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnClicked(EventArgs e) 
		{
			if (Clicked != null)
				Clicked(this,e);
		}

		#endregion

		#region Interface IPostBackDataHandler

		/// <summary>
		/// Processes post-back data for a server control.
		/// </summary>
		/// <param name="postDataKey">Key identifier for the control.</param>
		/// <param name="postCollection">The collection of all incoming name values.</param>
		/// <returns>False if the server control's state does not change as a result of the post-back otherwise it returns true.</returns>
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{	
			Page.Trace.Write(ClientID,"LoadPostData");

			PanelState oldState = State;
			PanelState newState = (PanelState)Enum.Parse(typeof(PanelState),postCollection[ClientID + "_State"]);

			State = newState;

			if (oldState != newState)
				return true;

			return false;
		}

		/// <summary>
		/// Signals the server control object to notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			Page.Trace.Write(ClientID,"RaisePostDataChangedEvent");
			OnStateChanged(new StateEventArgs(this.State));
		}

		#endregion

		#region Interface IPostBackEventHandler

		/// <summary>
		/// Enables the panel to process an event raised when a form is posted to the server.
		/// </summary>
		/// <param name="eventArgument">A String that represents an optional event argument to be passed to the event handler.</param>
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			OnClicked(EventArgs.Empty);
		}

		#endregion
	}

	#endregion
}
