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
    public class OnOffsByField
    {
        public static List<ECN_Framework_Entities.Activity.Report.OnOffsByField> Get(int groupID, string field, DateTime startdate, DateTime enddate, string reporttype)
        {
            return ECN_Framework_DataLayer.Activity.Report.OnOffsByField.Get(groupID, field, startdate, enddate, reporttype);
        }
    }
}
