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
    public class GroupConfig
    {
        public GroupConfig()
        {
            GroupConfigID = -1;
            CustomerID = -1;
            ShortName = null;
            IsPublic = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int GroupConfigID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string IsPublic { get; set; }
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
      
    }
}
