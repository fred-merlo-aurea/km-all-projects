using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace KMPS.ActivityImport.Entity
{
    public class TopicActivity
    {
        public TopicActivity() { }

        public static void Import(string xml, string sql)
        {
            SqlConnection conn = new SqlConnection(sql);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_ImportTopics_XML";
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@Xml", xml);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
    }
}
