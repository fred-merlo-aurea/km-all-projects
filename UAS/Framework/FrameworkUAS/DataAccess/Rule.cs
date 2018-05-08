using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class Rule 
    {
        public static Entity.Rule GetRule(int ruleId)
        {
            Entity.Rule x = new Entity.Rule();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Rule_Select_RuleId";
            cmd.Parameters.AddWithValue("@ruleId", ruleId);

            x = Get(cmd);
            return x;
        }
        public static List<Entity.Rule> GetRulesForClient(int clientId)
        {
            List<Entity.Rule> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Rule_Select_ClientId";
            cmd.Parameters.AddWithValue("@clientId", clientId);
            
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.Rule Get(SqlCommand cmd)
        {
            Entity.Rule retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Rule();
                        DynamicBuilder<Entity.Rule> builder = DynamicBuilder<Entity.Rule>.CreateBuilder(rdr);
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
        private static List<Entity.Rule> GetList(SqlCommand cmd)
        {
            List<Entity.Rule> retList = new List<Entity.Rule>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Rule retItem = new Entity.Rule();
                        DynamicBuilder<Entity.Rule> builder = DynamicBuilder<Entity.Rule>.CreateBuilder(rdr);
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
        public static bool IsRuleNameUnique(int clientId, string ruleName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_IsRuleNameUnique";
            cmd.Parameters.AddWithValue("@clientId", clientId);
            cmd.Parameters.AddWithValue("@ruleName", ruleName);
            int count = Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
            bool isUnique = true;
            if (count > 0) isUnique = false;
            return isUnique;
        }
        public static int Save(Entity.Rule x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Rule_Save";
            cmd.Parameters.AddWithValue("@RuleId", x.RuleId);
            cmd.Parameters.AddWithValue("@RuleName", x.RuleName);
            cmd.Parameters.AddWithValue("@DisplayName", x.DisplayName);
            cmd.Parameters.AddWithValue("@RuleDescription", x.RuleDescription);
            cmd.Parameters.AddWithValue("@IsDeleteRule", x.IsDeleteRule);
            cmd.Parameters.AddWithValue("@IsSystem", x.IsSystem);
            cmd.Parameters.AddWithValue("@IsGlobal", x.IsGlobal);
            cmd.Parameters.AddWithValue("@ClientId", x.ClientId);
            cmd.Parameters.AddWithValue("@IsActive", x.IsActive);
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@DateUpdated", x.DateUpdated);
            cmd.Parameters.AddWithValue("@CreatedByUserId", x.CreatedByUserId);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", x.UpdatedByUserId);
            cmd.Parameters.AddWithValue("@CustomImportRuleId", x.CustomImportRuleId);
            cmd.Parameters.AddWithValue("@RuleActionId", x.RuleActionId);

            int ruleId = Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
            return ruleId;
        }
        public static int CopyRule(int existingRuleId, int newRuleSetId, int userId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_Rule_Copy";
            cmd.Parameters.AddWithValue("@existingRuleId", existingRuleId);
            cmd.Parameters.AddWithValue("@newRuleSetId", newRuleSetId);
            cmd.Parameters.AddWithValue("@createdByUserId", userId);
            int ruleId = Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
            return ruleId;
        }
        #region Object.Rule
        private static Object.Rule GetObject(SqlCommand cmd)
        {
            Object.Rule retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.Rule();
                        DynamicBuilder<Object.Rule> builder = DynamicBuilder<Object.Rule>.CreateBuilder(rdr);
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
        private static List<Object.Rule> GetObjectList(SqlCommand cmd)
        {
            List<Object.Rule> retList = new List<Object.Rule>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Object.Rule retItem = new Object.Rule();
                        DynamicBuilder<Object.Rule> builder = DynamicBuilder<Object.Rule>.CreateBuilder(rdr);
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

        public static Object.Rule GetRuleObject(int ruleId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Rule_Select_RuleId";
            cmd.Parameters.AddWithValue("@ruleId", ruleId);
            
            return GetObject(cmd);
        }
        public static List<Object.Rule> GetRulesForSourceFile(int sourceFileId, bool isActive = true)
        {
            List<Object.Rule> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Rule_Select_SourceFileId";
            cmd.Parameters.AddWithValue("@sourceFileId", sourceFileId);
            cmd.Parameters.AddWithValue("@isActive", isActive);

            retItem = GetObjectList(cmd);
            return retItem;
        }
        public static List<Object.Rule> GetRulesForClient(int clientId, bool isActive = true)
        {
            List<Object.Rule> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Rule_Select_ClientId";
            cmd.Parameters.AddWithValue("@clientId", clientId);
            cmd.Parameters.AddWithValue("@isActive", isActive);

            retItem = GetObjectList(cmd);
            return retItem;
        }
        public static List<Object.Rule> GetRulesForRuleSet(int ruleSetId, bool isActive = true)
        {
            List<Object.Rule> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Rule_Select_RuleSetId";
            cmd.Parameters.AddWithValue("@ruleSetId", ruleSetId);
            cmd.Parameters.AddWithValue("@isActive", isActive);

            retItem = GetObjectList(cmd);
            return retItem;
        }
        public static List<Object.Rule> GetSystemRulesForRuleSet(int ruleSetId, bool isActive = true)
        {
            List<Object.Rule> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Rule_Select_System_RuleSetId";
            cmd.Parameters.AddWithValue("@ruleSetId", ruleSetId);
            cmd.Parameters.AddWithValue("@isActive", isActive);

            retItem = GetObjectList(cmd);
            return retItem;
        }
        public static List<Object.Rule> GetSystemRules( bool isActive = true)
        {
            List<Object.Rule> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Rule_Select_System";
            cmd.Parameters.AddWithValue("@isActive", isActive);

            retItem = GetObjectList(cmd);
            return retItem;
        }
        #endregion
        #region Object.CustomRule
        public static List<Object.CustomRule> GetCustomRules(int ruleSetId)
        {
            List<Object.CustomRule> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_CustomRule_RuleSetId";
            cmd.Parameters.AddWithValue("@ruleSetId", ruleSetId);

            retItem = GetCustomRuleList(cmd);
            return retItem;
        }
        public static Object.CustomRule GetCustomRule(int ruleId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_CustomRule_RuleId";
            cmd.Parameters.AddWithValue("@ruleId", ruleId);

            return DataFunctions.Get<Object.CustomRule>(cmd);
        }
        private static List<Object.CustomRule> GetCustomRuleList(SqlCommand cmd)
        {
            List<Object.CustomRule> retList = new List<Object.CustomRule>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Object.CustomRule retItem = new Object.CustomRule();
                        DynamicBuilder<Object.CustomRule> builder = DynamicBuilder<Object.CustomRule>.CreateBuilder(rdr);
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
        #endregion
        #region Object.CustomRuleInsertUpdateNew
        public static List<Object.CustomRuleInsertUpdateNew> GetCustomRuleInsertUpdateNew(int ruleId)
        {
            List<Object.CustomRuleInsertUpdateNew> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_CustomRuleInsertUpdateNew_RuleId";
            cmd.Parameters.AddWithValue("@ruleId", ruleId);
            
            retItem = GetCustomRuleInsertUpdateNewList(cmd);
            return retItem;
        }
        private static List<Object.CustomRuleInsertUpdateNew> GetCustomRuleInsertUpdateNewList(SqlCommand cmd)
        {
            List<Object.CustomRuleInsertUpdateNew> retList = new List<Object.CustomRuleInsertUpdateNew>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Object.CustomRuleInsertUpdateNew retItem = new Object.CustomRuleInsertUpdateNew();
                        DynamicBuilder<Object.CustomRuleInsertUpdateNew> builder = DynamicBuilder<Object.CustomRuleInsertUpdateNew>.CreateBuilder(rdr);
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
        #endregion
    }
}
