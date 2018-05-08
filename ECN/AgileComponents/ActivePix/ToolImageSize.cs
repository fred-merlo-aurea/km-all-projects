using System;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls
{
    /// <summary>
    /// Represents a <see cref="ToolImageSize"/> object.
    /// </summary>
    [ToolboxItem(false)]
    public class ToolImageSize : ToolPixBase, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {
        public override string ImageName
        {
            get
            {
                return "imagesize_off.gif";
            }
        }

        public override string OverImageName
        {
            get
            {
                return "imagesize_over.gif";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolImageSize"/> class.
        /// </summary>
        public ToolImageSize() : base()
        {
            Initialize(string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolImageSize"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public ToolImageSize(string id) : base(id)
        {
            Initialize(id);
        }

        private void Initialize(string id)
        {
            if (id == string.Empty)
            {
                this.ID = "_toolImageSize" + ImageEditor.indexTools++;
            }
            else
            {
                this.ID = id;
            }

            this.PopupContents.ID = ID + "ImageSize";

            SetButtonImagesIfNotFx11(string.Empty, string.Empty);
            SetButtonImagesIfFx11(ImageName, OverImageName);

            this.ToolTip = "ImageSize";
            this.UsePopupOnClick = true;
            this.PopupContents.Height = 130;
            this.PopupContents.Width = 200;
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

            this.PopupContents.ContentText = string.Format("<table><tr><td nowrap class={3}>Width : <input type=\'text\' size=4 value={4} id=\'{1}W\'></td><td rowspan=2 class={3} valign=top align=right><input type=\'button\' value=\'   Ok   \' onclick=\\\"{0}\\\"></td></tr><tr><td nowrap class={3}>Height : <input type=\'text\' value={5} size=4 id=\'{1}H\'></td></tr><tr><td nowrap class={3}><input type=\'checkbox\' Checked id=\'{1}C\'> Keep Proportions<br><input type=\'checkbox\' Checked id=\'{1}R\'> Resize Smaller</td></tr></table>", this.Page.GetPostBackClientEvent(this, string.Format("' + AIE_getObject('{0}W').value + ';' + AIE_getObject('{0}H').value + ';' + AIE_getObject('{0}C').checked + ';' + AIE_getObject('{0}R').checked + '", this.ClientID)).Replace("\\", ""), this.ClientID, this.ClientID.Replace(":", "_"), ParentImageEditor.TdCssClass, ParentImageEditor.ImageWidth.ToString(), ParentImageEditor.ImageHeight.ToString());
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
                bool constrainProportions, resizeSmaller;

                int.TryParse(args[0], out width);
                int.TryParse(args[1], out height);
                bool.TryParse(args[2], out constrainProportions);
                bool.TryParse(args[3], out resizeSmaller);

                job.ResizeImage(width, height, constrainProportions, resizeSmaller);

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
