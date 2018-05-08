using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM;
using KM.Common;
using KM.Common.Data;

namespace FrameworkSubGen.DataAccess
{
    public class Payment
    {
        public static bool SaveBulkXml(string xml)
        {
            bool success = false;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Payment_SaveBulkXml";
                cmd.Parameters.AddWithValue("@xml", xml);
                cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());

                success = KM.Common.DataFunctions.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Payment", "SaveBulkXml");
            }
            return success;
        }
        public static Entity.Payment Select(int subscriberId, DateTime dateCreated)
        {
            Entity.Payment retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Payment_Select_SubscriberId_DateCreated";
            cmd.Parameters.AddWithValue("@subscriberId", subscriberId);
            cmd.Parameters.AddWithValue("@dateCreated", dateCreated);
            retItem = Get(cmd);
            return retItem;
        }
        private static Entity.Payment Get(SqlCommand cmd)
        {
            Entity.Payment retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Payment();
                        DynamicBuilder<Entity.Payment> builder = DynamicBuilder<Entity.Payment>.CreateBuilder(rdr);
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
        public static bool Update_SubscriptionId(int orderId, int subscriptionId)
        {
            bool success = false;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Payment_UpdateSubscriptionId";
                cmd.Parameters.AddWithValue("@orderId", orderId);
                cmd.Parameters.AddWithValue("@subscriptionId", subscriptionId);
                cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());

                success = KM.Common.DataFunctions.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Payment", "Update_SubscriptionId");
            }
            return success;
        }
        public static bool Update_STRecordIdentifier(int orderId, Guid STRecordIdentifier)
        {
            bool success = false;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Payment_UpdateSTRecordIdentifier";
                cmd.Parameters.AddWithValue("@orderId", orderId);
                cmd.Parameters.AddWithValue("@STRecordIdentifier", STRecordIdentifier);
                cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());

                success = KM.Common.DataFunctions.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Payment", "Update_STRecordIdentifier");
            }
            return success;
        }
        public static Entity.Payment Select(Guid stRecordIdentifier)
        {
            Entity.Payment retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Payment_Select_STRecordIdentifier";
            cmd.Parameters.AddWithValue("@STRecordIdentifier", stRecordIdentifier);
            retItem = Get(cmd);
            return retItem;
        }
    }
}
