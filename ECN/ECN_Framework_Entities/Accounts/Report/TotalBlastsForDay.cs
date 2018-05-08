using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_Entities.Accounts.Report
{
    public class TotalBlastsForDay
    {
        public TotalBlastsForDay() { }

        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CampaignItemName { get; set; }
        public int BlastID { get; set; }
        public string EmailSubject { get; set; }
        public string GroupName { get; set; }
        public DateTime SendTime { get; set; }
        public string Status { get; set; }
        public string IsTestBlast { get; set; }
        public long SendCount { get; set; }
        public int UniqueEmailOpenCount { get; set; }
        public int UniqueEmailClickCount { get; set; }
    }
}
