using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Data;

namespace FrameworkUAD.BusinessLogic
{
    public class DataCompareProfile
    {
        public void InsertFromSubscriberFinal(KMPlatform.Object.ClientConnections client, string processCode)
        {
            DataAccess.DataCompareProfile.InsertFromSubscriberFinal(client, processCode);
        }

        public List<Entity.DataCompareProfile> Select(KMPlatform.Object.ClientConnections client, string processCode)
        {
            List<Entity.DataCompareProfile> x = null;
            x = DataAccess.DataCompareProfile.Select(client, processCode).ToList();
            return x;
        }

        public int GetDataCompareCount(KMPlatform.Object.ClientConnections client,string processCode, int dcTargetCodeId, int id = 0)
        {
            return DataAccess.DataCompareProfile.GetDataCompareCount(client, processCode, dcTargetCodeId, id); ;//call a sproc to get the count
        }

        public DataTable GetDataCompareData(KMPlatform.Object.ClientConnections client, string processCode, int dcTargetCodeId, string matchType, int id = 0)
        {
            return DataAccess.DataCompareProfile.GetDataCompareData(client, processCode, dcTargetCodeId, matchType, id); ;//call a sproc to get the count
        }

        public DataTable GetDataCompareSummary(KMPlatform.Object.ClientConnections client, string processCode, int dcTargetCodeId, string matchType, int id = 0)
        {
            return DataAccess.DataCompareProfile.GetDataCompareSummary(client, processCode, dcTargetCodeId, matchType, id); ;//call a sproc to get the count
        }

        /// <summary>
        /// if Target is Consensus then do not need id
        /// if Product then id is ProductId
        /// if Brand then id is BrandId
        /// </summary>
        /// <param name="client"></param>
        /// <param name="processCode"></param>
        /// <param name="dcTargetCodeId"></param>
        /// <param name="id">ProductId or BrandId - 0 if Consensus</param>
        /// <returns></returns>
        public void CreateCostDetail(int dcViewId, int dcTypeCodeID, string profileCount, string profileColumns, string demoColumns, int userId)
        {
            //int profileCount = GetDataCompareCount(client, processCode, dcTargetCodeId, id); ;//call a sproc to get the count
            FrameworkUAS.BusinessLogic.DataCompareDownloadCostDetail wrk = new FrameworkUAS.BusinessLogic.DataCompareDownloadCostDetail();
            wrk.CreateCostDetail(dcViewId, dcTypeCodeID, profileCount, profileColumns, demoColumns, userId);
        }
    }
}
