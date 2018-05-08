using System;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkSubGen.DataAccess
{
    public class DataAccessBase
    {
        public static bool SaveBulkXml(string xml, string commandText, string className)
        {
            var success = false;
            try
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = commandText;

                    cmd.Parameters.AddWithValue("@xml", xml);
                    cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());

                    success = KM.Common.DataFunctions.ExecuteNonQuery(cmd);
                }
            }
            catch (Exception ex) when (
                ex is InvalidCastException
                || ex is SqlException
                || ex is InvalidOperationException
                || ex is System.IO.IOException)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, className, "SaveBulkXml");
            }

            return success;
        }
    }
}
