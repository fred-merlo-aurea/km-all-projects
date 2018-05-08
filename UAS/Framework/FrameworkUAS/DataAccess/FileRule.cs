using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAS.DataAccess
{
    public class FileRule
    {
        public static List<Entity.FileRule> Select()
        {
            List<Entity.FileRule> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileRule_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.FileRule Get(SqlCommand cmd)
        {
            Entity.FileRule retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.FileRule();
                        DynamicBuilder<Entity.FileRule> builder = DynamicBuilder<Entity.FileRule>.CreateBuilder(rdr);
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
        private static List<Entity.FileRule> GetList(SqlCommand cmd)
        {
            List<Entity.FileRule> retList = new List<Entity.FileRule>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.FileRule retItem = new Entity.FileRule();
                        DynamicBuilder<Entity.FileRule> builder = DynamicBuilder<Entity.FileRule>.CreateBuilder(rdr);
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
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }

        public static int Save(Entity.FileRule x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileRule_Save";
            cmd.Parameters.Add(new SqlParameter("@FileRuleId", x.FileRuleID));
            cmd.Parameters.Add(new SqlParameter("@RuleName", x.RuleName));
            cmd.Parameters.Add(new SqlParameter("@DisplayName", x.DisplayName));
            cmd.Parameters.Add(new SqlParameter("@Description", x.Description));
            cmd.Parameters.Add(new SqlParameter("@RuleMethod", x.RuleMethod));
            cmd.Parameters.Add(new SqlParameter("@ProcedureTypeId", x.ProcedureTypeId));
            cmd.Parameters.Add(new SqlParameter("@ExecutionPointId", x.ExecutionPointId));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.UAS.ToString()));
        }
    }
}
