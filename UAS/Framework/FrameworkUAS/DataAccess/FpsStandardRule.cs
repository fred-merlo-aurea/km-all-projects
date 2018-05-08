using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
	public class FpsStandardRule
	{
        public static List<Entity.FpsStandardRule> Select()
        {
            List<Entity.FpsStandardRule> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FpsStandardRule_Select";
            retItem = SqlCommandExtensions.GetList<Entity.FpsStandardRule>(cmd, ConnectionString.UAS);
            return retItem;
        }
        private static Entity.FpsStandardRule Get(SqlCommand cmd)
        {
            Entity.FpsStandardRule retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.FpsStandardRule();
                        DynamicBuilder<Entity.FpsStandardRule> builder = DynamicBuilder<Entity.FpsStandardRule>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
      
        public static int Save(Entity.FpsStandardRule fpsStandardRule)
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "e_FpsStandardRule_Save";
			cmd.Parameters.AddWithValue("@FpsStandardRuleId", fpsStandardRule.FpsStandardRuleId);
			cmd.Parameters.AddWithValue("@RuleName", fpsStandardRule.RuleName);
			cmd.Parameters.AddWithValue("@DisplayName", fpsStandardRule.DisplayName);
			cmd.Parameters.AddWithValue("@Description", fpsStandardRule.Description);
			cmd.Parameters.AddWithValue("@RuleMethod", fpsStandardRule.RuleMethod);
			cmd.Parameters.AddWithValue("@ProcedureTypeId", fpsStandardRule.ProcedureTypeId);
			cmd.Parameters.AddWithValue("@ExecutionPointId", fpsStandardRule.ExecutionPointId);
			cmd.Parameters.AddWithValue("@IsActive", fpsStandardRule.IsActive);
			cmd.Parameters.AddWithValue("@DateCreated", (object)fpsStandardRule.DateCreated ?? DBNull.Value);
			cmd.Parameters.AddWithValue("@DateUpdated", (object)fpsStandardRule.DateUpdated ?? DBNull.Value);
			cmd.Parameters.AddWithValue("@CreatedByUserID", fpsStandardRule.CreatedByUserID);
			cmd.Parameters.AddWithValue("@UpdatedByUserID", fpsStandardRule.UpdatedByUserID);

			return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
		}
        public static bool Delete(int sourceFileId)
        {
            return FpsMap.Delete(sourceFileId);
        }
        public static void DeleteAll()
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = "Truncate FpsStandardRule";
			KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString());
		}
	}
}