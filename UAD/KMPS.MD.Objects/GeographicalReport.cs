using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;

namespace KMPS.MD.Objects
{
    public class GeographicalReport
    {
        #region Data
        public static DataTable GetGeographicalReportData(KMPlatform.Object.ClientConnections clientconnection, string procname, StringBuilder Queries)
        {
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand(procname, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Queries", Queries.ToString());
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static DataTable GetProductGeographicalReportData(KMPlatform.Object.ClientConnections clientconnection, string procname, StringBuilder Queries, int PubID)
        {
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand(procname, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Queries", Queries.ToString());
            cmd.Parameters.AddWithValue("@PubID", PubID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
    }
}
