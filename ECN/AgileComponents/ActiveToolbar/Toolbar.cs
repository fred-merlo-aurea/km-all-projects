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
using System.Drawing;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Text;
using ActiveUp.WebControls.Common;
using ActiveUp.WebControls.Common.Extension;

namespace ActiveUp.WebControls
{
	#region class Toolbar

	/// <summary>
	/// Represents a Toolbar object.
	/// </summary>
	[
		ToolboxData("<{0}:Toolbar runat=server></{0}:Toolbar>"),
		Designer(typeof(ToolbarDesigner)),
		ParseChildren(true,"Tools"),
		ToolboxBitmap(typeof(Toolbar), "ToolBoxBitmap.Toolbar.bmp"),
		Serializable,
	]
	public class Toolbar : CoreWebControl, IPostBackDataHandler, INamingContainer
	{
		#region Variables

		/// <summary>
		/// Client side script block.
		/// </summary>
		private string CLIENTSIDE_API = null; 

		/// <summary>
		/// Unique client script key.
		/// </summary>
		private const string SCRIPTKEY = "ActiveToolbar";

		private const string DragAndDropImageFx1 = "Dock.gif";

		/// <summary>
		/// Collection of <see cref="ToolBase"/>.
		/// </summary>
		private ToolCollection _tools;

		private string _scriptDirectory;
		private const string ScriptEntryPoint = "ATB_testIfScriptPresent()";

#if (LICENSE)
		/// <summary>
		/// Used for the license counter.
		/// </summary>
		internal static int _useCounter;

		//private string _license = string.Empty;
#endif

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Toolbar() : base()
		{
			BackColor = Color.FromArgb(0xE0,0xE0,0xE0);
			BorderColor = Color.Silver;
			BorderStyle = BorderStyle.Outset;
			BorderWidth = 2;
			_tools = new ToolCollection(this.Controls);
			//DragAndDropImage = "dock.gif"; (init in property get)
			//ImagesDirectory = "icons/"; (init in property)
			//_scriptDirectory = "/aspnet_client/ActiveWebControls/" + StaticContainer.VersionString + "/";
#if (!FX1_1)
			_scriptDirectory = string.Empty;
#else
			_scriptDirectory = Define.SCRIPT_DIRECTORY;
#endif
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the background color of the <see cref="Toolbar"/>.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			DefaultValue(typeof(Color),"#E0E0E0"),
			NotifyParentProperty(true),
			Description("Background color of the toolbar.")
		]
		public override Color BackColor
		{
			get {return base.BackColor;}
			set {base.BackColor = value;}
		}

		/// <summary>
		/// Gets or sets the border color of the Web control.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			DefaultValue(typeof(Color),"Silver"),
			NotifyParentProperty(true),
			Description("Border color of the toolbar.")
		]
		public override Color BorderColor
		{
			get {return base.BorderColor;}
			set {base.BorderColor = value;}
		}

		/// <summary>
		/// Gets or sets the border width of the Web server control.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			DefaultValue(typeof(Unit),"2px"),
			NotifyParentProperty(true),
			Description("Border width of the toolbar.")
		]
		public override Unit BorderWidth
		{
			get {return base.BorderWidth;}
			set {base.BorderWidth = value;}
		}

		/// <summary>
		/// Gets or sets the border style of the Web server control.
		/// </summary>
		[
			Bindable(true),
			Category("Appearance"),
			DefaultValue(typeof(BorderStyle),"Outset"),
			NotifyParentProperty(true),
			Description("Border style of the toolbar.")
		]
		public override BorderStyle BorderStyle
		{
			get { return base.BorderStyle;}
			set { base.BorderStyle = value;}
		}

		/// <summary>
		/// Gets the collection of <see cref="ToolBase"/> items.
		/// </summary>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		NotifyParentProperty(true),
		PersistenceMode(PersistenceMode.InnerDefaultProperty),
		Description("Collection of Toolbase items.")
		]
		//public ControlCollection Tools
		public ToolCollection Tools
		{
			get {return _tools;}

			/*get
			{
				object tools = ViewState["_tools"];

				if (tools == null)
					ViewState["_tools"] = new ToolCollection(this.Controls);
				return (ToolCollection)ViewState["_tools"];
			}*/
		}

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
		public virtual string ScriptDirectory
		{
			get
			{
                if (_scriptDirectory == null)
                    //return "/aspnet_client/ActiveWebControls/" + StaticContainer.VersionString + "/";				return _scriptDirectory;
#if (!FX1_1)
                    return string.Empty;
#else
					return Define.SCRIPT_DIRECTORY;
#endif
                else
                    return _scriptDirectory;
			}
			set
			{
				_scriptDirectory = value;
			}
		}

		/// <summary>
		/// Gets or sets the cell spacing of the table representing the toolbar.
		/// </summary>
		public int CellSpacing
		{
			get
			{
				object _cellSpacing;
				_cellSpacing = ViewState["_cellSpacing"];
				if (_cellSpacing != null)
					return (int)_cellSpacing; 
				return 0;
			}
			set
			{
				ViewState["_cellSpacing"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the cell padding of the table representing the toolbar.
		/// </summary>
		public int CellPadding
		{
			get
			{
				object _cellSpacing;
				_cellSpacing = ViewState["_cellPadding"];
				if (_cellSpacing != null)
					return (int)_cellSpacing; 
				return 0;
			}
			set
			{
				ViewState["_cellPadding"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the toolbar.
		/// </summary>
		public string Name
		{
			get
			{
				string _name;
				_name = ((string) base.ViewState["_name"]);
				if (_name != null)
					return _name; 
				return string.Empty;
			}
			set
			{
				ViewState["_name"] = value;
			}
		}

		/*
		/// <summary>
		/// Gets the reference of the toolbars container.
		/// </summary>
		public ToolbarsContainer Container
		{
			get
			{
				if (this.Parent != null && this.Parent.GetType().ToString() == "ActiveUp.WebControls.ToolbarsContainer")
					return (ToolbarsContainer)this.Parent;
				else
					return null;
			}
		}*/

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

		internal Unit Left
		{
			get
			{
				object left = ViewState["_left"];
				if (left != null)
					return (Unit)left;
				else 
					return Unit.Empty;
			}

			set
			{
				ViewState["_left"] = value;
			}
		}

		internal Unit Top
		{
			get
			{
				object top = ViewState["_top"];
				if (top != null)
					return (Unit)top;
				else 
					return Unit.Empty;
			}

			set
			{
				ViewState["_top"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the direction of the toolbar.
		/// </summary>
		[
		Bindable(true), 
		Category("Appearance"), 
		DefaultValue(ToolbarDirection.Horizontal),
		Description("The direction to use for the toolbar.")
		] 
		public ActiveUp.WebControls.ToolbarDirection Direction
		{
			get
			{
				object _direction;
				_direction = ViewState["_direction"];
				if (_direction != null)
					return ((ActiveUp.WebControls.ToolbarDirection) _direction); 
				return ActiveUp.WebControls.ToolbarDirection.Horizontal;
			}
			set
			{
				ViewState["_direction"] = value;
			}
		}

		/// <summary>
		/// Layout to use for the toolbar.
		/// </summary>
		[
		Bindable(true), 
		Category("Appearance"), 
		DefaultValue(typeof(ToolbarLayout),"Table"),
		Description("The layout to use for the toolbar.")
		] 
		public ActiveUp.WebControls.ToolbarLayout Layout
		{
			get
			{
				object _layout;
				_layout = ViewState["_layout"];
				if (_layout != null)
					return ((ActiveUp.WebControls.ToolbarLayout) _layout); 
				return ActiveUp.WebControls.ToolbarLayout.Table;
			}
			set
			{
				ViewState["_layout"] = value;
			}
		}

		/// <summary>
		/// Gets or sets image used for the drag and drop.
		/// </summary>
		/// <value>The image used for the drag and drop.</value>
		[
			Bindable(true),
			Category("Appearance"),
			Description("The image used for the drag and drop."),
			Fx1ConditionalDefaultValue("", DragAndDropImageFx1)
		]
		public string DragAndDropImage
		{
			get
			{
				var defaultValue = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, DragAndDropImageFx1);
				return ViewStateHelper.GetFromViewState(ViewState, nameof(DragAndDropImage), defaultValue);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(DragAndDropImage), value);
			}
		}

		/// <summary>
		/// Gets or sets if the popup can be dragged.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(false),
		NotifyParentProperty(true),
		Description("Indicates if the toolbar can be dragged or not.")
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
					return false;

			}

			set
			{
				ViewState["_dragable"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>The position.</value>
		public Position Position
		{
			get
			{
				object position = ViewState["_position"];
				if (position != null)
					return (Position)position;
				else
					return Position.Absolute;
			}

			set
			{
				ViewState["_position"] = value;
			}
		}
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
		/// Image used as background of the tool.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(""),
		Browsable(true),
		NotifyParentProperty(true),
		Description("Image used as background of the toolbar.")
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

		#endregion

		#region Methods

		/// <summary>
		/// Renders at the design time.
		/// </summary>
		/// <param name="output">The output.</param>
		public void RenderDesign(HtmlTextWriter output)
		{
			ToolbarDesigner.DesignToolbar(ref output, this);
		}

		/// <summary>
		/// Register the client-side script block in the ASPX page.
		/// </summary>
		public void RegisterAPIScriptBlock() 
		{
			/*try
			{
				Editor editor = ((Editor)this.Parent.Parent);
				if (editor.IsCallback)
					return;
			}

			catch {}*/
			// Register the script block is not allready done.

			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
				if ((this.ExternalScript == null || this.ExternalScript == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
				{
#if (!FX1_1)
                    Page.RegisterHiddenField("Containers", "");          
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
                    Page.RegisterHiddenField("Containers", "");

					if (this.ScriptDirectory.StartsWith("~"))
						this.ScriptDirectory = this.ScriptDirectory.Replace("~", System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActiveToolbar.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
			}


			Page.TestAndRegisterScriptBlock(SCRIPTKEY, ScriptDirectory, ScriptEntryPoint);
		}


		/// <summary>
		/// Notifies the Popup control to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{
			this.RegisterAPIScriptBlock();
			if (base.Page != null)
				Page.RegisterRequiresPostBack(this);
			
			string scriptKey = this.ClientID + "_" + SCRIPTKEY + "_Init";
			System.Text.StringBuilder initValues = new System.Text.StringBuilder();
			initValues.Append("<script language='javascript'>\n");
			// Direction
			initValues.Append("var ");
			initValues.Append(this.ClientID);
			initValues.Append("_direction = '");
			initValues.Append(Direction.ToString());
			initValues.Append("';\n</script>");
			// Back Color Dock
			/*initValues.Append("var ");
			initValues.Append(this.ClientID);
			initValues.Append("_BackColorDock = '");
			initValues.Append(Utils.Color2Hex(BackColorDock));
			initValues.Append("';\n</script>");*/

			Page.RegisterStartupScript(scriptKey, initValues.ToString());

			base.OnPreRender(e);
		
		}

		private void RenderToolbar(HtmlTextWriter output)
		{
			RenderToolbarHeader(output);

			var style = new StringBuilder();
			style.Append("style=\"");

			if (BorderColor != Color.Empty)
			{
				style.Append($"border-color:{Utils.Color2Hex(BorderColor)};");
			}

			if (BorderStyle != BorderStyle.NotSet)
			{
				style.Append($"border-style:{BorderStyle};");
			}

			if (BorderWidth != Unit.Empty)
			{
				style.Append($"border-width:{BorderWidth};");
			}

			if (BackColor != Color.Empty)
			{
				style.Append($"background-color:{Utils.Color2Hex(BackColor)};");
			}

			if (BackImage != string.Empty)
			{
				style.Append($"background-image:url({Utils.ConvertToImageDir(ImagesDirectory, BackImage)});");
			}

			style.Append("\"");

			RenderToolbarFooter(output, style);
		}

		private void RenderToolbarFooter(HtmlTextWriter output, StringBuilder style)
		{
			var table = new StringBuilder();
			table.Append("<table");
			table.Append($" cellpadding={CellPadding} cellspacing={CellSpacing}");
			table.Append(" ondragstart=return false;");
			table.Append($" {style}");
			table.Append(">");
			output.Write(table);

			if(Direction == ToolbarDirection.Horizontal)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
			}

			if(Dragable)
			{
				if(Direction == ToolbarDirection.Vertical)
				{
					output.RenderBeginTag(HtmlTextWriterTag.Tr);
				}

				output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.AddStyleAttribute("cursor", "move");
				output.AddAttribute("onmousedown", $"return ATB_dockmousedown('{UniqueID}')");
				output.AddAttribute("onmouseup", "return ATB_dockmouseup()");
				output.AddAttribute(HtmlTextWriterAttribute.Src,
					Utils.ConvertToImageDir(ImagesDirectory, DragAndDropImage, "dock.gif", Page, GetType()));
				output.RenderBeginTag(HtmlTextWriterTag.Img);
				output.RenderEndTag();
				output.RenderEndTag();

				if(Direction == ToolbarDirection.Vertical)
				{
					output.RenderEndTag();
				}
			}

			for(var i = 0; i < Tools.Count; i++)
			{
				if(Direction == ToolbarDirection.Vertical)
				{
					output.RenderBeginTag(HtmlTextWriterTag.Tr);
				}

				output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_isToolbar{i}");
				output.RenderBeginTag(HtmlTextWriterTag.Td);

				Tools[i].EnableSsl = EnableSsl;
				Tools[i].RenderControl(output);

				output.RenderEndTag();
				if(Direction == ToolbarDirection.Vertical)
				{
					output.RenderEndTag();
				}
			}

			if(Direction == ToolbarDirection.Horizontal)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.Write("&nbsp;");
				output.RenderEndTag();

				output.RenderEndTag();
			}

			output.Write("</table>");
			output.Write("</div>");
		}

		private void RenderToolbarHeader(HtmlTextWriter output)
		{
			if(Dragable)
			{
				output.Write(
					"<iframe style=\"position: absolute; display: none; top: 0px; left: 0px;width:0px;height:0px;\" scrolling=\"no\" id=\"{0}\"></iframe>",
					$"{ClientID}_mask");

				output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
				output.AddAttribute(HtmlTextWriterAttribute.Value, Left.ToString());
				output.AddAttribute(HtmlTextWriterAttribute.Id, $"{UniqueID}_left");
				output.AddAttribute(HtmlTextWriterAttribute.Name, $"{UniqueID}_left");
				output.RenderBeginTag(HtmlTextWriterTag.Input);
				output.RenderEndTag();

				output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
				output.AddAttribute(HtmlTextWriterAttribute.Value, Top.ToString());
				output.AddAttribute(HtmlTextWriterAttribute.Id, $"{UniqueID}_top");
				output.AddAttribute(HtmlTextWriterAttribute.Name, $"{UniqueID}_top");
				output.RenderBeginTag(HtmlTextWriterTag.Input);
				output.RenderEndTag();
			}

			var style = new StringBuilder();
			style.Append("style=\"");
			var enumerator = Style.Keys.GetEnumerator();
			while(enumerator.MoveNext())
			{
				var val = (string) enumerator.Current;
				if(string.Equals(val, "LEFT", StringComparison.OrdinalIgnoreCase) && Left == Unit.Empty)
				{
					Left = new Unit(Style[val]);
				}
				else if(string.Equals(val, "TOP", StringComparison.OrdinalIgnoreCase) && Top == Unit.Empty)
				{
					Top = new Unit(Style[val]);
				}
				else if(!string.Equals(val, "POSITION", StringComparison.OrdinalIgnoreCase))
				{
					var currentValue = (string) enumerator.Current;

					if (currentValue == null)
					{
						throw new InvalidOperationException();
					}

					style.Append($"{currentValue}:{Style[currentValue]};");
				}
			}

			if(Left != Unit.Empty)
			{
				style.Append($"left:{Left};");
			}

			if(Top != Unit.Empty)
			{
				style.Append($"top:{Top};");
			}

			style.Append($"position:{Position.ToString()};");
			style.Append("border-width:0px;");
			style.Append("\"");

			output.Write($"<div id={UniqueID} {style}>");
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output">Output stream that contains the HTML used to represent the control.</param>
		protected override void Render(HtmlTextWriter output)
		{
			
			/*output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Value,Direction.ToString());
			output.AddAttribute(HtmlTextWriterAttribute.Id, UniqueID + "_direction");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_direction");
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();*/

#if (LICENSE)
			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.ATB, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.HTB, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.AIE, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

			_useCounter++;

			if (!(this.Parent is ActiveUp.WebControls.ImageEditor) && !(this.Parent is ActiveUp.WebControls.ToolbarsContainer) && /*!licenseStatus.IsRegistered &&*/ Page != null && _useCounter == StaticContainer.UsageCount)
			{
				_useCounter = 0;
				output.Write(StaticContainer.TrialMessage);
			}
			else
			{
				this.RenderToolbar(output);
			}

#else
			RenderToolbar(output);
#endif
		}

		/*public override Control FindControl(string id)
		{
			foreach (Control c in Controls)
				if (c.ID == id)
					return c;
			return null;
		}*/

		/// <summary>
		/// Gets a real copy of the actual object. This will not return a reference.
		/// </summary>
		/// <returns>The new object.</returns>
		public Toolbar Clone()
		{	
			/*System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			System.IO.MemoryStream stream = new System.IO.MemoryStream();
			binFormatter.Serialize(stream, this);
			stream.Position = 0;
			return (Toolbar)binFormatter.Deserialize(stream);*/
			Toolbar t = new Toolbar();
			return t;
		}

		/// <summary>
		/// Finds the tool by specifing his id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public ToolBase FindTool(string id)
		{
			Page.Trace.Write("toolbase");
			foreach(ToolBase tool in this.Tools)
			{
				Page.Trace.Write(tool.ID + "-" + id);
				if (tool.ID == id)
					return tool;
			}

			return null;
		}

		/// <summary>
		/// Finds the tool by specifying his id.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public ToolBase FindTool(System.Type type)
		{
			foreach(ToolBase tool in this.Tools)
				if (tool.GetType() == type)
					return tool;

			return null;
		}

		#endregion

		#region Interface IPostBackDataHandler

		/// <summary>
		/// Processes post-back data from the control.
		/// <param name="postDataKey">The key identifier for the control.</param>
		/// <param name="postCollection">The collection of all incoming name values.</param>
		/// <returns>True if the state changes as a result of the post-back, otherwise it returns false.</returns>
		/// </summary>
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{
			Page.Trace.Write(this.ID, "LoadPostData...");
			
			if (postCollection[ID + "_left"] != null)
				Left = new Unit(postCollection[ID + "_left"]);
			else
				Left = Unit.Empty;

			if (postCollection[ID + "_top"] != null)
				Top = new Unit(postCollection[ID + "_top"]);
			else
				Top = Unit.Empty;
			
			return true;
		}

		/// <summary>
		/// Notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			Page.Trace.Write(this.ID, "RaisePostDataChangedEvent...");
		}

		#endregion

	}

	#endregion
}
