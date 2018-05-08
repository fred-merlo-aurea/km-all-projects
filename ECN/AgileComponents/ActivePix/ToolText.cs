using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;

namespace ActiveUp.WebControls
{
    /// <summary>
    /// Represents a <see cref="ToolText"/> object.
    /// </summary>
    [ToolboxItem(false)]
    public class ToolText : ToolPixBase, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {
        public override string ImageName
        {
            get
            {
                return "text_off.gif";
            }
        }

        public override string OverImageName
        {
            get
            {
                return "text_over.gif";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolText"/> class.
        /// </summary>
        public ToolText() : base()
        {
            Initialize(string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolText"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public ToolText(string id) : base(id)
        {
            Initialize(id);
        }

        private void Initialize(string id)
        {
            if (id == string.Empty)
            {
                this.ID = "_toolText" + ImageEditor.indexTools++;
            }
            else
            {
                this.ID = id;
            }

            this.PopupContents.ID = ID + "Text";

            SetButtonImagesIfNotFx11(string.Empty, string.Empty);
            SetButtonImagesIfFx11(ImageName, OverImageName);

            this.ToolTip = "Text";
            this.UsePopupOnClick = true;
            this.PopupContents.Height = 130;
            this.PopupContents.Width = 480;
            this.PopupContents.TitleText = "Text";
            this.PopupContents.AutoContent = true;
        }

        /// <summary>
        /// Do some work before rendering the control.
        /// </summary> 
        /// <param name="e">Event Args</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var content = string.Empty;

            content = "<table><tr><td nowrap class={3}>Text : <input type=\'text\' size=20 id=\'{1}T\'></td><td rowspan=3 class={3} valign=top align=right><input type=\'button\' value=\'   Ok   \' onclick=\\\"{0}\\\"></td></tr><tr><td nowrap class={3}>Font : <select id=\'{1}F\'><option value=\'Arial\'>Arial</option><option value=\'Arial Black\'>Arial Black</option><option value=\'Arial Narrow\'>Arial Narrow</option><option value=\'Comic Sans MS\'>Comic Sans MS</option><option value=\'Courier New\'>Courier New</option><option value=\'System\'>System</option><option value=\'Times New Roman\'>Times New Roman</option><option value=\'Verdana\'>Verdana</option><option value=\'Wingdings\'>Wingdings</option></select> Size : <select id=\'{1}S\'><option value=\'6\'>6</option><option value=\'8\'>8</option><option value=\'10\'>10</option><option value=\'11\'>11</option><option value=\'12\' selected>12</option><option value=\'14\'>14</option><option value=\'16\'>16</option><option value=\'18\'>18</option><option value=\'22\'>22</option><option value=\'26\'>26</option><option value=\'32\'>32</option></select> ";
            content += "Style : <select id=\'{1}L\'><option value=\'Regular\'>Regular</option><option value=\'Bold\'>Bold</option><option value=\'Italic\'>Italic</option><option value=\'Underline\'>Underline</option><option value=\'Strikeout\'>Strikeout</option></select></td></tr><tr><td nowrap class={3}>Color : <select id=\'{1}C\'><option value=\'Black\'>Black</option><option value=\'Maroon\'>Maroon</option><option value=\'Olive\'>Olive</option><option value=\'Green\'>Green</option><option value=\'Teal\'>Teal</option><option value=\'Navy\'>Navy</option><option value=\'Purple\'>Purple</option><option value=\'White\'>White</option><option value=\'Silver\'>Silver</option><option value=\'Red\'>Red</option><option value=\'Yellow\'>Yellow</option><option value=\'Lime\'>Lime</option><option value=\'Aqua\'>Aqua</option><option value=\'Blue\'>Blue</option><option value=\'Fuchsia\'>Fuchsia</option></select> Aligmnent : <select id=\'{1}G\'><option value=\'Near\'>Left</option><option value=\'Center\'>Center</option><option value=\'Far\'>Right</option></select> <input type=\'checkbox\' Checked id=\'{1}A\'> Antialias</td></tr></table>";

            this.PopupContents.ContentText = string.Format(content, this.Page.GetPostBackClientEvent(this, string.Format("' + AIE_getObject('{0}T').value + ';' + AIE_getObject('{0}C').options[AIE_getObject('{0}C').selectedIndex].value + ';' + AIE_getObject('{0}L').options[AIE_getObject('{0}L').selectedIndex].value + ';' + AIE_getObject('{0}S').options[AIE_getObject('{0}S').selectedIndex].value + ';' + AIE_getObject('{0}F').options[AIE_getObject('{0}F').selectedIndex].value + ';' + AIE_getObject('{0}A').checked + ';' + AIE_getObject('{0}G').options[AIE_getObject('{0}G').selectedIndex].value +  '", this.ClientID)).Replace("\\", ""), this.ClientID, this.ClientID.Replace(":", "_"), ParentImageEditor.TdCssClass, ParentImageEditor.ImageWidth.ToString(), ParentImageEditor.ImageHeight.ToString());
            this.ClientSideClick = string.Format("ATB_movePopupTo('{0}', AIE_findPosX(AIE_getObject('{1}workImage')) + 10, AIE_findPosY(AIE_getObject('{1}workImage')) + 10);", this.PopupContents.ID, ParentImageEditor.ClientID);
        }

        /// <summary>
        /// A RaisePostBackEvent.
        /// </summary>
        /// <param name="eventArgument">eventArgument</param>
        public new void RaisePostBackEvent(String eventArgument)
        {
            var args = eventArgument.Split(';');

            // Create the new name
            var tempFileName = ParentImageEditor.GetTempFileName();

            var job = new ImageJob(ParentImageEditor.WorkFile);

            try
            {
                int size;
                int.TryParse(args[3], out size);

                bool antialias;
                bool.TryParse(args[5], out antialias);

                var foreColor = Color.FromName(args[1]);
                var fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), args[2], true);
                var stringAlignment = (StringAlignment)Enum.Parse(typeof(StringAlignment), args[6], true);

                job.AddText(args[0], args[4], size, fontStyle, foreColor, antialias,
                    ParentImageEditor.Selection.X1, ParentImageEditor.Selection.Y1, stringAlignment);
            }
            catch
            {
                Page.RegisterStartupScript(tempFileName, "<script language='javascript'>alert('Error while adding text.');</script>");
            }

            PerformSave(job, tempFileName);

            OnClick(EventArgs.Empty);
        }
    }
}