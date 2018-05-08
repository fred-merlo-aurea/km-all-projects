using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class DataCompareDownloadField
    {
        public int Save(Entity.DataCompareDownloadField x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.DcDownloadFieldId = DataAccess.DataCompareDownloadField.Save(x);
                scope.Complete();
            }

            return x.DcDownloadFieldId;
        }
    }
}
