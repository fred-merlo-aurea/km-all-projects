using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class DataCompareRecordPriceRange
    {
        public List<Entity.DataCompareRecordPriceRange> SelectAll()
        {
            List<Entity.DataCompareRecordPriceRange> x = null;
            x = DataAccess.DataCompareRecordPriceRange.SelectAll().ToList();
            return x;
        }
        public int Save(Entity.DataCompareRecordPriceRange x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.DcRecordPriceRangeId = DataAccess.DataCompareRecordPriceRange.Save(x);
                scope.Complete();
            }

            return x.DcRecordPriceRangeId;
        }
    }
}
