using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;
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
	/// Sourcer order field.
	/// </summary>
	public enum DataSourceOrder
	{
		/// <summary>
		/// Ascending order.
		/// </summary>
		Ascending = 0,
		/// <summary>
		/// Descending order.
		/// </summary>
		Descending 
	};

	/// <summary>
	/// Selection type enumeration.
	/// </summary>
	public enum SelectionType
	{
		/// <summary>
		/// None selection
		/// </summary>
		None = 0,
		/// <summary>
		/// Checkbox selection.
		/// </summary>
		CheckBox,
		/// <summary>
		/// Single selection.
		/// </summary>
		Single
	};

	/// <summary>
	/// Represents the TreeView control.
	/// </summary>
	[	
	DefaultProperty("Text"), 
	ToolboxData("<{0}:TreeView runat=server></{0}:TreeView>"),
	Designer(typeof(ActiveUp.WebControls.Design.TreeViewControlDesigner)),
	Editor(typeof(ActiveUp.WebControls.Design.TreeViewComponentEditor),typeof(ComponentEditor)),
	CLSCompliantAttribute(true),
	ComVisibleAttribute(true),
	Serializable,
    ToolboxBitmap(typeof(TreeView), "ToolBoxBitmap.Treeview.bmp")
	]
	public class TreeView : TreeNode, INamingContainer
	{
		private string _dataValue = "";
		private string _dataText = "";
		private string _dataParent = "";
		private string _linkFormat = "";
		private string _dataLink = "";
		private string _dataIcon = "";
		private string _dataExpanded = "";
		private string _dataSelected = "";
		private string _sortColumn = ""; 
		private DataSourceOrder _sortOrder = DataSourceOrder.Ascending;
		private bool _textMode;
		private DataTable _dataSource;
		private IconSet _icons;
		private bool _displayMasterNode = true;
		private static string CLIENTSIDE_API = string.Empty;
		private static string SCRIPTKEY = "ACTIVETREE_KEY";
		private System.Web.UI.WebControls.Style _nodesStyleSelected;
		private string _scriptDirectory;
		private bool _autoCheckChildren = true;
		private bool _selectedGrayed = false;
		private Unit _panelHeight, _panelWidth;
		private SelectionType _selectionType = SelectionType.None;

#if (LICENSE)
		/// <summary>
		/// Used for the license counter.
		/// </summary>
		internal static int _useCounter;

		//private string _license = string.Empty;
#endif

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TreeView()
		{
			this.BaseTree = this;
			_linkFormat = string.Empty;
			this.Expanded = true;
			_icons = new IconSet();

			this.Key = DateTime.Now.Ticks.ToString();

			//_scriptDirectory = "/aspnet_client/ActiveWebControls/" + StaticContainer.VersionString + "/";
#if (!FX1_1)
      _scriptDirectory = string.Empty;
#else
			_scriptDirectory = Define.SCRIPT_DIRECTORY;
#endif

			/*
			DateTime evalEnd = new DateTime(2003, 12, 30);
			if (DateTime.Now > evalEnd)
				throw new Exception("Trial period expired. Please register: http://www.activeup.com.");*/

		}

		/// <summary>
		/// Gets or sets the main data source of the body template.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("Main data source of the body template."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)

		] 
		public DataTable DataSource
		{
			get
			{
				return _dataSource;
			}
			set
			{
				_dataSource = value;
			}
		}

		/// <summary>
		/// Binds a data source the treeview.
		/// </summary>
		public override void DataBind()
		{
			if (this.DataSource != null)
			{
				AddNodes("0");	
			}
		}

		/// <summary>
		/// Add a new child node.
		/// </summary>
		/// <param name="parent">Parent key to identify the parent node.</param>
		private void AddNodes(string parent)
		{
			string sortOrder = string.Empty;
			switch (_sortOrder)
			{
				case DataSourceOrder.Descending:
				{
					sortOrder = " DESC";
				} break;

				default : 
				{
					sortOrder = " ASC";
				} break;
			}


			//DataRow[] drTemp = this._dataSource.Select(this.DataParent + " = '" + parent + "'", this.DataText + " ASC");			
			DataRow[] drTemp = this._dataSource.Select(this.DataParent + " = '" + parent + "'", (this.SortColumn != string.Empty ? this.SortColumn : this.DataText) + sortOrder);
			foreach(DataRow row in drTemp)
			{
				string datavalue = "";
				string datalink = "";
				string datatarget = "";
				string datatext = "";
				string dataicon = "";
				bool dataexpanded = false;
				bool dataselected = false;

				// id
				if (this.DataValue != null && this.DataValue != "")
					datavalue = row[this.DataValue].ToString();
				
				// link
				if (this.DataLink != null && this.DataLink != "")
					datalink = row[this.DataLink].ToString();

				// target
				if (this.DataTarget != null && this.DataTarget != "")
					datatarget = row[this.DataTarget].ToString();

				// text
				if (this.DataText != null && this.DataText != "")
					datatext = row[this.DataText].ToString();

				// icon
				if (this.DataIcon != null && this.DataIcon != "")
					dataicon = row[this.DataIcon].ToString();

				// expanded
				dataexpanded = false;
				try
				{
					if (this.DataExpanded != null && this.DataExpanded != "")
						dataexpanded = bool.Parse(row[this.DataExpanded].ToString());
				}
				catch {}

				// selected
				dataselected = false;
				try
				{
					if (this.DataSelected != null && this.DataSelected != "")
						dataselected = bool.Parse(row[this.DataSelected].ToString());
				}

				catch {}

				TreeNode node = this.FindNode(parent);
				
				if (node == null)
				{
					if (dataicon != null && dataicon != string.Empty)
						this.AddNode(datavalue, datatext, datalink, datatarget, dataexpanded, dataicon, dataselected);
					else
						this.AddNode(datavalue, datatext, datalink, datatarget, dataexpanded, dataselected);
				}
				else
				{
					if (dataicon != null && dataicon != string.Empty)
						node.AddNode(datavalue, datatext, datalink, datatarget, dataexpanded, dataicon, dataselected);
					else
						node.AddNode(datavalue, datatext, datalink, datatarget, dataexpanded, dataselected);
				}

				AddNodes(row[this.DataValue].ToString());
			}
		}

		/// <summary>
		/// Gets the font properties associated with the tree view.
		/// </summary>
		[Category("Appearance")]
		[NotifyParentPropertyAttribute(true)]
		[DefaultValueAttribute(null)]
		[Browsable(false)]
		public override FontInfo Font
		{
			get
			{
				return base.Font;
			}
		}

				
		/// <summary>
		/// Gets or sets the text mode. If true, the treeview is displayed as text, otherwise it's displayed as html.
		/// </summary>
		[
		Bindable(true), 
		Category("TreeView"), 
		DefaultValue(false),
		Description("If the treeview have to be display as text or not.")
		] 
		public bool TextMode
		{
			get
			{	
				return _textMode;
			}

			set
			{
				_textMode = value;
			}
		}

		/// <summary>
		/// Gets or sets the selection type.
		/// </summary>
		[
		Bindable(true), 
		Category("TreeView"), 
		DefaultValue(SelectionType.None),
		Description("Indicates the type of selection.")
		] 
		public SelectionType SelectionType
		{
			get
			{	
				return _selectionType;
			}

			set
			{
				_selectionType = value;
			}
		}

		/// <summary>
		/// Gets or sets the icons necessary to display the treeview.
		/// </summary>
		[	
		Bindable(true), 
		Browsable(true),
		Category("TreeView"), 
		DefaultValue(""),
		//PersistenceMode( PersistenceMode.InnerProperty )
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)
		] 
		public IconSet Icons
		{
			get
			{	
				return _icons;
			}

			set
			{
				_icons = value;
			}
		}

		/// <summary>
		/// Gets or sets the field of the data source that provides the text content of the list items.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("Field of the data source that provides the text content of the list items")
		] 
		public string DataText
		{
			get
			{	
				return _dataText;
			}

			set
			{
				_dataText = value;
			}
		}

		/// <summary>
		/// Gets or sets the field of the data source that provides the value of each list item.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("Field of the data source that provides the value of each list item.")
		] 
		public string DataValue
		{
			get
			{	
				return _dataValue;
			}

			set
			{
				_dataValue = value;
			}
		}

		/// <summary>
		/// Gets or sets the field of the data source that provides the value of each link item.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("Field of the data source that provides the value of each link item.")
		]
		public string DataLink
		{
			get
			{
				return _dataLink;
			}

			set
			{
				_dataLink = value;
			}
		}

		/// <summary>
		/// Gets or sets the field of the data source that provides the value of each target item.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("Field of the data source that provides the value of each target item.")
		]
		public string DataTarget { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the field of the data source that provides the value of each target item.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("Field of the data source that provides the value of each icon item.")
		]
		public string DataIcon
		{
			get {return _dataIcon;}
			set {_dataIcon = value;}
		}

		/// <summary>
		/// Gets or sets the field of the data source that provides if the item must be expanded.
		/// </summary>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("Field of the data source that provides if the item must be expanded.")
		]
		public string DataExpanded
		{
			get {return _dataExpanded;}
			set {_dataExpanded = value;}
		}

		/// <summary>
		/// Gets or sets the data source that provides if the item must be selected
		/// </summary>
		/// <value>The data selected.</value>
		[
		Bindable(true),
		Category("Data"),
		DefaultValue(""),
		Description("Field of the data source that provides if the item must be selected.")
		]
		public string DataSelected
		{
			get {return _dataSelected;}
			set {_dataSelected = value;}
		}

		/// <summary>
		/// Gets or sets the field of data parent.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("Field of the data source that provides the value of each target item.")
		] 
		public string DataParent
		{
			get
			{	
				return _dataParent;
			}

			set
			{
				_dataParent = value;
			}
		}

		/// <summary>
		/// Gets or sets the field for sorting nodes.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue(""),
		Description("Field of the data source for sorting nodes.")
		] 
		public string SortColumn
		{
			get
			{
				return _sortColumn;
			}

			set
			{
				_sortColumn = value;
			}
		}

		/// <summary>
		/// Gets or sets the field for sort order.
		/// </summary>
		[
		Bindable(true), 
		Category("Data"), 
		DefaultValue("Ascending"),
		Description("Field of the data source for sort order.")
		] 
		public DataSourceOrder SortOrder
		{
			get
			{
				return _sortOrder;
			}

			set
			{
				_sortOrder = value;
			}
		}

		/// <summary>
		/// Gets or sets the format of the link.
		/// </summary>
		[
		Bindable(true), 
		Category("TreeView"), 
		DefaultValue(""),
		Description("The format of the link.")
		] 
		public string LinkFormat
		{
			get
			{	
				return _linkFormat;
			}

			set
			{
				_linkFormat = value;
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
		/// Gets or sets the value indicates if the master node must be displayed or not.
		/// </summary>
		[
		Bindable(true),
		Category("TreeView"),
		DefaultValue(true),
		Description("If the master node have to be display or not.")
		]
		public bool DisplayMasterNode
		{
			get
			{
				return _displayMasterNode;
			}

			set
			{
				_displayMasterNode = value;
			}
		}

		/// <summary>
		/// Gets or sets the filename of the external script file.
		/// </summary>
		public string ExternalScript
		{
			get
			{
				string _externalScript;
				_externalScript = ((string) base.ViewState["_externalScript"]);
				if (_externalScript != null)
					return _externalScript; 
				return string.Empty;
			}
			set
			{
				ViewState["_externalScript"] = value;
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
		/*[Bindable(true), 
		Category("TreeView"), 
		DefaultValue("")] 
		public string Target
		{
			get
			{	
				return _target;
			}

			set
			{
				_target = value;
			}
		}*/

		/// <summary>
		/// Get the specified resource from the assembly.
		/// </summary>
		/// <param name="resource">The name of the resource.</param>
		/// <returns>The string representation of the resource.</returns>
		private static string GetResource(string resource)
		{
			string str = null;
			Assembly asm = Assembly.GetExecutingAssembly();
			
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

		/// <summary>
		/// Raises the PreRender event.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{
			if (base.Page != null)
			{
				Page.RegisterRequiresPostBack(this);
				if (LoadOnDemand)
				{
					Page.Session[this.ID] = this;
				}
			}

			base.OnPreRender(e);
		
			/*this.Page.RegisterClientScriptBlock(TREEVIEW_KEY, this.GetResource("ActiveUp.WebControls.ActiveTree.ActiveTree.js"));
		
			string startupString = "<script language='javascript'>\n";
			startupString += "var atv_" + this.ClientID + "_de = '" + this.Icons.Default + "'\n";
			startupString += "var atv_" + this.ClientID + "_ex = '" + this.Icons.Expanded + "'\n";
			startupString += "var atv_" + this.ClientID + "_co = '" + this.Icons.Collapsed + "'\n";
			//startupString += "var atv_" + this.ClientID + "_curSelNode = ''" + "\n";
			startupString += "</script>\n";

			this.Page.RegisterStartupScript("ActiveTreeView_Startup_" + this.ClientID, startupString);*/

			RegisterAPIScriptBlock(this.Page);
		}

		/// <summary>
		/// Registers the API script block.
		/// </summary>
		/// <param name="page">The page.</param>
		public virtual void RegisterAPIScriptBlock(System.Web.UI.Page page) 
		{
			// Register the script block is not allready done.

			if (!Page.IsClientScriptBlockRegistered(SCRIPTKEY)) 
			{
				if ((this.ExternalScript == null || this.ExternalScript == string.Empty) && (this.ScriptDirectory == null || this.ScriptDirectory.TrimEnd() == string.Empty))
				{
#if (!FX1_1)
            Page.ClientScript.RegisterClientScriptInclude(SCRIPTKEY, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.ActiveTree.js"));
#else					
					if (CLIENTSIDE_API == null || CLIENTSIDE_API == string.Empty)
						CLIENTSIDE_API = EditorHelper.GetResource("ActiveUp.WebControls._resources.ActiveTree.js");
					
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
					Page.RegisterClientScriptBlock(SCRIPTKEY, "<script language=\"javascript\" src=\"" + this.ScriptDirectory.TrimEnd('/') + "/" + (this.ExternalScript == string.Empty ? "ActiveTree.js" : this.ExternalScript) + "\"  type=\"text/javascript\"></SCRIPT>");
				}
			}
            
			string startupString = "<script language='javascript'>\n";
			startupString += "// Test if the client script is present.\n";
			startupString += "try\n{\n";
			startupString += "ATV_testIfScriptPresent();\n";
			//startupString += "}\ncatch (e) \n{\nalert('Could not find external script file. Please Check the documentation.');\n}\n";
			startupString += "}\n catch (e)\n {\n alert('Could not find script file. Please ensure that the Javascript files are deployed in the " + ((ScriptDirectory == string.Empty) ? string.Empty : " [" + ScriptDirectory + "] directory or change the") + "ScriptDirectory and/or ExternalScript properties to point to the directory where the files are.'); \n}\n";
			startupString += "var atv_" + this.ClientID + "_de = '" + Utils.ConvertToImageDir(this.ImagesDirectory,this.Icons.Default,"folder.gif",this.Page,this.GetType()) + "';\n";
			startupString += "var atv_" + this.ClientID + "_ex = '" + Utils.ConvertToImageDir(this.ImagesDirectory,this.Icons.Expanded,"minus.gif",this.Page,this.GetType()) + "';\n";
			startupString += "var atv_" + this.ClientID + "_co = '" + Utils.ConvertToImageDir(this.ImagesDirectory,this.Icons.Collapsed,"plus.gif",this.Page,this.GetType()) + "';\n";
			startupString += "var atv_" + this.ClientID + "_path = '" + Path + "';\n";
			startupString += "</script>\n";

			page.RegisterStartupScript("ActiveTree_Startup_" + this.ClientID, startupString);
		}

		private void RenderTreeview(HtmlTextWriter output/*, LicenseStatus licenstStatus*/)
		{
			//Page.Trace.Write("Rendering TreeView");

			//output.Write("<input type=\"hidden\" name=\"atv_" + UniqueID + "_curSelNode\" id=\"atv_" + ClientID + "_curSelNode\" value=\"" + "" + "\">");
			//output.Write("<input type=\"hidden\" name=\"atv_" + UniqueID + "_curSelStyle\" id=\"atv_" + ClientID + "_curSelStyle\" value=\"" + "" + "\">");

			// A workaround before finding a solution
			/*output.AddAttribute(HtmlTextWriterAttribute.Type,"Hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Name,UniqueID);
			output.AddAttribute(HtmlTextWriterAttribute.Value,DateTime.Now.Ticks.ToString());
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();*/

			output.AddAttribute(HtmlTextWriterAttribute.Type,"Hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Name,this.UniqueID + "PostBackRef");
			output.AddAttribute(HtmlTextWriterAttribute.Value,this.Page.GetPostBackEventReference(this,""));
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			output.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("atv_{0}_curSelNode",this.ClientID));
			output.AddAttribute(HtmlTextWriterAttribute.Name, string.Format("atv_{0}_curSelNode",this.UniqueID));
			output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
			this.NodesStyleSelected.AddAttributesToRender(output);
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_curSelNodeStyleOriginal");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_curSelNodeStyleOriginal");
			output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
			this.NodesStyleSelected.AddAttributesToRender(output);
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_nodesStyleSelected");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_nodesStyleSelected");
			output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
			this.NodesStyleSelected.AddAttributesToRender(output);
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			this.ControlStyle.AddAttributesToRender(output);
			IEnumerator enumerator = this.Style.Keys.GetEnumerator();
			while(enumerator.MoveNext())
				output.AddStyleAttribute((string)enumerator.Current, this.Style[(string)enumerator.Current]);
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_contents");

			output.AddAttribute("onselectstart", "return false;");
			output.AddAttribute("ondragstart", "return false;");
			output.RenderBeginTag(HtmlTextWriterTag.Table); // Open Table 1
			output.RenderBeginTag(HtmlTextWriterTag.Tr); // Open Tr 1
			output.RenderBeginTag(HtmlTextWriterTag.Td); // Open Td 1

			if (this.DisplayMasterNode == true)
				base.Render(output);
			else
				base.RenderChildren(output);

			output.RenderEndTag(); // Close Td 1
			output.RenderEndTag(); // Close Tr 1

			//output.Write(CheckLicense());
			/*
							#if (TRIAL)

							output.RenderBeginTag(HtmlTextWriterTag.Tr);
							output.RenderBeginTag(HtmlTextWriterTag.Td);
							output.RenderBeginTag(HtmlTextWriterTag.Table);
							output.RenderBeginTag(HtmlTextWriterTag.Tr);
							output.AddAttribute(HtmlTextWriterAttribute.Align, "right");
							output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "red");
							output.RenderBeginTag(HtmlTextWriterTag.Td);
							output.Write("<font size=\"1\" color=\"white\">Register at <a href=\"http://www.activeup.com/?r=acl\"><font color=\"white\">Active Up</font></a></font>");
							output.RenderEndTag();
							output.RenderEndTag();
							output.RenderEndTag();
							output.RenderEndTag();
							output.RenderEndTag();
							#endif*/

			/*if (!(this.Parent is ActiveUp.WebControls.ToolCustomLinks))
			{
				output.Write(string.Format("<!--- Licensed to : {0} --->", licenstStatus.Info.Company));
			}*/

			output.RenderEndTag(); // Close Table 1
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			if (_panelWidth.Value > 0 && _panelHeight.Value > 0)
			{
				output.Write(string.Format("<div style=\"overflow:auto;height:{0};width:{1};\">",_panelHeight.ToString(),_panelWidth.ToString()));
			}

#if (LICENSE)

			/*LicenseProductCollection licenses = new LicenseProductCollection();
			licenses.Add(new LicenseProduct(ProductCode.AWC, 4, Edition.S1));
			licenses.Add(new LicenseProduct(ProductCode.ATV, 4, Edition.S1));
			ActiveUp.WebControls.Common.License license = new ActiveUp.WebControls.Common.License();
			LicenseStatus licenseStatus = license.CheckLicense(licenses, this.License);*/

			_useCounter++;

			if (!(this.Parent is ActiveUp.WebControls.Editor) && /*!licenseStatus.IsRegistered &&*/ Page != null && _useCounter == StaticContainer.UsageCount)
			{
				_useCounter = 0;
				output.Write(StaticContainer.TrialMessage);
			}
			else
			{
				this.RenderTreeview(output);
			}

#else
		RenderTreeview(output);
#endif

			if (_panelWidth.Value > 0 && _panelHeight.Value > 0)
			{
				output.Write("</div>");
			}

		}

		/// <summary>
		/// Load a treeview structure from a xml file.
		/// </summary>
		/// <param name="fileName">File path of the xml file.</param>
		public void LoadFromXml(string fileName)
		{
			TextReader reader = new StreamReader(fileName);
			XmlSerializer serializer = new XmlSerializer(typeof(ActiveUp.WebControls.Nodes)); 
			ActiveUp.WebControls.Nodes nodes = (ActiveUp.WebControls.Nodes)serializer.Deserialize(reader);
			reader.Close();

			if (nodes.NodesList.Count > 0)
			{
				foreach(ActiveUp.WebControls.Node n in nodes.NodesList)
				{
					if (n.ParentKey == "0")
					{
					   ActiveUp.WebControls.TreeNode node = this.AddNode(n.Key,n.Label,n.Link,n.Target,n.Expanded,n.Icon);
                       //this.Controls.Add(node);

					}

					else
					{
						ActiveUp.WebControls.TreeNode node = this.FindNode(n.ParentKey).AddNode(n.Key,n.Label,n.Link,n.Target,n.Expanded,n.Icon);
                        //this.Controls.Add(node);
					}
				}
			}
		}

		private TreeNode AddElement(ref ActiveUp.WebControls.Nodes nodes,TreeNode newNode)
		{
			if (newNode != null)
				nodes.NodesList.Add(new ActiveUp.WebControls.Node(newNode.Key,newNode.Parent.ID,newNode.Text,newNode.Link));

			if (this.Nodes.Count > 0)
			{
				foreach(TreeNode node in this.Nodes)
				{
					bool isAlreadyIn = false;
					for (int i = 0 ; i < nodes.NodesList.Count ; i++)
					{
						if (((ActiveUp.WebControls.Node)nodes.NodesList[i]).Key == node.Key)
						{
							isAlreadyIn = true;
							break;
						}
					}

					if (isAlreadyIn == false)
						if (this.AddElement(ref nodes,node) != null)
							return this.AddElement(ref nodes,node);
				}
			}

			return null;
		}

		/// <summary>
		/// Gets or sets the nodes style selected.
		/// </summary>
		/// <value>The nodes style selected.</value>
		[
		Bindable(true), 
		Category("TreeView-Nodes"), 
		DefaultValue(""),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Description("Style of the selected node.")
		] 
		public System.Web.UI.WebControls.Style NodesStyleSelected
		{
			get
			{	
				if (_nodesStyleSelected == null)
				{
					_nodesStyleSelected = new System.Web.UI.WebControls.Style();
					_nodesStyleSelected.Font.Size = FontUnit.XSmall;
					_nodesStyleSelected.BackColor = Color.Red;
				}
				return _nodesStyleSelected;
			}

			set
			{
				_nodesStyleSelected = value;
			}
		}


		/// <summary>
		/// Gets or sets a value indicating whether use same click event.
		/// </summary>
		/// <value><c>true</c> if use same click event; otherwise, <c>false</c>.</value>
		[
			Bindable(false),
			Category("Behavior"),
			Description("Gets or sets the value indicates if the click events must be the same for each nodes."),
			DefaultValue(true)
		]
		public bool UseSameClickEvent
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(UseSameClickEvent), true);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(UseSameClickEvent), value);
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if you want to auto check/uncheck children when this node checked/unchecked.
		/// </summary>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue(true),
		Browsable(true)
		] 
		public bool AutoCheckChildren 
		{
			get
			{
				return _autoCheckChildren;
			}

			set
			{
				_autoCheckChildren = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if you want to gray parent node when not all children are checked.
		/// </summary>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue(false),
		Browsable(true)
		] 
		public bool SelectedGrayed 
		{
			get
			{
				return _selectedGrayed;
			}

			set
			{
				_selectedGrayed = value;
			}
		}

		/// <summary>
		/// Gets or sets the panel width.
		/// </summary>
		/// <value>The panel width.</value>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue("Panel width."),
		Browsable(true)
		] 
		public Unit PanelWidth
		{
			get
			{
				return _panelWidth;
			}

			set
			{
				_panelWidth = value;
			}
		}

		/// <summary>
		/// Gets or sets the panel height.
		/// </summary>
		/// <value>The panel height.</value>
		[
		Bindable(true), 
		Category("Behavior"), 
		DefaultValue(""),
		Browsable(true)
		] 
		public Unit PanelHeight
		{
			get
			{
				return _panelHeight;
			}

			set
			{
				_panelHeight = value;
			}
		}


		/// <summary>
		/// Gets or sets the value indicating if you want to gray parent node when not all children are checked.
		/// </summary>
		[
		Bindable(false),
		Browsable(true),
		Category("Load on demand"),
		Description("Gets or sets the value indicates if you want the dynamic load for the nodes."),
		DefaultValue(false)
		]
		public bool LoadOnDemand
		{
			get
			{
				object loadOnDemand = ViewState["LoadOnDemand"];
				if (loadOnDemand != null)
					return (bool)loadOnDemand;
				else
					return false;
			}

			set
			{
				ViewState["LoadOnDemand"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the path indicates where the LoadOnDemand.aspx event handler is located.
		/// </summary>
		/// <value>The path.</value>
		[
		Bindable(false),
		Browsable(true),
		Category("Load on demand"),
		Description("Gets or sets the path indicates where the LoadOnDemand.aspx event handler is located."),
		DefaultValue("")
		]
		public string Path
		{
			get
			{
				object path = ViewState["Path"];
				if (path != null)
					return (string)path;
				else
					return string.Empty;
			}

			set
			{
				ViewState["Path"] = value;
			}
		}

		/// <summary>
		/// Renders the nodes on demand.
		/// </summary>
		/// <param name="parent">The node parent.</param>
		/// <param name="output">The output.</param>
		public static void RenderNodesOnDemand(TreeNode parent, HtmlTextWriter output)
		{
			foreach(TreeNode node in parent.Nodes)
			{
				node.RenderControl(output);
			}
		}

	}
}
