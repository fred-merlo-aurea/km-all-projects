using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class RuleSetRuleOrder
    {
        public static bool DeleteRule(int ruleSetId, int ruleId, int userId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_Rule_Delete";
            cmd.Parameters.AddWithValue("@ruleSetId", ruleSetId);
            cmd.Parameters.AddWithValue("@ruleId", ruleId);
            cmd.Parameters.AddWithValue("@userId", userId);
            int count = Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
            bool done = false;
            if (count > 0) done = true;
            return done;
        }
        public static bool UpdateExecutionOrder(int ruleSetId, int ruleId, int executionOrder)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleSetRuleOrder_UpdateExecutionOrder";
            cmd.Parameters.AddWithValue("@RuleSetId", ruleSetId);
            cmd.Parameters.AddWithValue("@RuleId", ruleId);
            cmd.Parameters.AddWithValue("@ExecutionOrder", executionOrder);
            int count = Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
            bool done = false;
            if (count > 0) done = true;
            return done;
        }
        public static bool Save(Entity.RuleSetRuleOrder x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleSetRuleOrder_Save";
            cmd.Parameters.AddWithValue("@RuleSetId", x.RuleSetId);
            cmd.Parameters.AddWithValue("@RuleId", x.RuleId);
            cmd.Parameters.AddWithValue("@ExecutionOrder", x.ExecutionOrder);
            cmd.Parameters.AddWithValue("@RuleScript", x.RuleScript);
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@DateUpdated", x.DateUpdated);
            cmd.Parameters.AddWithValue("@CreatedByUserId", x.CreatedByUserId);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", x.UpdatedByUserId);

            int count = Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
            bool done = false;
            if (count > 0) done = true;
            return done;
        }
        public static int GetExecutionOrder(int ruleSetId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select count(RuleSetId) + 1 from RuleSetRuleOrder with(nolock) where RuleSetId = " + ruleSetId.ToString();
            int count = Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
            return count;
        }
        public static int GetRuleCount(int ruleSetId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select count(RuleSetId) from RuleSetRuleOrder with(nolock) where RuleSetId = " + ruleSetId.ToString();
            int count = Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
            return count;
        }
    }
}
