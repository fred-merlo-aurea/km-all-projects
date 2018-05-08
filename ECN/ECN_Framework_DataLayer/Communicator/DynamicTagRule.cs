using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    [DataContract]
    public class DynamicTagRule
    {
        public static List<ECN_Framework_Entities.Communicator.DynamicTagRule> GetByDynamicTagID(int DynamicTagID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DynamicTagRule_Select_DynamicTagID";
            cmd.Parameters.AddWithValue("@DynamicTagID", DynamicTagID);
            return GetList(cmd);
        }

        public static int Save(ECN_Framework_Entities.Communicator.DynamicTagRule DynamicTagRule)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DynamicTagRule_Save";
            cmd.Parameters.Add(new SqlParameter("@DynamicTagRuleID", (object)DynamicTagRule.DynamicTagRuleID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DynamicTagID", DynamicTagRule.DynamicTagID));
            cmd.Parameters.Add(new SqlParameter("@RuleID", DynamicTagRule.RuleID));
            cmd.Parameters.Add(new SqlParameter("@ContentID", DynamicTagRule.ContentID));
            cmd.Parameters.Add(new SqlParameter("@Priority", DynamicTagRule.Priority));
            if (DynamicTagRule.DynamicTagRuleID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)DynamicTagRule.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)DynamicTagRule.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void DeleteByDynamicTagID(int DynamicTagID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DynamicTagRule_DeleteByDynamicTagID";
            cmd.Parameters.AddWithValue("@DynamicTagID", DynamicTagID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int DynamicTagRulesID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DynamicTagRule_Delete";
            cmd.Parameters.AddWithValue("@DynamicTagRuleID", DynamicTagRulesID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static ECN_Framework_Entities.Communicator.DynamicTagRule Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.DynamicTagRule retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.DynamicTagRule();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.DynamicTagRule>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.DynamicTagRule> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.DynamicTagRule> retList = new List<ECN_Framework_Entities.Communicator.DynamicTagRule>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.DynamicTagRule retItem = new ECN_Framework_Entities.Communicator.DynamicTagRule();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.DynamicTagRule>.CreateBuilder(rdr);
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
    }
}
