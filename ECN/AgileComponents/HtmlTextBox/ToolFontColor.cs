using System;
using System.Web.UI;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolFontColor"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolFontColor : ToolColorPickerBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolFontColor"/> class.
		/// </summary>
		public ToolFontColor() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolFontColor"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolFontColor(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolFontColor" + Editor.indexTools++;
			else
				this.ID = id;
			//this.ClientColorSelected =  "HTB_SetColorEditor('$EDITOR_ID$','f',HTB_GetSelectedColor('$CLIENT_ID$'))"; 
			this.ToolTip = "Font Color";
            this.Text = "<img src=$IMAGESDIRECTORY$fontcolor_off.gif>";

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
                this.Text = string.Format("<img src='{0}'>", Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.fontcolor_off.gif"));
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
            if (Text == "<img src=$IMAGESDIRECTORY$fontcolor_off.gif>")
                this.Text = string.Format("<img src={0}>", Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.fontcolor_off.gif"));
#endif

			base.OnPreRender(e);

			// Get Parent Editor
			Editor editor = (Editor)this.Parent.Parent.Parent;

			if (editor !=  null)
			{
				//this.ClientColorSelected = this.ClientColorSelected.Replace("$EDITOR_ID$", editor.ClientID);
				//this.ClientColorSelected = this.ClientColorSelected.Replace("$CLIENT_ID$",ClientID);
				this.ClientColorSelected =  string.Format("HTB_SetColorEditor('{0}','f',HTB_GetSelectedColor('{1}'))",editor.ClientID,this.ClientID); 
			}
			
		}

		/// <summary>
		/// Sends the Popup content to a provided HTBlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HTBlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
			Editor editor = (Editor)this.Parent.Parent.Parent;

#if (FX1_1)
            this.Text =  string.Format("<img src={0}>",Utils.ConvertToImageDir(editor.IconsDirectory,"fontcolor_off.gif"));
#endif

			base.Render(output);

			output.Write("<script language=\"javascript\">\n");
			output.Write(string.Format("function HTB_create_{0}_CustomColors(e)\n",ClientID));
			output.Write("{\n");
			//output.Write(string.Format("ATB_createPopup(\"{0}_CustomColors\",0, 0, {2}, {3}, \"Custom Colors\", HTB_BuildColorTable(\"{4}\",\"{0}\",\"HTB_SetSelectedColor('{0}',$color$);ATB_hidePopup('{0}_CustomColors');{1}\",false,\"f\"),\"#DDDDDD\",\"#DDDDDD\",\"Outset\",\"2\",\"#4F6DA5\",\"#808080\",\"#FFFFFF\",\"NotSet\",\"0\",\"#FFFFFF\",\"#DDDDDD\",\"#FFFFFF\",\"NotSet\",\"0\", \"#808080\",\"True\",\"False\",undefined,\"True\")",ClientID,ClientColorSelected,base.WidthPopup,base.HeightPopup,editor.ClientID,EnableSsl));	
            string closeImage = string.Empty;
            if (Page.Request.Browser.Browser.ToUpper() == "IE")
            {
                output.Write(string.Format("ATB_createPopup(\"{0}_CustomColors\",0, 0, {2}, {3}, \"Custom Colors\", HTB_BuildColorTable(\"{4}\",\"{0}\",\"HTB_SetSelectedColor('{0}',$color$);ATB_hidePopup('{0}_CustomColors');{1}\",false,\"f\"),\"#DDDDDD\",\"#DDDDDD\",\"Outset\",\"2\",\"#4F6DA5\",\"#808080\",\"#FFFFFF\",\"NotSet\",\"0\",\"#FFFFFF\",\"#DDDDDD\",\"#FFFFFF\",\"NotSet\",\"0\", \"#808080\",\"True\",\"False\",undefined,\"True\")", ClientID, ClientColorSelected, base.WidthPopup, base.HeightPopup, editor.ClientID, EnableSsl));	
            }
            else
            {
                string spacer = string.Empty;
               
#if (!FX1_1)
                spacer = Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory, "", "spacer.gif", Page, this.GetType());
                closeImage = "";
#else
                spacer = Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory, "spacer.gif");
                closeImage = "close.gif";
#endif
                output.Write(string.Format("ATB_createPopup(\"{0}_CustomColors\",0, 0, {2}, {3}, \"Custom Colors\", HTB_BuildColorTableMozilla(\"{4}\",\"{0}\",\"HTB_SetSelectedColor('{0}',$color$);ATB_hidePopup('{0}_CustomColors');{1}\",false,\"f\",\"{6}\"),\"#DDDDDD\",\"#DDDDDD\",\"Outset\",\"2\",\"#4F6DA5\",\"#808080\",\"#FFFFFF\",\"NotSet\",\"0\",\"#FFFFFF\",\"#DDDDDD\",\"#FFFFFF\",\"NotSet\",\"0\", \"#808080\",\"True\",\"False\",undefined,\"True\")", ClientID, ClientColorSelected, base.WidthPopup, base.HeightPopup, editor.ClientID, EnableSsl, spacer));	
            }
			output.Write(string.Format("\nATB_setTitleGradient(\"{0}_CustomColors\",\"#0A246A\",\"#A6CAF0\")\n",ClientID));
            output.Write(string.Format("\nATB_setCloseImage(\"{0}_CustomColors\",\"{1}\");", ClientID, Utils.ConvertToImageDir(editor.IconsDirectory == "/" ? string.Empty : editor.IconsDirectory, closeImage, "close.gif", this.Page, this.GetType())));
			output.Write("}\n");
			output.Write(string.Format("window.RegisterEvent(\"onload\", HTB_create_{0}_CustomColors);\n",ClientID));
            output.Write("\n</script>\n");


		}

	}
}
