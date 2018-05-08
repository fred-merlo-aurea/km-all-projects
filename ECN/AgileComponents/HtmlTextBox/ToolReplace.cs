using System;
using System.ComponentModel;
using System.Web.UI;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolReplace"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolReplace : ToolButton
	{
		private const string ReplaceText = "Replace";
		private const string ReplaceOffImage = "replace_off.gif";
		private const string ReplaceOverImage = "replace_over.gif";
		private const string TitleText = "Find and Replace";

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolReplace"/> class.
		/// </summary>
		public ToolReplace() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolReplace"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolReplace(string id) : base(id)
		{ 
			_Init(id); 
		}

		private void _Init(string id)
		{
			InitToolButton(
			    string.IsNullOrEmpty(id) ? $"{ToolText}{ReplaceText}{Editor.indexTools++}" : id,
				Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, ReplaceOffImage),
				Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, ReplaceOverImage),
				TitleText,
				true);

			SetPopupContents(165, 325, false, false);

			string content = string.Empty;
			/*content = "<table height=128>";
			content +=  "<tr>";
			content += 	 "<td>Find</td>";
			content += 	 "<td><input type=\'textbox\' name=\'find\' id=\'find\' size=\'30\' value=\'\'></td>";
			content +=  "</tr>";
			content +=  "<tr>";
			content += 	 "<td>Replace</td>";
			content += 	 "<td><input type=\'textbox\' name=\'replace\' id=\'replace\' size=\'30\' value=\'\'></td>";
			content +=  "</tr>";
			content +=  "<tr>";
			content += 	  "<td colspan=2>";
			content += 		 "<input type=\'checkbox\' name=\'caseSensitive\' id=\'caseSensitive\'>Case Sensitive";
			content += 		 "<input type=\'checkbox\' name=\'wholeWord\' id=\'wholeWord\'>Whole Word";
			content += 		 "<input type=\'checkbox\' name=\'queryPrompt\' id=\'queryPrompt\'>Prompt";
			content += 	 "</td>";
			content +=  "</tr>";
			content +=  "<tr>";
			content += 	 "<td colspan=2 align=center><input type=\'button\' value=\'Replace\' onclick=\\\"HTB_FindAndReplace('$EDITOR_ID$','$POPUP_ID$',find.value,replace.value,caseSensitive.checked, wholeWord.checked, queryPrompt.checked);\\\"></td>";
			content +=  "</tr>";
			content +=  "</table>";*/

			content += "<table height=128>";
			content += "	<tr>";
			content += "		<td>$FIND$</td>";
			content += "		<td><input type='textbox' name='$EDITOR_ID$_HTB_FR_find' id='$EDITOR_ID$_HTB_FR_find' size='35' value='' onkeyup=\\\"HTB_FR_CheckFindAndReplace('$EDITOR_ID$');\\\"></td>";
			content += "	</tr>";
			content += "	<tr>";
			content += "		<td>$REPLACE$</td>";
			content += "		<td><input type='textbox' name='$EDITOR_ID$_HTB_FR_replace' id='$EDITOR_ID$_HTB_FR_replace' size='35' value=''></td>";
			content += "	</tr>";
			content += "	<tr>";
			content += "		<td colspan=2 align=center>";
			content += "			<input type='checkbox' name='$EDITOR_ID$_HTB_FR_caseSensitive' id='$EDITOR_ID$_HTB_FR_caseSensitive'>$CASE_SENSITIVE$";
			content += "			<input type='checkbox' name='$EDITOR_ID$_HTB_FR_wholeWord' id='$EDITOR_ID$_HTB_FR_wholeWord'>$WHOLE_WORD$";
			content += "		</td>";
			content += "	</tr>";
			content += "	<tr>";
			content += "		<td colspan=2 align=center>";
			content += "			<input type='button' value='$REPLACE_ALL$' id='$EDITOR_ID$_HTB_FR_replaceAll' onclick=\\\"HTB_ReplaceAll('$EDITOR_ID$',$EDITOR_ID$_HTB_FR_find.value,$EDITOR_ID$_HTB_FR_replace.value,$EDITOR_ID$_HTB_FR_caseSensitive.checked, $EDITOR_ID$_HTB_FR_wholeWord.checked);\\\">";
			content += "			<input type='button' value='$REPLACEB$' id='$EDITOR_ID$_HTB_FR_replaceOne' onclick=\\\"HTB_Replace('$EDITOR_ID$',$EDITOR_ID$_HTB_FR_find.value,$EDITOR_ID$_HTB_FR_replace.value,$EDITOR_ID$_HTB_FR_caseSensitive.checked, $EDITOR_ID$_HTB_FR_wholeWord.checked);\\\">";
			content += "			<input type='button' value=' $NEXT$ ' id='$EDITOR_ID$_HTB_FR_next' onclick=\\\"HTB_Find('$EDITOR_ID$',$EDITOR_ID$_HTB_FR_find.value,$EDITOR_ID$_HTB_FR_caseSensitive.checked, $EDITOR_ID$_HTB_FR_wholeWord.checked);\\\">";
			content += "			<input type='button' value='$CANCEL$' id='$EDITOR_ID$_HTB_FR_cancel' onclick=\\\"ATB_hidePopup('$POPUP_ID$')\\\">";
			content += "		</td>";
			content += "	</tr>";
			content += "</table>";

			this.PopupContents.ContentText = content;
			//this.ClientSideClick = "HTB_SetPopupPosition('$EDITOR_ID$','$POPUP_ID$'); HTB_InitFindAndReplace(); var selection = eval(\'$EDITOR_ID$_State\').selection; if (selection != null) HTB_FR_find.value = selection.text;";
			
		}

		/// <summary>
		/// Renders at the design time.
		/// </summary>
		/// <param name="output">The output.</param>
        public override void RenderDesign(HtmlTextWriter output)
        {
#if (!FX1_1)
                if (ImageURL == string.Empty)
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.replace_off.gif");
                if (OverImageURL == string.Empty)
                    this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.replace_over.gif");
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
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.replace_off.gif");
                if (OverImageURL == string.Empty)
                    this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.replace_over.gif");
#endif
			base.OnPreRender(e);

			// Get Parent Editor
			Editor editor = (Editor)this.Parent.Parent.Parent;
			if (editor != null)
			{
				//this.ClientSideClick = this.ClientSideClick.Replace("$EDITOR_ID$", editor.ClientID);
				//this.ClientSideClick = this.ClientSideClick.Replace("$POPUP_ID$", this.PopupContents.ClientID);
				this.ClientSideClick = string.Format("HTB_SetPopupPosition('{0}','{1}'); HTB_InitFindAndReplace('{0}'); var selection = eval(\'{0}_State\').selection; if (selection != null) {0}_HTB_FR_find.value = selection.text;",editor.ClientID,this.PopupContents.ClientID);
				this.PopupContents.ContentText = this.PopupContents.ContentText.Replace("$EDITOR_ID$", editor.ClientID);
				this.PopupContents.ContentText = this.PopupContents.ContentText.Replace("$POPUP_ID$", this.PopupContents.ClientID);
			}
		}

        protected override void Render(HtmlTextWriter output)
        {
            try
            {
                if (Page != null && Page.Request.Browser.Browser.ToUpper().IndexOf("IE") >= 0)
                {
                    base.Render(output);
                }
            }
            catch { }
        }
	}
}
