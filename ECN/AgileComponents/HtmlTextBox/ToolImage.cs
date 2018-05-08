using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ToolImage"/> object.
	/// </summary>
    [ToolboxItem(false)]
	public class ToolImage : ToolButton 
	{
		private const string ImageUrlValue = "image_off.gif";
		private const string OverImageUrlValue = "image_over.gif";
		private const string ImageEditorText = "Image Editor";
		private const string ClientSideClickValue = "HTB_SetPopupPosition('$EDITOR_ID$','$POPUP_ID$'); HTB_InitImageEditor('$EDITOR_ID$');";
		private const string ImageText = "Image";
		private const int PopupContentsHeight = 440;
		private const int PopupContentsWidth = 432;

		private const string HtmlTemplate = "<table class=\'HTB_clsPopup\'>"
											+ "	<tr>"
											+ "		<td>"
											+ "			<table width=\'100%\'>"
											+ "				<tr>"
											+ "					<td><span>$GENERAL$</span></td>"
											+ "					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>"
											+ "					</td>"
											+ "				</tr>"
											+ "			</table>"
											+ "			<table>"
											+ "				<tr>"
											+ "					<td width=\'5\'></td>"
											+ "					<td><span>$PICTURE$ :</span></td>"
											+ "					<td width=\'1\'></td>"
											+ "					<td align=left><input type=\'text\' size=\'45\' id=\'HTB_IE_picture\'></td>"
											+ "				</tr>"
											+ "				<tr>"
											+ "					<td width=\'5\'></td>"
											+ "					<td width=\'69\'><span>$TEXT$ :</span></td>"
											+ "					<td width=\'1\'></td>"
											+ "					<td align=right><input size=\'43\' id=\'HTB_IE_text\'></td>"
											+ "				</tr>"
											+ "			</table>"
											+ "			<table width=\'100%\'>"
											+ "				<tr>"
											+ "					<td><span>$DISPOSITION$</span></td>"
											+ "					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>"
											+ "					</td>"
											+ "				</tr>"
											+ "			</table>"
											+ "			<table width=\'100%\'>"
											+ "				<tr>"
											+ "					<td width=\'5\'></td>"
											+ "					<td>"
											+ "						<table cellpadding=\'0\' cellspacing=\'0\' border=\'0\'>"
											+ "							<tr>"
											+ "								<td><span>$ALIGNMENT$ :</span></td>"
											+ "								<td><select style=\'width:106;height:22\' id=\'HTB_IE_alignment\'>"
											+ "									<option value=\'default\'>$DEFAULT$</option>"
											+ "									<option value=\'absbottom\'>$ABSBOTTOM$</option>"
											+ "									<option value=\'absmiddle\'>$ABSMIDDLE$</option>"
											+ "									<option value=\'baseline\'>$BASELINE$</option>"
											+ "									<option value=\'bottom\'>$BOTTOM$</option>"
											+ "									<option value=\'left\'>$LEFT$</option>"
											+ "									<option value=\'middle\'>$MIDDLE$</option>"
											+ "									<option value=\'right\'>$RIGHT$</option>"
											+ "									<option value=\'texttop\'>$TEXTTOP$</option>"
											+ "									<option value=\'top\'>$TOP$</option>"
											+ "									</select>"
											+ "								</td>"
											+ "								<td width=9>&nbsp;</td>"
											+ "								<td><span>$HORIZONTAL_SPACING$ :</span></td>"
											+ "								<td width=12></td>"
											+ "								<td align=right><input style=\'width:40;height:21\' id=\'HTB_IE_horizontalSpacing\'></td>"
											+ "							</tr>"
											+ "							<tr>"
											+ "								<td><span>$BORDER_THICKNESS$ :</span></td>"
											+ "								<td align=right><input style=\'width:40px;height:21px\' id=\'HTB_IE_borderThickness\'></td>"
											+ "								<td></td>"
											+ "								<td><span>$VERTICAL_SPACING$ :</span></td>"
											+ "								<td></td>"
											+ "								<td><input style=\'width:40px;height:21px\' id=\'HTB_IE_verticalSpacing\'></td>"
											+ "							</tr>"
											+ "						</table>"
											+ "					</td>"
											+ "				</tr>"
											+ "			</table>"
											+ "			<table width=\'100%\'>"
											+ "				<tr>"
											+ "					<td><span>$SIZE$</span></td>"
											+ "					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>"
											+ "					</td>"
											+ "				</tr>"
											+ "			</table>"
											+ "			<table>"
											+ "				<tr>"
											+ "					<td><input type=checkbox id=\'HTB_IE_specifySize\' onclick=\\\"HTB_IE_EnableDisableSpecifySize()\\\">$SPECIFY_SIZE$</td>"
											+ "					<td width=15></td>"
											+ "					<td>$WIDTH$:</td>"
											+ "					<td><input style=\'width:40;height:21\' id=\'HTB_IE_specifyValueWidth\' onkeyup=\\\"HTB_KeepImageAspectRatio(this);\\\" onmouseup=\\\"HTB_KeepImageAspectRatio(this);\\\"></td>"
											+ "					<td>$HEIGHT$:&nbsp;&nbsp; </td>"
											+ "					<td><input style=\'width:40;height:21\' id=\'HTB_IE_specifyValueHeight\' onkeyup=\\\"HTB_KeepImageAspectRatio(this);\\\" onmouseup=\\\"HTB_KeepImageAspectRatio(this);\\\"></td>"
											+ "				</tr>"
											+ "				<tr>"
											+ "					<td></td>"
											+ "					<td></td>"
											+ "					<td></td>"
											+ "					<td><input type=\'radio\' name=\'HTB_IE_gWidth\' id=\'HTB_IE_specifyInPixelsWidth\' onclick=\\\"HTB_IE_VerifyAspectRationCanBeUsed();\\\" value=\'Pixels\'>$IN_PIXELS$</td>"
											+ "					<td></td>"
											+ "					<td>"
											+ "					<input type=\'radio\' name=\'HTB_IE_gHeight\' id=\'HTB_IE_specifyInPixelsHeight\' onclick=\\\"HTB_IE_VerifyAspectRationCanBeUsed();\\\" value=\'Pixels\'>$IN_PIXELS$</td>"
											+ "				</tr>"
											+ "				<tr>"
											+ "					<td></td>"
											+ "					<td></td>"
											+ "					<td></td>"
											+ "					<td>"
											+ "					<input type=\'radio\' name=\'HTB_IE_gWidth\' id=\'HTB_IE_specifyInPercentWidth\' onclick=\\\"HTB_IE_VerifyAspectRationCanBeUsed();\\\" value=\'Percent\'>$IN_PERCENT$</td>"
											+ "					<td></td>"
											+ "					<td>"
											+ "					<input type=\'radio\' name=\'HTB_IE_gHeight\' id=\'HTB_IE_specifyInPercentHeight\' onclick=\\\"HTB_IE_VerifyAspectRationCanBeUsed();\\\" value=\'Percent\'>$IN_PERCENT$</td>"
											+ "				</tr>"
											+ "			</table>"
											+ "			<table>"
											+ "				<tr>"
											+ "					<td><span><input type=\'checkbox\' id=\'HTB_IE_keepAspectRatio\' onclick=\\\"HTB_SetImageAspectRatio();\\\">$KEEP_ASPECT_RATIO$</span></td>"
											+ "				</tr>"
											+ "			</table>"
											+ "			<table width=\'100%\'>"
											+ "				<tr>"
											+ "					<td><span>$STYLE$</span></td>"
											+ "					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>"
											+ "					</td>"
											+ "				</tr>"
											+ "			</table>"
											+ "			<table>"
											+ "				<tr>"
											+ "					<td width=\'5\'></td>"
											+ "					<td width=\'69\'><span>$CSS_CLASS$ :</span></td>"
											+ "					<td width=\'1\'></td>"
											+ "					<td align=right><input size=\'43\' id=\'HTB_IE_cssClass\'></td>"
											+ "				</tr>"
											+ "			</table>"
											+ "			<table width=\'100%\'>"
											+ "				<tr>"
											+ "					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>"
											+ "					</td>"
											+ "				</tr>"
											+ "			</table>"
											+ "			<table width=\'100%\'>"
											+ "				<tr>"
											+ "					<td align=\'right\'><input type=\'button\' value=\'$OK$\' style=\'width:75px;height:23px\' onclick=\\\"HTB_CreateImage('$EDITOR_ID$');ATB_hidePopup('$POPUP_ID$');\\\">&nbsp;&nbsp;<input type=\'button\' value=\'$CANCEL$\' style=\'width:75px;height:23px\' onclick=\\\"ATB_hidePopup('$POPUP_ID$');\\\"></td>"
											+ "				</tr>"
											+ "			</table>"
											+ "		</td>"
											+ "	</tr>"
											+ "</table>";

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolImage"/> class.
		/// </summary>
		public ToolImage() : base()
		{
			_Init(string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolImage"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public ToolImage(string id) : base(id)
		{
			_Init(id);
		}

		private void _Init(string id)
		{
			this.ID = id.IsNullOrWhiteSpace()
				          ? $"_toolImage{Editor.indexTools++}"
				          : id;

			this.ImageURL = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, ImageUrlValue);
			this.OverImageURL = Fx1ConditionalHelper<string>.GetFx1ConditionalValue(string.Empty, OverImageUrlValue);

			this.ToolTip = ImageText;
			this.PopupContents.Height = PopupContentsHeight;
			this.PopupContents.Width = PopupContentsWidth;
			this.PopupContents.TitleText = ImageEditorText;
			this.UsePopupOnClick = true;
			
			this.PopupContents.ContentText = HtmlTemplate;
			this.ClientSideClick = ClientSideClickValue;
		}



		/// <summary>
		/// Renders at the design time.
		/// </summary>
		/// <param name="output">The output.</param>
        public override void RenderDesign(HtmlTextWriter output)
        {
#if (!FX1_1)
                if (ImageURL == string.Empty)
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.image_off.gif");
                if (OverImageURL == string.Empty)
                    this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.image_over.gif");
#endif

            this.RenderControl(output);
        }

		/// <summary>
		/// Do some work before rendering the control.
		/// </summary> 
		/// <param name="e">Event Args</param>
		protected override void OnPreRender(EventArgs e) 
		{
			// Get Parent Editor
			Editor editor = (Editor)this.Parent.Parent.Parent;
			this.PopupContents.ID = editor.ClientID + "EditorPopup";
			this.PopupContents.ContentText = this.PopupContents.ContentText.Replace("$EDITOR_ID$",editor.ClientID);
			this.PopupContents.ContentText = this.PopupContents.ContentText.Replace("$POPUP_ID$",this.PopupContents.ID);
			this.ClientSideClick = this.ClientSideClick.Replace("$EDITOR_ID$",editor.ClientID);
			this.ClientSideClick = this.ClientSideClick.Replace("$POPUP_ID$",this.PopupContents.ID);

			bool isNs6 = false;
			System.Web.HttpBrowserCapabilities browser = Page.Request.Browser; 
			if (browser.Browser.ToUpper().IndexOf("IE") == -1)
				isNs6 = true;

			if (isNs6)
			{
				this.PopupContents.AutoContent = false;
			}
			else
			{
				this.PopupContents.AutoContent = true;
			}

#if (!FX1_1)

                if (ImageURL == string.Empty)
                    this.ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.image_off.gif");
                if (OverImageURL == string.Empty)
                    this.OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), "ActiveUp.WebControls._resources.Images.image_over.gif");
#endif

            base.OnPreRender(e);

		}
	}
}
