using System;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class UserAuthorizationLog
    {
        public static int Save(Entity.UserAuthorizationLog x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserAuthorizationLog_Save";
            cmd.Parameters.Add(new SqlParameter("@UserAuthLogID", x.UserAuthLogID));
            cmd.Parameters.Add(new SqlParameter("@AuthSource", x.AuthSource));
            cmd.Parameters.Add(new SqlParameter("@AuthMode", x.AuthMode.ToString()));
            cmd.Parameters.Add(new SqlParameter("@AuthAttemptDate", x.AuthAttemptDate));
            cmd.Parameters.Add(new SqlParameter("@AuthAttemptTime", x.AuthAttemptTime));
            cmd.Parameters.Add(new SqlParameter("@IsAuthenticated", x.IsAuthenticated));
            cmd.Parameters.Add(new SqlParameter("@IpAddress", x.IpAddress));
            cmd.Parameters.Add(new SqlParameter("@AuthUserName", x.AuthUserName));
            cmd.Parameters.Add(new SqlParameter("@AuthAccessKey", (object)x.AuthAccessKey ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ServerVariables", x.ServerVariables));
            cmd.Parameters.Add(new SqlParameter("@AppVersion", x.AppVersion));
            cmd.Parameters.Add(new SqlParameter("@UserID", (object)x.UserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LogOutDate", (object)x.LogOutDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LogOutTime", (object)x.LogOutTime ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }
        public static bool LogOut(int userAuthLogId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserAuthorizationLog_LogOut";
            cmd.Parameters.Add(new SqlParameter("@userAuthLogId", userAuthLogId));

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.KMPlatform.ToString());
        }
    }
}
