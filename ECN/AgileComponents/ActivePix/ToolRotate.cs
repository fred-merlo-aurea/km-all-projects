using System;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls
{
    /// <summary>
    /// Represents a <see cref="ToolRotate"/> object.
    /// </summary>
    [ToolboxItem(false)]
    public class ToolRotate : ToolPixBase, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {
        public override string ImageName
        {
            get
            {
                return "rotate_off.gif";
            }
        }

        public override string OverImageName
        {
            get
            {
                return "rotate_over.gif";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolRotate"/> class.
        /// </summary>
        public ToolRotate() : base()
        {
            Initialize(string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolRotate"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public ToolRotate(string id) : base(id)
        {
            Initialize(id);
        }

        private void Initialize(string id)
        {
            if (id == string.Empty)
            {
                this.ID = "_toolRotate" + ImageEditor.indexTools++;
            }
            else
            {
                this.ID = id;
            }

            this.PopupContents.ID = ID + "Rotate";

            SetButtonImagesIfNotFx11(string.Empty, string.Empty);
            SetButtonImagesIfFx11(ImageName, OverImageName);

            this.ToolTip = "Rotate";
            this.UsePopupOnClick = true;
            this.PopupContents.Height = 90;
            this.PopupContents.Width = 200;
            this.PopupContents.TitleText = "Rotate";
            this.PopupContents.AutoContent = true;
        }

        /// <summary>
        /// Do some work before rendering the control.
        /// </summary> 
        /// <param name="e">Event Args</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.PopupContents.ContentText = string.Format("<table><tr><td nowrap class={3}><input type=\'radio\' id=\'{1}L\' name=\'rot\' Checked> Left <input type=\'radio\' name=\'rot\' id=\'{1}R\'> Right</td><td rowspan=2 class={3} valign=top align=right><input type=\'button\' value=\'   Ok   \' onclick=\\\"{0}\\\"></td></tr><tr><td nowrap class={3}><input type=\'radio\' name=\'rot\' id=\'{1}A\'> Arbitrary : <input type=\'text\' size=3 id=\'{1}V\' value=\'0\'></td></tr></table>", this.Page.GetPostBackClientEvent(this, string.Format("' + AIE_getObject('{0}L').checked + ';' + AIE_getObject('{0}R').checked + ';' + AIE_getObject('{0}A').checked + ';' + AIE_getObject('{0}V').value + '", this.ClientID)).Replace("\\", ""), this.ClientID, this.ClientID.Replace(":", "_"), ParentImageEditor.TdCssClass);
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

            bool isLeft, isRight, isDegrees;

            bool.TryParse(args[0], out isLeft);
            bool.TryParse(args[1], out isRight);
            bool.TryParse(args[2], out isDegrees);

            if (isLeft)
            {
                job.RotateLeft();
            }
            else if (isRight)
            {
                job.RotateRight();
            }
            else if (isDegrees)
            {
                float degrees;
                float.TryParse(args[3], out degrees);
                job.Rotate(degrees);
            }

            ParentImageEditor.ImageHeight = job.Image.Height;
            ParentImageEditor.ImageWidth = job.Image.Width;

            PerformSave(job, tempFileName);

            OnClick(EventArgs.Empty);
        }
    }
}