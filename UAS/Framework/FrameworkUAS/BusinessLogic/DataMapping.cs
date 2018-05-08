using System;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class DataMapping
    {
       
        public  int Save(Entity.DataMapping x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.DataMappingID = DataAccess.DataMapping.Save(x);
                scope.Complete();
            }

            return x.DataMappingID;
        }
    }
}
