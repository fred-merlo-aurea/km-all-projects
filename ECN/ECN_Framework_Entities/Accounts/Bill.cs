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
    public class Bill
    {
        public Bill() 
        {
            BillID = -1;
            CustomerID = -1;
            QuoteID = -1;
            //CreateDate =
            Source = -1;
            IsSyncToQB = false;
            ErrorList = new List<ECNError>();
        }

        [DataMember]
        public int BillID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public int QuoteID { get; set; }
        [DataMember]
        public DateTime CreateDate { get; set; }
        [DataMember]
        public int Source { get; set; }
        [DataMember]
        public bool IsSyncToQB { get; set; }
        //validation
        public List<ECNError> ErrorList { get; set; }
    }
}
