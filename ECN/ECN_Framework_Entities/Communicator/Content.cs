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
    public class Content
    {
        public Content()
        {
            ContentID = -1;
            FolderID = null;
            LockedFlag = string.Empty;
            ContentSource = string.Empty;
            ContentText = string.Empty;
            ContentTypeCode = string.Empty;
            ContentCode = string.Empty;
            ContentTitle = string.Empty;
            CustomerID = null;
            ContentURL = string.Empty;
            ContentFilePointer = string.Empty;
            Sharing = string.Empty;
            MasterContentID = null;
            ContentMobile = string.Empty;
            ContentSMS = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            FilterList = new List<ContentFilter>();
            AliasList = new List<LinkAlias>();
            UseWYSIWYGeditor = null;
            DynamicTagList = new List<DynamicTag>();
            Archived = false;
            IsValidated = null;
        }

        #region Properties
        [DataMember]
        public int ContentID { get; set; }
        [DataMember]
        public int? FolderID { get; set; }
        [DataMember]
        public string LockedFlag { get; set; }
        [DataMember]
        public string ContentSource { get; set; }
        [DataMember]
        public string ContentText { get; set; }
        [DataMember]
        public string ContentTypeCode { get; set; }
        [DataMember]
        public string ContentCode { get; set; }
        [DataMember]
        public string ContentTitle { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public string ContentURL { get; set; }
        [DataMember]
        public string ContentFilePointer { get; set; }
        [DataMember]
        public string Sharing { get; set; }
        [DataMember]
        public int? MasterContentID { get; set; }
        [DataMember]
        public string ContentMobile { get; set; }
        [DataMember]
        public string ContentSMS { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public bool? UseWYSIWYGeditor { get; set; }

        [DataMember]
        public bool? Archived { get; set; }

        [DataMember]
        public bool? IsValidated { get; set; }
        
        //optional
        public List<ECN_Framework_Entities.Communicator.ContentFilter> FilterList { get; set; }
        public List<ECN_Framework_Entities.Communicator.LinkAlias> AliasList { get; set; }
        public List<ECN_Framework_Entities.Communicator.DynamicTag> DynamicTagList { get; set; }
        #endregion
    }
}
