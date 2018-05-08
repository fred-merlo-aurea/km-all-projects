using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class RuleSetFileMap
    {
        private static Entity.RuleSetFileMap Get(SqlCommand cmd)
        {
            Entity.RuleSetFileMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.RuleSetFileMap();
                        DynamicBuilder<Entity.RuleSetFileMap> builder = DynamicBuilder<Entity.RuleSetFileMap>.CreateBuilder(rdr);
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
        private static List<Entity.RuleSetFileMap> GetList(SqlCommand cmd)
        {
            List<Entity.RuleSetFileMap> retList = new List<Entity.RuleSetFileMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.RuleSetFileMap retItem = new Entity.RuleSetFileMap();
                        DynamicBuilder<Entity.RuleSetFileMap> builder = DynamicBuilder<Entity.RuleSetFileMap>.CreateBuilder(rdr);
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
        public static bool Save(Entity.RuleSetFileMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleSetFileMap_Save";
            cmd.Parameters.AddWithValue("@RuleSetId", x.RuleSetId);
            cmd.Parameters.AddWithValue("@SourceFileId", x.SourceFileId);
            cmd.Parameters.AddWithValue("@FileTypeId ", x.FileTypeId);
            cmd.Parameters.AddWithValue("@IsSystem", x.IsSystem);
            cmd.Parameters.AddWithValue("@IsGlobal", x.IsGlobal);
            cmd.Parameters.AddWithValue("@IsActive", x.IsActive);
            cmd.Parameters.AddWithValue("@ExecutionPointId", x.ExecutionPointId);
            cmd.Parameters.AddWithValue("@ExecutionOrder", x.ExecutionOrder);
            cmd.Parameters.AddWithValue("@WhereClause", x.WhereClause);//not used - use RuleSetRuleOrder.Script - this will have entire where/update/delete script
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@DateUpdated", x.DateUpdated);
            cmd.Parameters.AddWithValue("@CreatedByUserId", x.CreatedByUserId);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", x.UpdatedByUserId);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool Save(int ruleSetId, int sourceFileId, int userId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RuleSetFileMap_SaveFromFileMapperWizard";
            cmd.Parameters.AddWithValue("@ruleSetId", ruleSetId);
            cmd.Parameters.AddWithValue("@sourceFileId", sourceFileId);
            cmd.Parameters.AddWithValue("@userId", userId);

            //cmd.Parameters.AddWithValue("@FileTypeId ", x.FileTypeId);//save from SourceFile table
            //cmd.Parameters.AddWithValue("@IsSystem", x.IsSystem);//save from RuleSet table
            //cmd.Parameters.AddWithValue("@IsGlobal", x.IsGlobal);//save from RuleSet table
            //cmd.Parameters.AddWithValue("@IsActive", x.IsActive);//save from RuleSet table
            //cmd.Parameters.AddWithValue("@ExecutionPointId", x.ExecutionPointId);//save CodeName='Custom Import Rule' CodeTypeId=8 CodetypeName='Execution Points'


            //cmd.Parameters.AddWithValue("@ExecutionOrder", x.ExecutionOrder);//0
            //cmd.Parameters.AddWithValue("@WhereClause", x.WhereClause);//blank - not used - use RuleSetRuleOrder.Script - this will have entire where/update/delete script
            //cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);//getdate()
            //cmd.Parameters.AddWithValue("@DateUpdated", x.DateUpdated);//getdate()
            //cmd.Parameters.AddWithValue("@CreatedByUserId", x.CreatedByUserId);//userid
            //cmd.Parameters.AddWithValue("@UpdatedByUserId", x.UpdatedByUserId);//userid

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
    }
}
