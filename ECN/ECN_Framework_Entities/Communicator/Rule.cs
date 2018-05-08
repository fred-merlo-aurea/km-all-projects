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
    public class Rule
    {

        public Rule()
        {
            RuleID = -1;
            RuleName = null;
            ConditionConnector = null;
            WhereClause = null;
            CustomerID = null;
            RuleConditionsList = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        #region Properties
        [DataMember]
        public int RuleID { get; set; }
        [DataMember]
        public string RuleName { get; set; }
        [DataMember]
        public string ConditionConnector { get; set; }
        [DataMember]
        public string WhereClause { get; set; }
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

        public List<RuleCondition> RuleConditionsList { get; set; }
        #endregion
    }
}
