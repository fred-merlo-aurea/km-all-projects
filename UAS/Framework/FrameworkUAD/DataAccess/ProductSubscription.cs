using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class ProductSubscription
    {
        public static List<Entity.ProductSubscription> Select(int subscriptionID, KMPlatform.Object.ClientConnections client, string clientDisplayName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Select_SubscriptionID";
            cmd.Parameters.AddWithValue("@SubscriptionID", subscriptionID);
            cmd.Parameters.AddWithValue("@ClientDisplayName", clientDisplayName);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static Entity.ProductSubscription Select(Guid sfRecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Select_SFRecordIdentifier";
            cmd.Parameters.AddWithValue("@SFRecordIdentifier", sfRecordIdentifier);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Get(cmd);
        }
        public static Entity.ProductSubscription SelectSequenceIDPubID(int seqnum, int pubid, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Select_SequenceNum_PubID";
            cmd.Parameters.AddWithValue("@SequenceNum", seqnum);
            cmd.Parameters.AddWithValue("@PubID", pubid);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Get(cmd);
        }
        public static Entity.ProductSubscription SelectProductSubscription(int pubSubscriptionID, KMPlatform.Object.ClientConnections client, string clientDisplayName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Select_PubSubscriptionID";
            cmd.Parameters.AddWithValue("@PubSubscriptionID", pubSubscriptionID);
            cmd.Parameters.AddWithValue("@ClientDisplayName", clientDisplayName);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Get(cmd);
        }
        public static Entity.ProductSubscription SelectProcessCode(string processCode, KMPlatform.Object.ClientConnections client, string clientDisplayName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Select_ProcessCode";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@ClientDisplayName", clientDisplayName);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Get(cmd);
        }
        public static Entity.ProductSubscription Get(SqlCommand cmd)
        {
            Entity.ProductSubscription retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ProductSubscription();
                        DynamicBuilder<Entity.ProductSubscription> builder = DynamicBuilder<Entity.ProductSubscription>.CreateBuilder(rdr);
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
        public static List<Entity.ProductSubscription> GetList(SqlCommand cmd)
        {
            List<Entity.ProductSubscription> retList = new List<Entity.ProductSubscription>();

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.ProductSubscription retItem = new Entity.ProductSubscription();
                        DynamicBuilder<Entity.ProductSubscription> builder = DynamicBuilder<Entity.ProductSubscription>.CreateBuilder(rdr);
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
        private static List<int> GetIntList(SqlCommand cmd)
        {
            List<int> retList = new List<int>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        retList.Add(rdr.GetInt32(0));
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }
        public static bool Update_Requester_Flags(KMPlatform.Object.ClientConnections client, int productID, int issueID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PubSubscriptions_Update_ReqFlags";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@ProductID", productID));
            cmd.Parameters.Add(new SqlParameter("@IssueID", issueID));

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static int Save(Entity.ProductSubscription subscription, KMPlatform.Object.ClientConnections client)
        {
            var cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "e_ProductSubscription_Save",
                Connection = DataFunctions.GetClientSqlConnection(client)
            };

            var paramNameTransformations = new Dictionary<string, string>()
            {
                {"IMBSeq", "IMBSEQ" },
                {"Verify", "Verified" }
            };

            foreach (var prop in typeof(Entity.ProductSubscription).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var paramName = paramNameTransformations.ContainsKey(prop.Name)
                    ? paramNameTransformations[prop.Name]
                    : prop.Name;
                cmd.Parameters.AddWithValue($"@{paramName}", prop.GetValue(subscription) ?? DBNull.Value);
            }

            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static int UpdateQDate(int SubscriptionID, DateTime? QSourceDate, int UpdatedByUserID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Update_QDate";
            cmd.Parameters.Add(new SqlParameter("@SubscriptionID", SubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@QSourceDate", (object)QSourceDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", UpdatedByUserID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static List<Entity.ProductSubscription> Search(KMPlatform.Object.ClientConnections client, string clientDisplayName, string fName = "", string lName = "", string company = "", string title = "", string add1 = "", string city = "", string regionCode = "", string zip = "", string country = "", string email = "", string phone = "", int sequenceID = 0, string account = "", int publisherId = 0, int publicationId = 0,int subscriptionID = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Search_Params";
            cmd.Parameters.AddWithValue("@fName", fName);
            cmd.Parameters.AddWithValue("@lName", lName);
            cmd.Parameters.AddWithValue("@Company", company);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Add1", add1);
            cmd.Parameters.AddWithValue("@City", city);
            cmd.Parameters.AddWithValue("@RegionCode", regionCode);
            cmd.Parameters.AddWithValue("@Zip", zip);
            cmd.Parameters.AddWithValue("@Country", country);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@SequenceID", sequenceID);
            cmd.Parameters.AddWithValue("@AccountID", account);
            cmd.Parameters.AddWithValue("@PublisherID", publisherId);
            cmd.Parameters.AddWithValue("@PublicationID", publicationId);
            cmd.Parameters.AddWithValue("@ClientDisplayName", clientDisplayName);
            cmd.Parameters.AddWithValue("@SubscriptionID", subscriptionID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.ProductSubscription> SearchSuggestMatch(KMPlatform.Object.ClientConnections client, int publisherId, int publicationId, string firstName = "", string lastName = "", string email = "")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_SuggestMatch";
            cmd.Parameters.AddWithValue("@PublisherID", publisherId);
            cmd.Parameters.AddWithValue("@PublicationID", publicationId);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.ProductSubscription> SelectPaging(int page, int pageSize, int productID, KMPlatform.Object.ClientConnections client, string clientDisplayName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Select_ProductID_Paging";
            cmd.Parameters.AddWithValue("@CurrentPage", page);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Parameters.AddWithValue("@ClientDisplayName", clientDisplayName);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }

        public static int UpdateSubscription(int pubSubscriptionID, bool IsLocked, int UserID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Update";
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionID", pubSubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@IsLocked", IsLocked));
            cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static int DeleteSubscription(int SubscriptionID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Delete_ProductSubscriptionID";
            cmd.Parameters.Add(new SqlParameter("@SubscriptionID", SubscriptionID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static List<Entity.ProductSubscription> SelectPublication(int productID, KMPlatform.Object.ClientConnections client, string clientDisplayName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Select_PublicationID";
            cmd.Parameters.AddWithValue("@PublicationID", productID);
            cmd.Parameters.AddWithValue("@ClientDisplayName", clientDisplayName);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }

        public static bool ClearWaveMailingInfo(int waveMailingID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_ClearWaveMailingInfo";
            cmd.Parameters.AddWithValue("@WaveMailingID", waveMailingID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool ClearIMBSeq(int productID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Clear_IMB";
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool SaveBulkWaveMailing(string xml, int waveMailingID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_BulkUpdate_WaveMailing";
            cmd.Parameters.Add(new SqlParameter("@SubscriptionXML", xml));
            cmd.Parameters.Add(new SqlParameter("@WaveMailingID", waveMailingID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static List<Entity.ProductSubscription> SelectSequence(int SequenceID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Select_SequenceID";
            cmd.Parameters.AddWithValue("@SequenceID", SequenceID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.ProductSubscription> SelectSequence(string SequenceIdWhereClause, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Select_SequenceIdWhereClause";
            cmd.Parameters.AddWithValue("@SequenceIdWhereClause", SequenceIdWhereClause);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static int SelectCount(int productID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_Select_ProductID_Count";
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static List<Entity.ProductSubscription> SearchAddressZip(string address1, string zipCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscriber_SearchAddressZip";
            cmd.Parameters.AddWithValue("@Address1", address1);
            cmd.Parameters.AddWithValue("@ZipCode", zipCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static bool SaveBulkActionIDUpdate(string xml, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_BulkUpdate_ActionIDs";
            cmd.Parameters.Add(new SqlParameter("@SubscriptionXML", xml));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static DataTable Select_For_Export(int page, int pageSize, string columns, int productID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PubSubscriptions_Select_For_Export";
            cmd.Parameters.AddWithValue("@CurrentPage", page);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Parameters.AddWithValue("@Columns", columns);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
        }

        public static DataTable Select_For_Export_Static(int productID, string cols, string subs, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PubSubscriptions_Select_For_Export_Static";
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Parameters.AddWithValue("@Columns", cols);
            cmd.Parameters.AddWithValue("@Subs", subs);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
        }
        public static DataTable Select_For_Export_Static(int productID,int issueid, string cols, string subs, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueArchiveSubscription_SelectForExportStatic_IssueID";
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Parameters.AddWithValue("@Columns", cols);
            cmd.Parameters.AddWithValue("@Subs", subs);
            cmd.Parameters.AddWithValue("@IssueID", issueid);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
        }

        public static List<Entity.ProductSubscription> SelectForUpdate(int productID, int issueid, string pubsubs, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscription_SelectForUpdate";
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Parameters.AddWithValue("@IssueID", issueid);
            cmd.Parameters.AddWithValue("@PubSubs", pubsubs);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }

        public static bool RecordUpdate(string pubSubs, string changes, int issueid, int productid, int userid, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_RecordUpdate";
            cmd.Parameters.AddWithValue("@PubSubs", pubSubs);
            cmd.Parameters.AddWithValue("@Changes", changes);
            cmd.Parameters.AddWithValue("@IssueID", issueid);            
            cmd.Parameters.AddWithValue("@ProductID", productid);
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        #region Simple ProductSubscriptions
        public static List<Entity.ActionProductSubscription> GetListSimple(SqlCommand cmd)
        {
            List<Entity.ActionProductSubscription> retList = new List<Entity.ActionProductSubscription>();

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.ActionProductSubscription retItem = new Entity.ActionProductSubscription();
                        DynamicBuilder<Entity.ActionProductSubscription> builder = DynamicBuilder<Entity.ActionProductSubscription>.CreateBuilder(rdr);
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
        public static List<Entity.CopiesProductSubscription> GetListSimpleCopies(SqlCommand cmd)
        {
            List<Entity.CopiesProductSubscription> retList = new List<Entity.CopiesProductSubscription>();

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.CopiesProductSubscription retItem = new Entity.CopiesProductSubscription();
                        DynamicBuilder<Entity.CopiesProductSubscription> builder = DynamicBuilder<Entity.CopiesProductSubscription>.CreateBuilder(rdr);
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
        public static List<Entity.ActionProductSubscription> SelectProductID(int PubID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ActionProductSubscription_SelectByProductID";
            cmd.Parameters.AddWithValue("@ProductID", PubID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetListSimple(cmd);
        }
        public static List<Entity.ActionProductSubscription> SelectProductID(int PubID, int issueID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ActionProductSubscription_SelectByProductID_IssueID";
            cmd.Parameters.AddWithValue("@ProductID", PubID);
            cmd.Parameters.AddWithValue("@IssueID", issueID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetListSimple(cmd);
        }
        public static List<Entity.CopiesProductSubscription> SelectAllActiveIDs(int productID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscriptions_Select_AllActiveIDs";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ProductID", productID);

            return GetListSimpleCopies(cmd);
        }
        #endregion

        #region DQM
        public static bool UpdateMasterDB(KMPlatform.Object.ClientConnections client, string processCode)
        {
            bool done = true;
            try
            {
                SqlConnection sqlConn = DataFunctions.GetClientSqlConnection(client);
                SqlCommand cmd = new SqlCommand("e_ImportFromUAS", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProcessCode", processCode);
                cmd.CommandTimeout = 0;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                done = false;
                string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                fl.Save(new FrameworkUAS.Entity.FileLog(-1, -1, message, processCode));
            }

            return done;
        }
        public static bool DedupeMasterDB(KMPlatform.Object.ClientConnections client, string processCode)
        {
            bool done = true;
            try
            {
                SqlConnection sqlConn = DataFunctions.GetClientSqlConnection(client);
                SqlCommand cmd = new SqlCommand("e_DupeCleanUp", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                done = false;
                string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                fl.Save(new FrameworkUAS.Entity.FileLog(-1, -1, message, processCode));
            }

            return done;
        }
        public static bool CountryRegionCleanse(int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_CountryRegionCleanse";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        #endregion
    }
}
