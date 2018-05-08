using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Web.UI;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.IO;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolTemplateManager"/> object.
	/// </summary>
	[
	Serializable,
    ToolboxItem(false)
	]
	public class ToolTemplateManager : ToolDropDownList ,INamingContainer,IPostBackEventHandler,IPostBackDataHandler
	{
		private ArrayList _templateExtensions;
		private const string KEY = "HTB_TEMPLATE";
		private System.Text.Encoding _encoding;
		private bool _mustBeRendered = true;    
		private int _currentSelectedIndex;

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolTemplateManager"/> class.
		/// </summary>
		public ToolTemplateManager() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolTemplateManager"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolTemplateManager(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolTemplateManager" + Editor.indexTools++;
			else
				this.ID = id;

			ClientScriptKey = KEY;
			Encoding = System.Text.Encoding.ASCII;

			this.Text = "Template Manager";
			this.Height = Unit.Parse("22px");
		}

		/// <summary>
		/// Notifies the Popup control to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if (((base.Page != null) && base.Enabled))
			{
				Page.RegisterRequiresPostBack(this);
				Page.RegisterHiddenField(UniqueID,DateTime.Now.ToString());
			}

			bool isNs6 = false;
			System.Web.HttpBrowserCapabilities browser = Page.Request.Browser; 
			if (browser.Browser.ToUpper().IndexOf("IE") == -1)
				isNs6 = true;
			
			if (isNs6)
				_mustBeRendered = false;
			else
				_mustBeRendered = true;

			if (this.Files.Count == 0)
			{
				this.Items.Add(new ToolItem("<b>No files defined</b>",string.Empty));
			}
			else
			{
				this.AutoPostBack = true;
				this.Items.Clear();
				foreach(File f in this.Files)
				{
					this.Items.Add(new ToolItem(f.Label,f.Location));
				}
			}
		}

		/// <summary>
		/// Sends the Popup content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
			if (_mustBeRendered)
			{
				base.Render(output);
			}
		}

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
			Editor editor = (Editor)this.Parent.Parent.Parent; editor.Text = "Load";
			_currentSelectedIndex = Int32.Parse(postCollection[UniqueID + "_selectedIndex"]);
			return true;
		}

		/// <summary>
		/// Notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			Page.Trace.Write(this.ID, "RaisePostDataChangedEvent...");
			if (_currentSelectedIndex != -1)
			{
				LoadFile(this.Files[_currentSelectedIndex].Location);
				OnFileLoad(new EventArgs());
			}
		}

		/// <summary>
		/// Defines the FileLoad event.
		/// </summary>
		public event EventHandler FileLoad;

		/// <summary>
		/// Invokes delegates registered with the Load event.
		/// </summary>
		/// <param name="e">EventArgs</param>
		protected virtual void OnFileLoad(EventArgs e) 
		{
			if (FileLoad != null) 
				FileLoad(this, e);
		}

		/// <summary>
		/// Load the specified file content in the editor.
		/// </summary>
		/// <param name="location">The full path to the file.</param>
		private void LoadFile(string location)
		{
			string content = string.Empty;
            Editor editor = (Editor)this.Parent.Parent.Parent;
			if (location.ToUpper().StartsWith("HTTP://"))
			{
				System.IO.Stream stream;
				System.Net.WebRequest webRequest;
				System.Net.WebResponse webResponse;
				webRequest = System.Net.WebRequest.Create(location);
				webResponse = webRequest.GetResponse();
				stream = webResponse.GetResponseStream();
				content = new StreamReader(stream).ReadToEnd();
			}

			else
			{
				if (location.ToUpper().StartsWith("FILE://"))
					location = location.Substring(7);
 
				if (System.IO.File.Exists(location))
				{
					try
					{
						StreamReader textFileReader = new StreamReader(location, _encoding);
						content = textFileReader.ReadToEnd();
						textFileReader.Close();
					}
					catch
					{
						//ParentEditor.Messages.Add("An error occured while loading the file. Verify the permissions.");
					}

				}
				else
				{
					
				}
			}
			editor.Text = content;
		}

		/// <summary>
		/// Gets or sets the encoding to use to load files.
		/// </summary>
		public System.Text.Encoding Encoding
		{
			get
			{
				return _encoding;
			}
			set
			{
				_encoding = value;
			}
		}

		/// <summary>
		/// Gets or sets the Files objects used by the tool.
		/// </summary>
		public FileCollection Files
		{
			get
			{
				if (ViewState["_files"] == null)
					ViewState["_files"] = new FileCollection();
				return (FileCollection)ViewState["_files"];
			}
			set
			{
				ViewState["_files"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the template extensions (with the '.') that is autorized.
		/// </summary>
		public ArrayList TemplateExtensions
		{
			get
			{
				if (_templateExtensions == null)
					_templateExtensions = new ArrayList();
				return _templateExtensions;
			}
			set
			{
				_templateExtensions = value;
			}
		}
	}
}
