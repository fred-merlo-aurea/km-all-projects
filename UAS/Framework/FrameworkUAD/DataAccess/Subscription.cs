using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class Subscription
    {
        public static bool ExistsStandardFieldName(string Name, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select 1 from sysobjects so join syscolumns sc on so.id = sc.id where (so.name = 'Subscriptions'  or so.name = 'PubSubscriptions'  or so.name = 'QSource' or so.name = 'Transaction' or so.name = 'TransactionGroup' or so.name = 'EmailStatus') and sc.name = @Name";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }
        public static Entity.Subscription Select(int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_Select_SubscriptionID";
            cmd.Parameters.AddWithValue("@SubscriptionID", subscriptionID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Get(cmd);
        }
        public static List<Entity.Subscription> Select(string email, KMPlatform.Object.ClientConnections client, bool isClientService = false)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_Select_Email";
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd, isClientService);
        }
        public static List<Entity.Subscription> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static List<Entity.Subscription> SelectInValidAddresses(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscriptions_GetInValidLatLon";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static List<int> SelectIDs(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_SelectIDs";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetIntList(cmd);
        }
        public static List<Entity.Subscription> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_SelectForFileAudit";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@StartDate", (object)startDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", (object)endDate ?? DBNull.Value);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static DataTable FindMatches(int productID, string fname, string lname, string company, string address, string state, string zip, string phone, string email, string title, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dt_Subscribers_Match";
            cmd.Parameters.Add(new SqlParameter("@PubID", productID));
            cmd.Parameters.Add(new SqlParameter("@Firstname", fname));
            cmd.Parameters.Add(new SqlParameter("@Lastname", lname));
            cmd.Parameters.Add(new SqlParameter("@Company", company));
            cmd.Parameters.Add(new SqlParameter("@Address", address));
            cmd.Parameters.Add(new SqlParameter("@State", state));
            cmd.Parameters.Add(new SqlParameter("@zip", zip));
            cmd.Parameters.Add(new SqlParameter("@Phone", phone));
            cmd.Parameters.Add(new SqlParameter("@Email", email));
            cmd.Parameters.Add(new SqlParameter("@Title", productID));

            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
        }
        public static bool AddressUpdate(string xml, KMPlatform.Object.ClientConnections client)
        {
            bool done = true;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Subscription_AddressUpdate";
                cmd.Parameters.Add(new SqlParameter("@xml", xml.ToString()));
                cmd.Connection = DataFunctions.GetClientSqlConnection(client);

                done = KM.Common.DataFunctions.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                done = false;
                throw ex;
            }
            return done;
        }

        public static int Save(Entity.Subscription subscription, KMPlatform.Object.ClientConnections client)
        {
            var cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "e_Subscription_Save",
                Connection = DataFunctions.GetClientSqlConnection(client)
            };

            foreach (var prop in typeof(Entity.Subscription).GetProperties())
            {
                cmd.Parameters.AddWithValue($"@{prop.Name}", (object)prop.GetValue(subscription) ?? DBNull.Value);
            }

            var phoneExists = !string.IsNullOrWhiteSpace(subscription.Phone);
            var faxExists = !string.IsNullOrWhiteSpace(subscription.Fax);
            var emailExists = !string.IsNullOrWhiteSpace(subscription.Email);

            cmd.Parameters.AddWithValue("@PhoneExists", phoneExists);
            cmd.Parameters.AddWithValue("@FaxExists", faxExists);
            cmd.Parameters.AddWithValue("@EmailExists", emailExists);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static bool NcoaUpdateAddress(string xml, KMPlatform.Object.ClientConnections client, int userId, int SourceFileID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "job_NCOA_AddressUpdate";
            cmd.Parameters.AddWithValue("@xml", xml);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@SourceFileID", SourceFileID);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        #region Circ Merge
        public static int UpdateQDate(int SubscriptionID, DateTime? QSourceDate, int UpdatedByUserID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_Update_QDate";
            cmd.Parameters.Add(new SqlParameter("@SubscriptionID", SubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@QSourceDate", (object)QSourceDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", UpdatedByUserID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static List<Entity.Subscription> Search(KMPlatform.Object.ClientConnections client, string clientDisplayName, string fName = "", string lName = "", string company = "", string title = "", string add1 = "", string city = "", string regionCode = "", string zip = "", string country = "", string email = "", string phone = "", int sequenceID = 0, string account = "", int publisherId = 0, int publicationId = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_Search_Params";
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
            cmd.Parameters.AddWithValue("@clientDisplayName", clientDisplayName);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.Subscription> SearchSuggestMatch(KMPlatform.Object.ClientConnections client,int publisherId, int publicationId, string firstName = "", string lastName = "", string email = "")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_SuggestMatch";
            cmd.Parameters.AddWithValue("@PublisherID", publisherId);
            cmd.Parameters.AddWithValue("@PublicationID", publicationId);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.Subscription> SelectPaging(int page, int pageSize, int productID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_Select_ProductID_Paging";
            cmd.Parameters.AddWithValue("@CurrentPage", page);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }

        public static int UpdateSubscription(int SubscriptionID, bool IsLocked, int UserID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_Update";
            cmd.Parameters.Add(new SqlParameter("@SubscriptionID", SubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@IsLocked", IsLocked));
            cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static int DeleteSubscription(int SubscriptionID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_Delete_SubscriptionID";
            cmd.Parameters.Add(new SqlParameter("@SubscriptionID", SubscriptionID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static List<Entity.Subscription> SelectPublication(int productID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_Select_PublicationID";
            cmd.Parameters.AddWithValue("@PublicationID", productID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }

        public static bool ClearWaveMailingInfo(int waveMailingID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_ClearWaveMailingInfo";
            cmd.Parameters.AddWithValue("@WaveMailingID", waveMailingID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool SaveBulkWaveMailing(string xml, int waveMailingID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_BulkUpdate_WaveMailing";
            cmd.Parameters.Add(new SqlParameter("@SubscriptionXML", xml));
            cmd.Parameters.Add(new SqlParameter("@WaveMailingID", waveMailingID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        
        public static List<Entity.Subscription> SelectSequence(int SequenceID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_Select_SequenceID";
            cmd.Parameters.AddWithValue("@SequenceID", SequenceID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static int SelectCount(int productID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_Select_ProductID_Count";
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static List<Entity.Subscription> SearchAddressZip(string address1, string zipCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_SearchAddressZip";
            cmd.Parameters.AddWithValue("@Address1", address1);
            cmd.Parameters.AddWithValue("@ZipCode", zipCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static bool SaveBulkActionIDUpdate(string xml)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_BulkUpdate_ActionIDs";
            cmd.Parameters.Add(new SqlParameter("@SubscriptionXML", xml));

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        
        #endregion

        public static Entity.Subscription Get(SqlCommand cmd)
        {
            Entity.Subscription retItem = null;

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Subscription();
                        DynamicBuilder<Entity.Subscription> builder = DynamicBuilder<Entity.Subscription>.CreateBuilder(rdr);
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
        public static List<Entity.Subscription> GetList(SqlCommand cmd, bool isClientService = false)
        {
            List<Entity.Subscription> retList = new List<Entity.Subscription>();

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.Subscription retItem = new Entity.Subscription(isClientService);
                        DynamicBuilder<Entity.Subscription> builder = DynamicBuilder<Entity.Subscription>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            retItem.IsClientService = isClientService;
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
            try
            {
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
