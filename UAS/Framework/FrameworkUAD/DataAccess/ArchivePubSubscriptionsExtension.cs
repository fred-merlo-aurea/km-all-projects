using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FrameworkUAD.DataAccess
{
    public class ArchivePubSubscriptionsExtension
    {
        public static List<Entity.ArchivePubSubscriptionsExtension> SelectForUpdate(int productID, int issueid, string pubsubs, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ArchivePubSubscriptionsExtension_SelectForUpdate";
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Parameters.AddWithValue("@IssueID", issueid);
            cmd.Parameters.AddWithValue("@PubSubs", pubsubs);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static DataTable GetArchiveAdhocs(KMPlatform.Object.ClientConnections client, int pubSubID, int pubID, int issueID)
        {
            DataTable retItem = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "o_ArchivePubSubscription_Adhocs";
            cmd.Parameters.AddWithValue("@PubSubs", pubSubID);
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.Parameters.AddWithValue("@IssueID", issueID);

            retItem = KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
            return retItem;
        }
        public static bool Save(KMPlatform.Object.ClientConnections client, int issueArchiveSubscriptionID, int pubID, List<FrameworkUAD.Object.PubSubscriptionAdHoc> adhocs)
        {
            string xml = "<XML>";
            foreach (FrameworkUAD.Object.PubSubscriptionAdHoc ah in adhocs)
            {
                xml += "<AdHoc><Name>" + ah.AdHocField + "</Name><Value>" + ah.Value + "</Value></AdHoc>";
            }
            xml += "</XML>";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SaveArchivePubSubscriptions_AdHocs";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@IssueArchiveSubscriptionID", issueArchiveSubscriptionID);
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.Parameters.AddWithValue("@AdHocs", xml);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static Entity.ArchivePubSubscriptionsExtension Get(SqlCommand cmd)
        {
            Entity.ArchivePubSubscriptionsExtension retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ArchivePubSubscriptionsExtension();
                        DynamicBuilder<Entity.ArchivePubSubscriptionsExtension> builder = DynamicBuilder<Entity.ArchivePubSubscriptionsExtension>.CreateBuilder(rdr);
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
        public static List<Entity.ArchivePubSubscriptionsExtension> GetList(SqlCommand cmd)
        {
            List<Entity.ArchivePubSubscriptionsExtension> retList = new List<Entity.ArchivePubSubscriptionsExtension>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.ArchivePubSubscriptionsExtension retItem = new Entity.ArchivePubSubscriptionsExtension();
                        DynamicBuilder<Entity.ArchivePubSubscriptionsExtension> builder = DynamicBuilder<Entity.ArchivePubSubscriptionsExtension>.CreateBuilder(rdr);
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
    }
}
