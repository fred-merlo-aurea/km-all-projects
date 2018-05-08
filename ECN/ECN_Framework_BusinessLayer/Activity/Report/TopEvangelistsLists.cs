using System;
using System.Collections.Generic;
using System.Data;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class TopEvangelistsLists
    {
        public static DataTable Get(int campaignItemID)
        {
            return ECN_Framework_DataLayer.Activity.Report.TopEvangelists.Get(campaignItemID);
        }
    }
}
