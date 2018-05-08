using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class RuleSet
    {
        public static List<Entity.RuleSet> GetRuleSetsForClient(int clientId)
        {
            List<Entity.RuleSet> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleSet_Select_ClientId";
            cmd.Parameters.AddWithValue("@clientId", clientId);

            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.RuleSet GetSourceFile(int sourceFileId, bool isActive = true)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleSet_Select_SourceFileId_IsActive";
            cmd.Parameters.AddWithValue("@sourceFileId", sourceFileId);
            cmd.Parameters.AddWithValue("@isActive", isActive);
            
            return Get(cmd);
        }
        public static Entity.RuleSet GetRuleSetName(string ruleSetName, int clientID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleSet_Select_RuleSetName_ClientId";
            cmd.Parameters.AddWithValue("@ruleSetName", ruleSetName);
            cmd.Parameters.AddWithValue("@clientID", clientID);

            return Get(cmd);
        }
        public static bool RuleSetNameExists(string ruleSetName, int clientID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_RuleSet_RuleSetName_Exists";
            cmd.Parameters.AddWithValue("@ruleSetName", ruleSetName);
            cmd.Parameters.AddWithValue("@clientID", clientID);
            cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.UAS.ToString());
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        private static Entity.RuleSet Get(SqlCommand cmd)
        {
            Entity.RuleSet retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.RuleSet();
                        DynamicBuilder<Entity.RuleSet> builder = DynamicBuilder<Entity.RuleSet>.CreateBuilder(rdr);
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
        private static List<Entity.RuleSet> GetList(SqlCommand cmd)
        {
            List<Entity.RuleSet> retList = new List<Entity.RuleSet>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.RuleSet retItem = new Entity.RuleSet();
                        DynamicBuilder<Entity.RuleSet> builder = DynamicBuilder<Entity.RuleSet>.CreateBuilder(rdr);
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
        public static bool CopyRuleSet(int existingRuleSetId, int newRuleSetId, int createdByUserId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_RuleSet_Copy";
            cmd.Parameters.AddWithValue("@existingRuleSetId", existingRuleSetId);
            cmd.Parameters.AddWithValue("@newRuleSetId", newRuleSetId);
            cmd.Parameters.AddWithValue("@createdByUserId", createdByUserId);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static int Save(Entity.RuleSet x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleSet_Save";
            cmd.Parameters.AddWithValue("@RuleSetId", x.RuleSetId);
            cmd.Parameters.AddWithValue("@RuleSetName", x.RuleSetName);
            cmd.Parameters.AddWithValue("@DisplayName", x.DisplayName);
            cmd.Parameters.AddWithValue("@RuleSetDescription", x.RuleSetDescription);
            cmd.Parameters.AddWithValue("@IsActive", x.IsActive);
            cmd.Parameters.AddWithValue("@IsSystem", x.IsSystem);
            cmd.Parameters.AddWithValue("@IsGlobal", x.IsGlobal);
            cmd.Parameters.AddWithValue("@ClientId", x.ClientId);
            cmd.Parameters.AddWithValue("@IsDateSpecific", x.IsDateSpecific);
            cmd.Parameters.AddWithValue("@StartMonth", (object) x.StartMonth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndMonth", (object) x.EndMonth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StartDay", (object) x.StartDay ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDay", (object) x.EndDay ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StartYear", (object) x.StartYear ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndYear", (object) x.EndYear ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@DateUpdated", (object) x.DateUpdated ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedByUserId", x.CreatedByUserId);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", (object) x.UpdatedByUserId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CustomImportRuleId", x.CustomImportRuleId);
            
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
        public static bool UpdateRuleSet_Name_IsGlobal(int ruleSetId, string ruleSetName, bool isGlobalRuleSet, int updatedByUserId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleSet_Update_Name_IsGlobal";
            cmd.Parameters.AddWithValue("@RuleSetId", ruleSetId);
            cmd.Parameters.AddWithValue("@RuleSetName", ruleSetName);
            cmd.Parameters.AddWithValue("@IsGlobalRuleSet", isGlobalRuleSet);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", updatedByUserId);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        #region Object.RuleSet
        public static List<Object.RuleSet> GetRuleSetsForSourceFile(int sourceFileId, bool isActive = true)
        {
            List<Object.RuleSet> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_RuleSet_Select_SourceFileId";
            cmd.Parameters.AddWithValue("@sourceFileId", sourceFileId);
            cmd.Parameters.AddWithValue("@isActive", isActive);

            retItem = GetObjectList(cmd);
            return retItem;
        }
        public static List<Object.RuleSet> GetRuleSetsForSourceFile(int sourceFileId, int executionPointId, bool isActive = true)
        {
            List<Object.RuleSet> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_RuleSet_Select_SourceFileId_ExecutionPointId";
            cmd.Parameters.AddWithValue("@sourceFileId", sourceFileId);
            cmd.Parameters.AddWithValue("@executionPointId", executionPointId);
            cmd.Parameters.AddWithValue("@isActive", isActive);

            retItem = GetObjectList(cmd);
            return retItem;
        }
        public static List<Object.RuleSet> GetSystemRuleSets(bool isActive = true)
        {
            List<Object.RuleSet> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_RuleSet_Select_System";
            cmd.Parameters.AddWithValue("@isActive", isActive);

            retItem = GetObjectList(cmd);
            return retItem;
        }
        public static Object.RuleSet GetRuleSetObject(int ruleSetId)
        {
            Object.RuleSet retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_RuleSet_Select_RuleSetId";
            cmd.Parameters.AddWithValue("@ruleSetId", ruleSetId);

            retItem = GetObject(cmd);
            return retItem;
        }
        private static Object.RuleSet GetObject(SqlCommand cmd)
        {
            Object.RuleSet retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.RuleSet();
                        DynamicBuilder<Object.RuleSet> builder = DynamicBuilder<Object.RuleSet>.CreateBuilder(rdr);
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
        private static List<Object.RuleSet> GetObjectList(SqlCommand cmd)
        {
            List<Object.RuleSet> retList = new List<Object.RuleSet>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Object.RuleSet retItem = new Object.RuleSet();
                        DynamicBuilder<Object.RuleSet> builder = DynamicBuilder<Object.RuleSet>.CreateBuilder(rdr);
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
