using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_BusinessLayer.Publisher.Report
{
    [Serializable]
    public class ActivityTopClicks
    {
        public static List<ECN_Framework_Entities.Publisher.Report.ActivityTopClicks> GetList(int editionID, int blastID, int TopCount)
        {
            return ECN_Framework_DataLayer.Publisher.Report.ActivityTopClicks.GetList(editionID, blastID, TopCount);
        }
    }
}
