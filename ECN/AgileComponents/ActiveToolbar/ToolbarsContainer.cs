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
using System.Collections; 
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing;

using ActiveUp.WebControls.Common;
using ActiveUp.WebControls.Common.Extension;

namespace ActiveUp.WebControls
{
	#region class ToolbarsContainer

	/// <summary>
	/// Represents a ToolbarsContainer.
	/// </summary>
	[
	ToolboxData("<{0}:ToolbarsContainer runat=server></{0}:ToolbarsContainer>"),
	Designer(typeof(ToolbarsContainerDesigner)),
	ParseChildren(true,"Toolbars"),
	Serializable 
	]
	public class ToolbarsContainer : CoreWebControl, IPostBackEventHandler, IPostBackDataHandler
	{
		#region Variables
		private const string ScriptEntryPoint = "ATB_testIfScriptPresent()";
		private ToolbarCollection _toolbars = null;
		private string CLIENTSIDE_API = null; 
		private const string SCRIPTKEY = "ActiveToolbar";
		private const string Hidden = "hidden";
		private const string Left = "LEFT";
		private const string Top = "TOP";
		private const string LeftSmall = "left";
		private const string ZeroPx = "0px";
		private const string TopSmall = "top";
		private const string NonbreakingSpace = "&nbsp";
		private const string HundredPercent = "100%";
		private const string OnSelectStart = "onselectstart";
		private const string ReturnFalse = "return false";
		private const string BackgroundImage = "background-image";
		private const char Comma = ',';
		private const string PositionString = "position";
		private string _toolbarsRenderedByOther = "";
#if (LICENSE)
		//private string _license = string.Empty;
		private static int _useCounter;
#endif

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ToolbarsContainer()
		{
			//
			// TODO: Add constructor logic here
			//
			_toolbars = new ToolbarCollection(this.Controls);
			BackColor = Color.FromArgb(0xF0,0xE0,0xE0);
			BackColorDock = Color.FromArgb(0xFF,0x00,0x00);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the Toolbars present in the container. 
		/// </summary>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		NotifyParentProperty(true),
		PersistenceMode(PersistenceMode.InnerDefaultProperty)
		]
		public ToolbarCollection Toolbars
		{
			get { return _toolbars;}

			/*get
			{
				object toolbars = ViewState["_toolbars"];

				if (toolbars == null)
					ViewState["_toolbars"] = new ToolbarCollection(this.Controls);
				return (ToolbarCollection)ViewState["_toolbars"];
			}*/
		}

		/// <summary>
		/// Gets or sets the filename of the external script file.
		/// </summary>
		[Browsable(true)]
		public string ExternalScript
		{
			get
			{
				object externalScript = ViewState["ExternalScript"];
				if (externalScript == null)
					return string.Empty;
				return (string)externalScript;
			}
			set
			{
				ViewState["ExternalScript"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the relative or absolute path to the directory where ActiveToolbar API javascript file is.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the Html TextBox render.</remarks>
		[
		Browsable(true),
		Description("Gets or sets the relative or absolute path to the directory where ActiveToolbar API javascript file is."),
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
				object scriptDirectory = ViewState["ScriptDirectory"];
                if (scriptDirectory == null)
#if (!FX1_1)
                    return string.Empty;
#else
					return Define.SCRIPT_DIRECTORY;
#endif
				return (string)scriptDirectory;
			}
			set
			{
				ViewState["ScriptDirectory"] = value;
			}
		}

		/// <summary>
		/// Image used as background of the tool.
		/// </summary>
		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(""),
		Browsable(true),
		NotifyParentProperty(true),
		Description("Image used as background of the toolbar container.")
		]
		public string BackImage
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(BackImage), string.Empty);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(BackImage), value);
			}
		}

		/// <summary>
		/// Gets or sets the backround color of the container.
		/// </summary>
		[
		Bindable(true),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		DefaultValue(typeof(System.Drawing.Color),"#F0E0E0"),
		Category("Appearance")
		]
		public override Color BackColor
		{
			get {return base.BackColor;}
			set {base.BackColor = value;}
		}

		/// <summary>
		/// Gets or sets the background color of the container when a <see cref="Toolbar"/> can be docked.
		/// </summary>
		[
		Bindable(true),
		TypeConverterAttribute(typeof(System.Web.UI.WebControls.WebColorConverter)),
		DefaultValue(typeof(System.Drawing.Color),"#FF0000"),
		Category("Appearance")
		]
		public virtual Color BackColorDock
		{
			get
			{
				object backColorDock;
				backColorDock = ViewState["_backColorDock"];
				if (backColorDock != null)
					return (Color)backColorDock;
				else
					return Color.Empty;
			}

			set
			{
				ViewState["_backColorDock"] = value;
			}
		}

		private bool IsInternalToolbarsInitialized
		{
			get
			{
				object isInternalToolbarsInitialized = ViewState["_isInternalToolbarsInitialized"];
				if (isInternalToolbarsInitialized != null)
					return (bool)isInternalToolbarsInitialized;
				else
					return false;
			}

			set
			{
				ViewState["_isInternalToolbarsInitialized"] = value;
			}
		}

		private string InternalToolBarsID
		{
			get
			{
				object internalToolbarsID = ViewState["_internalToolBarsID"];
				if (internalToolbarsID != null)
					return (string)internalToolbarsID;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_internalToolBarsID"] = value;
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

		/// <summary>
		/// Gets or sets the toolbars position.
		/// </summary>
		/// <value>The toolbars position.</value>
		[
			Browsable(false)
		]
		public Position ToolbarsPosition
		{
			get
			{
				object toolbarsPosition = ViewState["_toolbarsPosition"];
				if (toolbarsPosition != null)
					return (Position)toolbarsPosition;
				else
					return Position.Absolute;
			}

			set
			{
				ViewState["_toolbarsPosition"] = value;
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
				return ViewStateHelper.GetFromViewState(ViewState, nameof(EnableSsl), false);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(EnableSsl), value);
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
			ToolbarsContainerDesigner.DesignToolbarsContainer(ref output, this);
		}		

		/// <summary>
		/// Register the Client-Side script block in the ASPX page.
		/// </summary>
		public void RegisterAPIScriptBlock() 
		{
			/*if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
				Page.RegisterHiddenField("Containers","");

				if ((this.ExternalScript == null || this.ExternalScript == string.Empty))
	 			{
					if (CLIENTSIDE_API == null)
						CLIENTSIDE_API = Utils.GetResource("ActiveUp.WebControls._resources.ActiveToolbar.js");
					
					if (!CLIENTSIDE_API.StartsWith("<script"))
						CLIENTSIDE_API = "<script language=\"javascript\">\n" + CLIENTSIDE_API + "\n";
					
					CLIENTSIDE_API += "\n</script>\n";

					//Page.RegisterClientScriptBlock(SCRIPTKEY, CLIENTSIDE_API);
				}
				else
				{
					if (this.ScriptDirectory.StartsWith("~"))
						this.ScriptDirectory = this.ScriptDirectory.Replace("~", System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
					//Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActiveToolbar.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
			}
			
			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY + "_" + ClientID))
			{
				if (Page.IsClientScriptBlockRegistered(SCRIPTKEY))
					CLIENTSIDE_API = string.Empty;

				CLIENTSIDE_API += "<script language=\"javascript\">\n";
				CLIENTSIDE_API += string.Format("function Init_{0}(e)\n",ClientID);
				CLIENTSIDE_API += "{\n";

				CLIENTSIDE_API += string.Format("ATB_addContainer('{0}');\n",ClientID);

				foreach (Toolbar t in this._toolbars)
				{
					
					if (this.Parent != null && this.Parent.GetType().ToString() != "ActiveUp.WebControls.Editor")
						CLIENTSIDE_API += string.Format("ATB_setToolbarAbsolute('{0}');",t.UniqueID);
					CLIENTSIDE_API += string.Format("ATB_addToolToContainer('{0}','{1}');",UniqueID,t.UniqueID);
				}
				CLIENTSIDE_API += "\n}\n";
				CLIENTSIDE_API += string.Format("window.RegisterEvent(\"onload\", Init_{0});\n",ClientID);
				CLIENTSIDE_API += "\n</script>\n";

				Page.RegisterClientScriptBlock(SCRIPTKEY + "_" + ClientID, CLIENTSIDE_API);

				if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY))
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\">var temp = '';</script>");

			}*/


			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
				Page.RegisterHiddenField("Containers","");

				if ((this.ExternalScript == null || this.ExternalScript == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
				{
#if (!FX1_1)
            Page.ClientScript.RegisterClientScriptInclude(SCRIPTKEY, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.ActiveToolbar.js"));
#else					
					if (CLIENTSIDE_API == null)
						CLIENTSIDE_API = Utils.GetResource("ActiveUp.WebControls._resources.ActiveToolbar.js");
					
					if (!CLIENTSIDE_API.StartsWith("<script"))
						CLIENTSIDE_API = "<script language=\"javascript\">\n" + CLIENTSIDE_API + "\n";
					
					CLIENTSIDE_API += "\n</script>\n";

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


		    Page.TestAndRegisterScriptBlock(SCRIPTKEY, ScriptDirectory, ScriptEntryPoint);

			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY + "_" + ClientID))
			{
				CLIENTSIDE_API = string.Empty;

				CLIENTSIDE_API += "<script language=\"javascript\">\n";
				CLIENTSIDE_API += string.Format("function Init_{0}(e)\n",ClientID);
				CLIENTSIDE_API += "{\n";

				CLIENTSIDE_API += string.Format("ATB_addContainer('{0}');\n",ClientID);

				foreach (Toolbar t in this._toolbars)
				{
					
					if (this.Parent != null && this.Parent.GetType().ToString() != "ActiveUp.WebControls.Editor")
						CLIENTSIDE_API += string.Format("ATB_setToolbarAbsolute('{0}');",t.UniqueID);
					CLIENTSIDE_API += string.Format("ATB_addToolToContainer('{0}','{1}');",UniqueID,t.UniqueID);
				}
				CLIENTSIDE_API += "\n}\n";
				CLIENTSIDE_API += string.Format("window.RegisterEvent(\"onload\", Init_{0});\n",ClientID);
				CLIENTSIDE_API += "\n</script>\n";

				Page.RegisterClientScriptBlock(SCRIPTKEY + "_" + ClientID, CLIENTSIDE_API);
			}

			
			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY + "_startup"))
			{
				string startupString = string.Empty;
				startupString += "<script>\n";
				startupString += "// Test if the client script is present.\n";
				startupString += "try\n{\n";
				startupString += "ATB_testIfScriptPresent();\n";
				startupString += "}\n catch (e)\n {\n alert('Could not find script file. Please ensure that the Javascript files are deployed in the " + ((ScriptDirectory == string.Empty) ? string.Empty : " [" + ScriptDirectory + "] directory or change the") + "ScriptDirectory and/or ExternalScript properties to point to the directory where the files are.'); \n}\n";
				startupString += "</script>\n";

				Page.RegisterClientScriptBlock(SCRIPTKEY + "_startup",startupString);
			}

		}

		/// <summary>
		/// Notifies the Popup control to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{ 
			if (IsInternalToolbarsInitialized == false)
			{
				foreach(Toolbar t in this.Toolbars) 
				{
					InternalToolBarsID += t.UniqueID;
					InternalToolBarsID += ",";
				}
				InternalToolBarsID = InternalToolBarsID.TrimEnd(',');
				IsInternalToolbarsInitialized = true;
			}

			this.RegisterAPIScriptBlock();
			if (Page != null)
				Page.RegisterRequiresPostBack(this);

			/*string scriptKey = this.ClientID + "_" + SCRIPTKEY + "_Init";
			System.Text.StringBuilder initValues = new System.Text.StringBuilder();
			initValues.Append("<script language='javascript'>\n");
			// Back Color
			initValues.Append("var ");
			initValues.Append(this.UniqueID);
			initValues.Append("_backColor = '");
			initValues.Append(Utils.Color2Hex(BackColor));
			initValues.Append("';\n");
			// Back Color Dock
			initValues.Append("var ");
			initValues.Append(this.UniqueID);
			initValues.Append("_backColorDock = '");
			initValues.Append(Utils.Color2Hex(BackColorDock));
			initValues.Append("';\n</script>");

			Page.RegisterStartupScript(scriptKey, initValues.ToString());*/

			base.OnPreRender(e);
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
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

			_useCounter++;

			if (!(this.Parent is ActiveUp.WebControls.ImageEditor) && /*!licenseStatus.IsRegistered &&*/ Page != null && _useCounter == StaticContainer.UsageCount)
			{
				_useCounter = 0;
				output.Write(StaticContainer.TrialMessage);
			}
			else
			{
				RenderToolbarsContainer(output);
			}

#else
			RenderToolbarsContainer(output);
#endif
		}

		/// <summary>
		/// Renders the toolbars container.
		/// </summary>
		/// <param name="output">The output.</param>
		protected void RenderToolbarsContainer(HtmlTextWriter output)
		{
			if (output == null)
			{
				throw new ArgumentNullException(nameof(output));
			}

			AddAttributesGroup(output, Hidden, string.Empty, $"{UniqueID}_toolbars", $"{UniqueID}_toolbars");

			if (BackColor != Color.Empty)
			{
				AddAttributesGroup(output, Hidden, Utils.Color2Hex(BackColor), $"{UniqueID}_backColor", $"{UniqueID}_backColor");
			}

			if(BackColorDock != Color.Empty)
			{
				AddAttributesGroup(output, Hidden, Utils.Color2Hex(BackColorDock), $"{UniqueID}_backColorDock", $"{UniqueID}_backColorDock");
			}

			output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px");
			output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid");
			output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "black");

			var top = string.Empty;
			var left = string.Empty;
			var enumerator = Style.Keys.GetEnumerator();
			while(enumerator.MoveNext())
			{
				if(enumerator.Current != null && string.Equals(enumerator.Current.ToString(), Left, StringComparison.OrdinalIgnoreCase))
				{
					left = Style[(string) enumerator.Current];
				}

				if(enumerator.Current != null && string.Equals(enumerator.Current.ToString(), Top, StringComparison.OrdinalIgnoreCase))
				{
					top = Style[(string) enumerator.Current];
				}
			}

			if(left != string.Empty && Width != Unit.Empty) { output.AddStyleAttribute(LeftSmall, left); }
			else { output.AddStyleAttribute(LeftSmall, ZeroPx); }

			output.AddStyleAttribute(TopSmall, top != string.Empty ? top : ZeroPx);

			output.AddStyleAttribute(HtmlTextWriterStyle.Width, Width != Unit.Empty ? Width.ToString() : HundredPercent);

			if(Height != Unit.Empty) { output.AddStyleAttribute(HtmlTextWriterStyle.Height, Height.ToString()); }

			output.AddStyleAttribute(PositionString, Position.ToString());
			output.AddAttribute(HtmlTextWriterAttribute.Id, UniqueID);
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
			output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, Utils.Color2Hex(BorderColor));
			if(BorderStyle != BorderStyle.NotSet) { output.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, BorderStyle.ToString()); }

			output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, BorderWidth != Unit.Empty ? BorderWidth.ToString() : ZeroPx);

			output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(BackColor));

			output.AddAttribute(OnSelectStart, ReturnFalse);
			if(BackImage != string.Empty) { output.AddStyleAttribute(BackgroundImage, $"url({ImagesDirectory}{BackImage})"); }

			RenderToolbarsPosition(output);
		}

		private void RenderToolbarsPosition(HtmlTextWriter output)
		{
			output.RenderBeginTag(HtmlTextWriterTag.Table);

			if(ToolbarsPosition == Position.Absolute)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.Write(NonbreakingSpace);
				output.RenderEndTag();
				output.RenderEndTag();
				output.RenderEndTag();
			}

			if(!string.IsNullOrEmpty(InternalToolBarsID))
			{
				var toolbarsId = InternalToolBarsID.Split(Comma);
				foreach(var toolbarId in toolbarsId)
				{
					if(!_toolbarsRenderedByOther.Contains(toolbarId))
					{
						var toolbarToRender = Page.FindControl(toolbarId);
						if(toolbarToRender != null)
						{
							if(ToolbarsPosition == Position.Relative)
							{
								output.RenderBeginTag(HtmlTextWriterTag.Tr);
								output.RenderBeginTag(HtmlTextWriterTag.Td);
								((Toolbar) toolbarToRender).Position = Position.Relative;
							}
							else { ((Toolbar) toolbarToRender).Position = Position.Absolute; }

							((Toolbar) toolbarToRender).EnableSsl = EnableSsl;
							((Toolbar) toolbarToRender).RenderControl(output);

							if(ToolbarsPosition == Position.Relative)
							{
								output.RenderEndTag();
								output.RenderEndTag();
							}
						}
					}
				}
			}

			foreach(Toolbar toolbar in _toolbars)
			{
				if(!InternalToolBarsID.Contains(toolbar.ID))
				{
					toolbar.EnableSsl = EnableSsl;

					if(ToolbarsPosition == Position.Relative)
					{
						output.RenderBeginTag(HtmlTextWriterTag.Tr);
						output.RenderBeginTag(HtmlTextWriterTag.Td);
						toolbar.Position = Position.Relative;
					}

					toolbar.RenderControl(output);
					if(ToolbarsPosition == Position.Relative)
					{
						output.RenderEndTag();
						output.RenderEndTag();
					}
				}
			}

			if(ToolbarsPosition == Position.Relative) { output.RenderEndTag(); }
		}

		/// <summary>
		/// Finds the tool by specifying the id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public ToolBase FindTool(string id)
		{
			foreach(Toolbar toolbar in this.Toolbars)
				foreach(ToolBase tool in toolbar.Tools)
					if (tool.ID == id)
						return tool;

			return null;
		}

		/// <summary>
		/// Finds the tool by specifying the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public ToolBase FindTool(System.Type type)
		{
			foreach(Toolbar toolbar in this.Toolbars)
				foreach(ToolBase tool in toolbar.Tools)
					if (tool.GetType() == type)
						return tool;

			return null;
		}
		#endregion

		#region Interface IPostBack

		/// <summary>
		/// Enables the control to process an event raised when a form is posted to the server.
		/// </summary>
		/// <param name="eventArgument">A String that represents an optional event argument to be passed to the event handler.</param>
		void IPostBackEventHandler.RaisePostBackEvent(String eventArgument)
		{
			Page.Trace.Write(this.ID, "RaisePostBackEvent...");
		}

		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{
			Page.Trace.Write(this.ID, "LoadPostData...");

			Toolbars.Clear();
			string[] toolbarsID = postCollection[UniqueID + "_toolbars"].Split(',');
			foreach(string toolbarID in toolbarsID)
				if (toolbarID != string.Empty)
				{
					//Toolbars.Add((Toolbar)Page.FindControl(toolbarID));

					Control tool = Page.FindControl(toolbarID);
					if (tool != null)
						Toolbars.Add((Toolbar)tool);
				}

			_toolbarsRenderedByOther = "";
			string containers = postCollection["Containers"];
			if (containers != null)
			{
				string[] containersID = containers.Split(',');
				foreach(string containerID in containersID)
				{
					if (containerID != null && containerID != string.Empty && containerID != ID)
					{
						string toolbars = postCollection[containerID + "_toolbars"];
						if (toolbars != null && toolbars != string.Empty)
						{
							toolbars = toolbars.TrimStart(',');
							toolbars = toolbars.TrimEnd(',');
						
							_toolbarsRenderedByOther += toolbars;
							_toolbarsRenderedByOther += ",";
						}
					}
				}
			}
			_toolbarsRenderedByOther = _toolbarsRenderedByOther.TrimEnd(',');
			
			return true;
		}

		/// <summary>
		/// Notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			Page.Trace.Write(this.ID, "RaisePostDataChangedEvent...");
		}

		private void AddAttributesGroup(HtmlTextWriter output, string type, string attributevalue, string id, string name)
		{
			output.AddAttribute(HtmlTextWriterAttribute.Type, type);
			output.AddAttribute(HtmlTextWriterAttribute.Value, attributevalue);
			output.AddAttribute(HtmlTextWriterAttribute.Id, id);
			output.AddAttribute(HtmlTextWriterAttribute.Name, name);
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();
		}

		#endregion
	}

	#endregion
}
