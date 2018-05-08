using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;

namespace FrameworkUAS.BusinessLogic
{
    public class DataCompareDownloadDetail
    {
        public void Save(int dcDownloadId, string subscriptionIdsXml)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.DataCompareDownloadDetail.Save(dcDownloadId, subscriptionIdsXml);
                scope.Complete();
            }

            return;
        }
    }
}
