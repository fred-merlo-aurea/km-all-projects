using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM;
using KM.Common;
using KM.Common.Data;

namespace FrameworkSubGen.DataAccess
{
    public class Subscription
    {
        public static bool SaveBulkXml(string xml)
        {
            bool success = false;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Subscription_SaveBulkXml";
                cmd.Parameters.AddWithValue("@xml", xml);
                cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());

                success = KM.Common.DataFunctions.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Subscription", "SaveBulkXml");
            }
            return success;
        }
        public static Entity.Subscription Select(int subscriptionId)
        {
            Entity.Subscription retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_Select_SubscriptionId";
            cmd.Parameters.AddWithValue("@subscriptionId", subscriptionId);
            retItem = Get(cmd);
            return retItem;
        }
        private static Entity.Subscription Get(SqlCommand cmd)
        {
            Entity.Subscription retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Subscription();
                        DynamicBuilder<Entity.Subscription> builder = DynamicBuilder<Entity.Subscription>.CreateBuilder(rdr);
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
    }
}
