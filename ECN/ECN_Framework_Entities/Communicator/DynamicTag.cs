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
    public class DynamicTag
    {

        public DynamicTag()
        {
            DynamicTagID = -1;
            Tag = null;
            ContentID = null;
            CustomerID = null;
            DynamicTagRulesList = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        #region Properties
        [DataMember]
        public int DynamicTagID { get; set; }
        [DataMember]
        public string Tag { get; set; }
        [DataMember]
        public int? ContentID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
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
        public List<DynamicTagRule> DynamicTagRulesList { get; set; }
        #endregion
    }
}