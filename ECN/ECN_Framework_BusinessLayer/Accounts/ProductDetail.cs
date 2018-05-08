using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class ProductDetail
    {
        private static readonly string CacheName = "CACHE_PRODUCTDETAIL";

        public static List<ECN_Framework_Entities.Accounts.ProductDetail> GetAll()
        {
            if (!ECN_Framework_Common.Functions.CacheHelper.IsCacheEnabled())
            {
                List<ECN_Framework_Entities.Accounts.ProductDetail> lProductDetail = null;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    lProductDetail = ECN_Framework_DataLayer.Accounts.ProductDetail.GetAll();
                    scope.Complete();
                }
                return lProductDetail;
            }
            else if (ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache(CacheName) == null)
            {
                List<ECN_Framework_Entities.Accounts.ProductDetail> lProductDetail = null;
                
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    lProductDetail = ECN_Framework_DataLayer.Accounts.ProductDetail.GetAll();
                    scope.Complete();
                }

                ECN_Framework_Common.Functions.CacheHelper.AddToCache(CacheName, lProductDetail);
                return lProductDetail;
            }
            else
            {
                return (List<ECN_Framework_Entities.Accounts.ProductDetail>)ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache(CacheName);
            }
        }
    }
}
