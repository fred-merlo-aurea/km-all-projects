using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class BillingReport
    {
        public BillingReport() 
        {
            BillingReportID = -1;
            BillingReportName = string.Empty;
            IncludeFulfillment = false;
            IncludeMasterFile = false;
            StartDate = null ;
            EndDate = null;
            IsRecurring = false;
            RecurrenceType = string.Empty; 
            EmailBillingRate = 0.0;
            MasterFileRate = null;
            FulfillmentRate = null;
            FromEmail = string.Empty;
            ToEmail = string.Empty;
            FromName = string.Empty;
            Subject = string.Empty;
            IsDeleted = false;
            CreatedDate = null;
            CreatedUserID = null;
            UpdatedDate = null;
            UpdatedUserID = null;
            CustomerIDs = string.Empty;
            BaseChannelID = -1;
            AllCustomers = false;
            BlastFields = string.Empty;
        }

        #region Properties
        [DataMember]
        public int BillingReportID { get; set; }
        [DataMember]
        public string BillingReportName { get; set; }
        [DataMember]
        public bool IncludeMasterFile { get; set; }
        [DataMember]
        public bool IncludeFulfillment { get; set; }
        [DataMember]
        public DateTime? StartDate { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }
        [DataMember]
        public bool IsRecurring { get; set; }
        [DataMember]
        public string RecurrenceType { get; set; }
        [DataMember]
        public double EmailBillingRate { get; set; }
        [DataMember]
        public double? MasterFileRate { get; set; }
        [DataMember]
        public double? FulfillmentRate { get; set; }
        [DataMember]
        public string FromEmail { get; set; }
        [DataMember]
        public string FromName { get; set; }
        [DataMember]
        public string ToEmail { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public string CustomerIDs { get; set; }
        [DataMember]
        public int? BaseChannelID { get; set; }
        [DataMember]
        public bool? AllCustomers { get; set; }
        [DataMember]
        public string BlastFields { get; set; }
        #endregion


    }
}
