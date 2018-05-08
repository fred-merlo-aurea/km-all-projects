using System;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class SubGen
    {
        public static bool SubGen_Subscriber_Update(string xml, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_SubGen_Subscriber_Update";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@xml", xml);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool SubGen_Address_Update(string xml, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_SubGen_Address_Update";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@xml", xml);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}
