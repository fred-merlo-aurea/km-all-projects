using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]
    public class Notification
    {
        #region Data

        public static bool ExistsByName(string NotificationName, int NotificationID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 NotificationID FROM Notification WITH (NOLOCK) " +
                              "WHERE NotificationID <> @NotificationID and NotificationName = @NotificationName and IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@NotificationID", NotificationID);
            cmd.Parameters.AddWithValue("@NotificationName", NotificationName);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static bool ExistsByTime(int NotificationID, string startDate, string startTime, string EndDate, string EndTime)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Notification_Exists_ByTime";
            cmd.Parameters.AddWithValue("@NotificationID", NotificationID);
            cmd.Parameters.Add(new SqlParameter("@StartDate", startDate));
            cmd.Parameters.Add(new SqlParameter("@StartTime", startTime));
            cmd.Parameters.Add(new SqlParameter("@EndDate", EndDate));
            cmd.Parameters.Add(new SqlParameter("@EndTime", EndTime));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Accounts.Notification> GetAll()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Notification_Select_All";
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Accounts.Notification GetByNotificationID(int NotificationID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Notification_Select_NotificationID";
            cmd.Parameters.AddWithValue("@NotificationID", NotificationID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Accounts.Notification GetByCurrentDateTime()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Notification_Select_CurrentDate_CurrentTime";
            return Get(cmd);
        }

        private static List<ECN_Framework_Entities.Accounts.Notification> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.Notification> retList = new List<ECN_Framework_Entities.Accounts.Notification>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.Notification retItem = new ECN_Framework_Entities.Accounts.Notification();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.Notification>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Accounts.Notification Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.Notification retItem = null;

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
                {
                    if (rdr != null && rdr.HasRows)
                    {
                        try
                        {
                            retItem = new ECN_Framework_Entities.Accounts.Notification();
                            var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.Notification>.CreateBuilder(rdr);
                            while (rdr.Read())
                            {
                                retItem = builder.Build(rdr);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            if (rdr != null)
                            {
                                rdr.Close();
                                rdr.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }


            return retItem;
        }
        #endregion
        #region CRUD

        public static void Delete(int NotificationID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Notification_Delete";
            cmd.Parameters.AddWithValue("@NotificationID", NotificationID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }

        public static int Save(ECN_Framework_Entities.Accounts.Notification notification)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Notification_Save";
            cmd.Parameters.Add(new SqlParameter("@NotificationID", notification.NotificationID));
            cmd.Parameters.Add(new SqlParameter("@NotificationName", notification.NotificationName));
            cmd.Parameters.Add(new SqlParameter("@NotificationText", notification.NotificationText));
            cmd.Parameters.Add(new SqlParameter("@StartDate", notification.StartDate));
            cmd.Parameters.Add(new SqlParameter("@StartTime", notification.StartTime));
            cmd.Parameters.Add(new SqlParameter("@EndDate", notification.EndDate));
            cmd.Parameters.Add(new SqlParameter("@EndTime", notification.EndTime));

            cmd.Parameters.Add(new SqlParameter("@BackGroundColor", notification.BackGroundColor));
            cmd.Parameters.Add(new SqlParameter("@CloseButtonColor", notification.CloseButtonColor));

            if (notification.NotificationID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)notification.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)notification.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
        }
        #endregion
    }
}
