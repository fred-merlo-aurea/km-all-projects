using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class DataImportExport
    {
        public static List<Entity.DataImportExport> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataImportExport_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }

        private static Entity.DataImportExport Get(SqlCommand cmd)
        {
            Entity.DataImportExport retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.DataImportExport();
                        DynamicBuilder<Entity.DataImportExport> builder = DynamicBuilder<Entity.DataImportExport>.CreateBuilder(rdr);
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
        private static List<Entity.DataImportExport> GetList(SqlCommand cmd)
        {
            List<Entity.DataImportExport> retList = new List<Entity.DataImportExport>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.DataImportExport retItem = new Entity.DataImportExport();
                        DynamicBuilder<Entity.DataImportExport> builder = DynamicBuilder<Entity.DataImportExport>.CreateBuilder(rdr);
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
