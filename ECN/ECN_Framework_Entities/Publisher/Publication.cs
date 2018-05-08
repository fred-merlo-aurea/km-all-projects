using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Publisher
{
    [Serializable]
    [DataContract]
    public class Publication
    {
        public Publication()
        {
            PublicationID = -1;
            PublicationName = string.Empty;
            PublicationType = string.Empty;
            PublicationCode = string.Empty;
            CustomerID = -1;
            GroupID = null;
            Active = true;
            CreatedDate = null;
            UpdatedDate = null;
            ContactAddress1 = string.Empty;
            ContactAddress2 = string.Empty;
            ContactEmail = string.Empty;
            ContactPhone = string.Empty;
            EnableSubscription = null;
            LogoLink = string.Empty;
            LogoURL = string.Empty;
            SubscriptionOption = null;
            CreatedUserID = null;
            ContactFormLink = string.Empty;
            SubscriptionFormLink = string.Empty;
            CategoryID = null;
            Circulation = null;
            FrequencyID = null;
            UpdatedUserID = null;
            IsDeleted = null;
        }

        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public string PublicationName { get; set; }
        [DataMember]
        public string PublicationType { get; set; }
        [DataMember]
        public string PublicationCode { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public int? GroupID { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public string ContactAddress1 { get; set; }
        [DataMember]
        public string ContactAddress2 { get; set; }
        [DataMember]
        public string ContactEmail { get; set; }
        [DataMember]
        public string ContactPhone { get; set; }
        [DataMember]
        public bool? EnableSubscription { get; set; }
        [DataMember]
        public string LogoLink { get; set; }
        [DataMember]
        public string LogoURL { get; set; }
        [DataMember]
        public int? SubscriptionOption { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public string ContactFormLink { get; set; }
        [DataMember]
        public string SubscriptionFormLink { get; set; }
        [DataMember]
        public int? CategoryID { get; set; }
        [DataMember]
        public int? Circulation { get; set; }
        [DataMember]
        public int? FrequencyID { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
    }
}
