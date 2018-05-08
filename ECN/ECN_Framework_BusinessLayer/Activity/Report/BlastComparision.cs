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
    public class BlastComparision
    {
        public static List<ECN_Framework_Entities.Activity.Report.BlastComparision> Get(string blastIDs)
        {
            return ECN_Framework_DataLayer.Activity.Report.BlastComparision.Get(blastIDs);
        }
    }
}
