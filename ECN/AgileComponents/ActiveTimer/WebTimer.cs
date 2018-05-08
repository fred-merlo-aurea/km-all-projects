// ActiveTimer 1.x
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
using System.Text;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="WebTimer"/> object.
	/// </summary>
    [ToolboxBitmap(typeof(WebTimer), "ToolBoxBitmap.Timer.bmp")]
	[Designer(typeof(WebTimerControlDesigner))]
	public class WebTimer : CoreWebControl, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
	{
		// Consts
		private static string CLIENTSIDE_API;
		private const string SCRIPTKEY = "WebTimer";

		private string _externalScript = string.Empty;

#if (LICENSE)
			//private string _license = string.Empty;
			private int _useCounter = 0;
#endif

		/// <summary>
		/// Initializes a new instance of the <see cref="WebTimer"/> class.
		/// </summary>
		public WebTimer()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Gets or sets the relative or absolute path to the external API javascript file.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the Html TextBox render.</remarks>
		[Bindable(false),
		Category("Behavior"),
		Description("Gets or sets the relative or absolute path to the external API javascript file.")	]
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
		/// The frequency of ellapsed events in milliseconds.
		/// </summary>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue(""),
		Description("The frequency of ellapsed events in milliseconds.")
		] 
		public int Interval
		{
			get
			{
				if (ViewState["_interval"] == null)
					ViewState["_interval"] = 1000;

				return Convert.ToInt32(ViewState["_interval"]); 
			}
			set
			{
				ViewState["_interval"] = value;
			}
		}

		/// <summary>
		/// The target date/time for countdown.
		/// </summary>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue(""),
		Description("The target date/time for countdown.")
		] 
		public DateTime TargetDateTime
		{
			get
			{
				if (ViewState["_targetDateTime"] == null)
					ViewState["_targetDateTime"] = DateTime.MinValue;

				return Convert.ToDateTime(ViewState["_targetDateTime"]); 
			}
			set
			{
				ViewState["_targetDateTime"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the code to execute client side. 
		/// </summary>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue(true),
		Browsable(true),
		Description("Gets or sets the code to execute client side.")
		] 
		public string ClientSideEvent
		{
			get
			{
				object _clientSideEvent;
				_clientSideEvent = ViewState["_clientSideEvent"];
				if (_clientSideEvent != null)
				{
					return ((string) _clientSideEvent); 
				}
				return string.Empty;
			}
			set
			{
				ViewState["_clientSideEvent"] = value;
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
				return false;
			}
			set
			{
				ViewState["_autoPostBack"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether or not the control repeat the time client-side. 
		/// </summary>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue(false),
		Browsable(true),
		Description("Gets or sets a value that indicates whether or not the control repeat the time client-side. ")
		] 
		public bool Repeat
		{
			get
			{
				object _repeat;
				_repeat = ViewState["_repeat"];
				if (_repeat != null)
				{
					return ((bool) _repeat); 
				}
				return false;
			}
			set
			{
				ViewState["_repeat"] = value;
			}
		}
		/// <summary>
		/// Gets or sets a value that indicates whether or not the control repeat the time client-side. 
		/// </summary>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue(false),
		Browsable(true),
		Description("Gets or sets a value that indicates whether or not the control repeat the time client-side. ")
		] 
		public bool Countdown
		{
			get
			{
				object _countdown;
				_countdown = ViewState["_countdown"];
				if (_countdown != null)
				{
					return ((bool) _countdown); 
				}
				return false;
			}
			set
			{
				ViewState["_countdown"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether or not the client time is synch to the server time. 
		/// </summary>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue(true),
		Browsable(true),
		Description("Gets or sets a value that indicates whether or not the client time is synch to the server time.")
		] 
		public bool SynchToServer
		{
			get
			{
				object _synchToServer;
				_synchToServer = ViewState["_synchToServer"];
				if (_synchToServer != null)
				{
					return ((bool) _synchToServer); 
				}
				return false;
			}
			set
			{
				ViewState["_synchToServer"] = value;
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
#endif
*/
		/*/// <summary>
		/// Gets or sets a value that indicates whether or not the timer is visually displayed. 
		/// </summary>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue(true),
		Browsable(true),
		Description("Gets or sets a value that indicates whether or not the timer is visually displayed.")
		] 
		public bool Show
		{
			get
			{
				object _show;
				_show = ViewState["_show"];
				if (_show != null)
				{
					return ((bool) _show); 
				}
				return false;
			}
			set
			{
				ViewState["_show"] = value;
			}
		}*/
		#region Events
		
		/// <summary>
		/// The Tick event handler. Fire when at each interval.
		/// </summary>
		[Category("Event")]
		public event EventHandler Tick;

		/// <summary>
		/// A OnTick event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnTick(EventArgs e) 
		{
			if (Tick != null)
				Tick(this,e);
		}
		#endregion

		#region Interface IPostBack

		/// <summary>
		/// A RaisePostBackEvent.
		/// </summary>
		/// <param name="eventArgument">eventArgument</param>
		public void RaisePostBackEvent(String eventArgument)
		{
			OnTick(EventArgs.Empty);
		}

		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{
			try
			{
				int presentValue = this.Interval;
				int postedValue = int.Parse(postCollection[postDataKey + "_Interval"]);
			
				bool presentValue2 = this.Enabled;
				bool postedValue2 = bool.Parse(postCollection[postDataKey + "_Enabled"]);
				
				if (!presentValue2.Equals(postedValue2)) 
				{
					this.Enabled = postedValue2;
				}

				if (!presentValue.Equals(postedValue)) 
				{
					this.Interval = postedValue;

					return true;
				}
			}
			catch
			{	
				return false;
			}

			return false;

		}

		/// <summary>
		/// When implemented by a class, signals the server control object to notify the ASP.NET application that the state of the
		/// control has changed.
		/// </summary>
		public virtual void RaisePostDataChangedEvent() 
		{
			//OnTick(EventArgs.Empty);
		}

		#endregion

		private void RegisterScriptBlock()
		{
			/*if (!Page.IsClientScriptBlockRegistered("AU_WebTimer"))
			{

				System.Text.StringBuilder api = new System.Text.StringBuilder();

				api.Append("<script language=\"javascript\">\n");
				api.Append("function WTM_DoTimer(code, interval, repeat, init)\n");
				api.Append("{\n");
				api.Append("	if (!init)\n");
				api.Append("		setTimeout(code, 10);\n");
				api.Append("\n");
				api.Append("	if (repeat)\n");
				api.Append("	{\n");
				api.Append("		repeatStr = \"WTM_DoTimer(\\\"\" + code + \"\\\", \" + interval + \", \" + repeat + \", false);\";\n");
				api.Append("		setTimeout(repeatStr, interval);\n");
				api.Append("	}\n");
				api.Append("	else\n");
				api.Append("		setTimeout(code, interval);\n");
				api.Append("}\n");
				api.Append("function WTM_SetInterval(id, interval)\n");
				api.Append("{\n");
				api.Append("	document.getElementById(id + '_Interval').value = interval;\n");
				api.Append("}\n");
				api.Append("</script>\n");
				
				Page.RegisterClientScriptBlock("AU_WebTimer", api.ToString());
			}*/

			// Register the script block is not allready done.
			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
				if ((this.ExternalScript == null || this.ExternalScript.TrimEnd() == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
				{
#if (!FX1_1)
            Page.ClientScript.RegisterClientScriptInclude(SCRIPTKEY, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.ActiveTimer.js"));
#else
					if (CLIENTSIDE_API == null)
						CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls._resources.ActiveTimer.js");
				
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
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActiveTimer.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
			}

			
			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY + "_startup"))
			{
				string startupString = string.Empty;
				startupString += "<script>\n";
				startupString += "// Test if the client script is present.\n";
				startupString += "try\n{\n";
				startupString += "AWT_testIfScriptPresent();\n";
				//startupString += "}\ncatch (e) \n{\nalert('Could not find external script file. Please Check the documentation.');\n}\n";
				startupString += "}\n catch (e)\n {\n alert('Could not find script file. Please ensure that the Javascript files are deployed in the " + ((ScriptDirectory == string.Empty) ? string.Empty : " [" + ScriptDirectory + "] directory or change the") + "ScriptDirectory and/or ExternalScript properties to point to the directory where the files are.'); \n}\n";
				startupString += "</script>\n";

				Page.RegisterClientScriptBlock(SCRIPTKEY + "_startup",startupString);
			}

			if (!Page.IsClientScriptBlockRegistered("AWT_SERVERTIME"))
			{
				DateTime d = DateTime.Now;
				string nowDate = string.Format("{0},{1},{2},{3},{4},{5}", d.Year.ToString(), d.Month.ToString(), d.Day.ToString(), d.Hour.ToString(), d.Minute.ToString(), d.Second.ToString());
			
				string serverTime = string.Format("<script language='javascript'>\nvar AWT_ServerTime = '{0}';\nvar AWT_ClientTime = new Date();\nvar AWT_DateDiff = AWT_ClientTime.getTime() - AWT_ParseDate(AWT_ServerTime).getTime();\n</script>", nowDate);

				Page.RegisterClientScriptBlock("AWT_SERVERTIME", serverTime);
			}

		}

		/// <summary>
		/// Notifies the tool to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{
			RegisterScriptBlock();

			if (base.Page != null && this.AutoPostBack)
				Page.RegisterRequiresPostBack(this);
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
			licenses.Add(new LicenseProduct(ProductCode.AWT, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

			_useCounter++;

			if (/*!licenseStatus.IsRegistered &&*/ Page != null
				&& !Page.IsStartupScriptRegistered("Register_Popup") && _useCounter == StaticContainer.UsageCount)
			{
				_useCounter = 0;
				output.Write(StaticContainer.TrialMessage);
			}
			else
			{
				RenderWebTimer(output);
			}
#else
			RenderWebTimer(output);
#endif
		}

		/// <summary>
		/// Renders the web timer.
		/// </summary>
		/// <param name="output">The output.</param>
		protected void RenderWebTimer(HtmlTextWriter output) 
		{
			//if (this.Show)
			if (this.Countdown)
				base.Render(output);

			// Render the interval container
			output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
			//output.AddAttribute(HtmlTextWriterAttribute.Value, (this.Countdown ? "1000" : this.Interval.ToString()));
			output.AddAttribute(HtmlTextWriterAttribute.Value, this.Interval.ToString());
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_Interval");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_Interval");
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			// Render the enabled container
			output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Value,this.Enabled.ToString());
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_Enabled");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_Enabled");
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			// Render the target date container
			output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
			DateTime d = this.TargetDateTime;
			string targetDate = string.Format("{0},{1},{2},{3},{4},{5}", d.Year.ToString(), d.Month.ToString(), d.Day.ToString(), d.Hour.ToString(), d.Minute.ToString(), d.Second.ToString());
			output.AddAttribute(HtmlTextWriterAttribute.Value, targetDate);
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_TargetDate");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_TargetDate");
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();
			
			/*if (this.AutoPostBack)
				{
					output.Write(string.Format("<script>setTimeout(\"{0}\", {1});</script>", this.Page.GetPostBackEventReference(this), this.Interval.ToString()));
				}
				else
				{*/
			output.Write(string.Format("<script language=\"javascript\">AWT_DoTimer(\"{0}\", \"{1}\", {2}, {3}, true, {4});</script>", this.ClientID, (this.AutoPostBack ? this.Page.GetPostBackEventReference(this) : this.ClientSideEvent), (this.Countdown ? "true" : this.Repeat.ToString().ToLower()), this.Countdown.ToString().ToLower(), this.SynchToServer.ToString().ToLower()));
			//}
			//}
		}
	}
}
