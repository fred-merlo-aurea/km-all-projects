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
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ExtendedWebControl"/> object.
	/// </summary>
	[ValidationPropertyAttribute("Text")] 	
	public class ExtendedWebControl : CoreWebControl
	{
		private const string ExternalScriptDefaultFx1 = "base.js";
		private bool _clientSideEnabled = true;
		private static string CLIENTSIDE_API;
	
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedWebControl"/> class.
		/// </summary>
		public ExtendedWebControl()
		{
			ExternalScript = string.Empty;
		}

#if (LICENSE)		

		/// <summary>
		/// Used for the license counter.
		/// </summary>
		internal static int _useCounter;

/*		/// <summary>
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
				object local = ViewState["License"];
				if (local != null)
					return (string)local;
				else
					return string.Empty;
			}
		
			set
			{
				ViewState["License"] = value;
			}
		}*/
#endif

		/// <summary>
		/// Gets or sets a value indicating whether client side is enabled.
		/// </summary>
		/// <value><c>true</c> if the client side is enabled; otherwise, <c>false</c>.</value>
		protected virtual bool ClientSideEnabled
		{
			get
			{
				object local = ViewState["ClientSideEnabled"];
				if (local != null)
					ViewState["ClientSideEnabled"] = this.IsUpLevel();
				return (bool)ViewState["ClientSideEnabled"];
			}
			set
			{
				ViewState["ClientSideEnabled"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the resource name.
		/// </summary>
		/// <value>The resource name.</value>
		protected virtual string ResourceName
		{
			get
			{
				object local = ViewState["RenResourceNamederType"];
				if (local != null)
					return (string)local;
				else
					return "ActiveUp.WebControls._resources.base.js";
			}
			set
			{
				ViewState["RenResourceNamederType"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the script key.
		/// </summary>
		/// <value>The script key.</value>
		protected virtual string ScriptKey
		{
			get
			{
				object local = ViewState["ScriptKey"];
				if (local != null)
					return (string)local;
				else
					return this.ToString();
			}
			set
			{
				ViewState["ScriptKey"] = value;
			}
		}

		/// <summary>
		/// Register the Client-Side script block in the ASPX page.
		/// </summary>
		protected void RegisterClientSideScriptBlock(System.Web.UI.Page page) 
		{
			// Register the script block is not allready done.

			if (!page.IsClientScriptBlockRegistered(this.ScriptKey)) 
			{
				if ((this.ExternalScript == null || this.ExternalScript == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
				{
#if (!FX1_1)
            Page.ClientScript.RegisterClientScriptInclude(ScriptKey, Page.ClientScript.GetWebResourceUrl(this.GetType(), ResourceName));
#else
					if (CLIENTSIDE_API == null)
						CLIENTSIDE_API = EditorHelper.GetResource(this.ResourceName).ToString();
					
					if (!CLIENTSIDE_API.StartsWith("<script"))
						CLIENTSIDE_API = "<script language=\"javascript\">\n" + CLIENTSIDE_API;

					CLIENTSIDE_API += "\n</script>\n";

					page.RegisterClientScriptBlock(this.ScriptKey, CLIENTSIDE_API);
#endif					
				}
				else
				{
					if (this.ScriptDirectory.StartsWith("~"))
						this.ScriptDirectory = this.ScriptDirectory.Replace("~", System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
					page.RegisterClientScriptBlock(this.ScriptKey, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActiveInput.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
			}


			if (!Page.IsClientScriptBlockRegistered(this.ScriptKey + "_startup"))
			{
				string startupString = string.Empty;
				startupString += "<script>\n";
				startupString += "// Test if the client script is present.\n";
				startupString += "try\n{\n";
				startupString += "AIP_testIfScriptPresent();\n";
				startupString += "}\n catch (e)\n {\n alert('Could not find script file. Please ensure that the Javascript files are deployed in the " + ((ScriptDirectory == string.Empty) ? string.Empty : " [" + ScriptDirectory + "] directory or change the") + "ScriptDirectory and/or ExternalScript properties to point to the directory where the files are.'); \n}\n";
				startupString += "</script>\n";

				page.RegisterClientScriptBlock(this.ScriptKey + "_startup",startupString);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.PreRender"/>
		/// event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			
			_clientSideEnabled = IsUpLevel();

			if (_clientSideEnabled)
			{
				if (((base.Page != null) /*&& base.Enabled*/))
				{
					RegisterClientSideScriptBlock(this.Page);
				}
			}
		}

		#region Behavior Properties
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
		/// Determine if we need to register the client side script and render the calendar, selectors with validation or selectors only.
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

		protected override string ExternalScriptDefault
		{
			get
			{
				return Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, ExternalScriptDefaultFx1);
			}
		}
		#endregion
	}
}
