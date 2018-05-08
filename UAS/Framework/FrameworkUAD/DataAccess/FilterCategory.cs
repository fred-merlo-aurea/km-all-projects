using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class FilterCategory
    {
        public static List<Entity.FilterCategory> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.FilterCategory> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterCategory_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.FilterCategory Get(SqlCommand cmd)
        {
            Entity.FilterCategory retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.FilterCategory();
                        DynamicBuilder<Entity.FilterCategory> builder = DynamicBuilder<Entity.FilterCategory>.CreateBuilder(rdr);
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
        public static List<Entity.FilterCategory> GetList(SqlCommand cmd)
        {
            List<Entity.FilterCategory> retList = new List<Entity.FilterCategory>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.FilterCategory retItem = new Entity.FilterCategory();
                        DynamicBuilder<Entity.FilterCategory> builder = DynamicBuilder<Entity.FilterCategory>.CreateBuilder(rdr);
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
