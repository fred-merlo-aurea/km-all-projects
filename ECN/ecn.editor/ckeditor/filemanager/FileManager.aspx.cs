using ECN_Framework.Common;

namespace ecn.editor.ckeditor
{
    public partial class filemanager : FileManagerBase
    {
        protected override void InitializeImagePath(string imagePath)
        {
            maingallery.imageDirectory = imagePath;
        }
    }
}
