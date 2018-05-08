using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class SubsriptionManagementUDF
    {
        public SubsriptionManagementUDF()
        {
            SubscriptionManagementUDFID = -1;
            SubscriptionManagementGroupID = -1;
            GroupDataFieldsID = -1;
            StaticValue = string.Empty;
            IsDeleted = false;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
        }

        [DataMember]
        public int SubscriptionManagementUDFID { get; set; }
        [DataMember]
        public int SubscriptionManagementGroupID { get; set; }
        [DataMember]
        public int GroupDataFieldsID { get; set; }
        [DataMember]
        public string StaticValue { get; set; }
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