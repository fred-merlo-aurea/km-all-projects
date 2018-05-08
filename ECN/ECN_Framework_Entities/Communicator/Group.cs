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
    public class Group
    {
        public Group()
        {
            GroupID = -1;
            CustomerID = -1;
            FolderID = null;
            GroupName = string.Empty;
            GroupDescription = string.Empty;
            OwnerTypeCode = string.Empty;
            MasterSupression = null;
            PublicFolder = null;
            OptinHTML = string.Empty;
            OptinFields = string.Empty;
            AllowUDFHistory = string.Empty;
            IsSeedList = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            Archived = false;
            //IsDeleted = null;
            FolderName = "";
        }

        [DataMember]
        public int GroupID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public int? FolderID { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public string GroupDescription { get; set; }
        [DataMember]
        public string OwnerTypeCode { get; set; }
        [DataMember]
        public int? MasterSupression { get; set; }
        [DataMember]
        public int? PublicFolder { get; set; }
        [DataMember]
        public string OptinHTML { get; set; }
        [DataMember]
        public string OptinFields { get; set; }
        [DataMember]
        public string AllowUDFHistory { get; set; }
        [DataMember]
        public bool? IsSeedList { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool? Archived { get; set; }

        [DataMember]
        public string FolderName { get; set; }
        //[DataMember]
        //public bool? IsDeleted { get; set; }
    }
}
