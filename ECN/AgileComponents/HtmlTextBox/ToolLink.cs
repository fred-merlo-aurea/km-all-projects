using System;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolLink"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolLink : ToolButton
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolLink"/> class.
		/// </summary>
		public ToolLink() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolLink"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolLink(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			if (id == string.Empty)
			{
				this.ID = "_toolLink" + Editor.indexTools++;
				this.PopupContents.ID = ID + "Popup";
			}
			else
			{
				this.ID = id;
				this.PopupContents.ID = ID + "Popup";
			}	
			
#if (!FX1_1)
            this.ImageURL = string.Empty;
            this.OverImageURL = string.Empty;
#else
			this.ImageURL = "link_off.gif";
			this.OverImageURL = "link_over.gif";
#endif
			this.ToolTip = "Hyperlink";
			this.UsePopupOnClick = true;
			this.PopupContents.TitleText = "Hyperlink";
			this.PopupContents.ShowShadow = false;
			
			string content = string.Empty;

			content +="<table class=\'HTB_clsPopup\'>";
			content +="	<tr>";
			content +="		<td>";
			content +="			<table width=\'100%\'>"; 
			content +="				<tr>";
			content +="					<td width=\'5\'></td>"; 
			content +="					<td>";
			content +="						<table cellpadding=\'0\' cellspacing=\'0\' border=\'0\'>";
			content +="							<tr>";
			content +="								<td><span>$ADDRESS$ :&nbsp; </span></td>";
			content +="								<td><input id=\'$EDITOR_ID$_HTB_L_address\' style=\'width:100%;height:20px\'></td>";
			content +="							</tr>";
			content +="							<tr>";
			content +="								<td><span>$TEXT$ : </span></td>";
			content +="								<td><input id=\'$EDITOR_ID$_HTB_L_text\' style=\'width:100%;height:20px\'></td>";
			content +="							</tr>";
			content +="							<tr>";
			content +="								<td><span>$ANCHOR$ : </span></td>";
			content +="								<td><input id=\'$EDITOR_ID$_HTB_L_anchor\' style=\'width:100%;height:20px\'></td>";
			content +="							</tr>";
			content +="							<tr>";
			content +="								<td><span>$TARGET$ : </span></td>";
			content +="								<td>";
			content +="									<select id=\'$EDITOR_ID$_HTB_L_preselectedTarget\' onchange=\\\"HTB_L_ChangeTarget('$EDITOR_ID$');\\\">";
			content +="											<option value=\'empty\'></option>";
			content +="											<option value=\'blank\'>_blank</option>";
			content +="											<option value=\'media\'>_media</option>";	
			content +="											<option value=\'parent\'>_parent</option>";	
			content +="											<option value=\'self\'>_self</option>";
			content +="											<option value=\'top\'>_top</option>";
			content +="											<option value=\'reset\'>-->Reset</option>";
			content +="											</select>";
			content +="									<input id=\'$EDITOR_ID$_HTB_L_target\'>";
			content +="								</td>";
			content +="							</tr>";
			content +="							<tr>";
			content +="								<td><span>$TOOLTIP$ : </span></td>";
			content +="								<td><input id=\'$EDITOR_ID$_HTB_L_tooltip\' style=\'width:100%;height:20px\'></td>";
			content +="							</tr>";
			content +="							<tr>";
			content +="								<td><span>$ALT_TEXT$ : </span></td>";
			content +="								<td><input id=\'$EDITOR_ID$_HTB_L_altText\' style=\'width:100%;height:20px\'></td>";
			content +="							</tr>";
			content +="							</table>";
			content +="					</td>";
			content +="				</tr>";
			content +="			</table>";
			content +="			<table align=center>";
			content +="					<td><input type=\'button\' id=\'$EDITOR_ID$_HTB_L_removeLink\' value=\'$REMOVE_LINK$\' onclick=\\\"parent.HTB_CommandBuilder('$EDITOR_ID$', 'unlink');ATB_hidePopup('$POPUP_ID$');\\\"></td>";
			content +="				<td><input type=\'button\' id=\'$EDITOR_ID$_HTB_L_insertLink\' value=\'$INSERT_LINK$\' onclick=\\\"HTB_CreateLink('$EDITOR_ID$','$POPUP_ID$')\\\"></td>";
			content +="			</table>";
			content +="		</td>";
			content +="	</tr>";
			content +="</table>";

			this.PopupContents.ContentText = content;
			this.ClientSideClick = "HTB_SetPopupPosition('$EDITOR_ID$','$POPUP_ID$');HTB_InitLink('$EDITOR_ID$');";
		}

		/// <summary>
		/// Renders at the design time.
		/// </summary>
		/// <param name="output">The output.</param>
        public override void RenderDesign(HtmlTextWriter output)
        {
#if (!FX1_1)
                if (ImageURL == string.Empty)
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.link_off.gif");
                if (OverImageURL == string.Empty)
                    this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.link_over.gif");
#endif

            this.RenderControl(output);
        }

		/// <summary>
		/// Notifies the tool to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{
#if (!FX1_1)
                if (ImageURL == string.Empty)
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.link_off.gif");
                if (OverImageURL == string.Empty)
                    this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.link_over.gif");
#endif

			base.OnPreRender(e);

			if(Page.Request.Browser.Browser.IndexOf("IE", StringComparison.OrdinalIgnoreCase) == -1)
			{
				SetContentsHeightWidthAutoContent(200, 330);
			}
			else
			{
				SetContentsHeightWidthAutoContent(213, 345);
			}

			// Get Parent Editor
			var editor = (Editor) Parent.Parent.Parent;
			if(editor != null)
			{
				PopupContents.ContentText = PopupContents.ContentText
					.Replace(ClientSideEditorId, editor.ClientID)
					.Replace("$POPUP_ID$", PopupContents.ID);
				ClientSideClick =
					$"HTB_SetPopupPosition('{editor.ClientID}','{PopupContents.ClientID}');HTB_InitLink('{editor.ClientID}');";
			}
		}
	}
}
