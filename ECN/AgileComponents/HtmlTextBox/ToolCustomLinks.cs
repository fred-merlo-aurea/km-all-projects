using System;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolCustomLinks"/> object.
	/// </summary>
	[
		Serializable,		
        ToolboxItem(false)
	]
	public class ToolCustomLinks : ToolDropDownList
	{
		private bool _mustBeRendered = true;
		private TreeView _treeCustomLinks = null;
		//private string TOOLCUSTOMLINKS_KEY = "TOOLCUSTOMLINKS_KEY";
		private string CLIENTSIDE_API = string.Empty;
		private ToolLinkItemCollection _links = new ToolLinkItemCollection();


		/// <summary>
		/// Initializes a new instance of the <see cref="ToolCustomLinks"/> class.
		/// </summary>
        public ToolCustomLinks() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolCustomLinks"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolCustomLinks(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolCustomLinks" + Editor.indexTools++;
			else
				this.ID = id;
						
			this.ChangeToSelectedText = SelectedText.Value;
			this.Height = Unit.Parse("22px");
			this.Text = "Custom Links";
			this.ChangeToSelectedText = SelectedText.None;
			this.CloseDropDownOnClickItem = false;
			this.Cellpadding = 2;
			this.ItemBackColor = Color.Empty;
			this.ItemBackColorRollOver = Color.Transparent;
			this.ItemBorderColor = Color.Transparent;
			this.ItemBorderColorRollOver = Color.Transparent;
			this.ItemBorderStyle = BorderStyle.Solid;
			this.ItemBorderWidth = Unit.Parse("1px");

			_treeCustomLinks = new TreeView();
			_treeCustomLinks.ID = ID + "Tree";
			_treeCustomLinks.DisplayMasterNode = false;
			/*_treeCustomLinks.Icons.Default = "icons/de.gif";
			_treeCustomLinks.Icons.Collapsed = "icons/plus.gif"; 
			_treeCustomLinks.Icons.Expanded = "icons/minus.gif";
			_treeCustomLinks.Icons.Connection = "icons/spacer.gif";
			_treeCustomLinks.Icons.Last = "icons/spacer.gif";
			_treeCustomLinks.Icons.Orphan = "icons/spacer.gif";¨*/

			this.Controls.Add(_treeCustomLinks);

			/*ToolLinkItem t1 = new ToolLinkItem("http://www.google.com","parent1",string.Empty,"_blank");
			ToolLinkItem t2 = new ToolLinkItem("http://www.google.com","parent2",string.Empty,"_blank");
			ToolLinkItem t3 = new ToolLinkItem("http://www.google.com","child1",string.Empty,"_blank");
			ToolLinkItem t4 = new ToolLinkItem("http://www.google.com","child1.1",string.Empty,"_blank");
			ToolLinkItem t5 = new ToolLinkItem("http://www.google.com","child2",string.Empty,"_blank");
			_links.Add(t1);
			t1.Links.Add(t3);
			t3.Links.Add(t4);
			_links.Add(t2);
			t2.Links.Add(t5);*/

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
				RegisterAPIScriptBlock(); 
			}

			bool isNs6 = false;
			System.Web.HttpBrowserCapabilities browser = Page.Request.Browser; 
			if (browser.Browser.ToUpper().IndexOf("IE") == -1)
				isNs6 = true;
			
			if (isNs6)
				_mustBeRendered = false;
			else
				_mustBeRendered = true;

		}

		/// <summary>
		/// Register the client-side script block in the ASPX page.
		/// </summary>
		public override void RegisterAPIScriptBlock() 
		{
			base.RegisterAPIScriptBlock();

			// Register the script block is not already done.
			/*if (!Page.IsClientScriptBlockRegistered(TOOLCUSTOMLINKS_KEY)) 
			{
				CLIENTSIDE_API += "\n<script language=\"javascript\">\n";
				CLIENTSIDE_API += string.Format("\nfunction InitToolCustomLinks_{0}(e)\n",ClientID);
				CLIENTSIDE_API += "{\n";
				CLIENTSIDE_API += string.Format("document.getElementById('{0}_item0').innerHTML = document.getElementById('{1}_contents').outerHTML;",this.ClientID,_treeCustomLinks.ClientID);
				CLIENTSIDE_API += string.Format("document.getElementById('{0}_contents').outerHTML = '';",_treeCustomLinks.ClientID);
				CLIENTSIDE_API += "}\n"; 
				CLIENTSIDE_API += string.Format("window.RegisterEvent(\"onload\", InitToolCustomLinks_{0});\n",ClientID);
				CLIENTSIDE_API += "\n</script>\n";

				Page.RegisterClientScriptBlock(TOOLCUSTOMLINKS_KEY, CLIENTSIDE_API);
			}*/

			Editor editor = (Editor)this.Parent.Parent.Parent;
			_treeCustomLinks.ScriptDirectory = editor.ScriptDirectory;
			_treeCustomLinks.RegisterAPIScriptBlock(this.Page);
		}

		private void AddTreeCustomLinks(string editorID)
		{
			foreach(ToolLinkItem link in _links)
			{
				AddTreeCustomLink(editorID,_treeCustomLinks.Nodes,link);	
			}

		}

		private void AddTreeCustomLink(string editorID,ControlCollection nodes, ToolLinkItem link)
		{
			TreeNode newNode = new TreeNode();
			newNode.Link = string.Format("javascript:HTB_InsertCustomLink('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",editorID,ClientID,link.Address,link.Text,link.Anchor,link.Target,link.Tooltip);
			newNode.Text = link.Text;
			nodes.Add(newNode);
			foreach(ToolLinkItem li in link.Links)
			{
				AddTreeCustomLink(editorID,newNode.Nodes, li);
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
				Editor editor = (Editor)this.Parent.Parent.Parent;

				/*output.AddStyleAttribute("left","10");
				output.AddStyleAttribute("position","absolute");
				output.AddStyleAttribute("display","none");
				output.RenderBeginTag(HtmlTextWriterTag.Div); // Open Div*/
				output.Write("<div style=\"display: none; position: absolute; left:10\">");

				AddTreeCustomLinks(editor.ClientID);

				//_treeCustomLinks.RenderControl(output);
				this.RenderChildren(output);

				//output.RenderEndTag(); // Close Div
				output.Write("</div>");

				this.Items.Add(new ToolItem(string.Empty,"_toolItemCustomLinksTree"));

				base.Render(output);

				output.Write("\n<script language=\"javascript\">\n");
				output.Write(string.Format("\nfunction InitToolCustomLinks_{0}(e)\n",ClientID));
				output.Write("{\n");
				output.Write(string.Format("document.getElementById('{0}_item0').innerHTML = document.getElementById('{1}_contents').outerHTML;",this.ClientID,_treeCustomLinks.ClientID));
				output.Write(string.Format("document.getElementById('{0}_contents').outerHTML = '';",_treeCustomLinks.ClientID));
				output.Write("}\n"); 
				output.Write(string.Format("window.RegisterEvent(\"onload\", InitToolCustomLinks_{0});\n",ClientID));
				output.Write("\n</script>\n");
			}
		}

		/// <summary>
		/// Gets or sets the links.
		/// </summary>
		/// <value>The links.</value>
		public ToolLinkItemCollection Links
		{
			get {return _links;}
			set {_links = value;}
		}
	}
}
