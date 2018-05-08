using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class SubscriptionResponseMapHistory
    {
        public SubscriptionResponseMapHistory() { }
        #region Properties
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public int ResponseID  { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string ResponseName { get; set; }
        [DataMember]
        public string ResponseOther { get; set; }
        [DataMember]
        public bool IsActive  { get; set; }
        [DataMember]
        public DateTime? DateCreated  { get; set; }
        [DataMember]
        public DateTime? DateUpdated  { get; set; }
        [DataMember]
        public int CreatedByUserID  { get; set; }
        [DataMember]
        public int? UpdatedByUserID  { get; set; }
        #endregion
    }
}
