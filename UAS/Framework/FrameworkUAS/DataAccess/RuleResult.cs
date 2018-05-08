using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class RuleResult
    {
        public static int Save(Entity.RuleResult x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleResult_Save";
            cmd.Parameters.AddWithValue("@RuleResultId", x.RuleResultId);
            cmd.Parameters.AddWithValue("@RuleId", x.RuleId);
            cmd.Parameters.AddWithValue("@RuleFieldId", x.RuleFieldId);
            cmd.Parameters.AddWithValue("@UpdateField", x.UpdateField);
            cmd.Parameters.AddWithValue("@UpdateFieldPrefix", x.UpdateFieldPrefix);
            cmd.Parameters.AddWithValue("@UpdateFieldValue", x.UpdateFieldValue);
            cmd.Parameters.AddWithValue("@IsClientField", x.IsClientField);
            cmd.Parameters.AddWithValue("@IsActive", x.IsActive);
            cmd.Parameters.AddWithValue("@CreatedDate", x.CreatedDate);
            cmd.Parameters.AddWithValue("@CreatedByUserId", x.CreatedByUserId);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", x.UpdatedByUserId);

            int ruleResultId = Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
            return ruleResultId;
        }
    }
}
