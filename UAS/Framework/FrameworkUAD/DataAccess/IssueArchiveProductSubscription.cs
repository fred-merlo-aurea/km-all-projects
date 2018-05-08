using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using KM.Common;
using ServiceStack;

namespace FrameworkUAD.DataAccess
{
    public class IssueArchiveProductSubscription
    {
        private static Entity.IssueArchiveProductSubscription Get(SqlCommand cmd)
        {
            Entity.IssueArchiveProductSubscription retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.IssueArchiveProductSubscription();
                        DynamicBuilder<Entity.IssueArchiveProductSubscription> builder = DynamicBuilder<Entity.IssueArchiveProductSubscription>.CreateBuilder(rdr);
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
        private static List<Entity.IssueArchiveProductSubscription> GetList(SqlCommand cmd)
        {
            List<Entity.IssueArchiveProductSubscription> retList = new List<Entity.IssueArchiveProductSubscription>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.IssueArchiveProductSubscription retItem = new Entity.IssueArchiveProductSubscription();
                        DynamicBuilder<Entity.IssueArchiveProductSubscription> builder = DynamicBuilder<Entity.IssueArchiveProductSubscription>.CreateBuilder(rdr);
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
        public static List<Entity.IssueArchiveProductSubscription> SelectIssue(int issueID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueArchiveProductSubscription> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueArchiveSubscription_Select_IssueID";
            cmd.Parameters.AddWithValue("@IssueID", issueID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.IssueArchiveProductSubscription> SelectPaging(int page, int pageSize, int issueID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueArchiveSubscription_Select_Paging";
            cmd.Parameters.AddWithValue("@CurrentPage", page);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@IssueID", issueID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static int SelectCount(int issueID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueArchiveSubscription_Select_Count";
            cmd.Parameters.AddWithValue("@IssueID", issueID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static int Save(Entity.IssueArchiveProductSubscription x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueArchiveProductSubscription_Save";
            cmd.Parameters.Add(new SqlParameter("@IssueArchiveSubscriptionId", x.IssueArchiveSubscriptionId));
            //cmd.Parameters.Add(new SqlParameter("@IssueArchiveSubscriberId", x.IssueArchiveSubscriberId));
            cmd.Parameters.Add(new SqlParameter("@IsComp", x.IsComp));
            cmd.Parameters.Add(new SqlParameter("@CompId", x.CompId));
            cmd.Parameters.AddWithValue("@PubSubscriptionID", x.PubSubscriptionID);
            cmd.Parameters.AddWithValue("@SubscriptionID", x.SubscriptionID);
            cmd.Parameters.AddWithValue("@PubID", (object)x.PubID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@demo7", (object)x.Demo7 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Qualificationdate", (object)x.QualificationDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PubQSourceID", (object)x.PubQSourceID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PubCategoryID", (object)x.PubCategoryID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PubTransactionID", (object)x.PubTransactionID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object)x.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EmailStatusID", (object)x.EmailStatusID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StatusUpdatedDate", (object)x.StatusUpdatedDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StatusUpdatedReason", (object)x.StatusUpdatedReason ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsComp", (object)x.IsComp ?? DBNull.Value);
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static int SaveAll(Entity.IssueArchiveProductSubscription subscription, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueArchiveProductSubscription_SaveAll";

            var paramNameTransformations = new Dictionary<string, string>()
            {
                {"IMBSeq", "IMBSEQ" },
                {"Verified", "Verify" }
            };

            foreach (var prop in typeof(Entity.IssueArchiveProductSubscription).GetProperties())
            {
                var paramName = paramNameTransformations.ContainsKey(prop.Name)
                    ? paramNameTransformations[prop.Name]
                    : prop.Name;
                cmd.Parameters.AddWithValue($"@{paramName}", prop.GetValue(subscription));
            }

            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool SaveBulkSqlInsert(List<Entity.IssueArchiveProductSubscription> list, KMPlatform.Object.ClientConnections client)
        {
            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.IssueArchiveProductSubscription>.ToDataTable(list);
            bool done = true;
            SqlConnection conn = KM.Common.DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.UAD_Master.ToString());

            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "IssueArchiveSubscription");
                bc.BatchSize = 1000;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("IssueArchiveSubscriptionId", "IssueArchiveSubscriptionId");
                bc.ColumnMappings.Add("IssueArchiveSubscriberId", "IssueArchiveSubscriberId");
                bc.ColumnMappings.Add("IsComp", "IsComp");
                bc.ColumnMappings.Add("CompId", "CompId");
                bc.ColumnMappings.Add("SubscriptionID", "SubscriptionID");
                bc.ColumnMappings.Add("SequenceID", "SequenceID");
                bc.ColumnMappings.Add("SubscriberID", "SubscriberID");
                bc.ColumnMappings.Add("PublisherID", "PublisherID");
                bc.ColumnMappings.Add("PublicationID", "PublicationID");
                bc.ColumnMappings.Add("ActionID_Current", "ActionID_Current");
                bc.ColumnMappings.Add("ActionID_Previous", "ActionID_Previous");
                bc.ColumnMappings.Add("SubscriptionStatusID", "SubscriptionStatusID");
                bc.ColumnMappings.Add("IsPaid", "IsPaid");
                bc.ColumnMappings.Add("QSourceID", "QSourceID");
                bc.ColumnMappings.Add("QSourceDate", "QSourceDate");
                bc.ColumnMappings.Add("DeliverabilityID", "DeliverabilityID");
                bc.ColumnMappings.Add("IsSubscribed", "IsSubscribed");
                bc.ColumnMappings.Add("SubscriberSourceCode", "SubscriberSourceCode");
                bc.ColumnMappings.Add("Copies", "Copies");
                bc.ColumnMappings.Add("OriginalSubscriberSourceCode", "OriginalSubscriberSourceCode");
                bc.ColumnMappings.Add("Par3cID", "Par3cID");
                bc.ColumnMappings.Add("SubsrcTypeID", "SubsrcTypeID");
                bc.ColumnMappings.Add("AccountNumber", "AccountNumber");
                bc.ColumnMappings.Add("OnBehalfOf", "OnBehalfOf");
                bc.ColumnMappings.Add("MemberGroup", "MemberGroup");
                bc.ColumnMappings.Add("Verify", "Verify");
                bc.ColumnMappings.Add("DateCreated", "DateCreated");
                bc.ColumnMappings.Add("DateUpdated", "DateUpdated");
                bc.ColumnMappings.Add("CreatedByUserID", "CreatedByUserID");
                bc.ColumnMappings.Add("UpdatedByUserID", "UpdatedByUserID");

                bc.WriteToServer(dt);
                bc.Close();
            }
            catch
            {
                done = false;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return done;
        }

        public static List<Entity.IssueArchiveProductSubscription> SelectForUpdate(int productID, int issueid, string pubsubs, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueArchiveProductSubscription_SelectForUpdate";
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Parameters.AddWithValue("@IssueID", issueid);
            cmd.Parameters.AddWithValue("@PubSubs", pubsubs);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
    }
}
