using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class FinalizeBatch
    {
        public List<Object.FinalizeBatch> SelectUser(int userId, KMPlatform.Object.ClientConnections client, bool isOpen)
        {
            List<Object.FinalizeBatch> retList = null;
            retList = DataAccess.FinalizeBatch.SelectUser(userId, isOpen, client);
            return retList;
        }
        public List<Object.FinalizeBatch> SelectAllUser(int userID, KMPlatform.Object.ClientConnections client)
        {
            List<Object.FinalizeBatch> retList = null;
            retList = DataAccess.FinalizeBatch.SelectAllUser(userID, client);
            return retList;
        }
        public List<Object.FinalizeBatch> SelectDateRange(DateTime startDate, DateTime endDate, KMPlatform.Object.ClientConnections client)
        {
            List<Object.FinalizeBatch> retList = null;
            retList = DataAccess.FinalizeBatch.SelectDateRange(startDate, endDate, client);
            return retList;
        }
        public Object.FinalizeBatch SelectBatchId(int batchId, KMPlatform.Object.ClientConnections client)
        {
            Object.FinalizeBatch retList = null;
            retList = DataAccess.FinalizeBatch.SelectBatchId(batchId, client);
            return retList;
        }
        public Object.FinalizeBatch SelectBatchNumber(int batchNumber, KMPlatform.Object.ClientConnections client)
        {
            Object.FinalizeBatch retList = null;
            retList = DataAccess.FinalizeBatch.SelectBatchNumber(batchNumber, client);
            return retList;
        }

        //public List<Object.FinalizeBatch> Select(int userID, KMPlatform.Object.ClientConnections client, int clientId, string clientName)
        //{
        //    List<Object.FinalizeBatch> retList = null;
        //    retList = DataAccess.FinalizeBatch.Select(userID,client, clientId, clientName);
        //    return retList;
        //}
        //public List<Object.FinalizeBatch> SelectBatch(int userID, KMPlatform.Object.ClientConnections client, int clientId, string clientName)
        //{
        //    List<Object.FinalizeBatch> retList = null;
        //    retList = DataAccess.FinalizeBatch.SelectBatch(userID, client, clientId, clientName);
        //    return retList;
        //}
        //public Object.FinalizeBatch SelectBatchID(KMPlatform.Object.ClientConnections client, int clientId, string clientName, int batchID)
        //{
        //    Object.FinalizeBatch retList = null;
        //    retList = DataAccess.FinalizeBatch.SelectBatchID(client, clientId, clientName, batchID);
        //    return retList;
        //}
        //public Object.FinalizeBatch SelectBatchID(int userID, KMPlatform.Object.ClientConnections client, int clientId, string clientName, int batchID)
        //{
        //    Object.FinalizeBatch retList = null;
        //    retList = DataAccess.FinalizeBatch.SelectBatchID(userID, client, clientId, clientName, batchID);
        //    return retList;
        //}
        //public List<Object.FinalizeBatch> SelectBatchUserName(int userID, KMPlatform.Object.ClientConnections client, int clientId, string clientName)
        //{
        //    List<Object.FinalizeBatch> retList = null;
        //    retList = DataAccess.FinalizeBatch.SelectBatchUserName(userID, client, clientId, clientName);
        //    return retList;
        //}
        ////SelectBatchUserName(userID, client.ClientConnections, client.ClientID, client.DisplayName);//pass IsDateFinalized, StartDate, EndDate
        //public List<Object.FinalizeBatch> SelectBatchUserName(int userID, KMPlatform.Object.ClientConnections client, int clientId, string clientName, bool isDateFinalized, DateTime? startDate = null, DateTime? endDate = null, string batchNumber = "", string userName = "", string productCode = "",)
        //{
        //    List<Object.FinalizeBatch> retList = null;
        //    if(startDate == null && endDate == null)
        //        retList = DataAccess.FinalizeBatch.SelectBatchUserName(userID, client, clientId, clientName, isDateFinalized);
        //    else if (startDate != null && endDate == null)
        //        retList = DataAccess.FinalizeBatch.SelectBatchUserName(userID, client, clientId, clientName, isDateFinalized, startDate);
        //    else if (startDate == null && endDate != null)
        //        retList = DataAccess.FinalizeBatch.SelectBatchUserName(userID, client, clientId, clientName, isDateFinalized, endDate);
        //    else if (startDate != null && endDate != null)
        //        retList = DataAccess.FinalizeBatch.SelectBatchUserName(userID, client, clientId, clientName, isDateFinalized, startDate, endDate);

        //    return retList;
        //}
    }
}
