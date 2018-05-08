using System;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolIeSpellCheck"/> object.
	/// </summary>
	[ToolboxItem(false)]
	public class ToolIeSpellCheck : ToolButton
	{
		private const string ImageUrlResourceName = "ActiveUp.WebControls._resources.Images.spell_off.gif";
		private const string OverImageUrlResourceName = "ActiveUp.WebControls._resources.Images.spell_over.gif";

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolIeSpellCheck"/> class.
		/// </summary>
		public ToolIeSpellCheck() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolIeSpellCheck"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolIeSpellCheck(string id) : base()
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
			this.ID = "_toolIeSpellCheck" + Editor.indexTools++;
			else
			this.ID = id;	

#if (!FX1_1)
            this.ImageURL = string.Empty;
            this.OverImageURL = string.Empty;
#else
			this.ImageURL = "spell_off.gif";
			this.OverImageURL = "spell_over.gif";
#endif
			this.ToolTip = "Check Spelling";
			this.ClientSideClick = "try {var tmpis = new ActiveXObject('ieSpell.ieSpellExtension');tmpis.CheckAllLinkedDocuments(window.document);} catch(err) {if (window.confirm('You are missing ieSpell!\\nWould you like to install it?')) {window.open('http://www.iespell.com/download.php');}}" + ClientSideClick;
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
