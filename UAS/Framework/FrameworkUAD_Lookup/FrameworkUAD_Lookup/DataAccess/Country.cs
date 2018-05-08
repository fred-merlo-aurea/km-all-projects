using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAD_Lookup.DataAccess
{
    public class Country
    {
        public static List<Entity.Country> Select()
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = "UAD_LOOKUP";

                List<Entity.Country> Countries = (List<Entity.Country>) CacheUtil.GetFromCache("Country", DatabaseName);

                if (Countries == null)
                {
                    Countries = GetData();

                    CacheUtil.AddToCache("Country", Countries, DatabaseName);
                }

                return Countries;
            }
            else
            {
                return GetData();
            }
            
        }

        private static List<Entity.Country> GetData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Country_Select";
            List<Entity.Country> retList = new List<Entity.Country>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Country retItem = new Entity.Country();
                        DynamicBuilder<Entity.Country> builder = DynamicBuilder<Entity.Country>.CreateBuilder(rdr);
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


        private static Entity.Country Get(SqlCommand cmd)
        {
            Entity.Country retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Country();
                        DynamicBuilder<Entity.Country> builder = DynamicBuilder<Entity.Country>.CreateBuilder(rdr);
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
        

        public static bool CountryRegionCleanse(int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_CountryRegionCleanse";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Connection = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}
