using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_Entities.Communicator
{
    public class MAConnector
    {
        public MAConnector()
        {
            ConnectorID = -1;
            From = "";
            To = "";
            MarketingAutomationID = -1;
            ControlID = "";
        }

        public int ConnectorID { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public int MarketingAutomationID { get; set; }

        public string ControlID { get; set; }
        
    }
}
