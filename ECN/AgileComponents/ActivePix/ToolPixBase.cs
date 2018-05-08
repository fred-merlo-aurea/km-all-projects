#if !FX1_1
#define NOT_FX1_1
#endif

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.UI;

namespace ActiveUp.WebControls
{
    [ToolboxItem(false)]
    public abstract class ToolPixBase : ToolButton, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {
        public abstract string ImageName { get; }
        public abstract string OverImageName { get; }

        /// <summary>
        /// Parent Toolbar object that contains this button
        /// </summary>
        public Toolbar ParentToolbar
        {
            get
            {
                return this.Parent as Toolbar;
            }
        }

        /// <summary>
        /// Parent ImageEditor object that contains this button's Toolbar and this button. 
        /// </summary>
        /// <seealso cref="ParentToolbar" />
        public ImageEditor ParentImageEditor
        {
            get
            {
                return this.ParentToolbar.Parent as ImageEditor;
            }
        }

        public ToolPixBase() : base()
        { }

        public ToolPixBase(string id) : base(id)
        { }

        /// <summary>
        /// Notifies the tool to perform any necessary prerendering steps prior to saving view state and rendering content.
        /// </summary>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            OnPreRenderIfNotFx11();
            base.OnPreRender(e);
        }

        /// <summary>
        /// Saves image after edit works on image is done
        /// </summary>
        /// <param name="job"></param>
        protected virtual void PerformSave(ImageJob job, string tempFileName)
        {
            if (ParentImageEditor.DirectWrite)
            {
                job.Save();
            }
            else
            {
                job.Save(Page.Server.MapPath(ParentImageEditor.TempDirectory) + tempFileName, FileCompression.CCITT4, 100, FileFormat.Jpeg);
                job.Dispose();

                // Delete old temp file
                System.IO.File.Delete(Page.Server.MapPath(ParentImageEditor.TempURL));

                ParentImageEditor.TempURL = ParentImageEditor.TempDirectory + tempFileName;
            }
        }

        [Conditional("NOT_FX1_1")]
        private void OnPreRenderIfNotFx11()
        {
            if (string.IsNullOrEmpty(ImageURL))
            {
                var imageUrlResourceName = string.Format("ActiveUp.WebControls._resources.Images.{0}", ImageName);
                ImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), imageUrlResourceName);
            }
            if (string.IsNullOrEmpty(OverImageURL))
            {
                var overImageUrlResourceName = string.Format("ActiveUp.WebControls._resources.Images.{0}", OverImageName);
                OverImageURL = Page.ClientScript.GetWebResourceUrl(this.GetType(), overImageUrlResourceName);
            }
        }

        [Conditional("FX1_1")]
        protected void SetButtonImagesIfFx11(string imageUrl, string overImageUrl)
        {
            this.ImageURL = imageUrl;
            this.OverImageURL = overImageUrl;
        }

        [Conditional("NOT_FX1_1")]
        protected void SetButtonImagesIfNotFx11(string imageUrl, string overImageUrl)
        {
            this.ImageURL = imageUrl;
            this.OverImageURL = overImageUrl;
        }
    }
}
