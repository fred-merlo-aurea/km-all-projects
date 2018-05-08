using System;
using System.Collections.Generic;
using FrameworkEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_BusinessLayer.Activity.Report.Interfaces
{
    public interface IOnOffsByFieldProxy
    {
        IList<FrameworkEntities.OnOffsByField> Get(
            int groupId,
            string field,
            DateTime startDate,
            DateTime endDate,
            string reporttype);
    }
}
