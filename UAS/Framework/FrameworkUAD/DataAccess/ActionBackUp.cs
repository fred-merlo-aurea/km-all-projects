using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FrameworkUAD.DataAccess
{
    public class ActionBackUp
    {
        public static bool Restore(int productID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ActionBackUp_Restore";
            cmd.Parameters.Add(new SqlParameter("@ProductID", productID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool Bulk_Insert(int productID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ActionBackUp_Bulk_Insert";
            cmd.Parameters.Add(new SqlParameter("@ProductID", productID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}
