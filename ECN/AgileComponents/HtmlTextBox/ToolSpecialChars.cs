using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolSpecialChars"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolSpecialChars : ToolDropDownList
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolSpecialChars"/> class.
		/// </summary>
		public ToolSpecialChars()
		{
			_Init(string.Empty);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolSpecialChars"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolSpecialChars(string id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolSpecialChars" + Editor.indexTools++;
			else
				this.ID = id;
			this.ItemBackColorRollOver = Color.Empty;
			this.BackColorItems = Color.FromArgb(0xF9,0xF8,0xF7);
			this.Items.Add(new ToolItem(string.Empty,string.Empty));
			this.ItemsAreaHeight = Unit.Parse("240px");
            this.Text = "<img src=$IMAGESDIRECTORY$specialchars_off.gif>";
            
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
                this.Text = string.Format("<img src='{0}'>", Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.specialchars_off.gif"));
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
			/*if (!Page.IsClientScriptBlockRegistered(KEY))
			{
				Page.RegisterHiddenField(ClientID + "_specialCharsNames",EditorHelper.FormatCollection(this.Chars.Labels,false));
				Page.RegisterHiddenField(ClientID + "_specialCharsCodes",EditorHelper.FormatCollection(this.Chars.Codes,false));
			}*/

#if (!FX1_1)
            if (Text == "<img src=$IMAGESDIRECTORY$specialchars_off.gif>")
                this.Text = string.Format("<img src={0}>", Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.specialchars_off.gif"));
#endif

			base.OnPreRender(e);

    	}

		/// <summary>
		/// Sends the Popup content to a provided HTBlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HTBlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
			Editor editor = (Editor)this.Parent.Parent.Parent;

			output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Value,EditorHelper.FormatCollection(this.Chars.Labels,false));
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_specialCharsNames");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_specialCharsNames");
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			output.AddAttribute(HtmlTextWriterAttribute.Type,"hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Value,EditorHelper.FormatCollection(this.Chars.Codes,false));
			output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_specialCharsCodes");
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_specialCharsCodes");
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

#if (FX1_1)
            this.Text =  string.Format("<img src={0}>",Utils.ConvertToImageDir(editor.IconsDirectory,"specialchars_off.gif"));
#endif

			base.Render(output);
			output.Write("<script language=\"javascript\">\n");
			output.Write(string.Format("HTB_CreateSpecialCharsTable('{0}','{1}');\n",editor.ClientID,ClientID));
			output.Write("\n</script>\n");
		}

		/// <summary>
		/// Gets or sets the chars.
		/// </summary>
		/// <value>The chars.</value>
		public LabeledCodeCollection Chars
		{
			get
			{
				if (ViewState["_chars"] == null)
				{
					ViewState["_chars"] = new LabeledCodeCollection();
					Chars.Add("&quot;");
					Chars.Add("&amp;", "&amp;&amp;");
					Chars.Add("&lt;", "&lt;&lt;");
					Chars.Add("&gt;");
					//Chars.Add("<SUP><FONT SIZE='-1'>TM</FONT></SUP>", "&lt;SUP&gt;&lt;FONT SIZE=&quot;-1&quot;&gt;TM&lt;/FONT&gt;&lt;/SUP&gt;");
					Chars.Add("_", "&nbsp;");
					Chars.Add("&iexcl;");
					Chars.Add("&iquest;");
					Chars.Add("&middot;");
					Chars.Add("&laquo;");
					Chars.Add("&raquo;");
					Chars.Add("&para;");
					Chars.Add("&sect;");
					Chars.Add("&copy;");
					Chars.Add("&reg;");
					Chars.Add("&uml;");
					Chars.Add("&macr;");
					Chars.Add("&acute;");
					Chars.Add("&cedil;");
					Chars.Add("&Agrave;");
					Chars.Add("&agrave;");
					Chars.Add("&Aacute;");
					Chars.Add("&aacute;");
					Chars.Add("&Acirc;");
					Chars.Add("&acirc;");
					Chars.Add("&Atilde;");
					Chars.Add("&atilde;");
					Chars.Add("&Auml;");
					Chars.Add("&auml;");
					Chars.Add("&Aring;");
					Chars.Add("&aring;");
					Chars.Add("&AElig;");
					Chars.Add("&aelig;");
					Chars.Add("&Ccedil;");
					Chars.Add("&ccedil;");
					Chars.Add("&Egrave;");
					Chars.Add("&egrave;");
					Chars.Add("&Eacute;");
					Chars.Add("&eacute;");
					Chars.Add("&Ecirc;");
					Chars.Add("&ecirc;");
					Chars.Add("&Euml;");
					Chars.Add("&euml;");
					Chars.Add("&Igrave;");
					Chars.Add("&igrave;");
					Chars.Add("&Iacute;");
					Chars.Add("&iacute;");
					Chars.Add("&Icirc;");
					Chars.Add("&icirc;");
					Chars.Add("&Iuml;");
					Chars.Add("&iuml;");
					Chars.Add("&micro;");
					Chars.Add("&Ntilde;");
					Chars.Add("&ntilde;");
					Chars.Add("&Ograve;");
					Chars.Add("&ograve;");
					Chars.Add("&Oacute;");
					Chars.Add("&oacute;");
					Chars.Add("&Ocirc;");
					Chars.Add("&ocirc;");
					Chars.Add("&Otilde;");
					Chars.Add("&otilde;");
					Chars.Add("&Ouml;");
					Chars.Add("&ouml;");
					Chars.Add("&Oslash;");
					Chars.Add("&oslash;");
					Chars.Add("&szlig;");
					Chars.Add("&Ugrave;");
					Chars.Add("&ugrave;");
					Chars.Add("&Uacute;");
					Chars.Add("&uacute;");
					Chars.Add("&Ucirc;");
					Chars.Add("&ucirc;");
					Chars.Add("&Uuml;");
					Chars.Add("&uuml;");
					Chars.Add("&yuml;");
					Chars.Add("&divide;");
					Chars.Add("&deg;");
					Chars.Add("&plusmn;");
					Chars.Add("&curren;");
					Chars.Add("&euro;");
					Chars.Add("&cent;");
					Chars.Add("&pound;");
					Chars.Add("&yen;");
				}
				return (LabeledCodeCollection)ViewState["_chars"];
			}
			set
			{
				ViewState["_chars"] = value;
			}
		}
	}
}
