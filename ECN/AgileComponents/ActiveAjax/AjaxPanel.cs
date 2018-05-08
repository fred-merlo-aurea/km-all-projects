using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActiveUp.WebControls.Common;
using ActiveUp.WebControls.Common.Extension;

namespace ActiveUp.WebControls
{
    #region AjaxPanel

    /// <summary>
    /// Represents a <see cref="AjaxPanel"/> object.
    /// </summary>
    [
		ToolboxData("<{0}:AjaxPanel runat=server></{0}:AjaxPanel>"),
		DefaultEvent("CallBack"),
		ToolboxBitmap(typeof(AjaxPanel), "ToolBoxBitmap.Ajax.bmp"),
		//Designer(typeof(AjaxPanelDesigner)),
		Serializable
	]

    public class AjaxPanel : System.Web.UI.WebControls.Panel
	{
        static LicenseContext _lastDesignContext = null;
        static bool _licenseChecked = false;

		#region Fields

		/// <summary>
		/// Unique client script key.
		/// </summary>
		private const string SCRIPTKEY = "ActiveAjaxPanel";

#if (LICENSE)
		//private string _license = string.Empty;
		private int _useCounter = 0;
#endif


		#endregion

		#region Constants

		/// <summary>
		/// Indicates the status of the http request is ok.
		/// </summary>
		private const string _OK_STATUS_ = "OK";
			
		/// <summary>
		/// Indicates the status of the http request contains an error.
		/// </summary>
		private const string _ERROR_STATUS_ = "ERROR";

		/// <summary>
		/// Indicates if the postback becomes from an ajax panel.
		/// </summary>
		private const string _PARAMETER_IS_CALLBACK_ = "AA_IsCallBack";

		/// <summary>
		/// Client id of the callback.
		/// </summary>
		private const string _PARAMETER_CLIENT_ID = "AA_ClientId";

		private const string _PARAMETER_ARGUMENT_ = "AA_Argument";

		//private const string _LITERAL_CONTENTS_TO_IGNORE_ = "\r\n\t";

		#endregion

		#region Constructors

		/// <summary>
		/// Create the ajax panel.
		/// </summary>
		public AjaxPanel()
		{
            this.Width = Unit.Parse("160px");
            this.Height = Unit.Parse("96px");

            _licenseChecked = true;
		}

		#endregion

		#region Properties

		#region Script

		/// <summary>
		/// Gets or sets the relative or absolute path to the directory where the API javascript file is.
		/// </summary>
		[Bindable(false),
		Category("Script"),
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

#endif
*/
		#endregion

		#region Behavior

		/// <summary>
		/// Gets or sets the html display while the user waits for the response.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("The html display while the user waits for the response"),
		DefaultValue("")
		]
		public string LoadingDisplay
		{
			get
			{
				object loadingDisplay = ViewState["LoadingDisplay"];

				if (loadingDisplay != null)
					return (string)loadingDisplay;

				return string.Empty;
			}

			set
			{
				ViewState["LoadingDisplay"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the interval (millisecond) to automatically trigger callback.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("The interval (millisecond) to automatically trigger callback."),
		DefaultValue("0")
		]
		public int RefreshInterval
		{
			get
			{
				object refreshInteval = ViewState["RefreshInterval"];

				if (refreshInteval != null)
					return (int)refreshInteval;

				return 0;
			}

			set
			{
				ViewState["RefreshInterval"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the values indicates if you want to save the state of all the forms controls.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("The values indicates if you want to save the state of all the forms controls."),
		DefaultValue(true)
		]
		public bool SaveState
		{
			get
			{
				object saveState = ViewState["SaveState"];

				if (saveState != null)
					return (bool)saveState;

				return true;
			}

			set
			{
				ViewState["SaveState"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the values indicates if you want to use the cache.
		/// </summary>
		[
		Bindable(true),
		Category("Behavior"),
		Description("The values indicates if you want to use the cache."),
		DefaultValue(false)
		]
		public bool UseCache 
		{
			get
			{
				object useCache = ViewState["UseCache"];

				if (useCache != null)
					return (bool)useCache;

				return false;
			}

			set
			{
				ViewState["UseCache"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicates if you want to encodes an Uniform Resource Identifier (URI) component by replacing each instance of certain characters by one, two, or three escape sequences representing the UTF-8 encoding of the character. 
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("Encodes an Uniform Resource Identifier (URI) component by replacing each instance of certain characters by one, two, or three escape sequences representing the UTF-8 encoding of the character."),
		DefaultValue(true)
		]
		public bool EncodeURIComponent
		{
			get
			{
				object encodeURIComponent = ViewState["EncodeURIComponent"];

				if (encodeURIComponent != null)
					return (bool)encodeURIComponent;

				return true;
			}

			set
			{
				ViewState["EncodeURIComponent"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicates if you want to post only the panel elements or all the elements from the whole page.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("The value indicates if you want to post only the panel elements or all the elements from the whole page."),
		DefaultValue(true)
		]
		public bool PostPanelOnly  
		{
			get
			{
				object postPanelOnly = ViewState["PostPanelOnly"]; 

				if (postPanelOnly != null)
					return (bool)postPanelOnly;

				return true;
			}

			set
			{
				ViewState["PostPanelOnly"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicates the id of the others control you want to post the state.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("If you use the PostPanelOnly property you can with this property add the id of the others control you want to post the state."),
		DefaultValue(""),
		TypeConverter(typeof(StringArrayConverter))
		]
		public string[] PostId  
		{
			get
			{
				object postId = ViewState["PostId"]; 

				if (postId != null)
					return (string[])postId;

				return new string[] {};
			}

			set
			{
				ViewState["PostId"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicates if you want to set the width automatically on callback.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("The value indicates if you want to set the width automatically on callback."),
		DefaultValue(false)
		]
		public bool RequestAutoWidth
		{
			get
			{
				object requestAutoWidth = ViewState["RequestAutoWidth"]; 

				if (requestAutoWidth != null)
					return (bool)requestAutoWidth;

				return false;
			}

			set
			{
				ViewState["RequestAutoWidth"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicates if you want to set the height automatically on callback.
		/// </summary>
		[
		Bindable(false),
		Category("Behavior"),
		Description("The value indicates if you want to set the height automatically on callback."),
		DefaultValue(true)
		]
		public bool RequestAutoHeight
		{
			get
			{
				object requestAutoHeight = ViewState["RequestAutoHeight"]; 

				if (requestAutoHeight != null)
					return (bool)requestAutoHeight;

				return true;
			}

			set
			{
				ViewState["RequestAutoHeight"] = value;
			}
		}

		#endregion

		#region Client Event

		/// <summary>
		/// Gets or set the javascript function called after the unitialized state of the xml http request.
		/// </summary>
		[
		Bindable(false),
		Category("Client Event"),
		Description("Javascript function called after the unitialized state of the xml http request."),
		DefaultValue("")
		]
		public string OnUnitialized 
		{
			get
			{
				object onUnitialized = ViewState["OnUnitialized"];

				if (onUnitialized != null)
					return (string)onUnitialized;

				return string.Empty;
			}

			set
			{
				ViewState["OnUnitialized"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the javascript function called after the loading state of the xml http request.
		/// </summary>
		[
		Bindable(false),
		Category("Client Event"),
		Description("Javascript function called after the loading state of the xml http request."),
		DefaultValue("")
		]
		public string OnLoading 
		{
			get
			{
				object onLoading = ViewState["OnLoading"];

				if (onLoading != null)
					return (string)onLoading;

				return string.Empty;
			}

			set
			{
				ViewState["OnLoading"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the javascript function called after the loaded state of the xml http request.
		/// </summary>
		[
		Bindable(false),
		Category("Client Event"),
		Description("Javascript function called after the loaded state of the xml http request."),
		DefaultValue("")
		]
		public string OnLoaded 
		{
			get
			{
				object onLoaded = ViewState["OnLoaded"];

				if (onLoaded != null)
					return (string)onLoaded;

				return string.Empty;
			}

			set
			{
				ViewState["OnLoaded"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the javascript function called after the interactive state of the xml http request.
		/// </summary>
		[
		Bindable(false),
		Category("Client Event"),
		Description("Javascript function called after the interactive state of the xml http request."),
		DefaultValue("")
		]
		public string OnInteractive 
		{
			get
			{
				object onInteractive = ViewState["OnInteractive"];

				if (onInteractive != null)
					return (string)onInteractive;

				return string.Empty;
			}

			set
			{
				ViewState["OnInteractive"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the javascript function called after the abort state of the xml http request.
		/// </summary>
		[
		Bindable(false),
		Category("Client Event"),
		Description("Javascript function called after the abort state of the xml http request."),
		DefaultValue("")
		]
		public string OnAbort 
		{
			get
			{
				object onAbort = ViewState["OnAbort"];

				if (onAbort != null)
					return (string)onAbort;

				return string.Empty;
			}

			set
			{
				ViewState["OnAbort"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the javascript function called after the error state of the xml http request.
		/// </summary>
		[
		Bindable(false),
		Category("Client Event"),
		Description("Javascript function called after the error state of the xml http request."),
		DefaultValue("")
		]
		public string OnError 
		{
			get
			{
				object onError = ViewState["OnError"];

				if (onError != null)
					return (string)onError;

				return string.Empty;
			}

			set
			{
				ViewState["OnError"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the javascript function called after the complete state of the xml http request.
		/// </summary>
		[
		Bindable(false),
		Category("Client Event"),
		Description("Javascript function called after the complete state of the xml http request."),
		DefaultValue("")
		]
		public string OnComplete 
		{
			get
			{
				object onComplete = ViewState["OnComplete"];

				if (onComplete != null)
					return (string)onComplete;

				return string.Empty;
			}

			set
			{
				ViewState["OnComplete"] = value;
			}
		}		

		#endregion

		#region Internal

		/// <summary>
		/// Indicates if a callback occurs from the panel.
		/// </summary>
		private bool IsCallback
		{
			get
			{
				bool isCallBack;
				try
				{
					isCallBack = bool.Parse(HttpRequest.Params[_PARAMETER_IS_CALLBACK_]) && HttpRequest.Params[_PARAMETER_CLIENT_ID] == this.ClientID;
				}
				catch
				{
					isCallBack = false;
				}
				
				return isCallBack;
			}
		}

		private string Argument 
		{
			get
			{ 
				string argument = string.Empty;
				try
				{
					argument = HttpRequest.Params[_PARAMETER_ARGUMENT_];
				}
				catch
				{
					
				}
				
				return argument;
			}
			
		}

		/// <summary>
		/// HTTP response information from the callback.
		/// </summary>
		private HttpResponse HttpResponse 
		{
			get
			{
				return System.Web.HttpContext.Current.Response;
			}
		}

		/// <summary>
		/// HTTP request information sent by the client.
		/// </summary>
		private HttpRequest HttpRequest 
		{
			get
			{
				return System.Web.HttpContext.Current.Request;				
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
            Page.ClientScript.RegisterClientScriptInclude(SCRIPTKEY, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.ActiveAjax.js"));
#else
					string CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls._resources.ActiveAjax.js");
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
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActiveAjax.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
				
      }

		    Page.TestAndRegisterScriptBlock(SCRIPTKEY, ScriptDirectory, "AA_TestIfScriptPresent()");


		    if (!Page.IsClientScriptBlockRegistered(string.Format("{0}_Var_{1}", SCRIPTKEY, ClientID)))
            {
				string varString = string.Empty;
				varString += "<script>\n";
				varString += "\n// Variable declaration related to the ajax panel '" + ClientID + "'\n";
				varString += string.Format("var ActiveAjax_{0} = new AA_CallBackObject('{0}');\n",this.ClientID);	

#if (DEBUG)
				varString += string.Format("ActiveAjax_{0}.Debug = true;\n",ClientID);
#endif
				if (EncodeURIComponent)
					varString += string.Format("ActiveAjax_{0}.EncodeURI = {1};\n",ClientID,EncodeURIComponent ? "true" : "false");

				if (SaveState)
					varString += string.Format("ActiveAjax_{0}.SaveState = {1};\n",ClientID,SaveState ? "true" : "false");

				if (UseCache)
					varString += string.Format("ActiveAjax_{0}.UseCache = {1};\n",ClientID,UseCache ? "true" : "false");

				if (OnAbort != string.Empty)
					varString += string.Format("ActiveAjax_{0}.OnAbort = {1};\n",ClientID,OnAbort);

				if (OnComplete != string.Empty)
					varString += string.Format("ActiveAjax_{0}.OnComplete = {1};\n",ClientID,OnComplete);

				if (OnError != string.Empty)
					varString += string.Format("ActiveAjax_{0}.OnError = {1};\n",ClientID,OnError);

				if (OnInteractive != string.Empty)
					varString += string.Format("ActiveAjax_{0}.OnInteractive = {1};\n",ClientID,OnInteractive);
	
				if (OnLoaded != string.Empty)
					varString += string.Format("ActiveAjax_{0}.OnLoaded = {1};\n",ClientID,OnLoaded);

				if (OnLoading != string.Empty)
					varString += string.Format("ActiveAjax_{0}.OnLoading = {1};\n",ClientID,OnLoading);

				if (OnUnitialized != string.Empty)
					varString += string.Format("ActiveAjax_{0}.OnUnitialized = {1};\n",ClientID,OnUnitialized);

				if (LoadingDisplay != string.Empty)
					varString += string.Format("ActiveAjax_{0}.LoadingDisplay = '{1}';\n",ClientID,LoadingDisplay);

				if (RequestAutoWidth)
					varString += string.Format("ActiveAjax_{0}.RequestAutoWidth = {1};\n",ClientID,RequestAutoWidth ? "true" : "false");

				if (RequestAutoHeight)
					varString += string.Format("ActiveAjax_{0}.RequestAutoHeight = {1};\n",ClientID,RequestAutoHeight ? "true" : "false");

				if (RefreshInterval > 0)
				{
					varString += string.Format("window.setInterval(\"ActiveAjax_{0}.DoActiveCallBack(\'{0}\',\'\');\",{1});\n",ClientID,RefreshInterval);
				}

				if (PostPanelOnly) 
				{
					string controlsID = string.Empty;

					foreach(Control c in this.Controls) 
					{
						//if (!(c is System.Web.UI.LiteralControl && ((System.Web.UI.LiteralControl)c).Text.StartsWith(_LITERAL_CONTENTS_TO_IGNORE_)))
						//{
						controlsID += c.ClientID;
						controlsID += ',';
						//}
					}
					controlsID = controlsID.TrimEnd(',');

					varString += string.Format("ActiveAjax_{0}.PostPanelOnly = '{1}';\n",ClientID,controlsID);

					if (PostId.Length > 0)
						varString += string.Format("ActiveAjax_{0}.PostId = '{1}';\n",ClientID,Utils.FormatStringArray(PostId,','));
				}

				varString += "\n</script>\n";

				Page.RegisterClientScriptBlock(SCRIPTKEY + "_Var_" + ClientID,varString);
				
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

			base.OnPreRender(e);
		}

		/// <summary> 
		/// Render the panel to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out.</param>
		protected override void Render(HtmlTextWriter output)
		{

#if (LICENSE)

			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.AAX, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

			_useCounter++;

			if (/*!licenseStatus.IsRegistered &&*/ Page != null && _useCounter == StaticContainer.UsageCount)
			{
				_useCounter = 0;
				output.Write(StaticContainer.TrialMessage);
			}
			else 
				RenderAjaxPanel(output);


#else
		RenderAjaxPanel(output);

#endif
		}

		/// <summary> 
		/// Render the panel to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out.</param>
		internal void RenderAjaxPanel(HtmlTextWriter output) 
		{
			output.AddAttribute(HtmlTextWriterAttribute.Type,"Hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Name + "_Ticks",UniqueID);
			output.AddAttribute(HtmlTextWriterAttribute.Value,DateTime.Now.Ticks.ToString());
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			if (IsCallback) 
			{
				StringWriter stringWriter = new StringWriter();
				output = new HtmlTextWriter(stringWriter);
				
				try 
				{
					this.OnCallBack(new AjaxPanelEventArgs(Argument,output));

					if (stringWriter.ToString() == string.Empty) 
					{
						if (this.Visible) 
						{
							foreach(Control c in this.Controls) 
							{
								//if (!(c is System.Web.UI.LiteralControl && ((System.Web.UI.LiteralControl)c).Text.StartsWith(_LITERAL_CONTENTS_TO_IGNORE_)))
								c.RenderControl(output);
							}
						}
					}
					
					HttpResponse.Clear();
					HttpResponse.StatusCode = 200;
					HttpResponse.StatusDescription = _OK_STATUS_;
					HttpResponse.Write(stringWriter);
					HttpResponse.Flush();
					HttpResponse.End();
				}

				catch (Exception ex)
				{
					HttpResponse.Clear();
					HttpResponse.StatusCode = 200;
					HttpResponse.StatusDescription = _ERROR_STATUS_;
					HttpResponse.Write(ex.Message);
					HttpResponse.Flush();
					HttpResponse.End();
				}
			}

			else 
			{
				base.Render(output);
			}
		}


		#endregion

		#endregion

		#region Events

		/// <summary>
		/// Event handler fires when a callback occurs on the panel.
		/// </summary>
		public delegate void AjaxPanelEventHandler(object sender, AjaxPanelEventArgs e);

		/// <summary>
		/// Server event occurs when the callback on the panel occurs.
		/// </summary>
		public event AjaxPanelEventHandler CallBack;	
			
		/// <summary>
		/// Server event occurs when a call back on the panel occurs.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnCallBack(AjaxPanelEventArgs e) 
		{
			if (CallBack != null)
				CallBack(this,e);
		}

		#endregion
	}

	#endregion
} 
