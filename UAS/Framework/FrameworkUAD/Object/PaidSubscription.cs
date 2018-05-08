using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class PaidSubscription
    {
        public PaidSubscription()
        {
            OrderDate = DateTimeFunctions.GetMinDate();
            IsGift = false;
            Payment = new PaidSubscriptionPayment();
            BundleItems = new List<PaidBundleItem>();

            SubscriberFirstName = string.Empty;
            SubscriberLastName = string.Empty;
            SubscriberEmail = string.Empty;
            SubscriberTitle = string.Empty;
            SubscriberCompany = string.Empty;
            SubscriberPhone = string.Empty;
            SubscriberMobile = string.Empty;
            SubscriberFax = string.Empty;
            RenewalCode = string.Empty;
            Source = string.Empty;

            MailingFirstName = string.Empty;
            MailingLastName = string.Empty;
            MailingEmail = string.Empty;
            MailingTitle = string.Empty;
            MailingCompany = string.Empty;
            MailingAddress = string.Empty;
            MailingAddress2 = string.Empty;
            MailingCity = string.Empty;
            MailingState = string.Empty;
            MailingZip = string.Empty;
            MailingPlus4 = string.Empty;
            MailingCounty = string.Empty;
            MailingCountry = string.Empty;
            MailingLatitude = 0;
            MailingLongitude = 0;
            BillingFirstName = string.Empty;
            BillingLastName = string.Empty;
            BillingEmail = string.Empty;
            BillingTitle = string.Empty;
            BillingCompany = string.Empty;
            BillingAddress = string.Empty;
            BillingAddress2 = string.Empty;
            BillingCity = string.Empty;
            BillingState = string.Empty;
            BillingZip = string.Empty;
            BillingPlus4 = string.Empty;
            BillingCounty = string.Empty;
            BillingCountry = string.Empty;
            BillingLatitude = 0;
            BillingLongitude = 0;

            ProductDemographics = new List<PaidSubscriptionProductDemographic>();
        }
        #region Properties
        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public bool IsGift { get; set; }
        [DataMember]
        public PaidSubscriptionPayment Payment { get; set; }
        #region Subscriber
        [DataMember]
        public string SubscriberFirstName { get; set; }
        [DataMember]
        public string SubscriberLastName { get; set; }
        [DataMember]
        public string SubscriberEmail { get; set; }
        [DataMember]
        public string SubscriberTitle { get; set; }
        [DataMember]
        public string SubscriberCompany { get; set; }
        [DataMember]
        public string SubscriberPhone { get; set; }
        [DataMember]
        public string SubscriberMobile { get; set; }
        [DataMember]
        public string SubscriberFax { get; set; }
        [DataMember]
        public string RenewalCode { get; set; }
        [DataMember]
        public string Source { get; set; }
        #endregion
        #region Mailing/Billing Address
        [DataMember]
        public string MailingFirstName { get; set; }
        [DataMember]
        public string MailingLastName { get; set; }
        [DataMember]
        public string MailingEmail { get; set; }
        [DataMember]
        public string MailingTitle { get; set; }
        [DataMember]
        public string MailingCompany { get; set; }
        [DataMember]
        public string MailingAddress { get; set; }
        [DataMember]
        public string MailingAddress2 { get; set; }
        [DataMember]
        public string MailingCity { get; set; }
        [DataMember]
        public string MailingState { get; set; }
        [DataMember]
        public string MailingZip { get; set; }
        [DataMember]
        public string MailingPlus4 { get; set; }
        [DataMember]
        public string MailingCounty { get; set; }
        [DataMember]
        public string MailingCountry { get; set; }
        [DataMember]
        public float MailingLatitude { get; set; }
        [DataMember]
        public float MailingLongitude { get; set; }

        [DataMember]
        public string BillingFirstName { get; set; }
        [DataMember]
        public string BillingLastName { get; set; }
        [DataMember]
        public string BillingEmail { get; set; }
        [DataMember]
        public string BillingTitle { get; set; }
        [DataMember]
        public string BillingCompany { get; set; }
        [DataMember]
        public string BillingAddress { get; set; }
        [DataMember]
        public string BillingAddress2 { get; set; }
        [DataMember]
        public string BillingCity { get; set; }
        [DataMember]
        public string BillingState { get; set; }
        [DataMember]
        public string BillingZip { get; set; }
        [DataMember]
        public string BillingPlus4 { get; set; }
        [DataMember]
        public string BillingCounty { get; set; }
        [DataMember]
        public string BillingCountry { get; set; }
        [DataMember]
        public float BillingLatitude { get; set; }
        [DataMember]
        public float BillingLongitude { get; set; }
        #endregion
        #region CustomFields / Product Dimensions
        [DataMember]
        public List<PaidSubscriptionProductDemographic> ProductDemographics { get; set; }
        #endregion

        [DataMember]
        public List<PaidBundleItem> BundleItems { get; set; }

        #region Ignored
         [IgnoreDataMember]
        public int SubscriberId_SubGen { get; set; }
         [IgnoreDataMember]
        public int SubscriberId_UAD { get; set; }
        //not exposed
        [IgnoreDataMember]
        public int ProductSubscriptionId_SubGen { get; set; }
        [IgnoreDataMember]
        public int ProductSubscriptionId_UAD { get; set; }
        [IgnoreDataMember]
        public int SubscriptionId_UAD { get; set; }
        #endregion
        #endregion
    }
}
