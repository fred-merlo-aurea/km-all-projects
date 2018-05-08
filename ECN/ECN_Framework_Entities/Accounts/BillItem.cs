using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class BillItem
    {
        public BillItem()
        {
            BillItemID = -1;
            BillID = -1;
            QuoteItemID = -1;
            //ChangedDate
            Quantity = -1;
            Rate = -1;
            //StartDate
            //EndDate
            Status = -1;
            TransactionID = string.Empty;
            IsProcessedByLicenseEngine = false;
            ErrorList = new List<ECNError>();
        }
        
        public int BillItemID { get; set; }
        public int BillID { get; set; }
        public int QuoteItemID { get; set; }
        public DateTime ChangedDate { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public string TransactionID { get; set; }
        public bool IsProcessedByLicenseEngine { get; set; }
        //validation
        public List<ECNError> ErrorList { get; set; }
    }
}
