using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Object
{
    [Serializable]
    [DataContract]
    public class DataCompareViewCost
    {
        public DataCompareViewCost(){}
        #region Properties
        [DataMember]
        public int DcRunId { get; set; }
        [DataMember]
        public decimal MatchCostTotal { get; set; }//Pick cost in this order:  User (if 1) then Client (if 1) then Base
        [DataMember]
        public decimal MatchBaseTotal { get; set; }
        [DataMember]
        public decimal MatchClientTotal { get; set; }
        [DataMember]
        public decimal MatchUserTotal { get; set; }
        [DataMember]
        public decimal MatchThirdPartyTotal { get; set; }
        [DataMember]
        public decimal LikeCostTotal { get; set; }//Pick cost in this order:  User (if 1) then Client (if 1) then Base
        [DataMember]
        public decimal LikeBaseTotal { get; set; }
        [DataMember]
        public decimal LikeClientTotal { get; set; }
        [DataMember]
        public decimal LikeUserTotal { get; set; }
        [DataMember]
        public decimal LikeThirdPartyTotal { get; set; }
        [DataMember]
        public int MatchRecordCount { get; set; }
        [DataMember]
        public int LikeRecordCount { get; set; }
        #endregion
    }
}
