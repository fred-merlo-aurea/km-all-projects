using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KMPlatform.Object;

namespace FrameworkUAD.DataAccess
{
    public class Market
    {
        public static List<Entity.Market> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Market> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Market_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.Market> SelectByUserID(int UserID, ClientConnections client)
        {
            List<Entity.Market> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Market_Select_UserID";
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        public static Entity.Market Get(SqlCommand cmd)
        {
            Entity.Market retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Market();
                        DynamicBuilder<Entity.Market> builder = DynamicBuilder<Entity.Market>.CreateBuilder(rdr);
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
        public static List<Entity.Market> GetList(SqlCommand cmd)
        {
            List<Entity.Market> retList = new List<Entity.Market>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.Market retItem = new Entity.Market();
                        DynamicBuilder<Entity.Market> builder = DynamicBuilder<Entity.Market>.CreateBuilder(rdr);
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
        public static int Save(Entity.Market x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Market_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@MarketID", x.BrandID);
	        cmd.Parameters.AddWithValue("@MarketName", x.BrandID);
	        cmd.Parameters.AddWithValue("@MarketXML", x.BrandID);
	        cmd.Parameters.AddWithValue("@BrandID", x.BrandID);
	        cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
	        cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
	        cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
	        cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
