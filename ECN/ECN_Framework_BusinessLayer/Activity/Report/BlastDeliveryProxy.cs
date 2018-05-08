using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using ReportEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class BlastDeliveryProxy : IBlastDeliveryProxy
    {
        public IList<ReportEntities.BlastDelivery> Get(
            string customerIds,
            DateTime startDate,
            DateTime endDate,
            bool unique)
        {
            return BlastDelivery.Get(customerIds, startDate, endDate, unique);
        }
    }
}
