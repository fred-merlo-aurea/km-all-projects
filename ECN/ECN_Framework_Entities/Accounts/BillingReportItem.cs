using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class BillingReportItem
    {
        public BillingReportItem()
        {
            BillingItemID = -1;
            BillingReportID = -1;
            BaseChannelID = -1;
            BaseChannelName = string.Empty; 
            BlastID = null;
            CustomerID = -1;
            CustomerName = string.Empty; 
            IsFlatRateItem = false;
            IsMasterFile = false;
            IsFulfillment = false;
            AmountOfItems = null;
            InvoiceText = string.Empty; 
            CreatedDate = null;
            CreatedUserID = null;
            UpdatedDate = null;
            UpdatedUserID = null;
            IsDeleted = false;
            Amount = null;
            SendTime = null;
            BlastField1 = null;
            GroupName = string.Empty;
        }

        #region Properties
        [DataMember]
        public int BaseChannelID { get; set; }
        [DataMember]
        public string BaseChannelName { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public int? BlastID { get; set; }
        [DataMember]
        public int BillingItemID { get; set; }
        [DataMember]
        public int BillingReportID { get; set; }
        [DataMember]
        public bool IsFlatRateItem { get; set; }
        [DataMember]
        public bool IsMasterFile { get; set; }
        [DataMember]
        public bool IsFulfillment { get; set; }
        [DataMember]
        public int? AmountOfItems { get; set; }
        [DataMember]
        public string InvoiceText { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public decimal? Amount { get; set; }
        [DataMember]
        public DateTime? SendTime { get; set; }
        [DataMember]
        public string BlastField1 { get; set; }
        [DataMember]
        public string BlastField2 { get; set; }
        [DataMember]
        public string BlastField3 { get; set; }
        [DataMember]
        public string BlastField4 { get; set; }
        [DataMember]
        public string BlastField5 { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public string FromName { get; set; }
        [DataMember]
        public string FromEmail { get; set; }
        [DataMember]
        public string GroupName { get; set; }

        #endregion
    }
}
