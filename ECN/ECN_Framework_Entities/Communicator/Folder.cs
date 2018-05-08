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
    public class Folder
    {
        public Folder()
        {
            FolderID = -1;
            CustomerID = null;
            FolderName = string.Empty;
            FolderDescription = string.Empty;
            FolderType = string.Empty;
            IsSystem = null;
            ParentID = 0;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int FolderID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public string FolderName { get; set; }
        [DataMember]
        public string FolderDescription { get; set; }
        [DataMember]
        public string FolderType { get; set; }
        [DataMember]
        public bool? IsSystem { get; set; }
        [DataMember]
        public int ParentID { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
    }
}
