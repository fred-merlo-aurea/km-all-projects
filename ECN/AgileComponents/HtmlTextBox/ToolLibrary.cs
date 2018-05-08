using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
    /// <summary>
    /// Represents a <see cref="ToolLibrary"/> object.
    /// </summary>
    [
    Serializable,
    ToolboxItem(false)
    ]
#if (!FX1_1)
    public class ToolLibrary : ToolButton, INamingContainer, IPostBackEventHandler, IPostBackDataHandler, ICallbackEventHandler
#else
    public class ToolLibrary : ToolButton, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
#endif
    {
        private const string DeleteFile = "DELETEFILE:";
        private const string EventArgument = "__EVENTARGUMENT";
        private const string CallServer = "CallServer";
        private const string Ie = "IE";
        private const string HtbReceiveServerData = "HTB_ReceiveServerData";
        private const string Arg = "arg";
        private const string FuncCallServer = "function CallServer(arg, context){{{0};}}";
        private bool _mustBeRendered = true;
        private TreeView _treeDirectories = null;
        private bool _expandedTree = true;

        /// <summary>
        /// Upload event.
        /// </summary>
        public event EventHandler Upload;
        /// <summary>
        /// Delete event.
        /// </summary>
        public event EventHandler Delete;
        private bool _uniqueFilenames, _showErrors = true, _showSuccess = false,/*_showPopup=true,*/_createThumbnail, _constrainProportions, _resizeSmaller;
        private int _uploadMaxSize;
        private string _thumbnailPrefix;
        private Size _maxSize, _thumbnailSize;
        private string _currentSelDirPath;
        private string _currentSelDirId = string.Empty;
        private string _rootPathID;
        private string _localEditorId = string.Empty;
        //private HtmlInputFile inputFile = new HtmlInputFile();
        protected string returnValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolLibrary"/> class.
        /// </summary>
        public ToolLibrary()
            : base()
        {
            _Init(string.Empty, string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolLibrary"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public ToolLibrary(string id)
            : base(id)
        {
            _Init(id, string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolLibrary"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="iconsDirectory">The icons directory.</param>
        public ToolLibrary(string id, string iconsDirectory)
            : base(id)
        {
            _Init(id, iconsDirectory);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var file = Page.Request.Params.Get(EventArgument);

            //file deleted
            if (!string.IsNullOrEmpty(file) && file.IndexOf(DeleteFile) != -1)
            {
                var eventArgs = DeleteImage(file.Remove(0, DeleteFile.Length));
                OnDelete(eventArgs);
            }

            //file uploaded
            if (Page.Request.Files.Count > 0)
            {
                foreach (FileDirectory dir in Directories)
                {
                    var eventArgs = UploadImage(dir.Path);
                    OnUpload(eventArgs);
                }
            }

            if (Fx1ConditionalHelper<bool>.GetFx1ConditionalValue(true, false))
            {
                var cbReference = Page.ClientScript.GetCallbackEventReference(this, Arg, HtbReceiveServerData, string.Empty);
                var callbackScript = string.Format(FuncCallServer, cbReference);
                Page.ClientScript.RegisterClientScriptBlock(GetType(), CallServer, callbackScript, true);
            }

            var isNs6 = string.Equals(Page.Request.Browser.Browser, Ie, StringComparison.OrdinalIgnoreCase);

            var content = new StringBuilder();

            AppendTableClsPopupHeader(content);
            AppendTableNs6(content, isNs6);
            AppendTableClsPopupMiddle(content);
            AppendTableClsPopupFooter(content);

            PopupContents.ContentText = content.ToString();
        }

        private static void AppendTableClsPopupHeader(StringBuilder content)
        {
            content.Append("<table class=HTB_clsPopup cellpadding=0 cellspacing=0>");
            content.Append("	<tr><td height=0></td></tr>");
            content.Append("	<tr valign=top>");
            content.Append("		<td>");
            content.Append("			<table cellpadding=0 cellspacing=0>");
            content.Append("				<tr>");
            content.Append("					<td>");
            content.Append("						<table cellpadding=0 cellspacing=0>");
            content.Append("							<tr>");
            content.Append("								<td width=\'5\'></td>");
            content.Append("								<td>");
            content.Append(
                "									<iframe id=\'$EDITOR_ID$_HTB_IU_frameTreeDirectories\' style=\'width:300;height:130;\'></iframe>");
            content.Append("								</td>");
            content.Append("							</tr>");
            content.Append("							<tr>");
            content.Append("								<td width=\'5\'></td>");
            content.Append("								<td>");
            content.Append(
                "									<iframe id=\'$EDITOR_ID$_HTB_IU_tableContents\' style=\'width:300;height:180;\'></iframe>");
            content.Append("								</td>");
            content.Append("							</tr>");
            content.Append("						</table>");
            content.Append("					</td>");
            content.Append("				</tr>");
        }

        private static void AppendTableNs6(StringBuilder content, bool isNs6)
        {
            content.Append("				<tr>");
            content.Append("					<td>");
            content.Append("						<table width=\'100%\' cellpadding=0 cellspacing=0>");
            content.Append("							<tr>");
            content.Append("								<td width=\'5\'></td>");
            content.Append("								<td><span>Upload in directory</span></td>");
            content.Append("							</tr>");

            if (isNs6)
            {
                content.Append("							<tr>");
                content.Append("								<td width=\'5\'></td>");
                content.Append("								<td valign=center>");
                content.Append("									<input type=\'file\' runat=\'server\' name=\'" + "$EDITOR_ID$" +
                               "_HTB_IU_uploadInDirectory\' id=\'" + "$EDITOR_ID$" +
                               "_HTB_IU_uploadInDirectory\' style=\'height:24;width:300;\'>");
                content.Append("								    &nbsp;");
                content.Append("								</td>");
                content.Append("								<td valign=center></td>");
                content.Append("							</tr>");
                content.Append("							<tr align=center style=\'height:30\'>");
                content.Append("								<td width=\'5\'></td>");
                content.Append("								<td valign=center><input runat=\'server\' type=button value=Upload" + "$DISABLED$" +
                               "style=\'height:24px\' onclick=\\\"HTB_PopulateList(\\\'$EDITOR_ID$\\\')\\\"></td>");
                content.Append("							</tr>");
            }

            else
            {
                content.Append("							<tr>");
                content.Append("								<td width=\'5\'></td>");
                content.Append("								<td valign=center>");
                content.Append("								    &nbsp;<br/><br/><br/>");
                content.Append("								</td>");
                content.Append("								<td valign=center></td>");
                content.Append("							</tr>");
                content.Append("							<tr align=center style=\'height:30\'>");
                content.Append("								<td width=\'5\'></td>");
                content.Append("								<td valign=center><input type=button value=Upload" + "$DISABLED$" +
                               "style=\'height:24px\' onclick=\\\"$POSTBACK$\\\"></td>");
                content.Append("							</tr>");
            }

            content.Append("						</table>");
            content.Append("					</td>");
            content.Append("				</tr>");
            content.Append("			</table>");
            content.Append("		</td>");
        }

        private static void AppendTableClsPopupMiddle(StringBuilder content)
        {
            content.Append("		<td valign=top>");
            content.Append("			<table cellpadding=0 cellspacing=0>");
            content.Append("				<tr>");
            content.Append("					<td>");
            content.Append("						<table width=\'100%\' cellpadding=0 cellspacing=0>");
            content.Append("							<tr>");
            content.Append("								<td width=\'5\'></td>");
            content.Append("								<td>");
            content.Append(
                "									<table width=\'220\' align=\'center\' cellpadding=\'0\' cellspacing=\'0\' style='font-family:calibri;border-width:1px;border-color:#808080;border-style:solid;'>");
            content.Append("										<tr height=\'150\'>");
            content.Append("											<td valign=\'middle\' align=\'center\'>");
            content.Append(
                "												<img src=\'\' width=\'0\' height=\'0\' name=\'$EDITOR_ID$_HTB_IU_preview\' id=\'$EDITOR_ID$_HTB_IU_preview\'>");
            content.Append("											</td>");
            content.Append("										</tr>");
            content.Append("										<tr>");
            content.Append("											<td>");
            content.Append("												<table width=\'100%\'>");
            content.Append("													<tr>");
            content.Append("														<td><span>General</span></td>");
            content.Append(
                "														<td width=\'100%\'><hr size=\'2\' width=\'100%\' style=\'color:#808080;height:1px\'>");
            content.Append("														</td>");
            content.Append("													</tr>");
            content.Append("												</table>");
            content.Append("												<table width=\'100%\'>");
            content.Append("													<td width=\'5\'></td>");
            content.Append("													<td>Text :</td>");
            content.Append("													<td align=\'right\'>");
            content.Append("													<input type=\'text\' size=\'22\' id=\'$EDITOR_ID$_HTB_IU_text\'></td>");
            content.Append("												</table>");
            content.Append("											</td>");
            content.Append("										</tr>");
            content.Append("										<tr>");
            content.Append("											<td>");
            content.Append("												<table width=\'100%\'>");
            content.Append("													<tr>");
            content.Append("														<td><span>Disposition</span></td>");
            content.Append(
                "														<td width=\'100%\'><hr size=\'2\' width=\'100%\' style=\'color:#808080;height:1px\'>");
            content.Append("														</td>");
            content.Append("													</tr>");
            content.Append("												</table>");
            content.Append("												<table width=\'100%\'>");
            content.Append("													<td width=\'5\'></td>");
            content.Append("													<td>Alignment :</td>");
            content.Append("													<td>");
            content.Append("														<select style=\'width:106;height:22\' id=\'$EDITOR_ID$_HTB_IU_alignment\'>");
            content.Append("															<option value=\'default\'>Default</option>");
            content.Append("															<option value=\'absbottom\'>AbsBottom</option>");
            content.Append("															<option value=\'absmiddle\'>AbsMiddle</option>");
            content.Append("															<option value=\'baseline\'>BaseLine</option>");
            content.Append("															<option value=\'bottom\'>Bottom</option>");
            content.Append("															<option value=\'left\'>Left</option>");
            content.Append("															<option value=\'middle\'>Middle</option>");
            content.Append("															<option value=\'right\'>Right</option>");
            content.Append("															<option value=\'texttop\'>TextTop</option>");
            content.Append("															<option value=\'top\'>Top</option>");
            content.Append("														</select>");
            content.Append("													</td>");
            content.Append("												</table>");
        }

        private static void AppendTableClsPopupFooter(StringBuilder content)
        {
            content.Append("												<table width=\'100%\'>");
            content.Append("													<td width=\'5\'></td>");
            content.Append("													<td>Border Thickness :</td>");
            content.Append("													<td align=\'right\'>");
            content.Append("													<input type=\'text\' size=\'6\' id=\'$EDITOR_ID$_HTB_IU_borderThikness\'></td>");
            content.Append("												</table>");
            content.Append("												<table width=\'100%\'>");
            content.Append("													<td width=\'5\'></td>");
            content.Append("													<td>Horizontal Spacing :</td>");
            content.Append("													<td align=\'right\'>");
            content.Append(
                "													<input type=\'text\' size=\'6\' id=\'$EDITOR_ID$_HTB_IU_horizontalSpacing\'></td>");
            content.Append("												</table>");
            content.Append("												<table width=\'100%\'>");
            content.Append("													<td width=\'5\'></td>");
            content.Append("													<td>Vertical Spacing :</td>");
            content.Append("													<td align=\'right\'>");
            content.Append("													<input type=\'text\' size=\'6\' id=\'$EDITOR_ID$_HTB_IU_verticalSpacing\'></td>");
            content.Append("												</table>");
            content.Append("											</td>");
            content.Append("										</tr>");
            content.Append("									</table>");
            content.Append("								</td>");
            content.Append("							</tr>");
            content.Append("						</table>");
            content.Append("					</td>");
            content.Append("				</tr>");
            content.Append("				<tr>");
            content.Append("					<td>");
            content.Append("						<table width=\'100%\'>");
            content.Append(
                "							<td align=\'center\'><input type=\'button\' value=\'Add Image\' onclick=\\\"HTB_IU_AddImage(\\\'$EDITOR_ID$\\\');\\\"></td>");
            content.Append("						</table>");
            content.Append("					</td>");
            content.Append("				</tr>");
            content.Append("			</table>");
            content.Append("		</td>");
            content.Append("		<td width=8></td>");
            content.Append("	</tr>");
            content.Append("</table>");
        }

        private void _Init(string id, string iconsDirectory)
        {
            this.Load += new System.EventHandler(this.Page_Load);
   
            if (id == string.Empty)
                this.ID = "_toolLibrary" + Editor.indexTools++;
            else
                this.ID = id;

            if (iconsDirectory != string.Empty)
                this.IconsDirectory = iconsDirectory;

            //this.PopupContents.ID = ID + "Popup";
#if (!FX1_1)
            this.ImageURL = string.Empty;
            this.OverImageURL = string.Empty;
#else
			this.ImageURL = "library_off.gif";
			this.OverImageURL = "library_over.gif";
#endif
            this.ToolTip = "Image Library";
            this.UsePopupOnClick = true;
            this.PopupContents.Width = 560;
            this.PopupContents.Height = 423;
            this.PopupContents.TitleText = "Image Library";

            _treeDirectories = new TreeView();
            //_treeDirectories.ID = ID + "Tree";
            _treeDirectories.DisplayMasterNode = true;
            _treeDirectories.NodesStyleSelected.Font.Bold = true;
            _treeDirectories.DisplayMasterNode = false;
            _treeDirectories.CssClass = "HTB_clsPopup";
            _treeDirectories.ImagesDirectory = this.IconsDirectory;

            this.ClientSideClick = "/*HTB_SetPopupPosition('$EDITOR_ID$','$POPUP_ID$');*/HTB_InitImageLibrary('$EDITOR_ID$'); ";

            this.Controls.Add(_treeDirectories);
        }

        /// <summary>
        /// Renders at the design time.
        /// </summary>
        /// <param name="output">The output.</param>
        public override void RenderDesign(HtmlTextWriter output)
        {
#if (!FX1_1)
            if (ImageURL == string.Empty)
                this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.library_off.gif");
            if (OverImageURL == string.Empty)
                this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.library_over.gif");
#endif

            this.RenderControl(output);
        }

        /// <summary>
        /// Notifies the tool to perform any necessary prerendering steps prior to saving view state and rendering content.
        /// </summary>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
                  
#if (!FX1_1)
            if (ImageURL == string.Empty)            
                this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.library_off.gif");
           if (OverImageURL == string.Empty)
                this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.library_over.gif");
#endif

            base.OnPreRender(e);

            if (((base.Page != null) && base.Enabled))
            {
                Page.RegisterRequiresPostBack(this);
                Page.RegisterHiddenField(UniqueID, DateTime.Now.ToString());
                RegisterAPIScriptBlock();
            }

            Editor editor = (Editor)this.Parent.Parent.Parent;
            _localEditorId = editor.ClientID;

            this.PopupContents.ID = editor.ClientID + "_Popup";
            _treeDirectories.ID = editor.ClientID + "_Tree";

            this.ClientSideClick = this.ClientSideClick.Replace("$EDITOR_ID$", editor.ClientID);
            this.ClientSideClick = this.ClientSideClick.Replace("$POPUP_ID$", this.PopupContents.ClientID);
            this.PopupContents.ContentText = this.PopupContents.ContentText.Replace("$EDITOR_ID$", editor.ClientID);
            //string eventargs = string.Format("HTB_IU_GetCurrentSelectedDir('{0}')",editor.ClientID);
            string eventargs = "HTB_IU_GetCurrentSelectedDir($EDITOR_ID$)";           

            this.PopupContents.ContentText = this.PopupContents.ContentText.Replace("$POSTBACK$", this.Page.GetPostBackEventReference(this, eventargs).Replace(@"'", @"\'"));
            this.PopupContents.ContentText = this.PopupContents.ContentText.Replace("$DISABLED$", (UploadDisabled == true ? " disabled " : " "));
            this.PopupContents.ContentText = this.PopupContents.ContentText.Replace(@"\'" + eventargs + @"\'", eventargs);
            this.PopupContents.ContentText = this.PopupContents.ContentText.Replace("$EDITOR_ID$", @"\'" + editor.ClientID + @"\'");

            

            // Check for proper EncType
            foreach (Control control in Page.Controls)
            {
                if (control.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlForm")
                {
                    System.Web.UI.HtmlControls.HtmlForm form = (System.Web.UI.HtmlControls.HtmlForm)control;

                    if (form.Enctype.ToLower() != "multipart/form-data")
                        form.Enctype = "multipart/form-data";

                    break;
                }
            }

            bool isNs6 = false;
            System.Web.HttpBrowserCapabilities browser = Page.Request.Browser;
            if (browser.Browser.ToUpper().IndexOf("NETSCAPE") != -1)
                isNs6 = true;

            if (isNs6)
                _mustBeRendered = false;
            else
                _mustBeRendered = true;

        }

        /// <summary>
        /// Registers the API script block.
        /// </summary>
        public void RegisterAPIScriptBlock()
        {
            _treeDirectories.ScriptDirectory = ((Toolbar)this.Parent).ScriptDirectory;
            _treeDirectories.ImagesDirectory = this.IconsDirectory;
            _treeDirectories.RegisterAPIScriptBlock(this.Page);
        }

        /// <summary>
        /// Sends the Popup content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
        protected override void Render(HtmlTextWriter output)
        {
            if (_mustBeRendered)
            {
                _treeDirectories.LoadOnDemand = LoadOnDemand;
                if (LoadOnDemand == true)
                {
                    _treeDirectories.Path = PathLoadOnDemand;
                    Page.Session[_treeDirectories.UniqueID.Replace(":", "_")] = _treeDirectories;

                }
                _treeDirectories.Expanded = ExpandedTree;
                _treeDirectories.Icons = TreeIcons;

                Editor editor = (Editor)this.Parent.Parent.Parent;

                output.AddAttribute(HtmlTextWriterAttribute.Id, editor.ClientID + "_HTB_IU_curSelDirFullPath");
                output.AddAttribute(HtmlTextWriterAttribute.Name, editor.ClientID + "_HTB_IU_curSelDirFullPath");
                output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                output.AddAttribute(HtmlTextWriterAttribute.Value, "");
                output.RenderBeginTag(HtmlTextWriterTag.Input); 
                output.RenderEndTag();

                /*output.AddAttribute(HtmlTextWriterAttribute.Id, editor.ClientID + "_HTB_IU_curSelDirId");
                output.AddAttribute(HtmlTextWriterAttribute.Name, editor.ClientID + "_HTB_IU_curSelDirId");
                output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                output.AddAttribute(HtmlTextWriterAttribute.Value, "");
                output.RenderBeginTag(HtmlTextWriterTag.Input); 
                output.RenderEndTag();*/                

                if (HttpContext.Current != null)
                {
                    output.AddAttribute(HtmlTextWriterAttribute.Id, editor.ClientID + "_HTB_IU_curServerPath");
                    output.AddAttribute(HtmlTextWriterAttribute.Name, editor.ClientID + "_HTB_IU_curServerPath");
                    output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                    output.AddAttribute(HtmlTextWriterAttribute.Value, HttpContext.Current.Server.MapPath(".").Replace(@"\", @"\\"));
                    output.RenderBeginTag(HtmlTextWriterTag.Input); 
                    output.RenderEndTag();
                }

                /*			FileDirectory f = new FileDirectory();
                            f.WebPath = @"http://localhost/webapplication56";
                            f.Path = @"e:\inetpub\wwwroot\webapplication56";
                            f.AutoLoadFilesAndSubDirectories = true;
                            Directories.Add(f);*/

                if (Directories.Count == 0)
                {
                    if (HttpContext.Current != null)
                    {
                        string path = HttpContext.Current.Server.MapPath(".");
                        PopulateTree(_treeDirectories, "Root", path, "", output, editor.ClientID);
                    }
                }
                else
                {

                    foreach (FileDirectory dir in Directories)
                    {
                        if (dir.Path != string.Empty && dir.WebPath != string.Empty)
                        {
                            if (dir.Files.Count == 0)
                                PopulateTree(_treeDirectories, dir.Name, dir.Path, dir.WebPath, output, editor.ClientID);
                        }
                    }
                }

                /*output.AddStyleAttribute("left","10");
                output.AddStyleAttribute("position","absolute");
                output.AddStyleAttribute("display","none");
                output.RenderBeginTag(HtmlTextWriterTag.Div);*/
                output.Write("<div style=\"left: none; position: absolute; display: none;\">");
                this.RenderChildren(output);
                //output.RenderEndTag();
                output.Write("</div>");

                if (_rootPathID != null && _rootPathID != string.Empty)
                    this.ClientSideClick += string.Format("HTB_IU_FillFilesContent('{0}','{1}','parent.{2}','{3}','{4}','{5}');", editor.ClientID, _rootPathID, Page.GetPostBackEventReference(this, "$FILETODEL$").Replace(@"'", @"\'"), IconsDirectory, DeleteIcon, DeleteButtonDisabled);
                base.Render(output);
                this.PopupContents.RenderControl(output);

                output.Write("\n<script language=\"javascript\">\n");
                output.Write(string.Format("\nfunction InitToolLibrary_{0}(e)\n", ClientID));
                output.Write("{\n");
                //output.Write("alert('{0}');\n", IconsDirectory);
                output.Write(string.Format("var treeDirectories = document.getElementById('{0}_contents').outerHTML\n", _treeDirectories.ClientID));
                output.Write(string.Format("treeDirectories = HTB_ReplaceAllOccurence(treeDirectories,'toggleNode(','parent.HTB_ToggleNode(\\\'{0}_HTB_IU_frameTreeDirectories\\\',');\n", editor.ClientID));
                output.Write(string.Format("treeDirectories = HTB_ReplaceAllOccurence(treeDirectories,'selectNode(','parent.HTB_IU_SelectNode(\\\'{0}_HTB_IU_frameTreeDirectories\\\',');\n", editor.ClientID));
                output.Write(string.Format("treeDirectories = HTB_ReplaceAllOccurence(treeDirectories,'loadNode(','parent.HTB_LoadNode(\\\'{0}_HTB_IU_frameTreeDirectories\\\',');\n", editor.ClientID));
                output.Write(string.Format("document.getElementById('{0}_HTB_IU_frameTreeDirectories').contentWindow.document.write(treeDirectories);", editor.ClientID));
                output.Write(string.Format("document.getElementById('{0}_contents').outerHTML = '';\n", _treeDirectories.ClientID));
                output.Write(string.Format("ATB_SetIsImageLibrary('{0}','{1}_divUploadInDirectory');", PopupContents.ID, editor.ClientID));

                /*if (PopupMustBeShow)
                {
                    output.Write(string.Format("ATB_showPopup('{0}');",this.PopupContents.ID));
                    output.Write(string.Format("HTB_IU_FillFilesContent('{0}','parent.{1}');",_currentSelDirId,Page.GetPostBackEventReference(this,"$FILETODEL$").Replace(@"'",@"\'")));
                    PopupMustBeShow = false;
                }*/

                output.Write("}\n");
                output.Write(string.Format("window.RegisterEvent(\"onload\", InitToolLibrary_{0});\n", ClientID));
                output.Write("\n</script>\n");
            }
        }

        private void PopulateTree(TreeNode node, string name, string path, string webPath, HtmlTextWriter output, string editorid)
        {
            Page.Trace.Write(name + "-" + path);
            if (node is ActiveUp.WebControls.TreeView)
            {
                TreeNode newNode = new TreeNode();
                newNode.Key = DateTime.Now.Ticks.ToString();
                if (name != string.Empty)
                    newNode.Text = name;
                else
                    newNode.Text = webPath;
                //newNode.Icon = @"icons/de.gif"; 
                newNode.ID = Guid.NewGuid().ToString().Replace("-", "_");
                newNode.Link = string.Format("javascript:parent.HTB_IU_FillFilesContent('{0}','{1}','parent.{2}','{3}','{4}','{5}')", editorid, newNode.ID, Page.GetPostBackEventReference(this, "$FILETODEL$").Replace(@"'", @"\'"), IconsDirectory, DeleteIcon, DeleteButtonDisabled);
                if (name == "Root" || _treeDirectories.Nodes.Count == 0)
                    newNode.Expanded = ExpandedTree;
                else
                    newNode.Expanded = false;
                node = node.AddNode(newNode);

                if (_rootPathID == string.Empty || _rootPathID == null)
                    _rootPathID = newNode.ID;

                if (_currentSelDirPath == path.Replace(@"\", @"\\"))
                    _currentSelDirId = newNode.ID;
            }

            string[] files = Directory.GetFileSystemEntries(path);

            string filesInfo = string.Empty;
            for (int index = 0; index < files.Length; index++)
            {
                FileInfo fileInfo = new FileInfo(files[index]);

                TreeNode newNode = new TreeNode();
                newNode.Key = DateTime.Now.Ticks.ToString();
                newNode.Text = fileInfo.Name;
                newNode.Expanded = false;

                if ((System.IO.File.GetAttributes(files[index]) & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    //newNode.Icon = @"icons/de.gif"; 
                    newNode.ID = Guid.NewGuid().ToString().Replace("-", "_");
                    newNode.Link = string.Format("javascript:parent.HTB_IU_FillFilesContent('{0}','{1}','parent.{2}','{3}','{4}','{5}')", editorid, newNode.ID, Page.GetPostBackEventReference(this, "$FILETODEL$").Replace(@"'", @"\'"), IconsDirectory, DeleteIcon, DeleteButtonDisabled);
                    node.AddNode(newNode);
                    PopulateTree(newNode, string.Empty, fileInfo.FullName, webPath + "/" + fileInfo.Name, output, editorid);

                    string fullFileName = fileInfo.DirectoryName.Replace(@"\", @"\\") + @"\\" + fileInfo.Name;
                    if (_currentSelDirPath == fullFileName)
                        _currentSelDirId = newNode.ID;
                }
                else
                {
                    if (ImageExtension.Contains(fileInfo.Extension))
                    {
                        try
                        {
                            Bitmap image = new Bitmap(fileInfo.FullName);
                            filesInfo += string.Format("{0}|{1}|{2}|{3}|{4}", fileInfo.Name, ConvertSizeToInformaticUnit(fileInfo.Length), fileInfo.Extension, image.Width, image.Height);
                            filesInfo += ";";
                            image.Dispose();
                        }
                        catch { }
                    }
                }

            }

            output.AddAttribute(HtmlTextWriterAttribute.Id, editorid + "_HTB_IU_dir_" + node.ID);
            output.AddAttribute(HtmlTextWriterAttribute.Type, "Hidden");
            filesInfo = filesInfo.TrimEnd(';');
            output.AddAttribute(HtmlTextWriterAttribute.Name, webPath.Replace(@"\", @"\\").TrimStart('/'));
            output.AddAttribute(HtmlTextWriterAttribute.Value, filesInfo);
            output.RenderBeginTag(HtmlTextWriterTag.Input); 
            output.RenderEndTag();
        }

        /// <summary>
        /// Converts the size to informatic unit.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public string ConvertSizeToInformaticUnit(object size)
        {
            string result = string.Empty;

            try
            {
                double sizeInt = Convert.ToDouble(size);

                if (sizeInt >= 1073741824)
                    return string.Format("{0:.0}", sizeInt / 1073741824) + " GB";
                if (sizeInt >= 1048576)
                    return string.Format("{0:.0}", sizeInt / 1048576) + " MB";
                else if (sizeInt >= 1024)
                    return string.Format("{0:.0}", sizeInt / 1024) + " KB";
                else
                    return sizeInt.ToString() + " B";

            }
            catch
            {
                result = "N/A";
            }

            return result;
        }

        /// <summary>
        /// Gets or sets the image extension.
        /// </summary>
        /// <value>The image extension.</value>
        public ExtensionCollection ImageExtension
        {
            get
            {
                if (ViewState["_imageExtension"] == null)
                {
                    ExtensionCollection extensions = new ExtensionCollection();
                    extensions.Add(".png");
                    extensions.Add(".jpg");
                    extensions.Add(".gif");
                    ViewState["_imageExtension"] = extensions;
                }
                return (ExtensionCollection)ViewState["_imageExtension"];
            }

            set
            {
                ViewState["_imageExtension"] = value;
            }
        }

        /// <summary>
        /// Specify whether you want to automatically create a thumbnail when an image is uploaded.
        /// </summary>
        /// <remarks>Note: if you load all the content of the directory where you upload the image, you will have both the thumbnail and the normal picture. You can specify the thumbnail prefix using the <see cref="ThumbnailPrefix"/> property.</remarks>
        public bool CreateThumbnail
        {
            get { return _createThumbnail; }
            set { _createThumbnail = value; }
        }

        /// <summary>
        /// Gets or sets the maximum size of a file in bytes to be accepted for upload.
        /// </summary>
        [Bindable(false),
        Category("Appearance"),
        Description("Gets or sets the maximum size of a file in bytes to be accepted for upload.")]
        public int UploadMaxSize
        {
            get
            {
                return _uploadMaxSize;
            }
            set
            {
                _uploadMaxSize = value;
            }
        }

        /// <summary>
        /// If true, makes sure the uploaded image filename is unique by adding random numbers. 
        /// </summary>
        [Bindable(false),
        Category("Behavior"),
        Description("If true, makes sure the uploaded image filename is unique by adding random numbers. ")]
        public bool UniqueFilenames
        {
            get
            {
                return _uniqueFilenames;
            }
            set
            {
                _uniqueFilenames = value;
            }
        }

        /// <summary>
        /// Lets you specify if you want to display error message to the end-user using the JScript message boxes.
        /// </summary>
        [Bindable(false),
        Category("Behavior"),
        Description("Lets you specify if you want to display error message to the end-user using the JScript message boxes.")]
        public bool ShowErrors
        {
            get
            {
                return _showErrors;
            }
            set
            {
                _showErrors = value;
            }
        }

        /// <summary>
        /// Lets you specify if you want to display success message to the end-user using the JScript message boxes.
        /// </summary>
        [Bindable(false),
        Category("Behavior"),
        Description("Lets you specify if you want to display success message to the end-user using the JScript message boxes.")]
        public bool ShowSuccess
        {
            get
            {
                return _showSuccess;
            }
            set
            {
                _showSuccess = value;
            }
        }

        /// <summary>
        /// Enables the control to process an event raised when a form is posted to the server.
        /// </summary>
        /// <param name="eventArgument">A String that represents an optional event argument to be passed to the event handler.</param>
        void IPostBackEventHandler.RaisePostBackEvent(String eventArgument)
        {

            Page.Trace.Write(this.ID, "RaisePostBackEvent...");
            base.RaisePostBackEvent(eventArgument);

            string delArgument = "DELETEFILE:";
            if (eventArgument.IndexOf(delArgument) == -1)
            {
                ImageUploadedEventArgs eventArgs = UploadImage(eventArgument);
                OnUpload(eventArgs);
            }
            else
            {
                ImageDeleteEventArgs eventArgs = DeleteImage(eventArgument.Substring(eventArgument.IndexOf("DELETEFILE:") + delArgument.Length));
                OnDelete(eventArgs);
            }

        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            Page.Trace.Write(this.ID, "LoadPostData...");

            Editor editor = (Editor)this.Parent.Parent.Parent;
            _currentSelDirPath = postCollection[editor.ClientID + "_HTB_IU_curSelDirFullPath"]; 
            //_currentSelDirPath = postCollection[editor.ClientID + "_HTB_IU_uploadInDirectory"];

            _currentSelDirId = postCollection[editor.ClientID + "_HTB_IU_curSelDirId"];

            return true;
        }

        /// <summary>
        /// Notify the ASP.NET application that the state of the control has changed.
        /// </summary>
        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            Page.Trace.Write(this.ID, "RaisePostDataChangedEvent...");
        }

        private ImageDeleteEventArgs DeleteImage(string image)
        {
            Page.Trace.Write("DeleteImage called");

            ImageDeleteEventArgs e = new ImageDeleteEventArgs();
            if (image != string.Empty)
            {
                e.Filename = image;
                e.Directory = image.Substring(0, image.LastIndexOf(@"\"));

                try
                {
                    if (System.IO.File.Exists(image))
                    {
                        System.IO.File.Delete(image);

                        /*if (_showPopup)
                        {
                            PopupMustBeShow = true;
                        }*/

                        if (_showSuccess)
                            Page.RegisterStartupScript("SHOW_SUCCESS", "<script language=\"javascript\">alert('Your image was deleted successfully.')</script>");

                        e.Success = true;
                    }
                }
                catch (Exception ex)
                {
                    Page.Trace.Write(ex.ToString());

                    if (_showErrors)
                        Page.RegisterStartupScript("SHOW_ERROR", string.Format("<script language=\"javascript\">alert('{0}.')</script>", ex.Message));
                }
                e.Success = false;
            }
            e.Success = false;
            return e;
        }

        private ImageUploadedEventArgs UploadImage(string directory)
		{
			Page.Trace.Warn(this.ID,string.Format("Requested files count : {0}",Page.Request.Files.Count));
			Page.Trace.Warn(this.ID,string.Format("Current selected dir : {0}",directory));

			ImageUploadedEventArgs e = new ImageUploadedEventArgs();
			
			e.Directory = directory; 
						
			if (Page.Request.Files != null && Page.Request.Files.Count > 0)
			{
				int indexRequestedFile = -1;
				for(int i = 0 ;  i < Page.Request.Files.Count ; i++)
				{
					if (Page.Request.Files.Keys[i].IndexOf(_localEditorId + "_HTB_IU_uploadInDirectory") >= 0)
					{
						indexRequestedFile = i;
						break;
					}
				}

				if (indexRequestedFile != -1)
				{
					string fullpath = Page.Request.Files[indexRequestedFile].FileName;
					if (fullpath.Length > 0)
					{
						string filename = fullpath.Substring(fullpath.LastIndexOf("\\") + 1, fullpath.Length - fullpath.LastIndexOf("\\") - 1);
						int filesize = Page.Request.Files[indexRequestedFile].ContentLength;                                              

						if (this.UniqueFilenames)
							filename = DateTime.Today.Subtract(new DateTime(2002,1,1)).Days.ToString().PadLeft(4, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "_" + filename;

						Page.Trace.Warn("directory" + directory);

						if (!System.IO.Path.IsPathRooted(directory))
							directory = Page.MapPath(directory);

						string serverpath = directory.TrimEnd('\\') + "\\" + filename.Replace("'", string.Empty);
						
						Page.Trace.Warn("serverpath:" + serverpath);
						e.Filename = serverpath;
						e.Size = filesize;

						if ((filesize < _uploadMaxSize || _uploadMaxSize == 0) && filename.Length > 0 && this.ImageExtension.Contains(System.IO.Path.GetExtension(filename).ToUpper()))
						{
							Page.Request.Files[indexRequestedFile].SaveAs(serverpath);
							
							if (this.CreateThumbnail)
								Resize(serverpath, directory.TrimEnd('\\') + "\\" + this.ThumbnailPrefix + filename, ThumbnailSize.Width, ThumbnailSize.Height, true);

							// Resize the image if needed
							if ((MaxSize.Width > 0 || MaxSize.Height > 0))
								Resize(serverpath, serverpath, MaxSize.Width, MaxSize.Height, false);
                            
                            if (_showSuccess)                            
                                Page.RegisterStartupScript("SHOW_SUCCESS", "<script language=\"javascript\">alert('Your image was uploaded successfully.')</script>");
                            
							/*if (_showPopup)
							{
								PopupMustBeShow = true;
							}*/

							e.Success = true;
							return e;
						}
						else if (_showErrors)
						{
							if ((filesize > _uploadMaxSize && _uploadMaxSize > 0))
                                Page.RegisterStartupScript("SHOW_ERROR", "<script language=\"javascript\">alert('Your image size is too high. Please reduce it\'s size.')</script>");
							else
                                Page.RegisterStartupScript("SHOW_ERROR", "<script language=\"javascript\">alert('Your image file extension was not accepted.')</script>");
						}
					}
				}
			}
		
			return e;
		}

        /// <summary>
        /// Raises the upload event.
        /// </summary>
        /// <param name="e">The <see cref="ActiveUp.WebControls.ImageUploadedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnUpload(ImageUploadedEventArgs e)
        {
            if (Upload != null)
            {
                
                Upload(this, e);
            }
        }

        /// <summary>
        /// Raises the delete event.
        /// </summary>
        /// <param name="e">The <see cref="ActiveUp.WebControls.ImageDeleteEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDelete(ImageDeleteEventArgs e)
        {
            if (Delete != null)
            {
                Delete(this, e);
            }
        }

        private FileDirectoryCollection _directories;
        /// <summary>
        /// Gets or sets the FileDirectory objects used by the Image tool.
        /// </summary>
        public FileDirectoryCollection Directories
        {
            get
            {
                /*if (Page.Session[this.ClientID + "_directories"] == null)
                    Page.Session[this.ClientID + "_directories"] = new FileDirectoryCollection();
                        return (FileDirectoryCollection)Page.Session[this.ClientID + "_directories"];*/
                if (_directories == null)
                    _directories = new FileDirectoryCollection();
                return _directories;
            }
            set
            {
                //Page.Session[this.ClientID + "_directories"] = value;
                _directories = value;
            }
        }

        /// <summary>
        /// Resizes the specified image file with the specified width and height.
        /// </summary>
        /// <param name="sourceImage">The source image file.</param>
        /// <param name="destinationImage">The destination image file.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="forceSaving">Force saving.</param>
        public void Resize(string sourceImage, string destinationImage, int width, int height, bool forceSaving)
        {
            Bitmap image = new Bitmap(sourceImage);
            Size newSize;

            if (image.Width > width || image.Height > height || (image.Width < width && image.Height < height) && this.ResizeSmaller)
            {
                if (this.ConstrainProportions)
                    newSize = this.Sizer(image.Width, image.Height, width, height);
                else
                    newSize = new Size(MaxSize.Width, MaxSize.Height);

                Bitmap newImage = new Bitmap(image, (int)newSize.Width, (int)newSize.Height);

                image.Dispose();

                newImage.Save(destinationImage, this.GetImageFormat(System.IO.Path.GetExtension(sourceImage)));
                newImage.Dispose();
            }
            else
            {
                if (forceSaving)
                    image.Save(destinationImage, this.GetImageFormat(System.IO.Path.GetExtension(sourceImage)));
            }

            image.Dispose();

        }

        /// <summary>
        /// Specify whether you want to keep image proportions when resizing.
        /// </summary>
        public bool ConstrainProportions
        {
            get
            {
                return _constrainProportions;
            }
            set
            {
                _constrainProportions = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum image size.
        /// </summary>
        public Size MaxSize
        {
            get
            {
                return _maxSize;
            }
            set
            {
                _maxSize = value;
            }
        }

        /// <summary>
        /// Return an ImageFormat object based on the format name.
        /// </summary>
        /// <param name="_outputFormat">The format name.</param>
        /// <returns>The ImageFormat name.</returns>
        public System.Drawing.Imaging.ImageFormat GetImageFormat(string _outputFormat)
        {
            switch (_outputFormat.ToLower())
            {
                case "bmp":
                case ".bmp": return System.Drawing.Imaging.ImageFormat.Bmp;
                case "emf":
                case ".emf": return System.Drawing.Imaging.ImageFormat.Emf;
                case "exif":
                case ".exif": return System.Drawing.Imaging.ImageFormat.Exif;
                case "gif":
                case ".gif": return System.Drawing.Imaging.ImageFormat.Gif;
                case "icon":
                case ".ico": return System.Drawing.Imaging.ImageFormat.Icon;
                case "jpeg":
                case ".jpeg":
                case "jpg":
                case ".jpg": return System.Drawing.Imaging.ImageFormat.Jpeg;
                case "png":
                case ".png": return System.Drawing.Imaging.ImageFormat.Png;
                case "tiff":
                case ".tiff":
                case "tif":
                case ".tif": return System.Drawing.Imaging.ImageFormat.Tiff;
                case "wmf":
                case ".wmf": return System.Drawing.Imaging.ImageFormat.Wmf;
            }

            return System.Drawing.Imaging.ImageFormat.Jpeg;

        }

        /// <summary>
        /// Return a size based on the current and maximum values.
        /// </summary>
        /// <param name="currentWidth">The current width.</param>
        /// <param name="currentHeight">The current height.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <returns></returns>
        public Size Sizer(int currentWidth, int currentHeight, int maxWidth, int maxHeight)
        {
            Size size = new Size(currentWidth, currentHeight);

            if ((double)currentWidth / (double)maxWidth > (double)currentHeight / (double)maxHeight)
            {
                size.Width = maxWidth;
                size.Height = currentHeight * maxWidth / currentWidth;
            }
            else
            {
                size.Height = maxHeight;
                size.Width = currentWidth * maxHeight / currentHeight;
            }

            return size;
        }

        /// <summary>
        /// Specify whether you want to resize an image if its size is smaller than the max values.
        /// </summary>
        public bool ResizeSmaller
        {
            get
            {
                return _resizeSmaller;
            }
            set
            {
                _resizeSmaller = value;
            }
        }

        /// <summary>
        /// Gets or sets the prefix to use for thumbnails filenames.
        /// </summary>
        /// <remarks>By default, the file name of the thumbnail is &quot;tn_[originalfilename]&quot;. You can specify here the thumbnail prefix you want. </remarks>
        public string ThumbnailPrefix
        {
            get
            {
                return _thumbnailPrefix;
            }
            set
            {
                _thumbnailPrefix = value;
            }
        }

        /// <summary>
        /// Gets or sets the thumbnail image size.
        /// </summary>
        public Size ThumbnailSize
        {
            get
            {
                return _thumbnailSize;
            }
            set
            {
                _thumbnailSize = value;
            }
        }

        /*public bool ShowPopup
        {
            get 
            {
                return _showPopup;
            }

            set
            {
                _showPopup = value;
            }
        }*/

        internal bool PopupMustBeShow
        {
            get
            {
                if (ViewState["_popupMustBeShow"] == null)
                    ViewState["_popupMustBeShow"] = false;
                return (bool)ViewState["_popupMustBeShow"];
            }

            set
            {
                ViewState["_popupMustBeShow"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether upload is disabled.
        /// </summary>
        /// <value><c>true</c> if upload is disabled; otherwise, <c>false</c>.</value>
        public bool UploadDisabled
        {
            get
            {
                if (ViewState["_uploadDisabled"] == null)
                    return false;

                return (bool)ViewState["_uploadDisabled"];
            }

            set
            {
                ViewState["_uploadDisabled"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether delete button is disabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if delete button is disabled; otherwise, <c>false</c>.
        /// </value>
        public bool DeleteButtonDisabled
		{
			get
			{
				return ViewStateHelper.GetFromViewState(ViewState, nameof(DeleteButtonDisabled), false);
			}

			set
			{
				ViewStateHelper.SetViewState(ViewState, nameof(DeleteButtonDisabled), value);
			}
		}

        /// <summary>
        /// Gets or sets the tree icons.
        /// </summary>
        /// <value>The tree icons.</value>
        public IconSet TreeIcons
        {
            get
            {
                if (ViewState["_treeIcons"] == null)
                {
                    IconSet icons = new IconSet();
                    ViewState["_treeIcons"] = icons;
                    return icons;
                }

                return (IconSet)ViewState["_treeIcons"];
            }

            set
            {
                ViewState["_treeIcons"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the delete icon.
        /// </summary>
        /// <value>The delete icon.</value>
        public string DeleteIcon
        {
            get
            {
                if (ViewState["_deleteIcon"] == null)
                {
                    string deleteIcon = "delete_off.gif";
                    ViewState["_deleteIcon"] = deleteIcon;
                    return deleteIcon;
                }

                return (string)ViewState["_deleteIcon"];
            }

            set
            {             
                ViewState["_deleteIcon"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the icons directory.
        /// </summary>
        /// <value>The icons directory.</value>
        public string IconsDirectory
        {
            get
            {
                object local = ViewState["_iconsDirectory"];
                if (local != null)
                    return (string)local;
                else
                    return Define.IMAGES_DIRECTORY;
            }

            set
            {
                ViewState["_iconsDirectory"] = value;
            }
        }

        /// <summary>
        /// Gets the tree font.
        /// </summary>
        /// <value>The tree font.</value>
        public FontInfo TreeFont
        {
            get
            {
                return _treeDirectories.Font;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether you want to use load on demand.
        /// </summary>
        /// <value><c>true</c> if load on demand used; otherwise, <c>false</c>.</value>
        public bool LoadOnDemand
        {
            get
            {
                return _treeDirectories.LoadOnDemand;
            }

            set
            {
                _treeDirectories.LoadOnDemand = value;
            }
        }

        /// <summary>
        /// Gets or sets the path for the load on demand.
        /// </summary>
        /// <value>The path for the load on demand.</value>
        public string PathLoadOnDemand
        {
            get
            {
                return _treeDirectories.Path;
            }

            set
            {
                _treeDirectories.Path = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tree can be expanded.
        /// </summary>
        /// <value><c>true</c> if the tree can be expanded; otherwise, <c>false</c>.</value>
        public bool ExpandedTree
        {
            get
            {
                return _expandedTree;
            }

            set
            {
                _expandedTree = value;
            }
        }

        

#if (!FX1_1)
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {         
            if (!String.IsNullOrEmpty(eventArgument))
            {
                returnValue = eventArgument;
            }

        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            ImageUploadedEventArgs e = new ImageUploadedEventArgs();
       
            string fileName = System.IO.Path.GetFileName(returnValue);           
   
            string path = this.Directories[0] != null ? this.Directories[0].Path : HttpContext.Current.Server.MapPath(this.IconsDirectory);
            string fullPath = string.Format("{0}{1}{2}", path, System.IO.Path.DirectorySeparatorChar, fileName);

            FileStream uploadedFileStream = new FileStream(returnValue, FileMode.Open, FileAccess.Read);            
            int fileSize = (int)uploadedFileStream.Length;

            if (this.UniqueFilenames)
            {
                fileName = DateTime.Today.Subtract(new DateTime(2002, 1, 1)).Days.ToString().PadLeft(4, '0') +
                    DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "_" + fileName;
            }

            string[] directories = path.Split(System.IO.Path.DirectorySeparatorChar);
            e.Directory = directories[directories.Length - 1];
            e.Filename = fullPath;
            e.Size = fileSize;

            if ((fileSize < _uploadMaxSize || _uploadMaxSize == 0) 
                && fileName.Length > 0 && this.ImageExtension.Contains(System.IO.Path.GetExtension(fileName).ToUpper()))
            {

                byte[] buffer = new byte[fileSize];
                uploadedFileStream.Read(buffer, 0, fileSize);

                uploadedFileStream.Dispose();
                uploadedFileStream.Close();

                FileStream fs = new FileStream(fullPath, FileMode.Create);
                fs.Write(buffer, 0, fileSize);

                Bitmap bmp = new Bitmap(fs);

                if (System.IO.Path.GetExtension(returnValue).Equals(".gif"))
                {
                    bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Gif);
                }
                else
                {
                    bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                bmp.Dispose();

                fs.Dispose();
                fs.Close();

                if (this.CreateThumbnail)
                    Resize(fullPath, path + this.ThumbnailPrefix + fileName, ThumbnailSize.Width, ThumbnailSize.Height, true);
                                
                if ((MaxSize.Width > 0 || MaxSize.Height > 0))
                    Resize(fullPath, fullPath, MaxSize.Width, MaxSize.Height, false);

                if (_showSuccess)
                    Page.RegisterStartupScript("SHOW_SUCCESS", "<script language=\"javascript\">alert('Your image was uploaded successfully.')</script>");

                e.Success = true;
            }
            else if (_showErrors)
            {
                if ((fileSize > _uploadMaxSize && _uploadMaxSize > 0))
                    Page.RegisterStartupScript("SHOW_ERROR", "<script language=\"javascript\">alert('Your image size is too high. Please reduce it\'s size.')</script>");
                else
                    Page.RegisterStartupScript("SHOW_ERROR", "<script language=\"javascript\">alert('Your image file extension was not accepted.')</script>");
            }

            OnUpload(e);
            return this.ClientID;

        }
#else
#endif

    }
}
