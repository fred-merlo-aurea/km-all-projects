using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class SubscriberAddKill
    {
        private static List<Entity.SubscriberAddKill> GetList(SqlCommand cmd)
        {
            List<Entity.SubscriberAddKill> retList = new List<Entity.SubscriberAddKill>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.SubscriberAddKill retItem = new Entity.SubscriberAddKill();
                        DynamicBuilder<Entity.SubscriberAddKill> builder = DynamicBuilder<Entity.SubscriberAddKill>.CreateBuilder(rdr);
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

        public static List<Entity.SubscriberAddKill> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberAddKill_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }

        public static int UpdateSubscription(int addKillID, int productID, string subscriptionIDs, bool deleteAddRemoveID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberAddKill_UpdateSubscription";
            cmd.Parameters.Add(new SqlParameter("@SubscriptionIDs", subscriptionIDs));
            cmd.Parameters.Add(new SqlParameter("@ProductID", productID));
            cmd.Parameters.Add(new SqlParameter("@AddRemoveID", addKillID));
            cmd.Parameters.Add(new SqlParameter("@DeleteAddRemoveID", deleteAddRemoveID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static int Save(Entity.SubscriberAddKill subAddKill, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberAddKill_Save";
            cmd.Parameters.Add(new SqlParameter("@AddKillID", subAddKill.AddKillID));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", subAddKill.PublicationID));
            cmd.Parameters.Add(new SqlParameter("@Count", subAddKill.Count));
            cmd.Parameters.Add(new SqlParameter("@AddKillCount", subAddKill.AddKillCount));
            cmd.Parameters.Add(new SqlParameter("@Type", subAddKill.Type));
            cmd.Parameters.Add(new SqlParameter("@IsActive", subAddKill.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", subAddKill.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)subAddKill.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", subAddKill.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)subAddKill.UpdatedByUserID ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool BulkInsertDetail(List<FrameworkUAD.Entity.SubscriberAddKillDetail> pubIDs, int addRemoveID, KMPlatform.Object.ClientConnections client)
        {
            bool done = true;

            DataTable dt = Core_AMS.Utilities.BulkDataReader<FrameworkUAD.Entity.SubscriberAddKillDetail>.ToDataTable(pubIDs);
            //DataColumn dc = new DataColumn("AddKillID");
            //dc.DefaultValue = addRemoveID;
            //dt.Columns.Add(dc);
            //dt.AcceptChanges();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(client);
            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "SubscriberAddKillDetail");
                bc.BatchSize = 2500;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("PubSubscriptionID", "PubSubscriptionID");
                bc.ColumnMappings.Add("AddKillID", "AddKillID");
                bc.ColumnMappings.Add("PubCategoryID", "PubCategoryID");
                bc.ColumnMappings.Add("PubTransactionID", "PubTransactionID");

                bc.WriteToServer(dt);
                bc.Close();
            }
            catch (Exception)
            {
                Core_AMS.Utilities.WPF.MessageError("There was a problem creating Add Remove preview data. Contact an administrator if the issue continues.");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return done;
        }

        public static bool ClearDetails(int productID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberAddKillDetail_Clear";
            cmd.Parameters.Add(new SqlParameter("@ProductID", productID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}
