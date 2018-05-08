using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity.Report
{
    [Serializable]
    public class AudienceEngagementReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.AudienceEngagementReport> GetList(int groupID, int clickpercentage, int days, string download, string downloadType)
        {
            List<ECN_Framework_Entities.Activity.Report.AudienceEngagementReport> retList = new List<ECN_Framework_Entities.Activity.Report.AudienceEngagementReport>();
            string sqlQuery = "spAudienceEngagementReport";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@ClickPercentage", clickpercentage);
            cmd.Parameters.AddWithValue("@Days", days);
            cmd.Parameters.AddWithValue("@Download", download);
            cmd.Parameters.AddWithValue("@DownloadType", downloadType);
            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {

                var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.AudienceEngagementReport>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    ECN_Framework_Entities.Activity.Report.AudienceEngagementReport x = builder.Build(rdr);
                    retList.Add(x);
                }
                rdr.Close();
                rdr.Dispose();
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static List<ECN_Framework_Entities.Activity.Report.AudienceEngagementReport> GetListByRange(int groupID, int clickpercentage, string download, string downloadType, DateTime startDate, DateTime endDate)
        {
            List<ECN_Framework_Entities.Activity.Report.AudienceEngagementReport> retList = new List<ECN_Framework_Entities.Activity.Report.AudienceEngagementReport>();
            string sqlQuery = "spAudienceEngagementReport_ByRange";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@ClickPercentage", clickpercentage);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            cmd.Parameters.AddWithValue("@Download", download);
            cmd.Parameters.AddWithValue("@DownloadType", downloadType);
            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {

                var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.AudienceEngagementReport>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    ECN_Framework_Entities.Activity.Report.AudienceEngagementReport x = builder.Build(rdr);
                    retList.Add(x);
                }
                rdr.Close();
                rdr.Dispose();
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static DataTable DownloadList(int groupID, int clickpercentage, int days, string downloadType, string dataToInclude)
        {
            List<ECN_Framework_Entities.Activity.Report.AudienceEngagementReport> retList = new List<ECN_Framework_Entities.Activity.Report.AudienceEngagementReport>();
            string sqlQuery = "spAudienceEngagementReport";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@ClickPercentage", clickpercentage);
            cmd.Parameters.AddWithValue("@Days", days);
            cmd.Parameters.AddWithValue("@Download", "Y");
            cmd.Parameters.AddWithValue("@DownloadType", downloadType);
            cmd.Parameters.AddWithValue("@DataToInclude", dataToInclude);
            DataTable dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
            return dt;
        }
    }
}
