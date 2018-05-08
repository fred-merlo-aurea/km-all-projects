using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class DataCompareDownloadFilterGroup
    {
        public List<Entity.DataCompareDownloadFilterGroup> SelectForDownload(int dcDownloadId)
        {
            List<Entity.DataCompareDownloadFilterGroup> x = null;
            x = DataAccess.DataCompareDownloadFilterGroup.SelectForDownload(dcDownloadId);
            foreach (Entity.DataCompareDownloadFilterGroup r in x)
                SetCustomProperties(r);
            return x;
        }

        public int Save(Entity.DataCompareDownloadFilterGroup x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.DcFilterGroupId = DataAccess.DataCompareDownloadFilterGroup.Save(x);
                scope.Complete();
            }

            return x.DcFilterGroupId;
        }

        private void SetCustomProperties(Entity.DataCompareDownloadFilterGroup dfg)
        {
            DataCompareDownloadFilterDetail cWorker = new DataCompareDownloadFilterDetail();
            dfg.DcFilterDetails = cWorker.SelectForFilterGroup(dfg.DcFilterGroupId);
        }
    }
}
