using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Drawing;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	[
	ParseChildren(true, "TabPages"),
	Serializable 
	]
	public class TabControl : System.Web.UI.WebControls.WebControl, IPostBackDataHandler, IPostBackEventHandler
	{
		private TabPageCollection _tabPages = null;
		private TabOrientation _orientation = TabOrientation.Horizontal;
		private static string CLIENTSIDE_API;
		private static string SCRIPTKEY = "ACTIVEINPUT";
		private bool _clientSideEnabled = true;
		private ImageSet _left, _right, _center;

		public TabControl()
		{
			_left = new ImageSet();
			_right = new ImageSet();
			_center = new ImageSet();

			this.Left.Off = "images/tabButton_Off_Left.gif";
			this.Left.On = "images/tabButton_On_Left.gif";
			this.Left.Over = "images/tabButton_Over_Left.gif";

			this.Right.Off = "images/tabButton_Off_Right.gif";
			this.Right.On = "images/tabButton_On_Right.gif";
			this.Right.Over = "images/tabButton_Over_Right.gif";

			this.Center.Off = "images/tabButton_Off_Center.gif";
			this.Center.On = "images/tabButton_On_Center.gif";
			this.Center.Over = "images/tabButton_Over_Center.gif";
		}


		public ImageSet Left
		{
			get
			{
				return _left;
			}
			set
			{
				_left = value;
			}
		}

		public ImageSet Right
		{
			get
			{
				return _right;
			}
			set
			{
				_right = value;
			}
		}
		
		public ImageSet Center
		{
			get
			{
				return _center;
			}
			set
			{
				_center = value;
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
		public TabPageCollection TabPages 
		{
			get 
			{
				if (_tabPages == null) 
				{
					_tabPages = new TabPageCollection();
					if (IsTrackingViewState) 
					{
						((IStateManager)_tabPages).TrackViewState();
					}
				}
				return _tabPages;
			}
		}

		#region .NET API
		/// <summary>
		/// Get the specified resource from the assembly.
		/// </summary>
		/// <param name="resource">The name of the resource.</param>
		/// <returns>The string representation of the resource.</returns>
		public static string GetResource(string resource)
		{
			return GetResource(resource, null);
		}

		/// <summary>
		/// Get the specified resource from the assembly.
		/// </summary>
		/// <param name="resource">The name of the resource.</param>
		/// <param name="type">The type of the assembly.</param>
		/// <returns>The string representation of the resource.</returns>
		public static string GetResource(string resource, System.Type type)
		{

			string str = null;
			Assembly asm;
			
			if (type != null)
				asm = Assembly.GetAssembly(type);
			else
				asm = Assembly.GetExecutingAssembly();
			// We check for null just in case the variable is called at design-time.
			if (asm != null)
			{
				// Just for clarity define multiple variables.
				Stream stm = asm.GetManifestResourceStream(resource);
				StreamReader reader = new StreamReader(stm);
				str = reader.ReadToEnd();
				reader.Close();
				stm.Close();
			}

			return str;
		}

		public virtual void RegisterAPIScriptBlock(System.Web.UI.Page page) 
		{
			// Register the script block is not allready done.
			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
				if ((this.ExternalScript == null || this.ExternalScript.TrimEnd() == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
				{
					if (CLIENTSIDE_API == null)
						CLIENTSIDE_API = GetResource("ActiveUp.WebControls._resources.TabControl.js");
					
					if (!CLIENTSIDE_API.StartsWith("<script"))
						CLIENTSIDE_API = "<script language=\"javascript\">\n" + CLIENTSIDE_API;

					CLIENTSIDE_API += "\n</script>\n";

					Page.RegisterClientScriptBlock(SCRIPTKEY, CLIENTSIDE_API);
				}
				else
				{
					if (this.ScriptDirectory.StartsWith("~"))
						this.ScriptDirectory = this.ScriptDirectory.Replace("~", System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActiveInput.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
			}

			System.Text.StringBuilder images = new System.Text.StringBuilder();
			images.Append("'");
			images.Append(this.Left.Off);
			images.Append("','");
			images.Append(this.Left.On);
			images.Append("','");
			images.Append(this.Left.Over);
			images.Append("','");
			images.Append(this.Center.Off);
			images.Append("','");
			images.Append(this.Center.On);
			images.Append("','");
			images.Append(this.Center.Over);
			images.Append("','");
			images.Append(this.Right.Off);
			images.Append("','");
			images.Append(this.Right.On);
			images.Append("','");
			images.Append(this.Right.Over);
			images.Append("'");

			Page.RegisterArrayDeclaration(this.ClientID + "_Images", images.ToString());
		}

		/// <summary>
		/// Gets or sets the relative or absolute path to the directory where input API javascript file is.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the input render.</remarks>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Gets or sets the relative or absolute path to the directory where input API javascript file is."),
		DefaultValue("/aspnet_client/ActiveWebControls/3_3_1973_0/")
		]
		public string ScriptDirectory
		{
			get
			{
				object local = ViewState["ScriptDirectory"];
				if (local != null)
					return (string)local;
				else
					return "/js/";
			}

			set
			{
				ViewState["ScriptDirectory"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the relative or absolute path to the icons directory.
		/// </summary>
		[Bindable(false),
		Category("Behavior"),
		Description("Gets or sets the relative or absolute path to the icons directory.")]
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

		/// <summary>
		/// Determine if we need to register the client side script and render the control.
		/// </summary>
		/// <returns>0 if scripting not allowed, 1 if not an uplevel browser but scripting allowed, 2 if all is OK.</returns>
		private bool IsUpLevel() 
		{
			Page page = Page;

			if (this.RenderType == RenderType.NotSet)
			{
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
			else if (this.RenderType == RenderType.UpLevel)
				return true;

			return false;
		}

		/// <summary>
		/// Gets or sets the value indicating if the client side script is disabled or not.
		/// </summary>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue(true),
		Browsable(true),
		Description("The value indicating if the client side script is disabled or not.")
		] 
		public RenderType RenderType
		{
			get
			{
				object local = ViewState["RenderType"];
				if (local != null)
					return (RenderType)local;
				else
					return RenderType.NotSet;
			}

			set
			{
				ViewState["RenderType"] = value;
			}
		}

		/// <summary>
		/// Do some work before rendering the control.
		/// </summary>
		/// <param name="e">Event Args</param>
		protected override void OnPreRender(EventArgs e) 
		{
			base.OnPreRender(e);
			
			_clientSideEnabled = IsUpLevel();

			//if (_clientSideEnabled)
			//{
				if (((base.Page != null) && base.Enabled))
				{
					RegisterAPIScriptBlock(this.Page);
				}
			//}
		}

		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{

			this.ControlStyle.AddAttributesToRender(writer, this);

			
			System.Web.UI.AttributeCollection collection1 = this.Attributes;
			IEnumerator enumerator1 = collection1.Keys.GetEnumerator();
			while (enumerator1.MoveNext())
			{
				string text2 = (string) enumerator1.Current;
				writer.AddAttribute(text2, collection1[text2]);
			}
		}


		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			HtmlTextWriter writer = GetCorrectTagWriter(output);
			int index;
			
			// Render buttons

			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			
			for(index=0;index<this.TabPages.Count;index++)
			{
				RenderTabButton(writer, index);
			}

			writer.RenderEndTag();
			writer.RenderEndTag();

			// Render Pages
			//this.AddAttributesToRender(writer);
			this.ControlStyle.AddAttributesToRender(writer);
			writer.AddStyleAttribute("overflow", "hidden");
			writer.AddStyleAttribute("position", "relative");
			writer.AddAttribute("id", this.ClientID + "_MainPanel");
			writer.RenderBeginTag(HtmlTextWriterTag.Div); // Main Div Start
            
			for(index=0;index<this.TabPages.Count;index++)
			{
				RenderTabPages(writer, index);
			}

			writer.RenderEndTag(); // Main Div End

			output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
			output.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
			output.AddAttribute(HtmlTextWriterAttribute.Value, this.SelectedIndex.ToString());
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();
		}

		protected void RenderTabButton(HtmlTextWriter writer, int index)
		{
			bool isSelected = false;

			if (this.SelectedIndex == index || (this.SelectedIndex == -1 && index == 0))
				isSelected = true;

			if (_clientSideEnabled)
			{
				writer.AddAttribute("onclick", string.Format("NAV_selectTabPage('{0}', '{1}');", this.ClientID, index.ToString()));
			}
			else
			{
				writer.AddAttribute("onclick", string.Format(Page.GetPostBackClientEvent(this, index.ToString())));
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_TabButton" + index.ToString());
			writer.AddAttribute("onmouseover", string.Format("NAV_buttonOver('{0}', '{1}');", this.ClientID, index.ToString()));
			writer.AddAttribute("onmouseout", string.Format("NAV_buttonOut('{0}', '{1}');", this.ClientID, index.ToString()));
			writer.AddStyleAttribute("cursor", "hand");
			writer.RenderBeginTag(HtmlTextWriterTag.Td); // Main TD
			
			writer.AddAttribute("class", "tabButtonOff");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.RenderBeginTag(HtmlTextWriterTag.Table); // Table
			writer.RenderBeginTag(HtmlTextWriterTag.Tr); // Tr
			writer.RenderBeginTag(HtmlTextWriterTag.Td); // Left TD
			writer.Write(string.Format("<img src=\"{0}\" border=\"0\" id=\"{1}\">", (isSelected ? this.Left.On : this.Left.Off), this.ClientID + "_TabButton_Left" + index.ToString()));
			writer.RenderEndTag(); // Left TD End

			if (isSelected)
				writer.AddAttribute(HtmlTextWriterAttribute.Background, this.Center.On);
			else
				writer.AddAttribute(HtmlTextWriterAttribute.Background, this.Center.Off);
			writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_TabButton_Center" + index.ToString());
			writer.RenderBeginTag(HtmlTextWriterTag.Td); // Center TD
			writer.Write(TabPages[index].Text);
			writer.RenderEndTag(); // Center TD End

			writer.RenderBeginTag(HtmlTextWriterTag.Td); // Left TD
			writer.Write(string.Format("<img src=\"{0}\" border=\"0\" id=\"{1}\">", (isSelected ? this.Right.On : this.Right.Off), this.ClientID + "_TabButton_Right" + index.ToString()));
			writer.RenderEndTag(); // Left TD End

			writer.RenderEndTag(); // Tr End
			writer.RenderEndTag(); // Table End
			
			writer.RenderEndTag(); // Main TD End
		}
		
		protected void RenderTabPages(HtmlTextWriter writer, int index)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_TabPage" + index.ToString());
			
			Page.Trace.Warn(SelectedIndex.ToString());

			if (!TabPages[index].Selected && !(SelectedIndex == -1 && index == 0))
			{
				writer.AddStyleAttribute("visibility", "hidden");
				writer.AddStyleAttribute("display", "none");
			}
			else
				writer.AddStyleAttribute("z-index", "1");
			writer.AddStyleAttribute("position", "absolute");
			writer.AddStyleAttribute("width", "100%");
			writer.AddStyleAttribute("height", "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.Write(TabPages[index].Value);
			writer.RenderEndTag();
		}

		#endregion

		public int SelectedIndex
		{
			get
			{
				int index = 0;
				for(index=0;index<TabPages.Count;index++)
				{
					if (TabPages[index].Selected)
						return index;
				}

				return -1;
			}
			set
			{
				foreach(TabPage tabPage in this.TabPages)
					tabPage.Selected = false;

				this.TabPages[value].Selected = true;
			}
		}

		#region IPostBackDataHandler

		/// <summary>
		/// Processes post-back data from the control.
		/// </summary>
		/// <param name="postDataKey">The key identifier for the control.</param>
		/// <param name="postCollection">The collection of all incoming name values.</param>
		/// <returns>True if the state changes as a result of the post-back, otherwise it returns false.</returns>
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{
			Page.Trace.Write(this.ID, "LoadPostData...");

			string _orderValue = postCollection[UniqueID];

			Page.Trace.Write(_orderValue);

			if (_orderValue != this.SelectedIndex.ToString() && this.TabPages.Count > 0)
			{	
				Page.Trace.Write("Returning True");
				
				this.SelectedIndex = Convert.ToInt32(_orderValue);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			Page.Trace.Write(this.ID, "RaisePostDataChangedEvent...");
			OnTabIndexChanged(EventArgs.Empty);
		}

		#endregion

		#region Events

		/// <summary>
		/// Raise the <see cref="SelectedIndexChanged"/> of the <see cref="ToolDropDownList"/> control. This allows you to handle the event directly.
		/// </summary>
		/// <param name="e">Event data.</param>
		protected virtual void OnTabIndexChanged(EventArgs e) 
		{
			// Check if someone use our event.
			if (TabIndexChanged != null)
				TabIndexChanged(this,e);
		}

		public event EventHandler TabIndexChanged;

		#endregion

		#region IPostBackEventHandler

		/// <summary>
		/// Enables the control to process an event raised when a form is posted to the server.
		/// </summary>
		/// <param name="eventArgument">A String that represents an optional event argument to be passed to the event handler.</param>
		void IPostBackEventHandler.RaisePostBackEvent(String eventArgument)
		{
			Page.Trace.Write(this.ID, "RaisePostBackEvent..." + eventArgument);
		
			this.SelectedIndex = Convert.ToInt32(eventArgument);

			OnTabIndexChanged(EventArgs.Empty);
		}

		#endregion

		private HtmlTextWriter tagWriter;
		private HtmlTextWriter GetCorrectTagWriter( HtmlTextWriter writer ) 
		{
			if ( this.tagWriter != null ) return this.tagWriter;

			if ( writer is System.Web.UI.Html32TextWriter ) 
			{
				this.tagWriter =  new HtmlTextWriter( writer.InnerWriter );
			} 
			else 
			{
				this.tagWriter = writer;
			}
			return this.tagWriter;
		}
	}
}
