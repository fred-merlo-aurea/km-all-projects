using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class RuleCondition
    {
        public static List<Entity.RuleCondition> Select(int ruleId)
        {
            List<Entity.RuleCondition> retList = new List<Entity.RuleCondition>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleCondition_Select_RuleId";
            cmd.Parameters.AddWithValue("@ruleId", ruleId);
            retList = GetList(cmd);
            return retList;
        }
        private static Entity.RuleCondition Get(SqlCommand cmd)
        {
            Entity.RuleCondition retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.RuleCondition();
                        DynamicBuilder<Entity.RuleCondition> builder = DynamicBuilder<Entity.RuleCondition>.CreateBuilder(rdr);
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
        private static List<Entity.RuleCondition> GetList(SqlCommand cmd)
        {
            List<Entity.RuleCondition> retList = new List<Entity.RuleCondition>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.RuleCondition retItem = new Entity.RuleCondition();
                        DynamicBuilder<Entity.RuleCondition> builder = DynamicBuilder<Entity.RuleCondition>.CreateBuilder(rdr);
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
        public static bool Save(Entity.RuleCondition x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleCondition_Save";
            cmd.Parameters.AddWithValue("@RuleId", x.RuleId);
            cmd.Parameters.AddWithValue("@Line", x.Line);
            cmd.Parameters.AddWithValue("@IsGrouped ", x.IsGrouped);
            cmd.Parameters.AddWithValue("@GroupNumber", x.GroupNumber);
            cmd.Parameters.AddWithValue("@ChainId", x.ChainId);
            cmd.Parameters.AddWithValue("@CompareField", x.CompareField);
            cmd.Parameters.AddWithValue("@CompareFieldPrefix", x.CompareFieldPrefix);
            cmd.Parameters.AddWithValue("@IsClientField", x.IsClientField);
            cmd.Parameters.AddWithValue("@OperatorId", x.OperatorId);
            cmd.Parameters.AddWithValue("@CompareValue", x.CompareValue);
            cmd.Parameters.AddWithValue("@IsActive", x.IsActive);
            cmd.Parameters.AddWithValue("@CreatedDate", x.CreatedDate);
            cmd.Parameters.AddWithValue("@CreatedByUserId", x.CreatedByUserId);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", x.UpdatedByUserId);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool Delete(int ruleId, int lineNumber)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleCondition_Delete";
            cmd.Parameters.AddWithValue("@ruleId", ruleId);
            cmd.Parameters.AddWithValue("@lineNumber", lineNumber);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
    }
}
