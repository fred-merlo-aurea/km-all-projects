using System;
using System.Collections.Generic;
using ECN_Framework_Entities.Activity.Report.Undeliverable;

namespace ECN_Framework_BusinessLayer.Activity.Report.Interfaces
{
    public interface IUndeliverableReportProxy
    {
        IList<IUndeliverable> GetAll(DateTime startDate, DateTime endDate, int customerId);
        IList<IUndeliverable> GetHardBounces(DateTime startDate, DateTime endDate, int customerId);
        IList<IUndeliverable> GetSoftBounces(DateTime startDate, DateTime endDate, int customerId);
        IList<IUndeliverable> GetUnsubscribes(DateTime startDate, DateTime endDate, int customerId);
        IList<IUndeliverable> GetMailBoxFull(DateTime startDate, DateTime endDate, int customerId);
    }
}