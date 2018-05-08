using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.ComponentModel;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{ 
    /// <summary>
	/// Represents a <see cref="Loader"/> object.
	/// </summary>
	[
        DefaultProperty("Text"), 
	    ToolboxData("<{0}:Loader runat=server></{0}:Loader>"),
	    ParseChildren(false),
        ToolboxBitmap(typeof(Loader), "ToolBoxBitmap.Loader.bmp")
    ]
	[Designer(typeof(LoaderControlDesigner))]
	public class Loader : System.Web.UI.WebControls.WebControl
	{
		// Consts
		//private static string CLIENTSIDE_API;
		//private const string SCRIPTKEY = "ALD_Loader";

		private string  _text;//, _externalScript, _scriptDirectory;
/*#if (LICENSE)
		private string _license = string.Empty;
#endif */

		/// <summary>
		/// Initializes a new instance of the <see cref="Loader"/> class.
		/// </summary>
		public Loader()
		{
			//
			// TODO: Add constructor logic here
			//
		}

/*#if (LICENSE)
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
		/// Gets or sets a value indicating whether the trigger must be in automatic mode.
		/// </summary>
		/// <value><c>true</c> if the trigger must be in automatic mode; otherwise, <c>false</c>.</value>
		public bool AutoTrigger
		{
			get
			{
				object obj = ViewState["_autoTrigger"];
				if (obj != null)
					return Convert.ToBoolean(ViewState["_autoTrigger"]);
				else
					return true;
			}
			set
			{
				ViewState["_autoTrigger"] = value;
			}
		}

		/*public PagePosition Position
		{
			get
			{
				object obj = ViewState["_position"];
				if (obj != null)
					return (PagePosition)ViewState["_position"];
				else
					return PagePosition.Absolute;
			}
			set
			{
				ViewState["_position"] = value;
			}
		}*/

		/// <summary>
		/// Gets or sets the time delay.
		/// </summary>
		/// <value>The time delay.</value>
		public int TimeDelay
		{
			get
			{
				object obj = ViewState["_timeDelay"];
				if (obj != null)
					return Convert.ToInt32(ViewState["_timeDelay"]);
				else
					return 0;
			}
			set
			{
				ViewState["_timeDelay"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the loader must be rendered on post back.
		/// </summary>
		/// <value><c>true</c> if the loader must be rendered on post back; otherwise, <c>false</c>.</value>
		public bool RenderOnPostBack
		{
			get
			{
				object obj = ViewState["_renderOnPostBack"];
				if (obj != null)
					return Convert.ToBoolean(ViewState["_renderOnPostBack"]);
				else
					return false;
			}
			set
			{
				ViewState["_renderOnPostBack"] = value;
			}
		}

		/*/// <summary>
		/// Gets or sets the relative or absolute path to the external Html TextBox API javascript file.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the Html TextBox render.</remarks>
		[Bindable(false),
		Category("Behavior"),
		Description("Gets or sets the relative or absolute path to the external Html TextBox API javascript file.")	]
		public string ExternalScript
		{
			get
			{
				return _externalScript;
			}
			set
			{
				_externalScript = value;
			}
		}

		/// <summary>
		/// Gets or sets the relative or absolute path to the directory where the API javascript file is.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the control render.</remarks>
		[Bindable(false),
		Category("Behavior"),
		Description("Gets or sets the relative or absolute path to the directory where the API javascript file is.")	]
		public string ScriptDirectory
		{
			get
			{
				return _scriptDirectory;
			}
			set
			{
				_scriptDirectory = value;
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			//RegisterScriptBlock();
		}*/

		/*/// <summary>
		/// Register the Client-Side script block in the ASPX page.
		/// </summary>
		public void RegisterScriptBlock() 
		{
			// Register the script block is not allready done.
			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
				if ((this.ExternalScript == null || this.ExternalScript == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory == string.Empty))
				{
					if (CLIENTSIDE_API == null)
						CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls._resources.ActiveLoader.js");
					
					if (!CLIENTSIDE_API.StartsWith("<script"))
						CLIENTSIDE_API = "<script language=\"javascript\">\n" + CLIENTSIDE_API;

					CLIENTSIDE_API += "\n</script>\n";

					Page.RegisterClientScriptBlock(SCRIPTKEY, CLIENTSIDE_API);
				}
				else
				{
					if (this.ScriptDirectory.StartsWith("~"))
						this.ScriptDirectory = this.ScriptDirectory.Replace("~", System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActiveLoader.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
			}
		}*/

		/// <summary>
		/// Gets or sets the value containing the preload HTML.
		/// </summary>
		[Bindable(false),
		Category("Behavior"),
		Description("Gets or sets the value containing the preload HTML.")	]
		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
			}
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			//string licenseMessage = this.GetLicenseMessage();
			
#if (LICENSE)		
			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.ALD, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

			int secs = DateTime.Now.Second;
			if (/*!licenseStatus.IsRegistered &&*/ Page != null
				&& !Page.IsStartupScriptRegistered("Register_Popup") && secs < 60 && secs > 55)
			{
				output.Write("<table bgcolor=red><tr><td><font face=verdana color=white><b>Reload the page to continue your evaluation.</b></font></td></tr></table>");
			}

#else
		RenderLoader(output);
#endif
		}

		/// <summary>
		/// Renders the loader web control.
		/// </summary>
		/// <param name="output">The output writer.</param>
		protected void RenderLoader(HtmlTextWriter output)
		{
			if (RenderOnPostBack == false || (RenderOnPostBack && !this.Page.IsPostBack))
			{
				/*foreach(Control control in Page.Controls)
					if (control.GetType().ToString() == "ActiveUp.WebControls.Loader")
						throw new Exception("Only one Loader control is allowed on the page.");*/

				HtmlTextWriter writer = GetCorrectTagWriter(output);

				writer.Write("<!--- Start --->");

				//writer.Write("<DIV id=\"ActiveLoader_Layer\" STYLE=\"position:absolute;top:0;left:0;width:100%;height:100%;z-index:-10000;visibility:hidden;display:none;\">");
				//this.ControlStyle.AddAttributesToRender(output);
			
				writer.AddStyleAttribute("position", "absolute");
				/*
				if (Position == PagePosition.Absolute)
				{
					IEnumerator enumerator = this.Style.Keys.GetEnumerator();
					while(enumerator.MoveNext())
						writer.AddStyleAttribute((string)enumerator.Current, this.Style[(string)enumerator.Current]);
					enumerator.Reset();
					while(enumerator.MoveNext())
						writer.AddStyleAttribute((string)enumerator.Current, this.Style[(string)enumerator.Current]);
					this.ControlStyle.AddAttributesToRender(writer);
				}*/
				
				writer.AddStyleAttribute("z-index", "-100000");
				writer.AddStyleAttribute("visibility", "hidden");
				writer.AddStyleAttribute("display", "none");
				writer.AddAttribute("id", "ActiveLoader_Layer");

				

				writer.RenderBeginTag(HtmlTextWriterTag.Div);

				/*if (Position != PagePosition.Absolute)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
					writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
					writer.RenderBeginTag(HtmlTextWriterTag.Table);
					writer.RenderBeginTag(HtmlTextWriterTag.Tr);
					switch (Position)
					{
						case PagePosition.TopLeft:
							writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
							writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
							break;
						case PagePosition.TopCenter:
							writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
							writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
							break;
						case PagePosition.TopRight:
							writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
							writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
							break;
						case PagePosition.MiddleLeft:
							writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
							writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
							break;
						case PagePosition.MiddleCenter:
							writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
							writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
							break;
						case PagePosition.MiddleRight:
							writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
							writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
							break;
						case PagePosition.BottomLeft:
							writer.AddAttribute(HtmlTextWriterAttribute.Valign, "bottom");
							writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
							break;
						case PagePosition.BottomCenter:
							writer.AddAttribute(HtmlTextWriterAttribute.Valign, "bottom");
							writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
							break;
						case PagePosition.BottomRight:
							writer.AddAttribute(HtmlTextWriterAttribute.Valign, "bottom");
							writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
							break;
					}
					//writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
					//writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
				}*/
				this.RenderChildren(writer);
				writer.Write(this.Text);
				/*if (Position != PagePosition.Absolute)
				{
					writer.RenderEndTag();
					writer.RenderEndTag();
					writer.RenderEndTag();
				}*/
				//output.Write("</DIV>");			
				writer.RenderEndTag();
				writer.Write("<!--- End --->");		
	
				string code = @"<script language='javascript'>
var ALD_isPageLoaded = false;

function ALD_GetLoader()
{
	return document.getElementById(""ActiveLoader_Layer"");
}

function ALD_PageLoaded()
{
	ALD_isPageLoaded = true;
	ALD_HideLoader();
}

function ALD_HideLoader()
{
	ALD_GetLoader().style.visibility = 'hidden';
	ALD_GetLoader().style.display = 'none';
	ALD_GetLoader().style.zIndex = -10000;
}

function ALD_ShowLoader(pageloaded)
{
	if (!pageloaded || (pageloaded && !ALD_isPageLoaded))
	{
		ALD_GetLoader().style.visibility = 'visible';
		ALD_GetLoader().style.display = 'block';
		ALD_GetLoader().style.zIndex = 150;
	}
}";

				output.Write(code);
				
				output.Write("\n\n");

				if (this.AutoTrigger)
				{
					if (this.TimeDelay > 0)
						output.Write(string.Format("setTimeout('ALD_ShowLoader(true)', {0});\n", this.TimeDelay.ToString()));
					else
						output.Write("ALD_ShowLoader();\n");
				}

				output.Write("ALD_GetLoader().style.top = 0;\n");
				output.Write("ALD_GetLoader().style.left = 0;\n");
				output.Write("ALD_GetLoader().style.width = document.body.clientWidth;\n");
				output.Write("ALD_GetLoader().style.height = document.body.clientHeight;\n\n");

				string code2 = @"
if (window.addEventListener){ 
	window.addEventListener(""load"", ALD_PageLoaded, false) 
} else if (window.attachevent) { 
	window.attachevent(""onload"", ALD_PageLoaded) 
} else { 
	window.onload=ALD_PageLoaded
}";

				if (this.AutoTrigger)
					output.Write(code2);

				output.Write("</script>\n");
				
			}
		}

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

		/*private string GetLicenseMessage()
		{
			Exception lastException = new Exception();
			LicenseStatus status, status2;
			
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License("ActiveCalendarV2");

			status = license.CheckLicense(ProductCode.ALD,2,this.License);
			status2 = license.CheckLicense(ProductCode.AWC,3,this.License);
				
			if (status.IsRegistered || status2.IsRegistered)
				return status.ErrorMessage;
			else if (status.ErrorType == LicenseError.Invalid || status.ErrorType == LicenseError.TrialExpired)
				return status.ErrorMessage;
			
			return status2.ErrorMessage;
		}

		private LicenseType GetLicenseType()
		{
			Exception lastException = new Exception();
			LicenseStatus status, status2;
			
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License("ActiveProtectorV1");

			status = license.CheckLicense(ProductCode.ALD,1,this.License);
			status2 = license.CheckLicense(ProductCode.AWC,3,this.License);
				
			if (status.IsRegistered || status2.IsRegistered)
				return LicenseType.Registered;
			else if ((status.ErrorType == LicenseError.Invalid || status.ErrorType == LicenseError.FileNotFound)
				&& (status2.ErrorType == LicenseError.Invalid || status2.ErrorType == LicenseError.FileNotFound))
				return LicenseType.Error;
			else if (status.ErrorType == LicenseError.TrialExpired || status.ErrorType == LicenseError.TrialExpired)
				return LicenseType.TrialExpired;
			else
				return LicenseType.TrialValid;
		}*/
	}
}
