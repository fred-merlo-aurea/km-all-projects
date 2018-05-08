#if !FX1_1
#define NOT_FX1_1
# endif

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ImageEditor"/> object.
	/// </summary>
	[ToolboxData("<{0}:ImageEditor runat=server></{0}:ImageEditor>")]
	[CLSCompliantAttribute(true)] 
	[ComVisibleAttribute(true)]
    [ToolboxBitmap(typeof(ImageEditor), "ToolBoxBitmap.Image.bmp")]
	//[Designer(typeof(EditorDesigner))]
	//[ParseChildrenAttribute(false)]
	//[PersistChildren(true)]
	//[ParseChildren(true)]
	//[Editor(typeof(EditorComponentEditor), typeof(ComponentEditor))]
	[Serializable]
	public class ImageEditor : CoreWebControl, INamingContainer, IPostBackDataHandler, IPostBackEventHandler
	{
		// Consts
		private static string CLIENTSIDE_API;
		private const string SCRIPTKEY = "ActiveImage";
		private const string ResourceName = "ActiveUp.WebControls._resources.Images.spacer.gif";
		private const string Spacer = "spacer.gif";
		private const string EditOff = "edit_off.gif";
		private const string DeleteOff = "delete_off.gif";
		private const string EditOver = "edit_over.gif";
		private const string DeleteOver = "delete_over.gif";
		private const string OnMouseOver = "onmouseover";
		private const string AieShowToolbar = "AIE_showToolbar('{0}');";
		private const string AieHideToolbar = "AIE_hideToolbar('{0}');";
		private string _template, _externalScript, _scriptDirectory, _tempDirectory, _imagesDirectory,
			_tdCssClass, _imageViewHelpText/*, _license*/;
		private Toolbar _toolbar = new Toolbar();
		private Version _version = null;
		private bool _toolsCreated, _autoCreateTools, _directWrite, _allowUpload, _allowEdition,
			_allowDelete, _hideImageViewHelp;
		private Selection _selection;
		private int _imageWidth, _imageHeight;
		private System.Drawing.Size _imageSize;
		private ImageEditorMode _editorMode, _editorModeAfterUpload;
		//private ImageSettings _uploadSettings, _saveSettings;

		internal static int indexTools = 0;

		/// <summary>
		/// Gets or sets the value indicates if you want to use the session to generated the filename.
		/// </summary>
		[
			DefaultValue(true),
			Description("Value indicates if you want to use the session to generated the filename.")
		]
		public bool UseSession 
		{
			get
			{
				object useSession = ViewState["UseSession"];
				if (useSession == null)
					return true;

				return (bool)useSession;
			}	

			set
			{
				ViewState["UseSession"] = value;
			}
		}
		
		/// <summary>
		/// Event handler contains a file as argument.
		/// </summary>
		public delegate void FileEventHandler(object sender, FileEventArgs e);

		/// <summary>
		/// Save server event.
		/// </summary>
		public event FileEventHandler Save;

		/// <summary>
		/// Triggers the Save event.
		/// </summary>
		public virtual void OnSave(object sender,string fileName)
		{
			if (Save != null)
				Save(this, new FileEventArgs(fileName));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageEditor"/> class.
		/// </summary>
		public ImageEditor()
		{
			try  
			{
				System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
				System.Version v = asm.GetName().Version;
				_version = v;
			}
			catch
			{
				_version = new System.Version(3,0,0,0);
			} 
			ControlStyle.BorderColor = System.Drawing.Color.DarkGray;
			ControlStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#DBD8D1");
			ControlStyle.Height = Unit.Pixel(100);
			ControlStyle.Width = Unit.Pixel(300); 
			ControlStyle.BorderWidth = Unit.Pixel(1);
			this.Template = "save,crop,text,flip,mirror,rotate,zoom,imagesize,canvassize";
			_externalScript = string.Empty;
			AutoCreateTools = true;
			//this.ScriptDirectory = "/aspnet_client/ActiveWebControls/" + StaticContainer.VersionString + "/";
#if (!FX1_1)
            this.ScriptDirectory = string.Empty;
#else
			this.ScriptDirectory = Define.SCRIPT_DIRECTORY;
#endif
			this.ImagesDirectory = string.Empty;
			this.TempDirectory = string.Empty;
			this.DirectWrite = false;
			this.TdCssClass = "aieTD";
			this.EditorMode = ImageEditorMode.Edit;
			this.EditorModeAfterUpload = ImageEditorMode.View;
			this.AllowEdition = true;
			this.AllowUpload = true;
			this.AllowDelete = true;
			this.ImageViewHelpText = "<font size='1'>Move the mouse over the image to show options</font>";
			//this.ImageURL = string.Empty;
			
		}

		/*
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
		}*/

		/// <summary>
		/// Gets or sets the relative or absolute path to the directory where the API javascript file is.
		/// </summary>
		/// <remarks>If the value of this property is string.Empty, the external file script is not used and the API is rendered in the page together with the Html TextBox render.</remarks>
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
                if (_scriptDirectory == null)
#if (!FX1_1)
                    return string.Empty;
#else        
					return Define.SCRIPT_DIRECTORY;
#endif
				return _scriptDirectory;
			}
			set
			{
				_scriptDirectory = value;
			}
		}

		/// <summary>
		/// Gets or sets the relative or absolute path to the directory where the temporary files will be created.
		/// </summary>
		/// <value>The temp directory.</value>
		[Bindable(false),
		Category("Behavior"),
		Description("Gets or sets the relative or absolute path to the directory where the temporary files will be created.")	]
		public string TempDirectory
		{
			get
			{
				return _tempDirectory;
			}
			set
			{
				_tempDirectory = value;
			}
		}

		/// <summary>
		/// Gets or sets the relative or absolute path to the directory where the images files will be created or uploaded.
		/// </summary>
		/// <value>The images directory.</value>
		[Bindable(false),
		Category("Behavior"),
		Description("Gets or sets the relative or absolute path to the directory where the images files will be created or uploaded.")	]
		public new string ImagesDirectory
		{
			get
			{
				return _imagesDirectory;
			}
			set
			{
				_imagesDirectory = value;
			}
		}

		/// <summary>
		/// Gets or sets the external script directory.
		/// </summary>
		/// <value>The external script.</value>
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
		/// Gets or sets the editor mode.
		/// </summary>
		/// <value>The editor mode.</value>
		public ImageEditorMode EditorMode
		{
			get
			{
				return _editorMode;
			}
			set
			{
				_editorMode = value;
			}
		}

		/// <summary>
		/// Gets or sets the editor mode after upload.
		/// </summary>
		/// <value>The editor mode after upload.</value>
		public ImageEditorMode EditorModeAfterUpload
		{
			get
			{
				return _editorModeAfterUpload;
			}
			set
			{
				_editorModeAfterUpload = value;
			}
		}

		/// <summary>
		/// Gets or sets the td CSS class.
		/// </summary>
		/// <value>The td CSS class.</value>
		public string TdCssClass
		{
			get
			{
				return _tdCssClass;
			}
			set
			{
				_tdCssClass = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether you writer directly into the picture.
		/// </summary>
		/// <value><c>true</c> if write directly into the picture; otherwise, <c>false</c>.</value>
		public bool DirectWrite
		{
			get
			{
				return _directWrite;
			}
			set
			{
				_directWrite = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether you allow the upload.
		/// </summary>
		/// <value><c>true</c> if you allow the upload; otherwise, <c>false</c>.</value>
		public bool AllowUpload
		{
			get
			{
				return _allowUpload;
			}
			set
			{
				_allowUpload = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether edition is allowed.
		/// </summary>
		/// <value><c>true</c> if edition is allowed; otherwise, <c>false</c>.</value>
		public bool AllowEdition
		{
			get
			{
				return _allowEdition;
			}
			set
			{
				_allowEdition = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether delete is allowed.
		/// </summary>
		/// <value><c>true</c> if delete is allowed; otherwise, <c>false</c>.</value>
		public bool AllowDelete
		{
			get
			{
				return _allowDelete;
			}
			set
			{
				_allowDelete = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether image view help is disabled.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if image view help is disabled; otherwise, <c>false</c>.
		/// </value>
		public bool ImageViewHelpDisabled
		{
			get
			{
				return _hideImageViewHelp;
			}
			set
			{
				_hideImageViewHelp = value;
			}
		}

		/// <summary>
		/// Gets or sets the image view help text.
		/// </summary>
		/// <value>The image view help text.</value>
		public string ImageViewHelpText
		{
			get
			{
				return _imageViewHelpText;
			}
			set
			{
				_imageViewHelpText = value;
			}
		}

		/// <summary>
		/// Gets or sets the width of the image.
		/// </summary>
		/// <value>The width of the image.</value>
		public int ImageWidth
		{
			get
			{
				return _imageWidth;
			}
			set
			{
				_imageWidth = value;
			}
		}

		/// <summary>
		/// Gets or sets the height of the image.
		/// </summary>
		/// <value>The height of the image.</value>
		public int ImageHeight
		{
			get
			{
				return _imageHeight;
			}
			set
			{
				_imageHeight = value;
			}
		}

		/// <summary>
		/// Gets the image size.
		/// </summary>
		/// <value>The image size.</value>
		public System.Drawing.Size ImageSize 
		{
			get
			{
				return _imageSize;
			}
		}
			
				

		/// <summary>
		/// Gets or sets the selection.
		/// </summary>
		/// <value>The selection.</value>
		public Selection Selection
		{
			get
			{
				//if (ViewState["_selection"] == null)
				//	ViewState["_selection"] = new Selection();
				//return (Selection)ViewState["_selection"];
				if (_selection == null)
					_selection = new Selection();
				return _selection;
			}
			set
			{
				//ViewState["_selection"] = value;
				_selection = value;
			}
		}

		/// <summary>
		/// Gets or sets the save settings.
		/// </summary>
		/// <value>The save settings.</value>
		public ImageSettings SaveSettings
		{
			get
			{
				if (ViewState["_saveSettings"] == null)
					ViewState["_saveSettings"] = new ImageSettings();
				return (ImageSettings)ViewState["_saveSettings"];
				/*if (ViewState["_saveSettings"] != null)
					return (ImageSettings)ViewState["_saveSettings"];
				else
				{
					ImageSettings settings = new ImageSettings();
					settings.Compression = FileCompression.CCITT4;
					settings.Quality = 70;
					settings.Format = FileFormat.Jpeg;
					return settings;
				}*/
			}
			set
			{
				ViewState["_saveSettings"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the upload settings.
		/// </summary>
		/// <value>The upload settings.</value>
		public ImageSettings UploadSettings
		{
			get
			{
				/*if (ViewState["_uploadSettings"] != null)
					return (ImageSettings)ViewState["_uploadSettings"];
				else
					return new ImageSettings();*/
				if (ViewState["_uploadSettings"] == null)
					ViewState["_uploadSettings"] = new ImageSettings();
				return (ImageSettings)ViewState["_uploadSettings"];
			}
			set
			{
				ViewState["_uploadSettings"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the image URL.
		/// </summary>
		/// <value>The image URL.</value>
		public string ImageURL
		{
			get
			{
				/*if (ViewState["_imageURL"] == null)
					ViewState["_imageURL"] = string.Empty;
				return (string)ViewState["_imageURL"];*/
				if (ViewState["_imageURL"] == null)
					return string.Empty;
				else
					return (string)ViewState["_imageURL"];
				//return _imageURL;
			}
			set
			{
				ViewState["_imageURL"] = value;
				//_imageURL = value;
			}
		}

		/// <summary>
		/// Gets or sets the save URL.
		/// </summary>
		/// <value>The save URL.</value>
		public string SaveURL
		{
			get
			{
				/*if (ViewState["_imageURL"] == null)
					ViewState["_imageURL"] = string.Empty;
				return (string)ViewState["_imageURL"];*/
				if (ViewState["_saveURLImg"] == null)
					return string.Empty;
				else
					return (string)ViewState["_saveURLImg"];
				//return _imageURL;
			}
			set
			{
				ViewState["_saveURLImg"] = value;
				//_imageURL = value;
			}
		}

		/// <summary>
		/// Gets or sets the temp URL.
		/// </summary>
		/// <value>The temp URL.</value>
		public string TempURL
		{
			get
			{
				if (ViewState["_tempURL"] == null)
					return string.Empty;
				else
					return ViewState["_tempURL"].ToString();
				//return _toolsCreated;
			}
			set
			{
				ViewState["_tempURL"] = value;
				//_toolsCreated = value;
			}
		}

		/// <summary>
		/// Gets the working file.
		/// </summary>
		/// <value>The working file.</value>
		[Browsable(false)]
		public string WorkFile
		{
			get
			{
				if (this.DirectWrite)
					return Page.Server.MapPath(this.ImageURL);
				else
					return Page.Server.MapPath(this.TempURL);
				//return Page.Server.MapPath(this.TempDirectory) + this.ViewState["_tempFile"];
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
		/// Gets or sets the toolbar.
		/// </summary>
		/// <value>The toolbar.</value>
		[Browsable(false)]
		public Toolbar Toolbar
		{
			get
			{
				if (_toolbar == null)
				{
					_toolbar = new Toolbar();
				}
				return _toolbar;
			}
			set
			{
				_toolbar = value;
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
		/// Gets or sets the template or layout to use for the toolbar.
		/// </summary>
		/// <value>The template or layout to use for the toolbar.</value>
		[
		Bindable(false),
		Category("Appearance"),
		Description("Gets or sets the template or layout to use for the toolbar."),
		//DefaultValue("bold,italic,underline,separator,cut,copy,paste,separator,custom,separator,paragraph,fontface,fontsize;alignleft,aligncenter,alignright,alignjustify,separator,orderedlist,unorderedlist,separator,indent,outdent,separator,codesnippets,print,separator,subscript,superscript,strikethrough,separator,replace,image,library,link,fontcolor,highlight,specialchars,codecleaner,table")
		]
		public string Template
		{
			get
			{
				return _template;
			}
			set
			{
				_template = value;
			}
		}

		/// <summary>
		/// Gets or sets the picture filters.
		/// </summary>
		/// <value>The picture filters.</value>
		[
		Bindable(true),
		Category("Behavior"),
		Description("Indicates the pictures filters you want to use."),
		TypeConverter(typeof(StringArrayConverter)),
		DefaultValue("jpg,jpeg,gif,bmp,png")
		]
		public string[] PictureFilters
		{
			get
			{
				object filters = ViewState["PictureFilters"];

				if (filters == null) 
				{
					return new string[] {"jpg","jpeg","gif","bmp","png"};
				}

				else 
				{
					return (string[])filters;
				}
			}
			set
			{
				ViewState["PictureFilters"] = value;
			}
		}

		/// <summary>
		/// Gets the temporary file name.
		/// </summary>
		/// <returns>The temporary file name.</returns>
		public string GetTempFileName()
		{
			return this.GetTempFileName(string.Empty);
		}

		/// <summary>
		/// Gets the temporary file name.
		/// </summary>
		/// <param name="prefix">The prefix.</param>
		/// <returns>The temporary file name.</returns>
		public string GetTempFileName(string prefix)
		{
			string guid = string.Empty;
			if (UseSession)
				guid = Page.Session.SessionID;
			else
				guid = System.Guid.NewGuid().ToString("N");

			return guid + prefix + "_image" + System.DateTime.Now.ToString("yyMMddhhmmss")+System.DateTime.Now.Millisecond.ToString()+(new Random().GetHashCode()) + ".jpg";
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
			if (AutoCreateTools && template.Length > 0)
			{
				_toolbar.Tools.Clear();
				_toolbar.ScriptDirectory = this.ScriptDirectory;

				string label = string.Empty;

				string[] toolbarShemes = template.Split(';');

				for(int i = 0; i < toolbarShemes.Length; i++)
				{
					_toolbar.CellSpacing = 2;
					_toolbar.Dragable = false;
					_toolbar.BorderStyle = BorderStyle.NotSet;
					_toolbar.ImagesDirectory = this.IconsDirectory;
					_toolbar.BackColor = this.BackColor;
					//_toolbar.License = this.License;

					foreach(string tool in toolbarShemes[i].Split(','))
					{
						switch(tool.ToLower().Trim())
						{
								//case "crop": _toolbar.Controls.Add(new ToolCrop()); break;
							case "save": _toolbar.Tools.Add(new ToolSave(ClientID + "_toolSave")); break;
							case "crop": _toolbar.Tools.Add(new ToolCrop(ClientID + "_toolCrop")); break;
							case "text": _toolbar.Tools.Add(new ToolText(ClientID + "_toolText")); break;
							case "flip": _toolbar.Tools.Add(new ToolFlip(ClientID + "_toolFlip")); break;
							case "mirror": _toolbar.Tools.Add(new ToolMirror(ClientID + "_toolMirror")); break;
							case "rotate": _toolbar.Tools.Add(new ToolRotate(ClientID + "_toolRotate")); break;
							case "zoom": _toolbar.Tools.Add(new ToolZoom(ClientID + "_toolZoom")); break;
							case "imagesize": _toolbar.Tools.Add(new ToolImageSize(ClientID + "_toolImageSize")); break;
							case "canvassize": _toolbar.Tools.Add(new ToolCanvasSize(ClientID + "_toolCanvasSize")); break;
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
		/// Register the Client-Side script block in the ASPX page.
		/// </summary>
		public void RegisterAPIScriptBlock() 
		{
			// Register the script block is not allready done.
			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
				if ((this.ExternalScript == null || this.ExternalScript.TrimEnd() == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
				{
#if (!FX1_1)
            Page.ClientScript.RegisterClientScriptInclude(SCRIPTKEY, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.ActiveImage.js"));
#else
					if (CLIENTSIDE_API == null)
						CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls._resources.ActiveImage.js");
					//CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls.HtmlTextBox.HtmlTextBox.js");

					
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
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActiveImage.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
			}

			Page.RegisterArrayDeclaration(ClientID + "_filterImages",Utils.ConvertStringArrayToRegisterArray(this.PictureFilters));

		}

		private bool EnsureToolsCreated()
		{
			if (!this.ToolsCreated)
			{	
				return this.CreateTools();
			}
			else
				return false;
		}

		private void UpdateImageSize()
		{
			// Get the size of the image for later use
			string file = string.Empty;
			if (!DirectWrite)
				file = Page.Server.MapPath(this.TempURL);
			else
				file = Page.Server.MapPath(this.ImageURL);

			ImageJob image = new ImageJob(file);
            _imageHeight = image.Image.Height;
			_imageWidth = image.Image.Width;
			_imageSize = image.Image.Size;
			image.Dispose();
		}

		/// <summary>
		/// Gets the image object.
		/// </summary>
		/// <value>The image object.</value>
		public ImageJob ImageObject
		{
			get
			{
				try
				{
					return new ImageJob(Page.Server.MapPath(this.TempURL));
				}

				catch {return null;}
			}
		}

		/*public int Width
		{
			get
			{
				if (ImageObject != null)
					return ImageObject.Image.Width;

				return 0;
			}
		}

		public int Height
		{
			get
			{
				if (ImageObject != null)
					return ImageObject.Image.Height;

				return 0;
			}
		}*/

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Load"/>
		/// event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			if (this.ImageURL != string.Empty)
			{
				if (!this.DirectWrite && !Page.IsPostBack)
				{
					string tempFile = this.GetTempFileName();
					this.TempURL = this.TempDirectory + tempFile;
					System.IO.File.Copy(Page.Server.MapPath(this.ImageURL), Page.Server.MapPath(this.TempURL));

					//UpdateImageSize();
				}

				UpdateImageSize();
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Init"/>
		/// event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnInit(EventArgs e) 
		{
			base.OnInit(e);

			_toolbar.ID = this.ID + "_toolbar";
			_toolbar.Position = Position.Relative;
			_toolbar.Width = Unit.Parse("100%");
			_toolbar.BackColor = Color.FromArgb(0xE0,0xE0,0xE0);

			EnsureToolsCreated();

			this.Controls.Add(_toolbar);
		}

		/// <summary>
		/// Do some work before rendering the control.
		/// </summary>
		/// <param name="e">Event Args</param>
		protected override void OnPreRender(EventArgs e) 
		{
			base.OnPreRender(e);

			this.RegisterAPIScriptBlock();

			// Register positions holder
			//Page.RegisterHiddenField(ClientID + "positions", "");

			if (Page != null)
				Page.RegisterRequiresPostBack(this);

			if (this.AllowUpload && ((this.EditorMode == ImageEditorMode.Upload)
				|| (this.EditorMode == ImageEditorMode.Edit && this.ImageURL == string.Empty)
				|| (this.EditorMode == ImageEditorMode.View && this.ImageURL == string.Empty)))
			{
				// Check for proper EncType
				foreach(Control control in Page.Controls)
				{
					if (control.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlForm")
					{
						System.Web.UI.HtmlControls.HtmlForm form = (System.Web.UI.HtmlControls.HtmlForm)control;

						if (form.Enctype.ToLower() != "multipart/form-data")
							form.Enctype = "multipart/form-data";

						break;
					}
				}
			}
		}

		/// <summary>
		///     Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output">The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			output.Write("<input type=hidden id=\"{0}positions\" name=\"{1}positions\">", ClientID, ClientID.Replace(":", "_"));
			var enumerator = Style.Keys.GetEnumerator();

			while (enumerator.MoveNext())
			{
				output.AddStyleAttribute((string) enumerator.Current, Style[(string) enumerator.Current]);
			}

			enumerator.Reset();

			while (enumerator.MoveNext())
			{
				output.AddStyleAttribute((string) enumerator.Current, Style[(string) enumerator.Current]);
			}

			ControlStyle.AddAttributesToRender(output);
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "AREA");
			output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE MAIN OPEN

			var imageURL = ImageURL;
			if (TempURL != string.Empty)
				imageURL = TempURL;

			if (AllowEdition && EditorMode == ImageEditorMode.Edit && imageURL != string.Empty)
			{
				// Edit Mode

				output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR MAIN OPEN TOOLBAR
				output.RenderBeginTag(HtmlTextWriterTag.Td); // TD MAIN OPEN TOOLBAR

				// Render the toolbar
				RenderChildren(output);

				output.RenderEndTag(); // TD MAIN CLOSE TOOLBAR
				output.RenderEndTag(); // TR MAIN CLOSE TOOLBAR

				RenderMainImage(output);

				RenderStatus(output);

				output.Write(
					"<table class=\"defStyle\" id=\"{0}rubber\" style=\"Z-INDEX: 200;border-style: dashed;border-width: 1px;border-color:black;\"><tr><td></td></tr></table>",
					ClientID);
				output.Write(
					"<table class=\"defStyle\" id=\"{0}rubber2\" style=\"Z-INDEX: 199;border-style: solid;border-width: 1px;border-color:white;\"><tr><td></td></tr></table>",
					ClientID);
			}
			else if (AllowUpload && (EditorMode == ImageEditorMode.Upload
									 || EditorMode == ImageEditorMode.Edit && imageURL == string.Empty
									 || EditorMode == ImageEditorMode.View && imageURL == string.Empty))
			{
				RenderAllowUpload(output);
			}
			else if (imageURL != string.Empty
					 && (EditorMode == ImageEditorMode.Edit && !AllowEdition
						 || EditorMode == ImageEditorMode.View
						 || EditorMode == ImageEditorMode.Upload && !AllowUpload))
			{
				RenderAllowEditionOrDeletion(output);
			}
			else
			{
				output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR NO IMAGE OPEN
				output.RenderBeginTag(HtmlTextWriterTag.Td); // TD NO IMAGE OPEN

				output.Write("No Image Available");

				output.RenderEndTag(); // TD NO IMAGE CLOSE
				output.RenderEndTag(); // TR NO IMAGE CLOSE

				output.RenderEndTag(); // TABLE MAIN CLOSE
			}

			output.Write(
				"<style> .defStyle { Z-INDEX:200;POSITION:absolute; left: -1000px; top: -1000px; display: none; visibility: hidden; cursor: crosshair; }\n.aieTD { font-family: Verdana; font-size: 9pt;}</style>");
		}

		private void RenderMainImage(HtmlTextWriter output)
		{
			output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR MAIN OPEN IMAGE
			output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			output.RenderBeginTag(HtmlTextWriterTag.Td); // TD MAIN OPEN IMAGE

			RenderOutputNotFX1_1(output);
			RenderOutputFX1_1(output);

			// The Image
			output.AddAttribute(HtmlTextWriterAttribute.Src, DirectWrite ? ImageURL : TempURL);
			output.RenderBeginTag(HtmlTextWriterTag.Img); // IMAGE OPEN
			output.RenderEndTag(); // IMAGE CLOSE

			output.RenderEndTag(); // TD MAIN CLOSE IMAGE
			output.RenderEndTag(); // TR MAIN CLOSE IMAGE
		}

		private void RenderStatus(HtmlTextWriter output)
		{
			output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR STATUS OPEN
			output.RenderBeginTag(HtmlTextWriterTag.Td); // TD STATUS OPEN

			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE IN STATUS OPEN
			output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR IN STATUS OPEN
			output.AddAttribute(HtmlTextWriterAttribute.Nowrap, "");

			output.RenderBeginTag(HtmlTextWriterTag.Td); // TD IN STATUS LEFT OPEN
			output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE XYPOS OPEN
			output.RenderBeginTag(HtmlTextWriterTag.Tr); // TABLE XYPOS OPEN
			output.AddAttribute(HtmlTextWriterAttribute.Class, TdCssClass);
			output.RenderBeginTag(HtmlTextWriterTag.Td); // TD XYPOS OPEN
			output.Write("Pos:X.");
			output.RenderEndTag();
			output.AddAttribute(HtmlTextWriterAttribute.Class, TdCssClass);
			output.RenderBeginTag(HtmlTextWriterTag.Td); // TD XYPOS OPEN
			output.Write("<div id=\"{0}xPosLabel\">*</div>", ClientID);
			output.RenderEndTag();
			output.AddAttribute(HtmlTextWriterAttribute.Class, TdCssClass);
			output.RenderBeginTag(HtmlTextWriterTag.Td); // TD XYPOS OPEN
			output.Write("|Y.");
			output.RenderEndTag();
			output.AddAttribute(HtmlTextWriterAttribute.Class, TdCssClass);
			output.RenderBeginTag(HtmlTextWriterTag.Td); // TD XYPOS OPEN
			output.Write("<div id=\"{0}yPosLabel\">*</div>", ClientID);
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();

			output.RenderEndTag(); // TD IN STATUS LEFT CLOSE

			output.AddAttribute(HtmlTextWriterAttribute.Align, "right");
			output.RenderBeginTag(HtmlTextWriterTag.Td); // TD IN STATUS LEFT OPEN

			output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE SELECTION OPEN
			output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR SELECTION OPEN

			output.AddAttribute(HtmlTextWriterAttribute.Class, TdCssClass);
			output.RenderBeginTag(HtmlTextWriterTag.Td); // TD SELECTION OPEN
			output.Write("Sel:");
			output.RenderEndTag(); // TD SELECTION CLOSE
			output.AddAttribute(HtmlTextWriterAttribute.Class, TdCssClass);
			output.RenderBeginTag(HtmlTextWriterTag.Td); // TD SELECTION OPEN
			output.Write("<div id=\"{0}storedPos\">0,0,0,0</div>", ClientID);
			output.RenderEndTag(); // TD SELECTION CLOSE

			output.RenderEndTag(); // TR SELECTION CLOSE
			output.RenderEndTag(); // TABLE SELECTION CLOSE

			output.RenderEndTag(); // TD IN STATUS LEFT CLOSE

			output.RenderEndTag(); // TR IN STATUS CLOSE
			output.RenderEndTag(); // TABLE IN STATUS CLOSE

			output.RenderEndTag(); // TD STATUS CLOSE
			output.RenderEndTag(); // TR STATUS CLOSE
		}

		private void RenderAllowUpload(HtmlTextWriter output)
		{
			output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR MESSAGE OPEN
			output.AddAttribute(HtmlTextWriterAttribute.Class, TdCssClass);
			output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			output.RenderBeginTag(HtmlTextWriterTag.Td); // TD MESSAGE OPEN
			output.Write("<b>Please select an image to upload.</b>");
			output.RenderEndTag(); // TD MESSAGE CLOSE
			output.RenderEndTag(); // TR MESSAGE CLOSE

			output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR UPLOAD OPEN
			output.AddAttribute(HtmlTextWriterAttribute.Class, TdCssClass);
			output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			output.RenderBeginTag(HtmlTextWriterTag.Td); // TD UPLOAD OPEN
			output.Write("<input type=file id=\"{0}Upload\" name=\"{0}Upload\">", ClientID);
			output.RenderEndTag(); // TD UPLOAD CLOSE
			output.RenderEndTag(); // TR UPLOAD CLOSE

			output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR SUBMIT OPEN
			output.AddAttribute(HtmlTextWriterAttribute.Class, TdCssClass);
			output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			output.RenderBeginTag(HtmlTextWriterTag.Td); // TD SUBMIT OPEN
			output.Write("<input type=button value=\"Upload Image\" onclick=\"{0}\">",
				Page.GetPostBackClientEvent(this, "uploading"));
			output.RenderEndTag(); // TD SUBMIT CLOSE
			output.RenderEndTag(); // TR SUBMIT CLOSE

			output.RenderEndTag(); // TABLE MAIN CLOSE
		}

		private void RenderAllowEditionOrDeletion(HtmlTextWriter output)
		{
			output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR NO IMAGE OPEN
			output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			output.RenderBeginTag(HtmlTextWriterTag.Td); // TD NO IMAGE OPEN

			if (AllowEdition || AllowDelete)
			{
				output.AddAttribute(OnMouseOver, string.Format(AieShowToolbar, ClientID));
				output.AddAttribute(OnMouseOver, string.Format(AieHideToolbar, ClientID));
			}

			output.AddAttribute(HtmlTextWriterAttribute.Src, TempURL != string.Empty ? ImageURL : TempURL);
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "workImage");
			output.AddAttribute("galleryimg", "no");
			output.RenderBeginTag(HtmlTextWriterTag.Img); // IMAGE OPEN
			output.RenderEndTag(); // IMAGE CLOSE

			output.RenderEndTag(); // TD NO IMAGE CLOSE
			output.RenderEndTag(); // TR NO IMAGE CLOSE

			if (!ImageViewHelpDisabled && (AllowEdition || AllowDelete))
			{
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.AddAttribute(HtmlTextWriterAttribute.Class, TdCssClass);
				output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.Write(ImageViewHelpText);
				output.RenderEndTag();
				output.RenderEndTag();
			}

			output.RenderEndTag(); // TABLE MAIN CLOSE

			RenderOnMouseOver(output);
		}

		private void RenderOnMouseOver(HtmlTextWriter output)
		{
			if (AllowEdition || AllowDelete)
			{
				var clientID = ClientID.Replace(":", "_");
				output.AddAttribute("onmouseover", "AIE_showToolbar('" + ClientID + "');");
				output.AddAttribute(HtmlTextWriterAttribute.Class, "defStyle");
				output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "toolbar");
				output.AddStyleAttribute("cursor", "hand");
				output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "#DBD8D1");
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "DarkGray");
				output.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px");
				output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
				output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
				output.RenderBeginTag(HtmlTextWriterTag.Table); // TABLE TOOLBAR OPEN
				output.RenderBeginTag(HtmlTextWriterTag.Tr); // TR TOOLBAR OPEN
				output.RenderBeginTag(HtmlTextWriterTag.Td); // TD TOOLBAR OPEN

				if (AllowEdition)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Src, IconsDirectory + EditOff);
					output.AddAttribute(HtmlTextWriterAttribute.Alt, "Edit");
					output.AddAttribute("onmouseover", "ATB_swap('" + clientID + "Edit', '" + IconsDirectory + EditOver + "');");
					output.AddAttribute("onmouseout", "ATB_swap('" + clientID + "Edit', '" + IconsDirectory + EditOff + "');");
					output.AddAttribute("onclick", Page.GetPostBackClientEvent(this, "edit"));
					output.AddAttribute(HtmlTextWriterAttribute.Name, clientID + "Edit");
					output.RenderBeginTag(HtmlTextWriterTag.Img); // EDIT IMAGE OPEN
					output.RenderEndTag(); // EDIT IMAGE CLOSE
				}

				if (AllowDelete)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Src, IconsDirectory + DeleteOff);
					output.AddAttribute(HtmlTextWriterAttribute.Alt, "Delete");
					output.AddAttribute("onmouseover", "ATB_swap('" + clientID + "Delete', '" + IconsDirectory + DeleteOver + "');");
					output.AddAttribute("onmouseout", "ATB_swap('" + clientID + "Delete', '" + IconsDirectory + DeleteOff + "');");
					output.AddAttribute("onclick", Page.GetPostBackClientEvent(this, "delete"));
					output.AddAttribute(HtmlTextWriterAttribute.Name, clientID + "Delete");
					output.RenderBeginTag(HtmlTextWriterTag.Img); // DELETE IMAGE OPEN
					output.RenderEndTag(); // DELETE IMAGE CLOSE
				}

				output.RenderEndTag();
				output.RenderEndTag(); // TR TOOLBAR CLOSE
				output.RenderEndTag(); // TABLE TOOLBAR CLOSE
			}
		}

		/// <summary>
		/// A LoadPostData method.
		/// </summary>
		/// <param name="postDataKey">PostDataKey.</param>
		/// <param name="postCollection">postCollection.</param>
		/// <returns>bool</returns>
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{
			Selection presentValue = this.Selection;
			Selection postedValue = new Selection(postCollection[postDataKey + "positions"]);

			if (presentValue == null || !presentValue.Equals(postedValue)) 
			{
				this.Selection = postedValue;
	
				return true;
			}

			return false;
		}


		/// <summary>
		/// A RaisePostDataChangedEvent.
		/// </summary>
		public virtual void RaisePostDataChangedEvent() 
		{
			OnSelectionChanged(EventArgs.Empty);
		}
      
		/// <summary>
		/// The SelectionChanged event handler.
		/// </summary>
		public event EventHandler SelectionChanged;
     
		/// <summary>
		/// A OnSelectionChanged event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSelectionChanged(EventArgs e) 
		{
			if (SelectionChanged != null)
				SelectionChanged(this,e);
		}

		/// <summary>
		/// The PreProcessing event handler.
		/// </summary>
		public event EventHandler PreProcessing;
     
		/// <summary>
		/// A OnPreProcessing event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPreProcessing(EventArgs e) 
		{
			if (PreProcessing != null)
				PreProcessing(this,e);
		}

		/// <summary>
		/// The PostProcessing event handler.
		/// </summary>
		public event EventHandler PostProcessing;
     
		/// <summary>
		/// A OnPostProcessing event.
		/// </summary>
		/// <param name="e"></param>
		internal virtual void OnPostProcessing(EventArgs e) 
		{
			if (PostProcessing != null)
				PostProcessing(this,e);
		}

		/// <summary>
		/// The OnUpload event handler.
		/// </summary>
		public event EventHandler Upload;
     
		/// <summary>
		/// A OnUpload event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnUpload(EventArgs e) 
		{
			if (Upload != null)
				Upload(this,e);
		}
        
		/// <summary>
		/// When implemented by a class, enables a server control to process an event raised when a form is posted to the server.
		/// </summary>
		/// <param name="eventArgument">A <see cref="T:System.String"/> that represents an optional event argument to be passed to the event handler.</param>
		public void RaisePostBackEvent(String eventArgument)
		{
			switch (eventArgument)
			{
				case "uploading":

					OnUpload(EventArgs.Empty);

					int indexRequestedFile = -1;
					for(int i = 0 ;  i < Page.Request.Files.Count ; i++)
					{
						if (Page.Request.Files.Keys[i].IndexOf(this.ClientID) >= 0)
						{
							indexRequestedFile = i;
							break;
						}
					}

					if (indexRequestedFile > -1 && Page.Request.Files[indexRequestedFile].FileName.Length > 0)
					{

						string imageURL = this.GetTempFileName("_uploaded-");
						if (this.ImagesDirectory != string.Empty)
							imageURL = this.ImagesDirectory.TrimEnd('/') + "/" + imageURL;
						else if (this.TempDirectory != string.Empty)
							imageURL = this.TempDirectory.TrimEnd('/') + "/" + imageURL;
						
						Page.Request.Files[indexRequestedFile].SaveAs(Page.Server.MapPath(imageURL));

						this.ImageURL = imageURL;

						ImageSettings blankSettings = new ImageSettings();

						if (UploadSettings != null && !blankSettings.Equals(UploadSettings))
						{
							OnPreProcessing(EventArgs.Empty);

							ImageJob job = new ImageJob(Page.Server.MapPath(imageURL));
							//job.License = _license;
							if (UploadSettings.MaxHeight != 0)
								job.ResizeImage(UploadSettings.MaxWidth, UploadSettings.MaxWidth,
									UploadSettings.ConstrainProportions, UploadSettings.ResizeSmaller);
							job.Save(Page.Server.MapPath(imageURL), UploadSettings.Compression,
								UploadSettings.Quality, UploadSettings.Format);
						}

						string tempFile = this.GetTempFileName();
						this.TempURL = this.TempDirectory + tempFile;
						System.IO.File.Copy(Page.Server.MapPath(this.ImageURL), Page.Server.MapPath(this.TempURL));

						UpdateImageSize();

						this.EditorMode = this.EditorModeAfterUpload;
					}
					break;
				case "edit":
					this.EditorMode = ImageEditorMode.Edit;
					break;
				case "delete":
					System.IO.File.Delete(Page.Server.MapPath(this.ImageURL));
					System.IO.File.Delete(Page.Server.MapPath(this.TempURL));
					this.TempURL = string.Empty;
					this.ImageURL = string.Empty;
					break;
			}
		}

		[Conditional("NOT_FX1_1")]
		private void RenderOutputNotFX1_1(HtmlTextWriter output)
		{
			output.Write("<input type=\"image\" src=\"{1}\" border=\"0\" galleryimg=\"no\" id=\"{0}workImage\" style=\"position:absolute;z-index:201;cursor:crosshair;width:{2}px;height:{3}px\" onclick=\"return false;\" onmousemove=\"AIE_trackMouse('{0}', event);\" onselectstart=\"return false;\" onmousedown=\"AIE_setPosition('{0}', event);\" onmouseup=\"AIE_resetMouse();\" ondragstart=\"return false;\">",
				ClientID,
				IconsDirectory == string.Empty
					? Page.ClientScript.GetWebResourceUrl(GetType(), ResourceName)
					: IconsDirectory + Spacer, ImageWidth, ImageHeight);
		}

		[Conditional("FX1_1")]
		private void RenderOutputFX1_1(HtmlTextWriter output)
		{
			output.Write(string.Format("<input type=\"image\" src=\"{1}\" border=\"0\" galleryimg=\"no\" id=\"{0}workImage\" style=\"position:absolute;z-index:201;cursor:crosshair;width:{2}px;height:{3}px\" onclick=\"return false;\" onmousemove=\"AIE_trackMouse('{0}', event);\" onselectstart=\"return false;\" onmousedown=\"AIE_setPosition('{0}', event);\" onmouseup=\"AIE_resetMouse();\" ondragstart=\"return false;\">", ClientID, this.IconsDirectory + "spacer.gif", this.ImageWidth.ToString(), this.ImageHeight.ToString()));
		}
	}
}
