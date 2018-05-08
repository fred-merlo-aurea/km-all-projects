using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Text;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolColorPickerBase"/> object.
	/// </summary>
	[ToolboxItem(false)]
	public class ToolColorPickerBase : ToolDropDownList
	{
		private int _widthPopup;
		private int _heightPopup;

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolColorPickerBase"/> class.
		/// </summary>
		public ToolColorPickerBase() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolColorPickerBase"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolColorPickerBase(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id != string.Empty)
				ID = id;
			this.ChangeToSelectedText = SelectedText.None;
			this.Width = Unit.Parse("20px");
			//this.Text = "*\n";
			this.ItemBackColorRollOver = Color.Empty;
			this.ItemsAreaHeight = Unit.Parse("100px");
			this.BackColorItems = Color.FromArgb(0xF9,0xF8,0xF7);
		}

		/// <summary>
		/// Notifies the Popup control to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{
			RegisterAPIScriptBlock();
			base.OnPreRender(e);   

			bool isNs6 = false;
			System.Web.HttpBrowserCapabilities browser = Page.Request.Browser; 
			if (browser.Browser.ToUpper().IndexOf("IE") == -1)
				isNs6 = true;

			if (isNs6)
			{
				_widthPopup = 240;
				_heightPopup = 208;
			}
			else
			{
				_widthPopup = 245;
				_heightPopup = 235;
			}

		} 

		/// <summary>
		/// Sends the Popup content to a provided HTBlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HTBlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
			Editor editor = (Editor)this.Parent.Parent.Parent;

			output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Value,"");
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_selectedColor");
			output.AddAttribute(HtmlTextWriterAttribute.Name, ClientID + "_selectedColor");
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			AddColorPicker();

			/*output.Write("<script language=\"javascript\">\n");
			output.Write(string.Format("function HTB_create_{0}_CustomColors(e)\n",ClientID));
			output.Write("{\n");
			output.Write(string.Format("ATB_createPopup(\"{0}_CustomColors\",0, 0, {2}, {3}, \"Custom Colors\", HTB_BuildColorTable(\"{0}\",\"HTB_SetSelectedColor('{0}',$color$);ATB_hidePopup('{0}_CustomColors');{1}\",false),\"#DDDDDD\",\"#DDDDDD\",\"Outset\",\"2\",\"#4F6DA5\",\"#808080\",\"#FFFFFF\",\"NotSet\",\"0\",\"#FFFFFF\",\"#DDDDDD\",\"#FFFFFF\",\"NotSet\",\"0\", \"#808080\",\"True\",\"False\")",ClientID,ClientColorSelected,_widthPopup,_heightPopup));	
			output.Write(string.Format("\nATB_setTitleGradient(\"{0}_CustomColors\",\"#0A246A\",\"#A6CAF0\")\n",ClientID));
			output.Write(string.Format("\nATB_setCloseImage(\"{0}_CustomColors\",\"{1}\");",ClientID,Utils.ConvertToImageDir(editor.IconsDirectory,"close.gif")));
			output.Write("}\n");
			output.Write(string.Format("window.RegisterEvent(\"onload\", HTB_create_{0}_CustomColors);\n",ClientID));
			output.Write("\n</script>\n");*/

			base.Render(output);
		}

		private void AddColorPicker()
		{
			var editor = (Editor) Parent.Parent.Parent;

			var content = new StringBuilder();

			content.Append("<table class=\"HTB_clsDropDownContent\">\n");
			content.Append(" 	<tr>\n");
			content.Append("		<td><span class=\"HTB_clsFont\">Standard Colors</span>\n");
			content.Append("			<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n");
			content.Append(" 				<tr>\n");
			AppendTableHtbClsColorCont(content, "black");
			AppendTableHtbClsColorCont(content, "white");
			AppendTableHtbClsColorCont(content, "#008000");
			AppendTableHtbClsColorCont(content, "#800000");
			AppendTableHtbClsColorCont(content, "#808000");
			AppendTableHtbClsColorCont(content, "#000080");
			AppendTableHtbClsColorCont(content, "#800080");
			AppendTableHtbClsColorCont(content, "#808080");
			content.Append("				</tr>\n");
			content.Append("				<tr>\n");
			AppendTableHtbClsColorCont(content, "#FFFF00");
			AppendTableHtbClsColorCont(content, "#00FF00");
			AppendTableHtbClsColorCont(content, "#00FFFF");
			AppendTableHtbClsColorCont(content, "#FF00FF");
			AppendTableHtbClsColorCont(content, "#C0C0C0");
			AppendTableHtbClsColorCont(content, "#FF0000");
			AppendTableHtbClsColorCont(content, "#0000FF");
			AppendTableHtbClsColorCont(content, "#008080");
			content.Append("				</tr>\n");
			content.Append("			</table>\n");
			content.Append("			<hr size=\"2\" width=\"100%\">\n");
			content.Append(
				$"			<table class=\"HTB_clsColorCont\" onclick=\"HTB_SetPopupPosition('{editor.ClientID}','{ClientID}_CustomColors'); ATB_showPopup('{ClientID}_CustomColors');{ClientColorSelected};HTB_OnColorOff(this);\" onmouseover=\"HTB_OnColorOver(this);\" onmouseout=\"HTB_OnColorOff(this);\" width=\"100%\">\n");
			content.Append("				<tr>\n");
			content.Append("					<td align=\"center\"><span class=\"HTB_clsFont\">More Colors...</span></td>\n");
			content.Append("				</tr>\n");
			content.Append("			</table>\n");
			content.Append("		</td>\n");
			content.Append("	</tr>\n");
			content.Append("</table>\n");

			Items.Add(new ToolItem(content.ToString(), string.Empty));
		}

		private void AppendTableHtbClsColorCont(StringBuilder content, string color)
		{
			content.Append("					<td>\n");
			content.Append(
				$"						<table class=\"HTB_clsColorCont\" onclick=\"HTB_SetSelectedColor('{ClientID}','{color}');{ClientColorSelected};HTB_OnColorOff(this);\" onmouseover=\"HTB_OnColorOver(this);\" onmouseout=\"HTB_OnColorOff(this);\" width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">\n");
			content.Append("							<tr>\n");
			content.Append("								<td align=\"center\">\n");
			content.Append(
				$"									<table class=\"HTB_clsDropDownItem\" style=\"background-color: {color};\" width=\"12\" height=\"12\">\n");
			content.Append("										<tr>\n");
			content.Append("											<td></td>\n");
			content.Append("										</tr>\n");
			content.Append("									</table>\n");
			content.Append("								</td>\n");
			content.Append("							</tr>\n");
			content.Append("						</table>\n");
			content.Append("					</td>\n");
		}

		/// <summary>
		/// Gets or sets the client color selected.
		/// </summary>
		/// <value>The client color selected.</value>
		public string ClientColorSelected
		{
			get
			{
				object clientColorSelected = ViewState["_clientColorSelected"];
				if (clientColorSelected != null)
					return (String)clientColorSelected;
				else
					return string.Empty;
			}

			set
			{
				ViewState["_clientColorSelected"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the popup width.
		/// </summary>
		/// <value>The popup width.</value>
		public int WidthPopup
		{
			get {return _widthPopup;}
			set {_widthPopup = value;}
		}

		/// <summary>
		/// Gets or sets the popup height.
		/// </summary>
		/// <value>The popup height.</value>
		public int HeightPopup
		{
			get {return _heightPopup;}
			set {_heightPopup = value;}
		}
	}
}
