using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_BusinessLayer.Publisher.Report
{
    [Serializable]
    public class ActivityForwardsDetails
    {
        public static List<ECN_Framework_Entities.Publisher.Report.ActivityForwardsDetails> GetList(int editionID, int blastID)
        {
            return ECN_Framework_DataLayer.Publisher.Report.ActivityFowardsDetails.GetList(editionID, blastID);
        }
    }
}
