// ActiveToolbar 1.x
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	#region class ToolSpacer

	/// <summary> 
	/// Represents a <see cref="ToolSpacer"/>.
	/// </summary>
	[
        Serializable,
        ToolboxItem(false)
    ]
	public class ToolSpacer : ToolBase
	{
		#region Constructors

		
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolSpacer"/> class.
		/// </summary>
		public ToolSpacer() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolSpacer"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolSpacer(string id) : base(id)
		{
		}

		#endregion
		
		#region Methods
		/// <summary>
		/// Render the tool to the specified HtmlTextWriter object. Usually a Page.
		/// </summary>
		protected void RenderTool(HtmlTextWriter output)
		{
			string backImage = string.Empty;
			if (BackImage != string.Empty)
			{
				if (Parent != null && Parent is ActiveUp.WebControls.Toolbar)
					backImage = "url(" + Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory,BackImage) + ")";
				else
					backImage = "url(" + BackImage + ")";
			}
			
			if (backImage != string.Empty)
				output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage,backImage);
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing,"0");
			output.AddAttribute(HtmlTextWriterAttribute.Border,"0");
			output.RenderBeginTag(HtmlTextWriterTag.Table);
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.RenderBeginTag(HtmlTextWriterTag.Td);

			string clientID = this.ClientID.Replace(":", "_");
			this.ControlStyle.AddAttributesToRender(output);
			output.AddAttribute(HtmlTextWriterAttribute.Name, clientID);
			output.AddAttribute(HtmlTextWriterAttribute.Alt, this.ToolTip);
            string fullPathSpacer = String.Empty;
            if (Parent != null && Parent is ActiveUp.WebControls.Toolbar)
                fullPathSpacer = Utils.ConvertToImageDir(((Toolbar)Parent).ImagesDirectory, this.ImageURL);
            else
                fullPathSpacer = this.ImageURL;

#if (!FX1_1)
            if (fullPathSpacer == string.Empty)
            {
                fullPathSpacer = Utils.ConvertToImageDir(string.Empty, fullPathSpacer, "separator.gif", Page, this.GetType());
            }
#endif

            output.AddAttribute(HtmlTextWriterAttribute.Src, fullPathSpacer);

			output.RenderBeginTag(HtmlTextWriterTag.Img);
			output.RenderEndTag();

			output.RenderEndTag(); // Td;
			output.RenderEndTag(); // Tr;
			output.RenderEndTag(); // Table;
		}

		/// <summary>
		/// Sends the Popup content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
			if (this.Enabled)
			{
				this.RenderTool(output);
			}
		}
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the relative link to the image to use.
		/// </summary>
		[
		Bindable(true), 
		Category("Appearance"), 
		DefaultValue(""),
		Description("The relative link to the image to use.")
		] 
		public string ImageURL
		{
			get
			{
				string _imageURL;
				_imageURL = ((string) base.ViewState["_imageURL"]);
				if (_imageURL != null)
				{
					return _imageURL; 
				}
				return string.Empty;
			}
			set
			{
				ViewState["_imageURL"] = value;
			}
		}
		#endregion
	}

	#endregion
}
