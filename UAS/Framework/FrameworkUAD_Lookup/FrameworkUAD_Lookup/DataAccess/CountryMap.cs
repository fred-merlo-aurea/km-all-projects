using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAD_Lookup.DataAccess
{
    public class CountryMap
    {
        public static List<Entity.CountryMap> Select()
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = "UAD_LOOKUP";

                List<Entity.CountryMap> retItem = (List<Entity.CountryMap>) CacheUtil.GetFromCache("CountryMap", DatabaseName);

                if (retItem == null)
                {
                    retItem = GetData();

                    CacheUtil.AddToCache("CountryMap", retItem, DatabaseName);
                }

                return retItem;
            }
            else
            {
                return GetData();
            }
        }

        private static Entity.CountryMap Get(SqlCommand cmd)
        {
            Entity.CountryMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.CountryMap();
                        DynamicBuilder<Entity.CountryMap> builder = DynamicBuilder<Entity.CountryMap>.CreateBuilder(rdr);
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
        private static List<Entity.CountryMap> GetData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CountryMap_Select";
            List<Entity.CountryMap> retList = new List<Entity.CountryMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.CountryMap retItem = new Entity.CountryMap();
                        DynamicBuilder<Entity.CountryMap> builder = DynamicBuilder<Entity.CountryMap>.CreateBuilder(rdr);
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
            catch (Exception ex)
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
