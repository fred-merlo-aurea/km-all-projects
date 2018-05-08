using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="HtmlProtector"/> object.
	/// </summary>
	[
		DefaultProperty("Text"), 
		ToolboxData("<{0}:HtmlProtector runat=server></{0}:HtmlProtector>"),
		ToolboxBitmap(typeof(HtmlProtector), "ToolBoxBitmap.Protector.bmp"),
        Designer(typeof(HtmlProtectorControlDesigner))
	]
	public class HtmlProtector : System.Web.UI.Control
	{
		private bool _removeLineFeeds = false, _insertBlankLines = false, _disableTextSelection = false,
			_disableRightClick = false, _disablePagePrinting = false, _disableClipBoard = false,
			_disableImageToolbar = false, _disableStatusLinks = false, _disableDragAndDrop = false,
			_totalProtectionEnabled = false;

		private string _externalScript = string.Empty, /*_scriptDirectory = "/aspnet_client/ActiveWebControls/" + StaticContainer.VersionString + "/"*/ _scriptDirectory = string.Empty;
			/*_license;*/

#if (LICENSE)
		private static int _useCounter;
		//private string _license = string.Empty;
#endif

		// Consts
		private static string CLIENTSIDE_API;
		private const string SCRIPTKEY = "HtmlProtector";

		/// <summary>
		/// Initializes a new instance of the <see cref="HtmlProtector"/> class.
		/// </summary>
		public HtmlProtector()
		{
#if (!FX1_1)
            _scriptDirectory = string.Empty;
#else
			_scriptDirectory = Define.SCRIPT_DIRECTORY;
#endif
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
		/// <summary>
		/// Gets or sets a value indicating whether disable the image toolbar.
		/// </summary>
		/// <value><c>true</c> if disable the image toolbar; otherwise, <c>false</c>.</value>
		public bool DisableImageToolbar
		{
			get
			{
				return _disableImageToolbar;
			}
			set
			{
				_disableImageToolbar = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether total protection is enabled.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if total protection is enabled; otherwise, <c>false</c>.
		/// </value>
		public bool TotalProtectionEnabled
		{
			get
			{
				return _totalProtectionEnabled;
			}
			set
			{
				_totalProtectionEnabled = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether disable drag and drop.
		/// </summary>
		/// <value><c>true</c> if disable drag and drop; otherwise, <c>false</c>.</value>
		public bool DisableDragAndDrop
		{
			get
			{
				return _disableDragAndDrop;
			}
			set
			{
				_disableDragAndDrop = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether disables status links.
		/// </summary>
		/// <value><c>true</c> if disables status links; otherwise, <c>false</c>.</value>
		public bool DisableStatusLinks
		{
			get
			{
				return _disableStatusLinks;
			}
			set
			{
				_disableStatusLinks = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether disable clip board.
		/// </summary>
		/// <value><c>true</c> if disable clip board; otherwise, <c>false</c>.</value>
		public bool DisableClipBoard
		{
			get
			{
				return _disableClipBoard;
			}
			set
			{
				_disableClipBoard = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether disable page printing.
		/// </summary>
		/// <value><c>true</c> if disable page printing; otherwise, <c>false</c>.</value>
		public bool DisablePagePrinting
		{
			get
			{
				return _disablePagePrinting;
			}
			set
			{
				_disablePagePrinting = value;
			}
		}
		
		/// <summary>
		/// Gets or sets a value indicating whether disable right click.
		/// </summary>
		/// <value><c>true</c> if disable right click; otherwise, <c>false</c>.</value>
		public bool DisableRightClick
		{
			get
			{
				return _disableRightClick;
			}
			set
			{
				_disableRightClick = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether disable text selection.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [disable text selection]; otherwise, <c>false</c>.
		/// </value>
		public bool DisableTextSelection
		{
			get
			{
				return _disableTextSelection;
			}
			set
			{
				_disableTextSelection = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether inserts blank lines.
		/// </summary>
		/// <value><c>true</c> if inserts blank lines; otherwise, <c>false</c>.</value>
		public bool InsertBlankLines
		{
			get
			{
				return _insertBlankLines;
			}
			set
			{
				_insertBlankLines = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether remove line feeds.
		/// </summary>
		/// <value><c>true</c> if removes line feeds; otherwise, <c>false</c>.</value>
		public bool RemoveLineFeeds
		{
			get
			{
				return _removeLineFeeds;
			}
			set
			{
				_removeLineFeeds = value;
			}
		}

		/// <summary>
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
				return _scriptDirectory;
			}
			set
			{
				_scriptDirectory = value;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.PreRender"/>
		/// event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			bool formPresent = false;
 
			foreach(Control control in Page.Controls)
			{
				if (control.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlForm")
				{
					formPresent = true;
 
					break;
				}
			}
 
			if (!formPresent && (this.DisableClipBoard || this.DisableDragAndDrop ||
				this.DisablePagePrinting || this.DisableRightClick || this.DisableStatusLinks ||
				this.DisableTextSelection))
			{
				throw new Exception("Some of the selected protection can't be used without a <form> tag with its ID, method=\"post\" and Runat=\"server\" attributes.");
			}

			if (_totalProtectionEnabled)
			{
				this.DisableClipBoard = true;
				this.DisableDragAndDrop = true;
				this.DisableImageToolbar = true;
				this.DisablePagePrinting = true;
				this.DisableRightClick = true;
				this.DisableStatusLinks = true;
				this.DisableTextSelection = true;
				this.InsertBlankLines = true;
				this.RemoveLineFeeds = true;
			}

			if (Page.Request.Browser.Crawler)
			{
				this.DisableClipBoard = false;
				this.DisableDragAndDrop = false;
				this.DisableImageToolbar = false;
				this.DisablePagePrinting = false;
				this.DisableRightClick = false;
				this.DisableStatusLinks = false;
				this.DisableTextSelection = false;
				this.InsertBlankLines = false;
				this.RemoveLineFeeds = false;
			}

			System.Text.StringBuilder startupScript = new System.Text.StringBuilder();

			if (!Page.IsStartupScriptRegistered(SCRIPTKEY + "_Test"))
				Page.RegisterStartupScript(SCRIPTKEY + "_Test", "<script>try { HPT_Test(); } catch (awse) { alert(awse); alert('Could not find external script file. Please Check the documentation.'); }</script>");

			if (_disableRightClick && !Page.IsStartupScriptRegistered(SCRIPTKEY + "_RightClick"))
			{
				startupScript.Append("<script>\n");
				startupScript.Append("HPT_DisableRightClick();\n");
				startupScript.Append("</script>\n");

				Page.RegisterStartupScript(SCRIPTKEY + "_RightClick", startupScript.ToString());
			}

			/*if (_disableRightClick && !Page.IsStartupScriptRegistered(SCRIPTKEY + "_RightClick"))
			{
				startupScript.Append("<script>\n");
				startupScript.Append("protectImages()\n");
				startupScript.Append("</script>\n");

				Page.RegisterStartupScript(SCRIPTKEY + "_RightClick", startupScript.ToString());
			}*/

			if (_disableTextSelection && !Page.IsStartupScriptRegistered(SCRIPTKEY + "_TextSelection"))
			{
				startupScript = new System.Text.StringBuilder();
				startupScript.Append("<script>\n");
				startupScript.Append("HPT_DisableTextSelection();\n");
				startupScript.Append("</script>\n");

				Page.RegisterStartupScript(SCRIPTKEY + "_TextSelection", startupScript.ToString());
			}

			if (_disablePagePrinting && !Page.IsStartupScriptRegistered(SCRIPTKEY + "_PagePrinting"))
			{
				startupScript = new System.Text.StringBuilder();
				startupScript.Append("<style type=\"text/css\" media=\"print\"><!--body{display:none}--></style>\n");
				startupScript.Append("<script>\n");
				startupScript.Append("HPT_DisablePagePrinting();\n");
				startupScript.Append("</script>\n");

				Page.RegisterStartupScript(SCRIPTKEY + "_PagePrinting", startupScript.ToString());
			}

			if (_disableClipBoard && !Page.IsStartupScriptRegistered(SCRIPTKEY + "_ClipBoard"))
			{
				startupScript = new System.Text.StringBuilder();
				startupScript.Append("<div style=\"position:absolute;left:-1000px;top:-1000px\"><input type=\"textarea\" id=\"copiedNothing\" name=\"copiedNothing\" value=\" \" style=\"visibility:hidden\"></div>\n");
				startupScript.Append("<script>\n");
				startupScript.Append("HPT_DisableClipBoard();\n");
				startupScript.Append("</script>\n");

				Page.RegisterStartupScript(SCRIPTKEY + "_ClipBoard", startupScript.ToString());
			}

			if (_disableImageToolbar && !Page.IsStartupScriptRegistered(SCRIPTKEY + "_ImageToolbar"))
			{
				Page.RegisterStartupScript(SCRIPTKEY + "_ImageToolbar", "<META HTTP-EQUIV=\"imagetoolbar\" CONTENT=\"no\">\n");
			}

			if (_disableStatusLinks && !Page.IsStartupScriptRegistered(SCRIPTKEY + "_StatusLinks"))
			{
				startupScript = new System.Text.StringBuilder();
				startupScript.Append("<script>\n");
				startupScript.Append("HPT_DisableStatusLinks();\n");
				startupScript.Append("</script>\n");

				Page.RegisterStartupScript(SCRIPTKEY + "_StatusLinks", startupScript.ToString());
			}

			if (_disableDragAndDrop && !Page.IsStartupScriptRegistered(SCRIPTKEY + "_DragAndDrop"))
			{
				startupScript = new System.Text.StringBuilder();
				startupScript.Append("<script>\n");
				startupScript.Append("HPT_DisableDragAndDrop();\n");
				startupScript.Append("</script>\n");

				Page.RegisterStartupScript(SCRIPTKEY + "_DragAndDrop", startupScript.ToString());
			}

			RegisterScriptBlock();
		}

		/// <summary>
		/// Register the Client-Side script block in the ASPX page.
		/// </summary>
		public void RegisterScriptBlock() 
		{
			// Register the script block is not allready done.
			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
#if (!FX1_1)
            Page.ClientScript.RegisterClientScriptInclude(SCRIPTKEY, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.HtmlProtector.js"));
#else
				if ((this.ExternalScript == null || this.ExternalScript.TrimEnd() == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
				{
					if (CLIENTSIDE_API == null)
						CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls._resources.HtmlProtector.js");
					
					if (!CLIENTSIDE_API.StartsWith("<script"))
						CLIENTSIDE_API = "<script language=\"javascript\">\n" + CLIENTSIDE_API;

					CLIENTSIDE_API += "\n</script>\n";

					Page.RegisterClientScriptBlock(SCRIPTKEY, CLIENTSIDE_API);
				}
				else
				{
					if (this.ScriptDirectory.StartsWith("~"))
						this.ScriptDirectory = this.ScriptDirectory.Replace("~", System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "HtmlProtector.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></script>");
				}
#endif
            }
            if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY + "_startup"))
			{
					string startupString = string.Empty;
					startupString += "<script>\n";
					startupString += "// Test if the client script is present.\n";
					startupString += "try\n{\n";
					startupString += "HPT_testIfScriptPresent();\n";
					//startupString += string.Format("}\n catch\n {\n alert('Could not find script file. Please ensure that the Javascript files are deployed in the {0}ScriptDirectory and/or ExternalScript properties to point to the directory where the files are.'); \n}\n",(ScriptDirectory == string.Empty) ? string.Empty : " [" + ScriptDirectory + "] directory or change the");
					startupString += "}\n catch (e)\n {\n alert('Could not find script file. Please ensure that the Javascript files are deployed in the " + ((ScriptDirectory == string.Empty) ? string.Empty : " [" + ScriptDirectory + "] directory or change the") + "ScriptDirectory and/or ExternalScript properties to point to the directory where the files are.'); \n}\n";
					startupString += "</script>\n";

					Page.RegisterClientScriptBlock(SCRIPTKEY + "_startup",startupString);
			}
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{

#if (LICENSE)
			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.HPT, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

			_useCounter++;

			if (/*!licenseStatus.IsRegistered &&*/ Page != null && _useCounter == StaticContainer.UsageCount)
			{
				_useCounter = 0;
				output.Write(StaticContainer.TrialMessage);
			}
			else
			{
				RenderProtector(output);				
			}

#else
			RenderProtector(output);
#endif
		}

		/// <summary>
		/// Renders the protector.
		/// </summary>
		/// <param name="output">The output.</param>
		protected void RenderProtector(HtmlTextWriter output) 
		{
			if (_insertBlankLines)
				for(int index=0;index<250;index++)
					output.Write("\r\n");

			//output.Write("<!--- Start --->\n");
            ProtectControls(output,this.Controls);			
			//output.Write("<!--- End --->\n");
		}

        protected void ProtectControls(HtmlTextWriter output,ControlCollection controls)
        {	
            foreach(Control ctl in controls)
			{
                if (ctl.GetType().FullName == "System.Web.UI.LiteralControl" ||
                    ctl.GetType().FullName == "System.Web.UI.ResourceBasedLiteralControl")
                {
                    string text = ((LiteralControl)ctl).Text;

                    if (_removeLineFeeds)
                        text = text.Replace("\n", "").Replace("\r\n", "");

                    //output.Write((Page.Request.Browser.Crawler ? text : HtmlProtector.HtmlEncrypt(text)));
                    output.Write(HtmlProtector.HtmlEncrypt(text));

                }
                else
                {
                    System.IO.StringWriter sw = new System.IO.StringWriter();
                    HtmlTextWriter textWriter = new HtmlTextWriter(sw);
                    ctl.RenderControl(textWriter);
                    //output.Write((Page.Request.Browser.Crawler ? textWriter.InnerWriter.ToString() : HtmlProtector.HtmlEncrypt(textWriter.InnerWriter.ToString())));
                    output.Write(HtmlProtector.HtmlEncrypt(textWriter.InnerWriter.ToString()));
                    //ctl.RenderControl(output);
                }

                /*if (ctl.Controls.Count > 0)
                {
                    ProtectControls(output,ctl.Controls);
                }*/
            }
        }

		/// <summary>
		/// Encrypts the html.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		public static string HtmlEncrypt(string input)
		{
			char[] str = input.ToCharArray();

			string encrypted = string.Empty;

			foreach(char ch in str)
				encrypted += "%" + String.Format("{0:x2}", checked((uint)System.Convert.ToUInt32(ch)));
									 
			return "<script>document.write(unescape(\"" + encrypted + "\"));</script>";
		}
	}
}
