using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
     [Serializable]
    public class FlashReport
    {
         public static List<ECN_Framework_Entities.Activity.Report.FlashReport> GetList(int groupID, int CustomerID, string PromoCode, DateTime FromDate, DateTime ToDate)
         {
             return ECN_Framework_DataLayer.Activity.Report.FlashReport.GetList(groupID, CustomerID, PromoCode, FromDate, ToDate);
         }
    }
}
