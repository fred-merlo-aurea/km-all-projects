using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class History
    {
        public List<Entity.History> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.History> retList = null;
            retList = DataAccess.History.Select(client);
            foreach (Entity.History h in retList)
            {
                h.UserLogList = UserLogList(h.HistoryID,client).ToList();
                h.HistoryResponseList = HistoryResponseList(h.HistoryID,client).ToList();
                h.HistoryMarketingMapList = HistoryMarketingMapList(h.HistoryID,client).ToList();
            }

            return retList;
        }
        public List<Entity.History> Select(int batchID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.History> retList = null;
            retList = DataAccess.History.Select(batchID, client);
            foreach (Entity.History h in retList)
                h.UserLogList = UserLogList(h.HistoryID, client).ToList();

            return retList;
        }
        public List<Entity.History> Select(DateTime startDate, DateTime endDate, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.History> retList = null;
            retList = DataAccess.History.Select(startDate, endDate, client);
            foreach (Entity.History h in retList)
                h.UserLogList = UserLogList(h.HistoryID, client).ToList();

            return retList;
        }
        public List<Entity.History> SelectBatch(int UserID, int PublicationID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.History> retList = null;
            retList = DataAccess.History.SelectBatch(UserID, PublicationID, client);
            return retList;
        }
        public List<int> UserLogList(int historyID, KMPlatform.Object.ClientConnections client)
        {
            List<int> retList = null;
            retList = DataAccess.History.UserLogList(historyID, client);
            return retList;
        }
        public List<int> HistoryResponseList(int historyID, KMPlatform.Object.ClientConnections client)
        {
            List<int> retList = null;
            retList = DataAccess.History.HistoryResponseList(historyID, client);
            return retList;
        }
        public List<int> HistoryMarketingMapList(int historyID, KMPlatform.Object.ClientConnections client)
        {
            List<int> retList = null;
            retList = DataAccess.History.HistoryMarketingMapList(historyID, client);
            return retList;
        }

        public Entity.History AddHistoryEntry(KMPlatform.Object.ClientConnections client, int batchID, int batchCountItem, int publicationID, int subscriberID, int subscriptionID, int historySubscriptionID, int historyPaidID, int userID, int historyPaidBillToID = -1, List<int> userLogIDs = null, List<int> historyResponseIDs = null, List<int> historyMarketingMapIDs = null)
        {
            Entity.History h = new Entity.History(batchID, batchCountItem,  publicationID, subscriberID, subscriptionID, historySubscriptionID, historyPaidID, userID, historyPaidBillToID);
            using (TransactionScope scope = new TransactionScope())
            {
                h.HistoryID = DataAccess.History.Save(h, client);
                scope.Complete();
            }

            if (userLogIDs != null)
            {
                foreach (int i in userLogIDs)
                    Insert_History_To_UserLog(h.HistoryID, i, client);
            }

            //if (historyResponseIDs != null)
            //{
            //    foreach (int i in historyResponseIDs)
            //        Insert_History_To_HistoryResponse(h.HistoryID, i, client);
            //}

            if (historyMarketingMapIDs != null)
            {
                foreach (int i in historyMarketingMapIDs)
                    Insert_History_To_HistoryMarketingMap(h.HistoryID, i, client);
            }
            return h;
        }
        public bool Insert_History_To_UserLog(int historyID, int userLogID, KMPlatform.Object.ClientConnections client)
        {
            bool saveDone = false;
            using (TransactionScope scope = new TransactionScope())
            {
                saveDone = DataAccess.History.Insert_History_To_UserLog(historyID, userLogID, client);
                scope.Complete();
            }

            return saveDone;
        }
        //public bool Insert_History_To_HistoryResponse(int historyID, int historyResponseID, KMPlatform.Object.ClientConnections client)
        //{
        //    bool saveDone = false;
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        saveDone = DataAccess.History.Insert_History_To_HistoryResponse(historyID, historyResponseID, client);
        //        scope.Complete();
        //    }

        //    return saveDone;
        //}
        public bool Insert_History_To_HistoryMarketingMap(int historyID, int historyMarketingMapID, KMPlatform.Object.ClientConnections client)
        {
            bool saveDone = false;
            using (TransactionScope scope = new TransactionScope())
            {
                saveDone = DataAccess.History.Insert_History_To_HistoryMarketingMap(historyID, historyMarketingMapID, client);
                scope.Complete();
            }

            return saveDone;
        }
        public int Save(Entity.History x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.HistoryID = DataAccess.History.Save(x, client);
                scope.Complete();
            }

            return x.HistoryID;
        }

        //public bool Insert_History_To_HistoryResponse_List(int historyID, List<Entity.HistoryResponseMap> list, KMPlatform.Object.ClientConnections client)
        //{
        //    bool done = false;

        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        done = DataAccess.History.Insert_History_To_HistoryResponse_List(historyID, list, client);
        //        scope.Complete();
        //    }

        //    return done;
        //}

        public bool Insert_History_To_HistoryMarketingMap_List(int historyID, List<Entity.HistoryMarketingMap> list, KMPlatform.Object.ClientConnections client)
        {
            bool done = false;

            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.History.Insert_History_To_HistoryMarketingMap_List(historyID, list, client);
                scope.Complete();
            }

            return done;
        }
    }
}
