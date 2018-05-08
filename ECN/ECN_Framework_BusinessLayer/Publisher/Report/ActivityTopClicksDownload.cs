using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_BusinessLayer.Publisher.Report
{
    [Serializable]
    public class ActivityTopClicksDownload
    {
        public static List<ECN_Framework_Entities.Publisher.Report.ActivityTopClicksDownload> GetList(int editionID, int blastID, int linkID, int TopCount, string type)
        {
            return ECN_Framework_DataLayer.Publisher.Report.ActivityTopClicksDownload.GetList(editionID, blastID, linkID, TopCount, type);
        }
    }
}
