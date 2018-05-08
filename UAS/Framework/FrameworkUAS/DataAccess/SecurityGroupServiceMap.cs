using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class SecurityGroupServiceMap
    {
        public static List<Entity.SecurityGroupServiceMap> Select()
        {
            List<Entity.SecurityGroupServiceMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroupServiceMap_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        private static List<Entity.SecurityGroupServiceMap> GetList(SqlCommand cmd)
        {
            List<Entity.SecurityGroupServiceMap> retList = new List<Entity.SecurityGroupServiceMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.SecurityGroupServiceMap retItem = new Entity.SecurityGroupServiceMap();
                        DynamicBuilder<Entity.SecurityGroupServiceMap> builder = DynamicBuilder<Entity.SecurityGroupServiceMap>.CreateBuilder(rdr);
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
