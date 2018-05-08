using System;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls
{
    /// <summary>
    /// Represents a <see cref="ToolCrop"/> object.
    /// </summary>
    [ToolboxItem(false)]
    public class ToolCrop : ToolPixBase, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {
        public override string ImageName
        {
            get
            {
                return "crop_off.gif";
            }
        }

        public override string OverImageName
        {
            get
            {
                return "crop_over.gif";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolCrop"/> class.
        /// </summary>
        public ToolCrop() : base()
        {
            Initialize(string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolCrop"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public ToolCrop(string id) : base(id)
        {
            Initialize(id);
        }

        private void Initialize(string id)
        {
            if (id == string.Empty)
            {
                this.ID = "_toolCrop" + (ImageEditor.indexTools).ToString();
            }
            else
            {
                this.ID = id;
            }

            SetButtonImagesIfNotFx11(string.Empty, string.Empty);
            SetButtonImagesIfFx11(ImageName, OverImageName);

            this.ToolTip = "Crop";
            this.Click += new EventHandler(this.CropClicked);
        }

        private void CropClicked(object obj, EventArgs e)
        {
            var selection = ParentImageEditor.Selection;

            // Create the new name
            var tempFileName = ParentImageEditor.GetTempFileName();

            if ((selection.X1 > 0 && selection.Y1 > 0) || (selection.X2 > 0 && selection.Y2 > 0))
            {
                var job = new ImageJob(ParentImageEditor.WorkFile);

                job.Crop(ParentImageEditor.Selection.Valid());

                ParentImageEditor.ImageHeight = job.Image.Height;
                ParentImageEditor.ImageWidth = job.Image.Width;

                PerformSave(job, tempFileName);
            }
            else
            {
                Page.RegisterStartupScript(tempFileName, "<script language='javascript'>alert('Please make a selection.');</script>");
            }
        }
    }
}
