using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAS.DataAccess
{
    public class Model
    {
        public static List<FrameworkUAS.Model.Rule> RulesGetRuleSet(int ruleSetId, int sourceFileId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "m_Rule_Get_ruleSetId_sourceFileId";
            cmd.Parameters.AddWithValue("@ruleSetId", ruleSetId);
            cmd.Parameters.AddWithValue("@sourceFileId", sourceFileId);
            return DataFunctions.GetList<FrameworkUAS.Model.Rule>(cmd);
        }
        public static List<FrameworkUAS.Model.Rule> RulesGetClientTemplates(int clientId, int sourceFileId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "m_Rule_Get_Templates_ClientId";
            cmd.Parameters.AddWithValue("@clientId", clientId);
            cmd.Parameters.AddWithValue("@sourceFileId", sourceFileId);
            return DataFunctions.GetList<FrameworkUAS.Model.Rule>(cmd);
        }
        public static List<FrameworkUAS.Model.Condition> ConditionsGetRuleId(int ruleId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "m_Condition_Get_ruleId";
            cmd.Parameters.AddWithValue("@ruleId", ruleId);
            return DataFunctions.GetList<FrameworkUAS.Model.Condition>(cmd);
        }
        public static List<FrameworkUAS.Model.Update> UpdatesGetRuleId(int ruleId)//RuleResult
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "m_Update_Get_ruleId";
            cmd.Parameters.AddWithValue("@ruleId", ruleId);
            return DataFunctions.GetList<FrameworkUAS.Model.Update>(cmd);
        }
    }
}
