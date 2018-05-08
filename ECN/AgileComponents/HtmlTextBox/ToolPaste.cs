#if !FX1_1
#define NOT_FX1_1
#endif

using System;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolPaste"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolPaste : ToolButton
	{
		private const string ImageUrlResourceName = "ActiveUp.WebControls._resources.Images.paste_off.gif";
		private const string OverImageUrlResourceName = "ActiveUp.WebControls._resources.Images.paste_over.gif";

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolPaste"/> class.
		/// </summary>
		public ToolPaste() : base()
		{
			_Init(string.Empty);			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolPaste"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolPaste(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolPaste" + Editor.indexTools++;
			else
				this.ID = id;

			this.ClientSideClick = "try {HTB_CommandBuilder('$EDITOR_ID$', 'paste', null);} catch(e) {alert('Your security settings doesn\\'t allow to use the \\'paste\\' command.');}";
#if (!FX1_1)
            this.ImageURL = string.Empty;
            this.OverImageURL = string.Empty;
#else
			this.ImageURL = "paste_off.gif";
			this.OverImageURL = "paste_over.gif";
#endif
			this.ToolTip = "Paste";
		}

		/// <summary>
		/// Renders at the design time.
		/// </summary>
		/// <param name="output">The output.</param>
		public override void RenderDesign(HtmlTextWriter output)
		{
			SetImageUrl(ImageUrlResourceName, OverImageUrlResourceName);
			this.RenderControl(output);
		}

		/// <summary>
		/// Do some work before rendering the control.
		/// </summary> 
		/// <param name="e">Event Args</param>
		protected override void OnPreRender(EventArgs e) 
		{
			var editor = this.Parent.Parent.Parent as Editor;
			ToolOnPreRender(ImageUrlResourceName, OverImageUrlResourceName, editor?.ClientID);
			base.OnPreRender(e);
		}
	}
}
