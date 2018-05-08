using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;

namespace ActiveUp.WebControls
{
    /// <summary>
    /// Represents a <see cref="ToolCanvasSize"/> object.
    /// </summary>
    [ToolboxItem(false)]
    public class ToolCanvasSize : ToolPixBase, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {
        public override string ImageName
        {
            get
            {
                return "canvassize_off.gif";
            }
        }

        public override string OverImageName
        {
            get
            {
                return "canvassize_over.gif";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolCanvasSize"/> class.
        /// </summary>
        public ToolCanvasSize() : base()
        {
            Initialize(string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolCanvasSize"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public ToolCanvasSize(string id) : base(id)
        {
            Initialize(id);
        }

        private void Initialize(string id)
        {
            if (id == string.Empty)
            {
                this.ID = "_toolCanvasSize" + ImageEditor.indexTools++;
            }
            else
            {
                this.ID = id;
            }

            this.PopupContents.ID = ID + "CanvasSize";

            SetButtonImagesIfNotFx11(string.Empty, string.Empty);
            SetButtonImagesIfFx11(ImageName, OverImageName);

            this.ToolTip = "CanvasSize";
            this.UsePopupOnClick = true;
            this.PopupContents.Height = 140;
            this.PopupContents.Width = 240;
            this.PopupContents.TitleText = "Image Size";
            this.PopupContents.AutoContent = true;
        }

        /// <summary>
        /// Do some work before rendering the control.
        /// </summary> 
        /// <param name="e">Event Args</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.PopupContents.ContentText = string.Format("<table><tr><td nowrap class={3}>Actual size : <b>{4}</b>x<b>{5}</b></td><td rowspan=3 class={3} valign=top align=right><input type=\'button\' value=\'   Ok   \' onclick=\\\"{0}\\\"></td></tr><tr><td nowrap class={3}>Width : <input type=\'text\' size=4 value={4} id=\'{1}W\'></td></tr><tr><td nowrap class={3}>Height : <input type=\'text\' value={5} size=4 id=\'{1}H\'></td></tr><tr><td nowrap class={3}>Anchor : <select id=\'{1}A\'><option value=\'TopLeft\'>Top Left</option><option value=\'TopCenter\'>Top Center</option><option value=\'TopRight\'>Top Right</option><option value=\'MiddleLeft\'>Middle Left</option><option value=\'MiddleCenter\'>Middle Center</option><option value=\'MiddleRight\'>Middle Right</option><option value=\'BottomLeft\'>Bottom Left</option><option value=\'BottomCenter\'>Bottom Center</option><option value=\'BottomRight\'>Bottom Right</option></select></td></tr></table>", this.Page.GetPostBackClientEvent(this, string.Format("' + AIE_getObject('{0}W').value + ';' + AIE_getObject('{0}H').value + ';' + AIE_getObject('{0}A').options[AIE_getObject('{0}A').selectedIndex].value + '", this.ClientID)).Replace("\\", ""), this.ClientID, this.ClientID.Replace(":", "_"), ParentImageEditor.TdCssClass, ParentImageEditor.ImageWidth.ToString(), ParentImageEditor.ImageHeight.ToString());
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
                int width, height;

                int.TryParse(args[0], out width);
                int.TryParse(args[1], out height);

                var anchor = (AnchorType)Enum.Parse(typeof(AnchorType), args[2], true);

                job.ResizeCanvas(width, height, anchor, Color.Black);

                ParentImageEditor.ImageHeight = job.Image.Height;
                ParentImageEditor.ImageWidth = job.Image.Width;
            }
            catch
            {
                Page.RegisterStartupScript(tempFileName, "<script language='javascript'>alert('Error while resizing.');</script>");
            }

            PerformSave(job, tempFileName);

            OnClick(EventArgs.Empty);
        }
    }
}
