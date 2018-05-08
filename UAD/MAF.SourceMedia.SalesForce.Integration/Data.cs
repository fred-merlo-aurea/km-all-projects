using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MAF.SourceMedia.SalesForce.Integration
{
    public class Data
    {
        public static DataTable GetSalesForceIntegrationData(int pubID)
        {
            DataTable result = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SourceMediaMasterDB"].ConnectionString);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_SourceMedia_SalesForceIntegrationdata";
            cmd.Parameters.AddWithValue("@PubID", pubID);

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
