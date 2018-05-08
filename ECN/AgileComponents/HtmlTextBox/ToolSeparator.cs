using System;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls
{

	/// <summary>
	/// Represents a <see cref="ToolSeparator"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolSeparator : ToolSpacer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolSeparator"/> class.
		/// </summary>
		public ToolSeparator() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolSeparator"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolSeparator(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
				this.ID = "_toolSeparator" + Editor.indexTools++;
			else
				this.ID = id;

#if (!FX1_1)
            this.ImageURL = string.Empty;
#else
			this.ImageURL = "separator.gif";
#endif
		}

		/// <summary>
		/// Renders at the design time.
		/// </summary>
		/// <param name="output">The output.</param>
        public override void RenderDesign(HtmlTextWriter output)
        {
#if (!FX1_1)
                if (ImageURL == string.Empty)
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.separator.gif");
#endif

            this.RenderControl(output);
        }

        /// <summary>
        /// Do some work before rendering the control.
        /// </summary> 
        /// <param name="e">Event Args</param>
        protected override void OnPreRender(EventArgs e)
        {
            Editor editor = (Editor)this.Parent.Parent.Parent;
            this.ClientSideClick = this.ClientSideClick.Replace("$EDITOR_ID$", editor.ClientID);
#if (!FX1_1)
                if (ImageURL == string.Empty)
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.separator.gif");
#endif
            base.OnPreRender(e);

        }
	}
}
