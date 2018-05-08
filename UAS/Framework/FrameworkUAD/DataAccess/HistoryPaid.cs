using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class HistoryPaid
    {
        public static List<Entity.HistoryPaid> Select(int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.HistoryPaid> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistoryPaid_Select_SubscriptionID";
            cmd.Parameters.Add(new SqlParameter("@SubscriptionID", subscriptionID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.HistoryPaid Get(SqlCommand cmd)
        {
            Entity.HistoryPaid retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.HistoryPaid();
                        DynamicBuilder<Entity.HistoryPaid> builder = DynamicBuilder<Entity.HistoryPaid>.CreateBuilder(rdr);
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
        private static List<Entity.HistoryPaid> GetList(SqlCommand cmd)
        {
            List<Entity.HistoryPaid> retList = new List<Entity.HistoryPaid>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.HistoryPaid retItem = new Entity.HistoryPaid();
                        DynamicBuilder<Entity.HistoryPaid> builder = DynamicBuilder<Entity.HistoryPaid>.CreateBuilder(rdr);
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
        public static int Save(Entity.HistoryPaid x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_HistoryPaid_Save";
            cmd.Parameters.Add(new SqlParameter("@SubscriptionPaidID", x.SubscriptionPaidID));
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionID", x.PubSubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@PriceCodeID", x.PriceCodeID));
            cmd.Parameters.Add(new SqlParameter("@StartIssueDate", x.StartIssueDate));
            cmd.Parameters.Add(new SqlParameter("@ExpireIssueDate", x.ExpireIssueDate));
            cmd.Parameters.Add(new SqlParameter("@CPRate", x.CPRate));
            cmd.Parameters.Add(new SqlParameter("@Amount", x.Amount));
            cmd.Parameters.Add(new SqlParameter("@AmountPaid", x.AmountPaid));
            cmd.Parameters.Add(new SqlParameter("@BalanceDue", x.BalanceDue));
            cmd.Parameters.Add(new SqlParameter("@PaidDate", x.PaidDate));
            cmd.Parameters.Add(new SqlParameter("@TotalIssues", x.TotalIssues));
            cmd.Parameters.Add(new SqlParameter("@CheckNumber", x.CheckNumber));
            cmd.Parameters.Add(new SqlParameter("@CCNumber", x.CCNumber));
            cmd.Parameters.Add(new SqlParameter("@CCExpirationMonth", x.CCExpirationMonth));
            cmd.Parameters.Add(new SqlParameter("@CCExpirationYear", x.CCExpirationYear));
            cmd.Parameters.Add(new SqlParameter("@CCHolderName", x.CCHolderName));
            cmd.Parameters.Add(new SqlParameter("@CreditCardTypeID", x.CreditCardTypeID));
            cmd.Parameters.Add(new SqlParameter("@PaymentTypeID", x.PaymentTypeID));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
