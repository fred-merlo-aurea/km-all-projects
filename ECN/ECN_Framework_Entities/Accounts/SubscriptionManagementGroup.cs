using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class SubscriptionManagementGroup
    {
        public SubscriptionManagementGroup()
        {
            SubscriptionManagementGroupID = -1;
            SubscriptionManagementPageID = -1;
            GroupID = -1;
            Label = string.Empty;
            IsDeleted = false;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
        }

        [DataMember]
        public int SubscriptionManagementGroupID { get; set; }
        [DataMember]
        public int SubscriptionManagementPageID { get; set; }
        [DataMember]
        public int GroupID { get; set; }
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }

    }
}
