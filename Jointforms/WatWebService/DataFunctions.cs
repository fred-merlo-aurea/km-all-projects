using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace WattWebService
{
    public class DataFunctions
    {
        #region SQL EXECUTE, EXECUTE SCLAR
        public static int execute(string sql, SqlConnection conn)
        {

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Connection.Open();
            int success = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return success;
        }

        public static int execute(SqlCommand cmd, SqlConnection conn)
        {

            cmd.Connection = conn;
            conn.Open();
            int success = cmd.ExecuteNonQuery();
            conn.Close();
            return success;
        }

        #endregion
    }
}