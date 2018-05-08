using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_BusinessLayer.Publisher.Report
{
    [Serializable]
    public class ActivitySearchDetails
    {
        public static List<ECN_Framework_Entities.Publisher.Report.ActivitySearchDetails> GetList(int editionID, int blastID)
        {
            return ECN_Framework_DataLayer.Publisher.Report.ActivitySearchDetails.GetList(editionID, blastID);
        }
    }
}
