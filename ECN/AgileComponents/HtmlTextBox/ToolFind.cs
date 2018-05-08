using System;
using System.ComponentModel;
using System.Web.UI;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolFind"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolFind : ToolButton
	{
		private const string FindOffImage = "find_off.gif";
		private const string FindOverImage = "find_over.gif";
		private const string TitleText = "Find";

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolFind"/> class.
		/// </summary>
		public ToolFind() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolFind"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolFind(string id) : base(id)
		{ 
			_Init(id);
		}

		private void _Init(string id)
		{
			InitToolButton(
			 string.IsNullOrEmpty(id) ? $"{ToolText}{TitleText}{Editor.indexTools++}" : id,
				Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, FindOffImage),
				Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, FindOverImage),
				TitleText,
				true);

			SetPopupContents(105, 305, true, false);

			string content = string.Empty;
			content += "<table width=283>";
			content += "	<tr>";
			content += "		<td>";
			content += "			<table width=\'100%\'>";
			content += "				<tr>";
			content += "					<td width=\'5\'></td>";
			content += "					<td>";
			content += "						<table cellpadding=\'0\' cellspacing=\'0\' border=\'0\' width=\'270\'>";
			content += "							<tr>";
			content += "								<td>$FIND$ <span> :&nbsp; </span></td>";
			content += "								<td width=\'232\'>";
			content += "								<input id=\'$EDITOR_ID$_HTB_F_find\' style=\'width:229;height:20\' name=\'$EDITOR_ID$_HTB_F_find\' size=\'20\' onkeyup=\\\"HTB_F_CheckFind('$EDITOR_ID$');\\\"></td>";
			content += "							</tr>";
			content += "						</table>";
			content += "					</td>";
			content += "				</tr>";
			content += "			</table>";
			content += "			<table align=center width=\'280\'>";
			content += "				<tr>";
			content += "					<td nowrap><input type=\'checkbox\' name=\'$EDITOR_ID$_HTB_F_caseSensitive\' id=\'$EDITOR_ID$_HTB_F_caseSensitive\'>$CASE_SENSITIVE$</td>";
			content += "					<td nowrap><input type=\'checkbox\' name=\'$EDITOR_ID$_HTB_F_wholeWord\' id=\'$EDITOR_ID$_HTB_F_wholeWord\'>$WHOLE_WORD$</td>";
			content += "					<td width=5></td>";
			content += "					<td><input type=\'button\' value=\' $NEXT$ \' id=\'$EDITOR_ID$_HTB_F_next\' onclick=\\\"HTB_Find('$EDITOR_ID$',document.getElementById('$EDITOR_ID$_HTB_F_find').value,document.getElementById('$EDITOR_ID$_HTB_F_caseSensitive').checked, document.getElementById('$EDITOR_ID$_HTB_F_wholeWord').checked);\\\"></td>";
			content += "				</tr>";
			content += "			</table>";
			content += "		</td>";
			content += "	</tr>";
			content += "</table>";

			this.PopupContents.ContentText = content;
			//this.ClientSideClick = "HTB_SetPopupPosition('$EDITOR_ID$','$POPUP_ID$'); HTB_InitFind(); var selection = eval(\'$EDITOR_ID$_State\').selection; if (selection != null) document.getElementById('HTB_F_find').value = selection.text;";
		}

		/// <summary>
		/// Renders at the design time.
		/// </summary>
		/// <param name="output">The output.</param>
        public override void RenderDesign(HtmlTextWriter output)
        {
#if (!FX1_1)
                if (ImageURL == string.Empty)
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.find_off.gif");
                if (OverImageURL == string.Empty)
                    this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.find_over.gif");
#endif

            this.RenderControl(output);
        }

		/// <summary>
		/// Notifies the tool to perform any necessary prerendering steps prior to saving view state and rendering content.
		/// </summary>
		/// <param name="e">An EventArgs object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e) 
		{
			// Get Parent Editor
			Editor editor = (Editor)this.Parent.Parent.Parent;
			if (editor != null)
			{
				//this.ClientSideClick = this.ClientSideClick.Replace("$EDITOR_ID$", editor.ClientID);
				//this.ClientSideClick = this.ClientSideClick.Replace("$POPUP_ID$", this.PopupContents.ClientID);
				this.ClientSideClick = string.Format("HTB_SetPopupPosition('{0}','{1}'); HTB_InitFind('{0}'); var selection = eval(\'{0}_State\').selection; if (selection != null) document.getElementById('{0}_HTB_F_find').value = selection.text;",editor.ClientID,this.PopupContents.ClientID);
				this.PopupContents.ContentText = this.PopupContents.ContentText.Replace("$EDITOR_ID$", editor.ClientID);
				this.PopupContents.ContentText = this.PopupContents.ContentText.Replace("$POPUP_ID$", this.PopupContents.ClientID);
			}

#if (!FX1_1)
                if (ImageURL == string.Empty)
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.find_off.gif");
                if (OverImageURL == string.Empty)
                    this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.find_over.gif");
#endif

            base.OnPreRender(e);
		}
	}
}
