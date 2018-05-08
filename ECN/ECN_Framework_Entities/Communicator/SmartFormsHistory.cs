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
    public class SmartFormsHistory
    {
        public SmartFormsHistory()
        {
            SmartFormID = -1;
            GroupID = null;
            SubscriptionGroupIDs = string.Empty;
            SmartFormName = string.Empty;
            SmartFormHTML = string.Empty;
            SmartFormFields = string.Empty;
            Response_UserMsgSubject = string.Empty;
            Response_UserMsgBody = string.Empty;
            Response_UserScreen = string.Empty;
            Response_FromEmail = string.Empty;
            Response_AdminEmail = string.Empty;
            Response_AdminMsgSubject = string.Empty;
            Response_AdminMsgBody = string.Empty;
            Type = string.Empty;
            DoubleOptIn = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
        }

        [DataMember]
        public int SmartFormID { get; set; }
        [DataMember]
        public int? GroupID { get; set; }
        [DataMember]
        public string SubscriptionGroupIDs { get; set; }
        [DataMember]
        public string SmartFormName { get; set; }
        [DataMember]
        public string SmartFormHTML { get; set; }
        [DataMember]
        public string SmartFormFields { get; set; }
        [DataMember]
        public string Response_UserMsgSubject { get; set; }
        [DataMember]
        public string Response_UserMsgBody { get; set; }
        [DataMember]
        public string Response_UserScreen { get; set; }
        [DataMember]
        public string Response_FromEmail { get; set; }
        [DataMember]
        public string Response_AdminEmail { get; set; }
        [DataMember]
        public string Response_AdminMsgSubject { get; set; }
        [DataMember]
        public string Response_AdminMsgBody { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public bool? DoubleOptIn { get; set; }
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
