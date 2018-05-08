using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class StatisticsByField
    {
        public static List<ECN_Framework_Entities.Activity.Report.StatisticsByField> Get(int blastID, string field)
        {
            return ECN_Framework_DataLayer.Activity.Report.StatisticsByField.Get(blastID, field);
        }
    }
}
