using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolTable"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolTable : ToolDropDownList
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolTable"/> class.
		/// </summary>
		public ToolTable() : base()
		{
			_Init(string.Empty);		
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolTable"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolTable(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolTable" + Editor.indexTools++;
			else
				this.ID = id;
			this.ChangeToSelectedText = SelectedText.None;
			this.Width = Unit.Parse("20px");
			this.ItemBackColorRollOver = Color.Empty;
			this.ItemsAreaHeight = Unit.Parse("145px");
			this.BackColorItems = Color.FromArgb(0xF9,0xF8,0xF7);
            this.Text = "<img src=$IMAGESDIRECTORY$table_off.gif>";
		}

		/// <summary>
		/// Renders the DropDownList at the design time.
		/// </summary>
		/// <param name="output">The output.</param>
        public override void RenderDesign(HtmlTextWriter output)
        {
#if (!FX1_1)
            if (((Toolbar)Parent).ImagesDirectory == string.Empty || ((Toolbar)Parent).ImagesDirectory == "/")
            {
                this.Text = string.Format("<img src='{0}'>", Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.table_off.gif"));
            }
#endif

            ToolDropDownListDesigner.DesignDropDownList(ref output, this);
        }


		/// <summary>
		/// Notifies the Popup control to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{
#if (!FX1_1)
            if (Text == "<img src=$IMAGESDIRECTORY$table_off.gif>")
                this.Text = string.Format("<img src={0}>", Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.table_off.gif"));
#endif

			RegisterAPIScriptBlock();
			base.OnPreRender(e);   
		} 

		/// <summary>
		/// Sends the Popup content to a provided HTBlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HTBlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
			Editor editor = (Editor)this.Parent.Parent.Parent;
#if (FX1_1)
            this.Text =  string.Format("<img src={0}>",Utils.ConvertToImageDir(editor.IconsDirectory,"table_off.gif"));
#endif

			AddTableEditor(editor);

			output.Write("<script language=\"javascript\">\n");
			output.Write(string.Format("function HTB_create_{0}_TableEditor(e)\n",ClientID));
			output.Write("{\n");
            output.Write(string.Format("ATB_createPopup(\"{0}_TableEditor\",0, 0, 400, 520, \"Table Editor\", HTB_BuildTableEditor(\"{0}\",\"{1}\",\"{0}_TableEditor\"),\"#DDDDDD\",\"#DDDDDD\",\"Outset\",\"2\",\"#4F6DA5\",\"#808080\",\"#FFFFFF\",\"NotSet\",\"0\",\"#FFFFFF\",\"#DDDDDD\",\"#FFFFFF\",\"NotSet\",\"0\", \"#808080\",\"True\",\"True\",undefined,\"{2}\",\"False\",\"False\");\n",ClientID,editor.ClientID,EnableSsl));
			output.Write(string.Format("ATB_setTitleGradient(\"{0}_TableEditor\",\"#0A246A\",\"#A6CAF0\")\n",ClientID));
			output.Write(string.Format("ATB_setTitleFont(\"{0}_TableEditor\",\"Verdana\",\"10pt\",\"True\",\"False\",\"\");\n",ClientID));

            string closeImage = string.Empty;
#if (!FX1_1)
            closeImage = "";
#else
            closeImage = "close.gif";
#endif
            output.Write(string.Format("ATB_setCloseImage(\"{0}_TableEditor\",\"{1}\");\n", ClientID, Utils.ConvertToImageDir(editor.IconsDirectory == "/" ? string.Empty : editor.IconsDirectory, closeImage, "close.gif", this.Page, this.GetType())));
			output.Write(string.Format("ATB_hidePopup(\"{0}_TableEditor\");\n",ClientID));
			output.Write("}\n");
			output.Write(string.Format("window.RegisterEvent(\"onload\", HTB_create_{0}_TableEditor);\n",ClientID));
			output.Write("\n</script>\n");

			base.Render(output);
		}

		private void AddTableEditor(Editor editor)
		{
			var stringBuilder = new StringBuilder();

			stringBuilder.Append("<table \"style='background-color: #E0E0E0;'\">\n");
			stringBuilder.Append("<tr>\n");
			stringBuilder.Append("		<td align=\"center\">\n");
			stringBuilder.Append("			<table width=\"100%\" cellspacing=\"0\" cellpadding=\"1\">\n");

			for (var i = 0; i < 4; i++)
			{
				AddTr(editor, stringBuilder, 0, 5, i + 1, i);
			}

			stringBuilder.Append("			</table>\n");
			stringBuilder.Append("			<span class=\"HTB_clsFont\" id=\"tableInfo\">Cancel</span>\n");
			stringBuilder.Append("			<hr size=\"2\" width=\"100%\">\n");
			stringBuilder.Append(
				$"			<table onclick=\"HTB_SetPopupPosition('{editor.ClientID}','{this.ClientID}' + '_TableEditor'); HTB_OpenTableEditor('{this.ClientID}' + '_TableEditor','{editor.ClientID}');HTB_OnColorOff(this);\" onmouseover=\"HTB_OnColorOver(this);\" onmouseout=\"HTB_OnColorOff(this);\" class=\"HTB_clsColorCont\" width=\"100%\">\n");
			stringBuilder.Append("				<tr>\n");
			stringBuilder.Append("					<td align=\"center\"><span class=\"HTB_clsFont\">Table Editor...</span></td>\n");
			stringBuilder.Append("				</tr>\n");
			stringBuilder.Append("			</table>\n");
			stringBuilder.Append("		</td>\n");
			stringBuilder.Append("	</tr>\n");
			stringBuilder.Append("</table>\n");

			this.Items.Add(new ToolItem(stringBuilder.ToString(), string.Empty));
		}

		private static void AddTr(Editor editor, StringBuilder stringBuilder, int start, int end, int createTableQuickParameter, int tableBuildOverParameter)
		{
			const string temp =
				"					<td align=\"center\" onclick=\"HTB_CreateTableQuick('{0}', {1}, {2});HTB_TableBuilderClear();\" onmouseover=\"HTB_TableBuilderOver(this, {3}, {4})\" onmouseout=\"HTB_TableBuilderClear();\">\n";
			const string temp1 = ("						<table id=\"cell{0}{1}\" class=\"HTB_clsColor\" width=\"18\" height=\"18\">\n");

			stringBuilder.Append("				<tr>\n");
			for (var i = start; i < end; i++)
			{
				stringBuilder.Append(string.Format(temp, editor.ClientID, i + 1, createTableQuickParameter, i, tableBuildOverParameter));
				stringBuilder.Append(string.Format(temp1, i, tableBuildOverParameter));
				AddTrCloseTableAndTd(stringBuilder);
			}
			stringBuilder.Append("				</tr>\n");
		}

		private static void AddTrCloseTableAndTd(StringBuilder stringBuilder)
		{
			stringBuilder.Append("							<tr>\n");
			stringBuilder.Append("								<td></td>\n");
			stringBuilder.Append("							</tr>\n");
			stringBuilder.Append("						</table>\n");
			stringBuilder.Append("					</td>\n");
		}
	}
}
