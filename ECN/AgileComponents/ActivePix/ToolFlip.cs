using System;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls
{
    /// <summary>
    /// Represents a <see cref="ToolFlip"/> object.
    /// </summary>
    [ToolboxItem(false)]
    public class ToolFlip : ToolPixBase, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {
        public override string ImageName
        {
            get
            {
                return "flip_off.gif";
            }
        }

        public override string OverImageName
        {
            get
            {
                return "flip_over.gif";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolFlip"/> class.
        /// </summary>
        public ToolFlip() : base()
        {
            Initialize(string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolFlip"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public ToolFlip(string id) : base(id)
        {
            Initialize(id);
        }

        private void Initialize(string id)
        {
            if (id == string.Empty)
            {
                this.ID = "_toolFlip" + (ImageEditor.indexTools).ToString();
            }
            else
            {
                this.ID = id;
            }

            SetButtonImagesIfNotFx11(string.Empty, string.Empty);
            SetButtonImagesIfFx11(ImageName, OverImageName);

            this.ToolTip = "Flip";
            this.Click += new EventHandler(this.FlipClicked);
        }

        private void FlipClicked(object obj, EventArgs e)
        {
            // Create the new name
            var tempFileName = ParentImageEditor.GetTempFileName();

            var job = new ImageJob(ParentImageEditor.WorkFile);
            job.Flip(FlipType.Vertical);

            ParentImageEditor.ImageHeight = job.Image.Height;
            ParentImageEditor.ImageWidth = job.Image.Width;

            PerformSave(job, tempFileName);
        }
    }
}
