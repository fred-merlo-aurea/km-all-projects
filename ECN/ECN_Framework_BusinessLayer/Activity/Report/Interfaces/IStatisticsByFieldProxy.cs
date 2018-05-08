using System.Collections.Generic;
using FrameworkEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_BusinessLayer.Activity.Report.Interfaces
{
    public interface IStatisticsByFieldProxy
    {
        IList<FrameworkEntities.StatisticsByField> Get(int blastId, string field);
    }
}