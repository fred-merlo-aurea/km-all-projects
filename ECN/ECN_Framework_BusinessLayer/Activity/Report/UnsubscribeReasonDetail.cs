using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class UnsubscribeReasonDetail
    {
        public static DataSet GetPaging(string SearchField, string SearchCriteria, DateTime FromDate, DateTime ToDate, int CustomerID, string Reason, int PageSize, int CurrentPage)
        {
            return ECN_Framework_DataLayer.Activity.Report.UnsubscribeReasonDetail.GetPaging(SearchField, SearchCriteria, FromDate, ToDate, CustomerID, Reason, PageSize, CurrentPage);
        }

        public static List<ECN_Framework_Entities.Activity.Report.UnsubscribeReasonDetail> GetReport(string SearchField, string SearchCriteria, DateTime FromDate, DateTime ToDate, int CustomerID,
            string Reason)
        {
            return ECN_Framework_DataLayer.Activity.Report.UnsubscribeReasonDetail.GetReport(SearchField, SearchCriteria, FromDate, ToDate, CustomerID, Reason);
        }
    }
}
