using System;
using System.Collections.Generic;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class DataCompareView
    {
        public List<Entity.DataCompareView> SelectForSourceFile(int sourceFileId)
        {
            List<Entity.DataCompareView> x = null;
            x = DataAccess.DataCompareView.SelectForSourceFile(sourceFileId);
            foreach (Entity.DataCompareView v in x)
                SetCustomProperties(v);
            return x;
        }
        public List<Entity.DataCompareView> SelectForClient(int clientId)
        {
            List<Entity.DataCompareView> x = null;
            x = DataAccess.DataCompareView.SelectForClient(clientId);
            foreach (Entity.DataCompareView v in x)
                SetCustomProperties(v);
            return x;
        }
        public List<Entity.DataCompareView> SelectForUser(int userId)
        {
            List<Entity.DataCompareView> x = null;
            x = DataAccess.DataCompareView.SelectForUser(userId);
            foreach (Entity.DataCompareView v in x)
                SetCustomProperties(v);
            return x;
        }
        public List<Entity.DataCompareView> SelectForRun(int dcRunId)
        {
            List<Entity.DataCompareView> x = null;
            x = DataAccess.DataCompareView.SelectForRun(dcRunId);
            foreach (Entity.DataCompareView v in x)
                SetCustomProperties(v);
            return x;
        }
        private void SetCustomProperties(Entity.DataCompareView dcr)
        {
            DataCompareDownload cWorker = new DataCompareDownload();
            dcr.Downloads = cWorker.SelectForView(dcr.DcViewId);
        }
        public int Save(Entity.DataCompareView x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.DcRunId = DataAccess.DataCompareView.Save(x);
                scope.Complete();
            }

            return x.DcRunId;
        }
        public Object.DataCompareViewCost GetDataCompareViewCost(int matchCount, int likeCount, int clientId, int userId, int dcRunId = 0)
        {
            Object.DataCompareViewCost x = null;
            x = DataAccess.DataCompareView.GetDataCompareViewCost(matchCount, likeCount, clientId, userId);
            x.DcRunId = dcRunId;
            x.MatchRecordCount = matchCount;
            x.LikeRecordCount = likeCount;
            return x;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">int userId</param>
        /// <param name="clientId">int clientId</param>
        /// <param name="count">int total count - be sure to correctly include file records plus target count</param>
        /// <param name="Match_or_Like"></param>
        /// <param name="MergePurge_or_Download"></param>
        /// <returns></returns>
        public decimal GetDataCompareCost(int userId, int clientId, int count, FrameworkUAD_Lookup.Enums.DataCompareType dcType, FrameworkUAD_Lookup.Enums.DataCompareCost MergePurge_or_Download)
        {
            return DataAccess.DataCompareView.GetDataCompareCost(userId, clientId, count, dcType, MergePurge_or_Download);
        }
        public bool Delete(int DcViewID)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.DataCompareView.Delete(DcViewID);
                scope.Complete();
                complete = true;
            }

            return complete;
        }
    }
}
