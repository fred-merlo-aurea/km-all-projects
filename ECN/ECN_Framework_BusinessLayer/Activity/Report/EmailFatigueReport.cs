using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    public class EmailFatigueReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.EmailFatigueReport> Get(int CustomerID, DateTime startDate, DateTime endDate, string filterField, int filterID)
        {
            List<ECN_Framework_Entities.Activity.Report.EmailFatigueReport> retList = new List<ECN_Framework_Entities.Activity.Report.EmailFatigueReport>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {

                retList = ECN_Framework_DataLayer.Activity.Report.EmailFatigueReport.GetReport(CustomerID, startDate, endDate, filterField, filterID);
                scope.Complete();
            }

            return retList;
        }
        public static DataTable Download(int CustomerID, DateTime startDate, DateTime endDate, string filterField, int filterID,string actionType, int grouping, int numberOfTouches)
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {

                dt = ECN_Framework_DataLayer.Activity.Report.EmailFatigueReport.Download(CustomerID, startDate, endDate, filterField, filterID,actionType,grouping,numberOfTouches);
                scope.Complete();
            }

            return dt;
        }
    }
}
