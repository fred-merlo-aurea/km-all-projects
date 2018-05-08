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
    public class DomainTracker
    {
        public static int Save(ECN_Framework_Entities.DomainTracker.DomainTracker item)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTracker_Save";
            cmd.Parameters.Add(new SqlParameter("@Domain", item.Domain));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", item.BaseChannelID));
            cmd.Parameters.Add(new SqlParameter("@TrackerKey", item.TrackerKey));
            cmd.Parameters.Add(new SqlParameter("@UserID", item.CreatedUserID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.DomainTracker.ToString()));
        }

        public static bool Exists(string domainName, int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " if exists (select top 1 dt.DomainTrackerID FROM DomainTracker dt WITH (NOLOCK) " +
                              " WHERE dt.BaseChannelID = @BaseChannelID and dt.Domain = @domainName and dt.IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@DomainName", domainName);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.DomainTracker.ToString())) > 0 ? true : false;
        }

        public static void Delete(int domainTrackerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTracker_Delete";
            cmd.Parameters.AddWithValue("@DomainTrackerID", domainTrackerID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static ECN_Framework_Entities.DomainTracker.DomainTracker GetByDomainTrackerID(int domainTrackerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTracker_Select_DomainTrackerID";
            cmd.Parameters.AddWithValue("@DomainTrackerID", domainTrackerID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.DomainTracker.DomainTracker> GetByBaseChannelID(int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTracker_Select_BaseChannelID";
            cmd.Parameters.AddWithValue("@baseChannelID", baseChannelID);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.DomainTracker.DomainTracker GetByTrackerKey(string trackerKey)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTracker_Select_TrackerKey";
            cmd.Parameters.AddWithValue("@TrackerKey", trackerKey);
            return Get(cmd);
        }

        public static DataTable DomainTrackerFields_User_Export(int domainTrackerID, string StartDate, string EndDate, string email = null, string TypeFilter = "known")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerFields_User_Export";
            cmd.Parameters.AddWithValue("@DomainTrackerID", domainTrackerID);
            cmd.Parameters.AddWithValue("@EmailAddress", email);
            // Data range
            if (!String.IsNullOrEmpty(StartDate))
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
            else
                cmd.Parameters.AddWithValue("@StartDate", "01/01/1885");
            if (!String.IsNullOrEmpty(EndDate))
                cmd.Parameters.AddWithValue("@EndDate", plusOneDayToDate(EndDate));
            else
                cmd.Parameters.AddWithValue("@EndDate", "01/01/2115");

            cmd.Parameters.AddWithValue("@TypeFilter", TypeFilter);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        private static ECN_Framework_Entities.DomainTracker.DomainTracker Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.DomainTracker.DomainTracker retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.DomainTracker.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.DomainTracker.DomainTracker();
                    var builder = DynamicBuilder<ECN_Framework_Entities.DomainTracker.DomainTracker>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.DomainTracker.DomainTracker> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.DomainTracker.DomainTracker> retList = new List<ECN_Framework_Entities.DomainTracker.DomainTracker>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.DomainTracker.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.DomainTracker.DomainTracker retItem = new ECN_Framework_Entities.DomainTracker.DomainTracker();
                    var builder = DynamicBuilder<ECN_Framework_Entities.DomainTracker.DomainTracker>.CreateBuilder(rdr);
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

        private static string plusOneDayToDate(string date)
        {
            //DateTime myDate = DateTime.ParseExact(date, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime myDate = DateTime.Parse(date, System.Globalization.CultureInfo.InvariantCulture);
            myDate = myDate.AddDays(1);
            return myDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
