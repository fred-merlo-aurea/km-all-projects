using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class UnsubscribeReason
    {
        public static List<ECN_Framework_Entities.Activity.Report.UnsubscribeReason> Get(string SearchField, string SearchCriteria,int customerID, DateTime FromDate, DateTime ToDate)
        {
            return ECN_Framework_DataLayer.Activity.Report.UnsubscribeReason.Get(SearchField, SearchCriteria, FromDate, ToDate, customerID);
        }
    }
}
