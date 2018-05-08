using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class OnOffsByFieldProxy : IOnOffsByFieldProxy
    {
        public IList<ECN_Framework_Entities.Activity.Report.OnOffsByField> Get(
            int groupId,
            string field,
            DateTime startDate,
            DateTime endDate,
            string reporttype)
        {
            return OnOffsByField.Get(groupId, field, startDate, endDate, reporttype);
        }
    }
}
