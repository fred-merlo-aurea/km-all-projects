using System;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class FilterExportField
    {
        public int Save(Entity.FilterExportField x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.FilterExportFieldId = DataAccess.FilterExportField.Save(x);
                scope.Complete();
            }

            return x.FilterExportFieldId;
        }
    }
}
