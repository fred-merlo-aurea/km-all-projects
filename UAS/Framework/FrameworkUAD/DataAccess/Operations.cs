using System;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class Operations
    {
        public static bool RemovePubCode(KMPlatform.Object.ClientConnections client, string pubCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_ADMS_Remove_By_PubCode";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@PubCode", pubCode);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool RemoveProcessCode(KMPlatform.Object.ClientConnections client, string processCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_ADMS_Remove_By_ProcessCode";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool QSourceValidation(KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_QSourceValidation";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool FileValidator_QSourceValidation(KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_FileValidator_QSourceValidation";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}
