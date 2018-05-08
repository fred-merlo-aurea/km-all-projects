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
    public class LinkAlias
    {
        public LinkAlias()
        {
            AliasID = -1;
            ContentID = null;
            Link = string.Empty;
            Alias = string.Empty;
            LinkOwnerID = null;
            LinkTypeID = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            linkOwner = null;
            CustomerID = null;
        }

        [DataMember]
        public int AliasID { get; set; }
        [DataMember]
        public int? ContentID { get; set; }
        [DataMember]
        public string Link { get; set; }
        [DataMember]
        public string Alias { get; set; }
        [DataMember]
        public int? LinkOwnerID { get; set; }
        [DataMember]
        public int? LinkTypeID { get; set; }
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
        //optional
        public ECN_Framework_Entities.Communicator.LinkOwnerIndex linkOwner { get; set; }
    }
}
