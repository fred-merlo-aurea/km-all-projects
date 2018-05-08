using System;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls
{
    /// <summary>
    /// Represents a <see cref="ToolMirror"/> object.
    /// </summary>
    [ToolboxItem(false)]
    public class ToolMirror : ToolPixBase, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {
        public override string ImageName
        {
            get
            {
                return "mirror_off.gif";
            }
        }

        public override string OverImageName
        {
            get
            {
                return "mirror_over.gif";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolMirror"/> class.
        /// </summary>
        public ToolMirror() : base()
        {
            Initialize(string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolMirror"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public ToolMirror(string id) : base(id)
        {
            Initialize(id);
        }

        private void Initialize(string id)
        {
            if (id == string.Empty)
            {
                this.ID = "_toolMirror" + (ImageEditor.indexTools).ToString();
            }
            else
            {
                this.ID = id;
            }

            SetButtonImagesIfNotFx11(string.Empty, string.Empty);
            SetButtonImagesIfFx11(ImageName, OverImageName);

            this.ToolTip = "Mirror";
            this.Click += new EventHandler(this.MirrorClicked);
        }

        private void MirrorClicked(object obj, EventArgs e)
        {
            // Create the new name
            var tempFileName = ParentImageEditor.GetTempFileName();

            var job = new ImageJob(ParentImageEditor.WorkFile);
            job.Flip(FlipType.Horizontal);

            ParentImageEditor.ImageHeight = job.Image.Height;
            ParentImageEditor.ImageWidth = job.Image.Width;

            PerformSave(job, tempFileName);
        }
    }
}