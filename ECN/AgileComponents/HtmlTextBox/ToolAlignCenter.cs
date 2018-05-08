#if !FX1_1
#define NOT_FX1_1
#endif

using System;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolAlignCenter"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolAlignCenter : ToolButton
	{
		private const string ImageUrlResourceName = "ActiveUp.WebControls._resources.Images.aligncenter_off.gif";
		private const string OverImageUrlResourceName = "ActiveUp.WebControls._resources.Images.aligncenter_over.gif";

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolAlignCenter"/> class.
		/// </summary>
		public ToolAlignCenter() : base()
		{
			_Init(string.Empty);	
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolAlignCenter"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolAlignCenter(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolAlignCenter" + Editor.indexTools++;
			else
				this.ID = id;

			this.ClientSideClick = "HTB_CommandBuilder('$EDITOR_ID$', 'justifycenter', null);";
#if (!FX1_1)
            this.ImageURL = string.Empty;
            this.OverImageURL = string.Empty;
#else
			this.ImageURL = "aligncenter_off.gif";
			this.OverImageURL = "aligncenter_over.gif";
#endif
			this.ToolTip = "Align Center";
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
