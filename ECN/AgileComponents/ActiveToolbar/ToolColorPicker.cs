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
	/// <summary>
	/// Represents a <see cref="ToolColorPicker"/> object.
	/// </summary>
	[
	ParseChildren(true, "Items"),
	Serializable,
	ToolboxItem(false),
   Designer(typeof(ToolColorPickerDesigner)),
	]
	public class ToolColorPicker : ToolBase, IPostBackEventHandler, IPostBackDataHandler
	{
		private const string SpacerImage = "spacer.gif";
		private const string CloseImage = "close.gif";
		private const string Hidden = "Hidden";
		private const string HiddenSmall = "hidden";
		private const string DownImage = "down.gif";
		private const string CursorHand = "cursor:hand;";
		private const string CursorPointer = "cursor:pointer;";
		private const string DisplayNone = "display:none;";
		private const string DoubleQuotes = "\"";
		private const string HundredPercent = "100%";
		private const string Zero = "0";
		private const string Padding = "padding";
		private const string ColorImage = "color.gif";
		private const string Ie = "IE";
		private const string StyleOpen = "style=\"";
		private const string TableOpen = "<table";
		private const string TagSuffix = ">";
		private const string Newline = "\n";
		private const string CellPaddingSpacingZero = " cellpadding=0 cellspacing=0";
		private const string TwoPxAll = "2px 2px 2px 2px";
		private const string ThreeZeroPxOneFourPx = "0px 0px 0px 4px";
		private const string JavaScriptOpen = "<script language=\"javascript\">\n";
		private const string AtbBuildColorTable = "ATB_BuildColorTable";
		private const string AtbBuildColorTableMozilla = "ATB_BuildColorTableMozilla";
		private const string ScriptClose = "\n</script>\n";
		private const string ForteenPx = "14px";
		private const string Solid = "solid";
		private const string OnePx = "1px";
		private const string TableClose = "</table>";
		private const string OnMouseEnterStyleDisplay = " onmouseenter=\"this.style.display='';\"";
		private const string SourceBlankHtm = "src='blank.htm'";
		private const string Language = "language";
		private const string Javascript = "javascript";
		private const string DivClose = "</div>";
		private const string OneTwentyPx = "120px";
		private const string OverflowAuto = "overflow:auto;";

		private int _widthPopup; 
		private int _heightPopup;
		private Color _selectedColor = Color.Empty;

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolColorPicker"/> class.
		/// </summary>
		public ToolColorPicker() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolColorPicker"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolColorPicker(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id != string.Empty)
				ID = id;
			this.ChangeToSelectedText = SelectedText.None;
			this.Width = Unit.Parse("20px");
			this.ItemsAreaHeight = Unit.Parse("100px");
			this.BackColorItems = Color.FromArgb(0xF9,0xF8,0xF7);

			ID = id;
			BackColor = Color.White;
			BackColorItems = Color.White;
			BorderColor = Color.FromArgb(0xB4,0xB1,0xA3);
			BorderColorRollOver = Color.FromArgb(0x31,0x6A,0xC5);
			BorderStyle = BorderStyle.Solid;
			BorderWidth = 1;
			ItemsAreaHeight = Unit.Empty;
			WindowBorderColor = Color.FromArgb(0xB4,0xB1,0xA3);
			WindowBorderStyle = BorderStyle.Solid;
			WindowBorderWidth = 1;
			SelectedIndex = -1;
		}

		#region Base

		#region Variables

		/// <summary>
		/// SelectedIndexChanged event handler.
		/// </summary>
		public delegate void SelectedIndexChangedEventHandler(object sender, EventArgs e);

		/// <summary>
		/// Client side script block.
		/// </summary>
		private string CLIENTSIDE_API = null; 

		/// <summary>
		/// Unique client script key.
		/// </summary>
		private const string ACTIVETOOLBARSCRIPTKEY = "ActiveToolbar";

		/// <summary>
		/// Indicates if the control can be use dhtml.
		/// </summary>
		private bool _allowDHTML = true;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the text displayed.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("The text to displayed.")
		] 
		private new string Text
		{
			get
			{
				return base.TextDefaultIdIfNull;
			}
			set
			{
				base.TextDefaultIdIfNull = value;
			}
		}

		/// <summary>
		/// Gets or sets the type of rendering.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("The type of rendering.")
		] 
		private ToolDropDownListType DropDownListType
		{
			get
			{
				object type;
				type = base.ViewState["_dropDownListType"];
				if (type != null)
					return (ToolDropDownListType)base.ViewState["_dropDownListType"];
				return ToolDropDownListType.DHTML;
			}
			set
			{
				ViewState["_dropDownListType"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the current selected index.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(-1),
		Description("Current selected index.")
		] 
		private int SelectedIndex
		{
			get
			{
				int count;
				for (count=0;count<Items.Count;count++)
					if (Items[count].Selected) 
						return count; 
				return -1; 
			}

			set
			{
				if (value < -1 || value >= Items.Count)
					throw new ArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.","value");

				for (int count=0;count<Items.Count;count++)
					Items[count].Selected = false;
					
				if (value != -1)
				{
					Items[value].Selected = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets the current selected item.
		/// </summary>
		private ToolItem SelectedItem
		{
			get
			{
				int count;
				count = this.SelectedIndex;
				if (count >= 0)
					return this.Items[count]; 
				return null; 
			}
		} 

		/// <summary>
		/// Gets or sets the border color.
		/// </summary>
		[
		Bindable(true),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		DefaultValue(typeof(System.Drawing.Color),"#B4B1A3"),
		Category("Appearance"),
		Description("Border color.")
		]
		public override Color BorderColor
		{
			get {return base.BorderColor;}
			set {base.BorderColor = value;}
		}

		/// <summary>
		/// Gets or sets the selected background color border.
		/// </summary>
		/// <value>The selected background color border.</value>
		[
		Bindable(true),
		DefaultValue(typeof(System.Drawing.Color),"#0A246A"),
		Category("Appearance"),
		Description("The background color border.")
		]
		public Color SelectedBackColorBorder
		{
			get
			{
				object selectedBackColorBorder;
				selectedBackColorBorder = ViewState["selectedBackColorBorder"];
				if (selectedBackColorBorder != null)
					return (Color)selectedBackColorBorder; 
				return Color.FromArgb(0x0A,0x24,0x6A);
			}
			set
			{
				ViewState["selectedBackColorBorder"] = value;
			}

		}

		/// <summary>
		/// Gets or sets the selected background color item.
		/// </summary>
		/// <value>The selected background color item.</value>
		[
		Bindable(true),
		DefaultValue(typeof(System.Drawing.Color),"#B6BDD2"),
		Category("Appearance"),
		Description("The selected background color item.")
		]
		public Color SelectedBackColorItem
		{
			get
			{
				object selectedBackColorItem;
				selectedBackColorItem = ViewState["selectedBackColorItem"];
				if (selectedBackColorItem != null)
					return (Color)selectedBackColorItem; 
				return Color.FromArgb(0xB6,0xBD,0xD2);
			}
			set
			{
				ViewState["selectedBackColorItem"] = value;
			}

		}

		/// <summary>
		/// Gets or sets the background color.
		/// </summary>
		[
		Category("Appearance"),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		BindableAttribute(true),
		DefaultValue(typeof(Color),"White"),
		Description("The background color.")
		]
		public override Color BackColor
		{
			get {return base.BackColor;}
			set {base.BackColor = value;}
		}

		/// <summary>
		/// Gets or sets the window border color.
		/// </summary>
		/// <value>The color of the window border.</value>
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

		/// <summary>
		/// Gets or sets the border color of each <see cref="ToolItem"/>.
		/// </summary>
		[
		Category("Appearance"),
		TypeConverterAttribute(typeof(WebColorConverter)),
		BindableAttribute(true),
		DefaultValue(typeof(Color),""),
		Description("The border color of each ToolItem.")
		]
		private Color ItemBorderColor
		{
			get
			{
				return base.ItemBorderColor;
			}
			set
			{
			    base.ItemBorderColor = value;
			}

		}

		/// <summary>
		/// Gets or sets the border color rollover of each <see cref="ToolItem"/>.
		/// </summary>
		[
		Category("Appearance"),
		TypeConverterAttribute(typeof(WebColorConverter)),
		BindableAttribute(true),
		DefaultValue(typeof(Color),""),
		Description("The border color rollover of each ToolItem.")
		]
		private Color ItemBorderColorRollOver
		{
		    get
		    {
		        return base.ItemBorderColorRollOver;
		    }
		    set
		    {
		        base.ItemBorderColorRollOver = value;
		    }
        }

		/// <summary>
		/// Gets or sets the alignment of each <see cref="ToolItem"/>.
		/// </summary>
		[
		Category("Appearance"),
		BindableAttribute(true),
		DefaultValue(typeof(Align),"Left"),
		Description("The alignment of each ToolItem.")
		]
		private Align ItemAlign
		{
		    get
		    {
		        return base.ItemAlign;
		    }
		    set
		    {
		        base.ItemAlign = value;
		    }
        }

		/// <summary>
		/// Gets or sets the border style of each <see cref="ToolItem"/>.
		/// </summary>
		[
		Category("Appearance"),
		BindableAttribute(true),
		DefaultValue(typeof(BorderStyle),"None"),
		Description("The border style of each ToolItem.")
		]
		private BorderStyle ItemBorderStyle
		{
			get
			{
				object itemBorderStyle;
				itemBorderStyle = ViewState["_itemBorderStyle"];
				if (itemBorderStyle != null)
					return (BorderStyle)itemBorderStyle;
				else
					return BorderStyle.None;
			}

			set
			{
				ViewState["_itemBorderStyle"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the border width of each <see cref="ToolItem"/>.
		/// </summary>
		[
		Category("Appearance"),
		BindableAttribute(true),
		DefaultValue(typeof(Unit),""),
		Description("The border width of each ToolItem.")
		]
		private Unit ItemBorderWidth
		{
			get
			{
				object itemBorderWidth;
				itemBorderWidth = ViewState["_itemBorderWidth"];
				if (itemBorderWidth != null)
					return (Unit)itemBorderWidth;
				else
					return Unit.Empty;
			}
			
			set
			{
				ViewState["_itemBorderWidth"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text to use for the indentation.
		/// </summary>
		[
		Category("Appearance"),
		BindableAttribute(true),
		DefaultValue(""),
		Description("The text to use for the indentation.")
		]
		private string IndentText
		{
			get
			{
			    return base.IndentText;
            }
			set
			{
			    base.IndentText = value;
			}
		}

		/// <summary>
		/// Gets or sets the value the component have to take when an item is selected.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(typeof(SelectedText),"None"),
		Description("Indicates the values the componenet have to take when an item is selected.")
		]
		private SelectedText ChangeToSelectedText
		{
			get
			{
				return base.ChangeToSelectedText;
			}
			set
			{
			    base.ChangeToSelectedText = value;
			}
		}

		private Unit ItemsAreaHeight
		{
			get
			{
				object itemsAreaHeight = ViewState["_itemsAreaHeight"];
				if (itemsAreaHeight != null)
					return (Unit)itemsAreaHeight;
				else
					return Unit.Empty;
			}

			set
			{
				ViewState["_itemsAreaHeight"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the drop down color close on click item].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the drop down color close on click; otherwise, <c>false</c>.
		/// </value>
		public bool CloseDropDownOnClickItem
		{
			get
			{
				object closeDropDownOnClickItem = ViewState["_closeDropDownOnClickItem"];
				if (closeDropDownOnClickItem != null)
					return (bool)closeDropDownOnClickItem;
				else
					return true;
			}

			set
			{
				ViewState["_closeDropDownOnClickItem"] = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		///	 Register the client-side script block in the ASPX page.
		/// </summary>
		public virtual void RegisterAPIScriptBlock() 
		{
			// Register the script block is not already done.
			Page.TestAndRegisterScriptInclude(ACTIVETOOLBARSCRIPTKEY, CLIENTSIDE_API, GetType());

			string startupString = "<script language='javascript'>\n";
			startupString += "// Variable declaration related to the control '" + ClientID + "'\n";
			startupString += ClientID + "_useSquareColor='" + this.UseSquareColor + "';\n";	
			startupString += "</script>\n";

			// Render the startup script
			Page.RegisterStartupScript(ClientID + "_startup", startupString);
		}

		/// <summary>
		/// Write the items to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		private void RenderItems(HtmlTextWriter output)
		{
			if (Items != null) 
				for (int i = 0 ; i < Items.Count ; i++)
				{
					if (_allowDHTML)
					{
						string onclick = string.Format("ATB_changeSelectedIndex('{0}','{1}','{2}');",this.ClientID,i,CloseDropDownOnClickItem);
						if (ItemBorderColor != Color.Empty)
							onclick += string.Format("document.getElementById('{0}_item{1}_parent').style.borderColor='{2}';",ClientID,i.ToString(),Utils.Color2Hex(ItemBorderColor));
						/*if (ItemBackColor != Color.Empty)
							onclick += string.Format("document.getElementById('{0}_item{1}_parent').style.backgroundColor='{2}';",ClientID,i.ToString(),Utils.Color2Hex(ItemBackColor));*/
						if (this.ClientSideClick != null && this.ClientSideClick != string.Empty)
							onclick += this.ClientSideClick;

						output.AddAttribute(HtmlTextWriterAttribute.Onclick, onclick);
						output.RenderBeginTag(HtmlTextWriterTag.Tr);
						output.RenderBeginTag(HtmlTextWriterTag.Td);
						/*output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
						output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0"); 
						output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor,Utils.Color2Hex(this.ItemBorderColor));
						output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, this.ItemBorderStyle.ToString());
						output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, this.ItemBorderWidth.ToString());*/
				
						string table = string.Empty;
						table += "<table";
						table += string.Format(" id={0}_item{1}_parent",ClientID,i.ToString());
						table += " cellpadding=0 cellspacing=0";

						if (this.Width.IsEmpty == true)
							table += " width=100%";
						else
							table += " width=100%";

						string mouseover = string.Empty;
						if (ItemBorderColorRollOver != Color.Empty)
							mouseover += string.Format("this.style.borderColor='{0}';",Utils.Color2Hex(ItemBorderColorRollOver));
						/*if (ItemBackColorRollOver != Color.Empty)
							mouseover += string.Format("this.style.backgroundColor='{0}';",Utils.Color2Hex(ItemBackColorRollOver));*/
						//output.AddAttribute(string.Format("onmouseover=\"{0}\"",mouseover),null);
						table += string.Format(" onmouseover=\"{0}\"",mouseover);

						string mouseout = string.Empty;
						if (ItemBorderColor != Color.Empty)
							mouseout += string.Format("this.style.borderColor='{0}';",Utils.Color2Hex(ItemBorderColor));
						/*if (ItemBackColor != Color.Empty)
							mouseout += string.Format("this.style.backgroundColor='{0}';",Utils.Color2Hex(ItemBackColor));*/
						//output.AddAttribute(string.Format("onmouseout=\"{0}\"",mouseout),null);
						table += string.Format(" onmouseout=\"{0}\"",mouseout);

						string style = "style=\"";
                        if (ItemBorderColor != Color.Empty)
							style+= string.Format("border-color:{0};",Utils.Color2Hex(ItemBorderColor));
						style += string.Format("border-style:{0};",ItemBorderStyle.ToString());
                        if (ItemBorderWidth != Unit.Empty)
							style += string.Format("border-width:{0};",ItemBorderWidth.ToString());
						style += "\"";
						table += string.Format(" {0}",style);

						table += ">";
						output.Write(table);
						//output.RenderBeginTag(HtmlTextWriterTag.Table);
						output.RenderBeginTag(HtmlTextWriterTag.Tr);
						/*output.AddAttribute(HtmlTextWriterAttribute.Nowrap,null);
						output.AddAttribute(HtmlTextWriterAttribute.Width,"100%");
						output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_item" + i.ToString());
						output.AddAttribute(HtmlTextWriterAttribute.Value, Items[i].Value);
						output.AddStyleAttribute("padding","0px 0px 0px 4px");*/
						string td = string.Empty;
						td += "<td";
						td += string.Format(" id={0}_item{1}",ClientID,i.ToString());
						td += string.Format(" value=\"{0}\"",Items[i].Value);
						td += " nowrap";
						td += " width=100%";
						td += " padding=\"0px 0px 0px 4px\"";
						td += string.Format(" align={0}",ItemAlign.ToString());
						td += ">";
						//output.RenderBeginTag(HtmlTextWriterTag.Td);
						output.Write(td);
						output.RenderBeginTag(HtmlTextWriterTag.Span);
						if (Items[i].EmbeddedControl != null)
							Items[i].EmbeddedControl.RenderControl(output);
						else
						{
							output.Write(Items[i].Text);
						}
						output.RenderEndTag();
						//output.RenderEndTag();
						output.Write("</td>");
						output.RenderEndTag();
						//output.RenderEndTag();
						output.Write("</table>");
						output.RenderEndTag();
						output.RenderEndTag();
					}
					else
					{
						output.AddAttribute(HtmlTextWriterAttribute.Value, Items[i].Value);
						if (Items[i].Selected)
							output.AddAttribute(HtmlTextWriterAttribute.Selected, "true");
						output.RenderBeginTag(HtmlTextWriterTag.Option);
						output.Write(Items[i].TextOnly);
						output.RenderEndTag();
					}
				}
		}

		/// <summary>
		/// Determine if we need to register the client side script and render the calendar, selectors with validation or selectors only.
		/// </summary>
		/// <returns>0 if scripting not allowed, 1 if not an uplevel browser but scripting allowed, 2 if all is OK.</returns>
		private bool AllowDHTML() 
		{
			if (this.DropDownListType == ToolDropDownListType.NotSet)
			{
				Page page = Page;
			
				if (page == null || page.Request == null || !page.Request.Browser.JavaScript ||	!(page.Request.Browser.EcmaScriptVersion.CompareTo(new Version(1, 2)) >= 0)) 
					return false;

				System.Web.HttpBrowserCapabilities browser = page.Request.Browser; 

				if (((browser.Browser.ToUpper().IndexOf("IE") > -1 && browser.MajorVersion >= 4)
					|| (browser.Browser.ToUpper().IndexOf("NETSCAPE") > -1 && browser.MajorVersion >= 5)))
					return true;

				else if (browser.Browser.ToUpper().IndexOf("OPERA") > -1 && browser.MajorVersion >= 3)
					return true;

				return false;
			}
			else
			{
				if (this.DropDownListType == ToolDropDownListType.DHTML)
					return true;
				else
					return false;
			}
		}

		#endregion

		#region IPostBackEventHandler

		/// <summary>
		/// Enables the control to process an event raised when a form is posted to the server.
		/// </summary>
		/// <param name="eventArgument">A String that represents an optional event argument to be passed to the event handler.</param>
		void IPostBackEventHandler.RaisePostBackEvent(String eventArgument)
		{
			//Page.Trace.Write(this.ID, "RaisePostBackEvent...");
			//OnSelectedIndexChanged(EventArgs.Empty);
		}

		#endregion

		#region IPostBackDataHandler

		/// <summary>
		/// Processes post-back data from the control.
		/// </summary>
		/// <param name="postDataKey">The key identifier for the control.</param>
		/// <param name="postCollection">The collection of all incoming name values.</param>
		/// <returns>True if the state changes as a result of the post-back, otherwise it returns false.</returns>
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{
			//Page.Trace.Write(this.ID, "LoadPostData...");

			/*int currentSelectedIndex = -1;

			if (this.AllowDHTML())
			{
				_oldSelectedIndex = Int32.Parse(postCollection[UniqueID + "_oldSelectedIndex"]);

				currentSelectedIndex = Int32.Parse(postCollection[UniqueID + "_selectedIndex"]);

				SelectedIndex = currentSelectedIndex;
			}
			else
			{
				string _selectedValue = postCollection[UniqueID + "_drop"];

				for(int index=0;index<this.Items.Count;index++)
				{
					if (this.Items[index].Value == _selectedValue)
					{
						this.Items[index].Selected = true;
						SelectedIndex = index;
						break;
					}
				}
			}

			if (_oldSelectedIndex != currentSelectedIndex)
				return true;*/

			try
			{
				string selectedColor = postCollection[ClientID + "_selectedColor"];
				if (selectedColor != string.Empty && selectedColor.Length == 7)
				{
                    _selectedColor = Utils.HexStringToColor(selectedColor);
				}
				else
					_selectedColor = Color.Empty;
				
			}

			catch
			{
				_selectedColor = Color.Empty;
			}

			return false;
		}

		/// <summary>
		/// Notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			//Page.Trace.Write(this.ID, "RaisePostDataChangedEvent...");
			//OnSelectedIndexChanged(EventArgs.Empty);
		}

		#endregion

		#endregion

		/// <summary>
		/// Notifies the Popup control to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{
			base.OnPreRender(e);

			_allowDHTML = AllowDHTML();

			if (_allowDHTML)
			{
				if (((base.Page != null) && base.Enabled))
				{
					RegisterAPIScriptBlock(); 
				}
			}

			RegisterAPIScriptBlock();
			
			bool isNs6 = false;
			System.Web.HttpBrowserCapabilities browser = Page.Request.Browser; 
			if (browser.Browser.ToUpper().IndexOf("NETSCAPE") != -1)
				isNs6 = true;

			if (isNs6)
			{
				_widthPopup = 240;
				_heightPopup = 208;
			}
			else
			{
				_widthPopup = 245;
				_heightPopup = 235;
			}
			

		} 

		/// <summary>
		/// Renders the specified output.
		/// </summary>
		/// <param name="writer">The output.</param>
		protected override void Render(HtmlTextWriter writer)
		{
			if (writer == null)
			{
				throw new InvalidOperationException($"{nameof(writer)} is null");
			}

			var isBrowserIe = Page.Request.Browser.Browser.Equals(Ie, StringComparison.OrdinalIgnoreCase);

			RenderHttpContext(writer, isBrowserIe);

			writer.AddAttribute(HtmlTextWriterAttribute.Type, Hidden);
			writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
			writer.AddAttribute(HtmlTextWriterAttribute.Value, DateTime.Now.Ticks.ToString());
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();

			writer.AddAttribute(HtmlTextWriterAttribute.Type, HiddenSmall);
			writer.AddAttribute(HtmlTextWriterAttribute.Value, string.Empty);
			writer.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_selectedColor");
			writer.AddAttribute(HtmlTextWriterAttribute.Name, $"{ClientID}_selectedColor");
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();

			var itemsAreaHeightToAdjust = ItemsAreaHeight == Unit.Empty;

			writer.AddAttribute(HtmlTextWriterAttribute.Type, HiddenSmall);
			writer.AddAttribute(HtmlTextWriterAttribute.Value, itemsAreaHeightToAdjust.ToString());
			writer.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_ItemsAreaHeightToAdjust");
			writer.AddAttribute(HtmlTextWriterAttribute.Name, $"{UniqueID}_ItemsAreaHeightToAdjust");
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();

			if (_allowDHTML)
			{
				RenderAllowDhtml(writer, isBrowserIe);
			}
			else
			{
				RenderNotAllowDhtml(writer);
			}
		}

		private void RenderNotAllowDhtml(HtmlTextWriter writer)
		{
			Page?.VerifyRenderingInServerForm(this);

			writer.AddAttribute(HtmlTextWriterAttribute.Name, $"{UniqueID}_drop");
			if (AutoPostBack && Page != null)
			{
				var text = Page.GetPostBackClientEvent(this, string.Empty);
				writer.AddAttribute(HtmlTextWriterAttribute.Onchange, text);
				writer.AddAttribute(Language, Javascript);
			}

			writer.RenderBeginTag(HtmlTextWriterTag.Select);
			RenderItems(writer);
			writer.RenderEndTag();
		}

		private void RenderAllowDhtml(HtmlTextWriter writer, bool isBrowserIe)
		{
			var style = RenderBorderBackgroundSize(writer);

			RenderMouseEvents(writer, style);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.Write(Newline);

			writer.AddAttribute(HtmlTextWriterAttribute.Width, Width.IsEmpty ? HundredPercent : Width.Value.ToString());

			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.AddAttribute(HtmlTextWriterAttribute.Width, HundredPercent);
			writer.AddAttribute(HtmlTextWriterAttribute.Height, HundredPercent);
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, Zero);
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, Zero);
			writer.AddAttribute(HtmlTextWriterAttribute.Border, Zero);
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Width, HundredPercent);
			writer.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_text");
			writer.AddStyleAttribute(Padding, UseSquareColor ? TwoPxAll : ThreeZeroPxOneFourPx);

			writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, null);
			if (ForeColor != Color.Empty)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Color, Utils.Color2Hex(ForeColor));
			}

			Utils.AddStyleFontAttribute(writer, Font);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			if (UseSquareColor)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_squareColor");
				writer.AddAttribute(HtmlTextWriterAttribute.Width, ForteenPx);
				writer.AddAttribute(HtmlTextWriterAttribute.Height, ForteenPx);
				writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, Zero);
				writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, Zero);
				writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(SelectedColor));
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(Color.Silver));
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, Solid);
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, OnePx);
				writer.RenderBeginTag(HtmlTextWriterTag.Table);
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.RenderEndTag();
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
			else
			{
				writer.Write($"<img src='{Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory, Image, ColorImage, Page, GetType())}'>");
			}

			RenderDropDownImage(writer);

			style = RenderBorderFooter(writer);

			RenderAllowDhtmlFooter(writer, isBrowserIe, style);
		}

		private void RenderDropDownImage(HtmlTextWriter writer)
		{
			writer.RenderEndTag();
			writer.AddAttribute(HtmlTextWriterAttribute.Height, HundredPercent);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.AddAttribute(HtmlTextWriterAttribute.Width, HundredPercent);
			writer.AddAttribute(HtmlTextWriterAttribute.Height, HundredPercent);
			writer.AddAttribute(HtmlTextWriterAttribute.Width, HundredPercent);
			writer.AddAttribute(HtmlTextWriterAttribute.Height, HundredPercent);
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, Zero);
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, Zero);
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Height, HundredPercent);
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, Zero);
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, Zero);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.AddAttribute(HtmlTextWriterAttribute.Border, Zero);

			var toolbarParent = Parent as Toolbar;
			if (!string.IsNullOrEmpty(DropDownImage))
			{
				writer.AddAttribute(
					HtmlTextWriterAttribute.Src,
					toolbarParent != null ? Utils.ConvertToImageDir(toolbarParent.ImagesDirectory, DropDownImage, DownImage, Page, GetType()) : DropDownImage);
			}
			else
			{
				if (toolbarParent != null)
				{
					if (Fx1ConditionalHelper<bool>.GetFx1ConditionalValue(false, true))
					{
						DropDownImage = DownImage;
					}

					writer.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertToImageDir(toolbarParent.ImagesDirectory, DropDownImage, DownImage, Page, GetType()));
				}
				else
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Src, DownImage);
				}
			}
		}

		private StringBuilder RenderBorderFooter(HtmlTextWriter writer)
		{
			StringBuilder style;
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.Write(TableClose);

			style = new StringBuilder();
			style.Append(StyleOpen);
			if (WindowBorderColor != Color.Empty)
			{
				style.Append($"border-color:{Utils.Color2Hex(WindowBorderColor)};");
			}

			style.Append($"border-style:{WindowBorderStyle.ToString()};");
			if (WindowBorderWidth != Unit.Empty)
			{
				style.Append($"border-width:{WindowBorderWidth.ToString()};");
			}

			if (BackColorItems != Color.Empty)
			{
				style.Append($"background-color:{Utils.Color2Hex(BackColorItems)};");
			}

			var cursor = CursorHand;
			if (Page != null && !string.Equals(Page.Request.Browser.Browser, Ie, StringComparison.OrdinalIgnoreCase))
			{
				cursor = CursorPointer;
			}

			style.Append(cursor);

			style.Append(DisplayNone);
			style.Append(DoubleQuotes);
			return style;
		}

		private void RenderAllowDhtmlFooter(HtmlTextWriter writer, bool isBrowserIe, StringBuilder style)
		{
			var table = new StringBuilder();
			table.Append(TableOpen);
			table.Append($" id={ClientID}_items");
			table.Append(CellPaddingSpacingZero);
			table.Append(OnMouseEnterStyleDisplay);
			table.Append($" onmouseleave=ATB_closeDropDownList('{ClientID}')");
			table.Append($" {style}");
			table.Append(TagSuffix);
			writer.Write(table);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			writer.Write(
				$"<div id=\"{ClientID + "_divItems"}\" style=\"{(isBrowserIe ? OverflowAuto : string.Empty)}height:{(ItemsAreaHeight != Unit.Empty ? ItemsAreaHeight.ToString() : OneTwentyPx)};\">");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, Cellpadding != Unit.Empty ? Cellpadding.ToString() : Zero);
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, Cellspacing != Unit.Empty ? Cellspacing.ToString() : Zero);

			writer.AddAttribute(HtmlTextWriterAttribute.Border, Zero);
			writer.AddAttribute(HtmlTextWriterAttribute.Width, HundredPercent);
			writer.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_tableItems");
			writer.RenderBeginTag(HtmlTextWriterTag.Table); // Open Table
			RenderItems(writer);
			writer.RenderEndTag();

			writer.Write(DivClose);
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.Write(TableClose);

			var enableSsl = EnableSsl ? SourceBlankHtm : string.Empty;

			writer.Write($"<iframe id=\"{ClientID}_mask\" scrolling=\"no\" frameborder=\"0\" style=\"position: absolute;display: none;\" {enableSsl}></iframe>");
		}

		private void RenderMouseEvents(HtmlTextWriter writer, StringBuilder style)
		{
			var table = new StringBuilder();
			table.Append(TableOpen);
			table.Append($" id={ClientID}_ddl");

			if (Width != Unit.Empty)
			{
				table.Append($" width={Width.ToString()}");
			}

			if (Height != Unit.Empty)
			{
				table.Append($" height={Height.ToString()}");
			}

			table.Append(CellPaddingSpacingZero);
			table.Append(style);
			table.Append($" onclick=ATB_openDropDownList('{ClientID}');");

			if (AllowRollOver)
			{
				table.Append($" onmouseover=\"this.style.borderColor='{Utils.Color2Hex(BorderColorRollOver)}';\"");
			}

			table.Append($" onmouseout=\"this.style.borderColor='{Utils.Color2Hex(BorderColor)}';\"");
			table.Append(TagSuffix);
			writer.Write(table);
		}

		private StringBuilder RenderBorderBackgroundSize(HtmlTextWriter writer)
		{
			Page?.VerifyRenderingInServerForm(this);

			if (AutoPostBack)
			{
				writer.Write($"<input type=\"hidden\" name=\"{UniqueID}_doPostBackWhenClick\" id=\"{ClientID}_doPostBackWhenClick\" value=\"True\">");
				writer.AddAttribute(HtmlTextWriterAttribute.Type, Hidden);
				writer.AddAttribute(HtmlTextWriterAttribute.Name, $"{ClientID}_selectedIndexChanged");
				writer.AddAttribute(HtmlTextWriterAttribute.Value, Page.GetPostBackEventReference(this, string.Empty));
				writer.RenderBeginTag(HtmlTextWriterTag.Input);
				writer.RenderEndTag();
			}
			else
			{
				writer.Write($"<input type=\"hidden\" name=\"{UniqueID}_doPostBackWhenClick\" id=\"{ClientID}_doPostBackWhenClick\" value=\"False\">");
			}

			writer.Write($"<input type=\"hidden\" name=\"{UniqueID}_oldSelectedIndex\" id=\"{ClientID}_oldSelectedIndex\" value=\"{SelectedIndex}\">");
			writer.Write($"<input type=\"hidden\" name=\"{UniqueID}_selectedIndex\" id=\"{ClientID}_selectedIndex\" value=\"{SelectedIndex}\">");

			writer.Write($"<input type=\"hidden\" name=\"{UniqueID}_changeToSelectedText\" id=\"{ClientID}_changeToSelectedText\" value=\"{ChangeToSelectedText}\">");

			var enumerator = Style.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				var currentValue = enumerator.Current as string;
				if (currentValue != null)
				{
					writer.AddStyleAttribute(currentValue, Style[currentValue]);
				}
			}

			ControlStyle.AddAttributesToRender(writer);

			var backImage = string.Empty;
			if (string.IsNullOrEmpty(BackImage))
			{
				var parentToolbar = Parent as Toolbar;
				backImage = parentToolbar != null ? $"url({Utils.ConvertToImageDir(parentToolbar.ImagesDirectory, BackImage)})" : $"url({BackImage})";
			}

			var style = new StringBuilder(StyleOpen);
			if (BackColor != Color.Empty)
			{
				style.Append($"background-color:{Utils.Color2Hex(BackColor)};");
			}

			if (BorderColor != Color.Empty)
			{
				style.Append($"border-color:{Utils.Color2Hex(BorderColor)};");
			}

			style.Append($"border-style:{BorderStyle.ToString()};");
			if (BorderWidth != Unit.Empty)
			{
				style.Append($"border-width:{BorderWidth};");
			}

			if (!string.IsNullOrEmpty(backImage))
			{
				style.Append($"background-image:{backImage};");
			}

			style.Append(DoubleQuotes);
			return style;
		}

		private void RenderHttpContext(HtmlTextWriter writer, bool isBrowserIe)
		{
			if (HttpContext.Current != null)
			{
				writer.Write(JavaScriptOpen);
				writer.Write($"function ATB_create_{ClientID}_CustomColors(e)\n");
				writer.Write($"{{{Newline}");

				var spacer = string.Empty;
				var tableName = isBrowserIe ? AtbBuildColorTable : AtbBuildColorTableMozilla;

				if (!isBrowserIe)
				{
					spacer = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(
						Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory, string.Empty, SpacerImage, Page, GetType()),
						Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory, SpacerImage));
					spacer = $",\"{spacer}\"";
				}

				writer.Write(
					$"ATB_createPopup(\"{ClientID}_CustomColors\",0, 0, {_widthPopup}, {_heightPopup}, \"Custom Colors\", "
					+ $"{tableName}(\"{ClientID}\",\"ATB_SetSelectedColor('{ClientID}',$color$);ATB_hidePopup('{ClientID}_CustomColors');"
					+ $"{ClientColorSelected}\",false,\"f\"{spacer}),\"#DDDDDD\",\"#DDDDDD\",\"Outset\",\"2\",\"#4F6DA5\",\"#808080\",\"#FFFFFF\",\"NotSet\",\"0\","
					+ "\"#FFFFFF\",\"#DDDDDD\",\"#FFFFFF\",\"NotSet\",\"0\", \"#808080\",\"True\",\"False\",undefined,\"True\")");

				writer.Write($"\nATB_setTitleGradient(\"{ClientID}_CustomColors\",\"#0A246A\",\"#A6CAF0\")\n");
				writer.Write($"\nATB_setCloseImage(\"{ClientID}_CustomColors\",\"{Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory, CloseImage, CloseImage, Page, GetType())}\");");
				writer.Write($"}}{Newline}");
				writer.Write($"window.RegisterEvent(\"onload\", ATB_create_{ClientID}_CustomColors);\n");
				writer.Write(ScriptClose);

				AddColorPicker();
			}
		}

		private void AddColorPicker()
		{
			//Editor editor = (Editor)this.Parent.Parent.Parent;

			

			string s = string.Empty;

			s +="<table class=\"ATB_clsDropDownContent\">\n";
			s +=" 	<tr>\n";
			s +="		<td><span class=\"ATB_clsFont\">Standard Colors</span>\n";
			s +="			<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n";
			s +=" 				<tr>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','black');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +=" 									<table style=\"background-color: black;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>\n";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','white');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +="									<table class=\"ATB_clsDropDownItem\" style=\"background-color: white;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>\n";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','#008000');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +="									<table class=\"ATB_clsDropDownItem\" style=\"background-color: #008000;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','#800000');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +="									<table class=\"ATB_clsDropDownItem\" style=\"background-color: #800000;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>\n";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','#808000');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +="									<table class=\"ATB_clsDropDownItem\" style=\"background-color: #808000;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>\n";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','#000080');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +="									<table class=\"ATB_clsDropDownItem\" style=\"background-color: #000080;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>\n";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','#800080');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +="									<table class=\"ATB_clsDropDownItem\" style=\"background-color: #800080;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>\n";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','#808080');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +="									<table class=\"ATB_clsDropDownItem\" style=\"background-color: #808080;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>\n";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="				</tr>\n";
			s +="				<tr>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','#FFFF00');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +="									<table class=\"ATB_clsDropDownItem\" style=\"background-color: #FFFF00;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>\n";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','#00FF00');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +="									<table class=\"ATB_clsDropDownItem\" style=\"background-color: #00FF00;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>\n";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','#00FFFF');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +="									<table class=\"ATB_clsDropDownItem\" style=\"background-color: #00FFFF;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>\n";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','#FF00FF');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +="									<table class=\"ATB_clsDropDownItem\" style=\"background-color: #FF00FF;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>\n";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','#C0C0C0');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +="									<table class=\"ATB_clsDropDownItem\" style=\"background-color: #C0C0C0;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>\n";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','#FF0000');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +="									<table class=\"ATB_clsDropDownItem\" style=\"background-color: #FF0000;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>\n";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','#0000FF');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +="									<table class=\"ATB_clsDropDownItem\" style=\"background-color: #0000FF;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>\n";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="					<td>\n";
			s += string.Format(" 		<table style=\"border : 1px {1} solid\" onclick=\"ATB_SetSelectedColor('{0}','#008080');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="							<tr>\n";
			s +="								<td align=\"center\">\n";
			s +="									<table class=\"ATB_clsDropDownItem\" style=\"background-color: #008080;\" width=\"12\" height=\"12\">\n";
			s +="										<tr>\n";
			s +="											<td></td>\n";
			s +="										</tr>\n";
			s +="									</table>\n";
			s +="								</td>\n";
			s +="							</tr>\n";
			s +="						</table>\n";
			s +="					</td>\n";
			s +="				</tr>\n";
			s +="			</table>\n";
			s +="			<hr size=\"2\" width=\"100%\">\n";
			//s +=string.Format("			<table class=\"ATB_clsColorCont\" onclick=\"ATB_SetPopupPosition('{1}','{0}_CustomColors'); ATB_showPopup('{0}_CustomColors');{1};ATB_OnColorOff(this);\" onmouseover=\"ATB_OnColorOver(this);\" onmouseout=\"ATB_OnColorOff(this);\" width=\"100%\">\n",ClientID,ClientColorSelected,""/*editor.ClientID*/);
			s +=string.Format("			<table style=\"border : 1px {1} solid\" onclick=\"ATB_showPopup('{0}_CustomColors');ATB_OnColorOff(this,'{1}','{1}');\" onmouseover=\"ATB_OnColorOver(this,'{2}','{3}');\" onmouseout=\"ATB_OnColorOff(this,'{1}','{1}');\" width=\"100%\">\n",ClientID,Utils.Color2Hex(BackColorItems),Utils.Color2Hex(SelectedBackColorBorder),Utils.Color2Hex(SelectedBackColorItem));
			s +="				<tr>\n";
			s +="					<td align=\"center\"><span class=\"ATB_clsFont\">More Colors...</span></td>\n";
			s +="				</tr>\n";
			s +="			</table>\n";
			s +="		</td>\n";
			s +="	</tr>\n";
			s +="</table>\n";

			this.Items.Add(new ToolItem(s,string.Empty));
		}

		private string ClientColorSelected
		{
			get
			{
				object clientColorSelected = ViewState["_clientColorSelected"];
				if (clientColorSelected != null)
					return (String)clientColorSelected;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_clientColorSelected"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the popup width.
		/// </summary>
		/// <value>The width popup.</value>
		public int WidthPopup
		{
			get {return _widthPopup;}
			set {_widthPopup = value;}
		}


		/// <summary>
		/// Gets or sets the popup height.
		/// </summary>
		/// <value>The popup height.</value>
		public int HeightPopup
		{
			get {return _heightPopup;}
			set {_heightPopup = value;}
		}

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		/// <value>The image.</value>
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
		/// Gets or sets a value indicating whether use square color in the title.
		/// </summary>
		/// <value><c>true</c> if use square color in the title; otherwise, <c>false</c>.</value>
		[
			DefaultValue(false)
		]
		public bool UseSquareColor
		{
			get
			{
				object useSquareColor = ViewState["_useSquareColor"];
				if (useSquareColor != null)
					return (bool)useSquareColor;
				else
					return false;
			}

			set
			{
				ViewState["_useSquareColor"] = value;
			}
		}


		/// <summary>
		/// Gets or sets the selected color.
		/// </summary>
		/// <value>The selected color.</value>
		[
			Browsable(false)
		]
		public Color SelectedColor
		{
			get {return _selectedColor;}

			set { _selectedColor = value;}

		}

		/// <summary>
		/// Renders at the design time.
		/// </summary>
		/// <param name="output">The output.</param>
        public override void RenderDesign(HtmlTextWriter output)
        {
            ToolColorPickerDesigner.DesignColorPicker(ref output, this);
        }
	}
}
