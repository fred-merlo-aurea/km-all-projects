using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class PaidOrderDetail
    {
        public static List<Entity.PaidOrderDetail> SelectPaidOrder(int paidOrderId, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PaidOrderDetail_Select_PaidOrderId";
            cmd.Parameters.AddWithValue("@PaidOrderId", paidOrderId);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.PaidOrderDetail> SelectProductSubscription(int productSubscriptionId, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PaidOrderDetail_Select_ProductSubscriptionId";
            cmd.Parameters.AddWithValue("@ProductSubscriptionId", productSubscriptionId);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.PaidOrderDetail> SelectSubGenBundle(int subGenBundleId, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PaidOrderDetail_Select_SubGenBundleId";
            cmd.Parameters.AddWithValue("@SubGenBundleId", subGenBundleId);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }

        private static Entity.PaidOrderDetail Get(SqlCommand cmd)
        {
            Entity.PaidOrderDetail retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.PaidOrderDetail();
                        DynamicBuilder<Entity.PaidOrderDetail> builder = DynamicBuilder<Entity.PaidOrderDetail>.CreateBuilder(rdr);
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
        private static List<Entity.PaidOrderDetail> GetList(SqlCommand cmd)
        {
            List<Entity.PaidOrderDetail> retList = new List<Entity.PaidOrderDetail>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.PaidOrderDetail retItem = new Entity.PaidOrderDetail();
                        DynamicBuilder<Entity.PaidOrderDetail> builder = DynamicBuilder<Entity.PaidOrderDetail>.CreateBuilder(rdr);
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

        public static int Save(Entity.PaidOrderDetail x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PaidOrderDetail_Save";
            cmd.Parameters.AddWithValue("@PaidOrderDetailId", x.PaidOrderDetailId);
            cmd.Parameters.AddWithValue("@PaidOrderId", x.PaidOrderId);
            cmd.Parameters.AddWithValue("@ProductSubscriptionId", x.ProductSubscriptionId);
            cmd.Parameters.AddWithValue("@ProductId", x.ProductId);
            cmd.Parameters.AddWithValue("@RefundDate", (object)x.RefundDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FulfilledDate", (object)x.FulfilledDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SubTotal", x.SubTotal);
            cmd.Parameters.AddWithValue("@TaxTotal", x.TaxTotal);
            cmd.Parameters.AddWithValue("@GrandTotal", x.GrandTotal);
            cmd.Parameters.AddWithValue("@SubGenBundleId", x.SubGenBundleId);
            cmd.Parameters.AddWithValue("@SubGenOrderItemId", x.SubGenOrderItemId);
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedByUserId", x.CreatedByUserId);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", (object)x.UpdatedByUserId ?? DBNull.Value);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static bool SaveBulkInsert(List<Entity.PaidOrderDetail> x, KMPlatform.Object.ClientConnections client)
        {
            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.PaidOrderDetail>.ToDataTable(x);
            bool done = true;
            SqlConnection conn = DataFunctions.GetClientSqlConnection(client);
            SqlBulkCopy bc = default(SqlBulkCopy);
            try
            {
                conn.Open();
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "PaidOrderDetail");
                bc.BatchSize = 0;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("PaidOrderDetailId", "PaidOrderDetailId");
                bc.ColumnMappings.Add("PaidOrderId", "PaidOrderId");
                bc.ColumnMappings.Add("ProductSubscriptionId", "ProductSubscriptionId");
                bc.ColumnMappings.Add("ProductId", "ProductId");
                bc.ColumnMappings.Add("RefundDate", "RefundDate");
                bc.ColumnMappings.Add("FulfilledDate", "FulfilledDate");
                bc.ColumnMappings.Add("SubTotal", "SubTotal");
                bc.ColumnMappings.Add("TaxTotal", "TaxTotal");
                bc.ColumnMappings.Add("GrandTotal", "GrandTotal");
                bc.ColumnMappings.Add("SubGenBundleId", "SubGenBundleId");
                bc.ColumnMappings.Add("SubGenOrderItemId", "SubGenOrderItemId");
                bc.ColumnMappings.Add("DateCreated", "DateCreated");
                bc.ColumnMappings.Add("DateUpdated", "DateUpdated");
                bc.ColumnMappings.Add("CreatedByUserId", "CreatedByUserId");
                bc.ColumnMappings.Add("UpdatedByUserId", "UpdatedByUserId");

                bc.WriteToServer(dt);
                bc.Close();
            }
            catch (Exception ex)
            {
                done = false;
                string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                alWorker.LogCriticalError(message, "PaidOrderDetail.SaveBulkInsert", KMPlatform.BusinessLogic.Enums.Applications.AMS_Operations);

                if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                {
                    string pattern = @"\d+";
                    Match match = Regex.Match(ex.Message.ToString(), pattern);
                    var index = Convert.ToInt32(match.Value) - 1;

                    FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                    var sortedColumns = fi.GetValue(bc);
                    var items = (object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                    FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                    var metadata = itemdata.GetValue(items[index]);

                    var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                    var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                    var formatEx = String.Format("Column: {0} contains data with a length greater than: {1}", column, length);
                    alWorker.LogCriticalError(formatEx.ToString(), "PaidOrderDetail.SaveBulkInsert", KMPlatform.BusinessLogic.Enums.Applications.AMS_Operations);
                }

                throw ex;
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
