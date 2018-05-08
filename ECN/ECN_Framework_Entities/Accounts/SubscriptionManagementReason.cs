using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class SubscriptionManagementReason
    {
        public SubscriptionManagementReason() {
            SubscriptionManagementReasonID = -1;
            SubscriptionManagementID = -1;
            Reason = string.Empty;
            IsDeleted = false;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedDate = null;
            UpdatedUserID = null;
            SortOrder = null;
        }

        [DataMember]
        public int SubscriptionManagementReasonID { get; set; }
        [DataMember]
        public int SubscriptionManagementID { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public int? SortOrder { get; set; }
        
    }
}
