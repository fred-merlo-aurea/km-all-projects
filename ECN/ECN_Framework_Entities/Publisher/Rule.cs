using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Publisher
{
    [Serializable]
    [DataContract]
    public class Rule
    {
        public Rule()
        {
            RuleID = -1;
            RuleName = string.Empty;
            PublicationID = -1;
            EditionID = -1;
            WhereClause = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
        }

        [DataMember]
        public int RuleID { get; set; }
        [DataMember]
        public string RuleName { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public int EditionID { get; set; }
        [DataMember]
        public string WhereClause { get; set; }
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
    }
}
