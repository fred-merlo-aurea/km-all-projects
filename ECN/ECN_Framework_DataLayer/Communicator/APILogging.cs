using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]    
    public class APILogging
    {
        public static int Insert(ECN_Framework_Entities.Communicator.APILogging log)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_APILogging_Insert";
            cmd.Parameters.Add(new SqlParameter("@AccessKey", log.AccessKey));
            cmd.Parameters.Add(new SqlParameter("@APIMethod", log.APIMethod));
            cmd.Parameters.Add(new SqlParameter("@Input", log.Input));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void UpdateLog(int APILogID, int? LogID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_APILogging_UpdateLog";
            cmd.Parameters.AddWithValue("@APILogID", APILogID);
            if(LogID != null)
                cmd.Parameters.AddWithValue("@LogID", LogID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
