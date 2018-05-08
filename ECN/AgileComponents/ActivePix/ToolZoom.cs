using System;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls
{
    /// <summary>
    /// Represents a <see cref="ToolZoom"/> object.
    /// </summary>
    [ToolboxItem(false)]
    public class ToolZoom : ToolPixBase, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {
        public override string ImageName
        {
            get
            {
                return "zoom_off.gif";
            }
        }

        public override string OverImageName
        {
            get
            {
                return "zoom_over.gif";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolZoom"/> class.
        /// </summary>
        public ToolZoom() : base()
        {
            Initialize(string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolZoom"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public ToolZoom(string id) : base(id)
        {
            Initialize(id);
        }

        private void Initialize(string id)
        {
            if (id == string.Empty)
            {
                this.ID = "_toolZoom" + ImageEditor.indexTools++;
            }
            else
            {
                this.ID = id;
            }

            this.PopupContents.ID = ID + "Zoom";

            SetButtonImagesIfNotFx11(string.Empty, string.Empty);
            SetButtonImagesIfFx11(ImageName, OverImageName);

            this.ToolTip = "Zoom";
            this.UsePopupOnClick = true;
            this.PopupContents.Height = 60;
            this.PopupContents.Width = 180;
            this.PopupContents.TitleText = "Zoom";
            this.PopupContents.AutoContent = true;
        }

        /// <summary>
        /// Do some work before rendering the control.
        /// </summary> 
        /// <param name="e">Event Args</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.PopupContents.ContentText = string.Format("<table><tr><td nowrap class={3}>Factor : <input id=\'{1}F\' value=\'100\' size=\'3\'></td><td class={3} valign=top align=right><input type=\'button\' value=\'   Ok   \' onclick=\\\"{0}\\\"></td></tr></table>", this.Page.GetPostBackClientEvent(this, string.Format("' + AIE_getObject('{0}F').value + '", this.ClientID)).Replace("\\", ""), this.ClientID, this.ClientID.Replace(":", "_"), ParentImageEditor.TdCssClass);
            this.ClientSideClick = string.Format("ATB_movePopupTo('{0}', AIE_findPosX(AIE_getObject('{1}workImage')) + 10, AIE_findPosY(AIE_getObject('{1}workImage')) + 10);", this.PopupContents.ID, ParentImageEditor.ClientID);
        }

        /// <summary>
        /// A RaisePostBackEvent.
        /// </summary>
        /// <param name="eventArgument">eventArgument</param>
        public new void RaisePostBackEvent(String eventArgument)
        {
            // Create the new name
            var tempFileName = ParentImageEditor.GetTempFileName();

            var job = new ImageJob(ParentImageEditor.WorkFile);

            ParentImageEditor.ImageHeight = job.Image.Height;
            ParentImageEditor.ImageWidth = job.Image.Width;

            if ((ParentImageEditor.Selection.X1 > 0 && ParentImageEditor.Selection.Y1 > 0) || (ParentImageEditor.Selection.X2 > 0 && ParentImageEditor.Selection.Y2 > 0))
            {
                float factor;
                float.TryParse(eventArgument, out factor);

                job.Zoom(ParentImageEditor.Selection.X1, ParentImageEditor.Selection.Y1, factor);

                PerformSave(job, tempFileName);
            }
            else
            {
                Page.RegisterStartupScript(tempFileName, "<script language='javascript'>alert('Please click on the image to set the center point.');</script>");
                job.Dispose();
            }

            OnClick(EventArgs.Empty);
        }
    }
}