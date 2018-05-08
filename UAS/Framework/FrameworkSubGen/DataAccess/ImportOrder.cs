using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM;
using KM.Common.Data;

namespace FrameworkSubGen.DataAccess
{
    public class ImportOrder
    {
        public static bool SaveBulkXml(string xml)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ImportOrder_SaveBulkXml";
            cmd.Parameters.AddWithValue("@xml", xml);
            cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}
