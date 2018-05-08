using System;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolInsertRule"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolInsertRule : ToolButton
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolInsertRule"/> class.
		/// </summary>
		public ToolInsertRule()
		{
			this.ClientSideClick = "HTB_CommandBuilder('$EDITOR_ID$', 'alignleft', null);";
		}

		/// <summary>
		/// Renders at the design time.
		/// </summary>
		/// <param name="output">The output.</param>
        public override void RenderDesign(HtmlTextWriter output)
        {
#if (!FX1_1)

                if (ImageURL == string.Empty)
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.rule_off.gif");
                if (OverImageURL == string.Empty)
                    this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.rule_over.gif");
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
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.rule_off.gif");
                if (OverImageURL == string.Empty)
                    this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.rule_over.gif");
#endif
            base.OnPreRender(e);

		}


	}
}
