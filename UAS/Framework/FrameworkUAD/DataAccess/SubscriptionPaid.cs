using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class SubscriptionPaid
    {
        public static List<Entity.SubscriptionPaid> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionPaid_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static Entity.SubscriptionPaid Select(int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionPaid_Select_SubscriptionID";
            cmd.Parameters.AddWithValue("@PubSubscriptionID", subscriptionID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Get(cmd);
        }
        private static Entity.SubscriptionPaid Get(SqlCommand cmd)
        {
            Entity.SubscriptionPaid retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubscriptionPaid();
                        DynamicBuilder<Entity.SubscriptionPaid> builder = DynamicBuilder<Entity.SubscriptionPaid>.CreateBuilder(rdr);
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
        private static List<Entity.SubscriptionPaid> GetList(SqlCommand cmd)
        {
            List<Entity.SubscriptionPaid> retList = new List<Entity.SubscriptionPaid>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.SubscriptionPaid retItem = new Entity.SubscriptionPaid();
                        DynamicBuilder<Entity.SubscriptionPaid> builder = DynamicBuilder<Entity.SubscriptionPaid>.CreateBuilder(rdr);
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

        public static int Save(Entity.SubscriptionPaid x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionPaid_Save";
            cmd.Parameters.Add(new SqlParameter("@SubscriptionPaidID", x.SubscriptionPaidID));
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionID", x.PubSubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@PriceCodeID", x.PriceCodeID));
            cmd.Parameters.Add(new SqlParameter("@StartIssueDate", (object)x.StartIssueDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ExpireIssueDate", (object)x.ExpireIssueDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CPRate", x.CPRate));
            cmd.Parameters.Add(new SqlParameter("@Amount", x.Amount));
            cmd.Parameters.Add(new SqlParameter("@AmountPaid", x.AmountPaid));
            cmd.Parameters.Add(new SqlParameter("@BalanceDue", x.BalanceDue));
            cmd.Parameters.Add(new SqlParameter("@PaidDate", (object)x.PaidDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@TotalIssues", x.TotalIssues));
            cmd.Parameters.Add(new SqlParameter("@CheckNumber", (object)x.CheckNumber ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CCNumber", (object)x.CCNumber ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CCExpirationMonth", (object)x.CCExpirationMonth ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CCExpirationYear", (object)x.CCExpirationYear ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CCHolderName", (object)x.CCHolderName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreditCardTypeID", x.CreditCardTypeID));
            cmd.Parameters.Add(new SqlParameter("@PaymentTypeID", x.PaymentTypeID));
            cmd.Parameters.Add(new SqlParameter("@DeliverID", x.DeliverID));
            cmd.Parameters.Add(new SqlParameter("@GraceIssues", x.GraceIssues));
            cmd.Parameters.Add(new SqlParameter("@WriteOffAmount", x.WriteOffAmount));
            cmd.Parameters.Add(new SqlParameter("@OtherType", (object)x.OtherType ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Frequency", x.Frequency));
            cmd.Parameters.Add(new SqlParameter("@Term", x.Term));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static int ImportFromSubGen(string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionPaid_ImportFromSubGen";
            cmd.Parameters.Add(new SqlParameter("@ProcessCode", processCode));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static bool Save(List<Entity.SubscriptionPaid> spList, KMPlatform.Object.ClientConnections client)
        {
            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.SubscriptionPaid>.ToDataTable(spList);
            bool done = true;
            SqlConnection conn = DataFunctions.GetClientSqlConnection(client);
            SqlBulkCopy bc = default(SqlBulkCopy);
            try
            {
                conn.Open();
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "SubscriptionPaid");
                bc.BatchSize = 0;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("SubscriptionPaidID", "SubscriptionPaidID");
                bc.ColumnMappings.Add("PubSubscriptionID", "PubSubscriptionID");
                bc.ColumnMappings.Add("PriceCodeID", "PriceCodeID");
                bc.ColumnMappings.Add("StartIssueDate", "StartIssueDate");
                bc.ColumnMappings.Add("ExpireIssueDate", "ExpireIssueDate");
                bc.ColumnMappings.Add("CPRate", "CPRate");
                bc.ColumnMappings.Add("Amount", "Amount");
                bc.ColumnMappings.Add("AmountPaid", "AmountPaid");
                bc.ColumnMappings.Add("BalanceDue", "BalanceDue");
                bc.ColumnMappings.Add("PaidDate", "PaidDate");
                bc.ColumnMappings.Add("TotalIssues", "TotalIssues");
                bc.ColumnMappings.Add("CheckNumber", "CheckNumber");
                bc.ColumnMappings.Add("CCNumber", "CCNumber");
                bc.ColumnMappings.Add("CCExpirationMonth", "CCExpirationMonth");
                bc.ColumnMappings.Add("CCExpirationYear", "CCExpirationYear");
                bc.ColumnMappings.Add("CCHolderName", "CCHolderName");
                bc.ColumnMappings.Add("CreditCardTypeID", "CreditCardTypeID");
                bc.ColumnMappings.Add("PaymentTypeID", "PaymentTypeID");
                bc.ColumnMappings.Add("DeliverID", "DeliverID");
                bc.ColumnMappings.Add("GraceIssues", "GraceIssues");
                bc.ColumnMappings.Add("WriteOffAmount", "WriteOffAmount");
                bc.ColumnMappings.Add("OtherType", "OtherType");
                bc.ColumnMappings.Add("DateCreated", "DateCreated");
                bc.ColumnMappings.Add("DateUpdated", "DateUpdated");
                bc.ColumnMappings.Add("CreatedByUserID", "CreatedByUserID");
                bc.ColumnMappings.Add("UpdatedByUserID", "UpdatedByUserID");
                bc.ColumnMappings.Add("Frequency", "Frequency");
                bc.ColumnMappings.Add("Term", "Term");

                bc.WriteToServer(dt);
                bc.Close();
            }
            catch (Exception ex)
            {
                done = false;
                string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                KMPlatform.BusinessLogic.ApplicationLog alWrk = new KMPlatform.BusinessLogic.ApplicationLog();
                alWrk.LogCriticalError(message, "SubscriptionPaid.Save", KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine);  
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return done;
        }
    }
}
