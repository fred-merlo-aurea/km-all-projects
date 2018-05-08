using System;
using System.Collections.Generic;

namespace ECN_Framework_BusinessLayer.Activity.Report.Interfaces
{
    public interface IBlastDeliveryProxy
    {
        IList<ECN_Framework_Entities.Activity.Report.BlastDelivery> Get(
            string customerIds,
            DateTime startDate,
            DateTime endDate,
            bool unique);
    }
}