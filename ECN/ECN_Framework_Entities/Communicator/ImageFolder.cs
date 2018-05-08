using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_Entities.Communicator
{
    public class ImageFolder
    {
        /// <summary>
        /// Unique identifier for an image folder.  Note: this is currently always 0 (zero).
        /// </summary>
        public int FolderID { get; set; }

        /// <summary>
        /// Name of the image folder, not including any path information.
        /// </summary>
        public string FolderName { get; set; }

        /// <summary>
        /// Name of the image folder including the full path.  Full path is relative to the root folder for customer images; 
        /// ignored when creating new folders.
        /// </summary>
        public string FolderFullName { get; set; }

        /// <summary>
        /// A URI suitable for creating a link to the image folder.
        /// </summary>
        public string FolderUrl { get; set; }
    }
}
