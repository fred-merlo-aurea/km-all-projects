using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class ImportSubscriber
    {
        public ImportSubscriber()
        {
            Dimensions = new ImportDimension();
            DateCreated = DateTime.Now;
            DateUpdated = null;
            IsMergedToUAD = false;
            DateMergedToUAD = null;
        }
        #region Properties
        [DataMember]
        public int SystemSubscriberID { get; set; }
        [DataMember]
        public bool IsLead { get; set; }
        [DataMember]
        public string RenewalCode_CustomerID { get; set; }
        [DataMember]
        public string SubscriberAccountFirstName { get; set; }
        [DataMember]
        public string SubscriberAccountLastName { get; set; }
        [DataMember]
        public string SubscriberEmail { get; set; }
        [DataMember]
        public string SubscriberPhone { get; set; }
        [DataMember]
        public string SubscriberSource { get; set; }
        [DataMember]
        public int SubscriptionGeniusMailingAddressID { get; set; }
        [DataMember]
        public string MailingAddressFirstName { get; set; }
        [DataMember]
        public string MailingAddressLastName { get; set; }
        [DataMember]
        public string MailingAddressTitle { get; set; }
        [DataMember]
        public string MailingAddressLine1 { get; set; }
        [DataMember]
        public string MailingAddressLine2 { get; set; }
        [DataMember]
        public string MailingAddressCity { get; set; }
        [DataMember]
        public string MailingAddressState { get; set; }
        [DataMember]
        public string MailingAddressZip { get; set; }
        [DataMember]
        public string MailingAddressCountry { get; set; }
        [DataMember]
        public string MailingAddressCompany { get; set; }
        [DataMember]
        public int SystemBillingAddressID { get; set; }
        [DataMember]
        public string BillingAddressFirstName { get; set; }
        [DataMember]
        public string BillingAddressLastName { get; set; }
        [DataMember]
        public string BillingAddressLine1 { get; set; }
        [DataMember]
        public string BillingAddressLine2 { get; set; }
        [DataMember]
        public string BillingAddressCity { get; set; }
        [DataMember]
        public string BillingAddressState { get; set; }
        [DataMember]
        public string BillingAddressZip { get; set; }
        [DataMember]
        public string BillingAddressCountry { get; set; }
        [DataMember]
        public string BillingAddressCompany { get; set; }
        [DataMember]
        public string PublicationName { get; set; }
        [DataMember]
        public int IssuesLeft { get; set; }
        [DataMember]
        public double UnearnedRevenue { get; set; }
        [DataMember]
        public int Copies { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public int ParentSubscriptionID { get; set; }
        [DataMember]
        public bool IsSiteLicenseMaster { get; set; }
        [DataMember]
        public bool IsSiteLicenseSeat { get; set; }
        [DataMember]
        public DateTime SubscriptionCreatedDate { get; set; }
        [DataMember]
        public DateTime SubscriptionRenewDate { get; set; }
        [DataMember]
        public DateTime SubscriptionExpireDate { get; set; }
        [DataMember]
        public DateTime SubscriptionLastQualifiedDate { get; set; }
        [DataMember]
        public string SubscriptionType { get; set; }
        [DataMember]
        public string AuditCategoryName { get; set; }
        [DataMember]
        public string AuditCategoryCode { get; set; }
        [DataMember]
        public string AuditRequestTypeName { get; set; }
        [DataMember]
        public string AuditRequestTypeCode { get; set; }
        [DataMember]
        public int TransactionID { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public int account_id { get; set; }

        [DataMember]
        public ImportDimension Dimensions { get; set; }

        //columns for sending to UAD
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public bool IsMergedToUAD { get; set; }
        [DataMember]
        public DateTime? DateMergedToUAD { get; set; }
        #endregion
    }
}
