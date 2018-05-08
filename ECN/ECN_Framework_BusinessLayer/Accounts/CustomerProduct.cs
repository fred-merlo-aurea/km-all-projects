using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class CustomerProduct
    {
        private static readonly string CacheName = "CACHE_CUSTOMERPRODUCT_";
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CustomerProduct;

        public static bool Exists(int productDetailID, int customerID)
        {
            return ECN_Framework_DataLayer.Accounts.CustomerProduct.Exists(productDetailID, customerID);
        }

        public static List<ECN_Framework_Entities.Accounts.CustomerProduct> GetbyCustomerID(int customerID, bool Cache)
        {
            if (!ECN_Framework_Common.Functions.CacheHelper.IsCacheEnabled() && !Cache)
            {
                List<ECN_Framework_Entities.Accounts.CustomerProduct> lAction = null;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    lAction = ECN_Framework_DataLayer.Accounts.CustomerProduct.GetbyCustomerID(customerID);
                    scope.Complete();
                }

                return lAction;
            }
            else if (ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache(CacheName + customerID) == null)
            {
                List<ECN_Framework_Entities.Accounts.CustomerProduct> lAction = null;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    lAction = ECN_Framework_DataLayer.Accounts.CustomerProduct.GetbyCustomerID(customerID);
                    scope.Complete();
                }

                ECN_Framework_Common.Functions.CacheHelper.AddToCache(CacheName + customerID, lAction);
                return lAction;
            }
            else
            {
                return (List<ECN_Framework_Entities.Accounts.CustomerProduct>)ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache(CacheName + customerID);
            }
        }

        public static List<ECN_Framework_Entities.Accounts.CustomerProduct> GetbyCustomerID(int customerID, bool Cache, KMPlatform.Entity.User user)
        {
            if (!ECN_Framework_Common.Functions.CacheHelper.IsCacheEnabled() && !Cache)
            {
                List<ECN_Framework_Entities.Accounts.CustomerProduct> lAction = null;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    lAction = ECN_Framework_DataLayer.Accounts.CustomerProduct.GetbyCustomerID(customerID);
                    scope.Complete();
                }

                if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != customerID && !SecurityCheck(lAction, user))
                    throw new ECN_Framework_Common.Objects.SecurityException();

                return lAction;
            }
            else if (ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache(CacheName + customerID) == null)
            {
                List<ECN_Framework_Entities.Accounts.CustomerProduct> lAction = null;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    lAction = ECN_Framework_DataLayer.Accounts.CustomerProduct.GetbyCustomerID(customerID);
                    scope.Complete();
                }

                if (!KM.Platform.User.IsSystemAdministrator(user) && user.CustomerID != customerID && !SecurityCheck(lAction, user))
                    throw new ECN_Framework_Common.Objects.SecurityException();
             
                ECN_Framework_Common.Functions.CacheHelper.AddToCache(CacheName + customerID, lAction);
                return lAction;
            }
            else
            {
                return (List<ECN_Framework_Entities.Accounts.CustomerProduct>)ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache(CacheName + customerID);
            }
        }

        public static void Save(ECN_Framework_Entities.Accounts.CustomerProduct customerproduct, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Functions.CacheHelper.ClearCache(CacheName + customerproduct.CustomerID);
            Validate(customerproduct, user);
            ECN_Framework_DataLayer.Accounts.CustomerProduct.Save(customerproduct);
        }

        public static void Validate(ECN_Framework_Entities.Accounts.CustomerProduct customerproduct, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!KM.Platform.User.IsSystemAdministrator(user))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }
            
            if (user.IsKMStaff)
            {
                //if (!staff.FeatureUpdateFlag)
                //{
                //    throw new ECN_Framework_Common.Objects.SecurityException();
                //}
            }
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            if (customerproduct.CustomerProductID <= 0 && customerproduct.CreatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (customerproduct.CustomerProductID > 0 && customerproduct.UpdatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Update(int CPID, int userID, int customerID)
        {
            ECN_Framework_Common.Functions.CacheHelper.ClearCache(CacheName + customerID);
            ECN_Framework_DataLayer.Accounts.CustomerProduct.Update(CPID, userID);
        }

        public static void Delete(int CustomerProductID)
        {
        }

        public static void DeleteAll(int customerID)
        {
            ECN_Framework_Common.Functions.CacheHelper.ClearCache(CacheName + customerID);
        }

        private static bool SecurityCheck(List<ECN_Framework_Entities.Accounts.CustomerProduct> lcustomerProduct, KMPlatform.Entity.User user)
        {
            if (lcustomerProduct != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var securityCheck = from e in lcustomerProduct
                                        join c in custList on e.CustomerID equals c.CustomerID
                                        select new { e.CustomerProductID };

                    if (securityCheck.Count() != lcustomerProduct.Count)
                        return false;
                }
                else
                {
                    var securityCheck = from e in lcustomerProduct
                                        where e.CustomerID != user.CustomerID
                                        select new { e.CustomerProductID };

                    if (securityCheck.Any())
                        return false;
                }
            }
            return true;
        }
    }
}
