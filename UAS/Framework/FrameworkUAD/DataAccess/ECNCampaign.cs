using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace FrameworkUAD.DataAccess
{
    public class ECNCampaign
    {
        public static List<Entity.ECNCampaign> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ECNCampaign> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ECNCampaign_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.ECNCampaign Get(SqlCommand cmd)
        {
            Entity.ECNCampaign retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ECNCampaign();
                        DynamicBuilder<Entity.ECNCampaign> builder = DynamicBuilder<Entity.ECNCampaign>.CreateBuilder(rdr);
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
        public static List<Entity.ECNCampaign> GetList(SqlCommand cmd)
        {
            List<Entity.ECNCampaign> retList = new List<Entity.ECNCampaign>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.ECNCampaign retItem = new Entity.ECNCampaign();
                        DynamicBuilder<Entity.ECNCampaign> builder = DynamicBuilder<Entity.ECNCampaign>.CreateBuilder(rdr);
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
