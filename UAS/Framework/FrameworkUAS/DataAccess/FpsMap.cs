using System;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
	public class FpsMap
	{
		public static int Save(Entity.FpsMap fpsMap)
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "e_FpsMap_Save";
			cmd.Parameters.AddWithValue("@FpsMapId", fpsMap.FpsMapId);
            cmd.Parameters.AddWithValue("@ClientId", fpsMap.ClientId);
            cmd.Parameters.AddWithValue("@SourceFileId", fpsMap.SourceFileId);
            cmd.Parameters.AddWithValue("@IsStandardRule", fpsMap.IsStandardRule);
			cmd.Parameters.AddWithValue("@FpsStandardRuleId", fpsMap.FpsStandardRuleId);
			cmd.Parameters.AddWithValue("@FpsCustomRuleId", fpsMap.FpsCustomRuleId);
			cmd.Parameters.AddWithValue("@RuleOrder", fpsMap.RuleOrder);
			cmd.Parameters.AddWithValue("@IsActive", fpsMap.IsActive);
			cmd.Parameters.AddWithValue("@DateCreated", (object)fpsMap.DateCreated ?? DBNull.Value);
			cmd.Parameters.AddWithValue("@DateUpdated", (object)fpsMap.DateUpdated ?? DBNull.Value);
			cmd.Parameters.AddWithValue("@CreatedByUserId", fpsMap.CreatedByUserId);
			cmd.Parameters.AddWithValue("@UpdatedByUserId", fpsMap.UpdatedByUserId);

			return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
		}
        public static bool Delete(int sourceFileId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FpsMap_Delete_SourceFileId";
            cmd.Parameters.Add(new SqlParameter("@sourceFileId", sourceFileId));

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static void DeleteAll()
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = "Truncate FpsMap";
			KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString());
		}
	}
}