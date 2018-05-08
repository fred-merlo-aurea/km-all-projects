using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;
using System.Data;

namespace ECN_Framework_BusinessLayer.Communicator.Report
{
    [Serializable]
    public class Deliverability
    {
        public static DataTable Get(string startDate, string endDate)
        {
            return ECN_Framework_DataLayer.Communicator.Report.Deliverability.Get(startDate, endDate);
        }

        public static DataTable GetByIP(string startDate, string endDate, string ipFilter)
        {
            return ECN_Framework_DataLayer.Communicator.Report.Deliverability.GetByIP(startDate, endDate, ipFilter); 
        }

    }
}
