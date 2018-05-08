using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAD_Lookup.DataAccess
{
    public class RegionMap
    {
        public static List<Entity.RegionMap> Select()
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = "UAD_LOOKUP";

                List<Entity.RegionMap> retItem = (List<Entity.RegionMap>) CacheUtil.GetFromCache("RegionMap", DatabaseName);

                if (retItem == null)
                {
                    retItem = GetData();

                    CacheUtil.AddToCache("RegionMap", retItem, DatabaseName);
                }

                return retItem;
            }
            else
            {
                return GetData();
            }
        }

        private static Entity.RegionMap Get(SqlCommand cmd)
        {
            Entity.RegionMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.RegionMap();
                        DynamicBuilder<Entity.RegionMap> builder = DynamicBuilder<Entity.RegionMap>.CreateBuilder(rdr);
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
        private static List<Entity.RegionMap> GetData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RegionMap_Select";
            List<Entity.RegionMap> retList = new List<Entity.RegionMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.RegionMap retItem = new Entity.RegionMap();
                        DynamicBuilder<Entity.RegionMap> builder = DynamicBuilder<Entity.RegionMap>.CreateBuilder(rdr);
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
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
    }
}
