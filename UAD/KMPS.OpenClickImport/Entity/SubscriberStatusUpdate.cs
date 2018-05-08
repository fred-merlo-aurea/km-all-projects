using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMPS.ActivityImport.Entity
{
    public class SubscriberStatusUpdate
    {
        public static void Import(string xml, string sql)
        {
            SqlConnection conn = new SqlConnection(sql);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_ImportSubscriberStatusUpdate_XML";
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@Xml", xml);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
    }
}
