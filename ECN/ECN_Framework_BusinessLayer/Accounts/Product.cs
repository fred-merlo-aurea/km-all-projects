using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;
using AccountsEntity = ECN_Framework_Entities.Accounts;
using AccountsBLL = ECN_Framework_BusinessLayer.Accounts;
using CommunicatorEntity = ECN_Framework_Entities.Communicator;
using CommunicatorBLL = ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Entities.Accounts;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class Product
    {
        private static readonly string CacheName = "CACHE_PRODUCT";

        public static List<ECN_Framework_Entities.Accounts.Product> GetAll()
        {
            if (!ECN_Framework_Common.Functions.CacheHelper.IsCacheEnabled())
            {
                List<ECN_Framework_Entities.Accounts.Product> lProduct = null;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    lProduct = ECN_Framework_DataLayer.Accounts.Product.GetAll();
                    scope.Complete();
                }
                return lProduct;
            }
            else if (ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache(CacheName) == null)
            {
                List<ECN_Framework_Entities.Accounts.Product> lProduct = null;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    lProduct = ECN_Framework_DataLayer.Accounts.Product.GetAll();
                    scope.Complete();
                }

                ECN_Framework_Common.Functions.CacheHelper.AddToCache(CacheName, lProduct);
                return lProduct;
            }
            else
            {
                return (List<ECN_Framework_Entities.Accounts.Product>)ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache(CacheName);
            }
        }
    }
}
