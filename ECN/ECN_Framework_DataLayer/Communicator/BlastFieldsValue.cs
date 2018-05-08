using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class BlastFieldsValue
    {
        public static DataTable GetByBlastFieldID(int BlastFieldID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastFieldsValue_Select";
            cmd.Parameters.AddWithValue("@BlastFieldID", BlastFieldID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.BlastFieldsValue> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.BlastFieldsValue> retList = new List<ECN_Framework_Entities.Communicator.BlastFieldsValue>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.BlastFieldsValue retItem = new ECN_Framework_Entities.Communicator.BlastFieldsValue();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastFieldsValue>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            retList.Add(retItem);
                        }
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        private static ECN_Framework_Entities.Communicator.BlastFieldsValue Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.BlastFieldsValue retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.BlastFieldsValue();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastFieldsValue>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        public static int Save(ECN_Framework_Entities.Communicator.BlastFieldsValue blastFieldsValue)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastFieldsValue_Save";
            cmd.Parameters.Add(new SqlParameter("@BlastFieldID", blastFieldsValue.BlastFieldID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", blastFieldsValue.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@Value", blastFieldsValue.Value));
            cmd.Parameters.Add(new SqlParameter("@UserID", blastFieldsValue.CreatedUserID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int BlastFieldsValueID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastFieldsValue_Delete";
            cmd.Parameters.AddWithValue("@BlastFieldsValueID", BlastFieldsValueID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}