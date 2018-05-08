using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class UploadLog
    {
        public UploadLog()
        {
            UploadID = -1;
            UserID = -1;
            CustomerID = -1;
            Path = string.Empty;
            FileName = string.Empty;
            UploadDate = DateTime.Now;
            PageSource = string.Empty;
        }

        [DataMember]
        public int UploadID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string Path { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public DateTime UploadDate { get; set; }
        [DataMember]
        public string PageSource { get; set; }
    }
}
