using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The ClientSubscription object.
    /// </summary>
	[Serializable]
	[DataContract(Name = "ClientSubscriber")]
	public class ClientSubscription
	{
		#region Exposed Properties
        /// <summary>
        /// External key ID for the ClientSubscription object.
        /// </summary>
		[DataMember(Name = "ExternalKeyID")]
		public int Sequence { get; set; }
        /// <summary>
        /// First name of the ClientSubscription object.
        /// </summary>
		[DataMember(Name = "FirstName")]
		public string FName { get; set; }
        /// <summary>
        /// Last name of the ClientSubscription object.
        /// </summary>
		[DataMember(Name = "LastName")]
		public string LName { get; set; }
        /// <summary>
        /// Title for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Title { get; set; }
        /// <summary>
        /// Company for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Company { get; set; }
        /// <summary>
        /// Address for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Address { get; set; }
        /// <summary>
        /// Address2 for the ClientSubscription object.
        /// </summary>
		[DataMember(Name = "Address2")]
		public string MailStop { get; set; }
        /// <summary>
        /// City for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string City { get; set; }
        /// <summary>
        /// State for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string State { get; set; }
        /// <summary>
        /// Zip code for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Zip { get; set; }
        /// <summary>
        /// Plus4 for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Plus4 { get; set; }
        /// <summary>
        /// ForZip for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string ForZip { get; set; }
        /// <summary>
        /// County for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string County { get; set; }
        /// <summary>
        /// Country for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Country { get; set; }
        /// <summary>
        /// Phone number for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Phone { get; set; }
        /// <summary>
        /// Fax number for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Fax { get; set; }
        /// <summary>
        /// Income for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Income { get; set; }
        /// <summary>
        /// Gender for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Gender { get; set; }
        /// <summary>
        /// Address3 for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Address3 { get; set; }
        /// <summary>
        /// Mobile number for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Mobile { get; set; }
        /// <summary>
        /// Score for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public int Score { get; set; }
        /// <summary>
        /// Latitude for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public decimal Latitude { get; set; }
        /// <summary>
        /// Longitude for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public decimal Longitude { get; set; }
        /// <summary>
        /// Demo7 for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Demo7 { get; set; }
        /// <summary>
        /// IGrp_No for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public Guid IGrp_No { get; set; }
        /// <summary>
        /// Transaction date for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public DateTime? TransactionDate { get; set; }
        /// <summary>
        /// QDate for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public DateTime? QDate { get; set; }
        /// <summary>
        /// Email for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Email { get; set; }
        /// <summary>
        /// Subscription ID for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public int SubscriptionID { get; set; }
        /// <summary>
        /// If you can mail the ClientSubscription object.
        /// </summary>
		[DataMember]
		public bool IsMailable { get; set; }
        /// <summary>
        /// Age for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public int Age { get; set; }
        /// <summary>
        /// Birthdate for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public DateTime? Birthdate { get; set; }
        /// <summary>
        /// Occupation for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Occupation { get; set; }
        /// <summary>
        /// Phone Ext for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string PhoneExt { get; set; }
        /// <summary>
        /// Website for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string Website { get; set; }
        /// <summary>
        /// Full address for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string FullAddress { get; set; }
        /// <summary>
        /// Account number for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public string AccountNumber { get; set; }
        /// <summary>
        /// External key ID for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public int ExternalKeyId { get; set; }
        /// <summary>
        /// Email ID for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public int EmailID { get; set; }
        /// <summary>
        /// Mail permission for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public bool? MailPermission { get; set; } //Demo31
        /// <summary>
        /// Fax permission for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public bool? FaxPermission { get; set; } //Demo32
        /// <summary>
        /// Phone permission for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public bool? PhonePermission { get; set; } //Demo33
        /// <summary>
        /// Other products permission for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public bool? OtherProductsPermission { get; set; } //Demo34
        /// <summary>
        /// Third party permission for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public bool? ThirdPartyPermission { get; set; } //Demo35
        /// <summary>
        /// Email renewal permission for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public bool? EmailRenewPermission { get; set; } //Demo36
        /// <summary>
        /// Text permission for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public bool? TextPermission { get; set; }
        /// <summary>
        /// List of SubscriberConsensusDemographic objects for the ClientSubscription object.
        /// </summary>
		[DataMember(Name = "SubscriberConsensusDemographics")]
		public List<SubscriberConsensusDemographic> SubscriptionConsensusDemographics { get; set; }
        /// <summary>
        /// List of ClientProductSubscription objects for the ClientSubscription object.
        /// </summary>
		[DataMember]
		public List<ClientProductSubscription> ProductList { get; set; }
		#endregion
	}
}