#if !FX1_1
#define NOT_FX1_1
#endif

using System;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolAlignRight"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolAlignRight : ToolButton
	{
		private const string ImageUrlResourceName = "ActiveUp.WebControls._resources.Images.alignright_off.gif";
		private const string OverImageUrlResourceName = "ActiveUp.WebControls._resources.Images.alignright_over.gif";

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolAlignRight"/> class.
		/// </summary>
		public ToolAlignRight() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolAlignRight"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolAlignRight(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolAlignRight" + Editor.indexTools++;
			else
				this.ID = id;
			this.ClientSideClick = "HTB_CommandBuilder('$EDITOR_ID$', 'justifyright', null);";
#if (!FX1_1)
            this.ImageURL = string.Empty;
            this.OverImageURL = string.Empty;
#else
			this.ImageURL = "alignright_off.gif";
			this.OverImageURL = "alignright_over.gif";
#endif
			this.ToolTip = "Align Right";
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