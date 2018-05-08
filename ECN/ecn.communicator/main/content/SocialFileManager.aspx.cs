using ECN_Framework.Common;

namespace ecn.communicator.main.content
{
    public partial class SocialFileManager : FileManagerBase
    {
        protected override void InitializeImagePath(string imagePath)
        {
            maingallery.imageDirectory = imagePath;
        }
    }
}