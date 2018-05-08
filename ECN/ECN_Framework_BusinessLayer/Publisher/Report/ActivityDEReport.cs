using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ECN_Framework_BusinessLayer.Publisher.Report
{
    [Serializable]
    public class ActivityDEReport
    {
        public ActivityDEReport()
        {
        }

        public string EmailAddress { get; set; }
        public int PageNumber { get; set; }
        public DateTime ActionDate { get; set; }
        public string IP { get; set; }

        public static List<ActivityDEReport> GetList(int editionID, int blastID, string reportType, int pageNo, int pageSize, ref int RecordCount)
        {
            List<ActivityDEReport> lActivityDEReport = new List<ActivityDEReport>();

            DataSet ds = ECN_Framework_DataLayer.Publisher.Report.ActivityDEReportDetails.GetList(editionID, blastID, reportType, pageNo, pageSize);

            RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                ActivityDEReport r = new ActivityDEReport();

                r.EmailAddress = dr["EmailAddress"].ToString();
                r.PageNumber = Convert.ToInt32(dr["PageNumber"]);
                r.ActionDate = Convert.ToDateTime(dr["ActionDate"]);
                r.IP = dr["IP"].ToString();

                lActivityDEReport.Add(r);
            }

            return lActivityDEReport;
        }
    }
}
