using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.DomainTracker
{
    [Serializable]
    [DataContract]
    public class DomainTrackerActivity
    {
        private const string CmdTextOsStats = "e_DomainTrackerActivity_GetOSStats";
        private const string CmdTextBrowserStats = "e_DomainTrackerActivity_GetBrowserStats";
        private const string CmdTextBrowserStatsKnownUnknown = "e_DomainTrackerActivity_GetBrowserStats_Known_Unknown";
        private const string CmdTextHeatMapStats = "e_DomainTrackerActivity_HeatMapStats";
        private const string CmdTextTotalViews = "e_DomainTrackerActivity_GetTotalViews";
        private const string CmdTextPageViews = "e_DomainTrackerActivity_GetPageViews";
        private const string CmdTextProfileId = "e_DomainTrackerActivity_Select_ProfileID";
        private const string PageUrlName = "@PageUrl";
        private const string ProfileIdName = "@ProfileID";
        private const string DomainTrackerIdName = "@DomainTrackerID";
        private const string StartDateName = "@startDate";
        private const string EndDateName = "@endDate";
        private const string FilterName = "@Filter";
        private const string TypeFilterName = "@TypeFilter";

        public static int Save(ECN_Framework_Entities.DomainTracker.DomainTrackerActivity domainTrackerActivity)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerActivity_Save";
            cmd.Parameters.Add(new SqlParameter("@DomainTrackerID", domainTrackerActivity.DomainTrackerID));
            cmd.Parameters.Add(new SqlParameter("@Browser", domainTrackerActivity.Browser));
            cmd.Parameters.Add(new SqlParameter("@IPAddress", domainTrackerActivity.IPAddress));
            cmd.Parameters.Add(new SqlParameter("@OS", domainTrackerActivity.OS));
            cmd.Parameters.Add(new SqlParameter("@PageURL", domainTrackerActivity.PageURL));
            cmd.Parameters.Add(new SqlParameter("@ProfileID", domainTrackerActivity.ProfileID));
            cmd.Parameters.Add(new SqlParameter("@ReferralURL", domainTrackerActivity.ReferralURL));
            cmd.Parameters.Add(new SqlParameter("@SourceBlastID", domainTrackerActivity.SourceBlastID));
            cmd.Parameters.Add(new SqlParameter("@TimeStamp", domainTrackerActivity.TimeStamp));
            cmd.Parameters.Add(new SqlParameter("@UserAgent", domainTrackerActivity.UserAgent));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.DomainTracker.ToString()).ToString());
        }

        public static DataTable GetURLStats(int DomainTrackerID, string startDate, string endDate)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerActivity_GetURLStats";
            cmd.Parameters.Add(new SqlParameter("@DomainTrackerID", DomainTrackerID));
            cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
            cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }
        public static DataTable GetURLStats(int DomainTrackerID, string startDate, string endDate, string Filter = "", int TopRow = 0, string TypeFilter = "known")
        {
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                Filter = Filter.Trim();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerActivity_GetURLStats";
            cmd.Parameters.Add(new SqlParameter("@DomainTrackerID", DomainTrackerID));
            cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
            cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));
            cmd.Parameters.Add(new SqlParameter("@Filter", Filter));
            cmd.Parameters.Add(new SqlParameter("@TopRow", TopRow));
            cmd.Parameters.AddWithValue("@TypeFilter", TypeFilter);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static DataTable GetURLStats_Known_Unknown(int DomainTrackerID, string startDate, string endDate, string Filter = "", int TopRow = 0)
        {
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                Filter = Filter.Trim();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerActivity_GetURLStats_Known";
            cmd.Parameters.Add(new SqlParameter("@DomainTrackerID", DomainTrackerID));
            cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
            cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));
            cmd.Parameters.Add(new SqlParameter("@Filter", Filter));
            cmd.Parameters.Add(new SqlParameter("@TopRow", TopRow));
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static DataTable GetOSStats(int DomainTrackerID)
        {
            var cmd = BuildStoredProcedure(
                CmdTextOsStats,
                new Dictionary<string, object>
                {
                    [DomainTrackerIdName] = DomainTrackerID
                });
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static DataTable GetOSStats(int DomainTrackerID, string startDate, string endDate, string Filter = "", string TypeFilter = "known")
        {
            var cmd = BuildStoredProcedure(
                CmdTextOsStats,
                new Dictionary<string, object>
                {
                    [DomainTrackerIdName] = DomainTrackerID,
                    [StartDateName] = startDate,
                    [EndDateName] = endDate,
                    [FilterName] = !string.IsNullOrWhiteSpace(Filter) ? Filter.Trim() : Filter,
                    [TypeFilterName] = TypeFilter
                });
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static DataTable GetOSStats_Known_Unknown(int DomainTrackerID, string startDate, string endDate, string Filter = "")
        {
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                Filter = Filter.Trim();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerActivity_GetOSStats_Known_Unknown";
            cmd.Parameters.Add(new SqlParameter("@DomainTrackerID", DomainTrackerID));
            cmd.Parameters.Add(new SqlParameter("@startDate", startDate));
            cmd.Parameters.Add(new SqlParameter("@endDate", endDate));
            cmd.Parameters.Add(new SqlParameter("@Filter", Filter));
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }
        public static DataTable GetBrowserStats(int DomainTrackerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerActivity_GetBrowserStats";
            cmd.Parameters.Add(new SqlParameter("@DomainTrackerID", DomainTrackerID));
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static DataTable GetBrowserStats(int DomainTrackerID, string startDate, string endDate, string Filter = "", string TypeFilter = "known")
        {
            var cmd = BuildStoredProcedure(
                CmdTextBrowserStats,
                new Dictionary<string, object>
                {
                    [DomainTrackerIdName] = DomainTrackerID,
                    [StartDateName] = startDate,
                    [EndDateName] = endDate,
                    [FilterName] = !string.IsNullOrWhiteSpace(Filter) ? Filter.Trim() : Filter,
                    [TypeFilterName] = TypeFilter
                });
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static DataTable GetBrowserStats_Known_Unknown(int DomainTrackerID, string startDate, string endDate, string Filter = "")
        {
            var cmd = BuildStoredProcedure(
                CmdTextBrowserStatsKnownUnknown,
                new Dictionary<string, object>
                {
                    [DomainTrackerIdName] = DomainTrackerID,
                    [StartDateName] = startDate,
                    [EndDateName] = endDate,
                    [FilterName] = !string.IsNullOrWhiteSpace(Filter) ? Filter.Trim() : Filter
                });
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static DataTable GetHeatMapDataTableStats(int DomainTrackerID, string startDate, string endDate, string Filter = "", string TypeFilter = "known")
        {
            var cmd = BuildStoredProcedure(
                CmdTextHeatMapStats,
                new Dictionary<string, object>
                {
                    [DomainTrackerIdName] = DomainTrackerID,
                    [StartDateName] = startDate,
                    [EndDateName] = endDate,
                    [FilterName] = !string.IsNullOrWhiteSpace(Filter) ? Filter.Trim() : Filter,
                    [TypeFilterName] = TypeFilter
                });
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static DataTable GetHeatMapKnown_UnknownStats(int DomainTrackerID, string startDate, string endDate, string Filter = "")
        {
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                Filter = Filter.Trim();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerActivity_HeatMapStats_Known_Unknown";
            cmd.Parameters.Add(new SqlParameter("@DomainTrackerID", DomainTrackerID));
            cmd.Parameters.Add(new SqlParameter("@startDate", startDate));
            cmd.Parameters.Add(new SqlParameter("@endDate", endDate));
            cmd.Parameters.Add(new SqlParameter("@Filter", Filter));
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static DataTable GetTotalViews(int DomainTrackerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerActivity_GetTotalViews";
            cmd.Parameters.Add(new SqlParameter("@DomainTrackerID", DomainTrackerID));
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static DataTable GetTotalViews(int DomainTrackerID, string startDate, string endDate,string TypeFilter, string Filter = "")
        {
            var cmd = BuildStoredProcedure(
                CmdTextTotalViews,
                new Dictionary<string, object>
                {
                    [DomainTrackerIdName] = DomainTrackerID,
                    [StartDateName] = startDate,
                    [EndDateName] = endDate,
                    [FilterName] = !string.IsNullOrWhiteSpace(Filter) ? Filter.Trim() : Filter,
                    [TypeFilterName] = TypeFilter
                });
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static DataTable GetPageViewsPerDay(int DomainTrackerID)
        {
            var cmd = BuildStoredProcedure(
                CmdTextPageViews,
                new Dictionary<string, object>
                {
                    [DomainTrackerIdName] = DomainTrackerID
                });
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static DataTable GetPageViewsPerDay(int DomainTrackerID, string startDate, string endDate, string Filter = "", string TypeFilter = "known")
        {
            var cmd = BuildStoredProcedure(
                CmdTextPageViews,
                new Dictionary<string, object>
                {
                    [DomainTrackerIdName] = DomainTrackerID,
                    [StartDateName] = startDate,
                    [EndDateName] = endDate,
                    [FilterName] = !string.IsNullOrWhiteSpace(Filter) ? Filter.Trim() : Filter,
                    [TypeFilterName] = TypeFilter
                });
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static DataTable GetPageViewsPerDay_Known(int DomainTrackerID, string startDate, string endDate, string Filter = "")
        {
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                Filter = Filter.Trim();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerActivity_GetPageViews_Known";
            cmd.Parameters.Add(new SqlParameter("@DomainTrackerID", DomainTrackerID));
            cmd.Parameters.Add(new SqlParameter("@startDate", startDate));
            cmd.Parameters.Add(new SqlParameter("@endDate", endDate));
            cmd.Parameters.Add(new SqlParameter("@Filter", Filter));
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static List<ECN_Framework_Entities.DomainTracker.DomainTrackerActivity> GetByProfileID(int profileID, int domainTrackerID, DateTime? StartDate, DateTime? EndDate, string PageUrl)
        {
            var cmd = BuildStoredProcedure(
                CmdTextProfileId,
                new Dictionary<string, object>
                {
                    [ProfileIdName] = profileID,
                    [DomainTrackerIdName] = domainTrackerID,
                    [StartDateName] = StartDate,
                    [EndDateName] = EndDate,
                    [PageUrlName] = !string.IsNullOrWhiteSpace(PageUrl) ? PageUrl : null
                });
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.DomainTracker.FieldsValuePair> GetFieldValuesByProfileID(int? ProfileID, int domainTrackerID, string StartDate, string EndDate, string PageUrl)//, int recordcount)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerFields_Select_FieldValuePairs";
            cmd.Parameters.AddWithValue("@DomainTrackerID", domainTrackerID);
            cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
            //cmd.Parameters.AddWithValue("@recordcount", recordcount);
            // Data range
            if (!String.IsNullOrEmpty(StartDate))
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
            if (!String.IsNullOrEmpty(EndDate))
                cmd.Parameters.AddWithValue("@EndDate", plusOneDayToDate(EndDate));

            if (!String.IsNullOrEmpty(PageUrl))
                cmd.Parameters.AddWithValue("@PageUrl", PageUrl);

            return GetListFieldsValuePair(cmd);
        }

        public static void MergeAnonActivity(string anonEmail, string actualEmail, int BaseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerActivity_MergeAnonActivity";
            cmd.Parameters.AddWithValue("@AnonEmail", anonEmail);
            cmd.Parameters.AddWithValue("@ActualEmail", actualEmail);
            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }


        private static ECN_Framework_Entities.DomainTracker.DomainTrackerActivity Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.DomainTracker.DomainTrackerActivity retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.DomainTracker.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.DomainTracker.DomainTrackerActivity();
                    var builder = DynamicBuilder<ECN_Framework_Entities.DomainTracker.DomainTrackerActivity>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        private static List<ECN_Framework_Entities.DomainTracker.DomainTrackerActivity> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.DomainTracker.DomainTrackerActivity> retList = new List<ECN_Framework_Entities.DomainTracker.DomainTrackerActivity>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.DomainTracker.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.DomainTracker.DomainTrackerActivity retItem = new ECN_Framework_Entities.DomainTracker.DomainTrackerActivity();
                    var builder = DynamicBuilder<ECN_Framework_Entities.DomainTracker.DomainTrackerActivity>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            retList.Add(retItem);
                        }
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        private static List<ECN_Framework_Entities.DomainTracker.FieldsValuePair> GetListFieldsValuePair(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.DomainTracker.FieldsValuePair> retList = new List<ECN_Framework_Entities.DomainTracker.FieldsValuePair>();

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.DomainTracker.ToString()))
                {
                    if (rdr != null)
                    {
                        ECN_Framework_Entities.DomainTracker.FieldsValuePair retItem = new ECN_Framework_Entities.DomainTracker.FieldsValuePair();
                        var builder = DynamicBuilder<ECN_Framework_Entities.DomainTracker.FieldsValuePair>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                retList.Add(retItem);
                            }
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
                cmd.Connection.Close();
                cmd.Dispose();
                return retList;
            }
            catch
            {
                return null;
            }
        }

        private static string plusOneDayToDate(string date)
        {
            DateTime myDate = DateTime.ParseExact(date, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            myDate = myDate.AddDays(1);
            return myDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }

	    private static SqlCommand BuildStoredProcedure(string cmdText, Dictionary<string, object> sqlParameters)
	    {
		    var sqlCmd = new SqlCommand
		    {
			    CommandType = CommandType.StoredProcedure,
			    CommandText = cmdText
		    };
		    sqlCmd.Parameters.AddRange(sqlParameters.Where(x => x.Value != null).Select(x=>new SqlParameter(x.Key, x.Value)).ToArray());
		    return sqlCmd;
	    }
    }
}
