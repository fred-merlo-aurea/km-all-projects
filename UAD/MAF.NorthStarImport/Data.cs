using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAF.NorthStarImport
{
    public class Data
    {

        public static bool ImportUnsubscribes(string xml, string sqlConn)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.CommandText = "job_Subscriptions_ImportUnsubscribe";
            cmd.Parameters.AddWithValue("@Xml", xml);
            cmd.Connection.Open();
            result = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
            cmd.Dispose();

            if (result == 0)
                return false;
            else
                return true;
        }
        public static bool ImportSubscribes(string xml, string sqlConn)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = new SqlConnection(sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_Subscriptions_ImportSubscribe";
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@Xml", xml);
            cmd.Connection.Open();
            result = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
            cmd.Dispose();

            if (result == 0)
                return false;
            else
                return true;
        }
        public static bool UpdateEmailStatus(string xml, string emailstatus, string sqlConn)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.CommandText = "job_UpdateEmailStatus";
            cmd.Parameters.AddWithValue("@Xml", xml);
            cmd.Parameters.AddWithValue("@status", emailstatus);
            cmd.Connection.Open();
            result = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
            cmd.Dispose();

            if (result == 0)
                return false;
            else
                return true;
        }

    }
}
