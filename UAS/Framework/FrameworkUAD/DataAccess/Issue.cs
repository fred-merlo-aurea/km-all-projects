using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class Issue
    {
        public static List<Entity.Issue> SelectPublication(int publicationID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Issue> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Issue_Select_PublicationID";
            cmd.Parameters.AddWithValue("@PublicationID", publicationID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.Issue> SelectPublisher(int publisherID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Issue> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Issue_Select_PublisherID";
            cmd.Parameters.AddWithValue("@PublisherID", publisherID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.Issue> Select( KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Issue> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Issue_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        public static bool ArchiveAll(int productID, int issueID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Issue_Archive_All";
            cmd.Parameters.Add(new SqlParameter("@ProductID", productID));
            cmd.Parameters.Add(new SqlParameter("@IssueID", issueID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool ArchiveAllIMB(int productID, int issueID, string imbSequences, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Issue_Archive_IMB_PubSubscriptions";
            cmd.Parameters.Add(new SqlParameter("@ProductID", productID));
            cmd.Parameters.Add(new SqlParameter("@IssueID", issueID));
            cmd.Parameters.Add(new SqlParameter("@IMBSequences", imbSequences));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        private static Entity.Issue Get(SqlCommand cmd)
        {
            Entity.Issue retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Issue();
                        DynamicBuilder<Entity.Issue> builder = DynamicBuilder<Entity.Issue>.CreateBuilder(rdr);
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
        private static List<Entity.Issue> GetList(SqlCommand cmd)
        {
            List<Entity.Issue> retList = new List<Entity.Issue>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.Issue retItem = new Entity.Issue();
                        DynamicBuilder<Entity.Issue> builder = DynamicBuilder<Entity.Issue>.CreateBuilder(rdr);
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

        public static int Save(Entity.Issue x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Issue_Save";
            cmd.Parameters.Add(new SqlParameter("@IssueId", x.IssueId));
            cmd.Parameters.Add(new SqlParameter("@PublicationId", x.PublicationId));
            cmd.Parameters.Add(new SqlParameter("@IssueName", x.IssueName));
            cmd.Parameters.Add(new SqlParameter("@IssueCode", x.IssueCode));
            cmd.Parameters.Add(new SqlParameter("@DateOpened", (object)x.DateOpened ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@OpenedByUserID", x.OpenedByUserID));
            cmd.Parameters.Add(new SqlParameter("@IsClosed", x.IsClosed));
            cmd.Parameters.Add(new SqlParameter("@DateClosed", (object)x.DateClosed ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ClosedByUserID", x.ClosedByUserID));
            cmd.Parameters.Add(new SqlParameter("@IsComplete", x.IsComplete));
            cmd.Parameters.Add(new SqlParameter("@DateComplete", (object)x.DateComplete ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CompleteByUserID", x.CompleteByUserID));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool BulkInsertSubGenIDs(List<Entity.IssueCloseSubGenMap> list, KMPlatform.Object.ClientConnections client)
        {
            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.IssueCloseSubGenMap>.ToDataTable(list);
            bool done = true;
            SqlConnection conn = DataFunctions.GetClientSqlConnection(client);

            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn, "IssueCloseSubGenMap");
                bc.BatchSize = 1000;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("IssueID", "IssueID");
                bc.ColumnMappings.Add("PubSubscriptionID", "PubSubscriptionID");
                bc.ColumnMappings.Add("SubGenSubscriberID", "SubGenSubscriberID");

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
        public static bool ValidateArchive(int pubId, int issueId, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Issue_ValidateArchive";
            cmd.Parameters.Add(new SqlParameter("@pubid", pubId));
            cmd.Parameters.Add(new SqlParameter("@issueid", issueId));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToBoolean(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool RollBackIssue(int pubId, int issueId, int origIMB, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Issue_Rollback";
            cmd.Parameters.Add(new SqlParameter("@imbSeq", origIMB));
            cmd.Parameters.Add(new SqlParameter("@pubid", pubId));
            cmd.Parameters.Add(new SqlParameter("@issueid", issueId));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToBoolean(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
