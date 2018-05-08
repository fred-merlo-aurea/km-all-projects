using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class LinkOwnerIndex
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.LinkOwnerIndex;

        private static bool HasFeature(int CustomerID)
        {
            ECN_Framework_Entities.Accounts.Customer cust = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(CustomerID, false);
            if (KMPlatform.BusinessLogic.Client.HasServiceFeature(cust.PlatformClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner))
                return true;
            return false;
        }

        public static bool Exists(int linkOwnerIndexID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.LinkOwnerIndex.Exists(linkOwnerIndexID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.LinkOwnerIndex.Exists(customerID);
                scope.Complete();
            }
            return exists;
        }

        //private static bool SecurityCheck(ECN_Framework_Entities.Communicator.LinkOwnerIndex linkOwner, KMPlatform.Entity.User user)
        //{
        //    if (linkOwner != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

        //            if (!custExists.Any())
        //                return false;
        //        }
        //        else if (linkOwner.CustomerID != user.CustomerID)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //private static bool SecurityCheck(List<ECN_Framework_Entities.Communicator.LinkOwnerIndex> linkOwnerList, KMPlatform.Entity.User user)
        //{
        //    if (linkOwnerList != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var securityCheck = from ct in linkOwnerList
        //                                join c in custList on ct.CustomerID equals c.CustomerID
        //                                select new { ct.LinkOwnerIndexID };

        //            if (securityCheck.Count() != linkOwnerList.Count)
        //                return false;
        //        }
        //        else
        //        {
        //            var securityCheck = from ct in linkOwnerList
        //                                where ct.CustomerID != user.CustomerID
        //                                select new { ct.LinkOwnerIndexID };

        //            if (securityCheck.Any())
        //                return false;
        //        }
        //    }
        //    return true;
        //}

        public static List<ECN_Framework_Entities.Communicator.LinkOwnerIndex> GetByCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            if (!HasFeature(customerID))
                throw new ECN_Framework_Common.Objects.SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };

            List<ECN_Framework_Entities.Communicator.LinkOwnerIndex> contentList = new List<ECN_Framework_Entities.Communicator.LinkOwnerIndex>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                contentList = ECN_Framework_DataLayer.Communicator.LinkOwnerIndex.GetByCustomerID(customerID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(contentList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return contentList;
        }

        public static List<ECN_Framework_Entities.Communicator.LinkOwnerIndex> GetByCustomerID_NoAccessCheck(int customerID)
        {

            List<ECN_Framework_Entities.Communicator.LinkOwnerIndex> contentList = new List<ECN_Framework_Entities.Communicator.LinkOwnerIndex>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                contentList = ECN_Framework_DataLayer.Communicator.LinkOwnerIndex.GetByCustomerID(customerID);
                scope.Complete();
            }

            return contentList;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static ECN_Framework_Entities.Communicator.LinkOwnerIndex GetByOwnerID_NoAccessCheck(int ownerID)
        {
            ECN_Framework_Entities.Communicator.LinkOwnerIndex linkOwner = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                linkOwner = ECN_Framework_DataLayer.Communicator.LinkOwnerIndex.GetByOwnerID(ownerID);
                scope.Complete();
            }

            return linkOwner;
        }

        public static ECN_Framework_Entities.Communicator.LinkOwnerIndex GetByOwnerID_NoAccessCheck_UseAmbientTransaction(int ownerID)
        {
            ECN_Framework_Entities.Communicator.LinkOwnerIndex linkOwner = null;
            using (TransactionScope scope = new TransactionScope())
            {
                linkOwner = ECN_Framework_DataLayer.Communicator.LinkOwnerIndex.GetByOwnerID(ownerID);
                scope.Complete();
            }

            return linkOwner;
        }

        public static ECN_Framework_Entities.Communicator.LinkOwnerIndex GetByOwnerID(int ownerID, KMPlatform.Entity.User user)
        {
            if (!HasFeature(user.CustomerID))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.LinkOwnerIndex linkOwner = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                linkOwner = ECN_Framework_DataLayer.Communicator.LinkOwnerIndex.GetByOwnerID(ownerID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(linkOwner, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return linkOwner;
        }

        public static void Delete(int ownerID, int customerID, KMPlatform.Entity.User user)
        {
            if (!HasFeature(customerID))
                throw new ECN_Framework_Common.Objects.SecurityException(); 
            
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(ownerID, user.CustomerID))
            {
                if (!ECN_Framework_BusinessLayer.Communicator.LinkAlias.ExistsByOwnerID(ownerID))
                {
                    //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                    GetByOwnerID(ownerID, user);

                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_DataLayer.Communicator.LinkOwnerIndex.Delete(ownerID, user.CustomerID, user.UserID);
                        scope.Complete();
                    }
                }
                else
                {
                    errorList.Add(new ECNError(Entity, Method, "Alias(s) associated with this owner"));
                    throw new ECNException(errorList);
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "LinkOwnerIndex does not exist"));
                throw new ECNException(errorList);
            }

        }

        public static void Delete(int customerID, KMPlatform.Entity.User user)
        {
            if (!HasFeature(customerID))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(user.CustomerID))
            {
                if (!ECN_Framework_BusinessLayer.Communicator.LinkAlias.Exists(customerID))
                {
                    //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                    GetByCustomerID(customerID, user);

                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_DataLayer.Communicator.LinkOwnerIndex.Delete(user.CustomerID, user.UserID);
                        scope.Complete();
                    }
                }
                else
                {
                    errorList.Add(new ECNError(Entity, Method, "Alias(s) associated with this owner"));
                    throw new ECNException(errorList);
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "LinkOwnerIndex does not exist"));
                throw new ECNException(errorList);
            }

        }

        public static void Save(ECN_Framework_Entities.Communicator.LinkOwnerIndex linkOwner, KMPlatform.Entity.User user)
        {
            if (!HasFeature(user.CustomerID))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (linkOwner.CreatedUserID == null && linkOwner.UpdatedUserID != null)
                linkOwner.CreatedUserID = linkOwner.UpdatedUserID;

            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (linkOwner.LinkOwnerIndexID > 0)
            {
                if (!Exists(linkOwner.LinkOwnerIndexID, linkOwner.CustomerID))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "LinkOwnerIndexID is invalid"));
                    throw new ECNException(errorList);
                }
            }
            Validate(linkOwner);

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(linkOwner, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                linkOwner.LinkOwnerIndexID = ECN_Framework_DataLayer.Communicator.LinkOwnerIndex.Save(linkOwner);
                scope.Complete();
            }
        }

        public static void Validate(ECN_Framework_Entities.Communicator.LinkOwnerIndex linkOwner)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (linkOwner.CustomerID == -1)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                if (linkOwner.LinkOwnerName.Trim() == string.Empty)
                    errorList.Add(new ECNError(Entity, Method, "LinkOwnerName is invalid"));
               
                if (linkOwner.IsActive == null)
                    errorList.Add(new ECNError(Entity, Method, "IsActive is invalid"));

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(linkOwner.CustomerID))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                    if (linkOwner.CreatedUserID == null || (linkOwner.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(linkOwner.CreatedUserID.Value, false))))
                    {
                        if (linkOwner.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(linkOwner.CreatedUserID.Value, linkOwner.CustomerID))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }

                    if (linkOwner.LinkOwnerIndexID > 0 && (linkOwner.UpdatedUserID == null || (linkOwner.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(linkOwner.UpdatedUserID.Value, false)))))
                    {
                        if (linkOwner.LinkOwnerIndexID > 0 && (linkOwner.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(linkOwner.UpdatedUserID.Value, linkOwner.CustomerID)))
                            errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }
                    scope.Complete();
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }   
        }
    }
}
