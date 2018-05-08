using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The ClientProductSubscription object.
    /// </summary>
    [Serializable]
    [DataContract]
    public class ClientProductSubscription
    {
        #region DataMember Properties
        /// <summary>
        /// Pub ID for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public int PubID { get; set; }
        /// <summary>
        /// PubSubscription ID for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public int PubSubscriptionID { get; set; }
        /// <summary>
        /// Subscription status ID for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public int SubscriptionStatusID { get; set; }
        /// <summary>
        /// Demo7 for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Demo7 { get; set; }
        /// <summary>
        /// QDate for the ClientProductSubscription object.
        /// </summary>
        [DataMember(Name = "QDate")]
        public DateTime? QualificationDate { get; set; }
        /// <summary>
        /// QSource ID for the ClientProductSubscription object.
        /// </summary>
        [DataMember(Name = "QSourceID")]
        public int PubQSourceID { get; set; }
        /// <summary>
        /// Category ID for the ClientProductSubscription object.
        /// </summary>
        [DataMember(Name = "CategoryID")]
        public int PubCategoryID { get; set; }
        /// <summary>
        /// Transaction ID for the ClientProductSubscription object.
        /// </summary>
        [DataMember(Name = "TransactionID")]
        public int PubTransactionID { get; set; }
        /// <summary>
        /// Email for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Email { get; set; }
        /// <summary>
        /// Status updated date for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public DateTime StatusUpdatedDate { get; set; }
        /// <summary>
        /// Status updated reason for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string StatusUpdatedReason { get; set; }
        /// <summary>
        /// Date created for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Date updated for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        /// <summary>
        /// Email status for the ClientProductSubscription object.
        /// </summary>
        [DataMember(Name = "EmailStatus")]
        public string Status { get; set; }
        /// <summary>
        /// Copies for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public int Copies { get; set; }
        /// <summary>
        /// Grace issues for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public int GraceIssues { get; set; }
        /// <summary>
        /// OnBehalfOf for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string OnBehalfOf { get; set; }
        /// <summary>
        /// Subscriber source code for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string SubscriberSourceCode { get; set; }
        /// <summary>
        /// OrigsSrc for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string OrigsSrc { get; set; }
        /// <summary>
        /// Sequence ID for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public int SequenceID { get; set; }
        /// <summary>
        /// Account number for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string AccountNumber { get; set; }
        /// <summary>
        /// Verify for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Verify { get; set; }
        /// <summary>
        /// External key ID for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public int ExternalKeyID { get; set; }
        /// <summary>
        /// First name of the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }
        /// <summary>
        /// Last name of the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string LastName { get; set; }
        /// <summary>
        /// Company for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Company { get; set; }
        /// <summary>
        /// Title for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// Occupation for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Occupation { get; set; }
        /// <summary>
        /// Address1 for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Address1 { get; set; }
        /// <summary>
        /// Address2 for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Address2 { get; set; }
        /// <summary>
        /// Address3 for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Address3 { get; set; }
        /// <summary>
        /// City for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string City { get; set; }
        /// <summary>
        /// State for the ClientProductSubscription object.
        /// </summary>
        [DataMember(Name = "State")]
        public string RegionCode { get; set; }
        /// <summary>
        /// Zip code for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string ZipCode { get; set; }
        /// <summary>
        /// Plus4 for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Plus4 { get; set; }
        /// <summary>
        /// Carrier route for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string CarrierRoute { get; set; }
        /// <summary>
        /// County for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string County { get; set; }
        /// <summary>
        /// Country for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Country { get; set; }
        /// <summary>
        /// Latitude for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public decimal Latitude { get; set; }
        /// <summary>
        /// Longitude for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public decimal Longitude { get; set; }
        /// <summary>
        /// Phone for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Phone { get; set; }
        /// <summary>
        /// Fax for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Fax { get; set; }
        /// <summary>
        /// Mobile for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }
        /// <summary>
        /// Website for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Website { get; set; }
        /// <summary>
        /// Birthdate for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public DateTime Birthdate { get; set; }
        /// <summary>
        /// Age for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public int Age { get; set; }
        /// <summary>
        /// Income for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Income { get; set; }
        /// <summary>
        /// Gender for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string Gender { get; set; }
        /// <summary>
        /// Phone Ext for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string PhoneExt { get; set; }
        /// <summary>
        /// IGrp_No for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public Guid? IGrp_No { get; set; }
        /// <summary>
        /// ReqFlag for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public int ReqFlag { get; set; }
        /// <summary>
        /// Pub code for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string PubCode { get; set; }
        /// <summary>
        /// Name of the ClientProductSubscription object.
        /// </summary>
        [DataMember(Name = "Name")]
        public string PubName { get; set; }
        /// <summary>
        /// Pub type for the ClientProductSubscription object.
        /// </summary>
        [DataMember(Name = "PubType")]
        public string PubTypeDisplayName { get; set; }
        /// <summary>
        /// Client name of the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string ClientName { get; set; }
        /// <summary>
        /// Full name of the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string FullName { get; set; }
        /// <summary>
        /// Full address for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public string FullAddress { get; set; }
        /// <summary>
        /// Email ID for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public int EmailID { get; set; }
        /// <summary>
        /// Mail permission for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public bool? MailPermission { get; set; } //Demo31
        /// <summary>
        /// Fax permission for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public bool? FaxPermission { get; set; } //Demo32
        /// <summary>
        /// Phone permission for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public bool? PhonePermission { get; set; } //Demo33
        /// <summary>
        /// Other products permssion for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public bool? OtherProductsPermission { get; set; } //Demo34
        /// <summary>
        /// Third party permission for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public bool? ThirdPartyPermission { get; set; } //Demo35
        /// <summary>
        /// Email renewal permission for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public bool? EmailRenewPermission { get; set; } //Demo36
        /// <summary>
        /// Text permission for the ClientProductSubscription object.
        /// </summary>
        [DataMember]
        public bool? TextPermission { get; set; }
        /// <summary>
        /// List of SubscriberProductDemographic objects for the ClientProductSubscription object.
        /// </summary>
        [DataMember(Name = "SubscriberProductDemographics")]
        public List<SubscriberProductDemographic> SubscriberProductDemographics { get; set; }
        #endregion
    }
}