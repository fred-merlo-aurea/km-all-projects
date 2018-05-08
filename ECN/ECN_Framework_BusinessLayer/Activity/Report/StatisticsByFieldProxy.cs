using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using FrameworkEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class StatisticsByFieldProxy : IStatisticsByFieldProxy
    {
        public IList<FrameworkEntities.StatisticsByField> Get(int blastId, string field)
        {
            return StatisticsByField.Get(blastId, field);
        }
    }
}
