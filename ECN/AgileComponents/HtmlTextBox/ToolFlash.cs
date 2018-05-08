using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolFlash"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolFlash : ToolButton
	{
		private const string FlashPopup = "FlashPopup";
		private const string ToolFlashValue = "_toolFlash";
		private const string Flash = "Flash";
		private const string FlashEditor = "Flash editor";
		private const string FlashOff = "flash_off.gif";
		private const string FlashOver = "flash_over.gif";

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolFlash"/> class.
		/// </summary>
		public ToolFlash() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolFlash"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolFlash(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			this.ID = string.IsNullOrEmpty(id) ? ToolFlashValue + Editor.indexTools++ : id;
			this.PopupContents.ID = ID + FlashPopup;
			this.ImageURL = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, FlashOff);
			this.OverImageURL = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, FlashOver);
			this.ToolTip = Flash;
			this.PopupContents.TitleText = FlashEditor;
			this.PopupContents.ShowShadow = false;
			this.UsePopupOnClick = true;

			var content = new StringBuilder();

			AppendTableClsPopupHeader(content);
			AppendTableClsPopupMiddle(content);
			AppendTableClsPopupFooter(content);

			this.PopupContents.ContentText = content.ToString();
		}

		private static void AppendTableClsPopupHeader(StringBuilder content)
		{
			content.Append("<table class=\'HTB_clsPopup\'>");
			content.Append("	<tr>");
			content.Append("		<td>");
			content.Append("			<table width=\'100%\'>");
			content.Append("				<tr>");
			content.Append("					<td><span>$GENERAL$</span></td>");
			content.Append("					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>");
			content.Append("					</td>");
			content.Append("				</tr>");
			content.Append("			</table>");
			content.Append("			<table>");
			content.Append("				<tr>");
			content.Append("					<td width=\'5\'></td>");
			content.Append("					<td width=\'69\'>$FLASH_FILE$<span> :</span></td>");
			content.Append("					<td width=\'1\'></td>");
			content.Append("					<td align=right><input type=\'file\' size=\'29\' id=\'$EDITOR_ID$_HTB_FL_file\'></td>");
			content.Append("				</tr>");
			content.Append("			</table>");
			content.Append("			<table>");
			content.Append("				<tr>");
			content.Append("					<td width=\'5\'></td>");
			content.Append(
				"					<td><input type=\'checkbox\' id=\'$EDITOR_ID$_HTB_FL_specifyClassID\' onclick=\\\"HTB_FL_EnableDisableSpecifyClassID('$EDITOR_ID$');\\\">$SPECIFY_CLASS_ID$</td>");
			content.Append("					<td width=\'5\'></td>");
			content.Append("					<td><span id=\'$EDITOR_ID$_HTB_FL_textClassID\'>$CLASS_ID$: </span></td>");
			content.Append(
				"					<td><input type=\'textbox\' id=\'$EDITOR_ID$_HTB_FL_classID\' style=\'width:170;height:21\'></td>");
			content.Append("				</tr>");
			content.Append("				<tr>");
			content.Append("					<td width=\'5\'></td>");
			content.Append("					<td></td>");
			content.Append("					<td></td>");
			content.Append("					<td><span id=\'$EDITOR_ID$_HTB_FL_textFlashVersion\'>$FLASH_VERSION$ : </span></td>");
			content.Append("					<td><select style=\'width:106;height:22\' id=\'$EDITOR_ID$_HTB_FL_flashVersion\'>");
			content.Append("									<option value=\'5\'>5.0</option>");
			content.Append("									<option value=\'6\'>6.0</option>");
			content.Append("						</select>");
			content.Append("					</td>");
			content.Append("				</tr>");
			content.Append("			</table>");
			content.Append("			<table width=\'100%\'>");
			content.Append("				<tr>");
			content.Append("					<td>$DISPOSITION$</td>");
			content.Append("					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>");
			content.Append("					</td>");
			content.Append("				</tr>");
			content.Append("			</table>");
		}

		private static void AppendTableClsPopupMiddle(StringBuilder content)
		{
			content.Append("			<table width=\'100%\'>");
			content.Append("				<tr>");
			content.Append("					<td width=\'5\'></td>");
			content.Append("					<td><span>$ALIGNMENT$ :</span></td>");
			content.Append("					<td width=\'5\'></td>");
			content.Append("					<td>");
			content.Append("						<select id=\'$EDITOR_ID$_HTB_FL_alignment\'>");
			content.Append("							<option value=\'baseline\'>$BASELINE$</option>");
			content.Append("							<option value=\'bottom\'>$BOTTOM$</option>");
			content.Append("							<option value=\'left\'>$LEFT$</option>");
			content.Append("							<option value=\'middle\'>$MIDDLE$</option>");
			content.Append("							<option value=\'right\'>$RIGHT$</option>");
			content.Append("							<option value=\'textTop\'>$TEXTTOP$</option>");
			content.Append("							<option value=\'top\'>$TOP$</option>");
			content.Append("						</select>");
			content.Append("					</td>");
			content.Append("					<td width=\'5\'></td>");
			content.Append("					<td><span>$FLASH_ALIGNMENT$ : </span></td>");
			content.Append("					<td>");
			content.Append("						<select id=\'$EDITOR_ID$_HTB_FL_flashAlignment\'>");
			content.Append("							<option value=\'CT\'>$CENTER_TOP$</option>");
			content.Append("							<option value=\'CC\'>$CENTER_CENTER$</option>");
			content.Append("							<option value=\'CB\'>$CENTER_BOTTOM$</option>");
			content.Append("							<option value=\'LT\'>$LEFT_TOP$</option>");
			content.Append("							<option value=\'LC\'>$LEFT_CENTER$</option>");
			content.Append("							<option value=\'LB\'>$LEFT_BOTTOM$</option>");
			content.Append("							<option value=\'RT\'>$RIGHT_BOTTOM$</option>");
			content.Append("							<option value=\'RC\'>$RIGHT_CENTER$</option>");
			content.Append("							<option value=\'RB\'>$RIGHT_BOTTOM$</option>");
			content.Append("						</select>");
			content.Append("					</td>");
			content.Append("				</tr>");
			content.Append("			</table>");
			content.Append("			<table width=\'100%\'>");
			content.Append("				<tr>");
			content.Append("					<td><span>$SIZE$</span></td>");
			content.Append("					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>");
			content.Append("					</td>");
			content.Append("				</tr>");
			content.Append("			</table>");
			content.Append("			<table>");
			content.Append("				<tr>");
			content.Append("					<td width=\'5\'></td>");
			content.Append(
				"					<td><input type=\'checkbox\' id=\'$EDITOR_ID$_HTB_FL_specifySize\' onclick=\\\"HTB_FL_EnableDisableSpecifySize('$EDITOR_ID$');\\\">$SPECIFY_SIZE$</td>");
			content.Append("					<td width=15></td>");
			content.Append("					<td><span id=\'$EDITOR_ID$_HTB_FL_textWidth\'>$WIDTH$ :</span></td>");
			content.Append("					<td><input style=\'width:40;height:21\' id=\'$EDITOR_ID$_HTB_FL_specifyValueWidth\'></td>");
			content.Append("					<td><span id=\'$EDITOR_ID$_HTB_FL_textHeight\'>$HEIGHT$ :</span></td>");
			content.Append("					<td><input style=\'width:40;height:21\' id=\'$EDITOR_ID$_HTB_FL_specifyValueHeight\'></td>");
			content.Append("				</tr>");
			content.Append("				<tr>");
			content.Append("					<td width=\'5\'></td>");
			content.Append("					<td></td>");
			content.Append("					<td></td>");
			content.Append(
				"					<td><input type=\'radio\' name=\'$EDITOR_ID$_HTB_FL_gWidth\' id=\'$EDITOR_ID$_HTB_FL_specifyInPixelsWidth\' value=\'Pixels\'><span id=\'$EDITOR_ID$_HTB_FL_textInPixelWidth\'>$IN_PIXELS$</span></td>");
			content.Append("					<td></td>");
			content.Append("					<td>");
			content.Append(
				"					<input type=\'radio\' name=\'$EDITOR_ID$_HTB_FL_gHeight\' id=\'$EDITOR_ID$_HTB_FL_specifyInPixelsHeight\' value=\'Pixels\'><span id=\'$EDITOR_ID$_HTB_FL_textInPixelHeight\'>$IN_PIXELS$</span></td>");
			content.Append("				</tr>");
			content.Append("				<tr>");
			content.Append("					<td width=\'5\'></td>");
			content.Append("					<td></td>");
			content.Append("					<td></td>");
			content.Append("					<td>");
			content.Append(
				"					<input type=\'radio\' name=\'$EDITOR_ID$_HTB_FL_gWidth\' id=\'$EDITOR_ID$_HTB_FL_specifyInPercentWidth\' value=\'Percent\'><span id=\'$EDITOR_ID$_HTB_FL_textInPercentWidth\'>$IN_PERCENT$</span></td>");
			content.Append("					<td></td>");
			content.Append("					<td>");
			content.Append(
				"					<input type=\'radio\' name=\'$EDITOR_ID$_HTB_FL_gHeight\' id='$EDITOR_ID$_HTB_FL_specifyInPercentHeight' value='Percent'><span id='$EDITOR_ID$_HTB_FL_textInPercentHeight'>$IN_PERCENT$</span></td>");
			content.Append("				</tr>");
			content.Append("			</table>");
			content.Append("			<table width=\'100%\'>");
			content.Append("				<tr>");
			content.Append("					<td>Options</td>");
			content.Append("					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>");
			content.Append("					</td>");
			content.Append("				</tr>");
			content.Append("			</table>");
		}

		private void AppendTableClsPopupFooter(StringBuilder content)
		{
			content.Append("			<table>");
			content.Append("				<tr>");
			content.Append("					<td width=\'5\'></td>");
			content.Append("					<td>$QUALITY$ : </td>");
			content.Append("					<td width=\'5\'></td>");
			content.Append("					<td>");
			content.Append("						<select id=\'$EDITOR_ID$_HTB_FL_quality\'>");
			content.Append("							<option value=\'low\'>$LOW$</option>");
			content.Append("							<option value=\'medium\'>$MEDIUM$</option>");
			content.Append("							<option value=\'high\'>$HIGH$</option>");
			content.Append("						</select>");
			content.Append("					</td>");
			content.Append("					<td width=\'15\'></td>");
			content.Append("					<td>$LOOP$ : </td>");
			content.Append("					<td width=\'15\'></td>");
			content.Append("					<td>$AUTO_PLAY$ : </td>");
			content.Append("					<td width=\'5\'></td>");
			content.Append("				</tr>");
			content.Append("				<tr>");
			content.Append("					<td></td>");
			content.Append("					<td></td>");
			content.Append("					<td></td>");
			content.Append("					<td></td>");
			content.Append("					<td></td>");
			content.Append(
				"					<td><input type=\'radio\' name=\'$EDITOR_ID$_HTB_FL_gLoop\' id=\'$EDITOR_ID$_HTB_FL_loopYes\' value=\'yes\'>$YES$</td>");
			content.Append("					<td></td>");
			content.Append(
				"					<td><input type=\'radio\' name=\'$EDITOR_ID$_HTB_FL_gAutoRead\' id=\'$EDITOR_ID$_HTB_FL_autoReadYes\' value=\'yes\'>$YES$</td>");
			content.Append("					<td></td>");
			content.Append("				</tr>");
			content.Append("				<tr>");
			content.Append("					<td></td>");
			content.Append("					<td></td>");
			content.Append("					<td></td>");
			content.Append("					<td></td>");
			content.Append("					<td></td>");
			content.Append(
				"					<td><input type=\'radio\' name=\'$EDITOR_ID$_HTB_FL_gLoop\' id=\'$EDITOR_ID$_HTB_FL_loopNo\' value=\'no\'>$NO$</td>");
			content.Append("					<td></td>");
			content.Append(
				"					<td><input type=\'radio\' name=\'$EDITOR_ID$_HTB_FL_gAutoRead\' id=\'$EDITOR_ID$_HTB_FL_autoReadNo\' value=\'no\'>$NO$</td>");
			content.Append("					<td></td>");
			content.Append("				</tr>");
			content.Append("			</table>");
			content.Append("			<table width=\'100%\'>");
			content.Append("				<tr>");
			content.Append("					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>");
			content.Append("					</td>");
			content.Append("				</tr>");
			content.Append("			</table>");
			content.Append("			<table width=\'100%\'>");
			content.Append("				<tr>");
			content.Append($"					<td align=\'right\'><input type=\'button\' value=\'$OK$\' style=\'width:75px;height:23px\' onclick=\\\"HTB_FL_CreateFlash('$EDITOR_ID$');ATB_hidePopup('{this.PopupContents.ID}');"+
			               "\\\">&nbsp;&nbsp;<input type=\'button\' value=\'$CANCEL$\' style=\'width:75px;height:23px\' onclick=\\\"ATB_hidePopup('{this.PopupContents.ID}');\\\"></td>");
			content.Append("				</tr>");
			content.Append("			</table>");
			content.Append("		</td>");
			content.Append("	</tr>");
			content.Append("</table>");
		}

		/// <summary>
		/// Renders at the design time.
		/// </summary>
		/// <param name="output">The output.</param>
        public override void RenderDesign(HtmlTextWriter output)
        {
#if (!FX1_1)
                if (ImageURL == string.Empty)
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.flash_off.gif");
                if (OverImageURL == string.Empty)
                    this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.flash_over.gif");
#endif

            this.RenderControl(output);
        }

		/// <summary>
		/// Do some work before rendering the control.
		/// </summary> 
		/// <param name="e">Event Args</param>
		protected override void OnPreRender(EventArgs e) 
		{
			if(Page.Request.Browser.Browser.IndexOf("IE", StringComparison.OrdinalIgnoreCase) == -1)
			{
				SetContentsHeightWidthAutoContent(485, 430, false);
			}
			else
			{
				SetContentsHeightWidthAutoContent(460, 430, true);
			}

			// Get Parent Editor
			var editor = Parent?.Parent?.Parent as Editor;
			if(editor != null)
			{
				PopupContents.ContentText = PopupContents.ContentText.Replace(ClientSideEditorId, editor.ClientID);
				ClientSideClick =
					$"HTB_SetPopupPosition('{editor.ClientID}','{PopupContents.ClientID}'); HTB_FL_InitFlashEditor('{editor.ClientID}');";
			}

#if (!FX1_1)
                if (ImageURL == string.Empty)
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.flash_off.gif");
                if (OverImageURL == string.Empty)
                    this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.flash_over.gif");
#endif

            base.OnPreRender(e);
		}
	}
}
