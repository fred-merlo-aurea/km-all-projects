using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAS.DataAccess
{
    public class FileRuleMap
    {
        public static List<Entity.FileRuleMap> Select()
        {
            List<Entity.FileRuleMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RulesMap_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.FileRuleMap Get(SqlCommand cmd)
        {
            Entity.FileRuleMap retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.FileRuleMap();
                        DynamicBuilder<Entity.FileRuleMap> builder = DynamicBuilder<Entity.FileRuleMap>.CreateBuilder(rdr);
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
        private static List<Entity.FileRuleMap> GetList(SqlCommand cmd)
        {
            List<Entity.FileRuleMap> retList = new List<Entity.FileRuleMap>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.FileRuleMap retItem = new Entity.FileRuleMap();
                        DynamicBuilder<Entity.FileRuleMap> builder = DynamicBuilder<Entity.FileRuleMap>.CreateBuilder(rdr);
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

        public static int Save(Entity.FileRuleMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileRuleMap_Save";
            cmd.Parameters.Add(new SqlParameter("@RulesAssignedID", x.RulesAssignedID));
            cmd.Parameters.Add(new SqlParameter("@RulesID", x.RulesID));
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", x.SourceFileID));
            cmd.Parameters.Add(new SqlParameter("@RuleOrder", x.RuleOrder));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.UAS.ToString()));
        }

        public static bool Delete(int SourceFileID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileRuleMap_Delete_SourceFileByID";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", SourceFileID));

            return DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.UAS.ToString());
        }
    }
}
