using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
	public class FpsCustomRule
	{
        public static List<Entity.FpsCustomRule> SelectClientId(int clientId)
        {
            List<Entity.FpsCustomRule> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FpsCustomRule_Select_ClientId";
            retItem = SqlCommandExtensions.GetList<Entity.FpsCustomRule>(cmd, ConnectionString.UAS);
            return retItem;
        }
        public static List<Entity.FpsCustomRule> SelectSourceFileId(int sourceFileId)
        {
            List<Entity.FpsCustomRule> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FpsCustomRule_Select_SourceFileId";
            retItem = SqlCommandExtensions.GetList<Entity.FpsCustomRule>(cmd, ConnectionString.UAS);
            return retItem;
        }
        public static int Save(Entity.FpsCustomRule fpsCustomRule)
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "e_FpsCustomRule_Save";
			cmd.Parameters.AddWithValue("@FpsCustomRuleId", fpsCustomRule.FpsCustomRuleId);
			cmd.Parameters.AddWithValue("@ClientId", fpsCustomRule.ClientId);
            cmd.Parameters.AddWithValue("@RuleName", fpsCustomRule.RuleName);
            cmd.Parameters.AddWithValue("@DisplayName", fpsCustomRule.DisplayName);
            cmd.Parameters.AddWithValue("@Description", fpsCustomRule.Description);
            cmd.Parameters.AddWithValue("@ExecutionPointId", fpsCustomRule.ExecutionPointId);
            cmd.Parameters.AddWithValue("@Line", fpsCustomRule.Line);
			cmd.Parameters.AddWithValue("@IsGrouped", fpsCustomRule.IsGrouped);
			cmd.Parameters.AddWithValue("@GroupNumber", fpsCustomRule.GroupNumber);
			cmd.Parameters.AddWithValue("@Link", fpsCustomRule.Link);
			cmd.Parameters.AddWithValue("@IncomingField", fpsCustomRule.IncomingField);
			cmd.Parameters.AddWithValue("@IsProfileField", fpsCustomRule.IsProfileField);
			cmd.Parameters.AddWithValue("@Operator", fpsCustomRule.Operator);
			cmd.Parameters.AddWithValue("@ConditionValue", fpsCustomRule.ConditionValue);
			cmd.Parameters.AddWithValue("@Clause", fpsCustomRule.Clause);
			cmd.Parameters.AddWithValue("@ProductField", fpsCustomRule.ProductField);
			cmd.Parameters.AddWithValue("@ProductValue", fpsCustomRule.ProductValue);
			cmd.Parameters.AddWithValue("@IsActive", fpsCustomRule.IsActive);
            cmd.Parameters.AddWithValue("@IsGlobalRule", fpsCustomRule.IsGlobalRule);
            cmd.Parameters.AddWithValue("@StartDate", (object)fpsCustomRule.StartDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", (object) fpsCustomRule.EndDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DateCreated", fpsCustomRule.DateCreated);
			cmd.Parameters.AddWithValue("@DateUpdated", (object)fpsCustomRule.DateUpdated ?? DBNull.Value);
			cmd.Parameters.AddWithValue("@CreatedByUserId", fpsCustomRule.CreatedByUserId);
			cmd.Parameters.AddWithValue("@UpdatedByUserId", fpsCustomRule.UpdatedByUserId);

			return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
		}
        public static void DeleteAll()
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = "Truncate FpsCustomRule";
			KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString());
		}
	}
}