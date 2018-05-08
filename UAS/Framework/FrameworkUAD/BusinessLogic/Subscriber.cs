using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.BusinessLogic
{
    public class Subscriber
    {
        #region Data

        public static bool ExistsStandardFieldName(KMPlatform.Object.ClientConnections clientconnection, string Name)
        {
            var x = FrameworkUAD.DataAccess.Subscriber.ExistsStandardFieldName(clientconnection, Name);
            return x;
        }

        public static List<Object.Subscriber> Get(KMPlatform.Object.ClientConnections clientconnection, int SubscriptionID, int BrandID)
        {
            var x = FrameworkUAD.DataAccess.Subscriber.Get(clientconnection, SubscriptionID, BrandID);
            return x;
        }
        public DataTable GetSubscriberData(KMPlatform.Object.ClientConnections clientconnection, StringBuilder Queries, List<string> StandardColumnsList, List<string> masterGroupColumns, List<string> masterGroupDescColumns, List<string> SubscriptionsExtMapperColumns, List<string> customColumnList, int BrandID, List<int> PubIDs, bool IsMostRecentData, int DownloadCount, string SubscriptionIDs = "")
        {
            var dt = DataAccess.Subscriber.GetSubscriberData(clientconnection, Queries, StandardColumnsList, masterGroupColumns, masterGroupDescColumns, SubscriptionsExtMapperColumns, customColumnList, BrandID, PubIDs, IsMostRecentData, DownloadCount, SubscriptionIDs);
            return dt;
        }
        public DataTable GetProductDimensionSubscriberData(KMPlatform.Object.ClientConnections clientconnection, StringBuilder Queries, List<string> StandardColumnsList, List<int> PubIDs, List<string> ResponseGroupIDs, List<string> ResponseGroupDescIDs, List<string> PubSubscriptionsExtMapperColumns, List<string> customColumnList, int BrandID, int DownloadCount, bool isFilterBased = true, List<int> subscriberIds = null)
        {
            var dt = DataAccess.Subscriber.GetProductDimensionSubscriberData(clientconnection, Queries, StandardColumnsList, PubIDs, ResponseGroupIDs, ResponseGroupDescIDs, PubSubscriptionsExtMapperColumns, customColumnList, BrandID, DownloadCount, isFilterBased, subscriberIds);
            return dt;
        }
        public DataTable GetArchivedProductDimensionSubscriberData(KMPlatform.Object.ClientConnections clientconnection, string query, List<string> StandardColumnsList, List<int> PubIDs, List<string> ResponseGroupIDs, List<string> ResponseGroupDescIDs, List<string> PubSubscriptionsExtMapperColumns, List<string> customColumnList, int BrandID,int IssueId, int DownloadCount)
        {
            var dt = DataAccess.Subscriber.GetArchivedProductDimensionSubscriberData(clientconnection,  query, StandardColumnsList, PubIDs, ResponseGroupIDs, ResponseGroupDescIDs, PubSubscriptionsExtMapperColumns, customColumnList, BrandID, IssueId, DownloadCount);
            return dt;
        }
        public static DataTable GetSubscriberData(KMPlatform.Object.ClientConnections clientconnection, StringBuilder Queries, string StandardColumns, List<FrameworkUAD.Entity.MasterGroup> masterGroupColumns, List<FrameworkUAD.Entity.MasterGroup> masterGroupDescColumns, List<FrameworkUAD.Entity.SubscriptionsExtensionMapper> SubscriptionsExtMapperColumns, string CustomColumns, int BrandID, List<int> PubIDs, bool IsMostRecentData, int DownloadCount, string SubscriptionIDs = "")
        {
            var x = FrameworkUAD.DataAccess.Subscriber.GetSubscriberData(clientconnection, Queries,StandardColumns, masterGroupColumns,masterGroupDescColumns,SubscriptionsExtMapperColumns,CustomColumns, BrandID,PubIDs, IsMostRecentData, DownloadCount, SubscriptionIDs = "");
            return x;
        }

        public static DataTable GetProductDimensionSubscriberData(KMPlatform.Object.ClientConnections clientconnection, StringBuilder Queries, string SubscriptionFields, List<int> PubIDs, List<FrameworkUAD.Entity.ResponseGroup> ResponseGroupIDs, List<FrameworkUAD.Entity.ResponseGroup> ResponseGroupDescIDs, List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> PubSubscriptionsExtMapperColumns, string CustomColumns, int BrandID, int DownloadCount)
        {
            var x = FrameworkUAD.DataAccess.Subscriber.GetProductDimensionSubscriberData(clientconnection,Queries, SubscriptionFields, PubIDs,ResponseGroupIDs,ResponseGroupDescIDs, PubSubscriptionsExtMapperColumns,  CustomColumns,  BrandID,  DownloadCount);
            return x;
        }

        public static DataTable GetSubscriberData(KMPlatform.Object.ClientConnections clientconnection, StringBuilder queries, string fields)
        {
            var x = FrameworkUAD.DataAccess.Subscriber.GetSubscriberData(clientconnection, queries, fields);
            return x;
        }

      
       
      

        public static int GetUniqueLocationsCount(KMPlatform.Object.ClientConnections clientconnection, StringBuilder Queries)
        {
            var x = FrameworkUAD.DataAccess.Subscriber.GetUniqueLocationsCount(clientconnection,  Queries);
            return x;
        }

        public static int GetUniqueEmailsCount(KMPlatform.Object.ClientConnections clientconnection, StringBuilder Queries)
        {
            var x = FrameworkUAD.DataAccess.Subscriber.GetUniqueEmailsCount( clientconnection,  Queries);
            return x;
        }

        public static DataTable GetSubscriptionExportColumns(KMPlatform.Object.ClientConnections clientconnection)
        {
            var x = FrameworkUAD.DataAccess.Subscriber.GetSubscriptionExportColumns(clientconnection);
            return x;
        }

        public static DataTable GetSubScheduledReportExportColumns(KMPlatform.Object.ClientConnections clientconnection)
        {
            var x = FrameworkUAD.DataAccess.Subscriber.GetSubScheduledReportExportColumns(clientconnection);
            return x;
        }

        public static DataTable GetSubProductScheduledReportExportColumns(KMPlatform.Object.ClientConnections clientconnection)
        {
            var x = FrameworkUAD.DataAccess.Subscriber.GetSubProductScheduledReportExportColumns(clientconnection);
            return x;
        }

        public static Object.Subscriber GetByIGrp_No(KMPlatform.Object.ClientConnections clientconnection, Guid IGrp_No)
        {
            var x = FrameworkUAD.DataAccess.Subscriber.GetByIGrp_No(clientconnection, IGrp_No);
            return x;
        }
        #endregion
    }
}
