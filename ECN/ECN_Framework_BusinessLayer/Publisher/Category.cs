using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Publisher
{
    [Serializable]
    public class Category
    {
        public static List<ECN_Framework_Entities.Publisher.Category> GetAll()
        {
            List<ECN_Framework_Entities.Publisher.Category> categoryList = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                categoryList = ECN_Framework_DataLayer.Publisher.Category.GetAll();
                scope.Complete();
            }

            return categoryList;
        }
    }
}
