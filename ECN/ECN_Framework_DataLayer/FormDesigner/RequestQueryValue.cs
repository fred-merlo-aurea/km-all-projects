using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.FormDesigner
{
    [Serializable]
    public class RequestQueryValue
    {
        public static List<ECN_Framework_Entities.FormDesigner.RequestQueryValue> GetByRuleID(int ruleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RequestQueryValue_Select_RuleID";
            cmd.Parameters.AddWithValue("@RuleID", ruleID);

            return GetList(cmd);
        }

        public static int Save(ECN_Framework_Entities.FormDesigner.RequestQueryValue rqv)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RequestQueryValue_Save";

            cmd.Parameters.AddWithValue("@Control_ID", rqv.Control_ID);
            cmd.Parameters.AddWithValue("@Rule_Seq_ID", rqv.Rule_Seq_ID);
            cmd.Parameters.AddWithValue("@Name", rqv.Name);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.FormDesigner.ToString()).ToString());
        }

        public static void DeleteByRuleID(int ruleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RequestQueryValue_Delete_RuleID";
            cmd.Parameters.AddWithValue("@Rule_Seq_ID", ruleID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.FormDesigner.ToString());
        }

        private static List<ECN_Framework_Entities.FormDesigner.RequestQueryValue> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.FormDesigner.RequestQueryValue> retList = new List<ECN_Framework_Entities.FormDesigner.RequestQueryValue>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.FormDesigner.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.FormDesigner.RequestQueryValue retItem = new ECN_Framework_Entities.FormDesigner.RequestQueryValue();
                    var builder = DynamicBuilder<ECN_Framework_Entities.FormDesigner.RequestQueryValue>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.FormDesigner.RequestQueryValue Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.FormDesigner.RequestQueryValue retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.FormDesigner.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.FormDesigner.RequestQueryValue();
                    var builder = DynamicBuilder<ECN_Framework_Entities.FormDesigner.RequestQueryValue>.CreateBuilder(rdr);
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
    }
}
