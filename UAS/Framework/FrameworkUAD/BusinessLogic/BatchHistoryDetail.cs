using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class BatchHistoryDetail
    {
        public List<Object.BatchHistoryDetail> Select(int userID, bool isActive, KMPlatform.Object.ClientConnections client, string clientName)
        {
            List<Object.BatchHistoryDetail> retList = null;
            retList = DataAccess.BatchHistoryDetail.Select(userID, isActive, client, clientName);
            return retList;
        }
        public List<Object.BatchHistoryDetail> SelectBatchID(int userID, bool isActive, KMPlatform.Object.ClientConnections client, string clientName, int batchID)
        {
            List<Object.BatchHistoryDetail> retList = null;
            retList = DataAccess.BatchHistoryDetail.SelectBatchID(userID, isActive, client, clientName, batchID);
            return retList;
        }
        public List<Object.BatchHistoryDetail> SelectBatchID(bool isActive, KMPlatform.Object.ClientConnections client, string clientName, int batchID)
        {
            List<Object.BatchHistoryDetail> retList = null;
            retList = DataAccess.BatchHistoryDetail.SelectBatchID(isActive, client, clientName, batchID);
            return retList;
        }
        public List<Object.BatchHistoryDetail> Select(KMPlatform.Object.ClientConnections client, string clientName)
        {
            List<Object.BatchHistoryDetail> retList = null;
            retList = DataAccess.BatchHistoryDetail.Select(client, clientName);
            return retList;
        }
        public List<Object.BatchHistoryDetail> Select(int SubscriptionID, KMPlatform.Object.ClientConnections client, string clientName)
        {
            List<Object.BatchHistoryDetail> retList = null;
            retList = DataAccess.BatchHistoryDetail.Select(SubscriptionID, client, clientName);
            return retList;
        }

        public List<Object.BatchHistoryDetail> SelectSubscriber(int subscriberID, KMPlatform.Object.ClientConnections client, string clientName)
        {
            List<Object.BatchHistoryDetail> retList = null;
            retList = DataAccess.BatchHistoryDetail.SelectSubscriber(subscriberID, client, clientName);
            return retList;
        }
        public List<Object.BatchHistoryDetail> SelectBatch(int BatchID, string Name, int SequenceID, KMPlatform.Object.ClientConnections client, string clientName)
        {
            List<Object.BatchHistoryDetail> retList = null;
            retList = DataAccess.BatchHistoryDetail.SelectBatch(BatchID, Name, SequenceID, client, clientName);
            return retList;
        }
        public List<Object.BatchHistoryDetail> SelectBatch(int BatchID, string Name, int SequenceID, DateTime From, DateTime To, KMPlatform.Object.ClientConnections client, string clientName)
        {
            List<Object.BatchHistoryDetail> retList = null;
            retList = DataAccess.BatchHistoryDetail.SelectBatch(BatchID, Name, SequenceID, From, To, client, clientName);
            return retList;
        }
        
    }
}
