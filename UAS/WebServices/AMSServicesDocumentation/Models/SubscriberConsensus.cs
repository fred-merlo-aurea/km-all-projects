using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The SubscriberConsensus object.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SubscriberConsensus
    {
        /// <summary>
        /// External key ID for the SubscriberConsensus object.
        /// </summary>
        #region Properties
        [DataMember(Name = "ExternalKeyID")]
        public int Sequence { get; set; }
        /// <summary>
        /// First name of the SubscriberConsensus object.
        /// </summary>
        [DataMember(Name = "FirstName")]
        public string FName { get; set; }
        /// <summary>
        /// Last name of the SubscriberConsensus object.
        /// </summary>
        [DataMember(Name = "LastName")]
        public string LName { get; set; }
        /// <summary>
        /// Title for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// Company for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string Company { get; set; }
        /// <summary>
        /// Address for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string Address { get; set; }
        /// <summary>
        /// Address2 for the SubscriberConsensus object.
        /// </summary>
        [DataMember(Name = "Address2")]
        public string MailStop { get; set; }
        /// <summary>
        /// City for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string City { get; set; }
        /// <summary>
        /// State for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string State { get; set; }
        /// <summary>
        /// Zip code for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string Zip { get; set; }
        /// <summary>
        /// Plus4 for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string Plus4 { get; set; }
        /// <summary>
        /// ForZip for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string ForZip { get; set; }
        /// <summary>
        /// County for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string County { get; set; }
        /// <summary>
        /// Country for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string Country { get; set; }
        /// <summary>
        /// Phone for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string Phone { get; set; }
        /// <summary>
        /// Fax for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string Fax { get; set; }
        /// <summary>
        /// Mail permission for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public bool MailPermission { get; set; }//rename to MailPermission Demo31
        /// <summary>
        /// Fax Permission for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public bool FaxPermission { get; set; }//rename to FaxPermission Demo32
        /// <summary>
        /// Phone permission for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public bool PhonePermission { get; set; }//rename to PhonePermission Demo33
        /// <summary>
        /// Other product permission for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public bool OtherProductsPermission { get; set; }//rename to OtherProductsPermission Demo34
        /// <summary>
        /// Third party permission for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public bool ThirdPartyPermission { get; set; }//rename to ThirdPartyPermission Demo35
        /// <summary>
        /// Email renewal permission for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public bool EmailRenewPermission { get; set; }//rename to EmailRenewPermission Demo36
        /// <summary>
        /// Gender for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string Gender { get; set; }
        /// <summary>
        /// Address3 for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string Address3 { get; set; }
        /// <summary>
        /// Home work Address for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string Home_Work_Address { get; set; }
        /// <summary>
        /// Mobile number for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }
        /// <summary>
        /// Score for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public int Score { get; set; }
        /// <summary>
        /// Latitude for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public decimal Latitude { get; set; }
        /// <summary>
        /// Longitude for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public decimal Longitude { get; set; }
        /// <summary>
        /// Demo7 for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string Demo7 { get; set; }
        /// <summary>
        /// IGrp_No for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public Guid IGrp_No { get; set; }
        /// <summary>
        /// Par3c for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string Par3C { get; set; }
        /// <summary>
        /// Transaction date for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public DateTime? TransactionDate { get; set; }
        /// <summary>
        /// QDate for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public DateTime? QDate { get; set; }
        /// <summary>
        /// Email for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public string Email { get; set; }
        /// <summary>
        /// Subscription ID for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public int SubscriptionID { get; set; }
        /// <summary>
        /// If the SubscriberConsensus object is active.
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }
        /// <summary>
        /// List of SubscriberConsensusDemographic objects for the SubscriberConsensus object.
        /// </summary>
        [DataMember]
        public List<SubscriberConsensusDemographic> Demographics { get; set; }
        #endregion
    }
}