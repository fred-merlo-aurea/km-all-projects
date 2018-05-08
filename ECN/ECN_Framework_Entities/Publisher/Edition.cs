using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Publisher
{
    [Serializable]
    [DataContract]
    public class Edition
    {
        public Edition()
        {
            EditionID = -1;
            EditionName = string.Empty;
            PublicationID = -1;
            EnableDate = null;
            DisableDate = null;
            Theme = string.Empty;
            Background = string.Empty;
            FileName = string.Empty;
            Pages = -1;
            ThumbnailPage = string.Empty;
            StatusID = ECN_Framework_Common.Objects.Publisher.Enums.Status.Active;
            Status = string.Empty;
            xmlTOC = string.Empty;
            IsSearchable = null;
            IsLoginRequired = null;
            OriginalEditionID = null;
            Version = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
            PageList = null;
        }

        [DataMember]
        public int EditionID { get; set; }
        [DataMember]
        public string EditionName { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public DateTime? EnableDate { get; set; }
        [DataMember]
        public DateTime? DisableDate { get; set; }
        [DataMember]
        public string Theme { get; set; }
        [DataMember]
        public string Background { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public int Pages { get; set; }
        [DataMember]
        public string ThumbnailPage { get; set; }
        [DataMember]
        public ECN_Framework_Common.Objects.Publisher.Enums.Status StatusID { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string xmlTOC { get; set; }
        [DataMember]
        public bool? IsSearchable { get; set; }
        [DataMember]
        public bool? IsLoginRequired { get; set; }
        [DataMember]
        public int? OriginalEditionID { get; set; }
        [DataMember]
        public int? Version { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        public List<Page> PageList { get; set; }
    }
}
