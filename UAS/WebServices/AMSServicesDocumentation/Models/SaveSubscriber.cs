using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    [Serializable]
    [DataContract]
    public class SaveSubscriber
    {
        /// <summary>
        /// The SaveSubscriber object.
        /// </summary>
        public SaveSubscriber() 
        {
            Products = new List<ProductSubscription>();
            IsActive = true;
            ConsensusDemographics = new List<SubscriberConsensusDemographic>();
        }
        
        #region Exposed Properties
        /// <summary>
        /// Sequence ID for the SaveSubscriber object.
        /// </summary>
        [DataMember(Name = "ExternalKeyID")]
        public int Sequence { get; set; }
        /// <summary>
        /// First name of the SaveSubscriber object.
        /// </summary>
        [DataMember(Name = "FirstName")]
        public string FName { get; set; }
        /// <summary>
        /// Last name of the SaveSubscriber object.
        /// </summary>
        [DataMember(Name = "LastName")]
        public string LName { get; set; }
        /// <summary>
        /// Job title for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// Company name of the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string Company { get; set; }
        /// <summary>
        /// Address for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string Address { get; set; }
        /// <summary>
        /// Address2 for the SaveSubscriber object.
        /// </summary>
        [DataMember(Name = "Address2")]
        public string MailStop { get; set; }
        /// <summary>
        /// City for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string City { get; set; }
        /// <summary>
        /// State for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string State { get; set; }
        /// <summary>
        /// Zip code for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string Zip { get; set; }
        /// <summary>
        /// Plus4 for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string Plus4 { get; set; }
        /// <summary>
        /// ForZip for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string ForZip { get; set; }
        /// <summary>
        /// County for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string County { get; set; }
        /// <summary>
        /// Country for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string Country { get; set; }
        /// <summary>
        /// Phone number for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string Phone { get; set; }
        /// <summary>
        /// Fax number for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string Fax { get; set; }
        /// <summary>
        /// Demo31 for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public bool Demo31 { get; set; }
        /// <summary>
        /// Demo32 for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public bool Demo32 { get; set; }
        /// <summary>
        /// Demo33 for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public bool Demo33 { get; set; }
        /// <summary>
        /// Demo34 for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public bool Demo34 { get; set; }
        /// <summary>
        /// Demo35 for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public bool Demo35 { get; set; }
        /// <summary>
        /// Demo36 for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public bool Demo36 { get; set; }
        /// <summary>
        /// Gender for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string Gender { get; set; }
        /// <summary>
        /// Address3 for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string Address3 { get; set; }
        /// <summary>
        /// Home work address for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string Home_Work_Address { get; set; }
        /// <summary>
        /// Mobile number for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }
        /// <summary>
        /// Score for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public int Score { get; set; }
        /// <summary>
        /// List of ProductSubscription objects for the SaveSubscriber object.
        /// </summary>
        [DataMember(Name = "SubscriberProduct")]
        public List<ProductSubscription> Products { get; set; }
        /// <summary>
        /// List of SubscriberConsensusDemographic objects for the SaveSubscriber object.
        /// </summary>
        [DataMember]
        public List<SubscriberConsensusDemographic> ConsensusDemographics { get; set; }
        /// <summary>
        /// Subscription ID for the SaveSubscriber object.
        /// </summary>
        [IgnoreDataMember]
        public int SubscriptionID { get; set; }
        /// <summary>
        /// If the SaveSubscriber object is active.
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }
        #endregion
    }
}