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
    public class Rule
    {
        public static int Save(ECN_Framework_Entities.Communicator.Rule Rule)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Rule_Save";
            cmd.Parameters.Add(new SqlParameter("@RuleID", (object)Rule.RuleID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ConditionConnector", Rule.ConditionConnector));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", Rule.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@WhereClause", Rule.WhereClause));
            cmd.Parameters.Add(new SqlParameter("@RuleName", Rule.RuleName));
            if (Rule.RuleID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)Rule.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)Rule.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int RuleID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Rule_Delete";
            cmd.Parameters.AddWithValue("@RuleID", RuleID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static ECN_Framework_Entities.Communicator.Rule GetByRuleID(int RuleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Rule_Select_RuleID";
            cmd.Parameters.AddWithValue("@RuleID", RuleID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.Rule> GetByCustomerID(int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Rule_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Communicator.Rule Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.Rule retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.Rule();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Rule>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.Rule> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.Rule> retList = new List<ECN_Framework_Entities.Communicator.Rule>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.Rule retItem = new ECN_Framework_Entities.Communicator.Rule();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Rule>.CreateBuilder(rdr);
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

        public static bool Exists(string RuleName, int CustomerID, int RuleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 RuleID FROM [Rule]  WITH (NOLOCK) WHERE RuleName=@RuleName and CustomerID = @CustomerID and IsDeleted = 0 and RuleID<>@RuleID) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@RuleID", RuleID);
            cmd.Parameters.AddWithValue("@RuleName", RuleName);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool IsUsedInDynamicTag(int RuleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 DynamicTagRuleID FROM [DynamicTagRule]  WITH (NOLOCK) WHERE [RuleID]=@RuleID and IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@RuleID", RuleID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool IsApplicable(int RuleID, int EmailID, int GroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Rule_ExistsByEmailID";
            cmd.Parameters.AddWithValue("@RuleID", RuleID);
            cmd.Parameters.AddWithValue("@EmailID", EmailID);
            cmd.Parameters.AddWithValue("@GroupID", GroupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }
    }
}
