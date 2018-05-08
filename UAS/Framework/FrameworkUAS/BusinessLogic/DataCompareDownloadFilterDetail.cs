using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class DataCompareDownloadFilterDetail
    {
        public List<Entity.DataCompareDownloadFilterDetail> SelectForFilterGroup(int dcFilterGroupID)
        {
            List<Entity.DataCompareDownloadFilterDetail> retItem = null;
            retItem = DataAccess.DataCompareDownloadFilterDetail.SelectForFilterGroup(dcFilterGroupID);

            return retItem;
        }

        public int Save(Entity.DataCompareDownloadFilterDetail x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.DcFilterDetailID = DataAccess.DataCompareDownloadFilterDetail.Save(x);
                scope.Complete();
            }

            return x.DcFilterDetailID;
        }
    }
}
