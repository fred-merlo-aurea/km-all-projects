using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class UndeliverableReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.Undeliverable.IUndeliverable> GetAll(DateTime startDate, DateTime endDate, int customerId)
        {
            return ECN_Framework_DataLayer.Activity.Report.UndeliverableReport.GetAll(startDate, endDate, customerId);
        }

        public static List<ECN_Framework_Entities.Activity.Report.Undeliverable.IUndeliverable> GetHardBounces(DateTime startDate, DateTime endDate, int customerId)
        {
            return ECN_Framework_DataLayer.Activity.Report.UndeliverableReport.GetHardBounces(startDate, endDate, customerId);
        }

        public static List<ECN_Framework_Entities.Activity.Report.Undeliverable.IUndeliverable> GetSoftBounces(DateTime startDate, DateTime endDate, int customerId)
        {
            return ECN_Framework_DataLayer.Activity.Report.UndeliverableReport.GetSoftBounces(startDate, endDate, customerId);
        }

        public static List<ECN_Framework_Entities.Activity.Report.Undeliverable.IUndeliverable> GetUnsubscribes(DateTime startDate, DateTime endDate, int customerId)
        {
            return ECN_Framework_DataLayer.Activity.Report.UndeliverableReport.GetUnsubscribes(startDate, endDate, customerId);
        }

        public static List<ECN_Framework_Entities.Activity.Report.Undeliverable.IUndeliverable> GetMailBoxFull(DateTime startDate, DateTime endDate, int customerId)
        {
            return ECN_Framework_DataLayer.Activity.Report.UndeliverableReport.GetMailBoxFull(startDate, endDate, customerId);
        }
    }
}
