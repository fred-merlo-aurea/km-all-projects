using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class SecurityGroupTemplate
    {
        public static List<KMPlatform.Entity.SecurityGroupTemplate> GetNonAdminTemplates()
        {
             SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroupTemplate_GetNonAdmin";
            
            return GetList(cmd);
        }

        private static Entity.SecurityGroupTemplate Get(SqlCommand cmd)
        {
            Entity.SecurityGroupTemplate retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SecurityGroupTemplate();
                        var builder = DynamicBuilder<Entity.SecurityGroupTemplate>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
        private static List<Entity.SecurityGroupTemplate> GetList(SqlCommand cmd)
        {
            List<Entity.SecurityGroupTemplate> retList = new List<Entity.SecurityGroupTemplate>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.SecurityGroupTemplate retItem = new Entity.SecurityGroupTemplate();
                        var builder = DynamicBuilder<Entity.SecurityGroupTemplate>.CreateBuilder(rdr);
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
    }
}
