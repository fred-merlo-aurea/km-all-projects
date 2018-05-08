using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class ApplicationLog
    {
        public static int Save(Entity.ApplicationLog appLog, BusinessLogic.Enums.Applications app, BusinessLogic.Enums.SeverityTypes severity)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ApplicationLog_Save";
            cmd.Parameters.AddWithValue("@ApplicationLogId", appLog.ApplicationLogId);
            cmd.Parameters.AddWithValue("@Application", app.ToString().Replace("_", " "));
            cmd.Parameters.AddWithValue("@Severity", severity.ToString().Replace("_"," "));
            cmd.Parameters.AddWithValue("@SourceMethod", appLog.SourceMethod);
            cmd.Parameters.AddWithValue("@Exception", appLog.Exception);
            cmd.Parameters.AddWithValue("@LogNote", appLog.LogNote);
            cmd.Parameters.AddWithValue("@IsBug", appLog.IsBug);
            cmd.Parameters.AddWithValue("@IsUserSubmitted", (object)appLog.IsUserSubmitted ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ClientId", (object)appLog.ClientId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SubmittedBy", appLog.SubmittedBy);
            cmd.Parameters.AddWithValue("@SubmittedByEmail", appLog.SubmittedByEmail);
            cmd.Parameters.AddWithValue("@IsFixed", (object)appLog.IsFixed ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FixedDate", (object)appLog.FixedDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FixedTime", (object)appLog.FixedTime ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FixedBy", appLog.FixedBy);
            cmd.Parameters.AddWithValue("@FixedNote", appLog.FixedNote);
            cmd.Parameters.AddWithValue("@LogAddedDate", appLog.LogAddedDate);
            cmd.Parameters.AddWithValue("@LogAddedTime", appLog.LogAddedTime);
            cmd.Parameters.AddWithValue("@LogUpdatedDate", (object)appLog.LogUpdatedDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LogUpdatedTime", (object)appLog.LogUpdatedTime ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@NotificationSent", (object)appLog.NotificationSent ?? DBNull.Value);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }

        public static bool UpdateNotified(int applicationLogId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update applicationlog set notificationsent = 1 where ApplicationLogId = @ApplicationLogId";
            cmd.Parameters.AddWithValue("@ApplicationLogId", applicationLogId);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static List<Entity.ApplicationLog> SelectApplication(int applicationId)
        {
            List<Entity.ApplicationLog> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ApplicationLog_Select_ApplicationId";
            cmd.Parameters.AddWithValue("@ApplicationId", applicationId);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ApplicationLog> SelectApplicationWithDateRange(int applicationId, DateTime startDate, DateTime endDate)
        {
            List<Entity.ApplicationLog> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ApplicationLog_Select_ApplicationId_DateRange";
            cmd.Parameters.AddWithValue("@ApplicationId", applicationId);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ApplicationLog> SelectWithDateRange(DateTime startDate, DateTime endDate)
        {
            List<Entity.ApplicationLog> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ApplicationLog_Select_DateRange";
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);

            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.ApplicationLog Get(SqlCommand cmd)
        {
            Entity.ApplicationLog retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ApplicationLog();
                        var builder = DynamicBuilder<Entity.ApplicationLog>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
        private static List<Entity.ApplicationLog> GetList(SqlCommand cmd)
        {
            List<Entity.ApplicationLog> retList = new List<Entity.ApplicationLog>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ApplicationLog retItem = new Entity.ApplicationLog();
                        var builder = DynamicBuilder<Entity.ApplicationLog>.CreateBuilder(rdr);
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
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
    }
}
