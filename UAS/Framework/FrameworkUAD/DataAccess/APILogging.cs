using System;
using System.Data.SqlClient;
using System.Data;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class APILogging
    {
        public static int Insert(FrameworkUAD.Entity.APILogging log, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_APILogging_Insert";
            cmd.Parameters.Add(new SqlParameter("@AccessKey", log.AccessKey));
            cmd.Parameters.Add(new SqlParameter("@APIMethod", log.APIMethod));
            cmd.Parameters.Add(new SqlParameter("@Input", log.Input));

            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool UpdateLog(int APILogID, int? LogID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_APILogging_UpdateLog";
            cmd.Parameters.AddWithValue("@APILogID", APILogID);
            if (LogID != null)
                cmd.Parameters.AddWithValue("@LogID", LogID);

            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}
