using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_Entities.Communicator
{
    public class MarketingAutomationHistory
    {
        public MarketingAutomationHistory()
        {
            MarketingAutomationHistoryID = -1;
            MarketingAutomationID = -1;
            UserID = -1;
            Action = "";
            HistoryDate = DateTime.Now;
        }

        public int MarketingAutomationHistoryID { get; set; }
        public int MarketingAutomationID { get; set; }
        public int UserID { get; set; }

        public string Action { get; set; }

        public DateTime HistoryDate { get; set; }

    }
}
