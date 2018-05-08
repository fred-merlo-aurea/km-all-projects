using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;


namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class SubscriptionManagement
    {
        public SubscriptionManagement()
        {
            SubscriptionManagementID = -1;
            BaseChannelID = -1;
            Name = string.Empty;
            Header = string.Empty;
            Footer = string.Empty;
            EmailHeader = string.Empty;
            EmailFooter = string.Empty;
            AdminEmail = string.Empty;
            MSMessage = string.Empty;
            IncludeMSGroups = false;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = false;
            UseReasonDropDown = false;
            ReasonVisible = false;
            UseThankYou = false;
            UseRedirect = false;
            ThankYouLabel = string.Empty;
            RedirectURL = string.Empty;
            RedirectDelay = 0;
        }


        [DataMember]
        public int SubscriptionManagementID { get; set; }
        [DataMember]
        public int BaseChannelID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Header { get; set; }
        [DataMember]
        public string Footer { get; set; }
        [DataMember]
        public string EmailHeader { get; set; }
        [DataMember]
        public string EmailFooter { get; set; }
        [DataMember]
        public string AdminEmail { get; set; }
        [DataMember]
        public string MSMessage { get; set; }
        [DataMember]
        public bool? IncludeMSGroups { get; set; }
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
        [DataMember]
        public bool? UseReasonDropDown { get; set; }
        [DataMember]
        public string ReasonLabel { get; set; }
        [DataMember]
        public string ThankYouLabel { get; set; }
        [DataMember]
        public bool? ReasonVisible { get; set; }
        [DataMember]
        public bool? UseThankYou { get; set; }
        [DataMember]
        public bool? UseRedirect { get; set; }

        [DataMember]
        public string RedirectURL { get; set; }
        [DataMember]
        public int RedirectDelay { get; set; }
    }
}
