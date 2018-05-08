using System;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class CodeSheetMasterCodeSheetBridge
    {
        public static int Save(Entity.CodeSheetMasterCodeSheetBridge x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CodeSheetMasterCodeSheetBridge_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@CodeSheetID", x.CodeSheetID);
            cmd.Parameters.AddWithValue("@MasterID", x.MasterID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool DeleteCodeSheetID(KMPlatform.Object.ClientConnections client, int codeSheetID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CodeSheetMasterCodeSheetBridge_Delete_CodeSheetID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@CodeSheetID", codeSheetID);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool DeleteMasterID(KMPlatform.Object.ClientConnections client, int masterID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CodeSheetMasterCodeSheetBridge_Delete_MasterID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@MasterID", masterID);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}
