using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class PaidOrder
    {
        public static List<Entity.PaidOrder> SelectSubscription(int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PaidOrder_Select_SubscriptionID";
            cmd.Parameters.AddWithValue("@SubscriptionID", subscriptionID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.PaidOrder> SelectSubGenSubscriber(int subGenSubscriberId, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PaidOrder_Select_SubGenSubscriberId";
            cmd.Parameters.AddWithValue("@SubGenSubscriberId", subGenSubscriberId);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        private static Entity.PaidOrder Get(SqlCommand cmd)
        {
            Entity.PaidOrder retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.PaidOrder();
                        DynamicBuilder<Entity.PaidOrder> builder = DynamicBuilder<Entity.PaidOrder>.CreateBuilder(rdr);
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
        private static List<Entity.PaidOrder> GetList(SqlCommand cmd)
        {
            List<Entity.PaidOrder> retList = new List<Entity.PaidOrder>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.PaidOrder retItem = new Entity.PaidOrder();
                        DynamicBuilder<Entity.PaidOrder> builder = DynamicBuilder<Entity.PaidOrder>.CreateBuilder(rdr);
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

        public static int Save(Entity.PaidOrder x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PaidOrder_Save";
            cmd.Parameters.AddWithValue("@PaidOrderId", x.PaidOrderId);
            cmd.Parameters.AddWithValue("@SubscriptionId", x.SubscriptionId);
            cmd.Parameters.AddWithValue("@ImportName", x.ImportName);
            cmd.Parameters.AddWithValue("@OrderDate", x.OrderDate);
            cmd.Parameters.AddWithValue("@IsGift", x.IsGift);
            cmd.Parameters.AddWithValue("@SubTotal", x.SubTotal);
            cmd.Parameters.AddWithValue("@TaxTotal", x.TaxTotal);
            cmd.Parameters.AddWithValue("@GrandTotal", x.GrandTotal);
            cmd.Parameters.AddWithValue("@PaymentAmount", x.PaymentAmount);
            cmd.Parameters.AddWithValue("@PaymentNote", x.PaymentNote);
            cmd.Parameters.AddWithValue("@PaymentTransactionId", x.PaymentTransactionId);
            cmd.Parameters.AddWithValue("@PaymentTypeCodeId", x.PaymentTypeCodeId);
            cmd.Parameters.AddWithValue("@UserId", x.UserId);
            cmd.Parameters.AddWithValue("@SubGenOrderId", x.SubGenOrderId);
            cmd.Parameters.AddWithValue("@SubGenSubscriberId", x.SubGenSubscriberId);
            cmd.Parameters.AddWithValue("@SubGenUserId", x.SubGenUserId);
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedByUserId", x.CreatedByUserId);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", (object)x.UpdatedByUserId ?? DBNull.Value);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
