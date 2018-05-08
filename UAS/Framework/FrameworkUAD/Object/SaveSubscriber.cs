using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class SaveSubscriber
    {
        public SaveSubscriber() 
        {
            Products = new List<ProductSubscription>();
            IsActive = true;
            ConsensusDemographics = new List<SubscriberConsensusDemographic>();
        }

        #region Exposed Properties
        [DataMember(Name = "ExternalKeyID")]
        public int Sequence { get; set; }
        [DataMember(Name = "FirstName")]
        public string FName { get; set; }
        [DataMember(Name = "LastName")]
        public string LName { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember(Name = "Address2")]
        public string MailStop { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string Plus4 { get; set; }
        [DataMember]
        public string ForZip { get; set; }
        [DataMember]
        public string County { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public bool Demo31 { get; set; }
        [DataMember]
        public bool Demo32 { get; set; }
        [DataMember]
        public bool Demo33 { get; set; }
        [DataMember]
        public bool Demo34 { get; set; }
        [DataMember]
        public bool Demo35 { get; set; }
        [DataMember]
        public bool Demo36 { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string Address3 { get; set; }
        [DataMember]
        public string Home_Work_Address { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public int Score { get; set; }

        [DataMember(Name = "SubscriberProduct")]
        public List<ProductSubscription> Products { get; set; }

        [DataMember]
        public List<SubscriberConsensusDemographic> ConsensusDemographics { get; set; }


        [IgnoreDataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        #endregion
    }
}
