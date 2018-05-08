using KMPS.MD.Objects;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MAF.NorthStarExport
{
    public class Data
    {
        public static DataTable GetBrand(string brandCode, KMPlatform.Object.ClientConnections clientconnections)
        {
            DataTable result = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnections);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_GetBrandWelcomeLetter";
            cmd.Parameters.AddWithValue("@BrandCode", brandCode);

            result = GetDataTable(cmd);
            return result;
        }
        public static DataTable GetWSR(int PubID, KMPlatform.Object.ClientConnections clientconnections)
        {
            DataTable result = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnections);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_GetWebsiteSubscriberRequest";
            cmd.Parameters.AddWithValue("@PubID", PubID);

            result = GetDataTable(cmd);
            return result;
        }
        public static DataTable GetDataTable(SqlCommand cmd)
        {
            cmd.CommandTimeout = 0;
            cmd.Connection.Open();

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dt = new DataTable();
            dt.Load(dr);
            cmd.Connection.Close();

            return dt;
        }
    }
}
