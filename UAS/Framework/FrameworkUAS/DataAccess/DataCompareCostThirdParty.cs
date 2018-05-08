using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class DataCompareCostThirdParty
    {
        public static List<Entity.DataCompareCostThirdParty> Select(int clientId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareCostThirdParty_Select_ClientId";
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            return GetList(cmd);
        }
        public static Entity.DataCompareCostThirdParty Get(SqlCommand cmd)
        {
            Entity.DataCompareCostThirdParty retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.DataCompareCostThirdParty();
                        DynamicBuilder<Entity.DataCompareCostThirdParty> builder = DynamicBuilder<Entity.DataCompareCostThirdParty>.CreateBuilder(rdr);
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
        public static List<Entity.DataCompareCostThirdParty> GetList(SqlCommand cmd)
        {
            List<Entity.DataCompareCostThirdParty> retList = new List<Entity.DataCompareCostThirdParty>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.DataCompareCostThirdParty retItem = new Entity.DataCompareCostThirdParty();
                        DynamicBuilder<Entity.DataCompareCostThirdParty> builder = DynamicBuilder<Entity.DataCompareCostThirdParty>.CreateBuilder(rdr);
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
