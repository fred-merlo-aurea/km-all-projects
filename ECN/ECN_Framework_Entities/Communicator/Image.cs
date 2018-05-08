using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_Entities.Communicator
{
    public class Image
    {

        public Image()
        {
            ImageName = string.Empty;
            FolderName = string.Empty;
            ImageURL = string.Empty;

            FolderID = -1;
            ImageID = 0;
            
            ImageData = null;

        }


        /// <summary>
        /// <code>FolderName</code> for an Image object, 
        /// </summary>
        public string FolderName { get; set; }

        /// <summary>
        /// <code>FolderID</code> for an Image object, 
        /// </summary>
        public int FolderID { get; set; }

        ///<summary>
        /// unique <code>ImageID</code> for an Image object,
        /// </summary>
        public int ImageID { get; set; }

        /// <summary>
        /// unique <code>ImageName</code> for an Image object, 
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// <code>ImageData</code> for an Image object, 
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// <code> ImageURL </code> for an Image object,
        /// <summary>
        public string ImageURL { get; set; }
    }
}
