using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class UASBridgeECN
    {
        public static List<Entity.UASBridgeECN> Select(int userID)
        {
            List<Entity.UASBridgeECN> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UASBridgeECN_Select_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.UASBridgeECN Get(SqlCommand cmd)
        {
            Entity.UASBridgeECN retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.UASBridgeECN();
                        DynamicBuilder<Entity.UASBridgeECN> builder = DynamicBuilder<Entity.UASBridgeECN>.CreateBuilder(rdr);
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
        private static List<Entity.UASBridgeECN> GetList(SqlCommand cmd)
        {
            List<Entity.UASBridgeECN> retList = new List<Entity.UASBridgeECN>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.UASBridgeECN retItem = new Entity.UASBridgeECN();
                        DynamicBuilder<Entity.UASBridgeECN> builder = DynamicBuilder<Entity.UASBridgeECN>.CreateBuilder(rdr);
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
