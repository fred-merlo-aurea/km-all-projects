using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using KM.Common;
using KMPlatform.Object;

namespace FrameworkUAD.DataAccess
{
    public class Subscriber
    {
        private const string GetSubscriberDataRecentConsensusCommandText = "sp_GetSubscriberData_RecentConsensus";
        private const string GetSubscriberDataCommandText = "sp_GetSubscriberData";
        private const string ElementStandardfield = "StandardField";
        private const string ElementCustomfield = "CustomField";
        private const string ElementMastergroup = "MasterGroup";
        private const string ElementSubscriptionsExtMapperValue = "SubscriptionsExtMapperValue";
        private const string RootElementStandardFields = "StandardFields";
        private const string RootElementCustomFields = "CustomFields";
        private const string RootElementMasterGroups = "MasterGroups";
        private const string RootElementSubscriptionsExtMapperValues = "SubscriptionsExtMapperValues";

        public static bool ExistsStandardFieldName(KMPlatform.Object.ClientConnections clientconnection, string Name)
        {
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select 1 from sysobjects so join syscolumns sc on so.id = sc.id where (so.name = 'Subscriptions'  or so.name = 'PubSubscriptions'  or so.name = 'QSource' or so.name = 'Transaction' or so.name = 'TransactionGroup' or so.name = 'EmailStatus') and sc.name = @Name");
            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            cmd.CommandType = CommandType.Text;
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, conn.ConnectionString)) > 0 ? true : false;
        }

        public static List<Object.Subscriber> Get(KMPlatform.Object.ClientConnections clientconnection, int SubscriptionID, int BrandID)
        {
            List<Object.Subscriber> retList = new List<Object.Subscriber>();
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
                DynamicBuilder<Object.Subscriber> builder = DynamicBuilder<Object.Subscriber>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Object.Subscriber x = builder.Build(rdr);
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
            var sdoc = CreateFieldsXDoc(standardColumnsList, RootElementStandardFields, ElementStandardfield, true, true, true);
            var custodoc = CreateFieldsXDoc(customColumnList, RootElementCustomFields, ElementCustomfield, true, true);
            var doc = CreateFieldsXDoc(masterGroupColumns, RootElementMasterGroups, ElementMastergroup);
            var docDesc = CreateFieldsXDoc(masterGroupDescColumns, RootElementMasterGroups, ElementMastergroup, true);
            var subExtMapperDoc = CreateFieldsXDoc(subscriptionsExtMapperColumns, RootElementSubscriptionsExtMapperValues, ElementSubscriptionsExtMapperValue, true);
            
            var conn = DataFunctions.GetClientSqlConnection(clientConnections);
            return ExecuteGetSubscriberData(
                conn,
                isMostRecentData ? GetSubscriberDataRecentConsensusCommandText : GetSubscriberDataCommandText,
                queries.ToString(),
                sdoc.ToString(),
                doc.ToString(),
                docDesc.ToString(),
                subExtMapperDoc.ToString(),
                custodoc.ToString(),
                brandId,
                productIds == null ? string.Empty : string.Join(",", productIds),
                downloadCount,
                isMostRecentData,
                subscriptionIds);
        }

        public static DataTable GetProductDimensionSubscriberData(KMPlatform.Object.ClientConnections clientConnection, StringBuilder queries, List<string> standardColumnsList, List<int> pubIDs, List<string> responseGroupIDs, List<string> responseGroupDescIDs, List<string> pubSubscriptionsExtMapperColumns, List<string> customColumnList, int brandId, int downloadCount, bool isFilterBased = true, List<int> subscriberIds = null)
        {
            var sdoc = CreateFieldsXDoc(standardColumnsList, "StandardFields", "StandardField", true, true, true);
            var custodoc = CreateFieldsXDoc(customColumnList, "CustomFields", "CustomField", true, true);
            var doc = CreateFieldsXDoc(responseGroupIDs, "ResponseGroups", "ResponseGroup");
            var docDesc = CreateFieldsXDoc(responseGroupDescIDs, "ResponseGroups", "ResponseGroup", true);
            var pubSubExtMapperDoc = CreateFieldsXDoc(pubSubscriptionsExtMapperColumns, "PubSubscriptionsExtMapperValues", "PubSubscriptionsExtMapperValue", true);
            
            string docSubscribers = "";
            if (subscriberIds != null)
            {
                docSubscribers = "<XML>";
                subscriberIds.ForEach(x => docSubscribers += "<S><ID>" + x.ToString() + "</ID></S>");
                docSubscribers += "</XML>";
            }

            var conn = DataFunctions.GetClientSqlConnection(clientConnection);
            return ExecuteGetProductDimensionSubscriberData(
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
                downloadCount,
                null,
                isFilterBased,
                docSubscribers);
        }

        public static XDocument CreateFieldsXDoc(List<string> columnsList, string rootElementName, string elementName, bool addCase = false, bool addDisplayName = false, bool processDisplayName = false)
        {
            if (columnsList == null)
            {
                throw new ArgumentNullException(nameof(columnsList));
            }

            var doc = new XDocument();
            var root = new XElement(rootElementName);

            foreach (var column in columnsList)
            {
                var element = new XElement(elementName);

                if (addDisplayName && processDisplayName)
                {
                    if (column.Contains(" as "))
                    {
                        element.Add(new XElement("Column", column.Split(new[] {" as "}, StringSplitOptions.None)[0]));
                        element.Add(new XElement("DisplayName", column.Split(new[] {" as "}, StringSplitOptions.None)[1].Split('|')[0]));
                    }
                    else
                    {
                        element.Add(new XElement("Column", column.Split('|')[0]));
                        element.Add(new XElement("DisplayName", column.Split('|')[0].Split('.')[1]));
                    }
                }
                else
                {
                    element.Add(new XElement("Column", column.Split('|')[0]));
                    
                    if (addDisplayName)
                    {
                        element.Add(new XElement("DisplayName", column.Split('|')[0]));
                    }
                }

                if (addCase)
                {
                    element.Add(new XElement("Case", column.Split('|')[1]));
                }
                
                root.Add(element);
            }

            doc.Add(root);
            return doc;
        }

        public static DataTable GetArchivedProductDimensionSubscriberData(KMPlatform.Object.ClientConnections clientConnection, string query, List<string> standardColumnsList, List<int> pubIDs, List<string> responseGroupIDs, List<string> responseGroupDescIDs, List<string> pubSubscriptionsExtMapperColumns, List<string> customColumnList, int brandId,int issueId, int downloadCount)
        {
            var sdoc = CreateFieldsXDoc(standardColumnsList, "StandardFields", "StandardField", true, true, true);
            var custodoc = CreateFieldsXDoc(customColumnList, "CustomFields", "CustomField", true, true);
            var doc = CreateFieldsXDoc(responseGroupIDs, "ResponseGroups", "ResponseGroup");
            var docDesc = CreateFieldsXDoc(responseGroupDescIDs, "ResponseGroups", "ResponseGroup", true);
            var pubSubExtMapperDoc = CreateFieldsXDoc(pubSubscriptionsExtMapperColumns, "PubSubscriptionsExtMapperValues", "PubSubscriptionsExtMapperValue", true);
            
            var conn = DataFunctions.GetClientSqlConnection(clientConnection);
            return ExecuteGetProductDimensionSubscriberData(
                conn,
                "sp_GetArchivedProductDimensionSubscriberData",
                query,
                sdoc.ToString(),
                pubIDs.First(),
                issueId,
                doc.ToString(),
                docDesc.ToString(),
                pubSubExtMapperDoc.ToString(),
                string.Join(",", custodoc.ToString()),
                brandId,
                downloadCount);
        }

        public static DataTable ExecuteGetProductDimensionSubscriberData(SqlConnection conn, string procedureName, string queries, string subscriptionFields, int pubId, int? issueId, string responseGroupId, string responseGroupIdDesc, string pubSubscriptionsExtMapperValues, string customColumns, int brandId, int? downloadCount = null, bool? uniqueDownload = null, bool? isFilterBased = null, string subscriberIds = null)
        {
            var cmd = new SqlCommand(procedureName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Queries", queries);
            cmd.Parameters.AddWithValue("@SubscriptionFields", subscriptionFields);
            cmd.Parameters.AddWithValue("@PubID", pubId);
            if (issueId != null)
            {
                cmd.Parameters.AddWithValue("@IssueID", issueId);
            }

            cmd.Parameters.AddWithValue("@ResponseGroupID", responseGroupId);
            cmd.Parameters.AddWithValue("@ResponseGroupID_Desc", responseGroupIdDesc);
            cmd.Parameters.AddWithValue("@PubSubscriptionsExtMapperValues", pubSubscriptionsExtMapperValues);
            cmd.Parameters.AddWithValue("@CustomColumns", customColumns);
            cmd.Parameters.AddWithValue("@BrandID", brandId);
            if (downloadCount.HasValue)
            {
                cmd.Parameters.AddWithValue("@DownloadCount", downloadCount.Value);
            }

            if (uniqueDownload.HasValue)
            {
                cmd.Parameters.AddWithValue("@uniquedownload", uniqueDownload.Value);
            }

            if (isFilterBased.HasValue)
            {
                cmd.Parameters.AddWithValue("@FilterBased", isFilterBased.Value);
            }

            if (subscriberIds != null)
            {
                cmd.Parameters.AddWithValue("@SubscriberIds", subscriberIds);
            }

            cmd.CommandTimeout = 0;

            return DataFunctions.GetDataTable(cmd, conn);
        }

        public static DataTable GetSubscriberData(KMPlatform.Object.ClientConnections clientconnection, StringBuilder queries, string standardColumns, List<FrameworkUAD.Entity.MasterGroup> masterGroupColumns, List<FrameworkUAD.Entity.MasterGroup> masterGroupDescColumns, List<FrameworkUAD.Entity.SubscriptionsExtensionMapper> subscriptionsExtMapperColumns, string customColumns, int brandId, List<int> pubIDs, bool isMostRecentData, int downloadCount, string subscriptionIDs = "")
        {
            XDocument doc = new XDocument(new XElement("MasterGroups"));

            foreach (FrameworkUAD.Entity.MasterGroup mg in masterGroupColumns)
            {
                doc.Root.Add(new XElement("MasterGroupColumnReference", mg.ColumnReference));
            }

            XDocument docDesc = new XDocument(new XElement("MasterGroups"));

            foreach (FrameworkUAD.Entity.MasterGroup mg in masterGroupDescColumns)
            {
                docDesc.Root.Add(new XElement("MasterGroupColumnReference", mg.ColumnReference));
            }

            XDocument subExtMapperDoc = new XDocument(new XElement("SubscriptionsExtMapperValues"));

            foreach (FrameworkUAD.Entity.SubscriptionsExtensionMapper sem in subscriptionsExtMapperColumns)
            {
                subExtMapperDoc.Root.Add(new XElement("SubscriptionsExtMapperValue", sem.CustomField));
            }

            var conn = DataFunctions.GetClientSqlConnection(clientconnection);
            return ExecuteGetSubscriberData(
                conn,
                isMostRecentData ? "sp_GetSubscriberData_RecentConsensus" : "sp_GetSubscriberData",
                queries.ToString(),
                standardColumns,
                doc.ToString(),
                docDesc.ToString(),
                subExtMapperDoc.ToString(),
                customColumns,
                brandId,
                pubIDs == null ? "" : string.Join(",", pubIDs),
                downloadCount,
                isMostRecentData,
                subscriptionIDs);
        }

        public static DataTable ExecuteGetSubscriberData(SqlConnection conn, string procedureName, string queries, string standardColumns, string doc, string docDesc, string subExtMapperDoc, string customColumns, int brandId, string pubIDs, int? downloadCount, bool isMostRecentData, string subscriptionIDs, bool? uniqueDownload = null)
        {
            var cmd = new SqlCommand(procedureName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Queries", queries);
            cmd.Parameters.AddWithValue("@StandardColumns", standardColumns);
            cmd.Parameters.AddWithValue("@MasterGroupValues", doc);
            cmd.Parameters.AddWithValue("@MasterGroupValues_Desc", docDesc);
            cmd.Parameters.AddWithValue("@SubscriptionsExtMapperValues", subExtMapperDoc);
            cmd.Parameters.AddWithValue("@CustomColumns", customColumns);
            cmd.Parameters.AddWithValue("@BrandID", brandId);
            cmd.Parameters.AddWithValue("@PubIDs", pubIDs);
            if (downloadCount.HasValue)
            {
                cmd.Parameters.AddWithValue("@DownloadCount", downloadCount.Value);
            }

            if (uniqueDownload.HasValue)
            {
                cmd.Parameters.AddWithValue("@uniquedownload", uniqueDownload);
            }

            if (!isMostRecentData && subscriptionIDs != null)
            {
                cmd.Parameters.AddWithValue("@SubscriptionID", subscriptionIDs);
            }

            cmd.CommandTimeout = 0;

            return DataFunctions.GetDataTable(cmd, conn);
        }

        public static DataTable GetProductDimensionSubscriberData(KMPlatform.Object.ClientConnections clientConnection, StringBuilder queries, string subscriptionFields, List<int> pubIDs, List<FrameworkUAD.Entity.ResponseGroup> responseGroupIDs, List<FrameworkUAD.Entity.ResponseGroup> responseGroupDescIDs, List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> pubSubscriptionsExtMapperColumns, string customColumns, int brandId, int downloadCount)
        {
            var responseGroupID = new StringBuilder();
            foreach (var responseGroup in responseGroupIDs)
            {
                responseGroupID.AppendFormat("<ResponseGroup ID=\"{0}\"/>{1}", responseGroup.ResponseGroupID, Environment.NewLine);
            }

            var responseGroupDescID = new StringBuilder();
            foreach (var responseGroupDesc in responseGroupDescIDs)
            {
                responseGroupDescID.AppendFormat("<ResponseGroup ID=\"{0}\"/>{1}", responseGroupDesc.ResponseGroupID, Environment.NewLine);
            }

            var pubSubExtMapperDoc = new XDocument(new XElement("PubSubscriptionsExtMapperValues"));

            if (pubSubscriptionsExtMapperColumns != null)
            {
                foreach (var sem in pubSubscriptionsExtMapperColumns)
                {
                    pubSubExtMapperDoc.Root.Add(new XElement("PubSubscriptionsExtMapperValue", sem.CustomField));
                }
            }

            var conn = DataFunctions.GetClientSqlConnection(clientConnection);
            return ExecuteGetProductDimensionSubscriberData(
                conn,
                "sp_GetProductDimensionSubscriberData",
                queries.ToString(),
                subscriptionFields,
                pubIDs.First(),
                null,
                responseGroupID.ToString(),
                responseGroupDescID.ToString(),
                pubSubExtMapperDoc.ToString(),
                customColumns,
                brandId,
                downloadCount);
        }

        public static DataTable GetSubscriberData(KMPlatform.Object.ClientConnections clientconnection, StringBuilder queries, string fields)
        {
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("spDownloaddetails", conn);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Queries", SqlDbType.Text)).Value = queries.ToString();
            cmd.Parameters.Add(new SqlParameter("@Fields", SqlDbType.VarChar)).Value = fields;
            cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar)).Value = string.Empty;
            return DataFunctions.GetDataTable(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }

        public static DataTable GetSubscriberData_CLV(KMPlatform.Object.ClientConnections clientConnection, StringBuilder queries, string standardColumns, List<Entity.MasterGroup> masterGroupColumns, List<Entity.MasterGroup> masterGroupDescColumns, List<Entity.SubscriptionsExtensionMapper> subscriptionsExtMapperColumns, string customColumns, int brandId, List<int> pubIDs, bool uniqueDownload, bool isMostRecentData)
        {
            var doc = new XDocument(new XElement("MasterGroups"));
            foreach (var column in masterGroupColumns)
            {
                doc.Root.Add(new XElement("MasterGroupColumnReference", column.ColumnReference));
            }

            var docDesc = new XDocument(new XElement("MasterGroups"));
            foreach (var column in masterGroupDescColumns)
            {
                docDesc.Root.Add(new XElement("MasterGroupColumnReference", column.ColumnReference));
            }

            var subExtMapperDoc = new XDocument(new XElement("SubscriptionsExtMapperValues"));
            foreach (var sem in subscriptionsExtMapperColumns)
            {
                subExtMapperDoc.Root.Add(new XElement("SubscriptionsExtMapperValue", sem.CustomField));
            }

            var conn = DataFunctions.GetClientSqlConnection(clientConnection);
            return ExecuteGetSubscriberData(
                conn,
                isMostRecentData ? "sp_GetSubscriberData_RecentConsensus_CLV" : "sp_GetSubscriberData_CLV",
                queries.ToString(),
                standardColumns,
                doc.ToString(),
                docDesc.ToString(),
                subExtMapperDoc.ToString(),
                customColumns,
                brandId,
                pubIDs == null ? "" : string.Join(",", pubIDs),
                null,
                isMostRecentData,
                null,
                uniqueDownload);
        }

        public static DataTable GetSubscriberData_EV(KMPlatform.Object.ClientConnections clientConnection, StringBuilder queries, string standardColumns, List<Entity.MasterGroup> masterGroupColumns, List<Entity.MasterGroup> masterGroupDescColumns, List<Entity.SubscriptionsExtensionMapper> subscriptionsExtMapperColumns, string customColumns, int brandId, List<int> pubIDs, bool uniqueDownload, bool isMostRecentData)
        {
            var doc = new XDocument(new XElement("MasterGroups"));
            foreach (var column in masterGroupColumns)
            {
                doc.Root.Add(new XElement("MasterGroupColumnReference", column.ColumnReference));
            }

            var docDesc = new XDocument(new XElement("MasterGroups"));
            foreach (var column in masterGroupDescColumns)
            {
                docDesc.Root.Add(new XElement("MasterGroupColumnReference", column.ColumnReference));
            }

            var subExtMapperDoc = new XDocument(new XElement("SubscriptionsExtMapperValues"));
            foreach (var sem in subscriptionsExtMapperColumns)
            {
                subExtMapperDoc.Root.Add(new XElement("SubscriptionsExtMapperValue", sem.CustomField));
            }

            var conn = DataFunctions.GetClientSqlConnection(clientConnection);
            return ExecuteGetSubscriberData(
                conn,
                isMostRecentData ? "sp_GetSubscriberData_RecentConsensus_EV" : "sp_GetSubscriberData_EV ",
                queries.ToString(),
                standardColumns,
                doc.ToString(),
                docDesc.ToString(),
                subExtMapperDoc.ToString(),
                customColumns,
                brandId,
                pubIDs == null ? "" : string.Join(",", pubIDs),
                null,
                isMostRecentData,
                null,
                uniqueDownload);
        }

        public static DataTable GetProductDimensionSubscriberData_CLV(KMPlatform.Object.ClientConnections clientConnection, StringBuilder queries, string subscriptionFields, List<int> pubIDs, List<Entity.ResponseGroup> responseGroupIDs, List<Entity.ResponseGroup> responseGroupDescIDs, List<Entity.ProductSubscriptionsExtensionMapper> pubSubscriptionsExtMapperColumns, String customColumns, int brandId, bool uniqueDownload)
        {
            var responseGroupID = new StringBuilder();
            foreach (var responseGroup in responseGroupIDs)
            {
                responseGroupID.AppendFormat("<ResponseGroup ID=\"{0}\"/>{1}", responseGroup.ResponseGroupID, Environment.NewLine);
            }

            var responseGroupDescID = new StringBuilder();
            foreach (var responseGroupDesc in responseGroupDescIDs)
            {
                responseGroupDescID.AppendFormat("<ResponseGroup ID=\"{0}\"/>{1}", responseGroupDesc.ResponseGroupID, Environment.NewLine);
            }

            var pubSubExtMapperDoc = new XDocument(new XElement("PubSubscriptionsExtMapperValues"));
            foreach (var sem in pubSubscriptionsExtMapperColumns)
            {
                pubSubExtMapperDoc.Root.Add(new XElement("PubSubscriptionsExtMapperValue", sem.CustomField));
            }

            var conn = DataFunctions.GetClientSqlConnection(clientConnection);
            return ExecuteGetProductDimensionSubscriberData(
                conn,
                "sp_GetProductDimensionSubscriberData_CLV",
                queries.ToString(),
                subscriptionFields,
                pubIDs.First(),
                null,
                responseGroupID.ToString(),
                responseGroupDescID.ToString(),
                pubSubExtMapperDoc.ToString(),
                customColumns,
                brandId,
                null,
                uniqueDownload);
        }

        public static DataTable GetProductDimensionSubscriberData_EV(KMPlatform.Object.ClientConnections clientConnection, StringBuilder queries, string subscriptionFields, List<int> pubIDs, List<Entity.ResponseGroup> responseGroupIDs, List<Entity.ResponseGroup> responseGroupDescIDs, List<Entity.ProductSubscriptionsExtensionMapper> pubSubscriptionsExtMapperColumns, string customColumns, int brandId, bool uniqueDownload)
        {
            var responseGroupID = new StringBuilder();
            foreach (var responseGroup in responseGroupIDs)
            {
                responseGroupID.AppendFormat("<ResponseGroup ID=\"{0}\"/>{1}", responseGroup.ResponseGroupID, Environment.NewLine);
            }

            var responseGroupDescID = new StringBuilder();
            foreach (var responseGroupDesc in responseGroupDescIDs)
            {
                responseGroupDescID.AppendFormat("<ResponseGroup ID=\"{0}\"/>{1}", responseGroupDesc.ResponseGroupID, Environment.NewLine);
            }

            var pubSubExtMapperDoc = new XDocument(new XElement("PubSubscriptionsExtMapperValues"));
            foreach (var sem in pubSubscriptionsExtMapperColumns)
            {
                pubSubExtMapperDoc.Root.Add(new XElement("PubSubscriptionsExtMapperValue", sem.CustomField));
            }

            var conn = DataFunctions.GetClientSqlConnection(clientConnection);
            return ExecuteGetProductDimensionSubscriberData(
                conn,
                "sp_GetProductDimensionSubscriberData_EV",
                queries.ToString(),
                subscriptionFields,
                pubIDs.First(),
                null,
                responseGroupID.ToString(),
                responseGroupDescID.ToString(),
                pubSubExtMapperDoc.ToString(),
                customColumns,
                brandId,
                null,
                uniqueDownload);
        }

        public static int GetUniqueLocationsCount(KMPlatform.Object.ClientConnections clientconnection, StringBuilder Queries)
        {
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_GetSubscriberUniqueLocationCount");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Queries", Queries.ToString()));
            cmd.CommandTimeout = 0;
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, conn.ConnectionString));
        }

        public static int GetUniqueEmailsCount(KMPlatform.Object.ClientConnections clientconnection, StringBuilder Queries)
        {
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_GetSubscriberUniqueEmailsCount");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Queries", Queries.ToString()));
            cmd.CommandTimeout = 0;
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, conn.ConnectionString));
        }

        public static DataTable GetSubscriptionExportColumns(KMPlatform.Object.ClientConnections clientconnection)
        {
            return DataFunctions.GetDataTable("select sc.name from sysobjects so join syscolumns sc on so.id = sc.id where so.name = 'Subscriptions' and sc.name not in ('EMAIL','FNAME','LNAME','COMPANY','TITLE','ADDRESS','MAILSTOP','CITY','STATE','ZIP','PLUS4','COUNTRY','FORZIP','PHONE','MOBILE','FAX','CategoryID','TransactionID','pubids', 'MailPermission', 'FaxPermission', 'PhonePermission', 'OtherProductsPermission', 'ThirdPartyPermission', 'EmailRenewPermission', 'TextPermission','SubscriptionID', 'SEQUENCE', 'CountryID','IsExcluded','PubID','STATLIST','IGRP_CNT','CGRP_NO','CGRP_CNT', 'IGRP_Rank', 'CGRP_Rank', 'EmailExists','FaxExists','PhoneExists','PRIORITY','Origssrc','SALES','Source','subsrc','Latitude','Longitude','IsLatLonValid','LatLonMsg','verified','Employ', 'Sales', 'SIC', 'SICCODE', 'StatusUpdatedDate', 'StatusUpdatedReason', 'EmailStatusID', 'Score', 'QDate', 'Notes', 'Demo7', 'AddressupdatedSourceTypeCodeID', 'AddressTypeCodeId', 'AddressLastUpdatedDate', 'CreatedByuserID', 'UpdatedByUserID', 'QSourceID', 'Par3C') order by sc.name", DataFunctions.GetClientSqlConnection(clientconnection));
        }

        public static DataTable GetSubScheduledReportExportColumns(KMPlatform.Object.ClientConnections clientconnection)
        {
            return DataFunctions.GetDataTable("select sc.name from sysobjects so join syscolumns sc on so.id = sc.id where so.name = 'Subscriptions' and sc.name not in ('SubscriptionID', 'SEQUENCE', 'CountryID','IsExcluded','PubID','STATLIST','IGRP_CNT','CGRP_NO','CGRP_CNT', 'EmailExists','FaxExists','PhoneExists','PRIORITY','Origssrc','SALES','Source','subsrc','Latitude','Longitude','IsLatLonValid','LatLonMsg','verified','Employ', 'Sales', 'SIC', 'SICCODE', 'StatusUpdatedDate', 'StatusUpdatedReason', 'EmailStatusID', 'Notes', 'Demo7', 'AddressupdatedSourceTypeCodeID', 'AddressTypeCodeId', 'AddressLastUpdatedDate', 'CreatedByuserID', 'UpdatedByUserID', 'TransactionID', 'QSourceID') order by sc.name", DataFunctions.GetClientSqlConnection(clientconnection));
        }

        public static DataTable GetSubProductScheduledReportExportColumns(KMPlatform.Object.ClientConnections clientconnection)
        {
            return DataFunctions.GetDataTable("select sc.name from sysobjects so join syscolumns sc on so.id = sc.id where so.name = 'Subscriptions' and sc.name not in ('SubscriptionID', 'SEQUENCE', 'CountryID','IsExcluded','PubID','STATLIST','IGRP_CNT','CGRP_NO','CGRP_CNT', 'EmailExists','FaxExists','PhoneExists','PRIORITY','Origssrc','SALES','Source','subsrc','Latitude','Longitude','IsLatLonValid','LatLonMsg','verified','Employ', 'Sales', 'SIC', 'SICCODE', 'StatusUpdatedDate', 'StatusUpdatedReason', 'EmailStatusID', 'Notes', 'AddressupdatedSourceTypeCodeID', 'AddressTypeCodeId', 'AddressLastUpdatedDate', 'CreatedByuserID', 'UpdatedByUserID', 'TransactionID', 'QSourceID') order by sc.name", DataFunctions.GetClientSqlConnection(clientconnection));
        }

        public static Object.Subscriber GetByIGrp_No(KMPlatform.Object.ClientConnections clientconnection, Guid IGrp_No)
        {
            Object.Subscriber retItem = new Object.Subscriber();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_Subscription_Select_IGrp_No", conn);
            cmd.Parameters.Add(new SqlParameter("@IGrp_No", IGrp_No));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Object.Subscriber> builder = DynamicBuilder<Object.Subscriber>.CreateBuilder(rdr);

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
    }
}
