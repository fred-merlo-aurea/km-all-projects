using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FrameworkUAD.DataAccess
{
    public class Databases
    {
        public static List<Entity.Databases> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Databases> retItem = null;
            SqlCommand cmd = new SqlCommand();
            //cmd.CommandType = CommandType.Text;
            //cmd.CommandText = "SELECT name as 'DatabaseName' FROM sys.databases With(NoLock) " +
            //                  "where database_id not in (1,2,3,4)";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Database_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.Databases Get(SqlCommand cmd)
        {
            Entity.Databases retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Databases();
                        DynamicBuilder<Entity.Databases> builder = DynamicBuilder<Entity.Databases>.CreateBuilder(rdr);
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
        private static List<Entity.Databases> GetList(SqlCommand cmd)
        {
            List<Entity.Databases> retList = new List<Entity.Databases>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.Databases retItem = new Entity.Databases();
                        DynamicBuilder<Entity.Databases> builder = DynamicBuilder<Entity.Databases>.CreateBuilder(rdr);
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
