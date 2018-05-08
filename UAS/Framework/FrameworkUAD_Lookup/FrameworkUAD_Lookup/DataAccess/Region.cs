using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAD_Lookup.DataAccess
{
    public class Region
    {
        public static List<Entity.Region> Select()
        {

            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = "UAD_LOOKUP";

                List<Entity.Region> Regions = (List<Entity.Region>) CacheUtil.GetFromCache("Region", DatabaseName);

                if (Regions == null)
                {
                    Regions = GetData();

                    CacheUtil.AddToCache("Region", Regions, DatabaseName);
                }

                return Regions;
            }
            else
            {
                return GetData();
            }
        }

        private static List<Entity.Region> GetData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Region_Select";
            
            List<Entity.Region> retList = new List<Entity.Region>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Region retItem = new Entity.Region();
                        DynamicBuilder<Entity.Region> builder = DynamicBuilder<Entity.Region>.CreateBuilder(rdr);
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
