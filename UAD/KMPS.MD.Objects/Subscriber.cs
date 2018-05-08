using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using KM.Common;
using KMPlatform.Object;
using UADSubscriber = FrameworkUAD.DataAccess.Subscriber;

namespace KMPS.MD.Objects
{
    public class Subscriber
    {
        public Subscriber()
        {
        }
        
        #region Properties
        public int SubscriptionID { get; set; }
        public string Magazine_Name { get; set; }
        public int Sequence { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string MailStop { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Plus4 { get; set; }
        public string ForZip { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public int? CountryID { get; set; }
        public string Phone { get; set; }
        public bool PhoneExists { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public bool FaxExists { get; set; }
        public string Email { get; set; }
        public bool EmailExists { get; set; }
        public int? CategoryID { get; set; }
        public int? TransactionID { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime QDate { get; set; }
        public int? QSourceID { get; set; }
        public string RegCode { get; set; }
        public string Verified { get; set; }
        public string Subsrc { get; set; }
        public string Origssrc { get; set; }
        public string Par3c { get; set; }
        public bool? MailPermission { get; set; }
        public bool? FaxPermission { get; set; }
        public bool? PhonePermission { get; set; }
        public bool? OtherProductsPermission { get; set; }
        public bool? ThirdPartyPermission { get; set; }
        public bool? EmailRenewPermission { get; set; }
        public bool? TextPermission { get; set; }
        //public bool Employ { get; set; }
        //public string Sales { get; set; }
        public string Source { get; set; }
        public string Priority { get; set; }
        public int IGRP_CNT { get; set; }
        public string Demo7 { get; set; }
        //private string _EmailAddress;
        public int SubscriberID { get; set; }
        public int Score { get; set; }
        public string TransactionCodeName { get; set; }
        public string QSourceName { get; set; }
        public string Notes { get; set; }
        public bool IsMailable { get; set; }
        public bool IsLatLonValid { get; set; }
        public Guid IGrp_No { get; set; }
        public int UpdatedByUserID { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        #endregion

        #region Data

        public static bool ExistsStandardFieldName(KMPlatform.Object.ClientConnections clientconnection, string Name)
        {
            SqlCommand cmd = new SqlCommand("select 1 from sysobjects so join syscolumns sc on so.id = sc.id where (so.name = 'Subscriptions'  or so.name = 'PubSubscriptions'  or so.name = 'QSource' or so.name = 'Transaction' or so.name = 'TransactionGroup' or so.name = 'EmailStatus') and sc.name = @Name");
            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            cmd.CommandType = CommandType.Text;
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static List<Subscriber> Get(KMPlatform.Object.ClientConnections clientconnection, int SubscriptionID, int BrandID)
        {
            List<Subscriber> retList = new List<Subscriber>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            string sqlQuery = string.Empty;
            if (BrandID == 0)
                sqlQuery = "select s.*, tc.TransactionCodeName as TransactionName, c.DisplayName  as QSourceName  from subscriptions s with (nolock) left outer join  UAD_Lookup..Code c with (nolock) on c.CodeID = s.QSourceID left outer join UAD_Lookup..[TransactionCode] tc with (nolock) on tc.TransactionCodeID =  s.TransactionID where subscriptionID =" + SubscriptionID;
            else
                sqlQuery = "select distinct s.*, bs.Score, tc.TransactionCodeName as TransactionName, c.DisplayName  as QSourceName  from subscriptions s  with (nolock) left outer join  UAD_Lookup..Code c with (nolock) on c.CodeID = s.QSourceID left outer join UAD_Lookup..[TransactionCode] tc with (nolock) on tc.TransactionCodeID =  s.TransactionID join PubSubscriptions ps with (nolock) on ps.SubscriptionID = s.SubscriptionID join BrandDetails bd with (nolock) on ps.PubID = bd.PubID left outer join brandscore bs with (nolock) on bs.subscriptionID = s.subscriptionID and bs.BrandId = @brandID where bd.BrandId = @brandID and s.subscriptionID = " + SubscriptionID;

            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            cmd.CommandType = CommandType.Text;

            if (BrandID != 0)
            {
                cmd.Parameters.AddWithValue("@brandID", BrandID);
            }

            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Subscriber> builder = DynamicBuilder<Subscriber>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Subscriber x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        public static DataTable GetSubscriberData(
            ClientConnections clientConnections,
            StringBuilder queries,
            List<string> standardColumnsList,
            List<string> masterGroupColumns,
            List<string> masterGroupDescColumns, 
            List<string> subscriptionsExtMapperColumns,
            List<string> customColumnList,
            int brandId,
            List<int> productIds,
            bool isMostRecentData,
            int downloadCount,
            string subscriptionIds = "")
        {
            return UADSubscriber.GetSubscriberData(
                clientConnections,
                queries,
                standardColumnsList,
                masterGroupColumns,
                masterGroupDescColumns,
                subscriptionsExtMapperColumns,
                customColumnList,
                brandId,
                productIds,
                isMostRecentData,
                downloadCount,
                subscriptionIds);
        }

        public static DataTable GetProductDimensionSubscriberData(KMPlatform.Object.ClientConnections clientConnection, StringBuilder queries, List<string> standardColumnsList, List<int> pubIDs, List<string> responseGroupIDs, List<string> responseGroupDescIDs,  List<string> pubSubscriptionsExtMapperColumns, List<string> customColumnList, int brandId, int downloadCount)
        {
            var sdoc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(standardColumnsList, "StandardFields", "StandardField", true, true, true);
            var custodoc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(customColumnList, "CustomFields", "CustomField", true, true);
            var doc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(responseGroupIDs, "ResponseGroups", "ResponseGroup");
            var docDesc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(responseGroupDescIDs, "ResponseGroups", "ResponseGroup", true);
            var pubSubExtMapperDoc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(pubSubscriptionsExtMapperColumns, "PubSubscriptionsExtMapperValues", "PubSubscriptionsExtMapperValue", true);
            
            var conn = DataFunctions.GetClientSqlConnection(clientConnection);
            return FrameworkUAD.DataAccess.Subscriber.ExecuteGetProductDimensionSubscriberData(
                conn, 
                "sp_GetProductDimensionSubscriberData", 
                queries.ToString(), 
                sdoc.ToString(), 
                pubIDs.First(), 
                null, 
                doc.ToString(), 
                docDesc.ToString(), 
                pubSubExtMapperDoc.ToString(), 
                string.Join(",", custodoc.ToString()), 
                brandId, 
                downloadCount);
        }

        public static DataTable GetSubscriberData(KMPlatform.Object.ClientConnections clientConnection, StringBuilder queries, string fields)
        {
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientConnection);
            SqlCommand cmd = new SqlCommand("spDownloaddetails", conn);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Queries", SqlDbType.Text)).Value = queries.ToString();
            cmd.Parameters.Add(new SqlParameter("@Fields", SqlDbType.VarChar)).Value = fields;
            cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar)).Value = string.Empty;
            return DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(clientConnection));
        }

        public static DataTable GetSubscriberData_CLV(KMPlatform.Object.ClientConnections clientconnection, StringBuilder queries, List<string> standardColumnsList, List<string> masterGroupColumns, List<string> masterGroupDescColumns,  List<string> subscriptionsExtMapperColumns, List<string> customColumnList, int brandId, List<int> pubIDs, bool uniquedownload, bool isMostRecentData)
        {
            var sdoc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(standardColumnsList, "StandardFields", "StandardField", true, true, true);
            var custodoc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(customColumnList, "CustomFields", "CustomField", true, true);
            var doc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(masterGroupColumns, "MasterGroups", "MasterGroup");
            var docDesc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(masterGroupDescColumns, "MasterGroups", "MasterGroup", true);
            var subExtMapperDoc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(subscriptionsExtMapperColumns, "SubscriptionsExtMapperValues", "SubscriptionsExtMapperValue", true);

            var conn = DataFunctions.GetClientSqlConnection(clientconnection);
            return FrameworkUAD.DataAccess.Subscriber.ExecuteGetSubscriberData(
                conn, 
                isMostRecentData ? "sp_GetSubscriberData_RecentConsensus_CLV" : "sp_GetSubscriberData_CLV",
                queries.ToString(), 
                sdoc.ToString(), 
                doc.ToString(), 
                docDesc.ToString(), 
                subExtMapperDoc.ToString(), 
                custodoc.ToString(), 
                brandId, 
                pubIDs == null ? String.Empty : string.Join(",", pubIDs), 
                null, 
                isMostRecentData, 
                null,
                uniquedownload);
        }

        public static DataTable GetSubscriberData_EV(KMPlatform.Object.ClientConnections clientConnection, StringBuilder queries, List<string> standardColumnsList, List<string> masterGroupColumns, List<string> masterGroupDescColumns, List<string> subscriptionsExtMapperColumns, List<string> customColumnList, int brandId, List<int> pubIDs, bool uniquedownload, bool isMostRecentData)
        {
            var sdoc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(standardColumnsList, "StandardFields", "StandardField", true, true, true);
            var custodoc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(customColumnList, "CustomFields", "CustomField", true, true);
            var doc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(masterGroupColumns, "MasterGroups", "MasterGroup");
            var docDesc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(masterGroupDescColumns, "MasterGroups", "MasterGroup", true);
            var subExtMapperDoc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(subscriptionsExtMapperColumns, "SubscriptionsExtMapperValues", "SubscriptionsExtMapperValue", true);
            
            var conn = DataFunctions.GetClientSqlConnection(clientConnection);
            return FrameworkUAD.DataAccess.Subscriber.ExecuteGetSubscriberData(
                conn, 
                isMostRecentData ? "sp_GetSubscriberData_RecentConsensus_EV" : "sp_GetSubscriberData_EV ",
                queries.ToString(), 
                sdoc.ToString(), 
                doc.ToString(), 
                docDesc.ToString(), 
                subExtMapperDoc.ToString(), 
                custodoc.ToString(), 
                brandId, 
                pubIDs == null ? String.Empty : string.Join(",", pubIDs), 
                null, 
                isMostRecentData, 
                null,
                uniquedownload);
        }

        public static DataTable GetProductDimensionSubscriberData_CLV(KMPlatform.Object.ClientConnections clientConnection, StringBuilder queries, List<string> standardColumnsList, List<int> pubIDs, List<string> responseGroupIDs, List<string> responseGroupDescIDs, List<string> pubSubscriptionsExtMapperColumns, List<string> customColumnList, int brandId, bool uniquedownload)
        {
            var sdoc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(standardColumnsList, "StandardFields", "StandardField", true, true, true);
            var custodoc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(customColumnList, "CustomFields", "CustomField", true, true);
            var doc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(responseGroupIDs, "ResponseGroups", "ResponseGroup");
            var docDesc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(responseGroupDescIDs, "ResponseGroups", "ResponseGroup", true);
            var pubSubExtMapperDoc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(pubSubscriptionsExtMapperColumns, "PubSubscriptionsExtMapperValues", "PubSubscriptionsExtMapperValue", true);
            
            var conn = DataFunctions.GetClientSqlConnection(clientConnection);
            return FrameworkUAD.DataAccess.Subscriber.ExecuteGetProductDimensionSubscriberData(
                conn, 
                "sp_GetProductDimensionSubscriberData_CLV", 
                queries.ToString(), 
                sdoc.ToString(), 
                pubIDs.First(), 
                null, 
                doc.ToString(), 
                docDesc.ToString(), 
                pubSubExtMapperDoc.ToString(), 
                custodoc.ToString(), 
                brandId, 
                null, 
                uniquedownload);
        }

        public static DataTable GetProductDimensionSubscriberData_EV(KMPlatform.Object.ClientConnections clientConnection, StringBuilder queries, List<string> standardColumnsList, List<int> pubIDs, List<string> responseGroupIDs, List<string> responseGroupDescIDs, List<string> pubSubscriptionsExtMapperColumns, List<string> customColumnList, int brandId, bool uniquedownload)
        {
            var sdoc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(standardColumnsList, "StandardFields", "StandardField", true, true, true);
            var custodoc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(customColumnList, "CustomFields", "CustomField", true, true);
            var doc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(responseGroupIDs, "ResponseGroups", "ResponseGroup");
            var docDesc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(responseGroupDescIDs, "ResponseGroups", "ResponseGroup", true);
            var pubSubExtMapperDoc = FrameworkUAD.DataAccess.Subscriber.CreateFieldsXDoc(pubSubscriptionsExtMapperColumns, "PubSubscriptionsExtMapperValues", "PubSubscriptionsExtMapperValue", true);
            
            var conn = DataFunctions.GetClientSqlConnection(clientConnection);
            return FrameworkUAD.DataAccess.Subscriber.ExecuteGetProductDimensionSubscriberData(
                conn,
                "sp_GetProductDimensionSubscriberData_EV",
                queries.ToString(),
                sdoc.ToString(),
                pubIDs.First(),
                null,
                doc.ToString(),
                docDesc.ToString(),
                pubSubExtMapperDoc.ToString(),
                custodoc.ToString(),
                brandId,
                null,
                uniquedownload);
        }

        public static int GetUniqueLocationsCount(KMPlatform.Object.ClientConnections clientconnection, StringBuilder Queries)
        {
            SqlCommand cmd = new SqlCommand("sp_GetSubscriberUniqueLocationCount");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Queries", Queries.ToString()));
            cmd.CommandTimeout = 0;
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))); 
        }

        public static int GetUniqueEmailsCount(KMPlatform.Object.ClientConnections clientconnection, StringBuilder Queries)
        {
            SqlCommand cmd = new SqlCommand("sp_GetSubscriberUniqueEmailsCount");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Queries", Queries.ToString()));
            cmd.CommandTimeout = 0;
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static DataTable GetSubscriptionExportColumns(KMPlatform.Object.ClientConnections clientconnection)
        {
            return DataFunctions.getDataTable("select sc.name from sysobjects so join syscolumns sc on so.id = sc.id where so.name = 'Subscriptions' and sc.name not in ('EMAIL','FNAME','LNAME','COMPANY','TITLE','ADDRESS','MAILSTOP','CITY','STATE','ZIP','PLUS4','COUNTRY','FORZIP','PHONE','MOBILE','FAX','CategoryID','TransactionID','pubids', 'MailPermission', 'FaxPermission', 'PhonePermission', 'OtherProductsPermission', 'ThirdPartyPermission', 'EmailRenewPermission', 'TextPermission','SubscriptionID', 'SEQUENCE', 'CountryID','IsExcluded','PubID','STATLIST','IGRP_CNT','CGRP_NO','CGRP_CNT', 'IGRP_Rank', 'CGRP_Rank', 'EmailExists','FaxExists','PhoneExists','PRIORITY','Origssrc','SALES','Source','subsrc','Latitude','Longitude','IsLatLonValid','LatLonMsg','verified','Employ', 'Sales', 'SIC', 'SICCODE', 'StatusUpdatedDate', 'StatusUpdatedReason', 'EmailStatusID', 'Score', 'QDate', 'Notes', 'Demo7', 'AddressupdatedSourceTypeCodeID', 'AddressTypeCodeId', 'AddressLastUpdatedDate', 'CreatedByuserID', 'UpdatedByUserID', 'QSourceID', 'Par3C') order by sc.name", DataFunctions.GetClientSqlConnection(clientconnection));
        }

        public static DataTable GetSubScheduledReportExportColumns(KMPlatform.Object.ClientConnections clientconnection)
        {
            return DataFunctions.getDataTable("select sc.name from sysobjects so join syscolumns sc on so.id = sc.id where so.name = 'Subscriptions' and sc.name not in ('SubscriptionID', 'SEQUENCE', 'CountryID','IsExcluded','PubID','STATLIST','IGRP_CNT','CGRP_NO','CGRP_CNT', 'EmailExists','FaxExists','PhoneExists','PRIORITY','Origssrc','SALES','Source','subsrc','Latitude','Longitude','IsLatLonValid','LatLonMsg','verified','Employ', 'Sales', 'SIC', 'SICCODE', 'StatusUpdatedDate', 'StatusUpdatedReason', 'EmailStatusID', 'Notes', 'Demo7', 'AddressupdatedSourceTypeCodeID', 'AddressTypeCodeId', 'AddressLastUpdatedDate', 'CreatedByuserID', 'UpdatedByUserID', 'TransactionID', 'QSourceID') order by sc.name", DataFunctions.GetClientSqlConnection(clientconnection));
        }

        public static DataTable GetSubProductScheduledReportExportColumns(KMPlatform.Object.ClientConnections clientconnection)
        {
            return DataFunctions.getDataTable("select sc.name from sysobjects so join syscolumns sc on so.id = sc.id where so.name = 'Subscriptions' and sc.name not in ('SubscriptionID', 'SEQUENCE', 'CountryID','IsExcluded','PubID','STATLIST','IGRP_CNT','CGRP_NO','CGRP_CNT', 'EmailExists','FaxExists','PhoneExists','PRIORITY','Origssrc','SALES','Source','subsrc','Latitude','Longitude','IsLatLonValid','LatLonMsg','verified','Employ', 'Sales', 'SIC', 'SICCODE', 'StatusUpdatedDate', 'StatusUpdatedReason', 'EmailStatusID', 'Notes', 'AddressupdatedSourceTypeCodeID', 'AddressTypeCodeId', 'AddressLastUpdatedDate', 'CreatedByuserID', 'UpdatedByUserID', 'TransactionID', 'QSourceID') order by sc.name", DataFunctions.GetClientSqlConnection(clientconnection));
        }

        public static Subscriber GetByIGrp_No(KMPlatform.Object.ClientConnections clientconnection, Guid IGrp_No)
        {
            Subscriber retItem = new Subscriber();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_Subscription_Select_IGrp_No", conn);
            cmd.Parameters.Add(new SqlParameter("@IGrp_No", IGrp_No));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Subscriber> builder = DynamicBuilder<Subscriber>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    retItem = builder.Build(rdr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retItem;
        }
        #endregion

        #region CRUD
        public void Save(KMPlatform.Object.ClientConnections clientconnection)
        {
            SqlCommand cmd = new SqlCommand("sp_SaveSubscriptions");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@SubscriptionID", SubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@FName", (object)FName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LName", (object)LName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Company", (object)Company ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Address", (object)Address ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Address3", (object)Address3 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@City", (object)City ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@State", (object)State ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Zip", (object)Zip ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CountryID", (object)CountryID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Title", (object)Title ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ForZip", (object)ForZip ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Plus4", (object)Plus4 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Phone", (object)Phone ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Fax", (object)Fax ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Mobile", (object)Mobile ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Email", (object)Email ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MailStop", (object)MailStop ?? DBNull.Value));
            //cmd.Parameters.Add(new SqlParameter("@QDate", (object)QDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MailPermission", (object)MailPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FaxPermission", (object)FaxPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PhonePermission", (object)PhonePermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@OtherProductsPermission", (object)OtherProductsPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ThirdPartyPermission", (object)ThirdPartyPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailRenewPermission", (object)EmailRenewPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@TextPermission", (object)TextPermission ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Notes", (object)Notes ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", UpdatedByUserID));
            DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
            return; 
        }

        public static void UpdateAddress(KMPlatform.Object.ClientConnections clientconnection, Subscriber s, bool overWrite)
        {
            SqlCommand cmd = new SqlCommand("e_Subscriptions_Update_Address");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@SubscriptionID", s.SubscriptionID));
            cmd.Parameters.Add(new SqlParameter("@Address", (object)s.Address ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MailStop", (object)s.MailStop ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Address3", (object)s.Address3 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@City", (object)s.City ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@State", (object)s.State ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Zip", (object)s.Zip ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CountryID", (object)s.CountryID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Phone", (object)s.Phone ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Fax", (object)s.Fax ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Email", (object)s.Email ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", s.UpdatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@OverWrite", overWrite));
            DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
            return;
        }

        public static void Merge(KMPlatform.Object.ClientConnections clientconnection, int subscriptionIDToKeep, int subscriptionIDToRemove, XDocument pubSubscriptionIDToKeep, XDocument pubSubscriptionIDToRemove, int userID)
        {
            SqlCommand cmd = new SqlCommand("sp_MergeSubscribers");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@SubscriptionIDToKeep", subscriptionIDToKeep));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionIDToRemove", subscriptionIDToRemove));
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionIDToKeep", pubSubscriptionIDToKeep.ToString()));
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionIDToRemove", pubSubscriptionIDToRemove.ToString()));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
            return; 
        }
        #endregion
    }
}