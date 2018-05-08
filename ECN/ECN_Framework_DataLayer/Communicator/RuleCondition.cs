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
    public class RuleCondition
    {
        public static int Save(ECN_Framework_Entities.Communicator.RuleCondition RuleCondition)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleCondition_Save";
            cmd.Parameters.Add(new SqlParameter("@RuleConditionID", (object)RuleCondition.RuleConditionID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Field", RuleCondition.Field));
            cmd.Parameters.Add(new SqlParameter("@DataType", RuleCondition.DataType));
            cmd.Parameters.Add(new SqlParameter("@Comparator", RuleCondition.Comparator));
            cmd.Parameters.Add(new SqlParameter("@Value", RuleCondition.Value));
            cmd.Parameters.Add(new SqlParameter("@RuleID", RuleCondition.RuleID));
            if (RuleCondition.RuleConditionID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)RuleCondition.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)RuleCondition.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int RuleConditionsID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleCondition_Delete";
            cmd.Parameters.AddWithValue("@RuleConditionID", RuleConditionsID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void DeleteByRuleID(int RuleID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleCondition_DeleteByRuleID";
            cmd.Parameters.AddWithValue("@RuleID", RuleID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.RuleCondition> GetByRuleID(int RuleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleCondition_Select_RuleID";
            cmd.Parameters.AddWithValue("@RuleID", RuleID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Communicator.RuleCondition Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.RuleCondition retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.RuleCondition();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.RuleCondition>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.RuleCondition> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.RuleCondition> retList = new List<ECN_Framework_Entities.Communicator.RuleCondition>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.RuleCondition retItem = new ECN_Framework_Entities.Communicator.RuleCondition();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.RuleCondition>.CreateBuilder(rdr);
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
