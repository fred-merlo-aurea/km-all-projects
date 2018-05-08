using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class EmailsDeliveredByPercentage
    {
        public static List<ECN_Framework_Entities.Activity.Report.EmailsDeliveredByPercentage> Get(int customerID, DateTime fromdate, DateTime todate)
        {
            return ECN_Framework_DataLayer.Activity.Report.EmailsDeliveredByPercentage.Get(customerID, fromdate, todate);
        }
    }
}
