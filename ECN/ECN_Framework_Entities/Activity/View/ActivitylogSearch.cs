using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Activity.View
{
        [Serializable]
        [DataContract]
        public class ActivitylogSearch
         {
            public ActivitylogSearch()
            {
                TotalRowsCount = 0;
                EmailID = 0;
                ActionDate = null;
                EmailSubject = string.Empty;
                BlastID = string.Empty;
                ActionTypeCode = string.Empty;
                ActionValue = string.Empty;
        }

            [DataMember]
            public int TotalRowsCount { get; set; }
            [DataMember]
            public int EmailID { get; set; }
            [DataMember]
            public string EmailSubject { get; set; }
            [DataMember]
            public string BlastID { get; set; }
            [DataMember]
            public DateTime? ActionDate { get; set; }
            [DataMember]
            public string ActionTypeCode { get; set; }
            [DataMember]
            public string ActionValue { get; set; }
    }
  }

