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
    public class AudienceEngagementReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.AudienceEngagementReport> GetList(int GroupID, int clickpercentage, int days, string download, string downloadType)
        {
            return ECN_Framework_DataLayer.Activity.Report.AudienceEngagementReport.GetList(GroupID, clickpercentage, days, download, downloadType);
        }

        public static List<ECN_Framework_Entities.Activity.Report.AudienceEngagementReport> GetListByRange(int GroupID, int clickpercentage, string download, string downloadType, DateTime startDate, DateTime endDate)
        {
            return ECN_Framework_DataLayer.Activity.Report.AudienceEngagementReport.GetListByRange(GroupID, clickpercentage, download, downloadType, startDate, endDate);
        }

        public static DataTable DownloadList(int GroupID, int clickpercentage, int days, string downloadType,  string dataToInclude)
        {
            return ECN_Framework_DataLayer.Activity.Report.AudienceEngagementReport.DownloadList(GroupID, clickpercentage, days, downloadType, dataToInclude);
        }
    }
}
