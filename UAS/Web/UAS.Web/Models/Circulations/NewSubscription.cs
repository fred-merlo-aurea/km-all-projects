using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UAS.Web.Models.Circulations
{
    public class NewSubscription
    {
        public bool AllowDataEntry { get; set; }
        public string ClientName { get; set; }
        public int PubSubscriptionID { get; set; }
        public int SubscriptionID { get; set; }
        public int PubQSourceID { get; set; }
        public int SequenceID { get; set; }
        public string PubCode { get; set; }
        public int PubID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public int AddressType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public int? CountryID { get; set; }
        public string Country { get; set; }
        public int? RegionID { get; set; }
        public string FullZip { get; set; }
        public string Zip { get; set; }
        public string Plus4 { get; set; }
        public string County { get; set; }
        public int PhoneCode { get; set; }
        public string Phone { get; set; }
        public string PhoneExt { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int  EmailStatusID { get; set; }
        public string Website { get; set; }
        public string OnBehalfOf { get; set; }
        public int SubscriptionStatus { get; set; }
        public bool CopiesEnabled { get; set; }
        public bool IsSubscribed { get; set; }
        public bool IsPaid { get; set; }
        public bool IsActive { get; set; }
        public bool IsInActiveWaveMailing { get; set; }
        public int CategoryCodeID { get; set; }
        public int CategoryCodeTypeID { get; set; }
        public int TransactionCodeID { get; set; }
        public bool btnPOKillChecked { get; set; }
        public bool btnPersonKillChecked { get; set; }
        public bool btnOnBehalfKillChecked { get; set; }
        public string AccountNumber { get; set; }
        public string TransactionName { get; set; }
        public int LockedByUser { get; set; }
        public int QSourceID { get; set; }
        public int Par3C { get; set; }
        public int Copies { get; set; }
        public string MemberGroup { get; set; }
        public string OriginalSubscriberSourceCode { get; set; }
        public string SubscriberSourceCode { get; set; }
        public int SubSrcID { get; set; }
        public string Deliverability { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Verify { get; set; }
        public DateTime? QDate { get; set; }
        public bool  MadeResponseChange { get; set; }
        public bool TriggerQualDate { get; set; }
        public bool  qualDateChanged { get; set; }
        public bool  IsCopiesEnabled{ get; set; }
        public bool? MailPermission { get; set; }
        public bool? FaxPermission { get; set; }
        public bool? PhonePermission { get; set; }
        public bool? OtherProductsPermission { get; set; }
        public bool? EmailRenewPermission { get; set; }
        public bool? ThirdPartyPermission { get; set; }
        public bool? TextPermission { get; set; }
        public string StatusUpdatedReason { get; set; }
        public bool IsLocked { get; set; }
        public bool Enabled { get; set; }
        public bool ReactivateEnabled { get; set; }
        public bool qualDateTriggered { get; set; }
        public bool IsNewSubscription { get; set; }
        public bool  AreQuestionsRequired { get; set; }
        public Paid PaymentInfo { get; set; }
        public Billed BillToInfo { get; set; }
        public IList<AdHocs> adhoclist { get; set; }
        public IList<Question> QuestionList { get; set; }
        public Dictionary<string,string> ErrorList { get; set; }
        public NewSubscription()
        {
            ErrorList = new Dictionary<string, string>();
            QuestionList = new List<Question>();
            adhoclist = new List<AdHocs>();
            
        }
    }


    
    public class TransactionTracker
    {
        public int CategoryCodeTypeId { get; set; }
        public int CategoryCodeID { get; set; }
        public bool RequalOnlyChange { get; set; }
        public bool RenewalPayment { get; set; }
        public bool CompleteChange { get; set; }
        public bool AddressOnlyChange { get; set; }
    }

   
}