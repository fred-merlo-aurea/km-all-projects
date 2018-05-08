using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class RuleValue
    {

        private static Entity.RuleValue Get(SqlCommand cmd)
        {
            Entity.RuleValue retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.RuleValue();
                        DynamicBuilder<Entity.RuleValue> builder = DynamicBuilder<Entity.RuleValue>.CreateBuilder(rdr);
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
        private static List<Entity.RuleValue> GetList(SqlCommand cmd)
        {
            List<Entity.RuleValue> retList = new List<Entity.RuleValue>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.RuleValue retItem = new Entity.RuleValue();
                        DynamicBuilder<Entity.RuleValue> builder = DynamicBuilder<Entity.RuleValue>.CreateBuilder(rdr);
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
        public static List<Entity.RuleValue> SelectIsActive(bool isActive = true)
        {
            List<Entity.RuleValue> retList = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleValue_Select_IsActive";
            cmd.Parameters.AddWithValue("@isActive", isActive);

            retList = GetList(cmd);
            return retList;
        }
        public static List<Entity.RuleValue> GetRuleValuesForRule(int ruleId, bool isActive = true)
        {
            List<Entity.RuleValue> retList = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleValue_Select_RuleId_IsActive";
            cmd.Parameters.AddWithValue("@ruleId", ruleId);
            cmd.Parameters.AddWithValue("@isActive", isActive);

            retList = GetList(cmd);
            return retList;
        }
        public static List<Entity.RuleValue> GetRuleValuesForRuleValue(int ruleValueId, bool isActive = true)
        {
            List<Entity.RuleValue> retList = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleValue_Select_RuleValueId_IsActive";
            cmd.Parameters.AddWithValue("@ruleValueId", ruleValueId);
            cmd.Parameters.AddWithValue("@isActive", isActive);

            retList = GetList(cmd);
            return retList;
        }
        public static List<Entity.RuleValue> GetRuleValuesForSourceFile(int sourceFileId, bool isActive = true)
        {
            List<Entity.RuleValue> retList = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleValue_Select_SourceFileId_IsActive";
            cmd.Parameters.AddWithValue("@sourceFileId", sourceFileId);
            cmd.Parameters.AddWithValue("@isActive", isActive);

            retList = GetList(cmd);
            return retList;
        }
    }
}
