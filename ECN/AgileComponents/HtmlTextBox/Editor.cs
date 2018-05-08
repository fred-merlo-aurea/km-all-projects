#define DEBUG

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using ActiveUp.WebControls.Common;
using ActiveUp.WebControls.Design;

[assembly: CLSCompliantAttribute(true)]

namespace ActiveUp.WebControls
{
    /// <summary>
    /// An online web based WYSIWYG HTML editor.
    /// </summary>
    [ToolboxData("<{0}:Editor runat=server></{0}:Editor>")]
    [ValidationPropertyAttribute("Text")]
    [CLSCompliantAttribute(true)]
    [ComVisibleAttribute(true)]
    [DefaultPropertyAttribute("Toolbars")]
    [Designer(typeof(EditorDesigner))]
    //[ParseChildrenAttribute(false)]
    //[PersistChildren(true)]
    [ParseChildren(true)]
    //[Editor(typeof(EditorComponentEditor), typeof(ComponentEditor))]
    [ToolboxBitmap(typeof(Editor), "ToolBoxBitmap.Editor.bmp")]
    [Serializable]
    public class Editor : CoreWebControl, INamingContainer, IPostBackDataHandler
    {
        private const string EditorModeDesignIconFx1 = "tab_design.gif";
        private string _text, _externalScript, _textareaCols, _textareaRows, _onFocus,
            _onBlur, _startupScript, _contentCssFile, _textareaCssClass, /*_license,*/ _localizationFile;
        private EditorMode _startupMode = EditorMode.Design;
        private bool _hardReturnEnabled, _hackProtectionDisabled, _toolsCreated, _autoCreateTools, _cleanOnPaste, _useBR, _allowTransparency, _persistText, _startupFocus, _localizationSettingsApplied, _editable;
        private Unit _editorAreaWidth;
        //private string _template = string.Empty;
        private ToolbarsContainer _toolbarsContainer = new ToolbarsContainer();
        private Popup _popupMenu = new Popup();
        private ToolLibrary _toolLibrary;
        private ToolSpellChecker _toolSpellChecker;
        private ToolTemplateManager _toolTemplateManager;
        private string KEY_STYLE = "HTB_STYLE";
        internal static int indexTools = 0;
        internal bool _iconsIsPreloaded = false;
        private int _maxLength;
        private EditorMaxLengthMode _maxLengthMode;
        private KeyDisabledItemCollection _keysDisabled = new KeyDisabledItemCollection();
        private ContextMenuItemCollection _contextMenuCustomItems = new ContextMenuItemCollection();
        private Version _version = null;
        private ContextMenuStyle _contextMenuStyle;
        private bool _autoHideToolbars;
        private LocalizationSettings _labels;
        HtmlInputFile hif = new HtmlInputFile();
#if (LICENSE)
		private static int _useCounter;
		//private string _license = string.Empty;
#endif
        private bool _popupToolbars;
        private bool _showNavigation;

        // Consts
        private static string CLIENTSIDE_API;
        private const string HTMLTEXTBOXSCRIPTKEY = "HtmlTextBox";

        /// <summary>
        /// Initializes a new instance of the <see cref="Editor"/> class.
        /// </summary>
        public Editor()
        {
            _version = StaticContainer.VersionObject;
            ControlStyle.BorderColor = System.Drawing.Color.DarkGray;
            ControlStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#DBD8D1");
            //ControlStyle.Font.Name = "Verdana";
            //ControlStyle.Font.Size = FontUnit.XSmall;
            ControlStyle.Height = Unit.Pixel(350);
            ControlStyle.Width = Unit.Pixel(620);
            EditorAreaWidth = Unit.Percentage(100);
            ControlStyle.BorderWidth = Unit.Pixel(1);
            //_template = "bold,italic,underline,separator,cut,copy,paste,separator,separator,paragraph,fontface,fontsize,fontcolor,highlight;alignleft,aligncenter,alignright,alignjustify,separator,orderedlist,unorderedlist,separator,indent,outdent,separator,print,separator,subscript,superscript,strikethrough,separator,find,replace,image,link,specialchars,codecleaner,table,flash";
            _autoCreateTools = true;
            _maxLength = 0;
            _maxLengthMode = EditorMaxLengthMode.Editor;
            _startupFocus = false;
            _hardReturnEnabled = true;
            _useBR = true;
            _contextMenuStyle = new ContextMenuStyle();
            this.TextareaColumns = string.Empty;
            this.TextareaCssClass = string.Empty;
            this.TextareaRows = string.Empty;
            _autoHideToolbars = false;
            _externalScript = string.Empty;
            _toolLibrary = new ToolLibrary(this.ClientID + "_toolLibrary");
            _toolSpellChecker = new ToolSpellChecker(this.ClientID + "_toolSpellChecker");
            _toolTemplateManager = new ToolTemplateManager(this.ClientID + "_toolTemplateManager");
            _localizationFile = string.Empty;
            _editable = true;
            _popupToolbars = false;
        }

        #region Behavior Properties
        /// <summary>
        /// Gets or sets the relative or absolute path to the external Html TextBox API javascript file.
        /// </summary>
        /// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the Html TextBox render.</remarks>
        [Bindable(false),
        Category("Behavior"),
        Description("Gets or sets the relative or absolute path to the external Html TextBox API javascript file.")]
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
        /// Gets or sets the editor type.
        /// </summary>
        /// <value>The type of the editor.</value>
        [
            Bindable(false),
            Browsable(false)
        ]
        public EditorType EditorType
        {
            get
            {
                object editorType = ViewState["HTB_EditorType"];
                if (editorType == null)
                {
                    ViewState["HTB_EditorType"] = EditorType.NotSet;
                }

                return (EditorType)ViewState["HTB_EditorType"];
            }
            set
            {
                ViewState["HTB_EditorType"] = value;
            }
        }

        /// <summary>
        /// Image used as background of the toolbar container.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
#if (!FX1_1)
 DefaultValue(""),
#else
		DefaultValue("back_l.gif"),
#endif
 Browsable(true),
        NotifyParentProperty(true),
        Description("Image used as background of the toolbar container.")
        ]
        public string BackImage
        {
            get
            {
                object backImage = ViewState["_backImageToolBarContainer"];
                if (backImage != null)
                    return (string)backImage;
                else
#if (!FX1_1)
                    return string.Empty;
#else
					return "back_l.gif";
#endif
            }

            set
            {
                ViewState["_backImageToolBarContainer"] = value;
            }
        }


        /// <summary>
        /// Gets or sets the number of columns for the Textarea if used.
        /// </summary>
        [Bindable(false),
        Category("Appearance"),
        Description("Get or set the number of columns for the Textarea if used.")]
        public string TextareaColumns
        {
            get
            {
                return _textareaCols;
            }

            set
            {
                _textareaCols = value;
            }
        }

        /// <summary>
        /// Gets or sets the additional JScript client-side functions to call when the editor get focus.
        /// </summary>
        [Bindable(false),
        Category("Behavior"),
        Description("Gets or sets the additional JScript client-side functions to call when the editor get focus.")]
        public string OnFocus
        {
            get
            {
                return _onFocus;
            }
            set
            {
                _onFocus = value;
            }
        }

        /// <summary>
        /// Gets or sets the additional JScript client-side functions to call when the editor get focus.
        /// </summary>
        [Bindable(false),
        Category("Behavior"),
        Description("Gets or sets the additional JScript client-side functions to call when the editor loose focus.")]
        public string OnBlur
        {
            get
            {
                return _onBlur;
            }
            set
            {
                _onBlur = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of rows for the Textarea if used.
        /// </summary>
        [Bindable(false),
        Category("Appearance"),
        Description("Get or set the number of rows for the Textarea if used.")]
        public string TextareaRows
        {
            get
            {
                return _textareaRows;
            }

            set
            {
                _textareaRows = value;
            }
        }

        /// <summary>
        /// Specify whether you want the editor to insert a new paragraph instead of a new line (BR) when the user hit RETURN.
        /// </summary>
        /// <remarks>Inserting new line can be usefull if you disable the normal line feed. In this case, just tell your end-users to press SHIFT-ENTER to add a BR (line feed) tag instead of a P (paragraph). When in this mode, you can insert an hard return by pressing CTRL-M. You can also change the behavior while editing by pressing CTRL-K.</remarks>
        [Bindable(false),
        Category("Behavior"),
        Description("Specify whether you want the editor to insert a BR tag instead of a new paragraph when the user hit RETURN."),
        DefaultValueAttribute(true)
        ]
        public bool HardReturnEnabled
        {
            get
            {
                return _hardReturnEnabled;
            }
            set
            {
                _hardReturnEnabled = value;
            }
        }


        /// <summary>
        /// Specify whether you want the editor to protect the content against potential javascript attacks.
        /// </summary>
        [
            Bindable(false),
            Category("Behavior"),
            Description("Specify whether you want the editor to protect the content against potential javascript attacks."),
            DefaultValue(false)
        ]
        public bool HackProtectionDisabled
        {
            get
            {
                return _hackProtectionDisabled;
            }
            set
            {
                _hackProtectionDisabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the editor area width. Usually to force good previewing of the edited document when thinner than the toolbar width.
        /// </summary>
        [
            Bindable(false),
            Category("Appearance"),
            Description("Gets or sets the editor area width. Usually to force good previewing of the edited document when thinner than the toolbar width."),
            DefaultValue(typeof(Unit), "100%")
        ]

        public System.Web.UI.WebControls.Unit EditorAreaWidth
        {
            get
            {
                return _editorAreaWidth;
            }
            set
            {
                _editorAreaWidth = value;
            }
        }
        #endregion

        #region Data Properties

        /// <summary>
        /// Gets or sets the HTML source code.
        /// </summary>
        /// <remarks>When the data is posted, this property contains the generated HTML. You can also use this property to specify default text to display when the control is rendered.</remarks>
        [Bindable(true),
        Category("Data"),
        DefaultValue(""),
        Description("Gets or sets the HTML source code.")]
        public string Text
        {
            get
            {
                if (_persistText)
                {
                    if (ViewState["_text"] == null)
                        ViewState["_text"] = string.Empty;
                    return (string)ViewState["_text"];
                }
                else
                    return _text;
            }

            set
            {
                if (_persistText)
                    ViewState["_text"] = value;

                _text = value;
            }
        }

        /// <summary>
        /// Gets or sets the toolbars.
        /// </summary>
        /// <value>The toolbars.</value>
        [Browsable(false)]
        public ToolbarsContainer Toolbars
        {
            get
            {
                if (_toolbarsContainer == null)
                    _toolbarsContainer = new ToolbarsContainer();
                return _toolbarsContainer;
            }
            set
            {
                _toolbarsContainer = value;
            }
        }

        /// <summary>
        /// Gets the version number of HTML TextBox.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Misc"),
        Description("Get the version of Html TextBox.")]
        public System.Version Version
        {
            get
            {
                return _version;
            }
        }
        #endregion

        #region Internal Methods
        /// <summary>
        /// Detects if the client browser is able to handle the client side scripts.
        /// </summary>
        /// <returns>True if the browser is capable.</returns>
        /// <example>
        /// <code>
        /// 
        /// VB
        /// 
        /// Dim Editor1 As New Editor()
        /// If Editor1.RenderUpLevel()
        ///		Editor1.Text = "Enter your formatted text here..."
        ///	Else
        ///		Editor1.Text = "Enter your text here..."
        ///	End If
        ///	
        /// C#
        /// 
        /// Editor Editor1 = new Editor()
        /// if (Editor1.RenderUpLevel())
        /// {
        ///		Editor1.Text = "Enter your formatted text here...";
        ///	}
        ///	else
        ///	{
        ///		Editor1.Text = "Enter your text here...";
        ///	}
        /// </code></example>
        public EditorType DetermineEditorType()
        {
            EditorHelper.DebugTrace("DetermineEditorType");

            if (EditorType == EditorType.NotSet)
            {
                try
                {
                    HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                    string userAgent = System.Web.HttpContext.Current.Request.UserAgent;
                    Page.Trace.Write("Start");
                    // Check if it's mac

                    if (userAgent.IndexOf("Mac") != -1 && userAgent.IndexOf("Gecko") == -1)
                        return EditorType.TextArea;
                    else if (userAgent.IndexOf("Mac") != -1 && userAgent.IndexOf("Gecko") != -1)
                        return EditorType.DHTML;

                    // Check if IE
                    if (browser.MSDomVersion.Major >= 5)
                        return EditorType.DHTML;

                    // Check if Mozzilla 1.3/Netscape 7.1
                    int gIndex = userAgent.IndexOf("Gecko");
                    if (gIndex != -1 && Convert.ToInt64(userAgent.Substring(gIndex + 6, 8)) >= 20030624)
                        return EditorType.DHTML;
                }
                catch
                {
                    // Sometimes, things doesn't happen like you want...
                }

                return EditorType.DHTML;
            }
            else
                return EditorType;
        }
        #endregion

        #region ASP.NET API
        /// <summary>
        /// Register the Client-Side script block in the ASPX page.
        /// </summary>
        public void RegisterAPIScriptBlock()
        {
            // Register the script block is not allready done.

            //_toolbarsContainer.RegisterAPIScriptBlock();

            if (!Page.IsClientScriptBlockRegistered(HTMLTEXTBOXSCRIPTKEY))
            {
                bool isNs6 = false;
                System.Web.HttpBrowserCapabilities browser = Page.Request.Browser;
                if (browser.Browser.ToUpper().IndexOf("IE") == -1)
                    isNs6 = true;

                if (isNs6)
                {
                    if (CLIENTSIDE_API == null)
                        CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls._resources.HtmlTextBox.js");

                    if (!CLIENTSIDE_API.StartsWith("<script"))
                        CLIENTSIDE_API = "<script language=\"javascript\">\n" + CLIENTSIDE_API;

                    CLIENTSIDE_API += "\n</script>\n";

                    Page.RegisterClientScriptBlock(HTMLTEXTBOXSCRIPTKEY, CLIENTSIDE_API);
                }

                else
                {
                    if ((this.ExternalScript == null || this.ExternalScript.TrimEnd() == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
                    {
#if (!FX1_1)
                        Page.ClientScript.RegisterClientScriptInclude(HTMLTEXTBOXSCRIPTKEY, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.HtmlTextBox.js"));
#else
						if (CLIENTSIDE_API == null)
							CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls._resources.HtmlTextBox.js");
						//CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls.HtmlTextBox.HtmlTextBox.js");

						
						if (!CLIENTSIDE_API.StartsWith("<script"))
							CLIENTSIDE_API = "<script language=\"javascript\">\n" + CLIENTSIDE_API;

						CLIENTSIDE_API += "\n</script>\n";

						Page.RegisterClientScriptBlock(HTMLTEXTBOXSCRIPTKEY, CLIENTSIDE_API);
#endif
                    }
                    else
                    {
                        if (this.ScriptDirectory.StartsWith("~"))
                            this.ScriptDirectory = this.ScriptDirectory.Replace("~", System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
                        Page.RegisterClientScriptBlock(HTMLTEXTBOXSCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "HtmlTextBox.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
                    }
                }

                //Page.RegisterHiddenField("HTB_IconsPreloaded","");
            }


            if (!Page.IsClientScriptBlockRegistered(KEY_STYLE))
            {
                string style = string.Empty;

                style += "<style>\n";
                style += ".HTB_clsPopup\n";
                style += "{\n";
                //style +="	background-color: #F9F8F7;\n";
                style += "	background-color: #DDDDDD;\n";
                style += "  font-size:8pt;\n";
                style += "}\n";
                style += ".HTB_clsBackColor\n";
                style += "{\n";
                style += "	background-color: #F9F8F7;\n";
                style += "}\n";
                style += ".HTB_clsFont\n";
                style += "{\n";
                style += "	font-family: Tahoma;\n";
                style += "	font-size: 9pt;\n";
                style += "}\n";
                style += ".HTB_clsColor\n";
                style += "{\n";
                style += "	border: 1px solid #666666;\n";
                style += "	background-color: #FFFFFF;\n";
                style += "}\n";
                style += ".HTB_clsColorCont\n";
                style += "{\n";
                style += "	border: 1px solid #F9F8F7;\n";
                style += "	background-color: #F9F8F7;\n";
                style += "	cursor: default;\n";
                style += "}\n";
                style += ".HTB_clsDropDownItem {\n";
                style += "border: 1px solid #666666;\n";
                style += "background-color: #FFFFFF;\n";
                style += "}";
                style += ".HTB_table{\n";
                style += "border: 1px solid silver\n";
                style += "}";
                style += ".HTB_tableTD{\n";
                style += "border: 1px solid silver\n";
                style += "}";

                style += "</style>\n";

                Page.RegisterClientScriptBlock(KEY_STYLE, style);
            }

        }

        /// <summary>
        /// Register the initialization script of the editor.
        /// </summary>
        public void RegisterStartupScriptBlock()
        {
            // Get the startup script from the resources.
            //string startupScript = EditorHelper.GetResource("ActiveUp.WebControls.HtmlTextBox.HtmlTextBox_Startup.js");
            string startupScript = EditorHelper.GetResource("ActiveUp.WebControls._resources.HtmlTextBox_Startup.js");

            // Specify the max length allowed in the editor (approximative due to automatic reformat).
            startupScript = startupScript.Replace("$MAX_LENGTH$", this.MaxLength.ToString());

            // Sets the css file.
            startupScript = startupScript.Replace("$CONTENTCSSFILE$", this.ContentCssFile);

            // Specify the hack protection.
            startupScript = startupScript.Replace("$HACK_PROTECTION$", (this.HackProtectionDisabled ? "0" : "1"));

            // Specify the clean on paste.
            startupScript = startupScript.Replace("$CLEAN_ON_PASTE$", (this.CleanOnPaste ? "1" : "0"));

            // Specify the auto-hide toolbars
            startupScript = startupScript.Replace("$AUTO_HIDE_TOOLBARS$", (this.AutoHideToolBars ? "1" : "0"));

            // Specify if the content is editable or not.
            startupScript = startupScript.Replace("$EDITABLE$", (this.Editable ? "editor.document.designMode=\"on\";" : string.Empty));

            // Specify if you want to insert a BR tag instead of a new paragraph
            startupScript = startupScript.Replace("$USE_BR$", (this.UseBR ? "1" : "0"));

            // Specify the icons directory
            startupScript = startupScript.Replace("$IMAGESDIR$", this.IconsDirectory);
            // Specify if you want to use the toolbar as popup
            startupScript = startupScript.Replace("$POPUP_TOOLBARS$", (_popupToolbars ? "1" : "0"));

            startupScript = startupScript.Replace("$CONTEXT_MENU_BACK_COLOR$", Utils.Color2Hex(ContextMenuStyle.BackColor));
            startupScript = startupScript.Replace("$CONTEXT_MENU_BACK_COLOR_ROLLOVER$", Utils.Color2Hex(ContextMenuStyle.BackColorRollOver));
            startupScript = startupScript.Replace("$CONTEXT_MENU_FORE_COLOR$", Utils.Color2Hex(ContextMenuStyle.ForeColor));
            startupScript = startupScript.Replace("$CONTEXT_MENU_FORE_COLOR_ROLLOVER$", Utils.Color2Hex(ContextMenuStyle.ForeColorRollOver));
            startupScript = startupScript.Replace("$CONTEXT_MENU_CSSCLASS$", ContextMenuStyle.CssClass);
            startupScript = startupScript.Replace("$CONTEXT_MENU_BORDER_COLOR$", Utils.Color2Hex(ContextMenuStyle.BorderColor));
            startupScript = startupScript.Replace("$CONTEXT_MENU_BORDER_STYLE$", ContextMenuStyle.BorderStyle.ToString());
            startupScript = startupScript.Replace("$CONTEXT_MENU_BORDER_WIDTH$", ContextMenuStyle.BorderWidth.Value.ToString());
            startupScript = startupScript.Replace("$CONTEXT_MENU_CUSTOM$", ContextMenuCustomItems.GetItemListToString('$', '|'));

            // Finish by ClientID & UniqueID
            Page.Trace.Write(this.ClientID);
            startupScript = startupScript.Replace("$CLIENT_ID$", this.ClientID);
            startupScript = startupScript.Replace("$UNIQUE_ID$", this.UniqueID);

            // The disabled keys
            string disabledKeys = string.Empty;
            foreach (KeyDisabledItem key in KeysDisabled)
                disabledKeys += ((int)key.Code).ToString() + ",";
            disabledKeys = disabledKeys.TrimEnd(',');
            startupScript = startupScript.Replace("$KEYS_DISABLED$", disabledKeys);

            // Specify if the editor must have the focus when the application start.
            startupScript = startupScript.Replace("$STARTUP_FOCUS$", (this.StartupFocus ? "1" : "0"));

            // Specify the startup script
            startupScript = startupScript.Replace("$STARTUPSCRIPT$", this.StartupScript);

            // Register the startup script on the ASPX page.
            Page.RegisterStartupScript(UniqueID + "_startup", startupScript);

            Page.RegisterArrayDeclaration(ClientID + "_textLabels", Labels.Texts.ConvertToStringToRegisterArray());

        }

        /// <summary>
        /// Do some work before rendering the control.
        /// </summary>
        /// <param name="e">Event Args</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _toolbarsContainer.ID = "_container";
            _toolbarsContainer.Position = Position.Relative;
            _toolbarsContainer.ToolbarsPosition = Position.Relative;
            _toolbarsContainer.Width = Unit.Parse("100%");
            _toolbarsContainer.BackColor = Color.FromArgb(0xE0, 0xE0, 0xE0);
            _toolbarsContainer.ScriptDirectory = this.ScriptDirectory;
            _toolbarsContainer.ImagesDirectory = this.IconsDirectory;
#if (!FX1_1)
            BackImage = Utils.ConvertToImageDir(IconsDirectory, BackImage, "back_l.gif", Page, this.GetType());
#endif
            _toolbarsContainer.BackImage = BackImage;
#if (LICENSE)
			//_toolbarsContainer.License = this.License;
#endif

            _toolLibrary.IconsDirectory = this.IconsDirectory;
            _toolSpellChecker.IconsDirectory = this.IconsDirectory;

            if (AutoCreateTools)
                EnsureToolsCreated();
            this.Controls.Add(_toolbarsContainer);

            if (_popupToolbars)
            {
                _popupMenu.ID = ClientID + "_ToolbarsPopup";
                if (this.StartupMode == EditorMode.Design)
                    _popupMenu.ShowedOnStart = true;
                _popupMenu.ContentDivId = ClientID + ":_container";
                _popupMenu.AutoContent = true;
                this.Controls.Add(_popupMenu);
            }
            if (LocalizationFile == string.Empty)
                LoadLocalizationSettings();
            else
                LoadLocalizationSettings(LocalizationFile);
        }

        /// <summary>
        /// Loads the localization settings.
        /// </summary>
        public void LoadLocalizationSettings()
        {
            LoadLocalizationSettings(string.Empty);
        }

        /// <summary>
        /// Loads the localization settings from the specified definition file.
        /// </summary>
        /// <param name="path">The full path to the file.</param>
        public void LoadLocalizationSettings(string path)
        {
            if (path != string.Empty)
            {
                if (!System.IO.Path.IsPathRooted(path))
                    path = Page.Server.MapPath(path);
                this.Labels = LocalizationHelper.Load(path);

            }
            else
                this.Labels = LocalizationHelper.LoadFromString(EditorHelper.GetResource("ActiveUp.WebControls.HtmlTextBox.DefaultLabels.xml"));

        }

        /// <summary>
        /// Apply the localization settings.
        /// </summary>
        /// <returns>True if success; False is failed.</returns>
        public bool ApplyLocalizationSettings()
        {
            ApplyLocalizationSettings(this.Labels);
            return true;
        }

        /// <summary>
        /// Apply the specified localization settings.
        /// </summary>
        /// <param name="ls">The LocalizationSettings object.</param>
        /// <returns>True if success; False is failed.</returns>
        public bool ApplyLocalizationSettings(LocalizationSettings ls)
        {
            if (!LocalizationSettingsApplied)
            {
                if (this.LocalizationFile != string.Empty)
                    LoadLocalizationSettings(this.LocalizationFile);

                LocalizationHelper.Apply(ls, this);

                LocalizationSettingsApplied = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Do some work before rendering the control.
        /// </summary>
        /// <param name="e">Event Args</param>
        protected override void OnPreRender(EventArgs e)
        {
            this.Page.Trace.Write(this.ID, "OnPreRender");

            base.OnPreRender(e);

            if (this.AutoDetectSsl && this.Page.Request.ServerVariables["HTTPS"].ToUpper() == "ON")
                this.EnableSsl = true;

            if (AutoCreateTools == false)
                EnsureToolsCreated();
            EnsureLocalizationSettingsApplied();

            EditorType type = DetermineEditorType();

            if (type == EditorType.DHTML)
            {
                Page.Trace.Write("Register API");
                this.RegisterAPIScriptBlock();
                this.RegisterStartupScriptBlock();
            }

            if (Page != null)
                Page.RegisterRequiresPostBack(this);
        }

        private void RenderEditor(HtmlTextWriter output)
        {
            var isNs6 = false;
            var browser = Page.Request.Browser;
            if(browser.Browser.IndexOf("IE", StringComparison.OrdinalIgnoreCase) == -1)
            {
                isNs6 = true;
            }

            output.AddAttribute(HtmlTextWriterAttribute.Id, "HTB_IconsPreloaded");
            output.AddAttribute(HtmlTextWriterAttribute.Value, string.Empty);
            output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            output.RenderBeginTag(HtmlTextWriterTag.Input);
            output.RenderEndTag();

            var type = DetermineEditorType();

            if(type == EditorType.DHTML)
            {
                RenderDhtml(output, isNs6);
            }
            else if(type == EditorType.TextArea)
            {
                RenderTextArea(output);
            }
        }

        private void RenderDhtml(HtmlTextWriter output, bool isNs6)
        {
            RenderDhtmlHeader(output, isNs6);

            foreach(Toolbar toolbar in _toolbarsContainer.Toolbars)
            {
                foreach(ToolBase tool in toolbar.Tools)
                {
                    tool.AllowRollOver = AllowRollOver;
                }
            }

            if(!_iconsIsPreloaded)
            {
                RenderDhtmlIconsNotPreloaded(output);
            }

            output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            output.AddAttribute(HtmlTextWriterAttribute.Value, _iconsIsPreloaded.ToString());
            output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_IconsIsPreloaded");
            output.AddAttribute(HtmlTextWriterAttribute.Name, $"{UniqueID}_IconsIsPreloaded");
            output.RenderBeginTag(HtmlTextWriterTag.Input);
            output.RenderEndTag();

            RenderChildren(output);

            output.RenderEndTag(); // TD
            output.RenderEndTag(); // TR

            RenderHtmlContent(output);

            if(EditorModeSelector != EditorModeSelectorType.None && Editable)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_bottomToolbar");
                output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
                if(MaxLength <= 0)
                {
                    output.AddAttribute(HtmlTextWriterAttribute.Align, "left");
                }

                output.RenderBeginTag(HtmlTextWriterTag.Td); // TD

                if(EditorModeSelector == EditorModeSelectorType.CheckBox)
                {
                    output.AddAttribute(HtmlTextWriterAttribute.Name, $"{UniqueID}_showHTML");
                    output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_showHTML");
                    output.AddAttribute("onclick", $"HTB_ToggleMode(\'{ClientID}\');");
                    if(StartupMode == EditorMode.Html)
                    {
                        output.AddAttribute(HtmlTextWriterAttribute.Checked, string.Empty);
                    }

                    output.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");

                    output.RenderBeginTag(HtmlTextWriterTag.Input); // INPUT
                    output.Write(EditorModeHtmlLabel);
                    output.RenderEndTag(); // INPUT
                }
                else if(EditorModeSelector == EditorModeSelectorType.Tabs)
                {
                    RenderEditorModeSelectorTabs(output);
                }

                output.RenderEndTag();
                output.RenderEndTag();
            }

            if(MaxLength <= 0 && ShowNavigation)
            {
                RenderForNavigationTag(output);
            }

            RenderDhtmlFooter(output);
        }

        private void RenderDhtmlHeader(HtmlTextWriter output, bool isNs6)
        {
            if (IconsDirectory != null && !IconsDirectory.EndsWith("/", StringComparison.CurrentCulture))
            {
                IconsDirectory += "/";
            }

            if (!isNs6)
            {
                output.Write(
                    "<div  id=\"{0}_divUploadInDirectory\" style=\"z-index:99999999;position:absolute;left:700;top:400;display:'none'\">",
                    ClientID);
                hif.ID = $"{ClientID}_HTB_IU_uploadInDirectory";
                hif.Style["height"] = "24";
                hif.Style["width"] = "300";
                hif.RenderControl(output);
                output.Write("</div>");
            }

            var enumerator = Style.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var currentValue = (string)enumerator.Current;
                if (currentValue == null)
                {
                    throw new ArgumentNullException(nameof(currentValue));
                }

                output.AddStyleAttribute(currentValue, Style[currentValue]);
            }

            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                var currentValue = (string)enumerator.Current;
                if (currentValue == null)
                {
                    throw new ArgumentNullException(nameof(currentValue));
                }

                output.AddStyleAttribute(currentValue, Style[currentValue]);
            }

            ControlStyle.AddAttributesToRender(output);
            output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}__AREA__");

            if (!string.IsNullOrEmpty(ToolTip))
            {
                output.AddAttribute(HtmlTextWriterAttribute.Title, ToolTip);
            }

            output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE MAIN
            output.AddAttribute(HtmlTextWriterAttribute.Class, "HTMLToolbar");
            output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
            output.RenderBeginTag(HtmlTextWriterTag.Td); // TD

            output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            output.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
            output.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
            output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            output.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
            output.AddAttribute(HtmlTextWriterAttribute.Bgcolor, Utils.Color2Hex(BackColor));
            output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE ZONES
            output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
            output.RenderBeginTag(HtmlTextWriterTag.Td); // TD TOOLBAR
        }

        private void RenderDhtmlIconsNotPreloaded(HtmlTextWriter output)
        {
            var icons = string.Empty;
            foreach (Toolbar toolbar in _toolbarsContainer.Toolbars)
            {
                var builder = new StringBuilder();
                builder.Append(icons);
                foreach (ToolBase tool in toolbar.Tools)
                {
                    var button = tool as ToolButton;
                    if (button != null)
                    {
                        var icon = button.ImageURL;
                        if (icons.IndexOf(icon, StringComparison.Ordinal) == -1)
                        {
                            builder.Append($"{icon},");
                        }

                        icon = button.OverImageURL;
                        if (icons.IndexOf(icon, StringComparison.Ordinal) == -1)
                        {
                            builder.Append($"{icon},");
                        }
                    }
                }

                icons = builder.ToString();
            }

            icons = icons.TrimEnd(',');

            output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            output.AddAttribute(HtmlTextWriterAttribute.Value, icons);
            output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_Icons");
            output.AddAttribute(HtmlTextWriterAttribute.Name, $"{UniqueID}_Icons");
            output.RenderBeginTag(HtmlTextWriterTag.Input);
            output.RenderEndTag();

            output.Write("<script language=\"javascript\">\n");
            output.Write("function HTB_Preload_{0}(e)", ClientID);
            output.Write("{\n");
            output.Write("HTB_PreloadIcons('{0}');\n", ClientID);
            output.Write("}\n");
            output.Write("window.RegisterEvent(\"onload\", HTB_Preload_{0});\n", ClientID);
            output.Write("\n</script>\n");

            _iconsIsPreloaded = true;
        }

        private void RenderEditorModeSelectorTabs(HtmlTextWriter output)
        {
            if (MaxLength > 0)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Align, "left");
                output.RenderBeginTag(HtmlTextWriterTag.Table);
                output.RenderBeginTag(HtmlTextWriterTag.Tr);
                output.RenderBeginTag(HtmlTextWriterTag.Td);
            }

            output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "2");
            output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            output.AddAttribute("onselectstart", "return false");
            output.AddAttribute("ondragstart", "return false");
            output.AddStyleAttribute("cursor", "default");
            output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE
            output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
            output.AddAttribute(HtmlTextWriterAttribute.Align, "left");
            output.RenderBeginTag(HtmlTextWriterTag.Td); // TD

            output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
            output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE
            output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
            output.AddAttribute("onclick", $"HTB_SetDesignMode(\'{ClientID}\');");
            output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_design");
            output.RenderBeginTag(HtmlTextWriterTag.Td); // TD
            output.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "verdana");
            output.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "11px");
            output.RenderBeginTag(HtmlTextWriterTag.Span); // SPAN
            if (!TextMode)
            {
                output.Write("&nbsp;");
                output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                output.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
                output.AddAttribute(HtmlTextWriterAttribute.Src,
                    Utils.ConvertToImageDir(IconsDirectory == "/" ? IconsDirectory = string.Empty : IconsDirectory,
                        EditorModeDesignIcon, "tab_design.gif", Page, GetType()));
                output.RenderBeginTag(HtmlTextWriterTag.Img);
                output.RenderEndTag();
                output.Write("&nbsp;");
            }

            output.Write(EditorModeDesignLabel);
            output.Write("&nbsp;");
            output.RenderEndTag(); // SPAN
            output.RenderEndTag(); // TD
            output.RenderEndTag(); // TR
            output.RenderEndTag(); // TABLE

            output.RenderEndTag(); // TD
            output.RenderBeginTag(HtmlTextWriterTag.Td); // TD

            output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
            output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE
            output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
            output.AddAttribute("onclick", $"HTB_SetHtmlMode(\'{ClientID}\');");
            output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_html");
            output.RenderBeginTag(HtmlTextWriterTag.Td); // TD
            output.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "verdana");
            output.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "11px");
            output.RenderBeginTag(HtmlTextWriterTag.Span); // SPAN
            if (!TextMode)
            {
                output.Write("&nbsp;");
                output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                output.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
                output.AddAttribute(HtmlTextWriterAttribute.Src,
                    Utils.ConvertToImageDir(IconsDirectory == "/" ? IconsDirectory = string.Empty : IconsDirectory,
                        EditorModeHtmlIcon, "tab_html.gif", Page, GetType()));
                output.RenderBeginTag(HtmlTextWriterTag.Img);
                output.RenderEndTag();
                output.Write("&nbsp;");
            }

            output.Write(EditorModeHtmlLabel);
            output.Write("&nbsp;");
            output.RenderEndTag(); // SPAN
            output.RenderEndTag(); // TD
            output.RenderEndTag(); // TR
            output.RenderEndTag(); // TABLE
            output.RenderEndTag(); // TD
            //---------- PREVIEW
            output.RenderBeginTag(HtmlTextWriterTag.Td); // TD

            output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
            output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE
            output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
            output.AddAttribute("onclick", $"HTB_SetPreviewMode(\'{ClientID}\');");
            output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_preview");
            output.RenderBeginTag(HtmlTextWriterTag.Td); // TD
            output.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "verdana");
            output.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "11px");
            output.RenderBeginTag(HtmlTextWriterTag.Span); // SPAN
            if (!TextMode)
            {
                output.Write("&nbsp;");
                output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                output.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
                output.AddAttribute(HtmlTextWriterAttribute.Src,
                    Utils.ConvertToImageDir(IconsDirectory == "/" ? IconsDirectory = string.Empty : IconsDirectory,
                        EditorModePreviewIcon, "tab_preview.gif", Page, GetType()));
                output.RenderBeginTag(HtmlTextWriterTag.Img);
                output.RenderEndTag();
                output.Write("&nbsp;");
            }

            output.Write(EditorModePreviewLabel);
            output.Write("&nbsp;");
            output.RenderEndTag(); // SPAN 
            output.RenderEndTag(); // TD
            output.RenderEndTag(); // TR
            output.RenderEndTag(); // TABLE
            output.RenderEndTag(); // TD
            //------------------------------

            output.RenderEndTag(); // TR
            output.RenderEndTag(); // TABLE
        }

        private void RenderHtmlContent(HtmlTextWriter output)
        {
            output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
            output.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
            output.AddAttribute(HtmlTextWriterAttribute.Class, "HTMLContent");
            output.RenderBeginTag(HtmlTextWriterTag.Td); // TD CONTENT

            Attributes.Remove("style");
            Attributes.AddAttributes(output);

            output.AddAttribute(HtmlTextWriterAttribute.Name, $"{ClientID}_Editor");
            output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_Editor");
            output.AddAttribute(HtmlTextWriterAttribute.Tabindex, TabIndex.ToString());

            if(EnableSsl)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Src, "blank.htm");
            }

            if(AllowTransparency)
            {
                output.AddAttribute("allowtransparency", "true");
            }

            output.AddAttribute(HtmlTextWriterAttribute.Width, EditorAreaWidth.ToString());
            output.AddAttribute(HtmlTextWriterAttribute.Height, "100%");
            output.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
            output.AddStyleAttribute(HtmlTextWriterStyle.Width, EditorAreaWidth.ToString());
            output.AddStyleAttribute("border", "solid 1px silver");
            output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, Utils.Color2Hex(Color.White));
            output.AddAttribute("onbeforedeactivate", $"HTB_SaveData(\'{ClientID}\');");
            if(OnFocus != string.Empty)
            {
                output.AddAttribute("onfocus", OnFocus);
            }

            if(OnBlur != string.Empty)
            {
                output.AddAttribute("onblur", OnBlur);
            }

            output.AddAttribute("oncontextmenu", $"return HTB_ShowContextMenu(\'{ClientID}\');");
            output.RenderBeginTag(HtmlTextWriterTag.Iframe); // IFRAME
            output.RenderEndTag(); // IFRAME

            output.RenderEndTag(); // TD
            output.RenderEndTag(); // TR
        }

        private void RenderDhtmlFooter(HtmlTextWriter output)
        {
            output.RenderEndTag(); // TABLE ZONES

            // COUNTER
            RenderEditorCounter(output);
            // END CONTER

            output.RenderEndTag(); // TD
            output.RenderEndTag(); // TR

            // Render the license information

            output.RenderEndTag(); // TABLE MAIN

            // Render the text container
            output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            output.AddAttribute(HtmlTextWriterAttribute.Value, EditorHelper.Encode(Text));
            output.RenderBeginTag(HtmlTextWriterTag.Input); // INPUT
            output.RenderEndTag(); // INPUT

            output.AddAttribute(HtmlTextWriterAttribute.Name, $"{UniqueID}_HardReturnEnabled");
            output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_HardReturnEnabled");
            output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            output.AddAttribute(HtmlTextWriterAttribute.Value, HardReturnEnabled.ToString());
            output.RenderBeginTag(HtmlTextWriterTag.Input); // INPUT
            output.RenderEndTag(); // INPUT

            output.AddAttribute(HtmlTextWriterAttribute.Name, $"{UniqueID}_Mode");
            output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_Mode");
            output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            output.AddAttribute(HtmlTextWriterAttribute.Value, !Editable ? "PREVIEW" : StartupMode.ToString());

            output.RenderBeginTag(HtmlTextWriterTag.Input); // INPUT
            output.RenderEndTag(); // INPUT
        }

        private void RenderTextArea(HtmlTextWriter output)
        {
            if(TextareaColumns != string.Empty)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Cols, _textareaCols);
            }

            if(TextareaColumns != string.Empty)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Rows, _textareaRows);
            }

            ControlStyle.AddAttributesToRender(output);
            if(TextareaCssClass != string.Empty)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Class, _textareaCssClass);
            }

            output.AddAttribute(HtmlTextWriterAttribute.Tabindex, TabIndex.ToString());
            output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            output.RenderBeginTag(HtmlTextWriterTag.Textarea); // Textarea Open
            output.Write(EditorHelper.Encode(Text));
            output.RenderEndTag(); // Textarea Close
        }

        private void RenderEditorCounter(HtmlTextWriter output)
        {
            if(MaxLength > 0)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Align, "right");
                output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE 1
                output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR 1
                output.RenderBeginTag(HtmlTextWriterTag.Td); // TD 1
                output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE 2
                output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR 2
                output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_counter");
                output.RenderBeginTag(HtmlTextWriterTag.Td); // TD 2
                output.AddStyleAttribute("font-family", "verdana");
                output.AddStyleAttribute("font-size", "11px");
                output.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
                output.RenderBeginTag(HtmlTextWriterTag.Span);
                output.Write("Counter&nbsp;&nbsp;");
                output.RenderEndTag();
                output.AddAttribute(HtmlTextWriterAttribute.Id, $"{ClientID}_counterText");
                var valCounter = MaxLength;
                if(Text != null)
                {
                    valCounter = Text.Length - MaxLength;
                }

                output.AddAttribute(HtmlTextWriterAttribute.Value, valCounter.ToString());
                output.AddStyleAttribute(HtmlTextWriterStyle.Width, "50px");
                output.AddStyleAttribute(HtmlTextWriterStyle.Height, "20px");
                output.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
                output.RenderBeginTag(HtmlTextWriterTag.Input);
                output.RenderEndTag();
                output.RenderEndTag(); // TD 2
                output.RenderEndTag(); // TR 2
                output.RenderEndTag(); // TABLE 2
                output.RenderEndTag(); // TD 1
                output.RenderEndTag(); // TR 1
                output.RenderEndTag(); // TABLE 1

                output.RenderEndTag();
                output.RenderEndTag();
                output.RenderEndTag();
            }
        }

        /// <summary>
        /// Renders for navigation tag.
        /// </summary>
        /// <param name="output">The output.</param>
        protected void RenderForNavigationTag(HtmlTextWriter output)
        {
            /*output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR
            output.RenderBeginTag(HtmlTextWriterTag.Td); // TD
            output.AddAttribute(HtmlTextWriterAttribute.Border,"0");
            output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
            output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0");
            output.AddAttribute(HtmlTextWriterAttribute.Id,ClientID + "_navTable");
            output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE
            output.AddAttribute(HtmlTextWriterAttribute.Id,ClientID + "_navTR");
            output.AddStyleAttribute("font-family","verdana");
            output.AddStyleAttribute("font-size","10px");
            output.RenderBeginTag(HtmlTextWriterTag.Tr);
            output.RenderBeginTag(HtmlTextWriterTag.Td);
            output.Write("No Navigation");
            output.RenderEndTag();
            output.RenderEndTag();
            output.RenderEndTag(); // TABLE
            output.RenderEndTag(); // TD
            output.RenderEndTag(); // TR*/
        }

        /// <summary> 
        /// Render this control to the output parameter specified.
        /// </summary>
        /// <param name="output">The HTML writer to write out to </param>
        protected override void Render(HtmlTextWriter output)
        {
#if (LICENSE)
			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.HTB, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

			_useCounter++;

			if (/*!licenseStatus.IsRegistered &&*/ Page != null && _useCounter == ActiveUp.WebControls.Common.StaticContainer.UsageCount)
			{
				_useCounter = 0;
				output.Write(ActiveUp.WebControls.Common.StaticContainer.TrialMessage);
			}
			else
			{
				this.RenderEditor(output);
			}
#else
            RenderEditor(output);
#endif
        }

        /// <summary>
        /// A LoadPostData method.
        /// </summary>
        /// <param name="postDataKey">PostDataKey.</param>
        /// <param name="postCollection">postCollection.</param>
        /// <returns>bool</returns>
        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            /*try
            {
                EditorMode presentStartupMode = this.StartupMode;
                EditorMode postedStartupMode = EditorMode.Design;
                switch (postCollection[postDataKey + "_Mode"].ToUpper())
                {
                    case "DESIGN": postedStartupMode = EditorMode.Design; break;
                    case "HTML": postedStartupMode = EditorMode.Html; break;
                    case "PREVIEW": postedStartupMode = EditorMode.Preview; break;
                }

                if (!presentStartupMode.Equals(postedStartupMode)) 
                    this.StartupMode = postedStartupMode;
            }
            catch
            {
            }*/

            _iconsIsPreloaded = bool.Parse(postCollection[postDataKey + "_IconsIsPreloaded"]);

            string presentValue = this.Text;
            string postedValue = postCollection[postDataKey];

            //Page.Trace.Warn("->Post",postDataKey + ":" + postedValue);	

            if (presentValue == null || !presentValue.Equals(postedValue))
            {
                if (this.HackProtectionDisabled)
                    this.Text = EditorHelper.Decode(postedValue);
                else
                    this.Text = EditorHelper.RemoveMaliciousCode(EditorHelper.Decode(postedValue));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Specifies if you want to display the HTML/Design selector as tabs, checkbox or none.
        /// </summary>
        /// <remarks>If you don't want to let the user swap between HTML mode and Design mode, set this property to <see cref="EditorModeSelectorType.None"/>. Don't forget to set the <see cref="EditorModeSelectorType"/> property to display the wanted edition mode.</remarks>
        [
            Bindable(false),
            Category("Appearance"),
            Description("Specifies if you want to display the HTML/Design selector as tabs, checkbox or none."),
            DefaultValue(typeof(EditorModeSelectorType), "Tabs")
        ]
        public EditorModeSelectorType EditorModeSelector
        {
            get
            {
                object editorModeSelector = ViewState["HTB_EditorModeSelector"];
                if (editorModeSelector == null)
                {
                    ViewState["HTB_EditorModeSelector"] = EditorModeSelectorType.Tabs;
                }
                return (EditorModeSelectorType)ViewState["HTB_EditorModeSelector"];
            }
            set
            {
                ViewState["HTB_EditorModeSelector"] = value;
            }
        }

        /*/// <summary>
        /// Specifies the default edition mode.
        /// </summary>
        /// <remarks>The default mode is <see cref="EditModeSelection.Design"/>.</remarks>
        [Bindable(false),
        Category("Appearance"),
        Description("Specifies the default edition mode.")]
        public EditorMode EditorModeDefaultSelection
        {
            get
            {
                object editorModeDefaultSelection = ViewState["HTB_EditorModeDefaultSelection"];
                if (editorModeDefaultSelection == null)
                {
                    ViewState["HTB_EditorModeDefaultSelection"] = EditorMode.Design;
                }
                return (EditorMode)ViewState["HTB_EditorModeDefaultSelection"];
            }
            set
            {
                ViewState["HTB_EditorModeDefaultSelection"] = value;
            }
        }*/

        /// <summary>
        /// Gets or sets the default edition mode
        /// </summary>
        /// <value>The default edition mode.</value>
        [
            Bindable(false),
            Category("Appearance"),
            Description("Specifies the default edition mode."),
            DefaultValue(typeof(EditorMode), "Design")
        ]
        public EditorMode StartupMode
        {
            get
            {
                return _startupMode;
            }
            set
            {
                _startupMode = value;
            }
        }

        /// <summary>
        /// Specifies the text to display with the HTML/Design mode checkbox or the HTML tab.
        /// </summary>
        /// <remarks>This is the text that will be displayed with the check box.
        /// You can include HTML, but be careful.</remarks>
        [
            Bindable(false),
            Category("Appearance"),
            Description("Specifies the text to display with the HTML/Design mode checkbox or the HTML tab."),
            DefaultValue("HTML")
        ]
        public string EditorModeHtmlLabel
        {
            get
            {
                object editorModeHtmlLabel = ViewState["HTB_EditModeHtmlLabel"];
                if (editorModeHtmlLabel == null)
                {
                    ViewState["HTB_EditModeHtmlLabel"] = "HTML";
                }
                return (string)ViewState["HTB_EditModeHtmlLabel"];

            }
            set
            {
                ViewState["HTB_EditModeHtmlLabel"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the editor preview label.
        /// </summary>
        /// <value>The editor mode preview label.</value>
        [
            Bindable(false),
            Category("Appearance"),
            Description("The editor preview label."),
            DefaultValue("Preview")
        ]
        public string EditorModePreviewLabel
        {
            get
            {
                object editorModePreviewLabel = ViewState["HTB_EditModePreviewLabel"];
                if (editorModePreviewLabel == null)
                {
                    ViewState["HTB_EditModePreviewLabel"] = "Preview";
                }
                return (string)ViewState["HTB_EditModePreviewLabel"];

            }
            set
            {
                ViewState["HTB_EditModePreviewLabel"] = value;
            }
        }

        /// <summary>
        /// Gets or sets if we will display in text mode.
        /// </summary>
        /// <remarks>If you don't want to use icons (images) to represent the tools, set the <c>TextMode</c> property to <c>true</c>. This will tell the control to render the tool as standard HTML buttons. The images specified in the tools will be ignored.</remarks>
        [
            Bindable(false),
            Category("Appearance"),
            Description("Get or set if we will display in text mode."),
            DefaultValue(false)
        ]
        public bool TextMode
        {
            get
            {
                object textMode = ViewState["HTB_TextMode"];
                if (textMode == null)
                {
                    ViewState["HTB_TextMode"] = false;
                }
                return (bool)ViewState["HTB_TextMode"];
            }
            set
            {
                ViewState["HTB_TextMode"] = value;
            }
        }

        /// <summary>
        /// Specifies the text to display with the Design mode tab.
        /// </summary>
        [
            Bindable(false),
            Category("Appearance"),
            Description("Specifies the text to display with the Design mode tab."),
            DefaultValue("Design")
        ]
        public string EditorModeDesignLabel
        {
            get
            {
                object editorModeDesignLabel = ViewState["HTB_EditorModeDesignLabel"];
                if (editorModeDesignLabel == null)
                {
                    ViewState["HTB_EditorModeDesignLabel"] = "Design";
                }
                return (string)ViewState["HTB_EditorModeDesignLabel"];
            }
            set
            {
                ViewState["HTB_EditorModeDesignLabel"] = value;
            }
        }

		/// <summary>
		/// Gets or sets the filename of the Design tab icon.
		/// </summary>
		[
			Bindable(false),
			Category("Appearance"),
			Description("Gets or sets the filename of the Design tab icon."),
			Fx1ConditionalDefaultValue("", EditorModeDesignIconFx1)
		]
		public string EditorModeDesignIcon
		{
			get
			{
				var defaultValue = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, EditorModeDesignIconFx1);
				return ViewStateHelper.GetFromViewState(ViewState, nameof(EditorModeDesignIcon), defaultValue);
			}
			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(EditorModeDesignIcon), value);
			}
		}

        /// <summary>
        /// Gets or sets the filename of the Html tab icon.
        /// </summary>
        [
            Bindable(false),
            Category("Appearance"),
            Description("Gets or sets the filename of the Design tab icon."),
#if (!FX1_1)
 DefaultValue("")
#else
			DefaultValue("tab_html.gif")
#endif
]
        public string EditorModeHtmlIcon
        {
            get
            {
                object editorModeHtmlIcon = ViewState["HTB_EditorModeHtmlIcon"];
                if (editorModeHtmlIcon == null)
                {
#if (!FX1_1)
                    return string.Empty;
#else
					ViewState["HTB_EditorModeHtmlIcon"] = "tab_html.gif";
#endif
                }
                return (string)ViewState["HTB_EditorModeHtmlIcon"];
            }
            set
            {
                ViewState["HTB_EditorModeHtmlIcon"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the filename of the preview tab icon.
        /// </summary>
        [
            Bindable(false),
            Category("Appearance"),
            Description("Gets or sets the filename of the preview tab icon."),
#if (!FX1_1)
 DefaultValue("")
#else
			DefaultValue("tab_preview.gif")
#endif
]
        public string EditorModePreviewIcon
        {
            get
            {
                object editorModePreviewIcon = ViewState["HTB_EditorModePreviewIcon"];
                if (editorModePreviewIcon == null)
                {
#if (!FX1_1)
                    return string.Empty;
#else
					ViewState["HTB_EditorModePreviewIcon"] = "tab_preview.gif";
#endif
                }
                return (string)ViewState["HTB_EditorModePreviewIcon"];
            }
            set
            {
                ViewState["HTB_EditorModePreviewIcon"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the template or layout to use for the toolbars.
        /// </summary>
        /// <remarks>
        /// This method is commonly used to quickly set the tools you need. This is the fastest method to set the template that will be used by the control to automaticelly generate his toolbars and tools collections.<br />
        /// If you set the <see cref="AutoCreateTools"/> property to true, this property will be used, even if you add tools later programmaticaly.<br />
        /// If you want to customize the tools, you can access them by their name or index trough the <c>Tools</c> collection of the Toolbars contained in the <see cref="Toolbars"/> collection. In this case, don't forget to call <see cref="EnsureToolsCreated"/> before to ensure that the tools are created.<br />
        /// If you want to create customized tools, it's possible to add yourself the Tools.Custom object in the <see cref="Toolbar"/> objects contained by the control. You can also develop your own tool objects that require postback handling and more. To achieve this, use ToolBase.
        /// </remarks>
        /// <example>
        ///	<code>
        ///	// The following example is to use in the designer.
        ///	
        ///	// A minimal toolbar
        ///	
        ///	&lt;AU:Editor ID="Editor" Runat="server" Template="bold,italic,underline"&gt;
        ///	
        ///	// The default toolbars
        ///	
        ///	&lt;AU:Editor ID="EDITBOX" Runat="server" Template="cut,copy,paste,separator,bold,
        ///	italic,underline,separator,link,image,rule,separator,alignleft,aligncenter,alignright,
        ///	separator,orderedlist,unorderedlist,separator,indent,outdent,separator;paragraph,fontface,
        ///	fontsize,fontcolor,separator"&gt;
        ///	
        /// // The following example is to use in the code file of the web form
        /// 
        /// // A minimal toolbar
        /// Editor1.Template = "bold,italic,underline";
        /// 
        /// // The default toolbars
        /// Editor1.Template = "cut,copy,paste,separator,bold,italic,underline,separator,
        ///		link,image,rule,separator,alignleft,aligncenter,alignright,separator,
        ///		orderedlist,unorderedlist,separator,indent,outdent,separator;paragraph,fontface,
        ///		fontsize,fontcolor,separator";
        ///	
        ///	
        ///	
        /// </code>
        /// </example>
        [
            Bindable(false),
            Category("Appearance"),
            Description("Gets or sets the template or layout to use for the toolbars."),
            DefaultValue("bold,italic,underline,separator,cut,copy,paste,separator,paragraph,fontface,fontsize,fontcolor,highlight;alignleft,aligncenter,alignright,alignjustify,separator,orderedlist,unorderedlist,separator,indent,outdent,separator,print,separator,subscript,superscript,strikethrough,separator,find,replace,image,link,specialchars,codecleaner,table,flash")
        ]
        public string Template
        {
            get
            {
                if (ViewState["_template"] == null)
                {
                    string template = "bold,italic,underline,separator,cut,copy,paste,separator,paragraph,fontface,fontsize,fontcolor,highlight;alignleft,aligncenter,alignright,alignjustify,separator,orderedlist,unorderedlist,separator,indent,outdent,separator,print,separator,subscript,superscript,strikethrough,separator,find,replace,image,link,specialchars,codecleaner,table,flash";
                    ViewState["_template"] = template;
                    return template;
                }

                return (string)ViewState["_template"];
            }

            set
            {
                ViewState["_template"] = value;
            }
        }


        /// <summary>
        /// A RaisePostDataChangedEvent.
        /// </summary>
        public virtual void RaisePostDataChangedEvent()
        {
            OnTextChanged(EventArgs.Empty);
        }

        /// <summary>
        /// The TextChanged event handler.
        /// </summary>
        public event EventHandler TextChanged;

        /// <summary>
        /// A OnTextChanged event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTextChanged(EventArgs e)
        {
            if (TextChanged != null)
                TextChanged(this, e);
        }

        /// <summary>
        /// Determine if the control has created the tools based on the Template property. If not, it will create them.
        /// </summary>
        public bool EnsureToolsCreated()
        {
            if (!this.ToolsCreated)
            {
                return this.CreateTools();
            }
            else
                return false;
        }

        /// <summary>
        /// Creates the tools contained in the <see cref="Template"/> property.
        /// </summary>
        /// <remarks>This method can be called if you change the layout of <see cref="Template"/> property after the OnPreRender event of the <see cref="Editor"/> object was called.</remarks>
        public bool CreateTools()
        {
            return this.CreateTools(this.Template, true);
        }

        /// <summary>
        /// Creates the tools contained in the <see cref="Template"/> property.
        /// </summary>
        /// <remarks>This method can be called if you change the layout of <see cref="Template"/> property after the OnPreRender event of the <see cref="Editor"/> object was called.</remarks>
        public bool CreateTools(string template, bool doUpdate)
        {
            if (/*AutoCreateTools &&*/ template.TrimEnd().Length > 0)
            {
                _toolbarsContainer.Toolbars.Clear();
                _toolbarsContainer.EnableSsl = this.EnableSsl;

                string label = string.Empty;

                string[] toolbarShemes = template.Split(';');

                for (int i = 0; i < toolbarShemes.Length; i++)
                {
                    Toolbar toolbar = new Toolbar();
                    toolbar.ID = "_toolbar" + i.ToString();
                    toolbar.CellSpacing = 1;
                    toolbar.Dragable = false;
                    toolbar.BorderStyle = BorderStyle.NotSet;
                    toolbar.Style["Z-INDEX"] = (toolbarShemes.Length - i).ToString();
                    toolbar.ImagesDirectory = this.IconsDirectory;
                    toolbar.ScriptDirectory = this.ScriptDirectory;
                    toolbar.BackColor = Color.Empty;

                    _toolbarsContainer.Toolbars.Add(toolbar);

                    foreach (string tool in toolbarShemes[i].Split(','))
                    {
                        switch (tool.ToLower().Trim())
                        {
                            case "bold": toolbar.Tools.Add(new ToolBold()); break;
                            case "italic": toolbar.Tools.Add(new ToolItalic()); break;
                            case "underline": toolbar.Tools.Add(new ToolUnderline()); break;
                            case "cut": toolbar.Tools.Add(new ToolCut()); break;
                            case "copy": toolbar.Tools.Add(new ToolCopy()); break;
                            case "paste": toolbar.Tools.Add(new ToolPaste()); break;
                            case "alignleft": toolbar.Tools.Add(new ToolAlignLeft()); break;
                            case "aligncenter": toolbar.Tools.Add(new ToolAlignCenter()); break;
                            case "alignright": toolbar.Tools.Add(new ToolAlignRight()); break;
                            case "alignjustify": toolbar.Tools.Add(new ToolAlignJustify()); break;
                            case "separator": toolbar.Tools.Add(new ToolSeparator()); break;
                            case "indent": toolbar.Tools.Add(new ToolIndent()); break;
                            case "outdent": toolbar.Tools.Add(new ToolOutdent()); break;
                            case "rule": toolbar.Tools.Add(new ToolRule()); break;
                            case "orderedlist": toolbar.Tools.Add(new ToolOrderedList()); break;
                            case "unorderedlist": toolbar.Tools.Add(new ToolUnorderedList()); break;
                            case "print": toolbar.Tools.Add(new ToolPrint()); break;
                            case "strikethrough": toolbar.Tools.Add(new ToolStrikeThrough()); break;
                            case "subscript": toolbar.Tools.Add(new ToolSubscript()); break;
                            case "superscript": toolbar.Tools.Add(new ToolSuperscript()); break;
                            case "highlight": toolbar.Tools.Add(new ToolHighlight()); break;
                            case "link": toolbar.Tools.Add(new ToolLink()); break;
                            case "image": toolbar.Tools.Add(new ToolImage()); break;
                            case "fontcolor": toolbar.Tools.Add(new ToolFontColor()); break;
                            case "fontface": toolbar.Tools.Add(new ToolFontFace()); break;
                            case "fontsize": toolbar.Tools.Add(new ToolFontSize()); break;
                            case "paragraph": toolbar.Tools.Add(new ToolParagraph()); break;
                            case "custom": toolbar.Tools.Add(new ToolCustomLinks()); break;
                            case "specialchars": toolbar.Tools.Add(new ToolSpecialChars()); break;
                            case "table": toolbar.Tools.Add(new ToolTable()); break;
                            case "codecleaner": toolbar.Tools.Add(new ToolCodeCleaner()); break;
                            case "customlinks": toolbar.Tools.Add(new ToolCustomLinks()); break;
                            case "find": toolbar.Tools.Add(new ToolFind()); break;
                            case "replace": toolbar.Tools.Add(new ToolReplace()); break;
                            case "library": toolbar.Tools.Add(_toolLibrary); break;
                            case "flash": toolbar.Tools.Add(new ToolFlash()); break;
                            case "customtags": toolbar.Tools.Add(new ToolCustomTags()); break;
                            case "codesnippets": toolbar.Tools.Add(new ToolCodeSnippets()); break;
                            case "multicodesnippets": toolbar.Tools.Add(new ToolMultiCodeSnippets()); break;
                            case "iespellchecker": toolbar.Tools.Add(new ToolIeSpellCheck()); break;
                            case "spellchecker": toolbar.Tools.Add(_toolSpellChecker); break;
                            case "templatemanager": toolbar.Tools.Add(_toolTemplateManager); break;
                            case "selectall": toolbar.Tools.Add(new ToolSelectAll()); break;
                            case "pagebreak": break;
                            case "": break;
                            default: throw new Exception(tool + " is not a valid tool. Check the spelling.");// break;					
                        }
                    }
                }

                if (doUpdate)
                    this.ToolsCreated = true;
                return true;
            }
            else
            {
                if (doUpdate)
                    this.ToolsCreated = true;
                return false;
            }
        }

        /// <summary>
        /// Gets the value indicating whether the control had created it's toolbar and tools collections based on the Template property layout.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ToolsCreated
        {
            get
            {
                //if (ViewState["_toolsCreated"] == null)
                //	ViewState["_toolsCreated"] = false;
                //return (bool)ViewState["_toolsCreated"];
                return _toolsCreated;
            }
            set
            {
                //ViewState["_toolsCreated"] = value;
                _toolsCreated = value;
            }
        }

        /// <summary>
        /// When set to false, the editor rendering engine will ignore the content of the <see cref="Template"/> property.
        /// </summary>
        [
            Bindable(false),
            Category("Behavior"),
            Description("When set to false, the editor rendering engine will ignore the content of the property."),
            DefaultValue(true)
        ]
        public bool AutoCreateTools
        {
            get
            {
                return _autoCreateTools;
            }
            set
            {
                _autoCreateTools = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating The toolbars is displayed as popup when the property is set to true.
        /// </summary>
        [
            Bindable(false),
            Browsable(true),
            Category("Behavior"),
            Description("The toolbars is displayed as popup when the property is set to true."),
            DefaultValue(false)
        ]
        public bool PopupToolbars
        {
            get
            {
                return _popupToolbars;
            }

            set
            {
                _popupToolbars = value;
            }
        }

        /// <summary>
        /// Specify whether you want the editor to be editable or act as a document viewer.
        /// </summary>
        [Bindable(false),
        Category("Behavior"),
        Description("Specify whether you want the editor to be editable or act as a document viewer."),
        DefaultValue(true)
        ]
        public bool Editable
        {
            get
            {
                return _editable;
            }
            set
            {
                _editable = value;
            }
        }


        /// <summary>
        /// Returns an <see cref="ArrayList" /> containing the filenames of all the icons in all toolbars.
        /// </summary>
        /// <remarks>This method is used internaly to generate the client side preloads script.</remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ArrayList IconsFilenames
        {
            get
            {
                ArrayList icons = new ArrayList();

                foreach (Toolbar toolbar in _toolbarsContainer.Toolbars)
                {
                    foreach (ToolBase tool in toolbar.Tools)
                    {
                        if (tool is ToolButton)
                        {
                            if (((ToolButton)tool).ImageURL.Length > 0 && !icons.Contains(((ToolButton)tool).ImageURL))
                                icons.Add(((ToolButton)tool).ImageURL.Length > 0 && !icons.Contains(((ToolButton)tool).OverImageURL));
                            if (((ToolButton)tool).OverImageURL.Length > 0)
                                icons.Add(((ToolButton)tool).OverImageURL);
                        }

                        else if (tool is ToolSpacer)
                        {
                            if (((ToolSpacer)tool).ImageURL.Length > 0 && !icons.Contains(((ToolSpacer)tool).ImageURL))
                                icons.Add(((ToolSpacer)tool).ImageURL);
                        }
                    }
                }

                return icons;
            }

        }

        /// <summary>
        /// Gets and sets max characters allowed in the editor.
        /// </summary>
        /// <remarks>The default value is 0 and mean <b>no limit</b>.<br /><br />
        /// You need to know that the max lenght is approximative. It's due to the fact that when the content is typed or pasted, the client-side function will crop the code, then the IFRAME object will try to reformat the HTML code to ensure that the output is valid. I all cases, this will add closing tags, so depending of the size of your document, the max length can be very approximative.</remarks>
        [
            Bindable(false),
            Category("Appearance"),
            Description("Gets and sets max character allowed in the editor."),
            DefaultValue(0)
        ]
        public int MaxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                _maxLength = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether you want to see the navigation tag.
        /// </summary>
        /// <value><c>true</c> if you want to see the navigation tag; otherwise, <c>false</c>.</value>
        [
        Bindable(false),
        Category("Appearance"),
        Description("Gets and sets the value indicates if you want to see the navigation tag."),
        DefaultValue(false)
        ]
        private bool ShowNavigation
        {
            get
            {
                return _showNavigation;
            }
            set
            {
                _showNavigation = value;
            }
        }



        /// <summary>
        /// Specify whether you want the editor to clean the document on each paste.
        /// </summary>
        [
            Bindable(false),
            Category("Behavior"),
            Description("Specify whether you want the editor to clean the document on each paste."),
            DefaultValue(false)
        ]
        public bool CleanOnPaste
        {
            get
            {
                return _cleanOnPaste;
            }
            set
            {
                _cleanOnPaste = value;
            }
        }

        /// <summary>
        /// Specify whether you want the editor auto-hide the toolbars.
        /// </summary>
        [
        Bindable(false),
        Category("Behavior"),
        Description("Specify whether you want the editor auto-hide the toolbars."),
        DefaultValue(false)
        ]
        public bool AutoHideToolBars
        {
            get
            {
                return _autoHideToolbars;
            }
            set
            {
                _autoHideToolbars = value;
            }
        }

        /// <summary>
        /// Specify whether you want the editor to insert a BR tag instead of a new paragraph (P) when the user hit RETURN.
        /// </summary>
        /// <remarks>Inserting new paragraphs on RETURN can be usefull. In this case, just tell your end-users to press SHIFT-ENTER to add a BR tard instead of a P. When in this mode, you can insert an hard return by pressing CTRL-M. You can also change the behavior while editing by pressing CTRL-K.</remarks>
        [
            Bindable(false),
            Category("Behavior"),
            Description("Specify whether you want the editor to insert a BR tag instead of a new paragraph when the user hit RETURN."),
            DefaultValue(true)
        ]
        public bool UseBR
        {
            get
            {
                return _useBR;
            }
            set
            {
                _useBR = value;
            }
        }

        /// <summary>
        /// Specifies if you want to enable the Editor IFRAME transparency.
        /// </summary>
        [Bindable(false),
        Category("Appearance"),
        DefaultValue(false),
        Description("Specifies if you want to enable the Editor IFRAME transparency.")]
        public bool AllowTransparency
        {
            get
            {
                return _allowTransparency;
            }
            set
            {
                _allowTransparency = value;
            }
        }

        /// <summary>
        /// Gets or sets the background color of the <see cref="Editor"/>.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(typeof(Color), "#DBD8D1"),
        NotifyParentProperty(true),
        Description("Background color of the Editor.")
        ]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        /// <summary>
        /// Specify whether you want the control to keep the data in the viewsate.
        /// </summary>
        /// <remarks>This property can be useful when using the control with a Tab Control like the IE Tab WebControls. When your are not on the tab that contain the editor, the content can be lost because the editor use an INPUT HIDDEN field to save the content of the editor.</remarks>
        [
            Bindable(false),
            Category("Data"),
            Description("Specify whether you want the control to keep the data in the viewsate."),
            DefaultValue(false)
        ]
        public bool PersistText
        {
            get
            {
                return _persistText;
            }
            set
            {
                _persistText = value;
            }
        }

        /// <summary>
        /// Specify whether you want the editor to auto detect the SSL environment.
        /// </summary>
        [
            Bindable(false),
            Category("Behavior"),
            Description("Specify whether you want the editor to auto detect the SSL environment."),
            DefaultValue(true)
        ]
        public bool AutoDetectSsl { get; set; } = true;

        /// <summary>
        /// Set to true if you need to use the control in a secure web page.
        /// </summary>
        [Bindable(false),
        Category("Behavior"),
        DefaultValue(false),
        Description("Set it to true if you need to use the control in a secure web page.")]
        public new bool EnableSsl { get; set; }

        /// <summary>
        /// Gets or sets the javascript code to execute after the initialization of the editor.
        /// </summary>
        [Bindable(false),
        Category("Behavior"),
        Description("Gets or sets the javascript code to execute after the initialization of the editor.")]
        public string StartupScript
        {
            get
            {
                return _startupScript;
            }
            set
            {
                _startupScript = value;
            }
        }

        /// <summary>
        /// Specify wether you want the editor to ignore key formatting commands
        /// </summary>
        /// <value>The keys disabled.</value>
        [
            Bindable(true),
            Browsable(true),
            Category("Behavior"),
            Description("Specify whether you want the editor to ignore key formatting commands."),
            DefaultValue(null),
            PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public KeyDisabledItemCollection KeysDisabled
        {
            get
            {
                return _keysDisabled;
            }
        }

        /// <summary>
        /// Specify whether you want to add custom commands for the context menu.
        /// </summary>
        [
        Bindable(true),
        Browsable(true),
        Category("Context Menu"),
        Description("Specify whether you want to add custom commands for the context menu."),
        DefaultValue(null),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public ContextMenuItemCollection ContextMenuCustomItems
        {
            get
            {
                return _contextMenuCustomItems;
            }
        }


        /// <summary>
        /// Gets or sets the max character mode allowed in the editor.
        /// </summary>
        /// <value>The max length.</value>
        [
        Bindable(false),
        Category("Appearance"),
        Description("Gets and sets max character mode allowed in the editor."),
        DefaultValue(typeof(EditorMaxLengthMode), "Editor")
        ]
        public EditorMaxLengthMode MaxLengthMode
        {
            get { return _maxLengthMode; }
            set { _maxLengthMode = value; }

        }

        /// <summary>
        /// Gets or sets a value indicating whether the editor must have the focus when the applciation start.
        /// </summary>
        /// <value><c>true</c> if the editor must have the focus when the application starts; otherwise, <c>false</c>.</value>
        [
        Bindable(false),
        Category("Behavior"),
        Description("Specify if the editor must have the focus when the applciation start."),
        DefaultValue(false)
        ]
        public bool StartupFocus
        {
            get { return _startupFocus; }
            set { _startupFocus = value; }
        }

        /// <summary>
        /// Indicates if you want to allow rollover.
        /// </summary>
        [
        Bindable(true),
        Browsable(true),
        Category("Behavior"),
        DefaultValue(true),
        Description("Allow rollover."),
        NotifyParentProperty(true)
        ]
        public bool AllowRollOver
        {
            get
            {
                object allowRollOver = ViewState["_allowRollOver"];
                if (allowRollOver != null)
                    return (bool)allowRollOver;
                else
                    return true;
            }

            set
            {
                ViewState["_allowRollOver"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the bottom toolbar.
        /// </summary>
        /// <value>The bottom toolbar.</value>
        [
            Bindable(true),
            Browsable(false),
            Description("Bottom toolbar.")
        ]
        public Toolbar BottomToolbar
        {
            get
            {
                object bottomToolbar = ViewState["_bottomToolbar"];
                if (bottomToolbar != null)
                    return new Toolbar();
                else
                    return (Toolbar)bottomToolbar;
            }

            set
            {
                ViewState["_bottomToolbar"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the file name of the style sheet file to attach to the edit box (preview styles).
        /// </summary>
        [Bindable(false),
        Category("Appearance"),
        Description("The file name of the style sheet file to attach to the edit box (preview styles).")]
        public string ContentCssFile
        {
            get
            {
                return _contentCssFile;
            }
            set
            {
                _contentCssFile = value;
            }
        }

        /// <summary>
        /// Gets or sets the file with the localized labels.
        /// </summary>
        [Bindable(false),
        Category("Appearance"),
        Description("Gets or sets the file with the localized labels.")]
        public string LocalizationFile
        {
            get
            {
                return _localizationFile;
            }
            set
            {
                _localizationFile = value;
            }
        }

        /// <summary>
        /// Gets or sets the Css class to use for the Textarea.
        /// </summary>
        /// <remarks>If you want to customize the appearance of the <c>Textarea</c> displayed when the client browser is not compatible, use this property to specify a Css Class.</remarks>
        [Bindable(false),
        Category("Appearance"),
        Description("Get or set the Css class to use for the Textarea.")]
        public string TextareaCssClass
        {
            get
            {
                return _textareaCssClass;
            }
            set
            {
                _textareaCssClass = value;
            }
        }

        /// <summary>
        /// Gets or sets the context menu style.
        /// </summary>
        /// <value>The context menu style.</value>
        [
            Bindable(false),
            Browsable(true),
            Category("Context Menu"),
            Description("The context menu style."),
            PersistenceModeAttribute(PersistenceMode.InnerProperty),
            DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content),
            DefaultValueAttribute(null),
            NotifyParentPropertyAttribute(true)
        ]
        public ContextMenuStyle ContextMenuStyle
        {
            get
            {
                return _contextMenuStyle;
            }
            set
            {
                _contextMenuStyle = value;
            }

        }

        /// <summary>
        /// Returns the HTML stripped text.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TextStripped
        {
            get
            {
                string newText = this.Text.Replace("</p>", "\n\n").Replace("</P>", "\n\n").Replace("<br>", "\n").Replace("<BR>", "\n").Replace("&nbsp;", " ");
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("<[^>]*>");
                newText = regex.Replace(newText, "");
                return newText;
            }
        }

        /// <summary>
        /// Returns the text content HTML encoded.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TextEncoded
        {
            get
            {
                string text = HtmlEncoder.Encode(this.Text);
                return text;
            }
        }

        /// <summary>
        /// Returns the text content in XHTML encoded.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TextXhtml
        {
            get
            {

                HtmlDocument htmlDocument = HtmlDocument.Create(HtmlEncoder.Encode(this.Text), false);
                return htmlDocument.XHTML;
            }
        }

        /// <summary>
        /// Gets or sets the labels of the Editor.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LocalizationSettings Labels
        {
            get
            {
                if (_labels == null)
                    _labels = new LocalizationSettings();
                return _labels;
            }
            set
            {
                _labels = value;
            }
        }

        /// <summary>
        /// Gets the value indicating whether the control had applied the localization settings.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool LocalizationSettingsApplied
        {
            get
            {
                return _localizationSettingsApplied;
            }
            set
            {
                _localizationSettingsApplied = value;
            }
        }

        /// <summary>
        /// Determine if the control has applied its localization settings. If not, it will apply them.
        /// </summary>
        public bool EnsureLocalizationSettingsApplied()
        {
            if (!this.LocalizationSettingsApplied)
            {
                return this.ApplyLocalizationSettings();
            }
            else
                return false;
        }

        #endregion
    }
}
