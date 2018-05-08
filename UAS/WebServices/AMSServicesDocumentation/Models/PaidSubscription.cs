using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The PaidSubscription object.
    /// </summary>
    [Serializable]
    [DataContract]
    public class PaidSubscription
    {
        #region Properties
        /// <summary>
        /// Subscription order date for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// If the PaidSubscription object is a gift.
        /// </summary>
        [DataMember]
        public bool IsGift { get; set; }
        /// <summary>
        /// PaidSubscriptionPayment for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public PaidSubscriptionPayment Payment { get; set; }
        #region Subscriber
        /// <summary>
        /// First name of the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string SubscriberFirstName { get; set; }
        /// <summary>
        /// Last name of the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string SubscriberLastName { get; set; }
        /// <summary>
        /// Email for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string SubscriberEmail { get; set; }
        /// <summary>
        /// Title for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string SubscriberTitle { get; set; }
        /// <summary>
        /// Company for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string SubscriberCompany { get; set; }
        /// <summary>
        /// Phone number for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string SubscriberPhone { get; set; }
        /// <summary>
        /// Mobile number for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string SubscriberMobile { get; set; }
        /// <summary>
        /// Fax number for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string SubscriberFax { get; set; }
        /// <summary>
        /// Subscription renewal code for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string RenewalCode { get; set; }
        /// <summary>
        /// Source for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string Source { get; set; }
        #endregion
        #region Mailing/Billing Address
        /// <summary>
        /// Mailing first name of the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string MailingFirstName { get; set; }
        /// <summary>
        /// Mailing last name of the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string MailingLastName { get; set; }
        /// <summary>
        /// Mailing email for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string MailingEmail { get; set; }
        /// <summary>
        /// Mailing title for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string MailingTitle { get; set; }
        /// <summary>
        /// Mailing company for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string MailingCompany { get; set; }
        /// <summary>
        /// Mailing address for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string MailingAddress { get; set; }
        /// <summary>
        /// Mailing address2 for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string MailingAddress2 { get; set; }
        /// <summary>
        /// Mailing city for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string MailingCity { get; set; }
        /// <summary>
        /// Mailing state for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string MailingState { get; set; }
        /// <summary>
        /// Mailing zip for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string MailingZip { get; set; }
        /// <summary>
        /// Mailing plus4 for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string MailingPlus4 { get; set; }
        /// <summary>
        /// Mailing county for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string MailingCounty { get; set; }
        /// <summary>
        /// Mailing country for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string MailingCountry { get; set; }
        /// <summary>
        /// Mailing latitude for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public float MailingLatitude { get; set; }
        /// <summary>
        /// Mailing longitude for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public float MailingLongitude { get; set; }
        /// <summary>
        /// Billing first name of the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string BillingFirstName { get; set; }
        /// <summary>
        /// Billing last name of the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string BillingLastName { get; set; }
        /// <summary>
        /// Mailing email for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string BillingEmail { get; set; }
        /// <summary>
        /// Mailing title for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string BillingTitle { get; set; }
        /// <summary>
        /// Billing company for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string BillingCompany { get; set; }
        /// <summary>
        /// Billing address for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string BillingAddress { get; set; }
        /// <summary>
        /// Billing address2 for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string BillingAddress2 { get; set; }
        /// <summary>
        /// Billing city for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string BillingCity { get; set; }
        /// <summary>
        /// Billing state for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string BillingState { get; set; }
        /// <summary>
        /// Billing zip for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string BillingZip { get; set; }
        /// <summary>
        /// Billing plus4 for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string BillingPlus4 { get; set; }
        /// <summary>
        /// Billing county for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string BillingCounty { get; set; }
        /// <summary>
        /// Billing country for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public string BillingCountry { get; set; }
        /// <summary>
        /// Billing latitude for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public float BillingLatitude { get; set; }
        /// <summary>
        /// Billing longitude for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public float BillingLongitude { get; set; }
        #endregion
        #region CustomFields / Product Dimensions
        /// <summary>
        /// List of PaidSubscriptionProductDemographic objects for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public List<PaidSubscriptionProductDemographic> ProductDemographics { get; set; }
        #endregion
        /// <summary>
        /// List of PaidBundleItem objects for the PaidSubscription object.
        /// </summary>
        [DataMember]
        public List<PaidBundleItem> BundleItems { get; set; }
        #endregion
    }
}