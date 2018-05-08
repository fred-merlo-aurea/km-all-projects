using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The ProductSubscription object.
    /// </summary>
    [Serializable]
    [DataContract]
    public class ProductSubscription
    {
        #region DataMember Properties
        /// <summary>
        /// Pub ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int PubID { get; set; }
        /// <summary>
        /// PubSubscription ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int PubSubscriptionID { get; set; }
        /// <summary>
        /// SubscriptionStatus ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int SubscriptionStatusID { get; set; }
        /// <summary>
        /// Demo7 for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Demo7 { get; set; }
        /// <summary>
        /// QDate for the ProductSubscription object.
        /// </summary>
        [DataMember(Name = "QDate")]
        public DateTime? QualificationDate { get; set; }
        /// <summary>
        /// QSource ID for the ProductSubscription object.
        /// </summary>
        [DataMember(Name = "QSourceID")]
        public int PubQSourceID { get; set; }
        /// <summary>
        /// Category ID for the ProductSubscription object.
        /// </summary>
        [DataMember(Name = "CategoryID")]
        public int PubCategoryID { get; set; }
        /// <summary>
        /// Transaction ID for the ProductSubscription object.
        /// </summary>
        [DataMember(Name = "TransactionID")]
        public int PubTransactionID { get; set; }
        /// <summary>
        /// Email for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Email { get; set; }
        /// <summary>
        /// Status update date for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public DateTime StatusUpdatedDate { get; set; }
        /// <summary>
        /// Status updated reason for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string StatusUpdatedReason { get; set; }
        /// <summary>
        /// Date created for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Date updated for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        /// <summary>
        /// Email status for the ProductSubscription object.
        /// </summary>
        [DataMember(Name = "EmailStatus")]
        public string Status { get; set; }
        /// <summary>
        /// Copies for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int Copies { get; set; }
        /// <summary>
        /// Grace issues for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int GraceIssues { get; set; }
        /// <summary>
        /// OnBehalfOf for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string OnBehalfOf { get; set; }
        /// <summary>
        /// Subscriber source code for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string SubscriberSourceCode { get; set; }
        /// <summary>
        /// OrigsSrc for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string OrigsSrc { get; set; }
        /// <summary>
        /// Sequence ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int SequenceID { get; set; }
        /// <summary>
        /// Account number for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string AccountNumber { get; set; }
        /// <summary>
        /// Verify for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Verify { get; set; }
        /// <summary>
        /// External key ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int ExternalKeyID { get; set; }
        /// <summary>
        /// First name of the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }
        /// <summary>
        /// Last name of the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string LastName { get; set; }
        /// <summary>
        /// Company for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Company { get; set; }
        /// <summary>
        /// Title for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// Occupation for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Occupation { get; set; }
        /// <summary>
        /// Address1 for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Address1 { get; set; }
        /// <summary>
        /// Address2 for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Address2 { get; set; }
        /// <summary>
        /// Address3 for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Address3 { get; set; }
        /// <summary>
        /// City for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string City { get; set; }
        /// <summary>
        /// State for the ProductSubscription object.
        /// </summary>
        [DataMember(Name = "State")]
        public string RegionCode { get; set; }
        /// <summary>
        /// Zip code for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string ZipCode { get; set; }
        /// <summary>
        /// Plus4 for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Plus4 { get; set; }
        /// <summary>
        /// Carrier route for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string CarrierRoute { get; set; }
        /// <summary>
        /// County for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string County { get; set; }
        /// <summary>
        /// Country for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Country { get; set; }
        /// <summary>
        /// Latitude for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public decimal Latitude { get; set; }
        /// <summary>
        /// Longitude for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public decimal Longitude { get; set; }
        /// <summary>
        /// Phone number for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Phone { get; set; }
        /// <summary>
        /// Fax number for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Fax { get; set; }
        /// <summary>
        /// Mobile number for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }
        /// <summary>
        /// Website for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Website { get; set; }
        /// <summary>
        /// Birthdate for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public DateTime Birthdate { get; set; }
        /// <summary>
        /// Age for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int Age { get; set; }
        /// <summary>
        /// Income for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Income { get; set; }
        /// <summary>
        /// Gender for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string Gender { get; set; }
        /// <summary>
        /// Phone Ext for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string PhoneExt { get; set; }
        /// <summary>
        /// IGrp_No for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public Guid? IGrp_No { get; set; }
        /// <summary>
        /// ReqFlag for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int ReqFlag { get; set; }
        /// <summary>
        /// Pub code for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string PubCode { get; set; }
        /// <summary>
        /// Name of the ProductSubscription object.
        /// </summary>
        [DataMember(Name = "Name")]
        public string PubName { get; set; }
        /// <summary>
        /// Pub type for the ProductSubscription object.
        /// </summary>
        [DataMember(Name = "PubType")]
        public string PubTypeDisplayName { get; set; }
        /// <summary>
        /// Client name of the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string ClientName { get; set; }
        /// <summary>
        /// Full name of the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string FullName { get; set; }
        /// <summary>
        /// Full address for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string FullAddress { get; set; }
        /// <summary>
        /// Mail ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int EmailID { get; set; }
        /// <summary>
        /// Mail permission for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public bool? MailPermission { get; set; } //Demo31
        /// <summary>
        /// Fax permission for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public bool? FaxPermission { get; set; } //Demo32
        /// <summary>
        /// Phone permission for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public bool? PhonePermission { get; set; } //Demo33
        /// <summary>
        /// Other product permission for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public bool? OtherProductsPermission { get; set; } //Demo34
        /// <summary>
        /// Third party permission for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public bool? ThirdPartyPermission { get; set; } //Demo35
        /// <summary>
        /// Email renewal permission for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public bool? EmailRenewPermission { get; set; } //Demo36
        /// <summary>
        /// Text permission for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public bool? TextPermission { get; set; }
        /// <summary>
        /// List of SubscriberProductDemographic objects for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public List<AMSServicesDocumentation.Models.SubscriberProductDemographic> SubscriberProductDemographics { get; set; }
        #endregion
        
        #region Ignore Properties for ClientServices
        /// <summary>
        /// Address type ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int AddressTypeID { get; set; }
        /// <summary>
        /// Address validation date for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public DateTime? AddressValidationDate { get; set; }
        /// <summary>
        /// Address validation source for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string AddressValidationSource { get; set; }
        /// <summary>
        /// Address validation message for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string AddressValidationMessage { get; set; }
        /// <summary>
        /// Address type code ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int AddressTypeCodeId { get; set; }
        /// <summary>
        /// Address last updated date for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public DateTime? AddressLastUpdatedDate { get; set; }
        /// <summary>
        /// Address updated source type code ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int AddressUpdatedSourceTypeCodeId { get; set; }
        /// <summary>
        /// Add remove ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int AddRemoveID { get; set; }
        /// <summary>
        /// Country ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int CountryID { get; set; }
        /// <summary>
        /// Created by user ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int CreatedByUserID { get; set; }
        /// <summary>
        /// Email status ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int EmailStatusID { get; set; }
        /// <summary>
        /// FullZip for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string FullZip { get; set; }
        /// <summary>
        /// IMBSeq for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string IMBSeq { get; set; }
        /// <summary>
        /// If the ProductSubscription object is active.
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }
        /// <summary>
        /// If the address is validated for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public bool IsAddressValidated { get; set; }
        /// <summary>
        /// If the ProductSubscription object is comp.
        /// </summary>
        [DataMember]
        public bool IsComp { get; set; }
        /// <summary>
        /// If the ProductSubscription object is locked.
        /// </summary>
        [DataMember]
        public bool IsLocked { get; set; }
        /// <summary>
        /// If the ProductSubscription object is active for wave mailing.
        /// </summary>
        [DataMember]
        public bool IsInActiveWaveMailing { get; set; }
        /// <summary>
        /// If the ProductSubscription object is a new subscription.
        /// </summary>
        [DataMember]
        public bool IsNewSubscription { get; set; }
        /// <summary>
        /// If the ProductSubscription object is paid.
        /// </summary>
        [DataMember]
        public bool IsPaid { get; set; }
        /// <summary>
        /// If the ProductSubscription object is subscribed.
        /// </summary>
        [DataMember]
        public bool IsSubscribed { get; set; }
        /// <summary>
        /// The user ID for the person that locked the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int LockedByUserID { get; set; }
        /// <summary>
        /// The date that the ProductSubscription object was locked.
        /// </summary>
        [DataMember]
        public DateTime? LockDate { get; set; }
        /// <summary>
        /// Date when the ProductSubscription object had the lock released.
        /// </summary>
        [DataMember]
        public DateTime? LockDateRelease { get; set; }
        /// <summary>
        /// List of MarketingMap objects for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public List<MarketingMap> MarketingMapList { get; set; }
        /// <summary>
        /// Member group for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string MemberGroup { get; set; }
        /// <summary>
        /// Par3CID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int Par3CID { get; set; }
        /// <summary>
        /// PhoneCode for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int PhoneCode { get; set; }
        /// <summary>
        /// List of ProductSubscriptionDetail objects for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public List<AMSServicesDocumentation.Models.ProductSubscriptionDetail> ProductMapList { get; set; }
        /// <summary>
        /// List of Prospect objects for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public List<Prospect> ProspectList { get; set; }
        /// <summary>
        /// Region ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int RegionID { get; set; }
        /// <summary>
        /// SFRecordIdentifier for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public Guid? SFRecordIdentifier { get; set; }
        /// <summary>
        /// SubScr ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int SubSrcID { get; set; }
        /// <summary>
        /// tmpSubscription ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int tmpSubscriptionID { get; set; }
        /// <summary>
        /// User ID for the user that updated the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        /// <summary>
        /// WaveMailing ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int WaveMailingID { get; set; }
        /// <summary>
        /// SubGenSubscriber ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int SubGenSubscriberID { get; set; }
        /// <summary>
        /// SubGenSubscription ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int SubGenSubscriptionID { get; set; }
        /// <summary>
        /// SubGenPublication ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int SubGenPublicationID { get; set; }
        /// <summary>
        /// SubGenMailingAddress ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int SubGenMailingAddressId { get; set; }
        /// <summary>
        /// SubGenBillingAddress ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int SubGenBillingAddressId { get; set; }
        /// <summary>
        /// Issues left for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int IssuesLeft { get; set; }
        /// <summary>
        /// Unearned revenue for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public decimal UnearnedReveue { get; set; }
        /// <summary>
        /// SubGenIsLead for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public bool SubGenIsLead { get; set; } 
        /// <summary>
        /// SubGenRenewalCode for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string SubGenRenewalCode { get; set; } 
        /// <summary>
        /// Date of SubGenSubscriptionRenewal for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public DateTime? SubGenSubscriptionRenewDate { get; set; } 
        /// <summary>
        /// Date that the SubGenSubscription expires for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public DateTime? SubGenSubscriptionExpireDate { get; set; } 
        /// <summary>
        /// SubGenSubscriptionLastQualifiedDate for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public DateTime? SubGenSubscriptionLastQualifiedDate { get; set; }
        /// <summary>
        /// List of PubSubscriptionAdHoc objects for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public List<AMSServicesDocumentation.Models.PubSubscriptionAdHoc> AdHocFields { get; set; }
        /// <summary>
        /// PublicationToolTip for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public string PublicationToolTip { get; set; }
        /// <summary>
        /// Subscription ID for the ProductSubscription object.
        /// </summary>
        [DataMember]
        public int SubscriptionID { get; set; }
        #endregion
    }
}