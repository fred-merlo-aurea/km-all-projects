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
    public class RuleCondition
    {
        public RuleCondition()
        {
            RuleConditionID = -1;
            RuleID = null;
            Field = null;
            DataType = null;
            Comparator = null;
            Value = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        #region Properties
        [DataMember]
        public int RuleConditionID { get; set; }
        [DataMember]
        public int? RuleID { get; set; }
        [DataMember]
        public string Field { get; set; }
        [DataMember]
        public string DataType { get; set; }
        [DataMember]
        public string Comparator { get; set; }
        [DataMember]
        public string Value { get; set; }
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
        #endregion
    }
}
