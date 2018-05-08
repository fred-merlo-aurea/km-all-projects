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
    public class DynamicTagRule
    {
        public DynamicTagRule()
        {
            DynamicTagRuleID = -1;
            RuleID = null;
            DynamicTagID = null;
            ContentID = null;
            Priority = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            Rule = null;
        }

        #region Properties
        [DataMember]
        public int DynamicTagRuleID { get; set; }
        [DataMember]
        public int? RuleID { get; set; }
        [DataMember]
        public int? DynamicTagID { get; set; }
        [DataMember]
        public int? ContentID { get; set; }
        [DataMember]
        public int? Priority { get; set; }
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

        public ECN_Framework_Entities.Communicator.Rule Rule { get; set; }
        #endregion
    }
}
