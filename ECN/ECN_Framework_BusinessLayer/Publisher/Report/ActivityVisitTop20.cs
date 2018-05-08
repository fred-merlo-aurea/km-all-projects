using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_BusinessLayer.Publisher.Report
{
    [Serializable]
    public class ActivityVisitTop20
    {
        public static List<ECN_Framework_Entities.Publisher.Report.ActivityVisitTop20> GetList(int editionID, int blastID)
        {
            return ECN_Framework_DataLayer.Publisher.Report.ActivityVisitTop20.GetList(editionID, blastID);
        }
    }
}
