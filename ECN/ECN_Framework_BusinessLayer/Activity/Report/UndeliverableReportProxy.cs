using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using ECN_Framework_Entities.Activity.Report.Undeliverable;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class UndeliverableReportProxy : IUndeliverableReportProxy
    {
        public IList<IUndeliverable> GetAll(DateTime startDate, DateTime endDate, int customerId)
        {
            return UndeliverableReport.GetAll(startDate, endDate, customerId);
        }

        public IList<IUndeliverable> GetHardBounces(DateTime startDate, DateTime endDate, int customerId)
        {
            return UndeliverableReport.GetHardBounces(startDate, endDate, customerId);
        }

        public IList<IUndeliverable> GetSoftBounces(DateTime startDate, DateTime endDate, int customerId)
        {
            return UndeliverableReport.GetSoftBounces(startDate, endDate, customerId);
        }

        public IList<IUndeliverable> GetUnsubscribes(DateTime startDate, DateTime endDate, int customerId)
        {
            return UndeliverableReport.GetUnsubscribes(startDate, endDate, customerId);
        }

        public IList<IUndeliverable> GetMailBoxFull(DateTime startDate, DateTime endDate, int customerId)
        {
            return UndeliverableReport.GetMailBoxFull(startDate, endDate, customerId);
        }
    }
}
