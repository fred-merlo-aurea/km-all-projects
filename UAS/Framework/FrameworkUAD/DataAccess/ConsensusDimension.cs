using System;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class ConsensusDimension
    {
        public static bool SaveXML(string xml, int masterGroupID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_ConsensusDimension_SaveXML";
            cmd.Parameters.Add(new SqlParameter("@xml", xml));
            cmd.Parameters.Add(new SqlParameter("@MasterGroupID", masterGroupID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}
